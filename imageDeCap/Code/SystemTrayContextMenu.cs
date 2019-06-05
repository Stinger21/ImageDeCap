using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace imageDeCap
{
    public static class SystemTrayContextMenu
    {
        public static ContextMenu IconRightClickMenu;
        public static MenuItem ExitButton = new MenuItem();
        public static MenuItem OpenWindowButton = new MenuItem();
        public static MenuItem ContactButton = new MenuItem();
        public static MenuItem PreferencesButton = new MenuItem();

        public static void Initialize()
        {

            IconRightClickMenu = new ContextMenu();

            IconRightClickMenu.MenuItems.Add(OpenWindowButton);
            IconRightClickMenu.MenuItems.Add("-");
            IconRightClickMenu.MenuItems.Add(ContactButton);
            IconRightClickMenu.MenuItems.Add(PreferencesButton);
            IconRightClickMenu.MenuItems.Add(ExitButton);

            ExitButton.Text = "Exit";
            ExitButton.Click += new EventHandler(ExitButton_Click);

            ContactButton.Text = "Contact / Bugs";
            ContactButton.Click += new EventHandler(ContactButton_Click);

            PreferencesButton.Text = "Preferences";
            PreferencesButton.Click += new EventHandler(Preferences_Click);

            OpenWindowButton.Text = "Open Window";
            OpenWindowButton.Click += new EventHandler(OpenWindowButton_Click);
        }

        private static void ExitButton_Click(object Sender, EventArgs e)// Exit button
        {
            Program.ImageDeCap.CloseProgram();
        }
        private static void ContactButton_Click(object Sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.mattwestphal.com/imagedecap");
        }
        private static void Preferences_Click(object Sender, EventArgs e)
        {
            Program.ImageDeCap.ShowProperties();
        }
        private static void OpenWindowButton_Click(object Sender, EventArgs e)// Open Window
        {
            Program.ImageDeCap.OpenWindow();
        }
    }
}
