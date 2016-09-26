using System.Threading;
using Ninject;
using Ninject.Modules;
using Ninject.Syntax;

namespace HelloGame.Client
{
    public class HelloGameClientNinjectBindings : NinjectModule
    {
        private readonly IResolutionRoot _serverNinject;

        public HelloGameClientNinjectBindings(IResolutionRoot serverNinject)
        {
            _serverNinject = serverNinject;
        }

        public override void Load()
        {
            Bind<Server.GameServer>().ToConstant(_serverNinject.Get<Server.GameServer>()).InSingletonScope();
            Bind<CancellationTokenSource>().ToConstant(new CancellationTokenSource()).InSingletonScope();
            Bind<HelloGameForm>().To<HelloGameForm>().WithConstructorArgument("showInitialForm", true);
        }
    }
}