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

namespace Battleground_Bot.Profile
{
    public partial class ProfileManager : XtraForm
    {
        private bool _loopRecordPoint;
        Radar _radarForm = new Radar();

        public ProfileManager()
        {
            InitializeComponent();
            Translate();
        }

        void Translate()
        {
            label1.Text = Translation.GetText(Translation.Text.List_of_Location) + ":";
            label2.Text = Translation.GetText(Translation.Text.Black_List_target_Guid) + ":";
            BAddToBlackList.Text = Translation.GetText(Translation.Text.Add_Target_to_Black_List);
            BRecordPoint.Text = Translation.GetText(Translation.Text.Record_Way);
            BDelListPoint.Text = Translation.GetText(Translation.Text.Del);
            BDelBlackList.Text = Translation.GetText(Translation.Text.Del);
            LFirstPointInfo.Text = Translation.GetText(Translation.Text.First_Location_of_Profile) + ": ";
            BSave.Text = Translation.GetText(Translation.Text.Save_Profile);
            BLoadProfil.Text = Translation.GetText(Translation.Text.Load_Profile);
            hotSpotCB.Properties.Caption = Translation.GetText(Translation.Text.Hot_Spot);
            label3.Text = Translation.GetText(Translation.Text.for_) + ":";
            BDtargetFaction.Text = Translation.GetText(Translation.Text.Del);
            label4.Text = Translation.GetText(Translation.Text.Target_Faction_at_attacked) + ":";
            ABtargetFaction.Text = Translation.GetText(Translation.Text.Add_Target_to_Target_Faction);
        }

        private void LoopRecordWay()
        {
            const float distanceSeparator = 5.0f;
            const float distanceZSeparator = 3.0f;
            int lastRotation = 0;
            _loopRecordPoint = true;

            ListOfPoint.Items.Add(ObjectManager.Me.Position.ToString());

            while (_loopRecordPoint)
            {
                string[] pointArray = Convert.ToString(ListOfPoint.Items[ListOfPoint.Items.Count - 1].ToString()).Replace(" ", "").Split(
                    Convert.ToChar(";"));
                var lastPoint = new Point(Convert.ToSingle(pointArray[0]), Convert.ToSingle(pointArray[1]),
                                          Convert.ToSingle(pointArray[2]));
                float disZTemp = lastPoint.DistanceZ(ObjectManager.Me.Position);

                if (((lastPoint.DistanceTo(ObjectManager.Me.Position) > distanceSeparator) &&
                     lastRotation != (int) Others.RadianToDegree(ObjectManager.Me.Rotation)) ||
                    disZTemp >= distanceZSeparator)
                {
                    ListOfPoint.Items.Add(ObjectManager.Me.Position.ToString());
                    lastRotation = (int) Others.RadianToDegree(ObjectManager.Me.Rotation);
                    ListOfPoint.SelectedIndex = ListOfPoint.Items.Count - 1;
                }
                Application.DoEvents();
                Thread.Sleep(50);
            }
        }

        private void StopRecordWay()
        {
            _loopRecordPoint = false;
        }

        private void BRecordPointClick(object sender, EventArgs e)
        {
            if (_loopRecordPoint)
            {
                StopRecordWay();
                BRecordPoint.BackColor = Color.LimeGreen;
                BSave.BackColor = Color.LimeGreen;
                BSave.Enabled = true;
                BLoadProfil.Enabled = true;
                hotSpotCB.Enabled = true;
                BRecordPoint.Text = Translation.GetText(Translation.Text.Record_Way);
            }
            else
            {
                if (hotSpotCB.Checked)
                {
                    ListOfPoint.Items.Add(ObjectManager.Me.Position.ToString());
                }
                else
                {
                    hotSpotCB.Enabled = false;
                    BRecordPoint.Text = Translation.GetText(Translation.Text.Stop_Record_Way);
                    BSave.Enabled = false;
                    BLoadProfil.Enabled = false;
                    BSave.BackColor = Color.Silver;
                    BRecordPoint.BackColor = Color.Red;
                    LoopRecordWay();
                }
            }
        }

        private void BDelListPointClick(object sender, EventArgs e)
        {
            ListOfPoint.Items.Remove(ListOfPoint.SelectedItem);
        }

        private void BAddToBlackListClick(object sender, EventArgs e)
        {
            if (ObjectManager.Me.Target > 0)
            {
                ListOfBlackList.Items.Add(ObjectManager.Target.Guid.ToString());
                ListOfBlackList.SelectedIndex = ListOfBlackList.Items.Count - 1;
            }
        }

        private void BDelBlackListClick(object sender, EventArgs e)
        {
            ListOfBlackList.Items.Remove(ListOfBlackList.SelectedItem);
        }

        private void UpdateFirstPointInfoTick(object sender, EventArgs e)
        {
            if (ListOfPoint.Items.Count > 0)
            {
                try
                {
                    try
                    {
                        // Add info for the radar
                        var listObjTemps = new List<Radar.ObjDraw>();
                        var listLineTemps = new List<Radar.LineDraw>();
                        var points = (from object objectListBox in ListOfPoint.Items
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
                                                 ? new Radar.ObjDraw {ColorDraw = Color.Yellow, Position = p}
                                                 : new Radar.ObjDraw {ColorDraw = Color.Green, Position = p});
                            first = false;
                            lastPos = p;
                        }
                        _radarForm.ListObjDraw = listObjTemps;
                        _radarForm.ListLineDraw = listLineTemps;
                    }
                    catch
                    {
                    }

                    // Gui info
                    string[] pointArray =
                        Convert.ToString(ListOfPoint.Items[0].ToString()).Replace(" ", "").Split(Convert.ToChar(";"));
                    var firstPoint = new Point(Convert.ToSingle(pointArray[0]), Convert.ToSingle(pointArray[1]),
                                               Convert.ToSingle(pointArray[2]));

                    var distanceFirstPoint = (int) firstPoint.DistanceTo(ObjectManager.Me.Position);

                    var disZTemp = (int) firstPoint.DistanceZ(ObjectManager.Me.Position);

                    LFirstPointInfo.Text = Translation.GetText(Translation.Text.Distance) + "=" +
                                           distanceFirstPoint + " " + Translation.GetText(Translation.Text.Distance) + " Z=" + disZTemp;
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

        private void BSaveClick(object sender, EventArgs e)
        {
            BSave.Text = Translation.GetText(Translation.Text.Wait) + "...";
            BSave.Enabled = false;
            try
            {
                string file =
                    Others.DialogBox_SaveFile(Application.StartupPath + "\\Products\\Battleground Bot\\Profiles\\",
                                              "Profile files (*.xml)|*.xml|All files (*.*)|*.*");

                if (file != "")
                {
                    var points = (from object objectListBox in ListOfPoint.Items
                                  select Convert.ToString(objectListBox.ToString()).Replace(" ", "").Split(Convert.ToChar(";"))
                                  into pointArray select new Point(Convert.ToSingle(pointArray[0]), Convert.ToSingle(pointArray[1]), Convert.ToSingle(pointArray[2]))).ToList();

                    var blackListGuid = (from object objectListBox in ListOfBlackList.Items select Convert.ToUInt64(objectListBox)).ToList();

                    var targetFaction = (from object objectListBox in targetFactionList.Items select Convert.ToUInt32(objectListBox)).ToList();


                    var subProfile = new SubProfile
                                         {
                                             Locations = points,
                                             BlackListGuid = blackListGuid,
                                             TargetFaction = targetFaction,
                                             Hotspots = hotSpotCB.Checked,
                                             PlayerFaction = PlayerFactionCB.Text
                                         };

                    var profile = new Profile {SubProfiles = new List<SubProfile> {subProfile}};

                    XmlSerializerHelper.Serialize(file, profile);
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(Translation.GetText(Translation.Text.Profile_error) + ":   {0}" + er);
                Log.AddLog(Translation.GetText(Translation.Text.Profile_error));
            }
            BSave.Text = Translation.GetText(Translation.Text.Save);
            BSave.Enabled = true;
        }

        private void BLoadProfilClick(object sender, EventArgs e)
        {
            string file = Others.DialogBox_OpenFile(
                Application.StartupPath + "\\Products\\Battleground Bot\\Profiles\\",
                "Profile files (*.xml)|*.xml|All files (*.*)|*.*");

            if (file != "")
            {
                var subProfile = new SubProfile();
                Profile profile;

                ListOfPoint.Items.Clear();
                ListOfBlackList.Items.Clear();
                targetFactionList.Items.Clear();

                try
                {
                    profile = XmlSerializerHelper.Deserialize<Profile>(file);
                    subProfile = profile.SubProfiles[0];
                }
                catch (Exception er)
                {
                    MessageBox.Show(Translation.GetText(Translation.Text.Profile_error) + ":   {0}" + er);
                    Log.AddLog(Translation.GetText(Translation.Text.Profile_error));
                }


                hotSpotCB.Checked = subProfile.Hotspots;

                PlayerFactionCB.Text = subProfile.PlayerFaction;

                foreach (Point pointTemp in subProfile.Locations)
                {
                    ListOfPoint.Items.Add(pointTemp.ToString());
                }

                foreach (ulong pointTemp in subProfile.BlackListGuid)
                {
                    ListOfBlackList.Items.Add(pointTemp.ToString());
                }

                foreach (uint pointTemp in subProfile.TargetFaction)
                {
                    targetFactionList.Items.Add(pointTemp.ToString());
                }
            }
        }

        private void ProfileManagerFormClosing(object sender, FormClosingEventArgs e)
        {
            StopRecordWay();
        }

        private void Profile_Manager_Shown(object sender, EventArgs e)
        {
        }

        private void CheckBox1CheckedChanged(object sender, EventArgs e)
        {
            BRecordPoint.Text = hotSpotCB.Checked ? Translation.GetText(Translation.Text.Add_this_HotSpot) : Translation.GetText(Translation.Text.Record_Way);
        }

        private void BDtargetFactionClick(object sender, EventArgs e)
        {
            targetFactionList.Items.Remove(targetFactionList.SelectedItem);
        }

        private void ABtargetFactionClick(object sender, EventArgs e)
        {
            if (ObjectManager.Me.Target > 0)
            {
                targetFactionList.Items.Add(
                    ObjectManager.Target.Faction.ToString());
                targetFactionList.SelectedIndex = targetFactionList.Items.Count - 1;
            }
        }

        private void RadarBClick(object sender, EventArgs e)
        {
            _radarForm = new Radar();
            _radarForm.Show();
        }
    }
}