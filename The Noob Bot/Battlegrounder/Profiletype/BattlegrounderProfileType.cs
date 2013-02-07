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
        public static List<Battleground> Battlegrounds = new List<Battleground>();
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
        public bool Bool1;
        public bool Bool2;
        public bool Bool3;
        public int Int1;
        public int Int2;
        public int Int3;
        public string String1;
        public string String2;
        public string String3;
        public double Timer1;
        public double Timer2;
        public double Timer3;
    }
}