namespace LogUploader.GUIs.CorrectPlayer
{
    partial class PlayerToCorrect
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
            this.lblAccountNameDes = new System.Windows.Forms.Label();
            this.lblAccountName = new System.Windows.Forms.Label();
            this.gbAccount = new System.Windows.Forms.GroupBox();
            this.gbSettings = new System.Windows.Forms.GroupBox();
            this.cbHelper = new System.Windows.Forms.ComboBox();
            this.cbMember = new System.Windows.Forms.ComboBox();
            this.rbLFG = new System.Windows.Forms.RadioButton();
            this.rbHelper = new System.Windows.Forms.RadioButton();
            this.rbMember = new System.Windows.Forms.RadioButton();
            this.gbAccount.SuspendLayout();
            this.gbSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblAccountNameDes
            // 
            this.lblAccountNameDes.AutoSize = true;
            this.lblAccountNameDes.Location = new System.Drawing.Point(14, 27);
            this.lblAccountNameDes.Name = "lblAccountNameDes";
            this.lblAccountNameDes.Size = new System.Drawing.Size(50, 13);
            this.lblAccountNameDes.TabIndex = 0;
            this.lblAccountNameDes.Text = "Account:";
            // 
            // lblAccountName
            // 
            this.lblAccountName.AutoSize = true;
            this.lblAccountName.Location = new System.Drawing.Point(15, 49);
            this.lblAccountName.Name = "lblAccountName";
            this.lblAccountName.Size = new System.Drawing.Size(49, 13);
            this.lblAccountName.TabIndex = 0;
            this.lblAccountName.Text = "xyz.1234";
            // 
            // gbAccount
            // 
            this.gbAccount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gbAccount.Controls.Add(this.lblAccountName);
            this.gbAccount.Controls.Add(this.lblAccountNameDes);
            this.gbAccount.Location = new System.Drawing.Point(3, 3);
            this.gbAccount.Name = "gbAccount";
            this.gbAccount.Size = new System.Drawing.Size(141, 93);
            this.gbAccount.TabIndex = 1;
            this.gbAccount.TabStop = false;
            this.gbAccount.Text = "Player";
            // 
            // gbSettings
            // 
            this.gbSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbSettings.Controls.Add(this.cbHelper);
            this.gbSettings.Controls.Add(this.cbMember);
            this.gbSettings.Controls.Add(this.rbLFG);
            this.gbSettings.Controls.Add(this.rbHelper);
            this.gbSettings.Controls.Add(this.rbMember);
            this.gbSettings.Location = new System.Drawing.Point(150, 3);
            this.gbSettings.Name = "gbSettings";
            this.gbSettings.Size = new System.Drawing.Size(297, 93);
            this.gbSettings.TabIndex = 1;
            this.gbSettings.TabStop = false;
            // 
            // cbHelper
            // 
            this.cbHelper.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbHelper.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbHelper.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbHelper.Enabled = false;
            this.cbHelper.FormattingEnabled = true;
            this.cbHelper.Location = new System.Drawing.Point(99, 41);
            this.cbHelper.Name = "cbHelper";
            this.cbHelper.Size = new System.Drawing.Size(192, 21);
            this.cbHelper.TabIndex = 2;
            // 
            // cbMember
            // 
            this.cbMember.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbMember.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbMember.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbMember.Enabled = false;
            this.cbMember.FormattingEnabled = true;
            this.cbMember.Location = new System.Drawing.Point(99, 18);
            this.cbMember.Name = "cbMember";
            this.cbMember.Size = new System.Drawing.Size(192, 21);
            this.cbMember.TabIndex = 1;
            // 
            // rbLFG
            // 
            this.rbLFG.AutoSize = true;
            this.rbLFG.Location = new System.Drawing.Point(6, 65);
            this.rbLFG.Name = "rbLFG";
            this.rbLFG.Size = new System.Drawing.Size(45, 17);
            this.rbLFG.TabIndex = 0;
            this.rbLFG.TabStop = true;
            this.rbLFG.Text = "LFG";
            this.rbLFG.UseVisualStyleBackColor = true;
            this.rbLFG.CheckedChanged += new System.EventHandler(this.rbMember_CheckedChanged);
            // 
            // rbHelper
            // 
            this.rbHelper.AutoSize = true;
            this.rbHelper.Location = new System.Drawing.Point(6, 42);
            this.rbHelper.Name = "rbHelper";
            this.rbHelper.Size = new System.Drawing.Size(56, 17);
            this.rbHelper.TabIndex = 0;
            this.rbHelper.TabStop = true;
            this.rbHelper.Text = "Helper";
            this.rbHelper.UseVisualStyleBackColor = true;
            this.rbHelper.CheckedChanged += new System.EventHandler(this.rbMember_CheckedChanged);
            // 
            // rbMember
            // 
            this.rbMember.AutoSize = true;
            this.rbMember.Location = new System.Drawing.Point(6, 19);
            this.rbMember.Name = "rbMember";
            this.rbMember.Size = new System.Drawing.Size(63, 17);
            this.rbMember.TabIndex = 0;
            this.rbMember.TabStop = true;
            this.rbMember.Text = "Member";
            this.rbMember.UseVisualStyleBackColor = true;
            this.rbMember.CheckedChanged += new System.EventHandler(this.rbMember_CheckedChanged);
            // 
            // PlayerToCorrect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbSettings);
            this.Controls.Add(this.gbAccount);
            this.Name = "PlayerToCorrect";
            this.Size = new System.Drawing.Size(450, 99);
            this.gbAccount.ResumeLayout(false);
            this.gbAccount.PerformLayout();
            this.gbSettings.ResumeLayout(false);
            this.gbSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblAccountNameDes;
        private System.Windows.Forms.Label lblAccountName;
        private System.Windows.Forms.GroupBox gbAccount;
        private System.Windows.Forms.GroupBox gbSettings;
        private System.Windows.Forms.ComboBox cbHelper;
        private System.Windows.Forms.ComboBox cbMember;
        private System.Windows.Forms.RadioButton rbLFG;
        private System.Windows.Forms.RadioButton rbHelper;
        private System.Windows.Forms.RadioButton rbMember;
    }
}
