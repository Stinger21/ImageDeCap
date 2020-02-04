using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace imageDeCap
{
    public static class Preferences
    {
        // General
        public static string c_General;
        public static bool SaveImages = false;
        public static string SaveImagesLocation = "";
        public static bool FreezeScreenOnRegionShot = true;
        public static bool EditScreenshotAfterCapture = true;
        public static bool CopyImageToClipboard = true;
        public static bool DisableNotifications = false;
        public static int RecordingFramerate = 30;
        public static bool BackupImages = true;

        // Hotkeys
        public static string c_Hotkeys;
        public static string HotkeyText = "ScrollLock";
        public static string HotkeyVideo = "Shift+Snapshot";
        public static string HotkeyImage = "Snapshot";

        // Uploading
        public static string c_Uploading;
        public static string PastebinSubjectLine = "ImageDeCap Upload!";
        public static bool NeverUpload = false;
        public static string NeverUploadForcePath = @"H:\Installers\ImageDeCap\NeverUploadForce.ini";
        public static bool CopyLinksToClipboard = true;
        public static bool OpenInBrowser = false;
        public static string ClipTarget = "gfycat";
        public static bool uploadToFTP = false;
        public static string FTPurl = "ftp://mysite.com/ftp/";
        public static string FTPusername = "Username";
        public static string FTPpassword = "Password";
        public static string FTPLink = "http://mysite.com/ftp/";
        public static bool CopyFTPLink = false;

        // Misc
        public static string c_Misc;
        public static bool FirstStartup = true;
        public static bool UseRuleOfThirds = false;
        public static bool AddWatermark = false;
        public static string WatermarkFilePath = "";
        public static int WatermarkLocation = 3;
        public static int BrushSmoothingDistance = 2;
        public static string ImageEditorFont = "Arial Black";
        public static int FontStyleType = (int)FontStyle.Bold;

        // Metaprogramming shenanigans to make the data above available as an ini file.
        static List<object> Defaults = new List<object>();
        public static void ResetAllPreferences()
        {
            FieldInfo[] fields = typeof(Preferences).GetFields();
            int numFields = fields.Count();
            for (int i = 0; i < numFields; i++)
            {
                fields[i].SetValue(typeof(Preferences), Defaults[i]);
            }
        }

        public static void ResetPreference(string PreferenceName)
        {
            FieldInfo[] fields = typeof(Preferences).GetFields();
            int numFields = fields.Count();
            for (int i = 0; i < numFields; i++)
            {
                if(PreferenceName == fields[i].Name)
                {
                    fields[i].SetValue(typeof(Preferences), Defaults[i]);
                }
            }
        }

        public static void Load()
        {
            // Get Defaults and save them so we can reset at any point
            FieldInfo[] fields = typeof(Preferences).GetFields();
            foreach (FieldInfo f in fields)
            {
                Defaults.Add(f.GetValue(typeof(Preferences)));
            }
            
            if (!File.Exists(MainWindow.PreferencesPath)) // If the file does not exist, we need to save it.
            {
                Save();
            }
            string FileData = File.ReadAllText(MainWindow.PreferencesPath);
            foreach(string s in FileData.Split('\n'))
            {
                if(!s.Contains("="))
                {
                    continue;
                }
                foreach (FieldInfo f in fields)
                {
                    string name = s.Split('=')[0];
                    if(name == f.Name)
                    {
                        string value = s.Split('=')[1];

                        if (int.TryParse(value, out int IntResult))
                        {
                            f.SetValue(typeof(Preferences), IntResult);
                        }
                        else if (bool.TryParse(value, out bool Boolesult))
                        {
                            f.SetValue(typeof(Preferences), Boolesult);
                        }
                        else
                        {
                            f.SetValue(typeof(Preferences), value);
                        }
                        break;
                    }
                }
            }


            if (File.Exists(Preferences.NeverUploadForcePath))
            {
                Preferences.NeverUpload = true;
            }
        }

        public static void Save()
        {
            FieldInfo[] fields = typeof(Preferences).GetFields();

            string FileData = "";
            foreach(FieldInfo f in fields)
            {
                if(f.Name.StartsWith("c_"))
                {
                    FileData += $"\n[{f.Name.Replace("c_", "")}]\n";
                }
                else
                {
                    
                    FileData += $"{f.Name}={f.GetValue(typeof(Preferences)).ToString()}\n";
                }
            }
            File.WriteAllText(MainWindow.PreferencesPath, FileData);
        }
    }
}
