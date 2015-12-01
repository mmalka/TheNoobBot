using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Gatherer;
using nManager.Helpful;
using nManager.Wow.Class;
using GathererProfile = nManager.Wow.Class.GathererProfile;

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
                    string text = Others.ReadFile(path);
                    if (text.Contains("<Profile") && text.Contains("<Points>"))
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
                if (IsWowRobotGatherFlyProfile(path))
                {
                    Profile origineProfile = XmlSerializer.Deserialize<Profile>(path);
                    GathererProfile profile = new GathererProfile();

                    foreach (Point p in origineProfile.Points)
                    {
                        profile.Points.Add(new Point(p.X, p.Y, p.Z, "Flying"));
                    }
                    foreach (Point p in origineProfile.BlackListPoints)
                    {
                        profile.BlackListRadius.Add(new GathererBlackListRadius {Position = p, Radius = 15});
                    }


                    string fileName = Path.GetFileNameWithoutExtension(path);

                    if (XmlSerializer.Serialize(Application.StartupPath + "\\Profiles\\Gatherer\\" + fileName + ".xml",
                        profile))
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