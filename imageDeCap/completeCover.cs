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
    public partial class completeCover : Form
    {
        Form1 mainProgram;
        public completeCover(Form1 mainProgram)
        {
            InitializeComponent();
            this.mainProgram = mainProgram;
        }
        bool keyPressed = false;
        bool escPressed = false;

        private void completeCover_Load(object sender, EventArgs e)
        {

        }
        /*
        int WM_NCHITTEST = 0x84;
        Int32 HTTRANSPARENT = -1;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)WM_NCHITTEST)
                m.Result = (IntPtr)HTTRANSPARENT;
            else
                base.WndProc(ref m);
        }
        */

        private void completeCover_MouseMove(object sender, MouseEventArgs e)
        {
            mainProgram.updateSelectedArea(this, keyPressed, escPressed);
            keyPressed = false;
            escPressed = false;
        }

        private void completeCover_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                keyPressed = true;
            }
            if (e.KeyCode == Keys.Escape)
            {
                escPressed = true;
            }
        }
    }
}
