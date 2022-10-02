using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _05_exercise
{
    internal class Race
    {
        private int floorLength;
        private Horse[] horses;
        Thread[] threads;
        private bool winner;
        private int margin;
        private string[] names;

        public Race(string[] names)
        {
            this.names = names;
        }

        private static readonly object l = new object();
        private static readonly object mainLock = new object();
        public void Play()
        {
            Menu m = new Menu();
            margin = 15;
            floorLength = Console.WindowWidth - margin;

            threads = new Thread[names.Length];
            horses = new Horse[names.Length];

            winner = false;

            int userBet;

            userBet = m.SubMenu("Choose your WINNER", true, names);

            for (int i = 0; i < horses.Length; i++)
            {
                threads[i] = new Thread(horseMovement);
            }

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Start(i);
            }

            for (int i = 0; i < floorLength; i++)
            {
                Console.SetCursorPosition(i, horses.Length);
                Console.Write(" ");
                Console.SetCursorPosition(i, horses.Length + 1);
                Console.Write("-");
            }

            Console.SetCursorPosition(floorLength, horses.Length);
            Console.Write("^");
            Console.SetCursorPosition(floorLength, horses.Length + 1);
            Console.Write("|");

            for (int k = 0; k < names.Length; k++)
            {
                Console.SetCursorPosition(Console.WindowWidth - names[k].Length, k);
                Console.WriteLine(names[k]);
            }


            lock (mainLock)
            {
                Monitor.Wait(mainLock);
            }

            //Console.SetCursorPosition(0, horses.Length + 2);
            //Console.WriteLine("Winner" + winner);

            Console.SetCursorPosition(0, horses.Length + 3);
            int winnerIndex = Array.FindIndex(horses, (x) => x.position > floorLength - 1);
            Console.WriteLine($"The winner is {names[winnerIndex]}");

            Console.SetCursorPosition(0, horses.Length + 4);
            string message = userBet == winnerIndex ? "YOU WIN" : "YOU LOST";
            Console.WriteLine($"{message}!!");

            resume();
        }

        private void horseMovement(object x)
        {
            int i = (int)x;
            horses[i] = new Horse(i);

            while (horses[i].position < floorLength && !winner)
            {
                lock (mainLock)
                {
                    lock (l)
                    {
                        if (!winner)
                        {
                            horses[i].Run();
                            clearConsoleLine(horses[i].line, margin);
                            Console.SetCursorPosition(horses[i].position, horses[i].line);
                            Console.Write("*");
                        }

                        if (horses[i].position >= floorLength)
                        {
                            winner = true;
                            Monitor.Pulse(mainLock);
                        }

                        Monitor.Pulse(l);
                    }
                }
            }
        }

        private static void resume()
        {
            Console.WriteLine("Pulse ENTER to continue...");

            ConsoleKeyInfo keyPress = Console.ReadKey(true);

            while (keyPress.Key != ConsoleKey.Enter)
            {
                keyPress = Console.ReadKey(true);
            }
        }
        private static void clearConsoleLine(int currentLineCursor, int margin)
        {
            Console.SetCursorPosition(0, currentLineCursor);
            Console.Write(new string(' ', Console.WindowWidth - margin));
        }
    }
}
