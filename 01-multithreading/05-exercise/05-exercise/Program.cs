namespace _05_exercise
{
    internal class Program
    {
        static private void paintSubmenu(string[] options, int option, string title, bool showIndex)
        {

            Console.Clear();
            Console.WriteLine($"\n{title,-30}\n\n");
            for (int i = 0; i < options.Length; i++)
            {
                if (i == option)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                if (showIndex)
                {
                    Console.WriteLine($"{i}. {options[i],-30}");

                }
                else
                {
                    Console.WriteLine($"{options[i],-30}");
                }
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;

            }
        }

        static private int subMenu(string title, bool showIndex, params string[] options)
        {
            bool exit = false;

            int option = 0;
            int select = -1;

            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            paintSubmenu(options, option, title, showIndex);
            Console.WriteLine($"{"Pulse ENTER to select",-30}");

            do
            {
                ConsoleKeyInfo tecla = Console.ReadKey();
                switch (tecla.Key)
                {
                    case ConsoleKey.DownArrow:
                        option = option < options.Length - 1 ? option + 1 : option;
                        break;
                    case ConsoleKey.UpArrow:
                        option = option > 0 ? option - 1 : option;
                        break;
                    case ConsoleKey.Enter:

                        select = option;
                        exit = true;
                        break;
                }
                paintSubmenu(options, option, title, showIndex);
                Console.WriteLine($"{"Pulse ENTER to select",-30}");

            } while (!exit);

            Console.Clear();

            return select;
        }
        private static readonly object l = new object();
        static void Main(string[] args)
        {
            //int menu;
            //menu = subMenu("Choose your WINNER", true, new string[] { "Perdigon", "Pegaso", "Troya", "Rocinante", "Spirit" });
            Thread[] threads = new Thread[5];


            new Thread(() =>
            {
                Horse h = new Horse();
                while (h.position < 15)
                {

                    h.Run();
                    Console.SetCursorPosition(h.position, 0);
                    Console.Write("*");
                }
                lock (l)
                {
                    Monitor.Pulse(l);
                }
            }).Start();

            new Thread(() =>
            {
                Horse h = new Horse();
                while (h.position < 15)
                {

                    h.Run();
                    Console.SetCursorPosition(h.position, 1);
                    Console.Write("*");
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



        }
    }
}