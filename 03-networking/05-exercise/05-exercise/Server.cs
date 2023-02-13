using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _05_exercise
{
    internal class Server
    {
        private readonly string PATH_WORD = Path.Combine(Environment.GetEnvironmentVariable("appdata"), "words.txt");
        private readonly string PATH_RECORDS = Path.Combine(Environment.GetEnvironmentVariable("appdata"), "records.bin");
        private readonly object l = new object();
        private List<String> words;
        private List<Record> records;
        private const int PORT = 31416;
        private bool executeServer = true;
        private Socket serverSocket;
        private enum eCommands
        {
            GETWORD, SENDWORD, GETRECORDS, SENDRECORD, CLOSESERVER
        }

        private bool ReadWords()
        {
            try
            {
                string fileContent = File.ReadAllText(PATH_WORD).ToUpper();
                words = fileContent.Split(",").ToList();
                if (string.IsNullOrEmpty(words[words.Count - 1]))
                {
                    words.RemoveAt(words.Count - 1);
                }
            }
            catch (IOException)
            {
                Debug.WriteLine($"Error on {nameof(ReadWords)} reading words");
                words = new List<string>();
                return false;
            }
            return true;
        }

        private bool SaveWords()
        {
            try
            {
                string fileContent = "";
                words.ForEach(word => fileContent += word + ",");
                File.WriteAllText(PATH_WORD, fileContent);
            }
            catch (IOException)
            {
                Debug.WriteLine($"Error on {nameof(SaveWords)} saving words");
                return false;
            }
            return true;
        }

        private bool ReadRecords()
        {

            try
            {
                string fileContent = Encoding.UTF8.GetString(File.ReadAllBytes(PATH_RECORDS));

                List<Record>? tempRecords = JsonSerializer.Deserialize<List<Record>>(fileContent);

                if (tempRecords != null)
                {
                    records = tempRecords;
                }
                else
                {
                    records = new List<Record>();
                    Debug.WriteLine($"Error on {nameof(SaveRecords)} null...");
                    return false;
                }

            }
            catch (IOException)
            {
                Debug.WriteLine($"Error on {nameof(SaveRecords)} reading...");
                records = new List<Record>();

                return false;
            }
            return true;
        }

        private bool SaveRecords()
        {

            try
            {
                byte[] fileContentBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(records));
                File.WriteAllBytes(PATH_RECORDS, fileContentBytes);
            }
            catch (IOException)
            {
                Debug.WriteLine($"Error on {nameof(SaveRecords)} saving...");
                return false;
            }
            return true;
        }


        public void Start()
        {
            IPEndPoint ie = new(IPAddress.Any, PORT);

            serverSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                serverSocket.Bind(ie);
            }
            catch (Exception)
            {
                Debug.WriteLine($"Port {ie.Port} in use!");

                if (!TryToBindPort(ie))
                {
                    return;
                }
            }

            serverSocket.Listen(20);

            ReadWords();
            ReadRecords();
            WaitForUserConnection();
        }

        private bool TryToBindPort(IPEndPoint ie)
        {
            bool isBinded = false;
            for (int i = 0; i < IPEndPoint.MaxPort; i++)
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
                    Debug.WriteLine($"Port {ie.Port} in use!");
                }
            }

            if (isBinded)
            {
                Debug.WriteLine($"Port {ie.Port} binded correctly!");
            }
            else
            {
                Debug.WriteLine($"Error binded. All ports in use");
            }

            return isBinded;
        }

        private void WaitForUserConnection()
        {
            while (executeServer)
            {
                try
                {
                    Socket clientSocket = serverSocket.Accept();
                    Thread thread = new Thread(UserManagement);
                    thread.IsBackground = true;
                    thread.Start(clientSocket);
                }
                catch (Exception)
                {
                    Debug.WriteLine("Server socket exception");
                }

            }
            serverSocket.Close();
        }

        private void UserManagement(object userSocket)
        {

            using (NetworkStream ns = new((Socket)userSocket))
            using (StreamReader sr = new(ns))
            using (StreamWriter sw = new(ns))
            {

                if (TryGetMessage(out string response, sr))
                {

                    string[] responseSplit = response.Split(" ");
                    response = responseSplit[0].ToUpper();
                    Debug.WriteLine("Command: " + response);

                    if (Enum.TryParse(typeof(eCommands), response, out object command))
                    {
                        switch (command)
                        {
                            case eCommands.GETWORD:
                                lock (l)
                                {
                                    if (words.Count <= 0) return;

                                    TrySendMessage(words.OrderBy(_ => new Random().Next()).ToList()[0], sw);
                                }

                                break;
                            case eCommands.SENDWORD:
                                if (responseSplit.Length != 2) return;

                                lock (l)
                                {
                                    SendWord(sw, responseSplit);
                                }
                                break;
                            case eCommands.GETRECORDS:
                                lock (l)
                                {
                                    string msgRecords = JsonSerializer.Serialize(records);
                                    Debug.WriteLine("Records: " + msgRecords);
                                    TrySendMessage(msgRecords, sw);
                                }

                                break;
                            case eCommands.SENDRECORD:
                                if (responseSplit.Length != 2) return;

                                lock (l)
                                {
                                    SendRecord(sw, responseSplit);
                                }
                                break;
                            case eCommands.CLOSESERVER:
                                lock (l)
                                {
                                    executeServer = false;
                                    serverSocket.Close();
                                }
                                break;
                        }
                    }
                }
            }
            (userSocket as Socket).Close();

        }

        private void SendWord(StreamWriter sw, string[] responseSplit)
        {
            bool isWordAdded = false;

            try
            {
                List<string> tempWords = responseSplit[1].ToUpper().Split(",").ToList();

                if (string.IsNullOrEmpty(tempWords[tempWords.Count - 1]))
                {
                    tempWords.RemoveAt(words.Count - 1);
                }

                tempWords.ForEach(word =>
                {
                    if (!words.Contains(word))
                    {
                        words.Add(word);
                    }
                });

                isWordAdded = SaveWords();

            }
            catch (IOException)
            {
                Debug.WriteLine($"Error on {nameof(ReadWords)} reading words");
                words = new List<string>();
            }

            TrySendMessage(isWordAdded ? "OK" : "ERROR", sw);
        }

        private void SendRecord(StreamWriter sw, string[] responseSplit)
        {
            Debug.WriteLine(nameof(eCommands.SENDRECORD));

            bool isRecordAdded = false;

            Record? newRecord = JsonSerializer.Deserialize<Record>(responseSplit[1]);

            if (newRecord != null)
            {
                if (records.Count >= 3)
                {
                    Record maxRecord = records.Max();

                    if (newRecord.Seconds < maxRecord.Seconds)
                    {
                        records.Remove(maxRecord);
                        records.Add(newRecord);
                        isRecordAdded = SaveRecords();
                    }
                }
                else
                {
                    records.Add(newRecord);
                    isRecordAdded = SaveRecords();
                }
            }

            TrySendMessage(isRecordAdded ? "ACCEPT" : "REJECT", sw);
        }

        private bool TryGetMessage(out string response, StreamReader sr)
        {
            try
            {
                response = sr.ReadLine();
                return true;
            }
            catch (IOException)
            {
                Debug.WriteLine("Error getting message");
                response = "";
                return false;
            }
        }

        private bool TrySendMessage(string message, StreamWriter sw)
        {
            try
            {
                sw.WriteLine(message);
                sw.Flush();
            }
            catch (IOException)
            {
                Debug.WriteLine($"Error on {nameof(TrySendMessage)} sending message");
                return false;
            }
            return true;
        }

    }

}
