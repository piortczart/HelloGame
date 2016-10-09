using System;
using System.Collections.Generic;
using HelloGame.Common.Extensions;

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

        private static readonly string[] PossibleAiNames =
        {
            "The Forecast",
            "Return 1000",
            "Wichura",
            "Czeœæ K&K",
            "TFS task 1209",
            "PO-1561",
            "Tupot Stopy",
            "Tigerek z Rana",
            "I tak i nie",
            "Muzk",
            "Satoria Protos",
            "Brilliant",
            "Mag Andrzej",
            "Tato",
            "Seba Prawdziwy",
            "Seba Pierwszy",
            "Huragan",
            "Visual Studio 2017",
            "while (true) {}",
            "TFS ¯yje",
            "Data Sajentist",
            "Polska Gooola!"
        };

        public static string GetRandomAiShipName()
        {
            return PossibleAiNames.GetRandomItem();
        }
    }
}