using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace HelloGame.Common.Network
{
    public class MessageTransciever
    {
        readonly BinaryFormatter _formatter = new BinaryFormatter();

        public void Send(NetworkMessage message, Stream stream)
        {
            _formatter.Serialize(stream, message);
        }

        public NetworkMessage Get(Stream stream)
        {
            return (NetworkMessage)_formatter.Deserialize(stream);
        }

    }
}