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
        GlobalKeyboardHook gHook; 

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
            props = new SettingsWindow(this);

            gHook = new GlobalKeyboardHook(); // Create a new GlobalKeyboardHook
            gHook.KeyDown += new KeyEventHandler(gHook_KeyDown);// Declare a KeyDown Event
            foreach (Keys key in Enum.GetValues(typeof(Keys)))// Add the keys you want to hook to the HookedKeys list
                gHook.HookedKeys.Add(key);

            gHook.KeyUp += new KeyEventHandler(gHook_KeyUp);// Declare a KeyDown Event
            foreach (Keys key in Enum.GetValues(typeof(Keys)))// Add the keys you want to hook to the HookedKeys list
                gHook.HookedKeys.Add(key);



            this.ShowInTaskbar = false;
            this.Opacity = 0.0f;

            listBox1.AllowDrop = true;
            listBox1.DragEnter += new DragEventHandler(Form1_DragEnter);
            listBox1.DragDrop += new DragEventHandler(Form1_DragDrop);
        }
        bool globalCtrlIsBeingHeldDown = false;
        bool globalShiftIsBeingHeldDown = false;
        // Handle the KeyDown Event
        public void gHook_KeyDown(object sender, KeyEventArgs e)
        {
            //textBox1.Text += " dn_" + (e.KeyValue).ToString();
            if (e.KeyValue == 162)
            {
                globalCtrlIsBeingHeldDown = true;
            }
            if (e.KeyValue == 160)
            {
                globalShiftIsBeingHeldDown = true;
            }
            if (globalCtrlIsBeingHeldDown && globalShiftIsBeingHeldDown)
            {
                if (e.KeyValue == 50)//Ctrl+Shift+2
                {
                    UploadPastebinClipboard();
                }
                if (e.KeyValue == 51)//Ctrl+Shift+3
                {
                    UploadImgurScreen();
                }
                if (e.KeyValue == 52)//Ctrl+Shift+4
                {
                    UploadToImgurBounds();
                }
                if (e.KeyValue == 53)//Ctrl+Shift+5
                {
                    UploadImgurWindow();
                }
            }
        }
        // Handle the KeyUp Event
        public void gHook_KeyUp(object sender, KeyEventArgs e)
        {
            //textBox1.Text += " up_" + (e.KeyValue).ToString();
            if (e.KeyValue == 162)
            {
                globalCtrlIsBeingHeldDown = false;
            }
            if (e.KeyValue == 160)
            {
                globalShiftIsBeingHeldDown = false;
            }
        }

        /*
        public void setHotkey(Keys modifier, Keys key, HotkeyPressedCb func)
        {
            hook.Dispose();
            Modifier mod = (modifier.HasFlag(Keys.Control) ? Modifier.Ctrl : Modifier.None) |
                (modifier.HasFlag(Keys.Shift) ? Modifier.Shift : Modifier.None) |
                (modifier.HasFlag(Keys.Alt) ? Modifier.Alt : Modifier.None);
            hook.registerHotkey(mod, key, func);
        }*/

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

        bool pushThroughCancel = false;
        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (!pushThroughCancel)
                {
                    e.Cancel = true;
                }
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
            actuallyCloseTheProgram();
        }
        private void actuallyCloseTheProgram()
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
            props.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void UploadPastebinClipboard()
        {
            uploadPastebin(Clipboard.GetText());
        }

        Magnificator magn;
        bool isTakingSnapshot = false;
        private void UploadToImgurBounds()
        {
            // prevent blackening
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
            // save unedited capture
            if (Properties.Settings.Default.saveImageAtAll && Directory.Exists(Properties.Settings.Default.SaveImagesHere))
            {
                string name = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                File.Copy(filePath, Properties.Settings.Default.SaveImagesHere + @"\" + name + ".png");
            }
            if(edit)
            {
                imageEditor editor = new imageEditor(filePath);
                editor.ShowDialog();

                filePath = System.IO.Path.GetTempPath() + "screenshot_edited.png";                
            }
            else
            {
                if(File.Exists(System.IO.Path.GetTempPath() + "screenshot_edited.png"))
                {
                    File.Delete(System.IO.Path.GetTempPath() + "screenshot_edited.png");
                }
                File.Copy(filePath, System.IO.Path.GetTempPath() + "screenshot_edited.png");
                filePath = System.IO.Path.GetTempPath() + "screenshot_edited.png";      
            }

            if (File.Exists(System.IO.Path.GetTempPath() + "screenshot_edited.png"))
            {
                string url = (string)cap.UploadImage(filePath);
                if (url == null)
                {
                    setClipboard(url);
                    notifyIcon1.ShowBalloonTip(500, "imageDeCap", "Upload to imgur failed!", ToolTipIcon.Error);
                    playSound("error.wav");
                }
                else
                {
                    if (Properties.Settings.Default.CopyLinksToClipboard)
                    {
                        notifyIcon1.ShowBalloonTip(500, "imageDeCap", "Imgur URL copied to clipboard!", ToolTipIcon.Info);
                    }
                    else
                    {
                        notifyIcon1.ShowBalloonTip(500, "imageDeCap", "Upload complete!", ToolTipIcon.Info);
                    }
                    playSound("upload.wav");
                    setClipboard(url);
                    addToLinks(url);
                }
            }
        }

        private void setClipboard(string text)
        {
            if(Properties.Settings.Default.CopyLinksToClipboard)
            {
                if (text != null)
                {
                    Clipboard.SetText(text);
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
            playSound("snip.wav");
            string pasteBinResult = cap.Send(text);
            if (pasteBinResult != null)
            {
                setClipboard(pasteBinResult);
                if (Properties.Settings.Default.CopyLinksToClipboard)
                {
                    notifyIcon1.ShowBalloonTip(500, "imageDeCap", "Pastebin link placed in clipboard!", ToolTipIcon.Info);
                }
                else
                {
                    notifyIcon1.ShowBalloonTip(500, "imageDeCap", "Upload complete!", ToolTipIcon.Info);
                }
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
                    backCover.Close();

                    topBox.Hide();
                    bottomBox.Hide();
                    leftBox.Hide();
                    rightBox.Hide();
                    if (Width > 0 && Height > 0)
                    {
                        playSound("snip.wav");
                        Bitmap result = cap.Capture(enmScreenCaptureMode.Bounds, X, Y, Width, Height);


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

        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            props.Show();
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











    }
}
