using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace _02_exercise
{
    public partial class Client : Form
    {
        private readonly string CONFIG_PATH = Path.Combine(Environment.GetEnvironmentVariable("PROGRAMDATA"), "shiftsConfig.json");
        private Config config;

        private const string LOGGING_ERROR_MSG = "Sorry, the user does not exist or the server does not respond.";
        private const string SAVE_CONFIG_ERROR_MSG = "The program can't save the config.";
        private const string PORT_ERROR_MSG = "Unable to find a free port.";
        private const string IP_OR_PORT_ERROR_MSG = "Error with IP or PORT configuration.";
        private const string IP_ERROR_MSG = "Sorry but the ip specified in the settings is not available.";
        private const string UNKNOWN_ERROR_MSG = "We are sorry but an unknown error occurred while establishing the connection.";
        private const string SERVER_ERROR_MSG = "The server does not respond.";
        private const string ASK_FOR_RETRY_PORT_ERROR_MSG = "Sorry but the port in configuration is in use."
                    + "\nDo you want us to try to find a free port for you?" +
                    "\nThis may take a little while.";


        public Client()
        {
            InitializeComponent();
            LoadConfig();

        }

        private void LoadConfig()
        {
            try
            {
                config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(CONFIG_PATH));
            }
            catch (Exception e)
            {
                config = new();
                Debug.WriteLine(e.Message);
            }
        }

        private void SaveConfig()
        {
            try
            {
                string configFile = JsonConvert.SerializeObject(config);
                File.WriteAllText(CONFIG_PATH, configFile);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                ShowMessageError(SAVE_CONFIG_ERROR_MSG);
            }
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            Settings settings = new(config);

            if (settings.ShowDialog() == DialogResult.Yes)
            {

                config = settings.Config;
                SaveConfig();

                Debug.WriteLine("Save");
                Debug.WriteLine($"{settings.Config.IP_Server} {settings.Config.Port} {settings.Config.User}");
                Debug.WriteLine($"{config.IP_Server} {config.Port} {config.User}");
            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            ProcessCommand((sender as Button).Text);
        }

        private void ProcessCommand(string command)
        {
            IPEndPoint ie;

            try
            {
                ie = new(IPAddress.Parse(config.IP_Server), config.Port);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                ShowMessageError(IP_OR_PORT_ERROR_MSG);
                return;
            }

            Socket serverSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            if (!EstablishServerConnection(ie, serverSocket))
            {
                return;
            }

            CommunicationWithServer(serverSocket, command);

            serverSocket.Close();
        }

        private bool EstablishServerConnection(IPEndPoint ie, Socket serverSocket)
        {
            try
            {
                serverSocket.Connect(ie);
            }
            catch (SocketException se)
            {
                Debug.WriteLine($"PORT {ie.Port} in use");
                Debug.WriteLine(se.SocketErrorCode);

                switch (se.SocketErrorCode)
                {
                    case SocketError.TimedOut:
                        ShowMessageError(IP_ERROR_MSG);
                        return false;

                    default:
                        ShowMessageError(UNKNOWN_ERROR_MSG);
                        return false;
                }
            }
            return true;
        }

        private void CommunicationWithServer(Socket serverSocket, string command)
        {

            string loggingResponse;
            string commandResponse;

            using (NetworkStream ns = new(serverSocket))
            using (StreamReader sr = new(ns))
            using (StreamWriter sw = new(ns))
            {
                sw.WriteLine($"USER {config.User}");
                sw.Flush();

                try
                {
                    loggingResponse = sr.ReadLine();
                }
                catch (IOException io)
                {
                    Trace.WriteLine(io.Message);
                    ShowMessageError(SERVER_ERROR_MSG);
                    return;
                }

                Debug.WriteLine(loggingResponse);

                if (loggingResponse != "OK")
                {
                    ShowMessageError(LOGGING_ERROR_MSG);
                    return;
                }
                else
                {

                    Debug.WriteLine("User logged correctly");

                    sw.WriteLine(command.ToLower());
                    sw.Flush();

                    try
                    {
                        commandResponse = sr.ReadToEnd();
                    }
                    catch (IOException io)
                    {
                        Trace.WriteLine(io.Message);
                        ShowMessageError(SERVER_ERROR_MSG);
                        return;
                    }

                    Debug.WriteLine(commandResponse);

                    lblShow.Text = commandResponse;

                }
            }
        }

        private void ShowMessageError(string message)
        {
            MessageBox.Show(this, message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

    }



}