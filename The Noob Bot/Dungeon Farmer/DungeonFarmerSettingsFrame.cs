using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DungeonFarmer.Bot;
using DungeonFarmer.Properties;
using nManager;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Helpers;

namespace DungeonFarmer
{
    public partial class DungeonFarmerSettingsFrame : Form
    {
        private readonly List<Instance> _instanceList;

        public DungeonFarmerSettingsFrame()
        {
            try
            {
                InitializeComponent();
                Translate();
                if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                    TopMost = true;
                InstanceListTable.Columns.Add("Id", "Id");
                InstanceListTable.Columns[0].ReadOnly = true;
                InstanceListTable.Columns[0].Width = 33;
                InstanceListTable.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                InstanceListTable.Columns.Add("InstanceName", "Instance Name");
                InstanceListTable.Columns[1].ReadOnly = true;
                InstanceListTable.Columns[1].Width = 250;
                InstanceListTable.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                InstanceListTable.Columns.Add("Continent", "Continent");
                InstanceListTable.Columns[2].ReadOnly = true;
                InstanceListTable.Columns[2].Width = 100;
                InstanceListTable.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                InstanceListTable.Columns.Add("InstancePriority", "Priority");
                InstanceListTable.Columns[3].Width = 40;
                InstanceListTable.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                var columnActive = new DataGridViewCheckBoxColumn();
                InstanceListTable.Columns.Add(columnActive);
                InstanceListTable.Columns[4].Name = "InstanceActive";
                InstanceListTable.Columns[4].HeaderText = @"Active";
                InstanceListTable.Columns[4].Width = 40;
                InstanceListTable.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                var columnKillBosses = new DataGridViewCheckBoxColumn();
                InstanceListTable.Columns.Add(columnKillBosses);
                InstanceListTable.Columns[5].Name = "KillBosses";
                InstanceListTable.Columns[5].HeaderText = @"Kill Bosses";
                InstanceListTable.Columns[5].Width = 80;
                InstanceListTable.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                var columnReset = new DataGridViewCheckBoxColumn();
                InstanceListTable.Columns.Add(columnReset);
                InstanceListTable.Columns[6].Name = "Reset";
                InstanceListTable.Columns[6].HeaderText = @"Reset";
                InstanceListTable.Columns[6].Width = 40;
                InstanceListTable.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                InstanceListTable.Columns.Add("LastUnitId", @"Last Unit Id");
                InstanceListTable.Columns[7].Width = 100;
                InstanceListTable.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                if (_instanceList == null)
                    _instanceList = XmlSerializer.Deserialize<List<Instance>>(Application.StartupPath + "\\Data\\DfInstanceList.xml");
                foreach (Instance instance in _instanceList)
                {
                    InstanceListTable.Rows.Add(new object[]
                    {
                        instance.InstanceId, instance.InstanceName, Usefuls.ContinentNameByContinentId(instance.EntranceContinentId), instance.InstancePriority, instance.Active, instance.KillBosses, instance.Reset,
                        instance.LastUnitId
                    });
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("DungeonFarmerSettingsFrame > DungeonFarmerSettingsFrame(): " + e);
            }
        }

        private void Translate()
        {
            CancelAndCloseButton.Text = nManager.Translate.Get(nManager.Translate.Id.CancelAndClose);
            SaveAndCloseButton.Text = nManager.Translate.Get(nManager.Translate.Id.SaveAndClose);
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SaveAndCloseButton_MouseEnter(object sender, EventArgs e)
        {
            SaveAndCloseButton.Image = Resources.greenB;
        }

        private void SaveAndCloseButton_MouseLeave(object sender, EventArgs e)
        {
            SaveAndCloseButton.Image = Resources.blueB;
        }

        private int GetInstanceIndexById(int instanceId)
        {
            for (int i = 0; i < _instanceList.Count; i++)
            {
                if (instanceId == _instanceList[i].InstanceId)
                {
                    return i;
                }
            }
            return -1;
        }

        private void SaveAndCloseButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (InstanceListTable.CurrentRow != null)
                    InstanceListTable.CurrentRow.DataGridView.EndEdit();
                SaveAndCloseButton.Enabled = false;
                DungeonFarmerSetting.CurrentSetting.Save();
                List<Instance> instanceList = _instanceList;
                for (int i = 0; i < InstanceListTable.Rows.Count - 1; i++)
                {
                    DataGridViewRow row = InstanceListTable.Rows[i];
                    int instanceIndex = GetInstanceIndexById((int) row.Cells[0].Value);
                    if (instanceIndex < 0)
                        continue;
                    instanceList[instanceIndex].InstancePriority = Convert.ToSingle(row.Cells[3].Value);
                    instanceList[instanceIndex].Active = (bool) row.Cells[4].Value;
                    instanceList[instanceIndex].KillBosses = (bool) row.Cells[5].Value;
                    instanceList[instanceIndex].Reset = (bool) row.Cells[6].Value;
                    instanceList[instanceIndex].LastUnitId = (uint) row.Cells[7].Value;
                }
                XmlSerializer.Serialize(Application.StartupPath + "\\Data\\DfInstanceList.xml", instanceList);
            }
            catch (Exception ex)
            {
                Logging.WriteError("DungeonFarmerSettingsFrame > SaveAndCloseButton_Click(object sender, EventArgs e): " + ex);
            }
            SaveAndCloseButton.Enabled = true;
            Close();
        }

        private void CancelAndCloseButton_MouseEnter(object sender, EventArgs e)
        {
            CancelAndCloseButton.Image = Resources.greenB;
        }

        private void CancelAndCloseButton_MouseLeave(object sender, EventArgs e)
        {
            CancelAndCloseButton.Image = Resources.blackB;
        }

        private void DigSitesTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (InstanceListTable.Rows.Count < e.RowIndex && InstanceListTable.Rows[e.RowIndex].Cells.Count < e.ColumnIndex)
                InstanceListTable.Rows[e.RowIndex].Cells[e.ColumnIndex].DataGridView.BeginEdit(true);
        }
    }
}