
 
using System;
using System.Diagnostics;
using System.Threading;

namespace _01_exam
{
    public delegate void ExceptionDelegate();

    class Curro
    {
        public Random g = new Random();
        public bool controlOn = true;            // Keys control
        public readonly object l = new object(); // Console and flags lock
        public readonly object o = new object(); // Gate lock
        public bool pause = false;               // Drones pause control
        public bool[] flying = { true, true };   // Drones flying control

        public Curro()
        {
            Thread d1 = new Thread(() => Drone(1));
            Thread d2 = new Thread(() => Drone(2));
            Thread c = new Thread(Control);
            d1.Start();
            d2.Start();
            c.Start();
        }


        public void Drone(int number)
        {
            int posX = 1;
            int posY = number;
            int time = g.Next(100, 200);

            while (flying[number - 1])
            {
                // Movement before gate
               lock (l)
                {
                    if (flying[number - 1])
                    {
                        if (pause)
                        {
                            Monitor.Wait(l);
                        }
                        Console.SetCursorPosition(posX, posY);
                        Console.WriteLine($"{number}"); // Yes, another way to use Format since C#6 ;-)

                        posX++;
                    }
                }
                // Movement on gate
                if (posX > 20)
                {
                    lock (o) // lock all loop with other object
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            lock (l) // lock de Console
                            {
                                Console.SetCursorPosition(posX + i, posY);
                                Console.WriteLine("*");
                            }
                            Thread.Sleep(time);
                        }

                        flying[number - 1] = false;
                    }
                }
                Thread.Sleep(time); //Outside lock!!!
            }
        }
        

        public void Control()
        {
            while (controlOn)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    switch (key.KeyChar)
                    {
                        case 'p': //pause drones
                            lock (l)
                            {
                                pause = true;
                            }
                            break;
                        case 'c': //continue drones
                            lock (l)
                            {
                                pause = false;
                                Monitor.Pulse(l); // or PulseAll
                                Monitor.Pulse(l);
                            }
                            break;
                        case '1': //Finalize drone 1
                            lock (l)
                                flying[0] = false;
                            break;
                        case '2': //Finalize drone 2
                            lock (l)
                                flying[1] = false;
                            break;
                        case 'o': //control off
                            controlOn = false; //Don't need lock!
                            break;
                        case 'i': //Information system
                            lock (l) //Lock the Console
                            {
                                ExceptionControl(RandomInfo);
                            }
                            break;

                    }

                }
            }
        }


        public void ExceptionControl(ExceptionDelegate d)
        {
            try
            {
                d();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Panic Error!!! {e.Message}"); //Cool, doesn't it? 
            }
        }


        public void RandomInfo()
        {
            Process[] ps = Process.GetProcesses();
            Process p = ps[g.Next(0, ps.Length)];
            Console.SetCursorPosition(1, 10);
            Console.Write("{0,1000}", "");
            Console.SetCursorPosition(1, 10);
            Console.WriteLine(p.ProcessName);

            for (int i = 0; i < Math.Min(p.Modules.Count, 10); i++)
            {
                Console.Write($"{p.Modules[i].ModuleName} ");
            }
        }

    } // End class Drones

}
