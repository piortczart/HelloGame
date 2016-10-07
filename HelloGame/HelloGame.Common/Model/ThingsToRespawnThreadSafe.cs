using System.Collections.Generic;
using HelloGame.Common.TimeStuffs;

namespace HelloGame.Common.Model
{
    public class ThingsToRespawnThreadSafe
    {
        private readonly TimeSource _timeSource;
        private readonly List<ThingToRespawn> _toRespawn = new List<ThingToRespawn>();
        private readonly object _synchro = new object();

        public ThingsToRespawnThreadSafe(TimeSource timeSource)
        {
            _timeSource = timeSource;
        }

        public void Add(ThingToRespawn thing)
        {
            lock (_synchro)
            {
                _toRespawn.Add(thing);
            }
        }

        public List<ThingToRespawn> GetReady()
        {
            lock (_synchro)
            {
                var result = new List<ThingToRespawn>();
                foreach (ThingToRespawn thingToRespawn in _toRespawn.ToArray())
                {
                    if (thingToRespawn.WhenToRespawn <= _timeSource.ElapsedSinceStart)
                    {
                        result.Add(thingToRespawn);
                        _toRespawn.Remove(thingToRespawn);
                    }
                }
                return result;
            }
        }
    }
}