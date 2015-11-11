using System;
using System.IO;
using System.Media;
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
        Loop = false;
    }

    public void Initialize()
    {
        Logging.WriteDebug(string.Format("The plugin {0} is loading.", Name));
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
        Logging.WriteDebug(string.Format("The plugin {0} has stopped.", Name));
    }
}

#endregion

#region Plugin core Class

#region Declaration variables

public static class MyPluginClass
{
    #region configurations du pluggin

    // Variable permettant de faire tourner en boucle le pluggin
    public static bool InternalLoop = true;
    // Informations relatives à la version du pluggin
    public static string Author = "CEREAL";
    public static string Name = "Whisper Forwarding";
    public static string TargetVersion = "4.8.x"; // Only the two first numbers are checked.
    public static string Version = "BETA 1.0.0";
    public static string Description = "Forward every chat/whisp that bot receive to another player";

    // Whisp chanel
    public static readonly Channel Whisp = new Channel();

    // Settings
    private static readonly MyPluginSettings MySettings = MyPluginSettings.GetSettings();

    // Son
    public static bool bJouerSon = false;

    // Variables de debug
    public static bool bDebugVerboseLight = false;
    public static bool bDebugVerboseFull = false;

    #region Init()

    public static void Init()
    {
        // Démarre l'instance de chat
        Logging.Write("Start Thread ChatChannel Class");
        ChatMonitor.ThreadChatChannel();

        // Charge la boucle principale, tâches d'initialisation ici
        MainLoop();
    }

    #endregion

    #region MainLoop()

    public static void MainLoop()
    {
        Logging.Write("Start plugin : " + Name);

        while (InternalLoop)
        {
            // Permet de gérer les messages arrivant dans le chat
            // "[Addon]";
            // "[Say]";
            // "[Party]";
            // "[Raid]";
            // "[Guild]";
            // "[Officers]";
            // "[Yell]";
            // "[Whisper]";
            // "[Monster Whisper]";
            // "[To]";
            // "[Emote]";
            // "[General]";
            // "[Loot]";
            // "[Real Id Whisper]";
            // "[Real Id To]";
            //     MySettings.ActiveChatForward
            //     MySettings.ChatForwardWhispPlayer
            //     MySettings.ChatPause
            //     MySettings.ChatStart
            string sMsgChat = ChatMonitor.ChatMessageContent;
            if (sMsgChat.Contains("[Real Id Whisper]") | sMsgChat.Contains("[Whisper]"))
            {
                // Si il faut renvoyer les messages à un autre joueur
                if (MySettings.ActiveChatForward)
                {
                    if (bDebugVerboseFull)
                    {
                        Logging.WriteDebug("MySettings.ActiveChatForward : " + MySettings.ActiveChatForward);
                    }

                    // string sPlayerName = "Bankkiss-Vol'jin";
                    string sPlayerName = MySettings.ChatForwardWhispPlayer;
                    int iL = 0;
                    if (iL <= 1)
                    {
                        // Concatène la chaine à renvoyer dans le chat
                        string sTMP = string.Empty;
                        sTMP = sMsgChat.Remove(sMsgChat.LastIndexOf(":"));
                        sTMP = sMsgChat.Replace(sTMP, string.Empty).Replace(":", string.Empty);
                        sTMP = sTMP.Trim();
                        sTMP = "CHAT message from [" + sPlayerName + "] : {" + sTMP + "}";

                        Logging.Write("Renvoie du chat vers : " + sPlayerName + " Contenu : " + sMsgChat);
                        Lua.LuaDoString("SendChatMessage(\"" + sTMP + "\", \"WHISPER\", nil, \"" + sPlayerName + "\");", false, true);
                        sMsgChat = string.Empty;
                        ChatMonitor.ChatMessageContent = string.Empty;
                        Thread.Sleep(Usefuls.Latency + 500);
                        iL++;
                    }
                }
            }
            Thread.Sleep(Usefuls.Latency + 500);
            Application.DoEvents();
        }
    }

    #endregion

    #region lancement d'une alerte sonore

    // Joue un son
    public static void JouerSonHorsPortee()
    {
        var player = new SoundPlayer
        {
            SoundLocation = (Application.StartupPath + "\\Plugins\\sound\\hors_portee.wav")
        };
        if (bJouerSon)
        {
            player.PlaySync();
        }
        else
        {
            player.Stop();
        }
    }

    #endregion

    #endregion

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

    // Class de Settings
    [Serializable]
    public class MyPluginSettings : Settings
    {
        // bouton on/off
        public bool ActiveChatForward = true;
        // champ de caractères
        public string ChatForwardWhispPlayer = "";

        public MyPluginSettings()
        {
            // Informations affichées dans la fenêtre de config
            ConfigWinForm("Gestion du pluggin");
            // Ajoute le texte + le nom du bouton et sa fonction ci-dessus
            AddControlInWinForm("Enable chat forwarding to another player ? ", "ActiveChatForward", "Options", "List");
            // Ajoute le texte + le nom du bouton et sa fonction ci-dessus
            AddControlInWinForm("Player name to forward whisp automatically (format : {Playername-Realmname} :", "ChatForwardWhispPlayer", "Options", "List");
            // Ajoute le texte + le nom du bouton et sa fonction ci-dessus
            AddControlInWinForm("Player names to conserve, separated by comma [,] :", "LigneVide2", "Options", "List");
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

#region Chat Monitor Class

public static class ChatMonitor
{
    // Propriété contenant le message renvoyé
    private static string _sChatMessage = string.Empty;
    public static int iLoop = 0;

    public static string ChatMessageContent
    {
        get { return _sChatMessage; }
        set { _sChatMessage = value; }
    }

    // void permettant d'instancier le thread
    // "[Addon]";
    // "[Say]";
    // "[Party]";
    // "[Raid]";
    // "[Guild]";
    // "[Officers]";
    // "[Yell]";
    // "[Whisper]";
    // "[Monster Whisper]";
    // "[To]";
    // "[Emote]";
    // "[General]";
    // "[Loot]";
    // "[Real Id Whisper]";
    // "[Real Id To]";
    public static void ThreadChatChannel()
    {
        var thread = new Thread(WhispAlert)
        {
            Name = "Thread Chat Monitoring"
        };
        thread.Start();
        Logging.WriteDebug("{ThreadChatChannel} : " + thread.ThreadState);
    }

    public static void WhispAlert()
    {
        Logging.Write("Chatmonitor Start !");
        var cChatChan2 = new Channel();
        bool bLoop = true;
        while (bLoop)
        {
            // Si la boucle atteint 1 passage détruit l'objet chat et le recrée car il y a un bug
            // le bot ne renvoie plus de messages au bout d'un moment alors je réinstancie l'objet
            if (iLoop == 1)
            {
                // Logging.Write("Netoyage de l'objet cChatChan2");
                cChatChan2 = null;
                cChatChan2 = new Channel();
                iLoop = 0;
            }

            // Lis le contenu du chat
            string str = cChatChan2.ReadAllChannel();

            // Logging.Write(iLoop + " Whisp : " + " : " + str.ToString());

            // Si il y a qqch de nouveau alors...
            if (!string.IsNullOrEmpty(str))
            {
                // Logging.WriteDebug(iLoop + " Whisp : " + " : " + str.ToString().Trim());
                // Affecte la valeur du message en chat
                ChatMessageContent = str.Trim();
                iLoop++;
            }
            else
            {
                // Vide la variable de message
                _sChatMessage = string.Empty;
            }
            Thread.Sleep(Usefuls.Latency + 1000);
        }
    }
}

#endregion