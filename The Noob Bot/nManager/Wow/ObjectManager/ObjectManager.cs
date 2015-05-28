using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using nManager.Helpful;
using nManager.Wow.Bot.States;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.Patchables;
using System.Collections.Concurrent;
using SlimDX.Direct3D9;

namespace nManager.Wow.ObjectManager
{
    public static class ObjectManager
    {
        private static readonly object Locker = new object();
        private static uint _lastTargetBase;
        // All Objects except Units are in _objectList
        private static List<WoWObject> _objectList;
        // Units and Gameobjects are in separate lists
        private static List<WoWUnit> _unitList;
        private static List<WoWGameObject> _gameobjectList;
        public static List<UInt128> BlackListMobAttack = new List<UInt128>();

        static ObjectManager()
        {
            try
            {
                ObjectDictionary = new ConcurrentDictionary<UInt128, WoWObject>();
                Me = new WoWPlayer(0);
            }
            catch (Exception e)
            {
                Logging.WriteError("ObjectManager(): " + e);
            }
        }

        private static uint ObjectManagerAddress { get; set; }

        public static List<WoWObject> ObjectList
        {
            get
            {
                lock (Locker)
                    return _objectList.ToList();
            }
        }

        public static ConcurrentDictionary<UInt128, WoWObject> ObjectDictionary { get; set; }
        public static WoWPlayer Me { get; private set; }

        public static WoWUnit Target
        {
            get
            {
                try
                {
                    if (Me.IsValid)
                    {
                        if (Me.Target > 0)
                        {
                            if (_lastTargetBase > 0)
                                if (new WoWUnit(_lastTargetBase).Guid == Me.Target)
                                {
                                    return new WoWUnit(_lastTargetBase);
                                }
                                else
                                {
                                    _lastTargetBase = 0;
                                }
                            WoWUnit t = new WoWUnit(GetObjectByGuid(Me.Target).GetBaseAddress);
                            _lastTargetBase = t.GetBaseAddress;
                            return t;
                        }
                    }
                }
                catch (Exception e)
                {
                    Logging.WriteError("Target: " + e);
                }
                return new WoWUnit(0);
            }
        }

        public static WoWUnit Pet
        {
            get
            {
                try
                {
                    UInt128 guidPet = Memory.WowMemory.Memory.ReadUInt128(Memory.WowProcess.WowModule + (uint) Addresses.Player.petGUID);
                    if (guidPet > 0)
                        return new WoWUnit(GetObjectByGuid(guidPet).GetBaseAddress);

                    // Now let's try to find other "pets" like Chaman Totems for example
                    WoWUnit u = GetWoWUnitSummonedOrCreatedByMeAndFighting();
                    if (u.IsValid)
                        return u;

                    return new WoWUnit(0);
                }
                catch (Exception e)
                {
                    Logging.WriteError("WoWUnit Pet: " + e);
                    return new WoWUnit(0);
                }
            }
        }

        internal static void Pulse()
        {
            try
            {
                lock (Locker)
                {
                    // Remove invalid objects.
                    foreach (KeyValuePair<UInt128, WoWObject> o in ObjectDictionary)
                    {
                        o.Value.UpdateBaseAddress(0);
                    }

                    // Fill the new list.
                    ReadObjectList();

                    // Clear out old references.
                    List<UInt128> toRemove = new List<UInt128>();
                    _objectList = new List<WoWObject>();
                    _unitList = new List<WoWUnit>();
                    _gameobjectList = new List<WoWGameObject>();
                    foreach (KeyValuePair<UInt128, WoWObject> o in ObjectDictionary)
                    {
                        if (o.Value.IsValid)
                        {
                            switch (o.Value.Type)
                            {
                                case WoWObjectType.Unit:
                                    _unitList.Add(new WoWUnit(o.Value.GetBaseAddress));
                                    break;
                                case WoWObjectType.GameObject:
                                    _gameobjectList.Add(new WoWGameObject(o.Value.GetBaseAddress));
                                    break;
                                default:
                                    _objectList.Add(o.Value);
                                    break;
                            }
                        }
                        else
                            toRemove.Add(o.Key);
                    }

                    // All done! Just make sure we pass up a valid list to the ObjectList.
                    foreach (UInt128 guid in toRemove)
                    {
                        WoWObject object1;
                        ObjectDictionary.TryRemove(guid, out object1);
                    }
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("ObjectManager > Pulse(): " + e);
            }
        }

        internal static void ReadObjectList()
        {
            try
            {
                // Make sure we have a valid address for the objmgr.
                while (Addresses.ObjectManagerClass.clientConnection == 0)
                    Thread.Sleep(10);

                ObjectManagerAddress =
                    Memory.WowMemory.Memory.ReadUInt(Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + Addresses.ObjectManagerClass.clientConnection) + (uint) Addresses.ObjectManager.objectManager);
                UInt128 localPlayerGuid = Memory.WowMemory.Memory.ReadUInt128(ObjectManagerAddress + (uint) Addresses.ObjectManager.localGuid);
                Usefuls.ContinentId = Memory.WowMemory.Memory.ReadInt(ObjectManagerAddress + (uint) Addresses.ObjectManager.continentId);
                // Get the first object in the linked list.
                int currentObject = Memory.WowMemory.Memory.ReadInt(ObjectManagerAddress + (uint) Addresses.ObjectManager.firstObject);

                while (currentObject != 0)
                {
                    try
                    {
                        UInt128 objGuid = Memory.WowMemory.Memory.ReadUInt128((uint) currentObject + (uint) Addresses.ObjectManager.objectGUID);
                        if (!ObjectDictionary.ContainsKey(objGuid))
                        {
                            WoWObjectType objType = (WoWObjectType) Memory.WowMemory.Memory.ReadInt((uint) currentObject + (uint) Addresses.ObjectManager.objectTYPE);

                            WoWObject obj = null;
                            // Add the object based on it's *actual* type. Note: WoW's Object descriptors for OBJECT_FIELD_TYPE
                            // is a bitmask. We want to use the type at 0x14, as it's an 'absolute' type.

                            switch (objType)
                            {
                                    // Belive it or not, the base Object class is hardly used in WoW.
                                case WoWObjectType.Object:
                                    obj = new WoWObject((uint) currentObject);
                                    break;
                                case WoWObjectType.Item:
                                    obj = new WoWItem((uint) currentObject);
                                    break;
                                case WoWObjectType.Container:
                                    obj = new WoWContainer((uint) currentObject);
                                    break;
                                case WoWObjectType.Unit:
                                    obj = new WoWUnit((uint) currentObject);
                                    break;
                                case WoWObjectType.Player:
                                    // Keep the static reference to the local player updated... at all times.
                                    if (localPlayerGuid == objGuid)
                                    {
                                        Me.UpdateBaseAddress((uint) currentObject);
                                    }
                                    obj = new WoWPlayer((uint) currentObject);
                                    break;
                                case WoWObjectType.GameObject:
                                    obj = new WoWGameObject((uint) currentObject);
                                    break;
                                case WoWObjectType.DynamicObject:
                                    obj = new WoWGameObject((uint) currentObject);
                                    break;
                                case WoWObjectType.Corpse:
                                    obj = new WoWCorpse((uint) currentObject);
                                    break;
                                    // These two aren't used in most bots, as they're fairly pointless.
                                    // They are AI and area triggers for NPCs handled by the client itself.
                                case WoWObjectType.AiGroup:
                                case WoWObjectType.AreaTrigger:
                                    break;
                                    /*default:
                                    Logging.Write("GUID: " + objGuid);
                                    Logging.Write("TYPE: " + objType);
                                    Logging.Write("GetWoWId: " + objGuid.GetWoWId);
                                    Logging.Write("GetWoWMapId: " + objGuid.GetWoWMapId);
                                    Logging.Write("GetWoWRealmId: " + objGuid.GetWoWRealmId);
                                    Logging.Write("GetWoWServerId: " + objGuid.GetWoWServerId);
                                    Logging.Write("GetWoWSubType: " + objGuid.GetWoWSubType);
                                    Logging.Write("GetWoWType: " + objGuid.GetWoWType);
                                    break;*/
                            }
                            if (obj != null)
                            {
                                // We have a valid object that isn't in the object list already.
                                // So lets add it.
                                ObjectDictionary.TryAdd(objGuid, obj);
                            }
                        }
                        else
                        {
                            // The object already exists, just update the pointer.
                            ObjectDictionary[objGuid].UpdateBaseAddress((uint) currentObject);
                        }
                    }
                    catch (Exception e)
                    {
                        Logging.WriteError("ObjectManager >  ReadObjectList()#1: " + e);
                    }
                    // We need the next object.
                    Int32 currentObjectNew = Memory.WowMemory.Memory.ReadInt((uint) currentObject + (uint) Addresses.ObjectManager.nextObject);
                    if (currentObjectNew == currentObject)
                    {
                        break;
                    }
                    currentObject = currentObjectNew;
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("ObjectManager >  ReadObjectList()#2: " + e);
            }
        }

        // Management Object
        public static WoWObject GetObjectByGuid(UInt128 guid)
        {
            try
            {
                lock (Locker)
                    return ObjectDictionary.ContainsKey(guid) ? ObjectDictionary[guid] : new WoWObject(0);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetObjectByGuid(UInt128 guid): " + e);
                return new WoWObject(0);
            }
        }

        // Make all base list construction fast
        public static List<WoWUnit> GetObjectWoWUnit()
        {
            try
            {
                lock (Locker)
                    return _unitList.ToList();
            }
            catch (Exception e)
            {
                Logging.WriteError("GetObjectWoWUnit(): " + e);
                return new List<WoWUnit>();
            }
        }

        public static List<WoWUnit> GetObjectWoWUnitInCombat()
        {
            try
            {
                List<WoWUnit> result = new List<WoWUnit>();
                foreach (WoWUnit u in GetObjectWoWUnit())
                    if (u.InCombat && u.Attackable) result.Add(u);
                return result;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetObjectWoWUnitInCombat(): " + e);
                return new List<WoWUnit>();
            }
        }

        public static List<WoWContainer> GetObjectWoWContainer()
        {
            try
            {
                List<WoWContainer> result = new List<WoWContainer>();
                foreach (WoWObject o in ObjectList)
                    if (o.Type == WoWObjectType.Container) result.Add(new WoWContainer(o.GetBaseAddress));
                return result;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetObjectWoWContainer(): " + e);
                return new List<WoWContainer>();
            }
        }

        public static List<WoWCorpse> GetObjectWoWCorpse()
        {
            try
            {
                List<WoWCorpse> result = new List<WoWCorpse>();
                foreach (WoWObject o in ObjectList)
                    if (o.Type == WoWObjectType.Corpse) result.Add(new WoWCorpse(o.GetBaseAddress));
                return result;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetObjectWoWCorpse(): " + e);
                return new List<WoWCorpse>();
            }
        }

        public static List<WoWItem> GetObjectWoWItem()
        {
            try
            {
                List<WoWItem> result = new List<WoWItem>();
                foreach (WoWObject o in ObjectList)
                    if (o.Type == WoWObjectType.Item) result.Add(new WoWItem(o.GetBaseAddress));
                return result;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetObjectWoWItem(): " + e);
                return new List<WoWItem>();
            }
        }

        public static WoWItem GetWoWItemById(int entry)
        {
            try
            {
                List<WoWItem> itemList = GetObjectWoWItem();
                foreach (WoWItem i in itemList)
                    if (i.Entry == entry)
                        return new WoWItem(i.GetBaseAddress);
                return new WoWItem(0);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWItemById(int entry): " + e);
                return new WoWItem(0);
            }
        }

        public static List<WoWGameObject> GetWoWGameObjectOfType(WoWGameObjectType reqtype)
        {
            try
            {
                List<WoWGameObject> result = new List<WoWGameObject>();
                lock (Locker)
                {
                    foreach (WoWGameObject go in _gameobjectList)
                    {
                        if (go.GOType == reqtype)
                            result.Add(go);
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWGameObjectOfType(WoWGameObjectType reqtype): " + e);
                return new List<WoWGameObject>();
            }
        }

        public static List<WoWGameObject> GetObjectWoWGameObject()
        {
            try
            {
                lock (Locker)
                    return _gameobjectList.ToList();
            }
            catch (Exception e)
            {
                Logging.WriteError("GetObjectWoWGameObject(): " + e);
                return new List<WoWGameObject>();
            }
        }

        public static WoWPlayer GetObjectWoWPlayer(UInt128 guid)
        {
            try
            {
                foreach (WoWObject o in ObjectList)
                    if (o.Type == WoWObjectType.Player && o.Guid == guid)
                        return new WoWPlayer(o.GetBaseAddress);
                return null;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetObjectWoWPlayer(UInt128 guid): " + e);
                return null;
            }
        }

        public static List<WoWPlayer> GetObjectWoWPlayer()
        {
            try
            {
                List<WoWPlayer> result = new List<WoWPlayer>();
                foreach (WoWObject o in ObjectList)
                    if (o.Type == WoWObjectType.Player && o.GetBaseAddress != Me.GetBaseAddress)
                        result.Add(new WoWPlayer(o.GetBaseAddress));
                return result;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetObjectWoWPlayer(): " + e);
                return new List<WoWPlayer>();
            }
        }

        public static List<WoWPlayer> GetObjectWoWPlayerTargetMe()
        {
            try
            {
                List<WoWPlayer> result = new List<WoWPlayer>();
                foreach (WoWPlayer p in GetObjectWoWPlayer())
                {
                    if (p.IsTargetingMe) result.Add(new WoWPlayer(p.GetBaseAddress));
                }
                return result;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetObjectWoWPlayerTargetMe(): " + e);
                return new List<WoWPlayer>();
            }
        }

        public static WoWUnit GetNearestWoWUnit(List<WoWUnit> listWoWUnit, Point point, bool ignoreBlackList = false)
        {
            try
            {
                WoWUnit objectReturn = new WoWUnit(0);
                float tempDistance = 9999999.0f;
                foreach (WoWUnit a in listWoWUnit)
                {
                    if (point.DistanceTo(a.Position) < tempDistance && (!nManagerSetting.IsBlackListed(a.Guid) || ignoreBlackList))
                    {
                        objectReturn = a;
                        tempDistance = a.GetDistance;
                    }
                }
                return objectReturn;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetNearestWoWUnit(List<WoWUnit> listWoWUnit, Point point): " + e);
            }
            return new WoWUnit(0);
        }

        public static WoWUnit GetNearestWoWUnit(List<WoWUnit> listWoWUnit, bool ignorenotSelectable = false, bool ignoreBlackList = false, bool allowPlayedControlled = false)
        {
            try
            {
                WoWUnit objectReturn = new WoWUnit(0);
                float tempDistance = 9999999.0f;
                foreach (WoWUnit a in listWoWUnit)
                {
                    if (a.GetDistance > tempDistance)
                        continue;
                    if (nManagerSetting.IsBlackListed(a.Guid) && !ignoreBlackList)
                        continue;
                    if (!ignorenotSelectable && a.NotSelectable)
                        continue;
                    if (a.IsTapped && (!a.IsTapped || !a.IsTappedByMe))
                        continue;
                    if (a.PlayerControlled && !allowPlayedControlled)
                        continue;
                    objectReturn = a;
                    tempDistance = a.GetDistance;
                }
                return objectReturn;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetNearestWoWUnit(List<WoWUnit> listWoWUnit): " + e);
            }
            return new WoWUnit(0);
        }

        public static WoWPlayer GetNearestWoWPlayer(List<WoWPlayer> listWoWPlayer)
        {
            try
            {
                WoWPlayer objectReturn = new WoWPlayer(0);
                float tempDistance = 9999999.0f;
                foreach (WoWPlayer a in listWoWPlayer)
                {
                    if (a.GetDistance < tempDistance)
                    {
                        objectReturn = a;
                        tempDistance = a.GetDistance;
                    }
                }
                return objectReturn;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetNearestWoWPlayer(List<WoWPlayer> listWoWPlayer): " + e);
            }
            return new WoWPlayer(0);
        }

        public static WoWGameObject GetNearestWoWGameObject(List<WoWGameObject> listWoWGameObject, Point point, bool ignoreBlackList = false)
        {
            try
            {
                WoWGameObject objectReturn = new WoWGameObject(0);
                float tempDistance = 9999999.0f;
                foreach (WoWGameObject a in listWoWGameObject)
                {
                    if (a.Position.DistanceTo(point) < tempDistance && (!nManagerSetting.IsBlackListed(a.Guid) || ignoreBlackList))
                    {
                        objectReturn = a;
                        tempDistance = a.Position.DistanceTo(point);
                    }
                }
                return objectReturn;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetNearestWoWGameObject(List<WoWGameObject> listWoWGameObject, Point point): " + e);
            }
            return new WoWGameObject(0);
        }

        public static WoWGameObject GetNearestWoWGameObject(List<WoWGameObject> listWoWGameObject, bool ignoreBlackList = false)
        {
            try
            {
                WoWGameObject objectReturn = new WoWGameObject(0);
                float tempDistance = 9999999.0f;
                foreach (WoWGameObject a in listWoWGameObject)
                {
                    if (a.GetDistance < tempDistance && (!nManagerSetting.IsBlackListed(a.Guid) || ignoreBlackList))
                    {
                        objectReturn = a;
                        tempDistance = a.GetDistance;
                    }
                }
                return objectReturn;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetNearestWoWGameObject(List<WoWGameObject> listWoWGameObject): " + e);
            }
            return new WoWGameObject(0);
        }

        public static List<WoWUnit> GetWoWUnitByFaction(List<WoWUnit> listWoWUnit, List<uint> factions, bool pvp = false)
        {
            try
            {
                List<WoWUnit> objectReturn = new List<WoWUnit>();
                foreach (WoWUnit a in listWoWUnit)
                {
                    try
                    {
                        if (factions.Contains(a.Faction) && !a.IsDead && !a.Invisible && (pvp || !a.InCombat || a.InCombatWithMe))
                            objectReturn.Add(a);
                    }
                    catch (Exception e)
                    {
                        Logging.WriteError(
                            "GetWoWUnitByFaction(List<WoWUnit> listWoWUnit, List<uint> factions, bool pvp = false)#1: " +
                            e);
                    }
                }
                return objectReturn;
            }
            catch (Exception e)
            {
                Logging.WriteError(
                    "GetWoWUnitByFaction(List<WoWUnit> listWoWUnit, List<uint> factions, bool pvp = false)#2: " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitByFaction(List<WoWUnit> listWoWUnit, uint faction)
        {
            try
            {
                List<uint> factions = new List<uint> {faction};
                return GetWoWUnitByFaction(listWoWUnit, factions);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitByFaction(List<WoWUnit> listWoWUnit, uint faction): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitByFaction(uint faction)
        {
            try
            {
                return GetWoWUnitByFaction(GetObjectWoWUnit(), faction);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitByFaction(uint faction): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitByFaction(List<uint> factions, bool pvp = false)
        {
            try
            {
                return GetWoWUnitByFaction(GetObjectWoWUnit(), factions, pvp);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitByFaction(List<uint> factions, bool pvp = false): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitHostile()
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit a in GetObjectWoWUnit())
                {
                    if (UnitRelation.GetReaction(Me.Faction, a.Faction) == Reaction.Hostile && !a.IsDead && (!a.InCombat || a.InCombatWithMe)) list.Add(a);
                }
                return
                    list;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitHostile(): " + e);
            }
            return new List<WoWUnit>();
        }

        public static int GetNumberWoWUnitZone(Point point, int distanceSearch)
        {
            try
            {
                int count = 0;
                foreach (WoWUnit a in GetObjectWoWUnit())
                {
                    if (point.DistanceTo(a.Position) <= distanceSearch && !a.IsDead) count++;
                }
                return count;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetNumberWoWUnitZone(Point point, int distanceSearch): " + e);
            }
            return 0;
        }

        public static List<WoWUnit> GetWoWUnitByName(List<WoWUnit> listWoWUnit, List<string> names)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit a in listWoWUnit)
                {
                    if (names.Contains(a.Name) && !a.IsDead && !a.Invisible) list.Add(a);
                }
                return list;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitByName(List<WoWUnit> listWoWUnit, List<string> names): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitByName(List<WoWUnit> listWoWUnit, string name)
        {
            try
            {
                List<string> names = new List<string> {name};
                return GetWoWUnitByName(listWoWUnit, names);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitByName(List<WoWUnit> listWoWUnit, string name): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitByName(string name)
        {
            try
            {
                return GetWoWUnitByName(GetObjectWoWUnit(), name);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitByName(string name): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitByName(List<string> names)
        {
            try
            {
                return GetWoWUnitByName(GetObjectWoWUnit(), names);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitByName(List<string> names): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitByEntry(List<WoWUnit> listWoWUnit, List<int> entrys, bool isDead = false)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit a in listWoWUnit)
                {
                    if (entrys.Contains(a.Entry) && (!isDead && !a.IsDead || isDead) && !a.Invisible) list.Add(a);
                }
                return list;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitByEntry(List<WoWUnit> listWoWUnit, List<int> entrys): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitByEntry(List<WoWUnit> listWoWUnit, int entry, bool isDead = false)
        {
            try
            {
                List<int> entrys = new List<int> {entry};
                return GetWoWUnitByEntry(listWoWUnit, entrys, isDead);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitByEntry(List<WoWUnit> listWoWUnit, int entry): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitByEntry(List<int> entrys, bool isDead = false)
        {
            try
            {
                return GetWoWUnitByEntry(GetObjectWoWUnit(), entrys, isDead);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitByEntry(List<int> entrys): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitByEntry(int entry, bool isDead = false)
        {
            try
            {
                return GetWoWUnitByEntry(GetObjectWoWUnit(), entry, isDead);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitByEntry(int entry): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitByQuestLoot(List<WoWUnit> listWoWUnit, int lootId)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit a in listWoWUnit)
                {
                    if ((a.QuestItem1 == lootId || a.QuestItem2 == lootId || a.QuestItem3 == lootId || a.QuestItem4 == lootId) && !a.Invisible)
                        list.Add(a);
                }
                return list;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitByQuestLoot(List<WoWUnit> listWoWUnit, int lootId): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitByQuestLoot(int lootId)
        {
            try
            {
                return GetWoWUnitByQuestLoot(GetObjectWoWUnit(), lootId);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitByQuestLoot(int lootId): " + e);
            }
            return new List<WoWUnit>();
        }

        public static WoWUnit GetWoWUnitSummonedOrCreatedByMeAndFighting(List<WoWUnit> listWoWUnit)
        {
            try
            {
                // We could do better by iterating listWoWUnit 2 times to get all "pets" then attackers
                // but it's a lot slower and this code works fine even with totems, they have the proper target
                foreach (WoWUnit a in listWoWUnit)
                {
                    if ((a.SummonedBy == Me.Guid || a.CreatedBy == Me.Guid) && a.Target != 0)
                    {
                        WoWUnit u = new WoWUnit(GetObjectByGuid(a.Target).GetBaseAddress);
                        if (u.IsValid && u.InCombat && u.Target == a.Guid)
                            return a;
                    }
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitSummonedOrCreatedByMeAndFighting(List<WoWUnit> listWoWUnit): " + e);
            }
            return new WoWUnit(0);
        }

        public static WoWUnit GetWoWUnitSummonedOrCreatedByMeAndFighting()
        {
            try
            {
                return GetWoWUnitSummonedOrCreatedByMeAndFighting(GetObjectWoWUnit());
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitSummonedOrCreatedByMeAndFighting(): " + e);
            }
            return new WoWUnit(0);
        }

        public static List<WoWGameObject> GetWoWGameObjectByName(List<WoWGameObject> listWoWGameObject,
            List<string> names)
        {
            try
            {
                List<string> namesTemps = new List<string>();
                foreach (string s in names)
                    namesTemps.Add(s.ToLower());
                List<WoWGameObject> list = new List<WoWGameObject>();
                foreach (WoWGameObject a in listWoWGameObject)
                {
                    if (namesTemps.Contains(a.Name.ToLower())) list.Add(a);
                }
                return list;
            }
            catch (Exception e)
            {
                Logging.WriteError(
                    "GetWoWGameObjectByName(List<WoWGameObject> listWoWGameObject, List<string> names): " + e);
            }
            return new List<WoWGameObject>();
        }

        public static List<WoWGameObject> GetWoWGameObjectByName(List<WoWGameObject> listWoWGameObject, string name)
        {
            try
            {
                List<string> names = new List<string> {name};
                return GetWoWGameObjectByName(listWoWGameObject, names);
            }
            catch (Exception e)
            {
                Logging.WriteError(" GetWoWGameObjectByName(List<WoWGameObject> listWoWGameObject, string name): " + e);
            }
            return new List<WoWGameObject>();
        }

        public static List<WoWGameObject> GetWoWGameObjectByName(string name)
        {
            try
            {
                return GetWoWGameObjectByName(GetObjectWoWGameObject(), name);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWGameObjectByName(string name): " + e);
            }
            return new List<WoWGameObject>();
        }

        public static List<WoWGameObject> GetWoWGameObjectByName(List<string> names)
        {
            try
            {
                return GetWoWGameObjectByName(GetObjectWoWGameObject(), names);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWGameObjectByName(List<string> names): " + e);
            }
            return new List<WoWGameObject>();
        }

        public static List<WoWGameObject> GetWoWGameObjectByDisplayId(List<WoWGameObject> listWoWGameObject,
            List<uint> displayId)
        {
            try
            {
                //bjectManager.ReadObjectList();
                List<WoWGameObject> list = new List<WoWGameObject>();
                foreach (WoWGameObject a in listWoWGameObject)
                {
                    if (displayId.Contains(a.DisplayId)) list.Add(a);
                }
                return list;
            }
            catch (Exception e)
            {
                Logging.WriteError(
                    "GetWoWGameObjectByDisplayId(List<WoWGameObject> listWoWGameObject, List<int> displayId): " + e);
            }
            return new List<WoWGameObject>();
        }

        public static List<WoWGameObject> GetWoWGameObjectByDisplayId(uint displayId)
        {
            try
            {
                List<uint> tListInt = new List<uint> {displayId};
                return GetWoWGameObjectByDisplayId(tListInt);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWGameObjectByDisplayId(int displayId): " + e);
            }
            return new List<WoWGameObject>();
        }

        public static List<WoWGameObject> GetWoWGameObjectByDisplayId(List<uint> displayId)
        {
            try
            {
                return GetWoWGameObjectByDisplayId(GetObjectWoWGameObject(), displayId);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWGameObjectByDisplayId(List<int> displayId): " + e);
            }
            return new List<WoWGameObject>();
        }

        public static List<WoWGameObject> GetWoWGameObjectByEntry(List<WoWGameObject> listWoWGameObject, List<int> entry)
        {
            try
            {
                List<WoWGameObject> list = new List<WoWGameObject>();
                foreach (WoWGameObject a in listWoWGameObject)
                {
                    if (entry.Contains(a.Entry)) list.Add(a);
                }
                return list;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWGameObjectByEntry(List<WoWGameObject> listWoWGameObject, List<int> entry): " +
                                   e);
            }
            return new List<WoWGameObject>();
        }

        public static List<WoWGameObject> GetWoWGameObjectByEntry(int entry)
        {
            try
            {
                List<int> tListInt = new List<int> {entry};
                return GetWoWGameObjectByEntry(tListInt);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWGameObjectByEntry(int entry): " + e);
            }
            return new List<WoWGameObject>();
        }

        public static List<WoWGameObject> GetWoWGameObjectByEntry(List<int> entry)
        {
            try
            {
                return GetWoWGameObjectByEntry(GetObjectWoWGameObject(), entry);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWGameObjectByEntry(List<int> entry): " + e);
            }
            return new List<WoWGameObject>();
        }

        public static List<WoWGameObject> GetWoWGameObjectById(List<WoWGameObject> listWoWGameObject, List<int> id)
        {
            try
            {
                List<WoWGameObject> list = new List<WoWGameObject>();
                foreach (WoWGameObject a in listWoWGameObject)
                {
                    if (id.Contains(a.Entry)) list.Add(a);
                }
                return list;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWGameObjectById(List<WoWGameObject> listWoWGameObject, List<int> id): " + e);
            }
            return new List<WoWGameObject>();
        }

        public static List<WoWGameObject> GetWoWGameObjectForFarm()
        {
            try
            {
                List<WoWGameObject> list = new List<WoWGameObject>();
                foreach (WoWGameObject a in GetObjectWoWGameObject())
                {
                    if ((a.GOType == WoWGameObjectType.Chest || a.GOType == WoWGameObjectType.Goober)) list.Add(a);
                }
                return
                    list;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWGameObjectForFarm(): " + e);
            }
            return new List<WoWGameObject>();
        }

        public static List<WoWGameObject> GetWoWGameObjectById(int id)
        {
            try
            {
                List<int> tListInt = new List<int> {id};
                return GetWoWGameObjectById(tListInt);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWGameObjectByyId(int id): " + e);
            }
            return new List<WoWGameObject>();
        }

        public static List<WoWGameObject> GetWoWGameObjectById(List<int> id)
        {
            try
            {
                return GetWoWGameObjectById(GetObjectWoWGameObject(), id);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWGameObjectById(List<int> id): " + e);
            }
            return new List<WoWGameObject>();
        }

        public static List<WoWUnit> GetWoWUnitLootable(List<WoWUnit> listWoWUnit)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit a in listWoWUnit)
                {
                    if (a.IsLootable) list.Add(a);
                }
                return list;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitLootable(List<WoWUnit> listWoWUnit): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitLootable()
        {
            try
            {
                return GetWoWUnitLootable(GetObjectWoWUnit());
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitLootable(): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWPlayer> GetWoWPlayerLootable()
        {
            try
            {
                return GetWoWPlayerLootable(GetObjectWoWPlayer());
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWPlayerLootable(): " + e);
            }
            return new List<WoWPlayer>();
        }

        public static List<WoWPlayer> GetWoWPlayerLootable(List<WoWPlayer> listWoWPlayer)
        {
            try
            {
                List<WoWPlayer> list = new List<WoWPlayer>();
                foreach (WoWPlayer a in listWoWPlayer)
                {
                    if (a.IsLootable) list.Add(a);
                }
                return list;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWPlayerLootable(List<WoWPlayer> listWoWPlayer): " + e);
            }
            return new List<WoWPlayer>();
        }

        public static List<WoWPlayer> GetWoWPlayerDead()
        {
            try
            {
                return GetWoWPlayerDead(GetObjectWoWPlayer());
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWPlayerDead(): " + e);
            }
            return new List<WoWPlayer>();
        }

        public static List<WoWPlayer> GetWoWPlayerDead(List<WoWPlayer> listWoWPlayer)
        {
            try
            {
                List<WoWPlayer> list = new List<WoWPlayer>();
                foreach (WoWPlayer a in listWoWPlayer)
                {
                    if (a.IsDead) list.Add(a);
                }
                return list;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWPlayerDead(List<WoWPlayer> listWoWPlayer): " + e);
            }
            return new List<WoWPlayer>();
        }

        public static List<WoWUnit> GetWoWUnitSkinnable(List<WoWUnit> listWoWUnit, List<UInt128> withoutGuid)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                int mySkinningLevel = Skill.GetValue(Enums.SkillLine.Skinning);
                int myHerbalismLevel = Skill.GetValue(Enums.SkillLine.Herbalism);
                if (myHerbalismLevel > 0)
                    myHerbalismLevel += Skill.GetSkillBonus(Enums.SkillLine.Herbalism);
                int myMiningLevel = Skill.GetValue(Enums.SkillLine.Mining);
                if (myMiningLevel > 0)
                    myMiningLevel += Skill.GetSkillBonus(Enums.SkillLine.Mining);
                int myEngeneeringLevel = Skill.GetValue(Enums.SkillLine.Engineering);
                if (myEngeneeringLevel > 0)
                    myEngeneeringLevel += Skill.GetSkillBonus(Enums.SkillLine.Engineering);
                foreach (WoWUnit a in listWoWUnit)
                {
                    if (a.IsSkinnable && !withoutGuid.Contains(a.Guid))
                    {
                        if (a.ExtraLootType.HasFlag(TypeFlag.HERB_LOOT))
                            if (a.GetSkillLevelRequired <= myHerbalismLevel) list.Add(a);
                            else continue;
                        if (a.ExtraLootType.HasFlag(TypeFlag.MINING_LOOT))
                            if (a.GetSkillLevelRequired <= myMiningLevel)
                                list.Add(a);
                            else continue;
                        if (a.ExtraLootType.HasFlag(TypeFlag.ENGENEERING_LOOT))
                            if (a.GetSkillLevelRequired <= myEngeneeringLevel)
                                list.Add(a);
                            else continue;
                        if (mySkinningLevel > 0)
                            list.Add(a);
                    }
                }
                return list;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitSkinnable(List<WoWUnit> listWoWUnit, List<UInt128> withoutGuid): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitSkinnable(List<UInt128> withoutGuid)
        {
            try
            {
                return GetWoWUnitSkinnable(GetObjectWoWUnit(), withoutGuid);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitSkinnable(List<UInt128> withoutGuid): " + e);
            }
            return new List<WoWUnit>();
        }

        public static int GetNumberAttackPlayer()
        {
            try
            {
                return GetHostileUnitAttackingPlayer().Count;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetNumberAttackPlayer(): " + e);
            }
            return 0;
        }

        public static uint GetUnitInSpellRange(float spellRange = 5)
        {
            if (spellRange < 5)
                spellRange = 5;
            uint unitInSpellRange = 0;
            foreach (WoWUnit u in GetObjectWoWUnit())
            {
                if (!u.IsValid)
                    continue;
                if (!u.Attackable)
                    continue;
                if (u.NotSelectable)
                    continue;
                if (!u.IsHostile)
                    continue;
                if (u.GetDistance > spellRange)
                    continue;
                unitInSpellRange++;
            }
            return unitInSpellRange;
        }

        public static WoWUnit GetUnitInAggroRange()
        {
            foreach (WoWUnit u in GetObjectWoWUnit())
            {
                if (u.IsValid && u.IsAlive && u.Attackable && !u.PlayerControlled && !u.NotSelectable &&
                    UnitRelation.GetReaction(Me, u) == Reaction.Hostile &&
                    u.GetDistance < (u.AggroDistance*0.90f) &&
                    !(u.InCombat && !u.IsTargetingMe))
                {
                    bool r;
                    List<Point> points = PathFinder.FindPath(u.Position, out r);
                    if (!r)
                        points.Add(u.Position);
                    if (Helpful.Math.DistanceListPoint(points) < u.AggroDistance*3)
                        return new WoWUnit(u.GetBaseAddress);
                }
            }
            return null;
        }

        public static List<WoWUnit> GetUnitTargetingPlayer()
        {
            var outputList = new List<WoWUnit>();
            List<WoWPlayer> playersList = GetObjectWoWPlayer();
            List<WoWUnit> unitsList = GetObjectWoWUnit();

            for (int i = 0; i < playersList.Count; i++)
            {
                WoWPlayer player = playersList[i];
                if (BlackListMobAttack.Contains(player.Guid))
                    continue;

                if (player.IsValid && player.IsAlive && (player.Target == Me.Guid || player.Target == Pet.Guid))
                    outputList.Add(player);
            }
            for (int i = 0; i < unitsList.Count; i++)
            {
                WoWUnit unit = unitsList[i];
                if (BlackListMobAttack.Contains(unit.Guid))
                    continue;
                if (unit.IsValid && unit.IsAlive && (unit.Target == Me.Guid || unit.Target == Pet.Guid))
                    outputList.Add(unit);
            }
            return outputList;
        }

        public static List<WoWUnit> GetHostileUnitTargetingPlayer()
        {
            try
            {
                Memory.WowMemory.GameFrameLock();
                var outputList = new List<WoWUnit>();
                List<WoWUnit> unitsList = GetUnitTargetingPlayer();

                foreach (WoWUnit unit in unitsList)
                {
                    if (unit.IsHostile)
                        outputList.Add(unit);
                }
                Memory.WowMemory.GameFrameUnLock();

                return outputList;
            }
            finally
            {
                Memory.WowMemory.GameFrameUnLock();
            }
        }

        public static List<WoWUnit> GetHostileUnitAttackingPlayer()
        {
            try
            {
                Memory.WowMemory.GameFrameLock();
                var outputList = new List<WoWUnit>();
                List<WoWUnit> unitsList = GetUnitTargetingPlayer();

                foreach (WoWUnit unit in unitsList)
                {
                    if (unit.IsHostile && unit.InCombat)
                        outputList.Add(unit);
                }
                Memory.WowMemory.GameFrameUnLock();

                return outputList;
            }
            finally
            {
                Memory.WowMemory.GameFrameUnLock();
            }
        }

        public static List<WoWUnit> GetWoWUnitRepair(List<WoWUnit> listWoWUnit)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit a in listWoWUnit)
                {
                    if (a.IsNpcRepair && !a.IsNpcInnkeeper) list.Add(a);
                }
                return list;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitRepair(List<WoWUnit> listWoWUnit): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitRepair()
        {
            try
            {
                return GetWoWUnitRepair(GetObjectWoWUnit());
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitRepair(): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitVendorFood(List<WoWUnit> listWoWUnit)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit a in listWoWUnit)
                {
                    if (a.IsNpcVendorFood && !a.IsNpcInnkeeper) list.Add(a);
                }
                return list;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitVendorFood(List<WoWUnit> listWoWUnit): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitInkeeper()
        {
            try
            {
                return GetWoWUnitInkeeper(GetObjectWoWUnit());
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitInkeeper(): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitInkeeper(List<WoWUnit> listWoWUnit)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit a in listWoWUnit)
                {
                    if (a.IsNpcInnkeeper) list.Add(a);
                }
                return list;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitInkeeper(List<WoWUnit> listWoWUnit): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitVendorFood()
        {
            try
            {
                return GetWoWUnitVendorFood(GetObjectWoWUnit());
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitVendorFood(): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitVendor(List<WoWUnit> listWoWUnit)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit a in listWoWUnit)
                {
                    if (a.IsNpcVendor && !a.IsNpcInnkeeper) list.Add(a);
                }
                return list;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitVendor(List<WoWUnit> listWoWUnit): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitVendor()
        {
            try
            {
                return GetWoWUnitVendor(GetObjectWoWUnit());
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitVendor(): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitTrainer(List<WoWUnit> listWoWUnit)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit a in listWoWUnit)
                {
                    if (a.IsNpcTrainer) list.Add(a);
                }
                return list;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitTrainer(List<WoWUnit> listWoWUnit): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitTrainer()
        {
            try
            {
                return GetWoWUnitTrainer(GetObjectWoWUnit());
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitTrainer(): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitFlightMaster(List<WoWUnit> listWoWUnit)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit a in listWoWUnit)
                {
                    if (a.IsNpcFlightMaster)
                        list.Add(a);
                }
                return list;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitFlightMaster(List<WoWUnit> listWoWUnit): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitFlightMaster()
        {
            try
            {
                return GetWoWUnitFlightMaster(GetObjectWoWUnit());
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitFlightMaster(): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitSpiritHealer(List<WoWUnit> listWoWUnit)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit a in listWoWUnit)
                {
                    if (a.IsNpcSpiritHealer) list.Add(a);
                }
                return list;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitSpiritHealer(List<WoWUnit> listWoWUnit): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitSpiritHealer()
        {
            try
            {
                return GetWoWUnitSpiritHealer(GetObjectWoWUnit());
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitSpiritHealer(): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitSpiritGuide(List<WoWUnit> listWoWUnit)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit a in listWoWUnit)
                {
                    if (a.IsNpcSpiritGuide) list.Add(a);
                }
                return list;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitSpiritGuide(List<WoWUnit> listWoWUnit): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitSpiritGuide()
        {
            try
            {
                return GetWoWUnitSpiritGuide(GetObjectWoWUnit());
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitSpiritGuide(): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitMailbox(List<WoWUnit> listWoWUnit)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit a in listWoWUnit)
                {
                    if (a.IsNpcMailbox) list.Add(a);
                }
                return list;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitMailbox(List<WoWUnit> listWoWUnit): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitMailbox()
        {
            try
            {
                return GetWoWUnitMailbox(GetObjectWoWUnit());
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitMailbox(): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitQuester(List<WoWUnit> listWoWUnit)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit a in listWoWUnit)
                {
                    if (a.IsNpcQuestGiver) list.Add(a);
                }
                return list;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitQuester(List<WoWUnit> listWoWUnit): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitQuester()
        {
            try
            {
                return GetWoWUnitQuester(GetObjectWoWUnit());
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitQuester(): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWPlayer> GetWoWUnitAlliance(List<WoWPlayer> listWoWUnit)
        {
            try
            {
                List<WoWPlayer> list = new List<WoWPlayer>();
                foreach (WoWPlayer a in listWoWUnit)
                {
                    if (a.PlayerFaction == "Alliance" && !a.IsDead && a.SummonedBy == 0) list.Add(a);
                }
                return list;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitAlliance(List<WoWPlayer> listWoWUnit): " + e);
            }
            return new List<WoWPlayer>();
        }

        public static List<WoWPlayer> GetWoWUnitAlliance()
        {
            try
            {
                return GetWoWUnitAlliance(GetObjectWoWPlayer());
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitAlliance(): " + e);
            }
            return new List<WoWPlayer>();
        }

        public static List<WoWPlayer> GetWoWUnitHorde(List<WoWPlayer> listWoWUnit)
        {
            try
            {
                List<WoWPlayer> list = new List<WoWPlayer>();
                foreach (WoWPlayer a in listWoWUnit)
                {
                    if (a.PlayerFaction == "Horde" && !a.IsDead && a.SummonedBy == 0) list.Add(a);
                }
                return list;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitHorde(List<WoWPlayer> listWoWUnit): " + e);
            }
            return new List<WoWPlayer>();
        }

        public static List<WoWPlayer> GetWoWUnitHorde()
        {
            try
            {
                return GetWoWUnitHorde(GetObjectWoWPlayer());
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitHorde(): " + e);
            }
            return new List<WoWPlayer>();
        }

        public static List<WoWPlayer> GetWoWUnitAllianceDead(List<WoWPlayer> listWoWUnit)
        {
            try
            {
                List<WoWPlayer> list = new List<WoWPlayer>();
                foreach (WoWPlayer a in listWoWUnit)
                {
                    if (a.PlayerFaction == "Alliance" && a.IsDead && a.SummonedBy == 0) list.Add(a);
                }
                return list;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitAllianceDead(List<WoWPlayer> listWoWUnit): " + e);
            }
            return new List<WoWPlayer>();
        }

        public static List<WoWPlayer> GetWoWUnitAllianceDead()
        {
            try
            {
                return GetWoWUnitAllianceDead(GetObjectWoWPlayer());
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitAllianceDead(): " + e);
            }
            return new List<WoWPlayer>();
        }

        public static List<WoWPlayer> GetWoWUnitHordeDead(List<WoWPlayer> listWoWUnit)
        {
            try
            {
                List<WoWPlayer> list = new List<WoWPlayer>();
                foreach (WoWPlayer a in listWoWUnit)
                {
                    if (a.PlayerFaction == "Horde" && a.IsDead && a.SummonedBy == 0) list.Add(a);
                }
                return list;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitHordeDead(List<WoWPlayer> listWoWUnit): " + e);
            }
            return new List<WoWPlayer>();
        }

        public static List<WoWPlayer> GetWoWUnitHordeDead()
        {
            try
            {
                return GetWoWUnitHordeDead(GetObjectWoWPlayer());
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitHordeDead(): " + e);
            }
            return new List<WoWPlayer>();
        }

        public static WoWPlayer GetWoWUnitWGFlagHolder(bool hostileHolder = true)
        {
            List<WoWPlayer> woWPlayerList = new List<WoWPlayer>();
            if (hostileHolder)
                woWPlayerList.AddRange(Me.PlayerFaction.ToLower() == "horde"
                    ? GetWoWUnitAlliance()
                    : GetWoWUnitHorde());
            else
                woWPlayerList.AddRange(Me.PlayerFaction.ToLower() == "horde"
                    ? GetWoWUnitHorde()
                    : GetWoWUnitAlliance());
            return woWPlayerList.FirstOrDefault(wowPlayer => wowPlayer.IsHoldingWGFlag);
        }

        public static bool IsSomeoneHoldingWGFlag(bool hostileHolder = true)
        {
            return GetWoWUnitWGFlagHolder(hostileHolder) != null;
        }

        public static List<WoWUnit> GetWoWUnitAuctioneer(List<WoWUnit> listWoWUnit)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit a in listWoWUnit)
                {
                    if (a.IsNpcAuctioneer) list.Add(a);
                }
                return list;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitAuctioneer(List<WoWUnit> listWoWUnit): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitAuctioneer()
        {
            try
            {
                return GetWoWUnitAuctioneer(GetObjectWoWUnit());
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitAuctioneer(): " + e);
            }
            return new List<WoWUnit>();
        }
    }
}