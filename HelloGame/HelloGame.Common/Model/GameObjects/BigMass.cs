using System;
using System.Collections.Generic;
using System.Drawing;
using HelloGame.Common.MathStuff;
using HelloGame.Common.Settings;

namespace HelloGame.Common.Model.GameObjects
{
    public class BigMass : ThingBase
    {
        public Color Color { get; private set; }
        public int DeathsCaused { get; set; }

        public override ThingAdditionalInfo ThingSerializationExtras
        {
            get
            {
                var result = base.ThingSerializationExtras;
                result.DeathsCaused = DeathsCaused;
                return result;
            }
        }

        public BigMass(ThingBaseInjections injections, int size, int? id, ThingAdditionalInfo additionalInfo,
            Color? color, ElapsingThingSettings elapsingThingSettings = null)
            : base(injections, ThingSettings.GetBigMassSettings(elapsingThingSettings), additionalInfo, id)
        {
            DeathsCaused = additionalInfo.DeathsCaused ?? 0;
            Physics.Size = size;
            Physics.Mass = size*10000;
            Color = color ?? GetRandom();
        }

        private static Color GetRandom()
        {
            return Color.FromArgb(MathX.Random.Next(0, 255), MathX.Random.Next(0, 255), MathX.Random.Next(0, 255));
        }

        protected override void CollidesWithInternal(ThingBase other)
        {
            if (!other.IsDestroyed)
            {
                DeathsCaused++;
                Color = Color.FromArgb(Math.Min(Color.R + 10, 255), Color.G, Color.B);
            }
        }

        protected override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color),
                new Rectangle((int) (Physics.Position.X - Physics.Size/2), (int) (Physics.Position.Y - Physics.Size/2),
                    (int) Physics.Size, (int) Physics.Size));

            g.DrawString(DeathsCaused.ToString(), Font, new SolidBrush(Color.FromArgb(Color.ToArgb() ^ 0xffffff)),
                new PointF((int) Physics.Position.X, (int) Physics.Position.Y));
        }

        protected override void UpdateModelInternal(TimeSpan timeSinceLastUpdate, IEnumerable<ThingBase> otherThings)
        {
        }
    }
}