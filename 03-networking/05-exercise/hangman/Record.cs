using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_exercise
{
    internal class Record : IComparable
    {
        private string name;

        public string Name
        {
            set
            {
                if (value.Length > 3) { throw new ArgumentException("value"); }
                name = value;
            }
            get
            {
                return name;
            }
        }

        public int Seconds { set; get; }

        public Record(string name, int seconds)
        {
            this.Name = name;
            this.Seconds = seconds;
        }

        public int CompareTo(object obj)
        {
            if (obj is null) return 1;

            return this.Seconds.CompareTo((obj as Record).Seconds);

        }
    }

}
