using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Helpful;
using nManager.Wow.ObjectManager;
using nManager.Wow.Enums;
using nManager.Wow.Bot.Tasks;

namespace Mimesis.Bot
{
    internal class MimesisClientCom
    {
        private static TcpClient client = null;
        private static IPEndPoint serviceEndPoint = null;
        public static List<MimesisHelpers.MimesisEvent> myTaskList = new List<MimesisHelpers.MimesisEvent>();
        public static List<int> myQuestList = Quest.GetLogQuestId();
        private static uint RollId = 0;

        public static bool Connect()
        {
            Logging.Write("Connecting to " + MimesisSettings.CurrentSetting.MasterIPAddress + ":" + MimesisSettings.CurrentSetting.MasterIPPort + " ...");
            client = new TcpClient();

            if (serviceEndPoint == null)
                serviceEndPoint = new IPEndPoint(IPAddress.Parse(MimesisSettings.CurrentSetting.MasterIPAddress), MimesisSettings.CurrentSetting.MasterIPPort);
            try
            {
                client.Connect(serviceEndPoint);
                Logging.Write("Connected!");
                EventsListener.HookEvent(WoWEventsType.QUEST_ACCEPTED, callback => EventQuestAccepted());
                EventsListener.HookEvent(WoWEventsType.QUEST_FINISHED, callback => EventQuestFinished());
                return true;
            }
            catch
            {
                Logging.Write("Could not connect to " + MimesisSettings.CurrentSetting.MasterIPAddress + ":" + MimesisSettings.CurrentSetting.MasterIPPort);
                return false;
            }
        }

        public static void Disconnect()
        {
            if (client == null)
                return;
            if (client.Connected)
            {
                try
                {
                    NetworkStream clientStream = client.GetStream();
                    byte[] opCodeAndSize = new byte[2];
                    opCodeAndSize[0] = (byte) MimesisHelpers.opCodes.Disconnect;
                    opCodeAndSize[1] = 0;
                    clientStream.Write(opCodeAndSize, 0, 2);
                    clientStream.Flush();
                }
                catch (System.IO.IOException)
                {
                }
            }
            Logging.Write("Disconnected from main bot.");
            EventsListener.UnHookEvent(WoWEventsType.QUEST_ACCEPTED);
            EventsListener.UnHookEvent(WoWEventsType.QUEST_FINISHED);
            client.Close();
        }

        public static ulong GetMasterGuid()
        {
            byte[] opCodeAndSize = new byte[2];
            byte[] buffer;
            opCodeAndSize[0] = (byte) MimesisHelpers.opCodes.QueryGuid;
            opCodeAndSize[1] = 0;

            NetworkStream clientStream = client.GetStream();
            clientStream.Write(opCodeAndSize, 0, 2);
            clientStream.Flush();

            // Now wait for an answer
            try
            {
                int bytesRead = clientStream.Read(opCodeAndSize, 0, 2);
                int len = opCodeAndSize[1];
                buffer = new byte[len];
                bytesRead += clientStream.Read(buffer, 0, len);
            }
            catch (Exception e)
            {
                Logging.WriteError("MimesisClientCom > GetMasterGuid(): " + e);
                return 0;
            }
            if ((MimesisHelpers.opCodes) opCodeAndSize[0] == MimesisHelpers.opCodes.ReplyGuid)
                return (ulong) BitConverter.ToInt64(buffer, 0);
            return 0;
        }

        public static Point GetMasterPosition()
        {
            byte[] opCodeAndSize = new byte[2];
            byte[] buffer;
            opCodeAndSize[0] = (byte) MimesisHelpers.opCodes.QueryPosition;
            opCodeAndSize[1] = 0;

            NetworkStream clientStream = client.GetStream();
            clientStream.Write(opCodeAndSize, 0, 2);
            clientStream.Flush();

            // Now wait for an answer
            try
            {
                int bytesRead = clientStream.Read(opCodeAndSize, 0, 2);
                int len = opCodeAndSize[1];
                buffer = new byte[len];
                bytesRead += clientStream.Read(buffer, 0, len);
            }
            catch (Exception e)
            {
                Logging.WriteError("MimesisClientCom > GetMasterPosition(): " + e);
                return new Point();
            }
            if ((MimesisHelpers.opCodes) opCodeAndSize[0] == MimesisHelpers.opCodes.ReplyPosition)
                return MimesisHelpers.BytesToObject<Point>(buffer);
            return new Point();
        }

        public static void JoinGroup()
        {
            byte[] opCodeAndSize = new byte[2];
            byte[] buffer;
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString(randomString + " = GetRealmName()");
            byte[] bufferName = MimesisHelpers.StringToBytes(ObjectManager.Me.Name + "-" + Lua.GetLocalizedText(randomString));
            opCodeAndSize[0] = (byte) MimesisHelpers.opCodes.RequestGrouping;
            opCodeAndSize[1] = (byte) bufferName.Length;
            NetworkStream clientStream = client.GetStream();
            clientStream.Write(opCodeAndSize, 0, 2);
            clientStream.Write(bufferName, 0, bufferName.Length); // It's hardcoded "PlayerName-RealmName"
            clientStream.Flush();
            // Now wait for an answer
            try
            {
                int bytesRead = clientStream.Read(opCodeAndSize, 0, 2);
                int len = opCodeAndSize[1]; // It's 4 (one uint)
                buffer = new byte[len];
                bytesRead += clientStream.Read(buffer, 0, len);
                RollId = BitConverter.ToUInt32(buffer, 0);
            }
            catch (Exception e)
            {
                Logging.WriteError("MimesisClientCom > JoinGroup(): " + e);
                return;
            }
            EventsListener.HookEvent(WoWEventsType.GROUP_ROSTER_UPDATE, callback => CloseGroupPopup());
            System.Threading.Thread.Sleep(250 + 2*Usefuls.Latency);
            Lua.LuaDoString("AcceptGroup()");
        }

        public static void CloseGroupPopup()
        {
            Lua.LuaDoString("StaticPopup_Hide(\"PARTY_INVITE\")");
            EventsListener.UnHookEvent(WoWEventsType.GROUP_ROSTER_UPDATE);
            EventsListener.HookEvent(WoWEventsType.START_LOOT_ROLL, callback => RollItem());
        }

        public static void ProcessEvents()
        {
            byte[] opCodeAndSize = new byte[2];
            byte[] buffer = new byte[1];
            opCodeAndSize[0] = (byte) MimesisHelpers.opCodes.QueryEvent;
            opCodeAndSize[1] = 0;

            NetworkStream clientStream = client.GetStream();
            clientStream.Write(opCodeAndSize, 0, 2);
            clientStream.Flush();

            // Now wait for an answer
            try
            {
                int bytesRead = clientStream.Read(opCodeAndSize, 0, 2);
                int len = opCodeAndSize[1];
                if (len > 0)
                {
                    buffer = new byte[len];
                    bytesRead += clientStream.Read(buffer, 0, len);
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("MimesisClientCom > ProcessEvents(): " + e);
                return;
            }
            if ((MimesisHelpers.opCodes) opCodeAndSize[0] == MimesisHelpers.opCodes.ReplyEvent && opCodeAndSize[1] > 0)
            {
                MimesisHelpers.MimesisEvent evt = MimesisHelpers.BytesToStruct<MimesisHelpers.MimesisEvent>(buffer);
                switch (evt.eType)
                {
                    case MimesisHelpers.eventType.pickupQuest:
                        Logging.WriteDebug("Received pickupquest " + evt.EventValue2);
                        myTaskList.Add(evt);
                        break;
                    case MimesisHelpers.eventType.turninQuest:
                        Logging.WriteDebug("Received turninquest " + evt.EventValue2);
                        myTaskList.Add(evt);
                        break;
                    case MimesisHelpers.eventType.mount:
                        Logging.WriteDebug("Received mount type " + (MountCapacity) evt.EventValue1);
                        myTaskList.Add(evt);
                        break;
                }
            }
        }

        public static void DoTasks()
        {
            if (!HasTaskToDo())
                return;
            MimesisHelpers.MimesisEvent evt = myTaskList[0];
            switch (evt.eType)
            {
                case MimesisHelpers.eventType.pickupQuest:
                case MimesisHelpers.eventType.turninQuest:
                    List<WoWUnit> listU = ObjectManager.GetWoWUnitByEntry(evt.EventValue1);
            if (listU.Count > 0)
            {
                WoWUnit u = listU[0];
                Npc quester = new Npc();
                        quester.Entry = evt.EventValue1;
                quester.Position = u.Position;
                quester.Name = u.Name;

                        if (evt.eType == MimesisHelpers.eventType.pickupQuest)
                            Quest.QuestPickUp(ref quester, evt.EventString1, evt.EventValue2);
                        else
                            Quest.QuestTurnIn(ref quester, Quest.GetLogQuestTitle(evt.EventValue2) , evt.EventValue2);
                    }
                    break;
                case MimesisHelpers.eventType.mount:
                    switch ((MountCapacity)evt.EventValue1)
                    {
                        case MountCapacity.Ground:
                            MountTask.MountingGroundMount(true);
                            break;
                        case MountCapacity.Fly:
                            MountTask.MountingFlyingMount(true);
                            break;
                        case MountCapacity.Swimm:
                            MountTask.MountingAquaticMount(true);
                            break;
                        default:
                            MountTask.DismountMount(true);
                            break;
                    }
                    myTaskList.Remove(evt);
                    break;
            }
        }

        public static void EventQuestAccepted()
        {
            List<int> newQuestList = Quest.GetLogQuestId();
            int questId = 0;
            foreach (var quest in newQuestList)
            {
                if (!myQuestList.Contains(quest))
                {
                    questId = quest;
                    break;
                }
            }
            if (questId != 0)
            {
                MimesisHelpers.MimesisEvent evtToRemove = new MimesisHelpers.MimesisEvent();
                foreach (MimesisHelpers.MimesisEvent evt in myTaskList)
                {
                    if (evt.eType == MimesisHelpers.eventType.pickupQuest && evt.EventValue2 == questId)
                    {
                        evtToRemove = evt;
                        break;
                    }
                }
                myTaskList.Remove(evtToRemove);
                myQuestList.Add(questId);
            }
        }

        public static void EventQuestFinished()
        {
            List<int> newQuestList = Quest.GetLogQuestId();
            int questId = 0;
            foreach (var quest in myQuestList)
            {
                if (!newQuestList.Contains(quest))
                {
                    questId = quest;
                    break;
                }
            }
            if (questId != 0)
            {
                MimesisHelpers.MimesisEvent evtToRemove = new MimesisHelpers.MimesisEvent();
                foreach (MimesisHelpers.MimesisEvent evt in myTaskList)
                {
                    if (evt.eType == MimesisHelpers.eventType.turninQuest && evt.EventValue2 == questId)
                    {
                        evtToRemove = evt;
                        break;
                    }
                }
                myTaskList.Remove(evtToRemove);
                myQuestList.Remove(questId);
            }
        }

        // I will need to expand this for new Tasks
        public static bool HasTaskToDo()
        {
            return myTaskList.Count > 0;
        }

        // We need the roll Id, but since the events have no data, how ?
        public static void RollItem()
        {
            RollId++;
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            Lua.LuaDoString("_, " + randomString + " = GetLootRollItemLink(" + RollId + ")");
            string itemLink = Lua.GetLocalizedText(randomString);

            // Then we need to use new code located in nManager/Wow/Helpers/ItemSelection.cs

            Lua.LuaDoString("RollOnLoot(" + RollId + ", 2)"); // Roll "Greed"
            Logging.Write("Doing Loot Roll Greed on RollId=" + RollId);
            // 0 - Pass (declines the loot)
            // 1 - Roll "need" (wins if highest roll)
            // 2 - Roll "greed" (wins if highest roll and no other member rolls "need")
            // 3 - Disenchant

            // register event CONFIRM_LOOT_ROLL
            EventsListener.HookEvent(WoWEventsType.CONFIRM_LOOT_ROLL, callback => ConfirmLootRoll(RollId));
        }

        public static void ConfirmLootRoll(uint id)
        {
            Logging.Write("Confirm Roll on RollId=" + id);
            Lua.LuaDoString("ConfirmLootRoll(" + id + ")");
            EventsListener.UnHookEvent(WoWEventsType.CONFIRM_LOOT_ROLL);
        }
    }
}
