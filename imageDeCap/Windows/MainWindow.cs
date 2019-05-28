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
    // The code in this whole program reads like a joke. DW tho, it only crashes sometimes. ;)

    public partial class MainWindow : Form
    {
        // Global Variables
        public static string videoFormat = ".mp4";
        public static string VersionNumber = "v1.27";

        List<string> Links = new List<string>();
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

        
        SettingsWindow props;
        bool hKey1Pressed = false;
        bool hKey2Pressed = false;
        bool hKey3Pressed = false;
        bool hKey4Pressed = false;
        public void mainLoop()
        {
            ClipboardHandler.Update();

            if (Program.hotkeysEnabled)
            {
                string hotkey = SettingsWindow.getCurrentHotkey();
                //if(hotkey == null)
                //{
                //    return;
                //}
                //bool PrintScreenWasPressed = (hotkey.Contains("Snapshot"));
                //hotkey = hotkey.Replace("+Snapshot", "");
                //hotkey = hotkey.Replace("Snapshot", "");
                //bool PrintScreenBoundToSomething = Preferences.Hotkey1_PrintScreen || Preferences.Hotkey2_PrintScreen || Preferences.Hotkey3_PrintScreen;
                //if (PrintScreenWasPressed && PrintScreenBoundToSomething)
                //{
                //    try
                //    {
                //        Clipboard.Clear();
                //    }
                //    catch
                //    {
                //
                //    }
                //}
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
                        UploadToImgurBounds(true);
                    }
                    hKey2Pressed = true;
                }
                else if (Preferences.Hotkey3 == hotkey)
                {
                    if (!hKey3Pressed)
                    {
                        UploadToImgurBounds();
                    }
                    hKey3Pressed = true;
                }
                else
                {
                    // no recognized hotkey
                    hKey1Pressed = false;
                    hKey2Pressed = false;
                    hKey3Pressed = false;
                    hKey4Pressed = false;
                }
            }
        }

        List<Bitmap> gEnc = new List<Bitmap>();
        public void StartRecordingGif(bool ForceEdit)
        {
            GifCaptureTimer.Interval = (int)(1000.0f / Preferences.GIFRecordingFramerate);
            FrameTime = 0;
            counter = 0;
            GifCaptureTimer.Enabled = true;
            GifCaptureTimer.Tag = ForceEdit;
            LastTime = DateTime.Now;

            this.topBox.BackColor = Color.Green;
            this.bottomBox.BackColor = Color.Green;
            this.leftBox.BackColor = Color.Green;
            this.rightBox.BackColor = Color.Green;

            ruleOfThirdsBox1.Hide();
            ruleOfThirdsBox2.Hide();
            ruleOfThirdsBox3.Hide();
            ruleOfThirdsBox4.Hide();

        }
        public void StopRecordingGif(CompleteCover cover, bool abort)
        {
            if(GifCaptureTimer.Enabled)
            {
                FrameTime /= counter;
                GifCaptureTimer.Enabled = false;
                topBox.Hide();
                bottomBox.Hide();
                leftBox.Hide();
                rightBox.Hide();

                ruleOfThirdsBox1.Hide();
                ruleOfThirdsBox2.Hide();
                ruleOfThirdsBox3.Hide();
                ruleOfThirdsBox4.Hide();

                cover.Close();
                if (!abort)
                {
                    Utilities.playSound("snip.wav");
                    
                    // Feed in through the tag weather the user right-clicked to force editor even when it's disabled.
                    UploadImageData(new byte[] { }, filetype.gif, false, (bool)GifCaptureTimer.Tag, gEnc.ToArray());
                }

                Program.ImageDeCap.isTakingSnapshot = false;
                Program.hotkeysEnabled = true;

            }
        }

        int RunningAverageOfFrameTime = 0;
        int FrameTime = 0;
        DateTime LastTime;
        int counter = 0;
        private void GifCaptureTimer_Tick(object sender, EventArgs e)
        {

            TimeSpan DeltaTime = DateTime.Now - LastTime;
            int DeltaTimeInMS = DeltaTime.Seconds * 100 + DeltaTime.Milliseconds;
            FrameTime += DeltaTimeInMS;
            RunningAverageOfFrameTime = FrameTime / (counter+1);
            //Console.WriteLine(1000 / RunningAverageOfFrameTime);

            LastTime = DateTime.Now;

            int width = Program.ImageDeCap.tempWidth + 1;
            int height = Program.ImageDeCap.tempHeight + 1;
            if (width % 2 == 1)
                width = width - 1;
            if (height % 2 == 1)
                height = height - 1;
            // Capture Bitma
            Bitmap b = Program.ImageDeCap.cap.Capture(
                enmScreenCaptureMode.Bounds,
                Program.ImageDeCap.X - 1,
                Program.ImageDeCap.Y - 1,
                width,
                height, true);

            gEnc.Add(b);

            int minutes = counter / 600;
            int seconds = (counter / 10) % 600;
            int csecs = counter % 10;
            CurrentBackCover.SetTimer("Time: " + minutes + ":" + seconds + "." + csecs, "Frames: " + counter, "Memory Usage: " + (gEnc.Count * width * height * 8) / 1000000 + " MB");
            counter++;
        }


        private bool hasWriteAccessToFolder(string folderPath)
        {
            try
            {
                // Attempt to get a list of security permissions from the folder. 
                // This will raise an exception if the path is read only or do not have access to view the permissions. 
                System.Security.AccessControl.DirectorySecurity ds = Directory.GetAccessControl(folderPath);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }
        public static void CreateShortcut(string shortcutLocation, string targetFileLocation)
        {
            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
            IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = "My shortcut description";   // The description of the shortcut
            shortcut.IconLocation = @"c:\myicon.ico";           // The icon of the shortcut
            shortcut.TargetPath = targetFileLocation;                 // The path of the file that will launch when the shortcut is run
            shortcut.Save();                                    // Save the shortcut
        }
        public static void AddToStartup()
        {
            string startupPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\imageDeCap.lnk";
            MainWindow.CreateShortcut(startupPath, MainWindow.ExeDirectory + @"\imageDeCap.exe");
        }
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
                if(!hasWriteAccessToFolder(ExeDirectory))
                {
                    MessageBox.Show("Insufficient permissions to write settings, try starting the program from somewhere else or start it as as administrator.", "Could not write settings.", MessageBoxButtons.OK);
                    ActuallyCloseTheProgram();
                    return;
                }
                LinksFilePath = ExeDirectory + @"\imageDecapLinks.ini";
                PreferencesPath = ExeDirectory + @"\ImageDeCap.ini";
                Portable = true;
            }

            if(Portable)
            {
                BackupDirectory = MainWindow.AppdataDirectory + @"\Backup";
            }
            else
            {
                BackupDirectory = MainWindow.AppdataDirectory + @"\Backup";
            }

            Preferences.Load();
            if(Preferences.FirstStartup)
            {
                Preferences.FirstStartup = false;
                if(Portable)
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

                MainWindow.AddToStartup();
            }
            else
            {
                this.Hide();
                this.ShowInTaskbar = false;
            }


            props = new SettingsWindow(this);
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

        bool pushThroughCancel = false;
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
            UploadToImgurBounds();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            UploadImgurWindow();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            UploadToImgurBounds(true);
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

        CompleteCover CurrentBackCover;
        public Magnifier magn;
        public bool isTakingSnapshot = false;
        private void UploadToImgurBounds(bool isGif = false)
        {
            Bitmap background = cap.Capture(enmScreenCaptureMode.Screen);
            // prevent blackening
            if (!isTakingSnapshot)
            {
                isTakingSnapshot = true;
                Program.hotkeysEnabled = false;
                //back cover used for pulling cursor position into updateSelectedArea()
                CurrentBackCover = new CompleteCover(isGif);
                CurrentBackCover.Show();
                CurrentBackCover.AfterShow(background);

                magn = new Magnifier(isGif);
                magn.Show();
                magn.TopMost = true;

                setBox(topBox, true);
                setBox(leftBox, true);
                setBox(bottomBox, true);
                setBox(rightBox, true);
                
                setBox(ruleOfThirdsBox1, false);
                setBox(ruleOfThirdsBox2, false);
                setBox(ruleOfThirdsBox3, false);
                setBox(ruleOfThirdsBox4, false);

                Program.ImageDeCap.tempWidth = 0;
                Program.ImageDeCap.tempHeight = 0;
            }
        }

        private void UploadImgurWindow()
        {
            if (!isTakingSnapshot)
            {
                uploadImgur(enmScreenCaptureMode.Window);
            }
        }
        private void UploadImgurScreen()
        {
            if (!isTakingSnapshot)
            {
                uploadImgur(enmScreenCaptureMode.Screen);
            }
        }
        private void uploadImgur(enmScreenCaptureMode mode)
        {
            Utilities.playSound("snip.wav");
            Bitmap result = cap.Capture(mode);
            UploadImageData(CompleteCover.GetBytes(result, System.Drawing.Imaging.ImageFormat.Png), filetype.png);
        }

        public enum filetype
        {
            jpg,
            png,
            bmp,
            gif,
            error,
        }

        public filetype getImageType(string filepath)
        {
            filepath = filepath.ToLower();
            if (filepath.EndsWith(".jpg") || filepath.EndsWith(".jpeg"))
            {
                return filetype.jpg;
            }
            else if (filepath.EndsWith(".png"))
            {
                return filetype.png;
            }
            else if (filepath.EndsWith(".bmp"))
            {
                return filetype.bmp;
            }
            else if (filepath.EndsWith(".gif") || filepath.EndsWith(MainWindow.videoFormat))
            {
                return filetype.gif;
            }
            else
            {
                return filetype.error;
            }
        }

        public void UploadImageFile(string filepath)
        {
            UploadImageData(File.ReadAllBytes(filepath), getImageType(filepath), true);
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

        public void UploadImageData(byte[] FileData, filetype imageType, bool ForceNoEdit = false, bool RMBClickForceEdit = false, Bitmap[] GifImage = null)
        {
            if (imageType == filetype.error)
            {
                return;
            }
            Program.hotkeysEnabled = true; // Enable hotkeys here again so you can kill the editor by starting a new capture.

            //Clipboard.GetData()
            //DataFormats.Format myFormat = DataFormats.GetFormat("myFormat");
            if (imageType != filetype.gif)
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
                if (imageType == filetype.gif)
                {
                    GifEditor editor = new GifEditor(GifImage, topBox.Location.X, topBox.Location.Y, 1000 / FrameTime);
                    editor.Show();
                    editor.FormClosed += EditorDone;
                }
                else
                {
                    NewImageEditor editor = new NewImageEditor(FileData, topBox.Location.X, topBox.Location.Y);
                    editor.Show();
                    editor.FormClosed += EditorDone;
                }
            }
            else
            {
                if (imageType == filetype.gif)
                {
                    FileData = GifEditor.VideoFromFrames(GifImage, 1000 / FrameTime);
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
            filetype f;
            NewImageEditor.EditorResult EditorResult = NewImageEditor.EditorResult.Quit;
            byte[] FileData;
            if (sender is NewImageEditor)
            {
                NewImageEditor editor = (NewImageEditor)sender;
                (EditorResult, FileData) = editor.FinalFunction();
                editor.Dispose();
                f = filetype.png;
            }
            else
            {
                GifEditor editor = (GifEditor)sender;
                (EditorResult, FileData) = editor.FinalFunction();
                editor.Dispose();
                f = filetype.gif;
            }
            UploadImageData_AfterEdit(EditorResult, FileData, f);
        }

        public void UploadImageData_AfterEdit(NewImageEditor.EditorResult EditorResult, byte[] FileData, filetype imageType)
        {
            foreach (var v in gEnc) { v.Dispose(); }
            gEnc.Clear();

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
                    DirectoryInfo di = Directory.CreateDirectory(directory_path);
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

            bool wat = Preferences.BackupImages;
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
                if (imageType == filetype.gif) // If it's a gif
                {
                    FileDialog(MainWindow.videoFormat, FileData);
                }
            }
            if (imageType != filetype.gif) // If it's an image, ask to save no matter what
            {
                if (EditorResult == NewImageEditor.EditorResult.Save)
                {
                    FileDialog(".png", FileData);
                }
            }
             // Copy image to clipboard if it's not a gif
            if (imageType != filetype.gif)
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
                    if (imageType == filetype.gif)
                    {
                        if (Preferences.GifTarget == "gfycat")
                        {
                            bw.DoWork += cap.UploadGif_Gfycat;
                        }
                        else if (Preferences.GifTarget == "imgur")
                        {
                            bw.DoWork += cap.UploadGif_Imgur;
                        }
                        else if(Preferences.GifTarget == "webmshare")
                        {
                            bw.DoWork += cap.UploadGif_Webmshare;
                        }
                    }
                    else
                    {
                        bw.DoWork += cap.UploadImage_Imgur;
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
                ClipboardHandler.setClipboard(url);
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
                ClipboardHandler.setClipboard(url);
                addToLinks(url);
            }

            if (Preferences.uploadToFTP)
            {
                string name = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += cap.uploadToFTP;
                bw.RunWorkerAsync(new object[] {    Preferences.FTPurl,
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
                bw.DoWork += cap.UploadPastebin;
                bw.RunWorkerCompleted += uploadPastebinCompleted;
                bw.RunWorkerAsync(text);

                BackgroundWorker bw2 = new BackgroundWorker();
                bw2.DoWork += cap.uploadToFTP;
                string name = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
                bw2.RunWorkerAsync(new object[] {    Preferences.FTPurl,
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
                ClipboardHandler.setClipboard(pasteBinResult);
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

        private void setBox(ScreenshotRegionLine box, bool grey)
        {
            box.Show();
            box.ShowInTaskbar = false;
            //box.BackColor = Color.Red;
            if(grey)
                box.BackColor = Color.Red;
            else
                box.BackColor = Color.Gray;
            box.Opacity = 0.5;
            box.SetBounds(0, 0, 0, 0);
            box.TopMost = true;
        }
        
        public ScreenCapturer cap = new ScreenCapturer();

        public ScreenshotRegionLine topBox = new ScreenshotRegionLine();
        public ScreenshotRegionLine bottomBox = new ScreenshotRegionLine();
        public ScreenshotRegionLine leftBox = new ScreenshotRegionLine();
        public ScreenshotRegionLine rightBox = new ScreenshotRegionLine();

        public ScreenshotRegionLine ruleOfThirdsBox1 = new ScreenshotRegionLine(true);
        public ScreenshotRegionLine ruleOfThirdsBox2 = new ScreenshotRegionLine(true);
        public ScreenshotRegionLine ruleOfThirdsBox3 = new ScreenshotRegionLine(true);
        public ScreenshotRegionLine ruleOfThirdsBox4 = new ScreenshotRegionLine(true);

        public int X = 0;
        public int Y = 0;
        int tempX = 0;
        int tempY = 0;
        public int tempWidth = 0;
        public int tempHeight = 0;
        int LastCursorX = 0;
        int LastCursorY = 0;
        public void updateSelectedArea(CompleteCover backCover, bool EnterPressed, bool EscapePressed, bool LmbDown, bool LmbUp, bool Lmb, bool Gif, bool RMB, bool HoldingAlt) // this thing is essentially a fucking frame-loop.
        {

            backCover.Activate();
            magn.Bounds = new Rectangle(Cursor.Position.X + 32, Cursor.Position.Y - 32, 124, 124);
            
                                                                                
            if (LmbUp) // keyUp
            {

                //* This should be a function to call for what happens when we have aquired a region.
                backCover.CompletedSelection(RMB);
                // End function thing
            }

            if (LmbDown)
            {
                tempX = Cursor.Position.X;
                tempY = Cursor.Position.Y;
            }
            
            // Holding M1
            if (Lmb)
            {
                topBox.SetBounds(X - 3, Y - 3, tempWidth + 3, 0);
                leftBox.SetBounds(X - 3, Y - 1, 0, tempHeight + 1);
                bottomBox.SetBounds(X - 3, tempHeight + Y, tempWidth + 5, 0);
                rightBox.SetBounds(tempWidth + X, Y - 3, 0, tempHeight + 3);

                if(Preferences.UseRuleOfThirds)
                {
                    ruleOfThirdsBox1.SetBounds(X + (tempWidth / 3), Y, 0, tempHeight);
                    ruleOfThirdsBox2.SetBounds(X + (tempWidth / 3) * 2, Y, 0, tempHeight);
                    ruleOfThirdsBox3.SetBounds(X, Y + (tempHeight / 3), tempWidth, 0);
                    ruleOfThirdsBox4.SetBounds(X, Y + (tempHeight / 3) * 2, tempWidth, 0);
                }

                tempWidth = Math.Abs(Cursor.Position.X - tempX);
                tempHeight = Math.Abs(Cursor.Position.Y - tempY);

                X = tempX;
                Y = tempY;

                if ((Cursor.Position.Y - tempY) < 0)
                    Y = tempY + (Cursor.Position.Y - tempY);

                if ((Cursor.Position.X - tempX) < 0)
                    X = tempX + (Cursor.Position.X - tempX);

                if (HoldingAlt)
                {
                    tempX += Cursor.Position.X - LastCursorX;
                    tempY += Cursor.Position.Y - LastCursorY;
                }
                LastCursorX = Cursor.Position.X;
                LastCursorY = Cursor.Position.Y;
            }
            if (EscapePressed)
            {
                magn.Close();
                backCover.Close();

                topBox.Hide();
                bottomBox.Hide();
                leftBox.Hide();
                rightBox.Hide();

                ruleOfThirdsBox1.Hide();
                ruleOfThirdsBox2.Hide();
                ruleOfThirdsBox3.Hide();
                ruleOfThirdsBox4.Hide();

                isTakingSnapshot = false;
                Program.hotkeysEnabled = true;
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
                props = new SettingsWindow(this);
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

        AboutWindow about;
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(about?.Visible == true)
            {

            }
            else
            {
                about = new AboutWindow();
                about.Show();
            }
        }

        private void clearLinksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("This will clear all links with no undo.", "Warning", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Links.Clear();
                listBox1.DataSource = null;
                listBox1.DataSource = Links;
                File.Delete(LinksFilePath);
            }
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.RightCtrl) || System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftCtrl)) &&
                (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.C) || System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.Insert)))
            {
                Clipboard.SetText(Links[listBox1.SelectedIndex]);
            }
            else if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.Delete))
            {
                Links.RemoveAt(listBox1.SelectedIndex);
                listBox1.DataSource = null;
                listBox1.DataSource = Links;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            VersionLabel.Text = MainWindow.VersionNumber;
        }

        private void BindPrintscreenTimer_Tick(object sender, EventArgs e)
        {
        }
    }
}
