using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _01_exercise
{
    public partial class TxtFormContent : Form
    {
        private string content;
        public string Content { get => content; set => content = value; }

        public TxtFormContent(string path)
        {
            InitializeComponent();
            txtContent.Text=File.ReadAllText(path);
            txtContent.Modified = false;
        }


        private void TxtFormContent_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(txtContent.Modified)
            {
                Content=txtContent.Text;
                this.DialogResult = DialogResult.Yes;
            }
        }
    }
}
