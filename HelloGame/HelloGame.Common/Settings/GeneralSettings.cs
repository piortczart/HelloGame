﻿using System.Drawing;

namespace HelloGame.Common.Settings
{
    public class GeneralSettings
    {
        public bool ShowTimeToLive { get; set; }
        public bool ShowThingIds { get; set; }
        public bool IsAiHostile { get; set; }
        public bool SpawnAi { get; set; }
        public bool ShowPlayerPhysicsDetails { get; set; }
        public decimal GravityFactor { get; set; } = 0.01m;
        public decimal CollisionTolerance { get; set; } = 0;
        public Size GameSize { get; set; } = new Size(2000, 2000);
        public int PlanetsCount { get; set; } = 2;
        public int AiShipCount { get; set; } = 3;

        public static GeneralSettings Gameplay => new GeneralSettings
        {
            ShowThingIds = false,
            IsAiHostile = true,
            SpawnAi = true,
            ShowPlayerPhysicsDetails = false,
        };

        public static GeneralSettings TestingAll => new GeneralSettings
        {
            ShowThingIds = true,
            IsAiHostile = false,
            SpawnAi = false,
            ShowPlayerPhysicsDetails = true,
        };

        public static GeneralSettings Custom => new GeneralSettings
        {
            ShowTimeToLive = true,
            ShowThingIds = true,
            IsAiHostile = true,
            SpawnAi = false,
            ShowPlayerPhysicsDetails = false,
        };
    }
}