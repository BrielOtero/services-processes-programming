using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace _01_exercise
{
    internal class Program
    {

        static void Main(string[] args)
        {

            Server server = new Server();
            server.StartServer();

        }

    }
}