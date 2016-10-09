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
        private readonly GameThingCoordinator _gameManager;
        private readonly ThingBaseInjections _thingInjections;

        public ThingFactory(bool isServer, ThingBaseInjections injections, GameThingCoordinator gameManager,
            ILoggerFactory loggerFactory)
        {
            _isServer = isServer;
            _gameManager = gameManager;
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
                    int? creator = (int?) description.ConstructParams[1];
                    ClanEnum clan = (ClanEnum) (int) (long) description.ConstructParams[2];
                    ElapsingThingSettings elapsing =
                        description.ConstructParams.Length > 3
                            ? ((string) description.ConstructParams[3]).DeSerializeJson<ElapsingThingSettings>()
                            : null;

                    result = GetPlayerShip(description.AlmostPhysics.PositionPoint, name, clan, description.Id,
                        _gameManager.GetThingById(creator), elapsing);
                    break;
                }
                case "PlayerShipMovable":
                {
                    string name = (string) description.ConstructParams[0];
                    int? creator = (int?) description.ConstructParams[1];
                    ClanEnum clan = (ClanEnum) (int) (long) description.ConstructParams[2];
                    ElapsingThingSettings elapsing =
                        description.ConstructParams.Length > 3
                            ? ((string) description.ConstructParams[3]).DeSerializeJson<ElapsingThingSettings>()
                            : null;

                    result = GetPlayerShipMovable(description.AlmostPhysics.PositionPoint, name, clan, description.Id,
                        _gameManager.GetThingById(creator), elapsing);
                    break;
                }
                case "AiShip":
                {
                    string name = (string) description.ConstructParams[0];
                    int? creator = (int?) description.ConstructParams[1];
                    AiType aiType = (AiType) (int) (long) description.ConstructParams[2];
                    ShipSettingType shipSettingType = (ShipSettingType) (int) (long) description.ConstructParams[3];
                    ElapsingThingSettings elapsing =
                        description.ConstructParams.Length > 4
                            ? ((string) description.ConstructParams[4]).DeSerializeJson<ElapsingThingSettings>()
                            : null;

                    result = GetRandomAiShip(description.AlmostPhysics.PositionPoint, name, aiType, shipSettingType,
                        description.Id, _gameManager.GetThingById(creator), elapsing);
                    break;
                }
                case "BigMass":
                {
                    int size = (int) (double) description.ConstructParams[0];
                    Color color = ((string) description.ConstructParams[1]).DeSerializeJson<Color>();
                    int? creator = (int?) description.ConstructParams[2];
                    ElapsingThingSettings elapsing =
                        description.ConstructParams.Length > 3
                            ? ((string) description.ConstructParams[3]).DeSerializeJson<ElapsingThingSettings>()
                            : null;

                    result = GetBigMass(size, description.AlmostPhysics.PositionPoint, color, description.Id,
                        _gameManager.GetThingById(creator), elapsing);
                    break;
                }
                case "LazerBeamPew":
                {
                    // This can be spawned by the player. Server should give it a proper id.
                    // The id comming from the player can be bad.
                    // TODO: This can be spawned by the server too!!
                    int? id = _isServer ? (int?) null : description.Id;
                    int? creatorId = (int?) (long?) description.ConstructParams[0];
                    ElapsingThingSettings elapsing =
                        description.ConstructParams.Length > 1
                            ? ((string) description.ConstructParams[1]).DeSerializeJson<ElapsingThingSettings>()
                            : null;

                    ThingBase creatorThing = _gameManager.GetThingById(creatorId);
                    result = creatorThing != null
                        ? GetLazerBeam(id, description.AlmostPhysics.PositionPoint, creatorThing, elapsing)
                        : null;
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

        public LazerBeamPew GetLazerBeam(int? id, Point location, ThingBase creator,
            ElapsingThingSettings elapsingThingSettings = null)
        {
            if (!_isServer && !id.HasValue)
            {
                throw new ArgumentException("The identifier is expected in a non-server environment.", nameof(id));
            }

            var lazer = new LazerBeamPew(_thingInjections, creator, id, elapsingThingSettings);
            Real2DVector lazerInteria = creator.Physics.GetDirection(creator.Settingz.LazerSpeed);
            lazer.Spawn(location, lazerInteria);
            lazer.Physics.Angle = creator.Physics.Angle;
            return lazer;
        }

        private PlayerShipMovable GetPlayerShipMovable(Point location, string name, ClanEnum clan, int? id = null,
            ThingBase creator = null, ElapsingThingSettings elapsingThingSettings = null)
        {
            if (!_isServer && !id.HasValue)
            {
                throw new ArgumentException("The identifier is expected in a non-server environment.", nameof(id));
            }

            var ship = new PlayerShipMovable(_thingInjections, _gameManager, name, clan, id, creator,
                elapsingThingSettings);
            ship.Spawn(location);
            return ship;
        }

        public PlayerShipOther GetPlayerShip(Point location, string name, ClanEnum clan, int? id = null,
            ThingBase creator = null, ElapsingThingSettings elapsingThingSettings = null, int score = 0)
        {
            if (!_isServer && !id.HasValue)
            {
                throw new ArgumentException("The identifier is expected in a non-server environment.", nameof(id));
            }

            var ship = new PlayerShipOther(_thingInjections, _gameManager, name, clan, id, creator,
                elapsingThingSettings, score);
            ship.Spawn(location);
            return ship;
        }

        public AiShip GetRandomAiShip(Point point, string name, AiType? aiType = null,
            ShipSettingType? shipSettingType = null, int? id = null, ThingBase creator = null,
            ElapsingThingSettings elapsingThingSettings = null)
        {
            ShipSettingType shipSettingTypeValue = shipSettingType ?? EnumHelper.GetRandomEnumValue<ShipSettingType>();
            AiType aiTypeValue = AiType.Regular; //aiType ?? EnumHelper.GetRandomEnumValue<AiType>();

            var ship = new AiShip(_thingInjections, _gameManager, aiTypeValue, shipSettingTypeValue, name, id, creator,
                elapsingThingSettings);
            ship.Spawn(point);
            return ship;
        }

        public BigMass GetBigMass(int size, Point point, Color? color, int? id = null, ThingBase creator = null,
            ElapsingThingSettings elapsingThingSettings = null)
        {
            BigMass mass = new BigMass(_thingInjections, size, id, creator, color, elapsingThingSettings);
            mass.Spawn(point);
            return mass;
        }
    }
}