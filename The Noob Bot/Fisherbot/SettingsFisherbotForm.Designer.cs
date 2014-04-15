using System.Windows.Forms;
using nManager.Helpful.Forms.UserControls;

namespace Fisherbot
{
    partial class SettingsFisherbotForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsFisherbotForm));
            this.useLure = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.fishSchool = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.UseLureLabel = new System.Windows.Forms.Label();
            this.ActivateFishingHole = new System.Windows.Forms.Label();
            this.fishSchoolProfil = new nManager.Helpful.Forms.UserControls.TnbComboBox();
            this.lureName = new System.Windows.Forms.TextBox();
            this.labelX3 = new System.Windows.Forms.Label();
            this.FishingPoleName = new System.Windows.Forms.Label();
            this.FisherbotPoolName = new System.Windows.Forms.TextBox();
            this.LeftWeaponName = new System.Windows.Forms.Label();
            this.weaponName = new System.Windows.Forms.TextBox();
            this.labelX6 = new System.Windows.Forms.Label();
            this.saveB = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.profileCreatorB = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.labelX7 = new System.Windows.Forms.Label();
            this.precisionMode = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.tnbControlMenu1 = new nManager.Helpful.Forms.UserControls.TnbControlMenu();
            this.FishingSchoolsProfile = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // useLure
            // 
            this.useLure.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.useLure.ForeColor = System.Drawing.Color.Black;
            this.useLure.Location = new System.Drawing.Point(247, 97);
            this.useLure.MaximumSize = new System.Drawing.Size(60, 20);
            this.useLure.MinimumSize = new System.Drawing.Size(60, 20);
            this.useLure.Name = "useLure";
            this.useLure.OffText = "OFF";
            this.useLure.OnText = "ON";
            this.useLure.Size = new System.Drawing.Size(60, 20);
            this.useLure.TabIndex = 2;
            this.useLure.Value = false;
            // 
            // fishSchool
            // 
            this.fishSchool.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.fishSchool.ForeColor = System.Drawing.Color.Black;
            this.fishSchool.Location = new System.Drawing.Point(247, 176);
            this.fishSchool.MaximumSize = new System.Drawing.Size(60, 20);
            this.fishSchool.MinimumSize = new System.Drawing.Size(60, 20);
            this.fishSchool.Name = "fishSchool";
            this.fishSchool.OffText = "OFF";
            this.fishSchool.OnText = "ON";
            this.fishSchool.Size = new System.Drawing.Size(60, 20);
            this.fishSchool.TabIndex = 3;
            this.fishSchool.Value = false;
            // 
            // UseLureLabel
            // 
            this.UseLureLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.UseLureLabel.Location = new System.Drawing.Point(10, 100);
            this.UseLureLabel.Name = "UseLureLabel";
            this.UseLureLabel.Size = new System.Drawing.Size(112, 22);
            this.UseLureLabel.TabIndex = 4;
            this.UseLureLabel.Text = "Use Lure";
            this.UseLureLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ActivateFishingHole
            // 
            this.ActivateFishingHole.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.ActivateFishingHole.Location = new System.Drawing.Point(10, 175);
            this.ActivateFishingHole.Name = "ActivateFishingHole";
            this.ActivateFishingHole.Size = new System.Drawing.Size(201, 22);
            this.ActivateFishingHole.TabIndex = 5;
            this.ActivateFishingHole.Text = "Fish in Fishing Schools";
            this.ActivateFishingHole.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // fishSchoolProfil
            // 
            this.fishSchoolProfil.BackColor = System.Drawing.Color.White;
            this.fishSchoolProfil.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            this.fishSchoolProfil.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.fishSchoolProfil.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fishSchoolProfil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fishSchoolProfil.ForeColor = System.Drawing.Color.Black;
            this.fishSchoolProfil.FormattingEnabled = true;
            this.fishSchoolProfil.HighlightColor = System.Drawing.Color.Gainsboro;
            this.fishSchoolProfil.ItemHeight = 20;
            this.fishSchoolProfil.Location = new System.Drawing.Point(162, 202);
            this.fishSchoolProfil.Name = "fishSchoolProfil";
            this.fishSchoolProfil.SelectorBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(106)))), ((int)(((byte)(194)))));
            this.fishSchoolProfil.SelectorImage = ((System.Drawing.Image)(resources.GetObject("fishSchoolProfil.SelectorImage")));
            this.fishSchoolProfil.Size = new System.Drawing.Size(145, 26);
            this.fishSchoolProfil.TabIndex = 12;
            // 
            // lureName
            // 
            this.lureName.ForeColor = System.Drawing.Color.Black;
            this.lureName.Location = new System.Drawing.Point(130, 121);
            this.lureName.Name = "lureName";
            this.lureName.Size = new System.Drawing.Size(178, 22);
            this.lureName.TabIndex = 13;
            // 
            // labelX3
            // 
            this.labelX3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.labelX3.Location = new System.Drawing.Point(10, 122);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(112, 22);
            this.labelX3.TabIndex = 14;
            this.labelX3.Text = "Lure Name*";
            this.labelX3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FishingPoleName
            // 
            this.FishingPoleName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.FishingPoleName.Location = new System.Drawing.Point(10, 260);
            this.FishingPoleName.Name = "FishingPoleName";
            this.FishingPoleName.Size = new System.Drawing.Size(112, 22);
            this.FishingPoleName.TabIndex = 16;
            this.FishingPoleName.Text = "Fishing Pole Name";
            this.FishingPoleName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FisherbotPoolName
            // 
            this.FisherbotPoolName.ForeColor = System.Drawing.Color.Black;
            this.FisherbotPoolName.Location = new System.Drawing.Point(158, 258);
            this.FisherbotPoolName.Name = "FisherbotPoolName";
            this.FisherbotPoolName.Size = new System.Drawing.Size(151, 22);
            this.FisherbotPoolName.TabIndex = 15;
            // 
            // LeftWeaponName
            // 
            this.LeftWeaponName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.LeftWeaponName.Location = new System.Drawing.Point(10, 288);
            this.LeftWeaponName.Name = "LeftWeaponName";
            this.LeftWeaponName.Size = new System.Drawing.Size(112, 22);
            this.LeftWeaponName.TabIndex = 18;
            this.LeftWeaponName.Text = "Left Weapon Name";
            this.LeftWeaponName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // weaponName
            // 
            this.weaponName.ForeColor = System.Drawing.Color.Black;
            this.weaponName.Location = new System.Drawing.Point(158, 286);
            this.weaponName.Name = "weaponName";
            this.weaponName.Size = new System.Drawing.Size(151, 22);
            this.weaponName.TabIndex = 17;
            // 
            // labelX6
            // 
            this.labelX6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.labelX6.Location = new System.Drawing.Point(10, 144);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(294, 22);
            this.labelX6.TabIndex = 19;
            this.labelX6.Text = "If empty, default items will be used.";
            this.labelX6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // saveB
            // 
            this.saveB.AutoEllipsis = true;
            this.saveB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.saveB.ForeColor = System.Drawing.Color.Snow;
            this.saveB.HooverImage = ((System.Drawing.Image)(resources.GetObject("saveB.HooverImage")));
            this.saveB.Image = global::Fisherbot.Properties.Resources.blueB;
            this.saveB.Location = new System.Drawing.Point(202, 54);
            this.saveB.Name = "saveB";
            this.saveB.Size = new System.Drawing.Size(106, 29);
            this.saveB.TabIndex = 20;
            this.saveB.Text = "Save and Close";
            this.saveB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.saveB.Click += new System.EventHandler(this.saveB_Click);
            // 
            // profileCreatorB
            // 
            this.profileCreatorB.AutoEllipsis = true;
            this.profileCreatorB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.profileCreatorB.ForeColor = System.Drawing.Color.Snow;
            this.profileCreatorB.HooverImage = ((System.Drawing.Image)(resources.GetObject("profileCreatorB.HooverImage")));
            this.profileCreatorB.Image = ((System.Drawing.Image)(resources.GetObject("profileCreatorB.Image")));
            this.profileCreatorB.Location = new System.Drawing.Point(86, 54);
            this.profileCreatorB.Name = "profileCreatorB";
            this.profileCreatorB.Size = new System.Drawing.Size(106, 29);
            this.profileCreatorB.TabIndex = 21;
            this.profileCreatorB.Text = "Profile Creator";
            this.profileCreatorB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.profileCreatorB.Click += new System.EventHandler(this.profileCreatorB_Click);
            // 
            // labelX7
            // 
            this.labelX7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.labelX7.Location = new System.Drawing.Point(10, 236);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(190, 22);
            this.labelX7.TabIndex = 23;
            this.labelX7.Text = "Retry until it land in the Fishing School";
            this.labelX7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // precisionMode
            // 
            this.precisionMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.precisionMode.ForeColor = System.Drawing.Color.Black;
            this.precisionMode.Location = new System.Drawing.Point(247, 233);
            this.precisionMode.MaximumSize = new System.Drawing.Size(60, 20);
            this.precisionMode.MinimumSize = new System.Drawing.Size(60, 20);
            this.precisionMode.Name = "precisionMode";
            this.precisionMode.OffText = "OFF";
            this.precisionMode.OnText = "ON";
            this.precisionMode.Size = new System.Drawing.Size(60, 20);
            this.precisionMode.TabIndex = 22;
            this.precisionMode.Value = false;
            // 
            // tnbControlMenu1
            // 
            this.tnbControlMenu1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tnbControlMenu1.BackgroundImage")));
            this.tnbControlMenu1.Location = new System.Drawing.Point(0, 0);
            this.tnbControlMenu1.LogoImage = ((System.Drawing.Image)(resources.GetObject("tnbControlMenu1.LogoImage")));
            this.tnbControlMenu1.Name = "tnbControlMenu1";
            this.tnbControlMenu1.Size = new System.Drawing.Size(320, 43);
            this.tnbControlMenu1.TabIndex = 24;
            this.tnbControlMenu1.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.tnbControlMenu1.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.tnbControlMenu1.TitleText = "TheNoobBot";
            // 
            // FishingSchoolsProfile
            // 
            this.FishingSchoolsProfile.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.FishingSchoolsProfile.Location = new System.Drawing.Point(10, 204);
            this.FishingSchoolsProfile.Name = "FishingSchoolsProfile";
            this.FishingSchoolsProfile.Size = new System.Drawing.Size(141, 22);
            this.FishingSchoolsProfile.TabIndex = 25;
            this.FishingSchoolsProfile.Text = "Fishing Schools Profile";
            this.FishingSchoolsProfile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SettingsFisherbotForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(320, 321);
            this.Controls.Add(this.FishingSchoolsProfile);
            this.Controls.Add(this.tnbControlMenu1);
            this.Controls.Add(this.labelX7);
            this.Controls.Add(this.precisionMode);
            this.Controls.Add(this.profileCreatorB);
            this.Controls.Add(this.saveB);
            this.Controls.Add(this.labelX6);
            this.Controls.Add(this.LeftWeaponName);
            this.Controls.Add(this.weaponName);
            this.Controls.Add(this.FishingPoleName);
            this.Controls.Add(this.FisherbotPoolName);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.lureName);
            this.Controls.Add(this.fishSchoolProfil);
            this.Controls.Add(this.ActivateFishingHole);
            this.Controls.Add(this.UseLureLabel);
            this.Controls.Add(this.fishSchool);
            this.Controls.Add(this.useLure);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SettingsFisherbotForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings Fisherbot";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TnbSwitchButton useLure;
        private TnbSwitchButton fishSchool;
        private Label UseLureLabel;
        private Label ActivateFishingHole;
        private TnbComboBox fishSchoolProfil;
        private TextBox lureName;
        private Label labelX3;
        private TextBox FisherbotPoolName;
        private Label FishingPoleName;
        private Label LeftWeaponName;
        private TextBox weaponName;
        private Label labelX6;
        private TnbButton saveB;
        private TnbButton profileCreatorB;
        private Label labelX7;
        private TnbSwitchButton precisionMode;
        private TnbControlMenu tnbControlMenu1;
        private Label FishingSchoolsProfile;

    }
}