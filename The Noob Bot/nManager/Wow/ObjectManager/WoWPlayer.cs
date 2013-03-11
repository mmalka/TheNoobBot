using System;
using System.Collections.Generic;
using System.Linq;
using nManager.Helpful;
using nManager.Wow.Helpers;
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
                {
                    uint descriptorsArray = Memory.WowMemory.Memory.ReadUInt(BaseAddress + Descriptors.startDescriptors);
                    uint displayPower = descriptorsArray +
                                        ((uint) Descriptors.UnitFields.DisplayPower*Descriptors.multiplicator);
                    return (WoWClass) Memory.WowMemory.Memory.ReadBytes(displayPower, 4)[1];
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > WowClass: " + e);
                    return WoWClass.None;
                }
            }
        }

        public WoWRace WowRace
        {
            get
            {
                try
                {
                    uint descriptorsArray = Memory.WowMemory.Memory.ReadUInt(BaseAddress + Descriptors.startDescriptors);
                    uint displayPower = descriptorsArray +
                                        ((uint) Descriptors.UnitFields.DisplayPower*Descriptors.multiplicator);
                    return (WoWRace) Memory.WowMemory.Memory.ReadBytes(displayPower, 4)[0];
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > WowRace: " + e);
                    return WoWRace.None;
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
                        case "PandarenAlliance":
                            return "Alliance";
                        case "Orc":
                        case "Undead":
                        case "Tauren":
                        case "Troll":
                        case "Blood Elf":
                        case "Goblin":
                        case "PandarenHorde":
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
                    if (faction.Equals((int) PlayerFactions.Human))
                        return "Human";
                    if (faction.Equals((int) PlayerFactions.BloodElf))
                        return "Blood Elf";
                    if (faction.Equals((int) PlayerFactions.Dwarf))
                        return "Dwarf";
                    if (faction.Equals((int) PlayerFactions.Gnome))
                        return "Gnome";
                    if (faction.Equals((int) PlayerFactions.NightElf))
                        return "Night Elf";
                    if (faction.Equals((int) PlayerFactions.Orc))
                        return "Orc";
                    if (faction.Equals((int) PlayerFactions.Tauren))
                        return "Tauren";
                    if (faction.Equals((int) PlayerFactions.Troll))
                        return "Troll";
                    if (faction.Equals((int) PlayerFactions.Undead))
                        return "Undead";
                    if (faction.Equals((int) PlayerFactions.Draenei))
                        return "Draenei";
                    if (faction.Equals((int) PlayerFactions.Goblin))
                        return "Goblin";
                    if (faction.Equals((int) PlayerFactions.Worgen))
                        return "Worgen";
                    if (faction.Equals((int) PlayerFactions.PandarenNeutral))
                        return "PandarenNeutral";
                    if (faction.Equals((int) PlayerFactions.PandarenHorde))
                        return "PandarenHorde";
                    if (faction.Equals((int) PlayerFactions.PandarenAlliance))
                        return "PandarenAlliance";
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > PlayerRace: " + e);
                }
                return "Unknown";
            }
        }

        public int ComboPoint
        {
            get
            {
                try
                {
                    int val =
                        Memory.WowMemory.Memory.ReadByte(Memory.WowProcess.WowModule +
                                                         (uint) Addresses.Player.PlayerComboPoint);
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
                    var flags = GetDescriptor<Int32>((uint) Descriptors.UnitFields.Flags);
                    try
                    {
                        if (GetBaseAddress == ObjectManager.Me.GetBaseAddress)
                        {
                            if (IsDeadMe)
                                isdead = true;
                            if (ObjectManager.Pet.GetBaseAddress > 0)
                                if (ObjectManager.GetNumberAttackPlayer() > 0 && ObjectManager.Pet.InCombat &&
                                    !ObjectManager.Pet.IsDead)
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

                    var itemId = new List<uint>
                        {
                            (uint) EquippedItems.GetEquippedItem((int) WoWInventorySlot.INVTYPE_AMMO).Entry,
                            (uint) EquippedItems.GetEquippedItem((int) WoWInventorySlot.INVTYPE_HEAD).Entry,
                            (uint) EquippedItems.GetEquippedItem((int) WoWInventorySlot.INVTYPE_NECK).Entry,
                            (uint) EquippedItems.GetEquippedItem((int) WoWInventorySlot.INVTYPE_SHOULDER).Entry,
                            (uint) EquippedItems.GetEquippedItem((int) WoWInventorySlot.INVTYPE_BODY).Entry,
                            (uint) EquippedItems.GetEquippedItem((int) WoWInventorySlot.INVTYPE_CHEST).Entry,
                            (uint) EquippedItems.GetEquippedItem((int) WoWInventorySlot.INVTYPE_WAIST).Entry,
                            (uint) EquippedItems.GetEquippedItem((int) WoWInventorySlot.INVTYPE_LEGS).Entry,
                            (uint) EquippedItems.GetEquippedItem((int) WoWInventorySlot.INVTYPE_FEET).Entry,
                            (uint) EquippedItems.GetEquippedItem((int) WoWInventorySlot.INVTYPE_WRIST).Entry,
                            (uint) EquippedItems.GetEquippedItem((int) WoWInventorySlot.INVTYPE_HAND).Entry,
                            (uint) EquippedItems.GetEquippedItem((int) WoWInventorySlot.INVTYPE_FINGER).Entry,
                            (uint) EquippedItems.GetEquippedItem((int) WoWInventorySlot.INVTYPE_FINGER + 1).Entry,
                            (uint) EquippedItems.GetEquippedItem((int) WoWInventorySlot.INVTYPE_TRINKET).Entry,
                            (uint) EquippedItems.GetEquippedItem((int) WoWInventorySlot.INVTYPE_TRINKET + 1).Entry,
                            (uint) EquippedItems.GetEquippedItem((int) WoWInventorySlot.INVTYPE_CLOAK).Entry,
                            (uint) EquippedItems.GetEquippedItem((int) WoWInventorySlot.INVTYPE_WEAPON).Entry,
                            (uint) EquippedItems.GetEquippedItem((int) WoWInventorySlot.INVTYPE_SHIELD).Entry,
                            (uint) EquippedItems.GetEquippedItem((int) WoWInventorySlot.INVTYPE_RANGED).Entry
                        };

                    WoWObject[] objects = ObjectManager.ObjectList.ToArray();

                    foreach (WoWObject o in objects.Where(o => o.Type == WoWObjectType.Item))
                    {
                        try
                        {
                            var itemIdTemp = GetDescriptor<uint>(o.GetBaseAddress,
                                                                 (uint) Descriptors.ObjectFields.Entry);
                            var itemGuidOwner = GetDescriptor<ulong>(o.GetBaseAddress,
                                                                     (uint)
                                                                     Descriptors.ItemFields.Owner);

                            if (!itemId.Contains(itemIdTemp) || itemGuidOwner != Guid) continue;
                            var itemDurability = GetDescriptor<int>(o.GetBaseAddress,
                                                                    (uint)
                                                                    Descriptors.ItemFields.Durability);
                            var itemMaxDurability = GetDescriptor<int>(o.GetBaseAddress,
                                                                       (uint)
                                                                       Descriptors.ItemFields.MaxDurability);
                            durabilitys += itemDurability;
                            maxDurabilitys += itemMaxDurability;
                        }
                        catch (Exception e)
                        {
                            Logging.WriteError("WoWPlayer > GetDurability#1: " + e);
                        }
                    }
                    return durabilitys*100/maxDurabilitys;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > GetDurability#2: " + e);
                }
                Logging.WriteError("Durability % finding had crashed, please report this issue, we are outputting 100% instead, it wont repair.");
                return 100;
            }
        }

        public float Rotation
        {
            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadFloat(BaseAddress + (uint) Addresses.UnitField.UNIT_FIELD_R);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > Rotation get: " + e);
                }
                return 0;
            }
            set
            {
                try
                {
                    Memory.WowMemory.Memory.WriteFloat(BaseAddress + (uint) Addresses.UnitField.UNIT_FIELD_R, value);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > : Rotation set" + e);
                }
            }
        }

        public int Experience
        {
            get
            {
                try
                {
                    return GetDescriptor<int>(Descriptors.PlayerFields.XP);
                }
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
                {
                    return GetDescriptor<int>(Descriptors.PlayerFields.NextLevelXP);
                }
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
                {
                    return
                        new Point(
                            Memory.WowMemory.Memory.ReadFloat(Memory.WowProcess.WowModule +
                                                              (uint) Addresses.CorpsePlayer.X),
                            Memory.WowMemory.Memory.ReadFloat(Memory.WowProcess.WowModule +
                                                              (uint) Addresses.CorpsePlayer.Y),
                            Memory.WowMemory.Memory.ReadFloat(Memory.WowProcess.WowModule +
                                                              (uint) Addresses.CorpsePlayer.Z));
                }
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
                {
                    return (Health <= 0 || Health == 0.01 || Health == 1) ||
                           (PositionCorpse.X != 0 && PositionCorpse.Y != 0);
                }
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
                    return (Health <= 0 || Health == 0.01 ||
                            GetDescriptor<Int32>((uint) Descriptors.UnitFields.DynamicFlags) == 0x20);
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
                {
                    return
                        (Memory.WowMemory.Memory.ReadInt(GetBaseAddress + (uint) Addresses.UnitField.CastingSpellID) > 0 ||
                         Memory.WowMemory.Memory.ReadInt(GetBaseAddress + (uint) Addresses.UnitField.ChannelSpellID) > 0 ||
                         forceIsCast);
                }
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
                    return (TrackCreatureFlags) GetDescriptor<uint>(Descriptors.PlayerFields.TrackCreatureMask);
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
                    uint descriptorsArray =
                        Memory.WowMemory.Memory.ReadUInt(GetBaseAddress + Descriptors.startDescriptors);
                    uint addressGD = descriptorsArray +
                                     ((uint) Descriptors.PlayerFields.TrackCreatureMask*Descriptors.multiplicator);
                    Memory.WowMemory.Memory.WriteUInt(addressGD, (uint) value);
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
                    return ObjectManager.Me.GetDescriptor<uint>(Descriptors.PlayerFields.VisibleItems + 15*2);
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
                    return (TrackObjectFlags) GetDescriptor<uint>(Descriptors.PlayerFields.TrackResourceMask);
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
                    uint descriptorsArray =
                        Memory.WowMemory.Memory.ReadUInt(GetBaseAddress + Descriptors.startDescriptors);
                    uint addressGD = descriptorsArray +
                                     ((uint) Descriptors.PlayerFields.TrackResourceMask*Descriptors.multiplicator);
                    Memory.WowMemory.Memory.WriteUInt(addressGD, (uint) value);
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
                return GetDescriptor<T>((uint) field);
            }
            catch (Exception e)
            {
                Logging.WriteError("WoWPlayer > GetDescriptor<T>(Descriptors.PlayerFields field): " + e);
            }
            return default(T);
        }
    }
}