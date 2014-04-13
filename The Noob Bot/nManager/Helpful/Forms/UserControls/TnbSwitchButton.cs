using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using nManager.Properties;

namespace nManager.Helpful.Forms.UserControls
{
    public sealed class TnbSwitchButton : Panel
    {
        private readonly Label _leftSide = new Label();
        private readonly PictureBox _rightSide = new PictureBox();

        private string _offText = "OFF";

        private string _onText = "ON";
        private bool _valuePrivate;

        public TnbSwitchButton()
        {
            SuspendLayout();
            Font = new Font(Font, FontStyle.Bold);
            MaximumSize = new Size(60, 20);
            MinimumSize = new Size(60, 20);
            Size = new Size(60, 20);
            Controls.Add(_leftSide);
            Controls.Add(_rightSide);
            _leftSide.AutoSize = false;
            _rightSide.AutoSize = false;
            _leftSide.MinimumSize = new Size(47, 20);
            _rightSide.MinimumSize = new Size(13, 20);
            _leftSide.MaximumSize = new Size(47, 20);
            _rightSide.MaximumSize = new Size(13, 20);
            _leftSide.Size = new Size(47, 20);
            _rightSide.Size = new Size(13, 20);
            _leftSide.Location = new Point(0, 0);
            _rightSide.Location = new Point(47, 0);
            _leftSide.TextAlign = ContentAlignment.MiddleCenter;
            _leftSide.ForeColor = Color.Snow;
            _leftSide.Font = Font;
            _leftSide.MouseClick += OnMouseClick;

            _rightSide.MouseClick += OnMouseClick;
            if (Value)
            {
                _leftSide.Text = OnText;
                _leftSide.Image = Resources.switchonl;
                _rightSide.Image = Resources.switchonr;
            }
            else
            {
                _leftSide.Text = OffText;
                _leftSide.Image = Resources.switchoffl;
                _rightSide.Image = Resources.switchoffr;
            }
            ResumeLayout(false);
            PerformLayout();
        }


        [Category("Appearance")]
        public string OffText
        {
            get { return _offText; }
            set
            {
                _offText = value;
                if (!Value)
                    _leftSide.Text = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public string OnText
        {
            get { return _onText; }
            set
            {
                _onText = value;
                if (Value)
                    _leftSide.Text = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public bool Value
        {
            get { return _valuePrivate; }
            set
            {
                _valuePrivate = value;
                UpdateValue(value);
                if (ValueChanged != null)
                    ValueChanged(this, EventArgs.Empty); // Fires event ValueChanged
                Invalidate();
            }
        }

        public event EventHandler ValueChanged;

        private void UpdateValue(bool value)
        {
            if (value)
            {
                _leftSide.Text = OnText;
                _leftSide.Image = Resources.switchonl;
                _rightSide.Image = Resources.switchonr;
            }
            else
            {
                _leftSide.Text = OffText;
                _leftSide.Image = Resources.switchoffl;
                _rightSide.Image = Resources.switchoffr;
            }
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            Value = !Value;
            UpdateValue(Value);
        }
    }
}