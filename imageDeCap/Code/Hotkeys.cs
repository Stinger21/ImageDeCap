using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
public delegate void HotkeyPressed();

namespace imageDeCap
{
    public static class Hotkeys
    {
        public static HotkeyPressed CaptureVideoHotkeyPressed = null;
        static bool UploadPastebitHotkey = false;
        static bool CaptureVideoHotkey = false;
        static bool CaptureImageHotkey = false;

        public static void Update()
        {
            string hotkey = GetCurrentHotkey(true);

            if (Preferences.HotkeyText == hotkey)
            {
                if (!UploadPastebitHotkey)
                    if (Program.hotkeysEnabled)
                        ScreenCapturer.UploadPastebinClipboard();
                UploadPastebitHotkey = true;
            }
            else if (Preferences.HotkeyVideo == hotkey)
            {
                if (!CaptureVideoHotkey)
                    if (Program.hotkeysEnabled)
                        ScreenCapturer.CaptureScreenRegion(true);

                CaptureVideoHotkeyPressed?.Invoke();

                CaptureVideoHotkey = true;
            }
            else if (Preferences.HotkeyImage == hotkey)
            {
                if (!CaptureImageHotkey)
                    if (Program.hotkeysEnabled)
                        ScreenCapturer.CaptureScreenRegion();
                CaptureImageHotkey = true;
            }
            else
            {
                // no recognized hotkey
                UploadPastebitHotkey = false;
                CaptureVideoHotkey = false;
                CaptureImageHotkey = false;
            }

        }

        // Sometimes on some machines windows will say random system and extra keys being pressed, we use a whitelist to mitegate this problem.
        static Key[] KeyWhitelist = new Key[]
        {
            Key.Cancel,Key.Back,Key.Tab,Key.LineFeed,Key.Clear,Key.Return,Key.Enter,Key.Pause,Key.CapsLock,Key.Escape,Key.Space,Key.PageUp,Key.Next,Key.PageDown,Key.End,Key.Home,Key.Left,Key.Up,Key.Right,Key.Down,Key.Snapshot,Key.PrintScreen,Key.Insert,Key.Delete,Key.LWin,Key.RWin,
            Key.NumPad0,Key.NumPad1,Key.NumPad2,Key.NumPad3,Key.NumPad4,Key.NumPad5,Key.NumPad6,Key.NumPad7,Key.NumPad8,Key.NumPad9,
            Key.Multiply,Key.Add,Key.Subtract,Key.Decimal,Key.Divide,Key.NumLock,Key.Scroll,Key.LeftShift,Key.RightShift,Key.LeftCtrl,Key.RightCtrl,Key.LeftAlt,Key.RightAlt,Key.BrowserBack,Key.BrowserForward,Key.BrowserRefresh,Key.BrowserStop,Key.BrowserSearch,Key.BrowserFavorites,Key.BrowserHome,Key.VolumeMute,Key.VolumeDown,Key.VolumeUp,Key.MediaNextTrack,Key.MediaPreviousTrack,Key.MediaStop,Key.MediaPlayPause,Key.LaunchMail,Key.SelectMedia,
            Key.OemSemicolon,Key.OemPlus,Key.OemComma,Key.OemMinus,Key.OemPeriod,Key.OemQuestion,Key.OemTilde,Key.OemOpenBrackets,Key.OemPipe,Key.OemCloseBrackets,Key.OemQuotes,
            Key.Oem1,Key.Oem2,Key.Oem3,Key.Oem4,Key.Oem5,Key.Oem6,Key.Oem7,Key.Oem8,
            Key.D0,Key.D1,Key.D2,Key.D3,Key.D4,Key.D5,Key.D6,Key.D7,Key.D8,Key.D9,
            Key.A,Key.B,Key.C,Key.D,Key.E,Key.F,Key.G,Key.H,Key.I,Key.J,Key.K,Key.L,Key.M,Key.N,Key.O,Key.P,Key.Q,Key.R,Key.S,Key.T,Key.U,Key.V,Key.W,Key.X,Key.Y,Key.Z,
            Key.F1,Key.F2,Key.F3,Key.F4,Key.F5,Key.F6,Key.F7,Key.F8,Key.F9,Key.F10,Key.F11,Key.F12,Key.F13,Key.F14,Key.F15,Key.F16,Key.F17,Key.F18,Key.F19,Key.F20,Key.F21,Key.F22,Key.F23,Key.F24,
        };


        static void unParse(string value, HashSet<Key> Keys)
        {
            value = value.Replace("Alt", "LeftAlt");
            value = value.Replace("Ctrl", "LeftCtrl");
            value = value.Replace("Shift", "LeftShift");
            value = value.Replace("Printscreen", "Snapshot");
            value = value.Replace("ScrollLock", "Scroll");
            
            foreach(string s in value.Split('+'))
            {
                Key key;
                if(Enum.TryParse<Key>(s, out key))
                {
                    Keys.Add(key);
                }
            }
        }

        public static string GetCurrentHotkey(bool CheckOnlyHotkeys = false)
        {
            string textToPutInBox = "";
            //int length = Enum.GetValues(typeof(Key)).Length;
            //
            //for (int i = length; i-- > 0;)
            //{
            //    if (Enum.IsDefined(typeof(Key), i) && i != 0)
            //    {
            //        bool isDown = System.Windows.Input.Keyboard.IsKeyDown((Key)i);
            //        if (isDown)
            //        {
            //            textToPutInBox += $"{((Key)i).ToString()}+";
            //        }
            //    }
            //}

            //long start = 0;
            //long end = 0;
            //Program.QueryPerformanceCounter(out start);


            if(CheckOnlyHotkeys)
            {
                HashSet<Key> keys = new HashSet<Key>();
                unParse(Preferences.HotkeyImage, keys);
                unParse(Preferences.HotkeyText, keys);
                unParse(Preferences.HotkeyVideo, keys);
                foreach (Key k in keys)
                {
                    if (System.Windows.Input.Keyboard.IsKeyDown(k))
                    {
                        textToPutInBox += $"{k}+";
                    }
                }
            }
            else
            {
                foreach (Key k in KeyWhitelist)
                {
                    if (System.Windows.Input.Keyboard.IsKeyDown(k))
                    {
                        textToPutInBox += $"{k}+";
                    }
                }

            }

            //Program.QueryPerformanceCounter(out end);
            //Console.WriteLine(end - start);


            if (textToPutInBox == null)
            {
                return "";
            }
            else if (textToPutInBox == "")
            {
                return "";
            }
            else
            {
                textToPutInBox = textToPutInBox.Replace("LeftAlt", "Alt");
                textToPutInBox = textToPutInBox.Replace("RightAlt", "Alt");
                textToPutInBox = textToPutInBox.Replace("LeftCtrl", "Ctrl");
                textToPutInBox = textToPutInBox.Replace("RightCtrl", "Ctrl");
                textToPutInBox = textToPutInBox.Replace("LeftShift", "Shift");
                textToPutInBox = textToPutInBox.Replace("RightShift", "Shift");
                textToPutInBox = textToPutInBox.Replace("Snapshot", "Printscreen");
                textToPutInBox = textToPutInBox.Replace("Scroll", "ScrollLock");

                textToPutInBox = textToPutInBox.Remove(textToPutInBox.Length - 1);
                return textToPutInBox;
            }

        }
    }
}
