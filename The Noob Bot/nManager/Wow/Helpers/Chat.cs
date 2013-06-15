using System;
using nManager.Helpful;
using nManager.Wow.Patchables;

namespace nManager.Wow.Helpers
{
    public class Chat
    {
        public static void SendChatMessage(String message)
        {
            try
            {
                Lua.LuaDoString("SendChatMessage(\"" + message + "\");");
            }
            catch (Exception exception)
            {
                Logging.WriteError("SendChatMessage(String message): " + exception);
            }
        }

        public static void SendChatMessageWhisperAtTarget(String message)
        {
            try
            {
                Lua.LuaDoString("SendChatMessage(\"" + message + "\", \"whisper\", nil, UnitName(\"target\"));");
            }
            catch (Exception exception)
            {
                Logging.WriteError("SendChatMessageWhisperAtTarget(String message): " + exception);
            }
        }

        public static void SendChatMessageWhisper(String message, String playerName)
        {
            try
            {
                Lua.LuaDoString("SendChatMessage(\"" + message + "\", \"whisper\", nil, \"" + playerName + "\");");
            }
            catch (Exception exception)
            {
                Logging.WriteError("SendChatMessageWhisper(String message, String playerName): " + exception);
            }
        }

        public static void SendChatMessageGuild(String message)
        {
            try
            {
                Lua.LuaDoString("SendChatMessage(\"" + message + "\", \"GUILD\");");
            }
            catch (Exception exception)
            {
                Logging.WriteError("SendChatMessageGuild(String message): " + exception);
            }
        }
    }

    public class Channel
    {
        #region Fields

        public int ActuelRead;

        public int GetMsgActuelInWow
        {
            get
            {
                try
                {
                    return
                        Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule +
                                                        (uint) Addresses.Chat.chatBufferPos);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("GetMsgActuelInWow: " + exception);
                }
                return 0;
            }
        }

        #endregion Fields

        #region Constructors

        public Channel()
        {
            try
            {
                GetMsgActuel();
            }
            catch (Exception exception)
            {
                Logging.WriteError("Channel(): " + exception);
            }
        }

        #endregion Constructors

        #region Methods

        public static string ReadLastMsg()
        {
            try
            {
                return
                    Memory.WowMemory.Memory.ReadUTF8String(
                        (uint)
                        (Memory.WowProcess.WowModule + (uint) Addresses.Chat.chatBufferStart +
                         (uint) Addresses.Chat.msgFormatedChat +
                         (int) Addresses.Chat.NextMessage*
                         Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule +
                                                         (uint) Addresses.Chat.chatBufferPos)));
            }
            catch (Exception exception)
            {
                Logging.WriteError("ReadLastMsg(): " + exception);
            }
            return "";
        }

        public string ReadAllChannel()
        {
            try
            {
                var unMsg = ReadMsg();
                if (unMsg.Msg != null)
                {
                    return DateTime.Now + " - " + unMsg.Pseudo + " " + getCanal(unMsg.Canal) + " : " + unMsg.Msg +
                           "\r\n";
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ReadAllChannel(): " + exception);
            }
            return "";
        }

        public string ReadGuildChannel()
        {
            try
            {
                var unMsg = ReadMsg();
                if (unMsg.Msg != null)
                {
                    if (unMsg.Canal == 4)
                    {
                        return DateTime.Now + " - " + unMsg.Pseudo + " " + getCanal(unMsg.Canal) + " : " + unMsg.Msg +
                               "\r\n";
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ReadGuildChannel(): " + exception);
            }
            return "";
        }

        public string ReadSayChannel()
        {
            try
            {
                var unMsg = ReadMsg();
                if (unMsg.Msg != null)
                {
                    if (unMsg.Canal == 1)
                    {
                        return DateTime.Now + " - " + unMsg.Pseudo + " " + getCanal(unMsg.Canal) + " : " + unMsg.Msg +
                               "\r\n";
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ReadSayChannel(): " + exception);
            }
            return "";
        }

        public string ReadWhisperChannel()
        {
            try
            {
                var unMsg = ReadMsg();
                if (unMsg.Msg != null)
                {
                    if (unMsg.Canal == 7)
                    {
                        return DateTime.Now + " - " + unMsg.Pseudo + " " + getCanal(unMsg.Canal) + " : " + unMsg.Msg +
                               "\r\n";
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ReadWhisperChannel(): " + exception);
            }
            return "";
        }

        private void GetMsgActuel()
        {
            try
            {
                ActuelRead =
                    Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + (uint) Addresses.Chat.chatBufferPos);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetMsgActuel(): " + exception);
            }
        }

        private Message ReadMsg()
        {
            try
            {
                if (ActuelRead > 59)
                    ActuelRead = 0;

                if (ActuelRead >=
                    Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule +
                                                    (uint) Addresses.Chat.chatBufferPos) &&
                    (ActuelRead -
                     Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule +
                                                     (uint) Addresses.Chat.chatBufferPos)) <= 2)
                    return new Message();

                string lecture =
                    Memory.WowMemory.Memory.ReadUTF8String(
                        (uint)
                        (Memory.WowProcess.WowModule + (uint) Addresses.Chat.chatBufferStart +
                         (uint) Addresses.Chat.msgFormatedChat + (int) Addresses.Chat.NextMessage*(ActuelRead)));
                var unMsg = new Message();
                if (lecture != "")
                {
                    ActuelRead++;
                    unMsg.Canal = Others.ToInt32(stringBetween(lecture, "Type: [", "]"));
                    unMsg.Pseudo = Others.ToUtf8(stringBetween(lecture, "Name: [", "]"));
                    unMsg.Msg = Others.ToUtf8(stringBetween(lecture, "Text: [", "]"));
                }

                return unMsg;
            }
            catch (Exception exception)
            {
                Logging.WriteError("Chat > ReadMsg(): " + exception);
                return new Message();
            }
        }

        private string getCanal(int canal)
        {
            try
            {
                switch (canal)
                {
                    case 0:
                        return "[Addon]";
                    case 1:
                        return "[Say]";
                    case 2:
                        return "[Groupe]";
                    case 3:
                        return "[Raid]";
                    case 4:
                        return "[Guild]";
                    case 5:
                        return "[Officier]";
                    case 6:
                        return "[Cri]";
                    case 7:
                        return "[Whisper]";
                    case 8:
                        return "[Chuchotte MOB]";
                    case 9:
                        return "[A]";
                    case 10:
                        return "[Emote]";
                    case 17:
                        return "[Général]";
                    case 27:
                        return "[Loot]";
                    default:
                        return "";
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("getCanal(int canal): " + exception);
                return "";
            }
        }

        private string stringBetween(string chaine, string debut, string fin)
        {
            try
            {
                int d = chaine.IndexOf(debut, StringComparison.Ordinal) + debut.Length;
                int f = chaine.IndexOf(fin, d, StringComparison.Ordinal);
                if (f - d <= 0)
                    return "";
                return chaine.Substring(d, f - d);
            }
            catch (Exception exception)
            {
                Logging.WriteError("string stringBetween(string chaine, string debut, string fin): " + exception);
                return "";
            }
        }

        #endregion Methods

        #region Nested Types

        public struct Message
        {
            #region Fields

            public
                int Canal;

            public
                string Msg;

            public
                string Pseudo;

            #endregion Fields
        }

        #endregion Nested Types
    }
}