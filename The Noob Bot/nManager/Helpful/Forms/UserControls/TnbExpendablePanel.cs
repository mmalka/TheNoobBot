using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using nManager.Properties;

namespace nManager.Helpful.Forms.UserControls
{
    internal class TnbExpendablePanel : Panel
    {
        private const int WmPaint = 0x000F;
        private readonly Label _header = new Label();
        public EventHandler OnOrderChanged;
        public EventHandler OnStatusChanged;
        private Color _borderColor = Color.FromArgb(52, 52, 52);
        private ButtonBorderStyle _borderStyle = ButtonBorderStyle.Solid;
        private Color _contentBackColor = Color.FromArgb(232, 232, 232);
        private Size _contentSize = new Size(575, 200);
        private bool _fold;
        private Image _folderImage = Resources.PanelExpendableMinusImg;
        private Color _headerBackColor = Color.FromArgb(250, 250, 250);
        private Image _headerImage = Resources.panelcontrolHeader;
        private Size _headerSize = new Size(575, 37);
        private int _orderIndex = -1;

        private PictureBox _toggler;
        private Image _unfolderImage = Resources.PanelExpendablePlusImg;
        private Color _titleForeColor;
        private Font _titleFont;

        public TnbExpendablePanel()
        {
            var tmpSize = new Size(HeaderSize.Width, HeaderSize.Height + ContentSize.Height);
            Size = tmpSize;
            TitleFont = new Font(new FontFamily("Arial"),8,FontStyle.Bold,GraphicsUnit.Point );
            TitleForeColor = Color.White;
            InitializeHeader();
            base.BackColor = _contentBackColor;
        }

        [Category("AaTnbControls")]
        public string TitleText
        {
            get { return _header.Text; }
            set
            {
                _header.Text = value;
                Invalidate();
            }
        }

        [Category("AaTnbControls")]
        public Color TitleForeColor
        {
            get { return _titleForeColor; }
            set
            {
                _titleForeColor = value;
                Invalidate();
            }
        }

        [Category("AaTnbControls")]
        public Font TitleFont
        {
            get { return _titleFont; }
            set
            {
                _titleFont = value;
                Invalidate();
            }
        }

        [Category("AaTnbControls")]
        public new ButtonBorderStyle BorderStyle
        {
            get { return _borderStyle; }
            set
            {
                _borderStyle = value;
                Invalidate();
            }
        }

        [Category("AaTnbControls")]
        public Image HeaderImage
        {
            get { return _headerImage; }
            set
            {
                _headerImage = value;
                Invalidate();
            }
        }

        [Category("AaTnbControls")]
        public Color HeaderBackColor
        {
            get { return _headerBackColor; }
            set
            {
                _headerBackColor = value;
                Invalidate();
                _header.Invalidate();
            }
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
                }
                if (OnStatusChanged != null)
                    OnStatusChanged(this, EventArgs.Empty);
                Invalidate();
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
                Invalidate();
            }
        }

        [Category("AaTnbControls")]
        public Size HeaderSize
        {
            get { return _headerSize; }
            set
            {
                _headerSize = new Size(value.Width, 37);
                Invalidate();
            }
        }

        [Category("AaTnbControls")]
        public Size ContentSize
        {
            get { return _contentSize; }
            set
            {
                _contentSize = new Size(HeaderSize.Width, value.Height);
                Invalidate();
            }
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
                    Invalidate();
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
                    Invalidate();
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
                Graphics g = Graphics.FromHwnd(Handle);
                var mainRect = new Rectangle(0, 0, Width, Height);
                ControlPaint.DrawBorder(g, mainRect, BorderColor, BorderStyle);
            }
        }

        private void InitializeHeader()
        {
            _toggler = new PictureBox();
            Controls.Add(_toggler);
            _toggler.Image = !Fold ? FolderImage : UnfolderImage;
            _toggler.Location = new Point(550, 17);
            _toggler.Visible = true;
            _toggler.Size = new Size(7, 6);
            Controls.Add(_header);
            _header.Visible = true;
            _header.AutoSize = false;
            _header.Text = "Spell Management - Manage your Spell priority";
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