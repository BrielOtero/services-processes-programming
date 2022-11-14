using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_02_03_04_exercise
{
    internal class Executive : Person, IGoosePasta
    {
        private string departmentName;
        private double profit;
        private int dependents;
        private double winPasta;

        public int Dependents
        {
            get => dependents;
            set
            {
                switch (value)
                {
                    case < 10:
                        profit = 2;
                        break;
                    case >= 11 and <= 50:
                        profit = 3.5;
                        break;
                    case > 50:
                        profit = 4;
                        break;
                }
                dependents = value;
            }
        }

        public Executive()
            : this("", "", 0, "", "", 0)
        {
        }

        public Executive(string name, string surname, int age, string dni, string departmentName, int dependents)
            : base(name, surname, age, dni)
        {

            this.departmentName = departmentName;
            Dependents = dependents;
        }

        public override double TaxAuthorities()
        {
            return 0.3 * winPasta;
        }

        public static Executive operator --(Executive executive)
        {
            if (executive.profit >= 0)
            {
                executive.profit = executive.profit - 1;

            }
            return executive;
        }

        public override void ShowValues()
        {
            base.ShowValues();
            Console.WriteLine("Department Name: {0}\nProfit: {1}\nDependents: {2}\n", departmentName, profit, dependents);
        }

        public override void InsertValues()
        {

            base.InsertValues();
            bool correct = true;

            Console.WriteLine("Insert the Department Name: ");
            departmentName = Console.ReadLine();

            do
            {
                if (!correct)
                {
                    Console.WriteLine("Insert a valid value");
                }

                Console.WriteLine("Insert the Dependents: ");
                correct = int.TryParse(Console.ReadLine(), out dependents);
            } while (!correct);


        }

        double IGoosePasta.WinPasta(double profitEUR)
        {
            if (profitEUR < 0)
            {
                Executive e = this;
                e--;

                winPasta = 0;
                return winPasta;
            }

            winPasta = (profit * profitEUR) / 100;
            return winPasta;
        }

    }
}

