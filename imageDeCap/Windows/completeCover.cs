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
    // This class is responsible for covering the screen and recording pieces of it.

    public partial class CompleteCover : Form
    {
        protected PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");

        // Final rectangle representing the VirtualScreen region selected
        public Rectangle SelectedRegion;

        // Initial location you clicked.
        int BoxStartX = 0;
        int BoxStartY = 0;

        // Vector from mouse position to box corner.
        int BoxDeltaX = 0;
        int BoxDeltaY = 0;

        public List<Bitmap> CapturedClpFrames = new List<Bitmap>();
        public int RecordedTime = 0;
        public int RecordedFramerate = 0;
        int FramesCaptured = 0;
        DateTime LastTime;

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
        
        bool wasPressed = false;
        bool AltKeyDown = false;
        bool ShiftKeyDown = false;

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
            else
            {
                cancelButton.Visible = false;
                doneButton.Visible = false;
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

            SelectedRegion.Width = 0;
            SelectedRegion.Height = 0;
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
        double Frac(double a)
        {
            return a - Math.Floor(a);
        }
        public void UpdateSelection()
        {
            Cursor.Current = Cursors.Cross;

            if (GifCaptureTimer.Enabled) // Don't update if we are capturing a gif
                return;

            bool LmbUp = false;
            bool LmbDown = false;
            bool LmbIsDown = MouseButtons == MouseButtons.Left;
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
                BoxStartX = Cursor.Position.X;
                BoxStartY = Cursor.Position.Y;
            }

            // Holding M1
            if (LmbIsDown)
            {
                int MouseX = Cursor.Position.X;
                int MouseY = Cursor.Position.Y;

                if (ShiftKeyDown)
                {
                    int MXD = MouseX - BoxStartX;
                    int MYD = MouseY - BoxStartY;

                    // angle value is 0 at all 90 degree angles and 1 at all 45 degree angles.
                    float angle = (float)Math.Abs(Frac(((Math.Atan2(MXD, MYD) + 0.785380) / 3.141521 / 2.0) * 4.0) * 2.0 - 1.0);

                    int Average = (int)((float)(Math.Abs(MXD) + Math.Abs(MYD)) / (angle + 1.0));
                    MouseX = (Average * Math.Sign(MXD)) + BoxStartX;
                    MouseY = (Average * Math.Sign(MYD)) + BoxStartY;
                }

                if (AltKeyDown)
                {
                    BoxStartX  = Cursor.Position.X + BoxDeltaX;
                    BoxStartY  = Cursor.Position.Y + BoxDeltaY;
                }

                // Get vector from 
                BoxDeltaX = BoxStartX - Cursor.Position.X;
                BoxDeltaY = BoxStartY - Cursor.Position.Y;

                SelectedRegion = Rectangle.FromLTRB(Math.Min(MouseX, BoxStartX),
                                                    Math.Min(MouseY, BoxStartY),
                                                    Math.Max(MouseX, BoxStartX),
                                                    Math.Max(MouseY, BoxStartY));
                
                // Magic numbers
                topBox.SetBounds(       SelectedRegion.X - 3 + 1,                SelectedRegion.Y - 3 + 1,              SelectedRegion.Width + 3,      0);
                leftBox.SetBounds(      SelectedRegion.X - 3 + 1,                SelectedRegion.Y - 1 + 1,              0,                  SelectedRegion.Height + 1);
                bottomBox.SetBounds(    SelectedRegion.X - 3 + 1,                SelectedRegion.Height + SelectedRegion.Y + 1,     SelectedRegion.Width + 5,      0);
                rightBox.SetBounds(     SelectedRegion.Width + SelectedRegion.X + 1,        SelectedRegion.Y - 3 + 1,              0,                  SelectedRegion.Height + 3);

                if (Preferences.UseRuleOfThirds)
                {
                    ruleOfThirdsBox1.SetBounds(SelectedRegion.X + (SelectedRegion.Width / 3), SelectedRegion.Y, 0, SelectedRegion.Height);
                    ruleOfThirdsBox2.SetBounds(SelectedRegion.X + (SelectedRegion.Width / 3) * 2, SelectedRegion.Y, 0, SelectedRegion.Height);
                    ruleOfThirdsBox3.SetBounds(SelectedRegion.X, SelectedRegion.Y + (SelectedRegion.Height / 3), SelectedRegion.Width, 0);
                    ruleOfThirdsBox4.SetBounds(SelectedRegion.X, SelectedRegion.Y + (SelectedRegion.Height / 3) * 2, SelectedRegion.Width, 0);
                }

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
            if(e.Shift)
            {
                ShiftKeyDown = true;
            }
        }

        private void CompleteCover_KeyUp(object sender, KeyEventArgs e)
        {
            if (!e.Alt)
            {
                AltKeyDown = false;
            }
            if (!e.Shift)
            {
                ShiftKeyDown = false;
            }
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

                if (SelectedRegion.Width > 0 && SelectedRegion.Height > 0) // Make sure we actually selected a region to take a screenshot of.
                {
                    Utilities.PlaySound("snip.wav");
                    Bitmap result = ScreenCapturer.Capture(
                        ScreenCaptureMode.Bounds, 
                        SelectedRegion.X, 
                        SelectedRegion.Y,
                        SelectedRegion.Width + 1, 
                        SelectedRegion.Height + 1);

                    if (FreezeScreen)
                        this.Close();

                    ScreenCapturer.UploadImageData(Utilities.GetBytes(result, ImageFormat.Png), Filetype.png, false, ForceEdit, null, SelectedRegion);
                }

                if (FreezeScreen)
                    this.Close();

                ScreenCapturer.IsTakingSnapshot = false;
                Program.hotkeysEnabled = true;
            }
            else
            {
                // From here, we fire up gif recording in Form1's main loop :D
                if (SelectedRegion.Width > 0 && SelectedRegion.Height > 0)
                {
                    magnifier.Close();
                    StartRecordingGif(ForceEdit);

                    this.Location = new Point(SelectedRegion.X - 2, SelectedRegion.Y + SelectedRegion.Height + 3);
                    this.Width = Math.Max(SelectedRegion.Width, 300);
                    this.Height = 50;

                    //int BottomDistance = Screen.FromControl(this).Bounds.Height - this.Location.Y + 2;
                    //int TopDistance = (this.Location.Y - SelectedRegion.Height) -3;
                    //int RightDistance = Screen.FromControl(this).Bounds.Width - SelectedRegion.Width + 2;
                    //int LeftDistance = (this.Location.X);
                    //
                    //int TaskbarHeight = 200;
                    //
                    //int ControlWidth = 278;
                    //int ControlHeight = 47;
                    //
                    //bool BottomAvailable = BottomDistance > TaskbarHeight + ControlHeight;
                    //bool TopAvailable = BottomDistance > TaskbarHeight + ControlHeight;
                    //bool LeftAvailable = BottomDistance > TaskbarHeight + ControlHeight;
                    //bool RightAvailable = BottomDistance > TaskbarHeight + ControlHeight;

                    // If it's near the bottom of the screen
                    //if (this.Location.Y > Screen.FromControl(this).Bounds.Height - 200)
                    //{
                    //    this.Location = new Point(SelectedRegion.X - 2, SelectedRegion.Y - 50 + 3);
                    //}

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
            RecordedTime = 0;
            FramesCaptured = 0;
            GifCaptureTimer.Enabled = true;
            GifCaptureTimer.Tag = ForceEdit;
            GifCaptureTimer.Interval = 1000 / Preferences.RecordingFramerate;
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
                if(abort)
                {
                    foreach (var v in CapturedClpFrames) { v.Dispose(); }
                    CapturedClpFrames.Clear();
                }
                else
                {
                    Utilities.PlaySound("snip.wav");

                    // Feed in through the tag weather the user right-clicked to force editor even when it's disabled.
                    ScreenCapturer.UploadImageData(new byte[] { }, Filetype.gif, false, (bool)GifCaptureTimer.Tag, CapturedClpFrames.ToArray());
                }

                ScreenCapturer.IsTakingSnapshot = false;
                Program.hotkeysEnabled = true;
            }
        }

        private void GifCaptureTimer_Tick(object sender, EventArgs e)
        {
            this.BringToFront();
            this.TopMost = true;

            TimeSpan DeltaTime = DateTime.Now - LastTime;
            LastTime = DateTime.Now;
            
            //int DeltaTimeInMS = DeltaTime.Seconds * 100 + DeltaTime.Milliseconds;
            RecordedTime += DeltaTime.Milliseconds;


            int width  = SelectedRegion.Width + 1;
            int height = SelectedRegion.Height + 1;
            if (width % 2 == 1)
                width = width - 1;
            if (height % 2 == 1)
                height = height - 1;
            // Capture Bitmap
            Bitmap b = ScreenCapturer.Capture(
                ScreenCaptureMode.Bounds,
                SelectedRegion.X - 1,
                SelectedRegion.Y - 1,
                width,
                height, true);

            CapturedClpFrames.Add(b);

            //int minutes = (RecordedTime / 1000 / 60) % 60;
            int seconds = (RecordedTime / 1000);
            int csecs = (RecordedTime % 1000);
            float RecordedTimeSeconds = RecordedTime / 1000.0f;

            TimeLabel.Text = $"Time: {seconds}";//.{csecs}
            FramesLabel.Text = $"Frames: {FramesCaptured + 1}";
            //MemoryLabel.Text = $"Memory Usage: {(FramesCaptured * SelectedRegion.Width * SelectedRegion.Height * 8L) / 1000000L} MB"; // marked L (int64) because the standard int32's would overflow.
            float RamLeft = ramCounter.NextValue() - 500;
            MemoryLabel.Text = $"RAM left: {RamLeft} MB";
            TargetFramerateLabel.Text = $"TF: {Preferences.RecordingFramerate}";
            ActualFramerateLabel.Text = $"RF: {(int)((FramesCaptured + 1) / RecordedTimeSeconds)}";
            RecordedFramerate = (int)(((float)FramesCaptured + 1.0f) / RecordedTimeSeconds);

            FramesCaptured++;
            if (RamLeft <= 0)
            {
                StopRecordingGif(this, false);
            }

            // If the user pressesd esc, complete the recording and open the editor.
            string hotkey = Hotkeys.GetCurrentHotkey();
            if (hotkey == "Escape")
            {
                StopRecordingGif(this, false);
            }

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
