using System.Windows.Forms;
using nManager.Helpful.Forms.UserControls;

namespace Fisherbot.Profile
{
    partial class ProfileCreator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProfileCreator));
            this.ListOfPointsRecorded = new System.Windows.Forms.ListBox();
            this.nSeparatorDistance = new System.Windows.Forms.NumericUpDown();
            this.RecordingIntervalDistance = new System.Windows.Forms.Label();
            this.BlacklistRadiusList = new System.Windows.Forms.ListBox();
            this.radiusN = new System.Windows.Forms.NumericUpDown();
            this.MainHeader = new nManager.Helpful.Forms.UserControls.TnbControlMenu();
            this.LoadaProfileButton = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.addBlackB = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.delBlackRadius = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.RemoveListPointButton = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.SaveProfileButton = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.recordWayB = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.BlackListRadius = new System.Windows.Forms.Label();
            this.FishingSchoolFishingLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nSeparatorDistance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radiusN)).BeginInit();
            this.SuspendLayout();
            // 
            // ListOfPointsRecorded
            // 
            this.ListOfPointsRecorded.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ListOfPointsRecorded.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.ListOfPointsRecorded.FormattingEnabled = true;
            this.ListOfPointsRecorded.Location = new System.Drawing.Point(13, 95);
            this.ListOfPointsRecorded.Name = "ListOfPointsRecorded";
            this.ListOfPointsRecorded.Size = new System.Drawing.Size(551, 95);
            this.ListOfPointsRecorded.TabIndex = 4;
            // 
            // nSeparatorDistance
            // 
            this.nSeparatorDistance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.nSeparatorDistance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.nSeparatorDistance.Location = new System.Drawing.Point(191, 199);
            this.nSeparatorDistance.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nSeparatorDistance.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nSeparatorDistance.Name = "nSeparatorDistance";
            this.nSeparatorDistance.Size = new System.Drawing.Size(42, 22);
            this.nSeparatorDistance.TabIndex = 5;
            this.nSeparatorDistance.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // RecordingIntervalDistance
            // 
            this.RecordingIntervalDistance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.RecordingIntervalDistance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.RecordingIntervalDistance.Location = new System.Drawing.Point(10, 192);
            this.RecordingIntervalDistance.Name = "RecordingIntervalDistance";
            this.RecordingIntervalDistance.Size = new System.Drawing.Size(170, 29);
            this.RecordingIntervalDistance.TabIndex = 6;
            this.RecordingIntervalDistance.Text = "Separation distance record:";
            this.RecordingIntervalDistance.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BlacklistRadiusList
            // 
            this.BlacklistRadiusList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.BlacklistRadiusList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.BlacklistRadiusList.FormattingEnabled = true;
            this.BlacklistRadiusList.Location = new System.Drawing.Point(13, 241);
            this.BlacklistRadiusList.Name = "BlacklistRadiusList";
            this.BlacklistRadiusList.Size = new System.Drawing.Size(551, 95);
            this.BlacklistRadiusList.TabIndex = 9;
            // 
            // radiusN
            // 
            this.radiusN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.radiusN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.radiusN.Location = new System.Drawing.Point(191, 344);
            this.radiusN.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.radiusN.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.radiusN.Name = "radiusN";
            this.radiusN.Size = new System.Drawing.Size(42, 22);
            this.radiusN.TabIndex = 11;
            this.radiusN.Value = new decimal(new int[] {
            35,
            0,
            0,
            0});
            // 
            // MainHeader
            // 
            this.MainHeader.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("MainHeader.BackgroundImage")));
            this.MainHeader.Location = new System.Drawing.Point(0, 0);
            this.MainHeader.LogoImage = ((System.Drawing.Image)(resources.GetObject("MainHeader.LogoImage")));
            this.MainHeader.Name = "MainHeader";
            this.MainHeader.Size = new System.Drawing.Size(575, 43);
            this.MainHeader.TabIndex = 19;
            this.MainHeader.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.MainHeader.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.MainHeader.TitleText = "Fisherbot Profile Creator - The Noob Bot DevVersionRestrict";
            // 
            // LoadaProfileButton
            // 
            this.LoadaProfileButton.AutoEllipsis = true;
            this.LoadaProfileButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.LoadaProfileButton.ForeColor = System.Drawing.Color.Snow;
            this.LoadaProfileButton.HooverImage = global::Fisherbot.Properties.Resources.greenB_150;
            this.LoadaProfileButton.Image = global::Fisherbot.Properties.Resources.blackB_150;
            this.LoadaProfileButton.Location = new System.Drawing.Point(413, 54);
            this.LoadaProfileButton.Name = "LoadaProfileButton";
            this.LoadaProfileButton.Size = new System.Drawing.Size(150, 29);
            this.LoadaProfileButton.TabIndex = 18;
            this.LoadaProfileButton.Text = "Load an existing profile";
            this.LoadaProfileButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LoadaProfileButton.Click += new System.EventHandler(this.loadB_Click);
            // 
            // addBlackB
            // 
            this.addBlackB.AutoEllipsis = true;
            this.addBlackB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.addBlackB.ForeColor = System.Drawing.Color.Snow;
            this.addBlackB.HooverImage = global::Fisherbot.Properties.Resources.greenB_260;
            this.addBlackB.Image = global::Fisherbot.Properties.Resources.blackB_260;
            this.addBlackB.Location = new System.Drawing.Point(303, 346);
            this.addBlackB.Name = "addBlackB";
            this.addBlackB.Size = new System.Drawing.Size(260, 29);
            this.addBlackB.TabIndex = 12;
            this.addBlackB.Text = "Add this position to Black list Radius";
            this.addBlackB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.addBlackB.Click += new System.EventHandler(this.addBlackB_Click);
            // 
            // delBlackRadius
            // 
            this.delBlackRadius.AutoEllipsis = true;
            this.delBlackRadius.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.delBlackRadius.ForeColor = System.Drawing.Color.Snow;
            this.delBlackRadius.HooverImage = global::Fisherbot.Properties.Resources.greenB_70;
            this.delBlackRadius.Image = global::Fisherbot.Properties.Resources.blackB_70;
            this.delBlackRadius.Location = new System.Drawing.Point(493, 242);
            this.delBlackRadius.Name = "delBlackRadius";
            this.delBlackRadius.Size = new System.Drawing.Size(70, 29);
            this.delBlackRadius.TabIndex = 10;
            this.delBlackRadius.Text = "Del";
            this.delBlackRadius.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.delBlackRadius.Click += new System.EventHandler(this.delBlackRadius_Click);
            // 
            // RemoveListPointButton
            // 
            this.RemoveListPointButton.AutoEllipsis = true;
            this.RemoveListPointButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.RemoveListPointButton.ForeColor = System.Drawing.Color.Snow;
            this.RemoveListPointButton.HooverImage = global::Fisherbot.Properties.Resources.greenB_70;
            this.RemoveListPointButton.Image = global::Fisherbot.Properties.Resources.blackB_70;
            this.RemoveListPointButton.Location = new System.Drawing.Point(493, 96);
            this.RemoveListPointButton.Name = "RemoveListPointButton";
            this.RemoveListPointButton.Size = new System.Drawing.Size(70, 29);
            this.RemoveListPointButton.TabIndex = 8;
            this.RemoveListPointButton.Text = "Del";
            this.RemoveListPointButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.RemoveListPointButton.Click += new System.EventHandler(this.delB_Click);
            // 
            // SaveProfileButton
            // 
            this.SaveProfileButton.AutoEllipsis = true;
            this.SaveProfileButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.SaveProfileButton.ForeColor = System.Drawing.Color.Snow;
            this.SaveProfileButton.HooverImage = ((System.Drawing.Image)(resources.GetObject("SaveProfileButton.HooverImage")));
            this.SaveProfileButton.Image = global::Fisherbot.Properties.Resources.blueB;
            this.SaveProfileButton.Location = new System.Drawing.Point(457, 385);
            this.SaveProfileButton.Name = "SaveProfileButton";
            this.SaveProfileButton.Size = new System.Drawing.Size(106, 29);
            this.SaveProfileButton.TabIndex = 2;
            this.SaveProfileButton.Text = "Save";
            this.SaveProfileButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.SaveProfileButton.Click += new System.EventHandler(this.saveB_Click);
            // 
            // recordWayB
            // 
            this.recordWayB.AutoEllipsis = true;
            this.recordWayB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.recordWayB.ForeColor = System.Drawing.Color.Snow;
            this.recordWayB.HooverImage = global::Fisherbot.Properties.Resources.greenB_260;
            this.recordWayB.Image = global::Fisherbot.Properties.Resources.blackB_260;
            this.recordWayB.Location = new System.Drawing.Point(303, 201);
            this.recordWayB.Name = "recordWayB";
            this.recordWayB.Size = new System.Drawing.Size(260, 29);
            this.recordWayB.TabIndex = 1;
            this.recordWayB.Text = "Record Way";
            this.recordWayB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.recordWayB.Click += new System.EventHandler(this.recordWayB_Click);
            // 
            // BlackListRadius
            // 
            this.BlackListRadius.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.BlackListRadius.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.BlackListRadius.Location = new System.Drawing.Point(10, 338);
            this.BlackListRadius.Name = "BlackListRadius";
            this.BlackListRadius.Size = new System.Drawing.Size(170, 29);
            this.BlackListRadius.TabIndex = 20;
            this.BlackListRadius.Text = "BlackList Radius";
            this.BlackListRadius.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FishingSchoolFishingLabel
            // 
            this.FishingSchoolFishingLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FishingSchoolFishingLabel.Location = new System.Drawing.Point(17, 55);
            this.FishingSchoolFishingLabel.Name = "FishingSchoolFishingLabel";
            this.FishingSchoolFishingLabel.Size = new System.Drawing.Size(390, 37);
            this.FishingSchoolFishingLabel.TabIndex = 21;
            this.FishingSchoolFishingLabel.Text = "It\'s only necessary to create a profile if you want to fish in Fishing Holes, oth" +
                "erwise, just run the FisherBot without any profile.";
            // 
            // ProfileCreator
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(575, 426);
            this.Controls.Add(this.FishingSchoolFishingLabel);
            this.Controls.Add(this.BlackListRadius);
            this.Controls.Add(this.MainHeader);
            this.Controls.Add(this.LoadaProfileButton);
            this.Controls.Add(this.addBlackB);
            this.Controls.Add(this.radiusN);
            this.Controls.Add(this.delBlackRadius);
            this.Controls.Add(this.BlacklistRadiusList);
            this.Controls.Add(this.RemoveListPointButton);
            this.Controls.Add(this.RecordingIntervalDistance);
            this.Controls.Add(this.nSeparatorDistance);
            this.Controls.Add(this.ListOfPointsRecorded);
            this.Controls.Add(this.SaveProfileButton);
            this.Controls.Add(this.recordWayB);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ProfileCreator";
            
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Profile Creator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProfileCreator_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.nSeparatorDistance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radiusN)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TnbButton recordWayB;
        private TnbButton SaveProfileButton;
        private System.Windows.Forms.ListBox ListOfPointsRecorded;
        private NumericUpDown nSeparatorDistance;
        private Label RecordingIntervalDistance;
        private TnbButton RemoveListPointButton;
        private TnbButton delBlackRadius;
        private System.Windows.Forms.ListBox BlacklistRadiusList;
        private NumericUpDown radiusN;
        private TnbButton addBlackB;
        private TnbButton LoadaProfileButton;
        private TnbControlMenu MainHeader;
        private Label BlackListRadius;
        private Label FishingSchoolFishingLabel;
    }
}