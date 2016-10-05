using System;

namespace HelloGame.Common
{
    /// <summary>
    /// Can be used to limit occurance of an event to the given frequency.
    /// </summary>
    public class Limiter
    {
        private TimeSpan _lastEvent = TimeSpan.Zero;
        readonly TimeSpan _frequency;
        private readonly object _synchronizer = new object();
        private readonly TimeSource _timeSource;

        public Limiter(TimeSpan frequency, TimeSource timeSource)
        {
            _frequency = frequency;
            _timeSource = timeSource;
        }

        public bool CanHappen(bool willHappen = true)
        {
            lock (_synchronizer)
            {
                TimeSpan nextEvent = _lastEvent.Add(_frequency);
                if (_timeSource.ElapsedSinceStart > nextEvent)
                {
                    if (willHappen)
                    {
                        _lastEvent = _timeSource.ElapsedSinceStart;
                    }
                    return true;
                }
                return false;
            }
        }
    }
}