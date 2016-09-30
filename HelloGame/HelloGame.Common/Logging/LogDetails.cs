using System;

namespace HelloGame.Common.Logging
{
    public class LogDetails
    {
        public string Text { get; set; }
        public string ExtraInfo { get; set; }
        public Type Type { get; set; }
        public DateTime When { get; set; }
        public Exception Exception { get; set; }
    }
}