using System.Threading;
using Ninject;
using Ninject.Modules;
using Ninject.Syntax;
using HelloGame.Common.Network;

namespace HelloGame.Client
{
    public class HelloGameClientNinjectBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IMessageTransciever>().To<MessageTransciever>();
            Bind<CancellationTokenSource>().ToConstant(new CancellationTokenSource()).InSingletonScope();
            Bind<HelloGameForm>().To<HelloGameForm>().WithConstructorArgument("showInitialForm", true);
        }
    }
}