using System;

namespace HelloGame.Common.Model.GameObjects.Ships
{
    public class PlayerShipAny : PlayerShip
    {
        public PlayerShipAny(ModelManager modelManager, string name, decimal size = 10, int? id = null) : base(modelManager, name, size, id)
        {
        }

        protected override void Umi(TimeSpan timeSinceLastUpdate)
        {
        }
    }
}