using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_exercise
{
    internal class Horse
    {
        public int position;

        public void Run()
        {
            Random r = new Random();
            position += r.Next(1, 3);
            Thread.Sleep(r.Next(1, 3) * 1000);
        }
    }

}
