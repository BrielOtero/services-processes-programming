using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hangman
{
    public partial class RecordName : System.Windows.Forms.Form
    {
        public string Name;
        public RecordName()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(txtName.Text))
            {
                Name = txtName.Text;
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show(this, $"Please insert valid name", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
