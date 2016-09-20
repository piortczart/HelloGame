using System;
using System.Drawing;
using HelloGame.MathStuff;

namespace HelloGame.GameObjects
{
    class Bomb : ThingBase
    {
        public bool IsBlownUp { get; set; }

        public Bomb()
            : this(TimeSpan.FromSeconds(2))
        {
        }

        private Bomb(TimeSpan timeToLive) : base(timeToLive)
        {
            Physics.Interia = new Real2DVector();
        }

        public override void PaintStuff(Graphics g)
        {
            if (!IsBlownUp)
            {
                Pen pen = new Pen(Color.FromArgb((int)(AgePercentage * 2.5), 0, 0));
                int width = 5;
                g.DrawArc(pen, new Rectangle((int)Model.PositionX - width / 2, (int)Model.PositionY - width / 2, width, width), 0, 360);

                if (AgePercentage > 95)
                {
                    IsBlownUp = true;
                }
            }
            else
            {
                int width = 50;
                g.FillEllipse(Brushes.Yellow, new Rectangle((int)Model.PositionX - width / 2, (int)Model.PositionY - width / 2, width, width));
            }
        }

        protected override void UpdateModelInternal(TimeSpan timeSinceLastUpdate)
        {
            Real2DVector totalForce = new Real2DVector();
            totalForce.Add(Physics.Interia);

            Model.PositionX += totalForce.X / 10;
            Model.PositionY += totalForce.Y / 10;
        }
    }
}
