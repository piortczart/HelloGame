using System;
using System.Collections.Generic;
using System.Drawing;
using HelloGame.Common.MathStuff;

namespace HelloGame.Common.Model.GameObjects
{
    public class BigMass : ThingBase
    {
        static readonly ThingSettings Settings = new ThingSettings
        {
            Aerodynamism = int.MaxValue,
            TimeToLive = TimeSpan.Zero,
            CanBeMoved = false
        };

        private static Color GetRandom()
        {
            return Color.FromArgb(MathX.Random.Next(0, 255), MathX.Random.Next(0, 255), MathX.Random.Next(0, 255));
        }

        public BigMass(int size) : base(Settings)
        {
            Physics.Size = size;
            Physics.Mass = size * 10000;
        }

        public override void CollidesWith(ThingBase other)
        {
            GetRandom();
        }

        public override void PaintStuff(Graphics g)
        {
            var shipPen = new Pen(Brushes.Red);

            g.DrawArc(shipPen, new Rectangle((int)(Physics.Position.X - Physics.Size / 2), (int)(Physics.Position.Y - Physics.Size / 2), (int)Physics.Size, (int)Physics.Size), 0, 360);
        }

        protected override void UpdateModelInternal(TimeSpan timeSinceLastUpdate, IEnumerable<ThingBase> otherThings)
        {
        }
    }
}
