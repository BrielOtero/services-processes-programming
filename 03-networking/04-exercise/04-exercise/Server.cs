using System;
using System.Collections;
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
        private List<User> connectedUsers = new();
        private List<UserQueue> waitQueue = new();

        private const int PRIMARY_PORT = 31416;
        private const int SECONDARY_PORT = 1024;
        private readonly string PATH_NAMES = Path.Combine(Environment.GetEnvironmentVariable("userprofile"), "usuarios.txt");
        private readonly string PATH_PIN = Path.Combine(Environment.GetEnvironmentVariable("userprofile"), "pin.bin");

        private bool executeServer = true;
        static readonly private object l = new object();
        static readonly private object n = new object();


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
            public string Name;
            public string Time;

            public UserQueue(string name, string time)
            {
                this.Name = name;
                this.Time = time;
            }
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

        private int ReadPin()
        {
            int response = 1234;

            try
            {
                using (FileStream fs = new(PATH_PIN, FileMode.Open))
                using (BinaryReader br = new(fs))
                {
                    try
                    {
                        response = br.ReadInt32();
                        Debug.WriteLine(response);
                    }
                    catch (IOException)
                    {
                        Debug.WriteLine($"Error in {nameof(ReadPin)} in read");
                        return response;
                    }
                }
                return response;
            }
            catch (IOException)
            {
                Debug.WriteLine($"Error in {nameof(ReadPin)} on using");
                return response;
            }
        }

        private bool SavePin(uint pin)
        {
            try
            {
                using (FileStream fs = new(PATH_PIN, FileMode.OpenOrCreate))
                using (BinaryWriter bw = new(fs))
                {
                    try
                    {
                        bw.Write(pin);
                    }
                    catch (IOException)
                    {
                        Debug.WriteLine($"Error in {nameof(ReadPin)} in write");
                        return false;
                    }
                }
                return true;
            }
            catch (IOException)
            {
                Debug.WriteLine($"Error in {nameof(ReadPin)} on using");
                return false;
            }
        }

        public void Init()
        {
            IPEndPoint ie = new(IPAddress.Any, PRIMARY_PORT);

            serverSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

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
            WaitForClientConnection();

        }


        private void WaitForClientConnection()
        {
            while (executeServer)
            {
                try
                {
                    clientSocket = serverSocket?.Accept();
                    new Thread(UserManagement).Start(clientSocket);
                }
                catch (SocketException)
                {
                    Debug.WriteLine($"{nameof(WaitForClientConnection)} Server accept socket");
                }
            }
            Debug.WriteLine("SERVER SOCKET CLOSE");
        }



        private void UserManagement(object userSocket)
        {

            User user = new((Socket)userSocket);

            lock (n)
            {
                connectedUsers.Add(user);
            }

            using (NetworkStream ns = new((Socket)user.UserSocket))
            using (StreamReader sr = new(ns))
            using (StreamWriter sw = new(ns))
            {
                Debug.WriteLine("Before admin");

                while (user.IsAdmin)
                {

                    lock (n)
                    {
                        if (!executeServer) break;
                    }

                    if (!user.IsLogged)
                    {
                        user.IsAdmin = false;

                        SendMessage("WELCOME", sw);
                        SendMessage("Insert your username", sw);

                        switch (GetRegisterUsername(sr, out string username))
                        {

                            case -1:
                                user.IsConnected = false;
                                SendMessage("Unknown user", sw);
                                break;
                            case 0:
                                user.IsLogged = true;
                                user.Username = username;
                                SendMessage("OK", sw);
                                break;
                            case 1:
                                SendMessage("Insert pin:", sw);
                                int pin = ReadPin();

                                if (GetMessage(out string pinString, sr) && int.TryParse(pinString, out int userPin) && pin == userPin)
                                {
                                    user.IsLogged = true;
                                    user.Username = username;
                                    user.IsAdmin = true;
                                    SendMessage("OK", sw);
                                }
                                else
                                {
                                    SendMessage("Wrong pin", sw);
                                    user.IsConnected = false;
                                }
                                break;
                        }
                    }

                    if (user.IsConnected)
                    {
                        if (!GetMessage(out string response, sr))
                        {
                            return;
                        }
                        string[] responseSplit;

                        try
                        {
                            responseSplit = response.Split(" ");
                        }
                        catch (Exception)
                        {
                            Debug.WriteLine("Split error");
                            return;
                        }

                        response = responseSplit[0];

                        if (!String.IsNullOrEmpty(response))
                        {
                            if (Enum.TryParse(typeof(eCommands), response.ToUpper(), false, out object command))
                            {
                                switch (command)
                                {
                                    case eCommands.LIST:
                                        Debug.WriteLine($"{eCommands.LIST.ToString()}");

                                        CommandList(sw);
                                        break;
                                    case eCommands.ADD:
                                        Debug.WriteLine($"{eCommands.ADD.ToString()}");

                                        CommandAdd(user, sw);

                                        break;
                                    case eCommands.DEL:
                                        Debug.WriteLine($"{eCommands.DEL.ToString()}");

                                        if (!user.IsAdmin) return;

                                        CommandDel(sw, responseSplit);

                                        break;
                                    case eCommands.CHPIN:
                                        Debug.WriteLine($"{eCommands.CHPIN.ToString()}");

                                        if (!user.IsAdmin) return;

                                        CommandChpin(sw, responseSplit);

                                        break;
                                    case eCommands.EXIT:
                                        Debug.WriteLine($"{eCommands.EXIT.ToString()}");

                                        if (!user.IsAdmin) return;

                                        user.IsAdmin = false;

                                        break;
                                    case eCommands.SHUTDOWN:
                                        Debug.WriteLine($"{eCommands.SHUTDOWN.ToString()}");

                                        if (!user.IsAdmin) return;

                                        CommandShutdown(user);
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }

                lock (n)
                {
                    connectedUsers.Remove(user);
                }
                user.UserSocket.Close();
            }
        }

        private void CommandList(StreamWriter sw)
        {
            lock (l)
            {
                waitQueue.ForEach((userQueue) =>
                {
                    SendMessage($"{userQueue.Name} {userQueue.Time}", sw);
                });
            }
        }

        private void CommandAdd(User user, StreamWriter sw)
        {
            lock (l)
            {

                if (!Array.Exists(waitQueue.ToArray(), (queueUser) => queueUser.Name == user.Username))
                {
                    waitQueue.Add(new UserQueue(user.Username, DateTime.Now.ToString()));
                    SendMessage("OK", sw);
                }
                else
                {
                    SendMessage("ERROR USER TAKEN", sw);
                }

            }
        }



        private void CommandChpin(StreamWriter sw, string[] responseSplit)
        {
            if (responseSplit.Length == 2)
            {
                lock (n)
                {

                    Debug.WriteLine($"Command {responseSplit[0]} | Pin {responseSplit[1]}");

                    if (uint.TryParse(responseSplit[1], out uint pin) && pin >= 1000 && SavePin(pin))
                    {
                        SendMessage("OK", sw);
                    }
                    else
                    {
                        SendMessage("PIN ERROR", sw);
                    }

                    Debug.WriteLine($"Pin {pin}");
                }
            }
            else
            {
                SendMessage("PIN ERROR", sw);

            }
        }

        private void CommandDel(StreamWriter sw, string[] responseSplit)
        {
            if (responseSplit.Length == 2)
            {
                lock (l)
                {

                    Debug.WriteLine($"Command {responseSplit[0]} | Pos {responseSplit[1]} | waitQueue {waitQueue.Count}");

                    if (uint.TryParse(responseSplit[1], out uint pos) && pos < waitQueue.Count)
                    {
                        waitQueue.RemoveAt((int)pos);
                        SendMessage("OK", sw);
                    }
                    else
                    {
                        SendMessage("DELETE ERROR", sw);
                    }

                    Debug.WriteLine($"Pos {pos}");
                }
            }
            else
            {
                SendMessage("DELETE ERROR", sw);
            }
        }

        private void CommandShutdown(User user)
        {
            lock (n)
            {
                executeServer = false;
                connectedUsers.ForEach(connectedUser =>
                {
                    if (!connectedUser.Equals(user))
                    {
                        connectedUser.UserSocket.Close();
                    }
                });
                serverSocket.Close();
            }
        }

        private void SendMessage(string message, StreamWriter sw)
        {
            try
            {
                sw.WriteLine(message);
                sw.Flush();
            }
            catch (IOException)
            {
                Debug.WriteLine($"Error on {nameof(SendMessage)} method sending message: {message}");
            }
        }

        private bool GetMessage(out string message, StreamReader sr)
        {
            try
            {
                message = sr.ReadLine();
            }
            catch (Exception)
            {
                message = "";
                return false;
            }
            return true;
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
