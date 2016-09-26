using System;
using System.Drawing;

namespace HelloGame.Common.Extensions
{
    public static class ExtensionsRandom
    {
        public static Point GetRandomPoint(this Random random, Rectangle area)
        {
            int x = random.Next(area.X, area.X + area.Width);
            int y = random.Next(area.Y, area.Y + area.Height);
            return new Point(x, y);
        }
    }
}