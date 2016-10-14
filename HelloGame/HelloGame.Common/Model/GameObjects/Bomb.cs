using System;
using System.Collections.Generic;
using System.Drawing;
using HelloGame.Common.MathStuff;
using HelloGame.Common.Settings;
using HelloGame.Common.Extensions;

namespace HelloGame.Common.Model.GameObjects
{
    public class Bomb : ThingBase
    {
        public bool IsArmed { get; private set; }

        public Bomb(ThingBaseInjections injections, int? id, ThingAdditionalInfo additionalInfo,
            ElapsingThingSettings elapsingThingSettings = null)
            : base(injections, ThingSettings.GetBombSettings(elapsingThingSettings), additionalInfo, id)
        {
            Physics.Interia = Vector2D.Zero();
        }

        protected override void Render(Graphics g)
        {
            if (!IsDestroyed)
            {
                g.DrawString("Boom boom.", Font, Brushes.Purple, Physics.PositionPoint.X, Physics.PositionPoint.Y + 10);

                int r = (int) (AgePercentage*2.5);

                Pen pen = new Pen(Color.FromArgb(Math.Min(255, r), 0, 0));
                int width = 5;

                if (!IsArmed)
                {
                    g.DrawCircle(Physics.PositionPoint, width/2, pen.Color);
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
            Destroy(TimeSpan.FromSeconds(2), null);
        }
    }
}