namespace Battleground_Bot.Profile
{
    partial class ProfileManager
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
            this.ListOfPoint = new DevExpress.XtraEditors.ListBoxControl();
            this.label1 = new System.Windows.Forms.Label();
            this.ListOfBlackList = new DevExpress.XtraEditors.ListBoxControl();
            this.label2 = new System.Windows.Forms.Label();
            this.BAddToBlackList = new DevExpress.XtraEditors.SimpleButton();
            this.BRecordPoint = new DevExpress.XtraEditors.SimpleButton();
            this.BDelBlackList = new DevExpress.XtraEditors.SimpleButton();
            this.BDelListPoint = new DevExpress.XtraEditors.SimpleButton();
            this.LFirstPointInfo = new System.Windows.Forms.Label();
            this.TUpdateFirstPointInfo = new System.Windows.Forms.Timer();
            this.BSave = new DevExpress.XtraEditors.SimpleButton();
            this.BLoadProfil = new DevExpress.XtraEditors.SimpleButton();
            this.hotSpotCB = new DevExpress.XtraEditors.CheckEdit();
            this.PlayerFactionCB = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.BDtargetFaction = new DevExpress.XtraEditors.SimpleButton();
            this.label4 = new System.Windows.Forms.Label();
            this.targetFactionList = new DevExpress.XtraEditors.ListBoxControl();
            this.ABtargetFaction = new DevExpress.XtraEditors.SimpleButton();
            this.RadarB = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.ListOfPoint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ListOfBlackList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hotSpotCB.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.targetFactionList)).BeginInit();
            this.SuspendLayout();
            // 
            // ListOfPoint
            // 
            this.ListOfPoint.AllowHtmlTextInToolTip = DevExpress.Utils.DefaultBoolean.Default;
            this.ListOfPoint.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.ListOfPoint.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.ListOfPoint.Appearance.TextOptions.HotkeyPrefix = DevExpress.Utils.HKeyPrefix.Default;
            this.ListOfPoint.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Default;
            this.ListOfPoint.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Default;
            this.ListOfPoint.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Default;
            this.ListOfPoint.HighlightedItemStyle = DevExpress.XtraEditors.HighlightStyle.Default;
            this.ListOfPoint.HotTrackSelectMode = DevExpress.XtraEditors.HotTrackSelectMode.SelectItemOnHotTrack;
            this.ListOfPoint.Location = new System.Drawing.Point(12, 21);
            this.ListOfPoint.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            this.ListOfPoint.Name = "ListOfPoint";
            this.ListOfPoint.SelectionMode = System.Windows.Forms.SelectionMode.One;
            this.ListOfPoint.Size = new System.Drawing.Size(262, 134);
            this.ListOfPoint.SortOrder = System.Windows.Forms.SortOrder.None;
            this.ListOfPoint.TabIndex = 0;
            this.ListOfPoint.ToolTipIconType = DevExpress.Utils.ToolTipIconType.None;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "List of Location:";
            // 
            // ListOfBlackList
            // 
            this.ListOfBlackList.AllowHtmlTextInToolTip = DevExpress.Utils.DefaultBoolean.Default;
            this.ListOfBlackList.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.ListOfBlackList.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.ListOfBlackList.Appearance.TextOptions.HotkeyPrefix = DevExpress.Utils.HKeyPrefix.Default;
            this.ListOfBlackList.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Default;
            this.ListOfBlackList.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Default;
            this.ListOfBlackList.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Default;
            this.ListOfBlackList.HighlightedItemStyle = DevExpress.XtraEditors.HighlightStyle.Default;
            this.ListOfBlackList.HotTrackSelectMode = DevExpress.XtraEditors.HotTrackSelectMode.SelectItemOnHotTrack;
            this.ListOfBlackList.Location = new System.Drawing.Point(12, 178);
            this.ListOfBlackList.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            this.ListOfBlackList.Name = "ListOfBlackList";
            this.ListOfBlackList.SelectionMode = System.Windows.Forms.SelectionMode.One;
            this.ListOfBlackList.Size = new System.Drawing.Size(262, 56);
            this.ListOfBlackList.SortOrder = System.Windows.Forms.SortOrder.None;
            this.ListOfBlackList.TabIndex = 2;
            this.ListOfBlackList.ToolTipIconType = DevExpress.Utils.ToolTipIconType.None;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 160);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Black List target Guid:";
            // 
            // BAddToBlackList
            // 
            this.BAddToBlackList.AllowHtmlTextInToolTip = DevExpress.Utils.DefaultBoolean.Default;
            this.BAddToBlackList.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.BAddToBlackList.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.BAddToBlackList.Appearance.TextOptions.HotkeyPrefix = DevExpress.Utils.HKeyPrefix.Default;
            this.BAddToBlackList.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Default;
            this.BAddToBlackList.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Default;
            this.BAddToBlackList.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Default;
            this.BAddToBlackList.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.BAddToBlackList.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BAddToBlackList.ImageLocation = DevExpress.XtraEditors.ImageLocation.Default;
            this.BAddToBlackList.Location = new System.Drawing.Point(12, 386);
            this.BAddToBlackList.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            this.BAddToBlackList.Name = "BAddToBlackList";
            this.BAddToBlackList.Size = new System.Drawing.Size(260, 24);
            this.BAddToBlackList.TabIndex = 4;
            this.BAddToBlackList.Text = "Add Target to Black List";
            this.BAddToBlackList.ToolTipIconType = DevExpress.Utils.ToolTipIconType.None;
            this.BAddToBlackList.Click += new System.EventHandler(this.BAddToBlackListClick);
            // 
            // BRecordPoint
            // 
            this.BRecordPoint.AllowHtmlTextInToolTip = DevExpress.Utils.DefaultBoolean.Default;
            this.BRecordPoint.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.BRecordPoint.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.BRecordPoint.Appearance.TextOptions.HotkeyPrefix = DevExpress.Utils.HKeyPrefix.Default;
            this.BRecordPoint.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Default;
            this.BRecordPoint.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Default;
            this.BRecordPoint.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Default;
            this.BRecordPoint.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.BRecordPoint.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BRecordPoint.ImageLocation = DevExpress.XtraEditors.ImageLocation.Default;
            this.BRecordPoint.Location = new System.Drawing.Point(12, 357);
            this.BRecordPoint.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            this.BRecordPoint.Name = "BRecordPoint";
            this.BRecordPoint.Size = new System.Drawing.Size(262, 23);
            this.BRecordPoint.TabIndex = 5;
            this.BRecordPoint.Text = "Record Way";
            this.BRecordPoint.ToolTipIconType = DevExpress.Utils.ToolTipIconType.None;
            this.BRecordPoint.Click += new System.EventHandler(this.BRecordPointClick);
            // 
            // BDelBlackList
            // 
            this.BDelBlackList.AllowHtmlTextInToolTip = DevExpress.Utils.DefaultBoolean.Default;
            this.BDelBlackList.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.BDelBlackList.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.BDelBlackList.Appearance.TextOptions.HotkeyPrefix = DevExpress.Utils.HKeyPrefix.Default;
            this.BDelBlackList.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Default;
            this.BDelBlackList.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Default;
            this.BDelBlackList.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Default;
            this.BDelBlackList.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.BDelBlackList.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BDelBlackList.ImageLocation = DevExpress.XtraEditors.ImageLocation.Default;
            this.BDelBlackList.Location = new System.Drawing.Point(210, 208);
            this.BDelBlackList.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            this.BDelBlackList.Name = "BDelBlackList";
            this.BDelBlackList.Size = new System.Drawing.Size(43, 23);
            this.BDelBlackList.TabIndex = 6;
            this.BDelBlackList.Text = "Del";
            this.BDelBlackList.ToolTipIconType = DevExpress.Utils.ToolTipIconType.None;
            this.BDelBlackList.Click += new System.EventHandler(this.BDelBlackListClick);
            // 
            // BDelListPoint
            // 
            this.BDelListPoint.AllowHtmlTextInToolTip = DevExpress.Utils.DefaultBoolean.Default;
            this.BDelListPoint.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.BDelListPoint.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.BDelListPoint.Appearance.TextOptions.HotkeyPrefix = DevExpress.Utils.HKeyPrefix.Default;
            this.BDelListPoint.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Default;
            this.BDelListPoint.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Default;
            this.BDelListPoint.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Default;
            this.BDelListPoint.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.BDelListPoint.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BDelListPoint.ImageLocation = DevExpress.XtraEditors.ImageLocation.Default;
            this.BDelListPoint.Location = new System.Drawing.Point(210, 130);
            this.BDelListPoint.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            this.BDelListPoint.Name = "BDelListPoint";
            this.BDelListPoint.Size = new System.Drawing.Size(43, 23);
            this.BDelListPoint.TabIndex = 7;
            this.BDelListPoint.Text = "Del";
            this.BDelListPoint.ToolTipIconType = DevExpress.Utils.ToolTipIconType.None;
            this.BDelListPoint.Click += new System.EventHandler(this.BDelListPointClick);
            // 
            // LFirstPointInfo
            // 
            this.LFirstPointInfo.AutoSize = true;
            this.LFirstPointInfo.Location = new System.Drawing.Point(9, 341);
            this.LFirstPointInfo.Name = "LFirstPointInfo";
            this.LFirstPointInfo.Size = new System.Drawing.Size(124, 13);
            this.LFirstPointInfo.TabIndex = 8;
            this.LFirstPointInfo.Text = "First Location of Profile: ";
            // 
            // TUpdateFirstPointInfo
            // 
            this.TUpdateFirstPointInfo.Enabled = true;
            this.TUpdateFirstPointInfo.Interval = 450;
            this.TUpdateFirstPointInfo.Tick += new System.EventHandler(this.UpdateFirstPointInfoTick);
            // 
            // BSave
            // 
            this.BSave.AllowHtmlTextInToolTip = DevExpress.Utils.DefaultBoolean.Default;
            this.BSave.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.BSave.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.BSave.Appearance.TextOptions.HotkeyPrefix = DevExpress.Utils.HKeyPrefix.Default;
            this.BSave.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Default;
            this.BSave.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Default;
            this.BSave.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Default;
            this.BSave.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.BSave.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BSave.ImageLocation = DevExpress.XtraEditors.ImageLocation.Default;
            this.BSave.Location = new System.Drawing.Point(146, 443);
            this.BSave.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            this.BSave.Name = "BSave";
            this.BSave.Size = new System.Drawing.Size(128, 23);
            this.BSave.TabIndex = 9;
            this.BSave.Text = "Save Profile";
            this.BSave.ToolTipIconType = DevExpress.Utils.ToolTipIconType.None;
            this.BSave.Click += new System.EventHandler(this.BSaveClick);
            // 
            // BLoadProfil
            // 
            this.BLoadProfil.AllowHtmlTextInToolTip = DevExpress.Utils.DefaultBoolean.Default;
            this.BLoadProfil.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.BLoadProfil.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.BLoadProfil.Appearance.TextOptions.HotkeyPrefix = DevExpress.Utils.HKeyPrefix.Default;
            this.BLoadProfil.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Default;
            this.BLoadProfil.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Default;
            this.BLoadProfil.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Default;
            this.BLoadProfil.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.BLoadProfil.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BLoadProfil.ImageLocation = DevExpress.XtraEditors.ImageLocation.Default;
            this.BLoadProfil.Location = new System.Drawing.Point(12, 443);
            this.BLoadProfil.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            this.BLoadProfil.Name = "BLoadProfil";
            this.BLoadProfil.Size = new System.Drawing.Size(126, 23);
            this.BLoadProfil.TabIndex = 10;
            this.BLoadProfil.Text = "Load Profile";
            this.BLoadProfil.ToolTipIconType = DevExpress.Utils.ToolTipIconType.None;
            this.BLoadProfil.Click += new System.EventHandler(this.BLoadProfilClick);
            // 
            // hotSpotCB
            // 
            this.hotSpotCB.AllowHtmlTextInToolTip = DevExpress.Utils.DefaultBoolean.Default;
            this.hotSpotCB.Location = new System.Drawing.Point(179, 19);
            this.hotSpotCB.Name = "hotSpotCB";
            this.hotSpotCB.Properties.AccessibleRole = System.Windows.Forms.AccessibleRole.Default;
            this.hotSpotCB.Properties.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.hotSpotCB.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.hotSpotCB.Properties.Appearance.TextOptions.HotkeyPrefix = DevExpress.Utils.HKeyPrefix.Default;
            this.hotSpotCB.Properties.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Default;
            this.hotSpotCB.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Default;
            this.hotSpotCB.Properties.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Default;
            this.hotSpotCB.Properties.AppearanceDisabled.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.hotSpotCB.Properties.AppearanceDisabled.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.hotSpotCB.Properties.AppearanceDisabled.TextOptions.HotkeyPrefix = DevExpress.Utils.HKeyPrefix.Default;
            this.hotSpotCB.Properties.AppearanceDisabled.TextOptions.Trimming = DevExpress.Utils.Trimming.Default;
            this.hotSpotCB.Properties.AppearanceDisabled.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Default;
            this.hotSpotCB.Properties.AppearanceDisabled.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Default;
            this.hotSpotCB.Properties.AppearanceFocused.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.hotSpotCB.Properties.AppearanceFocused.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.hotSpotCB.Properties.AppearanceFocused.TextOptions.HotkeyPrefix = DevExpress.Utils.HKeyPrefix.Default;
            this.hotSpotCB.Properties.AppearanceFocused.TextOptions.Trimming = DevExpress.Utils.Trimming.Default;
            this.hotSpotCB.Properties.AppearanceFocused.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Default;
            this.hotSpotCB.Properties.AppearanceFocused.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Default;
            this.hotSpotCB.Properties.AppearanceReadOnly.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.hotSpotCB.Properties.AppearanceReadOnly.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.hotSpotCB.Properties.AppearanceReadOnly.TextOptions.HotkeyPrefix = DevExpress.Utils.HKeyPrefix.Default;
            this.hotSpotCB.Properties.AppearanceReadOnly.TextOptions.Trimming = DevExpress.Utils.Trimming.Default;
            this.hotSpotCB.Properties.AppearanceReadOnly.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Default;
            this.hotSpotCB.Properties.AppearanceReadOnly.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Default;
            this.hotSpotCB.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.hotSpotCB.Properties.Caption = "Hot Spot";
            this.hotSpotCB.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Standard;
            this.hotSpotCB.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Default;
            this.hotSpotCB.Properties.ExportMode = DevExpress.XtraEditors.Repository.ExportMode.Default;
            this.hotSpotCB.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            this.hotSpotCB.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.InactiveChecked;
            this.hotSpotCB.Size = new System.Drawing.Size(93, 18);
            this.hotSpotCB.TabIndex = 15;
            this.hotSpotCB.ToolTipIconType = DevExpress.Utils.ToolTipIconType.None;
            this.hotSpotCB.CheckedChanged += new System.EventHandler(this.CheckBox1CheckedChanged);
            // 
            // PlayerFactionCB
            // 
            this.PlayerFactionCB.FormattingEnabled = true;
            this.PlayerFactionCB.Items.AddRange(new object[] {
            "All",
            "Alliance",
            "Horde"});
            this.PlayerFactionCB.Location = new System.Drawing.Point(38, 319);
            this.PlayerFactionCB.Name = "PlayerFactionCB";
            this.PlayerFactionCB.Size = new System.Drawing.Size(236, 21);
            this.PlayerFactionCB.TabIndex = 16;
            this.PlayerFactionCB.Text = "All";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 322);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "For:";
            // 
            // BDtargetFaction
            // 
            this.BDtargetFaction.AllowHtmlTextInToolTip = DevExpress.Utils.DefaultBoolean.Default;
            this.BDtargetFaction.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.BDtargetFaction.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.BDtargetFaction.Appearance.TextOptions.HotkeyPrefix = DevExpress.Utils.HKeyPrefix.Default;
            this.BDtargetFaction.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Default;
            this.BDtargetFaction.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Default;
            this.BDtargetFaction.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Default;
            this.BDtargetFaction.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.BDtargetFaction.DialogResult = System.Windows.Forms.DialogResult.None;
            this.BDtargetFaction.ImageLocation = DevExpress.XtraEditors.ImageLocation.Default;
            this.BDtargetFaction.Location = new System.Drawing.Point(210, 287);
            this.BDtargetFaction.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            this.BDtargetFaction.Name = "BDtargetFaction";
            this.BDtargetFaction.Size = new System.Drawing.Size(43, 23);
            this.BDtargetFaction.TabIndex = 20;
            this.BDtargetFaction.Text = "Del";
            this.BDtargetFaction.ToolTipIconType = DevExpress.Utils.ToolTipIconType.None;
            this.BDtargetFaction.Click += new System.EventHandler(this.BDtargetFactionClick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 239);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(139, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Target Faction at attacked:";
            // 
            // targetFactionList
            // 
            this.targetFactionList.AllowHtmlTextInToolTip = DevExpress.Utils.DefaultBoolean.Default;
            this.targetFactionList.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.targetFactionList.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.targetFactionList.Appearance.TextOptions.HotkeyPrefix = DevExpress.Utils.HKeyPrefix.Default;
            this.targetFactionList.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Default;
            this.targetFactionList.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Default;
            this.targetFactionList.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Default;
            this.targetFactionList.HighlightedItemStyle = DevExpress.XtraEditors.HighlightStyle.Default;
            this.targetFactionList.HotTrackSelectMode = DevExpress.XtraEditors.HotTrackSelectMode.SelectItemOnHotTrack;
            this.targetFactionList.Location = new System.Drawing.Point(12, 257);
            this.targetFactionList.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            this.targetFactionList.Name = "targetFactionList";
            this.targetFactionList.SelectionMode = System.Windows.Forms.SelectionMode.One;
            this.targetFactionList.Size = new System.Drawing.Size(262, 56);
            this.targetFactionList.SortOrder = System.Windows.Forms.SortOrder.None;
            this.targetFactionList.TabIndex = 18;
            this.targetFactionList.ToolTipIconType = DevExpress.Utils.ToolTipIconType.None;
            // 
            // ABtargetFaction
            // 
            this.ABtargetFaction.AllowHtmlTextInToolTip = DevExpress.Utils.DefaultBoolean.Default;
            this.ABtargetFaction.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.ABtargetFaction.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.ABtargetFaction.Appearance.TextOptions.HotkeyPrefix = DevExpress.Utils.HKeyPrefix.Default;
            this.ABtargetFaction.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Default;
            this.ABtargetFaction.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Default;
            this.ABtargetFaction.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Default;
            this.ABtargetFaction.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.ABtargetFaction.DialogResult = System.Windows.Forms.DialogResult.None;
            this.ABtargetFaction.ImageLocation = DevExpress.XtraEditors.ImageLocation.Default;
            this.ABtargetFaction.Location = new System.Drawing.Point(12, 413);
            this.ABtargetFaction.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            this.ABtargetFaction.Name = "ABtargetFaction";
            this.ABtargetFaction.Size = new System.Drawing.Size(260, 24);
            this.ABtargetFaction.TabIndex = 21;
            this.ABtargetFaction.Text = "Add Target to Target Faction";
            this.ABtargetFaction.ToolTipIconType = DevExpress.Utils.ToolTipIconType.None;
            this.ABtargetFaction.Click += new System.EventHandler(this.ABtargetFactionClick);
            // 
            // RadarB
            // 
            this.RadarB.AllowHtmlTextInToolTip = DevExpress.Utils.DefaultBoolean.Default;
            this.RadarB.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.RadarB.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.RadarB.Appearance.TextOptions.HotkeyPrefix = DevExpress.Utils.HKeyPrefix.Default;
            this.RadarB.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Default;
            this.RadarB.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Default;
            this.RadarB.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Default;
            this.RadarB.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.RadarB.DialogResult = System.Windows.Forms.DialogResult.None;
            this.RadarB.ImageLocation = DevExpress.XtraEditors.ImageLocation.Default;
            this.RadarB.Location = new System.Drawing.Point(187, 2);
            this.RadarB.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            this.RadarB.Name = "RadarB";
            this.RadarB.Size = new System.Drawing.Size(85, 17);
            this.RadarB.TabIndex = 22;
            this.RadarB.Text = "Radar";
            this.RadarB.ToolTipIconType = DevExpress.Utils.ToolTipIconType.None;
            this.RadarB.Click += new System.EventHandler(this.RadarBClick);
            // 
            // ProfileManager
            // 
            this.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.Appearance.TextOptions.HotkeyPrefix = DevExpress.Utils.HKeyPrefix.Default;
            this.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Default;
            this.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Default;
            this.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Default;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(286, 469);
            this.Controls.Add(this.RadarB);
            this.Controls.Add(this.ABtargetFaction);
            this.Controls.Add(this.BDtargetFaction);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.targetFactionList);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.PlayerFactionCB);
            this.Controls.Add(this.hotSpotCB);
            this.Controls.Add(this.BLoadProfil);
            this.Controls.Add(this.BSave);
            this.Controls.Add(this.LFirstPointInfo);
            this.Controls.Add(this.BDelListPoint);
            this.Controls.Add(this.BDelBlackList);
            this.Controls.Add(this.BRecordPoint);
            this.Controls.Add(this.BAddToBlackList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ListOfBlackList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ListOfPoint);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            this.MaximizeBox = false;
            this.Name = "ProfileManager";
            this.ShowIcon = false;
            this.Text = "Profile Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProfileManagerFormClosing);
            this.Shown += new System.EventHandler(this.Profile_Manager_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.ListOfPoint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ListOfBlackList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hotSpotCB.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.targetFactionList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ListBoxControl ListOfPoint;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.ListBoxControl ListOfBlackList;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.SimpleButton BAddToBlackList;
        private DevExpress.XtraEditors.SimpleButton BRecordPoint;
        private DevExpress.XtraEditors.SimpleButton BDelBlackList;
        private DevExpress.XtraEditors.SimpleButton BDelListPoint;
        private System.Windows.Forms.Label LFirstPointInfo;
        private System.Windows.Forms.Timer TUpdateFirstPointInfo;
        private DevExpress.XtraEditors.SimpleButton BSave;
        private DevExpress.XtraEditors.SimpleButton BLoadProfil;
        private DevExpress.XtraEditors.CheckEdit hotSpotCB;
        private System.Windows.Forms.ComboBox PlayerFactionCB;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.SimpleButton BDtargetFaction;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.ListBoxControl targetFactionList;
        private DevExpress.XtraEditors.SimpleButton ABtargetFaction;
        private DevExpress.XtraEditors.SimpleButton RadarB;
    }
}