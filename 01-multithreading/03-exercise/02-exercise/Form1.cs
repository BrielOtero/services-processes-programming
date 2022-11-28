using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

#nullable disable
namespace _02_exercise
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string cutString(string value, int chars)
        {

            if (value.Length > chars)
            {

                return value.Substring(0, chars - 3) + "...";
            }

            return value;
        }

        private void showInfo(Process[] processes, bool showMoreInfo = false)
        {
            string id;
            string processName;
            string windowsTitle;
            string message;
            string subpId;
            string subpStartTime;
            string modpName;
            string modpFileName;

            const string FORMAT = "{0,-4}  {1,-6}  {2,-5}  {3,-20}  {4,-6}  {5,-10}";
            Font myfont = new Font("Lucida Console", 9.0f);


            textBox1.Font = myfont;
            textBox1.Clear();

            foreach (Process p in processes)
            {
                id = cutString(p.Id.ToString(), 6);
                processName = cutString(p.ProcessName, 20);
                windowsTitle = cutString(p.MainWindowTitle, 10);

                message = String.Format(FORMAT, "PID", id, "Name", processName, "Title", windowsTitle);
                textBox1.AppendText(message + Environment.NewLine);
                Trace.WriteLine(message + Environment.NewLine);

                if (showMoreInfo)
                {

                    ProcessThreadCollection pt = p.Threads;

                    if (pt != null && pt.Count > 0)
                    {
                        textBox1.AppendText(Environment.NewLine + "Subprocess" + Environment.NewLine);

                        foreach (ProcessThread subp in pt)
                        {
                            subpId = cutString(subp.Id.ToString(), 6);
                            try
                            {

                                subpStartTime = cutString(subp.StartTime.ToString(), 10);
                                textBox1.AppendText($"\t{"PID",-4}  {subpId,-6}  {"Start Time",-10}  {subpStartTime,-10}" + Environment.NewLine);

                            }
                            catch (Win32Exception)
                            {
                                textBox1.Clear();
                                Trace.WriteLine("Denied");
                                return;
                            }
                        }
                    }

                    ProcessModuleCollection pm = p.Modules;

                    if (pm != null && pm.Count > 0)
                    {
                        textBox1.AppendText(Environment.NewLine + "Modules" + Environment.NewLine);

                        foreach (ProcessModule modp in pm)
                        {
                            modpName = cutString(modp.ModuleName, 20);
                            modpFileName = cutString(modp.FileName, 20);

                            textBox1.AppendText($"\t{"Name",-5}  {modpName,-20}  {"File Name",-10}  {modpFileName,-20}" + Environment.NewLine);

                        }
                    }
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            showInfo(Process.GetProcesses());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Font myfont = new Font("Lucida Console", 9.0f);
            textBox1.Font = myfont;



            bool find = getProcessesByText(out Process[] processes);

            if (find)
            {
                showInfo(processes, true);
            }


        }


        private void button3_Click(object sender, EventArgs e)
        {
            bool find = getProcessesByText(out Process[] processes);

            if (find)
            {
                processes[0].CloseMainWindow();
            }

        }


        private void button4_Click(object sender, EventArgs e)
        {
            bool find = getProcessesByText(out Process[] processes);

            if (find)
            {
                try
                {

                    processes[0].Kill();
                }
                catch (Win32Exception)
                {
                    Trace.WriteLine("Denied");
                    return;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(textBox2.Text);
            }
            catch (ObjectDisposedException)
            {
                Trace.WriteLine("error launching app");
            }
            catch (FileNotFoundException)
            {
                Trace.WriteLine("error launching app");
            }
            catch
            {
                Trace.WriteLine("error launching app");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {

            Process[] p = Process.GetProcesses();
            p = Array.FindAll(p, (x) => x.ProcessName.StartsWith(textBox2.Text));
            textBox1.Clear();
            Array.ForEach(p, (x) => textBox1.AppendText($"Name  {cutString(x.ProcessName, 20),-20}{Environment.NewLine}"));

        }

        private bool getProcessesByText(out Process[] processes)
        {
            bool correct = true;
            int id = 0;

            correct = int.TryParse(textBox2.Text, out id);

            if (correct)
            {
                try
                {
                    Process p = Process.GetProcessById(id);
                    processes = new Process[] { p };

                    return true;
                }
                catch (ArgumentException)
                {
                    Trace.WriteLine("error creating process");
                }
                catch
                {
                    Trace.WriteLine("error creating process");
                }


            }

            processes = null;

            return false;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}