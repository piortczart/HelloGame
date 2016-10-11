using System;
using HelloGame.Common.Settings;
using HelloGame.Common.TimeStuffs;

namespace HelloGame.Common.Model
{
    public class Weapon
    {
        public WeaponType WeaponType { get; set; }
        public int WeaponLevel { get; set; }
        public TimeSpan LastShotTime { get; set; }

        public bool CanShoot(TimeSource timeSource, ThingSettings shooterSettings)
        {
            var frequency = shooterSettings.GetWeaponFrequency(WeaponType);
            var nextShotPossible = LastShotTime.Add(frequency);
            return nextShotPossible <= timeSource.ElapsedSinceStart;
        }
    }
}