using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using HelloGame.MathStuff;

namespace HelloGame.GameObjects.Ships
{
    public class AiShip : DaShip
    {
        private readonly Limiter _locatePlayerLimiter = new Limiter(TimeSpan.FromSeconds(2));

        Real2DVector _playerPointer = new Real2DVector();

        public AiShip(GameState scene) : base(scene)
        {
        }

        protected override void PaintStuffInternal(Graphics g)
        {
            //g.DrawLine(ShipPen, Physics.PositionPoint, 
            //    new Point((int)(Physics.PositionPoint.X + playerPointer.X), (int)(Physics.PositionPoint.Y + playerPointer.Y)));

            //g.DrawString($"Ship angle: {playerPointer.AngleDegree}", font, Brushes.Black, new PointF(155, 295));
        }

        protected override void UpdateModelInternal(TimeSpan timeSinceLastUpdate, List<ThingBase> otherThings)
        {
            if (IsDestroyed)
            {
                return;
            }

            if (_locatePlayerLimiter.CanHappen())
            {
                // Locate a ship.
                var player = otherThings.FirstOrDefault(s => s is PlayerShip);
                if (player != null)
                {
                    // Face him.
                    var x = player.Physics.Position.X - Physics.Position.X;
                    var y = player.Physics.Position.Y - Physics.Position.Y;

                    _playerPointer = Real2DVector.GetFromCoords(x, y);

                    Physics.Angle = _playerPointer.Angle;

                    PewPew();
                }

            }

        }
    }
}
