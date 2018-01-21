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
            this.toolsBox = new System.Windows.Forms.GroupBox();
            this.UploadButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.BackgroundTrack = new System.Windows.Forms.TrackBar();
            this.FPSLabel = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.ScaleThing = new System.Windows.Forms.NumericUpDown();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.PictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.startTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.endTrack)).BeginInit();
            this.toolsBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BackgroundTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleThing)).BeginInit();
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
            this.startTrack.BackColor = System.Drawing.SystemColors.ControlDark;
            this.startTrack.Location = new System.Drawing.Point(12, 52);
            this.startTrack.Name = "startTrack";
            this.startTrack.Size = new System.Drawing.Size(658, 29);
            this.startTrack.TabIndex = 1;
            this.startTrack.ValueChanged += new System.EventHandler(this.startTrack_ValueChanged);
            this.startTrack.MouseDown += new System.Windows.Forms.MouseEventHandler(this.startTrack_MouseDown);
            this.startTrack.MouseUp += new System.Windows.Forms.MouseEventHandler(this.startTrack_MouseUp);
            // 
            // endTrack
            // 
            this.endTrack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.endTrack.AutoSize = false;
            this.endTrack.BackColor = System.Drawing.SystemColors.ControlDark;
            this.endTrack.Location = new System.Drawing.Point(12, 52);
            this.endTrack.Name = "endTrack";
            this.endTrack.Size = new System.Drawing.Size(658, 29);
            this.endTrack.TabIndex = 2;
            this.endTrack.Value = 10;
            this.endTrack.ValueChanged += new System.EventHandler(this.endTrack_ValueChanged);
            this.endTrack.MouseDown += new System.Windows.Forms.MouseEventHandler(this.endTrack_MouseDown);
            this.endTrack.MouseUp += new System.Windows.Forms.MouseEventHandler(this.endTrack_MouseUp);
            // 
            // toolsBox
            // 
            this.toolsBox.Controls.Add(this.UploadButton);
            this.toolsBox.Controls.Add(this.SaveButton);
            this.toolsBox.Controls.Add(this.endTrack);
            this.toolsBox.Controls.Add(this.startTrack);
            this.toolsBox.Controls.Add(this.BackgroundTrack);
            this.toolsBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolsBox.Location = new System.Drawing.Point(0, 214);
            this.toolsBox.Name = "toolsBox";
            this.toolsBox.Size = new System.Drawing.Size(682, 93);
            this.toolsBox.TabIndex = 3;
            this.toolsBox.TabStop = false;
            this.toolsBox.Text = "Tools";
            // 
            // UploadButton
            // 
            this.UploadButton.BackgroundImage = global::imageDeCap.Properties.Resources.upload;
            this.UploadButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.UploadButton.Location = new System.Drawing.Point(12, 19);
            this.UploadButton.Name = "UploadButton";
            this.UploadButton.Size = new System.Drawing.Size(30, 30);
            this.UploadButton.TabIndex = 42;
            this.UploadButton.UseVisualStyleBackColor = true;
            this.UploadButton.Click += new System.EventHandler(this.Uploadbutton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.BackgroundImage = global::imageDeCap.Properties.Resources.save;
            this.SaveButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SaveButton.Location = new System.Drawing.Point(48, 19);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(30, 30);
            this.SaveButton.TabIndex = 41;
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // BackgroundTrack
            // 
            this.BackgroundTrack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BackgroundTrack.AutoSize = false;
            this.BackgroundTrack.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundTrack.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.BackgroundTrack.Enabled = false;
            this.BackgroundTrack.Location = new System.Drawing.Point(12, 52);
            this.BackgroundTrack.Name = "BackgroundTrack";
            this.BackgroundTrack.Size = new System.Drawing.Size(658, 29);
            this.BackgroundTrack.TabIndex = 4;
            // 
            // FPSLabel
            // 
            this.FPSLabel.AutoSize = true;
            this.FPSLabel.Location = new System.Drawing.Point(515, 131);
            this.FPSLabel.Name = "FPSLabel";
            this.FPSLabel.Size = new System.Drawing.Size(145, 13);
            this.FPSLabel.TabIndex = 38;
            this.FPSLabel.Text = "Average recorded framerate: ";
            this.FPSLabel.Visible = false;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(537, 161);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 13);
            this.label9.TabIndex = 37;
            this.label9.Text = "Scale %:";
            this.label9.Visible = false;
            // 
            // ScaleThing
            // 
            this.ScaleThing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ScaleThing.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.ScaleThing.Location = new System.Drawing.Point(540, 176);
            this.ScaleThing.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.ScaleThing.Name = "ScaleThing";
            this.ScaleThing.Size = new System.Drawing.Size(120, 20);
            this.ScaleThing.TabIndex = 36;
            this.ScaleThing.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.ScaleThing.Visible = false;
            this.ScaleThing.ValueChanged += new System.EventHandler(this.ScaleThing_ValueChanged);
            // 
            // PictureBox
            // 
            this.PictureBox.Location = new System.Drawing.Point(12, 12);
            this.PictureBox.Name = "PictureBox";
            this.PictureBox.Size = new System.Drawing.Size(100, 50);
            this.PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PictureBox.TabIndex = 0;
            this.PictureBox.TabStop = false;
            this.PictureBox.Click += new System.EventHandler(this.PictureBox_Click);
            // 
            // GifEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 307);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.ScaleThing);
            this.Controls.Add(this.toolsBox);
            this.Controls.Add(this.FPSLabel);
            this.Controls.Add(this.PictureBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "GifEditor";
            this.Text = "ImageDeCap Gif Trimmer";
            this.Load += new System.EventHandler(this.GifEditor_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GifEditor_KeyDown);
            this.Resize += new System.EventHandler(this.GifEditor_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.startTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.endTrack)).EndInit();
            this.toolsBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BackgroundTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleThing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox PictureBox;
        private System.Windows.Forms.Timer frameTimer;
        private System.Windows.Forms.TrackBar startTrack;
        private System.Windows.Forms.TrackBar endTrack;
        private System.Windows.Forms.GroupBox toolsBox;
        private System.Windows.Forms.TrackBar BackgroundTrack;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown ScaleThing;
        private System.Windows.Forms.Label FPSLabel;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button UploadButton;
    }
}