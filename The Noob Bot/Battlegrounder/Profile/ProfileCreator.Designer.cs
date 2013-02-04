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
            this.recordWayB = new DevComponents.DotNetBar.ButtonX();
            this.SaveButton = new DevComponents.DotNetBar.ButtonX();
            this.RecordedPoints = new System.Windows.Forms.ListBox();
            this.DistanceBetweenRecord = new DevComponents.Editors.IntegerInput();
            this.DistanceBetweenRecordLabel = new DevComponents.DotNetBar.LabelX();
            this.DeleteButton = new DevComponents.DotNetBar.ButtonX();
            this.DeleteButtonBlackListRadius = new DevComponents.DotNetBar.ButtonX();
            this.RecordedBlackListRadius = new System.Windows.Forms.ListBox();
            this.Radius = new DevComponents.Editors.IntegerInput();
            this.AddToBlackList = new DevComponents.DotNetBar.ButtonX();
            this.LoadButton = new DevComponents.DotNetBar.ButtonX();
            this.CurrentBattleground = new DevComponents.DotNetBar.LabelX();
            this.RefreshCurrentBattleground = new DevComponents.DotNetBar.ButtonX();
            this.ZoneList = new System.Windows.Forms.ListBox();
            this.DelZoneButton = new DevComponents.DotNetBar.ButtonX();
            this.AddZoneButton = new DevComponents.DotNetBar.ButtonX();
            ((System.ComponentModel.ISupportInitialize)(this.DistanceBetweenRecord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Radius)).BeginInit();
            this.SuspendLayout();
            // 
            // recordWayB
            // 
            this.recordWayB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.recordWayB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.recordWayB.Location = new System.Drawing.Point(12, 296);
            this.recordWayB.Name = "recordWayB";
            this.recordWayB.Size = new System.Drawing.Size(352, 23);
            this.recordWayB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.recordWayB.TabIndex = 1;
            this.recordWayB.Text = "Record Way";
            this.recordWayB.Click += new System.EventHandler(this.recordWayB_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.SaveButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.SaveButton.Location = new System.Drawing.Point(318, 8);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(58, 19);
            this.SaveButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.SaveButton.TabIndex = 2;
            this.SaveButton.Text = "Save";
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // RecordedPoints
            // 
            this.RecordedPoints.BackColor = System.Drawing.Color.White;
            this.RecordedPoints.ForeColor = System.Drawing.Color.Black;
            this.RecordedPoints.FormattingEnabled = true;
            this.RecordedPoints.Location = new System.Drawing.Point(1, 156);
            this.RecordedPoints.Name = "RecordedPoints";
            this.RecordedPoints.Size = new System.Drawing.Size(375, 134);
            this.RecordedPoints.TabIndex = 4;
            // 
            // DistanceBetweenRecord
            // 
            this.DistanceBetweenRecord.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.DistanceBetweenRecord.BackgroundStyle.Class = "DateTimeInputBackground";
            this.DistanceBetweenRecord.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.DistanceBetweenRecord.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.DistanceBetweenRecord.ForeColor = System.Drawing.Color.Black;
            this.DistanceBetweenRecord.Location = new System.Drawing.Point(145, 5);
            this.DistanceBetweenRecord.MaxValue = 200;
            this.DistanceBetweenRecord.MinValue = 1;
            this.DistanceBetweenRecord.Name = "DistanceBetweenRecord";
            this.DistanceBetweenRecord.ShowUpDown = true;
            this.DistanceBetweenRecord.Size = new System.Drawing.Size(94, 22);
            this.DistanceBetweenRecord.TabIndex = 5;
            this.DistanceBetweenRecord.Value = 10;
            // 
            // DistanceBetweenRecordLabel
            // 
            this.DistanceBetweenRecordLabel.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.DistanceBetweenRecordLabel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.DistanceBetweenRecordLabel.ForeColor = System.Drawing.Color.Black;
            this.DistanceBetweenRecordLabel.Location = new System.Drawing.Point(1, 4);
            this.DistanceBetweenRecordLabel.Name = "DistanceBetweenRecordLabel";
            this.DistanceBetweenRecordLabel.Size = new System.Drawing.Size(138, 23);
            this.DistanceBetweenRecordLabel.TabIndex = 6;
            this.DistanceBetweenRecordLabel.Text = "Separation distance record:";
            // 
            // DeleteButton
            // 
            this.DeleteButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.DeleteButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.DeleteButton.Location = new System.Drawing.Point(338, 156);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(37, 23);
            this.DeleteButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.DeleteButton.TabIndex = 8;
            this.DeleteButton.Text = "Del";
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // DeleteButtonBlackListRadius
            // 
            this.DeleteButtonBlackListRadius.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.DeleteButtonBlackListRadius.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.DeleteButtonBlackListRadius.Location = new System.Drawing.Point(338, 333);
            this.DeleteButtonBlackListRadius.Name = "DeleteButtonBlackListRadius";
            this.DeleteButtonBlackListRadius.Size = new System.Drawing.Size(37, 18);
            this.DeleteButtonBlackListRadius.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.DeleteButtonBlackListRadius.TabIndex = 10;
            this.DeleteButtonBlackListRadius.Text = "Del";
            this.DeleteButtonBlackListRadius.Click += new System.EventHandler(this.DeleteButtonBlackListRadius_Click);
            // 
            // RecordedBlackListRadius
            // 
            this.RecordedBlackListRadius.BackColor = System.Drawing.Color.White;
            this.RecordedBlackListRadius.ForeColor = System.Drawing.Color.Black;
            this.RecordedBlackListRadius.FormattingEnabled = true;
            this.RecordedBlackListRadius.Location = new System.Drawing.Point(1, 333);
            this.RecordedBlackListRadius.Name = "RecordedBlackListRadius";
            this.RecordedBlackListRadius.Size = new System.Drawing.Size(375, 69);
            this.RecordedBlackListRadius.TabIndex = 9;
            // 
            // Radius
            // 
            this.Radius.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.Radius.BackgroundStyle.Class = "DateTimeInputBackground";
            this.Radius.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.Radius.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.Radius.ForeColor = System.Drawing.Color.Black;
            this.Radius.Location = new System.Drawing.Point(12, 409);
            this.Radius.MinValue = 1;
            this.Radius.Name = "Radius";
            this.Radius.ShowUpDown = true;
            this.Radius.Size = new System.Drawing.Size(113, 22);
            this.Radius.TabIndex = 11;
            this.Radius.Value = 35;
            // 
            // AddToBlackList
            // 
            this.AddToBlackList.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.AddToBlackList.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.AddToBlackList.Location = new System.Drawing.Point(131, 408);
            this.AddToBlackList.Name = "AddToBlackList";
            this.AddToBlackList.Size = new System.Drawing.Size(233, 23);
            this.AddToBlackList.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.AddToBlackList.TabIndex = 12;
            this.AddToBlackList.Text = "Add this position to Black list Radius";
            this.AddToBlackList.Click += new System.EventHandler(this.AddToBlackList_Click);
            // 
            // LoadButton
            // 
            this.LoadButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.LoadButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.LoadButton.Location = new System.Drawing.Point(254, 8);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(58, 19);
            this.LoadButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.LoadButton.TabIndex = 18;
            this.LoadButton.Text = "Load";
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // CurrentBattleground
            // 
            this.CurrentBattleground.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.CurrentBattleground.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.CurrentBattleground.ForeColor = System.Drawing.Color.Black;
            this.CurrentBattleground.Location = new System.Drawing.Point(1, 33);
            this.CurrentBattleground.Name = "CurrentBattleground";
            this.CurrentBattleground.Size = new System.Drawing.Size(289, 24);
            this.CurrentBattleground.TabIndex = 19;
            this.CurrentBattleground.Text = "You must be in a battle in order to use the Profile Creator.";
            // 
            // RefreshCurrentBattleground
            // 
            this.RefreshCurrentBattleground.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.RefreshCurrentBattleground.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.RefreshCurrentBattleground.Location = new System.Drawing.Point(296, 34);
            this.RefreshCurrentBattleground.Name = "RefreshCurrentBattleground";
            this.RefreshCurrentBattleground.Size = new System.Drawing.Size(79, 22);
            this.RefreshCurrentBattleground.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.RefreshCurrentBattleground.TabIndex = 20;
            this.RefreshCurrentBattleground.Text = "Refresh";
            this.RefreshCurrentBattleground.Click += new System.EventHandler(this.RefreshCurrentBattleground_Click);
            // 
            // ZoneList
            // 
            this.ZoneList.BackColor = System.Drawing.Color.White;
            this.ZoneList.ForeColor = System.Drawing.Color.Black;
            this.ZoneList.FormattingEnabled = true;
            this.ZoneList.Location = new System.Drawing.Point(1, 63);
            this.ZoneList.Name = "ZoneList";
            this.ZoneList.Size = new System.Drawing.Size(375, 82);
            this.ZoneList.TabIndex = 21;
            this.ZoneList.SelectedIndexChanged += new System.EventHandler(this.ZoneList_SelectedIndexChanged);
            // 
            // DelZoneButton
            // 
            this.DelZoneButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.DelZoneButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.DelZoneButton.Location = new System.Drawing.Point(339, 63);
            this.DelZoneButton.Name = "DelZoneButton";
            this.DelZoneButton.Size = new System.Drawing.Size(37, 23);
            this.DelZoneButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.DelZoneButton.TabIndex = 22;
            this.DelZoneButton.Text = "Del";
            this.DelZoneButton.Click += new System.EventHandler(this.DelZoneButton_Click);
            // 
            // AddZoneButton
            // 
            this.AddZoneButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.AddZoneButton.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.AddZoneButton.Location = new System.Drawing.Point(296, 63);
            this.AddZoneButton.Name = "AddZoneButton";
            this.AddZoneButton.Size = new System.Drawing.Size(37, 23);
            this.AddZoneButton.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.AddZoneButton.TabIndex = 23;
            this.AddZoneButton.Text = "Add";
            this.AddZoneButton.Click += new System.EventHandler(this.AddZoneButton_Click);
            // 
            // ProfileCreator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 432);
            this.Controls.Add(this.AddZoneButton);
            this.Controls.Add(this.DelZoneButton);
            this.Controls.Add(this.ZoneList);
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
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(395, 470);
            this.MinimumSize = new System.Drawing.Size(395, 470);
            this.Name = "ProfileCreator";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Profile Creator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProfileCreator_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.DistanceBetweenRecord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Radius)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX recordWayB;
        private DevComponents.DotNetBar.ButtonX SaveButton;
        private System.Windows.Forms.ListBox RecordedPoints;
        private DevComponents.Editors.IntegerInput DistanceBetweenRecord;
        private DevComponents.DotNetBar.LabelX DistanceBetweenRecordLabel;
        private DevComponents.DotNetBar.ButtonX DeleteButton;
        private DevComponents.DotNetBar.ButtonX DeleteButtonBlackListRadius;
        private System.Windows.Forms.ListBox RecordedBlackListRadius;
        private DevComponents.Editors.IntegerInput Radius;
        private DevComponents.DotNetBar.ButtonX AddToBlackList;
        private DevComponents.DotNetBar.ButtonX LoadButton;
        private DevComponents.DotNetBar.LabelX CurrentBattleground;
        private DevComponents.DotNetBar.ButtonX RefreshCurrentBattleground;
        private System.Windows.Forms.ListBox ZoneList;
        private DevComponents.DotNetBar.ButtonX DelZoneButton;
        private DevComponents.DotNetBar.ButtonX AddZoneButton;
    }
}