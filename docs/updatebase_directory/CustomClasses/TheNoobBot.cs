/*
* CustomClass for TheNoobBot
* Credit : Rival, Geesus, Enelya, Marstor, Vesper, Neo2003, Dreadlocks
* Thanks you !
*/

using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using Keybindings = nManager.Wow.Enums.Keybindings;
using Timer = nManager.Helpful.Timer;

public class Main : ICustomClass
{
    internal static float range = 5.0f;
    internal static bool loop = true;

    public float Range
    {
        get { return range; }
        set { range = value; }
    }

    public void Initialize()
    {
        Initialize(false);
    }

    public void Initialize(bool ConfigOnly)
    {
        try
        {
            Logging.WriteFight("Loading combat system.");

            switch (ObjectManager.Me.WowClass)
            {
                    #region DeathKnight Specialisation checking

                case WoWClass.DeathKnight:
                    var Blood_Rites = new Spell("Blood Rites");
                    var Reaping = new Spell("Reaping");
                    var Howling_Blast = new Spell("Howling Blast");

                    if (Blood_Rites.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Deathknight_Blood.xml";
                            Deathknight_Blood.DeathknightBloodSettings CurrentSetting;
                            CurrentSetting = new Deathknight_Blood.DeathknightBloodSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Deathknight_Blood.DeathknightBloodSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Deathknight Blood class...");
                            new Deathknight_Blood();
                        }
                    }
                    else if (Reaping.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Deathknight_Unholy.xml";
                            Deathknight_Unholy.DeathknightUnholySettings CurrentSetting;
                            CurrentSetting = new Deathknight_Unholy.DeathknightUnholySettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Deathknight_Unholy.DeathknightUnholySettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Deathknight Unholy class...");
                            new Deathknight_Unholy();
                        }
                    }
                    else if (Howling_Blast.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Deathknight_Frost.xml";
                            Deathknight_Frost.DeathknightFrostSettings CurrentSetting;
                            CurrentSetting = new Deathknight_Frost.DeathknightFrostSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Deathknight_Frost.DeathknightFrostSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Deathknight Frost class...");
                            new Deathknight_Frost();
                        }
                    }
                    else
                    {
                        if (ConfigOnly)
                        {
                            System.Windows.Forms.MessageBox.Show(
                                "Your specification haven't be found, loading Deathknight Apprentice Settings");
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Deathknight_Apprentice.xml";
                            Deathknight_Apprentice.DeathknightApprenticeSettings CurrentSetting;
                            CurrentSetting = new Deathknight_Apprentice.DeathknightApprenticeSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Deathknight_Apprentice.DeathknightApprenticeSettings>(
                                        CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("No specialisation detected.");
                            Logging.WriteFight("Loading Deathknight Apprentice class...");
                            new Deathknight_Apprentice();
                        }
                    }
                    break;

                    #endregion

                    #region Mage Specialisation checking

                case WoWClass.Mage:
                    var Summon_Water_Elemental = new Spell("Summon Water Elemental");
                    var Arcane_Barrage = new Spell("Arcane Barrage");
                    var Pyroblast = new Spell("Pyroblast");
                    if (Summon_Water_Elemental.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Loading Mage Frost class...");
                            range = 30.0f;
                            new Mage_Frost();
                        }
                    }
                    if (Arcane_Barrage.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Loading Arcane Mage class...");
                            range = 30.0f;
                            new Mage_Arcane();
                        }
                    }
                    if (Pyroblast.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Loading Mage Fire class...");
                            range = 30.0f;
                            new Mage_Fire();
                        }
                    }
                    if (!Summon_Water_Elemental.KnownSpell && !Arcane_Barrage.KnownSpell && !Pyroblast.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("No specialisation detected.");
                            Logging.WriteFight("Loading Mage frost class...");
                            range = 30.0f;
                            new Mage_Frost();
                        }
                    }
                    break;

                    #endregion

                    #region Warlock Specialisation checking

                case WoWClass.Warlock:
                    var Summon_Felguard = new Spell("Summon Felguard");
                    var Unstable_Affliction = new Spell("Unstable Affliction");
                    if (Unstable_Affliction.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Loading Affliction Warlock class...");
                            range = 30.0f;
                            new Affli();
                        }
                    }
                    if (Summon_Felguard.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Loading Demonology Warlock class...");
                            range = 30.0f;
                            new Demo();
                        }
                    }
                    if (!Unstable_Affliction.KnownSpell && !Summon_Felguard.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("No specialisation detected.");
                            Logging.WriteFight("Loading Demonology Warlock class...");
                            range = 30.0f;
                            new Demo();
                        }
                    }
                    break;

                    #endregion

                    #region Druid Specialisation checking

                case WoWClass.Druid:
                    var SavageRoar = new Spell("Savage Roar");
                    if (SavageRoar.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Feral Druid Found");
                            new DruidFeral();
                        }
                        break;
                    }
                    var Starsurge = new Spell("Starsurge");
                    if (Starsurge.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Balance Druid Found");
                            range = 30.0f;
                            new Balance();
                        }
                        break;
                    }
                    if (!Starsurge.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Druid without Spec");
                            range = 30.0f;
                            new Balance();
                        }
                        break;
                    }
                    break;

                    #endregion

                    #region Paladin Specialisation checking

                case WoWClass.Paladin:
                    var Paladin_Retribution_Spell = new Spell("Templar's Verdict");
                    var Paladin_Protection_Spell = new Spell("Avenger's Shield");
                    var Paladin_Holy_Spell = new Spell("Holy Shock");
                    if (Paladin_Retribution_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Paladin_Retribution.xml";
                            Paladin_Retribution.PaladinRetributionSettings CurrentSetting;
                            CurrentSetting = new Paladin_Retribution.PaladinRetributionSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Paladin_Retribution.PaladinRetributionSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Paladin Retribution class...");
                            new Paladin_Retribution();
                        }
                        break;
                    }
                    else if (Paladin_Protection_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Paladin_Protection.xml";
                            Paladin_Protection.PaladinProtectionSettings CurrentSetting;
                            CurrentSetting = new Paladin_Protection.PaladinProtectionSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Paladin_Protection.PaladinProtectionSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Paladin Protection class...");
                            new Paladin_Protection();
                        }
                        break;
                    }
                    else if (Paladin_Holy_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Paladin_Holy.xml";
                            Paladin_Holy.PaladinHolySettings CurrentSetting;
                            CurrentSetting = new Paladin_Holy.PaladinHolySettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting = Settings.Load<Paladin_Holy.PaladinHolySettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Paladin Holy class...");
                            range = 30.0f;
                            new Paladin_Holy();
                        }
                        break;
                    }
                    else
                    {
                        if (ConfigOnly)
                        {
                            System.Windows.Forms.MessageBox.Show(
                                "Your specification haven't be found, loading Paladin Retribution Settings");
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Paladin_Retribution.xml";
                            Paladin_Retribution.PaladinRetributionSettings CurrentSetting;
                            CurrentSetting = new Paladin_Retribution.PaladinRetributionSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Paladin_Retribution.PaladinRetributionSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("No specialisation detected.");
                            Logging.WriteFight("Loading Paladin Retribution class...");
                            new Paladin_Retribution();
                        }
                        break;
                    }

                    #endregion

                    #region Shaman Specialisation checking

                case WoWClass.Shaman:
                    var Thunderstorm = new Spell("Thunderstorm");
                    if (Thunderstorm.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Ele Shaman Found");
                            range = 30.0f;
                            new Ele();
                        }
                    }
                    if (!Thunderstorm.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Shaman without Spec");
                            range = 30.0f;
                            new Ele();
                        }
                    }
                    break;

                    #endregion

                    #region Priest Specialisation checking

                case WoWClass.Priest:
                    var Priest_Shadow_Spell = new Spell("Mind Flay");
                    var Priest_Discipline_Spell = new Spell("Penance");
                    var Priest_Holy_Spell = new Spell("Holy Word: Chastise");
                    if (Priest_Shadow_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Priest_Shadow.xml";
                            Priest_Shadow.PriestShadowSettings CurrentSetting;
                            CurrentSetting = new Priest_Shadow.PriestShadowSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Priest_Shadow.PriestShadowSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Priest Shadow class...");
                            range = 30.0f;
                            new Priest_Shadow();
                        }
                        break;
                    }
                    else if (Priest_Discipline_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            System.Windows.Forms.MessageBox.Show(
                                "Priest Discipline found, but no Discipline class available, loading Priest Shadow Settings");
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Priest_Shadow.xml";
                            Priest_Shadow.PriestShadowSettings CurrentSetting;
                            CurrentSetting = new Priest_Shadow.PriestShadowSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Priest_Shadow.PriestShadowSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Priest Discipline found, but no Discipline class available...");
                            Logging.WriteFight("Loading Priest Shadow class...");
                            range = 30.0f;
                            new Priest_Shadow();
                        }
                        break;
                    }
                    else if (Priest_Holy_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            System.Windows.Forms.MessageBox.Show(
                                "Priest Holy found, but no Holy class available, loading Priest Shadow Settings");
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Priest_Shadow.xml";
                            Priest_Shadow.PriestShadowSettings CurrentSetting;
                            CurrentSetting = new Priest_Shadow.PriestShadowSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Priest_Shadow.PriestShadowSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Priest Holy found, but no Holy class available...");
                            Logging.WriteFight("Loading Priest Shadow class...");
                            range = 30.0f;
                            new Priest_Shadow();
                        }
                        break;
                    }
                    else
                    {
                        if (ConfigOnly)
                        {
                            System.Windows.Forms.MessageBox.Show(
                                "Your specification haven't be found, loading Priest Shadow Settings");
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Priest_Shadow.xml";
                            Priest_Shadow.PriestShadowSettings CurrentSetting;
                            CurrentSetting = new Priest_Shadow.PriestShadowSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Priest_Shadow.PriestShadowSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("No specialisation detected.");
                            Logging.WriteFight("Loading Priest Shadow class...");
                            new Priest_Shadow();
                        }
                        break;
                    }

                    #endregion

                    #region Rogue Specialisation checking

                case WoWClass.Rogue:
                    var Blade_Flurry = new Spell("Blade Flurry");
                    var Mutilate = new Spell("Mutilate");
                    if (Blade_Flurry.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Combat Rogue found");
                            new RogueCom();
                        }
                    }
                    if (Mutilate.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Assassination Rogue found");
                            new RogueAssa();
                        }
                    }
                    else
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Rogue without Spec");
                            new Rogue();
                        }
                    }
                    break;

                    #endregion

                    #region Warrior Specialisation checking

                case WoWClass.Warrior:
                    var Mortal_Strike = new Spell("Mortal Strike");
                    if (Mortal_Strike.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Arms Warrior Found");
                            new Arms();
                        }
                        break;
                    }
                    var Shield_Slam = new Spell("Shield Slam");
                    if (Shield_Slam.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Def Warrior Found");
                            new WarriorProt();
                        }
                        break;
                    }
                    var Bloodthirst = new Spell("Bloodthirst");
                    if (Bloodthirst.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Fury Warrior Found");
                            new WarriorFury();
                        }
                        break;
                    }
                    if (!Mortal_Strike.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Warrior without Spec");
                            new Warrior();
                        }
                        break;
                    }
                    break;

                    #endregion

                    #region Hunter Specialisation checking

                case WoWClass.Hunter:
                    var Explosive_Shot = new Spell("Explosive Shot");
                    var Aimed_Shot = new Spell("Aimed Shot");
                    var FocusFire = new Spell("Focus Fire");
                    if (Explosive_Shot.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Survival Hunter Found");
                            range = 30.0f;
                            new Survival();
                        }
                        break;
                    }
                    else if (Aimed_Shot.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show("There is no settings available for your Class/Specialisation.");
                        }
                        else
                        {
                            Logging.WriteFight("Marksman Hunter Found");
                            range = 30.0f;
                            new Marks();
                        }
                        break;
                    }
                    else if (FocusFire.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Hunter_BeastMaster.xml";
                            Hunter_BeastMaster.HunterBeastMasterSettings CurrentSetting;
                            CurrentSetting = new Hunter_BeastMaster.HunterBeastMasterSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Hunter_BeastMaster.HunterBeastMasterSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Beast Master Hunter Found");
                            range = 30.0f;
                            new Hunter_BeastMaster();
                        }
                        break;
                    }
                    else
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Hunter_BeastMaster.xml";
                            Hunter_BeastMaster.HunterBeastMasterSettings CurrentSetting;
                            CurrentSetting = new Hunter_BeastMaster.HunterBeastMasterSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Hunter_BeastMaster.HunterBeastMasterSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("No specialisation detected.");
                            Logging.WriteFight("Loading Hunter Beast Master class...");
                            range = 30.0f;
                            new Hunter_BeastMaster();
                        }
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
        Logging.WriteFight("Combat system stopped.");
    }

    public void Dispose()
    {
        Logging.WriteFight("Combat system stopped.");
        loop = false;
    }

    public void ShowConfiguration()
    {
        Directory.CreateDirectory(Application.StartupPath + "\\CustomClasses\\Settings\\");
        Initialize(true);
    }
}

#region Deathknight

public class Deathknight_Apprentice
{
    [Serializable]
    public class DeathknightApprenticeSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Deathknight Presence & Buffs */
        public bool UseFrostPresence = true;
        public bool UseBloodPresence = true;
        /* Offensive Spell */
        public bool UseBloodBoil = true;
        public bool UseBloodStrike = true;
        public bool UseDeathCoil = true;
        public bool UseIcyTouch = true;
        public bool UsePlagueStrike = true;
        /* Offensive Cooldown */
        public bool UseDeathGrip = true;
        public bool UsePestilence = true;
        public bool UseRaiseDead = true;
        /* Defensive Cooldown */
        public bool UseChainsofIce = true;
        public bool UseMindFreeze = true;
        /* Healing Spell */
        public bool UseDeathStrike = true;

        public DeathknightApprenticeSettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Deathknight Apprentice Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Deathknight Presence & Buffs */
            AddControlInWinForm("Use Frost Presence", "UseFrostPresence", "Deathknight Presence & Buffs");
            AddControlInWinForm("Use Blood Presence", "UseFrostPresence", "Deathknight Presence & Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Blood Boil", "UseBloodBoil", "Offensive Spell");
            AddControlInWinForm("Use Blood Strike", "UseBloodStrike", "Offensive Spell");
            AddControlInWinForm("Use Death Coil", "UseDeathCoil", "Offensive Spell");
            AddControlInWinForm("Use Icy Touch", "UseIcyTouch", "Offensive Spell");
            AddControlInWinForm("Use Plague Strike", "UsePlagueStrike", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Death Grip", "UseDeathGrip", "Offensive Cooldown");
            AddControlInWinForm("Use Pestilence", "UsePestilence", "Offensive Cooldown");
            AddControlInWinForm("Use Raise Dead", "UseRaiseDead", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Chains of Ice", "UseChainsofIce", "Defensive Cooldown");
            AddControlInWinForm("Use Mind Freeze", "UseMindFreeze", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Death Strike", "UseDeathStrike", "Healing Spell");
        }

        public static DeathknightApprenticeSettings CurrentSetting { get; set; }

        public static DeathknightApprenticeSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath +
                                         "\\CustomClasses\\Settings\\Deathknight_Apprentice.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting =
                    Settings.Load<Deathknight_Apprentice.DeathknightApprenticeSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Deathknight_Apprentice.DeathknightApprenticeSettings();
            }
        }
    }

    private readonly DeathknightApprenticeSettings MySettings = DeathknightApprenticeSettings.GetSettings();

    #region Professions & Racials

    private readonly Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private readonly Spell Berserking = new Spell("Berserking");
    private readonly Spell Blood_Fury = new Spell("Blood Fury");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell War_Stomp = new Spell("War Stomp");

    #endregion

    #region Deathknight Presence & Buffs

    private readonly Spell Blood_Plague = new Spell("Blood Plague");
    private Timer Blood_Plague_Timer = new Timer(0);
    private readonly Spell Blood_Presence = new Spell("Blood Presence");
    private readonly Spell Frost_Fever = new Spell("Frost Fever");
    private Timer Frost_Fever_Timer = new Timer(0);
    private readonly Spell Frost_Presence = new Spell("Frost Presence");

    #endregion

    #region Offensive Spell

    private readonly Spell Blood_Boil = new Spell("Blood Boil");
    private readonly Spell Blood_Strike = new Spell("Blood Strike");
    private readonly Spell Death_Coil = new Spell("Death Coil");
    private readonly Spell Icy_Touch = new Spell("Icy Touch");
    private readonly Spell Plague_Strike = new Spell("Plague Strike");

    #endregion

    #region Offensive Cooldown

    private readonly Spell Death_Grip = new Spell("Death Grip");
    private readonly Spell Pestilence = new Spell("Pestilence");
    private Timer Pestilence_Timer = new Timer(0);
    private readonly Spell Raise_Dead = new Spell("Raise Dead");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Chains_of_Ice = new Spell("Chains of Ice");
    private readonly Spell Mind_Freeze = new Spell("Mind Freeze");

    #endregion

    #region Healing Spell

    private readonly Spell Death_Strike = new Spell("Death Strike");

    #endregion

    public Deathknight_Apprentice()
    {
        Main.range = 5.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
            {
                if (!ObjectManager.Me.IsMounted)
                {
                    if (Fight.InFight && ObjectManager.Me.Target > 0)
                    {
                        if (ObjectManager.Me.Target != lastTarget &&
                            (Death_Grip.IsDistanceGood || Icy_Touch.IsDistanceGood))
                        {
                            Pull();
                            lastTarget = ObjectManager.Me.Target;
                        }

                        Combat();
                    }

                    else if (ObjectManager.Me.IsCast)
                        Patrolling();
                }
            }
            catch
            {
            }
            Thread.Sleep(150);
        }
    }

    public void Pull()
    {
        if (Death_Grip.IsSpellUsable && MySettings.UseDeathGrip && Death_Grip.IsDistanceGood)
            Grip();
        else
        {
            if (Icy_Touch.IsSpellUsable && MySettings.UseIcyTouch && Icy_Touch.IsDistanceGood)
                Icy_Touch.Launch();
        }
    }

    public void Grip()
    {
        if (ObjectManager.Target.GetDistance > 10 && Death_Grip.IsSpellUsable && MySettings.UseDeathGrip
            && Death_Grip.IsDistanceGood)
        {
            Death_Grip.Launch();
            MovementManager.StopMove();
        }
    }

    public void Combat()
    {
        Presence();
        Defense_Cycle();
        Heal();
        Decast();
        DPS_Burst();
        DPS_Cycle();
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Presence();
            Heal();
        }
    }

    private void Presence()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Blood_Presence.KnownSpell && ObjectManager.GetNumberAttackPlayer() >= 3 && MySettings.UseBloodPresence)
        {
            if (!Blood_Presence.HaveBuff && Blood_Presence.IsSpellUsable)
                Blood_Presence.Launch();
        }
        else
        {
            if (Frost_Presence.KnownSpell && MySettings.UseFrostPresence)
            {
                if (!Frost_Presence.HaveBuff && Frost_Presence.IsSpellUsable)
                    Frost_Presence.Launch();
            }
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell
            && MySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 75)
            {
                if (Death_Strike.KnownSpell && Death_Strike.IsSpellUsable && MySettings.UseDeathStrike)
                {
                    Death_Strike.Launch();
                    return;
                }
            }
        }
    }

    private void DPS_Burst()
    {
        if (Raise_Dead.KnownSpell && MySettings.UseRaiseDead)
        {
            if (Raise_Dead.IsSpellUsable && ObjectManager.Target.GetDistance < 30)
                Raise_Dead.Launch();
        }
        else if (Lifeblood.KnownSpell && MySettings.UseLifeblood)
        {
            if (Lifeblood.IsSpellUsable && ObjectManager.Target.GetDistance < 10)
                Lifeblood.Launch();
        }
        else if (Berserking.KnownSpell && MySettings.UseBerserking)
        {
            if (Berserking.IsSpellUsable && ObjectManager.Target.GetDistance < 10)
                Berserking.Launch();
        }
        else
        {
            if (Blood_Fury.KnownSpell && MySettings.UseBloodFury)
            {
                if (Blood_Fury.IsSpellUsable && ObjectManager.Target.GetDistance < 10)
                    Blood_Fury.Launch();
            }
        }
    }

    private void Defense_Cycle()
    {
        if (Stoneform.KnownSpell && MySettings.UseStoneform)
        {
            if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable)
            {
                Stoneform.Launch();
                return;
            }
        }
        else
        {
            if (War_Stomp.KnownSpell && MySettings.UseWarStomp)
            {
                if (ObjectManager.Me.HealthPercent < 65 && ObjectManager.Target.GetDistance < 8
                    && War_Stomp.IsSpellUsable)
                {
                    War_Stomp.Launch();
                    return;
                }
            }
        }
    }

    private void DPS_Cycle()
    {
        Grip();

        if ((!Frost_Fever.TargetHaveBuff || Frost_Fever_Timer.IsReady) && MySettings.UseIcyTouch
            && Icy_Touch.IsSpellUsable && Icy_Touch.IsDistanceGood)
        {
            Icy_Touch.Launch();
            return;
        }
        else if ((!Blood_Plague.TargetHaveBuff || Blood_Plague_Timer.IsReady) && MySettings.UsePlagueStrike
                 && Plague_Strike.IsSpellUsable && Plague_Strike.IsDistanceGood)
        {
            Plague_Strike.Launch();
            return;
        }
        else if (Blood_Plague.TargetHaveBuff && Frost_Fever.TargetHaveBuff && Pestilence.KnownSpell &&
                 MySettings.UsePestilence
                 && Pestilence.IsSpellUsable && Pestilence.IsDistanceGood && ObjectManager.GetNumberAttackPlayer() > 1 &&
                 Pestilence_Timer.IsReady)
        {
            Pestilence.Launch();
            Pestilence_Timer = new Timer(1000*30);
            return;
        }
        else if (Blood_Plague.TargetHaveBuff && Frost_Fever.TargetHaveBuff && Blood_Boil.KnownSpell &&
                 MySettings.UseBloodBoil
                 && Blood_Boil.IsSpellUsable && ObjectManager.Target.GetDistance < 10 &&
                 ObjectManager.GetNumberAttackPlayer() > 2
                 && !Pestilence_Timer.IsReady)
        {
            Blood_Boil.Launch();
            return;
        }
        else if (Death_Coil.IsDistanceGood && Death_Coil.IsSpellUsable && MySettings.UseDeathCoil)
        {
            Death_Coil.Launch();
            return;
        }
        else if (Blood_Strike.IsSpellUsable && Blood_Strike.IsDistanceGood && MySettings.UseBloodStrike)
        {
            Blood_Strike.Launch();
            return;
        }
        else if (Icy_Touch.IsSpellUsable && Icy_Touch.IsDistanceGood && MySettings.UseIcyTouch)
        {
            Icy_Touch.Launch();
            return;
        }
        else
        {
            if (Plague_Strike.IsSpellUsable && Plague_Strike.IsDistanceGood && MySettings.UsePlagueStrike)
            {
                Plague_Strike.Launch();
                return;
            }
        }
    }

    private void Decast()
    {
        if (Arcane_Torrent.KnownSpell && MySettings.UseArcaneTorrent)
        {
            if (Arcane_Torrent.IsSpellUsable && ObjectManager.Target.IsCast && ObjectManager.Target.GetDistance < 8)
            {
                Arcane_Torrent.Launch();
                return;
            }
        }
        else
        {
            if (Mind_Freeze.KnownSpell && MySettings.UseMindFreeze)
            {
                if (ObjectManager.Target.IsCast && Mind_Freeze.IsSpellUsable && Mind_Freeze.IsDistanceGood)
                {
                    Mind_Freeze.Launch();
                    return;
                }
            }
        }

        if (ObjectManager.Target.GetMove && !Chains_of_Ice.TargetHaveBuff && MySettings.UseChainsofIce
            && Chains_of_Ice.KnownSpell && Chains_of_Ice.IsSpellUsable && Chains_of_Ice.IsDistanceGood)
        {
            Chains_of_Ice.Launch();
            return;
        }
    }
}

public class Deathknight_Blood
{
    [Serializable]
    public class DeathknightBloodSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Deathknight Presence & Buffs */
        public bool UseFrostPresence = true;
        public bool UseBloodPresence = true;
        public bool UseHornofWinter = true;
        public bool UsePathofFrost = true;
        public bool UseUnholyPresence = true;
        /* Offensive Spell */
        public bool UseBloodBoil = true;
        public bool UseDeathCoil = true;
        public bool UseDeathandDecay = true;
        public bool UseDeathStrike = true;
        public bool UseHeartStrike = true;
        public bool UseIcyTouch = true;
        public bool UsePlagueStrike = true;
        public bool UseRuneStrike = true;
        public bool UseSoulReaper = true;
        public bool UseUnholyBlight = true;
        /* Offensive Cooldown */
        public bool UseBloodTap = true;
        public bool UseDancingRuneWeapon = true;
        public bool UseDeathGrip = true;
        public bool UseEmpowerRuneWeapon = true;
        public bool UseOutbreak = true;
        public bool UsePestilence = true;
        public bool UseRaiseDead = true;
        /* Defensive Cooldown */
        public bool UseAntiMagicShell = true;
        public bool UseAntiMagicZone = true;
        public bool UseArmyoftheDead = true;
        public bool UseAsphyxiate = true;
        public bool UseBoneShield = true;
        public bool UseChainsofIce = true;
        public bool UseDeathsAdvance = true;
        public bool UseIceboundFortitude = true;
        public bool UseMindFreeze = true;
        public bool UseRemorselessWinter = true;
        public bool UseStrangulate = true;
        public bool UseVampiricBlood = true;
        /* Healing Spell */
        public bool UseConversion = true;
        public bool UseDeathPact = true;
        public bool UseDeathSiphon = true;
        public bool UseLichborne = true;
        public bool UseRuneTap = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;

        public DeathknightBloodSettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Deathknight Blood Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Deathknight Presence & Buffs */
            AddControlInWinForm("Use Frost Presence", "UseFrostPresence", "Deathknight Presence & Buffs");
            AddControlInWinForm("Use Blood Presence", "UseBloodPresence", "Deathknight Presence & Buffs");
            AddControlInWinForm("Use Horn of Winter", "UseHornofWinter", "Deathknight Presence & Buffs");
            AddControlInWinForm("Use Path of Frost", "UsePathofFrost", "Deathknight Presence & Buffs");
            AddControlInWinForm("Use Unholy Presence", "UseUnholyPresence", "Deathknight Presence & Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Blood Boil", "UseBloodBoil", "Offensive Spell");
            AddControlInWinForm("Use Death Coil", "UseDeathCoil", "Offensive Spell");
            AddControlInWinForm("Use Death and Decay", "UseDeathandDecay", "Offensive Spell");
            AddControlInWinForm("Use Death Strike", "UseDeathStrike", "Offensive Spell");
            AddControlInWinForm("Use Heart Strike", "UseHeartStrike", "Offensive Spell");
            AddControlInWinForm("Use Icy Touch", "UseIcyTouch", "Offensive Spell");
            AddControlInWinForm("Use Plague Strike", "UsePlagueStrike", "Offensive Spell");
            AddControlInWinForm("Use Rune Strike", "UseRuneStrike", "Offensive Spell");
            AddControlInWinForm("Use Soul Reaper", "UseSoulReaper", "Offensive Spell");
            AddControlInWinForm("Use Unholy Blight", "UseUnholyBlight", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Blood Tap", "UseBloodTap", "Offensive Cooldown");
            AddControlInWinForm("Use Dancing Rune Weapon", "UseDancingRuneWeapon", "Offensive Cooldown");
            AddControlInWinForm("Use Death Grip", "UseDeathGrip", "Offensive Cooldown");
            AddControlInWinForm("Use Empower Rune Weapon", "UseEmpowerRuneWeapon", "Offensive Cooldown");
            AddControlInWinForm("Use Outbreak", "UseOutbreak", "Offensive Cooldown");
            AddControlInWinForm("Use Pestilence", "UsePestilence", "Offensive Cooldown");
            AddControlInWinForm("Use Raise Dead", "UseRaiseDead", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Anti-Magic Shell", "UseAntiMagicShell", "Defensive Cooldown");
            AddControlInWinForm("Use Anti-Magic Zone", "UseAntiMagicZone", "Defensive Cooldown");
            AddControlInWinForm("Use Army of the Dead", "UseArmyoftheDead", "Defensive Cooldown");
            AddControlInWinForm("Use Asphyxiate", "UseAsphyxiate", "Defensive Cooldown");
            AddControlInWinForm("Use Bone Shield", "UseBoneShield", "Defensive Cooldown");
            AddControlInWinForm("Use Chains of Ice", "UseChainsofIce", "Defensive Cooldown");
            AddControlInWinForm("Use Death's Advance", "UseDeathsAdvance", "Defensive Cooldown");
            AddControlInWinForm("Use Icebound Fortitude", "UseIceboundFortitude", "Defensive Cooldown");
            AddControlInWinForm("Use Mind Freeze", "UseMindFreeze", "Defensive Cooldown");
            AddControlInWinForm("Use Remorseless Winter", "UseRemorseless Winter", "Defensive Cooldown");
            AddControlInWinForm("Use Strangulate", "UseStrangulate", "Defensive Cooldown");
            AddControlInWinForm("Use Vampiric Blood", "UseVampiricBlood", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Conversion", "UseConversion", "Healing Spell");
            AddControlInWinForm("Use Death Pact", "UseDeathPact", "Healing Spell");
            AddControlInWinForm("Use Death Siphon", "UseDeathSiphon", "Healing Spell");
            AddControlInWinForm("Use Lichborne", "UseLichborne", "Healing Spell");
            AddControlInWinForm("Use Rune Tap", "UseRuneTap", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineer Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static DeathknightBloodSettings CurrentSetting { get; set; }

        public static DeathknightBloodSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Deathknight_Blood.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Deathknight_Blood.DeathknightBloodSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Deathknight_Blood.DeathknightBloodSettings();
            }
        }
    }

    private readonly DeathknightBloodSettings MySettings = DeathknightBloodSettings.GetSettings();

    #region Professions & Racials

    private readonly Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private readonly Spell Berserking = new Spell("Berserking");
    private readonly Spell Blood_Fury = new Spell("Blood Fury");
    private readonly Spell Engineering = new Spell("Engineering");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell War_Stomp = new Spell("War Stomp");

    #endregion

    #region Deathknight Presence & Buffs

    private readonly Spell Blood_Plague = new Spell("Blood Plague");
    private Timer Blood_Plague_Timer = new Timer(0);
    private readonly Spell Blood_Presence = new Spell("Blood Presence");
    private readonly Spell Frost_Fever = new Spell("Frost Fever");
    private Timer Frost_Fever_Timer = new Timer(0);
    private readonly Spell Frost_Presence = new Spell("Frost Presence");
    private readonly Spell Horn_of_Winter = new Spell("Horn of Winter");
    private readonly Spell Path_of_Frost = new Spell("Path of Frost");
    private Timer Path_of_Frost_Timer = new Timer(0);
    private readonly Spell Roiling_Blood = new Spell("Roiling Blood");
    private readonly Spell Unholy_Presence = new Spell("Unholy Presence");

    #endregion

    #region Offensive Spell

    private readonly Spell Blood_Boil = new Spell("Blood Boil");
    private readonly Spell Blood_Strike = new Spell("Blood Strike");
    private readonly Spell Death_and_Decay = new Spell("Death and Decay");
    private readonly Spell Death_Coil = new Spell("Death Coil");
    private readonly Spell Death_Strike = new Spell("Death Strike");
    private readonly Spell Heart_Strike = new Spell("Heart Strike");
    private readonly Spell Icy_Touch = new Spell("Icy Touch");
    private readonly Spell Plague_Strike = new Spell("Plague Strike");
    private readonly Spell Rune_Strike = new Spell("Rune Strike");
    private readonly Spell Soul_Reaper = new Spell("Soul Reaper");
    private readonly Spell Unholy_Blight = new Spell("Unholy Blight");

    #endregion

    #region Offensive Cooldown

    private readonly Spell Blood_Tap = new Spell("Blood Tap");
    private readonly Spell Dancing_Rune_Weapon = new Spell("Dancing Rune Weapon");
    private Timer Dancing_Rune_Weapon_Timer = new Timer(0);
    private readonly Spell Death_Grip = new Spell("Death Grip");
    private readonly Spell Empower_Rune_Weapon = new Spell("Empower Rune Weapon");
    private readonly Spell Outbreak = new Spell("Outbreak");
    private readonly Spell Pestilence = new Spell("Pestilence");
    private Timer Pestilence_Timer = new Timer(0);
    private readonly Spell Raise_Dead = new Spell("Raise Dead");

    #endregion

    #region Defensive Cooldown

    private readonly Spell AntiMagic_Shell = new Spell("Anti-Magic Shell");
    private readonly Spell AntiMagic_Zone = new Spell("Anti-Magic Zone");
    private readonly Spell Army_of_the_Dead = new Spell("Army of the Dead");
    private readonly Spell Asphyxiate = new Spell("Asphyxiate");
    private readonly Spell Bone_Shield = new Spell("Bone Shield");
    private readonly Spell Chains_of_Ice = new Spell("Chains of Ice");
    private readonly Spell Deaths_Advance = new Spell("Death's Advance");
    private readonly Spell Icebound_Fortitude = new Spell("Icebound Fortitude");
    private readonly Spell Mind_Freeze = new Spell("Mind Freeze");
    private readonly Spell Remorseless_Winter = new Spell("Remorseless Winter");
    private readonly Spell Strangulate = new Spell("Strangulate");
    private readonly Spell Vampiric_Blood = new Spell("Vampiric Blood");

    #endregion

    #region Healing Spell

    private readonly Spell Conversion = new Spell("Conversion");
    private readonly Spell Death_Pact = new Spell("Death Pact");
    private readonly Spell Death_Siphon = new Spell("Death Siphon");
    private readonly Spell Lichborne = new Spell("Lichborne");
    private readonly Spell Rune_Tap = new Spell("Rune Tap");

    #endregion

    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    private Timer OnCD = new Timer(0);
    public int DRW = 1;
    public int LC = 0;

    public Deathknight_Blood()
    {
        Main.range = 5.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
            {
                Buff_Path();
                if (!ObjectManager.Me.IsMounted)
                {
                    if (Fight.InFight && ObjectManager.Me.Target > 0)
                    {
                        if (ObjectManager.Me.Target != lastTarget &&
                            (Death_Grip.IsDistanceGood || Icy_Touch.IsDistanceGood))
                        {
                            Pull();
                            lastTarget = ObjectManager.Me.Target;
                        }

                        if (ObjectManager.Target.Level < 70 && ObjectManager.Me.Level > 84
                            && MySettings.UseLowCombat)
                        {
                            LC = 1;
                            LowCombat();
                        }
                        else
                        {
                            LC = 0;
                            Combat();
                        }
                    }

                    else if (ObjectManager.Me.IsCast)
                        Patrolling();
                }
            }
            catch
            {
            }
            Thread.Sleep(150);
        }
    }

    public void Pull()
    {
        if (Death_Grip.IsSpellUsable && MySettings.UseDeathGrip && Death_Grip.IsDistanceGood)
        {
            Grip();
        }
        else
        {
            if (Icy_Touch.IsSpellUsable && MySettings.UseIcyTouch && Icy_Touch.IsDistanceGood)
                Icy_Touch.Launch();
        }
    }

    private void Buff_Path()
    {
        if (!Fight.InFight && Path_of_Frost.KnownSpell && Path_of_Frost.IsSpellUsable && MySettings.UsePathofFrost
            && (!Path_of_Frost.HaveBuff || Path_of_Frost_Timer.IsReady))
        {
            Path_of_Frost.Launch();
            Path_of_Frost_Timer = new Timer(1000*60*9);
        }
    }

    public void LowCombat()
    {
        Presence();
        AvoidMelee();
        Defense_Cycle();
        Heal();
        Decast();
        Buff();
        Grip();

        if (Icy_Touch.KnownSpell && Icy_Touch.IsSpellUsable && Icy_Touch.IsDistanceGood && MySettings.UseIcyTouch)
        {
            Icy_Touch.Launch();
            if (ObjectManager.Target.HealthPercent < 50 && ObjectManager.Target.HealthPercent > 0)
            {
                Icy_Touch.Launch();
                return;
            }
            return;
        }
        else if (Death_Coil.KnownSpell && Death_Coil.IsSpellUsable && Death_Coil.IsDistanceGood &&
                 MySettings.UseDeathCoil)
        {
            Death_Coil.Launch();
            return;
        }
        else
        {
            if (Blood_Boil.IsSpellUsable && Blood_Boil.IsSpellUsable && Blood_Boil.IsDistanceGood &&
                MySettings.UseBloodBoil)
            {
                Blood_Boil.Launch();
                return;
            }
        }
    }

    public void Combat()
    {
        Presence();
        AvoidMelee();
        if (OnCD.IsReady)
            Defense_Cycle();
        Heal();
        Decast();
        Buff();
        DPS_Burst();
        DPS_Cycle();
    }

    public void DPS_Burst()
    {
        if (MySettings.UseTrinket && Trinket_Timer.IsReady && ObjectManager.Target.GetDistance < 30)
        {
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Trinket_Timer = new Timer(1000*60*2);
            return;
        }
        else if (Berserking.IsSpellUsable && Berserking.KnownSpell && MySettings.UseBerserking
                 && ObjectManager.Target.GetDistance < 30)
        {
            Berserking.Launch();
            return;
        }
        else if (Blood_Fury.IsSpellUsable && Blood_Fury.KnownSpell && MySettings.UseBloodFury
                 && ObjectManager.Target.GetDistance < 30)
        {
            Blood_Fury.Launch();
            return;
        }
        else if (Lifeblood.IsSpellUsable && Lifeblood.KnownSpell && MySettings.UseLifeblood
                 && ObjectManager.Target.GetDistance < 30)
        {
            Lifeblood.Launch();
            return;
        }
        else
        {
            if (MySettings.UseEngGlove && Engineering.KnownSpell && Engineering_Timer.IsReady
                && ObjectManager.Target.GetDistance < 30)
            {
                Logging.WriteFight("Use Engineering Gloves.");
                Lua.RunMacroText("/use 10");
                Engineering_Timer = new Timer(1000*60);
                return;
            }
        }

        if (Dancing_Rune_Weapon_Timer.IsReady && DRW == 0)
            DRW = 1;

        if (DRW == 1 && Dancing_Rune_Weapon.IsSpellUsable && Dancing_Rune_Weapon.KnownSpell
            && Dancing_Rune_Weapon.IsDistanceGood && MySettings.UseDancingRuneWeapon)
        {
            Dancing_Rune_Weapon.Launch();
            Dancing_Rune_Weapon_Timer = new Timer(1000*60*1.5);
            DRW = 0;
            return;
        }
    }

    public void DPS_Cycle()
    {
        Grip();

        if (ObjectManager.Me.HealthPercent < 85 && MySettings.UseDeathCoil
            && Lichborne.HaveBuff && Death_Coil.KnownSpell && Death_Coil.IsSpellUsable)
        {
            Lua.RunMacroText("/target Player");
            Death_Coil.Launch();
            return;
        }
        else if (Unholy_Blight.KnownSpell && Unholy_Blight.IsSpellUsable && ObjectManager.Target.GetDistance < 9
                 && MySettings.UseUnholyBlight)
        {
            Unholy_Blight.Launch();
            Blood_Plague_Timer = new Timer(1000*27);
            Frost_Fever_Timer = new Timer(1000*27);
            return;
        }
        else if (Outbreak.KnownSpell && Outbreak.IsSpellUsable && Outbreak.IsDistanceGood && MySettings.UseOutbreak
                 &&
                 (Blood_Plague_Timer.IsReady || Frost_Fever_Timer.IsReady || !Blood_Plague.TargetHaveBuff ||
                  !Frost_Fever.TargetHaveBuff))
        {
            Outbreak.Launch();
            Blood_Plague_Timer = new Timer(1000*27);
            Frost_Fever_Timer = new Timer(1000*27);
            return;
        }
        else if (Blood_Plague_Timer.IsReady && Frost_Fever_Timer.IsReady && Blood_Plague.TargetHaveBuff &&
                 Frost_Fever.TargetHaveBuff
                 && Blood_Boil.IsSpellUsable && Blood_Boil.KnownSpell && ObjectManager.Target.GetDistance < 9
                 && MySettings.UseBloodBoil && Roiling_Blood.KnownSpell)
        {
            Blood_Boil.Launch();
            Blood_Plague_Timer = new Timer(1000*27);
            Frost_Fever_Timer = new Timer(1000*27);
            return;
        }
        else if (Plague_Strike.KnownSpell && Plague_Strike.IsDistanceGood && !Outbreak.IsSpellUsable
                 && !Unholy_Blight.IsSpellUsable && MySettings.UsePlagueStrike
                 && (Blood_Plague_Timer.IsReady || !Blood_Plague.TargetHaveBuff))
        {
            if (!Plague_Strike.IsSpellUsable && Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable
                && MySettings.UseBloodTap)
            {
                Blood_Tap.Launch();
                Thread.Sleep(200);
            }

            if (Plague_Strike.IsSpellUsable)
            {
                Plague_Strike.Launch();
                Blood_Plague_Timer = new Timer(1000*27);
                return;
            }
        }
        else if (Icy_Touch.KnownSpell && Icy_Touch.IsDistanceGood && MySettings.UseIcyTouch
                 && !Outbreak.IsSpellUsable && !Unholy_Blight.IsSpellUsable
                 && (Frost_Fever_Timer.IsReady || !Frost_Fever.TargetHaveBuff))
        {
            if (!Icy_Touch.IsSpellUsable && Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable
                && MySettings.UseBloodTap)
            {
                Blood_Tap.Launch();
                Thread.Sleep(200);
            }

            if (Icy_Touch.IsSpellUsable)
            {
                Icy_Touch.Launch();
                Frost_Fever_Timer = new Timer(1000*27);
                return;
            }
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 3 && Blood_Boil.IsSpellUsable && Blood_Boil.IsDistanceGood &&
                 Blood_Boil.KnownSpell
                 && MySettings.UseBloodBoil)
        {
            Blood_Boil.Launch();
            if (Blood_Plague.TargetHaveBuff && Frost_Fever.TargetHaveBuff && Roiling_Blood.KnownSpell)
            {
                Blood_Plague_Timer = new Timer(1000*27);
                Frost_Fever_Timer = new Timer(1000*27);
            }
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Pestilence.IsSpellUsable &&
                 Pestilence.IsDistanceGood
                 && Pestilence.KnownSpell && MySettings.UsePestilence && !Roiling_Blood.KnownSpell)
        {
            Pestilence.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Death_and_Decay.KnownSpell &&
                 MySettings.UseDeathandDecay
                 && Death_and_Decay.IsSpellUsable && Death_and_Decay.IsDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(43265, ObjectManager.Target.Position);
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() < 4 && ObjectManager.GetNumberAttackPlayer() > 1 &&
                 Heart_Strike.IsSpellUsable
                 && Heart_Strike.IsDistanceGood && Heart_Strike.KnownSpell && MySettings.UseHeartStrike)
        {
            Heart_Strike.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && Army_of_the_Dead.IsSpellUsable &&
                 Army_of_the_Dead.IsDistanceGood
                 && Army_of_the_Dead.KnownSpell && MySettings.UseArmyoftheDead)
        {
            Army_of_the_Dead.Launch();
            Thread.Sleep(4000);
            return;
        }
        else if (ObjectManager.Me.HaveBuff(81141) && MySettings.UseBloodBoil)
        {
            Blood_Boil.Launch();
            if (Blood_Plague.TargetHaveBuff && Frost_Fever.TargetHaveBuff)
            {
                Blood_Plague_Timer = new Timer(1000*27);
                Frost_Fever_Timer = new Timer(1000*27);
            }
            return;
        }
        else if (Soul_Reaper.KnownSpell && Soul_Reaper.IsDistanceGood &&
                 Soul_Reaper.IsSpellUsable
                 && ObjectManager.Target.HealthPercent < 35 &&
                 ObjectManager.Me.HealthPercent > 90
                 && MySettings.UseSoulReaper)
        {
            Soul_Reaper.Launch();
            return;
        }
        else if (Death_Strike.IsSpellUsable && Death_Strike.IsDistanceGood &&
                 Death_Strike.KnownSpell
                 && MySettings.UseDeathStrike)
        {
            Death_Strike.Launch();
            return;
        }
        else if (Rune_Strike.IsSpellUsable && Rune_Strike.IsDistanceGood &&
                 Rune_Strike.KnownSpell && DRW == 0
                 && MySettings.UseRuneStrike)
        {
            if (ObjectManager.Me.HealthPercent < 80 &&
                ((Lichborne.KnownSpell && MySettings.UseLichborne)
                 || (Conversion.KnownSpell && MySettings.UseConversion)))
                return;
            else
            {
                Rune_Strike.Launch();
                return;
            }
        }
            // Blizzard API Calls for Heart Strike using Blood Strike Function
        else if (Blood_Strike.IsSpellUsable && Blood_Strike.IsDistanceGood &&
                 Blood_Strike.KnownSpell
                 && MySettings.UseHeartStrike)
        {
            Blood_Strike.Launch();
            return;
        }
        else if (Empower_Rune_Weapon.IsSpellUsable &&
                 Empower_Rune_Weapon.KnownSpell &&
                 MySettings.UseEmpowerRuneWeapon)
        {
            Empower_Rune_Weapon.Launch();
            return;
        }
        else if (Horn_of_Winter.KnownSpell && Horn_of_Winter.IsSpellUsable &&
                 MySettings.UseHornofWinter)
        {
            Horn_of_Winter.Launch();
            return;
        }
        else
        {
            if (Arcane_Torrent.IsSpellUsable &&
                Arcane_Torrent.KnownSpell && MySettings.UseArcaneTorrent)
            {
                Arcane_Torrent.Launch();
                return;
            }
        }
    }

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Presence();
            Heal();
            Buff();
        }
    }

    private void Presence()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Unholy_Presence.KnownSpell && MySettings.UseLowCombat && MySettings.UseUnholyPresence)
        {
            if (!Unholy_Presence.HaveBuff && Unholy_Presence.IsSpellUsable && LC == 1)
                Unholy_Presence.Launch();
        }

        else if (Blood_Presence.KnownSpell && MySettings.UseBloodPresence)
        {
            if (!Blood_Presence.HaveBuff && Blood_Presence.IsSpellUsable)
                Blood_Presence.Launch();
        }

        else
        {
            if (Frost_Presence.KnownSpell && MySettings.UseFrostPresence && !MySettings.UseBloodPresence)
            {
                if (!Frost_Presence.HaveBuff && Frost_Presence.IsSpellUsable)
                    Frost_Presence.Launch();
            }
        }
    }

    private void Grip()
    {
        if (ObjectManager.Target.GetDistance > 5 &&
            Death_Grip.KnownSpell && Death_Grip.IsSpellUsable && Death_Grip.IsDistanceGood
            && MySettings.UseDeathGrip)
        {
            Death_Grip.Launch();
            MovementManager.StopMove();
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell
            && MySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 55 && MySettings.UseDeathPact
                 && Raise_Dead.KnownSpell && Raise_Dead.IsSpellUsable
                 && Death_Pact.KnownSpell && Death_Pact.IsSpellUsable)
        {
            int i = 0;
            while (i < 3)
            {
                i++;
                Raise_Dead.Launch();
                Death_Pact.Launch();

                if (!Death_Pact.IsSpellUsable)
                {
                    break;
                }
            }
        }
        else if (ObjectManager.Me.HealthPercent < 45 && MySettings.UseLichborne
                 && Lichborne.KnownSpell && Lichborne.IsSpellUsable
                 && Death_Coil.KnownSpell && Death_Coil.IsSpellUsable)
        {
            if (Lichborne.IsSpellUsable)
            {
                Lichborne.Launch();
                return;
            }
        }
        else if (ObjectManager.Me.HealthPercent < 45 && MySettings.UseConversion
                 && Conversion.KnownSpell && Conversion.IsSpellUsable
                 && ObjectManager.Me.RunicPower > 10)
        {
            if (Conversion.IsSpellUsable)
            {
                Conversion.Launch();
                while (ObjectManager.Me.RunicPower > 0)
                    Thread.Sleep(200);
                return;
            }
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Death_Siphon.KnownSpell && Death_Siphon.IsSpellUsable
                 && MySettings.UseDeathSiphon)
        {
            Death_Siphon.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 70)
            {
                if (Vampiric_Blood.KnownSpell && Vampiric_Blood.IsSpellUsable && MySettings.UseVampiricBlood)
                {
                    Vampiric_Blood.Launch();
                    Thread.Sleep(200);
                }

                if (!Rune_Tap.IsSpellUsable && Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable
                    && MySettings.UseBloodTap)
                {
                    Blood_Tap.Launch();
                    Thread.Sleep(200);
                }

                if (Rune_Tap.KnownSpell && Rune_Tap.IsSpellUsable && MySettings.UseRuneTap
                    && (Vampiric_Blood.HaveBuff || ObjectManager.Me.HaveBuff(81164)))
                    Rune_Tap.Launch();
            }
        }
    }

    private void Defense_Cycle()
    {
        if (!Bone_Shield.HaveBuff && Bone_Shield.KnownSpell && Bone_Shield.IsSpellUsable
            && MySettings.UseBoneShield)
        {
            Bone_Shield.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseIceboundFortitude
                 && Icebound_Fortitude.KnownSpell && Icebound_Fortitude.IsSpellUsable)
        {
            Icebound_Fortitude.Launch();
            OnCD = new Timer(1000*12);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell
                 && MySettings.UseStoneform)
        {
            Stoneform.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else
        {
            if (ObjectManager.Target.GetDistance < 8 && MySettings.UseRemorselessWinter
                && (ObjectManager.Me.HealthPercent < 70 || ObjectManager.GetNumberAttackPlayer() > 1))
            {
                if (Remorseless_Winter.KnownSpell && Remorseless_Winter.IsSpellUsable)
                {
                    Remorseless_Winter.Launch();
                    OnCD = new Timer(1000*8);
                    return;
                }
            }
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseMindFreeze
            && Mind_Freeze.KnownSpell && Mind_Freeze.IsSpellUsable && Mind_Freeze.IsDistanceGood)
        {
            Mind_Freeze.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseAntiMagicShell
                 && AntiMagic_Shell.KnownSpell && AntiMagic_Shell.IsSpellUsable)
        {
            AntiMagic_Shell.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
                 && Strangulate.KnownSpell && Strangulate.IsSpellUsable && Strangulate.IsDistanceGood
                 && (MySettings.UseStrangulate || MySettings.UseAsphyxiate))
        {
            Strangulate.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && AntiMagic_Zone.KnownSpell
                && MySettings.UseAntiMagicZone && AntiMagic_Zone.IsSpellUsable)
            {
                SpellManager.CastSpellByIDAndPosition(51052, ObjectManager.Me.Position);
                return;
            }
        }

        if (ObjectManager.Target.GetMove && !Chains_of_Ice.TargetHaveBuff && MySettings.UseChainsofIce
            && Chains_of_Ice.KnownSpell && Chains_of_Ice.IsSpellUsable && Chains_of_Ice.IsDistanceGood)
        {
            Chains_of_Ice.Launch();
            return;
        }
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (!Horn_of_Winter.HaveBuff && Horn_of_Winter.KnownSpell && Horn_of_Winter.IsSpellUsable
            && MySettings.UseHornofWinter)
        {
            Horn_of_Winter.Launch();
            return;
        }
        else if (!Fight.InFight && Deaths_Advance.KnownSpell && Deaths_Advance.IsSpellUsable
                 && MySettings.UseDeathsAdvance && ObjectManager.Me.GetMove)
        {
            Deaths_Advance.Launch();
            return;
        }
        else
        {
            if (AlchFlask_Timer.IsReady && MySettings.UseAlchFlask
                && ItemsManager.GetItemCountByIdLUA(75525) == 1)
            {
                Logging.WriteFight("Use Alchi Flask");
                Lua.RunMacroText("/use item:75525");
                AlchFlask_Timer = new Timer(1000*60*60*2);
                return;
            }
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 3 && ObjectManager.Target.InCombat)
        {
            nManager.Wow.Helpers.Keybindings.PressKeybindings(nManager.Wow.Enums.Keybindings.MOVEBACKWARD);
        }
    }
}

public class Deathknight_Unholy
{
    [Serializable]
    public class DeathknightUnholySettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Deathknight Presence & Buffs */
        public bool UseFrostPresence = true;
        public bool UseBloodPresence = true;
        public bool UseHornofWinter = true;
        public bool UsePathofFrost = true;
        public bool UseUnholyPresence = true;
        /* Offensive Spell */
        public bool UseBloodBoil = true;
        public bool UseDarkTransformation = true;
        public bool UseDeathCoil = true;
        public bool UseDeathandDecay = true;
        public bool UseFesteringStrike = true;
        public bool UseIcyTouch = true;
        public bool UsePlagueStrike = true;
        public bool UseSoulReaper = true;
        public bool UseScourgeStrike = true;
        public bool UseUnholyBlight = true;
        /* Offensive Cooldown */
        public bool UseBloodTap = true;
        public bool UseDeathGrip = true;
        public bool UseEmpowerRuneWeapon = true;
        public bool UseOutbreak = true;
        public bool UsePestilence = true;
        public bool UseRaiseDead = true;
        public bool UseSummonGargoyle = true;
        public bool UseUnholyFrenzy = true;
        /* Defensive Cooldown */
        public bool UseAntiMagicShell = true;
        public bool UseAntiMagicZone = true;
        public bool UseArmyoftheDead = true;
        public bool UseAsphyxiate = true;
        public bool UseChainsofIce = true;
        public bool UseDeathsAdvance = true;
        public bool UseIceboundFortitude = true;
        public bool UseMindFreeze = true;
        public bool UseRemorselessWinter = true;
        public bool UseStrangulate = true;
        /* Healing Spell */
        public bool UseConversion = true;
        public bool UseDeathPact = true;
        public bool UseDeathSiphon = true;
        public bool UseDeathStrike = true;
        public bool UseLichborne = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;

        public DeathknightUnholySettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Deathknight Unholy Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Deathknight Presence & Buffs */
            AddControlInWinForm("Use Frost Presence", "UseFrostPresence", "Deathknight Presence & Buffs");
            AddControlInWinForm("Use Blood Presence", "UseBloodPresence", "Deathknight Presence & Buffs");
            AddControlInWinForm("Use Horn of Winter", "UseHornofWinter", "Deathknight Presence & Buffs");
            AddControlInWinForm("Use Path of Frost", "UsePathofFrost", "Deathknight Presence & Buffs");
            AddControlInWinForm("Use Unholy Presence", "UseUnholyPresence", "Deathknight Presence & Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Blood Boil", "UseBloodBoil", "Offensive Spell");
            AddControlInWinForm("Use Dark Transformation", "UseDarkTransformation", "Offensive Spell");
            AddControlInWinForm("Use Death Coil", "UseDeathCoil", "Offensive Spell");
            AddControlInWinForm("Use Death and Decay", "UseDeathandDecay", "Offensive Spell");
            AddControlInWinForm("Use Festering Strike", "UseFesteringStrike", "Offensive Spell");
            AddControlInWinForm("Use Icy Touch", "UseIcyTouch", "Offensive Spell");
            AddControlInWinForm("Use Plague Strike", "UsePlagueStrike", "Offensive Spell");
            AddControlInWinForm("Use Soul Reaper", "UseSoulReaper", "Offensive Spell");
            AddControlInWinForm("Use Scourge Strike", "UseScourgeStrike", "Offensive Spell");
            AddControlInWinForm("Use Unholy Blight", "UseUnholyBlight", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Blood Tap", "UseBloodTap", "Offensive Cooldown");
            AddControlInWinForm("Use Death Grip", "UseDeathGrip", "Offensive Cooldown");
            AddControlInWinForm("Use Empower Rune Weapon", "UseEmpowerRuneWeapon", "Offensive Cooldown");
            AddControlInWinForm("Use Outbreak", "UseOutbreak", "Offensive Cooldown");
            AddControlInWinForm("Use Pestilence", "UsePestilence", "Offensive Cooldown");
            AddControlInWinForm("Use Raise Dead", "UseRaiseDead", "Offensive Cooldown");
            AddControlInWinForm("Use Summon Gargoyle", "UseSummonGargoyle", "Offensive Cooldown");
            AddControlInWinForm("Use Unholy Frenzy", "UseUnholyFrenzy", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Anti-Magic Shell", "UseAntiMagicShell", "Defensive Cooldown");
            AddControlInWinForm("Use Anti-Magic Zone", "UseAntiMagicZone", "Defensive Cooldown");
            AddControlInWinForm("Use Army of the Dead", "UseArmyoftheDead", "Defensive Cooldown");
            AddControlInWinForm("Use Asphyxiate", "UseAsphyxiate", "Defensive Cooldown");
            AddControlInWinForm("Use Chains of Ice", "UseChainsofIce", "Defensive Cooldown");
            AddControlInWinForm("Use Death's Advance", "UseDeathsAdvance", "Defensive Cooldown");
            AddControlInWinForm("Use Icebound Fortitude", "UseIceboundFortitude", "Defensive Cooldown");
            AddControlInWinForm("Use Mind Freeze", "UseMindFreeze", "Defensive Cooldown");
            AddControlInWinForm("Use Remorseless Winter", "UseRemorseless Winter", "Defensive Cooldown");
            AddControlInWinForm("Use Strangulate", "UseStrangulate", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Conversion", "UseConversion", "Healing Spell");
            AddControlInWinForm("Use Death Pact", "UseDeathPact", "Healing Spell");
            AddControlInWinForm("Use Death Siphon", "UseDeathSiphon", "Healing Spell");
            AddControlInWinForm("Use Death Strike", "UseDeathStrike", "Healing Spell");
            AddControlInWinForm("Use Lichborne", "UseLichborne", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static DeathknightUnholySettings CurrentSetting { get; set; }

        public static DeathknightUnholySettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Deathknight_Unholy.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Deathknight_Unholy.DeathknightUnholySettings>(CurrentSettingsFile);
            }
            else
            {
                return new Deathknight_Unholy.DeathknightUnholySettings();
            }
        }
    }

    private readonly DeathknightUnholySettings MySettings = DeathknightUnholySettings.GetSettings();

    #region Professions & Racials

    private readonly Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private readonly Spell Berserking = new Spell("Berserking");
    private readonly Spell Blood_Fury = new Spell("Blood Fury");
    private readonly Spell Engineering = new Spell("Engineering");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell War_Stomp = new Spell("War Stomp");

    #endregion

    #region Deathknight Presence & Buffs

    private readonly Spell Blood_Plague = new Spell("Blood Plague");
    private Timer Blood_Plague_Timer = new Timer(0);
    private readonly Spell Blood_Presence = new Spell("Blood Presence");
    private readonly Spell Frost_Fever = new Spell("Frost Fever");
    private Timer Frost_Fever_Timer = new Timer(0);
    private readonly Spell Frost_Presence = new Spell("Frost Presence");
    private readonly Spell Horn_of_Winter = new Spell("Horn of Winter");
    private readonly Spell Path_of_Frost = new Spell("Path of Frost");
    private Timer Path_of_Frost_Timer = new Timer(0);
    private readonly Spell Roiling_Blood = new Spell("Roiling Blood");
    private readonly Spell Unholy_Presence = new Spell("Unholy Presence");

    #endregion

    #region Offensive Spell

    private readonly Spell Blood_Boil = new Spell("Blood Boil");
    private readonly Spell Blood_Strike = new Spell("Blood Strike");
    private readonly Spell Dark_Transformation = new Spell("Dark Transformation");
    private Timer Dark_Transformation_Timer = new Timer(0);
    private readonly Spell Death_and_Decay = new Spell("Death and Decay");
    private readonly Spell Death_Coil = new Spell("Death Coil");
    private readonly Spell Festering_Strike = new Spell("Festering Strike");
    private readonly Spell Icy_Touch = new Spell("Icy Touch");
    private readonly Spell Plague_Strike = new Spell("Plague Strike");
    private readonly Spell Soul_Reaper = new Spell("Soul Reaper");
    private readonly Spell Scourge_Strike = new Spell("Scourge Strike");
    private readonly Spell Unholy_Blight = new Spell("Unholy Blight");

    #endregion

    #region Offensive Cooldown

    private readonly Spell Blood_Tap = new Spell("Blood Tap");
    private readonly Spell Death_Grip = new Spell("Death Grip");
    private readonly Spell Empower_Rune_Weapon = new Spell("Empower Rune Weapon");
    private readonly Spell Outbreak = new Spell("Outbreak");
    private readonly Spell Pestilence = new Spell("Pestilence");
    private Timer Pestilence_Timer = new Timer(0);
    private readonly Spell Raise_Dead = new Spell("Raise Dead");
    private readonly Spell Summon_Gargoyle = new Spell("Summon Gargoyle");
    private Timer Summon_Gargoyle_Timer = new Timer(0);
    private readonly Spell Unholy_Frenzy = new Spell("Unholy Frenzy");

    #endregion

    #region Defensive Cooldown

    private readonly Spell AntiMagic_Shell = new Spell("Anti-Magic Shell");
    private readonly Spell AntiMagic_Zone = new Spell("Anti-Magic Zone");
    private readonly Spell Army_of_the_Dead = new Spell("Army of the Dead");
    private readonly Spell Asphyxiate = new Spell("Asphyxiate");
    private readonly Spell Chains_of_Ice = new Spell("Chains of Ice");
    private readonly Spell Deaths_Advance = new Spell("Death's Advance");
    private readonly Spell Icebound_Fortitude = new Spell("Icebound Fortitude");
    private readonly Spell Mind_Freeze = new Spell("Mind Freeze");
    private readonly Spell Remorseless_Winter = new Spell("Remorseless Winter");
    private readonly Spell Strangulate = new Spell("Strangulate");

    #endregion

    #region Healing Spell

    private readonly Spell Conversion = new Spell("Conversion");
    private readonly Spell Death_Pact = new Spell("Death Pact");
    private readonly Spell Death_Siphon = new Spell("Death Siphon");
    private readonly Spell Death_Strike = new Spell("Death Strike");
    private readonly Spell Lichborne = new Spell("Lichborne");

    #endregion

    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    private Timer OnCD = new Timer(0);
    public int SG = 1;
    public int DT = 1;
    public int LC = 0;

    public Deathknight_Unholy()
    {
        Main.range = 5.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
            {
                Buff_Path();
                if (!ObjectManager.Me.IsMounted)
                {
                    if (Fight.InFight && ObjectManager.Me.Target > 0)
                    {
                        if (ObjectManager.Me.Target != lastTarget &&
                            (Death_Grip.IsDistanceGood || Icy_Touch.IsDistanceGood))
                        {
                            Pull();
                            lastTarget = ObjectManager.Me.Target;
                        }

                        if (ObjectManager.Target.Level < 70 && ObjectManager.Me.Level > 84
                            && MySettings.UseLowCombat)
                        {
                            LC = 1;
                            LowCombat();
                        }
                        else
                        {
                            LC = 0;
                            Combat();
                        }
                    }

                    else if (ObjectManager.Me.IsCast)
                        Patrolling();
                }
            }
            catch
            {
            }
            Thread.Sleep(150);
        }
    }

    public void Pull()
    {
        if (Death_Grip.IsSpellUsable && MySettings.UseDeathGrip && Death_Grip.IsDistanceGood)
            Grip();
        else
        {
            if (Icy_Touch.IsSpellUsable && MySettings.UseIcyTouch && Icy_Touch.IsDistanceGood)
                Icy_Touch.Launch();
        }
    }

    private void Buff_Path()
    {
        if (!Fight.InFight && Path_of_Frost.KnownSpell && Path_of_Frost.IsSpellUsable && MySettings.UsePathofFrost
            && (!Path_of_Frost.HaveBuff || Path_of_Frost_Timer.IsReady))
        {
            Path_of_Frost.Launch();
            Path_of_Frost_Timer = new Timer(1000*60*9);
        }
    }

    public void LowCombat()
    {
        Presence();
        AvoidMelee();
        Defense_Cycle();
        Heal();
        Decast();
        Buff();
        Grip();

        if (Icy_Touch.KnownSpell && Icy_Touch.IsSpellUsable && Icy_Touch.IsDistanceGood && MySettings.UseIcyTouch)
        {
            Icy_Touch.Launch();
            if (ObjectManager.Target.HealthPercent < 50 && ObjectManager.Target.HealthPercent > 0)
            {
                Icy_Touch.Launch();
                return;
            }
            return;
        }
        else if (Death_Coil.KnownSpell && Death_Coil.IsSpellUsable && Death_Coil.IsDistanceGood
                 && MySettings.UseDeathCoil)
        {
            Death_Coil.Launch();
            return;
        }
        else
        {
            if (Blood_Boil.KnownSpell && Blood_Boil.IsSpellUsable && Blood_Boil.IsDistanceGood
                && MySettings.UseBloodBoil)
            {
                Blood_Boil.Launch();
                return;
            }
        }
    }

    public void Combat()
    {
        Presence();
        AvoidMelee();
        if (OnCD.IsReady)
            Defense_Cycle();
        Heal();
        Decast();
        Buff();
        DPS_Burst();
        DPS_Cycle();
    }

    public void DPS_Burst()
    {
        if (MySettings.UseTrinket && Trinket_Timer.IsReady && ObjectManager.Target.GetDistance < 30)
        {
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Trinket_Timer = new Timer(1000*60*2);
            return;
        }
        else if (Berserking.IsSpellUsable && Berserking.KnownSpell && MySettings.UseBerserking
                 && ObjectManager.Target.GetDistance < 30)
        {
            Berserking.Launch();
            return;
        }
        else if (Blood_Fury.IsSpellUsable && Blood_Fury.KnownSpell && MySettings.UseBloodFury
                 && ObjectManager.Target.GetDistance < 30)
        {
            Blood_Fury.Launch();
            return;
        }
        else if (Lifeblood.IsSpellUsable && Lifeblood.KnownSpell && MySettings.UseLifeblood
                 && ObjectManager.Target.GetDistance < 30)
        {
            Lifeblood.Launch();
            return;
        }
        else if (MySettings.UseEngGlove && Engineering.KnownSpell && Engineering_Timer.IsReady
                 && ObjectManager.Target.GetDistance < 30)
        {
            Logging.WriteFight("Use Engineering Gloves.");
            Lua.RunMacroText("/use 10");
            Engineering_Timer = new Timer(1000*60);
            return;
        }
        else
        {
            if (Unholy_Frenzy.IsSpellUsable && Unholy_Frenzy.KnownSpell
                && ObjectManager.Target.GetDistance < 30)
            {
                Unholy_Frenzy.Launch();
                return;
            }
        }

        if (Summon_Gargoyle_Timer.IsReady && SG == 0)
            SG++;

        if (SG == 1 && Summon_Gargoyle.IsSpellUsable && Summon_Gargoyle.KnownSpell && Summon_Gargoyle.IsDistanceGood
            && MySettings.UseSummonGargoyle)
        {
            Summon_Gargoyle.Launch();
            Summon_Gargoyle_Timer = new Timer(180000);
            SG--;
            return;
        }

        if (Dark_Transformation_Timer.IsReady && DT == 0)
            DT++;

        if (DT == 1 && Dark_Transformation.IsSpellUsable && Dark_Transformation.KnownSpell
            && MySettings.UseDarkTransformation)
        {
            Dark_Transformation.Launch();
            Dark_Transformation_Timer = new Timer(1000*30);
            DT--;
            return;
        }
    }

    public void DPS_Cycle()
    {
        Grip();

        if (ObjectManager.Me.HealthPercent < 85 && MySettings.UseDeathCoil
            && Lichborne.HaveBuff && Death_Coil.KnownSpell && Death_Coil.IsSpellUsable)
        {
            Lua.RunMacroText("/target Player");
            Death_Coil.Launch();
            return;
        }
        else if (Unholy_Blight.KnownSpell && Unholy_Blight.IsSpellUsable && ObjectManager.Target.GetDistance < 9
                 && MySettings.UseUnholyBlight)
        {
            Unholy_Blight.Launch();
            Blood_Plague_Timer = new Timer(1000*27);
            Frost_Fever_Timer = new Timer(1000*27);
            return;
        }
        else if (Outbreak.KnownSpell && Outbreak.IsSpellUsable && Outbreak.IsDistanceGood && MySettings.UseOutbreak
                 &&
                 (Blood_Plague_Timer.IsReady || Frost_Fever_Timer.IsReady || !Blood_Plague.TargetHaveBuff ||
                  !Frost_Fever.TargetHaveBuff))
        {
            Outbreak.Launch();
            Blood_Plague_Timer = new Timer(1000*27);
            Frost_Fever_Timer = new Timer(1000*27);
            return;
        }
        else if (Blood_Plague_Timer.IsReady && Frost_Fever_Timer.IsReady && Blood_Plague.TargetHaveBuff &&
                 Frost_Fever.TargetHaveBuff
                 && Blood_Boil.IsSpellUsable && Blood_Boil.KnownSpell && ObjectManager.Target.GetDistance < 9
                 && MySettings.UseBloodBoil && Roiling_Blood.KnownSpell)
        {
            Blood_Boil.Launch();
            Blood_Plague_Timer = new Timer(1000*27);
            Frost_Fever_Timer = new Timer(1000*27);
            return;
        }
        else if (Plague_Strike.KnownSpell && Plague_Strike.IsDistanceGood && !Outbreak.IsSpellUsable
                 && !Unholy_Blight.IsSpellUsable && MySettings.UsePlagueStrike
                 && (Blood_Plague_Timer.IsReady || !Blood_Plague.TargetHaveBuff))
        {
            if (!Plague_Strike.IsSpellUsable && Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable
                && MySettings.UseBloodTap)
            {
                Blood_Tap.Launch();
                Thread.Sleep(200);
            }

            if (Plague_Strike.IsSpellUsable)
            {
                Plague_Strike.Launch();
                Blood_Plague_Timer = new Timer(1000*27);
                return;
            }
        }
        else if (Icy_Touch.KnownSpell && Icy_Touch.IsDistanceGood && MySettings.UseIcyTouch
                 && !Outbreak.IsSpellUsable && !Unholy_Blight.IsSpellUsable
                 && (Frost_Fever_Timer.IsReady || !Frost_Fever.TargetHaveBuff))
        {
            if (!Icy_Touch.IsSpellUsable && Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable
                && MySettings.UseBloodTap)
            {
                Blood_Tap.Launch();
                Thread.Sleep(200);
            }

            if (Icy_Touch.IsSpellUsable)
            {
                Icy_Touch.Launch();
                Frost_Fever_Timer = new Timer(1000*27);
                return;
            }
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Pestilence.IsSpellUsable && Pestilence.IsDistanceGood
                 && Pestilence.KnownSpell && MySettings.UsePestilence && !Roiling_Blood.KnownSpell)
        {
            Pestilence.Launch();
            return;
        }
        else if (Dark_Transformation_Timer.IsReady && DT == 1 && SG == 0 && MySettings.UseDarkTransformation)
        {
            if (Death_Coil.KnownSpell && Death_Coil.IsSpellUsable && Death_Coil.IsDistanceGood
                && MySettings.UseDeathCoil)
            {
                if (ObjectManager.Me.HealthPercent < 80 && ((Lichborne.KnownSpell && MySettings.UseLichborne)
                                                            || (Conversion.KnownSpell && MySettings.UseConversion)))
                    return;
                else
                {
                    Death_Coil.Launch();
                    return;
                }
            }
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && MySettings.UseDeathandDecay
                 && Death_and_Decay.KnownSpell && Death_and_Decay.IsSpellUsable && Death_and_Decay.IsDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(43265, ObjectManager.Target.Position);
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && Army_of_the_Dead.IsSpellUsable &&
                 Army_of_the_Dead.IsDistanceGood
                 && Army_of_the_Dead.KnownSpell && MySettings.UseArmyoftheDead)
        {
            Army_of_the_Dead.Launch();
            Thread.Sleep(4000);
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Blood_Boil.IsSpellUsable &&
                 Blood_Boil.IsDistanceGood && Blood_Boil.KnownSpell
                 && MySettings.UseBloodBoil)
        {
            Blood_Boil.Launch();
            if (Blood_Plague.TargetHaveBuff && Frost_Fever.TargetHaveBuff && Roiling_Blood.KnownSpell)
            {
                Blood_Plague_Timer = new Timer(1000*27);
                Frost_Fever_Timer = new Timer(1000*27);
            }
            return;
        }

        else if (Soul_Reaper.KnownSpell && Soul_Reaper.IsDistanceGood && Soul_Reaper.IsSpellUsable
                 && ObjectManager.Target.HealthPercent < 35 && ObjectManager.Me.HealthPercent > 80
                 && MySettings.UseSoulReaper)
        {
            Soul_Reaper.Launch();
            return;
        }
        else if (ObjectManager.Me.RunicPowerPercentage < 90 && MySettings.UseScourgeStrike
                 && Scourge_Strike.KnownSpell && Scourge_Strike.IsSpellUsable &&
                 Scourge_Strike.IsDistanceGood)
        {
            Scourge_Strike.Launch();
            return;
        }
        else if (ObjectManager.Me.RunicPowerPercentage < 90 && MySettings.UseFesteringStrike
                 && Festering_Strike.KnownSpell && Festering_Strike.IsSpellUsable &&
                 Festering_Strike.IsDistanceGood)
        {
            Festering_Strike.Launch();
            return;
        }
        else if (ObjectManager.Me.RunicPowerPercentage >= 90 || ObjectManager.Me.HaveBuff(81340)
                                                                && Death_Coil.KnownSpell &&
                                                                Death_Coil.IsSpellUsable &&
                 Death_Coil.IsDistanceGood
                 && MySettings.UseDeathCoil)
        {
            if (Blood_Tap.IsSpellUsable && Blood_Tap.KnownSpell)
            {
                Blood_Tap.Launch();
                Thread.Sleep(200);
            }
            Death_Coil.Launch();
            return;
        }
        else if (Horn_of_Winter.KnownSpell && Horn_of_Winter.IsSpellUsable && MySettings.UseHornofWinter)
        {
            Horn_of_Winter.Launch();
            return;
        }
        else if (Empower_Rune_Weapon.IsSpellUsable && Empower_Rune_Weapon.KnownSpell &&
                 MySettings.UseEmpowerRuneWeapon)
        {
            Empower_Rune_Weapon.Launch();
            return;
        }
        else
        {
            if (Arcane_Torrent.IsSpellUsable && Arcane_Torrent.KnownSpell && MySettings.UseArcaneTorrent)
            {
                Arcane_Torrent.Launch();
                return;
            }
        }
    }

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Presence();
            Heal();
            Buff();
        }
    }

    private void Presence()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Unholy_Presence.KnownSpell && MySettings.UseUnholyPresence && ObjectManager.Me.HealthPercent > 50)
        {
            if (!Unholy_Presence.HaveBuff && Unholy_Presence.IsSpellUsable)
                Unholy_Presence.Launch();
        }
        else if (Blood_Presence.KnownSpell && MySettings.UseBloodPresence && ObjectManager.Me.HealthPercent < 30)
        {
            if (!Blood_Presence.HaveBuff && Blood_Presence.IsSpellUsable)
                Blood_Presence.Launch();
        }
        else
        {
            if (Frost_Presence.KnownSpell && MySettings.UseFrostPresence && !MySettings.UseUnholyPresence)
            {
                if (!Frost_Presence.HaveBuff && Frost_Presence.IsSpellUsable)
                    Frost_Presence.Launch();
            }
        }
    }

    private void Grip()
    {
        if (ObjectManager.Target.GetDistance > 5 &&
            Death_Grip.KnownSpell && Death_Grip.IsSpellUsable && Death_Grip.IsDistanceGood
            && MySettings.UseDeathGrip)
        {
            Death_Grip.Launch();
            MovementManager.StopMove();
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell
            && MySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 55 && MySettings.UseDeathPact
                 && Raise_Dead.KnownSpell && Raise_Dead.IsSpellUsable
                 && Death_Pact.KnownSpell && Death_Pact.IsSpellUsable)
        {
            int i = 0;
            while (i < 3)
            {
                i++;
                Raise_Dead.Launch();
                Death_Pact.Launch();

                if (!Death_Pact.IsSpellUsable)
                {
                    break;
                }
            }
        }
        else if (ObjectManager.Me.HealthPercent < 45 && MySettings.UseLichborne
                 && Lichborne.KnownSpell && Lichborne.IsSpellUsable
                 && Death_Coil.KnownSpell && Death_Coil.IsSpellUsable)
        {
            if (Lichborne.IsSpellUsable)
            {
                Lichborne.Launch();
                return;
            }
        }
        else if (ObjectManager.Me.HealthPercent < 45 && MySettings.UseConversion
                 && Conversion.KnownSpell && Conversion.IsSpellUsable
                 && ObjectManager.Me.RunicPower > 10)
        {
            if (Conversion.IsSpellUsable)
            {
                Conversion.Launch();
                while (ObjectManager.Me.RunicPower > 0)
                    Thread.Sleep(200);
                return;
            }
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Death_Siphon.KnownSpell && Death_Siphon.IsSpellUsable
                 && MySettings.UseDeathSiphon)
        {
            Death_Siphon.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 80 && Death_Strike.IsSpellUsable && Death_Strike.KnownSpell
                && MySettings.UseDeathStrike)
            {
                Death_Strike.Launch();
                return;
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell)
        {
            Stoneform.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseIceboundFortitude
                 && Icebound_Fortitude.KnownSpell && Icebound_Fortitude.IsSpellUsable)
        {
            Icebound_Fortitude.Launch();
            OnCD = new Timer(1000*12);
            return;
        }
        else
        {
            if (ObjectManager.Target.GetDistance < 8 && MySettings.UseRemorselessWinter
                && (ObjectManager.Me.HealthPercent < 70 || ObjectManager.GetNumberAttackPlayer() > 1))
            {
                if (Remorseless_Winter.KnownSpell && Remorseless_Winter.IsSpellUsable)
                {
                    Remorseless_Winter.Launch();
                    OnCD = new Timer(1000*8);
                    return;
                }
            }
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseMindFreeze
            && Mind_Freeze.KnownSpell && Mind_Freeze.IsSpellUsable && Mind_Freeze.IsDistanceGood)
        {
            Mind_Freeze.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseAntiMagicShell
                 && AntiMagic_Shell.KnownSpell && AntiMagic_Shell.IsSpellUsable)
        {
            AntiMagic_Shell.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
                 && Strangulate.KnownSpell && Strangulate.IsSpellUsable && Strangulate.IsDistanceGood
                 && (MySettings.UseStrangulate || MySettings.UseAsphyxiate))
        {
            Strangulate.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && AntiMagic_Zone.KnownSpell
                && MySettings.UseAntiMagicZone && AntiMagic_Zone.IsSpellUsable)
            {
                SpellManager.CastSpellByIDAndPosition(51052, ObjectManager.Me.Position);
                return;
            }
        }

        if (ObjectManager.Target.GetMove && !Chains_of_Ice.TargetHaveBuff && MySettings.UseChainsofIce
            && Chains_of_Ice.KnownSpell && Chains_of_Ice.IsSpellUsable && Chains_of_Ice.IsDistanceGood)
        {
            Chains_of_Ice.Launch();
            return;
        }
    }

    private void Ghoul()
    {
        if ((ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) &&
            !ObjectManager.Me.IsMounted && !ObjectManager.Me.IsDeadMe && MySettings.UseRaiseDead)
        {
            Logging.WriteFight(" - PET DEAD - ");
            if (Raise_Dead.KnownSpell && Raise_Dead.IsSpellUsable)
            {
                Logging.WriteFight(" - SUMMONING PET - ");
                Raise_Dead.Launch();
            }
        }
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        Ghoul();

        if (!Horn_of_Winter.HaveBuff && Horn_of_Winter.KnownSpell && Horn_of_Winter.IsSpellUsable
            && MySettings.UseHornofWinter)
        {
            Horn_of_Winter.Launch();
            return;
        }
        else if (!Fight.InFight && Deaths_Advance.KnownSpell && Deaths_Advance.IsSpellUsable
                 && MySettings.UseDeathsAdvance && ObjectManager.Me.GetMove)
        {
            Deaths_Advance.Launch();
            return;
        }
        else
        {
            if (AlchFlask_Timer.IsReady && MySettings.UseAlchFlask
                && ItemsManager.GetItemCountByIdLUA(75525) == 1)
            {
                Logging.WriteFight("Use Alchi Flask");
                Lua.RunMacroText("/use item:75525");
                AlchFlask_Timer = new Timer(1000*60*60*2);
                return;
            }
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 3 && ObjectManager.Target.InCombat)
        {
            nManager.Wow.Helpers.Keybindings.PressKeybindings(nManager.Wow.Enums.Keybindings.MOVEBACKWARD);
        }
    }
}

public class Deathknight_Frost
{
    [Serializable]
    public class DeathknightFrostSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Deathknight Presence & Buffs */
        public bool UseFrostPresence = true;
        public bool UseBloodPresence = true;
        public bool UseHornofWinter = true;
        public bool UsePathofFrost = true;
        public bool UseUnholyPresence = true;
        /* Offensive Spell */
        public bool UseBloodBoil = true;
        public bool UseDeathCoil = true;
        public bool UseDeathandDecay = true;
        public bool UseFrostStrike = true;
        public bool UseHowlingBlast = true;
        public bool UseIcyTouch = true;
        public bool UsePlagueStrike = true;
        public bool UseObliterate = true;
        public bool UseSoulReaper = true;
        public bool UseUnholyBlight = true;
        /* Offensive Cooldown */
        public bool UseBloodTap = true;
        public bool UseDeathGrip = true;
        public bool UseEmpowerRuneWeapon = true;
        public bool UseOutbreak = true;
        public bool UsePestilence = true;
        public bool UsePillarofFrost = true;
        public bool UseRaiseDead = true;
        /* Defensive Cooldown */
        public bool UseAntiMagicShell = true;
        public bool UseAntiMagicZone = true;
        public bool UseArmyoftheDead = true;
        public bool UseAsphyxiate = true;
        public bool UseChainsofIce = true;
        public bool UseDeathsAdvance = true;
        public bool UseIceboundFortitude = true;
        public bool UseMindFreeze = true;
        public bool UseRemorselessWinter = true;
        public bool UseStrangulate = true;
        /* Healing Spell */
        public bool UseConversion = true;
        public bool UseDeathPact = true;
        public bool UseDeathSiphon = true;
        public bool UseDeathStrike = true;
        public bool UseLichborne = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;
        public bool UseDuelWield = false;
        public bool UseTwoHander = true;

        public DeathknightFrostSettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Deathknight Frost Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Deathknight Presence & Buffs */
            AddControlInWinForm("Use Frost Presence", "UseFrostPresence", "Deathknight Presence & Buffs");
            AddControlInWinForm("Use Blood Presence", "UseBloodPresence", "Deathknight Presence & Buffs");
            AddControlInWinForm("Use Horn of Winter", "UseHornofWinter", "Deathknight Presence & Buffs");
            AddControlInWinForm("Use Path of Frost", "UsePathofFrost", "Deathknight Presence & Buffs");
            AddControlInWinForm("Use Unholy Presence", "UseUnholyPresence", "Deathknight Presence & Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Blood Boil", "UseBloodBoil", "Offensive Spell");
            AddControlInWinForm("Use Dark Transformation", "UseDarkTransformation", "Offensive Spell");
            AddControlInWinForm("Use Death Coil", "UseDeathCoil", "Offensive Spell");
            AddControlInWinForm("Use Death and Decay", "UseDeathandDecay", "Offensive Spell");
            AddControlInWinForm("Use Frost Strike", "UseFrostStrike", "Offensive Spell");
            AddControlInWinForm("Use Howling Blast", "UseHowlingBlast", "Offensive Spell");
            AddControlInWinForm("Use Icy Touch", "UseIcyTouch", "Offensive Spell");
            AddControlInWinForm("Use Plague Strike", "UsePlagueStrike", "Offensive Spell");
            AddControlInWinForm("Use Obliterate", "UseObliterate", "Offensive Spell");
            AddControlInWinForm("Use Soul Reaper", "UseSoulReaper", "Offensive Spell");
            AddControlInWinForm("Use Unholy Blight", "UseUnholyBlight", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Blood Tap", "UseBloodTap", "Offensive Cooldown");
            AddControlInWinForm("Use Death Grip", "UseDeathGrip", "Offensive Cooldown");
            AddControlInWinForm("Use Empower Rune Weapon", "UseEmpowerRuneWeapon", "Offensive Cooldown");
            AddControlInWinForm("Use Outbreak", "UseOutbreak", "Offensive Cooldown");
            AddControlInWinForm("Use Pestilence", "UsePestilence", "Offensive Cooldown");
            AddControlInWinForm("Use Pillar of Frost", "UsePillarofFrost", "Offensive Cooldown");
            AddControlInWinForm("Use Raise Dead", "UseRaiseDead", "Offensive Cooldown");
            AddControlInWinForm("Use Summon Gargoyle", "UseSummonGargoyle", "Offensive Cooldown");
            AddControlInWinForm("Use Unholy Frenzy", "UseUnholyFrenzy", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Anti-Magic Shell", "UseAntiMagicShell", "Defensive Cooldown");
            AddControlInWinForm("Use Anti-Magic Zone", "UseAntiMagicZone", "Defensive Cooldown");
            AddControlInWinForm("Use Army of the Dead", "UseArmyoftheDead", "Defensive Cooldown");
            AddControlInWinForm("Use Asphyxiate", "UseAsphyxiate", "Defensive Cooldown");
            AddControlInWinForm("Use Chains of Ice", "UseChainsofIce", "Defensive Cooldown");
            AddControlInWinForm("Use Death's Advance", "UseDeathsAdvance", "Defensive Cooldown");
            AddControlInWinForm("Use Icebound Fortitude", "UseIceboundFortitude", "Defensive Cooldown");
            AddControlInWinForm("Use Mind Freeze", "UseMindFreeze", "Defensive Cooldown");
            AddControlInWinForm("Use Remorseless Winter", "UseRemorseless Winter", "Defensive Cooldown");
            AddControlInWinForm("Use Strangulate", "UseStrangulate", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Conversion", "UseConversion", "Healing Spell");
            AddControlInWinForm("Use Death Pact", "UseDeathPact", "Healing Spell");
            AddControlInWinForm("Use Death Siphon", "UseDeathSiphon", "Healing Spell");
            AddControlInWinForm("Use Death Strike", "UseDeathStrike", "Healing Spell");
            AddControlInWinForm("Use Lichborne", "UseLichborne", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
            AddControlInWinForm("Use Duel Wield", "UseDuelWield", "Game Settings");
            AddControlInWinForm("Use Two Hander", "UseTwoHander", "Game Settings");
        }

        public static DeathknightFrostSettings CurrentSetting { get; set; }

        public static DeathknightFrostSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Deathknight_Frost.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Deathknight_Frost.DeathknightFrostSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Deathknight_Frost.DeathknightFrostSettings();
            }
        }
    }

    private readonly DeathknightFrostSettings MySettings = DeathknightFrostSettings.GetSettings();

    #region Professions & Racials

    private readonly Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private readonly Spell Berserking = new Spell("Berserking");
    private readonly Spell Blood_Fury = new Spell("Blood Fury");
    private readonly Spell Engineering = new Spell("Engineering");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell War_Stomp = new Spell("War Stomp");

    #endregion

    #region Deathknight Presence & Buffs

    private readonly Spell Blood_Plague = new Spell("Blood Plague");
    private Timer Blood_Plague_Timer = new Timer(0);
    private readonly Spell Blood_Presence = new Spell("Blood Presence");
    private readonly Spell Freezing_Fog = new Spell(59052);
    private readonly Spell Frost_Fever = new Spell("Frost Fever");
    private Timer Frost_Fever_Timer = new Timer(0);
    private readonly Spell Frost_Presence = new Spell("Frost Presence");
    private readonly Spell Horn_of_Winter = new Spell("Horn of Winter");
    private readonly Spell Path_of_Frost = new Spell("Path of Frost");
    private Timer Path_of_Frost_Timer = new Timer(0);
    private readonly Spell Roiling_Blood = new Spell("Roiling Blood");
    private readonly Spell Unholy_Presence = new Spell("Unholy Presence");

    #endregion

    #region Offensive Spell

    private readonly Spell Blood_Boil = new Spell("Blood Boil");
    private readonly Spell Blood_Strike = new Spell("Blood Strike");
    private readonly Spell Death_and_Decay = new Spell("Death and Decay");
    private readonly Spell Death_Coil = new Spell("Death Coil");
    private readonly Spell Frost_Strike = new Spell("Frost Strike");
    private readonly Spell Howling_Blast = new Spell("Howling Blast");
    private readonly Spell Icy_Touch = new Spell("Icy Touch");
    private readonly Spell Plague_Strike = new Spell("Plague Strike");
    private readonly Spell Obliterate = new Spell("Obliterate");
    private readonly Spell Soul_Reaper = new Spell("Soul Reaper");
    private readonly Spell Unholy_Blight = new Spell("Unholy Blight");

    #endregion

    #region Offensive Cooldown

    private readonly Spell Blood_Tap = new Spell("Blood Tap");
    private readonly Spell Death_Grip = new Spell("Death Grip");
    private readonly Spell Empower_Rune_Weapon = new Spell("Empower Rune Weapon");
    private readonly Spell Pillar_of_Frost = new Spell("Pillar of Frost");
    private readonly Spell Outbreak = new Spell("Outbreak");
    private readonly Spell Pestilence = new Spell("Pestilence");
    private Timer Pestilence_Timer = new Timer(0);
    private readonly Spell Raise_Dead = new Spell("Raise Dead");

    #endregion

    #region Defensive Cooldown

    private readonly Spell AntiMagic_Shell = new Spell("Anti-Magic Shell");
    private readonly Spell AntiMagic_Zone = new Spell("Anti-Magic Zone");
    private readonly Spell Army_of_the_Dead = new Spell("Army of the Dead");
    private readonly Spell Asphyxiate = new Spell("Asphyxiate");
    private readonly Spell Chains_of_Ice = new Spell("Chains of Ice");
    private readonly Spell Deaths_Advance = new Spell("Death's Advance");
    private readonly Spell Icebound_Fortitude = new Spell("Icebound Fortitude");
    private readonly Spell Mind_Freeze = new Spell("Mind Freeze");
    private readonly Spell Remorseless_Winter = new Spell("Remorseless Winter");
    private readonly Spell Strangulate = new Spell("Strangulate");

    #endregion

    #region Healing Spell

    private readonly Spell Conversion = new Spell("Conversion");
    private readonly Spell Death_Pact = new Spell("Death Pact");
    private readonly Spell Death_Siphon = new Spell("Death Siphon");
    private readonly Spell Death_Strike = new Spell("Death Strike");
    private readonly Spell Lichborne = new Spell("Lichborne");

    #endregion

    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    private Timer OnCD = new Timer(0);
    public int LC = 0;

    public Deathknight_Frost()
    {
        Main.range = 5.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
            {
                Buff_Path();
                if (!ObjectManager.Me.IsMounted)
                {
                    if (Fight.InFight && ObjectManager.Me.Target > 0)
                    {
                        if (ObjectManager.Me.Target != lastTarget &&
                            (Death_Grip.IsDistanceGood || Icy_Touch.IsDistanceGood))
                        {
                            Pull();
                            lastTarget = ObjectManager.Me.Target;
                        }

                        if (ObjectManager.Target.Level < 70 && ObjectManager.Me.Level > 84
                            && MySettings.UseLowCombat)
                        {
                            LC = 1;
                            LowCombat();
                        }
                        else
                        {
                            LC = 0;
                            Combat();
                        }
                    }

                    else if (ObjectManager.Me.IsCast)
                        Patrolling();
                }
            }
            catch
            {
            }
            Thread.Sleep(250);
        }
    }

    public void Pull()
    {
        if (Death_Grip.IsSpellUsable && MySettings.UseDeathGrip && Death_Grip.IsDistanceGood)
            Grip();
        else
        {
            if (Icy_Touch.IsSpellUsable && MySettings.UseIcyTouch && Icy_Touch.IsDistanceGood)
                Icy_Touch.Launch();
        }
    }

    private void Buff_Path()
    {
        if (!Fight.InFight && Path_of_Frost.KnownSpell && Path_of_Frost.IsSpellUsable && MySettings.UsePathofFrost
            && (!Path_of_Frost.HaveBuff || Path_of_Frost_Timer.IsReady))
        {
            Path_of_Frost.Launch();
            Path_of_Frost_Timer = new Timer(1000*60*9);
        }
    }

    public void LowCombat()
    {
        Presence();
        AvoidMelee();
        Defense_Cycle();
        Heal();
        Decast();
        Buff();
        Grip();

        if (Howling_Blast.KnownSpell && Howling_Blast.IsSpellUsable && Howling_Blast.IsDistanceGood
            && MySettings.UseHowlingBlast)
        {
            Howling_Blast.Launch();
            if (ObjectManager.Target.HealthPercent < 50 && ObjectManager.Target.HealthPercent > 0)
            {
                Howling_Blast.Launch();
                return;
            }
            return;
        }
        else if (Death_Coil.KnownSpell && Death_Coil.IsSpellUsable && Death_Coil.IsDistanceGood
                 && MySettings.UseDeathCoil)
        {
            Death_Coil.Launch();
            return;
        }
        else
        {
            if (Blood_Boil.KnownSpell && Blood_Boil.IsSpellUsable && Blood_Boil.IsDistanceGood
                && MySettings.UseBloodBoil)
            {
                Blood_Boil.Launch();
                return;
            }
        }
    }

    public void Combat()
    {
        Presence();
        AvoidMelee();
        if (OnCD.IsReady)
            Defense_Cycle();
        Heal();
        Decast();
        Buff();
        DPS_Burst();
        DPS_Cycle();
    }

    public void DPS_Burst()
    {
        if (MySettings.UseTrinket && Trinket_Timer.IsReady && ObjectManager.Target.GetDistance < 30)
        {
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Trinket_Timer = new Timer(1000*60*2);
            return;
        }
        else if (Berserking.IsSpellUsable && Berserking.KnownSpell && MySettings.UseBerserking
                 && ObjectManager.Target.GetDistance < 30)
        {
            Berserking.Launch();
            return;
        }
        else if (Blood_Fury.IsSpellUsable && Blood_Fury.KnownSpell && MySettings.UseBloodFury
                 && ObjectManager.Target.GetDistance < 30)
        {
            Blood_Fury.Launch();
            return;
        }
        else if (Lifeblood.IsSpellUsable && Lifeblood.KnownSpell && MySettings.UseLifeblood
                 && ObjectManager.Target.GetDistance < 30)
        {
            Lifeblood.Launch();
            return;
        }
        else if (MySettings.UseEngGlove && Engineering.KnownSpell && Engineering_Timer.IsReady
                 && ObjectManager.Target.GetDistance < 30)
        {
            Logging.WriteFight("Use Engineering Gloves.");
            Lua.RunMacroText("/use 10");
            Engineering_Timer = new Timer(1000*60);
            return;
        }
        else
        {
            if (Pillar_of_Frost.IsSpellUsable && Pillar_of_Frost.KnownSpell && MySettings.UsePillarofFrost
                && ObjectManager.Target.GetDistance < 30)
            {
                Pillar_of_Frost.Launch();
                return;
            }
        }
    }

    public void DPS_Cycle()
    {
        Grip();

        if (ObjectManager.Me.HealthPercent < 85 && MySettings.UseDeathCoil
            && Lichborne.HaveBuff && Death_Coil.KnownSpell && Death_Coil.IsSpellUsable)
        {
            Lua.RunMacroText("/target Player");
            Death_Coil.Launch();
            return;
        }
        else if (Unholy_Blight.KnownSpell && Unholy_Blight.IsSpellUsable && ObjectManager.Target.GetDistance < 9
                 && MySettings.UseUnholyBlight)
        {
            Unholy_Blight.Launch();
            Blood_Plague_Timer = new Timer(1000*27);
            Frost_Fever_Timer = new Timer(1000*27);
            return;
        }
        else if (Outbreak.KnownSpell && Outbreak.IsSpellUsable && Outbreak.IsDistanceGood && MySettings.UseOutbreak
                 &&
                 (Blood_Plague_Timer.IsReady || Frost_Fever_Timer.IsReady || !Blood_Plague.TargetHaveBuff ||
                  !Frost_Fever.TargetHaveBuff))
        {
            Outbreak.Launch();
            Blood_Plague_Timer = new Timer(1000*27);
            Frost_Fever_Timer = new Timer(1000*27);
            return;
        }
        else if (Blood_Plague_Timer.IsReady && Frost_Fever_Timer.IsReady && Blood_Plague.TargetHaveBuff &&
                 Frost_Fever.TargetHaveBuff
                 && Blood_Boil.IsSpellUsable && Blood_Boil.KnownSpell && ObjectManager.Target.GetDistance < 9
                 && MySettings.UseBloodBoil && Roiling_Blood.KnownSpell)
        {
            Blood_Boil.Launch();
            Blood_Plague_Timer = new Timer(1000*27);
            Frost_Fever_Timer = new Timer(1000*27);
            return;
        }
        else if (Plague_Strike.KnownSpell && Plague_Strike.IsDistanceGood && !Outbreak.IsSpellUsable
                 && !Unholy_Blight.IsSpellUsable && MySettings.UsePlagueStrike
                 && (Blood_Plague_Timer.IsReady || !Blood_Plague.TargetHaveBuff))
        {
            if (!Plague_Strike.IsSpellUsable && Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable
                && MySettings.UseBloodTap)
            {
                Blood_Tap.Launch();
                Thread.Sleep(200);
            }

            if (Plague_Strike.IsSpellUsable)
            {
                Plague_Strike.Launch();
                Blood_Plague_Timer = new Timer(1000*27);
                return;
            }
        }
        else if (Icy_Touch.KnownSpell && Howling_Blast.IsDistanceGood
                 && !Outbreak.IsSpellUsable && !Unholy_Blight.IsSpellUsable
                 && (Frost_Fever_Timer.IsReady || !Frost_Fever.TargetHaveBuff))
        {
            if (!Howling_Blast.IsSpellUsable && Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable
                && MySettings.UseBloodTap)
            {
                Blood_Tap.Launch();
                Thread.Sleep(200);
            }

            if (Howling_Blast.IsSpellUsable && MySettings.UseHowlingBlast)
            {
                Howling_Blast.Launch();
                Frost_Fever_Timer = new Timer(1000*27);
                return;
            }

            else
            {
                if (Icy_Touch.IsSpellUsable && MySettings.UseIcyTouch)
                {
                    Icy_Touch.Launch();
                    Frost_Fever_Timer = new Timer(1000*27);
                    return;
                }
            }
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Pestilence.IsSpellUsable && Pestilence.IsDistanceGood
                 && Pestilence.KnownSpell && MySettings.UsePestilence && !Roiling_Blood.KnownSpell)
        {
            Pestilence.Launch();
            return;
        }
        else if (Freezing_Fog.HaveBuff && Howling_Blast.KnownSpell && Howling_Blast.IsDistanceGood
                 && MySettings.UseHowlingBlast)
        {
            Howling_Blast.Launch();
            Frost_Fever_Timer = new Timer(1000*27);
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && MySettings.UseDeathandDecay
                 && Death_and_Decay.KnownSpell && Death_and_Decay.IsSpellUsable && Death_and_Decay.IsDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(43265, ObjectManager.Target.Position);
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && Army_of_the_Dead.IsSpellUsable &&
                 Army_of_the_Dead.IsDistanceGood
                 && Army_of_the_Dead.KnownSpell && MySettings.UseArmyoftheDead)
        {
            Army_of_the_Dead.Launch();
            Thread.Sleep(4000);
            return;
        }
            // Blizzard API Calls for Frost Strike using Blood Strike Function
        else if (ObjectManager.Me.RunicPowerPercentage >= 90 && MySettings.UseFrostStrike
                 && Blood_Strike.KnownSpell && Blood_Strike.IsSpellUsable && Blood_Strike.IsDistanceGood)
        {
            Blood_Strike.Launch();
            return;
        }
        else
        {
            if (Soul_Reaper.KnownSpell && Soul_Reaper.IsDistanceGood && Soul_Reaper.IsSpellUsable
                && ObjectManager.Target.HealthPercent < 35 && ObjectManager.Me.HealthPercent > 80
                && MySettings.UseSoulReaper)
            {
                Soul_Reaper.Launch();
                return;
            }
        }

        if (MySettings.UseDuelWield)
        {
            if (ObjectManager.Me.HaveBuff(51124) && MySettings.UseFrostStrike
                && Blood_Strike.KnownSpell && Blood_Strike.IsSpellUsable && Blood_Strike.IsDistanceGood)
            {
                if (ObjectManager.Me.HealthPercent < 80 && ((Lichborne.KnownSpell && MySettings.UseLichborne)
                                                            || (Conversion.KnownSpell && MySettings.UseConversion)))
                    return;
                else
                {
                    Blood_Strike.Launch();
                    return;
                }
            }

            else
            {
                if (ObjectManager.Me.HaveBuff(51124) && Obliterate.KnownSpell && Obliterate.IsSpellUsable &&
                    Obliterate.IsDistanceGood
                    && MySettings.UseObliterate)
                {
                    if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseDeathStrike
                        && Death_Strike.KnownSpell && Death_Strike.IsSpellUsable && Death_Strike.IsDistanceGood)
                    {
                        Death_Strike.Launch();
                        return;
                    }
                    Obliterate.Launch();
                    return;
                }
            }
        }
        else
        {
            if (MySettings.UseTwoHander)
            {
                if (ObjectManager.Me.HaveBuff(51124) && Obliterate.KnownSpell && Obliterate.IsSpellUsable &&
                    Obliterate.IsDistanceGood
                    && MySettings.UseObliterate)
                {
                    if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseDeathStrike
                        && Death_Strike.KnownSpell && Death_Strike.IsSpellUsable && Death_Strike.IsDistanceGood)
                    {
                        Death_Strike.Launch();
                        return;
                    }
                    Obliterate.Launch();
                    return;
                }

                else
                {
                    if (ObjectManager.Me.HaveBuff(51124) && MySettings.UseFrostStrike
                        && Blood_Strike.KnownSpell && Blood_Strike.IsSpellUsable && Blood_Strike.IsDistanceGood)
                    {
                        if (ObjectManager.Me.HealthPercent < 80 && ((Lichborne.KnownSpell && MySettings.UseLichborne)
                                                                    ||
                                                                    (Conversion.KnownSpell && MySettings.UseConversion)))
                            return;
                        else
                        {
                            Blood_Strike.Launch();
                            return;
                        }
                    }
                }
            }
        }

        if (Obliterate.KnownSpell && Obliterate.IsSpellUsable && Obliterate.IsDistanceGood
            && MySettings.UseObliterate)
        {
            if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseDeathStrike
                && Death_Strike.KnownSpell && Death_Strike.IsSpellUsable && Death_Strike.IsDistanceGood)
            {
                Death_Strike.Launch();
                return;
            }
            Obliterate.Launch();
            return;
        }
        else if (Blood_Strike.KnownSpell && Blood_Strike.IsSpellUsable && Blood_Strike.IsDistanceGood
                 && MySettings.UseFrostStrike)
        {
            if (ObjectManager.Me.HealthPercent < 80 && ((Lichborne.KnownSpell && MySettings.UseLichborne)
                                                        || (Conversion.KnownSpell && MySettings.UseConversion)))
                return;
            else
            {
                Blood_Strike.Launch();
                return;
            }
        }
        else if (Horn_of_Winter.KnownSpell && Horn_of_Winter.IsSpellUsable && MySettings.UseHornofWinter)
        {
            Horn_of_Winter.Launch();
            return;
        }
        else if (Empower_Rune_Weapon.IsSpellUsable && Empower_Rune_Weapon.KnownSpell && MySettings.UseEmpowerRuneWeapon)
        {
            Empower_Rune_Weapon.Launch();
            return;
        }
        else
        {
            if (Arcane_Torrent.IsSpellUsable && Arcane_Torrent.KnownSpell && MySettings.UseArcaneTorrent)
            {
                Arcane_Torrent.Launch();
                return;
            }
        }
    }

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Presence();
            Heal();
            Buff();
        }
    }

    private void Presence()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Unholy_Presence.KnownSpell && MySettings.UseUnholyPresence && MySettings.UseLowCombat)
        {
            if (!Unholy_Presence.HaveBuff && Unholy_Presence.IsSpellUsable && LC == 1)
                Unholy_Presence.Launch();
        }
        else if (Blood_Presence.KnownSpell && MySettings.UseBloodPresence && ObjectManager.Me.HealthPercent < 30)
        {
            if (!Blood_Presence.HaveBuff && Blood_Presence.IsSpellUsable)
                Blood_Presence.Launch();
        }
        else
        {
            if (Frost_Presence.KnownSpell && MySettings.UseFrostPresence && ObjectManager.Me.HealthPercent > 50)
            {
                if (!Frost_Presence.HaveBuff && Frost_Presence.IsSpellUsable)
                    Frost_Presence.Launch();
            }
        }
    }

    private void Grip()
    {
        if (ObjectManager.Target.GetDistance > 5 &&
            Death_Grip.KnownSpell && Death_Grip.IsSpellUsable && Death_Grip.IsDistanceGood
            && MySettings.UseDeathGrip)
        {
            Death_Grip.Launch();
            MovementManager.StopMove();
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell
            && MySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 55 && MySettings.UseDeathPact
                 && Raise_Dead.KnownSpell && Raise_Dead.IsSpellUsable
                 && Death_Pact.KnownSpell && Death_Pact.IsSpellUsable)
        {
            int i = 0;
            while (i < 3)
            {
                i++;
                Raise_Dead.Launch();
                Death_Pact.Launch();

                if (!Death_Pact.IsSpellUsable)
                {
                    break;
                }
            }
        }
        else if (ObjectManager.Me.HealthPercent < 45 && MySettings.UseLichborne
                 && Lichborne.KnownSpell && Lichborne.IsSpellUsable
                 && Death_Coil.KnownSpell && Death_Coil.IsSpellUsable)
        {
            if (Lichborne.IsSpellUsable)
            {
                Lichborne.Launch();
                return;
            }
        }
        else if (ObjectManager.Me.HealthPercent < 45 && MySettings.UseConversion
                 && Conversion.KnownSpell && Conversion.IsSpellUsable
                 && ObjectManager.Me.RunicPower > 10)
        {
            if (Conversion.IsSpellUsable)
            {
                Conversion.Launch();
                while (ObjectManager.Me.RunicPower > 0)
                    Thread.Sleep(200);
                return;
            }
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 80 && Death_Siphon.KnownSpell && Death_Siphon.IsSpellUsable
                && MySettings.UseDeathSiphon)
            {
                Death_Siphon.Launch();
                return;
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell)
        {
            Stoneform.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseIceboundFortitude
                 && Icebound_Fortitude.KnownSpell && Icebound_Fortitude.IsSpellUsable)
        {
            Icebound_Fortitude.Launch();
            OnCD = new Timer(1000*12);
            return;
        }
        else
        {
            if (ObjectManager.Target.GetDistance < 8 && MySettings.UseRemorselessWinter
                && (ObjectManager.Me.HealthPercent < 70 || ObjectManager.GetNumberAttackPlayer() > 1))
            {
                if (Remorseless_Winter.KnownSpell && Remorseless_Winter.IsSpellUsable)
                {
                    Remorseless_Winter.Launch();
                    OnCD = new Timer(1000*8);
                    return;
                }
            }
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseMindFreeze
            && Mind_Freeze.KnownSpell && Mind_Freeze.IsSpellUsable && Mind_Freeze.IsDistanceGood)
        {
            Mind_Freeze.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseAntiMagicShell
                 && AntiMagic_Shell.KnownSpell && AntiMagic_Shell.IsSpellUsable)
        {
            AntiMagic_Shell.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
                 && Strangulate.KnownSpell && Strangulate.IsSpellUsable && Strangulate.IsDistanceGood
                 && (MySettings.UseStrangulate || MySettings.UseAsphyxiate))
        {
            Strangulate.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && AntiMagic_Zone.KnownSpell
                && MySettings.UseAntiMagicZone && AntiMagic_Zone.IsSpellUsable)
            {
                SpellManager.CastSpellByIDAndPosition(51052, ObjectManager.Me.Position);
                return;
            }
        }

        if (ObjectManager.Target.GetMove && !Chains_of_Ice.TargetHaveBuff && MySettings.UseChainsofIce
            && Chains_of_Ice.KnownSpell && Chains_of_Ice.IsSpellUsable && Chains_of_Ice.IsDistanceGood)
        {
            Chains_of_Ice.Launch();
            return;
        }
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (!Horn_of_Winter.HaveBuff && Horn_of_Winter.KnownSpell && Horn_of_Winter.IsSpellUsable
            && MySettings.UseHornofWinter)
        {
            Horn_of_Winter.Launch();
            return;
        }
        else if (!Fight.InFight && Deaths_Advance.KnownSpell && Deaths_Advance.IsSpellUsable
                 && MySettings.UseDeathsAdvance && ObjectManager.Me.GetMove)
        {
            Deaths_Advance.Launch();
            return;
        }
        else
        {
            if (AlchFlask_Timer.IsReady && MySettings.UseAlchFlask
                && ItemsManager.GetItemCountByIdLUA(75525) == 1)
            {
                Logging.WriteFight("Use Alchi Flask");
                Lua.RunMacroText("/use item:75525");
                AlchFlask_Timer = new Timer(1000*60*60*2);
                return;
            }
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 3 && ObjectManager.Target.InCombat)
        {
            nManager.Wow.Helpers.Keybindings.PressKeybindings(nManager.Wow.Enums.Keybindings.MOVEBACKWARD);
        }
    }
}

#endregion

#region Mage

public class Mage_Frost
{
    #region InitializeSpell

    private Spell Summon_Water_Elemental = new Spell("Summon Water Elemental");
    private Spell Ice_Barrier = new Spell("Ice Barrier");
    private Spell Mana_Shield = new Spell("Mana Shield");
    private Spell Mage_Ward = new Spell("Mage Ward");
    private Spell Frostbolt = new Spell("Frostbolt");
    private Spell Frostfire_Bolt = new Spell("Frostfire Bolt");
    private Spell Ice_Lance = new Spell("Ice Lance");
    private Spell Deep_Freeze = new Spell("Deep Freeze");
    private Spell Frost_Nova = new Spell("Frost Nova");
    private Spell Cone_of_Cold = new Spell("Cone of Cold");
    private Spell Flame_Orb = new Spell("Flame Orb");
    private Spell Freeze = new Spell("Freeze");
    private Spell Ring_of_Frost = new Spell("Ring of Frost");
    private Spell Mirror_Image = new Spell("Mirror Image");
    private Spell Invisibility = new Spell("Invisibility");
    private Spell Fireblast = new Spell("Fireblast");
    private Spell Icy_Veins = new Spell("Icy Veins");
    private Spell Cold_Snap = new Spell("Cold_Snap");
    private Spell Polymorph = new Spell("Polymorph");
    private Spell Ice_Block = new Spell("Ice Block");
    private Spell Counterspell = new Spell("Counterspell");
    private Spell Blink = new Spell("Blink");
    private Spell Evocate = new Spell("Evocate");
    private Spell Arcane_Blast = new Spell("Arcane Blast");
    private Spell Arcane_Explosion = new Spell("Arcane Explosion");
    private Spell Remove_Curse = new Spell("Remove Curse");
    private Spell Mage_Armor = new Spell("Mage Armor");
    private Spell Arcane_Brilliance = new Spell("Arcane Brilliance");
    private Spell Time_Warp = new Spell("Time Warp");
    private Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private Spell ArcaneTorrent = new Spell("Arcane Torrent");
    private Spell Escape_Artist = new Spell("Escape Artist");
    private Spell Heroism = new Spell("Heroism");
    private Spell Dalaran_Brilliance = new Spell("Dalaran Brilliance");
    private Spell Improved_Cone_of_Cold = new Spell("Improved Cone of Cold");

    #endregion InitializeSpell

    public Mage_Frost()
    {
        Main.range = 28.0f;

        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            if (!ObjectManager.Me.IsMounted)
            {
                Patrolling();

                if (Fight.InFight && ObjectManager.Me.Target > 0)
                {
                    if (ObjectManager.Me.Target != lastTarget && ObjectManager.Target.GetDistance <= Main.range)
                    {
                        Pull();
                        lastTarget = ObjectManager.Me.Target;
                    }

                    Combat();
                }
            }
            Thread.Sleep(350);
        }
    }

    public void Pull()
    {
        Pet();
        Buff();
    }

    public void Combat()
    {
        AvoidMelee();
        Blinking();
        Heal();
        Buff();
        Interrupt();
        BuffCombat();

        if (ObjectManager.Me.HaveBuff(44544) || Frost_Nova.TargetHaveBuff || Freeze.TargetHaveBuff ||
            Deep_Freeze.TargetHaveBuff || Improved_Cone_of_Cold.TargetHaveBuff)
        {
            if (Deep_Freeze.KnownSpell && Deep_Freeze.IsSpellUsable && Deep_Freeze.IsDistanceGood)
            {
                Deep_Freeze.Launch();
            }

            else
            {
                if (Ice_Lance.KnownSpell && Ice_Lance.IsSpellUsable && Deep_Freeze.IsDistanceGood)
                {
                    Ice_Lance.Launch();
                    return;
                }
            }
        }


        if (ObjectManager.Me.HaveBuff(57761) &&
            Frostfire_Bolt.KnownSpell &&
            Frostfire_Bolt.IsSpellUsable &&
            Frostfire_Bolt.IsDistanceGood)
        {
            Frostfire_Bolt.Launch();
            return;
        }

        if (Freeze.KnownSpell && Freeze.IsSpellUsable && Freeze.IsDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(33395, ObjectManager.Target.Position);
            return;
        }

        if (ObjectManager.Me.HealthPercent < 10 &&
            Ice_Lance.KnownSpell &&
            Ice_Lance.IsSpellUsable &&
            Ice_Lance.IsDistanceGood)
        {
            Ice_Lance.Launch();
            return;
        }

        if (ObjectManager.Target.GetDistance < 8 &&
            Cone_of_Cold.KnownSpell &&
            Cone_of_Cold.IsDistanceGood &&
            Cone_of_Cold.IsSpellUsable)
        {
            Cone_of_Cold.Launch();
            return;
        }

        if (ObjectManager.Target.GetDistance < 15 &&
            Flame_Orb.KnownSpell &&
            Flame_Orb.IsDistanceGood &&
            Flame_Orb.IsSpellUsable)
        {
            Flame_Orb.Launch();
        }

        if (Frostbolt.KnownSpell &&
            Frostbolt.IsDistanceGood &&
            Frostbolt.IsSpellUsable)
        {
            Frostbolt.Launch();
            return;
        }

        if (Fireblast.KnownSpell &&
            Fireblast.IsDistanceGood &&
            Fireblast.IsSpellUsable)
        {
            Fireblast.Launch();
            return;
        }

        if (Arcane_Blast.KnownSpell &&
            Arcane_Blast.IsDistanceGood &&
            Arcane_Blast.IsSpellUsable)
        {
            Arcane_Blast.Launch();
            return;
        }
    }

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Buff();
            Heal();
        }
    }

    private void Pet()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Summon_Water_Elemental.IsSpellUsable && Summon_Water_Elemental.KnownSpell &&
            (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0))
        {
            Logging.WriteFight(" - PET DEAD - ");
            Summon_Water_Elemental.Launch();
            return;
        }
    }

    private void Buff()
    {
        if (!Arcane_Brilliance.HaveBuff &&
            !Dalaran_Brilliance.HaveBuff &&
            Arcane_Brilliance.KnownSpell &&
            Arcane_Brilliance.IsSpellUsable)
        {
            Arcane_Brilliance.Launch();
        }

        if (!Mage_Armor.HaveBuff &&
            Mage_Armor.KnownSpell &&
            Mage_Armor.IsSpellUsable)
        {
            Mage_Armor.Launch();
        }

        if (!Ice_Barrier.HaveBuff &&
            Ice_Barrier.KnownSpell &&
            Ice_Barrier.IsSpellUsable)
        {
            Ice_Barrier.Launch();
        }

        if (!Mana_Shield.HaveBuff &&
            Mana_Shield.KnownSpell &&
            Mana_Shield.IsSpellUsable)
        {
            Mana_Shield.Launch();
        }

        if (!Mage_Ward.HaveBuff &&
            Mage_Ward.KnownSpell &&
            Mage_Ward.IsSpellUsable)
        {
            Mage_Ward.Launch();
        }
    }

    private void TargetMoving()
    {
        if (ObjectManager.Target.GetDistance >= 11 &&
            ObjectManager.Target.GetMove &&
            !Frostbolt.TargetHaveBuff &&
            !Cone_of_Cold.TargetHaveBuff &&
            Frostbolt.KnownSpell &&
            Frostbolt.IsDistanceGood &&
            Frostbolt.IsSpellUsable)
        {
            Frostbolt.Launch();
        }

        if (ObjectManager.Target.GetDistance <= 11 &&
            ObjectManager.Target.GetMove &&
            !Frostbolt.TargetHaveBuff &&
            !Cone_of_Cold.TargetHaveBuff &&
            Cone_of_Cold.KnownSpell &&
            Cone_of_Cold.IsDistanceGood &&
            Cone_of_Cold.IsSpellUsable)
        {
            Cone_of_Cold.Launch();
        }

        if (ObjectManager.Target.GetDistance <= 13 &&
            (ObjectManager.GetNumberAttackPlayer() >= 2 ||
             ObjectManager.Target.GetMove) &&
            !Frost_Nova.TargetHaveBuff &&
            Frost_Nova.KnownSpell &&
            Frost_Nova.IsDistanceGood &&
            Frost_Nova.IsSpellUsable)
        {
            Frost_Nova.Launch();
        }
    }

    private void Heal()
    {
        if (Ice_Block.KnownSpell &&
            ObjectManager.Me.HealthPercent > 0 &&
            ObjectManager.Me.HealthPercent < 25 &&
            ObjectManager.GetNumberAttackPlayer() >= 1 &&
            Ice_Block.IsSpellUsable)
        {
            Ice_Block.Launch();
        }

        if (Gift_of_the_Naaru.KnownSpell &&
            ObjectManager.Me.HealthPercent > 0 &&
            ObjectManager.Me.HealthPercent < 50 &&
            Gift_of_the_Naaru.IsSpellUsable)
        {
            Gift_of_the_Naaru.Launch();
        }

        if (Evocate.KnownSpell &&
            ObjectManager.Me.HealthPercent > 0 &&
            ObjectManager.Me.HealthPercent < 35 &&
            (ObjectManager.Me.HaveBuff(11426) || ObjectManager.Me.HaveBuff(1463) ||
             ObjectManager.GetNumberAttackPlayer() <= 2) &&
            Evocate.IsSpellUsable)
        {
            Evocate.Launch();
        }

        if (Ring_of_Frost.KnownSpell && Ring_of_Frost.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() >= 2 &&
            ObjectManager.Target.GetDistance <= 15)
        {
            SpellManager.CastSpellByIDAndPosition(82676, ObjectManager.Me.Position);
        }
    }

    private void BuffCombat()
    {
        if (ObjectManager.GetNumberAttackPlayer() <= 3 &&
            Icy_Veins.KnownSpell &&
            Icy_Veins.IsSpellUsable &&
            Icy_Veins.IsDistanceGood)
        {
            Icy_Veins.Launch();
        }

        if (!Time_Warp.HaveBuff &&
            !Heroism.HaveBuff &&
            ObjectManager.GetNumberAttackPlayer() <= 3 &&
            Time_Warp.KnownSpell &&
            Time_Warp.IsSpellUsable &&
            Time_Warp.IsDistanceGood)
        {
            Time_Warp.Launch();
        }
    }

    private void Interrupt()
    {
        if (ObjectManager.Target.IsCast &&
            Counterspell.KnownSpell &&
            Counterspell.IsSpellUsable &&
            Counterspell.IsDistanceGood)
        {
            Counterspell.Launch();
        }
    }

    private void Blinking()
    {
        if ((ObjectManager.Me.HaveBuff(44572) || ObjectManager.Me.HaveBuff(408) || ObjectManager.Me.HaveBuff(65929) ||
             ObjectManager.Me.HaveBuff(85388) ||
             ObjectManager.Me.HaveBuff(30283)) &&
            Blink.KnownSpell &&
            Blink.IsSpellUsable)
        {
            Blink.Launch();
        }

        if (ObjectManager.Target.GetDistance > 35 &&
            Blink.KnownSpell &&
            Blink.IsSpellUsable)
        {
            Blink.Launch();
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 1)
        {
            Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "{DOWN}");
        }
    }
}

public class Mage_Arcane
{
    #region InitializeSpell

    // Arcane Only
    private Spell Arcane_Barrage = new Spell("Arcane Barrage");
    private Spell Arcane_Blast = new Spell("Arcane Blast");
    private Spell Arcane_Explosion = new Spell("Arcane Explosion");
    private Spell Presence_of_Mind = new Spell("Presence of Mind");
    private Spell Slow = new Spell("Slow");

    // Survive
    private Spell Mana_Shield = new Spell("Mana Shield");
    private Spell Mage_Ward = new Spell("Mage Ward");
    private Spell Ring_of_Frost = new Spell("Ring of Frost");
    private Spell Frost_Nova = new Spell("Frost Nova");
    private Spell Blink = new Spell("Blink");
    private Spell Counterspell = new Spell("Counterspell");
    private Spell Frostbolt = new Spell("Frostbolt");

    // DPS
    private Spell Frostfire_Bolt = new Spell("Frostfire Bolt");
    private Spell Fireball = new Spell("Fireball");
    private Spell Flame_Orb = new Spell("Flame Orb");
    private Spell Fire_Blast = new Spell("Fire Blast");
    private Spell Arcane_Missiles = new Spell("Arcane Missiles");

    // BUFF & HELPING
    private Spell Evocation = new Spell("Evocation");
    private Spell Conjure_Refreshment = new Spell("Conjure Refreshment");
    private Spell Arcane_Brilliance = new Spell("Arcane Brilliance");
    private Spell Remove_Curse = new Spell("Remove Curse");
    private Spell Mage_Armor = new Spell("Mage Armor");
    private Spell Molten_Armor = new Spell("Molten Armor");
    private Spell Conjure_Mana_Gem = new Spell("Conjure Mana Gem");

    // BIG CD
    private Spell Mirror_Image = new Spell("Mirror Image");
    private Spell Time_Warp = new Spell("Time Warp");
    private Spell Invisibility = new Spell("Invisibility");
    private Spell Ice_Block = new Spell("Ice Block");
    private Spell Cold_Snap = new Spell("Cold Snap");

    // TIMER
    private Timer freeze = new Timer(0);
    private Timer look = new Timer(0);
    private Timer fighttimer = new Timer(0);
    private Timer waitfordebuff = new Timer(0);

    // Profession & Racials
    private Spell ArcaneTorrent = new Spell("Arcane Torrent");
    private Spell Lifeblood = new Spell("Lifeblood");
    private Spell Stoneform = new Spell("Stoneform");
    private Spell Tailoring = new Spell("Tailoring");
    private Spell Leatherworking = new Spell("Leatherworking");
    private Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private Spell War_Stomp = new Spell("War Stomp");
    private Spell Berserking = new Spell("Berserking");

    #endregion InitializeSpell

    public Mage_Arcane()
    {
        Main.range = 30.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            if (!ObjectManager.Me.IsMounted)
            {
                buffoutfight();

                if (!Fight.InFight && look.IsReady)
                {
                    look = new Timer(5000);
                    Lua.RunMacroText("/targetfriendplayer");
                }

                if (Fight.InFight && ObjectManager.Me.Target > 0 && ObjectManager.Target.GetDistance > Main.range)
                {
                    fighttimer = new Timer(60000);
                }

                if (Fight.InFight && ObjectManager.Me.Target > 0)
                {
                    if (ObjectManager.Me.Target != lastTarget && ObjectManager.Target.GetDistance <= Main.range)
                    {
                        pull();
                        lastTarget = ObjectManager.Me.Target;
                    }
                    fight();
                    if (!Fight.InFight)
                    {
                        Logging.WriteFight(" - Target Down - ");
                        look = new Timer(5000);
                    }

                    if (fighttimer.IsReady && ObjectManager.Target.HealthPercent > 90 && ObjectManager.Me.Target > 0 &&
                        ObjectManager.GetNumberAttackPlayer() < 2)
                    {
                        Logging.WriteFight(" - Target Evading - ");
                        break;
                    }
                }
            }
            Thread.Sleep(350);
        }
    }

    public void pull()
    {
        if (hardmob()) Logging.WriteFight(" -  Pull Hard Mob - ");
        if (!hardmob()) Logging.WriteFight(" -  Pull Easy Mob - ");
        fighttimer = new Timer(60000);
        if (ObjectManager.Target.GetDistance > 25 && !Slow.KnownSpell)
            SpellManager.CastSpellByIdLUA(116); //Frostbolt.Launch();
    }

    public void buffoutfight()
    {
        if (Fight.InFight || ObjectManager.Me.IsDeadMe) return;

        if (Evocation.KnownSpell && Evocation.IsSpellUsable &&
            ObjectManager.Me.HealthPercent < 40 ||
            ObjectManager.Me.ManaPercentage < 40)
        {
            SpellManager.CastSpellByIdLUA(12051);
            // Evocation.Launch();
        }

        if (!ObjectManager.Me.HaveBuff(79640) &&
            ItemsManager.GetItemCountByIdLUA(58149) == 1)
        {
            Logging.WriteFight("Use Alchi Flask");
            Lua.RunMacroText("/use item:58149");
        }

        if (Arcane_Brilliance.KnownSpell && Arcane_Brilliance.IsSpellUsable &&
            !Arcane_Brilliance.HaveBuff)
        {
            Arcane_Brilliance.Launch();
        }

        if (Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65499) == 0 && // 85
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(43523) == 0 && // 84-80
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(43518) == 0 && // 79-74
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65517) == 0 && // 73-64
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65516) == 0 && // 63-54
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65515) == 0 && // 53-44
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65500) == 0) // 43-38
        {
            Thread.Sleep(100);
            Conjure_Refreshment.Launch();
            Thread.Sleep(200);
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            Thread.Sleep(100);
        }

        if (Conjure_Mana_Gem.KnownSpell && ItemsManager.GetItemCountByIdLUA(36799) == 0)
        {
            Thread.Sleep(100);
            Conjure_Mana_Gem.Launch();
            Thread.Sleep(200);
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            Thread.Sleep(100);
        }

        if (Mage_Armor.KnownSpell && Mage_Armor.IsSpellUsable && !Mage_Armor.HaveBuff)
        {
            Mage_Armor.Launch();
        }

        if (!Mage_Armor.KnownSpell && Molten_Armor.KnownSpell && !Molten_Armor.HaveBuff)
        {
            Molten_Armor.Launch();
        }
    }

    public void fight()
    {
        selfheal();
        buffinfight();
        if (ObjectManager.GetNumberAttackPlayer() > 1) fighttimer = new Timer(60000);

        if (Mirror_Image.KnownSpell && Mirror_Image.IsSpellUsable && hardmob() ||
            ObjectManager.GetNumberAttackPlayer() > 1)
        {
            SpellManager.CastSpellByIdLUA(55342);
            // Mirror_Image.Launch();
        }

        if (Berserking.KnownSpell && Berserking.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1454);
            // Berserking.Launch();
        }

        if (Arcane_Blast.KnownSpell && Arcane_Blast.IsDistanceGood && Arcane_Blast.IsSpellUsable &&
            ObjectManager.Target.GetDistance > 8)
        {
            SpellManager.CastSpellByIdLUA(30451);
            // Arcane_Blast.Launch();
            return;
        }

        if (Arcane_Blast.KnownSpell && Arcane_Blast.IsDistanceGood && Arcane_Blast.IsSpellUsable &&
            !Arcane_Missiles.IsSpellUsable && !Arcane_Barrage.IsSpellUsable && !Fire_Blast.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(30451);
            // Arcane_Blast.Launch();
            return;
        }

        if (!Arcane_Blast.KnownSpell && Frostbolt.KnownSpell && Frostbolt.IsDistanceGood && Frostbolt.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(116);
            // Frostbolt.Launch();
        }

        if (!Frostbolt.KnownSpell && Fireball.IsSpellUsable && Fireball.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(133);
            // Fireball.Launch();
        }

        if (Arcane_Missiles.KnownSpell &&
            Arcane_Missiles.IsSpellUsable &&
            Arcane_Missiles.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(5143);
            // Arcane_Missiles.Launch();
            Thread.Sleep(200);
            while (ObjectManager.Me.IsCast)
            {
                Fight.StopFight();
                MovementManager.StopMove();
                Thread.Sleep(200);
            }
        }

        if (Slow.KnownSpell && Slow.IsDistanceGood && Slow.IsSpellUsable && !Slow.TargetHaveBuff &&
            ObjectManager.Target.IsTargetingMe)
        {
            SpellManager.CastSpellByIdLUA(31589);
            // Slow.Launch();
        }

        if (Arcane_Barrage.KnownSpell && Arcane_Barrage.IsSpellUsable && Arcane_Barrage.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(44425);
            // Arcane_Barrage.Launch();	
        }

        if (ObjectManager.Target.GetDistance < 8 &&
            Fire_Blast.KnownSpell &&
            Fire_Blast.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(2136);
            // Fire_Blast.Launch();
        }

        if (Counterspell.KnownSpell && Counterspell.IsSpellUsable && Counterspell.IsDistanceGood &&
            ObjectManager.Target.IsCast && ObjectManager.Target.HealthPercent > 30)
        {
            SpellManager.CastSpellByIdLUA(2139);
            // Counterspell.Launch();
        }

        if (ArcaneTorrent.KnownSpell && ArcaneTorrent.IsSpellUsable &&
            ObjectManager.Target.IsCast && ObjectManager.Target.GetDistance < 8)
        {
            ArcaneTorrent.Launch();
        }

        if (Arcane_Explosion.KnownSpell && Arcane_Explosion.IsSpellUsable &&
            ObjectManager.Target.GetDistance < 8 &&
            ObjectManager.GetNumberAttackPlayer() > 3)
        {
            SpellManager.CastSpellByIdLUA(1449);
            // Arcane_Explosion.Launch();
        }

        if (Flame_Orb.KnownSpell && Flame_Orb.IsDistanceGood && Flame_Orb.IsSpellUsable && hardmob())
        {
            SpellManager.CastSpellByIdLUA(82731);
            // Flame_Orb.Launch();
        }
    }

    private void buffinfight()
    {
        if (!Fight.InFight) return;

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            Stoneform.KnownSpell && Stoneform.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(20594);
            // Stoneform.Launch();
        }

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            War_Stomp.KnownSpell && War_Stomp.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(20549);
            // War_Stomp.Launch();
        }

        if (Presence_of_Mind.KnownSpell && Presence_of_Mind.IsSpellUsable && ObjectManager.Target.IsTargetingMe)
        {
            SpellManager.CastSpellByIdLUA(12043);
            // Presence_of_Mind.Launch();
        }

        if (Time_Warp.KnownSpell && Time_Warp.IsSpellUsable &&
            !Time_Warp.HaveBuff && hardmob() && ObjectManager.GetNumberAttackPlayer() > 2)
        {
            SpellManager.CastSpellByIdLUA(80353);
            // Time_Warp.Launch();
        }

        if (Frost_Nova.KnownSpell && Frost_Nova.IsSpellUsable && ObjectManager.Target.GetDistance < 6)
        {
            SpellManager.CastSpellByIdLUA(122);
            // Frost_Nova.Launch();
        }

        if (Blink.KnownSpell && Blink.IsSpellUsable && Fight.InFight && !Frost_Nova.IsSpellUsable &&
            ObjectManager.Target.GetDistance < 6 && ObjectManager.Target.HealthPercent > 30)
        {
            SpellManager.CastSpellByIdLUA(1953);
            // Blink.Launch();	
        }

        if (ObjectManager.Target.GetDistance > 55 && Blink.KnownSpell && Blink.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1953);
            // Blink.Launch();	
        }
    }

    private void selfheal()
    {
        if (Mage_Ward.KnownSpell && Mage_Ward.IsSpellUsable && ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe && !Counterspell.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(543);
            // Mage_Ward.Launch();
        }

        if (ObjectManager.Me.ManaPercentage < 40 &&
            ItemsManager.GetItemCountByIdLUA(36799) > 0)
        {
            Lua.RunMacroText("/use item:36799");
        }


        if (!Mana_Shield.HaveBuff && ObjectManager.Me.HealthPercent < 85 &&
            Mana_Shield.KnownSpell && Mana_Shield.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1463);
            // Mana_Shield.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 80 &&
            Lifeblood.KnownSpell && Lifeblood.IsSpellUsable)
        {
            Lifeblood.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 80 &&
            Gift_of_the_Naaru.KnownSpell && Gift_of_the_Naaru.IsSpellUsable)
        {
            Gift_of_the_Naaru.Launch();
        }

        if (Ice_Block.KnownSpell && Ice_Block.IsSpellUsable && ObjectManager.Me.HealthPercent > 0 &&
            ObjectManager.Me.HealthPercent < 25 && ObjectManager.GetNumberAttackPlayer() > 1 &&
            Fight.InFight)
        {
            SpellManager.CastSpellByIdLUA(45438);
            // Ice_Block.Launch();
        }

        if (Evocation.KnownSpell && Evocation.IsSpellUsable &&
            ObjectManager.Me.HealthPercent > 0 &&
            ObjectManager.Me.HealthPercent < 30 ||
            ObjectManager.Me.ManaPercentage < 20)
        {
            SpellManager.CastSpellByIdLUA(12051);
            // Evocation.Launch();
        }

        if (Ring_of_Frost.KnownSpell && Ring_of_Frost.IsSpellUsable &&
            ObjectManager.GetNumberAttackPlayer() > 2 &&
            ObjectManager.Target.GetDistance < 10 && hardmob())
        {
            SpellManager.CastSpellByIDAndPosition(82676, ObjectManager.Target.Position);
        }
    }

    public bool hardmob()
    {
        if (((ObjectManager.Target.MaxHealth*100)/ObjectManager.Me.MaxHealth) > 140)
        {
            return true;
        }
        return false;
    }
}

public class Mage_Fire
{
    #region InitializeSpell

    // Fire Only
    private Spell Pyroblast = new Spell("Pyroblast");
    private Spell Blast_Wave = new Spell("Blast Wave");
    private Spell Combustion = new Spell("Combustion");
    private Spell Dragons_Breath = new Spell("Dragon's Breath");
    private Spell Living_Bomb = new Spell("Living Bomb");
    private Spell Hot_Streak = new Spell("Hot Streak");
    private Spell Flamestrike = new Spell("Flamestrike");
    private Spell Scorch = new Spell("Scorch");

    // Survive
    private Spell Mana_Shield = new Spell("Mana Shield");
    private Spell Mage_Ward = new Spell("Mage Ward");
    private Spell Ring_of_Frost = new Spell("Ring of Frost");
    private Spell Frost_Nova = new Spell("Frost Nova");
    private Spell Blink = new Spell("Blink");
    private Spell Counterspell = new Spell("Counterspell");
    private Spell Frostbolt = new Spell("Frostbolt");

    // DPS
    private Spell Frostfire_Bolt = new Spell("Frostfire Bolt");
    private Spell Fireball = new Spell("Fireball");
    private Spell Flame_Orb = new Spell("Flame Orb");
    private Spell Fire_Blast = new Spell("Fire Blast");
    private Spell Arcane_Missiles = new Spell("Arcane Missiles");

    // BUFF & HELPING
    private Spell Evocation = new Spell("Evocation");
    private Spell Conjure_Refreshment = new Spell("Conjure Refreshment");
    private Spell Arcane_Brilliance = new Spell("Arcane Brilliance");
    private Spell Remove_Curse = new Spell("Remove Curse");
    private Spell Mage_Armor = new Spell("Mage Armor");
    private Spell Molten_Armor = new Spell("Molten Armor");
    private Spell Conjure_Mana_Gem = new Spell("Conjure Mana Gem");

    // BIG CD
    private Spell Mirror_Image = new Spell("Mirror Image");
    private Spell Time_Warp = new Spell("Time Warp");
    private Spell Invisibility = new Spell("Invisibility");
    private Spell Ice_Block = new Spell("Ice Block");
    private Spell Cold_Snap = new Spell("Cold Snap");

    // TIMER
    private Timer freeze = new Timer(0);
    private Timer look = new Timer(0);
    private Timer fighttimer = new Timer(0);
    private Timer waitfordebuff = new Timer(0);

    // Profession & Racials
    private Spell ArcaneTorrent = new Spell("Arcane Torrent");
    private Spell Lifeblood = new Spell("Lifeblood");
    private Spell Stoneform = new Spell("Stoneform");
    private Spell Tailoring = new Spell("Tailoring");
    private Spell Leatherworking = new Spell("Leatherworking");
    private Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private Spell War_Stomp = new Spell("War Stomp");
    private Spell Berserking = new Spell("Berserking");

    #endregion InitializeSpell

    public Mage_Fire()
    {
        Main.range = 30.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            if (!ObjectManager.Me.IsMounted)
            {
                buffoutfight();

                if (!Fight.InFight && look.IsReady)
                {
                    look = new Timer(5000);
                    Lua.RunMacroText("/targetfriendplayer");
                }

                if (Fight.InFight && ObjectManager.Me.Target > 0 && ObjectManager.Target.GetDistance > Main.range)
                {
                    fighttimer = new Timer(60000);
                }

                if (Fight.InFight && ObjectManager.Me.Target > 0)
                {
                    if (ObjectManager.Me.Target != lastTarget && ObjectManager.Target.GetDistance <= Main.range)
                    {
                        pull();
                        lastTarget = ObjectManager.Me.Target;
                    }
                    fight();
                    if (!Fight.InFight)
                    {
                        Logging.WriteFight(" - Target Down - ");
                        look = new Timer(5000);
                    }

                    if (fighttimer.IsReady && ObjectManager.Target.HealthPercent > 90 && ObjectManager.Me.Target > 0 &&
                        ObjectManager.GetNumberAttackPlayer() < 2)
                    {
                        Logging.WriteFight(" - Target Evading - ");
                        break;
                    }
                }
            }
            Thread.Sleep(350);
        }
    }

    public void pull()
    {
        if (hardmob()) Logging.WriteFight(" -  Pull Hard Mob - ");
        if (!hardmob()) Logging.WriteFight(" -  Pull Easy Mob - ");
        fighttimer = new Timer(60000);
        if (ObjectManager.Target.GetDistance > 25 && !Frostfire_Bolt.KnownSpell)
            SpellManager.CastSpellByIdLUA(116); //Frostbolt.Launch();
        if (ObjectManager.Target.GetDistance > 25) SpellManager.CastSpellByIdLUA(44614); //Frostfirebolt.Launch();
    }

    public void buffoutfight()
    {
        if (Fight.InFight || ObjectManager.Me.IsDeadMe) return;

        if (Evocation.KnownSpell && Evocation.IsSpellUsable &&
            ObjectManager.Me.HealthPercent < 40 ||
            ObjectManager.Me.ManaPercentage < 40)
        {
            SpellManager.CastSpellByIdLUA(12051);
            // Evocation.Launch();
        }

        if (!ObjectManager.Me.HaveBuff(79640) &&
            ItemsManager.GetItemCountByIdLUA(58149) == 1)
        {
            Logging.WriteFight("Use Alchi Flask");
            Lua.RunMacroText("/use item:58149");
        }

        if (Arcane_Brilliance.KnownSpell && Arcane_Brilliance.IsSpellUsable &&
            !Arcane_Brilliance.HaveBuff)
        {
            Arcane_Brilliance.Launch();
        }

        if (Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65499) == 0 && // 85
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(43523) == 0 && // 84-80
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(43518) == 0 && // 79-74
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65517) == 0 && // 73-64
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65516) == 0 && // 63-54
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65515) == 0 && // 53-44
            Conjure_Refreshment.KnownSpell && ItemsManager.GetItemCountByIdLUA(65500) == 0) // 43-38
        {
            Thread.Sleep(100);
            Conjure_Refreshment.Launch();
            Thread.Sleep(200);
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            Thread.Sleep(100);
        }

        if (Conjure_Mana_Gem.KnownSpell && ItemsManager.GetItemCountByIdLUA(36799) == 0)
        {
            Thread.Sleep(100);
            Conjure_Mana_Gem.Launch();
            Thread.Sleep(200);
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            Thread.Sleep(100);
        }

        if (Mage_Armor.KnownSpell && Mage_Armor.IsSpellUsable && !Mage_Armor.HaveBuff)
        {
            Mage_Armor.Launch();
        }

        if (!Mage_Armor.KnownSpell && Molten_Armor.KnownSpell && !Molten_Armor.HaveBuff)
        {
            Molten_Armor.Launch();
        }
    }

    public void fight()
    {
        selfheal();
        buffinfight();
        if (ObjectManager.GetNumberAttackPlayer() > 1) fighttimer = new Timer(60000);

        if (Mirror_Image.KnownSpell && Mirror_Image.IsSpellUsable && hardmob() ||
            ObjectManager.GetNumberAttackPlayer() > 1)
        {
            SpellManager.CastSpellByIdLUA(55342);
            // Mirror_Image.Launch();
        }

        if (Berserking.KnownSpell && Berserking.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1454);
            // Berserking.Launch();
        }

        if (Fireball.IsDistanceGood && Fireball.IsSpellUsable && !ObjectManager.Target.IsTargetingMe)
        {
            SpellManager.CastSpellByIdLUA(133);
            // Fireball.Launch();
            return;
        }

        if (Pyroblast.IsDistanceGood && Pyroblast.IsSpellUsable && ObjectManager.Me.HaveBuff(48108))
        {
            SpellManager.CastSpellByIdLUA(92315);
            // Pyroblast.Launch();
            return;
        }

        if (Fire_Blast.KnownSpell && Fire_Blast.IsSpellUsable && Fire_Blast.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(2136);
            // Fire_Blast.Launch();
        }

        if (Dragons_Breath.KnownSpell && Dragons_Breath.IsSpellUsable && Dragons_Breath.IsDistanceGood &&
            ObjectManager.Target.GetDistance < 6)
        {
            SpellManager.CastSpellByIdLUA(31661);
            // Dragons_Breath.Launch();
        }

        if (Fireball.IsSpellUsable && Fireball.IsDistanceGood && ObjectManager.Target.GetDistance > 5)
        {
            SpellManager.CastSpellByIdLUA(133);
            // Fireball.Launch();
        }

        if (Scorch.KnownSpell && Scorch.IsSpellUsable && Scorch.IsDistanceGood && ObjectManager.Target.GetDistance < 6)
        {
            SpellManager.CastSpellByIdLUA(2948);
            // Scorch.Launch();
        }

        if (Living_Bomb.KnownSpell && Living_Bomb.IsSpellUsable && Living_Bomb.IsDistanceGood &&
            !Living_Bomb.TargetHaveBuff && hardmob())
        {
            SpellManager.CastSpellByIdLUA(44457);
            // Living_Bomb.Launch();
        }

        if (Arcane_Missiles.KnownSpell &&
            Arcane_Missiles.IsSpellUsable &&
            Arcane_Missiles.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(5143);
            // Arcane_Missiles.Launch();
            Thread.Sleep(200);
            while (ObjectManager.Me.IsCast)
            {
                Fight.StopFight();
                MovementManager.StopMove();
                Thread.Sleep(200);
            }
        }

        if (Counterspell.KnownSpell && Counterspell.IsSpellUsable && Counterspell.IsDistanceGood &&
            ObjectManager.Target.IsCast && ObjectManager.Target.HealthPercent > 30)
        {
            SpellManager.CastSpellByIdLUA(2139);
            // Counterspell.Launch();
        }

        if (ArcaneTorrent.KnownSpell && ArcaneTorrent.IsSpellUsable &&
            ObjectManager.Target.IsCast && ObjectManager.Target.GetDistance < 8)
        {
            ArcaneTorrent.Launch();
        }

        if (Flamestrike.KnownSpell && Flamestrike.IsSpellUsable &&
            ObjectManager.Target.GetDistance < 8 &&
            !ObjectManager.Target.HaveBuff(2120) &&
            ObjectManager.GetNumberAttackPlayer() > 2)
        {
            SpellManager.CastSpellByIDAndPosition(2120, ObjectManager.Target.Position);
        }

        if (Flame_Orb.KnownSpell && Flame_Orb.IsDistanceGood && Flame_Orb.IsSpellUsable && hardmob())
        {
            SpellManager.CastSpellByIdLUA(82731);
            // Flame_Orb.Launch();
        }
    }

    private void buffinfight()
    {
        if (!Fight.InFight) return;

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            Stoneform.KnownSpell && Stoneform.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(20594);
            // Stoneform.Launch();
        }

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            War_Stomp.KnownSpell && War_Stomp.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(20549);
            // War_Stomp.Launch();
        }

        if (ObjectManager.Target.HaveBuff(12846) &&
            Combustion.KnownSpell && Combustion.IsSpellUsable && hardmob())
        {
            SpellManager.CastSpellByIdLUA(11129);
            // Combustion.Launch();
        }

        if (Time_Warp.KnownSpell && Time_Warp.IsSpellUsable &&
            !Time_Warp.HaveBuff && hardmob() && ObjectManager.GetNumberAttackPlayer() > 2)
        {
            SpellManager.CastSpellByIdLUA(80353);
            // Time_Warp.Launch();
        }

        if (Blast_Wave.KnownSpell && Blast_Wave.IsSpellUsable && ObjectManager.Target.IsTargetingMe)
        {
            SpellManager.CastSpellByIDAndPosition(11113, ObjectManager.Target.Position);
            return;
        }

        if (Frost_Nova.KnownSpell && Frost_Nova.IsSpellUsable && ObjectManager.Target.GetDistance < 6)
        {
            SpellManager.CastSpellByIdLUA(122);
            // Frost_Nova.Launch();
        }

        if (Blink.KnownSpell && Blink.IsSpellUsable && Fight.InFight && !Frost_Nova.IsSpellUsable &&
            ObjectManager.Target.GetDistance < 6 && ObjectManager.Target.HealthPercent > 30)
        {
            SpellManager.CastSpellByIdLUA(1953);
            // Blink.Launch();	
        }

        if (ObjectManager.Target.GetDistance > 55 && Blink.KnownSpell && Blink.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1953);
            // Blink.Launch();	
        }
    }

    private void selfheal()
    {
        if (Mage_Ward.KnownSpell && Mage_Ward.IsSpellUsable && ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe && !Counterspell.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(543);
            // Mage_Ward.Launch();
        }

        if (ObjectManager.Me.ManaPercentage < 40 &&
            ItemsManager.GetItemCountByIdLUA(36799) > 0)
        {
            Lua.RunMacroText("/use item:36799");
        }


        if (!Mana_Shield.HaveBuff && ObjectManager.Me.HealthPercent < 85 &&
            Mana_Shield.KnownSpell && Mana_Shield.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1463);
            // Mana_Shield.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 80 &&
            Lifeblood.KnownSpell && Lifeblood.IsSpellUsable)
        {
            Lifeblood.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 80 &&
            Gift_of_the_Naaru.KnownSpell && Gift_of_the_Naaru.IsSpellUsable)
        {
            Gift_of_the_Naaru.Launch();
        }

        if (Ice_Block.KnownSpell && Ice_Block.IsSpellUsable && ObjectManager.Me.HealthPercent > 0 &&
            ObjectManager.Me.HealthPercent < 25 && ObjectManager.GetNumberAttackPlayer() > 1 &&
            Fight.InFight)
        {
            SpellManager.CastSpellByIdLUA(45438);
            // Ice_Block.Launch();
        }

        if (Evocation.KnownSpell && Evocation.IsSpellUsable &&
            ObjectManager.Me.HealthPercent > 0 &&
            ObjectManager.Me.HealthPercent < 30 ||
            ObjectManager.Me.ManaPercentage < 20)
        {
            SpellManager.CastSpellByIdLUA(12051);
            // Evocation.Launch();
        }

        if (Ring_of_Frost.KnownSpell && Ring_of_Frost.IsSpellUsable &&
            ObjectManager.GetNumberAttackPlayer() > 2 &&
            ObjectManager.Target.GetDistance < 10 && hardmob())
        {
            SpellManager.CastSpellByIDAndPosition(82676, ObjectManager.Target.Position);
        }
    }

    public bool hardmob()
    {
        if (((ObjectManager.Target.MaxHealth*100)/ObjectManager.Me.MaxHealth) > 120)
        {
            return true;
        }
        return false;
    }
}

#endregion

#region Warlock

public class Demo
{
    #region InitializeSpell

    private Spell Immolate = new Spell("Immolate");
    private Spell Soul_Fire = new Spell("Soul Fire");
    private Spell Bane_of_Doom = new Spell("Bane of Doom");
    private Spell Shadow_Bolt = new Spell("Shadow Bolt");
    private Spell Shadowflame = new Spell("Shadowflame");
    private Spell Incinerate = new Spell("Incinerate");
    private Spell Health_Funnel = new Spell("Health Funnel");
    private Spell Life_Tap = new Spell("Life Tap");
    private Spell Drain_Soul = new Spell("Drain Soul");
    private Spell Corruption = new Spell("Corruption");
    private Spell Curse_of_the_Elements = new Spell("Curse of the Elements");
    private Spell Drain_Life = new Spell("Drain Life");
    private Spell Metamorphosis = new Spell("Metamorphosis");
    private Spell Immolation_Aura = new Spell("Immolation Aura");
    private Spell Demon_Soul = new Spell("Demon Soul");
    private Spell Demon_Leap = new Spell("Demon Leap");
    private Spell Summon_Imp = new Spell("Summon Imp");
    private Spell Summon_Felguard = new Spell("Summon Felguard");
    private Spell Summon_Infernal = new Spell("Summon Infernal");
    private Spell Death_Coil = new Spell("Death Coil");
    private Spell Soul_Link = new Spell("Soul Link");
    private Spell Curse_of_Weakness = new Spell("Curse of Weakness");
    private Spell Curse_of_Tongues = new Spell("Curse of Tongues");
    private Spell Hand_of_Guldan = new Spell("Hand of Gul'dan");
    private Spell Curse_of_Guldan = new Spell("Curse of Gul'dan");
    private Spell Fel_Domination = new Spell("Fel Domination");
    private Spell Soul_Harvest = new Spell("Soul Harvest");
    private Spell Create_Healthstone = new Spell("Create Healthstone");
    private Spell Fel_Armor = new Spell("Fel Armor");
    private Spell Demon_Armor = new Spell("Demon Armor");
    private Spell Molten_Core = new Spell("Molten Core");
    private Spell Soulburn = new Spell("Soulburn");
    private Spell Dark_Intent = new Spell("Dark Intent");
    private Timer look = new Timer(0);
    private Timer petchill = new Timer(0);
    private Timer fighttimer = new Timer(0);
    private Timer waitfordebuff = new Timer(0);
    private Timer mountchill = new Timer(0);

    // profession & racials
    private Spell ArcaneTorrent = new Spell("Arcane Torrent");
    private Spell Lifeblood = new Spell("Lifeblood");
    private Spell Stoneform = new Spell("Stoneform");
    private Spell Tailoring = new Spell("Tailoring");
    private Spell Leatherworking = new Spell("Leatherworking");
    private Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private Spell War_Stomp = new Spell("War Stomp");
    private Spell Berserking = new Spell("Berserking");

    #endregion InitializeSpell

    public Demo()
    {
        Main.range = 30.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            if (!ObjectManager.Me.IsMounted)
            {
                buffoutfight();

                if (!Fight.InFight && look.IsReady)
                {
                    look = new Timer(5000);
                    Lua.RunMacroText("/targetfriendplayer");
                }

                if (Fight.InFight && ObjectManager.Me.Target > 0 && ObjectManager.Target.GetDistance > Main.range)
                {
                    fighttimer = new Timer(60000);
                }

                if (Fight.InFight && ObjectManager.Me.Target > 0)
                {
                    if (ObjectManager.Me.Target != lastTarget && ObjectManager.Target.GetDistance <= Main.range)
                    {
                        pull();
                        lastTarget = ObjectManager.Me.Target;
                    }
                    fight();
                    if (!Fight.InFight)
                    {
                        Logging.WriteFight(" - Target Down - ");
                        look = new Timer(5000);
                    }

                    if (fighttimer.IsReady && ObjectManager.Target.HealthPercent > 90 && ObjectManager.Me.Target > 0)
                    {
                        Logging.WriteFight(" - Target Evading - ");
                        break;
                    }
                }
            }
            if (ObjectManager.Me.IsMounted) mountchill = new Timer(2000);
            Thread.Sleep(350);
        }
    }

    public void pull()
    {
        if (hardmob()) Logging.WriteFight(" -  Pull Hard Mob - ");
        if (!hardmob()) Logging.WriteFight(" -  Pull Easy Mob - ");
        pet();
        Lua.RunMacroText("/petattack");
        petchill = new Timer(3000);
        fighttimer = new Timer(60000);
    }

    public void buffoutfight()
    {
        if (Fight.InFight || ObjectManager.Me.IsDeadMe) return;

        pet();

        if (!Dark_Intent.HaveBuff && Dark_Intent.KnownSpell)
        {
            Dark_Intent.Launch();
        }

        if (!ObjectManager.Me.HaveBuff(79640) &&
            ItemsManager.GetItemCountByIdLUA(58149) == 1)
        {
            Logging.WriteFight("Use Alchi Flask");
            Lua.RunMacroText("/use item:58149");
        }

        if (ItemsManager.GetItemCountByIdLUA(5512) == 0 && Create_Healthstone.KnownSpell &&
            Create_Healthstone.IsSpellUsable)
        {
            Logging.WriteFight("Create Healthstone");
            Thread.Sleep(200);
            Create_Healthstone.Launch();
            Thread.Sleep(200);
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(100);
                Thread.Sleep(100);
            }
        }

        if (ObjectManager.Me.HealthPercent < 65 && Soul_Harvest.IsSpellUsable && Soul_Harvest.KnownSpell)
        {
            Thread.Sleep(200);
            Fight.StopFight();
            MovementManager.StopMove();
            if (ObjectManager.Me.ManaPercentage < 50) SpellManager.CastSpellByIdLUA(1454);
            Soul_Harvest.Launch();
            Thread.Sleep(200);
            Fight.StopFight();
            MovementManager.StopMove();
            while (ObjectManager.Me.IsCast)
            {
                Fight.StopFight();
                MovementManager.StopMove();
                Thread.Sleep(200);
            }
        }

        if (Demon_Leap.IsSpellUsable && Demon_Leap.KnownSpell)
        {
            SpellManager.CastSpellByIdLUA(54785);
            // Demon_Leap.Launch();
            return;
        }
    }

    public void fight()
    {
        selfheal();
        pet();
        buffinfight();
        if (ObjectManager.GetNumberAttackPlayer() > 1) fighttimer = new Timer(60000);

        if (ObjectManager.Target.IsTargetingMe)
        {
            SpellManager.CastSpellByIdLUA(89766);
        }

        if (petchill.IsReady)
        {
            SpellManager.CastSpellByIdLUA(89751);
        }

        if (ObjectManager.Me.HealthPercent < 20 &&
            ItemsManager.GetItemCountByIdLUA(5512) == 1)
        {
            Lua.RunMacroText("/use item:5512");
            Logging.WriteFight(" - Healthstone Used - ");
            return;
        }

        if (ObjectManager.Me.HaveBuff(63167))
        {
            SpellManager.CastSpellByIdLUA(6353);
            // Soul_Fire.Launch();
            return;
        }

        if (!ObjectManager.Me.HaveBuff(63167) &&
            ObjectManager.Me.HealthPercent > 50 &&
            ObjectManager.Target.HealthPercent < 26 &&
            ObjectManager.GetNumberAttackPlayer() < 2 &&
            Drain_Soul.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1120);
            // Drain_Soul.Launch();
            return;
        }

        if (Soulburn.KnownSpell && Soulburn.IsSpellUsable && Soul_Fire.IsSpellUsable && Soul_Fire.KnownSpell &&
            Soul_Fire.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(74434);
            // Soulburn.Launch();
            SpellManager.CastSpellByIdLUA(6353);
            // Soul_Fire.Launch();
        }

        if (!Curse_of_the_Elements.TargetHaveBuff && hardmob() &&
            ObjectManager.Target.HealthPercent < 100 &&
            ObjectManager.Target.HealthPercent > 40 &&
            Curse_of_the_Elements.KnownSpell &&
            Curse_of_the_Elements.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1490);
            // Curse_of_the_Elements.Launch();
            return;
        }

        selfheal();

        if (ObjectManager.Me.HealthPercent < 85 &&
            Death_Coil.KnownSpell &&
            Death_Coil.IsDistanceGood &&
            Death_Coil.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(6789);
            // Death_Coil.Launch();
            return;
        }

        if (ObjectManager.Target.GetDistance < 4 && Shadowflame.KnownSpell && Shadowflame.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(47897);
            // Shadowflame.Launch();
            nManager.Wow.Helpers.Keybindings.DownKeybindings(Keybindings.JUMP);
            Thread.Sleep(100);
            nManager.Wow.Helpers.Keybindings.DownKeybindings(Keybindings.MOVEBACKWARD);
            Thread.Sleep(1000);
            nManager.Wow.Helpers.Keybindings.UpKeybindings(Keybindings.JUMP);
            nManager.Wow.Helpers.Keybindings.UpKeybindings(Keybindings.MOVEBACKWARD);
            return;
        }

        if (ObjectManager.Me.HealthPercent > 79 && ObjectManager.Me.ManaPercentage < 50 && Life_Tap.KnownSpell)
        {
            SpellManager.CastSpellByIdLUA(1454);
            // Life_Tap.Launch();
            return;
        }

        if (Berserking.KnownSpell && Berserking.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1454);
            // Berserking.Launch();
        }

        if (!Incinerate.KnownSpell && Shadow_Bolt.IsSpellUsable) Shadow_Bolt.Launch();

        if (!Incinerate.KnownSpell && !Corruption.TargetHaveBuff && Corruption.IsSpellUsable &&
            ObjectManager.Target.HealthPercent > 40) Corruption.Launch();

        if (!Incinerate.KnownSpell && !Bane_of_Doom.TargetHaveBuff && Bane_of_Doom.IsSpellUsable &&
            ObjectManager.Target.HealthPercent > 40) Bane_of_Doom.Launch();

        if (!Curse_of_Guldan.TargetHaveBuff &&
            Hand_of_Guldan.KnownSpell &&
            Hand_of_Guldan.IsDistanceGood &&
            Hand_of_Guldan.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(71521);
            // Hand_of_Guldan.Launch();
            return;
        }

        if (ObjectManager.Target.HaveBuff(348) &&
            Incinerate.KnownSpell &&
            Incinerate.IsDistanceGood &&
            Incinerate.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(29722);
            // Incinerate.Launch();
            return;
        }

        if (!Immolate.TargetHaveBuff &&
            Immolate.KnownSpell &&
            Immolate.IsDistanceGood &&
            Immolate.IsSpellUsable &&
            waitfordebuff.IsReady &&
            ObjectManager.Target.HealthPercent > 40)
        {
            SpellManager.CastSpellByIdLUA(348);
            // Immolate.Launch();
            waitfordebuff = new Timer(2000);
            return;
        }
    }

    private void buffinfight()
    {
        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            Stoneform.KnownSpell && Stoneform.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(20594);
            // Stoneform.Launch();
        }

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            War_Stomp.KnownSpell && War_Stomp.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(20549);
            // War_Stomp.Launch();
        }

        if (Fel_Armor.KnownSpell && !Fel_Armor.HaveBuff && Fel_Armor.IsSpellUsable)
        {
            Fel_Armor.Launch();
        }

        if (Soul_Link.KnownSpell && !Soul_Link.HaveBuff && Soul_Link.IsSpellUsable)
        {
            Soul_Link.Launch();
        }
        else if (Demon_Armor.KnownSpell && !Fel_Armor.KnownSpell && !Demon_Armor.HaveBuff && Demon_Armor.IsSpellUsable)
        {
            Demon_Armor.Launch();
        }

        if (Summon_Infernal.KnownSpell && Summon_Infernal.IsDistanceGood && Summon_Infernal.IsSpellUsable && hardmob())
        {
            SpellManager.CastSpellByIDAndPosition(1122, ObjectManager.Target.Position);
        }

        if (Metamorphosis.KnownSpell && Metamorphosis.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(59672);
            // Metamorphosis.Launch();
        }

        if (!Metamorphosis.HaveBuff && Demon_Soul.KnownSpell && Demon_Soul.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(77801);
            // Demon_Soul.Launch();
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2 &&
            Immolation_Aura.KnownSpell && Immolation_Aura.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(50589);
            // Immolation_Aura.Launch();
        }
    }

    private void selfheal()
    {
        if (ObjectManager.Me.HealthPercent < 80 &&
            Lifeblood.KnownSpell && Lifeblood.IsSpellUsable)
        {
            Lifeblood.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 80 &&
            Gift_of_the_Naaru.KnownSpell && Gift_of_the_Naaru.IsSpellUsable)
        {
            Gift_of_the_Naaru.Launch();
        }

        if (ArcaneTorrent.KnownSpell && ArcaneTorrent.IsSpellUsable &&
            ObjectManager.Target.IsCast && ObjectManager.Target.GetDistance < 8)
        {
            ArcaneTorrent.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 50 &&
            Drain_Life.KnownSpell &&
            Drain_Life.IsDistanceGood &&
            Drain_Life.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(689);
            // Drain_Life.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(100);
                Thread.Sleep(100);
            }
            return;
        }
    }

    private void pet()
    {
        if (ObjectManager.Me.IsMounted || !mountchill.IsReady) return;

        if (Health_Funnel.KnownSpell && Health_Funnel.IsSpellUsable && ObjectManager.Pet.HealthPercent > 0 &&
            ObjectManager.Pet.HealthPercent < 50)
        {
            SpellManager.CastSpellByIdLUA(755);
            // Health_Funnel.Launch();
            while (ObjectManager.Me.IsCast)
            {
                if (ObjectManager.Pet.HealthPercent > 80 || ObjectManager.Pet.IsDead) break;
                Thread.Sleep(100);
            }
        }

        if ((ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) &&
            !ObjectManager.Me.IsMounted && !ObjectManager.Me.IsDeadMe)
        {
            Logging.WriteFight(" - PET DEAD - ");
            if (Summon_Felguard.KnownSpell && Summon_Felguard.IsSpellUsable)
            {
                if (Soulburn.KnownSpell && Soulburn.IsSpellUsable)
                {
                    SpellManager.CastSpellByIdLUA(74434);
                    // Soulburn.Launch();
                }
                Summon_Felguard.Launch();
            }
            if (!Summon_Felguard.KnownSpell) Summon_Imp.Launch();
        }
    }

    public bool hardmob()
    {
        if (((ObjectManager.Target.MaxHealth*100)/ObjectManager.Me.MaxHealth) > 120)
        {
            return true;
        }
        return false;
    }
}

public class Affli
{
    #region InitializeSpell

    private Spell Immolate = new Spell("Immolate");
    private Spell Soul_Fire = new Spell("Soul Fire");
    private Spell Bane_of_Doom = new Spell("Bane of Doom");
    private Spell Shadow_Bolt = new Spell("Shadow Bolt");
    private Spell Shadowflame = new Spell("Shadowflame");
    private Spell Incinerate = new Spell("Incinerate");
    private Spell Health_Funnel = new Spell("Health Funnel");
    private Spell Life_Tap = new Spell("Life Tap");
    private Spell Drain_Soul = new Spell("Drain Soul");
    private Spell Corruption = new Spell("Corruption");
    private Spell Curse_of_the_Elements = new Spell("Curse of the Elements");
    private Spell Drain_Life = new Spell("Drain Life");
    private Spell Demon_Soul = new Spell("Demon Soul");
    private Spell Soul_Swap = new Spell("Soul Swap");
    private Spell Summon_Imp = new Spell("Summon Imp");
    private Spell Summon_Felhunter = new Spell("Summon Felhunter");
    private Spell Summon_Infernal = new Spell("Summon Infernal");
    private Spell Death_Coil = new Spell("Death Coil");
    private Spell Curse_of_Weakness = new Spell("Curse of Weakness");
    private Spell Curse_of_Tongues = new Spell("Curse of Tongues");
    private Spell Hand_of_Guldan = new Spell("Hand of Gul'dan");
    private Spell Curse_of_Guldan = new Spell("Curse of Gul'dan");
    private Spell Soul_Harvest = new Spell("Soul Harvest");
    private Spell Create_Healthstone = new Spell("Create Healthstone");
    private Spell Fel_Armor = new Spell("Fel Armor");
    private Spell Demon_Armor = new Spell("Demon Armor");
    private Spell Soulburn = new Spell("Soulburn");
    private Spell Dark_Intent = new Spell("Dark Intent");
    private Spell Haunt = new Spell("Haunt");
    private Spell Unstable_Affliction = new Spell("Unstable Affliction");
    private Spell Bane_of_Agony = new Spell("Bane of Agony");
    private Spell Shadow_Trance = new Spell("Shadow Trance");
    private Timer look = new Timer(0);
    private Timer fighttimer = new Timer(0);
    private Timer waitfordebuff = new Timer(0);
    private Timer mountchill = new Timer(0);

    // profession & racials
    private Spell ArcaneTorrent = new Spell("Arcane Torrent");
    private Spell Lifeblood = new Spell("Lifeblood");
    private Spell Stoneform = new Spell("Stoneform");
    private Spell Tailoring = new Spell("Tailoring");
    private Spell Leatherworking = new Spell("Leatherworking");
    private Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private Spell War_Stomp = new Spell("War Stomp");
    private Spell Berserking = new Spell("Berserking");

    #endregion InitializeSpell

    public Affli()
    {
        Main.range = 30.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            if (!ObjectManager.Me.IsMounted)
            {
                buffoutfight();

                if (!Fight.InFight && look.IsReady)
                {
                    look = new Timer(5000);
                    Lua.RunMacroText("/targetfriendplayer");
                }

                if (Fight.InFight && ObjectManager.Me.Target > 0 && ObjectManager.Target.GetDistance > Main.range)
                {
                    fighttimer = new Timer(60000);
                }

                if (Fight.InFight && ObjectManager.Me.Target > 0)
                {
                    if (ObjectManager.Me.Target != lastTarget && ObjectManager.Target.GetDistance <= Main.range)
                    {
                        pull();
                        lastTarget = ObjectManager.Me.Target;
                    }
                    fight();
                    if (!Fight.InFight)
                    {
                        Logging.WriteFight(" - Target Down - ");
                        look = new Timer(5000);
                    }

                    if (fighttimer.IsReady && ObjectManager.Target.HealthPercent > 90 && ObjectManager.Me.Target > 0)
                    {
                        Logging.WriteFight(" - Target Evading - ");
                        break;
                    }
                }
            }
            if (ObjectManager.Me.IsMounted) mountchill = new Timer(2000);
            Thread.Sleep(350);
        }
    }

    public void pull()
    {
        if (hardmob()) Logging.WriteFight(" -  Pull Hard Mob - ");
        if (!hardmob()) Logging.WriteFight(" -  Pull Easy Mob - ");
        pet();
        fighttimer = new Timer(60000);
        Lua.RunMacroText("/petattack");
    }

    public void buffoutfight()
    {
        if (Fight.InFight || ObjectManager.Me.IsDeadMe) return;

        pet();

        if (!Dark_Intent.HaveBuff && Dark_Intent.KnownSpell)
        {
            Dark_Intent.Launch();
        }

        if (!ObjectManager.Me.HaveBuff(79640) &&
            ItemsManager.GetItemCountByIdLUA(58149) == 1)
        {
            Logging.WriteFight("Use Alchi Flask");
            Lua.RunMacroText("/use item:58149");
        }

        if (ItemsManager.GetItemCountByIdLUA(5512) == 0 && Create_Healthstone.KnownSpell &&
            Create_Healthstone.IsSpellUsable)
        {
            Logging.WriteFight(" - Create Healthstone - ");
            Thread.Sleep(200);
            Create_Healthstone.Launch();
            Thread.Sleep(200);
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
        }

        if (ObjectManager.Me.HealthPercent < 65 && Soul_Harvest.IsSpellUsable && Soul_Harvest.KnownSpell)
        {
            Thread.Sleep(200);
            Fight.StopFight();
            MovementManager.StopMove();
            if (ObjectManager.Me.ManaPercentage < 50) SpellManager.CastSpellByIdLUA(1454);
            Soul_Harvest.Launch();
            Thread.Sleep(200);
            Fight.StopFight();
            MovementManager.StopMove();
            while (ObjectManager.Me.IsCast)
            {
                Fight.StopFight();
                MovementManager.StopMove();
                Thread.Sleep(200);
            }
        }
    }

    public void fight()
    {
        selfheal();
        pet();
        buffinfight();
        if (ObjectManager.GetNumberAttackPlayer() > 1) fighttimer = new Timer(60000);

        if (ObjectManager.Me.HealthPercent < 20 &&
            ItemsManager.GetItemCountByIdLUA(5512) == 1)
        {
            Lua.RunMacroText("/use item:5512");
            Logging.WriteFight(" - Healthstone Used - ");
            return;
        }

        if (Soul_Swap.HaveBuff &&
            Soul_Swap.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(86213);
        }

        if (ObjectManager.Me.HaveBuff(17941) && Shadow_Bolt.IsDistanceGood && Shadow_Bolt.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(686);
            // Shadow_Bolt.Launch();
            return;
        }

        if (!ObjectManager.Me.HaveBuff(63167) &&
            ObjectManager.Me.HealthPercent > 50 &&
            ObjectManager.Target.HealthPercent < 26 &&
            ObjectManager.GetNumberAttackPlayer() < 2 &&
            Drain_Soul.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1120);
            // Drain_Soul.Launch();
            return;
        }

        if (!Curse_of_the_Elements.TargetHaveBuff && hardmob() &&
            ObjectManager.Target.HealthPercent < 100 &&
            ObjectManager.Target.HealthPercent > 40 &&
            Curse_of_the_Elements.KnownSpell &&
            Curse_of_the_Elements.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1490);
            // Curse_of_the_Elements.Launch();
            return;
        }

        selfheal();

        if (ObjectManager.Me.HealthPercent < 85 &&
            Death_Coil.KnownSpell &&
            Death_Coil.IsDistanceGood &&
            Death_Coil.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(6789);
            // Death_Coil.Launch();
            return;
        }

        if (ObjectManager.Target.GetDistance < 4 && Shadowflame.KnownSpell && Shadowflame.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(47897);
            // Shadowflame.Launch();
            nManager.Wow.Helpers.Keybindings.DownKeybindings(Keybindings.JUMP);
            Thread.Sleep(100);
            nManager.Wow.Helpers.Keybindings.DownKeybindings(Keybindings.MOVEBACKWARD);
            Thread.Sleep(1000);
            nManager.Wow.Helpers.Keybindings.UpKeybindings(Keybindings.JUMP);
            nManager.Wow.Helpers.Keybindings.UpKeybindings(Keybindings.MOVEBACKWARD);
            return;
        }

        if (ObjectManager.Me.HealthPercent > 79 && ObjectManager.Me.ManaPercentage < 50 && Life_Tap.KnownSpell)
        {
            SpellManager.CastSpellByIdLUA(1454);
            // Life_Tap.Launch();
            return;
        }

        if (Berserking.KnownSpell && Berserking.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1454);
            // Berserking.Launch();
        }

        if (!Haunt.TargetHaveBuff && Haunt.KnownSpell && Haunt.IsDistanceGood && Haunt.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(48181);
            // Haunt.Launch();
            return;
        }

        if (Soul_Swap.KnownSpell &&
            Soul_Swap.IsDistanceGood &&
            Soul_Swap.IsSpellUsable &&
            (ObjectManager.Target.HealthPercent < 35 && !hardmob())
            || hardmob())
        {
            SpellManager.CastSpellByIdLUA(86121);
            // Soul_Swap.Launch();
        }

        if (!Corruption.TargetHaveBuff && Corruption.KnownSpell && Corruption.IsDistanceGood && Corruption.IsSpellUsable)
        {
            Corruption.Launch();
            return;
        }

        if (!Bane_of_Agony.TargetHaveBuff && Bane_of_Agony.KnownSpell && Bane_of_Agony.IsDistanceGood && hardmob() &&
            Bane_of_Agony.IsSpellUsable)
        {
            Bane_of_Agony.Launch();
            return;
        }

        if (!Unstable_Affliction.TargetHaveBuff && Unstable_Affliction.KnownSpell && Unstable_Affliction.IsDistanceGood &&
            Unstable_Affliction.IsSpellUsable && waitfordebuff.IsReady)
        {
            waitfordebuff = new Timer(2500);
            SpellManager.CastSpellByIdLUA(30108);
            // Unstable_Affliction.Launch();
            return;
        }

        if (Drain_Life.KnownSpell &&
            Drain_Life.IsDistanceGood &&
            Drain_Life.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(689);
            // Drain_Life.Launch();
            Thread.Sleep(200);
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(100);
                Thread.Sleep(100);
            }
            return;
        }
    }

    private void buffinfight()
    {
        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            Stoneform.KnownSpell && Stoneform.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(20594);
            // Stoneform.Launch();
        }

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            War_Stomp.KnownSpell && War_Stomp.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(20549);
            // War_Stomp.Launch();
        }

        if (Fel_Armor.KnownSpell && !Fel_Armor.HaveBuff && Fel_Armor.IsSpellUsable)
        {
            Fel_Armor.Launch();
        }

        if (Demon_Armor.KnownSpell && !Fel_Armor.KnownSpell && !Demon_Armor.HaveBuff && Demon_Armor.IsSpellUsable)
        {
            Demon_Armor.Launch();
        }

        if (Summon_Infernal.KnownSpell && Summon_Infernal.IsDistanceGood && Summon_Infernal.IsSpellUsable && hardmob())
        {
            SpellManager.CastSpellByIDAndPosition(1122, ObjectManager.Target.Position);
        }

        if (Demon_Soul.KnownSpell && Demon_Soul.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(77801);
            // Demon_Soul.Launch();
        }
    }

    private void selfheal()
    {
        if (ObjectManager.Me.HealthPercent < 60 &&
            Lifeblood.KnownSpell && Lifeblood.IsSpellUsable)
        {
            Lifeblood.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 60 &&
            Gift_of_the_Naaru.KnownSpell && Gift_of_the_Naaru.IsSpellUsable)
        {
            Gift_of_the_Naaru.Launch();
        }

        if (ArcaneTorrent.KnownSpell && ArcaneTorrent.IsSpellUsable &&
            ObjectManager.Target.IsCast && ObjectManager.Target.GetDistance < 8)
        {
            ArcaneTorrent.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 50 &&
            Drain_Life.KnownSpell &&
            Drain_Life.IsDistanceGood &&
            Drain_Life.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(689);
            // Drain_Life.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }
    }

    private void pet()
    {
        if (ObjectManager.Me.IsMounted || !mountchill.IsReady) return;

        if (Health_Funnel.KnownSpell)
            if (ObjectManager.Pet.HealthPercent > 0 && ObjectManager.Pet.HealthPercent < 50 &&
                Health_Funnel.IsSpellUsable)
            {
                SpellManager.CastSpellByIdLUA(755);
                // Health_Funnel.Launch();
                while (ObjectManager.Me.IsCast)
                {
                    if (ObjectManager.Pet.HealthPercent > 85 || ObjectManager.Pet.IsDead)
                        break;
                    Thread.Sleep(100);
                }
            }

        if ((ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) &&
            !ObjectManager.Me.IsMounted && !ObjectManager.Me.IsDeadMe)
        {
            Logging.WriteFight(" - PET DEAD - ");
            if (Soulburn.KnownSpell && Soulburn.IsSpellUsable)
            {
                SpellManager.CastSpellByIdLUA(74434);
                // Soulburn.Launch();
            }
            Summon_Felhunter.Launch();
        }
    }

    public bool hardmob()
    {
        if (((ObjectManager.Target.MaxHealth*100)/ObjectManager.Me.MaxHealth) > 120)
        {
            return true;
        }
        return false;
    }
}

#endregion

#region Druid

public class Balance
{
    private Int32 firemode;

    #region InitializeSpell

    // BALANCE ONLY
    private Spell Starfall = new Spell("Starfall");
    private Spell Typhoon = new Spell("Typhoon");
    private Spell Moonkin_Form = new Spell("Moonkin Form");
    private Spell Force_of_Nature = new Spell("Force of Nature");
    private Spell Solar_Beam = new Spell("Solar Beam");
    private Spell Starsurge = new Spell("Starsurge");
    private Spell Sunfire = new Spell("Sunfire");

    // DPS
    private Spell Insect_Swarm = new Spell("Insect Swarm");
    private Spell Starfire = new Spell("Starfire");
    private Spell Moonfire = new Spell("Moonfire");
    private Spell Wrath = new Spell("Wrath");

    // HEAL
    private Spell Regrowth = new Spell("Regrowth");
    private Spell Rejuvenation = new Spell("Rejuvenation");
    private Spell Nourish = new Spell("Nourish");
    private Spell Lifebloom = new Spell("Lifebloom");
    private Spell Healing_Touch = new Spell("Healing Touch");

    // BUFF & HELPING
    private Spell Innervate = new Spell("Innervate");
    private Spell Mark_of_the_Wild = new Spell("Mark of the Wild");
    private Spell Barkskin = new Spell("Barkskin");

    // TIMER
    private Timer look = new Timer(0);
    private Timer fighttimer = new Timer(0);
    private Timer slowbloom = new Timer(0);

    // profession & racials
    private Spell ArcaneTorrent = new Spell("Arcane Torrent");
    private Spell Lifeblood = new Spell("Lifeblood");
    private Spell Stoneform = new Spell("Stoneform");
    private Spell Tailoring = new Spell("Tailoring");
    private Spell Leatherworking = new Spell("Leatherworking");
    private Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private Spell War_Stomp = new Spell("War Stomp");
    private Spell Berserking = new Spell("Berserking");

    #endregion InitializeSpell

    public Balance()
    {
        Main.range = 30.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            if (!ObjectManager.Me.IsMounted)
            {
                buffoutfight();

                if (!Fight.InFight && look.IsReady)
                {
                    look = new Timer(5000);
                    Lua.RunMacroText("/targetfriendplayer");
                }

                if (Fight.InFight && ObjectManager.Me.Target > 0 && ObjectManager.Target.GetDistance > Main.range)
                {
                    fighttimer = new Timer(60000);
                }

                if (Fight.InFight && ObjectManager.Me.Target > 0)
                {
                    if (ObjectManager.Me.Target != lastTarget && ObjectManager.Target.GetDistance <= Main.range)
                    {
                        pull();
                        lastTarget = ObjectManager.Me.Target;
                    }
                    fight();
                    if (!Fight.InFight)
                    {
                        Logging.WriteFight(" - Target Down - ");
                        look = new Timer(5000);
                    }

                    if (fighttimer.IsReady && ObjectManager.Target.HealthPercent > 90 && ObjectManager.Me.Target > 0)
                    {
                        Logging.WriteFight(" - Target Evading - ");
                        break;
                    }
                }
            }
            Thread.Sleep(350);
        }
    }

    public void pull()
    {
        if (hardmob()) Logging.WriteFight(" -  Pull Hard Mob - ");
        if (!hardmob()) Logging.WriteFight(" -  Pull Easy Mob - ");
        fighttimer = new Timer(60000);
    }

    public void buffoutfight()
    {
        if (Fight.InFight || ObjectManager.Me.IsDeadMe) return;

        if (!ObjectManager.Me.HaveBuff(79640) &&
            ItemsManager.GetItemCountByIdLUA(58149) == 1)
        {
            Logging.WriteFight("Use Alchi Flask");
            Lua.RunMacroText("/use item:58149");
        }

        if (Mark_of_the_Wild.KnownSpell && Mark_of_the_Wild.IsSpellUsable && !Mark_of_the_Wild.HaveBuff)
        {
            Mark_of_the_Wild.Launch();
        }
    }

    public void fight()
    {
        selfheal();
        buffinfight();
        if (ObjectManager.GetNumberAttackPlayer() > 1) fighttimer = new Timer(60000);

        if (!ObjectManager.Target.IsTargetingMe && Wrath.IsDistanceGood)
        {
            if (wrathspam())
            {
                SpellManager.CastSpellByIdLUA(5176);
                // Wrath.Launch();
            }

            if (starfirespam())
            {
                SpellManager.CastSpellByIdLUA(2912);
                // Starfire.Launch();
            }
            return;
        }

        if (ObjectManager.Me.HaveBuff(93400) && Starsurge.IsDistanceGood && Starsurge.IsSpellUsable &&
            ObjectManager.Target.IsTargetingMe)
        {
            SpellManager.CastSpellByIdLUA(78674);
            // Starsurge.Launch();
        }

        if (Moonfire.KnownSpell && Moonfire.IsDistanceGood && Moonfire.IsSpellUsable && !Moonfire.TargetHaveBuff &&
            starfirespam())
        {
            SpellManager.CastSpellByIdLUA(8921);
            // Moonfire.Launch();
        }

        if (wrathspam() && !ObjectManager.Target.HaveBuff(93402) && !Moonfire.TargetHaveBuff)
        {
            if (Sunfire.KnownSpell && Sunfire.IsDistanceGood && Sunfire.IsSpellUsable)
            {
                SpellManager.CastSpellByIdLUA(93402);
                // Sunfire.Launch();
                return;
            }
            if (Moonfire.KnownSpell && Moonfire.IsDistanceGood && Moonfire.IsSpellUsable)
            {
                SpellManager.CastSpellByIdLUA(8921);
                // Moonfire.Launch();
            }
        }

        if (Starsurge.KnownSpell && Starsurge.IsDistanceGood && Starsurge.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(78674);
            // Starsurge.Launch();
        }

        if (Insect_Swarm.KnownSpell && Insect_Swarm.IsDistanceGood && Insect_Swarm.IsSpellUsable &&
            !Insect_Swarm.TargetHaveBuff && hardmob())
        {
            SpellManager.CastSpellByIdLUA(5570);
            // Insect_Swarm.Launch();
        }

        if (Wrath.KnownSpell && Wrath.IsSpellUsable && Wrath.IsDistanceGood && wrathspam() &&
            (ObjectManager.Target.HaveBuff(93402) || Moonfire.TargetHaveBuff))
        {
            SpellManager.CastSpellByIdLUA(5176);
            // Wrath.Launch();
        }

        if (Starfire.KnownSpell && Starfire.IsSpellUsable && Starfire.IsDistanceGood && starfirespam() &&
            (ObjectManager.Target.HaveBuff(93402) || Moonfire.TargetHaveBuff))
        {
            SpellManager.CastSpellByIdLUA(2912);
            // Starfire.Launch();
        }

        if (Berserking.KnownSpell && Berserking.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1454);
            // Berserking.Launch();
        }

        if (Force_of_Nature.KnownSpell && Force_of_Nature.IsSpellUsable && Force_of_Nature.IsDistanceGood &&
            (hardmob() || ObjectManager.GetNumberAttackPlayer() > 1))
        {
            SpellManager.CastSpellByIDAndPosition(33831, ObjectManager.Target.Position);
        }

        if (Typhoon.KnownSpell && Typhoon.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 1 &&
            ObjectManager.Target.GetDistance < 40)
        {
            SpellManager.CastSpellByIdLUA(50516);
            // Typhoon.Launch();
        }

        if (Starfall.KnownSpell && Starfall.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 1 &&
            ObjectManager.Target.GetDistance < 40)
        {
            SpellManager.CastSpellByIdLUA(48505);
            // Starfall.Launch();
        }
    }

    private void buffinfight()
    {
        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            Stoneform.KnownSpell && Stoneform.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(20594);
            // Stoneform.Launch();
        }

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            War_Stomp.KnownSpell && War_Stomp.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(20549);
            // War_Stomp.Launch();
        }

        if (Innervate.KnownSpell && Innervate.IsSpellUsable && ObjectManager.Me.ManaPercentage < 40)
        {
            SpellManager.CastSpellByIdLUA(29166);
            // Innervate.Launch();
        }

        if (Moonkin_Form.KnownSpell && !Moonkin_Form.HaveBuff && ObjectManager.Me.HealthPercent > 60)
        {
            SpellManager.CastSpellByIdLUA(24858);
            // Moonkin_Form.Launch();
        }
    }

    private void selfheal()
    {
        if (ObjectManager.Me.HealthPercent < 80 &&
            Lifeblood.KnownSpell && Lifeblood.IsSpellUsable)
        {
            Lifeblood.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 80 &&
            Gift_of_the_Naaru.KnownSpell && Gift_of_the_Naaru.IsSpellUsable)
        {
            Gift_of_the_Naaru.Launch();
        }

        if (ArcaneTorrent.KnownSpell && ArcaneTorrent.IsSpellUsable &&
            ObjectManager.Target.IsCast && ObjectManager.Target.GetDistance < 8)
        {
            ArcaneTorrent.Launch();
        }

        if (Solar_Beam.KnownSpell && Solar_Beam.IsSpellUsable && Solar_Beam.IsDistanceGood &&
            ObjectManager.Target.IsCast)
        {
            SpellManager.CastSpellByIdLUA(78675);
            // Solar_Beam.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 60 && ObjectManager.Me.ManaPercentage < 25 && Regrowth.KnownSpell &&
            Regrowth.IsSpellUsable && !Regrowth.HaveBuff)
        {
            if (Barkskin.KnownSpell && Barkskin.IsSpellUsable) Barkskin.Launch();
            SpellManager.CastSpellByIdLUA(8936);
            // Regrowth.Launch();
            return;
        }

        if (ObjectManager.Me.HealthPercent < 50 && ObjectManager.Me.ManaPercentage > 25)
        {
            if (Barkskin.KnownSpell && Barkskin.IsSpellUsable) Barkskin.Launch();
            while (ObjectManager.Me.HealthPercent < 70)
            {
                if (Rejuvenation.KnownSpell && Rejuvenation.IsSpellUsable && !Rejuvenation.HaveBuff)
                {
                    SpellManager.CastSpellByIdLUA(774);
                    // Rejuvenation.Launch();
                }

                if (Regrowth.KnownSpell && Regrowth.IsSpellUsable && !Regrowth.HaveBuff)
                {
                    SpellManager.CastSpellByIdLUA(8936);
                    // Regrowth.Launch();
                }

                if (Regrowth.HaveBuff && Regrowth.KnownSpell && Rejuvenation.HaveBuff && Rejuvenation.KnownSpell &&
                    Nourish.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() < 2)
                {
                    SpellManager.CastSpellByIdLUA(50464);
                    // Nourish.Launch();
                }

                if (Regrowth.HaveBuff && Regrowth.KnownSpell && Rejuvenation.HaveBuff && Rejuvenation.KnownSpell &&
                    Healing_Touch.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 1)
                {
                    SpellManager.CastSpellByIdLUA(5185);
                    // Healing_Touch.Launch();
                }

                if (!Regrowth.KnownSpell && Nourish.KnownSpell)
                {
                    SpellManager.CastSpellByIdLUA(50464);
                    // Nourish.Launch();
                }

                if (Lifebloom.KnownSpell && Lifebloom.IsSpellUsable && slowbloom.IsReady)
                {
                    slowbloom = new Timer(4000);
                    SpellManager.CastSpellByIdLUA(33763);
                    // Lifebloom.Launch();
                }
                if (ObjectManager.Me.ManaPercentage < 10) return;
            }
        }
    }

    public bool wrathspam()
    {
        if (ObjectManager.Me.HaveBuff(48517))
        {
            firemode = 1;
        }

        if (ObjectManager.Me.HaveBuff(48518))
        {
            firemode = 2;
        }
        if (firemode == 0 || firemode == 1)
            return true;
        return false;
    }

    public bool starfirespam()
    {
        if (ObjectManager.Me.HaveBuff(48517))
        {
            firemode = 1;
        }

        if (ObjectManager.Me.HaveBuff(48518))
        {
            firemode = 2;
        }
        if (firemode == 2)
            return true;
        return false;
    }

    public bool hardmob()
    {
        if (((ObjectManager.Target.MaxHealth*100)/ObjectManager.Me.MaxHealth) > 110)
        {
            return true;
        }
        return false;
    }
}

public class DruidFeral
{
    #region Inits

    private float PullRange;
    private UInt64 lastTarget;
    private UInt64 Injured;
    private UInt64 LowHealth;
    private UInt64 CriticalHealth;

    private Int64 interupt_cost;

    private WoWPlayer Me = ObjectManager.Me;
    private WoWUnit Target = ObjectManager.Target;
    private bool haveFocus;

    #region balanceSpells

    private Spell Barkskin = new Spell("Barkskin");
    private Spell Cyclone = new Spell("Cyclone");
    private Spell Entangling_Roots = new Spell("Entangling Roots");
    private Spell Faerie_Fire = new Spell("Faerie Fire");
    private Spell Hibernate = new Spell("Hibernate");
    private Spell Hurricane = new Spell("Hurricane");
    private Spell Innervate = new Spell("Innervate");
    private Spell Insect_Swarm = new Spell("Insect Swarm");
    private Spell Moonfire = new Spell("Moonfire");
    private Spell Natures_Grasp = new Spell("Nature's Grasp");
    private Spell Soothe = new Spell("Soothe");
    private Spell Starfire = new Spell("Starfire");
    private Spell Moonglade = new Spell("Teleport: Moonglade");
    private Spell Thorns = new Spell("Thorns");
    private Spell Wrath = new Spell("Wrath");
    private Spell WildShroom = new Spell("Wild Mushroom");
    private Spell WildShroom_deto = new Spell("Wild Mushroom: Detonate");

    #endregion balanceSpells

    #region feralSpells

    private Spell Bash = new Spell("Bash");
    private Spell Bear_Form = new Spell("Bear Form");
    private Spell Berserk = new Spell("Berserk");
    private Spell Cat_Form = new Spell("Cat Form");
    private Spell Challenging_Roar = new Spell("Challenging Roar");
    private Spell Claw = new Spell("Claw");
    private Spell Cower = new Spell("Cower");
    private Spell Dash = new Spell("Dash");
    private Spell Demo_roar = new Spell("Demoralizing Roar");
    private Spell Enrage = new Spell("Enrage");
    private Spell FFF = new Spell("Faerie Fire (Feral)");
    private Spell Flight_Form = new Spell("Flight Form");
    private Spell Bear_Charge = new Spell(16979);
    private Spell Ferocious_Bite = new Spell("Ferocious Bite");
    private Spell Frenzied_Regen = new Spell("Frenzied Regeneration");
    private Spell Taunt = new Spell("Growl");
    private Spell Lacerate = new Spell("Lacerate");
    private Spell Maim = new Spell("Maim");
    private Spell Bear_Mangle = new Spell(33878);
    private Spell Maul = new Spell("Maul");
    private Spell Pounce = new Spell("Pounce");
    private Spell Prowl = new Spell("Prowl");
    private Spell Rake = new Spell("Rake");
    private Spell Ravage = new Spell("Ravage");
    private Spell Rip = new Spell("Rip");
    private Spell Savage_Roar = new Spell("Savage Roar");
    private Spell Shred = new Spell("Shred");
    private Spell Cat_Skull_Bash = new Spell(80965);
    private Spell Bear_Skull_Bash = new Spell(80964);
    private Spell Bear_StampRoar = new Spell(77761);
    private Spell Cat_StampRoar = new Spell(77764);
    private Spell Swift_Flight_Form = new Spell("Swift Flight Form");
    private Spell Bear_Swipe = new Spell(779);
    private Spell Cat_Swipe = new Spell(62078);
    private Spell Thrash = new Spell("Thrash");
    private Spell Tigers_Fury = new Spell("Tiger's Fury");
    private Spell Travel_Form = new Spell("Travel Form");

    #endregion feralSpells

    #region restoSpells

    private Spell Healing_Touch = new Spell("Healing Touch");
    private Spell Lifebloom = new Spell("Lifebloom");
    private Spell Mark_of_the_Wild = new Spell("Mark of the Wild");
    private Spell Regrowth = new Spell("Regrowth");
    private Spell Rejuvenation = new Spell("Rejuvenation");
    private Spell Tranquility = new Spell("Tranquility");

    #endregion restoSpells

    #endregion Inits

    //private DruidConfig df = new DruidConfig(); //NYI

    public DruidFeral()
    {
        Main.range = 5.0f; //Main ability range
        PullRange = 24.0f; //Pull spell range
        lastTarget = 0;

        Injured = 90;
        LowHealth = 50;
        CriticalHealth = 30;
        interupt_cost = Cat_Skull_Bash.Cost;

        //std = new Thread(new ThreadStart(DoAllways)); //NYI
        //std.Start();                                  //NYI


        haveFocus = false; //HaveFocus();//NYI waiting for a working Config CC

        while (Main.loop)
        {
            if (!Mounted() && !InCombat())
            {
                Patrolling();
                if (haveFocus) //NYI
                    FollowFocus();
            }


            while (InFight() && HaveTarget())
            {
                if (haveFocus) //NYI
                    AssistFocus();

                DoAllways();

                if (!IsTarget(lastTarget) && DistanceToTarget() <= PullRange)
                    Pull();
                else
                    Combat();
            }

            DoSavage_Roar();
        }
    }

    #region PlayerState

    public void Pull()
    {
        Charge();
        lastTarget = Me.Target;
    }

    public void Patrolling()
    {
        CheckUseItems();

        if (Heal())
            return;

        Buff();
        Mana();
        // CheckForm();
    }

    public void Combat()
    {
        CheckForm();
        if ((Cat_Form.HaveBuff && Me.EnergyPercentage > (2*interupt_cost)) || Bear_Form.HaveBuff)
        {
            DoFFF();

            if (Target.HealthPercent > Injured)
                DoBerserk();

            if (Me.HealthPercent < LowHealth)
                DoBarkskin();

            DoTigers_Fury();
            DoRavage();

            if (Me.ComboPoint > 3)
                if (!Rip.TargetHaveBuff && Target.HealthPercent > 30)
                    DoRip();
                else
                    DoFerocious_Bite();

            DoRake();

            if (8 <= Me.Level && Me.Level < 10)
                DoClaw();
            else if (10 <= Me.Level)
                DoCat_Mangle();
        }
    }

    #endregion PlayerState

    #region DruidBalanceSpells

    public void DoBarkskin()
    {
        if (Barkskin.KnownSpell &&
            Barkskin.IsSpellUsable)
            Barkskin.Launch();
    }

    public void DoCyclone()
    {
        if (Cyclone.KnownSpell &&
            Cyclone.IsSpellUsable &&
            Cyclone.IsDistanceGood)
            Cyclone.Launch();
    }

    public void DoEntangling_Roots()
    {
        if (Entangling_Roots.KnownSpell &&
            Entangling_Roots.IsSpellUsable &&
            Entangling_Roots.IsDistanceGood)
            Entangling_Roots.Launch();
    }

    public void DoFaerie_Fire()
    {
        if (Faerie_Fire.KnownSpell &&
            Faerie_Fire.IsSpellUsable &&
            Faerie_Fire.IsDistanceGood)
            Faerie_Fire.Launch();
    }

    public void DoHibernate()
    {
        if (Hibernate.KnownSpell &&
            Hibernate.IsSpellUsable &&
            Hibernate.IsDistanceGood)
            Hibernate.Launch();
    }

    public void DoHurricane()
    {
        if (Hurricane.KnownSpell &&
            Hurricane.IsSpellUsable &&
            Hurricane.IsDistanceGood)
            Hurricane.Launch();
    }

    public void DoInnervate()
    {
        if (Innervate.KnownSpell && Innervate.IsSpellUsable)
            Innervate.Launch();
    }

    public void DoInsect_Swarm()
    {
        if (Insect_Swarm.KnownSpell &&
            Insect_Swarm.IsSpellUsable &&
            Insect_Swarm.IsDistanceGood)
            Insect_Swarm.Launch();
    }

    public void DoMoonfire()
    {
        if (Moonfire.KnownSpell &&
            Moonfire.IsSpellUsable &&
            Moonfire.IsDistanceGood)
            Moonfire.Launch();
    }

    public void DoNatures_Grasp()
    {
        if (Natures_Grasp.KnownSpell &&
            Natures_Grasp.IsSpellUsable)
            Natures_Grasp.Launch();
    }

    public void DoSoothe()
    {
        if (Soothe.KnownSpell &&
            Soothe.IsSpellUsable &&
            Soothe.IsDistanceGood)
            Soothe.Launch();
    }

    public void DoStarfire()
    {
        if (Starfire.KnownSpell &&
            Starfire.IsSpellUsable &&
            Starfire.IsDistanceGood)
            Starfire.Launch();
    }

    public void DoMoonglade()
    {
        if (Moonglade.KnownSpell &&
            Moonglade.IsSpellUsable)
            Moonglade.Launch();
    }

    public void DoThorns()
    {
        if (Thorns.KnownSpell &&
            Thorns.IsSpellUsable &&
            Thorns.IsDistanceGood)
            Thorns.Launch();
    }

    public void DoWrath()
    {
        if (Wrath.KnownSpell &&
            Wrath.IsSpellUsable &&
            Wrath.IsDistanceGood)
            Wrath.Launch();
    }

    public void DoWildShroom()
    {
        if (WildShroom.KnownSpell &&
            WildShroom.IsSpellUsable &&
            WildShroom.IsDistanceGood)
            WildShroom.Launch();
    }

    public void DoWildShroom_deto()
    {
        if (WildShroom_deto.KnownSpell &&
            WildShroom_deto.IsSpellUsable &&
            WildShroom_deto.IsDistanceGood)
            WildShroom_deto.Launch();
    }

    #endregion DruidBalanceSpells

    #region DruidFeralSpells

    public void DoBash()
    {
        if (Bash.KnownSpell &&
            Bash.IsSpellUsable &&
            Bash.IsDistanceGood)
            Bash.Launch();
    }

    public void DoBear_Form()
    {
        if (Bear_Form.KnownSpell &&
            Bear_Form.IsSpellUsable)
            Bear_Form.Launch();
    }

    public void DoBerserk()
    {
        if (Berserk.KnownSpell &&
            Berserk.IsSpellUsable)
            Berserk.Launch();
    }

    public void DoCat_Form()
    {
        if (Cat_Form.KnownSpell &&
            Cat_Form.IsSpellUsable)
            Cat_Form.Launch();
    }

    public void DoChallenging_Roar()
    {
        if (Challenging_Roar.KnownSpell &&
            Challenging_Roar.IsSpellUsable)
            Challenging_Roar.Launch();
    }

    public void DoClaw()
    {
        if (Claw.KnownSpell &&
            Claw.IsSpellUsable &&
            Claw.IsDistanceGood)
            Claw.Launch();
    }

    public void DoCower()
    {
        if (Cower.KnownSpell &&
            Cower.IsSpellUsable &&
            Cower.IsDistanceGood)
            Cower.Launch();
    }

    public void DoDash()
    {
        if (Dash.KnownSpell &&
            Dash.IsSpellUsable)
            Dash.Launch();
    }

    public void DoDemo_roar()
    {
        if (Demo_roar.KnownSpell &&
            Demo_roar.IsSpellUsable &&
            Demo_roar.IsDistanceGood)
            Demo_roar.Launch();
    }

    public void DoEnrage()
    {
        if (Enrage.KnownSpell &&
            Enrage.IsSpellUsable)
            Enrage.Launch();
    }

    public void DoFFF()
    {
        if (FFF.KnownSpell &&
            FFF.IsSpellUsable &&
            FFF.IsDistanceGood &&
            (!FFF.TargetHaveBuff || FFF.TargetBuffStack < 3))
            FFF.Launch();
    }

    public void DoFlight_Form()
    {
        if (Flight_Form.KnownSpell &&
            Flight_Form.IsSpellUsable)
            Flight_Form.Launch();
    }

    public void DoBear_Charge()
    {
        if (Bear_Charge.KnownSpell &&
            Bear_Charge.IsSpellUsable &&
            Bear_Charge.IsDistanceGood)
            Bear_Charge.Launch();
    }

    public void DoCat_Charge()
    {
        if (DistanceToTarget() < 25.0f
            /*Cat_Charge.KnownSpell &&
            Cat_Charge.IsSpellUsable &&
            Cat_Charge.IsDistanceGood*/)
        {
            //Cat_Charge.Launch();
            Macro("/cast Feral Charge(Cat form)");
        }
    }

    public void DoFerocious_Bite()
    {
        if (Ferocious_Bite.KnownSpell &&
            Ferocious_Bite.IsSpellUsable &&
            Ferocious_Bite.IsDistanceGood)
            Ferocious_Bite.Launch();
    }

    public void DoFrenzied_Regen()
    {
        if (Frenzied_Regen.KnownSpell &&
            Frenzied_Regen.IsSpellUsable)
            Frenzied_Regen.Launch();
    }

    public void DoTaunt()
    {
        if (Taunt.KnownSpell &&
            Taunt.IsSpellUsable &&
            Taunt.IsDistanceGood)
            Taunt.Launch();
    }

    public void DoLacerate()
    {
        if (Lacerate.KnownSpell &&
            Lacerate.IsSpellUsable &
            Lacerate.IsDistanceGood)
            Lacerate.Launch();
    }

    public void DoMaim()
    {
        if (Maim.KnownSpell &&
            Maim.IsSpellUsable &&
            Maim.IsDistanceGood)
            Maim.Launch();
    }

    public void DoCat_Mangle()
    {
        //if ()
        //Cat_Mangle.Launch();
        Macro("/cast Mangle(Cat form)");
    }

    public void DoBear_Mangle()
    {
        if (Bear_Mangle.KnownSpell &&
            Bear_Mangle.IsSpellUsable &&
            Bear_Mangle.IsDistanceGood)
            Bear_Mangle.Launch();
    }

    public void DoMaul()
    {
        if (Maul.KnownSpell &&
            Maul.IsSpellUsable &&
            Maul.IsDistanceGood)
            Maul.Launch();
    }

    public void DoPounce()
    {
        if (Pounce.KnownSpell &&
            Pounce.IsSpellUsable &&
            Pounce.IsDistanceGood)
            Pounce.Launch();
    }

    public void DoProwl()
    {
        if (Prowl.KnownSpell &&
            Prowl.IsSpellUsable)
            Prowl.Launch();
    }

    public void DoRake()
    {
        if (Rake.KnownSpell &&
            Rake.IsSpellUsable &&
            Rake.IsDistanceGood &&
            !Rake.TargetHaveBuff)
            Rake.Launch();
    }

    public void DoRavage()
    {
        Ravage.Launch();
    }

    public void DoRip()
    {
        if (Rip.KnownSpell &&
            Rip.IsSpellUsable &&
            Rip.IsDistanceGood)
            Rip.Launch();
    }

    public void DoSavage_Roar()
    {
        if (Savage_Roar.KnownSpell &&
            Savage_Roar.IsSpellUsable &&
            Me.ComboPoint > 0)
            Savage_Roar.Launch();
    }

    public void DoShred()
    {
        if (Shred.KnownSpell &&
            Shred.IsSpellUsable &&
            Shred.IsDistanceGood)
            Shred.Launch();
    }

    public void DoCat_Skull_Bash()
    {
        if (Cat_Skull_Bash.KnownSpell &&
            Cat_Skull_Bash.IsSpellUsable &&
            Cat_Skull_Bash.IsDistanceGood)
        {
            Cat_Skull_Bash.Launch();
            Macro("/cast Skull Bash(Cat form)");
        }
    }

    public void DoBear_Skull_Bash()
    {
        if (Bear_Skull_Bash.KnownSpell &&
            Bear_Skull_Bash.IsSpellUsable &&
            Bear_Skull_Bash.IsDistanceGood)
            Bear_Skull_Bash.Launch();
    }

    public void DoBear_StampRoar()
    {
        if (Bear_StampRoar.KnownSpell &&
            Bear_StampRoar.IsSpellUsable)
            Bear_StampRoar.Launch();
    }

    public void DoCat_StampRoar()
    {
        if (Cat_StampRoar.KnownSpell &&
            Cat_StampRoar.IsSpellUsable)
            Cat_StampRoar.Launch();
    }

    public void DoSwift_Flight_Form()
    {
        if (Swift_Flight_Form.KnownSpell &&
            Swift_Flight_Form.IsSpellUsable)
            Swift_Flight_Form.Launch();
    }

    public void DoBear_Swipe()
    {
        if (Bear_Swipe.KnownSpell &&
            Bear_Swipe.IsSpellUsable &&
            Bear_Swipe.IsDistanceGood)
            Bear_Swipe.Launch();
    }

    public void DoCat_Swipe()
    {
        if (Cat_Swipe.KnownSpell &&
            Cat_Swipe.IsSpellUsable &&
            Cat_Swipe.IsDistanceGood)
            Cat_Swipe.Launch();
    }

    public void DoThrash()
    {
        if (Thrash.KnownSpell &&
            Thrash.IsSpellUsable &&
            Thrash.IsDistanceGood)
            Thrash.Launch();
    }

    public void DoTigers_Fury()
    {
        if (Tigers_Fury.KnownSpell &&
            Tigers_Fury.IsSpellUsable &&
            !Tigers_Fury.HaveBuff &&
            (DistanceToTarget() <= 10.0f))
            Tigers_Fury.Launch();
    }

    public void DoTravel_Form()
    {
        if (Travel_Form.KnownSpell &&
            Travel_Form.IsSpellUsable)
            Travel_Form.Launch();
    }

    #endregion DruidFeralSpells

    #region DruidRestoSpells

    public void DoMark_Of_The_Wild()
    {
        if (!Mark_of_the_Wild.HaveBuff && Mark_of_the_Wild.IsSpellUsable && Mark_of_the_Wild.KnownSpell)
            Mark_of_the_Wild.Launch();
    }

    public void DoNourish()
    {
    }

    public void DoRebirth()
    {
    }

    public void DoRemove_Corruption()
    {
    }

    public void DoRevive()
    {
    }

    public bool DoTranquility()
    {
        if (Tranquility.KnownSpell &&
            Tranquility.IsSpellUsable)
        {
            Fight.StopFight();
            MovementManager.StopMove();
            Healing_Touch.Launch();
            Thread.Sleep(5000);
            return true;
        }
        return false;
    }

    public bool DoHealing_Touch()
    {
        if (Healing_Touch.KnownSpell &&
            Healing_Touch.IsSpellUsable &&
            Healing_Touch.IsDistanceGood)
        {
            Fight.StopFight();
            MovementManager.StopMove();
            Healing_Touch.Launch();
            Thread.Sleep(2800);
            return true;
        }
        return false;
    }

    public bool DoRegrowth()
    {
        if (Me.HealthPercent <= 95 &&
            !Regrowth.HaveBuff &&
            Regrowth.KnownSpell &&
            Regrowth.IsSpellUsable)
        {
            Fight.StopFight();
            MovementManager.StopMove();
            Regrowth.Launch();
            return true;
        }
        return false;
    }

    public bool DoLifebloom()
    {
        if (Lifebloom.KnownSpell &&
            Lifebloom.IsSpellUsable &&
            Lifebloom.IsDistanceGood &&
            (!Lifebloom.HaveBuff || Lifebloom.BuffStack < 3))
        {
            Fight.StopFight();
            MovementManager.StopMove();
            Lifebloom.Launch();
            return true;
        }
        return false;
    }

    public bool DoRejuvenation()
    {
        if (!Rejuvenation.HaveBuff && Rejuvenation.KnownSpell && Rejuvenation.IsSpellUsable &&
            !Lifebloom.KnownSpell)
        {
            Fight.StopFight();
            MovementManager.StopMove();
            Rejuvenation.Launch();
            Thread.Sleep(1400);
            return true;
        }
        return false;
    }

    #endregion DruidRestoSpells

    #region SpecialMoves

    public void DoAllways()
    {
        Charge();
        Interupt();
    }

    public void Charge()
    {
        CheckForm();
        if (Cat_Form.HaveBuff)
            DoCat_Charge();
        if (Bear_Form.HaveBuff)
            DoBear_Charge();
    }

    public void Interupt()
    {
        if (Target.IsCast)
            DoCat_Skull_Bash();
    }

    public void Buff()
    {
        DoMark_Of_The_Wild();
    }

    public bool Heal()
    {
        if (Me.HealthPercent < CriticalHealth && Tranquility.IsSpellUsable)
            return DoTranquility();
        if (Me.HealthPercent < LowHealth)
            return DoHealing_Touch();
        if (Me.HealthPercent < Injured)
            return DoRegrowth();

        if (Me.HealthPercent < Injured)
        {
            if (DoLifebloom())
                return true;
            if (DoRejuvenation())
                return true;
        }
        return false;
    }

    public void Mana()
    {
        if (!Cat_Form.HaveBuff && !Bear_Form.HaveBuff && Me.ManaPercentage < 50.0f)
            DoInnervate();
    }

    public void CheckForm()
    {
        if (!ObjectManager.Me.IsMounted && !ObjectManager.Me.IsDeadMe)
        {
            if (!Cat_Form.HaveBuff)
            {
                Cat_Form.Launch();
            }
        }
    }

    public void CheckUseItems()
    {
        if (HaveItem("Strange Bloated Stomach"))
            Macro("/use Strange Bloated Stomach");
    }

    public void FollowFocus()
    {
        Macro("/follow focus");
    }

    public void AssistFocus()
    {
        Macro("/assist focus");
    }

    #endregion SpecialMoves

    #region Shortcuts

    /// <summary>
    /// Checks if you're mounted
    /// </summary>
    /// <returns>True if mounted</returns>
    public bool Mounted()
    {
        return ObjectManager.Me.IsMounted;
    }

    /// <summary>
    /// Checks if you're in combat
    /// </summary>
    /// <returns>True if in combat</returns>
    public bool InCombat()
    {
        return ObjectManager.Me.InCombat;
        //This is for if you are initiating a fight. Aka have an attack qued for the target.
        //return WowManager.WoW.PlayerManager.Fight.InFight;
    }

    /// <summary>
    /// Checks if you're initating a fight(!?)
    /// </summary>
    /// <returns></returns>
    public bool InFight()
    {
        return Fight.InFight;
    }

    /// <summary>
    /// Checks if you have a target
    /// </summary>
    /// <returns>True if you have a target</returns>
    public bool HaveTarget()
    {
        return ObjectManager.Me.Target > 0;
    }

    /// <summary>
    /// Checks if "a" is your target
    /// </summary>
    /// <param name="a">unitidentifyer as UInt64</param>
    /// <returns>True if the unit is your target</returns>
    public bool IsTarget(UInt64 a)
    {
        return ObjectManager.Me.Target == a;
    }

    /// <summary>
    /// Gets the distance to your target in yards (float)
    /// </summary>
    /// <returns>Distance to your target in yards</returns>
    public float DistanceToTarget()
    {
        return ObjectManager.Target.GetDistance;
    }

    /// <summary>
    /// Does command in wow, stated in the param
    /// </summary>
    /// <param name="s">The text command to do inGame</param>
    public void Macro(String s)
    {
        Lua.RunMacroText(s);
    }

    /// <summary>
    /// Checks if the item is in players inventory
    /// </summary>
    /// <param name="s">The item to check for (string)</param>
    /// <returns>True if item is in players inventory</returns>
    public bool HaveItem(String s)
    {
        return ItemsManager.GetItemCountByNameLUA(s) > 0;
    }

    /// <summary>
    /// Checks to see if player has focused a target
    /// </summary>
    /// <returns>True if player has focustarget</returns>
    public bool HaveFocus()
    {
        Fight.StopFight();
        Macro("/cleartarget");
        Thread.Sleep(Usefuls.Latency + 200);
        Macro("/target focus");
        Thread.Sleep(Usefuls.Latency + 1000);
        Fight.StartFight();
        return Me.Target != 0;
    }

    /// <summary>
    /// Write specified text to the BotLog
    /// </summary>
    /// <param name="s">The test you wish to </param>
    public void Log(String s)
    {
        Logging.WriteFight(s);
    }

    #endregion Shortcuts
}

#endregion

#region Paladin

public class Paladin_Holy
{
    [Serializable]
    public class PaladinHolySettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        public bool UseBerserking = true;
        /* Paladin Seals & Buffs */
        public bool UseSealOfTheRighteousness = true;
        public bool UseSealOfTruth = true;
        public bool UseSealOfInsight = true;
        public bool UseBlessingOfMight = true;
        public bool UseBlessingOfKings = true;
        /* Offensive Spell */
        public bool UseHolyShock = true;
        public bool UseDenounce = true;
        public bool UseHammerOfJustice = true;
        public bool UseHammerOfWrath = true;
        /* Offensive Cooldown */
        public bool UseDivineFavor = true;
        public bool UseHolyAvenger = true;
        public bool UseAvengingWrath = true;
        /* Defensive Cooldown */
        public bool UseSacredShield = true;
        public bool UseHandOfPurity = true;
        public bool UseDevotionAura = true;
        public bool UseDivineProtection = true;
        public bool UseDivineShield = true;
        public bool UseHandOfProtection = true;
        /* Healing Spell */
        public bool UseDivinePlea = true;
        public bool UseDivineLight = true;
        public bool UseHolyRadiance = true;
        public bool UseFlashOfLight = true;
        public bool UseHolyLight = true;
        public bool UseLayOnHands = true;
        public bool UseWordOfGlory = true;
        public bool UseBeaconOfLight = true;

        public PaladinHolySettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Paladin Protection Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
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
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Paladin_Holy.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return CurrentSetting = Settings.Load<Paladin_Holy.PaladinHolySettings>(CurrentSettingsFile);
            }
            else
            {
                return new Paladin_Holy.PaladinHolySettings();
            }
        }
    }

    private readonly PaladinHolySettings MySettings = PaladinHolySettings.GetSettings();

    #region Professions & Racial

    private readonly Spell ArcaneTorrent = new Spell("Arcane Torrent");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell War_Stomp = new Spell("War Stomp");
    private readonly Spell Berserking = new Spell("Berserking");

    #endregion

    #region Paladin Seals & Buffs

    private readonly Spell SealOfTheRighteousness = new Spell("Seal of Righteousness");
    private readonly Spell SealOfTruth = new Spell("Seal of Truth");
    private readonly Spell SealOfInsight = new Spell("Seal of Insight");
    private readonly Spell BlessingOfKings = new Spell("Blessing of Kings");
    private readonly Spell BlessingOfMight = new Spell("Blessing of Might");

    #endregion

    #region Offensive Spell

    private readonly Spell HolyShock = new Spell("Holy Shock");
    private readonly Spell Denounce = new Spell("Denounce");
    private readonly Spell HammerOfJustice = new Spell("Hammer of Justice");
    private readonly Spell HammerOfWrath = new Spell("Hammer of Wrath");

    #endregion

    #region Offensive Cooldown

    private readonly Spell DivineFavor = new Spell("Divine Favor");
    private readonly Spell HolyAvenger = new Spell("HolyAvenger");
    private readonly Spell AvengingWrath = new Spell("Avenging Wrath");

    #endregion

    #region Defensive Cooldown

    private readonly Spell SacredShield = new Spell("Sacred Shield");
    private readonly Spell HandOfPurity = new Spell("Hand of Purity");
    private readonly Spell DevotionAura = new Spell("Devotion Aura");
    private readonly Spell DivineProtection = new Spell("Divine Protection");
    private readonly Spell DivineShield = new Spell("Divine Shield");
    private readonly Spell HandOfProtection = new Spell("Hand of Protection");

    #endregion

    #region Healing Spell

    private readonly Spell DivinePlea = new Spell("Divine Plea");
    private readonly Spell DivineLight = new Spell("Divine Light");
    private readonly Spell HolyRadiance = new Spell("Holy Radiance");
    private readonly Spell FlashOfLight = new Spell("Flash of Light");
    private readonly Spell HolyLight = new Spell("Holy Light");
    private readonly Spell LayOnHands = new Spell("Lay on Hands");
    private readonly Spell WordOfGlory = new Spell("Word of Glory");
    private readonly Spell GlyphOfHarshWords = new Spell("Glyph of Harsh Words");
    private readonly Spell BeaconOfLight = new Spell("Beacon of Light");

    #endregion

    public Paladin_Holy()
    {
        Main.range = 30f;

        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
            {
                if (!ObjectManager.Me.IsMounted)
                {
                    Patrolling();

                    if (Fight.InFight && ObjectManager.Me.Target > 0)
                    {
                        if (ObjectManager.Me.Target != lastTarget && HolyShock.IsDistanceGood)
                        {
                            Pull();
                            lastTarget = ObjectManager.Me.Target;
                        }

                        Combat();
                    }
                }
            }
            catch
            {
            }
            Thread.Sleep(50);
        }
    }

    private void Pull()
    {
        if (HolyShock.KnownSpell && HolyShock.IsDistanceGood && HolyShock.IsSpellUsable && MySettings.UseHolyShock)
        {
            HolyShock.Launch();
            return;
        }
    }

    private void Combat()
    {
        DPS_Cycle();

        Heal();

        DPS_Burst();
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Seal();
            Blessing();
        }
    }

    private void Seal()
    {
        if (ObjectManager.Me.IsMounted)
            return;
        else if (SealOfInsight.KnownSpell && MySettings.UseSealOfInsight)
        {
            if (!SealOfInsight.HaveBuff && SealOfInsight.IsSpellUsable)
                SealOfInsight.Launch();
        }
        else if (SealOfTruth.KnownSpell && MySettings.UseSealOfTruth)
        {
            if (!SealOfTruth.HaveBuff && SealOfTruth.IsSpellUsable)
                SealOfTruth.Launch();
        }
        else if (SealOfTheRighteousness.KnownSpell && MySettings.UseSealOfTheRighteousness)
        {
            if (!SealOfTheRighteousness.HaveBuff && SealOfTheRighteousness.IsSpellUsable)
                SealOfTheRighteousness.Launch();
        }
    }

    private void Blessing()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (BlessingOfKings.KnownSpell && !BlessingOfKings.HaveBuff && BlessingOfKings.IsSpellUsable &&
            MySettings.UseBlessingOfKings)
        {
            BlessingOfKings.Launch();
        }
        else if ((!MySettings.UseBlessingOfKings || !BlessingOfKings.KnownSpell || !BlessingOfKings.HaveBuff) &&
                 BlessingOfMight.KnownSpell && !BlessingOfMight.HaveBuff && BlessingOfMight.IsSpellUsable &&
                 MySettings.UseBlessingOfMight)
        {
            BlessingOfMight.Launch();
        }
        if (BeaconOfLight.KnownSpell && !BeaconOfLight.HaveBuff && BeaconOfLight.IsSpellUsable &&
            MySettings.UseBeaconOfLight)
        {
            BeaconOfLight.Launch();
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 95 && !Fight.InFight && ObjectManager.GetNumberAttackPlayer() == 0)
        {
            if (FlashOfLight.KnownSpell && FlashOfLight.IsSpellUsable && MySettings.UseFlashOfLight)
            {
                FlashOfLight.Launch();
                MovementManager.StopMove();
                return;
            }
        }
        if (!ObjectManager.Me.HaveBuff(25771))
        {
            if (DivineShield.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent <= 20 &&
                DivineShield.IsSpellUsable && MySettings.UseDivineShield)
            {
                DivineShield.Launch();
                return;
            }
            if (LayOnHands.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent <= 20 &&
                LayOnHands.IsSpellUsable && MySettings.UseLayOnHands)
            {
                LayOnHands.Launch();
                return;
            }
            if (HandOfProtection.KnownSpell && ObjectManager.Me.HealthPercent > 0 &&
                ObjectManager.Me.HealthPercent <= 20 &&
                HandOfProtection.IsSpellUsable && MySettings.UseHandOfProtection)
            {
                HandOfProtection.Launch();
                return;
            }
        }
        if (ObjectManager.Me.ManaPercentage < 30)
        {
            if (ArcaneTorrent.KnownSpell && ArcaneTorrent.IsSpellUsable && MySettings.UseArcaneTorrent)
                ArcaneTorrent.Launch();
            if (DivinePlea.KnownSpell && DivinePlea.IsSpellUsable && MySettings.UseHandOfProtection)
            {
                DivinePlea.Launch();
                return;
            }
        }
        if (ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 50)
        {
            if (WordOfGlory.KnownSpell && WordOfGlory.IsSpellUsable &&
                (!GlyphOfHarshWords.KnownSpell /* || cast on me */) && MySettings.UseWordOfGlory)
                WordOfGlory.Launch();
            if (DivineLight.KnownSpell && DivineLight.IsSpellUsable && MySettings.UseDivineLight)
            {
                DivineLight.Launch();
                return;
            }
            if (FlashOfLight.KnownSpell && FlashOfLight.IsSpellUsable && MySettings.UseFlashOfLight)
            {
                FlashOfLight.Launch();
                return;
            }
            if (HolyLight.KnownSpell && HolyLight.IsSpellUsable && MySettings.UseHolyLight)
            {
                HolyLight.Launch();
                return;
            }
        }
        if (ObjectManager.Me.HealthPercent >= 0 && ObjectManager.Me.HealthPercent < 30)
        {
            if (WordOfGlory.KnownSpell && WordOfGlory.IsSpellUsable &&
                (!GlyphOfHarshWords.KnownSpell /* || cast on me */) && MySettings.UseWordOfGlory)
                WordOfGlory.Launch();
            if (DivineProtection.KnownSpell && DivineProtection.IsSpellUsable && MySettings.UseDivineProtection)
                DivineProtection.Launch();
            if (FlashOfLight.KnownSpell && FlashOfLight.IsSpellUsable && MySettings.UseFlashOfLight)
            {
                FlashOfLight.Launch();
                return;
            }
            if (HolyLight.KnownSpell && HolyLight.IsSpellUsable && MySettings.UseHolyLight)
            {
                HolyLight.Launch();
                return;
            }
            if (DivineLight.KnownSpell && DivineLight.IsSpellUsable && MySettings.UseDivineLight)
            {
                DivineLight.Launch();
                return;
            }
        }
    }

    private void DPS_Burst()
    {
        if (DivineFavor.KnownSpell && DivineFavor.IsSpellUsable)
        {
            if (AvengingWrath.KnownSpell && AvengingWrath.IsSpellUsable && MySettings.UseAvengingWrath)
            {
                AvengingWrath.Launch();
            }
            if (Lifeblood.KnownSpell && Lifeblood.IsSpellUsable && MySettings.UseLifeblood)
            {
                Lifeblood.Launch();
            }
            if (HolyAvenger.KnownSpell && HolyAvenger.IsSpellUsable && MySettings.UseHolyAvenger)
            {
                HolyAvenger.Launch();
            }
            if (MySettings.UseDivineFavor)
                DivineFavor.Launch();
            return;
        }
        else if (Lifeblood.KnownSpell && Lifeblood.IsSpellUsable && MySettings.UseLifeblood)
        {
            Lifeblood.Launch();
            return;
        }
    }

    private void DPS_Cycle()
    {
        if (HolyShock.KnownSpell && HolyShock.IsDistanceGood && HolyShock.IsSpellUsable && MySettings.UseHolyShock)
        {
            HolyShock.Launch();
            return;
        }
        if (HammerOfWrath.KnownSpell && HammerOfWrath.IsDistanceGood && HammerOfWrath.IsSpellUsable &&
            MySettings.UseHammerOfWrath)
        {
            HammerOfWrath.Launch();
            return;
        }
        if (HammerOfJustice.KnownSpell && HammerOfJustice.IsDistanceGood && HammerOfJustice.IsSpellUsable &&
            MySettings.UseHammerOfJustice)
        {
            HammerOfJustice.Launch();
            return;
        }
        if (Denounce.KnownSpell && Denounce.IsDistanceGood && Denounce.IsSpellUsable && MySettings.UseDenounce)
        {
            Denounce.Launch();
            return;
        }
    }
}

public class Paladin_Protection
{
    [Serializable]
    public class PaladinProtectionSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        public bool UseBerserking = true;
        /* Paladin Seals & Buffs */
        public bool UseSealOfTheRighteousness = true;
        public bool UseSealOfTruth = true;
        public bool UseSealOfInsight = false;
        public bool UseBlessingOfMight = true;
        public bool UseBlessingOfKings = true;
        /* Offensive Spell */
        public bool UseShieldOfTheRighteous = true;
        public bool UseConsecration = true;
        public bool UseAvengersShield = true;
        public bool UseHammerOfWrath = true;
        public bool UseCrusaderStrike = true;
        public bool UseHammerOfTheRighteous = true;
        public bool UseJudgment = true;
        public bool UseHammerOfJustice = true;
        public bool UseHolyWrath = true;
        /* Offensive Cooldown */
        public bool UseHolyAvenger = true;
        public bool UseAvengingWrath = true;
        /* Defensive Cooldown */
        public bool UseGuardianOfAncientKings = true;
        public bool UseArdentDefender = true;
        public bool UseSacredShield = true;
        public bool UseHandOfPurity = true;
        public bool UseDevotionAura = true;
        public bool UseDivineProtection = true;
        public bool UseDivineShield = true;
        public bool UseHandOfProtection = true;
        /* Healing Spell */
        public bool UseFlashOfLight = true;
        public bool UseLayOnHands = true;
        public bool UseWordOfGlory = true;

        public PaladinProtectionSettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Paladin Protection Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
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
            AddControlInWinForm("Use Shield of the Righteous", "UseShieldOfTheRighteous", "Offensive Spell");
            AddControlInWinForm("Use Consecration", "UseConsecration", "Offensive Spell");
            AddControlInWinForm("Use Avenger's Shield", "UseAvengersShield", "Offensive Spell");
            AddControlInWinForm("Use Hammer of Wrath", "UseHammerOfWrath", "Offensive Spell");
            AddControlInWinForm("Use Crusader Strike", "UseCrusaderStrike", "Offensive Spell");
            AddControlInWinForm("Use Hammer of the Righteous", "UseHammerOfTheRighteous", "Offensive Spell");
            AddControlInWinForm("Use Judgment", "UseJudgment", "Offensive Spell");
            AddControlInWinForm("Use Hammer of Justice", "UseHammerOfJustice", "Offensive Spell");
            AddControlInWinForm("Use Holy Wrath", "UseHolyWrath", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Holy Avenger", "UseHolyAvenger", "Offensive Cooldown");
            AddControlInWinForm("Use Avenging Wrath", "UseAvengingWrath", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Guardian of Ancient Kings", "UseGuardianOfAncientKings", "Defensive Cooldown");
            AddControlInWinForm("Use Ardent Defender", "UseArdentDefender", "Defensive Cooldown");
            AddControlInWinForm("Use Sacred Shield", "UseSacredShield", "Defensive Cooldown");
            AddControlInWinForm("Use Hand of Purity", "UseHandOfPurity", "Defensive Cooldown");
            AddControlInWinForm("Use Devotion Aura", "UseDevotionAura", "Defensive Cooldown");
            AddControlInWinForm("Use Divine Protection", "UseDivineProtection", "Defensive Cooldown");
            AddControlInWinForm("Use Divine Shield", "UseDivineShield", "Defensive Cooldown");
            AddControlInWinForm("Use Hand of Protection", "UseHandOfProtection", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Flash of Light", "UseFlashOfLight", "Healing Spell");
            AddControlInWinForm("Use Lay on Hands", "UseLayOnHands", "Healing Spell");
            AddControlInWinForm("Use Word of Glory", "UseWordOfGlory", "Healing Spell");
        }

        public static PaladinProtectionSettings CurrentSetting { get; set; }

        public static PaladinProtectionSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Paladin_Protection.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return CurrentSetting = Settings.Load<Paladin_Protection.PaladinProtectionSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Paladin_Protection.PaladinProtectionSettings();
            }
        }
    }

    private readonly PaladinProtectionSettings MySettings = PaladinProtectionSettings.GetSettings();

    #region Professions & Racial

    private readonly Spell ArcaneTorrent = new Spell("Arcane Torrent");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell War_Stomp = new Spell("War Stomp");
    private readonly Spell Berserking = new Spell("Berserking");

    #endregion

    #region Paladin Seals & Buffs

    private readonly Spell SealOfTheRighteousness = new Spell("Seal of Righteousness");
    private readonly Spell SealOfTruth = new Spell("Seal of Truth");
    private readonly Spell SealOfInsight = new Spell("Seal of Insight");
    private readonly Spell BlessingOfMight = new Spell("Blessing of Might");
    private readonly Spell BlessingOfKings = new Spell("Blessing of Kings");

    #endregion

    #region Offensive Spell

    private readonly Spell ShieldOfTheRighteous = new Spell("Shield of the Righteous");
    private readonly Spell Consecration = new Spell("Consecration");
    private readonly Spell AvengersShield = new Spell("Avenger's Shield");
    private readonly Spell HammerOfWrath = new Spell("Hammer of Wrath");
    private readonly Spell CrusaderStrike = new Spell("Crusader Strike");
    private readonly Spell HammerOfTheRighteous = new Spell("Hammer of the Righteous"); // 115798 = Weakened Blows
    private readonly Spell Judgment = new Spell("Judgment");
    private readonly Spell HammerOfJustice = new Spell("Hammer of Justice");
    private readonly Spell HolyWrath = new Spell("Holy Wrath");

    #endregion

    #region Offensive Cooldown

    private readonly Spell HolyAvenger = new Spell("Holy Avenger");
    private readonly Spell AvengingWrath = new Spell("Avenging Wrath");

    #endregion

    #region Defensive Cooldown

    private Timer OnCD = new Timer(0);
    private readonly Spell GuardianOfAncientKings = new Spell("Guardian of Ancient Kings");
    private readonly Spell ArdentDefender = new Spell("Ardent Defender");
    private readonly Spell SacredShield = new Spell("Sacred Shield");
    private readonly Spell HandOfPurity = new Spell("Hand Of Purity");
    private readonly Spell DevotionAura = new Spell("Devotion Aura");
    private readonly Spell DivineProtection = new Spell("Divine Protection");
    private readonly Spell DivineShield = new Spell("Divine Shield");
    private readonly Spell HandOfProtection = new Spell("Hand of Protection");

    #endregion

    #region Healing Spell

    private readonly Spell FlashOfLight = new Spell("Flash of Light");
    private readonly Spell LayOnHands = new Spell("Lay on Hands");
    private readonly Spell WordOfGlory = new Spell("Word of Glory");

    #endregion

    public Paladin_Protection()
    {
        Main.range = 5.0f;

        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
            {
                if (!ObjectManager.Me.IsMounted)
                {
                    if (Fight.InFight && ObjectManager.Me.Target > 0)
                    {
                        if (ObjectManager.Me.Target != lastTarget && Judgment.IsDistanceGood)
                        {
                            Pull();
                            lastTarget = ObjectManager.Me.Target;
                        }
                        Combat();
                        Seal();
                        Blessing();
                    }
                    else if (!ObjectManager.Me.IsCast)
                        Patrolling();
                }
            }
            catch
            {
            }
            Thread.Sleep(150);
        }
    }

    private void Pull()
    {
        if (AvengersShield.KnownSpell && MySettings.UseAvengersShield && AvengersShield.IsDistanceGood &&
            AvengersShield.IsSpellUsable)
        {
            AvengersShield.Launch();
        }
        if (Judgment.KnownSpell && MySettings.UseJudgment && Judgment.IsDistanceGood && Judgment.IsSpellUsable)
        {
            Judgment.Launch();
            Thread.Sleep(1000);
        }
        DPS_Burst();
    }

    private void Combat()
    {
        if (OnCD.IsReady)
            Defense_Cycle();

        DPS_Cycle();

        Heal();

        DPS_Burst();
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Seal();
            Blessing();
            Heal();
        }
    }

    private void Seal()
    {
        if (ObjectManager.Me.IsMounted)
            return;
        else if (SealOfTruth.KnownSpell && MySettings.UseSealOfTruth &&
                 (ObjectManager.GetNumberAttackPlayer() <= 7 || !MySettings.UseSealOfTheRighteousness))
        {
            if (!SealOfTruth.HaveBuff && SealOfTruth.IsSpellUsable)
                SealOfTruth.Launch();
        }
        else if (SealOfTheRighteousness.KnownSpell && MySettings.UseSealOfTheRighteousness)
        {
            if (!SealOfTheRighteousness.HaveBuff && SealOfTheRighteousness.IsSpellUsable)
                SealOfTheRighteousness.Launch();
        }
        else if (SealOfInsight.KnownSpell && MySettings.UseSealOfInsight)
        {
            if (!SealOfInsight.HaveBuff && SealOfInsight.IsSpellUsable)
                SealOfInsight.Launch();
        }
    }

    private void Blessing()
    {
        if (ObjectManager.Me.IsMounted)
            return;
        else if (BlessingOfMight.KnownSpell && MySettings.UseBlessingOfMight)
        {
            if (!BlessingOfMight.HaveBuff && BlessingOfMight.IsSpellUsable && MySettings.UseBlessingOfMight)
                BlessingOfMight.Launch();
        }
        else if (BlessingOfKings.KnownSpell && MySettings.UseBlessingOfKings && MySettings.UseBlessingOfKings)
        {
            if (!BlessingOfKings.HaveBuff && BlessingOfKings.IsSpellUsable)
                BlessingOfKings.Launch();
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 95 && !Fight.InFight && ObjectManager.GetNumberAttackPlayer() == 0)
        {
            if (FlashOfLight.KnownSpell && FlashOfLight.IsSpellUsable && MySettings.UseFlashOfLight)
            {
                FlashOfLight.Launch();
                MovementManager.StopMove();
                return;
            }
        }
        if (DivineShield.KnownSpell && MySettings.UseDivineShield && ObjectManager.Me.HealthPercent > 0 &&
            ObjectManager.Me.HealthPercent <= 20 && !ObjectManager.Me.HaveBuff(25771) && DivineShield.IsSpellUsable)
        {
            DivineShield.Launch();
            return;
        }
        if (LayOnHands.KnownSpell && MySettings.UseLayOnHands && ObjectManager.Me.HealthPercent > 0 &&
            ObjectManager.Me.HealthPercent <= 20 && !ObjectManager.Me.HaveBuff(25771) && LayOnHands.IsSpellUsable)
        {
            LayOnHands.Launch();
            return;
        }
        if (HandOfProtection.KnownSpell && MySettings.UseHandOfProtection && ObjectManager.Me.HealthPercent > 0 &&
            ObjectManager.Me.HealthPercent <= 20 && !ObjectManager.Me.HaveBuff(25771) && HandOfProtection.IsSpellUsable)
        {
            HandOfProtection.Launch();
            return;
        }
        if (ObjectManager.Me.ManaPercentage < 10)
        {
            if (ArcaneTorrent.KnownSpell && MySettings.UseArcaneTorrent && ArcaneTorrent.IsSpellUsable)
            {
                ArcaneTorrent.Launch();
                return;
            }
        }
        if (ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 50)
        {
            if (WordOfGlory.KnownSpell && MySettings.UseWordOfGlory && WordOfGlory.IsSpellUsable)
                WordOfGlory.Launch();
            if (FlashOfLight.KnownSpell && MySettings.UseFlashOfLight && FlashOfLight.IsSpellUsable)
            {
                FlashOfLight.Launch();
                return;
            }
        }
        if (ObjectManager.Me.HealthPercent >= 0 && ObjectManager.Me.HealthPercent < 30)
        {
            if (WordOfGlory.KnownSpell && MySettings.UseWordOfGlory && WordOfGlory.IsSpellUsable)
                WordOfGlory.Launch();
            if (DivineProtection.KnownSpell && MySettings.UseDivineProtection && DivineProtection.IsSpellUsable)
                DivineProtection.Launch();
            if (FlashOfLight.KnownSpell && MySettings.UseFlashOfLight && FlashOfLight.IsSpellUsable)
            {
                FlashOfLight.Launch();
                return;
            }
        }
    }

    private void DPS_Burst()
    {
        if (HolyAvenger.KnownSpell && MySettings.UseHolyAvenger && HolyAvenger.IsSpellUsable)
        {
            HolyAvenger.Launch();
            if (AvengingWrath.KnownSpell && MySettings.UseAvengingWrath && AvengingWrath.IsSpellUsable)
            {
                AvengingWrath.Launch();
                return;
            }
        }
        else if (AvengingWrath.KnownSpell && MySettings.UseAvengingWrath && AvengingWrath.IsSpellUsable)
        {
            AvengingWrath.Launch();
            return;
        }
    }

    private void Defense_Cycle()
    {
        if (HandOfPurity.KnownSpell && MySettings.UseHandOfPurity && HandOfPurity.IsSpellUsable &&
            !HandOfPurity.HaveBuff)
        {
            HandOfPurity.Launch();
            OnCD = new Timer(1000*6);
        }
        else if (HammerOfJustice.KnownSpell && MySettings.UseHammerOfJustice && HammerOfJustice.IsSpellUsable)
        {
            HammerOfJustice.Launch();
            OnCD = new Timer(1000*6);
            return;
        }
        else if (DivineProtection.KnownSpell && MySettings.UseDivineProtection && DivineProtection.IsSpellUsable)
        {
            DivineProtection.Launch();
            OnCD = new Timer(1000*10);
            return;
        }
        else if (DevotionAura.KnownSpell && MySettings.UseDevotionAura && DevotionAura.IsSpellUsable)
        {
            DevotionAura.Launch();
            OnCD = new Timer(1000*6);
            return;
        }
        else if (GuardianOfAncientKings.KnownSpell && MySettings.UseGuardianOfAncientKings &&
                 GuardianOfAncientKings.IsSpellUsable)
        {
            GuardianOfAncientKings.Launch();
            OnCD = new Timer(1000*12);
            return;
        }
        else if (ArdentDefender.KnownSpell && MySettings.UseArdentDefender &&
                 ArdentDefender.IsSpellUsable)
        {
            ArdentDefender.Launch();
            OnCD = new Timer(1000*10);
            return;
        }
        else if (WordOfGlory.KnownSpell && MySettings.UseWordOfGlory && WordOfGlory.IsSpellUsable)
        {
            WordOfGlory.Launch();
            OnCD = new Timer(1000*5);
            return;
        }
    }

    private void DPS_Cycle()
    {
        if (ShieldOfTheRighteous.KnownSpell && MySettings.UseShieldOfTheRighteous && ShieldOfTheRighteous.IsSpellUsable &&
            ShieldOfTheRighteous.IsDistanceGood && (ObjectManager.Me.HaveBuff(90174) || ObjectManager.Me.HolyPower >= 3))
        {
            ShieldOfTheRighteous.Launch();
            return;
        }
        if ((ObjectManager.GetNumberAttackPlayer() >= 2 || !ObjectManager.Target.HaveBuff(115798)) &&
            !ObjectManager.Me.HaveBuff(90174) && ObjectManager.Me.HolyPower < 3)
        {
            if (HammerOfTheRighteous.KnownSpell && MySettings.UseHammerOfTheRighteous &&
                HammerOfTheRighteous.IsDistanceGood && HammerOfTheRighteous.IsSpellUsable)
            {
                HammerOfTheRighteous.Launch();
                return;
            }
        }
        else
        {
            if (CrusaderStrike.KnownSpell && MySettings.UseCrusaderStrike && CrusaderStrike.IsDistanceGood &&
                CrusaderStrike.IsSpellUsable && !ObjectManager.Me.HaveBuff(90174) && ObjectManager.Me.HolyPower < 3)
            {
                CrusaderStrike.Launch();
                return;
            }
        }
        if (AvengersShield.KnownSpell && MySettings.UseAvengersShield && AvengersShield.IsDistanceGood &&
            AvengersShield.IsSpellUsable && !ObjectManager.Me.HaveBuff(90174) && ObjectManager.Me.HolyPower < 3)
        {
            AvengersShield.Launch();
            return;
        }
        if (HammerOfWrath.KnownSpell && MySettings.UseHammerOfWrath && HammerOfWrath.IsDistanceGood &&
            HammerOfWrath.IsSpellUsable && !ObjectManager.Me.HaveBuff(90174) && ObjectManager.Me.HolyPower < 3)
        {
            HammerOfWrath.Launch();
            return;
        }
        if (Judgment.KnownSpell && MySettings.UseJudgment && Judgment.IsDistanceGood && Judgment.IsSpellUsable &&
            !ObjectManager.Me.HaveBuff(90174) && ObjectManager.Me.HolyPower < 3)
        {
            Judgment.Launch();
            return;
        }
        if (Consecration.KnownSpell && MySettings.UseConsecration && Consecration.IsSpellUsable &&
            !ObjectManager.Me.HaveBuff(90174) && ObjectManager.Me.HolyPower < 3)
        {
            // Consecration.Launch(); // TODO: Check if Glyph of Consecration is used.
            SpellManager.CastSpellByIDAndPosition(26573, ObjectManager.Target.Position);
            return;
        }
        if (HolyWrath.KnownSpell && MySettings.UseHolyWrath && HolyWrath.IsSpellUsable &&
            !ObjectManager.Me.HaveBuff(90174) && ObjectManager.Me.HolyPower < 3 && !Judgment.IsSpellUsable &&
            !CrusaderStrike.IsSpellUsable && !Consecration.IsSpellUsable)
        {
            HolyWrath.Launch();
            return;
        }
    }
}

public class Paladin_Retribution
{
    [Serializable]
    public class PaladinRetributionSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        public bool UseBerserking = true;
        /* Paladin Seals & Buffs */
        public bool UseSealOfTheRighteousness = true;
        public bool UseSealOfTruth = true;
        public bool UseSealOfJustice = false;
        public bool UseSealOfInsight = false;
        public bool UseBlessingOfMight = true;
        public bool UseBlessingOfKings = true;
        /* Offensive Spell */
        public bool UseTemplarsVerdict = true;
        public bool UseDivineStorm = true;
        public bool UseExorcism = true;
        public bool UseHammerOfWrath = true;
        public bool UseCrusaderStrike = true;
        public bool UseHammerOfTheRighteous = true;
        public bool UseJudgment = true;
        public bool UseHammerOfJustice = true;
        /* Offensive Cooldown */
        public bool UseInquisition = true;
        public bool UseGuardianOfAncientKings = true;
        public bool UseHolyAvenger = true;
        public bool UseAvengingWrath = true;
        /* Defensive Cooldown */
        public bool UseDivineProtection = true;
        public bool UseDevotionAura = true;
        public bool UseSacredShield = true;
        public bool UseDivineShield = true;
        public bool UseHandOfProtection = false;
        /* Healing Spell */
        public bool UseFlashOfLight = true;
        public bool UseLayOnHands = true;
        public bool UseWordOfGlory = true;

        public PaladinRetributionSettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Paladin Retribution Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            /* Paladin Seals & Buffs */
            AddControlInWinForm("Use Seal of the Righteousness", "UseSealOfTheRighteousness", "Paladin Seals & Buffs");
            AddControlInWinForm("Use Seal of Truth", "UseSealOfTruth", "Paladin Seals & Buffs");
            AddControlInWinForm("Use Seal of Justice", "UseSealOfJustice", "Paladin Seals & Buffs");
            AddControlInWinForm("Use Seal of Insight", "UseSealOfInsight", "Paladin Seals & Buffs");
            AddControlInWinForm("Use Blessing of Might", "UseBlessingOfMight", "Paladin Seals & Buffs");
            AddControlInWinForm("Use Blessing of Kings", "UseBlessingOfKings", "Paladin Seals & Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Templar's Verdict", "UseTemplarsVerdict", "Offensive Spell");
            AddControlInWinForm("Use Divine Storm", "UseDivineStorm", "Offensive Spell");
            AddControlInWinForm("Use Exorcism", "UseExorcism", "Offensive Spell");
            AddControlInWinForm("Use Hammer of Wrath", "UseHammerOfWrath", "Offensive Spell");
            AddControlInWinForm("Use Crusader Strike", "UseCrusaderStrike", "Offensive Spell");
            AddControlInWinForm("Use Hammer of the Righteous", "UseHammerOfTheRighteous", "Offensive Spell");
            AddControlInWinForm("Use Judgment", "UseJudgment", "Offensive Spell");
            AddControlInWinForm("Use Hammer of Justice", "UseHammerOfJustice", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Inquisition", "UseInquisition", "Offensive Cooldown");
            AddControlInWinForm("Use Guardian of Ancient Kings", "UseGuardianOfAncientKings", "Offensive Cooldown");
            AddControlInWinForm("Use Holy Avenger", "UseHolyAvenger", "Offensive Cooldown");
            AddControlInWinForm("Use Avenging Wrath", "UseAvengingWrath", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Divine Protection", "UseDivineProtection", "Defensive Cooldown");
            AddControlInWinForm("Use Devotion Aura", "UseDevotionAura", "Defensive Cooldown");
            AddControlInWinForm("Use Sacred Shield", "UseSacredShield", "Defensive Cooldown");
            AddControlInWinForm("Use Divine Shield", "UseDivineShield", "Defensive Cooldown");
            AddControlInWinForm("Use Hand of Protection", "UseHandOfProtection", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Flash of Light", "UseFlashOfLight", "Healing Spell");
            AddControlInWinForm("Use Lay on Hands", "UseLayOnHands", "Healing Spell");
            AddControlInWinForm("Use Word of Glory", "UseWordOfGlory", "Healing Spell");
        }

        public static PaladinRetributionSettings CurrentSetting { get; set; }

        public static PaladinRetributionSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Paladin_Retribution.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Paladin_Retribution.PaladinRetributionSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Paladin_Retribution.PaladinRetributionSettings();
            }
        }
    }

    private readonly PaladinRetributionSettings MySettings = PaladinRetributionSettings.GetSettings();

    #region Professions & Racials

    private readonly Spell ArcaneTorrent = new Spell("Arcane Torrent");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell War_Stomp = new Spell("War Stomp");
    private readonly Spell Berserking = new Spell("Berserking");

    #endregion

    #region Paladin Seals & Buffs

    private readonly Spell SealOfTheRighteousness = new Spell("Seal of Righteousness");
    private readonly Spell SealOfTruth = new Spell("Seal of Truth");
    private readonly Spell SealOfJustice = new Spell("Seal of Justice");
    private readonly Spell SealOfInsight = new Spell("Seal of Insight");
    private readonly Spell BlessingOfMight = new Spell("Blessing of Might");
    private readonly Spell BlessingOfKings = new Spell("Blessing of Kings");

    #endregion

    #region Offensive Spell

    private readonly Spell TemplarsVerdict = new Spell("Templar's Verdict");
    private readonly Spell BoundlessConviction = new Spell("Boundless Conviction");
    private readonly Spell DivineStorm = new Spell("Divine Storm");
    private readonly Spell Exorcism = new Spell("Exorcism");
    private readonly Spell HammerOfWrath = new Spell("Hammer of Wrath");
    private readonly Spell CrusaderStrike = new Spell("Crusader Strike");
    private readonly Spell HammerOfTheRighteous = new Spell("Hammer of the Righteous");
    private readonly Spell Judgment = new Spell("Judgment");
    private readonly Spell HammerOfJustice = new Spell("Hammer of Justice");

    #endregion

    #region Offensive Cooldown

    private readonly Spell Inquisition = new Spell("Inquisition");
    private Timer InquisitionToUseInPriotiy = new Timer(0);
    private readonly Spell GuardianOfAncientKings = new Spell("Guardian of Ancient Kings");
    private Timer BurstTime = new Timer(0);
    private readonly Spell HolyAvenger = new Spell("Holy Avenger");
    private readonly Spell AvengingWrath = new Spell("Avenging Wrath");

    #endregion

    #region Defensive Cooldown

    private readonly Spell DivineProtection = new Spell("Divine Protection");
    private readonly Spell DevotionAura = new Spell("Devotion Aura");
    private readonly Spell SacredShield = new Spell("Sacred Shield");
    private readonly Spell DivineShield = new Spell("Divine Shield");
    private readonly Spell HandOfProtection = new Spell("Hand of Protection");

    #endregion

    #region Healing Spell

    private readonly Spell FlashOfLight = new Spell("Flash of Light");
    private readonly Spell LayOnHands = new Spell("Lay on Hands");
    private readonly Spell WordOfGlory = new Spell("Word of Glory");

    #endregion

    public Paladin_Retribution()
    {
        Main.range = 5.0f;

        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
            {
                if (!ObjectManager.Me.IsMounted)
                {
                    if (Fight.InFight && ObjectManager.Me.Target > 0)
                    {
                        if (ObjectManager.Me.Target != lastTarget &&
                            (Judgment.IsDistanceGood || Exorcism.IsDistanceGood))
                        {
                            Pull();
                            lastTarget = ObjectManager.Me.Target;
                        }
                        Combat();
                        Seal();
                        Blessing();
                    }
                    else if (!ObjectManager.Me.IsCast)
                        Patrolling();
                }
            }
            catch
            {
            }
            Thread.Sleep(150);
        }
    }

    private void Pull()
    {
        if (Exorcism.KnownSpell && Exorcism.IsDistanceGood && Exorcism.IsSpellUsable && MySettings.UseExorcism)
        {
            Exorcism.Launch();
        }
        else if (Judgment.KnownSpell && Judgment.IsDistanceGood && Judgment.IsSpellUsable && MySettings.UseJudgment)
        {
            Judgment.Launch();
        }
    }

    private void Combat()
    {
        DPS_Cycle();

        DPS_Burst();

        DPS_Cycle();

        Heal();

        DPS_Cycle();
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Seal();
            Blessing();
            Heal();
        }
    }

    private void Seal()
    {
        if (SealOfTruth.KnownSpell && ObjectManager.GetNumberAttackPlayer() <= 7 && MySettings.UseSealOfTruth)
        {
            if (!SealOfTruth.HaveBuff && SealOfTruth.IsSpellUsable)
                SealOfTruth.Launch();
        }
        else if (SealOfTheRighteousness.KnownSpell && MySettings.UseSealOfTheRighteousness)
        {
            if (!SealOfTheRighteousness.HaveBuff && SealOfTheRighteousness.IsSpellUsable)
                SealOfTheRighteousness.Launch();
        }
        else if (SealOfJustice.KnownSpell && MySettings.UseSealOfJustice)
        {
            if (!SealOfJustice.HaveBuff && SealOfJustice.IsSpellUsable)
                SealOfJustice.Launch();
        }
        else if (SealOfInsight.KnownSpell && MySettings.UseSealOfInsight)
        {
            if (!SealOfInsight.HaveBuff && SealOfInsight.IsSpellUsable)
                SealOfInsight.Launch();
        }
    }

    private void Blessing()
    {
        if (ObjectManager.Me.IsMounted)
            return;
        else if (BlessingOfKings.KnownSpell && MySettings.UseBlessingOfKings && !MySettings.UseBlessingOfMight)
        {
            if (!BlessingOfKings.HaveBuff && BlessingOfKings.IsSpellUsable)
                BlessingOfKings.Launch();
        }
        else if (BlessingOfMight.KnownSpell && MySettings.UseBlessingOfMight)
        {
            if (!BlessingOfMight.HaveBuff && BlessingOfMight.IsSpellUsable && MySettings.UseBlessingOfMight)
                BlessingOfMight.Launch();
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 95 && !Fight.InFight && ObjectManager.GetNumberAttackPlayer() == 0)
        {
            if (FlashOfLight.KnownSpell && FlashOfLight.IsSpellUsable && MySettings.UseFlashOfLight)
            {
                FlashOfLight.Launch();
                MovementManager.StopMove();
                return;
            }
        }
        if (DivineShield.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent <= 5 &&
            !ObjectManager.Me.HaveBuff(25771) && DivineShield.IsSpellUsable && MySettings.UseDivineShield)
        {
            DivineShield.Launch();
            return;
        }
        if (LayOnHands.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent <= 20 &&
            !ObjectManager.Me.HaveBuff(25771) && LayOnHands.IsSpellUsable && MySettings.UseLayOnHands)
        {
            LayOnHands.Launch();
            return;
        }
        if (ObjectManager.Me.ManaPercentage < 10)
        {
            if (ArcaneTorrent.KnownSpell && ArcaneTorrent.IsSpellUsable && MySettings.UseArcaneTorrent)
            {
                ArcaneTorrent.Launch();
                return;
            }
        }
        if (ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent < 50)
        {
            if (WordOfGlory.KnownSpell && WordOfGlory.IsSpellUsable && MySettings.UseWordOfGlory)
                WordOfGlory.Launch();
            if (FlashOfLight.KnownSpell && FlashOfLight.IsSpellUsable && MySettings.UseFlashOfLight)
            {
                FlashOfLight.Launch();
                return;
            }
        }
        if (ObjectManager.Me.HealthPercent >= 0 && ObjectManager.Me.HealthPercent < 30)
        {
            if (WordOfGlory.KnownSpell && WordOfGlory.IsSpellUsable && MySettings.UseWordOfGlory)
                WordOfGlory.Launch();
            if (DivineProtection.KnownSpell && DivineProtection.IsSpellUsable && MySettings.UseDivineProtection)
                DivineProtection.Launch();
            if (FlashOfLight.KnownSpell && FlashOfLight.IsSpellUsable && MySettings.UseFlashOfLight)
            {
                FlashOfLight.Launch();
                return;
            }
        }
    }

    private void DPS_Burst()
    {
        if (!GuardianOfAncientKings.KnownSpell || GuardianOfAncientKings.HaveBuff ||
            !GuardianOfAncientKings.IsSpellUsable)
        {
            if ((!GuardianOfAncientKings.KnownSpell || BurstTime.IsReady) && AvengingWrath.KnownSpell &&
                AvengingWrath.IsSpellUsable && (!HolyAvenger.KnownSpell || HolyAvenger.IsSpellUsable))
            {
                if (MySettings.UseAvengingWrath)
                    AvengingWrath.Launch();
                if ((!Inquisition.HaveBuff || InquisitionToUseInPriotiy.IsReady) && Inquisition.KnownSpell &&
                    MySettings.UseInquisition && Inquisition.IsSpellUsable &&
                    (ObjectManager.Me.HaveBuff(90174) || ObjectManager.Me.HolyPower >= 3))
                {
                    Inquisition.Launch();
                    InquisitionToUseInPriotiy = new Timer(1000*(10*3 - 6));
                }
                if (HolyAvenger.KnownSpell && HolyAvenger.IsSpellUsable && MySettings.UseHolyAvenger)
                    HolyAvenger.Launch();
                return;
            }
            else if ((!GuardianOfAncientKings.KnownSpell || BurstTime.IsReady) && HolyAvenger.KnownSpell &&
                     HolyAvenger.IsSpellUsable && MySettings.UseHolyAvenger)
            {
                HolyAvenger.Launch();
                if ((!Inquisition.HaveBuff || InquisitionToUseInPriotiy.IsReady) && Inquisition.KnownSpell &&
                    MySettings.UseInquisition && Inquisition.IsSpellUsable &&
                    (ObjectManager.Me.HaveBuff(90174) || ObjectManager.Me.HolyPower >= 3))
                {
                    Inquisition.Launch();
                    InquisitionToUseInPriotiy = new Timer(1000*(10*3 - 6));
                }
                if (AvengingWrath.KnownSpell && AvengingWrath.IsSpellUsable && MySettings.UseAvengingWrath)
                    AvengingWrath.Launch();
                return;
            }
        }
        else if (GuardianOfAncientKings.KnownSpell && GuardianOfAncientKings.IsSpellUsable &&
                 MySettings.UseGuardianOfAncientKings && AvengingWrath.IsSpellUsable &&
                 (!HolyAvenger.KnownSpell || HolyAvenger.IsSpellUsable))
        {
            GuardianOfAncientKings.Launch();
            BurstTime = new Timer(1000*6.5);
            return;
        }
    }

    private void DPS_Cycle()
    {
        if (HammerOfJustice.KnownSpell && HammerOfJustice.IsDistanceGood && HammerOfJustice.IsSpellUsable &&
            MySettings.UseHammerOfJustice)
        {
            // TODO : If target can be stun, if not, it will be a pure loss of DPS.
            HammerOfJustice.Launch();
            return;
        }
        if (Inquisition.KnownSpell && (!Inquisition.HaveBuff || InquisitionToUseInPriotiy.IsReady) &&
            Inquisition.IsSpellUsable && MySettings.UseInquisition &&
            (ObjectManager.Me.HaveBuff(90174) || ObjectManager.Me.HolyPower >= 3))
        {
            Inquisition.Launch();
            InquisitionToUseInPriotiy = new Timer(1000*(10*3 - 6));
            return;
        }
        else if ((ObjectManager.GetNumberAttackPlayer() <= 1 ||
                  (!MySettings.UseDivineStorm && MySettings.UseTemplarsVerdict)) && TemplarsVerdict.KnownSpell &&
                 (!Inquisition.KnownSpell || Inquisition.HaveBuff) && TemplarsVerdict.IsSpellUsable &&
                 TemplarsVerdict.IsDistanceGood &&
                 (ObjectManager.Me.HaveBuff(90174) || ObjectManager.Me.HolyPower == 5 ||
                  (ObjectManager.Me.HolyPower >= 3 && (!BoundlessConviction.KnownSpell || HolyAvenger.HaveBuff))))
        {
            TemplarsVerdict.Launch();
            return;
        }
        else if ((ObjectManager.GetNumberAttackPlayer() >= 2 ||
                  (MySettings.UseDivineStorm && !MySettings.UseTemplarsVerdict)) && DivineStorm.KnownSpell &&
                 MySettings.UseDivineStorm && (!Inquisition.KnownSpell || Inquisition.HaveBuff) &&
                 DivineStorm.IsSpellUsable && DivineStorm.IsDistanceGood &&
                 (ObjectManager.Me.HaveBuff(90174) || ObjectManager.Me.HolyPower == 5 ||
                  (ObjectManager.Me.HolyPower >= 3 && (!BoundlessConviction.KnownSpell || HolyAvenger.HaveBuff))))
        {
            DivineStorm.Launch();
            return;
        }
        else if (HammerOfWrath.KnownSpell && HammerOfWrath.IsDistanceGood && HammerOfWrath.IsSpellUsable &&
                 MySettings.UseHammerOfWrath)
        {
            HammerOfWrath.Launch();
            return;
        }
        else if (Exorcism.KnownSpell && Exorcism.IsDistanceGood && Exorcism.IsSpellUsable &&
                 MySettings.UseExorcism)
        {
            Exorcism.Launch();
            return;
        }
        else if ((ObjectManager.GetNumberAttackPlayer() <= 3 || ObjectManager.Target.HaveBuff(115798) ||
                  (MySettings.UseCrusaderStrike && !MySettings.UseHammerOfTheRighteous)) &&
                 CrusaderStrike.KnownSpell && CrusaderStrike.IsDistanceGood &&
                 CrusaderStrike.IsSpellUsable)
        {
            CrusaderStrike.Launch();
            return;
        }
        else if ((ObjectManager.GetNumberAttackPlayer() >= 4 ||
                  !ObjectManager.Target.HaveBuff(115798) ||
                  (!MySettings.UseCrusaderStrike && !MySettings.UseHammerOfTheRighteous)) &&
                 HammerOfTheRighteous.KnownSpell && HammerOfTheRighteous.IsDistanceGood &&
                 HammerOfTheRighteous.IsSpellUsable && !ObjectManager.Me.HaveBuff(90174))
        {
            HammerOfTheRighteous.Launch();
            return;
        }
        else if (Judgment.KnownSpell && Judgment.IsDistanceGood && Judgment.IsSpellUsable &&
                 MySettings.UseJudgment)
        {
            Judgment.Launch();
            return;
        }
        else if ((ObjectManager.GetNumberAttackPlayer() <= 1 ||
                  (!MySettings.UseDivineStorm && MySettings.UseTemplarsVerdict)) &&
                 TemplarsVerdict.KnownSpell &&
                 (!Inquisition.KnownSpell || Inquisition.HaveBuff) &&
                 TemplarsVerdict.IsSpellUsable && TemplarsVerdict.IsDistanceGood &&
                 (ObjectManager.Me.HaveBuff(90174) || ObjectManager.Me.HolyPower >= 3))
        {
            TemplarsVerdict.Launch();
            return;
        }
        else if ((ObjectManager.GetNumberAttackPlayer() >= 2 ||
                  (MySettings.UseDivineStorm && !MySettings.UseTemplarsVerdict)) &&
                 DivineStorm.KnownSpell &&
                 (!Inquisition.KnownSpell || Inquisition.HaveBuff) &&
                 DivineStorm.IsSpellUsable && DivineStorm.IsDistanceGood &&
                 (ObjectManager.Me.HaveBuff(90174) || ObjectManager.Me.HolyPower >= 3))
        {
            DivineStorm.Launch();
            return;
        }
        else if (MySettings.UseSacredShield && SacredShield.KnownSpell &&
                 SacredShield.IsDistanceGood && SacredShield.IsSpellUsable
                 && (!Inquisition.KnownSpell || Inquisition.HaveBuff)
                 && (!TemplarsVerdict.KnownSpell || TemplarsVerdict.IsSpellUsable)
                 && (!Judgment.KnownSpell || Judgment.IsSpellUsable)
                 && (!Judgment.KnownSpell || Judgment.IsSpellUsable)
                 && (!CrusaderStrike.KnownSpell || CrusaderStrike.IsSpellUsable)
                 && (!HammerOfWrath.KnownSpell || HammerOfWrath.IsSpellUsable)
                 && (!Exorcism.KnownSpell || Exorcism.IsSpellUsable))
        {
            // Since we have a GCD available and spell to cast, we can use the Sacred Shield. (Loss of DPS if not placed here.)
            SacredShield.Launch();
            return;
        }
        else
            return;
    }
}

#endregion

#region Shaman

public class Ele
{
    #region InitializeSpell

    // ELE ONLY
    private Spell Thunderstorm = new Spell("Thunderstorm");
    private Spell Lava_Burst = new Spell("Lava Burst");
    private Spell Elemental_Mastery = new Spell("Elemental Mastery");
    private Spell Flametongue_Weapon = new Spell("Flametongue Weapon");

    // SKILL
    private Spell Lightning_Bolt = new Spell("Lightning Bolt");
    private Spell Chain_Lightning = new Spell("Chain Lightning");
    private Spell Flame_Shock = new Spell("Flame Shock");
    private Spell Earth_Shock = new Spell("Earth Shock");
    private Spell Lightning_Shield = new Spell("Lightning Shield");
    private Spell Water_Shield = new Spell("Water Shield");
    private Spell Unleash_Elements = new Spell("Unleash Elements");

    // TIMER
    private Timer look = new Timer(0);
    private Timer fighttimer = new Timer(0);
    private Timer weaponbuff = new Timer(30000);

    // BUFF & HELPING
    private Spell Stoneclaw_Totem = new Spell("Stoneclaw Totem");
    private Spell Call_of_the_Elements = new Spell("Call of the Elements");
    private Spell Bloodlust = new Spell("Bloodlust");
    private Spell Heroism = new Spell("Heroism");
    private Spell Wind_Shear = new Spell("Wind Shear");
    private Spell Healing_Wave = new Spell("Healing Wave");
    private Spell Healing_Surge = new Spell("Healing Surge");
    private Spell Earth_Elemental_Totem = new Spell("Earth Elemental Totem");

    // profession & racials
    private Spell ArcaneTorrent = new Spell("Arcane Torrent");
    private Spell Lifeblood = new Spell("Lifeblood");
    private Spell Stoneform = new Spell("Stoneform");
    private Spell Tailoring = new Spell("Tailoring");
    private Spell Leatherworking = new Spell("Leatherworking");
    private Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private Spell War_Stomp = new Spell("War Stomp");
    private Spell Berserking = new Spell("Berserking");

    #endregion InitializeSpell

    public Ele()
    {
        Main.range = 30.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            if (!ObjectManager.Me.IsMounted)
            {
                buffoutfight();

                if (ItemsManager.GetItemCountByIdLUA(67495) > 0) Logging.WriteFight("/use item:67495");

                if (!Fight.InFight && look.IsReady)
                {
                    look = new Timer(5000);
                    Lua.RunMacroText("/targetfriendplayer");
                }

                if (Fight.InFight && ObjectManager.Me.Target > 0 && ObjectManager.Target.GetDistance > Main.range)
                {
                    fighttimer = new Timer(60000);
                }

                if (Fight.InFight && ObjectManager.Me.Target > 0)
                {
                    if (ObjectManager.Me.Target != lastTarget && ObjectManager.Target.GetDistance <= Main.range)
                    {
                        pull();
                        lastTarget = ObjectManager.Me.Target;
                    }
                    fight();
                    if (!Fight.InFight)
                    {
                        Logging.WriteFight(" - Target Down - ");
                        look = new Timer(5000);
                    }

                    if (fighttimer.IsReady && ObjectManager.Target.HealthPercent > 90 && ObjectManager.Me.Target > 0)
                    {
                        Logging.WriteFight(" - Target Evading - ");
                        break;
                    }
                }
            }
            Thread.Sleep(350);
        }
    }

    public void pull()
    {
        if (hardmob()) Logging.WriteFight(" -  Pull Hard Mob - ");
        if (!hardmob()) Logging.WriteFight(" -  Pull Easy Mob - ");
        Lua.RunMacroText("/petattack");
        fighttimer = new Timer(60000);
    }

    public void buffoutfight()
    {
        selfheal();

        if (Fight.InFight || ObjectManager.Me.IsDeadMe) return;

        if (!ObjectManager.Me.HaveBuff(79640) &&
            ItemsManager.GetItemCountByIdLUA(58149) == 1)
        {
            Logging.WriteFight("Use Alchi Flask");
            Lua.RunMacroText("/use item:58149");
        }

        if (!Call_of_the_Elements.KnownSpell && !ObjectManager.Me.HaveBuff(77747))
        {
            SpellManager.CastSpellByIdLUA(8227);
            // Flametongue_Totem.Launch();
        }
    }

    public void fight()
    {
        selfheal();
        buffinfight();
        if (ObjectManager.GetNumberAttackPlayer() > 1) fighttimer = new Timer(60000);

        if (!ObjectManager.Target.IsTargetingMe && Lightning_Bolt.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(403);
            // Lightning_Bolt.Launch();
            return;
        }

        if (Thunderstorm.KnownSpell && Thunderstorm.IsSpellUsable && ObjectManager.Me.ManaPercentage < 80 &&
            ObjectManager.Target.GetDistance < 10)
        {
            SpellManager.CastSpellByIdLUA(51490);
            // Thunderstorm.Launch();
        }

        if (!Flame_Shock.TargetHaveBuff && Flame_Shock.IsSpellUsable && Flame_Shock.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(8050);
            Thread.Sleep(50);
            SpellManager.CastSpellByIdLUA(51505);
            // Flame_Shock.Launch();
        }

        if (Lava_Burst.KnownSpell && Lava_Burst.IsSpellUsable && Lava_Burst.IsDistanceGood && Flame_Shock.TargetHaveBuff)
        {
            SpellManager.CastSpellByIdLUA(51505);
            // Lava_Burst.Launch();
            return;
        }

        if (Chain_Lightning.KnownSpell && Chain_Lightning.IsSpellUsable && Chain_Lightning.IsDistanceGood &&
            ObjectManager.GetNumberAttackPlayer() > 1)
        {
            SpellManager.CastSpellByIdLUA(421);
            // Chain_Lightning.Launch();
        }

        if (ObjectManager.Target.IsCast && Wind_Shear.KnownSpell && Wind_Shear.IsSpellUsable &&
            Wind_Shear.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(57994);
            // Wind_Shear.Launch();
        }

        if (Earth_Shock.IsSpellUsable && Earth_Shock.KnownSpell && Earth_Shock.IsDistanceGood)
        {
            if (!Flame_Shock.TargetHaveBuff && Flame_Shock.KnownSpell) return;
            SpellManager.CastSpellByIdLUA(8042);
            // Earth_Shock.Launch();
        }

        if (Berserking.KnownSpell && Berserking.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1454);
            // Berserking.Launch();
        }

        if (Unleash_Elements.KnownSpell && Unleash_Elements.IsSpellUsable && Unleash_Elements.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(73680);
            //Unleash_Elements.Launch();
        }

        if (Lightning_Bolt.IsSpellUsable && Lightning_Bolt.IsDistanceGood && !Lava_Burst.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(403);
            // Lightning_Bolt.Launch();
        }
    }

    private void buffinfight()
    {
        if (Flametongue_Weapon.KnownSpell && Flametongue_Weapon.IsSpellUsable && weaponbuff.IsReady)
        {
            Flametongue_Weapon.Launch();
            weaponbuff = new Timer(1800000);
        }

        if (Call_of_the_Elements.KnownSpell && Call_of_the_Elements.IsSpellUsable && !ObjectManager.Me.HaveBuff(77747))
        {
            SpellManager.CastSpellByIdLUA(66842);
            // Call_of_the_Elements.Launch();
        }

        if (ObjectManager.GetNumberAttackPlayer() > 3 || hardmob())
        {
            if (Elemental_Mastery.KnownSpell && Elemental_Mastery.IsSpellUsable)
            {
                SpellManager.CastSpellByIdLUA(16166);
                // Elemental_Mastery.Launch();
            }

            if (Heroism.KnownSpell && Heroism.IsSpellUsable)
            {
                SpellManager.CastSpellByIdLUA(32182);
                // Heroism.Launch();
            }

            if (Bloodlust.KnownSpell && Bloodlust.IsSpellUsable)
            {
                SpellManager.CastSpellByIdLUA(2825);
                // Bloodlust.Launch();
            }

            if (Earth_Elemental_Totem.KnownSpell && Earth_Elemental_Totem.IsSpellUsable &&
                ObjectManager.GetNumberAttackPlayer() > 4)
            {
                SpellManager.CastSpellByIdLUA(2062);
                // Earth_Elemental_Totem.Launch();
            }
        }

        if (!Lightning_Shield.HaveBuff && Lightning_Shield.KnownSpell && Lightning_Shield.IsSpellUsable &&
            Water_Shield.IsSpellUsable && ObjectManager.Me.ManaPercentage > 50)
        {
            SpellManager.CastSpellByIdLUA(324);
            // Lightning_Shield.Launch();
        }

        if (!Water_Shield.HaveBuff && Water_Shield.KnownSpell && Water_Shield.IsSpellUsable &&
            ObjectManager.Me.ManaPercentage < 20)
        {
            SpellManager.CastSpellByIdLUA(52127);
            // Water_Shield.Launch();
        }

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            Stoneform.KnownSpell && Stoneform.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(20594);
            // Stoneform.Launch();
        }

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            War_Stomp.KnownSpell && War_Stomp.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(20549);
            // War_Stomp.Launch();
        }
    }

    private void selfheal()
    {
        if (ObjectManager.Me.HealthPercent < 80 &&
            Lifeblood.KnownSpell && Lifeblood.IsSpellUsable)
        {
            Lifeblood.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 80 &&
            Gift_of_the_Naaru.KnownSpell && Gift_of_the_Naaru.IsSpellUsable)
        {
            Gift_of_the_Naaru.Launch();
        }

        if (ArcaneTorrent.KnownSpell && ArcaneTorrent.IsSpellUsable &&
            ObjectManager.Target.IsCast && ObjectManager.Target.GetDistance < 8)
        {
            ArcaneTorrent.Launch();
        }

        if (Stoneclaw_Totem.KnownSpell && Stoneclaw_Totem.IsSpellUsable && ObjectManager.Me.HealthPercent < 50 &&
            ObjectManager.GetNumberAttackPlayer() > 1)
        {
            SpellManager.CastSpellByIdLUA(5730);
            // Stoneclaw_Totem.Launch();
        }

        if (Healing_Surge.KnownSpell && Healing_Surge.IsSpellUsable && ObjectManager.Me.HealthPercent < 75)
        {
            SpellManager.CastSpellByIdLUA(8004);
            // Healing_Surge.Launch();
            return;
        }

        if (!Healing_Surge.KnownSpell && Healing_Wave.KnownSpell && Healing_Wave.IsSpellUsable &&
            ObjectManager.Me.HealthPercent < 75)
        {
            SpellManager.CastSpellByIdLUA(331);
            // Healing_Wave.Launch();
        }
    }

    public bool hardmob()
    {
        if (((ObjectManager.Target.MaxHealth*100)/ObjectManager.Me.MaxHealth) > 110)
        {
            return true;
        }
        return false;
    }
}

#endregion

#region Priest

public class Priest_Shadow
{
    [Serializable]
    public class PriestShadowSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Priest Buffs */
        public bool UseInnerFire = true;
        public bool UseInnerWill = false;
        public bool UseLevitate = false;
        public bool UsePowerWordFortitude = true;
        public bool UseShadowform = true;
        /* Offensive Spell */
        public bool UseDevouringPlague = true;
        public bool UseMindBlast = true;
        public bool UseMindFlay = true;
        public bool UseMindSear = true;
        public bool UseMindSpike = true;
        public bool UseShadowWordDeath = true;
        public bool UseShadowWordInsanity = true;
        public bool UseShadowWordPain = true;
        public bool UseVampiricTouch = true;
        /* Offensive Cooldown */
        public bool UsePowerInfusion = true;
        public bool UseShadowfiend = true;
        /* Defensive Cooldown */
        public bool UseDispersion = true;
        public bool UsePowerWordShield = true;
        public bool UsePsychicHorror = true;
        public bool UsePsychicScream = true;
        public bool UsePsyfiend = true;
        public bool UseSilence = true;
        public bool UseSpectralGuise = true;
        public bool UseVoidTendrils = true;
        /* Healing Spell */
        public bool UseDesperatePrayer = true;
        public bool UseFlash_Heal = true;
        public bool UseHymnofHope = true;
        public bool UsePrayerofMending = true;
        public bool UseRenew = true;
        public bool UseVampiricEmbrace = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;

        public PriestShadowSettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Shadow Priest Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Priest Buffs */
            AddControlInWinForm("Use Inner Fire", "UseInnerFire", "Priest Buffs");
            AddControlInWinForm("Use Inner Will", "UseInnerWill", "Priest Buffs");
            AddControlInWinForm("Use Levitate", "UseLevitate", "Priest Buffs");
            AddControlInWinForm("Use Power Word: Fortitude", "UsePowerWordFortitude", "Priest Buffs");
            AddControlInWinForm("Use Shadowform", "UseShadowform", "Priest Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Devouring Plague", "UseDevoringPlague", "Offensive Spell");
            AddControlInWinForm("Use Mind Blast", "UseMindBlast", "Offensive Spell");
            AddControlInWinForm("Use Mind Flay", "UseMindFlay", "Offensive Spell");
            AddControlInWinForm("Use Mind Sear", "UseMindSear", "Offensive Spell");
            AddControlInWinForm("Use Mind Spike", "UseMindSpike", "Offensive Spell");
            AddControlInWinForm("Use Shadow Word: Death", "UseShadowWordDeath", "Offensive Spell");
            AddControlInWinForm("Use Shadow Word: Insanity", "UseShadowWordInsanity", "Offensive Spell");
            AddControlInWinForm("Use Shadow Word: Pain", "UseShadowWordPain", "Offensive Spell");
            AddControlInWinForm("Use Vampiric Touch", "UseVampiricTouch", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Power Infusion", "UsePowerInfusion", "Offensive Cooldown");
            AddControlInWinForm("Use Shadowfiend", "UseShadowfiend", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Dispersion", "UseDispersion", "Defensive Cooldown");
            AddControlInWinForm("Use Power Word: Shield", "UsePowerWordShield", "Defensive Cooldown");
            AddControlInWinForm("Use Psychic Horror", "UsePsychicHorror", "Defensive Cooldown");
            AddControlInWinForm("Use Psychic Scream", "UsePsychicScream", "Defensive Cooldown");
            AddControlInWinForm("Use Psyfiend", "UsePsyfiend", "Defensive Cooldown");
            AddControlInWinForm("Use Silence", "UseSilence", "Defensive Cooldown");
            AddControlInWinForm("Use Spectral Guise", "UseSpectralGuise", "Defensive Cooldown");
            AddControlInWinForm("Use Void Tendrils", "UseVoidTendrils", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Desperate Prayer", "UseDesperatePrayer", "Healing Spell");
            AddControlInWinForm("Use Flash Heal", "UseFlash_Heal", "Healing Spell");
            AddControlInWinForm("Use Hymn of Hope", "UseHymnofHope", "Healing Spell");
            AddControlInWinForm("Use Prayer of Mending", "UsePrayerofMending", "Healing Spell");
            AddControlInWinForm("Use Renew", "UseRenew", "Healing Spell");
            AddControlInWinForm("Use Vampiric Embrace", "UseVampiricEmbrace", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static PriestShadowSettings CurrentSetting { get; set; }

        public static PriestShadowSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Priest_Shadow.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Priest_Shadow.PriestShadowSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Priest_Shadow.PriestShadowSettings();
            }
        }
    }

    private readonly PriestShadowSettings MySettings = PriestShadowSettings.GetSettings();

    #region Professions & Racials

    private readonly Spell Arcane_Torrent = new Spell("Arcane Torrent");
    private readonly Spell Berserking = new Spell("Berserking");
    private readonly Spell Blood_Fury = new Spell("Blood Fury");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell War_Stomp = new Spell("War Stomp");
    private readonly Spell Engineering = new Spell("Engineering");
    private readonly Spell Alchemy = new Spell("Alchemy");

    #endregion

    #region Priest Buffs

    private readonly Spell Inner_Fire = new Spell("Inner Fire");
    private readonly Spell Inner_Will = new Spell("Inner Will");
    private readonly Spell Levitate = new Spell("Levitate");
    private Timer Levitate_Timer = new Timer(0);
    private readonly Spell Power_Word_Fortitude = new Spell("Power Word: Fortitude");
    private readonly Spell Shadowform = new Spell("Shadowform");

    #endregion

    #region Offensive Spell

    private readonly Spell Devouring_Plague = new Spell("Devouring Plague");
    private Timer Devouring_Plague_Timer = new Timer(0);
    private readonly Spell Mind_Blast = new Spell("Mind Blast");
    private readonly Spell Mind_Flay = new Spell("Mind Flay");
    private readonly Spell Mind_Sear = new Spell("Mind Sear");
    private readonly Spell Mind_Spike = new Spell("Mind Spike");
    private readonly Spell Shadow_Word_Death = new Spell("Shadow Word: Death");
    private readonly Spell Shadow_Word_Insanity = new Spell("Shadow Word: Insanity");
    private readonly Spell Shadow_Word_Pain = new Spell("Shadow Word: Pain");
    private Timer Shadow_Word_Pain_Timer = new Timer(0);
    private readonly Spell Smite = new Spell("Smite");
    private readonly Spell Vampiric_Touch = new Spell("Vampiric Touch");
    private Timer Vampiric_Touch_Timer = new Timer(0);

    #endregion

    #region Offensive Cooldown

    private readonly Spell Power_Infusion = new Spell("Power Infusion");
    private readonly Spell Shadowfiend = new Spell("Shadowfiend");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Dispersion = new Spell("Dispersion");
    private readonly Spell Power_Word_Shield = new Spell("Power Word: Shield");
    private readonly Spell Psychic_Horror = new Spell("Psychic Horror");
    private readonly Spell Psychic_Scream = new Spell("Psychic Scream");
    private readonly Spell Psyfiend = new Spell("Psyfiend");
    private readonly Spell Silence = new Spell("Silence");
    private readonly Spell Spectral_Guise = new Spell("Spectral Guise");
    private readonly Spell Void_Tendrils = new Spell("Void Tendrils");

    #endregion

    #region Healing Spell

    private readonly Spell Desperate_Prayer = new Spell("Desperate Prayer");
    private readonly Spell Flash_Heal = new Spell("Flash Heal");
    private readonly Spell Hymn_of_Hope = new Spell("Hymn of Hope");
    private readonly Spell Prayer_of_Mending = new Spell("Prayer of Mending");
    private readonly Spell Renew = new Spell("Renew");
    private Timer Renew_Timer = new Timer(0);
    private readonly Spell Vampiric_Embrace = new Spell("Vampiric Embrace");

    #endregion

    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    public int LC = 0;

    public Priest_Shadow()
    {
        Main.range = 30.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            try
            {
                if (!ObjectManager.Me.IsMounted)
                {
                    Buff_Levitate();
                    if (Fight.InFight && ObjectManager.Me.Target > 0)
                    {
                        if (ObjectManager.Me.Target != lastTarget &&
                            (Mind_Spike.IsDistanceGood || Shadow_Word_Pain.IsDistanceGood))
                        {
                            Pull();
                            lastTarget = ObjectManager.Me.Target;
                        }

                        if (ObjectManager.Target.Level < 70 && ObjectManager.Me.Level > 84
                            && MySettings.UseLowCombat)
                        {
                            LC = 1;
                            LowCombat();
                        }
                        else
                        {
                            LC = 0;
                            Combat();
                        }
                    }
                    else if (!ObjectManager.Me.IsCast)
                        Patrolling();
                }
            }
            catch
            {
            }
            Thread.Sleep(250);
        }
    }

    public void Pull()
    {
        if (Devouring_Plague.IsSpellUsable && Devouring_Plague.KnownSpell && Devouring_Plague.IsDistanceGood
            && ObjectManager.Me.ShadowOrbs == 3 && MySettings.UseDevouringPlague)
        {
            Devouring_Plague.Launch();
            return;
        }
        else
        {
            if (Shadow_Word_Pain.IsSpellUsable && Shadow_Word_Pain.KnownSpell && Shadow_Word_Pain.IsDistanceGood
                && MySettings.UseShadowWordPain)
            {
                Shadow_Word_Pain.Launch();
                return;
            }
        }
    }

    public void Combat()
    {
        AvoidMelee();
        if (OnCD.IsReady)
            Defense_Cycle();
        Heal();
        Decast();
        Buff();
        DPS_Burst();
        DPS_Cycle();
    }

    private void Buff_Levitate()
    {
        if (!Fight.InFight && Levitate.KnownSpell && Levitate.IsSpellUsable && MySettings.UseLevitate
            && (!Levitate.HaveBuff || Levitate_Timer.IsReady))
        {
            Levitate.Launch();
            Levitate_Timer = new Timer(1000*60*9);
        }
    }

    public void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Power_Word_Fortitude.KnownSpell && Power_Word_Fortitude.IsSpellUsable &&
            !Power_Word_Fortitude.HaveBuff && MySettings.UsePowerWordFortitude)
        {
            Power_Word_Fortitude.Launch();
            return;
        }
        else if (Inner_Fire.KnownSpell && Inner_Fire.IsSpellUsable && !Inner_Fire.HaveBuff
                 && MySettings.UseInnerFire)
        {
            Inner_Fire.Launch();
            return;
        }
        else if (Inner_Will.KnownSpell && Inner_Will.IsSpellUsable && !Inner_Will.HaveBuff
                 && !MySettings.UseInnerFire && MySettings.UseInnerWill)
        {
            Inner_Will.Launch();
            return;
        }
        else if (AlchFlask_Timer.IsReady && MySettings.UseAlchFlask
                 && ItemsManager.GetItemCountByIdLUA(75525) == 1)
        {
            Logging.WriteFight("Use Alchi Flask");
            Lua.RunMacroText("/use item:75525");
            AlchFlask_Timer = new Timer(1000*60*60*2);
        }
        else
        {
            if (!Shadowform.HaveBuff && Shadowform.KnownSpell && Shadowform.IsSpellUsable
                && MySettings.UseShadowform)
            {
                Shadowform.Launch();
                return;
            }
        }
    }

    public void LowCombat()
    {
        AvoidMelee();
        Heal();
        Defense_Cycle();
        Buff();

        if (Devouring_Plague.IsSpellUsable && Devouring_Plague.KnownSpell && Devouring_Plague.IsDistanceGood
            && ObjectManager.Me.ShadowOrbs == 3 && MySettings.UseDevouringPlague)
        {
            Devouring_Plague.Launch();
            return;
        }
        else if (Mind_Spike.KnownSpell && Mind_Spike.IsSpellUsable && Mind_Spike.IsDistanceGood
                 && MySettings.UseMindSpike)
        {
            Mind_Spike.Launch();
            if (ObjectManager.Target.HealthPercent < 50 && ObjectManager.Target.HealthPercent > 0)
            {
                Mind_Spike.Launch();
                return;
            }
            return;
        }
        else
        {
            if (Mind_Sear.KnownSpell && Mind_Sear.IsSpellUsable && Mind_Sear.IsDistanceGood
                && MySettings.UseMindSear)
            {
                Mind_Sear.Launch();
                return;
            }
        }
    }

    public void DPS_Burst()
    {
        if (MySettings.UseTrinket && Trinket_Timer.IsReady && ObjectManager.Target.GetDistance < 30)
        {
            Logging.WriteFight("Use Trinket 1.");
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Logging.WriteFight("Use Trinket 2.");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Trinket_Timer = new Timer(1000*60*2);
            return;
        }
        else if (Berserking.IsSpellUsable && Berserking.KnownSpell && MySettings.UseBerserking
                 && ObjectManager.Target.GetDistance < 30)
        {
            Berserking.Launch();
            return;
        }
        else if (Blood_Fury.IsSpellUsable && Blood_Fury.KnownSpell && MySettings.UseBloodFury
                 && ObjectManager.Target.GetDistance < 30)
        {
            Blood_Fury.Launch();
            return;
        }
        else if (Lifeblood.IsSpellUsable && Lifeblood.KnownSpell && MySettings.UseLifeblood
                 && ObjectManager.Target.GetDistance < 30)
        {
            Lifeblood.Launch();
            return;
        }
        else if (MySettings.UseEngGlove && Engineering.KnownSpell && Engineering_Timer.IsReady
                 && ObjectManager.Target.GetDistance < 30)
        {
            Logging.WriteFight("Use Engineering Gloves.");
            Lua.RunMacroText("/use 10");
            Engineering_Timer = new Timer(1000*60);
            return;
        }
        else if (Power_Infusion.IsSpellUsable && Power_Infusion.KnownSpell
                 && MySettings.UsePowerInfusion && ObjectManager.Target.GetDistance < 30)
        {
            Power_Infusion.Launch();
            return;
        }
        else
        {
            if (Shadowfiend.IsSpellUsable && Shadowfiend.KnownSpell && Shadowfiend.IsDistanceGood
                && MySettings.UseShadowfiend)
            {
                Shadowfiend.Launch();
                return;
            }
        }
    }

    public void DPS_Cycle()
    {
        if (ObjectManager.Me.BarTwoPercentage < 80 && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable
            && MySettings.UseArcaneTorrent)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && Mind_Sear.IsSpellUsable && Mind_Sear.KnownSpell
                 && Mind_Sear.IsDistanceGood && !ObjectManager.Me.IsCast && MySettings.UseMindSear)
        {
            Mind_Sear.Launch();
            return;
        }
        else if (Shadow_Word_Pain.KnownSpell && Shadow_Word_Pain.IsSpellUsable
                 && Shadow_Word_Pain.IsDistanceGood && MySettings.UseShadowWordPain
                 && (!Shadow_Word_Pain.TargetHaveBuff || Shadow_Word_Pain_Timer.IsReady))
        {
            Shadow_Word_Pain.Launch();
            Shadow_Word_Pain_Timer = new Timer(1000*14);
            return;
        }
        else if (Shadow_Word_Insanity.KnownSpell && Shadow_Word_Insanity.IsDistanceGood
                 && Shadow_Word_Insanity.IsSpellUsable && MySettings.UseShadowWordInsanity)
        {
            Shadow_Word_Insanity.Launch();
            Shadow_Word_Pain_Timer = new Timer(0);
            return;
        }
        else if (Vampiric_Touch.KnownSpell && Vampiric_Touch.IsSpellUsable
                 && Vampiric_Touch.IsDistanceGood && MySettings.UseVampiricTouch
                 && (!Vampiric_Touch.TargetHaveBuff || Vampiric_Touch_Timer.IsReady))
        {
            Vampiric_Touch.Launch();
            Vampiric_Touch_Timer = new Timer(1000*11);
            return;
        }
        else if (Shadow_Word_Death.IsSpellUsable && Shadow_Word_Death.IsDistanceGood && Shadow_Word_Death.KnownSpell
                 && ObjectManager.Target.HealthPercent < 20 && MySettings.UseShadowWordDeath)
        {
            Shadow_Word_Death.Launch();
            return;
        }
        else if (Mind_Spike.IsSpellUsable && Mind_Spike.IsDistanceGood && Mind_Spike.KnownSpell &&
                 ObjectManager.Me.HaveBuff(87160) && MySettings.UseMindSpike)
        {
            Mind_Spike.Launch();
            return;
        }
        else if (Devouring_Plague.KnownSpell && Devouring_Plague.IsSpellUsable && Devouring_Plague.IsDistanceGood &&
                 ObjectManager.Me.ShadowOrbs == 3 && MySettings.UseDevouringPlague
                 && (!Devouring_Plague.TargetHaveBuff || Devouring_Plague_Timer.IsReady))
        {
            Devouring_Plague.Launch();
            Devouring_Plague_Timer = new Timer(1000*3);
            return;
        }
        else if (Mind_Blast.KnownSpell && Mind_Blast.IsSpellUsable && Mind_Blast.IsDistanceGood
                 && ObjectManager.Me.ShadowOrbs < 3 && MySettings.UseMindBlast)
        {
            Mind_Blast.Launch();
            return;
        }
        else if (!ObjectManager.Me.IsCast && Mind_Flay.IsSpellUsable && Mind_Flay.KnownSpell && Mind_Flay.IsDistanceGood
                 && MySettings.UseMindFlay)
        {
            Mind_Flay.Launch();
            return;
        }
            // Blizzard API Calls for Mind Flay using Smite Function
        else if (!ObjectManager.Me.IsCast && Smite.IsSpellUsable && Smite.KnownSpell && Smite.IsDistanceGood
                 && MySettings.UseMindFlay)
        {
            Smite.Launch();
            return;
        }
        else
        {
            if (!ObjectManager.Me.IsCast && Smite.IsSpellUsable && Smite.KnownSpell && Smite.IsDistanceGood)
            {
                Smite.Launch();
                return;
            }
        }
        return;
    }

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Heal();
            Buff();
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 95 && !Fight.InFight && ObjectManager.GetNumberAttackPlayer() == 0)
        {
            if (Flash_Heal.KnownSpell && Flash_Heal.IsSpellUsable && MySettings.UseFlash_Heal)
            {
                Flash_Heal.Launch(false);
                return;
            }
        }
        else if (!Fight.InFight && ObjectManager.Me.BarTwoPercentage < 40 && Hymn_of_Hope.KnownSpell &&
                 Hymn_of_Hope.IsSpellUsable
                 && ObjectManager.GetNumberAttackPlayer() == 0 && MySettings.UseHymnofHope)
        {
            Hymn_of_Hope.Launch(false);
            return;
        }
        else if (!Fight.InFight && ObjectManager.Me.BarTwoPercentage < 60 && ObjectManager.GetNumberAttackPlayer() == 0
                 && Dispersion.KnownSpell && Dispersion.IsSpellUsable && MySettings.UseDispersion)
        {
            Dispersion.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 65 && Desperate_Prayer.KnownSpell && Desperate_Prayer.IsSpellUsable
                 && MySettings.UseDesperatePrayer)
        {
            Desperate_Prayer.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 60 && Flash_Heal.KnownSpell && Flash_Heal.IsSpellUsable
                 && MySettings.UseFlash_Heal)
        {
            Flash_Heal.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell
                 && MySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && War_Stomp.IsSpellUsable && War_Stomp.KnownSpell
                 && MySettings.UseWarStomp && ObjectManager.Target.GetDistance < 8)
        {
            War_Stomp.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Vampiric_Embrace.IsSpellUsable && Vampiric_Embrace.KnownSpell
                 && MySettings.UseVampiricEmbrace)
        {
            Vampiric_Embrace.Launch();
            return;
        }
        else if (Power_Word_Shield.KnownSpell && Power_Word_Shield.IsSpellUsable
                 && !Power_Word_Shield.HaveBuff && MySettings.UsePowerWordShield
                 && !ObjectManager.Me.HaveBuff(6788))
        {
            Power_Word_Shield.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 50 && Prayer_of_Mending.KnownSpell && Prayer_of_Mending.IsSpellUsable
                 && MySettings.UsePrayerofMending)
        {
            Prayer_of_Mending.Launch();
            return;
        }
        else
        {
            if (Renew.KnownSpell && Renew.IsSpellUsable && !Renew.HaveBuff &&
                ObjectManager.Me.HealthPercent < 90 && MySettings.UseRenew)
            {
                Renew.Launch();
                return;
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 20 && Psychic_Scream.IsSpellUsable && Psychic_Scream.KnownSpell
            && MySettings.UsePsychicScream)
        {
            Psychic_Scream.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 20 && Dispersion.KnownSpell && Dispersion.IsSpellUsable
                 && MySettings.UseDispersion)
        {
            if (Renew.KnownSpell && Renew.IsSpellUsable && MySettings.UseRenew)
            {
                Renew.Launch();
                Thread.Sleep(1500);
            }
            Dispersion.Launch();
            OnCD = new Timer(1000*6);
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() >= 2 && ObjectManager.Me.HealthPercent < 35 &&
                 Void_Tendrils.IsSpellUsable && Void_Tendrils.KnownSpell
                 && MySettings.UseVoidTendrils)
        {
            Void_Tendrils.Launch();
            OnCD = new Timer(1000*10);
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() >= 2 && ObjectManager.Me.HealthPercent < 35 &&
                 Psyfiend.IsSpellUsable &&
                 Psyfiend.KnownSpell
                 && MySettings.UsePsyfiend)
        {
            Psyfiend.Launch();
            OnCD = new Timer(1000*10);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 70 && Spectral_Guise.IsSpellUsable && Spectral_Guise.KnownSpell
                 && MySettings.UseSpectralGuise)
        {
            if (Renew.KnownSpell && Renew.IsSpellUsable && MySettings.UseRenew)
            {
                Renew.Launch();
                Thread.Sleep(1500);
            }
            Spectral_Guise.Launch();
            OnCD = new Timer(1000*3);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell
                 && MySettings.UseStoneform)
        {
            Stoneform.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 80 && War_Stomp.IsSpellUsable && War_Stomp.KnownSpell
                && MySettings.UseWarStomp)
            {
                War_Stomp.Launch();
                OnCD = new Timer(1000*2);
                return;
            }
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseSilence
            && Silence.KnownSpell && Silence.IsSpellUsable && Silence.IsDistanceGood)
        {
            Silence.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UsePsychicHorror
                && Psychic_Horror.KnownSpell && Psychic_Horror.IsSpellUsable && Psychic_Horror.IsDistanceGood)
            {
                Psychic_Horror.Launch();
                return;
            }
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 1)
        {
            Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "{DOWN}");
        }
    }
}

#endregion

#region Rogue

public class RogueCom
{
    #region InitializeSpell

    private Spell DeadlyPoison = new Spell(2823);
    private Spell CripplingPoison = new Spell(3408);
    private Spell MindNumbingPoison = new Spell(5761);
    private Spell WoundPoison = new Spell(8679);
    private Spell MasterPoisoner = new Spell(58410);
    private Spell Blade_Flurry = new Spell("Blade Flurry");
    private Spell Sinister_Strike = new Spell("Sinister Strike");
    private Spell Slice_and_Dice = new Spell("Slice and Dice");
    private Spell Premeditation = new Spell("Premeditation");
    private Spell Revealing_Strike = new Spell("Revealing Strike");
    private Spell Rupture = new Spell("Rupture");
    private Spell Eviscerate = new Spell("Eviscerate");
    private Spell Adrenaline_Rush = new Spell("Adrenaline Rush");
    private Spell Killing_Spree = new Spell("Killing Spree");
    private Spell Kick = new Spell("Kick");
    private Spell Evasion = new Spell("Evasion");
    private Spell Sprint = new Spell("Sprint");
    private Spell Combat_Readiness = new Spell("Combat Readiness");
    private Spell Cloak_of_Shadows = new Spell("Cloak of Shadows");
    private Spell Stealth = new Spell("Stealth");
    private Spell Sap = new Spell("Sap");
    private Spell Pick_Pocket = new Spell("Pick Pocket");
    private Spell Recuperate = new Spell("Recuperate");
    private Spell Vanish = new Spell("Vanish");
    private Spell Kidney_Shot = new Spell("Kidney Shot");
    private Spell Cheap_Shot = new Spell("Cheap Shot");

    private Spell Deadly_Poison = new Spell("Deadly Poison");
    private Spell Wound_Poison = new Spell("Wound Poison");
    private Spell Instant_Poison = new Spell("Instant Poison");
    private Spell Crippling_Poison = new Spell("Crippling Poison");
    private Spell MindNumbing_Poison = new Spell("Mind-Numbing Poison");

    #endregion InitializeSpell

    public RogueCom()
    {
        Main.range = 5.0f;

        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            if (!ObjectManager.Me.IsMounted)
            {
                Patrolling();

                if (Fight.InFight && ObjectManager.Me.Target > 0)
                {
                    if (ObjectManager.Me.Target != lastTarget && ObjectManager.Target.GetDistance <= 50.0f)
                    {
                        Pull();
                        lastTarget = ObjectManager.Me.Target;
                    }

                    Combat();
                }
            }
            Thread.Sleep(350);
        }
    }

    public void Pull()
    {
        if (!Stealth.HaveBuff && Stealth.KnownSpell && Stealth.IsSpellUsable)
            Stealth.Launch();
    }

    public void Patrolling()
    {
        if (DeadlyPoison.KnownSpell)
        {
            if (!DeadlyPoison.HaveBuff && DeadlyPoison.IsSpellUsable)
            {
                DeadlyPoison.Launch();
                return;
            }
            else if (MasterPoisoner.KnownSpell && MasterPoisoner.IsSpellUsable && !MasterPoisoner.HaveBuff)
            {
                MasterPoisoner.Launch();
                return;
            }
            else if (WoundPoison.KnownSpell && WoundPoison.IsSpellUsable && !WoundPoison.HaveBuff &&
                     !MasterPoisoner.HaveBuff)
            {
                WoundPoison.Launch();
                return;
            }
            else if (MindNumbingPoison.KnownSpell && MindNumbingPoison.IsSpellUsable && !MindNumbingPoison.HaveBuff &&
                     !WoundPoison.HaveBuff && !MasterPoisoner.HaveBuff)
            {
                MindNumbingPoison.Launch();
                return;
            }
            else if (CripplingPoison.KnownSpell && CripplingPoison.IsSpellUsable && !CripplingPoison.HaveBuff &&
                     !MindNumbingPoison.HaveBuff && !WoundPoison.HaveBuff && !MasterPoisoner.HaveBuff)
            {
                CripplingPoison.Launch();
                return;
            }
        }
    }

    public void Combat()
    {
        AvoidMelee();
        Heal();
        BuffCombat();
        Decast();

        while (Stealth.HaveBuff && Cheap_Shot.KnownSpell && !Cheap_Shot.TargetHaveBuff)
        {
            if (!Sap.TargetHaveBuff && Sap.KnownSpell && Sap.IsSpellUsable && Sap.IsDistanceGood)
                Sap.Launch();

            if (Cheap_Shot.IsSpellUsable && Cheap_Shot.IsDistanceGood)
            {
                Cheap_Shot.Launch();
                return;
            }
        }

        if (ObjectManager.GetNumberAttackPlayer() >= 2 && Blade_Flurry.KnownSpell && Blade_Flurry.IsSpellUsable &&
            Blade_Flurry.IsDistanceGood)
            Blade_Flurry.Launch();

        if (ObjectManager.Me.HealthPercent <= 35 && !Kidney_Shot.TargetHaveBuff && Kidney_Shot.KnownSpell &&
            Kidney_Shot.IsSpellUsable && Kidney_Shot.IsDistanceGood)
            Kidney_Shot.Launch();

        if (ObjectManager.Me.ComboPoint > 1 && ObjectManager.Me.ComboPoint <= 3 && Slice_and_Dice.KnownSpell &&
            !Slice_and_Dice.HaveBuff)
        {
            if (Slice_and_Dice.IsSpellUsable && Slice_and_Dice.IsDistanceGood)
            {
                Slice_and_Dice.Launch();
                return;
            }
        }

        if (ObjectManager.Me.ComboPoint >= 4 && Eviscerate.KnownSpell)
        {
            if (Eviscerate.IsSpellUsable && Eviscerate.IsDistanceGood)
            {
                Eviscerate.Launch();
                return;
            }
        }

        if (Sinister_Strike.KnownSpell && Sinister_Strike.IsSpellUsable && Sinister_Strike.IsDistanceGood)
        {
            Sinister_Strike.Launch();
            return;
        }
    }

    public void Heal()
    {
        if (!Recuperate.HaveBuff && ObjectManager.Me.ComboPoint <= 3 && ObjectManager.Me.ComboPoint > 0 &&
            ObjectManager.Me.HealthPercent <= 50 && Recuperate.KnownSpell && Recuperate.IsSpellUsable &&
            Recuperate.IsDistanceGood)
        {
            Recuperate.Launch();
        }

        if (ObjectManager.Me.HealthPercent <= 10 && Vanish.KnownSpell && Vanish.IsSpellUsable && Vanish.IsDistanceGood)
        {
            Vanish.Launch();
            Thread.Sleep(5000);
        }

        if (ObjectManager.GetNumberAttackPlayer() > 3 && Vanish.KnownSpell && Vanish.IsSpellUsable &&
            Vanish.IsDistanceGood)
        {
            Vanish.Launch();
            Thread.Sleep(5000);
            return;
        }
    }

    public void BuffCombat()
    {
        if (ObjectManager.Target.GetDistance < 5 && Adrenaline_Rush.KnownSpell && Adrenaline_Rush.IsSpellUsable)
            Adrenaline_Rush.Launch();

        if (ObjectManager.Target.GetDistance < 5 && Evasion.KnownSpell && Evasion.IsSpellUsable)
            Evasion.Launch();

        if (Combat_Readiness.KnownSpell && Combat_Readiness.IsSpellUsable)
            Combat_Readiness.Launch();

        Lua.RunMacroText("/use 13");
        Lua.RunMacroText("/use 14");
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && Kick.KnownSpell && Kick.IsSpellUsable && Kick.IsDistanceGood)
            Kick.Launch();

        if (ObjectManager.Target.IsCast && Cloak_of_Shadows.KnownSpell && Cloak_of_Shadows.IsSpellUsable)
            Cloak_of_Shadows.Launch();
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 1)
            Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "{DOWN}");
    }
}

public class RogueAssa
{
    #region InitializeSpell

    private Spell DeadlyPoison = new Spell(2823);
    private Spell CripplingPoison = new Spell(3408);
    private Spell MindNumbingPoison = new Spell(5761);
    private Spell WoundPoison = new Spell(8679);
    private Spell MasterPoisoner = new Spell(58410);
    private Spell Blade_Flurry = new Spell("Blade Flurry");
    private Spell Sinister_Strike = new Spell("Sinister Strike");
    private Spell Slice_and_Dice = new Spell("Slice and Dice");
    private Spell Premeditation = new Spell("Premeditation");
    private Spell Revealing_Strike = new Spell("Revealing Strike");
    private Spell Rupture = new Spell("Rupture");
    private Spell Eviscerate = new Spell("Eviscerate");
    private Spell Adrenaline_Rush = new Spell("Adrenaline Rush");
    private Spell Killing_Spree = new Spell("Killing Spree");
    private Spell Kick = new Spell("Kick");
    private Spell Evasion = new Spell("Evasion");
    private Spell Sprint = new Spell("Sprint");
    private Spell Combat_Readiness = new Spell("Combat Readiness");
    private Spell Cloak_of_Shadows = new Spell("Cloak of Shadows");
    private Spell Stealth = new Spell("Stealth");
    private Spell Sap = new Spell("Sap");
    private Spell Pick_Pocket = new Spell("Pick Pocket");
    private Spell Recuperate = new Spell("Recuperate");
    private Spell Vanish = new Spell("Vanish");
    private Spell Kidney_Shot = new Spell("Kidney Shot");
    private Spell Cheap_Shot = new Spell("Cheap Shot");
    private Spell Mutilate = new Spell("Mutilate");
    private Spell Envenom = new Spell("Envenom");
    private Spell Vendetta = new Spell("Vendetta");
    private Spell Cold_Blood = new Spell("Cold Blood");

    private Spell Deadly_Poison = new Spell("Deadly Poison");
    private Spell Wound_Poison = new Spell("Wound Poison");
    private Spell Instant_Poison = new Spell("Instant Poison");
    private Spell Crippling_Poison = new Spell("Crippling Poison");
    private Spell MindNumbing_Poison = new Spell("Mind-Numbing Poison");

    #endregion InitializeSpell

    public RogueAssa()
    {
        Main.range = 5.0f;

        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            if (!ObjectManager.Me.IsMounted)
            {
                Patrolling();

                if (Fight.InFight && ObjectManager.Me.Target > 0)
                {
                    if (ObjectManager.Me.Target != lastTarget && ObjectManager.Target.GetDistance <= 50.0f)
                    {
                        Pull();
                        lastTarget = ObjectManager.Me.Target;
                    }

                    Combat();
                }
            }
            Thread.Sleep(350);
        }
    }

    public void Pull()
    {
        if (!Stealth.HaveBuff && Stealth.KnownSpell && Stealth.IsSpellUsable)
            Stealth.Launch();
    }

    public void Patrolling()
    {
        if (DeadlyPoison.KnownSpell)
        {
            if (!DeadlyPoison.HaveBuff && DeadlyPoison.IsSpellUsable)
            {
                DeadlyPoison.Launch();
                return;
            }
            else if (MasterPoisoner.KnownSpell && MasterPoisoner.IsSpellUsable && !MasterPoisoner.HaveBuff)
            {
                MasterPoisoner.Launch();
                return;
            }
            else if (WoundPoison.KnownSpell && WoundPoison.IsSpellUsable && !WoundPoison.HaveBuff &&
                     !MasterPoisoner.HaveBuff)
            {
                WoundPoison.Launch();
                return;
            }
            else if (MindNumbingPoison.KnownSpell && MindNumbingPoison.IsSpellUsable && !MindNumbingPoison.HaveBuff &&
                     !WoundPoison.HaveBuff && !MasterPoisoner.HaveBuff)
            {
                MindNumbingPoison.Launch();
                return;
            }
            else if (CripplingPoison.KnownSpell && CripplingPoison.IsSpellUsable && !CripplingPoison.HaveBuff &&
                     !MindNumbingPoison.HaveBuff && !WoundPoison.HaveBuff && !MasterPoisoner.HaveBuff)
            {
                CripplingPoison.Launch();
                return;
            }
        }
    }

    public void Combat()
    {
        AvoidMelee();
        Heal();
        BuffCombat();
        Decast();

        if ((Stealth.HaveBuff || Vanish.HaveBuff) && Cheap_Shot.KnownSpell && !Cheap_Shot.TargetHaveBuff)
        {
            if (!Sap.TargetHaveBuff && Sap.KnownSpell && Sap.IsSpellUsable && Sap.IsDistanceGood)
                Sap.Launch();

            if (Cheap_Shot.IsSpellUsable && Cheap_Shot.IsDistanceGood)
            {
                Cheap_Shot.Launch();
                return;
            }
        }

        if (ObjectManager.Me.HealthPercent <= 30 && !Kidney_Shot.TargetHaveBuff && Kidney_Shot.KnownSpell &&
            Kidney_Shot.IsSpellUsable && Kidney_Shot.IsDistanceGood)
            Kidney_Shot.Launch();

        if (ObjectManager.Me.ComboPoint <= 3 && ObjectManager.Me.ComboPoint > 1 && Slice_and_Dice.KnownSpell &&
            !Slice_and_Dice.HaveBuff)
        {
            if (Slice_and_Dice.IsSpellUsable && Slice_and_Dice.IsDistanceGood)
            {
                Slice_and_Dice.Launch();
                return;
            }
        }

        if (ObjectManager.Me.ComboPoint <= 3 && ObjectManager.Me.ComboPoint > 1 && Rupture.KnownSpell &&
            Slice_and_Dice.HaveBuff && !Rupture.TargetHaveBuff)
        {
            if (Rupture.IsSpellUsable && Rupture.IsDistanceGood)
            {
                Rupture.Launch();
                return;
            }
        }

        if (ObjectManager.Me.ComboPoint >= 4 && Envenom.KnownSpell)
        {
            if (Envenom.IsSpellUsable && Envenom.IsDistanceGood)
            {
                Envenom.Launch();
                return;
            }

            if (ObjectManager.Me.EnergyPercentage > 35 && !Envenom.IsSpellUsable && Envenom.IsDistanceGood)
            {
                Eviscerate.Launch();
                return;
            }
        }

        if (ObjectManager.Me.ComboPoint >= 4 && !Envenom.KnownSpell && Eviscerate.KnownSpell)
        {
            if (Eviscerate.IsSpellUsable && Eviscerate.IsDistanceGood)
            {
                Eviscerate.Launch();
                return;
            }
        }

        if (Mutilate.KnownSpell && Mutilate.IsSpellUsable && Mutilate.IsDistanceGood)
            Mutilate.Launch();
    }

    public void Heal()
    {
        if (!Recuperate.HaveBuff && ObjectManager.Me.ComboPoint <= 3 && ObjectManager.Me.ComboPoint > 1 &&
            ObjectManager.Me.HealthPercent <= 50 && Recuperate.KnownSpell && Recuperate.IsSpellUsable &&
            Recuperate.IsDistanceGood)
        {
            Recuperate.Launch();
        }

        if (ObjectManager.Me.HealthPercent <= 10 && Vanish.KnownSpell && Vanish.IsSpellUsable && Vanish.IsDistanceGood)
        {
            Vanish.Launch();
            Thread.Sleep(5000);
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() >= 3 && Vanish.KnownSpell && Vanish.IsSpellUsable &&
            Vanish.IsDistanceGood)
        {
            Vanish.Launch();
            Thread.Sleep(5000);
            return;
        }
    }

    public void BuffCombat()
    {
        if (ObjectManager.Me.EnergyPercentage < 75 && Cold_Blood.KnownSpell && !Cold_Blood.HaveBuff &&
            Cold_Blood.IsSpellUsable)
            Cold_Blood.Launch();

        if (ObjectManager.Target.GetDistance < 5 && Adrenaline_Rush.KnownSpell && Adrenaline_Rush.IsSpellUsable)
            Adrenaline_Rush.Launch();

        if (ObjectManager.Target.GetDistance < 5 && Evasion.KnownSpell && Evasion.IsSpellUsable)
            Evasion.Launch();

        if (Combat_Readiness.KnownSpell && Combat_Readiness.IsSpellUsable)
            Combat_Readiness.Launch();

        if (Vendetta.KnownSpell && Vendetta.IsSpellUsable)
            Vendetta.Launch();
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && Kick.KnownSpell && Kick.IsSpellUsable && Kick.IsDistanceGood)
            Kick.Launch();

        if (ObjectManager.Target.IsCast && Cloak_of_Shadows.KnownSpell && Cloak_of_Shadows.IsSpellUsable)
            Cloak_of_Shadows.Launch();
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 1)
            Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "{DOWN}");
    }
}

public class Rogue
{
    #region InitializeSpell

    private Spell DeadlyPoison = new Spell(2823);
    private Spell CripplingPoison = new Spell(3408);
    private Spell MindNumbingPoison = new Spell(5761);
    private Spell WoundPoison = new Spell(8679);
    private Spell MasterPoisoner = new Spell(58410);
    private Spell Blade_Flurry = new Spell("Blade Flurry");
    private Spell Sinister_Strike = new Spell("Sinister Strike");
    private Spell Slice_and_Dice = new Spell("Slice and Dice");
    private Spell Premeditation = new Spell("Premeditation");
    private Spell Revealing_Strike = new Spell("Revealing Strike");
    private Spell Rupture = new Spell("Rupture");
    private Spell Eviscerate = new Spell("Eviscerate");
    private Spell Adrenaline_Rush = new Spell("Adrenaline Rush");
    private Spell Killing_Spree = new Spell("Killing Spree");
    private Spell Kick = new Spell("Kick");
    private Spell Evasion = new Spell("Evasion");
    private Spell Sprint = new Spell("Sprint");
    private Spell Combat_Readiness = new Spell("Combat Readiness");
    private Spell Cloak_of_Shadows = new Spell("Cloak of Shadows");
    private Spell Stealth = new Spell("Stealth");
    private Spell Sap = new Spell("Sap");
    private Spell Pick_Pocket = new Spell("Pick Pocket");
    private Spell Recuperate = new Spell("Recuperate");
    private Spell Vanish = new Spell("Vanish");
    private Spell Kidney_Shot = new Spell("Kidney Shot");
    private Spell Cheap_Shot = new Spell("Cheap Shot");

    #endregion InitializeSpell

    public Rogue()
    {
        Main.range = 5.0f;

        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            if (!ObjectManager.Me.IsMounted)
            {
                Patrolling();
                if (Fight.InFight && ObjectManager.Me.Target > 0)
                {
                    if (ObjectManager.Me.Target != lastTarget && Sap.IsDistanceGood)
                    {
                        Pull();
                        lastTarget = ObjectManager.Me.Target;
                    }

                    Combat();
                }
            }
            Thread.Sleep(350);
        }
    }

    public void Pull()
    {
        if (ObjectManager.Target.GetDistance <= 20 && !Stealth.HaveBuff && Stealth.KnownSpell && Stealth.IsSpellUsable)
        {
            Stealth.Launch();
            Thread.Sleep(1500);
        }
    }

    public void Combat()
    {
        AvoidMelee();
        Heal();
        BuffCombat();
        Decast();

        while (ObjectManager.Me.ComboPoint >= 4 && Eviscerate.KnownSpell)
        {
            if (Eviscerate.IsSpellUsable && Eviscerate.IsDistanceGood)
            {
                Eviscerate.Launch();
                return;
            }
        }

        if (Sinister_Strike.KnownSpell && Sinister_Strike.IsSpellUsable && Sinister_Strike.IsDistanceGood)
        {
            Sinister_Strike.Launch();
            return;
        }
    }

    public void Patrolling()
    {
        if (DeadlyPoison.KnownSpell)
        {
            if (!DeadlyPoison.HaveBuff && DeadlyPoison.IsSpellUsable)
            {
                DeadlyPoison.Launch();
                return;
            }
            else if (MasterPoisoner.KnownSpell && MasterPoisoner.IsSpellUsable && !MasterPoisoner.HaveBuff)
            {
                MasterPoisoner.Launch();
                return;
            }
            else if (WoundPoison.KnownSpell && WoundPoison.IsSpellUsable && !WoundPoison.HaveBuff &&
                     !MasterPoisoner.HaveBuff)
            {
                WoundPoison.Launch();
                return;
            }
            else if (MindNumbingPoison.KnownSpell && MindNumbingPoison.IsSpellUsable && !MindNumbingPoison.HaveBuff &&
                     !WoundPoison.HaveBuff && !MasterPoisoner.HaveBuff)
            {
                MindNumbingPoison.Launch();
                return;
            }
            else if (CripplingPoison.KnownSpell && CripplingPoison.IsSpellUsable && !CripplingPoison.HaveBuff &&
                     !MindNumbingPoison.HaveBuff && !WoundPoison.HaveBuff && !MasterPoisoner.HaveBuff)
            {
                CripplingPoison.Launch();
                return;
            }
        }
    }

    public void Heal()
    {
    }

    public void BuffCombat()
    {
        if (ObjectManager.Target.GetDistance < 5 && Evasion.KnownSpell && Evasion.IsSpellUsable)
            Evasion.Launch();
    }

    private void Decast()
    {
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 1)
            Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "{DOWN}");
    }
}

#endregion

#region Warrior

public class Arms
{
    #region InitializeSpell

    // ARMS ONLY
    private Spell Mortal_Strike = new Spell("Mortal Strike");
    private Spell Sweeping_Strikes = new Spell("Sweeping Strikes");
    private Spell Deadly_Calm = new Spell("Deadly Calm");
    private Spell Juggernaut = new Spell("Juggernaut");
    private Spell Throwdown = new Spell("Throwdown");
    private Spell Bladestorm = new Spell("Bladestorm");

    // DPS
    private Spell Strike = new Spell("Strike");
    private Spell Rend = new Spell("Rend");
    private Spell Victory_Rush = new Spell("Victory Rush");
    private Spell Heroic_Strike = new Spell("Heroic Strike");
    private Spell Overpower = new Spell("Overpower");
    private Spell Heroic_Throw = new Spell("Heroic Throw");
    private Spell Execute = new Spell("Execute");
    private Spell Cleave = new Spell("Cleave");
    private Spell Slam = new Spell("Slam");


    // BUFF & HELPING
    private Spell Battle_Stance = new Spell("Battle Stance");
    private Spell Defensive_Stance = new Spell("Defensive Stance");
    private Spell Berserker_Stance = new Spell("Berserker Stance");
    private Spell Battle_Shout = new Spell("Battle Shout");
    private Spell Demoralizing_Shout = new Spell("Demoralizing Shout");
    private Spell Commanding_Shout = new Spell("Commanding Shout");
    private Spell Thunder_Clap = new Spell("Thunder Clap");
    private Spell Charge = new Spell("Charge");
    private Spell Pummel = new Spell("Pummel");
    private Spell Berserker_Rage = new Spell("Berserker Rage");
    private Spell Inner_Rage = new Spell("Inner Rage");
    private Spell Colossus_Smash = new Spell("Colossus Smash");

    // TIMER
    private Timer look = new Timer(0);
    private Timer fighttimer = new Timer(0);
    private Timer rendchill = new Timer(0);


    // Profession & Racials
    private Spell ArcaneTorrent = new Spell("Arcane Torrent");
    private Spell Lifeblood = new Spell("Lifeblood");
    private Spell Stoneform = new Spell("Stoneform");
    private Spell Tailoring = new Spell("Tailoring");
    private Spell Leatherworking = new Spell("Leatherworking");
    private Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private Spell War_Stomp = new Spell("War Stomp");
    private Spell Berserking = new Spell("Berserking");

    #endregion InitializeSpell

    public Arms()
    {
        Main.range = 5.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            if (!ObjectManager.Me.IsMounted)
            {
                buffoutfight();

                if (!Fight.InFight && look.IsReady)
                {
                    look = new Timer(5000);
                    Lua.RunMacroText("/targetfriendplayer");
                }

                if (Fight.InFight && ObjectManager.Me.Target > 0 && ObjectManager.Target.GetDistance > Main.range)
                {
                    fighttimer = new Timer(60000);
                }

                if (Fight.InFight && ObjectManager.Me.Target > 0)
                {
                    if (ObjectManager.Me.Target != lastTarget && ObjectManager.Target.GetDistance <= Main.range)
                    {
                        pull();
                        lastTarget = ObjectManager.Me.Target;
                    }
                    fight();
                    if (!Fight.InFight)
                    {
                        Logging.WriteFight(" - Target Down - ");
                        look = new Timer(5000);
                    }

                    if (fighttimer.IsReady && ObjectManager.Target.HealthPercent > 90 && ObjectManager.Me.Target > 0 &&
                        ObjectManager.GetNumberAttackPlayer() < 2)
                    {
                        Logging.WriteFight(" - Target Evading - ");
                        break;
                    }
                }
            }
            Thread.Sleep(350);
        }
    }

    public void pull()
    {
        if (hardmob()) Logging.WriteFight(" -  Pull Hard Mob - ");
        if (!hardmob()) Logging.WriteFight(" -  Pull Easy Mob - ");
        fighttimer = new Timer(60000);
        if (Charge.KnownSpell && Charge.IsSpellUsable && Charge.IsDistanceGood)
        {
            Charge.Launch();
        }
    }

    public void buffoutfight()
    {
        if (Fight.InFight || ObjectManager.Me.IsDeadMe) return;

        if (!ObjectManager.Me.HaveBuff(79640) &&
            ItemsManager.GetItemCountByIdLUA(58149) == 1)
        {
            Logging.WriteFight("Use Alchi Flask");
            Lua.RunMacroText("/use item:58149");
        }

        if (Battle_Shout.KnownSpell && Battle_Shout.IsSpellUsable && !Battle_Shout.HaveBuff)
        {
            Battle_Shout.Launch();
        }
    }

    public void fight()
    {
        selfheal();
        buffinfight();
        if (ObjectManager.GetNumberAttackPlayer() > 1) fighttimer = new Timer(60000);

        if (Berserking.KnownSpell && Berserking.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(1454);
            // Berserking.Launch();
        }

        if (Berserker_Rage.KnownSpell && Berserker_Rage.IsSpellUsable && ObjectManager.Me.RagePercentage < 50)
        {
            Berserker_Rage.Launch();
        }

        if (Charge.KnownSpell && Charge.IsSpellUsable && Charge.IsDistanceGood)
        {
            Charge.Launch();
        }

        if (Execute.KnownSpell && Execute.IsSpellUsable && Execute.IsDistanceGood)
        {
            Execute.Launch();
        }

        if (Rend.KnownSpell && Rend.IsSpellUsable && Rend.IsDistanceGood && !Rend.TargetHaveBuff && rendchill.IsReady)
        {
            rendchill = new Timer(2500);

            Rend.Launch();
        }

        if (Sweeping_Strikes.KnownSpell && Sweeping_Strikes.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 1)
        {
            Sweeping_Strikes.Launch();
        }

        if (Colossus_Smash.KnownSpell && Colossus_Smash.IsSpellUsable && Colossus_Smash.IsDistanceGood)
        {
            Colossus_Smash.Launch();
        }

        if (Heroic_Throw.KnownSpell && Heroic_Throw.IsSpellUsable && Heroic_Throw.IsDistanceGood)
        {
            Heroic_Throw.Launch();
        }

        if (Mortal_Strike.KnownSpell && Mortal_Strike.IsSpellUsable && Mortal_Strike.IsDistanceGood &&
            (ObjectManager.GetNumberAttackPlayer() < 2 ||
             (ObjectManager.GetNumberAttackPlayer() > 1 && !Cleave.KnownSpell)))
        {
            Mortal_Strike.Launch();
        }

        if (Overpower.KnownSpell && Overpower.IsSpellUsable && Overpower.IsDistanceGood &&
            (ObjectManager.GetNumberAttackPlayer() < 2 ||
             (ObjectManager.GetNumberAttackPlayer() > 1 && !Cleave.KnownSpell)))
        {
            Overpower.Launch();
        }

        if (!Mortal_Strike.KnownSpell && Strike.IsSpellUsable && Strike.IsDistanceGood &&
            (ObjectManager.GetNumberAttackPlayer() < 2 ||
             (ObjectManager.GetNumberAttackPlayer() > 1 && !Cleave.KnownSpell)))
        {
            Strike.Launch();
        }

        if (Cleave.KnownSpell && Cleave.IsSpellUsable && Cleave.IsDistanceGood &&
            ObjectManager.GetNumberAttackPlayer() > 1)
        {
            Cleave.Launch();
        }

        if (Heroic_Strike.KnownSpell && Heroic_Strike.IsSpellUsable && Heroic_Strike.IsDistanceGood &&
            ObjectManager.Me.RagePercentage > 70)
        {
            Heroic_Strike.Launch();
        }


        if (Pummel.KnownSpell && Pummel.IsSpellUsable && Pummel.IsDistanceGood &&
            ObjectManager.Target.IsCast)
        {
            Pummel.Launch();
        }

        if (Slam.KnownSpell && Slam.IsSpellUsable && Slam.IsDistanceGood && !Overpower.IsSpellUsable &&
            !Mortal_Strike.IsSpellUsable &&
            (ObjectManager.GetNumberAttackPlayer() < 2 ||
             (ObjectManager.GetNumberAttackPlayer() > 1 && !Cleave.KnownSpell)))
        {
            Slam.Launch();
        }

        if (ArcaneTorrent.KnownSpell && ArcaneTorrent.IsSpellUsable &&
            ObjectManager.Target.IsCast && ObjectManager.Target.GetDistance < 8)
        {
            ArcaneTorrent.Launch();
        }
    }

    private void buffinfight()
    {
        if (!Fight.InFight) return;

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            Stoneform.KnownSpell && Stoneform.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(20594);
            // Stoneform.Launch();
        }

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            War_Stomp.KnownSpell && War_Stomp.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(20549);
            // War_Stomp.Launch();
        }

        if (!Battle_Stance.HaveBuff && Battle_Stance.IsSpellUsable)
        {
            Battle_Stance.Launch();
        }

        if (Thunder_Clap.KnownSpell && Thunder_Clap.IsSpellUsable && !Thunder_Clap.TargetHaveBuff &&
            !Strike.IsSpellUsable && ObjectManager.Target.GetDistance < 9 &&
            (ObjectManager.Me.RagePercentage > 50 || hardmob() || ObjectManager.GetNumberAttackPlayer() > 1))
        {
            Thunder_Clap.Launch();
        }

        if (Demoralizing_Shout.KnownSpell && Demoralizing_Shout.IsSpellUsable && !Demoralizing_Shout.TargetHaveBuff &&
            !Strike.IsSpellUsable && ObjectManager.Target.GetDistance < 9 &&
            (ObjectManager.Me.RagePercentage > 50 || hardmob() || ObjectManager.GetNumberAttackPlayer() > 1))
        {
            Demoralizing_Shout.Launch();
        }
    }

    private void selfheal()
    {
        if (ObjectManager.Me.HealthPercent < 80 &&
            Lifeblood.KnownSpell && Lifeblood.IsSpellUsable)
        {
            Lifeblood.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 80 &&
            Gift_of_the_Naaru.KnownSpell && Gift_of_the_Naaru.IsSpellUsable)
        {
            Gift_of_the_Naaru.Launch();
        }

        if (Victory_Rush.KnownSpell && Victory_Rush.IsSpellUsable && Victory_Rush.IsDistanceGood)
        {
            Victory_Rush.Launch();
        }
    }

    public bool hardmob()
    {
        if (((ObjectManager.Target.MaxHealth*100)/ObjectManager.Me.MaxHealth) > 100)
        {
            return true;
        }
        return false;
    }
}

public class WarriorProt
{
    #region InitializeSpell

    private Spell Berserker_Stance = new Spell("Berserker Stance");
    private Spell Enraged_Regeneration = new Spell("Enraged Regeneration");
    private Spell Battle_Shout = new Spell("Battle Shout");
    private Spell Colossus_Smash = new Spell("Colossus Smash");
    private Spell Bloodthirst = new Spell("Bloodthirst");
    private Spell Raging_Blow = new Spell("Raging Blow");
    private Spell Slam = new Spell("Slam");
    private Spell Death_Wish = new Spell("Death Wish");
    private Spell Recklessness = new Spell("Recklessness");
    private Spell Execute = new Spell("Execute");
    private Spell Heroic_Strike = new Spell("Heroic Strike");
    private Spell Intercept = new Spell("Intercept");
    private Spell Enrage = new Spell("Enrage");
    private Spell Bloodsurge = new Spell("Bloodsurge");
    private Spell Heroic_Throw = new Spell("Heroic Throw");
    private Spell Heroic_Leap = new Spell("Heroic Leap");
    private Spell Cleave = new Spell("Cleave");
    private Spell Whirlwind = new Spell("Whirlwind");
    private Spell Victory_Rush = new Spell("Victory Rush");
    private Spell Hamstring = new Spell("Hamstring");
    private Spell Pummel = new Spell("Pummel");
    private Spell Shield_Slam = new Spell("Shield Slam");
    private Spell Concussion_Blow = new Spell("Concussion Blow");
    private Timer Rend_Timer = new Timer(0);
    private Timer Recklessness_Timer = new Timer(0);
    private Timer Enraged_Regeneration_Timer = new Timer(0);
    private Spell Rend = new Spell("Rend");
    private Spell Shockwave = new Spell("Shockwave");
    private Spell Devastate = new Spell("Devastate");
    private Spell Shield_Bash = new Spell("Shield Bash");
    private Spell Last_Stand = new Spell("Last Stand");
    private Spell Shield_Block = new Spell("Shield Block");
    private Spell Shield_Wall = new Spell("Shield Wall");
    private Spell Spell_Reflection = new Spell("Spell Reflection");
    private Spell Charge = new Spell("Charge");
    private Spell Revenge = new Spell("Revenge");
    private Spell Thunder_Clap = new Spell("Thunder Clap");
    private Spell Defensive_Stance = new Spell("Defensive Stance");
    private Spell Sunder_Armor = new Spell("Sunder Armor");
    private Spell Strike = new Spell("Strike");
    private Spell Battle_Stance = new Spell("Battle Stance");
    private Spell Intimidating_Shout = new Spell("Intimidating_Shout");
    private Spell Piercing_Howl = new Spell("Piercing_Howl");
    private Spell Berserker_Rage = new Spell("Berserker_Rage");

    #endregion InitializeSpell

    public WarriorProt()
    {
        Main.range = 5.0f;

        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            if (!ObjectManager.Me.IsMounted)
            {
                Patrolling();

                if (Fight.InFight && ObjectManager.Me.Target > 0)
                {
                    if (ObjectManager.Me.Target != lastTarget && Intercept.IsDistanceGood)
                    {
                        Pull();
                        lastTarget = ObjectManager.Me.Target;
                    }

                    Combat();
                }
            }
            Thread.Sleep(350);
        }
    }

    public void Pull()
    {
        if (!Defensive_Stance.HaveBuff)
            Defensive_Stance.Launch();

        if (Heroic_Throw.KnownSpell &&
            ObjectManager.Target.GetDistance < 30 &&
            Heroic_Throw.IsSpellUsable &&
            Heroic_Throw.IsDistanceGood)
        {
            Heroic_Throw.Launch();
        }
    }

    public void Combat()
    {
        Charges();
        Def();
        AvoidMelee();
        Heal();
        BuffCombat();
        Decast();
        Fear();
        Burst();

        if ((ObjectManager.Me.HaveBuff(46951) ||
             ObjectManager.Me.HaveBuff(46952) ||
             ObjectManager.Me.HaveBuff(46953)) &&
            Shield_Slam.KnownSpell && Shield_Slam.IsSpellUsable && Shield_Slam.IsDistanceGood)
        {
            Shield_Slam.Launch();
            return;
        }

        if (Rend_Timer.IsReady && Rend.KnownSpell && Rend.IsDistanceGood && Rend.IsSpellUsable)
        {
            Rend.Launch();
            Rend_Timer = new Timer(1000*10);
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() >= 2 &&
            Cleave.KnownSpell &&
            Cleave.IsSpellUsable &&
            Cleave.IsDistanceGood)
        {
            Cleave.Launch();
            return;
        }

        if (Victory_Rush.KnownSpell && Victory_Rush.IsSpellUsable && Victory_Rush.IsDistanceGood)
        {
            Victory_Rush.Launch();
            return;
        }

        if (Shield_Slam.KnownSpell && Shield_Slam.IsSpellUsable && Shield_Slam.IsDistanceGood)
        {
            Shield_Slam.Launch();
            return;
        }

        if (Revenge.KnownSpell && Revenge.IsDistanceGood && Revenge.IsSpellUsable)
        {
            Revenge.Launch();
            return;
        }

        if (!Recklessness_Timer.IsReady &&
            Concussion_Blow.KnownSpell && Concussion_Blow.IsSpellUsable && Concussion_Blow.IsDistanceGood)
        {
            Concussion_Blow.Launch();
            return;
        }

        if (Shockwave.KnownSpell && Shockwave.IsSpellUsable && Shockwave.IsDistanceGood)
        {
            Shockwave.Launch();
            return;
        }

        if (Devastate.KnownSpell && Devastate.IsSpellUsable && Devastate.IsDistanceGood)
        {
            if (Sunder_Armor.TargetHaveBuff)
            {
                Heroic_Strike.Launch();
                return;
            }

            Devastate.Launch();
            return;
        }
    }

    public void Patrolling()
    {
        if (!Defensive_Stance.HaveBuff)
            Defensive_Stance.Launch();
    }

    public void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 75 &&
            Berserker_Rage.KnownSpell && Enraged_Regeneration_Timer.IsReady &&
            Berserker_Rage.IsSpellUsable && Enraged_Regeneration.KnownSpell)
        {
            Berserker_Rage.Launch();
            Thread.Sleep(200);

            if (Berserker_Rage.HaveBuff)
            {
                int i = 0;
                while (i < 3)
                {
                    i++;
                    Enraged_Regeneration.Launch();
                    Enraged_Regeneration_Timer = new Timer(1000*60*3);

                    if (Enraged_Regeneration.HaveBuff)
                    {
                        break;
                    }
                }
            }
        }
    }

    public void Burst()
    {
        if (Rend.TargetHaveBuff && Shield_Block.HaveBuff && Concussion_Blow.KnownSpell &&
            Concussion_Blow.IsSpellUsable && Colossus_Smash.KnownSpell &&
            ObjectManager.Me.HealthPercent > 50 &&
            Recklessness.KnownSpell && Recklessness_Timer.IsReady)
        {
            if (!Berserker_Stance.HaveBuff)
            {
                int i = 0;
                while (i < 3)
                {
                    i++;
                    Berserker_Stance.Launch();

                    if (Berserker_Stance.HaveBuff)
                    {
                        break;
                    }
                }
            }

            if (Berserker_Stance.HaveBuff &&
                Concussion_Blow.KnownSpell &&
                Concussion_Blow.IsSpellUsable &&
                Concussion_Blow.IsDistanceGood)
            {
                int l = 0;
                while (l < 0)
                {
                    l++;
                    Concussion_Blow.Launch();

                    if (!Concussion_Blow.IsSpellUsable)
                    {
                        break;
                    }
                }
            }

            if (Berserker_Stance.HaveBuff)
            {
                if (!Recklessness.HaveBuff)
                {
                    int j = 0;
                    while (j < 3)
                    {
                        j++;
                        Recklessness.Launch();
                        Lua.RunMacroText("/use 13");
                        Lua.RunMacroText("/script UIErrorsFrame:Clear()");
                        Lua.RunMacroText("/use 14");
                        Lua.RunMacroText("/script UIErrorsFrame:Clear()");
                        Recklessness_Timer = new Timer(1000*60*5);

                        if (!Recklessness.IsSpellUsable)
                        {
                            break;
                        }
                    }
                }
            }

            if (Berserker_Stance.HaveBuff &&
                Colossus_Smash.KnownSpell && Colossus_Smash.IsSpellUsable && Colossus_Smash.IsDistanceGood)
            {
                int m = 0;
                while (m < 0)
                {
                    m++;
                    Colossus_Smash.Launch();

                    if (!Colossus_Smash.IsSpellUsable)
                    {
                        break;
                    }
                }
            }

            if (!Defensive_Stance.HaveBuff)
            {
                int k = 0;
                while (k < 0)
                {
                    k++;
                    Defensive_Stance.Launch();

                    if (Defensive_Stance.HaveBuff)
                    {
                        break;
                    }
                }
            }
        }
    }

    public void BuffCombat()
    {
        if (!Defensive_Stance.HaveBuff)
            Defensive_Stance.Launch();

        if (Shield_Block.KnownSpell &&
            Shield_Block.IsSpellUsable)
        {
            Shield_Block.Launch();
        }

        if (Battle_Shout.KnownSpell &&
            Battle_Shout.IsSpellUsable &&
            ObjectManager.Me.RagePercentage < 70)
        {
            Battle_Shout.Launch();
        }
    }

    private void Def()
    {
        if (ObjectManager.Me.HealthPercent < 60 &&
            Shield_Wall.KnownSpell && Shield_Wall.IsSpellUsable)
        {
            Shield_Wall.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 60 &&
            Last_Stand.KnownSpell && Last_Stand.IsSpellUsable)
        {
            Last_Stand.Launch();
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast &&
            Spell_Reflection.KnownSpell &&
            Spell_Reflection.IsSpellUsable &&
            Spell_Reflection.IsDistanceGood)
        {
            Spell_Reflection.Launch();
        }

        if (ObjectManager.Target.IsCast &&
            Shield_Bash.KnownSpell &&
            Shield_Bash.IsSpellUsable &&
            Shield_Bash.IsDistanceGood)
        {
            Shield_Bash.Launch();
        }
    }

    private void Fear()
    {
        if (ObjectManager.Target.GetMove &&
            ObjectManager.Target.GetDistance <= 8 &&
            !Piercing_Howl.TargetHaveBuff &&
            Intimidating_Shout.KnownSpell &&
            Intimidating_Shout.IsSpellUsable)
        {
            Intimidating_Shout.Launch();
        }
    }

    private void Charges()
    {
        if (Intercept.KnownSpell && Intercept.IsSpellUsable && Intercept.IsDistanceGood)
        {
            Intercept.Launch();
        }

        if (Heroic_Leap.KnownSpell && Heroic_Leap.IsSpellUsable && Heroic_Leap.IsDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(6544, ObjectManager.Target.Position);
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 1)
        {
            Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "{DOWN}");
        }
    }
}

public class WarriorFury
{
    #region InitializeSpell

    private Spell Berserker_Stance = new Spell("Berserker Stance");
    private Spell Battle_Stance = new Spell("Battle Stance");

    private Spell Inner_Rage = new Spell("Inner Rage");
    private Spell Enraged_Regeneration = new Spell("Enraged Regeneration");
    private Spell Battle_Shout = new Spell("Battle Shout");
    private Spell Colossus_Smash = new Spell("Colossus Smash");
    private Spell Bloodthirst = new Spell("Bloodthirst");
    private Spell Raging_Blow = new Spell("Raging Blow");
    private Spell Slam = new Spell("Slam");
    private Spell Death_Wish = new Spell("Death Wish");
    private Spell Recklessness = new Spell("Recklessness");
    private Spell Execute = new Spell("Execute");
    private Spell Heroic_Strike = new Spell("Heroic Strike");
    private Spell Intercept = new Spell("Intercept");
    private Spell Enrage = new Spell("Enrage");
    private Spell Bloodsurge = new Spell("Bloodsurge");
    private Spell Heroic_Throw = new Spell("Heroic Throw");
    private Spell Heroic_Leap = new Spell("Heroic Leap");
    private Spell Cleave = new Spell("Cleave");
    private Spell Whirlwind = new Spell("Whirlwind");
    private Spell Victory_Rush = new Spell("Victory Rush");
    private Spell Hamstring = new Spell("Hamstring");
    private Spell Pummel = new Spell("Pummel");
    private Spell Rend = new Spell("Rend");
    private Spell Mortal_Strike = new Spell("Mortal Strike");
    private Spell Overpower = new Spell("Overpower");
    private Spell Deadly_Calm = new Spell("Deadly Calm");
    private Spell Bladestorm = new Spell("Bladestorm");
    private Spell Sweeping_Strikes = new Spell("Sweeping Strikes");
    private Spell Thunder_Clap = new Spell("Thunder Clap");
    private Spell Throwdown = new Spell("Throwdown");
    private Spell Charge = new Spell("Charge");
    private Spell Strike = new Spell("Strike");
    private Spell Intimidating_Shout = new Spell("Intimidating Shout");
    private Spell Commanding_Shout = new Spell("Commanding Shout");
    private Spell Piercing_Howl = new Spell("Piercing Howl");
    private Spell Taste_for_Blood = new Spell("Taste for Blood");
    private Spell Berserker_Rage = new Spell("Berserker Rage");
    private Spell Victorious = new Spell("Victorious");
    private Spell Retaliation = new Spell("Retaliation");

    private Timer Rend_Timer = new Timer(0);
    private Timer Charge_Timer = new Timer(0);
    private Timer Recklessness_Timer = new Timer(0);
    private Timer Enraged_Regeneration_Timer = new Timer(0);
    private Timer Death_Wish_Timer = new Timer(0);
    private Timer Inner_Rage_Timer = new Timer(0);
    private Timer Intercept_leap_Timer = new Timer(0);

    #endregion InitializeSpell

    public WarriorFury()
    {
        Main.range = 5.0f;

        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            if (!ObjectManager.Me.IsMounted)
            {
                Patrolling();

                if (Fight.InFight && ObjectManager.Me.Target > 0)
                {
                    if (ObjectManager.Me.Target != lastTarget && Intercept.IsDistanceGood)
                    {
                        Pull();
                        lastTarget = ObjectManager.Me.Target;
                    }

                    Combat();
                }
            }
            Thread.Sleep(350);
        }
    }

    public void Pull()
    {
        if (!Berserker_Stance.HaveBuff)
            Berserker_Stance.Launch();

        Charges();
    }

    public void Combat()
    {
        Charges();
        AvoidMelee();
        Heal();
        BuffCombat();
        TargetMoving();
        Decast();
        Fear();

        if (Victory_Rush.KnownSpell &&
            Victory_Rush.IsSpellUsable &&
            Victory_Rush.IsDistanceGood)
        {
            Victory_Rush.Launch();

            if ((ObjectManager.Me.RagePercentage > 70 || Inner_Rage.HaveBuff) &&
                Heroic_Strike.KnownSpell &&
                Heroic_Strike.IsSpellUsable)
            {
                Heroic_Strike.Launch();
                return;
            }
        }

        if (Colossus_Smash.KnownSpell &&
            Colossus_Smash.IsSpellUsable &&
            Colossus_Smash.IsDistanceGood)
        {
            Colossus_Smash.Launch();

            if ((ObjectManager.Me.RagePercentage > 70 || Inner_Rage.HaveBuff) &&
                Heroic_Strike.KnownSpell &&
                Heroic_Strike.IsSpellUsable)
            {
                Heroic_Strike.Launch();
                return;
            }
        }

        if (Bloodthirst.KnownSpell &&
            Bloodthirst.IsDistanceGood &&
            Bloodthirst.IsSpellUsable)
        {
            Bloodthirst.Launch();

            if ((ObjectManager.Me.RagePercentage > 70 || Inner_Rage.HaveBuff) &&
                Heroic_Strike.KnownSpell &&
                Heroic_Strike.IsSpellUsable)
            {
                Heroic_Strike.Launch();
                return;
            }
        }

        if (Raging_Blow.KnownSpell &&
            Raging_Blow.IsSpellUsable &&
            Raging_Blow.IsDistanceGood)
        {
            Raging_Blow.Launch();

            if ((ObjectManager.Me.RagePercentage > 70 || Inner_Rage.HaveBuff) &&
                Heroic_Strike.KnownSpell &&
                Heroic_Strike.IsSpellUsable)
            {
                Heroic_Strike.Launch();
                return;
            }
        }

        if (ObjectManager.Me.HaveBuff(46916) &&
            Slam.KnownSpell &&
            Slam.IsDistanceGood)
        {
            Slam.Launch();

            if ((ObjectManager.Me.RagePercentage > 70 || Inner_Rage.HaveBuff) &&
                Heroic_Strike.KnownSpell &&
                Heroic_Strike.IsSpellUsable)
            {
                Heroic_Strike.Launch();
                return;
            }
        }

        if ((ObjectManager.Me.RagePercentage > 60 || Inner_Rage.HaveBuff) &&
            Heroic_Strike.KnownSpell &&
            Heroic_Strike.IsSpellUsable)
        {
            Heroic_Strike.Launch();
            return;
        }

        if (Heroic_Throw.KnownSpell &&
            Heroic_Throw.IsSpellUsable &&
            Heroic_Throw.IsDistanceGood)
        {
            Heroic_Throw.Launch();
        }
    }

    public void Patrolling()
    {
        if (!Berserker_Stance.HaveBuff)
            Berserker_Stance.Launch();
    }

    private void Charges()
    {
        if (Intercept.KnownSpell && Intercept.IsSpellUsable && Intercept.IsDistanceGood && Intercept_leap_Timer.IsReady)
        {
            Intercept.Launch();
            Intercept_leap_Timer = new Timer(1000*3);
        }

        if (Heroic_Leap.KnownSpell && Heroic_Leap.IsSpellUsable && Heroic_Leap.IsDistanceGood &&
            Intercept_leap_Timer.IsReady)
        {
            SpellManager.CastSpellByIDAndPosition(6544, ObjectManager.Target.Position);
            Intercept_leap_Timer = new Timer(1000*3);
        }
    }

    public void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 75 &&
            Berserker_Rage.KnownSpell && Enraged_Regeneration_Timer.IsReady &&
            Berserker_Rage.IsSpellUsable && Enraged_Regeneration.KnownSpell)
        {
            Berserker_Rage.Launch();
            Thread.Sleep(200);

            if (Berserker_Rage.HaveBuff)
            {
                int i = 0;
                while (i < 3)
                {
                    i++;
                    Enraged_Regeneration.Launch();
                    Enraged_Regeneration_Timer = new Timer(1000*60*3);

                    if (Enraged_Regeneration.HaveBuff)
                    {
                        break;
                    }
                }
            }
        }
    }

    public void BuffCombat()
    {
        if (!Berserker_Stance.HaveBuff)
            Berserker_Stance.Launch();

        if (!Berserker_Stance.KnownSpell)
        {
            Battle_Stance.Launch();
        }

        if (Battle_Shout.KnownSpell &&
            Battle_Shout.IsSpellUsable &&
            ObjectManager.Me.RagePercentage < 70)
        {
            Battle_Shout.Launch();
        }

        if (Inner_Rage_Timer.IsReady &&
            Inner_Rage.KnownSpell &&
            Inner_Rage.IsSpellUsable)
        {
            Inner_Rage.Launch();
            Inner_Rage_Timer = new Timer(1000*40);
        }

        if (Recklessness.KnownSpell || Death_Wish.KnownSpell)
        {
            if (Recklessness.KnownSpell && Death_Wish.KnownSpell &&
                Recklessness.IsSpellUsable && Recklessness_Timer.IsReady &&
                Death_Wish.IsSpellUsable && Death_Wish_Timer.IsReady)
            {
                int j = 0;
                while (j < 3)
                {
                    j++;
                    Recklessness.Launch();
                    Recklessness_Timer = new Timer(1000*300);

                    Death_Wish.Launch();
                    Death_Wish_Timer = new Timer(1000*150);

                    Lua.RunMacroText("/use 13");
                    Lua.RunMacroText("/script UIErrorsFrame:Clear()");
                    Lua.RunMacroText("/use 14");
                    Lua.RunMacroText("/script UIErrorsFrame:Clear()");

                    if (!Recklessness.IsSpellUsable &&
                        !Death_Wish.IsSpellUsable)
                    {
                        break;
                    }
                }
            }

            else if (Death_Wish.KnownSpell && Death_Wish.IsSpellUsable && Death_Wish_Timer.IsReady)
            {
                Death_Wish.Launch();
                Lua.RunMacroText("/use 13");
                Lua.RunMacroText("/script UIErrorsFrame:Clear()");
                Lua.RunMacroText("/use 14");
                Lua.RunMacroText("/script UIErrorsFrame:Clear()");
                Death_Wish_Timer = new Timer(1000*150);
            }

            else if (!Death_Wish.KnownSpell && Recklessness.IsSpellUsable && Recklessness_Timer.IsReady)
            {
                Recklessness.Launch();
                Lua.RunMacroText("/use 13");
                Lua.RunMacroText("/script UIErrorsFrame:Clear()");
                Lua.RunMacroText("/use 14");
                Lua.RunMacroText("/script UIErrorsFrame:Clear()");
                Recklessness_Timer = new Timer(1000*300);
            }
        }
    }

    private void TargetMoving()
    {
        if (ObjectManager.Target.GetMove &&
            !Hamstring.TargetHaveBuff &&
            Hamstring.KnownSpell &&
            Hamstring.IsDistanceGood &&
            Hamstring.IsSpellUsable)
        {
            Hamstring.Launch();
        }

        if (ObjectManager.Target.GetDistance <= 15 &&
            (ObjectManager.GetNumberAttackPlayer() >= 2 ||
             ObjectManager.Target.GetMove) &&
            !Piercing_Howl.TargetHaveBuff &&
            !Intimidating_Shout.TargetHaveBuff &&
            Piercing_Howl.KnownSpell &&
            Piercing_Howl.IsDistanceGood &&
            Piercing_Howl.IsSpellUsable)
        {
            Piercing_Howl.Launch();
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast &&
            Pummel.KnownSpell &&
            Pummel.IsSpellUsable &&
            Pummel.IsDistanceGood)
        {
            Pummel.Launch();
        }
    }

    private void Fear()
    {
        if (ObjectManager.Target.GetMove &&
            ObjectManager.Target.GetDistance <= 8 &&
            !Throwdown.TargetHaveBuff &&
            !Piercing_Howl.TargetHaveBuff &&
            Intimidating_Shout.KnownSpell &&
            Intimidating_Shout.IsSpellUsable)
        {
            Intimidating_Shout.Launch();
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 1)
        {
            Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "{DOWN}");
        }
    }
}

public class Warrior
{
    #region InitializeSpell

    private Spell Berserker_Stance = new Spell("Berserker Stance");
    private Spell Battle_Stance = new Spell("Battle Stance");

    private Spell Enraged_Regeneration = new Spell("Enraged Regeneration");
    private Spell Battle_Shout = new Spell("Battle Shout");
    private Spell Colossus_Smash = new Spell("Colossus Smash");
    private Spell Bloodthirst = new Spell("Bloodthirst");
    private Spell Raging_Blow = new Spell("Raging Blow");
    private Spell Slam = new Spell("Slam");
    private Spell Death_Wish = new Spell("Death Wish");
    private Spell Recklessness = new Spell("Recklessness");
    private Spell Execute = new Spell("Execute");
    private Spell Heroic_Strike = new Spell("Heroic Strike");
    private Spell Intercept = new Spell("Intercept");
    private Spell Enrage = new Spell("Enrage");
    private Spell Bloodsurge = new Spell("Bloodsurge");
    private Spell Heroic_Throw = new Spell("Heroic Throw");
    private Spell Heroic_Leap = new Spell("Heroic Leap");
    private Spell Cleave = new Spell("Cleave");
    private Spell Whirlwind = new Spell("Whirlwind");
    private Spell Victory_Rush = new Spell("Victory Rush");
    private Spell Hamstring = new Spell("Hamstring");
    private Spell Pummel = new Spell("Pummel");
    private Spell Rend = new Spell("Rend");
    private Spell Mortal_Strike = new Spell("Mortal Strike");
    private Spell Overpower = new Spell("Overpower");
    private Spell Deadly_Calm = new Spell("Deadly Calm");
    private Spell Bladestorm = new Spell("Bladestorm");
    private Spell Sweeping_Strikes = new Spell("Sweeping Strikes");
    private Spell Thunder_Clap = new Spell("Thunder Clap");
    private Spell Throwdown = new Spell("Throwdown");
    private Spell Charge = new Spell("Charge");
    private Spell Strike = new Spell("Strike");
    private Timer Rend_Timer = new Timer(0);

    #endregion InitializeSpell

    public Warrior()
    {
        Main.range = 5.0f;

        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            if (!ObjectManager.Me.IsMounted)
            {
                Patrolling();

                if (Fight.InFight && ObjectManager.Me.Target > 0)
                {
                    if (ObjectManager.Me.Target != lastTarget && Charge.IsDistanceGood)
                    {
                        Pull();
                        lastTarget = ObjectManager.Me.Target;
                    }

                    Combat();
                }
            }
            Thread.Sleep(350);
        }
    }

    public void Pull()
    {
        if (!Battle_Stance.HaveBuff)
            Battle_Stance.Launch();

        if (Charge.KnownSpell &&
            ObjectManager.Target.GetDistance > 8 &&
            Charge.IsSpellUsable &&
            Charge.IsDistanceGood)
        {
            Charge.Launch();
        }
    }

    public void Combat()
    {
        AvoidMelee();

        if (Strike.KnownSpell &&
            Strike.IsSpellUsable &&
            Strike.IsDistanceGood)
        {
            Strike.Launch();
            return;
        }

        if (Victory_Rush.KnownSpell &&
            Victory_Rush.IsSpellUsable &&
            Victory_Rush.IsDistanceGood)
        {
            Victory_Rush.Launch();
            return;
        }

        if (Rend_Timer.IsReady &&
            Rend.KnownSpell &&
            Rend.IsDistanceGood &&
            Rend.IsSpellUsable)
        {
            Rend.Launch();
            Rend_Timer = new Timer(1000*10);
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 3 &&
            Thunder_Clap.KnownSpell &&
            Thunder_Clap.IsSpellUsable &&
            Thunder_Clap.IsDistanceGood)
        {
            Thunder_Clap.Launch();
            return;
        }
    }

    private void AvoidMelee()
    {
        if (ObjectManager.Target.GetDistance < 1)
        {
            Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "{DOWN}");
        }
    }

    public void Patrolling()
    {
    }
}

#endregion

#region Hunter

public class Survival
{
    #region InitializeSpell

    // Survival Only
    private Spell Explosive_Shot = new Spell("Explosive Shot");
    private Spell Counterattack = new Spell("Counterattack");
    private Spell Black_Arrow = new Spell("Black Arrow");

    // DPS
    private Spell Raptor_Strike = new Spell("Raptor Strike");
    private Spell Arcane_Shot = new Spell("Arcane Shot");
    private Spell Steady_Shot = new Spell("Steady Shot");
    private Spell Serpent_Sting = new Spell("Serpent Sting");
    private Spell Multi_Shot = new Spell("Multi-Shot");
    private Spell Kill_Shot = new Spell("Kill Shot");
    private Spell Explosive_Trap = new Spell("Explosive Trap");
    private Spell Cobra_Shot = new Spell("Cobra Shot");
    private Spell Immolation_Trap = new Spell("Immolation Trap");

    // BUFF & HELPING
    private Spell Concussive_Shot = new Spell("Concussive Shot");
    private Spell Aspect_of_the_Hawk = new Spell(13165);
    private Spell Aspect_of_the_Iron_Hawk = new Spell(109260);
    private Spell Disengage = new Spell("Disengage");
    private Spell Hunters_Mark = new Spell("Hunter's Mark");
    private Spell Scatter_Shot = new Spell("Scatter Shot"); // 19503
    private Spell Feign_Death = new Spell("Feign Death"); //	5384
    private Spell Snake_Trap = new Spell("Snake Trap");
    private Spell Ice_Trap = new Spell("Ice Trap");
    private Spell Freezing_Trap = new Spell("Freezing Trap");
    private Spell Trap_Launcher = new Spell("Trap Launcher"); //	77769
    private Spell Rapid_Fire = new Spell("Rapid Fire"); //	3045
    private Spell Misdirection = new Spell("Misdirection");
    private Spell Deterrence = new Spell("Deterrence"); //	19263
    private Spell Wing_Clip = new Spell("Wing Clip");

    // PET
    private Spell Kill_Command = new Spell("Kill Command");
    private Spell Mend_Pet = new Spell("Mend Pet"); //	136
    private Spell Revive_Pet = new Spell("Revive Pet"); //	982
    private Spell Call_Pet = new Spell("Call Pet 1"); //	883

    // TIMER
    private Timer look = new Timer(0);
    private Timer fighttimer = new Timer(0);
    private Timer petheal = new Timer(0);
    private Timer traplaunchertimer = new Timer(0);
    private Timer disengagetimer = new Timer(0);
    private Timer Serpent_Sting_debuff = new Timer(0);
    private Timer mountchill = new Timer(0);

    // Profession & Racials
    private Spell ArcaneTorrent = new Spell("Arcane Torrent");
    private Spell Lifeblood = new Spell("Lifeblood");
    private Spell Stoneform = new Spell("Stoneform");
    private Spell Tailoring = new Spell("Tailoring");
    private Spell Leatherworking = new Spell("Leatherworking");
    private Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private Spell War_Stomp = new Spell("War Stomp");
    private Spell Berserking = new Spell("Berserking");

    #endregion InitializeSpell

    public Survival()
    {
        Main.range = 30.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            if (!ObjectManager.Me.IsMounted)
            {
                buffoutfight();

                if (!Fight.InFight && look.IsReady)
                {
                    look = new Timer(5000);
                    Lua.RunMacroText("/targetfriendplayer");
                }

                if (Fight.InFight && ObjectManager.Me.Target > 0 && ObjectManager.Target.GetDistance > Main.range)
                {
                    fighttimer = new Timer(60000);
                }

                if (Fight.InFight && ObjectManager.Me.Target > 0)
                {
                    if (ObjectManager.Me.Target != lastTarget && ObjectManager.Target.GetDistance <= Main.range)
                    {
                        pull();
                        lastTarget = ObjectManager.Me.Target;
                    }
                    fight();
                    if (!Fight.InFight)
                    {
                        Logging.WriteFight(" - Target Down - ");
                        look = new Timer(5000);
                    }

                    if (fighttimer.IsReady && ObjectManager.Target.HealthPercent > 90 && ObjectManager.Me.Target > 0 &&
                        ObjectManager.GetNumberAttackPlayer() < 2)
                    {
                        Logging.WriteFight(" - Target Evading - ");
                        break;
                    }
                }
            }
            if (ObjectManager.Me.IsMounted) mountchill = new Timer(2000);
            Thread.Sleep(350);
        }
    }

    public void pull()
    {
        if (hardmob()) Logging.WriteFight(" -  Pull Hard Mob - ");
        if (!hardmob()) Logging.WriteFight(" -  Pull Easy Mob - ");
        fighttimer = new Timer(60000);

        if (Concussive_Shot.KnownSpell && Concussive_Shot.IsSpellUsable && Concussive_Shot.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(5116);
            // Concussive_Shot.Launch();
        }
    }

    public void buffoutfight()
    {
        if (Fight.InFight || ObjectManager.Me.IsDeadMe) return;

        pet();

        if (!ObjectManager.Me.HaveBuff(79640) &&
            ItemsManager.GetItemCountByIdLUA(58149) == 1)
        {
            Logging.WriteFight("Use Alchi Flask");
            Lua.RunMacroText("/use item:58149");
        }

        if (Aspect_of_the_Iron_Hawk.KnownSpell && Aspect_of_the_Iron_Hawk.IsSpellUsable &&
            !Aspect_of_the_Iron_Hawk.HaveBuff)
        {
            Aspect_of_the_Iron_Hawk.Launch();
        }
        else if (Aspect_of_the_Hawk.KnownSpell && Aspect_of_the_Hawk.IsSpellUsable && !Aspect_of_the_Hawk.HaveBuff &&
                 !Aspect_of_the_Iron_Hawk.HaveBuff)
        {
            Aspect_of_the_Hawk.Launch();
        }
    }

    public void fight()
    {
        pet();
        selfheal();
        buffinfight();
        if (ObjectManager.GetNumberAttackPlayer() > 1) fighttimer = new Timer(60000);

        if (ObjectManager.GetNumberAttackPlayer() > 2 && Explosive_Trap.IsSpellUsable && Trap_Launcher.IsSpellUsable &&
            Arcane_Shot.IsDistanceGood)
        {
            traplaunchertimer = new Timer(1100);
            Trap_Launcher.Launch();
            while (!traplaunchertimer.IsReady)
            {
                if (Explosive_Trap.KnownSpell && Explosive_Trap.IsSpellUsable)
                {
                    SpellManager.CastSpellByIDAndPosition(13813, ObjectManager.Target.Position);
                }
            }
        }

        if (ObjectManager.GetNumberAttackPlayer() < 2 && Immolation_Trap.IsSpellUsable && Trap_Launcher.IsSpellUsable &&
            Arcane_Shot.IsDistanceGood &&
            !ObjectManager.Target.IsTargetingMe && ObjectManager.Target.HealthPercent > 70)
        {
            traplaunchertimer = new Timer(1100);
            Trap_Launcher.Launch();
            while (!traplaunchertimer.IsReady)
            {
                if (Immolation_Trap.KnownSpell && Immolation_Trap.IsSpellUsable)
                {
                    SpellManager.CastSpellByIDAndPosition(13795, ObjectManager.Target.Position);
                }
            }
        }

        if (ObjectManager.GetNumberAttackPlayer() < 2 && Immolation_Trap.IsSpellUsable && !Trap_Launcher.KnownSpell &&
            Arcane_Shot.IsDistanceGood)
        {
            Immolation_Trap.Launch();
        }


        if (Kill_Shot.KnownSpell && Kill_Shot.IsSpellUsable && Kill_Shot.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(53351);
            // Kill_Shot.Launch();
        }

        if (Hunters_Mark.KnownSpell && Hunters_Mark.IsSpellUsable && Hunters_Mark.IsDistanceGood &&
            !Hunters_Mark.TargetHaveBuff)
        {
            SpellManager.CastSpellByIdLUA(1130);
            // Hunters_Mark.Launch();
        }

        if ((ObjectManager.GetNumberAttackPlayer() > 2 || hardmob()) && Misdirection.KnownSpell &&
            Misdirection.IsSpellUsable)
        {
            Lua.RunMacroText("/cast [@pet] Misdirection");
            Lua.RunMacroText("/cast [@pet] Irreführung");
            Lua.RunMacroText("/cast [@pet] Détournement");
            Lua.RunMacroText("/cast [@pet] Перенаправление");
            Lua.RunMacroText("/cast [@pet] Redirección");
            Lua.RunMacroText("/cast [@pet] Redirecionar");
        }

        if (ObjectManager.Me.HaveBuff(56453))
        {
            if (Explosive_Shot.KnownSpell && Explosive_Shot.IsSpellUsable && Explosive_Shot.IsDistanceGood)
            {
                SpellManager.CastSpellByIdLUA(53301);
                // Explosive_Shot.Launch();
            }
            if (Arcane_Shot.KnownSpell && Arcane_Shot.IsSpellUsable && Arcane_Shot.IsDistanceGood)
            {
                SpellManager.CastSpellByIdLUA(3044);
                // Arcane_Shot.Launch();
            }
            return;
        }

        if (Concussive_Shot.KnownSpell && Concussive_Shot.IsSpellUsable && Concussive_Shot.IsDistanceGood &&
            !ObjectManager.Target.HaveBuff(1978))
        {
            SpellManager.CastSpellByIdLUA(5116);
            // Concussive_Shot.Launch();
        }

        if (!ObjectManager.Target.HaveBuff(1978) && Serpent_Sting_debuff.IsReady && Arcane_Shot.IsDistanceGood)
        {
            Serpent_Sting_debuff = new Timer(2500);
            Serpent_Sting.Launch();
        }

        if (!ObjectManager.Target.HaveBuff(1978) && !Serpent_Sting_debuff.IsReady)
        {
            if (Kill_Shot.KnownSpell && Kill_Shot.IsSpellUsable && Kill_Shot.IsDistanceGood)
            {
                Kill_Shot.Launch();
            }
        }

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) && Snake_Trap.IsSpellUsable &&
            Arcane_Shot.IsDistanceGood && !ObjectManager.Target.GetMove && Trap_Launcher.KnownSpell)
        {
            traplaunchertimer = new Timer(1100);
            Trap_Launcher.Launch();
            while (!traplaunchertimer.IsReady)
            {
                if (Snake_Trap.KnownSpell && Snake_Trap.IsSpellUsable && Trap_Launcher.HaveBuff)
                {
                    SpellManager.CastSpellByIDAndPosition(34600, ObjectManager.Target.Position);
                }
            }
        }

        if (Freezing_Trap.KnownSpell && Freezing_Trap.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 1)
        {
            SpellManager.CastSpellByIdLUA(1499);
            // Freezing_Trap.Launch();
        }

        if (ObjectManager.Target.HaveBuff(1978))
        {
            if (Multi_Shot.KnownSpell && Multi_Shot.IsSpellUsable && Multi_Shot.IsDistanceGood &&
                ObjectManager.GetNumberAttackPlayer() > 1)
            {
                SpellManager.CastSpellByIdLUA(2643);
                // Multi_Shot.Launch();
            }
            if (Explosive_Shot.KnownSpell && Explosive_Shot.IsSpellUsable && Explosive_Shot.IsDistanceGood &&
                ObjectManager.Me.FocusPercentage > 70)
            {
                SpellManager.CastSpellByIdLUA(53301);
                // Explosive_Shot.Launch();
            }

            if (Black_Arrow.KnownSpell && Black_Arrow.IsSpellUsable && Black_Arrow.IsDistanceGood &&
                !Explosive_Shot.IsSpellUsable)
            {
                SpellManager.CastSpellByIdLUA(3674);
                // Black_Arrow.Launch();
            }

            if (Arcane_Shot.KnownSpell && Arcane_Shot.IsSpellUsable && Arcane_Shot.IsDistanceGood &&
                !Explosive_Shot.IsSpellUsable && !Black_Arrow.IsSpellUsable)
            {
                SpellManager.CastSpellByIdLUA(3044);
                // Arcane_Shot.Launch();
            }
        }

        if (ObjectManager.Me.FocusPercentage < 70 && ObjectManager.Target.HaveBuff(1978))
        {
            if (Steady_Shot.KnownSpell && Steady_Shot.IsSpellUsable && Steady_Shot.IsDistanceGood &&
                !Cobra_Shot.KnownSpell)
            {
                SpellManager.CastSpellByIdLUA(56641);
                // Steady_Shot.Launch();
            }
            else if (Cobra_Shot.KnownSpell && Cobra_Shot.IsSpellUsable && Cobra_Shot.IsDistanceGood &&
                     ObjectManager.Target.HaveBuff(1978))
            {
                SpellManager.CastSpellByIdLUA(77767);
                // Cobra_Shot.Launch();
            }
        }

        if (ArcaneTorrent.KnownSpell && ArcaneTorrent.IsSpellUsable &&
            ObjectManager.Target.IsCast && ObjectManager.Target.GetDistance < 8)
        {
            ArcaneTorrent.Launch();
        }
    }

    private void pet()
    {
        if (ObjectManager.Me.IsMounted || !mountchill.IsReady) return;

        if ((ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) &&
            !ObjectManager.Me.IsMounted && !ObjectManager.Me.IsDeadMe)
        {
            Thread.Sleep(1000);
            SpellManager.CastSpellByIdLUA(883);
            //Call_Pet.Launch();
            Thread.Sleep(1000);
            if (!ObjectManager.Pet.IsAlive)
            {
                Revive_Pet.Launch();
                Thread.Sleep(1000);
            }
        }

        if (Mend_Pet.KnownSpell && Mend_Pet.IsSpellUsable && petheal.IsReady &&
            ObjectManager.Pet.Health > 0 && ObjectManager.Pet.HealthPercent < 70)
        {
            petheal = new Timer(9000);
            Mend_Pet.Launch();
        }

        if (Fight.InFight) Lua.RunMacroText("/petattack");
    }

    private void buffinfight()
    {
        if (!Fight.InFight) return;

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            Stoneform.KnownSpell && Stoneform.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(20594);
            // Stoneform.Launch();
        }

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            War_Stomp.KnownSpell && War_Stomp.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(20549);
            // War_Stomp.Launch();
        }

        if (Berserking.KnownSpell && Berserking.IsSpellUsable && Arcane_Shot.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(1454);
            // Berserking.Launch();
        }

        if (Rapid_Fire.KnownSpell && Rapid_Fire.IsSpellUsable &&
            (hardmob() || ObjectManager.GetNumberAttackPlayer() > 2) && Arcane_Shot.IsDistanceGood)
        {
            Rapid_Fire.Launch();
        }
    }

    private void selfheal()
    {
        if (ObjectManager.Me.HealthPercent < 80 &&
            Lifeblood.KnownSpell && Lifeblood.IsSpellUsable)
        {
            Lifeblood.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 80 &&
            Gift_of_the_Naaru.KnownSpell && Gift_of_the_Naaru.IsSpellUsable)
        {
            Gift_of_the_Naaru.Launch();
        }

        if (Disengage.KnownSpell && Disengage.IsSpellUsable && Disengage.IsDistanceGood &&
            ObjectManager.Target.HealthPercent > 30 && ObjectManager.Target.GetDistance < 5)
        {
            disengagetimer = new Timer(2000);
            while (ObjectManager.Target.GetDistance < 5 && !disengagetimer.IsReady)
                if (Wing_Clip.KnownSpell && Wing_Clip.IsSpellUsable && Wing_Clip.IsDistanceGood &&
                    !Wing_Clip.TargetHaveBuff)
                {
                    SpellManager.CastSpellByIdLUA(2974);
                    // Wing_Clip.Launch();
                }
            SpellManager.CastSpellByIdLUA(781);
            // Disengage.Launch();
            if (Concussive_Shot.KnownSpell && Concussive_Shot.IsSpellUsable && Concussive_Shot.IsDistanceGood)
            {
                SpellManager.CastSpellByIdLUA(5116);
                // Concussive_Shot.Launch();
            }
            return;
        }

        if (ObjectManager.Target.GetDistance < 10 &&
            ((Disengage.KnownSpell && !Disengage.IsSpellUsable) || !Disengage.KnownSpell))
        {
            disengagetimer = new Timer(5000);
            while (ObjectManager.Target.GetDistance < 10 || !disengagetimer.IsReady)
            {
                if (!Fight.InFight) return;
                Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "S");
                Thread.Sleep(100);

                if (Mend_Pet.KnownSpell && Mend_Pet.IsSpellUsable && petheal.IsReady &&
                    ObjectManager.Pet.Health > 0 && ObjectManager.Pet.HealthPercent < 70)
                {
                    petheal = new Timer(9000);
                    Mend_Pet.Launch();
                }

                if (Wing_Clip.KnownSpell && Wing_Clip.IsSpellUsable && Wing_Clip.IsDistanceGood &&
                    !Wing_Clip.TargetHaveBuff)
                {
                    SpellManager.CastSpellByIdLUA(2974);
                    // Wing_Clip.Launch();
                }

                if (Counterattack.KnownSpell && Counterattack.IsSpellUsable && Counterattack.IsDistanceGood)
                {
                    SpellManager.CastSpellByIdLUA(19306);
                    // Counterattack.Launch();
                }

                if (Raptor_Strike.KnownSpell && Raptor_Strike.IsSpellUsable && Raptor_Strike.IsDistanceGood)
                {
                    SpellManager.CastSpellByIdLUA(2973);
                    // Raptor_Strike.Launch();
                }

                if (Kill_Command.KnownSpell && Kill_Command.IsSpellUsable && Kill_Command.IsDistanceGood)
                {
                    SpellManager.CastSpellByIdLUA(34026);
                    // Kill_Command.Launch();
                }

                if (Kill_Shot.KnownSpell && Kill_Shot.IsSpellUsable && Kill_Shot.IsDistanceGood)
                {
                    SpellManager.CastSpellByIdLUA(53351);
                    // Kill_Shot.Launch();
                }

                if (Explosive_Shot.KnownSpell && Explosive_Shot.IsSpellUsable && Explosive_Shot.IsDistanceGood &&
                    ObjectManager.Me.FocusPercentage > 70)
                {
                    SpellManager.CastSpellByIdLUA(53301);
                    // Explosive_Shot.Launch();
                }

                if (Black_Arrow.KnownSpell && Black_Arrow.IsSpellUsable && Black_Arrow.IsDistanceGood &&
                    !Explosive_Shot.IsSpellUsable)
                {
                    SpellManager.CastSpellByIdLUA(3674);
                    // Black_Arrow.Launch();
                }

                if (Arcane_Shot.KnownSpell && Arcane_Shot.IsSpellUsable && Arcane_Shot.IsDistanceGood)
                {
                    SpellManager.CastSpellByIdLUA(3044);
                    // Arcane_Shot.Launch();
                }

                if (Feign_Death.KnownSpell && Feign_Death.IsSpellUsable)
                {
                    Feign_Death.Launch();
                    Thread.Sleep(3000);
                }

                if (Freezing_Trap.KnownSpell && Freezing_Trap.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 1)
                {
                    SpellManager.CastSpellByIdLUA(1499);
                    // Freezing_Trap.Launch();
                }

                if (Scatter_Shot.KnownSpell && Scatter_Shot.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 1)
                {
                    Scatter_Shot.Launch();
                }

                if (ObjectManager.Me.HealthPercent < 30 && ObjectManager.Target.IsTargetingMe)
                {
                    if (!Feign_Death.IsSpellUsable && !Scatter_Shot.IsSpellUsable && Deterrence.KnownSpell &&
                        Deterrence.KnownSpell)
                    {
                        Deterrence.Launch();
                    }
                }

                Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "{SPACE}");
            }
        }

        if (Feign_Death.KnownSpell && Feign_Death.IsSpellUsable && ObjectManager.Me.HealthPercent < 15 &&
            ObjectManager.Pet.Health > 10)
        {
            Feign_Death.Launch();
            Thread.Sleep(3000);
        }

        if (Feign_Death.KnownSpell && Feign_Death.IsSpellUsable && ObjectManager.Me.HealthPercent < 15 &&
            (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0))
        {
            Feign_Death.Launch();
            Lua.RunMacroText("/cleartarget");
            Thread.Sleep(30000);
        }
    }

    public bool hardmob()
    {
        if (((ObjectManager.Target.MaxHealth*100)/ObjectManager.Me.MaxHealth) > 110)
        {
            return true;
        }
        return false;
    }
}

public class Marks
{
    #region InitializeSpell

    // Marks Only
    private Spell Aimed_Shot = new Spell("Aimed Shot");
    private Spell Silencing_Shot = new Spell("Silencing Shot");
    private Spell Readiness = new Spell("Readiness");
    private Spell Chimera_Shot = new Spell("Chimera Shot");

    // DPS
    private Spell Raptor_Strike = new Spell("Raptor Strike");
    private Spell Arcane_Shot = new Spell("Arcane Shot");
    private Spell Steady_Shot = new Spell("Steady Shot");
    private Spell Serpent_Sting = new Spell("Serpent Sting");
    private Spell Multi_Shot = new Spell("Multi-Shot");
    private Spell Kill_Shot = new Spell("Kill Shot");
    private Spell Explosive_Trap = new Spell("Explosive Trap");
    private Spell Cobra_Shot = new Spell("Cobra Shot");
    private Spell Immolation_Trap = new Spell("Immolation Trap");

    // BUFF & HELPING
    private Spell Concussive_Shot = new Spell("Concussive Shot");
    private Spell Aspect_of_the_Hawk = new Spell(13165);
    private Spell Aspect_of_the_Iron_Hawk = new Spell(109260);
    private Spell Disengage = new Spell("Disengage");
    private Spell Hunters_Mark = new Spell("Hunter's Mark");
    private Spell Scatter_Shot = new Spell("Scatter Shot"); // 19503
    private Spell Feign_Death = new Spell("Feign Death"); //	5384
    private Spell Snake_Trap = new Spell("Snake Trap");
    private Spell Ice_Trap = new Spell("Ice Trap");
    private Spell Freezing_Trap = new Spell("Freezing Trap");
    private Spell Trap_Launcher = new Spell("Trap Launcher"); //	77769
    private Spell Rapid_Fire = new Spell("Rapid Fire"); //	3045
    private Spell Misdirection = new Spell("Misdirection");
    private Spell Deterrence = new Spell("Deterrence"); //	19263
    private Spell Wing_Clip = new Spell("Wing Clip");

    // PET
    private Spell Kill_Command = new Spell("Kill Command");
    private Spell Mend_Pet = new Spell("Mend Pet"); //	136
    private Spell Revive_Pet = new Spell("Revive Pet"); //	982
    private Spell Call_Pet = new Spell("Call Pet 1"); //	883

    // TIMER
    private Timer look = new Timer(0);
    private Timer fighttimer = new Timer(0);
    private Timer petheal = new Timer(0);
    private Timer traplaunchertimer = new Timer(0);
    private Timer disengagetimer = new Timer(0);
    private Timer Serpent_Sting_debuff = new Timer(0);
    private Timer mountchill = new Timer(0);

    // Profession & Racials
    private Spell ArcaneTorrent = new Spell("Arcane Torrent");
    private Spell Lifeblood = new Spell("Lifeblood");
    private Spell Stoneform = new Spell("Stoneform");
    private Spell Tailoring = new Spell("Tailoring");
    private Spell Leatherworking = new Spell("Leatherworking");
    private Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private Spell War_Stomp = new Spell("War Stomp");
    private Spell Berserking = new Spell("Berserking");

    #endregion InitializeSpell

    public Marks()
    {
        Main.range = 30.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            if (!ObjectManager.Me.IsMounted)
            {
                buffoutfight();

                if (!Fight.InFight && look.IsReady)
                {
                    look = new Timer(5000);
                    Lua.RunMacroText("/targetfriendplayer");
                }

                if (Fight.InFight && ObjectManager.Me.Target > 0 && ObjectManager.Target.GetDistance > Main.range)
                {
                    fighttimer = new Timer(60000);
                }

                if (Fight.InFight && ObjectManager.Me.Target > 0)
                {
                    if (ObjectManager.Me.Target != lastTarget && ObjectManager.Target.GetDistance <= Main.range)
                    {
                        pull();
                        lastTarget = ObjectManager.Me.Target;
                    }
                    fight();
                    if (!Fight.InFight)
                    {
                        Logging.WriteFight(" - Target Down - ");
                        look = new Timer(5000);
                    }

                    if (fighttimer.IsReady && ObjectManager.Target.HealthPercent > 90 && ObjectManager.Me.Target > 0 &&
                        ObjectManager.GetNumberAttackPlayer() < 2)
                    {
                        Logging.WriteFight(" - Target Evading - ");
                        break;
                    }
                }
            }
            if (ObjectManager.Me.IsMounted) mountchill = new Timer(2000);
            Thread.Sleep(350);
        }
    }

    public void pull()
    {
        if (hardmob()) Logging.WriteFight(" -  Pull Hard Mob - ");
        if (!hardmob()) Logging.WriteFight(" -  Pull Easy Mob - ");
        fighttimer = new Timer(60000);

        if (Concussive_Shot.KnownSpell && Concussive_Shot.IsSpellUsable && Concussive_Shot.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(5116);
            // Concussive_Shot.Launch();
        }
    }

    public void buffoutfight()
    {
        if (Fight.InFight || ObjectManager.Me.IsDeadMe) return;

        pet();

        if (!ObjectManager.Me.HaveBuff(79640) &&
            ItemsManager.GetItemCountByIdLUA(58149) == 1)
        {
            Logging.WriteFight("Use Alchi Flask");
            Lua.RunMacroText("/use item:58149");
        }

        if (Aspect_of_the_Iron_Hawk.KnownSpell && Aspect_of_the_Iron_Hawk.IsSpellUsable &&
            !Aspect_of_the_Iron_Hawk.HaveBuff)
        {
            Aspect_of_the_Iron_Hawk.Launch();
        }
        else if (Aspect_of_the_Hawk.KnownSpell && Aspect_of_the_Hawk.IsSpellUsable && !Aspect_of_the_Hawk.HaveBuff &&
                 !Aspect_of_the_Iron_Hawk.HaveBuff)
        {
            Aspect_of_the_Hawk.Launch();
        }
    }

    public void fight()
    {
        pet();
        selfheal();
        buffinfight();
        if (ObjectManager.GetNumberAttackPlayer() > 1) fighttimer = new Timer(60000);

        if (ObjectManager.GetNumberAttackPlayer() > 2 && Explosive_Trap.IsSpellUsable && Trap_Launcher.IsSpellUsable &&
            Arcane_Shot.IsDistanceGood)
        {
            traplaunchertimer = new Timer(1000);
            Trap_Launcher.Launch();
            while (!traplaunchertimer.IsReady)
            {
                if (Explosive_Trap.KnownSpell && Explosive_Trap.IsSpellUsable)
                {
                    SpellManager.CastSpellByIDAndPosition(13813, ObjectManager.Target.Position);
                }
            }
        }

        if (ObjectManager.GetNumberAttackPlayer() < 2 && Immolation_Trap.IsSpellUsable && Trap_Launcher.IsSpellUsable &&
            Arcane_Shot.IsDistanceGood &&
            !ObjectManager.Target.IsTargetingMe && ObjectManager.Target.HealthPercent > 70)
        {
            traplaunchertimer = new Timer(1000);
            Trap_Launcher.Launch();
            while (!traplaunchertimer.IsReady)
            {
                if (Immolation_Trap.KnownSpell && Immolation_Trap.IsSpellUsable)
                {
                    SpellManager.CastSpellByIDAndPosition(13795, ObjectManager.Target.Position);
                }
            }
        }

        if (ObjectManager.GetNumberAttackPlayer() < 2 && Immolation_Trap.IsSpellUsable && !Trap_Launcher.KnownSpell &&
            Arcane_Shot.IsDistanceGood)
        {
            Immolation_Trap.Launch();
        }


        if (Kill_Shot.KnownSpell && Kill_Shot.IsSpellUsable && Kill_Shot.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(53351);
            // Kill_Shot.Launch();
        }

        if (Hunters_Mark.KnownSpell && Hunters_Mark.IsSpellUsable && Hunters_Mark.IsDistanceGood &&
            !Hunters_Mark.TargetHaveBuff && !Chimera_Shot.KnownSpell)
        {
            SpellManager.CastSpellByIdLUA(1130);
            // Hunters_Mark.Launch();
        }

        if ((ObjectManager.GetNumberAttackPlayer() > 2 || hardmob()) && Misdirection.KnownSpell &&
            Misdirection.IsSpellUsable)
        {
            Lua.RunMacroText("/cast [@pet] Misdirection");
            Lua.RunMacroText("/cast [@pet] Irreführung");
            Lua.RunMacroText("/cast [@pet] Détournement");
            Lua.RunMacroText("/cast [@pet] Перенаправление");
            Lua.RunMacroText("/cast [@pet] Redirección");
            Lua.RunMacroText("/cast [@pet] Redirecionar");
        }

        if (ObjectManager.Me.HaveBuff(82926) && Aimed_Shot.IsSpellUsable && Aimed_Shot.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(19434);
            // Aimed_Shot.Launch();
        }

        if (ObjectManager.Me.HaveBuff(82897))
        {
            Kill_Command.Launch();
        }

        if (Concussive_Shot.KnownSpell && Concussive_Shot.IsSpellUsable && Concussive_Shot.IsDistanceGood &&
            !ObjectManager.Target.HaveBuff(1978))
        {
            SpellManager.CastSpellByIdLUA(5116);
            // Concussive_Shot.Launch();
        }

        if (!ObjectManager.Target.HaveBuff(1978) && Serpent_Sting_debuff.IsReady && Arcane_Shot.IsDistanceGood)
        {
            Serpent_Sting_debuff = new Timer(2000);
            Serpent_Sting.Launch();
        }

        if (!ObjectManager.Target.HaveBuff(1978) && !Serpent_Sting_debuff.IsReady)
        {
            if (Kill_Shot.KnownSpell && Kill_Shot.IsSpellUsable && Kill_Shot.IsDistanceGood)
            {
                Kill_Shot.Launch();
            }
            if (Kill_Shot.KnownSpell && Kill_Shot.IsSpellUsable && Kill_Shot.IsDistanceGood)
            {
                Kill_Command.Launch();
            }
        }

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) && Snake_Trap.IsSpellUsable &&
            Arcane_Shot.IsDistanceGood && !ObjectManager.Target.GetMove && Trap_Launcher.KnownSpell && look.IsReady)
        {
            traplaunchertimer = new Timer(1000);
            Trap_Launcher.Launch();
            while (!traplaunchertimer.IsReady)
            {
                if (Snake_Trap.KnownSpell && Snake_Trap.IsSpellUsable && Trap_Launcher.HaveBuff)
                {
                    SpellManager.CastSpellByIDAndPosition(34600, ObjectManager.Target.Position);
                }
            }
        }

        if (Freezing_Trap.KnownSpell && Freezing_Trap.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 1)
        {
            SpellManager.CastSpellByIdLUA(1499);
            // Freezing_Trap.Launch();
        }

        if (Scatter_Shot.KnownSpell && Scatter_Shot.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 2)
        {
            Scatter_Shot.Launch();
        }

        if (ObjectManager.Me.FocusPercentage > 44 && ObjectManager.Target.HaveBuff(1978))
        {
            if (Multi_Shot.KnownSpell && Multi_Shot.IsSpellUsable && Multi_Shot.IsDistanceGood &&
                ObjectManager.GetNumberAttackPlayer() > 1)
            {
                SpellManager.CastSpellByIdLUA(2643);
                // Multi_Shot.Launch();
            }
            if (Chimera_Shot.KnownSpell && Chimera_Shot.IsSpellUsable && Chimera_Shot.IsDistanceGood)
            {
                SpellManager.CastSpellByIdLUA(53209);
                // Chimera_Shot.Launch();
            }

            if (Aimed_Shot.KnownSpell && Aimed_Shot.IsSpellUsable && Aimed_Shot.IsDistanceGood &&
                !Chimera_Shot.IsSpellUsable)
            {
                SpellManager.CastSpellByIdLUA(19434);
                // Aimed_Shot.Launch();
            }

            if (Arcane_Shot.KnownSpell && Arcane_Shot.IsSpellUsable && Arcane_Shot.IsDistanceGood &&
                !Chimera_Shot.IsSpellUsable && !Aimed_Shot.IsSpellUsable)
            {
                SpellManager.CastSpellByIdLUA(3044);
                // Arcane_Shot.Launch();
            }
        }

        if (ObjectManager.Me.FocusPercentage < 50 && ObjectManager.Target.HaveBuff(1978))
        {
            if (Steady_Shot.KnownSpell && Steady_Shot.IsSpellUsable && Steady_Shot.IsDistanceGood)
            {
                SpellManager.CastSpellByIdLUA(56641);
                // Steady_Shot.Launch();
            }
        }

        if (ArcaneTorrent.KnownSpell && ArcaneTorrent.IsSpellUsable &&
            ObjectManager.Target.IsCast && ObjectManager.Target.GetDistance < 8)
        {
            ArcaneTorrent.Launch();
        }

        if (Silencing_Shot.KnownSpell && Silencing_Shot.IsSpellUsable && Silencing_Shot.IsDistanceGood &&
            ObjectManager.Target.IsCast)
        {
            Silencing_Shot.Launch();
        }
    }

    private void pet()
    {
        if (ObjectManager.Me.IsMounted || !mountchill.IsReady) return;

        if ((ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) &&
            !ObjectManager.Me.IsMounted && !ObjectManager.Me.IsDeadMe)
        {
            Thread.Sleep(1000);
            SpellManager.CastSpellByIdLUA(883);
            //Call_Pet.Launch();
            Thread.Sleep(1000);
            if (!ObjectManager.Pet.IsAlive)
            {
                Revive_Pet.Launch();
                Thread.Sleep(1000);
            }
        }

        if (Mend_Pet.KnownSpell && Mend_Pet.IsSpellUsable && petheal.IsReady &&
            ObjectManager.Pet.Health > 0 && ObjectManager.Pet.HealthPercent < 70)
        {
            petheal = new Timer(9000);
            Mend_Pet.Launch();
        }

        if (Fight.InFight) Lua.RunMacroText("/petattack");
    }

    private void buffinfight()
    {
        if (!Fight.InFight) return;

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            Stoneform.KnownSpell && Stoneform.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(20594);
            // Stoneform.Launch();
        }

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            War_Stomp.KnownSpell && War_Stomp.IsSpellUsable)
        {
            SpellManager.CastSpellByIdLUA(20549);
            // War_Stomp.Launch();
        }

        if (Berserking.KnownSpell && Berserking.IsSpellUsable && Arcane_Shot.IsDistanceGood)
        {
            SpellManager.CastSpellByIdLUA(1454);
            // Berserking.Launch();
        }

        if (Rapid_Fire.KnownSpell && Rapid_Fire.IsSpellUsable &&
            (hardmob() || ObjectManager.GetNumberAttackPlayer() > 2) && Arcane_Shot.IsDistanceGood)
        {
            Rapid_Fire.Launch();
        }

        if (Readiness.KnownSpell && Readiness.IsSpellUsable && (hardmob() || ObjectManager.GetNumberAttackPlayer() > 2) &&
            Arcane_Shot.IsDistanceGood &&
            !Rapid_Fire.IsSpellUsable && !Chimera_Shot.IsSpellUsable)
        {
            Readiness.Launch();
        }
    }

    private void selfheal()
    {
        if (ObjectManager.Me.HealthPercent < 80 &&
            Lifeblood.KnownSpell && Lifeblood.IsSpellUsable)
        {
            Lifeblood.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 80 &&
            Gift_of_the_Naaru.KnownSpell && Gift_of_the_Naaru.IsSpellUsable)
        {
            Gift_of_the_Naaru.Launch();
        }

        if (Disengage.KnownSpell && Disengage.IsSpellUsable && Disengage.IsDistanceGood &&
            ObjectManager.Target.HealthPercent > 30 && ObjectManager.Target.GetDistance < 5)
        {
            disengagetimer = new Timer(2000);
            while (ObjectManager.Target.GetDistance < 5 && !disengagetimer.IsReady)
                if (Wing_Clip.KnownSpell && Wing_Clip.IsSpellUsable && Wing_Clip.IsDistanceGood &&
                    !Wing_Clip.TargetHaveBuff)
                {
                    SpellManager.CastSpellByIdLUA(2974);
                    // Wing_Clip.Launch();
                }
            SpellManager.CastSpellByIdLUA(781);
            // Disengage.Launch();
            if (Concussive_Shot.KnownSpell && Concussive_Shot.IsSpellUsable && Concussive_Shot.IsDistanceGood)
            {
                SpellManager.CastSpellByIdLUA(5116);
                // Concussive_Shot.Launch();
            }
            return;
        }

        if (ObjectManager.Target.GetDistance < 10 &&
            ((Disengage.KnownSpell && !Disengage.IsSpellUsable) || !Disengage.KnownSpell))
        {
            disengagetimer = new Timer(5000);
            while (ObjectManager.Target.GetDistance < 10 || !disengagetimer.IsReady)
            {
                if (!Fight.InFight) return;
                Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "S");
                Thread.Sleep(100);

                if (Mend_Pet.KnownSpell && Mend_Pet.IsSpellUsable && petheal.IsReady &&
                    ObjectManager.Pet.Health > 0 && ObjectManager.Pet.HealthPercent < 70)
                {
                    petheal = new Timer(9000);
                    Mend_Pet.Launch();
                }

                if (Wing_Clip.KnownSpell && Wing_Clip.IsSpellUsable && Wing_Clip.IsDistanceGood &&
                    !Wing_Clip.TargetHaveBuff)
                {
                    SpellManager.CastSpellByIdLUA(2974);
                    // Wing_Clip.Launch();
                }

                if (ObjectManager.Me.HaveBuff(82926) && Aimed_Shot.IsSpellUsable && Aimed_Shot.IsDistanceGood)
                {
                    SpellManager.CastSpellByIdLUA(19434);
                    // Aimed_Shot.Launch();
                }

                if (Raptor_Strike.KnownSpell && Raptor_Strike.IsSpellUsable && Raptor_Strike.IsDistanceGood)
                {
                    SpellManager.CastSpellByIdLUA(2973);
                    // Raptor_Strike.Launch();
                }

                if (Kill_Command.KnownSpell && Kill_Command.IsSpellUsable && Kill_Command.IsDistanceGood)
                {
                    SpellManager.CastSpellByIdLUA(34026);
                    // Kill_Command.Launch();
                }

                if (Kill_Shot.KnownSpell && Kill_Shot.IsSpellUsable && Kill_Shot.IsDistanceGood)
                {
                    SpellManager.CastSpellByIdLUA(53351);
                    // Kill_Shot.Launch();
                }

                if (Chimera_Shot.KnownSpell && Chimera_Shot.IsSpellUsable && Chimera_Shot.IsDistanceGood &&
                    ObjectManager.Me.FocusPercentage > 70)
                {
                    SpellManager.CastSpellByIdLUA(53209);
                    // Chimera_Shot.Launch();
                }

                if (Arcane_Shot.KnownSpell && Arcane_Shot.IsSpellUsable && Arcane_Shot.IsDistanceGood &&
                    !Chimera_Shot.IsSpellUsable && ObjectManager.Me.FocusPercentage > 70)
                {
                    SpellManager.CastSpellByIdLUA(3044);
                    // Arcane_Shot.Launch();
                }

                if (Feign_Death.KnownSpell && Feign_Death.IsSpellUsable)
                {
                    Feign_Death.Launch();
                    Thread.Sleep(3000);
                }

                if (Freezing_Trap.KnownSpell && Freezing_Trap.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 1)
                {
                    SpellManager.CastSpellByIdLUA(1499);
                    // Freezing_Trap.Launch();
                }

                if (Scatter_Shot.KnownSpell && Scatter_Shot.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 1)
                {
                    Scatter_Shot.Launch();
                }

                if (ObjectManager.Me.HealthPercent < 30 && ObjectManager.Target.IsTargetingMe)
                {
                    if (!Feign_Death.IsSpellUsable && !Scatter_Shot.IsSpellUsable && Deterrence.KnownSpell &&
                        Deterrence.KnownSpell)
                    {
                        Deterrence.Launch();
                    }
                }

                Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "{SPACE}");
            }
        }

        if (Feign_Death.KnownSpell && Feign_Death.IsSpellUsable && ObjectManager.Me.HealthPercent < 30 &&
            ObjectManager.Pet.Health > 10)
        {
            Feign_Death.Launch();
            Thread.Sleep(2500);
        }

        if (Feign_Death.KnownSpell && Feign_Death.IsSpellUsable && ObjectManager.Me.HealthPercent < 15 &&
            (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0))
        {
            Feign_Death.Launch();
            Lua.RunMacroText("/cleartarget");
            Thread.Sleep(30000);
        }
    }

    public bool hardmob()
    {
        if (((ObjectManager.Target.MaxHealth*100)/ObjectManager.Me.MaxHealth) > 110)
        {
            return true;
        }
        return false;
    }
}

public class Hunter_BeastMaster
{
    [Serializable]
    public class HunterBeastMasterSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        public bool UseBerserking = true;
        /* Hunter aspects */
        // To be done
        /* Offensive Spell */

        /* Offensive Cooldown */

        /* Defensive Cooldown */

        /* Healing Spell */

        public HunterBeastMasterSettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Hunter Beast Master Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            /* Hunter aspects */
            /* ... */
        }

        public static HunterBeastMasterSettings CurrentSetting { get; set; }

        public static HunterBeastMasterSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Hunter_BeastMaster.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Hunter_BeastMaster.HunterBeastMasterSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Hunter_BeastMaster.HunterBeastMasterSettings();
            }
        }
    }

    private readonly HunterBeastMasterSettings MySettings = HunterBeastMasterSettings.GetSettings();

    #region Profession & Racials

    private readonly Spell ArcaneTorrent = new Spell("Arcane Torrent");
    private readonly Spell Lifeblood = new Spell("Lifeblood");
    private readonly Spell Stoneform = new Spell("Stoneform");
    private readonly Spell Gift_of_the_Naaru = new Spell("Gift of the Naaru");
    private readonly Spell War_Stomp = new Spell("War Stomp");
    private readonly Spell Berserking = new Spell("Berserking");

    #endregion

    #region Others

    // Beast Mastery only
    private readonly Spell Beastial_Wrath = new Spell("Beastial Wrath");
    private readonly Spell Focus_Fire = new Spell(82692); //"Focus Fire");
    private readonly Spell Intimidation = new Spell("Intimidation");
    // Beast master with a Spirit Beast only
    private readonly Spell Spirit_Mend = new Spell("Spirit Mend");

    // DPS
    private readonly Spell Arcane_Shot = new Spell("Arcane Shot");
    private readonly Spell Steady_Shot = new Spell("Steady Shot");
    private readonly Spell Serpent_Sting = new Spell("Serpent Sting");
    private readonly Spell Multi_Shot = new Spell("Multi-Shot");
    private readonly Spell Kill_Shot = new Spell("Kill Shot");
    private readonly Spell Explosive_Trap = new Spell("Explosive Trap");
    private readonly Spell Cobra_Shot = new Spell("Cobra Shot");
    private readonly Spell Immolation_Trap = new Spell("Immolation Trap");
    // New talents to implement
    // #1 - none
    // #2
    private readonly Spell Binding_Shot = new Spell(109248); // *
    private readonly Spell Silencing_Shot = new Spell(34490); // *
    private readonly Spell Wyvern_Sting = new Spell(19386); // *
    // #3
    private readonly Spell Aspect_of_the_Iron_Hawk = new Spell(109260); // *
    private readonly Spell Exhilaration = new Spell(109304); // *
    // #4
    private readonly Spell Dire_Beast = new Spell(120679); // *
    private readonly Spell Fervor = new Spell(82726); // *
    // #5
    private readonly Spell A_Murder_of_Crows = new Spell(131894); // *
    private readonly Spell Blink_Strike = new Spell(130392); // *
    private readonly Spell Lynx_Rush = new Spell(120697); // *
    // #6
    private readonly Spell Barrage = new Spell(120360); // *
    private readonly Spell Glaive_Toss = new Spell(117050); // *
    private readonly Spell Powershot = new Spell("Powershot"); // 109259

    // BUFF & HELPING
    private readonly Spell Concussive_Shot = new Spell("Concussive Shot");
    private readonly Spell Aspect_of_the_Hawk = new Spell(13165);
    private readonly Spell Disengage = new Spell("Disengage");
    private readonly Spell Hunters_Mark = new Spell("Hunter's Mark");
    private readonly Spell Scatter_Shot = new Spell("Scatter Shot"); // 19503
    private readonly Spell Feign_Death = new Spell("Feign Death"); // 5384
    private readonly Spell Snake_Trap = new Spell("Snake Trap");
    private readonly Spell Ice_Trap = new Spell("Ice Trap");
    private readonly Spell Freezing_Trap = new Spell("Freezing Trap");
    private readonly Spell Trap_Launcher = new Spell("Trap Launcher"); // 77769 - we need to manage the toggle
    private readonly Spell Rapid_Fire = new Spell("Rapid Fire"); // 3045
    private readonly Spell Misdirection = new Spell("Misdirection");
    private readonly Spell Deterrence = new Spell("Deterrence"); // 19263

    // PET
    private readonly Spell Kill_Command = new Spell("Kill Command");
    private readonly Spell Mend_Pet = new Spell("Mend Pet"); //	136
    private readonly Spell Revive_Pet = new Spell("Revive Pet"); //	982
    private readonly Spell Call_Pet = new Spell("Call Pet 1"); //	883
    // (*) : many spells exist with this name, so we have to use the spell id

    // TIMER
    private Timer look = new Timer(0);
    private Timer fighttimer = new Timer(0);
    private Timer petheal = new Timer(0);
    private Timer traplaunchertimer = new Timer(0);
    private Timer disengagetimer = new Timer(0);
    private Timer Serpent_Sting_debuff = new Timer(0);
    private Timer mountchill = new Timer(0);

    #endregion Others

    internal static bool reporting = true;

    public Hunter_BeastMaster()
    {
        Main.range = 30.0f;
        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            if (reporting)
            {
                Logging.WriteFight("Arcane ? : " + Arcane_Shot.KnownSpell);
                reporting = false;
            }
            if (!ObjectManager.Me.IsMounted)
            {
                buffoutfight();

                if (!Fight.InFight && look.IsReady)
                {
                    look = new Timer(5000);
                    Lua.RunMacroText("/targetfriendplayer");
                }

                if (Fight.InFight && ObjectManager.Me.Target > 0 && ObjectManager.Target.GetDistance > Main.range)
                {
                    fighttimer = new Timer(60000);
                }

                if (Fight.InFight && ObjectManager.Me.Target > 0)
                {
                    if (ObjectManager.Me.Target != lastTarget && ObjectManager.Target.GetDistance <= Main.range)
                    {
                        pull();
                        lastTarget = ObjectManager.Me.Target;
                    }
                    fight();
                    if (!Fight.InFight)
                    {
                        Logging.WriteFight(" - Target Down - ");
                        look = new Timer(5000);
                    }

                    if (fighttimer.IsReady && ObjectManager.Target.HealthPercent > 90 && ObjectManager.Me.Target > 0 &&
                        ObjectManager.GetNumberAttackPlayer() < 2)
                    {
                        Logging.WriteFight(" - Target Evading - ");
                        break;
                    }
                }
            }
            if (ObjectManager.Me.IsMounted) mountchill = new Timer(2000);
            Thread.Sleep(350);
        }
    }

    public void pull()
    {
        if (hardmob())
            Logging.WriteFight(" -  Pull Hard Mob - ");
        else
            Logging.WriteFight(" -  Pull Easy Mob - ");
        fighttimer = new Timer(60000);

        if (Concussive_Shot.KnownSpell && Concussive_Shot.IsSpellUsable && Concussive_Shot.IsDistanceGood)
        {
            //SpellManager.CastSpellByIdLUA(5116);
            Concussive_Shot.Launch();
        }
    }

    public void buffoutfight()
    {
        if (Fight.InFight || ObjectManager.Me.IsDeadMe) return;
        pet();
        if (!ObjectManager.Me.HaveBuff(79640) &&
            ItemsManager.GetItemCountByIdLUA(58149) == 1)
        {
            Logging.WriteFight("Use Alchi Flask");
            Lua.RunMacroText("/use item:58149");
        }
        if (Aspect_of_the_Iron_Hawk.KnownSpell && Aspect_of_the_Iron_Hawk.IsSpellUsable &&
            !Aspect_of_the_Iron_Hawk.HaveBuff)
        {
            Aspect_of_the_Iron_Hawk.Launch();
        }
        else if (Aspect_of_the_Hawk.KnownSpell && Aspect_of_the_Hawk.IsSpellUsable && !Aspect_of_the_Hawk.HaveBuff &&
                 !Aspect_of_the_Iron_Hawk.HaveBuff)
        {
            Aspect_of_the_Hawk.Launch();
        }
    }

    public void fight()
    {
        pet();
        selfheal();
        buffinfight();
        if (ObjectManager.GetNumberAttackPlayer() > 1) fighttimer = new Timer(60000);

        if (ObjectManager.GetNumberAttackPlayer() > 2 && Explosive_Trap.IsSpellUsable && Trap_Launcher.IsSpellUsable &&
            Arcane_Shot.IsDistanceGood)
        {
            traplaunchertimer = new Timer(1100);
            Trap_Launcher.Launch();
            while (!traplaunchertimer.IsReady)
            {
                if (Explosive_Trap.KnownSpell && Explosive_Trap.IsSpellUsable)
                {
                    SpellManager.CastSpellByIDAndPosition(13813, ObjectManager.Target.Position);
                }
            }
        }

        if (ObjectManager.GetNumberAttackPlayer() < 2 && Immolation_Trap.IsSpellUsable && Trap_Launcher.IsSpellUsable &&
            Arcane_Shot.IsDistanceGood &&
            !ObjectManager.Target.IsTargetingMe && ObjectManager.Target.HealthPercent > 70)
        {
            traplaunchertimer = new Timer(1100);
            Trap_Launcher.Launch();
            while (!traplaunchertimer.IsReady)
            {
                if (Immolation_Trap.KnownSpell && Immolation_Trap.IsSpellUsable)
                {
                    SpellManager.CastSpellByIDAndPosition(13795, ObjectManager.Target.Position);
                }
            }
        }

        if (ObjectManager.GetNumberAttackPlayer() < 2 && Immolation_Trap.IsSpellUsable && !Trap_Launcher.KnownSpell &&
            Arcane_Shot.IsDistanceGood)
        {
            Immolation_Trap.Launch();
        }

        if (Kill_Shot.KnownSpell && Kill_Shot.IsSpellUsable && Kill_Shot.IsDistanceGood)
        {
            //SpellManager.CastSpellByIdLUA(53351);
            Kill_Shot.Launch();
        }

        if (Hunters_Mark.KnownSpell && Hunters_Mark.IsSpellUsable && Hunters_Mark.IsDistanceGood &&
            !Hunters_Mark.TargetHaveBuff)
        {
            //SpellManager.CastSpellByIdLUA(1130);
            Hunters_Mark.Launch();
        }

        if ((ObjectManager.GetNumberAttackPlayer() > 2 || hardmob()) && Misdirection.KnownSpell &&
            Misdirection.IsSpellUsable)
        {
            Lua.RunMacroText("/cast [@pet] Misdirection");
            Lua.RunMacroText("/cast [@pet] Irreführung");
            Lua.RunMacroText("/cast [@pet] Détournement");
            Lua.RunMacroText("/cast [@pet] Перенаправление");
            Lua.RunMacroText("/cast [@pet] Redirección");
            Lua.RunMacroText("/cast [@pet] Redirecionar");
        }

        if (Focus_Fire.KnownSpell && Focus_Fire.IsSpellUsable && ObjectManager.Pet.BuffStack(19615) == 5)
            // Frenzy Effect
        {
            //SpellManager.CastSpellByIdLUA(82692);
            Focus_Fire.Launch();
        }

        if (Concussive_Shot.KnownSpell && Concussive_Shot.IsSpellUsable && Concussive_Shot.IsDistanceGood &&
            !ObjectManager.Target.HaveBuff(118253))
        {
            //SpellManager.CastSpellByIdLUA(5116);
            Concussive_Shot.Launch();
        }

        if (!ObjectManager.Target.HaveBuff(118253) && Serpent_Sting_debuff.IsReady && Serpent_Sting.IsDistanceGood)
        {
            Serpent_Sting_debuff = new Timer(2500);
            Serpent_Sting.Launch();
        }

        if (!ObjectManager.Target.HaveBuff(118253) && !Serpent_Sting_debuff.IsReady)
        {
            if (Kill_Shot.KnownSpell && Kill_Shot.IsSpellUsable && Kill_Shot.IsDistanceGood)
            {
                Kill_Shot.Launch();
            }
        }

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) && Snake_Trap.IsSpellUsable &&
            Arcane_Shot.IsDistanceGood && !ObjectManager.Target.GetMove && Trap_Launcher.KnownSpell)
        {
            traplaunchertimer = new Timer(1100);
            Trap_Launcher.Launch();
            while (!traplaunchertimer.IsReady)
            {
                if (Snake_Trap.KnownSpell && Snake_Trap.IsSpellUsable && Trap_Launcher.HaveBuff)
                {
                    SpellManager.CastSpellByIDAndPosition(34600, ObjectManager.Target.Position);
                }
            }
        }

        if (Freezing_Trap.KnownSpell && Freezing_Trap.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 1)
        {
            //SpellManager.CastSpellByIdLUA(1499);
            Freezing_Trap.Launch();
        }

        if (ObjectManager.Target.HaveBuff(118253))
        {
            if (Multi_Shot.KnownSpell && Multi_Shot.IsSpellUsable && Multi_Shot.IsDistanceGood &&
                ObjectManager.GetNumberAttackPlayer() > 1)
            {
                //SpellManager.CastSpellByIdLUA(2643);
                Multi_Shot.Launch();
            }

            if (Kill_Command.KnownSpell && Kill_Command.IsSpellUsable)
            {
                //SpellManager.CastSpellByIdLUA(34026);
                Kill_Command.Launch();
            }

            if (Arcane_Shot.KnownSpell && Arcane_Shot.IsSpellUsable && Arcane_Shot.IsDistanceGood)
            {
                //SpellManager.CastSpellByIdLUA(3044);
                Arcane_Shot.Launch();
            }
        }

        if (ObjectManager.Me.FocusPercentage < 70 && ObjectManager.Target.HaveBuff(118253))
        {
            if (Steady_Shot.KnownSpell && Steady_Shot.IsSpellUsable && Steady_Shot.IsDistanceGood &&
                !Cobra_Shot.KnownSpell)
            {
                //SpellManager.CastSpellByIdLUA(56641);
                Steady_Shot.Launch();
            }
            else if (Cobra_Shot.KnownSpell && Cobra_Shot.IsSpellUsable && Cobra_Shot.IsDistanceGood &&
                     ObjectManager.Target.HaveBuff(118253))
            {
                //SpellManager.CastSpellByIdLUA(77767);
                Cobra_Shot.Launch();
            }
        }

        if (ArcaneTorrent.KnownSpell && ArcaneTorrent.IsSpellUsable && MySettings.UseArcaneTorrent &&
            ObjectManager.Target.IsCast && ObjectManager.Target.GetDistance < 8)
        {
            ArcaneTorrent.Launch();
        }
    }

    private void pet()
    {
        if (ObjectManager.Me.IsMounted || !mountchill.IsReady) return;

        if ((ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) &&
            !ObjectManager.Me.IsMounted && !ObjectManager.Me.IsDeadMe)
        {
            Thread.Sleep(1000);
            //SpellManager.CastSpellByIdLUA(883);
            Call_Pet.Launch();
            Thread.Sleep(1000);
            if (!ObjectManager.Pet.IsAlive)
            {
                Revive_Pet.Launch();
                Thread.Sleep(1000);
            }
        }

        if (Mend_Pet.KnownSpell && Mend_Pet.IsSpellUsable && petheal.IsReady &&
            ObjectManager.Pet.Health > 0 && ObjectManager.Pet.HealthPercent < 70)
        {
            petheal = new Timer(9000);
            Mend_Pet.Launch();
        }

        if (Fight.InFight) Lua.RunMacroText("/petattack");
        return;
    }

    private void buffinfight()
    {
        if (!Fight.InFight) return;

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            Intimidation.KnownSpell && Intimidation.IsSpellUsable)
        {
            //SpellManager.CastSpellByIdLUA(19577);
            Intimidation.Launch();
        }

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            Stoneform.KnownSpell && Stoneform.IsSpellUsable && MySettings.UseStoneform)
        {
            //SpellManager.CastSpellByIdLUA(20594);
            Stoneform.Launch();
        }

        if ((ObjectManager.GetNumberAttackPlayer() > 1 || hardmob()) &&
            ObjectManager.Me.HealthPercent < 65 &&
            ObjectManager.Target.GetDistance < 5 &&
            War_Stomp.KnownSpell && War_Stomp.IsSpellUsable && MySettings.UseWarStomp)
        {
            //SpellManager.CastSpellByIdLUA(20549);
            War_Stomp.Launch();
        }

        if (Berserking.KnownSpell && Berserking.IsSpellUsable && MySettings.UseBerserking && Arcane_Shot.IsDistanceGood)
        {
            //SpellManager.CastSpellByIdLUA(1454);
            Berserking.Launch();
        }

        if (Rapid_Fire.KnownSpell && Rapid_Fire.IsSpellUsable &&
            (hardmob() || ObjectManager.GetNumberAttackPlayer() > 2) && Arcane_Shot.IsDistanceGood)
        {
            Rapid_Fire.Launch();
        }
    }

    private void selfheal()
    {
        if (ObjectManager.Me.HealthPercent < 80 &&
            Lifeblood.KnownSpell && Lifeblood.IsSpellUsable && MySettings.UseLifeblood)
        {
            Lifeblood.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 80 &&
            Gift_of_the_Naaru.KnownSpell && Gift_of_the_Naaru.IsSpellUsable && MySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 80 &&
            Spirit_Mend.KnownSpell && Spirit_Mend.IsSpellUsable)
        {
            Lua.RunMacroText("/target " + ObjectManager.Me.Name);
            //SpellManager.CastSpellByIdLUA(90361);
            Spirit_Mend.Launch();
            Lua.RunMacroText("/targetlasttarget");
        }

        if (Disengage.KnownSpell && Disengage.IsSpellUsable && Disengage.IsDistanceGood &&
            ObjectManager.Target.HealthPercent > 30 && ObjectManager.Target.GetDistance < 5)
        {
            disengagetimer = new Timer(2000);
            //SpellManager.CastSpellByIdLUA(781);
            Disengage.Launch();
            if (Concussive_Shot.KnownSpell && Concussive_Shot.IsSpellUsable && Concussive_Shot.IsDistanceGood)
            {
                //SpellManager.CastSpellByIdLUA(5116);
                Concussive_Shot.Launch();
            }
            return;
        }

        if (ObjectManager.Target.GetDistance < 10 &&
            ((Disengage.KnownSpell && !Disengage.IsSpellUsable) || !Disengage.KnownSpell))
        {
            disengagetimer = new Timer(5000);
            while (ObjectManager.Target.GetDistance < 10 || !disengagetimer.IsReady)
            {
                if (!Fight.InFight) return;
                Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "S");
                Thread.Sleep(100);

                if (Mend_Pet.KnownSpell && Mend_Pet.IsSpellUsable && petheal.IsReady &&
                    ObjectManager.Pet.Health > 0 && ObjectManager.Pet.HealthPercent < 70)
                {
                    petheal = new Timer(9000);
                    Mend_Pet.Launch();
                }

                if (Kill_Command.KnownSpell && Kill_Command.IsSpellUsable && Kill_Command.IsDistanceGood)
                {
                    //SpellManager.CastSpellByIdLUA(34026);
                    Kill_Command.Launch();
                }

                if (Kill_Shot.KnownSpell && Kill_Shot.IsSpellUsable && Kill_Shot.IsDistanceGood)
                {
                    //SpellManager.CastSpellByIdLUA(53351);
                    Kill_Shot.Launch();
                }

                if (Arcane_Shot.KnownSpell && Arcane_Shot.IsSpellUsable && Arcane_Shot.IsDistanceGood)
                {
                    //SpellManager.CastSpellByIdLUA(3044);
                    Arcane_Shot.Launch();
                }

                if (Feign_Death.KnownSpell && Feign_Death.IsSpellUsable)
                {
                    Feign_Death.Launch();
                    Thread.Sleep(5000);
                }

                if (Freezing_Trap.KnownSpell && Freezing_Trap.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 1)
                {
                    //SpellManager.CastSpellByIdLUA(1499);
                    Freezing_Trap.Launch();
                }

                if (Scatter_Shot.KnownSpell && Scatter_Shot.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 1)
                {
                    Scatter_Shot.Launch();
                }

                if (ObjectManager.Me.HealthPercent < 30 && ObjectManager.Target.IsTargetingMe)
                {
                    if (!Feign_Death.IsSpellUsable && !Scatter_Shot.IsSpellUsable && Deterrence.KnownSpell &&
                        Deterrence.KnownSpell)
                    {
                        Deterrence.Launch();
                    }
                }

                Keyboard.DownKey(nManager.Wow.Memory.WowProcess.MainWindowHandle, "{SPACE}");
            }
        }

        if (Feign_Death.KnownSpell && Feign_Death.IsSpellUsable && ObjectManager.Me.HealthPercent < 15 &&
            ObjectManager.Pet.Health > 10)
        {
            Feign_Death.Launch();
            Thread.Sleep(5000);
        }

        if (Feign_Death.KnownSpell && Feign_Death.IsSpellUsable && ObjectManager.Me.HealthPercent < 15 &&
            (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0))
        {
            Feign_Death.Launch();
            Lua.RunMacroText("/cleartarget");
            Thread.Sleep(30000);
        }
    }

    public bool hardmob()
    {
        if (((ObjectManager.Target.MaxHealth*100)/ObjectManager.Me.MaxHealth) > 130)
        {
            return true;
        }
        return false;
    }
}

#endregion