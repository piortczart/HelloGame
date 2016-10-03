using System;
using System.Diagnostics;

namespace HelloGame.Common
{
    public class EventPerSecond
    {
        readonly Stopwatch _stopwatch = Stopwatch.StartNew();
        private int _lastCounter;
        private int _coutner;
        private int _lastSecond;

        private object _synchro = new object();

        private int CurrentSecond => (int)Math.Floor(_stopwatch.Elapsed.TotalSeconds);

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