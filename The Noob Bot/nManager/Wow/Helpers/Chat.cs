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

        public int CurrentMsg;

        public int GetCurrentMsgInWow
        {
            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + (uint) Addresses.Chat.chatBufferPos);
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
                GetCurrentMsg();
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
                Message aMsg = ReadMsg();
                if (aMsg.Msg != null)
                {
                    return DateTime.Now + " - " + aMsg.Nickname + " " + getChannel(aMsg.Channel) + " : " + aMsg.Msg +
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
                Message aMsg = ReadMsg();
                if (aMsg.Msg != null)
                {
                    if (aMsg.Channel == 4)
                    {
                        return DateTime.Now + " - " + aMsg.Nickname + " " + getChannel(aMsg.Channel) + " : " + aMsg.Msg +
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
                Message aMsg = ReadMsg();
                if (aMsg.Msg != null)
                {
                    if (aMsg.Channel == 1)
                    {
                        return DateTime.Now + " - " + aMsg.Nickname + " " + getChannel(aMsg.Channel) + " : " + aMsg.Msg +
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
                Message aMsg = ReadMsg();
                if (aMsg.Msg != null)
                {
                    if (aMsg.Channel == 7)
                    {
                        return DateTime.Now + " - " + aMsg.Nickname + " " + getChannel(aMsg.Channel) + " : " + aMsg.Msg +
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

        public string ReadWhisperAndRealIdChannel()
        {
            try
            {
                Message aMsg = ReadMsg();
                if (aMsg.Msg != null)
                {
                    if (aMsg.Channel == 7 || aMsg.Channel == 51)
                    {
                        return DateTime.Now + " - " + aMsg.Nickname + " " + getChannel(aMsg.Channel) + " : " + aMsg.Msg +
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

        public string ReadLootChannel()
        {
            try
            {
                Message aMsg = ReadMsg();
                if (aMsg.Msg != null)
                {
                    if (aMsg.Channel == 27)
                    {
                        return DateTime.Now + " - " + aMsg.Nickname + " " + getChannel(aMsg.Channel) + " : " + aMsg.Msg +
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

        private void GetCurrentMsg()
        {
            try
            {
                CurrentMsg =
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
                if (CurrentMsg > 59)
                    CurrentMsg = 0;

                if (CurrentMsg >=
                    Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule +
                                                    (uint) Addresses.Chat.chatBufferPos) &&
                    (CurrentMsg -
                     Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule +
                                                     (uint) Addresses.Chat.chatBufferPos)) <= 2)
                    return new Message();

                string stream =
                    Memory.WowMemory.Memory.ReadUTF8String(
                        (uint)
                            (Memory.WowProcess.WowModule + (uint) Addresses.Chat.chatBufferStart +
                             (uint) Addresses.Chat.msgFormatedChat + (int) Addresses.Chat.NextMessage*(CurrentMsg)));

                var aMsg = new Message();
                if (stream != "")
                {
                    CurrentMsg++;
                    aMsg.Channel = Others.ToInt32(stringBetween(stream, "Type: [", "]"));
                    aMsg.Nickname = Others.ToUtf8(stringBetween(stream, "Name: [", "]"));
                    aMsg.Msg = Others.ToUtf8(stringBetween(stream, "Text: [", "]"));
                }

                return aMsg;
            }
            catch (Exception exception)
            {
                Logging.WriteError("Chat > ReadMsg(): " + exception);
                return new Message();
            }
        }

        private string getChannel(int channel)
        {
            try
            {
                switch (channel)
                {
                    case 0:
                        return "[Addon]";
                    case 1:
                        return "[Say]";
                    case 2:
                        return "[Party]";
                    case 3:
                        return "[Raid]";
                    case 4:
                        return "[Guild]";
                    case 5:
                        return "[Officers]";
                    case 6:
                        return "[Yell]";
                    case 7:
                        return "[Whisper]";
                    case 8:
                        return "[Monster Whisper]";
                    case 9:
                        return "[To]";
                    case 10:
                        return "[Emote]";
                    case 17:
                        return "[General]";
                    case 27:
                        return "[Loot]";
                    case 51:
                        return "[Real Id Whisper]";
                    case 52:
                        return "[Real Id To]";
                    default:
                        return "[Channel " + channel + "]";
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("getChannel(int Channel): " + exception);
                return "";
            }
        }

        private string stringBetween(string str, string begin, string end)
        {
            try
            {
                int d = str.IndexOf(begin, StringComparison.Ordinal) + begin.Length;
                int f = str.IndexOf(end, d, StringComparison.Ordinal);
                if (f - d <= 0)
                    return "";
                return str.Substring(d, f - d);
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
                int Channel;

            public
                string Msg;

            public
                string Nickname;

            #endregion Fields
        }

        #endregion Nested Types
    }
}