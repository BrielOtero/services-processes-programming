namespace _02_exercise
{
    internal class Program
    {

        public delegate void MyDelegate();

        public static void MenuGenerator(string[] options, MyDelegate[] myDelegates)
        {
            bool correct = true;
            int menu;
            int exitValue = options.Length;

            if (options != null || myDelegates != null || options.Length != myDelegates.Length)
            {
                Console.WriteLine("Menu not valid");
                return;
            }

            do
            {

                if (!correct)
                {
                    Console.WriteLine();
                    Console.WriteLine("+----------------+");
                    Console.WriteLine("| Invalid option |");
                    Console.WriteLine("+----------------+");
                    Console.WriteLine();
                }

                Console.WriteLine("Choose an option: ");
                Console.WriteLine();

                for (int i = 0; i < options.Length; i++)
                {
                    Console.WriteLine(i + ". " + options[i]);
                }

                Console.WriteLine(exitValue + ". Exit");

                correct = Int32.TryParse(Console.ReadLine(), out menu);
                Console.WriteLine();

                if (!correct)
                {
                    menu = -1;
                }

                if (correct && menu != exitValue)
                {
                    if (menu < options.Length)
                    {
                        myDelegates[menu]();

                    }
                    else
                    {
                        correct = false;
                    }
                }

            } while (menu != exitValue);

        }

        static void f1()
        {
            Console.WriteLine("A");
        }
        static void f2()
        {
            Console.WriteLine("B");
        }
        static void f3()
        {
            Console.WriteLine("C");
        }
        static void Main(string[] args)
        {
            MenuGenerator(new string[] { "Op1", "Op2", "Op3", "op4" }, new MyDelegate[] { () => Console.WriteLine("A"), () => Console.WriteLine("B"), () => Console.WriteLine("C"), /*() => Console.WriteLine("D") */});
        }

    }
}