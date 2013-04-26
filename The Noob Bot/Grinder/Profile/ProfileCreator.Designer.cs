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
            this.recordWayB = new DevComponents.DotNetBar.ButtonX();
            this.saveB = new DevComponents.DotNetBar.ButtonX();
            this.listPoint = new System.Windows.Forms.ListBox();
            this.nSeparatorDistance = new DevComponents.Editors.IntegerInput();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.delB = new DevComponents.DotNetBar.ButtonX();
            this.delBlackRadius = new DevComponents.DotNetBar.ButtonX();
            this.listBlackRadius = new System.Windows.Forms.ListBox();
            this.radiusN = new DevComponents.Editors.IntegerInput();
            this.addBlackB = new DevComponents.DotNetBar.ButtonX();
            this.addNpcB = new DevComponents.DotNetBar.ButtonX();
            this.delNpcB = new DevComponents.DotNetBar.ButtonX();
            this.listNpc = new System.Windows.Forms.ListBox();
            this.npcTypeC = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.loadB = new DevComponents.DotNetBar.ButtonX();
            this.nameNpcTb = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.addByNameNpcB = new DevComponents.DotNetBar.ButtonX();
            this.listZoneCb = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.addZoneB = new DevComponents.DotNetBar.ButtonX();
            this.zoneNameTb = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.delZoneB = new DevComponents.DotNetBar.ButtonX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.minLevelPlayer = new DevComponents.Editors.IntegerInput();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.maxLevelPlayer = new DevComponents.Editors.IntegerInput();
            this.maxLevelTarget = new DevComponents.Editors.IntegerInput();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.minLevelTarget = new DevComponents.Editors.IntegerInput();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.listEntryTb = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.addTargetEntryB = new DevComponents.DotNetBar.ButtonX();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            ((System.ComponentModel.ISupportInitialize)(this.nSeparatorDistance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radiusN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minLevelPlayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxLevelPlayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxLevelTarget)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minLevelTarget)).BeginInit();
            this.SuspendLayout();
            // 
            // recordWayB
            // 
            this.recordWayB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.recordWayB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.recordWayB.Location = new System.Drawing.Point(12, 304);
            this.recordWayB.Name = "recordWayB";
            this.recordWayB.Size = new System.Drawing.Size(352, 23);
            this.recordWayB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.recordWayB.TabIndex = 1;
            this.recordWayB.Text = "Record Way";
            this.recordWayB.Click += new System.EventHandler(this.recordWayB_Click);
            // 
            // saveB
            // 
            this.saveB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.saveB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.saveB.Location = new System.Drawing.Point(318, 8);
            this.saveB.Name = "saveB";
            this.saveB.Size = new System.Drawing.Size(58, 19);
            this.saveB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.saveB.TabIndex = 2;
            this.saveB.Text = "Save";
            this.saveB.Click += new System.EventHandler(this.saveB_Click);
            // 
            // listPoint
            // 
            this.listPoint.BackColor = System.Drawing.Color.White;
            this.listPoint.ForeColor = System.Drawing.Color.Black;
            this.listPoint.FormattingEnabled = true;
            this.listPoint.Location = new System.Drawing.Point(1, 164);
            this.listPoint.Name = "listPoint";
            this.listPoint.Size = new System.Drawing.Size(375, 134);
            this.listPoint.TabIndex = 4;
            // 
            // nSeparatorDistance
            // 
            this.nSeparatorDistance.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.nSeparatorDistance.BackgroundStyle.Class = "DateTimeInputBackground";
            this.nSeparatorDistance.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.nSeparatorDistance.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.nSeparatorDistance.ForeColor = System.Drawing.Color.Black;
            this.nSeparatorDistance.Location = new System.Drawing.Point(145, 5);
            this.nSeparatorDistance.MaxValue = 200;
            this.nSeparatorDistance.MinValue = 3;
            this.nSeparatorDistance.Name = "nSeparatorDistance";
            this.nSeparatorDistance.ShowUpDown = true;
            this.nSeparatorDistance.Size = new System.Drawing.Size(94, 22);
            this.nSeparatorDistance.TabIndex = 5;
            this.nSeparatorDistance.Value = 5;
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.ForeColor = System.Drawing.Color.Black;
            this.labelX1.Location = new System.Drawing.Point(1, 4);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(138, 23);
            this.labelX1.TabIndex = 6;
            this.labelX1.Text = "Separation distance record:";
            // 
            // delB
            // 
            this.delB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.delB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.delB.Location = new System.Drawing.Point(339, 164);
            this.delB.Name = "delB";
            this.delB.Size = new System.Drawing.Size(37, 23);
            this.delB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.delB.TabIndex = 8;
            this.delB.Text = "Del";
            this.delB.Click += new System.EventHandler(this.delB_Click);
            // 
            // delBlackRadius
            // 
            this.delBlackRadius.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.delBlackRadius.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.delBlackRadius.Location = new System.Drawing.Point(339, 341);
            this.delBlackRadius.Name = "delBlackRadius";
            this.delBlackRadius.Size = new System.Drawing.Size(37, 18);
            this.delBlackRadius.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.delBlackRadius.TabIndex = 10;
            this.delBlackRadius.Text = "Del";
            this.delBlackRadius.Click += new System.EventHandler(this.delBlackRadius_Click);
            // 
            // listBlackRadius
            // 
            this.listBlackRadius.BackColor = System.Drawing.Color.White;
            this.listBlackRadius.ForeColor = System.Drawing.Color.Black;
            this.listBlackRadius.FormattingEnabled = true;
            this.listBlackRadius.Location = new System.Drawing.Point(1, 341);
            this.listBlackRadius.Name = "listBlackRadius";
            this.listBlackRadius.Size = new System.Drawing.Size(375, 69);
            this.listBlackRadius.TabIndex = 9;
            // 
            // radiusN
            // 
            this.radiusN.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.radiusN.BackgroundStyle.Class = "DateTimeInputBackground";
            this.radiusN.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.radiusN.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.radiusN.ForeColor = System.Drawing.Color.Black;
            this.radiusN.Location = new System.Drawing.Point(12, 417);
            this.radiusN.MinValue = 1;
            this.radiusN.Name = "radiusN";
            this.radiusN.ShowUpDown = true;
            this.radiusN.Size = new System.Drawing.Size(113, 22);
            this.radiusN.TabIndex = 11;
            this.radiusN.Value = 35;
            // 
            // addBlackB
            // 
            this.addBlackB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.addBlackB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.addBlackB.Location = new System.Drawing.Point(131, 416);
            this.addBlackB.Name = "addBlackB";
            this.addBlackB.Size = new System.Drawing.Size(233, 23);
            this.addBlackB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.addBlackB.TabIndex = 12;
            this.addBlackB.Text = "Add this position to Black list Radius";
            this.addBlackB.Click += new System.EventHandler(this.addBlackB_Click);
            // 
            // addNpcB
            // 
            this.addNpcB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.addNpcB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.addNpcB.Location = new System.Drawing.Point(160, 506);
            this.addNpcB.Name = "addNpcB";
            this.addNpcB.Size = new System.Drawing.Size(216, 23);
            this.addNpcB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.addNpcB.TabIndex = 16;
            this.addNpcB.Text = "Add Target to Npc list";
            this.addNpcB.Click += new System.EventHandler(this.addNpcB_Click);
            // 
            // delNpcB
            // 
            this.delNpcB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.delNpcB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.delNpcB.Location = new System.Drawing.Point(339, 445);
            this.delNpcB.Name = "delNpcB";
            this.delNpcB.Size = new System.Drawing.Size(37, 18);
            this.delNpcB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.delNpcB.TabIndex = 14;
            this.delNpcB.Text = "Del";
            this.delNpcB.Click += new System.EventHandler(this.delNpcB_Click);
            // 
            // listNpc
            // 
            this.listNpc.BackColor = System.Drawing.Color.White;
            this.listNpc.ForeColor = System.Drawing.Color.Black;
            this.listNpc.FormattingEnabled = true;
            this.listNpc.Location = new System.Drawing.Point(1, 445);
            this.listNpc.Name = "listNpc";
            this.listNpc.Size = new System.Drawing.Size(375, 56);
            this.listNpc.TabIndex = 13;
            // 
            // npcTypeC
            // 
            this.npcTypeC.DisplayMember = "Text";
            this.npcTypeC.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.npcTypeC.ForeColor = System.Drawing.Color.Black;
            this.npcTypeC.FormattingEnabled = true;
            this.npcTypeC.ItemHeight = 16;
            this.npcTypeC.Location = new System.Drawing.Point(12, 507);
            this.npcTypeC.Name = "npcTypeC";
            this.npcTypeC.Size = new System.Drawing.Size(142, 22);
            this.npcTypeC.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.npcTypeC.TabIndex = 17;
            // 
            // loadB
            // 
            this.loadB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.loadB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.loadB.Location = new System.Drawing.Point(254, 8);
            this.loadB.Name = "loadB";
            this.loadB.Size = new System.Drawing.Size(58, 19);
            this.loadB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.loadB.TabIndex = 18;
            this.loadB.Text = "Load";
            this.loadB.Click += new System.EventHandler(this.loadB_Click);
            // 
            // nameNpcTb
            // 
            this.nameNpcTb.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.nameNpcTb.Border.Class = "TextBoxBorder";
            this.nameNpcTb.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.nameNpcTb.ForeColor = System.Drawing.Color.Black;
            this.nameNpcTb.Location = new System.Drawing.Point(160, 536);
            this.nameNpcTb.Name = "nameNpcTb";
            this.nameNpcTb.Size = new System.Drawing.Size(81, 22);
            this.nameNpcTb.TabIndex = 19;
            this.nameNpcTb.Text = "Name";
            // 
            // addByNameNpcB
            // 
            this.addByNameNpcB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.addByNameNpcB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.addByNameNpcB.Location = new System.Drawing.Point(247, 535);
            this.addByNameNpcB.Name = "addByNameNpcB";
            this.addByNameNpcB.Size = new System.Drawing.Size(129, 23);
            this.addByNameNpcB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.addByNameNpcB.TabIndex = 20;
            this.addByNameNpcB.Text = "Add by Name to Npc list";
            this.addByNameNpcB.Click += new System.EventHandler(this.addByNameNpcB_Click);
            // 
            // listZoneCb
            // 
            this.listZoneCb.DisplayMember = "Text";
            this.listZoneCb.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listZoneCb.ForeColor = System.Drawing.Color.Black;
            this.listZoneCb.FormattingEnabled = true;
            this.listZoneCb.ItemHeight = 16;
            this.listZoneCb.Location = new System.Drawing.Point(66, 34);
            this.listZoneCb.Name = "listZoneCb";
            this.listZoneCb.Size = new System.Drawing.Size(246, 22);
            this.listZoneCb.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.listZoneCb.TabIndex = 21;
            this.listZoneCb.SelectedIndexChanged += new System.EventHandler(this.listZoneCb_SelectedIndexChanged);
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.ForeColor = System.Drawing.Color.Black;
            this.labelX2.Location = new System.Drawing.Point(1, 34);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(59, 23);
            this.labelX2.TabIndex = 22;
            this.labelX2.Text = "List Zones:";
            // 
            // addZoneB
            // 
            this.addZoneB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.addZoneB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.addZoneB.Location = new System.Drawing.Point(318, 34);
            this.addZoneB.Name = "addZoneB";
            this.addZoneB.Size = new System.Drawing.Size(58, 23);
            this.addZoneB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.addZoneB.TabIndex = 23;
            this.addZoneB.Text = "Add Zone";
            this.addZoneB.Click += new System.EventHandler(this.addZoneB_Click);
            // 
            // zoneNameTb
            // 
            this.zoneNameTb.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.zoneNameTb.Border.Class = "TextBoxBorder";
            this.zoneNameTb.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.zoneNameTb.ForeColor = System.Drawing.Color.Black;
            this.zoneNameTb.Location = new System.Drawing.Point(77, 71);
            this.zoneNameTb.Name = "zoneNameTb";
            this.zoneNameTb.Size = new System.Drawing.Size(235, 22);
            this.zoneNameTb.TabIndex = 24;
            this.zoneNameTb.TextChanged += new System.EventHandler(this.zoneNameTb_TextChanged);
            // 
            // delZoneB
            // 
            this.delZoneB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.delZoneB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.delZoneB.Location = new System.Drawing.Point(318, 71);
            this.delZoneB.Name = "delZoneB";
            this.delZoneB.Size = new System.Drawing.Size(58, 23);
            this.delZoneB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.delZoneB.TabIndex = 25;
            this.delZoneB.Text = "Del Zone";
            this.delZoneB.Click += new System.EventHandler(this.delZoneB_Click);
            // 
            // labelX3
            // 
            this.labelX3.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.ForeColor = System.Drawing.Color.Black;
            this.labelX3.Location = new System.Drawing.Point(1, 71);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(70, 23);
            this.labelX3.TabIndex = 26;
            this.labelX3.Text = "Zone Name:";
            // 
            // labelX4
            // 
            this.labelX4.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.ForeColor = System.Drawing.Color.Black;
            this.labelX4.Location = new System.Drawing.Point(1, 105);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(70, 23);
            this.labelX4.TabIndex = 27;
            this.labelX4.Text = "Player Lvl Min";
            // 
            // minLevelPlayer
            // 
            this.minLevelPlayer.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.minLevelPlayer.BackgroundStyle.Class = "DateTimeInputBackground";
            this.minLevelPlayer.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.minLevelPlayer.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.minLevelPlayer.ForeColor = System.Drawing.Color.Black;
            this.minLevelPlayer.Location = new System.Drawing.Point(71, 106);
            this.minLevelPlayer.MaxValue = 90;
            this.minLevelPlayer.MinValue = 1;
            this.minLevelPlayer.Name = "minLevelPlayer";
            this.minLevelPlayer.ShowUpDown = true;
            this.minLevelPlayer.Size = new System.Drawing.Size(43, 22);
            this.minLevelPlayer.TabIndex = 28;
            this.minLevelPlayer.Value = 1;
            this.minLevelPlayer.ValueChanged += new System.EventHandler(this.Level_ValueChanged);
            // 
            // labelX5
            // 
            this.labelX5.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.ForeColor = System.Drawing.Color.Black;
            this.labelX5.Location = new System.Drawing.Point(115, 105);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(25, 23);
            this.labelX5.TabIndex = 29;
            this.labelX5.Text = "Max";
            // 
            // maxLevelPlayer
            // 
            this.maxLevelPlayer.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.maxLevelPlayer.BackgroundStyle.Class = "DateTimeInputBackground";
            this.maxLevelPlayer.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.maxLevelPlayer.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.maxLevelPlayer.ForeColor = System.Drawing.Color.Black;
            this.maxLevelPlayer.Location = new System.Drawing.Point(138, 106);
            this.maxLevelPlayer.MaxValue = 90;
            this.maxLevelPlayer.MinValue = 1;
            this.maxLevelPlayer.Name = "maxLevelPlayer";
            this.maxLevelPlayer.ShowUpDown = true;
            this.maxLevelPlayer.Size = new System.Drawing.Size(43, 22);
            this.maxLevelPlayer.TabIndex = 30;
            this.maxLevelPlayer.Value = 90;
            this.maxLevelPlayer.ValueChanged += new System.EventHandler(this.Level_ValueChanged);
            // 
            // maxLevelTarget
            // 
            this.maxLevelTarget.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.maxLevelTarget.BackgroundStyle.Class = "DateTimeInputBackground";
            this.maxLevelTarget.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.maxLevelTarget.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.maxLevelTarget.ForeColor = System.Drawing.Color.Black;
            this.maxLevelTarget.Location = new System.Drawing.Point(329, 107);
            this.maxLevelTarget.MaxValue = 100;
            this.maxLevelTarget.MinValue = 1;
            this.maxLevelTarget.Name = "maxLevelTarget";
            this.maxLevelTarget.ShowUpDown = true;
            this.maxLevelTarget.Size = new System.Drawing.Size(43, 22);
            this.maxLevelTarget.TabIndex = 34;
            this.maxLevelTarget.Value = 100;
            this.maxLevelTarget.ValueChanged += new System.EventHandler(this.Level_ValueChanged);
            // 
            // labelX6
            // 
            this.labelX6.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.labelX6.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX6.ForeColor = System.Drawing.Color.Black;
            this.labelX6.Location = new System.Drawing.Point(306, 106);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(25, 23);
            this.labelX6.TabIndex = 33;
            this.labelX6.Text = "Max";
            // 
            // minLevelTarget
            // 
            this.minLevelTarget.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.minLevelTarget.BackgroundStyle.Class = "DateTimeInputBackground";
            this.minLevelTarget.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.minLevelTarget.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.minLevelTarget.ForeColor = System.Drawing.Color.Black;
            this.minLevelTarget.Location = new System.Drawing.Point(259, 106);
            this.minLevelTarget.MaxValue = 100;
            this.minLevelTarget.MinValue = 1;
            this.minLevelTarget.Name = "minLevelTarget";
            this.minLevelTarget.ShowUpDown = true;
            this.minLevelTarget.Size = new System.Drawing.Size(43, 22);
            this.minLevelTarget.TabIndex = 32;
            this.minLevelTarget.Value = 1;
            this.minLevelTarget.ValueChanged += new System.EventHandler(this.Level_ValueChanged);
            // 
            // labelX7
            // 
            this.labelX7.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.labelX7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX7.ForeColor = System.Drawing.Color.Black;
            this.labelX7.Location = new System.Drawing.Point(187, 105);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(74, 23);
            this.labelX7.TabIndex = 31;
            this.labelX7.Text = "Target Lvl Min";
            // 
            // listEntryTb
            // 
            this.listEntryTb.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.listEntryTb.Border.Class = "TextBoxBorder";
            this.listEntryTb.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.listEntryTb.ForeColor = System.Drawing.Color.Black;
            this.listEntryTb.Location = new System.Drawing.Point(77, 136);
            this.listEntryTb.Name = "listEntryTb";
            this.listEntryTb.Size = new System.Drawing.Size(235, 22);
            this.listEntryTb.TabIndex = 35;
            this.listEntryTb.TextChanged += new System.EventHandler(this.listEntryTb_TextChanged);
            // 
            // addTargetEntryB
            // 
            this.addTargetEntryB.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.addTargetEntryB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.addTargetEntryB.Location = new System.Drawing.Point(318, 135);
            this.addTargetEntryB.Name = "addTargetEntryB";
            this.addTargetEntryB.Size = new System.Drawing.Size(58, 23);
            this.addTargetEntryB.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.addTargetEntryB.TabIndex = 36;
            this.addTargetEntryB.Text = "Add Target";
            this.addTargetEntryB.Click += new System.EventHandler(this.addTargetEntryB_Click);
            // 
            // labelX8
            // 
            this.labelX8.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.labelX8.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX8.ForeColor = System.Drawing.Color.Black;
            this.labelX8.Location = new System.Drawing.Point(1, 135);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(70, 23);
            this.labelX8.TabIndex = 37;
            this.labelX8.Text = "Ids of targets";
            // 
            // ProfileCreator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(100F, 100F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(379, 562);
            this.Controls.Add(this.labelX8);
            this.Controls.Add(this.addTargetEntryB);
            this.Controls.Add(this.listEntryTb);
            this.Controls.Add(this.maxLevelTarget);
            this.Controls.Add(this.labelX6);
            this.Controls.Add(this.minLevelTarget);
            this.Controls.Add(this.labelX7);
            this.Controls.Add(this.maxLevelPlayer);
            this.Controls.Add(this.labelX5);
            this.Controls.Add(this.minLevelPlayer);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.delZoneB);
            this.Controls.Add(this.zoneNameTb);
            this.Controls.Add(this.addZoneB);
            this.Controls.Add(this.labelX2);
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
            this.MaximizeBox = false;
            this.Name = "ProfileCreator";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Profile Creator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProfileCreator_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.nSeparatorDistance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radiusN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minLevelPlayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxLevelPlayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxLevelTarget)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minLevelTarget)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX recordWayB;
        private DevComponents.DotNetBar.ButtonX saveB;
        private System.Windows.Forms.ListBox listPoint;
        private DevComponents.Editors.IntegerInput nSeparatorDistance;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX delB;
        private DevComponents.DotNetBar.ButtonX delBlackRadius;
        private System.Windows.Forms.ListBox listBlackRadius;
        private DevComponents.Editors.IntegerInput radiusN;
        private DevComponents.DotNetBar.ButtonX addBlackB;
        private DevComponents.DotNetBar.ButtonX addNpcB;
        private DevComponents.DotNetBar.ButtonX delNpcB;
        private System.Windows.Forms.ListBox listNpc;
        private DevComponents.DotNetBar.Controls.ComboBoxEx npcTypeC;
        private DevComponents.DotNetBar.ButtonX loadB;
        private DevComponents.DotNetBar.Controls.TextBoxX nameNpcTb;
        private DevComponents.DotNetBar.ButtonX addByNameNpcB;
        private DevComponents.DotNetBar.Controls.ComboBoxEx listZoneCb;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.ButtonX addZoneB;
        private DevComponents.DotNetBar.Controls.TextBoxX zoneNameTb;
        private DevComponents.DotNetBar.ButtonX delZoneB;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.Editors.IntegerInput minLevelPlayer;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.Editors.IntegerInput maxLevelPlayer;
        private DevComponents.Editors.IntegerInput maxLevelTarget;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.Editors.IntegerInput minLevelTarget;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.DotNetBar.Controls.TextBoxX listEntryTb;
        private DevComponents.DotNetBar.ButtonX addTargetEntryB;
        private DevComponents.DotNetBar.LabelX labelX8;
    }
}