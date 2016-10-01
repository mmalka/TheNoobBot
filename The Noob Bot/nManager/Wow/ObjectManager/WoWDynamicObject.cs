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
    public class WoWDynamicObject : WoWObject
    {
        public WoWDynamicObject(uint address)
            : base(address)
        {
        }

        public new bool IsValid
        {
            get
            {
                try
                {
                    return SpellID > 0 && Radius > 0 && base.IsValid;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWGameObject > IsValid: " + e);
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
                    return GetDescriptor<UInt128>(Descriptors.DynamicObjectFields.Caster);
                }
                catch (Exception e)
                {
                    Logging.WriteError("DynamicObjectFields > Caster: " + e);
                }
                return 0;
            }
        }

        public uint TypeAndVisualID
        {
            get
            {
                try
                {
                    return GetDescriptor<uint>(Descriptors.DynamicObjectFields.TypeAndVisualID);
                }
                catch (Exception e)
                {
                    Logging.WriteError("GameObjectFields > TypeAndVisualID: " + e);
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
                    return GetDescriptor<uint>(Descriptors.DynamicObjectFields.SpellID);
                }
                catch (Exception e)
                {
                    Logging.WriteError("GameObjectFields > SpellID: " + e);
                }
                return 0;
            }
        }

        public float Radius
        {
            get
            {
                try
                {
                    return GetDescriptor<float>(Descriptors.DynamicObjectFields.Radius);
                }
                catch (Exception e)
                {
                    Logging.WriteError("GameObjectFields > Radius: " + e);
                }
                return 0;
            }
        }

        public uint CastTime
        {
            get
            {
                try
                {
                    return GetDescriptor<uint>(Descriptors.DynamicObjectFields.CastTime);
                }
                catch (Exception e)
                {
                    Logging.WriteError("GameObjectFields > CastTime: " + e);
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
                    Logging.WriteError("WoWGameObject > Faction: " + e);
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
                    Logging.WriteError("GameObjectFields > Position: " + e);
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
                    Logging.WriteError("GameObjectFields > Name: " + e);
                }
                return "";
            }
        }

        public T GetDescriptor<T>(Descriptors.DynamicObjectFields field) where T : struct
        {
            try
            {
                return GetDescriptor<T>((uint) field);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetDescriptor<T>(Descriptors.DynamicObjectFields field): " + e);
                return default(T);
            }
        }

        public override string ToString()
        {
            string retString = String.Format("DynamicObject: {0}BaseAddress: {1}, {0}Caster: {2}, {0}TypeAndVisualID: {3}, {0}SpellID: {4}, {0}Radius: {5}, {0}CastTime: {6}, {0}", Environment.NewLine, BaseAddress,
                Caster, TypeAndVisualID, SpellID, Radius, CastTime);
            return retString;
        }
    }
}