/*
* CombatClass for TheNoobBot
* Credit : Vesper
*/

using System;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Helpers;

public class Main : ICombatClass
{
    internal static float InternalRange = 5.0f;
    internal static bool InternalLoop = true;

    #region ICombatClass Members

    public float Range
    {
        get { return InternalRange; }
    }

    public void Initialize()
    {
        try
        {
            if (!InternalLoop)
                InternalLoop = true;
            Logging.WriteFight("Loading Anti AFK system.");

            new AntiAfk();
        }
        catch (Exception exception)
        {
            Logging.WriteError("Initialize(): " + exception);
        }
        Logging.WriteFight("Anti AFK system stopped.");
    }

    public void Dispose()
    {
        Logging.WriteFight("Anti AFK system stopped.");
        InternalLoop = false;
    }

    public void ShowConfiguration()
    {
        MessageBox.Show("There is no settings available");
    }

    public void ResetConfiguration()
    {
        MessageBox.Show("There is no settings available");
    }

    #endregion
}

public class AntiAfk
{
    /**
      * Author : VesperCore
      * Utility : Anti_AFK; to be used with a profile with only 1 point.
      * Purpose : Empty CombatClass template.
      * Depreciated : The bot wont let you go AFK with or without this file. 
    **/

    public AntiAfk()
    {
        Main.InternalRange = 5.0f;

        while (Main.InternalLoop)
        {
            MovementsAction.Jump();
            Thread.Sleep(new Random().Next(45000, 90000));
        }
    }
}