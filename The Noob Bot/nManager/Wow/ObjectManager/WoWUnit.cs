using System;
using System.Threading;
using System.Collections.Generic;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Enums;
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

        public new bool IsValid
        {
            get
            {
                try
                {
                    return BaseAddress != 0 && ObjectManager.ObjectDictionary.ContainsKey(Guid);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsValid: " + e);
                    return false;
                }
            }
        }

        public override Point Position
        {
            get
            {
                try
                {
                    if (BaseAddress == 0)
                        return new Point(0, 0, 0);
                    /*uint i = (uint)(Addresses.UnitField.UNIT_FIELD_X + (-500));
                    while (true)
                    {
                        Logging.Write("New check, i=" + i);
                        var pt = new Point(
                            Memory.WowMemory.Memory.ReadFloat(BaseAddress + i),
                            Memory.WowMemory.Memory.ReadFloat(BaseAddress + i + 4),
                            Memory.WowMemory.Memory.ReadFloat(BaseAddress + i + 8));
                        if (pt.X > -12000 && pt.X < 12000 && pt.Y > -12000 && pt.Y < 12000 && pt.Z > -12000 && pt.Z < 12000)
                            if (!(pt.X.ToString().Contains("E") || pt.Y.ToString().Contains("E") || pt.Z.ToString().Contains("E")))
                                Logging.Write(pt.ToString());
                        i += 4;
                        if (i > 0x1200)
                            break;
                    }*/

                    Point ret =
                        new Point(
                            Memory.WowMemory.Memory.ReadFloat(BaseAddress +
                                                              (uint) Addresses.UnitField.UNIT_FIELD_X),
                            Memory.WowMemory.Memory.ReadFloat(BaseAddress +
                                                              (uint) Addresses.UnitField.UNIT_FIELD_Y),
                            Memory.WowMemory.Memory.ReadFloat(BaseAddress +
                                                              (uint) Addresses.UnitField.UNIT_FIELD_Z));
                    if (InTransport)
                    {
                        WoWObject t = new WoWObject(ObjectManager.GetObjectByGuid(TransportGuid).GetBaseAddress);
                        if (t.Type == WoWObjectType.GameObject)
                        {
                            var o = new WoWGameObject(t.GetBaseAddress);
                            if (o.IsValid)
                            {
                                var posAbsolute = ret.Transform(o.WorldMatrix);
                                Point pos = new Point(posAbsolute.X, posAbsolute.Y, posAbsolute.Z);
                                return pos;
                            }
                        }
                        else if (t.Type == WoWObjectType.Unit)
                        {
                            var u = new WoWUnit(t.GetBaseAddress);
                            if (u.IsValid && u.IsAlive)
                                return u.Position;
                        }
                        else if (t.Type == WoWObjectType.Player)
                        {
                            if (t.GetBaseAddress == ObjectManager.Me.GetBaseAddress)
                                return ObjectManager.Me.Position;
                            var p = new WoWPlayer(t.GetBaseAddress);
                            if (p.IsValid && p.IsAlive)
                                return p.Position;
                        }
                    }

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

        public float Orientation
        {
            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadFloat(BaseAddress + (uint) Addresses.UnitField.UNIT_FIELD_R);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > Orientation: " + e);
                }
                return 0;
            }
        }

        public int Health
        {
            get
            {
                try
                {
                    return GetDescriptor<int>(Descriptors.UnitFields.Health);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > Health: " + e);
                    return 0;
                }
            }
        }

        public int MaxHealth
        {
            get
            {
                try
                {
                    return GetDescriptor<int>(Descriptors.UnitFields.MaxHealth);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > MaxHealth: " + e);
                    return 0;
                }
            }
        }

        public float HealthPercent
        {
            get
            {
                try
                {
                    float p = (Health*100/(float) MaxHealth);
                    if (p < 0 || p > 100)
                    {
                        return 0;
                    }
                    return p;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > HealthPercent: " + e);
                    return 0;
                }
            }
        }

        public float AggroDistance
        {
            get
            {
                try
                {
                    float num = (Level <= 5 ? 12 : 20) - (((int) ObjectManager.Me.Level - (int) Level)*1.1f);
                    if (num < 5)
                        num = 5;
                    else if (num > 45)
                        num = 45;
                    return num;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > AggroDistance: " + e);
                    return 20;
                }
            }
        }


        public bool IsHostile
        {
            get
            {
                WoWUnit localUnit = this;
                if (localUnit.IsDead || !localUnit.IsValid)
                    return false;
                if (localUnit is WoWPlayer)
                {
                    var p = localUnit as WoWPlayer;
                    if (p.IsValid)
                    {
                        if (p.Guid == ObjectManager.Me.Guid)
                            return false;
                        if (p.PlayerFaction != ObjectManager.Me.PlayerFaction)
                            return true;
                        string randomString = Others.GetRandomString(Others.Random(5, 10));
                        string result = Lua.LuaDoString(randomString + " = tostring(UnitIsEnemy(\"player\", \"target\"))", randomString);
                        if (result == "true")
                            return true;
                        return false;
                    }
                    return false;
                }
                return localUnit.Reaction <= Reaction.Neutral;
            }
        }

        public float GetCombatReach
        {
            get
            {
                try
                {
                    return GetDescriptor<float>(Descriptors.UnitFields.CombatReach);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > GetCombatReach: " + e);
                    return 0;
                }
            }
        }

        public string CreatureTypeTarget
        {
            get
            {
                lock (this)
                {
                    try
                    {
                        string randomStringResult = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString(randomStringResult + " = UnitCreatureType(\"target\")");
                        return Lua.GetLocalizedText(randomStringResult);
                    }
                    catch (Exception e)
                    {
                        Logging.WriteError("WoWUnit > CreatureTypeTarget: " + e);
                        return "";
                    }
                }
            }
        }

        public string CreatureRankTarget
        {
            get
            {
                lock (this)
                {
                    try
                    {
                        string randomStringResult = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString(randomStringResult + " = UnitClassification(\"target\")");
                        return Lua.GetLocalizedText(randomStringResult);
                    }
                    catch (Exception e)
                    {
                        Logging.WriteError("WoWUnit > CreatureRankTarget: " + e);
                        return "";
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
                    Logging.WriteError("WoWUnit > FactionTemplate: " + e);
                    return null;
                }
            }
        }

        public bool IsBoss
        {
            get
            {
                try
                {
                    uint pointerToUnk0 = Memory.WowMemory.Memory.ReadUInt(Memory.WowMemory.Memory.ReadUInt(BaseAddress + (uint) 0x11C) + (uint) 0x180);
                    int ret;
                    if (GetDbCacheRowPtr <= 0 || pointerToUnk0 > 0)
                        ret = 0;
                    else
                        ret = (Memory.WowMemory.Memory.ReadInt(GetDbCacheRowPtr + (uint) Addresses.UnitField.CachedIsBoss) >> 2) & 1;
                    return (ret == 1);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsBoss: " + e);
                    return false;
                }
            }
        }

        public bool OnTaxi
        {
            get { return GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags).HasFlag(UnitFlags.TaxiFlight); }
        }

        public uint Mana
        {
            get
            {
                try
                {
                    return GetPowerByPowerType(PowerType.Mana);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > Mana: " + e);
                    return 0;
                }
            }
        }

        public uint MaxMana
        {
            get
            {
                try
                {
                    return GetMaxPowerByPowerType(PowerType.Mana);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > MaxMana: " + e);
                    return 0;
                }
            }
        }

        public uint ManaPercentage
        {
            get
            {
                try
                {
                    if (MaxMana > 0)
                        return Mana*100/MaxMana;
                    return 100;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > ManaPercentage: " + e);
                    return 0;
                }
            }
        }

        public float Rage
        {
            get
            {
                try
                {
                    return GetPowerByPowerType(PowerType.Rage)/10f;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > Rage: " + e);
                    return 0;
                }
            }
        }

        public float MaxRage
        {
            get
            {
                try
                {
                    return GetMaxPowerByPowerType(PowerType.Rage)/10f;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > MaxRage: " + e);
                    return 0;
                }
            }
        }

        public uint RagePercentage
        {
            get
            {
                try
                {
                    return (uint) (Rage*100/MaxRage);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > RagePercentage: " + e);
                    return 0;
                }
            }
        }

        public uint Focus
        {
            get
            {
                try
                {
                    return GetPowerByPowerType(PowerType.Focus);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > Focus: " + e);
                    return 0;
                }
            }
        }

        public uint MaxFocus
        {
            get
            {
                try
                {
                    return GetMaxPowerByPowerType(PowerType.Focus);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > MaxFocus: " + e);
                    return 0;
                }
            }
        }

        public uint FocusPercentage
        {
            get
            {
                try
                {
                    return Focus*100/MaxFocus;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > FocusPercentage: " + e);
                    return 0;
                }
            }
        }


        public uint Energy
        {
            get
            {
                try
                {
                    return GetPowerByPowerType(PowerType.Energy);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > Energy: " + e);
                    return 0;
                }
            }
        }

        public uint MaxEnergy
        {
            get
            {
                try
                {
                    return GetMaxPowerByPowerType(PowerType.Energy);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > MaxEnergy: " + e);
                    return 0;
                }
            }
        }

        public uint EnergyPercentage
        {
            get
            {
                try
                {
                    return Energy*100/MaxEnergy;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > EnergyPercentage: " + e);
                    return 0;
                }
            }
        }

        public uint ComboPoint
        {
            get
            {
                try
                {
                    return GetPowerByPowerType(PowerType.ComboPoint);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > ComboPoint: " + e);
                    return 0;
                }
            }
        }

        public uint MaxComboPoint
        {
            get
            {
                try
                {
                    return GetMaxPowerByPowerType(PowerType.ComboPoint);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > MaxComboPoint: " + e);
                    return 0;
                }
            }
        }

        public uint ComboPointPercentage
        {
            get
            {
                try
                {
                    return ComboPoint*100/MaxComboPoint;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > ComboPointPercentage: " + e);
                    return 0;
                }
            }
        }

        public uint Chi
        {
            get
            {
                try
                {
                    return GetPowerByPowerType(PowerType.Chi);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > Chi: " + e);
                    return 0;
                }
            }
        }

        public uint MaxChi
        {
            get
            {
                try
                {
                    return GetMaxPowerByPowerType(PowerType.Chi);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > MaxChi: " + e);
                    return 0;
                }
            }
        }

        public uint ChiPercentage
        {
            get
            {
                try
                {
                    return Chi*100/MaxChi;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > ChiPercentage: " + e);
                    return 0;
                }
            }
        }

        public uint Runes
        {
            get
            {
                try
                {
                    return GetPowerByPowerType(PowerType.Runes);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > Runes: " + e);
                    return 0;
                }
            }
        }

        public uint MaxRunes
        {
            get
            {
                try
                {
                    return GetMaxPowerByPowerType(PowerType.Runes);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > MaxRunes: " + e);
                    return 0;
                }
            }
        }

        public uint RunesPercentage
        {
            get
            {
                try
                {
                    return Runes*100/MaxRunes;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > RunesPercentage: " + e);
                    return 0;
                }
            }
        }

        public uint RunicPower
        {
            get
            {
                try
                {
                    return GetPowerByPowerType(PowerType.RunicPower);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > RunicPower: " + e);
                    return 0;
                }
            }
        }

        public uint MaxRunicPower
        {
            get
            {
                try
                {
                    return GetMaxPowerByPowerType(PowerType.RunicPower);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > MaxRunicPower: " + e);
                    return 0;
                }
            }
        }

        public uint RunicPowerPercentage
        {
            get
            {
                try
                {
                    return RunicPower*100/MaxRunicPower;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > RunicPowerPercentage: " + e);
                    return 0;
                }
            }
        }

        public uint SoulShards
        {
            get
            {
                try
                {
                    return GetPowerByPowerType(PowerType.SoulShards);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > SoulShards: " + e);
                    return 0;
                }
            }
        }

        public uint MaxSoulShards
        {
            get
            {
                try
                {
                    return GetMaxPowerByPowerType(PowerType.SoulShards);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > MaxSoulShards: " + e);
                    return 0;
                }
            }
        }

        public uint SoulShardsPercentage
        {
            get
            {
                try
                {
                    return SoulShards*100/MaxSoulShards;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > SoulShardsPercentage: " + e);
                    return 0;
                }
            }
        }

        public uint Eclipse
        {
            get
            {
                try
                {
                    return GetPowerByPowerType(PowerType.Eclipse);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > Eclipse: " + e);
                    return 0;
                }
            }
        }

        public uint MaxEclipse
        {
            get
            {
                try
                {
                    return GetMaxPowerByPowerType(PowerType.Eclipse);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > MaxEclipse: " + e);
                    return 0;
                }
            }
        }

        public uint EclipsePercentage
        {
            get
            {
                try
                {
                    return Eclipse*100/MaxEclipse;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > EclipsePercentage: " + e);
                    return 0;
                }
            }
        }

        public uint HolyPower
        {
            get
            {
                try
                {
                    return GetPowerByPowerType(PowerType.HolyPower);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > HolyPower: " + e);
                    return 0;
                }
            }
        }

        public uint MaxHolyPower
        {
            get
            {
                try
                {
                    return GetMaxPowerByPowerType(PowerType.HolyPower);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > MaxHolyPower: " + e);
                    return 0;
                }
            }
        }

        public uint HolyPowerPercentage
        {
            get
            {
                try
                {
                    return HolyPower*100/MaxHolyPower;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > HolyPowerPercentage: " + e);
                    return 0;
                }
            }
        }

        public uint Alternate
        {
            get
            {
                try
                {
                    return GetPowerByPowerType(PowerType.Alternate);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > Alternate: " + e);
                    return 0;
                }
            }
        }

        public uint MaxAlternate
        {
            get
            {
                try
                {
                    return GetMaxPowerByPowerType(PowerType.Alternate);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > MaxAlternate: " + e);
                    return 0;
                }
            }
        }

        public uint AlternatePercentage
        {
            get
            {
                try
                {
                    return Alternate*100/MaxAlternate;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > AlternatePercentage: " + e);
                    return 0;
                }
            }
        }

        public uint DarkForce
        {
            get
            {
                try
                {
                    return GetPowerByPowerType(PowerType.DarkForce);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > DarkForce: " + e);
                    return 0;
                }
            }
        }

        public uint MaxDarkForce
        {
            get
            {
                try
                {
                    return GetMaxPowerByPowerType(PowerType.DarkForce);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > MaxDarkForce: " + e);
                    return 0;
                }
            }
        }

        public uint DarkForcePercentage
        {
            get
            {
                try
                {
                    return DarkForce*100/MaxDarkForce;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > DarkForcePercentage: " + e);
                    return 0;
                }
            }
        }

        public uint LightForce
        {
            get
            {
                try
                {
                    return GetPowerByPowerType(PowerType.LightForce);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > LightForce: " + e);
                    return 0;
                }
            }
        }

        public uint MaxLightForce
        {
            get
            {
                try
                {
                    return GetMaxPowerByPowerType(PowerType.LightForce);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > MaxLightForce: " + e);
                    return 0;
                }
            }
        }

        public uint LightForcePercentage
        {
            get
            {
                try
                {
                    return LightForce*100/MaxLightForce;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > LightForcePercentage: " + e);
                    return 0;
                }
            }
        }

        public uint ShadowOrbs
        {
            get
            {
                try
                {
                    return GetPowerByPowerType(PowerType.ShadowOrbs);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > ShadowOrbs: " + e);
                    return 0;
                }
            }
        }

        public uint MaxShadowOrbs
        {
            get
            {
                try
                {
                    return GetMaxPowerByPowerType(PowerType.ShadowOrbs);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > MaxShadowOrbs: " + e);
                    return 0;
                }
            }
        }

        public uint ShadowOrbsPercentage
        {
            get
            {
                try
                {
                    return ShadowOrbs*100/MaxShadowOrbs;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > ShadowOrbsPercentage: " + e);
                    return 0;
                }
            }
        }

        public uint BurningEmbers
        {
            get
            {
                try
                {
                    return GetPowerByPowerType(PowerType.BurningEmbers);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > BurningEmbers: " + e);
                    return 0;
                }
            }
        }

        public uint MaxBurningEmbers
        {
            get
            {
                try
                {
                    return GetMaxPowerByPowerType(PowerType.BurningEmbers);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > MaxBurningEmbers: " + e);
                    return 0;
                }
            }
        }

        public uint BurningEmbersPercentage
        {
            get
            {
                try
                {
                    return BurningEmbers*100/MaxBurningEmbers;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > BurningEmbersPercentage: " + e);
                    return 0;
                }
            }
        }

        public uint DemonicFury
        {
            get
            {
                try
                {
                    return GetPowerByPowerType(PowerType.DemonicFury);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > DemonicFury: " + e);
                    return 0;
                }
            }
        }

        public uint MaxDemonicFury
        {
            get
            {
                try
                {
                    return GetMaxPowerByPowerType(PowerType.DemonicFury);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > MaxDemonicFury: " + e);
                    return 0;
                }
            }
        }

        public uint DemonicFuryPercentage
        {
            get
            {
                try
                {
                    return DemonicFury*100/MaxDemonicFury;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > DemonicFuryPercentage: " + e);
                    return 0;
                }
            }
        }

        public uint ArcaneCharges
        {
            get
            {
                try
                {
                    return GetPowerByPowerType(PowerType.ArcaneCharges);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > ArcaneCharges: " + e);
                    return 0;
                }
            }
        }

        public uint MaxArcaneCharges
        {
            get
            {
                try
                {
                    return GetMaxPowerByPowerType(PowerType.ArcaneCharges);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > MaxArcaneCharges: " + e);
                    return 0;
                }
            }
        }

        public uint ArcaneChargesPercentage
        {
            get
            {
                try
                {
                    return ArcaneCharges*100/MaxArcaneCharges;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > ArcaneChargesPercentage: " + e);
                    return 0;
                }
            }
        }

        private uint GetPowerIndexByPowerType(PowerType powerType)
        {
            uint descriptorsArray = Memory.WowMemory.Memory.ReadUInt(BaseAddress + Descriptors.StartDescriptors);
            uint displayPower = descriptorsArray +
                                ((uint) Descriptors.UnitFields.Sex*Descriptors.Multiplicator);
            uint index = Memory.WowMemory.Memory.ReadByte(displayPower + 0x1) + (uint) powerType +
                         (uint) Addresses.PowerIndex.Multiplicator*Memory.WowMemory.Memory.ReadByte(displayPower + 0x1);
            uint result =
                Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule +
                                                 (uint) Addresses.PowerIndex.PowerIndexArrays + index*4); // To be updated. (Use Get Descriptors)
            return result;
        }

        public uint GetPowerByPowerType(PowerType powerType)
        {
            uint index = GetPowerIndexByPowerType(powerType);
            uint descriptorsArray = Memory.WowMemory.Memory.ReadUInt(BaseAddress + Descriptors.StartDescriptors);
            uint powerValue =
                Memory.WowMemory.Memory.ReadUInt(descriptorsArray +
                                                 ((uint) Descriptors.UnitFields.Power*Descriptors.Multiplicator +
                                                  index*4)); // To be updated. (Use Get Descriptors)
            return powerValue;
        }

        public uint GetMaxPowerByPowerType(PowerType powerType)
        {
            uint index = GetPowerIndexByPowerType(powerType);
            uint descriptorsArray = Memory.WowMemory.Memory.ReadUInt(BaseAddress + Descriptors.StartDescriptors);
            uint powerValue =
                Memory.WowMemory.Memory.ReadUInt(descriptorsArray +
                                                 ((uint) Descriptors.UnitFields.MaxPower*Descriptors.Multiplicator +
                                                  index*4)); // To be updated. (Use Get Descriptors)
            return powerValue;
        }

        public uint Faction
        {
            get
            {
                try
                {
                    return GetDescriptor<uint>(Descriptors.UnitFields.FactionTemplate);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > Faction: " + e);
                    return 0;
                }
            }
        }

        public uint DisplayId
        {
            get
            {
                try
                {
                    return GetDescriptor<uint>(Descriptors.UnitFields.DisplayID);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > DisplayId: " + e);
                    return 0;
                }
            }
        }

        private Point _lastPosMove = new Point();

        public bool GetMove
        {
            get
            {
                try
                {
                    bool bResult = false;

                    _lastPosMove = ObjectManager.Me.Position;
                    Thread.Sleep(50);
                    if (Math.Round(_lastPosMove.X, 1) != Math.Round(ObjectManager.Me.Position.X, 1) ||
                        Math.Round(_lastPosMove.Z, 1) != Math.Round(ObjectManager.Me.Position.Z, 1) ||
                        Math.Round(_lastPosMove.Y, 1) != Math.Round(ObjectManager.Me.Position.Y, 1))
                        bResult = true;
                    if (!bResult)
                    {
                        Thread.Sleep(30);
                        if (Math.Round(_lastPosMove.X, 1) != Math.Round(ObjectManager.Me.Position.X, 1) ||
                            Math.Round(_lastPosMove.Z, 1) != Math.Round(ObjectManager.Me.Position.Z, 1) ||
                            Math.Round(_lastPosMove.Y, 1) != Math.Round(ObjectManager.Me.Position.Y, 1))
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

        public override float GetDistance
        {
            get
            {
                try
                {
                    return Position.DistanceTo(ObjectManager.Me.Position);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > GetDistance: " + e);
                    return 0;
                }
            }
        }

        public float GetDistance2D
        {
            get
            {
                try
                {
                    return Position.DistanceTo2D(ObjectManager.Me.Position);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > GetDistance2D: " + e);
                    return 0;
                }
            }
        }

        public bool IsAlive
        {
            get
            {
                try
                {
                    if (!IsValid)
                        return false;
                    return !IsDead;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsAlive: " + e);
                    return false;
                }
            }
        }

        private static List<uint> _ghostSpells = new List<uint>();

        public bool IsDead
        {
            get
            {
                try
                {
                    if (!IsValid)
                        return true;
                    if (IsNpcQuestGiver)
                        return false;
                    if (Guid == ObjectManager.Me.Guid)
                        return ObjectManager.Me.IsDeadMe;
                    if (Type == WoWObjectType.Player)
                    {
                        if (_ghostSpells.Count <= 0) _ghostSpells = SpellManager.SpellListManager.SpellIdByName("Ghost");
                        if (HaveBuff(_ghostSpells))
                            return true;
                        return Health <= 1 || GetDescriptor<UnitDynamicFlags>(Descriptors.ObjectFields.DynamicFlags).HasFlag(UnitDynamicFlags.Dead);
                    }
                    return Health <= 0 || GetDescriptor<UnitDynamicFlags>(Descriptors.ObjectFields.DynamicFlags).HasFlag(UnitDynamicFlags.Dead);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsDead: " + e);
                    return false;
                }
            }
        }

        public bool IsLootable
        {
            get
            {
                try
                {
                    return GetDescriptor<UnitDynamicFlags>(Descriptors.ObjectFields.DynamicFlags).HasFlag(UnitDynamicFlags.Lootable);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsLootable: " + e);
                    return false;
                }
            }
        }

        public bool IsTapped
        {
            get
            {
                try
                {
                    return GetDescriptor<UnitDynamicFlags>(Descriptors.ObjectFields.DynamicFlags).HasFlag(UnitDynamicFlags.Tapped);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsTapped: " + e);
                    return false;
                }
            }
        }

        public bool IsTappedByMe
        {
            get
            {
                try
                {
                    return GetDescriptor<UnitDynamicFlags>(Descriptors.ObjectFields.DynamicFlags).HasFlag(UnitDynamicFlags.TappedByMe);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsTappedByMe: " + e);
                    return false;
                }
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
                    Logging.WriteError("WoWUnit > IsTargetingMe: " + e);
                    return false;
                }
            }
        }

        public UInt128 Target
        {
            get
            {
                try
                {
                    return GetDescriptor<UInt128>(Descriptors.UnitFields.Target);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > : Target get" + e);
                    return 0;
                }
            }
            set
            {
                try
                {
                    Memory.WowMemory.Memory.WriteInt128(
                        Memory.WowMemory.Memory.ReadUInt(GetBaseAddress + Descriptors.StartDescriptors) +
                        (uint) Descriptors.UnitFields.Target*Descriptors.Multiplicator, value);
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
                try
                {
                    return GetDescriptor<uint>(Descriptors.UnitFields.Level);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > Level: " + e);
                    return 0;
                }
            }
        }

        public enum PartyRole
        {
            Tank,
            DPS,
            Heal,
            None
        }

        public PartyRole GetUnitRole
        {
            get
            {
                string randomStringResult = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(randomStringResult + " = UnitGroupRolesAssigned(\"" + Name + "\")");
                var ret = Lua.GetLocalizedText(randomStringResult);
                if (ret == "TANK")
                    return PartyRole.Tank;
                if (ret == "DAMAGER")
                    return PartyRole.DPS;
                if (ret == "HEALER")
                    return PartyRole.Heal;
                return PartyRole.None;
            }
        }

        public PartyEnums.PartyType GetCurrentPartyType
        {
            get
            {
                uint instancePointer = Party.GetPartyPointer(PartyEnums.PartyType.LE_PARTY_CATEGORY_INSTANCE);
                uint homePointer = Party.GetPartyPointer();
                return instancePointer > 0
                    ? PartyEnums.PartyType.LE_PARTY_CATEGORY_INSTANCE
                    : (homePointer > 0 ? PartyEnums.PartyType.LE_PARTY_CATEGORY_HOME : PartyEnums.PartyType.None);
            }
        }

        public PartyEnums.PartyType GetCurrentPartyTypeLUA
        {
            get
            {
                if (Party.IsInGroupLUA(PartyEnums.PartyType.LE_PARTY_CATEGORY_INSTANCE))
                    return PartyEnums.PartyType.LE_PARTY_CATEGORY_INSTANCE;
                if (Party.IsInGroupLUA())
                    return PartyEnums.PartyType.LE_PARTY_CATEGORY_HOME;
                return PartyEnums.PartyType.None;
            }
        }

        public bool IsHomePartyLeader
        {
            get
            {
                try
                {
                    return Party.GetPartyLeaderGUID() == Guid;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsHomePartyLeader: " + e);
                    return false;
                }
            }
        }

        public bool IsInstancePartyLeader
        {
            get
            {
                try
                {
                    return Party.GetPartyLeaderGUID() == Guid;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsInstancePartyLeader: " + e);
                    return false;
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
                                                                   (uint) Addresses.Player.playerName);
                    }
                    if (Type == WoWObjectType.Player)
                    {
                        return Usefuls.GetPlayerName(Guid);
                    }
                    return Memory.WowMemory.Memory.ReadUTF8String(Memory.WowMemory.Memory.ReadUInt(GetDbCacheRowPtr + (uint) Addresses.UnitField.CachedName));
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > Name: " + e);
                    return "";
                }
            }
        }

        private uint GetDbCacheRowPtr
        {
            get { return Memory.WowMemory.Memory.ReadUInt(BaseAddress + (uint) Addresses.UnitField.DBCacheRow); }
        }

        public string SubName
        {
            get
            {
                try
                {
                    if (Type == WoWObjectType.Player)
                    {
                        return "";
                    }
                    return
                        Memory.WowMemory.Memory.ReadUTF8String(Memory.WowMemory.Memory.ReadUInt(GetDbCacheRowPtr + (uint) Addresses.UnitField.CachedSubName));
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > SubName: " + e);
                    return "";
                }
            }
        }

        private uint QuestItem(uint offset)
        {
            try
            {
                if (offset > 3)
                    return 0;
                return
                    Memory.WowMemory.Memory.ReadUInt(GetDbCacheRowPtr + (uint) Addresses.UnitField.CachedQuestItem1 + (0x04*offset));
            }
            catch (Exception e)
            {
                Logging.WriteError("WoWUnit > QuestItem(" + offset + "): " + e);
                return 0;
            }
        }

        public uint QuestItem1
        {
            get { return QuestItem(0); }
        }

        public uint QuestItem2
        {
            get { return QuestItem(1); }
        }

        public uint QuestItem3
        {
            get { return QuestItem(2); }
        }

        public uint QuestItem4
        {
            get { return QuestItem(3); }
        }

        public int ModelId
        {
            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadInt(GetDbCacheRowPtr + (uint) Addresses.UnitField.CachedModelId1);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > ModelId: " + e);
                    return 0;
                }
            }
        }

        public TypeFlag ExtraLootType
        {
            get
            {
                try
                {
                    int cachedtype = Memory.WowMemory.Memory.ReadInt(GetDbCacheRowPtr + (uint) Addresses.UnitField.CachedTypeFlag);
                    return (TypeFlag) cachedtype;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > ExtraLootType: " + e);
                    return TypeFlag.None;
                }
            }
        }

        public bool IsSkinnable
        {
            get
            {
                try
                {
                    return GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags).HasFlag(UnitFlags.Skinnable);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsSkinnable: " + e);
                    return false;
                }
            }
        }

        public uint GetSkillLevelRequired
        {
            get
            {
                if (Level > 87)
                    return 500 + ((Level - 87)*20);
                else if (Level > 83)
                    return 450 + ((Level - 83)*20);
                else if (Level > 80)
                    return 435 + ((Level - 80)*5);
                else if (Level > 73)
                    return 365 + ((Level - 73)*10);
                else if (Level > 20)
                    return Level*5;
                else if (Level < 10)
                    return 1;
                else // between level 10 and 20
                    return (Level*10) - 100;
            }
        }

        public bool IsSilenced
        {
            get
            {
                try
                {
                    return GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags).HasFlag(UnitFlags.Silenced);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsSilenced: " + e);
                    return false;
                }
            }
        }

        public bool IsStunned
        {
            get
            {
                try
                {
                    return GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags).HasFlag(UnitFlags.Stunned);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsStunned: " + e);
                    return false;
                }
            }
        }

        public bool IsConfused
        {
            get
            {
                try
                {
                    return GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags).HasFlag(UnitFlags.Confused);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsConfused: " + e);
                    return false;
                }
            }
        }

        public bool IsStunnable
        {
            get
            {
                try
                {
                    return CreatureRankTarget != "worldboss";
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsStunnable: " + e);
                    return true;
                }
            }
        }

        public bool IsNpcSpiritGuide
        {
            get
            {
                try
                {
                    return GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags).HasFlag(UnitNPCFlags.SpiritHealer);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsNpcSpiritGuide: " + e);
                    return false;
                }
            }
        }

        public bool IsNpcSpiritHealer
        {
            get
            {
                try
                {
                    return GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags).HasFlag(UnitNPCFlags.SpiritHealer);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsNpcSpiritHealer: " + e);
                    return false;
                }
            }
        }

        public bool IsNpcRepair
        {
            get
            {
                try
                {
                    return GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags).HasFlag(UnitNPCFlags.CanRepair);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsNpcRepair: " + e);
                    return false;
                }
            }
        }

        public bool IsNpcFlightMaster
        {
            get
            {
                try
                {
                    return GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags).HasFlag(UnitNPCFlags.Taxi);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsNpcFlightMaster: " + e);
                    return false;
                }
            }
        }

        public bool IsNpcMailbox
        {
            get
            {
                try
                {
                    return GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags).HasFlag(UnitNPCFlags.MailInfo);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsNpcMailbox: " + e);
                    return false;
                }
            }
        }

        public bool IsNpcQuestGiver
        {
            get
            {
                try
                {
                    return GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags).HasFlag(UnitNPCFlags.QuestGiver);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsNpcQuestGiver: " + e);
                    return false;
                }
            }
        }

        public bool IsNpcVendor
        {
            get
            {
                try
                {
                    return GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags).HasFlag(UnitNPCFlags.Vendor);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsNpcVendor: " + e);
                    return false;
                }
            }
        }

        public bool IsNpcInnkeeper
        {
            get
            {
                try
                {
                    return GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags).HasFlag(UnitNPCFlags.Innkeeper);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsNpcInnkeeper: " + e);
                    return false;
                }
            }
        }

        public bool IsNpcAuctioneer
        {
            get
            {
                try
                {
                    return GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags).HasFlag(UnitNPCFlags.Auctioneer);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsNpcAuctioneer: " + e);
                    return false;
                }
            }
        }

        public bool IsNpcVendorFood
        {
            get
            {
                try
                {
                    return GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags).HasFlag(UnitNPCFlags.SellsFood);
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
                    return GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags).HasFlag(UnitNPCFlags.CanTrain);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsNpcTrainer: " + e);
                    return false;
                }
            }
        }

        public UInt128 SummonedBy
        {
            get
            {
                try
                {
                    return GetDescriptor<UInt128>(Descriptors.UnitFields.SummonedBy);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > SummonedBy: " + e);
                    return 0;
                }
            }
        }

        public UInt128 CreatedBy
        {
            get
            {
                try
                {
                    return GetDescriptor<UInt128>(Descriptors.UnitFields.CreatedBy);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > CreatedBy: " + e);
                    return 0;
                }
            }
        }

        public bool AutoAttack
        {
            get
            {
                try
                {
                    return GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags).HasFlag(UnitFlags.PetInCombat);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > AutoAttack: " + e);
                    return false;
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
                    return GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags).HasFlag(UnitFlags.Combat);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > InCombat: " + e);
                    return false;
                }
            }
        }

        public bool InCombatWithMe
        {
            get
            {
                try
                {
                    return InCombat && IsTargetingMe && !IsDead;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > InCombatWithMe: " + e);
                    return false;
                }
            }
        }

        public bool IsCast
        {
            get
            {
                try
                {
                    return CastingSpellId > 0;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsCast: " + e);
                    return false;
                }
            }
        }

        public int CastingSpellId
        {
            get { return CurrentSpellIdCast > 0 ? CurrentSpellIdCast : CurrentSpellIdChannel > 0 ? CurrentSpellIdChannel : 0; }
        }

        public int CurrentSpellIdCast
        {
            get
            {
                var spellId = Memory.WowMemory.Memory.ReadInt(GetBaseAddress + (uint) Addresses.UnitField.CastingSpellID); // To Repair
                return spellId;
            }
        }

        public int CurrentSpellIdChannel
        {
            get
            {
                var spellId = Memory.WowMemory.Memory.ReadInt(GetBaseAddress + (uint) Addresses.UnitField.ChannelSpellID); // To Repair
                return spellId;
            }
        }

        public int CastEndsInMs
        {
            get
            {
                if (IsCast && CastingEndTime > 0 && CastingEndTime > Usefuls.GetWoWTime)
                    return CastingEndTime - Usefuls.GetWoWTime;
                return 0;
            }
        }

        public uint CastingStartTime
        {
            get
            {
                if (CurrentSpellIdCast > 0)
                    return Memory.WowMemory.Memory.ReadUInt(GetBaseAddress + (uint) Addresses.UnitField.CastingSpellStartTime);
                if (CurrentSpellIdChannel > 0)
                    return Memory.WowMemory.Memory.ReadUInt(GetBaseAddress + (uint) Addresses.UnitField.ChannelSpellStartTime);
                return 0;
            }
        }

        public int CastingEndTime
        {
            get
            {
                if (CurrentSpellIdCast > 0)
                    return Memory.WowMemory.Memory.ReadInt(GetBaseAddress + (uint) Addresses.UnitField.CastingSpellEndTime);
                if (CurrentSpellIdChannel > 0)
                    return Memory.WowMemory.Memory.ReadInt(GetBaseAddress + (uint) Addresses.UnitField.ChannelSpellEndTime);
                return 0;
            }
        }

        public bool CanInterruptCurrentCast
        {
            get
            {
                try
                {
                    if (IsCast)
                    {
                        uint castTest = Memory.WowMemory.Memory.ReadUInt(GetBaseAddress + (uint) Addresses.UnitField.CanInterruptOffset);
                        uint castTest2 = Memory.WowMemory.Memory.ReadUInt(GetBaseAddress + (uint) Addresses.UnitField.CanInterruptOffset2);
                        uint castTest3 = Memory.WowMemory.Memory.ReadUInt(GetBaseAddress + (uint) Addresses.UnitField.CanInterruptOffset3);
                        bool canInterrupt = (Memory.WowMemory.Memory.ReadByte(GetBaseAddress + (uint) Addresses.UnitField.CanInterrupt) & 8) == 0 && (castTest > 0 || castTest2 > 0 || castTest3 > 0);
                        return canInterrupt;
                    }
                    return false;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > CanInterruptCurrentCast: " + e);
                    return false;
                }
            }
        }

        public bool CanTurnIn
        {
            get
            {
                try
                {
                    UnitQuestGiverStatus questGiverStatus = (UnitQuestGiverStatus) Memory.WowMemory.Memory.ReadInt(GetBaseAddress + (uint) Addresses.Quests.QuestGiverStatus);
                    return questGiverStatus.HasFlag(UnitQuestGiverStatus.TurnIn) || questGiverStatus.HasFlag(UnitQuestGiverStatus.TurnInInvisible) ||
                           questGiverStatus.HasFlag(UnitQuestGiverStatus.TurnInRepeatable) || questGiverStatus.HasFlag(UnitQuestGiverStatus.LowLevelTurnInRepeatable);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > CanTurnIn: " + e);
                    return false;
                }
            }
        }

        public bool IsMounted
        {
            get
            {
                try
                {
                    return GetDescriptor<int>(Descriptors.UnitFields.MountDisplayID) > 0 || HaveBuff(SpellManager.MountDruidId()) || InTransport || Usefuls.IsFlying;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsMounted: " + e);
                    return false;
                }
            }
        }

        public int MountDisplayId
        {
            get
            {
                try
                {
                    return GetDescriptor<int>(Descriptors.UnitFields.MountDisplayID);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > MountDisplayId: " + e);
                    return 0;
                }
            }
        }

        public bool InPVP
        {
            get
            {
                try
                {
                    return GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags).HasFlag(UnitFlags.PvPFlagged);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > InPVP: " + e);
                    return false;
                }
            }
        }

        public bool PVP
        {
            get
            {
                try
                {
                    return GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags).HasFlag(UnitFlags.PlayerControlled);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > PVP: " + e);
                    return false;
                }
            }
        }

        public bool CanAttackTargetLUA
        {
            get
            {
                try
                {
                    string randomStringResult = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(randomStringResult + " = tostring(UnitCanAttack(\"player\", \"target\"))");
                    return Lua.GetLocalizedText(randomStringResult) == "true";
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > CanAttackLUA: " + e);
                    return false;
                }
            }
        }

        public bool Attackable
        {
            get
            {
                try
                {
                    return ((GetDescriptor<UInt32>(Descriptors.UnitFields.Flags) & 0x10382) == 0) &&
                           (UnitRelation.GetReaction(Faction) < Reaction.Neutral ||
                            (UnitRelation.GetReaction(Faction) == Reaction.Neutral && (GetDescriptor<UInt32>(Descriptors.UnitFields.NpcFlags) == 0 || Guid == ObjectManager.Me.Target && CanAttackTargetLUA)));
                    /*  GetDescriptor<UInt32>(Descriptors.UnitFields.Flags) & 0x10382) == 0
                        Donne ça en plus long et plus lent:
                        UnitFlags f = GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags);
                        !f.HasFlag(UnitFlags.SelectableNotAttackable_1) && !f.HasFlag(UnitFlags.SelectableNotAttackable_2) &&
                        !f.HasFlag(UnitFlags.NotAttackable) && !f.HasFlag(UnitFlags.Flag_9_0x200) &&
                        !f.HasFlag(UnitFlags.SelectableNotAttackable_3)*/
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > Attackable: " + e);
                    return false;
                }
            }
        }

        public bool NotAttackable
        {
            get
            {
                try
                {
                    return GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags).HasFlag(UnitFlags.NotAttackable);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > NotAttackable: " + e);
                    return false;
                }
            }
        }

        public bool NotSelectable
        {
            get
            {
                try
                {
                    return GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags).HasFlag(UnitFlags.NotSelectable);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > NotSelectable: " + e);
                    return false;
                }
            }
        }

        public bool PlayerControlled
        {
            get
            {
                try
                {
                    return GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags).HasFlag(UnitFlags.PlayerControlled);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > PlayerControlled: " + e);
                    return false;
                }
            }
        }

        public UInt128 TransportGuid
        {
            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadUInt128(GetBaseAddress + (uint) Addresses.UnitField.TransportGUID);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > TransportGuid: " + e);
                    return 0;
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
                    Logging.WriteError("WoWUnit > InTransport: " + e);
                    return false;
                }
            }
        }

        public Auras.UnitAuras UnitAuras
        {
            get { return BuffManager.AuraStack(BaseAddress); }
        }

        public Auras.UnitAura UnitAura(List<UInt32> idBuffs, UInt128 creatorGUID)
        {
            foreach (var aura in UnitAuras.Auras)
            {
                if (idBuffs.Contains(aura.AuraSpellId) && aura.AuraCreatorGUID == creatorGUID)
                    return aura;
            }
            return new Auras.UnitAura();
        }

        public Auras.UnitAura UnitAura(UInt32 idBuff, UInt128 creatorGUID)
        {
            var idBuffs = new List<UInt32> {idBuff};
            return UnitAura(idBuffs, creatorGUID);
        }

        public Auras.UnitAura UnitAura(List<UInt32> idBuffs)
        {
            foreach (var aura in UnitAuras.Auras)
            {
                if (idBuffs.Contains(aura.AuraSpellId))
                    return aura;
            }
            return new Auras.UnitAura();
        }

        public Auras.UnitAura UnitAura(UInt32 idBuff)
        {
            var idBuffs = new List<UInt32> {idBuff};
            return UnitAura(idBuffs);
        }

        public int BuffStack(List<UInt32> idBuffs)
        {
            try
            {
                return BuffManager.AuraStack(BaseAddress, idBuffs);
            }
            catch (Exception e)
            {
                Logging.WriteError("WoWUnit > BuffStack(List<UInt32> idBuffs): " + e);
                return -1;
            }
        }

        public int BuffStack(UInt32 idBuff)
        {
            try
            {
                var idBuffs = new List<UInt32> {idBuff};
                return BuffManager.AuraStack(BaseAddress, idBuffs);
            }
            catch (Exception e)
            {
                Logging.WriteError("WoWUnit > BuffStack(UInt32 idBuffs): " + e);
                return -1;
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
                Logging.WriteError("WoWUnit > HaveBuff(List<UInt32> idBuffs): " + e);
                return false;
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
                Logging.WriteError("WoWUnit > HaveBuff(UInt32 idBuffs): " + e);
                return false;
            }
        }

        public int AuraTimeLeft(UInt32 idBuff, bool fromMe = false)
        {
            var aura = UnitAura(idBuff);
            if (aura.IsActive && (!fromMe || aura.AuraCreatorGUID == ObjectManager.Me.Guid))
                return aura.AuraTimeLeftInMs;
            return 0;
        }

        public bool AuraIsActiveAndExpireInLessThanMs(UInt32 idBuff, uint expireInLessThanMs, bool fromMe = false)
        {
            return (AuraTimeLeft(idBuff, fromMe) > 0 && AuraTimeLeft(idBuff, fromMe) < expireInLessThanMs);
        }

        public Reaction Reaction
        {
            get
            {
                try
                {
                    return UnitRelation.GetReaction(ObjectManager.Me.Faction, Faction);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > Reaction: " + e);
                    return Reaction.Neutral;
                }
            }
        }

        private static readonly List<uint> FlagsIds = new List<uint>();

        public bool IsHoldingWGFlag
        {
            get
            {
                try
                {
                    if (FlagsIds.Count <= 0)
                    {
                        FlagsIds.Add(23333);
                        FlagsIds.Add(23335);
                    }
                    return HaveBuff(FlagsIds);
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool IsTracked
        {
            get
            {
                try
                {
                    return GetDescriptor<UnitDynamicFlags>(Descriptors.ObjectFields.DynamicFlags).HasFlag(UnitDynamicFlags.TrackUnit);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsTracked get: " + e);
                    return false;
                }
            }
            set
            {
                try
                {
                    uint descriptor = GetDescriptor<uint>(Descriptors.ObjectFields.DynamicFlags);
                    long t;
                    if (value)
                    {
                        t = descriptor | 2;
                    }
                    else
                    {
                        t = descriptor & -3L;
                    }

                    uint descriptorsArray =
                        Memory.WowMemory.Memory.ReadUInt(GetBaseAddress + Descriptors.StartDescriptors);
                    uint addressGD = descriptorsArray +
                                     ((uint) Descriptors.ObjectFields.DynamicFlags*Descriptors.Multiplicator);
                    Memory.WowMemory.Memory.WriteInt64(addressGD, t);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsTracked set: " + e);
                }
            }
        }

        public bool Invisible
        {
            get
            {
                try
                {
                    return GetDescriptor<UnitDynamicFlags>(Descriptors.UnitFields.Flags).HasFlag(UnitDynamicFlags.Invisible);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > Invisible: " + e);
                    return false;
                }
            }
        }

        public bool IsUnitBrawlerAndTappedByMe
        {
            get
            {
                if (!IsTappedByMe)
                    return false;
                if (Usefuls.ContinentId != 1043 && Usefuls.ContinentId != 369)
                    return false;
                return true;
            }
        }

        public bool IsTrivial
        {
            get
            {
                if (IsBoss)
                    return false;
                uint unitLevel = Level;
                uint playerLevel = ObjectManager.Me.Level;

                int levelAboveUnit = (int) (playerLevel - unitLevel);

                if (levelAboveUnit <= -3)
                    return false;

                if (levelAboveUnit < 0)
                {
                    if (ObjectManager.Me.MaxHealth/2 >= MaxHealth)
                        return true;
                    return false;
                }

                if (levelAboveUnit < 5)
                {
                    if (ObjectManager.Me.MaxHealth*1.5 >= MaxHealth)
                        return true;
                    return false;
                }

                if (levelAboveUnit < 10)
                {
                    if (ObjectManager.Me.MaxHealth*4 >= MaxHealth)
                        return true;
                    return false;
                }

                uint trivialHp = (uint) (playerLevel/unitLevel*8*ObjectManager.Me.MaxHealth);

                if (trivialHp >= MaxHealth)
                    return true;
                return false;
            }
        }

        public T GetDescriptor<T>(Descriptors.UnitFields field) where T : struct
        {
            try
            {
                return GetDescriptor<T>((uint) field);
            }
            catch (Exception e)
            {
                Logging.WriteError("WoWUnit > GetDescriptor<T>(Descriptors.UnitFields field): " + e);
                return default(T);
            }
        }
    }
}