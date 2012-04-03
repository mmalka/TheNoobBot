using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using WowManager;
using WowManager.Others;
using WowManager.UserInterfaceHelper;
using WowManager.WoW.ObjectManager;
using Point = WowManager.MiscStructs.Point;

namespace Questing_Bot.Grinding
{
    public partial class CreateProfile : XtraForm
    {
        private int _currentIdSubProfil;
        private bool _loopRecordPoint;
        private bool _loopRecordPointGhost;
        private bool _loopRecordPointToThis;
        private Profile _profile = new Profile();
        Radar _radarForm = new Radar();

        // Update Form by profile variable

        // Update SubProfile Form by profile variable
        private bool _usedUpdate;

        public CreateProfile()
        {
            InitializeComponent();
            Translate();
            UpdateFormByProfil();
        }

        void Translate()
        {
            label1.Text = Translation.GetText(Translation.Text.SubProfil);
            label2.Text = Translation.GetText(Translation.Text.Profiles_name);
            subProfileFD.Text = Translation.GetText(Translation.Text.Del);
            subProfileFA.Text = Translation.GetText(Translation.Text.Add);
            groupBox1.Text = Translation.GetText(Translation.Text.SubProfil);
            loadF.Text = Translation.GetText(Translation.Text.Load_Profile);
            saveF.Text = Translation.GetText(Translation.Text.Save_Profile);
            label11.Text = Translation.GetText(Translation.Text.MailBox);
            mailboxesFA.Text = Translation.GetText(Translation.Text.Add);
            mailboxesFD.Text = Translation.GetText(Translation.Text.Del);
            label12.Text = Translation.GetText(Translation.Text.List_of_Location) + ":";
            locationsFD.Text = Translation.GetText(Translation.Text.Del);
            vendorsFA.Text = Translation.GetText(Translation.Text.Add);
            locationsFA.Text = Translation.GetText(Translation.Text.Record_Way);
            label10.Text = Translation.GetText(Translation.Text.Vendor);
            vendorsFD.Text = Translation.GetText(Translation.Text.Del);
            label13.Text = Translation.GetText(Translation.Text.Target_Faction);
            label7.Text = Translation.GetText(Translation.Text.Max);
            hotSpotF.Properties.Caption = Translation.GetText(Translation.Text.Hot_Spot);
            label8.Text = Translation.GetText(Translation.Text.Min);
            trainerFD.Text = Translation.GetText(Translation.Text.Del);
            label9.Text = Translation.GetText(Translation.Text.Target_level);
            label14.Text = Translation.GetText(Translation.Text.Trainers_class);
            trainerFA.Text = Translation.GetText(Translation.Text.Add);
            label6.Text = Translation.GetText(Translation.Text.Max);
            LFirstPointInfo.Text = Translation.GetText(Translation.Text.First_Location) + " " + Translation.GetText(Translation.Text.Distance);
            randomF.Properties.Caption = Translation.GetText(Translation.Text.Order_Random);
            label5.Text = Translation.GetText(Translation.Text.Min);
            label4.Text = Translation.GetText(Translation.Text.Level);
            GhostBDel.Text = Translation.GetText(Translation.Text.Del);
            GhostBAdd.Text = Translation.GetText(Translation.Text.Rec);
            label15.Text = Translation.GetText(Translation.Text.Ghost);
            label3.Text = Translation.GetText(Translation.Text.Name);
            factionsFA.Text = Translation.GetText(Translation.Text.Add);
            ToThisSubProfileDel.Text = Translation.GetText(Translation.Text.Del);
            ToThisSubProfileAdd.Text = Translation.GetText(Translation.Text.Rec);
            label16.Text = Translation.GetText(Translation.Text.Path_to_this_SubProfile);
            BlackListDel.Text = Translation.GetText(Translation.Text.Del);
            BlackListAdd.Text = Translation.GetText(Translation.Text.Add);
            label17.Text = Translation.GetText(Translation.Text.Black_List_zone);
        }

        private void UpdateFormByProfil()
        {
            nameProfileF.Text = _profile.Name;
            UpdateSubProfilFormByProfil();
        }

        private void UpdateSubProfilFormByProfil(int idSubProdil)
        {
            _currentIdSubProfil = idSubProdil;
            UpdateSubProfilFormByProfil();
        }

        private void UpdateSubProfilFormByProfil()
        {
            if (!_usedUpdate)
            {
                _usedUpdate = true;

                if (_currentIdSubProfil > _profile.SubProfiles.Count - 1)
                    _currentIdSubProfil = 0;
                if (_profile.SubProfiles.Count == 0)
                {
                    _profile.SubProfiles = new List<SubProfile> {new SubProfile("SubProfil 1")};
                }

                subProfileF.Properties.Items.Clear();
                for (int i = 0; i <= _profile.SubProfiles.Count - 1; i++)
                {
                    subProfileF.Properties.Items.Add(((i + 1) + " - " + _profile.SubProfiles[i].Name));
                }
                subProfileF.Text = (_currentIdSubProfil + 1) + " - " + _profile.SubProfiles[_currentIdSubProfil].Name;


                nameF.Text = _profile.SubProfiles[_currentIdSubProfil].Name;

                if (_profile.SubProfiles[_currentIdSubProfil].MinLevel <= minLevelF.Minimum ||
                    _profile.SubProfiles[_currentIdSubProfil].MinLevel >= minLevelF.Maximum)
                    _profile.SubProfiles[_currentIdSubProfil].MinLevel = (int) minLevelF.Minimum;
                if (_profile.SubProfiles[_currentIdSubProfil].MaxLevel <= maxLevelF.Minimum ||
                    _profile.SubProfiles[_currentIdSubProfil].MaxLevel >= maxLevelF.Maximum)
                    _profile.SubProfiles[_currentIdSubProfil].MaxLevel = (int) maxLevelF.Maximum;
                minLevelF.Value = _profile.SubProfiles[_currentIdSubProfil].MinLevel;
                maxLevelF.Value = _profile.SubProfiles[_currentIdSubProfil].MaxLevel;

                if (_profile.SubProfiles[_currentIdSubProfil].TargetMinLevel <= targetMinLevelF.Minimum ||
                    _profile.SubProfiles[_currentIdSubProfil].TargetMinLevel >= targetMinLevelF.Maximum)
                    _profile.SubProfiles[_currentIdSubProfil].TargetMinLevel = (int) targetMinLevelF.Minimum;
                if (_profile.SubProfiles[_currentIdSubProfil].TargetMaxLevel <= targetMaxLevelF.Minimum ||
                    _profile.SubProfiles[_currentIdSubProfil].TargetMaxLevel >= targetMaxLevelF.Maximum)
                    _profile.SubProfiles[_currentIdSubProfil].TargetMaxLevel = (int) targetMaxLevelF.Maximum;
                targetMinLevelF.Value = _profile.SubProfiles[_currentIdSubProfil].TargetMinLevel;
                targetMaxLevelF.Value = _profile.SubProfiles[_currentIdSubProfil].TargetMaxLevel;

                vendorsF.Items.Clear();
                for (int i = 0; i <= _profile.SubProfiles[_currentIdSubProfil].Vendors.Count - 1; i++)
                {
                    vendorsF.Items.Add((_profile.SubProfiles[_currentIdSubProfil].Vendors[i].ToString()));
                }

                factionsF.Text = "";
                for (int i = 0; i <= _profile.SubProfiles[_currentIdSubProfil].Factions.Count - 1; i++)
                {
                    if (factionsF.Text != "")
                        factionsF.Text = factionsF.Text + " ; ";

                    factionsF.Text = factionsF.Text + _profile.SubProfiles[_currentIdSubProfil].Factions[i];
                }

                mailboxesF.Items.Clear();
                for (int i = 0; i <= _profile.SubProfiles[_currentIdSubProfil].Mailboxes.Count - 1; i++)
                {
                    mailboxesF.Items.Add((_profile.SubProfiles[_currentIdSubProfil].Mailboxes[i].ToString()));
                }

                locationsF.Items.Clear();
                for (int i = 0; i <= _profile.SubProfiles[_currentIdSubProfil].Locations.Count - 1; i++)
                {
                    locationsF.Items.Add((_profile.SubProfiles[_currentIdSubProfil].Locations[i].ToString()));
                }
                hotSpotF.Checked = _profile.SubProfiles[_currentIdSubProfil].Hotspots;
                randomF.Checked = _profile.SubProfiles[_currentIdSubProfil].random;
                if (hotSpotF.Checked)
                {
                    locationsFA.Text = Translation.GetText(Translation.Text.Add_HotSpot);
                    randomF.Visible = true;
                }
                else
                {
                    randomF.Visible = false;
                    locationsFA.Text = !_loopRecordPoint ? Translation.GetText(Translation.Text.Record_Way) : Translation.GetText(Translation.Text.Stop_Record_Way);
                }

                GhostF.Items.Clear();
                for (int i = 0; i <= _profile.SubProfiles[_currentIdSubProfil].GhostLocations.Count - 1; i++)
                {
                    GhostF.Items.Add((_profile.SubProfiles[_currentIdSubProfil].GhostLocations[i].ToString()));
                }

                ToThisSubProfileF.Items.Clear();
                for (int i = 0; i <= _profile.SubProfiles[_currentIdSubProfil].ToSubProfileLocations.Count - 1; i++)
                {
                    ToThisSubProfileF.Items.Add(
                        (_profile.SubProfiles[_currentIdSubProfil].ToSubProfileLocations[i].ToString()));
                }

                trainerF.Items.Clear();
                for (int i = 0; i <= _profile.SubProfiles[_currentIdSubProfil].Trainers.Count - 1; i++)
                {
                    trainerF.Items.Add((_profile.SubProfiles[_currentIdSubProfil].Trainers[i].WowClass + " - " +
                                        _profile.SubProfiles[_currentIdSubProfil].Trainers[i].Point));
                }

                BlackListF.Items.Clear();
                for (int i = 0; i <= _profile.SubProfiles[_currentIdSubProfil].BlackListZones.Count - 1; i++)
                {
                    BlackListF.Items.Add((_profile.SubProfiles[_currentIdSubProfil].BlackListZones[i].Radius + " - " +
                                          _profile.SubProfiles[_currentIdSubProfil].BlackListZones[i].Point));
                }

                _usedUpdate = false;
            }
        }

        // Save Profile to File
        private void SaveProfile()
        {
            string file = Others.DialogBox_SaveFile(Application.StartupPath + "\\Products\\Questing Bot\\Profiles Grinding\\",
                                                    "Profile files (*.xml)|*.xml|All files (*.*)|*.*");

            if (file != "")
            {
                XmlSerializerHelper.Serialize(file, _profile);
            }
        }

        // Load profile by File
        private void LoadProfile()
        {
            string file = Others.DialogBox_OpenFile(Application.StartupPath + "\\Products\\Questing Bot\\Profiles Grinding\\",
                                                    "Profile files (*.xml)|*.xml|All files (*.*)|*.*");

            if (file != "")
            {
                try
                {
                    _profile = XmlSerializerHelper.Deserialize<Profile>(file);
                }
                catch (Exception e)
                {
                    MessageBox.Show(Translation.GetText(Translation.Text.Profile_error) + ": " + e);
                }
                _currentIdSubProfil = 0;
                UpdateFormByProfil();
            }
        }

        // PROFILE NAME
        // Text Change
        private void NameProfileFTextChanged(object sender, EventArgs e)
        {
            _profile.Name = nameProfileF.Text;
        }

        // SUB PROFILE LIST
        // Add SubProfile
        private void SubProfileFaClick(object sender, EventArgs e)
        {
            _loopRecordPoint = false;
            _loopRecordPointGhost = false;
            _loopRecordPointToThis = false;
            _profile.SubProfiles.Add(new SubProfile(Translation.GetText(Translation.Text.SubProfil) + " " + (_profile.SubProfiles.Count + 1)));
            UpdateSubProfilFormByProfil(_profile.SubProfiles.Count - 1);
        }

        // Del Sub Profil
        private void SubProfileFdClick(object sender, EventArgs e)
        {
            _loopRecordPoint = false;
            _loopRecordPointGhost = false;
            _loopRecordPointToThis = false;
            string[] idTempsString = subProfileF.Text.Replace(" ", "").Split(Convert.ToChar("-"));
            if (idTempsString.Length > 0)
                _profile.SubProfiles.RemoveAt(Convert.ToInt32(idTempsString.GetValue(0)) - 1);
            UpdateSubProfilFormByProfil();
        }

        // Select others Sub profile
        private void SubProfileFSelectedIndexChanged(object sender, EventArgs e)
        {
            string[] idTempsString = subProfileF.Text.Replace(" ", "").Split(Convert.ToChar("-"));
            if (idTempsString.Length > 0)
                UpdateSubProfilFormByProfil(Convert.ToInt32(idTempsString.GetValue(0)) - 1);
        }

        //NAME SUBPROFIL
        // Text change
        private void NameFTextChanged(object sender, EventArgs e)
        {
            _profile.SubProfiles[_currentIdSubProfil].Name = nameF.Text;
            UpdateSubProfilFormByProfil();
        }

        // LEVEL
        // MinLevel change value
        private void MinLevelFValueChanged(object sender, EventArgs e)
        {
            _profile.SubProfiles[_currentIdSubProfil].MinLevel = (int) minLevelF.Value;
        }

        // MaxLevel change value
        private void MaxLevelFValueChanged(object sender, EventArgs e)
        {
            _profile.SubProfiles[_currentIdSubProfil].MaxLevel = (int) maxLevelF.Value;
        }

        // TARGET LEVEL
        // TargetMinLevel
        private void TargetMinLevelFValueChanged(object sender, EventArgs e)
        {
            _profile.SubProfiles[_currentIdSubProfil].TargetMinLevel = (int) targetMinLevelF.Value;
        }

        // TargetMaxLevel
        private void TargetMaxLevelFValueChanged(object sender, EventArgs e)
        {
            _profile.SubProfiles[_currentIdSubProfil].TargetMaxLevel = (int) targetMaxLevelF.Value;
        }

        // TARGET FACTION
        // Update faction profile
        private void FactionsFTextChanged(object sender, EventArgs e)
        {
            if (factionsF.Text.Replace(" ", "").Replace(";", "").Length > 0 && !_usedUpdate)
            {
                string[] factionTempsString = factionsF.Text.Replace(" ", "").Split(Convert.ToChar(";"));
                _profile.SubProfiles[_currentIdSubProfil].Factions.Clear();
                foreach (string t in factionTempsString)
                {
                    try
                    {
                        if (t != "")
                            _profile.SubProfiles[_currentIdSubProfil].Factions.Add(Convert.ToUInt32(t));
                    }
                    catch
                    {
                    }
                }
            }
        }

        // Add faction target
        private void FactionsFaClick(object sender, EventArgs e)
        {
            try
            {
                if (ObjectManager.Target.GetBaseAddress > 0)
                {
                    if (factionsF.Text.Replace(" ", "").Replace(";", "").Length > 0)
                        factionsF.Text = factionsF.Text + " ; ";
                    factionsF.Text = factionsF.Text + ObjectManager.Target.Faction;
                }
            }
            catch
            {
            }
        }

        // LOCATIONS LISTE
        // Del Location
        private void LocationsFdClick(object sender, EventArgs e)
        {
            if (locationsF.SelectedIndex >= 0)
                _profile.SubProfiles[_currentIdSubProfil].Locations.RemoveAt(locationsF.SelectedIndex);
            UpdateSubProfilFormByProfil();
        }

        // Record Location
        private void LocationsFaClick(object sender, EventArgs e)
        {
            if (hotSpotF.Checked)
            {
                _profile.SubProfiles[_currentIdSubProfil].Locations.Add(ObjectManager.Me.Position);
                UpdateSubProfilFormByProfil();
            }
            else if (_loopRecordPoint)
            {
                StopRecordWay();
                saveF.Enabled = true;
                loadF.Enabled = true;
                hotSpotF.Enabled = true;
                locationsFA.Text = Translation.GetText(Translation.Text.Record_Way);
            }
            else
            {
                saveF.Enabled = false;
                loadF.Enabled = false;
                hotSpotF.Enabled = false;
                LoopRecordWay();
            }
        }

        private void LoopRecordWay()
        {
            const float distanceSeparator = 5.0f;
            const float distanceZSeparator = 5.0f;

            _profile.SubProfiles[_currentIdSubProfil].Locations.Add(ObjectManager.Me.Position);
            UpdateSubProfilFormByProfil();

            _loopRecordPoint = true;
            int lastRotation = 0;
            while (_loopRecordPoint)
            {
                try
                {
                    locationsFA.Text = Translation.GetText(Translation.Text.Stop_Record_Way);
                    Point lastPoint =
                        _profile.SubProfiles[_currentIdSubProfil].Locations[
                            _profile.SubProfiles[_currentIdSubProfil].Locations.Count - 1];
                    float disZTemp = lastPoint.DistanceZ(ObjectManager.Me.Position);


                    if (((lastPoint.DistanceTo(ObjectManager.Me.Position) > distanceSeparator) &&
                         lastRotation != (int)Others.RadianToDegree(ObjectManager.Me.Rotation)) ||
                        disZTemp >= distanceZSeparator)
                    {
                        _profile.SubProfiles[_currentIdSubProfil].Locations.Add(ObjectManager.Me.Position);
                        lastRotation = (int)Others.RadianToDegree(ObjectManager.Me.Rotation);
                        UpdateSubProfilFormByProfil();
                        locationsF.SelectedIndex = locationsF.Items.Count - 1;
                    }
                }
                catch { }
                Application.DoEvents();
                Thread.Sleep(50);
            }
            locationsFA.Text = Translation.GetText(Translation.Text.Record_Way);
        }

        private void StopRecordWay()
        {
            _loopRecordPoint = false;
        }

        private void HotSpotFCheckedChanged(object sender, EventArgs e)
        {
            _profile.SubProfiles[_currentIdSubProfil].Hotspots = hotSpotF.Checked;
            UpdateSubProfilFormByProfil();
        }

        private void RandomFCheckedChanged(object sender, EventArgs e)
        {
            _profile.SubProfiles[_currentIdSubProfil].random = randomF.Checked;
            UpdateSubProfilFormByProfil();
        }

        // GHOST
        private void GhostBDelClick(object sender, EventArgs e)
        {
            if (GhostF.SelectedIndex >= 0)
                _profile.SubProfiles[_currentIdSubProfil].GhostLocations.RemoveAt(GhostF.SelectedIndex);
            UpdateSubProfilFormByProfil();
        }

        // Del
        // Rec button

        private void GhostBAddClick(object sender, EventArgs e)
        {
            if (_loopRecordPointGhost)
            {
                StopRecordWayGhost();
                GhostBAdd.Text = Translation.GetText(Translation.Text.Rec);
            }
            else
            {
                GhostBAdd.Text = Translation.GetText(Translation.Text.Stop);
                LoopRecordWayGhost();
            }
        }

        private void LoopRecordWayGhost()
        {
            const float distanceSeparator = 5.0f;
            const float distanceZSeparator = 5.0f;
            int lastRotation = 0;

            _profile.SubProfiles[_currentIdSubProfil].GhostLocations.Add(ObjectManager.Me.Position);
            UpdateSubProfilFormByProfil();

            _loopRecordPointGhost = true;
            while (_loopRecordPointGhost)
            {
                Point lastPoint =
                    _profile.SubProfiles[_currentIdSubProfil].GhostLocations[
                        _profile.SubProfiles[_currentIdSubProfil].GhostLocations.Count - 1];
                float disZTemp = lastPoint.DistanceZ(ObjectManager.Me.Position);

                if (((lastPoint.DistanceTo(ObjectManager.Me.Position) > distanceSeparator) &&
                     lastRotation != (int) Others.RadianToDegree(ObjectManager.Me.Rotation)) ||
                    disZTemp >= distanceZSeparator)
                {
                    _profile.SubProfiles[_currentIdSubProfil].GhostLocations.Add(ObjectManager.Me.Position);
                    lastRotation = (int) Others.RadianToDegree(ObjectManager.Me.Rotation);
                    UpdateSubProfilFormByProfil();
                    GhostF.SelectedIndex = GhostF.Items.Count - 1;
                }
                Application.DoEvents();
                Thread.Sleep(50);
            }
        }

        private void StopRecordWayGhost()
        {
            _loopRecordPointGhost = false;
        }

        // MAILBOX LISTE
        // Del mailbox
        private void MailboxesFdClick(object sender, EventArgs e)
        {
            if (mailboxesF.SelectedIndex >= 0)
                _profile.SubProfiles[_currentIdSubProfil].Mailboxes.RemoveAt(mailboxesF.SelectedIndex);
            UpdateSubProfilFormByProfil();
        }

        // Add mailbox
        private void MailboxesFaClick(object sender, EventArgs e)
        {
            _profile.SubProfiles[_currentIdSubProfil].Mailboxes.Add(ObjectManager.Me.Position);
            UpdateSubProfilFormByProfil();
            mailboxesF.SelectedIndex = mailboxesF.Items.Count - 1;
        }

        // VENDORS LISTE
        // Del vendor
        private void VendorsFdClick(object sender, EventArgs e)
        {
            if (vendorsF.SelectedIndex >= 0)
                _profile.SubProfiles[_currentIdSubProfil].Vendors.RemoveAt(vendorsF.SelectedIndex);
            UpdateSubProfilFormByProfil();
        }

        // Add vendor
        private void VendorsFaClick(object sender, EventArgs e)
        {
            _profile.SubProfiles[_currentIdSubProfil].Vendors.Add(ObjectManager.Me.Position);
            UpdateSubProfilFormByProfil();
            vendorsF.SelectedIndex = vendorsF.Items.Count - 1;
        }

        // TRAINER
        // Del trainer
        private void TrainerFdClick(object sender, EventArgs e)
        {
            _profile.SubProfiles[_currentIdSubProfil].Trainers.RemoveAt(trainerF.SelectedIndex);
            UpdateSubProfilFormByProfil();
        }

        // Add trainer
        private void TrainerFaClick(object sender, EventArgs e)
        {
            _profile.SubProfiles[_currentIdSubProfil].Trainers.Add(new Trainers(ObjectManager.Me.Position,
                                                                              classTrainer.Text));
            UpdateSubProfilFormByProfil();
            trainerF.SelectedIndex = trainerF.Items.Count - 1;
        }

        // SAVE
        private void SaveFClick(object sender, EventArgs e)
        {
            saveF.Enabled = false;
            saveF.Text = Translation.GetText(Translation.Text.Wait) + "...";
            try
            {
                SaveProfile();
            }
            catch (Exception er)
            {
                MessageBox.Show(Translation.GetText(Translation.Text.Profile_error) + ": " + er);
                Log.AddLog(Translation.GetText(Translation.Text.Profile_error));
            }
            saveF.Text = Translation.GetText(Translation.Text.Save);
            saveF.Enabled = true;
        }

        // LOAD
        private void LoadFClick(object sender, EventArgs e)
        {
            LoadProfile();
        }

        private void UpdateFirstPointInfoTick(object sender, EventArgs e)
        {
            if (_profile.SubProfiles[_currentIdSubProfil].Locations.Count > 0)
            {
                try
                {
                    // Add info for the radar
                    var listObjTemps = new List<Radar.ObjDraw>();
                    var listLineTemps = new List<Radar.LineDraw>();
                    var points = (from object objectListBox in locationsF.Items
                                  select
                                      Convert.ToString(objectListBox.ToString()).Replace(" ", "").Split(
                                          Convert.ToChar(";"))
                                      into pointArrays
                                      select
                                          new Point(Convert.ToSingle(pointArrays[0]), Convert.ToSingle(pointArrays[1]),
                                                    Convert.ToSingle(pointArrays[2]))).ToList();
                    var lastPos = new Point(points[0]);
                    bool first = true;
                    foreach (var p in points)
                    {
                        listLineTemps.Add(new Radar.LineDraw { ColorDraw = Color.Green, PositionFrom = lastPos, PositionTo = p });
                        listObjTemps.Add(first
                                             ? new Radar.ObjDraw { ColorDraw = Color.Yellow, Position = p }
                                             : new Radar.ObjDraw { ColorDraw = Color.Green, Position = p });
                        first = false;
                        lastPos = p;
                    }
                    _radarForm.ListObjDraw = listObjTemps;
                    _radarForm.ListLineDraw = listLineTemps;
                }
                catch
                {
                }

                // gui info
                try
                {
                    var distanceFirstPoint =
                        (int) _profile.SubProfiles[_currentIdSubProfil].Locations[0].DistanceTo(ObjectManager.Me.Position);

                    var disZTemp =
                        (int) _profile.SubProfiles[_currentIdSubProfil].Locations[0].DistanceZ(ObjectManager.Me.Position);

                    LFirstPointInfo.Text = Translation.GetText(Translation.Text.First_Location) + " " + Translation.GetText(Translation.Text.Distance) + ": " + distanceFirstPoint + " " + Translation.GetText(Translation.Text.Distance) + " Z=" + disZTemp;
                }
                catch
                {
                    LFirstPointInfo.Text = Translation.GetText(Translation.Text.First_Location_of_Profile_No_info);
                }
            }
            else
            {
                LFirstPointInfo.Text = Translation.GetText(Translation.Text.First_Location_of_Profile_No_info);
            }
        }

        private void Create_Profile_Shown(object sender, EventArgs e)
        {
        }

        private void CreateProfileFormClosing(object sender, FormClosingEventArgs e)
        {
            StopRecordWay();
        }

        // TO THIS SUBPROFILE
        // Rec loop
        private void ToThisSubProfileAddClick(object sender, EventArgs e)
        {
            if (_loopRecordPointToThis)
            {
                StopRecordWayToThis();
                ToThisSubProfileAdd.Text = Translation.GetText(Translation.Text.Rec);
            }
            else
            {
                ToThisSubProfileAdd.Text = Translation.GetText(Translation.Text.Stop);
                LoopRecordWayToThis();
            }
        }

        private void LoopRecordWayToThis()
        {
            const float distanceSeparator = 5.0f;
            const float distanceZSeparator = 5.0f;
            int lastRotation = 0;

            _profile.SubProfiles[_currentIdSubProfil].ToSubProfileLocations.Add(ObjectManager.Me.Position);
            UpdateSubProfilFormByProfil();

            _loopRecordPointToThis = true;
            while (_loopRecordPointToThis)
            {
                Point lastPoint =
                    _profile.SubProfiles[_currentIdSubProfil].ToSubProfileLocations[
                        _profile.SubProfiles[_currentIdSubProfil].ToSubProfileLocations.Count - 1];
                float disZTemp = lastPoint.DistanceZ(ObjectManager.Me.Position);

                if (((lastPoint.DistanceTo(ObjectManager.Me.Position) > distanceSeparator) &&
                     lastRotation != (int) Others.RadianToDegree(ObjectManager.Me.Rotation)) ||
                    disZTemp >= distanceZSeparator)
                {
                    _profile.SubProfiles[_currentIdSubProfil].ToSubProfileLocations.Add(ObjectManager.Me.Position);
                    lastRotation = (int) Others.RadianToDegree(ObjectManager.Me.Rotation);
                    UpdateSubProfilFormByProfil();
                    ToThisSubProfileF.SelectedIndex = ToThisSubProfileF.Items.Count - 1;
                }
                Application.DoEvents();
                Thread.Sleep(50);
            }
        }

        private void StopRecordWayToThis()
        {
            _loopRecordPointToThis = false;
        }

        // Del
        private void ToThisSubProfileDelClick(object sender, EventArgs e)
        {
            if (ToThisSubProfileF.SelectedIndex >= 0)
                _profile.SubProfiles[_currentIdSubProfil].ToSubProfileLocations.RemoveAt(ToThisSubProfileF.SelectedIndex);
            UpdateSubProfilFormByProfil();
        }

        // BACKLIST ZONE
        //Add
        private void BlackListAddClick(object sender, EventArgs e)
        {
            var t = new BlackListZone {Point = ObjectManager.Me.Position, Radius = (float) BlackListRadius.Value};
            _profile.SubProfiles[_currentIdSubProfil].BlackListZones.Add(t);
            UpdateSubProfilFormByProfil();
            BlackListF.SelectedIndex = BlackListF.Items.Count - 1;
        }

        // Del
        private void BlackListDelClick(object sender, EventArgs e)
        {
            if (BlackListF.SelectedIndex >= 0)
                _profile.SubProfiles[_currentIdSubProfil].BlackListZones.RemoveAt(BlackListF.SelectedIndex);
            UpdateSubProfilFormByProfil();
        }

        private void RadarB_Click(object sender, EventArgs e)
        {
            _radarForm = new Radar();
            _radarForm.Show();
        }
    }
}