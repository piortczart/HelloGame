using System;
using System.Drawing;
using HelloGame.Common.Logging;
using HelloGame.Common.MathStuff;
using HelloGame.Common.Model.GameObjects;
using HelloGame.Common.Model.GameObjects.Ships;

namespace HelloGame.Common.Model
{
    public interface IThingFactory
    {
        ThingBase CreateFromDescription(ThingDescription description);
        PlayerShipMovable GetPlayerShipMovable(int size, Point location, string name, int? id = null);
        PlayerShipAny GetPlayerShip(int size, Point location, string name, int? id = null);
        AiShip GetAiShip(int size, Point point, string name, int? id = null);
        BigMass GetBigMass(int? size = null, Point? point = null, int? id = null);
    }

    /// <summary>
    /// This was created because ThingBase (ship) needed GameManager to add stuff, was constructed in ThingFactory, which was needed in GameManager.
    /// Circular constructor stuff.
    /// </summary>
    public class GameThingCoordinator
    {
        private ModelManager _model;

        Action<ThingBase> _askToSpawnAction;
        Action<ThingBase> _updateThingAction;

        public GameThingCoordinator(ModelManager model)
        {
            _model = model;
        }

        public void SetActions(Action<ThingBase> askToSpawn, Action<ThingBase> updateThing)
        {
            _askToSpawnAction = askToSpawn;
            _updateThingAction = updateThing;
        }

        public void AskServerToSpawn(ThingBase thing) {
            _askToSpawnAction(thing);
        }

        public void UpdateThing(ThingBase thing) {
            _updateThingAction(thing);
        }
    }

    public class ThingFactory : IThingFactory
    {
        private readonly bool _isServer;
        private readonly GameThingCoordinator _gameManager;
        private readonly ILogger _logger;

        public ThingFactory(bool isServer, GameThingCoordinator gameManager, ILoggerFactory loggerFactory)
        {
            _isServer = isServer;
            _gameManager = gameManager;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public ThingBase CreateFromDescription(ThingDescription description)
        {
            switch (description.Type)
            {
                case "PlayerShipAny":
                    {
                        string name = (string)description.ConstructParams[0];
                        int size = (int)(double)description.ConstructParams[1];

                        return GetPlayerShip(size, description.AlmostPhysics.PositionPoint, name, description.Id);
                    }
                case "PlayerShipMovable":
                    {
                        string name = (string)description.ConstructParams[0];
                        int size = (int)(double)description.ConstructParams[1];

                        return GetPlayerShipMovable(size, description.AlmostPhysics.PositionPoint, name, description.Id);
                    }
                case "AiShip":
                    {
                        string name = (string)description.ConstructParams[0];
                        int size = (int)(double)description.ConstructParams[1];

                        return GetAiShip(size, description.AlmostPhysics.PositionPoint, name, description.Id);
                    }
                case "BigMass":
                    {
                        int size = (int)(double)description.ConstructParams[0];

                        return GetBigMass(size, description.AlmostPhysics.PositionPoint, description.Id);
                    }
                case "LazerBeamPew":
                    {
                        return GetLazerBeam(description.Id, description.AlmostPhysics.PositionPoint);
                    }
            }

            throw new NotImplementedException($"Cannot spawn {description.Type}");
        }

        private ThingBase GetLazerBeam(int? id, Point location)
        {
            if (!_isServer && !id.HasValue)
            {
                throw new ArgumentException("The identifier is expected in a non-server environment.", nameof(id));
            }

            var lazer = new LazerBeamPew(_logger, null);
            lazer.Spawn(location);
            return lazer;
        }

        public PlayerShipMovable GetPlayerShipMovable(int size, Point location, string name, int? id = null)
        {
            if (!_isServer && !id.HasValue)
            {
                throw new ArgumentException("The identifier is expected in a non-server environment.", nameof(id));
            }

            var ship = new PlayerShipMovable(_logger, _gameManager, name, size, id);
            ship.Spawn(location);
            return ship;
        }


        public PlayerShipAny GetPlayerShip(int size, Point location, string name, int? id = null)
        {
            if (!_isServer && !id.HasValue)
            {
                throw new ArgumentException("The identifier is expected in a non-server environment.", nameof(id));
            }

            var ship = new PlayerShipAny(_logger, _gameManager, name, size, id);
            ship.Spawn(location);
            return ship;
        }

        public AiShip GetAiShip(int size, Point point, string name, int? id = null)
        {
            var ship = new AiShip(_logger, _gameManager, name, size, id);
            ship.Spawn(point);
            return ship;
        }

        public BigMass GetBigMass(int? size = null, Point? point = null, int? id = null)
        {
            BigMass mass = new BigMass(_logger, size ?? MathX.Random.Next(80, 200), id);
            mass.Spawn(point ?? new Point(MathX.Random.Next(100, 500), MathX.Random.Next(400, 600)));
            return mass;
        }
    }
}