namespace Quester.Profile
{
    partial class QuesterProfileLoader
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuesterProfileLoader));
            this.ControlMenu = new System.Windows.Forms.PictureBox();
            this.ReduceButton = new System.Windows.Forms.PictureBox();
            this.CloseButton = new System.Windows.Forms.PictureBox();
            this.SimpleProfilesList = new System.Windows.Forms.ListBox();
            this.TopLeftLogo = new System.Windows.Forms.PictureBox();
            this.GroupedProfilesListLabel = new System.Windows.Forms.Label();
            this.QuesterProfileLoaderFormTitle = new System.Windows.Forms.Label();
            this.LoadGroupedProfile = new System.Windows.Forms.Label();
            this.QuesterProfileManagementSystemButton = new System.Windows.Forms.Label();
            this.SimpleProfilesListLabel = new System.Windows.Forms.Label();
            this.GroupedProfilesList = new System.Windows.Forms.ListBox();
            this.LoadSimpleProfile = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ControlMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReduceButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CloseButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TopLeftLogo)).BeginInit();
            this.SuspendLayout();
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
            this.ControlMenu.MaximumSize = new System.Drawing.Size(575, 43);
            this.ControlMenu.MinimumSize = new System.Drawing.Size(575, 43);
            this.ControlMenu.Name = "ControlMenu";
            this.ControlMenu.Size = new System.Drawing.Size(575, 43);
            this.ControlMenu.TabIndex = 0;
            this.ControlMenu.TabStop = false;
            this.ControlMenu.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseDown);
            this.ControlMenu.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseMove);
            this.ControlMenu.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseUp);
            // 
            // ReduceButton
            // 
            this.ReduceButton.BackColor = System.Drawing.Color.Transparent;
            this.ReduceButton.ErrorImage = null;
            this.ReduceButton.Image = global::Quester.Properties.Resources.reduce_button;
            this.ReduceButton.InitialImage = null;
            this.ReduceButton.Location = new System.Drawing.Point(523, 13);
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
            this.CloseButton.Image = global::Quester.Properties.Resources.close_button;
            this.CloseButton.InitialImage = null;
            this.CloseButton.Location = new System.Drawing.Point(550, 13);
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
            // SimpleProfilesList
            // 
            this.SimpleProfilesList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.SimpleProfilesList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SimpleProfilesList.FormattingEnabled = true;
            this.SimpleProfilesList.IntegralHeight = false;
            this.SimpleProfilesList.Location = new System.Drawing.Point(30, 157);
            this.SimpleProfilesList.Margin = new System.Windows.Forms.Padding(0);
            this.SimpleProfilesList.MaximumSize = new System.Drawing.Size(238, 234);
            this.SimpleProfilesList.MinimumSize = new System.Drawing.Size(238, 234);
            this.SimpleProfilesList.Name = "SimpleProfilesList";
            this.SimpleProfilesList.Size = new System.Drawing.Size(238, 234);
            this.SimpleProfilesList.TabIndex = 3;
            this.SimpleProfilesList.DoubleClick += new System.EventHandler(this.DoLoadSimpleProfile);
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
            // GroupedProfilesListLabel
            // 
            this.GroupedProfilesListLabel.AutoSize = true;
            this.GroupedProfilesListLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.GroupedProfilesListLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupedProfilesListLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(160)))), ((int)(((byte)(229)))));
            this.GroupedProfilesListLabel.Location = new System.Drawing.Point(304, 124);
            this.GroupedProfilesListLabel.Margin = new System.Windows.Forms.Padding(0);
            this.GroupedProfilesListLabel.MaximumSize = new System.Drawing.Size(234, 29);
            this.GroupedProfilesListLabel.MinimumSize = new System.Drawing.Size(234, 29);
            this.GroupedProfilesListLabel.Name = "GroupedProfilesListLabel";
            this.GroupedProfilesListLabel.Size = new System.Drawing.Size(234, 29);
            this.GroupedProfilesListLabel.TabIndex = 11;
            this.GroupedProfilesListLabel.Text = "GROUPED PROFILE LIST";
            this.GroupedProfilesListLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // QuesterProfileLoaderFormTitle
            // 
            this.QuesterProfileLoaderFormTitle.AutoSize = true;
            this.QuesterProfileLoaderFormTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.QuesterProfileLoaderFormTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QuesterProfileLoaderFormTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.QuesterProfileLoaderFormTitle.Location = new System.Drawing.Point(57, 4);
            this.QuesterProfileLoaderFormTitle.Margin = new System.Windows.Forms.Padding(0);
            this.QuesterProfileLoaderFormTitle.MaximumSize = new System.Drawing.Size(450, 35);
            this.QuesterProfileLoaderFormTitle.MinimumSize = new System.Drawing.Size(450, 35);
            this.QuesterProfileLoaderFormTitle.Name = "QuesterProfileLoaderFormTitle";
            this.QuesterProfileLoaderFormTitle.Size = new System.Drawing.Size(450, 35);
            this.QuesterProfileLoaderFormTitle.TabIndex = 12;
            this.QuesterProfileLoaderFormTitle.Text = "TheNoobBot";
            this.QuesterProfileLoaderFormTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.QuesterProfileLoaderFormTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseDown);
            this.QuesterProfileLoaderFormTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseMove);
            this.QuesterProfileLoaderFormTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseUp);
            // 
            // LoadGroupedProfile
            // 
            this.LoadGroupedProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoadGroupedProfile.ForeColor = System.Drawing.Color.White;
            this.LoadGroupedProfile.Image = global::Quester.Properties.Resources.blueB_242;
            this.LoadGroupedProfile.Location = new System.Drawing.Point(299, 414);
            this.LoadGroupedProfile.Margin = new System.Windows.Forms.Padding(0);
            this.LoadGroupedProfile.MaximumSize = new System.Drawing.Size(242, 29);
            this.LoadGroupedProfile.MinimumSize = new System.Drawing.Size(242, 29);
            this.LoadGroupedProfile.Name = "LoadGroupedProfile";
            this.LoadGroupedProfile.Size = new System.Drawing.Size(242, 29);
            this.LoadGroupedProfile.TabIndex = 13;
            this.LoadGroupedProfile.Text = "Load a Grouped Profile";
            this.LoadGroupedProfile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LoadGroupedProfile.Click += new System.EventHandler(this.DoLoadGroupedProfile);
            this.LoadGroupedProfile.MouseEnter += new System.EventHandler(this.LoadGroupedProfile_MouseEnter);
            this.LoadGroupedProfile.MouseLeave += new System.EventHandler(this.LoadGroupedProfile_MouseLeave);
            // 
            // QuesterProfileManagementSystemButton
            // 
            this.QuesterProfileManagementSystemButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.QuesterProfileManagementSystemButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QuesterProfileManagementSystemButton.ForeColor = System.Drawing.Color.White;
            this.QuesterProfileManagementSystemButton.Image = global::Quester.Properties.Resources.blueB_260;
            this.QuesterProfileManagementSystemButton.Location = new System.Drawing.Point(281, 67);
            this.QuesterProfileManagementSystemButton.Margin = new System.Windows.Forms.Padding(0);
            this.QuesterProfileManagementSystemButton.MaximumSize = new System.Drawing.Size(260, 29);
            this.QuesterProfileManagementSystemButton.MinimumSize = new System.Drawing.Size(260, 29);
            this.QuesterProfileManagementSystemButton.Name = "QuesterProfileManagementSystemButton";
            this.QuesterProfileManagementSystemButton.Size = new System.Drawing.Size(260, 29);
            this.QuesterProfileManagementSystemButton.TabIndex = 14;
            this.QuesterProfileManagementSystemButton.Text = "Quester Profile Management System";
            this.QuesterProfileManagementSystemButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.QuesterProfileManagementSystemButton.Click += new System.EventHandler(this.QuesterProfileManagementSystemButton_Click);
            this.QuesterProfileManagementSystemButton.MouseEnter += new System.EventHandler(this.QuesterProfileManagementSystemButton_MouseEnter);
            this.QuesterProfileManagementSystemButton.MouseLeave += new System.EventHandler(this.QuesterProfileManagementSystemButton_MouseLeave);
            // 
            // SimpleProfilesListLabel
            // 
            this.SimpleProfilesListLabel.AutoSize = true;
            this.SimpleProfilesListLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.SimpleProfilesListLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SimpleProfilesListLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(160)))), ((int)(((byte)(229)))));
            this.SimpleProfilesListLabel.Location = new System.Drawing.Point(32, 124);
            this.SimpleProfilesListLabel.Margin = new System.Windows.Forms.Padding(0);
            this.SimpleProfilesListLabel.MaximumSize = new System.Drawing.Size(234, 29);
            this.SimpleProfilesListLabel.MinimumSize = new System.Drawing.Size(234, 29);
            this.SimpleProfilesListLabel.Name = "SimpleProfilesListLabel";
            this.SimpleProfilesListLabel.Size = new System.Drawing.Size(234, 29);
            this.SimpleProfilesListLabel.TabIndex = 15;
            this.SimpleProfilesListLabel.Text = "SIMPLE PROFILE LIST";
            this.SimpleProfilesListLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GroupedProfilesList
            // 
            this.GroupedProfilesList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.GroupedProfilesList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.GroupedProfilesList.FormattingEnabled = true;
            this.GroupedProfilesList.IntegralHeight = false;
            this.GroupedProfilesList.Location = new System.Drawing.Point(302, 157);
            this.GroupedProfilesList.Margin = new System.Windows.Forms.Padding(0);
            this.GroupedProfilesList.MaximumSize = new System.Drawing.Size(238, 234);
            this.GroupedProfilesList.MinimumSize = new System.Drawing.Size(238, 234);
            this.GroupedProfilesList.Name = "GroupedProfilesList";
            this.GroupedProfilesList.Size = new System.Drawing.Size(238, 234);
            this.GroupedProfilesList.TabIndex = 16;
            this.GroupedProfilesList.DoubleClick += new System.EventHandler(this.DoLoadGroupedProfile);
            // 
            // LoadSimpleProfile
            // 
            this.LoadSimpleProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoadSimpleProfile.ForeColor = System.Drawing.Color.White;
            this.LoadSimpleProfile.Image = global::Quester.Properties.Resources.blueB_242;
            this.LoadSimpleProfile.Location = new System.Drawing.Point(27, 414);
            this.LoadSimpleProfile.Margin = new System.Windows.Forms.Padding(0);
            this.LoadSimpleProfile.MaximumSize = new System.Drawing.Size(242, 29);
            this.LoadSimpleProfile.MinimumSize = new System.Drawing.Size(242, 29);
            this.LoadSimpleProfile.Name = "LoadSimpleProfile";
            this.LoadSimpleProfile.Size = new System.Drawing.Size(242, 29);
            this.LoadSimpleProfile.TabIndex = 17;
            this.LoadSimpleProfile.Text = "Load a Simple Profile";
            this.LoadSimpleProfile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LoadSimpleProfile.Click += new System.EventHandler(this.DoLoadSimpleProfile);
            this.LoadSimpleProfile.MouseEnter += new System.EventHandler(this.LoadSimpleProfile_MouseEnter);
            this.LoadSimpleProfile.MouseLeave += new System.EventHandler(this.LoadSimpleProfile_MouseLeave);
            // 
            // QuesterProfileLoader
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(575, 475);
            this.Controls.Add(this.LoadSimpleProfile);
            this.Controls.Add(this.GroupedProfilesList);
            this.Controls.Add(this.SimpleProfilesListLabel);
            this.Controls.Add(this.QuesterProfileManagementSystemButton);
            this.Controls.Add(this.LoadGroupedProfile);
            this.Controls.Add(this.QuesterProfileLoaderFormTitle);
            this.Controls.Add(this.GroupedProfilesListLabel);
            this.Controls.Add(this.TopLeftLogo);
            this.Controls.Add(this.SimpleProfilesList);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.ReduceButton);
            this.Controls.Add(this.ControlMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(575, 475);
            this.MinimumSize = new System.Drawing.Size(575, 475);
            this.Name = "QuesterProfileLoader";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QuesterProfileLoader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.QuesterProfileLoader_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.ControlMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReduceButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CloseButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TopLeftLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox ControlMenu;
        private System.Windows.Forms.PictureBox ReduceButton;
        private System.Windows.Forms.PictureBox CloseButton;
        private System.Windows.Forms.ListBox SimpleProfilesList;
        private System.Windows.Forms.PictureBox TopLeftLogo;
        private System.Windows.Forms.Label GroupedProfilesListLabel;
        private System.Windows.Forms.Label QuesterProfileLoaderFormTitle;
        private System.Windows.Forms.Label LoadGroupedProfile;
        private System.Windows.Forms.Label QuesterProfileManagementSystemButton;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label SimpleProfilesListLabel;
        private System.Windows.Forms.ListBox GroupedProfilesList;
        private System.Windows.Forms.Label LoadSimpleProfile;
    }
}

