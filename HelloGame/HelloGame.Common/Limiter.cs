using System;
using System.Diagnostics;

namespace HelloGame.Common
{
    public class Limiter
    {
        private static readonly Stopwatch Stopwatch = Stopwatch.StartNew();
        private TimeSpan _lastEvent = TimeSpan.Zero;
        readonly TimeSpan _frequency;
        private object synchronizer = new object();

        public Limiter(TimeSpan frequency)
        {
            _frequency = frequency;
        }

        public bool CanHappen(bool willHappen = true)
        {
            lock (synchronizer)
            {
                TimeSpan nextEvent = _lastEvent.Add(_frequency);
                if (Stopwatch.Elapsed > nextEvent)
                {
                    if (willHappen)
                    {
                        _lastEvent = Stopwatch.Elapsed;
                    }
                    return true;
                }
                return false;
            }
        }
    }
}