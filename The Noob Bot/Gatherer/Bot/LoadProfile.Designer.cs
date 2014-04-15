using System.Windows.Forms;
using nManager.Helpful.Forms.UserControls;

namespace Gatherer.Bot
{
    partial class LoadProfile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadProfile));
            this.SelectProfileLabel = new System.Windows.Forms.Label();
            this.MainHeader = new nManager.Helpful.Forms.UserControls.TnbControlMenu();
            this.ProfileCreatorButton = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.LoadProfileButton = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.ProfileList = new nManager.Helpful.Forms.UserControls.TnbComboBox();
            this.SuspendLayout();
            // 
            // SelectProfileLabel
            // 
            this.SelectProfileLabel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectProfileLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.SelectProfileLabel.Location = new System.Drawing.Point(8, 58);
            this.SelectProfileLabel.Name = "SelectProfileLabel";
            this.SelectProfileLabel.Size = new System.Drawing.Size(177, 22);
            this.SelectProfileLabel.TabIndex = 2;
            this.SelectProfileLabel.Text = "Select a profile :";
            // 
            // MainHeader
            // 
            this.MainHeader.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("MainHeader.BackgroundImage")));
            this.MainHeader.Location = new System.Drawing.Point(0, 0);
            this.MainHeader.LogoImage = ((System.Drawing.Image)(resources.GetObject("MainHeader.LogoImage")));
            this.MainHeader.Name = "MainHeader";
            this.MainHeader.Size = new System.Drawing.Size(320, 43);
            this.MainHeader.TabIndex = 4;
            this.MainHeader.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.MainHeader.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.MainHeader.TitleText = "Load Profile";
            // 
            // ProfileCreatorButton
            // 
            this.ProfileCreatorButton.AutoEllipsis = true;
            this.ProfileCreatorButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ProfileCreatorButton.ForeColor = System.Drawing.Color.Snow;
            this.ProfileCreatorButton.HooverImage = ((System.Drawing.Image)(resources.GetObject("ProfileCreatorButton.HooverImage")));
            this.ProfileCreatorButton.Image = ((System.Drawing.Image)(resources.GetObject("ProfileCreatorButton.Image")));
            this.ProfileCreatorButton.Location = new System.Drawing.Point(205, 55);
            this.ProfileCreatorButton.Name = "ProfileCreatorButton";
            this.ProfileCreatorButton.Size = new System.Drawing.Size(106, 29);
            this.ProfileCreatorButton.TabIndex = 3;
            this.ProfileCreatorButton.Text = "Profile creator";
            this.ProfileCreatorButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ProfileCreatorButton.Click += new System.EventHandler(this.ProfileCreator_Click);
            // 
            // LoadProfileButton
            // 
            this.LoadProfileButton.AutoEllipsis = true;
            this.LoadProfileButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.LoadProfileButton.ForeColor = System.Drawing.Color.Snow;
            this.LoadProfileButton.HooverImage = ((System.Drawing.Image)(resources.GetObject("LoadProfileButton.HooverImage")));
            this.LoadProfileButton.Image = ((System.Drawing.Image)(resources.GetObject("LoadProfileButton.Image")));
            this.LoadProfileButton.Location = new System.Drawing.Point(99, 131);
            this.LoadProfileButton.Name = "LoadProfileButton";
            this.LoadProfileButton.Size = new System.Drawing.Size(106, 29);
            this.LoadProfileButton.TabIndex = 1;
            this.LoadProfileButton.Text = "Load Profile";
            this.LoadProfileButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LoadProfileButton.Click += new System.EventHandler(this.loadProfileB_Click);
            // 
            // ProfileList
            // 
            this.ProfileList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.ProfileList.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            this.ProfileList.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.ProfileList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ProfileList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ProfileList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(160)))), ((int)(((byte)(229)))));
            this.ProfileList.FormattingEnabled = true;
            this.ProfileList.HighlightColor = System.Drawing.Color.Gainsboro;
            this.ProfileList.ItemHeight = 16;
            this.ProfileList.Location = new System.Drawing.Point(12, 98);
            this.ProfileList.Name = "ProfileList";
            this.ProfileList.SelectorBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(106)))), ((int)(((byte)(194)))));
            this.ProfileList.SelectorImage = ((System.Drawing.Image)(resources.GetObject("ProfileList.SelectorImage")));
            this.ProfileList.Size = new System.Drawing.Size(296, 22);
            this.ProfileList.TabIndex = 0;
            // 
            // LoadProfile
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.BackgroundImage = global::Gatherer.Properties.Resources.backgroundLoadProduct;
            this.ClientSize = new System.Drawing.Size(320, 172);
            this.Controls.Add(this.MainHeader);
            this.Controls.Add(this.ProfileCreatorButton);
            this.Controls.Add(this.SelectProfileLabel);
            this.Controls.Add(this.LoadProfileButton);
            this.Controls.Add(this.ProfileList);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "LoadProfile";
            this.Padding = new System.Windows.Forms.Padding(12);
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Load Grinder Profile";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoadProfile_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private TnbButton LoadProfileButton;
        private TnbComboBox ProfileList;
        private Label SelectProfileLabel;
        private TnbButton ProfileCreatorButton;
        private TnbControlMenu MainHeader;
    }
}