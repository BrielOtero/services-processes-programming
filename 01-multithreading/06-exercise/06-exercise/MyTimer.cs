using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _06_exercise
{

    public delegate void Execution(Object a);


    internal class MyTimer
    {
        private static readonly object l = new object();
        public int interval;
        private Thread t;
        Action exe;


        public MyTimer(Action a)
        {
            exe = a;
            t = new Thread(a.Invoke);
        }

        public void Pause()
        {
            lock (l)
            {
                Monitor.Wait(l);
            }
        }

        public void Run()
        {
            t.Start();

            lock (l)
            {
                do
                {
                    exe.Invoke();
                    Thread.Sleep(interval);
                    Monitor.Pulse(l);
                } while (true);
            }

        }




    }






}
