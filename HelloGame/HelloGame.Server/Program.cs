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
            IResolutionRoot ninject = new StandardKernel(new HelloGameCommonNinjectBindings(true));

            while (true)
            {
                try
                {
                    ninject.Get<GameServer>().Start();
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
