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
        private int cont = 0;
        private int win = 20;
        public void Start()
        {
            Thread player1 = new Thread(runtime);
            Thread player2 = new Thread(runtime);
            Thread display = new Thread(animationRuntime);


            //player1.Start(0);
            //player2.Start(1);
            display.Start(3);

        }
        private void animationRuntime(Object index)
        {
            int cont = -1;
            string[] animation = { "|", "/", "-", "\\" };


            while (!finish)
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
                Console.SetCursorPosition(0, (int)index);
                Console.Write(animation[cont]);
                Thread.Sleep(200);
            }
        }
        private void runtime(object index)
        {
            int rand;
            int randTime;
            Random r = new Random();

            lock (l)
            {
                while (!finish)
                {
                    rand = r.Next(1, 11);
                    randTime = r.Next(100, 100 * rand);
                    Console.CursorVisible = false;
                    Console.SetCursorPosition(0, (int)index);
                    Console.Write(rand);
                    Thread.Sleep(randTime);
                }
            }

        }
    }
}
