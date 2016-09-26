using HelloGame.Common.Logging;
using HelloGame.Common.Model;
using Ninject.Modules;

namespace HelloGame.Common
{
    public class HelloGameCommonNinjectBindings : NinjectModule
    {
        readonly bool _isServer;

        public HelloGameCommonNinjectBindings(bool isServer)
        {
            _isServer = isServer;
        }

        public override void Load()
        {
            Bind<GameThingCoordinator>().To<GameThingCoordinator>().InSingletonScope();
            Bind<IThingFactory>().To<ThingFactory>().InSingletonScope().WithConstructorArgument(typeof(bool), _isServer);
            Bind<GameManager>().To<GameManager>().InSingletonScope().WithConstructorArgument(typeof(bool), _isServer);
            Bind<ModelManager>().To<ModelManager>().InSingletonScope();
            Bind<ILoggerFactory>().To<LoggerFactory>().InSingletonScope().WithConstructorArgument("extraInfo", _isServer ? "Server" : "Client");
        }
    }
}