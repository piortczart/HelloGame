using System;

namespace HelloGame.Common
{
    public class EventPerSecond
    {
        private readonly TimeSource _timeSource;
        private int _lastCounter;
        private int _coutner;
        private int _lastSecond;

        public EventPerSecond(TimeSource timeSource)
        {
            _timeSource = timeSource;
        }

        private object _synchro = new object();

        private int CurrentSecond => (int)Math.Floor(_timeSource.ElapsedSinceStart.TotalSeconds);

        public void Add()
        {
            lock (_synchro)
            {
                // The second has changed.
                if (CurrentSecond != _lastSecond)
                {
                    _lastSecond = CurrentSecond;
                    _lastCounter = _coutner;
                    _coutner = 0;
                }

                _coutner += 1;
            }
        }

        public long GetPerSecond()
        {
            lock (_synchro)
            {
                return _lastCounter;
            }
        }
    }
}