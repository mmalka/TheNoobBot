using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using nManager.Properties;

namespace nManager.Helpful.Forms.UserControls
{
    public class TnbComboBox : ComboBox
    {
        private const int WmPaint = 0x000F;
        private Color _borderColor = Color.FromArgb(121, 121, 121);
        private ButtonBorderStyle _borderStyle = ButtonBorderStyle.Solid;
        private Color _hightlightColor = Color.Gainsboro;
        private Color _selectorBorderColor = Color.FromArgb(83, 106, 194);
        private Image _selectorImage = Resources.selectorBack_big;

        public TnbComboBox()
        {
            base.DrawMode = DrawMode.OwnerDrawFixed;
            base.BackColor = Color.FromArgb(232, 232, 232);
            base.ForeColor = Color.FromArgb(98, 160, 229);
            ItemHeight = 20; // 26 px height
            DropDownStyle = ComboBoxStyle.DropDownList;
            FlatStyle = FlatStyle.Flat;
            DrawItem += AdvancedComboBox_DrawItem;
        }

        public new DrawMode DrawMode { get; set; }

        [Category("Appearance")]
        public Color HighlightColor
        {
            get { return _hightlightColor; }
            set
            {
                _hightlightColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color SelectorBorderColor
        {
            get { return _selectorBorderColor; }
            set
            {
                _selectorBorderColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public ButtonBorderStyle BorderStyle
        {
            get { return _borderStyle; }
            set
            {
                _borderStyle = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Image SelectorImage
        {
            get { return _selectorImage; }
            set
            {
                _selectorImage = value;
                Invalidate();
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WmPaint)
            {
                Graphics g = Graphics.FromHwnd(Handle);
                var rect = new Rectangle(0, 0, Width - 17, Height);
                ControlPaint.DrawBorder(g, rect, BorderColor, BorderStyle);
                rect = new Rectangle(Width - 18, 0, 18, Height);
                ControlPaint.DrawBorder(g, rect, SelectorBorderColor, BorderStyle);
                if (SelectorImage != null)
                {
                    rect = new Rectangle(Width - 17, 1, 16, Height - 2);
                    g.DrawImage(SelectorImage, rect);
                }
            }
        }

        private void AdvancedComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;
            var combo = sender as ComboBox;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                e.Graphics.FillRectangle(new SolidBrush(HighlightColor),
                    e.Bounds);
            else if (combo != null)
                e.Graphics.FillRectangle(new SolidBrush(combo.BackColor),
                    e.Bounds);

            if (combo != null)
                e.Graphics.DrawString(combo.Items[e.Index].ToString(), e.Font,
                    new SolidBrush(combo.ForeColor),
                    new Point(e.Bounds.X, e.Bounds.Y));
            e.DrawFocusRectangle();
        }
    }
}