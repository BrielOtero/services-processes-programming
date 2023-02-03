using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _04_exercise
{

    internal class Server
    {
        private string[] users;
        private List<string> waitQueue = new();

        private const int PRIMARY_PORT = 31416;
        private const int SECONDARY_PORT = 1024;
        private readonly string PATH_NAMES = Path.Combine(Environment.GetEnvironmentVariable("userprofile"), "usuarios.txt");

        private Socket serverSocket;
        private Socket clientSocket;


        private enum eCommands
        {
            LIST, ADD, DEL, CHPIN, EXIT, SHUTDOWN
        }

        private struct User
        {
            public bool IsConnected;
            public bool IsLogged;
            public bool IsAdmin;
            public Socket UserSocket;
            public string Username;

            public User(Socket userSocket) : this(userSocket, true, false, true, "") { }
            public User(Socket userSocket, bool isConnected, bool isLogged, bool isAdmin, string username)
            {
                UserSocket = userSocket;
                IsConnected = isConnected;
                IsLogged = isLogged;
                IsAdmin = isAdmin;
                Username = username;
            }
        }

        private struct UserQueue
        {
            public string Name
        }

        private bool ReadNames(string path)
        {
            string fileContent;

            try
            {
                fileContent = File.ReadAllText(path);
            }
            catch (Exception)
            {
                Trace.WriteLine($"Error in {nameof(ReadNames)} reading file content");
                return false;
            }

            string[] fileUsers = fileContent.Split(';');
            users = fileUsers;
            return true;
        }

        private int ReadPin(string path)
        {
            int response;

            using (FileStream fs = new(path, FileMode.Open))
            using (BinaryReader br = new(fs))
            {
                try
                {
                    response = br.ReadInt32();
                    Debug.WriteLine(response);
                }
                catch (IOException)
                {
                    return -1;
                }
            }
            return 0;
        }

        public void Init()
        {
            IPEndPoint ie = new(IPAddress.Any, PRIMARY_PORT);

            Socket serverSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                serverSocket.Bind(ie);
            }
            catch (SocketException)
            {
                Trace.WriteLine($"Port {ie.Port} in use!");
                Trace.WriteLine($"{nameof(PRIMARY_PORT)} don't binded");

                bool isBinded = false;
                for (int i = SECONDARY_PORT; i < IPEndPoint.MaxPort; i++)
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
                        Trace.WriteLine($"Port {ie.Port} in use!");
                    }
                }

                if (!isBinded)
                {
                    Trace.WriteLine($"{nameof(SECONDARY_PORT)} don't bind");
                    return;
                }
            }

            Console.WriteLine($"Server connected on port: {ie.Port}");


            if (!ReadNames(PATH_NAMES))
            {
                Console.WriteLine("Error reading USERS file");
                return;
            }
            serverSocket.Listen(18);
            WaitForClientConnection(serverSocket);

        }



        private void WaitForClientConnection(Socket serverSocket)
        {
            while (true)
            {
                clientSocket = serverSocket?.Accept();

                new Thread(UserManagement).Start(clientSocket);
            }
        }

       

        private void UserManagement(object userSocket)
        {
            //bool isUserConnected = true;
            //bool isUserLogged = false;
            //bool isAdmin = false;

            User user = new((Socket)userSocket);

            using (NetworkStream ns = new((Socket)user.UserSocket))
            using (StreamReader sr = new(ns))
            using (StreamWriter sw = new(ns))
            {
                Debug.WriteLine("Before admin");

                while (user.IsAdmin)
                {
                    Debug.WriteLine("New user connected");

                    user.IsAdmin = false;

                    if (!user.IsLogged)
                    {
                        sw.WriteLine("WELCOME");
                        sw.Flush();
                        sw.WriteLine("Insert your username");
                        sw.Flush();

                        switch (GetRegisterUsername(sr, out string username))
                        {

                            case -1:
                                user.IsConnected = false;
                                sw.WriteLine("Unknown user");
                                sw.Flush();
                                break;
                            case 0:
                                user.IsLogged = true;
                                user.Username = username;
                                sw.WriteLine("OK");
                                sw.Flush();
                                break;
                            case 1:
                                user.IsLogged = true;
                                break;
                        }
                    }

                    if (user.IsConnected)
                    {
                        string response = sr.ReadLine();

                        if (!String.IsNullOrEmpty(response))
                        {
                            if (Enum.TryParse(typeof(eCommands), response.ToUpper(), false, out object command))
                            {
                                switch (command)
                                {
                                    case eCommands.LIST:
                                        waitQueue.ForEach((x) =>
                                        {
                                            try
                                            {
                                                sw.WriteLine(x);
                                                sw.Flush();
                                            }
                                            catch (IOException)
                                            {
                                                Debug.WriteLine("Error showing waitQueue users");
                                            }
                                        });

                                        break;
                                    case eCommands.ADD:
                                        waitQueue.Add($"{user.Username} {DateTime.Now}");
                                        break;
                                    case eCommands.DEL:
                                        break;
                                    case eCommands.CHPIN:
                                        break;
                                    case eCommands.EXIT:
                                        break;
                                    case eCommands.SHUTDOWN:
                                        break;
                                    default:
                                        break;
                                }
                            }

                        }

                    }

                }
                user.UserSocket.Close();
            }
        }

        private int GetRegisterUsername(StreamReader sr, out string username)
        {
            string name;

            try
            {
                name = sr.ReadLine();
                Debug.WriteLine($"{nameof(GetRegisterUsername)} username is: {name}");
            }
            catch (IOException)
            {
                Debug.WriteLine($"{nameof(GetRegisterUsername)} error");
                username = "";
                return -1;
            }

            if (String.IsNullOrEmpty(name))
            {
                username = "";
                return -1;
            }

            if (Array.Exists(users, (user) => user == name))
            {
                username = name;
                return 0;
            }
            else if (name.ToLower() == "admin")
            {
                username = name;
                return 1;
            }

            username = "";
            return -1;
        }

    }



}
