using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using Timer = nManager.Helpful.Timer;
using KeybindingsEnum = nManager.Wow.Enums.Keybindings;
using KeybindingsHelper = nManager.Wow.Helpers.Keybindings;

#region Main routine

public class Main : ICombatClass
{
    internal static float InternalRange = 5.0f;
    internal static float InternalAggroRange = 5.0f;
    internal static bool InternalLoop = true;
    internal static Spell InternalLightHealingSpell;
    internal static float Version = 1.0f;

    #region ICombatClass Members

    public float AggroRange
    {
        get { return InternalAggroRange; }
    }

    public Spell LightHealingSpell
    {
        get { return InternalLightHealingSpell; }
        set { InternalLightHealingSpell = value; }
    }

    public float Range
    {
        get { return InternalRange; }
        set { InternalRange = value; }
    }

    public void Initialize()
    {
        Initialize(false);
    }

    public void Dispose()
    {
        Logging.WriteFight("Combat system stopped.");
        InternalLoop = false;
    }

    public void ShowConfiguration()
    {
        Directory.CreateDirectory(Application.StartupPath + "\\CombatClasses\\Settings\\");
        Initialize(true);
    }

    public void ResetConfiguration()
    {
        Directory.CreateDirectory(Application.StartupPath + "\\CombatClasses\\Settings\\");
        Initialize(true, true);
    }

    #endregion

    public void Initialize(bool configOnly, bool resetSettings = false)
    {
        try
        {
            InternalLoop = true;
            if (configOnly)
                return;

            if (!ObjectManager.Me.InTransport)
            {
                Dispose();
                return;
            }
            
            WoWObject vehicle = ObjectManager.GetObjectByGuid(ObjectManager.Me.TransportGuid);
            switch (vehicle.Entry)
            {
                #region Vehicule Selection

                case 89089:
                    Logging.WriteFight("Loading Prince Farondis Combat class...");
                    new PrinceFarondis89089(vehicle.GetBaseAddress);
                    break;
                default:
                    Dispose();
                    break;

                #endregion
            }
        }
        catch
        {
        }
        Logging.WriteFight("Combat system stopped.");
    }
}

#endregion

#region Abstract Vehicle class

public abstract class Vehicle
{
    #region General Variables

    private bool CombatMode = true;

    #endregion

    public Vehicle(uint vehicleBaseAddress)
    {
        Initialize(vehicleBaseAddress);
        UInt128 target = 0;
        while (Main.InternalLoop)
        {
            try
            {
                target = 0;
                if ((Fight.InFight || ObjectManager.Me.InCombat))
                {
                    target = ObjectManager.Me.Target;
                    if (target == 0)
                    {
                        WoWUnit unit = ObjectManager.GetNearestWoWUnit(ObjectManager.GetHostileUnitAttackingPlayer(), true, true, true);
                        if (!unit.IsValid)
                            unit = ObjectManager.GetNearestWoWUnit((List<WoWUnit>)ObjectManager.GetHostileUnitNearPlayer().Where(u => u.InCombat && u.Target == vehicleBaseAddress), true, true, true);

                        if (unit.IsValid)
                        {
                            target = unit.Guid;
                            ObjectManager.Me.Target = target;
                            Interact.InteractWith(unit.GetBaseAddress, true);
                        }
                    }
                }
                if (target > 0 && CanStartCombat)
                    Combat();
                else
                    Patrolling();
            }
            catch (Exception e)
            {
                Logging.WriteDebug(e.Message);
            }
            Thread.Sleep(100);
        }
    }

    abstract public void Initialize(uint vehicleBaseAddress);

    abstract public void GCDCycle();

    abstract public bool CanStartCombat { get; }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsCast)
        {
            //Log
            if (CombatMode)
            {
                Logging.WriteFight("Patrolling:");
                CombatMode = false;
            }
        }
    }

    private void Combat()
    {
        //Log 
        if (!CombatMode)
        {
            Logging.WriteFight("Combat:");
            CombatMode = true;
        }

        Usefuls.SleepGlobalCooldown();

        try
        {
            Memory.WowMemory.GameFrameLock(); // !!! WARNING - DONT SLEEP WHILE LOCKED - DO FINALLY(GameFrameUnLock()) !!!

            GCDCycle();
        }
        catch (Exception e)
        {
            Logging.WriteDebug(e.Message);
        }
        finally
        {
            Memory.WowMemory.GameFrameUnLock();
        }
    }
}

#endregion

#region Prince Farondis (89089)

public class PrinceFarondis89089 : Vehicle
{
    #region General Variables

    private WoWUnit CurrentVehicle;

    private int NextFacingPoint = 0;
    private Timer FacingTimer = new Timer(0);

    #endregion

    #region Offensive Actions

    private readonly Spell Fireball = new Spell(178784);
    private readonly Spell MeteorStorm = new Spell(179215);
    private Timer FireBallCastTimer = new Timer(0);
    private Timer MeteorStormTimer = new Timer(0);

    #endregion

    public PrinceFarondis89089(uint vehicleBaseAddress) : base(vehicleBaseAddress)
    {

    }

    public override void Initialize(uint vehicleBaseAddress)
    {
        CurrentVehicle = new WoWUnit(vehicleBaseAddress);
        Main.InternalRange = 38f;
        Main.InternalAggroRange = 38f;
    }

    public override void GCDCycle()
    {

        if (ObjectManager.Target.IsValid && !ObjectManager.Target.IsDead && !ObjectManager.Me.HaveBuff(212096)) {
            if (FireBallCastTimer.IsReady && MeteorStormTimer.IsReady && MeteorStorm.IsHostileDistanceGood)
            {
                KeybindingsHelper.PressKeybindings(KeybindingsEnum.ACTIONBUTTON2);
                MeteorStormTimer = new Timer(1000 * 10 + 100);
                return;
            }

            if (FireBallCastTimer.IsReady && Fireball.IsHostileDistanceGood)
            {
                KeybindingsHelper.PressKeybindings(KeybindingsEnum.ACTIONBUTTON1);
                FireBallCastTimer = new Timer(1000);
                FacingTimer = new Timer(500);
                return;
            }

            // Try to face the mob
            // TODO: Improve this
            if (FacingTimer.IsReady && !FireBallCastTimer.IsReady && !ObjectManager.Me.IsCast && !CurrentVehicle.IsCast)
            {
                List<Point> facingPoints = new List<Point>
                {
                    new Point(ObjectManager.Me.Position.X + 1.2f, ObjectManager.Me.Position.Y, ObjectManager.Me.Position.Z),
                    new Point(ObjectManager.Me.Position.X + 0.9f, ObjectManager.Me.Position.Y + 0.3f, ObjectManager.Me.Position.Z),
                    new Point(ObjectManager.Me.Position.X + 0.9f, ObjectManager.Me.Position.Y + 0.3f, ObjectManager.Me.Position.Z),
                    new Point(ObjectManager.Me.Position.X + 0.6f, ObjectManager.Me.Position.Y + 0.6f, ObjectManager.Me.Position.Z),
                    new Point(ObjectManager.Me.Position.X + 0.3f, ObjectManager.Me.Position.Y + 0.9f, ObjectManager.Me.Position.Z),
                    new Point(ObjectManager.Me.Position.X, ObjectManager.Me.Position.Y + 1.2f, ObjectManager.Me.Position.Z),
                    new Point(ObjectManager.Me.Position.X - 1.2f, ObjectManager.Me.Position.Y, ObjectManager.Me.Position.Z),
                    new Point(ObjectManager.Me.Position.X - 0.9f, ObjectManager.Me.Position.Y - 0.3f, ObjectManager.Me.Position.Z),
                    new Point(ObjectManager.Me.Position.X - 0.9f, ObjectManager.Me.Position.Y - 0.3f, ObjectManager.Me.Position.Z),
                    new Point(ObjectManager.Me.Position.X - 0.6f, ObjectManager.Me.Position.Y - 0.6f, ObjectManager.Me.Position.Z),
                    new Point(ObjectManager.Me.Position.X - 0.3f, ObjectManager.Me.Position.Y - 0.9f, ObjectManager.Me.Position.Z),
                    new Point(ObjectManager.Me.Position.X, ObjectManager.Me.Position.Y - 1.2f, ObjectManager.Me.Position.Z)
                };
                MovementManager.Go(new List<Point> { ObjectManager.Me.Position, facingPoints[NextFacingPoint] });
                FacingTimer = new Timer(1000);
                FireBallCastTimer = new Timer(0);
                NextFacingPoint = ++NextFacingPoint >= facingPoints.Count ? 0 : NextFacingPoint;
                return;
            }
        }
    }

    public override bool CanStartCombat
    {
        get
        {
            return Fireball.IsHostileDistanceGood;
        }
    }

}

#endregion
