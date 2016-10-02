using System;
using HelloGame.Common.Logging;

namespace HelloGame.Common.Model.GameObjects.Ships
{
    public class PlayerShipOther : PlayerShip
    {
        private static readonly ThingSettings Settings = new ThingSettings
        {
            Aerodynamism = 0.1m,
            TimeToLive = TimeSpan.Zero,
            Mass = 3,
            RadPerSecond = (decimal)Math.PI
        };

        public PlayerShipOther(ILogger logger, GameThingCoordinator gameManager, string name, decimal size = 10, int? id = null, ThingBase creator = null)
            : base(logger, gameManager, Settings, name, size, id, creator)
        {
        }

        protected override void Umi(TimeSpan timeSinceLastUpdate)
        {
        }
    }
}