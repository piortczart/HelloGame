using System;
using System.Collections.Concurrent;
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
        public readonly ConcurrentDictionary<NetworkStream, PlayerShipAny> Clients = new ConcurrentDictionary<NetworkStream, PlayerShipAny>();

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
            TcpClient tcpClient = client;
            NetworkStream clientStream = tcpClient.GetStream();
            Clients[clientStream] = null;

            // Deserialize the stream into object
            while (!cancellation.IsCancellationRequested)
            {
                NetworkMessage message = await _transciever.GetAsync(clientStream);
                _logger.LogInfo($"Got a client message: {message}");
                ProcessMessage(message, clientStream);
            }
        }

        private void ProcessMessage(NetworkMessage message, NetworkStream clientStream)
        {
            switch (message.Type)
            {
                case NetworkMessageType.Hello:
                    PlayerShipAny ship = _gameManager.AddPlayer(message.Payload.SubstringSafe(0, 15));
                    Clients[clientStream] = ship;
                    break;
                case NetworkMessageType.MyPosition:
                    _gameManager.ParseThingDescription(message.Payload.DeSerializeJson<ThingDescription>());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}