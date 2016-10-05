using System;
using HelloGame.Common.Extensions;
using HelloGame.Common.Model.GameObjects.Ships;

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
    }

    public class NetworkMessageHello
    {
        public string Name { get; set; }
        public ClanEnum Clan { get; set; }
    }
}