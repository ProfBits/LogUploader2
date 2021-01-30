using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogUploader.Data.Licenses;
using LogUploader.Localisation;

namespace LogUploader.GUIs
{
    public partial class SoftwareItem : UserControl
    {
        public SoftwareItem()
        {
            InitializeComponent();
            ApplyLanguage(Language.Data);
        }

        private void ApplyLanguage(ILanguage lang)
        {
            llProjectpage.Text = lang.SoftwareItemProject;
            llLizenze.Text = lang.SoftwareItemViewLicense;
        }

        [Browsable(true)]
        public string Software { get => lblsoft.Text; set => lblsoft.Text = value; }
        [Browsable(true)]
        public string Copyright { get => lblCopyright.Text; set => lblCopyright.Text = value; }
        [Browsable(true)]
        public string ProjectLink { get; set; }
        public ISoftwareLicense License { get; set; }
        [Browsable(true)]
        public string LicenseStr { get => lblLicense.Text; set => lblLicense.Text = value; }
        [Browsable(true)]
        public Image Image { get => pictureBox1.Image; set => pictureBox1.Image = value; }
        [Browsable(true)]
        public bool ImageUsed { get => panel1.Visible; set => panel1.Visible = value; }

        private async void llProjectpage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            await Task.Run(() => System.Diagnostics.Process.Start(ProjectLink));
        }

        private void llLizenze_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new SoftwareLicenseUI(License).Show();
        }
    }
}
