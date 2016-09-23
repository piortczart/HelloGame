using System;
using System.Windows.Forms;
using HelloGame.Common.Model;

namespace HelloGame
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new HelloGameForm(new Renderer(new ModelManager())));
        }
    }
}
