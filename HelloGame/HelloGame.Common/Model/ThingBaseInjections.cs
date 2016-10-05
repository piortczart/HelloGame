using HelloGame.Common.Logging;

namespace HelloGame.Common.Model
{
    public class ThingBaseInjections
    {
        public TimeSource TimeSource { get; set; }
        public ILoggerFactory LoggerFactory { get; set; }
        public GeneralSettings GeneralSettings { get; set; }

        public ThingBaseInjections(TimeSource timeSource, ILoggerFactory loggerFactory, GeneralSettings generalSettings)
        {
            TimeSource = timeSource;
            LoggerFactory = loggerFactory;
            GeneralSettings = generalSettings;
        }
    }
}