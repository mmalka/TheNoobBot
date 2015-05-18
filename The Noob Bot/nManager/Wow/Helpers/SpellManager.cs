using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.MemoryClass.Magic;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public class SpellManager
    {
        private static readonly List<uint> MountDruidIdList = new List<uint>();

        private static readonly List<uint> FlightFormsIdsList = new List<uint>();
        private static List<UInt32> _spellBookID = new List<UInt32>();
        private static bool _usedSbid;
        public static bool SpellBookLoaded;
        private static List<Spell> _spellBookSpell = new List<Spell>();
        public static Dictionary<uint, SpellInfoLua> _spellInfos = new Dictionary<uint, SpellInfoLua>();
        private static readonly Dictionary<string, List<uint>> CacheSpellIdByName = new Dictionary<string, List<uint>>();

        public static int GetGlobalCooldownLeft
        {
            get
            {
                List<SpellCooldownEntry> spellsOnCooldownList = GetAllSpellsOnCooldown;
                foreach (SpellCooldownEntry spellCooldown in spellsOnCooldownList)
                {
                    uint currentWoWTime = Usefuls.GetWoWTime;
                    if (spellCooldown.GCDDuration > 0)
                    {
                        int timeLeftMs = (int) (spellCooldown.GCDStartTime - currentWoWTime + spellCooldown.GCDDuration) + 50;
                        return timeLeftMs < 0 ? 50 : timeLeftMs;
                        // A GCD was recently active as their were footsteps in the memory, we want it to wait a minimum of 50ms.
                        // The bot will less spam an incomming spell.
                    }
                }
                return 1; // 0 would cause sleeps to freezes thread.
                // A GCD was not recently active, so just don't lose time and cast the next.
            }
        }

        public static List<SpellCooldownEntry> GetAllSpellsOnCooldown
        {
            get
            {
                BlackMagic m = Memory.WowMemory.Memory;

                uint currentListObject = m.ReadUInt(Memory.WowProcess.WowModule + (uint) Addresses.Player.LocalPlayerSpellsOnCooldown + 8);

                var spellCooldowns = new List<SpellCooldownEntry>();
                while ((currentListObject != 0) && ((currentListObject & 1) == 0))
                {
                    var spellCooldown = (SpellCooldownEntry) m.ReadObject(currentListObject, typeof (SpellCooldownEntry));

                    /*Logging.Write("-----------------------------------------------");
                    Logging.Write("SpellCooldownEntry: SpellId = " + spellCooldown.SpellId + ", ItemId = " + spellCooldown.ItemId);
                    Logging.Write("SpellCooldownEntry: SpellName = " + SpellListManager.SpellNameById(spellCooldown.SpellId) + ", ItemId = " + ItemsManager.GetItemNameById((int) spellCooldown.ItemId));
                    Logging.Write("StartTime: " + spellCooldown.StartTime + ", GetTime: " + Usefuls.GetWoWTime + ", " + (int) (Usefuls.GetWoWTime - spellCooldown.StartTime) + " miliseconds ago.");
                    Logging.Write("Cooldown Duration: " + spellCooldown.SpellOrItemCooldownDuration + "ms, CategoryCooldownDuration: " + spellCooldown.CategoryCooldownDuration + "ms.");
                    Logging.Write("GlobalCooldown Duration: " + spellCooldown.GCDDuration);
                    Logging.Write("SpellHasCooldown: " + (spellCooldown.HasCooldown == 1));
                    Logging.Write("SpellCategoryId: " + spellCooldown.SpellCategoryId);
                    Logging.Write("StartRecoveryCategoryId: " + spellCooldown.StartRecoveryCategoryId);*/

                    spellCooldowns.Add(spellCooldown);
                    currentListObject = m.ReadUInt(currentListObject + 0x4);
                }
                return spellCooldowns;
            }
        }

        public static List<uint> MountDruidId()
        {
            try
            {
                if (MountDruidIdList.Count <= 0)
                {
                    //MountDruidIdList.AddRange(SpellListManager.SpellIdByName("Swift Flight Form"));
                    //MountDruidIdList.AddRange(SpellListManager.SpellIdByName("Flight Form"));
                    //MountDruidIdList.AddRange(SpellListManager.SpellIdByName("Aquatic Form"));
                    MountDruidIdList.AddRange(SpellListManager.SpellIdByName("Travel Form"));
                    MountDruidIdList.AddRange(SpellListManager.SpellIdByName("Sky Golem"));
                }
                return MountDruidIdList;
            }
            catch (Exception exception)
            {
                Logging.WriteError("MountDruidId(): " + exception);
            }
            return new List<uint>();
        }

        public static List<uint> FlightFormsIds()
        {
            try
            {
                if (FlightFormsIdsList.Count <= 0)
                {
                    FlightFormsIdsList.AddRange(SpellListManager.SpellIdByName("Swift Flight Form"));
                    FlightFormsIdsList.AddRange(SpellListManager.SpellIdByName("Flight Form"));
                    // Sky Golem is not a flight form, and wont cause trouble to farm underwater.
                }
                return FlightFormsIdsList;
            }
            catch (Exception exception)
            {
                Logging.WriteError("MountDruidId(): " + exception);
            }
            return new List<uint>();
        }

        public static bool IsSpellUsableLUA(Spell spell)
        {
            try
            {
                if (GetSpellCooldown(spell.Id, spell.CategoryId, spell.StartRecoveryCategoryId) > 0)
                    return false;
                // We only need LUA to check for ressources now.

                string luaVarUsable = Others.GetRandomString(Others.Random(4, 10));
                string luaVarNoMana = Others.GetRandomString(Others.Random(4, 10));
                string luaResultUsable = Others.GetRandomString(Others.Random(4, 10));

                string luaCode = luaVarUsable + "," + luaVarNoMana + "=IsUsableSpell(\"" + spell.NameInGame + "\"); ";
                luaCode += "if " + luaVarUsable + " and not " + luaVarNoMana + " then ";
                luaCode += luaResultUsable + "=\"1\" ";
                luaCode += "end ";

                Lua.LuaDoString(luaCode, false, false);
                string ret = Lua.GetLocalizedText(luaResultUsable);

                return (ret == "1");
            }
            catch (Exception exception)
            {
                Logging.WriteError("IsSpellReadyInMs(Spell spell, int time): " + exception);
                return false;
            }
        }

        public static int GetSpellCooldown(List<uint> spellIds, uint categoryId = 0, uint startRecoveryCategoryId = 0)
        {
            for (int i = 0; i < spellIds.Count; i++)
            {
                uint spellId = spellIds[i];
                int timeLeft = GetSpellCooldown(spellId, categoryId, startRecoveryCategoryId);
                if (timeLeft > 0)
                    return timeLeft;
            }
            return 0;
        }

        public static int GetSpellCooldown(uint spellId, uint categoryId = 0, uint startRecoveryCategoryId = 0)
        {
            List<SpellCooldownEntry> spellsOnCooldownList = GetAllSpellsOnCooldown;
            foreach (SpellCooldownEntry spellCooldown in spellsOnCooldownList)
            {
                if (spellCooldown.GCDDuration > 0)
                    continue; // We don't want to check the GCD entry. 

                if (spellCooldown.SpellId != spellId && (spellCooldown.SpellCategoryId != categoryId || categoryId == 0))
                    continue;
                // This function doesn't need further editing, we are already checking for CategoryId too here,
                // All we have to fix, is the call to this functions, using CategoryId
                uint currentWoWTime = Usefuls.GetWoWTime;
                int timeLeftMs;
                if (spellCooldown.SpellOrItemCooldownDuration > 0 && spellCooldown.SpellId == spellId)
                {
                    // Force the SpellOrItemCooldownDuration to use SpellId as a reference instead of CategoryId.
                    timeLeftMs = (int) (spellCooldown.StartTime - currentWoWTime + spellCooldown.SpellOrItemCooldownDuration);
                    // The spell is maybe going to be ready in a few miliseconds, we want to be sure we slow down it with Latency so the bot wont try to cast it before it's really available.
                    timeLeftMs = timeLeftMs + Usefuls.Latency;
                    return timeLeftMs < 0 ? 0 : timeLeftMs;
                }
                if (spellCooldown.CategoryCooldownDuration <= 0)
                    continue;
                timeLeftMs = (int) (spellCooldown.CategoryCooldownStartTime - currentWoWTime + spellCooldown.CategoryCooldownDuration);
                // The spell is maybe going to be ready in a few miliseconds, we want to be sure we slow down it with Latency so the bot wont try to cast it before it's really available.
                timeLeftMs = timeLeftMs + Usefuls.Latency;
                return timeLeftMs < 0 ? 0 : timeLeftMs;
            }
            return 0;
            // Cooldown is totally up and wasn't recently in memory, we don't need to slow down it because it's 100% ready.
        }

        public static string GetClientNameBySpellName(List<string> spellList)
        {
            try
            {
                foreach (uint id in SpellBookID())
                {
                    string spellName = SpellListManager.SpellNameById(id);
                    if (spellList.Contains(spellName))
                    {
                        return spellName;
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetClientNameBySpellName(List<string> spellList): " + exception);
            }
            return "";
        }

        public static void CastSpellByNameLUA(string spellName)
        {
            try
            {
                Lua.LuaDoString("CastSpellByName(\"" + spellName + "\");");
            }
            catch (Exception exception)
            {
                Logging.WriteError("CastSpellByNameLUA(string spellName): " + exception);
            }
        }

        public static void CastSpellByNameLUA(string spellName, string target)
        {
            try
            {
                Lua.LuaDoString("CastSpellByName(\"" + spellName + "\", \"" + target + "\");");
            }
            catch (Exception exception)
            {
                Logging.WriteError("CastSpellByNameLUA(string spellName, string target): " + exception);
            }
        }

        public static void CastSpellByIDAndPosition(UInt32 spellId, Point postion)
        {
            try
            {
                ClickOnTerrain.Spell(spellId, postion);
            }
            catch (Exception exception)
            {
                Logging.WriteError("CastSpellByIDAndPosition(UInt32 spellId, Point postion): " + exception);
            }
        }

        public static void CastSpellByIdLUA(uint spellId)
        {
            try
            {
                var s = new Spell(spellId);
                CastSpellByNameLUA(s.NameInGame);
            }
            catch (Exception exception)
            {
                Logging.WriteError("CastSpellByIdLUA(uint spellId): " + exception);
            }
        }

        public static bool ExistMountLUA(string spellName)
        {
            try
            {
                string ret =
                    Lua.LuaDoString(
                        "ret = \"\"; nameclient = \"" + spellName +
                        "\"; for i=1,GetNumCompanions(\"MOUNT\"),1 do local _, name = GetCompanionInfo(\"MOUNT\", i)  if name == nameclient then ret = \"true\"  return end  end",
                        "ret");
                return ret == "true";
            }
            catch (Exception exception)
            {
                Logging.WriteError("ExistSpellLUA(string spellName): " + exception);
                return false;
            }
        }

        public static bool HaveBuffLua(string spellNameInGame)
        {
            try
            {
                lock (typeof (SpellManager))
                {
                    string randomStringResult = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(randomStringResult +
                                    " = \"false\" for i=1,40 do local n,_,_,_,_,_,_,_,id=UnitBuff(\"player\",i); if n == \"" +
                                    spellNameInGame + "\" then " + randomStringResult + " = \"true\" end end");
                    string sResult = Lua.GetLocalizedText(randomStringResult);
                    return (sResult == "true");
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("HaveBuffLua(string spellNameInGame): " + exception);
            }
            return false;
        }

        public static Spell SpellInfoLUA(uint spellID)
        {
            try
            {
                return new Spell(spellID);
            }
            catch (Exception exception)
            {
                Logging.WriteError("SpellInfoLUA(uint spellID): " + exception);
            }
            return new Spell("");
        }

        public static Spell GetSpellInfoLUA(string spellNameInGame)
        {
            try
            {
                string randomStringResult = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString("_, " + randomStringResult + " = GetSpellBookItemInfo(\"" + spellNameInGame + "\")");
                string sResult = Lua.GetLocalizedText(randomStringResult);
                return new Spell(sResult);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetSpellInfoLUA(string spellNameInGame): " + exception);
            }
            return new Spell("");
        }

        public static uint GetSpellIdBySpellNameInGame(string spellName)
        {
            try
            {
                string randomStringResult = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString("_, " + randomStringResult + " = GetSpellBookItemInfo(\"" + spellName + "\")");
                uint sResult = uint.Parse(Lua.GetLocalizedText(randomStringResult));
                return sResult;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetSpellInfoLUA(string spellNameInGame): " + exception);
            }
            return 0;
        }

        public static bool KnownSpell(uint spellId)
        {
            return SpellBookID().Contains(spellId);
        }

        public static bool HasSpell(int spellId)
        {
            if (spellId > (int) Addresses.SpellBook.SpellDBCMaxIndex)
                return false;
            int value = Memory.WowMemory.Memory.ReadInt(Memory.WowMemory.Memory.ReadUInt((uint) ((Memory.WowProcess.WowModule + (uint) Addresses.SpellBook.KnownAllSpells) + (spellId >> 5)*4)));
            return ((value & ~(1 << (spellId & 0x1F))) != 0);
        }

        public static bool ExistSpellBookLUA(string spellName)
        {
            try
            {
                string randomStringResult = Others.GetRandomString(Others.Random(4, 10));
                string randomStringNameClient = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(randomStringResult + " = \"\"; " + randomStringNameClient + " = \"" + spellName +
                                "\"; if (GetSpellBookItemInfo(" + randomStringNameClient + ")) then " +
                                randomStringResult + " = \"true\" else " + randomStringResult + " = \"false\" end");
                string sResult = Lua.GetLocalizedText(randomStringResult);
                if (sResult == "true")
                    return true;
                return false;
            }
            catch (Exception e)
            {
                Logging.WriteError("ExistSpellBookLUA(string spellName): " + e);
                return false;
            }
        }

        public static List<UInt32> SpellBookID()
        {
            try
            {
                while (_usedSbid)
                {
                    Thread.Sleep(10);
                }
                if (_spellBookID.Count <= 0)
                {
                    _usedSbid = true;
                    var spellBook = new List<uint>();

                    UInt32 nbSpells =
                        Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule +
                                                         (uint) Addresses.SpellBook.SpellBookNumSpells);
                    UInt32 spellBookInfoPtr =
                        Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule +
                                                         (uint) Addresses.SpellBook.SpellBookSpellsPtr);

                    for (UInt32 i = 0; i < nbSpells; i++)
                    {
                        uint Struct = Memory.WowMemory.Memory.ReadUInt(spellBookInfoPtr + i*4);
                        var si = (SpellInfo) Memory.WowMemory.Memory.ReadObject(Struct, typeof (SpellInfo));
                        if ((si.TabId <= 1 || si.TabId > 4) && si.State == SpellInfo.SpellState.Known)
                        {
                            spellBook.Add(si.ID);
                        }
                        Application.DoEvents();
                    }

                    UInt32 MountBookNumMounts = Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint) Addresses.SpellBook.MountBookNumMounts);
                    UInt32 MountBookMountsPtr = Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint) Addresses.SpellBook.MountBookMountsPtr);

                    for (UInt32 i = 0; i < MountBookNumMounts; i++)
                    {
                        uint MountId = Memory.WowMemory.Memory.ReadUInt(MountBookMountsPtr + i*4);
                        if (MountId > 0)
                        {
                            spellBook.Add(MountId);
                        }
                        Application.DoEvents();
                    }

                    uint pTalentSpell = Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint) Addresses.SpellBook.FirstTalentBookPtr);
                    uint talentSpellNext = Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint) Addresses.SpellBook.NextTalentBookPtr);

                    while (((int) pTalentSpell & 1) == 0 && pTalentSpell != 0)
                    {
                        uint originalSpellId = Memory.WowMemory.Memory.ReadUInt(pTalentSpell + (uint) Addresses.SpellBook.TalentBookSpellId);
                        uint talentOverrideSpellId = Memory.WowMemory.Memory.ReadUInt(pTalentSpell + (uint) Addresses.SpellBook.TalentBookOverrideSpellId);
                        uint nextSpell = Memory.WowMemory.Memory.ReadUInt((pTalentSpell + 4 + talentSpellNext));
                        pTalentSpell = nextSpell;

                        if (talentOverrideSpellId == 0)
                            break;

                        spellBook.Remove(originalSpellId);
                        spellBook.Add(talentOverrideSpellId);
                    }
                    _spellBookID = spellBook;
                    _usedSbid = false;
                    SpellBookLoaded = true;
                }
                return _spellBookID;
            }
            catch (Exception exception)
            {
                Logging.WriteError("SpellBookID(): " + exception);
            }
            return new List<uint>();
        }

        public static void UpdateSpellBookThread()
        {
            try
            {
                Logging.Write("Initialize Character's SpellBook update.");
                uint nbSpells =
                    Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint) Addresses.SpellBook.SpellBookNumSpells);
                uint spellBookInfoPtr =
                    Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint) Addresses.SpellBook.SpellBookSpellsPtr);

                for (UInt32 i = 0; i < nbSpells; i++)
                {
                    uint Struct = Memory.WowMemory.Memory.ReadUInt(spellBookInfoPtr + i*4);
                    var si = (SpellInfo) Memory.WowMemory.Memory.ReadObject(Struct, typeof (SpellInfo));
                    if ((si.TabId <= 1 || si.TabId > 4) && si.State == SpellInfo.SpellState.Known)
                    {
                        if (!_spellBookID.Contains(si.ID))
                            _spellBookID.Add(si.ID);
                        Spell Spell = SpellInfoLUA(si.ID);
                        if (!_spellBookSpell.Contains(Spell))
                            _spellBookSpell.Add(Spell);
                    }
                    Application.DoEvents();
                }

                UInt32 MountBookNumMounts =
                    Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule +
                                                     (uint) Addresses.SpellBook.MountBookNumMounts);
                UInt32 MountBookMountsPtr =
                    Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule +
                                                     (uint) Addresses.SpellBook.MountBookMountsPtr);

                for (UInt32 i = 0; i < MountBookNumMounts; i++)
                {
                    uint MountId = Memory.WowMemory.Memory.ReadUInt(MountBookMountsPtr + i*4);
                    if (MountId > 0)
                    {
                        if (!_spellBookID.Contains(MountId))
                            _spellBookID.Add(MountId);
                        Spell MountSpell = SpellInfoLUA(MountId);
                        if (!_spellBookSpell.Contains(MountSpell))
                            _spellBookSpell.Add(MountSpell);
                    }
                    Application.DoEvents();
                }

                Logging.Write("Character's SpellBook is currently being fully updated. May take few seconds...");
                foreach (Spell o in _spellBookSpell)
                {
                    o.Update();
                }

                if (CombatClass.IsAliveCombatClass)
                {
                    CombatClass.ResetCombatClass();
                }
                if (HealerClass.IsAliveHealerClass)
                {
                    HealerClass.ResetHealerClass();
                }
                Plugins.Plugins.ReLoadPlugins();
                Logging.Write("Character's SpellBook fully updated. Found " + _spellBookID.Count + " spells, mounts and professions.");
            }
            catch (Exception exception)
            {
                Logging.WriteError("UpdateSpellBook(): " + exception);
            }
        }

        public static void UpdateSpellBook()
        {
            var spellBook = new Thread(UpdateSpellBookThread) {Name = "SpellBook Update"};
            spellBook.Start();
        }

        public static List<Spell> SpellBook()
        {
            try
            {
                lock ("SpellBook")
                {
                    if (_spellBookSpell.Count <= 0)
                    {
                        Logging.Write("Initializing Character's SpellBook.");
                        SpellInfoCreateCache(SpellBookID());
                        SpellListManager.SpellIdByNameCreateCache();
                        var spellBook = new List<Spell>();
                        Logging.Write("May take few seconds...");
                        foreach (uint id in SpellBookID())
                        {
                            spellBook.Add(new Spell(id));
                        }
                        Logging.Write("Character's SpellBook fully loaded. Found " + _spellBookID.Count + " spells, mounts and professions.");
                        Logging.Status = "Character SpellBook loaded.";
                        _spellBookSpell = spellBook;
                    }
                }
                return _spellBookSpell;
            }
            catch (Exception exception)
            {
                Logging.WriteError("SpellBook(): " + exception);
            }
            return new List<Spell>();
        }

        // Localized spell names

        public static SpellInfoLua GetSpellInfo(uint id)
        {
            try
            {
                lock ("GetSpellInfo" + id)
                {
                    if (_spellInfos.ContainsKey(id))
                        return _spellInfos[id];
                    string randomString = Others.GetRandomString(Others.Random(5, 10));
                    string result = Lua.LuaDoString(
                        randomString + " = \"\"; " +
                        "local name, icon, castTime, minRange, maxRange, spellId = GetSpellInfo(" + id + "); " +
                        randomString +
                        " = tostring(name) .. \"##\" .. tostring(icon) .. \"##\" .. tostring(castTime) .. \"##\" .. tostring(minRange)  .. \"##\" .. tostring(maxRange)  .. \"##\" .. tostring(spellId);"
                        , randomString);
                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        string[] ar = {"##"};
                        string[] slipped = result.Split(ar, StringSplitOptions.None);
                        if (slipped.Length == 6)
                        {
                            var spellInfo = new SpellInfoLua();
                            int intOut;
                            float floatOut;

                            // ID
                            spellInfo.ID = id;
                            // name
                            if (!string.IsNullOrWhiteSpace(slipped[0]) && slipped[0] != "nil")
                                spellInfo.Name = slipped[0];
                            // icon
                            if (!string.IsNullOrWhiteSpace(slipped[2]))
                                spellInfo.Icon = slipped[2];
                            // castTime
                            if (!string.IsNullOrWhiteSpace(slipped[3]) && int.TryParse(slipped[3].Replace(".", ","), out intOut))
                                spellInfo.CastTime = intOut;
                            // minRange
                            if (!string.IsNullOrWhiteSpace(slipped[4]) && float.TryParse(slipped[4].Replace(".", ","), out floatOut))
                                spellInfo.MinRange = floatOut;
                            // maxRange
                            if (!string.IsNullOrWhiteSpace(slipped[5]) && float.TryParse(slipped[5].Replace(".", ","), out floatOut))
                                spellInfo.MaxRange = floatOut;

                            _spellInfos.Add(id, spellInfo);
                            return spellInfo;
                        }
                        Logging.WriteDebug("Return as bad format: public static SpellInfo GetSpellInfo(" + id + ")");
                    }
                    else
                    {
                        Logging.WriteDebug("Cannot find spell: public static SpellInfo GetSpellInfo(" + id + ")");
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("SpellInfo GetSpellInfo(uint id): " + exception);
            }
            return new SpellInfoLua();
        }

        // Create a localized cache for spell names
        public static void SpellInfoCreateCache(List<uint> listId)
        {
            try
            {
                lock ("SpellInfoCreateCache")
                {
                    string listIdString = "{ ";
                    foreach (uint id in listId)
                    {
                        listIdString += id + " ,";
                    }
                    if (listIdString.EndsWith(","))
                        listIdString = listIdString.Remove(listIdString.Length - 1);
                    listIdString += "}";

                    string randomString = Others.GetRandomString(Others.Random(5, 10));
                    string command = randomString + " = \"\"; " +
                                     "local spellBookList = " + listIdString + " " +
                                     "for arrayId = 1, table.getn(spellBookList) do " +
                                     "local name, icon, castTime, minRange, maxRange, spellId = GetSpellInfo(spellBookList[arrayId]); " +
                                     randomString + " = " + randomString +
                                     " .. tostring(name) .. \"##\" .. tostring(icon) .. \"##\" .. tostring(castTime) .. \"##\" .. tostring(minRange)  .. \"##\" .. tostring(maxRange)  .. \"##\" .. tostring(spellId);" +
                                     randomString + " = " + randomString + " .. \"||\"" +
                                     "end ";
                    string result = Lua.LuaDoString(command, randomString);
                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        string[] listSpell = result.Split(new[] {"||"}, StringSplitOptions.None);
                        if (listSpell.Length > 0)
                        {
                            foreach (string s in listSpell)
                            {
                                if (string.IsNullOrWhiteSpace(s))
                                    break;
                                string[] slipped = s.Split(new[] {"##"}, StringSplitOptions.None);
                                if (slipped.Length == 6)
                                {
                                    var spellInfo = new SpellInfoLua();
                                    int intOut;
                                    float floatOut;

                                    // name
                                    if (!string.IsNullOrWhiteSpace(slipped[0]) && slipped[0] != "nil")
                                        spellInfo.Name = slipped[0];
                                    // icon
                                    if (!string.IsNullOrWhiteSpace(slipped[1]))
                                        spellInfo.Icon = slipped[1];
                                    // castTime
                                    if (!string.IsNullOrWhiteSpace(slipped[2]) &&
                                        int.TryParse(slipped[2].Replace(".", ","), out intOut))
                                        spellInfo.CastTime = intOut;
                                    // minRange
                                    if (!string.IsNullOrWhiteSpace(slipped[3]) &&
                                        float.TryParse(slipped[3].Replace(".", ","), out floatOut))
                                        spellInfo.MinRange = floatOut;
                                    // maxRange
                                    if (!string.IsNullOrWhiteSpace(slipped[4]) &&
                                        float.TryParse(slipped[4].Replace(".", ","), out floatOut))
                                        spellInfo.MaxRange = floatOut;
                                    // ID
                                    if (!string.IsNullOrWhiteSpace(slipped[5]) &&
                                        int.TryParse(slipped[5].Replace(".", ","), out intOut))
                                        spellInfo.ID = (uint) intOut;

                                    if (listId.Contains(spellInfo.ID))
                                    {
                                        if (!_spellInfos.ContainsKey(spellInfo.ID))
                                            _spellInfos.Add(spellInfo.ID, spellInfo);
                                    }
                                }
                                else
                                {
                                    Logging.WriteDebug("Return as bad format: public static SpellInfo SpellInfoCreateCache()");
                                }
                            }
                        }
                    }
                    else
                    {
                        Logging.WriteDebug("Cannot find spell: public static SpellInfo SpellInfoCreateCache()");
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("SpellInfo GetSpellInfo(uint id): " + exception);
            }
        }

        public static string GetMountName()
        {
            try
            {
                var mountList =
                    new List<string>(Others.ReadFileAllLines(Application.StartupPath + "\\Data\\mountList.txt"));

                string key = GetClientNameBySpellName(mountList);
                if (key != "")
                    Logging.Write("Found mount: " + key);
                return key;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetMountName(): " + exception);
            }
            return "";
        }

        public static string GetFlyMountName()
        {
            try
            {
                var flyMountList =
                    new List<string>(Others.ReadFileAllLines(Application.StartupPath + "\\Data\\flymountList.txt"));

                string key = GetClientNameBySpellName(flyMountList);
                if (key != "")
                    Logging.Write("Found flying mount: " + key);
                return key;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetFlyMountName(): " + exception);
            }
            return "";
        }

        public static string GetAquaticMountName()
        {
            try
            {
                var aquaticMountList =
                    new List<string>(Others.ReadFileAllLines(Application.StartupPath + "\\Data\\aquaticmountList.txt"));

                string key = GetClientNameBySpellName(aquaticMountList);
                if (key != "")
                    Logging.Write("Found aquatic mount: " + key);
                return key;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetAquaticMountName(): " + exception);
            }
            return "";
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SpellCooldownEntry
        {
            public uint Previous;
            public uint Next;
            public uint SpellId;
            public uint ItemId;
            public int StartTime;
            public int SpellOrItemCooldownDuration;
            public uint SpellCategoryId;
            public int CategoryCooldownStartTime;
            public int CategoryCooldownDuration;
            public byte HasCooldown;
            public byte pad1, pad2, pad3;
            public int GCDStartTime;
            public uint StartRecoveryCategoryId;
            public int GCDDuration;
        }

        [StructLayout(LayoutKind.Explicit, Size = 0x10)]
        private struct SpellInfo
        {
            public enum SpellState : uint
            {
                Known = 1, // the spell has been learnt and can be cast
                // ReSharper disable UnusedMember.Local
                FutureSpell = 2, // the spell is known but not yet learnt
                PetAction = 3,
                Flyout = 4
                // ReSharper restore UnusedMember.Local
            };

            /// <summary>
            ///     The state of the spell in the spell book
            /// </summary>
            [FieldOffset(0x0)] public readonly SpellState State;

            /// <summary>
            ///     The spell identifier of the spell in the spell book
            /// </summary>
            [FieldOffset(0x4)] public readonly uint ID; // it's an int in client, but we don't care

            /// <summary>
            ///     The level of the spell level in the spell book
            /// </summary>
            [FieldOffset(0x8)] public readonly uint Level;

            /// <summary>
            ///     The tab where the spell is stored in the spell book
            /// </summary>
            [FieldOffset(0xC)] public readonly uint TabId;
        }

        public class SpellInfoLua
        {
            public int CastTime;
            public int Cost;
            public uint ID;
            public string Icon = "";
            public bool IsFunnel;
            public float MaxRange;
            public float MinRange;
            public string Name = "";
            public PowerType PowerType;
            public string Rank = "";
        }

        public class SpellListManager
        {
            // English spell dictionary (id, name)
            private static readonly object LoadSpellListLock = new object();
            public static Dictionary<uint, string> ListSpell { get; private set; }

            // Load data\spell.txt (English)
            internal static void LoadSpellList(string fileName)
            {
                try
                {
                    lock (LoadSpellListLock)
                    {
                        if (ListSpell == null)
                        {
                            var tListSpell = new Dictionary<uint, string>();
                            string[] listSpellTemps = Others.ReadFileAllLines(fileName);
                            foreach (string tempsSpell in listSpellTemps)
                            {
                                if (string.IsNullOrWhiteSpace(tempsSpell) || !tempsSpell.Contains(";")) continue;

                                string[] tmpSpell = tempsSpell.Split(';');

                                if (tmpSpell.Length != 2 || string.IsNullOrWhiteSpace(tmpSpell[0]) || string.IsNullOrWhiteSpace(tmpSpell[1])) continue;

                                uint tspellId = Others.ToUInt32(tmpSpell[0]);
                                if (!tListSpell.ContainsKey(tspellId))
                                {
                                    tListSpell.Add(tspellId, tmpSpell[1]);
                                }
                            }

                            ListSpell = tListSpell;
                        }
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("LoadSpellListe(string fileName): " + exception);
                    if (ListSpell == null)
                        Logging.WriteError("ListSpell == null");
                    else
                        Logging.WriteError("ListSpell.Count = " + ListSpell.Count);
                }
            }

            // Get the spell id using the provided English spell name
            public static List<uint> SpellIdByName(string spellName)
            {
                lock ("SpellIdByName")
                {
                    var listIdSpellFound = new List<UInt32>();
                    try
                    {
                        spellName = spellName.ToLower();

                        if (CacheSpellIdByName.TryGetValue(spellName, out listIdSpellFound))
                            return listIdSpellFound;

                        listIdSpellFound = new List<uint>();
                        foreach (var spell in ListSpell)
                        {
                            if (spell.Value.ToLower() == spellName)
                                listIdSpellFound.Add(spell.Key);
                        }

                        CacheSpellIdByName.Add(spellName, listIdSpellFound);

                        return listIdSpellFound;
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError("SpellIdByName(string spellName): " + exception);
                        return listIdSpellFound;
                    }
                }
            }

            public static void SpellIdByNameCreateCache()
            {
                lock ("SpellIdByName")
                {
                    try
                    {
                        foreach (var spell in ListSpell)
                        {
                            string name = spell.Value.ToLower();
                            if (!CacheSpellIdByName.ContainsKey(name))
                                CacheSpellIdByName.Add(name, new List<uint> {spell.Key});
                            else if (!CacheSpellIdByName[name].Contains(spell.Key))
                                CacheSpellIdByName[name].Add(spell.Key);
                        }
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError("SpellIdByNameCreateCache(): " + exception);
                    }
                }
            }

            // return an English spell name from the id
            public static string SpellNameById(UInt32 spellId)
            {
                try
                {
                    if (ListSpell.ContainsKey(spellId))
                        return ListSpell[spellId];
                }
                catch (Exception exception)
                {
                    Logging.WriteError("SpellNameById(UInt32 spellId): " + exception);
                }
                return "";
            }
        }
    }
}