using System;
using System.Drawing;

namespace HelloGame.Common.Physicsish
{
    public class Position
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }

        public Point Point => new Point((int) X, (int) Y);

        public Position(decimal x, decimal y)
        {
            X = x;
            Y = y;
        }

        public decimal DistanceTo(Position position)
        {
            decimal a = (X - position.X)*(X - position.X) + (Y - position.Y)*(Y - position.Y);
            if (Math.Abs(a) < 0.00005m)
            {
                return 0;
            }
            return (decimal) Math.Sqrt((double) a);
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