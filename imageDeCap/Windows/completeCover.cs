﻿using System;
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
        public int X = 0;
        public int Y = 0;
        public int tempX = 0;
        public int tempY = 0;
        public int tempWidth = 0;
        public int tempHeight = 0;
        public int LastCursorX = 0;
        public int LastCursorY = 0;

        public List<Bitmap> gEnc = new List<Bitmap>();
        DateTime LastTime;
        public int RecordedTime = 0;
        int FramesCaptured = 0;
        public int ActualFramerate = 0;
        public ScreenshotRegionLine topBox = new ScreenshotRegionLine();
        public ScreenshotRegionLine bottomBox = new ScreenshotRegionLine();
        public ScreenshotRegionLine leftBox = new ScreenshotRegionLine();
        public ScreenshotRegionLine rightBox = new ScreenshotRegionLine();
        public ScreenshotRegionLine ruleOfThirdsBox1 = new ScreenshotRegionLine(true);
        public ScreenshotRegionLine ruleOfThirdsBox2 = new ScreenshotRegionLine(true);
        public ScreenshotRegionLine ruleOfThirdsBox3 = new ScreenshotRegionLine(true);
        public ScreenshotRegionLine ruleOfThirdsBox4 = new ScreenshotRegionLine(true);

        public Magnifier magnifier;

        bool FreezeScreen;
        bool Gif = false;
        bool EscapePressed = false;
        bool LmbDown = false;
        bool LmbUp = false;
        bool Lmb = false;
        bool wasPressed = false;
        bool AltKeyDown = false;
        bool _Activated = false;
        
        public CompleteCover(bool Gif = false)
        {
            InitializeComponent();
            this.Gif = Gif;
            this.Opacity = 0.005f;
            this.ShowInTaskbar = false;

            FreezeScreen = imageDeCap.Preferences.FreezeScreenOnRegionShot;
            if (Gif)
                FreezeScreen = false;
        }

        void CaptureVideoHotkeyPressed()
        {
            StopRecordingGif(this, false);
        }

        public void AfterShow(Bitmap background, bool isGif)
        {
            float scaler = 1.0f/1.0f;

            if(isGif)
            {
                Hotkeys.CaptureVideoHotkeyPressed += CaptureVideoHotkeyPressed;
            }

            int x = (int)(SystemInformation.VirtualScreen.X * scaler);
            int y = (int)(SystemInformation.VirtualScreen.Y * scaler);
            int width = (int)(SystemInformation.VirtualScreen.Width * scaler);
            int height = (int)(SystemInformation.VirtualScreen.Height * scaler);

            if (FreezeScreen)
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
            //_Activated = true;
            BoxMovementTimer.Enabled = true;

            magnifier = new Magnifier(isGif);
            magnifier.Show();
            magnifier.TopMost = true;

            SetupBox(topBox, true);
            SetupBox(leftBox, true);
            SetupBox(bottomBox, true);
            SetupBox(rightBox, true);

            SetupBox(ruleOfThirdsBox1, false);
            SetupBox(ruleOfThirdsBox2, false);
            SetupBox(ruleOfThirdsBox3, false);
            SetupBox(ruleOfThirdsBox4, false);

            tempWidth = 0;
            tempHeight = 0;
        }

        private void SetupBox(ScreenshotRegionLine box, bool grey)
        {
            box.Show();
            box.ShowInTaskbar = false;
            box.BackColor = grey ? Color.Red : Color.Gray;
            box.Opacity = 0.5;
            box.SetBounds(0, 0, 0, 0);
            box.TopMost = true;
        }

        private void CompleteCover_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void BoxMovementTimer_Tick(object sender, EventArgs e)
        {
            UpdateSelection();
        }

        public void UpdateSelection()
        {
            Cursor.Current = Cursors.Cross;

            if (GifCaptureTimer.Enabled) // Don't update if we are capturing a gif
                return;

            Lmb = MouseButtons == MouseButtons.Left;
            if(wasPressed != (MouseButtons == MouseButtons.Left))
            {
                LmbUp = wasPressed;
                LmbDown = !wasPressed;
            }

            // Moving things from MainWindow to here...
            this.Activate();
            magnifier.Bounds = new Rectangle(Cursor.Position.X + 32, Cursor.Position.Y - 32, 124, 124);
            bool RMB = MouseButtons == (MouseButtons.Left | MouseButtons.Right);
            if(RMB)
            {
                EscapePressed = true;
            }
            if (LmbUp && !EscapePressed) // keyUp
            {
                CompletedSelection(RMB);
            }
            
            if (LmbDown)
            {
                tempX = Cursor.Position.X;
                tempY = Cursor.Position.Y;
            }

            // Holding M1
            if (Lmb)
            {
                // Magic numbers
                topBox.SetBounds(       X - 3 + 1,                Y - 3 + 1,              tempWidth + 3,      0);
                leftBox.SetBounds(      X - 3 + 1,                Y - 1 + 1,              0,                  tempHeight + 1);
                bottomBox.SetBounds(    X - 3 + 1,                tempHeight + Y + 1,     tempWidth + 5,      0);
                rightBox.SetBounds(     tempWidth + X + 1,        Y - 3 + 1,              0,                  tempHeight + 3);

                if (Preferences.UseRuleOfThirds)
                {
                    ruleOfThirdsBox1.SetBounds(X + (tempWidth / 3), Y, 0, tempHeight);
                    ruleOfThirdsBox2.SetBounds(X + (tempWidth / 3) * 2, Y, 0, tempHeight);
                    ruleOfThirdsBox3.SetBounds(X, Y + (tempHeight / 3), tempWidth, 0);
                    ruleOfThirdsBox4.SetBounds(X, Y + (tempHeight / 3) * 2, tempWidth, 0);
                }

                tempWidth = Math.Abs(Cursor.Position.X - tempX);
                tempHeight = Math.Abs(Cursor.Position.Y - tempY);

                X = tempX;
                Y = tempY;

                if ((Cursor.Position.Y - tempY) < 0)
                    Y = tempY + (Cursor.Position.Y - tempY);

                if ((Cursor.Position.X - tempX) < 0)
                    X = tempX + (Cursor.Position.X - tempX);

                if (AltKeyDown)
                {
                    tempX += Cursor.Position.X - LastCursorX;
                    tempY += Cursor.Position.Y - LastCursorY;
                }
                LastCursorX = Cursor.Position.X;
                LastCursorY = Cursor.Position.Y;
            }

            if (EscapePressed)
            {
                ScreenCapturer.CurrentBackCover.magnifier.Close();
                this.Close();

                topBox.Hide();
                bottomBox.Hide();
                leftBox.Hide();
                rightBox.Hide();

                ruleOfThirdsBox1.Hide();
                ruleOfThirdsBox2.Hide();
                ruleOfThirdsBox3.Hide();
                ruleOfThirdsBox4.Hide();

                ScreenCapturer.IsTakingSnapshot = false;
                Program.hotkeysEnabled = true;
            }
            
            EscapePressed = false;
            wasPressed = MouseButtons == MouseButtons.Left;
            LmbDown = false;
            LmbUp = false;
        }
        
        private void CompleteCover_KeyDown(object sender, KeyEventArgs e)
        {
            if(GifCaptureTimer.Enabled == false)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    EscapePressed = true;
                }
            }
            if(e.Alt)
            {
                AltKeyDown = true;
            }
        }

        private void CompleteCover_KeyUp(object sender, KeyEventArgs e)
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
            Cursor.Current = Cursors.Default;
            BoxMovementTimer.Enabled = false;
            if (!Gif) // If it's not a gif, hide everything and fire off an upload thread instantly.
            {
                magnifier.Close();
                if (!FreezeScreen)
                    this.Close();

                topBox.Hide();
                bottomBox.Hide();
                leftBox.Hide();
                rightBox.Hide();

                ruleOfThirdsBox1.Hide();
                ruleOfThirdsBox2.Hide();
                ruleOfThirdsBox3.Hide();
                ruleOfThirdsBox4.Hide();

                if (tempWidth > 0 && tempHeight > 0) // Make sure we actually selected a region to take a screenshot of.
                {
                    Utilities.PlaySound("snip.wav");
                    Bitmap result = ScreenCapturer.Capture(
                        ScreenCaptureMode.Bounds, 
                        X, 
                        Y,
                        tempWidth + 1, 
                        tempHeight + 1);

                    if (FreezeScreen)
                        this.Close();

                    ScreenCapturer.UploadImageData(GetBytes(result, ImageFormat.Png), Filetype.png, false, ForceEdit);
                }

                if (FreezeScreen)
                    this.Close();

                ScreenCapturer.IsTakingSnapshot = false;
                Program.hotkeysEnabled = true;
            }
            else
            {
                // From here, we fire up gif recording in Form1's main loop :D
                if (tempWidth > 0 && tempHeight > 0)
                {
                    magnifier.Close();
                    StartRecordingGif(ForceEdit);

                    this.Location = new Point(X - 2, Y + tempHeight + 3);
                    this.Width = Math.Max(tempWidth, 300);
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
        
        private void DoneButton_Click(object sender, EventArgs e)
        {
            StopRecordingGif(this, false);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            EscapePressed = true;
            StopRecordingGif(this, true);
        }
        
        public void StartRecordingGif(bool ForceEdit)
        {
            GifCaptureTimer.Interval = (int)(1000.0f / Preferences.RecordingFramerate);
            Console.WriteLine(GifCaptureTimer.Interval);
            Console.WriteLine(Preferences.RecordingFramerate);
            RecordedTime = 0;
            FramesCaptured = 0;
            GifCaptureTimer.Enabled = true;
            GifCaptureTimer.Tag = ForceEdit;
            LastTime = DateTime.Now;

            topBox.BackColor = Color.Green;
            bottomBox.BackColor = Color.Green;
            leftBox.BackColor = Color.Green;
            rightBox.BackColor = Color.Green;

            ruleOfThirdsBox1.Hide();
            ruleOfThirdsBox2.Hide();
            ruleOfThirdsBox3.Hide();
            ruleOfThirdsBox4.Hide();
        }

        public void StopRecordingGif(CompleteCover cover, bool abort)
        {
            if (GifCaptureTimer.Enabled)
            {
                //RecordedTime /= FramesCaptured;
                GifCaptureTimer.Enabled = false;
                topBox.Hide();
                bottomBox.Hide();
                leftBox.Hide();
                rightBox.Hide();

                ruleOfThirdsBox1.Hide();
                ruleOfThirdsBox2.Hide();
                ruleOfThirdsBox3.Hide();
                ruleOfThirdsBox4.Hide();

                cover.Close();
                if (!abort)
                {
                    Utilities.PlaySound("snip.wav");

                    // Feed in through the tag weather the user right-clicked to force editor even when it's disabled.
                    ScreenCapturer.UploadImageData(new byte[] { }, Filetype.gif, false, (bool)GifCaptureTimer.Tag, gEnc.ToArray());
                }

                ScreenCapturer.IsTakingSnapshot = false;
                Program.hotkeysEnabled = true;
            }
        }

        private void GifCaptureTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan DeltaTime = DateTime.Now - LastTime;
            LastTime = DateTime.Now;
            
            //int DeltaTimeInMS = DeltaTime.Seconds * 100 + DeltaTime.Milliseconds;
            RecordedTime += DeltaTime.Milliseconds;


            int width  = tempWidth + 1;
            int height = tempHeight + 1;
            if (width % 2 == 1)
                width = width - 1;
            if (height % 2 == 1)
                height = height - 1;
            // Capture Bitmap
            Bitmap b = ScreenCapturer.Capture(
                ScreenCaptureMode.Bounds,
                X - 1,
                Y - 1,
                width,
                height, true);

            gEnc.Add(b);

            //int minutes = (RecordedTime / 1000 / 60) % 60;
            int seconds = (RecordedTime / 1000);
            int csecs = RecordedTime % 1000;
            float RecordedTimeSeconds = RecordedTime / 1000.0f;

            TimeLabel.Text = $"Time: {seconds}.{csecs}";
            FramesLabel.Text = $"Frames: {FramesCaptured + 1}";
            MemoryLabel.Text = $"Memory Usage: {(gEnc.Count * width * height * 8) / 1000000} MB";
            TargetFramerateLabel.Text = $"TF: {Preferences.RecordingFramerate}";
            ActualFramerateLabel.Text = $"RF: {(int)((FramesCaptured + 1) / RecordedTimeSeconds)}";
            ActualFramerate = (int)(((float)FramesCaptured + 1.0f) / RecordedTimeSeconds);

            FramesCaptured++;
        }
        
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
