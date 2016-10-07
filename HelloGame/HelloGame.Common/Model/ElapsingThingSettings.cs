using System;

namespace HelloGame.Common.Model
{
    public class ElapsingThingSettings
    {
        public TimeSpan? SpawnedAt { get; set; }
        public TimeSpan? TimeToLive { get; set; }
    }
}