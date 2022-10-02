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

                lock (l)
                {
                    while (value != obj)
                    {
                        value++;
                        Console.SetCursorPosition(0, 0);
                        Console.Write($"Thread 1 -> {value,5}");
                    }
                    Monitor.Pulse(l);
                }


            }).Start();

            new Thread(() =>
            {
                lock (l)
                {
                    while (value != (obj * -1))
                    {
                        value--;
                        Console.SetCursorPosition(0, 0);
                        Console.Write($"Thread 2 -> {value,5}");
                    }

                    Monitor.Pulse(l);
                }


            }).Start();


            lock (l)
            {
                Monitor.Wait(l);
            }
            Console.Clear();
            Console.WriteLine($"{(value == obj ? "Thread 1" : "Thread 2")} WINS!!!!");

        }

    }
}