using System.Diagnostics;
using System.Security;

namespace _01_exercise
{
    public partial class Form1 : Form
    {
        private DirectoryInfo currentDirInfo;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnChangePath_Click(object sender, EventArgs e)
        {
            string path = txtPath.Text.Trim();

            if (path[0] == '%' && path[path.Length - 1] == '%')
            {
                path = Environment.GetEnvironmentVariable(path);
            }

            try
            {
                Trace.WriteLine("hey");
                currentDirInfo = new DirectoryInfo(path);
            }
            catch (Exception a)
            {
                Trace.WriteLine(a.Message);
            }
        }

        private void loadList()
        {

        }
    }
}