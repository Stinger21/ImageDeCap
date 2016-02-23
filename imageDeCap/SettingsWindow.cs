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
using IWshRuntimeLibrary;

namespace imageDeCap
{
    public partial class SettingsWindow : Form
    {
        Form1 parentForm;
        public SettingsWindow(Form1 parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            
            var converter = new KeysConverter();
            
            bool saveImagesAtAll = Properties.Settings.Default.saveImageAtAll;
            checkBox1.Checked = saveImagesAtAll;
            button2.Enabled = Properties.Settings.Default.saveImageAtAll;
            alsoSaveTextFilesBox.Enabled = Properties.Settings.Default.saveImageAtAll;
            alsoSaveTextFilesBox.Checked = Properties.Settings.Default.AlsoSaveTextFiles;

            textBox1.Text = Properties.Settings.Default.SaveImagesHere;
            textBox1.Enabled = checkBox1.Checked;

            checkBox7.Checked = Properties.Settings.Default.EditScreenshotAfterCapture;
            checkBox3.Checked = Properties.Settings.Default.CopyLinksToClipboard;
            checkBox4.Checked = Properties.Settings.Default.DisableSoundEffects;

            checkBox2.Checked = Properties.Settings.Default.UseHTTPS;
            checkBox2.Enabled = !Properties.Settings.Default.NeverUpload;
            checkBox3.Enabled = !Properties.Settings.Default.NeverUpload;
            checkBox5.Enabled = !Properties.Settings.Default.NeverUpload;
            textBox2.Enabled = !Properties.Settings.Default.NeverUpload;

            neverUpload.Checked = Properties.Settings.Default.NeverUpload;

            checkBox5.Checked = Properties.Settings.Default.DisableNotifications;

            textBox2.Text = Properties.Settings.Default.PastebinSubjectLine;
            
            checkBox6.Checked = Properties.Settings.Default.FreezeScreenOnRegionShot;

            checkBoxUploadToFTP.Checked = Properties.Settings.Default.uploadToFTP;
            AlsoFTPTectFilesBox.Enabled = Properties.Settings.Default.uploadToFTP;
            AlsoFTPTectFilesBox.Checked = Properties.Settings.Default.AlsoFTPTextFiles;
            FTPURL.Enabled = Properties.Settings.Default.uploadToFTP;
            FTPUsername.Enabled = Properties.Settings.Default.uploadToFTP;
            FTPpassword.Enabled = Properties.Settings.Default.uploadToFTP;


            HotkeyTextBox1.Text = Properties.Settings.Default.Hotkey1;
            HotkeyTextBox2.Text = Properties.Settings.Default.Hotkey2;
            HotkeyTextBox3.Text = Properties.Settings.Default.Hotkey3;
            HotkeyTextBox4.Text = Properties.Settings.Default.Hotkey4;

            FTPpassword.Text = Properties.Settings.Default.FTPpassword;
            FTPURL.Text = Properties.Settings.Default.FTPurl;
            FTPUsername.Text = Properties.Settings.Default.FTPusername;

            //CHECK IF INSTALLED
            isInstalled();
            
        }
        public bool isInstalled()
        {
            bool installed = true;
            string startMenuShortcutPath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + @"\imageDeCap.lnk";
            string appdataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\imageDeCap";
            string startupDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            if (!Directory.Exists(appdataDirectory))
            {
                Directory.CreateDirectory(appdataDirectory);
            }

            if (System.IO.File.Exists(appdataDirectory + @"\imageDeCap.exe"))
            {
                if (System.IO.File.Exists(startupDirectory + @"\imageDeCap.lnk"))
                {
                    if(System.IO.File.Exists(startMenuShortcutPath))
                    {
                        installedLabel.Text = "Installed!";
                        //installButton.Enabled = false;
                        //uninstallButton.Enabled = true;
                        installed = true;
                        //button3.Enabled = false;
                    }
                }
            }

            if (!System.IO.File.Exists(appdataDirectory + @"\imageDeCap.exe") || !System.IO.File.Exists(startupDirectory + @"\imageDeCap.lnk") || !System.IO.File.Exists(startMenuShortcutPath))
            {
                installedLabel.Text = "Not Installed.";
                //installButton.Enabled = true;
                //uninstallButton.Enabled = false;
                installed = false;
                //button3.Enabled = true;
            }
            return installed;
        }
        private void CreateShortcut(string targetProgram, string shortcutPath)
        {
            object shDesktop = (object)"Desktop";
            WshShell shell = new WshShell();
            string shortcutAddress = shortcutPath;
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
            shortcut.Description = "imageDeCap auto-start";
            //shortcut.Hotkey = "Ctrl+Shift+N";
            shortcut.TargetPath = targetProgram;
            shortcut.Save();
        }
        public void Install()
        {
            string shortcutPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\imageDeCap.lnk";
            string startMenuShortcutPath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + @"\imageDeCap.lnk";
            string programPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\imageDeCap\imageDeCap.exe";
            if(System.IO.File.Exists(programPath))
            {
                System.IO.File.Delete(programPath);
            }
            if (System.IO.File.Exists(shortcutPath))
            {
                System.IO.File.Delete(shortcutPath);
            }
            if (System.IO.File.Exists(startMenuShortcutPath))
            {
                System.IO.File.Delete(startMenuShortcutPath);
            }
            System.IO.File.Copy(System.Reflection.Assembly.GetEntryAssembly().Location, programPath);
            CreateShortcut(programPath, shortcutPath);
            CreateShortcut(programPath, startMenuShortcutPath);
            isInstalled();
        }
        public void UnInstall()
        {
            string shortcutPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\imageDeCap.lnk";
            string startMenuShortcutPath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + @"\imageDeCap.lnk";
            string programPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\imageDeCap\imageDeCap.exe";
            System.IO.File.Delete(startMenuShortcutPath);
            System.IO.File.Delete(shortcutPath);
            System.IO.File.Delete(programPath);
            isInstalled();
        }
        private void installButton_Click(object sender, EventArgs e)
        {
            if(isInstalled())
            {
                UnInstall();
                installButton.Text = "Install and add to startup";
                Properties.Settings.Default.IsInstalled = false;
                Properties.Settings.Default.Save();
            }
            else
            {
                Install();
                installButton.Text = "Uninstall";
                Properties.Settings.Default.IsInstalled = true;
                Properties.Settings.Default.Save();
            }
            
            //Properties.Settings.Default.IsInstalled = true;
            //Properties.Settings.Default.Save();
        }
        
        private void uninstallButton_Click(object sender, EventArgs e)
        {
            //UnInstall();
            //Properties.Settings.Default.IsInstalled = false;
            //Properties.Settings.Default.Save();
        }

        private void button5_Click(object sender, EventArgs e)//Apply
        {
            parentForm.hook.Dispose();
            Properties.Settings.Default.saveImageAtAll = checkBox1.Checked;
            Properties.Settings.Default.SaveImagesHere = textBox1.Text;

            Properties.Settings.Default.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
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
            Properties.Settings.Default.saveImageAtAll = checkBox1.Checked;
            Properties.Settings.Default.Save();
        }


        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.SaveImagesHere = textBox1.Text;
            Properties.Settings.Default.Save();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            //RegisterInStartup(checkBox2.Checked);
            Properties.Settings.Default.Save();
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
            Properties.Settings.Default.EditScreenshotAfterCapture = checkBox7.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.CopyLinksToClipboard = checkBox3.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.DisableSoundEffects = checkBox4.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkBox2_CheckedChanged_1(object sender, EventArgs e)
        {
            Properties.Settings.Default.UseHTTPS = checkBox2.Checked;
            Properties.Settings.Default.Save();
        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
            Properties.Settings.Default.PastebinSubjectLine = textBox2.Text;
            Properties.Settings.Default.Save();
        }

        private void neverUpload_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.NeverUpload = neverUpload.Checked;
            Properties.Settings.Default.Save();
            checkBox2.Enabled = !Properties.Settings.Default.NeverUpload;
            checkBox3.Enabled = !Properties.Settings.Default.NeverUpload;
            checkBox5.Enabled = !Properties.Settings.Default.NeverUpload;
            textBox2.Enabled = !Properties.Settings.Default.NeverUpload;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            parentForm.actuallyCloseTheProgram();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.DisableNotifications = checkBox5.Checked;
            Properties.Settings.Default.Save();
            
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.FreezeScreenOnRegionShot = checkBox6.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkBoxUploadToFTP_CheckedChanged(object sender, EventArgs e)
        {
            AlsoFTPTectFilesBox.Enabled = checkBoxUploadToFTP.Checked;
            Properties.Settings.Default.uploadToFTP = checkBoxUploadToFTP.Checked;
            Properties.Settings.Default.Save();

            AlsoFTPTectFilesBox.Enabled = Properties.Settings.Default.uploadToFTP;
            FTPURL.Enabled = Properties.Settings.Default.uploadToFTP;
            FTPUsername.Enabled = Properties.Settings.Default.uploadToFTP;
            FTPpassword.Enabled = Properties.Settings.Default.uploadToFTP;
        }

        private void FTPURL_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.FTPurl = FTPURL.Text;
            Properties.Settings.Default.Save();
        }

        private void FTPUsername_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.FTPusername = FTPUsername.Text;
            Properties.Settings.Default.Save();
        }

        private void FTPpassword_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.FTPpassword = FTPpassword.Text;
            Properties.Settings.Default.Save();
        }

        private void alsoSaveTextFilesBox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AlsoSaveTextFiles = alsoSaveTextFilesBox.Checked;
            Properties.Settings.Default.Save();
        }

        private void AlsoFTPTectFilesBox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AlsoFTPTextFiles = AlsoFTPTectFilesBox.Checked;
            Properties.Settings.Default.Save();
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
            Properties.Settings.Default.Hotkey1 = HotkeyTextBox1.Text;
            Properties.Settings.Default.Save();
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
            Properties.Settings.Default.Hotkey2 = HotkeyTextBox2.Text;
            Properties.Settings.Default.Save();
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
            Properties.Settings.Default.Hotkey3 = HotkeyTextBox3.Text;
            Properties.Settings.Default.Save();
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
            Properties.Settings.Default.Hotkey4 = HotkeyTextBox4.Text;
            Properties.Settings.Default.Save();
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
    }
}
