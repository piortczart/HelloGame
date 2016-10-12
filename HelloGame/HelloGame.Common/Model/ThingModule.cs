using System;

namespace HelloGame.Common.Model
{
    public class Modules
    {
        public Shield Shield { get; set; }
    }

    public class Shield
    {
        public decimal Percentage { get { return Max == 0 ? 0 : 100 * Current / Max; } }

        public decimal Max { get; set; }
        public decimal Current { get; set; }
        public TimeSpan Tick { get; set; }
        public static Shield BasicShield => new Shield(2);

        public Shield(decimal max)
        {
            Max = max;
            Current = max;
        }

        public bool DamageDealt(decimal damage)
        {
            if (damage == -1)
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
            return Current == 0;
        }

        internal void Add()
        {
            Current += 1;
            // Make sure we are not over max.
            Current = Math.Max(Current, Max);
        }
    }

}
