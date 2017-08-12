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
using AnimatedGif;
//using AnimatedGif;

namespace imageDeCap
{
    public partial class Form1 : Form
    {
        string xmlLinksPath = Path.GetTempPath() + "imageDecapLinks.xml";
        XElement xmlLinks = new XElement("uploadList");
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
                xmlLinks.Add(new XElement("Link", link));
                xmlLinks.Save(xmlLinksPath);
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
                if (imageDeCap.Properties.Settings.Default.Hotkey1 == hotkey)
                {
                    if (!hKey1Pressed)
                    {
                        UploadPastebinClipboard();
                        Console.WriteLine("HOTKEY1");
                    }
                    hKey1Pressed = true;
                }
                else if (imageDeCap.Properties.Settings.Default.Hotkey2 == hotkey)
                {
                    if (!hKey2Pressed)
                    {
                        //UploadImgurScreen();
                        UploadToImgurBounds(true);
                        Console.WriteLine("HOTKEY2");
                    }
                    hKey2Pressed = true;
                }
                else if (imageDeCap.Properties.Settings.Default.Hotkey3 == hotkey)
                {
                    if (!hKey3Pressed)
                    {
                        UploadToImgurBounds();
                        Console.WriteLine("HOTKEY3");
                    }
                    hKey3Pressed = true;
                }
                else if (imageDeCap.Properties.Settings.Default.Hotkey4 == hotkey)
                {
                    if (!hKey4Pressed)
                    {
                        UploadImgurWindow();
                        Console.WriteLine("HOTKEY4");
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

        bool useImageMagick = true;
        MagickImageCollection gEnc;
        Gifed.AnimatedGif gCreat;
        public void StartRecordingGif()
        {

            gEnc = new MagickImageCollection();
            gCreat = new Gifed.AnimatedGif();

            counter = 0;
            GifCaptureTimer.Enabled = true;

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

                    File.Delete(Path.GetTempPath() + "screenshot.gif");

                    if(useImageMagick)
                    {
                        gEnc.Write(Path.GetTempPath() + "screenshot.gif");
                    }
                    else
                    {
                        gCreat.Save(Path.GetTempPath() + "screenshot.gif");
                    }

                    
                    uploadImageFile(Path.GetTempPath() + "screenshot.gif", imageDeCap.Properties.Settings.Default.EditScreenshotAfterCapture);
                }

                Program.ImageDeCap.isTakingSnapshot = false;
                Program.hotkeysEnabled = true;
                GifCaptureTimer.Enabled = false;
            }
        }
        int counter = 0;
        private void GifCaptureTimer_Tick(object sender, EventArgs e)
        {
            // Capture Bitmap
            Bitmap b = cap.Capture(enmScreenCaptureMode.Bounds, tempX, tempY, tempWidth, tempHeight);
            gCreat.AddFrame(b, 10);
            gEnc.Add(new MagickImage(b));
            gEnc[counter].AnimationDelay = 10;
            
            counter++;

        }

        public Form1()
        {
            InitializeComponent();

            this.Hide();
            this.ShowInTaskbar = false;
            //this.Opacity = 0.0f;


            props = new SettingsWindow(this);

            // Alert for install!
            if (imageDeCap.Properties.Settings.Default.firstLaunch)
            {
                imageDeCap.Properties.Settings.Default.firstLaunch = false;
                imageDeCap.Properties.Settings.Default.Save();
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

            if (File.Exists(xmlLinksPath))
            {
                try
                {
                    XElement.Load(xmlLinksPath);
                }
                catch
                {
                    File.Delete(xmlLinksPath);
                }
            }

            if (File.Exists(xmlLinksPath))
            {
                xmlLinks = XElement.Load(xmlLinksPath);
                foreach (XElement e in xmlLinks.Elements())
                {
                    addToLinks(e.Value, false);
                }
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
                uploadImageFile(files[0]);
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
            UploadImgurScreen();
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
        private void UploadToImgurBounds(bool Gif = false)
        {
            // prevent blackening
            if (!isTakingSnapshot)
            {
                isTakingSnapshot = true;
                Program.hotkeysEnabled = false;
                //back cover used for pulling cursor position into updateSelectedArea()
                magn = new Magnificator();
                CurrentBackCover = new completeCover(Gif);
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

            result.Save(Path.GetTempPath() + "screenshot.png");
            result.Dispose();

            uploadImageFile(Path.GetTempPath() + "screenshot.png", imageDeCap.Properties.Settings.Default.EditScreenshotAfterCapture);

        }


        public  void uploadImageFile(string filePath, bool edit = false)
        {
            string name = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
            // Will always be .png here. Weather to compress or not is decided in the editor.
            string ending = ".png";
            bool IsGif = false;
            if(filePath.EndsWith(".jpg"))
            {
                ending = ".jpg";
            }
            else if(filePath.EndsWith(".gif"))
            {
                ending = ".gif";
                IsGif = true;
            }
            string whereToSave = imageDeCap.Properties.Settings.Default.SaveImagesHere + @"\" + name + ending;
            // save unedited capture
            if (imageDeCap.Properties.Settings.Default.saveImageAtAll && Directory.Exists(imageDeCap.Properties.Settings.Default.SaveImagesHere))
            {
                File.Copy(filePath, whereToSave);
            }
            if (IsGif == false)
            {
                Image bitmapImage = Image.FromFile(filePath);
                if (imageDeCap.Properties.Settings.Default.CopyImageToClipboard)
                {
                    Clipboard.SetImage(bitmapImage);
                    bitmapImage.Dispose();
                }
            } 
            string editedPath = Path.GetTempPath() + "screenshot_edited.png";
            
            if (edit)
            {
                if (IsGif)
                {
                    GifEditor editor = new GifEditor(filePath, whereToSave);
                    editor.ShowDialog();
                    editedPath = Path.GetTempPath() + "screenshot_edited.gif";
                }
                else
                {
                    imageEditor editor = new imageEditor(filePath, whereToSave, tempX, tempY);
                    editor.ShowDialog();
                    if (editor.checkBox1.Checked)//If compressed, pick the compressed version.
                    {
                        editedPath = Path.GetTempPath() + "screenshot_edited.jpg";
                    }

                    filePath = editedPath;
                    if (File.Exists(editedPath))
                    {
                        if (imageDeCap.Properties.Settings.Default.CopyImageToClipboard)
                        {
                            Image bitmapImage = Image.FromFile(filePath);
                            bitmapImage = Image.FromFile(filePath);
                            Clipboard.SetImage(bitmapImage);
                            bitmapImage.Dispose();
                        }
                    }
                }
            }
            else
            {
                if (File.Exists(editedPath))
                {
                    File.Delete(editedPath);
                }
                File.Copy(filePath, editedPath);
                filePath = editedPath;
            }

            if (!imageDeCap.Properties.Settings.Default.NeverUpload)
            {
                if (File.Exists(editedPath))
                {
                    BackgroundWorker bw = new BackgroundWorker();
                    bw.DoWork += cap.UploadImage;
                    bw.RunWorkerCompleted += uploadImageFileCompleted;
                    bw.RunWorkerAsync(filePath);
                }
            }

        }

        private void uploadImageFileCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string url = (string)e.Result;
            if (url.Contains("failed"))
            {
                ClipboardHandler.setClipboard(url);
                notifyIcon1.ShowBalloonTip(500, "imageDeCap", "Upload to imgur failed! \n" + url + "\nAre you connected to the internet? \nis Imgur Down?", ToolTipIcon.Error);
                Utilities.playSound("error.wav");
            }
            else
            {
                if (imageDeCap.Properties.Settings.Default.UseHTTPS)
                {
                    StringBuilder builder = new StringBuilder(url);
                    builder.Replace("http", "https");
                    url = builder.ToString();
                }
                if (!imageDeCap.Properties.Settings.Default.CopyLinksToClipboard)
                {
                    if (imageDeCap.Properties.Settings.Default.DisableNotifications)
                        notifyIcon1.ShowBalloonTip(500, "imageDeCap", "Imgur URL copied to clipboard!", ToolTipIcon.Info);
                }
                else
                {
                    if (!imageDeCap.Properties.Settings.Default.DisableNotifications)
                        notifyIcon1.ShowBalloonTip(500, "imageDeCap", "Upload complete!", ToolTipIcon.Info);
                }


                if (!Utilities.IsWindows10() || imageDeCap.Properties.Settings.Default.DisableNotifications)
                {//means it's probably windows 10, in which case we should not play the noise as windows 10 plays a fucking noise of its own no matter what. :|
                    Utilities.playSound("upload.wav");
                }
                ClipboardHandler.setClipboard(url);
                addToLinks(url);
            }

            if (imageDeCap.Properties.Settings.Default.uploadToFTP)
            {
                if (File.Exists(Path.GetTempPath() + "screenshot_edited.png") || File.Exists(Path.GetTempPath() + "screenshot_edited.jpg"))
                {
                    string name = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
                    BackgroundWorker bw = new BackgroundWorker();
                    bw.DoWork += cap.uploadToFTP;
                    bw.RunWorkerAsync(new string[] {    imageDeCap.Properties.Settings.Default.FTPurl,
                                                        imageDeCap.Properties.Settings.Default.FTPusername,
                                                        imageDeCap.Properties.Settings.Default.FTPpassword,
                                                        Path.GetTempPath() + "screenshot_edited.png", name + (url.EndsWith(".png") ? ".png" : ".jpg") });
                }

            }
        }

        private void uploadPastebin(string text)
        {
            if (!imageDeCap.Properties.Settings.Default.NeverUpload)
            {
                Utilities.playSound("snip.wav");
                //string pasteBinResult = cap.Send(text);
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += cap.Send;
                bw.RunWorkerCompleted += uploadPastebinCompleted;
                bw.RunWorkerAsync(text);
            }

            if (imageDeCap.Properties.Settings.Default.AlsoFTPTextFiles)
            {
                string tempTextFileFolder = Path.GetTempPath() + "textfile.txt";
                File.WriteAllText(tempTextFileFolder, text);
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += cap.uploadToFTP;
                string name = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
                bw.RunWorkerAsync(new string[] {    imageDeCap.Properties.Settings.Default.FTPurl,
                                                    imageDeCap.Properties.Settings.Default.FTPusername,
                                                    imageDeCap.Properties.Settings.Default.FTPpassword,
                                                    tempTextFileFolder,
                                                    name +".txt" });

            }
            if (imageDeCap.Properties.Settings.Default.AlsoSaveTextFiles)
            {
                if (imageDeCap.Properties.Settings.Default.saveImageAtAll && Directory.Exists(imageDeCap.Properties.Settings.Default.SaveImagesHere))
                {
                    string name = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
                    string whereToSave = imageDeCap.Properties.Settings.Default.SaveImagesHere + @"\" + name + ".txt";
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
                if (imageDeCap.Properties.Settings.Default.CopyLinksToClipboard)
                {
                    if (!imageDeCap.Properties.Settings.Default.DisableNotifications)
                        notifyIcon1.ShowBalloonTip(500, "imageDeCap", "Pastebin link placed in clipboard!", ToolTipIcon.Info);
                }
                else
                {
                    if (!imageDeCap.Properties.Settings.Default.DisableNotifications)
                        notifyIcon1.ShowBalloonTip(500, "imageDeCap", "Upload complete!", ToolTipIcon.Info);
                }

                if (!Utilities.IsWindows10() || imageDeCap.Properties.Settings.Default.DisableNotifications)
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

        public void updateSelectedArea(completeCover backCover, bool EnterPressed, bool EscapePressed, bool LmbDown, bool LmbUp, bool Lmb, bool Gif) // this thing is essentially a fucking frame-loop.
        {
            bool UseBackCover = false;
            if(imageDeCap.Properties.Settings.Default.FreezeScreenOnRegionShot)
            {
                UseBackCover = true;
            }
            if(Gif)
            {
                UseBackCover = false;
            }
                

            backCover.Activate();
            magn.Bounds = new Rectangle(Cursor.Position.X + 32, Cursor.Position.Y - 32, 124, 124);
            
                                                                                
            if (LmbUp) // keyUp
            {

                //* This should be a function to call for what happens when we have aquired a region.
                backCover.CompletedSelection();
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
            File.Delete(xmlLinksPath);
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
            string path = Path.GetTempPath() + "screenshot.gif";
            //GifEditor editor = new GifEditor(path, "asdf");
            //editor.Show();
        }
    }
}
