namespace LogUploader.GUIs
{
    partial class SoftwareItem
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.llLizenze = new System.Windows.Forms.LinkLabel();
            this.llProjectpage = new System.Windows.Forms.LinkLabel();
            this.lblLicense = new System.Windows.Forms.Label();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.lblsoft = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.Size = new System.Drawing.Size(100, 106);
            this.panel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(10, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(80, 86);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.llLizenze);
            this.panel2.Controls.Add(this.llProjectpage);
            this.panel2.Controls.Add(this.lblLicense);
            this.panel2.Controls.Add(this.lblCopyright);
            this.panel2.Controls.Add(this.lblsoft);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(100, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 106);
            this.panel2.TabIndex = 1;
            // 
            // llLizenze
            // 
            this.llLizenze.AutoSize = true;
            this.llLizenze.Location = new System.Drawing.Point(73, 75);
            this.llLizenze.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.llLizenze.Name = "llLizenze";
            this.llLizenze.Size = new System.Drawing.Size(69, 13);
            this.llLizenze.TabIndex = 2;
            this.llLizenze.TabStop = true;
            this.llLizenze.Text = "view License";
            this.llLizenze.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llLizenze_LinkClicked);
            // 
            // llProjectpage
            // 
            this.llProjectpage.AutoSize = true;
            this.llProjectpage.Location = new System.Drawing.Point(10, 75);
            this.llProjectpage.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.llProjectpage.Name = "llProjectpage";
            this.llProjectpage.Size = new System.Drawing.Size(40, 13);
            this.llProjectpage.TabIndex = 1;
            this.llProjectpage.TabStop = true;
            this.llProjectpage.Text = "Project";
            this.llProjectpage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llProjectpage_LinkClicked);
            // 
            // lblLicense
            // 
            this.lblLicense.AutoSize = true;
            this.lblLicense.Location = new System.Drawing.Point(11, 56);
            this.lblLicense.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.lblLicense.Name = "lblLicense";
            this.lblLicense.Size = new System.Drawing.Size(44, 13);
            this.lblLicense.TabIndex = 0;
            this.lblLicense.Text = "License";
            // 
            // lblCopyright
            // 
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.Location = new System.Drawing.Point(11, 40);
            this.lblCopyright.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(120, 13);
            this.lblCopyright.TabIndex = 0;
            this.lblCopyright.Text = "© Person #### - ####";
            // 
            // lblsoft
            // 
            this.lblsoft.AutoSize = true;
            this.lblsoft.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblsoft.Location = new System.Drawing.Point(10, 17);
            this.lblsoft.Margin = new System.Windows.Forms.Padding(10, 20, 3, 0);
            this.lblsoft.Name = "lblsoft";
            this.lblsoft.Size = new System.Drawing.Size(33, 20);
            this.lblsoft.TabIndex = 0;
            this.lblsoft.Text = "Lib";
            // 
            // SoftwareItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "SoftwareItem";
            this.Size = new System.Drawing.Size(300, 106);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.LinkLabel llLizenze;
        private System.Windows.Forms.LinkLabel llProjectpage;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.Label lblsoft;
        private System.Windows.Forms.Label lblLicense;
    }
}
