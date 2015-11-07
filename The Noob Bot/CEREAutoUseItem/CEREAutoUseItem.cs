using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Plugins;
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
        Logging.Write(string.Format("The plugin {0} has stopped.", Name));
        Loop = false;
    }

    public void Initialize()
    {
        Logging.Write(string.Format("The plugin {0} is loading.", Name));
        Initialize(false);
    }

    public void ShowConfiguration()
    {
        MyPluginClass.ShowConfiguration();
    }

    public void ResetConfiguration()
    {
        MyPluginClass.ResetConfiguration();
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
        if (Loop)
            Dispose();
    }
}

#endregion

#region Plugin core - Your plugin should be coded here

public static class MyPluginClass
{
    public static bool InternalLoop = true;
    public static string Author = "CEREAL";
    public static string Name = "Automatic Item Usage";
    public static string TargetVersion = "4.8.x";
    public static string Version = "1.0.1";
    public static string Description = "Plugins that allow to track/refresh buff such as experience potion...";
    // Variables de debug
    public static bool BDebugVerboseFull = false;
    // Variable de test du loot
    public static bool BLootInProcess = false;
    // Variable de test pour le mouvement, si il fait qqch, combat, etc...
    public static bool BBotInAction = false;
    // Settings
    private static readonly MyPluginSettings MySettings = MyPluginSettings.GetSettings();


    public static void Init()
    {
        Logging.Write(string.Format("The plugin {0} is loaded...", Name));

        // Do some init stuff here.
        MainLoop();
    }

    // Action principale
    public static void MainLoop()
    {
        while (InternalLoop)
        {
            // Si je dois looter les créatures et que le follow est pas en combat et moi non plus...
            if (!ObjectManager.Me.InCombat && ObjectManager.Me.IsAlive)
            {
                // ********************************************************************************
                // Section de l'item
                string sItemCount = "ITEM 01 : ";
                string sItemname = MySettings.SItem01;
                string sBuffname = MySettings.SBuff01;
                // Si il faut utilise l'objet                
                if (MySettings.BUseItem01 && sItemname != "" && sBuffname != "")
                {
                    VoidCheckItem(sItemCount, sItemname, sBuffname);
                }
                // ********************************************************************************
                // Section de l'item
                sItemCount = "ITEM 02 : ";
                sItemname = MySettings.SItem02;
                sBuffname = MySettings.SBuff02;
                // Si il faut utilise l'objet                
                if (MySettings.BUseItem02 && sItemname != "" && sBuffname != "")
                {
                    VoidCheckItem(sItemCount, sItemname, sBuffname);
                }
                // ********************************************************************************
                // Section de l'item
                sItemCount = "ITEM 03 : ";
                sItemname = MySettings.SItem03;
                sBuffname = MySettings.SBuff03;
                // Si il faut utilise l'objet                
                if (MySettings.BUseItem03 && sItemname != "" && sBuffname != "")
                {
                    VoidCheckItem(sItemCount, sItemname, sBuffname);
                }
                // ********************************************************************************
                // Section de l'item
                sItemCount = "ITEM 04 : ";
                sItemname = MySettings.SItem04;
                sBuffname = MySettings.SBuff04;
                // Si il faut utilise l'objet                
                if (MySettings.BUseItem04 && sItemname != "" && sBuffname != "")
                {
                    VoidCheckItem(sItemCount, sItemname, sBuffname);
                }
                // ********************************************************************************
                // Section de l'item
                sItemCount = "ITEM 05 : ";
                sItemname = MySettings.SItem05;
                sBuffname = MySettings.SBuff05;
                // Si il faut utilise l'objet                
                if (MySettings.BUseItem05 && sItemname != "" && sBuffname != "")
                {
                    VoidCheckItem(sItemCount, sItemname, sBuffname);
                }
                // ********************************************************************************
                // Section de l'item
                sItemCount = "ITEM 06 : ";
                sItemname = MySettings.SItem06;
                sBuffname = MySettings.SBuff06;
                // Si il faut utilise l'objet                
                if (MySettings.BUseItem06 && sItemname != "" && sBuffname != "")
                {
                    VoidCheckItem(sItemCount, sItemname, sBuffname);
                }
                // ********************************************************************************
                // Section de l'item
                sItemCount = "ITEM 07 : ";
                sItemname = MySettings.SItem07;
                sBuffname = MySettings.SBuff07;
                // Si il faut utilise l'objet                
                if (MySettings.BUseItem07 && sItemname != "" && sBuffname != "")
                {
                    VoidCheckItem(sItemCount, sItemname, sBuffname);
                }
                // ********************************************************************************
                // Section de l'item
                sItemCount = "ITEM 08 : ";
                sItemname = MySettings.SItem08;
                sBuffname = MySettings.SBuff08;
                // Si il faut utilise l'objet                
                if (MySettings.BUseItem08 && sItemname != "" && sBuffname != "")
                {
                    VoidCheckItem(sItemCount, sItemname, sBuffname);
                }
                // ********************************************************************************
                // Section de l'item
                sItemCount = "ITEM 09 : ";
                sItemname = MySettings.SItem09;
                sBuffname = MySettings.SBuff09;
                // Si il faut utilise l'objet                
                if (MySettings.BUseItem09 && sItemname != "" && sBuffname != "")
                {
                    VoidCheckItem(sItemCount, sItemname, sBuffname);
                }
            }
        }
    }

    public static void VoidCheckItem(string sItemDescID, string sItemname, string sBuffname)
    {
        // ********************************************************************************
        int iItemIDToUse = ItemsManager.GetItemIdByName(sItemname);
        // Logging.WriteDebug(ItemsManager.GetItemCount(iItemIDToUse).ToString());

        // Teste si l'objet est bien dans l'inventaire avant de...
        if (ItemsManager.GetItemCount(iItemIDToUse) != 0)
        {
            Logging.WriteDebug(sItemDescID);
            Logging.WriteDebug(sItemDescID + "Item name : " + sItemname + " : ItemID " + iItemIDToUse);
            Logging.WriteDebug(sItemDescID + "Buff name : " + sBuffname + " : " + SpellManager.HaveBuffLua(sBuffname));

            // Si l'item est pas en cooldown et que le buff est pas présent alors...
            if (!ItemsManager.IsItemOnCooldown(iItemIDToUse) && !SpellManager.HaveBuffLua(sBuffname))
            {
                Logging.WriteDebug(sItemDescID + "Have Buff = No");
                // lance l'objet
                ItemsManager.UseItem(sItemname);
            }
            else
            {
                // Juste un message de debug
                Logging.WriteDebug(sItemDescID + "Have Buff = Yes");
            }
            Thread.Sleep(Usefuls.Latency + 100);
            // ********************************************************************************
        }
        else
        {
            Logging.Write("!!! Warning !!! : " + sItemDescID + " : " + sItemname + " is not part of your inventory.");
            Logging.WriteError("!!! Warning !!! : " + sItemDescID + " : " + sItemname + " is not part of your inventory.");
        }
    }

    // ****************************************************************************************************************************************
    // Bouton de reset des configurations du pluggin
    public static void ResetConfiguration()
    {
        // Affecte le fichier de config XML à la variable
        string currentSettingsFile = Application.StartupPath + "\\Plugins\\Settings\\" + Name + "_" + ObjectManager.Me.Name + ".xml";
        var currentSetting = new MyPluginSettings();
        // Reset les champs
        currentSetting.ToForm();
        // Sauvegarde la config
        currentSetting.Save(currentSettingsFile);
    }

    // Bouton afficher la configuration du pluggin
    public static void ShowConfiguration()
    {
        // Affecte le fichier de config XML à la variable
        string currentSettingsFile = Application.StartupPath + "\\Plugins\\Settings\\" + Name + "_" + ObjectManager.Me.Name + ".xml";
        var currentSetting = new MyPluginSettings();
        // Si le fichier existe 
        if (File.Exists(currentSettingsFile))
        {
            // Charge les paramètres
            currentSetting = Settings.Load<MyPluginSettings>(currentSettingsFile);
        }
        currentSetting.ToForm();
        // Sauvegarde la config
        currentSetting.Save(currentSettingsFile);
    }

    #region configurations du pluggin

    // Class de Settings
    [Serializable]
    public class MyPluginSettings : Settings
    {
        // bouton on/off
        public bool BUseItem01 = false;
        public bool BUseItem02 = false;
        public bool BUseItem03 = false;
        public bool BUseItem04 = false;
        public bool BUseItem05 = false;
        public bool BUseItem06 = false;
        public bool BUseItem07 = false;
        public bool BUseItem08 = false;
        public bool BUseItem09 = false;
        public string SBuff01 = "Apprentissage accéléré";
        public string SBuff02 = "Murmures de démence";
        public string SBuff03 = "Whispers of Insanity	";
        public string SBuff04 = "Accelerated Learning";
        public string SBuff05 = "";
        public string SBuff06 = "";
        public string SBuff07 = "";
        public string SBuff08 = "";
        public string SBuff09 = "";
        public string SItem01 = "Potion excessive d’apprentissage accéléré";
        public string SItem02 = "Cristal murmurant d’Oralius";
        public string SItem03 = "Oralius' Whispering Crystal";
        public string SItem04 = "Excess Potion of Accelerated Learning";
        public string SItem05 = "";
        public string SItem06 = "";
        public string SItem07 = "";
        public string SItem08 = "";
        public string SItem09 = "";

        public MyPluginSettings()
        {
            // Informations affichées dans la fenêtre de config
            ConfigWinForm("Plugin management");
            AddControlInWinForm("01 Use Item ?", "bUseItem01", "Configuration", "List");
            AddControlInWinForm("01 Item Name : ", "sItem01", "Configuration", "List");
            AddControlInWinForm("01 Item Buff Name : ", "sBuff01", "Configuration", "List");

            AddControlInWinForm("02 Use Item ?", "bUseItem02", "Configuration", "List");
            AddControlInWinForm("02 Item Name : ", "sItem02", "Configuration", "List");
            AddControlInWinForm("02 Item Buff Name : ", "sBuff02", "Configuration", "List");

            AddControlInWinForm("03 Use Item ?", "bUseItem03", "Configuration", "List");
            AddControlInWinForm("03 Item Name : ", "sItem03", "Configuration", "List");
            AddControlInWinForm("03 Item Buff Name : ", "sBuff03", "Configuration", "List");

            AddControlInWinForm("04 Use Item ?", "bUseItem04", "Configuration", "List");
            AddControlInWinForm("04 Item Name : ", "sItem04", "Configuration", "List");
            AddControlInWinForm("04 Item Buff Name : ", "sBuff04", "Configuration", "List");

            AddControlInWinForm("05 Use Item ?", "bUseItem05", "Configuration", "List");
            AddControlInWinForm("05 Item Name : ", "sItem05", "Configuration", "List");
            AddControlInWinForm("05 Item Buff Name : ", "sBuff05", "Configuration", "List");

            AddControlInWinForm("06 Use Item ?", "bUseItem06", "Configuration", "List");
            AddControlInWinForm("06 Item Name : ", "sItem06", "Configuration", "List");
            AddControlInWinForm("06 Item Buff Name : ", "sBuff06", "Configuration", "List");

            AddControlInWinForm("07 Use Item ?", "bUseItem07", "Configuration", "List");
            AddControlInWinForm("07 Item Name : ", "sItem07", "Configuration", "List");
            AddControlInWinForm("07 Item Buff Name : ", "sBuff07", "Configuration", "List");

            AddControlInWinForm("08 Use Item ?", "bUseItem08", "Configuration", "List");
            AddControlInWinForm("08 Item Name : ", "sItem08", "Configuration", "List");
            AddControlInWinForm("08 Item Buff Name : ", "sBuff08", "Configuration", "List");

            AddControlInWinForm("09 Use Item ?", "bUseItem09", "Configuration", "List");
            AddControlInWinForm("09 Item Name : ", "sItem09", "Configuration", "List");
            AddControlInWinForm("09 Item Buff Name : ", "sBuff09", "Configuration", "List");
        }

        // Class permettant le get et set
        public static MyPluginSettings CurrentSetting { get; set; }
        // fonction Get
        public static MyPluginSettings GetSettings()
        {
            // Affecte le fichier de config XML à la variable
            string currentSettingsFile = Application.StartupPath + "\\Plugins\\Settings\\" + Name + "_" + ObjectManager.Me.Name + ".xml";
            // Si le fichier existe 
            if (File.Exists(currentSettingsFile))
            {
                // Charge les paramètres
                return CurrentSetting = Load<MyPluginSettings>(currentSettingsFile);
            }
            // Renvoie l'objet en retour de fonctions
            return new MyPluginSettings();
        }
    }
}

#endregion

#endregion