using LogUploader.Data.Licenses;
using LogUploader.Languages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogUploader.GUIs
{
    public partial class AboutUI : Form
    {
        private const string SPELLCHECKER = "";

        public AboutUI()
        {
            InitializeComponent();
            ApplieLanguage(Language.Data);
            UpdateVersion();
        }

        private void UpdateVersion()
        {
            var version = Helper.Updater.GetLocalVersion();
            lblVersion.Text += $" {version.Major}.{version.Minor}.{version.Build}";
            llPatchnotes.LinkClicked += (sender, e) => Process.Start($"https://github.com/ProfBits/LogUploader2/releases/tag/v{version.Major}.{version.Minor}.{version.Build}");
            this.toolTip1.SetToolTip(this.llPatchnotes, $"https://github.com/ProfBits/LogUploader2/releases/tag/v{version.Major}.{version.Minor}.{version.Build}");
        }

        private void ApplieLanguage(ILanguage lang)
        {
            Text = lang.AboutTitle;
            llProjectPage.Text = lang.AboutProjectPage;
            llLicense.Text = lang.AboutViewLicense;
            lblThanks.Text = lang.AboutSpecialThanks;
            lblTesters.Text = lang.AboutBetaTesters;
            lblThirdCopyRight.Text = lang.AboutCopyright;
            btnThirdParty.Text = lang.AboutView3rdParty;
            lblSpellCheck.Text = SPELLCHECKER + " " + lang.AboutForSpellCheck;
            //HACK
            lblVersion.Text = "Version";
            //TODO localize
            //lblVersion.Text = lang.AboutVersion; // nur "Version"
            //llPatchnotes.Text = lang.AboutViewPatchnotes;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new SoftwareLicenseUI(new LogUploaderLicense()).Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new SoftwareUsedUI().Show();
        }

        private void llProjectPage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start($"https://github.com/ProfBits/LogUploader2");
        }
    }
}
