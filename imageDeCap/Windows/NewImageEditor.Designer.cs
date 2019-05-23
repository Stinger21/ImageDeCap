namespace imageDeCap
{
    partial class NewImageEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewImageEditor));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SaveButton = new System.Windows.Forms.Button();
            this.BoxButton = new System.Windows.Forms.Button();
            this.ClipboardButton = new System.Windows.Forms.Button();
            this.TextFieldInput = new System.Windows.Forms.TextBox();
            this.ArrowButton = new System.Windows.Forms.Button();
            this.FrontSwatch = new System.Windows.Forms.Button();
            this.BackSwatch = new System.Windows.Forms.Button();
            this.PickButton = new System.Windows.Forms.Button();
            this.TextButton = new System.Windows.Forms.Button();
            this.UndoButton = new System.Windows.Forms.Button();
            this.UploadButton = new System.Windows.Forms.Button();
            this.HelpButton = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.InfoText = new System.Windows.Forms.Label();
            this.ImageContainer = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageContainer)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.SaveButton);
            this.groupBox1.Controls.Add(this.BoxButton);
            this.groupBox1.Controls.Add(this.ClipboardButton);
            this.groupBox1.Controls.Add(this.TextFieldInput);
            this.groupBox1.Controls.Add(this.ArrowButton);
            this.groupBox1.Controls.Add(this.FrontSwatch);
            this.groupBox1.Controls.Add(this.BackSwatch);
            this.groupBox1.Controls.Add(this.PickButton);
            this.groupBox1.Controls.Add(this.TextButton);
            this.groupBox1.Controls.Add(this.UndoButton);
            this.groupBox1.Controls.Add(this.UploadButton);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 423);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(407, 49);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // SaveButton
            // 
            this.SaveButton.BackgroundImage = global::imageDeCap.Properties.Resources.save;
            this.SaveButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SaveButton.Location = new System.Drawing.Point(78, 14);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(30, 30);
            this.SaveButton.TabIndex = 40;
            this.toolTip1.SetToolTip(this.SaveButton, "Save to file");
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // BoxButton
            // 
            this.BoxButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BoxButton.BackgroundImage = global::imageDeCap.Properties.Resources.Box;
            this.BoxButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BoxButton.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.BoxButton.Location = new System.Drawing.Point(299, 13);
            this.BoxButton.Name = "BoxButton";
            this.BoxButton.Size = new System.Drawing.Size(30, 30);
            this.BoxButton.TabIndex = 37;
            this.toolTip1.SetToolTip(this.BoxButton, "Box (Ctrl + B)");
            this.BoxButton.UseVisualStyleBackColor = true;
            this.BoxButton.Click += new System.EventHandler(this.AddBoxButton_Click);
            // 
            // ClipboardButton
            // 
            this.ClipboardButton.BackgroundImage = global::imageDeCap.Properties.Resources.clipboard;
            this.ClipboardButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClipboardButton.Location = new System.Drawing.Point(42, 14);
            this.ClipboardButton.Name = "ClipboardButton";
            this.ClipboardButton.Size = new System.Drawing.Size(30, 30);
            this.ClipboardButton.TabIndex = 39;
            this.toolTip1.SetToolTip(this.ClipboardButton, "Copy to clipboard");
            this.ClipboardButton.UseVisualStyleBackColor = true;
            this.ClipboardButton.Click += new System.EventHandler(this.ClipboardButton_click);
            // 
            // TextFieldInput
            // 
            this.TextFieldInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TextFieldInput.Location = new System.Drawing.Point(402, 48);
            this.TextFieldInput.Multiline = true;
            this.TextFieldInput.Name = "TextFieldInput";
            this.TextFieldInput.Size = new System.Drawing.Size(117, 37);
            this.TextFieldInput.TabIndex = 23;
            this.TextFieldInput.Text = "Type Something!";
            this.TextFieldInput.TextChanged += new System.EventHandler(this.TextFieldInput_TextChanged);
            this.TextFieldInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextFieldInput_KeyDown);
            // 
            // ArrowButton
            // 
            this.ArrowButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ArrowButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ArrowButton.BackgroundImage")));
            this.ArrowButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ArrowButton.Location = new System.Drawing.Point(263, 13);
            this.ArrowButton.Name = "ArrowButton";
            this.ArrowButton.Size = new System.Drawing.Size(30, 30);
            this.ArrowButton.TabIndex = 36;
            this.toolTip1.SetToolTip(this.ArrowButton, "Arrow (Ctrl + A)");
            this.ArrowButton.UseVisualStyleBackColor = true;
            this.ArrowButton.Click += new System.EventHandler(this.AddArrowButton_Click);
            // 
            // FrontSwatch
            // 
            this.FrontSwatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FrontSwatch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(57)))), ((int)(((byte)(43)))));
            this.FrontSwatch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FrontSwatch.Location = new System.Drawing.Point(189, 13);
            this.FrontSwatch.Name = "FrontSwatch";
            this.FrontSwatch.Size = new System.Drawing.Size(22, 22);
            this.FrontSwatch.TabIndex = 30;
            this.FrontSwatch.UseVisualStyleBackColor = false;
            this.FrontSwatch.Click += new System.EventHandler(this.CurrentColor_Click);
            // 
            // BackSwatch
            // 
            this.BackSwatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BackSwatch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(56)))));
            this.BackSwatch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BackSwatch.Location = new System.Drawing.Point(197, 21);
            this.BackSwatch.Name = "BackSwatch";
            this.BackSwatch.Size = new System.Drawing.Size(22, 22);
            this.BackSwatch.TabIndex = 38;
            this.BackSwatch.UseVisualStyleBackColor = false;
            this.BackSwatch.Click += new System.EventHandler(this.CurrentColor_Click);
            // 
            // PickButton
            // 
            this.PickButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PickButton.BackgroundImage = global::imageDeCap.Properties.Resources.Dropper;
            this.PickButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.PickButton.Location = new System.Drawing.Point(335, 13);
            this.PickButton.Name = "PickButton";
            this.PickButton.Size = new System.Drawing.Size(30, 30);
            this.PickButton.TabIndex = 31;
            this.toolTip1.SetToolTip(this.PickButton, "Color Picker (Hold Alt & Click the image)");
            this.PickButton.UseVisualStyleBackColor = true;
            this.PickButton.Click += new System.EventHandler(this.PickColorButton_click);
            // 
            // TextButton
            // 
            this.TextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TextButton.BackgroundImage = global::imageDeCap.Properties.Resources.Text;
            this.TextButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.TextButton.Location = new System.Drawing.Point(227, 13);
            this.TextButton.Name = "TextButton";
            this.TextButton.Size = new System.Drawing.Size(30, 30);
            this.TextButton.TabIndex = 27;
            this.toolTip1.SetToolTip(this.TextButton, "Text (Ctrl + T & Click Image)");
            this.TextButton.UseVisualStyleBackColor = true;
            this.TextButton.Click += new System.EventHandler(this.AddTextButton_Click);
            // 
            // UndoButton
            // 
            this.UndoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UndoButton.BackgroundImage = global::imageDeCap.Properties.Resources.undo;
            this.UndoButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.UndoButton.Location = new System.Drawing.Point(371, 13);
            this.UndoButton.Name = "UndoButton";
            this.UndoButton.Size = new System.Drawing.Size(30, 30);
            this.UndoButton.TabIndex = 26;
            this.toolTip1.SetToolTip(this.UndoButton, "Undo (Ctrl + Z)");
            this.UndoButton.UseVisualStyleBackColor = true;
            this.UndoButton.Click += new System.EventHandler(this.UndoButton_Click);
            // 
            // UploadButton
            // 
            this.UploadButton.BackgroundImage = global::imageDeCap.Properties.Resources.upload;
            this.UploadButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.UploadButton.Location = new System.Drawing.Point(6, 14);
            this.UploadButton.Name = "UploadButton";
            this.UploadButton.Size = new System.Drawing.Size(30, 30);
            this.UploadButton.TabIndex = 0;
            this.toolTip1.SetToolTip(this.UploadButton, "Upload to Imgur (Enter)");
            this.UploadButton.UseVisualStyleBackColor = true;
            this.UploadButton.Click += new System.EventHandler(this.UploadButton_click);
            // 
            // HelpButton
            // 
            this.HelpButton.BackgroundImage = global::imageDeCap.Properties.Resources.Help;
            this.HelpButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.HelpButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HelpButton.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.HelpButton.Location = new System.Drawing.Point(381, 3);
            this.HelpButton.Name = "HelpButton";
            this.HelpButton.Size = new System.Drawing.Size(22, 22);
            this.HelpButton.TabIndex = 41;
            this.toolTip1.SetToolTip(this.HelpButton, "Show Help");
            this.HelpButton.UseVisualStyleBackColor = true;
            this.HelpButton.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // colorDialog1
            // 
            this.colorDialog1.FullOpen = true;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.HelpButton);
            this.panel1.Controls.Add(this.InfoText);
            this.panel1.Controls.Add(this.ImageContainer);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(407, 423);
            this.panel1.TabIndex = 1;
            this.panel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseClick);
            // 
            // InfoText
            // 
            this.InfoText.AutoSize = true;
            this.InfoText.Location = new System.Drawing.Point(218, 74);
            this.InfoText.Name = "InfoText";
            this.InfoText.Size = new System.Drawing.Size(226, 195);
            this.InfoText.TabIndex = 35;
            this.InfoText.Text = resources.GetString("InfoText.Text");
            this.InfoText.Visible = false;
            // 
            // imageContainer
            // 
            this.ImageContainer.Location = new System.Drawing.Point(3, 3);
            this.ImageContainer.Name = "imageContainer";
            this.ImageContainer.Size = new System.Drawing.Size(400, 400);
            this.ImageContainer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.ImageContainer.TabIndex = 0;
            this.ImageContainer.TabStop = false;
            this.ImageContainer.MouseClick += new System.Windows.Forms.MouseEventHandler(this.imageContainer_MouseClick);
            this.ImageContainer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.imageContainer_MouseDown);
            this.ImageContainer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.imageContainer_MouseMove);
            // 
            // NewImageEditor
            // 
            this.AcceptButton = this.UploadButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 472);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(350, 350);
            this.Name = "NewImageEditor";
            this.Text = "ImageDeCap Screenshot Editor";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.imageEditor_KeyDown);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.NewImageEditor_MouseClick);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageContainer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.TextBox TextFieldInput;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ToolTip toolTip1;
        public System.Windows.Forms.Button FrontSwatch;
        private System.Windows.Forms.Button BackSwatch;

        public System.Windows.Forms.Button UploadButton;
        public System.Windows.Forms.Button ClipboardButton;
        public System.Windows.Forms.Button SaveButton;
        public System.Windows.Forms.Button TextButton;
        public System.Windows.Forms.Button ArrowButton;
        public System.Windows.Forms.Button BoxButton;

        public System.Windows.Forms.Button PickButton;
        public System.Windows.Forms.Button UndoButton;

        public System.Windows.Forms.Button HelpButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label InfoText;
        public System.Windows.Forms.PictureBox ImageContainer;
    }
}