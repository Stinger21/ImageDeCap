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
using System.IO;
using System.Text.RegularExpressions;

using System.Collections.Specialized;
using System.ComponentModel;

using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API;
using Imgur.API.Models;

namespace imageDeCap
{
    public enum enmScreenCaptureMode
    {
        Screen,
        Window,
        Bounds
    }

    public class ScreenCapturer
    {

        string ClientId = "da05117bbfa9bda";

        public void UploadImage(object sender, DoWorkEventArgs e)
        {
            byte[] FileData = (byte[])e.Argument;
        
            try
            {
                var client = new ImgurClient(ClientId);
                var endpoint = new ImageEndpoint(client);
                IImage image = endpoint.UploadImageBinaryAsync(FileData).GetAwaiter().GetResult();
                e.Result = (image.Link, FileData);
            }
            catch (ImgurException imgurEx)
            {
                e.Result = ("failed, " + imgurEx.Message, FileData);
            }
        }


        public void uploadToFTP(object sender, DoWorkEventArgs e)
        {
            object[] arguments = (object[])e.Argument;
            string url = (string)arguments[0];
            string username = (string)arguments[1];
            string password = (string)arguments[2];
            byte[] filedata = (byte[])arguments[3];
            string filename = (string)arguments[4];

            // Get the object used to communicate with the server.

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url + (url.EndsWith("/") ? "" : "/") + filename);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential(username, password);

            // Copy the contents of the file to the request stream.
            byte[] fileContents = filedata;
            request.ContentLength = fileContents.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);

            response.Close();
        }

        //private string ILoginURL = "http://pastebin.com/api/api_login.php";
        private string IPostURL = "http://pastebin.com/api/api_post.php";
        private string IDevKey = "4d1c2c0bb6fa2e5c1403cedccc50bfd5";
        private string IUserKey = null;

        public void Send(object sender, DoWorkEventArgs e)
        {
            string IBody = (string)e.Argument;
            string ISubj = Preferences.PastebinSubjectLine;

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
                int cursorX = 0;
                int cursorY = 0;
                Bitmap cursorBMP = CaptureCursor(ref cursorX, ref cursorY);
                if (cursorBMP != null)
                {
                    g.DrawImage(cursorBMP, new Rectangle(cursorX - Program.ImageDeCap.X, cursorY - Program.ImageDeCap.Y, cursorBMP.Width, cursorBMP.Height));
                    g.Flush();
                }
            }
            return result;
        }


        static Bitmap CaptureCursor(ref int x, ref int y)
        {
            Bitmap bmp;
            IntPtr hicon;
            Win32Stuff.CURSORINFO ci = new Win32Stuff.CURSORINFO();
            Win32Stuff.ICONINFO icInfo;
            ci.cbSize = Marshal.SizeOf(ci);
            if (Win32Stuff.GetCursorInfo(out ci))
            {
                if (ci.flags == Win32Stuff.CURSOR_SHOWING)
                {
                    hicon = Win32Stuff.CopyIcon(ci.hCursor);
                    if (Win32Stuff.GetIconInfo(hicon, out icInfo))
                    {
                        x = ci.ptScreenPos.x - ((int)icInfo.xHotspot);
                        y = ci.ptScreenPos.y - ((int)icInfo.yHotspot);
                        Icon ic = Icon.FromHandle(hicon);
                        bmp = ic.ToBitmap();

                        return bmp;
                    }
                }
            }
            return null;
        }
        


        public Point CursorPosition
        {
            get;
            protected set;
        }
    }
    


    class Win32Stuff
    {

        #region Class Variables

        public const int SM_CXSCREEN = 0;
        public const int SM_CYSCREEN = 1;

        public const Int32 CURSOR_SHOWING = 0x00000001;

        [StructLayout(LayoutKind.Sequential)]
        public struct ICONINFO
        {
            public bool fIcon;         // Specifies whether this structure defines an icon or a cursor. A value of TRUE specifies
            public Int32 xHotspot;     // Specifies the x-coordinate of a cursor's hot spot. If this structure defines an icon, the hot
            public Int32 yHotspot;     // Specifies the y-coordinate of the cursor's hot spot. If this structure defines an icon, the hot
            public IntPtr hbmMask;     // (HBITMAP) Specifies the icon bitmask bitmap. If this structure defines a black and white icon,
            public IntPtr hbmColor;    // (HBITMAP) Handle to the icon color bitmap. This member can be optional if this
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public Int32 x;
            public Int32 y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CURSORINFO
        {
            public Int32 cbSize;        // Specifies the size, in bytes, of the structure.
            public Int32 flags;         // Specifies the cursor state. This parameter can be one of the following values:
            public IntPtr hCursor;          // Handle to the cursor.
            public POINT ptScreenPos;       // A POINT structure that receives the screen coordinates of the cursor.
        }

        #endregion


        #region Class Functions

        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", EntryPoint = "GetDC")]
        public static extern IntPtr GetDC(IntPtr ptr);

        [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
        public static extern int GetSystemMetrics(int abc);

        [DllImport("user32.dll", EntryPoint = "GetWindowDC")]
        public static extern IntPtr GetWindowDC(Int32 ptr);

        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);


        [DllImport("user32.dll", EntryPoint = "GetCursorInfo")]
        public static extern bool GetCursorInfo(out CURSORINFO pci);

        [DllImport("user32.dll", EntryPoint = "CopyIcon")]
        public static extern IntPtr CopyIcon(IntPtr hIcon);

        [DllImport("user32.dll", EntryPoint = "GetIconInfo")]
        public static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);


        #endregion
    }
}
