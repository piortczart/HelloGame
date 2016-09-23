using System;
using System.Collections.Generic;
using System.Drawing;

namespace HelloGame.Common.Model.GameObjects
{
    public class LazerBeamPew : ThingBase
    {
        private static readonly ThingSettings Settings = new ThingSettings
        {
            Aerodynamism = 0,
            TimeToLive = TimeSpan.FromSeconds(1)
        };

        public LazerBeamPew(ThingBase creator) : base(Settings, creator)
        {
        }

        public override void CollidesWith(ThingBase other)
        {
            Destroy(TimeSpan.Zero);
        }

        public override void PaintStuff(Graphics g)
        {
            var pen = new Pen(Brushes.Red);

            Point pointInDirection = Physics.GetPointInDirection(5);
            g.DrawLine(pen, Physics.PositionPoint, pointInDirection);
        }

        protected override void UpdateModelInternal(TimeSpan timeSinceLastUpdate, List<ThingBase> otherThings)
        {
        }
    }
}
