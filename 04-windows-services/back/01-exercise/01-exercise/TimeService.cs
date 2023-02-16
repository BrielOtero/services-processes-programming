using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace _01_exercise
{
    public enum eCommands
    {
        TIME, DATE, ALL, CLOSE
    }

    public partial class TimeService : ServiceBase
    {

        private const int DEFAULT_PORT = 31416;
        private int PORT = 31416;
        private bool serverRunning = true;


        private readonly string PASSWORD_PATH = Path.Combine(Environment.GetEnvironmentVariable("PROGRAMDATA").ToString(), "password.txt");
        private readonly string PORT_PATH = Path.Combine(Environment.GetEnvironmentVariable("PROGRAMDATA").ToString(), "server-config.txt");
        private IPEndPoint ie;

        private Socket socketServer;
        private Socket socketClient;
        public TimeService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            StartServer();
        }

        protected override void OnStop()
        {
            serverRunning = false;
        }

        private void writeEvent(string message)
        {
            string name = "Time Service";
            string logDestination = "Application";

            if (!EventLog.SourceExists(name))
            {
                EventLog.CreateEventSource(name, logDestination);
            }
            EventLog.WriteEntry(name, message);
        }


        private void StartServer()
        {
            string portFileContent = File.ReadAllText(PORT_PATH);

            if (string.IsNullOrEmpty(portFileContent))
            {
                PORT = DEFAULT_PORT;
                writeEvent($"Error reading file PORT!");

            }
            else
            {
                if (uint.TryParse(portFileContent, out uint readPort))
                {
                    PORT = (int)readPort;
                    writeEvent($"Error reading file PORT!");
                }
                else
                {
                    PORT = DEFAULT_PORT;
                }
            }

            ie = new IPEndPoint(IPAddress.Any, PORT);
            Console.WriteLine("Start server");

            using (socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {

                try
                {

                    socketServer.Bind(ie);
                    Console.WriteLine("Bind establish");
                    writeEvent($"Listen PORT: {ie.Port}");
                }
                catch (SocketException s)
                {
                    Console.WriteLine(s.Message);
                    Console.WriteLine($"Port {ie.Port} in use!");
                    writeEvent("All PORTS in use");
                    return;
                }

                socketServer.Listen(10);
                WaitForConnection();
            }
            socketServer.Close();

        }

        public void WaitForConnection()
        {
            Console.WriteLine("Waiting for connection");

            while (serverRunning)
            {
                using (socketClient = socketServer?.Accept())
                using (NetworkStream ns = new NetworkStream(socketClient))
                using (StreamReader sr = new StreamReader(ns))
                using (StreamWriter sw = new StreamWriter(ns))
                {
                    Console.WriteLine("Connected");
                    serverRunning = WaitForCommand(sr, sw);
                }
            }
        }

        private bool WaitForCommand(StreamReader sr, StreamWriter sw)
        {
            string command;
            Console.WriteLine("Waiting for command");
            try
            {
                command = sr.ReadLine();

                if (string.IsNullOrEmpty(command))
                {
                    return false;
                }

                int response = TryExecuteCommand(command, sw);

                return ProcessCommandResponse(sw, response);

            }
            catch (IOException io)
            {
                Trace.WriteLine(io.Message);
                return false;
            }
        }

        private bool ProcessCommandResponse(StreamWriter sw, int response)
        {
            string message;

            switch (response)
            {
                case 0:
                    message = "Disconnected succesfully";
                    Console.WriteLine(message);
                    sw.WriteLine(message);
                    sw.Flush();
                    return false;
                case 1:
                    message = "Disconnected succesfully";
                    Console.WriteLine(message);

                    sw.WriteLine(message);
                    sw.Flush();
                    return true;
                case 2:
                    message = "The command is not defined";
                    Console.WriteLine(message);

                    sw.WriteLine(message);
                    sw.Flush();
                    goto case 1;
                case 3:
                    message = "The password is incorrect";
                    Console.WriteLine(message);

                    sw.WriteLine(message);
                    sw.Flush();
                    goto case 1;
                case 4:
                    message = "There isn't password, you must send close and password";
                    Console.WriteLine(message);

                    sw.WriteLine(message);
                    sw.Flush();
                    goto case 1;
                case 5:
                    message = "Sorry the programmer who made me forgot to create the password. I recommend you go insult him!";
                    Console.WriteLine(message);

                    sw.WriteLine(message);
                    sw.Flush();
                    goto case 1;

                default:
                    return false;
            }
        }

        private int TryExecuteCommand(string command, StreamWriter sw)
        {
            string password;
            string time;
            bool hasPassword;
            string restCommand = "";
            string[] splitCommand = command.Split(null);


            if (!Enum.IsDefined(typeof(eCommands), splitCommand[0].ToUpper()))
            {
                return 1;
            }

            if (command.ToUpper().StartsWith(eCommands.CLOSE.ToString()) && splitCommand.Length > 1)
            {
                for (int i = 1; i < splitCommand.Length; i++)
                {
                    restCommand += splitCommand[i];
                }

                hasPassword = restCommand.Length > 0;
            }
            else
            {
                hasPassword = false;
            }


            switch ((eCommands)Enum.Parse(typeof(eCommands), splitCommand[0].ToUpper()))
            {
                case eCommands.TIME:
                    time = DateTime.Now.ToString("hh:mm:ss");

                    sw.WriteLine(time);
                    Console.WriteLine(time);
                    return 1;

                case eCommands.DATE:
                    time = DateTime.Now.ToString("dd/MM/yyyy");

                    sw.WriteLine(time);
                    Console.WriteLine(time);
                    return 1;

                case eCommands.ALL:
                    time = DateTime.Now.ToString();

                    sw.WriteLine(time);
                    Console.WriteLine(time);
                    return 1;

                case eCommands.CLOSE:


                    if (!hasPassword)
                    {
                        return 4;
                    }

                    try
                    {
                        password = File.ReadAllText(PASSWORD_PATH).Trim();
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine(e.Message);
                        return 5;
                    }

                    return password == restCommand ? 0 : 3;
            }
            return 2;
        }
    }

}
