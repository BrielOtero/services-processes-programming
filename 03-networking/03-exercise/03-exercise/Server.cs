using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace _03_exercise
{
    internal class Server
    {
        private const int PORT = 31416;
        private IPEndPoint ie;
        private Socket serverSocket;
        private Socket userSocket;
        private Dictionary<String, Socket> users = new();
        private const string EXIT = "#EXIT";
        private const string LIST = "#LIST";



        public void StartServer()
        {
            ie = new(IPAddress.Any, PORT);

            using (serverSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                try
                {
                    serverSocket.Bind(ie);
                }
                catch (SocketException se)
                {
                    Debug.WriteLine($"Port {ie.Port} in use!");

                    if (!ProcessPortError(ie, serverSocket))
                    {
                        Console.WriteLine("Can't bind with any port");
                        return;
                    }

                }
                serverSocket.Listen(18);
                WaitForConnection();

            }
        }

        private void WaitForConnection()
        {
            while (true)
            {
                userSocket = serverSocket?.Accept();


                new Thread(UserManagment).Start(userSocket);

            }
        }
        static readonly private object l = new object();

        private void UserManagment(object userSocket)
        {
            bool isCorrectUsername = false;
            bool isUserConnected = true;
            string username = "";
            string showedUsername = "";
            string command = "";
            IPEndPoint userIE;

            using (NetworkStream ns = new((Socket)userSocket))
            using (StreamReader sr = new(ns))
            using (StreamWriter sw = new(ns))
            {

                while (isUserConnected)
                {
                    while (!isCorrectUsername)
                    {
                        sw.WriteLine("Insert username: ");
                        sw.Flush();
                        try
                        {
                            username = sr.ReadLine();
                        }
                        catch (IOException)
                        {
                            Debug.WriteLine("Username readLine error");
                            return;
                        }

                        lock (l)
                        {
                            if (users.ContainsKey(username.ToLower()))
                            {
                                sw.WriteLine("Username is taken, try another! ");
                                sw.Flush();
                            }
                            else
                            {
                                sw.WriteLine("Succesfully connected!");
                                sw.Flush();
                                isCorrectUsername = true;
                                users.Add(username.ToLower(), (Socket)userSocket);
                                userIE = (IPEndPoint)((Socket)userSocket).RemoteEndPoint;
                                showedUsername = $"{username}@{userIE.Address}";
                                SendMessage(username, "Connected", (Socket)userSocket);
                            }
                        }
                    }

                    if (GetMessage(sr, out string message))
                    {

                        switch (message.ToUpper())
                        {
                            case LIST:
                                break;
                            case EXIT:
                                SendMessage(showedUsername, "Disconnected", (Socket)userSocket);
                                break;
                            default:
                                SendMessage(showedUsername, message, (Socket)userSocket);
                                break;
                        }
                    }
                    else
                    {
                        lock (l)
                        {
                            SendMessage(showedUsername, "Disconnected", (Socket)userSocket);
                            users.Remove(showedUsername);
                        }
                    }

                }

            }
        }

        private bool GetMessage(StreamReader sr, out string message)
        {
            try
            {
                message = sr.ReadLine();

                if (message == null)
                {
                    return false;
                }
            }
            catch (IOException)
            {
                Debug.WriteLine("Command readLine error");
                message = "";
                return false;
            }

            return true;
        }


        private void SendMessage(string username, string message, Socket userSocket)
        {
            foreach (var user in users)
            {
                if (!user.Value.Equals(userSocket))
                {
                    using (NetworkStream ns = new(user.Value))
                    using (StreamReader sr = new(ns))
                    using (StreamWriter sw = new(ns))
                    {
                        sw.WriteLine($"{username}:{message}");
                        sw.Flush();
                    }
                }

            }
        }

        private bool ProcessPortError(IPEndPoint ie, Socket serverSocket)
        {

            bool isBinded = false;

            for (int i = IPEndPoint.MinPort; i < IPEndPoint.MaxPort; i++)
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
                    Debug.WriteLine($"PORT {i} in use");
                }
            }

            if (!isBinded)
            {
                return false;
            }


            Debug.WriteLine($"Connected to {ie.Port}");
            return true;
        }

        private enum eCommand
        {
            LIST,
        }
    }
}
