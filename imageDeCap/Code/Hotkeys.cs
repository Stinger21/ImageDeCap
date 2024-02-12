using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Diagnostics;
public delegate void HotkeyPressed();

namespace imageDeCap
{
    public static class Hotkeys
    {
        public static Stopwatch watch;
        public static HotkeyPressed CaptureVideoHotkeyPressed = null;
        static bool CaptureVideoHotkey = false;
        static bool CaptureImageHotkey = false;
        
        public static void Update()
        {
            string hotkey = GetCurrentHotkey(true);
            if (Preferences.HotkeyVideo == hotkey)
            {
                if (!CaptureVideoHotkey)
                    if (Program.hotkeysEnabled)
                        ScreenCapturer.CaptureScreenRegion(true);

                CaptureVideoHotkeyPressed?.Invoke();

                CaptureVideoHotkey = true;
            }
            else if (Preferences.HotkeyImage == hotkey)
            {
                Stopwatch watchTotal = Stopwatch.StartNew();

                if (!CaptureImageHotkey)
                    if (Program.hotkeysEnabled)
                        ScreenCapturer.CaptureScreenRegion();
                CaptureImageHotkey = true;

                watchTotal.Stop();
                Console.WriteLine("Total: " + watchTotal.ElapsedMilliseconds);
            }
            else
            {
                // no recognized hotkey
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

        static void UnParse(string value, HashSet<Key> Keys)
        {
            value = value.Replace("Alt", "LeftAlt");
            value = value.Replace("Ctrl", "LeftCtrl");
            value = value.Replace("Shift", "LeftShift");

            foreach(string s in value.Split('+'))
            {
                Key key;
                if(Enum.TryParse<Key>(s, out key))
                {
                    Keys.Add(key);
                }
            }
        }

        static List<Key> HeldKeys = new List<Key>();
        static HashSet<Key> keys = new HashSet<Key>();
        public static string GetCurrentHotkey(bool CheckOnlyHotkeys = false)
        {
            HeldKeys.Clear();

            if (CheckOnlyHotkeys)
            {
                keys.Clear();
                UnParse(Preferences.HotkeyImage, keys);
                UnParse(Preferences.HotkeyVideo, keys);
                foreach (Key k in keys)
                {
                    if (System.Windows.Input.Keyboard.IsKeyDown(k))
                    {
                        HeldKeys.Add(k);
                    }
                }
            }
            else
            {
                foreach (Key k in KeyWhitelist)
                {
                    if (System.Windows.Input.Keyboard.IsKeyDown(k))
                    {
                        HeldKeys.Add(k);
                    }
                }
            }

            HeldKeys.Sort();
            HeldKeys.Reverse();
            string textToPutInBox = "";
            foreach (Key k in HeldKeys)
            {
                textToPutInBox += $"{k}+";
            }

            if (textToPutInBox == null)
                return "";

            if (textToPutInBox == "")
                return "";

            textToPutInBox = textToPutInBox.Replace("LeftAlt", "Alt");
            textToPutInBox = textToPutInBox.Replace("RightAlt", "Alt");
            textToPutInBox = textToPutInBox.Replace("LeftCtrl", "Ctrl");
            textToPutInBox = textToPutInBox.Replace("RightCtrl", "Ctrl");
            textToPutInBox = textToPutInBox.Replace("LeftShift", "Shift");
            textToPutInBox = textToPutInBox.Replace("RightShift", "Shift");

            textToPutInBox = textToPutInBox.Remove(textToPutInBox.Length - 1);
            return textToPutInBox;
        }
    }
}
