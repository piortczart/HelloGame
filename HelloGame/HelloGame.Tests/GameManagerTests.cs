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

namespace HelloGame.Tests
{
    [TestClass]
    public class GameManagerTests
    {
        [TestMethod]
        public void GameManager_ShipPlanetCollision()
        {
            // The time is paused now.
            IResolutionRoot ninject =
                new StandardKernel(new HelloGameCommonNinjectBindings(new GeneralSettings
                {
                    SpawnAi = false,
                    CollisionTolerance = 0,
                    GravityFactor = 0.01m,
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
            var injections = ninject.Get<ThingBaseInjections>();

            TimeSpan timeToLive = ThingSettings.GetLazerBeamSettings(null).TimeToLive;
            TimeSpan halfTimeToLive = TimeSpan.FromMilliseconds(timeToLive.TotalMilliseconds/2);

            // Spawn a lazzzer close to the ship.
            var lazer = new LazerBeamPew(injections, null, -1);
            lazer.Spawn(new Point(15, 15));
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
            var aiShip = thingFactory.GetRandomAiShip(new Point(40, 15), "AI", null, null, -1);
            gameManager.ModelManager.AddThing(aiShip);

            // Create a player.
            PlayerShipOther playerShip = thingFactory.GetPlayerShip(
                new Point(10, 15), "PLAYUR", ClanEnum.Integrations, -2);
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
            timeSource.SkipTime(playerShip.Settingz.LazerLimit.Add(TimeSpan.FromMilliseconds(10)));
            gameManager.ModelManager.SingleModelUpdate();

            // TODO: what if a thing is spawned between two long model updates? (player shoots)
            // Physics will be calculated as if it was there since the last update!

            bool isShot = playerShip.PewPew();
            Assert.IsTrue(isShot);
            gameManager.ModelManager.SingleModelUpdate();

            // Find the lazer.
            ThingBase lazer =
                gameManager.ModelManager.ThingsThreadSafe.GetThingsReadOnly().Single(t => t is LazerBeamPew);

            decimal distanceStart = lazer.DistanceTo(aiShip);
            decimal distance;
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