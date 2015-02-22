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


namespace screenshotsPls
{
    public partial class Form1 : Form
    {
        [DllImport("User32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("User32.dll")]
        static extern void ReleaseDC(IntPtr dc);


        private KeyMessageFilter m_filter = new KeyMessageFilter();


        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem3;

        public Form1()
        {
            InitializeComponent();

            //hotkeys
            //HotKey wa = new HotKey(Keys.D2, HotKey.KeyModifiers.Shift | HotKey.KeyModifiers.Control, UploadToPasteBin, 0);

            //new HotKey(Keys.D3, HotKey.KeyModifiers.Shift | HotKey.KeyModifiers.Control, UploadImgurScreen, 1);
            //new HotKey(Keys.D4, HotKey.KeyModifiers.Shift | HotKey.KeyModifiers.Control, UploadToImgurBounds, 2);
            //new HotKey(Keys.D5, HotKey.KeyModifiers.Shift | HotKey.KeyModifiers.Control, UploadImgurWindow, 3);

            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();

            // Initialize contextMenu1
            this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { this.menuItem1, this.menuItem2, this.menuItem3 });

            // Initialize menuItem1
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "Exit";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);

            // Initialize menuItem2
            this.menuItem2.Index = 0;
            this.menuItem2.Text = "Settings";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);

            // Initialize menuItem2
            this.menuItem3.Index = 0;
            this.menuItem3.Text = "Contact / Bugs";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);

            notifyIcon1.ContextMenu = contextMenu1;
            notifyIcon1.Visible = true;

            //this.ControlBox = false;

        }
        bool ctrlDown = false;
        bool shiftDown = false;
        private void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            ctrlDown = e.KeyCode == Keys.ControlKey;
            shiftDown = e.KeyCode == Keys.ShiftKey;
            if (e.KeyCode == Keys.D4 && ctrlDown && shiftDown)
            {
                MessageBox.Show("wah!");
            }
            //textBoxLog.AppendText(string.Format("KeyDown - {0}\n", e.KeyCode));
            //textBoxLog.ScrollToCaret();
        }

        private void HookManager_KeyUp(object sender, KeyEventArgs e)
        {
            ctrlDown = e.KeyCode == Keys.ControlKey;
            shiftDown = e.KeyCode == Keys.ShiftKey;
            //textBoxLog.AppendText(string.Format("KeyUp - {0}\n", e.KeyCode));
            //textBoxLog.ScrollToCaret();
        }


        private void HookManager_KeyPress(object sender, KeyPressEventArgs e)
        {
            //textBoxLog.AppendText(string.Format("KeyPress - {0}\n", e.KeyChar));
            //textBoxLog.ScrollToCaret();
        }

        private void menuItem1_Click(object Sender, EventArgs e)
        {
            // Close the form, which closes the application. 
            //this.Close();
            Application.Exit();
        }
        private void menuItem2_Click(object Sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }
        private void menuItem3_Click(object Sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.mattwestphal.com");
        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
            } 
            
        }





        bool hasShownNotification = false;
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                if (!hasShownNotification)
                {
                    notifyIcon1.ShowBalloonTip(1000, "imageDeCap", "Is still running in the background", ToolTipIcon.None);
                    hasShownNotification = true;
                }

                this.ShowInTaskbar = false;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
            /*
            Bitmap result = cap.Capture();
            result.Save("C:\\screenshot.png");
            string url = (string)cap.UploadImage("C:\\screenshot.png");

            textBox1.Text = url;
             * */
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //UploadToPasteBin();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            //UploadToImgur();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            //textBox2.Text = Cursor.Position.X + ", " + Cursor.Position.Y;
            //textBox2.Text = SystemInformation.VirtualScreen.Width.ToString();
        }

        private void UploadToPasteBin(object sender = null, EventArgs e = null)
        {
            string pasteBinResult = cap.Send(Clipboard.GetText());
            textBox1.Text = pasteBinResult;
            if (pasteBinResult != null)
            {
                Clipboard.SetText(pasteBinResult);

                notifyIcon1.ShowBalloonTip(500, "imageDeCap", "Pastebin link placed in clipboard!", ToolTipIcon.Info);
                Assembly assembly = Assembly.GetExecutingAssembly();
                SoundPlayer sp = new SoundPlayer(assembly.GetManifestResourceStream("screenshotsPls.upload.wav"));
                sp.Play();
            }
            else
            {
                notifyIcon1.ShowBalloonTip(500, "imageDeCap", "upload to pastebin failed!", ToolTipIcon.Error);
                Assembly assembly = Assembly.GetExecutingAssembly();
                SoundPlayer sp = new SoundPlayer(assembly.GetManifestResourceStream("screenshotsPls.error.wav"));
                sp.Play();
            }

        }

        private void UploadToImgurBounds()
        {
            //back cover used for pulling cursor position into updateSelectedArea()
            completeCover backCover = new completeCover(this);
            backCover.Show();
            backCover.SetBounds(0, 0, SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
            backCover.BackColor = Color.Black;
            backCover.Opacity = 0.01;

            //X = 0;
            //Y = 0;
            //tempX = 0;
            //tempY = 0;
            //Width = 0;
            //Height = 0;
            setBox(topBox);
            setBox(leftBox);
            setBox(bottomBox);
            setBox(rightBox);
        }
        private void UploadImgurWindow(object sender, EventArgs e)
        {
            uploadImgur(enmScreenCaptureMode.Window);
        }
        private void UploadImgurScreen(object sender, EventArgs e)
        {
            uploadImgur(enmScreenCaptureMode.Screen);
        }
        private void uploadImgur(enmScreenCaptureMode mode)
        {
            Bitmap result = cap.Capture(mode);
            result.Save("C:\\screenshot.png");
            string url = (string)cap.UploadImage("C:\\screenshot.png");
            textBox1.Text = url;
            if (url == null)
            {
                Clipboard.SetText(url);
                notifyIcon1.ShowBalloonTip(500, "imageDeCap", "upload to imgur failed!", ToolTipIcon.Error);

                Assembly assembly = Assembly.GetExecutingAssembly();
                SoundPlayer sp = new SoundPlayer(assembly.GetManifestResourceStream("screenshotsPls.error.wav"));
                sp.Play();
            }
            else
            {
                notifyIcon1.ShowBalloonTip(500, "imageDeCap", "imgur URL copied to clipboard!", ToolTipIcon.Info);//annoying-ass opoup

                Assembly assembly = Assembly.GetExecutingAssembly();
                SoundPlayer sp = new SoundPlayer(assembly.GetManifestResourceStream("screenshotsPls.upload.wav"));//play FUCKING notification
                sp.Play();

                Clipboard.SetText(url);//cliboardfgbhloöijunsd
            }
        }


        private void setBox(boxOfWhy box)
        {
            box.Show();
            box.BackColor = Color.Red;
            box.Opacity = 0.5;
            bottomBox.SetBounds(0, 0, 0, 0);
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

        public void updateSelectedArea(completeCover backCover, bool keyPressed, bool escapePressed)//this thing is essentially a fucking frame-loop.
        {
           
            if (wasPressed != (MouseButtons == MouseButtons.Left))
            {
                if (wasPressed)//keyUp
                {
                }
                else//keyDown
                {
                    tempX = Cursor.Position.X;
                    tempY = Cursor.Position.Y;
                }
            }
            if(MouseButtons == MouseButtons.Left)
            {
                topBox.SetBounds(0, Y, SystemInformation.VirtualScreen.Width, 0);
                bottomBox.SetBounds(0, Height + Y, SystemInformation.VirtualScreen.Width, 0);

                leftBox.SetBounds(X, 0, 0, SystemInformation.VirtualScreen.Height);
                rightBox.SetBounds(Width + X, 0, 0, SystemInformation.VirtualScreen.Height);

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
                backCover.Close();

                topBox.Hide();
                bottomBox.Hide();
                leftBox.Hide();
                rightBox.Hide();
            }

            if(keyPressed)
            {
                backCover.Close();

                topBox.Hide();
                bottomBox.Hide();
                leftBox.Hide();
                rightBox.Hide();

                Bitmap result = cap.Capture(enmScreenCaptureMode.Bounds, X, Y, Width, Height);
                result.Save("C:\\screenshot.png");
                string url = (string)cap.UploadImage("C:\\screenshot.png");
                textBox1.Text = url;
                if (url == null)
                {
                    Clipboard.SetText(url);
                    notifyIcon1.ShowBalloonTip(500, "imageDeCap", "upload failed!", ToolTipIcon.Error);

                    Assembly assembly = Assembly.GetExecutingAssembly();
                    SoundPlayer sp = new SoundPlayer(assembly.GetManifestResourceStream("screenshotsPls.error.wav"));
                    sp.Play();
                }
                else
                {
                    notifyIcon1.ShowBalloonTip(500, "imageDeCap", "imgur URL copied to clipboard!", ToolTipIcon.Info);//annoying-ass opoup

                    Assembly assembly = Assembly.GetExecutingAssembly();
                    SoundPlayer sp = new SoundPlayer(assembly.GetManifestResourceStream("screenshotsPls.upload.wav"));//play FUCKING notification
                    sp.Play();

                    Clipboard.SetText(url);//cliboardfgbhloöijunsd
                }

            }

            wasPressed = MouseButtons == MouseButtons.Left;

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            UploadToImgurBounds();
        }


    }
}
