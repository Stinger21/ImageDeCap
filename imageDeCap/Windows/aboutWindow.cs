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
    public partial class AboutWindow : Form
    {
        public AboutWindow()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Program.ImageDeCap.Location.X + 100, Program.ImageDeCap.Location.Y - 100);
            this.ImageDecapLabel.Text = $"ImageDeCap {MainWindow.VersionNumber}";
        }

        private void Andrew_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://andrewnewton.me/");
        }

        private void Alastair_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/mutoso");
        }

        private void Peter_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/peterlindgren");
        }

        private void Website_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://mattwestphal.com/");
        }
    }
}
