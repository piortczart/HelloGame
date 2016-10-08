using System;
using HelloGame.Common.Model;

namespace HelloGame.Common.Settings
{
    /// <summary>
    /// General settings of a Thing.
    /// </summary>
    public class ThingSettings
    {
        public static readonly TimeSpan LiveForever = TimeSpan.FromMilliseconds(-1);

        public TimeSpan? SpawnedAt { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public decimal Aerodynamism { get; set; }
        public decimal Mass { get; set; }
        public decimal RadPerSecond { get; set; }
        public bool CanBeMoved { get; set; } = true;
        public decimal Size { get; set; } = 1;
        public TimeSpan LazerLimit { get; set; } = TimeSpan.FromSeconds(2);
        public bool CollidesWithPlanets { get; set; } = true;
        public decimal LazerSpeed { get; set; } = 30;
        public int PointsForKilling { get; set; } = 1;

        public static ThingSettings GetLazerBeamSettings(ElapsingThingSettings elapsingThingSettings)
        {
            return new ThingSettings(elapsingThingSettings?.SpawnedAt)
            {
                Aerodynamism = 0,
                TimeToLive = elapsingThingSettings?.TimeToLive ?? TimeSpan.FromSeconds(3)
            };
        }

        public static ThingSettings GetBigMassSettings(ElapsingThingSettings elapsingThingSettings)
        {
            return new ThingSettings(elapsingThingSettings?.SpawnedAt)
            {
                Aerodynamism = int.MaxValue,
                TimeToLive = elapsingThingSettings?.TimeToLive ?? ThingSettings.LiveForever,
                CanBeMoved = false
            };
        }

        public static ThingSettings GetBombSettings(ElapsingThingSettings elapsingThingSettings)
        {
            return new ThingSettings(elapsingThingSettings?.SpawnedAt)
            {
                Aerodynamism = 0.1m,
                TimeToLive = elapsingThingSettings?.TimeToLive ?? TimeSpan.FromSeconds(3),
                Mass = 5
            };
        }

        protected ThingSettings(TimeSpan? spawnedAt)
        {
            SpawnedAt = spawnedAt;
        }
    }
}