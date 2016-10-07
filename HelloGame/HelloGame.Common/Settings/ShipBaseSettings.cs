using System;
using HelloGame.Common.Model;
using HelloGame.Common.Model.GameObjects;
using HelloGame.Common.Model.GameObjects.Ships;

namespace HelloGame.Common.Settings
{
    public class ShipBaseSettings : ThingSettings
    {
        public decimal MaxEnginePower { get; set; }
        public decimal MaxInteria { get; set; }
        public TimeSpan DespawnTime { get; set; }
        public int PointsForKill { get; set; }

        private ShipBaseSettings(TimeSpan? spawnedAt) : base(spawnedAt)
        {
        }

        private static ShipBaseSettings SmallFast(ElapsingThingSettings elapsingThingSettings) =>
            new ShipBaseSettings(elapsingThingSettings.SpawnedAt)
            {
                Aerodynamism = 0.1m,
                TimeToLive = elapsingThingSettings.TimeToLive ?? LiveForever,
                Mass = 2,
                RadPerSecond = (decimal) Math.PI,
                LazerLimit = TimeSpan.FromMilliseconds(1000),
                MaxEnginePower = 5,
                MaxInteria = 5,
                DespawnTime = TimeSpan.FromSeconds(5),
                PointsForKill = 4,
                Size = 10
            };

        private static ShipBaseSettings BigSlow(ElapsingThingSettings elapsingThingSettings) =>
            new ShipBaseSettings(elapsingThingSettings.SpawnedAt)
            {
                Aerodynamism = 0.1m,
                TimeToLive = elapsingThingSettings.TimeToLive ?? LiveForever,
                Mass = 5,
                RadPerSecond = (decimal) Math.PI*3/4,
                LazerLimit = TimeSpan.FromMilliseconds(150),
                MaxEnginePower = 4,
                MaxInteria = 5,
                DespawnTime = TimeSpan.FromSeconds(5),
                PointsForKill = 4,
                Size = 30
            };

        private static ShipBaseSettings Balanced(ElapsingThingSettings elapsingThingSettings) =>
            new ShipBaseSettings(elapsingThingSettings.SpawnedAt)
            {
                Aerodynamism = 0.1m,
                TimeToLive = elapsingThingSettings.TimeToLive ?? LiveForever,
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


        public static ShipBaseSettings GetShipTypeSettings(ShipSettingType shipSettingType,
            ElapsingThingSettings ets)
        {
            if (ets == null)
            {
                ets = new ElapsingThingSettings();
            }
            switch (shipSettingType)
            {
                case ShipSettingType.BigSlow:
                    return BigSlow(ets);
                case ShipSettingType.Balanced:
                    return Balanced(ets);
                case ShipSettingType.SmallFast:
                    return SmallFast(ets);
                default:
                    throw new ArgumentOutOfRangeException(nameof(shipSettingType), shipSettingType, null);
            }
        }

        public static ShipBaseSettings GetClanShipSetting(ClanEnum clan, ElapsingThingSettings elapsingThingSettings)
        {
            switch (clan)
            {
                case ClanEnum.RMS:
                    return GetShipTypeSettings(ShipSettingType.BigSlow, elapsingThingSettings);
                case ClanEnum.Integrations:
                    return GetShipTypeSettings(ShipSettingType.SmallFast, elapsingThingSettings);
                case ClanEnum.Support:
                    return GetShipTypeSettings(ShipSettingType.Balanced, elapsingThingSettings);
                default:
                    throw new ArgumentOutOfRangeException(nameof(clan), clan, null);
            }
        }
    }
}