using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogUploader.Languages;
using LogUploader.Properties;

namespace LogUploader.GUI
{
    public partial class ProxyConfigUI : Form
    {
        private readonly Settings settings;
        private TempProxySettings initState;

        internal ProxyConfigUI(Settings settings)
        {
            this.settings = settings;
            InitializeComponent();
            ApplyLanguage(Language.Data);
        }

        private void LoadSettings(Settings settings)
        {
            initState = new TempProxySettings(settings);
            ApplyToForm(initState);
        }

        private void ApplyToForm(TempProxySettings state)
        {
            cbUseProxy.Checked = state.m_cbUseProxy;
            txtHost.Text = state.m_txtHost;
            nudPort.Value = state.m_nudPort;
            txtUser.Text = state.m_txtUser;
            txtPassword.Text = state.m_txtPassword;
        }

        private void Save()
        {
            settings.UseProxy = cbUseProxy.Checked;
            settings.ProxyAddress = txtHost.Text;
            settings.ProxyPort = (int)nudPort.Value;
            settings.ProxyUsername = txtUser.Text;
            //Helpers.SettingsHelper.StoreProxyPassword(settings, txtPassword.Text);
            initState = new TempProxySettings(settings);
        }

        private bool m_lastValueOfChanges = false;
        private bool Changes(TempProxySettings state)
        {
            try
            {
                var changed = cbUseProxy.Checked != state.m_cbUseProxy ||
                    txtHost.Text != state.m_txtHost ||
                    nudPort.Value != state.m_nudPort ||
                    txtUser.Text != state.m_txtUser ||
                    txtPassword.Text != state.m_txtPassword;
                m_lastValueOfChanges = changed;
                return changed;
            }
            catch (NullReferenceException)
            {
                return m_lastValueOfChanges;
            }

        }

        public bool HasChanges { get => Changes(initState); }

        private class TempProxySettings
        {
            public bool m_cbUseProxy { get; set; }
            public string m_txtHost { get; set; }
            public int m_nudPort { get; set; }
            public string m_txtUser { get; set; }
            public string m_txtPassword { get; set; }

            public TempProxySettings(Settings settings)
            {
                m_cbUseProxy = settings.UseProxy;
                m_txtHost = settings.ProxyAddress;
                m_nudPort = settings.ProxyPort;
                m_txtUser = settings.ProxyUsername;
                //m_txtPassword = Helpers.SettingsHelper.GetProxyPassword(settings);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ProxyConfigUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!HasChanges)
            {
                return;
            }

            var res = MessageBox.Show(Language.Data.ConfigDiscardMsgText, Language.Data.ConfigDiscardMsgTitel,
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            switch (res)
            {
                case DialogResult.Yes:
                    break;
                case DialogResult.No:
                    Save();
                    break;
                case DialogResult.Cancel:
                    e.Cancel = true;
                    return;
                default:
                    break;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
            Close();
        }

        private void ApplyLanguage(ILanguage lang)
        {
            Text = lang.ProxySettings;
            gbSettings.Text = lang.ProxySettings;
            cbUseProxy.Text = lang.ProxyUse;
            lblHost.Text = lang.ProxyHostename + ":";
            lblPort.Text = lang.ProxyPort + ":";
            lblUser.Text = lang.ProxyUser + ":";
            lblPassword.Text = lang.ProxyPassword + ":";
        }
    }
}
