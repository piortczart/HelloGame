using System;
using System.Collections.Generic;
using HelloGame.Common.Model;

namespace HelloGame.Common.Settings
{
    /// <summary>
    /// General settings of a Thing.
    /// </summary>
    public class ThingSettings
    {
        public static readonly TimeSpan LiveForever = TimeSpan.FromMilliseconds(-1);

        public TimeSpan? SpawnedAt { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public decimal Aerodynamism { get; set; }
        public decimal Mass { get; set; }
        public decimal RadPerSecond { get; set; }
        public bool CanBeMoved { get; set; } = true;
        public decimal Size { get; set; } = 1;
        public bool CollidesWithPlanets { get; set; } = true;
        public decimal LazerSpeed { get; set; } = 30;
        public int PointsForKilling { get; set; } = 1;
        public Weapons InitialWeapons { get; set; } = Weapons.BasicWeapons;
        public Shield InitialShield { get; set; }
        public TimeSpan DefaultWeaponFrequency { get; set; } = TimeSpan.FromSeconds(1);
        public decimal DamageOutput { get; set; }
        public bool Antigravity { get; set; }

        public Dictionary<WeaponType, TimeSpan> WeaponFrequencies { get; set; }
            = new Dictionary<WeaponType, TimeSpan>();

        public TimeSpan GetWeaponFrequency(WeaponType weaponType)
        {
            if (WeaponFrequencies.ContainsKey(weaponType))
            {
                return WeaponFrequencies[weaponType];
            }
            return DefaultWeaponFrequency;
        }

        public static ThingSettings GetLazerBeamSettings(ElapsingThingSettings elapsingThingSettings)
        {
            return new ThingSettings(elapsingThingSettings?.SpawnedAt)
            {
                Aerodynamism = 0,
                TimeToLive = elapsingThingSettings?.TimeToLive ?? TimeSpan.FromSeconds(3),
                DamageOutput = 1
            };
        }

        public static ThingSettings GetBigMassSettings(ElapsingThingSettings elapsingThingSettings)
        {
            return new ThingSettings(elapsingThingSettings?.SpawnedAt)
            {
                Aerodynamism = int.MaxValue,
                TimeToLive = elapsingThingSettings?.TimeToLive ?? LiveForever,
                CanBeMoved = false,
                DamageOutput = -1
            };
        }

        public static ThingSettings GetBombSettings(ElapsingThingSettings elapsingThingSettings)
        {
            return new ThingSettings(elapsingThingSettings?.SpawnedAt)
            {
                Aerodynamism = 0.1m,
                TimeToLive = elapsingThingSettings?.TimeToLive ?? TimeSpan.FromSeconds(3),
                Mass = 5,
                Size = 10,
                DamageOutput = -1
            };
        }

        protected ThingSettings(TimeSpan? spawnedAt)
        {
            SpawnedAt = spawnedAt;
        }
    }
}