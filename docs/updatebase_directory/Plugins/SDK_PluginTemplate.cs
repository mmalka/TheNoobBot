using System;
using System.Threading;
using System.Windows.Forms;
using nManager;
using nManager.Helpful;
using nManager.Plugins;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

#region Interface Implementation - Edition Expert only

public class Main : IPlugins
{
    private bool _checkFieldRunning;

    public bool Loop
    {
        get { return MyPluginClass.InternalLoop; }
        set { MyPluginClass.InternalLoop = value; }
    }

    public string Name
    {
        get { return MyPluginClass.Name; }
    }

    public string Author
    {
        get { return MyPluginClass.Author; }
    }

    public string Description
    {
        get { return MyPluginClass.Description; }
    }

    public string TargetVersion
    {
        get { return MyPluginClass.TargetVersion; }
    }

    public string Version
    {
        get { return MyPluginClass.Version; }
    }

    public bool IsStarted
    {
        get { return Loop && !_checkFieldRunning; }
    }

    public void Dispose()
    {
        Loop = false;
    }

    public void Initialize()
    {
        Logging.WriteDebug(string.Format("The plugin {0} is loading.", Name));
        Initialize(false);
    }

    public void ShowConfiguration()
    {
        // If you have a configuration form, please create/call MyPluginClass.ShowConfiguration();
        Logging.WriteDebug(string.Format("The plugin {0} don't implement any settings system.", Name));
        MessageBox.Show(string.Format("The plugin {0} don't need to be configured.", Name));
    }

    public void ResetConfiguration()
    {
        // If you have a configuration form, please create/call MyPluginClass.ResetConfiguration();
        Logging.WriteDebug(string.Format("The plugin {0} don't implement any settings system.", Name));
        MessageBox.Show(string.Format("The plugin {0} don't need to be configured.", Name));
    }

    public void CheckFields() // do not edit.
    {
        _checkFieldRunning = true;
        Loop = true;
        while (Loop)
        {
            Thread.Sleep(1000); // Don't do any action.
        }
    }

    public void Initialize(bool configOnly, bool resetSettings = false)
    {
        try
        {
            if (!configOnly && !resetSettings)
                Loop = true;
            MyPluginClass.Init();
        }
        catch (Exception e)
        {
            Logging.WriteError("IPlugins.Main.Initialize(bool configOnly, bool resetSettings = false): " + e);
        }
        Logging.WriteDebug(string.Format("The plugin {0} has stopped.", Name));
    }
}

#endregion

#region Plugin core - Your plugin should be coded here

public static class MyPluginClass
{
    public static bool InternalLoop = true; // Keep your plugin started.
    public static string Author = "Vesper";
    public static string Name = "SDK Plugin Template";
    public static string TargetVersion = "3.0.x"; // Only the two first numbers are checked.
    public static string Version = "1.0.0";
    public static string Description = "An empty template for the plugins system of TheNoobBot.";

    public static void Init()
    {
        // Do some init stuff here.
        MainLoop();
    }

    public static void MainLoop()
    {
        while (InternalLoop)
        {
		    // Code your plugin here. Don't forget to remove the logging following line.
            Logging.Write("Plugin '" + Name + "' running...");
            Thread.Sleep(1000);
        }
    }
}

#endregion