using System.Windows.Forms;
using nManager.Helpful.Forms.UserControls;

namespace Battlegrounder.Profile
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
            this.recordWayB = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.SaveButton = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.RecordedPoints = new System.Windows.Forms.ListBox();
            this.DistanceBetweenRecord = new System.Windows.Forms.NumericUpDown();
            this.DistanceBetweenRecordLabel = new System.Windows.Forms.Label();
            this.DeleteButton = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.DeleteButtonBlackListRadius = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.RecordedBlackListRadius = new System.Windows.Forms.ListBox();
            this.Radius = new System.Windows.Forms.NumericUpDown();
            this.AddToBlackList = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.LoadButton = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.CurrentBattleground = new System.Windows.Forms.Label();
            this.RefreshCurrentBattleground = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.ZoneList = new System.Windows.Forms.ListBox();
            this.DelZoneButton = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.AddZoneButton = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.label1 = new System.Windows.Forms.Label();
            this.MainHeader = new nManager.Helpful.Forms.UserControls.TnbControlMenu();
            ((System.ComponentModel.ISupportInitialize)(this.DistanceBetweenRecord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Radius)).BeginInit();
            this.SuspendLayout();
            // 
            // recordWayB
            // 
            this.recordWayB.AutoEllipsis = true;
            this.recordWayB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.recordWayB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.recordWayB.HooverImage = global::Battlegrounder.Properties.Resources.greenB_260;
            this.recordWayB.Image = global::Battlegrounder.Properties.Resources.blackB_260;
            this.recordWayB.Location = new System.Drawing.Point(303, 201);
            this.recordWayB.Name = "recordWayB";
            this.recordWayB.Size = new System.Drawing.Size(260, 29);
            this.recordWayB.TabIndex = 1;
            this.recordWayB.Text = "Record Way";
            this.recordWayB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.recordWayB.Click += new System.EventHandler(this.recordWayB_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.AutoEllipsis = true;
            this.SaveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.SaveButton.ForeColor = System.Drawing.Color.Snow;
            this.SaveButton.HooverImage = ((System.Drawing.Image)(resources.GetObject("SaveButton.HooverImage")));
            this.SaveButton.Image = global::Battlegrounder.Properties.Resources.blueB;
            this.SaveButton.Location = new System.Drawing.Point(457, 493);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(106, 29);
            this.SaveButton.TabIndex = 2;
            this.SaveButton.Text = "Save";
            this.SaveButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // RecordedPoints
            // 
            this.RecordedPoints.BackColor = System.Drawing.Color.White;
            this.RecordedPoints.ForeColor = System.Drawing.Color.Black;
            this.RecordedPoints.FormattingEnabled = true;
            this.RecordedPoints.Location = new System.Drawing.Point(13, 241);
            this.RecordedPoints.Name = "RecordedPoints";
            this.RecordedPoints.Size = new System.Drawing.Size(551, 95);
            this.RecordedPoints.TabIndex = 4;
            // 
            // DistanceBetweenRecord
            // 
            this.DistanceBetweenRecord.ForeColor = System.Drawing.Color.Black;
            this.DistanceBetweenRecord.Location = new System.Drawing.Point(181, 199);
            this.DistanceBetweenRecord.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.DistanceBetweenRecord.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.DistanceBetweenRecord.Name = "DistanceBetweenRecord";
            this.DistanceBetweenRecord.Size = new System.Drawing.Size(42, 22);
            this.DistanceBetweenRecord.TabIndex = 5;
            this.DistanceBetweenRecord.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // DistanceBetweenRecordLabel
            // 
            this.DistanceBetweenRecordLabel.AutoEllipsis = true;
            this.DistanceBetweenRecordLabel.ForeColor = System.Drawing.Color.Black;
            this.DistanceBetweenRecordLabel.Location = new System.Drawing.Point(10, 192);
            this.DistanceBetweenRecordLabel.Name = "DistanceBetweenRecordLabel";
            this.DistanceBetweenRecordLabel.Size = new System.Drawing.Size(170, 29);
            this.DistanceBetweenRecordLabel.TabIndex = 6;
            this.DistanceBetweenRecordLabel.Text = "Separation distance record:";
            this.DistanceBetweenRecordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DeleteButton
            // 
            this.DeleteButton.AutoEllipsis = true;
            this.DeleteButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.DeleteButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.DeleteButton.HooverImage = global::Battlegrounder.Properties.Resources.greenB_70;
            this.DeleteButton.Image = global::Battlegrounder.Properties.Resources.blackB_70;
            this.DeleteButton.Location = new System.Drawing.Point(493, 242);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(70, 29);
            this.DeleteButton.TabIndex = 8;
            this.DeleteButton.Text = "Del";
            this.DeleteButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // DeleteButtonBlackListRadius
            // 
            this.DeleteButtonBlackListRadius.AutoEllipsis = true;
            this.DeleteButtonBlackListRadius.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.DeleteButtonBlackListRadius.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.DeleteButtonBlackListRadius.HooverImage = global::Battlegrounder.Properties.Resources.greenB_70;
            this.DeleteButtonBlackListRadius.Image = global::Battlegrounder.Properties.Resources.blackB_70;
            this.DeleteButtonBlackListRadius.Location = new System.Drawing.Point(493, 388);
            this.DeleteButtonBlackListRadius.Name = "DeleteButtonBlackListRadius";
            this.DeleteButtonBlackListRadius.Size = new System.Drawing.Size(70, 29);
            this.DeleteButtonBlackListRadius.TabIndex = 10;
            this.DeleteButtonBlackListRadius.Text = "Del";
            this.DeleteButtonBlackListRadius.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.DeleteButtonBlackListRadius.Click += new System.EventHandler(this.DeleteButtonBlackListRadius_Click);
            // 
            // RecordedBlackListRadius
            // 
            this.RecordedBlackListRadius.BackColor = System.Drawing.Color.White;
            this.RecordedBlackListRadius.ForeColor = System.Drawing.Color.Black;
            this.RecordedBlackListRadius.FormattingEnabled = true;
            this.RecordedBlackListRadius.Location = new System.Drawing.Point(13, 387);
            this.RecordedBlackListRadius.Name = "RecordedBlackListRadius";
            this.RecordedBlackListRadius.Size = new System.Drawing.Size(551, 95);
            this.RecordedBlackListRadius.TabIndex = 9;
            // 
            // Radius
            // 
            this.Radius.ForeColor = System.Drawing.Color.Black;
            this.Radius.Location = new System.Drawing.Point(115, 345);
            this.Radius.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.Radius.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Radius.Name = "Radius";
            this.Radius.Size = new System.Drawing.Size(42, 22);
            this.Radius.TabIndex = 11;
            this.Radius.Value = new decimal(new int[] {
            35,
            0,
            0,
            0});
            // 
            // AddToBlackList
            // 
            this.AddToBlackList.AutoEllipsis = true;
            this.AddToBlackList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.AddToBlackList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.AddToBlackList.HooverImage = global::Battlegrounder.Properties.Resources.greenB_260;
            this.AddToBlackList.Image = global::Battlegrounder.Properties.Resources.blackB_260;
            this.AddToBlackList.Location = new System.Drawing.Point(303, 347);
            this.AddToBlackList.Name = "AddToBlackList";
            this.AddToBlackList.Size = new System.Drawing.Size(260, 29);
            this.AddToBlackList.TabIndex = 12;
            this.AddToBlackList.Text = "Add this position to Black list Radius";
            this.AddToBlackList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.AddToBlackList.Click += new System.EventHandler(this.AddToBlackList_Click);
            // 
            // LoadButton
            // 
            this.LoadButton.AutoEllipsis = true;
            this.LoadButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.LoadButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.LoadButton.HooverImage = global::Battlegrounder.Properties.Resources.greenB_150;
            this.LoadButton.Image = global::Battlegrounder.Properties.Resources.blackB_150;
            this.LoadButton.Location = new System.Drawing.Point(413, 54);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(150, 29);
            this.LoadButton.TabIndex = 18;
            this.LoadButton.Text = "Load";
            this.LoadButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // CurrentBattleground
            // 
            this.CurrentBattleground.AutoEllipsis = true;
            this.CurrentBattleground.ForeColor = System.Drawing.Color.Black;
            this.CurrentBattleground.Location = new System.Drawing.Point(13, 54);
            this.CurrentBattleground.Name = "CurrentBattleground";
            this.CurrentBattleground.Size = new System.Drawing.Size(386, 29);
            this.CurrentBattleground.TabIndex = 19;
            this.CurrentBattleground.Text = "You must be in a battle in order to use the Profile Creator.";
            this.CurrentBattleground.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RefreshCurrentBattleground
            // 
            this.RefreshCurrentBattleground.AutoEllipsis = true;
            this.RefreshCurrentBattleground.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.RefreshCurrentBattleground.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.RefreshCurrentBattleground.HooverImage = global::Battlegrounder.Properties.Resources.greenB_70;
            this.RefreshCurrentBattleground.Image = global::Battlegrounder.Properties.Resources.blackB_70;
            this.RefreshCurrentBattleground.Location = new System.Drawing.Point(329, 96);
            this.RefreshCurrentBattleground.Name = "RefreshCurrentBattleground";
            this.RefreshCurrentBattleground.Size = new System.Drawing.Size(70, 29);
            this.RefreshCurrentBattleground.TabIndex = 20;
            this.RefreshCurrentBattleground.Text = "Refresh";
            this.RefreshCurrentBattleground.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.RefreshCurrentBattleground.Click += new System.EventHandler(this.RefreshCurrentBattleground_Click);
            // 
            // ZoneList
            // 
            this.ZoneList.BackColor = System.Drawing.Color.White;
            this.ZoneList.ForeColor = System.Drawing.Color.Black;
            this.ZoneList.FormattingEnabled = true;
            this.ZoneList.Location = new System.Drawing.Point(13, 95);
            this.ZoneList.Name = "ZoneList";
            this.ZoneList.Size = new System.Drawing.Size(551, 95);
            this.ZoneList.TabIndex = 21;
            this.ZoneList.SelectedIndexChanged += new System.EventHandler(this.ZoneList_SelectedIndexChanged);
            // 
            // DelZoneButton
            // 
            this.DelZoneButton.AutoEllipsis = true;
            this.DelZoneButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.DelZoneButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.DelZoneButton.HooverImage = global::Battlegrounder.Properties.Resources.greenB_70;
            this.DelZoneButton.Image = global::Battlegrounder.Properties.Resources.blackB_70;
            this.DelZoneButton.Location = new System.Drawing.Point(493, 96);
            this.DelZoneButton.Name = "DelZoneButton";
            this.DelZoneButton.Size = new System.Drawing.Size(70, 29);
            this.DelZoneButton.TabIndex = 22;
            this.DelZoneButton.Text = "Del";
            this.DelZoneButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.DelZoneButton.Click += new System.EventHandler(this.DelZoneButton_Click);
            // 
            // AddZoneButton
            // 
            this.AddZoneButton.AutoEllipsis = true;
            this.AddZoneButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.AddZoneButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.AddZoneButton.HooverImage = global::Battlegrounder.Properties.Resources.greenB_70;
            this.AddZoneButton.Image = global::Battlegrounder.Properties.Resources.blackB_70;
            this.AddZoneButton.Location = new System.Drawing.Point(411, 96);
            this.AddZoneButton.Name = "AddZoneButton";
            this.AddZoneButton.Size = new System.Drawing.Size(70, 29);
            this.AddZoneButton.TabIndex = 23;
            this.AddZoneButton.Text = "Add";
            this.AddZoneButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.AddZoneButton.Click += new System.EventHandler(this.AddZoneButton_Click);
            // 
            // label1
            // 
            this.label1.AutoEllipsis = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(10, 338);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 29);
            this.label1.TabIndex = 24;
            this.label1.Text = "BlackList Radius :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MainHeader
            // 
            this.MainHeader.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("MainHeader.BackgroundImage")));
            this.MainHeader.Location = new System.Drawing.Point(0, 0);
            this.MainHeader.LogoImage = ((System.Drawing.Image)(resources.GetObject("MainHeader.LogoImage")));
            this.MainHeader.Name = "MainHeader";
            this.MainHeader.Size = new System.Drawing.Size(575, 43);
            this.MainHeader.TabIndex = 25;
            this.MainHeader.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.MainHeader.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.MainHeader.TitleText = "Profile Creator - The Noob Bot DevVersionRestrict";
            // 
            // ProfileCreator
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(575, 534);
            this.Controls.Add(this.MainHeader);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AddZoneButton);
            this.Controls.Add(this.DelZoneButton);
            this.Controls.Add(this.RefreshCurrentBattleground);
            this.Controls.Add(this.CurrentBattleground);
            this.Controls.Add(this.LoadButton);
            this.Controls.Add(this.AddToBlackList);
            this.Controls.Add(this.Radius);
            this.Controls.Add(this.DeleteButtonBlackListRadius);
            this.Controls.Add(this.RecordedBlackListRadius);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.DistanceBetweenRecordLabel);
            this.Controls.Add(this.DistanceBetweenRecord);
            this.Controls.Add(this.RecordedPoints);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.recordWayB);
            this.Controls.Add(this.ZoneList);
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
            ((System.ComponentModel.ISupportInitialize)(this.DistanceBetweenRecord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Radius)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TnbButton recordWayB;
        private TnbButton SaveButton;
        private System.Windows.Forms.ListBox RecordedPoints;
        private NumericUpDown DistanceBetweenRecord;
        private Label DistanceBetweenRecordLabel;
        private TnbButton DeleteButton;
        private TnbButton DeleteButtonBlackListRadius;
        private System.Windows.Forms.ListBox RecordedBlackListRadius;
        private NumericUpDown Radius;
        private TnbButton AddToBlackList;
        private TnbButton LoadButton;
        private Label CurrentBattleground;
        private TnbButton RefreshCurrentBattleground;
        private System.Windows.Forms.ListBox ZoneList;
        private TnbButton DelZoneButton;
        private TnbButton AddZoneButton;
        private Label label1;
        private TnbControlMenu MainHeader;
    }
}