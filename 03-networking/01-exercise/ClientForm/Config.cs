using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientForm
{
    public partial class Config : Form
    {
        public string ip_server;
        public int port;

        public Config(string ip_server, int port)
        {
            InitializeComponent();
            this.ip_server = ip_server;
            txtIP.Text = ip_server;
            this.port = port;
            txtPort.Text = port.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (!isIpValid())
            {
                MessageBox.Show(this, "The IP isn't valid", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtIP.Text = ip_server;
                return;
            }

            DialogResult = DialogResult.Yes;
            this.Close();
        }

        private bool isIpValid() => IPAddress.TryParse(ip_server, out IPAddress a);

        private void txtIP_TextChanged(object sender, EventArgs e)
        {
            ip_server = txtIP.Text;
        }

        private void txtPort_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(txtPort.Text, out int p))
            {
                MessageBox.Show(this,"The PORT isn't valid", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPort.Text = port.ToString();
                return;
            }

            port = p;
        }
    }
}
