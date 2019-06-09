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
        int resolution = 16;

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
            CopiedImage = new Bitmap(resolution, resolution);
            CopiedImageGraphics = Graphics.FromImage(CopiedImage);
            CopiedImageGraphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            FinalImage = new Bitmap(128, 128);
            FinalImageGraphics = Graphics.FromImage(FinalImage);
            FinalImageGraphics.InterpolationMode = InterpolationMode.NearestNeighbor;
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            CopiedImageGraphics.CopyFromScreen(MousePosition.X - (resolution / 2), MousePosition.Y - (resolution / 2), 0, 0, new Size(resolution, resolution));
            FinalImageGraphics.DrawImage(CopiedImage, 0, 0, 128, 128);
            FinalImageGraphics.DrawImage(Properties.Resources.Magnifier, 0, 0, 128, 128);
            
            MainPictureBox.Image = FinalImage;
            
            CaptureResolution.Text = $"{ScreenCapturer.CurrentBackCover.tempWidth + 1}x{ScreenCapturer.CurrentBackCover.tempHeight + 1}";
        }
        
        private void Magnifier_Load(object sender, EventArgs e)
        {
            this.Activate();
        }
    }

    public class PictureBoxWithInterpolationMode : PictureBox
    {
        public InterpolationMode InterpolationMode { get; set; }

        protected override void OnPaint(PaintEventArgs paintEventArgs)
        {
            paintEventArgs.Graphics.InterpolationMode = InterpolationMode;
            base.OnPaint(paintEventArgs);
        }
    }
}
