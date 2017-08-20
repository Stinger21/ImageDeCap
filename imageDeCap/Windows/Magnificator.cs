using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//Me
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Media;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Diagnostics;
using System.Threading;

namespace imageDeCap
{

    public partial class Magnificator : Form
    {
        List<PictureBox> Boxes = new List<PictureBox>();

        public Magnificator()
        {
            InitializeComponent();
            //SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            MakeBox(58, 57, 4, 4, Color.Red);

            // left
            MakeBox(50 - 28, 58, 32, 2, Color.Black);
            MakeBox(50 - 28, 58+2, 32, 1, Color.White);
            MakeBox(50 - 28, 58-1, 32, 1, Color.White);

            // up
            MakeBox(59, 49 - 28, 2, 32, Color.Black);
            MakeBox(59 + 2, 49 - 28, 1, 32, Color.White);
            MakeBox(59 - 1, 49 - 28, 1, 32, Color.White);

            // right
            MakeBox(66, 58, 32, 2, Color.Black);
            MakeBox(66, 58 + 2, 32, 1, Color.White);
            MakeBox(66, 58 - 1, 32, 1, Color.White);

            // down
            MakeBox(59, 65, 2, 32, Color.Black);
            MakeBox(59 + 2, 65, 1, 32, Color.White);
            MakeBox(59 - 1, 65, 1, 32, Color.White);

            this.ShowInTaskbar = false;
        }

        private void MakeBox(int x, int y, int with, int height, Color color)
        {
            PictureBox wa = new PictureBox();
            wa.SetBounds(x, y, with, height);
            wa.BackColor = color;
            Boxes.Add(wa);
            this.Controls.Add(wa);
        }

        Graphics g;
        Bitmap bmp;
        private void timer1_Tick(object sender, EventArgs e)
        {

            bmp = new Bitmap(32, 32);
            g = this.CreateGraphics();
            g = Graphics.FromImage(bmp);
            g.CopyFromScreen(MousePosition.X - 16, MousePosition.Y - 16, 0, 0, new Size(32, 32));
            pictureBoxWithInterpolationMode1.Image = bmp;
            foreach(PictureBox picture in Boxes)
            {
                picture.BringToFront();
            }
            label1.Text = Program.ImageDeCap.tempWidth + "x" + Program.ImageDeCap.tempHeight;
            label1.BringToFront();//lol
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBoxWithInterpolationMode1_Click(object sender, EventArgs e)
        {

        }

        private void Magnificator_Load(object sender, EventArgs e)
        {

            this.Activate();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
