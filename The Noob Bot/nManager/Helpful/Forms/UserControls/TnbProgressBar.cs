using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using nManager.Properties;

namespace nManager.Helpful.Forms.UserControls
{
    public class TnbProgressBar : Panel
    {
        private const int WmPaint = 0x000F;
        private const int Maximum = 100;
        private Image _barImage = Resources.barImg;
        private Color _borderColor = Color.FromArgb(121, 121, 121);
        private ButtonBorderStyle _borderStyle = ButtonBorderStyle.Solid;
        private int _value;

        public TnbProgressBar()
        {
            Size = new Size(Size.Width, 10);
            // Force initialize with 10px Height.
        }

        public DrawMode DrawMode { get; set; }

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
        public int Value
        {
            get { return _value; }
            set
            {
                if (value < 0)
                    value = 0;
                if (value > Maximum)
                    value = Maximum;
                _value = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public new ButtonBorderStyle BorderStyle
        {
            get { return _borderStyle; }
            set
            {
                _borderStyle = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Image BarImage
        {
            get { return _barImage; }
            set
            {
                if (value != null)
                    _barImage = value;
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
                var barRect = new Rectangle(mainRect.X + 1, mainRect.Y + 1, mainRect.Width - 2, mainRect.Height - 2);
                if (Value != 0)
                {
                    barRect.Width = (int) (barRect.Width*((double) Value/Maximum));
                    FillPattern(g, BarImage, barRect);
                }
                if (Value != 100)
                    g.FillRectangle(new SolidBrush(BackColor), barRect.Width + barRect.X, barRect.Y, Width - barRect.Width - 2, barRect.Height);
            }
        }

        public static void FillPattern(Graphics g, Image image, Rectangle rect)
        {
            for (int x = rect.X; x < rect.Right; x += image.Width)
            {
                for (int y = rect.Y; y < rect.Bottom; y += image.Height)
                {
                    var drawRect = new Rectangle(x, y, System.Math.Min(image.Width, rect.Right - x),
                        System.Math.Min(image.Height, rect.Bottom - y));
                    var imageRect = new Rectangle(0, 0, drawRect.Width, drawRect.Height);

                    g.DrawImage(image, drawRect, imageRect, GraphicsUnit.Pixel);
                }
            }
        }
    }
}