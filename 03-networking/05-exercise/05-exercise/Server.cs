using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace _05_exercise
{
    internal class Server
    {
        private readonly string PATH_WORD = Path.Combine(Environment.GetEnvironmentVariable("appdata"), "words.txt");
        private readonly string PATH_RECORDS = Path.Combine(Environment.GetEnvironmentVariable("appdata"), "records.bin");
        private List<String> words;
        private const int PORT = 31416;
        private bool executeServer = true;
        private Socket serverSocket;
        private Socket clientSocket;
        private enum eCommands
        {
            GETWORD, SENDWORD, GETRECORDS, SENRECORD, CLOSESERVER
        }

        private bool ReadWords()
        {
            try
            {
                string fileContent = File.ReadAllText(PATH_WORD).ToUpper();
                words = fileContent.Split(",").ToList();
            }
            catch (IOException)
            {
                Debug.WriteLine($"Error on {nameof(ReadWords)} reading words");
                return false;
            }
            return true;
        }


        public void Start()
        {
            IPEndPoint ie = new(IPAddress.Any, PORT);

            serverSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                serverSocket.Bind(ie);
            }
            catch (Exception)
            {
                Debug.WriteLine($"Port {ie.Port} in use!");

                if (!TryToBindPort(ie))
                {
                    return;
                }
            }

            serverSocket.Listen(20);

            WaitForUserConnection();
        }

        private bool TryToBindPort(IPEndPoint ie)
        {
            bool isBinded = false;
            for (int i = 0; i < IPEndPoint.MaxPort; i++)
            {
                try
                {
                    ie.Port = i;
                    serverSocket.Bind(ie);
                    isBinded = true;
                    break;
                }
                catch (Exception)
                {
                    Debug.WriteLine($"Port {ie.Port} in use!");
                }
            }

            if (isBinded)
            {
                Debug.WriteLine($"Port {ie.Port} binded correctly!");
            }
            else
            {
                Debug.WriteLine($"Error binded. All ports in use");
            }

            return isBinded;
        }

        private void WaitForUserConnection()
        {
            while (executeServer)
            {
                clientSocket = serverSocket.Accept();
                new Thread(UserManagement).Start(clientSocket);
            }
        }

        private struct User
        {
            public Socket UserSocket;
            public bool IsConnected;

            User(Socket userSocket) : this(userSocket, true)
            {
            }

            User(Socket userSocket, bool isConnected)
            {
                this.UserSocket = userSocket;
                this.IsConnected = isConnected;
            }

        }

        private void UserManagement(Object userSocket)
        {

        }




    }

}
