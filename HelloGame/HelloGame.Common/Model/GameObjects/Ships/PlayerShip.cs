using System;
using System.Collections.Generic;

namespace HelloGame.Common.Model.GameObjects.Ships
{
    public abstract class PlayerShip : DaShip
    {
        public PlayerShip(ModelManager modelManager, string name, decimal size = 10, int? id = null) : base(modelManager, size, name, id)
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
