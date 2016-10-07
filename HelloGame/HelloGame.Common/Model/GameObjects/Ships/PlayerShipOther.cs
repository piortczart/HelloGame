using System;

namespace HelloGame.Common.Model.GameObjects.Ships
{
    public class PlayerShipOther : PlayerShip
    {
        public PlayerShipOther(ThingBaseInjections injections, GameThingCoordinator coordinator, string name,
            ClanEnum clan, int? id = null, ThingBase creator = null,
            ElapsingThingSettings etsElapsingThingSettings = null)
            : base(injections, coordinator, name, clan, id, creator, etsElapsingThingSettings)
        {
        }

        protected override void Umi(TimeSpan timeSinceLastUpdate)
        {
        }
    }
}