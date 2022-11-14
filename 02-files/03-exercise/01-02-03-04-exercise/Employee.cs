using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace _01_02_03_04_exercise
{
    internal class Employee : Person
    {
        private double salary;
        private int irpf;
        private string phoneNumber;

        public Employee(string name, string surname, int age, string dni, double salary, string phoneNumber)
            : base(name, surname, age, dni)
        {

            Salary = salary;
            this.irpf = irpf;
            PhoneNumber = phoneNumber;
        }

        public Employee()
            : base("", "", 0, "")
        {
            Salary = 0;
            this.irpf = 0;
            PhoneNumber = "";
        }

        public double Salary
        {
            get => salary;

            set
            {
                switch (value)
                {
                    case < 600:
                        irpf = 7;
                        break;
                    case >= 600 and <= 3000:
                        irpf = 15;
                        break;
                    case > 3000:
                        irpf = 20;
                        break;
                }

                salary = value;
            }
        }

        public int Irpf { get => irpf; }
        public string PhoneNumber
        {
            get
            {
                return "+34" + phoneNumber;
            }
            set => phoneNumber = value;
        }

        public override void ShowValues()
        {
            base.ShowValues();
            Console.WriteLine("Salary: {0}\nIrpf: {1}\nPhone Number: {2}\n", Salary, Irpf, PhoneNumber);
        }

        public void ShowValues(int show)
        {
            string message = "";

            switch (show)
            {
                case 0:
                    message = "Name: " + this.Name;
                    break;
                case 1:
                    message = "Surname: " + this.Surname;
                    break;
                case 2:
                    message = "Age: " + this.Age;
                    break;
                case 3:
                    message = "Dni: " + this.Dni;
                    break;
                case 4:
                    message = "Salary: " + this.Salary;
                    break;
                case 5:
                    message = "Irpf: " + this.Irpf;
                    break;
                case 6:
                    message = "Phone Number: " + this.PhoneNumber;
                    break;

            }

            Console.WriteLine(message);
        }

        public override double TaxAuthorities()
        {
            return (Irpf * Salary) / 100;
        }

        public override void InsertValues()
        {
            base.InsertValues();
            bool correct = true;

            do
            {
                if (!correct)
                {
                    Console.WriteLine("Insert a valid value");
                }

                Console.WriteLine("Insert the Salary: ");
                correct = Double.TryParse(Console.ReadLine(), out double salary);

                if (correct)
                {
                    Salary = salary;
                }
            } while (!correct);


            do
            {
                if (!correct)
                {
                    Console.WriteLine("Insert a valid value");
                }


                Console.WriteLine("Insert the phone number: ");

                correct = int.TryParse(Console.ReadLine(), out int num);

                if (num.ToString().Length != 9)
                {
                    correct = false;
                }

                if (correct)
                {
                    PhoneNumber = num.ToString();
                }

            } while (!correct);
        }
    }
}
