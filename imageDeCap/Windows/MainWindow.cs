using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Media;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Diagnostics;
using System.Threading;
using System.Xml.Linq;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.Text.RegularExpressions;

namespace imageDeCap
{
    // The code in this whole program is a mess. DW tho, it only crashes sometimes. ;)


    // Notes about the files and what they are meant for as I am refactoring it, 2019-06-06
    // Static classes:
    // ScreenCapturer.cs Contains functions for capturing images off the screen and functions fr uploading bitmaps to websites
    // Hotkeys.cs [does not exist yet] handles hotkey stuff

    // Forms:
    // CompleteCover.cs Handles freezing the screen, firing up the editors, recording gifs etc.



    public partial class MainWindow : Form
    {
        // Global Variables
        AboutWindow about;
        SettingsWindow props;

        List<string> Links = new List<string>();
        bool hKey1Pressed = false;
        bool hKey2Pressed = false;
        bool hKey3Pressed = false;
        public static string videoFormat = ".mp4";
        public static string VersionNumber = "v1.27";
        public static string LinksFilePath = "ERROR";
        public static string PreferencesPath = "ERROR";
        public static string AppdataDirectory = "ERROR";
        public static string ExeDirectory = "ERROR";
        public static string BackupDirectory = "ERROR";

        public MainWindow()
        {
            InitializeComponent();
            this.VersionLabel.Text = MainWindow.VersionNumber;
            bool Portable = false;
            ExeDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            AppdataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\imageDeCap";
            
            // If the program is started from a folder containing the name "Program Files"
            // we assume that it's not running in portablemode.
            if (ExeDirectory.Contains("Program Files"))
            {
                // If it's in an install folder, put the settings files in appdata.
                if (!Directory.Exists(AppdataDirectory))
                    Directory.CreateDirectory(AppdataDirectory);
                LinksFilePath = AppdataDirectory + @"\imageDecapLinks.ini";
                PreferencesPath = AppdataDirectory + @"\ImageDeCap.ini";
                Portable = false;
            }
            else
            {
                // If we can't write settings, tell the user they need to run the program as administrator to proceed.
                if (!Utilities.HasWriteAccessToFolder(ExeDirectory))
                {
                    MessageBox.Show("Insufficient permissions to write settings next to the program executable, try starting the program from somewhere else or start it as as administrator.", "Could not write settings.", MessageBoxButtons.OK);
                    CloseProgram();
                    return;
                }
                LinksFilePath = ExeDirectory + @"\imageDecapLinks.ini";
                PreferencesPath = ExeDirectory + @"\ImageDeCap.ini";
                Portable = true;
            }
            
            BackupDirectory = AppdataDirectory + @"\Backup";

            Preferences.Load();
            if (Preferences.FirstStartup)
            {
                Preferences.FirstStartup = false;
                if (Portable)
                {
                    Preferences.BackupImages = false;
                    Preferences.Save();
                }
                else
                {
                    Preferences.BackupImages = true;
                    Preferences.Save();
                }
                Preferences.Save();
                OpenWindow();

                // Try deleting any old shortcut memes from 1.23 and earlier
                string startupPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\imageDeCap.lnk";
                string startMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + @"\imageDeCap.lnk";
                string appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\imageDeCap\imageDeCap.exe";

                if (File.Exists(startupPath))
                    File.Delete(startupPath);

                if (File.Exists(startMenuPath))
                    File.Delete(startMenuPath);

                try
                {
                    if (File.Exists(appdataPath))
                        File.Delete(appdataPath);
                }
                catch
                {
                    MessageBox.Show("An old version of ImageDeCap is currently running. Please close it before starting the new one.", "Error");
                    Environment.Exit(0);
                }

                Utilities.BubbleNotification("Press PRINTSCREEN to start!", null, ToolTipIcon.Info, "Welcome to ImageDeCap!");

                AddToStartup();
            }
            else
            {
                this.Hide();
                this.ShowInTaskbar = false;
            }

            props = new SettingsWindow();
            if (File.Exists(LinksFilePath))
            {
                string links = File.ReadAllText(LinksFilePath);
                links = Regex.Replace(links, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
                foreach (string s in links.Split('\n'))
                {
                    if (s == "")
                        continue;
                    AddToLinks(s, false);
                }
            }
            else
            {
                File.WriteAllText(LinksFilePath, "");
            }

            SystemTrayContextMenu.Initialize();
            BubbleNotification.ContextMenu = SystemTrayContextMenu.IconRightClickMenu;
            BubbleNotification.Visible = true;

            listBox1.AllowDrop = true;
            listBox1.DragEnter += new DragEventHandler(Form1_DragEnter);
            listBox1.DragDrop += new DragEventHandler(Form1_DragDrop);
        }

        public void AddToLinks(string link, bool addToXML = true)
        {
            Links.Add(link);
            listBox1.DataSource = null;
            listBox1.DataSource = Links;
            try
            {
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            }
            catch
            {

            }
            if (addToXML)
            {
                string links = String.Join("\n", Links);
                File.WriteAllText(LinksFilePath, links);
            }
        }

        public static void AddToStartup()
        {
            string startupPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\imageDeCap.lnk";
            Utilities.CreateShortcut(startupPath, ExeDirectory + @"\imageDeCap.exe");
        }

        public void CloseProgram()
        {
            Program.Quit = true;
        }

        public void ShowSettings()
        {
            try
            {
                props.Show();
            }
            catch
            {
                props = new SettingsWindow();
            }
            props.Show();
            props.BringToFront();
        }
        public void OpenWindow()
        {
            this.ShowInTaskbar = true;
            this.Show();
            this.Activate();
        }

        public void MainLoop()
        {
            ClipboardHandler.Update();

            if (Program.hotkeysEnabled)
            {
                string hotkey = SettingsWindow.GetCurrentHotkey();

                if (Preferences.Hotkey1 == hotkey)
                {
                    if (!hKey1Pressed)
                    {
                        ScreenCapturer.UploadPastebinClipboard();
                    }
                    hKey1Pressed = true;
                }
                else if (Preferences.Hotkey2 == hotkey)
                {
                    if (!hKey2Pressed)
                    {
                        ScreenCapturer.CaptureScreenRegion(true);
                    }
                    hKey2Pressed = true;
                }
                else if (Preferences.Hotkey3 == hotkey)
                {
                    if (!hKey3Pressed)
                    {
                        ScreenCapturer.CaptureScreenRegion();
                    }
                    hKey3Pressed = true;
                }
                else
                {
                    // no recognized hotkey
                    hKey1Pressed = false;
                    hKey2Pressed = false;
                    hKey3Pressed = false;
                }
            }
        }
















        // Direct UI Stuff

        private void ListBox1_DoubleClick(object sender, EventArgs e)
        {
            if (Links.Count > 0 && Links[listBox1.SelectedIndex].StartsWith("http"))
                Process.Start(Links[listBox1.SelectedIndex]);
        }

        public void BalloonTipClicked(object sender, EventArgs e)
        {
            if (Links.Count > 0 && Links[Links.Count - 1].StartsWith("http"))
                Process.Start(Links[Links.Count - 1]);
        }

        void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0)
            {
                ScreenCapturer.UploadImageData(File.ReadAllBytes(files[0]), Utilities.GetImageType(files[0]), true);
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
                this.ShowInTaskbar = false;
            }
        }

        private void CaptureImage_Click(object sender, EventArgs e)
        {
            ScreenCapturer.CaptureScreenRegion();
        }

        private void RecordGif_Click(object sender, EventArgs e)
        {
            ScreenCapturer.CaptureScreenRegion(true);
        }

        private void UploadText_Click(object sender = null, EventArgs e = null)
        {
            ScreenCapturer.UploadPastebinClipboard();
        }
        
        private void PreferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSettings();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseProgram();
        }

        private void HideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            props.Close();
            this.Close();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(about?.Visible == true)
            {
                about.Activate();
            }
            else
            {
                about = new AboutWindow();
                about.Show();
            }
        }

        private void ClearLinksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will clear all links with no undo.", "Warning", MessageBoxButtons.OKCancel) != DialogResult.OK)
                return;

            Links.Clear();
            listBox1.DataSource = null;
            listBox1.DataSource = Links;
            File.Delete(LinksFilePath);
        }

        private void ListBox1_KeyDown(object sender, KeyEventArgs e)
        {
            bool Copy = false;
            Copy |= System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.RightCtrl) || System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftCtrl);
            Copy |= System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.C) || System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.Insert);

            bool Delete = System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.Delete);

            if (Copy)
            {
                Clipboard.SetText(Links[listBox1.SelectedIndex]);
            }
            else if (Delete)
            {
                if (MessageBox.Show("Delete link?.", "Warning", MessageBoxButtons.OKCancel) != DialogResult.OK)
                    return;

                Links.RemoveAt(listBox1.SelectedIndex);
                listBox1.DataSource = null;
                listBox1.DataSource = Links;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            VersionLabel.Text = MainWindow.VersionNumber;
        }

        private void BubbleNotification_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OpenWindow();
        }
    }
}
