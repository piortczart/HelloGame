using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace HelloGame.Common.Network
{
    public class MessageTransciever : IMessageTransciever
    {
        readonly BinaryFormatter _formatter = new BinaryFormatter();

        private readonly object _synchro = new object();

        public bool Send(NetworkMessage message, Stream stream)
        {
            lock (_synchro)
            {
                try
                {
                    _formatter.Serialize(stream, message);
                    return true;
                }
                catch (System.Exception)
                {
                    return false;
                }
            }
        }

        public NetworkMessage Get(Stream stream)
        {
            return (NetworkMessage) _formatter.Deserialize(stream);
        }

        public Task<NetworkMessage> GetAsync(Stream stream)
        {
            return Task.Run(() => (NetworkMessage) _formatter.Deserialize(stream));
        }
    }
}