﻿using System;
using System.Collections.Generic;
using System.Drawing;
using HelloGame.Common.MathStuff;
using HelloGame.Common.Settings;

namespace HelloGame.Common.Model.GameObjects
{
    public class BigMass : ThingBase
    {
        public Color Color { get; private set; }

        public BigMass(ThingBaseInjections injections, int size, int? id, ThingBase creator,
            Color? color, ElapsingThingSettings elapsingThingSettings = null)
            : base(injections, ThingSettings.GetBigMassSettings(elapsingThingSettings), creator, id)
        {
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
                Color = Color.FromArgb(Math.Min(Color.R + 10, 255), Color.G, Color.B);
            }
        }

        protected override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color),
                new Rectangle((int) (Physics.Position.X - Physics.Size/2), (int) (Physics.Position.Y - Physics.Size/2),
                    (int) Physics.Size, (int) Physics.Size));
        }

        protected override void UpdateModelInternal(TimeSpan timeSinceLastUpdate, IEnumerable<ThingBase> otherThings)
        {
        }
    }
}