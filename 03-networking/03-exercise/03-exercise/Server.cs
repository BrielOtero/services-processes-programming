using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static _03_exercise.Server;

namespace _03_exercise
{
    internal class Server
    {
        private const int PORT = 31416;
        private IPEndPoint ie;
        private Socket serverSocket;
        private Socket userSocket;
        private Dictionary<String, User> users = new();
        private const string EXIT = "#EXIT";
        private const string LIST = "#LIST";


        public struct User
        {
            public Socket UserSocket;
            public string PublicUsername;
            public string Username;
            public bool IsConnected;
            public bool IsRegister;

            public User() : this(null, "", "", false, false) { }
            public User(Socket userSocket) : this(userSocket, "", "", true, false) { }

            public User(Socket userSocket, string publicUsername, string username, bool isConnected, bool isRegister)
            {
                this.UserSocket = userSocket;
                this.PublicUsername = publicUsername;
                this.Username = username;
                this.IsConnected = isConnected;
                this.IsRegister = isRegister;
            }
        }


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
                new Thread(UserManagement).Start(userSocket);
            }
        }
        static readonly private object l = new object();

        private void UserManagement(object userSocket)
        {
            User user = new((Socket)userSocket);


            using (NetworkStream ns = new(user.UserSocket))
            using (StreamReader sr = new(ns))
            using (StreamWriter sw = new(ns))
            {
                while (user.IsConnected)
                {
                    while (!user.IsRegister && user.IsConnected)
                    {
                        sw.WriteLine("Insert username: ");
                        sw.Flush();
                        user = RegisterUser(user, sw, sr);
                    }


                    int messageResponse = GetMessage(sr, out string message);

                    lock (l)
                    {
                        switch (messageResponse)
                        {
                            case -1:
                                user = ProcessMessage(user, sw, "#EXIT");
                                break;
                            case 0:
                                user = ProcessMessage(user, sw, message);
                                break;
                            case 1:
                                break;
                        }
                    }
                }
                user.UserSocket.Close();
            }
        }

        private User RegisterUser(User user, StreamWriter sw, StreamReader sr)
        {
            try
            {
                user.Username = sr.ReadLine()?.ToLower();
                Debug.WriteLine($"User {user.Username}");
            }
            catch (IOException)
            {
                Debug.WriteLine("Username readLine error");
                return user;
            }

            if (user.Username == String.Empty)
            {
                sw.WriteLine("Insert a valid username please! ");
                sw.Flush();
                return user;
            }

            if (user.Username == null)
            {
                Debug.WriteLine("Username is null");
                user.IsConnected = false;
                return user;
            }

            lock (l)
            {
                if (users.ContainsKey(user.Username))
                {
                    sw.WriteLine("Username is taken, try another! ");
                    sw.Flush();
                    return user;
                }
                else
                {
                    try
                    {
                        user.PublicUsername = $"{user.Username}@{((userSocket as Socket).RemoteEndPoint as IPEndPoint).Address.ToString()}";
                        Debug.WriteLine("USER  " + user.Username);
                    }
                    catch (Exception)
                    {
                        user.PublicUsername = $"{user.Username}@{"NoIPAvailable"}";
                        Debug.WriteLine("USER  " + user.Username);

                        Debug.WriteLine("Error getting publicUsername");
                    }


                    user.IsRegister = true;
                    users.Add(user.Username, user);

                    sw.WriteLine("Succesfully connected!");
                    sw.Flush();

                    SendMessage(user.Username, user.PublicUsername, "Connected");

                    return user;

                }
            }
        }


        private User ProcessMessage(User user, StreamWriter sw, string message)
        {

            switch (message.ToUpper())
            {
                case LIST:
                    sw.WriteLine("Connected users:");
                    sw.Flush();
                    sw.WriteLine();
                    sw.Flush();

                    foreach (var tempUser in users.Values)
                    {
                        sw.WriteLine(tempUser.PublicUsername);
                        sw.Flush();
                    }
                    break;
                case EXIT:
                    users.Remove(user.Username);
                    SendMessage(user.Username, user.PublicUsername, "Disconnected");
                    user.IsConnected = false;
                    break;
                default:
                    SendMessage(user.Username, user.PublicUsername, message);
                    break;
            }

            return user;
        }

        private int GetMessage(StreamReader sr, out string message)
        {
            try
            {
                message = sr.ReadLine();

                if (message == String.Empty)
                {
                    return 1;
                }

                if (message == null)
                {
                    return -1;
                }
            }
            catch (IOException)
            {
                Debug.WriteLine("Command readLine error");
                message = "";
                return -1;
            }

            return 0;
        }


        private void SendMessage(string username, string publicUsername, string message)
        {

            foreach (var user in users)
            {
                if (user.Key != username)
                {
                    using (NetworkStream ns = new(user.Value.UserSocket))
                    using (StreamReader sr = new(ns))
                    using (StreamWriter sw = new(ns))
                    {
                        sw.WriteLine($"{publicUsername}:{message}");
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

    }
}
