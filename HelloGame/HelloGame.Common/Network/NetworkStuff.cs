using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

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

        public Task<NetworkMessage> GetAsync(Stream stream)
        {
            return Task.Run(() => (NetworkMessage)_formatter.Deserialize(stream));
        }
    }
}