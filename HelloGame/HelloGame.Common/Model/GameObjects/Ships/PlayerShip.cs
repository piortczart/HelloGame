using System;
using System.Collections.Generic;

namespace HelloGame.Common.Model.GameObjects.Ships
{
    public abstract class PlayerShip : ShipBase
    {
        protected PlayerShip(ThingBaseInjections injections, GameThingCoordinator gameManager, string name, decimal size = 10, int? id = null, ThingBase creator = null) 
            : base(injections, gameManager, injections.GeneralSettings.PlayerShipBaseSettings, size, name, id, creator)
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
