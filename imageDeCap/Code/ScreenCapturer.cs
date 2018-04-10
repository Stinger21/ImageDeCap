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

using System.Net.Http;
using Newtonsoft.Json.Linq;

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
        public void UploadImage_Imgur(object sender, DoWorkEventArgs e)
        {
            byte[] FileData = (byte[])e.Argument;
            
            try
            {
                var client = new ImgurClient("da05117bbfa9bda");
                var endpoint = new ImageEndpoint(client);
                IImage image = endpoint.UploadImageBinaryAsync(FileData).GetAwaiter().GetResult();
                e.Result = (image.Link, FileData);
            }
            catch (ImgurException imgurEx)
            {
                e.Result = ("failed, " + imgurEx.Message, FileData);
            }
        }
        //public void UploadImage_Imgur(object sender, DoWorkEventArgs e)
        //{
        //    byte[] FileData = (byte[])e.Argument;
        //    /*
        //    curl --request POST \
        //    --url https://api.imgur.com/3/image \
        //    --header 'Authorization: Client-ID {{clientId}}' \
        //    --header 'content-type: multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW' \
        //    --form image=R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7
        //    */
        //    string url = nameof(FileData);
        //
        //
        //    var content = new MultipartFormDataContent($"{DateTime.UtcNow.Ticks}")
        //    {
        //        {new StringContent("file"), "type"},
        //        {new ByteArrayContent(FileData), nameof(FileData)}
        //    };
        //
        //    var request = new HttpRequestMessage(HttpMethod.Post, url)
        //    {
        //        Content = content
        //    };
        //    HttpClient client = new HttpClient();
        //    //client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", apiClient.OAuth2Token != null : $"Client-ID {apiClient.ClientId}");
        //    client.DefaultRequestHeaders.AddWithoutValidation("Authorization", "Client-ID da05117bbfa9bda");
        //    HttpResponseMessage message = client.SendAsync(request).GetAwaiter().GetResult();
        //
        //
        //
        //}

        public void UploadGif_Webmshare(object sender, DoWorkEventArgs e)
        {
            byte[] FileData = (byte[])e.Argument;
            var result = Webmshare.Upload(FileData);
            if (result.UploadSuccessfull)
            {
                e.Result = (result.Message, FileData);
            }
            else
            {
                e.Result = ("failed, " + result.Message, FileData);
            }
        }

        public void UploadGif_Gfycat(object sender, DoWorkEventArgs e)
        {
            byte[] FileData = (byte[])e.Argument;
            var result = Gfycat.Upload(FileData);
            if (result.UploadSuccessfull)
            {
                e.Result = (result.Message, FileData);
            }
            else
            {
                e.Result = ("failed, " + result.Message, FileData);
            }
        }

        public void UploadGif_Imgur(object sender, DoWorkEventArgs e)
        {
            //byte[] FileData = (byte[])e.Argument;
            //
            //
            //var client = new ImgurClient("da05117bbfa9bda");
            //var endpoint = new ImageEndpoint(client);
            //
            //endpoint.UploadImageUrlAsync();
            //var endpoint = new ImageEndpoint(client);
            //IImage image = endpoint.UploadImageBinaryAsync(FileData).GetAwaiter().GetResult();
            //e.Result = (image.Link, FileData);

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

            //Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);

            response.Close();
        }

        public void UploadPastebin(object sender, DoWorkEventArgs e)
        {
            //private string ILoginURL = "http://pastebin.com/api/api_login.php";
            string IPostURL = "http://pastebin.com/api/api_post.php";
            string IDevKey = "4d1c2c0bb6fa2e5c1403cedccc50bfd5";
            string IUserKey = null;

            string IBody = (string)e.Argument;
            string ISubj = Preferences.PastebinSubjectLine;

            if (string.IsNullOrEmpty(IBody.Trim())) { e.Result = "failed"; }
            if (string.IsNullOrEmpty(ISubj.Trim())) { e.Result = "failed"; }

            NameValueCollection IQuery = new NameValueCollection();

            IQuery.Add("api_dev_key", IDevKey);
            IQuery.Add("api_option", "paste");
            IQuery.Add("api_paste_code", IBody);
            IQuery.Add("api_paste_private", "0");
            IQuery.Add("api_paste_name", ISubj);
            IQuery.Add("api_paste_expire_date", "N");
            IQuery.Add("api_paste_format", "text");
            IQuery.Add("api_user_key", IUserKey);

            string IResponse = "";

            using (WebClient IClient = new WebClient())
            {
                try
                {
                    IResponse = Encoding.UTF8.GetString(IClient.UploadValues(IPostURL, IQuery));
                }
                catch (Exception ee)
                {
                    IResponse = "failed, " + ee.Message;
                }
            }
            e.Result = IResponse;
        }

        public Bitmap Capture(enmScreenCaptureMode screenCaptureMode = enmScreenCaptureMode.Window, int X = 0, int Y = 0, int Width = 0, int Height = 0, bool CaptureMouse = false)
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
                var foregroundWindowsHandle = Win32Stuff.GetForegroundWindow();
                var rect = new Win32Stuff.Rect();
                Win32Stuff.GetWindowRect(foregroundWindowsHandle, ref rect);
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
                if (CaptureMouse)
                {
                    int cursorX = 0;
                    int cursorY = 0;
                    Bitmap cursorBMP = CaptureCursor(ref cursorX, ref cursorY);
                    if (cursorBMP != null)
                    {
                        g.DrawImage(cursorBMP, new Rectangle(cursorX - Program.ImageDeCap.X, cursorY - Program.ImageDeCap.Y, cursorBMP.Width, cursorBMP.Height));
                        g.Flush();
                    }
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
                        if (ic.Size.Height == 0 || ic.Size.Width == 0)
                        {
                            return null;
                        }
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

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        #endregion


        #region Class Functions

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);
        
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

    // Good fucking god, web things are my cryptonite. Why does this all have to be so needlessly complicated. 

    public static class Webmshare
    {
        public static (bool UploadSuccessfull, string Message) Upload(byte[] FileData)
        {
            /*
            curl -i -X POST \
            -H "Content-Type:multipart/form-data" \
            -F "file=@\"./some_file.webm\";type=video/webm;filename=\"some_file.webm\"" \
            -F "expiration=1" \
            -F "public=0" \
            -F "title=Some title goes here ;)" \
            -F "autoplay=1" \
            -F "loop=1" \
            -F "muted=1" \
            'https://webmshare.com/api/upload'
            */


            HttpClient client = new HttpClient();
            MultipartFormDataContent form = new MultipartFormDataContent();

            form.Add(new StringContent("0"), "expiration");
            form.Add(new StringContent("0"), "public");
            form.Add(new StringContent("1"), "autoplay");
            form.Add(new StringContent("1"), "loop");
            form.Add(new StringContent("1"), "muted");
            form.Add(new ByteArrayContent(FileData), "file", "something.webm");

            var response = client.PostAsync("http://webmshare.com/api/upload", form).GetAwaiter().GetResult();

            var responseString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            var objects = JObject.Parse(responseString);
            
            string result = "";
            try
            {
                result = objects.GetValue("id").ToString();
                return (true, "https://webmshare.com/play/" + result);
            }
            catch
            {
                result = responseString;
                return (false, "failed, " + result);
            }
        }
    }

    public static class Gfycat
    {
        public static (bool UploadSuccessfull, string Message) Upload(byte[] FileData)
        {
            var createResponse = Create(new GfycatCreateRequest() { noMd5 = true }).GetAwaiter().GetResult();

            Upload(createResponse.GfyName, FileData).GetAwaiter().GetResult();
            int notfounds = 0;
            while (true)
            {
                var statusResponse = Status(createResponse.GfyName).GetAwaiter().GetResult();
                //Console.WriteLine("Gfycat " + FileData.Length + ", " + statusResponse.Task + ", " + statusResponse.Progress);

                if (statusResponse.Task == "NotFoundo")
                {
                    if (notfounds > 10)
                        return (false, statusResponse.Task);
                    notfounds++;
                }
                else if (statusResponse.Task == "encoding") { }
                else if (statusResponse.Task == "complete")
                    return (true, "https://gfycat.com/" + statusResponse.GfyName);
                else
                    return (false, statusResponse.Task);

                System.Threading.Thread.Sleep(1000);
            }
        }

        static async Task<GfycatCreateResponse> Create(GfycatCreateRequest request)
        {
            HttpClient client = new HttpClient();
            var response = await client.PostAsJsonAsync("https://api.gfycat.com/v1/gfycats", request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<GfycatCreateResponse>();
        }

        static async Task Upload(string gfyname, byte[] fileData)
        {
            HttpClient client = new HttpClient();
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(new StringContent(gfyname), "key", "key");
                formData.Add(new ByteArrayContent(fileData), "file", gfyname);

                var response = await client.PostAsync("https://filedrop.gfycat.com/", formData);

                response.EnsureSuccessStatusCode();
            }
        }

        static async Task<GfycatStatusResponse> Status(string gfyname)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync($"https://api.gfycat.com/v1/gfycats/fetch/status/{gfyname}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<GfycatStatusResponse>();
        }

        struct GfycatCreateRequest
        {
            public bool noMd5 { get; set; }
        }

        struct GfycatCreateResponse
        {
            public bool IsOk { get; set; }
            public string GfyName { get; set; }
            public string Secret { get; set; }
            public string UploadType { get; set; }
        }

        struct GfycatStatusResponse
        {
            public string Task { get; set; }
            public string GfyName { get; set; }
            public double Progress { get; set; }
        }
    }
    
}
