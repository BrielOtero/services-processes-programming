using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_exam
{
    internal class Drones
    {

        public delegate void MyDelegate();
        readonly private object console = new object();
        readonly private object room = new object();

        private bool intoTheRoom = false;
        private bool pause = false;
        bool controlOn = true;
        private bool[] isFlying;

        public Drones()
        {
            isFlying = new bool[2];
            isFlying[0] = true;
            isFlying[1] = true;

            Thread threadDrone1 = new Thread(() => gestionDrones(1));
            Thread threadDrone2 = new Thread(() => gestionDrones(2));
            Thread threadControl = new Thread(control);

            threadDrone1.Start();
            threadDrone2.Start();
            threadControl.Start();
        }

        public void gestionDrones(int dronAndRow)
        {
            int randSleep = new Random().Next(100, 201);
            int posY = dronAndRow;
            int posX = 0;

            while (posX <= 20 && isFlying[dronAndRow - 1])
            {
                lock (console)
                {
                    if (isFlying[dronAndRow - 1])
                    {
                        if (pause)
                        {
                            Monitor.Wait(console);
                        }
                        Console.SetCursorPosition(posX, posY);
                        Console.Write(dronAndRow);
                        posX++;
                    }
                }
                Thread.Sleep(randSleep);
            }

            lock (console)
            {
                if (intoTheRoom && isFlying[dronAndRow - 1])
                {
                    Monitor.Wait(console);
                }
                else
                {
                    intoTheRoom = true;
                }
            }

            while (posX <= 30 && isFlying[dronAndRow - 1])
            {
                lock (console)
                {

                    Console.SetCursorPosition(posX, posY);
                    Console.Write("*");

                    posX++;

                    if (posX>30)
                    {
                        Monitor.Pulse(console);
                    }
                }
                Thread.Sleep(randSleep);
            }
            intoTheRoom = false;
            isFlying[dronAndRow - 1] = true;
        }

        public void control()
        {
            while (controlOn)
            {
                if (Console.KeyAvailable) //if there’s a key in keyboard’s buffer
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    switch (key.KeyChar)
                    {
                        case 'p': //pause drones
                            lock (console)
                            {
                                pause = true;
                            }

                            break;
                        case 'c': //continue drones
                            lock (console)
                            {
                                pause = false;
                                Monitor.PulseAll(console);
                            }
                            break;
                        case '1': //finalize drone 1
                            lock (console)
                            {
                                isFlying[0] = false;
                            }
                            break;
                        case '2': //finalize drone 2
                            lock (console)
                            {
                                isFlying[1] = false;
                            }
                            break;
                        case 'o': //control off
                            controlOn = false;
                            break;
                        case 'i': //system information
                            lock (console)
                            {
                                ExceptionControl(RandomInfo);
                            }
                            break;
                    }
                }
            }
        }

        public void RandomInfo()
        {
            Random r;
            Process[] processes;
            int randomProccessIndex;
            Process rp;


            processes = Process.GetProcesses();
            r = new Random();
            randomProccessIndex = r.Next(processes.Length);
            rp = processes[randomProccessIndex];


            Console.SetCursorPosition(1, 10);
            Console.Write(new string(' ', 1000));
            Console.SetCursorPosition(1, 10);
            Console.WriteLine($"Name: {rp.ProcessName}");

            if (rp.Modules.Count > 0)
            {
                using (StreamWriter fw = new StreamWriter(Environment.GetEnvironmentVariable("USERPROFILE") + Path.DirectorySeparatorChar + "randominfo.txt"))
                {
                    Console.WriteLine("Modules: ");
                    int modulesLength = rp.Modules.Count <= 10 ? rp.Modules.Count : 10;

                    for (int i = 0; i < modulesLength; i++)
                    {
                        Console.WriteLine($"{rp.Modules[i].ModuleName}");
                        fw.Write($"{rp.Modules[i].ModuleName}");
                        if (i < modulesLength - 1)
                        {
                            fw.Write(',');
                        }
                    }
                }
            }
        }


        public void ExceptionControl(MyDelegate mydelegate)
        {
            try
            {
                mydelegate();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Panic Error!! {e.Message}");
            }
        }
    }

}
