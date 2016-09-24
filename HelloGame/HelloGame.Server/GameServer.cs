using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public class GameServer
    {
        private TcpListener _tcpListener;
        private readonly MessageTransciever _sender = new MessageTransciever();
        private readonly SynchronizedCollection<NetworkStream> _clients = new SynchronizedCollection<NetworkStream>();

        private readonly GameManager _gameManager;
        private readonly ILogger _logger;
        private Thread _listenThread;
        private Timer _propagateTimer;
        private readonly Dictionary<TcpClient, PlayerShipAny> _ships = new Dictionary<TcpClient, PlayerShipAny>();
        private static readonly TimeSpan PropagateFrequency = TimeSpan.FromMilliseconds(500);

        public GameServer(GameManager gameManager, ILoggerFactory loggerFactory)
        {
            _gameManager = gameManager;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public void Start(int port = 49182)
        {
            _logger.LogInfo($"Server starting at port: {port}");

            _tcpListener = new TcpListener(IPAddress.Any, port);
            _tcpListener.Start();

            _logger.LogInfo("Starting the game manager.");
            _gameManager.StartGame();

            _logger.LogInfo("Starting the listen thread.");
            _listenThread = new Thread(Process);
            _listenThread.Start();

            _logger.LogInfo("Starting the propagate timer.");
            _propagateTimer = new Timer(state => { Propagate(); }, null, PropagateFrequency, Timeout.InfiniteTimeSpan);
        }

        private void Propagate()
        {
            var message = new NetworkMessage
            {
                Type = NetworkMessageType.UpdateStuff,
                Payload = _gameManager
                    .ModelManager
                    .GetThings()
                    .Select(t => new ThingDescription(t, false))
                    .SerializeJson()
            };
            SendToAll(message);

            _propagateTimer.Change(PropagateFrequency, Timeout.InfiniteTimeSpan);
        }

        private void Process()
        {
            while (true)
            {
                // Blocks until a client has connected to the server
                TcpClient client = _tcpListener.AcceptTcpClient();
                Task.Run(() => { HandleClientComm(client); });
            }
        }

        public void SendToAll(NetworkMessage message)
        {
            foreach (NetworkStream networkStream in _clients)
            {
                _sender.Send(message, networkStream);
            }
        }

        private void HandleClientComm(TcpClient client)
        {
            TcpClient tcpClient = client;
            NetworkStream clientStream = tcpClient.GetStream();
            _clients.Add(clientStream);

            // Deserialize the stream into object
            while (true)
            {
                NetworkMessage message = _sender.Get(clientStream);
                _logger.LogInfo($"Got a client message: {message}");
                ProcessMessage(message, client);
            }
        }

        private void ProcessMessage(NetworkMessage message, TcpClient client)
        {
            switch (message.Type)
            {
                case NetworkMessageType.Hello:
                    PlayerShipAny ship = _gameManager.AddPlayer(message.Payload.SubstringSafe(0, 15));
                    _ships[client] = ship;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}