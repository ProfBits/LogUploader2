namespace LogUploader.GUI
{
    partial class LoadingScreenUI
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
                m_cts.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadingScreenUI));
            this.pHeader = new System.Windows.Forms.Panel();
            this.lblBeta = new System.Windows.Forms.Label();
            this.lblClose = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.pLoading = new System.Windows.Forms.Panel();
            this.pInfo = new System.Windows.Forms.Panel();
            this.lblPercent = new System.Windows.Forms.Label();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.lblTask = new System.Windows.Forms.Label();
            this.pBottom = new System.Windows.Forms.Panel();
            this.bgWorkerMain = new System.ComponentModel.BackgroundWorker();
            this.pHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.pInfo.SuspendLayout();
            this.pBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pHeader
            // 
            this.pHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pHeader.Controls.Add(this.lblBeta);
            this.pHeader.Controls.Add(this.lblClose);
            this.pHeader.Controls.Add(this.lblTitle);
            this.pHeader.Controls.Add(this.pbLogo);
            this.pHeader.Location = new System.Drawing.Point(0, 0);
            this.pHeader.Margin = new System.Windows.Forms.Padding(0);
            this.pHeader.Name = "pHeader";
            this.pHeader.Size = new System.Drawing.Size(500, 175);
            this.pHeader.TabIndex = 0;
            this.pHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragWindow);
            // 
            // lblBeta
            // 
            this.lblBeta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBeta.AutoSize = true;
            this.lblBeta.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBeta.ForeColor = System.Drawing.Color.White;
            this.lblBeta.Location = new System.Drawing.Point(392, 124);
            this.lblBeta.Name = "lblBeta";
            this.lblBeta.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblBeta.Size = new System.Drawing.Size(39, 13);
            this.lblBeta.TabIndex = 1;
            this.lblBeta.Text = "BETA";
            this.lblBeta.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblClose
            // 
            this.lblClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblClose.AutoSize = true;
            this.lblClose.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.lblClose.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClose.ForeColor = System.Drawing.Color.White;
            this.lblClose.Location = new System.Drawing.Point(470, 9);
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(18, 19);
            this.lblClose.TabIndex = 2;
            this.lblClose.Text = "X";
            this.lblClose.Click += new System.EventHandler(this.LblClose_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Tai Le", 32.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(139, 65);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(297, 55);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Log Uploader";
            this.lblTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragWindow);
            // 
            // pbLogo
            // 
            this.pbLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbLogo.Image = ((System.Drawing.Image)(resources.GetObject("pbLogo.Image")));
            this.pbLogo.Location = new System.Drawing.Point(27, 27);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(110, 110);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbLogo.TabIndex = 0;
            this.pbLogo.TabStop = false;
            this.pbLogo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragWindow);
            // 
            // pLoading
            // 
            this.pLoading.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(59)))), ((int)(((byte)(0)))));
            this.pLoading.Location = new System.Drawing.Point(0, 0);
            this.pLoading.Margin = new System.Windows.Forms.Padding(0);
            this.pLoading.Name = "pLoading";
            this.pLoading.Size = new System.Drawing.Size(501, 10);
            this.pLoading.TabIndex = 1;
            this.pLoading.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragWindow);
            // 
            // pInfo
            // 
            this.pInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pInfo.Controls.Add(this.lblPercent);
            this.pInfo.Controls.Add(this.lblCopyright);
            this.pInfo.Controls.Add(this.lblTask);
            this.pInfo.Location = new System.Drawing.Point(0, 10);
            this.pInfo.Margin = new System.Windows.Forms.Padding(0);
            this.pInfo.Name = "pInfo";
            this.pInfo.Size = new System.Drawing.Size(500, 115);
            this.pInfo.TabIndex = 2;
            this.pInfo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragWindow);
            // 
            // lblPercent
            // 
            this.lblPercent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPercent.AutoSize = true;
            this.lblPercent.ForeColor = System.Drawing.Color.White;
            this.lblPercent.Location = new System.Drawing.Point(464, 8);
            this.lblPercent.Name = "lblPercent";
            this.lblPercent.Size = new System.Drawing.Size(24, 13);
            this.lblPercent.TabIndex = 0;
            this.lblPercent.Text = "0 %";
            this.lblPercent.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblPercent.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragWindow);
            // 
            // lblCopyright
            // 
            this.lblCopyright.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.ForeColor = System.Drawing.Color.White;
            this.lblCopyright.Location = new System.Drawing.Point(9, 93);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(115, 13);
            this.lblCopyright.TabIndex = 0;
            this.lblCopyright.Text = "© ProfBits 2019 - 2024";
            this.lblCopyright.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragWindow);
            // 
            // lblTask
            // 
            this.lblTask.AutoSize = true;
            this.lblTask.ForeColor = System.Drawing.Color.White;
            this.lblTask.Location = new System.Drawing.Point(9, 8);
            this.lblTask.Name = "lblTask";
            this.lblTask.Size = new System.Drawing.Size(43, 13);
            this.lblTask.TabIndex = 0;
            this.lblTask.Text = "Starting";
            this.lblTask.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragWindow);
            // 
            // pBottom
            // 
            this.pBottom.BackColor = System.Drawing.Color.Maroon;
            this.pBottom.Controls.Add(this.pInfo);
            this.pBottom.Controls.Add(this.pLoading);
            this.pBottom.Location = new System.Drawing.Point(0, 175);
            this.pBottom.Margin = new System.Windows.Forms.Padding(0);
            this.pBottom.Name = "pBottom";
            this.pBottom.Size = new System.Drawing.Size(500, 125);
            this.pBottom.TabIndex = 3;
            this.pBottom.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragWindow);
            // 
            // bgWorkerMain
            // 
            this.bgWorkerMain.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker1_DoWork);
            // 
            // LoadingScreenUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(500, 300);
            this.Controls.Add(this.pBottom);
            this.Controls.Add(this.pHeader);
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoadingScreenUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LogUploader Startup";
            this.Shown += new System.EventHandler(this.LoadApplication);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragWindow);
            this.pHeader.ResumeLayout(false);
            this.pHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.pInfo.ResumeLayout(false);
            this.pInfo.PerformLayout();
            this.pBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pHeader;
        private System.Windows.Forms.Label lblClose;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Panel pLoading;
        private System.Windows.Forms.Panel pInfo;
        private System.Windows.Forms.Label lblPercent;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.Label lblTask;
        private System.Windows.Forms.Panel pBottom;
        private System.ComponentModel.BackgroundWorker bgWorkerMain;
        private System.Windows.Forms.Label lblBeta;
    }
}