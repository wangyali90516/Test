using System;
using static System.Windows.Forms.Application;

namespace RedisToTableTool
{
    internal static class Program
    {
        /// <summary>
        ///     应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            EnableVisualStyles();
            SetCompatibleTextRenderingDefault(false);
            Run(new Form1());
        }
    }
}