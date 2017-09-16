using System;
using System.Windows.Forms;
using nManager;
using nManager.Helpful;
using nManager.Products;

namespace Flying_To_Ground_Profiles_Converter
{
    public partial class WelcomeForm : Form
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
            MainHeader.TitleText = nManager.Translate.Get(nManager.Translate.Id.FtGConverterHeaderText) + " - " + Information.MainTitle;
            this.Text = MainHeader.TitleText;
            FtGConverterLine1.Text = nManager.Translate.Get(nManager.Translate.Id.FtGConverterLine1);
            FtGConverterLine2.Text = nManager.Translate.Get(nManager.Translate.Id.FtGConverterLine2);
            FtGConverterLine3.Text = nManager.Translate.Get(nManager.Translate.Id.FtGConverterLine3);
            FtGConverterButton.Text = nManager.Translate.Get(nManager.Translate.Id.FtGConverterButton);
        }

        private void form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Products.ProductStop();
        }


        private void convertB_Click(object sender, EventArgs e)
        {
            FtGConverterButton.Enabled = false;
            FtGConverterButton.Text = nManager.Translate.Get(nManager.Translate.Id.In_progress);
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
                }
                Application.DoEvents();
            }

            MessageBox.Show(string.Format("{0}, {1} {2}, {3} {4} ", nManager.Translate.Get(nManager.Translate.Id.Convertion_finish), success,
                nManager.Translate.Get(nManager.Translate.Id.success), error, nManager.Translate.Get(nManager.Translate.Id.errors)));
            FtGConverterButton.Text = nManager.Translate.Get(nManager.Translate.Id.Convert_Profiles);
            FtGConverterButton.Enabled = true;
        }
    }
}