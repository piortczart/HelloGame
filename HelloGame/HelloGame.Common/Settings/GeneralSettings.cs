using System;
using HelloGame.Common.Model;

namespace HelloGame.Common.Settings
{
    public class GeneralSettings
    {
        public bool ShowThingIds { get; set; }
        public bool IsAiHostile { get; set; }
        public bool SpawnAi { get; set; }
        public bool ShowPlayerPhysicsDetails { get; set; }
        public decimal GravityFactor { get; set; }
        public decimal CollisionTolerance { get; set; }

        public static GeneralSettings Gameplay => new GeneralSettings
        {
            ShowThingIds = false,
            IsAiHostile = true,
            SpawnAi = true,
            ShowPlayerPhysicsDetails = false,
            GravityFactor = 0.01m,
            CollisionTolerance = 0
        };

        public static GeneralSettings TestingAll => new GeneralSettings
        {
            ShowThingIds = true,
            IsAiHostile = false,
            SpawnAi = false,
            ShowPlayerPhysicsDetails = true,
            GravityFactor = 0.01m,
            CollisionTolerance = 0
        };

        public static GeneralSettings Custom => new GeneralSettings
        {
            ShowThingIds = false,
            IsAiHostile = true,
            SpawnAi = false,
            ShowPlayerPhysicsDetails = false,
            GravityFactor = 0.01m,
            CollisionTolerance = 0
        };

        public static ThingSettings LazerBeamSettings => new ThingSettings
        {
            Aerodynamism = 0,
            TimeToLive = TimeSpan.FromSeconds(3)
        };

        public static ThingSettings BigMassSettings => new ThingSettings
        {
            Aerodynamism = int.MaxValue,
            TimeToLive = ThingSettings.LiveForever,
            CanBeMoved = false
        };
    }
}