using System.Windows.Forms;
using nManager.Helpful.Forms.UserControls;

namespace Battlegrounder
{
    partial class SettingsBattlegrounderForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsBattlegrounderForm));
            this.WarsongGulchSwitch = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.ArathiBasinSwitch = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.AlteracValleySwitch = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.EyeoftheStormSwitch = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.StrandoftheAncientsSwitch = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.WarsongGulchLabel = new System.Windows.Forms.Label();
            this.ArathiBasinLabel = new System.Windows.Forms.Label();
            this.AlteracValleyLabel = new System.Windows.Forms.Label();
            this.EyeoftheStormLabel = new System.Windows.Forms.Label();
            this.IsleofConquestLabel = new System.Windows.Forms.Label();
            this.IsleofConquestSwitch = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.BattleforGilneasSwitch = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.BattleforGilneasLabel = new System.Windows.Forms.Label();
            this.TwinPeaksLabel = new System.Windows.Forms.Label();
            this.TwinPeaksSwitch = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.RandomBattlegroundLabel = new System.Windows.Forms.Label();
            this.RandomBattlegroundSwitch = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.SilvershardMinesLabel = new System.Windows.Forms.Label();
            this.TempleOfKotmoguLabel = new System.Windows.Forms.Label();
            this.SilvershardMinesSwitch = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.TempleOfKotmoguSwitch = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.RequeueAfterXMinutesLabel = new System.Windows.Forms.Label();
            this.RequeueAfterXMinutesSwitch = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.RequeueAfterXMinutes = new System.Windows.Forms.NumericUpDown();
            this.AlteracValley = new System.Windows.Forms.GroupBox();
            this.XMLProfileListAlteracValleyLabel = new System.Windows.Forms.Label();
            this.AlteracValleyProfileTypeLabel = new System.Windows.Forms.Label();
            this.IsleofConquest = new System.Windows.Forms.GroupBox();
            this.XMLProfileListIsleofConquestLabel = new System.Windows.Forms.Label();
            this.IsleofConquestProfileTypeLabel = new System.Windows.Forms.Label();
            this.TwinPeaks = new System.Windows.Forms.GroupBox();
            this.XMLProfileListTwinPeaksLabel = new System.Windows.Forms.Label();
            this.TwinPeaksProfileTypeLabel = new System.Windows.Forms.Label();
            this.WarsongGulch = new System.Windows.Forms.GroupBox();
            this.XMLProfileListWarsongGulchLabel = new System.Windows.Forms.Label();
            this.WarsongGulchProfileTypeLabel = new System.Windows.Forms.Label();
            this.TempleofKotmogu = new System.Windows.Forms.GroupBox();
            this.XMLProfileListTempleofKotmoguLabel = new System.Windows.Forms.Label();
            this.TempleofKotmoguProfileTypeLabel = new System.Windows.Forms.Label();
            this.BattleforGilneas = new System.Windows.Forms.GroupBox();
            this.XMLProfileListBattleforGilneasLabel = new System.Windows.Forms.Label();
            this.labelX3 = new System.Windows.Forms.Label();
            this.EyeoftheStorm = new System.Windows.Forms.GroupBox();
            this.XMLProfileListEyeoftheStormLabel = new System.Windows.Forms.Label();
            this.EyeoftheStormProfileTypeLabel = new System.Windows.Forms.Label();
            this.ArathiBasin = new System.Windows.Forms.GroupBox();
            this.XMLProfileListArathiBasinLabel = new System.Windows.Forms.Label();
            this.ArathiBasinProfileTypeLabel = new System.Windows.Forms.Label();
            this.SilvershardMines = new System.Windows.Forms.GroupBox();
            this.XMLProfileListSilvershardMinesLabel = new System.Windows.Forms.Label();
            this.SilvershardMinesProfileTypeLabel = new System.Windows.Forms.Label();
            this.StrandoftheAncients = new System.Windows.Forms.GroupBox();
            this.XMLProfileListStrandoftheAncientsLabel = new System.Windows.Forms.Label();
            this.StrandoftheAncientsProfileTypeLabel = new System.Windows.Forms.Label();
            this.StrandoftheAncientsLabel = new System.Windows.Forms.Label();
            this.RandomBattleground = new System.Windows.Forms.GroupBox();
            this.RequeueAfterXMinutesGroupBox = new System.Windows.Forms.GroupBox();
            this.MainHeader = new nManager.Helpful.Forms.UserControls.TnbControlMenu();
            this.createProfileB = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.CloseNoSaveButton = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.XMLProfileListSilvershardMines = new nManager.Helpful.Forms.UserControls.TnbComboBox();
            this.SilvershardMinesProfileType = new nManager.Helpful.Forms.UserControls.TnbComboBox();
            this.TempleofKotmoguProfileType = new nManager.Helpful.Forms.UserControls.TnbComboBox();
            this.XMLProfileListTempleofKotmogu = new nManager.Helpful.Forms.UserControls.TnbComboBox();
            this.XMLProfileListStrandoftheAncients = new nManager.Helpful.Forms.UserControls.TnbComboBox();
            this.StrandoftheAncientsProfileType = new nManager.Helpful.Forms.UserControls.TnbComboBox();
            this.XMLProfileListTwinPeaks = new nManager.Helpful.Forms.UserControls.TnbComboBox();
            this.TwinPeaksProfileType = new nManager.Helpful.Forms.UserControls.TnbComboBox();
            this.XMLProfileListBattleforGilneas = new nManager.Helpful.Forms.UserControls.TnbComboBox();
            this.BattleforGilneasProfileType = new nManager.Helpful.Forms.UserControls.TnbComboBox();
            this.IsleofConquestProfileType = new nManager.Helpful.Forms.UserControls.TnbComboBox();
            this.XMLProfileListIsleofConquest = new nManager.Helpful.Forms.UserControls.TnbComboBox();
            this.XMLProfileListEyeoftheStorm = new nManager.Helpful.Forms.UserControls.TnbComboBox();
            this.EyeoftheStormProfileType = new nManager.Helpful.Forms.UserControls.TnbComboBox();
            this.XMLProfileListWarsongGulch = new nManager.Helpful.Forms.UserControls.TnbComboBox();
            this.WarsongGulchProfileType = new nManager.Helpful.Forms.UserControls.TnbComboBox();
            this.XMLProfileListArathiBasin = new nManager.Helpful.Forms.UserControls.TnbComboBox();
            this.ArathiBasinProfileType = new nManager.Helpful.Forms.UserControls.TnbComboBox();
            this.XMLProfileListAlteracValley = new nManager.Helpful.Forms.UserControls.TnbComboBox();
            this.AlteracValleyProfileType = new nManager.Helpful.Forms.UserControls.TnbComboBox();
            this.SaveButton = new nManager.Helpful.Forms.UserControls.TnbButton();
            ((System.ComponentModel.ISupportInitialize)(this.RequeueAfterXMinutes)).BeginInit();
            this.AlteracValley.SuspendLayout();
            this.IsleofConquest.SuspendLayout();
            this.TwinPeaks.SuspendLayout();
            this.WarsongGulch.SuspendLayout();
            this.TempleofKotmogu.SuspendLayout();
            this.BattleforGilneas.SuspendLayout();
            this.EyeoftheStorm.SuspendLayout();
            this.ArathiBasin.SuspendLayout();
            this.SilvershardMines.SuspendLayout();
            this.StrandoftheAncients.SuspendLayout();
            this.RandomBattleground.SuspendLayout();
            this.RequeueAfterXMinutesGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // WarsongGulchSwitch
            // 
            this.WarsongGulchSwitch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.WarsongGulchSwitch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.WarsongGulchSwitch.Location = new System.Drawing.Point(173, 20);
            this.WarsongGulchSwitch.MaximumSize = new System.Drawing.Size(60, 20);
            this.WarsongGulchSwitch.MinimumSize = new System.Drawing.Size(60, 20);
            this.WarsongGulchSwitch.Name = "WarsongGulchSwitch";
            this.WarsongGulchSwitch.OffText = "OFF";
            this.WarsongGulchSwitch.OnText = "ON";
            this.WarsongGulchSwitch.Size = new System.Drawing.Size(60, 20);
            this.WarsongGulchSwitch.TabIndex = 0;
            this.WarsongGulchSwitch.Value = false;
            this.WarsongGulchSwitch.ValueChanged += new System.EventHandler(this.CheckAllSwitchEvent);
            // 
            // ArathiBasinSwitch
            // 
            this.ArathiBasinSwitch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.ArathiBasinSwitch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.ArathiBasinSwitch.Location = new System.Drawing.Point(173, 20);
            this.ArathiBasinSwitch.MaximumSize = new System.Drawing.Size(60, 20);
            this.ArathiBasinSwitch.MinimumSize = new System.Drawing.Size(60, 20);
            this.ArathiBasinSwitch.Name = "ArathiBasinSwitch";
            this.ArathiBasinSwitch.OffText = "OFF";
            this.ArathiBasinSwitch.OnText = "ON";
            this.ArathiBasinSwitch.Size = new System.Drawing.Size(60, 20);
            this.ArathiBasinSwitch.TabIndex = 21;
            this.ArathiBasinSwitch.Value = false;
            this.ArathiBasinSwitch.ValueChanged += new System.EventHandler(this.CheckAllSwitchEvent);
            // 
            // AlteracValleySwitch
            // 
            this.AlteracValleySwitch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.AlteracValleySwitch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.AlteracValleySwitch.Location = new System.Drawing.Point(175, 20);
            this.AlteracValleySwitch.MaximumSize = new System.Drawing.Size(60, 20);
            this.AlteracValleySwitch.MinimumSize = new System.Drawing.Size(60, 20);
            this.AlteracValleySwitch.Name = "AlteracValleySwitch";
            this.AlteracValleySwitch.OffText = "OFF";
            this.AlteracValleySwitch.OnText = "ON";
            this.AlteracValleySwitch.Size = new System.Drawing.Size(60, 20);
            this.AlteracValleySwitch.TabIndex = 22;
            this.AlteracValleySwitch.Value = false;
            this.AlteracValleySwitch.ValueChanged += new System.EventHandler(this.CheckAllSwitchEvent);
            // 
            // EyeoftheStormSwitch
            // 
            this.EyeoftheStormSwitch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.EyeoftheStormSwitch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.EyeoftheStormSwitch.Location = new System.Drawing.Point(173, 20);
            this.EyeoftheStormSwitch.MaximumSize = new System.Drawing.Size(60, 20);
            this.EyeoftheStormSwitch.MinimumSize = new System.Drawing.Size(60, 20);
            this.EyeoftheStormSwitch.Name = "EyeoftheStormSwitch";
            this.EyeoftheStormSwitch.OffText = "OFF";
            this.EyeoftheStormSwitch.OnText = "ON";
            this.EyeoftheStormSwitch.Size = new System.Drawing.Size(60, 20);
            this.EyeoftheStormSwitch.TabIndex = 23;
            this.EyeoftheStormSwitch.Value = false;
            this.EyeoftheStormSwitch.ValueChanged += new System.EventHandler(this.CheckAllSwitchEvent);
            // 
            // StrandoftheAncientsSwitch
            // 
            this.StrandoftheAncientsSwitch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.StrandoftheAncientsSwitch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.StrandoftheAncientsSwitch.Location = new System.Drawing.Point(173, 20);
            this.StrandoftheAncientsSwitch.MaximumSize = new System.Drawing.Size(60, 20);
            this.StrandoftheAncientsSwitch.MinimumSize = new System.Drawing.Size(60, 20);
            this.StrandoftheAncientsSwitch.Name = "StrandoftheAncientsSwitch";
            this.StrandoftheAncientsSwitch.OffText = "OFF";
            this.StrandoftheAncientsSwitch.OnText = "ON";
            this.StrandoftheAncientsSwitch.Size = new System.Drawing.Size(60, 20);
            this.StrandoftheAncientsSwitch.TabIndex = 24;
            this.StrandoftheAncientsSwitch.Value = false;
            this.StrandoftheAncientsSwitch.ValueChanged += new System.EventHandler(this.CheckAllSwitchEvent);
            // 
            // WarsongGulchLabel
            // 
            this.WarsongGulchLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.WarsongGulchLabel.Location = new System.Drawing.Point(6, 16);
            this.WarsongGulchLabel.Name = "WarsongGulchLabel";
            this.WarsongGulchLabel.Size = new System.Drawing.Size(161, 22);
            this.WarsongGulchLabel.TabIndex = 25;
            this.WarsongGulchLabel.Text = "Queue for Warsong Gulch";
            // 
            // ArathiBasinLabel
            // 
            this.ArathiBasinLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.ArathiBasinLabel.Location = new System.Drawing.Point(6, 16);
            this.ArathiBasinLabel.Name = "ArathiBasinLabel";
            this.ArathiBasinLabel.Size = new System.Drawing.Size(161, 22);
            this.ArathiBasinLabel.TabIndex = 26;
            this.ArathiBasinLabel.Text = "Queue for Arathi Basin";
            // 
            // AlteracValleyLabel
            // 
            this.AlteracValleyLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.AlteracValleyLabel.Location = new System.Drawing.Point(6, 16);
            this.AlteracValleyLabel.Name = "AlteracValleyLabel";
            this.AlteracValleyLabel.Size = new System.Drawing.Size(161, 22);
            this.AlteracValleyLabel.TabIndex = 27;
            this.AlteracValleyLabel.Text = "Queue for Alterac Valley";
            // 
            // EyeoftheStormLabel
            // 
            this.EyeoftheStormLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.EyeoftheStormLabel.Location = new System.Drawing.Point(6, 16);
            this.EyeoftheStormLabel.Name = "EyeoftheStormLabel";
            this.EyeoftheStormLabel.Size = new System.Drawing.Size(161, 22);
            this.EyeoftheStormLabel.TabIndex = 28;
            this.EyeoftheStormLabel.Text = "Queue for Eye of the Storm";
            // 
            // IsleofConquestLabel
            // 
            this.IsleofConquestLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.IsleofConquestLabel.Location = new System.Drawing.Point(6, 16);
            this.IsleofConquestLabel.Name = "IsleofConquestLabel";
            this.IsleofConquestLabel.Size = new System.Drawing.Size(161, 22);
            this.IsleofConquestLabel.TabIndex = 30;
            this.IsleofConquestLabel.Text = "Queue for Isle of Conquest";
            // 
            // IsleofConquestSwitch
            // 
            this.IsleofConquestSwitch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.IsleofConquestSwitch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.IsleofConquestSwitch.Location = new System.Drawing.Point(173, 20);
            this.IsleofConquestSwitch.MaximumSize = new System.Drawing.Size(60, 20);
            this.IsleofConquestSwitch.MinimumSize = new System.Drawing.Size(60, 20);
            this.IsleofConquestSwitch.Name = "IsleofConquestSwitch";
            this.IsleofConquestSwitch.OffText = "OFF";
            this.IsleofConquestSwitch.OnText = "ON";
            this.IsleofConquestSwitch.Size = new System.Drawing.Size(60, 20);
            this.IsleofConquestSwitch.TabIndex = 31;
            this.IsleofConquestSwitch.Value = false;
            this.IsleofConquestSwitch.ValueChanged += new System.EventHandler(this.CheckAllSwitchEvent);
            // 
            // BattleforGilneasSwitch
            // 
            this.BattleforGilneasSwitch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.BattleforGilneasSwitch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.BattleforGilneasSwitch.Location = new System.Drawing.Point(173, 20);
            this.BattleforGilneasSwitch.MaximumSize = new System.Drawing.Size(60, 20);
            this.BattleforGilneasSwitch.MinimumSize = new System.Drawing.Size(60, 20);
            this.BattleforGilneasSwitch.Name = "BattleforGilneasSwitch";
            this.BattleforGilneasSwitch.OffText = "OFF";
            this.BattleforGilneasSwitch.OnText = "ON";
            this.BattleforGilneasSwitch.Size = new System.Drawing.Size(60, 20);
            this.BattleforGilneasSwitch.TabIndex = 32;
            this.BattleforGilneasSwitch.Value = false;
            this.BattleforGilneasSwitch.ValueChanged += new System.EventHandler(this.CheckAllSwitchEvent);
            // 
            // BattleforGilneasLabel
            // 
            this.BattleforGilneasLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.BattleforGilneasLabel.Location = new System.Drawing.Point(6, 16);
            this.BattleforGilneasLabel.Name = "BattleforGilneasLabel";
            this.BattleforGilneasLabel.Size = new System.Drawing.Size(161, 22);
            this.BattleforGilneasLabel.TabIndex = 33;
            this.BattleforGilneasLabel.Text = "Queue for Battle for Gilneas";
            // 
            // TwinPeaksLabel
            // 
            this.TwinPeaksLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.TwinPeaksLabel.Location = new System.Drawing.Point(6, 16);
            this.TwinPeaksLabel.Name = "TwinPeaksLabel";
            this.TwinPeaksLabel.Size = new System.Drawing.Size(161, 22);
            this.TwinPeaksLabel.TabIndex = 34;
            this.TwinPeaksLabel.Text = "Queue for Twin Peaks";
            // 
            // TwinPeaksSwitch
            // 
            this.TwinPeaksSwitch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.TwinPeaksSwitch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.TwinPeaksSwitch.Location = new System.Drawing.Point(173, 20);
            this.TwinPeaksSwitch.MaximumSize = new System.Drawing.Size(60, 20);
            this.TwinPeaksSwitch.MinimumSize = new System.Drawing.Size(60, 20);
            this.TwinPeaksSwitch.Name = "TwinPeaksSwitch";
            this.TwinPeaksSwitch.OffText = "OFF";
            this.TwinPeaksSwitch.OnText = "ON";
            this.TwinPeaksSwitch.Size = new System.Drawing.Size(60, 20);
            this.TwinPeaksSwitch.TabIndex = 35;
            this.TwinPeaksSwitch.Value = false;
            this.TwinPeaksSwitch.ValueChanged += new System.EventHandler(this.CheckAllSwitchEvent);
            // 
            // RandomBattlegroundLabel
            // 
            this.RandomBattlegroundLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.RandomBattlegroundLabel.Location = new System.Drawing.Point(6, 20);
            this.RandomBattlegroundLabel.Name = "RandomBattlegroundLabel";
            this.RandomBattlegroundLabel.Size = new System.Drawing.Size(230, 22);
            this.RandomBattlegroundLabel.TabIndex = 36;
            this.RandomBattlegroundLabel.Text = "Queue in Random Battleground";
            // 
            // RandomBattlegroundSwitch
            // 
            this.RandomBattlegroundSwitch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.RandomBattlegroundSwitch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.RandomBattlegroundSwitch.Location = new System.Drawing.Point(86, 46);
            this.RandomBattlegroundSwitch.MaximumSize = new System.Drawing.Size(60, 20);
            this.RandomBattlegroundSwitch.MinimumSize = new System.Drawing.Size(60, 20);
            this.RandomBattlegroundSwitch.Name = "RandomBattlegroundSwitch";
            this.RandomBattlegroundSwitch.OffText = "OFF";
            this.RandomBattlegroundSwitch.OnText = "ON";
            this.RandomBattlegroundSwitch.Size = new System.Drawing.Size(60, 20);
            this.RandomBattlegroundSwitch.TabIndex = 37;
            this.RandomBattlegroundSwitch.Value = false;
            this.RandomBattlegroundSwitch.ValueChanged += new System.EventHandler(this.RandomBattlegroundSwitchValueChanged);
            // 
            // SilvershardMinesLabel
            // 
            this.SilvershardMinesLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.SilvershardMinesLabel.Location = new System.Drawing.Point(6, 16);
            this.SilvershardMinesLabel.Name = "SilvershardMinesLabel";
            this.SilvershardMinesLabel.Size = new System.Drawing.Size(161, 22);
            this.SilvershardMinesLabel.TabIndex = 41;
            this.SilvershardMinesLabel.Text = "Queue for Silvershard Mines";
            // 
            // TempleOfKotmoguLabel
            // 
            this.TempleOfKotmoguLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.TempleOfKotmoguLabel.Location = new System.Drawing.Point(6, 16);
            this.TempleOfKotmoguLabel.Name = "TempleOfKotmoguLabel";
            this.TempleOfKotmoguLabel.Size = new System.Drawing.Size(161, 22);
            this.TempleOfKotmoguLabel.TabIndex = 40;
            this.TempleOfKotmoguLabel.Text = "Queue for Temple of Kotmogu";
            // 
            // SilvershardMinesSwitch
            // 
            this.SilvershardMinesSwitch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.SilvershardMinesSwitch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.SilvershardMinesSwitch.Location = new System.Drawing.Point(173, 20);
            this.SilvershardMinesSwitch.MaximumSize = new System.Drawing.Size(60, 20);
            this.SilvershardMinesSwitch.MinimumSize = new System.Drawing.Size(60, 20);
            this.SilvershardMinesSwitch.Name = "SilvershardMinesSwitch";
            this.SilvershardMinesSwitch.OffText = "OFF";
            this.SilvershardMinesSwitch.OnText = "ON";
            this.SilvershardMinesSwitch.Size = new System.Drawing.Size(60, 20);
            this.SilvershardMinesSwitch.TabIndex = 39;
            this.SilvershardMinesSwitch.Value = false;
            this.SilvershardMinesSwitch.ValueChanged += new System.EventHandler(this.CheckAllSwitchEvent);
            // 
            // TempleOfKotmoguSwitch
            // 
            this.TempleOfKotmoguSwitch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.TempleOfKotmoguSwitch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.TempleOfKotmoguSwitch.Location = new System.Drawing.Point(173, 20);
            this.TempleOfKotmoguSwitch.MaximumSize = new System.Drawing.Size(60, 20);
            this.TempleOfKotmoguSwitch.MinimumSize = new System.Drawing.Size(60, 20);
            this.TempleOfKotmoguSwitch.Name = "TempleOfKotmoguSwitch";
            this.TempleOfKotmoguSwitch.OffText = "OFF";
            this.TempleOfKotmoguSwitch.OnText = "ON";
            this.TempleOfKotmoguSwitch.Size = new System.Drawing.Size(60, 20);
            this.TempleOfKotmoguSwitch.TabIndex = 38;
            this.TempleOfKotmoguSwitch.Value = false;
            this.TempleOfKotmoguSwitch.ValueChanged += new System.EventHandler(this.CheckAllSwitchEvent);
            // 
            // RequeueAfterXMinutesLabel
            // 
            this.RequeueAfterXMinutesLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.RequeueAfterXMinutesLabel.Location = new System.Drawing.Point(6, 20);
            this.RequeueAfterXMinutesLabel.Name = "RequeueAfterXMinutesLabel";
            this.RequeueAfterXMinutesLabel.Size = new System.Drawing.Size(192, 22);
            this.RequeueAfterXMinutesLabel.TabIndex = 43;
            this.RequeueAfterXMinutesLabel.Text = "Requeue after X minutes";
            // 
            // RequeueAfterXMinutesSwitch
            // 
            this.RequeueAfterXMinutesSwitch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.RequeueAfterXMinutesSwitch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.RequeueAfterXMinutesSwitch.Location = new System.Drawing.Point(86, 46);
            this.RequeueAfterXMinutesSwitch.MaximumSize = new System.Drawing.Size(60, 20);
            this.RequeueAfterXMinutesSwitch.MinimumSize = new System.Drawing.Size(60, 20);
            this.RequeueAfterXMinutesSwitch.Name = "RequeueAfterXMinutesSwitch";
            this.RequeueAfterXMinutesSwitch.OffText = "OFF";
            this.RequeueAfterXMinutesSwitch.OnText = "ON";
            this.RequeueAfterXMinutesSwitch.Size = new System.Drawing.Size(60, 20);
            this.RequeueAfterXMinutesSwitch.TabIndex = 42;
            this.RequeueAfterXMinutesSwitch.Value = false;
            // 
            // RequeueAfterXMinutes
            // 
            this.RequeueAfterXMinutes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.RequeueAfterXMinutes.Location = new System.Drawing.Point(207, 21);
            this.RequeueAfterXMinutes.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.RequeueAfterXMinutes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.RequeueAfterXMinutes.Name = "RequeueAfterXMinutes";
            this.RequeueAfterXMinutes.Size = new System.Drawing.Size(29, 22);
            this.RequeueAfterXMinutes.TabIndex = 44;
            this.RequeueAfterXMinutes.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // AlteracValley
            // 
            this.AlteracValley.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.AlteracValley.Controls.Add(this.XMLProfileListAlteracValleyLabel);
            this.AlteracValley.Controls.Add(this.XMLProfileListAlteracValley);
            this.AlteracValley.Controls.Add(this.AlteracValleyProfileType);
            this.AlteracValley.Controls.Add(this.AlteracValleyProfileTypeLabel);
            this.AlteracValley.Controls.Add(this.AlteracValleySwitch);
            this.AlteracValley.Controls.Add(this.AlteracValleyLabel);
            this.AlteracValley.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.AlteracValley.Location = new System.Drawing.Point(13, 52);
            this.AlteracValley.Name = "AlteracValley";
            this.AlteracValley.Size = new System.Drawing.Size(246, 115);
            this.AlteracValley.TabIndex = 57;
            this.AlteracValley.TabStop = false;
            this.AlteracValley.Text = "Alterac Valley";
            // 
            // XMLProfileListAlteracValleyLabel
            // 
            this.XMLProfileListAlteracValleyLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XMLProfileListAlteracValleyLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.XMLProfileListAlteracValleyLabel.Location = new System.Drawing.Point(86, 66);
            this.XMLProfileListAlteracValleyLabel.Name = "XMLProfileListAlteracValleyLabel";
            this.XMLProfileListAlteracValleyLabel.Size = new System.Drawing.Size(92, 19);
            this.XMLProfileListAlteracValleyLabel.TabIndex = 61;
            this.XMLProfileListAlteracValleyLabel.Text = "XML Profile List";
            // 
            // AlteracValleyProfileTypeLabel
            // 
            this.AlteracValleyProfileTypeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.AlteracValleyProfileTypeLabel.Location = new System.Drawing.Point(6, 43);
            this.AlteracValleyProfileTypeLabel.Name = "AlteracValleyProfileTypeLabel";
            this.AlteracValleyProfileTypeLabel.Size = new System.Drawing.Size(75, 22);
            this.AlteracValleyProfileTypeLabel.TabIndex = 58;
            this.AlteracValleyProfileTypeLabel.Text = "Profile Type";
            // 
            // IsleofConquest
            // 
            this.IsleofConquest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.IsleofConquest.Controls.Add(this.XMLProfileListIsleofConquestLabel);
            this.IsleofConquest.Controls.Add(this.IsleofConquestProfileType);
            this.IsleofConquest.Controls.Add(this.XMLProfileListIsleofConquest);
            this.IsleofConquest.Controls.Add(this.IsleofConquestProfileTypeLabel);
            this.IsleofConquest.Controls.Add(this.IsleofConquestLabel);
            this.IsleofConquest.Controls.Add(this.IsleofConquestSwitch);
            this.IsleofConquest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.IsleofConquest.Location = new System.Drawing.Point(541, 172);
            this.IsleofConquest.Name = "IsleofConquest";
            this.IsleofConquest.Size = new System.Drawing.Size(246, 115);
            this.IsleofConquest.TabIndex = 60;
            this.IsleofConquest.TabStop = false;
            this.IsleofConquest.Text = "Ilse of Conquest";
            // 
            // XMLProfileListIsleofConquestLabel
            // 
            this.XMLProfileListIsleofConquestLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XMLProfileListIsleofConquestLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.XMLProfileListIsleofConquestLabel.Location = new System.Drawing.Point(90, 66);
            this.XMLProfileListIsleofConquestLabel.Name = "XMLProfileListIsleofConquestLabel";
            this.XMLProfileListIsleofConquestLabel.Size = new System.Drawing.Size(92, 19);
            this.XMLProfileListIsleofConquestLabel.TabIndex = 63;
            this.XMLProfileListIsleofConquestLabel.Text = "XML Profile List";
            // 
            // IsleofConquestProfileTypeLabel
            // 
            this.IsleofConquestProfileTypeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.IsleofConquestProfileTypeLabel.Location = new System.Drawing.Point(6, 43);
            this.IsleofConquestProfileTypeLabel.Name = "IsleofConquestProfileTypeLabel";
            this.IsleofConquestProfileTypeLabel.Size = new System.Drawing.Size(75, 22);
            this.IsleofConquestProfileTypeLabel.TabIndex = 58;
            this.IsleofConquestProfileTypeLabel.Text = "Profile Type";
            // 
            // TwinPeaks
            // 
            this.TwinPeaks.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.TwinPeaks.Controls.Add(this.XMLProfileListTwinPeaksLabel);
            this.TwinPeaks.Controls.Add(this.XMLProfileListTwinPeaks);
            this.TwinPeaks.Controls.Add(this.TwinPeaksProfileType);
            this.TwinPeaks.Controls.Add(this.TwinPeaksProfileTypeLabel);
            this.TwinPeaks.Controls.Add(this.TwinPeaksSwitch);
            this.TwinPeaks.Controls.Add(this.TwinPeaksLabel);
            this.TwinPeaks.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.TwinPeaks.Location = new System.Drawing.Point(277, 293);
            this.TwinPeaks.Name = "TwinPeaks";
            this.TwinPeaks.Size = new System.Drawing.Size(246, 115);
            this.TwinPeaks.TabIndex = 62;
            this.TwinPeaks.TabStop = false;
            this.TwinPeaks.Text = "Twin Peaks";
            // 
            // XMLProfileListTwinPeaksLabel
            // 
            this.XMLProfileListTwinPeaksLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XMLProfileListTwinPeaksLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.XMLProfileListTwinPeaksLabel.Location = new System.Drawing.Point(88, 66);
            this.XMLProfileListTwinPeaksLabel.Name = "XMLProfileListTwinPeaksLabel";
            this.XMLProfileListTwinPeaksLabel.Size = new System.Drawing.Size(92, 19);
            this.XMLProfileListTwinPeaksLabel.TabIndex = 63;
            this.XMLProfileListTwinPeaksLabel.Text = "XML Profile List";
            // 
            // TwinPeaksProfileTypeLabel
            // 
            this.TwinPeaksProfileTypeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.TwinPeaksProfileTypeLabel.Location = new System.Drawing.Point(6, 43);
            this.TwinPeaksProfileTypeLabel.Name = "TwinPeaksProfileTypeLabel";
            this.TwinPeaksProfileTypeLabel.Size = new System.Drawing.Size(75, 22);
            this.TwinPeaksProfileTypeLabel.TabIndex = 58;
            this.TwinPeaksProfileTypeLabel.Text = "Profile Type";
            // 
            // WarsongGulch
            // 
            this.WarsongGulch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.WarsongGulch.Controls.Add(this.XMLProfileListWarsongGulchLabel);
            this.WarsongGulch.Controls.Add(this.XMLProfileListWarsongGulch);
            this.WarsongGulch.Controls.Add(this.WarsongGulchProfileType);
            this.WarsongGulch.Controls.Add(this.WarsongGulchProfileTypeLabel);
            this.WarsongGulch.Controls.Add(this.WarsongGulchSwitch);
            this.WarsongGulch.Controls.Add(this.WarsongGulchLabel);
            this.WarsongGulch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.WarsongGulch.Location = new System.Drawing.Point(277, 52);
            this.WarsongGulch.Name = "WarsongGulch";
            this.WarsongGulch.Size = new System.Drawing.Size(246, 115);
            this.WarsongGulch.TabIndex = 61;
            this.WarsongGulch.TabStop = false;
            this.WarsongGulch.Text = "Warsong Gulch";
            // 
            // XMLProfileListWarsongGulchLabel
            // 
            this.XMLProfileListWarsongGulchLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XMLProfileListWarsongGulchLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.XMLProfileListWarsongGulchLabel.Location = new System.Drawing.Point(88, 66);
            this.XMLProfileListWarsongGulchLabel.Name = "XMLProfileListWarsongGulchLabel";
            this.XMLProfileListWarsongGulchLabel.Size = new System.Drawing.Size(92, 19);
            this.XMLProfileListWarsongGulchLabel.TabIndex = 63;
            this.XMLProfileListWarsongGulchLabel.Text = "XML Profile List";
            // 
            // WarsongGulchProfileTypeLabel
            // 
            this.WarsongGulchProfileTypeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.WarsongGulchProfileTypeLabel.Location = new System.Drawing.Point(6, 43);
            this.WarsongGulchProfileTypeLabel.Name = "WarsongGulchProfileTypeLabel";
            this.WarsongGulchProfileTypeLabel.Size = new System.Drawing.Size(75, 22);
            this.WarsongGulchProfileTypeLabel.TabIndex = 58;
            this.WarsongGulchProfileTypeLabel.Text = "Profile Type";
            // 
            // TempleofKotmogu
            // 
            this.TempleofKotmogu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.TempleofKotmogu.Controls.Add(this.XMLProfileListTempleofKotmoguLabel);
            this.TempleofKotmogu.Controls.Add(this.TempleofKotmoguProfileType);
            this.TempleofKotmogu.Controls.Add(this.XMLProfileListTempleofKotmogu);
            this.TempleofKotmogu.Controls.Add(this.TempleofKotmoguProfileTypeLabel);
            this.TempleofKotmogu.Controls.Add(this.TempleOfKotmoguLabel);
            this.TempleofKotmogu.Controls.Add(this.TempleOfKotmoguSwitch);
            this.TempleofKotmogu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.TempleofKotmogu.Location = new System.Drawing.Point(541, 293);
            this.TempleofKotmogu.Name = "TempleofKotmogu";
            this.TempleofKotmogu.Size = new System.Drawing.Size(246, 115);
            this.TempleofKotmogu.TabIndex = 66;
            this.TempleofKotmogu.TabStop = false;
            this.TempleofKotmogu.Text = "Temple of Kotmogu";
            // 
            // XMLProfileListTempleofKotmoguLabel
            // 
            this.XMLProfileListTempleofKotmoguLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XMLProfileListTempleofKotmoguLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.XMLProfileListTempleofKotmoguLabel.Location = new System.Drawing.Point(90, 66);
            this.XMLProfileListTempleofKotmoguLabel.Name = "XMLProfileListTempleofKotmoguLabel";
            this.XMLProfileListTempleofKotmoguLabel.Size = new System.Drawing.Size(92, 19);
            this.XMLProfileListTempleofKotmoguLabel.TabIndex = 65;
            this.XMLProfileListTempleofKotmoguLabel.Text = "XML Profile List";
            // 
            // TempleofKotmoguProfileTypeLabel
            // 
            this.TempleofKotmoguProfileTypeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.TempleofKotmoguProfileTypeLabel.Location = new System.Drawing.Point(6, 43);
            this.TempleofKotmoguProfileTypeLabel.Name = "TempleofKotmoguProfileTypeLabel";
            this.TempleofKotmoguProfileTypeLabel.Size = new System.Drawing.Size(75, 22);
            this.TempleofKotmoguProfileTypeLabel.TabIndex = 58;
            this.TempleofKotmoguProfileTypeLabel.Text = "Profile Type";
            // 
            // BattleforGilneas
            // 
            this.BattleforGilneas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.BattleforGilneas.Controls.Add(this.XMLProfileListBattleforGilneasLabel);
            this.BattleforGilneas.Controls.Add(this.XMLProfileListBattleforGilneas);
            this.BattleforGilneas.Controls.Add(this.BattleforGilneasProfileType);
            this.BattleforGilneas.Controls.Add(this.labelX3);
            this.BattleforGilneas.Controls.Add(this.BattleforGilneasLabel);
            this.BattleforGilneas.Controls.Add(this.BattleforGilneasSwitch);
            this.BattleforGilneas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.BattleforGilneas.Location = new System.Drawing.Point(13, 293);
            this.BattleforGilneas.Name = "BattleforGilneas";
            this.BattleforGilneas.Size = new System.Drawing.Size(246, 115);
            this.BattleforGilneas.TabIndex = 64;
            this.BattleforGilneas.TabStop = false;
            this.BattleforGilneas.Text = "Battle for Gilneas";
            // 
            // XMLProfileListBattleforGilneasLabel
            // 
            this.XMLProfileListBattleforGilneasLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XMLProfileListBattleforGilneasLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.XMLProfileListBattleforGilneasLabel.Location = new System.Drawing.Point(86, 66);
            this.XMLProfileListBattleforGilneasLabel.Name = "XMLProfileListBattleforGilneasLabel";
            this.XMLProfileListBattleforGilneasLabel.Size = new System.Drawing.Size(92, 19);
            this.XMLProfileListBattleforGilneasLabel.TabIndex = 63;
            this.XMLProfileListBattleforGilneasLabel.Text = "XML Profile List";
            // 
            // labelX3
            // 
            this.labelX3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.labelX3.Location = new System.Drawing.Point(6, 43);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(75, 22);
            this.labelX3.TabIndex = 58;
            this.labelX3.Text = "Profile Type";
            // 
            // EyeoftheStorm
            // 
            this.EyeoftheStorm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.EyeoftheStorm.Controls.Add(this.XMLProfileListEyeoftheStormLabel);
            this.EyeoftheStorm.Controls.Add(this.XMLProfileListEyeoftheStorm);
            this.EyeoftheStorm.Controls.Add(this.EyeoftheStormProfileType);
            this.EyeoftheStorm.Controls.Add(this.EyeoftheStormProfileTypeLabel);
            this.EyeoftheStorm.Controls.Add(this.EyeoftheStormLabel);
            this.EyeoftheStorm.Controls.Add(this.EyeoftheStormSwitch);
            this.EyeoftheStorm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.EyeoftheStorm.Location = new System.Drawing.Point(13, 172);
            this.EyeoftheStorm.Name = "EyeoftheStorm";
            this.EyeoftheStorm.Size = new System.Drawing.Size(246, 115);
            this.EyeoftheStorm.TabIndex = 65;
            this.EyeoftheStorm.TabStop = false;
            this.EyeoftheStorm.Text = "Eye of the Storm";
            // 
            // XMLProfileListEyeoftheStormLabel
            // 
            this.XMLProfileListEyeoftheStormLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XMLProfileListEyeoftheStormLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.XMLProfileListEyeoftheStormLabel.Location = new System.Drawing.Point(86, 66);
            this.XMLProfileListEyeoftheStormLabel.Name = "XMLProfileListEyeoftheStormLabel";
            this.XMLProfileListEyeoftheStormLabel.Size = new System.Drawing.Size(92, 19);
            this.XMLProfileListEyeoftheStormLabel.TabIndex = 63;
            this.XMLProfileListEyeoftheStormLabel.Text = "XML Profile List";
            // 
            // EyeoftheStormProfileTypeLabel
            // 
            this.EyeoftheStormProfileTypeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.EyeoftheStormProfileTypeLabel.Location = new System.Drawing.Point(6, 43);
            this.EyeoftheStormProfileTypeLabel.Name = "EyeoftheStormProfileTypeLabel";
            this.EyeoftheStormProfileTypeLabel.Size = new System.Drawing.Size(75, 22);
            this.EyeoftheStormProfileTypeLabel.TabIndex = 58;
            this.EyeoftheStormProfileTypeLabel.Text = "Profile Type";
            // 
            // ArathiBasin
            // 
            this.ArathiBasin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.ArathiBasin.Controls.Add(this.XMLProfileListArathiBasinLabel);
            this.ArathiBasin.Controls.Add(this.XMLProfileListArathiBasin);
            this.ArathiBasin.Controls.Add(this.ArathiBasinProfileType);
            this.ArathiBasin.Controls.Add(this.ArathiBasinProfileTypeLabel);
            this.ArathiBasin.Controls.Add(this.ArathiBasinSwitch);
            this.ArathiBasin.Controls.Add(this.ArathiBasinLabel);
            this.ArathiBasin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.ArathiBasin.Location = new System.Drawing.Point(541, 52);
            this.ArathiBasin.Name = "ArathiBasin";
            this.ArathiBasin.Size = new System.Drawing.Size(246, 115);
            this.ArathiBasin.TabIndex = 63;
            this.ArathiBasin.TabStop = false;
            this.ArathiBasin.Text = "Arathi Basin";
            // 
            // XMLProfileListArathiBasinLabel
            // 
            this.XMLProfileListArathiBasinLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XMLProfileListArathiBasinLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.XMLProfileListArathiBasinLabel.Location = new System.Drawing.Point(90, 66);
            this.XMLProfileListArathiBasinLabel.Name = "XMLProfileListArathiBasinLabel";
            this.XMLProfileListArathiBasinLabel.Size = new System.Drawing.Size(92, 19);
            this.XMLProfileListArathiBasinLabel.TabIndex = 63;
            this.XMLProfileListArathiBasinLabel.Text = "XML Profile List";
            // 
            // ArathiBasinProfileTypeLabel
            // 
            this.ArathiBasinProfileTypeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.ArathiBasinProfileTypeLabel.Location = new System.Drawing.Point(6, 43);
            this.ArathiBasinProfileTypeLabel.Name = "ArathiBasinProfileTypeLabel";
            this.ArathiBasinProfileTypeLabel.Size = new System.Drawing.Size(75, 22);
            this.ArathiBasinProfileTypeLabel.TabIndex = 58;
            this.ArathiBasinProfileTypeLabel.Text = "Profile Type";
            // 
            // SilvershardMines
            // 
            this.SilvershardMines.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.SilvershardMines.Controls.Add(this.XMLProfileListSilvershardMinesLabel);
            this.SilvershardMines.Controls.Add(this.XMLProfileListSilvershardMines);
            this.SilvershardMines.Controls.Add(this.SilvershardMinesProfileType);
            this.SilvershardMines.Controls.Add(this.SilvershardMinesProfileTypeLabel);
            this.SilvershardMines.Controls.Add(this.SilvershardMinesLabel);
            this.SilvershardMines.Controls.Add(this.SilvershardMinesSwitch);
            this.SilvershardMines.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.SilvershardMines.Location = new System.Drawing.Point(13, 414);
            this.SilvershardMines.Name = "SilvershardMines";
            this.SilvershardMines.Size = new System.Drawing.Size(246, 115);
            this.SilvershardMines.TabIndex = 68;
            this.SilvershardMines.TabStop = false;
            this.SilvershardMines.Text = "Silvershard Mines";
            // 
            // XMLProfileListSilvershardMinesLabel
            // 
            this.XMLProfileListSilvershardMinesLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XMLProfileListSilvershardMinesLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.XMLProfileListSilvershardMinesLabel.Location = new System.Drawing.Point(86, 66);
            this.XMLProfileListSilvershardMinesLabel.Name = "XMLProfileListSilvershardMinesLabel";
            this.XMLProfileListSilvershardMinesLabel.Size = new System.Drawing.Size(92, 19);
            this.XMLProfileListSilvershardMinesLabel.TabIndex = 63;
            this.XMLProfileListSilvershardMinesLabel.Text = "XML Profile List";
            // 
            // SilvershardMinesProfileTypeLabel
            // 
            this.SilvershardMinesProfileTypeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.SilvershardMinesProfileTypeLabel.Location = new System.Drawing.Point(6, 43);
            this.SilvershardMinesProfileTypeLabel.Name = "SilvershardMinesProfileTypeLabel";
            this.SilvershardMinesProfileTypeLabel.Size = new System.Drawing.Size(75, 22);
            this.SilvershardMinesProfileTypeLabel.TabIndex = 58;
            this.SilvershardMinesProfileTypeLabel.Text = "Profile Type";
            // 
            // StrandoftheAncients
            // 
            this.StrandoftheAncients.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.StrandoftheAncients.Controls.Add(this.XMLProfileListStrandoftheAncientsLabel);
            this.StrandoftheAncients.Controls.Add(this.XMLProfileListStrandoftheAncients);
            this.StrandoftheAncients.Controls.Add(this.StrandoftheAncientsProfileType);
            this.StrandoftheAncients.Controls.Add(this.StrandoftheAncientsProfileTypeLabel);
            this.StrandoftheAncients.Controls.Add(this.StrandoftheAncientsSwitch);
            this.StrandoftheAncients.Controls.Add(this.StrandoftheAncientsLabel);
            this.StrandoftheAncients.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.StrandoftheAncients.Location = new System.Drawing.Point(277, 172);
            this.StrandoftheAncients.Name = "StrandoftheAncients";
            this.StrandoftheAncients.Size = new System.Drawing.Size(246, 115);
            this.StrandoftheAncients.TabIndex = 67;
            this.StrandoftheAncients.TabStop = false;
            this.StrandoftheAncients.Text = "Strand of the Ancients";
            // 
            // XMLProfileListStrandoftheAncientsLabel
            // 
            this.XMLProfileListStrandoftheAncientsLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XMLProfileListStrandoftheAncientsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.XMLProfileListStrandoftheAncientsLabel.Location = new System.Drawing.Point(88, 66);
            this.XMLProfileListStrandoftheAncientsLabel.Name = "XMLProfileListStrandoftheAncientsLabel";
            this.XMLProfileListStrandoftheAncientsLabel.Size = new System.Drawing.Size(92, 19);
            this.XMLProfileListStrandoftheAncientsLabel.TabIndex = 63;
            this.XMLProfileListStrandoftheAncientsLabel.Text = "XML Profile List";
            // 
            // StrandoftheAncientsProfileTypeLabel
            // 
            this.StrandoftheAncientsProfileTypeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.StrandoftheAncientsProfileTypeLabel.Location = new System.Drawing.Point(6, 43);
            this.StrandoftheAncientsProfileTypeLabel.Name = "StrandoftheAncientsProfileTypeLabel";
            this.StrandoftheAncientsProfileTypeLabel.Size = new System.Drawing.Size(75, 22);
            this.StrandoftheAncientsProfileTypeLabel.TabIndex = 58;
            this.StrandoftheAncientsProfileTypeLabel.Text = "Profile Type";
            // 
            // StrandoftheAncientsLabel
            // 
            this.StrandoftheAncientsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.StrandoftheAncientsLabel.Location = new System.Drawing.Point(6, 16);
            this.StrandoftheAncientsLabel.Name = "StrandoftheAncientsLabel";
            this.StrandoftheAncientsLabel.Size = new System.Drawing.Size(161, 22);
            this.StrandoftheAncientsLabel.TabIndex = 29;
            this.StrandoftheAncientsLabel.Text = "Queue for Strand of the Ancients";
            // 
            // RandomBattleground
            // 
            this.RandomBattleground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.RandomBattleground.Controls.Add(this.RandomBattlegroundSwitch);
            this.RandomBattleground.Controls.Add(this.RandomBattlegroundLabel);
            this.RandomBattleground.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.RandomBattleground.Location = new System.Drawing.Point(277, 414);
            this.RandomBattleground.Name = "RandomBattleground";
            this.RandomBattleground.Size = new System.Drawing.Size(246, 77);
            this.RandomBattleground.TabIndex = 68;
            this.RandomBattleground.TabStop = false;
            this.RandomBattleground.Text = "Random Battleground";
            // 
            // RequeueAfterXMinutesGroupBox
            // 
            this.RequeueAfterXMinutesGroupBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.RequeueAfterXMinutesGroupBox.Controls.Add(this.RequeueAfterXMinutesSwitch);
            this.RequeueAfterXMinutesGroupBox.Controls.Add(this.RequeueAfterXMinutesLabel);
            this.RequeueAfterXMinutesGroupBox.Controls.Add(this.RequeueAfterXMinutes);
            this.RequeueAfterXMinutesGroupBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.RequeueAfterXMinutesGroupBox.Location = new System.Drawing.Point(541, 414);
            this.RequeueAfterXMinutesGroupBox.Name = "RequeueAfterXMinutesGroupBox";
            this.RequeueAfterXMinutesGroupBox.Size = new System.Drawing.Size(246, 77);
            this.RequeueAfterXMinutesGroupBox.TabIndex = 69;
            this.RequeueAfterXMinutesGroupBox.TabStop = false;
            this.RequeueAfterXMinutesGroupBox.Text = "Automatic Requeue system";
            // 
            // MainHeader
            // 
            this.MainHeader.BackgroundImage = global::Battlegrounder.Properties.Resources._800x43_controlbar;
            this.MainHeader.Location = new System.Drawing.Point(0, 0);
            this.MainHeader.LogoImage = ((System.Drawing.Image)(resources.GetObject("MainHeader.LogoImage")));
            this.MainHeader.Name = "MainHeader";
            this.MainHeader.Size = new System.Drawing.Size(800, 43);
            this.MainHeader.TabIndex = 72;
            this.MainHeader.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.MainHeader.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.MainHeader.TitleText = "Battlegrounder Settings - The Noob Bot DevVersionRestrict";
            // 
            // createProfileB
            // 
            this.createProfileB.AutoEllipsis = true;
            this.createProfileB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.createProfileB.ForeColor = System.Drawing.Color.Snow;
            this.createProfileB.HooverImage = global::Battlegrounder.Properties.Resources.greenB_150;
            this.createProfileB.Image = global::Battlegrounder.Properties.Resources.blackB_150;
            this.createProfileB.Location = new System.Drawing.Point(277, 501);
            this.createProfileB.Name = "createProfileB";
            this.createProfileB.Size = new System.Drawing.Size(150, 29);
            this.createProfileB.TabIndex = 71;
            this.createProfileB.Text = "XML Profiles Manager";
            this.createProfileB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.createProfileB.Click += new System.EventHandler(this.CreateProfileBClick);
            // 
            // CloseNoSaveButton
            // 
            this.CloseNoSaveButton.AutoEllipsis = true;
            this.CloseNoSaveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.CloseNoSaveButton.ForeColor = System.Drawing.Color.Snow;
            this.CloseNoSaveButton.HooverImage = global::Battlegrounder.Properties.Resources.greenB_150;
            this.CloseNoSaveButton.Image = global::Battlegrounder.Properties.Resources.blackB_150;
            this.CloseNoSaveButton.Location = new System.Drawing.Point(458, 501);
            this.CloseNoSaveButton.Name = "CloseNoSaveButton";
            this.CloseNoSaveButton.Size = new System.Drawing.Size(150, 29);
            this.CloseNoSaveButton.TabIndex = 70;
            this.CloseNoSaveButton.Text = "Close without saving";
            this.CloseNoSaveButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CloseNoSaveButton.Click += new System.EventHandler(this.CloseNoSaveButtonClick);
            // 
            // XMLProfileListSilvershardMines
            // 
            this.XMLProfileListSilvershardMines.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.XMLProfileListSilvershardMines.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            this.XMLProfileListSilvershardMines.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.XMLProfileListSilvershardMines.DisplayMember = "Text";
            this.XMLProfileListSilvershardMines.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.XMLProfileListSilvershardMines.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.XMLProfileListSilvershardMines.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.XMLProfileListSilvershardMines.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.XMLProfileListSilvershardMines.FormattingEnabled = true;
            this.XMLProfileListSilvershardMines.HighlightColor = System.Drawing.Color.Gainsboro;
            this.XMLProfileListSilvershardMines.ItemHeight = 16;
            this.XMLProfileListSilvershardMines.Location = new System.Drawing.Point(6, 88);
            this.XMLProfileListSilvershardMines.Name = "XMLProfileListSilvershardMines";
            this.XMLProfileListSilvershardMines.SelectorBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(106)))), ((int)(((byte)(194)))));
            this.XMLProfileListSilvershardMines.SelectorImage = ((System.Drawing.Image)(resources.GetObject("XMLProfileListSilvershardMines.SelectorImage")));
            this.XMLProfileListSilvershardMines.Size = new System.Drawing.Size(231, 22);
            this.XMLProfileListSilvershardMines.TabIndex = 62;
            // 
            // SilvershardMinesProfileType
            // 
            this.SilvershardMinesProfileType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.SilvershardMinesProfileType.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            this.SilvershardMinesProfileType.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.SilvershardMinesProfileType.DisplayMember = "Text";
            this.SilvershardMinesProfileType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.SilvershardMinesProfileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SilvershardMinesProfileType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SilvershardMinesProfileType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.SilvershardMinesProfileType.FormattingEnabled = true;
            this.SilvershardMinesProfileType.HighlightColor = System.Drawing.Color.Gainsboro;
            this.SilvershardMinesProfileType.ItemHeight = 16;
            this.SilvershardMinesProfileType.Location = new System.Drawing.Point(86, 43);
            this.SilvershardMinesProfileType.Name = "SilvershardMinesProfileType";
            this.SilvershardMinesProfileType.SelectorBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(106)))), ((int)(((byte)(194)))));
            this.SilvershardMinesProfileType.SelectorImage = ((System.Drawing.Image)(resources.GetObject("SilvershardMinesProfileType.SelectorImage")));
            this.SilvershardMinesProfileType.Size = new System.Drawing.Size(150, 22);
            this.SilvershardMinesProfileType.TabIndex = 59;
            // 
            // TempleofKotmoguProfileType
            // 
            this.TempleofKotmoguProfileType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.TempleofKotmoguProfileType.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            this.TempleofKotmoguProfileType.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.TempleofKotmoguProfileType.DisplayMember = "Text";
            this.TempleofKotmoguProfileType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.TempleofKotmoguProfileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TempleofKotmoguProfileType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TempleofKotmoguProfileType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.TempleofKotmoguProfileType.FormattingEnabled = true;
            this.TempleofKotmoguProfileType.HighlightColor = System.Drawing.Color.Gainsboro;
            this.TempleofKotmoguProfileType.ItemHeight = 16;
            this.TempleofKotmoguProfileType.Location = new System.Drawing.Point(86, 43);
            this.TempleofKotmoguProfileType.Name = "TempleofKotmoguProfileType";
            this.TempleofKotmoguProfileType.SelectorBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(106)))), ((int)(((byte)(194)))));
            this.TempleofKotmoguProfileType.SelectorImage = ((System.Drawing.Image)(resources.GetObject("TempleofKotmoguProfileType.SelectorImage")));
            this.TempleofKotmoguProfileType.Size = new System.Drawing.Size(150, 22);
            this.TempleofKotmoguProfileType.TabIndex = 59;
            // 
            // XMLProfileListTempleofKotmogu
            // 
            this.XMLProfileListTempleofKotmogu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.XMLProfileListTempleofKotmogu.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            this.XMLProfileListTempleofKotmogu.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.XMLProfileListTempleofKotmogu.DisplayMember = "Text";
            this.XMLProfileListTempleofKotmogu.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.XMLProfileListTempleofKotmogu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.XMLProfileListTempleofKotmogu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.XMLProfileListTempleofKotmogu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.XMLProfileListTempleofKotmogu.FormattingEnabled = true;
            this.XMLProfileListTempleofKotmogu.HighlightColor = System.Drawing.Color.Gainsboro;
            this.XMLProfileListTempleofKotmogu.ItemHeight = 16;
            this.XMLProfileListTempleofKotmogu.Location = new System.Drawing.Point(10, 88);
            this.XMLProfileListTempleofKotmogu.Name = "XMLProfileListTempleofKotmogu";
            this.XMLProfileListTempleofKotmogu.SelectorBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(106)))), ((int)(((byte)(194)))));
            this.XMLProfileListTempleofKotmogu.SelectorImage = ((System.Drawing.Image)(resources.GetObject("XMLProfileListTempleofKotmogu.SelectorImage")));
            this.XMLProfileListTempleofKotmogu.Size = new System.Drawing.Size(231, 22);
            this.XMLProfileListTempleofKotmogu.TabIndex = 64;
            // 
            // XMLProfileListStrandoftheAncients
            // 
            this.XMLProfileListStrandoftheAncients.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.XMLProfileListStrandoftheAncients.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            this.XMLProfileListStrandoftheAncients.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.XMLProfileListStrandoftheAncients.DisplayMember = "Text";
            this.XMLProfileListStrandoftheAncients.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.XMLProfileListStrandoftheAncients.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.XMLProfileListStrandoftheAncients.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.XMLProfileListStrandoftheAncients.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.XMLProfileListStrandoftheAncients.FormattingEnabled = true;
            this.XMLProfileListStrandoftheAncients.HighlightColor = System.Drawing.Color.Gainsboro;
            this.XMLProfileListStrandoftheAncients.ItemHeight = 16;
            this.XMLProfileListStrandoftheAncients.Location = new System.Drawing.Point(8, 88);
            this.XMLProfileListStrandoftheAncients.Name = "XMLProfileListStrandoftheAncients";
            this.XMLProfileListStrandoftheAncients.SelectorBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(106)))), ((int)(((byte)(194)))));
            this.XMLProfileListStrandoftheAncients.SelectorImage = ((System.Drawing.Image)(resources.GetObject("XMLProfileListStrandoftheAncients.SelectorImage")));
            this.XMLProfileListStrandoftheAncients.Size = new System.Drawing.Size(231, 22);
            this.XMLProfileListStrandoftheAncients.TabIndex = 62;
            // 
            // StrandoftheAncientsProfileType
            // 
            this.StrandoftheAncientsProfileType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.StrandoftheAncientsProfileType.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            this.StrandoftheAncientsProfileType.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.StrandoftheAncientsProfileType.DisplayMember = "Text";
            this.StrandoftheAncientsProfileType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.StrandoftheAncientsProfileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.StrandoftheAncientsProfileType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StrandoftheAncientsProfileType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.StrandoftheAncientsProfileType.FormattingEnabled = true;
            this.StrandoftheAncientsProfileType.HighlightColor = System.Drawing.Color.Gainsboro;
            this.StrandoftheAncientsProfileType.ItemHeight = 16;
            this.StrandoftheAncientsProfileType.Location = new System.Drawing.Point(86, 43);
            this.StrandoftheAncientsProfileType.Name = "StrandoftheAncientsProfileType";
            this.StrandoftheAncientsProfileType.SelectorBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(106)))), ((int)(((byte)(194)))));
            this.StrandoftheAncientsProfileType.SelectorImage = ((System.Drawing.Image)(resources.GetObject("StrandoftheAncientsProfileType.SelectorImage")));
            this.StrandoftheAncientsProfileType.Size = new System.Drawing.Size(150, 22);
            this.StrandoftheAncientsProfileType.TabIndex = 59;
            // 
            // XMLProfileListTwinPeaks
            // 
            this.XMLProfileListTwinPeaks.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.XMLProfileListTwinPeaks.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            this.XMLProfileListTwinPeaks.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.XMLProfileListTwinPeaks.DisplayMember = "Text";
            this.XMLProfileListTwinPeaks.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.XMLProfileListTwinPeaks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.XMLProfileListTwinPeaks.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.XMLProfileListTwinPeaks.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.XMLProfileListTwinPeaks.FormattingEnabled = true;
            this.XMLProfileListTwinPeaks.HighlightColor = System.Drawing.Color.Gainsboro;
            this.XMLProfileListTwinPeaks.ItemHeight = 16;
            this.XMLProfileListTwinPeaks.Location = new System.Drawing.Point(8, 88);
            this.XMLProfileListTwinPeaks.Name = "XMLProfileListTwinPeaks";
            this.XMLProfileListTwinPeaks.SelectorBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(106)))), ((int)(((byte)(194)))));
            this.XMLProfileListTwinPeaks.SelectorImage = ((System.Drawing.Image)(resources.GetObject("XMLProfileListTwinPeaks.SelectorImage")));
            this.XMLProfileListTwinPeaks.Size = new System.Drawing.Size(231, 22);
            this.XMLProfileListTwinPeaks.TabIndex = 62;
            // 
            // TwinPeaksProfileType
            // 
            this.TwinPeaksProfileType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.TwinPeaksProfileType.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            this.TwinPeaksProfileType.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.TwinPeaksProfileType.DisplayMember = "Text";
            this.TwinPeaksProfileType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.TwinPeaksProfileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TwinPeaksProfileType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TwinPeaksProfileType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.TwinPeaksProfileType.FormattingEnabled = true;
            this.TwinPeaksProfileType.HighlightColor = System.Drawing.Color.Gainsboro;
            this.TwinPeaksProfileType.ItemHeight = 16;
            this.TwinPeaksProfileType.Location = new System.Drawing.Point(86, 43);
            this.TwinPeaksProfileType.Name = "TwinPeaksProfileType";
            this.TwinPeaksProfileType.SelectorBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(106)))), ((int)(((byte)(194)))));
            this.TwinPeaksProfileType.SelectorImage = ((System.Drawing.Image)(resources.GetObject("TwinPeaksProfileType.SelectorImage")));
            this.TwinPeaksProfileType.Size = new System.Drawing.Size(150, 22);
            this.TwinPeaksProfileType.TabIndex = 59;
            // 
            // XMLProfileListBattleforGilneas
            // 
            this.XMLProfileListBattleforGilneas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.XMLProfileListBattleforGilneas.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            this.XMLProfileListBattleforGilneas.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.XMLProfileListBattleforGilneas.DisplayMember = "Text";
            this.XMLProfileListBattleforGilneas.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.XMLProfileListBattleforGilneas.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.XMLProfileListBattleforGilneas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.XMLProfileListBattleforGilneas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.XMLProfileListBattleforGilneas.FormattingEnabled = true;
            this.XMLProfileListBattleforGilneas.HighlightColor = System.Drawing.Color.Gainsboro;
            this.XMLProfileListBattleforGilneas.ItemHeight = 16;
            this.XMLProfileListBattleforGilneas.Location = new System.Drawing.Point(6, 88);
            this.XMLProfileListBattleforGilneas.Name = "XMLProfileListBattleforGilneas";
            this.XMLProfileListBattleforGilneas.SelectorBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(106)))), ((int)(((byte)(194)))));
            this.XMLProfileListBattleforGilneas.SelectorImage = ((System.Drawing.Image)(resources.GetObject("XMLProfileListBattleforGilneas.SelectorImage")));
            this.XMLProfileListBattleforGilneas.Size = new System.Drawing.Size(231, 22);
            this.XMLProfileListBattleforGilneas.TabIndex = 62;
            // 
            // BattleforGilneasProfileType
            // 
            this.BattleforGilneasProfileType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.BattleforGilneasProfileType.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            this.BattleforGilneasProfileType.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.BattleforGilneasProfileType.DisplayMember = "Text";
            this.BattleforGilneasProfileType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.BattleforGilneasProfileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BattleforGilneasProfileType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BattleforGilneasProfileType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.BattleforGilneasProfileType.FormattingEnabled = true;
            this.BattleforGilneasProfileType.HighlightColor = System.Drawing.Color.Gainsboro;
            this.BattleforGilneasProfileType.ItemHeight = 16;
            this.BattleforGilneasProfileType.Location = new System.Drawing.Point(86, 43);
            this.BattleforGilneasProfileType.Name = "BattleforGilneasProfileType";
            this.BattleforGilneasProfileType.SelectorBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(106)))), ((int)(((byte)(194)))));
            this.BattleforGilneasProfileType.SelectorImage = ((System.Drawing.Image)(resources.GetObject("BattleforGilneasProfileType.SelectorImage")));
            this.BattleforGilneasProfileType.Size = new System.Drawing.Size(150, 22);
            this.BattleforGilneasProfileType.TabIndex = 59;
            // 
            // IsleofConquestProfileType
            // 
            this.IsleofConquestProfileType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.IsleofConquestProfileType.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            this.IsleofConquestProfileType.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.IsleofConquestProfileType.DisplayMember = "Text";
            this.IsleofConquestProfileType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.IsleofConquestProfileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.IsleofConquestProfileType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.IsleofConquestProfileType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.IsleofConquestProfileType.FormattingEnabled = true;
            this.IsleofConquestProfileType.HighlightColor = System.Drawing.Color.Gainsboro;
            this.IsleofConquestProfileType.ItemHeight = 16;
            this.IsleofConquestProfileType.Location = new System.Drawing.Point(86, 43);
            this.IsleofConquestProfileType.Name = "IsleofConquestProfileType";
            this.IsleofConquestProfileType.SelectorBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(106)))), ((int)(((byte)(194)))));
            this.IsleofConquestProfileType.SelectorImage = ((System.Drawing.Image)(resources.GetObject("IsleofConquestProfileType.SelectorImage")));
            this.IsleofConquestProfileType.Size = new System.Drawing.Size(150, 22);
            this.IsleofConquestProfileType.TabIndex = 59;
            // 
            // XMLProfileListIsleofConquest
            // 
            this.XMLProfileListIsleofConquest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.XMLProfileListIsleofConquest.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            this.XMLProfileListIsleofConquest.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.XMLProfileListIsleofConquest.DisplayMember = "Text";
            this.XMLProfileListIsleofConquest.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.XMLProfileListIsleofConquest.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.XMLProfileListIsleofConquest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.XMLProfileListIsleofConquest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.XMLProfileListIsleofConquest.FormattingEnabled = true;
            this.XMLProfileListIsleofConquest.HighlightColor = System.Drawing.Color.Gainsboro;
            this.XMLProfileListIsleofConquest.ItemHeight = 16;
            this.XMLProfileListIsleofConquest.Location = new System.Drawing.Point(10, 88);
            this.XMLProfileListIsleofConquest.Name = "XMLProfileListIsleofConquest";
            this.XMLProfileListIsleofConquest.SelectorBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(106)))), ((int)(((byte)(194)))));
            this.XMLProfileListIsleofConquest.SelectorImage = ((System.Drawing.Image)(resources.GetObject("XMLProfileListIsleofConquest.SelectorImage")));
            this.XMLProfileListIsleofConquest.Size = new System.Drawing.Size(231, 22);
            this.XMLProfileListIsleofConquest.TabIndex = 62;
            // 
            // XMLProfileListEyeoftheStorm
            // 
            this.XMLProfileListEyeoftheStorm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.XMLProfileListEyeoftheStorm.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            this.XMLProfileListEyeoftheStorm.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.XMLProfileListEyeoftheStorm.DisplayMember = "Text";
            this.XMLProfileListEyeoftheStorm.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.XMLProfileListEyeoftheStorm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.XMLProfileListEyeoftheStorm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.XMLProfileListEyeoftheStorm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.XMLProfileListEyeoftheStorm.FormattingEnabled = true;
            this.XMLProfileListEyeoftheStorm.HighlightColor = System.Drawing.Color.Gainsboro;
            this.XMLProfileListEyeoftheStorm.ItemHeight = 16;
            this.XMLProfileListEyeoftheStorm.Location = new System.Drawing.Point(6, 88);
            this.XMLProfileListEyeoftheStorm.Name = "XMLProfileListEyeoftheStorm";
            this.XMLProfileListEyeoftheStorm.SelectorBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(106)))), ((int)(((byte)(194)))));
            this.XMLProfileListEyeoftheStorm.SelectorImage = ((System.Drawing.Image)(resources.GetObject("XMLProfileListEyeoftheStorm.SelectorImage")));
            this.XMLProfileListEyeoftheStorm.Size = new System.Drawing.Size(231, 22);
            this.XMLProfileListEyeoftheStorm.TabIndex = 62;
            // 
            // EyeoftheStormProfileType
            // 
            this.EyeoftheStormProfileType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.EyeoftheStormProfileType.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            this.EyeoftheStormProfileType.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.EyeoftheStormProfileType.DisplayMember = "Text";
            this.EyeoftheStormProfileType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.EyeoftheStormProfileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.EyeoftheStormProfileType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EyeoftheStormProfileType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.EyeoftheStormProfileType.FormattingEnabled = true;
            this.EyeoftheStormProfileType.HighlightColor = System.Drawing.Color.Gainsboro;
            this.EyeoftheStormProfileType.ItemHeight = 16;
            this.EyeoftheStormProfileType.Location = new System.Drawing.Point(86, 43);
            this.EyeoftheStormProfileType.Name = "EyeoftheStormProfileType";
            this.EyeoftheStormProfileType.SelectorBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(106)))), ((int)(((byte)(194)))));
            this.EyeoftheStormProfileType.SelectorImage = ((System.Drawing.Image)(resources.GetObject("EyeoftheStormProfileType.SelectorImage")));
            this.EyeoftheStormProfileType.Size = new System.Drawing.Size(150, 22);
            this.EyeoftheStormProfileType.TabIndex = 59;
            // 
            // XMLProfileListWarsongGulch
            // 
            this.XMLProfileListWarsongGulch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.XMLProfileListWarsongGulch.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            this.XMLProfileListWarsongGulch.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.XMLProfileListWarsongGulch.DisplayMember = "Text";
            this.XMLProfileListWarsongGulch.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.XMLProfileListWarsongGulch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.XMLProfileListWarsongGulch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.XMLProfileListWarsongGulch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.XMLProfileListWarsongGulch.FormattingEnabled = true;
            this.XMLProfileListWarsongGulch.HighlightColor = System.Drawing.Color.Gainsboro;
            this.XMLProfileListWarsongGulch.ItemHeight = 16;
            this.XMLProfileListWarsongGulch.Location = new System.Drawing.Point(8, 88);
            this.XMLProfileListWarsongGulch.Name = "XMLProfileListWarsongGulch";
            this.XMLProfileListWarsongGulch.SelectorBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(106)))), ((int)(((byte)(194)))));
            this.XMLProfileListWarsongGulch.SelectorImage = ((System.Drawing.Image)(resources.GetObject("XMLProfileListWarsongGulch.SelectorImage")));
            this.XMLProfileListWarsongGulch.Size = new System.Drawing.Size(231, 22);
            this.XMLProfileListWarsongGulch.TabIndex = 62;
            // 
            // WarsongGulchProfileType
            // 
            this.WarsongGulchProfileType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.WarsongGulchProfileType.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            this.WarsongGulchProfileType.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.WarsongGulchProfileType.DisplayMember = "Text";
            this.WarsongGulchProfileType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.WarsongGulchProfileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.WarsongGulchProfileType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.WarsongGulchProfileType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.WarsongGulchProfileType.FormattingEnabled = true;
            this.WarsongGulchProfileType.HighlightColor = System.Drawing.Color.Gainsboro;
            this.WarsongGulchProfileType.ItemHeight = 16;
            this.WarsongGulchProfileType.Location = new System.Drawing.Point(86, 43);
            this.WarsongGulchProfileType.Name = "WarsongGulchProfileType";
            this.WarsongGulchProfileType.SelectorBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(106)))), ((int)(((byte)(194)))));
            this.WarsongGulchProfileType.SelectorImage = ((System.Drawing.Image)(resources.GetObject("WarsongGulchProfileType.SelectorImage")));
            this.WarsongGulchProfileType.Size = new System.Drawing.Size(150, 22);
            this.WarsongGulchProfileType.TabIndex = 59;
            // 
            // XMLProfileListArathiBasin
            // 
            this.XMLProfileListArathiBasin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.XMLProfileListArathiBasin.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            this.XMLProfileListArathiBasin.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.XMLProfileListArathiBasin.DisplayMember = "Text";
            this.XMLProfileListArathiBasin.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.XMLProfileListArathiBasin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.XMLProfileListArathiBasin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.XMLProfileListArathiBasin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.XMLProfileListArathiBasin.FormattingEnabled = true;
            this.XMLProfileListArathiBasin.HighlightColor = System.Drawing.Color.Gainsboro;
            this.XMLProfileListArathiBasin.ItemHeight = 16;
            this.XMLProfileListArathiBasin.Location = new System.Drawing.Point(10, 88);
            this.XMLProfileListArathiBasin.Name = "XMLProfileListArathiBasin";
            this.XMLProfileListArathiBasin.SelectorBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(106)))), ((int)(((byte)(194)))));
            this.XMLProfileListArathiBasin.SelectorImage = ((System.Drawing.Image)(resources.GetObject("XMLProfileListArathiBasin.SelectorImage")));
            this.XMLProfileListArathiBasin.Size = new System.Drawing.Size(231, 22);
            this.XMLProfileListArathiBasin.TabIndex = 62;
            // 
            // ArathiBasinProfileType
            // 
            this.ArathiBasinProfileType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.ArathiBasinProfileType.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            this.ArathiBasinProfileType.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.ArathiBasinProfileType.DisplayMember = "Text";
            this.ArathiBasinProfileType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ArathiBasinProfileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ArathiBasinProfileType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ArathiBasinProfileType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.ArathiBasinProfileType.FormattingEnabled = true;
            this.ArathiBasinProfileType.HighlightColor = System.Drawing.Color.Gainsboro;
            this.ArathiBasinProfileType.ItemHeight = 16;
            this.ArathiBasinProfileType.Location = new System.Drawing.Point(86, 43);
            this.ArathiBasinProfileType.Name = "ArathiBasinProfileType";
            this.ArathiBasinProfileType.SelectorBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(106)))), ((int)(((byte)(194)))));
            this.ArathiBasinProfileType.SelectorImage = ((System.Drawing.Image)(resources.GetObject("ArathiBasinProfileType.SelectorImage")));
            this.ArathiBasinProfileType.Size = new System.Drawing.Size(150, 22);
            this.ArathiBasinProfileType.TabIndex = 59;
            // 
            // XMLProfileListAlteracValley
            // 
            this.XMLProfileListAlteracValley.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.XMLProfileListAlteracValley.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            this.XMLProfileListAlteracValley.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.XMLProfileListAlteracValley.DisplayMember = "Text";
            this.XMLProfileListAlteracValley.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.XMLProfileListAlteracValley.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.XMLProfileListAlteracValley.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.XMLProfileListAlteracValley.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.XMLProfileListAlteracValley.FormattingEnabled = true;
            this.XMLProfileListAlteracValley.HighlightColor = System.Drawing.Color.Gainsboro;
            this.XMLProfileListAlteracValley.ItemHeight = 16;
            this.XMLProfileListAlteracValley.Location = new System.Drawing.Point(6, 88);
            this.XMLProfileListAlteracValley.Name = "XMLProfileListAlteracValley";
            this.XMLProfileListAlteracValley.SelectorBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(106)))), ((int)(((byte)(194)))));
            this.XMLProfileListAlteracValley.SelectorImage = ((System.Drawing.Image)(resources.GetObject("XMLProfileListAlteracValley.SelectorImage")));
            this.XMLProfileListAlteracValley.Size = new System.Drawing.Size(231, 22);
            this.XMLProfileListAlteracValley.TabIndex = 60;
            // 
            // AlteracValleyProfileType
            // 
            this.AlteracValleyProfileType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.AlteracValleyProfileType.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            this.AlteracValleyProfileType.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.AlteracValleyProfileType.DisplayMember = "Text";
            this.AlteracValleyProfileType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.AlteracValleyProfileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AlteracValleyProfileType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AlteracValleyProfileType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.AlteracValleyProfileType.FormattingEnabled = true;
            this.AlteracValleyProfileType.HighlightColor = System.Drawing.Color.Gainsboro;
            this.AlteracValleyProfileType.ItemHeight = 16;
            this.AlteracValleyProfileType.Location = new System.Drawing.Point(88, 43);
            this.AlteracValleyProfileType.Name = "AlteracValleyProfileType";
            this.AlteracValleyProfileType.SelectorBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(106)))), ((int)(((byte)(194)))));
            this.AlteracValleyProfileType.SelectorImage = ((System.Drawing.Image)(resources.GetObject("AlteracValleyProfileType.SelectorImage")));
            this.AlteracValleyProfileType.Size = new System.Drawing.Size(150, 22);
            this.AlteracValleyProfileType.TabIndex = 59;
            // 
            // SaveButton
            // 
            this.SaveButton.AutoEllipsis = true;
            this.SaveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.SaveButton.ForeColor = System.Drawing.Color.Snow;
            this.SaveButton.HooverImage = global::Battlegrounder.Properties.Resources.greenB_150;
            this.SaveButton.Image = global::Battlegrounder.Properties.Resources.blueB_150;
            this.SaveButton.Location = new System.Drawing.Point(638, 501);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(150, 29);
            this.SaveButton.TabIndex = 20;
            this.SaveButton.Text = "Save and Close";
            this.SaveButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.SaveButton.Click += new System.EventHandler(this.SaveButtonClick);
            // 
            // SettingsBattlegrounderForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(800, 542);
            this.Controls.Add(this.MainHeader);
            this.Controls.Add(this.createProfileB);
            this.Controls.Add(this.CloseNoSaveButton);
            this.Controls.Add(this.RequeueAfterXMinutesGroupBox);
            this.Controls.Add(this.RandomBattleground);
            this.Controls.Add(this.SilvershardMines);
            this.Controls.Add(this.TempleofKotmogu);
            this.Controls.Add(this.StrandoftheAncients);
            this.Controls.Add(this.TwinPeaks);
            this.Controls.Add(this.BattleforGilneas);
            this.Controls.Add(this.IsleofConquest);
            this.Controls.Add(this.EyeoftheStorm);
            this.Controls.Add(this.WarsongGulch);
            this.Controls.Add(this.ArathiBasin);
            this.Controls.Add(this.AlteracValley);
            this.Controls.Add(this.SaveButton);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SettingsBattlegrounderForm";
            
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Battlegrounder Settings";
            ((System.ComponentModel.ISupportInitialize)(this.RequeueAfterXMinutes)).EndInit();
            this.AlteracValley.ResumeLayout(false);
            this.IsleofConquest.ResumeLayout(false);
            this.TwinPeaks.ResumeLayout(false);
            this.WarsongGulch.ResumeLayout(false);
            this.TempleofKotmogu.ResumeLayout(false);
            this.BattleforGilneas.ResumeLayout(false);
            this.EyeoftheStorm.ResumeLayout(false);
            this.ArathiBasin.ResumeLayout(false);
            this.SilvershardMines.ResumeLayout(false);
            this.StrandoftheAncients.ResumeLayout(false);
            this.RandomBattleground.ResumeLayout(false);
            this.RequeueAfterXMinutesGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TnbButton SaveButton;
        private Label AlteracValleyLabel;
        private Label WarsongGulchLabel;
        private Label ArathiBasinLabel;
        private Label EyeoftheStormLabel;
        private Label IsleofConquestLabel;
        private Label TwinPeaksLabel;
        private Label BattleforGilneasLabel;
        private Label SilvershardMinesLabel;
        private Label TempleOfKotmoguLabel;
        private Label RandomBattlegroundLabel;
        private TnbSwitchButton AlteracValleySwitch;
        private TnbSwitchButton WarsongGulchSwitch;
        private TnbSwitchButton ArathiBasinSwitch;
        private TnbSwitchButton EyeoftheStormSwitch;
        private TnbSwitchButton StrandoftheAncientsSwitch;
        private TnbSwitchButton IsleofConquestSwitch;
        private TnbSwitchButton TwinPeaksSwitch;
        private TnbSwitchButton BattleforGilneasSwitch;
        private TnbSwitchButton SilvershardMinesSwitch;
        private TnbSwitchButton TempleOfKotmoguSwitch;        
        private TnbSwitchButton RandomBattlegroundSwitch;
        private Label RequeueAfterXMinutesLabel;
        private TnbSwitchButton RequeueAfterXMinutesSwitch;
        private NumericUpDown RequeueAfterXMinutes;
        private System.Windows.Forms.GroupBox AlteracValley;
        private TnbComboBox AlteracValleyProfileType;
        private Label AlteracValleyProfileTypeLabel;
        private Label IsleofConquestProfileTypeLabel;
        private System.Windows.Forms.GroupBox IsleofConquest;
        private TnbComboBox IsleofConquestProfileType;
        private System.Windows.Forms.GroupBox TwinPeaks;
        private TnbComboBox TwinPeaksProfileType;
        private Label TwinPeaksProfileTypeLabel;
        private System.Windows.Forms.GroupBox WarsongGulch;
        private TnbComboBox WarsongGulchProfileType;
        private Label WarsongGulchProfileTypeLabel;
        private System.Windows.Forms.GroupBox TempleofKotmogu;
        private TnbComboBox TempleofKotmoguProfileType;
        private Label TempleofKotmoguProfileTypeLabel;
        private System.Windows.Forms.GroupBox BattleforGilneas;
        private TnbComboBox BattleforGilneasProfileType;
        private Label labelX3;
        private System.Windows.Forms.GroupBox EyeoftheStorm;
        private TnbComboBox EyeoftheStormProfileType;
        private Label EyeoftheStormProfileTypeLabel;
        private System.Windows.Forms.GroupBox ArathiBasin;
        private TnbComboBox ArathiBasinProfileType;
        private Label ArathiBasinProfileTypeLabel;
        private System.Windows.Forms.GroupBox SilvershardMines;
        private TnbComboBox SilvershardMinesProfileType;
        private Label SilvershardMinesProfileTypeLabel;
        private System.Windows.Forms.GroupBox StrandoftheAncients;
        private TnbComboBox StrandoftheAncientsProfileType;
        private Label StrandoftheAncientsProfileTypeLabel;
        private Label StrandoftheAncientsLabel;
        private System.Windows.Forms.GroupBox RandomBattleground;
        private System.Windows.Forms.GroupBox RequeueAfterXMinutesGroupBox;
        private TnbButton CloseNoSaveButton;
        private TnbButton createProfileB;
        private TnbComboBox XMLProfileListAlteracValley;
        private Label XMLProfileListAlteracValleyLabel;
        private Label XMLProfileListWarsongGulchLabel;
        private Label XMLProfileListIsleofConquestLabel;
        private TnbComboBox XMLProfileListIsleofConquest;
        private Label XMLProfileListTwinPeaksLabel;
        private TnbComboBox XMLProfileListTwinPeaks;
        private TnbComboBox XMLProfileListWarsongGulch;
        private Label XMLProfileListTempleofKotmoguLabel;
        private TnbComboBox XMLProfileListTempleofKotmogu;
        private Label XMLProfileListBattleforGilneasLabel;
        private TnbComboBox XMLProfileListBattleforGilneas;
        private Label XMLProfileListEyeoftheStormLabel;
        private TnbComboBox XMLProfileListEyeoftheStorm;
        private Label XMLProfileListArathiBasinLabel;
        private TnbComboBox XMLProfileListArathiBasin;
        private Label XMLProfileListSilvershardMinesLabel;
        private TnbComboBox XMLProfileListSilvershardMines;
        private Label XMLProfileListStrandoftheAncientsLabel;
        private TnbComboBox XMLProfileListStrandoftheAncients;
        private nManager.Helpful.Forms.UserControls.TnbControlMenu MainHeader;
    }
}