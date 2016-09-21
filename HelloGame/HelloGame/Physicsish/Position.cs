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
            return (decimal)Math.Sqrt((double)((X - position.X) * (X - position.X) + (Y - position.Y) * (Y - position.Y)));
        }
    }
}
