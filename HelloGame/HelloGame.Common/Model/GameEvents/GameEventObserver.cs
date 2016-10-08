using System;
using HelloGame.Common.Model.GameObjects.Ships;

namespace HelloGame.Common.Model.GameEvents
{
    public class GameEventObserver
    {
        readonly Action<PlayerShipOther, PlayerShipOther> _onPlayerSpawned;

        public GameEventObserver(Action<PlayerShipOther, PlayerShipOther> onPlayerSpawned)
        {
            _onPlayerSpawned = onPlayerSpawned;
        }

        public void ThePlayerSpawned(PlayerShipOther oldShip, PlayerShipOther newShip)
        {
            _onPlayerSpawned?.Invoke(oldShip, newShip);
        }
    }
}