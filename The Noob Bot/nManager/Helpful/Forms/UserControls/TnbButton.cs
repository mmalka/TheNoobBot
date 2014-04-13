using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using nManager.Properties;

namespace nManager.Helpful.Forms.UserControls
{
    public sealed class TnbButton : Label
    {
        private Image _bImage;
        private Image _hooverImage = Resources.greenB;

        public TnbButton()
        {
            base.AutoSize = false;
            TextAlign = ContentAlignment.MiddleCenter;
            Image = Resources.blackB;
            AutoEllipsis = true;
            ForeColor = Color.Snow;
            Font = new Font(Font, FontStyle.Bold);
            MouseEnter += OnMouseEnter;
            MouseLeave += OnMouseLeave;
        }

        [Category("Appearance")]
        public Image HooverImage
        {
            get { return _hooverImage; }
            set
            {
                _hooverImage = value;
                Invalidate(); // causes control to be redrawn
            }
        }

        public bool Hoovering = false;

        public override bool AutoSize
        {
            get { return false; }
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            Image = _bImage;
            Hoovering = false;
        }

        private void OnMouseEnter(object sender, EventArgs e)
        {
            Hoovering = true;
            _bImage = Image;
            Image = HooverImage;
        }
    }
}