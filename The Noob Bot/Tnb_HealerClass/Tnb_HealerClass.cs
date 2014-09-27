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
                    #region Non healer classes detection

                case WoWClass.DeathKnight:
                case WoWClass.Mage:
                case WoWClass.Warlock:
                case WoWClass.Rogue:
                case WoWClass.Warrior:
                case WoWClass.Hunter:

                    string error = "Class " + ObjectManager.Me.WowClass + " can't be a healer.";
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

                    #region Paladin Specialisation checking

                case WoWClass.Paladin:

                    if (wowSpecialization == WoWSpecialization.PaladinHoly)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\HealerClasses\\Settings\\Paladin_Holy.xml";
                            var currentSetting = new PaladinHoly.PaladinHolySettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<PaladinHoly.PaladinHolySettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Paladin Holy Healer class...");
                            InternalRange = 30f;
                            new PaladinHoly();
                        }
                    }
                    else
                    {
                        string paladinNonHoly = "Class " + ObjectManager.Me.WowClass + " can be a healer, but only in Holy specialisation.";
                        if (configOnly)
                            MessageBox.Show(paladinNonHoly);
                        Logging.WriteFight(paladinNonHoly);
                    }
                    break;

                    #endregion

                    #region Shaman Specialisation checking

                case WoWClass.Shaman:

                    if (wowSpecialization == WoWSpecialization.ShamanRestoration)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\HealerClasses\\Settings\\Shaman_Restoration.xml";
                            var currentSetting = new ShamanRestoration.ShamanRestorationSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<ShamanRestoration.ShamanRestorationSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Shaman Restoration Healer class...");
                            InternalRange = 30f;
                            new ShamanRestoration();
                        }
                    }
                    else
                    {
                        string shamanNonRestauration = "Class " + ObjectManager.Me.WowClass + " can be a healer, but only in Restoration specialisation.";
                        if (configOnly)
                            MessageBox.Show(shamanNonRestauration);
                        Logging.WriteFight(shamanNonRestauration);
                    }
                    break;

                    #endregion

                    #region Priest Specialisation checking

                case WoWClass.Priest:

                    if (wowSpecialization == WoWSpecialization.PriestDiscipline)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\HealerClasses\\Settings\\Priest_Discipline.xml";
                            var currentSetting = new PriestDiscipline.PriestDisciplineSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<PriestDiscipline.PriestDisciplineSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Priest Discipline Healer class...");
                            InternalRange = 30f;
                            new PriestDiscipline();
                        }
                    }
                    else if (wowSpecialization == WoWSpecialization.PriestHoly)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\HealerClasses\\Settings\\Priest_Holy.xml";
                            var currentSetting = new PriestHoly.PriestHolySettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<PriestHoly.PriestHolySettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Priest Holy Healer class...");
                            InternalRange = 30f;
                            new PriestHoly();
                        }
                    }
                    else
                    {
                        string priestNonHoly = "Class " + ObjectManager.Me.WowClass + " can be a healer, but only in Holy or Discipline specialisation.";
                        if (configOnly)
                            MessageBox.Show(priestNonHoly);
                        Logging.WriteFight(priestNonHoly);
                    }
                    break;

                    #endregion

                    #region Monk Specialisation checking

                case WoWClass.Monk:

                    if (wowSpecialization == WoWSpecialization.MonkMistweaver)
                    {
                        if (configOnly)
                        {
                            string currentSettingsFile = Application.StartupPath + "\\HealerClasses\\Settings\\Monk_Mistweaver.xml";
                            var currentSetting = new MonkMistweaver.MonkMistweaverSettings();
                            if (File.Exists(currentSettingsFile) && !resetSettings)
                            {
                                currentSetting = Settings.Load<MonkMistweaver.MonkMistweaverSettings>(currentSettingsFile);
                            }
                            currentSetting.ToForm();
                            currentSetting.Save(currentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Monk Mistweaver Healer class...");
                            InternalRange = 30.0f;
                            new MonkMistweaver();
                        }
                    }
                    else
                    {
                        string monkNonMistweaver = "Class " + ObjectManager.Me.WowClass + " can be a healer, but only in Mistweaver specialisation.";
                        if (configOnly)
                            MessageBox.Show(monkNonMistweaver);
                        Logging.WriteFight(monkNonMistweaver);
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

    #region General Timers & Variables

    private Timer _alchFlaskTimer = new Timer(0);
    private Timer _engineeringTimer = new Timer(0);
    private Timer _onCd = new Timer(0);
    private Timer _trinketTimer = new Timer(0);

    #endregion

    #region Professions & Racials

    public readonly Spell Alchemy = new Spell("Alchemy");
    public readonly Spell ArcaneTorrent = new Spell("Arcane Torrent");
    public readonly Spell Berserking = new Spell("Berserking");
    public readonly Spell BloodFury = new Spell("Blood Fury");
    public readonly Spell Engineering = new Spell("Engineering");
    public readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru");
    public readonly Spell Lifeblood = new Spell("Lifeblood");
    public readonly Spell Stoneform = new Spell("Stoneform");
    public readonly Spell WarStomp = new Spell("War Stomp");

    #endregion

    #region Druid Buffs

    public readonly Spell Dash = new Spell("Dash");
    public readonly Spell FaerieFire = new Spell("Faerie Fire");
    public readonly Spell MarkoftheWild = new Spell("Mark of the Wild");
    public readonly Spell StampedingRoar = new Spell("Stampeding Roar");

    #endregion

    #region Offensive Spell

    public readonly Spell Hurricane = new Spell("Hurricane");
    public readonly Spell Moonfire = new Spell("Moonfire");
    public readonly Spell Wrath = new Spell("Wrath");
/*
        private Timer _moonfireTimer = new Timer(0);
*/

    #endregion

    #region Healing Cooldown

    public readonly Spell ForceofNature = new Spell("Force of Nature");
    public readonly Spell Incarnation = new Spell("Incarnation");

    #endregion

    #region Defensive Cooldown

    public readonly Spell Barkskin = new Spell("Barkskin");
    public readonly Spell DisorientingRoar = new Spell("Disorienting Roar");
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
    public readonly Spell Innervate = new Spell("Innervate");
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
    private Timer _healingTouchTimer = new Timer(0);
    private Timer _nourishTimer = new Timer(0);

    #endregion

    public DruidRestoration()
    {
        Main.InternalRange = 30f;

        while (Main.InternalLoop)
        {
            try
            {
                if (!ObjectManager.Me.IsDead && !Usefuls.IsLoadingOrConnecting && Usefuls.InGame)
                {
                    if (ObjectManager.Me.Target > 0)
                    {
                        if (UnitRelation.GetReaction(ObjectManager.Target.Faction) != Reaction.Friendly)
                            ObjectManager.Me.Target = ObjectManager.Me.Guid;
                    }
                    else
                        ObjectManager.Me.Target = ObjectManager.Me.Guid;
                    if (!ObjectManager.Me.IsMounted)
                    {
                        if (Heal.IsHealing)
                        {
                            if (ObjectManager.Me.HealthPercent < 100 && !Party.IsInGroup())
                            {
                                ObjectManager.Me.Target = ObjectManager.Me.Guid;
                            }
                            else if (Party.IsInGroup())
                            {
                                double lowestHp = 100;
                                var lowestHpPlayer = new WoWUnit(0);
                                foreach (Int128 playerInMyParty in Party.GetPartyPlayersGUID())
                                {
                                    if (playerInMyParty <= 0) continue;
                                    WoWObject obj = ObjectManager.GetObjectByGuid(playerInMyParty);
                                    if (!obj.IsValid || obj.Type != WoWObjectType.Player)
                                        continue;
                                    var currentPlayer = new WoWPlayer(obj.GetBaseAddress);
                                    if (!currentPlayer.IsValid || !currentPlayer.IsAlive) continue;

                                    if (currentPlayer.HealthPercent < 100 && currentPlayer.HealthPercent < lowestHp && HealerClass.InRange(currentPlayer))
                                    {
                                        lowestHp = currentPlayer.HealthPercent;
                                        lowestHpPlayer = currentPlayer;
                                    }
                                }
                                if (lowestHpPlayer.Guid > 0)
                                {
                                    if (ObjectManager.Me.HealthPercent < 50 &&
                                        ObjectManager.Me.HealthPercent - 10 < lowestHp)
                                    {
                                        lowestHpPlayer = ObjectManager.Me;
                                    }
                                    if (ObjectManager.Me.Target != lowestHpPlayer.Guid && lowestHpPlayer.IsAlive && HealerClass.InRange(lowestHpPlayer))
                                    {
                                        Logging.Write("Switching to target " + lowestHpPlayer.Name + ".");
                                        ObjectManager.Me.Target = lowestHpPlayer.Guid;
                                    }
                                }
                                else if (ObjectManager.Me.Target != ObjectManager.Me.Guid)
                                    ObjectManager.Me.Target = lowestHpPlayer.Guid;
                            }
                            else
                            {
                                Heal.IsHealing = false;
                                break;
                            }
                            if (HealerClass.InRange(ObjectManager.Target))
                                MountTask.DismountMount(false);
                            HealingFight();
                        }
                        else if (!ObjectManager.Me.IsCast)
                        {
                            Patrolling();
                        }
                    }
                }
                Thread.Sleep(500);
            }
            catch
            {
            }
        }
    }

    private void HealingFight()
    {
        Buff();
        if (_onCd.IsReady)
            DefenseCycle();
        Decast();
        HealingBurst();
        HealCycle();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (_alchFlaskTimer.IsReady && _mySettings.UseAlchFlask && Alchemy.KnownSpell
            && ItemsManager.GetItemCount(75525) == 1)
        {
            Logging.WriteFight("Use Alchi Flask");
            Lua.RunMacroText("/use item:75525");
            _alchFlaskTimer = new Timer(1000*60*60*2);
            return;
        }
        if (MarkoftheWild.KnownSpell && MarkoftheWild.IsSpellUsable && !MarkoftheWild.TargetHaveBuff
            && _mySettings.UseMarkoftheWild)
        {
            MarkoftheWild.Launch();
            return;
        }
        if (ObjectManager.GetNumberAttackPlayer() == 0 && _mySettings.UseDash
            && Dash.KnownSpell && Dash.IsSpellUsable && !Dash.TargetHaveBuff && !StampedingRoar.TargetHaveBuff
            && ObjectManager.Me.GetMove)
        {
            Dash.Launch();
            return;
        }
        if (ObjectManager.GetNumberAttackPlayer() == 0 && _mySettings.UseStampedingRoar
            && StampedingRoar.KnownSpell && StampedingRoar.IsSpellUsable && !Dash.TargetHaveBuff
            && !StampedingRoar.TargetHaveBuff && ObjectManager.Me.GetMove)
        {
            StampedingRoar.Launch();
        }
    }

    private void DefenseCycle()
    {
        if (ObjectManager.Me.HealthPercent <= _mySettings.UseStoneformAtPercentage && Stoneform.IsSpellUsable &&
            Stoneform.KnownSpell
            && _mySettings.UseStoneform)
        {
            Stoneform.Launch();
            _onCd = new Timer(1000*8);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 80 && Barkskin.KnownSpell && Barkskin.IsSpellUsable
            && _mySettings.UseBarkskin)
        {
            Barkskin.Launch();
            _onCd = new Timer(1000*12);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 80 && Ironbark.KnownSpell && Ironbark.IsSpellUsable
            && _mySettings.UseIronbark)
        {
            Ironbark.Launch();
            _onCd = new Timer(1000*12);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 80 && MightyBash.KnownSpell && MightyBash.IsSpellUsable
            && _mySettings.UseMightyBash && MightyBash.IsHostileDistanceGood)
        {
            MightyBash.Launch();
            _onCd = new Timer(1000*5);
            return;
        }
        if (MassEntanglement.KnownSpell && MassEntanglement.IsSpellUsable && MassEntanglement.IsHostileDistanceGood
            && _mySettings.UseMassEntanglement && ObjectManager.Me.HealthPercent < 80)
        {
            MassEntanglement.Launch();

            if (WildCharge.KnownSpell && WildCharge.IsHostileDistanceGood && WildCharge.IsSpellUsable
                && _mySettings.UseWildCharge)
            {
                Thread.Sleep(200);
                WildCharge.Launch();
            }
            return;
        }
        if (UrsolsVortex.KnownSpell && UrsolsVortex.IsSpellUsable && UrsolsVortex.IsHostileDistanceGood
            && _mySettings.UseUrsolsVortex && ObjectManager.Me.HealthPercent < 80)
        {
            UrsolsVortex.Launch();

            if (WildCharge.KnownSpell && WildCharge.IsHostileDistanceGood && WildCharge.IsSpellUsable
                && _mySettings.UseWildCharge)
            {
                Thread.Sleep(200);
                WildCharge.Launch();
            }
            return;
        }
        if (NaturesGrasp.KnownSpell && NaturesGrasp.IsSpellUsable
            && ObjectManager.Target.IsCast && _mySettings.UseNaturesGrasp && ObjectManager.Me.HealthPercent < 80)
        {
            NaturesGrasp.Launch();

            if (WildCharge.KnownSpell && WildCharge.IsHostileDistanceGood && WildCharge.IsSpellUsable
                && _mySettings.UseWildCharge)
            {
                Thread.Sleep(200);
                WildCharge.Launch();
            }
            return;
        }
        if (Typhoon.KnownSpell && Typhoon.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 2
            && ObjectManager.Target.GetDistance < 40 && ObjectManager.Me.HealthPercent < 70
            && _mySettings.UseTyphoon)
        {
            Typhoon.Launch();
            return;
        }
        if (DisorientingRoar.KnownSpell && DisorientingRoar.IsSpellUsable &&
            ObjectManager.GetNumberAttackPlayer() > 2
            && ObjectManager.Target.GetDistance < 10 && ObjectManager.Me.HealthPercent < 70
            && _mySettings.UseDisorientingRoar)
        {
            DisorientingRoar.Launch();
            _onCd = new Timer(1000*3);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= _mySettings.UseWarStompAtPercentage && WarStomp.IsSpellUsable &&
            WarStomp.KnownSpell
            && _mySettings.UseWarStomp)
        {
            WarStomp.Launch();
            _onCd = new Timer(1000*2);
        }
    }

    private void HealCycle()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ArcaneTorrent.IsSpellUsable && ArcaneTorrent.KnownSpell &&
            ObjectManager.Me.HealthPercent <= _mySettings.UseArcaneTorrentForResourceAtPercentage
            && _mySettings.UseArcaneTorrentForResource)
        {
            ArcaneTorrent.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 80 && NaturesSwiftness.IsSpellUsable && NaturesSwiftness.KnownSpell
            && _mySettings.UseNaturesSwiftness && _mySettings.UseHealingTouch)
        {
            NaturesSwiftness.Launch();
            Thread.Sleep(400);
            HealingTouch.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 70 && Renewal.IsSpellUsable && Renewal.KnownSpell
            && _mySettings.UseRenewal)
        {
            Renewal.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 95 && !Fight.InFight && ObjectManager.GetNumberAttackPlayer() == 0
            && HealingTouch.IsSpellUsable && HealingTouch.KnownSpell && _mySettings.UseHealingTouch)
        {
            HealingTouch.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 90 && CenarionWard.IsSpellUsable && CenarionWard.KnownSpell
            && _mySettings.UseCenarionWard)
        {
            CenarionWard.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= _mySettings.UseGiftoftheNaaruAtPercentage &&
            GiftoftheNaaru.IsSpellUsable && GiftoftheNaaru.KnownSpell
            && _mySettings.UseGiftoftheNaaru)
        {
            GiftoftheNaaru.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 90 && Rejuvenation.IsSpellUsable && Rejuvenation.KnownSpell
            && !Rejuvenation.TargetHaveBuff && _mySettings.UseRejuvenation)
        {
            Rejuvenation.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 60 && Regrowth.IsSpellUsable && Regrowth.KnownSpell
            && !Regrowth.TargetHaveBuff && _mySettings.UseRegrowth)
        {
            Regrowth.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 80 && Swiftmend.IsSpellUsable && Swiftmend.KnownSpell
            && _mySettings.UseSwiftmend)
        {
            Swiftmend.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 50 && WildGrowth.IsSpellUsable && WildGrowth.KnownSpell
            && _mySettings.UseWildGrowth)
        {
            WildGrowth.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 40 && HealingTouch.IsSpellUsable && HealingTouch.KnownSpell
            && _healingTouchTimer.IsReady && _mySettings.UseHealingTouch)
        {
            HealingTouch.Launch();
            _healingTouchTimer = new Timer(1000*15);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 40 && Nourish.IsSpellUsable && Nourish.KnownSpell
            && _nourishTimer.IsReady && _mySettings.UseNourish && !_mySettings.UseHealingTouch)
        {
            Nourish.Launch();
            _nourishTimer = new Timer(1000*15);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 35 && MightofUrsoc.IsSpellUsable && MightofUrsoc.KnownSpell
            && _mySettings.UseMightofUrsoc)
        {
            MightofUrsoc.Launch();
            return;
        }
        if (WildMushroom.KnownSpell && WildMushroom.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 3
            && WildMushroomBloom.KnownSpell && WildMushroom.IsSpellUsable && _mySettings.UseWildMushroom
            && ObjectManager.Me.HealthPercent < 80)
        {
            for (int i = 0; i < 3; i++)
            {
                SpellManager.CastSpellByIDAndPosition(88747, ObjectManager.Target.Position);
                Thread.Sleep(200);
            }

            WildMushroomBloom.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 30 && Tranquility.IsSpellUsable && Tranquility.KnownSpell
            && _mySettings.UseTranquility)
        {
            Tranquility.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(100);
                Thread.Sleep(100);
            }
            return;
        }

        if (ObjectManager.Me.ManaPercentage < 50 && _mySettings.UseInnervate)
        {
            Innervate.Launch();
        }
    }

    private void Decast()
    {
        if (ArcaneTorrent.IsSpellUsable && ArcaneTorrent.KnownSpell && ObjectManager.Target.GetDistance < 8
            && ObjectManager.Me.HealthPercent <= _mySettings.UseArcaneTorrentForDecastAtPercentage
            && _mySettings.UseArcaneTorrentForDecast && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe)
        {
            ArcaneTorrent.Launch();
        }
    }

    public void HealingBurst()
    {
        if (_mySettings.UseTrinket && _trinketTimer.IsReady && ObjectManager.Target.GetDistance < 30)
        {
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            _trinketTimer = new Timer(1000*60*2);
            return;
        }
        if (Berserking.IsSpellUsable && Berserking.KnownSpell && ObjectManager.Target.GetDistance < 30
            && _mySettings.UseBerserking)
        {
            Berserking.Launch();
            return;
        }
        if (BloodFury.IsSpellUsable && BloodFury.KnownSpell && ObjectManager.Target.GetDistance < 30
            && _mySettings.UseBloodFury)
        {
            BloodFury.Launch();
            return;
        }
        if (Lifeblood.IsSpellUsable && Lifeblood.KnownSpell && ObjectManager.Target.GetDistance < 30
            && _mySettings.UseLifeblood)
        {
            Lifeblood.Launch();
            return;
        }
        if (_engineeringTimer.IsReady && Engineering.KnownSpell && ObjectManager.Target.GetDistance < 30
            && _mySettings.UseEngGlove)
        {
            Logging.WriteFight("Use Engineering Gloves.");
            Lua.RunMacroText("/use 10");
            _engineeringTimer = new Timer(1000*60);
            return;
        }
        if (ForceofNature.IsSpellUsable && ForceofNature.KnownSpell && ForceofNature.IsHostileDistanceGood
            && _mySettings.UseForceofNature)
        {
            SpellManager.CastSpellByIDAndPosition(106737, ObjectManager.Target.Position);
            return;
        }
        if (Incarnation.IsSpellUsable && Incarnation.KnownSpell && _mySettings.UseIncarnation
            && ObjectManager.Target.GetDistance < 30)
        {
            Incarnation.Launch();
        }
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Buff();
        }
    }

    #region Nested type: DruidRestorationSettings

    [Serializable]
    public class DruidRestorationSettings : Settings
    {
        public bool UseAlchFlask = true;
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 100;
        public bool UseArcaneTorrentForResource = true;
        public int UseArcaneTorrentForResourceAtPercentage = 80;
        public bool UseBarkskin = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseCenarionWard = true;
        public bool UseDash = true;
        public bool UseDisorientingRoar = true;
        public bool UseEngGlove = true;
        public bool UseEntanglingRoots = true;
        public bool UseFaerieFire = true;
        public bool UseForceofNature = true;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseHealingTouch = true;
        public bool UseHurricane = true;
        public bool UseIncarnation = true;
        public bool UseInnervate = true;
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
        public bool UseSolarBeam = true;
        public bool UseStampedingRoar = true;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
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
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent for Interrupt", "UseArcaneTorrentForDecast", "Professions & Racials",
                "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent for Resource", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Druid Buffs */
            AddControlInWinForm("Use Dash", "UseDash", "Druid Buffs");
            AddControlInWinForm("Use Faerie Fire", "UseFaerieFire", "Druid Buffs");
            AddControlInWinForm("Use Mark of the Wild", "UseMarkoftheWild", "Druid Buffs");
            AddControlInWinForm("Use Stampeding Roar", "UseStampedingRoar", "Druid Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Hurricane", "UseHurricane", "Offensive Spell");
            AddControlInWinForm("Use Moonfire", "UseMoonfire", "Offensive Spell");
            AddControlInWinForm("Use Wrath", "UseWrath", "Offensive Spell");
            /* Healing Cooldown */
            AddControlInWinForm("Use Force of Nature", "UseForceofNature", "Offensive Cooldown");
            AddControlInWinForm("Use Incarnation", "UseIncarnation", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Barkskin", "UseBarkskin", "Defensive Cooldown");
            AddControlInWinForm("Use Disorienting Roar", "UseDisorientingRoar", "Defensive Cooldown");
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
            AddControlInWinForm("Use Innervate", "UseInnervate", "Healing Spell");
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
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
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

#region Paladin

public class PaladinHoly
{
    private readonly PaladinHolySettings _mySettings = PaladinHolySettings.GetSettings();

    #region Professions & Racial

    public readonly Spell ArcaneTorrent = new Spell("Arcane Torrent");
    public readonly Spell Berserking = new Spell("Berserking");
    public readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru");
    public readonly Spell Lifeblood = new Spell("Lifeblood");
    public readonly Spell Stoneform = new Spell("Stoneform");
    public readonly Spell WarStomp = new Spell("War Stomp");

    #endregion

    #region Paladin Seals & Buffs

    public readonly Spell BlessingOfKings = new Spell("Blessing of Kings");
    public readonly Spell BlessingOfMight = new Spell("Blessing of Might");
    public readonly Spell SealOfInsight = new Spell("Seal of Insight");
    public readonly Spell SealOfTheRighteousness = new Spell("Seal of Righteousness");
    public readonly Spell SealOfTruth = new Spell("Seal of Truth");

    #endregion

    #region Offensive Spell

    public readonly Spell Denounce = new Spell("Denounce");
    public readonly Spell HammerOfJustice = new Spell("Hammer of Justice");
    public readonly Spell HammerOfWrath = new Spell("Hammer of Wrath");
    public readonly Spell HolyShock = new Spell("Holy Shock");

    #endregion

    #region Offensive Cooldown

    public readonly Spell AvengingWrath = new Spell("Avenging Wrath");
    public readonly Spell DivineFavor = new Spell("Divine Favor");
    public readonly Spell HolyAvenger = new Spell("HolyAvenger");

    #endregion

    #region Defensive Cooldown

    public readonly Spell DevotionAura = new Spell("Devotion Aura");
    public readonly Spell DivineProtection = new Spell("Divine Protection");
    public readonly Spell DivineShield = new Spell("Divine Shield");
    public readonly Spell HandOfProtection = new Spell("Hand of Protection");
    public readonly Spell HandOfPurity = new Spell("Hand of Purity");
    public readonly Spell SacredShield = new Spell("Sacred Shield");

    #endregion

    #region Healing Spell

    public readonly Spell BeaconOfLight = new Spell("Beacon of Light");
    public readonly Spell DivineLight = new Spell("Divine Light");
    public readonly Spell DivinePlea = new Spell("Divine Plea");
    public readonly Spell FlashOfLight = new Spell("Flash of Light");
    public readonly Spell GlyphOfHarshWords = new Spell("Glyph of Harsh Words");
    public readonly Spell HolyLight = new Spell("Holy Light");
    public readonly Spell HolyRadiance = new Spell("Holy Radiance");
    public readonly Spell LayOnHands = new Spell("Lay on Hands");
    public readonly Spell WordOfGlory = new Spell("Word of Glory");

    #endregion

    public PaladinHoly()
    {
        Main.InternalRange = 30f;

        while (Main.InternalLoop)
        {
            try
            {
                if (!ObjectManager.Me.IsDead && !Usefuls.IsLoadingOrConnecting && Usefuls.InGame)
                {
                    if (ObjectManager.Me.Target > 0)
                    {
                        if (UnitRelation.GetReaction(ObjectManager.Target.Faction) != Reaction.Friendly)
                            ObjectManager.Me.Target = ObjectManager.Me.Guid;
                    }
                    else
                        ObjectManager.Me.Target = ObjectManager.Me.Guid;
                    if (!ObjectManager.Me.IsMounted)
                    {
                        if (Heal.IsHealing)
                        {
                            if (ObjectManager.Me.HealthPercent < 100 && !Party.IsInGroup())
                            {
                                ObjectManager.Me.Target = ObjectManager.Me.Guid;
                            }
                            else if (Party.IsInGroup())
                            {
                                double lowestHp = 100;
                                var lowestHpPlayer = new WoWUnit(0);
                                foreach (Int128 playerInMyParty in Party.GetPartyPlayersGUID())
                                {
                                    if (playerInMyParty <= 0) continue;
                                    WoWObject obj = ObjectManager.GetObjectByGuid(playerInMyParty);
                                    if (!obj.IsValid || obj.Type != WoWObjectType.Player)
                                        continue;
                                    var currentPlayer = new WoWPlayer(obj.GetBaseAddress);
                                    if (!currentPlayer.IsValid || !currentPlayer.IsAlive) continue;

                                    if (currentPlayer.HealthPercent < 100 && currentPlayer.HealthPercent < lowestHp && HealerClass.InRange(currentPlayer))
                                    {
                                        lowestHp = currentPlayer.HealthPercent;
                                        lowestHpPlayer = currentPlayer;
                                    }
                                }
                                if (lowestHpPlayer.Guid > 0)
                                {
                                    if (ObjectManager.Me.HealthPercent < 50 &&
                                        ObjectManager.Me.HealthPercent - 10 < lowestHp)
                                    {
                                        lowestHpPlayer = ObjectManager.Me;
                                    }
                                    if (ObjectManager.Me.Target != lowestHpPlayer.Guid && lowestHpPlayer.IsAlive && HealerClass.InRange(lowestHpPlayer))
                                    {
                                        Logging.Write("Switching to target " + lowestHpPlayer.Name + ".");
                                        ObjectManager.Me.Target = lowestHpPlayer.Guid;
                                    }
                                }
                                else if (ObjectManager.Me.Target != ObjectManager.Me.Guid)
                                    ObjectManager.Me.Target = lowestHpPlayer.Guid;
                            }
                            else
                            {
                                Heal.IsHealing = false;
                                break;
                            }
                            if (HealerClass.InRange(ObjectManager.Target))
                                MountTask.DismountMount(false);
                            HealingFight();
                        }
                        else if (!ObjectManager.Me.IsCast)
                        {
                            Patrolling();
                        }
                    }
                }
                Thread.Sleep(500);
            }
            catch
            {
            }
        }
    }

    private void HealingFight()
    {
        if (ObjectManager.Target.HealthPercent < 40)
            HealBurst();
        HealCycle();
        Buffs();
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Blessing();
        }
        Seal();
    }

    private void Buffs()
    {
        if (!ObjectManager.Me.IsMounted)
            Blessing();
        Seal();
    }

    private void Seal()
    {
        if (SealOfInsight.KnownSpell && _mySettings.UseSealOfInsight)
        {
            if (!SealOfInsight.HaveBuff && SealOfInsight.IsSpellUsable)
                SealOfInsight.Launch();
        }
        else if (SealOfTruth.KnownSpell && _mySettings.UseSealOfTruth)
        {
            if (!SealOfTruth.HaveBuff && SealOfTruth.IsSpellUsable)
                SealOfTruth.Launch();
        }
        else if (SealOfTheRighteousness.KnownSpell && _mySettings.UseSealOfTheRighteousness)
        {
            if (!SealOfTheRighteousness.HaveBuff && SealOfTheRighteousness.IsSpellUsable)
                SealOfTheRighteousness.Launch();
        }
    }

    private void Blessing()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (BlessingOfKings.KnownSpell && _mySettings.UseBlessingOfKings)
        {
            if (!BlessingOfKings.TargetHaveBuff && BlessingOfKings.IsSpellUsable)
                BlessingOfKings.Launch();
        }
        else if (BlessingOfMight.KnownSpell && _mySettings.UseBlessingOfMight)
        {
            if (!BlessingOfMight.TargetHaveBuff && BlessingOfMight.IsSpellUsable)
                BlessingOfMight.Launch();
        }
        if (BeaconOfLight.KnownSpell && _mySettings.UseBeaconOfLight)
        {
            if (!BeaconOfLight.TargetHaveBuff && BeaconOfLight.IsSpellUsable)
                BeaconOfLight.Launch();
        }
    }

    private void HealBurst()
    {
        if (DivineFavor.KnownSpell && DivineFavor.IsSpellUsable)
        {
            if (AvengingWrath.KnownSpell && AvengingWrath.IsSpellUsable && _mySettings.UseAvengingWrath)
            {
                AvengingWrath.Launch();
            }
            if (Lifeblood.KnownSpell && Lifeblood.IsSpellUsable && _mySettings.UseLifeblood)
            {
                Lifeblood.Launch();
            }
            if (HolyAvenger.KnownSpell && HolyAvenger.IsSpellUsable && _mySettings.UseHolyAvenger)
            {
                HolyAvenger.Launch();
            }
            if (_mySettings.UseDivineFavor)
                DivineFavor.Launch();
            return;
        }
        if (Lifeblood.KnownSpell && Lifeblood.IsSpellUsable && _mySettings.UseLifeblood)
        {
            Lifeblood.Launch();
        }
    }

    private void HealCycle()
    {
        if (!ObjectManager.Target.HaveBuff(25771))
        {
            if (_mySettings.UseDivineShield && ObjectManager.Target == ObjectManager.Me && DivineShield.KnownSpell && ObjectManager.Target.HealthPercent > 0 &&
                ObjectManager.Target.HealthPercent <= 20 &&
                DivineShield.IsSpellUsable)
            {
                DivineShield.Launch();
                return;
            }
            if (LayOnHands.KnownSpell && ObjectManager.Target.HealthPercent > 0 && ObjectManager.Target.HealthPercent <= 20 &&
                LayOnHands.IsSpellUsable && _mySettings.UseLayOnHands)
            {
                LayOnHands.Launch();
                return;
            }
            if (HandOfProtection.KnownSpell && ObjectManager.Target.HealthPercent > 0 &&
                ObjectManager.Target.HealthPercent <= 20 &&
                HandOfProtection.IsSpellUsable && _mySettings.UseHandOfProtection)
            {
                HandOfProtection.Launch();
                return;
            }
        }
        if (ObjectManager.Target.ManaPercentage < 30)
        {
            if (ArcaneTorrent.KnownSpell && ArcaneTorrent.IsSpellUsable && _mySettings.UseArcaneTorrentForResource)
                ArcaneTorrent.Launch();
            if (DivinePlea.KnownSpell && DivinePlea.IsSpellUsable && _mySettings.UseHandOfProtection)
            {
                DivinePlea.Launch();
                return;
            }
        }
        if (ObjectManager.Target.HealthPercent > 0 && ObjectManager.Target.HealthPercent < 50)
        {
            if (WordOfGlory.KnownSpell && WordOfGlory.IsSpellUsable && _mySettings.UseWordOfGlory)
                WordOfGlory.Launch();
            if (DivineLight.KnownSpell && DivineLight.IsSpellUsable && _mySettings.UseDivineLight)
            {
                DivineLight.Launch();
                return;
            }
            if (FlashOfLight.KnownSpell && FlashOfLight.IsSpellUsable && _mySettings.UseFlashOfLight)
            {
                FlashOfLight.Launch();
                return;
            }
            if (HolyLight.KnownSpell && HolyLight.IsSpellUsable && _mySettings.UseHolyLight)
            {
                HolyLight.Launch();
                return;
            }
        }
        if (ObjectManager.Target.HealthPercent >= 0 && ObjectManager.Target.HealthPercent < 30)
        {
            if (WordOfGlory.KnownSpell && WordOfGlory.IsSpellUsable &&
                (!GlyphOfHarshWords.KnownSpell /* || cast on me */) && _mySettings.UseWordOfGlory)
                WordOfGlory.Launch();
            if (DivineProtection.KnownSpell && DivineProtection.IsSpellUsable && _mySettings.UseDivineProtection)
                DivineProtection.Launch();
            if (FlashOfLight.KnownSpell && FlashOfLight.IsSpellUsable && _mySettings.UseFlashOfLight)
            {
                FlashOfLight.Launch();
                return;
            }
            if (HolyLight.KnownSpell && HolyLight.IsSpellUsable && _mySettings.UseHolyLight)
            {
                HolyLight.Launch();
                return;
            }
            if (DivineLight.KnownSpell && DivineLight.IsSpellUsable && _mySettings.UseDivineLight)
            {
                DivineLight.Launch();
            }
        }
    }

    #region Nested type: PaladinHolySettings

    [Serializable]
    public class PaladinHolySettings : Settings
    {
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 100;
        public bool UseArcaneTorrentForResource = true;
        public int UseArcaneTorrentForResourceAtPercentage = 80;
        public bool UseAvengingWrath = true;
        public bool UseBeaconOfLight = true;
        public bool UseBerserking = true;
        public bool UseBlessingOfKings = true;
        public bool UseBlessingOfMight = true;
        public bool UseDenounce = true;
        public bool UseDevotionAura = true;
        public bool UseDivineFavor = true;
        public bool UseDivineLight = true;
        public bool UseDivinePlea = true;
        public bool UseDivineProtection = true;
        public bool UseDivineShield = true;
        public bool UseFlashOfLight = true;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseHammerOfJustice = true;
        public bool UseHammerOfWrath = true;
        public bool UseHandOfProtection = true;
        public bool UseHandOfPurity = true;
        public bool UseHolyAvenger = true;
        public bool UseHolyLight = true;
        public bool UseHolyRadiance = true;
        public bool UseHolyShock = true;
        public bool UseLayOnHands = true;
        public bool UseLifeblood = true;
        public bool UseSacredShield = true;
        public bool UseSealOfInsight = true;
        public bool UseSealOfTheRighteousness = true;
        public bool UseSealOfTruth = true;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;
        public bool UseWordOfGlory = true;

        public PaladinHolySettings()
        {
            ConfigWinForm("Paladin Protection Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent for Interrupt", "UseArcaneTorrentForDecast", "Professions & Racials",
                "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent for Resource", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            /* Paladin Seals & Buffs */
            AddControlInWinForm("Use Seal of the Righteousness", "UseSealOfTheRighteousness", "Paladin Seals & Buffs");
            AddControlInWinForm("Use Seal of Truth", "UseSealOfTruth", "Paladin Seals & Buffs");
            AddControlInWinForm("Use Seal of Insight", "UseSealOfInsight", "Paladin Seals & Buffs");
            AddControlInWinForm("Use Blessing of Might", "UseBlessingOfMight", "Paladin Seals & Buffs");
            AddControlInWinForm("Use Blessing of Kings", "UseBlessingOfKings", "Paladin Seals & Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Holy Shock", "UseHolyShock", "Offensive Spell");
            AddControlInWinForm("Use Denounce", "UseDenounce", "Offensive Spell");
            AddControlInWinForm("Use Hammer of Justice", "UseHammerOfJustice", "Offensive Spell");
            AddControlInWinForm("Use Hammer of Wrath", "UseHammerOfWrath", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Divine Favor", "UseDivineFavor", "Offensive Cooldown");
            AddControlInWinForm("Use Holy Avenger", "UseHolyAvenger", "Offensive Cooldown");
            AddControlInWinForm("Use Avenging Wrath", "UseAvengingWrath", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Sacred Shield", "UseSacredShield", "Defensive Cooldown");
            AddControlInWinForm("Use Hand of Purity", "UseHandOfPurity", "Defensive Cooldown");
            AddControlInWinForm("Use Devotion Aura", "UseDevotionAura", "Defensive Cooldown");
            AddControlInWinForm("Use Divine Protection", "UseDivineProtection", "Defensive Cooldown");
            AddControlInWinForm("Use Divine Shield", "UseDivineShield", "Defensive Cooldown");
            AddControlInWinForm("Use Hand of Protection", "UseHandOfProtection", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Divine Plea", "UseDivinePlea", "Healing Spell");
            AddControlInWinForm("Use Divine Light", "UseDivineLight", "Healing Spell");
            AddControlInWinForm("Use Holy Radiance", "UseHolyRadiance", "Healing Spell");
            AddControlInWinForm("Use Flash of Light", "UseFlashOfLight", "Healing Spell");
            AddControlInWinForm("Use Holy Light", "UseHolyLight", "Healing Spell");
            AddControlInWinForm("Use Lay on Hands", "UseLayOnHands", "Healing Spell");
            AddControlInWinForm("Use Word of Glory", "UseWordOfGlory", "Healing Spell");
            AddControlInWinForm("Use Beacon of Light", "UseBeaconOfLight", "Healing Spell");
        }

        public static PaladinHolySettings CurrentSetting { get; set; }

        public static PaladinHolySettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\HealerClasses\\Settings\\Paladin_Holy.xml";
            if (File.Exists(currentSettingsFile))
            {
                return CurrentSetting = Load<PaladinHolySettings>(currentSettingsFile);
            }
            return new PaladinHolySettings();
        }
    }

    #endregion
}

#endregion

#region Shaman

public class ShamanRestoration
{
    private readonly ShamanRestorationSettings _mySettings = ShamanRestorationSettings.GetSettings();

    #region General Timers & Variables

    public int Lc = 0;
/*
        private Timer _alchFlaskTimer = new Timer(0);
*/
    private Timer _engineeringTimer = new Timer(0);
    private Timer _onCd = new Timer(0);
    private Timer _trinketTimer = new Timer(0);

    #endregion

    #region Professions & Racials

    public readonly Spell Alchemy = new Spell("Alchemy");
    public readonly Spell ArcaneTorrent = new Spell("Arcane Torrent");
    public readonly Spell Berserking = new Spell("Berserking");
    public readonly Spell BloodFury = new Spell("Blood Fury");
    public readonly Spell Engineering = new Spell("Engineering");
    public readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru");
    public readonly Spell Lifeblood = new Spell("Lifeblood");
    public readonly Spell Stoneform = new Spell("Stoneform");
    public readonly Spell WarStomp = new Spell("War Stomp");

    #endregion

    #region Shaman Buffs

    public readonly Spell EarthShield = new Spell("Earth Shield");
    public readonly Spell EarthlivingWeapon = new Spell("Earthliving Weapon");
    public readonly Spell FlametongueWeapon = new Spell("Flametongue Weapon");
    public readonly Spell FrostbrandWeapon = new Spell("Frostbrand Weapon");
    public readonly Spell GhostWolf = new Spell("Ghost Wolf");
    public readonly Spell LightningShield = new Spell("Lightning Shield");
    public readonly Spell RockbiterWeapon = new Spell("Rockbiter Weapon");
    public readonly Spell SpiritwalkersGrace = new Spell("Spiritwalker's Grace");
    public readonly Spell WaterShield = new Spell("Water Shield");
    public readonly Spell WaterWalking = new Spell("Water Walking");
    private Timer _waterWalkingTimer = new Timer(0);

    #endregion

    #region Offensive Spell

    public readonly Spell ChainLightning = new Spell("Chain Lightning");
    public readonly Spell EarthShock = new Spell("Earth Shock");
    public readonly Spell FlameShock = new Spell("Flame Shock");
    public readonly Spell FrostShock = new Spell("Frost Shock");
    public readonly Spell LavaBurst = new Spell("Lava Burst");
    public readonly Spell LightningBolt = new Spell("Lightning Bolt");
    public readonly Spell MagmaTotem = new Spell("Magma Totem");
    public readonly Spell PrimalStrike = new Spell("Primal Strike");
    public readonly Spell SearingTotem = new Spell("Searing Totem");
/*
        private Timer _flameShockTimer = new Timer(0);
*/

    #endregion

    #region Offensive Cooldown

    public readonly Spell AncestralSwiftness = new Spell("Ancestral Swiftness");
    public readonly Spell Ascendance = new Spell("Ascendance");
    public readonly Spell Bloodlust = new Spell("Bloodlust");
    public readonly Spell CalloftheElements = new Spell("Call of the Elements");
    public readonly Spell EarthElementalTotem = new Spell("Earth Elemental Totem");
    public readonly Spell ElementalBlast = new Spell("Elemental Blast");
    public readonly Spell ElementalMastery = new Spell("Elemental Mastery");
    public readonly Spell FireElementalTotem = new Spell("Fire Elemental Totem");
    public readonly Spell Heroism = new Spell("Heroism");
    public readonly Spell StormlashTotem = new Spell("Stormlash Totem");
    public readonly Spell TotemicProjection = new Spell("Totemic Projection");
    public readonly Spell UnleashElements = new Spell("Unleash Elements");
    public readonly Spell UnleashedFury = new Spell("Unleashed Fury");

    #endregion

    #region Defensive Cooldown

    public readonly Spell AstralShift = new Spell("Astral Shift");
    public readonly Spell CapacitorTotem = new Spell("Capacitor Totem");
    public readonly Spell EarthbindTotem = new Spell("Earthbind Totem");
    public readonly Spell GroundingTotem = new Spell("Grounding Totem");
    public readonly Spell StoneBulwarkTotem = new Spell("Stone Bulwark Totem");
    public readonly Spell WindShear = new Spell("Wind Shear");

    #endregion

    #region Healing Spell

    public readonly Spell AncestralGuidance = new Spell("Ancestral Guidance");
    public readonly Spell ChainHeal = new Spell("Chain Heal");
    public readonly Spell GreaterHealingWave = new Spell("Greater Healing Wave");
    public readonly Spell HealingRain = new Spell("Healing Rain");
    public readonly Spell HealingStreamTotem = new Spell("Healing Stream Totem");
    public readonly Spell HealingSurge = new Spell("Healing Surge");
    public readonly Spell HealingTideTotem = new Spell("Healing Tide Totem");
    public readonly Spell HealingWave = new Spell("HealingWave");
    public readonly Spell ManaTideTotem = new Spell("Mana Tide Totem");
    public readonly Spell Riptide = new Spell("Riptide");
    public readonly Spell SpiritLinkTotem = new Spell("Spirit Link Totem");
    public readonly Spell TotemicRecall = new Spell("Totemic Recall");

    #endregion

    public ShamanRestoration()
    {
        Main.InternalRange = 30f;

        while (Main.InternalLoop)
        {
            try
            {
                if (!ObjectManager.Me.IsDead && !Usefuls.IsLoadingOrConnecting && Usefuls.InGame)
                {
                    if (ObjectManager.Me.Target > 0)
                    {
                        if (UnitRelation.GetReaction(ObjectManager.Target.Faction) != Reaction.Friendly)
                            ObjectManager.Me.Target = ObjectManager.Me.Guid;
                    }
                    else
                        ObjectManager.Me.Target = ObjectManager.Me.Guid;
                    if (!ObjectManager.Me.IsMounted)
                    {
                        if (Heal.IsHealing)
                        {
                            if (ObjectManager.Me.HealthPercent < 100 && !Party.IsInGroup())
                            {
                                if (ObjectManager.Me.Target != ObjectManager.Me.Guid)
                                    ObjectManager.Me.Target = ObjectManager.Me.Guid;
                            }
                            else if (Party.IsInGroup())
                            {
                                double lowestHp = 100;
                                var lowestHpPlayer = new WoWUnit(0);
                                foreach (Int128 playerInMyParty in Party.GetPartyPlayersGUID())
                                {
                                    if (playerInMyParty <= 0) continue;
                                    WoWObject obj = ObjectManager.GetObjectByGuid(playerInMyParty);
                                    if (!obj.IsValid || obj.Type != WoWObjectType.Player)
                                        continue;
                                    var currentPlayer = new WoWPlayer(obj.GetBaseAddress);
                                    if (!currentPlayer.IsValid || !currentPlayer.IsAlive) continue;

                                    if (currentPlayer.HealthPercent < 100 && currentPlayer.HealthPercent < lowestHp && HealerClass.InRange(currentPlayer))
                                    {
                                        lowestHp = currentPlayer.HealthPercent;
                                        lowestHpPlayer = currentPlayer;
                                    }
                                }
                                if (lowestHpPlayer.Guid > 0)
                                {
                                    if (ObjectManager.Me.HealthPercent < 50 &&
                                        ObjectManager.Me.HealthPercent - 10 < lowestHp)
                                    {
                                        lowestHpPlayer = ObjectManager.Me;
                                    }
                                    if (ObjectManager.Me.Target != lowestHpPlayer.Guid && lowestHpPlayer.IsAlive && HealerClass.InRange(lowestHpPlayer))
                                    {
                                        Logging.Write("Switching to target " + lowestHpPlayer.Name + ".");
                                        ObjectManager.Me.Target = lowestHpPlayer.Guid;
                                    }
                                }
                                else if (ObjectManager.Me.Target != ObjectManager.Me.Guid)
                                    ObjectManager.Me.Target = lowestHpPlayer.Guid;
                            }
                            else
                            {
                                Heal.IsHealing = false;
                                break;
                            }
                            if (HealerClass.InRange(ObjectManager.Target))
                                MountTask.DismountMount(false);
                            HealingFight();
                        }
                        else if (!ObjectManager.Me.IsCast)
                        {
                            Patrolling();
                        }
                    }
                }
                Thread.Sleep(500);
            }
            catch
            {
            }
        }
    }

    private void HealingFight()
    {
        Buff();
        if (_onCd.IsReady)
            DefenseCycle();
        Decast();
        HealBurst();
        HealCycle();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (WaterWalking.IsSpellUsable && WaterWalking.KnownSpell &&
            (!WaterWalking.TargetHaveBuff || _waterWalkingTimer.IsReady)
            && ObjectManager.GetNumberAttackPlayer() == 0 && !Fight.InFight && _mySettings.UseWaterWalking)
        {
            WaterWalking.Launch();
            _waterWalkingTimer = new Timer(1000*60*9);
            return;
        }
        if ((ObjectManager.Me.ManaPercentage < 5 && WaterShield.KnownSpell && WaterShield.IsSpellUsable
             && _mySettings.UseWaterShield && !WaterShield.TargetHaveBuff)
            || (!_mySettings.UseLightningShield && !_mySettings.UseEarthShield))
        {
            WaterShield.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 50 && EarthShield.KnownSpell && EarthShield.IsSpellUsable
            && _mySettings.UseEarthShield && !EarthShield.TargetHaveBuff && ObjectManager.Me.ManaPercentage > 15
            || !_mySettings.UseLightningShield)
        {
            EarthShield.Launch();
            return;
        }
        if (LightningShield.KnownSpell && LightningShield.IsSpellUsable && !LightningShield.TargetHaveBuff
            && _mySettings.UseLightningShield && ObjectManager.Me.ManaPercentage > 15
            && ObjectManager.Me.HealthPercent > 70)
        {
            LightningShield.Launch();
            return;
        }
        if (ObjectManager.GetNumberAttackPlayer() > 0 && SpiritwalkersGrace.IsSpellUsable
            && SpiritwalkersGrace.KnownSpell && _mySettings.UseSpiritwalkersGrace && ObjectManager.Me.GetMove)
        {
            SpiritwalkersGrace.Launch();
            return;
        }
        if (FlametongueWeapon.KnownSpell && FlametongueWeapon.IsSpellUsable && !ObjectManager.Me.HaveBuff(10400)
            && _mySettings.UseFlametongueWeapon)
        {
            FlametongueWeapon.Launch();
            return;
        }
        if (EarthlivingWeapon.KnownSpell && EarthlivingWeapon.IsSpellUsable &&
            !ObjectManager.Me.HaveBuff(52007)
            && _mySettings.UseEarthlivingWeapon && !_mySettings.UseFlametongueWeapon)
        {
            EarthlivingWeapon.Launch();
            return;
        }
        if (FrostbrandWeapon.KnownSpell && FrostbrandWeapon.IsSpellUsable &&
            !ObjectManager.Me.HaveBuff(8034)
            && _mySettings.UseFrostbrandWeapon && !_mySettings.UseFlametongueWeapon &&
            !_mySettings.UseEarthlivingWeapon)
        {
            FrostbrandWeapon.Launch();
            return;
        }
        {
            if (RockbiterWeapon.KnownSpell && RockbiterWeapon.IsSpellUsable &&
                !ObjectManager.Me.HaveBuff(36494)
                && _mySettings.UseRockbiterWeapon && !_mySettings.UseFlametongueWeapon
                && !_mySettings.UseFrostbrandWeapon && !_mySettings.UseEarthlivingWeapon)
            {
                RockbiterWeapon.Launch();
                return;
            }
        }
        if (ObjectManager.GetNumberAttackPlayer() == 0 && GhostWolf.IsSpellUsable && GhostWolf.KnownSpell
            && _mySettings.UseGhostWolf && ObjectManager.Me.GetMove && !GhostWolf.TargetHaveBuff
            && ObjectManager.Target.GetDistance > 50)
        {
            GhostWolf.Launch();
        }
    }

    private void DefenseCycle()
    {
        if (ObjectManager.Me.HealthPercent < 50 && CapacitorTotem.KnownSpell && CapacitorTotem.IsSpellUsable
            && AirTotemReady() && _mySettings.UseCapacitorTotem)
        {
            CapacitorTotem.Launch();
            _onCd = new Timer(1000*5);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 50 && StoneBulwarkTotem.KnownSpell &&
            StoneBulwarkTotem.IsSpellUsable
            && EarthTotemReady() && _mySettings.UseStoneBulwarkTotem)
        {
            StoneBulwarkTotem.Launch();
            _onCd = new Timer(1000*10);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 70 && SpiritLinkTotem.KnownSpell &&
            SpiritLinkTotem.IsSpellUsable
            && AirTotemReady() && _mySettings.UseSpiritLinkTotem)
        {
            SpiritLinkTotem.Launch();
            _onCd = new Timer(1000*6);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= _mySettings.UseWarStompAtPercentage && WarStomp.IsSpellUsable &&
            WarStomp.KnownSpell
            && _mySettings.UseWarStomp)
        {
            WarStomp.Launch();
            _onCd = new Timer(1000*2);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= _mySettings.UseStoneformAtPercentage && Stoneform.IsSpellUsable &&
            Stoneform.KnownSpell
            && _mySettings.UseStoneform)
        {
            Stoneform.Launch();
            _onCd = new Timer(1000*8);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 70 && AstralShift.KnownSpell && AstralShift.IsSpellUsable
            && _mySettings.UseAstralShift)
        {
            AstralShift.Launch();
            _onCd = new Timer(1000*6);
        }
    }

    private void HealCycle()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ArcaneTorrent.IsSpellUsable && ArcaneTorrent.KnownSpell &&
            ObjectManager.Me.HealthPercent <= _mySettings.UseArcaneTorrentForResourceAtPercentage
            && _mySettings.UseArcaneTorrentForResource)
        {
            ArcaneTorrent.Launch();
            return;
        }
        if (ObjectManager.Me.ManaPercentage < 50 && TotemicRecall.KnownSpell && TotemicRecall.IsSpellUsable
            && _mySettings.UseTotemicRecall && ObjectManager.GetNumberAttackPlayer() == 0 && !Fight.InFight
            && TotemicRecallReady())
        {
            TotemicRecall.Launch();
            return;
        }
        if (ObjectManager.Me.ManaPercentage < 80 && ManaTideTotem.KnownSpell && ManaTideTotem.IsSpellUsable
            && _mySettings.UseManaTideTotem && WaterTotemReady())
        {
            ManaTideTotem.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 95 && HealingSurge.KnownSpell && HealingSurge.IsSpellUsable
            && ObjectManager.GetNumberAttackPlayer() == 0 && !Fight.InFight && _mySettings.UseHealingSurge)
        {
            HealingSurge.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }
        if (HealingSurge.KnownSpell && HealingSurge.IsSpellUsable && ObjectManager.Me.HealthPercent < 50
            && _mySettings.UseHealingSurge)
        {
            HealingSurge.Launch();
            return;
        }
        if (GreaterHealingWave.KnownSpell && GreaterHealingWave.IsSpellUsable
            && ObjectManager.Me.HealthPercent < 60 && _mySettings.UseGreaterHealingWave)
        {
            GreaterHealingWave.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= _mySettings.UseGiftoftheNaaruAtPercentage &&
            GiftoftheNaaru.KnownSpell && GiftoftheNaaru.IsSpellUsable
            && _mySettings.UseGiftoftheNaaru)
        {
            GiftoftheNaaru.Launch();
            return;
        }
        if (HealingTideTotem.KnownSpell && HealingTideTotem.IsSpellUsable &&
            ObjectManager.Me.HealthPercent < 70
            && WaterTotemReady() && _mySettings.UseHealingTideTotem)
        {
            HealingTideTotem.Launch();
            return;
        }
        if (AncestralGuidance.KnownSpell && AncestralGuidance.IsSpellUsable &&
            ObjectManager.Me.HealthPercent < 70
            && _mySettings.UseAncestralGuidance)
        {
            AncestralGuidance.Launch();
            return;
        }
        if (ChainHeal.KnownSpell && ChainHeal.IsSpellUsable && ObjectManager.Me.HealthPercent < 80
            && _mySettings.UseChainHeal)
        {
            ChainHeal.Launch();
            return;
        }
        if (HealingStreamTotem.KnownSpell && HealingStreamTotem.IsSpellUsable &&
            ObjectManager.Me.HealthPercent < 90
            && WaterTotemReady() && _mySettings.UseHealingStreamTotem)
        {
            HealingStreamTotem.Launch();
            return;
        }
        if (Riptide.KnownSpell && Riptide.IsSpellUsable && ObjectManager.Me.HealthPercent < 90
            && _mySettings.UseRiptide && !Riptide.TargetHaveBuff)
        {
            Riptide.Launch();
            return;
        }
        if (HealingWave.KnownSpell && HealingWave.IsSpellUsable && ObjectManager.Me.HealthPercent < 80
            && _mySettings.UseHealingWave)
        {
            HealingWave.Launch();
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && _mySettings.UseWindShear
            && WindShear.KnownSpell && WindShear.IsSpellUsable && WindShear.IsHostileDistanceGood)
        {
            WindShear.Launch();
            return;
        }
        if (ArcaneTorrent.IsSpellUsable && ArcaneTorrent.KnownSpell && ObjectManager.Target.GetDistance < 8
            && ObjectManager.Me.HealthPercent <= _mySettings.UseArcaneTorrentForDecastAtPercentage
            && _mySettings.UseArcaneTorrentForDecast && ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe)
        {
            ArcaneTorrent.Launch();
            return;
        }
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && _mySettings.UseGroundingTotem
            && GroundingTotem.KnownSpell && GroundingTotem.IsSpellUsable && AirTotemReady())
        {
            GroundingTotem.Launch();
            return;
        }
        if (ObjectManager.Target.GetMove && !FrostShock.TargetHaveBuff && _mySettings.UseFrostShock
            && FrostShock.KnownSpell && FrostShock.IsSpellUsable && FrostShock.IsHostileDistanceGood)
        {
            FrostShock.Launch();
            return;
        }
        if (ObjectManager.Target.GetMove && _mySettings.UseEarthbindTotem && EarthTotemReady()
            && EarthbindTotem.KnownSpell && EarthbindTotem.IsSpellUsable && EarthbindTotem.IsHostileDistanceGood)
        {
            EarthbindTotem.Launch();
        }
    }

    private void HealBurst()
    {
        if (_mySettings.UseTrinket && _trinketTimer.IsReady && ObjectManager.Target.GetDistance < 30)
        {
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            _trinketTimer = new Timer(1000*60*2);
            return;
        }
        if (Berserking.IsSpellUsable && Berserking.KnownSpell && ObjectManager.Target.GetDistance < 30
            && _mySettings.UseBerserking)
        {
            Berserking.Launch();
            return;
        }
        if (BloodFury.IsSpellUsable && BloodFury.KnownSpell && ObjectManager.Target.GetDistance < 30
            && _mySettings.UseBloodFury)
        {
            BloodFury.Launch();
            return;
        }
        if (Lifeblood.IsSpellUsable && Lifeblood.KnownSpell && ObjectManager.Target.GetDistance < 30
            && _mySettings.UseLifeblood)
        {
            Lifeblood.Launch();
            return;
        }
        if (_engineeringTimer.IsReady && Engineering.KnownSpell && ObjectManager.Target.GetDistance < 30
            && _mySettings.UseEngGlove)
        {
            Logging.WriteFight("Use Engineering Gloves.");
            Lua.RunMacroText("/use 10");
            _engineeringTimer = new Timer(1000*60);
            return;
        }
        if (UnleashElements.KnownSpell && UnleashElements.IsSpellUsable && UnleashedFury.KnownSpell
            && _mySettings.UseUnleashElements && UnleashElements.IsHostileDistanceGood)
        {
            UnleashElements.Launch();
            return;
        }
        if (ElementalBlast.KnownSpell && ElementalBlast.IsSpellUsable
            && _mySettings.UseElementalBlast && ElementalBlast.IsHostileDistanceGood)
        {
            ElementalBlast.Launch();
            return;
        }
        if (Ascendance.KnownSpell && Ascendance.IsSpellUsable && ObjectManager.Me.HealthPercent < 80
            && _mySettings.UseAscendance && ObjectManager.Target.GetDistance < 40)
        {
            Ascendance.Launch();
            return;
        }
        if (FireElementalTotem.KnownSpell && FireElementalTotem.IsSpellUsable
            && _mySettings.UseFireElementalTotem && ObjectManager.Target.GetDistance < 40)
        {
            FireElementalTotem.Launch();
            return;
        }
        if (StormlashTotem.KnownSpell && AirTotemReady()
            && _mySettings.UseStormlashTotem && ObjectManager.Target.GetDistance < 40)
        {
            if (!StormlashTotem.IsSpellUsable && _mySettings.UseCalloftheElements
                && CalloftheElements.KnownSpell && CalloftheElements.IsSpellUsable)
            {
                CalloftheElements.Launch();
                Thread.Sleep(200);
            }

            if (StormlashTotem.IsSpellUsable)
                StormlashTotem.Launch();
            return;
        }
        if (Bloodlust.KnownSpell && Bloodlust.IsSpellUsable && _mySettings.UseBloodlustHeroism
            && ObjectManager.Target.GetDistance < 40 && !ObjectManager.Me.HaveBuff(57724))
        {
            Bloodlust.Launch();
            return;
        }

        if (Heroism.KnownSpell && Heroism.IsSpellUsable && _mySettings.UseBloodlustHeroism
            && ObjectManager.Target.GetDistance < 40 && !ObjectManager.Me.HaveBuff(57723))
        {
            Heroism.Launch();
            return;
        }
        if (ElementalMastery.KnownSpell && ElementalMastery.IsSpellUsable
            && !ObjectManager.Me.HaveBuff(2825) && _mySettings.UseElementalMastery
            && !ObjectManager.Me.HaveBuff(32182))
        {
            ElementalMastery.Launch();
        }
    }

    private bool FireTotemReady()
    {
        if (FireElementalTotem.CreatedBySpell || MagmaTotem.CreatedBySpell)
            return false;
        return true;
    }

    private bool EarthTotemReady()
    {
        if (EarthbindTotem.CreatedBySpell || EarthElementalTotem.CreatedBySpell
            || StoneBulwarkTotem.CreatedBySpell)
            return false;
        return true;
    }

    private bool WaterTotemReady()
    {
        if (HealingStreamTotem.CreatedBySpell || HealingTideTotem.CreatedBySpell
            || ManaTideTotem.CreatedBySpell)
            return false;
        return true;
    }

    private bool AirTotemReady()
    {
        if (CapacitorTotem.CreatedBySpell || GroundingTotem.CreatedBySpell
            || StormlashTotem.CreatedBySpell || SpiritLinkTotem.CreatedBySpell)
            return false;
        return true;
    }

    private bool TotemicRecallReady()
    {
        if (FireElementalTotem.CreatedBySpell)
            return false;
        if (EarthElementalTotem.CreatedBySpell)
            return false;
        if (SearingTotem.CreatedBySpell)
            return true;
        return !FireTotemReady() || !EarthTotemReady() || !WaterTotemReady() || !AirTotemReady();
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Buff();
        }
    }

    #region Nested type: ShamanRestorationSettings

    [Serializable]
    public class ShamanRestorationSettings : Settings
    {
        public bool UseAlchFlask = true;
        public bool UseAncestralGuidance = true;
        public bool UseAncestralSwiftness = true;
        public bool UseArcaneTorrentForDecast = true;
        public int UseArcaneTorrentForDecastAtPercentage = 100;
        public bool UseArcaneTorrentForResource = true;
        public int UseArcaneTorrentForResourceAtPercentage = 80;
        public bool UseAscendance = true;
        public bool UseAstralShift = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseBloodlustHeroism = true;
        public bool UseCalloftheElements = true;
        public bool UseCapacitorTotem = true;
        public bool UseChainHeal = false;
        public bool UseChainLightning = true;
        public bool UseEarthElementalTotem = true;
        public bool UseEarthShield = true;
        public bool UseEarthShock = true;
        public bool UseEarthbindTotem = false;
        public bool UseEarthlivingWeapon = true;
        public bool UseElementalBlast = true;
        public bool UseElementalMastery = true;
        public bool UseEngGlove = true;
        public bool UseFireElementalTotem = true;
        public bool UseFlameShock = true;
        public bool UseFlametongueWeapon = true;
        public bool UseFrostShock = false;
        public bool UseFrostbrandWeapon = false;
        public bool UseGhostWolf = true;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseGreaterHealingWave = true;
        public bool UseGroundingTotem = true;
        public bool UseHealingRain = true;
        public bool UseHealingStreamTotem = true;
        public bool UseHealingSurge = true;
        public bool UseHealingTideTotem = true;
        public bool UseHealingWave = false;
        public bool UseLavaBurst = true;
        public bool UseLifeblood = true;
        public bool UseLightningBolt = true;
        public bool UseLightningShield = true;
        public bool UseLowCombat = true;
        public bool UseMagmaTotem = true;
        public bool UseManaTideTotem = true;
        public bool UsePrimalStrike = true;
        public bool UseRiptide = true;
        public bool UseRockbiterWeapon = false;
        public bool UseSearingTotem = true;
        public bool UseSpiritLinkTotem = true;
        public bool UseSpiritwalkersGrace = true;
        public bool UseStoneBulwarkTotem = true;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseStormlashTotem = true;
        public bool UseTotemicProjection = true;
        public bool UseTotemicRecall = true;
        public bool UseTrinket = true;
        public bool UseUnleashElements = true;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;
        public bool UseWaterShield = true;
        public bool UseWaterWalking = true;
        public bool UseWindShear = true;

        public ShamanRestorationSettings()
        {
            ConfigWinForm("Shaman Restoration Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent for Interrupt", "UseArcaneTorrentForDecast", "Professions & Racials",
                "AtPercentage");
            AddControlInWinForm("Use Arcane Torrent for Resource", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Shaman Buffs */
            AddControlInWinForm("Use Earth Shield", "UseEarthShield", "Shaman Buffs");
            AddControlInWinForm("Use Earthliving Weapon", "UseEarthlivingWeapon", "Shaman Buffs");
            AddControlInWinForm("Use Flametongue Weapon", "UseFlametongueWeapon", "Shaman Buffs");
            AddControlInWinForm("Use Frostbrand Weapon", "UseFrostbrandWeapon", "Shaman Buffs");
            AddControlInWinForm("Use Ghost Wolf", "UseGhostWolf", "Shaman Buffs");
            AddControlInWinForm("Use Lightning Shield", "UseLightningShield", "Shaman Buffs");
            AddControlInWinForm("Use Rockbiter Weapon", "UseRockbiterWeapon", "Shaman Buffs");
            AddControlInWinForm("Use Spiritwalker's Grace", "UseSpiritwalkersGrace", "Shaman Buffs");
            AddControlInWinForm("Use Water Shield", "UseWaterShield", "Shaman Buffs");
            AddControlInWinForm("Use Water Walking", "UseWaterWalking", "Shaman Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Chain Lightning", "UseChainLightning", "Offensive Spell");
            AddControlInWinForm("Use Earth Shock", "UseEarthShock", "Offensive Spell");
            AddControlInWinForm("Use Flame Shock", "UseFlameShock", "Offensive Spell");
            AddControlInWinForm("Use Frost Shock", "UseFrostShock", "Offensive Spell");
            AddControlInWinForm("Use Lava Burst", "UseLavaBurst", "Offensive Spell");
            AddControlInWinForm("Use Lightning Bolt", "UseLightningBolt", "Offensive Spell");
            AddControlInWinForm("Use Magma Totem", "UseMagmaTotem", "Offensive Spell");
            AddControlInWinForm("Use Searing Totem", "UseSearingTotem", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Ancestral Swiftness", "UseAncestralSwiftness", "Offensive Cooldown");
            AddControlInWinForm("Use Ascendance", "UseAscendance", "Offensive Cooldown");
            AddControlInWinForm("Use Bloodlust / Heroism", "UseBloodlustHeroism", "Offensive Cooldown");
            AddControlInWinForm("Use Call of the Elements", "UseCalloftheElements", "Offensive Cooldown");
            AddControlInWinForm("Use Earth Elemental Totem", "UseEarthElementalTotem", "Offensive Cooldown");
            AddControlInWinForm("Use Elemental Blast", "UseElementalBlast", "Offensive Cooldown");
            AddControlInWinForm("Use Elemental Mastery", "UseElementalMastery", "Offensive Cooldown");
            AddControlInWinForm("Use Fire Elemental Totem", "UseFireElementalTotem", "Offensive Cooldown");
            AddControlInWinForm("Use Stormlash Totem", "UseStormlashTotem", "Offensive Cooldown");
            AddControlInWinForm("Use Totemic Projection", "UseTotemicProjection", "Offensive Cooldown");
            AddControlInWinForm("Use Unleash Elements", "UseUnleashElements", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Astral Shift", "UseAstralShift", "Defensive Cooldown");
            AddControlInWinForm("Use Capacitor Totem", "UseCapacitorTotem", "Defensive Cooldown");
            AddControlInWinForm("Use Earthbind Totem", "UseEarthbindTotem", "Defensive Cooldown");
            AddControlInWinForm("Use Grounding Totem", "UseGroundingTotem", "Defensive Cooldown");
            AddControlInWinForm("Use StoneBulwark Totem", "UseStoneBulwarkTotem", "Defensive Cooldown");
            AddControlInWinForm("Use Wind Shear", "UseWindShear", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Ancestral Guidance", "UseAncestralGuidance", "Healing Spell");
            AddControlInWinForm("Use Chain Heal", "UseChainHeal", "Healing Spell");
            AddControlInWinForm("Use Greater Healing Wave", "UseGreaterHealingWave", "Healing Spell");
            AddControlInWinForm("Use Healing Rain", "UseHealingRain", "Healing Spell");
            AddControlInWinForm("Use Healing Surge", "UseHealingSurge", "Healing Spell");
            AddControlInWinForm("Use Healing Stream Totem", "UseHealingStreamTotem", "Healing Spell");
            AddControlInWinForm("Use Healing Tide Totem", "UsHealingTideTotem", "Healing Spell");
            AddControlInWinForm("Use Healing Wave", "UseHealingWave", "Healing Spell");
            AddControlInWinForm("Use Mana Tide Totem", "UseManaTideTotem", "Healing Spell");
            AddControlInWinForm("Use Riptide", "UseRiptide", "Healing Spell");
            AddControlInWinForm("Use Spirit Link Totem", "UseSpiritLinkTotem", "Healing Spell");
            AddControlInWinForm("Use Totemic Recall", "UseTotemicRecall", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static ShamanRestorationSettings CurrentSetting { get; set; }

        public static ShamanRestorationSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\HealerClasses\\Settings\\Shaman_Restoration.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<ShamanRestorationSettings>(currentSettingsFile);
            }
            return new ShamanRestorationSettings();
        }
    }

    #endregion
}

#endregion

#region Priest

public class PriestDiscipline
{
    private readonly PriestDisciplineSettings _mySettings = PriestDisciplineSettings.GetSettings();

    #region General Timers & Variables

    private Timer _alchFlaskTimer = new Timer(0);
    private Timer _engineeringTimer = new Timer(0);
    private Timer _onCd = new Timer(0);
    private Timer _trinketTimer = new Timer(0);

    #endregion

    #region Professions and Racials

    public readonly Spell Alchemy = new Spell("Alchemy");
    public readonly Spell ArcaneTorrent = new Spell("Arcane Torrent");
    public readonly Spell Berserking = new Spell("Berserking");
    public readonly Spell BloodFury = new Spell("Blood Fury");
    public readonly Spell Engineering = new Spell("Engineering");
    public readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru");
    public readonly Spell Lifeblood = new Spell("Lifeblood");
    public readonly Spell Stoneform = new Spell("Stoneform");
    public readonly Spell WarStomp = new Spell("War Stomp");

    #endregion

    #region Priest Buffs

    public readonly Spell InnerFire = new Spell("Inner Fire");
    public readonly Spell InnerWill = new Spell("Inner Will");
    public readonly Spell Levitate = new Spell("Levitate");
    public readonly Spell PowerWordFortitude = new Spell("Power Word: Fortitude");
/*
        private Timer _levitateTimer = new Timer(0);
*/

    #endregion

    #region Offensive Spell

    public readonly Spell Cascade = new Spell("Cascade");
    public readonly Spell DivineStar = new Spell("Divine Star");
    public readonly Spell Halo = new Spell("Halo");
    public readonly Spell MindSear = new Spell("Mind Sear");
    public readonly Spell PowerWordSolace = new Spell("Power Word: Solace");
    public readonly Spell ShadowWordDeath = new Spell("Shadow Word: Death");
    public readonly Spell ShadowWordPain = new Spell("Shadow Word: Pain");
    public readonly Spell Smite = new Spell("Smite");
/*
        private Timer _shadowWordPainTimer = new Timer(0);
*/

    #endregion

    #region Healing Cooldown

    public readonly Spell Archangel = new Spell("Archangel");
    public readonly Spell InnerFocus = new Spell("Inner Focus");
    public readonly Spell PowerInfusion = new Spell("Power Infusion");
    public readonly Spell Shadowfiend = new Spell("Shadowfiend");

    #endregion

    #region Defensive Cooldown

    public readonly Spell PainSuppression = new Spell("Pain Suppression");
    public readonly Spell PowerWordBarrier = new Spell("Power Word: Barrier");
    public readonly Spell PowerWordShield = new Spell("Power Word: Shield");
    public readonly Spell PsychicScream = new Spell("Psychic Scream");
    public readonly Spell Psyfiend = new Spell("Psyfiend");
    public readonly Spell SpectralGuise = new Spell("Spectral Guise");
    public readonly Spell VoidTendrils = new Spell("Void Tendrils");

    #endregion

    #region Healing Spell

    public readonly Spell DesperatePrayer = new Spell("Desperate Prayer");
    public readonly Spell FlashHeal = new Spell("Flash Heal");
    public readonly Spell GreaterHeal = new Spell("Greater Heal");
    public readonly Spell HealSpell = new Spell("Heal");
    public readonly Spell HolyFire = new Spell("Holy Fire");
    public readonly Spell HymnofHope = new Spell("Hymn of Hope");
    public readonly Spell Penance = new Spell("Penance");
    public readonly Spell PrayerofHealing = new Spell("Prayer of Healing");
    public readonly Spell PrayerofMending = new Spell("Prayer of Mending");
    public readonly Spell Renew = new Spell("Renew");
    public readonly Spell SpiritShell = new Spell("Spirit Shell");
/*
        private Timer _renewTimer = new Timer(0);
*/

    #endregion

    public PriestDiscipline()
    {
        Main.InternalRange = 30f;

        while (Main.InternalLoop)
        {
            try
            {
                if (!ObjectManager.Me.IsDead && !Usefuls.IsLoadingOrConnecting && Usefuls.InGame)
                {
                    if (ObjectManager.Me.Target > 0)
                    {
                        if (UnitRelation.GetReaction(ObjectManager.Target.Faction) != Reaction.Friendly)
                            ObjectManager.Me.Target = ObjectManager.Me.Guid;
                    }
                    else
                        ObjectManager.Me.Target = ObjectManager.Me.Guid;
                    if (!ObjectManager.Me.IsMounted)
                    {
                        if (Heal.IsHealing)
                        {
                            if (ObjectManager.Me.HealthPercent < 100 && !Party.IsInGroup())
                            {
                                if (ObjectManager.Me.Target != ObjectManager.Me.Guid)
                                    ObjectManager.Me.Target = ObjectManager.Me.Guid;
                            }
                            else if (Party.IsInGroup())
                            {
                                double lowestHp = 100;
                                var lowestHpPlayer = new WoWUnit(0);
                                foreach (Int128 playerInMyParty in Party.GetPartyPlayersGUID())
                                {
                                    if (playerInMyParty <= 0) continue;
                                    WoWObject obj = ObjectManager.GetObjectByGuid(playerInMyParty);
                                    if (!obj.IsValid || obj.Type != WoWObjectType.Player)
                                        continue;
                                    var currentPlayer = new WoWPlayer(obj.GetBaseAddress);
                                    if (!currentPlayer.IsValid || !currentPlayer.IsAlive) continue;

                                    if (currentPlayer.HealthPercent < 100 && currentPlayer.HealthPercent < lowestHp && HealerClass.InRange(currentPlayer))
                                    {
                                        lowestHp = currentPlayer.HealthPercent;
                                        lowestHpPlayer = currentPlayer;
                                    }
                                }
                                if (lowestHpPlayer.Guid > 0)
                                {
                                    if (ObjectManager.Me.HealthPercent < 50 &&
                                        ObjectManager.Me.HealthPercent - 10 < lowestHp)
                                    {
                                        lowestHpPlayer = ObjectManager.Me;
                                    }
                                    if (ObjectManager.Me.Target != lowestHpPlayer.Guid && lowestHpPlayer.IsAlive && HealerClass.InRange(lowestHpPlayer))
                                    {
                                        Logging.Write("Switching to target " + lowestHpPlayer.Name + ".");
                                        ObjectManager.Me.Target = lowestHpPlayer.Guid;
                                    }
                                }
                                else if (ObjectManager.Me.Target != ObjectManager.Me.Guid)
                                    ObjectManager.Me.Target = lowestHpPlayer.Guid;
                            }
                            else
                            {
                                Heal.IsHealing = false;
                                break;
                            }
                            if (HealerClass.InRange(ObjectManager.Target))
                                MountTask.DismountMount(false);
                            HealingFight();
                        }
                        else if (!ObjectManager.Me.IsCast)
                        {
                            Patrolling();
                        }
                    }
                }
                Thread.Sleep(500);
            }
            catch
            {
            }
        }
    }

/*
        private void BuffLevitate()
        {
            if (!Fight.InFight && Levitate.KnownSpell && Levitate.IsSpellUsable && _mySettings.UseLevitate
                && (!Levitate.TargetHaveBuff || _levitateTimer.IsReady))
            {
                Levitate.Launch();
                _levitateTimer = new Timer(1000*60*9);
            }
        }
*/

    private void HealingFight()
    {
        if (_onCd.IsReady)
            DefenseCycle();
        Buff();
        HealingBurst();
        HealCycle();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (PowerWordFortitude.KnownSpell && PowerWordFortitude.IsSpellUsable &&
            !PowerWordFortitude.TargetHaveBuff && _mySettings.UsePowerWordFortitude)
        {
            PowerWordFortitude.Launch();
            return;
        }
        if (InnerFire.KnownSpell && InnerFire.IsSpellUsable && !InnerFire.HaveBuff
            && _mySettings.UseInnerFire)
        {
            InnerFire.Launch();
            return;
        }
        if (InnerWill.KnownSpell && InnerWill.IsSpellUsable && !InnerWill.HaveBuff
            && !_mySettings.UseInnerFire && _mySettings.UseInnerWill)
        {
            InnerWill.Launch();
            return;
        }
        if (_alchFlaskTimer.IsReady && _mySettings.UseAlchFlask
            && ItemsManager.GetItemCount(75525) == 1)
        {
            Logging.WriteFight("Use Alchi Flask");
            Lua.RunMacroText("/use item:75525");
            _alchFlaskTimer = new Timer(1000*60*60*2);
        }
    }

    private void DefenseCycle()
    {
        if (ObjectManager.Me.HealthPercent <= _mySettings.UsePsychicScreamAtPercentage && PsychicScream.IsSpellUsable &&
            PsychicScream.KnownSpell
            && _mySettings.UsePsychicScream)
        {
            PsychicScream.Launch();
            _onCd = new Timer(1000*8);
            return;
        }
        if (ObjectManager.GetNumberAttackPlayer() >= 2 &&
            ObjectManager.Me.HealthPercent <= _mySettings.UseVoidTendrilsAtPercentage &&
            VoidTendrils.IsSpellUsable && VoidTendrils.KnownSpell && _mySettings.UseVoidTendrils)
        {
            VoidTendrils.Launch();
            _onCd = new Timer(1000*10);
            return;
        }
        if (ObjectManager.GetNumberAttackPlayer() >= 2 &&
            ObjectManager.Me.HealthPercent <= _mySettings.UsePsyfiendAtPercentage &&
            Psyfiend.IsSpellUsable && Psyfiend.KnownSpell && _mySettings.UsePsyfiend)
        {
            Psyfiend.Launch();
            _onCd = new Timer(1000*10);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= _mySettings.UseSpectralGuiseAtPercentage &&
            SpectralGuise.IsSpellUsable && SpectralGuise.KnownSpell
            && _mySettings.UseSpectralGuise)
        {
            if (Renew.KnownSpell && Renew.IsSpellUsable && _mySettings.UseRenew)
            {
                Renew.Launch();
                Thread.Sleep(1500);
            }
            SpectralGuise.Launch();
            _onCd = new Timer(1000*3);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= _mySettings.UsePowerWordBarrierAtPercentage &&
            PowerWordBarrier.IsSpellUsable && PowerWordBarrier.KnownSpell
            && _mySettings.UsePowerWordBarrier)
        {
            SpellManager.CastSpellByIDAndPosition(62618, ObjectManager.Me.Position);
            _onCd = new Timer(1000*10);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= _mySettings.UsePainSuppressionAtPercentage &&
            PainSuppression.IsSpellUsable && PainSuppression.KnownSpell
            && _mySettings.UsePainSuppression)
        {
            PainSuppression.Launch();
            _onCd = new Timer(1000*8);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= _mySettings.UseStoneformAtPercentage &&
            Stoneform.IsSpellUsable && Stoneform.KnownSpell
            && _mySettings.UseStoneform)
        {
            Stoneform.Launch();
            _onCd = new Timer(1000*8);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= _mySettings.UseWarStompAtPercentage &&
            WarStomp.IsSpellUsable && WarStomp.KnownSpell
            && _mySettings.UseWarStomp)
        {
            WarStomp.Launch();
            _onCd = new Timer(1000*2);
        }
    }

    private void HealCycle()
    {
        if (ObjectManager.Me.HealthPercent <= _mySettings.UseFlashHealNonCombatAtPercentage && !Fight.InFight &&
            ObjectManager.GetNumberAttackPlayer() == 0
            && FlashHeal.KnownSpell && FlashHeal.IsSpellUsable && _mySettings.UseFlashHealNonCombat)
        {
            FlashHeal.Launch(false);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= _mySettings.UseInnerFocusAtPercentage && InnerFocus.KnownSpell &&
            InnerFocus.IsSpellUsable
            && _mySettings.UseInnerFocus && !InnerFocus.TargetHaveBuff)
        {
            InnerFocus.Launch();
            return;
        }
        if (!Fight.InFight && ObjectManager.Me.ManaPercentage <= _mySettings.UseHymnofHopeAtPercentage &&
            HymnofHope.KnownSpell
            && HymnofHope.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() == 0 &&
            _mySettings.UseHymnofHope)
        {
            HymnofHope.Launch(false);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= _mySettings.UseDesperatePrayerAtPercentage &&
            DesperatePrayer.KnownSpell && DesperatePrayer.IsSpellUsable
            && _mySettings.UseDesperatePrayer)
        {
            DesperatePrayer.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= _mySettings.UseFlashHealInCombatAtPercentage &&
            FlashHeal.KnownSpell && FlashHeal.IsSpellUsable
            && _mySettings.UseFlashHealInCombat)
        {
            FlashHeal.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= _mySettings.UseGreaterHealAtPercentage &&
            GreaterHeal.KnownSpell && GreaterHeal.IsSpellUsable
            && _mySettings.UseGreaterHeal)
        {
            GreaterHeal.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= _mySettings.UseGiftoftheNaaruAtPercentage &&
            GiftoftheNaaru.IsSpellUsable && GiftoftheNaaru.KnownSpell
            && _mySettings.UseGiftoftheNaaru)
        {
            GiftoftheNaaru.Launch();
            return;
        }
        if (PowerWordShield.KnownSpell && PowerWordShield.IsSpellUsable
            && !PowerWordShield.TargetHaveBuff && _mySettings.UsePowerWordShield
            && !ObjectManager.Me.HaveBuff(6788) &&
            ObjectManager.Me.HealthPercent <= _mySettings.UsePowerWordShieldAtPercentage
            && (ObjectManager.GetNumberAttackPlayer() > 0 || ObjectManager.Me.GetMove))
        {
            PowerWordShield.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= _mySettings.UsePrayerofHealingAtPercentage &&
            PrayerofHealing.KnownSpell && PrayerofHealing.IsSpellUsable
            && _mySettings.UsePrayerofHealing)
        {
            PrayerofHealing.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= _mySettings.UsePrayerofMendingAtPercentage &&
            PrayerofMending.KnownSpell && PrayerofMending.IsSpellUsable
            && _mySettings.UsePrayerofMending)
        {
            PrayerofMending.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= _mySettings.UseHealAtPercentage &&
            HealSpell.KnownSpell && HealSpell.IsSpellUsable
            && (_mySettings.UseHeal || !GreaterHeal.KnownSpell))
        {
            HealSpell.Launch();
            return;
        }
        if (Renew.KnownSpell && Renew.IsSpellUsable && !Renew.TargetHaveBuff &&
            ObjectManager.Me.HealthPercent <= _mySettings.UseRenewAtPercentage &&
            _mySettings.UseRenew)
        {
            Renew.Launch();
        }
    }

    private void HealingBurst()
    {
        if (_mySettings.UseTrinket && _trinketTimer.IsReady && ObjectManager.Target.GetDistance < 30)
        {
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            _trinketTimer = new Timer(1000*60*2);
            return;
        }
        if (Berserking.IsSpellUsable && Berserking.KnownSpell && ObjectManager.Target.GetDistance < 30
            && _mySettings.UseBerserking)
        {
            Berserking.Launch();
            return;
        }
        if (BloodFury.IsSpellUsable && BloodFury.KnownSpell && ObjectManager.Target.GetDistance < 30
            && _mySettings.UseBloodFury)
        {
            BloodFury.Launch();
            return;
        }
        if (Lifeblood.IsSpellUsable && Lifeblood.KnownSpell && ObjectManager.Target.GetDistance < 30
            && _mySettings.UseLifeblood)
        {
            Lifeblood.Launch();
            return;
        }
        if (_engineeringTimer.IsReady && Engineering.KnownSpell && ObjectManager.Target.GetDistance < 30
            && _mySettings.UseEngGlove)
        {
            Logging.WriteFight("Use Engineering Gloves.");
            Lua.RunMacroText("/use 10");
            _engineeringTimer = new Timer(1000*60);
            return;
        }
        if (PowerInfusion.IsSpellUsable && PowerInfusion.KnownSpell
            && _mySettings.UsePowerInfusion && ObjectManager.Target.GetDistance < 40)
        {
            PowerInfusion.Launch();
            return;
        }
        if (Archangel.IsSpellUsable && Archangel.KnownSpell && ObjectManager.Me.BuffStack(81661) > 4
            && _mySettings.UseArchangel && ObjectManager.Target.GetDistance < 40)
        {
            Archangel.Launch();
            return;
        }
        if (SpiritShell.IsSpellUsable && SpiritShell.KnownSpell && ObjectManager.Me.HealthPercent > 80
            && _mySettings.UseSpiritShell && ObjectManager.Target.InCombat)
        {
            SpiritShell.Launch();
            return;
        }
        if (Shadowfiend.IsSpellUsable && Shadowfiend.KnownSpell && Shadowfiend.IsHostileDistanceGood
            && _mySettings.UseShadowfiend && ObjectManager.Me.InCombat)
        {
            WoWUnit unit = null;
            if (ObjectManager.GetNumberAttackPlayer() > 0)
                unit =
                    ObjectManager.GetNearestWoWUnit(ObjectManager.GetUnitAttackPlayer());

            if (unit != null)
                if (unit.IsValid)
                {
                    ObjectManager.Me.Target = unit.Guid;
                    Shadowfiend.Launch();
                }
        }
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Buff();
        }
    }

    #region Nested type: PriestDisciplineSettings

    [Serializable]
    public class PriestDisciplineSettings : Settings
    {
        public bool UseAlchFlask = true;
        public bool UseArcaneTorrent = true;
        public bool UseArchangel = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseCascade = true;
        public bool UseDesperatePrayer = true;
        public int UseDesperatePrayerAtPercentage = 65;
        public bool UseDivineStar = true;
        public bool UseEngGlove = true;
        public bool UseFlashHealInCombat = true;
        public int UseFlashHealInCombatAtPercentage = 60;
        public bool UseFlashHealNonCombat = true;
        public int UseFlashHealNonCombatAtPercentage = 95;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseGreaterHeal = true;
        public int UseGreaterHealAtPercentage = 70;
        public bool UseHalo = true;
        public bool UseHeal = true;
        public int UseHealAtPercentage = 70;
        public bool UseHolyFire = true;
        public bool UseHymnofHope = true;
        public int UseHymnofHopeAtPercentage = 40;
        public bool UseInnerFire = true;
        public bool UseInnerFocus = true;
        public int UseInnerFocusAtPercentage = 90;
        public bool UseInnerWill = false;
        public bool UseLevitate = false;
        public bool UseLifeblood = true;
        public bool UseMindSear = true;
        public bool UsePainSuppression = true;
        public int UsePainSuppressionAtPercentage = 70;
        public bool UsePenance = true;
        public bool UsePowerInfusion = true;
        public bool UsePowerWordBarrier = true;
        public int UsePowerWordBarrierAtPercentage = 60;
        public bool UsePowerWordFortitude = true;
        public bool UsePowerWordShield = true;
        public int UsePowerWordShieldAtPercentage = 100;
        public bool UsePowerWordSolace = true;
        public bool UsePrayerofHealing = false;
        public int UsePrayerofHealingAtPercentage = 50;
        public bool UsePrayerofMending = true;
        public int UsePrayerofMendingAtPercentage = 50;
        public bool UsePsychicScream = true;
        public int UsePsychicScreamAtPercentage = 20;
        public bool UsePsyfiend = true;
        public int UsePsyfiendAtPercentage = 35;
        public bool UseRenew = true;
        public int UseRenewAtPercentage = 90;
        public bool UseShadowWordDeath = true;
        public bool UseShadowWordPain = true;
        public bool UseShadowfiend = true;
        public bool UseSmite = true;
        public bool UseSpectralGuise = true;
        public int UseSpectralGuiseAtPercentage = 70;
        public bool UseSpiritShell = true;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseTrinket = true;
        public bool UseVoidTendrils = true;
        public int UseVoidTendrilsAtPercentage = 35;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;

        public PriestDisciplineSettings()
        {
            ConfigWinForm("Discipline Priest Settings");
            /* Professions and Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions and Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions and Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions and Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions and Racials", "AtPercentage");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions and Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions and Racials", "AtPercentage");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions and Racials", "AtPercentage");
            /* Priest Buffs */
            AddControlInWinForm("Use Inner Fire", "UseInnerFire", "Priest Buffs");
            AddControlInWinForm("Use Inner Will", "UseInnerWill", "Priest Buffs");
            AddControlInWinForm("Use Levitate", "UseLevitate", "Priest Buffs");
            AddControlInWinForm("Use Power Word: Fortitude", "UsePowerWordFortitude", "Priest Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Cascade", "UseCascade", "Offensive Spell");
            AddControlInWinForm("Use Divine Star", "Use Divine Star", "Offensive Spell");
            AddControlInWinForm("Use Halo", "UseHalo", "Offensive Spell");
            AddControlInWinForm("Use Holy Fire", "UseHolyFire", "Offensive Spell");
            AddControlInWinForm("Use Mind Sear", "UseMindSear", "Offensive Spell");
            AddControlInWinForm("Use Penance", "UsePenance", "Offensive Spell");
            AddControlInWinForm("Use Shadow Word: Death", "UseShadowWordDeath", "Offensive Spell");
            AddControlInWinForm("Use Shadow Word: Pain", "UseShadowWordPain", "Offensive Spell");
            AddControlInWinForm("Use Smite", "UseSmite", "Offensive Spell");
            /* Healing Cooldown */
            AddControlInWinForm("Use Archangel", "UseArchangel", "Healing Cooldown");
            AddControlInWinForm("Use Power Infusion", "UsePowerInfusion", "Healing Cooldown");
            AddControlInWinForm("Use Shadowfiend", "UseShadowfiend", "Healing Cooldown");
            AddControlInWinForm("Use Spirit Shell", "UseSpiritShell", "Healing Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Pain Suppression", "UsePainSuppression", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Power Word: Barrier", "UsePowerWordBarrier", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Power Word: Shield", "UsePowerWordShield", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Psychic Scream", "UsePsychicScream", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Psyfiend", "UsePsyfiend", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Spectral Guise", "UseSpectralGuise", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Void Tendrils", "UseVoidTendrils", "Defensive Cooldown", "AtPercentage");
            /* Healing Spell */
            AddControlInWinForm("Use Desperate Prayer", "UseDesperatePrayer", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Flash Heal for Regeneration after combat", "UseFlashHealNonCombat", "Healing Spell",
                "AtPercentage");
            AddControlInWinForm("Use Flash Heal during combat", "UseFlashHealInCombat", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Greater Heal", "UseGreaterHeal", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Heal", "UseHeal", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Hymn of Hope", "UseHymnofHope", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Inner Focus", "UseInnerFocus", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Prayer of Mending", "UsePrayerofMending", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Renew", "UseRenew", "Healing Spell", "AtPercentage");
            /* Game Settings */
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static PriestDisciplineSettings CurrentSetting { get; set; }

        public static PriestDisciplineSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\HealerClasses\\Settings\\Priest_Discipline.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<PriestDisciplineSettings>(currentSettingsFile);
            }
            return new PriestDisciplineSettings();
        }
    }

    #endregion
}

public class PriestHoly
{
    private readonly PriestHolySettings _mySettings = PriestHolySettings.GetSettings();

    #region General Timers & Variables

    private Timer _alchFlaskTimer = new Timer(0);
    private Timer _engineeringTimer = new Timer(0);
    private Timer _onCd = new Timer(0);
    private Timer _trinketTimer = new Timer(0);

    #endregion

    #region Professions and Racials

    public readonly Spell Alchemy = new Spell("Alchemy");
    public readonly Spell ArcaneTorrent = new Spell("Arcane Torrent");
    public readonly Spell Berserking = new Spell("Berserking");
    public readonly Spell BloodFury = new Spell("Blood Fury");
    public readonly Spell Engineering = new Spell("Engineering");
    public readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru");
    public readonly Spell Lifeblood = new Spell("Lifeblood");
    public readonly Spell Stoneform = new Spell("Stoneform");
    public readonly Spell WarStomp = new Spell("War Stomp");

    #endregion

    #region Priest Buffs

    public readonly Spell ChakraChastise = new Spell("Chakra: Chastise");
    public readonly Spell ChakraSanctuary = new Spell("Chakra: Sanctuary");
    public readonly Spell ChakraSerenity = new Spell("Chakra: Serenity");
    public readonly Spell InnerFire = new Spell("Inner Fire");
    public readonly Spell InnerWill = new Spell("Inner Will");
    public readonly Spell Levitate = new Spell("Levitate");
    public readonly Spell PowerWordFortitude = new Spell("Power Word: Fortitude");
/*
        private Timer _levitateTimer = new Timer(0);
*/

    #endregion

    #region Offensive Spell

    public readonly Spell Cascade = new Spell("Cascade");
    public readonly Spell DivineStar = new Spell("Divine Star");
    public readonly Spell Halo = new Spell("Halo");
    public readonly Spell HolyWordChastise = new Spell("Holy Word: Chastise");
    public readonly Spell MindSear = new Spell("Mind Sear");
    public readonly Spell PowerWordSolace = new Spell("Power Word: Solace");
    public readonly Spell ShadowWordDeath = new Spell("Shadow Word: Death");
    public readonly Spell ShadowWordPain = new Spell("Shadow Word: Pain");
    public readonly Spell Smite = new Spell("Smite");
/*
        private Timer _shadowWordPainTimer = new Timer(0);
*/

    #endregion

    #region Healing Cooldown

    public readonly Spell DivineHymn = new Spell("Divine Hymn");
    public readonly Spell LightWell = new Spell("Light Well");
    public readonly Spell PowerInfusion = new Spell("Power Infusion");
    public readonly Spell Shadowfiend = new Spell("Shadowfiend");

    #endregion

    #region Defensive Cooldown

    public readonly Spell GuardianSpirit = new Spell("Guardian Spirit");
    public readonly Spell PowerWordShield = new Spell("Power Word: Shield");
    public readonly Spell PsychicScream = new Spell("Psychic Scream");
    public readonly Spell Psyfiend = new Spell("Psyfiend");
    public readonly Spell SpectralGuise = new Spell("Spectral Guise");
    public readonly Spell VoidTendrils = new Spell("Void Tendrils");

    #endregion

    #region Healing Spell

    public readonly Spell CircleofHealing = new Spell("Circle of Healing");
    public readonly Spell DesperatePrayer = new Spell("Desperate Prayer");
    public readonly Spell FlashHeal = new Spell("Flash Heal");
    public readonly Spell GreaterHeal = new Spell("Greater Heal");
    public readonly Spell HealSpell = new Spell("Heal");
    public readonly Spell HolyFire = new Spell("Holy Fire");
    public readonly Spell HymnofHope = new Spell("Hymn of Hope");
    public readonly Spell PrayerofHealing = new Spell("Prayer of Healing");
    public readonly Spell PrayerofMending = new Spell("Prayer of Mending");
    public readonly Spell Renew = new Spell("Renew");
/*
        private Timer _renewTimer = new Timer(0);
*/

    #endregion

    public PriestHoly()
    {
        Main.InternalRange = 30f;

        while (Main.InternalLoop)
        {
            try
            {
                if (!ObjectManager.Me.IsDead && !Usefuls.IsLoadingOrConnecting && Usefuls.InGame)
                {
                    if (ObjectManager.Me.Target > 0)
                    {
                        if (UnitRelation.GetReaction(ObjectManager.Target.Faction) != Reaction.Friendly)
                            ObjectManager.Me.Target = ObjectManager.Me.Guid;
                    }
                    else
                        ObjectManager.Me.Target = ObjectManager.Me.Guid;
                    if (!ObjectManager.Me.IsMounted)
                    {
                        if (Heal.IsHealing)
                        {
                            if (ObjectManager.Me.HealthPercent < 100 && !Party.IsInGroup())
                            {
                                if (ObjectManager.Me.Target != ObjectManager.Me.Guid)
                                    ObjectManager.Me.Target = ObjectManager.Me.Guid;
                            }
                            else if (Party.IsInGroup())
                            {
                                double lowestHp = 100;
                                var lowestHpPlayer = new WoWUnit(0);
                                foreach (Int128 playerInMyParty in Party.GetPartyPlayersGUID())
                                {
                                    if (playerInMyParty <= 0) continue;
                                    WoWObject obj = ObjectManager.GetObjectByGuid(playerInMyParty);
                                    if (!obj.IsValid || obj.Type != WoWObjectType.Player)
                                        continue;
                                    var currentPlayer = new WoWPlayer(obj.GetBaseAddress);
                                    if (!currentPlayer.IsValid || !currentPlayer.IsAlive) continue;

                                    if (currentPlayer.HealthPercent < 100 && currentPlayer.HealthPercent < lowestHp && HealerClass.InRange(currentPlayer))
                                    {
                                        lowestHp = currentPlayer.HealthPercent;
                                        lowestHpPlayer = currentPlayer;
                                    }
                                }
                                if (lowestHpPlayer.Guid > 0)
                                {
                                    if (ObjectManager.Me.HealthPercent < 50 &&
                                        ObjectManager.Me.HealthPercent - 10 < lowestHp)
                                    {
                                        lowestHpPlayer = ObjectManager.Me;
                                    }
                                    if (ObjectManager.Me.Target != lowestHpPlayer.Guid && lowestHpPlayer.IsAlive && HealerClass.InRange(lowestHpPlayer))
                                    {
                                        Logging.Write("Switching to target " + lowestHpPlayer.Name + ".");
                                        ObjectManager.Me.Target = lowestHpPlayer.Guid;
                                    }
                                }
                                else if (ObjectManager.Me.Target != ObjectManager.Me.Guid)
                                    ObjectManager.Me.Target = lowestHpPlayer.Guid;
                            }
                            else
                            {
                                Heal.IsHealing = false;
                                break;
                            }
                            if (HealerClass.InRange(ObjectManager.Target))
                                MountTask.DismountMount(false);
                            HealingFight();
                        }
                        else if (!ObjectManager.Me.IsCast)
                        {
                            Patrolling();
                        }
                    }
                }
                Thread.Sleep(500);
            }
            catch
            {
            }
        }
    }

/*
        private void BuffLevitate()
        {
            if (!Fight.InFight && Levitate.KnownSpell && Levitate.IsSpellUsable && _mySettings.UseLevitate
                && (!Levitate.TargetHaveBuff || _levitateTimer.IsReady))
            {
                Levitate.Launch();
                _levitateTimer = new Timer(1000*60*9);
            }
        }
*/

    private void HealingFight()
    {
        if (_onCd.IsReady)
            DefenseCycle();
        Buff();
        HealingBurst();
        HealCycle();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (PowerWordFortitude.KnownSpell && PowerWordFortitude.IsSpellUsable &&
            !PowerWordFortitude.TargetHaveBuff && _mySettings.UsePowerWordFortitude)
        {
            PowerWordFortitude.Launch();
            return;
        }
        if (InnerFire.KnownSpell && InnerFire.IsSpellUsable && !InnerFire.HaveBuff
            && _mySettings.UseInnerFire)
        {
            InnerFire.Launch();
            return;
        }
        if (InnerWill.KnownSpell && InnerWill.IsSpellUsable && !InnerWill.HaveBuff
            && !_mySettings.UseInnerFire && _mySettings.UseInnerWill)
        {
            InnerWill.Launch();
            return;
        }
        if (ChakraChastise.KnownSpell && ChakraChastise.IsSpellUsable && !ChakraChastise.TargetHaveBuff
            && _mySettings.UseChakraChastise)
        {
            ChakraChastise.Launch();
            return;
        }
        if (ChakraSanctuary.KnownSpell && ChakraSanctuary.IsSpellUsable && !ChakraSanctuary.TargetHaveBuff
            && !_mySettings.UseChakraChastise && _mySettings.UseChakraSanctuary)
        {
            ChakraSanctuary.Launch();
            return;
        }
        if (ChakraSerenity.KnownSpell && ChakraSerenity.IsSpellUsable && !ChakraSerenity.TargetHaveBuff
            && !_mySettings.UseChakraChastise && !_mySettings.UseChakraSanctuary && _mySettings.UseChakraSerenity)
        {
            ChakraSerenity.Launch();
            return;
        }
        if (_alchFlaskTimer.IsReady && _mySettings.UseAlchFlask
            && ItemsManager.GetItemCount(75525) == 1)
        {
            Logging.WriteFight("Use Alchi Flask");
            Lua.RunMacroText("/use item:75525");
            _alchFlaskTimer = new Timer(1000*60*60*2);
        }
    }

    private void DefenseCycle()
    {
        if (ObjectManager.Me.HealthPercent <= _mySettings.UsePsychicScreamAtPercentage && PsychicScream.IsSpellUsable &&
            PsychicScream.KnownSpell
            && _mySettings.UsePsychicScream)
        {
            PsychicScream.Launch();
            _onCd = new Timer(1000*8);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= _mySettings.UseGuardianSpiritAtPercentage && GuardianSpirit.KnownSpell &&
            GuardianSpirit.IsSpellUsable
            && _mySettings.UseGuardianSpirit)
        {
            GuardianSpirit.Launch();
            return;
        }
        if (ObjectManager.GetNumberAttackPlayer() >= 2 &&
            ObjectManager.Me.HealthPercent <= _mySettings.UseVoidTendrilsAtPercentage &&
            VoidTendrils.IsSpellUsable && VoidTendrils.KnownSpell && _mySettings.UseVoidTendrils)
        {
            VoidTendrils.Launch();
            _onCd = new Timer(1000*10);
            return;
        }
        if (ObjectManager.GetNumberAttackPlayer() >= 2 &&
            ObjectManager.Me.HealthPercent <= _mySettings.UsePsyfiendAtPercentage &&
            Psyfiend.IsSpellUsable && Psyfiend.KnownSpell && _mySettings.UsePsyfiend)
        {
            Psyfiend.Launch();
            _onCd = new Timer(1000*10);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= _mySettings.UseSpectralGuiseAtPercentage &&
            SpectralGuise.IsSpellUsable && SpectralGuise.KnownSpell
            && _mySettings.UseSpectralGuise)
        {
            if (Renew.KnownSpell && Renew.IsSpellUsable && _mySettings.UseRenew)
            {
                Renew.Launch();
                Thread.Sleep(1500);
            }
            SpectralGuise.Launch();
            _onCd = new Timer(1000*3);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= _mySettings.UseStoneformAtPercentage &&
            Stoneform.IsSpellUsable && Stoneform.KnownSpell
            && _mySettings.UseStoneform)
        {
            Stoneform.Launch();
            _onCd = new Timer(1000*8);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= _mySettings.UseWarStompAtPercentage &&
            WarStomp.IsSpellUsable && WarStomp.KnownSpell
            && _mySettings.UseWarStomp)
        {
            WarStomp.Launch();
            _onCd = new Timer(1000*2);
        }
    }

    private void HealCycle()
    {
        if (ObjectManager.Me.HealthPercent <= _mySettings.UseFlashHealNonCombatAtPercentage && !Fight.InFight &&
            ObjectManager.GetNumberAttackPlayer() == 0
            && FlashHeal.KnownSpell && FlashHeal.IsSpellUsable && _mySettings.UseFlashHealNonCombat)
        {
            FlashHeal.Launch(false);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= _mySettings.UseDivineHymnAtPercentage && DivineHymn.KnownSpell &&
            DivineHymn.IsSpellUsable
            && _mySettings.UseDivineHymn)
        {
            DivineHymn.Launch();
            return;
        }
        if (!Fight.InFight && ObjectManager.Me.ManaPercentage <= _mySettings.UseHymnofHopeAtPercentage &&
            HymnofHope.KnownSpell
            && HymnofHope.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() == 0 &&
            _mySettings.UseHymnofHope)
        {
            HymnofHope.Launch(false);
            return;
        }
        if (ObjectManager.Me.HealthPercent <= _mySettings.UseDesperatePrayerAtPercentage &&
            DesperatePrayer.KnownSpell && DesperatePrayer.IsSpellUsable
            && _mySettings.UseDesperatePrayer)
        {
            DesperatePrayer.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= _mySettings.UseFlashHealInCombatAtPercentage &&
            FlashHeal.KnownSpell && FlashHeal.IsSpellUsable
            && _mySettings.UseFlashHealInCombat)
        {
            FlashHeal.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= _mySettings.UseGreaterHealAtPercentage &&
            GreaterHeal.KnownSpell && GreaterHeal.IsSpellUsable
            && _mySettings.UseGreaterHeal)
        {
            GreaterHeal.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= _mySettings.UseGiftoftheNaaruAtPercentage &&
            GiftoftheNaaru.IsSpellUsable && GiftoftheNaaru.KnownSpell
            && _mySettings.UseGiftoftheNaaru)
        {
            GiftoftheNaaru.Launch();
            return;
        }
        if (PowerWordShield.KnownSpell && PowerWordShield.IsSpellUsable
            && !PowerWordShield.TargetHaveBuff && _mySettings.UsePowerWordShield
            && !ObjectManager.Me.HaveBuff(6788) &&
            ObjectManager.Me.HealthPercent <= _mySettings.UsePowerWordShieldAtPercentage
            && (ObjectManager.GetNumberAttackPlayer() > 0 || ObjectManager.Me.GetMove))
        {
            PowerWordShield.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= _mySettings.UsePrayerofHealingAtPercentage &&
            PrayerofHealing.KnownSpell && PrayerofHealing.IsSpellUsable
            && _mySettings.UsePrayerofHealing)
        {
            PrayerofHealing.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= _mySettings.UseCircleofHealingAtPercentage &&
            CircleofHealing.KnownSpell && CircleofHealing.IsSpellUsable
            && _mySettings.UseCircleofHealing)
        {
            SpellManager.CastSpellByIDAndPosition(34861, ObjectManager.Me.Position);
            return;
        }
        if (ObjectManager.Me.HealthPercent <=
            _mySettings.UsePrayerofMendingAtPercentage &&
            PrayerofMending.KnownSpell && PrayerofMending.IsSpellUsable
            && _mySettings.UsePrayerofMending)
        {
            PrayerofMending.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent <= _mySettings.UseHealAtPercentage &&
            HealSpell.KnownSpell && HealSpell.IsSpellUsable
            && (_mySettings.UseHeal || !GreaterHeal.KnownSpell))
        {
            HealSpell.Launch();
            return;
        }
        if (LightWell.KnownSpell && LightWell.IsSpellUsable &&
            _mySettings.UseGlyphofLightspring
            &&
            ObjectManager.Me.HealthPercent <=
            _mySettings.UseLightWellAtPercentage && _mySettings.UseLightWell)
        {
            SpellManager.CastSpellByIDAndPosition(724,
                ObjectManager.Target
                    .Position);
            return;
        }
        if (Renew.KnownSpell && Renew.IsSpellUsable && !Renew.TargetHaveBuff &&
            ObjectManager.Me.HealthPercent <=
            _mySettings.UseRenewAtPercentage && _mySettings.UseRenew)
        {
            Renew.Launch();
        }
    }

    private void HealingBurst()
    {
        if (_mySettings.UseTrinket && _trinketTimer.IsReady && ObjectManager.Target.GetDistance < 30)
        {
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            _trinketTimer = new Timer(1000*60*2);
            return;
        }
        if (Berserking.IsSpellUsable && Berserking.KnownSpell && ObjectManager.Target.GetDistance < 30
            && _mySettings.UseBerserking)
        {
            Berserking.Launch();
            return;
        }
        if (BloodFury.IsSpellUsable && BloodFury.KnownSpell && ObjectManager.Target.GetDistance < 30
            && _mySettings.UseBloodFury)
        {
            BloodFury.Launch();
            return;
        }
        if (Lifeblood.IsSpellUsable && Lifeblood.KnownSpell && ObjectManager.Target.GetDistance < 30
            && _mySettings.UseLifeblood)
        {
            Lifeblood.Launch();
            return;
        }
        if (_engineeringTimer.IsReady && Engineering.KnownSpell && ObjectManager.Target.GetDistance < 30
            && _mySettings.UseEngGlove)
        {
            Logging.WriteFight("Use Engineering Gloves.");
            Lua.RunMacroText("/use 10");
            _engineeringTimer = new Timer(1000*60);
            return;
        }
        if (PowerInfusion.IsSpellUsable && PowerInfusion.KnownSpell
            && _mySettings.UsePowerInfusion && ObjectManager.Target.GetDistance < 40)
        {
            PowerInfusion.Launch();
            return;
        }
        if (Shadowfiend.IsSpellUsable && Shadowfiend.KnownSpell && Shadowfiend.IsHostileDistanceGood
            && _mySettings.UseShadowfiend && ObjectManager.Me.InCombat)
        {
            WoWUnit unit = null;
            if (ObjectManager.GetNumberAttackPlayer() > 0)
                unit =
                    ObjectManager.GetNearestWoWUnit(ObjectManager.GetUnitAttackPlayer());

            if (unit != null)
                if (unit.IsValid)
                {
                    ObjectManager.Me.Target = unit.Guid;
                    Shadowfiend.Launch();
                }
        }
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Buff();
        }
    }

    #region Nested type: PriestHolySettings

    [Serializable]
    public class PriestHolySettings : Settings
    {
        public bool UseAlchFlask = true;
        public bool UseArcaneTorrent = true;
        public bool UseArchangel = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseCascade = true;
        public bool UseChakraChastise = true;
        public bool UseChakraSanctuary = false;
        public bool UseChakraSerenity = false;
        public bool UseCircleofHealing = false;
        public int UseCircleofHealingAtPercentage = 50;
        public bool UseDesperatePrayer = true;
        public int UseDesperatePrayerAtPercentage = 65;
        public bool UseDivineHymn = true;
        public int UseDivineHymnAtPercentage = 30;
        public bool UseDivineStar = true;
        public bool UseEngGlove = true;
        public bool UseFlashHealInCombat = true;
        public int UseFlashHealInCombatAtPercentage = 60;
        public bool UseFlashHealNonCombat = true;
        public int UseFlashHealNonCombatAtPercentage = 95;
        public bool UseGiftoftheNaaru = true;
        public int UseGiftoftheNaaruAtPercentage = 80;
        public bool UseGlyphofLightspring = false;
        public bool UseGreaterHeal = true;
        public int UseGreaterHealAtPercentage = 70;
        public bool UseGuardianSpirit = true;
        public int UseGuardianSpiritAtPercentage = 20;
        public bool UseHalo = true;
        public bool UseHeal = true;
        public int UseHealAtPercentage = 70;
        public bool UseHolyFire = true;
        public bool UseHolyWordChastise = true;
        public bool UseHymnofHope = true;
        public int UseHymnofHopeAtPercentage = 40;
        public bool UseInnerFire = true;
        public bool UseInnerWill = false;
        public bool UseLevitate = false;
        public bool UseLifeblood = true;
        public bool UseLightWell = true;
        public int UseLightWellAtPercentage = 95;
        public bool UseMindSear = true;
        public bool UsePowerInfusion = true;
        public bool UsePowerWordFortitude = true;
        public bool UsePowerWordShield = true;
        public int UsePowerWordShieldAtPercentage = 100;
        public bool UsePowerWordSolace = true;
        public bool UsePrayerofHealing = false;
        public int UsePrayerofHealingAtPercentage = 50;
        public bool UsePrayerofMending = true;
        public int UsePrayerofMendingAtPercentage = 50;
        public bool UsePsychicScream = true;
        public int UsePsychicScreamAtPercentage = 20;
        public bool UsePsyfiend = true;
        public int UsePsyfiendAtPercentage = 35;
        public bool UseRenew = true;
        public int UseRenewAtPercentage = 90;
        public bool UseShadowWordDeath = true;
        public bool UseShadowWordPain = true;
        public bool UseShadowfiend = true;
        public bool UseSmite = true;
        public bool UseSpectralGuise = true;
        public int UseSpectralGuiseAtPercentage = 70;
        public bool UseStoneform = true;
        public int UseStoneformAtPercentage = 80;
        public bool UseTrinket = true;
        public bool UseVoidTendrils = true;
        public int UseVoidTendrilsAtPercentage = 35;
        public bool UseWarStomp = true;
        public int UseWarStompAtPercentage = 80;

        public PriestHolySettings()
        {
            ConfigWinForm("Holy Priest Settings");
            /* Professions and Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions and Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions and Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions and Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions and Racials", "AtPercentage");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions and Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions and Racials", "AtPercentage");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions and Racials", "AtPercentage");
            /* Priest Buffs */
            AddControlInWinForm("Use Chakra: Chastise", "UseChakraChastise", "Priest Buffs");
            AddControlInWinForm("Use Chakra: Sanctuary", "UseChakraSanctuary", "Priest Buffs");
            AddControlInWinForm("Use Chakra: Serenity", "UseChakraSerenity", "Priest Buffs");
            AddControlInWinForm("Use Inner Fire", "UseInnerFire", "Priest Buffs");
            AddControlInWinForm("Use Inner Will", "UseInnerWill", "Priest Buffs");
            AddControlInWinForm("Use Levitate", "UseLevitate", "Priest Buffs");
            AddControlInWinForm("Use Power Word: Fortitude", "UsePowerWordFortitude", "Priest Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Cascade", "UseCascade", "Offensive Spell");
            AddControlInWinForm("Use Divine Star", "Use Divine Star", "Offensive Spell");
            AddControlInWinForm("Use Halo", "UseHalo", "Offensive Spell");
            AddControlInWinForm("Use Holy Fire", "UseHolyFire", "Offensive Spell");
            AddControlInWinForm("Use Holy Word: Chastise", "UseHolyWordChastise", "Offensive Spell");
            AddControlInWinForm("Use Mind Sear", "UseMindSear", "Offensive Spell");
            AddControlInWinForm("Use Shadow Word: Death", "UseShadowWordDeath", "Offensive Spell");
            AddControlInWinForm("Use Shadow Word: Pain", "UseShadowWordPain", "Offensive Spell");
            AddControlInWinForm("Use Smite", "UseSmite", "Offensive Spell");
            /* Healing Cooldown */
            AddControlInWinForm("Use Divine Hymn", "UseDivineHymn", "Healing Cooldown", "AtPercentage");
            AddControlInWinForm("Use Light Well", "UseLightWell", "Healing Cooldown", "AtPercentage");
            AddControlInWinForm("Use Power Infusion", "UsePowerInfusion", "Healing Cooldown");
            AddControlInWinForm("Use Shadowfiend", "UseShadowfiend", "Healing Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Guardian Spirit", "UseGuardianSpirit", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Power Word: Shield", "UsePowerWordShield", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Psychic Scream", "UsePsychicScream", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Psyfiend", "UsePsyfiend", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Spectral Guise", "UseSpectralGuise", "Defensive Cooldown", "AtPercentage");
            AddControlInWinForm("Use Void Tendrils", "UseVoidTendrils", "Defensive Cooldown", "AtPercentage");
            /* Healing Spell */
            AddControlInWinForm("Use Circle of Healing", "UseCircleofHealing", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Desperate Prayer", "UseDesperatePrayer", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Flash Heal for Regeneration after combat", "UseFlashHealNonCombat", "Healing Spell",
                "AtPercentage");
            AddControlInWinForm("Use Flash Heal during combat", "UseFlashHealInCombat", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Greater Heal", "UseGreaterHeal", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Heal", "UseHeal", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Hymn of Hope", "UseHymnofHope", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Prayer of Mending", "UsePrayerofMending", "Healing Spell", "AtPercentage");
            AddControlInWinForm("Use Renew", "UseRenew", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Use Glyph of Lightspring", "UseGlyphofLightspring", "Game Settings");
        }

        public static PriestHolySettings CurrentSetting { get; set; }

        public static PriestHolySettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\HealerClasses\\Settings\\Priest_Holy.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<PriestHolySettings>(currentSettingsFile);
            }
            return new PriestHolySettings();
        }
    }

    #endregion
}

#endregion

#region Monk

public class MonkMistweaver
{
    private readonly MonkMistweaverSettings _mySettings = MonkMistweaverSettings.GetSettings();

    #region General Timers & Variables

    /*
        private Timer _serpentsZealTimer = new Timer(0);
*/
    private Timer _alchFlaskTimer = new Timer(0);
    private Timer _engineeringTimer = new Timer(0);
    private Timer _grappleWeaponTimer = new Timer(0);
    private Timer _healingSphereTimer = new Timer(0);
    private Timer _onCd = new Timer(0);
    private Timer _trinketTimer = new Timer(0);

    #endregion

    #region Professions & Racials

    public readonly Spell Alchemy = new Spell("Alchemy");
    public readonly Spell ArcaneTorrent = new Spell("Arcane Torrent");
    public readonly Spell Berserking = new Spell("Berserking");
    public readonly Spell BloodFury = new Spell("Blood Fury");
    public readonly Spell Engineering = new Spell("Engineering");
    public readonly Spell GiftoftheNaaru = new Spell("Gift of the Naaru");
    public readonly Spell Lifeblood = new Spell("Lifeblood");
    public readonly Spell Stoneform = new Spell("Stoneform");
    public readonly Spell WarStomp = new Spell("War Stomp");

    #endregion

    #region Monk Buffs

    public readonly Spell Disable = new Spell("Disable");
    public readonly Spell LegacyoftheEmperor = new Spell("Legacy of the Emperor");
    public readonly Spell StanceoftheFierceTiger = new Spell("Stance of the Fierce Tiger");
    public readonly Spell StanceoftheWiseSerpent = new Spell("Stance of the Wise Serpent");
    public readonly Spell SummonJadeSerpentStatue = new Spell("Summon Jade Serpent Statue");
    public readonly Spell TigersLust = new Spell("Tiger's Lust");

    #endregion

    #region Offensive Spell

    public readonly Spell BlackoutKick = new Spell("Blackout Kick");
    public readonly Spell CracklingJadeLightning = new Spell("Crackling Jade Lightning");
    public readonly Spell Jab = new Spell("Jab");
    public readonly Spell PathofBlossoms = new Spell("Path of Blossoms");
    public readonly Spell Provoke = new Spell("Provoke");
    public readonly Spell Roll = new Spell("Roll");
    public readonly Spell SpinningCraneKick = new Spell("Spinning Crane Kick");
    public readonly Spell TigerPalm = new Spell("Tiger Palm");
    public readonly Spell TouchofDeath = new Spell("Touch of Death");

    #endregion

    #region Healing Cooldown

    public readonly Spell ChiBrew = new Spell("Chi Brew");
    public readonly Spell InvokeXuentheWhiteTiger = new Spell("Invoke Xuen, the White Tiger");
    public readonly Spell RushingJadeWind = new Spell("Rushing Jade Wind");
    public readonly Spell ThunderFocusTea = new Spell("Thunder Focus Tea");

    #endregion

    #region Defensive Cooldown

    public readonly Spell ChargingOxWave = new Spell("Charging Ox Wave");
    public readonly Spell DampenHarm = new Spell("Dampen Harm");
    public readonly Spell DiffuseMagic = new Spell("Diffuse Magic");
    public readonly Spell FortifyingBrew = new Spell("Fortifying Brew");
    public readonly Spell GrappleWeapon = new Spell("Grapple Weapon");
    public readonly Spell LegSweep = new Spell("Leg Sweep");
    public readonly Spell LifeCocoon = new Spell("Life Cocoon");
    public readonly Spell SpearHandStrike = new Spell("Spear Hand Strike");
    public readonly Spell ZenMeditation = new Spell("Zen Meditation");

    #endregion

    #region Healing Spell

    public readonly Spell ChiBurst = new Spell("Chi Burst");
    public readonly Spell ChiWave = new Spell("Chi Wave");
    public readonly Spell EnvelopingMist = new Spell("Enveloping Mist");
    public readonly Spell ExpelHarm = new Spell("Expel Harm");
    public readonly Spell HealingSphere = new Spell("Healing Sphere");
    public readonly Spell ManaTea = new Spell("Mana Tea");
    public readonly Spell RenewingMist = new Spell("Renewing Mist");
    public readonly Spell Revival = new Spell("Revival");
    public readonly Spell SoothingMist = new Spell("Soothing Mist");
    public readonly Spell SurgingMist = new Spell("Surging Mist");
    public readonly Spell Uplift = new Spell("Uplift");
    public readonly Spell ZenSphere = new Spell("Zen Sphere");

    #endregion

    public MonkMistweaver()
    {
        Main.InternalRange = 30f;

        while (Main.InternalLoop)
        {
            try
            {
                if (!ObjectManager.Me.IsDead && !Usefuls.IsLoadingOrConnecting && Usefuls.InGame)
                {
                    if (ObjectManager.Me.Target > 0)
                    {
                        if (UnitRelation.GetReaction(ObjectManager.Target.Faction) != Reaction.Friendly)
                            ObjectManager.Me.Target = ObjectManager.Me.Guid;
                    }
                    else
                        ObjectManager.Me.Target = ObjectManager.Me.Guid;
                    if (!ObjectManager.Me.IsMounted)
                    {
                        if (Heal.IsHealing)
                        {
                            if (ObjectManager.Me.HealthPercent < 100 && !Party.IsInGroup())
                            {
                                if (ObjectManager.Me.Target != ObjectManager.Me.Guid)
                                    ObjectManager.Me.Target = ObjectManager.Me.Guid;
                            }
                            else if (Party.IsInGroup())
                            {
                                double lowestHp = 100;
                                var lowestHpPlayer = new WoWUnit(0);
                                foreach (Int128 playerInMyParty in Party.GetPartyPlayersGUID())
                                {
                                    if (playerInMyParty <= 0) continue;
                                    WoWObject obj = ObjectManager.GetObjectByGuid(playerInMyParty);
                                    if (!obj.IsValid || obj.Type != WoWObjectType.Player)
                                        continue;
                                    var currentPlayer = new WoWPlayer(obj.GetBaseAddress);
                                    if (!currentPlayer.IsValid || !currentPlayer.IsAlive) continue;

                                    if (currentPlayer.HealthPercent < 100 && currentPlayer.HealthPercent < lowestHp && HealerClass.InRange(currentPlayer))
                                    {
                                        lowestHp = currentPlayer.HealthPercent;
                                        lowestHpPlayer = currentPlayer;
                                    }
                                }
                                if (lowestHpPlayer.Guid > 0)
                                {
                                    if (ObjectManager.Me.HealthPercent < 50 &&
                                        ObjectManager.Me.HealthPercent - 10 < lowestHp)
                                    {
                                        lowestHpPlayer = ObjectManager.Me;
                                    }
                                    if (ObjectManager.Me.Target != lowestHpPlayer.Guid && lowestHpPlayer.IsAlive && HealerClass.InRange(lowestHpPlayer))
                                    {
                                        Logging.Write("Switching to target " + lowestHpPlayer.Name + ".");
                                        ObjectManager.Me.Target = lowestHpPlayer.Guid;
                                    }
                                }
                                else if (ObjectManager.Me.Target != ObjectManager.Me.Guid)
                                    ObjectManager.Me.Target = lowestHpPlayer.Guid;
                            }
                            else
                            {
                                Heal.IsHealing = false;
                                break;
                            }
                            if (HealerClass.InRange(ObjectManager.Target))
                                MountTask.DismountMount(false);
                            HealingFight();
                        }
                        else if (!ObjectManager.Me.IsCast)
                        {
                            Patrolling();
                        }
                    }
                }
                Thread.Sleep(500);
            }
            catch
            {
            }
        }
    }

    private void HealingFight()
    {
        Buff();
        if (_onCd.IsReady)
            DefenseCycle();
        Decast();
        HealingBurst();
        HealCycle();
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (LegacyoftheEmperor.KnownSpell && LegacyoftheEmperor.IsSpellUsable &&
            !LegacyoftheEmperor.HaveBuff && _mySettings.UseLegacyoftheEmperor)
        {
            LegacyoftheEmperor.Launch();
            return;
        }
        if (StanceoftheWiseSerpent.KnownSpell && StanceoftheWiseSerpent.IsSpellUsable && !StanceoftheWiseSerpent.HaveBuff
            && _mySettings.UseStanceoftheWiseSerpent)
        {
            StanceoftheWiseSerpent.Launch();
            return;
        }
        if (ObjectManager.GetNumberAttackPlayer() == 0 && TigersLust.IsSpellUsable && TigersLust.KnownSpell
            && _mySettings.UseTigersLust && ObjectManager.Me.GetMove)
        {
            TigersLust.Launch();
            return;
        }
        if (ObjectManager.GetNumberAttackPlayer() == 0 && Roll.IsSpellUsable && Roll.KnownSpell
            && _mySettings.UseRoll && ObjectManager.Me.GetMove && !TigersLust.HaveBuff
            && ObjectManager.Target.GetDistance > 14)
        {
            Roll.Launch();
            return;
        }
        if (ObjectManager.GetNumberAttackPlayer() > 0 && SummonJadeSerpentStatue.IsSpellUsable && SummonJadeSerpentStatue.KnownSpell
            && _mySettings.UseSummonJadeSerpentStatue && !SummonJadeSerpentStatue.HaveBuff && ObjectManager.Target.GetDistance < 40)
        {
            SummonJadeSerpentStatue.Launch();
            return;
        }
        if (_alchFlaskTimer.IsReady && _mySettings.UseAlchFlask
            && ItemsManager.GetItemCount(75525) == 1)
        {
            Logging.WriteFight("Use Alchi Flask");
            Lua.RunMacroText("/use item:75525");
            _alchFlaskTimer = new Timer(1000*60*60*2);
        }
    }

    private void DefenseCycle()
    {
        if (ObjectManager.Me.HealthPercent < 95 && _mySettings.UseGrappleWeapon && GrappleWeapon.IsHostileDistanceGood
            && GrappleWeapon.KnownSpell && GrappleWeapon.IsSpellUsable && _grappleWeaponTimer.IsReady)
        {
            GrappleWeapon.Launch();
            _grappleWeaponTimer = new Timer(1000*60);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 80 && FortifyingBrew.IsSpellUsable && FortifyingBrew.KnownSpell
            && _mySettings.UseFortifyingBrew)
        {
            FortifyingBrew.Launch();
            _onCd = new Timer(1000*20);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 80 && LifeCocoon.IsSpellUsable && LifeCocoon.KnownSpell
            && _mySettings.UseLifeCocoon)
        {
            LifeCocoon.Launch();
            _onCd = new Timer(1000*12);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 90 && ChargingOxWave.IsSpellUsable && ChargingOxWave.KnownSpell
            && _mySettings.UseChargingOxWave && ChargingOxWave.IsHostileDistanceGood)
        {
            ChargingOxWave.Launch();
            _onCd = new Timer(1000*3);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 90 && DampenHarm.IsSpellUsable && DampenHarm.KnownSpell
            && _mySettings.UseDampenHarm)
        {
            DampenHarm.Launch();
            _onCd = new Timer(1000*5);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 90 && LegSweep.IsSpellUsable && LegSweep.KnownSpell
            && _mySettings.UseLegSweep && ObjectManager.Target.GetDistance < 6)
        {
            LegSweep.Launch();
            _onCd = new Timer(1000*5);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 80 && ZenMeditation.IsSpellUsable && ZenMeditation.KnownSpell
            && _mySettings.UseZenMeditation)
        {
            ZenMeditation.Launch();
            _onCd = new Timer(1000*8);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell
            && _mySettings.UseStoneform)
        {
            Stoneform.Launch();
            _onCd = new Timer(1000*8);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 80 && WarStomp.IsSpellUsable && WarStomp.KnownSpell
            && _mySettings.UseWarStomp)
        {
            WarStomp.Launch();
            _onCd = new Timer(1000*2);
        }
    }

    private void HealCycle()
    {
        if (ObjectManager.Me.ManaPercentage < 50 && ManaTea.KnownSpell && ManaTea.IsSpellUsable
            && _mySettings.UseManaTea && ObjectManager.Me.BuffStack(115867) > 4
            && ObjectManager.GetNumberAttackPlayer() == 0)
        {
            ManaTea.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 95 && SurgingMist.KnownSpell && SurgingMist.IsSpellUsable
            && _mySettings.UseSurgingMist && ObjectManager.Me.BuffStack(118674) > 4
            && ObjectManager.GetNumberAttackPlayer() == 0)
        {
            SurgingMist.Launch();
            return;
        }
        if (HealingSphere.KnownSpell && HealingSphere.IsSpellUsable && ObjectManager.Me.Energy > 39 &&
            ObjectManager.Me.HealthPercent < 60 && _mySettings.UseHealingSphere && _healingSphereTimer.IsReady)
        {
            SpellManager.CastSpellByIDAndPosition(115460, ObjectManager.Me.Position);
            _healingSphereTimer = new Timer(1000*5);
            return;
        }
        if (ObjectManager.Me.HealthPercent < 70 && SurgingMist.KnownSpell && SurgingMist.IsSpellUsable
            && _mySettings.UseSurgingMist)
        {
            SurgingMist.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 85 && Uplift.KnownSpell && Uplift.IsSpellUsable
            && _mySettings.UseUplift && RenewingMist.HaveBuff)
        {
            Uplift.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 85 && ChiWave.KnownSpell && ChiWave.IsSpellUsable
            && _mySettings.UseChiWave)
        {
            ChiWave.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 90 && ChiBurst.KnownSpell && ChiBurst.IsSpellUsable
            && _mySettings.UseChiBurst)
        {
            ChiBurst.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 90 && ExpelHarm.KnownSpell && ExpelHarm.IsSpellUsable
            && _mySettings.UseExpelHarm && ExpelHarm.IsHostileDistanceGood)
        {
            ExpelHarm.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 90 && EnvelopingMist.KnownSpell && EnvelopingMist.IsSpellUsable
            && _mySettings.UseEnvelopingMist && !EnvelopingMist.HaveBuff)
        {
            EnvelopingMist.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 95 && SurgingMist.KnownSpell && SurgingMist.IsSpellUsable
            && _mySettings.UseSurgingMist && ObjectManager.Me.BuffStack(118674) > 4)
        {
            SurgingMist.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 95 && SoothingMist.KnownSpell && SoothingMist.IsSpellUsable
            && _mySettings.UseSoothingMist && !SoothingMist.HaveBuff && !ObjectManager.Me.IsCast)
        {
            SoothingMist.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 95 && RenewingMist.KnownSpell && RenewingMist.IsSpellUsable
            && _mySettings.UseRenewingMist && !RenewingMist.HaveBuff)
        {
            RenewingMist.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 95 && ZenSphere.KnownSpell && ZenSphere.IsSpellUsable
            && _mySettings.UseZenSphere)
        {
            ZenSphere.Launch();
        }
    }

    private void Decast()
    {
        if (ArcaneTorrent.KnownSpell && _mySettings.UseArcaneTorrent && ArcaneTorrent.IsSpellUsable
            && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ObjectManager.Target.GetDistance < 8)
        {
            ArcaneTorrent.Launch();
            return;
        }
        if (DiffuseMagic.KnownSpell && _mySettings.UseDiffuseMagic && DiffuseMagic.IsSpellUsable
            && ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe)
        {
            DiffuseMagic.Launch();
            return;
        }
        if (SpearHandStrike.KnownSpell && _mySettings.UseSpearHandStrike && ObjectManager.Target.IsCast
            && SpearHandStrike.IsSpellUsable && SpearHandStrike.IsHostileDistanceGood)
        {
            SpearHandStrike.Launch();
            return;
        }
        if (ObjectManager.Target.GetMove && !Disable.TargetHaveBuff && _mySettings.UseDisable
            && Disable.KnownSpell && Disable.IsSpellUsable && Disable.IsHostileDistanceGood)
        {
            Disable.Launch();
        }
    }

    private void HealingBurst()
    {
        if (_mySettings.UseTrinket && _trinketTimer.IsReady && ObjectManager.Target.GetDistance < 30)
        {
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            _trinketTimer = new Timer(1000*60*2);
            return;
        }
        if (Berserking.IsSpellUsable && Berserking.KnownSpell && ObjectManager.Target.GetDistance < 30
            && _mySettings.UseBerserking)
        {
            Berserking.Launch();
            return;
        }
        if (BloodFury.IsSpellUsable && BloodFury.KnownSpell && ObjectManager.Target.GetDistance < 30
            && _mySettings.UseBloodFury)
        {
            BloodFury.Launch();
            return;
        }
        if (Lifeblood.IsSpellUsable && Lifeblood.KnownSpell && ObjectManager.Target.GetDistance < 30
            && _mySettings.UseLifeblood)
        {
            Lifeblood.Launch();
            return;
        }
        if (_engineeringTimer.IsReady && Engineering.KnownSpell && ObjectManager.Target.GetDistance < 30
            && _mySettings.UseEngGlove)
        {
            Logging.WriteFight("Use Engineering Gloves.");
            Lua.RunMacroText("/use 10");
            _engineeringTimer = new Timer(1000*60);
            return;
        }
        if (ChiBrew.IsSpellUsable && ChiBrew.KnownSpell
            && _mySettings.UseChiBrew && ObjectManager.Me.Chi == 0)
        {
            ChiBrew.Launch();
            return;
        }
        if (TouchofDeath.IsSpellUsable && TouchofDeath.KnownSpell && TouchofDeath.IsHostileDistanceGood
            && _mySettings.UseTouchofDeath)
        {
            TouchofDeath.Launch();
            return;
        }
        if (InvokeXuentheWhiteTiger.IsSpellUsable && InvokeXuentheWhiteTiger.KnownSpell
            && _mySettings.UseInvokeXuentheWhiteTiger && InvokeXuentheWhiteTiger.IsHostileDistanceGood)
        {
            InvokeXuentheWhiteTiger.Launch();
            return;
        }
        if (ThunderFocusTea.IsSpellUsable && ThunderFocusTea.KnownSpell
            && _mySettings.UseThunderFocusTea && ObjectManager.Me.HealthPercent < 90)
        {
            ThunderFocusTea.Launch();
            return;
        }
        if (ObjectManager.Me.HealthPercent < 80 && Revival.KnownSpell && Revival.IsSpellUsable
            && _mySettings.UseRevival)
        {
            Revival.Launch();
            return;
        }
        if (RushingJadeWind.IsSpellUsable && RushingJadeWind.KnownSpell && RushingJadeWind.IsHostileDistanceGood
            && _mySettings.UseRushingJadeWind && ObjectManager.GetNumberAttackPlayer() > 3)
        {
            RushingJadeWind.Launch();
        }
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Buff();
        }
    }

    #region Nested type: MonkMistweaverSettings

    [Serializable]
    public class MonkMistweaverSettings : Settings
    {
        public bool UseAlchFlask = true;
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBlackoutKick = true;
        public bool UseBloodFury = true;
        public bool UseChargingOxWave = true;
        public bool UseChiBrew = true;
        public bool UseChiBurst = true;
        public bool UseChiWave = true;
        public bool UseCracklingJadeLightning = true;
        public bool UseDampenHarm = true;
        public bool UseDiffuseMagic = true;
        public bool UseDisable = false;
        public bool UseEngGlove = true;
        public bool UseEnvelopingMist = true;
        public bool UseExpelHarm = true;
        public bool UseFortifyingBrew = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseGrappleWeapon = true;
        public bool UseHealingSphere = true;
        public bool UseInvokeXuentheWhiteTiger = true;
        public bool UseJab = true;
        public bool UseLegSweep = true;
        public bool UseLegacyoftheEmperor = true;
        public bool UseLifeCocoon = true;
        public bool UseLifeblood = true;
        public bool UseManaTea = true;
        public bool UsePathofBlossoms = true;
        public bool UseProvoke = true;
        public bool UseRenewingMist = true;
        public bool UseRevival = true;
        public bool UseRoll = true;
        public bool UseRushingJadeWind = true;
        public bool UseSoothingMist = false;
        public bool UseSpearHandStrike = true;
        public bool UseSpinningCraneKick = true;
        public bool UseStanceoftheFierceTiger = true;
        public bool UseStanceoftheWiseSerpent = true;
        public bool UseStoneform = true;
        public bool UseSummonJadeSerpentStatue = true;
        public bool UseSurgingMist = true;
        public bool UseThunderFocusTea = true;
        public bool UseTigerPalm = true;
        public bool UseTigersLust = true;
        public bool UseTouchofDeath = true;
        public bool UseTrinket = true;
        public bool UseUplift = true;
        public bool UseWarStomp = true;
        public bool UseZenMeditation = true;
        public bool UseZenSphere = true;

        public MonkMistweaverSettings()
        {
            ConfigWinForm("Mistweaver Monk Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Monk Buffs */
            AddControlInWinForm("Use Disable", "UseDisable", "Monk Buffs");
            AddControlInWinForm("Use Legacy of the Emperor", "UseLegacyoftheEmperor", "Monk Buffs");
            AddControlInWinForm("Use Stance of the Fierce Tiger", "UseStanceoftheFierceTiger", "Monk Buffs");
            AddControlInWinForm("Use Summon Jade Serpent Statue", "UseSummonJadeSerpentStatue", "Monk Buffs");
            AddControlInWinForm("Use Tiger's Lust", "UseTigersLust", "Monk Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Chi Wave", "UseChiWave", "Offensive Spell");
            AddControlInWinForm("Use Blackout Kick", "UseBlackoutKick", "Offensive Spell");
            AddControlInWinForm("Use Crackling Jade Lightning", "UseCracklingJadeLightning", "Offensive Spell");
            AddControlInWinForm("Use Jab", "UseJab", "Offensive Spell");
            AddControlInWinForm("Use Path of Blossoms", "UsePathofBlossoms", "Offensive Spell");
            AddControlInWinForm("Use Provoke", "UseProvoke", "Offensive Spell");
            AddControlInWinForm("Use Roll", "UseRoll", "Offensive Spell");
            AddControlInWinForm("Use Spinning Crane Kick", "UseSpinningCraneKick", "Offensive Spell");
            AddControlInWinForm("Use Tiger Palm", "UseTigerPalm", "Offensive Spell");
            AddControlInWinForm("Use Touch of Death", "UseTouchofDeath", "Offensive Spell");
            /* Healing Cooldown */
            AddControlInWinForm("Use Chi Brew", "UseChiBrew", "Healing Cooldown");
            AddControlInWinForm("Use Invoke Xuen, the White Tiger", "UseInvokeXuentheWhiteTiger", "Healing Cooldown");
            AddControlInWinForm("Use Revival", "UseRevival", "Healing Cooldown");
            AddControlInWinForm("Use Rushing Jade Wind", "UseRushingJadeWind", "Healing Cooldown");
            AddControlInWinForm("Use Thunder Focus Tea", "UseThunderFocusTea", "Healing Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Charging Ox Wave", "UseChargingOxWave", "Defensive Cooldown");
            AddControlInWinForm("Use Dampen Harm ", "UseDampenHarm ", "Defensive Cooldown");
            AddControlInWinForm("Use Diffuse Magic", "UseDiffuseMagic", "Defensive Cooldown");
            AddControlInWinForm("Use Fortifying Brew", "UseFortifyingBrew", "Defensive Cooldown");
            AddControlInWinForm("Use Grapple Weapon", "UseGrappleWeapon", "Defensive Cooldown");
            AddControlInWinForm("Use Leg Sweep", "UseLegSweep", "Defensive Cooldown");
            AddControlInWinForm("Use Life Cocoon", "UseLifeCocoon", "Defensive Cooldown");
            AddControlInWinForm("Use Spear Hand Strike", "UseSpearHandStrike", "Defensive Cooldown");
            AddControlInWinForm("Use Zen Meditation", "UseZenMeditation", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Chi Burst", "UseChiBurst", "Healing Spell");
            AddControlInWinForm("Use Enveloping Mist", "UseEnvelopingMist", "Healing Spell");
            AddControlInWinForm("Use Expel Harm", "UseExpelHarm", "Healing Spell");
            AddControlInWinForm("Use Healing Sphere", "UseHealingSphere", "Healing Spell");
            AddControlInWinForm("Use Mana Tea", "UseManaTea", "Healing Spell");
            AddControlInWinForm("Use Renewing Mist", "UseRenewingMist", "Healing Spell");
            AddControlInWinForm("Use Soothing Mist", "UseSoothingMist", "Healing Spell");
            AddControlInWinForm("Use Surging Mist", "UseSurgingMist", "Healing Spell");
            AddControlInWinForm("Use Uplift", "UseUplift", "Healing Spell");
            AddControlInWinForm("Use Zen Sphere", "UseZenSphere", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static MonkMistweaverSettings CurrentSetting { get; set; }

        public static MonkMistweaverSettings GetSettings()
        {
            string currentSettingsFile = Application.StartupPath + "\\HealerClasses\\Settings\\Monk_Mistweaver.xml";
            if (File.Exists(currentSettingsFile))
            {
                return
                    CurrentSetting = Load<MonkMistweaverSettings>(currentSettingsFile);
            }
            return new MonkMistweaverSettings();
        }
    }

    #endregion
}

#endregion

// ReSharper restore ObjectCreationAsStatement