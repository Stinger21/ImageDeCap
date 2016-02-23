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
        //public static Form1 mainProgram;
        //[STAThread]
        //static void Main()
        //{
        //
        //
        //    bool createdNew = true;
        //    using (Mutex mutex = new Mutex(true, "MyApplicationName", out createdNew))
        //    {
        //        if (createdNew)
        //        {
        //            Application.EnableVisualStyles();
        //            Application.SetCompatibleTextRenderingDefault(false);
        //            Form1 mainProgram = new Form1();
        //            Application.Run(mainProgram);
        //        }
        //        else
        //        {
        //            Process current = Process.GetCurrentProcess();
        //            foreach (Process process in Process.GetProcessesByName(current.ProcessName))
        //            {
        //                if (process.Id != current.Id)
        //                {
        //                    //SetForegroundWindow(process.MainWindowHandle);
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}
        public static bool hotkeysEnabled = true;
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 f1 = new Form1();
            f1.FormClosed += QuitLoop;
            f1.Show();

            while (!mQuit)
            {
                     
                Application.DoEvents();

                f1.mainLoop();

                
                

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
