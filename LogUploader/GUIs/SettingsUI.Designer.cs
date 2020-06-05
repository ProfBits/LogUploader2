namespace LogUploader.GUI
{
    partial class SettingsUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsUI));
            this.gbCopyLinks = new System.Windows.Forms.GroupBox();
            this.cbLinkInSameLine = new System.Windows.Forms.CheckBox();
            this.SettingsbindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cbGNEmotes = new System.Windows.Forms.CheckBox();
            this.cbEmptyLinesInBetween = new System.Windows.Forms.CheckBox();
            this.cbShowSuccsess = new System.Windows.Forms.CheckBox();
            this.cbShwoEncounterName = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnDefault = new System.Windows.Forms.Button();
            this.gbDpsReport = new System.Windows.Forms.GroupBox();
            this.btnProxySettings = new System.Windows.Forms.Button();
            this.linklblGetUserToken = new System.Windows.Forms.LinkLabel();
            this.lblGetUserToken = new System.Windows.Forms.Label();
            this.txtUserToken = new System.Windows.Forms.TextBox();
            this.lblUserToken = new System.Windows.Forms.Label();
            this.gbGeneral = new System.Windows.Forms.GroupBox();
            this.cmbLang = new System.Windows.Forms.ComboBox();
            this.lblLang = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtArcLogPath = new System.Windows.Forms.TextBox();
            this.lblArcLogsPath = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.gbDiscord = new System.Windows.Forms.GroupBox();
            this.cbNameAsUser = new System.Windows.Forms.CheckBox();
            this.cbEiOnlyUploaded = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblWebHookCountDes = new System.Windows.Forms.Label();
            this.lblWebHookCount = new System.Windows.Forms.Label();
            this.btnAddWebHook = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.flpWebHooks = new System.Windows.Forms.FlowLayoutPanel();
            this.webHookConfig1 = new LogUploader.GUIs.WebHookConfig();
            this.ttHelp = new System.Windows.Forms.ToolTip(this.components);
            this.gbEi = new System.Windows.Forms.GroupBox();
            this.cbEiTheme = new System.Windows.Forms.CheckBox();
            this.cbEiCombatReplay = new System.Windows.Forms.CheckBox();
            this.gbCopyLinks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SettingsbindingSource)).BeginInit();
            this.gbDpsReport.SuspendLayout();
            this.gbGeneral.SuspendLayout();
            this.gbDiscord.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.flpWebHooks.SuspendLayout();
            this.gbEi.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbCopyLinks
            // 
            this.gbCopyLinks.Controls.Add(this.cbLinkInSameLine);
            this.gbCopyLinks.Controls.Add(this.cbGNEmotes);
            this.gbCopyLinks.Controls.Add(this.cbEmptyLinesInBetween);
            this.gbCopyLinks.Controls.Add(this.cbShowSuccsess);
            this.gbCopyLinks.Controls.Add(this.cbShwoEncounterName);
            this.gbCopyLinks.Location = new System.Drawing.Point(12, 226);
            this.gbCopyLinks.Name = "gbCopyLinks";
            this.gbCopyLinks.Size = new System.Drawing.Size(315, 134);
            this.gbCopyLinks.TabIndex = 0;
            this.gbCopyLinks.TabStop = false;
            this.gbCopyLinks.Text = "Copylinks";
            // 
            // cbLinkInSameLine
            // 
            this.cbLinkInSameLine.AutoSize = true;
            this.cbLinkInSameLine.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.SettingsbindingSource, "Inline", true));
            this.cbLinkInSameLine.Location = new System.Drawing.Point(6, 65);
            this.cbLinkInSameLine.Name = "cbLinkInSameLine";
            this.cbLinkInSameLine.Size = new System.Drawing.Size(169, 17);
            this.cbLinkInSameLine.TabIndex = 5;
            this.cbLinkInSameLine.Text = "Link in same line as encounter";
            this.cbLinkInSameLine.UseVisualStyleBackColor = true;
            // 
            // SettingsbindingSource
            // 
            this.SettingsbindingSource.AllowNew = true;
            this.SettingsbindingSource.DataSource = typeof(LogUploader.Data.Settings.SettingsData);
            // 
            // cbGNEmotes
            // 
            this.cbGNEmotes.AutoSize = true;
            this.cbGNEmotes.Cursor = System.Windows.Forms.Cursors.Help;
            this.cbGNEmotes.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.SettingsbindingSource, "UseGnDiscordEmotes", true));
            this.cbGNEmotes.Location = new System.Drawing.Point(6, 111);
            this.cbGNEmotes.Name = "cbGNEmotes";
            this.cbGNEmotes.Size = new System.Drawing.Size(138, 17);
            this.cbGNEmotes.TabIndex = 6;
            this.cbGNEmotes.Text = "Include Discord Emotes";
            this.ttHelp.SetToolTip(this.cbGNEmotes, "Discord Emotes can be customized in\r\n<Installation Folder>\\Data\\DataConfig.json");
            this.cbGNEmotes.UseVisualStyleBackColor = true;
            // 
            // cbEmptyLinesInBetween
            // 
            this.cbEmptyLinesInBetween.AutoSize = true;
            this.cbEmptyLinesInBetween.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.SettingsbindingSource, "EmptyLineBetween", true));
            this.cbEmptyLinesInBetween.Location = new System.Drawing.Point(6, 88);
            this.cbEmptyLinesInBetween.Name = "cbEmptyLinesInBetween";
            this.cbEmptyLinesInBetween.Size = new System.Drawing.Size(148, 17);
            this.cbEmptyLinesInBetween.TabIndex = 6;
            this.cbEmptyLinesInBetween.Text = "Empty Line between Logs";
            this.cbEmptyLinesInBetween.UseVisualStyleBackColor = true;
            // 
            // cbShowSuccsess
            // 
            this.cbShowSuccsess.AutoSize = true;
            this.cbShowSuccsess.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.SettingsbindingSource, "EncounterSuccess", true));
            this.cbShowSuccsess.Location = new System.Drawing.Point(6, 42);
            this.cbShowSuccsess.Name = "cbShowSuccsess";
            this.cbShowSuccsess.Size = new System.Drawing.Size(110, 17);
            this.cbShowSuccsess.TabIndex = 4;
            this.cbShowSuccsess.Text = "Include Succsess";
            this.cbShowSuccsess.UseVisualStyleBackColor = true;
            // 
            // cbShwoEncounterName
            // 
            this.cbShwoEncounterName.AutoSize = true;
            this.cbShwoEncounterName.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.SettingsbindingSource, "EncounterName", true));
            this.cbShwoEncounterName.Location = new System.Drawing.Point(6, 19);
            this.cbShwoEncounterName.Name = "cbShwoEncounterName";
            this.cbShwoEncounterName.Size = new System.Drawing.Size(112, 17);
            this.cbShwoEncounterName.TabIndex = 3;
            this.cbShwoEncounterName.Text = "Include encounter";
            this.cbShwoEncounterName.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(485, 377);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(566, 377);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "Save";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnDefault
            // 
            this.btnDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDefault.Location = new System.Drawing.Point(12, 377);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(75, 23);
            this.btnDefault.TabIndex = 9;
            this.btnDefault.Text = "Default";
            this.btnDefault.UseVisualStyleBackColor = true;
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // gbDpsReport
            // 
            this.gbDpsReport.Controls.Add(this.btnProxySettings);
            this.gbDpsReport.Controls.Add(this.linklblGetUserToken);
            this.gbDpsReport.Controls.Add(this.lblGetUserToken);
            this.gbDpsReport.Controls.Add(this.txtUserToken);
            this.gbDpsReport.Controls.Add(this.lblUserToken);
            this.gbDpsReport.Location = new System.Drawing.Point(12, 118);
            this.gbDpsReport.Name = "gbDpsReport";
            this.gbDpsReport.Size = new System.Drawing.Size(314, 102);
            this.gbDpsReport.TabIndex = 4;
            this.gbDpsReport.TabStop = false;
            this.gbDpsReport.Text = "dps.report";
            // 
            // btnProxySettings
            // 
            this.btnProxySettings.Enabled = false;
            this.btnProxySettings.Location = new System.Drawing.Point(6, 69);
            this.btnProxySettings.Name = "btnProxySettings";
            this.btnProxySettings.Size = new System.Drawing.Size(140, 23);
            this.btnProxySettings.TabIndex = 7;
            this.btnProxySettings.Text = "Proxy Settings...";
            this.btnProxySettings.UseVisualStyleBackColor = true;
            // 
            // linklblGetUserToken
            // 
            this.linklblGetUserToken.ActiveLinkColor = System.Drawing.Color.MidnightBlue;
            this.linklblGetUserToken.AutoSize = true;
            this.linklblGetUserToken.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linklblGetUserToken.Location = new System.Drawing.Point(96, 47);
            this.linklblGetUserToken.Name = "linklblGetUserToken";
            this.linklblGetUserToken.Size = new System.Drawing.Size(163, 13);
            this.linklblGetUserToken.TabIndex = 3;
            this.linklblGetUserToken.TabStop = true;
            this.linklblGetUserToken.Text = "https://dps.report/getUserToken";
            this.linklblGetUserToken.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linklblGetUserToken_LinkClicked);
            // 
            // lblGetUserToken
            // 
            this.lblGetUserToken.AutoSize = true;
            this.lblGetUserToken.Location = new System.Drawing.Point(6, 47);
            this.lblGetUserToken.Name = "lblGetUserToken";
            this.lblGetUserToken.Size = new System.Drawing.Size(84, 13);
            this.lblGetUserToken.TabIndex = 2;
            this.lblGetUserToken.Text = "Get your Token:";
            // 
            // txtUserToken
            // 
            this.txtUserToken.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.SettingsbindingSource, "UserToken", true));
            this.txtUserToken.Location = new System.Drawing.Point(78, 19);
            this.txtUserToken.Name = "txtUserToken";
            this.txtUserToken.Size = new System.Drawing.Size(206, 20);
            this.txtUserToken.TabIndex = 1;
            // 
            // lblUserToken
            // 
            this.lblUserToken.AutoSize = true;
            this.lblUserToken.Location = new System.Drawing.Point(6, 22);
            this.lblUserToken.Name = "lblUserToken";
            this.lblUserToken.Size = new System.Drawing.Size(66, 13);
            this.lblUserToken.TabIndex = 0;
            this.lblUserToken.Text = "User Token:";
            // 
            // gbGeneral
            // 
            this.gbGeneral.Controls.Add(this.cmbLang);
            this.gbGeneral.Controls.Add(this.lblLang);
            this.gbGeneral.Controls.Add(this.btnBrowse);
            this.gbGeneral.Controls.Add(this.txtArcLogPath);
            this.gbGeneral.Controls.Add(this.lblArcLogsPath);
            this.gbGeneral.Location = new System.Drawing.Point(12, 12);
            this.gbGeneral.Name = "gbGeneral";
            this.gbGeneral.Size = new System.Drawing.Size(315, 100);
            this.gbGeneral.TabIndex = 11;
            this.gbGeneral.TabStop = false;
            this.gbGeneral.Text = "General";
            // 
            // cmbLang
            // 
            this.cmbLang.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.SettingsbindingSource, "Language", true));
            this.cmbLang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLang.FormattingEnabled = true;
            this.cmbLang.Location = new System.Drawing.Point(67, 72);
            this.cmbLang.Name = "cmbLang";
            this.cmbLang.Size = new System.Drawing.Size(121, 21);
            this.cmbLang.TabIndex = 4;
            // 
            // lblLang
            // 
            this.lblLang.AutoSize = true;
            this.lblLang.Location = new System.Drawing.Point(6, 75);
            this.lblLang.Margin = new System.Windows.Forms.Padding(3);
            this.lblLang.Name = "lblLang";
            this.lblLang.Size = new System.Drawing.Size(55, 13);
            this.lblLang.TabIndex = 3;
            this.lblLang.Text = "Language";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(234, 36);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse ...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            // 
            // txtArcLogPath
            // 
            this.txtArcLogPath.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.SettingsbindingSource, "ArcLogsPath", true));
            this.txtArcLogPath.Location = new System.Drawing.Point(6, 38);
            this.txtArcLogPath.Name = "txtArcLogPath";
            this.txtArcLogPath.Size = new System.Drawing.Size(222, 20);
            this.txtArcLogPath.TabIndex = 1;
            // 
            // lblArcLogsPath
            // 
            this.lblArcLogsPath.AutoSize = true;
            this.lblArcLogsPath.Location = new System.Drawing.Point(6, 22);
            this.lblArcLogsPath.Margin = new System.Windows.Forms.Padding(3);
            this.lblArcLogsPath.Name = "lblArcLogsPath";
            this.lblArcLogsPath.Size = new System.Drawing.Size(77, 13);
            this.lblArcLogsPath.TabIndex = 0;
            this.lblArcLogsPath.Text = "Arc Logs Path:";
            this.ttHelp.SetToolTip(this.lblArcLogsPath, "specifie the \"arcdps.cbtlogs\" folder.\r\nNormally found under <user>\\Documents\\Guil" +
        "d Wars 2\\addons\\arcdps\\arcdps.cbtlogs");
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // gbDiscord
            // 
            this.gbDiscord.Controls.Add(this.cbNameAsUser);
            this.gbDiscord.Controls.Add(this.cbEiOnlyUploaded);
            this.gbDiscord.Controls.Add(this.flowLayoutPanel2);
            this.gbDiscord.Controls.Add(this.btnAddWebHook);
            this.gbDiscord.Controls.Add(this.panel1);
            this.gbDiscord.Location = new System.Drawing.Point(333, 12);
            this.gbDiscord.Name = "gbDiscord";
            this.gbDiscord.Size = new System.Drawing.Size(315, 283);
            this.gbDiscord.TabIndex = 12;
            this.gbDiscord.TabStop = false;
            this.gbDiscord.Text = "Discord Webhooks";
            // 
            // cbNameAsUser
            // 
            this.cbNameAsUser.AutoSize = true;
            this.cbNameAsUser.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.SettingsbindingSource, "NameAsDiscordUser", true));
            this.cbNameAsUser.Location = new System.Drawing.Point(9, 256);
            this.cbNameAsUser.Name = "cbNameAsUser";
            this.cbNameAsUser.Size = new System.Drawing.Size(158, 17);
            this.cbNameAsUser.TabIndex = 5;
            this.cbNameAsUser.Text = "Name as Discord Username";
            this.cbNameAsUser.UseVisualStyleBackColor = true;
            // 
            // cbEiOnlyUploaded
            // 
            this.cbEiOnlyUploaded.AutoSize = true;
            this.cbEiOnlyUploaded.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.SettingsbindingSource, "OnlyPostUploaded", true));
            this.cbEiOnlyUploaded.Location = new System.Drawing.Point(9, 233);
            this.cbEiOnlyUploaded.Name = "cbEiOnlyUploaded";
            this.cbEiOnlyUploaded.Size = new System.Drawing.Size(119, 17);
            this.cbEiOnlyUploaded.TabIndex = 4;
            this.cbEiOnlyUploaded.Text = "Only post Uploaded";
            this.cbEiOnlyUploaded.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.lblWebHookCountDes);
            this.flowLayoutPanel2.Controls.Add(this.lblWebHookCount);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(9, 204);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(218, 23);
            this.flowLayoutPanel2.TabIndex = 3;
            // 
            // lblWebHookCountDes
            // 
            this.lblWebHookCountDes.AutoSize = true;
            this.lblWebHookCountDes.Location = new System.Drawing.Point(3, 3);
            this.lblWebHookCountDes.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.lblWebHookCountDes.Name = "lblWebHookCountDes";
            this.lblWebHookCountDes.Size = new System.Drawing.Size(35, 13);
            this.lblWebHookCountDes.TabIndex = 3;
            this.lblWebHookCountDes.Text = "Count";
            // 
            // lblWebHookCount
            // 
            this.lblWebHookCount.AutoSize = true;
            this.lblWebHookCount.Location = new System.Drawing.Point(38, 3);
            this.lblWebHookCount.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.lblWebHookCount.Name = "lblWebHookCount";
            this.lblWebHookCount.Size = new System.Drawing.Size(21, 13);
            this.lblWebHookCount.TabIndex = 2;
            this.lblWebHookCount.Text = "##";
            // 
            // btnAddWebHook
            // 
            this.btnAddWebHook.Location = new System.Drawing.Point(233, 204);
            this.btnAddWebHook.Name = "btnAddWebHook";
            this.btnAddWebHook.Size = new System.Drawing.Size(75, 23);
            this.btnAddWebHook.TabIndex = 1;
            this.btnAddWebHook.Text = "Add";
            this.btnAddWebHook.UseVisualStyleBackColor = true;
            this.btnAddWebHook.Click += new System.EventHandler(this.AddWebHook_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.flpWebHooks);
            this.panel1.Location = new System.Drawing.Point(6, 16);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(303, 182);
            this.panel1.TabIndex = 0;
            // 
            // flpWebHooks
            // 
            this.flpWebHooks.AutoScroll = true;
            this.flpWebHooks.Controls.Add(this.webHookConfig1);
            this.flpWebHooks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpWebHooks.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpWebHooks.Location = new System.Drawing.Point(0, 0);
            this.flpWebHooks.Name = "flpWebHooks";
            this.flpWebHooks.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.flpWebHooks.Size = new System.Drawing.Size(303, 182);
            this.flpWebHooks.TabIndex = 0;
            this.flpWebHooks.WrapContents = false;
            // 
            // webHookConfig1
            // 
            this.webHookConfig1.Location = new System.Drawing.Point(3, 3);
            this.webHookConfig1.Name = "webHookConfig1";
            this.webHookConfig1.Size = new System.Drawing.Size(280, 152);
            this.webHookConfig1.TabIndex = 0;
            // 
            // ttHelp
            // 
            this.ttHelp.AutoPopDelay = 0;
            this.ttHelp.InitialDelay = 500;
            this.ttHelp.ReshowDelay = 100;
            // 
            // gbEi
            // 
            this.gbEi.Controls.Add(this.cbEiTheme);
            this.gbEi.Controls.Add(this.cbEiCombatReplay);
            this.gbEi.Location = new System.Drawing.Point(333, 301);
            this.gbEi.Name = "gbEi";
            this.gbEi.Size = new System.Drawing.Size(315, 67);
            this.gbEi.TabIndex = 13;
            this.gbEi.TabStop = false;
            this.gbEi.Text = "EliteInsights";
            // 
            // cbEiTheme
            // 
            this.cbEiTheme.AutoSize = true;
            this.cbEiTheme.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.SettingsbindingSource, "LightTheme", true));
            this.cbEiTheme.Location = new System.Drawing.Point(6, 42);
            this.cbEiTheme.Name = "cbEiTheme";
            this.cbEiTheme.Size = new System.Drawing.Size(99, 17);
            this.cbEiTheme.TabIndex = 1;
            this.cbEiTheme.Text = "Use light theme";
            this.cbEiTheme.UseVisualStyleBackColor = true;
            // 
            // cbEiCombatReplay
            // 
            this.cbEiCombatReplay.AutoSize = true;
            this.cbEiCombatReplay.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.SettingsbindingSource, "CreateCombatReplay", true));
            this.cbEiCombatReplay.Location = new System.Drawing.Point(6, 19);
            this.cbEiCombatReplay.Name = "cbEiCombatReplay";
            this.cbEiCombatReplay.Size = new System.Drawing.Size(139, 17);
            this.cbEiCombatReplay.TabIndex = 0;
            this.cbEiCombatReplay.Text = "Generate combat replay";
            this.cbEiCombatReplay.UseVisualStyleBackColor = true;
            // 
            // SettingsUI
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(653, 412);
            this.Controls.Add(this.gbEi);
            this.Controls.Add(this.gbDiscord);
            this.Controls.Add(this.gbGeneral);
            this.Controls.Add(this.gbDpsReport);
            this.Controls.Add(this.btnDefault);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.gbCopyLinks);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsUI";
            this.Text = "Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsUI_FormClosing);
            this.gbCopyLinks.ResumeLayout(false);
            this.gbCopyLinks.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SettingsbindingSource)).EndInit();
            this.gbDpsReport.ResumeLayout(false);
            this.gbDpsReport.PerformLayout();
            this.gbGeneral.ResumeLayout(false);
            this.gbGeneral.PerformLayout();
            this.gbDiscord.ResumeLayout(false);
            this.gbDiscord.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.flpWebHooks.ResumeLayout(false);
            this.gbEi.ResumeLayout(false);
            this.gbEi.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbCopyLinks;
        private System.Windows.Forms.CheckBox cbLinkInSameLine;
        private System.Windows.Forms.CheckBox cbEmptyLinesInBetween;
        private System.Windows.Forms.CheckBox cbShowSuccsess;
        private System.Windows.Forms.CheckBox cbShwoEncounterName;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnDefault;
        private System.Windows.Forms.GroupBox gbDpsReport;
        private System.Windows.Forms.LinkLabel linklblGetUserToken;
        private System.Windows.Forms.Label lblGetUserToken;
        private System.Windows.Forms.TextBox txtUserToken;
        private System.Windows.Forms.Label lblUserToken;
        private System.Windows.Forms.GroupBox gbGeneral;
        private System.Windows.Forms.Label lblArcLogsPath;
        private System.Windows.Forms.TextBox txtArcLogPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ComboBox cmbLang;
        private System.Windows.Forms.Label lblLang;
        private System.Windows.Forms.CheckBox cbGNEmotes;
        private System.Windows.Forms.Button btnProxySettings;
        private System.Windows.Forms.GroupBox gbDiscord;
        private System.Windows.Forms.ToolTip ttHelp;
        private System.Windows.Forms.BindingSource SettingsbindingSource;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flpWebHooks;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Label lblWebHookCount;
        private System.Windows.Forms.Button btnAddWebHook;
        private GUIs.WebHookConfig webHookConfig1;
        private System.Windows.Forms.Label lblWebHookCountDes;
        private System.Windows.Forms.GroupBox gbEi;
        private System.Windows.Forms.CheckBox cbEiTheme;
        private System.Windows.Forms.CheckBox cbEiCombatReplay;
        private System.Windows.Forms.CheckBox cbEiOnlyUploaded;
        private System.Windows.Forms.CheckBox cbNameAsUser;
    }
}