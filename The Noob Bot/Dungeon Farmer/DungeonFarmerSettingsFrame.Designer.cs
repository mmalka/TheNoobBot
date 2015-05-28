namespace DungeonFarmer
{
    partial class DungeonFarmerSettingsFrame
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DungeonFarmerSettingsFrame));
            this.SaveAndCloseButton = new System.Windows.Forms.Label();
            this.CancelAndCloseButton = new System.Windows.Forms.Label();
            this.InstanceListTable = new System.Windows.Forms.DataGridView();
            this.DungeonFarmerFormTitle = new nManager.Helpful.Forms.UserControls.TnbControlMenu();
            ((System.ComponentModel.ISupportInitialize)(this.InstanceListTable)).BeginInit();
            this.SuspendLayout();
            // 
            // SaveAndCloseButton
            // 
            this.SaveAndCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.SaveAndCloseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveAndCloseButton.ForeColor = System.Drawing.Color.White;
            this.SaveAndCloseButton.Image = global::DungeonFarmer.Properties.Resources.blueB;
            this.SaveAndCloseButton.Location = new System.Drawing.Point(659, 552);
            this.SaveAndCloseButton.Margin = new System.Windows.Forms.Padding(0);
            this.SaveAndCloseButton.MaximumSize = new System.Drawing.Size(106, 29);
            this.SaveAndCloseButton.MinimumSize = new System.Drawing.Size(106, 29);
            this.SaveAndCloseButton.Name = "SaveAndCloseButton";
            this.SaveAndCloseButton.Size = new System.Drawing.Size(106, 29);
            this.SaveAndCloseButton.TabIndex = 14;
            this.SaveAndCloseButton.Text = "Save && Close";
            this.SaveAndCloseButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.SaveAndCloseButton.Click += new System.EventHandler(this.SaveAndCloseButton_Click);
            this.SaveAndCloseButton.MouseEnter += new System.EventHandler(this.SaveAndCloseButton_MouseEnter);
            this.SaveAndCloseButton.MouseLeave += new System.EventHandler(this.SaveAndCloseButton_MouseLeave);
            // 
            // CancelAndCloseButton
            // 
            this.CancelAndCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.CancelAndCloseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelAndCloseButton.ForeColor = System.Drawing.Color.White;
            this.CancelAndCloseButton.Image = global::DungeonFarmer.Properties.Resources.blackB;
            this.CancelAndCloseButton.Location = new System.Drawing.Point(549, 552);
            this.CancelAndCloseButton.Margin = new System.Windows.Forms.Padding(0);
            this.CancelAndCloseButton.MaximumSize = new System.Drawing.Size(106, 29);
            this.CancelAndCloseButton.MinimumSize = new System.Drawing.Size(106, 29);
            this.CancelAndCloseButton.Name = "CancelAndCloseButton";
            this.CancelAndCloseButton.Size = new System.Drawing.Size(106, 29);
            this.CancelAndCloseButton.TabIndex = 25;
            this.CancelAndCloseButton.Text = "Cancel && Close";
            this.CancelAndCloseButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CancelAndCloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            this.CancelAndCloseButton.MouseEnter += new System.EventHandler(this.CancelAndCloseButton_MouseEnter);
            this.CancelAndCloseButton.MouseLeave += new System.EventHandler(this.CancelAndCloseButton_MouseLeave);
            // 
            // InstanceListTable
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(118)))), ((int)(((byte)(118)))), ((int)(((byte)(118)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(181)))), ((int)(((byte)(22)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.InstanceListTable.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.InstanceListTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.InstanceListTable.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.InstanceListTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.InstanceListTable.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(118)))), ((int)(((byte)(118)))), ((int)(((byte)(118)))));
            this.InstanceListTable.Location = new System.Drawing.Point(35, 69);
            this.InstanceListTable.Margin = new System.Windows.Forms.Padding(0);
            this.InstanceListTable.MaximumSize = new System.Drawing.Size(730, 464);
            this.InstanceListTable.MinimumSize = new System.Drawing.Size(730, 464);
            this.InstanceListTable.Name = "InstanceListTable";
            this.InstanceListTable.Size = new System.Drawing.Size(730, 464);
            this.InstanceListTable.TabIndex = 21;
            this.InstanceListTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DigSitesTable_CellContentClick);
            // 
            // DungeonFarmerFormTitle
            // 
            this.DungeonFarmerFormTitle.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("DungeonFarmerFormTitle.BackgroundImage")));
            this.DungeonFarmerFormTitle.Location = new System.Drawing.Point(0, 0);
            this.DungeonFarmerFormTitle.LogoImage = ((System.Drawing.Image)(resources.GetObject("DungeonFarmerFormTitle.LogoImage")));
            this.DungeonFarmerFormTitle.Name = "DungeonFarmerFormTitle";
            this.DungeonFarmerFormTitle.Size = new System.Drawing.Size(800, 43);
            this.DungeonFarmerFormTitle.TabIndex = 26;
            this.DungeonFarmerFormTitle.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.DungeonFarmerFormTitle.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.DungeonFarmerFormTitle.TitleText = "Dungeon Farmer Settings - TheNoobBot - 3.0.0";
            // 
            // DungeonFarmerSettingsFrame
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.DungeonFarmerFormTitle);
            this.Controls.Add(this.InstanceListTable);
            this.Controls.Add(this.CancelAndCloseButton);
            this.Controls.Add(this.SaveAndCloseButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(800, 600);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "DungeonFarmerSettingsFrame";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Dungeon Farmer Settings";
            ((System.ComponentModel.ISupportInitialize)(this.InstanceListTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label SaveAndCloseButton;
        private System.Windows.Forms.Label CancelAndCloseButton;
        private System.Windows.Forms.DataGridView InstanceListTable;
        private nManager.Helpful.Forms.UserControls.TnbControlMenu DungeonFarmerFormTitle;
    }
}

