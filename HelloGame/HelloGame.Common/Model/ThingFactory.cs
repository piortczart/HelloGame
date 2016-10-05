using System;
using System.Drawing;
using HelloGame.Common.Logging;
using HelloGame.Common.MathStuff;
using HelloGame.Common.Model.GameObjects;
using HelloGame.Common.Model.GameObjects.Ships;

namespace HelloGame.Common.Model
{
    public class ThingFactory
    {
        private readonly bool _isServer;
        private readonly GameThingCoordinator _gameManager;
        private readonly ILogger _logger;
        private readonly TimeSource _timeSource;
        private readonly ThingBaseInjections _thingInjections;

        public ThingFactory(bool isServer, ThingBaseInjections injections, GameThingCoordinator gameManager, ILoggerFactory loggerFactory, TimeSource timeSource)
        {
            _timeSource = timeSource;
            _isServer = isServer;
            _gameManager = gameManager;
            _logger = loggerFactory.CreateLogger(GetType());
            _thingInjections = injections;
        }

        public ThingBase CreateFromDescription(ThingDescription description)
        {
            switch (description.Type)
            {
                case "PlayerShipAny":
                    {
                        string name = (string)description.ConstructParams[0];
                        int? creator = (int?)description.ConstructParams[1];
                        ClanEnum clan = (ClanEnum)(int)(long)description.ConstructParams[2];

                        return GetPlayerShip(description.AlmostPhysics.PositionPoint, name, clan, description.Id, _gameManager.GetThingById(creator));
                    }
                case "PlayerShipMovable":
                    {
                        string name = (string)description.ConstructParams[0];
                        int? creator = (int?)description.ConstructParams[1];
                        ClanEnum clan = (ClanEnum)(int)(long)description.ConstructParams[2];

                        return GetPlayerShipMovable(description.AlmostPhysics.PositionPoint, name, clan, description.Id, _gameManager.GetThingById(creator));
                    }
                case "AiShip":
                    {
                        string name = (string)description.ConstructParams[0];
                        int? creator = (int?)description.ConstructParams[1];

                        return GetAiShip(description.AlmostPhysics.PositionPoint, name, description.Id, _gameManager.GetThingById(creator));
                    }
                case "BigMass":
                    {
                        int size = (int)(double)description.ConstructParams[0];
                        int? creator = (int?)description.ConstructParams[1];

                        return GetBigMass(size, description.AlmostPhysics.PositionPoint, description.Id, _gameManager.GetThingById(creator));
                    }
                case "LazerBeamPew":
                    {
                        // This can be spawned by the player. Server should give it a proper id.
                        // The id comming from the player can be bad.
                        // TODO: This can be spawned by the server too!!
                        int? id = _isServer ? (int?)null : description.Id;
                        int? creatorId = (int?)(long?)description.ConstructParams[0];

                        ThingBase creatorThing = _gameManager.GetThingById(creatorId);
                        return creatorThing != null ? GetLazerBeam(id, description.AlmostPhysics.PositionPoint, creatorThing) : null;
                    }
            }

            throw new NotImplementedException($"Cannot spawn {description.Type}");
        }

        public LazerBeamPew GetLazerBeam(int? id, Point location, ThingBase creator)
        {
            if (!_isServer && !id.HasValue)
            {
                throw new ArgumentException("The identifier is expected in a non-server environment.", nameof(id));
            }

            var lazer = new LazerBeamPew(_thingInjections, creator, id);
            Real2DVector lazerInteria = creator.Physics.GetDirection(creator.Settingz.LazerSpeed);
            lazer.Spawn(location, lazerInteria);
            lazer.Physics.Angle = creator.Physics.Angle;
            return lazer;
        }

        public PlayerShipMovable GetPlayerShipMovable(Point location, string name, ClanEnum clan, int? id = null, ThingBase creator = null)
        {
            if (!_isServer && !id.HasValue)
            {
                throw new ArgumentException("The identifier is expected in a non-server environment.", nameof(id));
            }

            var ship = new PlayerShipMovable(_thingInjections, _gameManager, name, clan, id, creator);
            ship.Spawn(location);
            return ship;
        }

        public PlayerShipOther GetPlayerShip(Point location, string name, ClanEnum clan, int? id = null, ThingBase creator = null)
        {
            if (!_isServer && !id.HasValue)
            {
                throw new ArgumentException("The identifier is expected in a non-server environment.", nameof(id));
            }

            var ship = new PlayerShipOther(_thingInjections, _gameManager, name, clan, id, creator);
            ship.Spawn(location);
            return ship;
        }

        public AiShip GetAiShip(Point point, string name, int? id = null, ThingBase creator = null)
        {
            var ship = new AiShip(_thingInjections, _gameManager, name, id, creator);
            ship.Spawn(point);
            return ship;
        }

        public BigMass GetBigMass(int size, Point point, int? id = null, ThingBase creator = null)
        {
            BigMass mass = new BigMass(_thingInjections, size, id, creator);
            mass.Spawn(point);
            return mass;
        }
    }
}