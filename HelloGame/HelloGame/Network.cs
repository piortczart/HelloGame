using HelloGame.Common;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace HelloGame
{
    public class ClientNetwork
    {
        Stream stream;
        MessageTransciever sender = new MessageTransciever();

        public void Connect()
        {
            TcpClient client = new TcpClient("localhost", 3000);
            stream = client.GetStream();
        }

        public void SendMyInfo()
        {
            sender.Send(new NetworkMessage { Text = "Hello!" }, stream);
        }

        public void Receive()
        {
            var message = sender.Get(stream);
        }
    }
}
