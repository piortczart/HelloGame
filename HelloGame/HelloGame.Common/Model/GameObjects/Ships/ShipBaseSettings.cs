using System;

namespace HelloGame.Common.Model.GameObjects.Ships
{
    public class ShipBaseSettings : ThingSettings
    {
        public decimal MaxEnginePower { get; set; }
        public decimal MaxInteria { get; set; }
        public TimeSpan DespawnTime { get; set; }
        public int PointsForKill { get; set; }
    }
}