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

namespace imageDeCap
{
    public partial class ProgressWindow : Form
    {
        public ProgressWindow()
        {
            this.Location = new Point(Cursor.Position.X + 25, Cursor.Position.Y + 25);
            InitializeComponent();
            this.Shown += FormShown;
            this.Show();
            Application.DoEvents();
        }

        public void SetProgress(string text, int current, int max)
        {
            this.Text = $"{text} - ImageDeCap";
            progressBar1.Value = current;
            progressBar1.Maximum = max;
            Application.DoEvents();
        }

        private void FormShown(object sender, EventArgs e)
        {
            this.Location = new Point(Cursor.Position.X + 25, Cursor.Position.Y + 25);
        }
    }
}
