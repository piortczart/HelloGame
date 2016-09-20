using System;

namespace HelloGame.GameObjects
{
    public abstract class ElapsingThing
    {
        protected readonly DateTime SpawnedAt;
        public TimeSpan Age => DateTime.Now - SpawnedAt;
        public bool IsTimeToDie { get; private set; }
        public TimeSpan TimeToLive { get; }
        public double AgePercentage => 100 * Age.TotalMilliseconds/ TimeToLive.TotalMilliseconds;

        protected ElapsingThing(TimeSpan timeToLive)
        {
            TimeToLive = timeToLive;
            SpawnedAt = DateTime.Now;
        }

        protected void UpdateElapsing()
        {
            if (TimeToLive.TotalMilliseconds > 0 && Age > TimeToLive)
            {
                IsTimeToDie = true;
            }
        }
    }
}