using System;
using System.Drawing;

namespace HelloGame
{
    class Projectile : AThing
    {
        AThing _parent;

        public Projectile(KeysInfo keysInfo, AThing parent)
            : this(keysInfo, parent, TimeSpan.Zero)
        {
        }

        public Projectile(KeysInfo keysInfo, AThing parent, TimeSpan timeToLive) : base(TimeSpan.FromSeconds(4))
        {
            _parent = parent;

            Physics.Interia = new Real2DVector();
            Physics.Drag = new Real2DVector();
        }

        public override void PaintStuff(Graphics g)
        {
            var pen = new Pen(Brushes.DarkBlue);
            int width = 5;
            g.DrawArc(pen, new Rectangle((int)Model.PositionX - width / 2, (int)Model.PositionY - width / 2, width, width), 0, 360);
        }

        public override void UpdateModelInternal()
        {
            Real2DVector totalForce = new Real2DVector();
            totalForce.Add(Physics.Interia);

            Model.PositionX += totalForce.X / 10;
            Model.PositionY += totalForce.Y / 10;
        }
    }
}
