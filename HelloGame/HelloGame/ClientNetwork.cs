using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using HelloGame.Common.Extensions;
using HelloGame.Common.Logging;
using HelloGame.Common.Model;
using HelloGame.Common.Network;
using HelloGame.Common.Model.GameObjects.Ships;

namespace HelloGame.Client
{
    /// <summary>
    /// Handles the client's network communication.
    /// Periodically sends client's updated position to the server.
    /// Receives messages from the server, updates model.
    /// </summary>
    public class ClientNetwork
    {
        private readonly GameManager _gameManager;
        private Stream _stream;
        private readonly MessageTransciever _sender = new MessageTransciever();
        private readonly ILogger _logger;
        private readonly Timer _sendMeTimer;

        private static readonly TimeSpan PropagateFrequency =
            TimeSpan.Parse(ConfigurationManager.AppSettings["PropagateFrequencyClient"]);

        private static readonly int ServerPortNumber = int.Parse(ConfigurationManager.AppSettings["ServerPortNumber"]);

        // This is exclusive for the propagate timer thread.
        private readonly HashSet<ThingBase> _thingsSent = new HashSet<ThingBase>();

        public ClientNetwork(ILoggerFactory loggerFactory, GameManager gameManager)
        {
            _gameManager = gameManager;
            _logger = loggerFactory.CreateLogger(GetType());
            _sendMeTimer = new Timer(state =>
            {
                SendMyPosition();
                SendMyItems();
            }, null, PropagateFrequency, Timeout.InfiniteTimeSpan);
        }

        private void SendMyItems()
        {
            List<ThingBase> unsentThings = _gameManager.GetAndDequeueThingsToSpawn();
            if (unsentThings.Any())
            {
                var message = new NetworkMessage
                {
                    Type = NetworkMessageType.PleaseSpawn,
                    Payload = unsentThings.Select(t => new ThingDescription(t, false)).SerializeJson()
                };
                _sender.Send(message, _stream);

                foreach (ThingBase unsentThing in unsentThings)
                {
                    _thingsSent.Add(unsentThing);
                }

                _logger.LogInfo($"Sent {unsentThings.Count} items, total sent: {_thingsSent.Count}");
            }
        }

        private void SendMyPosition()
        {
            ThingBase me = _gameManager.GetMe();
            if (me != null)
            {
                var message = new NetworkMessage
                {
                    Type = NetworkMessageType.MyPosition,
                    Payload = new ThingDescription(me, false).SerializeJson()
                };
                _sender.Send(message, _stream);
            }
            _sendMeTimer.Change(PropagateFrequency, Timeout.InfiniteTimeSpan);
        }

        public void StartConnection(string serverAddress, string playerName, ClanEnum clan,
            CancellationTokenSource cancellation)
        {
            _logger.LogInfo($"Starting connection to {serverAddress}:{ServerPortNumber}");

            Connect(serverAddress, ServerPortNumber);
            IntroduceMyself(playerName, clan);

            Task.Run(async () => { await Receive(cancellation.Token); }, cancellation.Token);
        }

        private void Connect(string server, int port)
        {
            TcpClient client = new TcpClient(server, port);
            _stream = client.GetStream();
        }

        private void IntroduceMyself(string playerName, ClanEnum clan)
        {
            _sender.Send(new NetworkMessage
            {
                Type = NetworkMessageType.Hello,
                Payload = new NetworkMessageHello {Name = playerName, Clan = clan}.SerializeJson()
            }, _stream);
        }

        private async Task Receive(CancellationToken token)
        {
            try
            {
                _logger.LogInfo("Starting receive loop.");

                while (!token.IsCancellationRequested)
                {
                    NetworkMessage message = await _sender.GetAsync(_stream);
                    //_logger.LogInfo($"Got messsage: {message.ToStringFull()}");

                    switch (message.Type)
                    {
                        case NetworkMessageType.UpdateStuff:
                        {
                            List<ThingDescription> stuff = message.Payload.DeSerializeJson<List<ThingDescription>>();
                            _logger.LogInfo($"Server sent update, count: {stuff.Count}");
                            foreach (ThingDescription description in stuff)
                            {
                                _gameManager.ParseThingDescription(description, ParseThingSource.ToClient);
                            }
                        }
                            break;
                        case NetworkMessageType.DeadStuff:
                        {
                            List<int> deadStuff = message.Payload.DeSerializeJson<List<int>>();
                            _gameManager.StuffDied(deadStuff);
                            break;
                        }
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError("Server communication error.", exception);
            }
        }
    }
}