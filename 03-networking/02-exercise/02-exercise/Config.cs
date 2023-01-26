using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_exercise
{
    public class Config
    {
        public Config()
        {
            IP_Server = "192.168.20.100";
            Port = 5001;
            User = "77542726C";
        }

        public string IP_Server { set; get; }
        public int Port { set; get; }
        public string User { set; get; }
       
    }
}
