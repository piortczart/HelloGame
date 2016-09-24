using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using HelloGame.Common.Extensions;
using HelloGame.Common.Logging;
using HelloGame.Common.Model;
using HelloGame.Common.Network;

namespace HelloGame
{
    public class ClientNetwork
    {
        private readonly GameManager _gameManager;
        Stream _stream;
        private readonly MessageTransciever _sender = new MessageTransciever();
        private Thread _receiveThread;
        private readonly ILogger _logger;

        public ClientNetwork(ILoggerFactory loggerFactory, GameManager gameManager)
        {
            _gameManager = gameManager;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public void StartConnection(string server, string playerName, int port = 49182)
        {
            _logger.LogInfo("Starting connection.");

            Connect(server, port);
            SendMyInfo(playerName);
            _receiveThread = new Thread(Receive);
            _receiveThread.Start();
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

        private void Receive()
        {
            _logger.LogInfo("Starting receive loop.");

            while (true)
            {
                NetworkMessage message = _sender.Get(_stream);
                _logger.LogInfo($"Got messsage: {message}");

                switch (message.Type)
                {
                    case NetworkMessageType.UpdateStuff:
                        {
                            List<ThingDescription> stuff = message.Payload.DeSerializeJson<List<ThingDescription>>();
                            foreach (ThingDescription description in stuff)
                            {
                                _gameManager.CreateFromDescription(description);
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
