using System.Diagnostics;

namespace _01_exercise
{
    internal class Program
    {

        static void Main(string[] args)
        {
            int[] v = { 2, 2, 6, 7, 1, 10, 3 };

            Array.ForEach(v, (x) =>
            {
                Console.ForegroundColor = x >= 5 ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine($"Student grade: {x,3}.");
                Console.ResetColor();
            });

            int res = Array.FindLastIndex(v, (x) =>
            {
                return x >= 5;
            });

            Console.WriteLine($"\nThe last passing student is number {res + 1} in the list.");

            Console.WriteLine("\nReverse\n");

            Array.ForEach(v, (x) =>
            {
                Console.WriteLine($"Before {x,2} | After {1 / x,2}");
            });

            Console.ReadKey();
        }
    }
}