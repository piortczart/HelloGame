﻿using System;
using System.Collections.Generic;
using System.Drawing;
using HelloGame.Common.MathStuff;
using HelloGame.Common.Settings;

namespace HelloGame.Common.Model.GameObjects
{
    public class Bomb : ThingBase
    {
        public bool IsArmed { get; private set; }

        public Bomb(ThingBaseInjections injections, int? id, ThingAdditionalInfo additionalInfo,
            ElapsingThingSettings elapsingThingSettings = null)
            : base(injections, ThingSettings.GetBombSettings(elapsingThingSettings), additionalInfo, id)
        {
            Physics.Interia = new Real2DVector();
        }

        protected override void Render(Graphics g)
        {
            if (!IsDestroyed)
            {
                g.DrawString("Alive!", Font, Brushes.Purple, Physics.PositionPoint.X, Physics.PositionPoint.Y + 10);

                int r = (int) (AgePercentage*2.5);

                Pen pen = new Pen(Color.FromArgb(Math.Min(255, r), 0, 0));
                int width = 5;

                if (!IsArmed)
                {
                    g.DrawArc(pen,
                        new Rectangle((int) Physics.Position.X - width/2, (int) Physics.Position.Y - width/2, width,
                            width), 0, 360);
                }
                else
                {
                    g.FillEllipse(pen.Brush,
                        new Rectangle((int) Physics.Position.X - width/2, (int) Physics.Position.Y - width/2, width,
                            width));
                }
            }
            else
            {
                g.DrawString("Destroyed", Font, Brushes.Purple, Physics.PositionPoint.X, Physics.PositionPoint.Y + 10);

                int width = 50;
                g.FillEllipse(Brushes.Yellow,
                    new Rectangle((int) Physics.Position.X - width/2, (int) Physics.Position.Y - width/2, width, width));
            }
        }

        protected override void UpdateModelInternal(TimeSpan timeSinceLastUpdate, IEnumerable<ThingBase> otherThings)
        {
            if (!IsArmed && Age > TimeSpan.FromSeconds(0.5))
            {
                IsArmed = true;
            }

            if (AgePercentage >= 50)
            {
                GoBoom();
            }
        }

        protected override void CollidesWithInternal(ThingBase other)
        {
            // Collided with ANYTHING? Destroy it.
            if (IsArmed)
            {
                GoBoom();
            }
        }

        private void GoBoom()
        {
            Destroy(TimeSpan.FromSeconds(2));
        }
    }
}