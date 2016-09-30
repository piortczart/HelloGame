using System;

namespace HelloGame.Common.Logging
{
    public interface ILogger
    {
        void LogInfo(string text);
        void LogError(string text, Exception error = null);
    }
}