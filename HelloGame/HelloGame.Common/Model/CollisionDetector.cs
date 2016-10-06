using HelloGame.Common.Settings;

namespace HelloGame.Common.Model
{
    public class CollisionDetector
    {
        private readonly GeneralSettings _settings;
        public readonly EventPerSecond CollisoinsCounter;

        public CollisionDetector(TimeSource timeSource, GeneralSettings settings)
        {
            _settings = settings;
            CollisoinsCounter = new EventPerSecond(timeSource);
        }

        public void DetectCollisions(ThingBase[] things)
        {
            CollisoinsCounter.Add();

            for (int i = 0; i < things.Length; i++)
            {
                ThingBase thing1 = things[i];
                if (thing1.IsDestroyed)
                {
                    continue;
                }

                for (int j = 0; j < things.Length; j++)
                {
                    ThingBase thing2 = things[j];
                    if (thing1 == thing2 || thing2.IsDestroyed)
                    {
                        continue;
                    }

                    if (thing1.DistanceTo(thing2) <= _settings.CollisionTolerance)
                    {
                        thing1.CollidesWith(thing2);
                        thing2.CollidesWith(thing1);
                    }
                }
            }
        }
    }
}