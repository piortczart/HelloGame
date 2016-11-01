using HelloGame.Common.Logging;
using HelloGame.Common.Model;
using HelloGame.Common.Settings;
using HelloGame.Common.TimeStuffs;
using Ninject.Modules;

namespace HelloGame.Common
{
    public class HelloGameCommonNinjectBindings : NinjectModule
    {
        readonly bool _isServer;
        readonly bool _pauseTime;
        readonly GeneralSettings _generalSettings;

        public HelloGameCommonNinjectBindings(GeneralSettings generalSettings, HelloGameCommonBindingsType bindingsType,
            bool pauseTime = false)
        {
            _isServer = bindingsType == HelloGameCommonBindingsType.Server;
            _pauseTime = pauseTime;
            _generalSettings = generalSettings;
        }

        public override void Load()
        {
            Bind<GameThingCoordinator>().To<GameThingCoordinator>().InSingletonScope();
            Bind<ThingFactory>().To<ThingFactory>().InSingletonScope().WithConstructorArgument(typeof(bool), _isServer);
            Bind<GameManager>().To<GameManager>().InSingletonScope().WithConstructorArgument(typeof(bool), _isServer);
            Bind<ModelManager>().To<ModelManager>().InSingletonScope().WithConstructorArgument(typeof(bool), _isServer);

            Bind<ILoggerFactory>()
                .To<LoggerFactory>()
                .InSingletonScope()
                .WithConstructorArgument("extraInfo", _isServer ? "Server" : "Client");
            Bind<Overlay>().To<Overlay>().InSingletonScope();
            Bind<TimeSource>()
                .To<TimeSource>()
                .InSingletonScope()
                .WithConstructorArgument("startStopwatch", !_pauseTime);
            Bind<GeneralSettings>().ToConstant(_generalSettings);
            Bind<ThingBaseInjections>().To<ThingBaseInjections>().InSingletonScope();
        }
    }
}