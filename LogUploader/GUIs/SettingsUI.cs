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
using LogUploader.GUIs;
using LogUploader.Data;
using LogUploader.Localisation;

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

            //openFileImport.Filter = "LogUploaderSettings files|*.lus|All files|*.*";
            openFileImport.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //saveFileExport.Filter = "LogUploaderSettings files|*.lus|All files|*.*";
            saveFileExport.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileExport.FileName = $"LogUploaderSettings{Environment.UserName}.lus";

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

        private void BtnOK_Click(object sender, EventArgs e)
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

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnDefault_Click(object sender, EventArgs e)
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
            lblRopLeadNote.Text = lang.ConfigRoPlusNote;
            cbGeneralPrerelease.Text = lang.ConfigGeneralAllowBeta;

            NoWebHooks.Text = lang.ConfigDiscordNoHooks;

            openFileImport.Title = lang.ConfigImportOpenTitle;
            openFileImport.Filter = lang.ConfigExportFileFilter;
            saveFileExport.Title = lang.ConfigExportSaveTitle;
            saveFileExport.Filter = lang.ConfigExportFileFilter;
            btnExport.Text = lang.ConfigExport;
            btnImport.Text = lang.ConfigImport;

#if DEBUG
            Text += " DEBUG";
#elif ALPHA
            Text += " ALPHA";
#elif BETA
            Text += " BETA";
#endif
        }

        #endregion

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

        private void LinklblGetUserToken_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linklblGetUserToken.LinkVisited = true;
            System.Diagnostics.Process.Start(@"https://dps.report/getUserToken");
        }

        private async void BtnEiUpdate_Click(object sender, EventArgs e)
        {
            Enabled = false;
            var tmp = btnEiUpdate.Text;
            btnEiUpdate.Text = "Updating ...";
            btnEiUpdate.Update();
            DialogResult res = DialogResult.OK;
            do
            {
                try
                {
                    //TODO Add progress window...
                    await EliteInsights.Update(initState);
                }
                catch (OperationCanceledException ex)
                {
                    Logger.Error("Manual EI update failed");
                    Logger.LogException(ex);
                    res = MessageBox.Show("EI uppdate failed.\nMessage:\n" + ex.Message, "Error - EI Update", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
            } while (res == DialogResult.Retry);
            btnEiUpdate.Text = tmp;
            Enabled = true;
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            var res = openFileImport.ShowDialog();
            if (res == DialogResult.Cancel) return;
            try
            {
                SettingsHelper.ImportSettings(CurrentState, openFileImport.FileName);
                SettingsbindingSource.ResetBindings(true);
                Refresh();
                return;
            }
            catch (InvalidOperationException)
            {
                do
                {
                    var inBox = new InputDialog(Language.Data.ConfigExportPwdPromptText, Language.Data.ConfigExportPwdPromptTitle);
                    res = inBox.ShowDialog();
                    if (res == DialogResult.Cancel) return;
                    try
                    {
                        SettingsHelper.ImportSettings(CurrentState, openFileImport.FileName, inBox.Input);
                        MessageBox.Show(Language.Data.ConfigImportMessageSucc, Language.Data.ConfigImportMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        SettingsbindingSource.ResetBindings(true);
                        Refresh();
                        return;
                    }
                    catch (System.Security.Cryptography.CryptographicException)
                    {
                        res = MessageBox.Show(Language.Data.ConfigExportPwdFailText, Language.Data.ConfigExportPwdFailTitle, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        if (res == DialogResult.Cancel) return;
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex);
                        MessageBox.Show(Language.Data.ConfigImportMessageFail, Language.Data.ConfigImportMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                } while (res == DialogResult.Retry);
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            var res = saveFileExport.ShowDialog();
            if (res == DialogResult.Cancel) return;
            var inBox = new InputDialog(Language.Data.ConfigExportPwdPromptNewText, Language.Data.ConfigExportPwdPromptTitle);
            res = inBox.ShowDialog();
            if (res == DialogResult.Cancel) return;
            try
            {
                SettingsHelper.ExportSettings(CurrentState, saveFileExport.FileName, inBox.Input);
                MessageBox.Show(Language.Data.ConfigExportMessageSucc, Language.Data.ConfigExportMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                MessageBox.Show(Language.Data.ConfigExportMessageFail, Language.Data.ConfigExportMessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}