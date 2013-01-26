using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

namespace Battlegrounder.Profile
{
    public partial class ProfileCreator : DevComponents.DotNetBar.Metro.MetroForm
    {
        private BattlegrounderProfile _profile = new BattlegrounderProfile();

        public ProfileCreator()
        {
            try
            {
                InitializeComponent();
                Translate();
                refreshListZones();
                CurrentBattlegroundInfo();
                TopMost = true;
            }
            catch (Exception e)
            {
                Logging.WriteError("Battlegrounder > Bot > ProfileCreator > ProfileCreator(): " + e);
            }
        }

        private void Translate()
        {
            recordWayB.Text = nManager.Translate.Get(nManager.Translate.Id.Record_Way);
            SaveButton.Text = nManager.Translate.Get(nManager.Translate.Id.Save);
            DistanceBetweenRecordLabel.Text = nManager.Translate.Get(nManager.Translate.Id.Separation_distance_record);
            DeleteButton.Text = nManager.Translate.Get(nManager.Translate.Id.Del);
            DeleteButtonBlackListRadius.Text = nManager.Translate.Get(nManager.Translate.Id.Del);
            AddToBlackList.Text = nManager.Translate.Get(nManager.Translate.Id.Add_this_position_to_Black_list_Radius);
            Text = nManager.Translate.Get(nManager.Translate.Id.Profile_Creator);
        }

        private void SaveButton_Click(object sender, EventArgs ex)
        {
            try
            {
                string file =
                    Others.DialogBoxSaveFile(Application.StartupPath + "\\Profiles\\Battlegrounder\\",
                                             "Profile files (*.xml)|*.xml|All files (*.*)|*.*");

                if (file != "")
                {
                    XmlSerializer.Serialize(file, _profile);
                }
            }
            catch (Exception e)
            {
                Logging.WriteError(
                    "Battlegrounder > Bot > ProfileCreator > SaveButton_Click(object sender, EventArgs ex): " + e);
            }
        }


        private void LoadButton_Click(object sender, EventArgs e)
        {
            try
            {
                string file =
                    Others.DialogBoxOpenFile(Application.StartupPath + "\\Profiles\\Battlegrounder\\",
                                             "Profile files (*.xml)|*.xml|All files (*.*)|*.*");

                if (File.Exists(file))
                {
                    _profile = new BattlegrounderProfile();
                    _profile = XmlSerializer.Deserialize<BattlegrounderProfile>(file);
                    refreshForm();
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError(
                    "Battlegrounder > Bot > ProfileCreator > LoadButton_Click(object sender, EventArgs e): " +
                    ex);
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
                Logging.WriteError(
                    "Battlegrounder > Bot > ProfileCreator > ProfileCreator_FormClosing(object sender, FormClosingEventArgs ex): " +
                    e);
            }
        }

        private int idZone;

        private void refreshListZones()
        {
            lock (typeof (ProfileCreator))
            {
                try
                {
                    // List Zones
                    ZoneList.Items.Clear();
                    if (_profile.BattlegrounderZones.Count <= 0)
                    {
                        AddZoneButton_Click(null, null);
                        return;
                    }
                    foreach (var p in _profile.BattlegrounderZones)
                    {
                        // if (!listZoneCb.Items.Contains(p.Name))
                        ZoneList.Items.Add(p.Name);
                    }
                    if (ZoneList.SelectedIndex != idZone)
                    {
                        ZoneList.SelectedIndex = idZone;
                    }
                }
                catch
                {
                }
            }
        }

        private void refreshForm()
        {
            lock (typeof (ProfileCreator))
            {
                try
                {
                    // Way
                    RecordedPoints.Items.Clear();
                    foreach (var p in _profile.BattlegrounderZones[idZone].Points)
                    {
                        RecordedPoints.Items.Add(p.ToString());
                    }
                    RecordedPoints.SelectedIndex = RecordedPoints.Items.Count - 1;
                }
                catch
                {
                }

                try
                {
                    // BlackList
                    RecordedBlackListRadius.Items.Clear();
                    foreach (var b in _profile.BattlegrounderZones[idZone].BlackListRadius)
                    {
                        RecordedBlackListRadius.Items.Add(b.Position.X + " ; " + b.Position.Y + " - " + b.Radius);
                    }
                    RecordedBlackListRadius.SelectedIndex = RecordedBlackListRadius.Items.Count - 1;
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
                    if (CanRecord())
                    {
                        _loopRecordPoint = true;
                        recordWayB.Text = nManager.Translate.Get(nManager.Translate.Id.Stop_Record_Way);
                        LoopRecordWay();
                    }
                    else
                    {
                        MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.NotInBg));
                    }
                }
            }
            catch (Exception e)
            {
                Logging.WriteError(
                    "Battlegrounder > Bot > ProfileCreator > recordWayB_Click(object sender, EventArgs ex): " + e);
            }
        }

        private void LoopRecordWay()
        {
            try
            {
                const float distanceZSeparator = 3.0f;
                int lastRotation = 0;
                _loopRecordPoint = true;

                _profile.BattlegrounderZones[idZone].Points.Add(ObjectManager.Me.Position);
                refreshForm();

                while (_loopRecordPoint)
                {
                    var lastPoint = _profile.BattlegrounderZones[idZone].Points[_profile.BattlegrounderZones[idZone].Points.Count - 1];
                    float disZTemp = lastPoint.DistanceZ(ObjectManager.Me.Position);

                    if (((lastPoint.DistanceTo(ObjectManager.Me.Position) > DistanceBetweenRecord.Value) &&
                         lastRotation != (int)nManager.Helpful.Math.RadianToDegree(ObjectManager.Me.Rotation)) ||
                        disZTemp >= distanceZSeparator)
                    {
                        _profile.BattlegrounderZones[idZone].Points.Add(ObjectManager.Me.Position);
                        lastRotation = (int)nManager.Helpful.Math.RadianToDegree(ObjectManager.Me.Rotation);
                        refreshForm();
                    }
                    Application.DoEvents();
                    Thread.Sleep(50);
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Battlegrounder > Bot > ProfileCreator > LoopRecordWay(): " + e);
            }
        }

        private void DeleteButton_Click(object sender, EventArgs ex)
        {
            try
            {
                if (CanRecord())
                {
                    if (RecordedPoints.SelectedIndex >= 0)
                        _profile.BattlegrounderZones[idZone].Points.RemoveAt(RecordedPoints.SelectedIndex);
                    refreshForm();
                }
                else
                {
                    MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.NotInBg));
                }
            }
            catch (Exception e)
            {
                Logging.WriteError(
                    "Battlegrounder > Bot > ProfileCreator > DeleteButton_Click(object sender, EventArgs ex): " +
                    e);
            }
        }

        // BLACK LIST
        private void DeleteButtonBlackListRadius_Click(object sender, EventArgs e)
        {
            try
            {
                if (CanRecord())
                {
                    if (RecordedBlackListRadius.SelectedIndex >= 0)
                        _profile.BattlegrounderZones[idZone].BlackListRadius.RemoveAt(
                            RecordedBlackListRadius.SelectedIndex);
                    refreshForm();
                }
                else
                {
                    MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.NotInBg));
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError(
                    "Battlegrounder > Bot > ProfileCreator > DeleteButtonBlackListRadius_Click(object sender, EventArgs e): " +
                    ex);
            }
        }

        private void AddToBlackList_Click(object sender, EventArgs e)
        {
            try
            {
                if (CanRecord())
                {
                    _profile.BattlegrounderZones[idZone].BlackListRadius.Add(new BattlegrounderBlackListRadius
                                                                                 {
                                                                                     Position = ObjectManager.Me.Position,
                                                                                     Radius = Radius.Value
                                                                                 });
                    refreshForm();
                }
                else
                {
                    MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.NotInBg));
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError(
                    "Battlegrounder > Bot > ProfileCreator > AddToBlackList_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void RecordedPoints_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void RecordedBlackListRadius_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void RefreshCurrentBattleground_Click(object sender, EventArgs e)
        {
            CurrentBattlegroundInfo();
        }

        public void CurrentBattlegroundInfo()
        {
            var Bg = new Battleground();
            if (Battleground.IsInBattleground())
            {
                if (!string.IsNullOrEmpty(Bg.NonLocalizedName))
                    CurrentBattleground.Text = string.Format("Profile for the Battleground: {0}", Bg.NonLocalizedName);
            }
            else
            {
                CurrentBattleground.Text = nManager.Translate.Get(nManager.Translate.Id.NotInBg);
            }
        }

        private bool CanRecord()
        {
            if (!Battleground.IsInBattleground())
                return false;
            return true;
        }

        private void ProfileCreator_Load(object sender, EventArgs e)
        {
        }

        private void DelZoneButton_Click(object sender, EventArgs ex)
        {
            try
            {
                if (CanRecord())
                {
                    _profile.BattlegrounderZones.RemoveAt(idZone);
                    idZone = _profile.BattlegrounderZones.Count - 1;
                    refreshListZones();
                }
                else
                {
                    MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.NotInBg));
                }
            }
            catch (Exception e)
            {
                Logging.WriteError(
                    "Battlegrounder > Bot > ProfileCreator > DeleteButton_Click(object sender, EventArgs ex): " +
                    e);
            }
        }

        private void AddZoneButton_Click(object sender, EventArgs ex)
        {
            try
            {
                if (CanRecord())
                {
                    var Bg = new Battleground();
                    _profile.BattlegrounderZones.Add(new BattlegrounderZone { Name = Bg.NonLocalizedName, BattlegroundId = Battleground.GetCurrentBattleground().ToString()});
                    idZone = _profile.BattlegrounderZones.Count - 1;
                    refreshListZones();
                }
                else
                {
                    MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.NotInBg));
                }
            }
            catch (Exception e)
            {
                Logging.WriteError(
                    "Battlegrounder > Bot > ProfileCreator > AddZoneButton_Click(object sender, EventArgs ex): " +
                    e);
            }
        }

        private void ZoneList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                idZone = ZoneList.SelectedIndex;
                refreshForm();
            }
            catch (Exception ex)
            {
                Logging.WriteError("ZoneList_SelectedIndexChanged(object sender, EventArgs e): " + ex);
            }
        }
    }
}