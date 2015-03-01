using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.Threading;

namespace imageDeCap
{
    static class Program
    {
        public static Form1 mainProgram;
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 mainProgram = new Form1();
            Application.Run(mainProgram);
        }


    }
}
