using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using meshReader.Game;
using meshReader.Game.Miscellaneous;

namespace meshBuilderGui
{
    public partial class MinimapDisplay : UserControl
    {
        private int _totalX;
        private int _totalY;

        public MinimapDisplay()
        {
            InitializeComponent();
        }

        private Bitmap ResizeImage(Image imgToResize)
        {
            int destWidth = (pictureBox1.Width / _totalX);
            int destHeight = (pictureBox1.Height / _totalY);

            var b = new Bitmap(destWidth, destHeight);
            var g = Graphics.FromImage(b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return b;
        }

        private int GetXBegin(int xLoc)
        {
            return (pictureBox1.Width / _totalX) * xLoc;
        }

        private int GetYBegin(int yLoc)
        {
            return (pictureBox1.Height / _totalY) * yLoc;
        }

        private static string GetMinimapFileByCoords(string world, int x, int y)
        {
            return "World\\Minimaps\\" + world + "\\map" + x + "_" + y + ".blp";
        }

        public void LoadFromContinent(string continent)
        {
            var wdt = new WDT("World\\Maps\\" + continent + "\\" + continent + ".wdt");
            if (!wdt.IsValid || wdt.IsGlobalModel)
                return;

            var result = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            for (int i = 0; i < result.Width; i++)
            {
                for (int j = 0; j < result.Height; j++)
                    result.SetPixel(i, j, Color.FromArgb(0, 29, 40));
            }

            int minX = 65, maxX = 0, minY = 65, maxY = 0;
            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    if (!wdt.HasTile(x, y))
                        continue;

                    if (x < minX)
                        minX = x;
                    if (x > maxX)
                        maxX = x;
                    if (y < minY)
                        minY = y;
                    if (y > maxY)
                        maxY = y;
                }
            }

            _totalX = maxX - minX + 1;
            _totalY = maxY - minY + 1;

            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    if (!wdt.HasTile(x, y))
                        continue;

                    try
                    {
                        var path = GetMinimapFileByCoords(continent, x, y);
                        var xBegin = GetXBegin(x - minX);
                        var yBegin = GetYBegin(y - minY);

                        var image = new Blp(path).GetImage(0);
                        var resized = ResizeImage(image);
                        for (int i = 0; i < resized.Width; i++)
                        {
                            for (int j = 0; j < resized.Height; j++)
                                result.SetPixel(xBegin + i, yBegin + j, resized.GetPixel(i, j));
                        }
                    }
                    catch (FileNotFoundException)
                    {
                    }
                }
            }

            pictureBox1.Image = result;
            pictureBox1.Visible = true;
        }

        private void OnClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
        }
    }
}