using System;
using System.Collections.Generic;
using HelloGame.Common.Logging;

namespace HelloGame.Common.Model.GameObjects.Ships
{
    public abstract class PlayerShip : ShipBase
    {
        private static readonly ThingSettings Settings = new ThingSettings
        {
            Aerodynamism = 0.1m,
            TimeToLive = TimeSpan.Zero,
            Mass = 3,
            RadPerSecond = (decimal)Math.PI,
            LazerLimit = TimeSpan.FromMilliseconds(200)
        };

        public PlayerShip(ILogger logger, GameThingCoordinator gameManager, string name, decimal size = 10, int? id = null, ThingBase creator = null) 
            : base(logger, gameManager, Settings, size, name, id, creator)
        {
        }

        protected override void UpdateModelInternal(TimeSpan timeSinceLastUpdate, IEnumerable<ThingBase> otherThings)
        {
            if (IsDestroyed)
            {
                return;
            }

            Umi(timeSinceLastUpdate);
        }

        protected abstract void Umi(TimeSpan timeSinceLastUpdate);
    }
}
