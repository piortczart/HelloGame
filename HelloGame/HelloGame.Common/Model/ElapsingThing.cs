using System;

namespace HelloGame.Common.Model
{
    public abstract class ElapsingThing
    {
        protected readonly TimeSpan SpawnedAt;
        public TimeSpan Age => TimeSource.ElapsedSinceStart - SpawnedAt;
        public bool IsTimeToElapse { get; private set; }
        public TimeSpan TimeToLive { get; private set; }
        public double AgePercentage => 100 * Age.TotalMilliseconds / TimeToLive.TotalMilliseconds;
        protected readonly TimeSource TimeSource;

        protected ElapsingThing(TimeSpan timeToLive, TimeSource timeSource)
        {
            TimeToLive = timeToLive;
            TimeSource = timeSource;
            SpawnedAt = TimeSource.ElapsedSinceStart;
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
            if (TimeToLive.TotalMilliseconds > 0 && Age > TimeToLive)
            {
                IsTimeToElapse = true;
            }
        }
    }
}