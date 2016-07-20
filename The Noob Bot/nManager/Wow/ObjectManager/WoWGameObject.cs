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

        public new bool IsValid
        {
            get
            {
                try
                {
                    return Name != "" && base.IsValid;
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWGameObject > IsValid: " + e);
                    return false;
                }
            }
        }

        public UInt128 CreatedBy
        {
            get
            {
                try
                {
                    return GetDescriptor<UInt128>(Descriptors.GameObjectFields.CreatedBy);
                }
                catch (Exception e)
                {
                    Logging.WriteError("GameObjectFields > CreatedBy: " + e);
                }
                return 0;
            }
        }

        public uint DisplayId
        {
            get
            {
                try
                {
                    return GetDescriptor<uint>(Descriptors.GameObjectFields.DisplayID);
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

        public float Orientation
        {
            get
            {
                try
                {
                    Quaternion q = Rotations;
                    float angle = (float) System.Math.Atan2(0.0 + (q.X*q.Y + q.Z*q.W)*2.0, 1.0 - (q.Y*q.Y + q.Z*q.Z)*2.0);
                    return (angle < 0.0f ? angle + (float) (2*System.Math.PI) : angle);
                }
                catch (Exception e)
                {
                    Logging.WriteError("GameObject > Orientation: " + e);
                    return 0.0f;
                }
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

        public string IconName
        {
            get
            {
                try
                {
                    return
                        Memory.WowMemory.Memory.ReadUTF8String(
                            Memory.WowMemory.Memory.ReadUInt(
                                Memory.WowMemory.Memory.ReadUInt(BaseAddress + (uint) Addresses.GameObject.DBCacheRow) +
                                (uint) Addresses.GameObject.CachedIconName));
                }
                catch (Exception e)
                {
                    Logging.WriteError("GameObjectFields > IconName: " + e);
                }
                return "";
            }
        }

        public string CastBarCaption
        {
            get
            {
                try
                {
                    return
                        Memory.WowMemory.Memory.ReadUTF8String(
                            Memory.WowMemory.Memory.ReadUInt(
                                Memory.WowMemory.Memory.ReadUInt(BaseAddress + (uint) Addresses.GameObject.DBCacheRow) +
                                (uint) Addresses.GameObject.CachedCastBarCaption));
                }
                catch (Exception e)
                {
                    Logging.WriteError("GameObjectFields > CastBarCaption: " + e);
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

        public Quaternion Rotations
        {
            get
            {
                Int64 packedQuaternion = Memory.WowMemory.Memory.ReadInt64(BaseAddress +
                                                                           (uint) Addresses.GameObject.PackedRotationQuaternion);
                return new Quaternion(packedQuaternion);
            }
        }

        public Matrix4 WorldMatrix
        {
            get
            {
                Matrix4.MatrixColumn _x = (Matrix4.MatrixColumn) Memory.WowMemory.Memory.ReadObject(BaseAddress + (uint) Addresses.GameObject.TransformationMatrice, typeof (Matrix4.MatrixColumn));
                Matrix4.MatrixX X = new Matrix4.MatrixX(_x.m1, _x.m2, _x.m3, _x.m4);
                Matrix4.MatrixColumn _y = (Matrix4.MatrixColumn) Memory.WowMemory.Memory.ReadObject(BaseAddress + (uint) Addresses.GameObject.TransformationMatrice + 0x10, typeof (Matrix4.MatrixColumn));
                Matrix4.MatrixY Y = new Matrix4.MatrixY(_y.m1, _y.m2, _y.m3, _y.m4);
                Matrix4.MatrixColumn _z = (Matrix4.MatrixColumn) Memory.WowMemory.Memory.ReadObject(BaseAddress + (uint) Addresses.GameObject.TransformationMatrice + 0x20, typeof (Matrix4.MatrixColumn));
                Matrix4.MatrixZ Z = new Matrix4.MatrixZ(_z.m1, _z.m2, _z.m3, _z.m4);
                Matrix4.MatrixColumn _w = (Matrix4.MatrixColumn) Memory.WowMemory.Memory.ReadObject(BaseAddress + (uint) Addresses.GameObject.TransformationMatrice + 0x30, typeof (Matrix4.MatrixColumn));
                Matrix4.MatrixW W = new Matrix4.MatrixW(_w.m1, _w.m2, _w.m3, _w.m4);
                return new Matrix4(X, Y, Z, W);
            }
        }

        public float Size
        {
            get
            {
                try
                {
                    return
                        Memory.WowMemory.Memory.ReadFloat(
                            Memory.WowMemory.Memory.ReadUInt(BaseAddress + (uint) Addresses.GameObject.DBCacheRow) +
                            (uint) Addresses.GameObject.CachedSize);
                }
                catch (Exception e)
                {
                    Logging.WriteError("GameObject > Size: " + e);
                    return 0;
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
                    Memory.WowMemory.Memory.ReadUInt(
                        Memory.WowMemory.Memory.ReadUInt(BaseAddress + (uint) Addresses.GameObject.DBCacheRow) +
                        (uint) Addresses.GameObject.CachedQuestItem1 + (0x04*offset));
            }
            catch (Exception e)
            {
                Logging.WriteError("GameObject > QuestItem(" + offset + "): " + e);
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

        public uint Data(uint offset)
        {
            try
            {
                if (offset > 31)
                    return 0;
                return
                    Memory.WowMemory.Memory.ReadUInt(
                        Memory.WowMemory.Memory.ReadUInt(BaseAddress + (uint) Addresses.GameObject.DBCacheRow) +
                        (uint) Addresses.GameObject.CachedData0 + (0x04*offset));
            }
            catch (Exception e)
            {
                Logging.WriteError("GameObject > Data(" + offset + "): " + e);
                return 0;
            }
        }

        public uint Data0
        {
            get { return Data(0); }
        }

        public uint Data1
        {
            get { return Data(1); }
        }

        public uint Data2
        {
            get { return Data(2); }
        }

        public uint Data3
        {
            get { return Data(3); }
        }

        public uint Data4
        {
            get { return Data(4); }
        }

        public uint Data5
        {
            get { return Data(5); }
        }

        public uint Data6
        {
            get { return Data(6); }
        }

        public uint Data7
        {
            get { return Data(7); }
        }

        public uint Data8
        {
            get { return Data(8); }
        }

        public uint Data9
        {
            get { return Data(9); }
        }

        public uint Data10
        {
            get { return Data(10); }
        }

        public uint Data11
        {
            get { return Data(11); }
        }

        public uint Data12
        {
            get { return Data(12); }
        }

        public uint Data13
        {
            get { return Data(13); }
        }

        public uint Data14
        {
            get { return Data(14); }
        }

        public uint Data15
        {
            get { return Data(15); }
        }

        public uint Data16
        {
            get { return Data(16); }
        }

        public uint Data17
        {
            get { return Data(17); }
        }

        public uint Data18
        {
            get { return Data(18); }
        }

        public uint Data19
        {
            get { return Data(19); }
        }

        public uint Data20
        {
            get { return Data(20); }
        }

        public uint Data21
        {
            get { return Data(21); }
        }

        public uint Data22
        {
            get { return Data(22); }
        }

        public uint Data23
        {
            get { return Data(23); }
        }

        public uint Data24
        {
            get { return Data(24); }
        }

        public uint Data25
        {
            get { return Data(25); }
        }

        public uint Data26
        {
            get { return Data(26); }
        }

        public uint Data27
        {
            get { return Data(27); }
        }

        public uint Data28
        {
            get { return Data(28); }
        }

        public uint Data29
        {
            get { return Data(29); }
        }

        public uint Data30
        {
            get { return Data(30); }
        }

        public uint Data31
        {
            get { return Data(31); }
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
                        case WoWGameObjectType.SharedNodes: // 50
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
        public static Spell LoggingSpell;

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
                                if ((WoWGameObjectLockType) Row.Record.LockType[j] == WoWGameObjectLockType.LOCKTYPE_SAW_TREE)
                                {
                                    uint RankRequired = Row.Record.Skill[j];
                                    if (LoggingSpell == null)
                                        LoggingSpell = new Spell("Logging");
                                    if (!LoggingSpell.KnownSpell)
                                        return false;
                                    if (RankRequired == 3)
                                        return LoggingSpell.Id == 167947;
                                    if (RankRequired == 2)
                                        return LoggingSpell.Id == 167946 || LoggingSpell.Id == 167947;
                                    return true;
                                }
                                if ((WoWGameObjectLockType) Row.Record.LockType[j] == WoWGameObjectLockType.LOCKTYPE_OPEN_KNEELING)
                                {
                                    return nManagerSetting.CurrentSetting.ActivateVeinsHarvesting && Entry == 232541; // Add special support for WoD Garrison Mine Cart.
                                }
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

                                if (currentSkillLevel > 0 && currentSkillLevel >= reqSkillValue)
                                    return true;
                                if (currentSkillLevel > 0 && currentSkillLevel < reqSkillValue)
                                    return false;

                                // currentSkillLevel == 0 but we allow to farm in garrison.
                                return Garrison.GarrisonMapIdList.Contains(Usefuls.RealContinentId);
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
                    if (Entry == 210565 && (ObjectManager.Me.Level < 90 || Usefuls.IsCompletedAchievement(6552, true)))
                        return false;

                    if (Entry == 236256) // Odd Boulder
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