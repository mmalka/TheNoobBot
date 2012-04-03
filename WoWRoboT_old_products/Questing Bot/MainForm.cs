using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Questing_Bot.Bot;
using WowManager;
using WowManager.Others;
using WowManager.Products;
using WowManager.UserInterfaceHelper;
using WowManager.Warden;
using WowManager.WoW.Interface;
using WowManager.WoW.ObjectManager;
using WowManager.WoW.SpellManager;

namespace Questing_Bot
{
    public partial class MainForm : XtraForm
    {
        private readonly StopBotAfter _stopBotAfterUc = new StopBotAfter();
        private bool _initializeFinish;
        private string _lastLog = string.Empty;
        private bool _loopRecordPoint;

        public MainForm()
        {
            InitializeComponent();
        }

        //Close
        private void MainFormFormClosing(object sender, FormClosingEventArgs e)
        {
            Products.DisposeProduct();
        }

        // Load
        private void MainFormLoad(object sender, EventArgs e)
        {
            // Initialise SpeelBook
            BLaunchBot.Enabled = false;
            BLaunchBot.Text = Translation.GetText(Translation.Text.Please_Wait);
            var searchThread = new Thread(InitializeThread) { Name = "Initialise" };
            searchThread.Start();

            // Stop bot if:
            xtraTabControl1.TabPages[xtraTabControl1.TabPages.IndexOf(stopBotAfterTB)].Controls.Add(_stopBotAfterUc);

            // Load config
            LoadConfig();

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

        // Init
        private void InitializeThread()
        {
            Quest.QueryQuestsCompleted();
            Log.AddLog(Translation.GetText(Translation.Text.Initialise_Thread_Spell_Book));
            SpellManager.SpellBook();
            Log.AddLog(Translation.GetText(Translation.Text.Initialise_Thread_finish));

            _initializeFinish = true;
        }

        // CUSTOM CLASS
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
        private void CustomClassCbMouseHover(object sender, EventArgs e)
        {
            CustomClassCB.Properties.Items.Clear();
            foreach (string subfolder in Others.GetFilesDirectory("\\CustomClasses", "*.cs"))
            {
                CustomClassCB.Properties.Items.Add(subfolder);
            }
        }

        // Update gui
        private void UpdateLogTick(object sender, EventArgs e)
        {
            try
            {
                // Warden
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

                // Button Launch bot
                if (!BLaunchBot.Enabled || !BLaunchBotGrinding.Enabled)
                {
                    if (_initializeFinish)
                    {
                        BLaunchBot.Text = Translation.GetText(Translation.Text.Launch_Bot);
                        BLaunchBot.Enabled = true;

                        BLaunchBotGrinding.Text = "Launch Grinding Bot";
                        BLaunchBotGrinding.Enabled = true;

                        if (WowManager.WowManager.AutoLaunch)
                        {
                            BLaunchBotClick(null, null);
                            WowManager.WowManager.AutoLaunch = false;
                        }
                    }
                }

                if (Config.Bot.Fsm.Running && !Config.Bot.BotIsActive)
                {
                    BLaunchBot.Enabled = false;
                    BLaunchBot.Text = Translation.GetText(Translation.Text.Stoping_bot);

                    BLaunchBotGrinding.Text = Translation.GetText(Translation.Text.Stoping_bot);
                    BLaunchBotGrinding.Enabled = false;
                }
                else if (Config.Bot.BotIsActive)
                {
                    BLaunchBot.Text = Translation.GetText(Translation.Text.Stop_Bot);
                    BLaunchBotGrinding.Text = Translation.GetText(Translation.Text.Stop_Bot);
                }
                else
                {
                    BLaunchBot.Text = Translation.GetText(Translation.Text.Launch_Bot);
                    BLaunchBotGrinding.Text = "Launch Grinding Bot";
                }

                // State:
                var tSubQuestStat = String.Empty;
                if (Config.Bot.QuestStat != String.Empty && Config.Bot.Fsm.CurrentState == Bot.States.Priority.Questing.ToString())
                    tSubQuestStat = " > " + Config.Bot.QuestStat;
                fsmStatL.Text = "State: " + Config.Bot.Fsm.CurrentState + tSubQuestStat + " - " +
                                "Quest: " + Config.Bot.CurrentQuest.Id + " " + Config.Bot.CurrentQuest.Name;

                statL.Text = ObjectManager.Me.Name + " lvl: " + ObjectManager.Me.Level + " - " + Translation.GetText(Translation.Text.Kills) + ": " + Config.Bot.Kills + " " +
                    Translation.GetText(Translation.Text.Deaths) + ": " + Config.Bot.Deaths + " " +
                    Translation.GetText(Translation.Text.Loots) + ": " + Config.Bot.NumberLoot + " " +
                    Translation.GetText(Translation.Text.Farm) + ": " + Config.Bot.NumberFarm;


                // Log:
                if (Log.GetLastLog != _lastLog)
                {
                    _lastLog = Log.GetLastLog;

                    logTB.Text = "";
                    for (int t = Log.GetLog.Count - 1; t >= 0 && t >= Log.GetLog.Count - 100; t--)
                    {
                        logTB.Text = logTB.Text + Log.GetLog[t] + Environment.NewLine;
                    }
                }

                // Stop bot if
                if (!Config.Bot.BotIsActive && _stopBotAfterUc.IsActive)
                    _stopBotAfterUc.Stop();

                if (_stopBotAfterUc.IsActive && _stopBotAfterUc.Statuts == StopBotAfter.StatStopBot.Pause)
                {
                    Config.Bot.Pause = true;
                }
                else
                {
                    if (_stopBotAfterUc.IsActive)
                        Config.Bot.Pause = false;
                }
            }
            catch
            {
            }
        }

        // Launch Bot
        private void BLaunchBotClick(object sender, EventArgs e)
        {
            LaunchBot();
        }
        public void LaunchBot(bool grinding = false)
        {
            SaveConfig();
            if (Config.Bot.BotIsActive)
            {
                // Stop
                Config.Bot.Dispose();
                _stopBotAfterUc.Stop();
            }
            else
            {
                // Launch
                Config.Bot = new Bot.Bot(MainFormConfig.Load(), grinding);
                _stopBotAfterUc.Pulse();
            }
        }

        // Helper profile
        private void TargetEntryBClick(object sender, EventArgs e)
        {
            resultHelperProfileTB.Text = ObjectManager.Target.Entry.ToString();
        }

        private void TargetLocationBClick(object sender, EventArgs e)
        {
            resultHelperProfileTB.Text = "<X>" + ObjectManager.Target.Position.X + "</X>" + Environment.NewLine +
                                         "<Y>" + ObjectManager.Target.Position.Y + "</Y>" + Environment.NewLine +
                                         "<Z>" + ObjectManager.Target.Position.Z + "</Z>";
            resultHelperProfileTB.Text = resultHelperProfileTB.Text.Replace(",", ".");
        }

        private void TargetNameBClick(object sender, EventArgs e)
        {
            resultHelperProfileTB.Text = ObjectManager.Target.Name;
        }

        private void MyLocationBClick(object sender, EventArgs e)
        {
            resultHelperProfileTB.Text = "<X>" + ObjectManager.Me.Position.X + "</X>" + Environment.NewLine +
                                         "<Y>" + ObjectManager.Me.Position.Y + "</Y>" + Environment.NewLine +
                                         "<Z>" + ObjectManager.Me.Position.Z + "</Z>";
            resultHelperProfileTB.Text = resultHelperProfileTB.Text.Replace(",", ".");
        }

        private void EntryByNameBClick(object sender, EventArgs e)
        {
            if (nameForEntryByNameTB.Text != "")
            {
                resultHelperProfileTB.Text =
                    ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectByName(nameForEntryByNameTB.Text)).Entry.
                        ToString();
            }
        }

        private void RecWayClick(object sender, EventArgs e)
        {
            if (_loopRecordPoint)
            {
                StopRecordWay();
                recWay.Text = Translation.GetText(Translation.Text.Record_Way);
            }
            else
            {
                resultHelperProfileTB.Text = string.Empty;
                LoopRecordWay();
            }
        }
        private void LoopRecordWay()
        {
            const float distanceSeparator = 5.0f;
            const float distanceZSeparator = 5.0f;
            var lastPoint = new WowManager.MiscStructs.Point();

            _loopRecordPoint = true;
            int lastRotation = 0;
            while (_loopRecordPoint)
            {
                try
                {
                    recWay.Text = Translation.GetText(Translation.Text.Stop_Record_Way);

                    float disZTemp = lastPoint.DistanceZ(ObjectManager.Me.Position);


                    if (((lastPoint.DistanceTo(ObjectManager.Me.Position) > distanceSeparator) &&
                         lastRotation != (int)Others.RadianToDegree(ObjectManager.Me.Rotation)) ||
                        disZTemp >= distanceZSeparator)
                    {
                        resultHelperProfileTB.Text += (Environment.NewLine + "<Point>" + Environment.NewLine +
                                                      " <X>" + ObjectManager.Me.Position.X + "</X>" + Environment.NewLine +
                                                      " <Y>" + ObjectManager.Me.Position.Y + "</Y>" + Environment.NewLine +
                                                      " <Z>" + ObjectManager.Me.Position.Z + "</Z>" + Environment.NewLine +
                                                      "</Point>").Replace(",", ".");
                        lastRotation = (int)Others.RadianToDegree(ObjectManager.Me.Rotation);
                        lastPoint = ObjectManager.Me.Position;
                    }
                }
                catch { }
                Application.DoEvents();
                Thread.Sleep(50);
            }
            recWay.Text = Translation.GetText(Translation.Text.Record_Way);
        }
        private void StopRecordWay()
        {
            _loopRecordPoint = false;
        }

        // Show profiles list
        private void ListProfilesMouseHover(object sender, EventArgs e)
        {
            ListProfiles.Properties.Items.Clear();
            foreach (string subfolder in Others.GetFilesDirectory("\\Products\\Questing Bot\\Profiles\\", "*.xml"))
            {
                ListProfiles.Properties.Items.Add(subfolder);
            }
        }

        // Reset Config
        private void ResetConfigBClick1(object sender, EventArgs e)
        {
            MainFormConfig.Reset();
            LoadConfig();
        }

        // Load Config
        public void LoadConfig()
        {
            MainFormConfig config = MainFormConfig.Load();

            // Mount
            useMountCB.Checked = config.UseMount;
            nMountBar.Value = config.MountBar;
            nMountSlot.Value = config.MountSlot;

            // Fisht
            attackNpcInCombatCb.Checked = config.AttackNpcInCombat;

            // Farm
            farmMineCB.Checked = config.FarmMine;
            farmHerbCB.Checked = config.FarmHerb;
            SkinningCB.Checked = config.Skinning;

            // Custom Class
            CustomClassCB.Text = config.CustomClassName;

            // profiles
            ListProfiles.Text = config.ProfileName;

            // Talents
            talentsCB.Checked = config.Talents;

            // Path finder
            pathFinderCB.Checked = config.UsePathFinder;

            // Dead
            cbUseSpiritHealer.Checked = config.UseSpiritHealer;

            // Mail
            mailAtCB.Checked = config.MailAt;
            mailAtNameTB.Text = config.MailAtName;

            // Regen
            Regen_Min_Hp.Value = config.RegenMinHp;
            Regen_Max_Hp.Value = config.RegenMaxHp;
            Regen_Use_Food.Checked = config.RegenUseFood;
            Regen_Min_Mp.Value = config.RegenMinMp;
            RegenMp.Checked = config.RegenMp;
            Regen_Max_Mp.Value = config.RegenMaxMp;
            Regen_Use_Water.Checked = config.RegenUseWater;
            regenPet.Checked = config.RegenPet;
            Regen_Pet_Min_Hp.Value = config.RegenPetMinHp;
            Regen_Pet_Max_Hp.Value = config.RegenPetMaxHp;
            Regen_Use_Pet_Macro.Checked = config.RegenUsePetMacro;
            petBar.Value = config.PetBar;
            petSlot.Value = config.PetSlot;

            // Relog
            cbReLog.Checked = config.ReLogActive;

            // Grinding
            profileGrindingCB.Text = config.ProfileGrinding;
            foodNameGrindingTB.Text = config.FoodNameGrinding;
            waterNameGrindingTB.Text = config.WaterNameGrinding;

            try
            {
                if (config.Relog != "")
                {
                    string fileLog = config.Relog;
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

        // Save Config
        public void SaveConfig()
        {
            var config = new MainFormConfig
            {
                // Mount
                UseMount = useMountCB.Checked,
                MountBar = (int)nMountBar.Value,
                MountSlot = (int)nMountSlot.Value,
                // Fisht
                AttackNpcInCombat = attackNpcInCombatCb.Checked,
                // Farm
                FarmMine = farmMineCB.Checked,
                FarmHerb = farmHerbCB.Checked,
                Skinning = SkinningCB.Checked,
                // Talents
                Talents = talentsCB.Checked,
                // Custom Class
                CustomClassName = CustomClassCB.Text,
                // profiles
                ProfileName = ListProfiles.Text,
                // Path finder
                UsePathFinder = pathFinderCB.Checked,
                // Dead
                UseSpiritHealer = cbUseSpiritHealer.Checked,
                // Mail
                MailAt = mailAtCB.Checked,
                MailAtName = mailAtNameTB.Text,
                // Regen
                RegenMinHp = (int)Regen_Min_Hp.Value,
                RegenMaxHp = (int)Regen_Max_Hp.Value,
                RegenUseFood = Regen_Use_Food.Checked,
                RegenMinMp = (int)Regen_Min_Mp.Value,
                RegenMp = RegenMp.Checked,
                RegenMaxMp = (int)Regen_Max_Mp.Value,
                RegenUseWater = Regen_Use_Water.Checked,
                RegenPet = regenPet.Checked,
                RegenPetMinHp = (int)Regen_Pet_Min_Hp.Value,
                RegenPetMaxHp = (int)Regen_Pet_Max_Hp.Value,
                RegenUsePetMacro = Regen_Use_Pet_Macro.Checked,
                PetBar = (int)petBar.Value,
                PetSlot = (int)petSlot.Value,
                // Relog
                ReLogActive = cbReLog.Checked,
                // Grinding
                ProfileGrinding = profileGrindingCB.Text,
                FoodNameGrinding = foodNameGrindingTB.Text,
                WaterNameGrinding = waterNameGrindingTB.Text,
            };

            try
            {
                if (tbAccountName.Text != "" && tbPassword.Text != "")
                    config.Relog = Others.StringToEncryptString(tbAccountName.Text) + "#" +
                                   Others.StringToEncryptString(tbPassword.Text) + "#" +
                                   Others.StringToEncryptString(numberPlayerNUD.Text) + "#" +
                                   Others.StringToEncryptString(tbRealm.Text);
                else
                    config.Relog = "";
            }
            catch
            {
            }

            config.Save();
        }

        private void editNodeList_Click(object sender, EventArgs e)
        {
            var f = new NodeList();
            f.Show();
        }

        private void ProfileGrindingCbMouseHover(object sender, EventArgs e)
        {
            profileGrindingCB.Properties.Items.Clear();
            foreach (string subfolder in Others.GetFilesDirectory("\\Products\\Questing Bot\\Profiles Grinding\\", "*.xml"))
            {
                profileGrindingCB.Properties.Items.Add(subfolder);
            }
            foreach (string subfolder in Others.GetFilesDirectory("\\Products\\Leveling Bot\\Profiles\\", "*.xml"))
            {
                if (!profileGrindingCB.Properties.Items.Contains(subfolder))
                    profileGrindingCB.Properties.Items.Add(subfolder);
            }
        }

        private void BLaunchBotGrindingClick(object sender, EventArgs e)
        {
            LaunchBot(true);
        }

        private void createProfilF_Click(object sender, EventArgs e)
        {
            var formProfil = new Grinding.CreateProfile();
            formProfil.Show();
        }

        private void getAllB_Click(object sender, EventArgs e)
        {
            try
            {
                var codeHtml = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"> <html xmlns=\"http://www.w3.org/1999/xhtml\"> <head> <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" /> <title>Get all objects information - " + DateTime.Now.ToString("dd/mm/yy HHh mmMin") + "</title>  <script type\"text/javascript\"> function trouverMots(chaine) { /* document.getElementById('rechDsPg').value = ''; */  var ouvrirBalise = '<span style=\"background-color: '; var frmOvrBalise = ';\">'; var fermerBalise = '</span>'; var doc = document.body.innerHTML; var j = 0;  var arrayClrs = new Array(\"#FFFF00\", \"#66FFFF\", \"#33FF33\", \"#3333FF\", \"#FF9900\", \"#FF33FF\", \"#CCFF00\", \"#FF0000\");  tablMots = chaine.split(' ');  rchSupp = new RegExp( '(' + ouvrirBalise + '[^><]*>)' , 'gi'); doc = doc.replace(rchSupp, ''); rchSupp = new RegExp( '(' + fermerBalise + ')' , 'gi'); doc = doc.replace(rchSupp, '');  for (i = 0; i < tablMots.length; i++) { if (j >= arrayClrs.length) {j = 0;} if (tablMots[i] != '' && tablMots[i].length > 2) { rch = new RegExp( '(' + tablMots[i] + ')' , 'gi'); ouvrBalise = ouvrirBalise + arrayClrs[j] + frmOvrBalise; doc = doc.replace(rch, ouvrBalise + '$1' + fermerBalise); j += 1; } }  document.body.innerHTML = doc; } </script>   </head>  <body>  <p> Search : <input id=\"rechDsPg\" type=\"text\" value=\"\" name=\"rechDsPg\" /> <input type=\"button\" onclick=\"trouverMots(document.getElementById('rechDsPg').value);\" value=\"OK\"> </p>   <table width=\"100%\" border=\"1\">   <tr>   <b>     <td bgcolor=\"#CCCCCC\">Name</td>     <td>Type</td>     <td bgcolor=\"#CCCCCC\">Entry ID</td>     <td>Position X</td>     <td bgcolor=\"#CCCCCC\">Position Y</td>     <td>Position Z</td>     <td bgcolor=\"#CCCCCC\">Distance</td>     <td>Faction</td>     <td bgcolor=\"#CCCCCC\">GUID</td>     <td>Summoned/Created By</td>     </b>   </tr>  ";
                // Me
                codeHtml +=
                       "<tr>     <td bgcolor=\"#CCCCCC\">" + ObjectManager.Me.Name + "</td>     <td>WoWPlayer</td>     <td bgcolor=\"#CCCCCC\">" + ObjectManager.Me.Entry + "</td>     <td>" + ObjectManager.Me.Position.X + "</td>     <td bgcolor=\"#CCCCCC\">" + ObjectManager.Me.Position.Y + "</td>     <td>" + ObjectManager.Me.Position.Z + "</td>     <td bgcolor=\"#CCCCCC\">" + ObjectManager.Me.GetDistance + "</td>     <td>" + ObjectManager.Me.Faction + "</td>     <td bgcolor=\"#CCCCCC\">" + ObjectManager.Me.Guid + "</td>     <td>" + ObjectManager.Me.SummonedBy + "</td>   </tr>";
     
                // WoWPlayer
                foreach (var woWPlayer in ObjectManager.GetObjectWoWPlayer())
                {
                    codeHtml +=
                        "<tr>     <td bgcolor=\"#CCCCCC\">" + woWPlayer.Name + "</td>     <td>WoWPlayer</td>     <td bgcolor=\"#CCCCCC\">" + woWPlayer.Entry + "</td>     <td>" + woWPlayer.Position.X + "</td>     <td bgcolor=\"#CCCCCC\">" + woWPlayer.Position.Y + "</td>     <td>" + woWPlayer.Position.Z + "</td>     <td bgcolor=\"#CCCCCC\">" + woWPlayer.GetDistance + "</td>     <td>" + woWPlayer.Faction + "</td>     <td bgcolor=\"#CCCCCC\">" + woWPlayer.Guid + "</td>     <td>" + woWPlayer.SummonedBy + "</td>   </tr>";
                }
                // WoWUnit
                foreach (var wowO in ObjectManager.GetObjectWoWUnit())
                {
                    codeHtml +=
                        "<tr>     <td bgcolor=\"#CCCCCC\">" + wowO.Name + " - (<i><a href=\"http://wowhead.com/npc=" + wowO.Entry + "\"  target=\"_blank\">on WowHead</a></i>)</td>     <td>WoWUnit</td>     <td bgcolor=\"#CCCCCC\">" + wowO.Entry + "</td>     <td>" + wowO.Position.X + "</td>     <td bgcolor=\"#CCCCCC\">" + wowO.Position.Y + "</td>     <td>" + wowO.Position.Z + "</td>     <td bgcolor=\"#CCCCCC\">" + wowO.GetDistance + "</td>     <td>" + wowO.Faction + "</td>     <td bgcolor=\"#CCCCCC\">" + wowO.Guid + "</td>     <td>" + wowO.SummonedBy + "</td>   </tr>";
                }
                // WoWGameObject
                foreach (var wowO in ObjectManager.GetObjectWoWGameObject())
                {
                    codeHtml +=
                        "<tr>     <td bgcolor=\"#CCCCCC\">" + wowO.Name + " - (<i><a href=\"http://wowhead.com/object=" + wowO.Entry + "\"  target=\"_blank\">on WowHead</a></i>)</td>     <td>WoWGameObject</td>     <td bgcolor=\"#CCCCCC\">" + wowO.Entry + "</td>     <td>" + wowO.Position.X + "</td>     <td bgcolor=\"#CCCCCC\">" + wowO.Position.Y + "</td>     <td>" + wowO.Position.Z + "</td>     <td bgcolor=\"#CCCCCC\">" + wowO.GetDistance + "</td>     <td>-</td>     <td bgcolor=\"#CCCCCC\">" + wowO.Guid + "</td>     <td>" + wowO.CreatedBy + "</td>   </tr>";
                }
                // WoWItem
                foreach (var wowO in ObjectManager.GetObjectWoWItem())
                {
                    codeHtml +=
                        "<tr>     <td bgcolor=\"#CCCCCC\">" + wowO.Name + " - (<i><a href=\"http://wowhead.com/item=" + wowO.Entry + "\"  target=\"_blank\">on WowHead</a></i>)</td>     <td>WoWItem</td>     <td bgcolor=\"#CCCCCC\">" + wowO.Entry + "</td>     <td>-</td>     <td bgcolor=\"#CCCCCC\">-</td>     <td>-</td>     <td bgcolor=\"#CCCCCC\">-</td>     <td>-</td>     <td bgcolor=\"#CCCCCC\">-</td>     <td>-</td>   </tr>";
                }
                // WoWCorpse
                foreach (var wowO in ObjectManager.GetObjectWoWCorpse())
                {
                    codeHtml +=
                        "<tr>     <td bgcolor=\"#CCCCCC\">" + wowO.Name + "</td>     <td>WoWCorpse</td>     <td bgcolor=\"#CCCCCC\">" + wowO.Entry + "</td>     <td>" + wowO.Position.X + "</td>     <td bgcolor=\"#CCCCCC\">" + wowO.Position.Y + "</td>     <td>" + wowO.Position.Z + "</td>     <td bgcolor=\"#CCCCCC\">" + wowO.GetDistance + "</td>     <td>-</td>     <td bgcolor=\"#CCCCCC\">" + wowO.Guid + "</td>     <td>-</td>   </tr>";
                }
                // WoWContainer
                foreach (var wowO in ObjectManager.GetObjectWoWContainer())
                {
                    codeHtml +=
                        "<tr>     <td bgcolor=\"#CCCCCC\">" + wowO.Name + " - (<i><a href=\"http://wowhead.com/item=" + wowO.Entry + "\"  target=\"_blank\">on WowHead</a></i>)</td>     <td>WoWContainer</td>     <td bgcolor=\"#CCCCCC\">" + wowO.Entry + "</td>     <td>-</td>     <td bgcolor=\"#CCCCCC\">-</td>     <td>-</td>     <td bgcolor=\"#CCCCCC\">-</td>     <td>-</td>     <td bgcolor=\"#CCCCCC\">" + wowO.Guid + "</td>     <td>-</td>   </tr>";
                }


                codeHtml += " </table> </body> </html>";
                Others.WriteFile("Get all objects information.html", codeHtml);

                var myInfo = new Process
                {
                    StartInfo =
                    {
                        FileName = "Get all objects information.html",
                        WorkingDirectory = Application.StartupPath
                    }
                };
                myInfo.Start();

            }
            catch{}
        }
    }
}