using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

namespace imageDeCap
{
    static class Program
    {
        public static Form1 ImageDeCap;
        public static bool hotkeysEnabled = true;
        [STAThread]
        static void Main()
        {
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ImageDeCap = new Form1();
            ImageDeCap.FormClosed += QuitLoop;
            ImageDeCap.Show();

            while (!mQuit)
            {
                Application.DoEvents();
                ImageDeCap.mainLoop();
                System.Threading.Thread.Sleep(10);
            } 
        }
        private static bool mQuit;
        private static void QuitLoop(object sender, FormClosedEventArgs e)
        {
            mQuit = true;
        }
    }
}
