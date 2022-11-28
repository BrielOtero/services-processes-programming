using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExServicios
{
	public delegate void MyDelegate();
	public class Drone
	{
		public readonly object l = new object();
		Random r = new Random();
		bool crossingHallway = false;
		bool pause = false;
		MyDelegate d;
		Thread drone1;
		Thread drone2;
		Thread control;
		bool[] pauses = {false, false};

		public Drone()
		{
			drone1 = new Thread(() => DroneMovement(1));
			drone2 = new Thread(() => DroneMovement(2));
			control = new Thread(Control);
			drone1.Start();
			drone2.Start();
			control.Start();
			drone1.Join();
			drone2.Join();
			control.Join();
		}

		public void RandomInfo()
		{
			Process[] processes = Process.GetProcesses();
			Process p = processes[r.Next(0, processes.Length + 1)];
			Console.SetCursorPosition(1, 10);
			Console.WriteLine(new string(' ', 1000));
			StreamWriter sw = new StreamWriter(Environment.GetEnvironmentVariable("APPDATA") + "\\randominfo.txt");
			Console.WriteLine("Name: {0}", p.ProcessName);
			Console.WriteLine("Modules:");
			sw.WriteLine("Name: {0}", p.ProcessName);
			sw.WriteLine("Modules:");
			for (int i = 0; i < 10; i++)
			{
				Console.WriteLine(p.Modules[i].ModuleName);
				sw.WriteLine(p.Modules[i].ModuleName);
			}
			sw.Close();
		}
		public void ExceptionControl(MyDelegate d)
		{
			try
			{
				d.Invoke();
			}
			catch (Exception e)
			{
				Console.WriteLine("Panic Error!!");
				Console.WriteLine(e.Message);
			}
		}

		public void DroneMovement(int nDron)
		{
			int wait = r.Next(100, 201);
			int mov = 0;
			bool reachDoor = false;
			bool reachOut = false;
			bool inside = false;
			while (!reachOut && !pauses[nDron - 1])
			{
				if (!pause)
				{
					lock (l)
					{
						if (crossingHallway && !inside && reachDoor)
						{
							Monitor.Wait(l);
						}

						if (!reachDoor)
						{
							Console.SetCursorPosition(mov, nDron);
							Console.Write(nDron.ToString());
							if (mov == 20)
							{
								reachDoor = true;
							}
							mov++;
						}

						if (mov > 20 && !crossingHallway)
						{
							crossingHallway = true;
							inside = true;
						}

						if (inside)
						{
							Console.SetCursorPosition(mov, nDron);
							Console.Write("*");
							mov++;
							if (mov == 40)
							{
								crossingHallway = false;
								reachOut = true;
								inside = false;
								Monitor.Pulse(l);
							}

						}
						Thread.Sleep(wait);
					}
				}
			}
		}

		private void Control()
		{
			while (true)
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
						pauses[0] = true;
						break;
					case '2': //finalize drone 2
						pauses[1] = true;
						break;
					case 'o': //control off
						control.Interrupt();
						break;
					case 'i': //system information
						ExceptionControl(d = RandomInfo);
						break;
				}
			}
		}
	}
}
