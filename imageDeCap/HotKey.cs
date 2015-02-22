using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//me
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;

using System.Net;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

using System.Collections.Specialized;
namespace screenshotsPls
{
    public class HotKey : IMessageFilter
    {

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers fsModifiers, Keys vk);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);


        public enum KeyModifiers
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            Windows = 8
        }

        private const int WM_HOTKEY = 0x0312;
        private int id;


        private IntPtr handle;
        public IntPtr Handle
        {
            get { return handle; }
            set { handle = value; }
        }

        private event EventHandler HotKeyPressed;

        public HotKey(Keys key, KeyModifiers modifier, EventHandler hotKeyPressed, int id)
        {
            this.id = id;
            HotKeyPressed = hotKeyPressed;
            RegisterHotKey(key, modifier);
            Application.AddMessageFilter(this);
        }

        ~HotKey()
        {
            Application.RemoveMessageFilter(this);
            UnregisterHotKey(handle, this.id);
        }


        private void RegisterHotKey(Keys key, KeyModifiers modifier)
        {
            if (key == Keys.None)
                return;

            bool isKeyRegisterd = RegisterHotKey(handle, this.id, modifier, key);
            if (!isKeyRegisterd)
                MessageBox.Show("Hotkey all ready in use");
                //throw new ApplicationException("Hotkey allready in use");
        }



        public bool PreFilterMessage(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    HotKeyPressed(this, new EventArgs());
                    return true;
            }
            return false;
        }

    }
}
