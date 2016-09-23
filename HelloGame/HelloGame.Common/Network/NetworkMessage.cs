using System;

namespace HelloGame.Common.Network
{
    [Serializable]
    public class NetworkMessage
    {
        public string Text { get; set; }
        public decimal NumberA { get; set; }
        public decimal NumberB { get; set; }
    }
}