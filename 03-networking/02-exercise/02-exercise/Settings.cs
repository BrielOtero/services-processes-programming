using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
            Config = config;
            txtIp.Text = Config.IP_Server;
            txtPort.Text = Config.Port.ToString();
            txtUser.Text = Config.User;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Regex regexDNI = new("([0-9]){8}([A-Z]){1}");
            string message = "";
            bool validIP;
            bool validPORT;
            bool validUSER;

            Debug.WriteLine(txtIp.Text);
            validIP = IPAddress.TryParse(txtIp.Text, out IPAddress newIP);
            Debug.WriteLine(validIP);
            validPORT = int.TryParse(txtPort.Text, out int newPort) && newPort < IPEndPoint.MaxPort;
            validUSER = regexDNI.IsMatch(txtUser.Text.ToUpper());


            message += validIP ? "" : "IP ";
            message += validPORT ? "" : "PORT ";
            message += validUSER ? "" : "USER";

            if (message.Length > 0)
            {
                MessageBox.Show(this, $"The {message} isn't valid", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Config.IP_Server = txtIp.Text;
            Config.Port = newPort;
            Config.User = txtUser.Text.ToUpper();

            DialogResult = DialogResult.Yes;
            this.Close();
        }

    }
}
