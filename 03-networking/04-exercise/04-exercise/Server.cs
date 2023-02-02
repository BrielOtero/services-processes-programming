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
        private List<string> waitQueue;

        private const int PRIMARY_PORT = 31416;
        private const int SECONDARY_PORT = 1024;
        private readonly string PATH_NAMES = Path.Combine(Environment.GetEnvironmentVariable("userprofile"), "usuarios.txt");

        private Socket serverSocket;
        private Socket clientSocket;

        private enum eCommands
        {
            LIST, ADD, DEL, CHPIN, EXIT, SHUTDOWN
        }


        private void ReadNames(string path)
        {
            string fileContent;

            try
            {
                fileContent = File.ReadAllText(path);
            }
            catch (Exception)
            {
                Trace.WriteLine($"Error in {nameof(ReadNames)} reading file content");
                return;
            }

            string[] fileUsers = fileContent.Split(';');
            users = fileUsers;
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

        private void Init()
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
            ReadNames(PATH_NAMES);

            while (true)
            {
                clientSocket = serverSocket.Accept();

                new Thread(UserManagement).Start(clientSocket);
            }


        }

        private void UserManagement(object clientSocket)
        {
            bool isUserConnected = true;
            bool isUserLogged = false;
            using (NetworkStream ns = new((Socket)clientSocket))
            using (StreamReader sr = new(ns))
            using (StreamWriter sw = new(ns))
            {
                while (isUserConnected)
                {
                    if (!isUserLogged)
                    {
                        sw.WriteLine("WELCOME");
                        sw.Flush();
                        sw.WriteLine("Insert your username");
                        sw.Flush();

                        switch (GetRegisterUsername(sr, out string message))
                        {

                            case -1:
                                isUserConnected = false;
                                sw.WriteLine("Unknown user");
                                break;
                            case 0:
                                isUserLogged = true;
                                break;
                            case 1:
                                isUserLogged = true;
                                break;
                        }
                    }
                    string response = sr.ReadLine();


                    switch (Enum.Parse(typeof(eCommands), response.ToLower()))
                    {
                        case eCommands.LIST:
                            waitQueue.ForEach((x) =>
                            {
                                try
                                {
                                    sw.WriteLine(x);
                                }
                                catch (IOException)
                                {
                                    Debug.WriteLine()
                                }
                            });
                            break;
                        case eCommands.ADD:
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
