using System.IO;
using System.Net.Sockets;
using System.Threading;
using HelloGame.Common.Network;

namespace HelloGame
{
    public class ClientNetwork
    {
        Stream _stream;
        private readonly MessageTransciever _sender = new MessageTransciever();
        private Thread _receiveThread;

        public void StartConnection(string server, string playerName, int port = 49182)
        {
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
            _sender.Send(new NetworkMessage { Text = "PN " + playerName }, _stream);
        }

        private void Receive()
        {
            var message = _sender.Get(_stream);
        }
    }
}
