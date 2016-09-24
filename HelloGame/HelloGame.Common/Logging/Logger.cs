using System;
using System.Diagnostics;

namespace HelloGame.Common.Logging
{
    public class Logger : ILogger
    {
        private readonly Type _sourceType;
        private readonly string _extraInfo;

        public Logger(Type sourceType, string extraInfo)
        {
            _sourceType = sourceType;
            _extraInfo = extraInfo;
        }

        public void LogInfo(string text)
        {
            Debug.WriteLine($"[{_extraInfo}] {_sourceType.Name}: {text}");
        }
    }
}