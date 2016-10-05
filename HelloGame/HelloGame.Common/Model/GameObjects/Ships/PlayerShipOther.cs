using System;

namespace HelloGame.Common.Model.GameObjects.Ships
{
    public class PlayerShipOther : PlayerShip
    {

        public PlayerShipOther(ThingBaseInjections injections, GameThingCoordinator gameManager, string name, ClanEnum clan, int? id = null, ThingBase creator = null)
            : base(injections, gameManager, name, clan, id, creator)
        {
        }

        protected override void Umi(TimeSpan timeSinceLastUpdate)
        {
        }
    }
}