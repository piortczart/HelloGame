using System.Drawing;
using System.Windows.Forms;

namespace HelloGame.Common.Extensions
{
    public static class ExtensionsGraphics
    {
        public static void DrawCircle(this Graphics graphics, Point center, int radius, Color color)
        {
            var leftTop = new Point(center.X - radius, center.Y - radius);
            graphics.DrawEllipse(new Pen(color), leftTop.X, leftTop.Y, radius * 2, radius * 2);
        }

        /// <summary>
        /// Draws the string centered on the given point.
        /// </summary>
        public static void DrawStringCentered(this Graphics graphics, string text, Font font, Brush brush,
            Point centerPosition)
        {
            Size textSize = TextRenderer.MeasureText(text, font);
            PointF position = new PointF(centerPosition.X - textSize.Width / 2, centerPosition.Y - textSize.Height / 2);
            graphics.DrawString(text, font, brush, position);
        }
    }
}