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
using System.Media;
using System.Diagnostics;
using System.Threading;
using System.Xml.Linq;
using Microsoft.Win32;

namespace imageDeCap
{
    public partial class Form1 : Form
    {
        string xmlLinksPath = System.IO.Path.GetTempPath() + "imageDecapLinks.xml";
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
                    System.Diagnostics.Process.Start(Links[listBox1.SelectedIndex]);
        }

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            if (Links.Count > 0)
                if (Links[listBox1.SelectedIndex].StartsWith("http"))
                    System.Diagnostics.Process.Start(Links[listBox1.SelectedIndex]);
        }

        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuItem1 = new System.Windows.Forms.MenuItem();
        private System.Windows.Forms.MenuItem menuItem2 = new System.Windows.Forms.MenuItem();
        private System.Windows.Forms.MenuItem menuItem3 = new System.Windows.Forms.MenuItem();
        private System.Windows.Forms.MenuItem menuItem4 = new System.Windows.Forms.MenuItem();

        SettingsWindow props;
        //GlobalKeyboardHook gHook; 

        bool hKey1Pressed = false;
        bool hKey2Pressed = false;
        bool hKey3Pressed = false;
        bool hKey4Pressed = false;
        public void mainLoop()
        {
            if (textToCopyToClipboard != "")
            {
                Clipboard.SetText(textToCopyToClipboard);
                textToCopyToClipboard = "";
            }

            if (Program.hotkeysEnabled)
            {
                string hotkey = SettingsWindow.getCurrentHotkey();
                //Console.WriteLine(Properties.Settings.Default.Hotkey1);
                if (Properties.Settings.Default.Hotkey1 == hotkey)
                {
                    if (!hKey1Pressed)
                    {
                        UploadPastebinClipboard();
                        Console.WriteLine("HOTKEY1");
                    }
                    hKey1Pressed = true;
                }
                else if (Properties.Settings.Default.Hotkey2 == hotkey)
                {
                    if (!hKey2Pressed)
                    {
                        UploadImgurScreen();
                        Console.WriteLine("HOTKEY2");
                    }
                    hKey2Pressed = true;
                }
                else if (Properties.Settings.Default.Hotkey3 == hotkey)
                {
                    if (!hKey3Pressed)
                    {
                        UploadToImgurBounds();
                        Console.WriteLine("HOTKEY3");
                    }
                    hKey3Pressed = true;
                }
                else if (Properties.Settings.Default.Hotkey4 == hotkey)
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
                    //no recognized hotkey
                    hKey1Pressed = false;
                    hKey2Pressed = false;
                    hKey3Pressed = false;
                    hKey4Pressed = false;
                }
            }
        }
        public Form1()
        {
            InitializeComponent();

            this.Hide();
            this.ShowInTaskbar = false;
            //this.Opacity = 0.0f;


            props = new SettingsWindow(this);

            //Alert for install!
            if (Properties.Settings.Default.firstLaunch)
            {
                Properties.Settings.Default.firstLaunch = false;
                Properties.Settings.Default.Save();
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

            this.contextMenu1 = new System.Windows.Forms.ContextMenu();

            this.contextMenu1.MenuItems.Add(this.menuItem2);
            this.contextMenu1.MenuItems.Add("-");
            this.contextMenu1.MenuItems.Add(this.menuItem3);
            this.contextMenu1.MenuItems.Add(this.menuItem4);
            this.contextMenu1.MenuItems.Add(this.menuItem1);

            this.menuItem1.Text = "Exit";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);

            this.menuItem3.Text = "Contact / Bugs";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);

            this.menuItem4.Text = "Properties";
            this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);

            this.menuItem2.Text = "Open Window";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);

            notifyIcon1.ContextMenu = contextMenu1;
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
        private void menuItem2_Click(object Sender, EventArgs e)//Open Window
        {
            this.ShowInTaskbar = true;
            this.Show();
            this.Activate();

        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)//Double Click notifyIcon
        {
            this.ShowInTaskbar = true;
            this.Show();
            this.Activate();
        }

        public Hotkey hook;

        public void gui_clipboardToPastebin(HotkeyEventArgs e)
        {
            UploadPastebinClipboard();
        }
        public void gui_boundsToImgur(HotkeyEventArgs e)
        {
            UploadToImgurBounds();
        }
        public void gui_windowToImgur(HotkeyEventArgs e)
        {
            UploadImgurWindow();
        }
        public void gui_screenToImgur(HotkeyEventArgs e)
        {
            UploadImgurScreen();
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


        private void menuItem1_Click(object Sender, EventArgs e)//Exit button
        {
            actuallyCloseTheProgram();
        }
        public void actuallyCloseTheProgram()
        {
            pushThroughCancel = true;
            props.Close();
            this.Close();
            Application.Exit();
        }



        private void menuItem3_Click(object Sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.mattwestphal.com/imagedecap/");
        }
        private void menuItem4_Click(object Sender, EventArgs e)
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void UploadPastebinClipboard()
        {
            if (!isTakingSnapshot)
            {
                uploadPastebin(Clipboard.GetText());
            }
        }

        Magnificator magn;
        bool isTakingSnapshot = false;
        private void UploadToImgurBounds()
        {
            // prevent blackening
            if (!isTakingSnapshot)
            {
                isTakingSnapshot = true;
                Program.hotkeysEnabled = false;
                //back cover used for pulling cursor position into updateSelectedArea()
                magn = new Magnificator();
                completeCover backCover = new completeCover(this);
                backCover.Show();
                backCover.SetBounds(SystemInformation.VirtualScreen.X, SystemInformation.VirtualScreen.Y, SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
                backCover.TopMost = false;

                magn.Show();
                magn.TopMost = true;
                //backCover.BackColor = Color.White;
                //backCover.Opacity = 0.1;

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
            playSound("snip.wav");
            Bitmap result = cap.Capture(mode);

            result.Save(System.IO.Path.GetTempPath() + "screenshot.png");
            result.Dispose();
            if (Properties.Settings.Default.EditScreenshotAfterCapture)
            {
                uploadImageFile(System.IO.Path.GetTempPath() + "screenshot.png", true);
            }
            else
            {
                uploadImageFile(System.IO.Path.GetTempPath() + "screenshot.png");
            }
        }


        private void uploadImageFile(string filePath, bool edit = false)
        {
            string name = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
            // Will always be .png here. Weather to compress or not is decided in the editor.
            string whereToSave = Properties.Settings.Default.SaveImagesHere + @"\" + name + (filePath.EndsWith(".png") ? ".png" : ".jpg");
            // save unedited capture
            if (Properties.Settings.Default.saveImageAtAll && Directory.Exists(Properties.Settings.Default.SaveImagesHere))
            {
                //if (!edit)
                //{
                File.Copy(filePath, whereToSave);
                //}
            }
            Image bitmapImage = Image.FromFile(filePath);
            if (Properties.Settings.Default.CopyImageToClipboard)
            {
                Clipboard.SetImage(bitmapImage);
                bitmapImage.Dispose();
            }

            string editedPath = System.IO.Path.GetTempPath() + "screenshot_edited.png";
            if (edit)
            {
                imageEditor editor = new imageEditor(filePath, whereToSave);
                editor.ShowDialog();
                if (editor.checkBox1.Checked)//If compressed, pick the compressed version.
                {
                    editedPath = System.IO.Path.GetTempPath() + "screenshot_edited.jpg";
                }

                filePath = editedPath;
                if (File.Exists(editedPath))
                {
                    if (Properties.Settings.Default.CopyImageToClipboard)
                    {
                        bitmapImage = Image.FromFile(filePath);
                        Clipboard.SetImage(bitmapImage);
                        bitmapImage.Dispose();
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
            if (!Properties.Settings.Default.NeverUpload)
            {
                if (File.Exists(editedPath))
                {
                    //string url = (string)cap.UploadImage(filePath);

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
                setClipboard(url);
                notifyIcon1.ShowBalloonTip(500, "imageDeCap", "Upload to imgur failed! \n" + url + "\nAre you connected to the internet? \nis Imgur Down?", ToolTipIcon.Error);
                playSound("error.wav");
            }
            else
            {
                if (Properties.Settings.Default.UseHTTPS)
                {
                    StringBuilder builder = new StringBuilder(url);
                    builder.Replace("http", "https");
                    url = builder.ToString();
                }
                if (!Properties.Settings.Default.CopyLinksToClipboard)
                {
                    if (Properties.Settings.Default.DisableNotifications)
                        notifyIcon1.ShowBalloonTip(500, "imageDeCap", "Imgur URL copied to clipboard!", ToolTipIcon.Info);
                }
                else
                {
                    if (!Properties.Settings.Default.DisableNotifications)
                        notifyIcon1.ShowBalloonTip(500, "imageDeCap", "Upload complete!", ToolTipIcon.Info);
                }


                if (!IsWindows10() || Properties.Settings.Default.DisableNotifications)
                {//means it's probably windows 10, in which case we should not play the noise as windows 10 plays a fucking noise on its own no matter what. :|
                    playSound("upload.wav");
                }
                setClipboard(url);
                addToLinks(url);
            }

            if (Properties.Settings.Default.uploadToFTP)
            {
                if (File.Exists(System.IO.Path.GetTempPath() + "screenshot_edited.png") || File.Exists(System.IO.Path.GetTempPath() + "screenshot_edited.jpg"))
                {
                    string name = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
                    BackgroundWorker bw = new BackgroundWorker();
                    bw.DoWork += cap.uploadToFTP;
                    bw.RunWorkerAsync(new string[] {    Properties.Settings.Default.FTPurl,
                                                        Properties.Settings.Default.FTPusername,
                                                        Properties.Settings.Default.FTPpassword,
                                                        System.IO.Path.GetTempPath() + "screenshot_edited.png", name + (url.EndsWith(".png") ? ".png" : ".jpg") });
                }

            }
        }
        string textToCopyToClipboard = "";
        private void setClipboard(string text)
        {
            if (Properties.Settings.Default.CopyLinksToClipboard)
            {
                if (text != null)
                {
                    textToCopyToClipboard = text;
                    //Clipboard.SetText(text);
                }
                else
                {
                    notifyIcon1.ShowBalloonTip(500, "imageDeCap", "failed to retrieve link.", ToolTipIcon.Error);
                    playSound("error.wav");
                }
            }
        }

        private void uploadPastebin(string text)
        {
            if (!Properties.Settings.Default.NeverUpload)
            {
                playSound("snip.wav");
                //string pasteBinResult = cap.Send(text);
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += cap.Send;
                bw.RunWorkerCompleted += uploadPastebinCompleted;
                bw.RunWorkerAsync(text);
            }

            if (Properties.Settings.Default.AlsoFTPTextFiles)
            {
                string tempTextFileFolder = System.IO.Path.GetTempPath() + "textfile.txt";
                File.WriteAllText(tempTextFileFolder, text);
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += cap.uploadToFTP;
                string name = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
                bw.RunWorkerAsync(new string[] {    Properties.Settings.Default.FTPurl,
                                                    Properties.Settings.Default.FTPusername,
                                                    Properties.Settings.Default.FTPpassword,
                                                    tempTextFileFolder,
                                                    name +".txt" });

            }
            if (Properties.Settings.Default.AlsoSaveTextFiles)
            {
                if (Properties.Settings.Default.saveImageAtAll && Directory.Exists(Properties.Settings.Default.SaveImagesHere))
                {
                    string name = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
                    string whereToSave = Properties.Settings.Default.SaveImagesHere + @"\" + name + ".txt";
                    File.WriteAllText(whereToSave, text);
                }
            }

        }
        private void uploadPastebinCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string pasteBinResult = (string)e.Result;
            if (!pasteBinResult.Contains("failed"))
            {
                setClipboard(pasteBinResult);
                if (Properties.Settings.Default.CopyLinksToClipboard)
                {
                    if (!Properties.Settings.Default.DisableNotifications)
                        notifyIcon1.ShowBalloonTip(500, "imageDeCap", "Pastebin link placed in clipboard!", ToolTipIcon.Info);
                }
                else
                {
                    if (!Properties.Settings.Default.DisableNotifications)
                        notifyIcon1.ShowBalloonTip(500, "imageDeCap", "Upload complete!", ToolTipIcon.Info);
                }

                if (!IsWindows10() || Properties.Settings.Default.DisableNotifications)
                {//means it's probably windows 10, in which case we should not play the noise as windows 10 plays a fucking noise on its own no matter what. :|
                    playSound("upload.wav");
                }
                addToLinks(pasteBinResult);
            }
            else
            {
                notifyIcon1.ShowBalloonTip(500, "imageDeCap", "upload to pastebin failed!\n" + pasteBinResult + "\nAre you connected to the internet? \nIs pastebin Down?", ToolTipIcon.Error);
                playSound("error.wav");
            }
        }
        static bool IsWindows10()
        {
            var reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");

            string productName = (string)reg.GetValue("ProductName");

            return productName.StartsWith("Windows 10");
        }
        private void setBox(boxOfWhy box)
        {
            box.Show();
            box.BackColor = Color.Red;
            box.Opacity = 0.5;
            box.SetBounds(0, 0, 0, 0);
            box.TopMost = true;
        }

        bool wasPressed = false;

        ScreenCapturer cap = new ScreenCapturer();

        boxOfWhy topBox = new boxOfWhy();
        boxOfWhy bottomBox = new boxOfWhy();
        boxOfWhy leftBox = new boxOfWhy();
        boxOfWhy rightBox = new boxOfWhy();

        int X = 0;
        int Y = 0;
        int tempX = 0;
        int tempY = 0;
        int Width = 0;
        int Height = 0;

        public void playSound(string soundName)
        {
            if (!Properties.Settings.Default.DisableSoundEffects)
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                SoundPlayer sp = new SoundPlayer(assembly.GetManifestResourceStream("imageDeCap." + soundName));
                sp.Play();
            }
        }

        public void updateSelectedArea(completeCover backCover, bool keyPressed, bool escapePressed)//this thing is essentially a fucking frame-loop.
        {
            backCover.Activate();
            magn.Bounds = new Rectangle(Cursor.Position.X + 32, Cursor.Position.Y - 32, 124, 124);
            if (wasPressed != (MouseButtons == MouseButtons.Left))
            {
                if (wasPressed)//keyUp
                {
                    magn.Close();
                    if (!Properties.Settings.Default.FreezeScreenOnRegionShot)
                        backCover.Close();

                    topBox.Hide();
                    bottomBox.Hide();
                    leftBox.Hide();
                    rightBox.Hide();
                    if (Width > 0 && Height > 0)
                    {
                        playSound("snip.wav");
                        Bitmap result = cap.Capture(enmScreenCaptureMode.Bounds, X - 1, Y - 1, Width + 1, Height + 1);

                        if (Properties.Settings.Default.FreezeScreenOnRegionShot)
                            backCover.Close();

                        File.Delete(System.IO.Path.GetTempPath() + "screenshot.png");
                        result.Save(System.IO.Path.GetTempPath() + "screenshot.png");
                        result.Dispose();
                        if (Properties.Settings.Default.EditScreenshotAfterCapture)
                        {
                            uploadImageFile(System.IO.Path.GetTempPath() + "screenshot.png", true);
                        }
                        else
                        {
                            uploadImageFile(System.IO.Path.GetTempPath() + "screenshot.png");
                        }
                    }
                    else
                    {

                    }
                    if (Properties.Settings.Default.FreezeScreenOnRegionShot)
                        backCover.Close();
                    isTakingSnapshot = false;
                    Program.hotkeysEnabled = true;
                }
                else//keyDown
                {
                    tempX = Cursor.Position.X;
                    tempY = Cursor.Position.Y;
                }
            }

            if (MouseButtons == MouseButtons.Left)
            {
                topBox.SetBounds(X - 3, Y - 3, Width + 3, 0);
                leftBox.SetBounds(X - 3, Y - 1, 0, Height + 1);
                bottomBox.SetBounds(X - 3, Height + Y, Width + 5, 0);
                rightBox.SetBounds(Width + X, Y - 3, 0, Height + 3);


                Width = Math.Abs(Cursor.Position.X - tempX);
                Height = Math.Abs(Cursor.Position.Y - tempY);

                Y = tempY;
                X = tempX;

                if ((Cursor.Position.Y - tempY) < 0)
                    Y = tempY + (Cursor.Position.Y - tempY);

                if ((Cursor.Position.X - tempX) < 0)
                    X = tempX + (Cursor.Position.X - tempX);
            }
            if (escapePressed)
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


            wasPressed = MouseButtons == MouseButtons.Left;

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void clearLinksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Links.Clear();
            listBox1.DataSource = null;
            listBox1.DataSource = Links;
            File.Delete(xmlLinksPath);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.B))
            {
                Console.WriteLine("aa");
            }
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.RightCtrl) || System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftCtrl)) &&
                (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.C) || System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.Insert)))
            {
                Clipboard.SetText(Links[listBox1.SelectedIndex]);
            }
        }
    }
}