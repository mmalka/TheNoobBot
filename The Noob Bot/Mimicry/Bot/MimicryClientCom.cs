using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using nManager.Wow.Class;
using nManager.Wow.Helpers;
using nManager.Helpful;

namespace MimicryBot.Bot
{
    class MimicryClientCom
    {
        private static TcpClient client = null;
        private static IPEndPoint serviceEndPoint = null;

        public static void Connect()
        {
            if (client == null)
                client = new TcpClient();
            if (serviceEndPoint == null)
                serviceEndPoint = new IPEndPoint(IPAddress.Parse("192.168.10.116"), 6543);
            Logging.Write("Connected to bot @192.168.10.116");
            client.Connect(serviceEndPoint);
        }

        public static void Disconnect()
        {
            if (client == null)
                return;
            byte[] opCode = new byte[1];
            NetworkStream clientStream = client.GetStream();
            opCode[0] = (byte)MimicryHelpers.opCodes.Disconnect;
            clientStream.Write(opCode, 0, 1);
            clientStream.Flush();

            Logging.Write("Disconnected from main bot.");
            client.Close();
        }

        public static ulong GetMasterGuid()
        {
            byte[] opCode = new byte[1];
            byte[] buffer = new byte[4096];
            opCode[0] = (byte)MimicryHelpers.opCodes.QueryGuid;

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
            if ((MimicryHelpers.opCodes)opCode[0] == MimicryHelpers.opCodes.ReplyGuid)
                return (ulong)BitConverter.ToInt64(buffer, 0);
            return 0;
        }

        public static Point GetMasterPosition()
        {
            byte[] opCode = new byte[1];
            byte[] buffer = new byte[4096];
            opCode[0] = (byte)MimicryHelpers.opCodes.QueryPosition;

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
            if ((MimicryHelpers.opCodes)opCode[0] == MimicryHelpers.opCodes.ReplyPosition)
                return MimicryHelpers.BytesToObject<Point>(buffer);
            return new Point();
        }
    }
}
