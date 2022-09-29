namespace _03_exercise
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int value = 0;
            do
            {

                new Thread(() =>
                {
                  

                        if (value != 1000)
                        {
                            value++;
                            Console.Clear();
                            Console.SetCursorPosition(0, 0);
                            Console.Write($"Thread 1 {value,5}");
                        }

                }).Start();

                new Thread(() =>
                {
                        if (value != -1000)
                        {
                            value--;
                            Console.Clear();
                            Console.SetCursorPosition(0, 0);
                            Console.Write($"Thread 2 {value,5}");
                        }

                }).Start();

            } while (value != -1000 || value != 1000);


            if (value == 1000)
            {
                Console.WriteLine("Thread 1 Win");

            }
            else
            {
                Console.WriteLine("Thread 2 Win");
            }




        }
    }
}