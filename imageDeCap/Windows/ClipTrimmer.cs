using MediaToolkit;
using MediaToolkit.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace imageDeCap
{

    public partial class ClipTrimmer : Form
    {
        float scalePct = 1.0f;
        ImageEditor.EditorResult result = ImageEditor.EditorResult.Quit;
        Bitmap[] TheImage;
        Bitmap[] EditedImage;
        Bitmap CurrentImage;
        int frames = 0;
        int SavedImageStart = 0;
        int SavedImageEnd = 0;
        int CurrentFrame = 0;
        int ActualFrameNumber = 0;
        bool Scrolling = false;


        public static byte[] VideoFromFrames(Bitmap[] frames, decimal framerate = 15.21M, bool sound = false)
        {
            string outpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\imageDeCap\out.avi";
            string outcompressedpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\imageDeCap\out" + MainWindow.videoFormat;
            string outcompressedpathWithAudio = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\imageDeCap\out_sound" + MainWindow.videoFormat;
            
            VideoWriter.Write(new RecorderParams(outpath, framerate, SharpAvi.KnownFourCCs.Codecs.MotionJpeg, 100, 0, 0, frames[0].Width, frames[0].Height), frames);

            var inputFile = new MediaFile { Filename = outpath };
            var outputFile = new MediaFile { Filename = outcompressedpath };
            using (var engine = new Engine())
            {
                // Compress video
                ProgressWindow w = new ProgressWindow();
                w.SetProgress($"Compressing Video...", 50, 100);
                engine.Convert(inputFile, outputFile);
                w.Close();
                if (sound)
                {
                    w = new ProgressWindow();
                    w.SetProgress($"Adding sound...", 50, 100);
                    engine.CustomCommand($"-i \"{outcompressedpath}\" -i \"{SoundRecording.Mp3Path}\" -c:v copy -c:a aac -strict experimental \"{outcompressedpathWithAudio}\"");
                    engine.ConversionCompleteEvent += ConversionComplete;
                    while (Converting)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(1);
                    }
                        
                    w.Close();
                }
                else
                {
                    if (File.Exists(outcompressedpathWithAudio))
                        File.Delete(outcompressedpathWithAudio);
                    File.Move(outcompressedpath, outcompressedpathWithAudio);
                }
            }

            byte[] data = File.ReadAllBytes(outcompressedpathWithAudio);
            //File.Delete(outpath);
            //File.Delete(outcompressedpath);
            //File.Delete(outcompressedpathWithAudio);
            //File.Delete(SoundPath);
            return data;
        }
        static bool Converting = true;

        private static void ConversionComplete(object sender, ConversionCompleteEventArgs e)
        {
            Converting = false;
            Console.WriteLine("Conversion Complete");
        }

        string SecondsToString(double number)
        {
            TimeSpan t = TimeSpan.FromSeconds(number);
            return $"{t.Hours}:{t.Minutes}:{t.Seconds}.{t.Milliseconds}";
        }

        // Trun the image into a byte array
        public (ImageEditor.EditorResult output, Bitmap[] Data, bool Sound) FinalFunction()
        {
            if(result == ImageEditor.EditorResult.Quit)
            {
                return (result, new Bitmap[] { }, false);
            }
            bool Sound = SoundCheckbox.Checked;
            if (Sound)
            {
                double FrameTime = 1.0 / CompleteCover.FramesPerSecond;

                double StartTrimTime = SavedImageStart * FrameTime;
                double EndTrimTime = SavedImageEnd * FrameTime;
                //double Duration = EndTrimTime - StartTrimTime;

                string StartTrimTimeInt = SecondsToString(StartTrimTime);
                string EndTrimTimeInt = SecondsToString(EndTrimTime);
                //int DurationInt = (int)Duration;

                using (var engine = new Engine())
                {
                    string command = "";// $"-i \"{SoundRecording.Mp3Path}\" -vf trim=duration={StartTrimTimeInt}";
                    //command = $"-i \"{SoundRecording.Mp3Path}\" -ss 00:00:0{StartTrimTimeInt} -t 00:00:0{DurationInt} -c copy \"{SoundRecording.Mp3Path}\"";
                    command = $"-i \"{SoundRecording.Mp3PathUntrimmed}\" -ss {StartTrimTimeInt} -to {EndTrimTimeInt} -c copy \"{SoundRecording.Mp3Path}\"";

                    engine.CustomCommand(command); // trim
                }

            }
            // cleanup
            return (result, EditedImage, Sound);
        }

        public ClipTrimmer(Bitmap[] ImageData, int X, int Y, decimal FrameRate)
        {
            InitializeComponent();
            TheImage = ImageData;
            CurrentImage = TheImage[0];
            PictureBox.Image = CurrentImage;//.ToBitmap();

            PictureBox.Size = new Size(CurrentImage.Width, CurrentImage.Height);

            int width = Math.Max(CurrentImage.Width + 40 - 22, 200);
            int height = Math.Max(CurrentImage.Height + 63 + 93 - 80, 200);

            this.Size = new Size(width, height);
            this.frames = TheImage.Length;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(X - 7, Y - 20);

            startTrack.Maximum = frames;
            endTrack.Maximum = frames;
            endTrack.Value = frames;
            BackgroundTrack.Maximum = frames;


            UploadButton.Enabled = Preferences.uploadToFTP; // enabled if we upload to ftp or imgur
            if (Preferences.NeverUpload)
            {
                this.AcceptButton = SaveButton;
            }
            else
            {
                this.AcceptButton = UploadButton;
            }

            frameTimer.Interval = (int)(1000.0M / FrameRate);
        }
        
        private void Uploadbutton_Click(object sender, EventArgs e)
        {
            this.Hide();
            CalculateFileSizeAndSaveOutputImage();
            result = ImageEditor.EditorResult.Upload;
            this.Close();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            CalculateFileSizeAndSaveOutputImage();
            result = ImageEditor.EditorResult.Save;
            this.Close();
        }
        
        private void FrameTimer_Tick(object sender, EventArgs e)
        {
            if (Scrolling == false)
            {
                CurrentFrame++;
                ActualFrameNumber = (CurrentFrame % (endTrack.Value - startTrack.Value)) + startTrack.Value;
            }
            else
            {
                ActualFrameNumber = (CurrentFrame % TheImage.Length);
            }
            
            CurrentImage = TheImage[ActualFrameNumber];
            PictureBox.Image = CurrentImage;
            BackgroundTrack.Value = Math.Min(Math.Max(ActualFrameNumber, BackgroundTrack.Minimum), BackgroundTrack.Maximum);
        }
        
        private void StartTrack_ValueChanged(object sender, EventArgs e)
        {
            if(startTrack.Value == startTrack.Maximum)
            {
                startTrack.Value = startTrack.Maximum - 1;
            }
            if(startTrack.Value >= endTrack.Value)
            {
                endTrack.Value = startTrack.Value + 1;
            }
            SetTracks();
            CurrentFrame = startTrack.Value;
        }

        private void EndTrack_ValueChanged(object sender, EventArgs e)
        {
            if (endTrack.Value == 0)
            {
                endTrack.Value = 1;
            }
            if (endTrack.Value <= startTrack.Value)
            {
                startTrack.Value = endTrack.Value - 1;
            }
            SetTracks();
            CurrentFrame = endTrack.Value;// -1;
        }

        private void ClipTrimmer_Resize(object sender, EventArgs e)
        {
            SetTracks();
        }

        private void SetTracks()
        {
            float RightPercentage = (float)startTrack.Value / (float)endTrack.Maximum;
            float LeftPercentage = (float)endTrack.Value / (float)endTrack.Maximum;

            int RightWidth = (int)(RightPercentage * (startTrack.Width - 28)) + 20;
            int LeftWidth = (int)(LeftPercentage * (endTrack.Width - 28)) + 10;
            
            startTrack.Region = new Region(new Rectangle(0, 0, RightWidth, this.Height));
            endTrack.Region = new Region(new Rectangle(LeftWidth, 0, this.Width, this.Height));
        }
        
        int PositionToValue(int X, TrackBar Track)// int Width, int Min, int Max)
        {
            int result = Convert.ToInt32((((double)X - 13.0f) / ((double)Track.Width - 26.0)) * (Track.Maximum - Track.Minimum));
            result = Math.Max(result, 0);
            result = Math.Min(Math.Max(result, BackgroundTrack.Minimum), BackgroundTrack.Maximum);
            return result;
        }

        TrackBar ClosestTrack(int X, TrackBar A, TrackBar B)
        {
            int CurrentValue = PositionToValue(X, A); // any value
            int DistA = Math.Abs(A.Value - CurrentValue);
            int DistB = Math.Abs(B.Value - CurrentValue);
            if (DistA < DistB)
                return A;
            else
                return B;
        }

        private void EndTrack_MouseDown(object sender, MouseEventArgs e)
        {
            Scrolling = true;
            endTrack.Value = PositionToValue(e.X, endTrack);
        }
        private void EndTrack_MouseMove(object sender, MouseEventArgs e)
        {
            if (Scrolling)
                endTrack.Value = PositionToValue(e.X, endTrack);
        }


        private void StartTrack_MouseDown(object sender, MouseEventArgs e)
        {
            Scrolling = true;
            startTrack.Value = PositionToValue(e.X, startTrack);
        }
        private void StartTrack_MouseMove(object sender, MouseEventArgs e)
        {
            if (Scrolling)
                startTrack.Value = PositionToValue(e.X, startTrack);
        }


        private void BackgroundTrack_MouseDown(object sender, MouseEventArgs e)
        {
            Scrolling = true;
            var ChosenTrack = ClosestTrack(e.X, startTrack, endTrack);
            ChosenTrack.Value = PositionToValue(e.X, ChosenTrack);
            BackgroundTrack.Value = PositionToValue(e.X, BackgroundTrack);
        }
        private void BackgroundTrack_MouseMove(object sender, MouseEventArgs e)
        {
            if (Scrolling)
            {
                var ChosenTrack = ClosestTrack(e.X, startTrack, endTrack);
                ChosenTrack.Value = PositionToValue(e.X, ChosenTrack);
                BackgroundTrack.Value = PositionToValue(e.X, BackgroundTrack);
            }
        }

        private void EndTrack_MouseUp(object sender, MouseEventArgs e) { Scrolling = false; }
        private void StartTrack_MouseUp(object sender, MouseEventArgs e) { Scrolling = false; }
        private void BackgroundTrack_MouseUp(object sender, MouseEventArgs e) { Scrolling = false; }
        

        private void CalculateFileSizeAndSaveOutputImage()
        {
            //If this is true, that means the user changed nothing and we don't need to re-compute the image.
            if((SavedImageStart == (int)startTrack.Value) &&
              (SavedImageEnd == (int)endTrack.Value))
            {
                return;
            }
            SavedImageStart = (int)startTrack.Value;
            SavedImageEnd = (int)endTrack.Value;
            EditedImage = Utilities.SubArray(TheImage, SavedImageStart, SavedImageEnd - SavedImageStart);
        }

        private void CalcSizeButton_Click(object sender, EventArgs e)
        {
            CalculateFileSizeAndSaveOutputImage();
        }

        private void ClipTrimmer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "Escape")
            {
                this.Close();
            }
        }
        
        private void ScaleThing_ValueChanged(object sender, EventArgs e)
        {
            PictureBox.Size = new Size((int)(CurrentImage.Width * scalePct), (int)(CurrentImage.Height * scalePct));
        }
    }
}
