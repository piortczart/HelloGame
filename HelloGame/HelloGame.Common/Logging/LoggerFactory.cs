using System;
using System.Collections.Generic;

namespace HelloGame.Common.Logging
{
    public class LoggerFactory : ILoggerFactory
    {
        private readonly string _extraInfo;
        private readonly SynchronizedCollection<Action<LogDetails>> _logActions = new SynchronizedCollection<Action<LogDetails>>();


        public LoggerFactory(string extraInfo)
        {
            _extraInfo = extraInfo;
        }

        public ILogger CreateLogger(Type sourceType)
        {
            return new Logger(sourceType, _extraInfo, _logActions);
        }

        public void AddLogAction(Action<LogDetails> logAction)
        {
            _logActions.Add(logAction);
        }
    }
}