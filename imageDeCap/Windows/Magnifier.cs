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
    public partial class Magnifier : Form
    {
        Bitmap CopiedImage;
        Graphics CopiedImageGraphics;

        Bitmap FinalImage;
        Graphics FinalImageGraphics;

        public Magnifier(bool IsGif)
        {
            InitializeComponent();
            
            this.ShowInTaskbar = false;
            
            CaptureResolution.Text = "0x0";
            CaptureResolution.Parent = MainPictureBox;
            CaptureResolution.BackColor = Color.Transparent;

            CaptureType.Text = IsGif ? "Gif" : "Image";
            CaptureType.Parent = MainPictureBox;
            CaptureType.BackColor = Color.Transparent;

            // Init drawing stuff
            CopiedImage = new Bitmap(32, 32);
            CopiedImageGraphics = Graphics.FromImage(CopiedImage);
            CopiedImageGraphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            FinalImage = new Bitmap(128, 128);
            FinalImageGraphics = Graphics.FromImage(FinalImage);
            FinalImageGraphics.InterpolationMode = InterpolationMode.NearestNeighbor;
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            CopiedImageGraphics.CopyFromScreen(MousePosition.X - 16, MousePosition.Y - 16, 0, 0, new Size(32, 32));
            FinalImageGraphics.DrawImage(CopiedImage, 0, 0, 128, 128);
            FinalImageGraphics.DrawImage(Properties.Resources.Magnifier, 0, 0, 128, 128);
            
            MainPictureBox.Image = FinalImage;
            
            CaptureResolution.Text = Program.ImageDeCap.tempWidth + "x" + Program.ImageDeCap.tempHeight;
        }
        
        private void Magnifier_Load(object sender, EventArgs e)
        {
            this.Activate();
        }
    }
}
