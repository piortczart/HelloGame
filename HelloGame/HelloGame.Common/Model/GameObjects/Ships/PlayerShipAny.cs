using System;
using HelloGame.Common.Logging;

namespace HelloGame.Common.Model.GameObjects.Ships
{
    public class PlayerShipAny : PlayerShip
    {
        public PlayerShipAny(ILogger logger, GameManager gameManager, string name, decimal size = 10, int? id = null) : base(logger, gameManager, name, size, id)
        {
        }

        protected override void Umi(TimeSpan timeSinceLastUpdate)
        {
        }
    }
}