using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using nManager;
using nManager.Helpful;
using nManager.Helpful.Forms;
using nManager.Products;
using nManager.Wow;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using The_Noob_Bot.Properties;
using Math = System.Math;
using Pulsator = nManager.Pulsator;

namespace The_Noob_Bot
{
    public partial class Main : Form
    {
        private readonly Image _activeBackground = Resources.tab_active_mainframe;
        private readonly Color _activeColor = Color.FromArgb(98, 160, 229);
        private readonly Image _inactiveBackground = Resources.tab_inactive_mainframe;
        private readonly Color _inactiveColor = Color.FromArgb(232, 232, 232);
        private readonly List<Logging.Log> _listLog = new List<Logging.Log>();
        private int _hardAdded;
        private bool _isAccountActive;
        private bool _isHomeActive = true;
        private bool _isLogActive;
        private string _playerName = "";
        //private bool _started;
        private bool _wowInTaskBarre;

        public Main()
        {
            InitializeBot();
            InitializeComponent();
            Translate();
            if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                TopMost = true;
            InitializeUI();
            Logging.Status = "Startup Complete";
            using (Graphics g = CreateGraphics())
            {
                if (Math.Abs(g.DpiX - 96F) > 0.1)
                {
                    Logging.Write("There is a problem with your Windows Font Size configuration.");
                    Logging.Write("Please set it to 100% size or you may have problems reading ThePrivateBot's forms. hint: http://www.wikihow.com/Change-Font-Size-on-a-Computer");
                }
            }
        }

        private void InitializeBot()
        {
            try
            {
                // 1st set the culture to invariant
                SetDefaultCulture(CultureInfo.InvariantCulture);
                ScriptOnlineManager.LoadScript();
                // Create folder:
                Directory.CreateDirectory(Application.StartupPath + "\\Logs\\");
                Directory.CreateDirectory(Application.StartupPath + "\\CombatClasses\\");
                Directory.CreateDirectory(Application.StartupPath + "\\CombatClasses\\Settings\\");
                Directory.CreateDirectory(Application.StartupPath + "\\CombatClasses\\Talents\\");
                Directory.CreateDirectory(Application.StartupPath + "\\Meshes\\");
                Directory.CreateDirectory(Application.StartupPath + "\\Settings\\");
                Directory.CreateDirectory(Application.StartupPath + "\\Data\\");
                Directory.CreateDirectory(Application.StartupPath + "\\Products\\");
                Directory.CreateDirectory(Application.StartupPath + "\\Profiles\\");
                ConfigWowForThisBot.ConfigWow();

                new Remote();

                var spellBook = new Thread(ThreadSpellBook) {Name = "SpellBook Loading"};
                spellBook.Start();
                MovementManager.LaunchThreadMovementManager();
                if (nManagerSetting.CurrentSetting.ActivateBroadcastingMimesis)
                    Communication.Listen();

                _playerName = ObjectManager.Me.Name;
            }
            catch (Exception ex)
            {
                Logging.WriteError("Main > InitializeBot(): " + ex);
            }
        }


        private void InitializeUI()
        {
            try
            {
                MainHeader.TitleText = ObjectManager.Me.Name + " - " + Information.MainTitle;
                if (LoginServer.IsFreeVersion)
                    MainHeader.TitleText += " - Trial";


                // Products:
                int i = 0;
                int i2 = -1;
                foreach (string f in Others.GetFilesDirectory(Application.StartupPath + "\\Products\\", "*.dll"))
                {
                    string text = f.Replace(".dll", "");
                    Translate.Id ret;
                    if (Enum.TryParse(text.Replace(" ", "_") + "_Product_Description", true, out ret))
                    {
                        if (!string.IsNullOrEmpty(nManager.Translate.Get(ret)))
                            text = text + " - " + nManager.Translate.Get(ret);
                    }
                    if (text == nManagerSetting.CurrentSetting.LastProductLoaded)
                        i2 = i;
                    ProductList.Items.Add(text);
                    i++;
                }
                ProductList.DropDownStyle = ComboBoxStyle.DropDownList;
                if (i2 >= 0)
                    ProductList.SelectedIndex = i2;
                Logging.OnChanged += SynchroniseLogging;
            }
            catch (Exception ex)
            {
                Logging.WriteError("Main > InitializeUI(): " + ex);
            }
        }

        private void SynchroniseLogging(object sender, Logging.LoggingChangeEventArgs e)
        {
            try
            {
                lock (this)
                {
                    if ((e.Log.LogType & GetFlag()) == e.Log.LogType)
                    {
                        for (int i = Logging.List.Count - 1; i >= 0; i--)
                        {
                            Logging.Log log = Logging.List[i];
                            if (log.DateTime == e.Log.DateTime && log.Text == e.Log.Text && log.LogType == e.Log.LogType && log.Color == e.Log.Color)
                            {
                                log.Processed = true;
                                break;
                            }
                        }
                        _listLog.Add(e.Log);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("SynchroniseLoggin(object sender, Logging.LoggingChangeEventArgs e): " + ex);
            }
        }

        private void AddLog()
        {
            try
            {
                lock (this)
                {
                    if (_listLog.Count > 0)
                    {
                        LoggingTextArea.AppendText(_listLog[0].ToString());
                        LoggingTextArea.Select(LoggingTextArea.Text.Length - _listLog[0].ToString().Length,
                            _listLog[0].ToString().Length);
                        LoggingTextArea.SelectionColor = _listLog[0].Color;
                        LoggingTextArea.AppendText(Environment.NewLine);
                        _listLog.RemoveAt(0);
                        LoggingTextArea.ScrollToCaret();
                    }
                    else if (_hardAdded < 10)
                    {
                        if (Logging.List.Count > _listLog.Count)
                        {
                            foreach (Logging.Log log in Logging.List)
                            {
                                if ((log.LogType & GetFlag()) == log.LogType && !log.Processed)
                                {
                                    log.Processed = true;
                                    _listLog.Add(log);
                                    _hardAdded++;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("AddLog(): " + e);
            }
        }

        private Logging.LogType GetFlag()
        {
            try
            {
                var flag = Logging.LogType.None;

                if (NormalLogSwitchButton.Value)
                    flag |= Logging.LogType.S;
                if (FightLogSwitchButton.Value)
                    flag |= Logging.LogType.F;
                if (NavigationLogSwitchButton.Value)
                    flag |= Logging.LogType.N;
                if (DebugLogSwitchButton.Value)
                {
                    flag |= Logging.LogType.D;
                    flag |= Logging.LogType.E;
                }

                return flag;
            }
            catch (Exception ex)
            {
                Logging.WriteError("GetFlag(): " + ex);
            }
            return Logging.LogType.None;
        }

        private void Translate()
        {
            NormalLogSwitchLabel.Text = nManager.Translate.Get(nManager.Translate.Id.Normal);
            FightLogSwitchLabel.Text = nManager.Translate.Get(nManager.Translate.Id.Fight);
            NavigationLogSwitchLabel.Text = nManager.Translate.Get(nManager.Translate.Id.Navigator);
            DebugLogSwitchLabel.Text = nManager.Translate.Get(nManager.Translate.Id.Debug);
            HealthLabel.Text = nManager.Translate.Get(nManager.Translate.Id.Health) + " :";
            LatestLogLabel.Text = nManager.Translate.Get(nManager.Translate.Id.LatestLogEntry);
            TargetLevelLabel.Text = nManager.Translate.Get(nManager.Translate.Id.TargetLevel);
            TargetHealthLabel.Text = nManager.Translate.Get(nManager.Translate.Id.TargetHealth);
            TargetNameLabel.Text = nManager.Translate.Get(nManager.Translate.Id.TargetName);
            FarmsLabel.Text = nManager.Translate.Get(nManager.Translate.Id.Farms) + " :";
            HonorPerHourLabel.Text = nManager.Translate.Get(nManager.Translate.Id.HonorPerHour);
            LootsLabel.Text = nManager.Translate.Get(nManager.Translate.Id.Loots) + " :";
            DeathsLabel.Text = nManager.Translate.Get(nManager.Translate.Id.Deaths) + " :";
            UnitKillsLabel.Text = nManager.Translate.Get(nManager.Translate.Id.UnitKills);
            XPPerHourLabel.Text = nManager.Translate.Get(nManager.Translate.Id.XPPerHour);
            RemoteSessionLabel.Text = nManager.Translate.Get(nManager.Translate.Id.RemoteSession) + " :";
            HomeTagButton.Text = nManager.Translate.Get(nManager.Translate.Id.Home).ToUpper();
            LogTabButton.Text = nManager.Translate.Get(nManager.Translate.Id.Log).ToUpper();
            AccountTabButton.Text = nManager.Translate.Get(nManager.Translate.Id.MyAccount).ToUpper();
            MainSettingsButton.Text = nManager.Translate.Get(nManager.Translate.Id.General_Settings);
            StartButton.Text = nManager.Translate.Get(nManager.Translate.Id.StartProduct);
            ProductSettingsButton.Text = nManager.Translate.Get(nManager.Translate.Id.Product_Settings);
            DevToolsLabel.Text = nManager.Translate.Get(nManager.Translate.Id.DevTools);
            WebsiteLink.Text = nManager.Translate.Get(nManager.Translate.Id.Website);
            WantToSubscribeLabel.Text = nManager.Translate.Get(nManager.Translate.Id.WantToSubscribe);
            ProductsListPriceLabel.Text = nManager.Translate.Get(nManager.Translate.Id.SelectOffer);
            GoToPaymentPageButton.Text = nManager.Translate.Get(nManager.Translate.Id.PaymentPage);
            AccountNameLabel.Text = nManager.Translate.Get(nManager.Translate.Id.AccountName);
            TimeLeftLabel.Text = nManager.Translate.Get(nManager.Translate.Id.TimeLeft);
            ProductsPriceList.Items.Add(nManager.Translate.Get(nManager.Translate.Id.SubscribeMonth));
            ProductsPriceList.Items.Add(nManager.Translate.Get(nManager.Translate.Id.SubscribeMonthPlatinium));
            ProductsPriceList.Items.Add(nManager.Translate.Get(nManager.Translate.Id.SubscribeHalfYear));
            ProductsPriceList.Items.Add(nManager.Translate.Get(nManager.Translate.Id.SubscribeLifeTime));
        }

        private void ThreadSpellBook()
        {
            try
            {
                SpellManager.SpellBook();
                string EnUsName;
                if (string.IsNullOrEmpty(nManagerSetting.CurrentSetting.FlyingMountName))
                {
                    EnUsName = SpellManager.GetFlyMountName();
                    if (EnUsName != "")
                    {
                        Spell temp = new Spell(EnUsName);
                        nManagerSetting.CurrentSetting.FlyingMountName = SpellManager.GetSpellInfo(temp.Id).Name;
                    }
                }
                if (string.IsNullOrEmpty(nManagerSetting.CurrentSetting.GroundMountName))
                {
                    EnUsName = SpellManager.GetMountName();
                    if (EnUsName != "")
                    {
                        Spell temp = new Spell(EnUsName);
                        nManagerSetting.CurrentSetting.GroundMountName = SpellManager.GetSpellInfo(temp.Id).Name;
                    }
                }
                if (string.IsNullOrEmpty(nManagerSetting.CurrentSetting.AquaticMountName))
                {
                    EnUsName = SpellManager.GetAquaticMountName();
                    if (EnUsName != "")
                    {
                        Spell temp = new Spell(EnUsName);
                        nManagerSetting.CurrentSetting.AquaticMountName = SpellManager.GetSpellInfo(temp.Id).Name;
                    }
                }
                var items = new List<string>();
                List<WoWItem> itemsBag = Bag.GetBagItem();
                if (nManagerSetting.CurrentSetting.DontSellTheseItems.Count == 0 ||
                    nManagerSetting.CurrentSetting.DontMailTheseItems.Count == 0 ||
                    itemsBag != null && itemsBag.Count > 0)
                {
                    for (int i = 0; i < itemsBag.Count; i++)
                    {
                        WoWItem item = itemsBag[i];
                        var iteminfo = new ItemInfo(item.Entry);
                        if (iteminfo.ItemRarity > 0)
                            items.Add(item.Name);
                    }

                    Logging.Write(items.Count + " " + nManager.Translate.Get(nManager.Translate.Id.ItemsFounds));
                    if (items.Count == 0)
                    {
                        Logging.Write(nManager.Translate.Get(nManager.Translate.Id.SwitchDxError1));
                        Logging.Write(nManager.Translate.Get(nManager.Translate.Id.SwitchDxError2));
                    }
                }
                if (nManagerSetting.CurrentSetting.DontSellTheseItems.Count == 0)
                {
                    nManagerSetting.CurrentSetting.DontSellTheseItems.AddRange(items);
                }
                if (nManagerSetting.CurrentSetting.DontMailTheseItems.Count == 0)
                {
                    nManagerSetting.CurrentSetting.DontMailTheseItems.AddRange(items);
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("Main > ThreadSpellBook(): " + ex);
            }
        }

        public void SetDefaultCulture(CultureInfo culture)
        {
            Type type = typeof (CultureInfo);
            try
            {
                type.InvokeMember("s_userDefaultCulture",
                    BindingFlags.SetField | BindingFlags.NonPublic | BindingFlags.Static,
                    null,
                    culture,
                    new object[] {culture});
                type.InvokeMember("s_userDefaultUICulture",
                    BindingFlags.SetField | BindingFlags.NonPublic | BindingFlags.Static,
                    null,
                    culture,
                    new object[] {culture});
            }
            catch
            {
            }
        }

        private void HomeTagButton_Click(object sender, EventArgs e)
        {
            if (!_isHomeActive)
            {
                HomeTagButton.ForeColor = _activeColor;
                HomeTagButton.Image = _activeBackground;
                PanelHome.Visible = true;
                _isHomeActive = true;
                LogTabButton.ForeColor = _inactiveColor;
                LogTabButton.Image = _inactiveBackground;
                PanelLog.Visible = false;
                _isLogActive = false;
                AccountTabButton.ForeColor = _inactiveColor;
                AccountTabButton.Image = _inactiveBackground;
                PanelAccount.Visible = false;
                _isAccountActive = false;
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            /*
             * This function will add ALL the panels to the main control for the ease of use.
             * If you need to edit one of the hidden panels, please do the following:
             * remove the line this.Controls.Add(this.PanelHome); from the Main.Designer.cs
             * add the line this.Controls.Add(this.THE_PANEL); instead.
             * When you are done, please revert these 2 changes.
             * 
             * This is done so panels wont register themself to eachothers, and only ONE
             * panel controls will be displayed at a time in the designer.
             */
            HomeTagButton.ForeColor = _activeColor;
            //Controls.Add(PanelHome); // only if not active
            HomeTagButton.Image = Resources.tab_active_mainframe;
            PanelHome.Visible = true;
            Controls.Add(PanelLog);
            PanelLog.Visible = false;
            Controls.Add(PanelAccount);
            PanelAccount.Visible = false;
            _isHomeActive = true;
        }

        private void LogTabButton_Click(object sender, EventArgs e)
        {
            if (!_isLogActive)
            {
                LogTabButton.ForeColor = _activeColor;
                LogTabButton.Image = _activeBackground;
                PanelLog.Visible = true;
                _isLogActive = true;
                HomeTagButton.ForeColor = _inactiveColor;
                HomeTagButton.Image = _inactiveBackground;
                _isHomeActive = false;
                PanelHome.Visible = false;
                AccountTabButton.ForeColor = _inactiveColor;
                AccountTabButton.Image = _inactiveBackground;
                PanelAccount.Visible = false;
                _isAccountActive = false;
            }
        }

        private void AccountTabButton_Click(object sender, EventArgs e)
        {
            if (!_isAccountActive)
            {
                AccountTabButton.ForeColor = _activeColor;
                AccountTabButton.Image = _activeBackground;
                PanelAccount.Visible = true;
                _isAccountActive = true;
                HomeTagButton.ForeColor = _inactiveColor;
                HomeTagButton.Image = _inactiveBackground;
                PanelHome.Visible = false;
                _isHomeActive = false;
                LogTabButton.ForeColor = _inactiveColor;
                LogTabButton.Image = _inactiveBackground;
                PanelLog.Visible = false;
                _isLogActive = false;
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (Products.IsStarted)
            {
                StartButton.Enabled = false;
                Products.ProductStop();
                StartButton.Enabled = true;
            }
            else
            {
                StartButton.Enabled = false;
                Products.ProductStart();
                StartButton.Enabled = true;
            }
        }

        private void HomeTagButton_MouseEnter(object sender, EventArgs e)
        {
            HomeTagButton.Font = new Font(HomeTagButton.Font, HomeTagButton.Font.Style | FontStyle.Underline);
        }

        private void HomeTagButton_MouseLeave(object sender, EventArgs e)
        {
            HomeTagButton.Font = new Font(HomeTagButton.Font, HomeTagButton.Font.Style ^ FontStyle.Underline);
        }

        private void LogTabButton_MouseEnter(object sender, EventArgs e)
        {
            LogTabButton.Font = new Font(LogTabButton.Font, LogTabButton.Font.Style | FontStyle.Underline);
        }

        private void LogTabButton_MouseLeave(object sender, EventArgs e)
        {
            LogTabButton.Font = new Font(LogTabButton.Font, LogTabButton.Font.Style ^ FontStyle.Underline);
        }

        private void AccountTabButton_MouseEnter(object sender, EventArgs e)
        {
            AccountTabButton.Font = new Font(AccountTabButton.Font, AccountTabButton.Font.Style | FontStyle.Underline);
        }

        private void AccountTabButton_MouseLeave(object sender, EventArgs e)
        {
            AccountTabButton.Font = new Font(AccountTabButton.Font, AccountTabButton.Font.Style ^ FontStyle.Underline);
        }

        private void ProductSettingsButton_Click(object sender, EventArgs e)
        {
            ProductSettingsButton.Enabled = false;
            Products.ProductSettings();
            ProductSettingsButton.Enabled = true;
        }

        private void MainSettingsButton_Click(object sender, EventArgs e)
        {
            MainSettingsButton.Enabled = false;
            var generalSettings = new GeneralSettings();
            generalSettings.ShowDialog();
            MainSettingsButton.Enabled = true;
        }

        private void LoggingSwitchs_ValueChanged(object sender, EventArgs e)
        {
            lock (this)
            {
                _listLog.Clear();
                _listLog.AddRange(Logging.ReadList(GetFlag(), true));
                LoggingTextArea.Clear();
            }
        }

        private void RemoteSessionSwitchButton_ValueChanged(object sender, EventArgs e)
        {
            Remote.RemoteActive = RemoteSessionSwitchButton.Value;
        }

        private void DevToolsLabel_MouseEnter(object sender, EventArgs e)
        {
            DevToolsLabel.Font = new Font(DevToolsLabel.Font, DevToolsLabel.Font.Style | FontStyle.Bold);
        }

        private void DevToolsLabel_MouseLeave(object sender, EventArgs e)
        {
            DevToolsLabel.Font = new Font(DevToolsLabel.Font, DevToolsLabel.Font.Style ^ FontStyle.Bold);
        }

        private void WebsiteLink_MouseEnter(object sender, EventArgs e)
        {
            WebsiteLink.Font = new Font(WebsiteLink.Font, WebsiteLink.Font.Style | FontStyle.Bold);
        }

        private void WebsiteLink_MouseLeave(object sender, EventArgs e)
        {
            WebsiteLink.Font = new Font(WebsiteLink.Font, WebsiteLink.Font.Style ^ FontStyle.Bold);
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Pulsator.Dispose(true);
        }

        private void ProductList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string productName = ProductList.Text;
            nManagerSetting.CurrentSetting.LastProductLoaded = productName;
            nManagerSetting.CurrentSetting.Save();
            if (productName.Contains(" - "))
            {
                string[] texte2 = productName.Split('-');
                if (texte2.Length > 0)
                    productName = texte2[0].Remove(texte2[0].Length - 1);
            }
            if (productName != string.Empty)
                if (productName != Products.ProductName)
                {
                    Products.DisposeProduct();
                    Products.LoadProducts(productName);
                }
        }

        private void DeveloperToolsThread()
        {
            try
            {
                var f = new DeveloperToolsMainFrame();
                f.ShowDialog();
            }
            catch (Exception ex)
            {
                Logging.WriteError("Main > DeveloperToolsThread(): " + ex);
            }
        }

        private void DevToolsLabel_Click(object sender, EventArgs e)
        {
            var t = new Thread(DeveloperToolsThread) {Name = "DeveloperToolsForm"};
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void MainPanelTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (_playerName != ObjectManager.Me.Name && !string.IsNullOrEmpty(ObjectManager.Me.Name) &&
                    ObjectManager.Me.IsValid)
                {
                    Thread.Sleep(1000);
                    if (_playerName != ObjectManager.Me.Name && !string.IsNullOrEmpty(ObjectManager.Me.Name) &&
                        ObjectManager.Me.IsValid)
                    {
                        Logging.WriteError(nManager.Translate.Get(nManager.Translate.Id.PlayerNameChanged));
                        Pulsator.Dispose(true);
                    }
                }

                if (ObjectManager.Me.IsValid)
                {
                    if (Health.Value < ObjectManager.Me.HealthPercent || Health.Value > ObjectManager.Me.HealthPercent)
                        Health.Value = (int) ObjectManager.Me.HealthPercent;
                    toolTip.SetToolTip(Health, ObjectManager.Me.Health + "/" + ObjectManager.Me.MaxHealth);
                }
                else
                {
                    Health.Value = 0;
                    toolTip.SetToolTip(Health, "0/" + ObjectManager.Me.MaxHealth);
                }
                if (Products.IsStarted && ObjectManager.Me.IsValid)
                {
                    XPPerHour.Text = Statistics.ExperienceByHr().ToString();
                    HonorPerHour.Text = Statistics.HonorByHr().ToString();
                    LootsCount.Text = Statistics.Loots + " (" + Statistics.LootsByHr() + "/" + nManager.Translate.Get(nManager.Translate.Id.hr) + ")";
                    UnitKillsCount.Text = Statistics.Kills + " (" + Statistics.KillsByHr() + "/" + nManager.Translate.Get(nManager.Translate.Id.hr) + ")";
                    DeathsCount.Text = Statistics.Deaths + " (" + Statistics.DeathsByHr() + "/" + nManager.Translate.Get(nManager.Translate.Id.hr) + ")";
                    FarmsCount.Text = Statistics.Farms + " (" + Statistics.FarmsByHr() + "/" + nManager.Translate.Get(nManager.Translate.Id.hr) + ")";
                }
                if (ObjectManager.Target.IsValid)
                {
                    TargetName.Text = ObjectManager.Target.Name;
                    if (TargetHealth.Value < ObjectManager.Target.HealthPercent || TargetHealth.Value > ObjectManager.Target.HealthPercent)
                        TargetHealth.Value = (int) ObjectManager.Target.HealthPercent;
                    toolTip.SetToolTip(TargetHealth, ObjectManager.Target.Health + "/" + ObjectManager.Target.MaxHealth);
                    TargetLevel.Text = ObjectManager.Target.Level.ToString();
                }
                else
                {
                    TargetName.Text = @"-";
                    TargetHealth.Value = 0;
                    toolTip.SetToolTip(TargetHealth, "0/1");
                    TargetLevel.Text = @"-";
                }
                Logging.Log log = Logging.ReadLast(Logging.LogType.S);
                LatestLog.Text = log.ToString();
                LatestLog.ForeColor = log.Color;
                BotStartedSinceLabel.Text = nManager.Translate.Get(nManager.Translate.Id.tnb_started_since) + @" : " + Others.SecToHour((Others.Times - LoginServer.StartTime)/1000);
            }
            catch (Exception ex)
            {
                Logging.WriteError("Main > gameInformationTimer_Tick(object sender, EventArgs e): " + ex);
            }

            if (Display.WindowInTaskBarre(Memory.WowProcess.MainWindowHandle) &&
                Products.IsStarted)
            {
                if (!_wowInTaskBarre)
                {
                    _wowInTaskBarre = true;
                    Display.ShowWindow(Memory.WowProcess.MainWindowHandle);
                }
            }
            else
            {
                _wowInTaskBarre = false;
            }
        }

        private void MainFormTimer_Tick(object sender, EventArgs e)
        {
            if (Products.IsAliveProduct)
            {
                if (Products.IsStarted)
                {
                    StartButton.Text = nManager.Translate.Get(nManager.Translate.Id.StopProduct).ToUpper();
                    if (!StartButton.Hoovering)
                        StartButton.Image = Resources.blackB_200;
                    ProductSettingsButton.Enabled = false;
                    MainSettingsButton.Enabled = false;
                    ProductList.Enabled = false;
                }
                else
                {
                    StartButton.Text = nManager.Translate.Get(nManager.Translate.Id.StartProduct).ToUpper();
                    if (!StartButton.Hoovering)
                        StartButton.Image = Resources.blueB_200;
                    ProductSettingsButton.Enabled = true;
                    MainSettingsButton.Enabled = true;
                    ProductList.Enabled = true;
                }
            }
            else
            {
                ProductSettingsButton.Enabled = false;
                MainSettingsButton.Enabled = true;
                ProductList.Enabled = true;
            }
        }

        private void AccountPanelTimer_Tick(object sender, EventArgs e)
        {
            string botOnline = Others.GetRequest("http://tech.theprivatebot.com/auth.php", "botOnline=true");
            if (LoginServer.IsFreeVersion)
            {
                int timeLeftSec = (LoginServer.StartTime + 1000*60*20) - Others.Times;
                AccountName.Text = LoginServer.Login;
                TimeLeft.Text = Others.SecToHour(timeLeftSec/1000);
                OnlineBot.Text = botOnline;
            }
            else
            {
                string timeLeft = Others.GetReqWithAuthHeader("http://tech.theprivatebot.com/auth.php?TimeSubscription=true", LoginServer.Login, LoginServer.Password)[0];
                AccountName.Text = LoginServer.Login;
                TimeLeft.Text = timeLeft;
                OnlineBot.Text = nManager.Translate.Get(nManager.Translate.Id.OnlineBots) + " " + botOnline;
                AccountPanelTimer.Interval = 5000;
            }
            if (Remote.RemoteActive)
            {
                RemoteSessionInfo.Text = nManager.Translate.Get(nManager.Translate.Id.SessionInfo) + " " + Remote.SessionKey;
            }
            else
            {
                RemoteSessionInfo.Text = "";
            }
        }

        private void WebsiteLink_Click(object sender, EventArgs e)
        {
            Others.OpenWebBrowserOrApplication("http://theprivatebot.com/");
        }

        private void GoToPaymentPageButton_Click(object sender, EventArgs e)
        {
            Others.OpenWebBrowserOrApplication("http://theprivatebot.com/get-a-bg-bot-wow/");
        }

        private void LoggingAreaTimer_Tick(object sender, EventArgs e)
        {
            AddLog();
        }

        private void LoggingTextArea_VisibleChanged(object sender, EventArgs e)
        {
            if (LoggingTextArea.Visible)
            {
                LoggingTextArea.SelectionStart = LoggingTextArea.TextLength;
                LoggingTextArea.ScrollToCaret();
            }
        }
    }
}