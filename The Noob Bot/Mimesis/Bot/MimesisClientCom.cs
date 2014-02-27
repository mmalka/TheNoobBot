using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Helpful;
using nManager.Wow.ObjectManager;

namespace Mimesis.Bot
{
    class MimesisClientCom
    {
        private static TcpClient client = null;
        private static IPEndPoint serviceEndPoint = null;

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
            byte[] opCode = new byte[1];
            NetworkStream clientStream = client.GetStream();
            opCode[0] = (byte)MimesisHelpers.opCodes.Disconnect;
            clientStream.Write(opCode, 0, 1);
            clientStream.Flush();

            Logging.Write("Disconnected from main bot.");
            client.Close();
        }

        public static ulong GetMasterGuid()
        {
            byte[] opCode = new byte[1];
            byte[] buffer = new byte[4096];
            opCode[0] = (byte)MimesisHelpers.opCodes.QueryGuid;

            NetworkStream clientStream = client.GetStream();
            clientStream.Write(opCode, 0, 1);
            clientStream.Flush();

            // Now wait for an answer
            try
            {
                int bytesRead = clientStream.Read(opCode, 0, 1);
                bytesRead += clientStream.Read(buffer, 0, 4096);
            }
            catch
            {
                return 0;
            }
            if ((MimesisHelpers.opCodes)opCode[0] == MimesisHelpers.opCodes.ReplyGuid)
                return (ulong)BitConverter.ToInt64(buffer, 0);
            return 0;
        }

        public static Point GetMasterPosition()
        {
            byte[] opCode = new byte[1];
            byte[] buffer = new byte[4096];
            opCode[0] = (byte)MimesisHelpers.opCodes.QueryPosition;

            NetworkStream clientStream = client.GetStream();
            clientStream.Write(opCode, 0, 1);
            clientStream.Flush();

            // Now wait for an answer
            try
            {
                int bytesRead = clientStream.Read(opCode, 0, 1);
                bytesRead += clientStream.Read(buffer, 0, 4096);
            }
            catch
            {
                return new Point();
            }
            if ((MimesisHelpers.opCodes)opCode[0] == MimesisHelpers.opCodes.ReplyPosition)
                return MimesisHelpers.BytesToObject<Point>(buffer);
            return new Point();
        }

        public static void ProcessEvents()
        {
            byte[] opCode = new byte[1];
            byte[] buffer = new byte[4096];
            opCode[0] = (byte)MimesisHelpers.opCodes.QueryEvent;

            NetworkStream clientStream = client.GetStream();
            clientStream.Write(opCode, 0, 1);
            clientStream.Flush();

            // Now wait for an answer
            try
            {
                int bytesRead = clientStream.Read(opCode, 0, 1);
                bytesRead += clientStream.Read(buffer, 0, 4096);
            }
            catch
            {
                return;
            }
            if ((MimesisHelpers.opCodes)opCode[0] == MimesisHelpers.opCodes.ReplyEvent)
            {
                MimesisHelpers.MimesisEvent evt = MimesisHelpers.BytesToStruct<MimesisHelpers.MimesisEvent>(buffer);
                switch (evt.eType)
                {
                    case MimesisHelpers.eventType.pickupQuest:
                        List<WoWUnit> listU = ObjectManager.GetWoWUnitByEntry(evt.TargetId);
                        if (listU.Count > 0)
                        {
                            WoWUnit u = listU[0];
                            Npc quester = new Npc();
                            quester.Entry = evt.TargetId;
                            quester.Position = u.Position;
                            quester.Name = u.Name;

                            MovementManager.StopMove();
                            Quest.QuestPickUp(ref quester, "not implemented", evt.QuestId);
                        }
                        break;
                }
            }
        }
    }
}
