using System;
using System.Collections.Generic;
using System.Drawing;
using HelloGame.Common.MathStuff;

namespace HelloGame.Common.Model.GameObjects
{
    public class Bomb : ThingBase
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
                    g.DrawArc(pen, new Rectangle((int)Physics.Position.X - width / 2, (int)Physics.Position.Y - width / 2, width, width), 0, 360);
                }
                else
                {
                    g.FillEllipse(pen.Brush, new Rectangle((int)Physics.Position.X - width / 2, (int)Physics.Position.Y - width / 2, width, width));
                }

                if (AgePercentage >= 100)
                {
                    Destroy(TimeSpan.FromSeconds(0.5));
                }
            }
            else
            {
                int width = 50;
                g.FillEllipse(Brushes.Yellow, new Rectangle((int)Physics.Position.X - width / 2, (int)Physics.Position.Y - width / 2, width, width));
            }
        }

        protected override void UpdateModelInternal(TimeSpan timeSinceLastUpdate, IEnumerable<ThingBase> otherThings)
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
                Destroy(TimeSpan.FromSeconds(0.5));
            }
        }
    }
}
