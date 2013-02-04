using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Gatherer;
using nManager.Helpful;
using nManager.Wow.Class;

namespace Profiles_Converters.Converters
{
    public class WowRobotGatherFly
    {
        public static bool IsWowRobotGatherFlyProfile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    var text = Others.ReadFile(path);
                    if (text.Contains("<Profile") && text.Contains("<Points>"))
                        return true;
                }
                else
                {
                    MessageBox.Show(nManager.Translate.Get(nManager.Translate.Id.File_not_found) + ".");
                    ;
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
                if (IsWowRobotGatherFlyProfile(path))
                {
                    var _origineProfile = XmlSerializer.Deserialize<Profile>(path);
                    var _profile = new GathererProfile();

                    foreach (var p in _origineProfile.Points)
                    {
                        _profile.Points.Add(new Point(p.X, p.Y, p.Z, "Flying"));
                    }
                    foreach (var p in _origineProfile.BlackListPoints)
                    {
                        _profile.BlackListRadius.Add(new GathererBlackListRadius {Position = p, Radius = 15});
                    }


                    var fileName = Path.GetFileNameWithoutExtension(path);

                    if (XmlSerializer.Serialize(Application.StartupPath + "\\Profiles\\Gatherer\\" + fileName + ".xml",
                                                _profile))
                    {
                        Logging.Write("Conversion Success (WowRobot to Gatherer bot): " + fileName);
                        return true;
                    }
                }
            }
            catch
            {
            }
            Logging.Write("Conversion Failled (WowRobot to Gatherer bot): " + path);
            return false;
        }

        // Class
        [Serializable]
        public class Profile
        {
            // List of location:
            public List<Point> Points = new List<Point>();
            // List of location BlackList:
            public List<Point> BlackListPoints = new List<Point>();
            // List of location City:
            public List<Point> CityPoints = new List<Point>();
            // Goto
            public bool GoTo;
        }
    }
}