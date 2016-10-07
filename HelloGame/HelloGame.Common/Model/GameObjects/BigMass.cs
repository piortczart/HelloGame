using System;
using System.Collections.Generic;
using System.Drawing;
using HelloGame.Common.MathStuff;
using HelloGame.Common.Settings;

namespace HelloGame.Common.Model.GameObjects
{
    public class BigMass : ThingBase
    {
        private Color _color;

        public BigMass(ThingBaseInjections injections, int size, int? id, ThingBase creator,
            ElapsingThingSettings elapsingThingSettings = null)
            : base(injections, ThingSettings.GetBigMassSettings(elapsingThingSettings), creator, id)
        {
            Physics.Size = size;
            Physics.Mass = size*10000;
            _color = GetRandom();
        }

        private static Color GetRandom()
        {
            return Color.FromArgb(MathX.Random.Next(0, 255), MathX.Random.Next(0, 255), MathX.Random.Next(0, 255));
        }

        public override void CollidesWith(ThingBase other)
        {
            if (!other.IsDestroyed)
            {
                _color = Color.FromArgb(Math.Min(_color.R + 10, 255), _color.G, _color.B);
            }
        }

        protected override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(_color),
                new Rectangle((int) (Physics.Position.X - Physics.Size/2), (int) (Physics.Position.Y - Physics.Size/2),
                    (int) Physics.Size, (int) Physics.Size));
        }

        protected override void UpdateModelInternal(TimeSpan timeSinceLastUpdate, IEnumerable<ThingBase> otherThings)
        {
        }
    }
}