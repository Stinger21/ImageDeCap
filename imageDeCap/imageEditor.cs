using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace imageDeCap
{
    public partial class imageEditor : Form
    {
        Stack<Bitmap> undoHistory = new Stack<Bitmap>();

        bool closedIntentionally = false;
        string imagePath;
        string newImagePath = System.IO.Path.GetTempPath() + "screenshot_edited.png";
        string compressedImagePath = System.IO.Path.GetTempPath() + "screenshot_edited.jpg";
        Image theImage;
        string whereToSave;
        public imageEditor(string imagePath, string whereToSave)//on start
        {
            this.whereToSave = whereToSave;
            this.imagePath = imagePath;
            InitializeComponent();
            this.AcceptButton = button1;
            theImage = Image.FromFile(imagePath);

            fileSizeLabel.Text = (new FileInfo(imagePath).Length / 1000) + "KB";

            int width;
            int height;
            if (theImage.Width < 600)
            {
                width = 600;
            }
            else
            {
                width = theImage.Width;
            }
            if (theImage.Height < 200)
            {
                height = 200;
            }
            else
            {
                height = theImage.Height;
            }
            this.Size = new System.Drawing.Size(width + 50, height + 110);

            imageContainer.Image = theImage;


            trackBar1.Value = (int)brushSize * 100;
            trackBar2.Value = (int)textSize * 100;

            label2.Text = "Brush: " + brushSize.ToString("0.0");
            label3.Text = "Text: " + textSize.ToString("0.0");


            if (Properties.Settings.Default.NeverUpload)
            {
                button1.Text = "Done";
            }
            else
            {
                button1.Text = "Upload";
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            theImage.Save(newImagePath);
            // save to screenshots folder after clicking Done if user added some edits
            if (Properties.Settings.Default.saveImageAtAll && Directory.Exists(Properties.Settings.Default.SaveImagesHere) && undoHistory.Count > 0)
            {
                //string name = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");

                //theImage.Save(Properties.Settings.Default.SaveImagesHere + @"\" + name + ".png");
                theImage.Save(this.whereToSave);
            }
            theImage.Dispose();
            closedIntentionally = true;
            Close();
        }
        
        int Width = 8;
        int Height = 8;
        Point lastPos;
        bool isPressed = false;
        float brushSize = 4.0f;
        float textSize = 12.0f;
        Color c = Color.FromArgb(192, 57, 43);
        bool brush = true;
        string colorName = "DarkRed";
        private void imageContainer_MouseClick(object sender, MouseEventArgs e)
        {
            if (!brush)
            {
                brush = true;
            }
        }

        private void imageContainer_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = imageContainer.PointToClient(Cursor.Position);
                label1.Text = mousePos.X.ToString() + ", " + mousePos.Y.ToString() + " - " + colorName;
                if (pickColor)
                {
                    c = ((Bitmap)theImage).GetPixel(mousePos.X, mousePos.Y);
                    currentColor.BackColor = c;
                    imageContainer.Cursor = Cursors.Default;
                    pickColor = false;
                }
                if (!brush)
                {
                    undoHistory.Push(new Bitmap(theImage));
                    using (Graphics g = Graphics.FromImage(theImage))
                    {
                        Pen MyPen = new Pen(c);
                        MyPen.Width = textSize;
                        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                        Size tsize = TextRenderer.MeasureText(textBox1.Text, new Font("Arial Black", textSize));
                        g.DrawString(textBox1.Text, new Font("Arial Black", textSize), new SolidBrush(c), new Point(mousePos.X - tsize.Width / 2, mousePos.Y - tsize.Height / 2));
                    }
                    imageContainer.Refresh();

                    //trackBar1.Value = (int)brushSize * 100;
                    //label2.Text = "Size: " + brushSize.ToString("0.0");
                }
            }
        }

        private void imageContainer_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = imageContainer.PointToClient(Cursor.Position);
            label1.Text = mousePos.X.ToString() + ", " + mousePos.Y.ToString() + " - " + colorName;

            if (MouseButtons == MouseButtons.Left)
            {
                
                if (!isPressed)
                {
                    lastPos = mousePos;
                    undoHistory.Push(new Bitmap(theImage));
                }
                if (brush)
                {

                    using (Graphics g = Graphics.FromImage(theImage))
                    {
                        Pen MyPen = new Pen(c);
                        MyPen.Width = brushSize;
                        //MyPen.DashCap = System.Drawing.Drawing2D.DashCap.Round;
                        MyPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                        MyPen.StartCap = System.Drawing.Drawing2D.LineCap.Round;

                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                        g.DrawLine(MyPen, lastPos, mousePos);

                        //g.DrawArc(new Pen(Color.Red, 8), new Rectangle(mousePos.X - (Width / 2), mousePos.Y - (Width / 2), Width, Height),0,360);
                    }
                    ApplyCompression();
                }

                imageContainer.Refresh();
                lastPos = mousePos;
                isPressed = true;
            }
            else
            {
                if(pickColor)
                {
                    currentColor.BackColor = ((Bitmap)theImage).GetPixel(mousePos.X, mousePos.Y);
                }
                if (tempImage)
                {
                    undoImageEdit();
                    tempImage = false;
                }
                if(moveBrushIsOnScreen)
                {
                    undoImageEdit();
                    moveBrushIsOnScreen = false;
                }

                isPressed = false;
                //pictureBox2.Location = new Point(mousePos.X+2, mousePos.Y+2);
                if (!brush)
                {//if we are not holding down any button and are in text mode.

                    //using (Graphics g = Graphics.FromImage(pictureBox2.Image))
                    //{
                    //    Pen MyPen = new Pen(c);
                    //    MyPen.Width = textSize;
                    //    g.DrawString(textBox1.Text, new Font("Arial Black", textSize), new SolidBrush(c), mousePos);
                    //}
                    //pictureBox2.Refresh();
                    
                    // Placeholder image for users niceness
                    //undoImageEdit();
                    undoHistory.Push(new Bitmap(theImage));
                    using (Graphics g = Graphics.FromImage(theImage))
                    {
                        Pen MyPen = new Pen(c);
                        MyPen.Width = textSize;
                        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                        Size tsize = TextRenderer.MeasureText(textBox1.Text, new Font("Arial Black", textSize));
                        g.DrawString(textBox1.Text, new Font("Arial Black", textSize), new SolidBrush(c), new Point(mousePos.X - tsize.Width/2, mousePos.Y - tsize.Height / 2));
                    }
                    ApplyCompression();
                    imageContainer.Refresh();
                    moveBrushIsOnScreen = true;
                }
            }
        }
        bool moveBrushIsOnScreen = false;
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            brushSize = ((float)trackBar1.Value) / 100.0f;
            label2.Text = "Brush: " + brushSize.ToString("0.0");
            if(tempImage)
            {
                undoImageEdit();
            }
            undoHistory.Push(new Bitmap(theImage));
            using (Graphics g = Graphics.FromImage(theImage))
            {
                Pen MyPen = new Pen(Color.FromArgb(128, 255, 0, 0));
                
                g.FillEllipse(MyPen.Brush, imageContainer.Width / 2 - brushSize / 2, imageContainer.Height / 2 - brushSize / 2, brushSize, brushSize);
            }
            ApplyCompression();
            imageContainer.Refresh();
            tempImage = true;
        }
        bool tempImage = false;
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            textSize = ((float)trackBar2.Value) / 100.0f;
            label3.Text = "Text: " + textSize.ToString("0.0");
            if(tempImage)
            {
                undoImageEdit();
            }
            undoHistory.Push(new Bitmap(theImage));
            using (Graphics g = Graphics.FromImage(theImage))
            {
                Pen MyPen = new Pen(c);
                MyPen.Width = textSize;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                g.DrawString(textBox1.Text, new Font("Arial Black", textSize), new SolidBrush(Color.FromArgb(128, 255, 0, 0)), 100,100);
            }
            ApplyCompression();
            imageContainer.Refresh();
            tempImage = true;
        }
        private void trackBar2_DragLeave(object sender, EventArgs e)
        {
            //undoImageEdit();
        }

        private void trackBar2_DragEnter(object sender, DragEventArgs e)
        {
        }

        private void radioButton1_CheckedChanged_1(object sender, EventArgs e)
        {
            
            /*
            // Executed when any radio button is changed.
            // ... It is wired up to every single radio button.
            // Search for first radio button in GroupBox.
            string result1 = null;
            foreach (Control control in this.groupBox1.Controls)
            {
                if (control is RadioButton)
                {
                    RadioButton radio = control as RadioButton;
                    if (radio.Checked)
                    {
                        result1 = radio.Text;
                    }
                }
            }
            // Search second GroupBox.
            string result2 = null;
            foreach (Control control in this.groupBox2.Controls)
            {
                if (control is RadioButton)
                {
                    RadioButton radio = control as RadioButton;
                    if (radio.Checked)
                    {
                        result2 = radio.Text;
                    }
                }
            }
            brush = result1 == null;*/
        }

        void setPalette(Button button)
        {
            c = button.BackColor;
            currentColor.BackColor = c;
            colorName = button.Text;
            Point mousePos = imageContainer.PointToClient(Cursor.Position);
            label1.Text = mousePos.X.ToString() + ", " + mousePos.Y.ToString() + " - " + colorName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            setPalette(c_red_1);
        }

        private void c_red_2_Click(object sender, EventArgs e)
        {
            setPalette(c_red_2);
        }

        private void c_red_3_Click(object sender, EventArgs e)
        {
            setPalette(c_red_3);
        }

        private void c_yellow_1_Click(object sender, EventArgs e)
        {
            setPalette(c_yellow_1);
        }

        private void c_yellow_2_Click(object sender, EventArgs e)
        {
            setPalette(c_yellow_2);
        }

        private void c_yellow_3_Click(object sender, EventArgs e)
        {
            setPalette(c_yellow_3);
        }

        private void c_green_1_Click(object sender, EventArgs e)
        {
            setPalette(c_green_1);
        }

        private void c_green_2_Click(object sender, EventArgs e)
        {
            setPalette(c_green_2);
        }

        private void c_green_3_Click(object sender, EventArgs e)
        {
            setPalette(c_green_3);
        }

        private void c_blue_1_Click(object sender, EventArgs e)
        {
            setPalette(c_blue_1);
        }

        private void c_blue_2_Click(object sender, EventArgs e)
        {
            setPalette(c_blue_2);
        }

        private void c_blue_3_Click(object sender, EventArgs e)
        {
            setPalette(c_blue_3);
        }

        private void c_purple_1_Click(object sender, EventArgs e)
        {
            setPalette(c_purple_1);
        }

        private void c_purple_2_Click(object sender, EventArgs e)
        {
            setPalette(c_purple_2);
        }

        private void c_purple_3_Click(object sender, EventArgs e)
        {
            setPalette(c_purple_3);
        }

        private void c_black_Click(object sender, EventArgs e)
        {
            setPalette(c_black);
        }

        private void c_grey_Click(object sender, EventArgs e)
        {
            setPalette(c_grey);
        }

        private void c_white_Click(object sender, EventArgs e)
        {
            setPalette(c_white);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void imageEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            theImage.Dispose();
            if (!closedIntentionally)
            {
                File.Delete(newImagePath);
            }
        }
        
        private void button2_Click_1(object sender, EventArgs e)
        {
            undoImageEdit();
        }
        public void undoImageEdit()
        {
            if (undoHistory.Count > 0)
            {
                using (Graphics g = Graphics.FromImage(theImage))
                {
                    g.DrawImage((Image)undoHistory.Pop(), Point.Empty);
                }
                imageContainer.Refresh();
            }
            ApplyCompression();
        }
        
        private void imageEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "Escape")
            {
                this.Close();
            }
            if(e.Control)
            {
                if(e.KeyCode.ToString() == "Z")
                {
                    undoImageEdit();
                }
            }
        }

        private void imageEditor_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void imageEditor_Load(object sender, EventArgs e)
        {

        }

        private void imageEditor_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void addTextButton_Click(object sender, EventArgs e)
        {
            brush = false;
            if (tempImage)
            {
                undoImageEdit();
                tempImage = false;
            }
            undoHistory.Push(new Bitmap(theImage));
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
        }

        private void panel1_Leave(object sender, EventArgs e)
        {

        }

        private void currentColor_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            c = colorDialog1.Color;
            currentColor.BackColor = c;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }



        private ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        private void saveJpg(Bitmap bmp, string path, long compressionLevel)
        {
            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
            
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);

            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, compressionLevel);
            myEncoderParameters.Param[0] = myEncoderParameter;
            try
            {
                bmp.Save(path, jpgEncoder, myEncoderParameters);
            }
            catch
            {
                Console.WriteLine("Failed to save jpg");
            }
            
            

        }

        
        Image jpgImage;
        void ApplyCompression()
        {
            try
            {
                jpgImage.Dispose();
            }
            catch { }
            string path = compressedImagePath;
            int compressionNumber = (int)Math.Abs(100 - compressionSlider.Value);
            saveJpg(theImage as Bitmap, path, compressionNumber);
            jpgImage = Image.FromFile(path);
            jpegBox.Image = jpgImage;
            if(compressionNumber < 10)
            {
                fileSizeLabel.Text = (new FileInfo(path).Length / 1000) + "KB [Do I look like I know what a jpeg is?]";
            }
            else
            {
                fileSizeLabel.Text = (new FileInfo(path).Length / 1000) + "KB";
            }
            label4.Text = "Compression: " + compressionNumber.ToString();
        }
        private void compressionSlider_Scroll(object sender, EventArgs e)
        {
            ApplyCompression();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                jpegBox.Show();
                compressionSlider.Enabled = true;
                ApplyCompression();
            }
            else
            {
                jpegBox.Hide();
                compressionSlider.Enabled = false;
                fileSizeLabel.Text = (new FileInfo(imagePath).Length / 1000) + "KB";
            }
        }

        bool pickColor = false;
        private void button3_Click_1(object sender, EventArgs e)
        {

            pickColor = true;
            imageContainer.Cursor = Cursors.Cross;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
