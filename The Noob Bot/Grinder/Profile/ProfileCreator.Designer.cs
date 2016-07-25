using System.Windows.Forms;
using nManager.Helpful.Forms.UserControls;

namespace Grinder.Profile
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
            this.ListOfZonesLabel = new System.Windows.Forms.Label();
            this.zoneNameTb = new System.Windows.Forms.TextBox();
            this.ZoneNameLabel = new System.Windows.Forms.Label();
            this.ZoneMinLevel = new System.Windows.Forms.Label();
            this.minLevelPlayer = new System.Windows.Forms.NumericUpDown();
            this.ZoneMaxLevelLabel = new System.Windows.Forms.Label();
            this.maxLevelPlayer = new System.Windows.Forms.NumericUpDown();
            this.maxLevelTarget = new System.Windows.Forms.NumericUpDown();
            this.TargetMaxLevelLabel = new System.Windows.Forms.Label();
            this.minLevelTarget = new System.Windows.Forms.NumericUpDown();
            this.TargetMinLevelLabel = new System.Windows.Forms.Label();
            this.listEntryTb = new System.Windows.Forms.TextBox();
            this.labelX8 = new System.Windows.Forms.Label();
            this.BlackListRadius = new System.Windows.Forms.Label();
            this.MainHeader = new nManager.Helpful.Forms.UserControls.TnbControlMenu();
            this.addTargetEntryB = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.delZoneB = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.addZoneB = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.listZoneCb = new nManager.Helpful.Forms.UserControls.TnbComboBox();
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
            ((System.ComponentModel.ISupportInitialize)(this.nSeparatorDistance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radiusN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minLevelPlayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxLevelPlayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxLevelTarget)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minLevelTarget)).BeginInit();
            this.SuspendLayout();
            // 
            // listPoint
            // 
            this.listPoint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.listPoint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.listPoint.FormattingEnabled = true;
            this.listPoint.Location = new System.Drawing.Point(13, 239);
            this.listPoint.Name = "listPoint";
            this.listPoint.Size = new System.Drawing.Size(551, 95);
            this.listPoint.TabIndex = 4;
            // 
            // nSeparatorDistance
            // 
            this.nSeparatorDistance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.nSeparatorDistance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.nSeparatorDistance.Location = new System.Drawing.Point(191, 343);
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
            5,
            0,
            0,
            0});
            // 
            // labelX1
            // 
            this.labelX1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.labelX1.Location = new System.Drawing.Point(10, 336);
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
            this.listBlackRadius.Location = new System.Drawing.Point(13, 381);
            this.listBlackRadius.Name = "listBlackRadius";
            this.listBlackRadius.Size = new System.Drawing.Size(551, 95);
            this.listBlackRadius.TabIndex = 9;
            // 
            // radiusN
            // 
            this.radiusN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.radiusN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.radiusN.Location = new System.Drawing.Point(230, 485);
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
            this.listNpc.Location = new System.Drawing.Point(24, 532);
            this.listNpc.Name = "listNpc";
            this.listNpc.Size = new System.Drawing.Size(375, 56);
            this.listNpc.TabIndex = 13;
            this.listNpc.Visible = false;
            // 
            // nameNpcTb
            // 
            this.nameNpcTb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.nameNpcTb.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.nameNpcTb.Location = new System.Drawing.Point(183, 623);
            this.nameNpcTb.Name = "nameNpcTb";
            this.nameNpcTb.Size = new System.Drawing.Size(81, 22);
            this.nameNpcTb.TabIndex = 19;
            this.nameNpcTb.Text = "Name";
            this.nameNpcTb.Visible = false;
            // 
            // ListOfZonesLabel
            // 
            this.ListOfZonesLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.ListOfZonesLabel.Location = new System.Drawing.Point(10, 89);
            this.ListOfZonesLabel.Name = "ListOfZonesLabel";
            this.ListOfZonesLabel.Size = new System.Drawing.Size(80, 29);
            this.ListOfZonesLabel.TabIndex = 22;
            this.ListOfZonesLabel.Text = "List of zones :";
            this.ListOfZonesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // zoneNameTb
            // 
            this.zoneNameTb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.zoneNameTb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.zoneNameTb.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.zoneNameTb.Location = new System.Drawing.Point(96, 130);
            this.zoneNameTb.Name = "zoneNameTb";
            this.zoneNameTb.Size = new System.Drawing.Size(155, 22);
            this.zoneNameTb.TabIndex = 24;
            this.zoneNameTb.TextChanged += new System.EventHandler(this.zoneNameTb_TextChanged);
            // 
            // ZoneNameLabel
            // 
            this.ZoneNameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.ZoneNameLabel.Location = new System.Drawing.Point(10, 123);
            this.ZoneNameLabel.Name = "ZoneNameLabel";
            this.ZoneNameLabel.Size = new System.Drawing.Size(80, 29);
            this.ZoneNameLabel.TabIndex = 26;
            this.ZoneNameLabel.Text = "Zone Name:";
            this.ZoneNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ZoneMinLevel
            // 
            this.ZoneMinLevel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.ZoneMinLevel.Location = new System.Drawing.Point(10, 160);
            this.ZoneMinLevel.Name = "ZoneMinLevel";
            this.ZoneMinLevel.Size = new System.Drawing.Size(105, 29);
            this.ZoneMinLevel.TabIndex = 27;
            this.ZoneMinLevel.Text = "Zone MinLevel :";
            this.ZoneMinLevel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // minLevelPlayer
            // 
            this.minLevelPlayer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.minLevelPlayer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.minLevelPlayer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.minLevelPlayer.Location = new System.Drawing.Point(122, 166);
            this.minLevelPlayer.Maximum = new decimal(new int[] {
            110,
            0,
            0,
            0});
            this.minLevelPlayer.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.minLevelPlayer.Name = "minLevelPlayer";
            this.minLevelPlayer.Size = new System.Drawing.Size(43, 22);
            this.minLevelPlayer.TabIndex = 28;
            this.minLevelPlayer.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.minLevelPlayer.ValueChanged += new System.EventHandler(this.Level_ValueChanged);
            // 
            // ZoneMaxLevelLabel
            // 
            this.ZoneMaxLevelLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.ZoneMaxLevelLabel.Location = new System.Drawing.Point(171, 160);
            this.ZoneMaxLevelLabel.Name = "ZoneMaxLevelLabel";
            this.ZoneMaxLevelLabel.Size = new System.Drawing.Size(80, 29);
            this.ZoneMaxLevelLabel.TabIndex = 29;
            this.ZoneMaxLevelLabel.Text = "MaxLevel :";
            this.ZoneMaxLevelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // maxLevelPlayer
            // 
            this.maxLevelPlayer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.maxLevelPlayer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.maxLevelPlayer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.maxLevelPlayer.Location = new System.Drawing.Point(248, 166);
            this.maxLevelPlayer.Maximum = new decimal(new int[] {
            110,
            0,
            0,
            0});
            this.maxLevelPlayer.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.maxLevelPlayer.Name = "maxLevelPlayer";
            this.maxLevelPlayer.Size = new System.Drawing.Size(43, 22);
            this.maxLevelPlayer.TabIndex = 30;
            this.maxLevelPlayer.Value = new decimal(new int[] {
            110,
            0,
            0,
            0});
            this.maxLevelPlayer.ValueChanged += new System.EventHandler(this.Level_ValueChanged);
            // 
            // maxLevelTarget
            // 
            this.maxLevelTarget.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.maxLevelTarget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.maxLevelTarget.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.maxLevelTarget.Location = new System.Drawing.Point(521, 166);
            this.maxLevelTarget.Maximum = new decimal(new int[] {
            113,
            0,
            0,
            0});
            this.maxLevelTarget.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.maxLevelTarget.Name = "maxLevelTarget";
            this.maxLevelTarget.Size = new System.Drawing.Size(43, 22);
            this.maxLevelTarget.TabIndex = 34;
            this.maxLevelTarget.Value = new decimal(new int[] {
            113,
            0,
            0,
            0});
            this.maxLevelTarget.ValueChanged += new System.EventHandler(this.Level_ValueChanged);
            // 
            // TargetMaxLevelLabel
            // 
            this.TargetMaxLevelLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.TargetMaxLevelLabel.Location = new System.Drawing.Point(454, 160);
            this.TargetMaxLevelLabel.Name = "TargetMaxLevelLabel";
            this.TargetMaxLevelLabel.Size = new System.Drawing.Size(64, 29);
            this.TargetMaxLevelLabel.TabIndex = 33;
            this.TargetMaxLevelLabel.Text = "MaxLevel :";
            this.TargetMaxLevelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // minLevelTarget
            // 
            this.minLevelTarget.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.minLevelTarget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.minLevelTarget.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.minLevelTarget.Location = new System.Drawing.Point(405, 166);
            this.minLevelTarget.Maximum = new decimal(new int[] {
            110,
            0,
            0,
            0});
            this.minLevelTarget.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.minLevelTarget.Name = "minLevelTarget";
            this.minLevelTarget.Size = new System.Drawing.Size(43, 22);
            this.minLevelTarget.TabIndex = 32;
            this.minLevelTarget.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.minLevelTarget.ValueChanged += new System.EventHandler(this.Level_ValueChanged);
            // 
            // TargetMinLevelLabel
            // 
            this.TargetMinLevelLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.TargetMinLevelLabel.Location = new System.Drawing.Point(297, 160);
            this.TargetMinLevelLabel.Name = "TargetMinLevelLabel";
            this.TargetMinLevelLabel.Size = new System.Drawing.Size(108, 29);
            this.TargetMinLevelLabel.TabIndex = 31;
            this.TargetMinLevelLabel.Text = "Target\'s MinLevel :";
            this.TargetMinLevelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // listEntryTb
            // 
            this.listEntryTb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.listEntryTb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listEntryTb.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.listEntryTb.Location = new System.Drawing.Point(108, 203);
            this.listEntryTb.Name = "listEntryTb";
            this.listEntryTb.Size = new System.Drawing.Size(294, 22);
            this.listEntryTb.TabIndex = 35;
            this.listEntryTb.TextChanged += new System.EventHandler(this.listEntryTb_TextChanged);
            // 
            // labelX8
            // 
            this.labelX8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.labelX8.Location = new System.Drawing.Point(10, 199);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(92, 29);
            this.labelX8.TabIndex = 37;
            this.labelX8.Text = "Ids of targets :";
            this.labelX8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BlackListRadius
            // 
            this.BlackListRadius.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.BlackListRadius.Location = new System.Drawing.Point(12, 479);
            this.BlackListRadius.Name = "BlackListRadius";
            this.BlackListRadius.Size = new System.Drawing.Size(212, 29);
            this.BlackListRadius.TabIndex = 38;
            this.BlackListRadius.Text = "BlackList Radius";
            this.BlackListRadius.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MainHeader
            // 
            this.MainHeader.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("MainHeader.BackgroundImage")));
            this.MainHeader.Location = new System.Drawing.Point(0, 0);
            this.MainHeader.LogoImage = ((System.Drawing.Image)(resources.GetObject("MainHeader.LogoImage")));
            this.MainHeader.Name = "MainHeader";
            this.MainHeader.Size = new System.Drawing.Size(575, 43);
            this.MainHeader.TabIndex = 39;
            this.MainHeader.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.MainHeader.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.MainHeader.TitleText = "Grinder Profile Creator - The Noob Bot DevVersionRestrict";
            // 
            // addTargetEntryB
            // 
            this.addTargetEntryB.AutoEllipsis = true;
            this.addTargetEntryB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.addTargetEntryB.ForeColor = System.Drawing.Color.Snow;
            this.addTargetEntryB.HooverImage = global::Grinder.Properties.Resources.greenB_150;
            this.addTargetEntryB.Image = ((System.Drawing.Image)(resources.GetObject("addTargetEntryB.Image")));
            this.addTargetEntryB.Location = new System.Drawing.Point(413, 199);
            this.addTargetEntryB.Name = "addTargetEntryB";
            this.addTargetEntryB.Size = new System.Drawing.Size(150, 29);
            this.addTargetEntryB.TabIndex = 36;
            this.addTargetEntryB.Text = "Add Target to the list";
            this.addTargetEntryB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.addTargetEntryB.Click += new System.EventHandler(this.addTargetEntryB_Click);
            // 
            // delZoneB
            // 
            this.delZoneB.AutoEllipsis = true;
            this.delZoneB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.delZoneB.ForeColor = System.Drawing.Color.Snow;
            this.delZoneB.HooverImage = ((System.Drawing.Image)(resources.GetObject("delZoneB.HooverImage")));
            this.delZoneB.Image = ((System.Drawing.Image)(resources.GetObject("delZoneB.Image")));
            this.delZoneB.Location = new System.Drawing.Point(458, 126);
            this.delZoneB.Name = "delZoneB";
            this.delZoneB.Size = new System.Drawing.Size(106, 29);
            this.delZoneB.TabIndex = 25;
            this.delZoneB.Text = "Delete";
            this.delZoneB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.delZoneB.Click += new System.EventHandler(this.delZoneB_Click);
            // 
            // addZoneB
            // 
            this.addZoneB.AutoEllipsis = true;
            this.addZoneB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.addZoneB.ForeColor = System.Drawing.Color.Snow;
            this.addZoneB.HooverImage = ((System.Drawing.Image)(resources.GetObject("addZoneB.HooverImage")));
            this.addZoneB.Image = ((System.Drawing.Image)(resources.GetObject("addZoneB.Image")));
            this.addZoneB.Location = new System.Drawing.Point(262, 126);
            this.addZoneB.Name = "addZoneB";
            this.addZoneB.Size = new System.Drawing.Size(106, 29);
            this.addZoneB.TabIndex = 23;
            this.addZoneB.Text = "Add as new";
            this.addZoneB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.addZoneB.Click += new System.EventHandler(this.addZoneB_Click);
            // 
            // listZoneCb
            // 
            this.listZoneCb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.listZoneCb.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            this.listZoneCb.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.listZoneCb.DisplayMember = "Text";
            this.listZoneCb.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listZoneCb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.listZoneCb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.listZoneCb.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.listZoneCb.FormattingEnabled = true;
            this.listZoneCb.HighlightColor = System.Drawing.Color.Gainsboro;
            this.listZoneCb.ItemHeight = 16;
            this.listZoneCb.Location = new System.Drawing.Point(96, 94);
            this.listZoneCb.Name = "listZoneCb";
            this.listZoneCb.SelectorBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(106)))), ((int)(((byte)(194)))));
            this.listZoneCb.SelectorImage = ((System.Drawing.Image)(resources.GetObject("listZoneCb.SelectorImage")));
            this.listZoneCb.Size = new System.Drawing.Size(467, 22);
            this.listZoneCb.TabIndex = 21;
            this.listZoneCb.SelectedIndexChanged += new System.EventHandler(this.listZoneCb_SelectedIndexChanged);
            // 
            // addByNameNpcB
            // 
            this.addByNameNpcB.AutoEllipsis = true;
            this.addByNameNpcB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.addByNameNpcB.ForeColor = System.Drawing.Color.Snow;
            this.addByNameNpcB.HooverImage = ((System.Drawing.Image)(resources.GetObject("addByNameNpcB.HooverImage")));
            this.addByNameNpcB.Image = ((System.Drawing.Image)(resources.GetObject("addByNameNpcB.Image")));
            this.addByNameNpcB.Location = new System.Drawing.Point(270, 622);
            this.addByNameNpcB.Name = "addByNameNpcB";
            this.addByNameNpcB.Size = new System.Drawing.Size(129, 23);
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
            this.loadB.ForeColor = System.Drawing.Color.Snow;
            this.loadB.HooverImage = global::Grinder.Properties.Resources.greenB_150;
            this.loadB.Image = global::Grinder.Properties.Resources.blackB_150;
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
            this.npcTypeC.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.npcTypeC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.npcTypeC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.npcTypeC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.npcTypeC.FormattingEnabled = true;
            this.npcTypeC.HighlightColor = System.Drawing.Color.Gainsboro;
            this.npcTypeC.ItemHeight = 16;
            this.npcTypeC.Location = new System.Drawing.Point(35, 594);
            this.npcTypeC.Name = "npcTypeC";
            this.npcTypeC.SelectorBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(106)))), ((int)(((byte)(194)))));
            this.npcTypeC.SelectorImage = ((System.Drawing.Image)(resources.GetObject("npcTypeC.SelectorImage")));
            this.npcTypeC.Size = new System.Drawing.Size(142, 22);
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
            this.addNpcB.Location = new System.Drawing.Point(183, 593);
            this.addNpcB.Name = "addNpcB";
            this.addNpcB.Size = new System.Drawing.Size(216, 23);
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
            this.delNpcB.Location = new System.Drawing.Point(362, 532);
            this.delNpcB.Name = "delNpcB";
            this.delNpcB.Size = new System.Drawing.Size(37, 18);
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
            this.addBlackB.HooverImage = global::Grinder.Properties.Resources.greenB_260;
            this.addBlackB.Image = global::Grinder.Properties.Resources.blackB_260;
            this.addBlackB.Location = new System.Drawing.Point(222, 382);
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
            this.delBlackRadius.HooverImage = global::Grinder.Properties.Resources.greenB_70;
            this.delBlackRadius.Image = global::Grinder.Properties.Resources.blackB_70;
            this.delBlackRadius.Location = new System.Drawing.Point(493, 382);
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
            this.delB.HooverImage = global::Grinder.Properties.Resources.greenB_70;
            this.delB.Image = global::Grinder.Properties.Resources.blackB_70;
            this.delB.Location = new System.Drawing.Point(493, 240);
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
            this.saveB.HooverImage = ((System.Drawing.Image)(resources.GetObject("saveB.HooverImage")));
            this.saveB.Image = global::Grinder.Properties.Resources.blueB;
            this.saveB.Location = new System.Drawing.Point(457, 490);
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
            this.recordWayB.HooverImage = global::Grinder.Properties.Resources.greenB_260;
            this.recordWayB.Image = ((System.Drawing.Image)(resources.GetObject("recordWayB.Image")));
            this.recordWayB.Location = new System.Drawing.Point(303, 345);
            this.recordWayB.Name = "recordWayB";
            this.recordWayB.Size = new System.Drawing.Size(260, 29);
            this.recordWayB.TabIndex = 1;
            this.recordWayB.Text = "Record Way";
            this.recordWayB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.recordWayB.Click += new System.EventHandler(this.recordWayB_Click);
            // 
            // ProfileCreator
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(575, 531);
            this.Controls.Add(this.MainHeader);
            this.Controls.Add(this.BlackListRadius);
            this.Controls.Add(this.labelX8);
            this.Controls.Add(this.addTargetEntryB);
            this.Controls.Add(this.listEntryTb);
            this.Controls.Add(this.maxLevelTarget);
            this.Controls.Add(this.TargetMaxLevelLabel);
            this.Controls.Add(this.minLevelTarget);
            this.Controls.Add(this.TargetMinLevelLabel);
            this.Controls.Add(this.maxLevelPlayer);
            this.Controls.Add(this.ZoneMaxLevelLabel);
            this.Controls.Add(this.minLevelPlayer);
            this.Controls.Add(this.ZoneMinLevel);
            this.Controls.Add(this.ZoneNameLabel);
            this.Controls.Add(this.delZoneB);
            this.Controls.Add(this.zoneNameTb);
            this.Controls.Add(this.addZoneB);
            this.Controls.Add(this.ListOfZonesLabel);
            this.Controls.Add(this.listZoneCb);
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
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "ProfileCreator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Profile Creator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProfileCreator_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.nSeparatorDistance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radiusN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minLevelPlayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxLevelPlayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxLevelTarget)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minLevelTarget)).EndInit();
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
        private TnbComboBox listZoneCb;
        private Label ListOfZonesLabel;
        private TnbButton addZoneB;
        private TextBox zoneNameTb;
        private TnbButton delZoneB;
        private Label ZoneNameLabel;
        private Label ZoneMinLevel;
        private NumericUpDown minLevelPlayer;
        private Label ZoneMaxLevelLabel;
        private NumericUpDown maxLevelPlayer;
        private NumericUpDown maxLevelTarget;
        private Label TargetMaxLevelLabel;
        private NumericUpDown minLevelTarget;
        private Label TargetMinLevelLabel;
        private TextBox listEntryTb;
        private TnbButton addTargetEntryB;
        private Label labelX8;
        private Label BlackListRadius;
        private TnbControlMenu MainHeader;
    }
}