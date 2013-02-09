using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Battlegrounder.Profile;
using nManager.Helpful;
using nManager.Wow.Class;

namespace Battlegrounder.Profiletype
{
    internal class AfkSomewhere
    {
        private static List<Point> _points = new List<Point>();
        private static readonly Random Randomized = new Random();
        private static BattlegrounderProfile _currentProfile = new BattlegrounderProfile();
        private static int _i;

        public static List<Point> AfkSomewhereNow(string battlegroundId)
        {
            _currentProfile = new BattlegrounderProfile();
            string path = Application.StartupPath + "\\Profiles\\Battlegrounder\\ProfileType\\AfkSomewhere\\" +
                          battlegroundId + ".xml";
            if (File.Exists(path))
            {
                _currentProfile =
                    XmlSerializer.Deserialize<BattlegrounderProfile>(path);
                if (_currentProfile.IsValid())
                {
                    foreach (
                        BattlegrounderZone battleground in
                            _currentProfile.BattlegrounderZones.Where(
                                battleground => battleground.BattlegroundId == battlegroundId)
                                           .Where(battleground => battleground.IsValid()))
                    {
                        _points = battleground.Points;
                        _i = Randomized.Next(_points.Count);
                        return new List<Point> {_points[_i]};
                    }
                }
            }
            return null;
        }
    }
}