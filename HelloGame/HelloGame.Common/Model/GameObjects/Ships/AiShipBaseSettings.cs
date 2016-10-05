using System;

namespace HelloGame.Common.Model.GameObjects.Ships
{
    public class AiShipBaseSettings : ShipBaseSettings
    {
        public int DistanceToPlayer { get; set; }
        public TimeSpan LocatePlayerFrequency { get; set; }
    }
}