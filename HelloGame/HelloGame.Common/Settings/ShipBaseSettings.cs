using System;
using System.Collections.Generic;
using HelloGame.Common.Model;
using HelloGame.Common.Model.GameObjects.Ships;

namespace HelloGame.Common.Settings
{
    public class ShipBaseSettings : ThingSettings
    {
        public decimal MaxEnginePower { get; set; }
        public decimal MaxInteria { get; set; }
        public TimeSpan DespawnTime { get; set; }
        public int PointsForKill { get; set; }

        private static readonly ShipBaseSettings SmallFast = new ShipBaseSettings
        {
            Aerodynamism = 0.1m,
            TimeToLive = ThingSettings.LiveForever,
            Mass = 2,
            RadPerSecond = (decimal) Math.PI,
            LazerLimit = TimeSpan.FromMilliseconds(1000),
            MaxEnginePower = 5,
            MaxInteria = 5,
            DespawnTime = TimeSpan.FromSeconds(5),
            PointsForKill = 4,
            Size = 10
        };

        private static readonly ShipBaseSettings BigSlow = new ShipBaseSettings
        {
            Aerodynamism = 0.1m,
            TimeToLive = ThingSettings.LiveForever,
            Mass = 5,
            RadPerSecond = (decimal) Math.PI*3/4,
            LazerLimit = TimeSpan.FromMilliseconds(150),
            MaxEnginePower = 4,
            MaxInteria = 5,
            DespawnTime = TimeSpan.FromSeconds(5),
            PointsForKill = 4,
            Size = 30
        };

        private static readonly ShipBaseSettings Balanced = new ShipBaseSettings
        {
            Aerodynamism = 0.1m,
            TimeToLive = ThingSettings.LiveForever,
            Mass = 3,
            RadPerSecond = (decimal) Math.PI,
            LazerLimit = TimeSpan.FromMilliseconds(300),
            MaxEnginePower = 5,
            MaxInteria = 5,
            DespawnTime = TimeSpan.FromSeconds(5),
            PointsForKill = 4,
            Size = 15,
            LazerSpeed = 50
        };

        public static readonly Dictionary<ShipSettingType, ShipBaseSettings> ShipTypeSettings = new Dictionary
            <ShipSettingType, ShipBaseSettings>
        {
            {ShipSettingType.BigSlow, BigSlow},
            {ShipSettingType.Balanced, Balanced},
            {ShipSettingType.SmallFast, SmallFast}
        };

        public static readonly Dictionary<ClanEnum, ShipBaseSettings> ClanShipSettings = new Dictionary
            <ClanEnum, ShipBaseSettings>
        {
            {ClanEnum.Integrations, ShipTypeSettings[ShipSettingType.SmallFast]},
            {ClanEnum.RMS, ShipTypeSettings[ShipSettingType.BigSlow]},
            {ClanEnum.Support, ShipTypeSettings[ShipSettingType.Balanced]}
        };
    }
}