using LogUploader.Data;
using LogUploader.Languages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogUploader.GUIs
{
    public partial class LogUploaderUI2 : Form
    {
        private readonly LogUploaderLogic Logic;

        internal LogUploaderUI2(LogUploaderLogic logic, bool betaEnableRaidOrga, IProgress<double> progress = null)
        {
            progress?.Report(0);
            Logic = logic;
            InitializeComponent();
            progress?.Report(0.2);
            InitPlayerDataHeader();

            ApplieLanguage();
            //TODO ApplieTheme
            //ApplieTheme();

            progress?.Report(0.25);
            BindEvents();
            BindComboBoxes();
            progress?.Report(0.40);
            UpdateData();
            cbAutoParse.Checked = Logic.EnableAutoParsing;
            cbAutoUpload.Checked = Logic.EnableAutoUpload;
            progress?.Report(0.60);
            UpdateSelectedWebHook();
            UpdateFilter();
            SetupTimers();
            SetupDatagrid();
            progress?.Report(0.9);

            filterEnabled = true;
            UpdateFilter();
            UpdateSelected();

            lblWorkCount.Text = "";
            lblWorkType.Text = "";

            if (!betaEnableRaidOrga)
            {
                gbRaidOrga.Enabled = false;
                gbRaidOrga.Visible = false;
            }

            progress?.Report(1);
        }

        private void ContextMenuGrid_Opening(object sender, CancelEventArgs e)
        {
            miOpenLink.Enabled = btnOpenDpsReport.Enabled;
            miOpenLocal.Enabled = btnOpenLocal.Enabled;
            miParse.Enabled = btnParse.Enabled;
            miUpload.Enabled = btnUpload.Enabled;
            miParseUpload.Enabled = btnParsAndUpload.Enabled;
        }

        private void DBLogDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0)
                return;
        }

        private void FilterControl_Changed(object sender, EventArgs e)
        {
            timerBossFilter.Stop();
            UpdateFilter();
        }

        private void BtnFilterToday_Click(object sender, EventArgs e)
        {
            AddFilterToday();
        }


        private void BtnFromatRest_Click(object sender, EventArgs e)
        {
            ResetFilter();
        }

        private readonly Timer timerBossFilter = new Timer();

        private void CmbFilterBoss_TextChanged(object sender, EventArgs e)
        {
            timerBossFilter.Stop();
            timerBossFilter.Start();
        }
        

        private void TimerBossFilter_Tick(object sender, EventArgs e)
        {
            timerBossFilter.Stop();
            FilterControl_Changed(sender, e);
        }

        private void DBLogDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            UpdateSelected();
        }

        private async void BtnParse_Click(object sender, EventArgs e)
        {
            var logsToDo = dBLogDataGridView.SelectedRows.Cast<DataGridViewRow>()
                .Where(row => !((bool) row.Cells["colHasHtml"].Value))
                .Select(row => new { ID = (int)row.Cells["colID"].Value, Name = GetJobNameFromRow(row) });
            await Task.Run(() =>
            {
                foreach (var element in logsToDo)
                    Logic.Parse(element.ID, $"{Language.Data.FooterParsing} {element.Name}");
            });
        }

        private void PgTop_Paint(object sender, PaintEventArgs e)
        {
            //TODO ajust color via theme
            ControlPaint.DrawBorder(e.Graphics, ((Panel) sender).ClientRectangle, Color.Green, ButtonBorderStyle.Solid);
        }

        private async void BtnOpenLocal_Click(object sender, EventArgs e)
        {
            var logsToOpen = dBLogDataGridView.SelectedRows.Cast<DataGridViewRow>()
                   .Select(row => (int)row.Cells["colID"].Value);
            await Task.Run(() => Logic.OpenLocal(logsToOpen.ToArray()));
        }

        private async void BtnUpload_Click(object sender, EventArgs e)
        {
            var logsToDo = dBLogDataGridView.SelectedRows.Cast<DataGridViewRow>()
                   .Where(row => !((bool)row.Cells["colHasLink"].Value))
                   .Select(row => new { ID = (int)row.Cells["colID"].Value, Name = GetJobNameFromRow(row) });
            await Task.Run(() =>
            {
                foreach (var element in logsToDo)
                    Logic.Upload(element.ID, $"{Language.Data.FooterUploading} {element.Name}");
            });
        }

        private async void BtnOpenDpsReport_Click(object sender, EventArgs e)
        {
            var logsToOpen = dBLogDataGridView.SelectedRows.Cast<DataGridViewRow>()
                   .Select(row => (int)row.Cells["colID"].Value);

            await Task.Run(() => Logic.OpenLink(logsToOpen.ToArray()));
        }

        private void BtnParsAndUpload_Click(object sender, EventArgs e)
        {
            var logsToDo = dBLogDataGridView.SelectedRows.Cast<DataGridViewRow>()
                      .Where(row => !(bool)row.Cells["colHasLink"].Value || !(bool)row.Cells["colHasHtml"].Value)
                      .Select(row => new { ID = (int)row.Cells["colID"].Value, Name = GetJobNameFromRow(row) });
            foreach (var element in logsToDo)
            {
                Logic.ParseAndUpload(element.ID, $"{Language.Data.FooterProcessing} {element.Name}");
            }
        }

        private readonly Timer CopyLinksTimer = new Timer();
        private async void BtnCopyLinks_Click(object sender, EventArgs e)
        {
            await CopyLinks();
        }

        private void Update_NewLogsAutoFeatures(object sender, EventArgs e)
        {
            if (Visible)
            {
                Logic.EnableAutoParsing = cbAutoParse.Checked;
                Logic.EnableAutoUpload = cbAutoUpload.Checked;
                Logic.SetSelectedAutoTasks(cbAutoParse.Checked, cbAutoUpload.Checked);
            }
        }

        private void BtnAbout_Click(object sender, EventArgs e)
        {
            new AboutUI().Show();
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            ShowSettings();
        }

        private async void BtnPostToDiscord_Click(object sender, EventArgs e)
        {
            btnPostToDiscord.Enabled = false;
            var logsToPost = dBLogDataGridView.SelectedRows.Cast<DataGridViewRow>()
                   .Select(row => (int)row.Cells["colID"].Value);
            await Logic.PostToDiscord((long)cmbWebhookSelect.SelectedValue, logsToPost.ToArray());
            btnPostToDiscord.Enabled = true;
        }

        private void LogUploaderUI2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Logic.SetSelectedWebhook((long) cmbWebhookSelect.SelectedValue);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Language.ReloadFromXML();
            ApplieLanguage();
            Refresh();
        }

        private void ViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dBLogDataGridView.SelectedRows.Count <= 0) return;
            var row = dBLogDataGridView.SelectedRows[0];
            var id = (int)row.Cells["colID"].Value;
            var log = Logic.QuickCacheLog(id);
            if (string.IsNullOrWhiteSpace(log.EvtcPath))
                    return;
            _ = Task.Run(() => System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{log.EvtcPath}\""));
        }

        private void TxtFilterDuration_KeyPress(object sender, KeyPressEventArgs e)
        {
            timerBossFilter.Stop();
            timerBossFilter.Start();
        }

        private async void LogUploaderUI2_Shown(object sender, EventArgs e)
        {
            await Task.Delay(1000);
            ShowWhatsNew();
        }

        private void BtnUpdateRaidOrga_Click(object sender, EventArgs e)
        {
            btnUpdateRaidOrga.Enabled = false;
            btnUpdateRaidOrga.Refresh();
            var logsToPost = dBLogDataGridView.SelectedRows.Cast<DataGridViewRow>()
                   .Select(row => (int)row.Cells["colID"].Value);
            var raid = (Data.RaidOrgaPlus.RaidSimple)cmbRaidOrgaTermin.SelectedValue;
            var LoadingUI = new LoadingBar("Updating " + raid.DisplayName, (ct, a, p) => Logic.UpdateRaidOrga(raid, logsToPost.ToList(), ct, a, p));
            LoadingUI.ShowDialog();
            btnUpdateRaidOrga.Enabled = true;
        }
    }
}
