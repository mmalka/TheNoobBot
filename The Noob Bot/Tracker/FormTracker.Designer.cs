using System.Windows.Forms;
using nManager.Helpful.Forms.UserControls;

namespace Tracker
{
    partial class FormTracker
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTracker));
            this.CreatureTrackableList = new System.Windows.Forms.ListBox();
            this.ObjectTrackableList = new System.Windows.Forms.ListBox();
            this.unNoTrack = new System.Windows.Forms.TextBox();
            this.CreatureTrackedList = new System.Windows.Forms.ListBox();
            this.ObjectTrackedList = new System.Windows.Forms.ListBox();
            this.TrackedByNameList = new System.Windows.Forms.ListBox();
            this.unAddTrack = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.unDelTrack = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.otDelTrack = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.otAddTrack = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.ctDelTrack = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.ctAddTrack = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.TrackByNPCNameLabel = new System.Windows.Forms.Label();
            this.TrackByObjectTypeLabel = new System.Windows.Forms.Label();
            this.TrackByCreatureTypeLabel = new System.Windows.Forms.Label();
            this.FormTrackerTimer = new System.Windows.Forms.Timer(this.components);
            this.MainHeader = new nManager.Helpful.Forms.UserControls.TnbControlMenu();
            this.SuspendLayout();
            // 
            // CreatureTrackableList
            // 
            this.CreatureTrackableList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.CreatureTrackableList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.CreatureTrackableList.FormattingEnabled = true;
            this.CreatureTrackableList.Location = new System.Drawing.Point(12, 66);
            this.CreatureTrackableList.Name = "CreatureTrackableList";
            this.CreatureTrackableList.Size = new System.Drawing.Size(128, 82);
            this.CreatureTrackableList.TabIndex = 0;
            // 
            // ObjectTrackableList
            // 
            this.ObjectTrackableList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ObjectTrackableList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.ObjectTrackableList.FormattingEnabled = true;
            this.ObjectTrackableList.Location = new System.Drawing.Point(12, 187);
            this.ObjectTrackableList.Name = "ObjectTrackableList";
            this.ObjectTrackableList.Size = new System.Drawing.Size(128, 82);
            this.ObjectTrackableList.TabIndex = 1;
            // 
            // unNoTrack
            // 
            this.unNoTrack.ForeColor = System.Drawing.Color.Black;
            this.unNoTrack.Location = new System.Drawing.Point(12, 315);
            this.unNoTrack.Name = "unNoTrack";
            this.unNoTrack.Size = new System.Drawing.Size(128, 22);
            this.unNoTrack.TabIndex = 2;
            // 
            // CreatureTrackedList
            // 
            this.CreatureTrackedList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.CreatureTrackedList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.CreatureTrackedList.FormattingEnabled = true;
            this.CreatureTrackedList.Location = new System.Drawing.Point(180, 66);
            this.CreatureTrackedList.Name = "CreatureTrackedList";
            this.CreatureTrackedList.Size = new System.Drawing.Size(128, 82);
            this.CreatureTrackedList.TabIndex = 3;
            // 
            // ObjectTrackedList
            // 
            this.ObjectTrackedList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ObjectTrackedList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.ObjectTrackedList.FormattingEnabled = true;
            this.ObjectTrackedList.Location = new System.Drawing.Point(180, 187);
            this.ObjectTrackedList.Name = "ObjectTrackedList";
            this.ObjectTrackedList.Size = new System.Drawing.Size(128, 82);
            this.ObjectTrackedList.TabIndex = 4;
            // 
            // TrackedByNameList
            // 
            this.TrackedByNameList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.TrackedByNameList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.TrackedByNameList.FormattingEnabled = true;
            this.TrackedByNameList.Location = new System.Drawing.Point(180, 296);
            this.TrackedByNameList.Name = "TrackedByNameList";
            this.TrackedByNameList.Size = new System.Drawing.Size(128, 56);
            this.TrackedByNameList.TabIndex = 5;
            // 
            // unAddTrack
            // 
            this.unAddTrack.AutoEllipsis = true;
            this.unAddTrack.Font = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.unAddTrack.ForeColor = System.Drawing.Color.Snow;
            this.unAddTrack.HooverImage = ((System.Drawing.Image)(resources.GetObject("unAddTrack.HooverImage")));
            this.unAddTrack.Image = ((System.Drawing.Image)(resources.GetObject("unAddTrack.Image")));
            this.unAddTrack.Location = new System.Drawing.Point(146, 305);
            this.unAddTrack.Name = "unAddTrack";
            this.unAddTrack.Size = new System.Drawing.Size(28, 20);
            this.unAddTrack.TabIndex = 6;
            this.unAddTrack.Text = ">";
            this.unAddTrack.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.unAddTrack.Click += new System.EventHandler(this.unAddTrack_Click);
            // 
            // unDelTrack
            // 
            this.unDelTrack.AutoEllipsis = true;
            this.unDelTrack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.unDelTrack.Font = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.unDelTrack.ForeColor = System.Drawing.Color.Snow;
            this.unDelTrack.HooverImage = ((System.Drawing.Image)(resources.GetObject("unDelTrack.HooverImage")));
            this.unDelTrack.Image = ((System.Drawing.Image)(resources.GetObject("unDelTrack.Image")));
            this.unDelTrack.Location = new System.Drawing.Point(146, 332);
            this.unDelTrack.Name = "unDelTrack";
            this.unDelTrack.Size = new System.Drawing.Size(28, 20);
            this.unDelTrack.TabIndex = 16;
            this.unDelTrack.Text = "<";
            this.unDelTrack.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.unDelTrack.Click += new System.EventHandler(this.unDelTrack_Click);
            // 
            // otDelTrack
            // 
            this.otDelTrack.AutoEllipsis = true;
            this.otDelTrack.Font = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.otDelTrack.ForeColor = System.Drawing.Color.Snow;
            this.otDelTrack.HooverImage = ((System.Drawing.Image)(resources.GetObject("otDelTrack.HooverImage")));
            this.otDelTrack.Image = ((System.Drawing.Image)(resources.GetObject("otDelTrack.Image")));
            this.otDelTrack.Location = new System.Drawing.Point(146, 231);
            this.otDelTrack.Name = "otDelTrack";
            this.otDelTrack.Size = new System.Drawing.Size(28, 20);
            this.otDelTrack.TabIndex = 9;
            this.otDelTrack.Text = "<";
            this.otDelTrack.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.otDelTrack.Click += new System.EventHandler(this.otDelTrack_Click);
            // 
            // otAddTrack
            // 
            this.otAddTrack.AutoEllipsis = true;
            this.otAddTrack.Font = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.otAddTrack.ForeColor = System.Drawing.Color.Snow;
            this.otAddTrack.HooverImage = ((System.Drawing.Image)(resources.GetObject("otAddTrack.HooverImage")));
            this.otAddTrack.Image = ((System.Drawing.Image)(resources.GetObject("otAddTrack.Image")));
            this.otAddTrack.Location = new System.Drawing.Point(146, 204);
            this.otAddTrack.Name = "otAddTrack";
            this.otAddTrack.Size = new System.Drawing.Size(28, 20);
            this.otAddTrack.TabIndex = 8;
            this.otAddTrack.Text = ">";
            this.otAddTrack.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.otAddTrack.Click += new System.EventHandler(this.otAddTrack_Click);
            // 
            // ctDelTrack
            // 
            this.ctDelTrack.AutoEllipsis = true;
            this.ctDelTrack.Font = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctDelTrack.ForeColor = System.Drawing.Color.Snow;
            this.ctDelTrack.HooverImage = ((System.Drawing.Image)(resources.GetObject("ctDelTrack.HooverImage")));
            this.ctDelTrack.Image = ((System.Drawing.Image)(resources.GetObject("ctDelTrack.Image")));
            this.ctDelTrack.Location = new System.Drawing.Point(146, 114);
            this.ctDelTrack.Name = "ctDelTrack";
            this.ctDelTrack.Size = new System.Drawing.Size(28, 20);
            this.ctDelTrack.TabIndex = 11;
            this.ctDelTrack.Text = "<";
            this.ctDelTrack.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ctDelTrack.Click += new System.EventHandler(this.ctDelTrack_Click);
            // 
            // ctAddTrack
            // 
            this.ctAddTrack.AutoEllipsis = true;
            this.ctAddTrack.Font = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctAddTrack.ForeColor = System.Drawing.Color.Snow;
            this.ctAddTrack.HooverImage = ((System.Drawing.Image)(resources.GetObject("ctAddTrack.HooverImage")));
            this.ctAddTrack.Image = ((System.Drawing.Image)(resources.GetObject("ctAddTrack.Image")));
            this.ctAddTrack.Location = new System.Drawing.Point(146, 87);
            this.ctAddTrack.Name = "ctAddTrack";
            this.ctAddTrack.Size = new System.Drawing.Size(28, 20);
            this.ctAddTrack.TabIndex = 10;
            this.ctAddTrack.Text = ">";
            this.ctAddTrack.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ctAddTrack.Click += new System.EventHandler(this.ctAddTrack_Click);
            // 
            // TrackByNPCNameLabel
            // 
            this.TrackByNPCNameLabel.Location = new System.Drawing.Point(12, 291);
            this.TrackByNPCNameLabel.Name = "TrackByNPCNameLabel";
            this.TrackByNPCNameLabel.Size = new System.Drawing.Size(128, 22);
            this.TrackByNPCNameLabel.TabIndex = 12;
            this.TrackByNPCNameLabel.Text = "By npc name:";
            this.TrackByNPCNameLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // TrackByObjectTypeLabel
            // 
            this.TrackByObjectTypeLabel.Location = new System.Drawing.Point(12, 163);
            this.TrackByObjectTypeLabel.Name = "TrackByObjectTypeLabel";
            this.TrackByObjectTypeLabel.Size = new System.Drawing.Size(296, 22);
            this.TrackByObjectTypeLabel.TabIndex = 13;
            this.TrackByObjectTypeLabel.Text = "Object type:";
            this.TrackByObjectTypeLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // TrackByCreatureTypeLabel
            // 
            this.TrackByCreatureTypeLabel.Location = new System.Drawing.Point(12, 43);
            this.TrackByCreatureTypeLabel.Name = "TrackByCreatureTypeLabel";
            this.TrackByCreatureTypeLabel.Size = new System.Drawing.Size(296, 22);
            this.TrackByCreatureTypeLabel.TabIndex = 14;
            this.TrackByCreatureTypeLabel.Text = "Creature type:";
            this.TrackByCreatureTypeLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // FormTrackerTimer
            // 
            this.FormTrackerTimer.Enabled = true;
            this.FormTrackerTimer.Interval = 200;
            this.FormTrackerTimer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MainHeader
            // 
            this.MainHeader.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("MainHeader.BackgroundImage")));
            this.MainHeader.Location = new System.Drawing.Point(0, 0);
            this.MainHeader.LogoImage = ((System.Drawing.Image)(resources.GetObject("MainHeader.LogoImage")));
            this.MainHeader.Name = "MainHeader";
            this.MainHeader.Size = new System.Drawing.Size(320, 43);
            this.MainHeader.TabIndex = 15;
            this.MainHeader.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.MainHeader.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.MainHeader.TitleText = "TheNoobBot";
            // 
            // FormTracker
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(320, 365);
            this.Controls.Add(this.MainHeader);
            this.Controls.Add(this.TrackByCreatureTypeLabel);
            this.Controls.Add(this.TrackByObjectTypeLabel);
            this.Controls.Add(this.TrackByNPCNameLabel);
            this.Controls.Add(this.ctDelTrack);
            this.Controls.Add(this.ctAddTrack);
            this.Controls.Add(this.otDelTrack);
            this.Controls.Add(this.otAddTrack);
            this.Controls.Add(this.unDelTrack);
            this.Controls.Add(this.unAddTrack);
            this.Controls.Add(this.TrackedByNameList);
            this.Controls.Add(this.ObjectTrackedList);
            this.Controls.Add(this.CreatureTrackedList);
            this.Controls.Add(this.unNoTrack);
            this.Controls.Add(this.ObjectTrackableList);
            this.Controls.Add(this.CreatureTrackableList);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormTracker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tracker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            this.Load += new System.EventHandler(this.FormTracker_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox CreatureTrackableList;
        private System.Windows.Forms.ListBox ObjectTrackableList;
        private TextBox unNoTrack;
        private System.Windows.Forms.ListBox CreatureTrackedList;
        private System.Windows.Forms.ListBox ObjectTrackedList;
        private System.Windows.Forms.ListBox TrackedByNameList;
        private TnbButton unAddTrack;
        private TnbButton unDelTrack;
        private TnbButton otDelTrack;
        private TnbButton otAddTrack;
        private TnbButton ctDelTrack;
        private TnbButton ctAddTrack;
        private Label TrackByNPCNameLabel;
        private Label TrackByObjectTypeLabel;
        private Label TrackByCreatureTypeLabel;
        private System.Windows.Forms.Timer FormTrackerTimer;
        private nManager.Helpful.Forms.UserControls.TnbControlMenu MainHeader;
    }
}