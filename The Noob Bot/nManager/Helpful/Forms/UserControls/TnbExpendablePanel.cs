using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using nManager.Properties;

namespace nManager.Helpful.Forms.UserControls
{
    public sealed class TnbExpendablePanel : Panel
    {
        private const int WmPaint = 0x000F;
        private readonly Label _header = new Label();
        public EventHandler OnOrderChanged;
        public EventHandler OnStatusChanged;
        private Color _borderColor = Color.FromArgb(52, 52, 52);
        private ButtonBorderStyle _borderStyle = ButtonBorderStyle.Solid;
        private Color _contentBackColor = Color.FromArgb(232, 232, 232);
        private Size _contentSize = new Size(556, 200);
        private bool _fold;
        private Image _folderImage = Resources.PanelExpendableMinusImg;
        private Size _headerSize = new Size(556, 36);
        private int _orderIndex = -1;

        private PictureBox _toggler;
        private Image _unfolderImage = Resources.PanelExpendablePlusImg;

        public TnbExpendablePanel()
        {
            var tmpSize = new Size(HeaderSize.Width, HeaderSize.Height + ContentSize.Height);
            Size = tmpSize;
            if (HeaderImage == null)
                HeaderImage = Resources.panelcontrolHeaderbottomborder;
            if (TitleForeColor == new Color())
                TitleForeColor = Color.FromArgb(255, 255, 255);
            InitializeHeader();
            Padding = new Padding(0, 0, 0, 12);
            base.BackColor = _contentBackColor;
        }

        [Category("AaTnbControls")]
        public string TitleText
        {
            get { return _header.Text; }
            set { _header.Text = value; }
        }

        [Category("AaTnbControls")]
        public Color TitleForeColor
        {
            get { return _header.ForeColor; }
            set { _header.ForeColor = value; }
        }

        [Category("AaTnbControls")]
        public Font TitleFont
        {
            get { return _header.Font; }
            set { _header.Font = value; }
        }

        [Category("AaTnbControls")]
        public new ButtonBorderStyle BorderStyle
        {
            get { return _borderStyle; }
            set
            {
                _borderStyle = value;
                Invalidate(); // redraw
            }
        }

        [Category("AaTnbControls")]
        public Image HeaderImage
        {
            get { return _header.Image; }
            set { _header.Image = value; }
        }

        [Category("AaTnbControls")]
        public Color HeaderBackColor
        {
            get { return _header.BackColor; }
            set { _header.BackColor = value; }
        }

        public new Color BackColor
        {
            get { return _contentBackColor; }
            set { _contentBackColor = value; }
        }

        [Category("AaTnbControls")]
        public bool Fold
        {
            get { return _fold; }
            set
            {
                _fold = value;
                if (value)
                {
                    AutoSize = false;
                    var tmpSize = new Size(HeaderSize.Width, HeaderSize.Height);
                    _toggler.Image = Resources.PanelExpendablePlusImg;
                    Size = tmpSize;
                }
                else
                {
                    AutoSize = true;
                    var tmpSize = new Size(HeaderSize.Width, HeaderSize.Height + ContentSize.Height);
                    _toggler.Image = Resources.PanelExpendableMinusImg;
                    Size = tmpSize;
                    MaximumSize = new Size(HeaderSize.Width, 0);
                    MinimumSize = new Size(HeaderSize.Width, HeaderSize.Height);
                }
                if (OnStatusChanged != null)
                    OnStatusChanged(this, EventArgs.Empty);
            }
        }

        [Category("AaTnbControls")]
        public int OrderIndex
        {
            get { return _orderIndex; }
            set
            {
                _orderIndex = value;
                if (OnOrderChanged != null)
                    OnOrderChanged(this, EventArgs.Empty);
            }
        }

        [Category("AaTnbControls")]
        public Size HeaderSize
        {
            get { return _headerSize; }
            set { _headerSize = new Size(value.Width, 36); }
        }

        [Category("AaTnbControls")]
        public Size ContentSize
        {
            get { return new Size(HeaderSize.Width, _contentSize.Height); }
            set { _contentSize = new Size(HeaderSize.Width, value.Height); }
        }

        [Category("AaTnbControls")]
        public Image FolderImage
        {
            get { return _folderImage; }
            set
            {
                if (value != null)
                {
                    _folderImage = value;
                }
            }
        }

        [Category("AaTnbControls")]
        public Image UnfolderImage
        {
            get { return _unfolderImage; }
            set
            {
                if (value != null)
                {
                    _unfolderImage = value;
                }
            }
        }


        [Category("AaTnbControls")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WmPaint)
            {
                //Graphics g = Graphics.FromHwnd(Handle);
                //var mainRect = new Rectangle(0, 0, Width, Height);
                //ControlPaint.DrawBorder(g, mainRect, BorderColor, BorderStyle);
            }
        }

        private void InitializeHeader()
        {
            _toggler = new PictureBox();
            Controls.Add(_toggler);
            _toggler.Image = !Fold ? FolderImage : UnfolderImage;
            _toggler.Location = new Point(HeaderSize.Width - 40, 17);
            _toggler.Visible = true;
            _toggler.Size = new Size(7, 6);
            Controls.Add(_header);
            _header.Visible = true;
            _header.AutoSize = false;
            _header.Text = @"Expendable Panel TitleText";
            _header.Font = TitleFont;
            _header.ForeColor = TitleForeColor;
            _header.TextAlign = ContentAlignment.MiddleCenter;
            _header.Size = HeaderSize;
            _header.Location = new Point(0, 0);
            if (HeaderImage != null)
                _header.Image = HeaderImage;
            else
                _header.BackColor = HeaderBackColor;
            _header.Invalidate();
            _header.Click += OnClickEvent;

            Fold = true;
        }

        private void OnClickEvent(object sender, EventArgs eventArgs)
        {
            Fold = !Fold;
        }
    }
}