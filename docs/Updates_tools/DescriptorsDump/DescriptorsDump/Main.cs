using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DescriptorsDump
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void DumpBClick(object sender, EventArgs e)
        {
            var result = Dump.Go(prefixEnumTB.Text, multiplyCB.Checked, checkBoxUpper.Checked, checkBoxRemLocal.Checked);
            dumpTB.Text = result;
        }
    }
}
