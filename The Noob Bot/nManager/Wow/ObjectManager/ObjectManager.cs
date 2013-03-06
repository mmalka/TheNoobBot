using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using nManager.Helpful;
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
                            var t = new WoWUnit(GetObjectByGuid(Me.Target).GetBaseAddress);
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
                    foreach (var o in ObjectDictionary)
                    {
                        o.Value.UpdateBaseAddress(0);
                    }

                    // Fill the new list.
                    ReadObjectList();

                    // Clear out old references.
                    var toRemove = (from o in ObjectDictionary where !o.Value.IsValid select o.Key).ToList();
                    foreach (ulong guid in toRemove)
                    {
                        ObjectDictionary.Remove(guid);
                    }

                    // All done! Just make sure we pass up a valid list to the ObjectList.
                    ObjectList = (from o in ObjectDictionary
                                  where o.Value.IsValid
                                  select o.Value).ToList();
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

                // These are 'hard coded' in the client. I don't remember the last time they changed.
                uint firstObject = (uint) Addresses.ObjectManager.firstObject;
                uint nextObject = (uint) Addresses.ObjectManager.nextObject;
                ulong localPlayerGuid =
                    Memory.WowMemory.Memory.ReadUInt64(ObjectManagerAddress + (uint) Addresses.ObjectManager.localGuid);

                // Get the first object in the linked list.
                int currentObject = Memory.WowMemory.Memory.ReadInt(ObjectManagerAddress + firstObject);

                while (currentObject != 0)
                {
                    try
                    {
                        ulong objGuid = Memory.WowMemory.Memory.ReadUInt64((uint) currentObject + (uint) Addresses.ObjectManager.objectGUID);
                        if (!ObjectDictionary.ContainsKey(objGuid))
                        {
                            var objType = (WoWObjectType) Memory.WowMemory.Memory.ReadInt((uint) currentObject + (uint) Addresses.ObjectManager.objectTYPE);

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
                    Int32 currentObjectNew = Memory.WowMemory.Memory.ReadInt((uint) currentObject + nextObject);
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

        private static List<WoWObject> GetObjectByType(WoWObjectType type)
        {
            try
            {
                WoWObject[] objects = ObjectList.ToArray();


                // Simply print each objects GUID and position.
                return (from o in objects where o.Type == type select new WoWObject(o.GetBaseAddress)).ToList();
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
                List<WoWObject> tempsListObj = GetObjectByType(WoWObjectType.Unit);
                return tempsListObj.Select(a => new WoWUnit(a.GetBaseAddress)).ToList();
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
                List<WoWObject> tempsListObj = GetObjectByType(WoWObjectType.Container);
                return tempsListObj.Select(a => new WoWContainer(a.GetBaseAddress)).ToList();
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
                List<WoWObject> tempsListObj = GetObjectByType(WoWObjectType.Corpse);
                return tempsListObj.Select(a => new WoWCorpse(a.GetBaseAddress)).ToList();
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
                List<WoWObject> tempsListObj = GetObjectByType(WoWObjectType.Item);
                return tempsListObj.Select(a => new WoWItem(a.GetBaseAddress)).ToList();
            }
            catch (Exception e)
            {
                Logging.WriteError("GetObjectWoWItem(): " + e);
                return new List<WoWItem>();
            }
        }

        public static WoWItem GetWoWItemById(uint entry)
        {
            try
            {
                List<WoWObject> tempsListObj = GetObjectByType(WoWObjectType.Item);
                foreach (var a in tempsListObj.Where(a => a.Entry == entry))
                {
                    return new WoWItem(a.GetBaseAddress);
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
                List<WoWObject> tempsListObj = GetObjectByType(WoWObjectType.GameObject);
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
                List<WoWObject> tempsListObj = GetObjectByType(WoWObjectType.GameObject);
                return tempsListObj.Select(a => new WoWGameObject(a.GetBaseAddress)).ToList();
            }
            catch (Exception e)
            {
                Logging.WriteError("GetObjectWoWGameObject(): " + e);
                return new List<WoWGameObject>();
            }
        }

        public static List<WoWPlayer> GetObjectWoWPlayer()
        {
            try
            {
                List<WoWObject> tempsListObj = GetObjectByType(WoWObjectType.Player);
                return
                    (from a in tempsListObj
                     where a.GetBaseAddress != Me.GetBaseAddress
                     select new WoWPlayer(a.GetBaseAddress)).ToList();
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
                var tempsList = new List<WoWPlayer>();
                tempsList.AddRange(GetObjectWoWPlayer());
                return (from a in tempsList where a.IsTargetingMe select new WoWPlayer(a.GetBaseAddress)).ToList();
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
                var objectReturn = new WoWUnit(0);
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

        public static WoWUnit GetNearestWoWUnit(List<WoWUnit> listWoWUnit)
        {
            try
            {
                var objectReturn = new WoWUnit(0);
                float tempDistance = 9999999.0f;
                foreach (WoWUnit a in listWoWUnit)
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
                Logging.WriteError("GetNearestWoWUnit(List<WoWUnit> listWoWUnit): " + e);
            }
            return new WoWUnit(0);
        }

        public static WoWPlayer GetNearestWoWPlayer(List<WoWPlayer> listWoWPlayer)
        {
            try
            {
                var objectReturn = new WoWPlayer(0);
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
                var objectReturn = new WoWGameObject(0);
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
                var objectReturn = new WoWGameObject(0);
                var tempDistance = 9999999.0f;
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
                var objectReturn = new List<WoWUnit>();
                foreach (var a in listWoWUnit)
                {
                    try
                    {
                        if (factions.Contains(a.Faction) && !a.IsDead && (pvp || !a.InCombat || a.InCombatWithMe))
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
                var factions = new List<uint> {faction};
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
                return
                    GetObjectWoWUnit()
                        .Where(
                            a =>
                            UnitRelation.GetReaction(Me.Faction, a.Faction) == Reaction.Hostile && !a.IsDead &&
                            (!a.InCombat || a.InCombatWithMe))
                        .ToList();
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
                return GetObjectWoWUnit().Count(a => point.DistanceTo(a.Position) <= distanceSearch && !a.IsDead);
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
                return listWoWUnit.Where(a => names.Contains(a.Name) && !a.IsDead).ToList();
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
                var names = new List<string> {name};
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
                return listWoWUnit.Where(a => entrys.Contains(a.Entry) && !a.IsDead).ToList();
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
                var entrys = new List<int> {entry};
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
                var namesTemps = names.Select(s => s.ToLower()).ToList();
                return listWoWGameObject.Where(a => namesTemps.Contains(a.Name.ToLower())).ToList();
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
                var names = new List<string> {name};
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
                return listWoWGameObject.Where(a => displayId.Contains(a.DisplayId)).ToList();
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
                var tListInt = new List<int> {displayId};
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
                return listWoWGameObject.Where(a => entry.Contains(a.Entry)).ToList();
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
                var tListInt = new List<int> {entry};
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
                return listWoWGameObject.Where(a => id.Contains(a.Entry)).ToList();
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
                return
                    GetObjectWoWGameObject()
                        .Where(a => (a.GOType == WoWGameObjectType.Chest || a.GOType == WoWGameObjectType.Goober))
                        .ToList();
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
                var tListInt = new List<int> {id};
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
                return listWoWUnit.Where(a => a.IsLootable).ToList();
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
                return listWoWPlayer.Where(a => a.IsLootable).ToList();
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
                return listWoWPlayer.Where(a => a.IsDead).ToList();
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
                return listWoWUnit.Where(a => a.IsSkinnable && !withoutGuid.Contains(a.Guid)).ToList();
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

        public static int GetNumberAttackPlayer()
        {
            try
            {
                return GetUnitAttackPlayer().Count;
            }
            catch (Exception e)
            {
                Logging.WriteError("GetNumberAttackPlayer(): " + e);
            }
            return 0;
        }

        public static List<WoWUnit> GetUnitAttackPlayer(List<WoWUnit> listWoWUnit)
        {
            try
            {
                var objectReturn = new List<WoWUnit>();
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
                var tp = new List<WoWPlayer>();

                tp.AddRange(Me.PlayerFaction.ToLower() == "horde" ? GetWoWUnitAlliance() : GetWoWUnitHorde());

                var tu = tp.Select(t => new WoWUnit(t.GetBaseAddress)).ToList();

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
                return listWoWUnit.Where(a => a.IsNpcRepair && !a.IsNpcInnkeeper).ToList();
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
                return listWoWUnit.Where(a => a.IsNpcVendorFood && !a.IsNpcInnkeeper).ToList();
            }
            catch (Exception e)
            {
                Logging.WriteError("GetWoWUnitVendorFood(List<WoWUnit> listWoWUnit): " + e);
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
                return listWoWUnit.Where(a => a.IsNpcVendor && !a.IsNpcInnkeeper).ToList();
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
                return listWoWUnit.Where(a => a.IsNpcTrainer).ToList();
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

        public static List<WoWUnit> GetWoWUnitSpiritHealer(List<WoWUnit> listWoWUnit)
        {
            try
            {
                return listWoWUnit.Where(a => a.IsNpcSpiritHealer).ToList();
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

        public static List<WoWPlayer> GetWoWUnitAlliance(List<WoWPlayer> listWoWUnit)
        {
            try
            {
                return listWoWUnit.Where(a => a.PlayerFaction == "Alliance" && !a.IsDead && a.SummonedBy == 0).ToList();
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
                return listWoWUnit.Where(a => a.PlayerFaction == "Horde" && !a.IsDead && a.SummonedBy == 0).ToList();
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
                return listWoWUnit.Where(a => a.PlayerFaction == "Alliance" && a.IsDead && a.SummonedBy == 0).ToList();
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
                return listWoWUnit.Where(a => a.PlayerFaction == "Horde" && a.IsDead && a.SummonedBy == 0).ToList();
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
            var woWPlayerList = new List<WoWPlayer>();
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