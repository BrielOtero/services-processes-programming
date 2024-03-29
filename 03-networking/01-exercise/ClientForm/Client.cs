using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace ClientForm
{
    public partial class Client : Form
    {
        private string IP_SERVER = "127.0.0.1";
        private int PORT = 31416;
        private Socket serverSocket;

        public Client()
        {
            InitializeComponent();
        }

        private void ProcessCommand(string command)
        {
            IPEndPoint ie = new IPEndPoint(IPAddress.Parse(IP_SERVER), PORT);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            if (!EstablishServerConnection(ie, serverSocket))
            {
                return;
            }

            CommunicationWithServer(command);

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
                Trace.WriteLine(se.Message);
                MessageBox.Show(this, "Unable to connect to server", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void CommunicationWithServer(string command)
        {
            using (NetworkStream ns = new(serverSocket))
            using (StreamReader sr = new(ns))
            using (StreamWriter sw = new(ns))
            {
                sw.WriteLine(command);
                sw.Flush();
                try
                {
                    lblResponse.Text = sr.ReadLine();
                    lblResponse.Text += "\n" + sr.ReadLine();

                }
                catch (IOException io)
                {
                    Trace.WriteLine(io.Message);
                    lblResponse.Text = "Sorry an error has occurred";
                }
            }
        }


        private void SendCommand(object sender, EventArgs e)
        {
            string command = (sender as Button).Text.ToLower();
            command = command == "close" ? $"{command} {txtPassword.Text}" : command;

            Trace.WriteLine("sendCommand: " + command + "<--");

            ProcessCommand(command);
        }

        private void BtnConfig_Click(object sender, EventArgs e)
        {
            Config config = new(IP_SERVER, PORT);
            if (config.ShowDialog() == DialogResult.Yes)
            {
                IP_SERVER = config.IP_Server;
                PORT = config.Port;
            }
        }
    }
}