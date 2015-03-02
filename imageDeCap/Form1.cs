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
using System.Windows.Input;
using System.Media;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Media;
using System.Diagnostics;
using System.Threading;

namespace imageDeCap
{
    public partial class Form1 : Form
    {

        List<string> Links = new List<string>();
        private void addToLinks(string link)
        {
            Links.Add(link);
            listBox1.DataSource = null;
            listBox1.DataSource = Links;

            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Links[listBox1.SelectedIndex]);
        }

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Links[listBox1.SelectedIndex]);
        }

        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuItem1 = new System.Windows.Forms.MenuItem();
        private System.Windows.Forms.MenuItem menuItem2 = new System.Windows.Forms.MenuItem();
        private System.Windows.Forms.MenuItem menuItem3 = new System.Windows.Forms.MenuItem();
        private System.Windows.Forms.MenuItem menuItem4 = new System.Windows.Forms.MenuItem();

        SettingsWindow props;
        public Form1()
        {
            InitializeComponent();
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


            hook = new Hotkey(label1);
            //setHotkeys();
            props = new SettingsWindow(this);
            //hook.Dispose();
            hook.registerHotkey(Modifier.Ctrl | Modifier.Shift, Keys.D2, gui_clipboardToPastebin);
            hook.registerHotkey(Modifier.Ctrl | Modifier.Shift, Keys.D3, gui_windowToImgur);
            hook.registerHotkey(Modifier.Ctrl | Modifier.Shift, Keys.D4, gui_boundsToImgur);
            hook.registerHotkey(Modifier.Ctrl | Modifier.Shift, Keys.D5, gui_screenToImgur);



            this.ShowInTaskbar = false;
            this.Opacity = 0.0f;

            listBox1.AllowDrop = true;
            listBox1.DragEnter += new DragEventHandler(Form1_DragEnter);
            listBox1.DragDrop += new DragEventHandler(Form1_DragDrop);
        }

        public void setHotkeys()
        {
            setHotkey(Properties.Settings.Default.shortcut_pastebin_mod, Properties.Settings.Default.shortcut_pastebin_key, gui_clipboardToPastebin);
            setHotkey(Properties.Settings.Default.shortcut_window_mod, Properties.Settings.Default.shortcut_window_key, gui_windowToImgur);
            setHotkey(Properties.Settings.Default.shortcut_region_mod, Properties.Settings.Default.shortcut_region_key, gui_boundsToImgur);
            setHotkey(Properties.Settings.Default.shortcut_screen_mod, Properties.Settings.Default.shortcut_screen_key, gui_screenToImgur);

        }
        public void setHotkey(Keys modifier, Keys key, HotkeyPressedCb func)
        {
            hook.Dispose();
            Modifier mod = (modifier.HasFlag(Keys.Control) ? Modifier.Ctrl : Modifier.None) |
                (modifier.HasFlag(Keys.Shift) ? Modifier.Shift : Modifier.None) |
                (modifier.HasFlag(Keys.Alt) ? Modifier.Alt : Modifier.None);
            hook.registerHotkey(mod, key, func);
        }

        void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if(files.Length > 0)
            {
                uploadImageFile(files[0]);
            }
        }

        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.ShowInTaskbar = false;
                this.Opacity = 0.0f;
            }
        }
        private void menuItem2_Click(object Sender, EventArgs e)//Open Window
        {
            this.ShowInTaskbar = true;
            this.Opacity = 1.0f;
            this.Activate();

        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)//Double Click notifyIcon
        {
            this.ShowInTaskbar = true;
            this.Opacity = 1.0f;
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
            Application.Exit();
        }



        private void menuItem3_Click(object Sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.mattwestphal.com");
        }
        private void menuItem4_Click(object Sender, EventArgs e)
        {
            props.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void UploadPastebinClipboard()
        {
            uploadPastebin(Clipboard.GetText());
        }

        Magnificator magn;
        bool isTakingSnapshot = false;
        private void UploadToImgurBounds()
        {
            if(!isTakingSnapshot)
            {
                isTakingSnapshot = true;
                //back cover used for pulling cursor position into updateSelectedArea()
                magn = new Magnificator();
                completeCover backCover = new completeCover(this);
                backCover.Show();
                magn.Show();
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
            uploadImgur(enmScreenCaptureMode.Window);
        }
        private void UploadImgurScreen()
        {
            uploadImgur(enmScreenCaptureMode.Screen);
        }
        private void uploadImgur(enmScreenCaptureMode mode)
        {
            playSound("snip.wav");
            Bitmap result = cap.Capture(mode);
            
            
            if(Properties.Settings.Default.saveImageAtAll)
            {
                Random rnd = new Random();
                int rndom = rnd.Next(222, 999);
                DateTime timeCreated = DateTime.Now;
                string name = timeCreated.Year.ToString("0000") +
                    timeCreated.Month.ToString("00") +
                        timeCreated.Day.ToString("00") +
                        timeCreated.Hour.ToString("00") +
                        timeCreated.Minute.ToString("00") +
                        timeCreated.Second.ToString("00") +
                        rndom.ToString("000");

                result.Save(Properties.Settings.Default.SaveImagesHere + @"\" + name + ".png");
                result.Dispose();
            }
            result.Save(System.IO.Path.GetTempPath() + "screenshot.png");
            result.Dispose();
            uploadImageFile(System.IO.Path.GetTempPath() + "screenshot.png");
        }

        private void uploadImageFile(string filePath, bool edit = false)
        {
            if(edit)
            {
                imageEditor editor = new imageEditor(filePath);
                editor.ShowDialog();
                filePath = System.IO.Path.GetTempPath() + "screenshot_edited.png";
            }
            if (File.Exists(System.IO.Path.GetTempPath() + "screenshot_edited.png"))
            {
                string url = (string)cap.UploadImage(filePath);
                if (url == null)
                {
                    Clipboard.SetText(url);
                    notifyIcon1.ShowBalloonTip(500, "imageDeCap", "upload to imgur failed!", ToolTipIcon.Error);
                    playSound("error.wav");
                }
                else
                {
                    notifyIcon1.ShowBalloonTip(500, "imageDeCap", "imgur URL copied to clipboard!", ToolTipIcon.Info);
                    playSound("upload.wav");
                    Clipboard.SetText(url);
                    addToLinks(url);
                }
            }
        }
        private void uploadPastebin(string text)
        {
            playSound("snip.wav");
            string pasteBinResult = cap.Send(text);
            if (pasteBinResult != null)
            {
                Clipboard.SetText(pasteBinResult);
                notifyIcon1.ShowBalloonTip(500, "imageDeCap", "Pastebin link placed in clipboard!", ToolTipIcon.Info);
                playSound("upload.wav");
                addToLinks(pasteBinResult);
            }
            else
            {
                notifyIcon1.ShowBalloonTip(500, "imageDeCap", "upload to pastebin failed!", ToolTipIcon.Error);
                playSound("error.wav");
            }
        }
        private void setBox(boxOfWhy box)
        {
            box.Show();
            box.BackColor = Color.Red;
            box.Opacity = 0.5;
            box.SetBounds(0, 0, 0, 0);
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
            Assembly assembly = Assembly.GetExecutingAssembly();
            SoundPlayer sp = new SoundPlayer(assembly.GetManifestResourceStream("imageDeCap." + soundName));
            sp.Play();
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
                    backCover.Close();

                    topBox.Hide();
                    bottomBox.Hide();
                    leftBox.Hide();
                    rightBox.Hide();
                    if (Width > 0 && Height > 0)
                    {
                        playSound("snip.wav");
                        Bitmap result = cap.Capture(enmScreenCaptureMode.Bounds, X, Y, Width, Height);

                        if (Properties.Settings.Default.saveImageAtAll)
                        {
                            Random rnd = new Random();
                            int rndom = rnd.Next(222, 999);
                            DateTime timeCreated = DateTime.Now;
                            string name = timeCreated.Year.ToString("0000") +
                                timeCreated.Month.ToString("00") +
                                    timeCreated.Day.ToString("00") +
                                    timeCreated.Hour.ToString("00") +
                                    timeCreated.Minute.ToString("00") +
                                    timeCreated.Second.ToString("00") +
                                    rndom.ToString("000");

                            result.Save(Properties.Settings.Default.SaveImagesHere + @"\" + name + ".png");
                            result.Dispose();
                        }
                        File.Delete(System.IO.Path.GetTempPath() + "screenshot.png");
                        result.Save(System.IO.Path.GetTempPath() + "screenshot.png");
                        result.Dispose();
                        uploadImageFile(System.IO.Path.GetTempPath() + "screenshot.png", true);
                    }
                    else
                    {

                    }
                    isTakingSnapshot = false;
                }
                else//keyDown
                {
                    tempX = Cursor.Position.X;
                    tempY = Cursor.Position.Y;
                }
            }

            if(MouseButtons == MouseButtons.Left)
            {
                topBox.SetBounds(X, Y, Width, 0);
                bottomBox.SetBounds(X, Height + Y, Width, 0);
                leftBox.SetBounds(X, Y, 0, Height);
                rightBox.SetBounds(Width + X, Y, 0, Height);

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

        private void button6_Click(object sender, EventArgs e)
        {   
            Random rnd = new Random();

            int rndom = rnd.Next(222, 999);
            DateTime timeCreated = DateTime.Now;
            string name = timeCreated.Year.ToString("0000") +
                timeCreated.Month.ToString("00") +
                    timeCreated.Day.ToString("00") +
                    timeCreated.Hour.ToString("00") +
                    timeCreated.Minute.ToString("00") +
                    timeCreated.Second.ToString("00") +
                    rndom.ToString("000");

                File.Copy("", "" + name + ".png");
            
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }



        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            props.Show();
        }











    }
}
