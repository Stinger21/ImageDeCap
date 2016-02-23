using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace imageDeCap
{
    public partial class completeCover : Form
    {
        Form1 mainProgram;
        public completeCover(Form1 mainProgram)
        {
            InitializeComponent();
            this.mainProgram = mainProgram;

            if (Properties.Settings.Default.FreezeScreenOnRegionShot)
            {
                ScreenCapturer cap = new ScreenCapturer();
                Bitmap fullSnapshot = cap.Capture(enmScreenCaptureMode.Screen);
                pictureBox1.Image = fullSnapshot;
                pictureBox1.SetBounds(0, 0, SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
            }
            else
            {
                this.Opacity = 0.05;
            }

            //SetBounds(SystemInformation.VirtualScreen.X, SystemInformation.VirtualScreen.Y, SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
            
            //this.Left = 0;
            //this.Top = 0;


            //this.Location = new Point(SystemInformation.VirtualScreen.X, SystemInformation.VirtualScreen.Y);
            //this.Size = new System.Drawing.Size(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);


            //System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
            //System.Drawing.Graphics formGraphics;
            //formGraphics = this.CreateGraphics();
            //formGraphics.FillRectangle(myBrush, new Rectangle(0, 0, 200, 300));
            //myBrush.Dispose();
            //formGraphics.Dispose();
            this.ShowInTaskbar = false;

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
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
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
                //MessageBox.Show("wa! ");
                escPressed = true;
                //this.Close();
            }
        }

    }
}
