using System;
using System.Windows.Forms;
using nManager.Helpful;

namespace Profiles_Converters
{
    public partial class formMain : DevComponents.DotNetBar.Metro.MetroForm
    {
        public formMain()
        {
            InitializeComponent();
            Translate();
            if (nManager.nManagerSetting.CurrentSetting.AlwaysOnTop)
                this.TopMost = true;
        }
        void Translate()
        {
            convertB.Text = nManager.Translate.Get(nManager.Translate.Id.Convert_Profiles);
            labelX1.Text = nManager.Translate.Get(nManager.Translate.Id.Convert_Profiles) + " (Pirox Fly gatherer, ";
            labelX4.Text = " grind) " + nManager.Translate.Get(nManager.Translate.Id.to) + " The Noob Bot profiles";
            Text = nManager.Translate.Get(nManager.Translate.Id.Profiles_Converters);
        }
        private void form_FormClosing(object sender, FormClosingEventArgs e)
        {
            nManager.Products.Products.ProductStop();
        }


        private void convertB_Click(object sender, EventArgs e)
        {
            convertB.Enabled = false;
            convertB.Text = nManager.Translate.Get(nManager.Translate.Id.In_progress);
            var files =
                Others.DialogBoxOpenFileMultiselect(
                    Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                    "Profile files (*.xml;*.ini)|*.xml;*.ini|All files (*.*)|*.*");
            int error = 0;
            int success = 0;
            foreach (var file in files)
            {
                if (!nManager.Products.Products.IsStarted)
                    return;


                if (Converters.HonorBuddyGrind.IsHonorBuddyGrindProfile(file))
                {
                    if (Converters.HonorBuddyGrind.Convert(file))
                        success++;
                    else
                        error++;
                }
                else if (Converters.GatherBuddy.IsGatherBuddyProfile(file))
                {
                    if (Converters.GatherBuddy.Convert(file))
                        success++;
                    else
                        error++;
                }
                else if (Converters.MMOLazyMyFlyer.IsMMoLazyFlyerProfile(file))
                {
                    if (Converters.MMOLazyMyFlyer.Convert(file))
                        success++;
                    else
                        error++;
                }
                else if (Converters.PiroxFlyGatherer.IsPiroxFlyGathererProfile(file))
                {
                    if (Converters.PiroxFlyGatherer.Convert(file))
                        success++;
                    else
                        error++;
                }
                else if (Converters.WowRobotGatherFly.IsWowRobotGatherFlyProfile(file))
                {
                    if (Converters.WowRobotGatherFly.Convert(file))
                        success++;
                    else
                        error++;
                }
                else
                {
                    error++;
                    Logging.Write("Profile: ''" + file + "'' no converted.");
                    Application.DoEvents();
                }
            }

            MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.Convertion_finish) + ", " + success + " " + nManager.Translate.Get(nManager.Translate.Id.success) + ", " + error + " " + nManager.Translate.Get(nManager.Translate.Id.errors) + " ");
            convertB.Text = nManager.Translate.Get(nManager.Translate.Id.Convert_Profiles);
            convertB.Enabled = true;
        }
    }
}