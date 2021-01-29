using LogUploader.Helper;
using LogUploader.Tools.Logger;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogUploader.GUIs
{
    public partial class FatalErrorUI : Form
    {
        public FatalErrorUI(string title, string message)
        {
            InitializeComponent();
            Logger.Message($"FatalErrorUI\nTitle: {title}\nMessage:\n{message}");
            lblTitel.Text = title;
            rtbMessage.Text = message;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnShowLog_Click(object sender, EventArgs e)
        {
            if (File.Exists(Logger.LogFile))
                _ = Task.Run(() => System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{Logger.LogFile}\""));
            else
                MessageBox.Show("No log file", "No logfile created yet", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }
    }
}
