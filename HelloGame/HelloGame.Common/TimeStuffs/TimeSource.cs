using System;
using System.Diagnostics;

namespace HelloGame.Common.TimeStuffs
{
    public class TimeSource
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public TimeSpan ElapsedSinceStart
        {
            get { return _stopwatch.Elapsed + _offset; }
        }

        private TimeSpan _offset = TimeSpan.Zero;

        public TimeSource(bool startStopwatch = true)
        {
            if (startStopwatch)
            {
                _stopwatch.Start();
            }
        }

        public void Pause()
        {
            _stopwatch.Stop();
        }

        public void Start()
        {
            _stopwatch.Start();
        }

        public void SkipTime(TimeSpan timeToSkip)
        {
            _offset = _offset.Add(timeToSkip);
        }
    }
}