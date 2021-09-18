namespace LogUploader.Tools.Settings.GUI
{
    partial class ProxyConfigUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProxyConfigUI));
            this.cbUseProxy = new System.Windows.Forms.CheckBox();
            this.gbSettings = new System.Windows.Forms.GroupBox();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.nudPort = new System.Windows.Forms.NumericUpDown();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblHost = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblPortSeperator = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
            this.SuspendLayout();
            // 
            // cbUseProxy
            // 
            this.cbUseProxy.AutoSize = true;
            this.cbUseProxy.Location = new System.Drawing.Point(12, 12);
            this.cbUseProxy.Name = "cbUseProxy";
            this.cbUseProxy.Size = new System.Drawing.Size(74, 17);
            this.cbUseProxy.TabIndex = 0;
            this.cbUseProxy.Text = "Use Proxy";
            this.cbUseProxy.UseVisualStyleBackColor = true;
            // 
            // gbSettings
            // 
            this.gbSettings.Controls.Add(this.lblPassword);
            this.gbSettings.Controls.Add(this.lblUser);
            this.gbSettings.Controls.Add(this.lblPortSeperator);
            this.gbSettings.Controls.Add(this.lblPort);
            this.gbSettings.Controls.Add(this.lblHost);
            this.gbSettings.Controls.Add(this.txtPassword);
            this.gbSettings.Controls.Add(this.txtUser);
            this.gbSettings.Controls.Add(this.nudPort);
            this.gbSettings.Controls.Add(this.txtHost);
            this.gbSettings.Location = new System.Drawing.Point(12, 35);
            this.gbSettings.Name = "gbSettings";
            this.gbSettings.Size = new System.Drawing.Size(260, 152);
            this.gbSettings.TabIndex = 1;
            this.gbSettings.TabStop = false;
            this.gbSettings.Text = "Proxy Settings";
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(6, 37);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(162, 20);
            this.txtHost.TabIndex = 0;
            // 
            // nudPort
            // 
            this.nudPort.Location = new System.Drawing.Point(184, 37);
            this.nudPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudPort.Name = "nudPort";
            this.nudPort.Size = new System.Drawing.Size(70, 20);
            this.nudPort.TabIndex = 1;
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(6, 81);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(162, 20);
            this.txtUser.TabIndex = 2;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(6, 120);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(162, 20);
            this.txtPassword.TabIndex = 2;
            // 
            // lblHost
            // 
            this.lblHost.AutoSize = true;
            this.lblHost.Location = new System.Drawing.Point(6, 21);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(58, 13);
            this.lblHost.TabIndex = 3;
            this.lblHost.Text = "Hostname:";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(181, 21);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(29, 13);
            this.lblPort.TabIndex = 3;
            this.lblPort.Text = "Port:";
            // 
            // lblPortSeperator
            // 
            this.lblPortSeperator.AutoSize = true;
            this.lblPortSeperator.Location = new System.Drawing.Point(171, 40);
            this.lblPortSeperator.Margin = new System.Windows.Forms.Padding(0);
            this.lblPortSeperator.Name = "lblPortSeperator";
            this.lblPortSeperator.Size = new System.Drawing.Size(10, 13);
            this.lblPortSeperator.TabIndex = 3;
            this.lblPortSeperator.Text = ":";
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(6, 65);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(32, 13);
            this.lblUser.TabIndex = 3;
            this.lblUser.Text = "User:";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(6, 104);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(56, 13);
            this.lblPassword.TabIndex = 3;
            this.lblPassword.Text = "Password:";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(197, 193);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(116, 193);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // ProxyConfigUI
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(284, 225);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gbSettings);
            this.Controls.Add(this.cbUseProxy);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProxyConfigUI";
            this.ShowInTaskbar = false;
            this.Text = "ProxyConfigUI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProxyConfigUI_FormClosing);
            this.gbSettings.ResumeLayout(false);
            this.gbSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbUseProxy;
        private System.Windows.Forms.GroupBox gbSettings;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label lblPortSeperator;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.NumericUpDown nudPort;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}