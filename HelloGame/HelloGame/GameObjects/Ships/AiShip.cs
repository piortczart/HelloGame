using HelloGame.MathStuff;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HelloGame.GameObjects.Ships
{
    public class AiShip : DaShip
    {
        private readonly Limiter _locatePlayerLimiter = new Limiter(TimeSpan.FromSeconds(5));

        public AiShip(GameState scene) : base(scene)
        {
        }

        protected override void UpdateModelInternal(TimeSpan timeSinceLastUpdate, List<ThingBase> otherThings)
        {
            if (_locatePlayerLimiter.CanHappen())
            {

                // Locate a ship.
                var player = otherThings.FirstOrDefault(s => s is PlayerShip);
                if (player != null)
                {
                    // Face him.
                    var x = player.Physics.PositionX - Physics.PositionX;
                    var y = player.Physics.PositionY - Physics.PositionY;
                    var v = new Real2DVector(x, y);

                    Physics.Angle = v.Angle;
                }

            }

            PewPew();
        }
    }
}
