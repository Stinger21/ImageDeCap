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
        public completeCover()
        {
            InitializeComponent();

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
            this.ShowInTaskbar = false;

        }
        bool EnterPressed = false;
        bool EscapePressed = false;

        bool LmbDown = false;
        bool LmbUp = false;
        bool Lmb = false;

        bool wasPressed = false;

        private void completeCover_Load(object sender, EventArgs e)
        {
            
        }

        private void completeCover_MouseMove(object sender, MouseEventArgs e)
        {
            Updatee();
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Updatee();
        }
        private void Updatee()
        {
            Lmb = MouseButtons == MouseButtons.Left;
            if(wasPressed != (MouseButtons == MouseButtons.Left))
            {
                if(wasPressed)
                {
                    LmbUp = true;
                }
                else
                {
                    LmbDown = true;
                }
            }

            Program.ImageDeCap.updateSelectedArea(this, EnterPressed, EscapePressed, LmbDown, LmbUp, Lmb);
            EnterPressed = false;
            EscapePressed = false;
            wasPressed = MouseButtons == MouseButtons.Left;
            LmbDown = false;
            LmbUp = false;
        }

        private void completeCover_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                EnterPressed = true;
            }
            if (e.KeyCode == Keys.Escape)
            {
                EscapePressed = true;
            }
        }

    }
}
