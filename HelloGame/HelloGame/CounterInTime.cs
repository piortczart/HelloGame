using System;
using System.Diagnostics;

namespace HelloGame
{
    public class CounterInTime
    {
        readonly Stopwatch _stopwatch = Stopwatch.StartNew();
        TimeSpan _time;
        private int _lastCounter;
        private int _coutner;
        private int _lastPiece;

        private int CurrentPiece => (int)Math.Floor(_stopwatch.Elapsed.TotalMilliseconds / _time.TotalMilliseconds);

        public CounterInTime(TimeSpan time)
        {
            _time = time;
        }

        public void Add()
        {
            if (CurrentPiece != _lastPiece)
            {
                _lastPiece = CurrentPiece;
                _lastCounter = _coutner;
                _coutner = 0;
            }

            _coutner += 1;
        }

        public decimal GetPerTime()
        {
            return (decimal)(_lastCounter / _time.TotalSeconds);
        }
    }
}