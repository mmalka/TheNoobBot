namespace Quester.Profile
{
    partial class GroupedProfileManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GroupedProfileManager));
            this.SaveGroupedProfileAs = new DevComponents.DotNetBar.ButtonX();
            this.CurrentlyGroupedProfiles = new System.Windows.Forms.ListBox();
            this.SaveGroupedProfile = new DevComponents.DotNetBar.ButtonX();
            this.AvailableSimpleProfiles = new System.Windows.Forms.ListBox();
            this.CurrentlyGroupedProfilesLabel = new DevComponents.DotNetBar.LabelX();
            this.AvailableSimpleProfilesLabel = new DevComponents.DotNetBar.LabelX();
            this.CancelGroupedProfileEdition = new DevComponents.DotNetBar.ButtonX();
            this.UngroupSelectedProfile = new DevComponents.DotNetBar.ButtonX();
            this.GroupSelectedProfile = new DevComponents.DotNetBar.ButtonX();
            this.UngroupAllProfiles = new DevComponents.DotNetBar.ButtonX();
            this.MoveUpButton = new DevComponents.DotNetBar.ButtonX();
            this.MoveDownButton = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // SaveGroupedProfileAs
            // 
            this.SaveGroupedProfileAs.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.SaveGroupedProfileAs.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.SaveGroupedProfileAs.Location = new System.Drawing.Point(12, 179);
            this.SaveGroupedProfileAs.Name = "SaveGroupedProfileAs";
            this.SaveGroupedProfileAs.Size = new System.Drawing.Size(166, 22);
            this.SaveGroupedProfileAs.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.SaveGroupedProfileAs.TabIndex = 0;
            this.SaveGroupedProfileAs.Text = "Save as new && Close";
            this.SaveGroupedProfileAs.Click += new System.EventHandler(this.SaveGroupedProfileAs_Click);
            // 
            // CurrentlyGroupedProfiles
            // 
            this.CurrentlyGroupedProfiles.FormattingEnabled = true;
            this.CurrentlyGroupedProfiles.Location = new System.Drawing.Point(287, 34);
            this.CurrentlyGroupedProfiles.Name = "CurrentlyGroupedProfiles";
            this.CurrentlyGroupedProfiles.Size = new System.Drawing.Size(241, 95);
            this.CurrentlyGroupedProfiles.TabIndex = 1;
            this.CurrentlyGroupedProfiles.DoubleClick += new System.EventHandler(this.DoUngroupSelectedProfile);
            // 
            // SaveGroupedProfile
            // 
            this.SaveGroupedProfile.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.SaveGroupedProfile.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.SaveGroupedProfile.Location = new System.Drawing.Point(186, 179);
            this.SaveGroupedProfile.Name = "SaveGroupedProfile";
            this.SaveGroupedProfile.Size = new System.Drawing.Size(166, 22);
            this.SaveGroupedProfile.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.SaveGroupedProfile.TabIndex = 3;
            this.SaveGroupedProfile.Text = "Save && Close";
            this.SaveGroupedProfile.Click += new System.EventHandler(this.SaveGroupedProfile_Click);
            // 
            // AvailableSimpleProfiles
            // 
            this.AvailableSimpleProfiles.FormattingEnabled = true;
            this.AvailableSimpleProfiles.Location = new System.Drawing.Point(12, 34);
            this.AvailableSimpleProfiles.Name = "AvailableSimpleProfiles";
            this.AvailableSimpleProfiles.Size = new System.Drawing.Size(241, 95);
            this.AvailableSimpleProfiles.TabIndex = 5;
            this.AvailableSimpleProfiles.DoubleClick += new System.EventHandler(this.DoGroupSelectedProfile);
            // 
            // CurrentlyGroupedProfilesLabel
            // 
            // 
            // 
            // 
            this.CurrentlyGroupedProfilesLabel.BackgroundStyle.Class = "";
            this.CurrentlyGroupedProfilesLabel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.CurrentlyGroupedProfilesLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.CurrentlyGroupedProfilesLabel.Location = new System.Drawing.Point(287, 12);
            this.CurrentlyGroupedProfilesLabel.Name = "CurrentlyGroupedProfilesLabel";
            this.CurrentlyGroupedProfilesLabel.Size = new System.Drawing.Size(241, 16);
            this.CurrentlyGroupedProfilesLabel.TabIndex = 7;
            this.CurrentlyGroupedProfilesLabel.Text = "Currently grouped profiles :";
            // 
            // AvailableSimpleProfilesLabel
            // 
            // 
            // 
            // 
            this.AvailableSimpleProfilesLabel.BackgroundStyle.Class = "";
            this.AvailableSimpleProfilesLabel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.AvailableSimpleProfilesLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.AvailableSimpleProfilesLabel.Location = new System.Drawing.Point(12, 12);
            this.AvailableSimpleProfilesLabel.Name = "AvailableSimpleProfilesLabel";
            this.AvailableSimpleProfilesLabel.Size = new System.Drawing.Size(241, 16);
            this.AvailableSimpleProfilesLabel.TabIndex = 8;
            this.AvailableSimpleProfilesLabel.Text = "Available profiles :";
            // 
            // CancelGroupedProfileEdition
            // 
            this.CancelGroupedProfileEdition.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.CancelGroupedProfileEdition.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.CancelGroupedProfileEdition.Location = new System.Drawing.Point(361, 179);
            this.CancelGroupedProfileEdition.Name = "CancelGroupedProfileEdition";
            this.CancelGroupedProfileEdition.Size = new System.Drawing.Size(166, 22);
            this.CancelGroupedProfileEdition.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.CancelGroupedProfileEdition.TabIndex = 10;
            this.CancelGroupedProfileEdition.Text = "Cancel && Close";
            this.CancelGroupedProfileEdition.Click += new System.EventHandler(this.CancelGroupedProfileEdition_Click);
            // 
            // UngroupSelectedProfile
            // 
            this.UngroupSelectedProfile.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.UngroupSelectedProfile.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.UngroupSelectedProfile.Location = new System.Drawing.Point(257, 77);
            this.UngroupSelectedProfile.Name = "UngroupSelectedProfile";
            this.UngroupSelectedProfile.Size = new System.Drawing.Size(24, 24);
            this.UngroupSelectedProfile.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.UngroupSelectedProfile.TabIndex = 11;
            this.UngroupSelectedProfile.Text = "<";
            this.UngroupSelectedProfile.Tooltip = "Ungroup selected profile";
            this.UngroupSelectedProfile.Click += new System.EventHandler(this.DoUngroupSelectedProfile);
            // 
            // GroupSelectedProfile
            // 
            this.GroupSelectedProfile.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.GroupSelectedProfile.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.GroupSelectedProfile.Location = new System.Drawing.Point(257, 41);
            this.GroupSelectedProfile.Name = "GroupSelectedProfile";
            this.GroupSelectedProfile.Size = new System.Drawing.Size(24, 24);
            this.GroupSelectedProfile.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.GroupSelectedProfile.TabIndex = 12;
            this.GroupSelectedProfile.Text = ">";
            this.GroupSelectedProfile.Tooltip = "Group the selected profile";
            this.GroupSelectedProfile.Click += new System.EventHandler(this.DoGroupSelectedProfile);
            // 
            // UngroupAllProfiles
            // 
            this.UngroupAllProfiles.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.UngroupAllProfiles.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.UngroupAllProfiles.Location = new System.Drawing.Point(257, 105);
            this.UngroupAllProfiles.Name = "UngroupAllProfiles";
            this.UngroupAllProfiles.Size = new System.Drawing.Size(24, 24);
            this.UngroupAllProfiles.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.UngroupAllProfiles.TabIndex = 13;
            this.UngroupAllProfiles.Text = "<<";
            this.UngroupAllProfiles.Tooltip = "Ungroup ALL Profiles";
            this.UngroupAllProfiles.Click += new System.EventHandler(this.DoUngroupAllProfiles);
            // 
            // MoveUpButton
            // 
            this.MoveUpButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.MoveUpButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.MoveUpButton.Location = new System.Drawing.Point(361, 150);
            this.MoveUpButton.Name = "MoveUpButton";
            this.MoveUpButton.Size = new System.Drawing.Size(80, 16);
            this.MoveUpButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.MoveUpButton.TabIndex = 14;
            this.MoveUpButton.Text = "Move Up";
            this.MoveUpButton.Click += new System.EventHandler(this.MoveUpButton_Click);
            // 
            // MoveDownButton
            // 
            this.MoveDownButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.MoveDownButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.MoveDownButton.Location = new System.Drawing.Point(448, 150);
            this.MoveDownButton.Name = "MoveDownButton";
            this.MoveDownButton.Size = new System.Drawing.Size(80, 16);
            this.MoveDownButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.MoveDownButton.TabIndex = 15;
            this.MoveDownButton.Text = "Move Down";
            this.MoveDownButton.Click += new System.EventHandler(this.MoveDownButton_Click);
            // 
            // GroupedProfileManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(541, 212);
            this.Controls.Add(this.MoveDownButton);
            this.Controls.Add(this.MoveUpButton);
            this.Controls.Add(this.UngroupAllProfiles);
            this.Controls.Add(this.GroupSelectedProfile);
            this.Controls.Add(this.UngroupSelectedProfile);
            this.Controls.Add(this.CancelGroupedProfileEdition);
            this.Controls.Add(this.AvailableSimpleProfilesLabel);
            this.Controls.Add(this.CurrentlyGroupedProfilesLabel);
            this.Controls.Add(this.AvailableSimpleProfiles);
            this.Controls.Add(this.SaveGroupedProfile);
            this.Controls.Add(this.CurrentlyGroupedProfiles);
            this.Controls.Add(this.SaveGroupedProfileAs);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(557, 250);
            this.MinimumSize = new System.Drawing.Size(557, 250);
            this.Name = "GroupedProfileManager";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Grouped Profile Manager";
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX SaveGroupedProfileAs;
        private System.Windows.Forms.ListBox CurrentlyGroupedProfiles;
        private DevComponents.DotNetBar.ButtonX SaveGroupedProfile;
        private System.Windows.Forms.ListBox AvailableSimpleProfiles;
        private DevComponents.DotNetBar.LabelX CurrentlyGroupedProfilesLabel;
        private DevComponents.DotNetBar.LabelX AvailableSimpleProfilesLabel;
        private DevComponents.DotNetBar.ButtonX CancelGroupedProfileEdition;
        private DevComponents.DotNetBar.ButtonX UngroupSelectedProfile;
        private DevComponents.DotNetBar.ButtonX GroupSelectedProfile;
        private DevComponents.DotNetBar.ButtonX UngroupAllProfiles;
        private DevComponents.DotNetBar.ButtonX MoveUpButton;
        private DevComponents.DotNetBar.ButtonX MoveDownButton;
    }
}