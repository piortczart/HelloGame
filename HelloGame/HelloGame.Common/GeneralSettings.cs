using HelloGame.Common.Model;
using HelloGame.Common.Model.GameObjects.Ships;
using System;
using System.Collections.Generic;

namespace HelloGame.Common
{
    public class GeneralSettings
    {
        public static GeneralSettings Gameplay => new GeneralSettings
        {
            ShowThingIds = false,
            IsAiHostile = true,
            SpawnAi = true,
            ShowPlayerPhysicsDetails = false,
            GravityFactor = 0.01m
        };
        public static GeneralSettings TestingAll => new GeneralSettings
        {
            ShowThingIds = true,
            IsAiHostile = false,
            SpawnAi = false,
            ShowPlayerPhysicsDetails = true,
            GravityFactor = 0.01m
        };
        public static GeneralSettings Custom => new GeneralSettings
        {
            ShowThingIds = false,
            IsAiHostile = true,
            SpawnAi = false,
            ShowPlayerPhysicsDetails = false,
            GravityFactor = 0.01m
        };

        public bool ShowThingIds { get; set; }
        public bool IsAiHostile { get; set; }
        public bool SpawnAi { get; set; }
        public bool ShowPlayerPhysicsDetails { get; set; }
        public decimal GravityFactor { get; internal set; }

        public ThingSettings LazerBeamSettings => new ThingSettings
        {
            Aerodynamism = 0,
            TimeToLive = TimeSpan.FromSeconds(3)
        };

        public ThingSettings BigMassSettings => new ThingSettings
        {
            Aerodynamism = int.MaxValue,
            TimeToLive = ThingSettings.LiveForever,
            CanBeMoved = false
        };

        public AiShipBaseSettings AiShipBaseSettings => new AiShipBaseSettings
        {
            Aerodynamism = 0.1m,
            TimeToLive = ThingSettings.LiveForever,
            Mass = 0,
            RadPerSecond = (decimal)Math.PI,
            LazerLimit = TimeSpan.FromSeconds(3),
            LocatePlayerFrequency = TimeSpan.FromSeconds(1),
            MaxEnginePower = 1,
            MaxInteria = 1,
            DespawnTime = TimeSpan.FromSeconds(5),
            DistanceToPlayer = 100,
            PointsForKill = 2,
            Size = 15,
            CollidesWithPlanets = false
        };

        public readonly Dictionary<ClanEnum, ShipBaseSettings> ClanSettings = new Dictionary<ClanEnum, ShipBaseSettings>
        {
            {
                ClanEnum.Integrations, new ShipBaseSettings
                {
                                Aerodynamism = 0.1m,
            TimeToLive = ThingSettings.LiveForever,
            Mass = 2,
            RadPerSecond = (decimal)Math.PI,
            LazerLimit = TimeSpan.FromMilliseconds(1000),
            MaxEnginePower = 5,
            MaxInteria = 5,
            DespawnTime = TimeSpan.FromSeconds(5),
            PointsForKill = 4,
            Size = 10
                }
            },
            {
                ClanEnum.RMS, new ShipBaseSettings {
                                Aerodynamism = 0.1m,
            TimeToLive = ThingSettings.LiveForever,
            Mass = 5,
            RadPerSecond = (decimal)Math.PI * 3/4,
            LazerLimit = TimeSpan.FromMilliseconds(150),
            MaxEnginePower = 4,
            MaxInteria = 5,
            DespawnTime = TimeSpan.FromSeconds(5),
            PointsForKill = 4,
            Size = 30
                }
            },
                        {
                ClanEnum.Support, new ShipBaseSettings {
                                Aerodynamism = 0.1m,
            TimeToLive = ThingSettings.LiveForever,
            Mass = 3,
            RadPerSecond = (decimal)Math.PI,
            LazerLimit = TimeSpan.FromMilliseconds(300),
            MaxEnginePower = 5,
            MaxInteria = 5,
            DespawnTime = TimeSpan.FromSeconds(5),
            PointsForKill = 4,
            Size = 15,
            LazerSpeed = 50
                }
            }
        };
    }
}
