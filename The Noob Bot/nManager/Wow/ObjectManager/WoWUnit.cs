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
                                                              (uint) Addresses.UnitField.UNIT_FIELD_X),
                            Memory.WowMemory.Memory.ReadFloat(BaseAddress +
                                                              (uint) Addresses.UnitField.UNIT_FIELD_Y),
                            Memory.WowMemory.Memory.ReadFloat(BaseAddress +
                                                              (uint) Addresses.UnitField.UNIT_FIELD_Z));

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

        public double HealthPercent
        {
            get
            {
                try
                {
                    var p = (int) ((Health*100)/(double) MaxHealth);
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

        public int AggroDistance
        {
            get
            {
                try
                {
                    int num = 20;
                    if (ObjectManager.Me.Level > Level)
                    {
                        num -= Math.Abs((int) (ObjectManager.Me.Level - Level));
                    }
                    if (ObjectManager.Me.Level < Level)
                    {
                        num += Math.Abs((int) (ObjectManager.Me.Level - Level));
                    }
                    if (num < 5)
                    {
                        num = 5;
                    }
                    return (num + 3);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > AggroDistance: " + e);
                    return 20;
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

        public uint Mana
        {
            get
            {
                try
                {
                    return GetPowerByPowerType(Enums.PowerType.Mana);
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
                    return GetMaxPowerByPowerType(Enums.PowerType.Mana);
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
                    else
                    {
                        return 100;
                    }
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWPlayer > ManaPercentage: " + e);
                    return 0;
                }
            }
        }

        public uint Rage
        {
            get
            {
                try
                {
                    return GetPowerByPowerType(Enums.PowerType.Rage);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > Rage: " + e);
                    return 0;
                }
            }
        }

        public uint MaxRage
        {
            get
            {
                try
                {
                    return GetMaxPowerByPowerType(Enums.PowerType.Rage);
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
                    return Rage*100/MaxRage;
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
                    return GetPowerByPowerType(Enums.PowerType.Focus);
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
                    return GetMaxPowerByPowerType(Enums.PowerType.Focus);
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
                    return GetPowerByPowerType(Enums.PowerType.Energy);
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
                    return GetMaxPowerByPowerType(Enums.PowerType.Energy);
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

        public uint Chi
        {
            get
            {
                try
                {
                    return GetPowerByPowerType(Enums.PowerType.Chi);
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
                    return GetMaxPowerByPowerType(Enums.PowerType.Chi);
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
                    return GetPowerByPowerType(Enums.PowerType.Runes);
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
                    return GetMaxPowerByPowerType(Enums.PowerType.Runes);
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
                    return GetPowerByPowerType(Enums.PowerType.RunicPower);
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
                    return GetMaxPowerByPowerType(Enums.PowerType.RunicPower);
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
                    return GetPowerByPowerType(Enums.PowerType.SoulShards);
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
                    return GetMaxPowerByPowerType(Enums.PowerType.SoulShards);
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
                    return GetPowerByPowerType(Enums.PowerType.Eclipse);
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
                    return GetMaxPowerByPowerType(Enums.PowerType.Eclipse);
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
                    return GetPowerByPowerType(Enums.PowerType.HolyPower);
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
                    return GetMaxPowerByPowerType(Enums.PowerType.HolyPower);
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
                    return GetPowerByPowerType(Enums.PowerType.Alternate);
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
                    return GetMaxPowerByPowerType(Enums.PowerType.Alternate);
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
                    return GetPowerByPowerType(Enums.PowerType.DarkForce);
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
                    return GetMaxPowerByPowerType(Enums.PowerType.DarkForce);
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
                    return GetPowerByPowerType(Enums.PowerType.LightForce);
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
                    return GetMaxPowerByPowerType(Enums.PowerType.LightForce);
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
                    return GetPowerByPowerType(Enums.PowerType.ShadowOrbs);
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
                    return GetMaxPowerByPowerType(Enums.PowerType.ShadowOrbs);
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
                    return GetPowerByPowerType(Enums.PowerType.BurningEmbers);
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
                    return GetMaxPowerByPowerType(Enums.PowerType.BurningEmbers);
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
                    return GetPowerByPowerType(Enums.PowerType.DemonicFury);
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
                    return GetMaxPowerByPowerType(Enums.PowerType.DemonicFury);
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
                    return GetPowerByPowerType(Enums.PowerType.ArcaneCharges);
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
                    return GetMaxPowerByPowerType(Enums.PowerType.ArcaneCharges);
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

        private uint GetPowerIndexByPowerType(Enums.PowerType powerType)
        {
            uint descriptorsArray = Memory.WowMemory.Memory.ReadUInt(BaseAddress + Descriptors.startDescriptors);
            uint displayPower = descriptorsArray +
                                ((uint) Descriptors.UnitFields.DisplayPower*Descriptors.multiplicator);
            uint index = Memory.WowMemory.Memory.ReadByte(displayPower + 0x1) + (uint) powerType +
                         (uint) Addresses.PowerIndex.Multiplicator*Memory.WowMemory.Memory.ReadByte(displayPower + 0x1);
            uint result =
                Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule +
                                                 (uint) Addresses.PowerIndex.PowerIndexArrays + index*4);
            return result;
        }

        public uint GetPowerByPowerType(Enums.PowerType powerType)
        {
            uint index = GetPowerIndexByPowerType(powerType);
            uint descriptorsArray = Memory.WowMemory.Memory.ReadUInt(BaseAddress + Descriptors.startDescriptors);
            uint powerValue =
                Memory.WowMemory.Memory.ReadUInt(descriptorsArray +
                                                 ((uint) Descriptors.UnitFields.Power*Descriptors.multiplicator +
                                                  index*4));
            return powerValue;
        }

        public uint GetMaxPowerByPowerType(Enums.PowerType powerType)
        {
            uint index = GetPowerIndexByPowerType(powerType);
            uint descriptorsArray = Memory.WowMemory.Memory.ReadUInt(BaseAddress + Descriptors.startDescriptors);
            uint powerValue =
                Memory.WowMemory.Memory.ReadUInt(descriptorsArray +
                                                 ((uint) Descriptors.UnitFields.MaxPower*Descriptors.multiplicator +
                                                  index*4));
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

        public float UnitSpeed
        {
            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadFloat(BaseAddress + (uint) Addresses.UnitField.UNIT_SPEED);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > UnitSpeed: " + e);
                    return 0;
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
                    return (Health > 0);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsAlive: " + e);
                    return false;
                }
            }
        }

        public bool IsDead
        {
            get
            {
                try
                {
                    return (Health <= 0 || Health == 0.01 ||
                            GetDescriptor<Int32>(Descriptors.UnitFields.DynamicFlags) == 0x20) ||
                           (Health == 1 && GetMove);
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
                    var dynFlags = GetDescriptor<Int32>(Descriptors.UnitFields.DynamicFlags);
                    if (dynFlags == 13 || dynFlags == 1)
                        return true;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsLootable: " + e);
                    return false;
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
                    var dynFlags = GetDescriptor<Int32>(Descriptors.UnitFields.DynamicFlags);
                    if (dynFlags == 4)
                        return true;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsTagged: " + e);
                    return false;
                }
                return false;
            }
        }

        public bool IsTaggedByMe
        {
            get
            {
                try
                {
                    if (base.Guid == ObjectManager.Me.Target)
                        return true;
                    return false;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsTaggedByMe: " + e);
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

        public UInt64 Target
        {
            get
            {
                try
                {
                    return GetDescriptor<ulong>(Descriptors.UnitFields.Target);
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
                    Memory.WowMemory.Memory.WriteUInt64(
                        Memory.WowMemory.Memory.ReadUInt(GetBaseAddress + Descriptors.startDescriptors) +
                        (uint) Descriptors.UnitFields.Target*Descriptors.multiplicator, value);
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

        public PartyEnums.PartyType GetCurrentPartyType
        {
            get
            {
                return Party.IsInGroup()
                           ? (Party.GetPartyPointer(PartyEnums.PartyType.Instance) > 0
                                  ? PartyEnums.PartyType.Instance
                                  : (Party.GetPartyPointer() > 0 ? PartyEnums.PartyType.Home : PartyEnums.PartyType.None))
                           : PartyEnums.PartyType.None;
                // Priority over the Instance group as we can be in both group type at the same time.
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
                    if (Type == Enums.WoWObjectType.Player)
                    {
                        return Usefuls.GetPlayerName(Guid);
                    }

                    return
                        Memory.WowMemory.Memory.ReadUTF8String(
                            Memory.WowMemory.Memory.ReadUInt(
                                Memory.WowMemory.Memory.ReadUInt(BaseAddress +
                                                                 (uint)
                                                                 Addresses.UnitField.DBCacheRow) +
                                (uint) Addresses.UnitField.CachedName));
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > Name: " + e);
                    return "";
                }
            }
        }

        public bool IsSkinnable
        {
            get
            {
                try
                {
                    var flags = GetDescriptor<Int32>(Descriptors.UnitFields.Flags);
                    return Convert.ToBoolean(flags & 0x4000000);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsSkinnable: " + e);
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
                    var flags = GetDescriptor<Int32>(Descriptors.UnitFields.NpcFlags);
                    return Convert.ToBoolean(flags & 0x00004000);
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
                    var flags = GetDescriptor<Int32>(Descriptors.UnitFields.NpcFlags);
                    return Convert.ToBoolean(flags & 0x00001000);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsNpcRepair: " + e);
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
                    var flags = GetDescriptor<Int32>(Descriptors.UnitFields.NpcFlags);
                    return Convert.ToBoolean(flags & 0x00000080);
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
                    var flags = GetDescriptor<Int32>(Descriptors.UnitFields.NpcFlags);
                    return Convert.ToBoolean(flags & 0x00010000);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsNpcInnkeeper: " + e);
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
                    var flags = GetDescriptor<Int32>(Descriptors.UnitFields.NpcFlags);
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
                    var flags = GetDescriptor<Int32>(Descriptors.UnitFields.NpcFlags);
                    return Convert.ToBoolean(flags & 0x00000010);
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
                try
                {
                    return GetDescriptor<ulong>(Descriptors.UnitFields.SummonedBy);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > SummonedBy: " + e);
                    return 0;
                }
            }
        }

        public ulong CreatedBy
        {
            get
            {
                try
                {
                    return GetDescriptor<ulong>(Descriptors.UnitFields.CreatedBy);
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
                    var flags = GetDescriptor<Int32>(Descriptors.UnitFields.Flags);
                    return Convert.ToBoolean(flags & 0x0000800);
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
                    var flags = GetDescriptor<Int32>(Descriptors.UnitFields.Flags);
                    return Convert.ToBoolean(flags & 0x0080000) && !IsDead;
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
                    return
                        (Memory.WowMemory.Memory.ReadInt(GetBaseAddress + (uint) Addresses.UnitField.CastingSpellID) > 0 ||
                         Memory.WowMemory.Memory.ReadInt(GetBaseAddress + (uint) Addresses.UnitField.ChannelSpellID) > 0);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > IsCast: " + e);
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
                    return GetDescriptor<int>(Descriptors.UnitFields.MountDisplayID) > 0 ||
                           HaveBuff(SpellManager.MountDruidId()) || InTransport;
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
                    var flags = GetDescriptor<Int32>(Descriptors.UnitFields.Flags);
                    return Convert.ToBoolean(flags & 0x1000);
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
                    var flags = GetDescriptor<Int32>(Descriptors.UnitFields.Flags);
                    return Convert.ToBoolean(flags & 0x0000008);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit > PVP: " + e);
                    return false;
                }
            }
        }

        public ulong TransportGuid
        {
            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadUInt64(GetBaseAddress + (uint) Addresses.UnitField.TransportGUID);
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

        public int BuffStack(List<UInt32> idBuffs)
        {
            try
            {
                return BuffManager.AuraStack(BaseAddress, idBuffs);
            }
            catch (Exception e)
            {
                Logging.WriteError("WoWUnit > BuffStack(List<UInt32> idBuffs): " + e);
                return 0;
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
                Logging.WriteError("WoWUnit > BuffStack(UInt32 idBuffs): " + e);
                return 0;
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

        public Enums.Reaction Reaction
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
                    return Enums.Reaction.Neutral;
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
                    return ((GetDescriptor<uint>(Descriptors.UnitFields.DynamicFlags) & 2) != 0);
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
                    var descriptor = GetDescriptor<uint>(Descriptors.UnitFields.DynamicFlags);
                    long t;
                    if (value)
                    {
                        t = descriptor | 2;
                    }
                    else
                    {
                        t = descriptor & -3L;
                    }

                    var descriptorsArray =
                        Memory.WowMemory.Memory.ReadUInt(GetBaseAddress + Descriptors.startDescriptors);
                    var addressGD = descriptorsArray +
                                    ((uint) Descriptors.UnitFields.DynamicFlags*Descriptors.multiplicator);
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