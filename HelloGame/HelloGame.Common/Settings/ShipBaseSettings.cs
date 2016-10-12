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
        public TimeSpan DespawnTime { get; set; } = TimeSpan.FromSeconds(2);
        public int PointsForKill { get; set; }
        public TimeSpan RespawnTime { get; set; } = TimeSpan.FromSeconds(2);

        private ShipBaseSettings(TimeSpan? spawnedAt) : base(spawnedAt)
        {
        }

        private static ShipBaseSettings SmallFast(ElapsingThingSettings elapsingThingSettings)
            => new ShipBaseSettings(elapsingThingSettings.SpawnedAt)
            {
                Aerodynamism = 0.1m,
                TimeToLive = elapsingThingSettings.TimeToLive ?? LiveForever,
                Mass = 2,
                RadPerSecond = (decimal) Math.PI,
                WeaponFrequencies = new Dictionary<WeaponType, TimeSpan>
                {
                    {WeaponType.Lazer, TimeSpan.FromMilliseconds(1000)},
                    {WeaponType.Bomb, TimeSpan.FromMilliseconds(2000)}
                },
                MaxEnginePower = 5,
                MaxInteria = 5,
                PointsForKill = 4,
                Size = 10,
                RespawnTime = TimeSpan.FromSeconds(3)
            };

        private static ShipBaseSettings BigSlow(ElapsingThingSettings elapsingThingSettings)
            => new ShipBaseSettings(elapsingThingSettings.SpawnedAt)
            {
                Aerodynamism = 0.1m,
                TimeToLive = elapsingThingSettings.TimeToLive ?? LiveForever,
                Mass = 5,
                RadPerSecond = (decimal) Math.PI*3/4,
                WeaponFrequencies = new Dictionary<WeaponType, TimeSpan>
                {
                    {WeaponType.Lazer, TimeSpan.FromMilliseconds(150)},
                    {WeaponType.Bomb, TimeSpan.FromMilliseconds(2000)}
                },
                InitialWeapons = new Weapons
                {
                    Main = new Weapon {WeaponType = WeaponType.Lazer, WeaponLevel = 1},
                    Secondary = new Weapon {WeaponType = WeaponType.Bomb, WeaponLevel = 2}
                },
                MaxEnginePower = 4,
                MaxInteria = 5,
                PointsForKill = 4,
                Size = 30,
                RespawnTime = TimeSpan.FromSeconds(7)
            };

        private static ShipBaseSettings Balanced(ElapsingThingSettings elapsingThingSettings)
            => new ShipBaseSettings(elapsingThingSettings.SpawnedAt)
            {
                Aerodynamism = 0.1m,
                TimeToLive = elapsingThingSettings.TimeToLive ?? LiveForever,
                Mass = 3,
                RadPerSecond = (decimal) Math.PI,
                WeaponFrequencies = new Dictionary<WeaponType, TimeSpan>
                {
                    {WeaponType.Lazer, TimeSpan.FromMilliseconds(300)},
                    {WeaponType.Bomb, TimeSpan.FromMilliseconds(2000)}
                },
                MaxEnginePower = 5,
                MaxInteria = 5,
                PointsForKill = 4,
                Size = 15,
                LazerSpeed = 50,
                RespawnTime = TimeSpan.FromSeconds(5)
            };


        public static ShipBaseSettings GetShipTypeSettings(ShipSettingType shipSettingType, ElapsingThingSettings ets)
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