using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace imageDeCap
{
    // uuhhh... thread safety. yeah.. that's it.
    public static class ClipboardHandler
    {
        static string TextToCopyToClipboard = "";

        public static void Update()
        {
            if (TextToCopyToClipboard == "")
                return;

            Clipboard.SetText(TextToCopyToClipboard);
            TextToCopyToClipboard = "";
        }

        public static void SetClipboard(string text)
        {
            if (!Preferences.CopyLinksToClipboard)
                return;

            if (text == null)
                return;

            TextToCopyToClipboard = text;
        }
    }
}
