using System;
using System.Drawing;

namespace HelloGame
{
    public class LazerBeamPew : AThing
    {
        public LazerBeamPew() : base(TimeSpan.FromSeconds(1))
        {
        }

        public override void PaintStuff(Graphics g)
        {
            var pen = new Pen(Brushes.Red);

            Point p2 = Model.GetPointInDirection(5);
            g.DrawLine(pen, Model.PositionPoint, p2);
        }

        public override void UpdateModelInternal()
        {
            Model.PositionX += Physics.Interia.X / 10;
            Model.PositionY += Physics.Interia.Y / 10;
        }
    }
}
