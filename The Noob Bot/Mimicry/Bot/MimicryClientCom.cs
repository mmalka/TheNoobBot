using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using nManager.Wow.Class;
using nManager.Wow.Helpers;

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
                serviceEndPoint = new IPEndPoint(IPAddress.Parse("192.168.10.1"), 6543);

            client.Connect(serviceEndPoint);
        }

        public static Point GetMasterPosition()
        {
            byte[] buffer = new byte[4096];
            buffer[0] = (byte)MimicryHelpers.opCodes.QueryPosition;

            NetworkStream clientStream = client.GetStream();
            clientStream.Write(buffer, 0, 1);
            clientStream.Flush();

            // Now wait for an answer
            try
            {
                int bytesRead = clientStream.Read(buffer, 0, 4096);
            }
            catch
            {
                return new Point();
            }
            return MimicryHelpers.BytesToStruct<Point>(buffer);
        }
    }
}
