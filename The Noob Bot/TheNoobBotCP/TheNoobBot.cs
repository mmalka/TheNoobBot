/*
* CustomProfile for TheNoobBot
* Credit : Vesper
*/

using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Products;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

public class Main : ICustomProfile
{
    internal static bool loop = true;

    #region ICustomProfile Members

    public string BattlegroundId { get; set; }

    public void Initialize()
    {
        try
        {
            if (!loop)
                loop = true;
            Logging.WriteFight("Loading TheNoobBot example Custom Profile.");
            if (BattlegroundId != null)
                if (BattlegroundId == "WarsongGulch")
                {
                    new CaptureTheFlagWG();
                }
        }
        catch (Exception exception)
        {
            Logging.WriteError("Initialize(): " + exception);
        }
        Logging.WriteFight("TheNoobBot example Custom Profile stopped.");
    }

    public void Dispose()
    {
        Logging.WriteFight("TheNoobBot example Custom Profile stopped.");
        loop = false;
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

public class CaptureTheFlagWG
{
    /**
      * Author : VesperCore
    **/

    private int Aggresivity;
    private bool IWearTheFlag = true;
    private bool SomeoneElseWearTheFlag;

    public CaptureTheFlagWG()
    {
        while (Main.loop)
        {
            try
            {
                if (Usefuls.InGame && !Usefuls.IsLoadingOrConnecting && !ObjectManager.Me.IsDeadMe &&
                    ObjectManager.Me.IsValid && !ObjectManager.Me.InCombat && Products.IsStarted)
                {
                    // For testing purpose:
                    if (IWearTheFlag)
                    {
                        // Go to place it;
                        var points = new List<Point>
                        {
                            new Point((float) 982.9947, (float) 1432.755, (float) 367.0161)
                        };
                        Battlegrounder.Bot.Bot.MovementLoop.PathLoop = points;
                    }
                    else if (!IWearTheFlag && SomeoneElseWearTheFlag)
                    {
                        if (Aggresivity == 0)
                        {
                            // Go support it;
                        }
                        else if (Aggresivity == 1)
                        {
                            // Go get the next one; Include waiting.
                        }
                        else if (Aggresivity == 2)
                        {
                            // Go to kill my faction flag's holder and get the next one;   
                        }
                    }
                    else if (!IWearTheFlag && !SomeoneElseWearTheFlag)
                    {
                        // Go get it;
                    }
                }
                else
                    Thread.Sleep(500);
            }
            catch (Exception e)
            {
                Logging.WriteError("Custom Profile > TheNoobBotCP > CaptureTheFlagWG > Loop: " + e);
            }
            Thread.Sleep(150);
        }
    }
}