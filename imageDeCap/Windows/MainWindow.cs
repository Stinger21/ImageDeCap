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
        bool pushThroughCancel = false;
        public bool isTakingSnapshot = false;

        public MainWindow()
        {
            InitializeComponent();
            this.VersionLabel.Text = MainWindow.VersionNumber;
            bool Portable = false;
            ExeDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            AppdataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\imageDeCap";
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
                // If it's not in Program Files that means the user is trying to use it portable and we should write settings next to the exe.

                // If we can't write settings, tell the user they need to run the program as administrator to proceed.
                if (!Utilities.HasWriteAccessToFolder(ExeDirectory))
                {
                    MessageBox.Show("Insufficient permissions to write settings, try starting the program from somewhere else or start it as as administrator.", "Could not write settings.", MessageBoxButtons.OK);
                    ActuallyCloseTheProgram();
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
                    addToLinks(s, false);
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

        private void addToLinks(string link, bool addToXML = true)
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

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (Links.Count > 0 && Links[listBox1.SelectedIndex].StartsWith("http"))
                    Process.Start(Links[listBox1.SelectedIndex]);
        }

        private void BalloonTipClicked(object sender, EventArgs e)
        {
            if (Links.Count > 0 && Links[Links.Count - 1].StartsWith("http"))
                    Process.Start(Links[Links.Count - 1]);
        }
        
        // gamedev much?
        public void MainLoop()
        {
            ClipboardHandler.Update();

            if (Program.hotkeysEnabled)
            {
                string hotkey = SettingsWindow.getCurrentHotkey();

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
                        UploadToImageOrGifBounds(true);
                    }
                    hKey2Pressed = true;
                }
                else if (Preferences.Hotkey3 == hotkey)
                {
                    if (!hKey3Pressed)
                    {
                        UploadToImageOrGifBounds();
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

        //public void StartRecordingGif(bool ForceEdit)
        //{
        //    GifCaptureTimer.Interval = (int)(1000.0f / Preferences.GIFRecordingFramerate);
        //    FrameTime = 0;
        //    counter = 0;
        //    GifCaptureTimer.Enabled = true;
        //    GifCaptureTimer.Tag = ForceEdit;
        //    LastTime = DateTime.Now;
        //
        //    CurrentBackCover.topBox.BackColor = Color.Green;
        //    CurrentBackCover.bottomBox.BackColor = Color.Green;
        //    CurrentBackCover.leftBox.BackColor = Color.Green;
        //    CurrentBackCover.rightBox.BackColor = Color.Green;
        //
        //    CurrentBackCover.ruleOfThirdsBox1.Hide();
        //    CurrentBackCover.ruleOfThirdsBox2.Hide();
        //    CurrentBackCover.ruleOfThirdsBox3.Hide();
        //    CurrentBackCover.ruleOfThirdsBox4.Hide();
        //}
        //
        //public void StopRecordingGif(CompleteCover cover, bool abort)
        //{
        //    if(GifCaptureTimer.Enabled)
        //    {
        //        FrameTime /= counter;
        //        GifCaptureTimer.Enabled = false;
        //        CurrentBackCover.topBox.Hide();
        //        CurrentBackCover.bottomBox.Hide();
        //        CurrentBackCover.leftBox.Hide();
        //        CurrentBackCover.rightBox.Hide();
        //
        //        CurrentBackCover.ruleOfThirdsBox1.Hide();
        //        CurrentBackCover.ruleOfThirdsBox2.Hide();
        //        CurrentBackCover.ruleOfThirdsBox3.Hide();
        //        CurrentBackCover.ruleOfThirdsBox4.Hide();
        //
        //        cover.Close();
        //        if (!abort)
        //        {
        //            Utilities.playSound("snip.wav");
        //            
        //            // Feed in through the tag weather the user right-clicked to force editor even when it's disabled.
        //            UploadImageData(new byte[] { }, Filetype.gif, false, (bool)GifCaptureTimer.Tag, gEnc.ToArray());
        //        }
        //
        //        Program.ImageDeCap.isTakingSnapshot = false;
        //        Program.hotkeysEnabled = true;
        //    }
        //}
        //
        //private void GifCaptureTimer_Tick(object sender, EventArgs e)
        //{
        //    TimeSpan DeltaTime = DateTime.Now - LastTime;
        //    int DeltaTimeInMS = DeltaTime.Seconds * 100 + DeltaTime.Milliseconds;
        //    FrameTime += DeltaTimeInMS;
        //    RunningAverageOfFrameTime = FrameTime / (counter+1);
        //
        //    LastTime = DateTime.Now;
        //
        //    int width   = CurrentBackCover.tempWidth + 1;
        //    int height  = CurrentBackCover.tempHeight + 1;
        //    if (width % 2 == 1)
        //        width = width - 1;
        //    if (height % 2 == 1)
        //        height = height - 1;
        //    // Capture Bitmap
        //    Bitmap b = ScreenCapturer.Capture(
        //        ScreenCaptureMode.Bounds,
        //        CurrentBackCover.X - 1,
        //        CurrentBackCover.Y - 1,
        //        width,
        //        height, true);
        //
        //    gEnc.Add(b);
        //
        //    int minutes = counter / 600;
        //    int seconds = (counter / 10) % 600;
        //    int csecs = counter % 10;
        //    CurrentBackCover.SetTimer("Time: " + minutes + ":" + seconds + "." + csecs, "Frames: " + counter, "Memory Usage: " + (gEnc.Count * width * height * 8) / 1000000 + " MB");
        //    counter++;
        //}
        
        public static void AddToStartup()
        {
            string startupPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\imageDeCap.lnk";
            Utilities.CreateShortcut(startupPath, MainWindow.ExeDirectory + @"\imageDeCap.exe");
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
                UploadImageFile(files[0]);
            }
        }

        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (!pushThroughCancel)
                {
                    e.Cancel = true;
                }
                this.Hide();
                this.ShowInTaskbar = false;
            }
        }

        public void OpenWindow()
        {
            this.ShowInTaskbar = true;
            this.Show();
            this.Activate();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)//Double Click notifyIcon
        {
            OpenWindow();
        }
        
        private void UploadToPasteBin(object sender = null, EventArgs e = null)
        {
            UploadPastebinClipboard();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            UploadToImageOrGifBounds();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            UploadImageOrGifWindow();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UploadToImageOrGifBounds(true);
        }

        public void ActuallyCloseTheProgram()
        {
            pushThroughCancel = true;
            props.Close();
            this.Close();
            Application.Exit();
            Program.Quit = true;
            Environment.Exit(0);
        }
        
        private void UploadPastebinClipboard()
        {
            if (!isTakingSnapshot)
            {
                SendKeys.SendWait("^c");
                System.Threading.Thread.Sleep(500);
                string clipboard = Clipboard.GetText();
                uploadPastebin(clipboard);
            }
        }

        private void UploadToImageOrGifBounds(bool isGif = false)
        {
            Bitmap background = ScreenCapturer.Capture(ScreenCaptureMode.Screen);
            // prevent blackening
            if (!isTakingSnapshot)
            {
                isTakingSnapshot = true;
                Program.hotkeysEnabled = false;
                // back cover used for pulling cursor position into updateSelectedArea()
                if (CurrentBackCover != null)
                    CurrentBackCover.Dispose();

                CurrentBackCover = new CompleteCover(isGif);
                CurrentBackCover.Show();
                CurrentBackCover.AfterShow(background, isGif);
            }
        }

        private void UploadImageOrGifWindow()
        {
            if (!isTakingSnapshot)
            {
                uploadImgur(ScreenCaptureMode.Window);
            }
        }

        private void UploadImgurScreen()
        {
            if (!isTakingSnapshot)
            {
                uploadImgur(ScreenCaptureMode.Screen);
            }
        }

        private void uploadImgur(ScreenCaptureMode mode)
        {
            Utilities.playSound("snip.wav");
            Bitmap result = ScreenCapturer.Capture(mode);
            UploadImageData(CompleteCover.GetBytes(result, System.Drawing.Imaging.ImageFormat.Png), Filetype.png);
        }
        
        public void UploadImageFile(string filepath)
        {
            UploadImageData(File.ReadAllBytes(filepath), Utilities.GetImageType(filepath), true);
        }

        private void FileDialog(string extension, byte[] data)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = extension+" files (*"+ extension + ")|*"+ extension;
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(saveFileDialog1.FileName, data);
            }
        }

        public void UploadImageData(byte[] FileData, Filetype imageType, bool ForceNoEdit = false, bool RMBClickForceEdit = false, Bitmap[] GifImage = null)
        {
            if (imageType == Filetype.error)
            {
                return;
            }
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
                    FileDialog(MainWindow.videoFormat, FileData);
                }
            }

            if (imageType != Filetype.gif) // If it's an image, ask to save no matter what
            {
                if (EditorResult == NewImageEditor.EditorResult.Save)
                {
                    FileDialog(".png", FileData);
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
                    bw.RunWorkerCompleted += uploadImageFileCompleted;
                    bw.RunWorkerAsync(FileData);
                }
            }
        }
        
        private void uploadImageFileCompleted(object sender, RunWorkerCompletedEventArgs e)
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
                    //means it's probably windows 10, in which case we should not play the noise as windows 10 plays a fucking noise of its own no matter what. :|
                    Utilities.playSound("upload.wav");
                }
                ClipboardHandler.SetClipboard(url);
                addToLinks(url);
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

        private void uploadPastebin(string text)
        {
            if (!Preferences.NeverUpload)
            {
                Utilities.playSound("snip.wav");
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += Uploading.UploadPastebin;
                bw.RunWorkerCompleted += uploadPastebinCompleted;
                bw.RunWorkerAsync(text);

                BackgroundWorker bw2 = new BackgroundWorker();
                bw2.DoWork += Uploading.UploadToFTP;
                string name = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
                bw2.RunWorkerAsync(new object[] {   Preferences.FTPurl,
                                                    Preferences.FTPusername,
                                                    Preferences.FTPpassword,
                                                    Encoding.ASCII.GetBytes(text),
                                                    name + ".txt" });

            }
            if (Preferences.saveImageAtAll && Directory.Exists(Preferences.SaveImagesHere))
            {
                string name = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
                string whereToSave = Preferences.SaveImagesHere + @"\" + name + ".txt";
                File.WriteAllText(whereToSave, text);
            }
        }

        private void uploadPastebinCompleted(object sender, RunWorkerCompletedEventArgs e)
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
                    //means it's probably windows 10, in which case we should not play the noise as windows 10 plays a noise on its own no matter what. :|
                    Utilities.playSound("upload.wav");
                }
                addToLinks(pasteBinResult);
            }
            else
            {
                Utilities.BubbleNotification($"upload to pastebin failed!\n{pasteBinResult}\nAre you connected to the internet? \nIs pastebin Down?", null, ToolTipIcon.Error);
                Utilities.playSound("error.wav");
            }
        }


        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowProperties();
        }

        public void ShowProperties()
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActuallyCloseTheProgram();
        }

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            props.Close();
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void clearLinksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will clear all links with no undo.", "Warning", MessageBoxButtons.OKCancel) != DialogResult.OK)
                return;

            Links.Clear();
            listBox1.DataSource = null;
            listBox1.DataSource = Links;
            File.Delete(LinksFilePath);
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
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
