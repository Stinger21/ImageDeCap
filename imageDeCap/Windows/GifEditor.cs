using ImageMagick;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace imageDeCap
{

    public partial class GifEditor : Form
    {
        public (bool Aborted, byte[] Data) FinalFunction()
        {
            return (Aborted, EditedImage.ToByteArray(MagickFormat.Gif));
        }


        bool Aborted = true;
        MagickImageCollection theImage = new MagickImageCollection();
        public MagickImageCollection EditedImage = new MagickImageCollection();
        IMagickImage CurrentImage;
        int frames = 0;
        public GifEditor(byte[] ImageData, int X, int Y)
        {
            InitializeComponent();
            theImage.Read(ImageData);

            CurrentImage = theImage[0];
            PictureBox.Image = CurrentImage.ToBitmap();

            PictureBox.Size = new Size(CurrentImage.Width, CurrentImage.Height);

            int width = Math.Max(CurrentImage.Width + 40, 700);
            int height = Math.Max(CurrentImage.Height + 63 + 93, 350);

            this.Size = new Size(width, height);
            this.frames = theImage.Count;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(X - 7, Y - 20);

            startTrack.Maximum = frames;
            endTrack.Maximum = frames;
            endTrack.Value = frames;
            BackgroundTrack.Maximum = frames;

            if (Preferences.NeverUpload)
            {
                uploadButton.Text = "Done";
            }
            else
            {
                uploadButton.Text = "Upload";
            }
            this.AcceptButton = uploadButton;

            CalculateFileSizeAndSaveOutputImage();
            frameTimer.Interval = (int)(1000.0f / Preferences.GIFRecordingFramerate);
        }

        private void GifEditor_Load(object sender, EventArgs e)
        {
            
        }

        int CurrentFrame = 0;
        private void frameTimer_Tick(object sender, EventArgs e)
        {
            int ActualFrameNumber = 0;
            if (Scrolling == false)
            {
                CurrentFrame++;
                ActualFrameNumber = (CurrentFrame % (endTrack.Value - startTrack.Value)) + startTrack.Value;
            }
            else
            {
                ActualFrameNumber = (CurrentFrame % theImage.Count);
            }

            Console.WriteLine(ActualFrameNumber);
            CurrentImage = theImage[ActualFrameNumber];
            PictureBox.Image = CurrentImage.ToBitmap();
            BackgroundTrack.Value = ActualFrameNumber;
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




        private void CalculateFileSizeAndSaveOutputImage()
        {
            List<IMagickImage> subImageList = theImage.ToList().GetRange(startTrack.Value, endTrack.Value - startTrack.Value);
            EditedImage = new MagickImageCollection();
            foreach (IMagickImage i in subImageList)
            {
                i.AnimationDelay = (int)(100.0f / Preferences.GIFRecordingFramerate);
                EditedImage.Add(i);
            }

            MemoryStream ms = new MemoryStream();
            EditedImage.Write(ms);

            sizeText.Text = "File-Size: " + (Math.Round(ms.Length / 10000.0f)/100.0f).ToString() + " MB";
        }
        private void calcSizeButton_Click(object sender, EventArgs e)
        {
            CalculateFileSizeAndSaveOutputImage();
        }

        private void uploadButton_Click(object sender, EventArgs e)
        {
            CalculateFileSizeAndSaveOutputImage();
            Aborted = false;
            this.Close();
        }

        private void GifEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "Escape")
            {
                this.Close();
            }
        }

        private void GifEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
