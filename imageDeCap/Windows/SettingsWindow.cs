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
        MainWindow parentForm;
        public SettingsWindow(MainWindow parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            initSettings();
        }

        void initSettings()
        {
            checkBox1.Checked = Preferences.saveImageAtAll;
            button2.Enabled = Preferences.saveImageAtAll;
            alsoSaveTextFilesBox.Enabled = Preferences.saveImageAtAll;
            alsoSaveTextFilesBox.Checked = Preferences.AlsoSaveTextFiles;

            textBox1.Text = Preferences.SaveImagesHere;
            textBox1.Enabled = checkBox1.Checked;

            checkBox7.Checked = Preferences.EditScreenshotAfterCapture;
            checkBox3.Checked = Preferences.CopyLinksToClipboard;
            checkBox4.Checked = Preferences.DisableSoundEffects;

            checkBox2.Checked = Preferences.OpenInBrowser;
            checkBox2.Enabled = !Preferences.NeverUpload;
            checkBox3.Enabled = !Preferences.NeverUpload;
            checkBox5.Enabled = !Preferences.NeverUpload;
            textBox2.Enabled = !Preferences.NeverUpload;

            neverUpload.Checked = Preferences.NeverUpload;

            checkBox5.Checked = Preferences.DisableNotifications;

            textBox2.Text = Preferences.PastebinSubjectLine;

            checkBox6.Checked = Preferences.FreezeScreenOnRegionShot;

            AlsoFTPTextFilesBox.Checked = Preferences.uploadToFTP;
            AlsoFTPTextFilesBox.Enabled = Preferences.uploadToFTP;
            AlsoFTPTextFilesBox.Checked = Preferences.AlsoFTPTextFiles;
            FTPURL.Enabled = Preferences.uploadToFTP;
            FTPUsername.Enabled = Preferences.uploadToFTP;
            FTPpassword.Enabled = Preferences.uploadToFTP;
            
            HotkeyTextBox1.Text = Preferences.Hotkey1;
            HotkeyTextBox2.Text = Preferences.Hotkey2;
            HotkeyTextBox3.Text = Preferences.Hotkey3;
            HotkeyTextBox4.Text = Preferences.Hotkey4;

            FTPpassword.Text = Preferences.FTPpassword;
            FTPURL.Text = Preferences.FTPurl;
            FTPUsername.Text = Preferences.FTPusername;

            gifFPS.Value = Preferences.GIFRecordingFramerate;

            CopyImageToClipboard.Checked = Preferences.CopyImageToClipboard;

            checkBox8.Checked = Preferences.UseRuleOfThirds;

            snipSoundBox.Text = Preferences.SnipSoundPath;
            uploadSoundBox.Text = Preferences.UploadSoundPath;
            uploadFailedSoundBox.Text = Preferences.ErrorSoundPath;

            watermarkCheckbox.Checked = Preferences.AddWatermark;
            watermarkTextbox.Text = Preferences.WatermarkFilePath;
            watermarkLocation0.Checked = false;
            watermarkLocation1.Checked = false;
            watermarkLocation2.Checked = false;
            watermarkLocation3.Checked = false;
            switch (Preferences.WatermarkLocation)
            {
                case 0:
                    watermarkLocation0.Checked = true;
                    break;
                case 1:
                    watermarkLocation1.Checked = true;
                    break;
                case 2:
                    watermarkLocation2.Checked = true;
                    break;
                case 3:
                    watermarkLocation3.Checked = true;
                    break;
            }
            
            watermarkLocation0.Enabled = Preferences.AddWatermark;
            watermarkLocation1.Enabled = Preferences.AddWatermark;
            watermarkLocation2.Enabled = Preferences.AddWatermark;
            watermarkLocation3.Enabled = Preferences.AddWatermark;
            watermarkBrowseButton.Enabled = Preferences.AddWatermark;
            watermarkTextbox.Enabled = Preferences.AddWatermark;

        }


        private void button5_Click(object sender, EventArgs e)//Apply
        {
            Preferences.saveImageAtAll = checkBox1.Checked;
            Preferences.SaveImagesHere = textBox1.Text;

            Preferences.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
                textBox1.Text = folderBrowserDialog1.SelectedPath;
        }


        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
        }
        private void writeHotkey(KeyEventArgs e, TextBox box)
        {
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
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = checkBox1.Checked;
            button2.Enabled = checkBox1.Checked;
            alsoSaveTextFilesBox.Enabled = checkBox1.Checked;
            Preferences.saveImageAtAll = checkBox1.Checked;
            Preferences.Save();
        }


        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Preferences.SaveImagesHere = textBox1.Text;
            Preferences.Save();
        }
        
        private void RegisterInStartup(bool isChecked)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey
                    ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (isChecked)
            {
                registryKey.SetValue("imageDeCap", Application.ExecutablePath);
            }
            else
            {
                registryKey.DeleteValue("imageDeCap");
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.EditScreenshotAfterCapture = checkBox7.Checked;
            Preferences.Save();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.CopyLinksToClipboard = checkBox3.Checked;
            Preferences.Save();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.DisableSoundEffects = checkBox4.Checked;
            Preferences.Save();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.OpenInBrowser = checkBox2.Checked;
            Preferences.Save();
        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
            Preferences.PastebinSubjectLine = textBox2.Text;
            Preferences.Save();
        }

        private void neverUpload_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.NeverUpload = neverUpload.Checked;
            Preferences.Save();
            checkBox2.Enabled = !Preferences.NeverUpload;
            checkBox3.Enabled = !Preferences.NeverUpload;
            checkBox5.Enabled = !Preferences.NeverUpload;
            textBox2.Enabled = !Preferences.NeverUpload;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Preferences.Reset();
            //Preferences.Save();
            initSettings();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.DisableNotifications = checkBox5.Checked;
            Preferences.Save();
            
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {

            Preferences.FreezeScreenOnRegionShot = checkBox6.Checked;
            Preferences.Save();
        }

        private void checkBoxUploadToFTP_CheckedChanged(object sender, EventArgs e)
        {
            //checkBoxUploadToFTP.Enabled = checkBoxUploadToFTP.Checked;
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

        private void alsoSaveTextFilesBox_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.AlsoSaveTextFiles = alsoSaveTextFilesBox.Checked;
            Preferences.Save();
        }

        private void AlsoFTPTectFilesBox_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.AlsoFTPTextFiles = AlsoFTPTextFilesBox.Checked;
            Preferences.Save();
        }

        public static string getCurrentHotkey()
        {

            string textToPutInBox = "";
            int length = Enum.GetValues(typeof(System.Windows.Input.Key)).Length;

            for (int i = length; i-- > 0;)
            {
                if(Enum.IsDefined(typeof(System.Windows.Input.Key), i) && i != 0)
                {
                    bool isDown = System.Windows.Input.Keyboard.IsKeyDown((System.Windows.Input.Key)i);
                    if (isDown)
                    {
                        textToPutInBox += ((System.Windows.Input.Key)i).ToString() + "+";
                        //textToPutInBox += i.ToString() + "+";
                    }
                }
            }
            if(textToPutInBox == "")
            {
                return "";
            }
            else
            {
                return textToPutInBox.Remove(textToPutInBox.Length - 1);
            }
        }

        private void HotkeyTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            HotkeyTextBox1.Text = getCurrentHotkey();
            Preferences.Hotkey1 = HotkeyTextBox1.Text;
            Preferences.Save();
        }
        private void HotkeyTextBox1_GotFocus(object sender, EventArgs e)
        {
            Program.hotkeysEnabled = false;
        }
        private void HotkeyTextBox1_LostFocus(object sender, EventArgs e)
        {
            Program.hotkeysEnabled = true;
        }

        private void HotkeyTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            HotkeyTextBox2.Text = getCurrentHotkey();
            Preferences.Hotkey2 = HotkeyTextBox2.Text;
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
        private void HotkeyTextBox3_KeyDown(object sender, KeyEventArgs e)
        {
            HotkeyTextBox3.Text = getCurrentHotkey();
            Preferences.Hotkey3 = HotkeyTextBox3.Text;
            Preferences.Save();
        }
        private void HotkeyTextBox3_GotFocus(object sender, EventArgs e)
        {
            Program.hotkeysEnabled = false;
        }
        private void HotkeyTextBox3_LostFocus(object sender, EventArgs e)
        {
            Program.hotkeysEnabled = true;
        }

        private void HotkeyTextBox4_KeyDown(object sender, KeyEventArgs e)
        {
            HotkeyTextBox4.Text = getCurrentHotkey();
            Preferences.Hotkey4 = HotkeyTextBox4.Text;
            Preferences.Save();
        }
        private void HotkeyTextBox4_GotFocus(object sender, EventArgs e)
        {
            Program.hotkeysEnabled = false;
        }
        private void HotkeyTextBox4_LostFocus(object sender, EventArgs e)
        {
            Program.hotkeysEnabled = true;
        }

        private void HotkeyTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void HotkeyTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void HotkeyTextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void HotkeyTextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void installedLabel_Click(object sender, EventArgs e)
        {

        }

        private void CopyImageToClipboard_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.CopyImageToClipboard = CopyImageToClipboard.Checked;
            Preferences.Save();
        }

        private void label3_Click_1(object sender, EventArgs e)
        {

        }
        
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.mattwestphal.com/");
        }

        private void imageContainer_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.mattwestphal.com/");
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void gifFPS_ValueChanged(object sender, EventArgs e)
        {
            Preferences.GIFRecordingFramerate = (int)gifFPS.Value;
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.UseRuleOfThirds = checkBox8.Checked;
            Preferences.Save();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (wavFileDialog.ShowDialog() == DialogResult.OK)
                snipSoundBox.Text = wavFileDialog.FileName;
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            if (wavFileDialog.ShowDialog() == DialogResult.OK)
                uploadSoundBox.Text = wavFileDialog.FileName;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (wavFileDialog.ShowDialog() == DialogResult.OK)
                uploadFailedSoundBox.Text = wavFileDialog.FileName;
        }

        private void snipSoundBox_TextChanged(object sender, EventArgs e)
        {
            Preferences.SnipSoundPath = snipSoundBox.Text;
            Preferences.Save();
        }

        private void uploadSoundBox_TextChanged(object sender, EventArgs e)
        {
            Preferences.UploadSoundPath = uploadSoundBox.Text;
            Preferences.Save();
        }

        private void uploadFailedSoundBox_TextChanged(object sender, EventArgs e)
        {
            Preferences.ErrorSoundPath = uploadFailedSoundBox.Text;
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

        private void watermarkBrowseButton_Click(object sender, EventArgs e)
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


        private void groupBox6_Enter(object sender, EventArgs e){}

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e){}

        private void fontDialog_Apply(object sender, EventArgs e)
        {

        }

        private void fontbutton_Click(object sender, EventArgs e)
        {
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                Preferences.ImageEditorFont = fontDialog.Font.Name;
                Preferences.FontStyleType = (int)fontDialog.Font.Style;
            }
            Preferences.Save();

        }
    }
}
