using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imageDeCap
{
    public static class SystemTrayContextMenu
    {
        public static System.Windows.Forms.ContextMenu IconRightClickMenu;
        public static System.Windows.Forms.MenuItem ExitButton = new System.Windows.Forms.MenuItem();
        public static System.Windows.Forms.MenuItem OpenWindowButton = new System.Windows.Forms.MenuItem();
        public static System.Windows.Forms.MenuItem ContactButton = new System.Windows.Forms.MenuItem();
        public static System.Windows.Forms.MenuItem PreferencesButton = new System.Windows.Forms.MenuItem();

        public static void Initialize()
        {

            IconRightClickMenu = new System.Windows.Forms.ContextMenu();

            IconRightClickMenu.MenuItems.Add(OpenWindowButton);
            IconRightClickMenu.MenuItems.Add("-");
            IconRightClickMenu.MenuItems.Add(ContactButton);
            IconRightClickMenu.MenuItems.Add(PreferencesButton);
            IconRightClickMenu.MenuItems.Add(ExitButton);

            ExitButton.Text = "Exit";
            ExitButton.Click += new System.EventHandler(ExitButton_Click);

            ContactButton.Text = "Contact / Bugs";
            ContactButton.Click += new System.EventHandler(ContactButton_Click);

            PreferencesButton.Text = "Preferences";
            PreferencesButton.Click += new System.EventHandler(Preferences_Click);

            OpenWindowButton.Text = "Open Window";
            OpenWindowButton.Click += new System.EventHandler(OpenWindowButton_Click);
        }

        private static void ExitButton_Click(object Sender, EventArgs e)//Exit button
        {
            Program.ImageDeCap.actuallyCloseTheProgram();
        }
        private static void ContactButton_Click(object Sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.mattwestphal.com/imagedecap/");
        }
        private static void Preferences_Click(object Sender, EventArgs e)
        {
            //Program.ImageDeCap.ShowProperties();
        }
        private static void OpenWindowButton_Click(object Sender, EventArgs e)//Open Window
        {
            //Program.ImageDeCap.OpenWindow();
        }
    }
}
