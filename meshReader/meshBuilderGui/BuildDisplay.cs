using System;
using System.Drawing;
using System.Windows.Forms;

namespace meshBuilderGui
{
    public partial class BuildDisplay : UserControl
    {
        private Bitmap _state;
        private readonly object _lock = new object();

        public BuildDisplay()
        {
            InitializeComponent();
            Clear();
        }

        public void MarkCompleted(int x, int y)
        {
            pictureBox1.Invoke(new Action(() => Mark(x, y, Color.LimeGreen))); //Color.Green)));
        }

        public void MarkRunning(int x, int y)
        {
            pictureBox1.Invoke(new Action(() => Mark(x, y, Color.Orange)));
        }

        public void MarkFailed(int x, int y)
        {
            pictureBox1.Invoke(new Action(() => Mark(x, y, Color.Red)));
        }
        public void MarkAlreadyBuilt(int x, int y)
        {
            pictureBox1.Invoke(new Action(() => Mark(x, y, Color.DodgerBlue))); //Color.Blue)));
        }

        public void MarkToBeDone(int x, int y)
        {
            pictureBox1.Invoke(new Action(() => Mark(x, y, Color.LightGray))); //Color.PeachPuff))); //Color.LightYellow)));
        }

        public void MarkDoneByother(int x, int y)
        {
            pictureBox1.Invoke(new Action(() => Mark(x, y, Color.HotPink)));
        }

        private void Mark(int tx, int ty, Color color)
        {
            lock (_lock)
            {
                try
                {
                    pictureBox1.Image = null;
                    const int offsetX = 0;
                    int xBegin = offsetX + (tx*4);
                    const int offsetY = 0;
                    int yBegin = offsetY + (ty*4);

                    for (int y = yBegin; y < (yBegin + 4); y++)
                    {
                        for (int x = xBegin; x < (xBegin + 4); x++)
                        {
                            _state.SetPixel(x, y, color);
                        }
                    }

                    pictureBox1.Image = _state;
                }
                catch (Exception)
                {
                    
                }
            }
        }

        public void Clear()
        {
            _state = new Bitmap(257, 257);
            for (int y = 0; y < 257; y++)
            {
                for (int x = 0; x < 257; x++)
                {
                    _state.SetPixel(x, y, Color.White);
                }
            }
            pictureBox1.Image = _state;
        }
    }
}
