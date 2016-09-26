using System;
using System.Threading;
using HelloGame.Common;
using Ninject;
using Ninject.Syntax;

namespace HelloGame.Server
{
    internal class Program
    {
        private static void Main()
        {
            IResolutionRoot ninject = new StandardKernel(new HelloGameCommonNinjectBindings(true), new HelloGameServerNinjectBindings());

            while (true)
            {
                try
                {
                    var cts = new CancellationTokenSource();
                    ninject.Get<GameServer>().Start(cts);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Thread.Sleep(10000);
                }
            }
        }
    }
}
