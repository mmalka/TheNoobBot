/*
* CustomClass for TheNoobBot
* Credit : Vesper
*/

using System;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Wow.Helpers;

public class Main : ICustomClass
{
    internal static float range = 5.0f;
    internal static bool loop = true;

    #region ICustomClass Members

    public float Range
    {
        get { return range; }
    }

    public void Initialize()
    {
        try
        {
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
        loop = false;
    }

    public void ShowConfiguration()
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
    **/

    public AntiAfk()
    {
        Main.range = 5.0f;

        while (Main.loop)
        {
            Thread.Sleep(200);
            Keybindings.PressKeybindings(nManager.Wow.Enums.Keybindings.JUMP);
            Thread.Sleep(55212);
        }
    }
}