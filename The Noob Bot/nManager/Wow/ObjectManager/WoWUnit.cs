using System;
using System.Threading;
using System.Collections.Generic;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Wow.Patchables;
using Math = System.Math;

namespace nManager.Wow.ObjectManager
{
    public class WoWUnit : WoWObject
    {
        public WoWUnit(uint address)
            : base(address)
        {
        }

        public override Point Position
        {
            get
            {
                try
                {

                    if (InTransport)
                    {
                        var t = new WoWUnit(ObjectManager.GetObjectByGuid(TransportGuid).GetBaseAddress);
                        if (t.IsValid && t.IsAlive)
                        {
                            return t.Position;
                        }
                    }

                    var ret =
                        new Point(
                            Memory.WowMemory.Memory.ReadFloat(BaseAddress +
                                                                  (uint)Addresses.UnitField.UNIT_FIELD_X),
                            Memory.WowMemory.Memory.ReadFloat(BaseAddress +
                                                                  (uint)Addresses.UnitField.UNIT_FIELD_Y),
                            Memory.WowMemory.Memory.ReadFloat(BaseAddress +
                                                                  (uint)Addresses.UnitField.UNIT_FIELD_Z));

                    if (Guid == ObjectManager.Me.Guid)
                    {
                        if (Usefuls.IsFlying)
                            ret.Type = "Flying";
                        if (Usefuls.IsSwimming)
                            ret.Type = "Swimming";
                    }

                    return ret;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > Position: " + e);
                    return new Point(0, 0, 0);
                }
            }
        }

        public int Health
        {
            get
            {
                try { return GetDescriptor<int>(Descriptors.UnitFields.UNIT_FIELD_HEALTH); }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > Health: " + e); return 0;
                }
            }
        }
        public int MaxHealth
        {
            get
            {
                try { return GetDescriptor<int>(Descriptors.UnitFields.UNIT_FIELD_MAXHEALTH); }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > MaxHealth: " + e); return 0;
                }
            }
        }
        public double HealthPercent
        {
            get
            {
                try
                {
                    var p = (int)((Health * 100) / (double)MaxHealth);
                    if (p < 0 || p > 100)
                    { return 0; }
                    return p;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > HealthPercent: " + e); return 0;
                }
            }
        }

        public int AggroDistance
        {
            get
            {
                try
                {
                    int num = 20;
                    if (ObjectManager.Me.Level > Level)
                    {
                        num -= Math.Abs((int)(ObjectManager.Me.Level - Level));
                    }
                    if (ObjectManager.Me.Level < Level)
                    {
                        num += Math.Abs((int)(ObjectManager.Me.Level - Level));
                    }
                    if (num < 5)
                    {
                        num = 5;
                    }
                    return (num + 3);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > AggroDistance: " + e); return 20;
                }
            }
        }

        public string CreatureTypeTarget
        {
            get
            {
                lock (this)
                {
                    try { string randomStringResult = Others.GetRandomString(Others.Random(4, 10)); Lua.LuaDoString(randomStringResult + " = UnitCreatureType(\"target\")"); return Lua.GetLocalizedText(randomStringResult); }
                    catch (Exception e)
                    {
                        Logging.WriteError("WoWUnit > CreatureTypeTarget: " + e); return "";
                    }
                }
            }
        }

        public WoWFactionTemplate FactionTemplate
        {
            get
            {
                try
                {
                    if (Faction == 0)
                    {
                        return null;
                    }
                    return WoWFactionTemplate.FromId(Faction);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > FactionTemplate: " + e); return null;
                }
            }
        }

        public int MaximumSoulShards
        {
            get
            {
                try { return GetDescriptor<int>(Descriptors.UnitFields.UNIT_FIELD_POWER2); }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > MaximumSoulShards: " + e); return 0;
                }
            }
        }

        public int SoulShards
        {
            get
            {
                try { return GetDescriptor<int>(Descriptors.UnitFields.UNIT_FIELD_POWER2); }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > SoulShards: " + e); return 0;
                }
            }
        }

        public int MaxHolyPower
        {
            get
            {
                try { return GetDescriptor<int>(Descriptors.UnitFields.UNIT_FIELD_MAXPOWER2); }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > MaxHolyPower: " + e); return 0;
                }
            }
        }

        public int HolyPower
        {
            get
            {
                try { return GetDescriptor<int>(Descriptors.UnitFields.UNIT_FIELD_POWER2); }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > HolyPower: " + e); return 0;
                }
            }
        }

        public uint Faction
        {
            get
            {
                try { return GetDescriptor<uint>(Descriptors.UnitFields.UNIT_FIELD_FACTIONTEMPLATE); }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > Faction: " + e); return 0;
                }
            }
        }
        public uint DisplayId
        {
            get
            {
                try
                {
                    return GetDescriptor<uint>(Descriptors.UnitFields.UNIT_FIELD_DISPLAYID);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > DisplayId: " + e);
                    return 0;
                }
            }
        }
        Point _lastPosMove = new Point();
        public bool GetMove
        {
            get
            {
                try
                {
                    bool bResult = false;

                    _lastPosMove = ObjectManager.Me.Position;
                    Thread.Sleep(50);
                    if (Math.Round(_lastPosMove.X, 1) != Math.Round(ObjectManager.Me.Position.X, 1) || Math.Round(_lastPosMove.Z, 1) != Math.Round(ObjectManager.Me.Position.Z, 1) || Math.Round(_lastPosMove.Y, 1) != Math.Round(ObjectManager.Me.Position.Y, 1))
                        bResult = true;
                    if (!bResult)
                    {
                        Thread.Sleep(30);
                        if (Math.Round(_lastPosMove.X, 1) != Math.Round(ObjectManager.Me.Position.X, 1) || Math.Round(_lastPosMove.Z, 1) != Math.Round(ObjectManager.Me.Position.Z, 1) || Math.Round(_lastPosMove.Y, 1) != Math.Round(ObjectManager.Me.Position.Y, 1))
                            bResult = true;
                    }
                    return bResult;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > GetMove: " + e);
                    return true;
                }
            }
        }
        public float SpeedMoving
        {
            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadFloat(BaseAddress + (uint)Addresses.UnitField.UNIT_SPEED);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > SpeedMoving: " + e); return 0;
                }

            }
        }
        public override float GetDistance
        {
            get
            {
                try { return Position.DistanceTo(ObjectManager.Me.Position); }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > GetDistance: " + e); return 0;
                }
            }
        }
        public float GetDistance2D
        {
            get
            {
                try { return Position.DistanceTo2D(ObjectManager.Me.Position); }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > GetDistance2D: " + e); return 0;
                }
            }
        }
        public bool IsAlive
        {
            get
            {
                try { return (Health > 0); }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsAlive: " + e); return false;
                }
            }
        }
        public bool IsDead
        {
            get
            {
                try { return (Health <= 0 || Health == 0.01 || GetDescriptor<Int32>(Descriptors.UnitFields.UNIT_DYNAMIC_FLAGS) == 0x20) || (Health == 1 && GetMove); }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsDead: " + e); return false;
                }
            }
        }
        public bool IsLootable
        {
            get
            {
                try
                {
                    var dynFlags = GetDescriptor<Int32>(Descriptors.UnitFields.UNIT_DYNAMIC_FLAGS);
                    if (dynFlags == 13 || dynFlags == 1)
                        return true;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsLootable: " + e); return false;
                }
                return false;
            }
        }
        public bool IsTagged
        {
            get
            {
                try
                {
                    var dynFlags = GetDescriptor<Int32>(Descriptors.UnitFields.UNIT_DYNAMIC_FLAGS);
                    if (dynFlags == 4)
                        return true;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsTagged: " + e); return false;
                }
                return false;
            }
        }
        public bool IsTaggedByYou
        {
            get
            {
                try
                {
                    var dynFlags = GetDescriptor<Int32>(Descriptors.UnitFields.UNIT_DYNAMIC_FLAGS);
                    if (dynFlags == 8 || dynFlags == 13 || dynFlags == 1 || dynFlags == 12)
                        return true;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsTaggedByYou: " + e); return false;
                }
                return false;
            }
        }
        public bool IsTargetingMe
        {
            get
            {
                try
                {
                    if (Target == ObjectManager.Me.Guid)
                        return true;
                    return false;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsTargetingMe: " + e); return false;
                }
            }
        }
        public UInt64 Target
        {
            get
            {
                try { return GetDescriptor<ulong>(Descriptors.UnitFields.UNIT_FIELD_TARGET); }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > : Target get" + e); return 0;
                }
            }
            set
            {
                try
                {
                    Memory.WowMemory.Memory.WriteUInt64(Memory.WowMemory.Memory.ReadUInt(GetBaseAddress + Descriptors.startDescriptors) + (uint)Descriptors.UnitFields.UNIT_FIELD_TARGET * Descriptors.multiplicator, value);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > Target set: " + e);
                }
            }
        }
        public uint Level
        {
            get
            {
                try { return GetDescriptor<uint>(Descriptors.UnitFields.UNIT_FIELD_LEVEL); }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > Level: " + e); return 0;
                }
            }
        }
        public override string Name
        {
            get
            {
                try
                {
                    if (BaseAddress == ObjectManager.Me.GetBaseAddress)
                    {
                        return
                                Memory.WowMemory.Memory.ReadUTF8String(Memory.WowProcess.WowModule +
                                                                            (uint)Addresses.Player.playerName);
                    }
                    if (Type == Enums.WoWObjectType.Player)
                    {
                        return Usefuls.GetPlayerName(Guid);
                    }

                    return
                        Memory.WowMemory.Memory.ReadUTF8String(
                            Memory.WowMemory.Memory.ReadUInt(
                                Memory.WowMemory.Memory.ReadUInt(BaseAddress +
                                                                 (uint)
                                                                 Addresses.UnitField.unitName1) +
                                (uint)Addresses.UnitField.unitName2));
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > Name: " + e);
                    return "";
                }
            }
        }
        public bool Skinnable
        {
            get
            {
                try
                {
                    var flags = GetDescriptor<Int32>(Descriptors.UnitFields.UNIT_FIELD_FLAGS);
                    return Convert.ToBoolean(flags & 0x4000000);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > Skinnable: " + e); return false;
                }
            }
        }
        public bool IsNpcSpiritHealer
        {
            get
            {
                try
                {
                    var flags = GetDescriptor<Int32>(Descriptors.UnitFields.UNIT_NPC_FLAGS);
                    return Convert.ToBoolean(flags & 0x00004000);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsNpcSpiritHealer: " + e); return false;
                }
            }
        }
        public bool IsNpcRepair
        {
            get
            {
                try
                {
                    var flags = GetDescriptor<Int32>(Descriptors.UnitFields.UNIT_NPC_FLAGS);
                    return Convert.ToBoolean(flags & 0x00001000);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsNpcRepair: " + e); return false;
                }
            }
        }
        public bool IsNpcVendor
        {
            get
            {
                try
                {
                    var flags = GetDescriptor<Int32>(Descriptors.UnitFields.UNIT_NPC_FLAGS);
                    return Convert.ToBoolean(flags & 0x00000080);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsNpcVendor: " + e); return false;
                }
            }
        }
        public bool IsNpcInnkeeper
        {
            get
            {
                try
                {
                    var flags = GetDescriptor<Int32>(Descriptors.UnitFields.UNIT_NPC_FLAGS);
                    return Convert.ToBoolean(flags & 0x00010000);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsNpcInnkeeper: " + e); return false;
                }
            }
        }
        public bool IsNpcVendorFood
        {
            get
            {
                try
                {
                    var flags = GetDescriptor<Int32>(Descriptors.UnitFields.UNIT_NPC_FLAGS);
                    return Convert.ToBoolean(flags & 0x00000200);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsNpcVendorFood: " + e);
                    return false;
                }
            }
        }
        public bool IsNpcTrainer
        {
            get
            {
                try
                {
                    var flags = GetDescriptor<Int32>(Descriptors.UnitFields.UNIT_NPC_FLAGS);
                    return Convert.ToBoolean(flags & 0x00000020) || Convert.ToBoolean(flags & 0x00000040);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsNpcTrainer: " + e);
                    return false;
                }
            }
        }
        public ulong SummonedBy
        {
            get
            {
                try { return GetDescriptor<ulong>(Descriptors.UnitFields.UNIT_FIELD_SUMMONEDBY); }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > SummonedBy: " + e); return 0;
                }
            }
        }
        public bool AutoAttack
        {
            get
            {
                try
                {
                    var flags = GetDescriptor<Int32>(Descriptors.UnitFields.UNIT_FIELD_FLAGS);
                    return Convert.ToBoolean(flags & 0x0000800);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > AutoAttack: " + e); return false;
                }
            }
        }
        public bool InCombat
        {
            get
            {
                try
                {
                    if (BaseAddress == ObjectManager.Me.BaseAddress)
                    {
                        if (InTransport)
                        {
                            if (Usefuls.PlayerUsingVehicle)
                            {
                                return false;
                            }
                        }
                    }
                    var flags = GetDescriptor<Int32>(Descriptors.UnitFields.UNIT_FIELD_FLAGS);
                    return Convert.ToBoolean(flags & 0x0080000) && !IsDead;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > InCombat: " + e); return false;
                }
            }
        }
        public bool InCombatWithMe
        {
            get
            {
                try { return InCombat && IsTargetingMe && !IsDead; }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > InCombatWithMe: " + e); return false;
                }
            }
        }
        public bool IsCast
        {
            get
            {
                try { return (Memory.WowMemory.Memory.ReadInt(GetBaseAddress + (uint)Addresses.UnitField.CastingSpellID) > 0 || Memory.WowMemory.Memory.ReadInt(GetBaseAddress + (uint)Addresses.UnitField.ChannelSpellID) > 0); }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsCast: " + e); return false;
                }
            }
        }
        public bool IsMounted
        {
            get
            {
                try { return GetDescriptor<int>(Descriptors.UnitFields.UNIT_FIELD_MOUNTDISPLAYID) > 0 || HaveBuff(SpellManager.MountDruidId()) || InTransport; }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsMounted: " + e); return false;
                }
            }
        }
        public int MountDisplayId
        {
            get
            {
                try
                { return GetDescriptor<int>(Descriptors.UnitFields.UNIT_FIELD_MOUNTDISPLAYID); }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > MountDisplayId: " + e); return 0;
                }
            }
        }
        public bool InPVP
        {
            get
            {
                try
                {
                    var flags = GetDescriptor<Int32>(Descriptors.UnitFields.UNIT_FIELD_FLAGS);
                    return Convert.ToBoolean(flags & 0x1000);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > InPVP: " + e); return false;
                }
            }
        }
        public bool PVP
        {
            get
            {
                try
                {
                    var flags = GetDescriptor<Int32>(Descriptors.UnitFields.UNIT_FIELD_FLAGS);
                    return Convert.ToBoolean(flags & 0x0000008);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > PVP: " + e); return false;
                }
            }
        }

        public ulong TransportGuid
        {

            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadUInt64(GetBaseAddress + (uint)Addresses.UnitField.TransportGUID);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > TransportGuid: " + e); return 0;
                }
            }
        }

        public bool InTransport
        {

            get
            {
                try
                {
                    return (TransportGuid > 0);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > InTransport: " + e); return false;
                }
            }
        }

        public int BuffStack(List<UInt32> idBuffs)
        {
            try
            {
                return BuffManager.AuraStack(BaseAddress, idBuffs);
            }
            catch (Exception e)
            {
                Logging.WriteError("WoWUnit > BuffStack(List<UInt32> idBuffs): " + e); return 0;
            }
        }
        public int BuffStack(UInt32 idBuff)
        {
            try
            {
                List<UInt32> idBuffs = new List<UInt32>();
                idBuffs.Add(idBuff);
                return BuffManager.AuraStack(BaseAddress, idBuffs);
            }
            catch (Exception e)
            {
                Logging.WriteError("WoWUnit > BuffStack(UInt32 idBuffs): " + e); return 0;
            }

        }

        public int GetHolyPowerStack()
        {
            try
            {
                string randomStringResult = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(randomStringResult + " = UnitPower(\"player\", 9)");
                string sResult = Lua.GetLocalizedText(randomStringResult);
                return int.Parse(sResult);
            }
            catch (Exception e)
            {
                Logging.WriteError("WoWUnit > GetHolyPowerStack(): " + e); return 0;
            }
        }

        public bool HaveBuff(List<UInt32> idBuffs)
        {
            try
            {
                return BuffManager.HaveBuff(BaseAddress, idBuffs);
            }
            catch (Exception e)
            {
                Logging.WriteError("WoWUnit > HaveBuff(List<UInt32> idBuffs): " + e); return false;
            }
        }
        public bool HaveBuff(UInt32 idBuffs)
        {
            try
            {
                return BuffManager.HaveBuff(BaseAddress, idBuffs);
            }
            catch (Exception e)
            {
                Logging.WriteError("WoWUnit > HaveBuff(UInt32 idBuffs): " + e); return false;
            }
        }
        public Enums.Reaction Reaction
        {
            get
            {
                try
                { return UnitRelation.GetReaction(ObjectManager.Me.Faction, Faction); }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > Reaction: " + e); return Enums.Reaction.Neutral;
                }
            }
        }
        public bool IsTracked
        {

            get
            {
                try
                {
                    return ((GetDescriptor<uint>(Descriptors.UnitFields.UNIT_DYNAMIC_FLAGS) & 2) != 0);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsTracked get: " + e); return false;
                }
            }
            set
            {
                try
                {
                    var descriptor = GetDescriptor<uint>(Descriptors.UnitFields.UNIT_DYNAMIC_FLAGS);
                    long t;
                    if (value)
                    {
                        t = descriptor | 2;
                    }
                    else
                    {
                        t = descriptor & -3L;
                    }

                    var descriptorsArray = Memory.WowMemory.Memory.ReadUInt(GetBaseAddress + Descriptors.startDescriptors);
                    var addressGD = descriptorsArray + ((uint)Descriptors.UnitFields.UNIT_DYNAMIC_FLAGS * Descriptors.multiplicator);
                    Memory.WowMemory.Memory.WriteInt64(addressGD, t);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsTracked set: " + e);
                }
            }
        }

        public T GetDescriptor<T>(Descriptors.UnitFields field) where T : struct
        {
            try
            {
                return GetDescriptor<T>((uint)field);
            }
            catch (Exception e)
            {
                Logging.WriteError("WoWUnit > GetDescriptor<T>(Descriptors.UnitFields field): " + e);
                return default(T);
            }
        }
    }

}
