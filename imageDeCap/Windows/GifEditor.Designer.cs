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
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.ScaleThing = new System.Windows.Forms.NumericUpDown();
            this.sizeText = new System.Windows.Forms.Label();
            this.calcSizeButton = new System.Windows.Forms.Button();
            this.uploadButton = new System.Windows.Forms.Button();
            this.BackgroundTrack = new System.Windows.Forms.TrackBar();
            this.PictureBox = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.sizeNr = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.textValue = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.timeStart = new System.Windows.Forms.NumericUpDown();
            this.timeEnd = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.locationX = new System.Windows.Forms.NumericUpDown();
            this.locationY = new System.Windows.Forms.NumericUpDown();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            ((System.ComponentModel.ISupportInitialize)(this.startTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.endTrack)).BeginInit();
            this.toolsBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleThing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackgroundTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sizeNr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.locationX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.locationY)).BeginInit();
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
            this.toolsBox.Controls.Add(this.checkBox1);
            this.toolsBox.Controls.Add(this.label1);
            this.toolsBox.Controls.Add(this.label9);
            this.toolsBox.Controls.Add(this.ScaleThing);
            this.toolsBox.Controls.Add(this.sizeText);
            this.toolsBox.Controls.Add(this.calcSizeButton);
            this.toolsBox.Controls.Add(this.uploadButton);
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
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(428, 23);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(113, 17);
            this.checkBox1.TabIndex = 39;
            this.checkBox1.Text = "Show text controls";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(212, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(182, 13);
            this.label1.TabIndex = 38;
            this.label1.Text = "The max size imgur will take is 10 MB";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(547, 13);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 13);
            this.label9.TabIndex = 37;
            this.label9.Text = "Scale %:";
            // 
            // ScaleThing
            // 
            this.ScaleThing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ScaleThing.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.ScaleThing.Location = new System.Drawing.Point(550, 28);
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
            // 
            // sizeText
            // 
            this.sizeText.AutoSize = true;
            this.sizeText.Location = new System.Drawing.Point(213, 34);
            this.sizeText.Name = "sizeText";
            this.sizeText.Size = new System.Drawing.Size(52, 13);
            this.sizeText.TabIndex = 7;
            this.sizeText.Text = "File-Size: ";
            // 
            // calcSizeButton
            // 
            this.calcSizeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.calcSizeButton.Location = new System.Drawing.Point(93, 23);
            this.calcSizeButton.Name = "calcSizeButton";
            this.calcSizeButton.Size = new System.Drawing.Size(116, 23);
            this.calcSizeButton.TabIndex = 6;
            this.calcSizeButton.Text = "Calculate File-Size";
            this.calcSizeButton.UseVisualStyleBackColor = true;
            this.calcSizeButton.Click += new System.EventHandler(this.calcSizeButton_Click);
            // 
            // uploadButton
            // 
            this.uploadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.uploadButton.Location = new System.Drawing.Point(12, 23);
            this.uploadButton.Name = "uploadButton";
            this.uploadButton.Size = new System.Drawing.Size(75, 23);
            this.uploadButton.TabIndex = 5;
            this.uploadButton.Text = "Upload";
            this.uploadButton.UseVisualStyleBackColor = true;
            this.uploadButton.Click += new System.EventHandler(this.uploadButton_Click);
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
            // PictureBox
            // 
            this.PictureBox.Location = new System.Drawing.Point(12, 12);
            this.PictureBox.Name = "PictureBox";
            this.PictureBox.Size = new System.Drawing.Size(100, 50);
            this.PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PictureBox.TabIndex = 0;
            this.PictureBox.TabStop = false;
            this.PictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseDown);
            this.PictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseMove);
            this.PictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseUp);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.sizeNr);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.listBox1);
            this.panel1.Controls.Add(this.textValue);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.timeStart);
            this.panel1.Controls.Add(this.timeEnd);
            this.panel1.Location = new System.Drawing.Point(428, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(242, 196);
            this.panel1.TabIndex = 4;
            this.panel1.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 81);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Size:";
            // 
            // sizeNr
            // 
            this.sizeNr.Enabled = false;
            this.sizeNr.Location = new System.Drawing.Point(7, 95);
            this.sizeNr.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.sizeNr.Name = "sizeNr";
            this.sizeNr.Size = new System.Drawing.Size(64, 20);
            this.sizeNr.TabIndex = 8;
            this.sizeNr.ValueChanged += new System.EventHandler(this.sizeNr_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 155);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Time End:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(5, 54);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(68, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Add";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(5, 28);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(68, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Delete";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(77, 29);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(160, 160);
            this.listBox1.TabIndex = 5;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // textValue
            // 
            this.textValue.Enabled = false;
            this.textValue.Location = new System.Drawing.Point(3, 3);
            this.textValue.Name = "textValue";
            this.textValue.Size = new System.Drawing.Size(236, 20);
            this.textValue.TabIndex = 0;
            this.textValue.Text = "Sample Text";
            this.textValue.TextChanged += new System.EventHandler(this.textValue_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 119);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Time Start:";
            // 
            // timeStart
            // 
            this.timeStart.Enabled = false;
            this.timeStart.Location = new System.Drawing.Point(7, 133);
            this.timeStart.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.timeStart.Name = "timeStart";
            this.timeStart.Size = new System.Drawing.Size(64, 20);
            this.timeStart.TabIndex = 3;
            this.timeStart.ValueChanged += new System.EventHandler(this.timeStart_ValueChanged);
            // 
            // timeEnd
            // 
            this.timeEnd.Enabled = false;
            this.timeEnd.Location = new System.Drawing.Point(7, 169);
            this.timeEnd.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.timeEnd.Name = "timeEnd";
            this.timeEnd.Size = new System.Drawing.Size(64, 20);
            this.timeEnd.TabIndex = 4;
            this.timeEnd.ValueChanged += new System.EventHandler(this.timeEnd_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(292, 155);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Y:";
            this.label3.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(292, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "X:";
            this.label2.Visible = false;
            // 
            // locationX
            // 
            this.locationX.Enabled = false;
            this.locationX.Location = new System.Drawing.Point(295, 130);
            this.locationX.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.locationX.Minimum = new decimal(new int[] {
            100000000,
            0,
            0,
            -2147483648});
            this.locationX.Name = "locationX";
            this.locationX.Size = new System.Drawing.Size(64, 20);
            this.locationX.TabIndex = 1;
            this.locationX.Visible = false;
            this.locationX.ValueChanged += new System.EventHandler(this.locationX_ValueChanged);
            // 
            // locationY
            // 
            this.locationY.Enabled = false;
            this.locationY.Location = new System.Drawing.Point(295, 171);
            this.locationY.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.locationY.Minimum = new decimal(new int[] {
            100000000,
            0,
            0,
            -2147483648});
            this.locationY.Name = "locationY";
            this.locationY.Size = new System.Drawing.Size(64, 20);
            this.locationY.TabIndex = 2;
            this.locationY.Visible = false;
            this.locationY.ValueChanged += new System.EventHandler(this.locationY_ValueChanged);
            // 
            // GifEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 307);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.toolsBox);
            this.Controls.Add(this.PictureBox);
            this.Controls.Add(this.locationY);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.locationX);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "GifEditor";
            this.Text = "ImageDeCap Gif Editor";
            this.Load += new System.EventHandler(this.GifEditor_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GifEditor_KeyDown);
            this.Resize += new System.EventHandler(this.GifEditor_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.startTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.endTrack)).EndInit();
            this.toolsBox.ResumeLayout(false);
            this.toolsBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleThing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackgroundTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sizeNr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.locationX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.locationY)).EndInit();
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
        private System.Windows.Forms.Button uploadButton;
        private System.Windows.Forms.Button calcSizeButton;
        private System.Windows.Forms.Label sizeText;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown ScaleThing;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.NumericUpDown timeEnd;
        private System.Windows.Forms.NumericUpDown timeStart;
        private System.Windows.Forms.NumericUpDown locationY;
        private System.Windows.Forms.NumericUpDown locationX;
        private System.Windows.Forms.TextBox textValue;
        private System.Windows.Forms.NumericUpDown sizeNr;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ColorDialog colorDialog1;
    }
}