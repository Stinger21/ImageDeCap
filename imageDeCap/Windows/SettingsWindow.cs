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
            initSettings();
        }

        void initSettings()
        {
            SaveImages.Checked = Preferences.saveImageAtAll;
            SaveImagesBrowseButton.Enabled = Preferences.saveImageAtAll;
            SaveImagesHereTextBox.Text = Preferences.SaveImagesHere;
            SaveImagesHereTextBox.Enabled = SaveImages.Checked;
            EditImages.Checked = Preferences.EditScreenshotAfterCapture;
            CopyLinksToClipboard.Checked = Preferences.CopyLinksToClipboard;
            BackupImages.Checked = Preferences.DisableSoundEffects;
            OpenInBrowser.Checked = Preferences.OpenInBrowser;
            OpenInBrowser.Enabled = !Preferences.NeverUpload;
            CopyLinksToClipboard.Enabled = !Preferences.NeverUpload;
            DisableNotifications.Enabled = !Preferences.NeverUpload;
            PastebinSubjectLineTextBox.Enabled = !Preferences.NeverUpload;
            PastebinSubjectLineTextBox.Text = Preferences.PastebinSubjectLine;
            neverUpload.Checked = Preferences.NeverUpload;
            DisableNotifications.Checked = Preferences.DisableNotifications;
            FreezeScreen.Checked = Preferences.FreezeScreenOnRegionShot;
            AlsoFTPTextFilesBox.Checked = Preferences.uploadToFTP;
            AlsoFTPTextFilesBox.Enabled = Preferences.uploadToFTP;
            FTPURL.Enabled = Preferences.uploadToFTP;
            FTPUsername.Enabled = Preferences.uploadToFTP;
            FTPpassword.Enabled = Preferences.uploadToFTP;
            HotkeyTextBox1.Text = Preferences.Hotkey1;
            HotkeyTextBox2.Text = Preferences.Hotkey2;
            HotkeyTextBox3.Text = Preferences.Hotkey3;
            FTPpassword.Text = Preferences.FTPpassword;
            FTPURL.Text = Preferences.FTPurl;
            FTPUsername.Text = Preferences.FTPusername;
            RecordingFramerate.Value = Preferences.GIFRecordingFramerate;
            CopyImageToClipboard.Checked = Preferences.CopyImageToClipboard;
            RuleOfThirds.Checked = Preferences.UseRuleOfThirds;
            watermarkCheckbox.Checked = Preferences.AddWatermark;
            watermarkTextbox.Text = Preferences.WatermarkFilePath;
            watermarkLocation0.Checked = Preferences.WatermarkLocation == 0;
            watermarkLocation1.Checked = Preferences.WatermarkLocation == 1;
            watermarkLocation2.Checked = Preferences.WatermarkLocation == 2;
            watermarkLocation3.Checked = Preferences.WatermarkLocation == 3;
            watermarkLocation0.Enabled = Preferences.AddWatermark;
            watermarkLocation1.Enabled = Preferences.AddWatermark;
            watermarkLocation2.Enabled = Preferences.AddWatermark;
            watermarkLocation3.Enabled = Preferences.AddWatermark;
            watermarkBrowseButton.Enabled = Preferences.AddWatermark;
            watermarkTextbox.Enabled = Preferences.AddWatermark;
            BackupImages.Checked = Preferences.BackupImages;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
                SaveImagesHereTextBox.Text = folderBrowserDialog1.SelectedPath;
        }
        
        private void button1_Click(object sender, EventArgs e)
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SaveImagesHereTextBox.Enabled = SaveImages.Checked;
            SaveImagesBrowseButton.Enabled = SaveImages.Checked;
            Preferences.saveImageAtAll = SaveImages.Checked;
            Preferences.Save();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Preferences.SaveImagesHere = SaveImagesHereTextBox.Text;
            Preferences.Save();
        }
        
        private void checkBox7_CheckedChanged(object sender, EventArgs e)
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

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
            Preferences.PastebinSubjectLine = PastebinSubjectLineTextBox.Text;
            Preferences.Save();
        }

        private void neverUpload_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.NeverUpload = neverUpload.Checked;
            Preferences.Save();
            OpenInBrowser.Enabled = !Preferences.NeverUpload;
            CopyLinksToClipboard.Enabled = !Preferences.NeverUpload;
            DisableNotifications.Enabled = !Preferences.NeverUpload;
            PastebinSubjectLineTextBox.Enabled = !Preferences.NeverUpload;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Preferences.ResetAllPreferences();
            initSettings();
            Preferences.Save();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.DisableNotifications = DisableNotifications.Checked;
            Preferences.Save();
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.FreezeScreenOnRegionShot = FreezeScreen.Checked;
            Preferences.Save();
        }

        private void checkBoxUploadToFTP_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.uploadToFTP = checkBoxUploadToFTP.Checked;
            Preferences.Save();

            AlsoFTPTextFilesBox.Enabled = Preferences.uploadToFTP;
            FTPURL.Enabled = Preferences.uploadToFTP;
            FTPUsername.Enabled = Preferences.uploadToFTP;
            FTPpassword.Enabled = Preferences.uploadToFTP;
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
        
        private void imageContainer_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.mattwestphal.com/");
        }
        
        private void gifFPS_ValueChanged(object sender, EventArgs e)
        {
            Preferences.GIFRecordingFramerate = (int)RecordingFramerate.Value;
        }
        
        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.UseRuleOfThirds = RuleOfThirds.Checked;
            Preferences.Save();
        }
        
        private void watermarkCheckbox_CheckedChanged(object sender, EventArgs e)
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
        
        private void watermarkBrowseButton_Click_1(object sender, EventArgs e)
        {
            if (ImageFileDialog.ShowDialog() == DialogResult.OK)
                watermarkTextbox.Text = ImageFileDialog.FileName;
        }
        
        private void watermarkTextbox_TextChanged(object sender, EventArgs e)
        {
            Preferences.WatermarkFilePath = watermarkTextbox.Text;
            Preferences.Save();
        }

        private void watermarkLocation0_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.WatermarkLocation = 0;
            Preferences.Save();
        }

        private void watermarkLocation1_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.WatermarkLocation = 1;
            Preferences.Save();
        }

        private void watermarkLocation2_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.WatermarkLocation = 2;
            Preferences.Save();
        }

        private void watermarkLocation3_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.WatermarkLocation = 3;
            Preferences.Save();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.GifTarget = "gfycat";
            Preferences.Save();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.GifTarget = "webmshare";
            Preferences.Save();
        }
        
        // Add the program to startup
        private void button4_Click(object sender, EventArgs e)
        {
            MainWindow.AddToStartup();
            Utilities.BubbleNotification("Added to startup!");
        }

        private void ResetHotkeysbutton_Click(object sender, EventArgs e)
        {
            Preferences.ResetPreference(nameof(Preferences.Hotkey1));
            Preferences.ResetPreference(nameof(Preferences.Hotkey2));
            Preferences.ResetPreference(nameof(Preferences.Hotkey3));
            Preferences.Save();
            initSettings();
        }

        private void label14_Click(object sender, EventArgs e)
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
        // So originally this whole program used this system for binding hotkeys (RegisterHotKey/UnregisterHotKey)
        // But the hotkeys were constantly being unbound for wierd reasons
        // So eventually I reaplaced it with a simple loop in MainWindow() that constantly checks what keys are being pressed.
        // If the keys pressed match the ones chosen in here, then it execures.

        // Unfortunetly this does not work for printscreen. idk why. So we temporaraly bind the PrintScreen button using this 
        // old system so that we can access it.

        public static string GetCurrentHotkey()
        {
            string textToPutInBox = "";
            int length = Enum.GetValues(typeof(System.Windows.Input.Key)).Length;

            for (int i = length; i-- > 0;)
            {
                if (Enum.IsDefined(typeof(System.Windows.Input.Key), i) && i != 0)
                {
                    bool isDown = System.Windows.Input.Keyboard.IsKeyDown((System.Windows.Input.Key)i);
                    if (isDown)
                    {
                        textToPutInBox += ((System.Windows.Input.Key)i).ToString() + "+";
                    }
                }
            }
            if (textToPutInBox == null)
            {
                return "";
            }
            else if (textToPutInBox == "")
            {
                return "";
            }
            else
            {
                textToPutInBox = textToPutInBox.Replace("LeftAlt", "Alt");
                textToPutInBox = textToPutInBox.Replace("RightAlt", "Alt");

                textToPutInBox = textToPutInBox.Replace("LeftCtrl", "Ctrl");
                textToPutInBox = textToPutInBox.Replace("RightCtrl", "Ctrl");

                textToPutInBox = textToPutInBox.Replace("LeftShift", "Shift");
                textToPutInBox = textToPutInBox.Replace("RightShift", "Shift");

                textToPutInBox = textToPutInBox.Remove(textToPutInBox.Length - 1);
                textToPutInBox = textToPutInBox.Replace("Scroll", "ScrollLock");
                return textToPutInBox;
            }

        }

        private void HotkeyTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            HotkeyTextBox1.Text = GetCurrentHotkey();
            Preferences.Hotkey1 = HotkeyTextBox1.Text;
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
                    Preferences.Hotkey1 = HotkeyTextBox1.Text;
                    break;
                case 2:
                    HotkeyTextBox2_KeyDown(null, null);
                    if (HotkeyTextBox2.Text == "")
                        HotkeyTextBox2.Text += "Snapshot";
                    else
                        HotkeyTextBox2.Text += "+Snapshot";
                    Preferences.Hotkey2 = HotkeyTextBox2.Text;
                    break;
                case 3:
                    HotkeyTextBox3_KeyDown(null, null);
                    if (HotkeyTextBox3.Text == "")
                        HotkeyTextBox3.Text += "Snapshot";
                    else
                        HotkeyTextBox3.Text += "+Snapshot";
                    Preferences.Hotkey3 = HotkeyTextBox3.Text;
                    break;
                default:
                    break;
            }
            Preferences.Save();
        }
        private void HotkeyTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            HotkeyTextBox2.Text = GetCurrentHotkey();
            Preferences.Hotkey2 = HotkeyTextBox2.Text;
            Preferences.Save();
        }
        private void HotkeyTextBox3_KeyDown(object sender, KeyEventArgs e)
        {
            HotkeyTextBox3.Text = GetCurrentHotkey();
            Preferences.Hotkey3 = HotkeyTextBox3.Text;
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
                /* Note that the three lines below are not needed if you only want to register one hotkey.
                 * The below lines are useful in case you want to register multiple keys, which you can use a switch with the id as argument, or if you want to know which key/modifier was pressed for some particular reason. */

                Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);                  // The key of the hotkey that was pressed.
                KeyModifier modifier = (KeyModifier)((int)m.LParam & 0xFFFF);       // The modifier of the hotkey that was pressed.
                int id = m.WParam.ToInt32();                                        // The id of the hotkey that was pressed.
                
                if (HotkeyTextBox1.ContainsFocus)
                {
                    HotkeyTextBox_PrintScreen(1);
                }
                if (HotkeyTextBox2.ContainsFocus)
                {
                    HotkeyTextBox_PrintScreen(2);
                }
                if (HotkeyTextBox3.ContainsFocus)
                {
                    HotkeyTextBox_PrintScreen(3);
                }
            }
        }

        private void BindUpPrintScreen()
        {
            Console.WriteLine("Enter");
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
            BindUpPrintScreen();
        }

        private void HotkeyTextBox2_Enter(object sender, EventArgs e)
        {
            BindUpPrintScreen();
        }

        private void HotkeyTextBox1_Enter(object sender, EventArgs e)
        {
            BindUpPrintScreen();
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

    }
}
