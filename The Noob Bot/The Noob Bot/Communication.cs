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
            while (true)
            {
                // Wait for a connection
                TcpClient client = tcpListener.AcceptTcpClient();
                // We got one, create a thread for it
                Logging.Write("Another Bot has connected to this one.");
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
                    break;
                }

                if (bytesRead == 0)
                {
                    // client gone, end the thread
                    break;
                }

                // Do something with that message
                byte[] bufferPos = MimicryHelpers.StructToBytes(ObjectManager.Me.Position);

                switch ((MimicryHelpers.opCodes)message[0])
                {
                    case MimicryHelpers.opCodes.QueryPosition:
                        clientStream.Write(bufferPos, 0, bufferPos.Length);
                        break;
                }
                clientStream.Flush();
            }
            tcpClient.Close();
        }

    }
}
