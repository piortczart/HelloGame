﻿using System;
using System.Windows.Forms;
using HelloGame.Common;
using HelloGame.Common.Settings;
using HelloGame.Server;
using Ninject;

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
            IKernel ninject = new StandardKernel(
                new HelloGameCommonNinjectBindings(GeneralSettings.TestingAll, HelloGameCommonBindingsType.Client),
                new HelloGameServerNinjectBindings());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(ninject.Get<Form1>());
        }
    }
}