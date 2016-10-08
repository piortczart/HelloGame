using System.Collections.Generic;
using HelloGame.Common.Model.GameObjects.Ships;

namespace HelloGame.Common.Model.GameEvents
{
    public class GameEventBusSameThread
    {
        private readonly List<GameEventObserver> _observersNotSafe = new List<GameEventObserver>();
        private readonly object _synchro = new object();

        public void ThePlayerSpawned(PlayerShipOther oldShip, PlayerShipOther newShip)
        {
            List<GameEventObserver> observersSafe;
            lock (_synchro)
            {
                observersSafe = new List<GameEventObserver>(_observersNotSafe);
            }

            foreach (GameEventObserver observer in observersSafe)
            {
                observer.ThePlayerSpawned(oldShip, newShip);
            }
        }

        public void AddObserver(GameEventObserver observer)
        {
            lock (_synchro)
            {
                _observersNotSafe.Add(observer);
            }
        }
    }
}