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
            this.recordWayB = new TnbButton();
            this.SaveButton = new TnbButton();
            this.RecordedPoints = new System.Windows.Forms.ListBox();
            this.DistanceBetweenRecord = new NumericUpDown();
            this.DistanceBetweenRecordLabel = new Label();
            this.DeleteButton = new TnbButton();
            this.DeleteButtonBlackListRadius = new TnbButton();
            this.RecordedBlackListRadius = new System.Windows.Forms.ListBox();
            this.Radius = new NumericUpDown();
            this.AddToBlackList = new TnbButton();
            this.LoadButton = new TnbButton();
            this.CurrentBattleground = new Label();
            this.RefreshCurrentBattleground = new TnbButton();
            this.ZoneList = new System.Windows.Forms.ListBox();
            this.DelZoneButton = new TnbButton();
            this.AddZoneButton = new TnbButton();
            ((System.ComponentModel.ISupportInitialize)(this.DistanceBetweenRecord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Radius)).BeginInit();
            this.SuspendLayout();
            // 
            // recordWayB
            // 
            this.recordWayB.Location = new System.Drawing.Point(12, 284);
            this.recordWayB.Name = "recordWayB";
            this.recordWayB.Size = new System.Drawing.Size(338, 22);
            this.recordWayB.TabIndex = 1;
            this.recordWayB.Text = "Record Way";
            this.recordWayB.Click += new System.EventHandler(this.recordWayB_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(305, 8);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(56, 18);
            this.SaveButton.TabIndex = 2;
            this.SaveButton.Text = "Save";
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // RecordedPoints
            // 
            this.RecordedPoints.BackColor = System.Drawing.Color.White;
            this.RecordedPoints.ForeColor = System.Drawing.Color.Black;
            this.RecordedPoints.FormattingEnabled = true;
            this.RecordedPoints.Location = new System.Drawing.Point(1, 150);
            this.RecordedPoints.Name = "RecordedPoints";
            this.RecordedPoints.Size = new System.Drawing.Size(360, 121);
            this.RecordedPoints.TabIndex = 4;
            // 
            // DistanceBetweenRecord
            // 
            this.DistanceBetweenRecord.ForeColor = System.Drawing.Color.Black;
            this.DistanceBetweenRecord.Location = new System.Drawing.Point(139, 5);
            this.DistanceBetweenRecord.Maximum = 200;
            this.DistanceBetweenRecord.Minimum = 1;
            this.DistanceBetweenRecord.Name = "DistanceBetweenRecord";
            this.DistanceBetweenRecord.Size = new System.Drawing.Size(90, 22);
            this.DistanceBetweenRecord.TabIndex = 5;
            this.DistanceBetweenRecord.Value = 10;
            // 
            // DistanceBetweenRecordLabel
            // 
            this.DistanceBetweenRecordLabel.ForeColor = System.Drawing.Color.Black;
            this.DistanceBetweenRecordLabel.Location = new System.Drawing.Point(1, 4);
            this.DistanceBetweenRecordLabel.Name = "DistanceBetweenRecordLabel";
            this.DistanceBetweenRecordLabel.Size = new System.Drawing.Size(132, 22);
            this.DistanceBetweenRecordLabel.TabIndex = 6;
            this.DistanceBetweenRecordLabel.Text = "Separation distance record:";
            // 
            // DeleteButton
            // 
            this.DeleteButton.Location = new System.Drawing.Point(324, 150);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(36, 22);
            this.DeleteButton.TabIndex = 8;
            this.DeleteButton.Text = "Del";
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // DeleteButtonBlackListRadius
            // 
            this.DeleteButtonBlackListRadius.Location = new System.Drawing.Point(324, 320);
            this.DeleteButtonBlackListRadius.Name = "DeleteButtonBlackListRadius";
            this.DeleteButtonBlackListRadius.Size = new System.Drawing.Size(36, 17);
            this.DeleteButtonBlackListRadius.TabIndex = 10;
            this.DeleteButtonBlackListRadius.Text = "Del";
            this.DeleteButtonBlackListRadius.Click += new System.EventHandler(this.DeleteButtonBlackListRadius_Click);
            // 
            // RecordedBlackListRadius
            // 
            this.RecordedBlackListRadius.BackColor = System.Drawing.Color.White;
            this.RecordedBlackListRadius.ForeColor = System.Drawing.Color.Black;
            this.RecordedBlackListRadius.FormattingEnabled = true;
            this.RecordedBlackListRadius.Location = new System.Drawing.Point(1, 320);
            this.RecordedBlackListRadius.Name = "RecordedBlackListRadius";
            this.RecordedBlackListRadius.Size = new System.Drawing.Size(360, 56);
            this.RecordedBlackListRadius.TabIndex = 9;
            // 
            // Radius
            // 
            this.Radius.ForeColor = System.Drawing.Color.Black;
            this.Radius.Location = new System.Drawing.Point(12, 393);
            this.Radius.Minimum = 1;
            this.Radius.Maximum = 200;
            this.Radius.Name = "Radius";
            this.Radius.Size = new System.Drawing.Size(108, 22);
            this.Radius.TabIndex = 11;
            this.Radius.Value = 35;
            // 
            // AddToBlackList
            // 
            this.AddToBlackList.Location = new System.Drawing.Point(126, 392);
            this.AddToBlackList.Name = "AddToBlackList";
            this.AddToBlackList.Size = new System.Drawing.Size(224, 22);
            this.AddToBlackList.TabIndex = 12;
            this.AddToBlackList.Text = "Add this position to Black list Radius";
            this.AddToBlackList.Click += new System.EventHandler(this.AddToBlackList_Click);
            // 
            // LoadButton
            // 
            this.LoadButton.Location = new System.Drawing.Point(244, 8);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(56, 18);
            this.LoadButton.TabIndex = 18;
            this.LoadButton.Text = "Load";
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // CurrentBattleground
            // 
            this.CurrentBattleground.ForeColor = System.Drawing.Color.Black;
            this.CurrentBattleground.Location = new System.Drawing.Point(1, 32);
            this.CurrentBattleground.Name = "CurrentBattleground";
            this.CurrentBattleground.Size = new System.Drawing.Size(277, 23);
            this.CurrentBattleground.TabIndex = 19;
            this.CurrentBattleground.Text = "You must be in a battle in order to use the Profile Creator.";
            // 
            // RefreshCurrentBattleground
            // 
            this.RefreshCurrentBattleground.Location = new System.Drawing.Point(284, 33);
            this.RefreshCurrentBattleground.Name = "RefreshCurrentBattleground";
            this.RefreshCurrentBattleground.Size = new System.Drawing.Size(76, 21);
            this.RefreshCurrentBattleground.TabIndex = 20;
            this.RefreshCurrentBattleground.Text = "Refresh";
            this.RefreshCurrentBattleground.Click += new System.EventHandler(this.RefreshCurrentBattleground_Click);
            // 
            // ZoneList
            // 
            this.ZoneList.BackColor = System.Drawing.Color.White;
            this.ZoneList.ForeColor = System.Drawing.Color.Black;
            this.ZoneList.FormattingEnabled = true;
            this.ZoneList.Location = new System.Drawing.Point(1, 60);
            this.ZoneList.Name = "ZoneList";
            this.ZoneList.Size = new System.Drawing.Size(360, 69);
            this.ZoneList.TabIndex = 21;
            this.ZoneList.SelectedIndexChanged += new System.EventHandler(this.ZoneList_SelectedIndexChanged);
            // 
            // DelZoneButton
            // 
            this.DelZoneButton.Location = new System.Drawing.Point(325, 60);
            this.DelZoneButton.Name = "DelZoneButton";
            this.DelZoneButton.Size = new System.Drawing.Size(36, 22);
            this.DelZoneButton.TabIndex = 22;
            this.DelZoneButton.Text = "Del";
            this.DelZoneButton.Click += new System.EventHandler(this.DelZoneButton_Click);
            // 
            // AddZoneButton
            // 
            this.AddZoneButton.Location = new System.Drawing.Point(284, 60);
            this.AddZoneButton.Name = "AddZoneButton";
            this.AddZoneButton.Size = new System.Drawing.Size(36, 22);
            this.AddZoneButton.TabIndex = 23;
            this.AddZoneButton.Text = "Add";
            this.AddZoneButton.Click += new System.EventHandler(this.AddZoneButton_Click);
            // 
            // ProfileCreator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(364, 415);
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
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(380, 453);
            this.MinimumSize = new System.Drawing.Size(380, 453);
            this.Name = "ProfileCreator";
            this.ShowIcon = false;
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
    }
}