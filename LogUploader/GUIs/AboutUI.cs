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
        private const string SPELLCHECKER = "Laelia#5033";

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
            if (version.Revision == 0)
            {
                llPatchnotes.LinkClicked += (sender, e) => Process.Start($"https://github.com/ProfBits/LogUploader2/releases/tag/v{version.Major}.{version.Minor}.{version.Build}");
            }
            else
            {
                lblVersion.Text += $".{version.Revision}";
                llPatchnotes.LinkClicked += (sender, e) => Process.Start($"https://github.com/ProfBits/LogUploader2/releases/tag/v{version.Major}.{version.Minor}.{version.Build}.{version.Revision}");
            }
            this.toolTip1.SetToolTip(this.llPatchnotes, $"https://github.com/ProfBits/LogUploader2/releases/tag/v{version.Major}.{version.Minor}.{version.Build}.{version.Revision}");
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
            lblVersion.Text = lang.AboutVersion;
            llPatchnotes.Text = lang.AboutViewPatchnotes;

#if DEBUG
            Text += " DEBUG";
#elif ALPHA
            Text += " ALPHA";
#elif BETA
            Text += " BETA";
#endif
        }

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new SoftwareLicenseUI(new LogUploaderLicense()).Show();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            new SoftwareUsedUI().Show();
        }

        private void LlProjectPage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start($"https://github.com/ProfBits/LogUploader2");
        }
    }
}
