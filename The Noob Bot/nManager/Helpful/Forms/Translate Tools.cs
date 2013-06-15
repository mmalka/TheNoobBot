using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace nManager.Helpful.Forms
{
    public partial class TranslateTools : DevComponents.DotNetBar.Metro.MetroForm
    {
        private Translate.Language _translation = new Translate.Language();

        public TranslateTools()
        {
            try
            {
                InitializeComponent();
                Translate();
                if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                    this.TopMost = true;
                foreach (object id in Enum.GetValues(typeof (Translate.Id)))
                {
                    _translation.Translations.Add(new Translate.Translation {Id = (Translate.Id) id, Text = ""});
                }
                listDigsitesDGV.DataSource = _translation.Translations;
            }
            catch (Exception e)
            {
                Logging.WriteError("Translate_Tools > Translate_Tools(): " + e);
            }
        }

        private void Translate()
        {
            saveB.Text = nManager.Translate.Get(nManager.Translate.Id.Save);
            loadB.Text = nManager.Translate.Get(nManager.Translate.Id.Load);
            Text = nManager.Translate.Get(nManager.Translate.Id.Translate_Tools);
        }

        private void saveB_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = Others.DialogBoxSaveFile(Application.StartupPath + "\\Data\\Lang\\",
                                                        "Langage files (*.xml)|*.xml");
                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    _translation.Translations = (List<Translate.Translation>) listDigsitesDGV.DataSource;
                    XmlSerializer.Serialize(filePath, _translation);
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("Translate_Tools > saveB_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void loadB_Click(object sender, EventArgs e)
        {
            string filePath = Others.DialogBoxOpenFile(Application.StartupPath + "\\Data\\Lang\\",
                                                    "Langage files (*.xml)|*.xml");
            if (File.Exists(filePath))
            {
                Translate.Language t = XmlSerializer.Deserialize<Translate.Language>(filePath);
                for (int i = 0; i <= t.Translations.Count - 1; i++)
                {
                    if (!string.IsNullOrEmpty(t.Translations[i].Text))
                    {
                        for (int j = 0; j <= _translation.Translations.Count - 1; j++)
                        {
                            if (t.Translations[i].Id == _translation.Translations[j].Id)
                            {
                                _translation.Translations[j].Text = t.Translations[i].Text;
                                break;
                            }
                        }
                    }
                }
                listDigsitesDGV.DataSource = _translation.Translations;
            }
        }
    }
}