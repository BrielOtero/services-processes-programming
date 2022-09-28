using System.Diagnostics;
using System.Windows.Forms;

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

        private void showInfo(Process[] processes)
        {
            string id;
            string processName;
            string windowsTitle;
            string message;
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

            bool correct = true;
            int id = 0;

            correct = int.TryParse(textBox2.Text, out id);

            if (correct)
            {
                Process p = Process.GetProcessById(id);
                Process[] processes = new Process[] { p };
                showInfo(processes);

            }



        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
    }
}