using System;
using System.Collections.Generic;
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
        get { return WhisperForwarding.InternalLoop; }
        set { WhisperForwarding.InternalLoop = value; }
    }

    public string Name
    {
        get { return WhisperForwarding.Name; }
    }

    public string Author
    {
        get { return WhisperForwarding.Author; }
    }

    public string Description
    {
        get { return WhisperForwarding.Description; }
    }

    public string TargetVersion
    {
        get { return WhisperForwarding.TargetVersion; }
    }

    public string Version
    {
        get { return WhisperForwarding.Version; }
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
        WhisperForwarding.ShowConfiguration();
    }

    public void ResetConfiguration()
    {
        WhisperForwarding.ResetConfiguration();
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
            WhisperForwarding.Init();
        }
        catch (Exception e)
        {
            Logging.WriteError("IPlugins.Main.Initialize(bool configOnly, bool resetSettings = false): " + e);
        }
        Logging.WriteDebug(string.Format("The plugin {0} has stopped.", Name));
    }
}

#endregion

#region Plugin core Class

public static class WhisperForwarding
{
    // Variable permettant de faire tourner en boucle le pluggin
    public static bool InternalLoop = true;
    public static string Author = "CEREAL";
    public static string Name = "Whisper Forwarding";
    public static string TargetVersion = "6.0.x";
    public static string Version = "1.0.4";
    public static string Description = "Forwards every whispers to a defined Master Player.";
    private static readonly WhisperForwardingSettings MySettings = WhisperForwardingSettings.GetSettings();

    #region Init()

    public static void Init()
    {
        MainLoop();
    }

    #endregion

    #region MainLoop()

    private static readonly Channel WhisperChannel = new Channel();

    private static KeyValuePair<string, string> ExtractPlayerNameAndMessage(string msg)
    {
        //  "11/29/2015 14:37:50 - Zazalaouf-Dalaran [Whisper] : zo\r\n"
        if (msg.Length > 22)
        {
            string senderName = msg.Substring(22, msg.Length - 22).Split('[')[0].Trim();
            if (senderName == "|Kb17|k0000000|k")
                senderName = "REAL ID";
            string[] tempMsg = msg.Split(':');
            string content = "";
            for (int i = 3; i < tempMsg.Length; i++)
            {
                if (i == 3)
                    content = content + tempMsg[i];
                else
                    content = content + ":" + tempMsg[i];
            }
            return new KeyValuePair<string, string>(senderName, content.Trim());
        }
        return new KeyValuePair<string, string>("", ""); // invalid whisper
    }

    public static void MainLoop()
    {
        Logging.WritePlugin("Start plugin : " + Name, Name);

        while (InternalLoop)
        {
            while (WhisperChannel.CurrentMsg < WhisperChannel.GetCurrentMsgInWow)
            {
                string msg = WhisperChannel.ReadWhisperAndRealIdChannel();
                if (!String.IsNullOrWhiteSpace(msg))
                {
                    KeyValuePair<string, string> whisp = ExtractPlayerNameAndMessage(msg);
                    string sPlayerName = MySettings.ForwardWhispersToPlayerName;
                    if (MySettings.ActivateWhisperForwarding)
                    {
                        string resultString = "CHAT message from [" + whisp.Key + "] : {" + whisp.Value + "}";
                        Logging.WritePlugin("Fowarding Whisper received to " + sPlayerName + ", Content : " + resultString, Name);
                        Lua.RunMacroText("/w " + sPlayerName + " " + resultString);
                        Thread.Sleep(Usefuls.Latency + 500);
                    }
                }
            }

            Thread.Sleep(200);
        }
    }

    #endregion

    public static void ResetConfiguration()
    {
        string currentSettingsFile = Application.StartupPath + "\\Plugins\\Settings\\" + Name + "_" + ObjectManager.Me.Name + ".xml";
        var currentSetting = new WhisperForwardingSettings();
        currentSetting.ToForm();
        currentSetting.Save(currentSettingsFile);
    }

    public static void ShowConfiguration()
    {
        string currentSettingsFile = Application.StartupPath + "\\Plugins\\Settings\\" + Name + "_" + ObjectManager.Me.Name + ".xml";
        var currentSetting = new WhisperForwardingSettings();
        if (File.Exists(currentSettingsFile))
        {
            currentSetting = Settings.Load<WhisperForwardingSettings>(currentSettingsFile);
        }
        currentSetting.ToForm();
        currentSetting.Save(currentSettingsFile);
    }

    [Serializable]
    public class WhisperForwardingSettings : Settings
    {
        public bool ActivateWhisperForwarding = true;
        public string ForwardWhispersToPlayerName = "";

        public WhisperForwardingSettings()
        {
            ConfigWinForm("Whisper Fowarding Settings");
            AddControlInWinForm("Activate Whisper Forwarding ? ", "ActivateWhisperForwarding", "Settings", "List");
            AddControlInWinForm("Foward whispers to player (format : Playername-Realmname :", "ForwardWhispersToPlayerName", "Settings", "List");
            AddControlInWinForm("Player names to conserve, separated by comma [,] :", "LigneVide2", "Settings", "List");
            // dunno about this
        }

        public static WhisperForwardingSettings CurrentSetting { get; set; }

        public static WhisperForwardingSettings GetSettings()
        {
            // Affecte le fichier de config XML à la variable
            string currentSettingsFile = Application.StartupPath + "\\Plugins\\Settings\\" + Name + "_" + ObjectManager.Me.Name + ".xml";
            // Si le fichier existe 
            if (File.Exists(currentSettingsFile))
            {
                // Charge les paramètres
                return CurrentSetting = Load<WhisperForwardingSettings>(currentSettingsFile);
            }
            // Renvoie l'objet en retour de fonctions
            return new WhisperForwardingSettings();
        }
    }
}

#endregion