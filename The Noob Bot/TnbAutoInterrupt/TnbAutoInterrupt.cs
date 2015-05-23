using System;
using System.Collections.Generic;
using System.IO;
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
        get { return MyPluginClass.InternalLoop; }
        set { MyPluginClass.InternalLoop = value; }
    }

    public string Name
    {
        get { return MyPluginClass.Name; }
    }

    public string Author
    {
        get { return MyPluginClass.Author; }
    }

    public string Description
    {
        get { return MyPluginClass.Description; }
    }

    public string TargetVersion
    {
        get { return MyPluginClass.TargetVersion; }
    }

    public string Version
    {
        get { return MyPluginClass.Version; }
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
        MyPluginClass.ShowConfiguration();
    }

    public void ResetConfiguration()
    {
        MyPluginClass.ResetConfiguration();
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
            MyPluginClass.Init();
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

public static class MyPluginClass
{
    public static bool InternalLoop = true;
    public static string Author = "Vesper";
    public static string Name = "AutoInterrupt";
    public static string TargetVersion = "4.6.x";
    public static string Version = "1.3.1";
    public static string Description = "Interrupt automatically when our target is casting or channeling a spell.";

    private static readonly List<Spell> AvailableInterruptersPVP = new List<Spell>();
    private static readonly List<Spell> AvailableInterruptersPve = new List<Spell>();
    private static readonly MyPluginSettings MySettings = MyPluginSettings.GetSettings();

    public static void Init()
    {
        GetAllAvailableInterrupters();
        if (AvailableInterruptersPVP.Count <= 0 && AvailableInterruptersPve.Count <= 0)
        {
            Logging.WriteDebug(Name + ": No spell capable of interrupt has been found.");
            return;
        }
        foreach (Spell spell in AvailableInterruptersPVP)
        {
            Logging.Write("Interrupter: " + spell.Name + " (" + spell.Id + ") has been found for PVP.");
        }
        foreach (Spell spell in AvailableInterruptersPve)
        {
            Logging.Write("Interrupter: " + spell.Name + " (" + spell.Id + ") has been found for PVE.");
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
        if (ObjectManager.Me.Target <= 0 || ObjectManager.Target.IsDead)
        {
            return;
        }
        if (ObjectManager.Target.Type == WoWObjectType.Player)
        {
            InterruptPVP();
        }
        if (ObjectManager.Target.Type == WoWObjectType.Unit)
        {
            InterruptPve();
        }
    }

    public static void InterruptPVP()
    {
        if (!ObjectManager.Target.IsHostile)
            return;
        if (ObjectManager.Target.CanInterruptCurrentCast && !IsSpellInIgnoreList())
        {
            var rnd = new Random();
            int sleepTime = rnd.Next(70, 250);
            Thread.Sleep(sleepTime); // Wait randomly between 70ms to 250ms before interrupt for account safety reason.
            while (ObjectManager.Target.CanInterruptCurrentCast && !IsSpellInIgnoreList())
            {
                foreach (Spell kicker in AvailableInterruptersPVP)
                {
                    if (ObjectManager.Target.GetDistance > kicker.MaxRangeHostile)
                    {
                        continue; // We are too far for this spell, try another one ASAP.
                    }
                    if (ObjectManager.Target.GetDistance < kicker.MinRangeFriend)
                    {
                        continue; // We are too close for this spell, try another one ASAP.
                    }
                    if (!kicker.IsSpellUsable)
                    {
                        continue; // This spell is on cooldown.
                    }
                    int spellId = ObjectManager.Target.CastingSpellId;
                    if (spellId > 0)
                    {
                        kicker.Launch();
                        Logging.Write("SpellId " + spellId + " from " + ObjectManager.Target.Name + " has been interrupted.");
                        Thread.Sleep(500);
                    }
                    break;
                }
            }
        }
    }

    public static void InterruptPve()
    {
        if (!ObjectManager.Target.IsHostile)
            return;
        if (!ObjectManager.Target.InCombat)
            return; // We don't wanna pull creatures.
        if (ObjectManager.Target.CanInterruptCurrentCast && !IsSpellInIgnoreList())
        {
            var rnd = new Random();
            int sleepTime = rnd.Next(70, 250);
            Thread.Sleep(sleepTime); // Wait randomly between 70ms to 250ms before interrupt for account safety reason.
            while (ObjectManager.Target.CanInterruptCurrentCast && !IsSpellInIgnoreList())
            {
                foreach (Spell kicker in AvailableInterruptersPve)
                {
                    if (ObjectManager.Target.GetDistance > kicker.MaxRangeHostile)
                    {
                        continue; // We are too far for this spell, try another one ASAP.
                    }
                    if (ObjectManager.Target.GetDistance < kicker.MinRangeFriend)
                    {
                        continue; // We are too close for this spell, try another one ASAP.
                    }
                    if (!kicker.IsSpellUsable)
                    {
                        continue; // This spell is on cooldown.
                    }
                    int spellId = ObjectManager.Target.CastingSpellId;
                    if (spellId > 0)
                    {
                        kicker.Launch();
                        Logging.Write("SpellId " + spellId + " from " + ObjectManager.Target.Name + " has been interrupted.");
                        Thread.Sleep(500);
                    }
                    break;
                }
            }
        }
    }

    public static bool IsSpellInIgnoreList()
    {
        string[] spellListPVP = MySettings.DontInterruptSpellList.Split(',');
        foreach (string sId in spellListPVP)
        {
            if (sId.Contains(ObjectManager.Target.CastingSpellId.ToString()))
            {
                Logging.Write("Target is casting spellId " + ObjectManager.Target.CastingSpellId + " but it's ignored.");
                return true;
            }
        }
        return false;
    }

    public static void GetAllAvailableInterrupters()
    {
        string[] spellListPVP = MySettings.SpellListPVP.Split(',');
        AvailableInterruptersPve.Clear();
        AvailableInterruptersPVP.Clear();
        foreach (string sId in spellListPVP)
        {
            uint id = Others.ToUInt32(sId.Trim());
            var spell = new Spell(id);
            if (spell.Name != "" && spell.KnownSpell)
            {
                if (MySettings.ActivateInterruptPVP)
                    AvailableInterruptersPVP.Add(spell);
                if (MySettings.ActivateInterruptPve)
                    AvailableInterruptersPve.Add(spell);
            }
        }
        if (!MySettings.ActivateInterruptPve)
            return;
        string[] spellListPve = MySettings.SpellListPve.Split(',');
        foreach (string sId in spellListPve)
        {
            uint id = Others.ToUInt32(sId.Trim());
            var spell = new Spell(id);
            if (spell.Name != "" && spell.KnownSpell)
            {
                AvailableInterruptersPve.Add(spell);
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
        public bool ActivateInterruptPVP = true;
        public bool ActivateInterruptPve = true;
        public string DontInterruptSpellList = "";
        public string SpellListPVP = "106839, 78675, 147362, 34490, 2139, 116705, 31935, 96231, 1766, 57994, 19647, 115782, 6552, 47528";
        public string SpellListPve = "15487, 47476";

        public MyPluginSettings()
        {
            ConfigWinForm(Name + " Spells Management");
            AddControlInWinForm("List of spells to not interrupt (ignore) :", "DontInterruptSpellList", Name + " Spells Management", "List");
            AddControlInWinForm("Auto Interrupt in PVP", "ActivateInterruptPVP", Name + " Spells Management");
            AddControlInWinForm("List of spells that can interrupt in PVP :", "SpellListPVP", Name + " Spells Management", "List");
            AddControlInWinForm("Auto Interrupt in PVE", "ActivateInterruptPve", Name + " Spells Management");
            AddControlInWinForm("List of spells that can only interrupt in PVE (will complete the PVP list in PVE) :", "SpellListPve", Name + " Spells Management", "List");
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