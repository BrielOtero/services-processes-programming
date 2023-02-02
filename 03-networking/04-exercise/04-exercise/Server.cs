using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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

        private struct User
        {

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

        private void ReadPin(string path)
        {
            int response;

            using (FileStream fs = new(path, FileMode.Open))
            using (BinaryReader br = new(fs))
            {
                fs.Seek(0, SeekOrigin.Begin);
                response = br.ReadInt16();

                Debug.WriteLine(response);

            }
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

            clientSocket = serverSocket.Accept();

            new Thread(UserManagement).Start(clientSocket);


           

        }

        private void UserManagement(object clientSocket)
        {
            using (NetworkStream ns = new((Socket)clientSocket))
            using (StreamReader sr = new(ns))
            using (StreamWriter sw = new(ns))
            {



                sw.WriteLine("WELCOME");
                sw.Flush();
                sw.WriteLine("Insert your username");
                sw.Flush();












            }
        }




    }

}
