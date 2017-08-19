using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//Me
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
using ImageMagick;
using System.Text.RegularExpressions;

namespace imageDeCap
{
    public partial class Form1 : Form
    {
        string LinksFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\imageDeCap\imageDecapLinks.ini";

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
            if (Links.Count > 0)
                if (Links[listBox1.SelectedIndex].StartsWith("http"))
                    Process.Start(Links[listBox1.SelectedIndex]);
        }

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            if (Links.Count > 0)
                if (Links[listBox1.SelectedIndex].StartsWith("http"))
                    Process.Start(Links[listBox1.SelectedIndex]);
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
                else if (Preferences.Hotkey4 == hotkey)
                {
                    if (!hKey4Pressed)
                    {
                        UploadImgurWindow();
                    }
                    hKey4Pressed = true;
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
        
        MagickImageCollection gEnc;
        public void StartRecordingGif(bool ForceEdit)
        {

            gEnc = new MagickImageCollection();
            GifCaptureTimer.Interval = (int)(1000.0f / Preferences.GIFRecordingFramerate);
            counter = 0;
            GifCaptureTimer.Enabled = true;
            GifCaptureTimer.Tag = ForceEdit;

            this.topBox.BackColor = Color.Green;
            this.bottomBox.BackColor = Color.Green;
            this.leftBox.BackColor = Color.Green;
            this.rightBox.BackColor = Color.Green;

        }
        public void StopRecordingGif(completeCover cover, bool abort)
        {
            if(GifCaptureTimer.Enabled)
            {
                Program.ImageDeCap.topBox.Hide();
                Program.ImageDeCap.bottomBox.Hide();
                Program.ImageDeCap.leftBox.Hide();
                Program.ImageDeCap.rightBox.Hide();
                cover.Close();
                if (!abort)
                {
                    Utilities.playSound("snip.wav");

                    byte[] gifData = gEnc.ToByteArray(MagickFormat.Gif);
                    // Feed in through the tag weather the user right-clicked to force editor even when it's disabled.
                    UploadImageData(gifData, filetype.gif, false, (bool)GifCaptureTimer.Tag);
                }

                Program.ImageDeCap.isTakingSnapshot = false;
                Program.hotkeysEnabled = true;
                GifCaptureTimer.Enabled = false;
            }
        }

        //public float GifRecorderFPS = 16.0f;
        int counter = 0;
        private void GifCaptureTimer_Tick(object sender, EventArgs e)
        {
            // Capture Bitma
            Bitmap b = Program.ImageDeCap.cap.Capture(
                enmScreenCaptureMode.Bounds,
                Program.ImageDeCap.X - 1,
                Program.ImageDeCap.Y - 1,
                Program.ImageDeCap.tempWidth + 1,
                Program.ImageDeCap.tempHeight + 1);

            MagickImage newImage = new MagickImage();
            newImage.Format = MagickFormat.Gif;
            newImage.Read(b);
            gEnc.Add(newImage);
            gEnc[counter].AnimationDelay = (int)(100.0f / Preferences.GIFRecordingFramerate);

            int minutes = counter / 600;
            int seconds = (counter/10) % 600;
            int csecs = counter % 10;
            CurrentBackCover.SetTimer("Time: " + minutes + ":" + seconds + "." + csecs, "Frames: " + counter);
            counter++;
        }

        public Form1()
        {
            InitializeComponent();

            this.Hide();
            this.ShowInTaskbar = false;

            props = new SettingsWindow(this);

            // Alert for install!
            if (Preferences.firstLaunch)
            {
                Preferences.firstLaunch = false;
                Preferences.Save();
                DialogResult d = MessageBox.Show("First Launch!\nInstall? (add to startup & start-menu)", "Image DeCap", MessageBoxButtons.YesNo);
                if (d == DialogResult.Yes)
                {
                    props.Install();
                    props.AddToStartup();
                    this.ShowInTaskbar = true;
                    this.Show();
                    this.Activate();
                }
            }

            if (File.Exists(LinksFilePath))
            {
                string links = File.ReadAllText(LinksFilePath);
                links = Regex.Replace(links, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
                foreach (string s in links.Split('\n'))
                {
                    addToLinks(s, false);
                }
            }
            else
            {
                File.WriteAllText(LinksFilePath, "");
            }

            SystemTrayContextMenu.Initialize();
            notifyIcon1.ContextMenu = SystemTrayContextMenu.IconRightClickMenu;
            notifyIcon1.Visible = true;

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
                props.Close();
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

        public void actuallyCloseTheProgram()
        {
            pushThroughCancel = true;
            props.Close();
            this.Close();
            Application.Exit();
        }


        private void UploadPastebinClipboard()
        {
            if (!isTakingSnapshot)
            {
                uploadPastebin(Clipboard.GetText());
            }
        }
        completeCover CurrentBackCover;
        public Magnificator magn;
        public bool isTakingSnapshot = false;
        private void UploadToImgurBounds(bool isGif = false)
        {
            // prevent blackening
            if (!isTakingSnapshot)
            {
                isTakingSnapshot = true;
                Program.hotkeysEnabled = false;
                //back cover used for pulling cursor position into updateSelectedArea()
                magn = new Magnificator();
                CurrentBackCover = new completeCover(isGif);
                CurrentBackCover.Show();
                CurrentBackCover.SetBounds(SystemInformation.VirtualScreen.X, SystemInformation.VirtualScreen.Y, SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
                CurrentBackCover.TopMost = false;

                magn.Show();
                magn.TopMost = true;

                setBox(topBox);
                setBox(leftBox);
                setBox(bottomBox);
                setBox(rightBox);
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
            UploadImageData(completeCover.GetBytes(result, System.Drawing.Imaging.ImageFormat.Png), filetype.png);
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
            else if (filepath.EndsWith(".gif"))
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

        public void UploadImageData(byte[] FileData, filetype imageType, bool ForceNoEdit = false, bool ForceEdit = false)
        {
            if(imageType == filetype.error)
            {
                return;
            }
            
            // save unedited capture
            if (Preferences.saveImageAtAll && Directory.Exists(Preferences.SaveImagesHere))
            {
                string SaveFileName = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
                string whereToSave = Preferences.SaveImagesHere + @"\" + SaveFileName + "." + imageType.ToString();
                File.WriteAllBytes(whereToSave, FileData);
            }

            bool CanceledUpload = false;
            // Edit the image if we are going to
            if (Preferences.EditScreenshotAfterCapture || ForceEdit)
            {
                if (ForceNoEdit == false)
                {
                    if(imageType == filetype.gif)
                    {
                        GifEditor editor = new GifEditor(FileData, topBox.Location.X, topBox.Location.Y);
                        editor.ShowDialog();
                        (CanceledUpload, FileData) = editor.FinalFunction();
                    }
                    else
                    {
                        imageEditor editor = new imageEditor(FileData, topBox.Location.X, topBox.Location.Y);
                        editor.ShowDialog();
                        (CanceledUpload, FileData) = editor.FinalFunction();
                    }
                }
            }

            // Copy image to clipboard if it's not a gif
            if (imageType != filetype.gif)
            {
                if (Preferences.CopyImageToClipboard)
                {
                    Image bitmapImage = Image.FromStream(new MemoryStream(FileData));
                    Clipboard.SetImage(bitmapImage);
                    bitmapImage.Dispose();
                }
            }
            
            // Upload the image
            if(!CanceledUpload)
            {
                if (!Preferences.NeverUpload)
                {
                    BackgroundWorker bw = new BackgroundWorker();
                    bw.DoWork += cap.UploadImage;
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
                notifyIcon1.ShowBalloonTip(500, "imageDeCap", "Upload to imgur failed! \n" + url + "\nAre you connected to the internet? \nis Imgur Down?", ToolTipIcon.Error);
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
                        notifyIcon1.ShowBalloonTip(500, "imageDeCap", "Imgur URL copied to clipboard!", ToolTipIcon.Info);
                }
                else
                {
                    if (!Preferences.DisableNotifications)
                        notifyIcon1.ShowBalloonTip(500, "imageDeCap", "Upload complete!", ToolTipIcon.Info);
                }
                
                if (!Utilities.IsWindows10() || Preferences.DisableNotifications)
                {//means it's probably windows 10, in which case we should not play the noise as windows 10 plays a fucking noise of its own no matter what. :|
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
                bw.DoWork += cap.Send;
                bw.RunWorkerCompleted += uploadPastebinCompleted;
                bw.RunWorkerAsync(text);
            }

            if (Preferences.AlsoFTPTextFiles)
            {
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += cap.uploadToFTP;
                string name = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
                bw.RunWorkerAsync(new object[] {    Preferences.FTPurl,
                                                    Preferences.FTPusername,
                                                    Preferences.FTPpassword,
                                                    Encoding.ASCII.GetBytes(text),
                                                    name + ".txt" });

            }
            if (Preferences.AlsoSaveTextFiles)
            {
                if (Preferences.saveImageAtAll && Directory.Exists(Preferences.SaveImagesHere))
                {
                    string name = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
                    string whereToSave = Preferences.SaveImagesHere + @"\" + name + ".txt";
                    File.WriteAllText(whereToSave, text);
                }
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
                        notifyIcon1.ShowBalloonTip(500, "imageDeCap", "Pastebin link placed in clipboard!", ToolTipIcon.Info);
                }
                else
                {
                    if (!Preferences.DisableNotifications)
                        notifyIcon1.ShowBalloonTip(500, "imageDeCap", "Upload complete!", ToolTipIcon.Info);
                }

                if (!Utilities.IsWindows10() || Preferences.DisableNotifications)
                {//means it's probably windows 10, in which case we should not play the noise as windows 10 plays a fucking noise on its own no matter what. :|
                    Utilities.playSound("upload.wav");
                }
                addToLinks(pasteBinResult);
            }
            else
            {
                notifyIcon1.ShowBalloonTip(500, "imageDeCap", "upload to pastebin failed!\n" + pasteBinResult + "\nAre you connected to the internet? \nIs pastebin Down?", ToolTipIcon.Error);
                Utilities.playSound("error.wav");
            }
        }

        private void setBox(boxOfWhy box)
        {
            box.Show();
            box.BackColor = Color.Red;
            box.Opacity = 0.5;
            box.SetBounds(0, 0, 0, 0);
            box.TopMost = true;
        }
        
        public ScreenCapturer cap = new ScreenCapturer();

        public boxOfWhy topBox = new boxOfWhy();
        public boxOfWhy bottomBox = new boxOfWhy();
        public boxOfWhy leftBox = new boxOfWhy();
        public boxOfWhy rightBox = new boxOfWhy();

        public int X = 0;
        public int Y = 0;
        int tempX = 0;
        int tempY = 0;
        public int tempWidth = 0;
        public int tempHeight = 0;

        public void updateSelectedArea(completeCover backCover, bool EnterPressed, bool EscapePressed, bool LmbDown, bool LmbUp, bool Lmb, bool Gif, bool RMB) // this thing is essentially a fucking frame-loop.
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
                
                tempWidth = Math.Abs(Cursor.Position.X - tempX);
                tempHeight = Math.Abs(Cursor.Position.Y - tempY);

                Y = tempY;
                X = tempX;

                if ((Cursor.Position.Y - tempY) < 0)
                    Y = tempY + (Cursor.Position.Y - tempY);

                if ((Cursor.Position.X - tempX) < 0)
                    X = tempX + (Cursor.Position.X - tempX);
            }
            if (EscapePressed)
            {
                magn.Close();
                backCover.Close();

                topBox.Hide();
                bottomBox.Hide();
                leftBox.Hide();
                rightBox.Hide();
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
            actuallyCloseTheProgram();
        }

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            props.Close();
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aboutWindow about;
            about = new aboutWindow();
            about.Show();
        }

        private void clearLinksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Links.Clear();
            listBox1.DataSource = null;
            listBox1.DataSource = Links;
            File.Delete(LinksFilePath);
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.RightCtrl) || System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftCtrl)) &&
                (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.C) || System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.Insert)))
            {
                Clipboard.SetText(Links[listBox1.SelectedIndex]);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}
