using System.Drawing;
using HelloGame.MathStuff;

namespace HelloGame.GameObjects
{
    public class ThingModel
    {
        public const double Mass = 1;
        public double ShipAngle { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }

        public Point PositionPoint => new Point((int)PositionX, (int)PositionY);

        public void SetPosition(Point point)
        {
            PositionX = point.X;
            PositionY = point.Y;
        }

        public Real2DVector GetDirection(double bigness)
        {
            return  new Real2DVector(ShipAngle, bigness);
        }

        public Point GetPointInDirection(double bigness)
        {
            Real2DVector direction = GetDirection(bigness);
            return new Point((int)(direction.X + PositionX), (int)(direction.Y + PositionY));
        }
    }
}
