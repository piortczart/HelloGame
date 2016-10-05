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
        public readonly ConcurrentDictionary<NetworkStream, PlayerShipOther> Clients = new ConcurrentDictionary<NetworkStream, PlayerShipOther>();

        public ClientMessageProcessing(GameManager gameManager, ILoggerFactory loggerFactory, MessageTransciever transciever)
        {
            _gameManager = gameManager;
            _transciever = transciever;
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
                Clients[clientStream] = null;

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
                    PlayerShipOther ship = _gameManager.AddPlayer(message.Payload.SubstringSafe(0, 15));
                    Clients[clientStream] = ship;
                    break;
                case NetworkMessageType.MyPosition:
                    _gameManager.ParseThingDescription(message.Payload.DeSerializeJson<ThingDescription>());
                    break;
                case NetworkMessageType.PleaseSpawn:
                    var stuff = message.Payload.DeSerializeJson<List<ThingDescription>>();
                    _gameManager.ParseThingDescriptions(stuff);
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
            PlayerShipOther ship;
            // Remove from the clients list.
            if (Clients.TryRemove(client, out ship))
            {
                // Despawn the ship.
                ship.Despawn();
            }
        }
    }
}