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

namespace Grinder.Profile
{
    public partial class ProfileCreator : Form
    {
        private int _idZone;
        private bool _loopRecordPoint;
        private GrinderProfile _profile = new GrinderProfile();

        public ProfileCreator()
        {
            try
            {
                InitializeComponent();
                Translate();
                listZoneCb.DropDownStyle = ComboBoxStyle.DropDownList;
                npcTypeC.DropDownStyle = ComboBoxStyle.DropDownList;
                foreach (Npc.NpcType t in Enum.GetValues(typeof (Npc.NpcType)).Cast<Npc.NpcType>().ToList())
                {
                    npcTypeC.Items.Add(t.ToString());
                }
                npcTypeC.Text = Npc.NpcType.None.ToString();

                RefreshListZones();
                if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                    TopMost = true;
            }
            catch (Exception e)
            {
                Logging.WriteError("Grinder > Bot > ProfileCreator > ProfileCreator(): " + e);
            }
        }

        private void Translate()
        {
            recordWayB.Text = nManager.Translate.Get(nManager.Translate.Id.Record_Way);
            saveB.Text = nManager.Translate.Get(nManager.Translate.Id.Save);
            labelX1.Text = nManager.Translate.Get(nManager.Translate.Id.Separation_distance_record) + ":";
            delB.Text = nManager.Translate.Get(nManager.Translate.Id.Del);
            delBlackRadius.Text = nManager.Translate.Get(nManager.Translate.Id.Del);
            addBlackB.Text = nManager.Translate.Get(nManager.Translate.Id.Add_this_position_to_Black_list_Radius);
            addNpcB.Text = nManager.Translate.Get(nManager.Translate.Id.Add_Target_to_Npc_list);
            delNpcB.Text = nManager.Translate.Get(nManager.Translate.Id.Del);
            loadB.Text = nManager.Translate.Get(nManager.Translate.Id.Load);
            nameNpcTb.Text = nManager.Translate.Get(nManager.Translate.Id.Name);
            addByNameNpcB.Text = nManager.Translate.Get(nManager.Translate.Id.Add_by_Name_to_Npc_list);
            ListOfZonesLabel.Text = nManager.Translate.Get(nManager.Translate.Id.List_Zones) + ":";
            addZoneB.Text = nManager.Translate.Get(nManager.Translate.Id.Add_Zone);
            delZoneB.Text = nManager.Translate.Get(nManager.Translate.Id.Del_Zone);
            ZoneNameLabel.Text = nManager.Translate.Get(nManager.Translate.Id.Zone_Name) + ":";
            ZoneMinLevel.Text = nManager.Translate.Get(nManager.Translate.Id.Player_Lvl) + " " +
                                nManager.Translate.Get(nManager.Translate.Id.Min);
            ZoneMaxLevelLabel.Text = nManager.Translate.Get(nManager.Translate.Id.Max);
            TargetMaxLevelLabel.Text = nManager.Translate.Get(nManager.Translate.Id.Max);
            TargetMinLevelLabel.Text = nManager.Translate.Get(nManager.Translate.Id.Target_Lvl) + " " +
                                       nManager.Translate.Get(nManager.Translate.Id.Min);
            addTargetEntryB.Text = nManager.Translate.Get(nManager.Translate.Id.Add_Target);
            labelX8.Text = nManager.Translate.Get(nManager.Translate.Id.Target_Ids);
            MainHeader.TitleText = nManager.Translate.Get(nManager.Translate.Id.Profile_Creator) + " - " + Information.MainTitle;
            this.Text = MainHeader.TitleText;
        }

        private void saveB_Click(object sender, EventArgs ex)
        {
            try
            {
                string file =
                    Others.DialogBoxSaveFile(Application.StartupPath + "\\Profiles\\Grinder\\",
                        "Profile files (*.xml)|*.xml|All files (*.*)|*.*");

                if (file != "")
                {
                    XmlSerializer.Serialize(file, _profile);
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Grinder > Bot > ProfileCreator > saveB_Click(object sender, EventArgs ex): " + e);
            }
        }


        private void loadB_Click(object sender, EventArgs e)
        {
            try
            {
                string file =
                    Others.DialogBoxOpenFile(Application.StartupPath + "\\Profiles\\Grinder\\",
                        "Profile files (*.xml)|*.xml|All files (*.*)|*.*");

                if (File.Exists(file))
                {
                    _profile = new GrinderProfile();
                    _profile = XmlSerializer.Deserialize<GrinderProfile>(file);
                    RefreshForm();
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("Grinder > Bot > ProfileCreator > loadB_Click(object sender, EventArgs e): " + ex);
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
                    "Grinder > Bot > ProfileCreator > ProfileCreator_FormClosing(object sender, FormClosingEventArgs ex): " +
                    e);
            }
        }

        private void RefreshListZones()
        {
            lock (typeof (ProfileCreator))
            {
                try
                {
                    // List Zones
                    listZoneCb.Items.Clear();
                    if (_profile.GrinderZones.Count <= 0)
                    {
                        addZoneB_Click(null, null);
                        return;
                    }
                    foreach (GrinderZone p in _profile.GrinderZones)
                    {
                        // if (!listZoneCb.Items.Contains(p.Name))
                        listZoneCb.Items.Add(p.Name + "(" + listZoneCb.Items.Count + ")" + " - Level " + p.MinLevel + " to " + p.MaxLevel + " with " + p.TargetEntry.Count + " targets");
                    }
                    if (listZoneCb.SelectedIndex != _idZone)
                    {
                        if (listZoneCb.Items.Count >= _idZone + 1)
                            listZoneCb.SelectedIndex = _idZone;
                    }
                }
                catch
                {
                }
            }
        }

        private void RefreshForm()
        {
            lock (typeof (ProfileCreator))
            {
                try
                {
                    // Target entry
                    string text = "";
                    foreach (int entry in _profile.GrinderZones[_idZone].TargetEntry)
                    {
                        if (text != "")
                            text = text + " ; ";
                        text = text + entry;
                    }
                    listEntryTb.Text = text;
                }
                catch
                {
                }
                try
                {
                    // Player Level
                    var savedMinPlayer = (int)_profile.GrinderZones[_idZone].MinLevel;
                    var savedMaxPlayer = (int)_profile.GrinderZones[_idZone].MaxLevel;

                    maxLevelPlayer.Value = savedMaxPlayer;
                    minLevelPlayer.Value = savedMinPlayer;
                }
                catch
                {
                }
                try
                {
                    // Target Level
                    var savedMinLevelTarget = (int)_profile.GrinderZones[_idZone].MinTargetLevel;
                    var savedMaxLevelTarget = (int)_profile.GrinderZones[_idZone].MaxTargetLevel;
                    maxLevelTarget.Value = savedMaxLevelTarget;
                    minLevelTarget.Value = savedMinLevelTarget;
                }
                catch
                {
                }
                try
                {
                    // Name Zone
                    zoneNameTb.Text = _profile.GrinderZones[_idZone].Name;
                }
                catch
                {
                }
                try
                {
                    // Way
                    listPoint.Items.Clear();
                    foreach (Point p in _profile.GrinderZones[_idZone].Points)
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
                    foreach (GrinderBlackListRadius b in _profile.GrinderZones[_idZone].BlackListRadius)
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
                    foreach (Npc n in _profile.GrinderZones[_idZone].Npc)
                    {
                        listNpc.Items.Add(n.Name + " - " + n.Type + " - " + n.Faction);
                    }
                    listNpc.SelectedIndex = listNpc.Items.Count - 1;
                }
                catch
                {
                }
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
                Logging.WriteError("Grinder > Bot > ProfileCreator > recordWayB_Click(object sender, EventArgs ex): " +
                                   e);
            }
        }

        private void LoopRecordWay()
        {
            try
            {
                const float distanceZSeparator = 3.0f;
                int lastRotation = 0;
                _loopRecordPoint = true;

                _profile.GrinderZones[_idZone].Points.Add(ObjectManager.Me.Position);
                RefreshForm();

                while (_loopRecordPoint)
                {
                    Point lastPoint = _profile.GrinderZones[_idZone].Points[_profile.GrinderZones[_idZone].Points.Count - 1];
                    float disZTemp = lastPoint.DistanceZ(ObjectManager.Me.Position);

                    if (((lastPoint.DistanceTo(ObjectManager.Me.Position) > (double) nSeparatorDistance.Value) &&
                         lastRotation != (int) Math.RadianToDegree(ObjectManager.Me.Rotation)) ||
                        disZTemp >= distanceZSeparator)
                    {
                        _profile.GrinderZones[_idZone].Points.Add(ObjectManager.Me.Position);
                        lastRotation = (int) Math.RadianToDegree(ObjectManager.Me.Rotation);
                        RefreshForm();
                    }
                    Application.DoEvents();
                    Thread.Sleep(50);
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Grinder > Bot > ProfileCreator > LoopRecordWay(): " + e);
            }
        }

        private void delB_Click(object sender, EventArgs ex)
        {
            try
            {
                if (listPoint.SelectedIndex >= 0)
                    _profile.GrinderZones[_idZone].Points.RemoveAt(listPoint.SelectedIndex);
                RefreshForm();
            }
            catch (Exception e)
            {
                Logging.WriteError("Grinder > Bot > ProfileCreator > delB_Click(object sender, EventArgs ex): " + e);
            }
        }

        // BLACK LIST
        private void delBlackRadius_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBlackRadius.SelectedIndex >= 0)
                    _profile.GrinderZones[_idZone].BlackListRadius.RemoveAt(listBlackRadius.SelectedIndex);
                RefreshForm();
            }
            catch (Exception ex)
            {
                Logging.WriteError(
                    "Grinder > Bot > ProfileCreator > delBlackRadius_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void addBlackB_Click(object sender, EventArgs e)
        {
            try
            {
                _profile.GrinderZones[_idZone].BlackListRadius.Add(new GrinderBlackListRadius
                {
                    Position =
                        ObjectManager.Me
                            .Position,
                    Radius = (float) radiusN.Value
                });
                RefreshForm();
            }
            catch (Exception ex)
            {
                Logging.WriteError("Grinder > Bot > ProfileCreator > addBlackB_Click(object sender, EventArgs e): " + ex);
            }
        }

        // NPC
        private void delNpcB_Click(object sender, EventArgs e)
        {
            try
            {
                if (listNpc.SelectedIndex >= 0)
                    _profile.GrinderZones[_idZone].Npc.RemoveAt(listNpc.SelectedIndex);
                RefreshForm();
            }
            catch (Exception ex)
            {
                Logging.WriteError("Grinder > Bot > ProfileCreator > delNpcB_Click(object sender, EventArgs e): " + ex);
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
                    ContinentIdInt = Usefuls.ContinentId,
                    Entry = ObjectManager.Target.Entry,
                    Faction =
                        (Npc.FactionType)
                            Enum.Parse(typeof (Npc.FactionType), ObjectManager.Me.PlayerFaction, true),
                    Name = ObjectManager.Target.Name,
                    Position = ObjectManager.Target.Position,
                    Type = (Npc.NpcType) Enum.Parse(typeof (Npc.NpcType), npcTypeC.Text, true)
                };
                _profile.GrinderZones[_idZone].Npc.Add(npc);
                RefreshForm();
            }
            catch (Exception ex)
            {
                Logging.WriteError("Grinder > Bot > ProfileCreator > addNpcB_Click(object sender, EventArgs e): " + ex);
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
                    MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.NPCNotFound) + ".");
                    return;
                }

                npc.ContinentIdInt =
                    Usefuls.ContinentId;
                npc.Faction =
                    (Npc.FactionType)
                        Enum.Parse(typeof (Npc.FactionType), ObjectManager.Me.PlayerFaction, true);
                npc.Type = (Npc.NpcType) Enum.Parse(typeof (Npc.NpcType), npcTypeC.Text, true);

                if (Usefuls.IsOutdoors)
                    npc.Position.Type = "Flying";

                _profile.GrinderZones[_idZone].Npc.Add(npc);
                RefreshForm();
                nameNpcTb.Text = "";
            }
            catch (Exception ex)
            {
                Logging.WriteError("addByNameNpcB_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void listZoneCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _idZone = listZoneCb.SelectedIndex;
                RefreshForm();
            }
            catch (Exception ex)
            {
                Logging.WriteError("listZoneCb_SelectedIndexChanged(object sender, EventArgs e): " + ex);
            }
        }

        private void addZoneB_Click(object sender, EventArgs e)
        {
            try
            {
                _idZone = listZoneCb.SelectedIndex;
                RefreshForm();
                _profile.GrinderZones.Add(new GrinderZone {Name = Usefuls.MapZoneName});
                _idZone = _profile.GrinderZones.Count - 1;
                RefreshListZones();
                RefreshForm();
            }
            catch (Exception ex)
            {
                Logging.WriteError("addZoneB_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void delZoneB_Click(object sender, EventArgs e)
        {
            try
            {
                _profile.GrinderZones.RemoveAt(_idZone);
                _idZone = _profile.GrinderZones.Count - 1;
                RefreshListZones();
            }
            catch (Exception ex)
            {
                Logging.WriteError("delZoneB_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void zoneNameTb_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _profile.GrinderZones[_idZone].Name = zoneNameTb.Text;
                RefreshListZones();
            }
            catch (Exception ex)
            {
                Logging.WriteError("zoneNameTb_TextChanged(object sender, EventArgs e): " + ex);
            }
        }

        private void Level_ValueChanged(object sender, EventArgs e)
        {
            lock (typeof (ProfileCreator))
            {
                try
                {
                    // Player Level
                    _profile.GrinderZones[_idZone].MaxLevel = (uint) maxLevelPlayer.Value;
                    _profile.GrinderZones[_idZone].MinLevel = (uint) minLevelPlayer.Value;
                }
                catch
                {
                }
                try
                {
                    // Target Level
                    _profile.GrinderZones[_idZone].MaxTargetLevel = (uint) maxLevelTarget.Value;
                    _profile.GrinderZones[_idZone].MinTargetLevel = (uint) minLevelTarget.Value;
                }
                catch
                {
                }
            }
        }

        private void addTargetEntryB_Click(object sender, EventArgs e)
        {
            try
            {
                lock (typeof (ProfileCreator))
                {
                    if (ObjectManager.Target.IsValid)
                    {
                        string text = listEntryTb.Text;
                        if (!string.IsNullOrWhiteSpace(text))
                            text = text + " ; ";
                        text = text + ObjectManager.Target.Entry;
                        listEntryTb.Text = text;
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("addTargetEntryB_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void listEntryTb_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string text = listEntryTb.Text;
                lock (typeof (ProfileCreator))
                {
                    if (text.Replace(" ", "").Replace(";", "").Length > 0)
                    {
                        string[] factionTempsString = text.Replace(" ", "").Split(';');
                        _profile.GrinderZones[_idZone].TargetEntry.Clear();
                        foreach (string t in factionTempsString)
                        {
                            try
                            {
                                if (t != "")
                                    if (!_profile.GrinderZones[_idZone].TargetEntry.Contains(Others.ToInt32(t)))
                                        _profile.GrinderZones[_idZone].TargetEntry.Add(Others.ToInt32(t));
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("addTargetEntryB_Click(object sender, EventArgs e): " + ex);
            }
        }
    }
}