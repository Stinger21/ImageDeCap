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
            InitializeComponent();
            this.Shown += FormShown;
            Location = Cursor.Position;
            this.Show();
        }

        public void SetProgress(string text, int current, int max)
        {
            this.Text = $"{text} - ImageDeCap";
            progressBar1.Value = current;
            progressBar1.Maximum = max;
        }

        private void FormShown(object sender, EventArgs e)
        {
            this.Location = Cursor.Position;
        }
    }
}
