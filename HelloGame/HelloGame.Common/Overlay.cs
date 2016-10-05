﻿using HelloGame.Common.Model;
using System.Drawing;

namespace HelloGame.Common
{
    public class Overlay
    {
        readonly Font _font = new Font(FontFamily.GenericMonospace, 12);
        private readonly EventPerSecond _paintCounter;
        int collisionCalculations;

        int _thingsCount = 0;

        public Overlay(TimeSource timeSource)
        {
            _paintCounter = new EventPerSecond(timeSource);
        }

        internal void Update(ModelManager modelManager)
        {
            _thingsCount = modelManager.Things.Count;
            collisionCalculations = (int)modelManager.CollisionCalculations.GetPerSecond();
        }

        public void Render(Graphics graphics)
        {
            _paintCounter.Add();

            graphics.DrawString($"Paints/s: {_paintCounter.GetPerSecond()}", _font, Brushes.Black, new PointF(10, 15));
            graphics.DrawString($"Collision calc/s: {collisionCalculations}", _font, Brushes.Black, new PointF(10, 30));
            graphics.DrawString($"Things: {_thingsCount}", _font, Brushes.Black, new PointF(10, 45));
        }
    }
}