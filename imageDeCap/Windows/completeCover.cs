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
    public partial class CompleteCover : Form
    {
        public CompleteCover(bool Gif = false)
        {
            InitializeComponent();
            this.Gif = Gif;
            this.Opacity = 0.005f;
            this.ShowInTaskbar = false;

            UseBackCover = false;
            if (imageDeCap.Preferences.FreezeScreenOnRegionShot)
                UseBackCover = true;

            if (Gif)
                UseBackCover = false;
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

        bool _Activated = false;
        public void AfterShow(Bitmap background)
        {
            float scaler = 1.0f/1.0f;

            int x = (int)(SystemInformation.VirtualScreen.X * scaler);
            int y = (int)(SystemInformation.VirtualScreen.Y * scaler);
            int width = (int)(SystemInformation.VirtualScreen.Width * scaler);
            int height = (int)(SystemInformation.VirtualScreen.Height * scaler);

            if (UseBackCover)
            {
                this.TopMost = false;
                pictureBox1.Image = background;
                pictureBox1.SetBounds(0, 0, width, height);
                this.SetBounds(x, y,        width, height);
                Application.DoEvents();
                this.Opacity = 1;
            }
            else
            {
                this.SetBounds(x, y, width, height);
            }
            _Activated = true;
        }

        private void completeCover_MouseMove(object sender, MouseEventArgs e)
        {
            Updatee();
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Updatee();
        }
        
        public void Updatee()
        {
            if(!_Activated)
                return;

            Cursor.Current = Cursors.Cross;

            if (Program.ImageDeCap.GifCaptureTimer.Enabled) // Don't update if we are capturing a gif
                return;

            Lmb = MouseButtons == MouseButtons.Left;
            if(wasPressed != (MouseButtons == MouseButtons.Left))
            {
                LmbUp = wasPressed;
                LmbDown = !wasPressed;
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
                Program.ImageDeCap.magnifier.Close();
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
                    Bitmap result = ScreenCapturer.Capture(
                        ScreenCaptureMode.Bounds, 
                        Program.ImageDeCap.X - 1, 
                        Program.ImageDeCap.Y - 1,
                        Program.ImageDeCap.tempWidth + 1, 
                        Program.ImageDeCap.tempHeight + 1);

                    if (UseBackCover)
                        this.Close();

                    Program.ImageDeCap.UploadImageData(GetBytes(result, ImageFormat.Png), Filetype.png, false, ForceEdit);
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
                    Program.ImageDeCap.magnifier.Close();
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

        public void SetTimer(string time, string frames, string Size)
        {
            TimeLabel.Text = time;
            FramesLabel.Text = frames;
            MemoryLabel.Text = Size;
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
