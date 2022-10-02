using System.Collections.Generic;

namespace _05_exercise
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Menu m = new Menu();
            int menu;
            string[] names = new string[] { "Perdigon", "Pegaso", "Troya", "Rocinante", "Spirit" };
            string[] menuOptions = new string[] { "Play", "Exit" };

            Race r = new Race(names);

            do
            {
                menu = m.SubMenu("Choose an option", true, menuOptions);

                if (menu == 0)
                {
                    r.Play();
                }

            } while (menu != menuOptions.Length - 1);

        }
    }
}