using System;
using System.Drawing;

namespace HelloGame.GameObjects
{
    public class LazerBeamPew : ThingBase
    {
        public LazerBeamPew() : base(TimeSpan.FromSeconds(1))
        {
        }

        public override void PaintStuff(Graphics g)
        {
            var pen = new Pen(Brushes.Red);

            Point pointInDirection = Model.GetPointInDirection(5);
            g.DrawLine(pen, Model.PositionPoint, pointInDirection);
        }

        protected override void UpdateModelInternal()
        {
            Model.PositionX += Physics.Interia.X / 10;
            Model.PositionY += Physics.Interia.Y / 10;
        }
    }
}
