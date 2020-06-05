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
            this.bntClose.Click += new System.EventHandler(this.bntClose_Click);
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
    }
}