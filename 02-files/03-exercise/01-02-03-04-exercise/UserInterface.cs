using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_02_03_04_exercise
{

    internal class UserInterface
    {

        private static void companyEarnings(IGoosePasta iGoosePasta)
        {
            Console.WriteLine("Win Pasta: " + iGoosePasta.WinPasta(1000));
        }

        public PeopleManager pm = new PeopleManager();

        private string[] typesPerson = { "Employee", "Executive" };
        private string[] mainMenu = { "Insert Person", "Delete Person in range", "Show all People", "Show a Person", "Exit" };
        private string[] yesNo = { "Yes", "No" };

        private string cutString(string value, int chars)
        {

            if (value.Length > chars)
            {

                return value.Substring(0, chars - 3) + "...";
            }

            return value;
        }

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

        private void resume()
        {
            Console.WriteLine($"{"Pulse ENTER to continue",20}");
            ConsoleKeyInfo keyPress = Console.ReadKey(intercept: true);

            while (keyPress.Key != ConsoleKey.Enter)
            {
                keyPress = Console.ReadKey(intercept: true);
            }
        }

        private int subMenu(string title, bool showIndex, params string[] options)
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

        public void Start()
        {
            int menu;
            int index;


            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();

            do
            {
                menu = subMenu("Choose an option:", true, mainMenu);
                Console.Clear();

                switch (menu)
                {
                    case 0:

                        index = subMenu("Select one", true, typesPerson);
                        createPerson(index);
                        resume();

                        break;
                    case 1:
                        //Aqui no cumple la especificacion para hacerlo mas chulo.
                        //Pero hay un metodo que se llama askMaxMin que no utilizo que cumple la especificación

                        removePerson();
                        resume();

                        break;
                    case 2:
                        if (pm.people.Count >= 1)
                        {
                            showAllPeople();
                        }
                        else
                        {
                            Console.WriteLine("Nothing to show");
                            resume();
                        }

                        break;
                    case 3:
                        if (pm.people.Count >= 1)
                        {
                            searchPerson();
                        }
                        else
                        {
                            Console.WriteLine("Nothing to show");
                        }

                        resume();
                        break;
                    case 4:
                        break;
                    default:
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine();
                        Console.WriteLine("+----------------+");
                        Console.WriteLine("| Invalid option |");
                        Console.WriteLine("+----------------+");
                        Console.WriteLine();
                        Console.ResetColor();
                        break;
                }
            } while (menu != 4);
        }

        private void searchPerson()
        {
            string findSurname = "";
            int findIdPerson = -1;



            Console.WriteLine("Insert the beginning of surname");
            findSurname = Console.ReadLine().ToLower();

            findIdPerson = pm.Position(findSurname);

            if (findIdPerson == -1)
            {
                Console.WriteLine("Person not found");
            }
            else
            {
                pm.people[findIdPerson].ShowValues();
                if (pm.people[findIdPerson].GetType() == typeof(Executive))
                {
                    companyEarnings((Executive)pm.people[findIdPerson]);
                }


            }
        }

        private void removePerson()
        {
            if (pm.people.Count >= 2)
            {
                bool error = false;
                string letter;
                int min;
                int max;

                string[] menuPeople;
                List<string> listMenu = new List<string>();

                //Add elements to List

                for (int i = 0; i < pm.people.Count; i++)
                {
                    letter = pm.people[i].GetType() == typeof(Executive) ? "D" : "E";
                    listMenu.Add($"|{i,-3}|{cutString(pm.people[i].Name, 10),-10}|{cutString(pm.people[i].Surname, 20),-20}|{letter,-10}");
                }

                //Ask min
                do
                {
                    if (error)
                    {
                        Console.WriteLine("ERROR: The min value can't be the last element");
                        resume();

                    }

                    error = false;

                    menuPeople = listMenu.ToArray();

                    min = subMenu("Select the minimum value to delete", false, menuPeople);

                    if (min >= menuPeople.Length - 1)
                    {
                        error = true;
                    }
                } while (error);

                //Ask max

                listMenu.RemoveRange(0, min + 1);

                menuPeople = listMenu.ToArray();

                max = subMenu("Select the maximun value to delete", false, menuPeople) + min + 1;

                checkDelete(min, max);

            }
            else
            {
                Console.WriteLine("You need almost 2 person to delete");
            }
        }

        private void checkDelete(int min, int max)
        {
            string letter;
            for (int i = min; i <= max; i++)
            {
                letter = pm.people[i].GetType() == typeof(Executive) ? "D" : "E";
                Console.WriteLine($"{i,3}. Name: {cutString(pm.people[i].Name, 10),10} Surname:{cutString(pm.people[i].Surname, 20),20} {letter} {pm.people[i].Age}");
            }

            int delete = subMenu("Do you really delete this elements?", true, yesNo);

            if (delete == 0)
            {
                bool correctDelete = pm.Delete(max, min);

                if (correctDelete)
                {
                    Console.WriteLine("Elements was delete correctly");
                }
                else
                {
                    Console.WriteLine("Elements wasn't delete");
                }
            }

        }

        private void createPerson(int index)
        {
            if (index == 0)
            {
                Employee employee = new Employee();
                employee.InsertValues();

                pm.people.Insert(pm.Position(employee.Age), employee);

            }
            else
            {
                Executive executive = new Executive();
                executive.InsertValues();
                pm.people.Insert(pm.Position(executive.Age), executive);
            }
        }

        private void showAllPeople(bool showResume = true)
        {
            string letter;
            string header = $"|{"i",-3}|{"Name",-10}|{"Surname",-20}|{"Letter",-10}|";

            Console.Write($"{header}\n+---+----------+--------------------+----------+\n");

            for (int i = 0; i < pm.people.Count; i++)
            {
                letter = pm.people[i].GetType() == typeof(Executive) ? "D" : "E";
                System.Console.WriteLine($"|{i,-3}|{cutString(pm.people[i].Name, 10),-10}|{cutString(pm.people[i].Surname, 20),-20}|{letter,-10}|");
            }

            if (showResume)
            {
                resume();
            }
        }

        private int askMaxMin(string sentence, int max, int min = 0)
        {
            int value;
            bool isWorking = true;

            do
            {
                Console.Clear();

                showAllPeople(false);


                if (!isWorking)
                {
                    Console.WriteLine("Insert a valid value");
                }

                Console.WriteLine($"\nInsert the {sentence} index to delete");

                isWorking = Int32.TryParse(Console.ReadLine(), out value);

                if (value < min || value >= max)
                {
                    isWorking = false;
                }


            } while (!isWorking);

            return value;
        }
    }

}
