using System;

namespace HelloGame.Common.Logging
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger(Type sourceType);
    }
}