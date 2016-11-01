using System;
using System.Windows.Forms;
using HelloGame.Common;
using HelloGame.Common.Settings;
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
            GeneralSettings settings = GeneralSettings.CurrentSettings;

            // And regular client binding with a server ninject passed to construct a server with separate objects and configuration.
            IResolutionRoot clientNinject =
                new StandardKernel(new HelloGameCommonNinjectBindings(settings, HelloGameCommonBindingsType.Client),
                    new HelloGameClientNinjectBindings());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(clientNinject.Get<HelloGameForm>());
        }
    }
}