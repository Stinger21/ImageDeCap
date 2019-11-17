using System.Drawing;
namespace imageDeCap
{
    partial class CompleteCover
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.doneButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.TimeLabel = new System.Windows.Forms.Label();
            this.FramesLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ActualFramerateLabel = new System.Windows.Forms.Label();
            this.TargetFramerateLabel = new System.Windows.Forms.Label();
            this.MemoryLabel = new System.Windows.Forms.Label();
            this.ClipCaptureTimer = new System.Windows.Forms.Timer(this.components);
            this.BoxMovementTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, -1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(10, 10);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseMove);
            // 
            // doneButton
            // 
            this.doneButton.BackColor = System.Drawing.Color.Black;
            this.doneButton.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.doneButton.FlatAppearance.BorderSize = 0;
            this.doneButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.doneButton.ForeColor = System.Drawing.Color.White;
            this.doneButton.Location = new System.Drawing.Point(0, 0);
            this.doneButton.Name = "doneButton";
            this.doneButton.Size = new System.Drawing.Size(57, 45);
            this.doneButton.TabIndex = 1;
            this.doneButton.Text = "Done";
            this.doneButton.UseVisualStyleBackColor = false;
            this.doneButton.Click += new System.EventHandler(this.DoneButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.Black;
            this.cancelButton.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.cancelButton.FlatAppearance.BorderSize = 0;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.ForeColor = System.Drawing.Color.White;
            this.cancelButton.Location = new System.Drawing.Point(57, 0);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(57, 45);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // TimeLabel
            // 
            this.TimeLabel.AutoSize = true;
            this.TimeLabel.BackColor = System.Drawing.Color.Black;
            this.TimeLabel.ForeColor = System.Drawing.Color.White;
            this.TimeLabel.Location = new System.Drawing.Point(0, 2);
            this.TimeLabel.Name = "TimeLabel";
            this.TimeLabel.Size = new System.Drawing.Size(28, 13);
            this.TimeLabel.TabIndex = 3;
            this.TimeLabel.Text = "0:00";
            // 
            // FramesLabel
            // 
            this.FramesLabel.AutoSize = true;
            this.FramesLabel.BackColor = System.Drawing.Color.Black;
            this.FramesLabel.ForeColor = System.Drawing.Color.White;
            this.FramesLabel.Location = new System.Drawing.Point(0, 17);
            this.FramesLabel.Name = "FramesLabel";
            this.FramesLabel.Size = new System.Drawing.Size(28, 13);
            this.FramesLabel.TabIndex = 4;
            this.FramesLabel.Text = "0:00";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.FramesLabel);
            this.panel1.Controls.Add(this.TimeLabel);
            this.panel1.Controls.Add(this.ActualFramerateLabel);
            this.panel1.Controls.Add(this.TargetFramerateLabel);
            this.panel1.Controls.Add(this.MemoryLabel);
            this.panel1.Location = new System.Drawing.Point(114, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(157, 45);
            this.panel1.TabIndex = 5;
            // 
            // ActualFramerateLabel
            // 
            this.ActualFramerateLabel.AutoSize = true;
            this.ActualFramerateLabel.BackColor = System.Drawing.Color.Black;
            this.ActualFramerateLabel.ForeColor = System.Drawing.Color.White;
            this.ActualFramerateLabel.Location = new System.Drawing.Point(111, 16);
            this.ActualFramerateLabel.Name = "ActualFramerateLabel";
            this.ActualFramerateLabel.Size = new System.Drawing.Size(45, 13);
            this.ActualFramerateLabel.TabIndex = 7;
            this.ActualFramerateLabel.Text = "RF: 100";
            // 
            // TargetFramerateLabel
            // 
            this.TargetFramerateLabel.AutoSize = true;
            this.TargetFramerateLabel.BackColor = System.Drawing.Color.Black;
            this.TargetFramerateLabel.ForeColor = System.Drawing.Color.White;
            this.TargetFramerateLabel.Location = new System.Drawing.Point(111, 0);
            this.TargetFramerateLabel.Name = "TargetFramerateLabel";
            this.TargetFramerateLabel.Size = new System.Drawing.Size(44, 13);
            this.TargetFramerateLabel.TabIndex = 6;
            this.TargetFramerateLabel.Text = "TF: 100";
            // 
            // MemoryLabel
            // 
            this.MemoryLabel.AutoSize = true;
            this.MemoryLabel.BackColor = System.Drawing.Color.Black;
            this.MemoryLabel.ForeColor = System.Drawing.Color.White;
            this.MemoryLabel.Location = new System.Drawing.Point(0, 31);
            this.MemoryLabel.Name = "MemoryLabel";
            this.MemoryLabel.Size = new System.Drawing.Size(28, 13);
            this.MemoryLabel.TabIndex = 6;
            this.MemoryLabel.Text = "0:00";
            // 
            // ClipCaptureTimer
            // 
            this.ClipCaptureTimer.Interval = 16;
            this.ClipCaptureTimer.Tick += new System.EventHandler(this.ClipCaptureTimer_Tick);
            // 
            // BoxMovementTimer
            // 
            this.BoxMovementTimer.Interval = 1;
            this.BoxMovementTimer.Tick += new System.EventHandler(this.BoxMovementTimer_Tick);
            // 
            // CompleteCover
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(268, 45);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.doneButton);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "CompleteCover";
            this.Opacity = 0.5D;
            this.Text = "completeCover";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Red;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CompleteCover_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CompleteCover_KeyUp);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CompleteCover_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button doneButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label TimeLabel;
        private System.Windows.Forms.Label FramesLabel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label MemoryLabel;
        public System.Windows.Forms.Timer ClipCaptureTimer;
        private System.Windows.Forms.Label ActualFramerateLabel;
        private System.Windows.Forms.Label TargetFramerateLabel;
        private System.Windows.Forms.Timer BoxMovementTimer;
    }
}