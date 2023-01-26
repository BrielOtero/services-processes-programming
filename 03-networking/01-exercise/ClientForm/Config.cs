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
        public string IP_Server;
        public int Port;

        public Config(string ip_Server, int port)
        {
            InitializeComponent();
            this.IP_Server = ip_Server;
            txtIP.Text = ip_Server;
            this.Port = port;
            txtPort.Text = port.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (!IsIpValid())
            {
                MessageBox.Show(this, "The IP isn't valid", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtIP.Text = IP_Server;
                return;
            }

            DialogResult = DialogResult.Yes;
            this.Close();
        }

        private bool IsIpValid() => IPAddress.TryParse(IP_Server, out IPAddress a);

        private void TxtIP_TextChanged(object sender, EventArgs e)
        {
            IP_Server = txtIP.Text;
        }

        private void TxtPort_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(txtPort.Text, out int p) && p < IPEndPoint.MaxPort)
            {
                MessageBox.Show(this, "The PORT isn't valid", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPort.Text = Port.ToString();
                return;
            }

            Port = p;
        }
    }
}
