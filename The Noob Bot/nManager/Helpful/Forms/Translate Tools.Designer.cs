namespace nManager.Helpful.Forms
{
    partial class Translate_Tools
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.listDigsitesDGV = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.saveB = new DevComponents.DotNetBar.ButtonX();
            this.loadB = new DevComponents.DotNetBar.ButtonX();
            ((System.ComponentModel.ISupportInitialize)(this.listDigsitesDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // listDigsitesDGV
            // 
            this.listDigsitesDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listDigsitesDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.listDigsitesDGV.DefaultCellStyle = dataGridViewCellStyle2;
            this.listDigsitesDGV.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.listDigsitesDGV.Location = new System.Drawing.Point(12, 43);
            this.listDigsitesDGV.Name = "listDigsitesDGV";
            this.listDigsitesDGV.Size = new System.Drawing.Size(601, 341);
            this.listDigsitesDGV.TabIndex = 0;
            // 
            // saveB
            // 
            this.saveB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.saveB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.saveB.Location = new System.Drawing.Point(449, 14);
            this.saveB.Name = "saveB";
            this.saveB.Size = new System.Drawing.Size(164, 23);
            this.saveB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.saveB.TabIndex = 1;
            this.saveB.Text = "Save";
            this.saveB.Click += new System.EventHandler(this.saveB_Click);
            // 
            // loadB
            // 
            this.loadB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.loadB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.loadB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.loadB.Location = new System.Drawing.Point(279, 14);
            this.loadB.Name = "loadB";
            this.loadB.Size = new System.Drawing.Size(164, 23);
            this.loadB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.loadB.TabIndex = 2;
            this.loadB.Text = "Load";
            this.loadB.Click += new System.EventHandler(this.loadB_Click);
            // 
            // Translate_Tools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 396);
            this.Controls.Add(this.loadB);
            this.Controls.Add(this.saveB);
            this.Controls.Add(this.listDigsitesDGV);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Translate_Tools";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Translate Tools";
            ((System.ComponentModel.ISupportInitialize)(this.listDigsitesDGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewX listDigsitesDGV;
        private DevComponents.DotNetBar.ButtonX saveB;
        private DevComponents.DotNetBar.ButtonX loadB;
    }
}