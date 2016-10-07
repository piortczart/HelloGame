using System;
using HelloGame.Common.Model;

namespace HelloGame.Common.Settings
{
    public class GeneralSettings
    {
        public bool ShowTimeToLive { get; private set; }
        public bool ShowThingIds { get; private set; }
        public bool IsAiHostile { get; private set; }
        public bool SpawnAi { get; private set; }
        public bool ShowPlayerPhysicsDetails { get; private set; }
        public decimal GravityFactor { get; private set; }
        public decimal CollisionTolerance { get; private set; }

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
            ShowTimeToLive = true,
            ShowThingIds = true,
            IsAiHostile = true,
            SpawnAi = false,
            ShowPlayerPhysicsDetails = false,
            GravityFactor = 0.01m,
            CollisionTolerance = 0,
        };
    }
}