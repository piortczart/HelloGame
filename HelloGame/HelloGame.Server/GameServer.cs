using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly MessageTransciever _sender;

        private readonly GameManager _gameManager;
        private readonly ClientMessageProcessing _clientMessageProcessing;
        private readonly ILogger _logger;
        private Timer _propagateTimer;
        private static readonly TimeSpan PropagateFrequency = TimeSpan.FromMilliseconds(25);

        public GameServer(GameManager gameManager, ILoggerFactory loggerFactory, ClientMessageProcessing clientMessageProcessing, MessageTransciever sender)
        {
            _gameManager = gameManager;
            _clientMessageProcessing = clientMessageProcessing;
            _sender = sender;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public void Start(CancellationTokenSource cancellationTokenSource, int port = 49182)
        {
            _logger.LogInfo($"Server starting at port: {port}");
            _clientMessageProcessing.Start(port);

            _logger.LogInfo("Starting the game manager.");
            _gameManager.StartGame();

            _logger.LogInfo("Starting the listen thread.");
            Task.Run(() => { _clientMessageProcessing.Process(cancellationTokenSource.Token); }, cancellationTokenSource.Token);

            _logger.LogInfo("Starting the propagate timer.");
            _propagateTimer = new Timer(state => { Propagate(); }, null, PropagateFrequency, Timeout.InfiniteTimeSpan);
        }

        private void Propagate()
        {
            _logger.LogInfo($"Number of things: {_gameManager.ModelManager.GetThings().Count} {String.Join(",", _gameManager.ModelManager.GetThings().Select(t => t.Id))}");

            SendUpdateMessage(_gameManager.ModelManager.GetThings());

            _propagateTimer.Change(PropagateFrequency, Timeout.InfiniteTimeSpan);
        }

        private void SendUpdateMessage(List<ThingBase> things)
        {
            foreach (var client in _clientMessageProcessing.Clients)
            {
                NetworkStream networkStream = client.Key;
                PlayerShipAny ship = client.Value;

                var message = new NetworkMessage
                {
                    Type = NetworkMessageType.UpdateStuff,
                    Payload = things.Select(t => new ThingDescription(t, t == ship)).SerializeJson()
                };

                _sender.Send(message, networkStream);
            }
        }

    }
}