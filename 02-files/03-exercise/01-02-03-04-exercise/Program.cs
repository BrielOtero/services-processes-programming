#define CREATE

using System.Diagnostics;
using System.Diagnostics.Metrics;
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
            /**
             * Path
             */
            string applicationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "exercise3");
            string employeePath = Path.Combine(applicationPath, "employee.dat");
            string executivePath = Path.Combine(applicationPath, "executive.dat");


            /**
             * Read and Loading Files
             */

            createDirectory(applicationPath);

            try
            {


                if (File.Exists(employeePath))
                {
                    using (ReadData rDataEmployee = new ReadData(new FileStream(employeePath, FileMode.Open)))
                    {
                        while (rDataEmployee.BaseStream.Position < rDataEmployee.BaseStream.Length)
                        {
                            Employee employee = rDataEmployee.ReadEmployee();
                            ui.pm.people.Add(employee);
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Trace.WriteLine("File not found exception on employee");
            }
            catch (IOException)
            {
                Trace.WriteLine("IO exception on employee");
            }
            catch (Exception)
            {
                Trace.WriteLine("Exception found on employee");
            }

            try
            {


                if (File.Exists(executivePath))
                {
                    using (ReadData rDataExecutive = new ReadData(new FileStream(executivePath, FileMode.Open)))
                    {
                        while (rDataExecutive.BaseStream.Position < rDataExecutive.BaseStream.Length)
                        {
                            Executive executive = rDataExecutive.ReadExecutive();
                            ui.pm.people.Add(executive);
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Trace.WriteLine("File not found exception on executive");
            }
            catch (IOException)
            {
                Trace.WriteLine("IO exception on executive");
            }
            catch (Exception)
            {
                Trace.WriteLine("Exception found on executive");
            }
#endif
            ui.Start();

            /**
             * Write and Saving Files
             */

            createDirectory(applicationPath);

            try
            {
                using (WriteDataFiles w = new WriteDataFiles(employeePath, executivePath))
                {
                    foreach (var item in ui.pm.people)
                    {
                        if (item.GetType() == typeof(Executive))
                        {
                            w.WDataExecutive.Write((Executive)item);
                        }
                        else
                        {
                            w.WDataEmployee.Write((Employee)item);
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Trace.WriteLine("File not found exception on Write Files");
            }
            catch (IOException)
            {
                Trace.WriteLine("IO exception on Write Files");
            }
            catch (Exception)
            {
                Trace.WriteLine("Exception found on Write Files");
            }
        }

        private static void createDirectory(string applicationPath)
        {
            try
            {
                Directory.CreateDirectory(applicationPath);
            }
            catch (FileNotFoundException)
            {
                Trace.WriteLine("File not found exception on Create Directory");
            }
            catch (IOException)
            {
                Trace.WriteLine("IO exception on Create Directory");
            }
            catch (Exception)
            {
                Trace.WriteLine("Exception found on Create Directory");
            }
        }

        public struct WriteDataFiles : IDisposable
        {
            public WriteDataFiles(string employeePath, string executivePath)
            {
                wDataEmployee = new WriteData(new FileStream(employeePath, FileMode.Create));
                wDataExecutive = new WriteData(new FileStream(executivePath, FileMode.Create));
            }

            static WriteData wDataEmployee;
            static WriteData wDataExecutive;

            public WriteData WDataEmployee { get => wDataEmployee; set => wDataEmployee = value; }
            public WriteData WDataExecutive { get => wDataExecutive; set => wDataExecutive = value; }

            public void Dispose()
            {
                wDataEmployee.Dispose();
                wDataExecutive.Dispose();
            }
        }





    }
}