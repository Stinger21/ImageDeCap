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
            this.ShowInTaskbar = false;

        }
        bool keyPressed = false;
        bool escPressed = false;

        private void completeCover_Load(object sender, EventArgs e)
        {


        }
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
                escPressed = true;
            }
        }

    }
}
