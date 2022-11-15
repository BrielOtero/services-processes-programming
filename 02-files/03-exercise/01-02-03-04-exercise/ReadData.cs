using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_02_03_04_exercise
{
    class ReadData : BinaryReader
    {
        public ReadData(Stream input) : base(input)
        {
        }

        public Employee ReadEmployee()
        {
            Employee employee = new Employee();
            employee.Name = base.ReadString();
            employee.Surname = base.ReadString();
            employee.Age = base.ReadInt32();
            employee.Dni = base.ReadString();
            employee.Salary = base.ReadDouble();
            employee.PhoneNumber = base.ReadString();

            return employee;
        }

        public Executive ReadExecutive()
        {
            Executive executive = new Executive();
            executive.Name = base.ReadString();
            executive.Surname = base.ReadString();
            executive.Age = base.ReadInt32();
            executive.DepartmentName = base.ReadString();
            executive.Dependents = base.ReadInt32();

            return executive;
        }
    }
}
