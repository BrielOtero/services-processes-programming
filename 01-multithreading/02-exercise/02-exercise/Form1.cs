using System.Diagnostics;

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

        private void button1_Click(object sender, EventArgs e)
        {
            Process[] processes = Process.GetProcesses();
            string id;
            string processName;
            string windowsTitle;

            foreach (Process p in processes)
            {
                id = cutString(p.Id.ToString(), 10);
                processName = cutString(p.ProcessName, 20);
                windowsTitle = cutString(p.MainWindowTitle, 20);

                textBox1.AppendText($"ID={id,10}       Process Name={processName,20}");

                if (windowsTitle.Length != 0)
                {
                    textBox1.AppendText($"Window Title={windowsTitle,10}{Environment.NewLine}");
                }
                else
                {
                    textBox1.AppendText(Environment.NewLine);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

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