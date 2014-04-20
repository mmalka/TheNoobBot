using System.Windows.Forms;
using nManager.Helpful.Forms.UserControls;

namespace Gatherer.Bot
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
            this.listPoint = new System.Windows.Forms.ListBox();
            this.nSeparatorDistance = new System.Windows.Forms.NumericUpDown();
            this.labelX1 = new System.Windows.Forms.Label();
            this.listBlackRadius = new System.Windows.Forms.ListBox();
            this.radiusN = new System.Windows.Forms.NumericUpDown();
            this.listNpc = new System.Windows.Forms.ListBox();
            this.nameNpcTb = new System.Windows.Forms.TextBox();
            this.MainHeader = new nManager.Helpful.Forms.UserControls.TnbControlMenu();
            this.addByNameNpcB = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.loadB = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.npcTypeC = new nManager.Helpful.Forms.UserControls.TnbComboBox();
            this.addNpcB = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.delNpcB = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.addBlackB = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.delBlackRadius = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.delB = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.saveB = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.recordWayB = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.BlackListRadius = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nSeparatorDistance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radiusN)).BeginInit();
            this.SuspendLayout();
            // 
            // listPoint
            // 
            this.listPoint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.listPoint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.listPoint.FormattingEnabled = true;
            this.listPoint.Location = new System.Drawing.Point(13, 95);
            this.listPoint.Name = "listPoint";
            this.listPoint.Size = new System.Drawing.Size(551, 95);
            this.listPoint.TabIndex = 4;
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
            3,
            0,
            0,
            0});
            this.nSeparatorDistance.Name = "nSeparatorDistance";
            this.nSeparatorDistance.Size = new System.Drawing.Size(42, 22);
            this.nSeparatorDistance.TabIndex = 5;
            this.nSeparatorDistance.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.labelX1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.labelX1.Location = new System.Drawing.Point(10, 192);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(170, 29);
            this.labelX1.TabIndex = 6;
            this.labelX1.Text = "Separation distance record:";
            this.labelX1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // listBlackRadius
            // 
            this.listBlackRadius.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.listBlackRadius.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.listBlackRadius.FormattingEnabled = true;
            this.listBlackRadius.Location = new System.Drawing.Point(13, 241);
            this.listBlackRadius.Name = "listBlackRadius";
            this.listBlackRadius.Size = new System.Drawing.Size(551, 95);
            this.listBlackRadius.TabIndex = 9;
            // 
            // radiusN
            // 
            this.radiusN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.radiusN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.radiusN.Location = new System.Drawing.Point(191, 344);
            this.radiusN.Maximum = new decimal(new int[] {
            200,
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
            // listNpc
            // 
            this.listNpc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.listNpc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.listNpc.FormattingEnabled = true;
            this.listNpc.Location = new System.Drawing.Point(13, 389);
            this.listNpc.Name = "listNpc";
            this.listNpc.Size = new System.Drawing.Size(360, 43);
            this.listNpc.TabIndex = 13;
            this.listNpc.Visible = false;
            // 
            // nameNpcTb
            // 
            this.nameNpcTb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.nameNpcTb.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.nameNpcTb.Location = new System.Drawing.Point(206, 423);
            this.nameNpcTb.Name = "nameNpcTb";
            this.nameNpcTb.Size = new System.Drawing.Size(78, 22);
            this.nameNpcTb.TabIndex = 19;
            this.nameNpcTb.Text = "Name";
            this.nameNpcTb.Visible = false;
            // 
            // MainHeader
            // 
            this.MainHeader.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("MainHeader.BackgroundImage")));
            this.MainHeader.Location = new System.Drawing.Point(0, 0);
            this.MainHeader.LogoImage = ((System.Drawing.Image)(resources.GetObject("MainHeader.LogoImage")));
            this.MainHeader.Name = "MainHeader";
            this.MainHeader.Size = new System.Drawing.Size(575, 43);
            this.MainHeader.TabIndex = 21;
            this.MainHeader.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.MainHeader.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.MainHeader.TitleText = "Gatherer Profile Creator - The Noob Bot DevVersionRestrict";
            // 
            // addByNameNpcB
            // 
            this.addByNameNpcB.AutoEllipsis = true;
            this.addByNameNpcB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.addByNameNpcB.ForeColor = System.Drawing.Color.Snow;
            this.addByNameNpcB.HooverImage = ((System.Drawing.Image)(resources.GetObject("addByNameNpcB.HooverImage")));
            this.addByNameNpcB.Image = ((System.Drawing.Image)(resources.GetObject("addByNameNpcB.Image")));
            this.addByNameNpcB.Location = new System.Drawing.Point(238, 385);
            this.addByNameNpcB.Name = "addByNameNpcB";
            this.addByNameNpcB.Size = new System.Drawing.Size(124, 22);
            this.addByNameNpcB.TabIndex = 20;
            this.addByNameNpcB.Text = "Add by Name to Npc list";
            this.addByNameNpcB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.addByNameNpcB.Visible = false;
            this.addByNameNpcB.Click += new System.EventHandler(this.addByNameNpcB_Click);
            // 
            // loadB
            // 
            this.loadB.AutoEllipsis = true;
            this.loadB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.loadB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.loadB.HooverImage = global::Gatherer.Properties.Resources.greenB_150;
            this.loadB.Image = global::Gatherer.Properties.Resources.blackB_150;
            this.loadB.Location = new System.Drawing.Point(413, 54);
            this.loadB.Name = "loadB";
            this.loadB.Size = new System.Drawing.Size(150, 29);
            this.loadB.TabIndex = 18;
            this.loadB.Text = "Load an existing profile";
            this.loadB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.loadB.Click += new System.EventHandler(this.loadB_Click);
            // 
            // npcTypeC
            // 
            this.npcTypeC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.npcTypeC.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            this.npcTypeC.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.npcTypeC.DisplayMember = "Text";
            this.npcTypeC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.npcTypeC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.npcTypeC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.npcTypeC.FormattingEnabled = true;
            this.npcTypeC.HighlightColor = System.Drawing.Color.Gainsboro;
            this.npcTypeC.ItemHeight = 16;
            this.npcTypeC.Location = new System.Drawing.Point(44, 423);
            this.npcTypeC.Name = "npcTypeC";
            this.npcTypeC.SelectorBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(106)))), ((int)(((byte)(194)))));
            this.npcTypeC.SelectorImage = ((System.Drawing.Image)(resources.GetObject("npcTypeC.SelectorImage")));
            this.npcTypeC.Size = new System.Drawing.Size(136, 22);
            this.npcTypeC.TabIndex = 17;
            this.npcTypeC.Visible = false;
            // 
            // addNpcB
            // 
            this.addNpcB.AutoEllipsis = true;
            this.addNpcB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.addNpcB.ForeColor = System.Drawing.Color.Snow;
            this.addNpcB.HooverImage = ((System.Drawing.Image)(resources.GetObject("addNpcB.HooverImage")));
            this.addNpcB.Image = ((System.Drawing.Image)(resources.GetObject("addNpcB.Image")));
            this.addNpcB.Location = new System.Drawing.Point(16, 385);
            this.addNpcB.Name = "addNpcB";
            this.addNpcB.Size = new System.Drawing.Size(207, 22);
            this.addNpcB.TabIndex = 16;
            this.addNpcB.Text = "Add Target to Npc list";
            this.addNpcB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.addNpcB.Visible = false;
            this.addNpcB.Click += new System.EventHandler(this.addNpcB_Click);
            // 
            // delNpcB
            // 
            this.delNpcB.AutoEllipsis = true;
            this.delNpcB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.delNpcB.ForeColor = System.Drawing.Color.Snow;
            this.delNpcB.HooverImage = ((System.Drawing.Image)(resources.GetObject("delNpcB.HooverImage")));
            this.delNpcB.Image = ((System.Drawing.Image)(resources.GetObject("delNpcB.Image")));
            this.delNpcB.Location = new System.Drawing.Point(379, 397);
            this.delNpcB.Name = "delNpcB";
            this.delNpcB.Size = new System.Drawing.Size(36, 17);
            this.delNpcB.TabIndex = 14;
            this.delNpcB.Text = "Del";
            this.delNpcB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.delNpcB.Visible = false;
            this.delNpcB.Click += new System.EventHandler(this.delNpcB_Click);
            // 
            // addBlackB
            // 
            this.addBlackB.AutoEllipsis = true;
            this.addBlackB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.addBlackB.ForeColor = System.Drawing.Color.Snow;
            this.addBlackB.HooverImage = global::Gatherer.Properties.Resources.greenB_260;
            this.addBlackB.Image = global::Gatherer.Properties.Resources.blackB_260;
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
            this.delBlackRadius.HooverImage = global::Gatherer.Properties.Resources.greenB_70;
            this.delBlackRadius.Image = global::Gatherer.Properties.Resources.blackB_70;
            this.delBlackRadius.Location = new System.Drawing.Point(493, 242);
            this.delBlackRadius.Name = "delBlackRadius";
            this.delBlackRadius.Size = new System.Drawing.Size(70, 29);
            this.delBlackRadius.TabIndex = 10;
            this.delBlackRadius.Text = "Del";
            this.delBlackRadius.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.delBlackRadius.Click += new System.EventHandler(this.delBlackRadius_Click);
            // 
            // delB
            // 
            this.delB.AutoEllipsis = true;
            this.delB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.delB.ForeColor = System.Drawing.Color.Snow;
            this.delB.HooverImage = global::Gatherer.Properties.Resources.greenB_70;
            this.delB.Image = global::Gatherer.Properties.Resources.blackB_70;
            this.delB.Location = new System.Drawing.Point(493, 96);
            this.delB.Name = "delB";
            this.delB.Size = new System.Drawing.Size(70, 29);
            this.delB.TabIndex = 8;
            this.delB.Text = "Del";
            this.delB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.delB.Click += new System.EventHandler(this.delB_Click);
            // 
            // saveB
            // 
            this.saveB.AutoEllipsis = true;
            this.saveB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.saveB.ForeColor = System.Drawing.Color.Snow;
            this.saveB.HooverImage = global::Gatherer.Properties.Resources.greenB;
            this.saveB.Image = global::Gatherer.Properties.Resources.blueB;
            this.saveB.Location = new System.Drawing.Point(457, 385);
            this.saveB.Name = "saveB";
            this.saveB.Size = new System.Drawing.Size(106, 29);
            this.saveB.TabIndex = 2;
            this.saveB.Text = "Save";
            this.saveB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.saveB.Click += new System.EventHandler(this.saveB_Click);
            // 
            // recordWayB
            // 
            this.recordWayB.AutoEllipsis = true;
            this.recordWayB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.recordWayB.ForeColor = System.Drawing.Color.Snow;
            this.recordWayB.HooverImage = global::Gatherer.Properties.Resources.greenB_260;
            this.recordWayB.Image = global::Gatherer.Properties.Resources.blackB_260;
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
            this.BlackListRadius.Location = new System.Drawing.Point(10, 338);
            this.BlackListRadius.Name = "BlackListRadius";
            this.BlackListRadius.Size = new System.Drawing.Size(170, 29);
            this.BlackListRadius.TabIndex = 22;
            this.BlackListRadius.Text = "BlackList Radius";
            // 
            // ProfileCreator
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(575, 426);
            this.Controls.Add(this.BlackListRadius);
            this.Controls.Add(this.MainHeader);
            this.Controls.Add(this.addByNameNpcB);
            this.Controls.Add(this.nameNpcTb);
            this.Controls.Add(this.loadB);
            this.Controls.Add(this.npcTypeC);
            this.Controls.Add(this.addNpcB);
            this.Controls.Add(this.delNpcB);
            this.Controls.Add(this.listNpc);
            this.Controls.Add(this.addBlackB);
            this.Controls.Add(this.radiusN);
            this.Controls.Add(this.delBlackRadius);
            this.Controls.Add(this.listBlackRadius);
            this.Controls.Add(this.delB);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.nSeparatorDistance);
            this.Controls.Add(this.listPoint);
            this.Controls.Add(this.saveB);
            this.Controls.Add(this.recordWayB);
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
            this.PerformLayout();

        }

        #endregion

        private TnbButton recordWayB;
        private TnbButton saveB;
        private System.Windows.Forms.ListBox listPoint;
        private NumericUpDown nSeparatorDistance;
        private Label labelX1;
        private TnbButton delB;
        private TnbButton delBlackRadius;
        private System.Windows.Forms.ListBox listBlackRadius;
        private NumericUpDown radiusN;
        private TnbButton addBlackB;
        private TnbButton addNpcB;
        private TnbButton delNpcB;
        private System.Windows.Forms.ListBox listNpc;
        private TnbComboBox npcTypeC;
        private TnbButton loadB;
        private TextBox nameNpcTb;
        private TnbButton addByNameNpcB;
        private TnbControlMenu MainHeader;
        private Label BlackListRadius;
    }
}