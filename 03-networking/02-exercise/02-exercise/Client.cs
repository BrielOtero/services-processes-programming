namespace _02_exercise
{
    public partial class Client : Form
    {
        private string IP_SERVER = "192.168.20.100";
        private int PORT = 5001;
        private Config config;

        public Client()
        {
            InitializeComponent();


            



        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            Settings settings = new();

            if(settings.ShowDialog() == DialogResult.Yes)
            {

            }
        }
    }
}