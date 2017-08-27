using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace imageDeCap
{
    public partial class aboutWindow : Form
    {
        public aboutWindow()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Program.ImageDeCap.Location.X - 100, Program.ImageDeCap.Location.Y - 100);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.mattwestphal.com");
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://alastair.se/");
        }
        

        private void label7_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.mattwestphal.com/imagedecap/");
        }

        private void label1_Click_2(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void imageContainer_Click(object sender, EventArgs e)
        {

            System.Diagnostics.Process.Start("http://www.mattwestphal.com/imagedecap/");
        }

        private void aboutWindow_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
