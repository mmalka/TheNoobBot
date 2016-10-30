using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualBasic;
using nManager;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using nManager.Wow.Patchables;
using Information = Microsoft.VisualBasic.Information;
using Keybindings = nManager.Wow.Enums.Keybindings;
using Math = System.Math;
using Point = nManager.Wow.Class.Point;

namespace Quester.Profile
{
    public partial class SimpleProfileEditor
    {
        private readonly TreeNode _npcParentNode;
        private readonly TreeNode _questParentNode;
        private string _fullpath;
        private TreeNode _lastSelectedNpc;
        private TreeNode _lastSelectedObjective;
        private TreeNode _lastSelectedQuest;

        private QuesterProfile _profile;

        public SimpleProfileEditor(string profile = "")
        {
            InitializeComponent();

            /*TODO Auto Create Taxi
            Wow.Enums.WoWEventsType.ENABLE_TAXI_BENCHMARK
            Wow.Enums.WoWEventsType.DISABLE_TAXI_BENCHMARK
            TODO QUEST_LOOT_RECEIVED = 804 Auto QuestLoot
             */
            _fsize = Size;

            PopulateComboBox();

            PanelObjTaxi.Location = PanelObjAll.Location;
            _npcParentNode = new TreeNode("NPCs");
            _questParentNode = new TreeNode("Quests");
            LoadNodes(profile);

            if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                TopMost = true;
        }

        private void ButtonLoadXML_Click(object sender, EventArgs e)
        {
            ClearQuestForm();
            DisableObjForm();
            LoadNodes();
        }

        private void SaveSimpleProfile_Click(object sender, EventArgs e)
        {
            if (!File.Exists(_fullpath))
            {
                SaveSimpleProfileAs_Click(sender, e);
                return;
            }
            if (_profile.Quests.Count > 0 || _profile.Questers.Count > 0)
            {
                XmlSerializer.Serialize(_fullpath, _profile);
                Close();
            }
            else
            {
                MessageBox.Show(Translate.Get(Translate.Id.CantSaveEmptySimpleExisting));
            }
        }

        private void SaveSimpleProfileAs_Click(object sender, EventArgs e)
        {
            if (_profile.Quests.Count > 0 || _profile.Questers.Count > 0)
            {
                string fileToSaveAs = Others.DialogBoxSaveFile(Application.StartupPath + "\\Profiles\\Quester\\", Translate.Get(Translate.Id.SimpleQuestProfileFile) + " (*.xml)|*.xml");
                if (fileToSaveAs != "")
                    XmlSerializer.Serialize(fileToSaveAs, _profile);
                Close();
            }
            else
            {
                MessageBox.Show(Translate.Get(Translate.Id.CantSaveEmptySimpleNew));
            }
        }

        private void ButtonNewNPC_Click(object sender, EventArgs e)
        {
            ClearNPCForm();
            TreeView.SelectedNode = null;
        }

        private void ButtonSaveNPC_Click(object sender, EventArgs e)
        {
            if (TreeView.SelectedNode != null && (string) TreeView.SelectedNode.Tag != "NPCs")
            {
                //NPC Modification
                Npc quester = _profile.Questers[_lastSelectedNpc.Index];

                quester.Name = TBNpcName.Text;
                quester.Entry = Others.ToInt32(TBNpcId.Text);
                quester.Position = new Point(float.Parse(TBNpcPosition.Text.Split(';')[0]), float.Parse(TBNpcPosition.Text.Split(';')[1]), float.Parse(TBNpcPosition.Text.Split(';')[2]));
                quester.Faction = (Npc.FactionType) CBNpcFaction.SelectedValue;
                quester.Type = (Npc.NpcType) CBNpcType.SelectedValue;
                quester.ContinentId = TBNpcContinentId.Text;
                TreeView.SelectedNode.Text = TBNpcName.Text;
            }
            else
            {
                //New NPC
                var newQuester = new Npc
                {
                    Name = TBNpcName.Text,
                    Entry = Others.ToInt32(TBNpcId.Text),
                    Position = new Point(float.Parse(TBNpcPosition.Text.Split(';')[0]), float.Parse(TBNpcPosition.Text.Split(';')[1]), float.Parse(TBNpcPosition.Text.Split(';')[2])),
                    Faction = (Npc.FactionType) CBNpcFaction.SelectedValue,
                    Type = (Npc.NpcType) CBNpcType.SelectedValue,
                    ContinentId = TBNpcContinentId.Text
                };

                _profile.Questers.Add(newQuester);

                var npcNode = new TreeNode(TBNpcName.Text) {Tag = "NPC"};

                _npcParentNode.Nodes.Add(npcNode);
                TreeView.SelectedNode = npcNode;
            }
        }


        private void ButtonNpcImport_Click(object sender, EventArgs e)
        {
            if (ObjectManager.Target.IsValid && QuestersDB.GetNpcByEntry(ObjectManager.Target.Entry).Entry == 0)
            {
                if (ObjectManager.Target.IsValid)
                {
                    TBNpcName.Text = ObjectManager.Target.Name;
                    TBNpcId.Text = ObjectManager.Target.Entry.ToString(CultureInfo.InvariantCulture);
                    TBNpcPosition.Text = ObjectManager.Target.Position.ToString();
                    CBNpcFaction.SelectedValue = ObjectManager.Target.Faction;

                    CBNpcType.SelectedValue = (ObjectManager.Target.GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags).HasFlag(UnitNPCFlags.QuestGiver)) ? Npc.NpcType.QuestGiver : Npc.NpcType.FlightMaster;
                    TBNpcContinentId.Text = Usefuls.ContinentNameByContinentId(Usefuls.ContinentId);
                }
            }
            else
            {
                WoWGameObject wowGO = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectOfType(WoWGameObjectType.Questgiver));

                if (wowGO.Entry > 0 && ObjectManager.Me.Position.DistanceTo(wowGO.Position) < 5f && QuestersDB.GetNpcByEntry(wowGO.Entry).Entry == 0)
                {
                    TBNpcName.Text = wowGO.Name;
                    TBNpcId.Text = wowGO.Entry.ToString(CultureInfo.InvariantCulture);
                    TBNpcPosition.Text = wowGO.Position.ToString();
                    CBNpcFaction.SelectedValue = wowGO.Faction;
                    CBNpcType.SelectedValue = Npc.NpcType.QuestGiver;
                    TBNpcContinentId.Text = Usefuls.ContinentNameByContinentId(Usefuls.ContinentId);
                }
            }
        }


        private void ButtonObjectiveNew_Click(object sender, EventArgs e)
        {
            DisableObjForm();
            TreeView.SelectedNode = null;
        }

        private void ButtonObjectiveSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (TreeView.SelectedNode != null && ((string) TreeView.SelectedNode.Tag == "Objective" || (string) TreeView.SelectedNode.Tag == "NewObjective"))
                {
                    //Existing Objective Modification(s)

                    QuestObjective objective = _profile.Quests[_lastSelectedObjective.Parent.Index].Objectives[_lastSelectedObjective.Index];

                    //Handles when a New Objective was added with the right click on the treeview
                    if ((string) TreeView.SelectedNode.Tag == "NewObjective")
                    {
                        int cbObjSelValue;
                        if (Information.IsNumeric(CBObjType.SelectedValue))
                        {
                            cbObjSelValue = (int) CBObjType.SelectedValue;
                        }
                        else
                        {
                            cbObjSelValue = ((ComboBoxValueString) CBObjType.SelectedValue).Value;
                        }
                        objective.Objective = (Objective) cbObjSelValue;
                        TreeView.SelectedNode.Tag = "Objective";
                        TreeView.SelectedNode.Text = objective.Objective + @" " + objective.QuestName;
                    }

                    switch (objective.Objective.ToString())
                    {
                        case "KillMob":
                            if (objective.CollectItemId > 0 || CBObjKillMobPickUpItem.Checked)
                            {
                                TBObjCollectCount.Enabled = true;
                                TBObjCollectItemID.Enabled = true;
                                objective.CollectCount = Others.ToInt32(TBObjCollectCount.Text);
                                objective.CollectItemId = Others.ToInt32(TBObjCollectItemID.Text);
                                objective.Count = 0;
                            }
                            else
                            {
                                objective.Count = Others.ToInt32(TBObjCount.Text);
                                objective.CollectCount = 0;
                                objective.CollectItemId = 0;
                            }
                            objective.CanPullUnitsAlreadyInFight = CBObjCanPullUnitsInFight.Checked;

                            break;
                        case "BuyItem":

                            objective.CollectCount = Others.ToInt32(TBObjCollectCount.Text);
                            objective.CollectItemId = Others.ToInt32(TBObjCollectItemID.Text);
                            objective.Name = TBObjNPCId.Text;
                            objective.Position = new Point(float.Parse(TBObjPosition.Text.Split(';')[0]), float.Parse(TBObjPosition.Text.Split(';')[1]),
                                float.Parse(TBObjPosition.Text.Split(';')[2]));
                            break;
                        case "PickUpObject":

                            objective.CollectCount = Others.ToInt32(TBObjCollectCount.Text);
                            objective.CollectItemId = Others.ToInt32(TBObjCollectItemID.Text);

                            break;
                        case "UseItem":

                            objective.UseItemId = Others.ToInt32(TBObjUseItemID.Text);
                            objective.Count = Others.ToInt32(TBObjCount.Text);
                            objective.Position = new Point(float.Parse(TBObjPosition.Text.Split(';')[0]), float.Parse(TBObjPosition.Text.Split(';')[1]),
                                float.Parse(TBObjPosition.Text.Split(';')[2]));
                            objective.Range = Others.ToInt32(TBObjRange.Text);
                            objective.WaitMs = Others.ToInt32(TBObjWaitMs.Text);

                            break;
                        case "UseItemAOE":

                            objective.UseItemId = Others.ToInt32(TBObjUseItemID.Text);
                            objective.Count = Others.ToInt32(TBObjCount.Text);
                            objective.Position = new Point(float.Parse(TBObjPosition.Text.Split(';')[0]), float.Parse(TBObjPosition.Text.Split(';')[1]),
                                float.Parse(TBObjPosition.Text.Split(';')[2]));
                            objective.Range = Others.ToInt32(TBObjRange.Text);
                            objective.WaitMs = Others.ToInt32(TBObjWaitMs.Text);

                            break;
                        case "UseSpell":
                            //TODO ENTRY + NAME + POSITION
                            objective.Position = new Point(float.Parse(TBObjPosition.Text.Split(';')[0]), float.Parse(TBObjPosition.Text.Split(';')[1]),
                                float.Parse(TBObjPosition.Text.Split(';')[2]));

                            objective.UseSpellId = Others.ToInt32(TBObjUseSpellId.Text);
                            objective.WaitMs = Others.ToInt32(TBObjWaitMs.Text);

                            break;
                        case "UseSpellAOE":
                            objective.Position = new Point(float.Parse(TBObjPosition.Text.Split(';')[0]), float.Parse(TBObjPosition.Text.Split(';')[1]),
                                float.Parse(TBObjPosition.Text.Split(';')[2]));


                            objective.UseSpellId = Others.ToInt32(TBObjUseSpellId.Text);
                            objective.Range = Others.ToInt32(TBObjRange.Text);
                            objective.WaitMs = Others.ToInt32(TBObjWaitMs.Text);


                            break;
                        case "InteractWith":
                            objective.Position = new Point(float.Parse(TBObjPosition.Text.Split(';')[0]), float.Parse(TBObjPosition.Text.Split(';')[1]),
                                float.Parse(TBObjPosition.Text.Split(';')[2]));
                            objective.GossipOptionsInteractWith = (Information.IsNumeric(TBObjGossipOption.Text) ? Others.ToInt32(TBObjGossipOption.Text) : 0);
                            objective.WaitMs = (Information.IsNumeric(TBObjWaitMs.Text) ? Others.ToInt32(TBObjWaitMs.Text) : 0);

                            break;
                        case "MoveTo":
                            objective.Position = new Point(float.Parse(TBObjPosition.Text.Split(';')[0]), float.Parse(TBObjPosition.Text.Split(';')[1]),
                                float.Parse(TBObjPosition.Text.Split(';')[2]));
                            break;

                        case "Wait":
                            objective.WaitMs = Others.ToInt32(TBObjWaitMs.Text);
                            break;
                        case "PickUpQuest":

                            objective.NpcEntry = Others.ToInt32(TBObjNPCId.Text);
                            objective.QuestId = Others.ToInt32(TBObjQuestID.Text);
                            objective.QuestName = TBObjQuestName.Text;

                            break;
                        case "TurnInQuest":
                            objective.NpcEntry = Others.ToInt32(TBObjNPCId.Text);
                            objective.QuestId = Others.ToInt32(TBObjQuestID.Text);
                            objective.QuestName = TBObjQuestName.Text;

                            break;
                        case "UseFlightPath":
                            objective.TaxiEntry = Others.ToInt32(TBObjTaxiEntryId.Text);
                            objective.FlightDestinationX = TBObjDestinationX.Text;
                            objective.FlightDestinationY = TBObjDestinationY.Text;
                            objective.WaitMs = Others.ToInt32(TBObjFlightWaitMs.Text);

                            break;
                        case "PickUpNPC":
                            objective.Count = Others.ToInt32(TBObjCount.Text);
                            objective.CanPullUnitsAlreadyInFight = CBObjCanPullUnitsInFight.Checked;
                            objective.GossipOptionsInteractWith = (Information.IsNumeric(TBObjGossipOption.Text)) ? Others.ToInt32(TBObjGossipOption.Text) : 0;
                            objective.WaitMs = Others.ToInt32(TBObjWaitMs.Text);
                            break;
                        case "UseVehicle":
                            objective.Position = new Point(float.Parse(TBObjPosition.Text.Split(';')[0]), float.Parse(TBObjPosition.Text.Split(';')[1]),
                                float.Parse(TBObjPosition.Text.Split(';')[2]));
                            break;
                        case "ClickOnTerrain":
                            objective.Position = new Point(float.Parse(TBObjPosition.Text.Split(';')[0]), float.Parse(TBObjPosition.Text.Split(';')[1]),
                                float.Parse(TBObjPosition.Text.Split(';')[2]));
                            objective.WaitMs = Others.ToInt32(TBObjWaitMs.Text);

                            break;
                        case "MessageBox":
                            objective.Message = TBObjMessage.Text;

                            break;
                        case "PressKey":
                            objective.Keys = (Keybindings) CBObjPressKeys.SelectedValue;

                            objective.Count = Others.ToInt32(TBObjCount.Text);
                            objective.WaitMs = Others.ToInt32(TBObjWaitMs.Text);
                            objective.Position = new Point(float.Parse(TBObjPosition.Text.Split(';')[0]), float.Parse(TBObjPosition.Text.Split(';')[1]),
                                float.Parse(TBObjPosition.Text.Split(';')[2]));

                            break;
                        case "CSharpScript":
                            objective.Count = Others.ToInt32(TBObjCount.Text);
                            objective.Script = TBObjMessage.Text;

                            break;
                    }


                    if (CBInternalObj.Checked)
                    {
                        //if (objective.InternalQuestId == 0) {
                        objective.InternalQuestId = Others.ToInt32(CBObjInternalQuestIdManual.Checked ? CBObjInternalQuestID.Text : CBObjInternalQuestID.SelectedValue.ToString());
                        //}
                    }
                    else
                    {
                        objective.InternalQuestId = 0;
                    }

                    if (Information.IsNumeric(TBObjInternalIndex.Text))
                    {
                        objective.InternalIndex = uint.Parse(TBObjInternalIndex.Text);
                    }

                    objective.IgnoreQuestCompleted = CBObjIgnoreQuestCompleted.Checked;

                    if (objective.Objective.ToString() != "UseVehicle")
                    {
                        objective.Entry.Clear();
                        foreach (String st in TBObjEntry.Lines)
                        {
                            if (st != "")
                            {
                                objective.Entry.Add(Convert.ToInt32(st));
                            }
                        }
                    }

                    objective.Hotspots.Clear();
                    foreach (Point point in LBObjHotspots.Items)
                    {
                        if (point != null)
                        {
                            objective.Hotspots.Add(point);
                        }
                    }

                    objective.IsDead = CBObjIsDead.Checked;

                    if (_displayXml)
                    {
                        DisplayXMLs(objective);
                    }
                }
                else if (_lastSelectedQuest != null)
                {
                    //New Objective

                    int cbObjSelValue;
                    if (Information.IsNumeric(CBObjType.SelectedValue))
                    {
                        cbObjSelValue = (int) CBObjType.SelectedValue;
                    }
                    else
                    {
                        cbObjSelValue = ((ComboBoxValueString) CBObjType.SelectedValue).Value;
                    }

                    var newObjective = new QuestObjective {Objective = (Objective) cbObjSelValue};

                    switch (newObjective.Objective.ToString())
                    {
                        case "KillMob":
                            if (CBObjKillMobPickUpItem.Checked)
                            {
                                newObjective.CollectCount = Others.ToInt32(TBObjCollectCount.Text);
                                newObjective.CollectItemId = Others.ToInt32(TBObjCollectItemID.Text);
                            }
                            else
                            {
                                newObjective.Count = Others.ToInt32(TBObjCount.Text);
                            }

                            newObjective.CanPullUnitsAlreadyInFight = CBObjCanPullUnitsInFight.Checked;

                            break;
                        case "BuyItem":
                            newObjective.CollectCount = Others.ToInt32(TBObjCollectCount.Text);
                            newObjective.CollectItemId = Others.ToInt32(TBObjCollectItemID.Text);
                            newObjective.Name = TBObjNPCId.Text;
                            newObjective.Position = new Point(float.Parse(TBObjPosition.Text.Split(';')[0]), float.Parse(TBObjPosition.Text.Split(';')[1]),
                                float.Parse(TBObjPosition.Text.Split(';')[2]));

                            break;
                        case "PickUpObject":
                            newObjective.CollectCount = Others.ToInt32(TBObjCollectCount.Text);
                            newObjective.CollectItemId = Others.ToInt32(TBObjCollectItemID.Text);

                            break;
                        case "UseItem":
                            newObjective.UseItemId = Others.ToInt32(TBObjUseItemID.Text);
                            newObjective.Count = Others.ToInt32(TBObjCount.Text);
                            newObjective.Position = new Point(float.Parse(TBObjPosition.Text.Split(';')[0]), float.Parse(TBObjPosition.Text.Split(';')[1]),
                                float.Parse(TBObjPosition.Text.Split(';')[2]));
                            newObjective.Range = Others.ToInt32(TBObjRange.Text);
                            newObjective.WaitMs = Others.ToInt32(TBObjWaitMs.Text);

                            break;
                        case "UseItemAOE":

                            newObjective.UseItemId = Others.ToInt32(TBObjUseItemID.Text);
                            newObjective.Count = Others.ToInt32(TBObjCount.Text);
                            newObjective.Position = new Point(float.Parse(TBObjPosition.Text.Split(';')[0]), float.Parse(TBObjPosition.Text.Split(';')[1]),
                                float.Parse(TBObjPosition.Text.Split(';')[2]));
                            newObjective.Range = Others.ToInt32(TBObjRange.Text);
                            newObjective.WaitMs = Others.ToInt32(TBObjWaitMs.Text);
                            break;
                        case "UseSpell":
                            //TODO ENTRY + NAME + POSITION

                            newObjective.Position = new Point(float.Parse(TBObjPosition.Text.Split(';')[0]), float.Parse(TBObjPosition.Text.Split(';')[1]),
                                float.Parse(TBObjPosition.Text.Split(';')[2]));
                            newObjective.UseSpellId = Others.ToInt32(TBObjUseSpellId.Text);
                            newObjective.WaitMs = Others.ToInt32(TBObjWaitMs.Text);

                            break;
                        case "UseSpellAOE":

                            newObjective.Position = new Point(float.Parse(TBObjPosition.Text.Split(';')[0]), float.Parse(TBObjPosition.Text.Split(';')[1]),
                                float.Parse(TBObjPosition.Text.Split(';')[2]));
                            newObjective.UseSpellId = Others.ToInt32(TBObjUseSpellId.Text);
                            newObjective.Range = Others.ToInt32(TBObjRange.Text);
                            newObjective.WaitMs = Others.ToInt32(TBObjWaitMs.Text);

                            break;
                        case "InteractWith":
                            newObjective.Position = new Point(float.Parse(TBObjPosition.Text.Split(';')[0]), float.Parse(TBObjPosition.Text.Split(';')[1]),
                                float.Parse(TBObjPosition.Text.Split(';')[2]));
                            newObjective.GossipOptionsInteractWith = (Information.IsNumeric(TBObjGossipOption.Text) ? Others.ToInt32(TBObjGossipOption.Text) : 0);
                            newObjective.WaitMs = (Information.IsNumeric(TBObjWaitMs.Text) ? Others.ToInt32(TBObjWaitMs.Text) : 0);

                            break;
                        case "MoveTo":
                            newObjective.Position = new Point(float.Parse(TBObjPosition.Text.Split(';')[0]), float.Parse(TBObjPosition.Text.Split(';')[1]),
                                float.Parse(TBObjPosition.Text.Split(';')[2]));
                            break;

                        case "Wait":
                            newObjective.WaitMs = Others.ToInt32(TBObjWaitMs.Text);
                            break;

                        case "PickUpQuest":
                            newObjective.NpcEntry = Others.ToInt32(TBObjNPCId.Text);
                            newObjective.QuestId = Others.ToInt32(TBObjQuestID.Text);
                            newObjective.QuestName = TBObjQuestName.Text;

                            break;

                        case "TurnInQuest":
                            newObjective.NpcEntry = Others.ToInt32(TBObjNPCId.Text);
                            newObjective.QuestId = Others.ToInt32(TBObjQuestID.Text);
                            newObjective.QuestName = TBObjQuestName.Text;

                            break;
                        case "UseFlightPath":
                            newObjective.TaxiEntry = Others.ToInt32(TBObjTaxiEntryId.Text);
                            newObjective.FlightDestinationX = TBObjDestinationX.Text;
                            newObjective.FlightDestinationY = TBObjDestinationY.Text;
                            newObjective.WaitMs = Others.ToInt32(TBObjFlightWaitMs.Text);
                            break;
                        case "PickUpNPC":

                            newObjective.Count = Others.ToInt32(TBObjCount.Text);
                            newObjective.CanPullUnitsAlreadyInFight = CBObjCanPullUnitsInFight.Checked;
                            newObjective.GossipOptionsInteractWith = (Information.IsNumeric(TBObjGossipOption.Text) ? Others.ToInt32(TBObjGossipOption.Text) : 0);
                            newObjective.WaitMs = Others.ToInt32(TBObjWaitMs.Text);
                            break;
                        case "UseVehicle":
                            newObjective.Position = new Point(float.Parse(TBObjPosition.Text.Split(';')[0]), float.Parse(TBObjPosition.Text.Split(';')[1]),
                                float.Parse(TBObjPosition.Text.Split(';')[2]));
                            break;
                        case "ClickOnTerrain":
                            newObjective.Position = new Point(float.Parse(TBObjPosition.Text.Split(';')[0]), float.Parse(TBObjPosition.Text.Split(';')[1]),
                                float.Parse(TBObjPosition.Text.Split(';')[2]));
                            newObjective.WaitMs = Others.ToInt32(TBObjWaitMs.Text);

                            break;
                        case "MessageBox":
                            newObjective.Message = TBObjMessage.Text;

                            break;
                        case "PressKey":
                            newObjective.Keys = (Keybindings) CBObjPressKeys.SelectedValue;
                            newObjective.Count = Others.ToInt32(TBObjCount.Text);
                            newObjective.WaitMs = Others.ToInt32(TBObjWaitMs.Text);
                            newObjective.Position = new Point(float.Parse(TBObjPosition.Text.Split(';')[0]), float.Parse(TBObjPosition.Text.Split(';')[1]),
                                float.Parse(TBObjPosition.Text.Split(';')[2]));
                            break;
                        case "CSharpScript":
                            newObjective.Count = Others.ToInt32(TBObjCount.Text);
                            newObjective.Script = TBObjMessage.Text;

                            break;
                    }
                    newObjective.Entry.Clear();
                    foreach (String st in TBObjEntry.Lines)
                    {
                        if (st != "")
                        {
                            newObjective.Entry.Add(Convert.ToInt32(st));
                        }
                    }
                    newObjective.Hotspots.Clear();
                    foreach (Point point  in LBObjHotspots.Items)
                    {
                        if (point != null)
                        {
                            newObjective.Hotspots.Add(point);
                        }
                    }

                    if (CBInternalObj.Checked)
                    {
                        newObjective.InternalQuestId = Others.ToInt32(CBObjInternalQuestIdManual.Checked ? CBObjInternalQuestID.Text : CBObjInternalQuestID.SelectedValue.ToString());
                    }

                    if (Information.IsNumeric(TBObjInternalIndex.Text))
                    {
                        newObjective.InternalIndex = uint.Parse(TBObjInternalIndex.Text);
                    }

                    newObjective.IgnoreQuestCompleted = CBObjIgnoreQuestCompleted.Checked;

                    newObjective.IsDead = CBObjIsDead.Checked;

                    Quest lastSelQuestx = _profile.Quests[_lastSelectedQuest.Index];

                    lastSelQuestx.Objectives.Add(newObjective);

                    var objectiveNode = new TreeNode(newObjective.Objective.ToString()) {Tag = "Objective"};

                    _lastSelectedQuest.Nodes.Add(objectiveNode);
                    TreeView.SelectedNode = objectiveNode;

                    if (_displayXml)
                    {
                        DisplayXMLs(newObjective);
                    }
                }
            }
            catch (Exception)
            {
                // Probablement plus intéressant d'afficher la vrai erreur dans les logs aussi.
                MessageBox.Show(@"Make sure the form is filled with numbers." + Constants.vbCrLf + @"Position/HostSpots ex: X;Y;Z");
            }
        }


        private void ButtonQuestNew_Click(object sender, EventArgs e)
        {
            ClearQuestForm();
            TreeView.SelectedNode = null;
        }


        private void ButtonQuestSave_Click(object sender, EventArgs e)
        {
            if (_profile != null)
            {
                if (TreeView.SelectedNode != null && (string) TreeView.SelectedNode.Tag == "Quest")
                {
                    //Modification


                    Quest quest = _profile.Quests[TreeView.SelectedNode.Index];


                    quest.Name = TBQuestQuestName.Text;
                    quest.Id = (Information.IsNumeric(TBQuestID.Text) ? Others.ToInt32(TBQuestID.Text) : 0);

                    int raceMask = 0;
                    foreach (ComboBoxValue item in CLBQuestRaceMask.CheckedItems)
                    {
                        raceMask += item.Value;
                    }

                    quest.RaceMask = raceMask;

                    int classMask = 0;
                    foreach (ComboBoxValue item in CLBQuestClassMask.CheckedItems)
                    {
                        classMask += item.Value;
                    }

                    quest.ClassMask = classMask;

                    quest.QuestLevel = (Information.IsNumeric(TBQuestLevel.Text) ? Others.ToInt32(TBQuestLevel.Text) : 0);
                    quest.MinLevel = (Information.IsNumeric(TBQuestMinLvl.Text) ? Others.ToInt32(TBQuestMinLvl.Text) : 0);
                    quest.MaxLevel = (Information.IsNumeric(TBQuestMaxLvl.Text) ? Others.ToInt32(TBQuestMaxLvl.Text) : 0);

                    quest.AutoAccepted = CBQuestAutoAccepted.Checked;

                    quest.AutoAccept.Clear();

                    foreach (string qInt in TBQuestAutoAcceptIDs.Lines)
                    {
                        if (qInt != string.Empty && Information.IsNumeric(qInt))
                        {
                            quest.AutoAccept.Add(Others.ToInt32(qInt));
                        }
                    }

                    quest.AutoComplete.Clear();

                    foreach (string qInt in TBQuestAutoCompleteIDs.Lines)
                    {
                        int qId = Others.ToInt32(qInt);
                        if (qId > 0)
                            quest.AutoComplete.Add(Others.ToInt32(qInt));
                    }
                    quest.NeedQuestCompletedId.Clear();

                    foreach (string qInt in TBQuestNeedQuestCompId.Lines)
                    {
                        int qId = Others.ToInt32(qInt);
                        if (qId > 0)
                            quest.NeedQuestCompletedId.Add(Others.ToInt32(qInt));
                    }

                    quest.NeedQuestNotCompletedId.Clear();

                    foreach (string qInt in TBQuestNeedQuestNotCompId.Lines)
                    {
                        int qId = Others.ToInt32(qInt);
                        if (qId > 0)
                            quest.NeedQuestNotCompletedId.Add(Others.ToInt32(qInt));
                    }


                    if (CBQuestWQ.Checked)
                    {
                        quest.WorldQuestLocation = new Point(float.Parse(TBQuestWQ.Text.Split(';')[0]), float.Parse(TBQuestWQ.Text.Split(';')[1]), float.Parse(TBQuestWQ.Text.Split(';')[2]));
                    }
                    else
                    {
                        if (CheckBoxItemPickUp.Checked)
                        {
                            quest.ItemPickUp = (Information.IsNumeric(TBQuestPickUpID.Text) ? Others.ToInt32(TBQuestPickUpID.Text) : 0);
                        }
                        else
                        {
                            quest.PickUp = (Information.IsNumeric(TBQuestPickUpID.Text) ? Others.ToInt32(TBQuestPickUpID.Text) : 0);
                        }

                        quest.TurnIn = (Information.IsNumeric(TBQuestTurnInID.Text) ? Others.ToInt32(TBQuestTurnInID.Text) : 0);
                    }
                    if (_displayXml)
                        DisplayXMLs(quest);
                    TreeView.SelectedNode.Text = TBQuestQuestName.Text + " (" + Quest.Id + ")";
                }
                else
                {
                    //New Quest
                    try
                    {
                        var newQuest = new Quest {Name = TBQuestQuestName.Text, Id = (Information.IsNumeric(TBQuestID.Text) ? Others.ToInt32(TBQuestID.Text) : 0)};


                        int raceMask = 0;
                        foreach (ComboBoxValue item in CLBQuestRaceMask.CheckedItems)
                        {
                            raceMask += item.Value;
                        }

                        newQuest.RaceMask = raceMask;

                        int classMask = 0;
                        foreach (ComboBoxValue item in CLBQuestClassMask.CheckedItems)
                        {
                            classMask += item.Value;
                        }

                        newQuest.ClassMask = classMask;

                        newQuest.QuestLevel = (Information.IsNumeric(TBQuestLevel.Text) ? Others.ToInt32(TBQuestLevel.Text) : 0);
                        newQuest.MinLevel = (Information.IsNumeric(TBQuestMinLvl.Text) ? Others.ToInt32(TBQuestMinLvl.Text) : 0);
                        newQuest.MaxLevel = (Information.IsNumeric(TBQuestMaxLvl.Text) ? Others.ToInt32(TBQuestMaxLvl.Text) : 0);


                        newQuest.AutoAccepted = CBQuestAutoAccepted.Checked;

                        newQuest.NeedQuestCompletedId.Clear();

                        foreach (String qInt in TBQuestNeedQuestCompId.Lines)
                        {
                            if (qInt != string.Empty && Information.IsNumeric(qInt))
                            {
                                newQuest.NeedQuestCompletedId.Add(Others.ToInt32(qInt));
                            }
                        }

                        newQuest.NeedQuestNotCompletedId.Clear();

                        foreach (String qInt in TBQuestNeedQuestNotCompId.Lines)
                        {
                            if (qInt != string.Empty && Information.IsNumeric(qInt))
                            {
                                newQuest.NeedQuestNotCompletedId.Add(Others.ToInt32(qInt));
                            }
                        }

                        newQuest.AutoAccept.Clear();

                        foreach (string qInt in TBQuestAutoAcceptIDs.Lines)
                        {
                            if (qInt != string.Empty && Information.IsNumeric(qInt))
                            {
                                newQuest.AutoAccept.Add(Others.ToInt32(qInt));
                            }
                        }

                        newQuest.AutoComplete.Clear();

                        foreach (string qInt in TBQuestAutoCompleteIDs.Lines)
                        {
                            if (qInt != string.Empty && Information.IsNumeric(qInt))
                            {
                                newQuest.AutoComplete.Add(Others.ToInt32(qInt));
                            }
                        }

                        if (CBQuestWQ.Checked)
                        {
                            newQuest.WorldQuestLocation = new Point(float.Parse(TBQuestWQ.Text.Split(';')[0]), float.Parse(TBQuestWQ.Text.Split(';')[1]), float.Parse(TBQuestWQ.Text.Split(';')[2]));
                        }
                        else
                        {
                            if (CheckBoxItemPickUp.Checked)
                            {
                                newQuest.ItemPickUp = (Information.IsNumeric(TBQuestPickUpID.Text) ? Others.ToInt32(TBQuestPickUpID.Text) : 0);
                            }
                            else
                            {
                                newQuest.PickUp = (Information.IsNumeric(TBQuestPickUpID.Text) ? Others.ToInt32(TBQuestPickUpID.Text) : 0);
                            }

                            newQuest.TurnIn = (Information.IsNumeric(TBQuestTurnInID.Text) ? Others.ToInt32(TBQuestTurnInID.Text) : 0);
                        }

                        _profile.Quests.Add(newQuest);

                        var questNode = new TreeNode(TBQuestQuestName.Text + " (" + newQuest.Id + ")") {Tag = "Quest"};

                        _questParentNode.Nodes.Add(questNode);
                        TreeView.SelectedNode = questNode;

                        if (_displayXml)
                        {
                            DisplayXMLs(newQuest);
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(@"Please make sure that the fields are filled with numbers!");
                    }
                }
            }
        }


        private void ButtonNewXML_Click(object sender, EventArgs e)
        {
            TreeView.Nodes.Clear();
            _npcParentNode.Nodes.Clear();
            _questParentNode.Nodes.Clear();
            _npcParentNode.Tag = "NPCs";
            _questParentNode.Tag = "Quests";
            ClearNPCForm();
            ClearQuestForm();

            _lastSelectedNpc = null;
            _lastSelectedObjective = null;
            _lastSelectedQuest = null;

            TreeView.Nodes.Add(_npcParentNode);
            TreeView.Nodes.Add(_questParentNode);
            _questParentNode.Expand();
            _fullpath = String.Empty;
            _profile = new QuesterProfile();
        }

        public void LoadNodes(string profile = "")
        {
            try
            {
                _fullpath = Application.StartupPath + @"\Profiles\Quester\" + profile;
                _profile = new QuesterProfile();

                if (string.IsNullOrEmpty(profile) || !File.Exists(_fullpath))
                {
                    string file = Others.DialogBoxOpenFile(Application.StartupPath + @"\Profiles\Quester\", "Profile files (*.xml)|*.xml|All files (*.*)|*.*");
                    if (File.Exists(file))
                    {
                        _fullpath = file;
                        _profile = XmlSerializer.Deserialize<QuesterProfile>(file);
                    }
                }
                else
                {
                    _profile = XmlSerializer.Deserialize<QuesterProfile>(_fullpath);
                }

                TreeView.Nodes.Clear();
                _npcParentNode.Nodes.Clear();
                _questParentNode.Nodes.Clear();
                _npcParentNode.Tag = "NPCs";
                TreeView.Nodes.Add(_npcParentNode);

                foreach (Npc vQuester in _profile.Questers)
                {
                    var npcNode = new TreeNode(vQuester.Name) {Tag = "NPC"};
                    _npcParentNode.Nodes.Add(npcNode);
                }

                _questParentNode.Tag = "Quests";
                TreeView.Nodes.Add(_questParentNode);

                foreach (Quest quest  in _profile.Quests)
                {
                    //QUEST
                    var questNode = new TreeNode(quest.Name + " (" + quest.Id + ")") {Tag = "Quest"};
                    _questParentNode.Nodes.Add(questNode);

                    //QUEST OBJECTIVES
                    foreach (QuestObjective questObjective in quest.Objectives)
                    {
                        var questObjectiveNode = new TreeNode(questObjective.Objective + " " + questObjective.QuestName) {Tag = "Objective"};

                        questNode.Nodes.Add(questObjectiveNode);
                    }
                }

                _questParentNode.Expand();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if ((string) e.Node.Tag == "NPC") //Npc Selected
            {
                PanelNPC.Visible = true;
                PanelSimpleQuest.Visible = false;

                Npc vquester = _profile.Questers[e.Node.Index];


                TBNpcName.Text = vquester.Name;
                TBNpcId.Text = vquester.Entry.ToString(CultureInfo.InvariantCulture);
                TBNpcPosition.Text = vquester.Position.X + ";" + vquester.Position.Y + ";" + vquester.Position.Z;
                CBNpcFaction.SelectedValue = Convert.ToInt32(string.Format("{0:D}", vquester.Faction));
                CBNpcType.SelectedValue = Convert.ToInt32(string.Format("{0:D}", vquester.Type));
                TBNpcContinentId.Text = vquester.ContinentId;
                _lastSelectedNpc = e.Node;
            }
            else if ((string) e.Node.Tag == "Quest") //Quest Selected
            {
                DisableObjForm();
                ClearQuestForm();

                PanelNPC.Visible = false;
                PanelSimpleQuest.Visible = true;
                TabControl1.SelectedTab = TabPageQuest;

                Quest quest = _profile.Quests[e.Node.Index];

                TBQuestQuestName.Text = quest.Name;
                TBQuestID.Text = quest.Id.ToString(CultureInfo.InvariantCulture);
                FillQuestMaskAfterObjSelection(quest.ClassMask, quest.RaceMask);

                //Quest Need Completed Ids
                TBQuestNeedQuestCompId.Text = "";
                if (quest.NeedQuestCompletedId.Count > 0)
                {
                    var intList = new List<string>();
                    foreach (int qInt in quest.NeedQuestCompletedId)
                    {
                        intList.Add(qInt.ToString(CultureInfo.InvariantCulture));
                    }
                    TBQuestNeedQuestCompId.Lines = intList.ToArray();
                }

                //Quest Need Not Completed Ids
                TBQuestNeedQuestNotCompId.Text = "";
                if (quest.NeedQuestNotCompletedId.Count > 0)
                {
                    var intList = new List<string>();
                    foreach (int qInt in quest.NeedQuestNotCompletedId)
                    {
                        intList.Add(qInt.ToString(CultureInfo.InvariantCulture));
                    }
                    TBQuestNeedQuestNotCompId.Lines = intList.ToArray();
                }

                //AutoAccept Quest Ids
                TBQuestAutoAcceptIDs.Text = "";
                if (quest.AutoAccept.Count > 0)
                {
                    var intList = new List<string>();
                    foreach (int qInt in quest.AutoAccept)
                    {
                        intList.Add(qInt.ToString(CultureInfo.InvariantCulture));
                    }
                    TBQuestAutoAcceptIDs.Lines = intList.ToArray();
                }

                //Auto Complete Quest Ids
                TBQuestAutoCompleteIDs.Text = "";
                if (quest.AutoComplete.Count > 0)
                {
                    var intList = new List<string>();
                    foreach (int qInt in quest.AutoComplete)
                    {
                        intList.Add(qInt.ToString(CultureInfo.InvariantCulture));
                    }
                    TBQuestAutoCompleteIDs.Lines = intList.ToArray();
                }

                CBQuestAutoAccepted.Checked = quest.AutoAccepted;

                TBQuestLevel.Text = quest.QuestLevel.ToString(CultureInfo.InvariantCulture);
                TBQuestMinLvl.Text = quest.MinLevel.ToString(CultureInfo.InvariantCulture);
                TBQuestMaxLvl.Text = quest.MaxLevel.ToString(CultureInfo.InvariantCulture);

                if (quest.WorldQuestLocation != null && quest.WorldQuestLocation.IsValid)
                {
                    TBQuestWQ.Text = quest.WorldQuestLocation.ToString();
                    CBQuestWQ.Checked = true;
                }
                else
                {
                    if (quest.ItemPickUp != 0)
                    {
                        CheckBoxItemPickUp.Checked = true;
                        TBQuestPickUpID.Text = quest.ItemPickUp.ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        CheckBoxItemPickUp.Checked = false;
                        TBQuestPickUpID.Text = quest.PickUp.ToString(CultureInfo.InvariantCulture);
                    }

                    TBQuestTurnInID.Text = quest.TurnIn.ToString(CultureInfo.InvariantCulture);
                }

                _lastSelectedQuest = e.Node;

                if (_displayXml)
                {
                    DisplayXMLs(quest);
                }
            }
            else if ((string) e.Node.Tag == "Objective" || (string) e.Node.Tag == "NewObjective") //Objective Selected
            {
                QuestObjective questObjective = _profile.Quests[e.Node.Parent.Index].Objectives[e.Node.Index];

                TabControl1.SelectedTab = TabPageObjectives;
                _lastSelectedObjective = e.Node;
                _lastSelectedQuest = e.Node.Parent;

                if (_displayXml)
                {
                    DisplayXMLs(questObjective);
                }

                FillObjectiveFormByType(questObjective, _profile.Quests[e.Node.Parent.Index]);
            }
        }

        public void DisplayXMLs(object objet)
        {
            var xmldoc = new XmlDocument();
            var ser = new System.Xml.Serialization.XmlSerializer(objet.GetType());
            var sww = new StringWriter();
            using (XmlWriter writer = XmlWriter.Create(sww))
            {
                ser.Serialize(writer, objet);
                xmldoc.LoadXml(sww.ToString());
            }

            XElement questXml = XElement.Load(new XmlNodeReader(xmldoc));

            questXml.RemoveAttributes();
            UcXmlRichTextBox1.Xml = questXml.ToString();
        }

        public void DisableObjForm()
        {
            TBObjCount.Enabled = false;
            TBObjEntry.Enabled = false;
            TBObjCollectItemID.Enabled = false;
            TBObjCollectCount.Enabled = false;
            TBObjUseItemID.Enabled = false;
            TBObjPosition.Enabled = false;
            TBObjWaitMs.Enabled = false;
            TBObjRange.Enabled = false;
            TBObjUseSpellId.Enabled = false;
            TBObjNPCId.Enabled = false;
            TBObjQuestID.Enabled = false;
            TBObjQuestName.Enabled = false;
            CBObjIgnoreQuestCompleted.Checked = false;
            TBObjDestinationX.Enabled = false;
            TBObjDestinationY.Enabled = false;
            TBObjTaxiEntryId.Enabled = false;
            CBObjInternalQuestID.Enabled = false;
            CBObjKillMobPickUpItem.Visible = false;
            CBObjCanPullUnitsInFight.Enabled = false;
            CBInternalObj.Enabled = false;
            TBObjGossipOption.Enabled = false;
            CBObjInternalQuestIdManual.Checked = false;
            CBInternalObj.CheckedChanged -= CBInternalObj_CheckedChanged;
            CBInternalObj.Checked = false;
            CBInternalObj.CheckedChanged += CBInternalObj_CheckedChanged;
            CBObjIsDead.Checked = false;
            TBObjMessage.Enabled = false;
            CBObjPressKeys.Enabled = false;

            TBObjCount.Text = string.Empty;
            TBObjEntry.Text = string.Empty;
            TBObjCollectItemID.Text = string.Empty;
            TBObjCollectCount.Text = string.Empty;
            TBObjUseItemID.Text = string.Empty;
            TBObjPosition.Text = string.Empty;
            TBObjWaitMs.Text = string.Empty;
            TBObjRange.Text = string.Empty;
            TBObjUseSpellId.Text = string.Empty;
            TBObjNPCId.Text = string.Empty;
            TBObjQuestID.Text = string.Empty;
            TBObjQuestName.Text = string.Empty;
            TBObjDestinationX.Text = string.Empty;
            TBObjDestinationY.Text = string.Empty;
            TBObjTaxiEntryId.Text = string.Empty;
            CBObjInternalQuestID.DataSource = null;
            CBObjCanPullUnitsInFight.Checked = false;
            LabelObjNPCIDorName.Text = @"NPC Id";
            CBInternalObj.Checked = false;
            TBObjGossipOption.Text = string.Empty;
            TBObjInternalIndex.Text = string.Empty;
            CBObjKillMobPickUpItem.CheckedChanged -= CBObjKillMobPickUpItem_CheckedChanged;
            CBObjKillMobPickUpItem.Checked = false;
            CBObjKillMobPickUpItem.CheckedChanged += CBObjKillMobPickUpItem_CheckedChanged;
            LBObjHotspots.Items.Clear();
            TBObjMessage.Text = string.Empty;
            CBObjPressKeys.Text = string.Empty;
        }


        public void ClearObjectiveForm()
        {
        }

        public void ClearQuestForm()
        {
            ClearMaskListBox();

            TBQuestQuestName.Text = string.Empty;
            TBQuestID.Text = string.Empty;
            CBClassMask.SelectedValue = 0;
            CBRaceMask.SelectedValue = 0;
            TBQuestLevel.Text = string.Empty;
            TBQuestMinLvl.Text = string.Empty;
            TBQuestMaxLvl.Text = string.Empty;
            CheckBoxItemPickUp.Checked = false;
            TBQuestPickUpID.Text = string.Empty;
            TBQuestTurnInID.Text = string.Empty;
            TBQuestNeedQuestCompId.Text = string.Empty;
            TBQuestNeedQuestNotCompId.Text = string.Empty;
            TBQuestWQ.Text = string.Empty;
            CBQuestWQ.Checked = false;
            CBQuestAutoAccepted.Checked = false;
            TBQuestWQ.Enabled = false;
            TBQuestAutoAcceptIDs.Text = string.Empty;
            TBQuestAutoCompleteIDs.Text = string.Empty;
        }

        public void ClearNPCForm()
        {
            TBNpcName.Text = string.Empty;
            TBNpcId.Text = string.Empty;
            TBNpcPosition.Text = string.Empty;
            TBNpcContinentId.Text = string.Empty;
        }


        public void FillObjectiveFormByType(QuestObjective qObjective, Quest quest)
        {
            DisableObjForm();

            int cbSelectValue;

            cbSelectValue = (int) qObjective.Objective;
            switch (qObjective.Objective.ToString())
            {
                case "KillMob":
                    CBObjKillMobPickUpItem.Visible = true;
                    TBObjEntry.Enabled = true;
                    if (qObjective.CollectItemId > 0)
                    {
                        TBObjCollectCount.Enabled = true;
                        TBObjCollectItemID.Enabled = true;
                        TBObjCollectCount.Text = qObjective.CollectCount.ToString(CultureInfo.InvariantCulture);
                        TBObjCollectItemID.Text = qObjective.CollectItemId.ToString(CultureInfo.InvariantCulture);
                        cbSelectValue = (int) (Objective.KillMob);
                        CBObjKillMobPickUpItem.Checked = true;
                    }
                    else
                    {
                        TBObjCount.Enabled = true;
                        TBObjCount.Text = qObjective.Count.ToString(CultureInfo.InvariantCulture);
                    }

                    CBObjCanPullUnitsInFight.Enabled = true;
                    CBObjCanPullUnitsInFight.Checked = qObjective.CanPullUnitsAlreadyInFight;

                    break;
                case "BuyItem":

                    TBObjEntry.Enabled = true;
                    TBObjCollectCount.Enabled = true;
                    TBObjCollectItemID.Enabled = true;
                    TBObjNPCId.Enabled = true;
                    TBObjPosition.Enabled = true;
                    LabelObjNPCIDorName.Text = @"NPC Name";

                    TBObjCollectCount.Text = qObjective.CollectCount.ToString(CultureInfo.InvariantCulture);
                    TBObjCollectItemID.Text = qObjective.CollectItemId.ToString(CultureInfo.InvariantCulture);
                    TBObjNPCId.Text = qObjective.Name;
                    TBObjPosition.Text = qObjective.Position.ToString();

                    break;
                case "PickUpObject":
                    TBObjCollectCount.Enabled = true;
                    TBObjCollectItemID.Enabled = true;
                    TBObjEntry.Enabled = true;


                    TBObjCollectCount.Text = qObjective.CollectCount.ToString(CultureInfo.InvariantCulture);
                    TBObjCollectItemID.Text = qObjective.CollectItemId.ToString(CultureInfo.InvariantCulture);

                    break;
                case "UseItem":
                    TBObjUseItemID.Enabled = true;
                    TBObjCount.Enabled = true;
                    TBObjPosition.Enabled = true;
                    TBObjEntry.Enabled = true;
                    TBObjWaitMs.Enabled = true;
                    TBObjRange.Enabled = true;

                    TBObjUseItemID.Text = qObjective.UseItemId.ToString(CultureInfo.InvariantCulture);
                    TBObjCount.Text = qObjective.Count.ToString(CultureInfo.InvariantCulture);
                    TBObjPosition.Text = qObjective.Position.ToString();
                    TBObjWaitMs.Text = qObjective.WaitMs.ToString(CultureInfo.InvariantCulture);
                    TBObjRange.Text = qObjective.Range.ToString(CultureInfo.InvariantCulture);

                    break;
                case "UseItemAOE":
                    TBObjUseItemID.Enabled = true;
                    TBObjCount.Enabled = true;
                    TBObjPosition.Enabled = true;
                    TBObjEntry.Enabled = true;
                    TBObjWaitMs.Enabled = true;
                    TBObjRange.Enabled = true;

                    TBObjUseItemID.Text = qObjective.UseItemId.ToString(CultureInfo.InvariantCulture);
                    TBObjCount.Text = qObjective.Count.ToString(CultureInfo.InvariantCulture);
                    TBObjPosition.Text = qObjective.Position.ToString();
                    TBObjWaitMs.Text = qObjective.WaitMs.ToString(CultureInfo.InvariantCulture);
                    TBObjRange.Text = qObjective.Range.ToString(CultureInfo.InvariantCulture);

                    break;
                case "UseSpell":
                    //TODO ENTRY + NAME + POSITION
                    TBObjUseSpellId.Enabled = true;
                    TBObjPosition.Enabled = true;
                    TBObjWaitMs.Enabled = true;

                    TBObjPosition.Text = qObjective.Position.ToString();
                    TBObjUseSpellId.Text = qObjective.UseSpellId.ToString(CultureInfo.InvariantCulture);
                    TBObjWaitMs.Text = qObjective.WaitMs.ToString(CultureInfo.InvariantCulture);

                    break;
                case "UseSpellAOE":
                    TBObjUseSpellId.Enabled = true;
                    TBObjPosition.Enabled = true;
                    TBObjRange.Enabled = true;
                    TBObjWaitMs.Enabled = true;

                    TBObjPosition.Text = qObjective.Position.ToString();
                    TBObjUseSpellId.Text = qObjective.UseSpellId.ToString(CultureInfo.InvariantCulture);
                    TBObjWaitMs.Text = qObjective.WaitMs.ToString(CultureInfo.InvariantCulture);
                    TBObjRange.Text = qObjective.Range.ToString(CultureInfo.InvariantCulture);

                    break;
                case "InteractWith":
                    TBObjEntry.Enabled = true;
                    TBObjPosition.Enabled = true;
                    TBObjWaitMs.Enabled = true;
                    TBObjGossipOption.Enabled = true;

                    TBObjPosition.Text = qObjective.Position.ToString();
                    TBObjWaitMs.Text = qObjective.WaitMs.ToString(CultureInfo.InvariantCulture);
                    TBObjGossipOption.Text = qObjective.GossipOptionsInteractWith.ToString(CultureInfo.InvariantCulture);
                    break;
                case "MoveTo":
                    TBObjPosition.Enabled = true;

                    TBObjPosition.Text = qObjective.Position.ToString();
                    break;
                case "Wait":
                    TBObjWaitMs.Enabled = true;

                    TBObjWaitMs.Text = qObjective.WaitMs.ToString(CultureInfo.InvariantCulture);

                    break;
                case "PickUpQuest":
                    TBObjNPCId.Enabled = true;
                    TBObjQuestID.Enabled = true;
                    TBObjQuestName.Enabled = true;
                    CBObjIgnoreQuestCompleted.Enabled = true;
                    CBInternalObj.Enabled = false;

                    TBObjNPCId.Text = qObjective.NpcEntry.ToString(CultureInfo.InvariantCulture);
                    TBObjQuestID.Text = qObjective.QuestId.ToString(CultureInfo.InvariantCulture);
                    TBObjQuestName.Text = qObjective.QuestName;

                    break;
                case "TurnInQuest":
                    TBObjNPCId.Enabled = true;
                    TBObjQuestID.Enabled = true;
                    TBObjQuestName.Enabled = true;
                    CBObjIgnoreQuestCompleted.Enabled = true;
                    CBInternalObj.Enabled = false;

                    TBObjNPCId.Text = qObjective.NpcEntry.ToString(CultureInfo.InvariantCulture);
                    TBObjQuestID.Text = qObjective.QuestId.ToString(CultureInfo.InvariantCulture);
                    TBObjQuestName.Text = qObjective.QuestName;

                    break;
                case "UseFlightPath":
                    TBObjTaxiEntryId.Enabled = true;
                    TBObjDestinationY.Enabled = true;
                    TBObjDestinationX.Enabled = true;

                    TBObjTaxiEntryId.Text = qObjective.TaxiEntry.ToString(CultureInfo.InvariantCulture);
                    TBObjDestinationY.Text = qObjective.FlightDestinationY;
                    TBObjDestinationX.Text = qObjective.FlightDestinationX;
                    TBObjFlightWaitMs.Text = qObjective.WaitMs.ToString(CultureInfo.InvariantCulture);
                    break;
                case "PickUpNPC":
                    TBObjCount.Enabled = true;
                    TBObjEntry.Enabled = true;
                    CBObjCanPullUnitsInFight.Enabled = true;
                    TBObjGossipOption.Enabled = true;
                    TBObjWaitMs.Enabled = true;

                    CBObjCanPullUnitsInFight.Checked = qObjective.CanPullUnitsAlreadyInFight;
                    TBObjCount.Text = qObjective.Count.ToString(CultureInfo.InvariantCulture);
                    TBObjGossipOption.Text = qObjective.GossipOptionsInteractWith.ToString(CultureInfo.InvariantCulture);
                    TBObjWaitMs.Text = qObjective.WaitMs.ToString(CultureInfo.InvariantCulture);
                    break;
                case "UseVehicle":
                    TBObjPosition.Enabled = true;
                    TBObjEntry.Enabled = true;
                    TBObjPosition.Text = qObjective.Position.ToString();
                    break;
                case "ClickOnTerrain":
                    TBObjPosition.Enabled = true;
                    TBObjWaitMs.Enabled = true;

                    TBObjPosition.Text = qObjective.Position.ToString();
                    TBObjWaitMs.Text = qObjective.WaitMs.ToString(CultureInfo.InvariantCulture);
                    break;
                case "MessageBox":
                    TBObjMessage.Enabled = true;
                    TBObjMessage.Text = qObjective.Message;
                    break;
                case "PressKey":
                    CBObjPressKeys.SelectedValue = (int) qObjective.Keys;
                    TBObjCount.Text = qObjective.Count.ToString(CultureInfo.InvariantCulture);
                    TBObjWaitMs.Text = qObjective.WaitMs.ToString(CultureInfo.InvariantCulture);
                    TBObjPosition.Text = qObjective.Position.ToString();
                    CBObjPressKeys.Enabled = true;
                    TBObjCount.Enabled = true;
                    TBObjWaitMs.Enabled = true;
                    TBObjPosition.Enabled = true;
                    break;
                case "CSharpScript":
                    TBObjCount.Text = qObjective.Count.ToString(CultureInfo.InvariantCulture);
                    TBObjCount.Enabled = true;
                    TBObjMessage.Text = qObjective.Script;
                    TBObjMessage.Enabled = true;

                    break;
            }

            switch (qObjective.Objective.ToString())
            {
                case "UseFlightPath":
                    PanelObjAll.Visible = false;
                    PanelObjTaxi.Visible = true;
                    break;
                default:
                    PanelObjAll.Visible = true;
                    PanelObjTaxi.Visible = false;
                    break;
            }

            CBObjIgnoreQuestCompleted.Checked = qObjective.IgnoreQuestCompleted;
            CBObjIsDead.Checked = qObjective.IsDead;

            CBObjType.SelectedValueChanged -= CBObjType_SelectedValueChanged;
            CBObjType.SelectedValue = cbSelectValue;
            CBObjType.SelectedValueChanged += CBObjType_SelectedValueChanged;

            CBObjInternalQuestID.Enabled = (qObjective.InternalQuestId > 0);

            TBObjInternalIndex.Text = qObjective.InternalIndex.ToString(CultureInfo.InvariantCulture);

            if (qObjective.Objective.ToString() != "TurnInQuest" && qObjective.Objective.ToString() != "PickUpQuest")
            {
                CBInternalObj.Enabled = true;
                CBInternalObj.Checked = (qObjective.InternalQuestId > 0);
            }

            //Fill HotSpots
            if (qObjective.Hotspots.Count > 0)
            {
                foreach (Point hPoint in qObjective.Hotspots)
                {
                    LBObjHotspots.Items.Add(hPoint);
                }
            }

            //Fill Entry
            if (qObjective.Entry.Count > 0 && qObjective.Objective.ToString() != "UseVehicle")
            {
                var intList = new List<string>();
                foreach (int ent in qObjective.Entry)
                {
                    intList.Add(ent.ToString(CultureInfo.InvariantCulture));
                }

                TBObjEntry.Lines = intList.ToArray();
            }


            if (CBInternalObj.Checked)
            {
                var listQuest = new List<ComboBoxValue>();

                foreach (QuestObjective obj in quest.Objectives)
                {
                    if (obj.Objective == Objective.PickUpQuest)
                    {
                        listQuest.Add(new ComboBoxValue
                        {
                            Name = obj.QuestName + " " + obj.QuestId,
                            Value = obj.QuestId
                        });
                    }
                }

                CBObjInternalQuestID.DataSource = listQuest;

                CBObjInternalQuestID.ValueMember = "Value";
                CBObjInternalQuestID.DisplayMember = "Name";
                ComboBoxValue var1 = listQuest.Find(x => x.Value == qObjective.InternalQuestId);
                if (var1 != null)
                {
                    CBObjInternalQuestID.SelectedValue = qObjective.InternalQuestId;
                }
                else
                {
                    CBObjInternalQuestID.Text = qObjective.InternalQuestId.ToString(CultureInfo.InvariantCulture);
                    CBObjInternalQuestIdManual.Checked = true;
                }
            }
        }


        public void PopulateComboBox()
        {
            //None = 0
            //ApplyBuff = 1
            //BuyItem = 2
            //EjectVehicle = 3
            //EquipItem = 4
            //InteractWith = 5
            //KillMob = 6
            //MoveTo = 7
            //PickUpObject = 8
            //PickUpQuest = 9
            //PressKey = 10
            //UseItem = 11
            //UseLuaMacro = 12
            //TurnInQuest = 13
            //UseFlightPath = 14
            //UseItemAOE = 15
            //UseActionButtonOnUnit = 16
            //UseRuneForge = 17
            //UseSpell = 18
            //UseSpellAOE = 19
            //UseVehicle = 20
            //Wait = 21
            //TravelTo = 22
            //ClickOnTerrain = 23
            //MessageBox = 24
            //PickUpNPC = 25
            //GarrisonHearthstone = 26
            //CSharpScript = 27,

            var cbObjTypeList = new List<ComboBoxValueString>();

            cbObjTypeList.Add(new ComboBoxValueString
            {
                Name = "Killing mobs",
                Value = 6
            });
            cbObjTypeList.Add(new ComboBoxValueString
            {
                Name = "Gathering Items",
                Value = 8
            });
            cbObjTypeList.Add(new ComboBoxValueString
            {
                Name = "Use an Item",
                Value = 11
            });
            cbObjTypeList.Add(new ComboBoxValueString
            {
                Name = "Pickup Quest",
                Value = 9
            });
            cbObjTypeList.Add(new ComboBoxValueString
            {
                Name = "Turnin Quest",
                Value = 13
            });
            cbObjTypeList.Add(new ComboBoxValueString
            {
                Name = "Interacting with a gameobject",
                Value = 5
            });
            cbObjTypeList.Add(new ComboBoxValueString
            {
                Name = "Going Somewhere",
                Value = 7
            });
            cbObjTypeList.Add(new ComboBoxValueString
            {
                Name = "Just Wait",
                Value = 21
            });
            cbObjTypeList.Add(new ComboBoxValueString
            {
                Name = "PickUp Npc",
                Value = 25
            });
            cbObjTypeList.Add(new ComboBoxValueString
            {
                Name = "Buying Item",
                Value = 2
            });
            cbObjTypeList.Add(new ComboBoxValueString
            {
                Name = "Use an Item AOE",
                Value = 15
            });
            cbObjTypeList.Add(new ComboBoxValueString
            {
                Name = "Casting a spell",
                Value = 18
            });
            cbObjTypeList.Add(new ComboBoxValueString
            {
                Name = "Casting a spell AOE",
                Value = 19
            });
            cbObjTypeList.Add(new ComboBoxValueString
            {
                Name = "Use Flight",
                Value = 14
            });
            cbObjTypeList.Add(new ComboBoxValueString
            {
                Name = "Use Vehicle",
                Value = 20
            });
            cbObjTypeList.Add(new ComboBoxValueString
            {
                Name = "Eject Vehicle",
                Value = 3
            });
            cbObjTypeList.Add(new ComboBoxValueString
            {
                Name = "Click On Terrain",
                Value = 23
            });
            cbObjTypeList.Add(new ComboBoxValueString
            {
                Name = "Message",
                Value = 24
            });
            cbObjTypeList.Add(new ComboBoxValueString
            {
                Name = "Press Key",
                Value = 10
            });
            cbObjTypeList.Add(new ComboBoxValueString
            {
                Name = "CSharpScript",
                Value = 27
            });
            CBObjType.DataSource = cbObjTypeList;

            CBObjType.ValueMember = "Value";
            CBObjType.DisplayMember = "Name";

            //NPC TYPE
            var npcType = new List<ComboBoxValue>();

            npcType.Add(new ComboBoxValue
            {
                Name = "QuestGiver",
                Value = 44
            });
            npcType.Add(new ComboBoxValue
            {
                Name = "FlightMaster",
                Value = 35
            });


            CBNpcType.DataSource = npcType;
            CBNpcType.ValueMember = "Value";
            CBNpcType.DisplayMember = "Name";

            var factL = new List<ComboBoxValue>();

            foreach (object st in Enum.GetValues(typeof (Npc.FactionType)))
            {
                factL.Add(new ComboBoxValue
                {
                    Name = st.ToString(),
                    Value = Convert.ToInt32(st)
                });
            }

            CBNpcFaction.DataSource = factL;
            CBNpcFaction.ValueMember = "Value";
            CBNpcFaction.DisplayMember = "Name";

            foreach (String st in Enum.GetNames(typeof (WoWClassMask)))
            {
                int classValue = Convert.ToInt32(string.Format("{0:D}", Enum.Parse(typeof (WoWClassMask), st)));

                CLBQuestClassMask.Items.Add(new ComboBoxValue
                {
                    Name = st,
                    Value = classValue
                });
            }

            CLBQuestClassMask.DisplayMember = "Name";
            CLBQuestClassMask.ValueMember = "Value";
            foreach (String st in Enum.GetNames(typeof (WoWRace)))
            {
                switch (st)
                {
                    case "Human":
                    case "Orc":
                    case "Dwarf":
                    case "NightElf":
                    case "Undead":
                    case "Tauren":
                    case "Gnome":
                    case "Troll":
                    case "Goblin":
                    case "BloodElf":
                    case "Draenei":
                    case "Worgen":
                    case "PandarenAliance":
                    case "PandarenHorde":
                        int raceValue = Convert.ToInt32(string.Format("{0:D}", Enum.Parse(typeof (WoWRace), st)));
                        int exp = raceValue - 1;
                        uint raceMask = (exp >= 0 ? (uint) (Math.Pow(2, exp)) : 0);
                        CLBQuestRaceMask.Items.Add(new ComboBoxValue
                        {
                            Name = st,
                            Value = (int) raceMask
                        });
                        break;
                }
            }
            CLBQuestRaceMask.DisplayMember = "Name";
            CLBQuestRaceMask.ValueMember = "Value";


            var pressKeysList = new List<ComboBoxValue>();

            foreach (object st in Enum.GetValues(typeof (Keybindings)))
            {
                pressKeysList.Add(new ComboBoxValue
                {
                    Name = st.ToString(),
                    Value = Convert.ToInt32(st)
                });
            }

            CBObjPressKeys.DataSource = pressKeysList;
            CBObjPressKeys.DisplayMember = "Name";
            CBObjPressKeys.ValueMember = "Value";
        }

        public string GetSelectedObjectiveTypeName()
        {
            int cbObjSelValue;
            if (CBObjType.SelectedValue is ComboBoxValueString)
            {
                cbObjSelValue = (CBObjType.SelectedValue as ComboBoxValueString).Value;
            }
            else if (CBObjType.SelectedValue is ComboBoxValue)
            {
                cbObjSelValue = (CBObjType.SelectedValue as ComboBoxValue).Value;
            }
            else
            {
                cbObjSelValue = Others.ToInt32(CBObjType.SelectedValue.ToString());
            }

            string selectedObjectiveName = Enum.GetName(typeof (Objective), cbObjSelValue);
            return selectedObjectiveName;
        }

        private void CBObjType_SelectedValueChanged(object sender, EventArgs e)
        {
            DisableObjForm();
            if (TreeView.SelectedNode != null && (string) TreeView.SelectedNode.Tag != "NewObjective")
            {
                TreeView.SelectedNode = null;
            }


            //  cbSelectValue = [Enum].Parse(GetType(ObjectivesEnum), .Element("Objective"))


            string selectedObjectiveName = GetSelectedObjectiveTypeName();

            switch (selectedObjectiveName)
            {
                case "KillMob":
                    TBObjEntry.Enabled = true;
                    CBObjKillMobPickUpItem.Visible = true;
                    TBObjCount.Enabled = true;
                    CBObjCanPullUnitsInFight.Enabled = true;
                    break;
                case "BuyItem":

                    TBObjEntry.Enabled = true;
                    TBObjCollectCount.Enabled = true;
                    TBObjCollectItemID.Enabled = true;
                    TBObjNPCId.Enabled = true;
                    TBObjPosition.Enabled = true;
                    LabelObjNPCIDorName.Text = @"NPC Name";
                    break;
                case "PickUpObject":
                    TBObjCollectCount.Enabled = true;
                    TBObjCollectItemID.Enabled = true;
                    TBObjEntry.Enabled = true;

                    break;


                case "UseItem":
                    TBObjUseItemID.Enabled = true;
                    TBObjCount.Enabled = true;
                    TBObjPosition.Enabled = true;
                    TBObjEntry.Enabled = true;
                    TBObjWaitMs.Enabled = true;
                    TBObjRange.Enabled = true;
                    break;
                case "UseItemAOE":
                    TBObjUseItemID.Enabled = true;
                    TBObjCount.Enabled = true;
                    TBObjPosition.Enabled = true;
                    TBObjEntry.Enabled = true;
                    TBObjWaitMs.Enabled = true;
                    TBObjRange.Enabled = true;

                    break;
                case "UseSpell":
                    //TODO ENTRY + NAME + POSITION
                    TBObjUseSpellId.Enabled = true;
                    TBObjPosition.Enabled = true;
                    TBObjWaitMs.Enabled = true;

                    break;

                case "UseSpellAOE":
                    TBObjUseSpellId.Enabled = true;
                    TBObjPosition.Enabled = true;
                    TBObjRange.Enabled = true;
                    TBObjWaitMs.Enabled = true;

                    break;


                case "InteractWith":
                    TBObjEntry.Enabled = true;
                    TBObjPosition.Enabled = true;
                    TBObjWaitMs.Enabled = true;
                    TBObjGossipOption.Enabled = true;
                    break;
                case "MoveTo":
                    TBObjPosition.Enabled = true;

                    break;
                case "Wait":
                    TBObjWaitMs.Enabled = true;
                    break;
                case "PickUpQuest":

                    TBObjNPCId.Enabled = true;
                    TBObjQuestID.Enabled = true;
                    TBObjQuestName.Enabled = true;

                    break;
                case "TurnInQuest":
                    TBObjNPCId.Enabled = true;
                    TBObjQuestID.Enabled = true;
                    TBObjQuestName.Enabled = true;

                    break;
                case "UseFlightPath":
                    TBObjTaxiEntryId.Enabled = true;
                    TBObjDestinationY.Enabled = true;
                    TBObjDestinationX.Enabled = true;

                    break;
                case "PickUpNPC":
                    TBObjCount.Enabled = true;
                    TBObjEntry.Enabled = true;
                    CBObjCanPullUnitsInFight.Enabled = true;
                    TBObjGossipOption.Enabled = true;
                    TBObjWaitMs.Enabled = true;
                    break;
                case "UseVehicle":
                    TBObjPosition.Enabled = true;
                    TBObjEntry.Enabled = true;

                    break;
                case "ClickOnTerrain":
                    TBObjPosition.Enabled = true;
                    TBObjWaitMs.Enabled = true;

                    break;
                case "MessageBox":
                    TBObjMessage.Enabled = true;
                    break;
                case "PressKey":
                    CBObjPressKeys.Enabled = true;
                    TBObjCount.Enabled = true;
                    TBObjWaitMs.Enabled = true;
                    TBObjPosition.Enabled = true;
                    break;
                case "CSharpScript":
                    TBObjCount.Enabled = true;
                    TBObjMessage.Enabled = true;
                    break;
            }

            switch (selectedObjectiveName)
            {
                case "UseFlightPath":
                    PanelObjAll.Visible = false;
                    PanelObjTaxi.Visible = true;
                    break;
                default:
                    PanelObjAll.Visible = true;
                    PanelObjTaxi.Visible = false;
                    break;
            }

            CBInternalObj.Enabled = (selectedObjectiveName != "TurnInQuest" && selectedObjectiveName != "PickUpQuest");
        }

        private void CBObjKillMobPickUpItem_CheckedChanged(object sender, EventArgs e)
        {
            if (CBObjKillMobPickUpItem.Checked)
            {
                TBObjEntry.Enabled = true;
                TBObjCollectCount.Enabled = true;
                TBObjCollectItemID.Enabled = true;
                TBObjCollectItemID.Text = @"1";
                TBObjCount.Enabled = false;
            }
            else
            {
                TBObjCount.Enabled = true;

                TBObjCollectItemID.Text = string.Empty;
                TBObjCollectCount.Enabled = false;
                TBObjCollectItemID.Enabled = false;
            }
            TBObjEntry.Enabled = true;
        }

        private void CBInternalObj_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (CBInternalObj.Checked)
                {
                    CBObjInternalQuestID.Enabled = true;

                    //Load the list of obj with PickUp Quest
                    var listQuest = new List<ComboBoxValue>();

                    for (int i = 0; i < _profile.Quests[_lastSelectedQuest.Index].Objectives.Count; i++)
                    {
                        QuestObjective obj = _profile.Quests[_lastSelectedQuest.Index].Objectives[i];
                        if (obj.Objective == Objective.PickUpQuest)
                        {
                            listQuest.Add(new ComboBoxValue
                            {
                                Name = obj.QuestName + " " + obj.QuestId,
                                Value = obj.QuestId
                            });
                        }
                    }

                    CBObjInternalQuestID.DataSource = listQuest;

                    CBObjInternalQuestID.ValueMember = "Value";
                    CBObjInternalQuestID.DisplayMember = "Name";
                    CBObjIgnoreQuestCompleted.Checked = true;
                }
                else
                {
                    CBObjInternalQuestID.Enabled = false;
                    CBObjInternalQuestID.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                //Catch Form Load triggering this Sub....and crashing
            }
        }


        private void ButtonObjImportFromGame_Click(object sender, EventArgs e)
        {
            //   If lastSelectedObjective IsNot Nothing Then

            if (ObjectManager.Target.IsValid && QuestersDB.GetNpcByEntry(ObjectManager.Target.Entry).Entry == 0)
            {
                if (MessageBox.Show("This Quest Giver isnt in the DB. Do you want to Add it?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ButtonNewNPC_Click(null, null);
                    ButtonNpcImport_Click(null, null);
                    ButtonSaveNPC_Click(null, null);
                }
            }

            WoWGameObject wowGOv = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectOfType(WoWGameObjectType.Questgiver));
            if (wowGOv.Entry > 0 && ObjectManager.Me.Position.DistanceTo(wowGOv.Position) < 5f && QuestersDB.GetNpcByEntry(wowGOv.Entry).Entry == 0)
            {
                if (MessageBox.Show(@"This Quest Giver (Object) isnt in the DB. Do you want to Add it?", @"Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ButtonNewNPC_Click(null, null);
                    ButtonNpcImport_Click(null, null);
                    ButtonSaveNPC_Click(null, null);
                }
            }


            if (ObjectManager.Target.IsNpcQuestGiver)
            {
                switch (GetSelectedObjectiveTypeName())
                {
                    case "PickUpQuest":
                    case "TurnInQuest":

                        TBObjNPCId.Text = ObjectManager.Target.Entry.ToString(CultureInfo.InvariantCulture);
                        string randomString = Others.GetRandomString(Others.Random(4, 10));
                        TBObjQuestID.Text = Lua.LuaDoString(randomString + " = GetQuestID()", randomString);
                        TBObjQuestName.Text = Lua.LuaDoString(randomString + "= GetTitleText()", randomString);

                        break;
                }
            }
            else
            {
                WoWGameObject wowGO = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectOfType(WoWGameObjectType.Questgiver));
                if (wowGO.Entry > 0 && ObjectManager.Me.Position.DistanceTo(wowGO.Position) < 5f)
                {
                    TBObjNPCId.Text = wowGO.Entry.ToString(CultureInfo.InvariantCulture);
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    TBObjQuestID.Text = Lua.LuaDoString(randomString + " = GetQuestID()", randomString);
                    TBObjQuestName.Text = Lua.LuaDoString(randomString + " = GetTitleText()", randomString);
                }
            }


            //Fill Count 
            if (GetSelectedObjectiveTypeName() == "KillMob" & CBObjKillMobPickUpItem.Checked == false)
            {
                int qId = 0;
                string count = "";
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                if (CBInternalObj.Checked & CBObjInternalQuestID.SelectedValue != null)
                {
                    qId = (int) CBObjInternalQuestID.SelectedValue;
                    //count = Lua.LuaDoString("text, objectiveType, finishedd,currentStatut,finishStatut= GetQuestObjectiveInfo(" + qId + ",1,false)", "finishStatut");
                    count = Lua.LuaDoString("_, _, _, _," + randomString + "= GetQuestObjectiveInfo(" + qId + ",1,false)", randomString);
                }
                else
                {
                    qId = _profile.Quests[_lastSelectedQuest.Index].Id;
                    //count = Lua.LuaDoString("text, objectiveType, finishedd,currentStatut,finishStatut= GetQuestObjectiveInfo(" + qId + ",1,false)", "finishStatut");
                    count = Lua.LuaDoString("_, _, _, _," + randomString + "= GetQuestObjectiveInfo(" + qId + ",1,false)", randomString);
                }

                if (CBObjKillMobPickUpItem.Checked == false)
                {
                    TBObjCount.Text = count;
                }
                else
                {
                    TBObjCollectCount.Text = count;
                }
            }


            if (ObjectManager.Target.IsNpcFlightMaster && GetSelectedObjectiveTypeName() == "UseFlightPath")
            {
                TBObjTaxiEntryId.Text = ObjectManager.Target.Entry.ToString(CultureInfo.InvariantCulture);
            }
            // End If
        }

        private void ButtonObjGetXY_Click(object sender, EventArgs e)
        {
            WoWUnit unit = default(WoWUnit);
            if (ObjectManager.Target.IsNpcFlightMaster)
            {
                unit = ObjectManager.Target;
            }
            else
            {
                if (ObjectManager.Me.Position.DistanceTo(ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitFlightMaster()).Position) < 5f)
                {
                    unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetWoWUnitFlightMaster());
                }
                else
                {
                    return;
                }
            }

            XElement taxilist = XElement.Load(Application.StartupPath + "\\Data\\TaxiList.xml");

            foreach (XElement taxi in taxilist.Elements())
            {
                if (taxi.Attribute("Id").Value == unit.Entry.ToString())
                {
                    TBObjDestinationX.Text = taxi.Element("Xcoord").Value;
                    TBObjDestinationY.Text = taxi.Element("Ycoord").Value;
                    break;
                }
            }
        }

        private void ButtonOpenWowHead_Click(object sender, EventArgs e)
        {
            if (TBQuestID.Text != string.Empty && Information.IsNumeric(TBQuestID.Text))
            {
                Others.OpenWebBrowserOrApplication("http://www.wowhead.com/quest=" + TBQuestID.Text);
            }
        }

        public void TODOs()
        {
            //TODO Add quest required ID from prev quest ID
        }

        private void ButtonQuestImportFromGame_Click(object sender, EventArgs e)
        {
            if (Others.IsFrameVisible("QuestFrameDetailPanel"))
            {
                if (ObjectManager.Target.IsValid && QuestersDB.GetNpcByEntry(ObjectManager.Target.Entry) == null)
                {
                    //TODO Check if NPC in the profile
                    if (MessageBox.Show(@"This Quest Giver isnt in the DB. Do you want to Add it?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        ButtonNewNPC.PerformClick();
                        ButtonNpcImport.PerformClick();
                        ButtonSaveNPC.PerformClick();

                        PanelNPC.Visible = true;
                        PanelSimpleQuest.Visible = false;
                    }
                }

                WoWGameObject wowGO = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectOfType(WoWGameObjectType.Questgiver));
                if (wowGO.Entry > 0 && ObjectManager.Me.Position.DistanceTo(wowGO.Position) < 5f && QuestersDB.GetNpcByEntry(wowGO.Entry) == null)
                {
                    if (MessageBox.Show(@"This Quest Giver (Object) isnt in the DB. Do you want to Add it?", @"Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        ButtonNewNPC.PerformClick();
                        ButtonNpcImport.PerformClick();
                        ButtonSaveNPC.PerformClick();
                        PanelNPC.Visible = true;
                        PanelSimpleQuest.Visible = false;
                    }
                }


                TBQuestPickUpID.Text = ObjectManager.Target.Entry.ToString(CultureInfo.InvariantCulture);
                TBQuestTurnInID.Text = ObjectManager.Target.Entry.ToString(CultureInfo.InvariantCulture);
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                TBQuestID.Text = Lua.LuaDoString(randomString + " = GetQuestID()", randomString);

                TBQuestQuestName.Text = Lua.LuaDoString(randomString + " = GetTitleText()", randomString);

                nManager.Wow.Helpers.Quest.AcceptQuest();
                Thread.Sleep(1000);
                int questLogIdx = Others.ToInt32(Lua.LuaDoString(randomString + " = GetQuestLogIndexByID(" + TBQuestID.Text + ")", randomString));

                int questl = Others.ToInt32(Lua.LuaDoString("_, " + randomString + " = GetQuestLogTitle(" + questLogIdx + ")", randomString));

                TBQuestLevel.Text = questl.ToString(CultureInfo.InvariantCulture);
                TBQuestMaxLvl.Text = GetMaxQuestLvl(questl).ToString(CultureInfo.InvariantCulture);
                TBQuestMinLvl.Text = GetMinQuestLvl(questl).ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                MessageBox.Show(@"No QuestGiver Quest Frame Opened, Cancel.");
            }
        }

        public int GetMaxQuestLvl(int questLvl)
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            int questGreenRange = Others.ToInt32(Lua.LuaDoString(randomString + " = GetQuestGreenRange()", randomString));

            int temp = 0;
            int pLevel = questLvl;
            int r = 0;

            while (temp == 0)
            {
                int levelDiff = questLvl - pLevel;
                if (levelDiff >= 5)
                {
                    pLevel = pLevel + 1;
                }
                else if (levelDiff >= 3)
                {
                    pLevel = pLevel + 1;
                }
                else if (levelDiff >= -2)
                {
                    pLevel = pLevel + 1;
                }
                else if (Math.Abs(levelDiff) <= questGreenRange)
                {
                    pLevel = pLevel + 1;
                }
                else
                {
                    temp = 1;
                    r = pLevel - 1;
                }
            }
            return r;
        }

        public int GetMinQuestLvl(int questLvl)
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            int questGreenRange = Others.ToInt32(Lua.LuaDoString(randomString + " = GetQuestGreenRange()", randomString));

            int temp = 0;
            int pLevel = questLvl;
            int r = 0;

            while (temp == 0)
            {
                int levelDiff = questLvl - pLevel;

                if (levelDiff >= 5)
                {
                }
                else if (levelDiff >= 3)
                {
                    temp = 1;
                    r = pLevel;
                }
                else if (levelDiff >= -2)
                {
                    pLevel = pLevel - 1;
                }
                else if (Math.Abs(levelDiff) <= questGreenRange)
                {
                    pLevel = pLevel - 1;
                }
            }
            return r;
        }

        public void ClearMaskListBox()
        {
            foreach (int item in CLBQuestClassMask.CheckedIndices)
            {
                CLBQuestClassMask.SetItemCheckState(item, CheckState.Unchecked);
            }
            foreach (int  item in CLBQuestRaceMask.CheckedIndices)
            {
                CLBQuestRaceMask.SetItemCheckState(item, CheckState.Unchecked);
            }
            CLBQuestRaceMask.ClearSelected();
            CLBQuestClassMask.ClearSelected();
        }


        public void SaveQuestMask()
        {
        }

        public void FillQuestMaskAfterObjSelection(int qClassMask, int qRaceMask)
        {
            var idxList = new List<int>();
            int idx = 0;
            int qmask = qClassMask;

            foreach (ComboBoxValue item in CLBQuestClassMask.Items)
            {
                if ((item.Value & qmask) != 0)
                {
                    idxList.Add(idx);
                }

                idx += 1;
            }


            foreach (int i in idxList)
            {
                CLBQuestClassMask.SetItemChecked(i, true);
            }

            idx = 0;
            qmask = qRaceMask;
            idxList.Clear();
            // Dim raceMask As Integer
            //Dim exp As Integer

            foreach (ComboBoxValue item in CLBQuestRaceMask.Items)
            {
                //      exp = item.Value - 1
                //    raceMask = IIf(exp >= 0, System.Math.Pow(2, exp), 0)

                if ((item.Value & qmask) != 0)
                {
                    idxList.Add(idx);
                }

                idx += 1;
            }

            foreach (int i in idxList)
            {
                CLBQuestRaceMask.SetItemChecked(i, true);
            }
        }

        private void ToolStripMenuItemAddNeedQuestComp_Click(object sender, EventArgs e)
        {
            if (_profile != null && _profile.Quests.Count > 0)
            {
                if (_lastSelectedQuest != null && TreeView.SelectedNode != null)
                {
                    TBQuestNeedQuestCompId.Text = _profile.Quests[_lastSelectedQuest.Index - 1].Id.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    TBQuestNeedQuestCompId.Text = _profile.Quests[_profile.Quests.Count - 1].Id.ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        #region "EVENTS - Optional"

        private bool _displayXml;
        private Size _fsize;

        public void QuestDetail(string te)
        {
            MessageBox.Show(te);
        }

        //public void QuestFinished(string te)
        //{
        //	int questID = Lua.LuaDoString("qId = GetQuestID()", "qId");

        //	string questName = Lua.LuaDoString("qTxt = GetTitleText()", "qTxt");

        //	foreach (void quest_loopVariable in profile.Quests) {
        //		quest = quest_loopVariable;
        //		if (quest.Id == questID) {
        //			foreach (TreeNode node in QuestParentNode.Nodes) {
        //				//Chercher si la quete existe
        //				if (node.Text == questName) {
        //					//If quest doesnt have objectives other than its own 
        //					if (ReturnNumQuestPickAndQuestTurnIn(quest.Objectives) > 0) {
        //						TreeView.SelectedNode = node;
        //						TBQuestTurnInID.Text = Target.Entry;
        //					//If the quest is a "multiple" quest
        //					} else {
        //						quest.Objectives.Add(CreateTurnInObj(Target.Entry, questID, questName));
        //						Wow.Helpers.Quest.AcceptQuest();
        //					}

        //					break; // TODO: might not be correct. Was : Exit For
        //				}
        //			}
        //		}
        //		foreach (void obj_loopVariable in quest.Objectives) {
        //			obj = obj_loopVariable;
        //			switch (obj.Objective.ToString) {
        //				case "PickUpQuest":
        //					if (obj.QuestId == questID) {
        //						//Find Parent Node
        //						foreach (TreeNode node in QuestParentNode.Nodes) {
        //							if (node.Text == quest.Name) {
        //								TreeNode newObjNode = new TreeNode("TurnInQuest " + questName);
        //								newObjNode.Tag = "Objective";
        //								node.Nodes.Add(newObjNode);

        //							}
        //						}
        //						quest.Objectives.Add(CreateTurnInObj(Target.Entry, questID, questName));
        //						Wow.Helpers.Quest.AcceptQuest();
        //						break; // TODO: might not be correct. Was : Exit For
        //					}

        //					break;
        //			}
        //		}

        //	}
        //}

        //public int ReturnNumQuestPickAndQuestTurnIn(List<Quester.Profile.QuestObjective> objs)
        //{

        //	int count = 0;

        //	foreach (void obj_loopVariable in objs) {
        //		obj = obj_loopVariable;
        //		if (obj.Objective == Quester.Profile.Objective.PickUpQuest | obj.Objective == Quester.Profile.Objective.TurnInQuest) {
        //			count += 1;
        //		}
        //	}
        //	return count;
        //}

        //public Quester.Profile.QuestObjective CreateTurnInObj(int npcId, int questId, string questName)
        //{
        //	Quester.Profile.QuestObjective qObj = new Quester.Profile.QuestObjective();

        //	var _with13 = qObj;
        //	_with13.Objective = Quester.Profile.Objective.TurnInQuest;
        //	_with13.IgnoreQuestCompleted = true;
        //	_with13.NpcEntry = npcId;
        //	_with13.QuestId = questId;
        //	_with13.QuestName = questName;

        //	return qObj;

        //}


        private void ButtonObjHotSpots_Click(object sender, EventArgs e)
        {
            LBObjHotspots.Items.Add(ObjectManager.Me.Position);
        }

        private void ButtonQuestHorde_Click(object sender, EventArgs e)
        {
            ClearMaskListBox();
            FillQuestMaskAfterObjSelection(0, 33555378);
        }

        private void ButtonQuestAlliance_Click(object sender, EventArgs e)
        {
            ClearMaskListBox();
            FillQuestMaskAfterObjSelection(0, 18875469);
        }

        private void ButtonObjImportEntry_Click(object sender, EventArgs e)
        {
            if (ObjectManager.Target.IsValid)
            {
                int entry = ObjectManager.Target.Entry;
                //if (TBObjEntry.Lines.Count > 0) {
                TBObjEntry.AppendText(entry + Environment.NewLine);
                //	} else {
                //	TBObjEntry.AppendText(entry.ToString());
                //}
            }
            else
            {
                string oName = Interaction.InputBox("Input Object Name:", "Import Object ID");
                if (oName != string.Empty)
                {
                    WoWGameObject wowGo = ObjectManager.GetNearestWoWGameObject(ObjectManager.GetWoWGameObjectByName(oName));
                    if (wowGo.Entry > 0)
                    {
                        //	if (TBObjEntry.Lines.Count() > 0) {
                        TBObjEntry.AppendText(wowGo.Entry + Environment.NewLine);
                        //} else {
                        //TBObjEntry.AppendText(wowGo.Entry.ToString());
                        //}
                    }
                }
            }
        }

        private void ButtonObjImportGPS_Click(object sender, EventArgs e)
        {
            TBObjPosition.Text = ObjectManager.Me.Position.ToString();
        }

        private void CBQuestWQ_CheckedChanged(object sender, EventArgs e)
        {
            if (CBQuestWQ.Checked)
            {
                TBQuestTurnInID.Enabled = false;
                TBQuestTurnInID.Text = string.Empty;
                TBQuestPickUpID.Enabled = false;
                TBQuestPickUpID.Text = string.Empty;
                TBQuestWQ.Enabled = true;
            }
            else
            {
                TBQuestPickUpID.Enabled = true;
                TBQuestTurnInID.Enabled = true;
                TBQuestWQ.Enabled = false;
                TBQuestWQ.Text = string.Empty;
            }
        }

        private void CBMainDisplayXML_CheckedChanged(object sender, EventArgs e)
        {
            if (CBMainDisplayXML.Checked)
            {
                _displayXml = true;
                Size = new Size(_fsize.Width + 579, Size.Height);
                object objXmlToDisplay = null;
                if (_lastSelectedObjective != null)
                {
                    objXmlToDisplay = _profile.Quests[_lastSelectedQuest.Index].Objectives[_lastSelectedObjective.Index];
                }
                else if (_lastSelectedQuest != null)
                {
                    objXmlToDisplay = _profile.Quests[_lastSelectedQuest.Index];
                }
                else if (_lastSelectedNpc != null)
                {
                    objXmlToDisplay = _profile.Questers[_lastSelectedNpc.Index];
                }
                DisplayXMLs(objXmlToDisplay);
            }
            else
            {
                _displayXml = false;
                Size = _fsize;
            }
        }

        private void TreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeView.SelectedNode = e.Node;
            }
        }


        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((string) TreeView.SelectedNode.Tag == "Quest")
            {
                _profile.Quests.RemoveAt(TreeView.SelectedNode.Index);
                TreeView.SelectedNode.Remove();
            }
            else if ((string) TreeView.SelectedNode.Tag == "Objective")
            {
                _profile.Quests[TreeView.SelectedNode.Parent.Index].Objectives.RemoveAt(TreeView.SelectedNode.Index);
                TreeView.SelectedNode.Remove();
            }
        }

        private void InsertUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dynamic insertIdxProfile;
            dynamic insertIdxTreeView = 0;

            if (ReferenceEquals(sender, InsertUpToolStripMenuItem))
            {
                insertIdxProfile = 1;
            }
            else
            {
                insertIdxProfile = -1;
                insertIdxTreeView = 1;
            }

            if ((string) TreeView.SelectedNode.Tag == "Quest")
            {
                var newQuestNode = new TreeNode("New Quest") {Tag = "Quest"};
                _questParentNode.Nodes.Insert(TreeView.SelectedNode.Index + insertIdxTreeView, newQuestNode);

                var newQuest = new Quest {Name = "New Quest"};
                _profile.Quests.Insert(TreeView.SelectedNode.Index - insertIdxProfile, newQuest);
            }
            else if ((string) TreeView.SelectedNode.Tag == "Objective")
            {
                var newObjNode = new TreeNode("New Objective") {Tag = "NewObjective"};
                TreeView.SelectedNode.Parent.Nodes.Insert(TreeView.SelectedNode.Index + insertIdxTreeView, newObjNode);

                var newObjective = new QuestObjective();
                _profile.Quests[TreeView.SelectedNode.Parent.Index].Objectives.Insert(TreeView.SelectedNode.Index - insertIdxProfile, newObjective);
            }
        }

        private void ButtonObjDumpIndex_Click(object sender, EventArgs e)
        {
            int questId = 0;

            if (TreeView.SelectedNode != null && (string) TreeView.SelectedNode.Tag != "NPCs" && (string) TreeView.SelectedNode.Tag != "Quests" && (string) TreeView.SelectedNode.Tag != "NPC")
            {
                if ((string) TreeView.SelectedNode.Tag == "Quest")
                {
                    questId = _profile.Quests[TreeView.SelectedNode.Index].Id;
                }
                else if ((string) TreeView.SelectedNode.Tag == "Objective")
                {
                    QuestObjective obj = _profile.Quests[TreeView.SelectedNode.Parent.Index].Objectives[TreeView.SelectedNode.Index];
                    if (obj.Objective.ToString() == "PickUpQuest")
                    {
                        questId = _profile.Quests[TreeView.SelectedNode.Parent.Index].Objectives[TreeView.SelectedNode.Index].QuestId;
                    }
                    else
                    {
                        if (obj.InternalQuestId > 0)
                        {
                            questId = obj.InternalQuestId;
                        }
                        else if (CBInternalObj.Checked)
                        {
                            questId = (int) CBObjInternalQuestID.SelectedValue;
                        }
                        else
                        {
                            questId = _profile.Quests[TreeView.SelectedNode.Parent.Index].Id;
                        }
                    }
                }

                nManager.Wow.Helpers.Quest.DumpInternalIndexForQuestId(questId);
            }
        }

        #endregion
    }

    public class ComboBoxValueString
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

    public class ComboBoxValue
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}