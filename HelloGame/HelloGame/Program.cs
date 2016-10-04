using System;
using System.Windows.Forms;
using HelloGame.Common;
using HelloGame.Server;
using Ninject;
using Ninject.Syntax;

namespace HelloGame.Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            GeneralSettings settings = GeneralSettings.Gameplay;
                //new GeneralSettings
                //{
                //    ShowThingIds = false
                //};

            // A separate server binding (in sense server is a separate application ran inside this process)
            IResolutionRoot serverNinject = new StandardKernel(new HelloGameCommonNinjectBindings(settings, true), new HelloGameServerNinjectBindings());
            // And regular client binding with a server ninject passed to construct a server with separate objects and configuration.
            IResolutionRoot clientNinject = new StandardKernel(new HelloGameCommonNinjectBindings(settings, false), new HelloGameClientNinjectBindings(serverNinject));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(clientNinject.Get<HelloGameForm>());
        }
    }
}
