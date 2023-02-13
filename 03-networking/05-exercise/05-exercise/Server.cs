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
        private List<String> words;
        private List<Record> records;
        private const int PORT = 31416;
        private bool executeServer = true;
        private Socket serverSocket;
        private Socket clientSocket;
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
                    Debug.WriteLine($"Error on {nameof(SaveRecords)} null...");
                    return false;
                }

            }
            catch (IOException)
            {
                Debug.WriteLine($"Error on {nameof(SaveRecords)} reading...");
                records = new List<Record>()
                {
                    new Record("abc", 8)
                };
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
                clientSocket = serverSocket.Accept();
                new Thread(UserManagement).Start(clientSocket);
            }
        }

        private void UserManagement(object userSocket)
        {

            using (NetworkStream ns = new((Socket)userSocket))
            using (StreamReader sr = new(ns))
            using (StreamWriter sw = new(ns))
            {
                //sw.WriteLine("");
                //sw.Flush();


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
                                if (words.Count <= 0) return;

                                TrySendMessage(words.OrderBy(_ => new Random().Next()).ToList()[0], sw);
                                break;
                            case eCommands.SENDWORD:
                                if (responseSplit.Length != 2) return;

                                bool isWordAdded = false;

                                if (!words.Contains(responseSplit[1]))
                                {
                                    words.Add(responseSplit[1]);
                                    isWordAdded = SaveWords();
                                }

                                TrySendMessage(isWordAdded ? "OK" : "ERROR", sw);
                                break;
                            case eCommands.GETRECORDS:
                                if (records.Count <= 0) return;

                                string msgRecords = JsonSerializer.Serialize(records);
                                Debug.WriteLine("Records: " + msgRecords);
                                TrySendMessage(msgRecords, sw);


                                break;
                            case eCommands.SENDRECORD:
                                if (responseSplit.Length != 2) return;

                                    Debug.WriteLine(nameof(eCommands.SENDRECORD));

                                    bool isRecordAdded = false;

                                    Record? newRecord = JsonSerializer.Deserialize<Record>(responseSplit[1]);

                                    if (newRecord != null)
                                    {
                                        if (records.Count == 3)
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
                                break;
                            case eCommands.CLOSESERVER:
                                break;
                        }
                    }
                }
            }
            (userSocket as Socket).Close();

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
