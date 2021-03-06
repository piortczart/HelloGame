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
        public float GravityFactor { get; set; } = 0.01f;
        public float CollisionTolerance { get; set; } = 0;
        public Size GameSize { get; set; } = new Size(600, 600);
        public int PlanetsCount { get; set; } = 2;
        public int AiShipCount { get; set; } = 1;
        public bool ShowShipList { get; set; } = true;
        public bool Invulnerability { get; set; }

        public static readonly GeneralSettings CurrentSettings = Custom;

        public static GeneralSettings Gameplay => new GeneralSettings
        {
            ShowThingIds = false,
            IsAiHostile = true,
            SpawnAi = true,
            ShowPlayerPhysicsDetails = false,
            GameSize = new Size(2000, 2000),
            PlanetsCount = 15,
            AiShipCount = 2,
            Invulnerability = false
        };

        public static GeneralSettings TestingAll => new GeneralSettings
        {
            ShowThingIds = true,
            IsAiHostile = false,
            SpawnAi = false,
            ShowPlayerPhysicsDetails = true,
            ShowShipList = true
        };

        public static GeneralSettings Custom => new GeneralSettings
        {
            ShowTimeToLive = true,
            ShowThingIds = true,
            IsAiHostile = false,
            SpawnAi = false,
            ShowPlayerPhysicsDetails = false,
            GameSize = new Size(600, 600),
            ShowShipList = true,
            Invulnerability = false
        };
    }
}