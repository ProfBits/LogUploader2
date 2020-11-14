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
using LogUploader.Languages;
using LogUploader.Data.Settings;
using LogUploader.GUIs;
using LogUploader.Data;

namespace LogUploader.GUI
{
    public partial class SettingsUI : Form
    {
        private Settings Settings;
        private SettingsData CurrentState;
        private SettingsData initState;
        private bool saved = false;

        private readonly Label NoWebHooks = new Label();

        internal SettingsUI()
        {
            InitializeComponent();
            InitRest();
        }

        private void InitRest()
        {
            Settings = new Settings();
            CurrentState = new SettingsData(Settings);
            initState = new SettingsData(Settings);

            InitNoWebHook();

            cmbLang.DisplayMember = "name";
            cmbLang.ValueMember = "value";
            cmbLang.DataSource = EnumHelper.GetValues<eLanguage>().Select(e => new { name = e.GetAttribute<CombBoxView>().Name, value = e }).ToList();

            SettingsbindingSource.DataSource = CurrentState;
            SettingsbindingSource.ResetBindings(true);
            UpdateWebHooks();

            ApplyLanguage(Language.Data);
        }

        private void InitNoWebHook()
        {
            NoWebHooks.Text = "No WebHooks configured\n\nAdd a new one!";
            NoWebHooks.AutoSize = false;
            NoWebHooks.Size = new Size(200, 100);
            NoWebHooks.Margin = new Padding(30, 3, 3, 3);
            NoWebHooks.TextAlign = ContentAlignment.MiddleLeft;
        }

        private void UpdateWebHooks()
        {
            flpWebHooks.Controls.Clear();
            if (CurrentState.WebHookDB.Count() == 0)
                flpWebHooks.Controls.Add(NoWebHooks);
            else
                flpWebHooks.Controls.AddRange(CurrentState.WebHookDB.GetWebHooks().Select(webHook => GetWebHookConfig(webHook)).ToArray());
            lblWebHookCount.Text = CurrentState.WebHookDB.Count().ToString();
        }

        private WebHookConfig GetWebHookConfig(WebHook webHook)
        {
            var whConf = new WebHookConfig(webHook);
            whConf.WebHookDeleted += WhConf_WebHookDeleted;
            return whConf;
        }

        private void WhConf_WebHookDeleted(object sender, WebHookDeleteEventArgs e)
        {
            CurrentState.WebHookDB.RemoveWebHook(e.ID);
            flpWebHooks.Controls.Remove((Control)sender);
            if (CurrentState.WebHookDB.Count() == 0)
                flpWebHooks.Controls.Add(NoWebHooks);
            else
                lblWebHookCount.Text = CurrentState.WebHookDB.Count().ToString();
        }

        #region events

        private void btnOK_Click(object sender, EventArgs e)
        {
            foreach (var control in flpWebHooks.Controls)
            {
                if (control is WebHookConfig whConf)
                    whConf.SaveChanges();
            }
            CurrentState.ApplyTo(Settings);
            Settings.Save();
            saved = true;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show(Language.Data.ConfigDiscardMsgText, Language.Data.ConfigDiscardMsgTitel, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
            if (res != DialogResult.OK)
                return;

            Enabled = false;
            Settings.Reset();
            InitRest();
            Enabled = true;
        }

        private void SettingsUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (saved)
            {
                DialogResult = CurrentState == initState ? DialogResult.Cancel : DialogResult.OK;
                return;
            }
            if (!CurrentState.Equals(initState))
            {
                var res = MessageBox.Show(Language.Data.ConfigDiscardMsgTitel, Language.Data.ConfigDiscardMsgText, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                switch (res)
                {
                    case DialogResult.OK:
                        DialogResult = DialogResult.Cancel;
                        return;
                    default:
                        if (e.CloseReason == CloseReason.UserClosing)
                            e.Cancel = true;
                        break;
                }
            }
            DialogResult = DialogResult.OK;
        }

        #endregion

        #region Helper

        private void ApplyLanguage(ILanguage lang)
        {
            Text = lang.ConfigTitle;
            gbGeneral.Text = lang.ConfigGeneralTitle;
            lblArcLogsPath.Text = lang.ConfigGeneralArcPaht;
            btnBrowse.Text = lang.ConfigGeneralBrowse;
            lblLang.Text = lang.ConfigGeneralLanguage;
            gbDpsReport.Text = lang.ConfigDpsReportTitle;
            lblUserToken.Text = lang.ConfigDpsReportToken;
            lblGetUserToken.Text = lang.ConfigDpsReportGetToken;
            btnProxySettings.Text = lang.ConfigDpsReportProxy;
            gbCopyLinks.Text = lang.ConfigCopyTitle;
            cbShwoEncounterName.Text = lang.ConfigCopyBoss;
            cbShowSuccsess.Text = lang.ConfigCopySuccess;
            cbLinkInSameLine.Text = lang.ConfigCopyInline;
            cbEmptyLinesInBetween.Text = lang.ConfigCopySpace;
            cbGNEmotes.Text = lang.ConfigCopyEmotes;
            gbDiscord.Text = lang.ConfigDiscordTitle;
            lblWebHookCountDes.Text = lang.ConfigDiscordCount;
            btnAddWebHook.Text = lang.ConfigDiscordAdd;
            cbEiOnlyUploaded.Text = lang.ConfigDiscordOnlyUploaded;
            cbNameAsUser.Text = lang.ConfigDiscordNameAsUsername;
            gbEi.Text = lang.ConfigEiTitle;
            cbEiCombatReplay.Text = lang.ConfigEiCombatReplay;
            cbEiTheme.Text = lang.ConfigEiLightTheme;
            cbEIAutoUpdate.Text = lang.ConfigEiAutoUpdate;
            btnEiUpdate.Text = lang.ConfigEiUpdate;
            btnDefault.Text = lang.ConfigDefault;
            btnCancel.Text = lang.ConfigCancel;
            btnOK.Text = lang.ConfigSave;
            gbRoPlus.Text = lang.ConfigRoPlusTitle;
            lblRoPlusUser.Text = lang.ConfigRoPlusUser;
            lblRoPlusPwd.Text = lang.ConfigRoPlusPwd;

            NoWebHooks.Text = lang.ConfigDiscordNoHooks;
        }

        #endregion

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void AddWebHook_Click(object sender, EventArgs e)
        {
            if (CurrentState.WebHookDB.Count() == 0)
                flpWebHooks.Controls.Remove(NoWebHooks);
            var wh = CurrentState.WebHookDB.GetNewWebHook();
            flpWebHooks.Controls.Add(GetWebHookConfig(wh));
            flpWebHooks.VerticalScroll.Value = flpWebHooks.VerticalScroll.Maximum;
            flpWebHooks.Refresh();
            lblWebHookCount.Text = CurrentState.WebHookDB.Count().ToString();
        }

        private void linklblGetUserToken_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linklblGetUserToken.LinkVisited = true;
            System.Diagnostics.Process.Start(@"https://dps.report/getUserToken");
        }

        private async void btnEiUpdate_Click(object sender, EventArgs e)
        {
            Enabled = false;
            var tmp = btnEiUpdate.Text;
            btnEiUpdate.Text = "Updating ...";
            btnEiUpdate.Update();
            await Task.Run(() => Helper.EliteInsights.Update(initState));
            btnEiUpdate.Text = tmp;
            Enabled = true;
        }
    }
}