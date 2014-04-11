namespace Archaeologist
{
    partial class ArchaeologistSettingsFrame
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArchaeologistSettingsFrame));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.TopLeftLogo = new System.Windows.Forms.PictureBox();
            this.ControlMenu = new System.Windows.Forms.PictureBox();
            this.ReduceButton = new System.Windows.Forms.PictureBox();
            this.CloseButton = new System.Windows.Forms.PictureBox();
            this.SaveAndCloseButton = new System.Windows.Forms.Label();
            this.DeveloperToolsFormTitle = new System.Windows.Forms.Label();
            this.CancelAndCloseButton = new System.Windows.Forms.Label();
            this.DigSitesTable = new System.Windows.Forms.DataGridView();
            this.UseKeystones = new System.Windows.Forms.CheckBox();
            this.SolvingEveryXMinLabel = new System.Windows.Forms.Label();
            this.MaxTryByDigsiteLabel = new System.Windows.Forms.Label();
            this.SolvingEveryXMin = new System.Windows.Forms.NumericUpDown();
            this.MaxTryByDigsite = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.TopLeftLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ControlMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReduceButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CloseButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DigSitesTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SolvingEveryXMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxTryByDigsite)).BeginInit();
            this.SuspendLayout();
            // 
            // TopLeftLogo
            // 
            this.TopLeftLogo.ErrorImage = null;
            this.TopLeftLogo.Image = ((System.Drawing.Image)(resources.GetObject("TopLeftLogo.Image")));
            this.TopLeftLogo.InitialImage = null;
            this.TopLeftLogo.Location = new System.Drawing.Point(13, 3);
            this.TopLeftLogo.Margin = new System.Windows.Forms.Padding(0);
            this.TopLeftLogo.Name = "TopLeftLogo";
            this.TopLeftLogo.Size = new System.Drawing.Size(30, 33);
            this.TopLeftLogo.TabIndex = 4;
            this.TopLeftLogo.TabStop = false;
            this.TopLeftLogo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseDown);
            this.TopLeftLogo.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseMove);
            this.TopLeftLogo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseUp);
            // 
            // ControlMenu
            // 
            this.ControlMenu.BackColor = System.Drawing.Color.Black;
            this.ControlMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.ControlMenu.ErrorImage = null;
            this.ControlMenu.Image = ((System.Drawing.Image)(resources.GetObject("ControlMenu.Image")));
            this.ControlMenu.InitialImage = null;
            this.ControlMenu.Location = new System.Drawing.Point(0, 0);
            this.ControlMenu.Margin = new System.Windows.Forms.Padding(0);
            this.ControlMenu.MaximumSize = new System.Drawing.Size(800, 43);
            this.ControlMenu.MinimumSize = new System.Drawing.Size(800, 43);
            this.ControlMenu.Name = "ControlMenu";
            this.ControlMenu.Size = new System.Drawing.Size(800, 43);
            this.ControlMenu.TabIndex = 1;
            this.ControlMenu.TabStop = false;
            this.ControlMenu.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseDown);
            this.ControlMenu.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseMove);
            this.ControlMenu.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseUp);
            // 
            // ReduceButton
            // 
            this.ReduceButton.BackColor = System.Drawing.Color.Transparent;
            this.ReduceButton.ErrorImage = null;
            this.ReduceButton.Image = global::Archaeologist.Properties.Resources.reduce_button;
            this.ReduceButton.InitialImage = null;
            this.ReduceButton.Location = new System.Drawing.Point(748, 13);
            this.ReduceButton.Margin = new System.Windows.Forms.Padding(0);
            this.ReduceButton.MaximumSize = new System.Drawing.Size(13, 14);
            this.ReduceButton.MinimumSize = new System.Drawing.Size(13, 14);
            this.ReduceButton.Name = "ReduceButton";
            this.ReduceButton.Size = new System.Drawing.Size(13, 14);
            this.ReduceButton.TabIndex = 1;
            this.ReduceButton.TabStop = false;
            this.ReduceButton.Click += new System.EventHandler(this.ReduceButton_Click);
            this.ReduceButton.MouseEnter += new System.EventHandler(this.ReduceButton_MouseEnter);
            this.ReduceButton.MouseLeave += new System.EventHandler(this.ReduceButton_MouseLeave);
            // 
            // CloseButton
            // 
            this.CloseButton.BackColor = System.Drawing.Color.Transparent;
            this.CloseButton.ErrorImage = null;
            this.CloseButton.Image = global::Archaeologist.Properties.Resources.close_button;
            this.CloseButton.InitialImage = null;
            this.CloseButton.Location = new System.Drawing.Point(775, 13);
            this.CloseButton.Margin = new System.Windows.Forms.Padding(0);
            this.CloseButton.MaximumSize = new System.Drawing.Size(13, 14);
            this.CloseButton.MinimumSize = new System.Drawing.Size(13, 14);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(13, 14);
            this.CloseButton.TabIndex = 2;
            this.CloseButton.TabStop = false;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            this.CloseButton.MouseEnter += new System.EventHandler(this.CloseButton_MouseEnter);
            this.CloseButton.MouseLeave += new System.EventHandler(this.CloseButton_MouseLeave);
            // 
            // SaveAndCloseButton
            // 
            this.SaveAndCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.SaveAndCloseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveAndCloseButton.ForeColor = System.Drawing.Color.White;
            this.SaveAndCloseButton.Image = global::Archaeologist.Properties.Resources.blueB;
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
            // DeveloperToolsFormTitle
            // 
            this.DeveloperToolsFormTitle.AutoSize = true;
            this.DeveloperToolsFormTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.DeveloperToolsFormTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeveloperToolsFormTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.DeveloperToolsFormTitle.Location = new System.Drawing.Point(57, 4);
            this.DeveloperToolsFormTitle.Margin = new System.Windows.Forms.Padding(0);
            this.DeveloperToolsFormTitle.MaximumSize = new System.Drawing.Size(675, 35);
            this.DeveloperToolsFormTitle.MinimumSize = new System.Drawing.Size(675, 35);
            this.DeveloperToolsFormTitle.Name = "DeveloperToolsFormTitle";
            this.DeveloperToolsFormTitle.Size = new System.Drawing.Size(675, 35);
            this.DeveloperToolsFormTitle.TabIndex = 20;
            this.DeveloperToolsFormTitle.Text = "Developer Tools - TheNoobBot - 3.0.0";
            this.DeveloperToolsFormTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.DeveloperToolsFormTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseDown);
            this.DeveloperToolsFormTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseMove);
            this.DeveloperToolsFormTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseUp);
            // 
            // CancelAndCloseButton
            // 
            this.CancelAndCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.CancelAndCloseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelAndCloseButton.ForeColor = System.Drawing.Color.White;
            this.CancelAndCloseButton.Image = global::Archaeologist.Properties.Resources.blackB;
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
            // DigSitesTable
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(118)))), ((int)(((byte)(118)))), ((int)(((byte)(118)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(181)))), ((int)(((byte)(22)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.DigSitesTable.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DigSitesTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.DigSitesTable.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DigSitesTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DigSitesTable.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(118)))), ((int)(((byte)(118)))), ((int)(((byte)(118)))));
            this.DigSitesTable.Location = new System.Drawing.Point(35, 69);
            this.DigSitesTable.Margin = new System.Windows.Forms.Padding(0);
            this.DigSitesTable.MaximumSize = new System.Drawing.Size(730, 464);
            this.DigSitesTable.MinimumSize = new System.Drawing.Size(730, 464);
            this.DigSitesTable.Name = "DigSitesTable";
            this.DigSitesTable.Size = new System.Drawing.Size(730, 464);
            this.DigSitesTable.TabIndex = 21;
            this.DigSitesTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DigSitesTable_CellContentClick);
            // 
            // UseKeystones
            // 
            this.UseKeystones.BackColor = System.Drawing.Color.Transparent;
            this.UseKeystones.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UseKeystones.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(160)))), ((int)(((byte)(229)))));
            this.UseKeystones.Location = new System.Drawing.Point(35, 552);
            this.UseKeystones.Name = "UseKeystones";
            this.UseKeystones.Size = new System.Drawing.Size(115, 29);
            this.UseKeystones.TabIndex = 26;
            this.UseKeystones.Text = "Use Keystones";
            this.UseKeystones.UseVisualStyleBackColor = false;
            // 
            // SolvingEveryXMinLabel
            // 
            this.SolvingEveryXMinLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.SolvingEveryXMinLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.SolvingEveryXMinLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(160)))), ((int)(((byte)(229)))));
            this.SolvingEveryXMinLabel.Location = new System.Drawing.Point(161, 552);
            this.SolvingEveryXMinLabel.Name = "SolvingEveryXMinLabel";
            this.SolvingEveryXMinLabel.Size = new System.Drawing.Size(142, 29);
            this.SolvingEveryXMinLabel.TabIndex = 27;
            this.SolvingEveryXMinLabel.Text = "Solving Every X Min:";
            this.SolvingEveryXMinLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MaxTryByDigsiteLabel
            // 
            this.MaxTryByDigsiteLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MaxTryByDigsiteLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.MaxTryByDigsiteLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.MaxTryByDigsiteLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(160)))), ((int)(((byte)(229)))));
            this.MaxTryByDigsiteLabel.Location = new System.Drawing.Point(361, 552);
            this.MaxTryByDigsiteLabel.Name = "MaxTryByDigsiteLabel";
            this.MaxTryByDigsiteLabel.Size = new System.Drawing.Size(130, 29);
            this.MaxTryByDigsiteLabel.TabIndex = 28;
            this.MaxTryByDigsiteLabel.Text = "Max Try By Digsite:";
            this.MaxTryByDigsiteLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SolvingEveryXMin
            // 
            this.SolvingEveryXMin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.SolvingEveryXMin.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SolvingEveryXMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.SolvingEveryXMin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(160)))), ((int)(((byte)(229)))));
            this.SolvingEveryXMin.Location = new System.Drawing.Point(306, 560);
            this.SolvingEveryXMin.Margin = new System.Windows.Forms.Padding(0);
            this.SolvingEveryXMin.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.SolvingEveryXMin.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.SolvingEveryXMin.Name = "SolvingEveryXMin";
            this.SolvingEveryXMin.Size = new System.Drawing.Size(38, 16);
            this.SolvingEveryXMin.TabIndex = 29;
            this.SolvingEveryXMin.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // MaxTryByDigsite
            // 
            this.MaxTryByDigsite.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.MaxTryByDigsite.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MaxTryByDigsite.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.MaxTryByDigsite.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(160)))), ((int)(((byte)(229)))));
            this.MaxTryByDigsite.Location = new System.Drawing.Point(494, 560);
            this.MaxTryByDigsite.Margin = new System.Windows.Forms.Padding(0);
            this.MaxTryByDigsite.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.MaxTryByDigsite.Name = "MaxTryByDigsite";
            this.MaxTryByDigsite.Size = new System.Drawing.Size(38, 16);
            this.MaxTryByDigsite.TabIndex = 30;
            this.MaxTryByDigsite.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.MaxTryByDigsite.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // ArchaeologistSettingsFrame
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.MaxTryByDigsite);
            this.Controls.Add(this.SolvingEveryXMin);
            this.Controls.Add(this.MaxTryByDigsiteLabel);
            this.Controls.Add(this.SolvingEveryXMinLabel);
            this.Controls.Add(this.UseKeystones);
            this.Controls.Add(this.DigSitesTable);
            this.Controls.Add(this.CancelAndCloseButton);
            this.Controls.Add(this.SaveAndCloseButton);
            this.Controls.Add(this.TopLeftLogo);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.ReduceButton);
            this.Controls.Add(this.DeveloperToolsFormTitle);
            this.Controls.Add(this.ControlMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(800, 600);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "ArchaeologistSettingsFrame";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Developer Tools";
            ((System.ComponentModel.ISupportInitialize)(this.TopLeftLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ControlMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReduceButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CloseButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DigSitesTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SolvingEveryXMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxTryByDigsite)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        private System.Windows.Forms.PictureBox TopLeftLogo;
        private System.Windows.Forms.PictureBox ControlMenu;
        private System.Windows.Forms.PictureBox ReduceButton;
        private System.Windows.Forms.PictureBox CloseButton;
        private System.Windows.Forms.Label DeveloperToolsFormTitle;
        private System.Windows.Forms.Label SaveAndCloseButton;
        private System.Windows.Forms.Label CancelAndCloseButton;
        private System.Windows.Forms.DataGridView DigSitesTable;
        private System.Windows.Forms.CheckBox UseKeystones;
        private System.Windows.Forms.Label SolvingEveryXMinLabel;
        private System.Windows.Forms.Label MaxTryByDigsiteLabel;
        private System.Windows.Forms.NumericUpDown SolvingEveryXMin;
        private System.Windows.Forms.NumericUpDown MaxTryByDigsite;
    }
}

