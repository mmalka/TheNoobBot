using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public class SpellManager
    {
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
            /// The state of the spell in the spell book
            /// </summary>
            [FieldOffset(0x0)] public readonly SpellState State;

            /// <summary>
            /// The spell identifier of the spell in the spell book
            /// </summary>
            [FieldOffset(0x4)] public readonly uint ID; // it's an int in client, but we don't care

            /// <summary>
            /// The level of the spell level in the spell book
            /// </summary>
            [FieldOffset(0x8)] public readonly uint Level;

            /// <summary>
            /// The tab where the spell is stored in the spell book
            /// </summary>
            [FieldOffset(0xC)] public readonly uint TabId;
        }

        private static readonly List<uint> MountDruidIdList = new List<uint>();

        public static List<uint> MountDruidId()
        {
            try
            {
                if (MountDruidIdList.Count <= 0)
                {
                    MountDruidIdList.AddRange(SpellListManager.SpellIdByName("Swift Flight Form"));
                    MountDruidIdList.AddRange(SpellListManager.SpellIdByName("Flight Form"));
                    MountDruidIdList.AddRange(SpellListManager.SpellIdByName("Aquatic Form"));
                }
                return MountDruidIdList;
            }
            catch (Exception exception)
            {
                Logging.WriteError("MountDruidId(): " + exception);
            }
            return new List<uint>();
        }

        public static string GetSlotBarBySpellName(string spell)
        {
            try
            {
                List<string> spellList = new List<string> {spell};
                return GetSlotBarBySpellName(spellList);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetSlotBarBySpellName(string spell): " + exception);
            }
            return "";
        }

        public static string GetSlotBarBySpellName(List<string> spellList)
        {
            try
            {
                for (int i = (int) Memory.WowProcess.WowModule + (int) Addresses.BarManager.startBar;
                     i <= (int) Memory.WowProcess.WowModule + (int) Addresses.BarManager.startBar + 0x11C;
                     // To be updated.
                     i = i + (int) Addresses.BarManager.nextSlot)
                {
                    uint sIdt = Memory.WowMemory.Memory.ReadUInt((uint) i);
                    if (sIdt != 0)
                    {
                        if (spellList.Contains(SpellListManager.SpellNameById(sIdt)))
                        {
                            int j = ((i - ((int) Memory.WowProcess.WowModule + (int) Addresses.BarManager.startBar))/
                                     (int) Addresses.BarManager.nextSlot);
                            int k = 0;
                            while (true)
                            {
                                if (j - 12 >= 0)
                                {
                                    j = j - 12;
                                    k++;
                                }
                                else
                                {
                                    return (k + 1) + ";" + (j + 1);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetSlotBarBySpellName(List<string> spellList): " + exception);
            }
            return "";
        }

        public static string GetSlotBarBySpellId(UInt32 spellId)
        {
            try
            {
                for (int i = (int) Memory.WowProcess.WowModule + (int) Addresses.BarManager.startBar;
                     i <= (int) Memory.WowProcess.WowModule + (int) Addresses.BarManager.startBar + 0x11C;
                     // To be updated.
                     i = i + (int) Addresses.BarManager.nextSlot)
                {
                    if (Memory.WowMemory.Memory.ReadUInt((uint) i) == spellId)
                    {
                        int j = (i - (int) Memory.WowProcess.WowModule + (int) Addresses.BarManager.startBar)/
                                (int) Addresses.BarManager.nextSlot;
                        int k = 0;
                        while (true)
                        {
                            if (j - 12 > 0)
                            {
                                j = j - 12;
                                k++;
                            }
                            else
                            {
                                return (k + 1) + ";" + (j + 1);
                            }

                            if (k > 20 || j > 20)
                                return "";
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetSlotBarBySpellId(UInt32 spellId): " + exception);
            }
            return "";
        }

        public static string GetClientNameBySpellName(List<string> spellList)
        {
            try
            {
                for (int i = (int) Memory.WowProcess.WowModule + (int) Addresses.BarManager.startBar;
                     i <= (int) Memory.WowProcess.WowModule + (int) Addresses.BarManager.startBar + 0x11C;
                     // To be updated.
                     i = i + (int) Addresses.BarManager.nextSlot)
                {
                    uint sIdt = Memory.WowMemory.Memory.ReadUInt((uint) i);
                    if (sIdt != 0)
                    {
                        if (spellList.Contains(SpellListManager.SpellNameById(sIdt)))
                        {
                            return SpellListManager.SpellNameById(sIdt);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetClientNameBySpellName(List<string> spellList): " + exception);
            }
            return "";
        }

        public static bool SlotIsEnable(string barAndSlot)
        {
            try
            {
                barAndSlot = barAndSlot.Replace("{", "");
                barAndSlot = barAndSlot.Replace("}", "");
                barAndSlot = barAndSlot.Replace(" ", "");
                string[] keySlot = barAndSlot.Split(';');


                if (Others.ToUInt32(keySlot[0]) == 1)
                {
                    int numBarOne = Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule +
                                                                    (uint) Addresses.BarManager.startBar);
                    if (numBarOne > 0)
                        keySlot[0] = (6 + (numBarOne)).ToString(CultureInfo.InvariantCulture);
                }

                uint adresse = Memory.WowProcess.WowModule + (uint) Addresses.BarManager.slotIsEnable +
                               (4*12*(Others.ToUInt32(keySlot[0]) - 1)) + (4*(Others.ToUInt32(keySlot[1]) - 1));

                return Memory.WowMemory.Memory.ReadUInt(adresse) == 1;
            }
            catch (Exception exception)
            {
                Logging.WriteError("SlotIsEnable(string barAndSlot): " + exception);
            }
            return false;
        }

        public static void LaunchSpellByName(string spellName)
        {
            try
            {
                string slotKeySpell = GetSlotBarBySpellName(spellName);
                if (slotKeySpell == "")
                    CastSpellByNameLUA(spellName);
                else
                    Keybindings.PressBarAndSlotKey(slotKeySpell);
            }
            catch (Exception exception)
            {
                Logging.WriteError("LaunchSpellByName(string spellName): " + exception);
            }
        }

        public static void LaunchSpellById(UInt32 spellId)
        {
            try
            {
                string slotKeySpell = GetSlotBarBySpellId(spellId);
                if (slotKeySpell == "")
                {
                    UInt32 spellIdTemps =
                        Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule +
                                                         (uint) Addresses.BarManager.startBar);
                    Memory.WowMemory.Memory.WriteUInt(
                        Memory.WowProcess.WowModule + (uint) Addresses.BarManager.startBar, spellId);
                    Keybindings.PressBarAndSlotKey("1;1");
                    Memory.WowMemory.Memory.WriteUInt(
                        Memory.WowProcess.WowModule + (uint) Addresses.BarManager.startBar, spellIdTemps);
                }
                else
                {
                    Keybindings.PressBarAndSlotKey(slotKeySpell);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("LaunchSpellById(UInt32 spellId): " + exception);
            }
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
                Spell s = new Spell(spellId);
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

        public static bool SpellUsableLUA(string spellName)
        {
            try
            {
                lock (typeof (SpellManager))
                {
                    string randomStringResult = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(" usable, nomana = IsUsableSpell(\"" + spellName +
                                    "\");  if (not usable) then   if (not nomana) then    " + randomStringResult +
                                    " = \"false\"   else     " + randomStringResult +
                                    " = \"false\"   end  else     start, duration, enabled = GetSpellCooldown(\"" +
                                    spellName + "\"); 	if start == 0 and duration == 0  then 	" + randomStringResult +
                                    " = \"true\" 	else 	" + randomStringResult + " = \"falseD\" 	end  end  ");
                    string sResult = Lua.GetLocalizedText(randomStringResult);
                    return (sResult == "true");
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("SpellUsableLUA(string spellName): " + exception);
            }
            return false;
        }

        public static bool QuickSpellUsableLUA(string spellName)
        {
            try
            {
                bool DecountLatency = true;
                uint ReducedMs = 130;
                bool DebugForTweaking = true;
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                lock (typeof (SpellManager))
                {
                    string randomStringResult = Others.GetRandomString(Others.Random(4, 10));
                    string randomStringTest = Others.GetRandomString(Others.Random(4, 10));
                    string randomStringUsable = Others.GetRandomString(Others.Random(4, 10));
                    string randomStringEnabled = Others.GetRandomString(Others.Random(4, 10));
                    string randomStringStartTime = Others.GetRandomString(Others.Random(4, 10));
                    string randomStringNoRessource = Others.GetRandomString(Others.Random(4, 10));
                    string randomStringDurationTime = Others.GetRandomString(Others.Random(4, 10));
                    string Ms = ReducedMs > 0 ? ReducedMs/1000f + "-" : "";
                    Lua.LuaDoString(
                        randomStringUsable + ", " + randomStringNoRessource + " = IsUsableSpell(\"" + spellName + "\"); " +
                        " if (not " + randomStringUsable + ") " +
                        "   then " +
                        "     if (not " + randomStringNoRessource + ") " +
                        "       then " +
                        "         " + randomStringResult + " = \"false\" " +
                        "       else " +
                        "         " + randomStringResult + " = \"false\"" +
                        "     end" +
                        "   else " +
                        "     " + randomStringStartTime + ", " + randomStringDurationTime + ", " + randomStringEnabled + " = GetSpellCooldown(\"" + spellName + "\"); " +
                        "     if " + randomStringStartTime + " == 0 and " + randomStringDurationTime + " == 0 " +
                        "       then " +
                        "         " + randomStringResult + " = \"true\" " +
                        "       else " +
                        "         " + randomStringTest + " = " + randomStringStartTime + "+" + randomStringDurationTime + "-GetTime()-" + Ms + "" +
                        (DecountLatency ? Usefuls.Latency > 0 ? Usefuls.Latency/1000f : 0 : 0) + " " +
                        "         if ( " + randomStringTest + " <= 0) " +
                        "           then " +
                        "             " + randomStringResult + " = \"Accelerated\" " +
                        "           else " +
                        "             " + randomStringResult + " = \"false\"; " +
                        "         end " +
                        "     end " +
                        " end");
                    string sResult = Lua.GetLocalizedText(randomStringResult);
                    if (DebugForTweaking && sResult == "Accelerated")
                        Logging.WriteFight("IsSpellUsable: The spell " + spellName + " have been forced Usable with " + Lua.GetLocalizedText(randomStringTest) +
                                           " seconds in advance. If this is not the next spell launched, optimize your settings.");
                    return sResult == "true" || sResult == "Accelerated";
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("SpellUsableLUA(string spellName): " + exception);
            }
            return false;
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

        private static List<UInt32> _spellBookID = new List<UInt32>();
        private static bool _usedSbid;
        public static bool SpellBookLoaded;

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
                    List<uint> spellBook = new List<uint>();

                    UInt32 nbSpells =
                        Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule +
                                                         (uint) Addresses.SpellBook.SpellBookNumSpells);
                    UInt32 spellBookInfoPtr =
                        Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule +
                                                         (uint) Addresses.SpellBook.SpellBookSpellsPtr);

                    for (UInt32 i = 0; i < nbSpells; i++)
                    {
                        uint Struct = Memory.WowMemory.Memory.ReadUInt(spellBookInfoPtr + i*4);
                        SpellInfo si = (SpellInfo) Memory.WowMemory.Memory.ReadObject(Struct, typeof (SpellInfo));
                        if ((si.TabId <= 1 || si.TabId > 4) && si.State == SpellInfo.SpellState.Known)
                        {
                            spellBook.Add(si.ID);
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
                            spellBook.Add(MountId);
                        }
                        Application.DoEvents();
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

        public static void UpdateSpellBook()
        {
            try
            {
                uint nbSpells =
                    Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint) Addresses.SpellBook.SpellBookNumSpells);
                uint spellBookInfoPtr =
                    Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint) Addresses.SpellBook.SpellBookSpellsPtr);

                for (UInt32 i = 0; i < nbSpells; i++)
                {
                    uint Struct = Memory.WowMemory.Memory.ReadUInt(spellBookInfoPtr + i*4);
                    SpellInfo si = (SpellInfo) Memory.WowMemory.Memory.ReadObject(Struct, typeof (SpellInfo));
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
            }
            catch (Exception exception)
            {
                Logging.WriteError("UpdateSpellBook(): " + exception);
            }
        }

        private static List<Spell> _spellBookSpell = new List<Spell>();

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
                        List<Spell> spellBook = new List<Spell>();
                        Logging.Write("May take few seconds...");
                        foreach (uint id in SpellBookID())
                        {
                            spellBook.Add(new Spell(id));
                        }
                        Logging.Write("Character's SpellBook fully loaded. Found " + _spellBookID.Count + " spells and professions.");
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

        private static Dictionary<uint, SpellInfoLua> _spellInfos = new Dictionary<uint, SpellInfoLua>();

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
                        "local name, rank, icon, cost, isFunnel, powerType, castTime, minRange, maxRange = GetSpellInfo(" + id + "); " +
                        randomString +
                        " = tostring(name) .. \"##\" .. tostring(rank) .. \"##\" .. tostring(icon) .. \"##\" .. tostring(cost)  .. \"##\" .. tostring(isFunnel)  .. \"##\" .. tostring(powerType)  .. \"##\" .. tostring(castTime)  .. \"##\" .. tostring(minRange)  .. \"##\" .. tostring(maxRange);"
                        , randomString);
                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        string[] ar = {"##"};
                        string[] slipped = result.Split(ar, StringSplitOptions.None);
                        if (slipped.Length == 9)
                        {
                            SpellInfoLua spellInfo = new SpellInfoLua();
                            int intOut;
                            float floatOut;

                            // ID
                            spellInfo.ID = id;
                            // name
                            if (!string.IsNullOrWhiteSpace(slipped[0]) && slipped[0] != "nil")
                                spellInfo.Name = slipped[0];
                            // rank
                            if (!string.IsNullOrWhiteSpace(slipped[1]))
                                spellInfo.Rank = slipped[1];
                            // icon
                            if (!string.IsNullOrWhiteSpace(slipped[2]))
                                spellInfo.Icon = slipped[2];
                            // cost
                            if (!string.IsNullOrWhiteSpace(slipped[3]) && int.TryParse(slipped[3].Replace(".", ","), out intOut))
                                spellInfo.Cost = intOut;
                            // isFunnel
                            if (!string.IsNullOrWhiteSpace(slipped[4]) && slipped[4].ToLower() == "true")
                                spellInfo.IsFunnel = true;
                            // powerType
                            if (!string.IsNullOrWhiteSpace(slipped[5]) && int.TryParse(slipped[5].Replace(".", ","), out intOut))
                                spellInfo.PowerType = (PowerType) intOut;
                            // castTime
                            if (!string.IsNullOrWhiteSpace(slipped[6]) && int.TryParse(slipped[6].Replace(".", ","), out intOut))
                                spellInfo.CastTime = intOut;
                            // minRange
                            if (!string.IsNullOrWhiteSpace(slipped[7]) && float.TryParse(slipped[7].Replace(".", ","), out floatOut))
                                spellInfo.MinRange = floatOut;
                            // maxRange
                            if (!string.IsNullOrWhiteSpace(slipped[8]) && float.TryParse(slipped[8].Replace(".", ","), out floatOut))
                                spellInfo.MaxRange = floatOut;

                            _spellInfos.Add(id, spellInfo);
                            return spellInfo;
                        }
                        else
                        {
                            Logging.WriteDebug("Return as bad format: public static SpellInfo GetSpellInfo(" + id + ")");
                        }
                    }
                    else
                    {
                        Logging.WriteDebug("Cannot found spell: public static SpellInfo GetSpellInfo(" + id + ")");
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("SpellInfo GetSpellInfo(uint id): " + exception);
            }
            return new SpellInfoLua();
        }

        public static List<SpellInfoLua> SpellInfoCreateCache(List<uint> listId)
        {
            try
            {
                lock ("SpellInfoCreateCache")
                {
                    List<SpellInfoLua> ret = new List<SpellInfoLua>();
                    string listIdString = "{ ";
                    foreach (uint id in listId)
                    {
                        listIdString += id + " ,";
                    }
                    if (listIdString.EndsWith(","))
                        listIdString = listIdString.Remove(listIdString.Length - 1);
                    listIdString += "}";

                    string randomString = Others.GetRandomString(Others.Random(5, 10));
                    string result = Lua.LuaDoString(
                        randomString + " = \"\"; " +
                        "local spellBookList = " + listIdString + " " +
                        "for arrayId = 1, table.getn(spellBookList) do " +
                        "local name, rank, icon, cost, isFunnel, powerType, castTime, minRange, maxRange = GetSpellInfo(spellBookList[arrayId]); " +
                        randomString + " = " + randomString +
                        " .. tostring(name) .. \"##\" .. tostring(rank) .. \"##\" .. tostring(icon) .. \"##\" .. tostring(cost)  .. \"##\" .. tostring(isFunnel)  .. \"##\" .. tostring(powerType)  .. \"##\" .. tostring(castTime)  .. \"##\" .. tostring(minRange)  .. \"##\" .. tostring(maxRange)  .. \"##\" .. tostring(spellBookList[arrayId]);" +
                        randomString + " = " + randomString + " .. \"||\"" +
                        "end "
                        , randomString);
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
                                if (slipped.Length == 10)
                                {
                                    SpellInfoLua spellInfo = new SpellInfoLua();
                                    int intOut;
                                    float floatOut;

                                    // name
                                    if (!string.IsNullOrWhiteSpace(slipped[0]) && slipped[0] != "nil")
                                        spellInfo.Name = slipped[0];
                                    // rank
                                    if (!string.IsNullOrWhiteSpace(slipped[1]))
                                        spellInfo.Rank = slipped[1];
                                    // icon
                                    if (!string.IsNullOrWhiteSpace(slipped[2]))
                                        spellInfo.Icon = slipped[2];
                                    // cost
                                    if (!string.IsNullOrWhiteSpace(slipped[3]) &&
                                        int.TryParse(slipped[3].Replace(".", ","), out intOut))
                                        spellInfo.Cost = intOut;
                                    // isFunnel
                                    if (!string.IsNullOrWhiteSpace(slipped[4]) && slipped[4].ToLower() == "true")
                                        spellInfo.IsFunnel = true;
                                    // powerType
                                    if (!string.IsNullOrWhiteSpace(slipped[5]) &&
                                        int.TryParse(slipped[5].Replace(".", ","), out intOut))
                                        spellInfo.PowerType = (PowerType) intOut;
                                    // castTime
                                    if (!string.IsNullOrWhiteSpace(slipped[6]) &&
                                        int.TryParse(slipped[6].Replace(".", ","), out intOut))
                                        spellInfo.CastTime = intOut;
                                    // minRange
                                    if (!string.IsNullOrWhiteSpace(slipped[7]) &&
                                        float.TryParse(slipped[7].Replace(".", ","), out floatOut))
                                        spellInfo.MinRange = floatOut;
                                    // maxRange
                                    if (!string.IsNullOrWhiteSpace(slipped[8]) &&
                                        float.TryParse(slipped[8].Replace(".", ","), out floatOut))
                                        spellInfo.MaxRange = floatOut;
                                    // ID
                                    if (!string.IsNullOrWhiteSpace(slipped[9]) &&
                                        int.TryParse(slipped[9].Replace(".", ","), out intOut))
                                        spellInfo.ID = (uint) intOut;

                                    if (listId.Contains(spellInfo.ID))
                                    {
                                        if (!_spellInfos.ContainsKey(spellInfo.ID))
                                            _spellInfos.Add(spellInfo.ID, spellInfo);
                                        ret.Add(spellInfo);
                                    }
                                }
                                else
                                {
                                    Logging.WriteDebug("Return as bad format: public static SpellInfo SpellInfoCreateCache()");
                                }
                            }
                            return ret;
                        }
                    }
                    else
                    {
                        Logging.WriteDebug("Cannot found spell: public static SpellInfo SpellInfoCreateCache()");
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("SpellInfo GetSpellInfo(uint id): " + exception);
            }
            return new List<SpellInfoLua>();
        }

        public class SpellInfoLua
        {
            public uint ID;
            public string Name = "";
            public string Rank = "";
            public string Icon = "";
            public int Cost;
            public bool IsFunnel;
            public Enums.PowerType PowerType;
            public int CastTime;
            public float MinRange;
            public float MaxRange;
        }

        private static readonly Dictionary<string, List<uint>> CacheSpellIdByName = new Dictionary<string, List<uint>>();


        public static string GetMountBarAndSlot()
        {
            try
            {
                List<string> mountList =
                    new List<string>(Others.ReadFileAllLines(Application.StartupPath + "\\Data\\mountList.txt"));

                string key = GetSlotBarBySpellName(mountList);
                if (key != "")
                    Logging.Write("Searching for mount: " + key);
                return key;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetMountBarAndSlot(): " + exception);
            }
            return "";
        }

        public static string GetMountName()
        {
            try
            {
                List<string> mountList =
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
                List<string> flyMountList =
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

        public static string GetFlyMountBarAndSlot()
        {
            try
            {
                List<string> flyMountList =
                    new List<string>(Others.ReadFileAllLines(Application.StartupPath + "\\Data\\flymountList.txt"));

                string key = GetSlotBarBySpellName(flyMountList);
                if (key != "")
                    Logging.Write("Searching for flying mount: " + key);
                return key;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetFlyMountBarAndSlot(): " + exception);
            }
            return "";
        }

        public static string GetAquaticMountName()
        {
            try
            {
                List<string> aquaticMountList =
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

        public static string GetAquaticMountBarAndSlot()
        {
            try
            {
                List<string> aquaticMountList =
                    new List<string>(Others.ReadFileAllLines(Application.StartupPath + "\\Data\\aquaticmountList.txt"));

                string key = GetSlotBarBySpellName(aquaticMountList);
                if (key != "")
                    Logging.Write("Searching for aquatic mount: " + key);
                return key;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetAquaticMountBarAndSlot(): " + exception);
            }
            return "";
        }

        public class SpellListManager
        {
            public static Dictionary<uint, string> ListSpell { get; private set; }
            public static List<string> ListSpellName { get; private set; }
            private static readonly object LoadSpellListeLock = new object();

            internal static void LoadSpellListe(string fileName)
            {
                try
                {
                    lock (LoadSpellListeLock)
                    {
                        if (ListSpell == null)
                        {
                            Dictionary<uint, string> tListSpell = new Dictionary<uint, string>();
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
                            ListSpellName = new List<string>();
                            foreach (KeyValuePair<uint, string> spell in ListSpell)
                            {
                                ListSpellName.Add(spell.Value);
                            }
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

            public static List<uint> SpellIdByName(string spellName)
            {
                lock ("SpellIdByName")
                {
                    List<uint> listIdSpellFound = new List<UInt32>();
                    try
                    {
                        spellName = spellName.ToLower();

                        if (CacheSpellIdByName.TryGetValue(spellName, out listIdSpellFound))
                            return listIdSpellFound;

                        listIdSpellFound = new List<uint>();
                        foreach (KeyValuePair<uint, string> spell in ListSpell)
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
                        foreach (KeyValuePair<uint, string> spell in ListSpell)
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

            public static string SpellNameByIdExperimental(UInt32 spellId)
            {
                try
                {
                    string randomStringResult = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(randomStringResult + ",_,_,_,_,_,_,_,_ = GetSpellInfo(" + spellId + ")");
                    string sResult = Lua.GetLocalizedText(randomStringResult);
                    Logging.WriteDebug("SpellNameByIdExperimental(UInt32 spellId): " + sResult + ";" +
                                       SpellNameById(spellId) + ";" + spellId);
                    return sResult;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("SpellNameByIdExperimental(UInt32 spellId): " + exception);
                }
                return "";
            }
        }
    }
}