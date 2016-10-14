using System;
using System.Drawing;
using HelloGame.Common.MathStuff;
using HelloGame.Common.Model;

namespace HelloGame.Common.Physicsish
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
        public Vector2D SelfPropelling { get; set; }

        /// <summary>
        /// Current interia of the object.
        /// </summary>
        public Vector2D Interia { get; set; }

        /// <summary>
        /// Current drag.
        /// </summary>
        public Vector2D Drag { get; set; }

        public Vector2D Gravity { get; set; }

        public decimal RadPerSecond { get; set; }

        public Vector2D TotalForce => Vector2D.Combine(SelfPropelling, Interia, Drag);

        public decimal Angle { get; set; }
        public Position Position { get; set; }

        public Point PositionPoint => new Point((int) Position.X, (int) Position.Y);

        public decimal Size { get; set; }

        public AlmostPhysics(decimal aerodynamism, decimal size = 1)
        {
            Interia = new Vector2D();
            SelfPropelling = new Vector2D();
            Aerodynamism = aerodynamism;
            Size = size;
        }

        public void SetInitialPosition(Point point)
        {
            Position = new Position(point.X, point.Y);
        }

        public void PositionDelta(decimal deltaX, decimal deltaY)
        {
            Position.X += deltaX;
            Position.Y += deltaY;
        }

        public Vector2D GetDirection(decimal bigness)
        {
            return new Vector2D(Angle, bigness);
        }

        public Point GetPointInDirection(decimal bigness, bool inverted = false)
        {
            Vector2D direction = GetDirection(bigness);
            if (inverted)
            {
                direction = direction.GetOpposite();
            }
            return new Point((int) (direction.X + Position.X), (int) (direction.Y + Position.Y));
        }

        public void Update(AlmostPhysics other, ThingBase.UpdateLocationSettings settings)
        {
            switch (settings)
            {
                case ThingBase.UpdateLocationSettings.All:
                    Position = new Position(other.Position.X, other.Position.Y);
                    Angle = other.Angle;
                    Drag = other.Drag;
                    RadPerSecond = other.RadPerSecond;
                    SelfPropelling = other.SelfPropelling;
                    Aerodynamism = other.Aerodynamism;
                    Mass = other.Mass;
                    Gravity = other.Gravity;
                    Interia = other.Interia;
                    Size = other.Size;
                    break;
                case ThingBase.UpdateLocationSettings.ExcludePositionAndAngle:
                    //Position = new Position(other.Position.X, other.Position.Y);
                    //Angle = other.Angle;
                    Drag = other.Drag;
                    RadPerSecond = other.RadPerSecond;
                    SelfPropelling = other.SelfPropelling;
                    Aerodynamism = other.Aerodynamism;
                    Mass = other.Mass;
                    Gravity = other.Gravity;
                    Interia = other.Interia;
                    Size = other.Size;
                    break;
                case ThingBase.UpdateLocationSettings.AngleAndEngineAndPosition:
                    Position = new Position(other.Position.X, other.Position.Y);
                    Angle = other.Angle;
                    //Drag = other.Drag;
                    //RadPerSecond = other.RadPerSecond;
                    SelfPropelling = other.SelfPropelling;
                    //Aerodynamism = other.Aerodynamism;
                    //Mass = other.Mass;
                    //Gravity = other.Gravity;
                    //Interia = other.Interia;
                    //Size = other.Size;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(settings), settings, null);
            }
        }

        public void Reset(Position position)
        {
            Angle = 0;
            Drag = Vector2D.GetZero();
            Gravity = Vector2D.GetZero();
            Interia = Vector2D.GetZero();
            Position = position;
            SelfPropelling = Vector2D.GetZero();
        }
    }
}