using HelloGame.Common.Logging;
using HelloGame.Common.Settings;

namespace HelloGame.Common.Model
{
    public class ThingBaseInjections
    {
        public TimeSource TimeSource { get; set; }
        public ILoggerFactory LoggerFactory { get; set; }
        public GeneralSettings GeneralSettings { get; set; }
        public GameThingCoordinator Coordinator { get; set; }

        public ThingBaseInjections(TimeSource timeSource, ILoggerFactory loggerFactory, GeneralSettings generalSettings,
            GameThingCoordinator coordinator)
        {
            TimeSource = timeSource;
            LoggerFactory = loggerFactory;
            GeneralSettings = generalSettings;
            Coordinator = coordinator;
        }
    }
}