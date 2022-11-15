using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_02_03_04_exercise
{
    class WriteData : BinaryWriter
    {
        public WriteData(Stream input) : base(input)
        {
        }

        public void Write(Employee employee)
        {

            base.Write(employee.Name);
            base.Write(employee.Surname);
            base.Write(employee.Age);
            base.Write(employee.Dni);
            base.Write(employee.Salary);
            base.Write(employee.PhoneNumber);
        }

        public void Write(Executive executive)
        {
            base.Write(executive.Name);
            base.Write(executive.Surname);
            base.Write(executive.Age);
            base.Write(executive.DepartmentName);
            base.Write(executive.Dependents);
        }
    }
}
