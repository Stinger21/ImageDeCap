using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace imageDeCap
{
    [Flags]
    public enum Modifier : int
    {
        None = 0x0000,
        Alt = 0x0001,
        Ctrl = 0x0002,
        NoRepeat = 0x4000,
        Shift = 0x0004,
        Win = 0x0008
    }

    public class HotkeyEventArgs : EventArgs
    {
        private Modifier _modifier;
        private Keys _key;

        internal HotkeyEventArgs(Modifier modifier, Keys key)
        {
            _modifier = modifier;
            _key = key;
        }

        public Modifier Modifier
        {
            get { return _modifier; }
        }

        public Keys Key
        {
            get { return _key; }
        }
    }

    public delegate void HotkeyPressedCb(HotkeyEventArgs args);

    class HotkeyCombo
    {
        public int modifier = 0;
        public int key = 0;

        public HotkeyCombo()
        {
        }

        public HotkeyCombo(int mmodifier, int kkey)
        {
            modifier = mmodifier;
            key = kkey;
        }

        public bool Equals(HotkeyCombo rhs)
        {
            return (modifier == rhs.modifier) && (key == rhs.key);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as HotkeyCombo);
        }

        public override int GetHashCode()
        {
            return modifier * 31 + key;
        }
    }

    class Window : NativeWindow, IDisposable
    {
        private static int WM_HOTKEY = 0x0312;
        public Dictionary<HotkeyCombo, HotkeyPressedCb> callbacks = new Dictionary<HotkeyCombo, HotkeyPressedCb>();

        public Window()
        {
            this.CreateHandle(new CreateParams());
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_HOTKEY)
            {
                // Unpack the key and modifier from the lparam.
                Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                Modifier modifier = (Modifier)((int)m.LParam & 0xFFFF);

                // Find and call the delegate hooked to the modifier/key combo.
                HotkeyCombo combo = new HotkeyCombo((int)modifier, (int)key);
                callbacks[combo](new HotkeyEventArgs(modifier, key));
            }
        }

        public void Dispose()
        {
            this.DestroyHandle();
        }
    }

    public class Hotkey
    {
        /* http://msdn.microsoft.com/en-gb/library/windows/desktop/ms646309%28v=vs.85%29.aspx */
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        /* http://msdn.microsoft.com/en-us/library/windows/desktop/ms646327%28v=vs.85%29.aspx */
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        Window _window = new Window();
        int _currentId = 0;
        Label lab;
        public Hotkey(Label lab)
        {
            this.lab = lab;
        }

        public void registerHotkey(Modifier modifier, Keys key, HotkeyPressedCb func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func", "Function pointer is null.");
            }

            _currentId += 1;
            if (!RegisterHotKey(_window.Handle, _currentId, (uint)modifier, (uint)key))
            {
                MessageBox.Show("Couldn't register hotkey " + modifier.ToString() + ", " + key.ToString()+"\nIs it already in use by another program?");
            }

            _window.callbacks.Add(new HotkeyCombo((int)modifier, (int)key), func);
        }

        public void Dispose()
        {
            for (int i = _currentId; i > 0; --i)
            {
                UnregisterHotKey(_window.Handle, i);
            }
            _window.Dispose();
        }
    }
}