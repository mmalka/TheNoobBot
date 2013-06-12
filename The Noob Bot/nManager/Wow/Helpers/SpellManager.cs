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
                FutureSpell = 2, // the spell is known but not yet learnt
                PetAction = 3,
                Flyout = 4
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
                if (ObjectManager.ObjectManager.Me.WowClass != WoWClass.Druid)
                    return null;
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
                var spellList = new List<string> {spell};
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
                            return SpellListManager.SpellNameByIdExperimental(sIdt);
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
                var keySlot = barAndSlot.Split(Convert.ToChar(";"));


                if (Convert.ToUInt32(keySlot[0]) == 1)
                {
                    int numBarOne = Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule +
                                                                    (uint) Addresses.BarManager.startBar);
                    if (numBarOne > 0)
                        keySlot[0] = (6 + (numBarOne)).ToString(CultureInfo.InvariantCulture);
                }

                uint adresse = Memory.WowProcess.WowModule + (uint) Addresses.BarManager.slotIsEnable +
                               (4*12*(Convert.ToUInt32(keySlot[0]) - 1)) + (4*(Convert.ToUInt32(keySlot[1]) - 1));

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
                var ret =
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
                    Logging.Write("Initializing SpellBook - (Wait few seconds)");
                    _usedSbid = true;
                    var spellBook = new List<uint>();

                    UInt32 nbSpells =
                        Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule +
                                                         (uint) Addresses.SpellBook.nbSpell);
                    UInt32 spellBookInfoPtr =
                        Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule +
                                                         (uint) Addresses.SpellBook.knownSpell);

                    for (UInt32 i = 0; i < nbSpells; i++)
                    {
                        var Struct = Memory.WowMemory.Memory.ReadUInt(spellBookInfoPtr + i*4);
                        var si = (SpellInfo) Memory.WowMemory.Memory.ReadObject(Struct, typeof (SpellInfo));
                        if (si.State == SpellInfo.SpellState.Known)
                            spellBook.Add(si.ID);
                        Application.DoEvents();
                    }

                    _spellBookID = spellBook;
                    _usedSbid = false;
                    Logging.Write("Initialize SpellBook Finished (" + _spellBookID.Count + " spell found)");
                }
                return _spellBookID;
            }
            catch (Exception exception)
            {
                Logging.WriteError("SpellBookID(): " + exception);
            }
            return new List<uint>();
        }

        public static int SpellAvailable()
        {
            try
            {
                UInt32 nbSpells =
                    Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint) Addresses.SpellBook.nbSpell);
                var spellBookInfoPtr =
                    Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint) Addresses.SpellBook.knownSpell);
                var j = 0;
                for (UInt32 i = 0; i < nbSpells; i++)
                {
                    var Struct = Memory.WowMemory.Memory.ReadUInt(spellBookInfoPtr + i*4);
                    var si = (SpellInfo) Memory.WowMemory.Memory.ReadObject(Struct, typeof (SpellInfo));
                    if (si.State == SpellInfo.SpellState.Known)
                        j++;
                }
                return j;
            }
            catch (Exception exception)
            {
                Logging.WriteError("SpellAvailable(): " + exception);
            }
            return 0;
        }

        private static List<string> _spellBookName = new List<string>();
        private static bool _usedSbn;

        public static List<string> SpellBookName()
        {
            try
            {
                while (_usedSbn)
                {
                    Thread.Sleep(10);
                }
                if (_spellBookName.Count <= 0)
                {
                    _usedSbn = true;
                    var spellBook = SpellBookID().Select(SpellListManager.SpellNameByIdExperimental).ToList();
                    _spellBookName = spellBook;
                    _usedSbn = false;
                }
                return _spellBookName;
            }
            catch (Exception exception)
            {
                Logging.WriteError("SpellBookName(): " + exception);
            }
            return new List<string>();
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
                        var spellBook = SpellBookID().Select(SpellInfoLUA).ToList();
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

        public static void UpdateSpellBook()
        {
            try
            {
                var nbSpells =
                    Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint) Addresses.SpellBook.nbSpell);
                var spellBookInfoPtr =
                    Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + (uint) Addresses.SpellBook.knownSpell);

                for (UInt32 i = 0; i < nbSpells; i++)
                {
                    var Struct = Memory.WowMemory.Memory.ReadUInt(spellBookInfoPtr + i*4);
                    var si = (SpellInfo) Memory.WowMemory.Memory.ReadObject(Struct, typeof (SpellInfo));
                    if (si.State == SpellInfo.SpellState.Known)
                    {
                        if (!_spellBookID.Contains(si.ID))
                        {
                            _spellBookID.Add(si.ID);
                            _spellBookName.Add(SpellListManager.SpellNameByIdExperimental(si.ID));
                            _spellBookSpell.Add(SpellInfoLUA(si.ID));
                        }
                    }
                    Application.DoEvents();
                }


                foreach (var o in _spellBookSpell)
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


        public static string GetMountBarAndSlot()
        {
            try
            {
                var mountList =
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

        public static string GetFlyMountBarAndSlot()
        {
            try
            {
                var flyMountList =
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

        public static string GetAquaticMountBarAndSlot()
        {
            try
            {
                var aquaticMountList =
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
            public static List<SpellList> ListSpell { get; private set; }
            public static List<string> ListSpellName { get; private set; }
            private static string _spellName = "";

            internal static void LoadSpellListe(string fileName)
            {
                try
                {
                    if (ListSpell == null)
                    {
                        string[] listSpellTemps = Others.ReadFileAllLines(fileName);
                        ListSpell = new List<SpellList>();
                        ListSpellName = new List<string>();
                        foreach (string tempsSpell in listSpellTemps)
                        {
                            string[] tmpSpell = tempsSpell.Split(';');
                            ListSpell.Add(new SpellList(Convert.ToUInt32(tmpSpell[0]), tmpSpell[1]));
                            ListSpellName.Add(tmpSpell[1]);
                        }
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("LoadSpellListe(string fileName): " + exception);
                }
            }

            public static List<uint> SpellIdByName(string spellName)
            {
                var listIdSpellFound = new List<UInt32>();
                try
                {
                    _spellName = spellName;
                    var listSpellFound = ListSpell.FindAll(FindByName);
                    listIdSpellFound.AddRange(listSpellFound.Select(tempsSpell => tempsSpell.Id));

                    return listIdSpellFound;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("SpellIdByName(string spellName): " + exception);
                    return listIdSpellFound;
                }
            }

            private static bool FindByName(SpellList tempsSpell)
            {
                try
                {
                    if (tempsSpell.Name.ToLower() == _spellName.ToLower())
                    {
                        return true;
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("FindByName(SpellList tempsSpell): " + exception);
                }
                return false;
            }

            public static string SpellNameById(UInt32 spellId)
            {
                try
                {
                    _spellId = spellId;
                    List<SpellList> listSpellFound = ListSpell.FindAll(FindById);
                    if (listSpellFound.Count > 0)
                        return listSpellFound[0].Name;
                    return "";
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
                    _spellId = spellId;
                    string randomStringResult = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(randomStringResult + " = GetSpellInfo(" + spellId + ")");
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

            private static uint _spellId;

            private static bool FindById(SpellList tempsSpell)
            {
                try
                {
                    if (tempsSpell.Id == _spellId)
                    {
                        return true;
                    }
                    {
                        return false;
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("FindById(SpellList tempsSpell): " + exception);
                }
                return false;
            }
        }
    }
}