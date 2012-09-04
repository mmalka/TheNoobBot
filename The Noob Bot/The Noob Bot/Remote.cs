using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using nManager.Helpful;
using nManager.Wow;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;

namespace The_Noob_Bot
{
    internal class Remote
    {
        #region FlagRequest enum

        private enum FlagRequest
        {
            None = 0,
            CloseWow = 1,
            CloseBot = 2,
            ShutDownPc = 3,
            SendWhisper = 4,
        }

        #endregion

        internal static bool RemoteActive;
        private readonly Thread _work;
        private bool _firstActive;

        private const string RemoteScript = "http://tech.thenoobbot.com/remote.php";
        private int _sessionKey;

        internal Remote()
        {
            try
            {
                if (_work == null)
                {
                    _work = new Thread(ThreadBotRemote) {Name = "remote"};
                    _work.Start();
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Remote > Remote(): " + e);
            }
        }

        private void ThreadBotRemote()
        {
            try
            {
                _sessionKey = Others.Random(0, 999999);
                while (true)
                {
                    try
                    {
                        while (!RemoteActive)
                        {
                            Thread.Sleep(1000*3);
                            _firstActive = false;
                        }
                        if (!_firstActive)
                        {
                            Logging.Write("Remote activated. Session id: " + _sessionKey);
                            _firstActive = true;
                        }

                        SendGetToServer();
                    }
                    catch (Exception e)
                    {
                        Logging.WriteError("Remote > ThreadBotRemote()#1: " + e);
                    }
                    Thread.Sleep(1000*5);
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Remote > ThreadBotRemote()#2: " + e);
            }
            // ReSharper disable FunctionNeverReturns
        }

        // ReSharper restore FunctionNeverReturns
        private Channel channel;
        private List<string> channelWhisper = new List<string>();

        private void SendGetToServer()
        {
            try
            {
                try
                {
                    if (channel == null)
                        channel = new Channel();

                    for (int i = 0; i < 20; i++)
                    {
                        var v = channel.ReadWhisperChannel();
                        if (v != "")
                        {
                            channelWhisper.Add(v);
                            if (channelWhisper.Count > 3)
                                channelWhisper.RemoveAt(0);
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Logging.WriteError("Remote > SendGetToServer()#1: " + e);
                }

                var whisper = channelWhisper.Aggregate("",
                                                       (current, cw) =>
                                                       cw.Replace("~", "-").Replace("|", "-") + "~" + current);
                whisper = whisper.Replace("[Whisper]", "");

                Logging.LogType flag = Logging.LogType.Normal;
                flag |= Logging.LogType.Debug;
                flag |= Logging.LogType.Error;
                flag |= Logging.LogType.Fight;
                flag |= Logging.LogType.Navigator;

                var packetClient = new PacketClient
                                       {
                                           Name = ObjectManager.Me.Name,
                                           Level = (int) ObjectManager.Me.Level,
                                           Health = (int) ObjectManager.Me.HealthPercent,
                                           X = ObjectManager.Me.Position.X,
                                           Y = ObjectManager.Me.Position.Y,
                                           Z = ObjectManager.Me.Position.Z,
                                           LastLog = Logging.ReadLastString(flag),
                                           TargetName = ObjectManager.Target.Name,
                                           TargetLevel = (int) ObjectManager.Target.Level,
                                           TargetHealth = (int) ObjectManager.Target.HealthPercent,
                                           InGame = Usefuls.InGame,
                                           SubMapName = Usefuls.SubMapZoneName,
                                           ClassPlayer = ObjectManager.Me.WowClass.ToString(),
                                           BagSpace = Usefuls.GetContainerNumFreeSlots,
                                           LastWhisper = whisper,
                                       };


                string req = packetClient.Name + "|" + packetClient.Level + "|" + packetClient.Health + "|" +
                             packetClient.X + "|" +
                             packetClient.Y + "|" + packetClient.Z + "|" + packetClient.LastLog + "|" +
                             packetClient.TargetName + "|" +
                             packetClient.TargetLevel + "|" + packetClient.TargetHealth + "|" + packetClient.InGame +
                             "|" + packetClient.SubMapName + "|" + packetClient.ClassPlayer + "| |" +
                             packetClient.BagSpace + "|" + packetClient.LastWhisper;


                var result =
                    Others.GetReqWithAuthHeader(RemoteScript + "?sessionId=" + _sessionKey + "&forServer=" + req,
                                                LoginServer.Login, LoginServer.Password);
                if (result[0] == null)
                    result[0] = "";

                string whisperContant = "";
                string whisperFor = "";
                if (result[0].Contains("|"))
                {
                    try
                    {
                        string[] t = result[0].Split(Convert.ToChar("|"));
                        if (t.Length >= 3)
                        {
                            result[0] = t[0];
                            whisperFor = t[1];
                            whisperContant = t[2];
                        }
                    }
                    catch (Exception e)
                    {
                        Logging.WriteError("Remote > SendGetToServer()#2: " + e);
                    }
                }
                if (result.Count > 0)
                {
                    if (result[0] == "")
                        result[0] = "0";
                    switch ((FlagRequest) Convert.ToInt32(result[0]))
                    {
                        case FlagRequest.CloseBot:
                            Logging.WriteDebug("Remote: Close bot.");
                            try
                            {
                                nManager.Pulsator.Dispose(true);
                            }
                            catch
                            {
                            }
                            Process.GetCurrentProcess().Kill();
                            break;
                        case FlagRequest.CloseWow:
                            Logging.WriteDebug("Remote: Close game.");
                            Memory.WowProcess.KillWowProcess();
                            break;
                        case FlagRequest.ShutDownPc:
                            Logging.WriteDebug("Remote: Shutdown Computer.");
                            Others.ShutDownPc();
                            break;
                        case FlagRequest.SendWhisper:
                            if (whisperFor != "" && whisperContant != "")
                            {
                                Chat.SendChatMessageWhisper(whisperContant, whisperFor);
                                channelWhisper.Add("By me To " + whisperFor + ": " + whisperContant);
                                if (channelWhisper.Count > 3)
                                    channelWhisper.RemoveAt(0);
                            }
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Remote > SendGetToServer()#3: " + e);
            }
        }

        #region Nested type: PacketClient

        private struct PacketClient
        {
            public int BagSpace; // 14
            public float X; // 3
            public float Y; // 4
            public float Z; // 5
            public string ClassPlayer; // 12
            public int Health; // 2
            public bool InGame; // 10
            public string LastLog; // 6
            public int Level; // 1
            public string Name; // 0
            public string SubMapName; // 11
            public int TargetHealth; // 9
            public int TargetLevel; // 8
            public string TargetName; // 7
            public string LastWhisper; // 15
        }

        #endregion
    }
}