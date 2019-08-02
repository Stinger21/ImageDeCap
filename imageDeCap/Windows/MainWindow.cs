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
    // ScreenCapturer.cs    Contains functions for capturing images off the screen
    // Uploading.cs         Contains functions for uploading bitmaps to websites

    // Hotkeys.cs           [does not exist yet] handles hotkey stuff
    // Preferences.cs       metaprogramming thing for saving preferences to ini
    // Utilities.cs         misc utilities / program-wide functions
    // ClipboardHandler.cs  threadsafe clipboard interaction

    // Forms:
    // CompleteCover.cs Handles freezing the screen, firing up the editors, recording gifs etc.
    
    public partial class MainWindow : Form
    {


        // Global Variables
        AboutWindow about;
        SettingsWindow props;

        List<string> Links = new List<string>();
        public static string videoFormat = ".mp4";
        public static string VersionNumber = "v1.27";

        public static string LinksFilePath = "ERROR";
        public static string PreferencesPath = "ERROR";
        public static string AppdataDirectory = "ERROR";
        public static string ExeDirectory = "ERROR";
        public static string BackupDirectory = "ERROR";
        public void Initialize()
        {
            this.VersionLabel.Text = MainWindow.VersionNumber;
            ExeDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            AppdataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\imageDeCap";

            // If the program is started from a folder containing the name "Program Files"
            // we assume that it's not running in portablemode.
            bool Portable = !ExeDirectory.Contains("Program Files");

            if (Portable)
            {
                // If we can't write settings, tell the user they need to run the program as administrator to proceed.
                if (!Utilities.HasWriteAccessToFolder(ExeDirectory))
                {
                    MessageBox.Show("Insufficient permissions to write settings next to the program executable, try starting the program from somewhere else or start it as as administrator.", "Could not write settings.", MessageBoxButtons.OK);
                    Utilities.CloseProgram();
                    return;
                }
                LinksFilePath = ExeDirectory + @"\imageDecapLinks.ini";
                PreferencesPath = ExeDirectory + @"\ImageDeCap.ini";
            }
            else
            {
                // If it's in an install folder, put the settings files in appdata.
                if (!Directory.Exists(AppdataDirectory))
                    Directory.CreateDirectory(AppdataDirectory);
                LinksFilePath = AppdataDirectory + @"\imageDecapLinks.ini";
                PreferencesPath = AppdataDirectory + @"\ImageDeCap.ini";
            }
            BackupDirectory = AppdataDirectory + @"\Backup";

            Preferences.Load();
            if (Preferences.FirstStartup)
            {

                // do first-startup stuff
                Preferences.FirstStartup = false;
                if (Portable)
                    Preferences.BackupImages = false;
                else
                    Preferences.BackupImages = true;
                Preferences.Save();

                OpenWindow();
                Utilities.BubbleNotification("Press PRINTSCREEN to start!", null, ToolTipIcon.Info, "Welcome to ImageDeCap!");
                Utilities.AddToStartup();

                // Try deleting any old shortcut stuff from 1.23 and earlier
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
            }
            else
            {
                this.Hide();
                this.ShowInTaskbar = false;
            }

            if (File.Exists(LinksFilePath))
            {
                string links = File.ReadAllText(LinksFilePath);
                links = Regex.Replace(links, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
                foreach (string s in links.Split('\n'))
                {
                    if (s == "")
                        continue;
                    AddLink(s, false);
                }
            }
            else
            {
                File.WriteAllText(LinksFilePath, "");
            }

            SystemTrayContextMenu.Initialize();
            BubbleNotification.ContextMenu = SystemTrayContextMenu.IconRightClickMenu;
            BubbleNotification.Visible = true;

            LinksListBox.AllowDrop = true;
            LinksListBox.DragEnter += new DragEventHandler(Form1_DragEnter);
            LinksListBox.DragDrop += new DragEventHandler(Form1_DragDrop);
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        public void AddLink(string link, bool Write = true)
        {
            Links.Add(link);
            LinksListBox.DataSource = null;
            LinksListBox.DataSource = Links;
            try
            {
                LinksListBox.SelectedIndex = LinksListBox.Items.Count - 1;
            }
            catch
            {

            }
            if (Write)
            {
                string links = String.Join("\n", Links);
                File.WriteAllText(LinksFilePath, links);
            }
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
            Hotkeys.Update();
        }

        private void ListBox1_DoubleClick(object sender, EventArgs e)
        {
            if (Links.Count > 0 && Links[LinksListBox.SelectedIndex].StartsWith("http"))
                Process.Start(Links[LinksListBox.SelectedIndex]);
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
            Utilities.CloseProgram();
        }

        private void HideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            props.Close();
            this.Close();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (about?.Visible == true)
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
            LinksListBox.DataSource = null;
            LinksListBox.DataSource = Links;
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
                Clipboard.SetText(Links[LinksListBox.SelectedIndex]);
            }
            else if (Delete)
            {
                if (MessageBox.Show("Delete link?.", "Warning", MessageBoxButtons.OKCancel) != DialogResult.OK)
                    return;

                Links.RemoveAt(LinksListBox.SelectedIndex);
                LinksListBox.DataSource = null;
                LinksListBox.DataSource = Links;
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

        private void button1_Click(object sender, EventArgs e)
        {
        }
    }

    public static class SystemTrayContextMenu
    {
        public static ContextMenu IconRightClickMenu;
        public static MenuItem ExitButton = new MenuItem();
        public static MenuItem OpenWindowButton = new MenuItem();
        public static MenuItem ContactButton = new MenuItem();
        public static MenuItem PreferencesButton = new MenuItem();

        public static void Initialize()
        {
            IconRightClickMenu = new ContextMenu();

            IconRightClickMenu.MenuItems.Add(OpenWindowButton);
            IconRightClickMenu.MenuItems.Add("-");
            IconRightClickMenu.MenuItems.Add(ContactButton);
            IconRightClickMenu.MenuItems.Add(PreferencesButton);
            IconRightClickMenu.MenuItems.Add(ExitButton);

            ExitButton.Text = "Exit";
            ExitButton.Click += new EventHandler(ExitButton_Click);

            ContactButton.Text = "Contact / Bugs";
            ContactButton.Click += new EventHandler(ContactButton_Click);

            PreferencesButton.Text = "Settings";
            PreferencesButton.Click += new EventHandler(Preferences_Click);

            OpenWindowButton.Text = "Open Window";
            OpenWindowButton.Click += new EventHandler(OpenWindowButton_Click);
        }

        private static void ExitButton_Click(object Sender, EventArgs e)// Exit button
        {
            Utilities.CloseProgram();
        }

        private static void ContactButton_Click(object Sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.mattwestphal.com/imagedecap");
        }

        private static void Preferences_Click(object Sender, EventArgs e)
        {
            Program.ImageDeCap.ShowSettings();
        }

        private static void OpenWindowButton_Click(object Sender, EventArgs e)// Open Window
        {
            Program.ImageDeCap.OpenWindow();
        }
    }
}
