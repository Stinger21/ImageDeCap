using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace imageDeCap
{
    public static class ClipboardHandler
    {
        static string TextToCopyToClipboard = "";

        // uuhhh... thread safety. yeah.. that's it.
        public static void Update()
        {
            if (TextToCopyToClipboard == "")
                return;

            Clipboard.SetText(TextToCopyToClipboard);
            TextToCopyToClipboard = "";
        }

        public static void SetClipboardText(string text)
        {
            if (!Preferences.CopyLinksToClipboard)
                return;

            if (text == null)
                return;

            TextToCopyToClipboard = text;
        }

        public static void SetClipboardImage(byte[] ImageData)
        {
            // try again a couple times :o
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    Image bitmapImage = Image.FromStream(new MemoryStream(ImageData));
                    Clipboard.SetImage(bitmapImage);
                    bitmapImage.Dispose();
                    break;
                }
                catch (ExternalException) // Requested clipboard operation did not succeed
                {
                    Thread.Sleep(100);
                }
            }
        }
    }
}
