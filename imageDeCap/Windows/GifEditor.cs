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
        public GifEditor(MagickImageCollection ImageData, int X, int Y)
        {
            InitializeComponent();
            theImage = ImageData;

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
            this.AcceptButton = calcSizeButton;
            frameTimer.Interval = (int)(1000.0f / Preferences.GIFRecordingFramerate);
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
                ActualFrameNumber = (CurrentFrame % theImage.Count);
            }

            Console.WriteLine(ActualFrameNumber);
            CurrentImage = theImage[ActualFrameNumber];
            PictureBox.Image = CurrentImage.ToBitmap();
            BackgroundTrack.Value = Math.Min(Math.Max(ActualFrameNumber, BackgroundTrack.Minimum), BackgroundTrack.Maximum);

            Graphics g = Graphics.FromImage(PictureBox.Image);
            foreach(TextData t in Texts)
            {
                if(ActualFrameNumber < t.TimeEnd && ActualFrameNumber >= t.TimeStart)
                {
                    g.DrawString(t.text, new Font(Preferences.ImageEditorFont, t.size, (FontStyle)Preferences.FontStyleType), new SolidBrush(Color.FromArgb(255, 255, 0, 0)), new Point(t.X, t.Y));
                }
            }
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

            float scalePct = (float)ScaleThing.Value / 100.0f;

            SavedImageStart = (int)startTrack.Value;
            SavedImageEnd = (int)endTrack.Value;
            SavedImageScale = (int)ScaleThing.Value;

            List<IMagickImage> subImageList = theImage.ToList().GetRange(startTrack.Value, endTrack.Value - startTrack.Value);
            EditedImage = new MagickImageCollection();
            int j = 0;
            foreach (MagickImage i in subImageList)
            {
                i.AnimationDelay = (int)(100.0f / Preferences.GIFRecordingFramerate);
                foreach (TextData t in Texts)
                {
                    if (j < t.TimeEnd && j >= t.TimeStart)
                    {
                        i.Settings.FontPointsize = t.size;
                        i.Settings.FillColor = new MagickColor(Color.Red);
                        i.Settings.Font = "Arial-Black";
                        i.Settings.FontFamily = "Arial";
                        i.Annotate(t.text, new MagickGeometry(t.X, t.Y, 20, 20), Gravity.Northwest);
                    }
                }
                i.Resize((int)(i.Width * scalePct),
                         (int)(i.Height * scalePct));
                
                EditedImage.Add(i);
                j += 1;
            }
            
            MemoryStream ms = new MemoryStream();
            EditedImage.Write(ms);

            PictureBox.Size = new Size((int)(CurrentImage.Width * scalePct), (int)(CurrentImage.Height * scalePct));

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

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            panel1.Visible = checkBox1.Checked;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TextData data = new TextData();
            data.text = "Sample Text";
            data.size = 16;
            data.TimeEnd = frames;
            sizeNr.Value = data.size;
            timeEnd.Value = (int)frames;
            Texts.Add(data);
            listBox1.DataSource = null;
            listBox1.DataSource = Texts;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                return;
            }
            Texts.RemoveAt(listBox1.SelectedIndex);
            listBox1.DataSource = null;
            listBox1.DataSource = Texts;
        }

        List<TextData> Texts = new List<TextData>();
        TextData currentText;
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine(listBox1.SelectedIndex);
            bool SelectionIsValid = listBox1.SelectedIndex != -1;

            button1.Enabled = SelectionIsValid;
            textValue.Enabled = SelectionIsValid;
            sizeNr.Enabled = SelectionIsValid;
            locationX.Enabled = SelectionIsValid;
            locationY.Enabled = SelectionIsValid;
            timeStart.Enabled = SelectionIsValid;
            timeEnd.Enabled = SelectionIsValid;

            if (SelectionIsValid == false)
            {
                return;
            }
            currentText = Texts[listBox1.SelectedIndex];
            if (currentText == null)
            {
                return;
            }
            textValue.Text = currentText.text;
            locationX.Value = currentText.X;
            locationY.Value = currentText.Y;
            timeStart.Value = currentText.TimeStart;
            timeEnd.Value = currentText.TimeEnd;
            sizeNr.Value = currentText.size;
            
        }

        private void textValue_TextChanged(object sender, EventArgs e)
        {
            if (currentText != null)
            {
                currentText.text = textValue.Text;
            }

            int len = textValue.SelectionLength;
            int strt = textValue.SelectionStart;

            listBox1.DataSource = null;
            listBox1.DataSource = Texts;

            textValue.Focus();
            textValue.Select(strt, len);
        }


        private void timeStart_ValueChanged(object sender, EventArgs e)
        {
            if (currentText != null)
            {
                currentText.TimeStart = (int)timeStart.Value;
            }
        }
        private void timeEnd_ValueChanged(object sender, EventArgs e)
        {
            if (currentText != null)
            {
                currentText.TimeEnd = (int)timeEnd.Value;
            }
        }

        private void locationX_ValueChanged(object sender, EventArgs e)
        {
            if (currentText != null)
            {
                currentText.X = (int)locationX.Value;
            }
        }

        private void locationY_ValueChanged(object sender, EventArgs e)
        {
            if (currentText != null)
            {
                currentText.Y = (int)locationY.Value;
            }
        }

        private void sizeNr_ValueChanged(object sender, EventArgs e)
        {
            if (currentText != null)
            {
                currentText.size = (int)sizeNr.Value;
            }
        }

        int LastMouseX = 0;
        int LastMouseY = 0;
        bool MouseDown = false;
        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDown = true;
            LastMouseX = e.X;
            LastMouseY = e.Y;
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            float scaleValue = (100.0f / (float)ScaleThing.Value);
            int DeltaX = (int)(((float)e.X - (float)LastMouseX) * scaleValue);
            int DeltaY = (int)(((float)e.Y - (float)LastMouseY) * scaleValue);
            
            if(MouseDown)
            {
                if (listBox1.SelectedIndex != -1)
                {
                    if (Texts[listBox1.SelectedIndex] != null)
                    {
                        Texts[listBox1.SelectedIndex].X += DeltaX;
                        Texts[listBox1.SelectedIndex].Y += DeltaY;
                        locationX.Value = Math.Max(Texts[listBox1.SelectedIndex].X, 0);
                        locationY.Value = Math.Max(Texts[listBox1.SelectedIndex].Y, 0);
                    }
                }
                
            }
            LastMouseX = e.X;
            LastMouseY = e.Y;
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            MouseDown = false;
        }

        private void GifEditor_Load(object sender, EventArgs e)
        {

        }
    }
    class TextData
    {
        public int size;
        public string text;
        public int X;
        public int Y;
        public int TimeStart;
        public int TimeEnd;
        public override string ToString()
        {
            return text;
        }
    }
}
