using System;
using System.Collections.Generic;
using System.Windows.Forms;
using nManager.Wow.Class;
using nManager.Wow.Helpers;

namespace nManager.Helpful.Forms
{
    public partial class NodesListManager : DevComponents.DotNetBar.Metro.MetroForm
    {
        public NodesListManager()
        {
            try
            {
                InitializeComponent();
                Translate();

                var list = NodesList.LoadList();
                listDigsitesDGV.DataSource = list;
            }
            catch (Exception e)
            {
                Logging.WriteError("DigSites_List_Management > DigSites_List_Management(): " + e);

            }
        }
        void Translate()
        {
            saveB.Text = nManager.Translate.Get(nManager.Translate.Id.Save);
            Text = nManager.Translate.Get(nManager.Translate.Id.Nodes_List_Manager);
        }

        private void saveB_Click(object sender, EventArgs e)
        {
            try
            {
                NodesList.SaveList((List<Node>)listDigsitesDGV.DataSource);
            }
            catch (Exception ex)
            {
                Logging.WriteError("DigSites_List_Management > saveB_Click(object sender, EventArgs e): " + ex);

            }
        }


    }
}