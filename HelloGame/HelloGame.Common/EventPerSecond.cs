using System;
using System.Diagnostics;

namespace HelloGame.Common
{
    public class EventPerSecond
    {
        readonly Stopwatch _stopwatch = Stopwatch.StartNew();
        private long _lastCounter;
        private long _coutner;
        private long _lastSecond;

        private int CurrentSecond => (int)Math.Floor(_stopwatch.Elapsed.TotalSeconds);

        public void Add()
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

        public long GetPerSecond()
        {
            return _lastCounter;
        }
    }
}