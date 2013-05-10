namespace Quester.Profile
{
    partial class LoadQuesterProfile
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
            this.LoadSimpleProfile = new DevComponents.DotNetBar.ButtonX();
            this.GroupedProfilesList = new System.Windows.Forms.ListBox();
            this.LoadGroupedProfile = new DevComponents.DotNetBar.ButtonX();
            this.SimpleProfilesList = new System.Windows.Forms.ListBox();
            this.GroupedProfileListLabel = new DevComponents.DotNetBar.LabelX();
            this.SimpleProfilesListLabel = new DevComponents.DotNetBar.LabelX();
            this.QuesterProfileManagementSystemButton = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // LoadSimpleProfile
            // 
            this.LoadSimpleProfile.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.LoadSimpleProfile.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.LoadSimpleProfile.Location = new System.Drawing.Point(12, 135);
            this.LoadSimpleProfile.Name = "LoadSimpleProfile";
            this.LoadSimpleProfile.Size = new System.Drawing.Size(241, 22);
            this.LoadSimpleProfile.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.LoadSimpleProfile.TabIndex = 0;
            this.LoadSimpleProfile.Text = "Load a Simple Profile";
            this.LoadSimpleProfile.Click += new System.EventHandler(this.DoLoadSimpleProfile);
            // 
            // GroupedProfilesList
            // 
            this.GroupedProfilesList.FormattingEnabled = true;
            this.GroupedProfilesList.Location = new System.Drawing.Point(287, 34);
            this.GroupedProfilesList.Name = "GroupedProfilesList";
            this.GroupedProfilesList.Size = new System.Drawing.Size(241, 95);
            this.GroupedProfilesList.TabIndex = 1;
            this.GroupedProfilesList.DoubleClick += new System.EventHandler(this.DoLoadGroupedProfile);
            // 
            // LoadGroupedProfile
            // 
            this.LoadGroupedProfile.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.LoadGroupedProfile.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.LoadGroupedProfile.Location = new System.Drawing.Point(287, 135);
            this.LoadGroupedProfile.Name = "LoadGroupedProfile";
            this.LoadGroupedProfile.Size = new System.Drawing.Size(240, 22);
            this.LoadGroupedProfile.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.LoadGroupedProfile.TabIndex = 3;
            this.LoadGroupedProfile.Text = "Load a Grouped Profile";
            this.LoadGroupedProfile.Click += new System.EventHandler(this.DoLoadGroupedProfile);
            // 
            // SimpleProfilesList
            // 
            this.SimpleProfilesList.FormattingEnabled = true;
            this.SimpleProfilesList.Location = new System.Drawing.Point(12, 34);
            this.SimpleProfilesList.Name = "SimpleProfilesList";
            this.SimpleProfilesList.Size = new System.Drawing.Size(241, 95);
            this.SimpleProfilesList.TabIndex = 5;
            this.SimpleProfilesList.DoubleClick += new System.EventHandler(this.DoLoadSimpleProfile);
            // 
            // GroupedProfileListLabel
            // 
            // 
            // 
            // 
            this.GroupedProfileListLabel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.GroupedProfileListLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.GroupedProfileListLabel.Location = new System.Drawing.Point(287, 12);
            this.GroupedProfileListLabel.Name = "GroupedProfileListLabel";
            this.GroupedProfileListLabel.Size = new System.Drawing.Size(241, 16);
            this.GroupedProfileListLabel.TabIndex = 7;
            this.GroupedProfileListLabel.Text = "Grouped Profile List";
            // 
            // SimpleProfilesListLabel
            // 
            // 
            // 
            // 
            this.SimpleProfilesListLabel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.SimpleProfilesListLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.SimpleProfilesListLabel.Location = new System.Drawing.Point(12, 12);
            this.SimpleProfilesListLabel.Name = "SimpleProfilesListLabel";
            this.SimpleProfilesListLabel.Size = new System.Drawing.Size(241, 16);
            this.SimpleProfilesListLabel.TabIndex = 8;
            this.SimpleProfilesListLabel.Text = "Simple Profile List";
            // 
            // QuesterProfileManagementSystemButton
            // 
            this.QuesterProfileManagementSystemButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.QuesterProfileManagementSystemButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.QuesterProfileManagementSystemButton.Location = new System.Drawing.Point(12, 179);
            this.QuesterProfileManagementSystemButton.Name = "QuesterProfileManagementSystemButton";
            this.QuesterProfileManagementSystemButton.Size = new System.Drawing.Size(515, 22);
            this.QuesterProfileManagementSystemButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.QuesterProfileManagementSystemButton.TabIndex = 10;
            this.QuesterProfileManagementSystemButton.Text = "Quester Profile Management System";
            this.QuesterProfileManagementSystemButton.Click += new System.EventHandler(this.QuesterProfileManagementSystemButton_Click);
            // 
            // LoadQuesterProfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(541, 212);
            this.Controls.Add(this.QuesterProfileManagementSystemButton);
            this.Controls.Add(this.SimpleProfilesListLabel);
            this.Controls.Add(this.GroupedProfileListLabel);
            this.Controls.Add(this.SimpleProfilesList);
            this.Controls.Add(this.LoadGroupedProfile);
            this.Controls.Add(this.GroupedProfilesList);
            this.Controls.Add(this.LoadSimpleProfile);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(557, 250);
            this.MinimumSize = new System.Drawing.Size(557, 250);
            this.Name = "LoadQuesterProfile";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Load a Quester Profile";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoadQuesterProfile_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX LoadSimpleProfile;
        private System.Windows.Forms.ListBox GroupedProfilesList;
        private DevComponents.DotNetBar.ButtonX LoadGroupedProfile;
        private System.Windows.Forms.ListBox SimpleProfilesList;
        private DevComponents.DotNetBar.LabelX GroupedProfileListLabel;
        private DevComponents.DotNetBar.LabelX SimpleProfilesListLabel;
        private DevComponents.DotNetBar.ButtonX QuesterProfileManagementSystemButton;
    }
}