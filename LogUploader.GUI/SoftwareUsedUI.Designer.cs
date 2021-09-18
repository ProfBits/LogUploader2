namespace LogUploader.GUIs
{
    partial class SoftwareUsedUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SoftwareUsedUI));
            this.panel1 = new System.Windows.Forms.Panel();
            this.bntClose = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.Dapper = new LogUploader.GUIs.SoftwareItem();
            this.SQLiteCore = new LogUploader.GUIs.SoftwareItem();
            this.WindowsAPICodePackCore = new LogUploader.GUIs.SoftwareItem();
            this.WindowsAPICodePackShell = new LogUploader.GUIs.SoftwareItem();
            this.MakeSFX = new LogUploader.GUIs.SoftwareItem();
            this.CommonMark = new LogUploader.GUIs.SoftwareItem();
            this.NewtonsoftJson = new LogUploader.GUIs.SoftwareItem();
            this.PropMapper = new LogUploader.GUIs.SoftwareItem();
            this.MicrosoftBuild = new LogUploader.GUIs.SoftwareItem();
            this.MicrosoftBuildUtilitiesCore = new LogUploader.GUIs.SoftwareItem();
            this.SystemResourcesExtensions = new LogUploader.GUIs.SoftwareItem();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.bntClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 368);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(323, 32);
            this.panel1.TabIndex = 0;
            // 
            // bntClose
            // 
            this.bntClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bntClose.Location = new System.Drawing.Point(228, 6);
            this.bntClose.Name = "bntClose";
            this.bntClose.Size = new System.Drawing.Size(75, 23);
            this.bntClose.TabIndex = 0;
            this.bntClose.Text = "Close";
            this.bntClose.UseVisualStyleBackColor = true;
            this.bntClose.Click += new System.EventHandler(this.BntClose_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Controls.Add(this.Dapper);
            this.flowLayoutPanel1.Controls.Add(this.SQLiteCore);
            this.flowLayoutPanel1.Controls.Add(this.WindowsAPICodePackCore);
            this.flowLayoutPanel1.Controls.Add(this.WindowsAPICodePackShell);
            this.flowLayoutPanel1.Controls.Add(this.MakeSFX);
            this.flowLayoutPanel1.Controls.Add(this.CommonMark);
            this.flowLayoutPanel1.Controls.Add(this.NewtonsoftJson);
            this.flowLayoutPanel1.Controls.Add(this.PropMapper);
            this.flowLayoutPanel1.Controls.Add(this.MicrosoftBuild);
            this.flowLayoutPanel1.Controls.Add(this.MicrosoftBuildUtilitiesCore);
            this.flowLayoutPanel1.Controls.Add(this.SystemResourcesExtensions);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(323, 368);
            this.flowLayoutPanel1.TabIndex = 1;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // Dapper
            // 
            this.Dapper.Copyright = "© Stack Exchange, Inc. 2019";
            this.Dapper.Image = null;
            this.Dapper.ImageUsed = false;
            this.Dapper.License = null;
            this.Dapper.LicenseStr = "Apache-2.0";
            this.Dapper.Location = new System.Drawing.Point(3, 3);
            this.Dapper.Name = "Dapper";
            this.Dapper.ProjectLink = "https://www.nuget.org/packages/Dapper/2.0.35?_src=template";
            this.Dapper.Size = new System.Drawing.Size(300, 106);
            this.Dapper.Software = "Dapper";
            this.Dapper.TabIndex = 0;
            // 
            // SQLiteCore
            // 
            this.SQLiteCore.Copyright = "SQLite Development Team";
            this.SQLiteCore.Image = ((System.Drawing.Image)(resources.GetObject("SQLiteCore.Image")));
            this.SQLiteCore.ImageUsed = true;
            this.SQLiteCore.License = null;
            this.SQLiteCore.LicenseStr = "Public Domain";
            this.SQLiteCore.Location = new System.Drawing.Point(3, 115);
            this.SQLiteCore.Name = "SQLiteCore";
            this.SQLiteCore.ProjectLink = "https://www.nuget.org/packages/System.Data.SQLite.Core/1.0.112.2?_src=template";
            this.SQLiteCore.Size = new System.Drawing.Size(300, 106);
            this.SQLiteCore.Software = "SQLite.Core";
            this.SQLiteCore.TabIndex = 1;
            // 
            // WindowsAPICodePackCore
            // 
            this.WindowsAPICodePackCore.Copyright = "© Aybe and/or Microsoft ";
            this.WindowsAPICodePackCore.Image = null;
            this.WindowsAPICodePackCore.ImageUsed = false;
            this.WindowsAPICodePackCore.License = null;
            this.WindowsAPICodePackCore.LicenseStr = "Custom License";
            this.WindowsAPICodePackCore.Location = new System.Drawing.Point(3, 227);
            this.WindowsAPICodePackCore.Name = "WindowsAPICodePackCore";
            this.WindowsAPICodePackCore.ProjectLink = "https://www.nuget.org/packages/WindowsAPICodePack-Core/1.1.2?_src=template";
            this.WindowsAPICodePackCore.Size = new System.Drawing.Size(300, 106);
            this.WindowsAPICodePackCore.Software = "WindowsAPICodePack-Core";
            this.WindowsAPICodePackCore.TabIndex = 2;
            // 
            // WindowsAPICodePackShell
            // 
            this.WindowsAPICodePackShell.Copyright = "© Aybe and/or Microsoft ";
            this.WindowsAPICodePackShell.Image = null;
            this.WindowsAPICodePackShell.ImageUsed = false;
            this.WindowsAPICodePackShell.License = null;
            this.WindowsAPICodePackShell.LicenseStr = "Custom License";
            this.WindowsAPICodePackShell.Location = new System.Drawing.Point(3, 339);
            this.WindowsAPICodePackShell.Name = "WindowsAPICodePackShell";
            this.WindowsAPICodePackShell.ProjectLink = "https://www.nuget.org/packages/WindowsAPICodePack-Shell/1.1.1?_src=template";
            this.WindowsAPICodePackShell.Size = new System.Drawing.Size(300, 106);
            this.WindowsAPICodePackShell.Software = "WindowsAPICodePack-Shell";
            this.WindowsAPICodePackShell.TabIndex = 3;
            // 
            // MakeSFX
            // 
            this.MakeSFX.Copyright = "© RevoCue AI s.r.o. 2012 - 2020";
            this.MakeSFX.Image = ((System.Drawing.Image)(resources.GetObject("MakeSFX.Image")));
            this.MakeSFX.ImageUsed = true;
            this.MakeSFX.License = null;
            this.MakeSFX.LicenseStr = "Free";
            this.MakeSFX.Location = new System.Drawing.Point(3, 451);
            this.MakeSFX.Name = "MakeSFX";
            this.MakeSFX.ProjectLink = "https://revocue.cz/en/make-sfx/index.php";
            this.MakeSFX.Size = new System.Drawing.Size(300, 106);
            this.MakeSFX.Software = "Make SFX";
            this.MakeSFX.TabIndex = 4;
            // 
            // CommonMark
            // 
            this.CommonMark.Copyright = "© 2014, Kārlis Gaņģis All rights reserved.";
            this.CommonMark.Image = ((System.Drawing.Image)(resources.GetObject("CommonMark.Image")));
            this.CommonMark.ImageUsed = true;
            this.CommonMark.License = null;
            this.CommonMark.LicenseStr = "Custom";
            this.CommonMark.Location = new System.Drawing.Point(3, 563);
            this.CommonMark.Name = "CommonMark";
            this.CommonMark.ProjectLink = "https://revocue.cz/en/make-sfx/index.php";
            this.CommonMark.Size = new System.Drawing.Size(300, 106);
            this.CommonMark.Software = "CommonMark.NET";
            this.CommonMark.TabIndex = 5;
            // 
            // NewtonsoftJson
            // 
            this.NewtonsoftJson.Copyright = "© 2007 James Newton-King";
            this.NewtonsoftJson.Image = ((System.Drawing.Image)(resources.GetObject("NewtonsoftJson.Image")));
            this.NewtonsoftJson.ImageUsed = true;
            this.NewtonsoftJson.License = null;
            this.NewtonsoftJson.LicenseStr = "MIT";
            this.NewtonsoftJson.Location = new System.Drawing.Point(3, 675);
            this.NewtonsoftJson.Name = "NewtonsoftJson";
            this.NewtonsoftJson.ProjectLink = "https://www.newtonsoft.com/json";
            this.NewtonsoftJson.Size = new System.Drawing.Size(300, 106);
            this.NewtonsoftJson.Software = "Newtonsoft.Json";
            this.NewtonsoftJson.TabIndex = 6;
            // 
            // PropMapper
            // 
            this.PropMapper.Copyright = "Copyright (c) 2019 Jitbit (the company behind \"Jitbit Helpdesk\" software)";
            this.PropMapper.Image = null;
            this.PropMapper.ImageUsed = false;
            this.PropMapper.License = null;
            this.PropMapper.LicenseStr = "MIT";
            this.PropMapper.Location = new System.Drawing.Point(3, 787);
            this.PropMapper.Name = "PropMapper";
            this.PropMapper.ProjectLink = "https://github.com/jitbit/PropMapper";
            this.PropMapper.Size = new System.Drawing.Size(300, 106);
            this.PropMapper.Software = "PropMapper";
            this.PropMapper.TabIndex = 7;
            // 
            // MicrosoftBuild
            // 
            this.MicrosoftBuild.Copyright = "© .NET Foundation and Contributors";
            this.MicrosoftBuild.Image = ((System.Drawing.Image)(resources.GetObject("MicrosoftBuild.Image")));
            this.MicrosoftBuild.ImageUsed = true;
            this.MicrosoftBuild.License = null;
            this.MicrosoftBuild.LicenseStr = "MIT";
            this.MicrosoftBuild.Location = new System.Drawing.Point(3, 899);
            this.MicrosoftBuild.Name = "MicrosoftBuild";
            this.MicrosoftBuild.ProjectLink = "https://www.nuget.org/packages/Microsoft.Build/16.11.0";
            this.MicrosoftBuild.Size = new System.Drawing.Size(300, 106);
            this.MicrosoftBuild.Software = "Microsoft.Build";
            this.MicrosoftBuild.TabIndex = 8;
            // 
            // MicrosoftBuildUtilitiesCore
            // 
            this.MicrosoftBuildUtilitiesCore.Copyright = "© .NET Foundation and Contributors";
            this.MicrosoftBuildUtilitiesCore.Image = ((System.Drawing.Image)(resources.GetObject("MicrosoftBuildUtilitiesCore.Image")));
            this.MicrosoftBuildUtilitiesCore.ImageUsed = true;
            this.MicrosoftBuildUtilitiesCore.License = null;
            this.MicrosoftBuildUtilitiesCore.LicenseStr = "MIT";
            this.MicrosoftBuildUtilitiesCore.Location = new System.Drawing.Point(3, 1011);
            this.MicrosoftBuildUtilitiesCore.Name = "MicrosoftBuildUtilitiesCore";
            this.MicrosoftBuildUtilitiesCore.ProjectLink = "https://www.nuget.org/packages/Microsoft.Build.Utilities.Core/16.11.0";
            this.MicrosoftBuildUtilitiesCore.Size = new System.Drawing.Size(300, 106);
            this.MicrosoftBuildUtilitiesCore.Software = "Microsoft.Build.Utilities.Core";
            this.MicrosoftBuildUtilitiesCore.TabIndex = 9;
            // 
            // SystemResourcesExtensions
            // 
            this.SystemResourcesExtensions.Copyright = "© .NET Foundation and Contributors";
            this.SystemResourcesExtensions.Image = ((System.Drawing.Image)(resources.GetObject("SystemResourcesExtensions.Image")));
            this.SystemResourcesExtensions.ImageUsed = true;
            this.SystemResourcesExtensions.License = null;
            this.SystemResourcesExtensions.LicenseStr = "MIT";
            this.SystemResourcesExtensions.Location = new System.Drawing.Point(3, 1123);
            this.SystemResourcesExtensions.Name = "SystemResourcesExtensions";
            this.SystemResourcesExtensions.ProjectLink = "https://www.nuget.org/packages/System.Resources.Extensions/5.0.0";
            this.SystemResourcesExtensions.Size = new System.Drawing.Size(300, 106);
            this.SystemResourcesExtensions.Software = "System.Resources.Extensions";
            this.SystemResourcesExtensions.TabIndex = 10;
            // 
            // SoftwareUsedUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.bntClose;
            this.ClientSize = new System.Drawing.Size(323, 400);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(339, 2000);
            this.MinimumSize = new System.Drawing.Size(339, 400);
            this.Name = "SoftwareUsedUI";
            this.Text = "Used Software";
            this.panel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button bntClose;
        private SoftwareItem Dapper;
        private SoftwareItem SQLiteCore;
        private SoftwareItem WindowsAPICodePackCore;
        private SoftwareItem WindowsAPICodePackShell;
        private SoftwareItem MakeSFX;
        private SoftwareItem CommonMark;
        private SoftwareItem NewtonsoftJson;
        private SoftwareItem PropMapper;
        private SoftwareItem MicrosoftBuild;
        private SoftwareItem MicrosoftBuildUtilitiesCore;
        private SoftwareItem SystemResourcesExtensions;
    }
}