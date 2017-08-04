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
using System.Media;
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

            PictureBox wa = new PictureBox();
            wa.SetBounds(58, 58, 4, 4);
            wa.BackColor = Color.Red;
            Boxes.Add(wa);
            this.Controls.Add(wa);


             wa = new PictureBox();
            wa.SetBounds(50, 58, 4, 4);
            wa.BackColor = Color.Black;
            Boxes.Add(wa);
            this.Controls.Add(wa);
             wa = new PictureBox();
            wa.SetBounds(58, 50, 4, 4);
            wa.BackColor = Color.Black;
            Boxes.Add(wa);
            this.Controls.Add(wa);
             wa = new PictureBox();
            wa.SetBounds(66, 58, 4, 4);
            wa.BackColor = Color.Black;
            Boxes.Add(wa);
            this.Controls.Add(wa);
             wa = new PictureBox();
            wa.SetBounds(58, 66, 4, 4);
            wa.BackColor = Color.Black;
            Boxes.Add(wa);
            this.Controls.Add(wa);
            this.ShowInTaskbar = false;
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
            foreach(PictureBox FUCK in Boxes)
            {
                FUCK.BringToFront();
            }
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
    }
}
