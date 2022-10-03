namespace _04_exercise
{
    internal class Program
    {
        static readonly private object l = new object();
        static int value = 0;
        static int obj = 1000;
        static void Main(string[] args)
        {

            new Thread(() =>
            {

                while (value < obj && value > -obj)
                {
                    lock (l)
                    {
                        if (value < obj && value > -obj)
                        {
                            value++;
                            Console.SetCursorPosition(0, 0);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Thread 1 -> {value,5}");
                        }

                    }
                }


                lock (l)
                {
                    Monitor.Pulse(l);
                }


            }).Start();

            new Thread(() =>
            {

                while (value < obj && value > -obj)
                {
                    lock (l)
                    {
                        if (value < obj && value > -obj)
                        {

                            value--;
                            Console.SetCursorPosition(0, 0);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"Thread 2 -> {value,5}");
                        }
                    }
                }

                lock (l)
                {
                    Monitor.Pulse(l);

                }

            }).Start();


            lock (l)
            {
                Monitor.Wait(l);
            }

            Console.WriteLine($"{(value == obj ? "Thread 1" : "Thread 2")} WINS!!!!");
            Console.ReadKey();

        }

    }
}