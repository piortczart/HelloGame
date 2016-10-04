using System;

namespace HelloGame.Common
{
    public class TimeCounter
    {
        private TimeSource _timeSource;
        private TimeSpan _lastCall = TimeSpan.Zero;
        private bool _isFirstCall = true;

        public TimeCounter(TimeSource timeSource)
        {
            _timeSource = timeSource;
        }

        public TimeSpan GetTimeSinceLastCall()
        {
            TimeSpan totalTimeElapsed = _timeSource.ElapsedSinceStart;
            var result = totalTimeElapsed - _lastCall;

            // First time calling this method? Return 0.
            if (_isFirstCall)
            {
                result = TimeSpan.Zero;
                _isFirstCall = false;
            }

            _lastCall = totalTimeElapsed;
            return result;
        }
    }
}
