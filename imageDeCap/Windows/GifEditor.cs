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

    public partial class GifEditor : Form
    {
        public static byte[] VideoFromFrames(Bitmap[] frames, int framerate = 15)
        {
            string outpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\imageDeCap\out.avi";
            string outcompressedpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\imageDeCap\out" + MainWindow.videoFormat;

            VideoWriter.Write(new RecorderParams(outpath, framerate, SharpAvi.KnownFourCCs.Codecs.MotionJpeg, 100, 0, 0, frames[0].Width, frames[0].Height), frames);

            var inputFile = new MediaFile { Filename = outpath };
            var outputFile = new MediaFile { Filename = outcompressedpath };
            using (var engine = new Engine())
            {
                engine.Convert(inputFile, outputFile);
            }

            byte[] data = File.ReadAllBytes(outcompressedpath);
            File.Delete(outpath);
            File.Delete(outcompressedpath);
            return data;
        }

        // Trun the image into a byte array
        public (NewImageEditor.EditorResult output, byte[] Data) FinalFunction()
        {
            if(result == NewImageEditor.EditorResult.Quit)
            {
                return (result, new byte[] { });
            }

            Bitmap[] ScaledImages = null;
            bool edited = scalePct != 1.0f;
            if (edited)
            {
                ScaledImages = new Bitmap[EditedImage.Length];
                for (int i = 0; i < EditedImage.Length; i++)
                {
                    int newWidth = (int)((float)EditedImage[i].Width * scalePct);
                    int newHeight = (int)((float)EditedImage[i].Height * scalePct);
                    ScaledImages[i] = new Bitmap(newWidth, newHeight);
                    using (Graphics gr = Graphics.FromImage(ScaledImages[i]))
                    {
                        gr.SmoothingMode = SmoothingMode.HighQuality;
                        gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        gr.CompositingQuality = CompositingQuality.HighQuality;
                        gr.DrawImage(EditedImage[i], new Rectangle(0, 0, newWidth, newHeight));
                    }
                }
            }

            // cleanup
            return (result, VideoFromFrames(edited ? ScaledImages : EditedImage, FrameRate));
        }

        NewImageEditor.EditorResult result = NewImageEditor.EditorResult.Quit;

        Bitmap[] TheImage;
        Bitmap[] EditedImage;
        Bitmap CurrentImage;

        int frames = 0;
        int FrameRate = 15;
        public GifEditor(Bitmap[] ImageData, int X, int Y, int FrameRate)
        {
            InitializeComponent();
            TheImage = ImageData;
            this.FrameRate = FrameRate;
            CurrentImage = TheImage[0];
            PictureBox.Image = CurrentImage;//.ToBitmap();

            PictureBox.Size = new Size(CurrentImage.Width, CurrentImage.Height);

            int width = Math.Max(CurrentImage.Width + 40, 700);
            int height = Math.Max(CurrentImage.Height + 63 + 93, 350);

            this.Size = new Size(width, height);
            this.frames = TheImage.Length;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(X - 7, Y - 20);

            startTrack.Maximum = frames;
            endTrack.Maximum = frames;
            endTrack.Value = frames;
            BackgroundTrack.Maximum = frames;

            if (Preferences.NeverUpload)
            {
                this.AcceptButton = SaveButton;
                UploadButton.Enabled = false;
            }
            else
            {
                this.AcceptButton = UploadButton;
            }

            frameTimer.Interval = 1000 / FrameRate;

            FPSLabel.Text = "Average recorded framerate: " + FrameRate.ToString();
        }


        private void Uploadbutton_Click(object sender, EventArgs e)
        {
            this.Hide();
            CalculateFileSizeAndSaveOutputImage();
            result = NewImageEditor.EditorResult.Upload;
            this.Close();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            CalculateFileSizeAndSaveOutputImage();
            result = NewImageEditor.EditorResult.Save;
            this.Close();
        }


        int CurrentFrame = 0;
        int ActualFrameNumber = 0;
        private void frameTimer_Tick(object sender, EventArgs e)
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

            //Console.WriteLine(ActualFrameNumber);
            CurrentImage = TheImage[ActualFrameNumber];
            PictureBox.Image = CurrentImage;
            BackgroundTrack.Value = Math.Min(Math.Max(ActualFrameNumber, BackgroundTrack.Minimum), BackgroundTrack.Maximum);

        }
        
        private void startTrack_ValueChanged(object sender, EventArgs e)
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
        private void endTrack_ValueChanged(object sender, EventArgs e)
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
            CurrentFrame = endTrack.Value-1;
        }
        private void GifEditor_Resize(object sender, EventArgs e)
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


        bool Scrolling = false;
        private void startTrack_MouseDown(object sender, MouseEventArgs e)
        {
            Scrolling = true;
            startTrack.Value = Convert.ToInt32(((double)e.X / (double)startTrack.Width) * (startTrack.Maximum - startTrack.Minimum));
        }
        private void endTrack_MouseDown(object sender, MouseEventArgs e)
        {
            Scrolling = true;
            endTrack.Value = Convert.ToInt32(((double)e.X / (double)endTrack.Width) * (endTrack.Maximum - endTrack.Minimum));
        }
        private void endTrack_MouseUp(object sender, MouseEventArgs e)
        {
            Scrolling = false;
        }
        private void startTrack_MouseUp(object sender, MouseEventArgs e)
        {
            Scrolling = false;
        }

        public static T[] SubArray<T>(T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        int SavedImageStart = 0;
        int SavedImageEnd = 0;
        int SavedImageScale = 0;

        private void CalculateFileSizeAndSaveOutputImage()
        {
            //If this is true, that means the user changed nothing and we don't need to re-compute the image.
            if((SavedImageStart == (int)startTrack.Value) &&
              (SavedImageEnd == (int)endTrack.Value) &&
              (SavedImageScale == (int)ScaleThing.Value))
            {
                return;
            }

                
            scalePct = (float)ScaleThing.Value / 100.0f;

            SavedImageStart = (int)startTrack.Value;
            SavedImageEnd = (int)endTrack.Value;
            SavedImageScale = (int)ScaleThing.Value;

            EditedImage = SubArray(TheImage, SavedImageStart, SavedImageEnd - SavedImageStart);
        }
        private void calcSizeButton_Click(object sender, EventArgs e)
        {
            CalculateFileSizeAndSaveOutputImage();
        }
        private void GifEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "Escape")
            {
                this.Close();
            }
        }
        
        float scalePct = 1.0f;
        private void ScaleThing_ValueChanged(object sender, EventArgs e)
        {
            scalePct = (float)ScaleThing.Value / 100.0f;
            PictureBox.Size = new Size((int)(CurrentImage.Width * scalePct), (int)(CurrentImage.Height * scalePct));
        }

        private void ScaleThing_KeyDown(object sender, KeyEventArgs e)
        {
            //scalePct = (float)ScaleThing.Value / 100.0f;
            //PictureBox.Size = new Size((int)(CurrentImage.Width * scalePct), (int)(CurrentImage.Height * scalePct));
        }

        private void GifEditor_Load(object sender, EventArgs e)
        {

        }

        private void PictureBox_Click(object sender, EventArgs e)
        {

        }

        private void ScaleThing_KeyUp(object sender, KeyEventArgs e)
        {
            //scalePct = (float)ScaleThing.Value / 100.0f;
            //PictureBox.Size = new Size((int)(CurrentImage.Width * scalePct), (int)(CurrentImage.Height * scalePct));
        }

        private void ScaleThing_KeyPress(object sender, KeyPressEventArgs e)
        {
            scalePct = (float)ScaleThing.Value / 100.0f;
            PictureBox.Size = new Size((int)(CurrentImage.Width * scalePct), (int)(CurrentImage.Height * scalePct));
        }
    }
    class TextData
    {
        public int size = 0;
        public string text = "";
        public int X = 0;
        public int Y = 0;
        public int TimeStart = 0;
        public int TimeEnd = 0;
        public override string ToString()
        {
            return text;
        }
    }
}
