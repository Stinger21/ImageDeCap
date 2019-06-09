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
using System.Threading;
using System.Diagnostics;

namespace imageDeCap
{
    public enum ScreenCaptureMode
    {
        Screen,
        Window,
        Bounds
    }

    public static class ScreenCapturer
    {

        public static CompleteCover CurrentBackCover;
        public static bool IsTakingSnapshot = false;
        
        // UPLOADING FUNCTIONS

        public static void UploadPastebinClipboard()
        {
            if (!IsTakingSnapshot)
            {
                SendKeys.SendWait("^c");
                System.Threading.Thread.Sleep(500);
                string clipboard = Clipboard.GetText();

                if (!Preferences.NeverUpload)
                {
                    Utilities.PlaySound("snip.wav");
                    BackgroundWorker bw = new BackgroundWorker();
                    bw.DoWork += Uploading.UploadPastebin;
                    bw.RunWorkerCompleted += UploadPastebinCompleted;
                    bw.RunWorkerAsync(clipboard);

                    BackgroundWorker bw2 = new BackgroundWorker();
                    bw2.DoWork += Uploading.UploadToFTP;
                    string name = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
                    bw2.RunWorkerAsync(new object[] { Preferences.FTPurl,
                                                  Preferences.FTPusername,
                                                  Preferences.FTPpassword,
                                                  Encoding.ASCII.GetBytes(clipboard),
                                                  $"{name}.txt" });
                }

                if (Preferences.SaveImages && Directory.Exists(Preferences.SaveImagesLocation))
                {
                    string name = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
                    string whereToSave = $"{Preferences.SaveImagesLocation}\\{name}.txt";
                    File.WriteAllText(whereToSave, clipboard);
                }
            }
        }

        public static void CaptureScreenRegion(bool isGif = false)
        {
            Bitmap background = ScreenCapturer.Capture(ScreenCaptureMode.Screen);
            // prevent blackening
            if (!IsTakingSnapshot)
            {
                IsTakingSnapshot = true;
                Program.hotkeysEnabled = false;
                // back cover used for pulling cursor position into updateSelectedArea()
                if (CurrentBackCover != null)
                    CurrentBackCover.Dispose();

                CurrentBackCover = new CompleteCover(isGif);
                CurrentBackCover.Show();
                CurrentBackCover.AfterShow(background, isGif);
            }
        }

        public static void UploadImageData(byte[] ImageData, Filetype imageType, bool ForceNoEdit = false, bool RMBClickForceEdit = false, Bitmap[] GifImage = null)
        {
            Program.hotkeysEnabled = true; // Enable hotkeys here again so you can take more screenshots

            if (imageType == Filetype.error)
                return;

            // Copy image to clipboard
            if (imageType != Filetype.gif)
            {
                if (Preferences.CopyImageToClipboard)
                {
                    ClipboardHandler.SetClipboardImage(ImageData);
                }
            }

            if ((Preferences.EditScreenshotAfterCapture || RMBClickForceEdit) && ForceNoEdit == false)
            {
                if (imageType == Filetype.gif)
                {
                    //int framerate = 1000 / CurrentBackCover.RecordedTime;
                    GifEditor editor = new GifEditor(GifImage, CurrentBackCover.topBox.Location.X, CurrentBackCover.topBox.Location.Y, CurrentBackCover.RecorderFramerate);
                    editor.Show();
                    editor.FormClosed += EditorDone;
                }
                else
                {
                    NewImageEditor editor = new NewImageEditor(ImageData, CurrentBackCover.topBox.Location.X, CurrentBackCover.topBox.Location.Y);
                    editor.Show();
                    editor.FormClosed += EditorDone;
                }
            }
            else
            {
                // If it's a gif make save the default :>
                NewImageEditor.EditorResult EditorResult = NewImageEditor.EditorResult.Upload;
                if(imageType == Filetype.gif)
                    EditorResult = NewImageEditor.EditorResult.Save;

                UploadImageData_AfterEdit(EditorResult, imageType, ImageData, GifImage);
            }
        }

        public static void EditorDone(object sender, EventArgs e)
        {
            Filetype f;
            NewImageEditor.EditorResult EditorResult = NewImageEditor.EditorResult.Quit;
            byte[] ImageData = null;
            Bitmap[] GifData = null;
            if (sender is NewImageEditor)
            {
                NewImageEditor editor = (NewImageEditor)sender;
                (EditorResult, ImageData) = editor.FinalFunction();
                editor.Dispose();
                f = Filetype.png;
            }
            else
            {
                GifEditor editor = (GifEditor)sender;
                (EditorResult, GifData) = editor.FinalFunction();
                editor.Dispose();
                f = Filetype.gif;
            }
            UploadImageData_AfterEdit(EditorResult, f, ImageData, GifData);
        }

        // TODO: Change this to take a path with the editorresult
        public static void UploadImageData_AfterEdit(NewImageEditor.EditorResult EditorResult, Filetype imageType, byte[] FileData, Bitmap[] GifImage)
        {
            if (EditorResult == NewImageEditor.EditorResult.Quit)
            {
                foreach (var v in CurrentBackCover.CapturedClpFrames) { v.Dispose(); }
                CurrentBackCover.CapturedClpFrames.Clear();
                return;
            }

            // Ask the user where to save before turning the array of images into a video because video compression can take a while.
            string SavePath = null;
            if (EditorResult == NewImageEditor.EditorResult.Save) // If gif, ask to save only if 
            {
                if (imageType == Filetype.gif)
                {
                    SavePath = Utilities.FileDialog(MainWindow.videoFormat);
                }
                if (imageType != Filetype.gif)
                {
                    SavePath = Utilities.FileDialog(".png");
                }
            }

            // compress video
            if (imageType == Filetype.gif)
            {
                FileData = GifEditor.VideoFromFrames(GifImage, 1000 / CurrentBackCover.RecordedTime);
                foreach (var v in CurrentBackCover.CapturedClpFrames) { v.Dispose(); }
                CurrentBackCover.CapturedClpFrames.Clear();
            }
            if (SavePath != null)
            {
                File.WriteAllBytes(SavePath, FileData);
            }

            string SaveFileName = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
            string Extension = imageType.ToString();
            if (Extension == "gif")
            {
                Extension = MainWindow.videoFormat.Replace(".", "");
            }

            if (Preferences.SaveImages)
            {
                string directory_path = Path.GetFullPath(Environment.ExpandEnvironmentVariables(Preferences.SaveImagesLocation));
                string file_path = Path.Combine(directory_path, $"{SaveFileName}.{Extension}");
                try
                {
                    Directory.CreateDirectory(directory_path);
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

            if (Preferences.BackupImages)
            {
                if (!Directory.Exists(MainWindow.BackupDirectory))
                {
                    Directory.CreateDirectory(MainWindow.BackupDirectory);
                }
                File.WriteAllBytes(MainWindow.BackupDirectory + $"\\{SaveFileName}.{Extension}", FileData);
                int i = 0;
                foreach (string file in Directory.GetFiles(MainWindow.BackupDirectory).OrderBy(f => f).Reverse())
                {
                    i++;
                    if (i > 100)
                    {
                        File.Delete(file);
                    }
                }
            }


            // Copy image to clipboard if it's not a gif
            if (imageType != Filetype.gif)
            {
                if (EditorResult == NewImageEditor.EditorResult.Clipboard)
                {
                    if (Preferences.CopyImageToClipboard)
                    {
                        ClipboardHandler.SetClipboardImage(FileData);
                    }
                }
            }

            // Upload the image
            if (EditorResult == NewImageEditor.EditorResult.Upload)
            {
                if (!Preferences.NeverUpload)
                {
                    BackgroundWorker bw = new BackgroundWorker();
                    if (imageType == Filetype.gif)
                    {
                        if (Preferences.GifTarget == "gfycat")
                        {
                            bw.DoWork += Uploading.UploadGif_Gfycat;
                        }
                        else if (Preferences.GifTarget == "imgur")
                        {
                            bw.DoWork += Uploading.UploadGif_Imgur;
                        }
                        else if (Preferences.GifTarget == "webmshare")
                        {
                            bw.DoWork += Uploading.UploadGif_Webmshare;
                        }
                    }
                    else
                    {
                        bw.DoWork += Uploading.UploadImage_Imgur;
                    }
                    bw.RunWorkerCompleted += UploadImageFileCompleted;
                    bw.RunWorkerAsync(FileData);
                }
            }
        }

        public static void UploadImageFileCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            (string url, byte[] ImageData) = (ValueTuple<string, byte[]>)e.Result;

            if (url.Contains("failed"))
            {
                ClipboardHandler.SetClipboardText(url);
                Utilities.BubbleNotification($"Upload to imgur failed! \n{url}\nAre you connected to the internet? \nis Imgur Down?", null, ToolTipIcon.Error);
                Utilities.PlaySound("error.wav");
            }
            else
            {
                if (Preferences.OpenInBrowser)
                {
                    Process.Start(url);
                }
                if (!Preferences.CopyLinksToClipboard)
                {
                    if (Preferences.DisableNotifications)
                        Utilities.BubbleNotification("Imgur URL copied to clipboard!", Program.ImageDeCap.BalloonTipClicked);
                }
                else
                {
                    if (!Preferences.DisableNotifications)
                        Utilities.BubbleNotification("Upload Complete!", Program.ImageDeCap.BalloonTipClicked);
                }

                if (!Utilities.IsWindows10() || Preferences.DisableNotifications)
                {
                    Utilities.PlaySound("upload.wav");
                }
                ClipboardHandler.SetClipboardText(url);
                Program.ImageDeCap.AddLink(url);
            }

            if (Preferences.uploadToFTP)
            {
                string name = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += Uploading.UploadToFTP;
                bw.RunWorkerAsync(new object[] { Preferences.FTPurl,
                                                 Preferences.FTPusername,
                                                 Preferences.FTPpassword,
                                                 ImageData,
                                                 name + (url.EndsWith(".png") ? ".png" : ".jpg") });
            }
        }

        public static void UploadPastebinCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string pasteBinResult = (string)e.Result;
            if (!pasteBinResult.Contains("failed"))
            {
                ClipboardHandler.SetClipboardText(pasteBinResult);
                if (Preferences.CopyLinksToClipboard)
                {
                    if (!Preferences.DisableNotifications)
                        Utilities.BubbleNotification("Pastebin link placed in clipboard!", Program.ImageDeCap.BalloonTipClicked);
                }
                else
                {
                    if (!Preferences.DisableNotifications)
                        Utilities.BubbleNotification("Upload complete!", Program.ImageDeCap.BalloonTipClicked);
                }

                if (!Utilities.IsWindows10() || Preferences.DisableNotifications)
                {
                    Utilities.PlaySound("upload.wav");
                }
                Program.ImageDeCap.AddLink(pasteBinResult);
            }
            else
            {
                Utilities.BubbleNotification($"upload to pastebin failed!\n{pasteBinResult}\nAre you connected to the internet? \nIs pastebin Down?", null, ToolTipIcon.Error);
                Utilities.PlaySound("error.wav");
            }
        }
        

        public static Bitmap Capture(ScreenCaptureMode screenCaptureMode = ScreenCaptureMode.Window, int X = 0, int Y = 0, int Width = 0, int Height = 0, bool CaptureMouse = false)
        {
            Rectangle bounds;

            if (screenCaptureMode == ScreenCaptureMode.Screen)
            {
                bounds = SystemInformation.VirtualScreen;
            }
            else if (screenCaptureMode == ScreenCaptureMode.Window)
            {
                var foregroundWindowsHandle = Win32Stuff.GetForegroundWindow();
                var rect = new Win32Stuff.Rect();
                Win32Stuff.GetWindowRect(foregroundWindowsHandle, ref rect);
                bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
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
                        g.DrawImage(cursorBMP, new Rectangle(cursorX - CurrentBackCover.SelectedRegion.X, cursorY - CurrentBackCover.SelectedRegion.Y, cursorBMP.Width, cursorBMP.Height));
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
            ci.cbSize = Marshal.SizeOf(ci);
            if (!Win32Stuff.GetCursorInfo(out ci))
                return null;

            if (ci.flags != Win32Stuff.CURSOR_SHOWING)
                return null;

            hicon = Win32Stuff.CopyIcon(ci.hCursor);
            if (!Win32Stuff.GetIconInfo(hicon, out Win32Stuff.ICONINFO icInfo))
                return null;
            
            x = ci.ptScreenPos.x - ((int)icInfo.xHotspot);
            y = ci.ptScreenPos.y - ((int)icInfo.yHotspot);
            Icon ic = Icon.FromHandle(hicon);
            if (ic.Size.Height == 0 || ic.Size.Width == 0)
                return null;

            bmp = ic.ToBitmap();
            return bmp;
        }
    }

    static class Win32Stuff
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
            MultipartFormDataContent form = new MultipartFormDataContent
            {
                { new StringContent("0"), "expiration" },
                { new StringContent("0"), "public" },
                { new StringContent("1"), "autoplay" },
                { new StringContent("1"), "loop" },
                { new StringContent("1"), "muted" },
                { new ByteArrayContent(FileData), "file", "something.webm" }
            };

            var response = client.PostAsync("http://webmshare.com/api/upload", form).GetAwaiter().GetResult();
    
            var responseString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
    
            var objects = JObject.Parse(responseString);
            
            string result = "";
            try
            {
                result = objects.GetValue("id").ToString();
                return (true, $"https://webmshare.com/play/{result}");
            }
            catch
            {
                result = responseString;
                return (false, $"failed, {result}");
            }
        }
    }
    
    public static class Gfycat
    {
        public static (bool UploadSuccessfull, string Message) Upload(byte[] FileData)
        {
            var createResponse = Create(new GfycatCreateRequest() { NoMd5 = true }).GetAwaiter().GetResult();
    
            Upload(createResponse.GfyName, FileData).GetAwaiter().GetResult();
            int notfounds = 0;
            while (true)
            {
                var statusResponse = Status(createResponse.GfyName).GetAwaiter().GetResult();
    
                if (statusResponse.Task == "NotFoundo")
                {
                    if (notfounds > 10)
                        return (false, statusResponse.Task);
                    notfounds++;
                }
                else if (statusResponse.Task == "encoding") { }
                else if (statusResponse.Task == "complete")
                    return (true, $"https://gfycat.com/{statusResponse.GfyName}");
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
            public bool NoMd5 { get; set; }
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
