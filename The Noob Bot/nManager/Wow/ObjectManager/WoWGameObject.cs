using System;
using System.Linq;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Patchables;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;

namespace nManager.Wow.ObjectManager
{
    public class WoWGameObject : WoWObject
    {
        public enum GameObjectFlags
        {
            GO_FLAG_IN_USE = 0x00000001, //disables interaction while animated
            GO_FLAG_LOCKED = 0x00000002, //require key, spell, event, etc to be opened. Makes "Locked" appear in tooltip
            GO_FLAG_INTERACT_COND = 0x00000004, //cannot interact (condition to interact)
            GO_FLAG_TRANSPORT = 0x00000008, //any kind of transport? Object can transport (elevator, boat, car)
            GO_FLAG_NO_INTERACT = 0x00000010, //players cannot interact with this go
            GO_FLAG_NODESPAWN = 0x00000020, //never despawn, typically for doors, they just change state
            GO_FLAG_TRIGGERED = 0x00000040, //typically, summoned objects. Triggered by spell or other events
        };

        public WoWGameObject(uint address)
            : base(address)
        {
        }

        public ulong CreatedBy
        {
            get
            {
                try
                {
                    return GetDescriptor<ulong>(Descriptors.GameObjectFields.CreatedBy);
                }
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
                try
                {
                    return GetDescriptor<int>(Descriptors.GameObjectFields.DisplayID);
                }
                catch (Exception e)
                {
                    Logging.WriteError("GameObjectFields > DisplayId: " + e);
                }
                return 0;
            }
        }

        public uint Faction
        {
            get
            {
                try
                {
                    return GetDescriptor<uint>(Descriptors.GameObjectFields.FactionTemplate);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWGameObject > Faction: " + e);
                    return 0;
                }
            }
        }

        public override Point Position
        {
            get
            {
                try
                {
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
                    Logging.WriteError("GameObjectFields > GetDistance: " + e);
                }
                return 0;
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
                    Logging.WriteError("GameObjectFields > GetDistance2D: " + e);
                }
                return 0;
            }
        }

        public float ParentRotation
        {
            get
            {
                try
                {
                    return GetDescriptor<float>(Descriptors.GameObjectFields.ParentRotation);
                }
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
                try
                {
                    return
                        Memory.WowMemory.Memory.ReadUInt(
                            Memory.WowMemory.Memory.ReadUInt(BaseAddress + (uint) Addresses.GameObject.DBCacheRow) +
                            (uint) Addresses.GameObject.CachedData0);
                }
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
                try
                {
                    return
                        Memory.WowMemory.Memory.ReadUInt(
                            Memory.WowMemory.Memory.ReadUInt(BaseAddress + (uint) Addresses.GameObject.DBCacheRow) +
                            (uint) Addresses.GameObject.CachedData1);
                }
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
                try
                {
                    return
                        Memory.WowMemory.Memory.ReadUInt(
                            Memory.WowMemory.Memory.ReadUInt(BaseAddress + (uint) Addresses.GameObject.DBCacheRow) +
                            (uint) Addresses.GameObject.CachedData8);
                }
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
                    int bytes1 = GetDescriptor<Int32>(Descriptors.GameObjectFields.PercentHealth);
                    return (WoWGameObjectType) ((bytes1 >> 8) & 0xFF);
                }
                catch (Exception e)
                {
                    Logging.WriteError("GameObjectFields > Type: " + e);
                }
                return 0;
            }
        }

        public GameObjectFlags GOFlags
        {
            get
            {
                try
                {
                    return GetDescriptor<GameObjectFlags>(Descriptors.GameObjectFields.Flags);
                }
                catch (Exception e)
                {
                    Logging.WriteError("GameObjectFields > Flags: " + e);
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
                        case WoWGameObjectType.Door: // 0
                        case WoWGameObjectType.Button: // 1
                            return Data1;
                        case WoWGameObjectType.Questgiver: // 2
                        case WoWGameObjectType.Chest: // 3
                        case WoWGameObjectType.Trap: // 6  This lock is generaly a check for DISARM_TRAP capacity
                        case WoWGameObjectType.Goober: // 10
                        case WoWGameObjectType.FlagStand: // 24
                        case WoWGameObjectType.FlagDrop: // 26
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
                case WoWGameObjectLockType.LOCKTYPE_OPEN: // 5
                case WoWGameObjectLockType.LOCKTYPE_TREASURE: // 6
                    return SkillLine.Free;
                case WoWGameObjectLockType.LOCKTYPE_QUICK_OPEN: // 10
                case WoWGameObjectLockType.LOCKTYPE_OPEN_TINKERING: // 12
                case WoWGameObjectLockType.LOCKTYPE_OPEN_KNEELING: // 13
                    return SkillLine.None;
                    //case WoWGameObjectLockType.LOCKTYPE_DISARM_TRAP:
                    //    return SkillLine.Lockpicking;
            }
            return SkillLine.None;
        }

        public bool IsHerb { get; set; }

        public bool CanOpen
        {
            get
            {
                if (GOFlags.HasFlag(GameObjectFlags.GO_FLAG_IN_USE) ||
                    GOFlags.HasFlag(GameObjectFlags.GO_FLAG_INTERACT_COND))
                    return false;

                if (nManagerSetting.CurrentSetting.DontHarvestTheFollowingObjects.Count > 0)
                {
                    int entryid = 0;
                    if (
                        nManagerSetting.CurrentSetting.DontHarvestTheFollowingObjects.Where(
                            entry => int.TryParse(entry.Trim(), out entryid)).Any(entry => Entry == entryid))
                    {
                        return false;
                    }
                }
                if (LockEntry != 0)
                {
                    WoWLock Row = WoWLock.FromId(LockEntry);
                    if (Row.Record.KeyType == null)
                        return false;

                    for (int j = 0; j < 8; j++)
                    {
                        switch ((WoWGameObjectLockKeyType) Row.Record.KeyType[j])
                        {
                            case WoWGameObjectLockKeyType.LOCK_KEY_NONE:
                                break;

                            case WoWGameObjectLockKeyType.LOCK_KEY_ITEM: // Do we have the key(s) ?
                                int itemId = (int) Row.Record.LockType[j];
                                if (ItemsManager.GetItemCount(itemId) < 0)
                                    return false;
                                break;

                            case WoWGameObjectLockKeyType.LOCK_KEY_SKILL: // Do we have the skill ?
                                SkillLine skill = SkillByLockType((WoWGameObjectLockType) Row.Record.LockType[j]);
                                if (skill == SkillLine.None) // Lock Type unsupported by now
                                {
                                    //Logging.WriteDebug("GameObject \"" + Name + "\" (ID " + Entry + ", Type " + GOType + (GOType == WoWGameObjectType.Goober ? ", Quest: " + Data1 : "") + (GOType == WoWGameObjectType.Chest ? ", Quest: " + Data8 : "") + ", Lock " + LockEntry + ") has a SKILL LockType " + Row.Record.LockType[j] + " which is not supported");
                                    return false;
                                }
                                // Most of quest chests but also treasures
                                if (skill == SkillLine.Free)
                                    break;
                                // let's accept it, we check for quest later in code and act like if no lock was set

                                // Prevent herbing when the setting is off
                                if (skill == SkillLine.Herbalism &&
                                    !nManagerSetting.CurrentSetting.ActivateHerbsHarvesting)
                                    return false;

                                // Prevent mining when the setting is off
                                if (skill == SkillLine.Mining && !nManagerSetting.CurrentSetting.ActivateVeinsHarvesting)
                                    return false;

                                uint reqSkillValue = Row.Record.Skill[j];

                                if (skill == SkillLine.Lockpicking &&
                                    !nManagerSetting.CurrentSetting.ActivateChestLooting)
                                    return false;

                                // special case for rogues and lockpicking since the skill does not exist anymore
                                if (skill == SkillLine.Lockpicking)
                                {
                                    Spell lockpick = new Spell(1804); // Pick lock
                                    if (lockpick.KnownSpell && ObjectManager.Me.Level*5 >= reqSkillValue)
                                        return true;
                                    return false;
                                }

                                int currentSkillLevel = Skill.GetValue(skill);
                                if (currentSkillLevel != 0)
                                    currentSkillLevel += Skill.GetSkillBonus(skill);
                                //Logging.Write("Requires " + skill + " level " + reqSkillValue + " I have " + currentSkillLevel);
                                IsHerb = skill == SkillLine.Herbalism;
                                return currentSkillLevel > 0 && currentSkillLevel >= reqSkillValue;
                        }
                    }
                }
                // No lock = no gathering GameObject, then obey to lootChests setting or all lock passed
                if (!nManagerSetting.CurrentSetting.ActivateChestLooting)
                    return false;

                // Finaly we check if a quest is required
                if (GOType == WoWGameObjectType.Goober) // && Data1 != 0)
                {
                    //Logging.Write("This Goober has quest " + Data1);
                    //if (!Quest.GetLogQuestId().Contains((int)Data1))
                    return false; // Refuse Goober, they never have the required data
                }
                if (GOType == WoWGameObjectType.Chest)
                {
                    if (Data8 != 0 && !Quest.GetLogQuestId().Contains((int) Data8))
                        return false; // Quest check

                    // Refuse Dark Soil if we are below level 90 or we comleted achievement "Friends on the Farm"
                    if (Entry == 210565 && (ObjectManager.Me.Level < 90 || Usefuls.IsCompletedAchievement(6552)))
                        return false;

                    //if (Entry == 214945) // Onyx Egg. Maybe we should disabled them when exalted with Cloud Serpents
                    //    return false;
                }
                return true;
            }
        }

        public T GetDescriptor<T>(Descriptors.GameObjectFields field) where T : struct
        {
            try
            {
                return GetDescriptor<T>((uint) field);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetDescriptor<T>(Descriptors.GameObjectFields field): " + e);
                return default(T);
            }
        }
    }
}