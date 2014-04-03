using System;
using System.Collections.Generic;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Class;

namespace Archaeologist
{
    public partial class DigSitesListManagement : DevComponents.DotNetBar.Metro.MetroForm
    {
        public DigSitesListManagement()
        {
            try
            {
                InitializeComponent();
                Translate();

                List<Digsite> list = nManager.Wow.Helpers.Archaeology.GetAllDigsitesZone();
                listDigsitesDGV.DataSource = list;

                solvingEveryXMin.Value = Bot.ArchaeologistSetting.CurrentSetting.SolvingEveryXMin;
                maxTryByDigsite.Value = Bot.ArchaeologistSetting.CurrentSetting.MaxTryByDigsite;
                useKeystone.Checked = Bot.ArchaeologistSetting.CurrentSetting.UseKeystones;
                if (nManager.nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                    TopMost = true;
            }
            catch (Exception e)
            {
                Logging.WriteError("DigSites_List_Management > DigSites_List_Management(): " + e);
            }
        }

        private void Translate()
        {
            saveB.Text = nManager.Translate.Get(nManager.Translate.Id.Save);
            labelX1.Text = nManager.Translate.Get(nManager.Translate.Id.Solving_Every);
            labelX2.Text = nManager.Translate.Get(nManager.Translate.Id.min);
            labelX3.Text = nManager.Translate.Get(nManager.Translate.Id.Max_Try_By_Digsite);
            Text = nManager.Translate.Get(nManager.Translate.Id.DigSites_list_Management);
        }

        private void saveB_Click(object sender, EventArgs e)
        {
            try
            {
                Bot.ArchaeologistSetting.CurrentSetting.SolvingEveryXMin = solvingEveryXMin.Value;
                Bot.ArchaeologistSetting.CurrentSetting.MaxTryByDigsite = maxTryByDigsite.Value;
                Bot.ArchaeologistSetting.CurrentSetting.UseKeystones = useKeystone.Checked;
                Bot.ArchaeologistSetting.CurrentSetting.Save();
                XmlSerializer.Serialize(
                    Application.StartupPath + "\\Data\\ArchaeologistDigsites.xml",
                    listDigsitesDGV.DataSource);
            }
            catch (Exception ex)
            {
                Logging.WriteError("DigSites_List_Management > saveB_Click(object sender, EventArgs e): " + ex);
            }
        }
    }
}