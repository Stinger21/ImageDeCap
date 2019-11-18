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
using System.Drawing.Imaging;
using System.Reflection;

namespace imageDeCap
{
    public enum ScreenCaptureMode
    {
        Screen,
        Window,
        Bounds
    }
    //public class NonGDIBitmap
    //{
    //    byte[] Data;
    //    public NonGDIBitmap(Bitmap bitmap)
    //    {
    //        BitmapData bmpdata = null;
    //
    //        try
    //        {
    //            bmpdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
    //            int numbytes = bmpdata.Stride * bitmap.Height;
    //            byte[] bytedata = new byte[numbytes];
    //            IntPtr ptr = bmpdata.Scan0;
    //
    //            Marshal.Copy(ptr, bytedata, 0, numbytes);
    //
    //            Data = bytedata;
    //        }
    //        finally
    //        {
    //            if (bmpdata != null)
    //                bitmap.UnlockBits(bmpdata);
    //        }
    //    }
    //}

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
                Utilities.PlaySound("snip.wav");

                if (!Preferences.NeverUpload)
                {
                    BackgroundWorker bw = new BackgroundWorker();
                    bw.DoWork += Uploading.UploadPastebin;
                    bw.RunWorkerCompleted += UploadPastebinCompleted;
                    bw.RunWorkerAsync(clipboard);
                }

                if (Preferences.uploadToFTP)
                {
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

        public static void CaptureScreenRegion(bool isClip = false)
        {
            Bitmap background = null;
            if(!isClip)
            {
                var bounds = SystemInformation.VirtualScreen;
                background = CaptureScreen(bounds.Left, bounds.Top, bounds.Size.Width, bounds.Size.Height);
            }

            // prevent blackening
            if (!IsTakingSnapshot)
            {
                IsTakingSnapshot = true;
                Program.hotkeysEnabled = false;
                // back cover used for pulling cursor position into updateSelectedArea()
                if (CurrentBackCover != null)
                    CurrentBackCover.Dispose();

                CurrentBackCover = new CompleteCover(isClip);
                CurrentBackCover.Show();
                CurrentBackCover.AfterShow(background, isClip);
            }
        }

        public static void UploadImageData(byte[] ImageData, Filetype imageType, bool ForceNoEdit = false, bool RMBClickForceEdit = false, Bitmap[] ClipFrames = null, Rectangle SelectedRegion = default(Rectangle))
        {
            Program.hotkeysEnabled = true; // Enable hotkeys here again so you can take more screenshots

            if (imageType == Filetype.error)
                return;

            // Copy image to clipboard
            if (imageType != Filetype.mp4)
            {
                if (Preferences.CopyImageToClipboard)
                {
                    ClipboardHandler.SetClipboardImage(ImageData);
                }
            }

            if ((Preferences.EditScreenshotAfterCapture || RMBClickForceEdit) && ForceNoEdit == false)
            {
                if (imageType == Filetype.mp4)
                {
                    //int framerate = 1000 / CurrentBackCover.RecordedTime;
                    ClipTrimmer editor = new ClipTrimmer(ClipFrames, CurrentBackCover.topBox.Location.X, CurrentBackCover.topBox.Location.Y, CurrentBackCover.RecordedFramerate);
                    editor.Show();
                    editor.FormClosed += EditorDone;
                }
                else
                {
                    ImageEditor editor = new ImageEditor(ImageData, CurrentBackCover.topBox.Location.X, CurrentBackCover.topBox.Location.Y, SelectedRegion);
                    editor.Show();
                    editor.FormClosed += EditorDone;
                }
            }
            else
            {
                // If it's a clip make save the default :>
                ImageEditor.EditorResult EditorResult = ImageEditor.EditorResult.Upload;
                if(imageType == Filetype.mp4)
                    EditorResult = ImageEditor.EditorResult.Save;

                UploadImageData_AfterEdit(EditorResult, imageType, ImageData, ClipFrames);
            }
        }

        public static void EditorDone(object sender, EventArgs e)
        {
            Filetype f;
            ImageEditor.EditorResult EditorResult = ImageEditor.EditorResult.Quit;
            byte[] ImageData = null;
            Bitmap[] ClipData = null;
            bool Sound = false;
            if (sender is ImageEditor)
            {
                ImageEditor editor = (ImageEditor)sender;
                (EditorResult, ImageData) = editor.FinalFunction();
                editor.Dispose();
                f = Filetype.png;
            }
            else
            {
                ClipTrimmer editor = (ClipTrimmer)sender;
                (EditorResult, ClipData, Sound) = editor.FinalFunction();
                editor.Dispose();
                f = Filetype.mp4;
            }
            UploadImageData_AfterEdit(EditorResult, f, ImageData, ClipData, Sound);
        }


        public static void UploadImageData_AfterEdit(ImageEditor.EditorResult EditorResult, Filetype imageType, byte[] FileData, Bitmap[] ClipData, bool Sound = false)
        {
            if (EditorResult == ImageEditor.EditorResult.Quit)
            {
                foreach (var v in CurrentBackCover.CapturedClipFrames) { v.Dispose(); }
                CurrentBackCover.CapturedClipFrames.Clear();
                return;
            }

            // Ask the user where to save before turning the array of images into a video because video compression can take a while.
            string SavePath = null;
            if (EditorResult == ImageEditor.EditorResult.Save)
            {
                if (imageType == Filetype.mp4)
                {
                    SavePath = Utilities.FileDialog(MainWindow.videoFormat);
                }
                if (imageType != Filetype.mp4)
                {
                    SavePath = Utilities.FileDialog(".png");
                }
            }

            // compress video
            if (imageType == Filetype.mp4)
            {
                if(CurrentBackCover.RecordedFramerate == 0)
                {
                    // ERROR, the recorded framerate was 0. o_o
                    MessageBox.Show("Something went wrong with this recording.", "Framerate was 0.");
                    return;
                }
                FileData = ClipTrimmer.VideoFromFrames(ClipData, CurrentBackCover.RecordedFramerate, Sound);
                foreach (var v in CurrentBackCover.CapturedClipFrames) { v.Dispose(); }
                CurrentBackCover.CapturedClipFrames.Clear();
            }
            if (SavePath != null)
            {
                File.WriteAllBytes(SavePath, FileData);
            }

            string SaveFileName = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
            string Extension = imageType.ToString();
            //if (Extension == "gif")
            //{
            //    Extension = MainWindow.videoFormat.Replace(".", "");
            //}

            if (Preferences.SaveImages && Directory.Exists(Preferences.SaveImagesLocation))
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


            // Copy image to clipboard if it's not a clip
            if (imageType != Filetype.mp4)
            {
                if (EditorResult == ImageEditor.EditorResult.Clipboard)
                {
                    if (Preferences.CopyImageToClipboard)
                    {
                        ClipboardHandler.SetClipboardImage(FileData);
                    }
                }
            }


            BackgroundWorker bwFTP = new BackgroundWorker();
            if (Preferences.uploadToFTP)
            {
                bwFTP.DoWork += Uploading.UploadToFTP;
                bwFTP.RunWorkerAsync(new object[] { Preferences.FTPurl,
                                                Preferences.FTPusername,
                                                Preferences.FTPpassword,
                                                FileData,
                                                $"{SaveFileName}.{Extension}" });
            }
            if (Preferences.CopyFTPLink && Preferences.uploadToFTP)
            {
                bwFTP.RunWorkerCompleted += UploadImageFileCompleted;
            }

            // Upload the image
            if (EditorResult == ImageEditor.EditorResult.Upload)
            {
                BackgroundWorker bw1 = new BackgroundWorker();
                if(!Preferences.NeverUpload)
                {
                    if (imageType == Filetype.mp4)
                    {
                        //if (Preferences.ClipTarget == "gfycat")
                        //{
                        //    bw1.DoWork += Uploading.UploadClip_Gfycat;
                        //}
                        //else if (Preferences.ClipTarget == "imgur")
                        //{
                        //    bw1.DoWork += Uploading.UploadClip_Imgur;
                        //}
                        //else if (Preferences.ClipTarget == "webmshare")
                        //{
                        //    bw1.DoWork += Uploading.UploadClip_Webmshare;
                        //}
                    }
                    else
                    {
                        bw1.DoWork += Uploading.UploadImage_Imgur;
                    }
                }

                if (Preferences.CopyFTPLink && Preferences.uploadToFTP)
                {
                    //bwFTP.RunWorkerCompleted += UploadImageFileCompleted;
                }
                else if(!Preferences.NeverUpload)
                {
                    bw1.RunWorkerCompleted += UploadImageFileCompleted;
                }
                bw1.RunWorkerAsync(FileData);

                
            }
        }

        public static void UploadImageFileCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            (string url, byte[] ImageData) = (ValueTuple<string, byte[]>)e.Result;
            
            if (url.Contains("failed"))
            {
                ClipboardHandler.SetClipboardText(url);
                Utilities.BubbleNotification($"Upload to failed! \n{url}\nAre you connected to the internet?", null, ToolTipIcon.Error);
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
                        Utilities.BubbleNotification("URL copied to clipboard!", Program.ImageDeCap.BalloonTipClicked);
                }
                else
                {
                    if (!Preferences.DisableNotifications)
                        Utilities.BubbleNotification("Upload Complete!", Program.ImageDeCap.BalloonTipClicked);
                }
                bool WindowsXP = !Utilities.IsWindows10();
                bool NotificationsEnabled = !Preferences.DisableNotifications;
                if (WindowsXP && NotificationsEnabled)
                {
                    Utilities.PlaySound("upload.wav");
                }
                ClipboardHandler.SetClipboardText(url);
                Program.ImageDeCap.AddLink(url);
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
            result = CaptureScreen(bounds.Left, bounds.Top, bounds.Size.Width, bounds.Size.Height);
            using (var g = Graphics.FromImage(result))
            {
                if (CaptureMouse)
                {
                    int cursorX = 0;
                    int cursorY = 0;
                    Bitmap cursorBMP = CaptureCursor(ref cursorX, ref cursorY);
                    if (cursorBMP != null)
                    {
                        Rectangle SelectedRegion = CurrentBackCover.SelectedRegion;
                        g.DrawImage(cursorBMP, new Rectangle(cursorX - SelectedRegion.X, cursorY - SelectedRegion.Y, cursorBMP.Width, cursorBMP.Height));
                        g.Flush();
                        cursorBMP.Dispose();
                    }
                    else
                    {
                        Console.WriteLine("Failed to capture cursor.");
                    }
                }
            }
            return result;
        }


        static Bitmap CaptureCursor(ref int x, ref int y)
        {
            Win32Stuff.CURSORINFO ci = new Win32Stuff.CURSORINFO();
            ci.cbSize = Marshal.SizeOf(ci);
            if (!Win32Stuff.GetCursorInfo(out ci))
                return null;
        
            if (ci.flags != Win32Stuff.CURSOR_SHOWING)
                return null;

            IntPtr hicon = Win32Stuff.CopyIcon(ci.hCursor);

            if (!Win32Stuff.GetIconInfo(hicon, out Win32Stuff.ICONINFO icInfo))
            {
                Win32Stuff.DestroyIcon(hicon);
                Win32Stuff.DeleteObject(icInfo.hbmMask);
                Win32Stuff.DeleteObject(icInfo.hbmColor);
                return null;
            }

            x = ci.ptScreenPos.x - ((int)icInfo.xHotspot);
            y = ci.ptScreenPos.y - ((int)icInfo.yHotspot);
            Icon ic = Icon.FromHandle(hicon);
            if (ic.Size.Height == 0 || ic.Size.Width == 0)
            {
                Win32Stuff.DestroyIcon(hicon);
                Win32Stuff.DeleteObject(icInfo.hbmMask);
                Win32Stuff.DeleteObject(icInfo.hbmColor);
                return null;
            }
            Bitmap b = ic.ToBitmap();

            Win32Stuff.DestroyIcon(hicon);
            Win32Stuff.DeleteObject(icInfo.hbmMask);
            Win32Stuff.DeleteObject(icInfo.hbmColor);
            
            return b;
        }


        public static Bitmap CaptureScreen(int x, int y, int width, int height)
        {
            IntPtr handle = User32.GetDesktopWindow();
            IntPtr hdcSrc = User32.GetWindowDC(handle);
            User32.RECT windowRect = new User32.RECT();
            User32.GetWindowRect(handle, ref windowRect);
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
            IntPtr hOld = GDI32.SelectObject(hdcDest, hBitmap);
            GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, x, y, GDI32.SRCCOPY);
            GDI32.SelectObject(hdcDest, hOld);
            GDI32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);
            Image img = Image.FromHbitmap(hBitmap);
            GDI32.DeleteObject(hBitmap);
            return (Bitmap)img;
        }

        private class GDI32
        {
            public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter
            [DllImport("gdi32.dll")]
            public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
                int nWidth, int nHeight, IntPtr hObjectSource,
                int nXSrc, int nYSrc, int dwRop);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
                int nHeight);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteObject(IntPtr hObject);
            [DllImport("gdi32.dll")]
            public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        }

        /// <summary>
        /// Helper class containing User32 API functions
        /// </summary>
        private class User32
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }
            [DllImport("user32.dll")]
            public static extern IntPtr GetDesktopWindow();
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowDC(IntPtr hWnd);
            [DllImport("user32.dll")]
            public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
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

        public enum TernaryRasterOperations : uint
        {
            SRCCOPY = 0x00CC0020,
            SRCPAINT = 0x00EE0086,
            SRCAND = 0x008800C6,
            SRCINVERT = 0x00660046,
            SRCERASE = 0x00440328,
            NOTSRCCOPY = 0x00330008,
            NOTSRCERASE = 0x001100A6,
            MERGECOPY = 0x00C000CA,
            MERGEPAINT = 0x00BB0226,
            PATCOPY = 0x00F00021,
            PATPAINT = 0x00FB0A09,
            PATINVERT = 0x005A0049,
            DSTINVERT = 0x00550009,
            BLACKNESS = 0x00000042,
            WHITENESS = 0x00FF0062
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

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool DestroyIcon(IntPtr handle);


        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        public static extern IntPtr DeleteObject(IntPtr hDc);

        [DllImport("gdi32.dll", EntryPoint = "BitBlt")]
        public static extern bool BitBlt(IntPtr hDcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hDcSrc, int nXSrc, int nYSrc, uint dwRop);

        [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
        public static extern IntPtr SelectObject(IntPtr hDc, IntPtr hgdiobj);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDc);

        [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
        public static extern bool DeleteDC(IntPtr hDc);

        [DllImport("kernel32.dll", EntryPoint = "RegisterApplicationRestart")]
        public static extern int RegisterApplicationRestart([MarshalAs(UnmanagedType.BStr)] string commandLineArgs, int flags);

        [DllImport("user32.dll", EntryPoint = "mouse_event")]
        public static extern void Mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [DllImport("user32.dll", EntryPoint = "keybd_event")]
        public static extern void Keybd_event(byte vk, byte scan, int flags, int extrainfo);

        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        public static extern void SetCursorPos(int x, int y);



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
}
