using System;
using System.Collections.Generic;
using System.Drawing;

namespace HelloGame.GameObjects
{
    public class BigMass : ThingBase
    {
        private Color color = GetRandom();
        private int _size;
        
        static readonly ThingSettings settings = new ThingSettings()
        {
            Aerodynamism = int.MaxValue,
            TimeToLive = TimeSpan.Zero
        };

        private static Color GetRandom()
        {
            return Color.FromArgb(MathX.Random.Next(0, 255), MathX.Random.Next(0, 255), MathX.Random.Next(0, 255));
        }

        public BigMass(int size) : base(settings)
        {
            _size = size;
            Physics.Mass = size * 10000;
        }

        public override void CollidesWith(ThingBase other)
        {
            color = GetRandom();
        }

        public override void PaintStuff(Graphics g)
        {
            var shipPen = new Pen(Brushes.Red);

            g.DrawArc(shipPen, new Rectangle((int)Physics.PositionX - _size / 2, (int)Physics.PositionY - _size / 2, _size, _size), 0, 360);
        }

        protected override void UpdateModelInternal(TimeSpan timeSinceLastUpdate, List<ThingBase> otherThings)
        {
        }
    }
}
