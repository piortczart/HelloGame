namespace HelloGame.Common.Model
{
    public class Weapons
    {
        public static Weapons BasicWeapons = new Weapons
        {
            Main = new Weapon
            {
                WeaponType = WeaponType.Lazer
            },
            Secondary = new Weapon
            {
                WeaponType = WeaponType.Bomb
            }
        };

        public Weapon Main { get; set; }
        public Weapon Secondary { get; set; }
    }
}