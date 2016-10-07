using System;
using HelloGame.Common.Model.GameObjects;
using HelloGame.Common.TimeStuffs;

namespace HelloGame.Common.Model
{
    public abstract class ElapsingThing
    {
        protected readonly TimeSpan SpawnedAt;
        public TimeSpan Age => TimeSource.ElapsedSinceStart - SpawnedAt;
        public bool IsTimeToElapse { get; set; }
        public TimeSpan TimeToLive { get; set; }
        public double AgePercentage => 100*Age.TotalMilliseconds/TimeToLive.TotalMilliseconds;
        protected readonly TimeSource TimeSource;

        public ElapsingThingSettings ElapsingSettings
            => new ElapsingThingSettings {SpawnedAt = SpawnedAt, TimeToLive = TimeToLive};

        protected ElapsingThing(TimeSpan timeToLive, TimeSource timeSource, TimeSpan? spawnedAt)
        {
            TimeToLive = timeToLive;
            TimeSource = timeSource;
            SpawnedAt = spawnedAt ?? TimeSource.ElapsedSinceStart;
        }

        protected void ElapseIn(TimeSpan lifeLeft)
        {
            TimeToLive = lifeLeft;
        }

        public void Despawn()
        {
            IsTimeToElapse = true;
        }

        protected void UpdateElapsing()
        {
            // TimeToLive less than 0 means infinite.
            if (TimeToLive.TotalMilliseconds >= 0 && Age > TimeToLive)
            {
                IsTimeToElapse = true;
            }
        }
    }
}