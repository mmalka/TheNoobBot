using meshBuilderGui.Properties;

namespace meshBuilderGui
{
    partial class Interface
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Interface));
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.startXBox = new System.Windows.Forms.TextBox();
            this.startYBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.countXBox = new System.Windows.Forms.TextBox();
            this.countYBox = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.statL = new System.Windows.Forms.Label();
            this.timeLeftL = new System.Windows.Forms.Label();
            this.continentNameCB = new System.Windows.Forms.ComboBox();
            this.wowDirCB = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.meshTB = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.buildDisplay1 = new meshBuilderGui.BuildDisplay();
            this.populate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(206, 319);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(64, 20);
            this.button1.TabIndex = 1;
            this.button1.Text = "Start Build";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button1.Enabled = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 349);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Start Tile X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 374);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Start Tile Y";
            // 
            // startXBox
            // 
            this.startXBox.Location = new System.Drawing.Point(70, 346);
            this.startXBox.Name = "startXBox";
            this.startXBox.Size = new System.Drawing.Size(62, 20);
            this.startXBox.TabIndex = 5;
            this.startXBox.Text = "0";
            // 
            // startYBox
            // 
            this.startYBox.Location = new System.Drawing.Point(70, 371);
            this.startYBox.Name = "startYBox";
            this.startYBox.Size = new System.Drawing.Size(62, 20);
            this.startYBox.TabIndex = 6;
            this.startYBox.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(138, 349);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Count";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(138, 374);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Count";
            // 
            // countXBox
            // 
            this.countXBox.Location = new System.Drawing.Point(179, 346);
            this.countXBox.Name = "countXBox";
            this.countXBox.Size = new System.Drawing.Size(91, 20);
            this.countXBox.TabIndex = 9;
            // 
            // countYBox
            // 
            this.countYBox.Location = new System.Drawing.Point(179, 371);
            this.countYBox.Name = "countYBox";
            this.countYBox.Size = new System.Drawing.Size(91, 20);
            this.countYBox.TabIndex = 10;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 250;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // statL
            // 
            this.statL.AutoSize = true;
            this.statL.Location = new System.Drawing.Point(5, 397);
            this.statL.Name = "statL";
            this.statL.Size = new System.Drawing.Size(58, 13);
            this.statL.TabIndex = 11;
            this.statL.Text = "No Started";
            // 
            // timeLeftL
            // 
            this.timeLeftL.AutoSize = true;
            this.timeLeftL.Location = new System.Drawing.Point(157, 397);
            this.timeLeftL.Name = "timeLeftL";
            this.timeLeftL.Size = new System.Drawing.Size(51, 13);
            this.timeLeftL.TabIndex = 12;
            this.timeLeftL.Text = "Time Left";
            // 
            // continentNameCB
            // 
            /*this.continentNameCB.AutoCompleteCustomSource.AddRange(new string[] {
            "Azeroth",
            "Kalimdor",
            "Expansion01",
            "Northrend",
            "HawaiiMainLand",
            "Draenor",
            "DeathKnightStart",
            "Gilneas",
            "Gilneas2",
            "TanaanJungleIntro",
            "Ashran",
            "LostIsles",
            "NewRaceStartZone",
            "PVPZone01",
            "PVPZone03",
            "PVPZone04",
            "NetherstormBG",
            "NorthrendBG",
            "IsleofConquest",
            "CataclysmCTF",
            "TolBarad",
            "Gilneas_BG_2",
            "ValleyOfPower",
            "SunwellPlateau",
            "Deephome",
            "DarkmoonFaire",
            "MoguIslandDailyArea",
            "EyeoftheStorm2.0",
            "GoldRushBG",
            "FWHordeGarrisonLevel1",
            "FWHordeGarrisonLeve2new",
            "FWHordeGarrisonLevel2",
            "SMVAllianceGarrisonLevel1",
            "SMVAllianceGarrisonLevel2new",
            "SMVAllianceGarrisonLevel2",
            "BlackrockFoundryRaid",
            "StormwindJail",
            "OnyxiaLairInstance"});*/
            this.continentNameCB.FormattingEnabled = true;
            this.continentNameCB.Items.AddRange(new object[] {
            "Please click on list button"
            /*"Azeroth",
            "Kalimdor",
            "Expansion01",
            "Northrend",
            "HawaiiMainLand",
            "Draenor",
            "DeathKnightStart",
            "Gilneas",
            "Gilneas2",
            "TanaanJungleIntro",
            "Ashran",
            "LostIsles",
            "NewRaceStartZone",
            "PVPZone01",
            "PVPZone03",
            "PVPZone04",
            "NetherstormBG",
            "NorthrendBG",
            "IsleofConquest",
            "CataclysmCTF",
            "TolBarad",
            "Gilneas_BG_2",
            "ValleyOfPower",
            "SunwellPlateau",
            "Deephome",
            "DarkmoonFaire",
            "MoguIslandDailyArea",
            "EyeoftheStorm2.0",
            "GoldRushBG",
            "FWHordeGarrisonLevel1",
            "FWHordeGarrisonLeve2new",
            "FWHordeGarrisonLevel2",
            "SMVAllianceGarrisonLevel1",
            "SMVAllianceGarrisonLevel2new",
            "SMVAllianceGarrisonLevel2",
            "BlackrockFoundryRaid",
            "StormwindJail",
            "OnyxiaLairInstance"*/});
            this.continentNameCB.Location = new System.Drawing.Point(8, 318);
            this.continentNameCB.Name = "continentNameCB";
            this.continentNameCB.Size = new System.Drawing.Size(158, 21);
            this.continentNameCB.TabIndex = 13;
            this.continentNameCB.SelectedIndex = 0;
            // 
            // wowDirCB
            // 
            this.wowDirCB.AutoCompleteCustomSource.AddRange(new string[] { Settings.Default.WoWPath });
            this.wowDirCB.FormattingEnabled = true;
            this.wowDirCB.Items.AddRange(new object[] { Settings.Default.WoWPath });
            this.wowDirCB.Location = new System.Drawing.Point(62, 274);
            this.wowDirCB.Name = "wowDirCB";
            this.wowDirCB.Size = new System.Drawing.Size(208, 21);
            this.wowDirCB.TabIndex = 14;
            this.wowDirCB.SelectedIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 277);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Wow Dir:";
            // 
            // meshTB
            // 
            this.meshTB.Location = new System.Drawing.Point(62, 294);
            this.meshTB.Name = "meshTB";
            this.meshTB.Size = new System.Drawing.Size(208, 20);
            this.meshTB.TabIndex = 16;
            this.meshTB.Text = global::meshBuilderGui.Properties.Settings.Default.MeshesPath;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 296);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Meshes:";
            // 
            // buildDisplay1
            // 
            this.buildDisplay1.Location = new System.Drawing.Point(7, 6);
            this.buildDisplay1.Name = "buildDisplay1";
            this.buildDisplay1.Size = new System.Drawing.Size(262, 262);
            this.buildDisplay1.TabIndex = 0;
            // 
            // populate
            // 
            this.populate.Location = new System.Drawing.Point(170, 319);
            this.populate.Name = "populate";
            this.populate.Size = new System.Drawing.Size(34, 20);
            this.populate.TabIndex = 18;
            this.populate.Text = "List";
            this.populate.UseVisualStyleBackColor = true;
            this.populate.Click += new System.EventHandler(this.populate_Click);
            // 
            // Interface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(278, 417);
            this.Controls.Add(this.populate);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.meshTB);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.wowDirCB);
            this.Controls.Add(this.continentNameCB);
            this.Controls.Add(this.timeLeftL);
            this.Controls.Add(this.statL);
            this.Controls.Add(this.countYBox);
            this.Controls.Add(this.countXBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.startYBox);
            this.Controls.Add(this.startXBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buildDisplay1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Interface";
            this.Text = "Mesh Builder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Interface_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BuildDisplay buildDisplay1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox startXBox;
        private System.Windows.Forms.TextBox startYBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox countXBox;
        private System.Windows.Forms.TextBox countYBox;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label statL;
        private System.Windows.Forms.Label timeLeftL;
        private System.Windows.Forms.ComboBox continentNameCB;
        private System.Windows.Forms.ComboBox wowDirCB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox meshTB;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button populate;


    }
}

