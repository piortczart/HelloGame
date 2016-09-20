﻿using HelloGame.MathStuff;
using System.Drawing;

namespace HelloGame
{
    public class AlmostPhysics
    {
        /// <summary>
        /// The more the bigger drag.
        /// </summary>
        public decimal Aerodynamism { get; set; }

        public decimal Mass { get; set; }
        /// <summary>
        /// Anything propeling the object.
        /// </summary>
        public Real2DVector SelfPropelling { get; set; }
        /// <summary>
        /// Current interia of the object.
        /// </summary>
        public Real2DVector Interia { get; set; }
        /// <summary>
        /// Current drag.
        /// </summary>
        public Real2DVector Drag { get; set; }

        public decimal RadPerSecond { get; set; }

        public Real2DVector TotalForce => Real2DVector.Combine(SelfPropelling, Interia, Drag);

        public decimal Angle { get; set; }
        public decimal PositionX { get; set; }
        public decimal PositionY { get; set; }

        public Point PositionPoint => new Point((int)PositionX, (int)PositionY);

        public AlmostPhysics(decimal aerodynamism)
        {
            Interia = new Real2DVector();
            SelfPropelling = new Real2DVector();
            Aerodynamism = aerodynamism;
        }

        public void SetPosition(Point point)
        {
            PositionX = point.X;
            PositionY = point.Y;
        }

        public Real2DVector GetDirection(decimal bigness)
        {
            return new Real2DVector(Angle, bigness);
        }

        public Point GetPointInDirection(decimal bigness)
        {
            Real2DVector direction = GetDirection(bigness);
            return new Point((int)(direction.X + PositionX), (int)(direction.Y + PositionY));
        }
    }
}