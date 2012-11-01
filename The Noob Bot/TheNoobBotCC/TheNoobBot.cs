/*
* CustomClass for TheNoobBot
* Credit : Rival, Geesus, Enelya, Marstor, Vesper, Neo2003, Dreadlocks
* Thanks you !
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using nManager.Wow.Helpers;
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
            if (!loop)
                loop = true;
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
                            if (File.Exists(CurrentSettingsFile))
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
                            range = 5.0f;
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
                            if (File.Exists(CurrentSettingsFile))
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
                            range = 5.0f;
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
                            if (File.Exists(CurrentSettingsFile))
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
                            range = 5.0f;
                            new Deathknight_Frost();
                        }
                    }
                    else
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show(
                                "Your specification haven't be found, loading Deathknight Apprentice Settings");
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Deathknight_Apprentice.xml";
                            Deathknight_Apprentice.DeathknightApprenticeSettings CurrentSetting;
                            CurrentSetting = new Deathknight_Apprentice.DeathknightApprenticeSettings();
                            if (File.Exists(CurrentSettingsFile))
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
                            range = 5.0f;
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
                    var Warlock_Demonology_Spell = new Spell("Summon Felguard");
                    var Warlock_Affliction_Spell = new Spell("Unstable Affliction");
                    var Warlock_Destruction_Spell = new Spell("Conflagrate");

                    if (Warlock_Demonology_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Warlock_Demonology.xml";
                            Warlock_Demonology.WarlockDemonologySettings CurrentSetting;
                            CurrentSetting = new Warlock_Demonology.WarlockDemonologySettings();
                            if (File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Warlock_Demonology.WarlockDemonologySettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Warlock Demonology class...");
                            range = 30.0f;
                            new Warlock_Demonology();
                        }
                    }
                    else if (Warlock_Affliction_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Warlock_Affliction.xml";
                            Warlock_Affliction.WarlockAfflictionSettings CurrentSetting;
                            CurrentSetting = new Warlock_Affliction.WarlockAfflictionSettings();
                            if (File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Warlock_Affliction.WarlockAfflictionSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Warlock Affliction class...");
                            range = 30.0f;
                            new Warlock_Affliction();
                        }
                    }
                    else if (Warlock_Destruction_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Warlock_Destruction.xml";
                            Warlock_Destruction.WarlockDestructionSettings CurrentSetting;
                            CurrentSetting = new Warlock_Destruction.WarlockDestructionSettings();
                            if (File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Warlock_Destruction.WarlockDestructionSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Warlock Destruction class...");
                            range = 30.0f;
                            new Warlock_Destruction();
                        }
                    }
                    else
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show(
                                "Your specification haven't be found, loading Warlock Demonology Settings");
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Warlock_Demonology.xml";
                            Warlock_Demonology.WarlockDemonologySettings CurrentSetting;
                            CurrentSetting = new Warlock_Demonology.WarlockDemonologySettings();
                            if (File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Warlock_Demonology.WarlockDemonologySettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("No specialisation detected.");
                            Logging.WriteFight("Loading Warlock Demonology class...");
                            new Warlock_Demonology();
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
                            var CurrentSetting = new Paladin_Retribution.PaladinRetributionSettings();
                            if (File.Exists(CurrentSettingsFile))
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
                    }
                    else if (Paladin_Protection_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Paladin_Protection.xml";
                            var CurrentSetting = new Paladin_Protection.PaladinProtectionSettings();
                            if (File.Exists(CurrentSettingsFile))
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
                    }
                    else if (Paladin_Holy_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Paladin_Holy.xml";
                            var CurrentSetting = new Paladin_Holy.PaladinHolySettings();
                            if (File.Exists(CurrentSettingsFile))
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
                    }
                    else
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show(
                                "Your specification haven't be found, loading Paladin Retribution Settings");
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Paladin_Retribution.xml";
                            var CurrentSetting = new Paladin_Retribution.PaladinRetributionSettings();
                            if (File.Exists(CurrentSettingsFile))
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
                    }
                    break;

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
                            if (File.Exists(CurrentSettingsFile))
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
                    }
                    else if (Priest_Discipline_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show(
                                "Priest Discipline found, but no Discipline class available, loading Priest Shadow Settings");
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Priest_Shadow.xml";
                            Priest_Shadow.PriestShadowSettings CurrentSetting;
                            CurrentSetting = new Priest_Shadow.PriestShadowSettings();
                            if (File.Exists(CurrentSettingsFile))
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
                    }
                    else if (Priest_Holy_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show(
                                "Priest Holy found, but no Holy class available, loading Priest Shadow Settings");
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Priest_Shadow.xml";
                            Priest_Shadow.PriestShadowSettings CurrentSetting;
                            CurrentSetting = new Priest_Shadow.PriestShadowSettings();
                            if (File.Exists(CurrentSettingsFile))
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
                    }
                    else
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show(
                                "Your specification haven't be found, loading Priest Shadow Settings");
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Priest_Shadow.xml";
                            Priest_Shadow.PriestShadowSettings CurrentSetting;
                            CurrentSetting = new Priest_Shadow.PriestShadowSettings();
                            if (File.Exists(CurrentSettingsFile))
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
                            range = 30.0f;
                            new Priest_Shadow();
                        }
                    }
                    break;

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
                    var Warrior_Arms_Spell = new Spell("Mortal Strike");
                    var Warrior_Fury_Spell = new Spell("Bloodthirst");
                    var Warrior_Protection_Spell = new Spell("Shield Slam");

                    if (Warrior_Arms_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Warrior_Arms.xml";
                            Warrior_Arms.WarriorArmsSettings CurrentSetting;
                            CurrentSetting = new Warrior_Arms.WarriorArmsSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Warrior_Arms.WarriorArmsSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Warrior Arms class...");
                            new Warrior_Arms();
                        }
                    }
                    else if (Warrior_Fury_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Warrior_Fury.xml";
                            Warrior_Fury.WarriorFurySettings CurrentSetting;
                            CurrentSetting = new Warrior_Fury.WarriorFurySettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Warrior_Fury.WarriorFurySettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Warrior Fury class...");
                            new Warrior_Fury();
                        }
                    }
                    else if (Warrior_Protection_Spell.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Warrior_Protection.xml";
                            Warrior_Protection.WarriorProtectionSettings CurrentSetting;
                            CurrentSetting = new Warrior_Protection.WarriorProtectionSettings();
                            if (System.IO.File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Warrior_Protection.WarriorProtectionSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("Loading Warrior Protection class...");
                            new Warrior_Protection();
                        }
                    }
                    else
                    {
                        if (ConfigOnly)
                        {
                            MessageBox.Show(
                                "Your specification haven't be found, loading Warrior Arms Settings");
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Warrior_Arms.xml";
                            Warrior_Arms.WarriorArmsSettings CurrentSetting;
                            CurrentSetting = new Warrior_Arms.WarriorArmsSettings();
                            if (File.Exists(CurrentSettingsFile))
                            {
                                CurrentSetting =
                                    Settings.Load<Warrior_Arms.WarriorArmsSettings>(CurrentSettingsFile);
                            }
                            CurrentSetting.ToForm();
                            CurrentSetting.Save(CurrentSettingsFile);
                        }
                        else
                        {
                            Logging.WriteFight("No specialisation detected.");
                            Logging.WriteFight("Loading Warrior Arms class...");
                            new Warrior_Arms();
                        }
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
                    }
                    else if (FocusFire.KnownSpell)
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Hunter_BeastMaster.xml";
                            Hunter_BeastMaster.HunterBeastMasterSettings CurrentSetting;
                            CurrentSetting = new Hunter_BeastMaster.HunterBeastMasterSettings();
                            if (File.Exists(CurrentSettingsFile))
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
                    }
                    else
                    {
                        if (ConfigOnly)
                        {
                            string CurrentSettingsFile = Application.StartupPath +
                                                         "\\CustomClasses\\Settings\\Hunter_BeastMaster.xml";
                            Hunter_BeastMaster.HunterBeastMasterSettings CurrentSetting;
                            CurrentSetting = new Hunter_BeastMaster.HunterBeastMasterSettings();
                            if (File.Exists(CurrentSettingsFile))
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
        public bool UseChainsofIce = false;
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
            if (File.Exists(CurrentSettingsFile))
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
        public bool UseChainsofIce = false;
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
            if (File.Exists(CurrentSettingsFile))
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
        else if (ObjectManager.GetNumberAttackPlayer() == 0 && Deaths_Advance.KnownSpell && Deaths_Advance.IsSpellUsable
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
        while (ObjectManager.Target.GetDistance < 3 && ObjectManager.Target.InCombat)
        {
            nManager.Wow.Helpers.Keybindings.PressKeybindings(Keybindings.MOVEBACKWARD);
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
        public bool UseChainsofIce = false;
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
            if (File.Exists(CurrentSettingsFile))
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
        else if (ObjectManager.GetNumberAttackPlayer() == 0 && Deaths_Advance.KnownSpell && Deaths_Advance.IsSpellUsable
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
        while (ObjectManager.Target.GetDistance < 3 && ObjectManager.Target.InCombat)
        {
            nManager.Wow.Helpers.Keybindings.PressKeybindings(Keybindings.MOVEBACKWARD);
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
        public bool UseChainsofIce = false;
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
            if (File.Exists(CurrentSettingsFile))
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
        else if (ObjectManager.GetNumberAttackPlayer() == 0 && Deaths_Advance.KnownSpell && Deaths_Advance.IsSpellUsable
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
        while (ObjectManager.Target.GetDistance < 3 && ObjectManager.Target.InCombat)
        {
            nManager.Wow.Helpers.Keybindings.PressKeybindings(Keybindings.MOVEBACKWARD);
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
        while (ObjectManager.Target.GetDistance < 3 && ObjectManager.Target.InCombat)
        {
            nManager.Wow.Helpers.Keybindings.PressKeybindings(Keybindings.MOVEBACKWARD);
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

public class Warlock_Demonology
{
    [Serializable]
    public class WarlockDemonologySettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Warlock Buffs */
        public bool UseCurseofEnfeeblement = false;
        public bool UseCurseoftheElements = true;
        public bool UseDarkIntent = true;
        public bool UseGrimoireofSacrifice = true;
        public bool UseMetamorphosis = true;
        public bool UseSoulLink = true;
        public bool UseSoulstone = true;
        /* Offensive Spell */
        public bool UseCarrionSwarm = true;
        public bool UseCommandDemon = true;
        public bool UseCorruption = true;
        public bool UseDoom = true;
        public bool UseFelFlame = true;
        public bool UseHandofGuldan = true;
        public bool UseHarvestLife = true;
        public bool UseHellfire = true;
        public bool UseImmolationAura = true;
        public bool UseShadowBolt = true;
        public bool UseSoulFire = true;
        public bool UseSummonImp = false;
        public bool UseSummonVoidwalker = false;
        public bool UseSummonFelhunter = false;
        public bool UseSummonSuccubus = false;
        public bool UseSummonFelguard = true;
        public bool UseTouchofChaos = true;
        public bool UseVoidRay = true;
        /* Offensive Cooldown */
        public bool UseArchimondesVengeance = true;
        public bool UseDarkSoul = true;
        public bool UseGrimoireofService = true;
        public bool UseSummonDoomguard = true;
        public bool UseSummonInfernal = false;
        /* Defensive Cooldown */
        public bool UseDarkBargain = true;
        public bool UseHowlofTerror = true;
        public bool UseSacrificialPact = true;
        public bool UseShadowfury = true;
        public bool UseTwilightWard = true;
        public bool UseUnboundWill = true;
        public bool UseUnendingResolve = true;
        /* Healing Spell */
        public bool UseCreateHealthstone = true;
        public bool UseDarkRegeneration = true;
        public bool UseDrainLife = true;
        public bool UseHealthFunnel = true;
        public bool UseLifeTap = true;
        public bool UseMortalCoil = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;

        public WarlockDemonologySettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Warlock Demonology Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Warlock Buffs */
            AddControlInWinForm("Use Curse of Enfeeblement", "UseCurseofEnfeeblement", "Warlock Buffs");
            AddControlInWinForm("Use Curse of the Elements", "UseCurseoftheElements", "Warlock Buffs");
            AddControlInWinForm("Use Dark Intent", "UseDarkIntent", "Warlock Buffs");
            AddControlInWinForm("Use Grimoire of Sacrifice", "UseGrimoireofSacrifice", "Warlock Buffs");
            AddControlInWinForm("Use Metamorphosis", "UseMetamorphosis", "Warlock Buffs");
            AddControlInWinForm("Use Soul Link ", "UseSoulLink ", "Warlock Buffs");
            AddControlInWinForm("Use Soulstone", "UseSoulstone", "Warlock Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Carrion Swarm", "UseCarrionSwarm", "Offensive Spell");
            AddControlInWinForm("Use Command Demon", "UseCommandDemon", "Offensive Spell");
            AddControlInWinForm("Use Corruption", "UseCorruption", "Offensive Spell");
            AddControlInWinForm("Use Doom", "UseDoom", "Offensive Spell");
            AddControlInWinForm("Use Fel Flame", "UseFelFlame", "Offensive Spell");
            AddControlInWinForm("Use Hand of Guldan", "UseHandofGuldan", "Offensive Spell");
            AddControlInWinForm("Use Harvest Life", "UseHarvestLife", "Offensive Spell");
            AddControlInWinForm("Use Hellfire", "UseHellfire", "Offensive Spell");
            AddControlInWinForm("Use Immolation Aura", "UseImmolationAura", "Offensive Spell");
            AddControlInWinForm("Use Shadow Bolt", "UseShadowBolt", "Offensive Spell");
            AddControlInWinForm("Use Soul Fire", "UseSoulFire", "Offensive Spell");
            AddControlInWinForm("Use Summon Imp", "UseSummonImp", "Offensive Spell");
            AddControlInWinForm("Use Summon Voidwalker", "UseSummonVoidwalker", "Offensive Spell");
            AddControlInWinForm("Use Summon Felhunter", "UseSummonFelhunter", "Offensive Spell");
            AddControlInWinForm("Use Summon Succubus", "UseSummonSuccubus", "Offensive Spell");
            AddControlInWinForm("Use Summon Felguard", "UseSummonFelguard", "Offensive Spell");
            AddControlInWinForm("Use Touch of Chaos", "UseTouchofChaos", "Offensive Spell");
            AddControlInWinForm("Use Void Ray", "UseVoidRay", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Archimonde's Vengeance", "UseArchimondesVengeance", "Offensive Cooldown");
            AddControlInWinForm("Use Dark Soul", "UseDarkSoul", "Offensive Cooldown");
            AddControlInWinForm("Use Grimoire of Service", "UseGrimoireofService", "Offensive Cooldown");
            AddControlInWinForm("Use Summon Doomguard", "UseSummonDoomguard", "Offensive Cooldown");
            AddControlInWinForm("Use Summon Infernal", "UseSummonInfernal", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Dark Bargain", "UseDarkBargain", "Defensive Cooldown");
            AddControlInWinForm("Use Howl of Terror", "UseHowlofTerror", "Defensive Cooldown");
            AddControlInWinForm("Use Sacrificial Pact", "UseSacrificialPact", "Defensive Cooldown");
            AddControlInWinForm("Use Shadowfury", "UseShadowfury", "Defensive Cooldown");
            AddControlInWinForm("Use Twilight Ward", "UseTwilightWard", "Defensive Cooldown");
            AddControlInWinForm("Use Unbound Will", "UseUnboundWill", "Defensive Cooldown");
            AddControlInWinForm("Use Unending Resolve", "UseUnendingResolve", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Create Healthstone", "UseCreateHealthstone", "Healing Spell");
            AddControlInWinForm("Use Dark Regeneration", "UseDarkRegeneration", "Healing Spell");
            AddControlInWinForm("Use Drain Life", "UseDrainLife", "Healing Spell");
            AddControlInWinForm("Use Health Funnel", "UseHealthFunnel", "Healing Spell");
            AddControlInWinForm("Use Life Tap", "UseLifeTap", "Healing Spell");
            AddControlInWinForm("Use Mortal Coil", "UseMortalCoil", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static WarlockDemonologySettings CurrentSetting { get; set; }

        public static WarlockDemonologySettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Warlock_Demonology.xml";
            if (File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Warlock_Demonology.WarlockDemonologySettings>(CurrentSettingsFile);
            }
            else
            {
                return new Warlock_Demonology.WarlockDemonologySettings();
            }
        }
    }

    private readonly WarlockDemonologySettings MySettings = WarlockDemonologySettings.GetSettings();

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

    #region Warlock Buffs

    private readonly Spell Curse_of_Enfeeblement = new Spell("Curse of Enfeeblement");
    private readonly Spell Curse_of_the_Elements = new Spell("Curse of the Elements");
    private readonly Spell Dark_Intent = new Spell("Dark Intent");
    private readonly Spell Grimoire_of_Sacrifice = new Spell("Grimoire of Sacrifice");
    private readonly Spell Metamorphosis = new Spell("Metamorphosis");
    private readonly Spell Soul_Link = new Spell("Soul Link");
    private readonly Spell Soulstone = new Spell("Soulstone");

    #endregion

    #region Offensive Spell

    private readonly Spell Carrion_Swarm = new Spell("Carrion Swarm");
    private readonly Spell Command_Demon = new Spell("Command Demon");
    private readonly Spell Corruption = new Spell("Corruption");
    private Timer Corruption_Timer = new Timer(0);
    private readonly Spell Doom = new Spell("Doom");
    private Timer Doom_Timer = new Timer(0);
    private readonly Spell Fel_Flame = new Spell("Fel Flame");
    private readonly Spell Hand_of_Guldan = new Spell("Hand of Gul'dan");
    private readonly Spell Harvest_Life = new Spell("Harvest Life");
    private readonly Spell Hellfire = new Spell("Hellfire");
    private readonly Spell Immolation_Aura = new Spell("Immolation Aura");
    private readonly Spell Shadow_Bolt = new Spell("Shadow Bolt");
    private readonly Spell Soul_Fire = new Spell("Soul Fire");
    private readonly Spell Summon_Imp = new Spell("Summon Imp");
    private readonly Spell Summon_Voidwalker = new Spell("Summon Voidwalker");
    private readonly Spell Summon_Felhunter = new Spell("Summon Felhunter");
    private readonly Spell Summon_Succubus = new Spell("Summon Succubus");
    private readonly Spell Summon_Felguard = new Spell("Summon Felguard");
    private readonly Spell Touch_of_Chaos = new Spell("Touch of Chaos");
    private readonly Spell Void_Ray = new Spell("Void Ray");

    #endregion

    #region Offensive Cooldown

    private readonly Spell Archimondes_Vengeance = new Spell("Archimonde's Vengeance");
    private readonly Spell Dark_Soul = new Spell("Dark Soul");
    private readonly Spell Grimoire_of_Service = new Spell("Grimoire of Service");
    private readonly Spell Summon_Doomguard = new Spell("Summon Doomguard");
    private readonly Spell Summon_Infernal = new Spell("Summon Infernal");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Dark_Bargain = new Spell("Dark Bargain");
    private readonly Spell Howl_of_Terror = new Spell("Howl_of_Terror");
    private readonly Spell Sacrificial_Pact = new Spell("Sacrificial Pact");
    private readonly Spell Shadowfury = new Spell("Shadowfury");
    private readonly Spell Twilight_Ward = new Spell("Twilight Ward");
    private readonly Spell Unbound_Will = new Spell("Unbound Will");
    private readonly Spell Unending_Resolve = new Spell("Unending Resolve");

    #endregion

    #region Healing Spell

    private readonly Spell Create_Healthstone = new Spell("Create Healthstone");
    private Timer Healthstone_Timer = new Timer(0);
    private readonly Spell Dark_Regeneration = new Spell("Dark Regeneration");
    private readonly Spell Drain_Life = new Spell("Drain Life");
    private readonly Spell Health_Funnel = new Spell("Health Funnel");
    private readonly Spell Life_Tap = new Spell("Life Tap");
    private readonly Spell Mortal_Coil = new Spell("Mortal Coil");

    #endregion

    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    public int LC = 0;

    public Warlock_Demonology()
    {
        Main.range = 30.0f;
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
                            (Doom.IsDistanceGood || Corruption.IsDistanceGood))
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
        if (Corruption.IsSpellUsable && Corruption.IsDistanceGood && Corruption.KnownSpell
            && MySettings.UseDoom && ObjectManager.Me.DemonicFury > 199)
        {
            if (Metamorphosis.KnownSpell && Metamorphosis.IsSpellUsable
                && MySettings.UseMetamorphosis && !Metamorphosis.HaveBuff)
            {
                Metamorphosis.Launch();
                Thread.Sleep(400);
                Corruption.Launch();
                Doom_Timer = new Timer(1000*60);
            }

            if (Metamorphosis.HaveBuff)
            {
                Thread.Sleep(2500);
                Metamorphosis.Launch();
            }
            return;
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

    public void LowCombat()
    {
        AvoidMelee();
        Heal();
        Defense_Cycle();
        Buff();

        if (ObjectManager.Me.BarTwoPercentage < 75 && Life_Tap.KnownSpell && Life_Tap.IsSpellUsable
            && MySettings.UseLifeTap)
        {
            Life_Tap.Launch();
            return;
        }
        else
        {
            if (Fel_Flame.IsDistanceGood && Fel_Flame.IsSpellUsable && Fel_Flame.KnownSpell
                && MySettings.UseFelFlame)
            {
                Fel_Flame.Launch();
                if (ObjectManager.Target.HealthPercent < 50 && ObjectManager.Target.HealthPercent > 0)
                {
                    Fel_Flame.Launch();
                    return;
                }
            }
        }

        if (Hellfire.IsSpellUsable && Hellfire.KnownSpell && Hellfire.IsDistanceGood
            && MySettings.UseHellfire)
        {
            Hellfire.Launch();
            Thread.Sleep(200);
            while (ObjectManager.Me.IsCast && ObjectManager.Target.HealthPercent > 0)
            {
                Thread.Sleep(200);
            }
            return;
        }
    }

    public void DPS_Burst()
    {
        if (MySettings.UseTrinket && Trinket_Timer.IsReady && ObjectManager.Target.GetDistance < 40)
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
                 && ObjectManager.Target.GetDistance < 40)
        {
            Berserking.Launch();
            return;
        }
        else if (Blood_Fury.IsSpellUsable && Blood_Fury.KnownSpell && MySettings.UseBloodFury
                 && ObjectManager.Target.GetDistance < 40)
        {
            Blood_Fury.Launch();
            return;
        }
        else if (Lifeblood.IsSpellUsable && Lifeblood.KnownSpell && MySettings.UseLifeblood
                 && ObjectManager.Target.GetDistance < 40)
        {
            Lifeblood.Launch();
            return;
        }
        else if (MySettings.UseEngGlove && Engineering.KnownSpell && Engineering_Timer.IsReady
                 && ObjectManager.Target.GetDistance < 40)
        {
            Logging.WriteFight("Use Engineering Gloves.");
            Lua.RunMacroText("/use 10");
            Engineering_Timer = new Timer(1000*60);
            return;
        }
        else if (Dark_Soul.KnownSpell && Dark_Soul.IsSpellUsable
                 && MySettings.UseDarkSoul && ObjectManager.Target.GetDistance < 40)
        {
            Dark_Soul.Launch();
            return;
        }
        else if (Summon_Doomguard.KnownSpell && Summon_Doomguard.IsSpellUsable
                 && MySettings.UseSummonDoomguard && Summon_Doomguard.IsDistanceGood)
        {
            Summon_Doomguard.Launch();
            return;
        }
        else if (Summon_Infernal.KnownSpell && Summon_Infernal.IsSpellUsable
                 && MySettings.UseSummonInfernal && Summon_Infernal.IsDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(1122, ObjectManager.Target.Position);
            return;
        }
        else if (Archimondes_Vengeance.KnownSpell && Archimondes_Vengeance.IsSpellUsable
                 && MySettings.UseArchimondesVengeance)
        {
            Archimondes_Vengeance.Launch();
            return;
        }
        else
        {
            if (Grimoire_of_Service.KnownSpell && Grimoire_of_Service.IsSpellUsable
                && MySettings.UseGrimoireofService && ObjectManager.Target.GetDistance < 40)
            {
                Grimoire_of_Service.Launch();
                return;
            }
        }
    }

    public void DPS_Cycle()
    {
        if (ObjectManager.Me.DemonicFury > 899 || (Doom_Timer.IsReady || !ObjectManager.Target.HaveBuff(603)))
        {
            if (ObjectManager.Me.DemonicFury > 199)
            {
                if (Corruption.KnownSpell && Corruption.IsSpellUsable && Corruption.IsDistanceGood
                    && MySettings.UseCorruption)
                {
                    Corruption.Launch();
                    Corruption_Timer = new Timer(1000*20);
                }

                if (MySettings.UseMetamorphosis)
                    MetamorphosisCombat();
                return;
            }
        }

        if (Metamorphosis.HaveBuff)
            MetamorphosisCombat();

        if (Curse_of_the_Elements.KnownSpell && Curse_of_the_Elements.IsSpellUsable && MySettings.UseCurseoftheElements
            && Curse_of_the_Elements.IsDistanceGood && !Curse_of_the_Elements.TargetHaveBuff)
        {
            Curse_of_the_Elements.Launch();
            return;
        }
        else if (Curse_of_Enfeeblement.KnownSpell && Curse_of_Enfeeblement.IsSpellUsable &&
                 MySettings.UseCurseofEnfeeblement
                 && Curse_of_Enfeeblement.IsDistanceGood && !Curse_of_Enfeeblement.TargetHaveBuff &&
                 !MySettings.UseCurseoftheElements)
        {
            Curse_of_Enfeeblement.Launch();
            return;
        }
        else if (ObjectManager.Me.BarTwoPercentage < 75 && Life_Tap.KnownSpell && Life_Tap.IsSpellUsable
                 && MySettings.UseLifeTap)
        {
            Life_Tap.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && Harvest_Life.IsSpellUsable && Harvest_Life.KnownSpell
                 && MySettings.UseHarvestLife && Harvest_Life.IsDistanceGood)
        {
            Harvest_Life.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 2 && Command_Demon.IsSpellUsable && Command_Demon.KnownSpell
                 && Command_Demon.IsDistanceGood && ObjectManager.Pet.Guid == 207 && ObjectManager.Pet.Health > 0
                 && MySettings.UseCommandDemon)
        {
            Command_Demon.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && Hellfire.IsSpellUsable && Hellfire.KnownSpell
                 && MySettings.UseHellfire && ObjectManager.Target.GetDistance < 20
                 && (!Harvest_Life.KnownSpell || !MySettings.UseHarvestLife))
        {
            Hellfire.Launch();
            Thread.Sleep(200);
            while (ObjectManager.Me.IsCast && ObjectManager.Target.HealthPercent > 0)
            {
                Thread.Sleep(200);
            }
            return;
        }
        else if (Corruption.KnownSpell && Corruption.IsSpellUsable && Corruption.IsDistanceGood
                 && MySettings.UseCorruption && (!Corruption.TargetHaveBuff || Corruption_Timer.IsReady))
        {
            Corruption.Launch();
            Corruption_Timer = new Timer(1000*20);
            return;
        }
        else if (Hand_of_Guldan.KnownSpell && Hand_of_Guldan.IsSpellUsable && Hand_of_Guldan.IsDistanceGood
                 && MySettings.UseHandofGuldan && !ObjectManager.Target.HaveBuff(47960))
        {
            Hand_of_Guldan.Launch();
            return;
        }
        else if (Soul_Fire.KnownSpell && Soul_Fire.IsSpellUsable && Soul_Fire.IsDistanceGood
                 && MySettings.UseSoulFire && ObjectManager.Me.HaveBuff(122355))
        {
            Soul_Fire.Launch();
            return;
        }
        else
        {
            if (Shadow_Bolt.KnownSpell && Shadow_Bolt.IsSpellUsable && Shadow_Bolt.IsDistanceGood
                && MySettings.UseShadowBolt)
            {
                Shadow_Bolt.Launch();
                return;
            }
        }
    }

    public void MetamorphosisCombat()
    {
        while (ObjectManager.Me.DemonicFury > 100)
        {
            if (Metamorphosis.KnownSpell && Metamorphosis.IsSpellUsable && !Metamorphosis.HaveBuff)
            {
                Metamorphosis.Launch();
                Thread.Sleep(700);
            }

            if (ObjectManager.GetNumberAttackPlayer() > 2)
            {
                if (Hellfire.KnownSpell && Hellfire.IsSpellUsable && Metamorphosis.HaveBuff
                    && MySettings.UseImmolationAura && ObjectManager.Target.GetDistance < 20)
                {
                    Hellfire.Launch();
                    Thread.Sleep(200);
                }
                else if (Carrion_Swarm.IsSpellUsable && Carrion_Swarm.KnownSpell
                         && Metamorphosis.HaveBuff && ObjectManager.Target.GetDistance < 20)
                {
                    Carrion_Swarm.Launch();
                    Thread.Sleep(200);
                }
                else
                {
                    if (Fel_Flame.IsSpellUsable && Fel_Flame.KnownSpell && Fel_Flame.IsDistanceGood
                        && MySettings.UseVoidRay && Metamorphosis.HaveBuff)
                    {
                        Fel_Flame.Launch();
                        Thread.Sleep(200);
                    }
                }
            }

            else
            {
                if (Corruption.IsDistanceGood && Metamorphosis.HaveBuff
                    && Corruption.KnownSpell && Corruption.IsSpellUsable && MySettings.UseDoom
                    && (Doom_Timer.IsReady || !ObjectManager.Target.HaveBuff(603)))
                {
                    Corruption.Launch();
                    Doom_Timer = new Timer(1000*60);
                    Thread.Sleep(200);
                }

                if (Shadow_Bolt.KnownSpell && Shadow_Bolt.IsSpellUsable && Shadow_Bolt.IsDistanceGood
                    && MySettings.UseTouchofChaos && Metamorphosis.HaveBuff)
                {
                    Shadow_Bolt.Launch();
                    Thread.Sleep(200);
                }
            }
        }

        Thread.Sleep(700);
        if (Metamorphosis.HaveBuff)
        {
            Metamorphosis.Launch();
            return;
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

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        Pet();

        if (!Dark_Intent.HaveBuff && Dark_Intent.KnownSpell && Dark_Intent.IsSpellUsable
            && MySettings.UseDarkIntent)
        {
            Dark_Intent.Launch();
            return;
        }
        else if (!Soul_Link.HaveBuff && Soul_Link.KnownSpell && Soul_Link.IsSpellUsable
                 && MySettings.UseSoulLink && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0))
        {
            Soul_Link.Launch();
            return;
        }
        if (!Soulstone.HaveBuff && Soulstone.KnownSpell && Soulstone.IsSpellUsable
            && MySettings.UseSoulstone)
        {
            Soulstone.Launch();
            return;
        }
        else
        {
            if (ItemsManager.GetItemCountByIdLUA(5512) == 0 && Create_Healthstone.KnownSpell
                && Create_Healthstone.IsSpellUsable && MySettings.UseCreateHealthstone)
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
        }
    }

    private void Pet()
    {
        if (Health_Funnel.KnownSpell)
        {
            if (ObjectManager.Pet.HealthPercent > 0 && ObjectManager.Pet.HealthPercent < 50
                && Health_Funnel.IsSpellUsable && Health_Funnel.KnownSpell && MySettings.UseHealthFunnel)
            {
                Health_Funnel.Launch();
                while (ObjectManager.Me.IsCast)
                {
                    if (ObjectManager.Pet.HealthPercent > 85 || ObjectManager.Pet.IsDead)
                        break;
                    Thread.Sleep(100);
                }
            }
        }

        if (MySettings.UseSummonFelhunter && Summon_Felhunter.KnownSpell && Summon_Felhunter.IsSpellUsable &&
            (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && !Grimoire_of_Sacrifice.HaveBuff)
        {
            Summon_Felhunter.Launch();
            Logging.WriteFight(" - PET DEAD - ");
        }
        else if (MySettings.UseSummonFelguard && Summon_Felguard.KnownSpell && Summon_Felguard.IsSpellUsable &&
                 (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && !Grimoire_of_Sacrifice.HaveBuff)
        {
            Summon_Felguard.Launch();
            Logging.WriteFight(" - PET DEAD - ");
        }
        else if (MySettings.UseSummonImp && Summon_Imp.KnownSpell && Summon_Imp.IsSpellUsable &&
                 (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && !Grimoire_of_Sacrifice.HaveBuff)
        {
            Summon_Imp.Launch();
            Logging.WriteFight(" - PET DEAD - ");
        }
        else if (MySettings.UseSummonVoidwalker && Summon_Voidwalker.KnownSpell &&
                 Summon_Voidwalker.IsSpellUsable &&
                 (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && !Grimoire_of_Sacrifice.HaveBuff)
        {
            Summon_Voidwalker.Launch();
            Logging.WriteFight(" - PET DEAD - ");
        }
        else if (MySettings.UseSummonSuccubus && Summon_Succubus.KnownSpell &&
                 Summon_Succubus.IsSpellUsable &&
                 (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && !Grimoire_of_Sacrifice.HaveBuff)
        {
            Summon_Succubus.Launch();
            Logging.WriteFight(" - PET DEAD - ");
        }
        Thread.Sleep(200);
        if (Grimoire_of_Sacrifice.KnownSpell && !Grimoire_of_Sacrifice.HaveBuff && Grimoire_of_Sacrifice.IsSpellUsable
            && MySettings.UseGrimoireofSacrifice && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0))
        {
            Grimoire_of_Sacrifice.Launch();
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
        else if (ObjectManager.Me.HealthPercent < 65 && Dark_Regeneration.IsSpellUsable && Dark_Regeneration.KnownSpell
                 && MySettings.UseDarkRegeneration)
        {
            Dark_Regeneration.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 75 && ItemsManager.GetItemCountByIdLUA(5512) > 0
                 && MySettings.UseCreateHealthstone && Healthstone_Timer.IsReady)
        {
            Logging.WriteFight("Use Healthstone.");
            nManager.Wow.Helpers.ItemsManager.UseItem("Healthstone");
            Healthstone_Timer = new Timer(1000*60*2);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 85 && Mortal_Coil.IsSpellUsable && Mortal_Coil.KnownSpell
                 && MySettings.UseMortalCoil && Mortal_Coil.IsDistanceGood)
        {
            Mortal_Coil.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 70 && Drain_Life.KnownSpell
                && MySettings.UseDrainLife && Drain_Life.IsDistanceGood && Drain_Life.IsSpellUsable)
            {
                Drain_Life.Launch();
                while (ObjectManager.Me.IsCast)
                {
                    Thread.Sleep(200);
                }
                return;
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 70 && MySettings.UseUnendingResolve
            && Unending_Resolve.KnownSpell && Unending_Resolve.IsSpellUsable)
        {
            Unending_Resolve.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 20 && MySettings.UseHowlofTerror
                 && Howl_of_Terror.KnownSpell && Howl_of_Terror.IsSpellUsable && ObjectManager.Target.GetDistance < 8)
        {
            Howl_of_Terror.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 40 && MySettings.UseDarkBargain
                 && Dark_Bargain.KnownSpell && Dark_Bargain.IsSpellUsable)
        {
            Dark_Bargain.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 95 && MySettings.UseSacrificialPact
                 && Sacrificial_Pact.KnownSpell && Sacrificial_Pact.IsSpellUsable
                 && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0))
        {
            Sacrificial_Pact.Launch();
            OnCD = new Timer(1000*10);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && MySettings.UseShadowfury
                 && Shadowfury.KnownSpell && Shadowfury.IsSpellUsable && ObjectManager.Target.GetDistance < 8)
        {
            Shadowfury.Launch();
            OnCD = new Timer(1000*3);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && War_Stomp.IsSpellUsable && War_Stomp.KnownSpell
                 && MySettings.UseWarStomp)
        {
            War_Stomp.Launch();
            OnCD = new Timer(1000*2);
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell
                && MySettings.UseStoneform)
            {
                Stoneform.Launch();
                OnCD = new Timer(1000*8);
                return;
            }
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ObjectManager.Target.GetDistance < 8
            && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable && MySettings.UseArcaneTorrent)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
                 && MySettings.UseTwilightWard && Twilight_Ward.KnownSpell && Twilight_Ward.IsSpellUsable)
        {
            Twilight_Ward.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseSummonFelhunter
                && Command_Demon.IsSpellUsable && Command_Demon.KnownSpell && ObjectManager.Target.GetDistance < 40)
            {
                Command_Demon.Launch();
                return;
            }
        }
    }

    private void AvoidMelee()
    {
        while (ObjectManager.Target.GetDistance < 3 && ObjectManager.Target.InCombat)
        {
            nManager.Wow.Helpers.Keybindings.PressKeybindings(Keybindings.MOVEBACKWARD);
        }
    }
}

public class Warlock_Destruction
{
    [Serializable]
    public class WarlockDestructionSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Warlock Buffs */
        public bool UseCurseofEnfeeblement = false;
        public bool UseCurseoftheElements = true;
        public bool UseDarkIntent = true;
        public bool UseGrimoireofSacrifice = true;
        public bool UseSoulLink = true;
        public bool UseSoulstone = true;
        /* Offensive Spell */
        public bool UseChaosBolt = true;
        public bool UseCommandDemon = true;
        public bool UseConflagrate = true;
        public bool UseFelFlame = true;
        public bool UseFireandBrimstone = true;
        public bool UseHarvestLife = true;
        public bool UseImmolate = true;
        public bool UseIncinerate = true;
        public bool UseRainofFire = true;
        public bool UseShadowburn = true;
        public bool UseSummonImp = false;
        public bool UseSummonVoidwalker = false;
        public bool UseSummonFelhunter = true;
        public bool UseSummonSuccubus = false;
        /* Offensive Cooldown */
        public bool UseArchimondesVengeance = true;
        public bool UseDarkSoul = true;
        public bool UseGrimoireofService = true;
        public bool UseSummonDoomguard = true;
        public bool UseSummonInfernal = false;
        /* Defensive Cooldown */
        public bool UseDarkBargain = true;
        public bool UseHowlofTerror = true;
        public bool UseSacrificialPact = true;
        public bool UseShadowfury = true;
        public bool UseTwilightWard = true;
        public bool UseUnboundWill = true;
        public bool UseUnendingResolve = true;
        /* Healing Spell */
        public bool UseCreateHealthstone = true;
        public bool UseDarkRegeneration = true;
        public bool UseDrainLife = true;
        public bool UseEmberTap = true;
        public bool UseFlamesofXoroth = true;
        public bool UseHealthFunnel = true;
        public bool UseLifeTap = true;
        public bool UseMortalCoil = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;

        public WarlockDestructionSettings()
        {
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Warlock Buffs */
            AddControlInWinForm("Use Curse of Enfeeblement", "UseCurseofEnfeeblement", "Warlock Buffs");
            AddControlInWinForm("Use Curse of the Elements", "UseCurseoftheElements", "Warlock Buffs");
            AddControlInWinForm("Use Dark Intent", "UseDarkIntent", "Warlock Buffs");
            AddControlInWinForm("Use Grimoire of Sacrifice", "UseGrimoireofSacrifice", "Warlock Buffs");
            AddControlInWinForm("Use Soul Link ", "UseSoulLink ", "Warlock Buffs");
            AddControlInWinForm("Use Soulstone", "UseSoulstone", "Warlock Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Chaos Bolt", "UseChaosBolt", "Offensive Spell");
            AddControlInWinForm("Use Command Demon", "UseCommandDemon", "Offensive Spell");
            AddControlInWinForm("Use Conflagrate", "UseConflagrate", "Offensive Spell");
            AddControlInWinForm("Use Fel Flame", "UseFelFlame", "Offensive Spell");
            AddControlInWinForm("Use Fire and Brimstone", "UseFireandBrimstone", "Offensive Spell");
            AddControlInWinForm("Use Harvest Life", "UseHarvestLife", "Offensive Spell");
            AddControlInWinForm("Use Immolate", "UseImmolate", "Offensive Spell");
            AddControlInWinForm("Use Incinerate", "UseIncinerate", "Offensive Spell");
            AddControlInWinForm("Use Rain of Fire", "UseRainofFire", "Offensive Spell");
            AddControlInWinForm("Use Shadowburn", "UseShadowburn", "Offensive Spell");
            AddControlInWinForm("Use Summon Imp", "UseSummonImp", "Offensive Spell");
            AddControlInWinForm("Use Summon Voidwalker", "UseSummonVoidwalker", "Offensive Spell");
            AddControlInWinForm("Use Summon Felhunter", "UseSummonFelhunter", "Offensive Spell");
            AddControlInWinForm("Use Summon Succubus", "UseSummonSuccubus", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Archimonde's Vengeance", "UseArchimondesVengeance", "Offensive Cooldown");
            AddControlInWinForm("Use Dark Soul", "UseDarkSoul", "Offensive Cooldown");
            AddControlInWinForm("Use Grimoire of Service", "UseGrimoireofService", "Offensive Cooldown");
            AddControlInWinForm("Use Summon Doomguard", "UseSummonDoomguard", "Offensive Cooldown");
            AddControlInWinForm("Use Summon Infernal", "UseSummonInfernal", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Dark Bargain", "UseDarkBargain", "Defensive Cooldown");
            AddControlInWinForm("Use Howl of Terror", "UseHowlofTerror", "Defensive Cooldown");
            AddControlInWinForm("Use Sacrificial Pact", "UseSacrificialPact", "Defensive Cooldown");
            AddControlInWinForm("Use Shadowfury", "UseShadowfury", "Defensive Cooldown");
            AddControlInWinForm("Use Twilight Ward", "UseTwilightWard", "Defensive Cooldown");
            AddControlInWinForm("Use Unbound Will", "UseUnboundWill", "Defensive Cooldown");
            AddControlInWinForm("Use Unending Resolve", "UseUnendingResolve", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Create Healthstone", "UseCreateHealthstone", "Healing Spell");
            AddControlInWinForm("Use Dark Regeneration", "UseDarkRegeneration", "Healing Spell");
            AddControlInWinForm("Use Drain Life", "UseDrainLife", "Healing Spell");
            AddControlInWinForm("Use Ember Tap", "UseEmberTap", "Healing Spell");
            AddControlInWinForm("Use Flames of Xoroth", "UseFlamesofXoroth", "Healing Spell");
            AddControlInWinForm("Use Health Funnel", "UseHealthFunnel", "Healing Spell");
            AddControlInWinForm("Use Life Tap", "UseLifeTap", "Healing Spell");
            AddControlInWinForm("Use Mortal Coil", "UseMortalCoil", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static WarlockDestructionSettings CurrentSetting { get; set; }

        public static WarlockDestructionSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Warlock_Destruction.xml";
            if (File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Warlock_Destruction.WarlockDestructionSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Warlock_Destruction.WarlockDestructionSettings();
            }
        }
    }

    private readonly WarlockDestructionSettings MySettings = WarlockDestructionSettings.GetSettings();

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

    #region Warlock Buffs

    private readonly Spell Curse_of_Enfeeblement = new Spell("Curse of Enfeeblement");
    private readonly Spell Curse_of_the_Elements = new Spell("Curse of the Elements");
    private readonly Spell Dark_Intent = new Spell("Dark Intent");
    private readonly Spell Grimoire_of_Sacrifice = new Spell("Grimoire of Sacrifice");
    private readonly Spell Soul_Link = new Spell("Soul Link");
    private readonly Spell Soulstone = new Spell("Soulstone");

    #endregion

    #region Offensive Spell

    private readonly Spell Chaos_Bolt = new Spell("Chaos Bolt");
    private readonly Spell Command_Demon = new Spell("Command Demon");
    private readonly Spell Conflagrate = new Spell("Conflagrate");
    private readonly Spell Corruption = new Spell("Corruption");
    private readonly Spell Fel_Flame = new Spell("Fel Flame");
    private readonly Spell Fire_and_Brimstone = new Spell("Fire and Brimstone");
    private readonly Spell Harvest_Life = new Spell("Harvest Life");
    private readonly Spell Immolate = new Spell("Immolate");
    private Timer Immolate_Timer = new Timer(0);
    private readonly Spell Incinerate = new Spell("Incinerate");
    private readonly Spell Rain_of_Fire = new Spell("Rain of Fire");
    private readonly Spell Shadow_Bolt = new Spell("Shadow Bolt");
    private readonly Spell Shadowburn = new Spell("Shadowburn");
    private readonly Spell Summon_Imp = new Spell("Summon Imp");
    private readonly Spell Summon_Voidwalker = new Spell("Summon Voidwalker");
    private readonly Spell Summon_Felhunter = new Spell("Summon Felhunter");
    private readonly Spell Summon_Succubus = new Spell("Summon Succubus");

    #endregion

    #region Offensive Cooldown

    private readonly Spell Archimondes_Vengeance = new Spell("Archimonde's Vengeance");
    private readonly Spell Dark_Soul = new Spell("Dark Soul");
    private readonly Spell Grimoire_of_Service = new Spell("Grimoire of Service");
    private readonly Spell Summon_Doomguard = new Spell("Summon Doomguard");
    private readonly Spell Summon_Infernal = new Spell("Summon Infernal");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Dark_Bargain = new Spell("Dark Bargain");
    private readonly Spell Howl_of_Terror = new Spell("Howl_of_Terror");
    private readonly Spell Sacrificial_Pact = new Spell("Sacrificial Pact");
    private readonly Spell Shadowfury = new Spell("Shadowfury");
    private readonly Spell Twilight_Ward = new Spell("Twilight Ward");
    private readonly Spell Unbound_Will = new Spell("Unbound Will");
    private readonly Spell Unending_Resolve = new Spell("Unending Resolve");

    #endregion

    #region Healing Spell

    private readonly Spell Create_Healthstone = new Spell("Create Healthstone");
    private Timer Healthstone_Timer = new Timer(0);
    private readonly Spell Dark_Regeneration = new Spell("Dark Regeneration");
    private readonly Spell Drain_Life = new Spell("Drain Life");
    private readonly Spell Ember_Tap = new Spell("Ember Tap");
    private readonly Spell Flames_of_Xoroth = new Spell("Flames of Xoroth");
    private readonly Spell Health_Funnel = new Spell("Health Funnel");
    private readonly Spell Life_Tap = new Spell("Life Tap");
    private readonly Spell Mortal_Coil = new Spell("Mortal Coil");

    #endregion

    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    public int LC = 0;

    public Warlock_Destruction()
    {
        Main.range = 30.0f;
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
                            (Curse_of_the_Elements.IsDistanceGood))
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
        if (Curse_of_the_Elements.KnownSpell && Curse_of_the_Elements.IsSpellUsable
            && Curse_of_the_Elements.IsDistanceGood && !Curse_of_the_Elements.TargetHaveBuff
            && MySettings.UseCurseoftheElements)
        {
            Curse_of_the_Elements.Launch();
            return;
        }
    }

    public void LowCombat()
    {
        AvoidMelee();
        Heal();
        Defense_Cycle();
        Buff();

        // Blizzard API Calls for Incinerate using Shadow Bolt Function
        if (Shadow_Bolt.KnownSpell && Shadow_Bolt.IsSpellUsable && Shadow_Bolt.IsDistanceGood
            && MySettings.UseIncinerate)
        {
            Shadow_Bolt.Launch();
            return;
        }

        if (ObjectManager.Target.HealthPercent < 50 && ObjectManager.Target.HealthPercent > 0)
        {
            if (Shadow_Bolt.KnownSpell && Shadow_Bolt.IsSpellUsable && Shadow_Bolt.IsDistanceGood
                && MySettings.UseIncinerate)
            {
                Shadow_Bolt.Launch();
                return;
            }
        }

        if (Rain_of_Fire.IsSpellUsable && Rain_of_Fire.KnownSpell && Rain_of_Fire.IsDistanceGood
            && MySettings.UseRainofFire)
        {
            SpellManager.CastSpellByIDAndPosition(5740, ObjectManager.Target.Position);
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
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
        else if (Dark_Soul.KnownSpell && Dark_Soul.IsSpellUsable
                 && MySettings.UseDarkSoul && ObjectManager.Target.GetDistance < 40)
        {
            Dark_Soul.Launch();
            return;
        }
        else if (Summon_Doomguard.KnownSpell && Summon_Doomguard.IsSpellUsable
                 && MySettings.UseSummonDoomguard && Summon_Doomguard.IsDistanceGood)
        {
            Summon_Doomguard.Launch();
            return;
        }
        else if (Summon_Infernal.KnownSpell && Summon_Infernal.IsSpellUsable
                 && MySettings.UseSummonInfernal && Summon_Infernal.IsDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(1122, ObjectManager.Target.Position);
            return;
        }
        else if (Archimondes_Vengeance.KnownSpell && Archimondes_Vengeance.IsSpellUsable
                 && MySettings.UseArchimondesVengeance)
        {
            Archimondes_Vengeance.Launch();
            return;
        }
        else
        {
            if (Grimoire_of_Service.KnownSpell && Grimoire_of_Service.IsSpellUsable
                && MySettings.UseGrimoireofService && ObjectManager.Target.GetDistance < 40)
            {
                Grimoire_of_Service.Launch();
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

    public void DPS_Cycle()
    {
        if (Curse_of_the_Elements.KnownSpell && Curse_of_the_Elements.IsSpellUsable && MySettings.UseCurseoftheElements
            && Curse_of_the_Elements.IsDistanceGood && !Curse_of_the_Elements.TargetHaveBuff)
        {
            Curse_of_the_Elements.Launch();
            return;
        }
        else if (Curse_of_Enfeeblement.KnownSpell && Curse_of_Enfeeblement.IsSpellUsable &&
                 MySettings.UseCurseofEnfeeblement
                 && Curse_of_Enfeeblement.IsDistanceGood && !Curse_of_Enfeeblement.TargetHaveBuff &&
                 !MySettings.UseCurseoftheElements)
        {
            Curse_of_Enfeeblement.Launch();
            return;
        }
            // Blizzard API Calls for Immolate using Corruption Function
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && Fire_and_Brimstone.IsSpellUsable &&
                 Fire_and_Brimstone.KnownSpell
                 && !ObjectManager.Target.HaveBuff(348) && Corruption.IsSpellUsable && Corruption.KnownSpell &&
                 Corruption.IsDistanceGood
                 && MySettings.UseFireandBrimstone && MySettings.UseImmolate)
        {
            Fire_and_Brimstone.Launch();
            Thread.Sleep(200);
            Corruption.Launch();
            Immolate_Timer = new Timer(1000*12);
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && ObjectManager.Target.HaveBuff(348)
                 && MySettings.UseHarvestLife && Harvest_Life.KnownSpell && Harvest_Life.IsSpellUsable
                 && Harvest_Life.IsDistanceGood)
        {
            Harvest_Life.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }
            // Blizzard API Calls for Incinerate using Shadow Bolt Function
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && Fire_and_Brimstone.IsSpellUsable &&
                 Fire_and_Brimstone.KnownSpell
                 && Shadow_Bolt.KnownSpell && Shadow_Bolt.IsSpellUsable && Shadow_Bolt.IsDistanceGood
                 && MySettings.UseFireandBrimstone && MySettings.UseIncinerate)
        {
            Fire_and_Brimstone.Launch();
            Thread.Sleep(200);
            Shadow_Bolt.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && Rain_of_Fire.IsSpellUsable &&
                 Rain_of_Fire.KnownSpell
                 && Rain_of_Fire.IsDistanceGood && MySettings.UseRainofFire)
        {
            SpellManager.CastSpellByIDAndPosition(5740, ObjectManager.Target.Position);
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }
        else if (Conflagrate.KnownSpell && Conflagrate.IsSpellUsable && Conflagrate.IsDistanceGood
                 && MySettings.UseConflagrate)
        {
            Conflagrate.Launch();
            return;
        }
        else
        {
            if (Corruption.IsSpellUsable && Corruption.KnownSpell && Corruption.IsDistanceGood
                && MySettings.UseImmolate && !ObjectManager.Target.HaveBuff(348) ||
                Immolate_Timer.IsReady)
            {
                Corruption.Launch();
                Immolate_Timer = new Timer(1000*12);
                return;
            }
        }

        if (ObjectManager.Target.HealthPercent < 20)
        {
            if (Shadowburn.KnownSpell && Shadowburn.IsSpellUsable && Shadowburn.IsDistanceGood
                && !ObjectManager.Me.HaveBuff(117828) && MySettings.UseShadowburn)
            {
                Shadowburn.Launch();
                return;
            }
        }
        else
        {
            if (Chaos_Bolt.KnownSpell && Chaos_Bolt.IsSpellUsable && Chaos_Bolt.IsDistanceGood
                && !ObjectManager.Me.HaveBuff(117828) && MySettings.UseChaosBolt)
            {
                Chaos_Bolt.Launch();
                return;
            }
        }

        if (Shadow_Bolt.KnownSpell && Shadow_Bolt.IsSpellUsable && Shadow_Bolt.IsDistanceGood
            && MySettings.UseIncinerate)
        {
            Shadow_Bolt.Launch();
            return;
        }
    }

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Heal();
            Buff();
        }
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        Pet();

        if (!Dark_Intent.HaveBuff && Dark_Intent.KnownSpell && Dark_Intent.IsSpellUsable
            && MySettings.UseDarkIntent)
        {
            Dark_Intent.Launch();
            return;
        }
        else if (!Soul_Link.HaveBuff && Soul_Link.KnownSpell && Soul_Link.IsSpellUsable
                 && MySettings.UseSoulLink && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0))
        {
            Soul_Link.Launch();
            return;
        }
        if (!Soulstone.HaveBuff && Soulstone.KnownSpell && Soulstone.IsSpellUsable
            && MySettings.UseSoulstone)
        {
            Soulstone.Launch();
            return;
        }
        else
        {
            if (ItemsManager.GetItemCountByIdLUA(5512) == 0 && Create_Healthstone.KnownSpell
                && Create_Healthstone.IsSpellUsable && MySettings.UseCreateHealthstone)
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
        }
    }

    private void Pet()
    {
        if (Health_Funnel.KnownSpell)
        {
            if (ObjectManager.Pet.HealthPercent > 0 && ObjectManager.Pet.HealthPercent < 50
                && Health_Funnel.IsSpellUsable && Health_Funnel.KnownSpell && MySettings.UseHealthFunnel)
            {
                Health_Funnel.Launch();
                while (ObjectManager.Me.IsCast)
                {
                    if (ObjectManager.Pet.HealthPercent > 85 || ObjectManager.Pet.IsDead)
                        break;
                    Thread.Sleep(100);
                }
            }
        }

        if (MySettings.UseFlamesofXoroth && Flames_of_Xoroth.KnownSpell && Flames_of_Xoroth.IsSpellUsable &&
            (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && !Grimoire_of_Sacrifice.HaveBuff)
        {
            Flames_of_Xoroth.Launch();
            Logging.WriteFight(" - PET DEAD - ");
        }
        else if (MySettings.UseSummonFelhunter && Summon_Felhunter.KnownSpell && Summon_Felhunter.IsSpellUsable &&
                 (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && !Grimoire_of_Sacrifice.HaveBuff)
        {
            Summon_Felhunter.Launch();
            Logging.WriteFight(" - PET DEAD - ");
        }
        else if (MySettings.UseSummonImp && Summon_Imp.KnownSpell && Summon_Imp.IsSpellUsable &&
                 (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && !Grimoire_of_Sacrifice.HaveBuff)
        {
            Summon_Imp.Launch();
            Logging.WriteFight(" - PET DEAD - ");
        }
        else if (MySettings.UseSummonVoidwalker && Summon_Voidwalker.KnownSpell &&
                 Summon_Voidwalker.IsSpellUsable &&
                 (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && !Grimoire_of_Sacrifice.HaveBuff)
        {
            Summon_Voidwalker.Launch();
            Logging.WriteFight(" - PET DEAD - ");
        }
        else if (MySettings.UseSummonSuccubus && Summon_Succubus.KnownSpell &&
                 Summon_Succubus.IsSpellUsable &&
                 (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && !Grimoire_of_Sacrifice.HaveBuff)
        {
            Summon_Succubus.Launch();
            Logging.WriteFight(" - PET DEAD - ");
        }
        Thread.Sleep(200);
        if (Grimoire_of_Sacrifice.KnownSpell && !Grimoire_of_Sacrifice.HaveBuff && Grimoire_of_Sacrifice.IsSpellUsable
            && MySettings.UseGrimoireofSacrifice && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0))
        {
            Grimoire_of_Sacrifice.Launch();
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
        else if (ObjectManager.Me.HealthPercent < 60 && Ember_Tap.IsSpellUsable && Ember_Tap.KnownSpell
                 && MySettings.UseEmberTap)
        {
            Ember_Tap.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 65 && Dark_Regeneration.IsSpellUsable && Dark_Regeneration.KnownSpell
                 && MySettings.UseDarkRegeneration)
        {
            Dark_Regeneration.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 75 && ItemsManager.GetItemCountByIdLUA(5512) > 0
                 && MySettings.UseCreateHealthstone && Healthstone_Timer.IsReady)
        {
            Logging.WriteFight("Use Healthstone.");
            nManager.Wow.Helpers.ItemsManager.UseItem("Healthstone");
            Healthstone_Timer = new Timer(1000*60*2);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 85 && Mortal_Coil.IsSpellUsable && Mortal_Coil.KnownSpell
                 && MySettings.UseMortalCoil && Mortal_Coil.IsDistanceGood)
        {
            Mortal_Coil.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 70 && Drain_Life.KnownSpell
                && MySettings.UseDrainLife && Drain_Life.IsDistanceGood && Drain_Life.IsSpellUsable)
            {
                Drain_Life.Launch();
                while (ObjectManager.Me.IsCast)
                {
                    Thread.Sleep(200);
                }
                return;
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 70 && MySettings.UseUnendingResolve
            && Unending_Resolve.KnownSpell && Unending_Resolve.IsSpellUsable)
        {
            Unending_Resolve.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 20 && MySettings.UseHowlofTerror
                 && Howl_of_Terror.KnownSpell && Howl_of_Terror.IsSpellUsable && ObjectManager.Target.GetDistance < 8)
        {
            Howl_of_Terror.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 40 && MySettings.UseDarkBargain
                 && Dark_Bargain.KnownSpell && Dark_Bargain.IsSpellUsable)
        {
            Dark_Bargain.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 95 && MySettings.UseSacrificialPact
                 && Sacrificial_Pact.KnownSpell && Sacrificial_Pact.IsSpellUsable
                 && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0))
        {
            Sacrificial_Pact.Launch();
            OnCD = new Timer(1000*10);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && MySettings.UseShadowfury
                 && Shadowfury.KnownSpell && Shadowfury.IsSpellUsable && ObjectManager.Target.GetDistance < 8)
        {
            Shadowfury.Launch();
            OnCD = new Timer(1000*3);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && War_Stomp.IsSpellUsable && War_Stomp.KnownSpell
                 && MySettings.UseWarStomp)
        {
            War_Stomp.Launch();
            OnCD = new Timer(1000*2);
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell
                && MySettings.UseStoneform)
            {
                Stoneform.Launch();
                OnCD = new Timer(1000*8);
                return;
            }
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ObjectManager.Target.GetDistance < 8
            && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable && MySettings.UseArcaneTorrent)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
                 && MySettings.UseTwilightWard && Twilight_Ward.KnownSpell && Twilight_Ward.IsSpellUsable)
        {
            Twilight_Ward.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseSummonFelhunter
                && Command_Demon.IsSpellUsable && Command_Demon.KnownSpell && ObjectManager.Target.GetDistance < 40)
            {
                Command_Demon.Launch();
                return;
            }
        }
    }

    private void AvoidMelee()
    {
        while (ObjectManager.Target.GetDistance < 3 && ObjectManager.Target.InCombat)
        {
            nManager.Wow.Helpers.Keybindings.PressKeybindings(Keybindings.MOVEBACKWARD);
        }
    }
}

public class Warlock_Affliction
{
    [Serializable]
    public class WarlockAfflictionSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Warlock Buffs */
        public bool UseCurseofEnfeeblement = false;
        public bool UseCurseofExhaustion = false;
        public bool UseCurseoftheElements = true;
        public bool UseDarkIntent = true;
        public bool UseGrimoireofSacrifice = true;
        public bool UseSoulLink = true;
        public bool UseSoulstone = true;
        /* Offensive Spell */
        public bool UseAgony = true;
        public bool UseCommandDemon = true;
        public bool UseCorruption = true;
        public bool UseDrainSoul = true;
        public bool UseFelFlame = true;
        public bool UseHarvestLife = true;
        public bool UseHaunt = true;
        public bool UseMaleficGrasp = true;
        public bool UseRainofFire = true;
        public bool UseSeedofCorruption = true;
        public bool UseShadowBolt = true;
        public bool UseSoulSwap = true;
        public bool UseSoulburn = true;
        public bool UseSummonImp = false;
        public bool UseSummonVoidwalker = false;
        public bool UseSummonFelhunter = true;
        public bool UseSummonSuccubus = false;
        public bool UseUnstableAffliction = true;
        /* Offensive Cooldown */
        public bool UseArchimondesVengeance = true;
        public bool UseDarkSoul = true;
        public bool UseGrimoireofService = true;
        public bool UseSummonDoomguard = true;
        public bool UseSummonInfernal = false;
        /* Defensive Cooldown */
        public bool UseDarkBargain = true;
        public bool UseHowlofTerror = true;
        public bool UseSacrificialPact = true;
        public bool UseShadowfury = true;
        public bool UseTwilightWard = true;
        public bool UseUnboundWill = true;
        public bool UseUnendingResolve = true;
        /* Healing Spell */
        public bool UseCreateHealthstone = true;
        public bool UseDarkRegeneration = true;
        public bool UseDrainLife = true;
        public bool UseHealthFunnel = true;
        public bool UseLifeTap = true;
        public bool UseMortalCoil = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;

        public WarlockAfflictionSettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Warlock Affliction Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Warlock Buffs */
            AddControlInWinForm("Use Curse of Enfeeblement", "UseCurseofEnfeeblement", "Warlock Buffs");
            AddControlInWinForm("Use Curse of Exhaustion", "UseCurseofExhaustion", "Warlock Buffs");
            AddControlInWinForm("Use Curse of the Elements", "UseCurseoftheElements", "Warlock Buffs");
            AddControlInWinForm("Use Dark Intent", "UseDarkIntent", "Warlock Buffs");
            AddControlInWinForm("Use Grimoire of Sacrifice", "UseGrimoireofSacrifice", "Warlock Buffs");
            AddControlInWinForm("Use Soul Link ", "UseSoulLink ", "Warlock Buffs");
            AddControlInWinForm("Use Soulstone", "UseSoulstone", "Warlock Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Agony", "UseAgony", "Offensive Spell");
            AddControlInWinForm("Use Command Demon", "UseCommandDemon", "Offensive Spell");
            AddControlInWinForm("Use Corruption", "UseCorruption", "Offensive Spell");
            AddControlInWinForm("Use Drain Soul", "UseDrainSoul", "Offensive Spell");
            AddControlInWinForm("Use Fel Flame", "UseFelFlame", "Offensive Spell");
            AddControlInWinForm("Use Harvest Life", "UseHarvestLife", "Offensive Spell");
            AddControlInWinForm("Use Haunt", "UseHaunt", "Offensive Spell");
            AddControlInWinForm("Use Malefic Grasp", "UseMaleficGrasp", "Offensive Spell");
            AddControlInWinForm("Use Rain of Fire", "UseRainofFire", "Offensive Spell");
            AddControlInWinForm("Use Seed of Corruption", "UseSeedofCorruption", "Offensive Spell");
            AddControlInWinForm("Use Shadow Bolt", "UseShadowBolt", "Offensive Spell");
            AddControlInWinForm("Use Soul Swap", "UseSoulSwap", "Offensive Spell");
            AddControlInWinForm("Use Soulburn", "UseSoulburn", "Offensive Spell");
            AddControlInWinForm("Use Summon Imp", "UseSummonImp", "Offensive Spell");
            AddControlInWinForm("Use Summon Voidwalker", "UseSummonVoidwalker", "Offensive Spell");
            AddControlInWinForm("Use Summon Felhunter", "UseSummonFelhunter", "Offensive Spell");
            AddControlInWinForm("Use Summon Succubus", "UseSummonSuccubus", "Offensive Spell");
            AddControlInWinForm("Use Unstable Affliction", "UseUnstableAffliction", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Archimonde's Vengeance", "UseArchimondesVengeance", "Offensive Cooldown");
            AddControlInWinForm("Use Dark Soul", "UseDarkSoul", "Offensive Cooldown");
            AddControlInWinForm("Use Grimoire of Service", "UseGrimoireofService", "Offensive Cooldown");
            AddControlInWinForm("Use Summon Doomguard", "UseSummonDoomguard", "Offensive Cooldown");
            AddControlInWinForm("Use Summon Infernal", "UseSummonInfernal", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Dark Bargain", "UseDarkBargain", "Defensive Cooldown");
            AddControlInWinForm("Use Howl of Terror", "UseHowlofTerror", "Defensive Cooldown");
            AddControlInWinForm("Use Sacrificial Pact", "UseSacrificialPact", "Defensive Cooldown");
            AddControlInWinForm("Use Shadowfury", "UseShadowfury", "Defensive Cooldown");
            AddControlInWinForm("Use Twilight Ward", "UseTwilightWard", "Defensive Cooldown");
            AddControlInWinForm("Use Unbound Will", "UseUnboundWill", "Defensive Cooldown");
            AddControlInWinForm("Use Unending Resolve", "UseUnendingResolve", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Create Healthstone", "UseCreateHealthstone", "Healing Spell");
            AddControlInWinForm("Use Dark Regeneration", "UseDarkRegeneration", "Healing Spell");
            AddControlInWinForm("Use Drain Life", "UseDrainLife", "Healing Spell");
            AddControlInWinForm("Use Health Funnel", "UseHealthFunnel", "Healing Spell");
            AddControlInWinForm("Use Life Tap", "UseLifeTap", "Healing Spell");
            AddControlInWinForm("Use Mortal Coil", "UseMortalCoil", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static WarlockAfflictionSettings CurrentSetting { get; set; }

        public static WarlockAfflictionSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Warlock_Affliction.xml";
            if (File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Warlock_Affliction.WarlockAfflictionSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Warlock_Affliction.WarlockAfflictionSettings();
            }
        }
    }

    private readonly WarlockAfflictionSettings MySettings = WarlockAfflictionSettings.GetSettings();

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

    #region Warlock Buffs

    private readonly Spell Curse_of_Enfeeblement = new Spell("Curse of Enfeeblement");
    private readonly Spell Curse_of_Exhaustion = new Spell("Curse of Exhaustion");
    private readonly Spell Curse_of_the_Elements = new Spell("Curse of the Elements");
    private readonly Spell Dark_Intent = new Spell("Dark Intent");
    private readonly Spell Grimoire_of_Sacrifice = new Spell("Grimoire of Sacrifice");
    private readonly Spell Soul_Link = new Spell("Soul Link");
    private readonly Spell Soulstone = new Spell("Soulstone");

    #endregion

    #region Offensive Spell

    private readonly Spell Agony = new Spell("Agony");
    private Timer Agony_Timer = new Timer(0);
    private readonly Spell Command_Demon = new Spell("Command Demon");
    private readonly Spell Corruption = new Spell("Corruption");
    private Timer Corruption_Timer = new Timer(0);
    private readonly Spell Drain_Soul = new Spell("Drain Soul");
    private readonly Spell Fel_Flame = new Spell("Fel Flame");
    private readonly Spell Harvest_Life = new Spell("Harvest Life");
    private readonly Spell Haunt = new Spell("Haunt");
    private readonly Spell Malefic_Grasp = new Spell("Malefic Grasp");
    private readonly Spell Rain_of_Fire = new Spell("Rain of Fire");
    private readonly Spell Seed_of_Corruption = new Spell("Seed of Corruption");
    private readonly Spell Shadow_Bolt = new Spell("Shadow Bolt");
    private readonly Spell Soul_Swap = new Spell("Soul Swap");
    private readonly Spell Soulburn = new Spell("Soulburn");
    private readonly Spell Summon_Imp = new Spell("Summon Imp");
    private readonly Spell Summon_Voidwalker = new Spell("Summon Voidwalker");
    private readonly Spell Summon_Felhunter = new Spell("Summon Felhunter");
    private readonly Spell Summon_Succubus = new Spell("Summon Succubus");
    private readonly Spell Summon_Felguard = new Spell("Summon Felguard");
    private readonly Spell Unstable_Affliction = new Spell("Unstable Affliction");
    private Timer Unstable_Affliction_Timer = new Timer(0);

    #endregion

    #region Offensive Cooldown

    private readonly Spell Archimondes_Vengeance = new Spell("Archimonde's Vengeance");
    private readonly Spell Dark_Soul = new Spell("Dark Soul");
    private readonly Spell Grimoire_of_Service = new Spell("Grimoire of Service");
    private readonly Spell Summon_Doomguard = new Spell("Summon Doomguard");
    private readonly Spell Summon_Infernal = new Spell("Summon Infernal");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Dark_Bargain = new Spell("Dark Bargain");
    private readonly Spell Howl_of_Terror = new Spell("Howl_of_Terror");
    private readonly Spell Sacrificial_Pact = new Spell("Sacrificial Pact");
    private readonly Spell Shadowfury = new Spell("Shadowfury");
    private readonly Spell Twilight_Ward = new Spell("Twilight Ward");
    private readonly Spell Unbound_Will = new Spell("Unbound Will");
    private readonly Spell Unending_Resolve = new Spell("Unending Resolve");

    #endregion

    #region Healing Spell

    private readonly Spell Create_Healthstone = new Spell("Create Healthstone");
    private Timer Healthstone_Timer = new Timer(0);
    private readonly Spell Dark_Regeneration = new Spell("Dark Regeneration");
    private readonly Spell Drain_Life = new Spell("Drain Life");
    private readonly Spell Health_Funnel = new Spell("Health Funnel");
    private readonly Spell Life_Tap = new Spell("Life Tap");
    private readonly Spell Mortal_Coil = new Spell("Mortal Coil");

    #endregion

    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    public int LC = 0;

    public Warlock_Affliction()
    {
        Main.range = 30.0f;
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
                            (Soul_Swap.IsDistanceGood || Agony.IsDistanceGood))
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
        if (!Agony.TargetHaveBuff && !Corruption.TargetHaveBuff && !Unstable_Affliction.TargetHaveBuff)
        {
            if (Soulburn.IsSpellUsable && Soulburn.KnownSpell && Soul_Swap.IsSpellUsable && Soul_Swap.KnownSpell
                && Soul_Swap.IsDistanceGood && MySettings.UseSoulSwap && MySettings.UseSoulburn)
            {
                if (!Soulburn.HaveBuff)
                {
                    Soulburn.Launch();
                    Thread.Sleep(200);
                }
                Soul_Swap.Launch();
                Agony_Timer = new Timer(1000*20);
                Corruption_Timer = new Timer(1000*15);
                Unstable_Affliction_Timer = new Timer(1000*10);
            }
        }
        return;
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

    public void LowCombat()
    {
        AvoidMelee();
        Heal();
        Defense_Cycle();
        Buff();

        if (ObjectManager.Me.BarTwoPercentage < 75 && Life_Tap.KnownSpell && Life_Tap.IsSpellUsable
            && MySettings.UseLifeTap)
        {
            Life_Tap.Launch();
            return;
        }

        if (Malefic_Grasp.KnownSpell && Malefic_Grasp.IsSpellUsable && Malefic_Grasp.IsDistanceGood
            && MySettings.UseMaleficGrasp)
        {
            Malefic_Grasp.Launch();
            Thread.Sleep(200);
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
        }

        if (ObjectManager.Target.HealthPercent < 50 && ObjectManager.Target.HealthPercent > 0)
        {
            if (Malefic_Grasp.KnownSpell && Malefic_Grasp.IsSpellUsable && Malefic_Grasp.IsDistanceGood
                && MySettings.UseMaleficGrasp)
            {
                Malefic_Grasp.Launch();
                Thread.Sleep(200);
                while (ObjectManager.Me.IsCast)
                {
                    Thread.Sleep(200);
                }
            }
        }

        if (ObjectManager.Target.HealthPercent > 90)
        {
            if (Rain_of_Fire.IsSpellUsable && Rain_of_Fire.KnownSpell && Rain_of_Fire.IsDistanceGood
                && MySettings.UseRainofFire)
            {
                SpellManager.CastSpellByIDAndPosition(5740, ObjectManager.Target.Position);
                while (ObjectManager.Me.IsCast)
                {
                    Thread.Sleep(200);
                }
                return;
            }
        }
        return;
    }

    public void DPS_Burst()
    {
        if (MySettings.UseTrinket && Trinket_Timer.IsReady && ObjectManager.Target.GetDistance < 40)
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
                 && ObjectManager.Target.GetDistance < 40)
        {
            Berserking.Launch();
            return;
        }
        else if (Blood_Fury.IsSpellUsable && Blood_Fury.KnownSpell && MySettings.UseBloodFury
                 && ObjectManager.Target.GetDistance < 40)
        {
            Blood_Fury.Launch();
            return;
        }
        else if (Lifeblood.IsSpellUsable && Lifeblood.KnownSpell && MySettings.UseLifeblood
                 && ObjectManager.Target.GetDistance < 40)
        {
            Lifeblood.Launch();
            return;
        }
        else if (MySettings.UseEngGlove && Engineering.KnownSpell && Engineering_Timer.IsReady
                 && ObjectManager.Target.GetDistance < 40)
        {
            Logging.WriteFight("Use Engineering Gloves.");
            Lua.RunMacroText("/use 10");
            Engineering_Timer = new Timer(1000*60);
            return;
        }
        else if (Dark_Soul.KnownSpell && Dark_Soul.IsSpellUsable
                 && MySettings.UseDarkSoul && ObjectManager.Target.GetDistance < 40)
        {
            Dark_Soul.Launch();
            return;
        }
        else if (Summon_Doomguard.KnownSpell && Summon_Doomguard.IsSpellUsable
                 && MySettings.UseSummonDoomguard && Summon_Doomguard.IsDistanceGood)
        {
            Summon_Doomguard.Launch();
            return;
        }
        else if (Summon_Infernal.KnownSpell && Summon_Infernal.IsSpellUsable
                 && MySettings.UseSummonInfernal && Summon_Infernal.IsDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(1122, ObjectManager.Target.Position);
            return;
        }
        else if (Archimondes_Vengeance.KnownSpell && Archimondes_Vengeance.IsSpellUsable
                 && MySettings.UseArchimondesVengeance)
        {
            Archimondes_Vengeance.Launch();
            return;
        }
        else
        {
            if (Grimoire_of_Service.KnownSpell && Grimoire_of_Service.IsSpellUsable
                && MySettings.UseGrimoireofService && ObjectManager.Target.GetDistance < 40)
            {
                Grimoire_of_Service.Launch();
                return;
            }
        }
    }

    public void DPS_Cycle()
    {
        if (Curse_of_the_Elements.KnownSpell && Curse_of_the_Elements.IsSpellUsable && MySettings.UseCurseoftheElements
            && Curse_of_the_Elements.IsDistanceGood && !Curse_of_the_Elements.TargetHaveBuff)
        {
            Curse_of_the_Elements.Launch();
            return;
        }
        else if (Curse_of_Enfeeblement.KnownSpell && Curse_of_Enfeeblement.IsSpellUsable &&
                 MySettings.UseCurseofEnfeeblement
                 && Curse_of_Enfeeblement.IsDistanceGood && !Curse_of_Enfeeblement.TargetHaveBuff &&
                 !MySettings.UseCurseoftheElements)
        {
            Curse_of_Enfeeblement.Launch();
            return;
        }
        else if (Curse_of_Exhaustion.KnownSpell && Curse_of_Exhaustion.IsSpellUsable &&
                 MySettings.UseCurseofExhaustion
                 && Curse_of_Exhaustion.IsDistanceGood && !Curse_of_Exhaustion.TargetHaveBuff &&
                 !MySettings.UseCurseoftheElements
                 && !MySettings.UseCurseofEnfeeblement)
        {
            Curse_of_Exhaustion.Launch();
            return;
        }
        else if (ObjectManager.Me.BarTwoPercentage < 75 && Life_Tap.KnownSpell && Life_Tap.IsSpellUsable
                 && MySettings.UseLifeTap)
        {
            Life_Tap.Launch();
            return;
        }
        else if (ObjectManager.Target.HealthPercent < 20)
        {
            if (Drain_Soul.KnownSpell && Drain_Soul.IsSpellUsable && MySettings.UseDrainSoul &&
                Drain_Soul.IsDistanceGood)
            {
                Drain_Soul.Launch();
                while (ObjectManager.Me.IsCast && !Agony_Timer.IsReady && !Corruption_Timer.IsReady
                       && !Unstable_Affliction_Timer.IsReady)
                {
                    Thread.Sleep(200);
                }
            }

            if (Agony_Timer.IsReady || Corruption_Timer.IsReady || Unstable_Affliction_Timer.IsReady)
            {
                if (Soulburn.IsSpellUsable && Soulburn.KnownSpell && Soul_Swap.IsSpellUsable &&
                    Soul_Swap.KnownSpell
                    && Soul_Swap.IsDistanceGood && MySettings.UseSoulburn && MySettings.UseSoulSwap)
                {
                    Soulburn.Launch();
                    Thread.Sleep(200);
                    Soul_Swap.Launch();
                    Agony_Timer = new Timer(1000*20);
                    Corruption_Timer = new Timer(1000*15);
                    Unstable_Affliction_Timer = new Timer(1000*10);
                }
            }
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && Soulburn.IsSpellUsable && Soulburn.KnownSpell &&
                 !Corruption.TargetHaveBuff
                 && Seed_of_Corruption.IsSpellUsable && Seed_of_Corruption.KnownSpell &&
                 Seed_of_Corruption.IsDistanceGood
                 && MySettings.UseSoulburn && MySettings.UseSeedofCorruption)
        {
            Soulburn.Launch();
            Thread.Sleep(200);
            Seed_of_Corruption.Launch();
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && Harvest_Life.IsSpellUsable &&
                 Harvest_Life.KnownSpell
                 && Harvest_Life.IsDistanceGood && MySettings.UseHarvestLife)
        {
            Harvest_Life.Launch();
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }
        else if (ObjectManager.GetNumberAttackPlayer() > 4 && Rain_of_Fire.IsSpellUsable &&
                 Rain_of_Fire.KnownSpell
                 && Rain_of_Fire.IsDistanceGood && MySettings.UseRainofFire)
        {
            SpellManager.CastSpellByIDAndPosition(5740, ObjectManager.Target.Position);
            while (ObjectManager.Me.IsCast)
            {
                Thread.Sleep(200);
            }
            return;
        }
        else if (Agony.KnownSpell && Agony.IsSpellUsable && Agony.IsDistanceGood &&
                 MySettings.UseAgony
                 && (!Agony.TargetHaveBuff || Agony_Timer.IsReady))
        {
            Agony.Launch();
            Agony_Timer = new Timer(1000*20);
            return;
        }
        else if (Corruption.KnownSpell && Corruption.IsSpellUsable && Corruption.IsDistanceGood
                 && MySettings.UseCorruption &&
                 (!Corruption.TargetHaveBuff || Corruption_Timer.IsReady))
        {
            Corruption.Launch();
            Corruption_Timer = new Timer(1000*15);
            return;
        }
        else if (Unstable_Affliction.KnownSpell && Unstable_Affliction.IsSpellUsable &&
                 Unstable_Affliction.IsDistanceGood
                 && MySettings.UseUnstableAffliction &&
                 (!Unstable_Affliction.TargetHaveBuff || Unstable_Affliction_Timer.IsReady))
        {
            Unstable_Affliction.Launch();
            Unstable_Affliction_Timer = new Timer(1000*10);
            return;
        }
        else if (Haunt.KnownSpell && Haunt.IsSpellUsable && Haunt.IsDistanceGood &&
                 !Haunt.TargetHaveBuff
                 && MySettings.UseHaunt)
        {
            Haunt.Launch();
            return;
        }
            // Blizzard API Calls for Malefic Grasp using Shadow Bolt Function
        else
        {
            if (!ObjectManager.Me.IsCast && Shadow_Bolt.KnownSpell &&
                Shadow_Bolt.IsSpellUsable
                && !Agony_Timer.IsReady && !Corruption_Timer.IsReady &&
                !Unstable_Affliction_Timer.IsReady
                && Shadow_Bolt.IsDistanceGood && MySettings.UseMaleficGrasp)
            {
                Shadow_Bolt.Launch();
                return;
            }
        }
    }

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Heal();
            Buff();
        }
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        Pet();

        if (!Dark_Intent.HaveBuff && Dark_Intent.KnownSpell && Dark_Intent.IsSpellUsable
            && MySettings.UseDarkIntent)
        {
            Dark_Intent.Launch();
            return;
        }
        else if (!Soul_Link.HaveBuff && Soul_Link.KnownSpell && Soul_Link.IsSpellUsable
                 && MySettings.UseSoulLink && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0))
        {
            Soul_Link.Launch();
            return;
        }
        if (!Soulstone.HaveBuff && Soulstone.KnownSpell && Soulstone.IsSpellUsable
            && MySettings.UseSoulstone)
        {
            Soulstone.Launch();
            return;
        }
        else
        {
            if (ItemsManager.GetItemCountByIdLUA(5512) == 0 && Create_Healthstone.KnownSpell
                && Create_Healthstone.IsSpellUsable && MySettings.UseCreateHealthstone)
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
        }
    }

    private void Pet()
    {
        if (Health_Funnel.KnownSpell)
        {
            if (ObjectManager.Pet.HealthPercent > 0 && ObjectManager.Pet.HealthPercent < 50
                && Health_Funnel.IsSpellUsable && Health_Funnel.KnownSpell && MySettings.UseHealthFunnel)
            {
                Health_Funnel.Launch();
                while (ObjectManager.Me.IsCast)
                {
                    if (ObjectManager.Pet.HealthPercent > 85 || ObjectManager.Pet.IsDead)
                        break;
                    Thread.Sleep(100);
                }
            }
        }

        if (MySettings.UseSummonFelhunter && Summon_Felhunter.KnownSpell && Summon_Felhunter.IsSpellUsable &&
            (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && !Grimoire_of_Sacrifice.HaveBuff)
        {
            Summon_Felhunter.Launch();
            Logging.WriteFight(" - PET DEAD - ");
        }
        else if (MySettings.UseSummonImp && Summon_Imp.KnownSpell && Summon_Imp.IsSpellUsable &&
                 (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && !Grimoire_of_Sacrifice.HaveBuff)
        {
            Summon_Imp.Launch();
            Logging.WriteFight(" - PET DEAD - ");
        }
        else if (MySettings.UseSummonVoidwalker && Summon_Voidwalker.KnownSpell &&
                 Summon_Voidwalker.IsSpellUsable &&
                 (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && !Grimoire_of_Sacrifice.HaveBuff)
        {
            Summon_Voidwalker.Launch();
            Logging.WriteFight(" - PET DEAD - ");
        }
        else if (MySettings.UseSummonSuccubus && Summon_Succubus.KnownSpell &&
                 Summon_Succubus.IsSpellUsable &&
                 (ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) && !Grimoire_of_Sacrifice.HaveBuff)
        {
            Summon_Succubus.Launch();
            Logging.WriteFight(" - PET DEAD - ");
        }
        Thread.Sleep(200);
        if (Grimoire_of_Sacrifice.KnownSpell && !Grimoire_of_Sacrifice.HaveBuff && Grimoire_of_Sacrifice.IsSpellUsable
            && MySettings.UseGrimoireofSacrifice && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0))
        {
            Grimoire_of_Sacrifice.Launch();
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
        else if (ObjectManager.Me.HealthPercent < 65 && Dark_Regeneration.IsSpellUsable && Dark_Regeneration.KnownSpell
                 && MySettings.UseDarkRegeneration)
        {
            Dark_Regeneration.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 75 && ItemsManager.GetItemCountByIdLUA(5512) > 0
                 && MySettings.UseCreateHealthstone && Healthstone_Timer.IsReady)
        {
            Logging.WriteFight("Use Healthstone.");
            nManager.Wow.Helpers.ItemsManager.UseItem("Healthstone");
            Healthstone_Timer = new Timer(1000*60*2);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 85 && Mortal_Coil.IsSpellUsable && Mortal_Coil.KnownSpell
                 && MySettings.UseMortalCoil && Mortal_Coil.IsDistanceGood)
        {
            Mortal_Coil.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 70 && Drain_Life.KnownSpell
                && MySettings.UseDrainLife && Drain_Life.IsDistanceGood && Drain_Life.IsSpellUsable)
            {
                Drain_Life.Launch();
                while (ObjectManager.Me.IsCast)
                {
                    Thread.Sleep(200);
                }
                return;
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 70 && MySettings.UseUnendingResolve
            && Unending_Resolve.KnownSpell && Unending_Resolve.IsSpellUsable)
        {
            Unending_Resolve.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 20 && MySettings.UseHowlofTerror
                 && Howl_of_Terror.KnownSpell && Howl_of_Terror.IsSpellUsable && ObjectManager.Target.GetDistance < 8)
        {
            Howl_of_Terror.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 40 && MySettings.UseDarkBargain
                 && Dark_Bargain.KnownSpell && Dark_Bargain.IsSpellUsable)
        {
            Dark_Bargain.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 95 && MySettings.UseSacrificialPact
                 && Sacrificial_Pact.KnownSpell && Sacrificial_Pact.IsSpellUsable
                 && (ObjectManager.Pet.Health != 0 || ObjectManager.Pet.Guid != 0))
        {
            Sacrificial_Pact.Launch();
            OnCD = new Timer(1000*10);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && MySettings.UseShadowfury
                 && Shadowfury.KnownSpell && Shadowfury.IsSpellUsable && ObjectManager.Target.GetDistance < 8)
        {
            Shadowfury.Launch();
            OnCD = new Timer(1000*3);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && War_Stomp.IsSpellUsable && War_Stomp.KnownSpell
                 && MySettings.UseWarStomp)
        {
            War_Stomp.Launch();
            OnCD = new Timer(1000*2);
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell
                && MySettings.UseStoneform)
            {
                Stoneform.Launch();
                OnCD = new Timer(1000*8);
                return;
            }
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ObjectManager.Target.GetDistance < 8
            && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable && MySettings.UseArcaneTorrent)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
                 && MySettings.UseTwilightWard && Twilight_Ward.KnownSpell && Twilight_Ward.IsSpellUsable)
        {
            Twilight_Ward.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && MySettings.UseSummonFelhunter
                && Command_Demon.IsSpellUsable && Command_Demon.KnownSpell && ObjectManager.Target.GetDistance < 40)
            {
                Command_Demon.Launch();
                return;
            }
        }
    }

    private void AvoidMelee()
    {
        while (ObjectManager.Target.GetDistance < 3 && ObjectManager.Target.InCombat)
        {
            nManager.Wow.Helpers.Keybindings.PressKeybindings(Keybindings.MOVEBACKWARD);
        }
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
    public class PaladinHolySettings : Settings
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
            if (File.Exists(CurrentSettingsFile))
            {
                return CurrentSetting = Load<PaladinHolySettings>(CurrentSettingsFile);
            }
            return new PaladinHolySettings();
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
                    if (Fight.InFight && ObjectManager.Me.Target > 0)
                    {
                        if (ObjectManager.Me.Target != lastTarget && HolyShock.IsDistanceGood)
                        {
                            Pull();
                            lastTarget = ObjectManager.Me.Target;
                        }
                        Combat();
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
        if (HolyShock.KnownSpell && HolyShock.IsDistanceGood && HolyShock.IsSpellUsable && MySettings.UseHolyShock)
        {
            HolyShock.Launch();
            return;
        }
    }

    private void Combat()
    {
        DPS_Cycle();

        DPS_Burst();

        DPS_Cycle();

        Heal();

        DPS_Cycle();

        Buffs();
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Blessing();
            Heal();
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
        if (SealOfInsight.KnownSpell && MySettings.UseSealOfInsight)
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

        if (BlessingOfKings.KnownSpell && MySettings.UseBlessingOfKings)
        {
            if (!BlessingOfKings.HaveBuff && BlessingOfKings.IsSpellUsable)
                BlessingOfKings.Launch();
        }
        else if (BlessingOfMight.KnownSpell && MySettings.UseBlessingOfMight)
        {
            if (!BlessingOfMight.HaveBuff && BlessingOfMight.IsSpellUsable)
                BlessingOfMight.Launch();
        }
        if (BeaconOfLight.KnownSpell && MySettings.UseBeaconOfLight)
        {
            if (!BeaconOfLight.HaveBuff && BeaconOfLight.IsSpellUsable)
                BeaconOfLight.Launch();
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 95 && !Fight.InFight && ObjectManager.GetNumberAttackPlayer() == 0)
        {
            if (DivineLight.KnownSpell && DivineLight.IsSpellUsable && MySettings.UseDivineLight)
            {
                DivineLight.Launch(true, true, true);
                return;
            }
            if (FlashOfLight.KnownSpell && FlashOfLight.IsSpellUsable && MySettings.UseFlashOfLight)
            {
                FlashOfLight.Launch(true, true, true);
                return;
            }
            if (HolyLight.KnownSpell && HolyLight.IsSpellUsable && MySettings.UseHolyLight)
            {
                HolyLight.Launch(true, true, true);
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
            if (File.Exists(CurrentSettingsFile))
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

        DPS_Burst();

        DPS_Cycle();

        Heal();

        DPS_Cycle();

        Buffs();
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Blessing();
            Heal();
        }
        Seal();
    }

    private void Buffs()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Blessing();
        }
        Seal();
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

        if (BlessingOfMight.KnownSpell && MySettings.UseBlessingOfMight)
        {
            if (!BlessingOfMight.HaveBuff && BlessingOfMight.IsSpellUsable)
                BlessingOfMight.Launch();
        }
        else if (BlessingOfKings.KnownSpell && MySettings.UseBlessingOfKings)
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
                FlashOfLight.Launch(true, true, true);
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
    public class PaladinRetributionSettings : Settings
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
        public bool RefreshWeakenedBlows = true;
        public bool UseDivineProtection = true;
        public bool UseDevotionAura = true;
        public bool UseSacredShield = true;
        public bool UseDivineShield = true;
        public bool UseHandOfProtection = false;
        /* Healing Spell */
        public bool UseFlashOfLight = true;
        public bool UseLayOnHands = true;
        public bool UseWordOfGlory = true;
        /* Flask & Potion management */
        public bool UseFlaskOrBattleElixir = false;
        public string FlaskOrBattleElixir = "Flask of Winter's Bite";
        public bool UseGuardianElixir = false;
        public string GuardianElixir = "";
        public bool UseCombatPotion = false;
        public string CombatPotion = "Potion of Mogu Power";
        public bool UseTeasureFindingPotion = false;
        public string TeasureFindingPotion = "Potion of Luck";
        public bool UseWellFedBuff = false;
        public string WellFedBuff = "Black Pepper Ribs and Shrimp";

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
            AddControlInWinForm("Refresh Weakened Blows", "RefreshWeakenedBlows", "Defensive Cooldown");
            AddControlInWinForm("Use Divine Protection", "UseDivineProtection", "Defensive Cooldown");
            AddControlInWinForm("Use Devotion Aura", "UseDevotionAura", "Defensive Cooldown");
            AddControlInWinForm("Use Sacred Shield", "UseSacredShield", "Defensive Cooldown");
            AddControlInWinForm("Use Divine Shield", "UseDivineShield", "Defensive Cooldown");
            AddControlInWinForm("Use Hand of Protection", "UseHandOfProtection", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Flash of Light", "UseFlashOfLight", "Healing Spell");
            AddControlInWinForm("Use Lay on Hands", "UseLayOnHands", "Healing Spell");
            AddControlInWinForm("Use Word of Glory", "UseWordOfGlory", "Healing Spell");
            /* Flask & Potion Management */
            AddControlInWinForm("Use Flask or Battle Elixir", "UseFlaskOrBattleElixir", "Flask & Potion Management");
            AddControlInWinForm("Flask or Battle Elixir Name", "FlaskOrBattleElixir", "Flask & Potion Management");
            AddControlInWinForm("Use Guardian Elixir", "UseGuardianElixir", "Flask & Potion Management");
            AddControlInWinForm("Guardian Elixir Name", "GuardianElixir", "Flask & Potion Management");
            AddControlInWinForm("Use Combat Potion", "UseCombatPotion", "Flask & Potion Management");
            AddControlInWinForm("Combat Potion Name", "CombatPotion", "Flask & Potion Management");
            AddControlInWinForm("Use Teasure Finding Potion", "UseTeasureFindingPotion", "Flask & Potion Management");
            AddControlInWinForm("Teasure Finding Potion Name", "TeasureFindingPotion", "Flask & Potion Management");
            AddControlInWinForm("Use Well Fed Buff", "UseWellFedBuff", "Flask & Potion Management");
            AddControlInWinForm("Well Fed Buff Name", "WellFedBuff", "Flask & Potion Management");
        }

        public static PaladinRetributionSettings CurrentSetting { get; set; }

        public static PaladinRetributionSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Paladin_Retribution.xml";
            if (File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Load<PaladinRetributionSettings>(CurrentSettingsFile);
            }
            return new PaladinRetributionSettings();
        }
    }

    private static readonly PaladinRetributionSettings MySettings = PaladinRetributionSettings.GetSettings();

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

    #region Flask & Potion Management

    private readonly uint FlaskOrBattleElixir = (uint) ItemsManager.GetIdByName(MySettings.FlaskOrBattleElixir);
    private readonly uint GuardianElixir = (uint) ItemsManager.GetIdByName(MySettings.GuardianElixir);
    private readonly uint CombatPotion = (uint) ItemsManager.GetIdByName(MySettings.CombatPotion);
    private readonly uint TeasureFindingPotion = (uint) ItemsManager.GetIdByName(MySettings.TeasureFindingPotion);
    private readonly uint WellFedBuff = (uint) ItemsManager.GetIdByName(MySettings.WellFedBuff);

    private readonly WoWItem Hands = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_HAND);
    private readonly WoWItem FirstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    private readonly WoWItem SecondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

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

        Buffs();
    }

    private void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            if (MySettings.UseFlaskOrBattleElixir && MySettings.FlaskOrBattleElixir != string.Empty)
                if (!SpellManager.HaveBuffLua(ItemsManager.GetItemSpellByItemName(MySettings.FlaskOrBattleElixir)) &&
                    !ItemsManager.IsItemOnCooldown(FlaskOrBattleElixir) &&
                    ItemsManager.IsUsableItemById(FlaskOrBattleElixir))
                    ItemsManager.UseItem(MySettings.FlaskOrBattleElixir);
            if (MySettings.UseGuardianElixir && MySettings.GuardianElixir != string.Empty)
                if (!SpellManager.HaveBuffLua(ItemsManager.GetItemSpellByItemName(MySettings.GuardianElixir)) &&
                    !ItemsManager.IsItemOnCooldown(GuardianElixir) && ItemsManager.IsUsableItemById(GuardianElixir))
                    ItemsManager.UseItem(MySettings.GuardianElixir);
            Blessing();
            Heal();
        }
        Seal();
    }

    private void Buffs()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            if (MySettings.UseFlaskOrBattleElixir && MySettings.FlaskOrBattleElixir != string.Empty)
                if (!SpellManager.HaveBuffLua(ItemsManager.GetItemSpellByItemName(MySettings.FlaskOrBattleElixir)) &&
                    !ItemsManager.IsItemOnCooldown(FlaskOrBattleElixir) &&
                    ItemsManager.IsUsableItemById(FlaskOrBattleElixir))
                    ItemsManager.UseItem(MySettings.FlaskOrBattleElixir);
            if (MySettings.UseGuardianElixir && MySettings.GuardianElixir != string.Empty)
                if (!SpellManager.HaveBuffLua(ItemsManager.GetItemSpellByItemName(MySettings.GuardianElixir)) &&
                    !ItemsManager.IsItemOnCooldown(GuardianElixir) && ItemsManager.IsUsableItemById(GuardianElixir))
                    ItemsManager.UseItem(MySettings.GuardianElixir);
            Blessing();
        }
        Seal();
    }

    private void Seal()
    {
        if (SealOfTruth.KnownSpell &&
            (ObjectManager.GetNumberAttackPlayer() <= 7 || !MySettings.UseSealOfTheRighteousness) &&
            MySettings.UseSealOfTruth)
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

        if (BlessingOfKings.KnownSpell && MySettings.UseBlessingOfKings)
        {
            if (!BlessingOfKings.HaveBuff && BlessingOfKings.IsSpellUsable)
                BlessingOfKings.Launch();
        }
        else if (BlessingOfMight.KnownSpell && MySettings.UseBlessingOfMight)
        {
            if (!BlessingOfMight.HaveBuff && BlessingOfMight.IsSpellUsable)
                BlessingOfMight.Launch();
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 95 && !Fight.InFight && ObjectManager.GetNumberAttackPlayer() == 0)
        {
            if (FlashOfLight.KnownSpell && FlashOfLight.IsSpellUsable && MySettings.UseFlashOfLight)
            {
                FlashOfLight.Launch(true, true, true);
                return;
            }
        }
        if (DivineShield.KnownSpell && ObjectManager.Me.HealthPercent > 0 && ObjectManager.Me.HealthPercent <= 5 &&
            DivineShield.IsSpellUsable && MySettings.UseDivineShield)
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
            if (DevotionAura.KnownSpell && DevotionAura.IsSpellUsable && MySettings.UseDevotionAura)
                DevotionAura.Launch();
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
            else if (HandOfProtection.KnownSpell && HandOfProtection.IsSpellUsable && !ObjectManager.Me.HaveBuff(25771) &&
                     MySettings.UseHandOfProtection)
                HandOfProtection.Launch();
            if (FlashOfLight.KnownSpell && FlashOfLight.IsSpellUsable && MySettings.UseFlashOfLight)
            {
                FlashOfLight.Launch();
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
            }
        }
        else if (GuardianOfAncientKings.KnownSpell && GuardianOfAncientKings.IsSpellUsable &&
                 MySettings.UseGuardianOfAncientKings && AvengingWrath.IsSpellUsable &&
                 (!HolyAvenger.KnownSpell || HolyAvenger.IsSpellUsable))
        {
            GuardianOfAncientKings.Launch();
            BurstTime = new Timer(1000*6.5);
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
        }
        else if ((ObjectManager.GetNumberAttackPlayer() <= 1 ||
                  (!MySettings.UseDivineStorm && MySettings.UseTemplarsVerdict)) && TemplarsVerdict.KnownSpell &&
                 (!Inquisition.KnownSpell || Inquisition.HaveBuff) && TemplarsVerdict.IsSpellUsable &&
                 TemplarsVerdict.IsDistanceGood &&
                 (ObjectManager.Me.HaveBuff(90174) || ObjectManager.Me.HolyPower == 5 ||
                  (ObjectManager.Me.HolyPower >= 3 && (!BoundlessConviction.KnownSpell || HolyAvenger.HaveBuff))))
        {
            TemplarsVerdict.Launch();
        }
        else if ((ObjectManager.GetNumberAttackPlayer() >= 2 ||
                  (MySettings.UseDivineStorm && !MySettings.UseTemplarsVerdict)) && DivineStorm.KnownSpell &&
                 MySettings.UseDivineStorm && (!Inquisition.KnownSpell || Inquisition.HaveBuff) &&
                 DivineStorm.IsSpellUsable && DivineStorm.IsDistanceGood &&
                 (ObjectManager.Me.HaveBuff(90174) || ObjectManager.Me.HolyPower == 5 ||
                  (ObjectManager.Me.HolyPower >= 3 && (!BoundlessConviction.KnownSpell || HolyAvenger.HaveBuff))))
        {
            DivineStorm.Launch();
        }
        else if (HammerOfWrath.KnownSpell && HammerOfWrath.IsDistanceGood && HammerOfWrath.IsSpellUsable &&
                 MySettings.UseHammerOfWrath)
        {
            HammerOfWrath.Launch();
        }
        else if (Exorcism.KnownSpell && Exorcism.IsDistanceGood && Exorcism.IsSpellUsable &&
                 MySettings.UseExorcism)
        {
            Exorcism.Launch();
        }
        else if ((ObjectManager.GetNumberAttackPlayer() <= 3 || !MySettings.UseHammerOfTheRighteous ||
                  ObjectManager.Target.HaveBuff(115798) || !MySettings.RefreshWeakenedBlows) &&
                 MySettings.UseCrusaderStrike &&
                 CrusaderStrike.KnownSpell && CrusaderStrike.IsDistanceGood &&
                 CrusaderStrike.IsSpellUsable)
        {
            CrusaderStrike.Launch();
        }
        else if ((ObjectManager.GetNumberAttackPlayer() >= 4 ||
                  (!ObjectManager.Target.HaveBuff(115798) && MySettings.RefreshWeakenedBlows) ||
                  (!MySettings.UseCrusaderStrike && !MySettings.UseHammerOfTheRighteous)) &&
                 HammerOfTheRighteous.KnownSpell && HammerOfTheRighteous.IsDistanceGood &&
                 HammerOfTheRighteous.IsSpellUsable && !ObjectManager.Me.HaveBuff(90174))
        {
            HammerOfTheRighteous.Launch();
        }
        else if (Judgment.KnownSpell && Judgment.IsDistanceGood && Judgment.IsSpellUsable &&
                 MySettings.UseJudgment)
        {
            Judgment.Launch();
        }
        else if ((ObjectManager.GetNumberAttackPlayer() <= 1 ||
                  (!MySettings.UseDivineStorm && MySettings.UseTemplarsVerdict)) &&
                 TemplarsVerdict.KnownSpell &&
                 (!Inquisition.KnownSpell || Inquisition.HaveBuff) &&
                 TemplarsVerdict.IsSpellUsable && TemplarsVerdict.IsDistanceGood &&
                 (ObjectManager.Me.HaveBuff(90174) || ObjectManager.Me.HolyPower >= 3))
        {
            TemplarsVerdict.Launch();
        }
        else if ((ObjectManager.GetNumberAttackPlayer() >= 2 ||
                  (MySettings.UseDivineStorm && !MySettings.UseTemplarsVerdict)) &&
                 DivineStorm.KnownSpell &&
                 (!Inquisition.KnownSpell || Inquisition.HaveBuff) &&
                 DivineStorm.IsSpellUsable && DivineStorm.IsDistanceGood &&
                 (ObjectManager.Me.HaveBuff(90174) || ObjectManager.Me.HolyPower >= 3))
        {
            DivineStorm.Launch();
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
        }
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
            if (File.Exists(CurrentSettingsFile))
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
            Thread.Sleep(150);
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
        else if (Shadow_Word_Death.IsSpellUsable && Shadow_Word_Death.IsDistanceGood && Shadow_Word_Death.KnownSpell
                 && ObjectManager.Target.HealthPercent < 20 && MySettings.UseShadowWordDeath)
        {
            Shadow_Word_Death.Launch();
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
                 && MySettings.UseMindFlay && Shadow_Word_Pain.TargetHaveBuff && Vampiric_Touch.TargetHaveBuff
                 && !ObjectManager.Me.HaveBuff(87160) && ObjectManager.GetNumberAttackPlayer() < 5
                 && ObjectManager.Me.ShadowOrbs != 3)
        {
            Mind_Flay.Launch();
            return;
        }
            // Blizzard API Calls for Mind Flay using Smite Function
        else if (!ObjectManager.Me.IsCast && Smite.IsSpellUsable && Smite.KnownSpell && Smite.IsDistanceGood
                 && MySettings.UseMindFlay && Shadow_Word_Pain.TargetHaveBuff && Vampiric_Touch.TargetHaveBuff
                 && !ObjectManager.Me.HaveBuff(87160) && ObjectManager.GetNumberAttackPlayer() < 5
                 && ObjectManager.Me.ShadowOrbs != 3)
        {
            Smite.Launch();
            return;
        }
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
                 && !ObjectManager.Me.HaveBuff(6788) && (ObjectManager.GetNumberAttackPlayer() > 0
                                                         || ObjectManager.Me.GetMove))
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
        while (ObjectManager.Target.GetDistance < 3 && ObjectManager.Target.InCombat)
        {
            nManager.Wow.Helpers.Keybindings.PressKeybindings(Keybindings.MOVEBACKWARD);
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
        while (ObjectManager.Target.GetDistance < 3 && ObjectManager.Target.InCombat)
        {
            nManager.Wow.Helpers.Keybindings.PressKeybindings(Keybindings.MOVEBACKWARD);
        }
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
        while (ObjectManager.Target.GetDistance < 3 && ObjectManager.Target.InCombat)
        {
            nManager.Wow.Helpers.Keybindings.PressKeybindings(Keybindings.MOVEBACKWARD);
        }
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
        while (ObjectManager.Target.GetDistance < 3 && ObjectManager.Target.InCombat)
        {
            nManager.Wow.Helpers.Keybindings.PressKeybindings(Keybindings.MOVEBACKWARD);
        }
    }
}

#endregion

#region Warrior

public class Warrior_Arms
{
    [Serializable]
    public class WarriorArmsSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Warrior Buffs */
        public bool UseBattleShout = true;
        public bool UseBattleStance = true;
        public bool UseBerserkerStance = false;
        public bool UseCommandingShout = false;
        public bool UseDefensiveStance = true;
        /* Offensive Spell */
        public bool UseAvatar = true;
        public bool UseBladestorm = true;
        public bool UseBloodbath = true;
        public bool UseCharge = true;
        public bool UseCleave = true;
        public bool UseColossusSmash = true;
        public bool UseDragonRoar = true;
        public bool UseExecute = true;
        public bool UseHeroicLeap = true;
        public bool UseHeroicStrike = true;
        public bool UseHeroicThrow = true;
        public bool UseMortalStrike = true;
        public bool UseOverpower = true;
        public bool UseShockwave = true;
        public bool UseSlam = true;
        public bool UseStormBolt = true;
        public bool UseTaunt = true;
        public bool UseThunderClap = true;
        public bool UseWhirlwind = true;
        /* Offensive Cooldown */
        public bool UseBerserkerRage = true;
        public bool UseDeadlyCalm = true;
        public bool UseRecklessness = true;
        public bool UseShatteringThrow = true;
        public bool UseSweepingStrikes = true;
        public bool UseSkullBanner = true;
        /* Defensive Cooldown */
        public bool UseDemoralizingBanner = true;
        public bool UseDiebytheSword = true;
        public bool UseDisarm = true;
        public bool UseDisruptingShout = true;
        public bool UseHamstring = false;
        public bool UseIntimidatingShout = true;
        public bool UseMassSpellReflection = true;
        public bool UsePiercingHowl = false;
        public bool UsePummel = true;
        public bool UseStaggeringShout = true;
        /* Healing Spell */
        public bool UseEnragedRegeneration = true;
        public bool UseRallyingCry = true;
        public bool UseVictoryRush = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;

        public WarriorArmsSettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Warrior Arms Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Warrior Buffs */
            AddControlInWinForm("Use Battle Shout", "UseBattleShout", "Warrior Buffs");
            AddControlInWinForm("Use Battle Stance", "UseBattleStance", "Warrior Buffs");
            AddControlInWinForm("Use Berserker Stance", "UseBerserkerStance", "Warrior Buffs");
            AddControlInWinForm("Use Commanding Shout", "UseCommandingShout", "Warrior Buffs");
            AddControlInWinForm("Use Defensive Stance", "UseDefensiveStance", "Warrior Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Avatar", "UseAvatar", "Offensive Spell");
            AddControlInWinForm("Use Bladestorm", "UseBladestorm", "Offensive Spell");
            AddControlInWinForm("Use Bloodbath", "UseBloodbath", "Offensive Spell");
            AddControlInWinForm("Use Charge", "UseCharge", "Offensive Spell");
            AddControlInWinForm("Use Cleave", "UseCleave", "Offensive Spell");
            AddControlInWinForm("Use Colossus Smash", "UseColossusSmash", "Offensive Spell");
            AddControlInWinForm("Use Dragon Roar", "UseDragonRoar", "Offensive Spell");
            AddControlInWinForm("Use Exectue", "UseExecute", "Offensive Spell");
            AddControlInWinForm("Use Heroic Leap", "UseHeroicLeap", "Offensive Spell");
            AddControlInWinForm("Use Heroic Strike", "UseHeroicStrike", "Offensive Spell");
            AddControlInWinForm("Use Heroic Throw", "UseHeroicThrow", "Offensive Spell");
            AddControlInWinForm("Use Mortal Strike", "UseMortalStrike", "Offensive Spell");
            AddControlInWinForm("Use Overpower", "UseOverpower", "Offensive Spell");
            AddControlInWinForm("Use Shockwave", "UseShockwave", "Offensive Spell");
            AddControlInWinForm("Use Slam", "UseSlam", "Offensive Spell");
            AddControlInWinForm("Use Storm Bolt", "UseStormBolt", "Offensive Spell");
            AddControlInWinForm("Use Taunt", "UseTaunt", "Offensive Spell");
            AddControlInWinForm("Use Thunder Clap", "UseThunderClap", "Offensive Spell");
            AddControlInWinForm("Use Whirlwind", "UseWhirlwind", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Berserker Rage", "UseBerserkerRage", "Offensive Cooldown");
            AddControlInWinForm("Use Deadly Calm", "UseDeadlyCalm", "Offensive Cooldown");
            AddControlInWinForm("Use Recklessness", "UseRecklessness", "Offensive Cooldown");
            AddControlInWinForm("Use Shattering Throw", "UseShatteringThrow", "Offensive Cooldown");
            AddControlInWinForm("Use Sweeping Strikes", "UseSweepingStrikes", "Offensive Cooldown");
            AddControlInWinForm("Use Skull Banner", "UseSkullBanner", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Demoralizing Banner", "UseDemoralizingBanner", "Defensive Cooldown");
            AddControlInWinForm("Use Die by the Sword", "UseDiebytheSword", "Defensive Cooldown");
            AddControlInWinForm("Use Disarm", "UseDisarm", "Defensive Cooldown");
            AddControlInWinForm("Use Disrupting Shout", "UseDisruptingShout", "Defensive Cooldown");
            AddControlInWinForm("Use Hamstring", "UseHamstring", "Defensive Cooldown");
            AddControlInWinForm("Use Intimidating Shout", "UseIntimidatingShout", "Defensive Cooldown");
            AddControlInWinForm("Use Mass Spell Reflection", "UseMassSpellReflection", "Defensive Cooldown");
            AddControlInWinForm("Use Piercing Howl", "UsePiercingHowl", "Defensive Cooldown");
            AddControlInWinForm("Use Pummel", "UsePummel", "Defensive Cooldown");
            AddControlInWinForm("Use Staggering Shout", "UseStaggeringShout", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Enraged Regeneration", "UseEnragedRegeneration", "Healing Spell");
            AddControlInWinForm("Use Rallying Cry", "UseRallyingCry", "Healing Spell");
            AddControlInWinForm("Use Victory Rush", "UseVictoryRush", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static WarriorArmsSettings CurrentSetting { get; set; }

        public static WarriorArmsSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Warrior_Arms.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Warrior_Arms.WarriorArmsSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Warrior_Arms.WarriorArmsSettings();
            }
        }
    }

    private readonly WarriorArmsSettings MySettings = WarriorArmsSettings.GetSettings();

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

    #region Warrior Buffs

    private readonly Spell Battle_Shout = new Spell("Battle Shout");
    private readonly Spell Battle_Stance = new Spell("Battle Stance");
    private readonly Spell Berserker_Stance = new Spell("Berserker Stance");
    private readonly Spell Commanding_Shout = new Spell("Commanding Shout");
    private readonly Spell Defensive_Stance = new Spell("Defensive Stance");

    #endregion

    #region Offensive Spell

    private readonly Spell Avatar = new Spell("Avatar");
    private readonly Spell Bladestorm = new Spell("Bladestorm");
    private readonly Spell Bloodbath = new Spell("Bloodbath");
    private readonly Spell Charge = new Spell("Charge");
    private readonly Spell Cleave = new Spell("Cleave");
    private readonly Spell Colossus_Smash = new Spell("Colossus Smash");
    private readonly Spell Dragon_Roar = new Spell("Dragon Roar");
    private readonly Spell Execute = new Spell("Execute");
    private readonly Spell Heroic_Leap = new Spell("Heroic Leap");
    private readonly Spell Heroic_Strike = new Spell("Heroic Strike");
    private readonly Spell Heroic_Throw = new Spell("Heroic Throw");
    private readonly Spell Impending_Victory = new Spell("Impending Victory");
    private readonly Spell Mortal_Strike = new Spell("Mortal Strike");
    private readonly Spell Overpower = new Spell("Overpower");
    private readonly Spell Shockwave = new Spell("Shockwave");
    private readonly Spell Slam = new Spell("Slam");
    private readonly Spell Storm_Bolt = new Spell("Storm Bolt");
    private readonly Spell Taunt = new Spell("Taunt");
    private readonly Spell Thunder_Clap = new Spell("Thunder Clap");
    private readonly Spell Whirlwind = new Spell("Whirlwind");

    #endregion

    #region Offensive Cooldown

    private readonly Spell Berserker_Rage = new Spell("Berserker Rage");
    private readonly Spell Deadly_Calm = new Spell("Deadly Calm");
    private readonly Spell Recklessness = new Spell("Recklessness");
    private readonly Spell Shattering_Throw = new Spell("Shattering Throw");
    private readonly Spell Sweeping_Strikes = new Spell("Sweeping Strikes");
    private readonly Spell Skull_Banner = new Spell("Skull Banner");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Die_by_the_Sword = new Spell("Die by the Sword");
    private readonly Spell Disarm = new Spell("Disarm");
    private Timer Disarm_Timer = new Timer(0);
    private readonly Spell Disrupting_Shout = new Spell("Disrupting Shout");
    private readonly Spell Hamstring = new Spell("Hamstring");
    private readonly Spell Intimidating_Shout = new Spell("Intimidating Shout");
    private readonly Spell Mass_Spell_Reflection = new Spell("Mass Spell Reflection");
    private readonly Spell Piercing_Howl = new Spell("Piercing Howl");
    private readonly Spell Pummel = new Spell("Pummel");
    private readonly Spell Staggering_Shout = new Spell("Staggering Shout");
    private readonly Spell Demoralizing_Banner = new Spell("Demoralizing Banner");

    #endregion

    #region Healing Spell

    private readonly Spell Enraged_Regeneration = new Spell("Enraged Regeneration");
    private readonly Spell Rallying_Cry = new Spell("Rallying Cry");
    private readonly Spell Victory_Rush = new Spell("Victory Rush");

    #endregion

    //private readonly WoWItem FirstTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET);
    //private readonly WoWItem SecondTrinket = EquippedItems.GetEquippedItem(WoWInventorySlot.INVTYPE_TRINKET, 2);

    private Timer Trinket_Timer = new Timer(0);
    private Timer OnCD = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    public int LC = 0;

    public Warrior_Arms()
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
                            Taunt.IsDistanceGood)
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
            Thread.Sleep(150);
        }
    }

    public void Pull()
    {
        if (Heroic_Leap.IsDistanceGood && Heroic_Leap.KnownSpell && Heroic_Leap.IsSpellUsable
            && MySettings.UseHeroicLeap)
        {
            SpellManager.CastSpellByIDAndPosition(6544, ObjectManager.Target.Position);
            Thread.Sleep(200);
        }

        if (Taunt.IsDistanceGood && Taunt.KnownSpell && Taunt.IsSpellUsable
            && MySettings.UseTaunt && ObjectManager.Target.GetDistance > 20)
        {
            Taunt.Launch();
            return;
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

    public void LowCombat()
    {
        AvoidMelee();
        Heal();
        Defense_Cycle();
        Buff();

        if (Heroic_Throw.KnownSpell && Heroic_Throw.IsSpellUsable && Heroic_Throw.IsDistanceGood
            && MySettings.UseHeroicThrow && !ObjectManager.Target.InCombat)
        {
            Heroic_Throw.Launch();
            return;
        }

        if (Charge.KnownSpell && Charge.IsSpellUsable && Charge.IsDistanceGood
            && MySettings.UseCharge && ObjectManager.Target.GetDistance > 14)
        {
            Charge.Launch();
            return;
        }

        if (Mortal_Strike.KnownSpell && Mortal_Strike.IsSpellUsable && Mortal_Strike.IsDistanceGood
            && MySettings.UseMortalStrike)
        {
            Mortal_Strike.Launch();
            return;
        }
        else if (Colossus_Smash.KnownSpell && Colossus_Smash.IsDistanceGood && Colossus_Smash.IsSpellUsable
                 && MySettings.UseColossusSmash)
        {
            Colossus_Smash.Launch();
            return;
        }
        else if (Heroic_Strike.KnownSpell && Heroic_Strike.IsSpellUsable && Heroic_Strike.IsDistanceGood
                 && MySettings.UseHeroicStrike && ObjectManager.GetNumberAttackPlayer() < 3
                 && (ObjectManager.Me.RagePercentage > 90 || ObjectManager.Me.HaveBuff(125831)))
        {
            if (Deadly_Calm.KnownSpell && Deadly_Calm.IsSpellUsable && MySettings.UseDeadlyCalm)
            {
                Deadly_Calm.Launch();
                Thread.Sleep(200);
            }

            Heroic_Strike.Launch();
            return;
        }
        else if (Shockwave.KnownSpell && Shockwave.IsSpellUsable && ObjectManager.Target.GetDistance < 10
                 && MySettings.UseShockwave)
        {
            Shockwave.Launch();
            return;
        }
        else if (Dragon_Roar.KnownSpell && Dragon_Roar.IsSpellUsable && ObjectManager.Target.GetDistance < 8
                 && MySettings.UseDragonRoar)
        {
            Dragon_Roar.Launch();
            return;
        }
        else
        {
            if (Bladestorm.KnownSpell && Bladestorm.IsSpellUsable && ObjectManager.Target.GetDistance < 8
                && MySettings.UseBladestorm)
            {
                Bladestorm.Launch();
                return;
            }
        }

        if (Thunder_Clap.KnownSpell && Thunder_Clap.IsSpellUsable && Thunder_Clap.IsDistanceGood
            && MySettings.UseThunderClap)
        {
            Thunder_Clap.Launch();
            return;
        }
    }

    public void DPS_Burst()
    {
        /*if (MySettings.UseTrinket)
        {
            if (!SpellManager.HaveBuffLua(ItemsManager.GetItemSpellByItemName(FirstTrinket.Name)))
            {
                if (ItemsManager.IsUsableItemByName(FirstTrinket.Name) && !ItemsManager.IsItemOnCooldown((uint) FirstTrinket.Entry))
                {
                    ItemsManager.UseItem(FirstTrinket.Name);
                    Logging.WriteFight("Use First Trinket Slot");
                }
            }
            if (!SpellManager.HaveBuffLua(ItemsManager.GetItemSpellByItemName(SecondTrinket.Name)))
            {
                if (ItemsManager.IsUsableItemByName(SecondTrinket.Name) && !ItemsManager.IsItemOnCooldown((uint)SecondTrinket.Entry))
                {
                    ItemsManager.UseItem(SecondTrinket.Name);
                    Logging.WriteFight("Use Second Trinket Slot");
                }
            }
        }*/
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
        else if (Berserker_Rage.KnownSpell && Berserker_Rage.IsSpellUsable && ObjectManager.Me.RagePercentage < 50
                 && MySettings.UseBerserkerRage && ObjectManager.Target.GetDistance < 30)
        {
            Berserker_Rage.Launch();
            return;
        }
        else if (Battle_Shout.KnownSpell && Battle_Shout.IsSpellUsable && ObjectManager.Me.RagePercentage < 80
                 && MySettings.UseBattleShout && ObjectManager.Target.GetDistance < 30)
        {
            Battle_Shout.Launch();
            return;
        }
        else if (Commanding_Shout.KnownSpell && Commanding_Shout.IsSpellUsable && ObjectManager.Me.RagePercentage < 80
                 && MySettings.UseCommandingShout && !MySettings.UseBattleShout && ObjectManager.Target.GetDistance < 30)
        {
            Commanding_Shout.Launch();
            return;
        }
        else if (Recklessness.KnownSpell && Recklessness.IsSpellUsable && MySettings.UseRecklessness
                 && ObjectManager.Target.GetDistance < 30)
        {
            Recklessness.Launch();
            return;
        }
        else if (Shattering_Throw.KnownSpell && Shattering_Throw.IsSpellUsable && Shattering_Throw.IsDistanceGood
                 && MySettings.UseShatteringThrow)
        {
            Shattering_Throw.Launch();
            return;
        }
        else if (Skull_Banner.KnownSpell && Skull_Banner.IsSpellUsable
                 && MySettings.UseSkullBanner && ObjectManager.Target.GetDistance < 30)
        {
            Skull_Banner.Launch();
            return;
        }
        else if (Avatar.KnownSpell && Avatar.IsSpellUsable
                 && MySettings.UseAvatar && ObjectManager.Target.GetDistance < 30)
        {
            Avatar.Launch();
            return;
        }
        else if (Bloodbath.KnownSpell && Bloodbath.IsSpellUsable
                 && MySettings.UseBloodbath && ObjectManager.Target.GetDistance < 30)
        {
            Bloodbath.Launch();
            return;
        }
        else if (Deadly_Calm.KnownSpell && Deadly_Calm.IsSpellUsable && ObjectManager.Me.RagePercentage > 90
                 && MySettings.UseDeadlyCalm && Heroic_Strike.IsDistanceGood)
        {
            Deadly_Calm.Launch();
            return;
        }
        else
        {
            if (Storm_Bolt.KnownSpell && Storm_Bolt.IsSpellUsable
                && MySettings.UseStormBolt && Storm_Bolt.IsDistanceGood)
            {
                Storm_Bolt.Launch();
                return;
            }
        }

        if (Heroic_Strike.KnownSpell && Heroic_Strike.IsSpellUsable && Heroic_Strike.IsDistanceGood
            && MySettings.UseHeroicStrike && ObjectManager.Me.Level < 10)
        {
            Heroic_Strike.Launch();
            return;
        }
    }

    public void DPS_Cycle()
    {
        if (Heroic_Throw.KnownSpell && Heroic_Throw.IsSpellUsable && Heroic_Throw.IsDistanceGood
            && MySettings.UseHeroicThrow && !ObjectManager.Target.InCombat)
        {
            Heroic_Throw.Launch();
            return;
        }

        if (Charge.KnownSpell && Charge.IsSpellUsable && Charge.IsDistanceGood
            && MySettings.UseCharge && ObjectManager.Target.GetDistance > 14)
        {
            Charge.Launch();
            return;
        }

        if (Victory_Rush.KnownSpell && Victory_Rush.IsSpellUsable && Victory_Rush.IsDistanceGood
            && MySettings.UseVictoryRush && ObjectManager.Me.HealthPercent < 90)
        {
            Victory_Rush.Launch();
            return;
        }
        else if (Sweeping_Strikes.KnownSpell && Sweeping_Strikes.IsSpellUsable &&
                 ObjectManager.GetNumberAttackPlayer() > 1
                 && MySettings.UseSweepingStrikes)
        {
            Sweeping_Strikes.Launch();
            return;
        }
        else if (Thunder_Clap.KnownSpell && Thunder_Clap.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 2
                 && MySettings.UseThunderClap)
        {
            Thunder_Clap.Launch();
            return;
        }
        else if (Whirlwind.KnownSpell && Whirlwind.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 3
                 && MySettings.UseWhirlwind)
        {
            Whirlwind.Launch();
            return;
        }
        else if (Cleave.KnownSpell && Cleave.IsSpellUsable && Cleave.IsDistanceGood
                 && ObjectManager.GetNumberAttackPlayer() == 3 && MySettings.UseCleave)
        {
            if (Deadly_Calm.KnownSpell && Deadly_Calm.IsSpellUsable && MySettings.UseDeadlyCalm)
            {
                Deadly_Calm.Launch();
                Thread.Sleep(200);
            }

            Cleave.Launch();
            return;
        }
        else if (Heroic_Strike.KnownSpell && Heroic_Strike.IsSpellUsable && Heroic_Strike.IsDistanceGood
                 && MySettings.UseHeroicStrike && ObjectManager.GetNumberAttackPlayer() < 3
                 && (ObjectManager.Me.HaveBuff(125831) || ObjectManager.Me.HaveBuff(85730)))
        {
            if (Deadly_Calm.KnownSpell && Deadly_Calm.IsSpellUsable && MySettings.UseDeadlyCalm)
            {
                Deadly_Calm.Launch();
                Thread.Sleep(200);
            }

            Heroic_Strike.Launch();
            return;
        }
        else if (Shockwave.KnownSpell && Shockwave.IsSpellUsable && ObjectManager.Target.GetDistance < 10
                 && MySettings.UseShockwave)
        {
            Shockwave.Launch();
            return;
        }
        else if (Dragon_Roar.KnownSpell && Dragon_Roar.IsSpellUsable && ObjectManager.Target.GetDistance < 8
                 && MySettings.UseDragonRoar)
        {
            Dragon_Roar.Launch();
            return;
        }
        else if (Bladestorm.KnownSpell && Bladestorm.IsSpellUsable && ObjectManager.Target.GetDistance < 8
                 && MySettings.UseBladestorm)
        {
            Bladestorm.Launch();
            return;
        }
        else if (Mortal_Strike.KnownSpell && Mortal_Strike.IsSpellUsable && Mortal_Strike.IsDistanceGood
                 && MySettings.UseMortalStrike && ObjectManager.Me.RagePercentage < 100)
        {
            Mortal_Strike.Launch();
            return;
        }
        else if (Colossus_Smash.KnownSpell && Colossus_Smash.IsSpellUsable && Colossus_Smash.IsDistanceGood
                 && MySettings.UseColossusSmash)
        {
            Colossus_Smash.Launch();
            return;
        }
        else if (Execute.KnownSpell && Execute.IsSpellUsable && Execute.IsDistanceGood
                 && MySettings.UseExecute && ObjectManager.GetNumberAttackPlayer() < 4)
        {
            Execute.Launch();
            return;
        }
        else if (Overpower.KnownSpell && Overpower.IsSpellUsable && Overpower.IsDistanceGood
                 && MySettings.UseOverpower && ObjectManager.Me.RagePercentage < 100)
        {
            Overpower.Launch();
            return;
        }
        else
        {
            if (Slam.KnownSpell && Slam.IsSpellUsable && Slam.IsDistanceGood && MySettings.UseSlam
                && ObjectManager.GetNumberAttackPlayer() < 4 && ObjectManager.Target.HealthPercent > 20)
            {
                Slam.Launch();
                return;
            }
        }
    }

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Heal();
            Buff();
        }
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ObjectManager.Me.HealthPercent < 30 && MySettings.UseDefensiveStance
            && Defensive_Stance.KnownSpell && Defensive_Stance.IsSpellUsable && !Defensive_Stance.HaveBuff)
        {
            Defensive_Stance.Launch();
            return;
        }
        else if (!Battle_Stance.HaveBuff && Battle_Stance.KnownSpell && Battle_Stance.IsSpellUsable
                 && MySettings.UseBattleStance && ObjectManager.Me.HealthPercent > 50)
        {
            Battle_Stance.Launch();
            return;
        }
        else if (!Berserker_Stance.HaveBuff && Berserker_Stance.KnownSpell && Berserker_Stance.IsSpellUsable
                 && MySettings.UseBerserkerStance && !MySettings.UseBattleStance && ObjectManager.Me.HealthPercent > 50)
        {
            Berserker_Stance.Launch();
            return;
        }
        else if (Battle_Shout.KnownSpell && Battle_Shout.IsSpellUsable && !Battle_Shout.HaveBuff
                 && MySettings.UseBattleShout)
        {
            Battle_Shout.Launch();
            return;
        }
        else
        {
            if (Commanding_Shout.KnownSpell && Commanding_Shout.IsSpellUsable && !Commanding_Shout.HaveBuff
                && MySettings.UseCommandingShout && !MySettings.UseBattleShout)
            {
                Commanding_Shout.Launch();
                return;
            }
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Victory_Rush.KnownSpell && Victory_Rush.IsSpellUsable && Victory_Rush.IsDistanceGood
            && MySettings.UseVictoryRush && ObjectManager.Me.HealthPercent < 90)
        {
            Victory_Rush.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 30 && Rallying_Cry.IsSpellUsable && Rallying_Cry.KnownSpell
                 && MySettings.UseRallyingCry && Fight.InFight)
        {
            Rallying_Cry.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell
                 && MySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 80 && Enraged_Regeneration.IsSpellUsable &&
                Enraged_Regeneration.KnownSpell
                && MySettings.UseEnragedRegeneration)
            {
                Enraged_Regeneration.Launch();
                return;
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 95 && MySettings.UseDisarm && Disarm.IsDistanceGood
            && Disarm.KnownSpell && Disarm.IsSpellUsable && Disarm_Timer.IsReady)
        {
            Disarm.Launch();
            Disarm_Timer = new Timer(1000*60);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 20 && MySettings.UseIntimidatingShout
                 && Intimidating_Shout.KnownSpell && Intimidating_Shout.IsSpellUsable &&
                 ObjectManager.Target.GetDistance < 8)
        {
            Intimidating_Shout.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseDiebytheSword
                 && Die_by_the_Sword.KnownSpell && Die_by_the_Sword.IsSpellUsable)
        {
            Die_by_the_Sword.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseDemoralizingBanner
                 && Demoralizing_Banner.KnownSpell && Demoralizing_Banner.IsSpellUsable &&
                 ObjectManager.Target.GetDistance < 30)
        {
            SpellManager.CastSpellByIDAndPosition(114203, ObjectManager.Target.Position);
            OnCD = new Timer(1000*15);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && War_Stomp.IsSpellUsable && War_Stomp.KnownSpell
                 && MySettings.UseWarStomp)
        {
            War_Stomp.Launch();
            OnCD = new Timer(1000*2);
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell
                && MySettings.UseStoneform)
            {
                Stoneform.Launch();
                OnCD = new Timer(1000*8);
                return;
            }
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ObjectManager.Target.GetDistance < 8
            && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable && MySettings.UseArcaneTorrent)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else if (!Hamstring.TargetHaveBuff && MySettings.UseHamstring && Hamstring.KnownSpell
                 && Hamstring.IsSpellUsable && Hamstring.IsDistanceGood)
        {
            Hamstring.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && Pummel.IsDistanceGood
                 && Pummel.KnownSpell && Pummel.IsSpellUsable && MySettings.UsePummel)
        {
            Pummel.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe &&
                 ObjectManager.Target.GetDistance < 10
                 && Disrupting_Shout.KnownSpell && Disrupting_Shout.IsSpellUsable && MySettings.UseDisruptingShout)
        {
            Disrupting_Shout.Launch();
            return;
        }
        else if (ObjectManager.Target.GetMove && !Piercing_Howl.TargetHaveBuff && MySettings.UsePiercingHowl
                 && Piercing_Howl.KnownSpell && Piercing_Howl.IsSpellUsable && ObjectManager.Target.GetDistance < 15)
        {
            Piercing_Howl.Launch();
            return;
        }
        else if (Hamstring.TargetHaveBuff && MySettings.UseStaggeringShout && Staggering_Shout.KnownSpell
                 && Staggering_Shout.IsSpellUsable && ObjectManager.Target.GetDistance < 20)
        {
            Staggering_Shout.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe &&
                MySettings.UseMassSpellReflection
                && Mass_Spell_Reflection.KnownSpell && Mass_Spell_Reflection.IsSpellUsable)
            {
                Mass_Spell_Reflection.Launch();
                return;
            }
        }
    }

    private void AvoidMelee()
    {
        while (ObjectManager.Target.GetDistance < 3 && ObjectManager.Target.InCombat)
        {
            nManager.Wow.Helpers.Keybindings.PressKeybindings(Keybindings.MOVEBACKWARD);
        }
    }
}

public class Warrior_Protection
{
    [Serializable]
    public class WarriorProtectionSettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Warrior Buffs */
        public bool UseBattleShout = true;
        public bool UseBattleStance = true;
        public bool UseBerserkerStance = false;
        public bool UseCommandingShout = false;
        public bool UseDefensiveStance = true;
        /* Offensive Spell */
        public bool UseAvatar = true;
        public bool UseBladestorm = true;
        public bool UseBloodbath = true;
        public bool UseCharge = true;
        public bool UseCleave = true;
        public bool UseDevastate = true;
        public bool UseDragonRoar = true;
        public bool UseExecute = true;
        public bool UseHeroicLeap = true;
        public bool UseHeroicStrike = true;
        public bool UseHeroicThrow = true;
        public bool UseRevenge = true;
        public bool UseShieldSlam = true;
        public bool UseShockwave = true;
        public bool UseStormBolt = true;
        public bool UseTaunt = true;
        public bool UseThunderClap = true;
        /* Offensive Cooldown */
        public bool UseBerserkerRage = true;
        public bool UseDeadlyCalm = true;
        public bool UseRecklessness = true;
        public bool UseShatteringThrow = true;
        public bool UseSweepingStrikes = true;
        public bool UseSkullBanner = true;
        /* Defensive Cooldown */
        public bool UseDemoralizingBanner = true;
        public bool UseDemoralizingShout = true;
        public bool UseDisarm = true;
        public bool UseDisruptingShout = true;
        public bool UseHamstring = false;
        public bool UseIntimidatingShout = true;
        public bool UseMassSpellReflection = true;
        public bool UsePiercingHowl = false;
        public bool UsePummel = true;
        public bool UseShieldBarrier = true;
        public bool UseShieldBlock = true;
        public bool UseShieldWall = true;
        public bool UseSpellReflection = true;
        public bool UseStaggeringShout = true;
        /* Healing Spell */
        public bool UseEnragedRegeneration = true;
        public bool UseLastStand = true;
        public bool UseRallyingCry = true;
        public bool UseVictoryRush = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;

        public WarriorProtectionSettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Warrior Protection Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Warrior Buffs */
            AddControlInWinForm("Use Battle Shout", "UseBattleShout", "Warrior Buffs");
            AddControlInWinForm("Use Battle Stance", "UseBattleStance", "Warrior Buffs");
            AddControlInWinForm("Use Berserker Stance", "UseBerserkerStance", "Warrior Buffs");
            AddControlInWinForm("Use Commanding Shout", "UseCommandingShout", "Warrior Buffs");
            AddControlInWinForm("Use Defensive Stance", "UseDefensiveStance", "Warrior Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Avatar", "UseAvatar", "Offensive Spell");
            AddControlInWinForm("Use Bladestorm", "UseBladestorm", "Offensive Spell");
            AddControlInWinForm("Use Bloodbath", "UseBloodbath", "Offensive Spell");
            AddControlInWinForm("Use Charge", "UseCharge", "Offensive Spell");
            AddControlInWinForm("Use Cleave", "UseCleave", "Offensive Spell");
            AddControlInWinForm("Use Devastate", "UseDevastate", "Offensive Spell");
            AddControlInWinForm("Use Dragon Roar", "UseDragonRoar", "Offensive Spell");
            AddControlInWinForm("Use Exectue", "UseExecute", "Offensive Spell");
            AddControlInWinForm("Use Heroic Leap", "UseHeroicLeap", "Offensive Spell");
            AddControlInWinForm("Use Heroic Strike", "UseHeroicStrike", "Offensive Spell");
            AddControlInWinForm("Use Heroic Throw", "UseHeroicThrow", "Offensive Spell");
            AddControlInWinForm("Use Revenge", "UseRevenge", "Offensive Spell");
            AddControlInWinForm("Use Shield Slam", "UseShieldSlam", "Offensive Spell");
            AddControlInWinForm("Use Shockwave", "UseShockwave", "Offensive Spell");
            AddControlInWinForm("Use Storm Bolt", "UseStormBolt", "Offensive Spell");
            AddControlInWinForm("Use Taunt", "UseTaunt", "Offensive Spell");
            AddControlInWinForm("Use Thunder Clap", "UseThunderClap", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Berserker Rage", "UseBerserkerRage", "Offensive Cooldown");
            AddControlInWinForm("Use Deadly Calm", "UseDeadlyCalm", "Offensive Cooldown");
            AddControlInWinForm("Use Recklessness", "UseRecklessness", "Offensive Cooldown");
            AddControlInWinForm("Use Shattering Throw", "UseShatteringThrow", "Offensive Cooldown");
            AddControlInWinForm("Use Sweeping Strikes", "UseSweepingStrikes", "Offensive Cooldown");
            AddControlInWinForm("Use Skull Banner", "UseSkullBanner", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Demoralizing Banner", "UseDemoralizingBanner", "Defensive Cooldown");
            AddControlInWinForm("Use Demoralizing Shout", "UseDemoralizingShout", "Defensive Cooldown");
            AddControlInWinForm("Use Disarm", "UseDisarm", "Defensive Cooldown");
            AddControlInWinForm("Use Disrupting Shout", "UseDisruptingShout", "Defensive Cooldown");
            AddControlInWinForm("Use Hamstring", "UseHamstring", "Defensive Cooldown");
            AddControlInWinForm("Use Intimidating Shout", "UseIntimidatingShout", "Defensive Cooldown");
            AddControlInWinForm("Use Mass Spell Reflection", "UseMassSpellReflection", "Defensive Cooldown");
            AddControlInWinForm("Use Piercing Howl", "UsePiercingHowl", "Defensive Cooldown");
            AddControlInWinForm("Use Pummel", "UsePummel", "Defensive Cooldown");
            AddControlInWinForm("Use Shield Barrier", "UseShieldBarrier", "Defensive Cooldown");
            AddControlInWinForm("Use Shield Block", "UseShieldBlock", "Defensive Cooldown");
            AddControlInWinForm("Use Shield Wall", "UseShieldWall", "Defensive Cooldown");
            AddControlInWinForm("Use Spell Reflection", "UseSpellReflection", "Defensive Cooldown");
            AddControlInWinForm("Use Staggering Shout", "UseStaggeringShout", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Enraged Regeneration", "UseEnragedRegeneration", "Healing Spell");
            AddControlInWinForm("Use Last Stand", "UseLastStand", "Healing Spell");
            AddControlInWinForm("Use Rallying Cry", "UseRallyingCry", "Healing Spell");
            AddControlInWinForm("Use Victory Rush", "UseVictoryRush", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static WarriorProtectionSettings CurrentSetting { get; set; }

        public static WarriorProtectionSettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Warrior_Protection.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Warrior_Protection.WarriorProtectionSettings>(CurrentSettingsFile);
            }
            else
            {
                return new Warrior_Protection.WarriorProtectionSettings();
            }
        }
    }

    private readonly WarriorProtectionSettings MySettings = WarriorProtectionSettings.GetSettings();

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

    #region Warrior Buffs

    private readonly Spell Battle_Shout = new Spell("Battle Shout");
    private readonly Spell Battle_Stance = new Spell("Battle Stance");
    private readonly Spell Berserker_Stance = new Spell("Berserker Stance");
    private readonly Spell Commanding_Shout = new Spell("Commanding Shout");
    private readonly Spell Defensive_Stance = new Spell("Defensive Stance");

    #endregion

    #region Offensive Spell

    private readonly Spell Avatar = new Spell("Avatar");
    private readonly Spell Bladestorm = new Spell("Bladestorm");
    private readonly Spell Bloodbath = new Spell("Bloodbath");
    private readonly Spell Charge = new Spell("Charge");
    private readonly Spell Cleave = new Spell("Cleave");
    private readonly Spell Devastate = new Spell("Devastate");
    private readonly Spell Dragon_Roar = new Spell("Dragon Roar");
    private readonly Spell Execute = new Spell("Execute");
    private readonly Spell Heroic_Leap = new Spell("Heroic Leap");
    private readonly Spell Heroic_Strike = new Spell("Heroic Strike");
    private readonly Spell Heroic_Throw = new Spell("Heroic Throw");
    private readonly Spell Revenge = new Spell("Revenge");
    private readonly Spell Shield_Slam = new Spell("Shield Slam");
    private readonly Spell Shockwave = new Spell("Shockwave");
    private readonly Spell Storm_Bolt = new Spell("Storm Bolt");
    private readonly Spell Sunder_Armor = new Spell("Sunder Armor");
    private readonly Spell Taunt = new Spell("Taunt");
    private readonly Spell Thunder_Clap = new Spell("Thunder Clap");

    #endregion

    #region Offensive Cooldown

    private readonly Spell Berserker_Rage = new Spell("Berserker Rage");
    private readonly Spell Deadly_Calm = new Spell("Deadly Calm");
    private readonly Spell Recklessness = new Spell("Recklessness");
    private readonly Spell Shattering_Throw = new Spell("Shattering Throw");
    private readonly Spell Sweeping_Strikes = new Spell("Sweeping Strikes");
    private readonly Spell Skull_Banner = new Spell("Skull Banner");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Demoralizing_Banner = new Spell("Demoralizing Banner");
    private readonly Spell Demoralizing_Shout = new Spell("Demoralizing Shout");
    private readonly Spell Disarm = new Spell("Disarm");
    private Timer Disarm_Timer = new Timer(0);
    private readonly Spell Disrupting_Shout = new Spell("Disrupting Shout");
    private readonly Spell Hamstring = new Spell("Hamstring");
    private readonly Spell Intimidating_Shout = new Spell("Intimidating Shout");
    private readonly Spell Mass_Spell_Reflection = new Spell("Mass Spell Reflection");
    private readonly Spell Piercing_Howl = new Spell("Piercing Howl");
    private readonly Spell Pummel = new Spell("Pummel");
    private readonly Spell Shield_Barrier = new Spell("Shield Barrier");
    private Timer Shield_Barrier_Timer = new Timer(0);
    private readonly Spell Shield_Block = new Spell("Shield Block");
    private readonly Spell Shield_Wall = new Spell("Shield Wall");
    private readonly Spell Spell_Reflection = new Spell("Spell Reflection");
    private readonly Spell Staggering_Shout = new Spell("Staggering Shout");

    #endregion

    #region Healing Spell

    private readonly Spell Enraged_Regeneration = new Spell("Enraged Regeneration");
    private readonly Spell Last_Stand = new Spell("Last Stand");
    private readonly Spell Rallying_Cry = new Spell("Rallying Cry");
    private readonly Spell Victory_Rush = new Spell("Victory Rush");

    #endregion

    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    public int LC = 0;

    public Warrior_Protection()
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
                            Taunt.IsDistanceGood)
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
        if (Heroic_Leap.IsDistanceGood && Heroic_Leap.KnownSpell && Heroic_Leap.IsSpellUsable
            && MySettings.UseHeroicLeap)
        {
            SpellManager.CastSpellByIDAndPosition(6544, ObjectManager.Target.Position);
            Thread.Sleep(200);
        }

        if (Taunt.IsDistanceGood && Taunt.KnownSpell && Taunt.IsSpellUsable
            && MySettings.UseTaunt && ObjectManager.Target.GetDistance > 20)
        {
            Taunt.Launch();
            return;
        }
    }

    public void LowCombat()
    {
        AvoidMelee();
        Heal();
        Defense_Cycle();
        Buff();

        if (Heroic_Throw.KnownSpell && Heroic_Throw.IsSpellUsable && Heroic_Throw.IsDistanceGood
            && MySettings.UseHeroicThrow && !ObjectManager.Target.InCombat)
        {
            Heroic_Throw.Launch();
            return;
        }

        if (Charge.KnownSpell && Charge.IsSpellUsable && Charge.IsDistanceGood
            && MySettings.UseCharge && ObjectManager.Target.GetDistance > 14)
        {
            Charge.Launch();
            return;
        }

        if (Shield_Slam.KnownSpell && Shield_Slam.IsSpellUsable && Shield_Slam.IsDistanceGood
            && ObjectManager.Me.RagePercentage < 95 && MySettings.UseShieldSlam)
        {
            Shield_Slam.Launch();
            return;
        }
        else if (Heroic_Strike.KnownSpell && Heroic_Strike.IsSpellUsable && Heroic_Strike.IsDistanceGood
                 && MySettings.UseHeroicStrike &&
                 (ObjectManager.Me.RagePercentage > 80 || ObjectManager.Me.HaveBuff(122510)))
        {
            if (ObjectManager.Me.HealthPercent > 80)
            {
                if (Deadly_Calm.KnownSpell && Deadly_Calm.IsSpellUsable && MySettings.UseDeadlyCalm)
                {
                    Deadly_Calm.Launch();
                    Thread.Sleep(200);
                }
                Heroic_Strike.Launch();
                return;
            }
        }
        else if (Revenge.KnownSpell && Revenge.IsDistanceGood && Revenge.IsSpellUsable
                 && ObjectManager.Me.RagePercentage < 95 && MySettings.UseRevenge)
        {
            Revenge.Launch();
            return;
        }
        else if (Shockwave.KnownSpell && Shockwave.IsSpellUsable && Shockwave.IsDistanceGood
                 && MySettings.UseShockwave)
        {
            Shockwave.Launch();
            return;
        }
        else if (Dragon_Roar.KnownSpell && Dragon_Roar.IsSpellUsable && Dragon_Roar.IsDistanceGood
                 && MySettings.UseDragonRoar)
        {
            Dragon_Roar.Launch();
            return;
        }
        else if (Bladestorm.KnownSpell && Bladestorm.IsSpellUsable && Bladestorm.IsDistanceGood
                 && MySettings.UseBladestorm)
        {
            Bladestorm.Launch();
            return;
        }
        else
        {
            // Blizzard API Calls for Devastate using Sunder Armor Function
            if (Sunder_Armor.KnownSpell && Sunder_Armor.IsSpellUsable && Sunder_Armor.IsDistanceGood
                && MySettings.UseDevastate)
            {
                Sunder_Armor.Launch();
                return;
            }
        }

        if (Thunder_Clap.KnownSpell && Thunder_Clap.IsSpellUsable && Thunder_Clap.IsDistanceGood
            && MySettings.UseThunderClap)
        {
            Thunder_Clap.Launch();
            return;
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
        else if (Berserker_Rage.KnownSpell && Berserker_Rage.IsSpellUsable && ObjectManager.Me.RagePercentage < 50
                 && MySettings.UseBerserkerRage && ObjectManager.Target.GetDistance < 30)
        {
            Berserker_Rage.Launch();
            return;
        }
        else if (Battle_Shout.KnownSpell && Battle_Shout.IsSpellUsable && ObjectManager.Me.RagePercentage < 80
                 && MySettings.UseBattleShout && ObjectManager.Target.GetDistance < 30)
        {
            Battle_Shout.Launch();
            return;
        }
        else if (Commanding_Shout.KnownSpell && Commanding_Shout.IsSpellUsable && ObjectManager.Me.RagePercentage < 80
                 && MySettings.UseCommandingShout && !MySettings.UseBattleShout && ObjectManager.Target.GetDistance < 30)
        {
            Commanding_Shout.Launch();
            return;
        }
        else if (Recklessness.KnownSpell && Recklessness.IsSpellUsable && MySettings.UseRecklessness
                 && ObjectManager.Target.GetDistance < 30)
        {
            Recklessness.Launch();
            return;
        }
        else if (Shattering_Throw.KnownSpell && Shattering_Throw.IsSpellUsable && Shattering_Throw.IsDistanceGood
                 && MySettings.UseShatteringThrow)
        {
            Shattering_Throw.Launch();
            return;
        }
        else if (Skull_Banner.KnownSpell && Skull_Banner.IsSpellUsable
                 && MySettings.UseSkullBanner && ObjectManager.Target.GetDistance < 30)
        {
            Skull_Banner.Launch();
            return;
        }
        else if (Avatar.KnownSpell && Avatar.IsSpellUsable
                 && MySettings.UseAvatar && ObjectManager.Target.GetDistance < 30)
        {
            Avatar.Launch();
            return;
        }
        else if (Bloodbath.KnownSpell && Bloodbath.IsSpellUsable
                 && MySettings.UseBloodbath && ObjectManager.Target.GetDistance < 30)
        {
            Bloodbath.Launch();
            return;
        }
        else
        {
            if (Storm_Bolt.KnownSpell && Storm_Bolt.IsSpellUsable
                && MySettings.UseStormBolt && Storm_Bolt.IsDistanceGood)
            {
                Storm_Bolt.Launch();
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

    public void DPS_Cycle()
    {
        if (Heroic_Throw.KnownSpell && Heroic_Throw.IsSpellUsable && Heroic_Throw.IsDistanceGood
            && MySettings.UseHeroicThrow && !ObjectManager.Target.InCombat)
        {
            Heroic_Throw.Launch();
            return;
        }

        if (Charge.KnownSpell && Charge.IsSpellUsable && Charge.IsDistanceGood
            && MySettings.UseCharge && ObjectManager.Target.GetDistance > 14)
        {
            Charge.Launch();
            return;
        }

        if (Victory_Rush.KnownSpell && Victory_Rush.IsSpellUsable && Victory_Rush.IsDistanceGood
            && MySettings.UseVictoryRush && ObjectManager.Me.HealthPercent < 90)
        {
            Victory_Rush.Launch();
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2 && Thunder_Clap.KnownSpell && Thunder_Clap.IsSpellUsable
            && Thunder_Clap.IsDistanceGood && MySettings.UseThunderClap)
        {
            Thunder_Clap.Launch();
            return;
        }

        if (Cleave.KnownSpell && Cleave.IsSpellUsable && Cleave.IsDistanceGood &&
            ObjectManager.GetNumberAttackPlayer() > 2
            && MySettings.UseCleave && (ObjectManager.Me.RagePercentage > 80 || ObjectManager.Me.HaveBuff(122510)))
        {
            if (ObjectManager.Me.HealthPercent > 80)
            {
                if (Deadly_Calm.KnownSpell && Deadly_Calm.IsSpellUsable && MySettings.UseDeadlyCalm)
                {
                    Deadly_Calm.Launch();
                    Thread.Sleep(200);
                }
                Cleave.Launch();
                return;
            }
        }

        else
        {
            if (Heroic_Strike.KnownSpell && Heroic_Strike.IsSpellUsable && Heroic_Strike.IsDistanceGood
                && MySettings.UseHeroicStrike &&
                (ObjectManager.Me.RagePercentage > 80 || ObjectManager.Me.HaveBuff(122510)))
            {
                if (ObjectManager.Me.HealthPercent > 80)
                {
                    if (Deadly_Calm.KnownSpell && Deadly_Calm.IsSpellUsable && MySettings.UseDeadlyCalm)
                    {
                        Deadly_Calm.Launch();
                        Thread.Sleep(200);
                    }
                    Heroic_Strike.Launch();
                    return;
                }
            }
        }

        if (Shield_Slam.KnownSpell && Shield_Slam.IsSpellUsable && Shield_Slam.IsDistanceGood
            && MySettings.UseShieldSlam && ObjectManager.Me.RagePercentage < 95)
        {
            Shield_Slam.Launch();
            return;
        }
        else if (Revenge.KnownSpell && Revenge.IsDistanceGood && Revenge.IsSpellUsable
                 && MySettings.UseRevenge && ObjectManager.Me.RagePercentage < 95)
        {
            Revenge.Launch();
            return;
        }
        else if (Shockwave.KnownSpell && Shockwave.IsSpellUsable && Shockwave.IsDistanceGood
                 && MySettings.UseShockwave)
        {
            Shockwave.Launch();
            return;
        }
        else if (Dragon_Roar.KnownSpell && Dragon_Roar.IsSpellUsable && Dragon_Roar.IsDistanceGood
                 && MySettings.UseDragonRoar)
        {
            Shockwave.Launch();
            return;
        }
        else if (Bladestorm.KnownSpell && Bladestorm.IsSpellUsable && Bladestorm.IsDistanceGood
                 && MySettings.UseBladestorm)
        {
            Bladestorm.Launch();
            return;
        }
        else if (Thunder_Clap.KnownSpell && Thunder_Clap.IsSpellUsable && Thunder_Clap.IsDistanceGood
                 && MySettings.UseThunderClap && !ObjectManager.Target.HaveBuff(115798))
        {
            Thunder_Clap.Launch();
            return;
        }
        else if (Battle_Shout.KnownSpell && Battle_Shout.IsSpellUsable && MySettings.UseBattleShout)
        {
            Battle_Shout.Launch();
            return;
        }
        else if (Commanding_Shout.KnownSpell && Commanding_Shout.IsSpellUsable && MySettings.UseCommandingShout
                 && !MySettings.UseBattleShout)
        {
            Commanding_Shout.Launch();
            return;
        }
        else
        {
            if (Sunder_Armor.KnownSpell && Sunder_Armor.IsSpellUsable && Sunder_Armor.IsDistanceGood
                && MySettings.UseDevastate)
            {
                Sunder_Armor.Launch();
                return;
            }
        }
    }

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Heal();
            Buff();
        }
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (MySettings.UseDefensiveStance && Defensive_Stance.KnownSpell && Defensive_Stance.IsSpellUsable
            && !Defensive_Stance.HaveBuff && LC != 1)
        {
            Defensive_Stance.Launch();
            return;
        }
        else if (!Battle_Stance.HaveBuff && Battle_Stance.KnownSpell && Battle_Stance.IsSpellUsable
                 && MySettings.UseBattleStance && LC == 1)
        {
            Battle_Stance.Launch();
            return;
        }
        else if (!Berserker_Stance.HaveBuff && Berserker_Stance.KnownSpell && Berserker_Stance.IsSpellUsable
                 && MySettings.UseBerserkerStance && !MySettings.UseBattleStance && !MySettings.UseDefensiveStance)
        {
            Berserker_Stance.Launch();
            return;
        }
        else if (Battle_Shout.KnownSpell && Battle_Shout.IsSpellUsable && !Battle_Shout.HaveBuff
                 && MySettings.UseBattleShout)
        {
            Battle_Shout.Launch();
            return;
        }
        else
        {
            if (Commanding_Shout.KnownSpell && Commanding_Shout.IsSpellUsable && !Commanding_Shout.HaveBuff
                && MySettings.UseCommandingShout && !MySettings.UseBattleShout)
            {
                Commanding_Shout.Launch();
                return;
            }
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Victory_Rush.KnownSpell && Victory_Rush.IsSpellUsable && Victory_Rush.IsDistanceGood
            && MySettings.UseVictoryRush && ObjectManager.Me.HealthPercent < 90)
        {
            Victory_Rush.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 30 && Last_Stand.IsSpellUsable && Last_Stand.KnownSpell
                 && MySettings.UseLastStand && Fight.InFight)
        {
            Last_Stand.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 30 && Rallying_Cry.IsSpellUsable && Rallying_Cry.KnownSpell
                 && MySettings.UseRallyingCry && Fight.InFight && !Last_Stand.HaveBuff)
        {
            Rallying_Cry.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell
                 && MySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 80 && Enraged_Regeneration.IsSpellUsable &&
                Enraged_Regeneration.KnownSpell
                && MySettings.UseEnragedRegeneration)
            {
                Enraged_Regeneration.Launch();
                return;
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 95 && MySettings.UseDisarm && Disarm.IsDistanceGood
            && Disarm.KnownSpell && Disarm.IsSpellUsable && Disarm_Timer.IsReady)
        {
            Disarm.Launch();
            Disarm_Timer = new Timer(1000*60);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 20 && MySettings.UseIntimidatingShout
                 && Intimidating_Shout.KnownSpell && Intimidating_Shout.IsSpellUsable &&
                 ObjectManager.Target.GetDistance < 8)
        {
            Intimidating_Shout.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 60 && Shield_Wall.KnownSpell && Shield_Wall.IsSpellUsable
                 && MySettings.UseShieldWall)
        {
            Shield_Wall.Launch();
            OnCD = new Timer(1000*12);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseDemoralizingBanner
                 && Demoralizing_Banner.KnownSpell && Demoralizing_Banner.IsSpellUsable &&
                 ObjectManager.Target.GetDistance < 30)
        {
            SpellManager.CastSpellByIDAndPosition(114203, ObjectManager.Target.Position);
            OnCD = new Timer(1000*15);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 90 && MySettings.UseDemoralizingShout
                 && Demoralizing_Shout.KnownSpell && Demoralizing_Shout.IsSpellUsable &&
                 ObjectManager.Target.GetDistance < 30)
        {
            Demoralizing_Shout.Launch();
            OnCD = new Timer(1000*10);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && War_Stomp.IsSpellUsable && War_Stomp.KnownSpell
                 && MySettings.UseWarStomp)
        {
            War_Stomp.Launch();
            OnCD = new Timer(1000*2);
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
            if (ObjectManager.Me.HealthPercent < 80 && Shield_Block.KnownSpell && Shield_Block.IsSpellUsable
                && MySettings.UseShieldBlock)
            {
                Shield_Block.Launch();
                OnCD = new Timer(1000*6);
                return;
            }
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ObjectManager.Target.GetDistance < 8
            && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable && MySettings.UseArcaneTorrent)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else if (!Hamstring.TargetHaveBuff && MySettings.UseHamstring && Hamstring.KnownSpell
                 && Hamstring.IsSpellUsable && Hamstring.IsDistanceGood)
        {
            Hamstring.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && Pummel.IsDistanceGood
                 && Pummel.KnownSpell && Pummel.IsSpellUsable && MySettings.UsePummel)
        {
            Pummel.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe &&
                 ObjectManager.Target.GetDistance < 10
                 && Disrupting_Shout.KnownSpell && Disrupting_Shout.IsSpellUsable && MySettings.UseDisruptingShout)
        {
            Disrupting_Shout.Launch();
            return;
        }
        else if (ObjectManager.Target.GetMove && !Piercing_Howl.TargetHaveBuff && MySettings.UsePiercingHowl
                 && Piercing_Howl.KnownSpell && Piercing_Howl.IsSpellUsable && ObjectManager.Target.GetDistance < 15)
        {
            Piercing_Howl.Launch();
            return;
        }
        else if (Hamstring.TargetHaveBuff && MySettings.UseStaggeringShout && Staggering_Shout.KnownSpell
                 && Staggering_Shout.IsSpellUsable && ObjectManager.Target.GetDistance < 20)
        {
            Staggering_Shout.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && Spell_Reflection.KnownSpell && Spell_Reflection.IsSpellUsable
                 && MySettings.UseSpellReflection)
        {
            Spell_Reflection.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe &&
                 MySettings.UseMassSpellReflection
                 && Mass_Spell_Reflection.KnownSpell && Mass_Spell_Reflection.IsSpellUsable)
        {
            Mass_Spell_Reflection.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe
                && ObjectManager.Me.HealthPercent < 80 && Shield_Barrier.KnownSpell
                && Shield_Barrier.IsSpellUsable && MySettings.UseShieldBarrier && Shield_Barrier_Timer.IsReady)
            {
                Shield_Barrier.Launch();
                Shield_Barrier_Timer = new Timer(1000*6);
                return;
            }
        }
    }

    private void AvoidMelee()
    {
        while (ObjectManager.Target.GetDistance < 3 && ObjectManager.Target.InCombat)
        {
            nManager.Wow.Helpers.Keybindings.PressKeybindings(Keybindings.MOVEBACKWARD);
        }
    }
}

public class Warrior_Fury
{
    [Serializable]
    public class WarriorFurySettings : nManager.Helpful.Settings
    {
        /* Professions & Racials */
        public bool UseArcaneTorrent = true;
        public bool UseBerserking = true;
        public bool UseBloodFury = true;
        public bool UseLifeblood = true;
        public bool UseStoneform = true;
        public bool UseGiftoftheNaaru = true;
        public bool UseWarStomp = true;
        /* Warrior Buffs */
        public bool UseBattleShout = true;
        public bool UseBattleStance = true;
        public bool UseBerserkerStance = false;
        public bool UseCommandingShout = false;
        public bool UseDefensiveStance = true;
        /* Offensive Spell */
        public bool UseAvatar = true;
        public bool UseBladestorm = true;
        public bool UseBloodbath = true;
        public bool UseBloodthirst = true;
        public bool UseCharge = true;
        public bool UseCleave = true;
        public bool UseColossusSmash = true;
        public bool UseDragonRoar = true;
        public bool UseExecute = true;
        public bool UseHeroicLeap = true;
        public bool UseHeroicStrike = true;
        public bool UseHeroicThrow = true;
        public bool UseRagingBlow = true;
        public bool UseShockwave = true;
        public bool UseStormBolt = true;
        public bool UseTaunt = true;
        public bool UseThunderClap = true;
        public bool UseWhirlwind = true;
        public bool UseWildStrike = true;
        /* Offensive Cooldown */
        public bool UseBerserkerRage = true;
        public bool UseDeadlyCalm = true;
        public bool UseRecklessness = true;
        public bool UseShatteringThrow = true;
        public bool UseSweepingStrikes = true;
        public bool UseSkullBanner = true;
        /* Defensive Cooldown */
        public bool UseDemoralizingBanner = true;
        public bool UseDiebytheSword = true;
        public bool UseDisarm = true;
        public bool UseDisruptingShout = true;
        public bool UseHamstring = false;
        public bool UseIntimidatingShout = true;
        public bool UseMassSpellReflection = true;
        public bool UsePiercingHowl = false;
        public bool UsePummel = true;
        public bool UseStaggeringShout = true;
        /* Healing Spell */
        public bool UseEnragedRegeneration = true;
        public bool UseRallyingCry = true;
        public bool UseVictoryRush = true;
        /* Game Settings */
        public bool UseLowCombat = true;
        public bool UseTrinket = true;
        public bool UseEngGlove = true;
        public bool UseAlchFlask = true;

        public WarriorFurySettings()
        {
            ConfigWinForm(new System.Drawing.Point(400, 400), "Warrior Fury Settings");
            /* Professions & Racials */
            AddControlInWinForm("Use Arcane Torrent", "UseArcaneTorrent", "Professions & Racials");
            AddControlInWinForm("Use Berserking", "UseBerserking", "Professions & Racials");
            AddControlInWinForm("Use Blood Fury", "UseBloodFury", "Professions & Racials");
            AddControlInWinForm("Use Gift of the Naaru", "UseGiftoftheNaaru", "Professions & Racials");
            AddControlInWinForm("Use Lifeblood", "UseLifeblood", "Professions & Racials");
            AddControlInWinForm("Use Stoneform", "UseStoneform", "Professions & Racials");
            AddControlInWinForm("Use War Stomp", "UseWarStomp", "Professions & Racials");
            /* Warrior Buffs */
            AddControlInWinForm("Use Battle Shout", "UseBattleShout", "Warrior Buffs");
            AddControlInWinForm("Use Battle Stance", "UseBattleStance", "Warrior Buffs");
            AddControlInWinForm("Use Berserker Stance", "UseBerserkerStance", "Warrior Buffs");
            AddControlInWinForm("Use Commanding Shout", "UseCommandingShout", "Warrior Buffs");
            AddControlInWinForm("Use Defensive Stance", "UseDefensiveStance", "Warrior Buffs");
            /* Offensive Spell */
            AddControlInWinForm("Use Avatar", "UseAvatar", "Offensive Spell");
            AddControlInWinForm("Use Bladestorm", "UseBladestorm", "Offensive Spell");
            AddControlInWinForm("Use Bloodbath", "UseBloodbath", "Offensive Spell");
            AddControlInWinForm("Use Bloodthirst", "UseBloodthirst", "Offensive Spell");
            AddControlInWinForm("Use Charge", "UseCharge", "Offensive Spell");
            AddControlInWinForm("Use Cleave", "UseCleave", "Offensive Spell");
            AddControlInWinForm("Use Colossus Smash", "UseColossusSmash", "Offensive Spell");
            AddControlInWinForm("Use Dragon Roar", "UseDragonRoar", "Offensive Spell");
            AddControlInWinForm("Use Exectue", "UseExecute", "Offensive Spell");
            AddControlInWinForm("Use Heroic Leap", "UseHeroicLeap", "Offensive Spell");
            AddControlInWinForm("Use Heroic Strike", "UseHeroicStrike", "Offensive Spell");
            AddControlInWinForm("Use Heroic Throw", "UseHeroicThrow", "Offensive Spell");
            AddControlInWinForm("Use Raging Blow", "UseRagingBlow", "Offensive Spell");
            AddControlInWinForm("Use Shockwave", "UseShockwave", "Offensive Spell");
            AddControlInWinForm("Use Storm Bolt", "UseStormBolt", "Offensive Spell");
            AddControlInWinForm("Use Taunt", "UseTaunt", "Offensive Spell");
            AddControlInWinForm("Use Thunder Clap", "UseThunderClap", "Offensive Spell");
            AddControlInWinForm("Use Whirlwind", "UseWhirlwind", "Offensive Spell");
            AddControlInWinForm("Use Wild Strike", "UseWildStrike", "Offensive Spell");
            /* Offensive Cooldown */
            AddControlInWinForm("Use Berserker Rage", "UseBerserkerRage", "Offensive Cooldown");
            AddControlInWinForm("Use Deadly Calm", "UseDeadlyCalm", "Offensive Cooldown");
            AddControlInWinForm("Use Recklessness", "UseRecklessness", "Offensive Cooldown");
            AddControlInWinForm("Use Shattering Throw", "UseShatteringThrow", "Offensive Cooldown");
            AddControlInWinForm("Use Sweeping Strikes", "UseSweepingStrikes", "Offensive Cooldown");
            AddControlInWinForm("Use Skull Banner", "UseSkullBanner", "Offensive Cooldown");
            /* Defensive Cooldown */
            AddControlInWinForm("Use Demoralizing Banner", "UseDemoralizingBanner", "Defensive Cooldown");
            AddControlInWinForm("Use Die by the Sword", "UseDiebytheSword", "Defensive Cooldown");
            AddControlInWinForm("Use Disarm", "UseDisarm", "Defensive Cooldown");
            AddControlInWinForm("Use Disrupting Shout", "UseDisruptingShout", "Defensive Cooldown");
            AddControlInWinForm("Use Hamstring", "UseHamstring", "Defensive Cooldown");
            AddControlInWinForm("Use Intimidating Shout", "UseIntimidatingShout", "Defensive Cooldown");
            AddControlInWinForm("Use Mass Spell Reflection", "UseMassSpellReflection", "Defensive Cooldown");
            AddControlInWinForm("Use Piercing Howl", "UsePiercingHowl", "Defensive Cooldown");
            AddControlInWinForm("Use Pummel", "UsePummel", "Defensive Cooldown");
            AddControlInWinForm("Use Staggering Shout", "UseStaggeringShout", "Defensive Cooldown");
            /* Healing Spell */
            AddControlInWinForm("Use Enraged Regeneration", "UseEnragedRegeneration", "Healing Spell");
            AddControlInWinForm("Use Rallying Cry", "UseRallyingCry", "Healing Spell");
            AddControlInWinForm("Use Victory Rush", "UseVictoryRush", "Healing Spell");
            /* Game Settings */
            AddControlInWinForm("Use Low Combat Settings", "UseLowCombat", "Game Settings");
            AddControlInWinForm("Use Trinket", "UseTrinket", "Game Settings");
            AddControlInWinForm("Use Engineering Gloves", "UseEngGlove", "Game Settings");
            AddControlInWinForm("Use Alchemist Flask", "UseAlchFlask", "Game Settings");
        }

        public static WarriorFurySettings CurrentSetting { get; set; }

        public static WarriorFurySettings GetSettings()
        {
            string CurrentSettingsFile = Application.StartupPath + "\\CustomClasses\\Settings\\Warrior_Fury.xml";
            if (System.IO.File.Exists(CurrentSettingsFile))
            {
                return
                    CurrentSetting = Settings.Load<Warrior_Fury.WarriorFurySettings>(CurrentSettingsFile);
            }
            else
            {
                return new Warrior_Fury.WarriorFurySettings();
            }
        }
    }

    private readonly WarriorFurySettings MySettings = WarriorFurySettings.GetSettings();

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

    #region Warrior Buffs

    private readonly Spell Battle_Shout = new Spell("Battle Shout");
    private readonly Spell Battle_Stance = new Spell("Battle Stance");
    private readonly Spell Berserker_Stance = new Spell("Berserker Stance");
    private readonly Spell Commanding_Shout = new Spell("Commanding Shout");
    private readonly Spell Defensive_Stance = new Spell("Defensive Stance");

    #endregion

    #region Offensive Spell

    private readonly Spell Avatar = new Spell("Avatar");
    private readonly Spell Bladestorm = new Spell("Bladestorm");
    private readonly Spell Bloodbath = new Spell("Bloodbath");
    private readonly Spell Bloodthirst = new Spell("Bloodthirst");
    private readonly Spell Charge = new Spell("Charge");
    private readonly Spell Cleave = new Spell("Cleave");
    private readonly Spell Colossus_Smash = new Spell("Colossus Smash");
    private readonly Spell Dragon_Roar = new Spell("Dragon Roar");
    private readonly Spell Execute = new Spell("Execute");
    private readonly Spell Heroic_Leap = new Spell("Heroic Leap");
    private readonly Spell Heroic_Strike = new Spell("Heroic Strike");
    private readonly Spell Heroic_Throw = new Spell("Heroic Throw");
    private readonly Spell Impending_Victory = new Spell("Impending Victory");
    private readonly Spell Raging_Blow = new Spell("Raging Blow");
    private readonly Spell Shockwave = new Spell("Shockwave");
    private readonly Spell Storm_Bolt = new Spell("Storm Bolt");
    private readonly Spell Taunt = new Spell("Taunt");
    private readonly Spell Thunder_Clap = new Spell("Thunder Clap");
    private readonly Spell Whirlwind = new Spell("Whirlwind");
    private readonly Spell Wild_Strike = new Spell("Wild Strike");

    #endregion

    #region Offensive Cooldown

    private readonly Spell Berserker_Rage = new Spell("Berserker Rage");
    private readonly Spell Deadly_Calm = new Spell("Deadly Calm");
    private readonly Spell Recklessness = new Spell("Recklessness");
    private readonly Spell Shattering_Throw = new Spell("Shattering Throw");
    private readonly Spell Sweeping_Strikes = new Spell("Sweeping Strikes");
    private readonly Spell Skull_Banner = new Spell("Skull Banner");

    #endregion

    #region Defensive Cooldown

    private readonly Spell Die_by_the_Sword = new Spell("Die by the Sword");
    private readonly Spell Disarm = new Spell("Disarm");
    private Timer Disarm_Timer = new Timer(0);
    private readonly Spell Disrupting_Shout = new Spell("Disrupting Shout");
    private readonly Spell Hamstring = new Spell("Hamstring");
    private readonly Spell Intimidating_Shout = new Spell("Intimidating Shout");
    private readonly Spell Mass_Spell_Reflection = new Spell("Mass Spell Reflection");
    private readonly Spell Piercing_Howl = new Spell("Piercing Howl");
    private readonly Spell Pummel = new Spell("Pummel");
    private readonly Spell Staggering_Shout = new Spell("Staggering Shout");
    private readonly Spell Demoralizing_Banner = new Spell("Demoralizing Banner");

    #endregion

    #region Healing Spell

    private readonly Spell Enraged_Regeneration = new Spell("Enraged Regeneration");
    private readonly Spell Rallying_Cry = new Spell("Rallying Cry");
    private readonly Spell Victory_Rush = new Spell("Victory Rush");

    #endregion

    private Timer OnCD = new Timer(0);
    private Timer Trinket_Timer = new Timer(0);
    private Timer Engineering_Timer = new Timer(0);
    private Timer AlchFlask_Timer = new Timer(0);
    public int LC = 0;

    public Warrior_Fury()
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
                            Taunt.IsDistanceGood)
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
        if (Heroic_Leap.IsDistanceGood && Heroic_Leap.KnownSpell && Heroic_Leap.IsSpellUsable
            && MySettings.UseHeroicLeap)
        {
            SpellManager.CastSpellByIDAndPosition(6544, ObjectManager.Target.Position);
            Thread.Sleep(200);
        }

        if (Taunt.IsDistanceGood && Taunt.KnownSpell && Taunt.IsSpellUsable
            && MySettings.UseTaunt && ObjectManager.Target.GetDistance > 20)
        {
            Taunt.Launch();
            return;
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

    public void LowCombat()
    {
        AvoidMelee();
        Heal();
        Defense_Cycle();
        Buff();

        if (Heroic_Throw.KnownSpell && Heroic_Throw.IsSpellUsable && Heroic_Throw.IsDistanceGood
            && MySettings.UseHeroicThrow && !ObjectManager.Target.InCombat)
        {
            Heroic_Throw.Launch();
            return;
        }

        if (Charge.KnownSpell && Charge.IsSpellUsable && Charge.IsDistanceGood
            && MySettings.UseCharge && ObjectManager.Target.GetDistance > 14)
        {
            Charge.Launch();
            return;
        }

        if (Bloodthirst.KnownSpell && Bloodthirst.IsSpellUsable && Bloodthirst.IsDistanceGood
            && MySettings.UseBloodthirst)
        {
            Bloodthirst.Launch();
            return;
        }
        else if (Colossus_Smash.KnownSpell && Colossus_Smash.IsDistanceGood && Colossus_Smash.IsSpellUsable
                 && MySettings.UseColossusSmash)
        {
            Colossus_Smash.Launch();
            return;
        }
        else if (Heroic_Strike.KnownSpell && Heroic_Strike.IsSpellUsable && Heroic_Strike.IsDistanceGood
                 && MySettings.UseHeroicStrike && ObjectManager.GetNumberAttackPlayer() < 3
                 && ObjectManager.Me.RagePercentage > 80)
        {
            if (Deadly_Calm.KnownSpell && Deadly_Calm.IsSpellUsable && MySettings.UseDeadlyCalm)
            {
                Deadly_Calm.Launch();
                Thread.Sleep(200);
            }

            Heroic_Strike.Launch();
            return;
        }
        else if (Shockwave.KnownSpell && Shockwave.IsSpellUsable && ObjectManager.Target.GetDistance < 10
                 && MySettings.UseShockwave)
        {
            Shockwave.Launch();
            return;
        }
        else if (Dragon_Roar.KnownSpell && Dragon_Roar.IsSpellUsable && ObjectManager.Target.GetDistance < 8
                 && MySettings.UseDragonRoar)
        {
            Dragon_Roar.Launch();
            return;
        }
        else
        {
            if (Bladestorm.KnownSpell && Bladestorm.IsSpellUsable && ObjectManager.Target.GetDistance < 8
                && MySettings.UseBladestorm)
            {
                Bladestorm.Launch();
                return;
            }
        }

        if (Thunder_Clap.KnownSpell && Thunder_Clap.IsSpellUsable && Thunder_Clap.IsDistanceGood
            && MySettings.UseThunderClap)
        {
            Thunder_Clap.Launch();
            return;
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
        else if (Berserker_Rage.KnownSpell && Berserker_Rage.IsSpellUsable && ObjectManager.Me.RagePercentage < 50
                 && MySettings.UseBerserkerRage && ObjectManager.Target.GetDistance < 30)
        {
            Berserker_Rage.Launch();
            return;
        }
        else if (Battle_Shout.KnownSpell && Battle_Shout.IsSpellUsable && ObjectManager.Me.RagePercentage < 80
                 && MySettings.UseBattleShout && ObjectManager.Target.GetDistance < 30)
        {
            Battle_Shout.Launch();
            return;
        }
        else if (Commanding_Shout.KnownSpell && Commanding_Shout.IsSpellUsable && ObjectManager.Me.RagePercentage < 80
                 && MySettings.UseCommandingShout && !MySettings.UseBattleShout && ObjectManager.Target.GetDistance < 30)
        {
            Commanding_Shout.Launch();
            return;
        }
        else if (Recklessness.KnownSpell && Recklessness.IsSpellUsable && MySettings.UseRecklessness
                 && ObjectManager.Target.GetDistance < 30)
        {
            Recklessness.Launch();
            return;
        }
        else if (Shattering_Throw.KnownSpell && Shattering_Throw.IsSpellUsable && Shattering_Throw.IsDistanceGood
                 && MySettings.UseShatteringThrow)
        {
            Shattering_Throw.Launch();
            return;
        }
        else if (Skull_Banner.KnownSpell && Skull_Banner.IsSpellUsable
                 && MySettings.UseSkullBanner && ObjectManager.Target.GetDistance < 30)
        {
            Skull_Banner.Launch();
            return;
        }
        else if (Avatar.KnownSpell && Avatar.IsSpellUsable
                 && MySettings.UseAvatar && ObjectManager.Target.GetDistance < 30)
        {
            Avatar.Launch();
            return;
        }
        else if (Bloodbath.KnownSpell && Bloodbath.IsSpellUsable
                 && MySettings.UseBloodbath && ObjectManager.Target.GetDistance < 30)
        {
            Bloodbath.Launch();
            return;
        }
        else
        {
            if (Storm_Bolt.KnownSpell && Storm_Bolt.IsSpellUsable
                && MySettings.UseStormBolt && Storm_Bolt.IsDistanceGood)
            {
                Storm_Bolt.Launch();
                return;
            }
        }
    }

    public void DPS_Cycle()
    {
        if (Heroic_Throw.KnownSpell && Heroic_Throw.IsSpellUsable && Heroic_Throw.IsDistanceGood
            && MySettings.UseHeroicThrow && !ObjectManager.Target.InCombat)
        {
            Heroic_Throw.Launch();
            return;
        }

        if (Charge.KnownSpell && Charge.IsSpellUsable && Charge.IsDistanceGood
            && MySettings.UseCharge && ObjectManager.Target.GetDistance > 14)
        {
            Charge.Launch();
            return;
        }

        if (Victory_Rush.KnownSpell && Victory_Rush.IsSpellUsable && Victory_Rush.IsDistanceGood
            && MySettings.UseVictoryRush && ObjectManager.Me.HealthPercent < 90)
        {
            Victory_Rush.Launch();
            return;
        }
        else if (Whirlwind.KnownSpell && Whirlwind.IsSpellUsable && ObjectManager.GetNumberAttackPlayer() > 3
                 && MySettings.UseWhirlwind)
        {
            Whirlwind.Launch();
            return;
        }
        else if (Cleave.KnownSpell && Cleave.IsSpellUsable && Cleave.IsDistanceGood && MySettings.UseCleave
                 && ObjectManager.GetNumberAttackPlayer() > 1 && ObjectManager.GetNumberAttackPlayer() < 4)
        {
            if (Deadly_Calm.KnownSpell && Deadly_Calm.IsSpellUsable && MySettings.UseDeadlyCalm)
            {
                Deadly_Calm.Launch();
                Thread.Sleep(200);
            }

            Cleave.Launch();
            return;
        }
        else if (Heroic_Strike.KnownSpell && Heroic_Strike.IsSpellUsable && Heroic_Strike.IsDistanceGood
                 && MySettings.UseHeroicStrike && ObjectManager.GetNumberAttackPlayer() < 3
                 && ObjectManager.Me.RagePercentage > 80)
        {
            if (Deadly_Calm.KnownSpell && Deadly_Calm.IsSpellUsable && MySettings.UseDeadlyCalm)
            {
                Deadly_Calm.Launch();
                Thread.Sleep(200);
            }

            Heroic_Strike.Launch();
            return;
        }
        else if (Shockwave.KnownSpell && Shockwave.IsSpellUsable && ObjectManager.Target.GetDistance < 10
                 && MySettings.UseShockwave)
        {
            Shockwave.Launch();
            return;
        }
        else if (Dragon_Roar.KnownSpell && Dragon_Roar.IsSpellUsable && ObjectManager.Target.GetDistance < 8
                 && MySettings.UseDragonRoar)
        {
            Dragon_Roar.Launch();
            return;
        }
        else if (Bladestorm.KnownSpell && Bladestorm.IsSpellUsable && ObjectManager.Target.GetDistance < 8
                 && MySettings.UseBladestorm)
        {
            Bladestorm.Launch();
            return;
        }
        else if (Bloodthirst.KnownSpell && Bloodthirst.IsSpellUsable && Bloodthirst.IsDistanceGood
                 && MySettings.UseBloodthirst)
        {
            Bloodthirst.Launch();
            return;
        }
        else if (Colossus_Smash.KnownSpell && Colossus_Smash.IsSpellUsable && Colossus_Smash.IsDistanceGood
                 && MySettings.UseColossusSmash)
        {
            Colossus_Smash.Launch();
            return;
        }
        else if (Execute.KnownSpell && Execute.IsSpellUsable && Execute.IsDistanceGood
                 && MySettings.UseExecute && ObjectManager.GetNumberAttackPlayer() < 4)
        {
            Execute.Launch();
            return;
        }
        else if (Raging_Blow.KnownSpell && Raging_Blow.IsSpellUsable && Raging_Blow.IsDistanceGood
                 && MySettings.UseRagingBlow)
        {
            Raging_Blow.Launch();
            return;
        }
        else if (Wild_Strike.KnownSpell && Wild_Strike.IsSpellUsable && Wild_Strike.IsDistanceGood
                 && MySettings.UseWildStrike && ObjectManager.Me.HaveBuff(46915))
        {
            Wild_Strike.Launch();
            return;
        }
        else
        {
            if (Heroic_Throw.KnownSpell && Heroic_Throw.IsSpellUsable && Heroic_Throw.IsDistanceGood
                && MySettings.UseHeroicThrow)
            {
                Heroic_Throw.Launch();
                return;
            }
        }
    }

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Heal();
            Buff();
        }
    }

    private void Buff()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (ObjectManager.Me.HealthPercent < 30 && MySettings.UseDefensiveStance
            && Defensive_Stance.KnownSpell && Defensive_Stance.IsSpellUsable && !Defensive_Stance.HaveBuff)
        {
            Defensive_Stance.Launch();
            return;
        }
        else if (!Battle_Stance.HaveBuff && Battle_Stance.KnownSpell && Battle_Stance.IsSpellUsable
                 && MySettings.UseBattleStance && ObjectManager.Me.HealthPercent > 50)
        {
            Battle_Stance.Launch();
            return;
        }
        else if (!Berserker_Stance.HaveBuff && Berserker_Stance.KnownSpell && Berserker_Stance.IsSpellUsable
                 && MySettings.UseBerserkerStance && !MySettings.UseBattleStance && ObjectManager.Me.HealthPercent > 50)
        {
            Berserker_Stance.Launch();
            return;
        }
        else if (Battle_Shout.KnownSpell && Battle_Shout.IsSpellUsable && !Battle_Shout.HaveBuff
                 && MySettings.UseBattleShout)
        {
            Battle_Shout.Launch();
            return;
        }
        else
        {
            if (Commanding_Shout.KnownSpell && Commanding_Shout.IsSpellUsable && !Commanding_Shout.HaveBuff
                && MySettings.UseCommandingShout && !MySettings.UseBattleShout)
            {
                Commanding_Shout.Launch();
                return;
            }
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.IsMounted)
            return;

        if (Victory_Rush.KnownSpell && Victory_Rush.IsSpellUsable && Victory_Rush.IsDistanceGood
            && MySettings.UseVictoryRush && ObjectManager.Me.HealthPercent < 90)
        {
            Victory_Rush.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 30 && Rallying_Cry.IsSpellUsable && Rallying_Cry.KnownSpell
                 && MySettings.UseRallyingCry && Fight.InFight)
        {
            Rallying_Cry.Launch();
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && Gift_of_the_Naaru.IsSpellUsable && Gift_of_the_Naaru.KnownSpell
                 && MySettings.UseGiftoftheNaaru)
        {
            Gift_of_the_Naaru.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 80 && Enraged_Regeneration.IsSpellUsable &&
                Enraged_Regeneration.KnownSpell
                && MySettings.UseEnragedRegeneration)
            {
                Enraged_Regeneration.Launch();
                return;
            }
        }
    }

    private void Defense_Cycle()
    {
        if (ObjectManager.Me.HealthPercent < 95 && MySettings.UseDisarm && Disarm.IsDistanceGood
            && Disarm.KnownSpell && Disarm.IsSpellUsable && Disarm_Timer.IsReady)
        {
            Disarm.Launch();
            Disarm_Timer = new Timer(1000*60);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 20 && MySettings.UseIntimidatingShout
                 && Intimidating_Shout.KnownSpell && Intimidating_Shout.IsSpellUsable &&
                 ObjectManager.Target.GetDistance < 8)
        {
            Intimidating_Shout.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseDiebytheSword
                 && Die_by_the_Sword.KnownSpell && Die_by_the_Sword.IsSpellUsable)
        {
            Die_by_the_Sword.Launch();
            OnCD = new Timer(1000*8);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && MySettings.UseDemoralizingBanner
                 && Demoralizing_Banner.KnownSpell && Demoralizing_Banner.IsSpellUsable &&
                 ObjectManager.Target.GetDistance < 30)
        {
            SpellManager.CastSpellByIDAndPosition(114203, ObjectManager.Target.Position);
            OnCD = new Timer(1000*15);
            return;
        }
        else if (ObjectManager.Me.HealthPercent < 80 && War_Stomp.IsSpellUsable && War_Stomp.KnownSpell
                 && MySettings.UseWarStomp)
        {
            War_Stomp.Launch();
            OnCD = new Timer(1000*2);
            return;
        }
        else
        {
            if (ObjectManager.Me.HealthPercent < 80 && Stoneform.IsSpellUsable && Stoneform.KnownSpell
                && MySettings.UseStoneform)
            {
                Stoneform.Launch();
                OnCD = new Timer(1000*8);
                return;
            }
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && ObjectManager.Target.GetDistance < 8
            && Arcane_Torrent.KnownSpell && Arcane_Torrent.IsSpellUsable && MySettings.UseArcaneTorrent)
        {
            Arcane_Torrent.Launch();
            return;
        }
        else if (!Hamstring.TargetHaveBuff && MySettings.UseHamstring && Hamstring.KnownSpell
                 && Hamstring.IsSpellUsable && Hamstring.IsDistanceGood)
        {
            Hamstring.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe && Pummel.IsDistanceGood
                 && Pummel.KnownSpell && Pummel.IsSpellUsable && MySettings.UsePummel)
        {
            Pummel.Launch();
            return;
        }
        else if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe &&
                 ObjectManager.Target.GetDistance < 10
                 && Disrupting_Shout.KnownSpell && Disrupting_Shout.IsSpellUsable && MySettings.UseDisruptingShout)
        {
            Disrupting_Shout.Launch();
            return;
        }
        else if (ObjectManager.Target.GetMove && !Piercing_Howl.TargetHaveBuff && MySettings.UsePiercingHowl
                 && Piercing_Howl.KnownSpell && Piercing_Howl.IsSpellUsable && ObjectManager.Target.GetDistance < 15)
        {
            Piercing_Howl.Launch();
            return;
        }
        else if (Hamstring.TargetHaveBuff && MySettings.UseStaggeringShout && Staggering_Shout.KnownSpell
                 && Staggering_Shout.IsSpellUsable && ObjectManager.Target.GetDistance < 20)
        {
            Staggering_Shout.Launch();
            return;
        }
        else
        {
            if (ObjectManager.Target.IsCast && ObjectManager.Target.IsTargetingMe &&
                MySettings.UseMassSpellReflection
                && Mass_Spell_Reflection.KnownSpell && Mass_Spell_Reflection.IsSpellUsable)
            {
                Mass_Spell_Reflection.Launch();
                return;
            }
        }
    }

    private void AvoidMelee()
    {
        while (ObjectManager.Target.GetDistance < 3 && ObjectManager.Target.InCombat)
        {
            nManager.Wow.Helpers.Keybindings.PressKeybindings(Keybindings.MOVEBACKWARD);
        }
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
            Lua.RunMacroText("/cast [@pet] IrrefÃ¼hrung");
            Lua.RunMacroText("/cast [@pet] DÃ©tournement");
            Lua.RunMacroText("/cast [@pet] ÐŸÐµÑ€ÐµÐ½Ð°Ð¿Ñ€Ð°Ð²Ð»ÐµÐ½Ð¸Ðµ");
            Lua.RunMacroText("/cast [@pet] RedirecciÃ³n");
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

                nManager.Wow.Helpers.Keybindings.PressKeybindings(Keybindings.JUMP);
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
            Lua.RunMacroText("/cast [@pet] IrrefÃ¼hrung");
            Lua.RunMacroText("/cast [@pet] DÃ©tournement");
            Lua.RunMacroText("/cast [@pet] ÐŸÐµÑ€ÐµÐ½Ð°Ð¿Ñ€Ð°Ð²Ð»ÐµÐ½Ð¸Ðµ");
            Lua.RunMacroText("/cast [@pet] RedirecciÃ³n");
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

                nManager.Wow.Helpers.Keybindings.PressKeybindings(Keybindings.JUMP);
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
            if (File.Exists(CurrentSettingsFile))
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
            Lua.RunMacroText("/cast [@pet] IrrefÃ¼hrung");
            Lua.RunMacroText("/cast [@pet] DÃ©tournement");
            Lua.RunMacroText("/cast [@pet] ÐŸÐµÑ€ÐµÐ½Ð°Ð¿Ñ€Ð°Ð²Ð»ÐµÐ½Ð¸Ðµ");
            Lua.RunMacroText("/cast [@pet] RedirecciÃ³n");
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

                nManager.Wow.Helpers.Keybindings.PressKeybindings(Keybindings.JUMP);
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