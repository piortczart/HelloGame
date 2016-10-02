using HelloGame.Common.Model;
using System.Drawing;

namespace HelloGame.Common
{
    public class Overlay
    {
        Font Font = new Font(FontFamily.GenericMonospace, 12);
        private readonly EventPerSecond _paintCounter = new EventPerSecond();

        int _thingsCount = 0;

        internal void Update(ModelManager modelManager)
        {
            _thingsCount = modelManager.GetThings().Count;
        }

        public void Render(Graphics graphics)
        {
            _paintCounter.Add();

            graphics.DrawString($"Paints/s: {_paintCounter.GetPerSecond()}", Font, Brushes.Black, new PointF(10, 15));
            graphics.DrawString($"Things: {_thingsCount}", Font, Brushes.Black, new PointF(10, 25));
        }
    }
}
