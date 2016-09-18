using System.Drawing;

namespace HelloGame
{
    public class ThingModel
    {
        public const double mass = 1;
        public double ShipAngle { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public Point PositionPoint
        {
            get
            {
                return new Point((int)PositionX, (int)PositionY);
            }
        }

    }
}
