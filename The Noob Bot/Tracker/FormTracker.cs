using System;
using System.Collections.Generic;
using System.Windows.Forms;
using nManager;
using nManager.Helpful;
using nManager.Products;
using nManager.Wow.Enums;
using nManager.Wow.ObjectManager;

namespace Tracker
{
    public partial class FormTracker : Form
    {
        public FormTracker()
        {
            InitializeComponent();
            Translate();
            ConfigForm();
            UpdateTrackInGame();
            if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                TopMost = true;
        }

        private void Translate()
        {
            TrackByNPCNameLabel.Text = nManager.Translate.Get(nManager.Translate.Id.By_npc_name) + ":";
            TrackByObjectTypeLabel.Text = nManager.Translate.Get(nManager.Translate.Id.Object_type) + ":";
            TrackByCreatureTypeLabel.Text = nManager.Translate.Get(nManager.Translate.Id.Creature_type) + ":";
            MainHeader.TitleText = nManager.Translate.Get(nManager.Translate.Id.Tracker);
            this.Text = Others.GetRandomString(Others.Random(4, 10));
        }

        private void ConfigForm()
        {
            foreach (string value in Enum.GetNames(typeof (TrackCreatureFlags)))
            {
                CreatureTrackableList.Items.Add(value);
            }

            foreach (string value in Enum.GetNames(typeof (TrackObjectFlags)))
            {
                ObjectTrackableList.Items.Add(value);
            }
        }

        private void unDelTrack_Click(object sender, EventArgs e)
        {
            if (TrackedByNameList.SelectedItems.Count > 0)
            {
                for (int i = TrackedByNameList.SelectedIndices.Count - 1; i >= 0; i--)
                {
                    TrackedByNameList.Items.RemoveAt(TrackedByNameList.SelectedIndices[i]);
                }
            }
        }

        private void unAddTrack_Click(object sender, EventArgs e)
        {
            if (unNoTrack.Text.Replace(" ", "").Length > 0)
            {
                if (!TrackedByNameList.Items.Contains(unNoTrack.Text))
                {
                    TrackedByNameList.Items.Add(unNoTrack.Text);
                    unNoTrack.Text = "";
                }
            }
        }

        private void otAddTrack_Click(object sender, EventArgs e)
        {
            if (ObjectTrackableList.SelectedItems.Count > 0)
            {
                for (int i = ObjectTrackableList.SelectedIndices.Count - 1; i >= 0; i--)
                {
                    if (!ObjectTrackedList.Items.Contains(ObjectTrackableList.Items[ObjectTrackableList.SelectedIndices[i]]))
                        ObjectTrackedList.Items.Add(ObjectTrackableList.Items[ObjectTrackableList.SelectedIndices[i]]);
                }
                UpdateTrackInGame();
            }
        }

        private void otDelTrack_Click(object sender, EventArgs e)
        {
            if (ObjectTrackedList.SelectedItems.Count > 0)
            {
                for (int i = ObjectTrackedList.SelectedIndices.Count - 1; i >= 0; i--)
                {
                    ObjectTrackedList.Items.RemoveAt(ObjectTrackedList.SelectedIndices[i]);
                }
                UpdateTrackInGame();
            }
        }

        private void ctAddTrack_Click(object sender, EventArgs e)
        {
            if (CreatureTrackableList.SelectedItems.Count > 0)
            {
                for (int i = CreatureTrackableList.SelectedIndices.Count - 1; i >= 0; i--)
                {
                    if (!CreatureTrackedList.Items.Contains(CreatureTrackableList.Items[CreatureTrackableList.SelectedIndices[i]]))
                        CreatureTrackedList.Items.Add(CreatureTrackableList.Items[CreatureTrackableList.SelectedIndices[i]]);
                }
                UpdateTrackInGame();
            }
        }

        private void ctDelTrack_Click(object sender, EventArgs e)
        {
            if (CreatureTrackedList.SelectedItems.Count > 0)
            {
                for (int i = CreatureTrackedList.SelectedIndices.Count - 1; i >= 0; i--)
                {
                    CreatureTrackedList.Items.RemoveAt(CreatureTrackedList.SelectedIndices[i]);
                }
                UpdateTrackInGame();
            }
        }

        private void UpdateTrackInGame()
        {
            var listUnitFlag = new List<string>();
            for (int i = 0; i <= CreatureTrackedList.Items.Count - 1; i++)
            {
                listUnitFlag.Add(CreatureTrackedList.Items[i].ToString());
            }
            nManager.Wow.Helpers.Tracker.TrackCreatureFlags(listUnitFlag);

            var listObjectFlag = new List<string>();
            for (int i = 0; i <= ObjectTrackedList.Items.Count - 1; i++)
            {
                listObjectFlag.Add(ObjectTrackedList.Items[i].ToString());
            }
            nManager.Wow.Helpers.Tracker.TrackObjectFlags(listObjectFlag);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (TrackedByNameList.Items.Count > 0)
                {
                    var listName = new List<string>();
                    for (int i = 0; i <= TrackedByNameList.Items.Count - 1; i++)
                    {
                        listName.Add(TrackedByNameList.Items[i].ToString());
                        Application.DoEvents();
                    }

                    var tList = new List<WoWUnit>();
                    tList.AddRange(ObjectManager.GetObjectWoWUnit());
                    for (int i = 0; i <= tList.Count - 1; i++)
                    {
                        tList[i].IsTracked = listName.Contains(tList[i].Name);
                        Application.DoEvents();
                    }
                }
            }
            catch
            {
            }
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormTrackerTimer.Enabled = false;
            Products.ProductStop();
        }

        private void FormTracker_Load(object sender, EventArgs e)
        {
            MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.TrackerPopUp));
        }
    }
}