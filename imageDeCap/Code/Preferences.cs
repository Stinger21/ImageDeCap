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
        public static string SaveImagesHere = "";
        public static bool saveImageAtAll = false;
        public static bool AlsoSaveTextFiles = false;
        public static bool FreezeScreenOnRegionShot = true;
        public static bool EditScreenshotAfterCapture = true;
        public static bool CopyImageToClipboard = true;
        public static bool DisableNotifications = false;
        public static bool DisableSoundEffects = false;
        public static int GIFRecordingFramerate = 15; // unimplemented

        // Hotkeys
        public static string c_Hotkeys;
        public static string Hotkey1 = "LeftCtrl+LeftShift+D2";
        public static string Hotkey2 = "LeftCtrl+LeftShift+D3";
        public static string Hotkey3 = "LeftCtrl+LeftShift+D4";
        public static string Hotkey4 = "LeftCtrl+LeftShift+D5";

        // Uploading
        public static string c_Uploading;
        public static string PastebinSubjectLine = "ImageDeCap Upload!";
        public static bool NeverUpload = false;
        public static bool CopyLinksToClipboard = true;
        public static bool OpenInBrowser = false;
        public static bool uploadToFTP = false;
        public static string FTPurl = "ftp://speedtest.tele2.net/upload/";
        public static string FTPusername = "anonymous";
        public static string FTPpassword = "password";
        public static bool AlsoFTPTextFiles = false;

        // Misc
        public static string c_Misc;
        public static bool FirstStartup = true;
        public static string SnipSoundPath = "";
        public static string UploadSoundPath = "";
        public static string ErrorSoundPath = "";
        public static bool UseRuleOfThirds = true;

        public static bool AddWatermark = false;
        public static string WatermarkFilePath = "";
        public static int WatermarkLocation = 3;

        public static string ImageEditorFont = "Arial Black";
        public static int FontStyleType = (int)FontStyle.Bold;

        // Metaprogramming shenanigans to make the data above avilable as an ini file.
        static List<object> Defaults = new List<object>();
        public static void Reset()
        {
            FieldInfo[] fields = typeof(Preferences).GetFields();
            int numFields = fields.Count();
            for (int i = 0; i < numFields; i++)
            {
                fields[i].SetValue(typeof(Preferences), Defaults[i]);
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

                        int IntResult;
                        bool Boolesult;
                        if (int.TryParse(value, out IntResult))
                        {
                            f.SetValue(typeof(Preferences), IntResult);
                        }
                        else if (bool.TryParse(value, out Boolesult))
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
        }
        public static void Save()
        {
            FieldInfo[] fields = typeof(Preferences).GetFields();

            string FileData = "";
            foreach(FieldInfo f in fields)
            {
                if(f.Name.StartsWith("c_"))
                {
                    FileData += "\n[" + f.Name.Replace("c_", "") + "]\n";
                }
                else
                {
                    
                    FileData += f.Name + "=" + f.GetValue(typeof(Preferences)).ToString() + "\n";
                }
            }
            File.WriteAllText(MainWindow.PreferencesPath, FileData);
        }
    }
}
