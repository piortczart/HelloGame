using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject.Syntax;
using HelloGame.Common;
using Ninject;
using HelloGame.Common.Model;
using HelloGame.Common.Logging;
using System.Drawing;
using System;
using HelloGame.Common.Extensions;
using HelloGame.Common.Model.GameObjects;
using HelloGame.Common.Model.GameObjects.Ships;
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
                new StandardKernel(new HelloGameCommonNinjectBindings(GeneralSettings.Gameplay, true, true));

            var coordinator = ninject.Get<GameThingCoordinator>();
            var gameManager = ninject.Get<GameManager>();
            var timeSource = ninject.Get<TimeSource>();
            var injections = ninject.Get<ThingBaseInjections>();

            PlayerShipOther player = gameManager.AddPlayer("Plajur", ClanEnum.Integrations);

            // TODO: in progress..
            Assert.Fail("In progress.");

            //gameManager.AddBigThing();

            //timeSource.SkipTime(TimeSpan.FromMilliseconds(500));
            //gameManager.ModelManager.SingleModelUpdate();
            //Assert.IsFalse(lazer.IsDestroyed);
            //Assert.IsFalse(lazer.IsTimeToElapse);
            //Assert.IsNotNull(gameManager.ModelManager.ThingsThreadSafe.GetById(lazer.Id));

            //// Now a bot over 1s sould have passed. Lazer should be despawned.
            //timeSource.SkipTime(TimeSpan.FromMilliseconds(501));
            //gameManager.ModelManager.SingleModelUpdate();
            //Assert.IsTrue(lazer.IsTimeToElapse);
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
                new StandardKernel(new HelloGameCommonNinjectBindings(GeneralSettings.Gameplay, true, true));
            var loggerFactory = new LoggerFactory("");
            ILogger logger = loggerFactory.CreateLogger(GetType());
            var gameManager = ninject.Get<GameManager>();
            var thingFactory = ninject.Get<ThingFactory>();
            var timeSource = ninject.Get<TimeSource>();
            var injections = ninject.Get<ThingBaseInjections>();

            // Create an AI Ship.
            var aiShip = thingFactory.GetRandomAiShip(new Point(10, 10), "AI", null, null, 1);
            gameManager.ModelManager.AddThing(aiShip);

            // Create a player.
            var playerShip = thingFactory.GetPlayerShip(new Point(40, 10), "PLAYUR",
                Common.Model.GameObjects.Ships.ClanEnum.Integrations, 2);
            gameManager.ModelManager.AddThing(playerShip);

            // Make 1 model update.
            timeSource.SkipTime(TimeSpan.FromMilliseconds(10));
            gameManager.ModelManager.SingleModelUpdate();
            // Sanity check - Make sure we have the AI ship.
            ThingBase ship1 = gameManager.ModelManager.ThingsThreadSafe.GetById(aiShip.Id);
            Assert.AreEqual(aiShip, ship1);
            Assert.IsFalse(aiShip.IsDestroyed);

            // Spawn a lazzzer close to the ship.
            var lazer = new LazerBeamPew(injections, playerShip, -1);
            lazer.Spawn(new Point(15, 15));
            gameManager.ModelManager.AddThing(lazer);

            timeSource.SkipTime(TimeSpan.FromMilliseconds(10));
            gameManager.ModelManager.SingleModelUpdate();

            // Get the ship again.
            Assert.IsTrue(aiShip.IsDestroyed);
        }
    }
}