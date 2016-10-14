using System;
using System.Drawing;

namespace HelloGame.Common.Physicsish
{
    public class Position
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Point Point => new Point((int) X, (int) Y);

        public Position(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float DistanceTo(Position position)
        {
            float a = (X - position.X)*(X - position.X) + (Y - position.Y)*(Y - position.Y);
            if (Math.Abs(a) < 0.00005f)
            {
                return 0;
            }
            return (float) Math.Sqrt(a);
        }


        public override string ToString()
        {
            return $"{X:##}:{Y:##}";
        }

        public Position Copy()
        {
            return new Position(X, Y);
        }
    }
}