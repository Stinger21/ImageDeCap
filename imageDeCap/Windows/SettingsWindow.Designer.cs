﻿namespace imageDeCap
{
    partial class SettingsWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsWindow));
            this.button3 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.button4 = new System.Windows.Forms.Button();
            this.ImageFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.PrintScreenTimer = new System.Windows.Forms.Timer(this.components);
            this.imageContainer = new System.Windows.Forms.PictureBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.neverUpload = new System.Windows.Forms.CheckBox();
            this.OpenInBrowser = new System.Windows.Forms.CheckBox();
            this.CopyLinksToClipboard = new System.Windows.Forms.CheckBox();
            this.checkBoxUploadToFTP = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.FTPUsername = new System.Windows.Forms.TextBox();
            this.FTPpassword = new System.Windows.Forms.TextBox();
            this.PastebinSubjectLineTextBox = new System.Windows.Forms.TextBox();
            this.FTPURL = new System.Windows.Forms.TextBox();
            this.FTPLink = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.CopyFTPLink = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.HotkeyTextBox2 = new System.Windows.Forms.TextBox();
            this.watermarkLocation2 = new System.Windows.Forms.RadioButton();
            this.watermarkBrowseButton = new System.Windows.Forms.Button();
            this.watermarkLocation1 = new System.Windows.Forms.RadioButton();
            this.HotkeyTextBox1 = new System.Windows.Forms.TextBox();
            this.HotkeyTextBox3 = new System.Windows.Forms.TextBox();
            this.watermarkLocation0 = new System.Windows.Forms.RadioButton();
            this.watermarkTextbox = new System.Windows.Forms.TextBox();
            this.watermarkLocation3 = new System.Windows.Forms.RadioButton();
            this.watermarkCheckbox = new System.Windows.Forms.CheckBox();
            this.ResetHotkeysbutton = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.FreezeScreen = new System.Windows.Forms.CheckBox();
            this.BackupImages = new System.Windows.Forms.CheckBox();
            this.CopyImageToClipboard = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.SaveImages = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.DisableNotifications = new System.Windows.Forms.CheckBox();
            this.RuleOfThirds = new System.Windows.Forms.CheckBox();
            this.EditImages = new System.Windows.Forms.CheckBox();
            this.RecordingFramerate = new System.Windows.Forms.NumericUpDown();
            this.SaveImagesHereTextBox = new System.Windows.Forms.TextBox();
            this.SaveImagesBrowseButton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            ((System.ComponentModel.ISupportInitialize)(this.imageContainer)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RecordingFramerate)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(362, 84);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(101, 36);
            this.button3.TabIndex = 26;
            this.button3.Text = "Reset all settings";
            this.toolTip1.SetToolTip(this.button3, "Resets all preferences and settings, including first-time startup.");
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(362, 216);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 36);
            this.button1.TabIndex = 2;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Location = new System.Drawing.Point(362, 119);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(101, 36);
            this.button4.TabIndex = 32;
            this.button4.Text = "Add to startup";
            this.toolTip1.SetToolTip(this.button4, "Adds the program to startup.");
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.AddToStartupButton_Click);
            // 
            // ImageFileDialog
            // 
            this.ImageFileDialog.DefaultExt = "png";
            this.ImageFileDialog.Filter = "png (*.png)|*.png|jpg (*.jpg)|*.jpg|bmp (*.bmp)|*.bmp";
            this.ImageFileDialog.Title = "Select image file";
            // 
            // PrintScreenTimer
            // 
            this.PrintScreenTimer.Enabled = true;
            this.PrintScreenTimer.Interval = 1000;
            // 
            // imageContainer
            // 
            this.imageContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.imageContainer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.imageContainer.Image = ((System.Drawing.Image)(resources.GetObject("imageContainer.Image")));
            this.imageContainer.Location = new System.Drawing.Point(382, 12);
            this.imageContainer.Name = "imageContainer";
            this.imageContainer.Size = new System.Drawing.Size(64, 64);
            this.imageContainer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imageContainer.TabIndex = 1;
            this.imageContainer.TabStop = false;
            this.imageContainer.Click += new System.EventHandler(this.ImageContainer_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.CopyFTPLink);
            this.tabPage3.Controls.Add(this.label17);
            this.tabPage3.Controls.Add(this.FTPLink);
            this.tabPage3.Controls.Add(this.FTPURL);
            this.tabPage3.Controls.Add(this.PastebinSubjectLineTextBox);
            this.tabPage3.Controls.Add(this.FTPpassword);
            this.tabPage3.Controls.Add(this.FTPUsername);
            this.tabPage3.Controls.Add(this.label16);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.label12);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.checkBoxUploadToFTP);
            this.tabPage3.Controls.Add(this.CopyLinksToClipboard);
            this.tabPage3.Controls.Add(this.OpenInBrowser);
            this.tabPage3.Controls.Add(this.neverUpload);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(356, 229);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Uploading";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // neverUpload
            // 
            this.neverUpload.AutoSize = true;
            this.neverUpload.Location = new System.Drawing.Point(8, 17);
            this.neverUpload.Name = "neverUpload";
            this.neverUpload.Size = new System.Drawing.Size(160, 17);
            this.neverUpload.TabIndex = 7;
            this.neverUpload.Text = "Never upload to public sites.";
            this.neverUpload.UseVisualStyleBackColor = true;
            this.neverUpload.CheckedChanged += new System.EventHandler(this.NeverUpload_CheckedChanged);
            // 
            // OpenInBrowser
            // 
            this.OpenInBrowser.AutoSize = true;
            this.OpenInBrowser.Location = new System.Drawing.Point(8, 35);
            this.OpenInBrowser.Name = "OpenInBrowser";
            this.OpenInBrowser.Size = new System.Drawing.Size(207, 17);
            this.OpenInBrowser.TabIndex = 44;
            this.OpenInBrowser.Text = "Open uploaded image in web browser.";
            this.OpenInBrowser.UseVisualStyleBackColor = true;
            this.OpenInBrowser.CheckedChanged += new System.EventHandler(this.OpenInBrowser_CheckedChanged);
            // 
            // CopyLinksToClipboard
            // 
            this.CopyLinksToClipboard.AutoSize = true;
            this.CopyLinksToClipboard.Checked = true;
            this.CopyLinksToClipboard.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CopyLinksToClipboard.Location = new System.Drawing.Point(8, 52);
            this.CopyLinksToClipboard.Name = "CopyLinksToClipboard";
            this.CopyLinksToClipboard.Size = new System.Drawing.Size(135, 17);
            this.CopyLinksToClipboard.TabIndex = 45;
            this.CopyLinksToClipboard.Text = "Copy links to clipboard.";
            this.CopyLinksToClipboard.UseVisualStyleBackColor = true;
            this.CopyLinksToClipboard.CheckedChanged += new System.EventHandler(this.CopyLinksToClipboard_CheckedChanged);
            // 
            // checkBoxUploadToFTP
            // 
            this.checkBoxUploadToFTP.AutoSize = true;
            this.checkBoxUploadToFTP.Location = new System.Drawing.Point(8, 108);
            this.checkBoxUploadToFTP.Name = "checkBoxUploadToFTP";
            this.checkBoxUploadToFTP.Size = new System.Drawing.Size(95, 17);
            this.checkBoxUploadToFTP.TabIndex = 9;
            this.checkBoxUploadToFTP.Text = "Upload to FTP";
            this.checkBoxUploadToFTP.UseVisualStyleBackColor = true;
            this.checkBoxUploadToFTP.CheckedChanged += new System.EventHandler(this.CheckBoxUploadToFTP_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Pastebin Subject Line";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 3);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(44, 13);
            this.label12.TabIndex = 48;
            this.label12.Text = "General";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 49;
            this.label2.Text = "Host";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(2, 157);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 50;
            this.label6.Text = "Username";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(5, 181);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(53, 13);
            this.label16.TabIndex = 51;
            this.label16.Text = "Password";
            // 
            // FTPUsername
            // 
            this.FTPUsername.Location = new System.Drawing.Point(63, 154);
            this.FTPUsername.Name = "FTPUsername";
            this.FTPUsername.Size = new System.Drawing.Size(286, 20);
            this.FTPUsername.TabIndex = 28;
            this.FTPUsername.Text = "Username";
            this.FTPUsername.TextChanged += new System.EventHandler(this.FTPUsername_TextChanged);
            // 
            // FTPpassword
            // 
            this.FTPpassword.Location = new System.Drawing.Point(63, 178);
            this.FTPpassword.Name = "FTPpassword";
            this.FTPpassword.PasswordChar = '*';
            this.FTPpassword.Size = new System.Drawing.Size(286, 20);
            this.FTPpassword.TabIndex = 29;
            this.FTPpassword.Text = "Password";
            this.FTPpassword.TextChanged += new System.EventHandler(this.FTPpassword_TextChanged);
            // 
            // PastebinSubjectLineTextBox
            // 
            this.PastebinSubjectLineTextBox.Location = new System.Drawing.Point(123, 77);
            this.PastebinSubjectLineTextBox.Name = "PastebinSubjectLineTextBox";
            this.PastebinSubjectLineTextBox.Size = new System.Drawing.Size(223, 20);
            this.PastebinSubjectLineTextBox.TabIndex = 22;
            this.PastebinSubjectLineTextBox.Text = "ImageDeCap Upload!";
            this.PastebinSubjectLineTextBox.TextChanged += new System.EventHandler(this.TextBox2_TextChanged_1);
            // 
            // FTPURL
            // 
            this.FTPURL.Location = new System.Drawing.Point(64, 128);
            this.FTPURL.Name = "FTPURL";
            this.FTPURL.Size = new System.Drawing.Size(285, 20);
            this.FTPURL.TabIndex = 27;
            this.FTPURL.Text = "ftp://mysite.com/ftp/";
            this.FTPURL.TextChanged += new System.EventHandler(this.FTPURL_TextChanged);
            // 
            // FTPLink
            // 
            this.FTPLink.Location = new System.Drawing.Point(63, 203);
            this.FTPLink.Name = "FTPLink";
            this.FTPLink.Size = new System.Drawing.Size(126, 20);
            this.FTPLink.TabIndex = 52;
            this.FTPLink.Text = "http://mysite.com/ftp/";
            this.FTPLink.TextChanged += new System.EventHandler(this.FTPLink_TextChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(7, 206);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(51, 13);
            this.label17.TabIndex = 53;
            this.label17.Text = "Http path";
            // 
            // CopyFTPLink
            // 
            this.CopyFTPLink.AutoSize = true;
            this.CopyFTPLink.Location = new System.Drawing.Point(193, 206);
            this.CopyFTPLink.Name = "CopyFTPLink";
            this.CopyFTPLink.Size = new System.Drawing.Size(162, 17);
            this.CopyFTPLink.TabIndex = 54;
            this.CopyFTPLink.Text = "Copy HTTP link to clipboard.";
            this.CopyFTPLink.UseVisualStyleBackColor = true;
            this.CopyFTPLink.CheckedChanged += new System.EventHandler(this.CopyFTPLink_CheckedChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ResetHotkeysbutton);
            this.tabPage2.Controls.Add(this.watermarkCheckbox);
            this.tabPage2.Controls.Add(this.watermarkLocation3);
            this.tabPage2.Controls.Add(this.watermarkTextbox);
            this.tabPage2.Controls.Add(this.HotkeyTextBox3);
            this.tabPage2.Controls.Add(this.HotkeyTextBox1);
            this.tabPage2.Controls.Add(this.HotkeyTextBox2);
            this.tabPage2.Controls.Add(this.watermarkLocation0);
            this.tabPage2.Controls.Add(this.watermarkLocation1);
            this.tabPage2.Controls.Add(this.watermarkBrowseButton);
            this.tabPage2.Controls.Add(this.watermarkLocation2);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(356, 229);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Hotkeys";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 35;
            this.label5.Text = "Record clip.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 34;
            this.label4.Text = "Upload text.";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 13);
            this.label7.TabIndex = 36;
            this.label7.Text = "Capture image.";
            // 
            // HotkeyTextBox2
            // 
            this.HotkeyTextBox2.Location = new System.Drawing.Point(6, 55);
            this.HotkeyTextBox2.Multiline = true;
            this.HotkeyTextBox2.Name = "HotkeyTextBox2";
            this.HotkeyTextBox2.Size = new System.Drawing.Size(209, 17);
            this.HotkeyTextBox2.TabIndex = 31;
            this.HotkeyTextBox2.Text = "ctrl shift 3";
            this.HotkeyTextBox2.Enter += new System.EventHandler(this.HotkeyTextBox2_Enter);
            this.HotkeyTextBox2.GotFocus += new System.EventHandler(this.HotkeyTextBox2_GotFocus);
            this.HotkeyTextBox2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HotkeyTextBox2_KeyDown);
            this.HotkeyTextBox2.Leave += new System.EventHandler(this.HotkeyTextBox2_Leave);
            this.HotkeyTextBox2.LostFocus += new System.EventHandler(this.HotkeyTextBox2_LostFocus);
            // 
            // watermarkLocation2
            // 
            this.watermarkLocation2.AutoSize = true;
            this.watermarkLocation2.Location = new System.Drawing.Point(246, 126);
            this.watermarkLocation2.Name = "watermarkLocation2";
            this.watermarkLocation2.Size = new System.Drawing.Size(79, 17);
            this.watermarkLocation2.TabIndex = 47;
            this.watermarkLocation2.Text = "Bottom Left";
            this.watermarkLocation2.UseVisualStyleBackColor = true;
            this.watermarkLocation2.Visible = false;
            this.watermarkLocation2.CheckedChanged += new System.EventHandler(this.WatermarkLocation2_CheckedChanged);
            // 
            // watermarkBrowseButton
            // 
            this.watermarkBrowseButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.watermarkBrowseButton.FlatAppearance.BorderSize = 0;
            this.watermarkBrowseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.watermarkBrowseButton.Location = new System.Drawing.Point(114, 165);
            this.watermarkBrowseButton.Name = "watermarkBrowseButton";
            this.watermarkBrowseButton.Size = new System.Drawing.Size(55, 23);
            this.watermarkBrowseButton.TabIndex = 43;
            this.watermarkBrowseButton.Text = "Browse";
            this.watermarkBrowseButton.UseVisualStyleBackColor = true;
            this.watermarkBrowseButton.Visible = false;
            this.watermarkBrowseButton.Click += new System.EventHandler(this.WatermarkBrowseButton_Click_1);
            // 
            // watermarkLocation1
            // 
            this.watermarkLocation1.AutoSize = true;
            this.watermarkLocation1.Location = new System.Drawing.Point(246, 141);
            this.watermarkLocation1.Name = "watermarkLocation1";
            this.watermarkLocation1.Size = new System.Drawing.Size(72, 17);
            this.watermarkLocation1.TabIndex = 46;
            this.watermarkLocation1.Text = "Top Right";
            this.watermarkLocation1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.watermarkLocation1.UseVisualStyleBackColor = true;
            this.watermarkLocation1.Visible = false;
            this.watermarkLocation1.CheckedChanged += new System.EventHandler(this.WatermarkLocation1_CheckedChanged);
            // 
            // HotkeyTextBox1
            // 
            this.HotkeyTextBox1.Location = new System.Drawing.Point(6, 91);
            this.HotkeyTextBox1.Multiline = true;
            this.HotkeyTextBox1.Name = "HotkeyTextBox1";
            this.HotkeyTextBox1.Size = new System.Drawing.Size(209, 17);
            this.HotkeyTextBox1.TabIndex = 30;
            this.HotkeyTextBox1.Text = "ctrl shift 2";
            this.HotkeyTextBox1.Enter += new System.EventHandler(this.HotkeyTextBox1_Enter);
            this.HotkeyTextBox1.GotFocus += new System.EventHandler(this.HotkeyTextBox1_GotFocus);
            this.HotkeyTextBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HotkeyTextBox1_KeyDown);
            this.HotkeyTextBox1.Leave += new System.EventHandler(this.HotkeyTextBox1_Leave);
            this.HotkeyTextBox1.LostFocus += new System.EventHandler(this.HotkeyTextBox1_LostFocus);
            // 
            // HotkeyTextBox3
            // 
            this.HotkeyTextBox3.Location = new System.Drawing.Point(6, 19);
            this.HotkeyTextBox3.Multiline = true;
            this.HotkeyTextBox3.Name = "HotkeyTextBox3";
            this.HotkeyTextBox3.Size = new System.Drawing.Size(209, 17);
            this.HotkeyTextBox3.TabIndex = 32;
            this.HotkeyTextBox3.Text = "ctrl shift 4";
            this.HotkeyTextBox3.Enter += new System.EventHandler(this.HotkeyTextBox3_Enter);
            this.HotkeyTextBox3.GotFocus += new System.EventHandler(this.HotkeyTextBox3_GotFocus);
            this.HotkeyTextBox3.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HotkeyTextBox3_KeyDown);
            this.HotkeyTextBox3.Leave += new System.EventHandler(this.HotkeyTextBox3_Leave);
            this.HotkeyTextBox3.LostFocus += new System.EventHandler(this.HotkeyTextBox3_LostFocus);
            // 
            // watermarkLocation0
            // 
            this.watermarkLocation0.AutoSize = true;
            this.watermarkLocation0.Location = new System.Drawing.Point(246, 111);
            this.watermarkLocation0.Name = "watermarkLocation0";
            this.watermarkLocation0.Size = new System.Drawing.Size(65, 17);
            this.watermarkLocation0.TabIndex = 45;
            this.watermarkLocation0.Text = "Top Left";
            this.watermarkLocation0.UseVisualStyleBackColor = true;
            this.watermarkLocation0.Visible = false;
            this.watermarkLocation0.CheckedChanged += new System.EventHandler(this.WatermarkLocation0_CheckedChanged);
            // 
            // watermarkTextbox
            // 
            this.watermarkTextbox.Location = new System.Drawing.Point(85, 194);
            this.watermarkTextbox.Name = "watermarkTextbox";
            this.watermarkTextbox.Size = new System.Drawing.Size(96, 20);
            this.watermarkTextbox.TabIndex = 42;
            this.watermarkTextbox.Visible = false;
            this.watermarkTextbox.TextChanged += new System.EventHandler(this.WatermarkTextbox_TextChanged);
            // 
            // watermarkLocation3
            // 
            this.watermarkLocation3.AutoSize = true;
            this.watermarkLocation3.Checked = true;
            this.watermarkLocation3.Location = new System.Drawing.Point(246, 156);
            this.watermarkLocation3.Name = "watermarkLocation3";
            this.watermarkLocation3.Size = new System.Drawing.Size(86, 17);
            this.watermarkLocation3.TabIndex = 48;
            this.watermarkLocation3.TabStop = true;
            this.watermarkLocation3.Text = "Bottom Right";
            this.watermarkLocation3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.watermarkLocation3.UseVisualStyleBackColor = true;
            this.watermarkLocation3.Visible = false;
            this.watermarkLocation3.CheckedChanged += new System.EventHandler(this.WatermarkLocation3_CheckedChanged);
            // 
            // watermarkCheckbox
            // 
            this.watermarkCheckbox.AutoSize = true;
            this.watermarkCheckbox.Location = new System.Drawing.Point(187, 194);
            this.watermarkCheckbox.Name = "watermarkCheckbox";
            this.watermarkCheckbox.Size = new System.Drawing.Size(145, 17);
            this.watermarkCheckbox.TabIndex = 2;
            this.watermarkCheckbox.Text = "Add watermark to images";
            this.watermarkCheckbox.UseVisualStyleBackColor = true;
            this.watermarkCheckbox.Visible = false;
            this.watermarkCheckbox.CheckedChanged += new System.EventHandler(this.WatermarkCheckbox_CheckedChanged);
            // 
            // ResetHotkeysbutton
            // 
            this.ResetHotkeysbutton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.ResetHotkeysbutton.FlatAppearance.BorderSize = 0;
            this.ResetHotkeysbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ResetHotkeysbutton.Location = new System.Drawing.Point(8, 114);
            this.ResetHotkeysbutton.Name = "ResetHotkeysbutton";
            this.ResetHotkeysbutton.Size = new System.Drawing.Size(100, 36);
            this.ResetHotkeysbutton.TabIndex = 39;
            this.ResetHotkeysbutton.Text = "Reset hotkeys";
            this.ResetHotkeysbutton.UseVisualStyleBackColor = true;
            this.ResetHotkeysbutton.Click += new System.EventHandler(this.ResetHotkeysbutton_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.SaveImagesBrowseButton);
            this.tabPage1.Controls.Add(this.SaveImagesHereTextBox);
            this.tabPage1.Controls.Add(this.RecordingFramerate);
            this.tabPage1.Controls.Add(this.EditImages);
            this.tabPage1.Controls.Add(this.RuleOfThirds);
            this.tabPage1.Controls.Add(this.DisableNotifications);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.SaveImages);
            this.tabPage1.Controls.Add(this.label14);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.CopyImageToClipboard);
            this.tabPage1.Controls.Add(this.BackupImages);
            this.tabPage1.Controls.Add(this.FreezeScreen);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(356, 229);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // FreezeScreen
            // 
            this.FreezeScreen.AutoSize = true;
            this.FreezeScreen.Location = new System.Drawing.Point(8, 90);
            this.FreezeScreen.Name = "FreezeScreen";
            this.FreezeScreen.Size = new System.Drawing.Size(150, 17);
            this.FreezeScreen.TabIndex = 29;
            this.FreezeScreen.Text = "Freeze screen on capture.";
            this.FreezeScreen.UseVisualStyleBackColor = true;
            this.FreezeScreen.CheckedChanged += new System.EventHandler(this.CheckBox6_CheckedChanged);
            // 
            // BackupImages
            // 
            this.BackupImages.AutoSize = true;
            this.BackupImages.Checked = true;
            this.BackupImages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.BackupImages.Location = new System.Drawing.Point(8, 107);
            this.BackupImages.Name = "BackupImages";
            this.BackupImages.Size = new System.Drawing.Size(168, 17);
            this.BackupImages.TabIndex = 5;
            this.BackupImages.Text = "Back up the last 100 captures";
            this.BackupImages.UseVisualStyleBackColor = true;
            this.BackupImages.CheckedChanged += new System.EventHandler(this.BackupImages_CheckedChanged);
            // 
            // CopyImageToClipboard
            // 
            this.CopyImageToClipboard.AutoSize = true;
            this.CopyImageToClipboard.Location = new System.Drawing.Point(8, 124);
            this.CopyImageToClipboard.Name = "CopyImageToClipboard";
            this.CopyImageToClipboard.Size = new System.Drawing.Size(247, 17);
            this.CopyImageToClipboard.TabIndex = 9;
            this.CopyImageToClipboard.Text = "Copy image to clipboard immedietly on capture.";
            this.CopyImageToClipboard.UseVisualStyleBackColor = true;
            this.CopyImageToClipboard.CheckedChanged += new System.EventHandler(this.CopyImageToClipboard_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(55, 197);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(135, 13);
            this.label9.TabIndex = 35;
            this.label9.Text = "Target recording framerate.";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label14.Location = new System.Drawing.Point(169, 108);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(31, 13);
            this.label14.TabIndex = 41;
            this.label14.Text = "here.";
            this.label14.Click += new System.EventHandler(this.BackupFolderLink_Click);
            // 
            // SaveImages
            // 
            this.SaveImages.AutoSize = true;
            this.SaveImages.Location = new System.Drawing.Point(8, 19);
            this.SaveImages.Name = "SaveImages";
            this.SaveImages.Size = new System.Drawing.Size(114, 17);
            this.SaveImages.TabIndex = 0;
            this.SaveImages.Text = "Save to this folder.";
            this.SaveImages.UseVisualStyleBackColor = true;
            this.SaveImages.CheckedChanged += new System.EventHandler(this.CheckBox1_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 42;
            this.label3.Text = "Saving";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 74);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 13);
            this.label8.TabIndex = 43;
            this.label8.Text = "General";
            // 
            // DisableNotifications
            // 
            this.DisableNotifications.AutoSize = true;
            this.DisableNotifications.Location = new System.Drawing.Point(8, 141);
            this.DisableNotifications.Name = "DisableNotifications";
            this.DisableNotifications.Size = new System.Drawing.Size(123, 17);
            this.DisableNotifications.TabIndex = 5;
            this.DisableNotifications.Text = "Disable notifications.";
            this.DisableNotifications.UseVisualStyleBackColor = true;
            this.DisableNotifications.CheckedChanged += new System.EventHandler(this.CheckBox5_CheckedChanged);
            // 
            // RuleOfThirds
            // 
            this.RuleOfThirds.AutoSize = true;
            this.RuleOfThirds.Location = new System.Drawing.Point(8, 175);
            this.RuleOfThirds.Name = "RuleOfThirds";
            this.RuleOfThirds.Size = new System.Drawing.Size(192, 17);
            this.RuleOfThirds.TabIndex = 1;
            this.RuleOfThirds.Text = "Show rule of thirds when capturing.";
            this.RuleOfThirds.UseVisualStyleBackColor = true;
            this.RuleOfThirds.CheckedChanged += new System.EventHandler(this.CheckBox8_CheckedChanged);
            // 
            // EditImages
            // 
            this.EditImages.AutoSize = true;
            this.EditImages.Checked = true;
            this.EditImages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.EditImages.Location = new System.Drawing.Point(8, 158);
            this.EditImages.Name = "EditImages";
            this.EditImages.Size = new System.Drawing.Size(110, 17);
            this.EditImages.TabIndex = 8;
            this.EditImages.Text = "Edit after capture.";
            this.EditImages.UseVisualStyleBackColor = true;
            this.EditImages.CheckedChanged += new System.EventHandler(this.CheckBox7_CheckedChanged);
            // 
            // RecordingFramerate
            // 
            this.RecordingFramerate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RecordingFramerate.Location = new System.Drawing.Point(8, 194);
            this.RecordingFramerate.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.RecordingFramerate.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.RecordingFramerate.Name = "RecordingFramerate";
            this.RecordingFramerate.Size = new System.Drawing.Size(44, 20);
            this.RecordingFramerate.TabIndex = 31;
            this.toolTip1.SetToolTip(this.RecordingFramerate, "The framerate will decrease if performance is low.");
            this.RecordingFramerate.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.RecordingFramerate.ValueChanged += new System.EventHandler(this.ClipFPS_ValueChanged);
            // 
            // SaveImagesHereTextBox
            // 
            this.SaveImagesHereTextBox.Location = new System.Drawing.Point(8, 38);
            this.SaveImagesHereTextBox.Name = "SaveImagesHereTextBox";
            this.SaveImagesHereTextBox.Size = new System.Drawing.Size(228, 20);
            this.SaveImagesHereTextBox.TabIndex = 1;
            this.SaveImagesHereTextBox.Text = "C:\\";
            this.SaveImagesHereTextBox.TextChanged += new System.EventHandler(this.TextBox1_TextChanged);
            // 
            // SaveImagesBrowseButton
            // 
            this.SaveImagesBrowseButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.SaveImagesBrowseButton.FlatAppearance.BorderSize = 0;
            this.SaveImagesBrowseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveImagesBrowseButton.Location = new System.Drawing.Point(238, 37);
            this.SaveImagesBrowseButton.Name = "SaveImagesBrowseButton";
            this.SaveImagesBrowseButton.Size = new System.Drawing.Size(75, 22);
            this.SaveImagesBrowseButton.TabIndex = 2;
            this.SaveImagesBrowseButton.Text = "Browse";
            this.SaveImagesBrowseButton.UseVisualStyleBackColor = true;
            this.SaveImagesBrowseButton.Click += new System.EventHandler(this.SaveImagesBrowseButton_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(364, 255);
            this.tabControl1.TabIndex = 27;
            // 
            // SettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 254);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.imageContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(488, 335);
            this.MinimizeBox = false;
            this.Name = "SettingsWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings - ImageDeCap";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsWindow_FormClosing);
            this.Load += new System.EventHandler(this.SettingsWindow_Load);
            this.Shown += new System.EventHandler(this.SettingsWindow_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.imageContainer)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RecordingFramerate)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox imageContainer;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.OpenFileDialog ImageFileDialog;
        private System.Windows.Forms.Timer PrintScreenTimer;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.CheckBox CopyFTPLink;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox FTPLink;
        private System.Windows.Forms.TextBox FTPURL;
        private System.Windows.Forms.TextBox PastebinSubjectLineTextBox;
        private System.Windows.Forms.TextBox FTPpassword;
        private System.Windows.Forms.TextBox FTPUsername;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxUploadToFTP;
        private System.Windows.Forms.CheckBox CopyLinksToClipboard;
        private System.Windows.Forms.CheckBox OpenInBrowser;
        private System.Windows.Forms.CheckBox neverUpload;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button ResetHotkeysbutton;
        private System.Windows.Forms.CheckBox watermarkCheckbox;
        private System.Windows.Forms.RadioButton watermarkLocation3;
        private System.Windows.Forms.TextBox watermarkTextbox;
        private System.Windows.Forms.TextBox HotkeyTextBox3;
        private System.Windows.Forms.TextBox HotkeyTextBox1;
        private System.Windows.Forms.TextBox HotkeyTextBox2;
        private System.Windows.Forms.RadioButton watermarkLocation0;
        private System.Windows.Forms.RadioButton watermarkLocation1;
        private System.Windows.Forms.Button watermarkBrowseButton;
        private System.Windows.Forms.RadioButton watermarkLocation2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button SaveImagesBrowseButton;
        private System.Windows.Forms.TextBox SaveImagesHereTextBox;
        private System.Windows.Forms.NumericUpDown RecordingFramerate;
        private System.Windows.Forms.CheckBox EditImages;
        private System.Windows.Forms.CheckBox RuleOfThirds;
        private System.Windows.Forms.CheckBox DisableNotifications;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox SaveImages;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox CopyImageToClipboard;
        private System.Windows.Forms.CheckBox BackupImages;
        private System.Windows.Forms.CheckBox FreezeScreen;
        private System.Windows.Forms.TabControl tabControl1;
    }
}