using System;
using System.Collections.Generic;
using HelloGame.Common.Logging;

namespace HelloGame.Common.Model.GameObjects.Ships
{
    public abstract class PlayerShip : DaShip
    {
        public PlayerShip(ILogger logger, GameThingCoordinator gameManager, ThingSettings settings, string name, decimal size = 10, int? id = null, ThingBase creator = null) 
            : base(logger, gameManager, settings, size, name, id, creator)
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
