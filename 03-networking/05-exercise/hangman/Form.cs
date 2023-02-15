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
        //private const string IP_SERVER = "192.168.20.18";
        //private const int PORT = 31416;
        private const string IP_SERVER = "127.0.0.1";
        private IPEndPoint ie;
        private Socket clientSocket;
        private string guessWord;
        private List<Record> records;
        private Record newRecord;
        private int guessedLetters;
        private bool isHanged;
        private string words;


        //ERROR MESSAGE
        private const string IP_OR_PORT_ERROR_MSG = "Error with IP or PORT configuration.";
        private const string IP_ERROR_MSG = "Sorry but the ip specified in the settings is not available.";
        private const string UNKNOWN_ERROR_MSG = "We are sorry but an unknown error occurred while establishing the connection.";
        private const string SERVER_ERROR_MSG = "The server does not respond.";
        private const string NO_RECORD_MSG = "We're sorry but you haven't broken any records";
        private const string YOU_HAVE_RECORD_MSG = "You have a new record";
        private const string YOU_LOST_MSG = "You lost!";
        private const string NO_RECORDS = "There are no records";
        private const string WORDS_SAVED = "Words saved";




        Timer timer = new Timer();

        private enum eCommands
        {
            GETWORD, SENDRECORD, GETRECORDS, SENDWORD, ISRECORD
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
                        return GetWord(sw, sr);
                    case eCommands.SENDWORD:
                        return SendWord(sw);
                    case eCommands.ISRECORD:
                        return IsRecord(sw, sr);
                    case eCommands.SENDRECORD:
                        return SendRecord(sw);
                    case eCommands.GETRECORDS:
                        return GetRecords(sw, sr);
                }
            }
            return true;
        }

        private bool SendRecord(StreamWriter sw)
        {
            RecordName recordName = new RecordName();

            if (recordName.ShowDialog() != DialogResult.OK)
            {
                return false;
            }

            newRecord.Name = recordName.Name;

            string recordInJson = JsonConvert.SerializeObject(newRecord);

            if (!SendMessage($"{eCommands.SENDRECORD.ToString()} {recordInJson}", sw))
            {
                ShowMessageError(SERVER_ERROR_MSG);
                return false;
            }

            return true;
        }


        private bool SendWord(StreamWriter sw)
        {
            if (!SendMessage($"{eCommands.SENDWORD.ToString()} {words}", sw))
            {
                ShowMessageError(SERVER_ERROR_MSG);
                return false;
            }

            return true;
        }

        private bool IsRecord(StreamWriter sw, StreamReader sr)
        {
            newRecord = new Record("", (int)timer.Tag);

            Debug.WriteLine("New Record time: " + (int)timer.Tag);

            if (!SendMessage("GETRECORDS", sw))
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
            Debug.WriteLine("Records Count: " + records.Count);

            if (records.Count >= 3)
            {
                Record maxRecord = records.Max();

                if (newRecord.Seconds >= maxRecord.Seconds)
                {
                    return false;
                }
            }

            return true;
        }

        private bool GetRecords(StreamWriter sw, StreamReader sr)
        {

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
            Debug.WriteLine("Records Count: " + records.Count);

            return true;
        }

        private bool GetWord(StreamWriter sw, StreamReader sr)
        {
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

            guessWord = guessWordResponse.ToUpper();
            Debug.WriteLine("The word is: " + guessWord);

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

            pLetters.Controls.Clear();

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
                pLetters.Controls.Add(txtLetter);
                x += 40;
            }

            dhHangman.Mistakes = 0;
            timer.Tag = 0;
            timer.Start();
            isHanged = false;
            guessedLetters = 0;
            dhHangman.Visible = true;
            lblInfo.Visible = true;
            txtTryLetter.Visible = true;
            btnTryLetter.Visible = true;
            lblTime.Visible = true;
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

            if (letter == "" || isHanged) return;

            if (guessWord.Contains(letter))
            {
                txtTryLetter.BackColor = Color.Green;

                Debug.WriteLine("Letter: " + letter);
                for (int i = 0; i < pLetters.Controls.Count; i++)
                {
                    Debug.WriteLine("Tag letter: " + pLetters.Controls[i].Tag);
                    char charLetter = (char)(pLetters.Controls[i] as TextBox).Tag;

                    if (charLetter == char.Parse(letter) && pLetters.Controls[i].Text != letter)
                    {
                        pLetters.Controls[i].Text = letter;
                        guessedLetters++;
                    }
                }

                Debug.WriteLine("GuessedLetters: " + guessedLetters);
                Debug.WriteLine("GuessedWord length: " + guessWord.Length);

                if (guessedLetters == guessWord.Length)
                {
                    Debug.WriteLine("You find the word!");
                    timer.Stop();
                    if (ExecuteCommand(eCommands.ISRECORD))
                    {
                        if (!ExecuteCommand(eCommands.SENDRECORD))
                        {
                            ShowMessageError(SERVER_ERROR_MSG);
                            return;
                        }

                        MessageBox.Show(this, YOU_HAVE_RECORD_MSG, "RECORD", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        ShowMessageError(NO_RECORD_MSG);
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
            isHanged = true;
            MessageBox.Show(this, YOU_LOST_MSG+$"The word was{guessWord}", "LOST", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Timer_OnTick(object sender, EventArgs e)
        {
            timer.Tag = (int)timer.Tag + 1;
            TimeSpan time = TimeSpan.FromSeconds((int)timer.Tag);
            Debug.WriteLine((int)timer.Tag);
            lblTime.Text = time.ToString("mm':'ss");
        }

        private void TsmiShowRecords_Click(object sender, EventArgs e)
        {
            if (!ExecuteCommand(eCommands.GETRECORDS))
            {
                ShowMessageError(SERVER_ERROR_MSG);
                return;
            }

            if (records.Count <= 0)
            {
                MessageBox.Show(this, NO_RECORDS, "Sorry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string showRecords = "";
            records.ForEach(record => showRecords += $"Name: {record.Name} Time: {record.Seconds} \n");
            MessageBox.Show(this, showRecords, "Records", MessageBoxButtons.OK);
        }

        private void TsmiAddWords_Click(object sender, EventArgs e)
        {
            SendWord sendWord = new SendWord();

            if (sendWord.ShowDialog() == DialogResult.OK)
            {
                words = sendWord.Words;

                if (!string.IsNullOrEmpty(words))
                {
                    if (ExecuteCommand(eCommands.SENDWORD))
                    {
                        MessageBox.Show(this, WORDS_SAVED, "Words", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        ShowMessageError(SERVER_ERROR_MSG);
                    }
                }

            }

        }
    }
}
