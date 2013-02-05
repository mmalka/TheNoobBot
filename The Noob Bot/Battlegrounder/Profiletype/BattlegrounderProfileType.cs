using System;
using System.Collections.Generic;

namespace Battlegrounder.Profiletype
{
    public class ComboboxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }

    [Serializable]
    public class BattlegrounderProfileType
    {
        public List<Battleground> Battlegrounds = new List<Battleground>();
    }

    [Serializable]
    public class Battleground
    {
        public string BattlegroundName = "";
        public string BattlegroundId;
        public List<ProfileType> ProfileTypes = new List<ProfileType>();
    }

    [Serializable]
    public class ProfileType
    {
        public string ProfileTypeName = "";
        public string ProfileTypeId = "";
    }
}