using System;
using System.Threading;
using System.Collections.Generic;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Patchables;
using nManager.Wow.Helpers;

namespace nManager.Wow.ObjectManager
{
    /// <summary>
    /// The base object class that all objects in WoW inherit.
    /// </summary>
    public class WoWObject
    {
        public WoWObject(uint address)
        {
            BaseAddress = address;
        }

        protected uint BaseAddress { private set; get; }

        public uint GetBaseAddress
        {
            get
            {
                try
                {
                    if (!IsValid)
                        return 0;
                    return BaseAddress;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWObject > GetBaseAddress: " + e);
                }
                return 0;
            }
        }

        public bool IsValid
        {
            get
            {
                try
                {
                    return BaseAddress != 0 && ObjectManager.ObjectDictionary.ContainsKey(Guid);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWObject > IsValid: " + e);
                    return false;
                }
            }
        }

        public bool UnitNearest
        {
            get
            {
                List<WoWUnit> units = ObjectManager.GetObjectWoWUnit();
                float i = 0;
                foreach (var iUnit in units)
                {
                    if (iUnit.IsAlive && iUnit.Position.DistanceTo2D(Position) <= (iUnit.AggroDistance + 1f) &&
                        UnitRelation.GetReaction(ObjectManager.Me, iUnit) == Reaction.Hostile)
                    {
                        if (iUnit.MaxHealth > ObjectManager.Me.MaxHealth/2)
                            i++;
                        else if (iUnit.MaxHealth > ObjectManager.Me.MaxHealth/10)
                            i += 0.5f;
                    } // else 0 for very small creatures
                }
                bool r = i > nManagerSetting.CurrentSetting.DontHarvestIfMoreThanXUnitInAggroRange;
                if (r)
                {
                    nManagerSetting.AddBlackList(Guid, 15*1000);
                    Logging.Write(i + " hostile Units Near " + Name);
                }
                return r;
            }
        }

        // These are simple descriptors, or other funcs that should be shared across objects.
        public UInt64 Guid
        {
            get
            {
                try
                {
                    if (BaseAddress > 0)
                        return GetDescriptor<UInt64>(Descriptors.ObjectFields.Guid);
                    return 0;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWObject > Guid: " + e);
                    return 0;
                }
            }
        }

        public WoWObjectType Type
        {
            get
            {
                try
                {
                    return (WoWObjectType) Memory.WowMemory.Memory.ReadInt(BaseAddress + (uint) Addresses.ObjectManager.objectTYPE);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWObject > Type: " + e);
                    return WoWObjectType.Object;
                }
            }
        }

        public int Entry
        {
            get
            {
                try
                {
                    return GetDescriptor<int>(Descriptors.ObjectFields.Entry);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWObject > Entry: " + e);
                    return 0;
                }
            }
        }

        public float Scale
        {
            get
            {
                try
                {
                    return GetDescriptor<float>(Descriptors.ObjectFields.Scale);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWObject > Scale: " + e);
                    return 0;
                }
            }
        }

        public virtual Point Position
        {
            get
            {
                try
                {
                    return new Point(0, 0, 0);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWObject > Position: " + e);
                    return new Point();
                }
            }
        }

        public virtual string Name
        {
            get
            {
                try
                {
                    return string.Empty;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWObject > Name: " + e);
                    return "";
                }
            }
        }

        public virtual float GetDistance
        {
            get
            {
                try
                {
                    return 0.0f;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWObject > GetDistance: " + e);
                    return 0;
                }
            }
        }

        internal void UpdateBaseAddress(uint address)
        {
            try
            {
                BaseAddress = address;
            }
            catch (Exception e)
            {
                Logging.WriteError("WoWObject > UpdateBaseAddress(uint address): " + e);
            }
        }

        internal T GetDescriptor<T>(Descriptors.ObjectFields field) where T : struct
        {
            try
            {
                // Makes life easier...
                return GetDescriptor<T>((uint) field);
            }
            catch (Exception e)
            {
                Logging.WriteError("WoWObject > GetDescriptor<T>(Descriptors.ObjectFields field): " + e);
                return default(T);
            }
        }

        internal T GetDescriptor<T>(uint field) where T : struct
        {
            try
            {
                return GetDescriptor<T>(BaseAddress, field);
            }
            catch (Exception e)
            {
                Logging.WriteError("WoWObject > GetDescriptor<T>(uint field): " + e);
                return default(T);
            }
        }

        internal T GetDescriptor<T>(uint baseAddress, uint field) where T : struct
        {
            try
            {
                object ret = null;

                if (baseAddress > 0)
                {
                    uint descriptorsArray = Memory.WowMemory.Memory.ReadUInt(baseAddress + Descriptors.startDescriptors);
                    uint addressGD = descriptorsArray + (field*Descriptors.multiplicator);

                    if (typeof (T) == typeof (string))
                    {
                        string retTemp = "";
                        byte[] buf = Memory.WowMemory.Memory.ReadBytes(addressGD, 1);
                        while (buf[0] != 0)
                        {
                            retTemp = retTemp + Convert.ToChar(buf[0]);
                            addressGD = addressGD + 1;
                            buf = Memory.WowMemory.Memory.ReadBytes(addressGD, 1);
                            //Thread.Sleep(1);
                        }
                        ret = retTemp;
                        return (T) ret;
                    }

                    if (typeof (T) == typeof (ulong))
                    {
                        ret = Memory.WowMemory.Memory.ReadUInt64(addressGD);
                        return (T) ret;
                    }

                    switch (System.Type.GetTypeCode(typeof (T)))
                    {
                        case TypeCode.Boolean:
                            ret = (Memory.WowMemory.Memory.ReadShort(addressGD) >= 0);
                            break;
                        case TypeCode.Char:
                            ret = (char) Memory.WowMemory.Memory.ReadShort(addressGD);
                            break;
                        case TypeCode.Byte:
                            ret = Memory.WowMemory.Memory.ReadByte(addressGD);
                            break;
                        case TypeCode.Int16:
                            ret = Memory.WowMemory.Memory.ReadShort(addressGD);
                            break;
                        case TypeCode.UInt16:
                            ret = Memory.WowMemory.Memory.ReadUShort(addressGD);
                            break;
                        case TypeCode.Int32:
                            ret = Memory.WowMemory.Memory.ReadInt(addressGD);
                            break;
                        case TypeCode.UInt32:
                            ret = Memory.WowMemory.Memory.ReadUInt(addressGD);
                            break;
                        case TypeCode.Int64:
                            ret = Memory.WowMemory.Memory.ReadInt64(addressGD);
                            break;
                        case TypeCode.UInt64:
                            ret = Memory.WowMemory.Memory.ReadUInt64(addressGD);
                            break;
                        case TypeCode.Single:
                            ret = Memory.WowMemory.Memory.ReadFloat(addressGD);
                            break;
                        case TypeCode.Double:
                            ret = Memory.WowMemory.Memory.ReadDouble(addressGD);
                            break;
                    }
                }
                else
                    return default(T);
                if (ret != null)
                {
                    return (T) ret;
                }
                Logging.WriteError("WoWObject > GetDescriptor<T>(uint baseAddress, uint field): Value not found");
                return default(T);
            }
            catch (Exception e)
            {
                Logging.WriteError("WoWObject > GetDescriptor<T>(uint baseAddress, uint field): " + e);
                return default(T);
            }
        }
    }
}