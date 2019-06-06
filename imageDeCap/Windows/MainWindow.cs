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
    
    // Forms:
    // CompleteCover.cs Handles freezing the screen, firing up the editors, recording gifs etc.



    public partial class MainWindow : Form
    {
        // Global Variables
        AboutWindow about;
        SettingsWindow props;
        public CompleteCover CurrentBackCover;

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
        public bool IsTakingSnapshot = false;

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

        private void AddToLinks(string link, bool addToXML = true)
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
                        UploadPastebinClipboard();
                    }
                    hKey1Pressed = true;
                }
                else if (Preferences.Hotkey2 == hotkey)
                {
                    if (!hKey2Pressed)
                    {
                        CaptureScreenRegion(true);
                    }
                    hKey2Pressed = true;
                }
                else if (Preferences.Hotkey3 == hotkey)
                {
                    if (!hKey3Pressed)
                    {
                        CaptureScreenRegion();
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




        // UPLOADING FUNCTIONS

        private void UploadPastebinClipboard()
        {
            if (!IsTakingSnapshot)
            {
                SendKeys.SendWait("^c");
                System.Threading.Thread.Sleep(500);
                string clipboard = Clipboard.GetText();

                if (!Preferences.NeverUpload)
                {
                    Utilities.playSound("snip.wav");
                    BackgroundWorker bw = new BackgroundWorker();
                    bw.DoWork += Uploading.UploadPastebin;
                    bw.RunWorkerCompleted += UploadPastebinCompleted;
                    bw.RunWorkerAsync(clipboard);

                    BackgroundWorker bw2 = new BackgroundWorker();
                    bw2.DoWork += Uploading.UploadToFTP;
                    string name = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
                    bw2.RunWorkerAsync(new object[] { Preferences.FTPurl,
                                                  Preferences.FTPusername,
                                                  Preferences.FTPpassword,
                                                  Encoding.ASCII.GetBytes(clipboard),
                                                  name + ".txt" });
                }

                if (Preferences.saveImageAtAll && Directory.Exists(Preferences.SaveImagesHere))
                {
                    string name = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
                    string whereToSave = Preferences.SaveImagesHere + @"\" + name + ".txt";
                    File.WriteAllText(whereToSave, clipboard);
                }
            }
        }

        private void CaptureScreenRegion(bool isGif = false)
        {
            Bitmap background = ScreenCapturer.Capture(ScreenCaptureMode.Screen);
            // prevent blackening
            if (!IsTakingSnapshot)
            {
                IsTakingSnapshot = true;
                Program.hotkeysEnabled = false;
                // back cover used for pulling cursor position into updateSelectedArea()
                if (CurrentBackCover != null)
                    CurrentBackCover.Dispose();

                CurrentBackCover = new CompleteCover(isGif);
                CurrentBackCover.Show();
                CurrentBackCover.AfterShow(background, isGif);
            }
        }

        public void UploadImageData(byte[] FileData, Filetype imageType, bool ForceNoEdit = false, bool RMBClickForceEdit = false, Bitmap[] GifImage = null)
        {
            if (imageType == Filetype.error)
                return;

            Program.hotkeysEnabled = true; // Enable hotkeys here again so you can kill the editor by starting a new capture.
            
            if (imageType != Filetype.gif)
            {
                if (Preferences.CopyImageToClipboard)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        try
                        {
                            Clipboard.SetImage(Image.FromStream(new MemoryStream(FileData)));
                            break;
                        }
                        catch (ExternalException) // Requested clipboard operation did not succeed
                        {
                            Thread.Sleep(100);
                        }
                    }
                }
            }

            if ((Preferences.EditScreenshotAfterCapture || RMBClickForceEdit) && ForceNoEdit == false)
            {
                if (imageType == Filetype.gif)
                {
                    GifEditor editor = new GifEditor(GifImage, CurrentBackCover.topBox.Location.X, CurrentBackCover.topBox.Location.Y, 1000 / CurrentBackCover.FrameTime);
                    editor.Show();
                    editor.FormClosed += EditorDone;
                }
                else
                {
                    NewImageEditor editor = new NewImageEditor(FileData, CurrentBackCover.topBox.Location.X, CurrentBackCover.topBox.Location.Y);
                    editor.Show();
                    editor.FormClosed += EditorDone;
                }
            }
            else
            {
                if (imageType == Filetype.gif)
                {
                    FileData = GifEditor.VideoFromFrames(GifImage, 1000 / CurrentBackCover.FrameTime);
                    UploadImageData_AfterEdit(NewImageEditor.EditorResult.Upload, FileData, imageType);
                }
                else
                {
                    UploadImageData_AfterEdit(NewImageEditor.EditorResult.Upload, FileData, imageType);
                }
            }
        }

        public void EditorDone(object sender, EventArgs e)
        {
            Filetype f;
            NewImageEditor.EditorResult EditorResult = NewImageEditor.EditorResult.Quit;
            byte[] FileData;
            if (sender is NewImageEditor)
            {
                NewImageEditor editor = (NewImageEditor)sender;
                (EditorResult, FileData) = editor.FinalFunction();
                editor.Dispose();
                f = Filetype.png;
            }
            else
            {
                GifEditor editor = (GifEditor)sender;
                (EditorResult, FileData) = editor.FinalFunction();
                editor.Dispose();
                f = Filetype.gif;
            }
            UploadImageData_AfterEdit(EditorResult, FileData, f);
        }

        public void UploadImageData_AfterEdit(NewImageEditor.EditorResult EditorResult, byte[] FileData, Filetype imageType)
        {
            foreach (var v in CurrentBackCover.gEnc) { v.Dispose(); }
            CurrentBackCover.gEnc.Clear();

            string SaveFileName = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
            string Extension = imageType.ToString();
            if (Extension == "gif")
            {
                Extension = videoFormat.Replace(".", "");
            }

            if (Preferences.saveImageAtAll)
            {
                string directory_path = Path.GetFullPath(Environment.ExpandEnvironmentVariables(Preferences.SaveImagesHere));
                string file_path = Path.Combine(directory_path, SaveFileName + "." + Extension);
                try
                {
                    Directory.CreateDirectory(directory_path);
                    try
                    {
                        File.WriteAllBytes(file_path, FileData);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Failed to create the file {file_path}. Exception: {e.Message}");
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Failed create the directory {directory_path}. Exception: {e.Message}");
                }
            }
            
            if (Preferences.BackupImages)
            {
                if(!Directory.Exists(BackupDirectory))
                {
                    Directory.CreateDirectory(BackupDirectory);
                }
                File.WriteAllBytes(BackupDirectory + @"\" + SaveFileName + "." + Extension, FileData);
                int i = 0;
                foreach(string file in Directory.GetFiles(BackupDirectory).OrderBy(f => f).Reverse())
                {
                    i++;
                    if(i > 100)
                    {
                        File.Delete(file);
                    }
                }
            }

            if (EditorResult == NewImageEditor.EditorResult.Quit)
            {
                return;
            }
            
            if (EditorResult == NewImageEditor.EditorResult.Save) // If gif, ask to save only if 
            {
                if (imageType == Filetype.gif) // If it's a gif
                {
                    Utilities.FileDialog(MainWindow.videoFormat, FileData);
                }
            }

            if (imageType != Filetype.gif) // If it's an image, ask to save no matter what
            {
                if (EditorResult == NewImageEditor.EditorResult.Save)
                {
                    Utilities.FileDialog(".png", FileData);
                }
            }

            // Copy image to clipboard if it's not a gif
            if (imageType != Filetype.gif)
            {
                if (EditorResult == NewImageEditor.EditorResult.Clipboard)
                {
                    if (Preferences.CopyImageToClipboard)
                    {
                        Image bitmapImage = Image.FromStream(new MemoryStream(FileData));
                        Clipboard.SetImage(bitmapImage);
                        bitmapImage.Dispose();
                    }
                }
            }

            // Upload the image
            if (EditorResult == NewImageEditor.EditorResult.Upload)
            {
                if (!Preferences.NeverUpload)
                {
                    BackgroundWorker bw = new BackgroundWorker();
                    if (imageType == Filetype.gif)
                    {
                        if (Preferences.GifTarget == "gfycat")
                        {
                            bw.DoWork += Uploading.UploadGif_Gfycat;
                        }
                        else if (Preferences.GifTarget == "imgur")
                        {
                            bw.DoWork += Uploading.UploadGif_Imgur;
                        }
                        else if(Preferences.GifTarget == "webmshare")
                        {
                            bw.DoWork += Uploading.UploadGif_Webmshare;
                        }
                    }
                    else
                    {
                        bw.DoWork += Uploading.UploadImage_Imgur;
                    }
                    bw.RunWorkerCompleted += UploadImageFileCompleted;
                    bw.RunWorkerAsync(FileData);
                }
            }
        }
        
        private void UploadImageFileCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            (string url, byte[] ImageData) = (ValueTuple<string, byte[]>)e.Result;

            if (url.Contains("failed"))
            {
                ClipboardHandler.SetClipboard(url);
                Utilities.BubbleNotification($"Upload to imgur failed! \n{url}\nAre you connected to the internet? \nis Imgur Down?", null, ToolTipIcon.Error);
                Utilities.playSound("error.wav");
            }
            else
            {
                if(Preferences.OpenInBrowser)
                {
                    Process.Start(url);
                }
                if (!Preferences.CopyLinksToClipboard)
                {
                    if (Preferences.DisableNotifications)
                        Utilities.BubbleNotification("Imgur URL copied to clipboard!", BalloonTipClicked);
                }
                else
                {
                    if (!Preferences.DisableNotifications)
                        Utilities.BubbleNotification("Upload Complete!", BalloonTipClicked);
                }
                
                if (!Utilities.IsWindows10() || Preferences.DisableNotifications)
                {
                    Utilities.playSound("upload.wav");
                }
                ClipboardHandler.SetClipboard(url);
                AddToLinks(url);
            }

            if (Preferences.uploadToFTP)
            {
                string name = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += Uploading.UploadToFTP;
                bw.RunWorkerAsync(new object[] { Preferences.FTPurl,
                                                 Preferences.FTPusername,
                                                 Preferences.FTPpassword,
                                                 ImageData,
                                                 name + (url.EndsWith(".png") ? ".png" : ".jpg") });
            }
        }
        
        private void UploadPastebinCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string pasteBinResult = (string)e.Result;
            if (!pasteBinResult.Contains("failed"))
            {
                ClipboardHandler.SetClipboard(pasteBinResult);
                if (Preferences.CopyLinksToClipboard)
                {
                    if (!Preferences.DisableNotifications)
                        Utilities.BubbleNotification("Pastebin link placed in clipboard!", BalloonTipClicked);
                }
                else
                {
                    if (!Preferences.DisableNotifications)
                        Utilities.BubbleNotification("Upload complete!", BalloonTipClicked);
                }

                if (!Utilities.IsWindows10() || Preferences.DisableNotifications)
                {
                    Utilities.playSound("upload.wav");
                }
                AddToLinks(pasteBinResult);
            }
            else
            {
                Utilities.BubbleNotification($"upload to pastebin failed!\n{pasteBinResult}\nAre you connected to the internet? \nIs pastebin Down?", null, ToolTipIcon.Error);
                Utilities.playSound("error.wav");
            }
        }






        // Direct UI Stuff

        private void ListBox1_DoubleClick(object sender, EventArgs e)
        {
            if (Links.Count > 0 && Links[listBox1.SelectedIndex].StartsWith("http"))
                Process.Start(Links[listBox1.SelectedIndex]);
        }

        private void BalloonTipClicked(object sender, EventArgs e)
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
                UploadImageData(File.ReadAllBytes(files[0]), Utilities.GetImageType(files[0]), true);
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
            CaptureScreenRegion();
        }

        private void RecordGif_Click(object sender, EventArgs e)
        {
            CaptureScreenRegion(true);
        }

        private void UploadText_Click(object sender = null, EventArgs e = null)
        {
            UploadPastebinClipboard();
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
