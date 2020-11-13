namespace LogUploader.GUIs
{
    partial class LoadingBar
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
                Cts.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadingBar));
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblCancel = new System.Windows.Forms.Label();
            this.lblPercent = new System.Windows.Forms.Label();
            this.lblTask = new System.Windows.Forms.Label();
            this.pLoadingBk = new System.Windows.Forms.Panel();
            this.pLoading = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblClose = new System.Windows.Forms.Label();
            this.lblTaskName = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.pLoadingBk.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblCancel);
            this.panel1.Controls.Add(this.lblPercent);
            this.panel1.Controls.Add(this.lblTask);
            this.panel1.Controls.Add(this.pLoadingBk);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 69);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(350, 81);
            this.panel1.TabIndex = 0;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragWindow);
            // 
            // lblCancel
            // 
            this.lblCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCancel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCancel.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.lblCancel.Location = new System.Drawing.Point(267, 48);
            this.lblCancel.Name = "lblCancel";
            this.lblCancel.Size = new System.Drawing.Size(71, 24);
            this.lblCancel.TabIndex = 0;
            this.lblCancel.Text = "Cancel";
            this.lblCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCancel.Click += new System.EventHandler(this.lblCancel_Click);
            // 
            // lblPercent
            // 
            this.lblPercent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPercent.AutoSize = true;
            this.lblPercent.Location = new System.Drawing.Point(317, 15);
            this.lblPercent.Name = "lblPercent";
            this.lblPercent.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblPercent.Size = new System.Drawing.Size(30, 13);
            this.lblPercent.TabIndex = 0;
            this.lblPercent.Text = "42 %";
            this.lblPercent.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblPercent.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragWindow);
            // 
            // lblTask
            // 
            this.lblTask.AutoSize = true;
            this.lblTask.Location = new System.Drawing.Point(3, 15);
            this.lblTask.Name = "lblTask";
            this.lblTask.Size = new System.Drawing.Size(31, 13);
            this.lblTask.TabIndex = 0;
            this.lblTask.Text = "Task";
            this.lblTask.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragWindow);
            // 
            // pLoadingBk
            // 
            this.pLoadingBk.BackColor = System.Drawing.Color.Maroon;
            this.pLoadingBk.Controls.Add(this.pLoading);
            this.pLoadingBk.Dock = System.Windows.Forms.DockStyle.Top;
            this.pLoadingBk.Location = new System.Drawing.Point(0, 0);
            this.pLoadingBk.Name = "pLoadingBk";
            this.pLoadingBk.Size = new System.Drawing.Size(350, 10);
            this.pLoadingBk.TabIndex = 0;
            this.pLoadingBk.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragWindow);
            // 
            // pLoading
            // 
            this.pLoading.BackColor = System.Drawing.Color.OrangeRed;
            this.pLoading.Dock = System.Windows.Forms.DockStyle.Left;
            this.pLoading.Location = new System.Drawing.Point(0, 0);
            this.pLoading.Margin = new System.Windows.Forms.Padding(0);
            this.pLoading.Name = "pLoading";
            this.pLoading.Size = new System.Drawing.Size(139, 10);
            this.pLoading.TabIndex = 0;
            this.pLoading.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragWindow);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblClose);
            this.panel2.Controls.Add(this.lblTaskName);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(350, 69);
            this.panel2.TabIndex = 1;
            this.panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragWindow);
            // 
            // lblClose
            // 
            this.lblClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblClose.AutoSize = true;
            this.lblClose.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.lblClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClose.Location = new System.Drawing.Point(324, 9);
            this.lblClose.Margin = new System.Windows.Forms.Padding(0);
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(17, 16);
            this.lblClose.TabIndex = 0;
            this.lblClose.Text = "X";
            this.lblClose.Click += new System.EventHandler(this.lblClose_Click);
            // 
            // lblTaskName
            // 
            this.lblTaskName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTaskName.AutoSize = true;
            this.lblTaskName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTaskName.Location = new System.Drawing.Point(12, 35);
            this.lblTaskName.Name = "lblTaskName";
            this.lblTaskName.Size = new System.Drawing.Size(66, 20);
            this.lblTaskName.TabIndex = 0;
            this.lblTaskName.Text = "Loading";
            this.lblTaskName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragWindow);
            // 
            // LoadingBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(350, 150);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LoadingBar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "LoadingBar";
            this.Shown += new System.EventHandler(this.LoadingBar_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragWindow);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pLoadingBk.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pLoadingBk;
        private System.Windows.Forms.Panel pLoading;
        private System.Windows.Forms.Label lblPercent;
        private System.Windows.Forms.Label lblTask;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblTaskName;
        private System.Windows.Forms.Label lblCancel;
        private System.Windows.Forms.Label lblClose;
    }
}