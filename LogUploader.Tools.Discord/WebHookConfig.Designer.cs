namespace LogUploader.Tools.Discord.GUI
{
    partial class WebHookConfig
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblNameDes = new System.Windows.Forms.Label();
            this.lblLinkDes = new System.Windows.Forms.Label();
            this.lblFormatDes = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtLink = new System.Windows.Forms.TextBox();
            this.cmbFormat = new System.Windows.Forms.ComboBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtAvatar = new System.Windows.Forms.TextBox();
            this.lblAvatarDes = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblNameDes
            // 
            this.lblNameDes.AutoSize = true;
            this.lblNameDes.Location = new System.Drawing.Point(6, 19);
            this.lblNameDes.Margin = new System.Windows.Forms.Padding(3);
            this.lblNameDes.Name = "lblNameDes";
            this.lblNameDes.Size = new System.Drawing.Size(38, 13);
            this.lblNameDes.TabIndex = 0;
            this.lblNameDes.Text = "Name:";
            // 
            // lblLinkDes
            // 
            this.lblLinkDes.AutoSize = true;
            this.lblLinkDes.Location = new System.Drawing.Point(6, 45);
            this.lblLinkDes.Margin = new System.Windows.Forms.Padding(3);
            this.lblLinkDes.Name = "lblLinkDes";
            this.lblLinkDes.Size = new System.Drawing.Size(30, 13);
            this.lblLinkDes.TabIndex = 0;
            this.lblLinkDes.Text = "Link:";
            // 
            // lblFormatDes
            // 
            this.lblFormatDes.AutoSize = true;
            this.lblFormatDes.Location = new System.Drawing.Point(6, 71);
            this.lblFormatDes.Margin = new System.Windows.Forms.Padding(3);
            this.lblFormatDes.Name = "lblFormatDes";
            this.lblFormatDes.Size = new System.Drawing.Size(42, 13);
            this.lblFormatDes.TabIndex = 0;
            this.lblFormatDes.Text = "Format:";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(54, 16);
            this.txtName.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(217, 20);
            this.txtName.TabIndex = 1;
            // 
            // txtLink
            // 
            this.txtLink.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLink.Location = new System.Drawing.Point(54, 42);
            this.txtLink.Margin = new System.Windows.Forms.Padding(3, 3, 12, 3);
            this.txtLink.Name = "txtLink";
            this.txtLink.Size = new System.Drawing.Size(217, 20);
            this.txtLink.TabIndex = 1;
            // 
            // cmbFormat
            // 
            this.cmbFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFormat.FormattingEnabled = true;
            this.cmbFormat.Location = new System.Drawing.Point(54, 68);
            this.cmbFormat.Name = "cmbFormat";
            this.cmbFormat.Size = new System.Drawing.Size(217, 21);
            this.cmbFormat.TabIndex = 2;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(196, 121);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtAvatar);
            this.groupBox1.Controls.Add(this.lblAvatarDes);
            this.groupBox1.Controls.Add(this.lblNameDes);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.lblLinkDes);
            this.groupBox1.Controls.Add(this.cmbFormat);
            this.groupBox1.Controls.Add(this.lblFormatDes);
            this.groupBox1.Controls.Add(this.txtLink);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 149);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // txtAvatar
            // 
            this.txtAvatar.Location = new System.Drawing.Point(78, 95);
            this.txtAvatar.Name = "txtAvatar";
            this.txtAvatar.Size = new System.Drawing.Size(193, 20);
            this.txtAvatar.TabIndex = 4;
            // 
            // lblAvatarDes
            // 
            this.lblAvatarDes.AutoSize = true;
            this.lblAvatarDes.Location = new System.Drawing.Point(6, 98);
            this.lblAvatarDes.Margin = new System.Windows.Forms.Padding(3);
            this.lblAvatarDes.Name = "lblAvatarDes";
            this.lblAvatarDes.Size = new System.Drawing.Size(66, 13);
            this.lblAvatarDes.TabIndex = 0;
            this.lblAvatarDes.Text = "Avatar URL:";
            // 
            // WebHookConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "WebHookConfig";
            this.Size = new System.Drawing.Size(280, 149);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblNameDes;
        private System.Windows.Forms.Label lblLinkDes;
        private System.Windows.Forms.Label lblFormatDes;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtLink;
        private System.Windows.Forms.ComboBox cmbFormat;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtAvatar;
        private System.Windows.Forms.Label lblAvatarDes;
    }
}
