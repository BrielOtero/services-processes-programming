namespace _04_exercise
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BmpUtil bmpUtil = new BmpUtil("C:\\Users\\Gabriel\\Downloads\\image.bmp");
            Console.WriteLine("Is bmp: " + bmpUtil.IsBmp());
            
            Bmp bmp = bmpUtil.InfoBmp();

            if (bmp != null)
            {
                Console.WriteLine("Width: " + bmp.Width);
                Console.WriteLine("Height: " + bmp.Height);
                Console.WriteLine("Is Compress: " + bmp.IsCompress);
                Console.WriteLine("Bits Per Pixel: " + bmp.BitsPerPixel);
            }
            else
            {
                Console.WriteLine("Error reading bmp");
            }


        }
    }
}