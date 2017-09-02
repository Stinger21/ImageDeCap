namespace imageDeCap
{
    partial class Magnificator
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pictureBoxWithInterpolationMode1 = new imageDeCap.PictureBoxWithInterpolationMode();
            this.label1 = new System.Windows.Forms.Label();
            this.CaptureType = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWithInterpolationMode1)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pictureBoxWithInterpolationMode1
            // 
            this.pictureBoxWithInterpolationMode1.BackColor = System.Drawing.Color.Black;
            this.pictureBoxWithInterpolationMode1.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            this.pictureBoxWithInterpolationMode1.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxWithInterpolationMode1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxWithInterpolationMode1.Name = "pictureBoxWithInterpolationMode1";
            this.pictureBoxWithInterpolationMode1.Size = new System.Drawing.Size(128, 128);
            this.pictureBoxWithInterpolationMode1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxWithInterpolationMode1.TabIndex = 2;
            this.pictureBoxWithInterpolationMode1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "0x0";
            // 
            // CaptureType
            // 
            this.CaptureType.AutoSize = true;
            this.CaptureType.Location = new System.Drawing.Point(5, 19);
            this.CaptureType.Name = "CaptureType";
            this.CaptureType.Size = new System.Drawing.Size(36, 13);
            this.CaptureType.TabIndex = 4;
            this.CaptureType.Text = "Image";
            // 
            // Magnificator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(124, 124);
            this.Controls.Add(this.CaptureType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBoxWithInterpolationMode1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Magnificator";
            this.Text = "Magnificator";
            this.Load += new System.EventHandler(this.Magnificator_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWithInterpolationMode1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private PictureBoxWithInterpolationMode pictureBoxWithInterpolationMode1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label CaptureType;
    }
}