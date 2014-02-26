using System;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using nManager.Wow.ObjectManager;
using nManager.Wow.Helpers;

namespace nManager.Helpful
{
    public static class Communication
    {
        private static TcpListener _tcpListener;
        private static Thread _listenThread;

        public static void Shutdown(int port = 6543)
        {
            if (_tcpListener != null)
                _tcpListener.Stop();
            if (_listenThread != null && _listenThread.IsAlive)
                _listenThread.Abort();
            Logging.Write("This TheNoobBot session is no longer broadcasting its position and actions on port " + port + " for others TheNoobBot sessions with Mimesis started.");
        }

        public static void Listen()
        {
            int port = nManagerSetting.CurrentSetting.BroadcastingPort;
            _tcpListener = new TcpListener(IPAddress.Any, port);
            _listenThread = new Thread(new ThreadStart(ListenForClients));
            _listenThread.Start();
            Logging.Write("This TheNoobBot session is now broadcasting its position and actions on port " + port + " for others TheNoobBot sessions with Mimesis started.");
        }

        private static void ListenForClients()
        {
            _tcpListener.Start();
            while (true)
            {
                // Wait for a connection
                TcpClient client = _tcpListener.AcceptTcpClient();
                // We got one, create a thread for it
                Socket s = client.Client;
                Logging.Write("Bot with address " + IPAddress.Parse(((IPEndPoint) s.RemoteEndPoint).Address.ToString()) + " has connected.");
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                clientThread.Start(client);
            }
        }

        private static void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient) client;
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
                byte[] bufferPos = MimesisHelpers.ObjectToBytes(ObjectManager.Me.Position);
                byte[] bufferGuid = BitConverter.GetBytes(ObjectManager.Me.Guid);
                byte[] opCode = new byte[1];

                switch ((MimesisHelpers.opCodes) message[0])
                {
                    case MimesisHelpers.opCodes.QueryPosition:
                        opCode[0] = (byte) MimesisHelpers.opCodes.ReplyPosition;
                        clientStream.Write(opCode, 0, 1);
                        clientStream.Write(bufferPos, 0, bufferPos.Length);
                        break;
                    case MimesisHelpers.opCodes.QueryGuid:
                        opCode[0] = (byte) MimesisHelpers.opCodes.ReplyGuid;
                        clientStream.Write(opCode, 0, 1);
                        clientStream.Write(bufferGuid, 0, bufferGuid.Length);
                        break;
                    case MimesisHelpers.opCodes.Disconnect:
                        tcpClient.Close();
                        Logging.Write("Client diconnected");
                        return;
                    case MimesisHelpers.opCodes.QueryEvent:
                        break;
                }
                clientStream.Flush();
            }
            tcpClient.Close();
        }
    }
}