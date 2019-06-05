using System;
using System.Windows.Forms;

namespace imageDeCap
{
    partial class GifEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GifEditor));
            this.frameTimer = new System.Windows.Forms.Timer(this.components);
            this.startTrack = new System.Windows.Forms.TrackBar();
            this.endTrack = new System.Windows.Forms.TrackBar();
            this.UploadButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.BackgroundTrack = new System.Windows.Forms.TrackBar();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.PictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.startTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.endTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackgroundTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // frameTimer
            // 
            this.frameTimer.Enabled = true;
            this.frameTimer.Tick += new System.EventHandler(this.frameTimer_Tick);
            // 
            // startTrack
            // 
            this.startTrack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.startTrack.AutoSize = false;
            this.startTrack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.startTrack.Location = new System.Drawing.Point(72, 124);
            this.startTrack.Name = "startTrack";
            this.startTrack.Size = new System.Drawing.Size(112, 36);
            this.startTrack.TabIndex = 1;
            this.startTrack.ValueChanged += new System.EventHandler(this.startTrack_ValueChanged);
            this.startTrack.MouseDown += new System.Windows.Forms.MouseEventHandler(this.startTrack_MouseDown);
            this.startTrack.MouseMove += new System.Windows.Forms.MouseEventHandler(this.startTrack_MouseMove);
            this.startTrack.MouseUp += new System.Windows.Forms.MouseEventHandler(this.startTrack_MouseUp);
            // 
            // endTrack
            // 
            this.endTrack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.endTrack.AutoSize = false;
            this.endTrack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.endTrack.Location = new System.Drawing.Point(72, 124);
            this.endTrack.Name = "endTrack";
            this.endTrack.Size = new System.Drawing.Size(112, 36);
            this.endTrack.TabIndex = 2;
            this.endTrack.Value = 10;
            this.endTrack.ValueChanged += new System.EventHandler(this.endTrack_ValueChanged);
            this.endTrack.MouseDown += new System.Windows.Forms.MouseEventHandler(this.endTrack_MouseDown);
            this.endTrack.MouseMove += new System.Windows.Forms.MouseEventHandler(this.endTrack_MouseMove);
            this.endTrack.MouseUp += new System.Windows.Forms.MouseEventHandler(this.endTrack_MouseUp);
            // 
            // UploadButton
            // 
            this.UploadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.UploadButton.BackColor = System.Drawing.SystemColors.Control;
            this.UploadButton.BackgroundImage = global::imageDeCap.Properties.Resources.upload;
            this.UploadButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.UploadButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.UploadButton.FlatAppearance.BorderSize = 0;
            this.UploadButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UploadButton.Location = new System.Drawing.Point(0, 124);
            this.UploadButton.Name = "UploadButton";
            this.UploadButton.Size = new System.Drawing.Size(36, 36);
            this.UploadButton.TabIndex = 42;
            this.UploadButton.UseVisualStyleBackColor = false;
            this.UploadButton.Click += new System.EventHandler(this.Uploadbutton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SaveButton.BackColor = System.Drawing.SystemColors.Control;
            this.SaveButton.BackgroundImage = global::imageDeCap.Properties.Resources.save;
            this.SaveButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.SaveButton.FlatAppearance.BorderSize = 0;
            this.SaveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveButton.Location = new System.Drawing.Point(36, 124);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(36, 36);
            this.SaveButton.TabIndex = 41;
            this.SaveButton.UseVisualStyleBackColor = false;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // BackgroundTrack
            // 
            this.BackgroundTrack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BackgroundTrack.AutoSize = false;
            this.BackgroundTrack.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundTrack.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BackgroundTrack.Location = new System.Drawing.Point(72, 124);
            this.BackgroundTrack.Name = "BackgroundTrack";
            this.BackgroundTrack.Size = new System.Drawing.Size(112, 36);
            this.BackgroundTrack.TabIndex = 4;
            this.BackgroundTrack.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BackgroundTrack_MouseDown);
            this.BackgroundTrack.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BackgroundTrack_MouseMove);
            this.BackgroundTrack.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BackgroundTrack_MouseUp);
            // 
            // PictureBox
            // 
            this.PictureBox.Location = new System.Drawing.Point(1, 0);
            this.PictureBox.Name = "PictureBox";
            this.PictureBox.Size = new System.Drawing.Size(100, 50);
            this.PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PictureBox.TabIndex = 0;
            this.PictureBox.TabStop = false;
            // 
            // GifEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(184, 161);
            this.Controls.Add(this.startTrack);
            this.Controls.Add(this.endTrack);
            this.Controls.Add(this.BackgroundTrack);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.UploadButton);
            this.Controls.Add(this.PictureBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "GifEditor";
            this.Text = "Trimmer - ImageDeCap";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GifEditor_KeyDown);
            this.Resize += new System.EventHandler(this.GifEditor_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.startTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.endTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackgroundTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox PictureBox;
        private System.Windows.Forms.Timer frameTimer;
        private System.Windows.Forms.TrackBar startTrack;
        private System.Windows.Forms.TrackBar endTrack;
        private System.Windows.Forms.TrackBar BackgroundTrack;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button UploadButton;
    }
}