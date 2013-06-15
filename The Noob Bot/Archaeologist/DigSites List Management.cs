using System;
using System.Windows.Forms;
using nManager.Helpful;

namespace Archaeologist
{
    public partial class DigSites_List_Management : DevComponents.DotNetBar.Metro.MetroForm
    {
        public DigSites_List_Management()
        {
            try
            {
                InitializeComponent();
                Translate();

                var list = nManager.Wow.Helpers.Archaeology.GetAllDigsitesZone();
                listDigsitesDGV.DataSource = list;

                solvingEveryXMin.Value = Bot.ArchaeologistSetting.CurrentSetting.solvingEveryXMin;
                maxTryByDigsite.Value = Bot.ArchaeologistSetting.CurrentSetting.maxTryByDigsite;
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
                Bot.ArchaeologistSetting.CurrentSetting.solvingEveryXMin = solvingEveryXMin.Value;
                Bot.ArchaeologistSetting.CurrentSetting.maxTryByDigsite = maxTryByDigsite.Value;
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