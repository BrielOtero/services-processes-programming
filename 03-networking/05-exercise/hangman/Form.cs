using _05_exercise;
using Newtonsoft.Json;
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
        private List<Record> records;
        private Record newRecord;


        //ERROR MESSAGE
        private const string IP_OR_PORT_ERROR_MSG = "Error with IP or PORT configuration.";
        private const string IP_ERROR_MSG = "Sorry but the ip specified in the settings is not available.";
        private const string UNKNOWN_ERROR_MSG = "We are sorry but an unknown error occurred while establishing the connection.";
        private const string SERVER_ERROR_MSG = "The server does not respond.";

        Timer timer = new Timer();

        private enum eCommands
        {
            GETWORD, SENDRECORD, SENDWORD, GETRECORDS
        }



        public Form()
        {
            InitializeComponent();
            timer.Interval = 1000;
            timer.Tick += Timer_OnTick;
        }

        private void TsmiNewGame_Click(object sender, EventArgs e)
        {
            if (!ExecuteCommand(eCommands.GETWORD))
            {
                return;
            }

            StartNewGame();
        }

        private bool ExecuteCommand(eCommands command)
        {
            try
            {
                ie = new IPEndPoint(IPAddress.Parse(IP_SERVER), PORT);
            }
            catch (ArgumentNullException ae)
            {
                Debug.WriteLine(ae.Message);

                ShowMessageError(IP_OR_PORT_ERROR_MSG);
                return false;
            }

            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


            if (!TryEstablishServerConnection())
            {
                return false;
            }

            return CommunicationWithServer(command);
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

        private bool CommunicationWithServer(eCommands command)
        {
            using (NetworkStream ns = new NetworkStream(clientSocket))
            using (StreamWriter sw = new StreamWriter(ns))
            using (StreamReader sr = new StreamReader(ns))
            {
                switch (command)
                {
                    case eCommands.GETWORD:

                        if (!SendMessage(eCommands.GETWORD.ToString(), sw))
                        {
                            ShowMessageError(SERVER_ERROR_MSG);
                            return false;
                        }

                        if (!GetMessage(out string guessWordResponse, sr))
                        {
                            ShowMessageError(SERVER_ERROR_MSG);
                            return false;
                        }

                        guessWord = guessWordResponse;
                        Debug.WriteLine("The word is: " + guessWord);
                        break;
                    case eCommands.GETRECORDS:
                        newRecord = new Record("", 1);

                        if (!SendMessage(eCommands.GETRECORDS.ToString(), sw))
                        {
                            ShowMessageError(SERVER_ERROR_MSG);
                            return false;
                        }
                        if (!GetMessage(out string recordsResponse, sr))
                        {
                            ShowMessageError(SERVER_ERROR_MSG);
                            return false;
                        }
                        records = JsonConvert.DeserializeObject<List<Record>>(recordsResponse);
                        Record maxRecord = records.Max();

                        if (newRecord.Seconds < maxRecord.Seconds)
                        {
                            ExecuteCommand(eCommands.SENDRECORD);
                        }
                        break;
                    case eCommands.SENDRECORD:
                        string recordInJson = JsonConvert.SerializeObject(newRecord);

                        if (!SendMessage($"{eCommands.SENDRECORD.ToString()} {recordInJson}", sw))
                        {
                            ShowMessageError(SERVER_ERROR_MSG);
                            return false;
                        }
                        break;



                }


            }
            return true;
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

        private void StartNewGame()
        {
            Debug.WriteLine("THE GAME START");

            dhHangman.Mistakes = 0;

            timer.Tag = 0;
            timer.Start();

            //if (letters is null)
            //{
            //    letters = new List<TextBox>();
            //}
            //else
            //{
            //    letters.Clear();
            //}


            int x = 0;
            int y = 0;
            foreach (var letter in guessWord)
            {
                TextBox txtLetter = new TextBox();
                txtLetter.Tag = letter;
                txtLetter.Font = new Font("Arial", 18);
                txtLetter.ReadOnly = true;
                txtLetter.MaxLength = 1;
                txtLetter.Size = new Size(30, 30);
                txtLetter.Location = new Point(x, y);
                txtLetter.TextChanged += LetterText_TextChanged;
                pLetters.Controls.Add(txtLetter);
                x += 40;
            }

            dhHangman.Visible = true;
            lblInfo.Visible = true;
            txtTryLetter.Visible = true;
            btnTryLetter.Visible = true;
            lblTime.Visible = true;
        }

        private void LetterText_TextChanged(object sender, EventArgs e)
        {
            TextBox txtLetter = (sender as TextBox);
        }


        private void TxtTryLetter_TextChanged(object sender, EventArgs e)
        {
            TextBox txtTryLetter = (sender as TextBox);

            txtTryLetter.Text = txtTryLetter.Text.ToUpper();

            if (txtTryLetter.Text == "")
            {
                txtTryLetter.BackColor = Color.White;
            }
        }

        private void BtnTryLetter_Click(object sender, EventArgs e)
        {
            string letter = txtTryLetter.Text.Trim();

            if (letter == "") return;
            if (guessWord.Contains(letter))
            {
                txtTryLetter.BackColor = Color.Green;

                Debug.WriteLine("Letter: " + letter);
                for (int i = 0; i < pLetters.Controls.Count; i++)
                {
                    Debug.WriteLine("Tag letter: " + pLetters.Controls[i].Tag);
                    if ((char)(pLetters.Controls[i] as TextBox).Tag == char.Parse(letter))
                    {
                        pLetters.Controls[i].Text = letter;
                    }
                }
            }
            else
            {
                txtTryLetter.BackColor = Color.Red;
                dhHangman.Mistakes += 1;

            }

        }

        private void DhHangman_Hanged(object sender, EventArgs e)
        {

        }

        private void Timer_OnTick(object sender, EventArgs e)
        {
            timer.Tag = (int)timer.Tag + 1;
            TimeSpan time = TimeSpan.FromSeconds((int)timer.Tag);
            Debug.WriteLine((int)timer.Tag);

            lblTime.Text = $"{time.Minutes:D2}:{time.Seconds:D2}";
        }
    }
}
