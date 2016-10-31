using System;
using System.Drawing;
using DeepEqual.Syntax;
using HelloGame.Common;
using HelloGame.Common.Extensions;
using HelloGame.Common.Model;
using HelloGame.Common.Model.GameObjects;
using HelloGame.Common.Model.GameObjects.Ships;
using HelloGame.Common.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Ninject.Syntax;

namespace HelloGame.Tests
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void LazerSerializatoion()
        {
            IResolutionRoot serverNinject =
                new StandardKernel(new HelloGameCommonNinjectBindings(GeneralSettings.Gameplay, HelloGameCommonBindingsType.Server, true));

            var factory = serverNinject.Get<ThingFactory>();
            var manager = serverNinject.Get<GameManager>();

            // Create a new lazer.
            AiShip ai = manager.AddAiShipRandom("hi");
            LazerBeamPew lazer = factory.GetLazerBeam(null, ThingAdditionalInfo.GetNew(ai),
                new ElapsingThingSettings
                {
                    TimeToLive = TimeSpan.FromMilliseconds(10),
                    SpawnedAt = TimeSpan.FromMilliseconds(1)
                });

            Assert.AreEqual(TimeSpan.FromMilliseconds(10), lazer.TimeToLive);
            Assert.AreEqual(false, lazer.IsTimeToElapse);

            // Act
            // Serialize and deserialize.
            var serializedLazer = new ThingDescription(lazer, false);
            LazerBeamPew sameLazer = factory.CreateFromDescription(serializedLazer) as LazerBeamPew;

            // Assert
            // Make sure they are equal (the server will override the ID)
            lazer.WithDeepEqual(sameLazer).IgnoreSourceProperty(l => l.Id).Assert();
        }

        [TestMethod]
        public void PlayerShipSerializatoion()
        {
            IResolutionRoot serverNinject =
                new StandardKernel(new HelloGameCommonNinjectBindings(GeneralSettings.Gameplay, HelloGameCommonBindingsType.Server, true));

            var factory = serverNinject.Get<ThingFactory>();

            var weapons = new Weapons
            {
                Main = new Weapon {WeaponType = WeaponType.Bomb, WeaponLevel = 11},
                Secondary = new Weapon {WeaponType = WeaponType.Lazer, WeaponLevel = -1}
            };

            // Create a new player ship.
            PlayerShipOther ship = factory.GetPlayerShip(Point.Empty, "hi", ClanEnum.Integrations, null,
                new ThingAdditionalInfo
                {
                    CreatorId = null,
                    Score = 15,
                    WeaponsSerialized = weapons.SerializeJson()
                },
                new ElapsingThingSettings
                {
                    TimeToLive = TimeSpan.FromMilliseconds(10),
                    SpawnedAt = TimeSpan.FromMilliseconds(1),
                });

            Assert.AreEqual(TimeSpan.FromMilliseconds(10), ship.TimeToLive);
            Assert.AreEqual(false, ship.IsTimeToElapse);
            Assert.AreEqual(15, ship.Score);
            Assert.IsNotNull(ship.Weapons);

            // Act
            // Serialize and deserialize.
            var serialized = new ThingDescription(ship, false);
            PlayerShipOther sameShip = factory.CreateFromDescription(serialized) as PlayerShipOther;

            // Assert
            // Make sure they are equal (the server will override the ID)
            ship.WithDeepEqual(sameShip).IgnoreSourceProperty(l => l.Id).Assert();
        }

        [TestMethod]
        public void BombSerializatoion()
        {
            IResolutionRoot serverNinject =
                new StandardKernel(new HelloGameCommonNinjectBindings(GeneralSettings.Gameplay, HelloGameCommonBindingsType.Server, true));

            var factory = serverNinject.Get<ThingFactory>();
            var manager = serverNinject.Get<GameManager>();

            // Create a new bomb.
            AiShip ai = manager.AddAiShipRandom("hi");
            Bomb bomb = factory.GetBomb(null, ThingAdditionalInfo.GetNew(ai),
                new ElapsingThingSettings
                {
                    TimeToLive = TimeSpan.FromMilliseconds(10),
                    SpawnedAt = TimeSpan.FromMilliseconds(1)
                });

            Assert.AreEqual(TimeSpan.FromMilliseconds(10), bomb.TimeToLive);
            Assert.AreEqual(false, bomb.IsTimeToElapse);

            // Act
            // Serialize and deserialize.
            var serializedLazer = new ThingDescription(bomb, false);
            Bomb sameBomb = factory.CreateFromDescription(serializedLazer) as Bomb;

            // Assert
            // Make sure they are equal (the server will override the ID)
            bomb.WithDeepEqual(sameBomb).IgnoreSourceProperty(l => l.Id).Assert();
        }
    }
}