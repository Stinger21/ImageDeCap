namespace imageDeCap
{
    partial class Magnifier
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.CaptureResolution = new System.Windows.Forms.Label();
            this.CaptureType = new System.Windows.Forms.Label();
            this.MainPictureBox = new imageDeCap.PictureBoxWithInterpolationMode();
            ((System.ComponentModel.ISupportInitialize)(this.MainPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // UpdateTimer
            // 
            this.UpdateTimer.Enabled = true;
            this.UpdateTimer.Interval = 1;
            this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
            // 
            // CaptureResolution
            // 
            this.CaptureResolution.AutoSize = true;
            this.CaptureResolution.BackColor = System.Drawing.Color.Black;
            this.CaptureResolution.ForeColor = System.Drawing.Color.White;
            this.CaptureResolution.Location = new System.Drawing.Point(-1, 1);
            this.CaptureResolution.Name = "CaptureResolution";
            this.CaptureResolution.Size = new System.Drawing.Size(24, 13);
            this.CaptureResolution.TabIndex = 3;
            this.CaptureResolution.Text = "0x0";
            // 
            // CaptureType
            // 
            this.CaptureType.AutoSize = true;
            this.CaptureType.BackColor = System.Drawing.Color.Black;
            this.CaptureType.ForeColor = System.Drawing.Color.White;
            this.CaptureType.Location = new System.Drawing.Point(-1, 12);
            this.CaptureType.Name = "CaptureType";
            this.CaptureType.Size = new System.Drawing.Size(36, 13);
            this.CaptureType.TabIndex = 4;
            this.CaptureType.Text = "Image";
            // 
            // MainPictureBox
            // 
            this.MainPictureBox.BackColor = System.Drawing.Color.Black;
            this.MainPictureBox.InitialImage = global::imageDeCap.Properties.Resources.Magnifier;
            this.MainPictureBox.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            this.MainPictureBox.Location = new System.Drawing.Point(0, 0);
            this.MainPictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.MainPictureBox.Name = "MainPictureBox";
            this.MainPictureBox.Size = new System.Drawing.Size(128, 128);
            this.MainPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.MainPictureBox.TabIndex = 2;
            this.MainPictureBox.TabStop = false;
            // 
            // Magnifier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(124, 124);
            this.Controls.Add(this.CaptureType);
            this.Controls.Add(this.CaptureResolution);
            this.Controls.Add(this.MainPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Magnifier";
            this.Text = "Magnifier";
            this.Load += new System.EventHandler(this.Magnifier_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MainPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer UpdateTimer;
        private PictureBoxWithInterpolationMode MainPictureBox;
        private System.Windows.Forms.Label CaptureResolution;
        private System.Windows.Forms.Label CaptureType;
    }
}