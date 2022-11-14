#define CREATE

using System.Xml.Linq;

namespace _01_02_03_04_exercise
{

    internal class Program
    {
        private static void companyEarnings(IGoosePasta iGoosePasta)
        {
            Console.WriteLine("Insert Company Earnings");
            Console.WriteLine("Win Pasta: " + iGoosePasta.WinPasta(Convert.ToInt32(Console.ReadLine())));
        }
        static void Main(string[] args)
        {

            UserInterface ui = new UserInterface();

#if CREATE

            Employee e1 = new Employee("Manolo", "Sanchez", 22, "33144756C", 25000, "614567426");
            Employee e2 = new Employee("Pepe", "Otero", 25, "22664666C", 12000, "655443326");
            Employee e3 = new Employee("Lucia", "Dominguez", 30, "33127566C", 42000, "8645554726");
            Executive ex = new Executive("Paco", "Martinez", 50, "23148944E", "Contabilidad", 3);

            ui.pm.people.Insert(ui.pm.Position(e1.Age), e1);
            ui.pm.people.Insert(ui.pm.Position(e2.Age), e2);
            ui.pm.people.Insert(ui.pm.Position(e3.Age), e3);
            ui.pm.people.Insert(ui.pm.Position(ex.Age), ex);
#endif
            ui.Start();

        }

    }
}