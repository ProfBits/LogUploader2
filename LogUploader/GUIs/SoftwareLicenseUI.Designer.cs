namespace LogUploader.GUIs
{
    partial class SoftwareLicenseUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SoftwareLicenseUI));
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblBy = new System.Windows.Forms.Label();
            this.lblOwner = new System.Windows.Forms.Label();
            this.lblSoftware = new System.Windows.Forms.Label();
            this.lblFor = new System.Windows.Forms.Label();
            this.lblLizense = new System.Windows.Forms.Label();
            this.rtbLicenseText = new System.Windows.Forms.RichTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblBy);
            this.panel1.Controls.Add(this.lblOwner);
            this.panel1.Controls.Add(this.lblSoftware);
            this.panel1.Controls.Add(this.lblFor);
            this.panel1.Controls.Add(this.lblLizense);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(384, 86);
            this.panel1.TabIndex = 0;
            // 
            // lblBy
            // 
            this.lblBy.AutoSize = true;
            this.lblBy.Location = new System.Drawing.Point(13, 56);
            this.lblBy.Name = "lblBy";
            this.lblBy.Size = new System.Drawing.Size(18, 13);
            this.lblBy.TabIndex = 3;
            this.lblBy.Text = "by";
            // 
            // lblOwner
            // 
            this.lblOwner.AutoSize = true;
            this.lblOwner.Location = new System.Drawing.Point(38, 56);
            this.lblOwner.Name = "lblOwner";
            this.lblOwner.Size = new System.Drawing.Size(38, 13);
            this.lblOwner.TabIndex = 2;
            this.lblOwner.Text = "Owner";
            // 
            // lblSoftware
            // 
            this.lblSoftware.AutoSize = true;
            this.lblSoftware.Location = new System.Drawing.Point(38, 38);
            this.lblSoftware.Name = "lblSoftware";
            this.lblSoftware.Size = new System.Drawing.Size(49, 13);
            this.lblSoftware.TabIndex = 2;
            this.lblSoftware.Text = "Software";
            // 
            // lblFor
            // 
            this.lblFor.AutoSize = true;
            this.lblFor.Location = new System.Drawing.Point(13, 38);
            this.lblFor.Name = "lblFor";
            this.lblFor.Size = new System.Drawing.Size(19, 13);
            this.lblFor.TabIndex = 1;
            this.lblFor.Text = "for";
            // 
            // lblLizense
            // 
            this.lblLizense.AutoSize = true;
            this.lblLizense.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLizense.Location = new System.Drawing.Point(12, 9);
            this.lblLizense.Name = "lblLizense";
            this.lblLizense.Size = new System.Drawing.Size(122, 20);
            this.lblLizense.TabIndex = 0;
            this.lblLizense.Text = "Some License";
            // 
            // rtbLicenseText
            // 
            this.rtbLicenseText.BackColor = System.Drawing.Color.White;
            this.rtbLicenseText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLicenseText.Location = new System.Drawing.Point(10, 10);
            this.rtbLicenseText.Name = "rtbLicenseText";
            this.rtbLicenseText.ReadOnly = true;
            this.rtbLicenseText.ShortcutsEnabled = false;
            this.rtbLicenseText.Size = new System.Drawing.Size(364, 205);
            this.rtbLicenseText.TabIndex = 1;
            this.rtbLicenseText.Text = "";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rtbLicenseText);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 86);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(10);
            this.panel2.Size = new System.Drawing.Size(384, 225);
            this.panel2.TabIndex = 2;
            // 
            // SoftwareLicenseUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(384, 311);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(200, 200);
            this.Name = "SoftwareLicenseUI";
            this.Text = "SoftwareLicenzeUI";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox rtbLicenseText;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblBy;
        private System.Windows.Forms.Label lblOwner;
        private System.Windows.Forms.Label lblSoftware;
        private System.Windows.Forms.Label lblFor;
        private System.Windows.Forms.Label lblLizense;
    }
}