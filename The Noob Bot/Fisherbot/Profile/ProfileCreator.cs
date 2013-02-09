using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.ObjectManager;

namespace Fisherbot.Profile
{
    public partial class ProfileCreator : DevComponents.DotNetBar.Metro.MetroForm
    {
        private FisherbotProfile _profile = new FisherbotProfile();

        public ProfileCreator()
        {
            try
            {
                InitializeComponent();
                Translate();
            }
            catch (Exception e)
            {
                Logging.WriteError("Fisherbot > Bot > ProfileCreator > ProfileCreator(): " + e);
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
            Text = nManager.Translate.Get(nManager.Translate.Id.Profile_Creator);
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
                    refreshForm();
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("Fisherbot > Bot > ProfileCreator > loadB_Click(object sender, EventArgs e): " + ex);
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
                    "Fisherbot > Bot > ProfileCreator > ProfileCreator_FormClosing(object sender, FormClosingEventArgs ex): " +
                    e);
            }
        }

        private void refreshForm()
        {
            try
            {
                // Way
                listPoint.Items.Clear();
                foreach (var p in _profile.Points)
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
                foreach (var b in _profile.BlackListRadius)
                {
                    listBlackRadius.Items.Add(b.Position.X + " ; " + b.Position.Y + " - " + b.Radius);
                }
                listBlackRadius.SelectedIndex = listBlackRadius.Items.Count - 1;
            }
            catch
            {
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
                refreshForm();

                while (_loopRecordPoint)
                {
                    var lastPoint = _profile.Points[_profile.Points.Count - 1];
                    float disZTemp = lastPoint.DistanceZ(ObjectManager.Me.Position);

                    if ((lastPoint.DistanceTo(ObjectManager.Me.Position) > nSeparatorDistance.Value) ||
                        disZTemp >= distanceZSeparator)
                    {
                        _profile.Points.Add(ObjectManager.Me.Position);
                        refreshForm();
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
                if (listPoint.SelectedIndex >= 0)
                    _profile.Points.RemoveAt(listPoint.SelectedIndex);
                refreshForm();
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
                if (listBlackRadius.SelectedIndex >= 0)
                    _profile.BlackListRadius.RemoveAt(listBlackRadius.SelectedIndex);
                refreshForm();
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
                    {Position = ObjectManager.Me.Position, Radius = radiusN.Value});
                refreshForm();
            }
            catch (Exception ex)
            {
                Logging.WriteError("Fisherbot > Bot > ProfileCreator > addBlackB_Click(object sender, EventArgs e): " +
                                   ex);
            }
        }
    }
}