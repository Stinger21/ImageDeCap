using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//Me
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Media;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Diagnostics;
using System.Threading;


namespace imageDeCap
{
    // Form used to draw lines when capturing a screenshot
    public partial class ScreenshotRegionLine : Form
    {
        public ScreenshotRegionLine(bool grey = false)
        {
            InitializeComponent();
            this.Show();
            this.ShowInTaskbar = false;
            //this.Opacity = 0.5;
            this.TopMost = true;
            this.SetBounds(-10, -10, 0, 0);
        }

        //protected override void WndProc(ref Message m)
        //{
        //    int WM_NCHITTEST = 0x84;
        //    Int32 HTTRANSPARENT = -1;
        //
        //    if (m.Msg == (int)WM_NCHITTEST)
        //        m.Result = (IntPtr)HTTRANSPARENT;
        //    else
        //        base.WndProc(ref m);
        //}

        // Makes the form not show up in alt-tab
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // turn on WS_EX_TOOLWINDOW style bit
                cp.ExStyle |= 0x80;
                return cp;
            }
        }
    }
}
