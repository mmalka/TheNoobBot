using System;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using Timer = nManager.Helpful.Timer;

public class Main : ICustomClass
{
    #region Initialize_CustomClass

    internal static float range = 3.5f;
    public static bool loop = true;

    public float Range
    {
        get { return range; }
    }

    public void ShowConfiguration()
    {
        MessageBox.Show("No setting for this custom class.");
    }

    public void Initialize()
    {
        Logging.WriteFight("Loading The Noob Bot CC");

        switch (ObjectManager.Me.WowClass)
        {
            case WoWClass.DeathKnight:

                Spell Frost_Strike = new Spell("Frost Strike");
                Spell Scourge_Strike = new Spell("Scourge Strike");
                Spell Heart_Strike = new Spell("Heart Strike");

                if (Frost_Strike.KnownSpell)
                {
                    DKFrost ccDKF = new DKFrost();
                }

                if (Scourge_Strike.KnownSpell)
                {
                    DKUnholy ccDKU = new DKUnholy();
                }

                if (Heart_Strike.KnownSpell)
                {
                    DKBlood ccDKB = new DKBlood();
                }

                break;

            default:
                Dispose();
                break;
        }
        Logging.WriteFight("The Noob Bot CC closed.");
    }

    #endregion Initialize_CustomClass

    #region Dispose_CustomClass

    public void Dispose()
    {
        Logging.WriteFight("Closing The Noob Bot CC");
        loop = false;
    }

    #endregion Dispose_CustomClass
}

#region CustomClass

public class DKFrost
{
    #region InitializeSpell

    Spell Frost_Presence = new Spell("Frost Presence");
    Spell Blood_Presence = new Spell("Blood Presence");
    Spell Unholy_Presence = new Spell("Unholy Presence");
    Spell Death_Grip = new Spell("Death Grip");
    Spell Icy_Touch = new Spell("Icy Touch");
    Spell Frost_Fever = new Spell("Frost Fever");
    Spell Blood_Plague = new Spell("Blood Plague");
    Spell Plague_Strike = new Spell("Plague Strike");
    Spell Death_Strike = new Spell("Death Strike");
    Spell Blood_Strike = new Spell("Blood Strike");
    Spell Horn_of_Winter = new Spell("Horn of Winter");
    Spell Rune_Strike = new Spell("Rune Strike");
    Spell Outbreak = new Spell("Outbreak");
    Spell Obliterate = new Spell("Obliterate");
    Spell Frost_Strike = new Spell("Frost Strike");
    Spell Pillar_of_Frost = new Spell("Pillar of Frost");
    Spell Raise_Dead = new Spell("Raise Dead");
    Spell Death_Pact = new Spell("Death Pact");
    Spell Threat_of_Thassarian = new Spell("Threat of Thassarian");
    Spell Death_Coil = new Spell("Death Coil");
    Spell Howling_Blast = new Spell("Howling Blast");
    Spell Death_and_Decay = new Spell("Death and Decay");
    Spell Blood_Boil = new Spell("Blood Boil");
    Spell Bone_Shield = new Spell("Bone Shield");
    Spell Dancing_Rune_Weapon = new Spell("Dancing Rune Weapon");
    Spell Heart_Strike = new Spell("Heart Strike");
    Spell Rune_Tap = new Spell("Rune Tap");
    Spell Blood_Tap = new Spell("Blood Tap");
    Spell Empower_Rune_Weapon = new Spell("Empower Rune Weapon");
    Spell Dark_Command = new Spell("Dark Command");
    Spell Icebound_Fortitude = new Spell("Icebound Fortitude");
    Spell Vampiric_Blood = new Spell("Vampiric Blood");
    Spell Mind_Freeze = new Spell("Mind Freeze");
    Spell AntiMagic_Shell = new Spell("Anti-Magic Shell");
    Spell Strangulate = new Spell("Strangulate");
    Spell Dark_Simulacrum = new Spell("Dark Simulacrum");
    Timer Pestilence_Timer = new Timer(0);
    Spell Pestilence = new Spell("Pestilence");
    Spell Chains_of_Ice = new Spell("Chains of Ice");
    Spell Hungering_Cold = new Spell("Hungering Cold");
    Spell Freezing_Fog = new Spell("Freezing Fog");
    Spell Killing_Machine = new Spell("Killing Machine");
    Spell Lichborne = new Spell("Lichborne");
    Spell Path_of_Frost = new Spell("Path of Frost");

    Timer Plague_Strike_Timer = new Timer(0);

    #endregion InitializeSpell

    public DKFrost()
    {
        Main.range = 3.6f; // Range

        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            Buff_path();

            if (!ObjectManager.Me.IsMounted)
            {
                Patrolling();

                if (Fight.InFight && ObjectManager.Me.Target > 0)
                {
                    if (ObjectManager.Me.Target != lastTarget && Death_Grip.IsDistanceGood)
                    {
                        Pull();
                        lastTarget = ObjectManager.Me.Target;
                    }

                    Combat();
                }
            }
            Thread.Sleep(700);
        }
    }

    public void Pull()
    {
        Grip();
    }

    private void Buff_path()
    {
        if (!Fight.InFight && !Path_of_Frost.HaveBuff && Path_of_Frost.KnownSpell && Path_of_Frost.IsSpellUsable)
        {
            Path_of_Frost.Launch();
        }
    }

    public void Combat()
    {
        Presence();
        AvoidMelee();
        Grip();
        Decast();
        TargetMoving();
        Heal();
        Resistance();
        Buff();

        if (ObjectManager.Me.HealthPercent < 85 &&
            Lichborne.HaveBuff && Death_Coil.KnownSpell && Death_Coil.IsSpellUsable)
        {
            Lua.RunMacroText("/target Player");
            Death_Coil.Launch();
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2 &&
            Death_and_Decay.KnownSpell && Death_and_Decay.IsSpellUsable && Death_and_Decay.IsDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(43265, ObjectManager.Target.Position);
        }

        if (Plague_Strike_Timer.IsReady && Plague_Strike.KnownSpell && Plague_Strike.IsDistanceGood && Outbreak.KnownSpell && !Outbreak.IsSpellUsable)
        {
            if (!Plague_Strike.IsSpellUsable && Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable)
            {
                Blood_Tap.Launch();
                Thread.Sleep(500);
            }

            if (Plague_Strike.IsSpellUsable)
            {
                Plague_Strike.Launch();
                Plague_Strike_Timer = new Timer(1000 * 14);
                return;
            }
        }

        if (Outbreak.KnownSpell && Outbreak.IsSpellUsable && Outbreak.IsDistanceGood)
        {
            Outbreak.Launch();
            Plague_Strike_Timer = new Timer(1000 * 14);
            return;
        }

        if (!Obliterate.IsSpellUsable && !Blood_Strike.IsSpellUsable &&
            Frost_Strike.KnownSpell && Frost_Strike.IsSpellUsable && Frost_Strike.IsDistanceGood)
        {
            if (Howling_Blast.KnownSpell && Howling_Blast.IsSpellUsable && Howling_Blast.IsDistanceGood)
            {
                Howling_Blast.Launch();
                return;
            }

            else if (Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable)
            {
                Blood_Tap.Launch();
                return;
            }

            else if (Empower_Rune_Weapon.KnownSpell && Empower_Rune_Weapon.IsSpellUsable)
            {
                Empower_Rune_Weapon.Launch();
                return;
            }

            else if (!Lichborne.HaveBuff && Rune_Strike.KnownSpell && Rune_Strike.IsSpellUsable && Rune_Strike.IsDistanceGood)
            {
                Rune_Strike.Launch();
                return;
            }

            else if (!Lichborne.HaveBuff)
            {
                Frost_Strike.Launch();
                return;
            }
        }

        if (!Outbreak.KnownSpell)
        {
            if (!Blood_Plague.TargetHaveBuff &&
                Plague_Strike.KnownSpell && Plague_Strike.IsSpellUsable && Plague_Strike.IsDistanceGood)
            {
                Plague_Strike.Launch();
                return;
            }

            if (!Frost_Fever.TargetHaveBuff &&
                Icy_Touch.KnownSpell && Icy_Touch.IsSpellUsable && Icy_Touch.IsDistanceGood)
            {
                Icy_Touch.Launch();
                return;
            }
        }

        if (Obliterate.KnownSpell && Obliterate.IsSpellUsable && Obliterate.IsDistanceGood)
        {
            if (ObjectManager.Me.HealthPercent < 55 &&
                Death_Strike.KnownSpell && Death_Strike.IsSpellUsable && Death_Strike.IsDistanceGood)
            {
                Lua.RunMacroText("/use 13");
                Lua.RunMacroText("/script UIErrorsFrame:Clear()");
                Lua.RunMacroText("/use 14");
                Lua.RunMacroText("/script UIErrorsFrame:Clear()");
                Death_Strike.Launch();
                return;
            }

            else
            {
                Lua.RunMacroText("/use 13");
                Lua.RunMacroText("/script UIErrorsFrame:Clear()");
                Lua.RunMacroText("/use 14");
                Lua.RunMacroText("/script UIErrorsFrame:Clear()");
                Obliterate.Launch();
                return;
            }
        }

        if (ObjectManager.Me.BarTwoPercentage == 100 &&
            Frost_Strike.KnownSpell && Frost_Strike.IsSpellUsable && Frost_Strike.IsDistanceGood)
        {
            if (!Lichborne.HaveBuff && Rune_Strike.KnownSpell && Rune_Strike.IsSpellUsable && Rune_Strike.IsDistanceGood)
            {
                Rune_Strike.Launch();
                return;
            }

            else if (!Lichborne.HaveBuff)
            {
                Frost_Strike.Launch();
                return;
            }
        }

        if ((ObjectManager.Me.HaveBuff(59052) || ObjectManager.Me.HaveBuff(49188) ||
            ObjectManager.Me.HaveBuff(56822) || ObjectManager.Me.HaveBuff(59057)) &&
            Howling_Blast.KnownSpell && Howling_Blast.IsSpellUsable && Howling_Blast.IsDistanceGood)
        {
            Howling_Blast.Launch();
            return;
        }

        if (Blood_Strike.KnownSpell && Blood_Strike.IsSpellUsable && Blood_Strike.IsDistanceGood)
        {
            Blood_Strike.Launch();
            return;
        }
    }

    public void Patrolling()
    {
        if (!Fight.InFight && !Path_of_Frost.HaveBuff && Path_of_Frost.KnownSpell && Path_of_Frost.IsSpellUsable)
        {
            Path_of_Frost.Launch();
        }

        if (!ObjectManager.Me.IsMounted)
        {
            Buff();
        }

        Presence();
    }

    private void Presence()
    {
        if ((Unholy_Presence.KnownSpell || Frost_Presence.KnownSpell || Blood_Presence.KnownSpell) && ObjectManager.Me.PVP)
        {
            if (!Unholy_Presence.HaveBuff && Unholy_Presence.KnownSpell)
            {
                Unholy_Presence.Launch();
                return;
            }

            else if (!Unholy_Presence.KnownSpell && !Frost_Presence.HaveBuff && Frost_Presence.KnownSpell)
            {
                Frost_Presence.Launch();
                return;
            }

            /*else if (!Unholy_Presence.KnownSpell && !Frost_Presence.KnownSpell && !Blood_Presence.HaveBuff)
            {
                Blood_Presence.Launch();
                return;
            }*/
        }

        else if (Unholy_Presence.KnownSpell || Frost_Presence.KnownSpell || Blood_Presence.KnownSpell)
        {
            if (!Frost_Presence.HaveBuff && Frost_Presence.KnownSpell)
            {
                Frost_Presence.Launch();
                return;
            }

            else if (!Frost_Presence.KnownSpell && !Blood_Presence.HaveBuff)
            {
                Blood_Presence.Launch();
                return;
            }
        }
    }

    private void Grip()
    {
        if (ObjectManager.Target.GetDistance > 5 &&
            Death_Grip.KnownSpell && Death_Grip.IsSpellUsable && Death_Grip.IsDistanceGood)
        {
            Death_Grip.Launch();
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 75 &&
            Raise_Dead.KnownSpell && Raise_Dead.IsSpellUsable &&
            Death_Pact.KnownSpell && Death_Pact.IsSpellUsable)
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

        if (ObjectManager.Me.HealthPercent < 65 &&
            Lichborne.KnownSpell && Lichborne.IsSpellUsable &&
            Death_Coil.KnownSpell && Death_Coil.IsSpellUsable)
        {
            if (Lichborne.IsSpellUsable)
            {
                Lichborne.Launch();
                return;
            }
        }
    }

    private void Resistance()
    {
        if (ObjectManager.Me.HealthPercent < 75 &&
            Icebound_Fortitude.KnownSpell && Icebound_Fortitude.IsSpellUsable)
        {
            Icebound_Fortitude.Launch();
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe &&
            Mind_Freeze.KnownSpell && Mind_Freeze.IsSpellUsable && Mind_Freeze.IsDistanceGood)
        {
            Mind_Freeze.Launch();
        }

        if (ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe &&
            AntiMagic_Shell.KnownSpell && AntiMagic_Shell.IsSpellUsable)
        {
            AntiMagic_Shell.Launch();
        }

        if (ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe &&
            Strangulate.KnownSpell && Strangulate.IsSpellUsable && Strangulate.IsDistanceGood)
        {
            Strangulate.Launch();
        }

        if (ObjectManager.Target.IsCast &&
            Dark_Simulacrum.KnownSpell && Dark_Simulacrum.IsSpellUsable && Dark_Simulacrum.IsDistanceGood)
        {
            Dark_Simulacrum.Launch();
        }
    }

    private void TargetMoving()
    {
        if (ObjectManager.Target.GetDistance >= 10 &&
            Hungering_Cold.KnownSpell && Hungering_Cold.IsSpellUsable)
        {
            Hungering_Cold.Launch();
        }

        if (ObjectManager.Target.GetMove && !Chains_of_Ice.TargetHaveBuff &&
            Chains_of_Ice.KnownSpell && Chains_of_Ice.IsSpellUsable && Chains_of_Ice.IsDistanceGood)
        {
            Chains_of_Ice.Launch();
        }
    }

    private void Buff()
    {
        if (!Horn_of_Winter.HaveBuff && Horn_of_Winter.KnownSpell && Horn_of_Winter.IsSpellUsable)
        {
            Horn_of_Winter.Launch();
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

public class DKBlood
{
    #region InitializeSpell

    Spell Frost_Presence = new Spell("Frost Presence");
    Spell Blood_Presence = new Spell("Blood Presence");
    Spell Unholy_Presence = new Spell("Unholy Presence");
    Spell Death_Grip = new Spell("Death Grip");
    Spell Icy_Touch = new Spell("Icy Touch");
    Spell Frost_Fever = new Spell("Frost Fever");
    Spell Blood_Plague = new Spell("Blood Plague");
    Spell Plague_Strike = new Spell("Plague Strike");
    Spell Death_Strike = new Spell("Death Strike");
    Spell Blood_Strike = new Spell("Blood Strike");
    Spell Horn_of_Winter = new Spell("Horn of Winter");
    Spell Rune_Strike = new Spell("Rune Strike");
    Spell Outbreak = new Spell("Outbreak");
    Spell Obliterate = new Spell("Obliterate");
    Spell Frost_Strike = new Spell("Frost Strike");
    Spell Pillar_of_Frost = new Spell("Pillar of Frost");
    Spell Raise_Dead = new Spell("Raise Dead");
    Spell Death_Pact = new Spell("Death Pact");
    Spell Threat_of_Thassarian = new Spell("Threat of Thassarian");
    Spell Death_Coil = new Spell("Death Coil");
    Spell Howling_Blast = new Spell("Howling Blast");
    Spell Death_and_Decay = new Spell("Death and Decay");
    Spell Blood_Boil = new Spell("Blood Boil");
    Spell Bone_Shield = new Spell("Bone Shield");
    Spell Dancing_Rune_Weapon = new Spell("Dancing Rune Weapon");
    Spell Heart_Strike = new Spell("Heart Strike");
    Spell Rune_Tap = new Spell("Rune Tap");
    Spell Blood_Tap = new Spell("Blood Tap");
    Spell Dark_Command = new Spell("Dark Command");
    Spell Icebound_Fortitude = new Spell("Icebound Fortitude");
    Spell Vampiric_Blood = new Spell("Vampiric Blood");
    Spell Mind_Freeze = new Spell("Mind Freeze");
    Spell AntiMagic_Shell = new Spell("Anti-Magic Shell");
    Spell Strangulate = new Spell("Strangulate");
    Spell Dark_Simulacrum = new Spell("Dark Simulacrum");
    Timer Pestilence_Timer = new Timer(0);
    Spell Pestilence = new Spell("Pestilence");
    Spell Chains_of_Ice = new Spell("Chains of Ice");
    Spell Hungering_Cold = new Spell("Hungering Cold");
    Spell Empower_Rune_Weapon = new Spell("Empower Rune Weapon");
    Spell Lichborne = new Spell("Lichborne");
    Spell Path_of_Frost = new Spell("Path of Frost");

    Timer Plague_Strike_Timer = new Timer(0);
    Timer Icy_Touch_Timer = new Timer(0);

    #endregion InitializeSpell

    public DKBlood()
    {
        Main.range = 3.6f; // Range

        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            Buff_path();

            if (!ObjectManager.Me.IsMounted)
            {
                Patrolling();

                if (Fight.InFight && ObjectManager.Me.Target > 0)
                {
                    if (ObjectManager.Me.Target != lastTarget && Death_Grip.IsDistanceGood)
                    {
                        Pull();
                        lastTarget = ObjectManager.Me.Target;
                    }

                    Combat();
                }
            }
            Thread.Sleep(700);
        }
    }

    private void Buff_path()
    {
        if (!Fight.InFight && !Path_of_Frost.HaveBuff && Path_of_Frost.KnownSpell && Path_of_Frost.IsSpellUsable)
        {
            Path_of_Frost.Launch();
        }
    }

    public void Pull()
    {
        Grip();
    }

    public void Combat()
    {
        Presence();
        AvoidMelee();
        Grip();
        Decast();
        TargetMoving();
        Heal();
        Resistance();
        Buff();

        if (ObjectManager.Me.HealthPercent < 85 &&
            Lichborne.HaveBuff && Death_Coil.KnownSpell && Death_Coil.IsSpellUsable)
        {
            Lua.RunMacroText("/target Player");
            Death_Coil.Launch();
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2 &&
            Death_and_Decay.KnownSpell && Death_and_Decay.IsSpellUsable && Death_and_Decay.IsDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(43265, ObjectManager.Target.Position);
        }

        if (!Death_Strike.IsSpellUsable && !Heart_Strike.IsSpellUsable)
        {
            if (Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable)
            {
                Blood_Tap.Launch();
                return;
            }

            else if (Empower_Rune_Weapon.KnownSpell && Empower_Rune_Weapon.IsSpellUsable)
            {
                Empower_Rune_Weapon.Launch();
                return;
            }

            else if (!Lichborne.HaveBuff && Rune_Strike.KnownSpell && Rune_Strike.IsSpellUsable && Rune_Strike.IsDistanceGood)
            {
                Rune_Strike.Launch();
                return;
            }

            else if (!Lichborne.HaveBuff && Death_Coil.KnownSpell && Death_Coil.IsSpellUsable && Death_Coil.IsDistanceGood)
            {
                Death_Coil.Launch();
                return;
            }
        }

        if (Plague_Strike_Timer.IsReady && Plague_Strike.KnownSpell && Plague_Strike.IsDistanceGood && ((Outbreak.KnownSpell && !Outbreak.IsSpellUsable) || !Outbreak.KnownSpell))
        {
            if (!Plague_Strike.IsSpellUsable && Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable)
            {
                Blood_Tap.Launch();
                Thread.Sleep(500);
            }

            if (Plague_Strike.IsSpellUsable)
            {
                Plague_Strike.Launch();
                Plague_Strike_Timer = new Timer(1000 * 14);
                return;
            }
        }

        if (Icy_Touch_Timer.IsReady && Icy_Touch.KnownSpell && Icy_Touch.IsDistanceGood && ((Outbreak.KnownSpell && !Outbreak.IsSpellUsable) || !Outbreak.KnownSpell))
        {
            if (!Icy_Touch.IsSpellUsable && Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable)
            {
                Blood_Tap.Launch();
                Thread.Sleep(500);
            }

            if (Icy_Touch.IsSpellUsable)
            {
                Icy_Touch.Launch();
                Icy_Touch_Timer = new Timer(1000 * 14);
                return;
            }
        }

        if (Outbreak.KnownSpell && Outbreak.IsSpellUsable && Outbreak.IsDistanceGood)
        {
            Outbreak.Launch();
            Plague_Strike_Timer = new Timer(1000 * 14);
            Icy_Touch_Timer = new Timer(1000 * 14);
            return;
        }

        if (!Lichborne.HaveBuff && Dancing_Rune_Weapon.KnownSpell && Dancing_Rune_Weapon.IsSpellUsable)
        {
            Dancing_Rune_Weapon.Launch();
            return;
        }

        if (Death_Strike.KnownSpell && Death_Strike.IsDistanceGood && Death_Strike.IsSpellUsable)
        {
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Death_Strike.Launch();
            return;
        }

        if (!Lichborne.HaveBuff && Rune_Strike.KnownSpell && Rune_Strike.IsDistanceGood && Rune_Strike.IsSpellUsable)
        {
            Rune_Strike.Launch();
            return;
        }

        if (!Lichborne.HaveBuff && !Rune_Strike.KnownSpell && Death_Coil.IsDistanceGood && Death_Coil.IsSpellUsable)
        {
            Death_Coil.Launch();
            return;
        }

        if (Heart_Strike.KnownSpell && Heart_Strike.IsDistanceGood && Heart_Strike.IsSpellUsable)
        {
            Heart_Strike.Launch();
            return;
        }

        if ((Unholy_Presence.HaveBuff || Frost_Presence.HaveBuff) &&
            !Death_Strike.IsSpellUsable && !Heart_Strike.IsSpellUsable && ObjectManager.Me.BarTwoPercentage >= 70)
        {
            if (!Lichborne.HaveBuff && Rune_Strike.KnownSpell && Rune_Strike.IsSpellUsable && Rune_Strike.IsDistanceGood)
            {
                Rune_Strike.Launch();
                return;
            }

            else if (!Lichborne.HaveBuff && Death_Coil.KnownSpell && Death_Coil.IsSpellUsable && Death_Coil.IsDistanceGood)
            {
                Death_Coil.Launch();
                return;
            }
        }
    }

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            if (!Bone_Shield.HaveBuff && Bone_Shield.KnownSpell && Bone_Shield.IsSpellUsable)
            {
                Bone_Shield.Launch();
            }

            Buff();
        }

        Presence();
    }

    private void Presence()
    {
        if ((Unholy_Presence.KnownSpell || Frost_Presence.KnownSpell || Blood_Presence.KnownSpell) && ObjectManager.Me.PVP)
        {
            if (!Unholy_Presence.HaveBuff && Unholy_Presence.KnownSpell)
            {
                Unholy_Presence.Launch();
                return;
            }

            else if (!Unholy_Presence.KnownSpell && !Frost_Presence.HaveBuff && Frost_Presence.KnownSpell)
            {
                Frost_Presence.Launch();
                return;
            }

            else if (!Unholy_Presence.KnownSpell && !Frost_Presence.KnownSpell && !Blood_Presence.HaveBuff)
            {
                Blood_Presence.Launch();
                return;
            }
        }

        /*else if (Unholy_Presence.KnownSpell || Frost_Presence.KnownSpell || Blood_Presence.KnownSpell)
        {
            if (Blood_Presence.KnownSpell && !Blood_Presence.HaveBuff)
            {
                Blood_Presence.Launch();
                return;
            }
        }*/
    }

    private void Grip()
    {
        if (ObjectManager.Target.GetDistance > 5 &&
            Death_Grip.KnownSpell && Death_Grip.IsSpellUsable && Death_Grip.IsDistanceGood)
        {
            Death_Grip.Launch();
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 90 &&
            Rune_Tap.KnownSpell &&
            Rune_Tap.IsSpellUsable)
        {
            Rune_Tap.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 75 &&
            Raise_Dead.KnownSpell && Raise_Dead.IsSpellUsable &&
            Death_Pact.KnownSpell && Death_Pact.IsSpellUsable)
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

        if (ObjectManager.Me.HealthPercent < 55 &&
            Lichborne.KnownSpell && Lichborne.IsSpellUsable &&
            Death_Coil.KnownSpell && Death_Coil.IsSpellUsable)
        {
            if (Lichborne.IsSpellUsable)
            {
                Lichborne.Launch();
                return;
            }
        }
    }

    private void Resistance()
    {
        if (!Bone_Shield.HaveBuff && Bone_Shield.KnownSpell && Bone_Shield.IsSpellUsable)
        {
            Bone_Shield.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 75 &&
            Icebound_Fortitude.KnownSpell && Icebound_Fortitude.IsSpellUsable)
        {
            Icebound_Fortitude.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 60 &&
            Vampiric_Blood.KnownSpell && Vampiric_Blood.IsSpellUsable)
        {
            Vampiric_Blood.Launch();
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe &&
            Mind_Freeze.KnownSpell && Mind_Freeze.IsSpellUsable && Mind_Freeze.IsDistanceGood)
        {
            Mind_Freeze.Launch();
        }

        if (ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe &&
            AntiMagic_Shell.KnownSpell && AntiMagic_Shell.IsSpellUsable)
        {
            AntiMagic_Shell.Launch();
        }

        if (ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe &&
            Strangulate.KnownSpell && Strangulate.IsSpellUsable && Strangulate.IsDistanceGood)
        {
            Strangulate.Launch();
        }

        if (ObjectManager.Target.IsCast &&
            Dark_Simulacrum.KnownSpell && Dark_Simulacrum.IsSpellUsable && Dark_Simulacrum.IsDistanceGood)
        {
            Dark_Simulacrum.Launch();
        }
    }

    private void TargetMoving()
    {
        if (ObjectManager.Target.GetMove && !Chains_of_Ice.TargetHaveBuff &&
            Chains_of_Ice.KnownSpell && Chains_of_Ice.IsSpellUsable && Chains_of_Ice.IsDistanceGood)
        {
            Chains_of_Ice.Launch();
        }
    }

    private void Buff()
    {
        if (!Horn_of_Winter.HaveBuff && Horn_of_Winter.KnownSpell && Horn_of_Winter.IsSpellUsable)
        {
            Horn_of_Winter.Launch();
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

public class DKUnholy
{
    #region InitializeSpell

    Spell Frost_Presence = new Spell("Frost Presence");
    Spell Blood_Presence = new Spell("Blood Presence");
    Spell Unholy_Presence = new Spell("Unholy Presence");
    Spell Death_Grip = new Spell("Death Grip");
    Spell Icy_Touch = new Spell("Icy Touch");
    Spell Frost_Fever = new Spell("Frost Fever");
    Spell Blood_Plague = new Spell("Blood Plague");
    Spell Plague_Strike = new Spell("Plague Strike");
    Spell Death_Strike = new Spell("Death Strike");
    Spell Blood_Strike = new Spell("Blood Strike");
    Spell Horn_of_Winter = new Spell("Horn of Winter");
    Spell Rune_Strike = new Spell("Rune Strike");
    Spell Outbreak = new Spell("Outbreak");
    Spell Obliterate = new Spell("Obliterate");
    Spell Frost_Strike = new Spell("Frost Strike");
    Spell Pillar_of_Frost = new Spell("Pillar of Frost");
    Spell Raise_Dead = new Spell("Raise Dead");
    Spell Death_Pact = new Spell("Death Pact");
    Spell Threat_of_Thassarian = new Spell("Threat of Thassarian");
    Spell Death_Coil = new Spell("Death Coil");
    Spell Howling_Blast = new Spell("Howling Blast");
    Spell Death_and_Decay = new Spell("Death and Decay");
    Spell Blood_Boil = new Spell("Blood Boil");
    Spell Bone_Shield = new Spell("Bone Shield");
    Spell Dancing_Rune_Weapon = new Spell("Dancing Rune Weapon");
    Spell Heart_Strike = new Spell("Heart Strike");
    Spell Rune_Tap = new Spell("Rune Tap");
    Spell Blood_Tap = new Spell("Blood Tap");
    Spell Dark_Command = new Spell("Dark Command");
    Spell Icebound_Fortitude = new Spell("Icebound Fortitude");
    Spell Vampiric_Blood = new Spell("Vampiric Blood");
    Spell Mind_Freeze = new Spell("Mind Freeze");
    Spell AntiMagic_Shell = new Spell("Anti-Magic Shell");
    Spell Strangulate = new Spell("Strangulate");
    Spell Dark_Simulacrum = new Spell("Dark Simulacrum");
    Spell Unholy_Frenzy = new Spell("Unholy Frenzy");
    Timer Pestilence_Timer = new Timer(0);
    Spell Pestilence = new Spell("Pestilence");
    Spell Chains_of_Ice = new Spell("Chains of Ice");
    Spell Hungering_Cold = new Spell("Hungering Cold");
    Spell Festering_Strike = new Spell("Festering Strike");
    Spell Scourge_Strike = new Spell("Scourge Strike");
    Spell Dark_Transformation = new Spell("Dark Transformation");
    Spell Summon_Gargoyle = new Spell("Summon Gargoyle");
    Spell AntiMagic_Zone = new Spell("Anti-Magic Zone");
    Spell Empower_Rune_Weapon = new Spell("Empower Rune Weapon");
    Spell Sudden_Doom = new Spell("Sudden Doom");
    Spell Lichborne = new Spell("Lichborne");
    Spell Path_of_Frost = new Spell("Path of Frost");

    Timer Plague_Strike_Timer = new Timer(0);
    Timer Icy_Touch_Timer = new Timer(0);

    #endregion InitializeSpell

    public DKUnholy()
    {
        Main.range = 3.6f; // Range

        UInt64 lastTarget = 0;

        while (Main.loop)
        {
            Buff_path();

            if (!ObjectManager.Me.IsMounted)
            {
                Patrolling();

                if (Fight.InFight && ObjectManager.Me.Target > 0)
                {
                    if (ObjectManager.Me.Target != lastTarget && Death_Grip.IsDistanceGood)
                    {
                        Pull();
                        lastTarget = ObjectManager.Me.Target;
                    }

                    Combat();
                }
            }
            Thread.Sleep(700);
        }
    }

    public void Pull()
    {
        Pet();
        Grip();
    }

    private void Buff_path()
    {
        if (!Fight.InFight && !Path_of_Frost.HaveBuff && Path_of_Frost.KnownSpell && Path_of_Frost.IsSpellUsable)
        {
            Path_of_Frost.Launch();
        }
    }

    public void Combat()
    {
        Pet();
        Presence();
        AvoidMelee();
        Grip();
        Decast();
        TargetMoving();
        Heal();
        Resistance();
        Buff();

        if (ObjectManager.Me.HealthPercent < 85 &&
            Lichborne.HaveBuff && Death_Coil.KnownSpell && Death_Coil.IsSpellUsable)
        {
            Lua.RunMacroText("/target Player");
            Death_Coil.Launch();
            return;
        }

        if (ObjectManager.GetNumberAttackPlayer() > 2 &&
            Death_and_Decay.KnownSpell && Death_and_Decay.IsSpellUsable && Death_and_Decay.IsDistanceGood)
        {
            SpellManager.CastSpellByIDAndPosition(43265, ObjectManager.Target.Position);
        }

        if (ObjectManager.Me.HealthPercent < 55 &&
            Death_Strike.KnownSpell && Death_Strike.IsDistanceGood)
        {
            if (Empower_Rune_Weapon.KnownSpell && Empower_Rune_Weapon.IsSpellUsable)
            {
                Empower_Rune_Weapon.Launch();
                return;
            }

            if ((!Empower_Rune_Weapon.IsSpellUsable || !Death_Strike.IsSpellUsable) && !Death_Strike.IsSpellUsable && Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable)
            {
                Blood_Tap.Launch();
                return;
            }

            if (Death_Strike.IsSpellUsable)
            {
                Death_Strike.Launch();
                return;
            }
        }

        if (!Festering_Strike.IsSpellUsable && !Scourge_Strike.IsSpellUsable)
        {
            if (Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable)
            {
                Blood_Tap.Launch();
                return;
            }

            else if (Empower_Rune_Weapon.KnownSpell && Empower_Rune_Weapon.IsSpellUsable)
            {
                Empower_Rune_Weapon.Launch();
                return;
            }
        }

        if (ObjectManager.Me.BarTwoPercentage >= 60 && Death_Coil.KnownSpell)
        {
            if (!Lichborne.HaveBuff && Summon_Gargoyle.KnownSpell && Summon_Gargoyle.IsSpellUsable && Summon_Gargoyle.IsDistanceGood)
            {
                Summon_Gargoyle.Launch();
                return;
            }

            if (!Lichborne.HaveBuff && Death_Coil.KnownSpell && Death_Coil.IsSpellUsable && Death_Coil.IsDistanceGood)
            {
                Death_Coil.Launch();
                return;
            }
        }

        if (!Festering_Strike.KnownSpell &&
            Plague_Strike_Timer.IsReady && Plague_Strike.KnownSpell && Plague_Strike.IsDistanceGood && ((Outbreak.KnownSpell && !Outbreak.IsSpellUsable) || !Outbreak.KnownSpell))
        {
            if (!Plague_Strike.IsSpellUsable && Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable)
            {
                Blood_Tap.Launch();
                Thread.Sleep(500);
            }

            if (Plague_Strike.IsSpellUsable)
            {
                Plague_Strike.Launch();
                Plague_Strike_Timer = new Timer(1000 * 14);
                return;
            }
        }

        if (!Festering_Strike.KnownSpell &&
            Icy_Touch_Timer.IsReady && Icy_Touch.KnownSpell && Icy_Touch.IsDistanceGood && ((Outbreak.KnownSpell && !Outbreak.IsSpellUsable) || !Outbreak.KnownSpell))
        {
            if (!Icy_Touch.IsSpellUsable && Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable)
            {
                Blood_Tap.Launch();
                Thread.Sleep(500);
            }

            if (Icy_Touch.IsSpellUsable)
            {
                Icy_Touch.Launch();
                Icy_Touch_Timer = new Timer(1000 * 14);
                return;
            }
        }

        if (Festering_Strike.KnownSpell && Festering_Strike.IsDistanceGood && Festering_Strike.IsSpellUsable)
        {
            if (Plague_Strike_Timer.IsReady &&
                !Blood_Plague.TargetHaveBuff && Plague_Strike.KnownSpell && Plague_Strike.IsDistanceGood &&
                ((Outbreak.KnownSpell && !Outbreak.IsSpellUsable) || !Outbreak.KnownSpell))
            {
                if (!Plague_Strike.IsSpellUsable && Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable)
                {
                    Blood_Tap.Launch();
                    Thread.Sleep(500);
                }

                if (Plague_Strike.IsSpellUsable)
                {
                    Plague_Strike.Launch();
                    Plague_Strike_Timer = new Timer(1000 * 14);
                }
            }

            if (Icy_Touch_Timer.IsReady &&
                !Frost_Fever.TargetHaveBuff && Icy_Touch.KnownSpell && Icy_Touch.IsDistanceGood &&
                ((Outbreak.KnownSpell && !Outbreak.IsSpellUsable) || !Outbreak.KnownSpell))
            {
                if (!Icy_Touch.IsSpellUsable && Blood_Tap.KnownSpell && Blood_Tap.IsSpellUsable)
                {
                    Blood_Tap.Launch();
                    Thread.Sleep(500);
                }

                if (Icy_Touch.IsSpellUsable)
                {
                    Icy_Touch.Launch();
                    Icy_Touch_Timer = new Timer(1000 * 14);
                }
            }

            if (Frost_Fever.TargetHaveBuff && Blood_Plague.TargetHaveBuff &&
                Festering_Strike.IsSpellUsable && Festering_Strike.IsDistanceGood)
            {
                Festering_Strike.Launch();
                return;
            }
        }

        if (Outbreak.KnownSpell && Outbreak.IsSpellUsable && Outbreak.IsDistanceGood)
        {
            Outbreak.Launch();
            Plague_Strike_Timer = new Timer(1000 * 14);
            Icy_Touch_Timer = new Timer(1000 * 14);
            return;
        }

        if (Dark_Transformation.KnownSpell && Dark_Transformation.IsSpellUsable)
        {
            Lua.RunMacroText("/use 13");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Lua.RunMacroText("/use 14");
            Lua.RunMacroText("/script UIErrorsFrame:Clear()");
            Dark_Transformation.Launch();
            return;
        }

        if (!Festering_Strike.KnownSpell && Death_Strike.KnownSpell && Death_Strike.IsDistanceGood && Death_Strike.IsSpellUsable)
        {
            Death_Strike.Launch();
            return;
        }

        if (Scourge_Strike.KnownSpell && Scourge_Strike.IsDistanceGood && Scourge_Strike.IsSpellUsable)
        {
            Scourge_Strike.Launch();
            return;
        }

        if (Unholy_Frenzy.KnownSpell && Unholy_Frenzy.IsSpellUsable)
        {
            Lua.RunMacroText("/target player");
            Unholy_Frenzy.Launch();
        }
    }

    private void Pet()
    {
        if ((ObjectManager.Pet.Health == 0 || ObjectManager.Pet.Guid == 0) &&
            !ObjectManager.Me.IsMounted && !ObjectManager.Me.IsDeadMe)
        {
            if (Raise_Dead.KnownSpell && Raise_Dead.IsSpellUsable)
            {
                Raise_Dead.Launch();
            }
        }
    }

    public void Patrolling()
    {
        if (!ObjectManager.Me.IsMounted)
        {
            Buff();
        }

        Presence();
    }

    private void Presence()
    {
        if ((Unholy_Presence.KnownSpell || Frost_Presence.KnownSpell || Blood_Presence.KnownSpell) && ObjectManager.Me.PVP)
        {
            if (!Unholy_Presence.HaveBuff && Unholy_Presence.KnownSpell)
            {
                Unholy_Presence.Launch();
                return;
            }

            else if (!Unholy_Presence.KnownSpell && !Frost_Presence.HaveBuff && Frost_Presence.KnownSpell)
            {
                Frost_Presence.Launch();
                return;
            }

            else if (!Unholy_Presence.KnownSpell && !Frost_Presence.KnownSpell && !Blood_Presence.HaveBuff)
            {
                Blood_Presence.Launch();
                return;
            }
        }

        /*else if (Unholy_Presence.KnownSpell || Frost_Presence.KnownSpell || Blood_Presence.KnownSpell)
        {
            if (Blood_Presence.KnownSpell && !Blood_Presence.HaveBuff)
            {
                Blood_Presence.Launch();
                return;
            }
        }*/
    }

    private void Grip()
    {
        if (ObjectManager.Target.GetDistance > 5 &&
            Death_Grip.KnownSpell && Death_Grip.IsSpellUsable && Death_Grip.IsDistanceGood)
        {
            Death_Grip.Launch();
        }
    }

    private void Heal()
    {
        if (ObjectManager.Me.HealthPercent < 65 &&
            Lichborne.KnownSpell && Lichborne.IsSpellUsable &&
            Death_Coil.KnownSpell && Death_Coil.IsSpellUsable)
        {
            if (Lichborne.IsSpellUsable)
            {
                Lichborne.Launch();
                return;
            }
        }
    }

    private void Resistance()
    {
        if (!Bone_Shield.HaveBuff && Bone_Shield.KnownSpell && Bone_Shield.IsSpellUsable)
        {
            Bone_Shield.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 75 &&
            Icebound_Fortitude.KnownSpell && Icebound_Fortitude.IsSpellUsable)
        {
            Icebound_Fortitude.Launch();
        }

        if (ObjectManager.Me.HealthPercent < 60 &&
            Vampiric_Blood.KnownSpell && Vampiric_Blood.IsSpellUsable)
        {
            Vampiric_Blood.Launch();
        }

        if (ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe &&
            AntiMagic_Zone.KnownSpell && AntiMagic_Zone.IsSpellUsable)
        {
            AntiMagic_Zone.Launch();
        }
    }

    private void Decast()
    {
        if (ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe &&
            Mind_Freeze.KnownSpell && Mind_Freeze.IsSpellUsable && Mind_Freeze.IsDistanceGood)
        {
            Mind_Freeze.Launch();
        }

        if (ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe &&
            AntiMagic_Shell.KnownSpell && AntiMagic_Shell.IsSpellUsable)
        {
            AntiMagic_Shell.Launch();
        }

        if (ObjectManager.Target.IsCast &&
            ObjectManager.Target.IsTargetingMe &&
            Strangulate.KnownSpell && Strangulate.IsSpellUsable && Strangulate.IsDistanceGood)
        {
            Strangulate.Launch();
        }

        if (ObjectManager.Target.IsCast &&
            Dark_Simulacrum.KnownSpell && Dark_Simulacrum.IsSpellUsable && Dark_Simulacrum.IsDistanceGood)
        {
            Dark_Simulacrum.Launch();
        }
    }

    private void TargetMoving()
    {
        if (ObjectManager.Target.GetMove && !Chains_of_Ice.TargetHaveBuff &&
            Chains_of_Ice.KnownSpell && Chains_of_Ice.IsSpellUsable && Chains_of_Ice.IsDistanceGood)
        {
            Chains_of_Ice.Launch();
        }
    }

    private void Buff()
    {
        if (!Horn_of_Winter.HaveBuff && Horn_of_Winter.KnownSpell && Horn_of_Winter.IsSpellUsable)
        {
            Horn_of_Winter.Launch();
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

#endregion CustomClass
