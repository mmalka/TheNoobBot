using System;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Patchables;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;

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

        public uint Data0
        {
            get
            {
                try { return Memory.WowMemory.Memory.ReadUInt(Memory.WowMemory.Memory.ReadUInt(BaseAddress + (uint)Addresses.GameObject.DBCacheRow) + (uint)Addresses.GameObject.CachedData0); }
                catch (Exception e)
                {
                    Logging.WriteError("GameObject > data0: " + e);
                }
                return 0;
            }
        }
        public uint Data1
        {
            get
            {
                try { return Memory.WowMemory.Memory.ReadUInt(Memory.WowMemory.Memory.ReadUInt(BaseAddress + (uint)Addresses.GameObject.DBCacheRow) + (uint)Addresses.GameObject.CachedData1); }
                catch (Exception e)
                {
                    Logging.WriteError("GameObject > data1: " + e);
                }
                return 0;
            }
        }

        public uint Data8
        {
            get
            {
                try { return Memory.WowMemory.Memory.ReadUInt(Memory.WowMemory.Memory.ReadUInt(BaseAddress + (uint)Addresses.GameObject.DBCacheRow) + (uint)Addresses.GameObject.CachedData8); }
                catch (Exception e)
                {
                    Logging.WriteError("GameObject > data8: " + e);
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

        public uint LockEntry
        {
            get
            {
                try
                {
                    switch (GOType)
                    {
                        case WoWGameObjectType.Door:        // 0
                        case WoWGameObjectType.Button:      // 1
                            return Data1;
                        case WoWGameObjectType.Questgiver:  // 2
                        case WoWGameObjectType.Chest:       // 3
                        case WoWGameObjectType.Trap:        // 6  This lock is generaly a check for DISARM_TRAP capacity
                        case WoWGameObjectType.Goober:      // 10
                        case WoWGameObjectType.FlagStand:   // 24
                        case WoWGameObjectType.FlagDrop:    // 26
                            return Data0;
                        default:
                            return 0;
                    }
                }
                catch (Exception e)
                {
                    Logging.WriteError("GameObjectFields > LockType: " + e);
                }
                return 0;
            }
        }

        private SkillLine SkillByLockType(WoWGameObjectLockType lType)
        {
            switch (lType)
            {
                case WoWGameObjectLockType.LOCKTYPE_PICKLOCK:
                    return SkillLine.Lockpicking;
                case WoWGameObjectLockType.LOCKTYPE_HERBALISM:
                    return SkillLine.Herbalism;
                case WoWGameObjectLockType.LOCKTYPE_MINING:
                    return SkillLine.Mining;
                case WoWGameObjectLockType.LOCKTYPE_FISHING:
                    return SkillLine.Fishing;
                case WoWGameObjectLockType.LOCKTYPE_INSCRIPTION:
                    return SkillLine.Inscription;
                //case WoWGameObjectLockType.LOCKTYPE_DISARM_TRAP:
                //    return SkillLine.Lockpicking;
                default: break;
            }
            return SkillLine.None;
        }

        public bool CanOpen
        {
            get
            {
                if (LockEntry != 0)
                {
                    WoWLock Row = WoWLock.FromId(LockEntry);
                    if (Row.Record.KeyType == null)
                        return false;

                    for (int j = 0; j < 8; j++)
                    {
                        switch ((WoWGameObjectLockKeyType)Row.Record.KeyType[j])
                        {
                            case WoWGameObjectLockKeyType.LOCK_KEY_NONE:
                                break;

                            case WoWGameObjectLockKeyType.LOCK_KEY_ITEM: // Do we have the key(s) ?
                                uint itemId = Row.Record.LockType[j];
                                if (ItemsManager.GetItemCountByIdLUA(itemId) < 0)
                                    return false;
                                break;

                            case WoWGameObjectLockKeyType.LOCK_KEY_SKILL: // Do we have the skill ?
                                SkillLine skill = SkillByLockType((WoWGameObjectLockType)Row.Record.LockType[j]);
                                if (skill == SkillLine.None) // Lock Type unsupported by now
                                    return false;

                                // Prevent herbing when the setting is off
                                if (skill == SkillLine.Herbalism && !nManagerSetting.CurrentSetting.harvestHerbs)
                                    return false;
                                // Prevent mining when the setting is off
                                if (skill == SkillLine.Mining && !nManagerSetting.CurrentSetting.harvestMinerals)
                                    return false;

                                uint reqSkillValue = Row.Record.Skill[j];

                                if (skill == SkillLine.Lockpicking && !nManagerSetting.CurrentSetting.lootChests)
                                    return false;

                                // special case for rogues and lockpicking since the skill does not exist anymore
                                if (skill == SkillLine.Lockpicking)
                                {
                                    Spell lockpick = new Spell(1804); // Pick lock
                                    if (lockpick.KnownSpell && ObjectManager.Me.Level * 5 >= reqSkillValue)
                                        return true;
                                    else
                                        return false;
                                }

                                //Logging.Write("Requires " + skill + " level " + reqSkillValue);
                                if (Skill.GetValue(skill) < reqSkillValue)
                                    return false;

                                return true;

                            default:
                                break;
                        }
                    }
                }
                // No lock = no gathering GameObject, then obey to lootChests setting
                if (!nManagerSetting.CurrentSetting.lootChests)
                    return false;

                // Finaly we check is a quest is required
                if (GOType == WoWGameObjectType.Goober)// && Data1 != 0)
                {
                    //Logging.Write("This Goober has quest " + Data1);
                    //if (!Quest.GetLogQuestId().Contains((int)Data1))
                        return false;
                }
                if (GOType == WoWGameObjectType.Chest && Data8 != 0)
                {
                    if (!Quest.GetLogQuestId().Contains((int)Data8))
                        return false;
                }
                return true;
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
