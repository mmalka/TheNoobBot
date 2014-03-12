namespace Archaeologist
{
    partial class DigSitesListManagement
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DigSitesListManagement));
            this.listDigsitesDGV = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.saveB = new DevComponents.DotNetBar.ButtonX();
            this.solvingEveryXMin = new DevComponents.Editors.IntegerInput();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.maxTryByDigsite = new DevComponents.Editors.IntegerInput();
            ((System.ComponentModel.ISupportInitialize)(this.listDigsitesDGV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.solvingEveryXMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxTryByDigsite)).BeginInit();
            this.SuspendLayout();
            // 
            // listDigsitesDGV
            // 
            this.listDigsitesDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listDigsitesDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.listDigsitesDGV.DefaultCellStyle = dataGridViewCellStyle1;
            this.listDigsitesDGV.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.listDigsitesDGV.Location = new System.Drawing.Point(12, 41);
            this.listDigsitesDGV.Name = "listDigsitesDGV";
            this.listDigsitesDGV.Size = new System.Drawing.Size(577, 327);
            this.listDigsitesDGV.TabIndex = 0;
            // 
            // saveB
            // 
            this.saveB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.saveB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.saveB.Location = new System.Drawing.Point(431, 13);
            this.saveB.Name = "saveB";
            this.saveB.Size = new System.Drawing.Size(157, 22);
            this.saveB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.saveB.TabIndex = 1;
            this.saveB.Text = "Save";
            this.saveB.Click += new System.EventHandler(this.saveB_Click);
            // 
            // solvingEveryXMin
            // 
            this.solvingEveryXMin.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.solvingEveryXMin.BackgroundStyle.Class = "DateTimeInputBackground";
            this.solvingEveryXMin.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.solvingEveryXMin.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.solvingEveryXMin.ForeColor = System.Drawing.Color.Black;
            this.solvingEveryXMin.Location = new System.Drawing.Point(94, 14);
            this.solvingEveryXMin.MinValue = 1;
            this.solvingEveryXMin.Name = "solvingEveryXMin";
            this.solvingEveryXMin.ShowUpDown = true;
            this.solvingEveryXMin.Size = new System.Drawing.Size(69, 22);
            this.solvingEveryXMin.TabIndex = 2;
            this.solvingEveryXMin.Value = 1;
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.ForeColor = System.Drawing.Color.Black;
            this.labelX1.Location = new System.Drawing.Point(12, 13);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(77, 22);
            this.labelX1.TabIndex = 3;
            this.labelX1.Text = "Solving Every";
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.ForeColor = System.Drawing.Color.Black;
            this.labelX2.Location = new System.Drawing.Point(169, 13);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(30, 22);
            this.labelX2.TabIndex = 4;
            this.labelX2.Text = "min";
            // 
            // labelX3
            // 
            this.labelX3.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = "";
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.ForeColor = System.Drawing.Color.Black;
            this.labelX3.Location = new System.Drawing.Point(204, 14);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(98, 22);
            this.labelX3.TabIndex = 6;
            this.labelX3.Text = "Max Try By Digsite";
            // 
            // maxTryByDigsite
            // 
            this.maxTryByDigsite.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.maxTryByDigsite.BackgroundStyle.Class = "DateTimeInputBackground";
            this.maxTryByDigsite.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.maxTryByDigsite.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.maxTryByDigsite.ForeColor = System.Drawing.Color.Black;
            this.maxTryByDigsite.Location = new System.Drawing.Point(308, 15);
            this.maxTryByDigsite.MinValue = 1;
            this.maxTryByDigsite.Name = "maxTryByDigsite";
            this.maxTryByDigsite.ShowUpDown = true;
            this.maxTryByDigsite.Size = new System.Drawing.Size(48, 22);
            this.maxTryByDigsite.TabIndex = 5;
            this.maxTryByDigsite.Value = 1;
            // 
            // DigSitesListManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(600, 380);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.maxTryByDigsite);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.solvingEveryXMin);
            this.Controls.Add(this.saveB);
            this.Controls.Add(this.listDigsitesDGV);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DigSitesListManagement";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DigSites list Management";
            ((System.ComponentModel.ISupportInitialize)(this.listDigsitesDGV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.solvingEveryXMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxTryByDigsite)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewX listDigsitesDGV;
        private DevComponents.DotNetBar.ButtonX saveB;
        private DevComponents.Editors.IntegerInput solvingEveryXMin;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.Editors.IntegerInput maxTryByDigsite;
    }
}