using imageDeCap.Properties;
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

        public List<Bitmap> CapturedClipFrames = new List<Bitmap>();
        public int RecordedTime = 0;
        public decimal RecordedFramerate = 0;
        int FramesCaptured = 0;
        DateTime LastTime;

        bool FreezeScreen;
        bool Clip = false;
        bool EscapePressed = false;
        
        bool wasPressed = false;
        bool AltKeyDown = false;
        bool ShiftKeyDown = false;

        public CompleteCover()
        {
            InitializeComponent();
            this.Opacity = 0.005f;
            this.ShowInTaskbar = false;
        }

        void CaptureVideoHotkeyPressed()
        {
            StopRecordingClip(false);
        }

        public void AfterShow(Bitmap background, bool isClip)
        {
            this.Clip = isClip;

            FreezeScreen = imageDeCap.Preferences.FreezeScreenOnRegionShot;
            if (Clip)
                FreezeScreen = false;

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
                Hotkeys.watch = System.Diagnostics.Stopwatch.StartNew(); // 1
                this.TopMost = false;
                pictureBox1.Image = background;
                pictureBox1.SetBounds(0, 0, width, height);
                this.SetBounds(x, y, width, height);
                //pictureBox1.Update();
                Application.DoEvents();
                this.Opacity = 1;
                Hotkeys.watch.Stop();
                Console.WriteLine(Hotkeys.watch.ElapsedMilliseconds);
            }
            else
            {
                this.SetBounds(x, y, width, height);
            }
            Hotkeys.watch = System.Diagnostics.Stopwatch.StartNew(); // 2
            BoxMovementTimer.Enabled = true;

            //ScreenCapturer.magnifier.Show();
            ScreenCapturer.magnifier.TopMost = true;

            ScreenCapturer.Box.SetBounds(-10, -10, 0, 0);
            ScreenCapturer.Box.BackColor = Color.Red;
            ScreenCapturer.topBox.SetBounds(-10, -10, 0, 0);
            ScreenCapturer.topBox.BackColor = Color.Red;
            ScreenCapturer.leftBox.SetBounds(-10, -10, 0, 0);
            ScreenCapturer.leftBox.BackColor = Color.Red;
            ScreenCapturer.bottomBox.SetBounds(-10, -10, 0, 0);
            ScreenCapturer.bottomBox.BackColor = Color.Red;
            ScreenCapturer.rightBox.SetBounds(-10, -10, 0, 0);
            ScreenCapturer.rightBox.BackColor = Color.Red;
            if (Preferences.UseRuleOfThirds)
            {
                ScreenCapturer.ruleOfThirdsBox1.SetBounds(-10, -10, 0, 0);
                ScreenCapturer.ruleOfThirdsBox1.BackColor = Color.Gray;
                ScreenCapturer.ruleOfThirdsBox2.SetBounds(-10, -10, 0, 0);
                ScreenCapturer.ruleOfThirdsBox2.BackColor = Color.Gray;
                ScreenCapturer.ruleOfThirdsBox3.SetBounds(-10, -10, 0, 0);
                ScreenCapturer.ruleOfThirdsBox3.BackColor = Color.Gray;
                ScreenCapturer.ruleOfThirdsBox4.SetBounds(-10, -10, 0, 0);
                ScreenCapturer.ruleOfThirdsBox4.BackColor = Color.Gray;

            }

            SelectedRegion.Width = 0;
            SelectedRegion.Height = 0;
            Hotkeys.watch.Stop();
            Console.WriteLine(Hotkeys.watch.ElapsedMilliseconds);
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

        [DllImport("User32.dll")]
        public static extern Int32 SetForegroundWindow(int hWnd);

        public void UpdateSelection()
        {
            SetForegroundWindow(Handle.ToInt32());

            Cursor.Current = Cursors.Cross;

            if (ClipCaptureTimer2 != null) // Don't update if we are capturing a clip
                return;

            bool LmbUp = false;
            bool LmbDown = false;
            bool LmbIsDown = MouseButtons == MouseButtons.Left;
            bool RMB = MouseButtons == (MouseButtons.Left | MouseButtons.Right);

            Vector2 Mouse = new Vector2(Cursor.Position.X, Cursor.Position.Y);
            
            if (wasPressed != (MouseButtons == MouseButtons.Left))
            {
                LmbUp = wasPressed;
                LmbDown = !wasPressed;
            }

            this.Activate();
            ScreenCapturer.magnifier.Bounds = new Rectangle((int)Mouse.X + 32, (int)Mouse.Y - 32, 124, 124);
            
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

                ScreenCapturer.topBox.SetBounds(SelectedRegion.X - 3 + 1, SelectedRegion.Y - 3 + 1, SelectedRegion.Width + 3, 0);
                ScreenCapturer.leftBox.SetBounds(SelectedRegion.X - 3 + 1, SelectedRegion.Y - 1 + 1, 0, SelectedRegion.Height + 1);
                ScreenCapturer.bottomBox.SetBounds(SelectedRegion.X - 3 + 1, SelectedRegion.Height + SelectedRegion.Y + 1, SelectedRegion.Width + 5, 0);
                ScreenCapturer.rightBox.SetBounds(SelectedRegion.Width + SelectedRegion.X + 1, SelectedRegion.Y - 3 + 1, 0, SelectedRegion.Height + 3);

                if (Preferences.UseRuleOfThirds)
                {
                    ScreenCapturer.ruleOfThirdsBox1.SetBounds(SelectedRegion.X + (SelectedRegion.Width / 3), SelectedRegion.Y, 0, SelectedRegion.Height);
                    ScreenCapturer.ruleOfThirdsBox2.SetBounds(SelectedRegion.X + (SelectedRegion.Width / 3) * 2, SelectedRegion.Y, 0, SelectedRegion.Height);
                    ScreenCapturer.ruleOfThirdsBox3.SetBounds(SelectedRegion.X, SelectedRegion.Y + (SelectedRegion.Height / 3), SelectedRegion.Width, 0);
                    ScreenCapturer.ruleOfThirdsBox4.SetBounds(SelectedRegion.X, SelectedRegion.Y + (SelectedRegion.Height / 3) * 2, SelectedRegion.Width, 0);
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
                Hide();
                HideBoxes();

                ScreenCapturer.IsTakingSnapshot = false;
                Program.hotkeysEnabled = true;
            }
            
            EscapePressed = false;
            wasPressed = MouseButtons == MouseButtons.Left;
        }
        void Hide()
        {
            this.SetBounds(-10, -10, 0, 0);
            BoxMovementTimer.Enabled = false;
            ScreenCapturer.magnifier.Location = new Point(-20000, -20000);
        }
        private void CompleteCover_KeyDown(object sender, KeyEventArgs e)
        {
            if(ClipCaptureTimer2 == null)
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

        public static void HideBoxes()
        {
            ScreenCapturer.Box.SetBounds(-10, -10, 0, 0);
            ScreenCapturer.topBox.SetBounds(-10, -10, 0, 0);
            ScreenCapturer.bottomBox.SetBounds(-10, -10, 0, 0);
            ScreenCapturer.leftBox.SetBounds(-10, -10, 0, 0);
            ScreenCapturer.rightBox.SetBounds(-10, -10, 0, 0);
            if (Preferences.UseRuleOfThirds)
            {
                ScreenCapturer.ruleOfThirdsBox1.SetBounds(-10, -10, 0, 0);
                ScreenCapturer.ruleOfThirdsBox2.SetBounds(-10, -10, 0, 0);
                ScreenCapturer.ruleOfThirdsBox3.SetBounds(-10, -10, 0, 0);
                ScreenCapturer.ruleOfThirdsBox4.SetBounds(-10, -10, 0, 0);
            }
        }

        // this is called from Form1's updateSelectedArea when it considers itself done figuring out what region to capture.
        public void CompletedSelection(bool ForceEdit = false)
        {
            Cursor.Current = Cursors.Default;

            BoxMovementTimer.Enabled = false;
            ScreenCapturer.magnifier.Location = new Point(-20000, -20000);

            HideBoxes();

            // From here, we fire up clip recording
            if (SelectedRegion.Width > 0 && SelectedRegion.Height > 0)
            {
                if (!Clip) // If it's not a clip, hide everything and fire off an upload thread instantly.
                {
                    if (!FreezeScreen)
                        Hide();

                    Utilities.PlaySound("snip.wav");
                    Bitmap result = ScreenCapturer.Capture(
                        ScreenCaptureMode.Bounds,
                        SelectedRegion.X,
                        SelectedRegion.Y,
                        SelectedRegion.Width + 1,
                        SelectedRegion.Height + 1);

                    if (FreezeScreen)
                        Hide();

                    ScreenCapturer.UploadImageData(Utilities.GetBytes(result, ImageFormat.Png), Filetype.png, false, ForceEdit, null, SelectedRegion);
                }
                else
                {
                    ScreenCapturer.magnifier.Location = new Point(-20000, -20000);
                    StartRecordingClip(ForceEdit);

                    this.Location = new Point(SelectedRegion.X - 2, SelectedRegion.Y + SelectedRegion.Height + 3);
                    this.Width = Math.Max(SelectedRegion.Width, 300);
                    this.Height = 50;

                    this.ResumeLayout(false);
                    //this.TopMost = true;

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
            else
            {
                Hide();
            }

            if (FreezeScreen)
                Hide();

            ScreenCapturer.IsTakingSnapshot = false;
            Program.hotkeysEnabled = true;
        }
        
        private void DoneButton_Click(object sender, EventArgs e)
        {
            StopRecordingClip(false);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            EscapePressed = true;
            StopRecordingClip(true);
        }
        BetterTimer ClipCaptureTimer2;

        DateTime ClipRecordStartTime;
        TimeSpan TotalRecordedTime;
        public void StartRecordingClip(bool ForceEdit)
        {
            RecordedTime = 0;
            FramesCaptured = 0;
            //ClipCaptureTimer.Enabled = true;
            //ClipCaptureTimer.Tag = ForceEdit;
            //ClipCaptureTimer.Interval = 1000 / Preferences.RecordingFramerate;
            ClipCaptureTimer2 = new BetterTimer();
            ClipCaptureTimer2.FramesPerSecond = Preferences.RecordingFramerate;
            ClipCaptureTimer2.Tag = ForceEdit;
            ClipCaptureTimer2.Tick += ClipCaptureTimer_Tick;

            LastTime = DateTime.Now;

            ScreenCapturer.Box.BackColor = Color.Green;
            ScreenCapturer.topBox.BackColor = Color.Green;
            ScreenCapturer.bottomBox.BackColor = Color.Green;
            ScreenCapturer.leftBox.BackColor = Color.Green;
            ScreenCapturer.rightBox.BackColor = Color.Green;

            SoundRecording.Start();
            ClipRecordStartTime = DateTime.Now;
        }
        public static double RecordedSeconds;
        public static double FramesPerSecond;
        
        public void StopRecordingClip(bool abort)
        {
            if (ClipCaptureTimer2 != null)
            {
                TotalRecordedTime = DateTime.Now - ClipRecordStartTime;
                double f = (double)FramesCaptured;
                RecordedSeconds = TotalRecordedTime.TotalMilliseconds / 1000.0;
                FramesPerSecond = f / RecordedSeconds;
                RecordedFramerate = (decimal)FramesPerSecond;

                Hide();
                SoundRecording.Stop();
                if (abort)
                {
                    foreach (var v in CapturedClipFrames) { v.Dispose(); }
                    CapturedClipFrames.Clear();
                }
                else
                {
                    Utilities.PlaySound("snip.wav");

                    // Feed in through the tag weather the user right-clicked to force editor even when it's disabled.
                    ScreenCapturer.UploadImageData(new byte[] { }, Filetype.mp4, false, (bool)ClipCaptureTimer2.Tag, CapturedClipFrames.ToArray());
                }
                HideBoxes();
                ClipCaptureTimer2.Dispose();
                ClipCaptureTimer2 = null;

                ScreenCapturer.IsTakingSnapshot = false;
                Program.hotkeysEnabled = true;
            }
        }

        [DllImport("user32.dll")]
        static extern uint GetGuiResources(IntPtr hProcess, uint uiFlags);
        
        private void ClipCaptureTimer_Tick()
        {
            this.BringToFront();
            this.TopMost = true;

            TimeSpan DeltaTime = DateTime.Now - LastTime;
            LastTime = DateTime.Now;
            
            RecordedTime += DeltaTime.Milliseconds;
            
            int width = SelectedRegion.Width + 1;
            int height = SelectedRegion.Height + 1;
            if (width % 2 == 1)
                width -= 1;
            if (height % 2 == 1)
                height -= 1;

            Bitmap b = ScreenCapturer.Capture(ScreenCaptureMode.Bounds, SelectedRegion.X, SelectedRegion.Y, width, height, true);

            CapturedClipFrames.Add(b);
            
            int seconds = (RecordedTime / 1000);
            float RecordedTimeSeconds = RecordedTime / 1000.0f;
            float RamLeft = 1;
            if (MainWindow.ramCounter != null)
                RamLeft = MainWindow.ramCounter.NextValue() - 1000; // 1000 here is a buffer number to make sure the capturing stops before we run out of memory.
            RecordedFramerate = (int)(((float)FramesCaptured + 1.0f) / RecordedTimeSeconds);
            
            TimeLabel.Text = $"Time: {(DateTime.Now - ClipRecordStartTime).ToString(@"mm\:ss\.ff")}";
            FramesLabel.Text = $"Frames: {FramesCaptured + 1}";
            MemoryLabel.Text = $"RAM left: {RamLeft} MB";
            TargetFramerateLabel.Text = $"TF: {Preferences.RecordingFramerate}";
            ActualFramerateLabel.Text = $"RF: {(int)Math.Round((FramesCaptured + 1) / RecordedTimeSeconds)}";
            
            FramesCaptured++;
            if (RamLeft <= 0)
            {
                StopRecordingClip(false);
            }

            // If the user pressesd esc, complete the recording and open the editor.
            string hotkey = Hotkeys.GetCurrentHotkey();
            if (hotkey == "Escape")
            {
                StopRecordingClip(false);
            }

            uint GDIHandles = GetGuiResources(Process.GetCurrentProcess().Handle, 0);
            uint UserHandles = GetGuiResources(Process.GetCurrentProcess().Handle, 0);
            
            //Almost out of GDI handles, stopping recording.
            if (UserHandles > 9000 || UserHandles > 9000)
            {
                StopRecordingClip(false);
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
