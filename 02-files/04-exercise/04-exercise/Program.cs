namespace _04_exercise
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Bmp bmp = new Bmp("C:\\Users\\Gabriel\\Downloads\\image.bmp");
            Console.WriteLine(bmp.IsBmp());
        }
    }
}