using LogUploader.Data.Licenses;
using LogUploader.Languages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new SoftwareLicenseUI(new LogUploaderLicense());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new SoftwareUsedUI().Show();
        }
    }
}
