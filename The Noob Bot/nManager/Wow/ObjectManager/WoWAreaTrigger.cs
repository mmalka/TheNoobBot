using System;
using System.Linq;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Patchables;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using Quaternion = nManager.Wow.Class.Quaternion;

namespace nManager.Wow.ObjectManager
{
    public class WoWAreaTrigger : WoWObject
    {
        public WoWAreaTrigger(uint address)
            : base(address)
        {
        }

        public new bool IsValid
        {
            get
            {
                try
                {
                    return SpellID > 0 /*&& Radius > 0*/&& base.IsValid;
                }
                catch (Exception e)
                {
                    Logging.WriteError("AreaTriggerFields > IsValid: " + e);
                    return false;
                }
            }
        }

        public UInt128 Caster
        {
            get
            {
                try
                {
                    return GetDescriptor<UInt128>(Descriptors.AreaTriggerFields.Caster);
                }
                catch (Exception e)
                {
                    Logging.WriteError("AreaTriggerFields > Caster: " + e);
                }
                return 0;
            }
        }

        public float OverrideScaleCurve
        {
            get
            {
                try
                {
                    return GetDescriptor<float>(Descriptors.AreaTriggerFields.OverrideScaleCurve);
                }
                catch (Exception e)
                {
                    Logging.WriteError("AreaTriggerFields > OverrideScaleCurve: " + e);
                }
                return 0;
            }
        }

        public float ExtraScaleCurve
        {
            get
            {
                try
                {
                    return GetDescriptor<float>(Descriptors.AreaTriggerFields.ExtraScaleCurve);
                }
                catch (Exception e)
                {
                    Logging.WriteError("AreaTriggerFields > ExtraScaleCurve: " + e);
                }
                return 0;
            }
        }


        public uint Duration
        {
            get
            {
                try
                {
                    return GetDescriptor<uint>(Descriptors.AreaTriggerFields.Duration);
                }
                catch (Exception e)
                {
                    Logging.WriteError("AreaTriggerFields > Duration: " + e);
                }
                return 0;
            }
        }

        public uint TimeToTarget
        {
            get
            {
                try
                {
                    return GetDescriptor<uint>(Descriptors.AreaTriggerFields.TimeToTarget);
                }
                catch (Exception e)
                {
                    Logging.WriteError("AreaTriggerFields > TimeToTarget: " + e);
                }
                return 0;
            }
        }

        public uint TimeToTargetScale
        {
            get
            {
                try
                {
                    return GetDescriptor<uint>(Descriptors.AreaTriggerFields.TimeToTargetScale);
                }
                catch (Exception e)
                {
                    Logging.WriteError("AreaTriggerFields > TimeToTargetScale: " + e);
                }
                return 0;
            }
        }

        public uint TimeToTargetExtraScale
        {
            get
            {
                try
                {
                    return GetDescriptor<uint>(Descriptors.AreaTriggerFields.TimeToTargetExtraScale);
                }
                catch (Exception e)
                {
                    Logging.WriteError("AreaTriggerFields > TimeToTargetExtraScale: " + e);
                }
                return 0;
            }
        }

        public uint SpellID
        {
            get
            {
                try
                {
                    return GetDescriptor<uint>(Descriptors.AreaTriggerFields.SpellID);
                }
                catch (Exception e)
                {
                    Logging.WriteError("AreaTriggerFields > SpellID: " + e);
                }
                return 0;
            }
        }

        public uint SpellVisualID
        {
            get
            {
                try
                {
                    return GetDescriptor<uint>(Descriptors.AreaTriggerFields.SpellVisualID);
                }
                catch (Exception e)
                {
                    Logging.WriteError("AreaTriggerFields > SpellVisualID: " + e);
                }
                return 0;
            }
        }

        public float BoundsRadius2D
        {
            get
            {
                try
                {
                    return GetDescriptor<float>(Descriptors.AreaTriggerFields.BoundsRadius2D);
                }
                catch (Exception e)
                {
                    Logging.WriteError("AreaTriggerFields > Radius: " + e);
                }
                return 0;
            }
        }

        public uint DecalPropertiesID
        {
            get
            {
                try
                {
                    return GetDescriptor<uint>(Descriptors.AreaTriggerFields.DecalPropertiesID);
                }
                catch (Exception e)
                {
                    Logging.WriteError("AreaTriggerFields > DecalPropertiesID: " + e);
                }
                return 0;
            }
        }

        private uint _faction;

        public uint Faction
        {
            get
            {
                try
                {
                    if (_faction == 0)
                    {
                        WoWObject objCaster = ObjectManager.GetObjectByGuid(Caster);
                        if (objCaster.Type == WoWObjectType.Unit && objCaster is WoWUnit)
                            _faction = (objCaster as WoWUnit).Faction;
                        if (objCaster.Type == WoWObjectType.GameObject && objCaster is WoWGameObject)
                            _faction = (objCaster as WoWGameObject).Faction;
                    }
                    return _faction;
                }
                catch (Exception e)
                {
                    Logging.WriteError("AreaTriggerFields > Faction: " + e);
                    return 0;
                }
            }
        }

        // NON WORKING
        public override Point Position
        {
            get
            {
                try
                {
                    /* CUSTOM
                    uint i = (uint)(Addresses.GameObject.GAMEOBJECT_FIELD_X + (-500));
                    while (false)
                    {
                        Logging.Write("New check, i=" + i);
                        var pt = new Point(
                            Memory.WowMemory.Memory.ReadFloat(BaseAddress + i),
                            Memory.WowMemory.Memory.ReadFloat(BaseAddress + i + 4),
                            Memory.WowMemory.Memory.ReadFloat(BaseAddress + i + 8));
                        if (pt.X > -12000 && pt.X < 12000 && pt.Y > -12000 && pt.Y < 12000 && pt.Z > -12000 && pt.Z < 12000)
                            Logging.Write(pt.ToString());
                        i += 4;
                        if (i > 0x1200)
                            break;
                    }

                    CUSTOM */
                    return
                        new Point(
                            Memory.WowMemory.Memory.ReadFloat(BaseAddress +
                                                              (uint) Addresses.GameObject.GAMEOBJECT_FIELD_X),
                            Memory.WowMemory.Memory.ReadFloat(BaseAddress +
                                                              (uint) Addresses.GameObject.GAMEOBJECT_FIELD_Y),
                            Memory.WowMemory.Memory.ReadFloat(BaseAddress +
                                                              (uint) Addresses.GameObject.GAMEOBJECT_FIELD_Z));
                }
                catch (Exception e)
                {
                    Logging.WriteError("AreaTriggerFields > Position: " + e);
                }
                return new Point();
            }
        }

        // NON WORKING
        public override string Name
        {
            get
            {
                try
                {
                    return
                        Memory.WowMemory.Memory.ReadUTF8String(
                            Memory.WowMemory.Memory.ReadUInt(
                                Memory.WowMemory.Memory.ReadUInt(BaseAddress + (uint) Addresses.GameObject.DBCacheRow) +
                                (uint) Addresses.GameObject.CachedName));
                }
                catch (Exception e)
                {
                    Logging.WriteError("AreaTriggerFields > Name: " + e);
                }
                return "";
            }
        }

        public T GetDescriptor<T>(Descriptors.AreaTriggerFields field) where T : struct
        {
            try
            {
                return GetDescriptor<T>((uint) field);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetDescriptor<T>(Descriptors.AreaTriggerFields field): " + e);
                return default(T);
            }
        }

        public override string ToString()
        {
            string retString = String.Format("DynamicObject: {0}BaseAddress: {1}, {0}Caster: {2}, {0}OverrideScaleCurve: {3}, {0}ExtraScaleCurve: {4}, {0}Duration: {5}, {0}TimeToTarget: {6}, {0}" +
                                             "{0}TimeToTargetScale: {7}, {0}TimeToTargetExtraScale: {8}, {0}SpellID: {9}, {0}SpellVisualID: {10}, {0}BoundsRadius2D: {11}, {0}Faction: {12}, {0}Name: {13}",
                Environment.NewLine, BaseAddress, Caster, OverrideScaleCurve, ExtraScaleCurve, Duration, TimeToTarget, TimeToTargetScale, TimeToTargetExtraScale, SpellID, SpellVisualID, BoundsRadius2D
                , Faction, Name);
            return retString;
        }
    }
}