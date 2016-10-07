using System;
using System.Windows.Forms;

namespace HelloGame.Common.Extensions
{
    public static class ExtensionsControl
    {
        public static void Invoke(this Control control, Action action)
        {
            try
            {
                control.Invoke(action);
            }
            catch
            {
                // Ignore
            }
        }
    }
}