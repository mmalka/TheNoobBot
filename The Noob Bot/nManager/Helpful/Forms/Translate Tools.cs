using System;
using System.IO;
using System.Windows.Forms;

namespace nManager.Helpful.Forms
{
    public partial class TranslationManagementMainFrame : DevComponents.DotNetBar.Metro.MetroForm
    {
        private readonly Translate.Language _translation = new Translate.Language();
        private readonly Translate.Language _defaultLangage = XmlSerializer.Deserialize<Translate.Language>(Application.StartupPath + @"\Data\Lang\English.xml");

        public TranslationManagementMainFrame()
        {
            try
            {
                InitializeComponent();
                Translate();
                if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                    TopMost = true;
                TranslationTable.Columns.Add("Id", "Id");
                TranslationTable.Columns[0].ReadOnly = true;
                TranslationTable.Columns[0].Width = 124;
                TranslationTable.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                TranslationTable.Columns.Add("YourTranslation", "Your translated text");
                TranslationTable.Columns[1].Width = 300;
                TranslationTable.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                TranslationTable.Columns.Add("DefaultText", "Default English text");
                TranslationTable.Columns[2].ReadOnly = true;
                TranslationTable.Columns[2].Width = 300;
                TranslationTable.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                foreach (Translate.Translation translation in _defaultLangage.Translations)
                {
                    TranslationTable.Rows.Add(new object[] {translation.Id.ToString(), "", translation.Text});
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Translate_Tools > Translate_Tools(): " + e);
            }
        }

        private void Translate()
        {
            SaveAsButton.Text = nManager.Translate.Get(nManager.Translate.Id.Save);
            LoadAsButton.Text = nManager.Translate.Get(nManager.Translate.Id.Load);
            TitleText = nManager.Translate.Get(nManager.Translate.Id.Translate_Tools);
        }

        private void SaveAsButton_Click(object sender, EventArgs e)
        {
            SaveGrid();
        }

        private void LoadAsButton_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void SaveGrid()
        {
            try
            {
                string filePath = Others.DialogBoxSaveFile(Application.StartupPath + @"\Data\Lang\", "Langage files (*.xml)|*.xml");
                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    _translation.Translations.Clear();
                    for (int i = 0; i < TranslationTable.Rows.Count - 1; i++)
                    {
                        DataGridViewRow row = TranslationTable.Rows[i];
                        // Foreach is necessary since the user can sort the Grid, the indexes wont match.
                        foreach (Translate.Id currId in Enum.GetValues(typeof (Translate.Id)))
                        {
                            if (currId.ToString() != row.Cells[0].Value.ToString())
                                continue;
                            string textContent = row.Cells[1].Value == null || string.IsNullOrEmpty(row.Cells[1].Value.ToString()) ||
                                                 row.Cells[1].Value.ToString() == row.Cells[0].Value.ToString() || row.Cells[1].Value.ToString().Contains("_")
                                                     ? row.Cells[2].Value.ToString()
                                                     : row.Cells[1].Value.ToString();
                            _translation.Translations.Add(new Translate.Translation
                                {
                                    Id = currId,
                                    Text = textContent
                                });
                            break;
                        }
                    }
                    XmlSerializer.Serialize(filePath, _translation);
                    LoadGrid(filePath);
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("Translate_Tools > SaveGrid(): " + ex);
            }
        }

        private void LoadGrid(string filePath = "")
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                    filePath = Others.DialogBoxOpenFile(Application.StartupPath + @"\Data\Lang\", "Langage files (*.xml)|*.xml");
                if (File.Exists(filePath))
                {
                    Translate.Language currentLangage = XmlSerializer.Deserialize<Translate.Language>(filePath);
                    CheckIdListAndReOrganize(ref currentLangage);
                    for (int i = 0; i <= _defaultLangage.Translations.Count - 1; i++)
                    {
                        if (string.IsNullOrEmpty(currentLangage.Translations[i].Text) || currentLangage.Translations[i].Text == currentLangage.Translations[i].Id.ToString() ||
                            currentLangage.Translations[i].Text.Contains("_"))
                            currentLangage.Translations[i].Text = _defaultLangage.Translations[i].Text;
                    }
                    TranslationTable.Rows.Clear();
                    for (int i = 0; i < _defaultLangage.Translations.Count; i++)
                    {
                        Translate.Translation translation = _defaultLangage.Translations[i];
                        TranslationTable.Rows.Add(new object[] {translation.Id.ToString(), currentLangage.Translations[i].Text, translation.Text});
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("Translate_Tools > LoadGrid(string filePath): " + ex);
            }
        }

        private void CheckIdListAndReOrganize(ref Translate.Language lang)
        {
            for (int i = 0; i <= _defaultLangage.Translations.Count - 1; i++)
            {
                if (lang.Translations.Count - 1 > i)
                {
                    if (_defaultLangage.Translations[i].Id != lang.Translations[i].Id)
                    {
                        bool found = false;
                        int foundAt = 0;
                        string textFound = "";
                        for (int j = 0; j < lang.Translations.Count - 1; j++)
                        {
                            if (lang.Translations[j].Id == _defaultLangage.Translations[i].Id)
                            {
                                found = true;
                                foundAt = j;
                                textFound = lang.Translations[j].Text;
                                break;
                            }
                        }
                        if (found)
                        {
                            lang.Translations.RemoveAt(foundAt);
                            if (string.IsNullOrEmpty(textFound))
                                textFound = _defaultLangage.Translations[i].Text;
                            lang.Translations.Insert(i, new Translate.Translation {Id = _defaultLangage.Translations[i].Id, Text = textFound});
                        }
                        else
                        {
                            lang.Translations.Insert(i, _defaultLangage.Translations[i]);
                        }
                    }
                }
                else if (lang.Translations.Count - 1 < i && i != _defaultLangage.Translations.Count)
                {
                    lang.Translations.Insert(i, _defaultLangage.Translations[i]);
                }
            }
            while (_defaultLangage.Translations.Count < lang.Translations.Count)
            {
                lang.Translations.RemoveAt(lang.Translations.Count - 1);
            }
        }
    }
}