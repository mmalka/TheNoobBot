using nManager.Helpful.Forms.UserControls;

namespace The_Noob_Bot
{
    partial class Main
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.HomeTagButton = new System.Windows.Forms.Label();
            this.LogTabButton = new System.Windows.Forms.Label();
            this.AccountTabButton = new System.Windows.Forms.Label();
            this.PanelHome = new System.Windows.Forms.Panel();
            this.BotStartedSinceLabel = new System.Windows.Forms.Label();
            this.TargetHealth = new nManager.Helpful.Forms.UserControls.TnbProgressBar();
            this.TargetName = new System.Windows.Forms.Label();
            this.TargetLevel = new System.Windows.Forms.Label();
            this.LatestLog = new System.Windows.Forms.Label();
            this.TargetHealthLabel = new System.Windows.Forms.Label();
            this.LatestLogLabel = new System.Windows.Forms.Label();
            this.TargetLevelLabel = new System.Windows.Forms.Label();
            this.TargetNameLabel = new System.Windows.Forms.Label();
            this.Health = new nManager.Helpful.Forms.UserControls.TnbProgressBar();
            this.FarmsCount = new System.Windows.Forms.Label();
            this.DeathsCount = new System.Windows.Forms.Label();
            this.UnitKillsCount = new System.Windows.Forms.Label();
            this.XPPerHour = new System.Windows.Forms.Label();
            this.LootsCount = new System.Windows.Forms.Label();
            this.HonorPerHour = new System.Windows.Forms.Label();
            this.FarmsLabel = new System.Windows.Forms.Label();
            this.DeathsLabel = new System.Windows.Forms.Label();
            this.UnitKillsLabel = new System.Windows.Forms.Label();
            this.XPPerHourLabel = new System.Windows.Forms.Label();
            this.LootsLabel = new System.Windows.Forms.Label();
            this.HonorPerHourLabel = new System.Windows.Forms.Label();
            this.HealthLabel = new System.Windows.Forms.Label();
            this.PanelLog = new System.Windows.Forms.Panel();
            this.PanelAccount = new System.Windows.Forms.Panel();
            this.TimeLeft = new System.Windows.Forms.Label();
            this.AccountName = new System.Windows.Forms.Label();
            this.OnlineBot = new System.Windows.Forms.Label();
            this.TimeLeftLabel = new System.Windows.Forms.Label();
            this.GoToPaymentPageButton = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.ProductsPriceList = new nManager.Helpful.Forms.UserControls.TnbComboBox();
            this.AccountNameLabel = new System.Windows.Forms.Label();
            this.ProductsListPriceLabel = new System.Windows.Forms.Label();
            this.WantToSubscribeLabel = new System.Windows.Forms.Label();
            this.RemoteSessionInfo = new System.Windows.Forms.Label();
            this.RemoteSessionSwitchButton = new nManager.Helpful.Forms.UserControls.TnbSwitchButton();
            this.RemoteSessionLabel = new System.Windows.Forms.Label();
            this.WebsiteLink = new System.Windows.Forms.Label();
            this.QuestToolLabel = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.HomePanelTimer = new System.Windows.Forms.Timer(this.components);
            this.MainFormTimer = new System.Windows.Forms.Timer(this.components);
            this.AccountPanelTimer = new System.Windows.Forms.Timer(this.components);
            this.LoggingAreaTimer = new System.Windows.Forms.Timer(this.components);
            this.MainHeader = new nManager.Helpful.Forms.UserControls.TnbControlMenu();
            this.ProductList = new nManager.Helpful.Forms.UserControls.TnbComboBox();
            this.StartButton = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.ProductSettingsButton = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.MainSettingsButton = new nManager.Helpful.Forms.UserControls.TnbButton();
            this.DevToolsLabel = new System.Windows.Forms.Label();
            this.ProductStartedSinceLabel = new System.Windows.Forms.Label();
            this.PanelHome.SuspendLayout();
            this.PanelAccount.SuspendLayout();
            this.SuspendLayout();
            // 
            // HomeTagButton
            // 
            this.HomeTagButton.AutoEllipsis = true;
            this.HomeTagButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HomeTagButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(160)))), ((int)(((byte)(229)))));
            this.HomeTagButton.Image = global::The_Noob_Bot.Properties.Resources.tab_active_mainframe;
            this.HomeTagButton.Location = new System.Drawing.Point(1, 44);
            this.HomeTagButton.Margin = new System.Windows.Forms.Padding(0);
            this.HomeTagButton.MaximumSize = new System.Drawing.Size(108, 33);
            this.HomeTagButton.MinimumSize = new System.Drawing.Size(108, 33);
            this.HomeTagButton.Name = "HomeTagButton";
            this.HomeTagButton.Size = new System.Drawing.Size(108, 33);
            this.HomeTagButton.TabIndex = 13;
            this.HomeTagButton.Text = "HOME";
            this.HomeTagButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.HomeTagButton.Click += new System.EventHandler(this.HomeTagButton_Click);
            this.HomeTagButton.MouseEnter += new System.EventHandler(this.HomeTagButton_MouseEnter);
            this.HomeTagButton.MouseLeave += new System.EventHandler(this.HomeTagButton_MouseLeave);
            // 
            // LogTabButton
            // 
            this.LogTabButton.AutoEllipsis = true;
            this.LogTabButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LogTabButton.ForeColor = System.Drawing.Color.Snow;
            this.LogTabButton.Image = global::The_Noob_Bot.Properties.Resources.tab_inactive_mainframe;
            this.LogTabButton.Location = new System.Drawing.Point(109, 44);
            this.LogTabButton.Margin = new System.Windows.Forms.Padding(0);
            this.LogTabButton.MaximumSize = new System.Drawing.Size(108, 33);
            this.LogTabButton.MinimumSize = new System.Drawing.Size(108, 33);
            this.LogTabButton.Name = "LogTabButton";
            this.LogTabButton.Size = new System.Drawing.Size(108, 33);
            this.LogTabButton.TabIndex = 14;
            this.LogTabButton.Text = "LOG";
            this.LogTabButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LogTabButton.Click += new System.EventHandler(this.LogTabButton_Click);
            this.LogTabButton.MouseEnter += new System.EventHandler(this.LogTabButton_MouseEnter);
            this.LogTabButton.MouseLeave += new System.EventHandler(this.LogTabButton_MouseLeave);
            // 
            // AccountTabButton
            // 
            this.AccountTabButton.AutoEllipsis = true;
            this.AccountTabButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AccountTabButton.ForeColor = System.Drawing.Color.Snow;
            this.AccountTabButton.Image = global::The_Noob_Bot.Properties.Resources.tab_inactive_mainframe;
            this.AccountTabButton.Location = new System.Drawing.Point(217, 44);
            this.AccountTabButton.Margin = new System.Windows.Forms.Padding(0);
            this.AccountTabButton.MaximumSize = new System.Drawing.Size(108, 33);
            this.AccountTabButton.MinimumSize = new System.Drawing.Size(108, 33);
            this.AccountTabButton.Name = "AccountTabButton";
            this.AccountTabButton.Size = new System.Drawing.Size(108, 33);
            this.AccountTabButton.TabIndex = 15;
            this.AccountTabButton.Text = "MY ACCOUNT";
            this.AccountTabButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.AccountTabButton.Click += new System.EventHandler(this.AccountTabButton_Click);
            this.AccountTabButton.MouseEnter += new System.EventHandler(this.AccountTabButton_MouseEnter);
            this.AccountTabButton.MouseLeave += new System.EventHandler(this.AccountTabButton_MouseLeave);
            // 
            // PanelHome
            // 
            this.PanelHome.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.PanelHome.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("PanelHome.BackgroundImage")));
            this.PanelHome.Controls.Add(this.ProductStartedSinceLabel);
            this.PanelHome.Controls.Add(this.BotStartedSinceLabel);
            this.PanelHome.Controls.Add(this.TargetHealth);
            this.PanelHome.Controls.Add(this.TargetName);
            this.PanelHome.Controls.Add(this.TargetLevel);
            this.PanelHome.Controls.Add(this.LatestLog);
            this.PanelHome.Controls.Add(this.TargetHealthLabel);
            this.PanelHome.Controls.Add(this.LatestLogLabel);
            this.PanelHome.Controls.Add(this.TargetLevelLabel);
            this.PanelHome.Controls.Add(this.TargetNameLabel);
            this.PanelHome.Controls.Add(this.Health);
            this.PanelHome.Controls.Add(this.FarmsCount);
            this.PanelHome.Controls.Add(this.DeathsCount);
            this.PanelHome.Controls.Add(this.UnitKillsCount);
            this.PanelHome.Controls.Add(this.XPPerHour);
            this.PanelHome.Controls.Add(this.LootsCount);
            this.PanelHome.Controls.Add(this.HonorPerHour);
            this.PanelHome.Controls.Add(this.FarmsLabel);
            this.PanelHome.Controls.Add(this.DeathsLabel);
            this.PanelHome.Controls.Add(this.UnitKillsLabel);
            this.PanelHome.Controls.Add(this.XPPerHourLabel);
            this.PanelHome.Controls.Add(this.LootsLabel);
            this.PanelHome.Controls.Add(this.HonorPerHourLabel);
            this.PanelHome.Controls.Add(this.HealthLabel);
            this.PanelHome.Location = new System.Drawing.Point(1, 78);
            this.PanelHome.Margin = new System.Windows.Forms.Padding(0);
            this.PanelHome.MaximumSize = new System.Drawing.Size(573, 203);
            this.PanelHome.MinimumSize = new System.Drawing.Size(573, 203);
            this.PanelHome.Name = "PanelHome";
            this.PanelHome.Size = new System.Drawing.Size(573, 203);
            this.PanelHome.TabIndex = 16;
            // 
            // BotStartedSinceLabel
            // 
            this.BotStartedSinceLabel.AutoEllipsis = true;
            this.BotStartedSinceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BotStartedSinceLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.BotStartedSinceLabel.Location = new System.Drawing.Point(298, 120);
            this.BotStartedSinceLabel.Name = "BotStartedSinceLabel";
            this.BotStartedSinceLabel.Size = new System.Drawing.Size(260, 15);
            this.BotStartedSinceLabel.TabIndex = 49;
            this.BotStartedSinceLabel.Text = "Bot started since :";
            // 
            // TargetHealth
            // 
            this.TargetHealth.BarImage = global::The_Noob_Bot.Properties.Resources.barImg;
            this.TargetHealth.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            this.TargetHealth.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.TargetHealth.DrawMode = System.Windows.Forms.DrawMode.Normal;
            this.TargetHealth.Location = new System.Drawing.Point(418, 43);
            this.TargetHealth.Name = "TargetHealth";
            this.TargetHealth.Size = new System.Drawing.Size(115, 10);
            this.TargetHealth.TabIndex = 48;
            this.TargetHealth.Value = 90;
            // 
            // TargetName
            // 
            this.TargetName.AutoEllipsis = true;
            this.TargetName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TargetName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.TargetName.Location = new System.Drawing.Point(418, 12);
            this.TargetName.Name = "TargetName";
            this.TargetName.Size = new System.Drawing.Size(115, 15);
            this.TargetName.TabIndex = 44;
            this.TargetName.Text = "...";
            // 
            // TargetLevel
            // 
            this.TargetLevel.AutoEllipsis = true;
            this.TargetLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TargetLevel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.TargetLevel.Location = new System.Drawing.Point(418, 66);
            this.TargetLevel.Name = "TargetLevel";
            this.TargetLevel.Size = new System.Drawing.Size(115, 15);
            this.TargetLevel.TabIndex = 42;
            this.TargetLevel.Text = "...";
            // 
            // LatestLog
            // 
            this.LatestLog.AutoEllipsis = true;
            this.LatestLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LatestLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.LatestLog.Location = new System.Drawing.Point(298, 174);
            this.LatestLog.Name = "LatestLog";
            this.LatestLog.Size = new System.Drawing.Size(265, 15);
            this.LatestLog.TabIndex = 41;
            // 
            // TargetHealthLabel
            // 
            this.TargetHealthLabel.AutoEllipsis = true;
            this.TargetHealthLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TargetHealthLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.TargetHealthLabel.Location = new System.Drawing.Point(298, 39);
            this.TargetHealthLabel.Name = "TargetHealthLabel";
            this.TargetHealthLabel.Size = new System.Drawing.Size(115, 15);
            this.TargetHealthLabel.TabIndex = 38;
            this.TargetHealthLabel.Text = "Target\'s Health :";
            // 
            // LatestLogLabel
            // 
            this.LatestLogLabel.AutoEllipsis = true;
            this.LatestLogLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LatestLogLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.LatestLogLabel.Location = new System.Drawing.Point(298, 147);
            this.LatestLogLabel.Name = "LatestLogLabel";
            this.LatestLogLabel.Size = new System.Drawing.Size(260, 15);
            this.LatestLogLabel.TabIndex = 37;
            this.LatestLogLabel.Text = "Latest Log Entry :";
            // 
            // TargetLevelLabel
            // 
            this.TargetLevelLabel.AutoEllipsis = true;
            this.TargetLevelLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TargetLevelLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.TargetLevelLabel.Location = new System.Drawing.Point(298, 66);
            this.TargetLevelLabel.Name = "TargetLevelLabel";
            this.TargetLevelLabel.Size = new System.Drawing.Size(115, 15);
            this.TargetLevelLabel.TabIndex = 36;
            this.TargetLevelLabel.Text = "Target\'s Level :";
            // 
            // TargetNameLabel
            // 
            this.TargetNameLabel.AutoEllipsis = true;
            this.TargetNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TargetNameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.TargetNameLabel.Location = new System.Drawing.Point(298, 12);
            this.TargetNameLabel.Name = "TargetNameLabel";
            this.TargetNameLabel.Size = new System.Drawing.Size(115, 15);
            this.TargetNameLabel.TabIndex = 35;
            this.TargetNameLabel.Text = "Target\'s Name :";
            // 
            // Health
            // 
            this.Health.BarImage = ((System.Drawing.Image)(resources.GetObject("Health.BarImage")));
            this.Health.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            this.Health.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.Health.DrawMode = System.Windows.Forms.DrawMode.Normal;
            this.Health.Location = new System.Drawing.Point(131, 16);
            this.Health.Name = "Health";
            this.Health.Size = new System.Drawing.Size(115, 10);
            this.Health.TabIndex = 34;
            this.Health.Value = 20;
            // 
            // FarmsCount
            // 
            this.FarmsCount.AutoEllipsis = true;
            this.FarmsCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FarmsCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.FarmsCount.Location = new System.Drawing.Point(128, 120);
            this.FarmsCount.Name = "FarmsCount";
            this.FarmsCount.Size = new System.Drawing.Size(115, 15);
            this.FarmsCount.TabIndex = 33;
            this.FarmsCount.Text = "0 (0/hr)";
            // 
            // DeathsCount
            // 
            this.DeathsCount.AutoEllipsis = true;
            this.DeathsCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeathsCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.DeathsCount.Location = new System.Drawing.Point(128, 174);
            this.DeathsCount.Name = "DeathsCount";
            this.DeathsCount.Size = new System.Drawing.Size(115, 15);
            this.DeathsCount.TabIndex = 32;
            this.DeathsCount.Text = "0 (0/hr)";
            // 
            // UnitKillsCount
            // 
            this.UnitKillsCount.AutoEllipsis = true;
            this.UnitKillsCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UnitKillsCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.UnitKillsCount.Location = new System.Drawing.Point(128, 147);
            this.UnitKillsCount.Name = "UnitKillsCount";
            this.UnitKillsCount.Size = new System.Drawing.Size(115, 15);
            this.UnitKillsCount.TabIndex = 31;
            this.UnitKillsCount.Text = "0 (0/hr)";
            // 
            // XPPerHour
            // 
            this.XPPerHour.AutoEllipsis = true;
            this.XPPerHour.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XPPerHour.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.XPPerHour.Location = new System.Drawing.Point(128, 39);
            this.XPPerHour.Name = "XPPerHour";
            this.XPPerHour.Size = new System.Drawing.Size(115, 15);
            this.XPPerHour.TabIndex = 30;
            this.XPPerHour.Text = "0";
            // 
            // LootsCount
            // 
            this.LootsCount.AutoEllipsis = true;
            this.LootsCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LootsCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.LootsCount.Location = new System.Drawing.Point(128, 93);
            this.LootsCount.Name = "LootsCount";
            this.LootsCount.Size = new System.Drawing.Size(115, 15);
            this.LootsCount.TabIndex = 29;
            this.LootsCount.Text = "0 (0/hr)";
            // 
            // HonorPerHour
            // 
            this.HonorPerHour.AutoEllipsis = true;
            this.HonorPerHour.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HonorPerHour.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.HonorPerHour.Location = new System.Drawing.Point(128, 66);
            this.HonorPerHour.Name = "HonorPerHour";
            this.HonorPerHour.Size = new System.Drawing.Size(115, 15);
            this.HonorPerHour.TabIndex = 28;
            this.HonorPerHour.Text = "0";
            // 
            // FarmsLabel
            // 
            this.FarmsLabel.AutoEllipsis = true;
            this.FarmsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FarmsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.FarmsLabel.Location = new System.Drawing.Point(8, 120);
            this.FarmsLabel.Name = "FarmsLabel";
            this.FarmsLabel.Size = new System.Drawing.Size(115, 15);
            this.FarmsLabel.TabIndex = 26;
            this.FarmsLabel.Text = "Farms :";
            // 
            // DeathsLabel
            // 
            this.DeathsLabel.AutoEllipsis = true;
            this.DeathsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeathsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.DeathsLabel.Location = new System.Drawing.Point(8, 174);
            this.DeathsLabel.Name = "DeathsLabel";
            this.DeathsLabel.Size = new System.Drawing.Size(115, 15);
            this.DeathsLabel.TabIndex = 25;
            this.DeathsLabel.Text = "Deaths :";
            // 
            // UnitKillsLabel
            // 
            this.UnitKillsLabel.AutoEllipsis = true;
            this.UnitKillsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UnitKillsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.UnitKillsLabel.Location = new System.Drawing.Point(8, 147);
            this.UnitKillsLabel.Name = "UnitKillsLabel";
            this.UnitKillsLabel.Size = new System.Drawing.Size(115, 15);
            this.UnitKillsLabel.TabIndex = 24;
            this.UnitKillsLabel.Text = "Unit Kills :";
            // 
            // XPPerHourLabel
            // 
            this.XPPerHourLabel.AutoEllipsis = true;
            this.XPPerHourLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XPPerHourLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.XPPerHourLabel.Location = new System.Drawing.Point(8, 39);
            this.XPPerHourLabel.Name = "XPPerHourLabel";
            this.XPPerHourLabel.Size = new System.Drawing.Size(115, 15);
            this.XPPerHourLabel.TabIndex = 23;
            this.XPPerHourLabel.Text = "XP per Hour :";
            // 
            // LootsLabel
            // 
            this.LootsLabel.AutoEllipsis = true;
            this.LootsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LootsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.LootsLabel.Location = new System.Drawing.Point(8, 93);
            this.LootsLabel.Name = "LootsLabel";
            this.LootsLabel.Size = new System.Drawing.Size(115, 15);
            this.LootsLabel.TabIndex = 22;
            this.LootsLabel.Text = "Loots :";
            // 
            // HonorPerHourLabel
            // 
            this.HonorPerHourLabel.AutoEllipsis = true;
            this.HonorPerHourLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HonorPerHourLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.HonorPerHourLabel.Location = new System.Drawing.Point(8, 66);
            this.HonorPerHourLabel.Name = "HonorPerHourLabel";
            this.HonorPerHourLabel.Size = new System.Drawing.Size(115, 15);
            this.HonorPerHourLabel.TabIndex = 21;
            this.HonorPerHourLabel.Text = "Honor per Hour :";
            // 
            // HealthLabel
            // 
            this.HealthLabel.AutoEllipsis = true;
            this.HealthLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HealthLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.HealthLabel.Location = new System.Drawing.Point(8, 12);
            this.HealthLabel.Name = "HealthLabel";
            this.HealthLabel.Size = new System.Drawing.Size(115, 15);
            this.HealthLabel.TabIndex = 18;
            this.HealthLabel.Text = "Health :";
            // 
            // PanelLog
            // 
            this.PanelLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.PanelLog.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("PanelLog.BackgroundImage")));
            this.PanelLog.Location = new System.Drawing.Point(1, 78);
            this.PanelLog.Margin = new System.Windows.Forms.Padding(0);
            this.PanelLog.MaximumSize = new System.Drawing.Size(573, 203);
            this.PanelLog.MinimumSize = new System.Drawing.Size(573, 203);
            this.PanelLog.Name = "PanelLog";
            this.PanelLog.Size = new System.Drawing.Size(573, 203);
            this.PanelLog.TabIndex = 17;
            this.PanelLog.Visible = false;
            // 
            // PanelAccount
            // 
            this.PanelAccount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.PanelAccount.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("PanelAccount.BackgroundImage")));
            this.PanelAccount.Controls.Add(this.TimeLeft);
            this.PanelAccount.Controls.Add(this.AccountName);
            this.PanelAccount.Controls.Add(this.OnlineBot);
            this.PanelAccount.Controls.Add(this.TimeLeftLabel);
            this.PanelAccount.Controls.Add(this.GoToPaymentPageButton);
            this.PanelAccount.Controls.Add(this.ProductsPriceList);
            this.PanelAccount.Controls.Add(this.AccountNameLabel);
            this.PanelAccount.Controls.Add(this.ProductsListPriceLabel);
            this.PanelAccount.Controls.Add(this.WantToSubscribeLabel);
            this.PanelAccount.Controls.Add(this.RemoteSessionInfo);
            this.PanelAccount.Controls.Add(this.RemoteSessionSwitchButton);
            this.PanelAccount.Controls.Add(this.RemoteSessionLabel);
            this.PanelAccount.Location = new System.Drawing.Point(1, 78);
            this.PanelAccount.Margin = new System.Windows.Forms.Padding(0);
            this.PanelAccount.MaximumSize = new System.Drawing.Size(573, 203);
            this.PanelAccount.MinimumSize = new System.Drawing.Size(573, 203);
            this.PanelAccount.Name = "PanelAccount";
            this.PanelAccount.Size = new System.Drawing.Size(573, 203);
            this.PanelAccount.TabIndex = 17;
            this.PanelAccount.Visible = false;
            // 
            // TimeLeft
            // 
            this.TimeLeft.AutoEllipsis = true;
            this.TimeLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TimeLeft.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(160)))), ((int)(((byte)(229)))));
            this.TimeLeft.Location = new System.Drawing.Point(9, 134);
            this.TimeLeft.Name = "TimeLeft";
            this.TimeLeft.Size = new System.Drawing.Size(225, 15);
            this.TimeLeft.TabIndex = 15;
            this.TimeLeft.Text = "0 days and 0 hours";
            // 
            // AccountName
            // 
            this.AccountName.AutoEllipsis = true;
            this.AccountName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AccountName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(160)))), ((int)(((byte)(229)))));
            this.AccountName.Location = new System.Drawing.Point(9, 83);
            this.AccountName.Name = "AccountName";
            this.AccountName.Size = new System.Drawing.Size(115, 15);
            this.AccountName.TabIndex = 14;
            this.AccountName.Text = "AccountName";
            // 
            // OnlineBot
            // 
            this.OnlineBot.AutoEllipsis = true;
            this.OnlineBot.AutoSize = true;
            this.OnlineBot.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OnlineBot.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.OnlineBot.Location = new System.Drawing.Point(9, 167);
            this.OnlineBot.Name = "OnlineBot";
            this.OnlineBot.Size = new System.Drawing.Size(93, 15);
            this.OnlineBot.TabIndex = 13;
            this.OnlineBot.Text = "0 Online Bots";
            // 
            // TimeLeftLabel
            // 
            this.TimeLeftLabel.AutoEllipsis = true;
            this.TimeLeftLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TimeLeftLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.TimeLeftLabel.Location = new System.Drawing.Point(9, 115);
            this.TimeLeftLabel.Name = "TimeLeftLabel";
            this.TimeLeftLabel.Size = new System.Drawing.Size(115, 15);
            this.TimeLeftLabel.TabIndex = 11;
            this.TimeLeftLabel.Text = "Time left:";
            // 
            // GoToPaymentPageButton
            // 
            this.GoToPaymentPageButton.AutoEllipsis = true;
            this.GoToPaymentPageButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.GoToPaymentPageButton.ForeColor = System.Drawing.Color.Snow;
            this.GoToPaymentPageButton.HooverImage = global::The_Noob_Bot.Properties.Resources.greenB_200;
            this.GoToPaymentPageButton.Image = global::The_Noob_Bot.Properties.Resources.blueB_200;
            this.GoToPaymentPageButton.Location = new System.Drawing.Point(286, 152);
            this.GoToPaymentPageButton.Name = "GoToPaymentPageButton";
            this.GoToPaymentPageButton.Size = new System.Drawing.Size(200, 29);
            this.GoToPaymentPageButton.TabIndex = 10;
            this.GoToPaymentPageButton.Text = "Open the payment page";
            this.GoToPaymentPageButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.GoToPaymentPageButton.Click += new System.EventHandler(this.GoToPaymentPageButton_Click);
            // 
            // ProductsPriceList
            // 
            this.ProductsPriceList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.ProductsPriceList.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            this.ProductsPriceList.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.ProductsPriceList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ProductsPriceList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ProductsPriceList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProductsPriceList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(160)))), ((int)(((byte)(229)))));
            this.ProductsPriceList.FormattingEnabled = true;
            this.ProductsPriceList.HighlightColor = System.Drawing.Color.Gainsboro;
            this.ProductsPriceList.ItemHeight = 20;
            this.ProductsPriceList.Location = new System.Drawing.Point(286, 119);
            this.ProductsPriceList.Name = "ProductsPriceList";
            this.ProductsPriceList.SelectorBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(106)))), ((int)(((byte)(194)))));
            this.ProductsPriceList.SelectorImage = ((System.Drawing.Image)(resources.GetObject("ProductsPriceList.SelectorImage")));
            this.ProductsPriceList.Size = new System.Drawing.Size(200, 26);
            this.ProductsPriceList.TabIndex = 9;
            // 
            // AccountNameLabel
            // 
            this.AccountNameLabel.AutoEllipsis = true;
            this.AccountNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AccountNameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.AccountNameLabel.Location = new System.Drawing.Point(9, 64);
            this.AccountNameLabel.Name = "AccountNameLabel";
            this.AccountNameLabel.Size = new System.Drawing.Size(115, 15);
            this.AccountNameLabel.TabIndex = 6;
            this.AccountNameLabel.Text = "Account Login:";
            // 
            // ProductsListPriceLabel
            // 
            this.ProductsListPriceLabel.AutoEllipsis = true;
            this.ProductsListPriceLabel.AutoSize = true;
            this.ProductsListPriceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProductsListPriceLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(159)))), ((int)(((byte)(159)))));
            this.ProductsListPriceLabel.Location = new System.Drawing.Point(286, 97);
            this.ProductsListPriceLabel.Name = "ProductsListPriceLabel";
            this.ProductsListPriceLabel.Size = new System.Drawing.Size(162, 12);
            this.ProductsListPriceLabel.TabIndex = 5;
            this.ProductsListPriceLabel.Text = "Select an offer in the list below";
            this.ProductsListPriceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // WantToSubscribeLabel
            // 
            this.WantToSubscribeLabel.AutoEllipsis = true;
            this.WantToSubscribeLabel.AutoSize = true;
            this.WantToSubscribeLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.WantToSubscribeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WantToSubscribeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(161)))), ((int)(((byte)(75)))));
            this.WantToSubscribeLabel.Location = new System.Drawing.Point(286, 65);
            this.WantToSubscribeLabel.Name = "WantToSubscribeLabel";
            this.WantToSubscribeLabel.Size = new System.Drawing.Size(180, 16);
            this.WantToSubscribeLabel.TabIndex = 4;
            this.WantToSubscribeLabel.Text = "WANT TO SUBSCRIBE ?";
            this.WantToSubscribeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RemoteSessionInfo
            // 
            this.RemoteSessionInfo.AutoEllipsis = true;
            this.RemoteSessionInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RemoteSessionInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(160)))), ((int)(((byte)(229)))));
            this.RemoteSessionInfo.Location = new System.Drawing.Point(240, 14);
            this.RemoteSessionInfo.Name = "RemoteSessionInfo";
            this.RemoteSessionInfo.Size = new System.Drawing.Size(319, 15);
            this.RemoteSessionInfo.TabIndex = 2;
            this.RemoteSessionInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RemoteSessionSwitchButton
            // 
            this.RemoteSessionSwitchButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.RemoteSessionSwitchButton.Location = new System.Drawing.Point(162, 12);
            this.RemoteSessionSwitchButton.MaximumSize = new System.Drawing.Size(60, 20);
            this.RemoteSessionSwitchButton.MinimumSize = new System.Drawing.Size(60, 20);
            this.RemoteSessionSwitchButton.Name = "RemoteSessionSwitchButton";
            this.RemoteSessionSwitchButton.OffText = "OFF";
            this.RemoteSessionSwitchButton.OnText = "ON";
            this.RemoteSessionSwitchButton.Size = new System.Drawing.Size(60, 20);
            this.RemoteSessionSwitchButton.TabIndex = 1;
            this.RemoteSessionSwitchButton.Value = false;
            this.RemoteSessionSwitchButton.ValueChanged += new System.EventHandler(this.RemoteSessionSwitchButton_ValueChanged);
            // 
            // RemoteSessionLabel
            // 
            this.RemoteSessionLabel.AutoEllipsis = true;
            this.RemoteSessionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RemoteSessionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(160)))), ((int)(((byte)(229)))));
            this.RemoteSessionLabel.Location = new System.Drawing.Point(8, 14);
            this.RemoteSessionLabel.Name = "RemoteSessionLabel";
            this.RemoteSessionLabel.Size = new System.Drawing.Size(150, 13);
            this.RemoteSessionLabel.TabIndex = 0;
            this.RemoteSessionLabel.Text = "REMOTE SESSION";
            this.RemoteSessionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // WebsiteLink
            // 
            this.WebsiteLink.AutoEllipsis = true;
            this.WebsiteLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WebsiteLink.ForeColor = System.Drawing.Color.Snow;
            this.WebsiteLink.Image = ((System.Drawing.Image)(resources.GetObject("WebsiteLink.Image")));
            this.WebsiteLink.Location = new System.Drawing.Point(545, 44);
            this.WebsiteLink.Margin = new System.Windows.Forms.Padding(0);
            this.WebsiteLink.MaximumSize = new System.Drawing.Size(25, 33);
            this.WebsiteLink.MinimumSize = new System.Drawing.Size(25, 33);
            this.WebsiteLink.Name = "WebsiteLink";
            this.WebsiteLink.Size = new System.Drawing.Size(25, 33);
            this.WebsiteLink.TabIndex = 21;
            this.WebsiteLink.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.WebsiteLink.Click += new System.EventHandler(this.WebsiteLink_Click);
            this.WebsiteLink.MouseEnter += new System.EventHandler(this.WebsiteLink_MouseEnter);
            this.WebsiteLink.MouseLeave += new System.EventHandler(this.WebsiteLink_MouseLeave);
            // 
            // QuestToolLabel
            // 
            this.QuestToolLabel.AutoEllipsis = true;
            this.QuestToolLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QuestToolLabel.ForeColor = System.Drawing.Color.Snow;
            this.QuestToolLabel.Image = global::The_Noob_Bot.Properties.Resources.tab_inactive_mainframe;
            this.QuestToolLabel.Location = new System.Drawing.Point(462, 44);
            this.QuestToolLabel.Margin = new System.Windows.Forms.Padding(0);
            this.QuestToolLabel.MaximumSize = new System.Drawing.Size(80, 33);
            this.QuestToolLabel.MinimumSize = new System.Drawing.Size(80, 33);
            this.QuestToolLabel.Name = "QuestToolLabel";
            this.QuestToolLabel.Size = new System.Drawing.Size(80, 33);
            this.QuestToolLabel.TabIndex = 22;
            this.QuestToolLabel.Text = "QuestTool";
            this.QuestToolLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.QuestToolLabel.Click += new System.EventHandler(this.QuestToolLabel_Click);
            this.QuestToolLabel.MouseEnter += new System.EventHandler(this.QuestToolLabel_MouseEnter);
            this.QuestToolLabel.MouseLeave += new System.EventHandler(this.QuestToolLabel_MouseLeave);
            // 
            // HomePanelTimer
            // 
            this.HomePanelTimer.Enabled = true;
            this.HomePanelTimer.Interval = 1000;
            this.HomePanelTimer.Tick += new System.EventHandler(this.MainPanelTimer_Tick);
            // 
            // MainFormTimer
            // 
            this.MainFormTimer.Enabled = true;
            this.MainFormTimer.Interval = 250;
            this.MainFormTimer.Tick += new System.EventHandler(this.MainFormTimer_Tick);
            // 
            // AccountPanelTimer
            // 
            this.AccountPanelTimer.Enabled = true;
            this.AccountPanelTimer.Interval = 1000;
            this.AccountPanelTimer.Tick += new System.EventHandler(this.AccountPanelTimer_Tick);
            // 
            // MainHeader
            // 
            this.MainHeader.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("MainHeader.BackgroundImage")));
            this.MainHeader.Location = new System.Drawing.Point(0, 0);
            this.MainHeader.LogoImage = ((System.Drawing.Image)(resources.GetObject("MainHeader.LogoImage")));
            this.MainHeader.Name = "MainHeader";
            this.MainHeader.Size = new System.Drawing.Size(575, 43);
            this.MainHeader.TabIndex = 23;
            this.MainHeader.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.MainHeader.TitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
            this.MainHeader.TitleText = "PlayerName - TheNoobBot - DevVersionRestrict";
            // 
            // ProductList
            // 
            this.ProductList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.ProductList.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(121)))), ((int)(((byte)(121)))));
            this.ProductList.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.ProductList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ProductList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ProductList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProductList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(160)))), ((int)(((byte)(229)))));
            this.ProductList.FormattingEnabled = true;
            this.ProductList.HighlightColor = System.Drawing.Color.Gainsboro;
            this.ProductList.ItemHeight = 20;
            this.ProductList.Location = new System.Drawing.Point(13, 293);
            this.ProductList.Margin = new System.Windows.Forms.Padding(0);
            this.ProductList.MaximumSize = new System.Drawing.Size(549, 0);
            this.ProductList.MinimumSize = new System.Drawing.Size(549, 0);
            this.ProductList.Name = "ProductList";
            this.ProductList.SelectorBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(106)))), ((int)(((byte)(194)))));
            this.ProductList.SelectorImage = global::The_Noob_Bot.Properties.Resources.selectorBack_big;
            this.ProductList.Size = new System.Drawing.Size(549, 26);
            this.ProductList.TabIndex = 20;
            this.ProductList.SelectedIndexChanged += new System.EventHandler(this.ProductList_SelectedIndexChanged);
            // 
            // StartButton
            // 
            this.StartButton.AutoEllipsis = true;
            this.StartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartButton.ForeColor = System.Drawing.Color.Snow;
            this.StartButton.HooverImage = global::The_Noob_Bot.Properties.Resources.greenB_200;
            this.StartButton.Image = global::The_Noob_Bot.Properties.Resources.blueB_200;
            this.StartButton.Location = new System.Drawing.Point(12, 330);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(200, 29);
            this.StartButton.TabIndex = 19;
            this.StartButton.Text = "START PRODUCT";
            this.StartButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // ProductSettingsButton
            // 
            this.ProductSettingsButton.AutoEllipsis = true;
            this.ProductSettingsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProductSettingsButton.ForeColor = System.Drawing.Color.Snow;
            this.ProductSettingsButton.HooverImage = global::The_Noob_Bot.Properties.Resources.greenB_150;
            this.ProductSettingsButton.Image = global::The_Noob_Bot.Properties.Resources.blackB_150;
            this.ProductSettingsButton.Location = new System.Drawing.Point(238, 330);
            this.ProductSettingsButton.Name = "ProductSettingsButton";
            this.ProductSettingsButton.Size = new System.Drawing.Size(150, 29);
            this.ProductSettingsButton.TabIndex = 18;
            this.ProductSettingsButton.Text = "Product Settings";
            this.ProductSettingsButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ProductSettingsButton.Click += new System.EventHandler(this.ProductSettingsButton_Click);
            // 
            // MainSettingsButton
            // 
            this.MainSettingsButton.AutoEllipsis = true;
            this.MainSettingsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainSettingsButton.ForeColor = System.Drawing.Color.Snow;
            this.MainSettingsButton.HooverImage = global::The_Noob_Bot.Properties.Resources.greenB_150;
            this.MainSettingsButton.Image = global::The_Noob_Bot.Properties.Resources.blackB_150;
            this.MainSettingsButton.Location = new System.Drawing.Point(413, 330);
            this.MainSettingsButton.Name = "MainSettingsButton";
            this.MainSettingsButton.Size = new System.Drawing.Size(150, 29);
            this.MainSettingsButton.TabIndex = 17;
            this.MainSettingsButton.Text = "General Settings";
            this.MainSettingsButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.MainSettingsButton.Click += new System.EventHandler(this.MainSettingsButton_Click);
            // 
            // DevToolsLabel
            // 
            this.DevToolsLabel.AutoEllipsis = true;
            this.DevToolsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DevToolsLabel.ForeColor = System.Drawing.Color.Snow;
            this.DevToolsLabel.Image = global::The_Noob_Bot.Properties.Resources.tab_inactive_mainframe;
            this.DevToolsLabel.Location = new System.Drawing.Point(378, 44);
            this.DevToolsLabel.Margin = new System.Windows.Forms.Padding(0);
            this.DevToolsLabel.MaximumSize = new System.Drawing.Size(80, 33);
            this.DevToolsLabel.MinimumSize = new System.Drawing.Size(80, 33);
            this.DevToolsLabel.Name = "DevToolsLabel";
            this.DevToolsLabel.Size = new System.Drawing.Size(80, 33);
            this.DevToolsLabel.TabIndex = 24;
            this.DevToolsLabel.Text = "DevTools";
            this.DevToolsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.DevToolsLabel.Click += new System.EventHandler(this.DevToolsLabel_Click_1);
            this.DevToolsLabel.MouseEnter += new System.EventHandler(this.DevToolsLabel_MouseEnter);
            this.DevToolsLabel.MouseLeave += new System.EventHandler(this.DevToolsLabel_MouseLeave);
            // 
            // ProductStartedSinceLabel
            // 
            this.ProductStartedSinceLabel.AutoEllipsis = true;
            this.ProductStartedSinceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProductStartedSinceLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this.ProductStartedSinceLabel.Location = new System.Drawing.Point(298, 99);
            this.ProductStartedSinceLabel.Name = "ProductStartedSinceLabel";
            this.ProductStartedSinceLabel.Size = new System.Drawing.Size(260, 15);
            this.ProductStartedSinceLabel.TabIndex = 50;
            this.ProductStartedSinceLabel.Text = "Product started since :";
            // 
            // Main
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(575, 371);
            this.Controls.Add(this.DevToolsLabel);
            this.Controls.Add(this.MainHeader);
            this.Controls.Add(this.QuestToolLabel);
            this.Controls.Add(this.WebsiteLink);
            this.Controls.Add(this.ProductList);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.ProductSettingsButton);
            this.Controls.Add(this.MainSettingsButton);
            this.Controls.Add(this.AccountTabButton);
            this.Controls.Add(this.LogTabButton);
            this.Controls.Add(this.HomeTagButton);
            this.Controls.Add(this.PanelHome);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(575, 371);
            this.MinimumSize = new System.Drawing.Size(575, 371);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.Load += new System.EventHandler(this.Main_Load);
            this.PanelHome.ResumeLayout(false);
            this.PanelAccount.ResumeLayout(false);
            this.PanelAccount.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label HomeTagButton;
        private System.Windows.Forms.Label LogTabButton;
        private System.Windows.Forms.Label AccountTabButton;
        private System.Windows.Forms.Panel PanelHome;
        private System.Windows.Forms.Panel PanelLog;
        private System.Windows.Forms.Panel PanelAccount;
        private System.Windows.Forms.Label RemoteSessionLabel;
        private TnbComboBox ProductList;
        private TnbButton MainSettingsButton;
        private TnbButton ProductSettingsButton;
        private TnbButton StartButton;
        private TnbSwitchButton RemoteSessionSwitchButton;
        private System.Windows.Forms.Label RemoteSessionInfo;
        private TnbButton GoToPaymentPageButton;
        private TnbComboBox ProductsPriceList;
        private System.Windows.Forms.Label AccountNameLabel;
        private System.Windows.Forms.Label ProductsListPriceLabel;
        private System.Windows.Forms.Label WantToSubscribeLabel;
        private System.Windows.Forms.Label TimeLeft;
        private System.Windows.Forms.Label AccountName;
        private System.Windows.Forms.Label OnlineBot;
        private System.Windows.Forms.Label TimeLeftLabel;
        private System.Windows.Forms.Label FarmsLabel;
        private System.Windows.Forms.Label DeathsLabel;
        private System.Windows.Forms.Label UnitKillsLabel;
        private System.Windows.Forms.Label XPPerHourLabel;
        private System.Windows.Forms.Label LootsLabel;
        private System.Windows.Forms.Label HonorPerHourLabel;
        private System.Windows.Forms.Label HealthLabel;
        private System.Windows.Forms.Label FarmsCount;
        private System.Windows.Forms.Label DeathsCount;
        private System.Windows.Forms.Label UnitKillsCount;
        private System.Windows.Forms.Label XPPerHour;
        private System.Windows.Forms.Label LootsCount;
        private System.Windows.Forms.Label HonorPerHour;
        private System.Windows.Forms.Label TargetName;
        private System.Windows.Forms.Label TargetLevel;
        private System.Windows.Forms.Label LatestLog;
        private System.Windows.Forms.Label TargetHealthLabel;
        private System.Windows.Forms.Label LatestLogLabel;
        private System.Windows.Forms.Label TargetLevelLabel;
        private System.Windows.Forms.Label TargetNameLabel;
        private TnbProgressBar Health;
        private TnbProgressBar TargetHealth;
        private System.Windows.Forms.Label WebsiteLink;
        private System.Windows.Forms.Label QuestToolLabel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Timer HomePanelTimer;
        private System.Windows.Forms.Timer MainFormTimer;
        private System.Windows.Forms.Timer AccountPanelTimer;
        private System.Windows.Forms.Timer LoggingAreaTimer;
        private TnbControlMenu MainHeader;
        private System.Windows.Forms.Label BotStartedSinceLabel;
        private System.Windows.Forms.Label DevToolsLabel;
        private System.Windows.Forms.Label ProductStartedSinceLabel;
    }
}

