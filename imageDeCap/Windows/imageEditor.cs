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
        public (bool Aborted, byte[] Data) FinalFunction()
        {
            byte[] outputData;
            if(checkBox1.Checked)
            {
                outputData = completeCover.GetBytes(jpgImage, ImageFormat.Jpeg);
            }
            else
            {
                outputData = completeCover.GetBytes(mainImage, ImageFormat.Png);
            }
            return (Aborted, outputData);
        }

        Stack<Bitmap> undoHistory = new Stack<Bitmap>();

        bool Aborted = true;

        Image mainImage;
        Image jpgImage;
        int FileSize;
        public imageEditor(byte[] ImageData, int X, int Y)//on start
        {
            InitializeComponent();
            this.AcceptButton = button1;
            mainImage = Image.FromStream(new MemoryStream(ImageData));

            FileSize = ImageData.Length;
            fileSizeLabel.Text = (ImageData.Length / 1000) + "KB";

            int width;
            int height;
            if (mainImage.Width < 600)
            {
                width = 600;
            }
            else
            {
                width = mainImage.Width;
            }
            if (mainImage.Height < 200)
            {
                height = 200;
            }
            else
            {
                height = mainImage.Height;
            }
            this.Size = new System.Drawing.Size(width + 40, height + 140);
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(X - 7, Y - 20);

            imageContainer.Image = mainImage;


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
            if (tempImage)
            {
                undoImageEdit();
                tempImage = false;
            }

            //theImage.Save(newImagePath);
            // save to screenshots folder after clicking Done if user added some edits
            //if (Properties.Settings.Default.saveImageAtAll && Directory.Exists(Properties.Settings.Default.SaveImagesHere) && undoHistory.Count > 0)
            //{
            //    theImage.Save(this.whereToSave);
            //}
            Aborted = false;
            Close();
        }
        
        int Width = 8;
        int Height = 8;
        Point leftMouseLastPos;
        Point rightMouseLastPos;
        Point rightMouseDownPos;
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

                if(System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftAlt))
                {
                    c = ((Bitmap)mainImage).GetPixel(mousePos.X, mousePos.Y);
                    currentColor.BackColor = c;
                }

                if (pickColor)
                {
                    c = ((Bitmap)mainImage).GetPixel(mousePos.X, mousePos.Y);
                    currentColor.BackColor = c;
                    imageContainer.Cursor = Cursors.Default;
                    pickColor = false;
                }
                if (!brush)
                {
                    undoHistory.Push(new Bitmap(mainImage));
                    using (Graphics g = Graphics.FromImage(mainImage))
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
                else
                {
                    if(System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftShift) ||
                        System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.RightShift))
                    {
                        if (leftMouseLastPos == Point.Empty)
                        {
                            leftMouseLastPos = mousePos;
                        }
                        using (Graphics g = Graphics.FromImage(mainImage))
                        {
                            Pen MyPen = new Pen(c);
                            MyPen.Width = brushSize;
                            MyPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                            MyPen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                            
                            g.DrawLine(MyPen, leftMouseLastPos, mousePos);
                        }
                        ApplyCompression();
                        imageContainer.Refresh();

                    }
                    leftMouseLastPos = mousePos;
                }
            }

            if(e.Button == MouseButtons.Right)
            {
                Point mousePos = imageContainer.PointToClient(Cursor.Position);
                rightMouseDownPos = mousePos;
                rightMouseLastPos = mousePos;
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
                    leftMouseLastPos = mousePos;
                    undoHistory.Push(new Bitmap(mainImage));
                }
                if (brush)
                {
                    using (Graphics g = Graphics.FromImage(mainImage))
                    {
                        Pen MyPen = new Pen(c);
                        MyPen.Width = brushSize;
                        MyPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                        MyPen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        
                        g.DrawLine(MyPen, leftMouseLastPos, mousePos);
                    }
                    ApplyCompression();
                }

                imageContainer.Refresh();
                leftMouseLastPos = mousePos;
                isPressed = true;
            }
            else if(MouseButtons == MouseButtons.Right)
            {
                int targetValue = (mousePos.X - rightMouseLastPos.X) * 200;
                Console.WriteLine(targetValue);
                int newVal = trackBar1.Value;
                newVal += targetValue;
                newVal = (int)Math.Min(Math.Max((double)newVal, (double)trackBar1.Minimum), (double)trackBar1.Maximum);
                trackBar1.Value = newVal;
                trackBar1_Scroll(null, null);
                rightMouseLastPos = mousePos;
            }
            else
            {
                if(pickColor)
                {
                    currentColor.BackColor = ((Bitmap)mainImage).GetPixel(mousePos.X, mousePos.Y);
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

                if (!brush)
                {//if we are not holding down any button and are in text mode.
                    
                    undoHistory.Push(new Bitmap(mainImage));
                    using (Graphics g = Graphics.FromImage(mainImage))
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
            undoHistory.Push(new Bitmap(mainImage));
            using (Graphics g = Graphics.FromImage(mainImage))
            {
                Pen MyPen = new Pen(Color.FromArgb(128, 255, 0, 0));
                float halfBrush = brushSize / 2.0f;
                g.FillEllipse(MyPen.Brush, rightMouseDownPos.X - halfBrush, rightMouseDownPos.Y - halfBrush, brushSize, brushSize);
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
            undoHistory.Push(new Bitmap(mainImage));
            using (Graphics g = Graphics.FromImage(mainImage))
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
        }

        private void trackBar2_DragEnter(object sender, DragEventArgs e)
        {
        }

        private void radioButton1_CheckedChanged_1(object sender, EventArgs e)
        {
            
        }

        void setPalette(Button button)
        {
            c = button.BackColor;
            currentColor.BackColor = c;
            colorName = button.Text;
            Point mousePos = imageContainer.PointToClient(Cursor.Position);
            label1.Text = mousePos.X.ToString() + ", " + mousePos.Y.ToString() + " - " + colorName;
        }

        private void button2_Click(object sender, EventArgs e){setPalette(c_red_1);}
        private void c_red_2_Click(object sender, EventArgs e){setPalette(c_red_2);}
        private void c_red_3_Click(object sender, EventArgs e){setPalette(c_red_3);}
        private void c_yellow_1_Click(object sender, EventArgs e){setPalette(c_yellow_1);}
        private void c_yellow_2_Click(object sender, EventArgs e){setPalette(c_yellow_2);}
        private void c_yellow_3_Click(object sender, EventArgs e){setPalette(c_yellow_3);}
        private void c_green_1_Click(object sender, EventArgs e){setPalette(c_green_1);}
        private void c_green_2_Click(object sender, EventArgs e){setPalette(c_green_2);}
        private void c_green_3_Click(object sender, EventArgs e){setPalette(c_green_3);}
        private void c_blue_1_Click(object sender, EventArgs e){setPalette(c_blue_1);}
        private void c_blue_2_Click(object sender, EventArgs e){setPalette(c_blue_2);}
        private void c_blue_3_Click(object sender, EventArgs e){setPalette(c_blue_3);}
        private void c_purple_1_Click(object sender, EventArgs e){setPalette(c_purple_1);}
        private void c_purple_2_Click(object sender, EventArgs e){setPalette(c_purple_2);}
        private void c_purple_3_Click(object sender, EventArgs e){setPalette(c_purple_3);}
        private void c_black_Click(object sender, EventArgs e){setPalette(c_black);}
        private void c_grey_Click(object sender, EventArgs e){setPalette(c_grey);}
        private void c_white_Click(object sender, EventArgs e){setPalette(c_white);}

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void imageEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (Aborted)
            //{
            //    File.Delete(newImagePath);
            //}
        }
        
        private void button2_Click_1(object sender, EventArgs e)
        {
            undoImageEdit();
        }
        public void undoImageEdit()
        {
            if (undoHistory.Count > 0)
            {
                using (Graphics g = Graphics.FromImage(mainImage))
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
                if (e.KeyCode.ToString() == "T")
                {
                    if (brush)
                    {
                        addTextButton_Click(null, null);
                    }
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
            undoHistory.Push(new Bitmap(mainImage));
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

        
        void ApplyCompression()
        {
            try
            {
                jpgImage.Dispose();
            }catch { }
            
            int compressionNumber = (int)Math.Abs(100 - compressionSlider.Value);
            
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            myEncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, compressionNumber);

            var ms = new MemoryStream();
            mainImage.Save(ms, GetEncoder(ImageFormat.Jpeg), myEncoderParameters);
            jpgImage = Image.FromStream(ms);
            
            jpegBox.Image = jpgImage;
            if(compressionNumber < 10)
            {
                fileSizeLabel.Text = (ms.Length / 1000) + "KB [Do I look like I know what a jpeg is?]";
            }
            else
            {
                fileSizeLabel.Text = (ms.Length / 1000) + "KB";
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
                fileSizeLabel.Text = (FileSize / 1000) + "KB";
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

        private void HighlightCheckbox_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
