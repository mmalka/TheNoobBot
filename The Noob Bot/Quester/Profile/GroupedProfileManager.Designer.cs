namespace Quester.Profile
{
    partial class GroupedProfileManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GroupedProfileManager));
            this.ControlMenu = new System.Windows.Forms.PictureBox();
            this.ReduceButton = new System.Windows.Forms.PictureBox();
            this.CloseButton = new System.Windows.Forms.PictureBox();
            this.SimpleProfilesList = new System.Windows.Forms.ListBox();
            this.TopLeftLogo = new System.Windows.Forms.PictureBox();
            this.CurrentlyGroupedProfilesLabel = new System.Windows.Forms.Label();
            this.GroupedProfileManagerFormTitle = new System.Windows.Forms.Label();
            this.SaveGroupedProfileAs = new System.Windows.Forms.Label();
            this.MoveDownButton = new System.Windows.Forms.Label();
            this.AvailableSimpleProfilesLabel = new System.Windows.Forms.Label();
            this.GroupedProfilesList = new System.Windows.Forms.ListBox();
            this.SaveGroupedProfile = new System.Windows.Forms.Label();
            this.CancelGroupedProfileEdition = new System.Windows.Forms.Label();
            this.MoveUpButton = new System.Windows.Forms.Label();
            this.GroupSelectedProfile = new System.Windows.Forms.Button();
            this.UngroupSelectedProfile = new System.Windows.Forms.Button();
            this.UngroupAllProfiles = new System.Windows.Forms.Button();
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
            this.SimpleProfilesList.Location = new System.Drawing.Point(30, 104);
            this.SimpleProfilesList.Margin = new System.Windows.Forms.Padding(0);
            this.SimpleProfilesList.MaximumSize = new System.Drawing.Size(238, 275);
            this.SimpleProfilesList.MinimumSize = new System.Drawing.Size(238, 275);
            this.SimpleProfilesList.Name = "SimpleProfilesList";
            this.SimpleProfilesList.Size = new System.Drawing.Size(238, 275);
            this.SimpleProfilesList.TabIndex = 3;
            this.SimpleProfilesList.DoubleClick += new System.EventHandler(this.DoGroupSelectedProfile);
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
            // CurrentlyGroupedProfilesLabel
            // 
            this.CurrentlyGroupedProfilesLabel.AutoSize = true;
            this.CurrentlyGroupedProfilesLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.CurrentlyGroupedProfilesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurrentlyGroupedProfilesLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(160)))), ((int)(((byte)(229)))));
            this.CurrentlyGroupedProfilesLabel.Location = new System.Drawing.Point(304, 71);
            this.CurrentlyGroupedProfilesLabel.Margin = new System.Windows.Forms.Padding(0);
            this.CurrentlyGroupedProfilesLabel.MaximumSize = new System.Drawing.Size(234, 29);
            this.CurrentlyGroupedProfilesLabel.MinimumSize = new System.Drawing.Size(234, 29);
            this.CurrentlyGroupedProfilesLabel.Name = "CurrentlyGroupedProfilesLabel";
            this.CurrentlyGroupedProfilesLabel.Size = new System.Drawing.Size(234, 29);
            this.CurrentlyGroupedProfilesLabel.TabIndex = 11;
            this.CurrentlyGroupedProfilesLabel.Text = "CURRENTLY GROUPED PROFILES:";
            this.CurrentlyGroupedProfilesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GroupedProfileManagerFormTitle
            // 
            this.GroupedProfileManagerFormTitle.AutoSize = true;
            this.GroupedProfileManagerFormTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.GroupedProfileManagerFormTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupedProfileManagerFormTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.GroupedProfileManagerFormTitle.Location = new System.Drawing.Point(57, 4);
            this.GroupedProfileManagerFormTitle.Margin = new System.Windows.Forms.Padding(0);
            this.GroupedProfileManagerFormTitle.MaximumSize = new System.Drawing.Size(450, 35);
            this.GroupedProfileManagerFormTitle.MinimumSize = new System.Drawing.Size(450, 35);
            this.GroupedProfileManagerFormTitle.Name = "GroupedProfileManagerFormTitle";
            this.GroupedProfileManagerFormTitle.Size = new System.Drawing.Size(450, 35);
            this.GroupedProfileManagerFormTitle.TabIndex = 12;
            this.GroupedProfileManagerFormTitle.Text = "Grouped Profile Manager - The Noob Bot 3.0.0";
            this.GroupedProfileManagerFormTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.GroupedProfileManagerFormTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseDown);
            this.GroupedProfileManagerFormTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseMove);
            this.GroupedProfileManagerFormTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseUp);
            // 
            // SaveGroupedProfileAs
            // 
            this.SaveGroupedProfileAs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.SaveGroupedProfileAs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveGroupedProfileAs.ForeColor = System.Drawing.Color.White;
            this.SaveGroupedProfileAs.Image = global::Quester.Properties.Resources.blueB_150;
            this.SaveGroupedProfileAs.Location = new System.Drawing.Point(269, 434);
            this.SaveGroupedProfileAs.Margin = new System.Windows.Forms.Padding(0);
            this.SaveGroupedProfileAs.MaximumSize = new System.Drawing.Size(150, 29);
            this.SaveGroupedProfileAs.MinimumSize = new System.Drawing.Size(150, 29);
            this.SaveGroupedProfileAs.Name = "SaveGroupedProfileAs";
            this.SaveGroupedProfileAs.Size = new System.Drawing.Size(150, 29);
            this.SaveGroupedProfileAs.TabIndex = 13;
            this.SaveGroupedProfileAs.Text = "Save as new && Close";
            this.SaveGroupedProfileAs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.SaveGroupedProfileAs.Click += new System.EventHandler(this.SaveGroupedProfileAs_Click);
            this.SaveGroupedProfileAs.MouseEnter += new System.EventHandler(this.SaveGroupedProfileAs_MouseEnter);
            this.SaveGroupedProfileAs.MouseLeave += new System.EventHandler(this.SaveGroupedProfileAs_MouseLeave);
            // 
            // MoveDownButton
            // 
            this.MoveDownButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.MoveDownButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MoveDownButton.ForeColor = System.Drawing.Color.White;
            this.MoveDownButton.Image = global::Quester.Properties.Resources.blackB;
            this.MoveDownButton.Location = new System.Drawing.Point(434, 393);
            this.MoveDownButton.Margin = new System.Windows.Forms.Padding(0);
            this.MoveDownButton.MaximumSize = new System.Drawing.Size(106, 29);
            this.MoveDownButton.MinimumSize = new System.Drawing.Size(106, 29);
            this.MoveDownButton.Name = "MoveDownButton";
            this.MoveDownButton.Size = new System.Drawing.Size(106, 29);
            this.MoveDownButton.TabIndex = 14;
            this.MoveDownButton.Text = "Move Down";
            this.MoveDownButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.MoveDownButton.Click += new System.EventHandler(this.MoveDownButton_Click);
            this.MoveDownButton.MouseEnter += new System.EventHandler(this.MoveDownButton_MouseEnter);
            this.MoveDownButton.MouseLeave += new System.EventHandler(this.MoveDownButton_MouseLeave);
            // 
            // AvailableSimpleProfilesLabel
            // 
            this.AvailableSimpleProfilesLabel.AutoSize = true;
            this.AvailableSimpleProfilesLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.AvailableSimpleProfilesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AvailableSimpleProfilesLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(160)))), ((int)(((byte)(229)))));
            this.AvailableSimpleProfilesLabel.Location = new System.Drawing.Point(32, 71);
            this.AvailableSimpleProfilesLabel.Margin = new System.Windows.Forms.Padding(0);
            this.AvailableSimpleProfilesLabel.MaximumSize = new System.Drawing.Size(234, 29);
            this.AvailableSimpleProfilesLabel.MinimumSize = new System.Drawing.Size(234, 29);
            this.AvailableSimpleProfilesLabel.Name = "AvailableSimpleProfilesLabel";
            this.AvailableSimpleProfilesLabel.Size = new System.Drawing.Size(234, 29);
            this.AvailableSimpleProfilesLabel.TabIndex = 15;
            this.AvailableSimpleProfilesLabel.Text = "AVAILABLE PROFILES:";
            this.AvailableSimpleProfilesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GroupedProfilesList
            // 
            this.GroupedProfilesList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.GroupedProfilesList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.GroupedProfilesList.FormattingEnabled = true;
            this.GroupedProfilesList.IntegralHeight = false;
            this.GroupedProfilesList.Location = new System.Drawing.Point(302, 104);
            this.GroupedProfilesList.Margin = new System.Windows.Forms.Padding(0);
            this.GroupedProfilesList.MaximumSize = new System.Drawing.Size(238, 275);
            this.GroupedProfilesList.MinimumSize = new System.Drawing.Size(238, 275);
            this.GroupedProfilesList.Name = "GroupedProfilesList";
            this.GroupedProfilesList.Size = new System.Drawing.Size(238, 275);
            this.GroupedProfilesList.TabIndex = 16;
            this.GroupedProfilesList.DoubleClick += new System.EventHandler(this.DoUngroupSelectedProfile);
            // 
            // SaveGroupedProfile
            // 
            this.SaveGroupedProfile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.SaveGroupedProfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveGroupedProfile.ForeColor = System.Drawing.Color.White;
            this.SaveGroupedProfile.Image = global::Quester.Properties.Resources.blueB;
            this.SaveGroupedProfile.Location = new System.Drawing.Point(434, 434);
            this.SaveGroupedProfile.Margin = new System.Windows.Forms.Padding(0);
            this.SaveGroupedProfile.MaximumSize = new System.Drawing.Size(106, 29);
            this.SaveGroupedProfile.MinimumSize = new System.Drawing.Size(106, 29);
            this.SaveGroupedProfile.Name = "SaveGroupedProfile";
            this.SaveGroupedProfile.Size = new System.Drawing.Size(106, 29);
            this.SaveGroupedProfile.TabIndex = 17;
            this.SaveGroupedProfile.Text = "Save && Close";
            this.SaveGroupedProfile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.SaveGroupedProfile.Click += new System.EventHandler(this.SaveGroupedProfile_Click);
            this.SaveGroupedProfile.MouseEnter += new System.EventHandler(this.SaveGroupedProfile_MouseEnter);
            this.SaveGroupedProfile.MouseLeave += new System.EventHandler(this.SaveGroupedProfile_MouseLeave);
            // 
            // CancelGroupedProfileEdition
            // 
            this.CancelGroupedProfileEdition.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.CancelGroupedProfileEdition.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelGroupedProfileEdition.ForeColor = System.Drawing.Color.White;
            this.CancelGroupedProfileEdition.Image = global::Quester.Properties.Resources.blackB;
            this.CancelGroupedProfileEdition.Location = new System.Drawing.Point(148, 434);
            this.CancelGroupedProfileEdition.Margin = new System.Windows.Forms.Padding(0);
            this.CancelGroupedProfileEdition.MaximumSize = new System.Drawing.Size(106, 29);
            this.CancelGroupedProfileEdition.MinimumSize = new System.Drawing.Size(106, 29);
            this.CancelGroupedProfileEdition.Name = "CancelGroupedProfileEdition";
            this.CancelGroupedProfileEdition.Size = new System.Drawing.Size(106, 29);
            this.CancelGroupedProfileEdition.TabIndex = 18;
            this.CancelGroupedProfileEdition.Text = "Cancel && Close";
            this.CancelGroupedProfileEdition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CancelGroupedProfileEdition.Click += new System.EventHandler(this.CancelGroupedProfileEdition_Click);
            this.CancelGroupedProfileEdition.MouseEnter += new System.EventHandler(this.CancelGroupedProfileEdition_MouseEnter);
            this.CancelGroupedProfileEdition.MouseLeave += new System.EventHandler(this.CancelGroupedProfileEdition_MouseLeave);
            // 
            // MoveUpButton
            // 
            this.MoveUpButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.MoveUpButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MoveUpButton.ForeColor = System.Drawing.Color.White;
            this.MoveUpButton.Image = global::Quester.Properties.Resources.blackB;
            this.MoveUpButton.Location = new System.Drawing.Point(313, 393);
            this.MoveUpButton.Margin = new System.Windows.Forms.Padding(0);
            this.MoveUpButton.MaximumSize = new System.Drawing.Size(106, 29);
            this.MoveUpButton.MinimumSize = new System.Drawing.Size(106, 29);
            this.MoveUpButton.Name = "MoveUpButton";
            this.MoveUpButton.Size = new System.Drawing.Size(106, 29);
            this.MoveUpButton.TabIndex = 19;
            this.MoveUpButton.Text = "Move Up";
            this.MoveUpButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.MoveUpButton.Click += new System.EventHandler(this.MoveUpButton_Click);
            this.MoveUpButton.MouseEnter += new System.EventHandler(this.MoveUpButton_MouseEnter);
            this.MoveUpButton.MouseLeave += new System.EventHandler(this.MoveUpButton_MouseLeave);
            // 
            // GroupSelectedProfile
            // 
            this.GroupSelectedProfile.Location = new System.Drawing.Point(271, 183);
            this.GroupSelectedProfile.Name = "GroupSelectedProfile";
            this.GroupSelectedProfile.Size = new System.Drawing.Size(27, 23);
            this.GroupSelectedProfile.TabIndex = 20;
            this.GroupSelectedProfile.Text = ">";
            this.GroupSelectedProfile.UseVisualStyleBackColor = true;
            this.GroupSelectedProfile.Click += new System.EventHandler(this.DoGroupSelectedProfile);
            // 
            // UngroupSelectedProfile
            // 
            this.UngroupSelectedProfile.Location = new System.Drawing.Point(271, 238);
            this.UngroupSelectedProfile.Name = "UngroupSelectedProfile";
            this.UngroupSelectedProfile.Size = new System.Drawing.Size(28, 23);
            this.UngroupSelectedProfile.TabIndex = 21;
            this.UngroupSelectedProfile.Text = "<";
            this.UngroupSelectedProfile.UseVisualStyleBackColor = true;
            this.UngroupSelectedProfile.Click += new System.EventHandler(this.DoUngroupSelectedProfile);
            // 
            // UngroupAllProfiles
            // 
            this.UngroupAllProfiles.Location = new System.Drawing.Point(271, 277);
            this.UngroupAllProfiles.Name = "UngroupAllProfiles";
            this.UngroupAllProfiles.Size = new System.Drawing.Size(28, 23);
            this.UngroupAllProfiles.TabIndex = 22;
            this.UngroupAllProfiles.Text = "<<";
            this.UngroupAllProfiles.UseVisualStyleBackColor = true;
            this.UngroupAllProfiles.Click += new System.EventHandler(this.DoUngroupAllProfiles);
            // 
            // GroupedProfileManager
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(575, 475);
            this.Controls.Add(this.UngroupAllProfiles);
            this.Controls.Add(this.UngroupSelectedProfile);
            this.Controls.Add(this.GroupSelectedProfile);
            this.Controls.Add(this.MoveUpButton);
            this.Controls.Add(this.CancelGroupedProfileEdition);
            this.Controls.Add(this.SaveGroupedProfile);
            this.Controls.Add(this.GroupedProfilesList);
            this.Controls.Add(this.AvailableSimpleProfilesLabel);
            this.Controls.Add(this.MoveDownButton);
            this.Controls.Add(this.SaveGroupedProfileAs);
            this.Controls.Add(this.GroupedProfileManagerFormTitle);
            this.Controls.Add(this.CurrentlyGroupedProfilesLabel);
            this.Controls.Add(this.TopLeftLogo);
            this.Controls.Add(this.SimpleProfilesList);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.ReduceButton);
            this.Controls.Add(this.ControlMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(575, 475);
            this.MinimumSize = new System.Drawing.Size(575, 475);
            this.Name = "GroupedProfileManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GroupedProfileManager";
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
        private System.Windows.Forms.Label CurrentlyGroupedProfilesLabel;
        private System.Windows.Forms.Label GroupedProfileManagerFormTitle;
        private System.Windows.Forms.Label SaveGroupedProfileAs;
        private System.Windows.Forms.Label MoveDownButton;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label AvailableSimpleProfilesLabel;
        private System.Windows.Forms.ListBox GroupedProfilesList;
        private System.Windows.Forms.Label SaveGroupedProfile;
        private System.Windows.Forms.Label CancelGroupedProfileEdition;
        private System.Windows.Forms.Label MoveUpButton;
        private System.Windows.Forms.Button GroupSelectedProfile;
        private System.Windows.Forms.Button UngroupSelectedProfile;
        private System.Windows.Forms.Button UngroupAllProfiles;
    }
}

