using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using nManager;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using Math = nManager.Helpful.Math;

namespace Battlegrounder.Profile
{
    public partial class ProfileCreator : Form
    {
        private int _idZone;
        private bool _loopRecordPoint;
        private BattlegrounderProfile _profile = new BattlegrounderProfile();

        public ProfileCreator()
        {
            try
            {
                InitializeComponent();
                Translate();
                RefreshListZones();
                CurrentBattlegroundInfo();
                if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
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
            MainHeader.TitleText = nManager.Translate.Get(nManager.Translate.Id.Profile_Creator) + " - " + Information.MainTitle;
            this.Text = MainHeader.TitleText;
        }

        private void SaveButton_Click(object sender,
            EventArgs ex)
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


        private void LoadButton_Click(object sender,
            EventArgs e)
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
                    RefreshListZones();
                    RefreshForm();
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError(
                    "Battlegrounder > Bot > ProfileCreator > LoadButton_Click(object sender, EventArgs e): " +
                    ex);
                RefreshListZones();
                RefreshForm();
            }
        }

        private void ProfileCreator_FormClosing(object sender,
            FormClosingEventArgs ex)
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

        private void RefreshListZones()
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
                    foreach (BattlegrounderZone p in _profile.BattlegrounderZones)
                    {
                        // if (!listZoneCb.Items.Contains(p.Name))
                        ZoneList.Items.Add(p.Name);
                    }
                    if (ZoneList.SelectedIndex != _idZone)
                    {
                        if (ZoneList.Items.Count >= _idZone + 1)
                            ZoneList.SelectedIndex = _idZone;
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
                    // Way
                    RecordedPoints.Items.Clear();
                    foreach (Point p in _profile.BattlegrounderZones[_idZone].Points)
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
                    foreach (BattlegrounderBlackListRadius b in _profile.BattlegrounderZones[_idZone].BlackListRadius)
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

        private void recordWayB_Click(object sender,
            EventArgs ex)
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

                _profile.BattlegrounderZones[_idZone].Points.Add(ObjectManager.Me.Position);
                RefreshForm();

                while (_loopRecordPoint)
                {
                    Point lastPoint =
                        _profile.BattlegrounderZones[_idZone].Points[
                            _profile.BattlegrounderZones[_idZone].Points.Count - 1];
                    float disZTemp = lastPoint.DistanceZ(ObjectManager.Me.Position);

                    if (((lastPoint.DistanceTo(ObjectManager.Me.Position) > (double) DistanceBetweenRecord.Value) &&
                         lastRotation != (int) Math.RadianToDegree(ObjectManager.Me.Rotation)) ||
                        disZTemp >= distanceZSeparator)
                    {
                        _profile.BattlegrounderZones[_idZone].Points.Add(ObjectManager.Me.Position);
                        lastRotation = (int) Math.RadianToDegree(ObjectManager.Me.Rotation);
                        RefreshForm();
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

        private void DeleteButton_Click(object sender,
            EventArgs ex)
        {
            try
            {
                if (CanRecord())
                {
                    if (RecordedPoints.SelectedIndex >= 0)
                        _profile.BattlegrounderZones[_idZone].Points.RemoveAt(RecordedPoints.SelectedIndex);
                    RefreshForm();
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
        private void DeleteButtonBlackListRadius_Click(object sender,
            EventArgs e)
        {
            try
            {
                if (CanRecord())
                {
                    if (RecordedBlackListRadius.SelectedIndex >= 0)
                        _profile.BattlegrounderZones[_idZone].BlackListRadius.RemoveAt(
                            RecordedBlackListRadius.SelectedIndex);
                    RefreshForm();
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError(
                    "Battlegrounder > Bot > ProfileCreator > DeleteButtonBlackListRadius_Click(object sender, EventArgs e): " +
                    ex);
            }
        }

        private void AddToBlackList_Click(object sender,
            EventArgs e)
        {
            try
            {
                if (CanRecord())
                {
                    _profile.BattlegrounderZones[_idZone].BlackListRadius.Add(new BattlegrounderBlackListRadius
                    {
                        Position =
                            ObjectManager.Me.Position,
                        Radius = (float) Radius.Value
                    });
                    RefreshForm();
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError(
                    "Battlegrounder > Bot > ProfileCreator > AddToBlackList_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void RefreshCurrentBattleground_Click(object sender,
            EventArgs e)
        {
            CurrentBattlegroundInfo();
        }

        public void CurrentBattlegroundInfo()
        {
            var bg = new Battleground();
            if (Usefuls.IsInBattleground)
            {
                if (!string.IsNullOrEmpty(bg.NonLocalizedName))
                    CurrentBattleground.Text = string.Format("Profile for the Battleground: {0}", bg.NonLocalizedName);
            }
            else
            {
                CurrentBattleground.Text = nManager.Translate.Get(nManager.Translate.Id.NotInBg);
            }
        }

        private bool CanRecord(bool justCheckIsInBg = false)
        {
            if (!Usefuls.IsInBattleground)
            {
                MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.NotInBg));
                return false;
            }
            if (justCheckIsInBg)
                return true;
            BattlegrounderZone bgz =
                _profile.BattlegrounderZones.Find(
                    bgzz => bgzz.Name == ZoneList.SelectedItem.ToString());
            if (bgz == null || Battleground.GetCurrentBattleground().ToString() != bgz.BattlegroundId)
            {
                MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.CantRecordInWrongZone));
                return false;
            }
            return true;
        }

        private void DelZoneButton_Click(object sender,
            EventArgs ex)
        {
            try
            {
                if (CanRecord(true))
                {
                    _profile.BattlegrounderZones.RemoveAt(_idZone);
                    _idZone = _profile.BattlegrounderZones.Count - 1;
                    RefreshListZones();
                }
            }
            catch (Exception e)
            {
                Logging.WriteError(
                    "Battlegrounder > Bot > ProfileCreator > DeleteButton_Click(object sender, EventArgs ex): " +
                    e);
            }
        }

        private void AddZoneButton_Click(object sender,
            EventArgs ex)
        {
            try
            {
                if (CanRecord(true))
                {
                    if (
                        _profile.BattlegrounderZones.Find(
                            bgz => bgz.BattlegroundId == Battleground.GetCurrentBattleground().ToString()) !=
                        null)
                    {
                        MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.CantDuplicateZone));
                        return;
                    }

                    var bg = new Battleground();
                    _profile.BattlegrounderZones.Add(new BattlegrounderZone
                    {
                        Name = bg.NonLocalizedName,
                        BattlegroundId =
                            Battleground.GetCurrentBattleground()
                                .ToString()
                    });
                    _idZone = _profile.BattlegrounderZones.Count - 1;
                    RefreshListZones();
                }
            }
            catch (Exception e)
            {
                Logging.WriteError(
                    "Battlegrounder > Bot > ProfileCreator > AddZoneButton_Click(object sender, EventArgs ex): " +
                    e);
            }
        }

        private void ZoneList_SelectedIndexChanged(object sender,
            EventArgs e)
        {
            try
            {
                if (_loopRecordPoint)
                {
                    _loopRecordPoint = false;
                    recordWayB.Text = nManager.Translate.Get(nManager.Translate.Id.Record_Way);
                }
                _idZone = ZoneList.SelectedIndex;
                RefreshForm();
            }
            catch (Exception ex)
            {
                Logging.WriteError("ZoneList_SelectedIndexChanged(object sender, EventArgs e): " + ex);
            }
        }
    }
}