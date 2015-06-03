/*
* CombatClass for TheNoobBot
* Credit : Rival, Geesus, Enelya, Marstor, Vesper, Neo2003, Dreadlocks
* Thanks you !
*/
// ReSharper disable EmptyGeneralCatchClause
// ReSharper disable ObjectCreationAsStatement

using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using Timer = nManager.Helpful.Timer;

public class Main : IHealerClass
{
    internal static float InternalRange = 30f;
    internal static bool InternalLoop = true;

    #region IHealerClass Members

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
        Logging.WriteFight("Healing system stopped.");
        InternalLoop = false;
    }

    public void ShowConfiguration()
    {
        Directory.CreateDirectory(Application.StartupPath + "\\HealerClasses\\Settings\\");
        Initialize(true);
    }

    public void ResetConfiguration()
    {
        Directory.CreateDirectory(Application.StartupPath + "\\HealerClasses\\Settings\\");
        Initialize(true, true);
    }

    #endregion

    public void Initialize(bool configOnly, bool resetSettings = false)
    {
        try
        {
            if (!InternalLoop)
                InternalLoop = true;
            Logging.WriteFight("Loading healing system.");
            WoWSpecialization wowSpecialization = ObjectManager.Me.WowSpecialization(true);
            switch (ObjectManager.Me.WowClass)
            {
                #region Non Druid Class check

                case WoWClass.DeathKnight:
                case WoWClass.Mage:
                case WoWClass.Warlock:
                case WoWClass.Rogue:
                case WoWClass.Warrior:
                case WoWClass.Hunter:
                case WoWClass.Paladin:
                case WoWClass.Monk:
                case WoWClass.Shaman:
                case WoWClass.Priest:

                    string error = "This is a Druid healing rotation. You are not druid.";
                    if (configOnly)
                        MessageBox.Show(error);
                    Logging.WriteFight(error);
                    break;

                #endregion

                #region Druid Specialisation checking

                case WoWClass.Druid:

                    if (wowSpecialization == WoWSpecialization.DruidRestoration)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\HealerClasses\\Settings\\Druid_Restoration.xml";
                            var currentSetting = new DruidRestoration.DruidRestorationSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<DruidRestoration.DruidRestorationSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Druid Restoration Found");
                            InternalRange = 30f;
                            new DruidRestoration();
                        }
                    }
                    else
                    {
                        string druidNonRestauration = "Class " + ObjectManager.Me.WowClass + " can be a healer, but only in Restoration specialisation.";
                        if (configOnly)
                            MessageBox.Show(druidNonRestauration);
                        Logging.WriteFight(druidNonRestauration);
                    }
                    break;

                #endregion

                default:
                    Dispose();
                    break;
            }
        }
        catch
        {
        }
        Logging.WriteFight("Healing system stopped.");
    }
}

#region Druid

public class DruidRestoration
{
    private readonly DruidRestorationSettings _mySettings = DruidRestorationSettings.GetSettings();
    #region Druid Buffs
    public readonly Spell FaerieFire = new Spell("Faerie Fire");
    public readonly Spell MarkoftheWild = new Spell("Mark of the Wild");

    #endregion

    #region Healing Cooldown

    public readonly Spell ForceofNature = new Spell("Force of Nature");
    public readonly Spell Incarnation = new Spell("Incarnation");

    #endregion

    #region Defensive Cooldown

    public readonly Spell Barkskin = new Spell("Barkskin");
    public readonly Spell IncapacitatingRoar = new Spell("Incapacitating Roar");
    public readonly Spell EntanglingRoots = new Spell("Entangling Roots");
    public readonly Spell Ironbark = new Spell("Ironbark");
    public readonly Spell MassEntanglement = new Spell("Mass Entanglement");
    public readonly Spell MightyBash = new Spell("Mighty Bash");
    public readonly Spell NaturesGrasp = new Spell("Nature's Grasp");
    public readonly Spell SolarBeam = new Spell("Solar Beam");
    public readonly Spell Typhoon = new Spell("Typhoon");
    public readonly Spell UrsolsVortex = new Spell("Ursol's Vortex");
    public readonly Spell WildCharge = new Spell("Wild Charge");

    #endregion

    #region Healing Spell

    public readonly Spell CenarionWard = new Spell("Cenarion Ward");
    public readonly Spell HealingTouch = new Spell("Healing Touch");
    public readonly Spell Lifebloom = new Spell("Lifebloom");
    public readonly Spell MightofUrsoc = new Spell("Might of Ursoc");
    public readonly Spell NaturesSwiftness = new Spell("Nature's Swiftness");
    public readonly Spell Nourish = new Spell("Nourish");
    public readonly Spell Regrowth = new Spell("Regrowth");
    public readonly Spell Rejuvenation = new Spell("Rejuvenation");
    public readonly Spell Renewal = new Spell("Renewal");
    public readonly Spell Swiftmend = new Spell("Swiftmend");
    public readonly Spell Tranquility = new Spell("Tranquility");
    public readonly Spell WildGrowth = new Spell("Wild Growth");
    public readonly Spell WildMushroom = new Spell("Wild Mushroom");
    public readonly Spell WildMushroomBloom = new Spell("Wild Mushroom: Bloom");
    public readonly Spell Genesis = new Spell("Genesis");
    private Timer _healingTouchTimer = new Timer(0);
    private Timer _nourishTimer = new Timer(0);

    #endregion

    #region Test Tank
    WoWPlayer tank = new WoWPlayer(0);
    #endregion

    public DruidRestoration()
    {
        Main.InternalRange = 30f;
        while (Main.InternalLoop)
        {
            try
            {
                if (!ObjectManager.Me.IsDead)
                {
                    if (ObjectManager.Me.Target > 0)
                    {
                        if (UnitRelation.GetReaction(ObjectManager.Target.Faction) != Reaction.Friendly)
                            Interact.InteractWith(ObjectManager.Me.GetBaseAddress, false);
                        //ObjectManager.Me.Target = ObjectManager.Me.Guid;
                    }
                    else
                        Interact.InteractWith(ObjectManager.Me.GetBaseAddress, false);
                    // ObjectManager.Me.Target = ObjectManager.Me.Guid;
                }

                if (ObjectManager.Me.InCombat)
                {
                    if (ObjectManager.Me.IsMounted)
                        MountTask.DismountMount(true);
                    if (ObjectManager.Me.HealthPercent < 100 && !Party.IsInGroup())
                    {
                        Interact.InteractWith(ObjectManager.Me.GetBaseAddress, false);
                        tank = ObjectManager.Me;
                        //ObjectManager.Me.Target = ObjectManager.Me.Guid;
                    }
                    else if (Party.IsInGroup())
                    {
                        double lowestHp = 100;
                        var lowestHpPlayer = new WoWUnit(0);
                        foreach (UInt128 playerInMyParty in Party.GetPartyPlayersGUID())
                        {
                            if (playerInMyParty <= 0) continue;
                            WoWObject obj = ObjectManager.GetObjectByGuid(playerInMyParty);
                            if (!obj.IsValid || obj.Type != WoWObjectType.Player)
                                continue;
                            var currentPlayer = new WoWPlayer(obj.GetBaseAddress);
                            var tank = new WoWPlayer(obj.GetBaseAddress);
                            if (!currentPlayer.IsValid || !currentPlayer.IsAlive) continue;

                            if (currentPlayer.HealthPercent < 100 && currentPlayer.HealthPercent < lowestHp && HealerClass.InRange(currentPlayer))
                            {
                                lowestHp = currentPlayer.HealthPercent;
                                lowestHpPlayer = currentPlayer;
                            }
                            if (currentPlayer.GetUnitRole == WoWUnit.PartyRole.Tank && tank != currentPlayer)
                            {
                                Logging.WriteFight("Player: " + currentPlayer.Name + " - Is role: " + currentPlayer.GetUnitRole + " - DEBUG ONLY");
                                tank = currentPlayer;
                            }
                        }
                        if (lowestHpPlayer.Guid > 0)
                        {
                            if (ObjectManager.Me.HealthPercent < 50 &&
                                ObjectManager.Me.HealthPercent - 10 < lowestHp)
                            {
                                lowestHpPlayer = ObjectManager.Me;
                            }

                            if (tank != null && tank.HealthPercent < 70)
                                lowestHpPlayer = tank;

                            if (ObjectManager.Me.Target != lowestHpPlayer.Guid && lowestHpPlayer.IsAlive && HealerClass.InRange(lowestHpPlayer))
                            {

                                Logging.Write("Switching to target " + lowestHpPlayer.Name + ".");
                                Interact.InteractWith(lowestHpPlayer.GetBaseAddress, false);
                                //ObjectManager.Me.Target = lowestHpPlayer.Guid;
                            }
                            else if (ObjectManager.Me.Target != ObjectManager.Me.Guid)
                                Interact.InteractWith(lowestHpPlayer.GetBaseAddress, false);
                            //ObjectManager.Me.Target = lowestHpPlayer.Guid;
                        }
                    }

                    if (HealerClass.InRange(ObjectManager.Target))
                        MountTask.DismountMount(false);
                    HealRotation();
                }
                else
                {
                    if (ObjectManager.Me.HealthPercent < 95 && !ObjectManager.Me.InCombat)
                    {
                        Logging.WriteFight("Healing to Full [ Out of Combat ]");
                        HealRotation();
                    }
                }
                Thread.Sleep(100);
            }
            catch
            {
            }
        }
    }

    private void HealRotation()
    {
        if (ObjectManager.Me.IsMounted)
            return;
        if (ObjectManager.Me.IsCast)
            return;


        if (Ironbark.IsSpellUsable && tank.HealthPercent < 40)
        {
            Interact.InteractWith(tank.GetBaseAddress, false);
            Ironbark.Cast();
        }
        if (Ironbark.IsSpellUsable && ObjectManager.Target.HealthPercent < 35)
            Ironbark.CastOnSelf();

        if (Renewal.IsSpellUsable && ObjectManager.Target.HealthPercent < 40)
            Renewal.Cast();

        if (Genesis.IsSpellUsable && ObjectManager.Target.HaveBuff(774) && ObjectManager.Target.UnitAura(774, ObjectManager.Me.Guid).AuraTimeLeftInMs < 1500)
            Genesis.Cast();

        if (Swiftmend.IsSpellUsable && ObjectManager.Target.HaveBuff(8936) && ObjectManager.Target.UnitAura(8936, ObjectManager.Me.Guid).AuraTimeLeftInMs < 1500)
            Swiftmend.Cast();
        if (Party.IsInGroup() && tank != null && ObjectManager.Target == tank)
        {
            if (Lifebloom.IsSpellUsable && !ObjectManager.Target.HaveBuff(33763) && ObjectManager.Target.HealthPercent < 95)
                Lifebloom.Cast();
        }
        else
        {
            if (Lifebloom.IsSpellUsable && !ObjectManager.Target.HaveBuff(33763) && ObjectManager.Target.HealthPercent < 95)
                Lifebloom.Cast();
        }

        if (Regrowth.IsSpellUsable && !ObjectManager.Target.HaveBuff(8936) && ObjectManager.Target.HealthPercent < 95)
            Regrowth.Cast();

        if (Rejuvenation.IsSpellUsable && !ObjectManager.Target.HaveBuff(774) && ObjectManager.Target.HealthPercent < 95)
            Rejuvenation.Cast();

        if (HealingTouch.IsSpellUsable && ObjectManager.Target.HaveBuff(33763) && ObjectManager.Target.HaveBuff(8936) && ObjectManager.Target.HaveBuff(774) && ObjectManager.Target.HealthPercent < 85)
            HealingTouch.Cast();

        if (WildMushroom.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 3
            && WildMushroom.IsSpellUsable && _mySettings.UseWildMushroom
            && ObjectManager.Me.HealthPercent < 80)
        {
            for (int i = 0; i < 3; i++)
            {
                SpellManager.CastSpellByIDAndPosition(88747, ObjectManager.Target.Position);
                Thread.Sleep(200);
            }

            WildMushroomBloom.Cast();
            return;
        }
    }

    #region Nested type: DruidRestorationSettings

    [Serializable]
    public class DruidRestorationSettings : Settings
    {
        public bool UseBarkskin = true;
        public bool UseIncapacitatingRoar = true;
        public bool UseEntanglingRoots = true;
        public bool UseFaerieFire = true;
        public bool UseForceofNature = true;
        public bool UseHealingTouch = true;
        public bool UseHurricane = true;
        public bool UseIncarnation = true;
        public bool UseIronbark = true;
        public bool UseLifeblood = true;
        public bool UseLifebloom = true;
        public bool UseLowCombat = true;
        public bool UseMarkoftheWild = true;
        public bool UseMassEntanglement = true;
        public bool UseMightofUrsoc = true;
        public bool UseMightyBash = true;
        public bool UseMoonfire = true;
        public bool UseNaturesGrasp = true;
        public bool UseNaturesSwiftness = true;
        public bool UseNourish = false;
        public bool UseRegrowth = true;
        public bool UseRejuvenation = true;
        public bool UseRenewal = true;
        public bool UseSwiftmend = true;
        public bool UseTranquility = true;
        public bool UseTrinket = true;
        public bool UseTyphoon = true;
        public bool UseUrsolsVortex = true;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;
        public bool UseWildCharge = true;
        public bool UseWildGrowth = true;
        public bool UseWildMushroom = false;
        public bool UseWrath = true;

        public DruidRestorationSettings()
        {
            ConfigWinForm("Druid Restoration Settings");
            /* Druid Buffs */
            AddControlInWinForm("Use Mark of the Wild", "UseMarkoftheWild", "Druid Buffs");
            /* Healing Cooldown */
            AddControlInWinForm("Use Force of Nature", "UseForceofNature", "Offensive Cooldown");
            AddControlInWinForm("Use Incarnation", "UseIncarnation", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Barkskin", "UseBarkskin", "Defensive Cooldown");
            AddControlInWinForm("Use Incapacitating Roar", "UseIncapacitatingRoar", "Defensive Cooldown");
            AddControlInWinForm("Use Entangling Roots", "UseEntanglingRoots", "Defensive Cooldown");
            AddControlInWinForm("Use Ironbark", "UseIronbark", "Defensive Cooldown");
            AddControlInWinForm("Use Mass Entanglement", "UseMassEntanglement", "Defensive Cooldown");
            AddControlInWinForm("Use Mighty Bash", "UseMightyBash", "Defensive Cooldown");
            AddControlInWinForm("Use Nature's Grasp", "UseNaturesGrasp", "Defensive Cooldown");
            AddControlInWinForm("Use Solar Beam", "UseSolarBeam", "Defensive Cooldown");
            AddControlInWinForm("Use Typhoon", "UseTyphoon", "Defensive Cooldown");
            AddControlInWinForm("Use Ursol's Vortex", "UseUrsolsVortex", "Defensive Cooldown");
            AddControlInWinForm("Use Wild Charge", "UseWildCharge", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Cenarion Ward", "UseCenarionWard", "Healing Spell");
            AddControlInWinForm("Use Healing Touch", "UseHealingTouch", "Healing Spell");
            AddControlInWinForm("Use Lifebloom", "UseLifebloom", "Offensive Spell");
            AddControlInWinForm("Use Might of Ursoc", "UseMightofUrsoc", "Healing Spell");
            AddControlInWinForm("Use Nature's Swiftness", "UseNaturesSwiftness", "Healing Spell");
            AddControlInWinForm("Use Nourish", "UseNourish", "Offensive Spell");
            AddControlInWinForm("Use Regrowth", "UseRegrowth", "Offensive Spell");
            AddControlInWinForm("Use Rejuvenation", "UseRejuvenation", "Healing Spell");
            AddControlInWinForm("Use Renewal", "UseRenewal", "Healing Spell");
            AddControlInWinForm("Use Swiftmend", "UseSwiftmend", "Offensive Spell");
            AddControlInWinForm("Use Tranquility", "UseTranquility", "Healing Spell");
            AddControlInWinForm("Use Wild Growth", "UseWildGrowth", "Offensive Spell");
            AddControlInWinForm("Use WildMushroom", "UseWildMushroom", "Offensive Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
        }

        public static DruidRestorationSettings CurrentSetting { get; set; }

        public static DruidRestorationSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\HealerClasses\\Settings\\Druid_Restoration.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<DruidRestorationSettings>(currentSettingsFile);
            }
            return new DruidRestorationSettings();
        }
    }

    #endregion
}

#endregion
// ReSharper restore ObjectCreationAsStatement