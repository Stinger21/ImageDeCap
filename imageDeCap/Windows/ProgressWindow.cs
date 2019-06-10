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

    public partial class ProgressWindow : Form
    {
        public ProgressWindow()
        {
            InitializeComponent();
            this.Shown += FormShown;
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
