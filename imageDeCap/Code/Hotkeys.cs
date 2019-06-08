using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imageDeCap
{
    public static class Hotkeys
    {
        static bool hKey1Pressed = false;
        static bool hKey2Pressed = false;
        static bool hKey3Pressed = false;
        public static void Update()
        {
            if (Program.hotkeysEnabled)
            {
                string hotkey = GetCurrentHotkey();

                if (Preferences.Hotkey1 == hotkey)
                {
                    if (!hKey1Pressed)
                    {
                        ScreenCapturer.UploadPastebinClipboard();
                    }
                    hKey1Pressed = true;
                }
                else if (Preferences.Hotkey2 == hotkey)
                {
                    if (!hKey2Pressed)
                    {
                        ScreenCapturer.CaptureScreenRegion(true);
                    }
                    hKey2Pressed = true;
                }
                else if (Preferences.Hotkey3 == hotkey)
                {
                    if (!hKey3Pressed)
                    {
                        ScreenCapturer.CaptureScreenRegion();
                    }
                    hKey3Pressed = true;
                }
                else
                {
                    // no recognized hotkey
                    hKey1Pressed = false;
                    hKey2Pressed = false;
                    hKey3Pressed = false;
                }
            }
        }


        public static string GetCurrentHotkey()
        {
            string textToPutInBox = "";
            int length = Enum.GetValues(typeof(System.Windows.Input.Key)).Length;

            for (int i = length; i-- > 0;)
            {
                if (Enum.IsDefined(typeof(System.Windows.Input.Key), i) && i != 0)
                {
                    bool isDown = System.Windows.Input.Keyboard.IsKeyDown((System.Windows.Input.Key)i);
                    if (isDown)
                    {
                        textToPutInBox += $"{((System.Windows.Input.Key)i).ToString()}+";
                    }
                }
            }
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

                textToPutInBox = textToPutInBox.Remove(textToPutInBox.Length - 1);
                textToPutInBox = textToPutInBox.Replace("Scroll", "ScrollLock");
                return textToPutInBox;
            }

        }


    }
}
