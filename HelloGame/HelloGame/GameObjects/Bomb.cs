using System;
using System.Drawing;
using HelloGame.MathStuff;
using System.Collections.Generic;

namespace HelloGame.GameObjects
{
    class Bomb : ThingBase
    {
        public bool IsArmed { get; private set; }

        private static readonly ThingSettings Settings = new ThingSettings
        {
            Aerodynamism = 0.1m,
            TimeToLive = TimeSpan.FromSeconds(3),
            Mass = 5
        };

        public Bomb(ThingBase creator) : base(Settings, creator)
        {
            Physics.Interia = new Real2DVector();
        }

        public override void PaintStuff(Graphics g)
        {
            if (!IsDestroyed)
            {
                int r = (int)(AgePercentage * 2.5);

                Pen pen = new Pen(Color.FromArgb(Math.Min(255, r), 0, 0));
                int width = 5;

                if (!IsArmed)
                {
                    g.DrawArc(pen, new Rectangle((int)Physics.PositionX - width / 2, (int)Physics.PositionY - width / 2, width, width), 0, 360);
                }
                else
                {
                    g.FillEllipse(pen.Brush, new Rectangle((int)Physics.PositionX - width / 2, (int)Physics.PositionY - width / 2, width, width));
                }

                if (AgePercentage > 95)
                {
                    Destroy();
                }
            }
            else
            {
                int width = 50;
                g.FillEllipse(Brushes.Yellow, new Rectangle((int)Physics.PositionX - width / 2, (int)Physics.PositionY - width / 2, width, width));
            }
        }

        protected override void UpdateModelInternal(TimeSpan timeSinceLastUpdate, List<ThingBase> otherThings)
        {
            if (!IsArmed && Age > TimeSpan.FromSeconds(0.5))
            {
                IsArmed = true;
            }
        }

        public override void CollidesWith(ThingBase other)
        {
            if (IsArmed)
            {
                Destroy();
                ElapseIn(TimeSpan.FromSeconds(0.5));
            }
        }
    }
}
