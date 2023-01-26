using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace _02_exercise
{
    public partial class Client : Form
    {
        readonly string CONFIG_PATH = Path.Combine(Environment.GetEnvironmentVariable("PROGRAMDATA"), "shiftsConfig.json");
        private Config config;

        private const string LOGGING_ERROR_MSG = "Sorry, the user does not exist or the server does not respond.";
        private const string SAVE_CONFIG_ERROR_MSG = "The program can't save the config";
        private const string PORT_ERROR_MSG = "Unable to find a free port";
        private const string IP_OR_PORT_ERROR_MSG = "Error with IP or PORT configuration";
        private const string SERVER_ERROR_MSG = "The server does not respond.";
        private const string ASK_FOR_RETRY_PORT_ERROR_MSG = "Sorry but the port in configuration is in use"
                    + "\nDo you want us to try to find a free port for you?" +
                    "\nThis may take a little while";


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
                serverSocket.Bind(ie);
            }
            catch (Exception)
            {
                Debug.WriteLine($"PORT {ie.Port} in use");

                if (MessageBox.Show(this, ASK_FOR_RETRY_PORT_ERROR_MSG, "ERROR", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return false;
                }
                else
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
                        ShowMessageError(PORT_ERROR_MSG);
                        return false;
                    }

                }
            }
            Debug.WriteLine($"Connected to {ie.Port}");
            return true;
        }

        private void CommunicationWithServer(Socket serverSocket, string command)
        {

            string firstResponse;
            string secondResponse;

            using (NetworkStream ns = new(serverSocket))
            using (StreamReader sr = new(ns))
            using (StreamWriter sw = new(ns))
            {
                sw.WriteLine($"USER {config.User}");
                sw.Flush();

                try
                {
                    firstResponse = sr.ReadLine();
                }
                catch (IOException io)
                {
                    Trace.WriteLine(io.Message);
                    ShowMessageError(SERVER_ERROR_MSG);
                    return;
                }

                Debug.WriteLine(firstResponse);

                if (firstResponse == "OK")
                {
                    Debug.WriteLine("User logged correctly");

                    sw.WriteLine(command);
                    sw.Flush();

                    try
                    {
                        secondResponse = sr.ReadToEnd();
                    }
                    catch (IOException io)
                    {
                        Trace.WriteLine(io.Message);
                        ShowMessageError(SERVER_ERROR_MSG);
                        return;
                    }

                    Debug.WriteLine(secondResponse);

                    lblShow.Text = secondResponse;
                }
                else
                {
                    ShowMessageError(LOGGING_ERROR_MSG);
                    return;
                }
            }
        }

        private void ShowMessageError(string message)
        {
            MessageBox.Show(this, message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

    }



}