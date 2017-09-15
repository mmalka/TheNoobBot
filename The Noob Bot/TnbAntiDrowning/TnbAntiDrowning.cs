using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Plugins;
using nManager.Products;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using Timer = nManager.Helpful.Timer;
using Point = nManager.Wow.Class.Point;
using ObjectManager = nManager.Wow.ObjectManager.ObjectManager;

#region Interface Implementation - Edition Expert only

public class Main : State, IPlugins
{
    private bool _checkFieldRunning;

    public bool Loop
    {
        get { return MyStatePlugin.InternalLoop; }
        set { MyStatePlugin.InternalLoop = value; }
    }

    public string Name
    {
        get { return MyStatePlugin.Name; }
    }

    public string Author
    {
        get { return MyStatePlugin.Author; }
    }

    public string Description
    {
        get { return MyStatePlugin.Description; }
    }

    public string TargetVersion
    {
        get { return MyStatePlugin.TargetVersion; }
    }

    public string Version
    {
        get { return MyStatePlugin.Version; }
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
        MyStatePlugin.ShowConfiguration();
    }

    public void ResetConfiguration()
    {
        MyStatePlugin.ResetConfiguration();
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
            MyStatePlugin.Init();
        }
        catch (Exception e)
        {
            Logging.WriteError("IPlugins.Main.Initialize(bool configOnly, bool resetSettings = false): " + e);
        }
        Logging.Write(string.Format("The plugin {0} has stopped.", Name));
    }

    public override int Priority
    {
        get { return MyStatePlugin.StatePriority; }
        set { MyStatePlugin.StatePriority = value; }
    }

    public override string DisplayName
    {
        get { return MyStatePlugin.Name; }
    }

    public override bool NeedToRun
    {
        get { return MyStatePlugin.NeedToRun(); }
    }

    public override List<State> NextStates
    {
        get { return new List<State>(); }
    }

    public override List<State> BeforeStates
    {
        get { return new List<State>(); }
    }

    public override void Run()
    {
        MyStatePlugin.Run();
    }
}

#endregion

#region Plugin core - Your plugin should be coded here

public static class MyStatePlugin
{
    /*
     * Finite-state machine (FSM) requires a NeedToRun and a Run method.
     * The NeedToRun() code will be checked by the main product currently running
     * according to its priority.
     * For example, "Pause" State have a priority of 200 while "Idle" State (Do nothing) have a priority of 0.
     * Usually, main product activities have a priority of 20-30 while more important thing like resurrect have a priority of 100+.
     * You can ask the list of priority for a defined product to developers on our Discord server.
     * Note that if, say, you wanted  to make a plugin to leave a dungeon and do a ResetInstance(), you'd probably need
     * to set a priority between 31 and 39 as Grinder main module have a priority of 30 and higher priority start at 40 with an increment of 10.
     * Two plugins can have the same priority, but only the one loaded  first will be above the other so if you need your plugin to work well with
     * another plugin you know. Make sure that the one that should get higher priority is not using the same number. (31 vs 39 instead of both having 39)
     * 
     * Grinder Example
     * Pause 200 > SelectProfileState 150 > Resurrect 140 > IsAttacked 130 > Regeneration 120 > ToTown 110 > Looting 100 > Travel 90 > SpecializationCheck 80
     * LevelupCheck 70 > Trainers 60 > MillingState 50 > ProspectingState 40 > Grinding 30 > Farming 20 > MovementLoop 10 > Idle 0
     * The bot wont accept your plugin if the priority is above 199 or below 1.
     * 
     * Remember that you must not stuck the bot into the Run() code. You must make sure that your code can be called severals time in a row while performing
     * the action just fine. (If you're going somewhere, don't make a "while(moving...)", instead make sure you always go back to your code with a valid "NeedToRun"
     * and that if you already started a movement, then you just keep returning the code.
     */
    public static bool InternalLoop = true;
    public static string Author = "Vesper";
    public static string Name = "AntiDrowning";
    public static string TargetVersion = "7.1.x";
    public static string Version = "1.0.0";
    public static int StatePriority = 31; // Have more priority than grinding itself, but will still resurrect/fight before doing that code.
    public static string Description = "Will attempt to go back to the surface in case of drowning.";
    private static readonly MyPluginSettings MySettings = MyPluginSettings.GetSettings();
    private static Spell DruidMountSpell = new Spell("Travel Form");

    public static bool NeedToRun()
    {
        if (Usefuls.BadBottingConditions || Usefuls.ShouldFight)
            return false;

        if (Usefuls.IsSwimming && ObjectManager.Me.BreathPercentage < 20)
            return true;
        return false;
        /*
        if (!Usefuls.IsSwimming || ObjectManager.Me.BreathPercentage >= 20) 
            return false;
        if (ObjectManager.Me.WowClass == WoWClass.Druid && DruidMountSpell.IsSpellUsable)
            return true;
        for (int i = 5; i <= 80; i+=5)
        {
            i = 15;
            Point posAbove = ObjectManager.Me.Position;
            posAbove.Z += i;
            if (TraceLine.TraceLineGo(ObjectManager.Me.Position, posAbove))
            {
                bool hitAllButLiquid = TraceLine.TraceLineGo(ObjectManager.Me.Position, posAbove, CGWorldFrameHitFlags.HitTestAllButLiquid);
                bool hitOnlyLiquid = TraceLine.TraceLineGo(ObjectManager.Me.Position, posAbove, CGWorldFrameHitFlags.HitTestLiquid);
                if (hitAllButLiquid && !hitOnlyLiquid)
                {
                    // We hit something that is not water line first. We cannot go out of water from this position.
                    return false;
                }
                if (hitOnlyLiquid)
                    return true;
            }
            Thread.Sleep(100);
        }
        return true;
        */
    }

    public static void Run()
    {
        if (ObjectManager.Me.BreathPercentage >= 95)
            return;
        if (ObjectManager.Me.WowClass == WoWClass.Druid && DruidMountSpell.IsSpellUsable)
        {
            DruidMountSpell.Cast();
            var timerRegen = new Timer(5000);
            while (ObjectManager.Me.BreathPercentage < 95 && !timerRegen.IsReady)
            {
                Thread.Sleep(50);
            }
        }
        else
        {
            Logging.WritePlugin("Trying to reach the surface...", Name);
            MovementManager.StopMove();
            MovementsAction.Ascend(true);
            Timer timerSurface = new Timer(40000);
            while (ObjectManager.Me.BreathPercentage < 95 && !timerSurface.IsReady)
            {
                Thread.Sleep(50);
            }
            Thread.Sleep(150);
            MovementsAction.Ascend(false);
        }
    }

    public static void Init()
    {
        /*
         * You can use this area to do some logging or stuff.
         */
        Logging.WritePlugin("Loaded TnbAntiDrowning with BreathPercentage tolerance at " + MySettings.BreathPercentage + ".", Name);
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
        public uint BreathPercentage = 100;

        public MyPluginSettings()
        {
            ConfigWinForm("Anti Drowning Settings");
            AddControlInWinForm("Going to surface when", "BreathPercentage", "Example Title", "BelowPercentage");
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