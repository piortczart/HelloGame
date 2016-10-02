using System;
using System.Collections.Generic;
using System.Configuration;
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
        private static readonly TimeSpan PropagateFrequency = TimeSpan.Parse(ConfigurationManager.AppSettings["PropagateFrequencyServer"]);
        private static readonly int ServerPortNumber = int.Parse(ConfigurationManager.AppSettings["ServerPortNumber"]);

        public GameServer(GameManager gameManager, ILoggerFactory loggerFactory, ClientMessageProcessing clientMessageProcessing, MessageTransciever sender)
        {
            _gameManager = gameManager;
            _clientMessageProcessing = clientMessageProcessing;
            _sender = sender;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public async Task Start(CancellationTokenSource cancellationTokenSource)
        {
            _logger.LogInfo($"Server starting at port: {ServerPortNumber}");
            _clientMessageProcessing.Start(ServerPortNumber);

            _logger.LogInfo("Starting the game manager.");
            _gameManager.StartGame();

            _logger.LogInfo("Starting the propagate timer.");
            _propagateTimer = new Timer(state => { Propagate(); }, null, PropagateFrequency, Timeout.InfiniteTimeSpan);

            _logger.LogInfo("Starting the listen thread.");
            await Task.Run(() => { _clientMessageProcessing.Process(cancellationTokenSource.Token); }, cancellationTokenSource.Token);
        }

        private void Propagate()
        {
            _logger.LogInfo($"Propagating. Number of things: {_gameManager.ModelManager.GetThings().Count} (ids: {String.Join(",", _gameManager.ModelManager.GetThings().Select(t => t.Id + "_" + t.GetType().Name))})");

            SendUpdateMessage(_gameManager.ModelManager.GetThings());

            _propagateTimer.Change(PropagateFrequency, Timeout.InfiniteTimeSpan);
        }

        private void SendUpdateMessage(List<ThingBase> things)
        {
            foreach (var client in _clientMessageProcessing.Clients)
            {
                NetworkStream networkStream = client.Key;
                PlayerShipOther ship = client.Value;

                var message1 = new NetworkMessage
                {
                    Type = NetworkMessageType.UpdateStuff,
                    Payload = things.Select(t => new ThingDescription(t, t == ship)).SerializeJson()
                };

                _sender.Send(message1, networkStream);

                List<int> deadThings = _gameManager.ModelManager.GetDeadThings().Select(t => t.Id).ToList();
                if (deadThings.Any())
                {
                    var message2 = new NetworkMessage
                    {
                        Type = NetworkMessageType.DeadStuff,
                        Payload = deadThings.SerializeJson()
                    };

                    _sender.Send(message2, networkStream);
                }
            }
        }

    }
}