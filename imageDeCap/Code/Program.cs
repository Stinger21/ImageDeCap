﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Reflection;

namespace imageDeCap
{
    internal static class NativeMethods
    {
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool SetProcessDpiAwarenessContext(int dpiFlag);

        [DllImport("SHCore.dll", SetLastError = true)]
        internal static extern bool SetProcessDpiAwareness(PROCESS_DPI_AWARENESS awareness);

        [DllImport("user32.dll")]
        internal static extern bool SetProcessDPIAware();

        internal enum PROCESS_DPI_AWARENESS
        {
            Process_DPI_Unaware = 0,
            Process_System_DPI_Aware = 1,
            Process_Per_Monitor_DPI_Aware = 2
        }

        internal enum DPI_AWARENESS_CONTEXT
        {
            DPI_AWARENESS_CONTEXT_UNAWARE = 16,
            DPI_AWARENESS_CONTEXT_SYSTEM_AWARE = 17,
            DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE = 18,
            DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 = 34
        }
    }
    
    static class Program
    {
        public static MainWindow ImageDeCap;
        public static bool hotkeysEnabled = true;
        public static bool Quit = false;

        [STAThread]
        static void Main(string[] args)
        {
            // Prefent the program from resizing and breaking with windows scaling. (handle scaling ourselves.)
            NativeMethods.SetProcessDpiAwarenessContext((int)NativeMethods.DPI_AWARENESS_CONTEXT.DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2);

            // Commands:
            // -ForceStartup:
            //      Forces the program to start even if there are other instances of the program already running.
            // 

            bool ForceStartup = false;
            if(args.Length > 0)
            {
                if(args[0].Contains("ForceStartup"))
                {
                    ForceStartup = true;
                    System.Threading.Thread.Sleep(2000);
                }
            }

            // stop from having multiple instances of this program running at the same time.
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1 && ForceStartup == false)
            {
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            ImageDeCap = new MainWindow();
            ImageDeCap.Initialize();
            
            Quit = false;

            while (!Quit)
            {
                Application.DoEvents();
                ImageDeCap.MainLoop();
                // WHAT
                for (int i = 0; i < BetterTimer.TimersMarkedForDeletion.Count; i++)
                {
                    BetterTimer.Timers.Remove(BetterTimer.TimersMarkedForDeletion[i]);
                    BetterTimer.TimersMarkedForDeletion.Remove(BetterTimer.TimersMarkedForDeletion[i]);
                    i--;
                }
                foreach (var item in BetterTimer.Timers)
                {
                    item.TickPrivate();
                }
                System.Threading.Thread.Sleep(1);
            }
        }

    }
}
