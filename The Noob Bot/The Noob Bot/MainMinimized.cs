using System;
using nManager.Helpful;
using nManager.Wow.ObjectManager;

namespace The_Noob_Bot
{
    partial class MainMinimized : DevComponents.DotNetBar.Metro.MetroForm
    {
        public MainMinimized()
        {
            InitializeComponent();
            Translate();
        }

        private void Translate()
        {
            startB.Text = nManager.Translate.Get(nManager.Translate.Id.Start);
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            try
            {
                toolTip1.SetToolTip(pictureBox1, Main.MinimizesWindowToolTip);
                toolTip1.ShowAlways = true;
                toolTip1.ToolTipTitle = nManager.Translate.Get(nManager.Translate.Id.Information) + ":";
            }
            catch (Exception ex)
            {
                Logging.WriteError("Main >  pictureBox1_MouseHover(object sender, EventArgs e): " + ex);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            try
            {
                Hide();
            }
            catch (Exception ex)
            {
                Logging.WriteError("Main >  pictureBox2_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void updateInfo_Tick(object sender, EventArgs e)
        {
            try
            {
                startB.Enabled = Main.MinimizesWindowBoutonActive;
                startB.Image = Main.MinimizesWindowBoutonImage;
                startB.Text = Main.MinimizesWindowBoutonText;
                Text = ObjectManager.Me.Name + " - " + nManager.Products.Products.ProductName;
            }
            catch (Exception ex)
            {
                Logging.WriteError("Main > updateInfo_Tick(object sender, EventArgs e): " + ex);
            }
        }

        private void pictureBox2_MouseHover(object sender, EventArgs e)
        {
            try
            {
                toolTip1.SetToolTip(pictureBox2, " ");
                toolTip1.ShowAlways = true;
                toolTip1.ToolTipTitle = nManager.Translate.Get(nManager.Translate.Id.Maximize_window);
            }
            catch (Exception ex)
            {
                Logging.WriteError("Main >  pictureBox2_MouseHover(object sender, EventArgs e): " + ex);
            }
        }

        private void startB_Click(object sender, EventArgs e)
        {
            try
            {
                if (nManager.Products.Products.IsStarted)
                {
                    startB.Enabled = false;
                    nManager.Products.Products.ProductStop();
                    startB.Enabled = true;
                }
                else
                {
                    startB.Enabled = false;
                    nManager.Products.Products.ProductStart();
                    startB.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("Main >  startB_Click(object sender, EventArgs e): " + ex);
            }
        }

        private void MainMinimized_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            try
            {
                nManager.Pulsator.Dispose(true);
            }
            catch (Exception ex)
            {
                Logging.WriteError(
                    "Main >  MainMinimized_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e): " + ex);
            }
        }
    }
}