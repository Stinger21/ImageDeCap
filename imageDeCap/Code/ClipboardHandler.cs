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
        static string textToCopyToClipboard = "";

        public static void Update()
        {
            if (textToCopyToClipboard != "")
            {
                Clipboard.SetText(textToCopyToClipboard);
                textToCopyToClipboard = "";
            }
        }

        public static void setClipboard(string text)
        {
            if (imageDeCap.Preferences.CopyLinksToClipboard)
            {
                if (text != null)
                {
                    textToCopyToClipboard = text;
                }
                else
                {
                    Utilities.BubbleNotification("failed to retrieve link.", null, ToolTipIcon.Error);
                    //Program.ImageDeCap.BubbleNotification.ShowBalloonTip(500, "imageDeCap", "failed to retrieve link.", ToolTipIcon.Error);
                    Utilities.playSound("error.wav");
                }
            }
        }
    }
}
