using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace screenshotsPls
{
    public partial class boxOfWhy : Form
    {
        public boxOfWhy()
        {
            InitializeComponent();
        }

        private void boxOfWhy_Load(object sender, EventArgs e)
        {

        }
        int WM_NCHITTEST = 0x84;
        Int32 HTTRANSPARENT = -1;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)WM_NCHITTEST)
                m.Result = (IntPtr)HTTRANSPARENT;
            else
                base.WndProc(ref m);
        }
    }
}
