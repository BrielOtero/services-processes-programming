using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _04_exercise
{
    internal class Bmp
    {
        private int width;
        private int height;
        private bool isCompress;
        private int bitPerPixel;

        public Bmp(int width, int height, bool isCompress, int bitPerPixel)
        {
            this.width = width;
            this.height = height;
            this.isCompress = isCompress;
            this.bitPerPixel = bitPerPixel;
        }

        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }
        public bool IsCompress { get => isCompress; set => isCompress = value; }
        public int BitsPerPixel { get => bitPerPixel; set => bitPerPixel = value; }
    }
}
