using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hangman
{
    public partial class SendWord : System.Windows.Forms.Form
    {
        public string Words;
        public SendWord()
        {
            InitializeComponent();
        }

        private void ChFile_CheckedChanged(object sender, EventArgs e)
        {
            if (chFile.Checked)
            {
                btnOpenFile.Visible = true;
                txtWords.Visible = false;
            }
            else
            {
                btnOpenFile.Visible = false;
                txtWords.Visible = true;
            }
        }

        private void BtnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Texto (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Words = File.ReadAllText(openFileDialog.FileName).ToUpper();
            }
        }

        private void TxtWords_TextChanged(object sender, EventArgs e)
        {
            Words = txtWords.Text;
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Words))
            {
                MessageBox.Show(this, "Please insert valid words", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.DialogResult = DialogResult.OK;
        }
    }
}
