using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _07_exercise
{


    internal class Animation
    {


        private static readonly object l = new object();
        private bool finish = false;
        private bool firstBoot = true;
        private int cont = 0;
        private int win = 20;
        private bool playAnimation = true;
        public void Start()
        {
            Thread player1 = new Thread(() =>
            {
                int rand = 0;
                int randTime = 0;
                Random r = new Random();

                while (cont < 20 && cont > -20)
                {
                    lock (l)
                    {
                        if (cont < 20 && cont > -20)
                        {
                            rand = r.Next(1, 11);
                            randTime = r.Next(100, 100 * rand);
                            Console.CursorVisible = false;
                            clearConsoleLine(0, 0);
                            Console.SetCursorPosition(0, 0);
                            Console.Write($"{rand,2} Points:{cont}");

                            if (rand == 5 || rand == 7)
                            {
                                if (playAnimation)
                                {
                                    playAnimation = false;
                                    cont++;
                                    Console.Write(" +1");
                                }
                                else
                                {
                                    cont += 5;
                                    Console.Write(" +5");
                                }
                            }
                        }

                    }
                    firstBoot = false;
                    Thread.Sleep(randTime);
                }

                clearConsoleLine(0, 0);
                Console.SetCursorPosition(0, 0);
                Console.Write($"{rand,2} Points:{cont}");
            });
            player1.Start();

            Thread player2 = new Thread(() =>
            {
                int rand = 0;
                int randTime = 0;
                Random r = new Random();

                while (cont < 20 && cont > -20)
                {
                    lock (l)
                    {
                        if (cont < 20 && cont > -20)
                        {
                            rand = r.Next(1, 11);
                            randTime = r.Next(100, 100 * rand);
                            Console.CursorVisible = false;
                            clearConsoleLine(2, 0);
                            Console.SetCursorPosition(0, 2);
                            Console.Write($"{rand,2} Points:{cont}");
                            if (rand == 5 || rand == 7)
                            {
                                if (!playAnimation)
                                {
                                    playAnimation = true;
                                    cont--;
                                    Console.Write(" -1");
                                }
                                else
                                {
                                    if (!firstBoot)
                                    {
                                        cont += -5;
                                        Console.Write(" -5");
                                    }
                                }
                            }
                        }

                    }
                    firstBoot = false;
                    Thread.Sleep(randTime);
                }

                clearConsoleLine(2, 0);
                Console.SetCursorPosition(0, 2);
                Console.Write($"{rand,2} Points:{cont}");
            });
            player2.Start();


            Thread display = new Thread(animationRuntime);
            display.Start(1);

        }
        private void animationRuntime(Object index)
        {
            int cont = -1;
            string[] animation = { "|", "/", "-", "\\" };

            while (cont < 20 && cont > -20)
            {

                while (playAnimation)
                {
                    lock (l)
                    {

                        if (cont < 3)
                        {
                            cont++;
                        }
                        else
                        {
                            cont = 0;
                        }
                        Console.CursorVisible = false;
                        Console.SetCursorPosition(5, (int)index);
                        Console.WriteLine(animation[cont]);
                    }
                    Thread.Sleep(200);
                }
            }

        }

        private static void clearConsoleLine(int currentLineCursor, int margin)
        {
            Console.SetCursorPosition(0, currentLineCursor);
            Console.Write(new string(' ', Console.WindowWidth - margin));
        }
    }
}
