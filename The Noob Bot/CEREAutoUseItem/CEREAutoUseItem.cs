using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using nManager.Plugins;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
using nManager.Products;

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
    public static string TargetVersion = "6.0.x";
    public static string Version = "1.0.5";
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
        Logging.WritePlugin(string.Format("The plugin {0} is loaded...", Name), Name);

        // Do some init stuff here.
        MainLoop();
    }

    // Action principale
    public static void MainLoop()
    {
        while (InternalLoop)
        {
            if (Products.IsStarted)
            {
                // Si je dois looter les créatures et que le follow est pas en combat et moi non plus...
                if (!ObjectManager.Me.InCombat && ObjectManager.Me.IsAlive)
                {
                    // ********************************************************************************
                    // Retour de fonction permettant de désactiver l'item quand il y en a plus dans l'inventaire
                    bool bReturn = true;
                    // Section de l'item
                    string sItemCount = "ITEM 01 : ";
                    string sItemname = MySettings.sItem01;
                    string sBuffname = MySettings.sBuff01;
                    // Si il faut utilise l'objet                
                    if (MySettings.bUseItem01 && sItemname != "" && sBuffname != "")
                    {
                        bReturn = boolCheckItem(sItemCount, sItemname, sBuffname);
                        // Si return = false déactive l'utilisation de l'item
                        if (bReturn == false)
                        {
                            MySettings.bUseItem01 = false;
                        }
                    }
                    // ********************************************************************************
                    // Section de l'item
                    sItemCount = "ITEM 02 : ";
                    sItemname = MySettings.sItem02;
                    sBuffname = MySettings.sBuff02;
                    // Si il faut utilise l'objet                
                    if (MySettings.bUseItem02 && sItemname != "" && sBuffname != "")
                    {
                        bReturn = boolCheckItem(sItemCount, sItemname, sBuffname);
                        // Si return = false déactive l'utilisation de l'item
                        if (bReturn == false)
                        {
                            MySettings.bUseItem02 = false;
                        }
                    }
                    // ********************************************************************************
                    // Section de l'item
                    sItemCount = "ITEM 03 : ";
                    sItemname = MySettings.sItem03;
                    sBuffname = MySettings.sBuff03;
                    // Si il faut utilise l'objet                
                    if (MySettings.bUseItem03 && sItemname != "" && sBuffname != "")
                    {
                        bReturn = boolCheckItem(sItemCount, sItemname, sBuffname);
                        // Si return = false déactive l'utilisation de l'item
                        if (bReturn == false)
                        {
                            MySettings.bUseItem03 = false;
                        }
                    }
                    // ********************************************************************************
                    // Section de l'item
                    sItemCount = "ITEM 04 : ";
                    sItemname = MySettings.sItem04;
                    sBuffname = MySettings.sBuff04;
                    // Si il faut utilise l'objet                
                    if (MySettings.bUseItem04 && sItemname != "" && sBuffname != "")
                    {
                        bReturn = boolCheckItem(sItemCount, sItemname, sBuffname);
                        // Si return = false déactive l'utilisation de l'item
                        if (bReturn == false)
                        {
                            MySettings.bUseItem04 = false;
                        }
                    }
                    // ********************************************************************************
                    // Section de l'item
                    sItemCount = "ITEM 05 : ";
                    sItemname = MySettings.sItem05;
                    sBuffname = MySettings.sBuff05;
                    // Si il faut utilise l'objet                
                    if (MySettings.bUseItem05 && sItemname != "" && sBuffname != "")
                    {
                        bReturn = boolCheckItem(sItemCount, sItemname, sBuffname);
                        // Si return = false déactive l'utilisation de l'item
                        if (bReturn == false)
                        {
                            MySettings.bUseItem05 = false;
                        }
                    }
                    // ********************************************************************************
                    // Section de l'item
                    sItemCount = "ITEM 06 : ";
                    sItemname = MySettings.sItem06;
                    sBuffname = MySettings.sBuff06;
                    // Si il faut utilise l'objet                
                    if (MySettings.bUseItem06 && sItemname != "" && sBuffname != "")
                    {
                        bReturn = boolCheckItem(sItemCount, sItemname, sBuffname);
                        // Si return = false déactive l'utilisation de l'item
                        if (bReturn == false)
                        {
                            MySettings.bUseItem06 = false;
                        }
                    }
                    // ********************************************************************************
                    // Section de l'item
                    sItemCount = "ITEM 07 : ";
                    sItemname = MySettings.sItem07;
                    sBuffname = MySettings.sBuff07;
                    // Si il faut utilise l'objet                
                    if (MySettings.bUseItem07 && sItemname != "" && sBuffname != "")
                    {
                        bReturn = boolCheckItem(sItemCount, sItemname, sBuffname);
                        // Si return = false déactive l'utilisation de l'item
                        if (bReturn == false)
                        {
                            MySettings.bUseItem07 = false;
                        }
                    }
                    // ********************************************************************************
                    // Section de l'item
                    sItemCount = "ITEM 08 : ";
                    sItemname = MySettings.sItem08;
                    sBuffname = MySettings.sBuff08;
                    // Si il faut utilise l'objet                
                    if (MySettings.bUseItem08 && sItemname != "" && sBuffname != "")
                    {
                        bReturn = boolCheckItem(sItemCount, sItemname, sBuffname);
                        // Si return = false déactive l'utilisation de l'item
                        if (bReturn == false)
                        {
                            MySettings.bUseItem08 = false;
                        }
                    }
                    // ********************************************************************************
                    // Section de l'item
                    sItemCount = "ITEM 09 : ";
                    sItemname = MySettings.sItem09;
                    sBuffname = MySettings.sBuff09;
                    // Si il faut utilise l'objet                
                    if (MySettings.bUseItem09 && sItemname != "" && sBuffname != "")
                    {
                        bReturn = boolCheckItem(sItemCount, sItemname, sBuffname);
                        // Si return = false déactive l'utilisation de l'item
                        if (bReturn == false)
                        {
                            MySettings.bUseItem09 = false;
                        }
                    }
                }
            }
        }
    }

    public static bool boolCheckItem(string sItemDescID, string sItemname, string sBuffname)
    {
        // ********************************************************************************
        int iItemIDToUse = ItemsManager.GetItemIdByName(sItemname);
        // Logging.WritePluginDebug(ItemsManager.GetItemCount(iItemIDToUse).ToString());

        // Teste si l'objet est bien dans l'inventaire avant de...
        if (ItemsManager.GetItemCount(iItemIDToUse) > 0)
        {
            Logging.WritePluginDebug(sItemDescID, Name);
            Logging.WritePluginDebug(sItemDescID + "Item name : " + sItemname + " : ItemID " + iItemIDToUse, Name);
            Logging.WritePluginDebug(sItemDescID + "Buff name : " + sBuffname + " : " + SpellManager.HaveBuffLua(sBuffname), Name);

            // Si l'item est pas en cooldown et que le buff est pas présent alors...
            if (!ItemsManager.IsItemOnCooldown(iItemIDToUse) && !SpellManager.HaveBuffLua(sBuffname))
            {
                Logging.WritePluginDebug(sItemDescID + "Have Buff = No", Name);
                // lance l'objet
                ItemsManager.UseItem(sItemname);
                return true;
            }
            else
            {
                // Juste un message de debug
                Logging.WritePluginDebug(sItemDescID + "Have Buff = Yes", Name);
            }
            Thread.Sleep(Usefuls.Latency + 100);
            // ********************************************************************************
            return true;
        }
        else
        {
            Logging.WritePlugin("********************************************************", Name);
            Logging.WritePlugin("!!! Warning !!! : " + sItemDescID + " : " + sItemname + " is not part of your inventory.", Name);
            Logging.WritePlugin("********************************************************", Name);
            Logging.WritePluginError("!!! Warning !!! : " + sItemDescID + " : " + sItemname + " is not part of your inventory.", Name);
            return false;
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
        public bool bStartProduct = true;
        public bool bUseItem01 = false;
        public bool bUseItem02 = false;
        public bool bUseItem03 = false;
        public bool bUseItem04 = false;
        public bool bUseItem05 = false;
        public bool bUseItem06 = false;
        public bool bUseItem07 = false;
        public bool bUseItem08 = false;
        public bool bUseItem09 = false;
        public string sBuff01 = "Apprentissage accéléré";
        public string sBuff02 = "Murmures de démence";
        public string sBuff03 = "Whispers of Insanity	";
        public string sBuff04 = "Accelerated Learning";
        public string sBuff05 = "";
        public string sBuff06 = "";
        public string sBuff07 = "";
        public string sBuff08 = "";
        public string sBuff09 = "";
        public string sItem01 = "Potion excessive d’apprentissage accéléré";
        public string sItem02 = "Cristal murmurant d’Oralius";
        public string sItem03 = "Oralius' Whispering Crystal";
        public string sItem04 = "Excess Potion of Accelerated Learning";
        public string sItem05 = "";
        public string sItem06 = "";
        public string sItem07 = "";
        public string sItem08 = "";
        public string sItem09 = "";

        public MyPluginSettings()
        {
            // Informations affichées dans la fenêtre de config
            ConfigWinForm("Plugin management");

            AddControlInWinForm("Activate pluggin only when product is started [START PRODUCT] ?", "bStartProduct", "Configuration", "List");

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