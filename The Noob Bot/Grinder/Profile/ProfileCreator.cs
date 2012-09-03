using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using nManager;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.ObjectManager;

namespace Grinder.Profile
{
    public partial class ProfileCreator : DevComponents.DotNetBar.Metro.MetroForm
    {
        GrinderProfile _profile = new GrinderProfile();
        public ProfileCreator()
        {
            try
            {
                InitializeComponent();
                Translate();
                listZoneCb.DropDownStyle = ComboBoxStyle.DropDownList;
                npcTypeC.DropDownStyle = ComboBoxStyle.DropDownList;
                foreach (var t in Enum.GetValues(typeof(Npc.NpcType)).Cast<Npc.NpcType>().ToList())
                {
                    npcTypeC.Items.Add(t.ToString());
                }
                npcTypeC.Text = Npc.NpcType.None.ToString();

                refreshListZones();
            }
            catch (Exception e)
            {
                Logging.WriteError("Grinder > Bot > ProfileCreator > ProfileCreator(): " + e);
            }
        }
        void Translate()
        {
            recordWayB.Text = nManager.Translate.Get(nManager.Translate.Id.Record_Way);
            saveB.Text = nManager.Translate.Get(nManager.Translate.Id.Save);
            labelX1.Text = nManager.Translate.Get(nManager.Translate.Id.Separation_distance_record)+":";
            delB.Text = nManager.Translate.Get(nManager.Translate.Id.Del);
            delBlackRadius.Text = nManager.Translate.Get(nManager.Translate.Id.Del);
            addBlackB.Text = nManager.Translate.Get(nManager.Translate.Id.Add_this_position_to_Black_list_Radius);
            addNpcB.Text = nManager.Translate.Get(nManager.Translate.Id.Add_Target_to_Npc_list);
            delNpcB.Text = nManager.Translate.Get(nManager.Translate.Id.Del);
            loadB.Text = nManager.Translate.Get(nManager.Translate.Id.Load);
            nameNpcTb.Text = nManager.Translate.Get(nManager.Translate.Id.Name);
            addByNameNpcB.Text = nManager.Translate.Get(nManager.Translate.Id.Add_by_Name_to_Npc_list);
            labelX2.Text = nManager.Translate.Get(nManager.Translate.Id.List_Zones)+":";
            addZoneB.Text = nManager.Translate.Get(nManager.Translate.Id.Add_Zone);
            delZoneB.Text = nManager.Translate.Get(nManager.Translate.Id.Del_Zone);
            labelX3.Text = nManager.Translate.Get(nManager.Translate.Id.Zone_Name)+":";
            labelX4.Text = nManager.Translate.Get(nManager.Translate.Id.Player_Lvl) + " " + nManager.Translate.Get(nManager.Translate.Id.Min);
            labelX5.Text = nManager.Translate.Get(nManager.Translate.Id.Max);
            labelX6.Text = nManager.Translate.Get(nManager.Translate.Id.Max);
            labelX7.Text = nManager.Translate.Get(nManager.Translate.Id.Target_Lvl) + " " + nManager.Translate.Get(nManager.Translate.Id.Min);
            addTargetEntryB.Text = nManager.Translate.Get(nManager.Translate.Id.Add_Target);
            labelX8.Text = nManager.Translate.Get(nManager.Translate.Id.Target_Ids);
            Text = nManager.Translate.Get(nManager.Translate.Id.Profile_Creator);
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
                    refreshForm();
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("Grinder > Bot > ProfileCreator > loadB_Click(object sender, EventArgs e): " + ex);
                refreshForm();
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
                Logging.WriteError("Grinder > Bot > ProfileCreator > ProfileCreator_FormClosing(object sender, FormClosingEventArgs ex): " + e);
            }
        }

        private int idZone;
        void refreshListZones()
        {
            lock (typeof(ProfileCreator))
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
                    foreach (var p in _profile.GrinderZones)
                    {
                        // if (!listZoneCb.Items.Contains(p.Name))
                        listZoneCb.Items.Add(p.Name);
                    }
                    if (listZoneCb.SelectedIndex != idZone)
                    {
                        listZoneCb.SelectedIndex = idZone;
                        return;
                    }
                }
                catch
                {
                }
            }
        }
        void refreshForm()
        {
            lock (typeof(ProfileCreator))
            {
                try
                {
                    // Target entry
                    var text =  "";
                    foreach (var entry in _profile.GrinderZones[idZone].TargetEntry)
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
                    maxLevelPlayer.ValueObject = (int)_profile.GrinderZones[idZone].MaxLevel;
                    minLevelPlayer.ValueObject = (int)_profile.GrinderZones[idZone].MinLevel;
                }
                catch
                {
                }
                try
                {
                    // Target Level
                    maxLevelTarget.ValueObject = (int)_profile.GrinderZones[idZone].MaxTargetLevel;
                    minLevelTarget.ValueObject = (int)_profile.GrinderZones[idZone].MinTargetLevel;
                }
                catch
                {
                }
                try
                {
                    // Name Zone
                    zoneNameTb.Text = _profile.GrinderZones[idZone].Name;
                }
                catch
                {
                }
                try
                {
                    // Way
                    listPoint.Items.Clear();
                    foreach (var p in _profile.GrinderZones[idZone].Points)
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
                    foreach (var b in _profile.GrinderZones[idZone].BlackListRadius)
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
                    foreach (var n in _profile.GrinderZones[idZone].Npc)
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
        private bool _loopRecordPoint;
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
                Logging.WriteError("Grinder > Bot > ProfileCreator > recordWayB_Click(object sender, EventArgs ex): " + e);
            }
        }

        private void LoopRecordWay()
        {
            try
            {
                const float distanceZSeparator = 3.0f;
                int lastRotation = 0;
                _loopRecordPoint = true;

                _profile.GrinderZones[idZone].Points.Add(ObjectManager.Me.Position);
                refreshForm();

                while (_loopRecordPoint)
                {
                    var lastPoint = _profile.GrinderZones[idZone].Points[_profile.GrinderZones[idZone].Points.Count - 1];
                    float disZTemp = lastPoint.DistanceZ(ObjectManager.Me.Position);

                    if (((lastPoint.DistanceTo(ObjectManager.Me.Position) > nSeparatorDistance.Value) &&
                         lastRotation != (int)nManager.Helpful.Math.RadianToDegree(ObjectManager.Me.Rotation)) ||
                        disZTemp >= distanceZSeparator)
                    {
                        _profile.GrinderZones[idZone].Points.Add(ObjectManager.Me.Position);
                        lastRotation = (int)nManager.Helpful.Math.RadianToDegree(ObjectManager.Me.Rotation);
                        refreshForm();
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
                        _profile.GrinderZones[idZone].Points.RemoveAt(listPoint.SelectedIndex);
                    refreshForm();
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
                        _profile.GrinderZones[idZone].BlackListRadius.RemoveAt(listBlackRadius.SelectedIndex);
                refreshForm();
            }
            catch (Exception ex)
            {
                Logging.WriteError("Grinder > Bot > ProfileCreator > delBlackRadius_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void addBlackB_Click(object sender, EventArgs e)
        {
            try
            {
                _profile.GrinderZones[idZone].BlackListRadius.Add(new GrinderBlackListRadius { Position = ObjectManager.Me.Position, Radius = radiusN.Value });
                refreshForm();
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
                    _profile.GrinderZones[idZone].Npc.RemoveAt(listNpc.SelectedIndex);
                refreshForm();
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
                    ContinentId =
                        (nManager.Wow.Enums.ContinentId)(nManager.Wow.Helpers.Usefuls.ContinentId),
                    Entry = ObjectManager.Target.Entry,
                    Faction =
                        (Npc.FactionType)
                        Enum.Parse(typeof(Npc.FactionType), ObjectManager.Me.PlayerFaction, true),
                    Name = ObjectManager.Target.Name,
                    Position = ObjectManager.Target.Position,
                    Type = (Npc.NpcType)Enum.Parse(typeof(Npc.NpcType), npcTypeC.Text, true)
                };
                _profile.GrinderZones[idZone].Npc.Add(npc);
                refreshForm();
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

                var gameObjects = ObjectManager.GetWoWGameObjectByName(nameNpcTb.Text);

                if (gameObjects.Count > 0)
                {
                    var gameObject = ObjectManager.GetNearestWoWGameObject(gameObjects);
                    if (gameObject.IsValid)
                    {
                        npc.Entry = gameObject.Entry;
                        npc.Position = gameObject.Position;
                        npc.Name = gameObject.Name;
                    }
                }

                if (npc.Entry <= 0)
                {
                    var units = ObjectManager.GetWoWUnitByName(nameNpcTb.Text);
                    if (units.Count > 0)
                    {
                        var unit = ObjectManager.GetNearestWoWUnit(units);
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
                    MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.No_found) + ".");
                    return;
                }

                npc.ContinentId =
                    (nManager.Wow.Enums.ContinentId)(nManager.Wow.Helpers.Usefuls.ContinentId);
                npc.Faction =
                    (Npc.FactionType)
                    Enum.Parse(typeof(Npc.FactionType), ObjectManager.Me.PlayerFaction, true);
                npc.Type = (Npc.NpcType)Enum.Parse(typeof(Npc.NpcType), npcTypeC.Text, true);

                if (nManager.Wow.Helpers.Usefuls.IsOutdoors)
                    npc.Position.Type = "Flying";

                _profile.GrinderZones[idZone].Npc.Add(npc);
                refreshForm();
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
                idZone = listZoneCb.SelectedIndex;
                refreshForm();
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
                _profile.GrinderZones.Add(new GrinderZone { Name = nManager.Wow.Helpers.Usefuls.MapZoneName });
                idZone = _profile.GrinderZones.Count - 1;
                refreshListZones();
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
                _profile.GrinderZones.RemoveAt(idZone);
                idZone = _profile.GrinderZones.Count - 1;
                refreshListZones();
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
                _profile.GrinderZones[idZone].Name = zoneNameTb.Text;
                refreshListZones();
            }
            catch (Exception ex)
            {
                Logging.WriteError("zoneNameTb_TextChanged(object sender, EventArgs e): " + ex);
            }
        }

        private void Level_ValueChanged(object sender, EventArgs e)
        {
            lock (typeof(ProfileCreator))
            {
                try
                {
                    // Player Level
                    _profile.GrinderZones[idZone].MaxLevel = (uint)maxLevelPlayer.Value;
                    _profile.GrinderZones[idZone].MinLevel = (uint)minLevelPlayer.Value;
                }
                catch
                {
                }
                try
                {
                    // Target Level
                    _profile.GrinderZones[idZone].MaxTargetLevel = (uint)maxLevelTarget.Value;
                    _profile.GrinderZones[idZone].MinTargetLevel = (uint)minLevelTarget.Value;
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
                lock (typeof(ProfileCreator))
                {
                    if (ObjectManager.Target.IsValid)
                    {
                        var text = listEntryTb.Text;
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
                var text = listEntryTb.Text;
                lock (typeof(ProfileCreator))
                {
                    if (text.Replace(" ", "").Replace(";", "").Length > 0)
                    {
                        string[] factionTempsString = text.Replace(" ", "").Split(Convert.ToChar(";"));
                        _profile.GrinderZones[idZone].TargetEntry.Clear();
                        foreach (string t in factionTempsString)
                        {
                            try
                            {
                                if (t != "")
                                    if (!_profile.GrinderZones[idZone].TargetEntry.Contains(Convert.ToInt32(t)))
                                        _profile.GrinderZones[idZone].TargetEntry.Add(Convert.ToInt32(t));
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