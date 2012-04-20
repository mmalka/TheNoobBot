using System;
using System.Collections.Generic;
using System.Windows.Forms;
using nManager.Wow.Enums;
using nManager.Wow.ObjectManager;

namespace Tracker
{
    public partial class FormTracker : DevComponents.DotNetBar.Metro.MetroForm
    {
        public FormTracker()
        {
            InitializeComponent();
            Translate();
            ConfigForm();
            UpdateTrackInGame();
        }
        void Translate()
        {
            labelX1.Text = nManager.Translate.Get(nManager.Translate.Id.By_npc_name) + ":";
            labelX2.Text = nManager.Translate.Get(nManager.Translate.Id.Object_type) + ":";
            labelX3.Text = nManager.Translate.Get(nManager.Translate.Id.Creature_type) + ":";
            Text = nManager.Translate.Get(nManager.Translate.Id.Tracker);
        }
        void ConfigForm()
        {
            foreach (string value in Enum.GetNames(typeof(TrackCreatureFlags)))
            {
                ctNoTrack.Items.Add(value);
            }

            foreach (string value in Enum.GetNames(typeof(TrackObjectFlags)))
            {
                otNoTrack.Items.Add(value);
            }
        }

        private void unDelTrack_Click(object sender, EventArgs e)
        {
            if (unTrack.SelectedItems.Count > 0)
            {
                for (int i = unTrack.SelectedIndices.Count - 1; i >= 0; i--)
                {
                    unTrack.Items.RemoveAt(unTrack.SelectedIndices[i]);
                }
            }
        }

        private void unAddTrack_Click(object sender, EventArgs e)
        {
            if (unNoTrack.Text.Replace(" ", "").Length > 0)
            {
                if (!unTrack.Items.Contains(unNoTrack.Text))
                {
                    unTrack.Items.Add(unNoTrack.Text);
                    unNoTrack.Text = "";
                }
            }
        }

        private void otAddTrack_Click(object sender, EventArgs e)
        {
            if (otNoTrack.SelectedItems.Count > 0)
            {
                for (int i = otNoTrack.SelectedIndices.Count - 1; i >= 0; i--)
                {
                    if (!otTrack.Items.Contains(otNoTrack.Items[otNoTrack.SelectedIndices[i]]))
                        otTrack.Items.Add(otNoTrack.Items[otNoTrack.SelectedIndices[i]]);
                }
                UpdateTrackInGame();
            }
        }

        private void otDelTrack_Click(object sender, EventArgs e)
        {
            if (otTrack.SelectedItems.Count > 0)
            {
                for (int i = otTrack.SelectedIndices.Count - 1; i >= 0; i--)
                {
                    otTrack.Items.RemoveAt(otTrack.SelectedIndices[i]);
                }
                UpdateTrackInGame();
            }
        }

        private void ctAddTrack_Click(object sender, EventArgs e)
        {
            if (ctNoTrack.SelectedItems.Count > 0)
            {
                for (int i = ctNoTrack.SelectedIndices.Count - 1; i >= 0; i--)
                {
                    if (!ctTrack.Items.Contains(ctNoTrack.Items[ctNoTrack.SelectedIndices[i]]))
                        ctTrack.Items.Add(ctNoTrack.Items[ctNoTrack.SelectedIndices[i]]);
                }
                UpdateTrackInGame();
            }
        }

        private void ctDelTrack_Click(object sender, EventArgs e)
        {
            if (ctTrack.SelectedItems.Count > 0)
            {
                for (int i = ctTrack.SelectedIndices.Count - 1; i >= 0; i--)
                {
                    ctTrack.Items.RemoveAt(ctTrack.SelectedIndices[i]);
                }
                UpdateTrackInGame();
            }
        }

        private void UpdateTrackInGame()
        {
            var listUnitFlag = new List<string>();
            for (int i = 0; i <= ctTrack.Items.Count - 1; i++)
            {
                listUnitFlag.Add(ctTrack.Items[i].ToString());
            }
            nManager.Wow.Helpers.Tracker.TrackCreatureFlags(listUnitFlag);

            var listObjectFlag = new List<string>();
            for (int i = 0; i <= otTrack.Items.Count - 1; i++)
            {
                listObjectFlag.Add(otTrack.Items[i].ToString());
            }
            nManager.Wow.Helpers.Tracker.TrackObjectFlags(listObjectFlag);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (unTrack.Items.Count > 0)
                {
                    var listName = new List<string>();
                    for (int i = 0; i <= unTrack.Items.Count - 1; i++)
                    {
                        listName.Add(unTrack.Items[i].ToString());
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
            timer1.Enabled = false;
            nManager.Products.Products.ProductStop();
        }
    }
}