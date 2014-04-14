using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using nManager.Properties;

namespace nManager.Helpful.Forms.UserControls
{
    internal class TnbControlMenu : Panel
    {
        private PictureBox _closeButton;
        private Image _logoImage = Resources.logoImageG;
        private PictureBox _reduceButton;
        private Font _titleFont = new Font(new FontFamily("Microsoft Sans Serif"), 12, GraphicsUnit.Point);
        private Color _titleForeColor = Color.FromArgb(222, 222, 222);
        private Label _titleLabel = new Label();
        private PictureBox _tnbLogo;
        private bool _flagClick;
        private int _positionInitialeX;
        private int _positionInitialeY;

        public TnbControlMenu()
        {
            Size = new Size(575, 43);
            InitializeContent();
        }

        [Category("AaTnbControls")]
        public string TitleText
        {
            get { return _titleLabel.Text; }
            set
            {
                _titleLabel.Text = value;
                Invalidate();
            }
        }

        [Category("AaTnbControls")]
        public Image LogoImage
        {
            get { return _logoImage; }
            set
            {
                _logoImage = value;
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

        private void InitializeContent()
        {
            _reduceButton = new PictureBox {Visible = true, Location = new Point(523, 13), Size = new Size(13, 14), Image = Resources.reduce_button};
            _reduceButton.Click += OnReduce;
            _closeButton = new PictureBox {Visible = true, Location = new Point(550, 13), Size = new Size(13, 14), Image = Resources.close_button};
            _closeButton.Click += OnClose;
            _tnbLogo = new PictureBox {Visible = true, Location = new Point(13, 3), Size = new Size(30, 33), Image = LogoImage};
            _titleLabel = new Label
            {
                Location = new Point(57, 4),
                Size = new Size(450, 35),
                Visible = true,
                AutoSize = false,
                BackColor = Color.FromArgb(65, 65, 65),
                Text = TitleText,
                Font = TitleFont,
                ForeColor = TitleForeColor,
                TextAlign = ContentAlignment.MiddleCenter
            };
            base.AutoSize = false;
            base.BackgroundImage = Resources.controlbar;

            Controls.Add(_closeButton);
            Controls.Add(_titleLabel);
            Controls.Add(_tnbLogo);
            Controls.Add(_reduceButton);
            _tnbLogo.MouseDown += MainFormMouseDown;
            _tnbLogo.MouseMove += MainFormMouseMove;
            _tnbLogo.MouseUp += MainFormMouseUp;
            _titleLabel.MouseDown += MainFormMouseDown;
            _titleLabel.MouseMove += MainFormMouseMove;
            _titleLabel.MouseUp += MainFormMouseUp;
            MouseDown += MainFormMouseDown;
            MouseMove += MainFormMouseMove;
            MouseUp += MainFormMouseUp;
            _closeButton.MouseEnter += MouseEnterCloseButton;
            _closeButton.MouseLeave += MouseLeaveCloseButton;
            _reduceButton.MouseEnter += MouseEnterReduceButton;
            _reduceButton.MouseLeave += MouseLeaveReduceButton;
        }

        private void MouseEnterCloseButton(object sender, EventArgs eventArgs)
        {
            _closeButton.Image = Resources.close_buttonG;
        }

        private void MouseLeaveCloseButton(object sender, EventArgs eventArgs)
        {
            _closeButton.Image = Resources.close_button;
        }

        private void MouseEnterReduceButton(object sender, EventArgs eventArgs)
        {
            _reduceButton.Image = Resources.reduce_buttonG;
        }
        private void MouseLeaveReduceButton(object sender, EventArgs eventArgs)
        {
            _reduceButton.Image = Resources.reduce_button;
        }

        private void OnClose(object sender, EventArgs eventArgs)
        {
            if (Form.ActiveForm != null)
                Form.ActiveForm.Close();
        }

        private void OnReduce(object sender, EventArgs eventArgs)
        {
            if (Form.ActiveForm != null)
                Form.ActiveForm.WindowState = FormWindowState.Minimized;
        }
        
        private void MainFormMouseDown(object sender, MouseEventArgs e)
        {
            _flagClick = true;
            _positionInitialeX = e.X;
            _positionInitialeY = e.Y;
        }

        private void MainFormMouseUp(object sender, MouseEventArgs e)
        {
            _flagClick = false;
        }


        private void MainFormMouseMove(object sender, MouseEventArgs e)
        {
            if (_flagClick)
            {
                Parent.Location = new Point(Parent.Left + (e.X - _positionInitialeX), Parent.Top + (e.Y - _positionInitialeY));
            }
        }

    }
}