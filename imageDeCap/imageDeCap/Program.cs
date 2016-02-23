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
        public static Form1 mainProgram;
        [STAThread]
        static void Main()
        {


            bool createdNew = true;
            using (Mutex mutex = new Mutex(true, "MyApplicationName", out createdNew))
            {
                if (createdNew)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Form1 mainProgram = new Form1();
                    Application.Run(mainProgram);
                }
                else
                {
                    Process current = Process.GetCurrentProcess();
                    foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                    {
                        if (process.Id != current.Id)
                        {
                            //SetForegroundWindow(process.MainWindowHandle);
                            break;
                        }
                    }
                }
            }
        }


    }
}
