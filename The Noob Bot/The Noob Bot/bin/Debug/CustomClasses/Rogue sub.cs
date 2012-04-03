using System;
using System.Collections.Generic;
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
    #region Initialize_CustomClass

    internal static float range = 3.5f;
    internal static bool loop = true;

    public float Range
    {
        get { return range; }
        set { range = value; }
    }

    public void ShowConfiguration()
    {
        MessageBox.Show("No setting for this custom class.");
    }

    public void Initialize()
    {
        try
        {
            Logging.WriteFight("Loading My Easy Custom Class" + ObjectManager.Me.WowClass);

            MyCc cc = new MyCc(ObjectManager.Me.WowClass);
        }
        catch (Exception)
        {
        }
        Logging.WriteFight("My Easy Custom Class closed.");
    }

    #endregion Initialize_CustomClass

    #region Dispose_CustomClass

    public void Dispose()
    {
        Logging.WriteFight("Closing My Easy Custom Class");
        loop = false;
    }

    #endregion Dispose_CustomClass
}

public class MyCc
{
    private WoWClass _myClass = WoWClass.None;
    private MyEasyCustomClassStruct.SpellManager _spellManager = new MyEasyCustomClassStruct.SpellManager();

    public MyCc(WoWClass myClass)
    {
        // Load xml config:
        _myClass = myClass;
        _spellManager = MyEasyCustomClassStruct.SpellManager.LoadFile(_myClass);

        // Range:
        Main.range = _spellManager.range;

        // Inizialize spell:
        foreach (MyEasyCustomClassStruct.SpellClass s in _spellManager.Spells)
        {
            if (s.Type == MyEasyCustomClassStruct.TypeParam.Spell || s.Type == MyEasyCustomClassStruct.TypeParam.Buff)
                s.spell = new Spell(s.Name);
        }

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
                        if (ObjectManager.Me.Target != lastTarget)
                        {
                            Pull();
                            lastTarget = ObjectManager.Me.Target;
                        }

                        Combat();
                    }
                }
            }
            catch (Exception)
            {
            }
            Thread.Sleep(500);
        }
    }

    private void Pull()
    {
        foreach (MyEasyCustomClassStruct.SpellClass s in _spellManager.Spells)
        {
            try
            {
                if (s.When == MyEasyCustomClassStruct.WhenParam.Pull)
                {
                    if (s.ParamsIsGood())
                    {
                        s.LaunchOrUse();
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }

    private void Combat()
    {
        foreach (MyEasyCustomClassStruct.SpellClass s in _spellManager.Spells)
        {
            try
            {
                if (s.When == MyEasyCustomClassStruct.WhenParam.Combat)
                {
                    if (s.ParamsIsGood())
                    {
                        s.LaunchOrUse();
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }

    private void Patrolling()
    {
        Timer Main_Poison_Timer = new Timer(0);
        Timer Off_Poison_Timer = new Timer(0);
        if (!ObjectManager.Me.IsMounted)
        {
            if (Main_Poison_Timer.IsReady && !ObjectManager.Me.IsMounted)
            {
                nManager.Wow.Helpers.Keybindings.PressBarAndSlotKey("1;7"); // Bar 1 ; Slot 7
                Thread.Sleep(4000); // Attandre 4 sec
                Main_Poison_Timer = new Timer(1000 * 3600); // On lance le timer pour qu'il soit pret dans 3600 sec
            }
            if (Off_Poison_Timer.IsReady && !ObjectManager.Me.IsMounted)
            {
                nManager.Wow.Helpers.Keybindings.PressBarAndSlotKey("1;8"); // Bar 1 ; Slot 8
                Thread.Sleep(4000); // Attandre 4 sec
                Off_Poison_Timer = new Timer(1000 * 3600); // 3600 sec
            }
            foreach (MyEasyCustomClassStruct.SpellClass s in _spellManager.Spells)
            {
                try
                {
                    if (s.When == MyEasyCustomClassStruct.WhenParam.Patrolling)
                    {
                        if (s.ParamsIsGood())
                        {
                            s.LaunchOrUse();
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }
    }
}

namespace MyEasyCustomClassStruct
{
    [Serializable]
    public class SpellManager
    {
        public List<SpellClass> Spells = new List<SpellClass>();
        public float range = 3.5f;

        internal static SpellManager LoadFile(WoWClass woWClass)
        {
            try
            {
                return Others.ExistFile(Application.StartupPath + "\\CustomClasses\\MyEasyCustomClass\\" + woWClass + ".xml") ? XmlSerializer.Deserialize<SpellManager>(Application.StartupPath + "\\CustomClasses\\MyEasyCustomClass\\" + woWClass + ".xml") : null;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e);
                return new SpellManager();
            }
        }

        internal static void SaveFile(WoWClass woWClass, SpellManager spellManager)
        {
            try
            {
                XmlSerializer.Serialize(Application.StartupPath + "\\CustomClasses\\MyEasyCustomClass\\" + woWClass + ".xml", spellManager);
            }
            catch (Exception e)
            {
                Logging.WriteFight(e.ToString());
            }
        }
    }

    [Serializable]
    public class SpellClass
    {
        public string Name = "";
        public List<Param> Params = new List<Param>();
        public TypeParam Type = TypeParam.Spell;
        public WhenParam When = WhenParam.Combat;
        internal int lastUse;
        internal Spell spell;

        internal bool ParamsIsGood()
        {
            try
            {
                // If spell:
                if (Type == TypeParam.Spell)
                {
                    if (!spell.KnownSpell ||
                        !spell.IsDistanceGood ||
                        !spell.IsSpellUsable)
                    {
                        return false;
                    }
                }
                else if (Type == TypeParam.Buff)
                {
                    if (!spell.KnownSpell ||
                        spell.HaveBuff ||
                        !spell.IsSpellUsable)
                    {
                        return false;
                    }
                }
            }
            catch
            {
            }
            foreach (Param p in Params)
            {
                try
                {
                    // ParamsPram:
                    switch (p.param)
                    {
                        case ParamsPram.Maximum_Health_Percent:
                            if (ObjectManager.Me.HealthPercent > Convert.ToInt32(p.value))
                                return false;
                            break;
                        case ParamsPram.Maximum_Pet_Health_Percent:
                            if (ObjectManager.Pet.HealthPercent > Convert.ToInt32(p.value))
                                return false;
                            break;
                        case ParamsPram.Maximum_Target_Health_Percent:
                            if (ObjectManager.Target.HealthPercent > Convert.ToInt32(p.value))
                                return false;
                            break;
                        case ParamsPram.Minimum_Health_Percent:
                            if (ObjectManager.Me.HealthPercent < Convert.ToInt32(p.value))
                                return false;
                            break;
                        case ParamsPram.Minimum_Pet_Health_Percent:
                            if (ObjectManager.Pet.HealthPercent < Convert.ToInt32(p.value))
                                return false;
                            break;
                        case ParamsPram.Minimum_Target_Health_Percent:
                            if (ObjectManager.Target.HealthPercent < Convert.ToInt32(p.value))
                                return false;
                            break;
                        case ParamsPram.Required_Buff_Name:
                            if (!ObjectManager.Me.HaveBuff(nManager.Wow.Helpers.SpellManager.SpellListManager.SpellIdByName(p.value)))
                                return false;
                            break;
                        case ParamsPram.Required_Knows_Spell_Name:
                            if (!nManager.Wow.Helpers.SpellManager.SpellBookName().Contains(p.value))
                                return false;
                            break;
                        case ParamsPram.Required_Not_Knows_Spell_Name:
                            if (nManager.Wow.Helpers.SpellManager.SpellBookName().Contains(p.value))
                                return false;
                            break;
                        case ParamsPram.Timer:
                            if (lastUse + Convert.ToInt32(p.value) > Others.Times)
                                return false;
                            break;
                        case ParamsPram.Min_Player_Level:
                            if (ObjectManager.Me.Level < Convert.ToInt32(p.value))
                                return false;
                            break;
                        case ParamsPram.Max_Player_Level:
                            if (ObjectManager.Me.Level > Convert.ToInt32(p.value))
                                return false;
                            break;
                        case ParamsPram.Min_Number_Attack_Player:
                            if (ObjectManager.GetNumberAttackPlayer() < Convert.ToInt32(p.value))
                                return false;
                            break;
                        case ParamsPram.Max_Number_Attack_Player:
                            if (ObjectManager.GetNumberAttackPlayer() > Convert.ToInt32(p.value))
                                return false;
                            break;

                        case ParamsPram.If_Target_IsCast:
                            if (!ObjectManager.Target.IsCast)
                                return false;
                            break;
                        case ParamsPram.Min_Target_Level:
                            if (ObjectManager.Target.Level < Convert.ToInt32(p.value))
                                return false;
                            break;
                        case ParamsPram.Max_Target_Level:
                            if (ObjectManager.Target.Level > Convert.ToInt32(p.value))
                                return false;
                            break;

                        case ParamsPram.Min_Player_ComboPoint:
                            if (ObjectManager.Me.ComboPoint < Convert.ToInt32(p.value))
                                return false;
                            break;
                        case ParamsPram.Max_Player_ComboPoint:
                            if (ObjectManager.Me.ComboPoint > Convert.ToInt32(p.value))
                                return false;
                            break;
                        case ParamsPram.Min_Player_BarTwo_Percentage:
                            if (ObjectManager.Me.BarTwoPercentage < Convert.ToInt32(p.value))
                                return false;
                            break;
                        case ParamsPram.Max_Player_BarTwo_Percentage:
                            if (ObjectManager.Me.BarTwoPercentage > Convert.ToInt32(p.value))
                                return false;
                            break;
                        case ParamsPram.Player_IsMount:
                            if (!ObjectManager.Me.IsMounted)
                                return false;
                            break;
                        case ParamsPram.Player_IsDead:
                            if (!ObjectManager.Me.IsDeadMe)
                                return false;
                            break;
                        case ParamsPram.Target_Targeting_Me:
                            if (!ObjectManager.Target.IsTargetingMe)
                                return false;
                            break;
                        case ParamsPram.Min_Target_Distance:
                            if (ObjectManager.Target.GetDistance < Convert.ToInt32(p.value))
                                return false;
                            break;
                        case ParamsPram.Max_Target_Distance:
                            if (ObjectManager.Target.GetDistance > Convert.ToInt32(p.value))
                                return false;
                            break;

                        case ParamsPram.none:
                            break;
                    }
                }
                catch (Exception)
                {
                }
            }
            return true;
        }

        internal void LaunchOrUse()
        {
            try
            {
                if (Type == TypeParam.Buff || Type == TypeParam.Spell)
                {
                    spell.Launch();
                }
                else if (Type == TypeParam.Item)
                {
                    ItemsManager.UseItem(Name);
                }
                lastUse = Others.Times;
            }
            catch (Exception)
            {
            }
        }
    }

    [Serializable]
    public class Param
    {
        public ParamsPram param = ParamsPram.none;
        public string value = "";
    }

    public enum WhenParam
    {
        Pull,
        Combat,
        Patrolling
    }

    public enum TypeParam
    {
        Buff,
        Spell,
        Item
    }

    public enum ParamsPram
    {
        none,
        Minimum_Health_Percent,
        Maximum_Health_Percent,
        Minimum_Target_Health_Percent,
        Maximum_Target_Health_Percent,
        Minimum_Pet_Health_Percent,
        Maximum_Pet_Health_Percent,
        Required_Knows_Spell_Name,
        Required_Not_Knows_Spell_Name,
        Required_Buff_Name,
        Min_Player_Level,
        Max_Player_Level,
        Min_Number_Attack_Player,
        Max_Number_Attack_Player,
        Timer,
        Min_Target_Level,
        Max_Target_Level,
        If_Target_IsCast,
        Min_Player_ComboPoint,
        Max_Player_ComboPoint,
        Min_Player_BarTwo_Percentage,
        Max_Player_BarTwo_Percentage,
        Player_IsMount,
        Player_IsDead,
        Target_Targeting_Me,
        Min_Target_Distance,
        Max_Target_Distance,
    }
}