using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using BinDiffTrimmer.Helpers;

namespace BinDiffTrimmer
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            Logging.OnWrite += Logging_OnWrite;
        }

        private void Logging_OnWrite(string message, Color col)
        {
            try
            {
                if (InvokeRequired)
                    Invoke(new Logging.WriteDelegate(Logging_OnWrite), message, col);

                else
                {
                    Color defaultColor = _rtbLog.SelectionColor;

                    _rtbLog.SelectionColor = col;
                    _rtbLog.AppendText(message);
                    _rtbLog.SelectionColor = defaultColor;
                    _rtbLog.AppendText(Environment.NewLine);
                    _rtbLog.ScrollToCaret();
                }
            }
            catch (ObjectDisposedException ex)
            {
                Logging.WriteException(ex);
            }
        }

        private void _btnUntrimmed_Click(object sender, EventArgs e)
        {
            _rtbLog.Clear();
            var ofd = new OpenFileDialog {Filter = "*.txt | *.txt", Title = "Select untrimmed patchdiff2 file;"};
            DialogResult dr = ofd.ShowDialog();
            //Logging.LogOnWrite = false;

            if (dr == DialogResult.OK)
            {
                var th = new Thread(Process) {IsBackground = true, Name = "Process Thread"};
                th.Start(ofd.FileName);
            }
        }

        private void _btnEnum_Click(object sender, EventArgs e)
        {
            _rtbLog.Clear();
            var ofd = new OpenFileDialog {Filter = "*.txt | *.txt", Title = "Select untrimmer patchdiff2 file;"};
            DialogResult dr = ofd.ShowDialog();
            //Logging.LogOnWrite = false;

            if (dr == DialogResult.OK)
            {
                var th = new Thread(Ess) {IsBackground = true, Name = "Process Thread"};
                th.Start(ofd.FileName);
            }
        }

        private static void Process(object file)
        {
            string path = Convert.ToString(file);

            var lines = File.ReadAllLines(path);

            try
            {
                foreach (string s in lines)
                {
                    var trimmed = s.Split(' ').ToList();

                    for (int i = trimmed.Count - 1; i >= 0; i--)
                    {
                        if (string.IsNullOrEmpty(trimmed[i]))
                            trimmed.RemoveAt(i);
                    }

                    if (trimmed[1] != "1.00" && trimmed[1] != "0.99" && trimmed[1] != "0.98")
                        continue;

                    if (!IsNumeric(trimmed[3]) || trimmed[4] == trimmed[6])
                        continue;

                    if (!trimmed[6].Contains("sub_") && !trimmed[6].Contains("__imp"))
                    {
                        if (!trimmed[6].Contains("Script_"))
                            Logging.Write(trimmed[3] + "\t" + trimmed[6]);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Write("Exceotion!:{0} - {1}", ex.Message,
                    ex.InnerException != null ? ex.InnerException.Message : "");
            }
        }

        private static void Ess(object file)
        {
            string path = Convert.ToString(file);

            var lines = File.ReadAllLines(path);

            Logging.Write("public enum GlobalOffsets");

            const string lolAttheCompiler = "{";
            Logging.Write(lolAttheCompiler);

            try
            {
                foreach (string s in lines)
                {
                    var trimmed = s.Split(' ').ToList();

                    for (int i = trimmed.Count - 1; i >= 0; i--)
                    {
                        if (string.IsNullOrEmpty(trimmed[i]))
                            trimmed.RemoveAt(i);
                    }

                    if (!IsNumeric(trimmed[3]))
                        continue;

                    if (!trimmed[2].Contains("sub") && !trimmed[2].Contains("lua"))
                    {
                        Logging.Write("    {0} = 0x{1},", trimmed[2], trimmed[3]);
                    }
                }

                const string lolatthecompilerAGAIN = "}";
                Logging.Write(lolatthecompilerAGAIN);
            }
            catch (Exception ex)
            {
                Logging.Write("Exceotion!:{0} - {1}", ex.Message,
                    ex.InnerException != null ? ex.InnerException.Message : "");
            }
        }

        private void _btnCopyToClipboard_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(_rtbLog.Text);
        }

        private static bool IsNumeric(string s)
        {
            try
            {
                uint result;
                uint.TryParse(s, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out result);
                return result != 0;
            }
            catch
            {
                return false;
            }
        }
    }
}