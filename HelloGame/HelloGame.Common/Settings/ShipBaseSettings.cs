using System;
using System.Collections.Generic;
using HelloGame.Common.Model;
using HelloGame.Common.Model.GameObjects.Ships;

namespace HelloGame.Common.Settings
{
    public class ShipBaseSettings : ThingSettings
    {
        public float MaxEnginePower { get; set; }
        public float MaxInteria { get; set; }
        public TimeSpan DespawnTime { get; set; } = TimeSpan.FromSeconds(2);
        public int PointsForKill { get; set; }
        public TimeSpan RespawnTime { get; set; } = TimeSpan.FromSeconds(2);

        private ShipBaseSettings(TimeSpan? spawnedAt) : base(spawnedAt)
        {
        }

        private static ShipBaseSettings SmallFast(ElapsingThingSettings elapsingThingSettings)
            => new ShipBaseSettings(elapsingThingSettings.SpawnedAt)
            {
                Aerodynamism = 0.1f,
                TimeToLive = elapsingThingSettings.TimeToLive ?? LiveForever,
                Mass = 2,
                RadPerSecond = (float) Math.PI,
                WeaponFrequencies = new Dictionary<WeaponType, TimeSpan>
                {
                    {WeaponType.Lazer, TimeSpan.FromMilliseconds(1000)},
                    {WeaponType.Bomb, TimeSpan.FromMilliseconds(2000)}
                },
                MaxEnginePower = 5,
                MaxInteria = 5,
                PointsForKill = 4,
                Size = 10,
                RespawnTime = TimeSpan.FromSeconds(3),
                InitialShield = new Shield(0)
            };

        private static ShipBaseSettings BigSlow(ElapsingThingSettings elapsingThingSettings)
            => new ShipBaseSettings(elapsingThingSettings.SpawnedAt)
            {
                Aerodynamism = 0.1f,
                TimeToLive = elapsingThingSettings.TimeToLive ?? LiveForever,
                Mass = 5,
                RadPerSecond = (float) Math.PI*3/4,
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
                RespawnTime = TimeSpan.FromSeconds(7),
                InitialShield = new Shield(2)
            };

        private static ShipBaseSettings Balanced(ElapsingThingSettings elapsingThingSettings)
            => new ShipBaseSettings(elapsingThingSettings.SpawnedAt)
            {
                Aerodynamism = 0.1f,
                TimeToLive = elapsingThingSettings.TimeToLive ?? LiveForever,
                Mass = 3,
                RadPerSecond = (float) Math.PI,
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
                RespawnTime = TimeSpan.FromSeconds(5),
                InitialShield = new Shield(1)
            };


        public static ShipBaseSettings GetShipTypeSettings(ShipSettingType shipSettingType, ElapsingThingSettings ets,
            Type source)
        {
            if (ets == null)
            {
                ets = new ElapsingThingSettings();
            }
            ShipBaseSettings result;
            switch (shipSettingType)
            {
                case ShipSettingType.BigSlow:
                    result = BigSlow(ets);
                    break;
                case ShipSettingType.Balanced:
                    result = Balanced(ets);
                    break;
                case ShipSettingType.SmallFast:
                    result = SmallFast(ets);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(shipSettingType), shipSettingType, null);
            }
            if (source == typeof(AiShip))
            {
                result.Antigravity = true;
            }
            return result;
        }

        public static ShipBaseSettings GetClanShipSetting(ClanEnum clan, ElapsingThingSettings elapsingThingSettings,
            Type source)
        {
            switch (clan)
            {
                case ClanEnum.RMS:
                    return GetShipTypeSettings(ShipSettingType.BigSlow, elapsingThingSettings, source);
                case ClanEnum.Integrations:
                    return GetShipTypeSettings(ShipSettingType.SmallFast, elapsingThingSettings, source);
                case ClanEnum.Support:
                    return GetShipTypeSettings(ShipSettingType.Balanced, elapsingThingSettings, source);
                default:
                    throw new ArgumentOutOfRangeException(nameof(clan), clan, null);
            }
        }
    }
}