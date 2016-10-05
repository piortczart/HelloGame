using System;
using System.Collections.Generic;
using System.Drawing;
using HelloGame.Common.MathStuff;

namespace HelloGame.Common.Model.GameObjects
{
    public class BigMass : ThingBase
    {
        private static Color GetRandom()
        {
            return Color.FromArgb(MathX.Random.Next(0, 255), MathX.Random.Next(0, 255), MathX.Random.Next(0, 255));
        }

        public BigMass(ThingBaseInjections injections, int size, int? id, ThingBase creator) : base(injections, injections.GeneralSettings.BigMassSettings, creator, id)
        {
            Physics.Size = size;
            Physics.Mass = size * 10000;
        }

        public override void CollidesWith(ThingBase other)
        {
            GetRandom();
        }

        protected override void Render(Graphics g)
        {
            var pen = new Pen(GetRandom());
            g.FillEllipse(Brushes.DarkRed, new Rectangle((int)(Physics.Position.X - Physics.Size / 2), (int)(Physics.Position.Y - Physics.Size / 2), (int)Physics.Size, (int)Physics.Size));
        }

        protected override void UpdateModelInternal(TimeSpan timeSinceLastUpdate, IEnumerable<ThingBase> otherThings)
        {
        }
    }
}
