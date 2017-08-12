using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace imageDeCap
{
    public partial class completeCover : Form
    {
        public completeCover(bool Gif = false)
        {
            this.Gif = Gif;
            InitializeComponent();

            UseBackCover = false;
            if (imageDeCap.Properties.Settings.Default.FreezeScreenOnRegionShot)
            {
                UseBackCover = true;
            }
            if (Gif)
            {
                UseBackCover = false;
            }
            if (UseBackCover)
            {
                ScreenCapturer cap = new ScreenCapturer();
                Bitmap fullSnapshot = cap.Capture(enmScreenCaptureMode.Screen);
                pictureBox1.Image = fullSnapshot;
                pictureBox1.SetBounds(0, 0, SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
            }
            else
            {
                this.Opacity = 0.005;
            }
            this.ShowInTaskbar = false;
        }
        bool UseBackCover;
        bool Gif = false;

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

        // Called by mainloop in now heuheuhe
        public void Updatee()
        {
            if(Program.ImageDeCap.GifCaptureTimer.Enabled) // Don't update if we are capturing a gif
            {
                return;
            }
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

            Program.ImageDeCap.updateSelectedArea(this, EnterPressed, EscapePressed, LmbDown, LmbUp, Lmb, Gif);
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
                Program.ImageDeCap.StopRecordingGif(this, false);
            }
            if (e.KeyCode == Keys.Escape)
            {
                EscapePressed = true;
                Program.ImageDeCap.StopRecordingGif(this, true);
            }
        }

        // this is called from Form1's updateSelectedArea when it considers itself done figuring out what region to capture.
        public void CompletedSelection()
        {
            if(!Gif) // If it's not a gif, hide everything and fire off an upload thread instantly.
            {
                Program.ImageDeCap.magn.Close();
                if (!UseBackCover)
                    this.Close();

                Program.ImageDeCap.topBox.Hide();
                Program.ImageDeCap.bottomBox.Hide();
                Program.ImageDeCap.leftBox.Hide();
                Program.ImageDeCap.rightBox.Hide();


                if (Program.ImageDeCap.tempWidth > 0 && Program.ImageDeCap.tempHeight > 0) // Make sure we actually selected a region to take a screenshot of.
                {
                    Utilities.playSound("snip.wav");
                    Bitmap result = Program.ImageDeCap.cap.Capture(
                        enmScreenCaptureMode.Bounds, 
                        Program.ImageDeCap.X - 1, 
                        Program.ImageDeCap.Y - 1,
                        Program.ImageDeCap.tempWidth + 1, 
                        Program.ImageDeCap.tempHeight + 1);

                    if (UseBackCover)
                        this.Close();

                    File.Delete(Path.GetTempPath() + "screenshot.png");
                    result.Save(Path.GetTempPath() + "screenshot.png");
                    result.Dispose();

                    Program.ImageDeCap.uploadImageFile(Path.GetTempPath() + "screenshot.png", imageDeCap.Properties.Settings.Default.EditScreenshotAfterCapture);
                }

                if (UseBackCover)
                    this.Close();

                Program.ImageDeCap.isTakingSnapshot = false;
                Program.hotkeysEnabled = true;
            }
            else
            {
                // From here, we fire up gif recording in Form1's main loop :D
                if (Program.ImageDeCap.tempWidth > 0 && Program.ImageDeCap.tempHeight > 0)
                {
                    Program.ImageDeCap.magn.Close();
                    Program.ImageDeCap.StartRecordingGif();
                }
            }
        }
    }
}
