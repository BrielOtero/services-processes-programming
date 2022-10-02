using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_exercise
{
    internal class Menu
    {
        private void paintSubmenu(string[] options, int option, string title, bool showIndex)
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

        public int SubMenu(string title, bool showIndex, params string[] options)
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
    }
}
