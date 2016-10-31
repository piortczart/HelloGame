using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject.Syntax;
using HelloGame.Common;
using Ninject;
using HelloGame.Common.Model;
using System.Drawing;
using System;
using System.Linq;
using HelloGame.Common.Extensions;
using HelloGame.Common.Model.GameObjects;
using HelloGame.Common.Model.GameObjects.Ships;
using HelloGame.Common.Physicsish;
using HelloGame.Common.Settings;
using HelloGame.Common.TimeStuffs;
using HelloGame.Client;
using HelloGame.Common.Network;
using NSubstitute;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Configuration;
using HelloGame.Common.MathStuff;

namespace HelloGame.Tests
{
    [TestClass]
    public class GameManagerTests
    {
        /// <summary>
        /// Spawn a player and a bomb belonging to him. Pretend enough time passed for the bomb to despawn. Check if it did.
        /// </summary>
        [TestMethod]
        public void GameManager_BombGoesBoom()
        {
            // The time is paused now. We will do a step by step in the test.
            // This is a client-side test.
            IResolutionRoot ninject =
                new StandardKernel(new HelloGameCommonNinjectBindings(GeneralSettings.Gameplay, true, true));

            var gameManager = ninject.Get<GameManager>();
            var timeSource = ninject.Get<TimeSource>();
            var thingFactory = ninject.Get<ThingFactory>();

            // Spawn a player.
            PlayerShipOther playerShip = thingFactory.GetPlayerShip(
                new Point(10, 15), "PLAYUR", ClanEnum.Integrations);
            gameManager.ModelManager.AddThing(playerShip);

            // Expected bomb's time to live.
            TimeSpan timeToLive = ThingSettings.GetBombSettings(null).TimeToLive;

            // Spawn a bomb.
            Bomb bomb = thingFactory.GetBomb(null, ThingAdditionalInfo.GetNew(playerShip));
            gameManager.ModelManager.AddThing(bomb);

            gameManager.ModelManager.SingleModelUpdate();
            Assert.IsFalse(bomb.IsTimeToElapse);
            Assert.IsNotNull(gameManager.ModelManager.ThingsThreadSafe.GetById(bomb.Id));

            // Pretend enough time has passed for the bomb to despawn.
            timeSource.SkipTime(timeToLive.Add(TimeSpan.FromMilliseconds(10)));
            gameManager.ModelManager.SingleModelUpdate();
            Assert.IsTrue(bomb.IsTimeToElapse);
            Assert.IsNull(gameManager.ModelManager.ThingsThreadSafe.GetById(bomb.Id));
        }


        [TestMethod]
        public async Task GameManager_Client_AiShootsPlayer()
        {
            ConfigurationManager.AppSettings["PropagateFrequencyServer"] = "00:00:00.050";
            ConfigurationManager.AppSettings["PropagateFrequencyClient"] = "00:00:00.050";
            ConfigurationManager.AppSettings["ServerPortNumber"] = "4450";

            var settings = new GeneralSettings
            {
                SpawnAi = false,
                CollisionTolerance = 5, // So we will get a hit initially even though the ship is moving a bit...
                GravityFactor = 0.01f,
            };
            IResolutionRoot serverNinject =
                new StandardKernel(new HelloGameCommonNinjectBindings(settings, true, true));

            IKernel ninject = new StandardKernel(
                new HelloGameCommonNinjectBindings(settings, false, true),
                new HelloGameClientNinjectBindings(serverNinject));

            var tran = Substitute.For<IMessageTransciever>();
            ninject.Rebind<IMessageTransciever>().ToConstant(tran);

            var serverThingFactory = serverNinject.Get<ThingFactory>();
            var originalPosition = new Point(10, 10);
            PlayerShip playerShip = serverThingFactory.GetPlayerShip(originalPosition, "hula", ClanEnum.Support);
            // Pretend it's already moving. Going up...
            playerShip.Physics.Interia = Vector2D.GetFromAngleLength((float) Math.PI/2, 20);
            var things = new List<ThingBase>
            {
                playerShip
            };

            // First message - the player only.
            Task<NetworkMessage> first = Task.FromResult(new NetworkMessage
            {
                Type = NetworkMessageType.UpdateStuff,
                Payload = things.Select(t => new ThingDescription(t, true)).SerializeJson()
            });

            // Second message - the AI and a lazer going to the player.
            AiShip ai = serverThingFactory.GetRandomAiShip(new Point(100, 40), "hyhy");
            ai.PewPew(0);
            LazerBeamPew lazer = serverThingFactory.GetLazerBeam(null, ThingAdditionalInfo.GetNew(ai));
            things.Add(ai);
            things.Add(lazer);
            Task<NetworkMessage> second = Task.FromResult(new NetworkMessage
            {
                Type = NetworkMessageType.UpdateStuff,
                Payload = things.Select(t => new ThingDescription(t, true)).SerializeJson()
            });

            // Return the messages one by one.
            tran.GetAsync(null).ReturnsForAnyArgs(first, second);

            var gameManager = ninject.Get<GameManager>();
            var modelManager = gameManager.ModelManager;
            var timeSource = ninject.Get<TimeSource>();
            var clientNetwork = ninject.Get<ClientNetwork>();

            TimeSpan step = TimeSpan.FromMilliseconds(20);

            timeSource.SkipTime(step);
            modelManager.SingleModelUpdate();

            var stuff = modelManager.ThingsThreadSafe.GetThingsReadOnly();
            Assert.AreEqual(0, stuff.Count);

            // This should parse the "first" message - the player is spawning.
            await clientNetwork.WaitAndParseMessageTest();

            stuff = modelManager.ThingsThreadSafe.GetThingsReadOnly();
            Assert.AreEqual(1, stuff.Count);

            ThingBase shipNow = stuff.Single();

            Assert.AreEqual(originalPosition, shipNow.Physics.PositionPoint);

            // Make an update of the model.
            timeSource.SkipTime(step);
            modelManager.SingleModelUpdate();

            // Make sure it has moved (up, so Y axis only)
            // If a small iteria force is simulated and a small time is passed, this might still be the same.
            // In this case increase the step or the initial force.
            Assert.AreEqual(originalPosition.X, shipNow.Physics.PositionPoint.X);
            Assert.AreNotEqual(originalPosition.Y, shipNow.Physics.PositionPoint.Y);

            // This should parse the "second" message. The player is hit by a lazer.
            await clientNetwork.WaitAndParseMessageTest();

            stuff = modelManager.ThingsThreadSafe.GetThingsReadOnly();
            Assert.AreEqual(3, stuff.Count);

            // Make an update of the model. Lazer destroys the ship. Lazer gets destroyed.
            timeSource.SkipTime(step);
            modelManager.SingleModelUpdate();
            stuff = modelManager.ThingsThreadSafe.GetThingsReadOnly();
            shipNow = stuff.First();
            LazerBeamPew lazerNow = GetNthAs<ThingBase, LazerBeamPew>(stuff, 2);

            Assert.IsTrue(shipNow.IsDestroyed);
            Assert.IsTrue(lazerNow.IsDestroyed);

            // Make another update. Check what's up.
            timeSource.SkipTime(step);
            modelManager.SingleModelUpdate();
            stuff = modelManager.ThingsThreadSafe.GetThingsReadOnly();
            // Lazer should have been removed.
            Assert.AreEqual(2, stuff.Count);
            shipNow = stuff.First();

            Assert.IsTrue(shipNow.IsDestroyed);
        }

        private static G GetNthAs<T, G>(IReadOnlyCollection<T> list, int position)
        {
            return (G) Convert.ChangeType(list.Skip(position).First(), typeof(G));
        }

        [TestMethod]
        public void GameManager_Server_ShipPlanetCollision()
        {
            // The time is paused now.
            IResolutionRoot ninject =
                new StandardKernel(new HelloGameCommonNinjectBindings(new GeneralSettings
                {
                    SpawnAi = false,
                    CollisionTolerance = 0,
                    GravityFactor = 0.01f,
                }, true, true));

            var gameManager = ninject.Get<GameManager>();
            var thingFactory = ninject.Get<ThingFactory>();
            var modelManager = gameManager.ModelManager;
            var timeSource = ninject.Get<TimeSource>();

            var playerLocation = new Point(1, 1);
            PlayerShipOther newShip = thingFactory.GetPlayerShip(playerLocation, "HA", ClanEnum.Integrations);
            modelManager.AddThing(newShip);

            var massLocation = new Point(100, 100);
            var massSize = 40;
            BigMass bigMass = thingFactory.GetBigMass(massSize, massLocation, Color.DarkRed);
            modelManager.AddThing(bigMass);

            TimeSpan step = TimeSpan.FromMilliseconds(20);

            do
            {
                timeSource.SkipTime(step);
                modelManager.SingleModelUpdate();
            } while (timeSource.ElapsedSinceStart < TimeSpan.FromSeconds(20) && !newShip.IsDestroyed);

            if (!newShip.IsDestroyed)
            {
                Assert.Fail("Death took too long!");
            }

            // Should not have elapsed yet.
            Assert.IsFalse(newShip.IsTimeToElapse);

            Position shipPos = newShip.Physics.Position.Copy();

            TimeSpan expectedTimeToDespawn = newShip.ShipSettings.DespawnTime.Add(step);
            TimeSpan currentTime = timeSource.ElapsedSinceStart;

            while (timeSource.ElapsedSinceStart - currentTime < expectedTimeToDespawn)
            {
                timeSource.SkipTime(step);
                modelManager.SingleModelUpdate();

                var currentPos = newShip.Physics.Position;
                Assert.AreEqual(shipPos.X, currentPos.X);
            }

            TimeSpan timeTakenToDespawn = timeSource.ElapsedSinceStart - currentTime;
            Assert.IsTrue(newShip.IsTimeToElapse);
            Assert.IsTrue(timeTakenToDespawn.TotalMilliseconds >= expectedTimeToDespawn.TotalMilliseconds);
            Assert.IsTrue(timeTakenToDespawn.TotalMilliseconds <
                          expectedTimeToDespawn.TotalMilliseconds + step.TotalMilliseconds*2);
        }

        [TestMethod]
        public void GameManager_LaserDespawn()
        {
            // The time is paused now.
            IResolutionRoot ninject =
                new StandardKernel(new HelloGameCommonNinjectBindings(GeneralSettings.Gameplay, true, true));

            var gameManager = ninject.Get<GameManager>();
            var timeSource = ninject.Get<TimeSource>();
            var thingFactory = ninject.Get<ThingFactory>();

            TimeSpan timeToLive = ThingSettings.GetLazerBeamSettings(null).TimeToLive;
            TimeSpan halfTimeToLive = TimeSpan.FromMilliseconds(timeToLive.TotalMilliseconds/2);

            PlayerShipOther player = thingFactory.GetPlayerShip(Point.Empty, "hula", ClanEnum.Integrations);
            gameManager.ModelManager.AddThing(player);
            LazerBeamPew lazer = thingFactory.GetLazerBeam(null, ThingAdditionalInfo.GetNew(player));
            gameManager.ModelManager.AddThing(lazer);

            timeSource.SkipTime(halfTimeToLive);
            gameManager.ModelManager.SingleModelUpdate();
            Assert.IsFalse(lazer.IsDestroyed);
            Assert.IsFalse(lazer.IsTimeToElapse);
            Assert.IsNotNull(gameManager.ModelManager.ThingsThreadSafe.GetById(lazer.Id));

            // Now about over 1s sould have passed. Lazer should be despawned.
            timeSource.SkipTime(halfTimeToLive.Add(TimeSpan.FromMilliseconds(50)));
            gameManager.ModelManager.SingleModelUpdate();
            Assert.IsTrue(lazer.IsTimeToElapse);
        }

        [TestMethod]
        public void GameManager_CloseLaserVsAiShip()
        {
            // The time is paused now.
            IResolutionRoot ninject =
                new StandardKernel(new HelloGameCommonNinjectBindings(new GeneralSettings
                {
                    SpawnAi = false,
                    IsAiHostile = false,
                    CollisionTolerance = 0
                }, true, true));
            var gameManager = ninject.Get<GameManager>();
            var thingFactory = ninject.Get<ThingFactory>();
            var timeSource = ninject.Get<TimeSource>();

            // Create an AI Ship.
            var aiShip = thingFactory.GetRandomAiShip(new Point(40, 15), "AI");
            gameManager.ModelManager.AddThing(aiShip);

            // Create a player.
            PlayerShipOther playerShip = thingFactory.GetPlayerShip(new Point(10, 15), "PLAYUR", ClanEnum.Integrations);
            gameManager.ModelManager.AddThing(playerShip);

            //
            //  Player spawned to the left, facing AI.
            //  (PL)-   (AI)-
            //  AI is not hostile, should stay put.
            //

            TimeSpan step = TimeSpan.FromMilliseconds(10);

            // Make 1 model update.
            timeSource.SkipTime(step);
            gameManager.ModelManager.SingleModelUpdate();
            // Sanity check - Make sure we have the AI ship.
            ThingBase ship1 = gameManager.ModelManager.ThingsThreadSafe.GetById(aiShip.Id);
            Assert.AreEqual(aiShip, ship1);
            Assert.IsFalse(aiShip.IsDestroyed);

            // Make the player shoot a lazer.
            // There is a limiter, make sure enough time has passed.
            TimeSpan toSkip = playerShip.Settingz.GetWeaponFrequency(WeaponType.Lazer)
                .Add(TimeSpan.FromMilliseconds(10));
            timeSource.SkipTime(toSkip);
            gameManager.ModelManager.SingleModelUpdate();

            // TODO: what if a thing is spawned between two long model updates? (player shoots)
            // Physics will be calculated as if it was there since the last update!

            bool isShot = playerShip.PewPew(0);
            Assert.IsTrue(isShot);
            gameManager.ModelManager.SingleModelUpdate();

            // Find the lazer.
            ThingBase lazer =
                gameManager.ModelManager.ThingsThreadSafe.GetThingsReadOnly().Single(t => t is LazerBeamPew);

            float distance;
            int i = 0;
            do
            {
                timeSource.SkipTime(TimeSpan.FromMilliseconds(10));
                gameManager.ModelManager.SingleModelUpdate();
                distance = lazer.DistanceTo(playerShip);
            } while (distance > 0 && i++ < 10);

            // Get the ship again.
            Assert.IsTrue(aiShip.IsDestroyed);
        }
    }
}