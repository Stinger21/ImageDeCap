using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
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
            this.Opacity = 0.005f;

            UseBackCover = false;
            if (imageDeCap.Preferences.FreezeScreenOnRegionShot)
            {
                UseBackCover = true;
            }
            if (Gif)
            {
                UseBackCover = false;
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

        bool AltKeyDown = false;

        bool Activated = false;
        public void AfterShow()
        {
            if (UseBackCover)
            {
                ScreenCapturer cap = new ScreenCapturer();
                this.TopMost = false;
                pictureBox1.Image = cap.Capture(enmScreenCaptureMode.Screen);
                pictureBox1.SetBounds(0, 0, SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
                this.SetBounds(SystemInformation.VirtualScreen.X, SystemInformation.VirtualScreen.Y, SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
                Application.DoEvents();
                this.Opacity = 1;
            }
            else
            {
                this.SetBounds(SystemInformation.VirtualScreen.X, SystemInformation.VirtualScreen.Y, SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
            }
            Activated = true;
        }
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
            if(!Activated)
            {
                return;
            }
            Cursor.Current = Cursors.Cross;

            if (Program.ImageDeCap.GifCaptureTimer.Enabled) // Don't update if we are capturing a gif
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

            Program.ImageDeCap.updateSelectedArea(this, EnterPressed, EscapePressed, LmbDown, LmbUp, Lmb, Gif, MouseButtons == (MouseButtons.Left | MouseButtons.Right), AltKeyDown);
            EnterPressed = false;
            EscapePressed = false;
            wasPressed = MouseButtons == MouseButtons.Left;
            LmbDown = false;
            LmbUp = false;
        }

        private void completeCover_KeyDown(object sender, KeyEventArgs e)
        {
            if(Program.ImageDeCap.GifCaptureTimer.Enabled == false)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    EscapePressed = true;
                    Program.ImageDeCap.StopRecordingGif(this, true);
                }
            }
            if(e.Alt)
            {
                AltKeyDown = true;
            }
        }

        private void completeCover_KeyUp(object sender, KeyEventArgs e)
        {
            AltKeyDown = false;
        }

        public static byte[] GetBytes(Image image, ImageFormat format)
        {
            var ms = new MemoryStream();
            image.Save(ms, format);
            return ms.ToArray();
        }
        // this is called from Form1's updateSelectedArea when it considers itself done figuring out what region to capture.
        public void CompletedSelection(bool ForceEdit = false)
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

                Program.ImageDeCap.ruleOfThirdsBox1.Hide();
                Program.ImageDeCap.ruleOfThirdsBox2.Hide();
                Program.ImageDeCap.ruleOfThirdsBox3.Hide();
                Program.ImageDeCap.ruleOfThirdsBox4.Hide();


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

                    Program.ImageDeCap.UploadImageData(GetBytes(result, ImageFormat.Png), MainWindow.filetype.png, false, ForceEdit);
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
                    Program.ImageDeCap.StartRecordingGif(ForceEdit);

                    this.Location = new Point(Program.ImageDeCap.X, Program.ImageDeCap.Y + Program.ImageDeCap.tempHeight);
                    this.Width = Math.Max(Program.ImageDeCap.tempWidth, 300);
                    this.Height = 50;
                    
                    this.ResumeLayout(false);
                    this.TopMost = true;

                    pictureBox1.Hide();

                    System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
                    System.Drawing.Graphics formGraphics;
                    formGraphics = this.CreateGraphics();
                    formGraphics.FillRectangle(myBrush, new Rectangle(0, 0, this.Width, this.Height));
                    myBrush.Dispose();
                    formGraphics.Dispose();
                    this.Opacity = 1;

                }
            }
        }
        public void SetTimer(string time, string frames)
        {
            label1.Text = time;
            label2.Text = frames;
        }
        private void doneButton_Click(object sender, EventArgs e)
        {
            EnterPressed = true;
            Program.ImageDeCap.StopRecordingGif(this, false);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            EscapePressed = true;
            Program.ImageDeCap.StopRecordingGif(this, true);
        }
        
    }
}
