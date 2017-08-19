using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace imageDeCap
{
    public static class Utilities
    {
        public static bool IsWindows10()
        {
            var reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            string productName = (string)reg.GetValue("ProductName");
            return productName.StartsWith("Windows 10");
        }

        public static void playSound(string soundName)
        {
            if (!imageDeCap.Preferences.DisableSoundEffects)
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                string soundPath = "imageDeCap.Sounds." + soundName;
                Stream soundResource = assembly.GetManifestResourceStream(soundPath);
                SoundPlayer sp = new SoundPlayer(soundResource);
                sp.Play();
            }
        }

    }
}
