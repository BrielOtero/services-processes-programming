using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _04_exercise
{
    internal class BmpUtil
    {
        string bmpPath;

        public BmpUtil(string bmpPath)
        {
            this.bmpPath = bmpPath;
        }
        public bool IsBmp()
        {
            try
            {
                if (!File.Exists(this.bmpPath))
                {
                    return false;
                }

                using (BinaryReader br = new BinaryReader(new FileStream(bmpPath, FileMode.Open)))
                {
                    string format = ((char)br.ReadByte()).ToString() + ((char)br.ReadByte()).ToString();

                    if (format != "BM")
                    {
                        return false;
                    }

                    br.BaseStream.Seek(0x02, SeekOrigin.Begin);

                    if (br.ReadInt32() != new FileInfo(bmpPath).Length)
                    {
                        return false;
                    }

                    br.BaseStream.Seek(0x0E, SeekOrigin.Begin);

                    if (br.ReadInt32() < 40)
                    {
                        return false;
                    }
                }
            }
            catch (EndOfStreamException)
            {
                Trace.WriteLine("EndOfStreamException isBMP");
                return false;
            }
            catch (IOException)
            {
                Trace.WriteLine("IOException isBMP");
                return false;
            }
            catch (Exception)
            {
                Trace.WriteLine("Exception isBMP");
                return false;
            }

            return true;
        }

        public Bmp InfoBmp()
        {
            if (IsBmp())
            {
                int width;
                int height;
                bool isCompress;
                int bitPerPixel;

                try
                {
                    using (BinaryReader br = new BinaryReader(new FileStream(bmpPath, FileMode.Open)))
                    {
                        br.BaseStream.Seek(0x12, SeekOrigin.Begin);
                        width = br.ReadInt32();
                        height = br.ReadInt32();
                        br.BaseStream.Seek(0x1E, SeekOrigin.Begin);
                        isCompress = br.ReadInt32() != 0;
                        br.BaseStream.Seek(0x1C, SeekOrigin.Begin);
                        bitPerPixel = br.ReadInt32();
                        return new Bmp(width, height, isCompress, bitPerPixel);
                    }
                }
                catch (EndOfStreamException)
                {
                    Trace.WriteLine("EndOfStreamException Info BMP");
                }
                catch (IOException)
                {
                    Trace.WriteLine("IOException Info BMP");
                }
                catch (Exception)
                {
                    Trace.WriteLine("Exception Info BMP");
                }
            }
                return null;
        }

    }
}
