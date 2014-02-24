using System;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Text;
using nManager.Wow.ObjectManager;
using nManager.Helpful;
using nManager.Wow.Helpers;

namespace The_Noob_Bot
{
    public static class Communication
    {
        private static TcpListener tcpListener;
        private static Thread listenThread;

        public static void Listen()
        {
            tcpListener = new TcpListener(IPAddress.Any, 6543);
            listenThread = new Thread(new ThreadStart(ListenForClients));
            listenThread.Start();
        }

        private static void ListenForClients()
        {
            tcpListener.Start();
            while (true)
            {
                // Wait for a connection
                TcpClient client = tcpListener.AcceptTcpClient();
                // We got one, create a thread for it
                Socket s = client.Client;
                Logging.Write("Bot with address " + IPAddress.Parse(((IPEndPoint)s.RemoteEndPoint).Address.ToString()) + " has connected.");
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                clientThread.Start(client);
            }
        }

        private static void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();

            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;
                try
                {
                    // blocking call
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    Logging.Write("Client connection lost!");
                    break;
                }

                if (bytesRead == 0)
                {
                    // client gone, end the thread
                    break;
                }

                // Do something with that message
                byte[] bufferPos = MimicryHelpers.ObjectToBytes(ObjectManager.Me.Position);
                byte[] bufferGuid = BitConverter.GetBytes(ObjectManager.Me.Guid);
                byte[] opCode = new byte[1];

                switch ((MimicryHelpers.opCodes)message[0])
                {
                    case MimicryHelpers.opCodes.QueryPosition:
                        opCode[0] = (byte)MimicryHelpers.opCodes.ReplyPosition;
                        clientStream.Write(opCode, 0, 1);
                        clientStream.Write(bufferPos, 0, bufferPos.Length);
                        break;
                    case MimicryHelpers.opCodes.QueryGuid:
                        opCode[0] = (byte)MimicryHelpers.opCodes.ReplyGuid;
                        clientStream.Write(opCode, 0, 1);
                        clientStream.Write(bufferGuid, 0, bufferGuid.Length);
                        break;
                    case MimicryHelpers.opCodes.Disconnect:
                        tcpClient.Close();
                        return;
                    case MimicryHelpers.opCodes.QueryEvent:
                        break;
                }
                clientStream.Flush();
            }
            tcpClient.Close();
        }

    }
}
