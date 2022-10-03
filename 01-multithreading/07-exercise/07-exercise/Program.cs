namespace _07_exercise
{
    internal class Program
    {
        private delegate void Execution();
        private bool finish = false;
        static void Main(string[] args)
        {
            Thread player1= new Thread(runtime);
            Thread player2;
            Thread display;

        }

        private void runtime()
        {
            int rand;
            int randTime;
            Random r = new Random();
            while (!finish)
            {

                rand = r.Next(1, 11);
                randTime = r.Next(100, 100 * rand);
                
                Console.WriteLine(rand);

                Thread.Sleep(randTime);
            }
        }


    }
}