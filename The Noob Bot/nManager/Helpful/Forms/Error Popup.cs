using System;
using System.Windows.Forms;

namespace nManager.Helpful.Forms
{
    public partial class Error_Popup : DevComponents.DotNetBar.Metro.MetroForm
    {
        public Error_Popup(string ErrorMessage)
        {
            InitializeComponent();
            ErrorDescription.Text = ErrorMessage;
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}