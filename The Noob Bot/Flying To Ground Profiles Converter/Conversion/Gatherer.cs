using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Gatherer;
using nManager;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Helpers;

namespace Flying_To_Ground_Profiles_Converter.Conversion
{
    public class Gatherer
    {
        private static List<Point> _points = new List<Point>();
        private static bool _result;

        public static bool IsGathererProfile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    string text = Others.ReadFile(path);
                    if (text.Contains("<GathererProfile") && text.Contains("<Points>"))
                        return true;
                }
                else
                {
                    MessageBox.Show(string.Format("{0}.", Translate.Get(Translate.Id.File_not_found)));
                }
            }
            catch
            {
                MessageBox.Show(string.Format("{0}.", Translate.Get(Translate.Id.File_not_found)));
            }
            return false;
        }

        public static bool Convert(string path)
        {
            try
            {
                GathererProfile flyingProfile = XmlSerializer.Deserialize<GathererProfile>(path);
                string flyingProfileName = Path.GetFileNameWithoutExtension(path);

                if (flyingProfile.Points.Count <= 0)
                {
                    Logging.Write(string.Format("The file {0} is not a valid Gatherer Profile.", flyingProfileName));
                    return false;
                }

                if (flyingProfile.Points.Any(p => p.Type.ToLower() == "flying"))
                    MovementManager.FlyingToGroundProfilesConverter(flyingProfile.Points, out _points, out _result);
                else
                {
                    Logging.Write(string.Format("The profile {0} is not a Flying Gatherer Profile.", flyingProfileName));
                    return false;
                }
                if (!_result)
                    return false;
                flyingProfile.Points = _points;
                string pathDir = Application.StartupPath + "\\Profiles\\Gatherer\\";
                string fullPath = pathDir + "Ground_" + flyingProfileName + ".xml";
                if (XmlSerializer.Serialize(fullPath, flyingProfile))
                {
                    Logging.Write(string.Format("The Flying profile {0} have been saved.", flyingProfileName));
                    Logging.Write("Path: " + fullPath);
                    return true;
                }
                Logging.Write(string.Format("The Flying profile {0} have not been saved correctly, make sure you have access right on the directory {1}.",
                                            flyingProfileName, pathDir));
                return false;
            }
            catch
            {
                Logging.Write("Conversion Failled (WowRobot to Gatherer bot): " + path);
                return false;
            }
        }
    }
}