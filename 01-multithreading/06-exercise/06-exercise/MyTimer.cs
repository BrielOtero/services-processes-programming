using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _06_exercise
{

    public delegate void Execution();


    internal class MyTimer
    {
        private static readonly object l = new object();
        public int interval;
        private Thread t;
        private bool finish = true;
        private Execution exe;

        public MyTimer(Execution exe)
        {
            this.exe = exe;
            t = new Thread(Runtime);
            t.IsBackground = true;
            t.Start();
        }
        public void Runtime()
        {
            lock (l)
            {
                if (finish)
                {
                    Monitor.Wait(l);
                }
            }


            while (!finish)
            {
                exe.Invoke();
                Thread.Sleep(interval);
            }
            Runtime();
        }
        public void Run()
        {

            lock (l)
            {
                finish = false;
                Monitor.Pulse(l);
            }
        }

        public void Pause()
        {
            lock (l)
            {
                finish = true;
            }
        }




    }






}
