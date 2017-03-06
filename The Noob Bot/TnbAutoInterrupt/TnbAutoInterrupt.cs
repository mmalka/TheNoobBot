using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Plugins;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

#region Interface Implementation - Edition Expert only

public class Main : IPlugins
{
    private bool _checkFieldRunning;

    public bool Loop
    {
        get { return TnbAutoInterrupt.InternalLoop; }
        set { TnbAutoInterrupt.InternalLoop = value; }
    }

    public string Name
    {
        get { return TnbAutoInterrupt.Name; }
    }

    public string Author
    {
        get { return TnbAutoInterrupt.Author; }
    }

    public string Description
    {
        get { return TnbAutoInterrupt.Description; }
    }

    public string TargetVersion
    {
        get { return TnbAutoInterrupt.TargetVersion; }
    }

    public string Version
    {
        get { return TnbAutoInterrupt.Version; }
    }

    public bool IsStarted
    {
        get { return Loop && !_checkFieldRunning; }
    }

    public void Dispose()
    {
        Loop = false;
    }

    public void Initialize()
    {
        Logging.WriteDebug(string.Format("The plugin {0} is loading.", Name));
        Initialize(false);
    }

    public void ShowConfiguration()
    {
        TnbAutoInterrupt.ShowConfiguration();
    }

    public void ResetConfiguration()
    {
        TnbAutoInterrupt.ResetConfiguration();
    }

    public void CheckFields() // do not edit.
    {
        _checkFieldRunning = true;
        Loop = true;
        while (Loop)
        {
            Thread.Sleep(1000); // Don't do any action.
        }
    }

    public void Initialize(bool configOnly, bool resetSettings = false)
    {
        try
        {
            if (!configOnly && !resetSettings)
                Loop = true;
            TnbAutoInterrupt.Init();
        }
        catch (Exception e)
        {
            Logging.WriteError("IPlugins.Main.Initialize(bool configOnly, bool resetSettings = false): " + e);
        }
        Logging.Write(string.Format("The plugin {0} has stopped.", Name));
    }
}

#endregion

#region Plugin core - Your plugin should be coded here

public static class TnbAutoInterrupt
{
    public static bool InternalLoop = true;
    public static string Author = "Vesper, Ryuichiro";
    public static string Name = "AutoInterrupt";
    public static string TargetVersion = "6.5.x";
    public static string Version = "1.4.0";
    public static string Description = "Interrupt automatically when our target is casting or channeling a spell.";

    private static readonly List<int> NotInterruptedSpells = new List<int>();
    private static readonly List<Spell> AvailableInterruptersPVP = new List<Spell>();
    private static readonly List<Spell> AvailableInterruptersPVE = new List<Spell>();
    private static readonly MyPluginSettings MySettings = MyPluginSettings.GetSettings();

    public static void Init()
    {
        GetAllAvailableInterrupters();
        if (AvailableInterruptersPVP.Count <= 0 && AvailableInterruptersPVE.Count <= 0)
        {
            Logging.WritePluginDebug(Name + ": No spell capable of interrupt has been found.", Name);
            return;
        }
        foreach (Spell spell in AvailableInterruptersPVP)
        {
            Logging.WritePlugin("Interrupter: " + spell.Name + " (" + spell.Id + ") has been found for PVP.", Name);
        }
        foreach (Spell spell in AvailableInterruptersPVE)
        {
            Logging.WritePlugin("Interrupter: " + spell.Name + " (" + spell.Id + ") has been found for PVE.", Name);
        }

        GetSpellInIgnoreList();
        if (NotInterruptedSpells.Count > 0)
        {
            Logging.WritePlugin("List of Spell Ids that shouldn't be interrupted:", Name);
            foreach (int sId in NotInterruptedSpells)
            {
                Logging.WritePlugin(sId.ToString(), Name);
            }
        }

        MainLoop();
    }

    public static void MainLoop()
    {
        while (InternalLoop)
        {
            CheckToInterrupt();
            Thread.Sleep(200);
        }
    }

    public static void CheckToInterrupt()
    {
        if (ObjectManager.Me.Target <= 0 || ObjectManager.Target.IsDead || !ObjectManager.Target.IsHostile)
        {
            return;
        }
        if (ObjectManager.Target.Type == WoWObjectType.Player)
        {
            InterruptPVP();
        }
        if (ObjectManager.Target.Type == WoWObjectType.Unit)
        {
            InterruptPVE();
        }
    }

    public static void InterruptPVP()
    {
        bool IsChanneled = ObjectManager.Target.CurrentSpellIdChannel > 0;
        var rnd = new Random();
        int rndTime = rnd.Next(70, 250);

        while (ObjectManager.Target.CanInterruptCurrentCast)
        {
            Thread.Sleep(rndTime); // Wait randomly between 70ms to 250ms before interrupt for account safety reason.

            if (IsChanneled || ObjectManager.Target.CastEndsInMs < (rndTime + MySettings.LeadTime + Usefuls.Latency))
            {
                foreach (Spell kicker in AvailableInterruptersPVP)
                {
                    if (!kicker.IsSpellUsable || !kicker.IsHostileDistanceGood)
                        continue;

                    int spellId = ObjectManager.Target.CastingSpellId;
                    if (spellId > 0)
                    {
                        kicker.Cast();
                        Logging.WritePlugin("SpellId " + spellId + " from " + ObjectManager.Target.Name + " has been interrupted.", Name);
                        Thread.Sleep(500);
                    }
                    break;
                }
            }
        }
    }

    public static void InterruptPVE()
    {
        bool IsChanneled = ObjectManager.Target.CurrentSpellIdChannel > 0;
        var rnd = new Random();
        int rndTime = rnd.Next(70, 250);

        while (ObjectManager.Target.CanInterruptCurrentCast)
        {
            Thread.Sleep(rndTime); // Wait randomly between 70ms to 250ms before interrupt for account safety reason.

            if (IsChanneled || ObjectManager.Target.CastEndsInMs < (rndTime + MySettings.LeadTime + Usefuls.Latency))
            {
                foreach (Spell kicker in AvailableInterruptersPVE)
                {
                    if (!kicker.IsSpellUsable || !kicker.IsHostileDistanceGood)
                        continue;

                    int spellId = ObjectManager.Target.CastingSpellId;
                    if (spellId > 0)
                    {
                        kicker.Cast();
                        Logging.WritePlugin("SpellId " + spellId + " from " + ObjectManager.Target.Name + " has been interrupted.", Name);
                        Thread.Sleep(500);
                    }
                    break;
                }
            }
        }
    }

    public static bool IsSpellInIgnoreList()
    {
        if (NotInterruptedSpells.Contains(ObjectManager.Target.CastingSpellId))
        {
            Logging.WritePlugin("Target is casting spellId " + ObjectManager.Target.CastingSpellId + " but it's ignored.", Name);
            return true;
        }
        return false;
    }

    public static void GetSpellInIgnoreList()
    {
        NotInterruptedSpells.Clear();
        string[] spellListIgnore = MySettings.DontInterruptSpellList.Split(',');
        foreach (string sId in spellListIgnore)
        {
            int id = Others.ToInt32(sId.Trim());
            if (id != 0)
                NotInterruptedSpells.Add(id);
        }
    }

    public static void GetAllAvailableInterrupters()
    {
        AvailableInterruptersPVP.Clear();
        AvailableInterruptersPVE.Clear();
        string[] spellListPVP = MySettings.SpellListPVP.Split(',');
        foreach (string sId in spellListPVP)
        {
            uint id = Others.ToUInt32(sId.Trim());
            var spell = new Spell(id);
            if (spell.Name != "" && spell.KnownSpell)
            {
                if (MySettings.ActivateInterruptPVP)
                    AvailableInterruptersPVP.Add(spell);
                if (MySettings.ActivateInterruptPVE)
                    AvailableInterruptersPVE.Add(spell);
            }
        }
        if (!MySettings.ActivateInterruptPVE)
            return;
        string[] spellListPve = MySettings.SpellListPve.Split(',');
        foreach (string sId in spellListPve)
        {
            uint id = Others.ToUInt32(sId.Trim());
            var spell = new Spell(id);
            if (spell.Name != "" && spell.KnownSpell)
            {
                AvailableInterruptersPVE.Add(spell);
            }
        }
    }

    public static void ResetConfiguration()
    {
        string currentSettingsFile = Application.StartupPath + "\\Plugins\\Settings\\" + Name + ".xml";
        var currentSetting = new MyPluginSettings();
        currentSetting.ToForm();
        currentSetting.Save(currentSettingsFile);
    }

    public static void ShowConfiguration()
    {
        string currentSettingsFile = Application.StartupPath + "\\Plugins\\Settings\\" + Name + ".xml";
        var currentSetting = new MyPluginSettings();
        if (File.Exists(currentSettingsFile))
        {
            currentSetting = Settings.Load<MyPluginSettings>(currentSettingsFile);
        }
        currentSetting.ToForm();
        currentSetting.Save(currentSettingsFile);
    }

    [Serializable]
    public class MyPluginSettings : Settings
    {
        public uint LeadTime = 100;
        public bool ActivateInterruptPVP = true;
        public bool ActivateInterruptPVE = true;
        public string DontInterruptSpellList = "";
        public string SpellListPVP = "106839, 78675, 147362, 34490, 2139, 116705, 31935, 96231, 1766, 57994, 19647, 115782, 6552, 47528";
        public string SpellListPve = "15487, 47476";

        public MyPluginSettings()
        {
            ConfigWinForm(Name + " Spells Management");
            AddControlInWinForm("List of spells to not interrupt (ignore) :", "DontInterruptSpellList", Name + " Spells Management", "List");
            AddControlInWinForm("Auto Interrupt in PVP", "ActivateInterruptPVP", Name + " Spells Management");
            AddControlInWinForm("List of spells that can interrupt in PVP :", "SpellListPVP", Name + " Spells Management", "List");
            AddControlInWinForm("Auto Interrupt in PVE", "ActivateInterruptPVE", Name + " Spells Management");
            AddControlInWinForm("List of spells that can only interrupt in PVE (will complete the PVP list in PVE) :", "SpellListPve", Name + " Spells Management", "List");
            ConfigWinForm("Internal Values");
            AddControlInWinForm("Time before cast finishes at which it should be interrupted.\r\n(too low times will fail)", "LeadTime", "Internal Values");
        }

        public static MyPluginSettings CurrentSetting { get; set; }

        public static MyPluginSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\Plugins\\Settings\\" + Name + ".xml";
            if (File.Exists(currentSettingsFile))
            {
                return CurrentSetting = Load<MyPluginSettings>(currentSettingsFile);
            }
            return new MyPluginSettings();
        }
    }
}

#endregion