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
            this.recordWayB = new TnbButton();
            this.saveB = new TnbButton();
            this.listPoint = new System.Windows.Forms.ListBox();
            this.nSeparatorDistance = new NumericUpDown();
            this.labelX1 = new Label();
            this.delB = new TnbButton();
            this.delBlackRadius = new TnbButton();
            this.listBlackRadius = new System.Windows.Forms.ListBox();
            this.radiusN = new NumericUpDown();
            this.addBlackB = new TnbButton();
            this.addNpcB = new TnbButton();
            this.delNpcB = new TnbButton();
            this.listNpc = new System.Windows.Forms.ListBox();
            this.npcTypeC = new TnbComboBox();
            this.loadB = new TnbButton();
            this.nameNpcTb = new TextBox();
            this.addByNameNpcB = new TnbButton();
            this.listZoneCb = new TnbComboBox();
            this.labelX2 = new Label();
            this.addZoneB = new TnbButton();
            this.zoneNameTb = new TextBox();
            this.delZoneB = new TnbButton();
            this.labelX3 = new Label();
            this.labelX4 = new Label();
            this.minLevelPlayer = new NumericUpDown();
            this.labelX5 = new Label();
            this.maxLevelPlayer = new NumericUpDown();
            this.maxLevelTarget = new NumericUpDown();
            this.labelX6 = new Label();
            this.minLevelTarget = new NumericUpDown();
            this.labelX7 = new Label();
            this.listEntryTb = new TextBox();
            this.addTargetEntryB = new TnbButton();
            this.labelX8 = new Label();
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
            this.recordWayB.Location = new System.Drawing.Point(12, 304);
            this.recordWayB.Name = "recordWayB";
            this.recordWayB.Size = new System.Drawing.Size(352, 23);
            this.recordWayB.TabIndex = 1;
            this.recordWayB.Text = "Record Way";
            this.recordWayB.Click += new System.EventHandler(this.recordWayB_Click);
            // 
            // saveB
            // 
            this.saveB.Location = new System.Drawing.Point(318, 8);
            this.saveB.Name = "saveB";
            this.saveB.Size = new System.Drawing.Size(58, 19);
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
            this.nSeparatorDistance.ForeColor = System.Drawing.Color.Black;
            this.nSeparatorDistance.Location = new System.Drawing.Point(145, 5);
            this.nSeparatorDistance.Maximum = 200;
            this.nSeparatorDistance.Minimum = 3;
            this.nSeparatorDistance.Name = "nSeparatorDistance";
            this.nSeparatorDistance.Size = new System.Drawing.Size(94, 22);
            this.nSeparatorDistance.TabIndex = 5;
            this.nSeparatorDistance.Value = 5;
            // 
            // labelX1
            // 
            this.labelX1.ForeColor = System.Drawing.Color.Black;
            this.labelX1.Location = new System.Drawing.Point(1, 4);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(138, 23);
            this.labelX1.TabIndex = 6;
            this.labelX1.Text = "Separation distance record:";
            // 
            // delB
            // 
            this.delB.Location = new System.Drawing.Point(339, 164);
            this.delB.Name = "delB";
            this.delB.Size = new System.Drawing.Size(37, 23);
            this.delB.TabIndex = 8;
            this.delB.Text = "Del";
            this.delB.Click += new System.EventHandler(this.delB_Click);
            // 
            // delBlackRadius
            // 
            this.delBlackRadius.Location = new System.Drawing.Point(339, 341);
            this.delBlackRadius.Name = "delBlackRadius";
            this.delBlackRadius.Size = new System.Drawing.Size(37, 18);
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
            this.radiusN.ForeColor = System.Drawing.Color.Black;
            this.radiusN.Location = new System.Drawing.Point(12, 417);
            this.radiusN.Minimum = 1;
            this.radiusN.Maximum = 200;
            this.radiusN.Name = "radiusN";
            this.radiusN.Size = new System.Drawing.Size(113, 22);
            this.radiusN.TabIndex = 11;
            this.radiusN.Value = 35;
            // 
            // addBlackB
            // 
            this.addBlackB.Location = new System.Drawing.Point(131, 416);
            this.addBlackB.Name = "addBlackB";
            this.addBlackB.Size = new System.Drawing.Size(233, 23);
            this.addBlackB.TabIndex = 12;
            this.addBlackB.Text = "Add this position to Black list Radius";
            this.addBlackB.Click += new System.EventHandler(this.addBlackB_Click);
            // 
            // addNpcB
            // 
            this.addNpcB.Location = new System.Drawing.Point(160, 506);
            this.addNpcB.Name = "addNpcB";
            this.addNpcB.Size = new System.Drawing.Size(216, 23);
            this.addNpcB.TabIndex = 16;
            this.addNpcB.Text = "Add Target to Npc list";
            this.addNpcB.Click += new System.EventHandler(this.addNpcB_Click);
            // 
            // delNpcB
            // 
            this.delNpcB.Location = new System.Drawing.Point(339, 445);
            this.delNpcB.Name = "delNpcB";
            this.delNpcB.Size = new System.Drawing.Size(37, 18);
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
            this.npcTypeC.TabIndex = 17;
            // 
            // loadB
            // 
            this.loadB.Location = new System.Drawing.Point(254, 8);
            this.loadB.Name = "loadB";
            this.loadB.Size = new System.Drawing.Size(58, 19);
            this.loadB.TabIndex = 18;
            this.loadB.Text = "Load";
            this.loadB.Click += new System.EventHandler(this.loadB_Click);
            // 
            // nameNpcTb
            // 
            this.nameNpcTb.ForeColor = System.Drawing.Color.Black;
            this.nameNpcTb.Location = new System.Drawing.Point(160, 536);
            this.nameNpcTb.Name = "nameNpcTb";
            this.nameNpcTb.Size = new System.Drawing.Size(81, 22);
            this.nameNpcTb.TabIndex = 19;
            this.nameNpcTb.Text = "Name";
            // 
            // addByNameNpcB
            // 
            this.addByNameNpcB.Location = new System.Drawing.Point(247, 535);
            this.addByNameNpcB.Name = "addByNameNpcB";
            this.addByNameNpcB.Size = new System.Drawing.Size(129, 23);
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
            this.listZoneCb.TabIndex = 21;
            this.listZoneCb.SelectedIndexChanged += new System.EventHandler(this.listZoneCb_SelectedIndexChanged);
            // 
            // labelX2
            // 
            this.labelX2.ForeColor = System.Drawing.Color.Black;
            this.labelX2.Location = new System.Drawing.Point(1, 34);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(59, 23);
            this.labelX2.TabIndex = 22;
            this.labelX2.Text = "List Zones:";
            // 
            // addZoneB
            // 
            this.addZoneB.Location = new System.Drawing.Point(318, 34);
            this.addZoneB.Name = "addZoneB";
            this.addZoneB.Size = new System.Drawing.Size(58, 23);
            this.addZoneB.TabIndex = 23;
            this.addZoneB.Text = "Add Zone";
            this.addZoneB.Click += new System.EventHandler(this.addZoneB_Click);
            // 
            // zoneNameTb
            // 
            this.zoneNameTb.ForeColor = System.Drawing.Color.Black;
            this.zoneNameTb.Location = new System.Drawing.Point(77, 71);
            this.zoneNameTb.Name = "zoneNameTb";
            this.zoneNameTb.Size = new System.Drawing.Size(235, 22);
            this.zoneNameTb.TabIndex = 24;
            this.zoneNameTb.TextChanged += new System.EventHandler(this.zoneNameTb_TextChanged);
            // 
            // delZoneB
            // 
            this.delZoneB.Location = new System.Drawing.Point(318, 71);
            this.delZoneB.Name = "delZoneB";
            this.delZoneB.Size = new System.Drawing.Size(58, 23);
            this.delZoneB.TabIndex = 25;
            this.delZoneB.Text = "Del Zone";
            this.delZoneB.Click += new System.EventHandler(this.delZoneB_Click);
            // 
            // labelX3
            // 
            this.labelX3.ForeColor = System.Drawing.Color.Black;
            this.labelX3.Location = new System.Drawing.Point(1, 71);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(70, 23);
            this.labelX3.TabIndex = 26;
            this.labelX3.Text = "Zone Name:";
            // 
            // labelX4
            // 
            this.labelX4.ForeColor = System.Drawing.Color.Black;
            this.labelX4.Location = new System.Drawing.Point(1, 105);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(70, 23);
            this.labelX4.TabIndex = 27;
            this.labelX4.Text = "Player Lvl Min";
            // 
            // minLevelPlayer
            // 
            this.minLevelPlayer.ForeColor = System.Drawing.Color.Black;
            this.minLevelPlayer.Location = new System.Drawing.Point(71, 106);
            this.minLevelPlayer.Maximum = 90;
            this.minLevelPlayer.Minimum = 1;
            this.minLevelPlayer.Name = "minLevelPlayer";
            this.minLevelPlayer.Size = new System.Drawing.Size(43, 22);
            this.minLevelPlayer.TabIndex = 28;
            this.minLevelPlayer.Value = 1;
            this.minLevelPlayer.ValueChanged += new System.EventHandler(this.Level_ValueChanged);
            // 
            // labelX5
            // 
            this.labelX5.ForeColor = System.Drawing.Color.Black;
            this.labelX5.Location = new System.Drawing.Point(115, 105);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(25, 23);
            this.labelX5.TabIndex = 29;
            this.labelX5.Text = "Max";
            // 
            // maxLevelPlayer
            // 
            this.maxLevelPlayer.ForeColor = System.Drawing.Color.Black;
            this.maxLevelPlayer.Location = new System.Drawing.Point(138, 106);
            this.maxLevelPlayer.Maximum = 90;
            this.maxLevelPlayer.Minimum = 1;
            this.maxLevelPlayer.Name = "maxLevelPlayer";
            this.maxLevelPlayer.Size = new System.Drawing.Size(43, 22);
            this.maxLevelPlayer.TabIndex = 30;
            this.maxLevelPlayer.Value = 90;
            this.maxLevelPlayer.ValueChanged += new System.EventHandler(this.Level_ValueChanged);
            // 
            // maxLevelTarget
            // 
            this.maxLevelTarget.ForeColor = System.Drawing.Color.Black;
            this.maxLevelTarget.Location = new System.Drawing.Point(329, 107);
            this.maxLevelTarget.Maximum = 100;
            this.maxLevelTarget.Minimum = 1;
            this.maxLevelTarget.Name = "maxLevelTarget";
            this.maxLevelTarget.Size = new System.Drawing.Size(43, 22);
            this.maxLevelTarget.TabIndex = 34;
            this.maxLevelTarget.Value = 100;
            this.maxLevelTarget.ValueChanged += new System.EventHandler(this.Level_ValueChanged);
            // 
            // labelX6
            // 
            this.labelX6.ForeColor = System.Drawing.Color.Black;
            this.labelX6.Location = new System.Drawing.Point(306, 106);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(25, 23);
            this.labelX6.TabIndex = 33;
            this.labelX6.Text = "Max";
            // 
            // minLevelTarget
            // 
            this.minLevelTarget.ForeColor = System.Drawing.Color.Black;
            this.minLevelTarget.Location = new System.Drawing.Point(259, 106);
            this.minLevelTarget.Maximum = 100;
            this.minLevelTarget.Minimum = 1;
            this.minLevelTarget.Name = "minLevelTarget";
            this.minLevelTarget.Size = new System.Drawing.Size(43, 22);
            this.minLevelTarget.TabIndex = 32;
            this.minLevelTarget.Value = 1;
            this.minLevelTarget.ValueChanged += new System.EventHandler(this.Level_ValueChanged);
            // 
            // labelX7
            // 
            this.labelX7.ForeColor = System.Drawing.Color.Black;
            this.labelX7.Location = new System.Drawing.Point(187, 105);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(74, 23);
            this.labelX7.TabIndex = 31;
            this.labelX7.Text = "Target Lvl Min";
            // 
            // listEntryTb
            // 
            this.listEntryTb.ForeColor = System.Drawing.Color.Black;
            this.listEntryTb.Location = new System.Drawing.Point(77, 136);
            this.listEntryTb.Name = "listEntryTb";
            this.listEntryTb.Size = new System.Drawing.Size(235, 22);
            this.listEntryTb.TabIndex = 35;
            this.listEntryTb.TextChanged += new System.EventHandler(this.listEntryTb_TextChanged);
            // 
            // addTargetEntryB
            // 
            this.addTargetEntryB.Location = new System.Drawing.Point(318, 135);
            this.addTargetEntryB.Name = "addTargetEntryB";
            this.addTargetEntryB.Size = new System.Drawing.Size(58, 23);
            this.addTargetEntryB.TabIndex = 36;
            this.addTargetEntryB.Text = "Add Target";
            this.addTargetEntryB.Click += new System.EventHandler(this.addTargetEntryB_Click);
            // 
            // labelX8
            // 
            this.labelX8.ForeColor = System.Drawing.Color.Black;
            this.labelX8.Location = new System.Drawing.Point(1, 135);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(70, 23);
            this.labelX8.TabIndex = 37;
            this.labelX8.Text = "Ids of targets";
            // 
            // ProfileCreator
            // 
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
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
            this.ShowInTaskbar = true;
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
        private Label labelX2;
        private TnbButton addZoneB;
        private TextBox zoneNameTb;
        private TnbButton delZoneB;
        private Label labelX3;
        private Label labelX4;
        private NumericUpDown minLevelPlayer;
        private Label labelX5;
        private NumericUpDown maxLevelPlayer;
        private NumericUpDown maxLevelTarget;
        private Label labelX6;
        private NumericUpDown minLevelTarget;
        private Label labelX7;
        private TextBox listEntryTb;
        private TnbButton addTargetEntryB;
        private Label labelX8;
    }
}