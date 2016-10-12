using System.Drawing;
using System.Windows.Forms;

namespace HelloGame.Common.Extensions
{
    public static class ExtensionsGraphics
    {
        /// <summary>
        /// Draws the string centered on the given point.
        /// </summary>
        public static void DrawStringCentered(this Graphics graphics, string text, Font font, Brush brush,
            Point centerPosition)
        {
            Size textSize = TextRenderer.MeasureText(text, font);
            PointF position = new PointF(centerPosition.X - textSize.Width/2, centerPosition.Y - textSize.Height/2);
            graphics.DrawString(text, font, brush, position);
        }
    }
}