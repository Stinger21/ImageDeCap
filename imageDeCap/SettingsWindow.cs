using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Win32;

namespace imageDeCap
{
    public partial class SettingsWindow : Form
    {
        Form1 parentForm;
        public SettingsWindow(Form1 parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            
            var converter = new KeysConverter();
            
            bool saveImagesAtAll = Properties.Settings.Default.saveImageAtAll;
            checkBox1.Checked = saveImagesAtAll;
            button2.Enabled = Properties.Settings.Default.saveImageAtAll;
            
            textBox1.Text = Properties.Settings.Default.SaveImagesHere;
            textBox1.Enabled = checkBox1.Checked;

            checkBox7.Checked = Properties.Settings.Default.EditScreenshotAfterCapture;
            checkBox3.Checked = Properties.Settings.Default.CopyLinksToClipboard;
            checkBox4.Checked = Properties.Settings.Default.DisableSoundEffects;
        }


        private void button5_Click(object sender, EventArgs e)//Apply
        {
            parentForm.hook.Dispose();
            Properties.Settings.Default.saveImageAtAll = checkBox1.Checked;
            Properties.Settings.Default.SaveImagesHere = textBox1.Text;

            Properties.Settings.Default.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }


        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
        }
        private void writeHotkey(KeyEventArgs e, TextBox box)
        {

        }



        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SettingsWindow_Load(object sender, EventArgs e)
        {

        }

        private void SettingsWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = checkBox1.Checked;
            button2.Enabled = checkBox1.Checked;
            Properties.Settings.Default.saveImageAtAll = checkBox1.Checked;
            Properties.Settings.Default.Save();
        }


        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.SaveImagesHere = textBox1.Text;
            Properties.Settings.Default.Save();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            //RegisterInStartup(checkBox2.Checked);
            Properties.Settings.Default.Save();
        }

        private void RegisterInStartup(bool isChecked)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey
                    ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (isChecked)
            {
                registryKey.SetValue("imageDeCap", Application.ExecutablePath);
            }
            else
            {
                registryKey.DeleteValue("imageDeCap");
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.EditScreenshotAfterCapture = checkBox7.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.CopyLinksToClipboard = checkBox3.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.DisableSoundEffects = checkBox4.Checked;
            Properties.Settings.Default.Save();
        }

    }
}
