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
        private readonly GameManager _gameManager;
        private readonly ILogger _logger;

        public ThingFactory(bool isServer, GameManager gameManager, ILoggerFactory loggerFactory)
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
            }

            throw new NotImplementedException($"Cannot spawn {description.Type}");
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