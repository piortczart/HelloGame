using Ninject.Modules;

namespace HelloGame.Server
{
    public class HelloGameServerNinjectBindings :NinjectModule
    {
        public override void Load()
        {
            Bind<GameServer>().To<GameServer>().InSingletonScope();

        }
    }
}