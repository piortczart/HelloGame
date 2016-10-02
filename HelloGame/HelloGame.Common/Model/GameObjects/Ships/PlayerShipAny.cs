using System;
using HelloGame.Common.Logging;

namespace HelloGame.Common.Model.GameObjects.Ships
{
    public class PlayerShipOther : PlayerShip
    {

        public PlayerShipOther(ILogger logger, GameThingCoordinator gameManager, string name, decimal size = 10, int? id = null, ThingBase creator = null)
            : base(logger, gameManager, name, size, id, creator)
        {
        }

        protected override void Umi(TimeSpan timeSinceLastUpdate)
        {
        }
    }
}