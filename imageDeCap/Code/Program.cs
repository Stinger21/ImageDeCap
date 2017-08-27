using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace imageDeCap
{
    static class Program
    {
        public static MainWindow ImageDeCap;
        public static bool hotkeysEnabled = true;
        [STAThread]
        static void Main(string[] args)
        {
            // Commands:
            // -ForceStartup:
            //      Forces the program to start even if there are other instances of the program already running.
            // -CaptureImage

            bool ForceStartup = false;
            if(args.Length > 0)
            {
                if(args[0].Contains("ForceStartup"))
                {
                    ForceStartup = true;
                    System.Threading.Thread.Sleep(2000);
                }
            }
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1 && ForceStartup == false)
            {
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            ImageDeCap = new MainWindow();

            Quit = false;
            while (!Quit)
            {
                Application.DoEvents();
                ImageDeCap.mainLoop();
                System.Threading.Thread.Sleep(10);
            } 
        }
        public static bool Quit;
    }
}
