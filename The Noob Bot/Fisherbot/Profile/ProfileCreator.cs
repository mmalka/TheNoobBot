using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using nManager;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.ObjectManager;

namespace Fisherbot.Profile
{
    public partial class ProfileCreator : Form
    {
        private bool _loopRecordPoint;
        private FisherbotProfile _profile = new FisherbotProfile();

        public ProfileCreator()
        {
            try
            {
                InitializeComponent();
                Translate();
                if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                    TopMost = true;
            }
            catch (Exception e)
            {
                Logging.WriteError("Fisherbot > Bot > ProfileCreator > ProfileCreator(): " + e);
            }
        }

        private void Translate()
        {
            recordWayB.Text = nManager.Translate.Get(nManager.Translate.Id.Record_Way);
            SaveProfileButton.Text = nManager.Translate.Get(nManager.Translate.Id.Save);
            RecordingIntervalDistance.Text = nManager.Translate.Get(nManager.Translate.Id.Separation_distance_record);
            RemoveListPointButton.Text = nManager.Translate.Get(nManager.Translate.Id.Del);
            delBlackRadius.Text = nManager.Translate.Get(nManager.Translate.Id.Del);
            addBlackB.Text = nManager.Translate.Get(nManager.Translate.Id.Add_this_position_to_Black_list_Radius);
            MainHeader.TitleText = nManager.Translate.Get(nManager.Translate.Id.Profile_Creator) + " - " + Information.MainTitle;
            this.Text = MainHeader.TitleText;
        }

        private void saveB_Click(object sender, EventArgs ex)
        {
            try
            {
                string file =
                    Others.DialogBoxSaveFile(Application.StartupPath + "\\Profiles\\Fisherbot\\",
                        "Profile files (*.xml)|*.xml|All files (*.*)|*.*");

                if (file != "")
                {
                    XmlSerializer.Serialize(file, _profile);
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Fisherbot > Bot > ProfileCreator > saveB_Click(object sender, EventArgs ex): " + e);
            }
        }


        private void loadB_Click(object sender, EventArgs e)
        {
            try
            {
                string file =
                    Others.DialogBoxOpenFile(Application.StartupPath + "\\Profiles\\Fisherbot\\",
                        "Profile files (*.xml)|*.xml|All files (*.*)|*.*");

                if (File.Exists(file))
                {
                    _profile = new FisherbotProfile();
                    _profile = XmlSerializer.Deserialize<FisherbotProfile>(file);
                    RefreshForm();
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("Fisherbot > Bot > ProfileCreator > loadB_Click(object sender, EventArgs e): " + ex);
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
                    "Fisherbot > Bot > ProfileCreator > ProfileCreator_FormClosing(object sender, FormClosingEventArgs ex): " +
                    e);
            }
        }

        private void RefreshForm()
        {
            try
            {
                // Way
                ListOfPointsRecorded.Items.Clear();
                foreach (Point p in _profile.Points)
                {
                    ListOfPointsRecorded.Items.Add(p.ToString());
                }
                ListOfPointsRecorded.SelectedIndex = ListOfPointsRecorded.Items.Count - 1;
            }
            catch
            {
            }

            try
            {
                // BlackList
                BlacklistRadiusList.Items.Clear();
                foreach (FisherbotBlackListRadius b in _profile.BlackListRadius)
                {
                    BlacklistRadiusList.Items.Add(b.Position.X + " ; " + b.Position.Y + " - " + b.Radius);
                }
                BlacklistRadiusList.SelectedIndex = BlacklistRadiusList.Items.Count - 1;
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
                Logging.WriteError(
                    "Fisherbot > Bot > ProfileCreator > recordWayB_Click(object sender, EventArgs ex): " + e);
            }
        }

        private void LoopRecordWay()
        {
            try
            {
                const float distanceZSeparator = 5.0f;
                _loopRecordPoint = true;

                _profile.Points.Add(ObjectManager.Me.Position);
                RefreshForm();

                while (_loopRecordPoint)
                {
                    Point lastPoint = _profile.Points[_profile.Points.Count - 1];
                    float disZTemp = lastPoint.DistanceZ(ObjectManager.Me.Position);

                    if ((lastPoint.DistanceTo(ObjectManager.Me.Position) > (double) nSeparatorDistance.Value) ||
                        disZTemp >= distanceZSeparator)
                    {
                        _profile.Points.Add(ObjectManager.Me.Position);
                        RefreshForm();
                    }
                    Application.DoEvents();
                    Thread.Sleep(50);
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Fisherbot > Bot > ProfileCreator > LoopRecordWay(): " + e);
            }
        }

        private void delB_Click(object sender, EventArgs ex)
        {
            try
            {
                if (ListOfPointsRecorded.SelectedIndex >= 0)
                    _profile.Points.RemoveAt(ListOfPointsRecorded.SelectedIndex);
                RefreshForm();
            }
            catch (Exception e)
            {
                Logging.WriteError("Fisherbot > Bot > ProfileCreator > delB_Click(object sender, EventArgs ex): " + e);
            }
        }

        // BLACK LIST
        private void delBlackRadius_Click(object sender, EventArgs e)
        {
            try
            {
                if (BlacklistRadiusList.SelectedIndex >= 0)
                    _profile.BlackListRadius.RemoveAt(BlacklistRadiusList.SelectedIndex);
                RefreshForm();
            }
            catch (Exception ex)
            {
                Logging.WriteError(
                    "Fisherbot > Bot > ProfileCreator > delBlackRadius_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void addBlackB_Click(object sender, EventArgs e)
        {
            try
            {
                _profile.BlackListRadius.Add(new FisherbotBlackListRadius
                {Position = ObjectManager.Me.Position, Radius = (float) radiusN.Value});
                RefreshForm();
            }
            catch (Exception ex)
            {
                Logging.WriteError("Fisherbot > Bot > ProfileCreator > addBlackB_Click(object sender, EventArgs e): " +
                                   ex);
            }
        }
    }
}