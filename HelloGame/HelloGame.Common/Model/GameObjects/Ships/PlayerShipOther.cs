using System;

namespace HelloGame.Common.Model.GameObjects.Ships
{
    public class PlayerShipOther : PlayerShip
    {
        public PlayerShipOther(ThingBaseInjections injections, GameThingCoordinator coordinator, string name,
            ClanEnum clan, ThingAdditionalInfo additionalInfo, int? id = null,
            ElapsingThingSettings etsElapsingThingSettings = null)
            : base(injections, coordinator, name, clan, id, additionalInfo, etsElapsingThingSettings)
        {
        }

        protected override void Umi(TimeSpan timeSinceLastUpdate)
        {
        }
    }
}