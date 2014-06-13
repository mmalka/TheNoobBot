using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Archaeologist.Bot;
using Archaeologist.Properties;
using nManager;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using Point = System.Drawing.Point;

namespace Archaeologist
{
    public partial class ArchaeologistSettingsFrame : Form
    {
        private string _activeDigsiteColumn = "Active";
        private string _digsiteNameColumn = "Digsite's Name";
        private bool _flagClick;
        private int _positionInitialeX;
        private int _positionInitialeY;
        private string _priorityDigsiteColumn = "Priority";

        public ArchaeologistSettingsFrame()
        {
            try
            {
                InitializeComponent();
                Translate();
                SolvingEveryXMin.Value = ArchaeologistSetting.CurrentSetting.SolvingEveryXMin;
                MaxTryByDigsite.Value = ArchaeologistSetting.CurrentSetting.MaxTryByDigsite;
                UseKeystones.Checked = ArchaeologistSetting.CurrentSetting.UseKeystones;
                if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                    TopMost = true;
                DigSitesTable.Columns.Add("Id", "Id");
                DigSitesTable.Columns[0].ReadOnly = true;
                DigSitesTable.Columns[0].Width = 33;
                DigSitesTable.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DigSitesTable.Columns.Add("DigSiteName", _digsiteNameColumn);
                DigSitesTable.Columns[1].ReadOnly = true;
                DigSitesTable.Columns[1].Width = 559;
                DigSitesTable.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DigSitesTable.Columns.Add("DigSitePriority", _priorityDigsiteColumn);
                DigSitesTable.Columns[2].Width = 40;
                DigSitesTable.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                var columnActive = new DataGridViewCheckBoxColumn();
                DigSitesTable.Columns.Add(columnActive);
                DigSitesTable.Columns[3].Name = "DigSitePriority";
                DigSitesTable.Columns[3].HeaderText = _activeDigsiteColumn;
                DigSitesTable.Columns[3].Width = 40;
                DigSitesTable.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                foreach (Digsite digsite in Archaeology.GetAllDigsitesZone())
                {
                    DigSitesTable.Rows.Add(new object[] {digsite.id, digsite.name, digsite.PriorityDigsites, digsite.Active});
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("ArchaeologistSettingsFrame > ArchaeologistSettingsFrame(): " + e);
            }
        }

        private void Translate()
        {
            UseKeystones.Text = nManager.Translate.Get(nManager.Translate.Id.UseKeystones);
            _digsiteNameColumn = nManager.Translate.Get(nManager.Translate.Id.DigsiteName);
            _priorityDigsiteColumn = nManager.Translate.Get(nManager.Translate.Id.Priority);
            _activeDigsiteColumn = nManager.Translate.Get(nManager.Translate.Id.Active);
            SolvingEveryXMinLabel.Text = nManager.Translate.Get(nManager.Translate.Id.SolvingEveryXMinutes);
            MaxTryByDigsiteLabel.Text = nManager.Translate.Get(nManager.Translate.Id.MaxTryByDigsite);
            DeveloperToolsFormTitle.Text = nManager.Translate.Get(nManager.Translate.Id.ArchaeologistSettingsFrameTitle) + @" - " + Information.MainTitle;
            CancelAndCloseButton.Text = nManager.Translate.Get(nManager.Translate.Id.CancelAndClose);
            SaveAndCloseButton.Text = nManager.Translate.Get(nManager.Translate.Id.SaveAndClose);
        }

        private void MainFormMouseDown(object sender, MouseEventArgs e)
        {
            _flagClick = true;
            _positionInitialeX = e.X;
            _positionInitialeY = e.Y;
        }

        private void MainFormMouseUp(object sender, MouseEventArgs e)
        {
            _flagClick = false;
        }


        private void MainFormMouseMove(object sender, MouseEventArgs e)
        {
            if (_flagClick)
            {
                Location = new Point(Left + (e.X - _positionInitialeX), Top + (e.Y - _positionInitialeY));
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ReduceButton_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void SaveAndCloseButton_MouseEnter(object sender, EventArgs e)
        {
            SaveAndCloseButton.Image = Resources.greenB;
        }

        private void SaveAndCloseButton_MouseLeave(object sender, EventArgs e)
        {
            SaveAndCloseButton.Image = Resources.blueB;
        }

        private void ReduceButton_MouseEnter(object sender, EventArgs e)
        {
            ReduceButton.Image = Resources.reduce_buttonG;
        }

        private void ReduceButton_MouseLeave(object sender, EventArgs e)
        {
            ReduceButton.Image = Resources.reduce_button;
        }

        private void CloseButton_MouseEnter(object sender, EventArgs e)
        {
            CloseButton.Image = Resources.close_buttonG;
        }

        private void CloseButton_MouseLeave(object sender, EventArgs e)
        {
            CloseButton.Image = Resources.close_button;
        }

        private void SaveAndCloseButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (DigSitesTable.CurrentRow != null)
                    DigSitesTable.CurrentRow.DataGridView.EndEdit();
                SaveAndCloseButton.Enabled = false;
                ArchaeologistSetting.CurrentSetting.SolvingEveryXMin = (int) SolvingEveryXMin.Value;
                ArchaeologistSetting.CurrentSetting.MaxTryByDigsite = (int) MaxTryByDigsite.Value;
                ArchaeologistSetting.CurrentSetting.UseKeystones = UseKeystones.Checked;
                ArchaeologistSetting.CurrentSetting.Save();
                var digsite = new List<Digsite>();
                for (int i = 0; i < DigSitesTable.Rows.Count - 1; i++)
                {
                    DataGridViewRow row = DigSitesTable.Rows[i];
                    digsite.Add(new Digsite {id = (int) row.Cells[0].Value, name = (string) row.Cells[1].Value, PriorityDigsites = Convert.ToSingle(row.Cells[2].Value), Active = (bool) row.Cells[3].Value});
                }
                if (XmlSerializer.Serialize(Application.StartupPath + "\\Data\\ArchaeologistDigsites.xml", digsite))
                    Archaeology.ForceReloadDigsites = true;
            }
            catch (Exception ex)
            {
                Logging.WriteError("ArchaeologistSettingsFrame > SaveAndCloseButton_Click(object sender, EventArgs e): " + ex);
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
            if (DigSitesTable.Rows.Count < e.RowIndex && DigSitesTable.Rows[e.RowIndex].Cells.Count < e.ColumnIndex)
                DigSitesTable.Rows[e.RowIndex].Cells[e.ColumnIndex].DataGridView.BeginEdit(true);
        }
    }
}