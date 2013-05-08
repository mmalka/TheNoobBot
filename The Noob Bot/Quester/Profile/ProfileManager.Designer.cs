namespace Quester.Profile
{
    partial class ProfileManager
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
            this.ProfileManagerAddGrouped = new DevComponents.DotNetBar.ButtonX();
            this.ExistingGroupedProfiles = new System.Windows.Forms.ListBox();
            this.ProfileManagerAdd = new DevComponents.DotNetBar.ButtonX();
            this.ProfileManagerEditGrouped = new DevComponents.DotNetBar.ButtonX();
            this.ProfileManagerEdit = new DevComponents.DotNetBar.ButtonX();
            this.ExistingSimpleProfiles = new System.Windows.Forms.ListBox();
            this.ProfileManagerRemoveGrouped = new DevComponents.DotNetBar.ButtonX();
            this.ProfileManagerGroupedLabel = new DevComponents.DotNetBar.LabelX();
            this.ProfileManagerSimpleLabel = new DevComponents.DotNetBar.LabelX();
            this.ProfileManagerRemove = new DevComponents.DotNetBar.ButtonX();
            this.ProfileManagerGroupedDocumentation = new DevComponents.DotNetBar.ButtonX();
            this.ProfileManagerSimpleDocumentation = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // ProfileManagerAddGrouped
            // 
            this.ProfileManagerAddGrouped.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ProfileManagerAddGrouped.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ProfileManagerAddGrouped.Location = new System.Drawing.Point(290, 37);
            this.ProfileManagerAddGrouped.Name = "ProfileManagerAddGrouped";
            this.ProfileManagerAddGrouped.Size = new System.Drawing.Size(181, 25);
            this.ProfileManagerAddGrouped.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ProfileManagerAddGrouped.TabIndex = 0;
            this.ProfileManagerAddGrouped.Text = "Create a grouped profile";
            this.ProfileManagerAddGrouped.Click += new System.EventHandler(this.ProfileManagerAddGrouped_Click);
            // 
            // ExistingGroupedProfiles
            // 
            this.ExistingGroupedProfiles.FormattingEnabled = true;
            this.ExistingGroupedProfiles.Location = new System.Drawing.Point(12, 37);
            this.ExistingGroupedProfiles.Name = "ExistingGroupedProfiles";
            this.ExistingGroupedProfiles.Size = new System.Drawing.Size(272, 134);
            this.ExistingGroupedProfiles.TabIndex = 1;
            // 
            // ProfileManagerAdd
            // 
            this.ProfileManagerAdd.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ProfileManagerAdd.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ProfileManagerAdd.Location = new System.Drawing.Point(290, 202);
            this.ProfileManagerAdd.Name = "ProfileManagerAdd";
            this.ProfileManagerAdd.Size = new System.Drawing.Size(181, 25);
            this.ProfileManagerAdd.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ProfileManagerAdd.TabIndex = 2;
            this.ProfileManagerAdd.Text = "Create a simple profile";
            this.ProfileManagerAdd.Click += new System.EventHandler(this.ProfileManagerAdd_Click);
            // 
            // ProfileManagerEditGrouped
            // 
            this.ProfileManagerEditGrouped.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ProfileManagerEditGrouped.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ProfileManagerEditGrouped.Location = new System.Drawing.Point(290, 68);
            this.ProfileManagerEditGrouped.Name = "ProfileManagerEditGrouped";
            this.ProfileManagerEditGrouped.Size = new System.Drawing.Size(181, 25);
            this.ProfileManagerEditGrouped.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ProfileManagerEditGrouped.TabIndex = 3;
            this.ProfileManagerEditGrouped.Text = "Edit a grouped profile";
            this.ProfileManagerEditGrouped.Click += new System.EventHandler(this.ProfileManagerEditGrouped_Click);
            // 
            // ProfileManagerEdit
            // 
            this.ProfileManagerEdit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ProfileManagerEdit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ProfileManagerEdit.Location = new System.Drawing.Point(290, 233);
            this.ProfileManagerEdit.Name = "ProfileManagerEdit";
            this.ProfileManagerEdit.Size = new System.Drawing.Size(181, 25);
            this.ProfileManagerEdit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ProfileManagerEdit.TabIndex = 4;
            this.ProfileManagerEdit.Text = "Edit a simple profile";
            this.ProfileManagerEdit.Click += new System.EventHandler(this.ProfileManageEdit_Click);
            // 
            // ExistingSimpleProfiles
            // 
            this.ExistingSimpleProfiles.FormattingEnabled = true;
            this.ExistingSimpleProfiles.Location = new System.Drawing.Point(12, 202);
            this.ExistingSimpleProfiles.Name = "ExistingSimpleProfiles";
            this.ExistingSimpleProfiles.Size = new System.Drawing.Size(272, 134);
            this.ExistingSimpleProfiles.TabIndex = 5;
            // 
            // ProfileManagerRemoveGrouped
            // 
            this.ProfileManagerRemoveGrouped.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ProfileManagerRemoveGrouped.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ProfileManagerRemoveGrouped.Location = new System.Drawing.Point(290, 146);
            this.ProfileManagerRemoveGrouped.Name = "ProfileManagerRemoveGrouped";
            this.ProfileManagerRemoveGrouped.Size = new System.Drawing.Size(181, 25);
            this.ProfileManagerRemoveGrouped.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ProfileManagerRemoveGrouped.TabIndex = 6;
            this.ProfileManagerRemoveGrouped.Text = "Remove a grouped profile";
            this.ProfileManagerRemoveGrouped.Click += new System.EventHandler(this.ProfileManagerRemoveGrouped_Click);
            // 
            // ProfileManagerGroupedLabel
            // 
            // 
            // 
            // 
            this.ProfileManagerGroupedLabel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ProfileManagerGroupedLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.ProfileManagerGroupedLabel.Location = new System.Drawing.Point(12, 12);
            this.ProfileManagerGroupedLabel.Name = "ProfileManagerGroupedLabel";
            this.ProfileManagerGroupedLabel.Size = new System.Drawing.Size(459, 19);
            this.ProfileManagerGroupedLabel.TabIndex = 7;
            this.ProfileManagerGroupedLabel.Text = "Grouped Profile Manager";
            this.ProfileManagerGroupedLabel.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // ProfileManagerSimpleLabel
            // 
            // 
            // 
            // 
            this.ProfileManagerSimpleLabel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ProfileManagerSimpleLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.ProfileManagerSimpleLabel.Location = new System.Drawing.Point(12, 177);
            this.ProfileManagerSimpleLabel.Name = "ProfileManagerSimpleLabel";
            this.ProfileManagerSimpleLabel.Size = new System.Drawing.Size(459, 19);
            this.ProfileManagerSimpleLabel.TabIndex = 8;
            this.ProfileManagerSimpleLabel.Text = "Simple Profile Manager";
            this.ProfileManagerSimpleLabel.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // ProfileManagerRemove
            // 
            this.ProfileManagerRemove.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ProfileManagerRemove.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ProfileManagerRemove.Location = new System.Drawing.Point(290, 311);
            this.ProfileManagerRemove.Name = "ProfileManagerRemove";
            this.ProfileManagerRemove.Size = new System.Drawing.Size(181, 25);
            this.ProfileManagerRemove.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ProfileManagerRemove.TabIndex = 9;
            this.ProfileManagerRemove.Text = "Remove a grouped profile";
            this.ProfileManagerRemove.Click += new System.EventHandler(this.ProfileManagerRemove_Click);
            // 
            // ProfileManagerGroupedDocumentation
            // 
            this.ProfileManagerGroupedDocumentation.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ProfileManagerGroupedDocumentation.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ProfileManagerGroupedDocumentation.Location = new System.Drawing.Point(290, 99);
            this.ProfileManagerGroupedDocumentation.Name = "ProfileManagerGroupedDocumentation";
            this.ProfileManagerGroupedDocumentation.Size = new System.Drawing.Size(181, 25);
            this.ProfileManagerGroupedDocumentation.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ProfileManagerGroupedDocumentation.TabIndex = 10;
            this.ProfileManagerGroupedDocumentation.Text = "Grouped Profile Documentation";
            this.ProfileManagerGroupedDocumentation.Click += new System.EventHandler(this.ProfileManagerGroupedDocumentation_Click);
            // 
            // ProfileManagerSimpleDocumentation
            // 
            this.ProfileManagerSimpleDocumentation.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ProfileManagerSimpleDocumentation.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.ProfileManagerSimpleDocumentation.Location = new System.Drawing.Point(290, 264);
            this.ProfileManagerSimpleDocumentation.Name = "ProfileManagerSimpleDocumentation";
            this.ProfileManagerSimpleDocumentation.Size = new System.Drawing.Size(181, 25);
            this.ProfileManagerSimpleDocumentation.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ProfileManagerSimpleDocumentation.TabIndex = 11;
            this.ProfileManagerSimpleDocumentation.Text = "Simple Profile Documentation";
            this.ProfileManagerSimpleDocumentation.Click += new System.EventHandler(this.ProfileManagerSimpleDocumentation_Click);
            // 
            // ProfileManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(100F, 100F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(483, 353);
            this.Controls.Add(this.ProfileManagerSimpleDocumentation);
            this.Controls.Add(this.ProfileManagerGroupedDocumentation);
            this.Controls.Add(this.ProfileManagerRemove);
            this.Controls.Add(this.ProfileManagerSimpleLabel);
            this.Controls.Add(this.ProfileManagerGroupedLabel);
            this.Controls.Add(this.ProfileManagerRemoveGrouped);
            this.Controls.Add(this.ExistingSimpleProfiles);
            this.Controls.Add(this.ProfileManagerEdit);
            this.Controls.Add(this.ProfileManagerEditGrouped);
            this.Controls.Add(this.ProfileManagerAdd);
            this.Controls.Add(this.ExistingGroupedProfiles);
            this.Controls.Add(this.ProfileManagerAddGrouped);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ProfileManager";
            this.Text = "Quester Profile Manager";
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX ProfileManagerAddGrouped;
        private System.Windows.Forms.ListBox ExistingGroupedProfiles;
        private DevComponents.DotNetBar.ButtonX ProfileManagerAdd;
        private DevComponents.DotNetBar.ButtonX ProfileManagerEditGrouped;
        private DevComponents.DotNetBar.ButtonX ProfileManagerEdit;
        private System.Windows.Forms.ListBox ExistingSimpleProfiles;
        private DevComponents.DotNetBar.ButtonX ProfileManagerRemoveGrouped;
        private DevComponents.DotNetBar.LabelX ProfileManagerGroupedLabel;
        private DevComponents.DotNetBar.LabelX ProfileManagerSimpleLabel;
        private DevComponents.DotNetBar.ButtonX ProfileManagerRemove;
        private DevComponents.DotNetBar.ButtonX ProfileManagerGroupedDocumentation;
        private DevComponents.DotNetBar.ButtonX ProfileManagerSimpleDocumentation;
    }
}