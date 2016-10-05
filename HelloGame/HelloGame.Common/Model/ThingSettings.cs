using System;

namespace HelloGame.Common.Model
{
    /// <summary>
    /// General settings of a Thing.
    /// </summary>
    public class ThingSettings
    {
        public static readonly TimeSpan LiveForever = TimeSpan.FromMilliseconds(-1);

        public TimeSpan TimeToLive { get; set; }
        public decimal Aerodynamism { get; set; }
        public decimal Mass { get; set; }
        public decimal RadPerSecond { get; set; }
        public bool CanBeMoved { get; set; } = true;
        public decimal Size { get; set; } = 1;
        public TimeSpan LazerLimit { get; set; } = TimeSpan.FromSeconds(2);
    }
}