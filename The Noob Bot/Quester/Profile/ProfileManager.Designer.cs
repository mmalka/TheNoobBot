using System;
using System.Windows.Forms;
using nManager.Helpful.Forms.UserControls;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProfileManager));
            this.ProfileManagerAddGrouped = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.ExistingGroupedProfiles = new System.Windows.Forms.ListBox();
            this.ProfileManagerAdd = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.ProfileManagerEditGrouped = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.ProfileManagerEdit = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.ExistingSimpleProfiles = new System.Windows.Forms.ListBox();
            this.ProfileManagerRemoveGrouped = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.ProfileManagerGroupedLabel = new System.Windows.Forms.Label();
            this.ProfileManagerSimpleLabel = new System.Windows.Forms.Label();
            this.ProfileManagerRemove = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.ProfileManagerGroupedDocumentation = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.ProfileManagerSimpleDocumentation = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.MainHeader = new nManager.Helpful.Forms.UserControls.TnbControlMenu();
            this.SuspendLayout();
            // 
            // ProfileManagerAddGrouped
            // 
            this.ProfileManagerAddGrouped.AutoEllipsis = true;
            this.ProfileManagerAddGrouped.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ProfileManagerAddGrouped.ForeColor = System.Drawing.Color.Snow;
            this.ProfileManagerAddGrouped.HooverImage = global::Quester.Properties.Resources.greenB_150;
            this.ProfileManagerAddGrouped.Image = global::Quester.Properties.Resources.blueB_150;
            this.ProfileManagerAddGrouped.Location = new System.Drawing.Point(259, 74);
            this.ProfileManagerAddGrouped.Name = "ProfileManagerAddGrouped";
            this.ProfileManagerAddGrouped.Size = new System.Drawing.Size(150, 29);
            this.ProfileManagerAddGrouped.TabIndex = 0;
            this.ProfileManagerAddGrouped.Text = "Create a grouped profile";
            this.ProfileManagerAddGrouped.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ProfileManagerAddGrouped.Click += new System.EventHandler(this.ProfileManagerAddGrouped_Click);
            // 
            // ExistingGroupedProfiles
            // 
            this.ExistingGroupedProfiles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ExistingGroupedProfiles.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.ExistingGroupedProfiles.FormattingEnabled = true;
            this.ExistingGroupedProfiles.Location = new System.Drawing.Point(13, 74);
            this.ExistingGroupedProfiles.Name = "ExistingGroupedProfiles";
            this.ExistingGroupedProfiles.Size = new System.Drawing.Size(241, 108);
            this.ExistingGroupedProfiles.TabIndex = 1;
            this.ExistingGroupedProfiles.DoubleClick += new System.EventHandler(this.DoProfileManagerEditGrouped);
            // 
            // ProfileManagerAdd
            // 
            this.ProfileManagerAdd.AutoEllipsis = true;
            this.ProfileManagerAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ProfileManagerAdd.ForeColor = System.Drawing.Color.Snow;
            this.ProfileManagerAdd.HooverImage = ((System.Drawing.Image)(resources.GetObject("ProfileManagerAdd.HooverImage")));
            this.ProfileManagerAdd.Image = global::Quester.Properties.Resources.blueB_150;
            this.ProfileManagerAdd.Location = new System.Drawing.Point(259, 214);
            this.ProfileManagerAdd.Name = "ProfileManagerAdd";
            this.ProfileManagerAdd.Size = new System.Drawing.Size(150, 29);
            this.ProfileManagerAdd.TabIndex = 2;
            this.ProfileManagerAdd.Text = "Create a simple profile";
            this.ProfileManagerAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ProfileManagerAdd.Click += new System.EventHandler(this.ProfileManagerAdd_Click);
            // 
            // ProfileManagerEditGrouped
            // 
            this.ProfileManagerEditGrouped.AutoEllipsis = true;
            this.ProfileManagerEditGrouped.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ProfileManagerEditGrouped.ForeColor = System.Drawing.Color.Snow;
            this.ProfileManagerEditGrouped.HooverImage = ((System.Drawing.Image)(resources.GetObject("ProfileManagerEditGrouped.HooverImage")));
            this.ProfileManagerEditGrouped.Image = global::Quester.Properties.Resources.blueB_150;
            this.ProfileManagerEditGrouped.Location = new System.Drawing.Point(413, 74);
            this.ProfileManagerEditGrouped.Name = "ProfileManagerEditGrouped";
            this.ProfileManagerEditGrouped.Size = new System.Drawing.Size(150, 29);
            this.ProfileManagerEditGrouped.TabIndex = 3;
            this.ProfileManagerEditGrouped.Text = "Edit a grouped profile";
            this.ProfileManagerEditGrouped.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ProfileManagerEditGrouped.Click += new System.EventHandler(this.DoProfileManagerEditGrouped);
            // 
            // ProfileManagerEdit
            // 
            this.ProfileManagerEdit.AutoEllipsis = true;
            this.ProfileManagerEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ProfileManagerEdit.ForeColor = System.Drawing.Color.Snow;
            this.ProfileManagerEdit.HooverImage = ((System.Drawing.Image)(resources.GetObject("ProfileManagerEdit.HooverImage")));
            this.ProfileManagerEdit.Image = global::Quester.Properties.Resources.blueB_150;
            this.ProfileManagerEdit.Location = new System.Drawing.Point(413, 214);
            this.ProfileManagerEdit.Name = "ProfileManagerEdit";
            this.ProfileManagerEdit.Size = new System.Drawing.Size(150, 29);
            this.ProfileManagerEdit.TabIndex = 4;
            this.ProfileManagerEdit.Text = "Edit a simple profile";
            this.ProfileManagerEdit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ProfileManagerEdit.Click += new System.EventHandler(this.DoProfileManagerEditSimple);
            // 
            // ExistingSimpleProfiles
            // 
            this.ExistingSimpleProfiles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ExistingSimpleProfiles.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.ExistingSimpleProfiles.FormattingEnabled = true;
            this.ExistingSimpleProfiles.Location = new System.Drawing.Point(13, 214);
            this.ExistingSimpleProfiles.Name = "ExistingSimpleProfiles";
            this.ExistingSimpleProfiles.Size = new System.Drawing.Size(241, 108);
            this.ExistingSimpleProfiles.TabIndex = 5;
            this.ExistingSimpleProfiles.DoubleClick += new System.EventHandler(this.DoProfileManagerEditSimple);
            // 
            // ProfileManagerRemoveGrouped
            // 
            this.ProfileManagerRemoveGrouped.AutoEllipsis = true;
            this.ProfileManagerRemoveGrouped.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ProfileManagerRemoveGrouped.ForeColor = System.Drawing.Color.Snow;
            this.ProfileManagerRemoveGrouped.HooverImage = ((System.Drawing.Image)(resources.GetObject("ProfileManagerRemoveGrouped.HooverImage")));
            this.ProfileManagerRemoveGrouped.Image = ((System.Drawing.Image)(resources.GetObject("ProfileManagerRemoveGrouped.Image")));
            this.ProfileManagerRemoveGrouped.Location = new System.Drawing.Point(413, 154);
            this.ProfileManagerRemoveGrouped.Name = "ProfileManagerRemoveGrouped";
            this.ProfileManagerRemoveGrouped.Size = new System.Drawing.Size(150, 29);
            this.ProfileManagerRemoveGrouped.TabIndex = 6;
            this.ProfileManagerRemoveGrouped.Text = "Remove a grouped profile";
            this.ProfileManagerRemoveGrouped.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ProfileManagerRemoveGrouped.Click += new System.EventHandler(this.ProfileManagerRemoveGrouped_Click);
            // 
            // ProfileManagerGroupedLabel
            // 
            this.ProfileManagerGroupedLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.ProfileManagerGroupedLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.ProfileManagerGroupedLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.ProfileManagerGroupedLabel.Location = new System.Drawing.Point(13, 52);
            this.ProfileManagerGroupedLabel.Name = "ProfileManagerGroupedLabel";
            this.ProfileManagerGroupedLabel.Size = new System.Drawing.Size(406, 16);
            this.ProfileManagerGroupedLabel.TabIndex = 7;
            this.ProfileManagerGroupedLabel.Text = "Grouped Profile Manager";
            // 
            // ProfileManagerSimpleLabel
            // 
            this.ProfileManagerSimpleLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.ProfileManagerSimpleLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.ProfileManagerSimpleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.ProfileManagerSimpleLabel.Location = new System.Drawing.Point(13, 191);
            this.ProfileManagerSimpleLabel.Name = "ProfileManagerSimpleLabel";
            this.ProfileManagerSimpleLabel.Size = new System.Drawing.Size(406, 16);
            this.ProfileManagerSimpleLabel.TabIndex = 8;
            this.ProfileManagerSimpleLabel.Text = "Simple Profile Manager";
            // 
            // ProfileManagerRemove
            // 
            this.ProfileManagerRemove.AutoEllipsis = true;
            this.ProfileManagerRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ProfileManagerRemove.ForeColor = System.Drawing.Color.Snow;
            this.ProfileManagerRemove.HooverImage = ((System.Drawing.Image)(resources.GetObject("ProfileManagerRemove.HooverImage")));
            this.ProfileManagerRemove.Image = ((System.Drawing.Image)(resources.GetObject("ProfileManagerRemove.Image")));
            this.ProfileManagerRemove.Location = new System.Drawing.Point(413, 294);
            this.ProfileManagerRemove.Name = "ProfileManagerRemove";
            this.ProfileManagerRemove.Size = new System.Drawing.Size(150, 29);
            this.ProfileManagerRemove.TabIndex = 9;
            this.ProfileManagerRemove.Text = "Remove a simple profile";
            this.ProfileManagerRemove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ProfileManagerRemove.Click += new System.EventHandler(this.ProfileManagerRemove_Click);
            // 
            // ProfileManagerGroupedDocumentation
            // 
            this.ProfileManagerGroupedDocumentation.AutoEllipsis = true;
            this.ProfileManagerGroupedDocumentation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ProfileManagerGroupedDocumentation.ForeColor = System.Drawing.Color.Snow;
            this.ProfileManagerGroupedDocumentation.HooverImage = ((System.Drawing.Image)(resources.GetObject("ProfileManagerGroupedDocumentation.HooverImage")));
            this.ProfileManagerGroupedDocumentation.Image = ((System.Drawing.Image)(resources.GetObject("ProfileManagerGroupedDocumentation.Image")));
            this.ProfileManagerGroupedDocumentation.Location = new System.Drawing.Point(259, 113);
            this.ProfileManagerGroupedDocumentation.Name = "ProfileManagerGroupedDocumentation";
            this.ProfileManagerGroupedDocumentation.Size = new System.Drawing.Size(150, 29);
            this.ProfileManagerGroupedDocumentation.TabIndex = 10;
            this.ProfileManagerGroupedDocumentation.Text = "Grouped Profile Documentation";
            this.ProfileManagerGroupedDocumentation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ProfileManagerGroupedDocumentation.Click += new System.EventHandler(this.ProfileManagerGroupedDocumentation_Click);
            // 
            // ProfileManagerSimpleDocumentation
            // 
            this.ProfileManagerSimpleDocumentation.AutoEllipsis = true;
            this.ProfileManagerSimpleDocumentation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ProfileManagerSimpleDocumentation.ForeColor = System.Drawing.Color.Snow;
            this.ProfileManagerSimpleDocumentation.HooverImage = ((System.Drawing.Image)(resources.GetObject("ProfileManagerSimpleDocumentation.HooverImage")));
            this.ProfileManagerSimpleDocumentation.Image = ((System.Drawing.Image)(resources.GetObject("ProfileManagerSimpleDocumentation.Image")));
            this.ProfileManagerSimpleDocumentation.Location = new System.Drawing.Point(259, 250);
            this.ProfileManagerSimpleDocumentation.Name = "ProfileManagerSimpleDocumentation";
            this.ProfileManagerSimpleDocumentation.Size = new System.Drawing.Size(150, 29);
            this.ProfileManagerSimpleDocumentation.TabIndex = 11;
            this.ProfileManagerSimpleDocumentation.Text = "Simple Profile Documentation";
            this.ProfileManagerSimpleDocumentation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ProfileManagerSimpleDocumentation.Click += new System.EventHandler(this.ProfileManagerSimpleDocumentation_Click);
            // 
            // MainHeader
            // 
            this.MainHeader.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("MainHeader.BackgroundImage")));
            this.MainHeader.Location = new System.Drawing.Point(0, 0);
            this.MainHeader.LogoImage = ((System.Drawing.Image)(resources.GetObject("MainHeader.LogoImage")));
            this.MainHeader.Name = "MainHeader";
            this.MainHeader.Size = new System.Drawing.Size(575, 43);
            this.MainHeader.TabIndex = 12;
            this.MainHeader.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.MainHeader.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.MainHeader.TitleText = "Quester Profile Manager - The Noob Bot DevVersionRestrict";
            // 
            // ProfileManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(575, 335);
            this.Controls.Add(this.MainHeader);
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
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(575, 335);
            this.MinimumSize = new System.Drawing.Size(575, 335);
            this.Name = "ProfileManager";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Quester Profile Management System";
            this.ResumeLayout(false);

        }

        #endregion

        private TnbButton ProfileManagerAddGrouped;
        private System.Windows.Forms.ListBox ExistingGroupedProfiles;
        private TnbButton ProfileManagerAdd;
        private TnbButton ProfileManagerEditGrouped;
        private TnbButton ProfileManagerEdit;
        private System.Windows.Forms.ListBox ExistingSimpleProfiles;
        private TnbButton ProfileManagerRemoveGrouped;
        private Label ProfileManagerGroupedLabel;
        private Label ProfileManagerSimpleLabel;
        private TnbButton ProfileManagerRemove;
        private TnbButton ProfileManagerGroupedDocumentation;
        private TnbButton ProfileManagerSimpleDocumentation;
        private TnbControlMenu MainHeader;
    }
}