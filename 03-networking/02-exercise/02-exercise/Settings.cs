using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _02_exercise
{
    public partial class Settings : Form
    {
        public Config Config;

        public Settings(Config config)
        {
            InitializeComponent();
            this.Config = config;
            txtIp.Text = Config.IP_Server;
            txtPort.Text = Config.Port.ToString();
            txtUser.Text = Config.User;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }
    }
}
