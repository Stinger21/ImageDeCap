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

namespace imageDeCap
{
    public partial class imageEditor : Form
    {
        Stack<Bitmap> undoHistory = new Stack<Bitmap>();

        bool closedIntentionally = false;
        string imagePath;
        string newImagePath = System.IO.Path.GetTempPath() + "screenshot_edited.png";
        Image theImage;
        public imageEditor(string imagePath)//on start
        {
            this.imagePath = imagePath;
            InitializeComponent();
            this.AcceptButton = button1;
            theImage = Image.FromFile(imagePath);

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
            //imageContainer.(0, 0, theImage.Width, theImage.Height);

            c_red_1.BackColor =     Color.FromArgb(255, 0, 0);
            c_red_2.BackColor =     Color.FromArgb(204, 0, 0);
            c_red_3.BackColor =     Color.FromArgb(234, 153, 153);

            c_yellow_1.BackColor =  Color.FromArgb(255, 255, 0);
            c_yellow_2.BackColor =  Color.FromArgb(240, 164, 50);
            c_yellow_3.BackColor =  Color.FromArgb(255, 229, 153);

            c_green_1.BackColor =   Color.FromArgb(0, 255, 0);
            c_green_2.BackColor =   Color.FromArgb(106, 168, 79);
            c_green_3.BackColor =   Color.FromArgb(182, 215, 168);

            c_blue_1.BackColor =    Color.FromArgb(74, 134, 232);
            c_blue_2.BackColor =    Color.FromArgb(60, 120, 216);
            c_blue_3.BackColor =    Color.FromArgb(164, 194, 244);

            c_purple_1.BackColor =  Color.FromArgb(153, 0, 255);
            c_purple_2.BackColor =  Color.FromArgb(103, 78, 167);
            c_purple_3.BackColor =  Color.FromArgb(180, 167, 214);
            
            c_black.BackColor =     Color.FromArgb(0, 0, 0);
            c_grey.BackColor =      Color.FromArgb(128, 128, 128);
            c_white.BackColor =     Color.FromArgb(255, 255, 255);

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            theImage.Save(newImagePath);
            closedIntentionally = true;
            Close();
        }

        private void imageContainer_Click(object sender, EventArgs e)
        {
        }

        private void imageContainer_MouseHover(object sender, EventArgs e)
        {
        }
        int Width = 8;
        int Height = 8;
        Point lastPos;
        bool isPressed = false;
        float brushSize = 6.0f;
        float textSize = 12.0f;
        Color c = Color.Red;
        bool brush = true;

        private void imageContainer_MouseClick(object sender, MouseEventArgs e)
        {
            Point mousePos = imageContainer.PointToClient(Cursor.Position);
            label1.Text = mousePos.X.ToString() + ", " + mousePos.Y.ToString();
            if (!brush)
            {
                undoHistory.Push(new Bitmap(theImage));
                using (Graphics g = Graphics.FromImage(theImage))
                {
                    Pen MyPen = new Pen(c);
                    MyPen.Width = textSize;
                    g.DrawString(textBox1.Text, new Font("Arial Black", textSize), new SolidBrush(c), mousePos);
                }
                imageContainer.Refresh();
            }
        }

        private void imageContainer_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = imageContainer.PointToClient(Cursor.Position);
            label1.Text = mousePos.X.ToString() + ", " + mousePos.Y.ToString();

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
                }
                
                imageContainer.Refresh();
                lastPos = mousePos;
                isPressed = true;
            }
            else
            {
                isPressed = false;
            }
        }
        
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if(brush)
            {
                brushSize = ((float)trackBar1.Value) / 100.0f;
                label2.Text = "Size: " + brushSize.ToString("0.0");
            }
            else
            {
                textSize = ((float)trackBar1.Value) / 100.0f;
                label2.Text = "Size: " + textSize.ToString("0.0");
            }
        }

        private void radioButton1_CheckedChanged_1(object sender, EventArgs e)
        {
            brush = radioButton1.Checked;

            if (brush)
            {
                trackBar1.Value = (int)brushSize * 100;
                label2.Text = "Size: " + brushSize.ToString("0.0");
            }
            else
            {
                trackBar1.Value = (int)textSize * 100;
                label2.Text = "Size: " + textSize.ToString("0.0");
            }
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

        private void button2_Click(object sender, EventArgs e)
        {
            c = Color.FromArgb(255, 0, 0);
        }

        private void c_red_2_Click(object sender, EventArgs e)
        {
            c = Color.FromArgb(204, 0, 0);
        }

        private void c_red_3_Click(object sender, EventArgs e)
        {

            c = Color.FromArgb(234, 153, 153);
        }

        private void c_yellow_1_Click(object sender, EventArgs e)
        {
            c = Color.FromArgb(255, 255, 0);
        }

        private void c_yellow_2_Click(object sender, EventArgs e)
        {
            c = Color.FromArgb(240, 164, 50);
        }

        private void c_yellow_3_Click(object sender, EventArgs e)
        {
            c = Color.FromArgb(255, 229, 153);
        }

        private void c_green_1_Click(object sender, EventArgs e)
        {
            c = Color.FromArgb(0, 255, 0);
        }

        private void c_green_2_Click(object sender, EventArgs e)
        {
            c = Color.FromArgb(106, 168, 79);
        }

        private void c_green_3_Click(object sender, EventArgs e)
        {
            c = Color.FromArgb(182, 215, 168);
        }

        private void c_blue_1_Click(object sender, EventArgs e)
        {
            c = Color.FromArgb(74, 134, 232);
        }

        private void c_blue_2_Click(object sender, EventArgs e)
        {
            c = Color.FromArgb(60, 120, 216);
        }

        private void c_blue_3_Click(object sender, EventArgs e)
        {
            c = Color.FromArgb(164, 194, 244);
        }

        private void c_purple_1_Click(object sender, EventArgs e)
        {
            c = Color.FromArgb(153, 0, 255);
        }

        private void c_purple_2_Click(object sender, EventArgs e)
        {
            c = Color.FromArgb(103, 78, 167);
        }

        private void c_purple_3_Click(object sender, EventArgs e)
        {
            c = Color.FromArgb(180, 167, 214);
        }

        private void c_black_Click(object sender, EventArgs e)
        {
            c = Color.FromArgb(0, 0, 0);
        }

        private void c_grey_Click(object sender, EventArgs e)
        {
            c = Color.FromArgb(128, 128, 128);
        }

        private void c_white_Click(object sender, EventArgs e)
        {
            c = Color.FromArgb(255, 255, 255);
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
        }

        bool ctrlIsPressed = false;
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


    }
}
