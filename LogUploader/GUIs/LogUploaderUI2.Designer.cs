namespace LogUploader.GUIs
{
    partial class LogUploaderUI2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                CopyLinksTimer.Dispose();
                CTSUpdateSelection.Dispose();
                GCTimer.Dispose();
                PlayerDataHeader.Dispose();
                timerBossFilter.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogUploaderUI2));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pStatus = new System.Windows.Forms.Panel();
            this.pWorkStatus = new System.Windows.Forms.Panel();
            this.flpProgress = new System.Windows.Forms.FlowLayoutPanel();
            this.pgTop = new System.Windows.Forms.Panel();
            this.pgBottom = new System.Windows.Forms.Panel();
            this.lblWorkCount = new System.Windows.Forms.Label();
            this.lblWorkType = new System.Windows.Forms.Label();
            this.pElementCounts = new System.Windows.Forms.Panel();
            this.flpSelectionStats = new System.Windows.Forms.FlowLayoutPanel();
            this.lblTotalCount = new System.Windows.Forms.Label();
            this.lblElements = new System.Windows.Forms.Label();
            this.lblSelectedCount = new System.Windows.Forms.Label();
            this.lblSelected = new System.Windows.Forms.Label();
            this.lblShowenCount = new System.Windows.Forms.Label();
            this.lblShown = new System.Windows.Forms.Label();
            this.pControl = new System.Windows.Forms.Panel();
            this.tlControlsTable = new System.Windows.Forms.TableLayoutPanel();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.lblFilterPercent = new System.Windows.Forms.Label();
            this.btnFilterReset = new System.Windows.Forms.Button();
            this.btnFilterToday = new System.Windows.Forms.Button();
            this.lblFilterTo = new System.Windows.Forms.Label();
            this.lblFilterFrom = new System.Windows.Forms.Label();
            this.lblFilterDate = new System.Windows.Forms.Label();
            this.dtpFilterTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFilterFrom = new System.Windows.Forms.DateTimePicker();
            this.rbFilterWipe = new System.Windows.Forms.RadioButton();
            this.rbFilterKill = new System.Windows.Forms.RadioButton();
            this.txtFilterDuration = new System.Windows.Forms.TextBox();
            this.numFilterHpLeft = new System.Windows.Forms.NumericUpDown();
            this.cbFilterSuccsess = new System.Windows.Forms.CheckBox();
            this.cbFilterDuration = new System.Windows.Forms.CheckBox();
            this.cbFilterHpLeft = new System.Windows.Forms.CheckBox();
            this.cbFilterBoss = new System.Windows.Forms.CheckBox();
            this.cmbFilterHp = new System.Windows.Forms.ComboBox();
            this.cmbFilterDuration = new System.Windows.Forms.ComboBox();
            this.cmbFilterBoss = new System.Windows.Forms.ComboBox();
            this.gbDetails = new System.Windows.Forms.GroupBox();
            this.tpDetailsTop = new System.Windows.Forms.TableLayoutPanel();
            this.lblDetCM = new System.Windows.Forms.Label();
            this.lblDetUpload = new System.Windows.Forms.Label();
            this.lblDetParsed = new System.Windows.Forms.Label();
            this.lblDetDate = new System.Windows.Forms.Label();
            this.lblDetSuccess = new System.Windows.Forms.Label();
            this.lblDetCorrected = new System.Windows.Forms.Label();
            this.lblDetOpenRemot = new System.Windows.Forms.Label();
            this.cbDetUploaded = new System.Windows.Forms.CheckBox();
            this.cbDetParsed = new System.Windows.Forms.CheckBox();
            this.cbDetCM = new System.Windows.Forms.CheckBox();
            this.lblDetHp = new System.Windows.Forms.Label();
            this.lblDetHpDes = new System.Windows.Forms.Label();
            this.cbDetSuccess = new System.Windows.Forms.CheckBox();
            this.lblDetDuration = new System.Windows.Forms.Label();
            this.lblDetDurationDes = new System.Windows.Forms.Label();
            this.lblDetSize = new System.Windows.Forms.Label();
            this.lblDetSizeDes = new System.Windows.Forms.Label();
            this.lblDetDateDes = new System.Windows.Forms.Label();
            this.lblDetBoss = new System.Windows.Forms.Label();
            this.lblDetBossDes = new System.Windows.Forms.Label();
            this.cbDetCorrected = new System.Windows.Forms.CheckBox();
            this.lblDetOpenLocal = new System.Windows.Forms.Label();
            this.pDetailsPlayers = new System.Windows.Forms.Panel();
            this.flpPlayers = new System.Windows.Forms.FlowLayoutPanel();
            this.gbActions = new System.Windows.Forms.GroupBox();
            this.cmbWebhookSelect = new System.Windows.Forms.ComboBox();
            this.lblLinksCopied = new System.Windows.Forms.Label();
            this.btnParsAndUpload = new System.Windows.Forms.Button();
            this.btnOpenDpsReport = new System.Windows.Forms.Button();
            this.dBLogBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnPostToDiscord = new System.Windows.Forms.Button();
            this.btnCopyLinks = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnOpenLocal = new System.Windows.Forms.Button();
            this.btnParse = new System.Windows.Forms.Button();
            this.gbSettings = new System.Windows.Forms.GroupBox();
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.cbAutoUpload = new System.Windows.Forms.CheckBox();
            this.cbAutoParse = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.gbRaidOrga = new System.Windows.Forms.GroupBox();
            this.btnRaidOrgaReload = new System.Windows.Forms.Panel();
            this.btnUpdateRaidOrga = new System.Windows.Forms.Button();
            this.cmbRaidOrgaTermin = new System.Windows.Forms.ComboBox();
            this.pGrid = new System.Windows.Forms.Panel();
            this.dBLogDataGridView = new System.Windows.Forms.DataGridView();
            this.colBossName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDataCorrected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colDuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuccsess = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colHpLeft = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIsCM = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colHasHtml = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colHasLink = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBossID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEvtcPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJsonPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHtmlPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLink = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTimeStamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDurationMs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFlags = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHasEvtc = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colHasJson = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.contextMenuGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miParse = new System.Windows.Forms.ToolStripMenuItem();
            this.miOpenLocal = new System.Windows.Forms.ToolStripMenuItem();
            this.miUpload = new System.Windows.Forms.ToolStripMenuItem();
            this.miOpenLink = new System.Windows.Forms.ToolStripMenuItem();
            this.miParseUpload = new System.Windows.Forms.ToolStripMenuItem();
            this.miViewInExplorer = new System.Windows.Forms.ToolStripMenuItem();
            this.pTop = new System.Windows.Forms.Panel();
            this.pStatus.SuspendLayout();
            this.pWorkStatus.SuspendLayout();
            this.flpProgress.SuspendLayout();
            this.pElementCounts.SuspendLayout();
            this.flpSelectionStats.SuspendLayout();
            this.pControl.SuspendLayout();
            this.tlControlsTable.SuspendLayout();
            this.gbFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFilterHpLeft)).BeginInit();
            this.gbDetails.SuspendLayout();
            this.tpDetailsTop.SuspendLayout();
            this.pDetailsPlayers.SuspendLayout();
            this.gbActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dBLogBindingSource)).BeginInit();
            this.gbSettings.SuspendLayout();
            this.gbRaidOrga.SuspendLayout();
            this.pGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dBLogDataGridView)).BeginInit();
            this.contextMenuGrid.SuspendLayout();
            this.SuspendLayout();
            // 
            // pStatus
            // 
            this.pStatus.Controls.Add(this.pWorkStatus);
            this.pStatus.Controls.Add(this.pElementCounts);
            this.pStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pStatus.Location = new System.Drawing.Point(0, 631);
            this.pStatus.Margin = new System.Windows.Forms.Padding(0);
            this.pStatus.Name = "pStatus";
            this.pStatus.Size = new System.Drawing.Size(1027, 20);
            this.pStatus.TabIndex = 1;
            // 
            // pWorkStatus
            // 
            this.pWorkStatus.Controls.Add(this.flpProgress);
            this.pWorkStatus.Dock = System.Windows.Forms.DockStyle.Right;
            this.pWorkStatus.Location = new System.Drawing.Point(667, 0);
            this.pWorkStatus.Name = "pWorkStatus";
            this.pWorkStatus.Size = new System.Drawing.Size(360, 20);
            this.pWorkStatus.TabIndex = 1;
            // 
            // flpProgress
            // 
            this.flpProgress.Controls.Add(this.pgTop);
            this.flpProgress.Controls.Add(this.pgBottom);
            this.flpProgress.Controls.Add(this.lblWorkCount);
            this.flpProgress.Controls.Add(this.lblWorkType);
            this.flpProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpProgress.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpProgress.Location = new System.Drawing.Point(0, 0);
            this.flpProgress.Name = "flpProgress";
            this.flpProgress.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.flpProgress.Size = new System.Drawing.Size(360, 20);
            this.flpProgress.TabIndex = 0;
            // 
            // pgTop
            // 
            this.pgTop.BackColor = System.Drawing.Color.Transparent;
            this.pgTop.Location = new System.Drawing.Point(321, 5);
            this.pgTop.Margin = new System.Windows.Forms.Padding(0, 2, 9, 3);
            this.pgTop.Name = "pgTop";
            this.pgTop.Size = new System.Drawing.Size(30, 10);
            this.pgTop.TabIndex = 1;
            this.pgTop.Paint += new System.Windows.Forms.PaintEventHandler(this.PgTop_Paint);
            // 
            // pgBottom
            // 
            this.pgBottom.BackColor = System.Drawing.Color.Green;
            this.pgBottom.Location = new System.Drawing.Point(251, 5);
            this.pgBottom.Margin = new System.Windows.Forms.Padding(3, 2, 0, 3);
            this.pgBottom.Name = "pgBottom";
            this.pgBottom.Size = new System.Drawing.Size(70, 10);
            this.pgBottom.TabIndex = 1;
            // 
            // lblWorkCount
            // 
            this.lblWorkCount.AutoSize = true;
            this.lblWorkCount.Location = new System.Drawing.Point(209, 3);
            this.lblWorkCount.Name = "lblWorkCount";
            this.lblWorkCount.Size = new System.Drawing.Size(36, 13);
            this.lblWorkCount.TabIndex = 0;
            this.lblWorkCount.Text = "# of #";
            // 
            // lblWorkType
            // 
            this.lblWorkType.AutoSize = true;
            this.lblWorkType.Location = new System.Drawing.Point(144, 3);
            this.lblWorkType.Name = "lblWorkType";
            this.lblWorkType.Size = new System.Drawing.Size(59, 13);
            this.lblWorkType.TabIndex = 0;
            this.lblWorkType.Text = "Processing";
            // 
            // pElementCounts
            // 
            this.pElementCounts.Controls.Add(this.flpSelectionStats);
            this.pElementCounts.Dock = System.Windows.Forms.DockStyle.Left;
            this.pElementCounts.Location = new System.Drawing.Point(0, 0);
            this.pElementCounts.Name = "pElementCounts";
            this.pElementCounts.Size = new System.Drawing.Size(300, 20);
            this.pElementCounts.TabIndex = 0;
            // 
            // flpSelectionStats
            // 
            this.flpSelectionStats.Controls.Add(this.lblTotalCount);
            this.flpSelectionStats.Controls.Add(this.lblElements);
            this.flpSelectionStats.Controls.Add(this.lblSelectedCount);
            this.flpSelectionStats.Controls.Add(this.lblSelected);
            this.flpSelectionStats.Controls.Add(this.lblShowenCount);
            this.flpSelectionStats.Controls.Add(this.lblShown);
            this.flpSelectionStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpSelectionStats.Location = new System.Drawing.Point(0, 0);
            this.flpSelectionStats.Name = "flpSelectionStats";
            this.flpSelectionStats.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.flpSelectionStats.Size = new System.Drawing.Size(300, 20);
            this.flpSelectionStats.TabIndex = 0;
            // 
            // lblTotalCount
            // 
            this.lblTotalCount.AutoSize = true;
            this.lblTotalCount.Location = new System.Drawing.Point(3, 3);
            this.lblTotalCount.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblTotalCount.MinimumSize = new System.Drawing.Size(30, 0);
            this.lblTotalCount.Name = "lblTotalCount";
            this.lblTotalCount.Size = new System.Drawing.Size(30, 13);
            this.lblTotalCount.TabIndex = 0;
            this.lblTotalCount.Text = "##";
            this.lblTotalCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblElements
            // 
            this.lblElements.AutoSize = true;
            this.lblElements.Location = new System.Drawing.Point(33, 3);
            this.lblElements.Margin = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.lblElements.Name = "lblElements";
            this.lblElements.Size = new System.Drawing.Size(50, 13);
            this.lblElements.TabIndex = 0;
            this.lblElements.Text = "Elements";
            // 
            // lblSelectedCount
            // 
            this.lblSelectedCount.AutoSize = true;
            this.lblSelectedCount.Location = new System.Drawing.Point(101, 3);
            this.lblSelectedCount.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblSelectedCount.MinimumSize = new System.Drawing.Size(30, 0);
            this.lblSelectedCount.Name = "lblSelectedCount";
            this.lblSelectedCount.Size = new System.Drawing.Size(30, 13);
            this.lblSelectedCount.TabIndex = 0;
            this.lblSelectedCount.Text = "##";
            this.lblSelectedCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblSelected
            // 
            this.lblSelected.AutoSize = true;
            this.lblSelected.Location = new System.Drawing.Point(131, 3);
            this.lblSelected.Margin = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.lblSelected.Name = "lblSelected";
            this.lblSelected.Size = new System.Drawing.Size(49, 13);
            this.lblSelected.TabIndex = 0;
            this.lblSelected.Text = "Selected";
            // 
            // lblShowenCount
            // 
            this.lblShowenCount.AutoSize = true;
            this.lblShowenCount.Location = new System.Drawing.Point(198, 3);
            this.lblShowenCount.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblShowenCount.MinimumSize = new System.Drawing.Size(30, 0);
            this.lblShowenCount.Name = "lblShowenCount";
            this.lblShowenCount.Size = new System.Drawing.Size(30, 13);
            this.lblShowenCount.TabIndex = 0;
            this.lblShowenCount.Text = "##";
            this.lblShowenCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblShown
            // 
            this.lblShown.AutoSize = true;
            this.lblShown.Location = new System.Drawing.Point(228, 3);
            this.lblShown.Margin = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.lblShown.Name = "lblShown";
            this.lblShown.Size = new System.Drawing.Size(46, 13);
            this.lblShown.TabIndex = 0;
            this.lblShown.Text = "Showen";
            // 
            // pControl
            // 
            this.pControl.Controls.Add(this.tlControlsTable);
            this.pControl.Dock = System.Windows.Forms.DockStyle.Right;
            this.pControl.Location = new System.Drawing.Point(670, 1);
            this.pControl.Name = "pControl";
            this.pControl.Size = new System.Drawing.Size(357, 630);
            this.pControl.TabIndex = 2;
            // 
            // tlControlsTable
            // 
            this.tlControlsTable.ColumnCount = 3;
            this.tlControlsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tlControlsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlControlsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlControlsTable.Controls.Add(this.gbFilter, 1, 0);
            this.tlControlsTable.Controls.Add(this.gbDetails, 1, 1);
            this.tlControlsTable.Controls.Add(this.gbActions, 2, 1);
            this.tlControlsTable.Controls.Add(this.gbSettings, 2, 4);
            this.tlControlsTable.Controls.Add(this.button2, 2, 3);
            this.tlControlsTable.Controls.Add(this.gbRaidOrga, 2, 2);
            this.tlControlsTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlControlsTable.Location = new System.Drawing.Point(0, 0);
            this.tlControlsTable.Name = "tlControlsTable";
            this.tlControlsTable.RowCount = 5;
            this.tlControlsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tlControlsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 210F));
            this.tlControlsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tlControlsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlControlsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlControlsTable.Size = new System.Drawing.Size(357, 630);
            this.tlControlsTable.TabIndex = 0;
            // 
            // gbFilter
            // 
            this.tlControlsTable.SetColumnSpan(this.gbFilter, 2);
            this.gbFilter.Controls.Add(this.lblFilterPercent);
            this.gbFilter.Controls.Add(this.btnFilterReset);
            this.gbFilter.Controls.Add(this.btnFilterToday);
            this.gbFilter.Controls.Add(this.lblFilterTo);
            this.gbFilter.Controls.Add(this.lblFilterFrom);
            this.gbFilter.Controls.Add(this.lblFilterDate);
            this.gbFilter.Controls.Add(this.dtpFilterTo);
            this.gbFilter.Controls.Add(this.dtpFilterFrom);
            this.gbFilter.Controls.Add(this.rbFilterWipe);
            this.gbFilter.Controls.Add(this.rbFilterKill);
            this.gbFilter.Controls.Add(this.txtFilterDuration);
            this.gbFilter.Controls.Add(this.numFilterHpLeft);
            this.gbFilter.Controls.Add(this.cbFilterSuccsess);
            this.gbFilter.Controls.Add(this.cbFilterDuration);
            this.gbFilter.Controls.Add(this.cbFilterHpLeft);
            this.gbFilter.Controls.Add(this.cbFilterBoss);
            this.gbFilter.Controls.Add(this.cmbFilterHp);
            this.gbFilter.Controls.Add(this.cmbFilterDuration);
            this.gbFilter.Controls.Add(this.cmbFilterBoss);
            this.gbFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbFilter.Location = new System.Drawing.Point(8, 3);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(346, 154);
            this.gbFilter.TabIndex = 0;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Filter";
            // 
            // lblFilterPercent
            // 
            this.lblFilterPercent.AutoSize = true;
            this.lblFilterPercent.Location = new System.Drawing.Point(197, 46);
            this.lblFilterPercent.Name = "lblFilterPercent";
            this.lblFilterPercent.Size = new System.Drawing.Size(15, 13);
            this.lblFilterPercent.TabIndex = 11;
            this.lblFilterPercent.Text = "%";
            // 
            // btnFilterReset
            // 
            this.btnFilterReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFilterReset.Location = new System.Drawing.Point(289, 126);
            this.btnFilterReset.Name = "btnFilterReset";
            this.btnFilterReset.Size = new System.Drawing.Size(53, 23);
            this.btnFilterReset.TabIndex = 10;
            this.btnFilterReset.Text = "Reset";
            this.btnFilterReset.UseVisualStyleBackColor = true;
            this.btnFilterReset.Click += new System.EventHandler(this.BtnFromatRest_Click);
            // 
            // btnFilterToday
            // 
            this.btnFilterToday.Location = new System.Drawing.Point(226, 97);
            this.btnFilterToday.Name = "btnFilterToday";
            this.btnFilterToday.Size = new System.Drawing.Size(75, 23);
            this.btnFilterToday.TabIndex = 9;
            this.btnFilterToday.Text = "Today";
            this.btnFilterToday.UseVisualStyleBackColor = true;
            this.btnFilterToday.Click += new System.EventHandler(this.BtnFilterToday_Click);
            // 
            // lblFilterTo
            // 
            this.lblFilterTo.AutoSize = true;
            this.lblFilterTo.Location = new System.Drawing.Point(52, 130);
            this.lblFilterTo.Name = "lblFilterTo";
            this.lblFilterTo.Size = new System.Drawing.Size(20, 13);
            this.lblFilterTo.TabIndex = 8;
            this.lblFilterTo.Text = "To";
            // 
            // lblFilterFrom
            // 
            this.lblFilterFrom.AutoSize = true;
            this.lblFilterFrom.Location = new System.Drawing.Point(42, 104);
            this.lblFilterFrom.Name = "lblFilterFrom";
            this.lblFilterFrom.Size = new System.Drawing.Size(30, 13);
            this.lblFilterFrom.TabIndex = 8;
            this.lblFilterFrom.Text = "From";
            // 
            // lblFilterDate
            // 
            this.lblFilterDate.AutoSize = true;
            this.lblFilterDate.Location = new System.Drawing.Point(3, 104);
            this.lblFilterDate.Name = "lblFilterDate";
            this.lblFilterDate.Size = new System.Drawing.Size(30, 13);
            this.lblFilterDate.TabIndex = 7;
            this.lblFilterDate.Text = "Date";
            // 
            // dtpFilterTo
            // 
            this.dtpFilterTo.Checked = false;
            this.dtpFilterTo.CustomFormat = "dd\'.\'MM\'.\'yyyy HH\':\'mm";
            this.dtpFilterTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFilterTo.Location = new System.Drawing.Point(78, 124);
            this.dtpFilterTo.Name = "dtpFilterTo";
            this.dtpFilterTo.ShowCheckBox = true;
            this.dtpFilterTo.Size = new System.Drawing.Size(142, 20);
            this.dtpFilterTo.TabIndex = 6;
            this.dtpFilterTo.Value = new System.DateTime(2020, 5, 17, 10, 18, 0, 0);
            this.dtpFilterTo.ValueChanged += new System.EventHandler(this.FilterControl_Changed);
            // 
            // dtpFilterFrom
            // 
            this.dtpFilterFrom.Checked = false;
            this.dtpFilterFrom.CustomFormat = "dd\'.\'MM\'.\'yyyy HH\':\'mm";
            this.dtpFilterFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFilterFrom.Location = new System.Drawing.Point(78, 98);
            this.dtpFilterFrom.Name = "dtpFilterFrom";
            this.dtpFilterFrom.ShowCheckBox = true;
            this.dtpFilterFrom.Size = new System.Drawing.Size(142, 20);
            this.dtpFilterFrom.TabIndex = 6;
            this.dtpFilterFrom.Value = new System.DateTime(2020, 5, 17, 10, 18, 0, 0);
            this.dtpFilterFrom.ValueChanged += new System.EventHandler(this.FilterControl_Changed);
            // 
            // rbFilterWipe
            // 
            this.rbFilterWipe.AutoSize = true;
            this.rbFilterWipe.Enabled = false;
            this.rbFilterWipe.Location = new System.Drawing.Point(284, 44);
            this.rbFilterWipe.Name = "rbFilterWipe";
            this.rbFilterWipe.Size = new System.Drawing.Size(50, 17);
            this.rbFilterWipe.TabIndex = 5;
            this.rbFilterWipe.Text = "Wipe";
            this.rbFilterWipe.UseVisualStyleBackColor = true;
            this.rbFilterWipe.CheckedChanged += new System.EventHandler(this.FilterControl_Changed);
            // 
            // rbFilterKill
            // 
            this.rbFilterKill.AutoSize = true;
            this.rbFilterKill.Checked = true;
            this.rbFilterKill.Enabled = false;
            this.rbFilterKill.Location = new System.Drawing.Point(240, 44);
            this.rbFilterKill.Name = "rbFilterKill";
            this.rbFilterKill.Size = new System.Drawing.Size(38, 17);
            this.rbFilterKill.TabIndex = 5;
            this.rbFilterKill.TabStop = true;
            this.rbFilterKill.Text = "Kill";
            this.rbFilterKill.UseVisualStyleBackColor = true;
            this.rbFilterKill.CheckedChanged += new System.EventHandler(this.FilterControl_Changed);
            // 
            // txtFilterDuration
            // 
            this.txtFilterDuration.Enabled = false;
            this.txtFilterDuration.Location = new System.Drawing.Point(126, 70);
            this.txtFilterDuration.Name = "txtFilterDuration";
            this.txtFilterDuration.Size = new System.Drawing.Size(86, 20);
            this.txtFilterDuration.TabIndex = 4;
            this.txtFilterDuration.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtFilterDuration_KeyPress);
            // 
            // numFilterHpLeft
            // 
            this.numFilterHpLeft.DecimalPlaces = 2;
            this.numFilterHpLeft.Enabled = false;
            this.numFilterHpLeft.Location = new System.Drawing.Point(126, 44);
            this.numFilterHpLeft.Name = "numFilterHpLeft";
            this.numFilterHpLeft.Size = new System.Drawing.Size(65, 20);
            this.numFilterHpLeft.TabIndex = 3;
            this.numFilterHpLeft.ValueChanged += new System.EventHandler(this.FilterControl_Changed);
            this.numFilterHpLeft.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtFilterDuration_KeyPress);
            // 
            // cbFilterSuccsess
            // 
            this.cbFilterSuccsess.AutoSize = true;
            this.cbFilterSuccsess.Location = new System.Drawing.Point(240, 19);
            this.cbFilterSuccsess.Name = "cbFilterSuccsess";
            this.cbFilterSuccsess.Size = new System.Drawing.Size(72, 17);
            this.cbFilterSuccsess.TabIndex = 2;
            this.cbFilterSuccsess.Text = "Succsess";
            this.cbFilterSuccsess.UseVisualStyleBackColor = true;
            this.cbFilterSuccsess.CheckedChanged += new System.EventHandler(this.FilterControl_Changed);
            // 
            // cbFilterDuration
            // 
            this.cbFilterDuration.AutoSize = true;
            this.cbFilterDuration.Location = new System.Drawing.Point(6, 72);
            this.cbFilterDuration.Name = "cbFilterDuration";
            this.cbFilterDuration.Size = new System.Drawing.Size(66, 17);
            this.cbFilterDuration.TabIndex = 2;
            this.cbFilterDuration.Text = "Duration";
            this.cbFilterDuration.UseVisualStyleBackColor = true;
            this.cbFilterDuration.CheckedChanged += new System.EventHandler(this.FilterControl_Changed);
            // 
            // cbFilterHpLeft
            // 
            this.cbFilterHpLeft.AutoSize = true;
            this.cbFilterHpLeft.Location = new System.Drawing.Point(6, 45);
            this.cbFilterHpLeft.Name = "cbFilterHpLeft";
            this.cbFilterHpLeft.Size = new System.Drawing.Size(57, 17);
            this.cbFilterHpLeft.TabIndex = 2;
            this.cbFilterHpLeft.Text = "Hp left";
            this.cbFilterHpLeft.UseVisualStyleBackColor = true;
            this.cbFilterHpLeft.CheckedChanged += new System.EventHandler(this.FilterControl_Changed);
            // 
            // cbFilterBoss
            // 
            this.cbFilterBoss.AutoSize = true;
            this.cbFilterBoss.Location = new System.Drawing.Point(6, 19);
            this.cbFilterBoss.Name = "cbFilterBoss";
            this.cbFilterBoss.Size = new System.Drawing.Size(49, 17);
            this.cbFilterBoss.TabIndex = 2;
            this.cbFilterBoss.Text = "Boss";
            this.cbFilterBoss.UseVisualStyleBackColor = true;
            this.cbFilterBoss.CheckedChanged += new System.EventHandler(this.FilterControl_Changed);
            // 
            // cmbFilterHp
            // 
            this.cmbFilterHp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterHp.Enabled = false;
            this.cmbFilterHp.FormattingEnabled = true;
            this.cmbFilterHp.Items.AddRange(new object[] {
            "<",
            "<=",
            "=",
            ">=",
            ">"});
            this.cmbFilterHp.Location = new System.Drawing.Point(78, 43);
            this.cmbFilterHp.Name = "cmbFilterHp";
            this.cmbFilterHp.Size = new System.Drawing.Size(42, 21);
            this.cmbFilterHp.TabIndex = 1;
            this.cmbFilterHp.SelectedIndexChanged += new System.EventHandler(this.FilterControl_Changed);
            // 
            // cmbFilterDuration
            // 
            this.cmbFilterDuration.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterDuration.Enabled = false;
            this.cmbFilterDuration.FormattingEnabled = true;
            this.cmbFilterDuration.Items.AddRange(new object[] {
            "<",
            "<=",
            "=",
            ">=",
            ">"});
            this.cmbFilterDuration.Location = new System.Drawing.Point(78, 70);
            this.cmbFilterDuration.Name = "cmbFilterDuration";
            this.cmbFilterDuration.Size = new System.Drawing.Size(42, 21);
            this.cmbFilterDuration.TabIndex = 1;
            this.cmbFilterDuration.SelectedIndexChanged += new System.EventHandler(this.FilterControl_Changed);
            // 
            // cmbFilterBoss
            // 
            this.cmbFilterBoss.Enabled = false;
            this.cmbFilterBoss.FormattingEnabled = true;
            this.cmbFilterBoss.Location = new System.Drawing.Point(61, 17);
            this.cmbFilterBoss.Name = "cmbFilterBoss";
            this.cmbFilterBoss.Size = new System.Drawing.Size(151, 21);
            this.cmbFilterBoss.TabIndex = 1;
            this.cmbFilterBoss.SelectedIndexChanged += new System.EventHandler(this.FilterControl_Changed);
            this.cmbFilterBoss.TextChanged += new System.EventHandler(this.CmbFilterBoss_TextChanged);
            // 
            // gbDetails
            // 
            this.gbDetails.Controls.Add(this.tpDetailsTop);
            this.gbDetails.Controls.Add(this.pDetailsPlayers);
            this.gbDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbDetails.Location = new System.Drawing.Point(8, 163);
            this.gbDetails.Name = "gbDetails";
            this.tlControlsTable.SetRowSpan(this.gbDetails, 4);
            this.gbDetails.Size = new System.Drawing.Size(170, 464);
            this.gbDetails.TabIndex = 1;
            this.gbDetails.TabStop = false;
            this.gbDetails.Text = "Details";
            // 
            // tpDetailsTop
            // 
            this.tpDetailsTop.ColumnCount = 3;
            this.tpDetailsTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tpDetailsTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tpDetailsTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tpDetailsTop.Controls.Add(this.lblDetCM, 0, 7);
            this.tpDetailsTop.Controls.Add(this.lblDetUpload, 0, 9);
            this.tpDetailsTop.Controls.Add(this.lblDetParsed, 0, 8);
            this.tpDetailsTop.Controls.Add(this.lblDetDate, 1, 1);
            this.tpDetailsTop.Controls.Add(this.lblDetSuccess, 0, 5);
            this.tpDetailsTop.Controls.Add(this.lblDetCorrected, 0, 3);
            this.tpDetailsTop.Controls.Add(this.lblDetOpenRemot, 2, 9);
            this.tpDetailsTop.Controls.Add(this.cbDetUploaded, 1, 9);
            this.tpDetailsTop.Controls.Add(this.cbDetParsed, 1, 8);
            this.tpDetailsTop.Controls.Add(this.cbDetCM, 1, 7);
            this.tpDetailsTop.Controls.Add(this.lblDetHp, 1, 6);
            this.tpDetailsTop.Controls.Add(this.lblDetHpDes, 0, 6);
            this.tpDetailsTop.Controls.Add(this.cbDetSuccess, 1, 5);
            this.tpDetailsTop.Controls.Add(this.lblDetDuration, 1, 4);
            this.tpDetailsTop.Controls.Add(this.lblDetDurationDes, 0, 4);
            this.tpDetailsTop.Controls.Add(this.lblDetSize, 1, 2);
            this.tpDetailsTop.Controls.Add(this.lblDetSizeDes, 0, 2);
            this.tpDetailsTop.Controls.Add(this.lblDetDateDes, 0, 1);
            this.tpDetailsTop.Controls.Add(this.lblDetBoss, 1, 0);
            this.tpDetailsTop.Controls.Add(this.lblDetBossDes, 0, 0);
            this.tpDetailsTop.Controls.Add(this.cbDetCorrected, 1, 3);
            this.tpDetailsTop.Controls.Add(this.lblDetOpenLocal, 2, 8);
            this.tpDetailsTop.Location = new System.Drawing.Point(6, 19);
            this.tpDetailsTop.Name = "tpDetailsTop";
            this.tpDetailsTop.RowCount = 10;
            this.tpDetailsTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tpDetailsTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tpDetailsTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tpDetailsTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tpDetailsTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tpDetailsTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tpDetailsTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tpDetailsTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tpDetailsTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tpDetailsTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tpDetailsTop.Size = new System.Drawing.Size(158, 190);
            this.tpDetailsTop.TabIndex = 1;
            // 
            // lblDetCM
            // 
            this.lblDetCM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetCM.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblDetCM.Location = new System.Drawing.Point(0, 134);
            this.lblDetCM.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.lblDetCM.Name = "lblDetCM";
            this.lblDetCM.Size = new System.Drawing.Size(71, 17);
            this.lblDetCM.TabIndex = 22;
            this.lblDetCM.Text = "CM";
            this.lblDetCM.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetUpload
            // 
            this.lblDetUpload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetUpload.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblDetUpload.Location = new System.Drawing.Point(0, 172);
            this.lblDetUpload.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.lblDetUpload.Name = "lblDetUpload";
            this.lblDetUpload.Size = new System.Drawing.Size(71, 17);
            this.lblDetUpload.TabIndex = 21;
            this.lblDetUpload.Text = "Uploaded";
            this.lblDetUpload.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetParsed
            // 
            this.lblDetParsed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetParsed.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblDetParsed.Location = new System.Drawing.Point(0, 153);
            this.lblDetParsed.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.lblDetParsed.Name = "lblDetParsed";
            this.lblDetParsed.Size = new System.Drawing.Size(71, 17);
            this.lblDetParsed.TabIndex = 20;
            this.lblDetParsed.Text = "Parsed";
            this.lblDetParsed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetDate
            // 
            this.lblDetDate.AutoEllipsis = true;
            this.tpDetailsTop.SetColumnSpan(this.lblDetDate, 2);
            this.lblDetDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetDate.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblDetDate.Location = new System.Drawing.Point(71, 20);
            this.lblDetDate.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.lblDetDate.Name = "lblDetDate";
            this.lblDetDate.Size = new System.Drawing.Size(87, 17);
            this.lblDetDate.TabIndex = 19;
            // 
            // lblDetSuccess
            // 
            this.lblDetSuccess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetSuccess.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblDetSuccess.Location = new System.Drawing.Point(0, 96);
            this.lblDetSuccess.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.lblDetSuccess.Name = "lblDetSuccess";
            this.lblDetSuccess.Size = new System.Drawing.Size(71, 17);
            this.lblDetSuccess.TabIndex = 18;
            this.lblDetSuccess.Text = "Success";
            this.lblDetSuccess.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetCorrected
            // 
            this.lblDetCorrected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetCorrected.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblDetCorrected.Location = new System.Drawing.Point(0, 58);
            this.lblDetCorrected.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.lblDetCorrected.Name = "lblDetCorrected";
            this.lblDetCorrected.Size = new System.Drawing.Size(71, 17);
            this.lblDetCorrected.TabIndex = 17;
            this.lblDetCorrected.Text = "Corrected";
            this.lblDetCorrected.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetOpenRemot
            // 
            this.lblDetOpenRemot.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblDetOpenRemot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetOpenRemot.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblDetOpenRemot.Location = new System.Drawing.Point(86, 172);
            this.lblDetOpenRemot.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.lblDetOpenRemot.Name = "lblDetOpenRemot";
            this.lblDetOpenRemot.Size = new System.Drawing.Size(72, 17);
            this.lblDetOpenRemot.TabIndex = 16;
            this.lblDetOpenRemot.Text = "open";
            this.lblDetOpenRemot.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblDetOpenRemot.Click += new System.EventHandler(this.BtnOpenDpsReport_Click);
            // 
            // cbDetUploaded
            // 
            this.cbDetUploaded.AutoSize = true;
            this.cbDetUploaded.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbDetUploaded.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbDetUploaded.Enabled = false;
            this.cbDetUploaded.Location = new System.Drawing.Point(71, 172);
            this.cbDetUploaded.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.cbDetUploaded.Name = "cbDetUploaded";
            this.cbDetUploaded.Size = new System.Drawing.Size(15, 17);
            this.cbDetUploaded.TabIndex = 14;
            this.cbDetUploaded.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbDetUploaded.ThreeState = true;
            this.cbDetUploaded.UseVisualStyleBackColor = true;
            // 
            // cbDetParsed
            // 
            this.cbDetParsed.AutoSize = true;
            this.cbDetParsed.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbDetParsed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbDetParsed.Enabled = false;
            this.cbDetParsed.Location = new System.Drawing.Point(71, 153);
            this.cbDetParsed.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.cbDetParsed.Name = "cbDetParsed";
            this.cbDetParsed.Size = new System.Drawing.Size(15, 17);
            this.cbDetParsed.TabIndex = 13;
            this.cbDetParsed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbDetParsed.ThreeState = true;
            this.cbDetParsed.UseVisualStyleBackColor = true;
            // 
            // cbDetCM
            // 
            this.cbDetCM.AutoSize = true;
            this.cbDetCM.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbDetCM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbDetCM.Enabled = false;
            this.cbDetCM.Location = new System.Drawing.Point(71, 134);
            this.cbDetCM.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.cbDetCM.Name = "cbDetCM";
            this.cbDetCM.Size = new System.Drawing.Size(15, 17);
            this.cbDetCM.TabIndex = 12;
            this.cbDetCM.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbDetCM.ThreeState = true;
            this.cbDetCM.UseVisualStyleBackColor = true;
            // 
            // lblDetHp
            // 
            this.tpDetailsTop.SetColumnSpan(this.lblDetHp, 2);
            this.lblDetHp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetHp.Location = new System.Drawing.Point(71, 115);
            this.lblDetHp.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.lblDetHp.Name = "lblDetHp";
            this.lblDetHp.Size = new System.Drawing.Size(87, 17);
            this.lblDetHp.TabIndex = 11;
            this.lblDetHp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDetHpDes
            // 
            this.lblDetHpDes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetHpDes.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblDetHpDes.Location = new System.Drawing.Point(0, 115);
            this.lblDetHpDes.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.lblDetHpDes.Name = "lblDetHpDes";
            this.lblDetHpDes.Size = new System.Drawing.Size(71, 17);
            this.lblDetHpDes.TabIndex = 10;
            this.lblDetHpDes.Text = "HP left";
            this.lblDetHpDes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbDetSuccess
            // 
            this.cbDetSuccess.AutoSize = true;
            this.cbDetSuccess.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbDetSuccess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbDetSuccess.Enabled = false;
            this.cbDetSuccess.Location = new System.Drawing.Point(71, 96);
            this.cbDetSuccess.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.cbDetSuccess.Name = "cbDetSuccess";
            this.cbDetSuccess.Size = new System.Drawing.Size(15, 17);
            this.cbDetSuccess.TabIndex = 9;
            this.cbDetSuccess.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbDetSuccess.ThreeState = true;
            this.cbDetSuccess.UseVisualStyleBackColor = true;
            // 
            // lblDetDuration
            // 
            this.tpDetailsTop.SetColumnSpan(this.lblDetDuration, 2);
            this.lblDetDuration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetDuration.Location = new System.Drawing.Point(71, 77);
            this.lblDetDuration.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.lblDetDuration.Name = "lblDetDuration";
            this.lblDetDuration.Size = new System.Drawing.Size(87, 17);
            this.lblDetDuration.TabIndex = 8;
            this.lblDetDuration.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDetDurationDes
            // 
            this.lblDetDurationDes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetDurationDes.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblDetDurationDes.Location = new System.Drawing.Point(0, 77);
            this.lblDetDurationDes.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.lblDetDurationDes.Name = "lblDetDurationDes";
            this.lblDetDurationDes.Size = new System.Drawing.Size(71, 17);
            this.lblDetDurationDes.TabIndex = 7;
            this.lblDetDurationDes.Text = "Duration";
            this.lblDetDurationDes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDetSize
            // 
            this.tpDetailsTop.SetColumnSpan(this.lblDetSize, 2);
            this.lblDetSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetSize.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblDetSize.Location = new System.Drawing.Point(71, 39);
            this.lblDetSize.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.lblDetSize.Name = "lblDetSize";
            this.lblDetSize.Size = new System.Drawing.Size(87, 17);
            this.lblDetSize.TabIndex = 5;
            // 
            // lblDetSizeDes
            // 
            this.lblDetSizeDes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetSizeDes.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblDetSizeDes.Location = new System.Drawing.Point(0, 39);
            this.lblDetSizeDes.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.lblDetSizeDes.Name = "lblDetSizeDes";
            this.lblDetSizeDes.Size = new System.Drawing.Size(71, 17);
            this.lblDetSizeDes.TabIndex = 4;
            this.lblDetSizeDes.Text = "Size:";
            this.lblDetSizeDes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblDetDateDes
            // 
            this.lblDetDateDes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetDateDes.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblDetDateDes.Location = new System.Drawing.Point(0, 20);
            this.lblDetDateDes.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.lblDetDateDes.Name = "lblDetDateDes";
            this.lblDetDateDes.Size = new System.Drawing.Size(71, 17);
            this.lblDetDateDes.TabIndex = 2;
            this.lblDetDateDes.Text = "Date:";
            this.lblDetDateDes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblDetBoss
            // 
            this.lblDetBoss.AutoEllipsis = true;
            this.tpDetailsTop.SetColumnSpan(this.lblDetBoss, 2);
            this.lblDetBoss.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetBoss.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblDetBoss.Location = new System.Drawing.Point(71, 1);
            this.lblDetBoss.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.lblDetBoss.MaximumSize = new System.Drawing.Size(87, 19);
            this.lblDetBoss.Name = "lblDetBoss";
            this.lblDetBoss.Size = new System.Drawing.Size(87, 17);
            this.lblDetBoss.TabIndex = 1;
            // 
            // lblDetBossDes
            // 
            this.lblDetBossDes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetBossDes.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblDetBossDes.Location = new System.Drawing.Point(0, 1);
            this.lblDetBossDes.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.lblDetBossDes.Name = "lblDetBossDes";
            this.lblDetBossDes.Size = new System.Drawing.Size(71, 17);
            this.lblDetBossDes.TabIndex = 0;
            this.lblDetBossDes.Text = "Boss:";
            this.lblDetBossDes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cbDetCorrected
            // 
            this.cbDetCorrected.AutoSize = true;
            this.cbDetCorrected.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbDetCorrected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbDetCorrected.Enabled = false;
            this.cbDetCorrected.Location = new System.Drawing.Point(71, 58);
            this.cbDetCorrected.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.cbDetCorrected.Name = "cbDetCorrected";
            this.cbDetCorrected.Size = new System.Drawing.Size(15, 17);
            this.cbDetCorrected.TabIndex = 6;
            this.cbDetCorrected.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbDetCorrected.ThreeState = true;
            this.cbDetCorrected.UseVisualStyleBackColor = true;
            // 
            // lblDetOpenLocal
            // 
            this.lblDetOpenLocal.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblDetOpenLocal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetOpenLocal.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblDetOpenLocal.Location = new System.Drawing.Point(86, 153);
            this.lblDetOpenLocal.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.lblDetOpenLocal.Name = "lblDetOpenLocal";
            this.lblDetOpenLocal.Size = new System.Drawing.Size(72, 17);
            this.lblDetOpenLocal.TabIndex = 15;
            this.lblDetOpenLocal.Text = "open";
            this.lblDetOpenLocal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblDetOpenLocal.Click += new System.EventHandler(this.BtnOpenLocal_Click);
            // 
            // pDetailsPlayers
            // 
            this.pDetailsPlayers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pDetailsPlayers.Controls.Add(this.flpPlayers);
            this.pDetailsPlayers.Location = new System.Drawing.Point(5, 214);
            this.pDetailsPlayers.Margin = new System.Windows.Forms.Padding(2);
            this.pDetailsPlayers.Name = "pDetailsPlayers";
            this.pDetailsPlayers.Size = new System.Drawing.Size(160, 244);
            this.pDetailsPlayers.TabIndex = 0;
            // 
            // flpPlayers
            // 
            this.flpPlayers.AutoScroll = true;
            this.flpPlayers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpPlayers.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpPlayers.Location = new System.Drawing.Point(0, 0);
            this.flpPlayers.Margin = new System.Windows.Forms.Padding(0);
            this.flpPlayers.Name = "flpPlayers";
            this.flpPlayers.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.flpPlayers.Size = new System.Drawing.Size(160, 244);
            this.flpPlayers.TabIndex = 1;
            this.flpPlayers.WrapContents = false;
            // 
            // gbActions
            // 
            this.gbActions.Controls.Add(this.cmbWebhookSelect);
            this.gbActions.Controls.Add(this.lblLinksCopied);
            this.gbActions.Controls.Add(this.btnParsAndUpload);
            this.gbActions.Controls.Add(this.btnOpenDpsReport);
            this.gbActions.Controls.Add(this.btnPostToDiscord);
            this.gbActions.Controls.Add(this.btnCopyLinks);
            this.gbActions.Controls.Add(this.btnUpload);
            this.gbActions.Controls.Add(this.btnOpenLocal);
            this.gbActions.Controls.Add(this.btnParse);
            this.gbActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbActions.Location = new System.Drawing.Point(184, 163);
            this.gbActions.Name = "gbActions";
            this.gbActions.Size = new System.Drawing.Size(170, 204);
            this.gbActions.TabIndex = 2;
            this.gbActions.TabStop = false;
            this.gbActions.Text = "Actions";
            // 
            // cmbWebhookSelect
            // 
            this.cmbWebhookSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbWebhookSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWebhookSelect.FormattingEnabled = true;
            this.cmbWebhookSelect.Location = new System.Drawing.Point(6, 144);
            this.cmbWebhookSelect.Name = "cmbWebhookSelect";
            this.cmbWebhookSelect.Size = new System.Drawing.Size(158, 21);
            this.cmbWebhookSelect.TabIndex = 4;
            // 
            // lblLinksCopied
            // 
            this.lblLinksCopied.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLinksCopied.AutoSize = true;
            this.lblLinksCopied.Location = new System.Drawing.Point(100, 119);
            this.lblLinksCopied.Name = "lblLinksCopied";
            this.lblLinksCopied.Size = new System.Drawing.Size(64, 13);
            this.lblLinksCopied.TabIndex = 3;
            this.lblLinksCopied.Text = "### Copied";
            this.lblLinksCopied.Visible = false;
            // 
            // btnParsAndUpload
            // 
            this.btnParsAndUpload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnParsAndUpload.Location = new System.Drawing.Point(6, 77);
            this.btnParsAndUpload.Name = "btnParsAndUpload";
            this.btnParsAndUpload.Size = new System.Drawing.Size(158, 23);
            this.btnParsAndUpload.TabIndex = 2;
            this.btnParsAndUpload.Text = "Parse Local & Upload";
            this.btnParsAndUpload.UseVisualStyleBackColor = true;
            this.btnParsAndUpload.Click += new System.EventHandler(this.BtnParsAndUpload_Click);
            // 
            // btnOpenDpsReport
            // 
            this.btnOpenDpsReport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenDpsReport.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.dBLogBindingSource, "HasLink", true));
            this.btnOpenDpsReport.Location = new System.Drawing.Point(87, 48);
            this.btnOpenDpsReport.Name = "btnOpenDpsReport";
            this.btnOpenDpsReport.Size = new System.Drawing.Size(77, 23);
            this.btnOpenDpsReport.TabIndex = 2;
            this.btnOpenDpsReport.Text = "dps.report";
            this.btnOpenDpsReport.UseVisualStyleBackColor = true;
            this.btnOpenDpsReport.Click += new System.EventHandler(this.BtnOpenDpsReport_Click);
            // 
            // dBLogBindingSource
            // 
            this.dBLogBindingSource.DataSource = typeof(LogUploader.Tools.Database.DBLog);
            // 
            // btnPostToDiscord
            // 
            this.btnPostToDiscord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPostToDiscord.Location = new System.Drawing.Point(53, 171);
            this.btnPostToDiscord.Name = "btnPostToDiscord";
            this.btnPostToDiscord.Size = new System.Drawing.Size(111, 23);
            this.btnPostToDiscord.TabIndex = 2;
            this.btnPostToDiscord.Text = "Post to Discord";
            this.btnPostToDiscord.UseVisualStyleBackColor = true;
            this.btnPostToDiscord.Click += new System.EventHandler(this.BtnPostToDiscord_Click);
            // 
            // btnCopyLinks
            // 
            this.btnCopyLinks.Location = new System.Drawing.Point(6, 115);
            this.btnCopyLinks.Name = "btnCopyLinks";
            this.btnCopyLinks.Size = new System.Drawing.Size(88, 23);
            this.btnCopyLinks.TabIndex = 2;
            this.btnCopyLinks.Text = "Copy links";
            this.btnCopyLinks.UseVisualStyleBackColor = true;
            this.btnCopyLinks.Click += new System.EventHandler(this.BtnCopyLinks_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.dBLogBindingSource, "HasEvtc", true));
            this.btnUpload.Location = new System.Drawing.Point(6, 48);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(75, 23);
            this.btnUpload.TabIndex = 2;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.BtnUpload_Click);
            // 
            // btnOpenLocal
            // 
            this.btnOpenLocal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenLocal.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.dBLogBindingSource, "HasHtml", true));
            this.btnOpenLocal.Location = new System.Drawing.Point(87, 19);
            this.btnOpenLocal.Name = "btnOpenLocal";
            this.btnOpenLocal.Size = new System.Drawing.Size(78, 23);
            this.btnOpenLocal.TabIndex = 1;
            this.btnOpenLocal.Text = "Open Local";
            this.btnOpenLocal.UseVisualStyleBackColor = true;
            this.btnOpenLocal.Click += new System.EventHandler(this.BtnOpenLocal_Click);
            // 
            // btnParse
            // 
            this.btnParse.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.dBLogBindingSource, "HasEvtc", true));
            this.btnParse.Location = new System.Drawing.Point(6, 19);
            this.btnParse.Name = "btnParse";
            this.btnParse.Size = new System.Drawing.Size(75, 23);
            this.btnParse.TabIndex = 0;
            this.btnParse.Text = "Parse Local";
            this.btnParse.UseVisualStyleBackColor = true;
            this.btnParse.Click += new System.EventHandler(this.BtnParse_Click);
            // 
            // gbSettings
            // 
            this.gbSettings.Controls.Add(this.btnAbout);
            this.gbSettings.Controls.Add(this.btnSettings);
            this.gbSettings.Controls.Add(this.cbAutoUpload);
            this.gbSettings.Controls.Add(this.cbAutoParse);
            this.gbSettings.Location = new System.Drawing.Point(184, 533);
            this.gbSettings.Name = "gbSettings";
            this.gbSettings.Size = new System.Drawing.Size(170, 94);
            this.gbSettings.TabIndex = 3;
            this.gbSettings.TabStop = false;
            this.gbSettings.Text = "Settings";
            // 
            // btnAbout
            // 
            this.btnAbout.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbout.Location = new System.Drawing.Point(6, 67);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(52, 21);
            this.btnAbout.TabIndex = 3;
            this.btnAbout.Text = "About";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.BtnAbout_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSettings.Location = new System.Drawing.Point(77, 65);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(87, 23);
            this.btnSettings.TabIndex = 2;
            this.btnSettings.Text = "Settings...";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.BtnSettings_Click);
            // 
            // cbAutoUpload
            // 
            this.cbAutoUpload.AutoSize = true;
            this.cbAutoUpload.Location = new System.Drawing.Point(6, 42);
            this.cbAutoUpload.Name = "cbAutoUpload";
            this.cbAutoUpload.Size = new System.Drawing.Size(134, 17);
            this.cbAutoUpload.TabIndex = 1;
            this.cbAutoUpload.Text = "Auto Upload new Logs";
            this.cbAutoUpload.UseVisualStyleBackColor = true;
            this.cbAutoUpload.CheckedChanged += new System.EventHandler(this.Update_NewLogsAutoFeatures);
            // 
            // cbAutoParse
            // 
            this.cbAutoParse.AutoSize = true;
            this.cbAutoParse.Location = new System.Drawing.Point(6, 19);
            this.cbAutoParse.Name = "cbAutoParse";
            this.cbAutoParse.Size = new System.Drawing.Size(127, 17);
            this.cbAutoParse.TabIndex = 0;
            this.cbAutoParse.Text = "Auto Parse new Logs";
            this.cbAutoParse.UseVisualStyleBackColor = true;
            this.cbAutoParse.CheckedChanged += new System.EventHandler(this.Update_NewLogsAutoFeatures);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(184, 453);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(102, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Reload Lang XML";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // gbRaidOrga
            // 
            this.gbRaidOrga.Controls.Add(this.btnRaidOrgaReload);
            this.gbRaidOrga.Controls.Add(this.btnUpdateRaidOrga);
            this.gbRaidOrga.Controls.Add(this.cmbRaidOrgaTermin);
            this.gbRaidOrga.Location = new System.Drawing.Point(184, 373);
            this.gbRaidOrga.Name = "gbRaidOrga";
            this.gbRaidOrga.Size = new System.Drawing.Size(170, 74);
            this.gbRaidOrga.TabIndex = 5;
            this.gbRaidOrga.TabStop = false;
            this.gbRaidOrga.Text = "RaidOrga+";
            // 
            // btnRaidOrgaReload
            // 
            this.btnRaidOrgaReload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRaidOrgaReload.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRaidOrgaReload.BackgroundImage")));
            this.btnRaidOrgaReload.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRaidOrgaReload.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRaidOrgaReload.Location = new System.Drawing.Point(6, 46);
            this.btnRaidOrgaReload.Name = "btnRaidOrgaReload";
            this.btnRaidOrgaReload.Size = new System.Drawing.Size(22, 22);
            this.btnRaidOrgaReload.TabIndex = 2;
            this.btnRaidOrgaReload.Click += new System.EventHandler(this.btnRaidOrgaReload_Click);
            // 
            // btnUpdateRaidOrga
            // 
            this.btnUpdateRaidOrga.Location = new System.Drawing.Point(53, 45);
            this.btnUpdateRaidOrga.Name = "btnUpdateRaidOrga";
            this.btnUpdateRaidOrga.Size = new System.Drawing.Size(111, 23);
            this.btnUpdateRaidOrga.TabIndex = 1;
            this.btnUpdateRaidOrga.Text = "Update Termin";
            this.btnUpdateRaidOrga.UseVisualStyleBackColor = true;
            this.btnUpdateRaidOrga.Click += new System.EventHandler(this.BtnUpdateRaidOrga_Click);
            // 
            // cmbRaidOrgaTermin
            // 
            this.cmbRaidOrgaTermin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRaidOrgaTermin.FormattingEnabled = true;
            this.cmbRaidOrgaTermin.Location = new System.Drawing.Point(6, 19);
            this.cmbRaidOrgaTermin.Name = "cmbRaidOrgaTermin";
            this.cmbRaidOrgaTermin.Size = new System.Drawing.Size(158, 21);
            this.cmbRaidOrgaTermin.TabIndex = 0;
            // 
            // pGrid
            // 
            this.pGrid.Controls.Add(this.dBLogDataGridView);
            this.pGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pGrid.Location = new System.Drawing.Point(0, 1);
            this.pGrid.Name = "pGrid";
            this.pGrid.Size = new System.Drawing.Size(670, 630);
            this.pGrid.TabIndex = 3;
            // 
            // dBLogDataGridView
            // 
            this.dBLogDataGridView.AllowUserToAddRows = false;
            this.dBLogDataGridView.AllowUserToDeleteRows = false;
            this.dBLogDataGridView.AllowUserToResizeRows = false;
            this.dBLogDataGridView.AutoGenerateColumns = false;
            this.dBLogDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dBLogDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colBossName,
            this.colDate,
            this.colSize,
            this.colDataCorrected,
            this.colDuration,
            this.colSuccsess,
            this.colHpLeft,
            this.colIsCM,
            this.colHasHtml,
            this.colHasLink,
            this.colID,
            this.colBossID,
            this.colEvtcPath,
            this.colJsonPath,
            this.colHtmlPath,
            this.colLink,
            this.colTimeStamp,
            this.colDurationMs,
            this.colFlags,
            this.colHasEvtc,
            this.colHasJson});
            this.dBLogDataGridView.ContextMenuStrip = this.contextMenuGrid;
            this.dBLogDataGridView.DataSource = this.dBLogBindingSource;
            this.dBLogDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dBLogDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dBLogDataGridView.Location = new System.Drawing.Point(0, 0);
            this.dBLogDataGridView.Name = "dBLogDataGridView";
            this.dBLogDataGridView.ReadOnly = true;
            this.dBLogDataGridView.RowHeadersVisible = false;
            this.dBLogDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dBLogDataGridView.Size = new System.Drawing.Size(670, 630);
            this.dBLogDataGridView.TabIndex = 0;
            this.dBLogDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DBLogDataGridView_CellDoubleClick);
            this.dBLogDataGridView.SelectionChanged += new System.EventHandler(this.DBLogDataGridView_SelectionChanged);
            // 
            // colBossName
            // 
            this.colBossName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colBossName.DataPropertyName = "BossName";
            this.colBossName.HeaderText = "Boss";
            this.colBossName.MinimumWidth = 100;
            this.colBossName.Name = "colBossName";
            this.colBossName.ReadOnly = true;
            // 
            // colDate
            // 
            this.colDate.DataPropertyName = "Date";
            this.colDate.FillWeight = 10F;
            this.colDate.HeaderText = "Date";
            this.colDate.MinimumWidth = 100;
            this.colDate.Name = "colDate";
            this.colDate.ReadOnly = true;
            // 
            // colSize
            // 
            this.colSize.DataPropertyName = "SizeKb";
            this.colSize.FillWeight = 10F;
            this.colSize.HeaderText = "SizeKb";
            this.colSize.MinimumWidth = 50;
            this.colSize.Name = "colSize";
            this.colSize.ReadOnly = true;
            this.colSize.Width = 50;
            // 
            // colDataCorrected
            // 
            this.colDataCorrected.DataPropertyName = "DataCorrected";
            this.colDataCorrected.FillWeight = 1F;
            this.colDataCorrected.HeaderText = "DataCorrected";
            this.colDataCorrected.MinimumWidth = 80;
            this.colDataCorrected.Name = "colDataCorrected";
            this.colDataCorrected.ReadOnly = true;
            this.colDataCorrected.Width = 80;
            // 
            // colDuration
            // 
            this.colDuration.DataPropertyName = "Duration";
            this.colDuration.FillWeight = 10F;
            this.colDuration.HeaderText = "Duration";
            this.colDuration.MinimumWidth = 60;
            this.colDuration.Name = "colDuration";
            this.colDuration.ReadOnly = true;
            this.colDuration.Width = 60;
            // 
            // colSuccsess
            // 
            this.colSuccsess.DataPropertyName = "Succsess";
            this.colSuccsess.FillWeight = 1F;
            this.colSuccsess.HeaderText = "Succsess";
            this.colSuccsess.MinimumWidth = 60;
            this.colSuccsess.Name = "colSuccsess";
            this.colSuccsess.ReadOnly = true;
            this.colSuccsess.Width = 60;
            // 
            // colHpLeft
            // 
            this.colHpLeft.DataPropertyName = "RemainingHealth";
            dataGridViewCellStyle1.Format = "F2";
            this.colHpLeft.DefaultCellStyle = dataGridViewCellStyle1;
            this.colHpLeft.FillWeight = 10F;
            this.colHpLeft.HeaderText = "% left";
            this.colHpLeft.MinimumWidth = 64;
            this.colHpLeft.Name = "colHpLeft";
            this.colHpLeft.ReadOnly = true;
            this.colHpLeft.Width = 64;
            // 
            // colIsCM
            // 
            this.colIsCM.DataPropertyName = "IsCM";
            this.colIsCM.FillWeight = 1F;
            this.colIsCM.HeaderText = "CM";
            this.colIsCM.MinimumWidth = 30;
            this.colIsCM.Name = "colIsCM";
            this.colIsCM.ReadOnly = true;
            this.colIsCM.Width = 30;
            // 
            // colHasHtml
            // 
            this.colHasHtml.DataPropertyName = "HasHtml";
            this.colHasHtml.FillWeight = 1F;
            this.colHasHtml.HeaderText = "Parsed";
            this.colHasHtml.MinimumWidth = 50;
            this.colHasHtml.Name = "colHasHtml";
            this.colHasHtml.ReadOnly = true;
            this.colHasHtml.Width = 50;
            // 
            // colHasLink
            // 
            this.colHasLink.DataPropertyName = "HasLink";
            this.colHasLink.HeaderText = "Uploaded";
            this.colHasLink.MinimumWidth = 60;
            this.colHasLink.Name = "colHasLink";
            this.colHasLink.ReadOnly = true;
            this.colHasLink.Width = 60;
            // 
            // colID
            // 
            this.colID.DataPropertyName = "ID";
            this.colID.HeaderText = "ID";
            this.colID.Name = "colID";
            this.colID.ReadOnly = true;
            this.colID.Visible = false;
            // 
            // colBossID
            // 
            this.colBossID.DataPropertyName = "BossID";
            this.colBossID.HeaderText = "BossID";
            this.colBossID.Name = "colBossID";
            this.colBossID.ReadOnly = true;
            this.colBossID.Visible = false;
            // 
            // colEvtcPath
            // 
            this.colEvtcPath.DataPropertyName = "EvtcPath";
            this.colEvtcPath.HeaderText = "EvtcPath";
            this.colEvtcPath.Name = "colEvtcPath";
            this.colEvtcPath.ReadOnly = true;
            this.colEvtcPath.Visible = false;
            // 
            // colJsonPath
            // 
            this.colJsonPath.DataPropertyName = "JsonPath";
            this.colJsonPath.HeaderText = "JsonPath";
            this.colJsonPath.Name = "colJsonPath";
            this.colJsonPath.ReadOnly = true;
            this.colJsonPath.Visible = false;
            // 
            // colHtmlPath
            // 
            this.colHtmlPath.DataPropertyName = "HtmlPath";
            this.colHtmlPath.HeaderText = "HtmlPath";
            this.colHtmlPath.Name = "colHtmlPath";
            this.colHtmlPath.ReadOnly = true;
            this.colHtmlPath.Visible = false;
            // 
            // colLink
            // 
            this.colLink.DataPropertyName = "Link";
            this.colLink.HeaderText = "Link";
            this.colLink.Name = "colLink";
            this.colLink.ReadOnly = true;
            this.colLink.Visible = false;
            // 
            // colTimeStamp
            // 
            this.colTimeStamp.DataPropertyName = "TimeStamp";
            this.colTimeStamp.HeaderText = "TimeStamp";
            this.colTimeStamp.Name = "colTimeStamp";
            this.colTimeStamp.ReadOnly = true;
            this.colTimeStamp.Visible = false;
            // 
            // colDurationMs
            // 
            this.colDurationMs.DataPropertyName = "DurationMs";
            this.colDurationMs.HeaderText = "DurationMs";
            this.colDurationMs.Name = "colDurationMs";
            this.colDurationMs.ReadOnly = true;
            this.colDurationMs.Visible = false;
            // 
            // colFlags
            // 
            this.colFlags.DataPropertyName = "Flags";
            this.colFlags.HeaderText = "Flags";
            this.colFlags.Name = "colFlags";
            this.colFlags.ReadOnly = true;
            this.colFlags.Visible = false;
            // 
            // colHasEvtc
            // 
            this.colHasEvtc.DataPropertyName = "HasEvtc";
            this.colHasEvtc.HeaderText = "HasEvtc";
            this.colHasEvtc.Name = "colHasEvtc";
            this.colHasEvtc.ReadOnly = true;
            this.colHasEvtc.Visible = false;
            // 
            // colHasJson
            // 
            this.colHasJson.DataPropertyName = "HasJson";
            this.colHasJson.HeaderText = "HasJson";
            this.colHasJson.Name = "colHasJson";
            this.colHasJson.ReadOnly = true;
            this.colHasJson.Visible = false;
            // 
            // contextMenuGrid
            // 
            this.contextMenuGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miParse,
            this.miOpenLocal,
            this.miUpload,
            this.miOpenLink,
            this.miParseUpload,
            this.miViewInExplorer});
            this.contextMenuGrid.Name = "contextMenuGrid";
            this.contextMenuGrid.Size = new System.Drawing.Size(167, 136);
            this.contextMenuGrid.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuGrid_Opening);
            // 
            // miParse
            // 
            this.miParse.Name = "miParse";
            this.miParse.Size = new System.Drawing.Size(166, 22);
            this.miParse.Text = "Parse";
            this.miParse.Click += new System.EventHandler(this.BtnParse_Click);
            // 
            // miOpenLocal
            // 
            this.miOpenLocal.Name = "miOpenLocal";
            this.miOpenLocal.Size = new System.Drawing.Size(166, 22);
            this.miOpenLocal.Text = "Open Local";
            this.miOpenLocal.Click += new System.EventHandler(this.BtnOpenLocal_Click);
            // 
            // miUpload
            // 
            this.miUpload.Name = "miUpload";
            this.miUpload.Size = new System.Drawing.Size(166, 22);
            this.miUpload.Text = "Upload";
            this.miUpload.Click += new System.EventHandler(this.BtnUpload_Click);
            // 
            // miOpenLink
            // 
            this.miOpenLink.Name = "miOpenLink";
            this.miOpenLink.Size = new System.Drawing.Size(166, 22);
            this.miOpenLink.Text = "Open dps.report";
            this.miOpenLink.Click += new System.EventHandler(this.BtnOpenDpsReport_Click);
            // 
            // miParseUpload
            // 
            this.miParseUpload.Name = "miParseUpload";
            this.miParseUpload.Size = new System.Drawing.Size(166, 22);
            this.miParseUpload.Text = "Parse and Upload";
            this.miParseUpload.Click += new System.EventHandler(this.BtnParsAndUpload_Click);
            // 
            // miViewInExplorer
            // 
            this.miViewInExplorer.Name = "miViewInExplorer";
            this.miViewInExplorer.Size = new System.Drawing.Size(166, 22);
            this.miViewInExplorer.Text = "View in Explorer";
            this.miViewInExplorer.Click += new System.EventHandler(this.ViewToolStripMenuItem_Click);
            // 
            // pTop
            // 
            this.pTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pTop.Location = new System.Drawing.Point(0, 0);
            this.pTop.Name = "pTop";
            this.pTop.Size = new System.Drawing.Size(1027, 1);
            this.pTop.TabIndex = 0;
            // 
            // LogUploaderUI2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1027, 651);
            this.Controls.Add(this.pGrid);
            this.Controls.Add(this.pControl);
            this.Controls.Add(this.pStatus);
            this.Controls.Add(this.pTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(670, 570);
            this.Name = "LogUploaderUI2";
            this.Text = "LogUploader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LogUploaderUI2_FormClosing);
            this.Shown += new System.EventHandler(this.LogUploaderUI2_Shown);
            this.pStatus.ResumeLayout(false);
            this.pWorkStatus.ResumeLayout(false);
            this.flpProgress.ResumeLayout(false);
            this.flpProgress.PerformLayout();
            this.pElementCounts.ResumeLayout(false);
            this.flpSelectionStats.ResumeLayout(false);
            this.flpSelectionStats.PerformLayout();
            this.pControl.ResumeLayout(false);
            this.tlControlsTable.ResumeLayout(false);
            this.gbFilter.ResumeLayout(false);
            this.gbFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFilterHpLeft)).EndInit();
            this.gbDetails.ResumeLayout(false);
            this.tpDetailsTop.ResumeLayout(false);
            this.tpDetailsTop.PerformLayout();
            this.pDetailsPlayers.ResumeLayout(false);
            this.gbActions.ResumeLayout(false);
            this.gbActions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dBLogBindingSource)).EndInit();
            this.gbSettings.ResumeLayout(false);
            this.gbSettings.PerformLayout();
            this.gbRaidOrga.ResumeLayout(false);
            this.pGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dBLogDataGridView)).EndInit();
            this.contextMenuGrid.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pStatus;
        private System.Windows.Forms.Panel pControl;
        private System.Windows.Forms.TableLayoutPanel tlControlsTable;
        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.Button btnFilterToday;
        private System.Windows.Forms.Label lblFilterTo;
        private System.Windows.Forms.Label lblFilterFrom;
        private System.Windows.Forms.Label lblFilterDate;
        private System.Windows.Forms.DateTimePicker dtpFilterTo;
        private System.Windows.Forms.DateTimePicker dtpFilterFrom;
        private System.Windows.Forms.RadioButton rbFilterWipe;
        private System.Windows.Forms.RadioButton rbFilterKill;
        private System.Windows.Forms.TextBox txtFilterDuration;
        private System.Windows.Forms.NumericUpDown numFilterHpLeft;
        private System.Windows.Forms.CheckBox cbFilterSuccsess;
        private System.Windows.Forms.CheckBox cbFilterDuration;
        private System.Windows.Forms.CheckBox cbFilterHpLeft;
        private System.Windows.Forms.CheckBox cbFilterBoss;
        private System.Windows.Forms.ComboBox cmbFilterHp;
        private System.Windows.Forms.ComboBox cmbFilterDuration;
        private System.Windows.Forms.ComboBox cmbFilterBoss;
        private System.Windows.Forms.GroupBox gbDetails;
        private System.Windows.Forms.GroupBox gbActions;
        private System.Windows.Forms.Button btnParsAndUpload;
        private System.Windows.Forms.Button btnOpenDpsReport;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Button btnOpenLocal;
        private System.Windows.Forms.Button btnParse;
        private System.Windows.Forms.GroupBox gbSettings;
        private System.Windows.Forms.Panel pGrid;
        private System.Windows.Forms.Panel pTop;
        private System.Windows.Forms.ComboBox cmbWebhookSelect;
        private System.Windows.Forms.Label lblLinksCopied;
        private System.Windows.Forms.Button btnPostToDiscord;
        private System.Windows.Forms.Button btnCopyLinks;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.CheckBox cbAutoUpload;
        private System.Windows.Forms.CheckBox cbAutoParse;
        private System.Windows.Forms.DataGridView dBLogDataGridView;
        private System.Windows.Forms.BindingSource dBLogBindingSource;
        private System.Windows.Forms.Panel pWorkStatus;
        private System.Windows.Forms.FlowLayoutPanel flpProgress;
        private System.Windows.Forms.Panel pgTop;
        private System.Windows.Forms.Panel pgBottom;
        private System.Windows.Forms.Label lblWorkCount;
        private System.Windows.Forms.Label lblWorkType;
        private System.Windows.Forms.Panel pElementCounts;
        private System.Windows.Forms.FlowLayoutPanel flpSelectionStats;
        private System.Windows.Forms.Label lblTotalCount;
        private System.Windows.Forms.Label lblElements;
        private System.Windows.Forms.Label lblSelectedCount;
        private System.Windows.Forms.Label lblSelected;
        private System.Windows.Forms.Label lblShowenCount;
        private System.Windows.Forms.Label lblShown;
        private System.Windows.Forms.ContextMenuStrip contextMenuGrid;
        private System.Windows.Forms.ToolStripMenuItem miParse;
        private System.Windows.Forms.ToolStripMenuItem miOpenLocal;
        private System.Windows.Forms.ToolStripMenuItem miUpload;
        private System.Windows.Forms.ToolStripMenuItem miOpenLink;
        private System.Windows.Forms.ToolStripMenuItem miParseUpload;
        private System.Windows.Forms.Button btnFilterReset;
        private System.Windows.Forms.Label lblFilterPercent;
        private System.Windows.Forms.Panel pDetailsPlayers;
        private System.Windows.Forms.FlowLayoutPanel flpPlayers;
        private System.Windows.Forms.TableLayoutPanel tpDetailsTop;
        private System.Windows.Forms.Label lblDetOpenRemot;
        private System.Windows.Forms.CheckBox cbDetUploaded;
        private System.Windows.Forms.CheckBox cbDetParsed;
        private System.Windows.Forms.CheckBox cbDetCM;
        private System.Windows.Forms.Label lblDetHp;
        private System.Windows.Forms.Label lblDetHpDes;
        private System.Windows.Forms.CheckBox cbDetSuccess;
        private System.Windows.Forms.Label lblDetDuration;
        private System.Windows.Forms.Label lblDetDurationDes;
        private System.Windows.Forms.Label lblDetSize;
        private System.Windows.Forms.Label lblDetSizeDes;
        private System.Windows.Forms.Label lblDetDateDes;
        private System.Windows.Forms.Label lblDetBoss;
        private System.Windows.Forms.Label lblDetBossDes;
        private System.Windows.Forms.CheckBox cbDetCorrected;
        private System.Windows.Forms.Label lblDetOpenLocal;
        private System.Windows.Forms.Label lblDetSuccess;
        private System.Windows.Forms.Label lblDetCorrected;
        private System.Windows.Forms.Label lblDetDate;
        private System.Windows.Forms.Label lblDetCM;
        private System.Windows.Forms.Label lblDetUpload;
        private System.Windows.Forms.Label lblDetParsed;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolStripMenuItem miViewInExplorer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBossName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSize;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colDataCorrected;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDuration;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSuccsess;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHpLeft;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colIsCM;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colHasHtml;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colHasLink;
        private System.Windows.Forms.DataGridViewTextBoxColumn colID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBossID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEvtcPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJsonPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHtmlPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLink;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTimeStamp;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDurationMs;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFlags;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colHasEvtc;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colHasJson;
        private System.Windows.Forms.GroupBox gbRaidOrga;
        private System.Windows.Forms.Button btnUpdateRaidOrga;
        private System.Windows.Forms.ComboBox cmbRaidOrgaTermin;
        private System.Windows.Forms.Panel btnRaidOrgaReload;
    }
}