using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//me
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;

using System.Net;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

using System.Collections.Specialized;
using System.ComponentModel;

namespace imageDeCap
{
    public enum enmScreenCaptureMode
    {
        Screen,
        Window,
        Bounds
    }

    class ScreenCapturer
    {
        public void recordScreen()
        {
            
            // Get instance of the ScreenCapture object
            //var screenCapture = Windows.Media.Capture.ScreenCapture.GetForCurrentView();
        }

        string ClientId = "da05117bbfa9bda";
        public void UploadImage(object sender, DoWorkEventArgs e)
        {
            
            string filepath = (string)e.Argument;

            WebClient w = new WebClient();
            w.Headers.Add("Authorization", "Client-ID " + ClientId);
            System.Collections.Specialized.NameValueCollection Keys = new System.Collections.Specialized.NameValueCollection();
            try
            {
                Keys.Add("image", Convert.ToBase64String(File.ReadAllBytes(filepath)));
                byte[] responseArray = w.UploadValues("https://api.imgur.com/3/image", Keys);
                dynamic result = Encoding.ASCII.GetString(responseArray); 
                Regex reg = new System.Text.RegularExpressions.Regex("link\":\"(.*?)\"");
                Match match = reg.Match(result);

                string url = match.ToString().Replace("link\":\"", "").Replace("\"", "").Replace("\\/", "/");
                e.Result = url;
                //return url;
            }
            catch (Exception s)
            {
                //MessageBox.Show("Something went wrong. " + s.Message);
                //return null;
                e.Result = "failed, " + s.Message;
            }
        }

        public void uploadToFTP(object sender, DoWorkEventArgs e)
        {
            string[] arguments = (string[])e.Argument;
            string url = arguments[0];
            string username = arguments[1];
            string password = arguments[2];
            string filepath = arguments[3];
            string filename = arguments[4];

            // Get the object used to communicate with the server.

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url + (url.EndsWith("/") ? "" : "/") + filename);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential(username, password);

            // Copy the contents of the file to the request stream.
            byte[] fileContents = File.ReadAllBytes(filepath);
            request.ContentLength = fileContents.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);

            response.Close();
        }

        private string ILoginURL = "http://pastebin.com/api/api_login.php";
        private string IPostURL = "http://pastebin.com/api/api_post.php";
        private string IDevKey = "4d1c2c0bb6fa2e5c1403cedccc50bfd5";
        private string IUserKey = null;

        public void Send(object sender, DoWorkEventArgs e)
        {
            string IBody = (string)e.Argument;
            string ISubj = Properties.Settings.Default.PastebinSubjectLine;

            if (string.IsNullOrEmpty(IBody.Trim())) { e.Result = "failed"; }
            if (string.IsNullOrEmpty(ISubj.Trim())) { e.Result = "failed"; }

            NameValueCollection IQuery = new NameValueCollection();

            IQuery.Add("api_dev_key",           IDevKey);
            IQuery.Add("api_option",            "paste");
            IQuery.Add("api_paste_code",        IBody);
            IQuery.Add("api_paste_private",     "0");
            IQuery.Add("api_paste_name",        ISubj);
            IQuery.Add("api_paste_expire_date", "N");
            IQuery.Add("api_paste_format",      "text");
            IQuery.Add("api_user_key",          IUserKey);

            string IResponse = "";

            using (WebClient IClient = new WebClient())
            {
                try
                {
                    IResponse = Encoding.UTF8.GetString(IClient.UploadValues(IPostURL, IQuery));
                }
                catch(Exception ee)
                {
                    IResponse = "failed, " + ee.Message;
                }
            }
            e.Result = IResponse;
        }




        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public Bitmap Capture(enmScreenCaptureMode screenCaptureMode = enmScreenCaptureMode.Window, int X = 0, int Y = 0, int Width = 0, int Height = 0)
        {
            Rectangle bounds;

            if (screenCaptureMode == enmScreenCaptureMode.Screen)
            {
                //bounds = new Rectangle(0, 0, SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
                bounds = SystemInformation.VirtualScreen;
                CursorPosition = Cursor.Position;
            }
            else if (screenCaptureMode == enmScreenCaptureMode.Window)
            {
                var foregroundWindowsHandle = GetForegroundWindow();
                var rect = new Rect();
                GetWindowRect(foregroundWindowsHandle, ref rect);
                bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
                CursorPosition = new Point(Cursor.Position.X - rect.Left, Cursor.Position.Y - rect.Top);
            }
            else
            {
                bounds = new Rectangle(X, Y, Width, Height);
            }

            var result = new Bitmap(bounds.Width, bounds.Height);

            using (var g = Graphics.FromImage(result))
            {
                g.Clear(Color.Black);
                g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size, CopyPixelOperation.SourceCopy);
            }

            return result;
        }

        public Point CursorPosition
        {
            get;
            protected set;
        }
    }
}
