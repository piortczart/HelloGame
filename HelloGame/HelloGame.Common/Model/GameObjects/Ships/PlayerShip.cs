using System;
using System.Collections.Generic;

namespace HelloGame.Common.Model.GameObjects.Ships
{
    public abstract class PlayerShip : DaShip
    {
        public PlayerShip(ModelManager modelManager, decimal size = 10) : base(modelManager, size)
        {
        }

        protected override void UpdateModelInternal(TimeSpan timeSinceLastUpdate, List<ThingBase> otherThings)
        {
            if (IsDestroyed)
            {
                return;
            }

            Umi(timeSinceLastUpdate);
        }

        protected abstract void Umi(TimeSpan timeSinceLastUpdate);
    }

    public class PlayerShipAny : PlayerShip
    {
        public PlayerShipAny(ModelManager modelManager, decimal size = 10) : base(modelManager, size)
        {
        }

        protected override void Umi(TimeSpan timeSinceLastUpdate)
        {
        }
    }
}
