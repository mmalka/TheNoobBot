using System;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using nManager.Wow.ObjectManager;
using nManager.Wow.Helpers;
using nManager.Wow.Enums;

namespace nManager.Helpful
{
    public class Communication
    {
        private static TcpListener _tcpListener;
        private static Thread _listenThread;
        private static uint _eventSerialNumber;
        private static List<int> _currentQuestList;
        private static List<MimesisHelpers.MimesisEvent> _globalList;
        private static Timer _cleanupTimer;
        private static bool _requireHook;

        public static bool RequiresHook
        {
            get { return _requireHook; }
        }

        public static void Shutdown(int port = 6543)
        {
            bool done = false;
            if (_listenThread != null && _listenThread.IsAlive)
            {
                _listenThread.Abort();
                _listenThread = null;
                done = true;
            }
            if (_tcpListener != null && _tcpListener.Server != null)
            {
                _tcpListener.Stop();
                _tcpListener = null;
                done = true;
            }
            if (done)
            {
                Logging.Write("This TheNoobBot session is no longer broadcasting its position and actions on port " + port + " for others TheNoobBot sessions with Mimesis started.");
                // We now unhook these events
                EventsListener.UnHookEvent(WoWEventsType.QUEST_ACCEPTED);
                EventsListener.UnHookEvent(WoWEventsType.QUEST_FINISHED);
                _requireHook = false;
            }
        }

        public static void StartListenOnPort(int port)
        {
            try
            {
                _tcpListener.Start();
                Logging.Write("This TheNoobBot session is now broadcasting its position and actions on port " + port + " for others TheNoobBot sessions with Mimesis started.");
                try
                {
                    _requireHook = true;
                    EventsListener.HookEvent(WoWEventsType.QUEST_ACCEPTED, callback => EventQuestAccepted());
                    //EventsListener.HookEvent(WoWEventsType.QUEST_FINISHED, callback => EventQuestFinished());
                }
                catch
                {
                    Logging.WriteError("event QUEST_ACCEPTED already hooked");
                }
                _eventSerialNumber = 0;
                _currentQuestList = Quest.GetLogQuestId();
                _globalList = new List<MimesisHelpers.MimesisEvent>();
                _cleanupTimer = new Timer(3 * 1000);
            }
            catch (SocketException)
            {
                Random random = new Random();
                int randomPort = random.Next(1024, 65536);
                while (randomPort == port) // Make sure we don't use the same port.
                    random.Next(1024, 65536); // Many of the 1024 firsts ports are reserved.
                Logging.WriteError("Mimesis Broadcaster cannot listen on port " + port + ", another application is already using this port, trying to use " + randomPort + " instead.");
                _tcpListener = new TcpListener(IPAddress.Any, randomPort);
                StartListenOnPort(randomPort);
            }
        }

        public static void Listen()
        {
            Shutdown(); // Make sure we shutdown all previous sessions first. It should be useless if the rest is well coded.
            int port = nManagerSetting.CurrentSetting.BroadcastingPort;
            _tcpListener = new TcpListener(IPAddress.Any, port);
            _listenThread = new Thread(new ThreadStart(() => ListenForClients(port)));
            _listenThread.Start();
        }

        private static void ListenForClients(int port)
        {
            if (_listenThread == null || !_listenThread.IsAlive)
                return;
            StartListenOnPort(port);
            while (_listenThread != null && _listenThread.IsAlive && _tcpListener != null && _tcpListener.Server != null)
            {
                if (_tcpListener.Pending())
                {
                    // Wait for a connection
                    TcpClient client = _tcpListener.AcceptTcpClient();
                    // We got one, create a thread for it
                    Socket s = client.Client;
                    Logging.Write("Bot with address " + IPAddress.Parse(((IPEndPoint) s.RemoteEndPoint).Address.ToString()) + " has connected.");
                    ClientThread cThread = new ClientThread();
                    Thread clientThread = new Thread(new ParameterizedThreadStart(cThread.HandleClientComm));
                    clientThread.Start(client);
                }
                else
                {
                    Thread.Sleep(100);
                    if (_cleanupTimer.IsReady) // Every 3 seconds, we drop the head event from the list
                    {
                        lock (_globalList)
                        {
                            if (_globalList.Count > 0)
                            {
                                MimesisHelpers.MimesisEvent evt = _globalList[0];
                                _globalList.Remove(evt);
                            }
                        }
                        _cleanupTimer.Reset();
                    }
                }
            }
        }

        /*
         * We create a callback method which create a global list of events with a serial number
        */
        public static void EventQuestAccepted()
        {
            WoWObject questGiverO = ObjectManager.GetObjectByGuid(ObjectManager.Me.Target);
            if (questGiverO == null || !questGiverO.IsValid)
                return;
            WoWUnit questGiver = new WoWUnit(questGiverO.GetBaseAddress);
            if (!questGiver.IsValid)
                return;
            _eventSerialNumber++;
            // We create a global event based on the data we will gather
            MimesisHelpers.MimesisEvent evt = new MimesisHelpers.MimesisEvent();
            evt.SerialNumber = _eventSerialNumber;
            evt.eType = MimesisHelpers.eventType.pickupQuest;
            evt.TargetId = questGiver.Entry;
            // we diff current quest list vs old one
            List<int> newQuestList = Quest.GetLogQuestId();
            foreach (var quest in newQuestList)
            {
                if (!_currentQuestList.Contains(quest))
                {
                    evt.QuestId = quest;
                    break;
                }
            }
            // now add this new event to the globale list
            lock(_globalList) _globalList.Add(evt);
            _currentQuestList.Add(evt.QuestId);
            _cleanupTimer.Reset(); // Let 3 seconds for all client threads to pickup this event before purging it
        }

        public static void EventQuestFinished()
        {
            WoWObject questGiverO = ObjectManager.GetObjectByGuid(ObjectManager.Me.Target);
            if (questGiverO == null || !questGiverO.IsValid)
                return;
            WoWUnit questGiver = new WoWUnit(questGiverO.GetBaseAddress);
            if (!questGiver.IsValid)
                return;
            _eventSerialNumber++;
            // We create a global event based on the data we will gather
            MimesisHelpers.MimesisEvent evt = new MimesisHelpers.MimesisEvent();
            evt.SerialNumber = _eventSerialNumber;
            evt.eType = MimesisHelpers.eventType.turninQuest;
            evt.TargetId = questGiver.Entry;
            // we diff current quest list vs old one
            List<int> newQuestList = Quest.GetLogQuestId();
            foreach (var quest in _currentQuestList)
            {
                if (!newQuestList.Contains(quest))
                {
                    evt.QuestId = quest;
                    break;
                }
            }
            // now add this new event to the globale list
            lock (_globalList) _globalList.Add(evt);
            _currentQuestList.Remove(evt.QuestId);
            _cleanupTimer.Reset(); // Let 3 seconds for all client threads to pickup this event before purging it
        }

        private class ClientThread
        {
            public void HandleClientComm(object client)
            {
                if (_listenThread == null || !_listenThread.IsAlive || _tcpListener == null || _tcpListener.Server == null)
                    return;
                TcpClient tcpClient = (TcpClient)client;
                NetworkStream clientStream = tcpClient.GetStream();

                byte[] opCodeAndLen = new byte[2];
                byte[] message = new byte[0];
                int bytesRead;
                List<MimesisHelpers.MimesisEvent> eventList = new List<MimesisHelpers.MimesisEvent>();
                uint _currentSerialNumber = 0;

                while (_listenThread != null && _listenThread.IsAlive && _tcpListener != null && _tcpListener.Server != null)
                {
                    bytesRead = 0;
                    try
                    {
                        // non blocking call
                        if (clientStream.DataAvailable)
                        {
                            bytesRead = clientStream.Read(opCodeAndLen, 0, 2);
                            int len = opCodeAndLen[1];
                            if (len > 0)
                            {
                                message = new byte[len];
                                bytesRead += clientStream.Read(message, 0, len);
                            }
                        }
                    }
                    catch
                    {
                        Logging.Write("Client connection lost!");
                        break;
                    }

                    if (bytesRead > 0)
                    {
                        // Do something with this message
                        switch ((MimesisHelpers.opCodes)opCodeAndLen[0])
                        {
                            case MimesisHelpers.opCodes.QueryPosition:
                                byte[] bufferPos = MimesisHelpers.ObjectToBytes(ObjectManager.Me.Position);
                                opCodeAndLen[0] = (byte)MimesisHelpers.opCodes.ReplyPosition;
                                opCodeAndLen[1] = (byte)bufferPos.Length;
                                clientStream.Write(opCodeAndLen, 0, 2);
                                clientStream.Write(bufferPos, 0, bufferPos.Length);
                                break;
                            case MimesisHelpers.opCodes.QueryGuid:
                                byte[] bufferGuid = BitConverter.GetBytes(ObjectManager.Me.Guid);
                                opCodeAndLen[0] = (byte)MimesisHelpers.opCodes.ReplyGuid;
                                opCodeAndLen[1] = (byte)bufferGuid.Length;
                                clientStream.Write(opCodeAndLen, 0, 2);
                                clientStream.Write(bufferGuid, 0, bufferGuid.Length);
                                break;
                            case MimesisHelpers.opCodes.Disconnect:
                                tcpClient.Close();
                                Logging.Write("Client diconnected");
                                return;
                            case MimesisHelpers.opCodes.QueryEvent:
                                opCodeAndLen[0] = (byte)MimesisHelpers.opCodes.ReplyEvent;
                                if (eventList.Count > 0)
                                {
                                    MimesisHelpers.MimesisEvent mevent = eventList[0];
                                    byte[] bufferEvent = MimesisHelpers.StructToBytes(mevent);
                                    opCodeAndLen[1] = (byte)bufferEvent.Length;
                                    clientStream.Write(opCodeAndLen, 0, 2);
                                    clientStream.Write(bufferEvent, 0, bufferEvent.Length);
                                    eventList.Remove(mevent);
                                }
                                else
                                {
                                    MimesisHelpers.MimesisEvent emptyEv = new MimesisHelpers.MimesisEvent();
                                    emptyEv.eType = MimesisHelpers.eventType.none;
                                    byte[] bufferEvent = MimesisHelpers.StructToBytes(emptyEv);
                                    opCodeAndLen[1] = (byte)bufferEvent.Length;
                                    clientStream.Write(opCodeAndLen, 0, 2);
                                    clientStream.Write(bufferEvent, 0, bufferEvent.Length);
                                }
                                break;
                            case MimesisHelpers.opCodes.RequestGrouping:
                                string sentName = MimesisHelpers.BytesToString(message);
                                Lua.LuaDoString("InviteUnit(\"" + sentName + "\")");
                                opCodeAndLen[0] = (byte)MimesisHelpers.opCodes.ReplyGrouping;
                                opCodeAndLen[1] = 0;
                                clientStream.Write(opCodeAndLen, 0, 2);
                                break;
                            /* Some more things to replicate
                             * - TAXIMAP_OPENED
                            */
                        }
                        clientStream.Flush();
                        Thread.Sleep(100);
                        // We should code here event collecting (eg.: pickup quest, turnin quest, interact with object...)
                        /*
                         * We loop throu the global list which is out of this thread, if an event has an higher serial number
                         * than the last we know localy, we copy the event to the local list in this thread.
                        */
                        uint highestSerialNumber = 0;
                        lock (_globalList)
                        {
                            foreach (MimesisHelpers.MimesisEvent evt in _globalList)
                            {
                                if (evt.SerialNumber > _currentSerialNumber)
                                {
                                    eventList.Add(evt);
                                    if (evt.SerialNumber > highestSerialNumber) // Useless most probably because the list should be sorted
                                        highestSerialNumber = evt.SerialNumber;
                                }
                            }
                        }
                        _currentSerialNumber = highestSerialNumber;
                    }
                }
                clientStream.Dispose();
                tcpClient.Close();
            }
        }
    }
}