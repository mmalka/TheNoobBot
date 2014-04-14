namespace nManager.Helpful.Forms
{
    partial class TranslationManagementMainFrame
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TranslationManagementMainFrame));
            this.LoadButton = new System.Windows.Forms.Label();
            this.SaveButton = new System.Windows.Forms.Label();
            this.QuitButton = new System.Windows.Forms.Label();
            this.TranslationTable = new System.Windows.Forms.DataGridView();
            this.MainHeader = new nManager.Helpful.Forms.UserControls.TnbControlMenu();
            ((System.ComponentModel.ISupportInitialize)(this.TranslationTable)).BeginInit();
            this.SuspendLayout();
            // 
            // LoadButton
            // 
            this.LoadButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.LoadButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoadButton.ForeColor = System.Drawing.Color.White;
            this.LoadButton.Image = global::nManager.Properties.Resources.blueB_260;
            this.LoadButton.Location = new System.Drawing.Point(507, 54);
            this.LoadButton.Margin = new System.Windows.Forms.Padding(0);
            this.LoadButton.MaximumSize = new System.Drawing.Size(260, 29);
            this.LoadButton.MinimumSize = new System.Drawing.Size(260, 29);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(260, 29);
            this.LoadButton.TabIndex = 13;
            this.LoadButton.Text = "LOAD A TRANSLATION FILE";
            this.LoadButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            this.LoadButton.MouseEnter += new System.EventHandler(this.LoadButton_MouseEnter);
            this.LoadButton.MouseLeave += new System.EventHandler(this.LoadButton_MouseLeave);
            // 
            // SaveButton
            // 
            this.SaveButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.SaveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveButton.ForeColor = System.Drawing.Color.White;
            this.SaveButton.Image = global::nManager.Properties.Resources.blueB_260;
            this.SaveButton.Location = new System.Drawing.Point(507, 559);
            this.SaveButton.Margin = new System.Windows.Forms.Padding(0);
            this.SaveButton.MaximumSize = new System.Drawing.Size(260, 29);
            this.SaveButton.MinimumSize = new System.Drawing.Size(260, 29);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(260, 29);
            this.SaveButton.TabIndex = 14;
            this.SaveButton.Text = "SAVE AND CLOSE";
            this.SaveButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            this.SaveButton.MouseEnter += new System.EventHandler(this.SaveButton_MouseEnter);
            this.SaveButton.MouseLeave += new System.EventHandler(this.SaveButton_MouseLeave);
            // 
            // QuitButton
            // 
            this.QuitButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.QuitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QuitButton.ForeColor = System.Drawing.Color.White;
            this.QuitButton.Image = global::nManager.Properties.Resources.blackB_260;
            this.QuitButton.Location = new System.Drawing.Point(237, 559);
            this.QuitButton.Margin = new System.Windows.Forms.Padding(0);
            this.QuitButton.MaximumSize = new System.Drawing.Size(260, 29);
            this.QuitButton.MinimumSize = new System.Drawing.Size(260, 29);
            this.QuitButton.Name = "QuitButton";
            this.QuitButton.Size = new System.Drawing.Size(260, 29);
            this.QuitButton.TabIndex = 15;
            this.QuitButton.Text = "CLOSE WITHOUT SAVING";
            this.QuitButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.QuitButton.Click += new System.EventHandler(this.CloseButton_Click);
            this.QuitButton.MouseEnter += new System.EventHandler(this.QuitButton_MouseEnter);
            this.QuitButton.MouseLeave += new System.EventHandler(this.QuitButton_MouseLeave);
            // 
            // TranslationTable
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(118)))), ((int)(((byte)(118)))), ((int)(((byte)(118)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(181)))), ((int)(((byte)(22)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.TranslationTable.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.TranslationTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.TranslationTable.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TranslationTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TranslationTable.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(118)))), ((int)(((byte)(118)))), ((int)(((byte)(118)))));
            this.TranslationTable.Location = new System.Drawing.Point(35, 95);
            this.TranslationTable.Name = "TranslationTable";
            this.TranslationTable.Size = new System.Drawing.Size(730, 452);
            this.TranslationTable.TabIndex = 21;
            // 
            // MainHeader
            // 
            this.MainHeader.BackgroundImage = global::nManager.Properties.Resources._800x43_controlbar;
            this.MainHeader.Location = new System.Drawing.Point(0, 0);
            this.MainHeader.LogoImage = ((System.Drawing.Image)(resources.GetObject("MainHeader.LogoImage")));
            this.MainHeader.Name = "MainHeader";
            this.MainHeader.Size = new System.Drawing.Size(800, 43);
            this.MainHeader.TabIndex = 22;
            this.MainHeader.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.MainHeader.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.MainHeader.TitleText = "Translate Manager - TheNoobBot - DevVersionRestrict";
            // 
            // TranslationManagementMainFrame
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.MainHeader);
            this.Controls.Add(this.TranslationTable);
            this.Controls.Add(this.QuitButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.LoadButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(800, 600);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "TranslationManagementMainFrame";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Translate Tools";
            ((System.ComponentModel.ISupportInitialize)(this.TranslationTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label LoadButton;
        private System.Windows.Forms.Label SaveButton;
        private System.Windows.Forms.Label QuitButton;
        private System.Windows.Forms.DataGridView TranslationTable;
        private UserControls.TnbControlMenu MainHeader;
    }
}

