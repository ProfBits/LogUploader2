﻿using LogUploader.Data;
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
        private LogUploaderLogic Logic;

        internal LogUploaderUI2(LogUploaderLogic logic, bool eap, bool eau, IProgress<double> progress = null)
        {
            progress?.Report(0);
            Logic = logic;
            InitializeComponent();
            progress?.Report(0.4);
            InitPlayerDataHeader();

            ApplieLanguage();
            //TODO ApplieTheme
            //ApplieTheme();
            progress?.Report(0.6);
            BindEvents();
            BindComboBoxes();
            progress?.Report(0.7);
            UpdateData();
            cbAutoParse.Checked = eap;
            cbAutoUpload.Checked = eau;
            progress?.Report(0.8);
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
            progress?.Report(1);
        }

        private void contextMenuGrid_Opening(object sender, CancelEventArgs e)
        {
            miOpenLink.Enabled = btnOpenDpsReport.Enabled;
            miOpenLocal.Enabled = btnOpenLocal.Enabled;
            miParse.Enabled = btnParse.Enabled;
            miUpload.Enabled = btnUpload.Enabled;
            miParseUpload.Enabled = btnParsAndUpload.Enabled;
        }

        private void dBLogDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0)
                return;
        }

        private void FilterControl_Changed(object sender, EventArgs e)
        {
            timerBossFilter.Stop();
            UpdateFilter();
        }

        private void btnFilterToday_Click(object sender, EventArgs e)
        {
            AddFilterToday();
        }


        private void btnFromatRest_Click(object sender, EventArgs e)
        {
            ResetFilter();
        }

        private Timer timerBossFilter = new Timer();

        private void cmbFilterBoss_TextChanged(object sender, EventArgs e)
        {
            timerBossFilter.Stop();
            timerBossFilter.Start();
        }
        

        private void TimerBossFilter_Tick(object sender, EventArgs e)
        {
            timerBossFilter.Stop();
            FilterControl_Changed(sender, e);
        }

        private void dBLogDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            UpdateSelected();
        }

        private async void btnParse_Click(object sender, EventArgs e)
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

        private void pgTop_Paint(object sender, PaintEventArgs e)
        {
            //TODO ajust color via theme
            ControlPaint.DrawBorder(e.Graphics, ((Panel) sender).ClientRectangle, Color.Green, ButtonBorderStyle.Solid);
        }

        private async void btnOpenLocal_Click(object sender, EventArgs e)
        {
            var logsToOpen = dBLogDataGridView.SelectedRows.Cast<DataGridViewRow>()
                   .Select(row => (int)row.Cells["colID"].Value);
            await Task.Run(() => Logic.OpenLocal(logsToOpen.ToArray()));
        }

        private async void btnUpload_Click(object sender, EventArgs e)
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

        private async void btnOpenDpsReport_Click(object sender, EventArgs e)
        {
            var logsToOpen = dBLogDataGridView.SelectedRows.Cast<DataGridViewRow>()
                   .Select(row => (int)row.Cells["colID"].Value);

            await Task.Run(() => Logic.OpenLink(logsToOpen.ToArray()));
        }

        private void btnParsAndUpload_Click(object sender, EventArgs e)
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
        private async void btnCopyLinks_Click(object sender, EventArgs e)
        {
            await CopyLinks();
        }

        private void Update_NewLogsAutoFeatures(object sender, EventArgs e)
        {
            Logic.EnableAutoParsing = cbAutoParse.Checked;
            Logic.EnableAutoUpload = cbAutoUpload.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new AboutUI().Show();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            ShowSettings();
        }

        private async void btnPostToDiscord_Click(object sender, EventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            Language.ReloadFromXML();
            ApplieLanguage();
            Refresh();
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dBLogDataGridView.SelectedRows.Count <= 0) return;
            var row = dBLogDataGridView.SelectedRows[0];
            var id = (int)row.Cells["colID"].Value;
            var log = Logic.QuickCacheLog(id);
            if (string.IsNullOrWhiteSpace(log.EvtcPath))
                    return;
            _ = Task.Run(() => System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{log.EvtcPath}\""));
        }

        private void txtFilterDuration_KeyPress(object sender, KeyPressEventArgs e)
        {
            timerBossFilter.Stop();
            timerBossFilter.Start();
        }
    }
}
