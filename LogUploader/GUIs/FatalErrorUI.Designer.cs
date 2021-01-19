
namespace LogUploader.GUIs
{
    partial class FatalErrorUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FatalErrorUI));
            this.rtbMessage = new System.Windows.Forms.RichTextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnShowLog = new System.Windows.Forms.Button();
            this.lblTitel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtbMessage
            // 
            this.rtbMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbMessage.BackColor = System.Drawing.SystemColors.Control;
            this.rtbMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbMessage.Location = new System.Drawing.Point(12, 31);
            this.rtbMessage.Name = "rtbMessage";
            this.rtbMessage.ReadOnly = true;
            this.rtbMessage.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbMessage.Size = new System.Drawing.Size(337, 157);
            this.rtbMessage.TabIndex = 0;
            this.rtbMessage.Text = "<Message>";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOk.Location = new System.Drawing.Point(274, 206);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // btnShowLog
            // 
            this.btnShowLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnShowLog.Location = new System.Drawing.Point(167, 206);
            this.btnShowLog.Name = "btnShowLog";
            this.btnShowLog.Size = new System.Drawing.Size(99, 23);
            this.btnShowLog.TabIndex = 2;
            this.btnShowLog.Text = "Show Log";
            this.btnShowLog.UseVisualStyleBackColor = true;
            this.btnShowLog.Click += new System.EventHandler(this.BtnShowLog_Click);
            // 
            // lblTitel
            // 
            this.lblTitel.BackColor = System.Drawing.Color.White;
            this.lblTitel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitel.Location = new System.Drawing.Point(0, 0);
            this.lblTitel.Name = "lblTitel";
            this.lblTitel.Size = new System.Drawing.Size(337, 21);
            this.lblTitel.TabIndex = 3;
            this.lblTitel.Text = "<Title>";
            this.lblTitel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 203);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 26);
            this.label1.TabIndex = 4;
            this.label1.Text = "Please provide the Log when \r\nsubmitting a bug. Thanks!";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.lblTitel);
            this.panel1.Location = new System.Drawing.Point(12, 3);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.panel1.Size = new System.Drawing.Size(337, 22);
            this.panel1.TabIndex = 5;
            // 
            // FatalErrorUI
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnOk;
            this.ClientSize = new System.Drawing.Size(361, 241);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnShowLog);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.rtbMessage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(377, 280);
            this.Name = "FatalErrorUI";
            this.Text = "Fatal Error";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbMessage;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnShowLog;
        private System.Windows.Forms.Label lblTitel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
    }
}