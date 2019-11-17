using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Win32;
using System.IO;

namespace imageDeCap
{
    public partial class SettingsWindow : Form
    {
        public SettingsWindow()
        {
            InitializeComponent();
            InitSettings();
        }

        void InitSettings()
        {
            checkBoxUploadToFTP.Checked         = Preferences.uploadToFTP;
            SaveImages.Checked                  = Preferences.SaveImages;
            SaveImagesBrowseButton.Enabled      = Preferences.SaveImages;
            SaveImagesHereTextBox.Text          = Preferences.SaveImagesLocation;
            SaveImagesHereTextBox.Enabled       = SaveImages.Checked;
            EditImages.Checked                  = Preferences.EditScreenshotAfterCapture;
            CopyLinksToClipboard.Checked        = Preferences.CopyLinksToClipboard;
            BackupImages.Checked                = Preferences.BackupImages;
            OpenInBrowser.Checked               = Preferences.OpenInBrowser;
            OpenInBrowser.Enabled               = !Preferences.NeverUpload;
            CopyLinksToClipboard.Enabled        = !Preferences.NeverUpload;
            DisableNotifications.Enabled        = !Preferences.NeverUpload;
            PastebinSubjectLineTextBox.Enabled  = !Preferences.NeverUpload;
            PastebinSubjectLineTextBox.Text     = Preferences.PastebinSubjectLine;
            neverUpload.Checked                 = Preferences.NeverUpload;
            DisableNotifications.Checked        = Preferences.DisableNotifications;
            FreezeScreen.Checked                = Preferences.FreezeScreenOnRegionShot;
            FTPURL.Enabled                      = Preferences.uploadToFTP;
            FTPUsername.Enabled                 = Preferences.uploadToFTP;
            FTPpassword.Enabled                 = Preferences.uploadToFTP;
            FTPLink.Enabled                     = Preferences.uploadToFTP;
            CopyFTPLink.Enabled                 = Preferences.uploadToFTP;
            FTPURL.Text                         = Preferences.FTPurl;
            FTPUsername.Text                    = Preferences.FTPusername;
            FTPpassword.Text                    = Preferences.FTPpassword;
            FTPLink.Text                        = Preferences.FTPLink;
            CopyFTPLink.Checked                 = Preferences.CopyFTPLink;
            HotkeyTextBox1.Text                 = Preferences.HotkeyText;
            HotkeyTextBox2.Text                 = Preferences.HotkeyVideo;
            HotkeyTextBox3.Text                 = Preferences.HotkeyImage;
            RecordingFramerate.Value            = Preferences.RecordingFramerate;
            CopyImageToClipboard.Checked        = Preferences.CopyImageToClipboard;
            RuleOfThirds.Checked                = Preferences.UseRuleOfThirds;
            watermarkCheckbox.Checked           = Preferences.AddWatermark;
            watermarkTextbox.Text               = Preferences.WatermarkFilePath;
            watermarkLocation0.Checked          = Preferences.WatermarkLocation == 0;
            watermarkLocation1.Checked          = Preferences.WatermarkLocation == 1;
            watermarkLocation2.Checked          = Preferences.WatermarkLocation == 2;
            watermarkLocation3.Checked          = Preferences.WatermarkLocation == 3;
            watermarkLocation0.Enabled          = Preferences.AddWatermark;
            watermarkLocation1.Enabled          = Preferences.AddWatermark;
            watermarkLocation2.Enabled          = Preferences.AddWatermark;
            watermarkLocation3.Enabled          = Preferences.AddWatermark;
            watermarkBrowseButton.Enabled       = Preferences.AddWatermark;
            watermarkTextbox.Enabled            = Preferences.AddWatermark;
            BackupImages.Checked                = Preferences.BackupImages;
        }

        private void SaveImagesBrowseButton_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
                SaveImagesHereTextBox.Text = folderBrowserDialog1.SelectedPath;
        }
        
        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SettingsWindow_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Program.ImageDeCap.Location.X - 100, Program.ImageDeCap.Location.Y - 100);
        }

        private void SettingsWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
            UnregisterHotKey(this.Handle, 0);
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            SaveImagesHereTextBox.Enabled = SaveImages.Checked;
            SaveImagesBrowseButton.Enabled = SaveImages.Checked;
            Preferences.SaveImages = SaveImages.Checked;
            Preferences.Save();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            Preferences.SaveImagesLocation = SaveImagesHereTextBox.Text;
            Preferences.Save();
        }
        
        private void CheckBox7_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.EditScreenshotAfterCapture = EditImages.Checked;
            Preferences.Save();
        }
        
        private void CopyLinksToClipboard_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.CopyLinksToClipboard = CopyLinksToClipboard.Checked;
            Preferences.Save();
        }

        private void OpenInBrowser_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.OpenInBrowser = OpenInBrowser.Checked;
            Preferences.Save();
        }

        private void TextBox2_TextChanged_1(object sender, EventArgs e)
        {
            Preferences.PastebinSubjectLine = PastebinSubjectLineTextBox.Text;
            Preferences.Save();
        }

        private void NeverUpload_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.NeverUpload = neverUpload.Checked;
            Preferences.Save();
            OpenInBrowser.Enabled = !Preferences.NeverUpload;
            CopyLinksToClipboard.Enabled = !Preferences.NeverUpload;
            DisableNotifications.Enabled = !Preferences.NeverUpload;
            PastebinSubjectLineTextBox.Enabled = !Preferences.NeverUpload;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Preferences.ResetAllPreferences();
            Preferences.Save();
            InitSettings();
        }

        private void CheckBox5_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.DisableNotifications = DisableNotifications.Checked;
            Preferences.Save();
        }

        private void CheckBox6_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.FreezeScreenOnRegionShot = FreezeScreen.Checked;
            Preferences.Save();
        }

        private void CheckBoxUploadToFTP_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.uploadToFTP = checkBoxUploadToFTP.Checked;
            Preferences.Save();
            
            FTPURL.Enabled = Preferences.uploadToFTP;
            FTPUsername.Enabled = Preferences.uploadToFTP;
            FTPpassword.Enabled = Preferences.uploadToFTP;
            FTPLink.Enabled = Preferences.uploadToFTP;
            CopyFTPLink.Enabled = Preferences.uploadToFTP;
        }

        private void FTPURL_TextChanged(object sender, EventArgs e)
        {
            Preferences.FTPurl = FTPURL.Text;
            Preferences.Save();
        }

        private void FTPUsername_TextChanged(object sender, EventArgs e)
        {
            Preferences.FTPusername = FTPUsername.Text;
            Preferences.Save();
        }

        private void FTPpassword_TextChanged(object sender, EventArgs e)
        {
            Preferences.FTPpassword = FTPpassword.Text;
            Preferences.Save();
        }
        
        private void CopyImageToClipboard_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.CopyImageToClipboard = CopyImageToClipboard.Checked;
            Preferences.Save();
        }
        
        private void ImageContainer_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.mattwestphal.com/");
        }
        
        private void ClipFPS_ValueChanged(object sender, EventArgs e)
        {
            Preferences.RecordingFramerate = (int)RecordingFramerate.Value;
            Preferences.Save();
        }
        
        private void CheckBox8_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.UseRuleOfThirds = RuleOfThirds.Checked;
            Preferences.Save();
        }
        
        private void WatermarkCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.AddWatermark = watermarkCheckbox.Checked;
            watermarkLocation0.Enabled = Preferences.AddWatermark;
            watermarkLocation1.Enabled = Preferences.AddWatermark;
            watermarkLocation2.Enabled = Preferences.AddWatermark;
            watermarkLocation3.Enabled = Preferences.AddWatermark;
            watermarkBrowseButton.Enabled = Preferences.AddWatermark;
            watermarkTextbox.Enabled = Preferences.AddWatermark;
            Preferences.Save();
        }
        
        private void WatermarkBrowseButton_Click_1(object sender, EventArgs e)
        {
            if (ImageFileDialog.ShowDialog() == DialogResult.OK)
                watermarkTextbox.Text = ImageFileDialog.FileName;
        }
        
        private void WatermarkTextbox_TextChanged(object sender, EventArgs e)
        {
            Preferences.WatermarkFilePath = watermarkTextbox.Text;
            Preferences.Save();
        }

        private void WatermarkLocation0_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.WatermarkLocation = 0;
            Preferences.Save();
        }

        private void WatermarkLocation1_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.WatermarkLocation = 1;
            Preferences.Save();
        }

        private void WatermarkLocation2_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.WatermarkLocation = 2;
            Preferences.Save();
        }

        private void WatermarkLocation3_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.WatermarkLocation = 3;
            Preferences.Save();
        }
        
        // Add the program to startup
        private void AddToStartupButton_Click(object sender, EventArgs e)
        {
            Utilities.AddToStartup();
            Utilities.BubbleNotification("Added to startup!");
        }

        private void ResetHotkeysbutton_Click(object sender, EventArgs e)
        {
            Preferences.ResetPreference(nameof(Preferences.HotkeyText));
            Preferences.ResetPreference(nameof(Preferences.HotkeyVideo));
            Preferences.ResetPreference(nameof(Preferences.HotkeyImage));
            Preferences.Save();
            InitSettings();
        }

        private void BackupFolderLink_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(MainWindow.BackupDirectory))
            {
                Directory.CreateDirectory(MainWindow.BackupDirectory);
            }
            System.Diagnostics.Process.Start(MainWindow.BackupDirectory);
        }

        private void BackupImages_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.BackupImages = BackupImages.Checked;
            Preferences.Save();
        }
        
        /* PRINT SCREEN AAAA*/
        // originally this whole program used this system for binding hotkeys (RegisterHotKey/UnregisterHotKey)
        // But the hotkeys were constantly being unbound for wierd reasons

        // This is left here to lock up the printscreen button so it can be used for typing into the hotkey box.

        private void HotkeyTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            HotkeyTextBox1.Text = Hotkeys.GetCurrentHotkey();
            Preferences.HotkeyText = HotkeyTextBox1.Text;
            Preferences.Save();
        }
        private void HotkeyTextBox2_GotFocus(object sender, EventArgs e)
        {
            Program.hotkeysEnabled = false;
        }
        private void HotkeyTextBox2_LostFocus(object sender, EventArgs e)
        {
            Program.hotkeysEnabled = true;
        }
        private void HotkeyTextBox1_GotFocus(object sender, EventArgs e)
        {
            Program.hotkeysEnabled = false;
        }
        private void HotkeyTextBox1_LostFocus(object sender, EventArgs e)
        {
            Program.hotkeysEnabled = true;
        }
        private void HotkeyTextBox3_GotFocus(object sender, EventArgs e)
        {
            Program.hotkeysEnabled = false;
        }
        private void HotkeyTextBox3_LostFocus(object sender, EventArgs e)
        {
            Program.hotkeysEnabled = true;
        }

        private void HotkeyTextBox_PrintScreen(int number)
        {
            switch (number)
            {
                case 1:
                    HotkeyTextBox1_KeyDown(null, null);
                    if (HotkeyTextBox1.Text == "")
                        HotkeyTextBox1.Text += "Snapshot";
                    else
                        HotkeyTextBox1.Text += "+Snapshot";
                    Preferences.HotkeyText = HotkeyTextBox1.Text;
                    break;
                case 2:
                    HotkeyTextBox2_KeyDown(null, null);
                    if (HotkeyTextBox2.Text == "")
                        HotkeyTextBox2.Text += "Snapshot";
                    else
                        HotkeyTextBox2.Text += "+Snapshot";
                    Preferences.HotkeyVideo = HotkeyTextBox2.Text;
                    break;
                case 3:
                    HotkeyTextBox3_KeyDown(null, null);
                    if (HotkeyTextBox3.Text == "")
                        HotkeyTextBox3.Text += "Snapshot";
                    else
                        HotkeyTextBox3.Text += "+Snapshot";
                    Preferences.HotkeyImage = HotkeyTextBox3.Text;
                    break;
                default:
                    break;
            }
            Preferences.Save();
        }
        private void HotkeyTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            HotkeyTextBox2.Text = Hotkeys.GetCurrentHotkey();
            Preferences.HotkeyVideo = HotkeyTextBox2.Text;
            Preferences.Save();
        }
        private void HotkeyTextBox3_KeyDown(object sender, KeyEventArgs e)
        {
            HotkeyTextBox3.Text = Hotkeys.GetCurrentHotkey();
            Preferences.HotkeyImage = HotkeyTextBox3.Text;
            Preferences.Save();
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            WinKey = 8
        }
        
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            
            if (m.Msg == 0x0312)
            {
                if (HotkeyTextBox1.ContainsFocus)
                    HotkeyTextBox_PrintScreen(1);
                if (HotkeyTextBox2.ContainsFocus)
                    HotkeyTextBox_PrintScreen(2);
                if (HotkeyTextBox3.ContainsFocus)
                    HotkeyTextBox_PrintScreen(3);
            }
        }

        private void LockPrintScreen()
        {
            RegisterHotKey(this.Handle, 0, 0, Keys.PrintScreen.GetHashCode());
            RegisterHotKey(this.Handle, 1, (int)KeyModifier.Alt, Keys.PrintScreen.GetHashCode());
            RegisterHotKey(this.Handle, 2, (int)KeyModifier.Control, Keys.PrintScreen.GetHashCode());
            RegisterHotKey(this.Handle, 2, (int)KeyModifier.Shift, Keys.PrintScreen.GetHashCode());
            RegisterHotKey(this.Handle, 3, (int)KeyModifier.Alt | (int)KeyModifier.Control, Keys.PrintScreen.GetHashCode());
            RegisterHotKey(this.Handle, 4, (int)KeyModifier.Control | (int)KeyModifier.Shift, Keys.PrintScreen.GetHashCode());
            RegisterHotKey(this.Handle, 5, (int)KeyModifier.Alt | (int)KeyModifier.Shift, Keys.PrintScreen.GetHashCode());
            RegisterHotKey(this.Handle, 6, (int)KeyModifier.Alt | (int)KeyModifier.Control | (int)KeyModifier.Shift, Keys.PrintScreen.GetHashCode());
        }

        private void HotkeyTextBox3_Enter(object sender, EventArgs e)
        {
            LockPrintScreen();
        }

        private void HotkeyTextBox2_Enter(object sender, EventArgs e)
        {
            LockPrintScreen();
        }

        private void HotkeyTextBox1_Enter(object sender, EventArgs e)
        {
            LockPrintScreen();
        }

        private void HotkeyTextBox3_Leave(object sender, EventArgs e)
        {
            UnregisterHotKey(this.Handle, 0);
        }

        private void HotkeyTextBox2_Leave(object sender, EventArgs e)
        {
            UnregisterHotKey(this.Handle, 0);
        }

        private void HotkeyTextBox1_Leave(object sender, EventArgs e)
        {
            UnregisterHotKey(this.Handle, 0);
        }

        private void SettingsWindow_Shown(object sender, EventArgs e)
        {
            // Starting location
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width/3, Screen.PrimaryScreen.Bounds.Height/3);
        }

        private void CopyFTPLink_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.CopyFTPLink = CopyFTPLink.Checked;
            Preferences.Save();
        }

        private void FTPLink_TextChanged(object sender, EventArgs e)
        {
            Preferences.FTPLink = FTPLink.Text;
            Preferences.Save();
        }
    }
}
