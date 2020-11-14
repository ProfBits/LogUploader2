using LogUploader.Data;
using LogUploader.Helper;
using LogUploader.Languages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogUploader.GUIs
{
    partial class LogUploaderUI2
    {
        private void ApplieLanguage()
        {
            ILanguage lang = Language.Data;
            colBossName.HeaderText = lang.ColHeaderBoss;
            colDate.HeaderText = lang.ColHeaderDate;
            colSize.HeaderText = lang.ColHeaderSize;
            colDataCorrected.HeaderText = lang.ColHeaderDataCorrected;
            colDuration.HeaderText = lang.ColHeaderDuration;
            colSuccsess.HeaderText = lang.ColHeaderSuccess;
            colHpLeft.HeaderText = lang.ColHeaderHpLeft;
            colIsCM.HeaderText = lang.ColHeaderCM;
            colHasHtml.HeaderText = lang.ColHeaderParsed;
            colHasLink.HeaderText = lang.ColHeaderUploaded;

            gbFilter.Text = lang.FilterHeader;
            cbFilterBoss.Text = lang.FilterBoss;
            cbFilterHpLeft.Text = lang.FilterHPLeft;
            cbFilterDuration.Text = lang.FilterDuration;
            lblFilterDate.Text = lang.FilterDate;
            lblFilterFrom.Text = lang.FilterFrom;
            lblFilterTo.Text = lang.FilterTo;
            cbFilterSuccsess.Text = lang.FilterSuccess;
            rbFilterKill.Text = lang.FilterKill;
            rbFilterWipe.Text = lang.FilterWipe;
            btnFilterToday.Text = lang.FilterToday;
            btnFilterReset.Text = lang.FilterReset;

            gbDetails.Text = lang.DetailsHeader;
            lblDetBossDes.Text = lang.DetailsBoss;
            lblDetDateDes.Text = lang.DetailsDate;
            lblDetSizeDes.Text = lang.DetailsSize;
            lblDetCorrected.Text = lang.DetailsCorrected;
            lblDetDurationDes.Text = lang.DetailsDuration;
            lblDetSuccess.Text = lang.DetailsSuccess;
            lblDetHpDes.Text = lang.DetailsHpLeft;
            lblDetCM.Text = lang.DetailsCM;
            lblDetParsed.Text = lang.DetailsParsed;
            lblDetUpload.Text = lang.DetailsUploaded;
            lblDetOpenLocal.Text = lang.DetailsOpenLocal;
            lblDetOpenRemot.Text = lang.DetailsOpenRemote;

            gbActions.Text = lang.ActionsHeader;
            btnParse.Text = lang.ActionsParseLocal;
            btnOpenLocal.Text = lang.ActionsOpenLocal;
            btnUpload.Text = lang.ActionsUpload;
            btnOpenDpsReport.Text = lang.ActionsOpenRemote;
            btnParsAndUpload.Text = lang.ActionsParseAndUpload;
            btnCopyLinks.Text = lang.ActionsCopyLinks;
            btnPostToDiscord.Text = lang.ActionsPostToDiscord;

            gbSettings.Text = lang.SettingsHeader;
            cbAutoParse.Text = lang.SettingsAutoParse;
            cbAutoUpload.Text = lang.SettingsAutoUpload;
            btnAbout.Text = lang.SettingsAbout;
            btnSettings.Text = lang.SettingsSettings;
            lblElements.Text = lang.FooterElements;
            lblSelected.Text = lang.FooterSelected;
            lblShown.Text = lang.FooterShown;

            miParse.Text = lang.ActionsParseLocal;
            miUpload.Text = lang.ActionsUpload;
            miOpenLocal.Text = lang.ActionsOpenLocal;
            miOpenLink.Text = lang.ActionsOpenRemote;
            miParseUpload.Text = lang.ActionsParseAndUpload;
            miViewInExplorer.Text = lang.ActionsViewInExplorer;
        }

        internal LogUploaderLogic DEBUGgetLogic() => Logic;

        private PlayerData PlayerDataHeader;

        private void InitPlayerDataHeader()
        {
            PlayerDataHeader = new PlayerData();

            PlayerDataHeader.Width = 143;
            PlayerDataHeader.Margin = new Padding(0, 1, 0, 1);

            PlayerDataHeader.ClassImage = null; //Profession.Get("").Icon;
            PlayerDataHeader.Name = Language.Data.DetailsAccName;
            PlayerDataHeader.SubGroup = Language.Data.DetailsSubGroup;
            PlayerDataHeader.DPS = Language.Data.DetailsDPS;
        }

        private void BindEvents()
        {
            Logic.DataChanged += (sender, e) =>
            {
                Action a = UpdateData;
                dBLogDataGridView.Invoke(a);
            };
            Logic.JobAdded += Logic_JobAdded;
            Logic.JobsDone += Logic_JobsDone;
            Logic.JobFaulted += Logic_JobFaulted;
            Logic.JobDone += Logic_JobDone;
            Logic.JobStarted += Logic_JobStarted;
        }

        private void Logic_JobStarted(object sender, Helper.JobQueue.JobStartedEventArgs<CachedLog> e)
        {
            JobsDone++;
            Action setDataSource = () =>
            {
                if (e.Job is Helper.JobQueue.NamedJob<CachedLog> job)
                    lblWorkType.Text = $"{job.Name}";
                else
                    lblWorkType.Text = Language.Data.MiscGenericProcessing;
                lblWorkType.Visible = true;
                lblWorkCount.Text = $"{JobsDone} of {JobsTotal}";
                UpdateProgress((double)(JobsDone - 1) / JobsTotal);
                flpProgress.Refresh();
            };
            lblWorkCount.Invoke(setDataSource);
        }

        private void Logic_JobDone(object sender, Helper.JobQueue.JobDoneEventArgs<Data.CachedLog> e)
        {
        }

        private void Logic_JobFaulted(object sender, Helper.JobQueue.JobFaultedEventArgs<Data.CachedLog> e)
        {
            JobsDone++;
            Action setDataSource = () =>
            {
                lblWorkCount.Text = $"{JobsDone} of {JobsTotal}";
                UpdateProgress((double)(JobsDone - 1) / JobsTotal);
                flpProgress.Refresh();
            };
            lblWorkCount.Invoke(setDataSource);
            var message = "";
            if (e.Job is Helper.JobQueue.NamedJob<CachedLog> job)
                message += $"Job: {job.Name}\n\n";
            message += $"The log may be to small, corrupted or you are not connected to the internet for uploading.\nIf you belive this is a mistake feel free to contact ProfBits#3742 and send him this message\n\n";
            message += $"Exception:\n{e.Exception.Message}\n\n";
            message += $"{e.Exception.StackTrace}";

            if (e.Job is Helper.JobQueue.NamedJob<CachedLog> njob)
                Logger.Error($"Job: {njob.Name} FAULTED");
            Logger.LogException(e.Exception);
            var a = Task.Run(() => MessageBox.Show(message, $"Job Faulted", MessageBoxButtons.OK, MessageBoxIcon.Error));
        }

        private void Logic_JobsDone(object sender, Helper.JobQueue.JobQueueEmptyEventArgs e)
        {
            Action setDataSource = () =>
            {
                lblWorkCount.Text = $"{JobsTotal} of {JobsTotal}";
                lblWorkType.Visible = false;
                UpdateProgress(1);
                flpProgress.Refresh();
                GCTimer.Start();
            };
            lblWorkCount.Invoke(setDataSource);
            JobsTotal = 0;
            JobsDone = 0;
        }

        private int JobsTotal = 0;
        private int JobsDone = 0;

        private void Logic_JobAdded(object sender, Helper.JobQueue.JobAddedEventArgs<Data.CachedLog> e)
        {
            JobsTotal++;
            Action setDataSource = () =>
            {
                lblWorkCount.Text = $"{JobsDone} of {JobsTotal}";
                UpdateProgress((double)(JobsDone - 1) / JobsTotal);
                flpProgress.Refresh();
            };
            lblWorkCount.Invoke(setDataSource);
            GCTimer.Stop();
        }

        private void UpdateProgress(double progess)
        {
            pgBottom.Width = (int) (100 * progess);
            pgTop.Width = 100 - pgBottom.Width;
        }

        private DataTable dataTable;
        private void UpdateData()
        {
            dataTable = Logic.GetData();
            var col = dBLogDataGridView.SortedColumn;
            var dir = dBLogDataGridView.SortOrder;

            dBLogDataGridView.DataSource = dataTable;
            if (col != null && dir != SortOrder.None)
            dBLogDataGridView.Sort(col, dir == SortOrder.Ascending ? System.ComponentModel.ListSortDirection.Ascending : System.ComponentModel.ListSortDirection.Descending);
            lblTotalCount.Text = Logic.GetElementCount().ToString();
            UpdateFilter();
        }

        System.Threading.CancellationTokenSource CTSUpdateSelection = new System.Threading.CancellationTokenSource();
        private readonly object PrevLock = new object();

        private async void UpdateSelected()
        {
            GCTimer.Stop();
            CTSUpdateSelection.Cancel();
            CTSUpdateSelection = new System.Threading.CancellationTokenSource();
            var token = CTSUpdateSelection.Token;
            
            flpPlayers.Controls.Clear();
            lblSelectedCount.Text = dBLogDataGridView.SelectedRows.Count.ToString();
            var logsToDisplay = dBLogDataGridView.SelectedRows.Cast<DataGridViewRow>()
                   .Select(row => (int)row.Cells["colID"].Value)
                   .ToArray();
            if (token.IsCancellationRequested) return;
            var a = await Task.Run(() => Logic.GetQuickPreview(token, logsToDisplay));
            if (token.IsCancellationRequested) return;
            Action<LogPreview> set = SetPreview;
            lock (PrevLock)
                this.Invoke(set, a);
            if (a.MultiSelect) return;
            GCTimer.Stop();
            if (token.IsCancellationRequested) return;
            var b = await Task.Run(() => Logic.GetFullPreview(token, logsToDisplay));
            if (token.IsCancellationRequested) return;
            lock (PrevLock)
                this.Invoke(set, b);

        }

        private void SetPreview(LogPreview perv)
        {
            flpPlayers.Visible = false;
            if (perv.Players != null)
            {
                flpPlayers.Controls.Clear();
                flpPlayers.Controls.Add(PlayerDataHeader);
                flpPlayers.Controls.AddRange(perv.Players.ToArray());
                flpPlayers.Visible = true;

                flpPlayers.Refresh();
            }
            lblDetBoss.Text = perv.Name;
            lblDetSize.Text = perv.SizeKb.ToString(Languages.Language.Current == eLanguage.DE ? "0 'Kb'" : "0 'Kb'");
            if (perv.MultiSelect)
                lblDetDate.Text = perv.Date.ToString(Languages.Language.Current == eLanguage.DE ? "dd'.'MM'.'yy" : "MM'-'dd'-'yy");
            else
                lblDetDate.Text = perv.Date.ToString(Languages.Language.Current == eLanguage.DE ? "dd'.'MM'.'yy HH':'mm" : "MM'-'dd'-'yy HH':'mm");
            lblDetDuration.Text = perv.MaxDuratin.ToString(Languages.Language.Current == eLanguage.DE ? "h':'mm':'ss','fff" : "h':'mm':'ss'.'fff");
            lblDetHp.Text = perv.HPLeft.ToString("F2") + " %";

            cbDetCorrected.CheckState = perv.Corrected;
            cbDetSuccess.CheckState = perv.Success;
            cbDetCM.CheckState = perv.IsCM;
            cbDetParsed.CheckState = perv.HasHtmlCb;
            cbDetUploaded.CheckState = perv.HasLinkCb;

            lblDetOpenLocal.Visible = perv.HasHtml;
            lblDetOpenRemot.Visible = perv.HasLink;

            tpDetailsTop.Refresh();

            GCTimer.Start();

        }

        private void BindComboBoxes()
        {
            cmbFilterBoss.DataSource = Boss.All.Select(b => b.Name).Distinct().Select(n => Boss.getByName(n)).ToList();
            cmbFilterBoss.DisplayMember = "Name";
            cmbFilterBoss.ValueMember = "ID";

            cmbFilterDuration.Items.Clear();
            cmbFilterDuration.Items.AddRange(Logic.FilterRelations);
            cmbFilterDuration.SelectedIndex = cmbFilterDuration.Items.Count - 1;
            cmbFilterHp.Items.Clear();
            cmbFilterHp.Items.AddRange(Logic.FilterRelations);
            cmbFilterHp.SelectedIndex = 0;

            cmbWebhookSelect.DataSource = Logic.GetWebHooks();
            cmbWebhookSelect.DisplayMember = "Name";
            cmbWebhookSelect.ValueMember = "ID";

            cmbRaidOrgaTermin.DataSource = Logic.GetRaidOrgaTermine();
            cmbRaidOrgaTermin.DisplayMember = "DisplayName";
            cmbRaidOrgaTermin.ValueMember = "Self";
        }

        private void UpdateAutoLogActions()
        {
            Logic.EnableAutoParsing = cbAutoParse.Checked;
            Logic.EnableAutoUpload = cbAutoUpload.Checked;
        }

        private bool filterEnabled = false;

        private void UpdateFilter()
        {
            if (!filterEnabled)
                return;
            TimeSpan duration;
            if (!TimeSpan.TryParse(txtFilterDuration.Text, out duration))
                duration = new TimeSpan(1, 0, 0);
            var filterConfig = new FilterConfiguration(
                cbFilterBoss.Checked,
                cmbFilterBoss.Text,
                cbFilterHpLeft.Checked,
                cmbFilterHp.Text,
                (double)numFilterHpLeft.Value,
                cbFilterDuration.Checked,
                cmbFilterDuration.Text,
                duration,
                cbFilterSuccsess.Checked,
                rbFilterKill.Checked,
                dtpFilterFrom.Checked,
                dtpFilterFrom.Value,
                dtpFilterTo.Checked,
                dtpFilterTo.Value
                );
            ApplieFilter(filterConfig);
        }

        private void ApplieFilter(FilterConfiguration filterConfig)
        {
            if (!filterEnabled)
                return;
            // Update enables
            cmbFilterBoss.Enabled = filterConfig.BossEnabled;
            cmbFilterDuration.Enabled = filterConfig.DurationEnabled;
            txtFilterDuration.Enabled = filterConfig.DurationEnabled;
            cmbFilterHp.Enabled = filterConfig.HPEnabled;
            numFilterHpLeft.Enabled = filterConfig.HPEnabled;
            rbFilterKill.Enabled = filterConfig.SuccsessEnabled;
            rbFilterWipe.Enabled = filterConfig.SuccsessEnabled;

            //generate filter string
            var filter = Logic.GetFilter(filterConfig);
            dataTable.DefaultView.RowFilter = filter;

            //update shown
            if (string.IsNullOrWhiteSpace(filter))
            {
                lblShowenCount.Visible = false;
                lblShown.Visible = false;
            }
            else
            {
                lblShowenCount.Text = dataTable.DefaultView.Count.ToString();
                lblShowenCount.Visible = true;
                lblShown.Visible = true;
            }
        }

        private void AddFilterToday()
        {
            filterEnabled = false;
            dtpFilterFrom.Checked = true;
                dtpFilterFrom.Value = DateTime.Now.Date;
            dtpFilterTo.Checked = true;
            dtpFilterTo.Value = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
            filterEnabled = true;
            UpdateFilter();
        }

        private void ResetFilter()
        {
            filterEnabled = false;
            cbFilterBoss.Checked = false;
            cbFilterHpLeft.Checked = false;
            cbFilterDuration.Checked = false;
            cbFilterSuccsess.Checked = false;
            dtpFilterFrom.Checked = false;
            dtpFilterTo.Checked = false;
            filterEnabled = true;
            UpdateFilter();
        }

        private Timer GCTimer = new Timer();
        private void SetupTimers()
        {
            timerBossFilter.Interval = 1000;
            timerBossFilter.Tick += TimerBossFilter_Tick;

            CopyLinksTimer.Interval = 10_000;
            CopyLinksTimer.Tick += CopyLinksTimer_Tick;

            GCTimer.Interval = 10_000;
            GCTimer.Tick += GCTimer_Tick;
        }

        private void GCTimer_Tick(object sender, EventArgs e)
        {
            GCTimer.Stop();
            GC.Collect();
        }

        private void SetupDatagrid()
        {
            dBLogDataGridView.Sort(colDate, ListSortDirection.Descending);
            colDuration.DefaultCellStyle.Format = "mm':'ss','fff";
            colSize.DefaultCellStyle.Format = "0 'kb'";
        }

        private async Task CopyLinks()
        {
            btnCopyLinks.Enabled = false;
            CopyLinksTimer.Stop();
            int count = 0;
            await Task.Run(() =>
            {
                var logsToCopy = dBLogDataGridView.SelectedRows.Cast<DataGridViewRow>()
                       .Select(row => (int)row.Cells["colID"].Value);

                count = Logic.CopyLinks(this, logsToCopy.ToArray());
            });
            lblLinksCopied.Visible = true;
            lblLinksCopied.Text = $"{count} {Languages.Language.Data.ActionsCopied}";
            CopyLinksTimer.Stop();
            CopyLinksTimer.Start();
            btnCopyLinks.Enabled = true;
        }

        private void CopyLinksTimer_Tick(object sender, EventArgs e)
        {
            CopyLinksTimer.Stop();
            lblLinksCopied.Visible = false;
            lblLinksCopied.Text = "";
        }

        private string GetJobNameFromRow(DataGridViewRow row)
        {
            var bossName = (string)row.Cells["colBossName"].Value;
            if (bossName.Length > 19)
                bossName = bossName.Substring(0, 19) + "...";
            return bossName + " " + ((DateTime)row.Cells["colDate"].Value).ToString("HH':'mm");
        }

        private void ShowSettings()
        {
            Logic.SetSelectedWebhook((long)cmbWebhookSelect.SelectedValue);
            var settings = new GUI.SettingsUI();
            switch (settings.ShowDialog())
            {
                case DialogResult.OK:
                    Enabled = false;
                    Logic.ApplySettings();
                    ApplieNewSettings();
                    Enabled = true;
                    break;
                default:
                    break;
            }
        }

        private void ApplieNewSettings()
        {
            InitPlayerDataHeader();

            ApplieLanguage();
            //TODO ApplieTheme
            //ApplieTheme();
            BindComboBoxes();
            UpdateAutoLogActions();
            UpdateSelectedWebHook();

            UpdateData();

            filterEnabled = true;
            UpdateFilter();
            UpdateSelected();

            lblWorkCount.Text = "";
            lblWorkType.Text = "";
        }

        private void UpdateSelectedWebHook()
        {
            cmbWebhookSelect.SelectedValue = Logic.GetSelectedWebhook();
        }

        private void ShowWhatsNew()
        {
            var version = Helper.GP.GetVersion();
            var versionStr = $"v{version.Major}.{version.Minor}.{version.Build}";
            if (!Logic.Settings.WhatsNewShown.Equals(versionStr))
            {
                Logic.UpdateWhatsNew(versionStr);
                new WhatsNewUI(Logic.Settings, versionStr).Show();
            }
        }
    }
}
