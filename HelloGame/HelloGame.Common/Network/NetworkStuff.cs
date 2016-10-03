﻿using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace HelloGame.Common.Network
{
    public class MessageTransciever
    {
        readonly BinaryFormatter _formatter = new BinaryFormatter();

        public bool Send(NetworkMessage message, Stream stream)
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