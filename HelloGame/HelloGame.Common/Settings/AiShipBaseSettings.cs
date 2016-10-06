using System;
using System.Collections.Generic;

namespace HelloGame.Common.Settings
{
    public class AiShipSettings
    {
        private static AiShipSettings RegularAi => new AiShipSettings
        {
            LocatePlayerFrequency = TimeSpan.FromSeconds(1),
            DistanceToPlayer = 100
        };

        private static AiShipSettings StupidAi => new AiShipSettings
        {
            LocatePlayerFrequency = TimeSpan.FromSeconds(10),
            DistanceToPlayer = null
        };

        public static Dictionary<AiType, AiShipSettings> AiSettings = new Dictionary<AiType, AiShipSettings>
        {
            {AiType.Regular, RegularAi},
            {AiType.Stupid, StupidAi}
        };

        public int? DistanceToPlayer { get; set; }
        public TimeSpan LocatePlayerFrequency { get; set; }
    }
}