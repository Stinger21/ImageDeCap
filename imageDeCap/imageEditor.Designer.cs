namespace imageDeCap
{
    partial class imageEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(imageEditor));
            this.imageContainer = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.c_red_1 = new System.Windows.Forms.Button();
            this.c_red_2 = new System.Windows.Forms.Button();
            this.c_red_3 = new System.Windows.Forms.Button();
            this.c_yellow_3 = new System.Windows.Forms.Button();
            this.c_yellow_2 = new System.Windows.Forms.Button();
            this.c_yellow_1 = new System.Windows.Forms.Button();
            this.c_green_3 = new System.Windows.Forms.Button();
            this.c_green_2 = new System.Windows.Forms.Button();
            this.c_green_1 = new System.Windows.Forms.Button();
            this.c_blue_3 = new System.Windows.Forms.Button();
            this.c_blue_2 = new System.Windows.Forms.Button();
            this.c_blue_1 = new System.Windows.Forms.Button();
            this.c_purple_3 = new System.Windows.Forms.Button();
            this.c_purple_2 = new System.Windows.Forms.Button();
            this.c_purple_1 = new System.Windows.Forms.Button();
            this.c_white = new System.Windows.Forms.Button();
            this.c_grey = new System.Windows.Forms.Button();
            this.c_black = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.imageContainer)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // imageContainer
            // 
            this.imageContainer.Location = new System.Drawing.Point(9, 3);
            this.imageContainer.Name = "imageContainer";
            this.imageContainer.Size = new System.Drawing.Size(109, 113);
            this.imageContainer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.imageContainer.TabIndex = 0;
            this.imageContainer.TabStop = false;
            this.imageContainer.Click += new System.EventHandler(this.imageContainer_Click);
            this.imageContainer.MouseClick += new System.Windows.Forms.MouseEventHandler(this.imageContainer_MouseClick);
            this.imageContainer.MouseHover += new System.EventHandler(this.imageContainer_MouseHover);
            this.imageContainer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.imageContainer_MouseMove);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.c_white);
            this.groupBox1.Controls.Add(this.c_grey);
            this.groupBox1.Controls.Add(this.c_black);
            this.groupBox1.Controls.Add(this.c_purple_3);
            this.groupBox1.Controls.Add(this.c_purple_2);
            this.groupBox1.Controls.Add(this.c_purple_1);
            this.groupBox1.Controls.Add(this.c_blue_3);
            this.groupBox1.Controls.Add(this.c_blue_2);
            this.groupBox1.Controls.Add(this.c_blue_1);
            this.groupBox1.Controls.Add(this.c_green_3);
            this.groupBox1.Controls.Add(this.c_green_2);
            this.groupBox1.Controls.Add(this.c_green_1);
            this.groupBox1.Controls.Add(this.c_yellow_3);
            this.groupBox1.Controls.Add(this.c_yellow_2);
            this.groupBox1.Controls.Add(this.c_yellow_1);
            this.groupBox1.Controls.Add(this.c_red_3);
            this.groupBox1.Controls.Add(this.c_red_2);
            this.groupBox1.Controls.Add(this.c_red_1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.trackBar1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 263);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(668, 57);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tools";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(668, 263);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Screenshot";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button1.Location = new System.Drawing.Point(587, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Done";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.imageContainer);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(662, 244);
            this.panel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(587, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "0, 0";
            // 
            // trackBar1
            // 
            this.trackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar1.AutoSize = false;
            this.trackBar1.LargeChange = 1000;
            this.trackBar1.Location = new System.Drawing.Point(360, 24);
            this.trackBar1.Maximum = 10000;
            this.trackBar1.Minimum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(221, 30);
            this.trackBar1.SmallChange = 100;
            this.trackBar1.TabIndex = 2;
            this.trackBar1.TickFrequency = 1000;
            this.trackBar1.Value = 100;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(446, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Size: 5.0";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // c_red_1
            // 
            this.c_red_1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.c_red_1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.c_red_1.Location = new System.Drawing.Point(58, 9);
            this.c_red_1.Name = "c_red_1";
            this.c_red_1.Size = new System.Drawing.Size(14, 14);
            this.c_red_1.TabIndex = 4;
            this.c_red_1.UseVisualStyleBackColor = false;
            this.c_red_1.Click += new System.EventHandler(this.button2_Click);
            // 
            // c_red_2
            // 
            this.c_red_2.BackColor = System.Drawing.Color.Red;
            this.c_red_2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.c_red_2.Location = new System.Drawing.Point(58, 25);
            this.c_red_2.Name = "c_red_2";
            this.c_red_2.Size = new System.Drawing.Size(14, 14);
            this.c_red_2.TabIndex = 6;
            this.c_red_2.UseVisualStyleBackColor = false;
            this.c_red_2.Click += new System.EventHandler(this.c_red_2_Click);
            // 
            // c_red_3
            // 
            this.c_red_3.BackColor = System.Drawing.Color.Red;
            this.c_red_3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.c_red_3.Location = new System.Drawing.Point(58, 41);
            this.c_red_3.Name = "c_red_3";
            this.c_red_3.Size = new System.Drawing.Size(14, 14);
            this.c_red_3.TabIndex = 7;
            this.c_red_3.UseVisualStyleBackColor = false;
            this.c_red_3.Click += new System.EventHandler(this.c_red_3_Click);
            // 
            // c_yellow_3
            // 
            this.c_yellow_3.BackColor = System.Drawing.Color.Red;
            this.c_yellow_3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.c_yellow_3.Location = new System.Drawing.Point(78, 41);
            this.c_yellow_3.Name = "c_yellow_3";
            this.c_yellow_3.Size = new System.Drawing.Size(14, 14);
            this.c_yellow_3.TabIndex = 10;
            this.c_yellow_3.UseVisualStyleBackColor = false;
            this.c_yellow_3.Click += new System.EventHandler(this.c_yellow_3_Click);
            // 
            // c_yellow_2
            // 
            this.c_yellow_2.BackColor = System.Drawing.Color.Red;
            this.c_yellow_2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.c_yellow_2.Location = new System.Drawing.Point(78, 25);
            this.c_yellow_2.Name = "c_yellow_2";
            this.c_yellow_2.Size = new System.Drawing.Size(14, 14);
            this.c_yellow_2.TabIndex = 9;
            this.c_yellow_2.UseVisualStyleBackColor = false;
            this.c_yellow_2.Click += new System.EventHandler(this.c_yellow_2_Click);
            // 
            // c_yellow_1
            // 
            this.c_yellow_1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.c_yellow_1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.c_yellow_1.Location = new System.Drawing.Point(78, 9);
            this.c_yellow_1.Name = "c_yellow_1";
            this.c_yellow_1.Size = new System.Drawing.Size(14, 14);
            this.c_yellow_1.TabIndex = 8;
            this.c_yellow_1.UseVisualStyleBackColor = false;
            this.c_yellow_1.Click += new System.EventHandler(this.c_yellow_1_Click);
            // 
            // c_green_3
            // 
            this.c_green_3.BackColor = System.Drawing.Color.Red;
            this.c_green_3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.c_green_3.Location = new System.Drawing.Point(98, 41);
            this.c_green_3.Name = "c_green_3";
            this.c_green_3.Size = new System.Drawing.Size(14, 14);
            this.c_green_3.TabIndex = 13;
            this.c_green_3.UseVisualStyleBackColor = false;
            this.c_green_3.Click += new System.EventHandler(this.c_green_3_Click);
            // 
            // c_green_2
            // 
            this.c_green_2.BackColor = System.Drawing.Color.Red;
            this.c_green_2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.c_green_2.Location = new System.Drawing.Point(98, 25);
            this.c_green_2.Name = "c_green_2";
            this.c_green_2.Size = new System.Drawing.Size(14, 14);
            this.c_green_2.TabIndex = 12;
            this.c_green_2.UseVisualStyleBackColor = false;
            this.c_green_2.Click += new System.EventHandler(this.c_green_2_Click);
            // 
            // c_green_1
            // 
            this.c_green_1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.c_green_1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.c_green_1.Location = new System.Drawing.Point(98, 9);
            this.c_green_1.Name = "c_green_1";
            this.c_green_1.Size = new System.Drawing.Size(14, 14);
            this.c_green_1.TabIndex = 11;
            this.c_green_1.UseVisualStyleBackColor = false;
            this.c_green_1.Click += new System.EventHandler(this.c_green_1_Click);
            // 
            // c_blue_3
            // 
            this.c_blue_3.BackColor = System.Drawing.Color.Red;
            this.c_blue_3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.c_blue_3.Location = new System.Drawing.Point(118, 41);
            this.c_blue_3.Name = "c_blue_3";
            this.c_blue_3.Size = new System.Drawing.Size(14, 14);
            this.c_blue_3.TabIndex = 16;
            this.c_blue_3.UseVisualStyleBackColor = false;
            this.c_blue_3.Click += new System.EventHandler(this.c_blue_3_Click);
            // 
            // c_blue_2
            // 
            this.c_blue_2.BackColor = System.Drawing.Color.Red;
            this.c_blue_2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.c_blue_2.Location = new System.Drawing.Point(118, 25);
            this.c_blue_2.Name = "c_blue_2";
            this.c_blue_2.Size = new System.Drawing.Size(14, 14);
            this.c_blue_2.TabIndex = 15;
            this.c_blue_2.UseVisualStyleBackColor = false;
            this.c_blue_2.Click += new System.EventHandler(this.c_blue_2_Click);
            // 
            // c_blue_1
            // 
            this.c_blue_1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.c_blue_1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.c_blue_1.Location = new System.Drawing.Point(118, 9);
            this.c_blue_1.Name = "c_blue_1";
            this.c_blue_1.Size = new System.Drawing.Size(14, 14);
            this.c_blue_1.TabIndex = 14;
            this.c_blue_1.UseVisualStyleBackColor = false;
            this.c_blue_1.Click += new System.EventHandler(this.c_blue_1_Click);
            // 
            // c_purple_3
            // 
            this.c_purple_3.BackColor = System.Drawing.Color.Red;
            this.c_purple_3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.c_purple_3.Location = new System.Drawing.Point(138, 41);
            this.c_purple_3.Name = "c_purple_3";
            this.c_purple_3.Size = new System.Drawing.Size(14, 14);
            this.c_purple_3.TabIndex = 19;
            this.c_purple_3.UseVisualStyleBackColor = false;
            this.c_purple_3.Click += new System.EventHandler(this.c_purple_3_Click);
            // 
            // c_purple_2
            // 
            this.c_purple_2.BackColor = System.Drawing.Color.Red;
            this.c_purple_2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.c_purple_2.Location = new System.Drawing.Point(138, 25);
            this.c_purple_2.Name = "c_purple_2";
            this.c_purple_2.Size = new System.Drawing.Size(14, 14);
            this.c_purple_2.TabIndex = 18;
            this.c_purple_2.UseVisualStyleBackColor = false;
            this.c_purple_2.Click += new System.EventHandler(this.c_purple_2_Click);
            // 
            // c_purple_1
            // 
            this.c_purple_1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.c_purple_1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.c_purple_1.Location = new System.Drawing.Point(138, 9);
            this.c_purple_1.Name = "c_purple_1";
            this.c_purple_1.Size = new System.Drawing.Size(14, 14);
            this.c_purple_1.TabIndex = 17;
            this.c_purple_1.UseVisualStyleBackColor = false;
            this.c_purple_1.Click += new System.EventHandler(this.c_purple_1_Click);
            // 
            // c_white
            // 
            this.c_white.BackColor = System.Drawing.Color.Red;
            this.c_white.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.c_white.Location = new System.Drawing.Point(158, 41);
            this.c_white.Name = "c_white";
            this.c_white.Size = new System.Drawing.Size(14, 14);
            this.c_white.TabIndex = 22;
            this.c_white.UseVisualStyleBackColor = false;
            this.c_white.Click += new System.EventHandler(this.c_white_Click);
            // 
            // c_grey
            // 
            this.c_grey.BackColor = System.Drawing.Color.Red;
            this.c_grey.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.c_grey.Location = new System.Drawing.Point(158, 25);
            this.c_grey.Name = "c_grey";
            this.c_grey.Size = new System.Drawing.Size(14, 14);
            this.c_grey.TabIndex = 21;
            this.c_grey.UseVisualStyleBackColor = false;
            this.c_grey.Click += new System.EventHandler(this.c_grey_Click);
            // 
            // c_black
            // 
            this.c_black.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.c_black.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.c_black.Location = new System.Drawing.Point(158, 9);
            this.c_black.Name = "c_black";
            this.c_black.Size = new System.Drawing.Size(14, 14);
            this.c_black.TabIndex = 20;
            this.c_black.UseVisualStyleBackColor = false;
            this.c_black.Click += new System.EventHandler(this.c_black_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(178, 31);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(176, 20);
            this.textBox1.TabIndex = 23;
            this.textBox1.Text = "Sample Text";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(6, 16);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(51, 17);
            this.radioButton1.TabIndex = 24;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Brush";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged_1);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(6, 34);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(45, 17);
            this.radioButton2.TabIndex = 25;
            this.radioButton2.Text = "Text";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // imageEditor
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 320);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "imageEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.imageEditor_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.imageContainer)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox imageContainer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button c_red_1;
        private System.Windows.Forms.Button c_red_3;
        private System.Windows.Forms.Button c_red_2;
        private System.Windows.Forms.Button c_purple_3;
        private System.Windows.Forms.Button c_purple_2;
        private System.Windows.Forms.Button c_purple_1;
        private System.Windows.Forms.Button c_blue_3;
        private System.Windows.Forms.Button c_blue_2;
        private System.Windows.Forms.Button c_blue_1;
        private System.Windows.Forms.Button c_green_3;
        private System.Windows.Forms.Button c_green_2;
        private System.Windows.Forms.Button c_green_1;
        private System.Windows.Forms.Button c_yellow_3;
        private System.Windows.Forms.Button c_yellow_2;
        private System.Windows.Forms.Button c_yellow_1;
        private System.Windows.Forms.Button c_white;
        private System.Windows.Forms.Button c_grey;
        private System.Windows.Forms.Button c_black;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
    }
}