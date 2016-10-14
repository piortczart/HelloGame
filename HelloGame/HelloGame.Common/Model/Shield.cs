using System;
using HelloGame.Common.MathStuff;

namespace HelloGame.Common.Model
{
    public class Shield
    {
        public float Percentage => MathX.IsAlmostZero(Max) ? 0 : 100*Current/Max;

        public float Max { get; set; }
        public float Current { get; set; }
        public TimeSpan Tick { get; set; }

        public Shield(float max)
        {
            Max = max;
            Current = max;
        }

        public bool DamageDealt(float damage)
        {
            if (MathX.IsAlmostSame(damage, -1))
            {
                Current = 0;
            }
            else
            {
                if (Current > 0)
                {
                    Current -= damage;
                }
                if (Current < 0)
                {
                    Current = 0;
                }
            }
            return MathX.IsAlmostZero(Current);
        }

        internal void Add()
        {
            Current += 1;
            // Make sure we are not over max.
            Current = Math.Max(Current, Max);
        }
    }
}