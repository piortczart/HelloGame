using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace HelloGame.Common
{
    [Serializable]
    public class NetworkMessage
    {
        public string Text { get; set; }
        public decimal NumberA { get; set; }
        public decimal NumberB { get; set; }
    }

    public class MessageTransciever
    {
        BinaryFormatter formatter = new BinaryFormatter();

        public void Send(NetworkMessage message, Stream stream)
        {
            formatter.Serialize(stream, message);
        }

        public NetworkMessage Get(Stream stream)
        {
            return (NetworkMessage)formatter.Deserialize(stream);
        }

    }
}
