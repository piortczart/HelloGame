﻿using System;
using System.Collections.Generic;
using System.Drawing;
using HelloGame.Common.Settings;

namespace HelloGame.Common.Model.GameObjects
{
    public class LazerBeamPew : ThingBase
    {
        public LazerBeamPew(ThingBaseInjections injections, ThingAdditionalInfo additionalInfo, int? id,
            ElapsingThingSettings elapsingThingSettings = null)
            : base(injections, ThingSettings.GetLazerBeamSettings(elapsingThingSettings), additionalInfo, id)
        {
        }

        protected override void CollidesWithInternal(ThingBase other)
        {
            // Lazer gets destroyed on contact with anything (but not the shooter).
            if (other != Creator)
            {
                Destroy(TimeSpan.Zero, other);
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