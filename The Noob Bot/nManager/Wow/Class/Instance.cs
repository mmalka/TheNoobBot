using System;
using System.Xml.Serialization;
using nManager.Wow.Enums;

namespace nManager.Wow.Class
{
    [Serializable]
    public class Instance
    {
        [XmlAttribute("Id")] public int InstanceId = 0; // Simple indexing Id. The real "InstanceId" will be its ContinentId.
        [XmlAttribute("Name")] public string InstanceName = "None";
        [XmlAttribute("Level")] public uint InstanceLevel = 110; // Will determine if the instance is actually solo-able for this character or not.
        [XmlAttribute("Active")] public bool Active = false;

        public int EntranceContinentId = -1; // Continent where you can find the instance entrance.
        public Point EntranceLocation = new Point(); // Behind the portal to get in.

        public int InstanceContinentId = -1; // ContinentId of the instance itself.
        public Point DepartureLocation = new Point(); // Behind the portal to get out.

        public float InstancePriority = 1; // User will be able to let multiple instances active, and the bot will loop though them every 10 cleans.
        public bool KillBosses = false;

        public uint LastUnitId = 0; // The id of the last unit to kill before reseting or leaving the instance.
        public bool Reset = true;

        public InstanceType Type = InstanceType.Dungeon;
    }
}