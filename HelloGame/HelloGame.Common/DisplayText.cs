﻿using System;

namespace HelloGame.Common
{
    public class DisplayText
    {
        public string Text { get; set; }

        public TimeSpan ExpireTime { get; }

        public bool Big { get; }

        public DisplayText(TimeSpan currentTime, string text, TimeSpan timeToLive, bool big)
        {
            ExpireTime = currentTime.Add(timeToLive);
            Text = text;
            Big = big;
        }

        public bool IsCurrent(TimeSpan currentTime)
        {
            return currentTime < ExpireTime;
        }
    }
}