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
        private readonly Limiter _locatePlayerLimiter = new Limiter(TimeSpan.FromSeconds(2));

        Real2DVector _playerPointer = new Real2DVector();

        public AiShip(ILogger logger, GameManager gameManager, string name, decimal size = 10, int? id = null) : base(logger, gameManager, size, name, id)
        {
        }

        protected override void PaintStuffInternal(Graphics g)
        {
            //g.DrawLine(ShipPen, Physics.PositionPoint, 
            //    new Point((int)(Physics.PositionPoint.X + playerPointer.X), (int)(Physics.PositionPoint.Y + playerPointer.Y)));

            //g.DrawString($"Ship angle: {playerPointer.AngleDegree}", font, Brushes.Black, new PointF(155, 295));
        }

        protected override void UpdateModelInternal(TimeSpan timeSinceLastUpdate, IEnumerable<ThingBase> otherThings)
        {
            if (IsDestroyed)
            {
                return;
            }

            if (_locatePlayerLimiter.CanHappen())
            {
                // Locate a ship.
                ThingBase player = otherThings.FirstOrDefault(s => s is PlayerShip);
                if (player != null)
                {
                    // Face him.
                    decimal x = player.Physics.Position.X - Physics.Position.X;
                    decimal y = player.Physics.Position.Y - Physics.Position.Y;

                    _playerPointer = Real2DVector.GetFromCoords(x, y);

                    decimal angleBefore = Physics.Angle;
                    Physics.Angle = _playerPointer.Angle;

                    if (angleBefore != 0 && Physics.Angle < 0.1m)
                    {
                        ;
                    }

                    //PewPew();
                }

            }

        }
    }
}
