using System;
using System.Collections.Generic;
using System.Drawing;
using HelloGame.Common.Logging;

namespace HelloGame.Common.Model.GameObjects
{
    public class LazerBeamPew : ThingBase
    {
        public bool IsArmed { get; private set; }

        private static readonly ThingSettings Settings = new ThingSettings
        {
            Aerodynamism = 0,
            TimeToLive = TimeSpan.FromSeconds(1)
        };

        public LazerBeamPew(ILogger logger, ThingBase creator, int? id) : base(logger, Settings, creator, id)
        {
        }

        public override void CollidesWith(ThingBase other)
        {
            if (IsArmed)
            {
                Destroy(TimeSpan.Zero);
            }
        }

        public override void Render(Graphics g)
        {
            var pen = new Pen(Brushes.Red);

            Point pointInDirection = Physics.GetPointInDirection(5);
            g.DrawLine(pen, Physics.PositionPoint, pointInDirection);
        }

        protected override void UpdateModelInternal(TimeSpan timeSinceLastUpdate, IEnumerable<ThingBase> otherThings)
        {
            if (!IsArmed && Age > TimeSpan.FromSeconds(0.5))
            {
                IsArmed = true;
            }
        }
    }
}
