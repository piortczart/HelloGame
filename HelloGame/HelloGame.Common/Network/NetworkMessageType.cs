using System;

namespace HelloGame.Common.Network
{
    [Serializable]
    public enum NetworkMessageType
    {
        UpdateStuff,
        Hello,
        MyPosition,
        PleaseSpawn
    }
}