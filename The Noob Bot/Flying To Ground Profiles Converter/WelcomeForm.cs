using System;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;
using nManager;
using nManager.Helpful;
using nManager.Products;

namespace Flying_To_Ground_Profiles_Converter
{
    public partial class WelcomeForm : MetroForm
    {
        public WelcomeForm()
        {
            InitializeComponent();
            Translate();
            if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                TopMost = true;
        }

        private void Translate()
        {
            // TODO: Translate the labels.
        }

        private void form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Products.ProductStop();
        }


        private void convertB_Click(object sender, EventArgs e)
        {
            convertB.Enabled = false;
            convertB.Text = nManager.Translate.Get(nManager.Translate.Id.In_progress);
            string[] files =
                Others.DialogBoxOpenFileMultiselect(
                    Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                    "Profile files (*.xml;*.ini)|*.xml;*.ini|All files (*.*)|*.*");
            int error = 0;
            int success = 0;
            foreach (string file in files)
            {
                if (!Products.IsStarted)
                    return;

                if (Conversion.Gatherer.IsGathererProfile(file))
                {
                    if (Conversion.Gatherer.Convert(file))
                        success++;
                    else
                        error++;
                }
                else if (Conversion.Fisherbot.IsFisherbotProfile(file))
                {
                    if (Conversion.Fisherbot.Convert(file))
                        success++;
                    else
                        error++;
                }
                else if (Conversion.Grinder.IsGrinderProfile(file))
                {
                    if (Conversion.Grinder.Convert(file))
                        success++;
                    else
                        error++;
                }
                else
                {
                    error++;
                    Application.DoEvents();
                }
            }

            MessageBox.Show(string.Format("{0}, {1} {2}, {3} {4} ", nManager.Translate.Get(nManager.Translate.Id.Convertion_finish), success,
                                          nManager.Translate.Get(nManager.Translate.Id.success), error, nManager.Translate.Get(nManager.Translate.Id.errors)));
            convertB.Text = nManager.Translate.Get(nManager.Translate.Id.Convert_Profiles);
            convertB.Enabled = true;
        }
    }
}