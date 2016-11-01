using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using HelloGame.Common.Model;
using HelloGame.Common.Model.GameObjects.Ships;

namespace HelloGame.Server
{
    public class ServersClients
    {
        private readonly Dictionary<NetworkStream, PlayerShipOther> _clientsShips =
            new Dictionary<NetworkStream, PlayerShipOther>();

        private readonly object _synchro = new object();

        public ServersClients(GameManager gameManager)
        {
            // This class wants to know when player's ship gets replaced. It will happen in the GameManager.
            gameManager.OnPlayerSpawned += ReplacePlayersShip;
        }

        public Dictionary<NetworkStream, PlayerShipOther> GetAllReadOnly()
        {
            lock (_synchro)
            {
                return new Dictionary<NetworkStream, PlayerShipOther>(_clientsShips);
            }
        }

        private void ReplacePlayersShip(PlayerShipOther oldShip, PlayerShipOther newShip)
        {
            lock (_synchro)
            {
                NetworkStream key = _clientsShips.SingleOrDefault(s => s.Value == oldShip).Key;
                if (key == null)
                {
                    //throw new ArgumentException("Could not find the player!");
                    return;
                }
                _clientsShips[key] = newShip;
            }
        }

        public void NewClient(NetworkStream clientStream)
        {
            lock (_synchro)
            {
                _clientsShips[clientStream] = null;
            }
        }

        public void SetShip(NetworkStream clientStream, PlayerShipOther ship)
        {
            lock (_synchro)
            {
                _clientsShips[clientStream] = ship;
            }
        }

        public PlayerShipOther GetShip(NetworkStream clientStream)
        {
            lock (_synchro)
            {
                return _clientsShips[clientStream];
            }
        }

        public void Disconnected(NetworkStream client)
        {
            lock (_synchro)
            {
                PlayerShipOther ship = _clientsShips[client];
                _clientsShips.Remove(client);
                // Despawn the ship.
                ship.Despawn();
            }
        }
    }
}