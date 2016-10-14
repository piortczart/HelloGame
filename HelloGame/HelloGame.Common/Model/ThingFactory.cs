using System;
using System.Drawing;
using HelloGame.Common.Extensions;
using HelloGame.Common.Logging;
using HelloGame.Common.MathStuff;
using HelloGame.Common.Model.GameObjects;
using HelloGame.Common.Model.GameObjects.Ships;
using HelloGame.Common.Settings;

namespace HelloGame.Common.Model
{
    public class ThingFactory
    {
        private readonly bool _isServer;
        private readonly GameThingCoordinator _coordinator;
        private readonly ThingBaseInjections _thingInjections;

        public ThingFactory(bool isServer, ThingBaseInjections injections, GameThingCoordinator gameManager,
            ILoggerFactory loggerFactory)
        {
            _isServer = isServer;
            _coordinator = gameManager;
            loggerFactory.CreateLogger(GetType());
            _thingInjections = injections;
        }

        /// <summary>
        /// Creates a full replica of a Thing from it's description. Includes physics.
        /// </summary>
        public ThingBase CreateFromDescription(ThingDescription description)
        {
            ThingBase result;

            switch (description.Type)
            {
                case "PlayerShipAny":
                {
                    string name = (string) description.ConstructParams[0];
                    ThingAdditionalInfo extras =
                        ((string) description.ConstructParams[1]).DeSerializeJson<ThingAdditionalInfo>();
                    extras.SetCreator(_coordinator);
                    ClanEnum clan = (ClanEnum) Convert.ToInt32(description.ConstructParams[2]);
                    ElapsingThingSettings elapsing =
                        description.ConstructParams.Length > 3
                            ? ((string) description.ConstructParams[3]).DeSerializeJson<ElapsingThingSettings>()
                            : null;

                    result = GetPlayerShip(description.AlmostPhysics.PositionPoint, name, clan, description.Id, extras,
                        elapsing);
                    break;
                }
                case "PlayerShipMovable":
                {
                    string name = (string) description.ConstructParams[0];
                    ThingAdditionalInfo extras =
                        ((string) description.ConstructParams[1]).DeSerializeJson<ThingAdditionalInfo>();
                    extras.SetCreator(_coordinator);
                    ClanEnum clan = (ClanEnum) (int) (long) description.ConstructParams[2];
                    ElapsingThingSettings elapsing =
                        description.ConstructParams.Length > 3
                            ? ((string) description.ConstructParams[3]).DeSerializeJson<ElapsingThingSettings>()
                            : null;

                    result = GetPlayerShipMovable(description.AlmostPhysics.PositionPoint, name, clan, description.Id,
                        extras, elapsing);
                    break;
                }
                case "AiShip":
                {
                    string name = (string) description.ConstructParams[0];
                    ThingAdditionalInfo extras =
                        ((string) description.ConstructParams[1]).DeSerializeJson<ThingAdditionalInfo>();
                    extras.SetCreator(_coordinator);
                    AiType aiType = (AiType) (int) (long) description.ConstructParams[2];
                    ShipSettingType shipSettingType = (ShipSettingType) (int) (long) description.ConstructParams[3];
                    ElapsingThingSettings elapsing =
                        description.ConstructParams.Length > 4
                            ? ((string) description.ConstructParams[4]).DeSerializeJson<ElapsingThingSettings>()
                            : null;

                    result = GetRandomAiShip(description.AlmostPhysics.PositionPoint, name, aiType, shipSettingType,
                        description.Id, extras, elapsing);
                    break;
                }
                case "BigMass":
                {
                    int size = (int) (double) description.ConstructParams[0];
                    Color color = ((string) description.ConstructParams[1]).DeSerializeJson<Color>();
                    ThingAdditionalInfo extras =
                        ((string) description.ConstructParams[2]).DeSerializeJson<ThingAdditionalInfo>();
                    extras.SetCreator(_coordinator);
                    ElapsingThingSettings elapsing =
                        description.ConstructParams.Length > 3
                            ? ((string) description.ConstructParams[3]).DeSerializeJson<ElapsingThingSettings>()
                            : null;

                    result = GetBigMass(size, description.AlmostPhysics.PositionPoint, color, description.Id,
                        extras, elapsing);
                    break;
                }
                case "LazerBeamPew":
                {
                    // This can be spawned by the player. Server should give it a proper id.
                    // The id comming from the player can be bad.
                    // TODO: This can be spawned by the server too!!
                    int? id = _isServer ? (int?) null : description.Id;
                    ThingAdditionalInfo extras =
                        ((string) description.ConstructParams[0]).DeSerializeJson<ThingAdditionalInfo>();
                    extras.SetCreator(_coordinator);
                    ElapsingThingSettings elapsing =
                        description.ConstructParams.Length > 1
                            ? ((string) description.ConstructParams[1]).DeSerializeJson<ElapsingThingSettings>()
                            : null;

                    result = GetLazerBeam(id, extras, elapsing);
                    break;
                }
                case "Bomb":
                {
                    int? id = _isServer ? (int?) null : description.Id;
                    ThingAdditionalInfo extras =
                        ((string) description.ConstructParams[0]).DeSerializeJson<ThingAdditionalInfo>();
                    extras.SetCreator(_coordinator);
                    ElapsingThingSettings elapsing =
                        description.ConstructParams.Length > 1
                            ? ((string) description.ConstructParams[1]).DeSerializeJson<ElapsingThingSettings>()
                            : null;

                    result = GetBomb(id, extras, elapsing);
                    break;
                }
                default:
                {
                    throw new NotImplementedException($"Cannot spawn {description.Type}");
                }
            }

            result?.Physics.Update(description.AlmostPhysics, ThingBase.UpdateLocationSettings.All);

            return result;
        }

        public LazerBeamPew GetLazerBeam(int? id, ThingAdditionalInfo extras,
            ElapsingThingSettings elapsingThingSettings = null, Weapon weapon = null)
        {
            if (!_isServer && !id.HasValue)
            {
                throw new ArgumentException("The identifier is expected in a non-server environment.", nameof(id));
            }

            // The creator could potentially be dead. Will not spawn his lazer.
            // TODO: this can lead to problems if someone is on the server, not on the client, a lazer is shot by him => client wont's spawn it
            //Debug.Assert(extras.Creator != null,
            //    "A lazer will not be spawned because the creator cannot be found. Weird.");
            ThingBase shooter = extras.Creator;
            if (shooter == null)
            {
                return null;
            }

            Vector2D lazerInteria = extras.Creator.Physics.GetDirection(extras.Creator.Settingz.LazerSpeed);
            Point spawnPoint = shooter.Physics.GetPointInDirection(shooter.Settingz.Size/2);

            var lazer = new LazerBeamPew(_thingInjections, extras, id, elapsingThingSettings);
            lazer.Spawn(spawnPoint, lazerInteria);
            lazer.Physics.Angle = extras.Creator.Physics.Angle;
            return lazer;
        }

        public Bomb GetBomb(int? id, ThingAdditionalInfo extras, ElapsingThingSettings elapsingThingSettings = null,
            Weapon weapon = null)
        {
            ThingBase shooter = extras.Creator;
            if (shooter == null)
            {
                return null;
            }

            // Go a bit slower than the ship.
            Vector2D initialInteria = shooter.Physics.TotalForce.GetScaled(0.9f, false);

            // Spawn behind the ship.
            Point spawnPoint = shooter.Physics.GetPointInDirection(shooter.Physics.Size, true);

            var bomb = new Bomb(_thingInjections, id, extras, elapsingThingSettings);
            bomb.Spawn(spawnPoint, initialInteria);
            return bomb;
        }

        private PlayerShipMovable GetPlayerShipMovable(Point location, string name, ClanEnum clan, int? id = null,
            ThingAdditionalInfo additionalInfo = null, ElapsingThingSettings elapsingThingSettings = null)
        {
            if (!_isServer && !id.HasValue)
            {
                throw new ArgumentException("The identifier is expected in a non-server environment.", nameof(id));
            }

            var ship = new PlayerShipMovable(_thingInjections, _coordinator, name, clan, id,
                additionalInfo ?? ThingAdditionalInfo.GetNew(null), elapsingThingSettings);
            ship.Spawn(location);
            return ship;
        }

        public PlayerShipOther GetPlayerShip(Point location, string name, ClanEnum clan, int? id = null,
            ThingAdditionalInfo additionalInfo = null, ElapsingThingSettings elapsingThingSettings = null)
        {
            if (!_isServer && !id.HasValue)
            {
                throw new ArgumentException("The identifier is expected in a non-server environment.", nameof(id));
            }

            bool isFirstSpawn = _isServer && !id.HasValue;
            if (!isFirstSpawn && additionalInfo == null)
            {
                throw new ArgumentException("The additionalInfo is expected during non-first (consequent) creations.",
                    nameof(id));
            }

            var ship = new PlayerShipOther(_thingInjections, _coordinator, name, clan,
                additionalInfo ?? ThingAdditionalInfo.GetNew(null), id, elapsingThingSettings);
            ship.Spawn(location);
            return ship;
        }

        public AiShip GetRandomAiShip(Point point, string name, AiType? aiType = null,
            ShipSettingType? shipSettingType = null, int? id = null, ThingAdditionalInfo additionalInfo = null,
            ElapsingThingSettings elapsingThingSettings = null)
        {
            ShipSettingType shipSettingTypeValue = shipSettingType ?? EnumHelper.GetRandomEnumValue<ShipSettingType>();
            AiType aiTypeValue = AiType.Regular; //aiType ?? EnumHelper.GetRandomEnumValue<AiType>();

            var ship = new AiShip(_thingInjections, _coordinator, aiTypeValue, shipSettingTypeValue, name,
                additionalInfo ?? ThingAdditionalInfo.GetNew(null), id, elapsingThingSettings);
            ship.Spawn(point);
            return ship;
        }

        public BigMass GetBigMass(int size, Point point, Color? color, int? id = null,
            ThingAdditionalInfo additionalInfo = null,
            ElapsingThingSettings elapsingThingSettings = null)
        {
            BigMass mass = new BigMass(_thingInjections, size, id, additionalInfo ?? ThingAdditionalInfo.GetNew(null),
                color, elapsingThingSettings);
            mass.Spawn(point);
            return mass;
        }
    }
}