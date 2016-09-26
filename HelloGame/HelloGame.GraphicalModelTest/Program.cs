using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using HelloGame.Client;
using HelloGame.Common;
using HelloGame.Server;
using Ninject;
using Ninject.Syntax;

namespace HelloGame.GraphicalModelTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IKernel ninject = new StandardKernel(new HelloGameCommonNinjectBindings(true), new HelloGameServerNinjectBindings());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(ninject.Get<Form1>());
        }
    }
}
