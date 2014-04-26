using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using nManager.Helpful;
using nManager.Wow.Bot.States;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.Patchables;

namespace nManager.Wow.ObjectManager
{
    public static class ObjectManager
    {
        private static readonly object Locker = new object();
        private static uint _lastTargetBase;
        private static uint _lastPetBase;
        public static List<ulong> BlackListMobAttack = new List<ulong>();

        static ObjectManager()
        {
            try
            {
                ObjectList = new List<WoWObject>();
                ObjectDictionary = new Dictionary<ulong, WoWObject>();
                Me = new WoWPlayer(0);
            }
            catch (Exception e)
            {
                Logging.WriteError("ObjectManager(): " + e);
            }
        }

        private static uint ObjectManagerAddress { get; set; }

        public static List<WoWObject> ObjectList { get; private set; }
        public static Dictionary<ulong, WoWObject> ObjectDictionary { get; set; }
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
                    if (_lastPetBase > 0)
                    {
                        if (new WoWUnit(_lastPetBase).Health > 0 && new WoWUnit(_lastPetBase).SummonedBy == Me.Guid)
                            return new WoWUnit(_lastPetBase);
                        _lastPetBase = 0;
                    }

                    ulong guidPet =
                        Memory.WowMemory.Memory.ReadUInt64(Memory.WowProcess.WowModule + (uint) Addresses.Player.petGUID);

                    if (guidPet > 0)
                        return new WoWUnit(GetObjectByGuid(guidPet).GetBaseAddress);
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
                    foreach (KeyValuePair<ulong, WoWObject> o in ObjectDictionary)
                    {
                        o.Value.UpdateBaseAddress(0);
                    }

                    // Fill the new list.
                    ReadObjectList();

                    // Clear out old references.
                    List<ulong> toRemove = new List<ulong>();
                    List<WoWObject> list = new List<WoWObject>();
                    foreach (KeyValuePair<ulong, WoWObject> o in ObjectDictionary)
                    {
                        if (o.Value.IsValid)
                            list.Add(o.Value);
                        else
                            toRemove.Add(o.Key);
                    }
                    // All done! Just make sure we pass up a valid list to the ObjectList.
                    ObjectList = list;

                    foreach (ulong guid in toRemove)
                    {
                        ObjectDictionary.Remove(guid);
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
                    Memory.WowMemory.Memory.ReadUInt(
                        Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule +
                                                         Addresses.ObjectManagerClass.clientConnection) +
                        (uint) Addresses.ObjectManager.objectManager);

                ulong localPlayerGuid =
                    Memory.WowMemory.Memory.ReadUInt64(ObjectManagerAddress + (uint) Addresses.ObjectManager.localGuid);

                // Get the first object in the linked list.
                int currentObject = Memory.WowMemory.Memory.ReadInt(ObjectManagerAddress + (uint) Addresses.ObjectManager.firstObject);

                while (currentObject != 0)
                {
                    try
                    {
                        ulong objGuid = Memory.WowMemory.Memory.ReadUInt64((uint) currentObject + (uint) Addresses.ObjectManager.objectGUID);
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
                            }
                            if (obj != null)
                            {
                                // We have a valid object that isn't in the object list already.
                                // So lets add it.
                                ObjectDictionary.Add(objGuid, obj);
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
        public static WoWObject GetObjectByGuid(ulong guid)
        {
            try
            {
                lock (Locker)
                    return ObjectDictionary.ContainsKey(guid) ? ObjectDictionary[guid] : new WoWObject(0);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetObjectByGuid(ulong guid): " + e);
                return new WoWObject(0);
            }
        }

        private static IEnumerable<WoWObject> GetObjectByType(WoWObjectType type)
        {
            try
            {
                WoWObject[] objects = ObjectList.ToArray();


                // Simply print each objects GUID and position.
                List<WoWObject> list = new List<WoWObject>();
                foreach (WoWObject o in objects)
                {
                    if (o.Type == type) list.Add(new WoWObject(o.GetBaseAddress));
                }
                return list;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetObjectByType(WoWObjectType type): " + e);
                return new List<WoWObject>();
            }
        }

        public static List<WoWUnit> GetObjectWoWUnit()
        {
            try
            {
                IEnumerable<WoWObject> tempsListObj = GetObjectByType(WoWObjectType.Unit);
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWObject a in tempsListObj)
                    list.Add(new WoWUnit(a.GetBaseAddress));
                return list;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetObjectWoWUnit(): " + e);
                return new List<WoWUnit>();
            }
        }

        public static List<WoWContainer> GetObjectWoWContainer()
        {
            try
            {
                IEnumerable<WoWObject> tempsListObj = GetObjectByType(WoWObjectType.Container);
                List<WoWContainer> list = new List<WoWContainer>();
                foreach (WoWObject a in tempsListObj)
                {
                    WoWContainer container = new WoWContainer(a.GetBaseAddress);
                    list.Add(container);
                }
                return list;
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
                IEnumerable<WoWObject> tempsListObj = GetObjectByType(WoWObjectType.Corpse);
                List<WoWCorpse> list = new List<WoWCorpse>();
                foreach (WoWObject a in tempsListObj)
                    list.Add(new WoWCorpse(a.GetBaseAddress));
                return list;
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
                IEnumerable<WoWObject> tempsListObj = GetObjectByType(WoWObjectType.Item);
                List<WoWItem> list = new List<WoWItem>();
                foreach (WoWObject a in tempsListObj)
                    list.Add(new WoWItem(a.GetBaseAddress));
                return list;
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
                IEnumerable<WoWObject> tempsListObj = GetObjectByType(WoWObjectType.Item);
                foreach (WoWObject a in tempsListObj)
                {
                    if (a.Entry == entry)
                    {
                        return new WoWItem(a.GetBaseAddress);
                    }
                }
                return new WoWItem(0);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWItemById(uint entry): " + e);
                return new WoWItem(0);
            }
        }

        public static List<WoWGameObject> GetWoWGameObjectOfType(WoWGameObjectType reqtype)
        {
            try
            {
                IEnumerable<WoWObject> tempsListObj = GetObjectByType(WoWObjectType.GameObject);
                List<WoWGameObject> retList = new List<WoWGameObject>();
                foreach (WoWObject a in tempsListObj)
                {
                    WoWGameObject b = new WoWGameObject(a.GetBaseAddress);
                    if (b.GOType == reqtype)
                        retList.Add(b);
                }
                return retList;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetObjectWoWGameObject(): " + e);
                return new List<WoWGameObject>();
            }
        }

        public static List<WoWGameObject> GetObjectWoWGameObject()
        {
            try
            {
                IEnumerable<WoWObject> tempsListObj = GetObjectByType(WoWObjectType.GameObject);
                List<WoWGameObject> list = new List<WoWGameObject>();
                foreach (WoWObject a in tempsListObj)
                    list.Add(new WoWGameObject(a.GetBaseAddress));
                return list;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetObjectWoWGameObject(): " + e);
                return new List<WoWGameObject>();
            }
        }

        public static WoWPlayer GetObjectWoWPlayer(ulong guid)
        {
            try
            {
                IEnumerable<WoWObject> tempsListObj = GetObjectByType(WoWObjectType.Player);
                foreach (WoWObject a in tempsListObj)
                {
                    if (a.Guid == guid)
                        return new WoWPlayer(a.GetBaseAddress);
                }
                return null;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetObjectWoWPlayer(uint guid): " + e);
                return null;
            }
        }

        public static List<WoWPlayer> GetObjectWoWPlayer()
        {
            try
            {
                IEnumerable<WoWObject> tempsListObj = GetObjectByType(WoWObjectType.Player);
                List<WoWPlayer> list = new List<WoWPlayer>();
                foreach (WoWObject a in tempsListObj)
                {
                    if (a.GetBaseAddress != Me.GetBaseAddress) list.Add(new WoWPlayer(a.GetBaseAddress));
                }
                return
                    list;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetObjectWoWPlayer(): " + e);
                return new List<WoWPlayer>();
            }
        }

        public static List<WoWObject> GetObjectWoW()
        {
            try
            {
                return ObjectList;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetObjectWoW(): " + e);
            }
            return new List<WoWObject>();
        }

        public static List<WoWPlayer> GetObjectWoWPlayerTargetMe()
        {
            try
            {
                List<WoWPlayer> tempsList = new List<WoWPlayer>();
                tempsList.AddRange(GetObjectWoWPlayer());
                List<WoWPlayer> list = new List<WoWPlayer>();
                foreach (WoWPlayer a in tempsList)
                {
                    if (a.IsTargetingMe) list.Add(new WoWPlayer(a.GetBaseAddress));
                }
                return list;
            }
            catch (Exception e)
            {
                Logging.WriteError(" GetObjectWoWPlayerTargetMe(): " + e);
                return new List<WoWPlayer>();
            }
        }

        public static WoWUnit GetNearestWoWUnit(List<WoWUnit> listWoWUnit, Point point)
        {
            try
            {
                WoWUnit objectReturn = new WoWUnit(0);
                float tempDistance = 9999999.0f;
                foreach (WoWUnit a in listWoWUnit)
                {
                    if (point.DistanceTo(a.Position) < tempDistance && !nManagerSetting.IsBlackListed(a.Guid))
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

        public static WoWUnit GetNearestWoWUnit(List<WoWUnit> listWoWUnit, bool ignorenotSelectable = false)
        {
            try
            {
                WoWUnit objectReturn = new WoWUnit(0);
                float tempDistance = 9999999.0f;
                foreach (WoWUnit a in listWoWUnit)
                {
                    if (a.GetDistance < tempDistance && !nManagerSetting.IsBlackListed(a.Guid) && (ignorenotSelectable || !a.NotSelectable))
                    {
                        objectReturn = a;
                        tempDistance = a.GetDistance;
                    }
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

        public static WoWGameObject GetNearestWoWGameObject(List<WoWGameObject> listWoWGameObject, Point point)
        {
            try
            {
                WoWGameObject objectReturn = new WoWGameObject(0);
                float tempDistance = 9999999.0f;
                //foreach (var a in listWoWGameObject.Where(a => a.Position.DistanceTo(point) < tempDistance))
                foreach (WoWGameObject a in listWoWGameObject)
                {
                    if (a.Position.DistanceTo(point) < tempDistance && !nManagerSetting.IsBlackListed(a.Guid))
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

        public static WoWGameObject GetNearestWoWGameObject(List<WoWGameObject> listWoWGameObject)
        {
            try
            {
                WoWGameObject objectReturn = new WoWGameObject(0);
                float tempDistance = 9999999.0f;
                //foreach (var a in listWoWGameObject.Where(a => a.GetDistance < tempDistance))
                foreach (WoWGameObject a in listWoWGameObject)
                {
                    if (a.GetDistance < tempDistance && !nManagerSetting.IsBlackListed(a.Guid))
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

        public static List<WoWUnit> GetWoWUnitByEntry(List<WoWUnit> listWoWUnit, List<int> entrys)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit a in listWoWUnit)
                {
                    if (entrys.Contains(a.Entry) && !a.IsDead && !a.Invisible) list.Add(a);
                }
                return list;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitByEntry(List<WoWUnit> listWoWUnit, List<int> entrys): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitByEntry(List<WoWUnit> listWoWUnit, int entry)
        {
            try
            {
                List<int> entrys = new List<int> {entry};
                return GetWoWUnitByEntry(listWoWUnit, entrys);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitByEntry(List<WoWUnit> listWoWUnit, int entry): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitByEntry(int entry)
        {
            try
            {
                return GetWoWUnitByEntry(GetObjectWoWUnit(), entry);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitByEntry(int entry): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitByEntry(List<int> entrys)
        {
            try
            {
                return GetWoWUnitByEntry(GetObjectWoWUnit(), entrys);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitByEntry(List<int> entrys): " + e);
            }
            return new List<WoWUnit>();
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
            List<int> displayId)
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

        public static List<WoWGameObject> GetWoWGameObjectByDisplayId(int displayId)
        {
            try
            {
                List<int> tListInt = new List<int> {displayId};
                return GetWoWGameObjectByDisplayId(tListInt);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWGameObjectByDisplayId(int displayId): " + e);
            }
            return new List<WoWGameObject>();
        }

        public static List<WoWGameObject> GetWoWGameObjectByDisplayId(List<int> displayId)
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

        public static List<WoWUnit> GetWoWUnitSkinnable(List<WoWUnit> listWoWUnit, List<ulong> withoutGuid)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                int mySkinningLevel = Skill.GetValue(Enums.SkillLine.Skinning);
                if (mySkinningLevel > 0)
                    mySkinningLevel += Skill.GetSkillBonus(Enums.SkillLine.Skinning);
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
                        if (a.ExtraLootType.HasFlag(TypeFlag.HERB_LOOT) && a.GetSkillLevelRequired <= myHerbalismLevel)
                            list.Add(a);
                        else if (a.ExtraLootType.HasFlag(TypeFlag.MINING_LOOT) && a.GetSkillLevelRequired <= myMiningLevel)
                            list.Add(a);
                        else if (a.ExtraLootType.HasFlag(TypeFlag.ENGENEERING_LOOT) && a.GetSkillLevelRequired <= myEngeneeringLevel)
                            list.Add(a);
                        else if (a.GetSkillLevelRequired <= mySkinningLevel)
                            list.Add(a);
                    }
                }
                return list;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitSkinnable(List<WoWUnit> listWoWUnit, List<ulong> withoutGuid): " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitSkinnable(List<ulong> withoutGuid)
        {
            try
            {
                return GetWoWUnitSkinnable(GetObjectWoWUnit(), withoutGuid);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitSkinnable(List<ulong> withoutGuid): " + e);
            }
            return new List<WoWUnit>();
        }

        public static int GetNumberAttackPlayer(List<WoWUnit> listWoWUnit)
        {
            try
            {
                return GetUnitAttackPlayer(listWoWUnit).Count;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetNumberAttackPlayer(List<WoWUnit> listWoWUnit): " + e);
            }
            return 0;
        }

        public static int GetNumberAttackPlayer(int belowThisRange = 0)
        {
            try
            {
                return belowThisRange > 0 ? GetUnitAttackPlayer().Count(unit => unit.GetDistance <= belowThisRange) : GetUnitAttackPlayer().Count;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetNumberAttackPlayer(): " + e);
            }
            return 0;
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
                    if (Helpful.Math.DistanceListPoint(points) < u.AggroDistance*4)
                        return new WoWUnit(u.GetBaseAddress);
                }
            }
            return null;
        }

        public static List<WoWUnit> GetUnitAttackPlayer(List<WoWUnit> listWoWUnit)
        {
            try
            {
                List<WoWUnit> objectReturn = new List<WoWUnit>();
                foreach (WoWUnit a in listWoWUnit)
                {
                    if ((!BlackListMobAttack.Contains(a.Guid)) || (a.GetMove && !TraceLine.TraceLineGo(a.Position)))
                    {
                        bool petAttacked = false;
                        try
                        {
                            if (Pet.GetBaseAddress > 0)
                                if (a.InCombat && !a.IsDead && a.Target == Pet.Guid && !Pet.IsDead)
                                    petAttacked = true;
                        }
                        catch (Exception e)
                        {
                            Logging.WriteError("GetUnitAttackPlayer(List<WoWUnit> listWoWUnit)#1: " + e);
                        }
                        if ((a.IsTargetingMe && a.InCombat && !a.IsDead) || petAttacked)
                            objectReturn.Add(a);
                    }
                }
                return objectReturn;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetUnitAttackPlayer(List<WoWUnit> listWoWUnit)#2: " + e);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetUnitAttackPlayer()
        {
            try
            {
                List<WoWPlayer> tp = new List<WoWPlayer>();

                tp.AddRange(Me.PlayerFaction.ToLower() == "horde" ? GetWoWUnitAlliance() : GetWoWUnitHorde());

                List<WoWUnit> tu = new List<WoWUnit>();
                foreach (WoWPlayer t in tp)
                    tu.Add(new WoWUnit(t.GetBaseAddress));

                tu.AddRange(GetObjectWoWUnit());

                return GetUnitAttackPlayer(tu);
            }
            catch (Exception e)
            {
                Logging.WriteError("GetUnitAttackPlayer(): " + e);
                return new List<WoWUnit>();
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
                    if (a.IsNpcFlightMaster) list.Add(a);
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
    }
}