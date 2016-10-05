using System;
using System.Collections.Generic;

namespace HelloGame.Common.Model.GameObjects.Ships
{
    public abstract class PlayerShip : ShipBase
    {
        public ClanEnum Clan;

        protected PlayerShip(ThingBaseInjections injections, GameThingCoordinator gameManager, string name, ClanEnum clan, int? id = null, ThingBase creator = null)
            : base(injections, gameManager, injections.GeneralSettings.ClanSettings[clan], name, id, creator)
        {
            Clan = clan;
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
