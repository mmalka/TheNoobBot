using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Helpful.Forms;
using nManager.Helpful.Forms.UserControls;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

namespace The_Noob_Bot
{
    internal partial class Main : DevComponents.DotNetBar.Metro.MetroAppForm
    {
        private readonly MainMinimized _minimizedWindow;
        public static string MinimizesWindowToolTip = "";
        public static string MinimizesWindowBoutonText = "";
        public static bool MinimizesWindowBoutonActive;
        public static Image MinimizesWindowBoutonImage;

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

        public Main()
        {
            try
            {
                InitializeBot();
                InitializeComponent();
                Translate();
                if (nManager.nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                    TopMost = true;
                InitializeInterface();
                InitializeUI();
                _minimizedWindow = new MainMinimized();
                _minimizedWindow.VisibleChanged += MinimizedVisivleChange;
                Logging.Status = "Startup Complete";
            }
            catch (Exception ex)
            {
                Logging.WriteError("Main > Main(): " + ex);
            }
        }

        private void InitializeInterface()
        {
            try
            {
                // Logging tab:
                LoggingUC loggingUc = new LoggingUC {Size = new Size(tLogging.Size.Width, tLogging.Size.Height - 0)};
                tLogging.Controls.Add(loggingUc);
            }
            catch (Exception ex)
            {
                Logging.WriteError("Main > InitializeInterface(): " + ex);
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

                Thread spellBook = new Thread(ThreadSpellBook) {Name = "SpellBook Loading"};
                spellBook.Start();
                MovementManager.LaunchThreadMovementManager();

                _playerName = ObjectManager.Me.Name;
            }
            catch (Exception ex)
            {
                Logging.WriteError("Main > InitializeBot(): " + ex);
            }
        }

        private void ThreadSpellBook()
        {
            try
            {
                SpellManager.SpellBook();

                if (string.IsNullOrEmpty(nManager.nManagerSetting.CurrentSetting.FlyingMountName))
                {
                    nManager.nManagerSetting.CurrentSetting.FlyingMountName = SpellManager.GetFlyMountName();
                }
                if (string.IsNullOrEmpty(nManager.nManagerSetting.CurrentSetting.GroundMountName))
                {
                    nManager.nManagerSetting.CurrentSetting.GroundMountName = SpellManager.GetMountName();
                }
                if (string.IsNullOrEmpty(nManager.nManagerSetting.CurrentSetting.AquaticMountName))
                {
                    nManager.nManagerSetting.CurrentSetting.AquaticMountName = SpellManager.GetAquaticMountName();
                }
                List<string> items = new List<string>();
                if (nManager.nManagerSetting.CurrentSetting.DontSellTheseItems.Count == 0 ||
                    nManager.nManagerSetting.CurrentSetting.DontMailTheseItems.Count == 0)
                {
                    for (int i = 0; i < Bag.GetBagItem().Count; i++)
                    {
                        WoWItem item = Bag.GetBagItem()[i];
                        var iteminfo = new nManager.Wow.Class.ItemInfo(item.Entry);
                        if (iteminfo.ItemRarity > 0)
                            items.Add(item.Name);
                    }

                    Logging.Write(items.Count + " items found.");
                    if (items.Count == 0)
                    {
                        Logging.Write(
                            "Possible issue while botting detected, please try to switch DirectX version of your WoW client.");
                        Logging.Write(
                            "Ignore this message if you really have ZERO items in your World of Warcraft bags.");
                    }
                }
                if (nManager.nManagerSetting.CurrentSetting.DontSellTheseItems.Count == 0)
                {
                    nManager.nManagerSetting.CurrentSetting.DontSellTheseItems.AddRange(items);
                }
                if (nManager.nManagerSetting.CurrentSetting.DontMailTheseItems.Count == 0)
                {
                    nManager.nManagerSetting.CurrentSetting.DontMailTheseItems.AddRange(items);
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("Main > ThreadSpellBook(): " + ex);
            }
        }

        private void InitializeUI()
        {
            try
            {
                GetSubcriptionInfo();
                Text = "The Noob Bot - " + nManager.Information.Version;
                if (LoginServer.IsFreeVersion)
                    Text += " - Trial version";
                playerNameB.Text = ObjectManager.Me.Name;


                // Products:
                foreach (string f in Others.GetFilesDirectory(Application.StartupPath + "\\Products\\", "*.dll"))
                {
                    string text = f.Replace(".dll", "");
                    nManager.Translate.Id ret;
                    if (Enum.TryParse(text.Replace(" ", "_") + "_Product_Description", true, out ret))
                    {
                        if (!string.IsNullOrEmpty(nManager.Translate.Get(ret)))
                            text = text + " - " + nManager.Translate.Get(ret);
                    }

                    listProductsCb.Items.Add(text);
                }
                listProductsCb.DropDownStyle = ComboBoxStyle.DropDownList;
                Logging.OnChangedStatus += SynchroniseStatus;
            }
            catch (Exception ex)
            {
                Logging.WriteError("Main > InitializeUI(): " + ex);
            }
        }

        private void SetToolTypeIfNeeded(Control label)
        {
            using (Graphics g = CreateGraphics())
            {
                SizeF size = g.MeasureString(label.Text, label.Font);
                if (size.Width > label.Width)
                {
                    labelsToolTip.SetToolTip(label, label.Text);
                }
            }
        }

        private void Translate()
        {
            metroShell1.HelpButtonText = nManager.Translate.Get(nManager.Translate.Id.WEBSITE);
            expandablePanel1.TitleText = "  " + nManager.Translate.Get(nManager.Translate.Id.Game_Informations);
            devToolsB.Text = nManager.Translate.Get(nManager.Translate.Id.Dev_Tools);
            targetName.Text = nManager.Translate.Get(nManager.Translate.Id.Target_Name);
            SetToolTypeIfNeeded(targetName);
            lastLogL.Text = nManager.Translate.Get(nManager.Translate.Id.Last_log);
            SetToolTypeIfNeeded(lastLogL);
            labelX13.Text = nManager.Translate.Get(nManager.Translate.Id.Last_log) + ":";
            SetToolTypeIfNeeded(labelX13);
            targetLevelL.Text = nManager.Translate.Get(nManager.Translate.Id.Target_Level);
            SetToolTypeIfNeeded(targetLevelL);
            labelX11.Text = nManager.Translate.Get(nManager.Translate.Id.Target_Health);
            SetToolTypeIfNeeded(labelX11);
            farmsL.Text = nManager.Translate.Get(nManager.Translate.Id.Farms) + ": 0 (0/" +
                          nManager.Translate.Get(nManager.Translate.Id.hr) + ")";
            SetToolTypeIfNeeded(farmsL);
            honorHrL.Text = nManager.Translate.Get(nManager.Translate.Id.Honor_HR) + ": 0";
            SetToolTypeIfNeeded(honorHrL);
            lootL.Text = nManager.Translate.Get(nManager.Translate.Id.Loots) + ": 0 (0/" +
                         nManager.Translate.Get(nManager.Translate.Id.hr) + ")";
            SetToolTypeIfNeeded(lootL);
            deathsL.Text = nManager.Translate.Get(nManager.Translate.Id.Deaths) + ": 0 (0/" +
                           nManager.Translate.Get(nManager.Translate.Id.hr) + ")";
            SetToolTypeIfNeeded(deathsL);
            killsL.Text = nManager.Translate.Get(nManager.Translate.Id.Kills) + ": 0 (0/" +
                          nManager.Translate.Get(nManager.Translate.Id.hr) + ")";
            SetToolTypeIfNeeded(killsL);
            xpHrL.Text = nManager.Translate.Get(nManager.Translate.Id.XP_HR) + ": 0";
            SetToolTypeIfNeeded(xpHrL);
            levelL.Text = nManager.Translate.Get(nManager.Translate.Id.Level);
            SetToolTypeIfNeeded(levelL);
            labelX3.Text = nManager.Translate.Get(nManager.Translate.Id.Health);
            SetToolTypeIfNeeded(labelX3);
            expandablePanel2.TitleText = nManager.Translate.Get(nManager.Translate.Id.Account_Informations);
            SetToolTypeIfNeeded(expandablePanel2);
            accountInfoL.Text = nManager.Translate.Get(nManager.Translate.Id.Information_account);
            SetToolTypeIfNeeded(accountInfoL);
            labelX1.Text = nManager.Translate.Get(nManager.Translate.Id.Remote) + ":";
            SetToolTypeIfNeeded(labelX1);
            tHome.Text = "&" + nManager.Translate.Get(nManager.Translate.Id.Home);

            trer.Text = "&" + nManager.Translate.Get(nManager.Translate.Id.Log);

            playerNameB.Text = nManager.Translate.Get(nManager.Translate.Id.Player_Name);

            labelX2.Text = nManager.Translate.Get(nManager.Translate.Id.Product);
            SetToolTypeIfNeeded(labelX2);
            settingsB.Text = nManager.Translate.Get(nManager.Translate.Id.General_Settings);
            SetToolTypeIfNeeded(settingsB);
            startB.Text = nManager.Translate.Get(nManager.Translate.Id.Start);
            SetToolTypeIfNeeded(startB);
            productSettingsB.Text = nManager.Translate.Get(nManager.Translate.Id.Product_Settings);
            SetToolTypeIfNeeded(productSettingsB);
            buttonX1.Text = nManager.Translate.Get(nManager.Translate.Id.Minimise);
            SetToolTypeIfNeeded(buttonX1);
            metroTabItem2.Text = "&" + nManager.Translate.Get(nManager.Translate.Id.My_tnb_Account);
        }

        private string _playerName = "";

        private string _status = "";

        private void SynchroniseStatus(object sender, Logging.StatusChangeEventArgs e)
        {
            try
            {
                _status = e.Status;
            }
            catch (Exception ex)
            {
                Logging.WriteError("Main > SynchroniseStatus(object sender, Logging.StatusChangeEventArgs e): " + ex);
            }
        }

        private void GetSubcriptionInfo()
        {
            try
            {
                Thread worker = new Thread(GetSubcriptionInfoThread) {Name = "Get Subcription Info"};
                worker.Start();
            }
            catch (Exception ex)
            {
                Logging.WriteError("Main > GetSubcriptionInfo(): " + ex);
            }
        }

        private string _subscriptionInfo;

        private void GetSubcriptionInfoThread()
        {
            try
            {
                string botOnline = Others.GetRequest("http://tech.thenoobbot.com/auth.php", "botOnline=true");
                if (LoginServer.IsFreeVersion)
                {
                    while (LoginServer.IsFreeVersion)
                    {
                        int timeLeftSec = (LoginServer.StartTime + 1000*60*20) - Others.Times;
                        string timeLeft = Others.SecToHour(timeLeftSec/1000);
                        _subscriptionInfo = nManager.Translate.Get(nManager.Translate.Id.UserName) + ": " +
                                            LoginServer.Login + " " + Environment.NewLine + Environment.NewLine +
                                            nManager.Translate.Get(nManager.Translate.Id.Trial_version__time_left) +
                                            ": " + timeLeft + " " + Environment.NewLine + Environment.NewLine +
                                            nManager.Translate.Get(nManager.Translate.Id.Tnb_online) + ": " + botOnline;
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    string timeLeft =
                        Others.GetReqWithAuthHeader("http://tech.thenoobbot.com/auth.php?TimeSubscription=true",
                                                    LoginServer.Login, LoginServer.Password)[0];
                    _subscriptionInfo = nManager.Translate.Get(nManager.Translate.Id.UserName) + ": " +
                                        LoginServer.Login + " " + Environment.NewLine + Environment.NewLine +
                                        "Subscription time left: " + timeLeft + " " + Environment.NewLine +
                                        Environment.NewLine + "Bot online: " + botOnline;
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("Main > GetSubcriptionInfoThread(): " + ex);
            }
        }

        private void myBotAccountTabTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                accountInfoL.Text = _subscriptionInfo;
            }
            catch (Exception ex)
            {
                Logging.WriteError("Main > myBotAccountTabTimer_Tick(object sender, EventArgs e): " + ex);
            }
        }

        private void metroShell1_Click(object sender, EventArgs e)
        {
            try
            {
                Others.OpenWebBrowserOrApplication("http://thenoobbot.com/");
            }
            catch (Exception ex)
            {
                Logging.WriteError("Main > metroShell1_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void remoteCb_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                Remote.RemoteActive = remoteCb.Value;
            }
            catch (Exception ex)
            {
                Logging.WriteError("Main > remoteCb_ValueChanged(object sender, EventArgs e): " + ex);
            }
        }

        private void settingsB_Click(object sender, EventArgs e)
        {
            try
            {
                GeneralSettings generalSettings = new GeneralSettings();
                generalSettings.ShowDialog();
            }
            catch (Exception ex)
            {
                Logging.WriteError("Main > settingsB_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                nManager.Pulsator.Dispose(true);
            }
            catch (Exception ex)
            {
                Logging.WriteError("Main > Main_FormClosed(object sender, FormClosedEventArgs e): " + ex);
            }
        }

        private void listProductsCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string productName = listProductsCb.Text;
                if (productName.Contains(" - "))
                {
                    string[] texte2 = productName.Split('-');
                    if (texte2.Length > 0)
                        productName = texte2[0].Remove(texte2[0].Length - 1);
                }
                if (productName != string.Empty)
                    if (productName != nManager.Products.Products.ProductName)
                    {
                        nManager.Products.Products.DisposeProduct();
                        nManager.Products.Products.LoadProducts(productName);
                    }
            }
            catch (Exception ex)
            {
                Logging.WriteError("Main > listProductsCb_SelectedIndexChanged(object sender, EventArgs e): " + ex);
            }
        }

        private void startB_Click(object sender, EventArgs e)
        {
            try
            {
                if (nManager.Products.Products.IsStarted)
                {
                    startB.Enabled = false;
                    nManager.Products.Products.ProductStop();
                    startB.Enabled = true;
                }
                else
                {
                    startB.Enabled = false;
                    nManager.Products.Products.ProductStart();
                    startB.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("Main > startB_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void productSettingsB_Click(object sender, EventArgs e)
        {
            try
            {
                nManager.Products.Products.ProductSettings();
            }
            catch (Exception ex)
            {
                Logging.WriteError("Main > productSettingsB_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void buttonProductManager_Tick(object sender, EventArgs e)
        {
            try
            {
                if (nManager.Products.Products.IsAliveProduct)
                {
                    startB.Enabled = true;
                    if (nManager.Products.Products.IsStarted)
                    {
                        startB.Text = nManager.Translate.Get(nManager.Translate.Id.Stop);
                        startB.Image = Properties.Resources.Stop;
                        productSettingsB.Enabled = false;
                        settingsB.Enabled = false;
                        listProductsCb.Enabled = false;
                    }
                    else
                    {
                        startB.Text = nManager.Translate.Get(nManager.Translate.Id.Start);
                        startB.Image = Properties.Resources.Play;
                        productSettingsB.Enabled = true;
                        settingsB.Enabled = true;
                        listProductsCb.Enabled = true;
                    }
                }
                else
                {
                    startB.Enabled = false;
                    productSettingsB.Enabled = false;
                    settingsB.Enabled = true;
                    listProductsCb.Enabled = true;
                }

                MinimizesWindowBoutonText = startB.Text;
                MinimizesWindowBoutonActive = startB.Enabled;
                MinimizesWindowBoutonImage = startB.Image;
            }
            catch (Exception ex)
            {
                Logging.WriteError("Main > buttonProductManager_Tick(object sender, EventArgs e): " + ex);
            }
        }

        private bool _wowInTaskBarre;

        private void gameInformationTimer_Tick(object sender, EventArgs e)
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
                        Logging.WriteError("Character changed, closing bot.");
                        nManager.Pulsator.Dispose(true);
                    }
                }

                if (ObjectManager.Me.IsValid)
                {
                    healthB.Value = (int) ObjectManager.Me.HealthPercent;
                    healthL.Text = ObjectManager.Me.Health + "/" +
                                   ObjectManager.Me.MaxHealth;
                    levelL.Text = nManager.Translate.Get(nManager.Translate.Id.Level) + " " + ObjectManager.Me.Level;
                    statusBarText.Text = _status;
                }
                else
                {
                    healthB.Value = 0;
                    healthL.Text = "-/-";
                }
                if (nManager.Products.Products.IsStarted && ObjectManager.Me.IsValid)
                {
                    xpHrL.Text = nManager.Translate.Get(nManager.Translate.Id.XP_HR) + ": " +
                                 nManager.Statistics.ExperienceByHr();
                    honorHrL.Text = nManager.Translate.Get(nManager.Translate.Id.Honor_HR) + ": " +
                                    nManager.Statistics.HonorByHr();
                    lootL.Text = nManager.Translate.Get(nManager.Translate.Id.Loots) + ": " + nManager.Statistics.Loots +
                                 " (" + nManager.Statistics.LootsByHr() + "/" +
                                 nManager.Translate.Get(nManager.Translate.Id.hr) + ")";
                    killsL.Text = nManager.Translate.Get(nManager.Translate.Id.Kills) + ": " + nManager.Statistics.Kills +
                                  " (" + nManager.Statistics.KillsByHr() + "/" +
                                  nManager.Translate.Get(nManager.Translate.Id.hr) + ")";
                    deathsL.Text = nManager.Translate.Get(nManager.Translate.Id.Deaths) + ": " +
                                   nManager.Statistics.Deaths + " (" + nManager.Statistics.DeathsByHr() + "/" +
                                   nManager.Translate.Get(nManager.Translate.Id.hr) + ")";
                    farmsL.Text = nManager.Translate.Get(nManager.Translate.Id.Farms) + ": " + nManager.Statistics.Farms +
                                  " (" + nManager.Statistics.FarmsByHr() + "/" +
                                  nManager.Translate.Get(nManager.Translate.Id.hr) + ")";
                }
                if (nManager.Products.Products.IsStarted)
                    expandablePanel1.TitleText = "  " + nManager.Translate.Get(nManager.Translate.Id.Game_Informations) +
                                                 " - " + nManager.Translate.Get(nManager.Translate.Id.Running_time) +
                                                 ": " + Others.SecToHour(nManager.Statistics.RunningTimeInSec());

                if (ObjectManager.Target.IsValid)
                {
                    targetName.Text = nManager.Translate.Get(nManager.Translate.Id.Target_Name) + ": " +
                                      ObjectManager.Target.Name;
                    targetHealthB.Value = (int) ObjectManager.Target.HealthPercent;
                    targetHealthL.Text = ObjectManager.Target.Health + "/" + ObjectManager.Target.MaxHealth;
                    targetLevelL.Text = nManager.Translate.Get(nManager.Translate.Id.Level) + " " +
                                        ObjectManager.Target.Level;
                }
                else
                {
                    targetName.Text = nManager.Translate.Get(nManager.Translate.Id.Target_Name) + ": -";
                    targetHealthB.Value = 0;
                    targetHealthL.Text = "-/-";
                    targetLevelL.Text = nManager.Translate.Get(nManager.Translate.Id.Level) + " -";
                }
                Logging.Log log = Logging.ReadLast(Logging.LogType.S);
                lastLogL.Text = log.ToString();
                lastLogL.ForeColor = log.Color;

                MinimizesWindowToolTip = "";
                if (nManager.Products.Products.IsStarted)
                    MinimizesWindowToolTip = " " + nManager.Translate.Get(nManager.Translate.Id.Running_time) + ": " +
                                             Others.SecToHour(nManager.Statistics.RunningTimeInSec()) + " " +
                                             Environment.NewLine;

                MinimizesWindowToolTip = MinimizesWindowToolTip + " " +
                                         nManager.Translate.Get(nManager.Translate.Id.Health) + " " + healthB.Value +
                                         "% " + Environment.NewLine + " " + levelL.Text + " " + Environment.NewLine +
                                         " " + xpHrL.Text + " " + Environment.NewLine + " " +
                                         honorHrL.Text + " " + Environment.NewLine + " " + lootL.Text + " " +
                                         Environment.NewLine + " " + killsL.Text + " " + Environment.NewLine + " " +
                                         deathsL.Text +
                                         " " + Environment.NewLine + " " + farmsL.Text + " " + Environment.NewLine + " " +
                                         nManager.Translate.Get(nManager.Translate.Id.Last_log) + ": " + lastLogL.Text;
            }
            catch (Exception ex)
            {
                Logging.WriteError("Main > gameInformationTimer_Tick(object sender, EventArgs e): " + ex);
            }

            if (Display.WindowInTaskBarre(nManager.Wow.Memory.WowProcess.MainWindowHandle) &&
                nManager.Products.Products.IsStarted)
            {
                if (!_wowInTaskBarre)
                {
                    _wowInTaskBarre = true;
                    Display.ShowWindow(nManager.Wow.Memory.WowProcess.MainWindowHandle);
                    /*MessageBox.Show(
                        nManager.Translate.Get(
                            nManager.Translate.Id.
                                The_game_is_in_your_taskbar__this_program_don_t_works_if_you_restore_The_Game_window) +
                        ".", nManager.Translate.Get(nManager.Translate.Id.Information), MessageBoxButtons.OK,
                        MessageBoxIcon.Information);*/
                }
            }
            else
            {
                _wowInTaskBarre = false;
            }
        }

        private void devToolsB_Click(object sender, EventArgs e)
        {
            try
            {
                Thread t = new Thread(DevToolsThread) {Name = "DevTools Form"};
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
            }
            catch (Exception ex)
            {
                Logging.WriteError("Main > devToolsB_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void DevToolsThread()
        {
            try
            {
                DeveloperTools f = new DeveloperTools();
                f.ShowDialog();
            }
            catch (Exception ex)
            {
                Logging.WriteError("Main > DevToolsThread(): " + ex);
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            _minimizedWindow.Show();
            Hide();
        }

        private void MinimizedVisivleChange(object sender, EventArgs e)
        {
            try
            {
                if (!_minimizedWindow.Visible)
                    Show();
            }
            catch (Exception ex)
            {
                Logging.WriteError("Main > MinimizedVisivleChange(object sender, EventArgs e): " + ex);
            }
        }
    }
}