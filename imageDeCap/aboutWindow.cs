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
    }
}
