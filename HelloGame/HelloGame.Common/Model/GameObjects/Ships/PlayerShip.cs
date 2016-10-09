using System;
using System.Collections.Generic;
using HelloGame.Common.Settings;

namespace HelloGame.Common.Model.GameObjects.Ships
{
    public abstract class PlayerShip : ShipBase
    {
        public readonly ClanEnum Clan;

        protected PlayerShip(ThingBaseInjections injections, GameThingCoordinator coordinator, string name,
            ClanEnum clan, int? id = null, ThingBase creator = null, ElapsingThingSettings elapsingThingSettings = null,
            int score = 0)
            : base(
                injections, coordinator, ShipBaseSettings.GetClanShipSetting(clan, elapsingThingSettings), name, id,
                creator, score)
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