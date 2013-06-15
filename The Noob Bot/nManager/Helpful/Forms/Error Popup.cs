using System;

namespace nManager.Helpful.Forms
{
    public partial class ErrorPopup : DevComponents.DotNetBar.Metro.MetroForm
    {
        public ErrorPopup(string errorMessage)
        {
            InitializeComponent();
            ErrorDescription.Text = errorMessage;
            if (nManagerSetting.CurrentSetting.ActivateAlwaysOnTopFeature)
                TopMost = true;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}