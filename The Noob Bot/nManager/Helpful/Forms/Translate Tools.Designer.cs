using System.Drawing;

namespace nManager.Helpful.Forms
{
    partial class TranslationManagementMainFrame
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TranslationManagementMainFrame));
            this.TranslationTable = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.SaveAsButton = new DevComponents.DotNetBar.ButtonX();
            this.LoadAsButton = new DevComponents.DotNetBar.ButtonX();
            ((System.ComponentModel.ISupportInitialize)(this.TranslationTable)).BeginInit();
            this.SuspendLayout();
            // 
            // TranslationTable
            // 
            this.TranslationTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TranslationTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.TranslationTable.DefaultCellStyle = dataGridViewCellStyle1;
            this.TranslationTable.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.TranslationTable.Location = new System.Drawing.Point(0, 40);
            this.TranslationTable.Margin = new System.Windows.Forms.Padding(0);
            this.TranslationTable.Name = "TranslationTable";
            this.TranslationTable.Size = new System.Drawing.Size(784, 520);
            this.TranslationTable.TabIndex = 0;
            // 
            // SaveAsButton
            // 
            this.SaveAsButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.SaveAsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveAsButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.SaveAsButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.SaveAsButton.Location = new System.Drawing.Point(394, 4);
            this.SaveAsButton.Margin = new System.Windows.Forms.Padding(0);
            this.SaveAsButton.Name = "SaveAsButton";
            this.SaveAsButton.Size = new System.Drawing.Size(390, 30);
            this.SaveAsButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.SaveAsButton.TabIndex = 1;
            this.SaveAsButton.Text = "Save";
            this.SaveAsButton.Click += new System.EventHandler(this.SaveAsButton_Click);
            // 
            // LoadAsButton
            // 
            this.LoadAsButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.LoadAsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadAsButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.LoadAsButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.LoadAsButton.Location = new System.Drawing.Point(0, 4);
            this.LoadAsButton.Margin = new System.Windows.Forms.Padding(0);
            this.LoadAsButton.Name = "LoadAsButton";
            this.LoadAsButton.Size = new System.Drawing.Size(390, 30);
            this.LoadAsButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.LoadAsButton.TabIndex = 2;
            this.LoadAsButton.Text = "Load";
            this.LoadAsButton.Click += new System.EventHandler(this.LoadAsButton_Click);
            // 
            // TranslationManagementMainFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.LoadAsButton);
            this.Controls.Add(this.SaveAsButton);
            this.Controls.Add(this.TranslationTable);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "TranslationManagementMainFrame";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TitleText = "Translation Management System";
            ((System.ComponentModel.ISupportInitialize)(this.TranslationTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewX TranslationTable;
        private DevComponents.DotNetBar.ButtonX SaveAsButton;
        private DevComponents.DotNetBar.ButtonX LoadAsButton;
    }
}