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

        private byte GetCharByte(uint index)
        {
            uint descriptorsArrayOfBytes = Memory.WowMemory.Memory.ReadUInt(BaseAddress + Descriptors.StartDescriptors);
            uint getBytes = descriptorsArrayOfBytes + ((uint) Descriptors.UnitFields.Sex*Descriptors.Multiplicator);
            byte[] Bytes = Memory.WowMemory.Memory.ReadBytes(getBytes, 4);
            return Bytes[index];
        }

        public WoWRace WowRace
        {
            get { return (WoWRace) GetCharByte(0); }
        }

        public new WoWClass WowClass
        {
            get { return (WoWClass) GetCharByte(1); }
        }

        public WoWGender WowGender
        {
            get { return (WoWGender) GetCharByte(3); }
        }

        public WoWSpecialization WowSpecialization(bool doOutput = false)
        {
            try
            {
                string specInfo = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(
                    "if GetSpecialization() ~= nil and GetSpecializationInfo(GetSpecialization()) ~= nil then id,name,description,icon,role,primary = GetSpecializationInfo(GetSpecialization()) " +
                    specInfo + " = id .. \"^\" .. name .. \"^\" .. role .. \"^\" .. primary else " + specInfo + " = 0 end");
                string[] specInfos = Lua.GetLocalizedText(specInfo).Split('^');
                if (specInfos.Count() != 4)
                {
                    if (doOutput)
                        Logging.WriteDebug("WoW Specialization not found");
                    return WoWSpecialization.None;
                }
                if (doOutput)
                    Logging.WriteDebug("WoW Specialization found: " + WowClass + " " + specInfos[1] + ", role: " + specInfos[2]);
                WoWSpecialization rWoWSpecialization = (WoWSpecialization) Others.ToInt32(specInfos[0]);
                return rWoWSpecialization;
            }
            catch (Exception e)
            {
                Logging.WriteError("WoWPlayer > WoWSpecialization: " + e);
            }
            return WoWSpecialization.None;
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
                        case "PandarenNeutral":
                            return "Neutral";
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

        public bool InCombatBlizzard
        {
            get
            {
                try
                {
                    bool petAttack = false;
                    bool isdead = false;
                    try
                    {
                        if (GetBaseAddress == ObjectManager.Me.GetBaseAddress)
                        {
                            if (IsDeadMe)
                                isdead = true;
                            if (ObjectManager.Pet.GetBaseAddress > 0)
                                if (ObjectManager.Pet.InCombat && !ObjectManager.Pet.IsDead)
                                    petAttack = true;

                            if (!isdead && (GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags).HasFlag(UnitFlags.InCombat)))
                                return true;
                            return petAttack;
                        }
                    }
                    catch (Exception e)
                    {
                        Logging.WriteError("WoWPlayer > InCombat#1: " + e);
                    }
                    return (GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags).HasFlag(UnitFlags.InCombat) || petAttack) && !isdead;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > InCombat#2: " + e);
                }
                return false;
            }
        }

        public bool InInevitableCombat
        {
            get
            {
                return ObjectManager.Me.InCombat && !(ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying)) && !CustomProfile.GetSetIgnoreFight &&
                       !Quest.GetSetIgnoreFight && !Quest.GetSetIgnoreAllFight;
            }
        }

        public new bool InCombat
        {
            get { return InCombatBlizzard && ObjectManager.GetNumberAttackPlayer() > 0; }
        }

        public int GetDurability
        {
            get
            {
                try
                {
                    int durabilitys = 0;
                    int maxDurabilitys = 0;
                    int brokenItems = 0;
                    WoWObject[] objects = ObjectManager.ObjectList.ToArray();
                    foreach (WoWObject o in objects.Where(o => o.Type == WoWObjectType.Item))
                    {
                        try
                        {
                            UInt128 itemGuidOwner = GetDescriptor<UInt128>(o.GetBaseAddress, (uint) Descriptors.ItemFields.Owner);
                            if (!EquippedItems.IsEquippedItemByGuid(o.Guid) || itemGuidOwner != Guid)
                                continue;
                            int itemMaxDurability = GetDescriptor<int>(o.GetBaseAddress, (uint) Descriptors.ItemFields.MaxDurability);
                            if (itemMaxDurability == 0)
                                continue;
                            int itemDurability = GetDescriptor<int>(o.GetBaseAddress, (uint) Descriptors.ItemFields.Durability);
                            if (itemDurability == 0)
                                brokenItems++;
                            durabilitys += itemDurability;
                            maxDurabilitys += itemMaxDurability;
                        }
                        catch (Exception e)
                        {
                            Logging.WriteError("WoWPlayer > GetDurability#1: " + e);
                        }
                    }
                    int ret = maxDurabilitys == 0 ? 100 : durabilitys*100/maxDurabilitys;
                    if (ret > 35 && brokenItems >= 2)
                        return 35; // force "low" if 2 items are broken completly
                    return ret;
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
                    /*for (int i = 0xE42824; i < 0xE6A824; i++)
                    {*/
                    var posCorpse =
                        new Point(
                            Memory.WowMemory.Memory.ReadFloat(Memory.WowProcess.WowModule + (uint) Addresses.CorpsePlayer.X), // Addresses.CorpsePlayer.X 
                            Memory.WowMemory.Memory.ReadFloat(Memory.WowProcess.WowModule + (uint) Addresses.CorpsePlayer.Y),
                            Memory.WowMemory.Memory.ReadFloat(Memory.WowProcess.WowModule + (uint) Addresses.CorpsePlayer.Z));

                    /*if (posCorpse.X > 1510.22 && posCorpse.Y < 60.93436 && posCorpse.X < 1530.22 && posCorpse.Y > 50.93436 && posCorpse.Z > 60 && posCorpse.Z < 70.09796)
                    {
                            Logging.Write("i =  " + i.ToString("x8") + ", pos: " + posCorpse);
                        }
                    }*/
                    return posCorpse;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > PositionCorpse: " + e);
                }
                return new Point();
            }
        }

        private static List<uint> _ghostSpells = new List<uint>();

        public bool IsDeadMe
        {
            get
            {
                try
                {
                    if (!IsValid)
                        return true;
                    if (_ghostSpells.Count <= 0) _ghostSpells = SpellManager.SpellListManager.SpellIdByName("Ghost");
                    if (HaveBuff(_ghostSpells))
                        return true;
                    if (Health <= 1 || GetDescriptor<UnitDynamicFlags>(Descriptors.ObjectFields.DynamicFlags).HasFlag(UnitDynamicFlags.Dead))
                        return true;
                    return (PositionCorpse.X != 0 && PositionCorpse.Y != 0);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > IsDeadMe: " + e);
                }
                return false;
            }
        }

        /*public new bool IsDead
        {
            get
            {
                try
                {
                    if (!IsValid)
                        return true;
                    if (Guid == ObjectManager.Me.Guid && IsDeadMe)
                        return true;
                    if (_ghostSpells.Count <= 0) _ghostSpells = SpellManager.SpellListManager.SpellIdByName("Ghost");
                    if (HaveBuff(_ghostSpells))
                        return true;
                    return Health <= 1 || GetDescriptor<UnitDynamicFlags>(Descriptors.ObjectFields.DynamicFlags).HasFlag(UnitDynamicFlags.Dead);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > IsDead: " + e);
                }
                return false;
            }
        }*/

        public bool ForceIsCasting { set; get; }

        public bool IsCasting
        {
            get
            {
                try
                {
                    if (ObjectManager.Me.Guid == Guid && ForceIsCasting)
                        return true;
                    return CurrentSpellIdCast > 0 || CurrentSpellIdChannel > 0;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > IsCasting: " + e);
                }
                return false;
            }
        }

        public new bool IsCast
        {
            get { return IsCasting; }
        }

        public bool IsMainHandTemporaryEnchanted
        {
            get
            {
                try
                {
                    string randomStringResult = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(randomStringResult + " = tostring(GetWeaponEnchantInfo())");
                    string sResult = Lua.GetLocalizedText(randomStringResult);
                    if (sResult == "true")
                        return true;
                    return false;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > IsMainHandTemporaryEnchanted: " + e);
                    return false;
                }
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
                        Memory.WowMemory.Memory.ReadUInt(GetBaseAddress + Descriptors.StartDescriptors);
                    uint addressGD = descriptorsArray +
                                     ((uint) Descriptors.PlayerFields.TrackCreatureMask*Descriptors.Multiplicator);
                    Memory.WowMemory.Memory.WriteUInt(addressGD, (uint) value);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > MeCreatureTrack: " + e);
                }
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
                        Memory.WowMemory.Memory.ReadUInt(GetBaseAddress + Descriptors.StartDescriptors);
                    uint addressGD = descriptorsArray +
                                     ((uint) Descriptors.PlayerFields.TrackResourceMask*Descriptors.Multiplicator);
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

        public void StopCast()
        {
            Lua.RunMacroText("/stopcasting");
        }
    }
}