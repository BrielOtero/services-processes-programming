using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _01_exercise
{
    public enum eCommands
    {
        TIME, DATE, ALL, CLOSE
    }

    internal class Server
    {
        private const int PORT = 31416;
        private readonly string PASSWORD_PATH = Path.Combine(Environment.GetEnvironmentVariable("APPDATA").ToString(), "password.txt");
        private IPEndPoint? ie;

        private Socket? socketServer;
        private Socket? socketClient;

        public void StartServer()
        {
            ie = new IPEndPoint(IPAddress.Any, PORT);
            Console.WriteLine("Start server");

            using (socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {

                try
                {
                    socketServer.Bind(ie);
                    Console.WriteLine("Bind establish");
                }
                catch (SocketException s)
                {
                    Console.WriteLine(s.Message);
                    return;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }

                socketServer.Listen(10);
                WaitForConnection();
            }


        }

        public void WaitForConnection()
        {
            Console.WriteLine("Waiting for connection");
            while (true)
            {
                using (socketClient = socketServer?.Accept())
                using (NetworkStream ns = new(socketClient))
                using (StreamReader sr = new(ns))
                using (StreamWriter sw = new(ns))
                {
                    WaitForCommand(sr, sw);
                }
            }
        }

        private void WaitForCommand(StreamReader sr, StreamWriter sw)
        {
            string? command;

            try
            {
                command = sr.ReadLine()?.ToUpper();

                if (string.IsNullOrEmpty(command))
                {
                    return;
                }

                int response = TryExecuteCommand(command, sw);

                ProcessCommandResponse(sw, response);

            }
            catch (IOException io)
            {
                Trace.WriteLine(io.Message);
            }
        }

        private void ProcessCommandResponse(StreamWriter sw, int response)
        {
            string message;

            switch (response)
            {
                case 0:
                    message = "Disconnected succesfully";
                    Console.WriteLine(message);

                    sw.WriteLine(message);
                    sw.Flush();
                    break;
                case 1:
                    message = "The command is not defined";
                    Console.WriteLine(message);

                    sw.WriteLine(message);
                    sw.Flush();
                    goto case 0;
                case 2:
                    message = "The password is incorrect";
                    Console.WriteLine(message);

                    sw.WriteLine(message);
                    sw.Flush();
                    goto case 0;
                case 3:
                    message = "There isn't password, you must send close and password";
                    Console.WriteLine(message);

                    sw.WriteLine(message);
                    sw.Flush();
                    goto case 0;
                case 4:
                    message = "Sorry the programmer who made me forgot to create the password. I recommend you go insult him!";
                    Console.WriteLine(message);

                    sw.WriteLine(message);
                    sw.Flush();
                    goto case 0;

                default:
                    break;
            }
        }

        private int TryExecuteCommand(string command, StreamWriter sw)
        {
            string password;
            string time;
            string[] splitCommand = command.Split(null);

            if (!Enum.IsDefined(typeof(eCommands), splitCommand[0]))
            {
                return 1;
            }

            switch ((eCommands)Enum.Parse(typeof(eCommands), splitCommand[0].ToUpper()))
            {
                case eCommands.TIME:
                    time = DateTime.Now.ToString("hh:mm:ss");

                    sw.WriteLine(time);
                    Console.WriteLine(time);
                    return 0;

                case eCommands.DATE:
                    time = DateTime.Now.ToString("dd/MM/yyyy");

                    sw.WriteLine(time);
                    Console.WriteLine(time);
                    return 0;

                case eCommands.ALL:
                    time = DateTime.Now.ToString();

                    sw.WriteLine(time);
                    Console.WriteLine(time);
                    return 0;

                case eCommands.CLOSE:


                    if (splitCommand.Length <= 1 || string.IsNullOrEmpty(splitCommand[1].Trim()))
                    {
                        return 3;
                    }

                    try
                    {
                        password = File.ReadAllText(PASSWORD_PATH).Trim();
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine(e.Message);
                        return 4;
                    }

                    return $"close {password}" == command.ToLower() ? 0 : 2;
            }
            return 1;
        }
    }



}
