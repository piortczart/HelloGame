using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using HelloGame.Common.Extensions;
using HelloGame.Common.Logging;
using HelloGame.Common.Model;
using HelloGame.Common.Network;

namespace HelloGame.Client
{
    public class ClientNetwork
    {
        private readonly GameManager _gameManager;
        Stream _stream;
        private readonly MessageTransciever _sender = new MessageTransciever();
        private readonly ILogger _logger;
        private readonly Timer _sendMeTimer;
        private static readonly TimeSpan PropagateFrequency = TimeSpan.FromMilliseconds(100);

        public ClientNetwork(ILoggerFactory loggerFactory, GameManager gameManager)
        {
            _gameManager = gameManager;
            _logger = loggerFactory.CreateLogger(GetType());
            _sendMeTimer = new Timer(state => { SendMyPosition(); }, null, PropagateFrequency, Timeout.InfiniteTimeSpan);
        }

        private void SendMyPosition()
        {
            ThingBase me = _gameManager.GetMe();
            if (me != null)
            {
                _sender.Send(
                    new NetworkMessage
                    {
                        Type = NetworkMessageType.MyPosition,
                        Payload = new ThingDescription(me, false).SerializeJson()
                    }, _stream);
            }
            _sendMeTimer.Change(PropagateFrequency, Timeout.InfiniteTimeSpan);
        }

        public void StartConnection(string server, string playerName, CancellationTokenSource cancellation, int port = 49182)
        {
            _logger.LogInfo("Starting connection.");

            Connect(server, port);
            SendMyInfo(playerName);

            Task.Run(async () => { await Receive(cancellation.Token); }, cancellation.Token);
        }

        private void Connect(string server, int port)
        {
            TcpClient client = new TcpClient(server, port);
            _stream = client.GetStream();
        }

        private void SendMyInfo(string playerName)
        {
            _sender.Send(new NetworkMessage { Type = NetworkMessageType.Hello, Payload = playerName }, _stream);
        }

        private async Task Receive(CancellationToken token)
        {
            _logger.LogInfo("Starting receive loop.");

            while (!token.IsCancellationRequested)
            {
                NetworkMessage message = await _sender.GetAsync(_stream);
                _logger.LogInfo($"Got messsage: {message.ToStringFull()}");

                switch (message.Type)
                {
                    case NetworkMessageType.UpdateStuff:
                        {
                            List<ThingDescription> stuff = message.Payload.DeSerializeJson<List<ThingDescription>>();
                            foreach (ThingDescription description in stuff)
                            {
                                _gameManager.ParseThingDescription(description);
                            }
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
