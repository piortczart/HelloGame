﻿using System;
using System.Collections.Generic;
using HelloGame.Common.Logging;

namespace HelloGame.Common.Model.GameObjects.Ships
{
    public abstract class PlayerShip : DaShip
    {
        public PlayerShip(ILogger logger, GameThingCoordinator gameManager, string name, decimal size = 10, int? id = null) : base(logger, gameManager, size, name, id)
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
