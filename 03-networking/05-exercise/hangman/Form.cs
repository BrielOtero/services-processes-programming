using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hangman
{
    public partial class Form : System.Windows.Forms.Form
    {
        private const int PORT = 31416;
        private const string IP_SERVER = "127.0.0.1";
        private IPEndPoint ie;
        private Socket clientSocket;
        private string guessWord;


        //ERROR MESSAGE
        private const string IP_OR_PORT_ERROR_MSG = "Error with IP or PORT configuration.";
        private const string IP_ERROR_MSG = "Sorry but the ip specified in the settings is not available.";
        private const string UNKNOWN_ERROR_MSG = "We are sorry but an unknown error occurred while establishing the connection.";
        private const string SERVER_ERROR_MSG = "The server does not respond.";



        public Form()
        {
            InitializeComponent();
        }

        private void tsmiNewGame_Click(object sender, EventArgs e)
        {
            try
            {
                ie = new IPEndPoint(IPAddress.Parse(IP_SERVER), PORT);
            }
            catch (ArgumentNullException ae)
            {
                Debug.WriteLine(ae.Message);

                ShowMessageError(IP_OR_PORT_ERROR_MSG);
                return;
            }

            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


            if (!TryEstablishServerConnection())
            {
                return;
            }

            CommunicationWithServer();
        }

        private bool TryEstablishServerConnection()
        {
            try
            {
                clientSocket.Connect(ie);
            }
            catch (SocketException se)
            {
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

        private void CommunicationWithServer()
        {
            using (NetworkStream ns = new NetworkStream(clientSocket))
            using (StreamWriter sw = new StreamWriter(ns))
            using (StreamReader sr = new StreamReader(ns))
            {
                if (!SendMessage("getWord", sw))
                {
                    ShowMessageError(SERVER_ERROR_MSG);
                    return;
                }

                if (!GetMessage(out string response, sr))
                {
                    ShowMessageError(SERVER_ERROR_MSG);
                    return;
                }

                guessWord = response;
                Debug.WriteLine("The word is: " + guessWord);

                StartGame();
            }
        }

        private bool SendMessage(string message, StreamWriter sw)
        {
            try
            {
                sw.WriteLine(message);
                sw.Flush();
            }
            catch (IOException)
            {
                Debug.WriteLine("Error sending message");
                return false;
            }

            return true;
        }

        private bool GetMessage(out string response, StreamReader sr)
        {
            try
            {
                response = sr.ReadLine();
            }
            catch (IOException)
            {
                Debug.WriteLine("Error reading message");
                response = "";
                return false;
            }

            return true;
        }

        private void ShowMessageError(string message)
        {
            MessageBox.Show(this, message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void StartGame()
        {
            Debug.WriteLine("THE GAME START");

            int x = 5;
            int y = 430;
            foreach (var letter in guessWord)
            {
                TextBox txtLetter = new TextBox();
                txtLetter.Tag = letter;
                txtLetter.Font = new Font("Arial", 30);
                txtLetter.ReadOnly = true;
                txtLetter.MaxLength = 1;
                txtLetter.Size = new Size(50, 50);
                txtLetter.Location = new Point(x, y);
                txtLetter.TextChanged += LetterTextChanged;
                this.Controls.Add(txtLetter);
                x += 60;
            }
        }

        private void LetterTextChanged(object sender, EventArgs e)
        {
            TextBox txtLetter = (sender as TextBox);

            txtLetter.Text = txtLetter.Text.ToUpper();

            if(txtLetter.Text == txtLetter.Tag)
            {
                txtLetter.BackColor = Color.Green;
            }else if(txtLetter.Text != txtLetter.Tag && txtLetter.Text != "")
            {
                txtLetter.BackColor = Color.Red;
            }
            else
            {
                txtLetter.BackColor= Color.White;
            }
        }
    }
}
