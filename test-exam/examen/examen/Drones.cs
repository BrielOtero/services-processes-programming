using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace examen
{
    internal class Drones
    {
        public delegate void MyDelegate();
        private bool inDoor = false;
        private bool pause = false;
        private bool[] doorReaches = { false, false };
        public readonly object l = new object();

        public Drones()
        {
            new Thread(() => DronMovements(1)).Start();
            new Thread(() => DronMovements(2)).Start();
            new Thread(() => Control()).Start();
        }
        public void RandomInfo()
        {
            Process[] processes = Process.GetProcesses();
            Process randomProcess = processes[new Random().Next(processes.Length)];
            string text = "";
            Console.SetCursorPosition(1, 10);
            WriteSpaces(1000);
            Console.SetCursorPosition(1, 10);
            text = $"Process: {randomProcess.ProcessName}{Environment.NewLine}";
            Console.Write(text);
            for (int i = 0; i < randomProcess.Modules.Count; i++)
            {
                if (i == 10)
                {
                    break;
                }
                Console.WriteLine($"Module {i}: {randomProcess.Modules[i].ModuleName}");
                text += $"Module {i + 1}: {randomProcess.Modules[i].ModuleName}{Environment.NewLine}";
            }
            File.WriteAllText($"{Environment.GetEnvironmentVariable("userprofile")}\\info.txt", text);
        }

        public void WriteSpaces(int spaces)
        {
            if (spaces == 0)
            {
                return;
            }
            Console.Write(" ");
            WriteSpaces(spaces - 1);
        }

        public void ExceptionControl(MyDelegate myDelegate)
        {
            try
            {
                myDelegate.Invoke();
            }
            catch (Exception e)
            {
                Console.WriteLine("Panic Error!! ");
                Console.WriteLine(e.Message);
            }
        }

        public void DronMovements(int dronNumber)
        {
            int col = 0;
            bool reachDoor = false;
            while (!reachDoor && !doorReaches[dronNumber - 1])
            {
                if (!pause)
                {
                    if (col < 20)
                    {
                        Thread.Sleep(new Random().Next(100, 201));
                    }
                    lock (l)
                    {
                        Console.SetCursorPosition(col, dronNumber);
                        Console.Write(dronNumber);
                        col++;
                        if (col == 20)
                        {
                            reachDoor = true;
                            doorReaches[dronNumber - 1] = true;
                        }
                    }
                }
            }
            lock (l)
            {
                if (inDoor)
                {
                    Monitor.Wait(l);
                }
                else
                {
                    inDoor = true;
                }
            }
            bool despegar = false;
            while (!despegar)
            {
                Thread.Sleep(100);
                lock (l)
                {
                    col++;
                    Console.SetCursorPosition(col, dronNumber);
                    Console.Write("*");
                    if (col == 30)
                    {
                        despegar = true;
                        Monitor.Pulse(l);
                    }
                }
            }
        }

        public void Control()
        {
            bool exit = false;
            while (!exit)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.KeyChar)
                {
                    case 'p': //pause drones
                        pause = true;
                        break;
                    case 'c': //continue drones
                        pause = false;
                        break;
                    case '1': //finalize drone 1
                        doorReaches[0] = true;
                        break;
                    case '2': //finalize drone 2
                        doorReaches[1] = true;
                        break;
                    case 'o': //control off
                        exit = true;
                        break;
                    case 'i': //system information
                        pause = true;
                        ExceptionControl(RandomInfo);
                        pause = false;
                        break;
                }
            }
        }
    }
}
