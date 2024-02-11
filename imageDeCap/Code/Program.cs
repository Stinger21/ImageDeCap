using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;

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

        static bool openWindow = false;
        public static void WaitForBringToFrontEventLoop()
        {
            while (true)
            {
                var server = new NamedPipeServerStream("MyNamedPipe", PipeDirection.InOut);
                server.WaitForConnection();
                openWindow = true;
                server.Close();
            }
        }

        [STAThread]
        static void Main(string[] args)
        {
            NativeMethods.SetProcessDpiAwarenessContext((int)NativeMethods.DPI_AWARENESS_CONTEXT.DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2);

            bool AlreadyRunning = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1;
            if (AlreadyRunning)
            {
                NamedPipeClientStream client = new NamedPipeClientStream(".", "MyNamedPipe", PipeDirection.InOut);
                client.Connect();
                client.Close();
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            ImageDeCap = new MainWindow();
            ImageDeCap.Initialize();

            new Thread(WaitForBringToFrontEventLoop).Start();

            Quit = false;
            while (!Quit)
            {
                Application.DoEvents();

                ClipboardHandler.Update();
                Hotkeys.Update();

                if (openWindow)
                {
                    openWindow = false;
                    ImageDeCap.OpenWindow();
                }
                System.Threading.Thread.Sleep(1);
            } 
        }
    }
}
