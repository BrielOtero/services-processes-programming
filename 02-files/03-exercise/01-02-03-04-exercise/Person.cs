using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace _01_02_03_04_exercise
{
    abstract class Person
    {
        string name;
        string surname;
        int age;
        string dni;

        public abstract double TaxAuthorities();

        public string Name { get => name; set => name = value; }
        public string Surname { get => surname; set => surname = value; }
        public int Age
        {
            get => age;
            set
            {
                if (value < 0)
                {
                    value = 0;
                }

                age = value;
            }
        }
        public string Dni
        {
            get
            {
                if (dni != "")
                {
                    int rest = Convert.ToInt32(dni) % 23;
                    string lettersDni = "TRWAGMYFPDXBNJZSQVHLCKE";

                    return dni + lettersDni[rest].ToString();
                }
                else
                {
                    return dni;
                }
            }

            set
            {
                if (value.Length > 0)
                {
                    dni = value.Substring(0, 8);

                }
                else
                {
                    dni = value;
                }
            }
        }

        public Person(string name, string surname, int age, string dni)
        {
            this.Name = name;
            this.Surname = surname;
            this.Age = age;
            this.Dni = dni;
        }

        public Person()
           : this("", "", 0, "")
        {
        }


        public virtual void ShowValues()
        {
            Console.WriteLine("Name: {0}\nSurname: {1}\nAge: {2}\nDni: {3}\n", Name, Surname, Age, Dni);
        }


        public virtual void InsertValues()
        {
            Console.WriteLine("Insert the Name: ");
            Name = Console.ReadLine();

            Console.WriteLine("Insert the Surname: ");
            Surname = Console.ReadLine();

            bool correct = true;

            do
            {
                if (!correct)
                {
                    Console.WriteLine("Insert a valid value");
                }

                Console.WriteLine("Insert the Age: ");
                correct = int.TryParse(Console.ReadLine(), out int age);

                if (correct)
                {
                    Age = age;
                }

            } while (!correct);

            do
            {
                if (!correct)
                {
                    Console.WriteLine("Insert a valid value");
                }
                correct = true;
                Console.WriteLine("Insert the Dni: ");
                string dni = Console.ReadLine();

                if (dni.Length != 9)
                {
                    correct = false;
                }

                if (correct)
                {
                    Dni = dni;
                }

            } while (!correct);
        }


    }
}


