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
        private string PASSWORD_PATH = Path.Combine(Environment.GetEnvironmentVariable("APPDATA").ToString(), "password.txt");
        private IPEndPoint? ie;

        private Socket? socketServer;
        private Socket? socketClient;





        public void StartServer()
        {
            ie = new IPEndPoint(IPAddress.Any, PORT);

            using (socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {

                try
                {
                    socketServer.Bind(ie);
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
                waitForConnection();
            }


        }

        public void waitForConnection()
        {
            while (true)
            {
                using (socketClient = socketServer?.Accept())
                using (NetworkStream ns = new(socketClient))
                using (StreamReader sr = new(ns))
                using (StreamWriter sw = new(ns))
                {
                    while (waitForCommand(sr, sw)) ;
                }
            }
        }

        private bool waitForCommand(StreamReader sr, StreamWriter sw)
        {
            string? command;

            try
            {
                command = sr.ReadLine()?.ToUpper();

                if (string.IsNullOrEmpty(command))
                {
                    return true;
                }

                int response = tryExecuteCommand(command, sw);

                return proccessCommandResponse(sw, response);

            }
            catch (IOException io)
            {
                Trace.WriteLine(io.Message);
                return false;
            }
        }

        private static bool proccessCommandResponse(StreamWriter sw, int response)
        {
            switch (response)
            {
                case 0:
                    sw.WriteLine("Disconnected succesfully");
                    sw.Flush();
                    return false;
                case 1:
                    sw.WriteLine("The command is not defined");
                    sw.Flush();
                    return true;
                case 2:
                    sw.WriteLine("The password is incorrect");
                    sw.Flush();
                    return true;
                case 3:
                    sw.WriteLine("There isn't password, you must send close and password");
                    sw.Flush();
                    return true;
                case 4:
                    sw.WriteLine("Sorry the programmer who made me forgot to create the password. I recommend you go insult him!");
                    sw.Flush();
                    return false;

                default:
                    return true;
            }
        }

        private int tryExecuteCommand(string command, StreamWriter sw)
        {
            string password;
            string[] splitCommand = command.Split(null);

            if (!Enum.IsDefined(typeof(eCommands), splitCommand[0]))
            {
                return 1;
            }

            switch ((eCommands)Enum.Parse(typeof(eCommands), splitCommand[0].ToUpper()))
            {
                case eCommands.TIME:
                    sw.WriteLine(DateTime.Now.ToString("hh:mm:ss"));
                    return 0;

                case eCommands.DATE:
                    sw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy"));
                    return 0;

                case eCommands.ALL:
                    sw.WriteLine(DateTime.Now.ToString());
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
