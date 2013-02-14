using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using meshDatabase;
using meshPather;
using meshReader;
using meshReader.Game;
using meshReader.Game.Miscellaneous;
using RecastLayer;
using Point = System.Windows.Point;
using System.Linq;

namespace meshMarker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private int _startX;
        private int _startY;
        private readonly PointCollection _points = new PointCollection();
        private Polygon _tempPolygon;
        private readonly List<Polygon> _finalPolygons = new List<Polygon>();
        private readonly List<Ellipse> _finalEllipses = new List<Ellipse>();
        private readonly ObservableCollection<string> _continentList = new ObservableCollection<string>();

        private Pather _pather;

        private string _world = "Azeroth";

        private static string GetMinimapFileByCoords(string world, int x, int y)
        {
            return "World\\Minimaps\\" + world + "\\map" + x + "_" + y + ".blp";
        }

        private bool UseCircleTool
        {
            get { return circleRadio.IsChecked != null && circleRadio.IsChecked.Value; }
        }

        private void UpdateBackground()
        {
            if (_startX == 0 && _startY == 0)
            {
                var files = Directory.GetFiles("S:\\meshReader\\Meshes\\" + _world + "\\");
                var best =
                    files.Where(f => f.EndsWith(".tile")).OrderByDescending(f => new FileInfo(f).Length).FirstOrDefault();
                best = best.Substring(best.LastIndexOf('\\') + 1);
                var tokens = best.Split('_');
                _startX = int.Parse(tokens[1]);
                _startY = int.Parse(tokens[2].Substring(0, tokens[2].IndexOf('.')));
            }

            originTileLabel.Content = OriginTileX + ", " + OriginTileY;
            _pather = new Pather("S:\\meshReader\\Meshes\\" + _world);

            var wdt = new WDT("World\\Maps\\" + _world + "\\" + _world + ".wdt");
            if (!wdt.IsValid || wdt.IsGlobalModel)
                return;                   

            const int totalX = 3;
            const int totalY = 3;

            const int tileSize = 256;
            var result = new Bitmap(totalX * tileSize, totalY * tileSize);
            for (int y = _startY - 1; y <= _startY+1; y++)
            {
                for (int x = _startX - 1; x <= _startX+1; x++)
                {
                    if (!wdt.HasTile(x, y))
                        continue;

                    try
                    {
                        var path = GetMinimapFileByCoords(_world, x, y);
                        var xBegin = (x-(_startX-1))*tileSize;
                        var yBegin = (y-(_startY-1))*tileSize;

                        var image = new Blp(path).GetImage(0);
                        var bitmap = new Bitmap(image);
                        for (int i = 0; i < tileSize; i++)
                        {
                            for (int j = 0; j < tileSize; j++)
                                result.SetPixel(xBegin + i, yBegin + j, bitmap.GetPixel(i, j));
                        }
                    }
                    catch (FileNotFoundException)
                    {

                    }
                }
            }

            // somebody needs to kill WPF developers for this bullshit
            var ms = new MemoryStream();
            result.Save(ms, ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);
            var bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();

            canvas1.Background = new ImageBrush(bi);
        }

        public MainWindow()
        {
            _startY = 0;
            InitializeComponent();
            MpqManager.Initialize("S:\\WoW\\");
            PopulateContinents();
        }

        private void PopulateContinents()
        {
            comboBox1.IsReadOnly = true;

            var list = Directory.GetDirectories("S:\\meshReader\\Meshes\\");
            foreach (var dir in list)
            {
                var files = Directory.GetFiles(dir);

                // dungeon mesh
                if (files.Any(f => f.EndsWith(".dmesh")))
                    continue;

                _continentList.Add(dir.Substring(dir.LastIndexOf('\\') + 1));
            }

            comboBox1.ItemsSource = _continentList;
            comboBox1.SelectionChanged += HandleContinentSelection;
        }

        private void HandleContinentSelection(object sender, SelectionChangedEventArgs e)
        {
            _world = (string) comboBox1.SelectedItem;
            _startX = 0;
            _startY = 0;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    _startY -= 1;
                    FixPolygons(0, +256);
                    UpdateBackground();
                    break;

                case Key.Down:
                    _startY += 1;
                    FixPolygons(0, -256);
                    UpdateBackground();
                    break;

                case Key.Left:
                    _startX -= 1;
                    FixPolygons(+256, 0);
                    UpdateBackground();
                    break;

                case Key.Right:
                    _startX += 1;
                    FixPolygons(-256, 0);
                    UpdateBackground();
                    break;
            }
        }

        private static bool IsLeft(Point a, Point b, Point c)
        {
            var u1 = b.X - a.X;
            var v1 = b.Y - a.Y;
            var u2 = c.X - a.X;
            var v2 = c.Y - a.Y;
            return u1*v2 - v1*u2 < 0;
        }

        private static bool ComparePoint(Point a, Point b)
        {
            if (a.X < b.X) return true;
            if (a.X > b.X) return false;
            if (a.Y < b.Y) return true;
            if (a.Y > b.Y) return false;
            return false;
        }

        private static Polygon CalculateConvexHull(PointCollection points)
        {
            // FInd lower-leftmost point
            int hull = 0;
            int i;
            for (i = 1; i < points.Count; i++)
                if (ComparePoint(points[i], points[hull]))
                    hull = i;

            // Gift wrap hull
            var outIndices = new int[points.Count];
            int endPt;
            i = 0;
            do
            {
                outIndices[i++] = hull;
                endPt = 0;
                for (int j = 1; j < points.Count; j++)
                    if (hull == endPt || IsLeft(points[hull], points[endPt], points[j]))
                        endPt = j;
                hull = endPt;
            } while (endPt != outIndices[0]);

            // build polygon
            int results = i;
            var ret = new Polygon();
            for (i = 0; i < results; i++)
                ret.Points.Add(points[outIndices[i]]);
            return ret;
        }

        private void OnClick(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(canvas1);
            _points.Add(pos);
            
            if (UseCircleTool)
            {
                var ellipse = new Ellipse();
                ellipse.Width = ellipse.Height = 10;
                ellipse.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(110, 255, 0, 0));
                ellipse.Margin = new Thickness(pos.X - (10 / 2), pos.Y - (10 / 2), 0, 0);
                var rdPos = GetPointLocation(pos.X, pos.Y);
                var gamePos = World.ToWoW(rdPos);
                ellipse.ToolTip = (int)rdPos[0] + ", " + (int)rdPos[2] + "[" + (int)gamePos[0] + ", " + (int)gamePos[1] + "]";
                canvas1.Children.Add(ellipse);
                _finalEllipses.Add(ellipse);
                return;
            }

            if (_points.Count > 2)
            {
                CreatePolygon();
            }
        }

        private void CreatePolygon()
        {
            if (_tempPolygon != null && canvas1.Children.Contains(_tempPolygon))
                canvas1.Children.Remove(_tempPolygon);

            _tempPolygon = CalculateConvexHull(_points);
            _tempPolygon.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(100, 0, 0, 255));
            canvas1.Children.Add(_tempPolygon);
        }

        public int OriginTileX
        {
            get { return _startX - 1; }
        }

        public int OriginTileY
        {
            get { return _startY - 1; }
        }

        private void FixPolygons(int xChange, int yChange)
        {
            if (!UseCircleTool)
            {
                foreach (var poly in _finalPolygons)
                {
                    for (int i = 0; i < poly.Points.Count; i++)
                    {
                        var pos = poly.Points[i];
                        pos.X += xChange;
                        pos.Y += yChange;
                        poly.Points[i] = pos;
                    }
                }
            }

            for (int i = 0; i < _finalEllipses.Count; i++)
            {
                var ellipse = _finalEllipses[i];
                var margin = ellipse.Margin;
                margin.Left += xChange;
                margin.Top += yChange;
                _finalEllipses[i].Margin = margin;
            }
        }

        private void OnRightClick(object sender, MouseButtonEventArgs e)
        {
            if (_points.Count > 2)
            {
                _finalPolygons.Add(_tempPolygon);
                _tempPolygon = null;
                _points.Clear();
            }
        }

        private int MarkMeshEllipses(PolyArea areaType)
        {
            int marked = 0;

            foreach (var ellipse in _finalEllipses)
            {
                var center = GetPointLocation(ellipse.Margin.Left, ellipse.Margin.Top);

                uint[] foundPolys;
                var status = _pather.Query.QueryPolygons(center, new float[] { 5, 500, 5 }, _pather.Filter, out foundPolys);
                if (foundPolys == null || status.HasFailed())
                    continue;

                bool haveHeight = false;
                float terrainHeight = 0.0f;
                foreach (var pref in foundPolys)
                {
                    var height = _pather.Query.GetPolyHeight(pref, center);
                    if (height == 0.0f)
                        continue;
                    terrainHeight = height;
                    haveHeight = true;
                    break;
                }

                if (!haveHeight)
                    continue;

                center[1] = terrainHeight;

                var startRef = _pather.Query.FindNearestPolygon(center, new float[] {5, 5, 5}, _pather.Filter);
                if (startRef == 0)
                    continue;

                marked += _pather.Query.MarkAreaInCircle(startRef, center, 5, _pather.Filter, PolyArea.Road);
            }

            return marked;
        }

        private int MarkMesh(PolyArea areaType)
        {
            int marked = 0;

            foreach (var poly in _finalPolygons)
            {
                // calculate center
                var center = new float[3];
                foreach (var point in poly.Points)
                {
                    var loc = GetPointLocation(point.X, point.Y);
                    center[0] += loc[0];
                    center[1] += loc[1];
                    center[2] += loc[2];
                }
                center[0] /= poly.Points.Count;
                center[1] /= poly.Points.Count;
                center[2] /= poly.Points.Count;

                uint[] foundPolys;
                var status = _pather.Query.QueryPolygons(center, new float[] {5, 500, 5}, _pather.Filter, out foundPolys);
                if (foundPolys == null || status.HasFailed())
                    continue;

                bool haveHeight = false;
                float terrainHeight = 0.0f;
                foreach (var pref in foundPolys)
                {
                    var height = _pather.Query.GetPolyHeight(pref, center);
                    if (height == 0.0f)
                        continue;
                    terrainHeight = height;
                    haveHeight = true;
                    break;
                }

                if (!haveHeight)
                    continue;

                center[1] = terrainHeight;

                // two points per poly point since we need 3D
                var verts = new float[poly.Points.Count * 6];

                for (int i = 0; i < poly.Points.Count; i++)
                {
                    var point = poly.Points[i];
                    var loc = GetPointLocation(point.X, point.Y);
                    verts[(i*6) + 0] = loc[0];
                    verts[(i*6) + 1] = terrainHeight - 20.0f;
                    verts[(i*6) + 2] = loc[2];
                    verts[(i*6) + 3] = loc[0];
                    verts[(i*6) + 4] = terrainHeight + 20.0f;
                    verts[(i*6) + 5] = loc[2];
                }

                var startRef = _pather.Query.FindNearestPolygon(center, new float[] {5, 5, 5}, _pather.Filter);
                if (startRef == 0)
                    continue;

                marked += _pather.Query.MarkAreaInShape(startRef, verts, _pather.Filter, areaType);
            }

            return marked;
        }

        private float[] GetPointLocation(double x, double y)
        {
            float[] bmin, bmax;
            _pather.Mesh.GetTileBBox(OriginTileX, OriginTileY, out bmin, out bmax);
            bmin[0] += (float)((bmax[0] - bmin[0])*(x/256));
            bmin[2] += (float)((bmax[2] - bmin[2])*(y/256));
            return bmin;
        }

        private void reloadButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _finalPolygons.Clear();
            _tempPolygon = null;
            _points.Clear();
            comboBox1.IsEnabled = false;
            UpdateBackground();
        }

        private void button1_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _points.Clear();
            _tempPolygon = null;
        }

        private void button3_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_finalPolygons.Count > 0)
            {
                canvas1.Children.Remove(_finalPolygons[_finalPolygons.Count - 1]);
                _finalPolygons.RemoveAt(_finalPolygons.Count - 1);
            }
        }

        private void button4_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_pather == null)
                return;

            progressBar1.Maximum = 100;
            progressBar1.Value = 0;

            for (int y = OriginTileY; y <= OriginTileY + 2; y++)
                for (int x = OriginTileX; x <= OriginTileX + 2; x++)
                    _pather.LoadTile(x, y);

            progressBar1.Value = 30;

            if (UseCircleTool)
                markedLabel.Content = MarkMeshEllipses(PolyArea.Road);
            else
                markedLabel.Content = MarkMesh(PolyArea.Road);
            progressBar1.Value = 60;

            for (int y = OriginTileY; y <= OriginTileY + 2; y++)
            {
                for (int x = OriginTileX; x <= OriginTileX + 2; x++)
                {
                    byte[] data;
                    if (_pather.RemoveTile(x, y, out data) && data != null)
                        File.WriteAllBytes(_pather.GetTilePath(x, y), data);
                }
            }
            progressBar1.Value = 100;
        }

        private void button2_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _finalPolygons.Clear();
            canvas1.Children.Clear();
        }
    }
}
