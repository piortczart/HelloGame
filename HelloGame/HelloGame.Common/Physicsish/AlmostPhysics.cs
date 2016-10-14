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
        public float Aerodynamism { get; set; }

        public float Mass { get; set; }

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

        public float RadPerSecond { get; set; }

        public Vector2D TotalForce => Vector2D.Combine(SelfPropelling, Interia, Drag);

        public float Angle { get; set; }
        public Position Position { get; set; }

        public Point PositionPoint => new Point((int) Position.X, (int) Position.Y);

        public float Size { get; set; }

        public AlmostPhysics(float aerodynamism, float size = 1)
        {
            Interia = Vector2D.Zero();
            SelfPropelling = Vector2D.Zero();
            Aerodynamism = aerodynamism;
            Size = size;
        }

        public void SetInitialPosition(Point point)
        {
            Position = new Position(point.X, point.Y);
        }

        public void PositionDelta(float deltaX, float deltaY)
        {
            Position.X += deltaX;
            Position.Y += deltaY;
        }

        public Vector2D GetVelocity(float length)
        {
            return Vector2D.GetFromAngleLength(Angle, length);
        }

        public Point GetPointInDirection(float bigness, bool inverted = false)
        {
            Vector2D direction = GetVelocity(bigness);
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
    }
}