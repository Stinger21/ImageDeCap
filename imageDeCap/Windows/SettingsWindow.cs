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

            textBox1.Text = Preferences.SaveImagesHere;
            textBox1.Enabled = checkBox1.Checked;

            checkBox7.Checked = Preferences.EditScreenshotAfterCapture;
            checkBox3.Checked = Preferences.CopyLinksToClipboard;
            BackupImages.Checked = Preferences.DisableSoundEffects;

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
            FTPURL.Enabled = Preferences.uploadToFTP;
            FTPUsername.Enabled = Preferences.uploadToFTP;
            FTPpassword.Enabled = Preferences.uploadToFTP;
            
            HotkeyTextBox1.Text = Preferences.Hotkey1;
            HotkeyTextBox2.Text = Preferences.Hotkey2;
            HotkeyTextBox3.Text = Preferences.Hotkey3;

            FTPpassword.Text = Preferences.FTPpassword;
            FTPURL.Text = Preferences.FTPurl;
            FTPUsername.Text = Preferences.FTPusername;

            gifFPS.Value = Preferences.GIFRecordingFramerate;

            CopyImageToClipboard.Checked = Preferences.CopyImageToClipboard;

            checkBox8.Checked = Preferences.UseRuleOfThirds;

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

            //if (Preferences.GifTarget == "gfycat")
            //{
            //    gfycatButton.Checked = true;
            //}
            //else
            //{
            //    webmshareButton.Checked = true;
            //}


            BackupImages.Checked = Preferences.BackupImages;

        }


        private void button5_Click(object sender, EventArgs e) // Apply
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
            HotkeyTextBox3_Leave(null, null);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = checkBox1.Checked;
            button2.Enabled = checkBox1.Checked;
            Preferences.saveImageAtAll = checkBox1.Checked;
            Preferences.Save();
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
            Preferences.DisableSoundEffects = BackupImages.Checked;
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
            Preferences.ResetAllPreferences();
            initSettings();
            Preferences.Save();
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
                    }
                }
            }
            if(textToPutInBox == null)
            {
                return "";
            }
            else if(textToPutInBox == "")
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
                //Console.WriteLine(textToPutInBox);
                return textToPutInBox;
            }

        }

        private void HotkeyTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            HotkeyTextBox1.Text = getCurrentHotkey();
            Preferences.Hotkey1 = HotkeyTextBox1.Text;
            Preferences.Save();
        }
        private void HotkeyTextBox_PrintScreen(int number)
        {
            switch (number)
            {
                case 1:
                    HotkeyTextBox1_KeyDown(null, null);
                    if(HotkeyTextBox1.Text == "")
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
            HotkeyTextBox2.Text = getCurrentHotkey();
            Preferences.Hotkey2 = HotkeyTextBox2.Text;
            Preferences.Save();
        }
        private void HotkeyTextBox3_KeyDown(object sender, KeyEventArgs e)
        {
            HotkeyTextBox3.Text = getCurrentHotkey();
            Preferences.Hotkey3 = HotkeyTextBox3.Text;
            Preferences.Save();
        }
        private void HotkeyTextBox3_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void HotkeyTextBox2_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void HotkeyTextBox1_KeyUp(object sender, KeyEventArgs e)
        {

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
        //private void HotkeyTextBox4_GotFocus(object sender, EventArgs e)
        //{
        //    Program.hotkeysEnabled = false;
        //}
        //private void HotkeyTextBox4_LostFocus(object sender, EventArgs e)
        //{
        //    Program.hotkeysEnabled = true;
        //}


        private void CopyImageToClipboard_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.CopyImageToClipboard = CopyImageToClipboard.Checked;
            Preferences.Save();
        }
        
        private void label10_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.mattwestphal.com/");
        }

        private void imageContainer_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.mattwestphal.com/");
        }
        
        private void gifFPS_ValueChanged(object sender, EventArgs e)
        {
            Preferences.GIFRecordingFramerate = (int)gifFPS.Value;
        }
        
        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.UseRuleOfThirds = checkBox8.Checked;
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
        
        private void fontbutton_Click(object sender, EventArgs e)
        {
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                Preferences.ImageEditorFont = fontDialog.Font.Name;
                Preferences.FontStyleType = (int)fontDialog.Font.Style;
            }
            Preferences.Save();
        }
        private void HotkeyTextBox3_TextChanged(object sender, EventArgs e)
        {

        }



        /* PRINT SCREEN AAAA*/
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

        bool PrintScreenLocked = true;
        public void SetPrintScreenLocked(bool Locked)
        {
            PrintScreenLocked = Locked;
            if (Locked == false)
            {
                UnregisterHotKey(this.Handle, 0); // If locked is false, stop locking and unregister the hotkey
            }
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

                // //Console.WriteLine("pront scroon");
                
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
                //MessageBox.Show("Hotkey has been pressed!");
                // do something
            }
        }

        /*PRINT SCREEN AAA*/

        private void timer1_Tick(object sender, EventArgs e)
        {
            // this is the real cancer of windows. Putting this bind to happen once per second because windows looooooves to unbind it.
            if (PrintScreenLocked)
            {
                // If printscreen is locked, lock it once per second because fuck you
            }
        }

        private void BindUpPrintScreen()
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
            //Console.WriteLine("ENTER");
            BindUpPrintScreen();
        }

        private void HotkeyTextBox3_Leave(object sender, EventArgs e)
        {
            //Console.WriteLine("LEAVE");
            UnregisterHotKey(this.Handle, 0);
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

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

        private void imgurButton_CheckedChanged(object sender, EventArgs e)
        {
            Preferences.GifTarget = "imgur";
            Preferences.Save();
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        // Add the program to startup
        private void button4_Click(object sender, EventArgs e)
        {
            MainWindow.AddToStartup();
            MessageBox.Show("Added to startup", "ImageDeCap added to startup!");
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
    }
}
