using System;

namespace HelloGame.Physicsish
{
    public class Position
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }

        public Position(decimal x, decimal y)
        {
            X = x;
            Y = y;
        }

        public decimal DistanceTo(Position position)
        {
            decimal a = (X - position.X) * (X - position.X) + (Y - position.Y) * (Y - position.Y);
            if (a < -0.00005m)
            {
                return 0;
            }
            return (decimal)Math.Sqrt((double)a);
        }
    }
}
