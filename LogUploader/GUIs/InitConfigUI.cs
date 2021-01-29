using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogUploader.Properties;
using LogUploader.Helper;
using Extensiones;

using LogUploader.Data.Settings;
using LogUploader.Localisation;

namespace LogUploader.GUI
{
    public partial class InitConfigUI : Form
    {

        readonly SettingsData Settings;
        private bool closeForm = false;

        public InitConfigUI()
        {
            InitializeComponent();

            Settings = new SettingsData(new Settings());
            bsSettings.DataSource = Settings;

            ApplyLanguage(Language.Data);

            cbLang.DisplayMember = "name";
            cbLang.ValueMember = "value";
            cbLang.DataSource = EnumHelper.GetValues<eLanguage>().Select(e => new { name = e.GetAttribute<CombBoxView>().Name, value = e }).ToList();
            cbLang.SelectedValue = Settings.Language;


            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (System.IO.Directory.Exists(basePath + @"\Guild Wars 2\addons\arcdps\arcdps.cbtlogs"))
                folderBrowserDialog1.SelectedPath = basePath + @"\Guild Wars 2\addons\arcdps\arcdps.cbtlogs";
            else
                folderBrowserDialog1.SelectedPath = basePath;

            Settings.ArcLogsPath = folderBrowserDialog1.SelectedPath;

            bsSettings.ResetBindings(false);

        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = txtPath.Text;
            folderBrowserDialog1.ShowDialog();
            Settings.ArcLogsPath = folderBrowserDialog1.SelectedPath;
            bsSettings.ResetBindings(false);
        }

        private void InitConfigUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (closeForm)
                return;

            if (!ReallyClose())
                e.Cancel = true;
        }

        private void BtnCancle_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool ReallyClose()
        {
            var res = MessageBox.Show(Language.Data.InitCancelSetupText, Language.Data.InitCancelSetupTitel, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            switch (res)
            {
                case DialogResult.Yes:
                    return true;
                case DialogResult.No:
                default:
                    return false;
            }
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (!ValidatePaht())
            {
                MessageBox.Show(Language.Data.InitInvalidPathText, Language.Data.InitInvalidPathTitel);
                return;
            }
                

            Settings.FirstBoot = false;
            var settings = new Settings();
            Settings.ApplyTo(settings);
            settings.Save();

            closeForm = true;
            Close();
        }

        private bool ValidatePaht()
        {
            if (!System.IO.Directory.Exists(Settings.ArcLogsPath))
                return false;

            if (Settings.ArcLogsPath.EndsWith(@"arcdps.cbtlogs\")
            || Settings.ArcLogsPath.EndsWith(@"arcdps.cbtlogs"))
                return true;

            var dir = System.IO.Directory.EnumerateDirectories(Settings.ArcLogsPath).Where(path => path.EndsWith(@"arcdps.cbtlogs\") || path.EndsWith(@"arcdps.cbtlogs")).FirstOrDefault();
            if (dir != null)
            {
                Settings.ArcLogsPath = dir;
                return true;
            }
            return false;
        }

        private void ApplyLanguage(ILanguage lang)
        {
            Text = lang.InitTitle;
            lblArcPath.Text = lang.InitArcPaht + @" (...\arcdps.cbtlogs\)";
            btnBrowse.Text = lang.InitBrowse;
            lblLang.Text = lang.InitLanguage;
            btnCancle.Text = lang.InitCancle;
            btnStart.Text = lang.InitStart;
        }

        private void CbLang_SelectedValueChanged(object sender, EventArgs e)
        {
            Language.Current = (eLanguage)(cbLang.SelectedValue ?? eLanguage.EN);
            ApplyLanguage(Language.Data);
        }
    }
}
