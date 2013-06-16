using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Gatherer;
using nManager.Helpful;
using nManager.Wow.Class;

namespace Profiles_Converters.Converters
{
    public class PiroxFlyGatherer
    {
        public static bool IsPiroxFlyGathererProfile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    string text = Others.ReadFile(path);
                    if (text.Contains("[GoTo]") && text.Contains("UseFlyMount"))
                        return true;
                }
                else
                {
                    MessageBox.Show(string.Format("{0}.", nManager.Translate.Get(nManager.Translate.Id.File_not_found)));
                    }
            }
            catch
            {
            }
            return false;
        }

        public static bool Convert(string path)
        {
            try
            {
                if (IsPiroxFlyGathererProfile(path))
                {
                    GathererProfile _profile = new GathererProfile();
                    IniFile file = new IniFile(path);
                    for (int i = 1; i < 0x2710; i++)
                    {
                        string str4 = file.IniReadValue("GoTo", "z" + i);
                        if ((str4 != null) && str4.StartsWith("WPX"))
                        {
                            string[] strArray =
                                str4.Replace("WPX", "")
                                    .Replace("(", "")
                                    .Replace(")", "")
                                    .Trim()
                                    .Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                            Point item = new Point(float.Parse(strArray[0], CultureInfo.InvariantCulture),
                                                 float.Parse(strArray[1], CultureInfo.InvariantCulture),
                                                 float.Parse(strArray[2], CultureInfo.InvariantCulture), "Flying");
                            _profile.Points.Add(item);
                        }
                    }


                    string fileName = Path.GetFileNameWithoutExtension(path);

                    if (XmlSerializer.Serialize(Application.StartupPath + "\\Profiles\\Gatherer\\" + fileName + ".xml",
                                                _profile))
                    {
                        Logging.Write("Conversion Success (Pirox Fly Gatherer to Gatherer bot): " + fileName);
                        return true;
                    }
                }
            }
            catch
            {
            }
            Logging.Write("Conversion Failled (Pirox Fly Gatherer to Gatherer bot): " + path);
            return false;
        }
    }
}