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
        private Socket serverSocket;
        private IPEndPoint ie;
        private readonly int PORT = 31416;
        private bool executeServer = true;
        private List<User> users = new List<User>();
        private List<int> numbers = new();

        private User? userWinner;
        private int thereAreWinner;


        public void Start()
        {
            for (int i = 1; i < 21; i++)
            {
                numbers.Add(i);
            }

            thereAreWinner = -1;
            userWinner = null;

            numbers = numbers.OrderBy(_ => new Random().Next()).ToList();

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
        }

        private void TimeManagement()
        {
            bool executeTimer = true;
            int timeInSeconds = 0;
            int timeToReach = 5;
            bool timerStart = false;
            int winnerNumber;

            while (executeTimer)
            {
                Thread.Sleep(1000);

                lock (l)
                {
                    if (users.Count >= 2)
                    {
                        timerStart = true;
                    }

                    if (timerStart)
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
                                user.IsWinner = true;
                                userWinner = user;
                            }
                            else
                            {
                                user.IsWinner = false;
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
            public bool IsWinner;

            public User(Socket userSocket) : this(userSocket, -1, null, true, false) { }
            public User(int luckyNumber) : this(null, luckyNumber, null, true, false) { }


            public User(Socket userSocket, int luckyNumber, IPAddress userIpAdress, bool isConnected, bool isWinner)
            {
                this.UserSocket = userSocket;
                this.LuckyNumber = luckyNumber;
                this.UserIpAddress = userIpAdress;
                this.IsConnected = isConnected;
                this.IsWinner = isWinner;
            }
        }
        private readonly object l = new();

        private void UserManagement(object clientSocket)
        {
            User user;
            bool isUserWaitingNotified = false;
            bool needMaxUsersNotified = false;


            lock (l)
            {
                user = new((Socket)clientSocket);

                if (numbers.Count != 0)
                {
                    user.LuckyNumber = numbers[0];
                    user.UserIpAddress = (user.UserSocket.RemoteEndPoint as IPEndPoint).Address;
                    numbers.RemoveAt(0);
                    users.Add(user);
                }
                else
                {
                    needMaxUsersNotified = true;
                }
            }

            using (NetworkStream ns = new(user.UserSocket))
            using (StreamWriter sw = new(ns))
            using (StreamReader sr = new(ns))
            {
                if (needMaxUsersNotified)
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
                                    TrySendMessage(user.IsWinner ? $"You WIN with number {user.LuckyNumber}" : $" The WINNER is {userWinner?.UserIpAddress.ToString()} with number {userWinner?.LuckyNumber}. Your number was {user.LuckyNumber}", sw);
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
            if (!needMaxUsersNotified)
            {
                lock (l)
                {
                    users.Remove(user);
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
            catch (SocketException)
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
