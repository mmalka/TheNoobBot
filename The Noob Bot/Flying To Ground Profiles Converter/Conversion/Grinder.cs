using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Grinder.Profile;
using nManager;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Helpers;

namespace Flying_To_Ground_Profiles_Converter.Conversion
{
    public class Grinder
    {
        private static List<Point> _points = new List<Point>();
        private static bool _result;

        public static bool IsGrinderProfile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    string text = Others.ReadFile(path);
                    if (text.Contains("<GrinderProfile") && text.Contains("<Points>"))
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
                var flyingProfile = XmlSerializer.Deserialize<GrinderProfile>(path);
                var tempFlyingProfile = new GrinderProfile();
                string flyingProfileName = Path.GetFileNameWithoutExtension(path);

                if (flyingProfile.GrinderZones.Count <= 0)
                {
                    Logging.Write(string.Format("The file {0} is not a valid Grinder Profile.", flyingProfileName));
                    return false;
                }

                foreach (GrinderZone zone in flyingProfile.GrinderZones)
                {
                    if (zone.Points.Any(point => point.Type.ToLower() == "flying"))
                    {
                        MovementManager.FlyingToGroundProfilesConverter(zone.Points, out _points, out _result);
                        if (_result)
                        {
                            zone.Points = _points;
                            tempFlyingProfile.GrinderZones.Add(zone);
                            continue;
                        }
                        Logging.Write(string.Format("The profile {0} contains at least one Flying Grinder Zone but the conversion has failed.", flyingProfileName));
                        return false;
                    }
                    Logging.Write(string.Format("The profile {0} does not contains any Flying Grinder Zone.", flyingProfileName));
                    return false;
                }

                flyingProfile.GrinderZones = tempFlyingProfile.GrinderZones;

                string pathDir = Application.StartupPath + "\\Profiles\\Grinder\\";
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
                Logging.Write("Conversion of the file failed.");
                return false;
            }
        }
    }
}