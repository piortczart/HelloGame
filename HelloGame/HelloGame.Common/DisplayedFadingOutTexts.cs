using System;
using System.Collections.Generic;
using System.Linq;
using HelloGame.Common.TimeStuffs;

namespace HelloGame.Common
{
    public class DisplayedFadingOutTexts
    {
        private readonly List<DisplayedFadingOutText> _displayTexts = new List<DisplayedFadingOutText>();
        private readonly object _synchro = new object();

        private readonly TimeSource _timeSource;

        public DisplayedFadingOutTexts(TimeSource timeSource)
        {
            _timeSource = timeSource;
        }

        public void Add(string text, TimeSpan timeToLive, bool big = false)
        {
            lock (_synchro)
            {
                _displayTexts.Add(new DisplayedFadingOutText(_timeSource.ElapsedSinceStart, text, timeToLive, big));
            }
        }

        public IReadOnlyCollection<DisplayedFadingOutText> GetCurrent(bool big = false)
        {
            lock (_synchro)
            {
                return
                    _displayTexts.Where(t => t.IsCurrent(_timeSource.ElapsedSinceStart) && t.Big == big)
                        .OrderBy(t => t.ExpireTime)
                        .ToList()
                        .AsReadOnly();
            }
        }
    }
}