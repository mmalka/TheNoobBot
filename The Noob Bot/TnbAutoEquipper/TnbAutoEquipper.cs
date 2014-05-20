using System;
using System.Collections.Generic;
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
        Logging.Write(string.Format("The plugin {0} has stopped.", Name));
    }
}

#endregion

#region Plugin core - Your plugin should be coded here

public static class MyPluginClass
{
    public static bool InternalLoop = true;
    public static string Author = "Vesper";
    public static string Name = "AutoEquipper";
    public static string TargetVersion = "3.2.x";
    public static string Version = "1.1.1";
    public static string Description = "Always check the inventory on new loot for a better item for our class/specialization.";

    private static readonly object ParseItemLock = new object();

    public static void Init()
    {
        // Do some init stuff here.
        if (!nManagerSetting.CurrentSetting.ActivateLootStatistics)
        {
            Logging.WriteDebug(string.Format("The plugin {0} needs ActivateLootStatistics to be activated in General Settings.", Name));
            return;
        }
        while (Others.ItemStock.Count <= 0)
        {
            Thread.Sleep(100); // wait for LootStatistics to finish its first run.
        }
        foreach (var itemStock in Others.ItemStock)
        {
            ParseItemStock(itemStock, new EventArgs()); // Parse the inventory once before subscribing to new loots.
        }
        if (Others.ItemStockUpdated != null)
            Others.ItemStockUpdated -= ParseItemStock;
        Others.ItemStockUpdated += ParseItemStock; // Subscribe to TheNoobBot event OnLoot, return the every single new loots.
        MainLoop();
    }

    private static void ParseItemStock(object sender, EventArgs e)
    {
        lock (ParseItemLock)
        {
            if (!(sender is KeyValuePair<int, int>))
                return;
            var itemStock = (KeyValuePair<int, int>) sender;
            string itemName = ItemsManager.GetItemNameById(itemStock.Key);
            ItemInfo itemInfo;
            if (ItemSelection.EvaluateItemStatsVsEquiped(itemName, out itemInfo) < 0)
            {
                if (itemInfo != null)
                {
                    while (!Usefuls.InGame || ObjectManager.Me.InCombat || ObjectManager.Me.IsDeadMe || ObjectManager.Me.InTransport)
                    {
                        Thread.Sleep(100); // We wait until we are free to equipp the item.
                    }
                    Logging.WriteDebug(Name + ": Equipp " + itemName);
                    ItemsManager.EquipItemByName(itemName); // Equipp now and we will reEquipp each better items till the ends of new loots.
                    Thread.Sleep(100);
                }
            }
        }
    }

    public static void MainLoop()
    {
        while (InternalLoop)
        {
            Thread.Sleep(1);
        }
    }
}

#endregion