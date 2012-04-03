using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Battleground_Bot.API;
using Battleground_Bot.Profile;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using WowManager;
using WowManager.Navigation;
using WowManager.Others;
using WowManager.Products;
using WowManager.UserInterfaceHelper;
using WowManager.Warden;
using WowManager.WoW;
using WowManager.WoW.Interface;
using WowManager.WoW.ObjectManager;
using WowManager.WoW.PlayerManager;
using WowManager.WoW.SpellManager;
using Useful = WowManager.WoW.Interface.Useful;

namespace Battleground_Bot
{
    public partial class MainForm : XtraForm
    {
        private readonly List<IPlugins> _pluginsList = new List<IPlugins>();
        private readonly StopBotAfter _stopBotAfterUc = new StopBotAfter(true);
        private int _startTime = -1;
        private int _baseHonorPoint = -1;
        private bool _initializeFinish;
        private bool _reloger;

        public MainForm()
        {
            InitializeComponent();
            Translate();
            xtraTabControl1.TabPages[xtraTabControl1.TabPages.IndexOf(stopBotAfterTB)].Controls.Add(_stopBotAfterUc);
                // Add stop bot after usercontrol
        }

        void Translate()
        {
            groupBox1.Text = Translation.GetText(Translation.Text.BattleGround_List_Max_2);
            simpleButton1.Text = Translation.GetText(Translation.Text.Create_or_Edit_Profile);
            editNodeList.Text = Translation.GetText(Translation.Text.Edit_Node_List);
            randomBgCB.Properties.Caption = Translation.GetText(Translation.Text.Random_BattleGround) + "*";
            groupBox2.Text = Translation.GetText(Translation.Text.Custom_Class_and_Mount);
            requeueCB.Properties.Caption = Translation.GetText(Translation.Text.ReQueue);
            lootCB.Properties.Caption = Translation.GetText(Translation.Text.Loot);
            label34.Text = Translation.GetText(Translation.Text.Slot);
            label35.Text = Translation.GetText(Translation.Text.Bar);
            useMountCB.Properties.Caption = Translation.GetText(Translation.Text.Use_Mount);
            configCCB.Text = Translation.GetText(Translation.Text.Config_CC);
            BLaunchBot.Text = Translation.GetText(Translation.Text.Start_Bot);
            logL.Text = Translation.GetText(Translation.Text.Log) + ": ";
            pathFinderCB.Properties.Caption = Translation.GetText(Translation.Text.Use_Path_Finder);
            wardenActiveL.Text = Translation.GetText(Translation.Text.Warden_not_active);
            xtraTabPage1.Text = Translation.GetText(Translation.Text.Main);
            HonorHL.Text = Translation.GetText(Translation.Text.Honor_H) + ": 0";
            resetConfigB.Text = Translation.GetText(Translation.Text.Reset_Config);
            label1.Text = "* = " + Translation.GetText(Translation.Text.Profile_not_good_for_the_moment);
            honorGainedL.Text = Translation.GetText(Translation.Text.Honor_Gained) + ": 0";
            stopBotAfterTB.Text = Translation.GetText(Translation.Text.Stop_bot_if);
            tabPage4.Text = Translation.GetText(Translation.Text.Reloger);
            label3.Text = Translation.GetText(Translation.Text.Realm_Server);
            label4.Text = Translation.GetText(Translation.Text.Player_Name);
            cbReLog.Properties.Caption = Translation.GetText(Translation.Text.Activate_Auto_Login);
            label12.Text = Translation.GetText(Translation.Text.Password);
            label13.Text = Translation.GetText(Translation.Text.Account_Name);
            xtraTabPage2.Text = Translation.GetText(Translation.Text.Plugins);
            Main.Text = Translation.GetText(Translation.Text.Main);
            enablePlugin.Text = Translation.GetText(Translation.Text.Enable_Plugin);
            disablePluginB.Text = Translation.GetText(Translation.Text.Disable_Plugin);
        }

        internal bool EnabledButtonStartBot
        {
            set { BLaunchBot.Enabled = value; }
            get { return BLaunchBot.Enabled; }
        }

        private void MainFormLoad(object sender, EventArgs e)
        {


            // Load ListPlugin
            pluginsLB.Items.Clear();
            foreach (string subfolder in Others.GetFilesDirectory("\\Products\\Battleground Bot\\Plugins", "*.dll"))
            {
                pluginDisable.Items.Add(subfolder.Replace(".dll", ""));
            }

            // Load gui
            LoadConfig();

            // Load items and spell list.
            BLaunchBot.Enabled = false;
            var searchThread = new Thread(InitializeThread) {Name = "Initialise"};
            searchThread.Start();

            // AntiAfk
            Cheat.AntiAfkPulse();


            // Get mount key
            try
            {
                if (nMountBar.Value == 1 && nMountSlot.Value == 1 && ObjectManager.Me.Level > 20)
                {
                    string barAndSlot = SpellManager.GetMountBarAndSlot();

                    if (barAndSlot != "")
                    {
                        barAndSlot = barAndSlot.Replace("{", "");
                        barAndSlot = barAndSlot.Replace("}", "");
                        barAndSlot = barAndSlot.Replace(" ", "");
                        string[] keySlot = barAndSlot.Split(Convert.ToChar(";"));

                        nMountBar.Value = Convert.ToInt32(keySlot[0]);
                        nMountSlot.Value = Convert.ToInt32(keySlot[1]);
                        useMountCB.Checked = true;
                    }
                }
            }
            catch
            {
            }
        }

        private void InitializeThread()
        {
            Log.AddLog(Translation.GetText(Translation.Text.Initialise_Thread_Spell_Book));
            SpellManager.SpellBook();
            Log.AddLog(Translation.GetText(Translation.Text.Initialise_Thread_finish));
            _initializeFinish = true;
        }

        private void MainFormFormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (IPlugins p in _pluginsList)
            {
                try
                {
                    p.Dispose();
                }
                catch
                {
                    return;
                }
            }
            Cheat.AntiAfkDispose();
            try
            {
                Fight.StopFight();
                Log.AddLog(string.Format("{0} BattleGround Bot.", Translation.GetText(Translation.Text.Closing)));
                Products.DisposeProduct();
            }
            catch
            {
                return;
            }
        }

        // Show list cc
        private void CustomClassCbClick(object sender, EventArgs e)
        {
            CustomClassCB.Properties.Items.Clear();
            foreach (string subfolder in Others.GetFilesDirectory("\\CustomClasses", "*.cs"))
            {
                CustomClassCB.Properties.Items.Add(subfolder);
            }
        }

        // Config CC
        private void ConfigCcbClick(object sender, EventArgs e)
        {
            if (Others.ExistFile(Application.StartupPath + "\\CustomClasses\\" + CustomClassCB.Text + ".dll"))
            {
                CustomClassConfig.Load(Application.StartupPath + "\\CustomClasses\\" + CustomClassCB.Text + ".dll");
            }
            else
            {
                MessageBox.Show(Translation.GetText(Translation.Text.Custom_Class_not_configurable));
            }
        }

        // CheckBox BattlerGround
        private int NbChecked()
        {
            int i = 0;
            if (alteracValleyCB.Checked)
                i++;
            if (arathiBasinCB.Checked)
                i++;
            if (eyeOfTheStormCB.Checked)
                i++;
            if (warsongGuichCB.Checked)
                i++;
            if (sotaCB.Checked)
                i++;
            if (iocCB.Checked)
                i++;
            if (tpCB.Checked)
                i++;
            if (bfgCB.Checked)
                i++;

            return i;
        }

        private void UnCheckAll()
        {
            alteracValleyCB.Checked = false;
            arathiBasinCB.Checked = false;
            eyeOfTheStormCB.Checked = false;
            warsongGuichCB.Checked = false;
            sotaCB.Checked = false;
            iocCB.Checked = false;
            tpCB.Checked = false;
            bfgCB.Checked = false;
        }

        private void EyeOfTheStormCbCheckedChanged(object sender, EventArgs e)
        {
            if (eyeOfTheStormCB.Checked)
            {
                randomBgCB.Checked = false;
                if (NbChecked() > 2)
                {
                    eyeOfTheStormCB.Checked = false;
                    MessageBox.Show(Translation.GetText(Translation.Text.Max_battleGround_is_2));
                }
            }
        }

        private void WarsongGuichCbCheckedChanged(object sender, EventArgs e)
        {
            if (warsongGuichCB.Checked)
            {
                randomBgCB.Checked = false;
                if (NbChecked() > 2)
                {
                    warsongGuichCB.Checked = false;
                    MessageBox.Show(Translation.GetText(Translation.Text.Max_battleGround_is_2));
                }
            }
        }

        private void ArathiBasinCbCheckedChanged(object sender, EventArgs e)
        {
            if (arathiBasinCB.Checked)
            {
                randomBgCB.Checked = false;
                if (NbChecked() > 2)
                {
                    arathiBasinCB.Checked = false;
                    MessageBox.Show(Translation.GetText(Translation.Text.Max_battleGround_is_2));
                }
            }
        }

        private void AlteracValleyCbCheckedChanged(object sender, EventArgs e)
        {
            if (alteracValleyCB.Checked)
            {
                randomBgCB.Checked = false;
                if (NbChecked() > 2)
                {
                    alteracValleyCB.Checked = false;
                    MessageBox.Show(Translation.GetText(Translation.Text.Max_battleGround_is_2));
                }
            }
        }

        private void SotaCbCheckedChanged(object sender, EventArgs e)
        {
            if (sotaCB.Checked)
            {
                randomBgCB.Checked = false;
                if (NbChecked() > 2)
                {
                    sotaCB.Checked = false;
                    MessageBox.Show(Translation.GetText(Translation.Text.Max_battleGround_is_2));
                }
            }
        }

        private void IocCbCheckedChanged(object sender, EventArgs e)
        {
            if (iocCB.Checked)
            {
                randomBgCB.Checked = false;
                if (NbChecked() > 2)
                {
                    iocCB.Checked = false;
                    MessageBox.Show(Translation.GetText(Translation.Text.Max_battleGround_is_2));
                }
            }
        }

        private void BfgCbCheckedChanged(object sender, EventArgs e)
        {
            if (bfgCB.Checked)
            {
                randomBgCB.Checked = false;
                if (NbChecked() > 2)
                {
                    bfgCB.Checked = false;
                    MessageBox.Show(Translation.GetText(Translation.Text.Max_battleGround_is_2));
                }
            }
        }

        private void TpCbCheckedChanged(object sender, EventArgs e)
        {
            if (tpCB.Checked)
            {
                randomBgCB.Checked = false;
                if (NbChecked() > 2)
                {
                    tpCB.Checked = false;
                    MessageBox.Show(Translation.GetText(Translation.Text.Max_battleGround_is_2));
                }
            }
        }

        private void RandomBgCbCheckedChanged(object sender, EventArgs e)
        {
            if (randomBgCB.Checked)
                UnCheckAll();
        }

        // Others

        private void LogTick(object sender, EventArgs e)
        {
            try
            {
                posL.Text = Translation.GetText(Translation.Text.Pos) + ": " + ObjectManager.Me.Position;

                if (_baseHonorPoint < 0)
                    _baseHonorPoint = Useful.GetHonorPoint;
                honorGainedL.Text = Translation.GetText(Translation.Text.Honor_Gained) + ": " + (Useful.GetHonorPoint - _baseHonorPoint);

                if (_initializeFinish && Config.Bot.BotStoped)
                {
                    BLaunchBot.Enabled = true;
                    if (WowManager.WowManager.AutoLaunch)
                    {
                        BLaunchBotClick(null, null);
                        WowManager.WowManager.AutoLaunch = false;
                    }
                }

                logL.Text = Translation.GetText(Translation.Text.Log) + ": " + Log.GetLastLog;

                if (Warden.IsActive)
                {
                    wardenActiveL.ForeColor = Color.Green;
                    wardenActiveL.Text = Translation.GetText(Translation.Text.AntiWarden_Activated);
                }
                else
                {
                    wardenActiveL.ForeColor = Color.Green;
                    wardenActiveL.Text = Translation.GetText(Translation.Text.Warden_not_active);
                }

                if (!Config.Bot.BotStarted && _stopBotAfterUc.IsActive)
                    _stopBotAfterUc.Stop();

                if (Config.Bot.BotStarted)
                    HonorHL.Text = Translation.GetText(Translation.Text.Honor_H) + ": " +
                                   (3600*(Useful.GetHonorPoint - _baseHonorPoint)/((Others.Times - _startTime)/1000));
            }
            catch
            {
            }
        }

        // Launch bot
        private void BLaunchBotClick(object sender, EventArgs e)
        {
            LaunchBot();
        }

        public void LaunchBot(bool byApi = false)
        {
            SaveConfig();
            bool t = false;
            if (Config.Bot != null)
            {
                if (Config.Bot.BotStarted)
                {
                    _stopBotAfterUc.Stop();
                    Config.Bot.BotStarted = false;
                    t = true;
                    BLaunchBot.Enabled = false;
                    MovementManager.Stop();
                    MovementManager.StopMove();
                    Fight.StopFight();
                    BLaunchBot.Text = Translation.GetText(Translation.Text.Launch_Bot);
                }
            }
            if (!t)
                if (NbChecked() > 0 || randomBgCB.Checked)
                {
                    _stopBotAfterUc.Pulse();
                    Config.Bot = new Bot.Bot(byApi, randomBgCB.Checked, warsongGuichCB.Checked, eyeOfTheStormCB.Checked,
                                             arathiBasinCB.Checked, alteracValleyCB.Checked, sotaCB.Checked,
                                             iocCB.Checked, tpCB.Checked, bfgCB.Checked, CustomClassCB.Text,
                                             pathFinderCB.Checked, useMountCB.Checked,
                                             nMountBar.Value + ";" + nMountSlot.Value, lootCB.Checked, requeueCB.Checked);
                    BLaunchBot.Text = Translation.GetText(Translation.Text.Stop_Bot);
                    Config.Bot.InisializeBotConfig();
                    _startTime = Others.Times;
                }
                else
                    MessageBox.Show(Translation.GetText(Translation.Text.Please_select_BattlerGround));
        }

        // Reloger

        private void RelogerTick(object sender, EventArgs e)
        {
            if (cbReLog.Checked)
            {
                if (tbAccountName.Text != "" && tbPassword.Text != "" && !Useful.InGame)
                {
                    if (!_reloger)
                        Log.AddLog(Translation.GetText(Translation.Text.Relog_Player));
                    var s = new Login.SettingsLogin
                                {
                                    Username = tbAccountName.Text,
                                    Password = tbPassword.Text,
                                    Realm = tbRealm.Text,
                                    Character = numberPlayerNUD.Text
                                };

                    Login.Pulse(s);
                    _reloger = true;
                }
                if (_reloger && Useful.InGame && !Useful.isLoadingOrConnecting)
                {
                    Thread.Sleep(4000);
                    if (Useful.InGame)
                    {
                        Log.AddLog(Translation.GetText(Translation.Text.Relog_player_finish_restart_bot));
                        _reloger = false;
                        ConfigWowForWowManager.ConfigWow();
                        if (!Config.Bot.BotStarted)
                        {
                            LaunchBot();
                        }
                    }
                }
            }
        }

        // Create Profile
        private void SimpleButton1Click(object sender, EventArgs e)
        {
            var form = new ProfileManager();
            form.Show();
        }

        // Config Gui
        public void LoadConfig()
        {
            GuiConfig t = GuiConfig.Load();

            pluginsLB.Items.Clear();
            foreach (string v in t.EnablePlugins)
            {
                if (Others.ExistFile(Application.StartupPath + "\\Products\\Battleground Bot\\Plugins\\" + v + ".dll"))
                {
                    EnablePlugin(v);
                    pluginsLB.Items.Add(v);
                    ClearListPlugin();
                }
            }

            randomBgCB.Checked = t.RandomBg;
            warsongGuichCB.Checked = t.WarsongGuich;
            eyeOfTheStormCB.Checked = t.EyeOfTheStorm;
            arathiBasinCB.Checked = t.ArathiBasin;
            alteracValleyCB.Checked = t.AlteracValley;
            sotaCB.Checked = t.Sota;
            iocCB.Checked = t.Ioc;
            tpCB.Checked = t.Tp;
            bfgCB.Checked = t.Bfg;
            CustomClassCB.Text = t.CustomClass;
            pathFinderCB.Checked = t.PathFinder;
            useMountCB.Checked = t.UseMount;
            nMountSlot.Value = t.MountSlot;
            nMountBar.Value = t.MountBar;
            lootCB.Checked = t.Loot;
            requeueCB.Checked = t.Requeue;

            try
            {
                if (t.Reloger != "")
                {
                    string fileLog = t.Reloger;
                    string[] fileLogArray = fileLog.Split(Convert.ToChar("#"));
                    tbAccountName.Text = Others.EncryptStringToString(fileLogArray[0]);
                    tbPassword.Text = Others.EncryptStringToString(fileLogArray[1]);
                    numberPlayerNUD.Text = Others.EncryptStringToString(fileLogArray[2]);
                    tbRealm.Text = Others.EncryptStringToString(fileLogArray[3]);
                }
                else
                {
                    tbAccountName.Text = "";
                    tbPassword.Text = "";
                    tbRealm.Text = Useful.RealmName;
                    numberPlayerNUD.Text = ObjectManager.Me.Name;
                }
            }
            catch
            {
            }
        }

        public GuiConfig SaveConfig()
        {
            string relog = "";
            try
            {
                if (tbAccountName.Text != "" && tbPassword.Text != "")
                    relog = Others.StringToEncryptString(tbAccountName.Text) + "#" +
                            Others.StringToEncryptString(tbPassword.Text) + "#" +
                            Others.StringToEncryptString(numberPlayerNUD.Text) + "#" +
                            Others.StringToEncryptString(tbRealm.Text);
                else
                    relog = "";
            }
            catch
            {
            }

            var enablePlugins = (from object v in pluginsLB.Items
                                 where Others.ExistFile(Application.StartupPath + "\\Products\\Battleground Bot\\Plugins\\" + v + ".dll")
                                 select v.ToString()).ToList();

            var t = new GuiConfig(randomBgCB.Checked, warsongGuichCB.Checked, eyeOfTheStormCB.Checked,
                                  arathiBasinCB.Checked, alteracValleyCB.Checked, sotaCB.Checked, iocCB.Checked,
                                  tpCB.Checked, bfgCB.Checked, CustomClassCB.Text, pathFinderCB.Checked,
                                  useMountCB.Checked, (int) nMountBar.Value, (int) nMountSlot.Value, relog,
                                  lootCB.Checked, requeueCB.Checked, enablePlugins);

            t.Save();

            return t;
        }

        private void ResetConfigBClick(object sender, EventArgs e)
        {
            GuiConfig.Reset();
            LoadConfig();
        }

        private void editNodeList_Click(object sender, EventArgs e)
        {
            var f = new NodeList();
            f.Show();
        }

        // PLUGIN
        // Enable AddOn Buttom
        private void EnablePluginClick(object sender, EventArgs e)
        {
            if (pluginDisable.SelectedIndex >= 0)
            {
                EnablePlugin(pluginDisable.Items[pluginDisable.SelectedIndex].ToString());
                pluginsLB.Items.Add(pluginDisable.Items[pluginDisable.SelectedIndex]);
                pluginDisable.Items.RemoveAt(pluginDisable.SelectedIndex);
            }
        }

        // Enable addon
        private void EnablePlugin(string name)
        {
            try
            {
                if (name.Replace(" ", "") != string.Empty && !pluginsLB.Items.Contains(name))
                {
                    IPlugins t = Plugins.LoadPlugin(name);
                    if (t != null)
                    {
                        _pluginsList.Add(t);
                        _pluginsList[_pluginsList.Count - 1].Initialize();
                        if (_pluginsList[_pluginsList.Count - 1].UserControl != null)
                        {
                            var xtraTabPageTemps = new XtraTabPage {Text = name};
                            _pluginsList[_pluginsList.Count - 1].Name = name;
                            xtraTabPageTemps.Controls.Add(_pluginsList[_pluginsList.Count - 1].UserControl);
                            xtraTabPageTemps.AutoScroll = true;
                            xtraTabControl1.TabPages.Add(xtraTabPageTemps);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Exception: " + e);
            }
        }

        private void DisablePlugin(string name)
        {
            try
            {
                bool activated = false;
                int i = 0;
                foreach (IPlugins plugin in _pluginsList)
                {
                    if (plugin.Name == name)
                    {
                        activated = true;
                        break;
                    }
                    i++;
                }

                if (activated)
                {
                    try
                    {
                        _pluginsList[i].Dispose();
                        _pluginsList.RemoveAt(i);
                    }
                    catch
                    {
                    }
                    for (int j = 0; j <= xtraTabControl1.TabPages.Count - 1; j++)
                    {
                        if (xtraTabControl1.TabPages[j].Text == name)
                        {
                            xtraTabControl1.TabPages.RemoveAt(j);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void ClearListPlugin()
        {
            for (int i = 0; i <= pluginDisable.Items.Count - 1; i++)
            {
                if (
                    Others.ExistFile(Application.StartupPath + "\\Products\\Battleground Bot\\Plugins\\" +
                                     pluginDisable.Items[i] + ".dll"))
                    if (pluginsLB.Items.Contains(pluginDisable.Items[i]))
                        pluginDisable.Items.RemoveAt(i);
            }
        }

        private void DisablePluginBClick(object sender, EventArgs e)
        {
            if (pluginsLB.SelectedIndex >= 0)
            {
                DisablePlugin(pluginsLB.Items[pluginsLB.SelectedIndex].ToString());
                pluginDisable.Items.Add(pluginsLB.Items[pluginsLB.SelectedIndex]);
                pluginsLB.Items.RemoveAt(pluginsLB.SelectedIndex);
            }
        }
    }
}