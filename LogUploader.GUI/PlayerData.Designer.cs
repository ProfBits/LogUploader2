namespace LogUploader.GUIs
{
    partial class PlayerData
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pbClass = new System.Windows.Forms.PictureBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblSG = new System.Windows.Forms.Label();
            this.lblDPS = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbClass)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Controls.Add(this.pbClass, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblSG, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblDPS, 3, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(150, 20);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // pbClass
            // 
            this.pbClass.Location = new System.Drawing.Point(0, 0);
            this.pbClass.Margin = new System.Windows.Forms.Padding(0);
            this.pbClass.Name = "pbClass";
            this.pbClass.Size = new System.Drawing.Size(20, 20);
            this.pbClass.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbClass.TabIndex = 0;
            this.pbClass.TabStop = false;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblName.Location = new System.Drawing.Point(21, 0);
            this.lblName.Margin = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(69, 20);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSG
            // 
            this.lblSG.AutoSize = true;
            this.lblSG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSG.Location = new System.Drawing.Point(90, 0);
            this.lblSG.Margin = new System.Windows.Forms.Padding(0);
            this.lblSG.Name = "lblSG";
            this.lblSG.Size = new System.Drawing.Size(20, 20);
            this.lblSG.TabIndex = 3;
            this.lblSG.Text = "1";
            this.lblSG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDPS
            // 
            this.lblDPS.AutoSize = true;
            this.lblDPS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDPS.Location = new System.Drawing.Point(110, 0);
            this.lblDPS.Margin = new System.Windows.Forms.Padding(0);
            this.lblDPS.Name = "lblDPS";
            this.lblDPS.Size = new System.Drawing.Size(40, 20);
            this.lblDPS.TabIndex = 4;
            this.lblDPS.Text = "dps";
            this.lblDPS.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PlayerData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "PlayerData";
            this.Size = new System.Drawing.Size(150, 20);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbClass)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pbClass;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblSG;
        private System.Windows.Forms.Label lblDPS;
    }
}
