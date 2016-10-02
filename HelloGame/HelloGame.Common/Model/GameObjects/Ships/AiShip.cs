using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using HelloGame.Common.Logging;
using HelloGame.Common.MathStuff;

namespace HelloGame.Common.Model.GameObjects.Ships
{
    public class AiShip : DaShip
    {
        private readonly Limiter _locatePlayerLimiter = new Limiter(TimeSpan.FromSeconds(1));
        Real2DVector _playerPointer = new Real2DVector();

        private static readonly ThingSettings Settings = new ThingSettings
        {
            Aerodynamism = 0.1m,
            TimeToLive = TimeSpan.Zero,
            Mass = 3,
            RadPerSecond = (decimal)Math.PI,
            LazerLimit = TimeSpan.FromSeconds(5)
        };

        public AiShip(ILogger logger, GameThingCoordinator gameManager, string name, decimal size = 10, int? id = null, ThingBase creator = null) 
            : base(logger, gameManager, Settings, size, name, id, creator)
        {
        }

        protected override void PaintStuffInternal(Graphics g)
        {
        }

        protected override void UpdateModelInternal(TimeSpan timeSinceLastUpdate, IEnumerable<ThingBase> otherThings)
        {
            if (IsDestroyed)
            {
                return;
            }

            ThingBase player = otherThings.FirstOrDefault(s => s is PlayerShip);
            if (player != null)
            {
                if (_locatePlayerLimiter.CanHappen())
                {
                    // Locate a player's ship.

                    // Face him.
                    decimal x = player.Physics.Position.X - Physics.Position.X;
                    decimal y = player.Physics.Position.Y - Physics.Position.Y;

                    _playerPointer = Real2DVector.GetFromCoords(x, y);

                    decimal angleBefore = Physics.Angle;
                    Physics.Angle = _playerPointer.Angle;

                    PewPew();
                }

                if (player.Physics.Position.DistanceTo(Physics.Position) > 300)
                {
                    int maxSpeed = 3;
                    Physics.SelfPropelling.Change(Physics.Angle, maxSpeed);
                }
                else
                {
                    int maxSpeed = -3;
                    Physics.SelfPropelling.Change(Physics.Angle, maxSpeed);
                }
            }
        }
    }
}
