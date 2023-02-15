using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace _06_exercise
{
    internal class Server
    {
        private readonly int PORT = 31416;
        private Socket serverSocket;
        private IPEndPoint ie;
        private bool executeServer = true;
        private bool isGameAlive = true;

        private List<User> users = new List<User>();
        private List<int> numbers = new();

        private User? userWinner;
        private int thereAreWinner;
        //private bool maxUsersReached;


        public void Start()
        {
            ie = new(IPAddress.Any, PORT);

            serverSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                serverSocket.Bind(ie);
            }
            catch (SocketException)
            {
                Debug.WriteLine($"Port {ie.Port} in use!");

                if (!TryToBindPort()) return;
            }

            serverSocket.Listen(10);

            WaitForUserConnection();

        }

        private void WaitForUserConnection()
        {
            while (isGameAlive)
            {
                ResetGame();
                Thread timeThread = new(TimeManagement);
                timeThread.IsBackground = true;
                timeThread.Start();

                while (executeServer)
                {
                    try
                    {
                        Socket clientSocket = serverSocket.Accept();
                        Thread thread = new(UserManagement);
                        thread.IsBackground = true;
                        thread.Start(clientSocket);
                    }
                    catch (SocketException se)
                    {
                        Debug.WriteLine(se.Message);
                    }
                }
                users.ForEach(user =>
                {
                    user.UserSocket.Close();
                });
            }

        }
        private void ResetGame()
        {
            lock (l)
            {
                numbers.Clear();
                users.Clear();

                for (int i = 1; i < 21; i++)
                {
                    numbers.Add(i);
                }
                thereAreWinner = -1;
                userWinner = null;
                numbers = numbers.OrderBy(_ => new Random().Next()).ToList();
                executeServer = true;
            }
        }

        private void TimeManagement()
        {
            bool executeTimer = true;
            int timeInSeconds = 0;
            int timeToReach = 30;
            bool isTimerStarted = false;
            int winnerNumber;

            while (executeTimer)
            {
                Thread.Sleep(1000);

                lock (l)
                {
                    if (users.Count >= 2)
                    {
                        isTimerStarted = true;
                    }

                    if (isTimerStarted)
                    {
                        timeInSeconds++;
                        TrySendMessageToAll(TimeSpan.FromSeconds(timeInSeconds).ToString("mm':'ss"));
                    }

                    if (timeInSeconds >= timeToReach)
                    {
                        winnerNumber = new Random().Next(1, 20);
                        bool areWinner = false;

                        users.ForEach(user =>
                        {
                            if (user.LuckyNumber == winnerNumber)
                            {
                                userWinner = user;
                            }
                        });

                        if (userWinner != null)
                        {
                            thereAreWinner = 0;
                        }
                        else
                        {
                            thereAreWinner = 1;
                            userWinner = new(winnerNumber);
                        }

                        Debug.WriteLine("Winner number: " + winnerNumber);
                        executeTimer = false;
                        executeServer = false;

                    }
                }
            }
        }

        private struct User
        {
            public Socket UserSocket;
            public IPAddress UserIpAddress;
            public bool IsConnected;
            public int LuckyNumber;

            public User(Socket userSocket) : this(userSocket, -1, null, true) { }
            public User(int luckyNumber) : this(null, luckyNumber, null, true) { }


            public User(Socket userSocket, int luckyNumber, IPAddress userIpAddress, bool isConnected)
            {
                this.UserSocket = userSocket;
                this.LuckyNumber = luckyNumber;
                this.UserIpAddress = userIpAddress;
                this.IsConnected = isConnected;
            }
        }
        private readonly object l = new();

        private void UserManagement(object clientSocket)
        {
            User user;
            bool isUserWaitingNotified = false;
            bool maxUsersReached = false;

            lock (l)
            {
                user = new((Socket)clientSocket);

                Debug.WriteLine(numbers.Count);
                if (numbers.Count != 0)
                {
                    maxUsersReached = false;
                    user.LuckyNumber = numbers[0];
                    user.UserIpAddress = (user.UserSocket.RemoteEndPoint as IPEndPoint).Address;
                    numbers.RemoveAt(0);
                    users.Add(user);
                }
                else
                {
                    maxUsersReached = true;
                }
            }

            using (NetworkStream ns = new(user.UserSocket))
            using (StreamWriter sw = new(ns))
            using (StreamReader sr = new(ns))
            {
                if (maxUsersReached)
                {
                    TrySendMessage("MAX USERS REACHED", sw);
                }
                else
                {

                    while (user.IsConnected)
                    {
                        if (!isUserWaitingNotified)
                        {
                            lock (l)
                            {
                                if (users.Count < 2)
                                {
                                    TrySendMessage("Waiting for more users", sw);
                                    isUserWaitingNotified = true;
                                }
                            }
                        }


                        lock (l)
                        {
                            switch (thereAreWinner)
                            {
                                case 0:
                                    string winnerMessage = $"You WIN with number {user.LuckyNumber}";
                                    string lostMessage = $" The WINNER is {userWinner?.UserIpAddress.ToString()} with number {userWinner?.LuckyNumber}. Your number was {user.LuckyNumber}";
                                    TrySendMessage(user.LuckyNumber == userWinner?.LuckyNumber ? winnerMessage : lostMessage, sw);
                                    user.IsConnected = false;
                                    break;
                                case 1:
                                    TrySendMessage($" NOTHING WIN. The number is {userWinner?.LuckyNumber}. Your number was {user.LuckyNumber}", sw);
                                    user.IsConnected = false;
                                    break;
                            }
                        }
                    }
                }

            }
            user.UserSocket.Close();

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
                Debug.WriteLine($"ERROR: TrySendMessage {message}");
                return false;
            }
            return true;
        }


        private bool TrySendMessageToAll(string message)
        {

            foreach (var user in users)
            {
                if (user.IsConnected)
                {
                    try
                    {
                        using (NetworkStream ns = new(user.UserSocket))
                        using (StreamWriter sw = new(ns))
                        {

                            sw.WriteLine(message);
                        }
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine($"ERROR: TrySendMessageAll {message}");
                    }
                }
            }
            return false;
        }

        private bool TryToBindPort()
        {
            bool isBinded = false;

            for (int i = IPEndPoint.MinPort; i < IPEndPoint.MaxPort; i++)
            {
                try
                {
                    ie.Port = i;
                    serverSocket.Bind(ie);
                    isBinded = true;
                }
                catch (Exception)
                {
                    Debug.WriteLine($"Port {ie.Port} in use!");
                }
            }

            if (!isBinded)
            {
                Console.WriteLine("PORT BIND ERROR");
                return false;
            }

            return true;
        }

    }
}
