using System;
using System.Collections.Generic;
using System.Drawing;
using HelloGame.Common.Settings;

namespace HelloGame.Common.Model.GameObjects
{
    public class LazerBeamPew : ThingBase
    {
        public LazerBeamPew(ThingBaseInjections injections, ThingBase creator, int? id,
            ElapsingThingSettings elapsingThingSettings = null)
            : base(injections, ThingSettings.GetLazerBeamSettings(elapsingThingSettings), creator, id)
        {
        }

        public override void CollidesWith(ThingBase other)
        {
            if (other != Creator)
            {
                Destroy(TimeSpan.Zero);
            }
        }

        protected override void Render(Graphics g)
        {
            var pen = new Pen(Brushes.Red);

            Point pointInDirection = Physics.GetPointInDirection(5);
            g.DrawLine(pen, Physics.PositionPoint, pointInDirection);
        }

        protected override void UpdateModelInternal(TimeSpan timeSinceLastUpdate, IEnumerable<ThingBase> otherThings)
        {
        }
    }
}