using HelloGame.Common.Model;
using System.Drawing;
using HelloGame.Common.TimeStuffs;

namespace HelloGame.Common
{
    public class Overlay
    {
        readonly Font _font = new Font(FontFamily.GenericMonospace, 12);
        private readonly EventPerSecond _paintCounter;
        private int _collisionCalculations;
        private int _thingsCount;

        public Overlay(TimeSource timeSource)
        {
            _paintCounter = new EventPerSecond(timeSource);
        }

        internal void Update(ModelManager modelManager)
        {
            _thingsCount = modelManager.ThingsThreadSafe.Count;
            _collisionCalculations = (int) modelManager.CollisionCalculations.GetPerSecond();
        }

        public void Render(Graphics graphics)
        {
            _paintCounter.Add();

            graphics.DrawString($"Paints/s: {_paintCounter.GetPerSecond()}", _font, Brushes.Black, new PointF(10, 15));
            graphics.DrawString($"Collision calc/s: {_collisionCalculations}", _font, Brushes.Black, new PointF(10, 30));
            graphics.DrawString($"Things: {_thingsCount}", _font, Brushes.Black, new PointF(10, 45));
        }
    }
}