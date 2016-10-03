using System;

namespace HelloGame.Common.Model
{
    public abstract class ElapsingThing
    {
        protected readonly DateTime SpawnedAt;
        public TimeSpan Age => DateTime.Now - SpawnedAt;
        public bool IsTimeToElapse { get; private set; }
        public TimeSpan TimeToLive { get; private set; }
        public double AgePercentage => 100 * Age.TotalMilliseconds / TimeToLive.TotalMilliseconds;

        protected ElapsingThing(TimeSpan timeToLive)
        {
            TimeToLive = timeToLive;
            SpawnedAt = DateTime.Now;
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