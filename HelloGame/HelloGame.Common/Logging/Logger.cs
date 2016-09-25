using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HelloGame.Common.Logging
{
    public class Logger : ILogger
    {
        private readonly Type _sourceType;
        private readonly string _extraInfo;
        private readonly SynchronizedCollection<Action<LogDetails>> _logActions;

        public Logger(Type sourceType, string extraInfo, SynchronizedCollection<Action<LogDetails>> logActions)
        {
            _sourceType = sourceType;
            _extraInfo = extraInfo;
            _logActions = logActions;
        }

        public void LogInfo(string text)
        {
            var logDetails = new LogDetails
            {
                Text = text,
                ExtraInfo = _extraInfo,
                Type = _sourceType,
                When = DateTime.Now
            };

            foreach (Action<LogDetails> logAction in _logActions)
            {
                logAction(logDetails);
            }

            Debug.WriteLine(FormatLog(logDetails));
        }

        public static string FormatLog(LogDetails details)
        {
            return $"{details.When:HH:mm:ss} [{details.ExtraInfo}] {details.Type.Name}: {details.Text}";
        }
    }
}