using System;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Patchables;
using nManager.Wow.Enums;

namespace nManager.Wow.ObjectManager
{
    public class WoWGameObject : WoWObject
    {
        public WoWGameObject(uint address)
            : base(address)
        {
        }

        public ulong CreatedBy
        {
            get
            {
                try { return GetDescriptor<ulong>((uint)Descriptors.GameObjectFields.createdBy); }
                catch (Exception e)
                {
                    Logging.WriteError("GameObjectFields > CreatedBy: " + e);
                }
                return 0;
            }
        }
        public int DisplayId
        {
            get
            {
                try { return GetDescriptor<int>(Descriptors.GameObjectFields.displayID); }
                catch (Exception e)
                {
                    Logging.WriteError("GameObjectFields > DisplayId: " + e);
                }
                return 0;
            }
        }
        public override Point Position
        {
            get
            {
                try { return new Point(Memory.WowMemory.Memory.ReadFloat(BaseAddress + (uint)Addresses.GameObject.GAMEOBJECT_FIELD_X), Memory.WowMemory.Memory.ReadFloat(BaseAddress + (uint)Addresses.GameObject.GAMEOBJECT_FIELD_Y), Memory.WowMemory.Memory.ReadFloat(BaseAddress + (uint)Addresses.GameObject.GAMEOBJECT_FIELD_Z)); }
                catch (Exception e)
                {
                    Logging.WriteError("GameObjectFields > Position: " + e);
                }
                return new Point();
            }
        }
        public override string Name
        {
            get
            {
                try { return Memory.WowMemory.Memory.ReadUTF8String(Memory.WowMemory.Memory.ReadUInt(Memory.WowMemory.Memory.ReadUInt(BaseAddress + (uint)Addresses.GameObject.DBCacheRow) + (uint)Addresses.GameObject.CachedName)); }
                catch (Exception e)
                {
                    Logging.WriteError("GameObjectFields > Name: " + e);
                }
                return "";
            }
        }
        public override float GetDistance
        {
            get
            {
                try { return Position.DistanceTo(ObjectManager.Me.Position); }
                catch (Exception e)
                {
                    Logging.WriteError("GameObjectFields > GetDistance: " + e);
                }
                return 0;
            }
        }
        public float GetDistance2D
        {
            get
            {
                try { return Position.DistanceTo2D(ObjectManager.Me.Position); }
                catch (Exception e)
                {
                    Logging.WriteError("GameObjectFields > GetDistance2D: " + e);
                }
                return 0;
            }
        }

        public float ParentRotation
        {
            get
            {
                try { return GetDescriptor<float>(Descriptors.GameObjectFields.parentRotation); }
                catch (Exception e)
                {
                    Logging.WriteError("GameObjectFields > ParentRotation: " + e);
                }
                return 0;
            }
        }

        public WoWGameObjectType GOType
        {
            get
            {
                try
                {
                    var bytes1 = GetDescriptor<Int32>(Descriptors.GameObjectFields.bytes_1);
                    return (WoWGameObjectType)((bytes1 >> 8) & 0xFF);
                }
                catch (Exception e)
                {
                    Logging.WriteError("GameObjectFields > Type: " + e);
                }
                return 0;
            }
        }

        public T GetDescriptor<T>(Descriptors.GameObjectFields field) where T : struct
        {
            try
            {
                return GetDescriptor<T>((uint)field);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetDescriptor<T>(Descriptors.GameObjectFields field): " + e);
                return default(T);
            }
        }
    }
}
