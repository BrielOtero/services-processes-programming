using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace _03_exercise
{
    internal class Server
    {
        private const int PORT = 31416;
        private IPEndPoint ie;
        private Socket serverSocket;
        private Socket clientSocket;


        private void StartServer()
        {
            ie = new(IPAddress.Any, PORT);


            using (serverSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                try
                {
                    serverSocket.Bind(ie);

                }
                catch (SocketException se)
                {

                    ProcessPortError

                }




            }
        }

        private bool ProcessPortError(IPEndPoint ie, Socket serverSocket)
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
                return false;
            }


            Debug.WriteLine($"Connected to {ie.Port}");
            return true;
        }
    }
}
