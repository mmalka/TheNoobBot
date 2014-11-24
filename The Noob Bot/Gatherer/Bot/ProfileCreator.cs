using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using nManager;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using Math = nManager.Helpful.Math;

namespace Gatherer.Bot
{
    public partial class ProfileCreator : Form
    {
        private bool _loopRecordPoint;
        private GathererProfile _profile = new GathererProfile();

        public ProfileCreator()
        {
            try
            {
                InitializeComponent();
                Translate();
                npcTypeC.DropDownStyle = ComboBoxStyle.DropDownList;
                foreach (Npc.NpcType t in Enum.GetValues(typeof (Npc.NpcType)).Cast<Npc.NpcType>().ToList())
                {
                    npcTypeC.Items.Add(t.ToString());
                }
                npcTypeC.Text = Npc.NpcType.None.ToString();
                if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                    TopMost = true;
            }
            catch (Exception e)
            {
                Logging.WriteError("Gatherer > Bot > ProfileCreator > ProfileCreator(): " + e);
            }
        }

        private void Translate()
        {
            recordWayB.Text = nManager.Translate.Get(nManager.Translate.Id.Record_Way);
            saveB.Text = nManager.Translate.Get(nManager.Translate.Id.Save);
            labelX1.Text = nManager.Translate.Get(nManager.Translate.Id.Separation_distance_record);
            delB.Text = nManager.Translate.Get(nManager.Translate.Id.Del);
            delBlackRadius.Text = nManager.Translate.Get(nManager.Translate.Id.Del);
            addBlackB.Text = nManager.Translate.Get(nManager.Translate.Id.Add_this_position_to_Black_list_Radius);
            addNpcB.Text = nManager.Translate.Get(nManager.Translate.Id.Add_Target_to_Npc_list);
            delNpcB.Text = nManager.Translate.Get(nManager.Translate.Id.Del);
            loadB.Text = nManager.Translate.Get(nManager.Translate.Id.Load);
            nameNpcTb.Text = nManager.Translate.Get(nManager.Translate.Id.Name);
            addByNameNpcB.Text = nManager.Translate.Get(nManager.Translate.Id.Add_by_Name_to_Npc_list);
            MainHeader.TitleText = nManager.Translate.Get(nManager.Translate.Id.Profile_Creator) + " - " + Information.MainTitle;
            this.Text = MainHeader.TitleText;
        }

        private void saveB_Click(object sender, EventArgs ex)
        {
            try
            {
                string file =
                    Others.DialogBoxSaveFile(Application.StartupPath + "\\Profiles\\Gatherer\\",
                        "Profile files (*.xml)|*.xml|All files (*.*)|*.*");

                if (file != "")
                {
                    XmlSerializer.Serialize(file, _profile);
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Gatherer > Bot > ProfileCreator > saveB_Click(object sender, EventArgs ex): " + e);
            }
        }


        private void loadB_Click(object sender, EventArgs e)
        {
            try
            {
                string file =
                    Others.DialogBoxOpenFile(Application.StartupPath + "\\Profiles\\Gatherer\\",
                        "Profile files (*.xml)|*.xml|All files (*.*)|*.*");

                if (File.Exists(file))
                {
                    _profile = new GathererProfile();
                    _profile = XmlSerializer.Deserialize<GathererProfile>(file);
                    RefreshForm();
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("Gatherer > Bot > ProfileCreator > loadB_Click(object sender, EventArgs e): " + ex);
                RefreshForm();
            }
        }

        private void ProfileCreator_FormClosing(object sender, FormClosingEventArgs ex)
        {
            try
            {
                _loopRecordPoint = false;
            }
            catch (Exception e)
            {
                Logging.WriteError(
                    "Gatherer > Bot > ProfileCreator > ProfileCreator_FormClosing(object sender, FormClosingEventArgs ex): " +
                    e);
            }
        }

        private void RefreshForm()
        {
            try
            {
                // Way
                listPoint.Items.Clear();
                foreach (Point p in _profile.Points)
                {
                    listPoint.Items.Add(p.ToString());
                }
                listPoint.SelectedIndex = listPoint.Items.Count - 1;
            }
            catch
            {
            }

            try
            {
                // BlackList
                listBlackRadius.Items.Clear();
                foreach (GathererBlackListRadius b in _profile.BlackListRadius)
                {
                    listBlackRadius.Items.Add(b.Position.X + " ; " + b.Position.Y + " - " + b.Radius);
                }
                listBlackRadius.SelectedIndex = listBlackRadius.Items.Count - 1;
            }
            catch
            {
            }

            try
            {
                // Npc
                listNpc.Items.Clear();
                foreach (Npc n in _profile.Npc)
                {
                    listNpc.Items.Add(n.Name + " - " + n.Type + " - " + n.Faction);
                }
                listNpc.SelectedIndex = listNpc.Items.Count - 1;
            }
            catch
            {
            }
        }


        // WAY

        private void recordWayB_Click(object sender, EventArgs ex)
        {
            try
            {
                if (_loopRecordPoint)
                {
                    _loopRecordPoint = false;
                    recordWayB.Text = nManager.Translate.Get(nManager.Translate.Id.Record_Way);
                }
                else
                {
                    _loopRecordPoint = true;
                    recordWayB.Text = nManager.Translate.Get(nManager.Translate.Id.Stop_Record_Way);
                    LoopRecordWay();
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Gatherer > Bot > ProfileCreator > recordWayB_Click(object sender, EventArgs ex): " +
                                   e);
            }
        }

        private void LoopRecordWay()
        {
            try
            {
                const float distanceZSeparator = 15.0f;
                int lastRotation = 0;
                _loopRecordPoint = true;

                _profile.Points.Add(ObjectManager.Me.Position);
                RefreshForm();

                while (_loopRecordPoint)
                {
                    Point lastPoint = _profile.Points[_profile.Points.Count - 1];
                    float disZTemp = lastPoint.DistanceZ(ObjectManager.Me.Position);

                    if (((lastPoint.DistanceTo(ObjectManager.Me.Position) > (double) nSeparatorDistance.Value) &&
                         lastRotation != (int) Math.RadianToDegree(ObjectManager.Me.Rotation)) ||
                        disZTemp >= distanceZSeparator)
                    {
                        _profile.Points.Add(ObjectManager.Me.Position);
                        lastRotation = (int) Math.RadianToDegree(ObjectManager.Me.Rotation);
                        RefreshForm();
                    }
                    Application.DoEvents();
                    Thread.Sleep(50);
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Gatherer > Bot > ProfileCreator > LoopRecordWay(): " + e);
            }
        }

        private void delB_Click(object sender, EventArgs ex)
        {
            try
            {
                if (listPoint.SelectedIndex >= 0)
                    _profile.Points.RemoveAt(listPoint.SelectedIndex);
                RefreshForm();
            }
            catch (Exception e)
            {
                Logging.WriteError("Gatherer > Bot > ProfileCreator > delB_Click(object sender, EventArgs ex): " + e);
            }
        }

        // BLACK LIST
        private void delBlackRadius_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBlackRadius.SelectedIndex >= 0)
                    _profile.BlackListRadius.RemoveAt(listBlackRadius.SelectedIndex);
                RefreshForm();
            }
            catch (Exception ex)
            {
                Logging.WriteError(
                    "Gatherer > Bot > ProfileCreator > delBlackRadius_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void addBlackB_Click(object sender, EventArgs e)
        {
            try
            {
                _profile.BlackListRadius.Add(new GathererBlackListRadius
                {Position = ObjectManager.Me.Position, Radius = (float) radiusN.Value});
                RefreshForm();
            }
            catch (Exception ex)
            {
                Logging.WriteError("Gatherer > Bot > ProfileCreator > addBlackB_Click(object sender, EventArgs e): " +
                                   ex);
            }
        }

        // NPC
        private void delNpcB_Click(object sender, EventArgs e)
        {
            try
            {
                if (listNpc.SelectedIndex >= 0)
                    _profile.Npc.RemoveAt(listNpc.SelectedIndex);
                RefreshForm();
            }
            catch (Exception ex)
            {
                Logging.WriteError("Gatherer > Bot > ProfileCreator > delNpcB_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void addNpcB_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ObjectManager.Me.IsValid || !ObjectManager.Target.IsValid)
                    return;

                var npc = new Npc
                {
                    ContinentId =
                        (ContinentId) (Usefuls.ContinentId),
                    Entry = ObjectManager.Target.Entry,
                    Faction =
                        (Npc.FactionType)
                            Enum.Parse(typeof (Npc.FactionType), ObjectManager.Me.PlayerFaction, true),
                    Name = ObjectManager.Target.Name,
                    Position = ObjectManager.Target.Position,
                    Type = (Npc.NpcType) Enum.Parse(typeof (Npc.NpcType), npcTypeC.Text, true)
                };
                _profile.Npc.Add(npc);
                RefreshForm();
            }
            catch (Exception ex)
            {
                Logging.WriteError("Gatherer > Bot > ProfileCreator > addNpcB_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void addByNameNpcB_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ObjectManager.Me.IsValid)
                    return;

                if (string.IsNullOrEmpty(nameNpcTb.Text))
                {
                    MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.Name_Empty));
                    return;
                }

                var npc = new Npc();

                List<WoWGameObject> gameObjects = ObjectManager.GetWoWGameObjectByName(nameNpcTb.Text);

                if (gameObjects.Count > 0)
                {
                    WoWGameObject gameObject = ObjectManager.GetNearestWoWGameObject(gameObjects);
                    if (gameObject.IsValid)
                    {
                        npc.Entry = gameObject.Entry;
                        npc.Position = gameObject.Position;
                        npc.Name = gameObject.Name;
                    }
                }

                if (npc.Entry <= 0)
                {
                    List<WoWUnit> units = ObjectManager.GetWoWUnitByName(nameNpcTb.Text);
                    if (units.Count > 0)
                    {
                        WoWUnit unit = ObjectManager.GetNearestWoWUnit(units);
                        if (unit.IsValid)
                        {
                            npc.Entry = unit.Entry;
                            npc.Position = unit.Position;
                            npc.Name = unit.Name;
                        }
                    }
                }

                if (npc.Entry <= 0)
                {
                    MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.NPCNotFound));
                    return;
                }

                npc.ContinentId =
                    (ContinentId) (Usefuls.ContinentId);
                npc.Faction =
                    (Npc.FactionType)
                        Enum.Parse(typeof (Npc.FactionType), ObjectManager.Me.PlayerFaction, true);
                npc.Type = (Npc.NpcType) Enum.Parse(typeof (Npc.NpcType), npcTypeC.Text, true);

                if (Usefuls.IsOutdoors)
                    npc.Position.Type = "Flying";

                _profile.Npc.Add(npc);
                RefreshForm();
                nameNpcTb.Text = "";
            }
            catch (Exception ex)
            {
                Logging.WriteError("addByNameNpcB_Click(object sender, EventArgs e): " + ex);
            }
        }
    }
}