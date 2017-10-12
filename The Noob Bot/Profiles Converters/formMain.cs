using System;
using System.Windows.Forms;
using nManager;
using nManager.Helpful;
using nManager.Products;
using Profiles_Converters.Converters;

namespace Profiles_Converters
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Translate();
            if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                TopMost = true;
        }

        private void Translate()
        {
            convertB.Text = nManager.Translate.Get(nManager.Translate.Id.Convert_Profiles);
            MainContent.Text = string.Format("{0}(Pirox Fly gatherer, MMOLazy MyFlyer, Gather Buddy,  WowRobot Gather Fly, HonorBuddy  grind) {1} TheNoobBot profiles.",
                nManager.Translate.Get(nManager.Translate.Id.Convert_Profiles), nManager.Translate.Get(nManager.Translate.Id.to));
            MainHeader.TitleText = nManager.Translate.Get(nManager.Translate.Id.Profiles_Converters);
            this.Text = Others.GetRandomString(Others.Random(4, 10));
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


                if (HonorBuddyGrind.IsHonorBuddyGrindProfile(file))
                {
                    if (HonorBuddyGrind.Convert(file))
                        success++;
                    else
                        error++;
                }
                else if (GatherBuddy.IsGatherBuddyProfile(file))
                {
                    if (GatherBuddy.Convert(file))
                        success++;
                    else
                        error++;
                }
                else if (MMOLazyMyFlyer.IsMMoLazyFlyerProfile(file))
                {
                    if (MMOLazyMyFlyer.Convert(file))
                        success++;
                    else
                        error++;
                }
                else if (PiroxFlyGatherer.IsPiroxFlyGathererProfile(file))
                {
                    if (PiroxFlyGatherer.Convert(file))
                        success++;
                    else
                        error++;
                }
                else if (WowRobotGatherFly.IsWowRobotGatherFlyProfile(file))
                {
                    if (WowRobotGatherFly.Convert(file))
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

            MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.Convertion_finish) + ", " + success + " " +
                            nManager.Translate.Get(nManager.Translate.Id.success) + ", " + error + " " +
                            nManager.Translate.Get(nManager.Translate.Id.errors) + " ");
            convertB.Text = nManager.Translate.Get(nManager.Translate.Id.Convert_Profiles);
            convertB.Enabled = true;
        }
    }
}