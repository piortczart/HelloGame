using System;
using HelloGame.Common.Extensions;

namespace HelloGame.Common.Network
{
    [Serializable]
    public class NetworkMessage
    {
        public NetworkMessageType Type { get; set; }
        public string Payload { get; set; }

        public override string ToString()
        {
            return $"[{Type}] {Payload.SubstringSafe(0, 50)}";
        }

        public string ToStringFull()
        {
            return $"[{Type}] {Payload}";
        }
    }
}