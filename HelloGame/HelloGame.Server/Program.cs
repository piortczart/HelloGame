using System;
using System.Threading;

namespace HelloGame.Server
{
    internal class Program
    {
        private static void Main()
        {
            while (true)
            {
                try
                {
                    new Server().Start();
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
