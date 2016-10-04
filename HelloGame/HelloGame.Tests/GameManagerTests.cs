using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject.Syntax;
using HelloGame.Common;
using Ninject;
using HelloGame.Common.Model;
using HelloGame.Common.Logging;
using System.Drawing;
using System;
using HelloGame.Common.Model.GameObjects;

namespace HelloGame.Tests
{
    [TestClass]
    public class GameManagerTests
    {
        [TestMethod]
        public void GameManager_LaserDespawn()
        {
            // The time is paused now.
            IResolutionRoot ninject = new StandardKernel(new HelloGameCommonNinjectBindings(GeneralSettings.Gameplay, true, true));
            var loggerFactory = new LoggerFactory("");
            ILogger logger = loggerFactory.CreateLogger(GetType());
            var gameManager = ninject.Get<GameManager>();
            var gameCoordinator = ninject.Get<GameThingCoordinator>();
            var thingFactory = ninject.Get<ThingFactory>();
            var timeSource = ninject.Get<TimeSource>();
            var injections = ninject.Get<ThingBaseInjections>();

            // Spawn a lazzzer close to the ship.
            var lazer = new LazerBeamPew(injections, null, -1);
            lazer.Spawn(new Point(15, 15));
            gameManager.ModelManager.AddOrUpdateThing(lazer);

            timeSource.SkipTime(TimeSpan.FromMilliseconds(500));
            gameManager.ModelManager.SingleModelUpdate();
            Assert.IsFalse(lazer.IsDestroyed);
            Assert.IsFalse(lazer.IsTimeToElapse);
            Assert.IsNotNull(gameManager.ModelManager.Things.GetById(lazer.Id));

            // Now a bot over 1s sould have passed. Lazer should be despawned.
            timeSource.SkipTime(TimeSpan.FromMilliseconds(501));
            gameManager.ModelManager.SingleModelUpdate();
            Assert.IsTrue(lazer.IsTimeToElapse);
        }

        [TestMethod]
        public void GameManager_CloseLaserVsAiShip()
        {
            // The time is paused now.
            IResolutionRoot ninject = new StandardKernel(new HelloGameCommonNinjectBindings(GeneralSettings.Gameplay, true, true));
            var loggerFactory = new LoggerFactory("");
            ILogger logger = loggerFactory.CreateLogger(GetType());
            var gameManager = ninject.Get<GameManager>();
            var gameCoordinator = ninject.Get<GameThingCoordinator>();
            var thingFactory = ninject.Get<ThingFactory>();
            var timeSource = ninject.Get<TimeSource>();
            var injections = ninject.Get<ThingBaseInjections>();

            // Create an AI Ship.
            var aiShip = thingFactory.GetAiShip(10, new Point(10, 10), "AI", 1);
            gameManager.ModelManager.AddOrUpdateThing(aiShip);

            // Create a player.
            var playerShip = thingFactory.GetPlayerShip(10, new Point(10, 10), "PLAYUR", 2);
            gameManager.ModelManager.AddOrUpdateThing(aiShip);

            // Make 1 model update.
            timeSource.SkipTime(TimeSpan.FromMilliseconds(10));
            gameManager.ModelManager.SingleModelUpdate();
            // Sanity check - Make sure we have the AI ship.
            ThingBase ship1 = gameManager.ModelManager.Things.GetById(aiShip.Id);
            Assert.AreEqual(aiShip, ship1);
            Assert.IsFalse(aiShip.IsDestroyed);

            // Spawn a lazzzer close to the ship.
            var lazer = new LazerBeamPew(injections, playerShip, -1);
            lazer.Spawn(new Point(15, 15));
            gameManager.ModelManager.AddOrUpdateThing(lazer);

            timeSource.SkipTime(TimeSpan.FromMilliseconds(10));
            gameManager.ModelManager.SingleModelUpdate();

            // Get the ship again.
            Assert.IsTrue(aiShip.IsDestroyed);
        }
    }
}
