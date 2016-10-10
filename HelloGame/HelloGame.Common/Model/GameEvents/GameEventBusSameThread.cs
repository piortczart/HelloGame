using System;
using HelloGame.Common.Model.GameObjects.Ships;

namespace HelloGame.Common.Model.GameEvents
{
    /// <summary>
    /// TODO: This can be removed and the event can be added to GameManager
    /// </summary>
    public class GameEventBusSameThread
    {
        public event Action<PlayerShipOther, PlayerShipOther> OnPlayerSpawned;

        public void ThePlayerSpawned(PlayerShipOther oldShip, PlayerShipOther newShip)
        {
            OnPlayerSpawned?.Invoke(oldShip, newShip);
        }
    }
}