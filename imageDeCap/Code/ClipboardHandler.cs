using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace imageDeCap
{
    public static class ClipboardHandler
    {
        static string TextToCopyToClipboard = "";

        public static void Update()
        {
            if (TextToCopyToClipboard != "")
            {
                Clipboard.SetText(TextToCopyToClipboard);
                TextToCopyToClipboard = "";
            }
        }

        public static void SetClipboard(string text)
        {
            if (Preferences.CopyLinksToClipboard)
            {
                if (text != null)
                {
                    TextToCopyToClipboard = text;
                }
                else
                {
                    Utilities.BubbleNotification("failed to retrieve link.", null, ToolTipIcon.Error);
                    Utilities.playSound("error.wav");
                }
            }
        }
    }
}
