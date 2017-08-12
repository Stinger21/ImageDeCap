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
        MagickImageCollection theImage = new MagickImageCollection();
        IMagickImage CurrentImage;
        int frames = 0;
        string whereToSave;
        string newImagePath;
        public GifEditor(string filepath, string whereToSave)
        {
            this.whereToSave = whereToSave;
            InitializeComponent();
            theImage.Read(filepath);
            newImagePath = filepath;

            CurrentImage = theImage[0];
            PictureBox.Image = CurrentImage.ToBitmap();

            PictureBox.Size = new Size(CurrentImage.Width, CurrentImage.Height);

            int width = Math.Max(CurrentImage.Width + 40, 700);
            int height = Math.Max(CurrentImage.Height + 63 + 93, 350);

            this.Size = new Size(width, height);
            this.frames = theImage.Count;

            //PictureBox.Location = new Point((width / 2) - (CurrentImage.Width / 2), 12);
            startTrack.Maximum = frames;
            endTrack.Maximum = frames;
            endTrack.Value = frames;
            BackgroundTrack.Maximum = frames;

            if (Properties.Settings.Default.NeverUpload)
            {
                uploadButton.Text = "Done";
            }
            else
            {
                uploadButton.Text = "Upload";
            }
            this.AcceptButton = uploadButton;

            CalculateFileSizeAndSaveOutputImage();
        }

        private void GifEditor_Load(object sender, EventArgs e)
        {
            
        }

        int CurrentFrame = 0;
        private void frameTimer_Tick(object sender, EventArgs e)
        {
            if(Scrolling == false)
            {
                CurrentFrame++;
            }
            CurrentImage = theImage[(CurrentFrame % (endTrack.Value - startTrack.Value)) + startTrack.Value];
            PictureBox.Image = CurrentImage.ToBitmap();
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
            CurrentFrame = startTrack.Value;
        }
        private void endTrack_MouseDown(object sender, MouseEventArgs e)
        {
            Scrolling = true;
            endTrack.Value = Convert.ToInt32(((double)e.X / (double)endTrack.Width) * (endTrack.Maximum - endTrack.Minimum));
            CurrentFrame = endTrack.Value;
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
            MagickImageCollection newImage = new MagickImageCollection(subImageList);
            newImage.Write(newImagePath);

            sizeText.Text = "File-Size: " + (Math.Round(new FileInfo(newImagePath).Length / 10000.0f)/100.0f).ToString() + " MB";
        }
        private void calcSizeButton_Click(object sender, EventArgs e)
        {
            CalculateFileSizeAndSaveOutputImage();
        }

        private void uploadButton_Click(object sender, EventArgs e)
        {
            theImage.Write(newImagePath);
            if (Properties.Settings.Default.saveImageAtAll && Directory.Exists(Properties.Settings.Default.SaveImagesHere))
            {
                theImage.Write(this.whereToSave);
            }
            this.Close();
        }

        private void GifEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "Escape")
            {
                // Delete file, so it does not upload :)
                File.Delete(newImagePath);
                this.Close();
            }
        }
    }
}
