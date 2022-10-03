//#define TEST
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
        public int line;

        public Horse(int line)
        {
            this.position = 0;
            this.line = line;
        }

        public void Run()
        {

#if TEST
            position++;
        
#else
            Random r = new Random();
            position += r.Next(1, 5);

#endif

        }
    }

}
