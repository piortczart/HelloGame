using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using HelloGame.Common.Extensions;
using HelloGame.Common.Logging;
using HelloGame.Common.Model;
using HelloGame.Common.Model.GameObjects.Ships;
using HelloGame.Common.Network;

namespace HelloGame.Server
{
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


        public async Task ProcessAsync(CancellationToken cancellation)
        {
            while (!cancellation.IsCancellationRequested)
            {
                using (cancellation.Register(_tcpListener.Stop))
                {
                    // Blocks until a client has connected to the server
                    TcpClient client = await _tcpListener.AcceptTcpClientAsync();
                    // This should create a new task.
                    Task.Run(async () => { await HandleClientComm(client, cancellation); }, cancellation);
                }
            }
        }

        private async Task HandleClientComm(TcpClient client, CancellationToken cancellation)
        {
            NetworkStream clientStream = null;
            try
            {
                TcpClient tcpClient = client;
                clientStream = tcpClient.GetStream();
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
                if (clientStream != null)
                {
                    _serversClients.Disconnected(clientStream);
                }
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
                        ParseThingResult parseResult =
                            _gameManager.ParseThingDescription(message.Payload.DeSerializeJson<ThingDescription>(),
                                ParseThingSource.ToServer_PlayerPosition);
                        // The client does not know he's dead.
                        if (parseResult == ParseThingResult.UpdateFailedThingMissing)
                        {
                            // TODO: So we ignore it?
                        }
                    }
                    break;
                }
                case NetworkMessageType.PleaseSpawn:
                {
                    var stuff = message.Payload.DeSerializeJson<List<ThingDescription>>();
                    // Player can't spawn anything if he is dead.
                    PlayerShipOther ship = _serversClients.GetShip(clientStream);
                    {
                        if (!ship.IsDestroyed)
                        {
                            _gameManager.ParseThingDescriptions(stuff, ParseThingSource.ToServer_SpawnRequest);
                        }
                    }
                    break;
                }
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