using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;

namespace nManager.Helpful.Forms.UserControls
{
    public sealed partial class UCXmlRichTextBox : RichTextBox
    {
        #region Constructor

        public UCXmlRichTextBox()
        {
            InitializeComponent();

            Font = new Font("Consolas", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);

            miCopyRtf.Click += miCopyRtf_Click;
            miCopyText.Click += miCopyText_Click;
            miSelectAll.Click += miSelectAll_Click;
        }

        #endregion Constructor

        #region Properties

        private string _xml = "";

        public string Xml
        {
            get { return _xml; }
            set
            {
                Text = "";
                _xml = value;
                SetXml(_xml);
            }
        }

        #endregion Properties

        #region Private Methods

        private void SetXml(string s)
        {
            if (String.IsNullOrEmpty(s)) return;

            XDocument xdoc = XDocument.Parse(s);

            string formattedText = xdoc.ToString().Trim();

            if (String.IsNullOrEmpty(formattedText)) return;

            var machine = new XmlStateMachine();

            if (s.StartsWith("<?"))
            {
                string xmlDeclaration = machine.GetXmlDeclaration(s);
                if (xmlDeclaration != String.Empty) formattedText = xmlDeclaration + Environment.NewLine + formattedText;
            }

            int location = 0;
            int failCount = 0;
            int tokenTryCount = 0;
            while (location < formattedText.Length)
            {
                XmlTokenType ttype;
                string token = machine.GetNextToken(formattedText, location, out ttype);
                Color color = machine.GetTokenColor(ttype);
                AppendText(this, token, color);

                location += token.Length;
                tokenTryCount++;

                // Check for ongoing failure
                if (token.Length == 0) failCount++;
                if (failCount > 10 || tokenTryCount > formattedText.Length)
                {
                    string theRestOfIt = formattedText.Substring(location, formattedText.Length - location);
                    //this.AppendText(Environment.NewLine + Environment.NewLine + theRestOfIt); // DEBUG
                    AppendText(theRestOfIt);
                    break;
                }
            }
        }

        #endregion Private Methods

        public static void AppendText(RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }

        #region Context Menu

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(SelectedText))
            {
                miCopyText.Enabled = false;
                miCopyRtf.Enabled = false;
            }
            else
            {
                miCopyText.Enabled = true;
                miCopyRtf.Enabled = true;
            }
        }

        private void miCopyText_Click(object sender, EventArgs e)
        {
            string s = SelectedText;
            try
            {
                XDocument doc = XDocument.Parse(s);
                s = doc.ToString();
            }
            catch
            {
            }
            Clipboard.SetText(s);
        }

        private void miCopyRtf_Click(object sender, EventArgs e)
        {
            var dto = new DataObject();
            dto.SetText(SelectedRtf, TextDataFormat.Rtf);
            dto.SetText(SelectedText, TextDataFormat.UnicodeText);
            Clipboard.Clear();
            Clipboard.SetDataObject(dto);
        }

        private void miSelectAll_Click(object sender, EventArgs e)
        {
            SelectAll();
        }

        #endregion Context Menu
    }

    #region Extension Methods

    internal static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }

    #endregion Extension Methods
}