using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace imageDeCap
{
    // This class is responsible for covering the screen and recording pieces of it.

    public partial class CompleteCover : Form
    {

        // Final rectangle representing the VirtualScreen region selected
        public Rectangle SelectedRegion;

        // Initial location you clicked.
        Vector2 BoxStart;

        // Vector from mouse position to box corner.
        Vector2 BoxDelta;

        public List<Bitmap> CapturedClpFrames = new List<Bitmap>();
        public int RecordedTime = 0;
        public decimal RecordedFramerate = 0;
        int FramesCaptured = 0;
        DateTime LastTime;

        public ScreenshotRegionLine Box = new ScreenshotRegionLine();
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
        bool Clip = false;
        bool EscapePressed = false;
        
        bool wasPressed = false;
        bool AltKeyDown = false;
        bool ShiftKeyDown = false;

        public CompleteCover(bool Clip = false)
        {
            InitializeComponent();
            this.Clip = Clip;
            this.Opacity = 0.005f;
            this.ShowInTaskbar = false;

            FreezeScreen = imageDeCap.Preferences.FreezeScreenOnRegionShot;
            if (Clip)
                FreezeScreen = false;
        }

        void CaptureVideoHotkeyPressed()
        {
            StopRecordingClip(this, false);
        }

        public void AfterShow(Bitmap background, bool isClip)
        {
            if (isClip)
            {
                Hotkeys.CaptureVideoHotkeyPressed += CaptureVideoHotkeyPressed;
            }
            else
            {
                cancelButton.Visible = false;
                doneButton.Visible = false;
            }

            int x = SystemInformation.VirtualScreen.X;
            int y = SystemInformation.VirtualScreen.Y;
            int width = SystemInformation.VirtualScreen.Width;
            int height = SystemInformation.VirtualScreen.Height;

            if (FreezeScreen && !isClip)
            {
                this.TopMost = false;
                pictureBox1.Image = background;
                pictureBox1.SetBounds(0, 0, width, height);
                this.SetBounds(x, y, width, height);
                Application.DoEvents();
                this.Opacity = 1;
            }
            else
            {
                this.SetBounds(x, y, width, height);
            }
            //_Activated = true;
            BoxMovementTimer.Enabled = true;

            magnifier = new Magnifier(isClip);
            magnifier.Show();
            magnifier.TopMost = true;

            SetupBox(Box, true);
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
        Vector2 LastMouse;
        public void UpdateSelection()
        {
            Cursor.Current = Cursors.Cross;

            if (ClipCaptureTimer.Enabled) // Don't update if we are capturing a clip
                return;

            bool LmbUp = false;
            bool LmbDown = false;
            bool LmbIsDown = MouseButtons == MouseButtons.Left;
            bool RMB = MouseButtons == (MouseButtons.Left | MouseButtons.Right);

            Vector2 Mouse = new Vector2(Cursor.Position.X, Cursor.Position.Y);
            
            //Vector2 MouseDelta = Mouse - LastMouse;
            LastMouse = Mouse;
            //Mouse += MouseDelta * 0.25f;

            if (wasPressed != (MouseButtons == MouseButtons.Left))
            {
                LmbUp = wasPressed;
                LmbDown = !wasPressed;
            }


            // Moving things from MainWindow to here...
            this.Activate();
            magnifier.Bounds = new Rectangle((int)Mouse.X + 32, (int)Mouse.Y - 32, 124, 124);
            
            if (LmbDown)
            {
                BoxStart = Mouse;
            }

            // Holding M1
            if (LmbIsDown)
            {
                if (ShiftKeyDown)
                {
                    Vector2 MD = Mouse - BoxStart;

                    // angle value is 0 at all 90 degree angles and 1 at all 45 degree angles.
                    float angle = (float)Math.Abs(Frac(((Math.Atan2(MD.X, MD.Y) + 0.785380) / 3.141521 / 2.0) * 4.0) * 2.0 - 1.0);

                    int Average = (int)((float)(Math.Abs(MD.X) + Math.Abs(MD.Y)) / (angle + 1.0));
                    Mouse.X = (Average * Math.Sign(MD.X)) + BoxStart.X;
                    Mouse.Y = (Average * Math.Sign(MD.Y)) + BoxStart.Y;
                }

                if (AltKeyDown)
                {
                    BoxStart = Mouse + BoxDelta;
                }

                // Get vector from 
                BoxDelta = BoxStart - Mouse;

                SelectedRegion = Rectangle.FromLTRB((int)Math.Min(Mouse.X, BoxStart.X), (int)Math.Min(Mouse.Y, BoxStart.Y), (int)Math.Max(Mouse.X, BoxStart.X), (int)Math.Max(Mouse.Y, BoxStart.Y));

                // Magic numbers
                //Box.SetBounds(SelectedRegion.X, SelectedRegion.Y, SelectedRegion.Width, SelectedRegion.Height);

                topBox.SetBounds(SelectedRegion.X - 3 + 1, SelectedRegion.Y - 3 + 1, SelectedRegion.Width + 3, 0);
                leftBox.SetBounds(SelectedRegion.X - 3 + 1, SelectedRegion.Y - 1 + 1, 0, SelectedRegion.Height + 1);
                bottomBox.SetBounds(SelectedRegion.X - 3 + 1, SelectedRegion.Height + SelectedRegion.Y + 1, SelectedRegion.Width + 5, 0);
                rightBox.SetBounds(SelectedRegion.Width + SelectedRegion.X + 1, SelectedRegion.Y - 3 + 1, 0, SelectedRegion.Height + 3);

                if (Preferences.UseRuleOfThirds)
                {
                    ruleOfThirdsBox1.SetBounds(SelectedRegion.X + (SelectedRegion.Width / 3), SelectedRegion.Y, 0, SelectedRegion.Height);
                    ruleOfThirdsBox2.SetBounds(SelectedRegion.X + (SelectedRegion.Width / 3) * 2, SelectedRegion.Y, 0, SelectedRegion.Height);
                    ruleOfThirdsBox3.SetBounds(SelectedRegion.X, SelectedRegion.Y + (SelectedRegion.Height / 3), SelectedRegion.Width, 0);
                    ruleOfThirdsBox4.SetBounds(SelectedRegion.X, SelectedRegion.Y + (SelectedRegion.Height / 3) * 2, SelectedRegion.Width, 0);
                }

            }

            if (RMB)
            {
                EscapePressed = true;
            }
            if (LmbUp && !EscapePressed) // keyUp
            {
                CompletedSelection(RMB);
            }

            if (EscapePressed)
            {
                ScreenCapturer.CurrentBackCover.magnifier.Close();
                this.Close();

                Box.Hide();
                topBox.Hide();
                bottomBox.Hide();
                leftBox.Hide();
                rightBox.Hide();

                if (Preferences.UseRuleOfThirds)
                {
                    ruleOfThirdsBox1.Hide();
                    ruleOfThirdsBox2.Hide();
                    ruleOfThirdsBox3.Hide();
                    ruleOfThirdsBox4.Hide();
                }

                ScreenCapturer.IsTakingSnapshot = false;
                Program.hotkeysEnabled = true;
            }
            
            EscapePressed = false;
            wasPressed = MouseButtons == MouseButtons.Left;
        }
        
        private void CompleteCover_KeyDown(object sender, KeyEventArgs e)
        {
            if(ClipCaptureTimer.Enabled == false)
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
            if (!Clip) // If it's not a clip, hide everything and fire off an upload thread instantly.
            {
                magnifier.Close();
                if (!FreezeScreen)
                    this.Close();

                Box.Hide();
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
                // From here, we fire up clip recording
                if (SelectedRegion.Width > 0 && SelectedRegion.Height > 0)
                {
                    magnifier.Close();
                    StartRecordingClip(ForceEdit);

                    this.Location = new Point(SelectedRegion.X - 2, SelectedRegion.Y + SelectedRegion.Height + 3);
                    this.Width = Math.Max(SelectedRegion.Width, 300);
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
            StopRecordingClip(this, false);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            EscapePressed = true;
            StopRecordingClip(this, true);
        }

        DateTime ClipRecordStartTime;
        TimeSpan TotalRecordedTime;
        public void StartRecordingClip(bool ForceEdit)
        {
            RecordedTime = 0;
            FramesCaptured = 0;
            ClipCaptureTimer.Enabled = true;
            ClipCaptureTimer.Tag = ForceEdit;
            ClipCaptureTimer.Interval = 1000 / Preferences.RecordingFramerate;
            LastTime = DateTime.Now;

            Box.BackColor = Color.Green;
            topBox.BackColor = Color.Green;
            bottomBox.BackColor = Color.Green;
            leftBox.BackColor = Color.Green;
            rightBox.BackColor = Color.Green;

            ruleOfThirdsBox1.Hide();
            ruleOfThirdsBox2.Hide();
            ruleOfThirdsBox3.Hide();
            ruleOfThirdsBox4.Hide();
            SoundRecording.Start();//@"C:\Users\Stinger\Desktop\what.wav"
            ClipRecordStartTime = DateTime.Now;
        }
        public static double RecordedSeconds;
        public static double FramesPerSecond;
        
        public void StopRecordingClip(CompleteCover cover, bool abort)
        {
            if (ClipCaptureTimer.Enabled)
            {
                TotalRecordedTime = DateTime.Now - ClipRecordStartTime;
                double f = (double)FramesCaptured;
                RecordedSeconds = TotalRecordedTime.TotalMilliseconds / 1000.0;
                FramesPerSecond = f / RecordedSeconds;
                RecordedFramerate = (decimal)FramesPerSecond;
                ClipCaptureTimer.Enabled = false;
                Box.Hide();
                topBox.Hide();
                bottomBox.Hide();
                leftBox.Hide();
                rightBox.Hide();

                ruleOfThirdsBox1.Hide();
                ruleOfThirdsBox2.Hide();
                ruleOfThirdsBox3.Hide();
                ruleOfThirdsBox4.Hide();

                cover.Close();
                SoundRecording.Stop();
                if (abort)
                {
                    foreach (var v in CapturedClpFrames) { v.Dispose(); }
                    CapturedClpFrames.Clear();
                }
                else
                {
                    Utilities.PlaySound("snip.wav");

                    // Feed in through the tag weather the user right-clicked to force editor even when it's disabled.
                    ScreenCapturer.UploadImageData(new byte[] { }, Filetype.mp4, false, (bool)ClipCaptureTimer.Tag, CapturedClpFrames.ToArray());
                }

                ScreenCapturer.IsTakingSnapshot = false;
                Program.hotkeysEnabled = true;
            }
        }

        [DllImport("user32.dll")]
        static extern uint GetGuiResources(IntPtr hProcess, uint uiFlags);
        

        private void ClipCaptureTimer_Tick(object sender, EventArgs e)
        {

            this.BringToFront();
            this.TopMost = true;

            TimeSpan DeltaTime = DateTime.Now - LastTime;
            LastTime = DateTime.Now;
            
            RecordedTime += DeltaTime.Milliseconds;


            int width  = SelectedRegion.Width + 1;
            int height = SelectedRegion.Height + 1;
            if (width % 2 == 1)
                width = width - 1;
            if (height % 2 == 1)
                height = height - 1;
            // Capture Bitmap

            Bitmap b = ScreenCapturer.Capture(ScreenCaptureMode.Bounds, SelectedRegion.X, SelectedRegion.Y, width, height, true);

            CapturedClpFrames.Add(b);
            
            int seconds = (RecordedTime / 1000);
            //int csecs = (RecordedTime % 1000);
            float RecordedTimeSeconds = RecordedTime / 1000.0f;

            TimeLabel.Text = $"Time: {TimeSpan.FromSeconds(seconds).ToString()}";//.{csecs}
            FramesLabel.Text = $"Frames: {FramesCaptured + 1}";
            float RamLeft = MainWindow.ramCounter.NextValue() - 1000; // 1000 here is a buffer number to make sure the capturing stops before we run out of memory.
            MemoryLabel.Text = $"RAM left: {RamLeft} MB";
            TargetFramerateLabel.Text = $"TF: {Preferences.RecordingFramerate}";
            ActualFramerateLabel.Text = $"RF: {(int)((FramesCaptured + 1) / RecordedTimeSeconds)}";
            RecordedFramerate = (int)(((float)FramesCaptured + 1.0f) / RecordedTimeSeconds);

            FramesCaptured++;
            if (RamLeft <= 0)
            {
                StopRecordingClip(this, false);
            }

            // If the user pressesd esc, complete the recording and open the editor.
            string hotkey = Hotkeys.GetCurrentHotkey();
            if (hotkey == "Escape")
            {
                StopRecordingClip(this, false);
            }

            uint GDIHandles = GetGuiResources(Process.GetCurrentProcess().Handle, 0);
            uint UserHandles = GetGuiResources(Process.GetCurrentProcess().Handle, 0);

            //Almost out of GDI handles, stopping recording.
            if (UserHandles > 9000 || UserHandles > 9000)
            {
                StopRecordingClip(this, false);
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
