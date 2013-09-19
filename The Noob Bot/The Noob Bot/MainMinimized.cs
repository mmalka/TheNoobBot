using System;
using System.Threading;
using nManager.Helpful;
using nManager.Wow.ObjectManager;

namespace The_Noob_Bot
{
    partial class MainMinimized : DevComponents.DotNetBar.Metro.MetroForm
    {
        private Thread _productStartThread;
        private Thread _productStopThread;

        public MainMinimized()
        {
            InitializeComponent();
            Translate();
            if (nManager.nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                TopMost = true;
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

        private void ThreadStartProduct()
        {
            StartBEnabled = false;
            nManager.Products.Products.ProductStart();
            StartBEnabled = true;
        }

        private void ThreadStopProduct()
        {
            StartBEnabled = false;
            nManager.Products.Products.ProductStop();
            StartBEnabled = true;
        }

        private bool StartBEnabled
        {
            set { startB.Enabled = value; }
        }

        private void startB_Click(object sender, EventArgs e)
        {
            try
            {
                if (!nManager.Products.Products.IsStarted)
                {
                    if (_productStartThread == null)
                    {
                        _productStartThread = new Thread(ThreadStartProduct) {IsBackground = true, Name = "Thread Start Product"};
                    }
                    _productStartThread.Start();
                    _productStopThread = null;
                }
                else
                {
                    if (_productStopThread == null)
                    {
                        _productStopThread = new Thread(ThreadStopProduct) {IsBackground = true, Name = "Thread Stop Product"};
                    }
                    _productStopThread.Start();
                    _productStartThread = null;
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError("MainMinimized > startB_Click(object sender, EventArgs e): " + ex);
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