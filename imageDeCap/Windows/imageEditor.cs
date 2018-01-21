using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Drawing.Imaging;

namespace imageDeCap
{
    public partial class imageEditor : Form
    {
        public (bool Aborted, byte[] Data) FinalFunction()
        {
            byte[] outputData;
            
            if(SkipEdits)
            {
                // Get image.
                Bitmap[] arr = undoHistory.ToArray();
                Image LastImage = mainImage;
                if(arr.Length > 0)
                    LastImage = arr[arr.Length - 1];

                // get bytes
                if (checkBox1.Checked)
                {
                    Image cmpImage = CompressToJpg(LastImage, (int)Math.Abs(100 - compressionSlider.Value)).result;
                    DrawWatermark(ref cmpImage);
                    outputData = completeCover.GetBytes(cmpImage, ImageFormat.Jpeg);
                }
                else
                {
                    DrawWatermark(ref LastImage);
                    outputData = completeCover.GetBytes(LastImage, ImageFormat.Png);
                }
            }
            else
            {
                if (checkBox1.Checked)
                {
                    DrawWatermark(ref jpgImage);
                    outputData = completeCover.GetBytes(jpgImage, ImageFormat.Jpeg);
                }
                else
                {
                    DrawWatermark(ref mainImage);
                    outputData = completeCover.GetBytes(mainImage, ImageFormat.Png);
                }
            }
            return (Aborted, outputData);
        }
        static void DrawWatermark(ref Image input)
        {
            if(!Preferences.AddWatermark)
            {
                return;
            }

            if(!File.Exists(Preferences.WatermarkFilePath))
            {
                return;
            }

            Image watermarkImage = Image.FromFile(Preferences.WatermarkFilePath);

            using (Graphics g = Graphics.FromImage(input))
            {
                Point location = new Point(0, 0);
                switch (Preferences.WatermarkLocation)
                {
                    case 0:
                            location = new Point(0, 0);
                        break;
                    case 1:
                            location = new Point(input.Width - watermarkImage.Width, 0);
                        break;
                    case 2:
                            location = new Point(0, input.Height - watermarkImage.Height);
                        break;
                    case 3:
                            location = new Point(input.Width - watermarkImage.Width, input.Height - watermarkImage.Height);
                        break;

                }
                g.DrawImage(watermarkImage, location);
            }
        }

        Stack<Bitmap> undoHistory = new Stack<Bitmap>();

        bool Aborted = true;
        bool SkipEdits = true;

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
            trackBar2.Value = (int)textSize * 200;

            label2.Text = "Brush: " + brushSize.ToString("0.0");
            label3.Text = "Text: " + textSize.ToString("0.0");


            if (Preferences.NeverUpload)
            {
                button1.Text = "Done";
                button4.Visible = false;
            }
            else
            {
                button1.Text = "Upload";
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (tempImage)
            {
                undoImageEdit();
                tempImage = false;
            }

            Aborted = false;
            SkipEdits = false;
            Close();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (tempImage)
            {
                undoImageEdit();
                tempImage = false;
            }

            Aborted = true;
            SkipEdits = false;
            Close();
        }

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
            if(e.Button == MouseButtons.Left)
            {
                if (!brush)
                {
                    brush = true;
                }
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
                    setColor(c);
                }

                if (pickColor)
                {
                    c = ((Bitmap)mainImage).GetPixel(mousePos.X, mousePos.Y);
                    setColor(c);
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
                        Size tsize = TextRenderer.MeasureText(textBox1.Text, new Font(Preferences.ImageEditorFont, textSize, (FontStyle)Preferences.FontStyleType));
                        g.DrawString(textBox1.Text, new Font(Preferences.ImageEditorFont, textSize, (FontStyle)Preferences.FontStyleType), new SolidBrush(c), new Point(mousePos.X - tsize.Width / 2, mousePos.Y - tsize.Height / 2));
                    }
                    imageContainer.Refresh();
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
                TrackBar TargetTrackbar;
                if (brush)
                    TargetTrackbar = trackBar1;
                else
                    TargetTrackbar = trackBar2;

                int targetValue = (mousePos.X - rightMouseLastPos.X) * 200;
                int newVal = TargetTrackbar.Value;
                newVal += targetValue;
                newVal = (int)Math.Min(Math.Max((double)newVal, (double)TargetTrackbar.Minimum), (double)TargetTrackbar.Maximum);

                TargetTrackbar.Value = newVal;
                if (brush)
                    trackBar1_Scroll(null, null);
                else
                    trackBar2_Scroll(null, null);
                
                rightMouseLastPos = mousePos;
            }
            else
            {
                UpdateImage();
            }
        }
        private void UpdateImage()
        {
            Point mousePos = imageContainer.PointToClient(Cursor.Position);
            if (pickColor)
            {
                currentColor.BackColor = ((Bitmap)mainImage).GetPixel(mousePos.X, mousePos.Y);
            }
            if (tempImage)
            {
                undoImageEdit();
                tempImage = false;
            }
            if (moveBrushIsOnScreen)
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
                    Size tsize = TextRenderer.MeasureText(textBox1.Text, new Font(Preferences.ImageEditorFont, textSize, (FontStyle)Preferences.FontStyleType));
                    g.DrawString(textBox1.Text, new Font(Preferences.ImageEditorFont, textSize, (FontStyle)Preferences.FontStyleType), new SolidBrush(c), new Point(mousePos.X - tsize.Width / 2, mousePos.Y - tsize.Height / 2));
                }
                ApplyCompression();
                imageContainer.Refresh();
                moveBrushIsOnScreen = true;
            }
        }
        
        bool moveBrushIsOnScreen = false;

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
            textSize = ((float)trackBar2.Value) / 200.0f;
            label3.Text = "Text: " + textSize.ToString("0.0");
            if(tempImage)
            {
                undoImageEdit();
            }
            undoHistory.Push(new Bitmap(mainImage));
            using (Graphics g = Graphics.FromImage(mainImage))
            {
                Point mousePos = imageContainer.PointToClient(Cursor.Position);
                Pen MyPen = new Pen(c);
                MyPen.Width = textSize;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                Size tsize = TextRenderer.MeasureText(textBox1.Text, new Font(Preferences.ImageEditorFont, textSize, (FontStyle)Preferences.FontStyleType));
                g.DrawString(textBox1.Text, new Font(Preferences.ImageEditorFont, textSize, (FontStyle)Preferences.FontStyleType), new SolidBrush(Color.FromArgb(128, 255, 0, 0)), new Point(mousePos.X - tsize.Width / 2, mousePos.Y - tsize.Height / 2));
            }
            ApplyCompression();
            imageContainer.Refresh();
            tempImage = true;
        }
        
        void setColor(Color newcolor)
        {
            if (toggled)
                currentColor.BackColor = newcolor;
            else
                currentColor2.BackColor = newcolor;
            c = newcolor;

        }

        void setPalette(EventArgs e, Button button)
        {
            MouseEventArgs ee = (MouseEventArgs)e;

            setColor(button.BackColor);

            colorName = button.Text;
            Point mousePos = imageContainer.PointToClient(Cursor.Position);
            label1.Text = mousePos.X.ToString() + ", " + mousePos.Y.ToString() + " - " + colorName;
        }

        private void button2_Click(object sender, EventArgs e)      {setPalette(e, c_red_1);}
        private void c_red_2_Click(object sender, EventArgs e)      {setPalette(e, c_red_2);}
        private void c_red_3_Click(object sender, EventArgs e)      {setPalette(e, c_red_3);}
        private void c_yellow_1_Click(object sender, EventArgs e)   {setPalette(e, c_yellow_1);}
        private void c_yellow_2_Click(object sender, EventArgs e)   {setPalette(e, c_yellow_2);}
        private void c_yellow_3_Click(object sender, EventArgs e)   {setPalette(e, c_yellow_3);}
        private void c_green_1_Click(object sender, EventArgs e)    {setPalette(e, c_green_1);}
        private void c_green_2_Click(object sender, EventArgs e)    {setPalette(e, c_green_2);}
        private void c_green_3_Click(object sender, EventArgs e)    {setPalette(e, c_green_3);}
        private void c_blue_1_Click(object sender, EventArgs e)     {setPalette(e, c_blue_1);}
        private void c_blue_2_Click(object sender, EventArgs e)     {setPalette(e, c_blue_2);}
        private void c_blue_3_Click(object sender, EventArgs e)     {setPalette(e, c_blue_3);}
        private void c_purple_1_Click(object sender, EventArgs e)   {setPalette(e, c_purple_1);}
        private void c_purple_2_Click(object sender, EventArgs e)   {setPalette(e, c_purple_2);}
        private void c_purple_3_Click(object sender, EventArgs e)   {setPalette(e, c_purple_3);}
        private void c_black_Click(object sender, EventArgs e)      {setPalette(e, c_black);}
        private void c_grey_Click(object sender, EventArgs e)       {setPalette(e, c_grey);}
        private void c_white_Click(object sender, EventArgs e)      {setPalette(e, c_white);}


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

        bool toggled = true;
        private void imageEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "Escape")
            {
                this.Close();
            }
            else if(e.KeyCode.ToString() == "X")
            {
                toggled = !toggled;
                if (toggled)
                {
                    
                    currentColor.BringToFront();
                    c = currentColor.BackColor;
                }
                else
                {
                    currentColor2.BringToFront();
                    c = currentColor2.BackColor;
                }
            }
            if (e.Control)
            {
                if (e.KeyCode.ToString() == "Z")
                {
                    undoImageEdit();
                }
                else if (e.KeyCode.ToString() == "T")
                {
                    if (brush)
                    {
                        addTextButton_Click(null, null);
                    }
                }
                else if (e.KeyCode.ToString() == "C")
                {
                    Clipboard.SetImage(mainImage);
                }
            }
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
            textBox1.Focus();
            textBox1.SelectAll();
        }
        
        private void currentColor_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            c = colorDialog1.Color;
            currentColor.BackColor = c;
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

        private (Image result, int sizeinbytes) CompressToJpg(Image image, int compressionPercentage)
        {
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            myEncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, compressionPercentage);
            var ms = new MemoryStream();
            image.Save(ms, GetEncoder(ImageFormat.Jpeg), myEncoderParameters);
            return (Image.FromStream(ms), (int)ms.Length);
        }
        
        void ApplyCompression()
        {
            try
            {
                jpgImage.Dispose();
            }catch { }
            
            int compressionNumber = (int)Math.Abs(100 - compressionSlider.Value);

            int size = 0;

            (jpgImage, size) = CompressToJpg(mainImage, compressionNumber);
            
            
            jpegBox.Image = jpgImage;
            if(compressionNumber < 10)
            {
                fileSizeLabel.Text = Math.Round(size / 1_000_000.0f, 2) + "MB [Do I look like I know what a jpeg is?]";
            }
            else
            {
                fileSizeLabel.Text = Math.Round(size / 1_000_000.0f, 2) + "MB";
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //textBox1.Select(textBox1.Text.Length, 0);
            UpdateImage();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Back)
            {
                if(e.Control)
                {
                    int lastSpace = textBox1.Text.LastIndexOf(' ');
                    if(lastSpace > 0)
                    {
                        textBox1.Text = textBox1.Text.Substring(0, lastSpace);
                        textBox1.Text = textBox1.Text.Replace('\u007f', ' ');
                        textBox1.Select(textBox1.Text.Length, 0);
                    }
                    else
                    {
                        textBox1.Text = "";
                    }
                }
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            InfoText.Visible = checkBox2.Checked;
        }

        private void imageEditor_Load(object sender, EventArgs e)
        {

        }
    }
}
