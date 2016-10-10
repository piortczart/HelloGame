using System.IO;
using System.Threading.Tasks;

namespace HelloGame.Common.Network
{
    public interface IMessageTransciever
    {
        bool Send(NetworkMessage message, Stream stream);
        NetworkMessage Get(Stream stream);
        Task<NetworkMessage> GetAsync(Stream stream);
    }
}