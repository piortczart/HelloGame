using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HelloGame.Common;
using HelloGame.Common.Settings;
using Ninject;
using Ninject.Syntax;

namespace HelloGame.Server
{
    internal class Program
    {
        private static void Main()
        {
            bool showForm = false;

            IResolutionRoot ninject =
                new StandardKernel(
                    new HelloGameCommonNinjectBindings(GeneralSettings.Gameplay, true),
                    new HelloGameServerNinjectBindings());

            var cts = new CancellationTokenSource();

            Task serverTask = Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        ninject.Get<GameServer>().Start(cts).Wait(cts.Token);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        Thread.Sleep(10000);
                    }
                }
            }, cts.Token);

            if (showForm)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(ninject.Get<ServerSpectatorForm>());
            }
            else
            {
                serverTask.Wait(cts.Token);
            }
        }
    }
}