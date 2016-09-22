using System;
using System.Collections.Generic;
using System.Drawing;
using HelloGame.MathStuff;

namespace HelloGame.GameObjects
{
    public class BigMass : ThingBase
    {
        private readonly int _size;
        
        static readonly ThingSettings Settings = new ThingSettings
        {
            Aerodynamism = int.MaxValue,
            TimeToLive = TimeSpan.Zero
        };

        private static Color GetRandom()
        {
            return Color.FromArgb(MathX.Random.Next(0, 255), MathX.Random.Next(0, 255), MathX.Random.Next(0, 255));
        }

        public BigMass(int size) : base(Settings)
        {
            _size = size;
            Physics.Mass = size * 10000;
        }

        public override void CollidesWith(ThingBase other)
        {
            GetRandom();
        }

        public override void PaintStuff(Graphics g)
        {
            var shipPen = new Pen(Brushes.Red);

            g.DrawArc(shipPen, new Rectangle((int)Physics.Position.X - _size / 2, (int)Physics.Position.Y - _size / 2, _size, _size), 0, 360);
        }

        protected override void UpdateModelInternal(TimeSpan timeSinceLastUpdate, List<ThingBase> otherThings)
        {
        }
    }
}
