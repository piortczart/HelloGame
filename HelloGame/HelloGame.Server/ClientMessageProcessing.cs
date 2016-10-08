using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using HelloGame.Common.Extensions;
using HelloGame.Common.Logging;
using HelloGame.Common.Model;
using HelloGame.Common.Model.GameEvents;
using HelloGame.Common.Model.GameObjects.Ships;
using HelloGame.Common.Network;

namespace HelloGame.Server
{
    public class ServersClients
    {
        private readonly Dictionary<NetworkStream, PlayerShipOther> _clientsShips =
            new Dictionary<NetworkStream, PlayerShipOther>();

        private readonly object _synchro = new object();

        public ServersClients(GameEventBusSameThread gameEvents)
        {
            // This class wants to know when player's ship gets replaced. It will happen in the GameManager.
            gameEvents.AddObserver(new GameEventObserver(ReplacePlayersShip));
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
                NetworkStream key = _clientsShips.Single(s => s.Value == oldShip).Key;
                if (key == null)
                {
                    throw new ArgumentException("Could not find the player!");
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

    public class ClientMessageProcessing
    {
        private TcpListener _tcpListener;
        private readonly GameManager _gameManager;
        private readonly MessageTransciever _transciever;
        private readonly ILogger _logger;
        private readonly ServersClients _serversClients;

        public ClientMessageProcessing(GameManager gameManager, ILoggerFactory loggerFactory,
            MessageTransciever transciever, ServersClients serversClients)
        {
            _gameManager = gameManager;
            _transciever = transciever;
            _serversClients = serversClients;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public void Start(int port)
        {
            _tcpListener = new TcpListener(IPAddress.Any, port);
            _tcpListener.Start();
        }


        public void Process(CancellationToken cancellation)
        {
            while (!cancellation.IsCancellationRequested)
            {
                // Blocks until a client has connected to the server
                TcpClient client = _tcpListener.AcceptTcpClient();
                Task.Run(async () => { await HandleClientComm(client, cancellation); }, cancellation);
            }
        }

        private async Task HandleClientComm(TcpClient client, CancellationToken cancellation)
        {
            try
            {
                TcpClient tcpClient = client;
                var clientStream = tcpClient.GetStream();
                _serversClients.NewClient(clientStream);

                // Deserialize the stream into object
                while (!cancellation.IsCancellationRequested)
                {
                    NetworkMessage message = await _transciever.GetAsync(clientStream);
                    ProcessMessage(message, clientStream);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError("Handling client communication failed.", exception);
                client.Close();
            }
        }

        private void ProcessMessage(NetworkMessage message, NetworkStream clientStream)
        {
            switch (message.Type)
            {
                case NetworkMessageType.Hello:
                {
                    NetworkMessageHello hello = message.Payload.DeSerializeJson<NetworkMessageHello>();
                    PlayerShipOther ship = _gameManager.AddPlayerRandom(hello.Name.SubstringSafe(0, 15), hello.Clan);
                    _serversClients.SetShip(clientStream, ship);
                    break;
                }
                case NetworkMessageType.MyPosition:
                {
                    // He can still think he is alive, we cannot simply update his position if he's not.
                    PlayerShipOther ship = _serversClients.GetShip(clientStream);
                    if (!ship.IsDestroyed)
                    {
                        _gameManager.ParseThingDescription(message.Payload.DeSerializeJson<ThingDescription>(),
                            ParseThingSource.ToServer_PlayerPosition);
                    }
                    break;
                }
                case NetworkMessageType.PleaseSpawn:
                    var stuff = message.Payload.DeSerializeJson<List<ThingDescription>>();
                    _gameManager.ParseThingDescriptions(stuff, ParseThingSource.ToServer_SpawnRequest);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Attempts to send the message to the client, returns true on success, false on error.
        /// Disconnects the user on error. Despawns the ship.
        /// </summary>
        public bool SendDisconnectOnError(NetworkMessage message, NetworkStream stream)
        {
            bool isSuccess = _transciever.Send(message, stream);
            if (!isSuccess)
            {
                Disconnect(stream);
            }
            return isSuccess;
        }

        private void Disconnect(NetworkStream client)
        {
            _serversClients.Disconnected(client);
        }
    }
}