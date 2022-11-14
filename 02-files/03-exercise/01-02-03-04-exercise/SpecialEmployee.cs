using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_02_03_04_exercise
{
    internal class SpecialEmployee : Employee, IGoosePasta
    {

        public SpecialEmployee(string name, string surname, int age, string dni, double salary, string phoneNumber)
        : base(name, surname, age, dni, salary, phoneNumber)
        {
        }

        private double winPasta;
        public double WinPasta(double profitEUR)
        {
            winPasta = profitEUR * 0.5;
            return winPasta;
        }

        public override double TaxAuthorities()
        {
            return (0.5 * winPasta) + winPasta;
        }
    }
}
