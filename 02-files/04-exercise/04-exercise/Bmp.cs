using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _04_exercise
{
    internal class Bmp
    {
        string bmpPath;

        public Bmp(string bmpPath)
        {
            this.bmpPath = bmpPath; 
        }
        public void IsBmp()
        {
            if (File.Exists(bmpPath))
            {
                using (FileStream fs = new FileStream(bmpPath, FileMode.Open))
                {
                    string format=fs.re
                }

            }

        }

        public void InfoBmp()
        {

        }

    }
}
