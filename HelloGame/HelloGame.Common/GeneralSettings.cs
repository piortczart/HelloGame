using HelloGame.Common.Model;
using HelloGame.Common.Model.GameObjects.Ships;
using System;

namespace HelloGame.Common
{
    public class GeneralSettings
    {
        public static GeneralSettings Gameplay => new GeneralSettings
        {
            ShowThingIds = false,
            IsAiHostile = true,
            SpawnAi = true,
            ShowPlayerPhysicsDetails = false
        };
        public static GeneralSettings TestingAll => new GeneralSettings
        {
            ShowThingIds = true,
            IsAiHostile = false,
            SpawnAi = false,
            ShowPlayerPhysicsDetails = true
        };
        public static GeneralSettings Custom => new GeneralSettings
        {
            ShowThingIds = false,
            IsAiHostile = true,
            SpawnAi = false,
            ShowPlayerPhysicsDetails = false
        };

        public bool ShowThingIds { get; set; }
        public bool IsAiHostile { get; set; }
        public bool SpawnAi { get; set; }
        public bool ShowPlayerPhysicsDetails { get; set; }

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
            Mass = 3,
            RadPerSecond = (decimal)Math.PI,
            LazerLimit = TimeSpan.FromSeconds(3),
            LocatePlayerFrequency = TimeSpan.FromSeconds(1),
            MaxEnginePower = 5,
            MaxInteria = 3,
            DespawnTime = TimeSpan.FromSeconds(5),
            DistanceToPlayer = 100,
            PointsForKill = 2
        };

        public ShipBaseSettings PlayerShipBaseSettings => new ShipBaseSettings
        {
            Aerodynamism = 0.1m,
            TimeToLive = ThingSettings.LiveForever,
            Mass = 3,
            RadPerSecond = (decimal)Math.PI,
            LazerLimit = TimeSpan.FromMilliseconds(200),
            MaxEnginePower = 5,
            MaxInteria = 5,
            DespawnTime = TimeSpan.FromSeconds(5),
            PointsForKill = 4
        };
    }
}
