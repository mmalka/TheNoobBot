using System;
using System.Collections.Generic;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Patchables;

namespace nManager.Wow.ObjectManager
{
    public class WoWPlayer : WoWUnit
    {
        public WoWPlayer(uint address)
            : base(address)
        {
        }

        public WoWClass WowClass
        {
            get
            {
                try
                { return (WoWClass)BitConverter.GetBytes(GetDescriptor<Int32>((uint)Descriptors.UnitFields.UNIT_FIELD_BYTES_0))[1]; }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > PlayerFaction: " + e);
                    return WoWClass.None;
                }
            }
        }
        public string PlayerFaction
        {
            get
            {
                try
                {
                    switch (PlayerRace)
                    {
                        case "Human":
                        case "Dwarf":
                        case "Gnome":
                        case "Night Elf":
                        case "Draenei":
                        case "Worgen":
                            return "Alliance";
                        case "Orc":
                        case "Undead":
                        case "Tauren":
                        case "Troll":
                        case "Blood Elf":
                        case "Goblin":
                            return "Horde";
                    }
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > PlayerFaction: " + e);
                }
                return "Neutral";
            }
        }
        public string PlayerRace
        {
            get
            {
                try
                {
                    long faction = Faction;
                    if (faction.Equals((int)PlayerFactions.Human))
                        return "Human";
                    if (faction.Equals((int)PlayerFactions.BloodElf))
                        return "Blood Elf";
                    if (faction.Equals((int)PlayerFactions.Dwarf))
                        return "Dwarf";
                    if (faction.Equals((int)PlayerFactions.Gnome))
                        return "Gnome";
                    if (faction.Equals((int)PlayerFactions.NightElf))
                        return "Night Elf";
                    if (faction.Equals((int)PlayerFactions.Orc))
                        return "Orc";
                    if (faction.Equals((int)PlayerFactions.Tauren))
                        return "Tauren";
                    if (faction.Equals((int)PlayerFactions.Troll))
                        return "Troll";
                    if (faction.Equals((int)PlayerFactions.Undead))
                        return "Undead";
                    if (faction.Equals((int)PlayerFactions.Draenei))
                        return "Draenei";
                    if (faction.Equals((int)PlayerFactions.Goblin))
                        return "Goblin";
                    if (faction.Equals((int)PlayerFactions.Worgen))
                        return "Worgen";

                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > PlayerRace: " + e);
                }
                return "Unknown";
            }
        }
        public uint BarTwo
        {
            get
            {
                try
                {
                    return GetDescriptor<uint>((uint)Descriptors.UnitFields.UNIT_FIELD_POWER1);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > BarTwo: " + e);
                    return 0;
                }

            }
        }
        public uint BarTwoMax
        {
            get
            {
                try
                {
                    return GetDescriptor<uint>((uint)Descriptors.UnitFields.UNIT_FIELD_MAXPOWER1);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > BarTwoMax: " + e);
                    return 0;
                }

            }
        }
        public uint BarTwoPercentage
        {
            get
            {
                try
                {
                    return BarTwo * 100 / BarTwoMax;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > BarTwoPercentage: " + e);
                    return 0;
                }
            }
        }
        public int ComboPoint
        {
            get
            {
                try
                {
                    int val = Memory.WowMemory.Memory.ReadByte(Memory.WowProcess.WowModule + (uint)Addresses.Player.PlayerComboPoint);
                    return val;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > ComboPoint: " + e);
                }
                return 0;
            }
        }
        public new bool InCombat
        {
            get
            {
                try
                {
                    bool petAttack = false;
                    bool isdead = false;
                    var flags = GetDescriptor<Int32>((uint)Descriptors.UnitFields.UNIT_FIELD_FLAGS);
                    try
                    {
                        if (GetBaseAddress == ObjectManager.Me.GetBaseAddress)
                        {
                            if (IsDeadMe)
                                isdead = true;
                            if (ObjectManager.Pet.GetBaseAddress > 0)
                                if (ObjectManager.GetNumberAttackPlayer() > 0 && ObjectManager.Pet.InCombat && !ObjectManager.Pet.IsDead)
                                    petAttack = true;

                            if (Convert.ToBoolean(flags & 0x0080000) && !isdead)
                                if (ObjectManager.GetNumberAttackPlayer() > 0)
                                    return true;
                                else
                                    return (petAttack);
                        }
                    }
                    catch (Exception e)
                    {
                        Logging.WriteError("WoWPlayer > InCombat#1: " + e);
                    }
                    return ((Convert.ToBoolean(flags & 0x0080000) || petAttack) && !isdead);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > InCombat#2: " + e);
                }
                return false;
            }
        }
        public int GetDurability
        {
            get
            {
                try
                {
                    int durabilitys = 0;
                    int maxDurabilitys = 0;

                    var ItemId = new List<uint>
                                     {
                                         GetDescriptor<uint>(Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_1_ENTRYID),
                                         GetDescriptor<uint>(Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_2_ENTRYID),
                                         GetDescriptor<uint>(Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_3_ENTRYID),
                                         GetDescriptor<uint>(Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_4_ENTRYID),
                                         GetDescriptor<uint>(Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_5_ENTRYID),
                                         GetDescriptor<uint>(Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_6_ENTRYID),
                                         GetDescriptor<uint>(Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_7_ENTRYID),
                                         GetDescriptor<uint>(Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_8_ENTRYID),
                                         GetDescriptor<uint>(Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_9_ENTRYID),
                                         GetDescriptor<uint>(Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_10_ENTRYID),
                                         GetDescriptor<uint>(Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_11_ENTRYID),
                                         GetDescriptor<uint>(Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_12_ENTRYID),
                                         GetDescriptor<uint>(Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_13_ENTRYID),
                                         GetDescriptor<uint>(Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_14_ENTRYID),
                                         GetDescriptor<uint>(Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_15_ENTRYID),
                                         GetDescriptor<uint>(Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_16_ENTRYID),
                                         GetDescriptor<uint>(Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_17_ENTRYID),
                                         GetDescriptor<uint>(Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_18_ENTRYID),
                                         GetDescriptor<uint>(Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_19_ENTRYID)
                                     };

                    WoWObject[] objects = ObjectManager.ObjectList.ToArray();

                    foreach (WoWObject o in objects)
                    {
                        if (o.Type == WoWObjectType.Item)
                        {
                            try
                            {
                                var ItemIdTemp = GetDescriptor<uint>(o.GetBaseAddress,
                                                                      (uint)Descriptors.ObjectFields.OBJECT_FIELD_ENTRY);
                                var ItemGuidOwner = GetDescriptor<ulong>(o.GetBaseAddress,
                                                                           (uint)
                                                                           Descriptors.ItemFields.ITEM_FIELD_OWNER);

                                if (ItemId.Contains(ItemIdTemp) && ItemGuidOwner == Guid)
                                {
                                    var ItemDurability = GetDescriptor<int>(o.GetBaseAddress,
                                                                            (uint)
                                                                            Descriptors.ItemFields.ITEM_FIELD_DURABILITY);
                                    var ItemMaxDurability = GetDescriptor<int>(o.GetBaseAddress,
                                                                               (uint)
                                                                               Descriptors.ItemFields.
                                                                                   ITEM_FIELD_MAXDURABILITY);

                                    if (ItemDurability > 0 && ItemMaxDurability > 0)
                                    {
                                        durabilitys += ItemDurability;
                                        maxDurabilitys += ItemMaxDurability;
                                    }

                                }
                            }
                            catch (Exception e)
                            {
                                Logging.WriteError("WoWPlayer > GetDurability#1: " + e);
                            }
                        }
                    }
                    try
                    {
                        return durabilitys * 100 / maxDurabilitys;
                    }
                    catch
                    {
                        return 100;
                    }
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > GetDurability#2: " + e);
                }
                return 100;
            }

        }
        public float Rotation
        {
            get
            {
                try
                { return Memory.WowMemory.Memory.ReadFloat(BaseAddress + (uint)Addresses.UnitField.UNIT_FIELD_R); }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > Rotation get: " + e);
                }
                return 0;
            }
            set
            {
                try
                { Memory.WowMemory.Memory.WriteFloat(BaseAddress + (uint)Addresses.UnitField.UNIT_FIELD_R, value); }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > : Rotation set" + e);
                }
            }
        }
        public float Pitch
        {
            get
            {
                try
                { return Memory.WowMemory.Memory.ReadFloat(BaseAddress + (uint)Addresses.UnitField.UNIT_FIELD_H); }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > Pitch get: " + e);
                }
                return 0;
            }
            set
            {
                try
                { Memory.WowMemory.Memory.WriteFloat(BaseAddress + (uint)Addresses.UnitField.UNIT_FIELD_H, value); }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > Pitch set: " + e);
                }
            }
        }
        public int Experience
        {
            get
            {
                try
                { return GetDescriptor<int>(Descriptors.PlayerFields.PLAYER_XP); }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > Experience: " + e);
                }
                return 0;
            }
        }
        public int ExperienceMax
        {
            get
            {
                try
                { return GetDescriptor<int>(Descriptors.PlayerFields.PLAYER_NEXT_LEVEL_XP); }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > ExperienceMax: " + e);
                }
                return 0;
            }
        }
        public Point PositionCorpse
        {
            get
            {
                try
                { return new Point(Memory.WowMemory.Memory.ReadFloat(Memory.WowProcess.WowModule + (uint)Addresses.CorpsePlayer.X), Memory.WowMemory.Memory.ReadFloat(Memory.WowProcess.WowModule + (uint)Addresses.CorpsePlayer.Y), Memory.WowMemory.Memory.ReadFloat(Memory.WowProcess.WowModule + (uint)Addresses.CorpsePlayer.Z)); }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > PositionCorpse: " + e);
                }
                return new Point();
            }
        }
        public bool IsDeadMe
        {
            get
            {
                try
                { return (Health <= 0 || Health == 0.01 || Health == 1) || (PositionCorpse.X != 0 && PositionCorpse.Y != 0); }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > IsDeadMe: " + e);
                }
                return false;
            }
        }
        public new bool IsDead
        {
            get
            {
                try
                {
                    if (Guid == ObjectManager.Me.Guid) return IsDeadMe;
                    return (Health <= 0 || Health == 0.01 || GetDescriptor<Int32>((uint)Descriptors.UnitFields.UNIT_DYNAMIC_FLAGS) == 0x20);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > IsDead: " + e);
                }
                return false;
            }
        }
        public bool forceIsCast { set; get; }
        public new bool IsCast
        {
            get
            {
                try
                { return (Memory.WowMemory.Memory.ReadInt(GetBaseAddress + (uint)Addresses.UnitField.CastingSpellID) > 0 || Memory.WowMemory.Memory.ReadInt(GetBaseAddress + (uint)Addresses.UnitField.ChannelSpellID) > 0 || forceIsCast); }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > IsCast: " + e);
                }
                return false;
            }
        }
        public TrackCreatureFlags MeCreatureTrack
        {
            get
            {
                try
                {
                    return (TrackCreatureFlags)GetDescriptor<uint>(Descriptors.PlayerFields.PLAYER_TRACK_CREATURES);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > MeCreatureTrack: " + e);
                }
                return 0;
            }
            set
            {
                try
                {
                    uint descriptorsArray = Memory.WowMemory.Memory.ReadUInt(GetBaseAddress + Descriptors.startDescriptors);
                    uint addressGD = descriptorsArray + ((uint)Descriptors.PlayerFields.PLAYER_TRACK_CREATURES * Descriptors.multiplicator);
                    Memory.WowMemory.Memory.WriteUInt(addressGD, (uint)value);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > MeCreatureTrack: " + e);
                }
            }
        }

        public uint GetEquipedItem
        {
            get
            {
                try
                {
                    return ObjectManager.Me.GetDescriptor<uint>(Descriptors.PlayerFields.PLAYER_VISIBLE_ITEM_16_ENTRYID);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > GetEquipedItem: " + e);
                }
                return 0;
            }
        }

        public TrackObjectFlags MeObjectTrack
        {
            get
            {
                try
                {
                    return (TrackObjectFlags)GetDescriptor<uint>(Descriptors.PlayerFields.PLAYER_TRACK_RESOURCES);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > MeObjectTrack get: " + e);
                }
                return 0;
            }
            set
            {
                try
                {
                    uint descriptorsArray = Memory.WowMemory.Memory.ReadUInt(GetBaseAddress + Descriptors.startDescriptors);
                    uint addressGD = descriptorsArray + ((uint)Descriptors.PlayerFields.PLAYER_TRACK_RESOURCES * Descriptors.multiplicator);
                    Memory.WowMemory.Memory.WriteUInt(addressGD, (uint)value);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > MeObjectTrack set: " + e);
                }
            }
        }

        public T GetDescriptor<T>(Descriptors.PlayerFields field) where T : struct
        {
            try
            {
                return GetDescriptor<T>((uint)field);
            }
            catch (Exception e)
            {
                Logging.WriteError("WoWPlayer > GetDescriptor<T>(Descriptors.PlayerFields field): " + e);
            }
            return default(T);
        }
    }
}
