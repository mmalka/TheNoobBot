namespace nManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TranslationManagementMainFrame));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.TopLeftLogo = new System.Windows.Forms.PictureBox();
            this.ControlMenu = new System.Windows.Forms.PictureBox();
            this.ReduceButton = new System.Windows.Forms.PictureBox();
            this.LoadButton = new System.Windows.Forms.Label();
            this.CloseButton = new System.Windows.Forms.PictureBox();
            this.SaveButton = new System.Windows.Forms.Label();
            this.QuitButton = new System.Windows.Forms.Label();
            this.TranslateManagerFormTitle = new System.Windows.Forms.Label();
            this.TranslationTable = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.TopLeftLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ControlMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReduceButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CloseButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TranslationTable)).BeginInit();
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
            this.ReduceButton.Image = global::nManager.Properties.Resources.reduce_button;
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
            // CloseButton
            // 
            this.CloseButton.BackColor = System.Drawing.Color.Transparent;
            this.CloseButton.ErrorImage = null;
            this.CloseButton.Image = global::nManager.Properties.Resources.close_button;
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
            // TranslateManagerFormTitle
            // 
            this.TranslateManagerFormTitle.AutoSize = true;
            this.TranslateManagerFormTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.TranslateManagerFormTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TranslateManagerFormTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.TranslateManagerFormTitle.Location = new System.Drawing.Point(57, 4);
            this.TranslateManagerFormTitle.Margin = new System.Windows.Forms.Padding(0);
            this.TranslateManagerFormTitle.MaximumSize = new System.Drawing.Size(675, 35);
            this.TranslateManagerFormTitle.MinimumSize = new System.Drawing.Size(675, 35);
            this.TranslateManagerFormTitle.Name = "TranslateManagerFormTitle";
            this.TranslateManagerFormTitle.Size = new System.Drawing.Size(675, 35);
            this.TranslateManagerFormTitle.TabIndex = 20;
            this.TranslateManagerFormTitle.Text = "Translate Manager - TheNoobBot - 3.0.0";
            this.TranslateManagerFormTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.TranslateManagerFormTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseDown);
            this.TranslateManagerFormTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseMove);
            this.TranslateManagerFormTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseUp);
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
            // TranslationManagementMainFrame
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.TranslationTable);
            this.Controls.Add(this.QuitButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.TopLeftLogo);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.ReduceButton);
            this.Controls.Add(this.LoadButton);
            this.Controls.Add(this.TranslateManagerFormTitle);
            this.Controls.Add(this.ControlMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(800, 600);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "TranslationManagementMainFrame";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Translate Tools";
            ((System.ComponentModel.ISupportInitialize)(this.TopLeftLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ControlMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReduceButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CloseButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TranslationTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        private System.Windows.Forms.PictureBox TopLeftLogo;
        private System.Windows.Forms.PictureBox ControlMenu;
        private System.Windows.Forms.PictureBox ReduceButton;
        private System.Windows.Forms.PictureBox CloseButton;
        private System.Windows.Forms.Label LoadButton;
        private System.Windows.Forms.Label TranslateManagerFormTitle;
        private System.Windows.Forms.Label SaveButton;
        private System.Windows.Forms.Label QuitButton;
        private System.Windows.Forms.DataGridView TranslationTable;
    }
}

