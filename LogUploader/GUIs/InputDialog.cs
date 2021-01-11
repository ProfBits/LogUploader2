using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogUploader.GUIs
{
    public partial class InputDialog : Form
    {
        public InputDialog(string question, string title, string input = "")
        {
            InitializeComponent();
            DialogResult = DialogResult.None;
            Text = title;
            lblQuestion.Text = question;
            textBox1.Text = input;
        }

        public string Input { get => textBox1.Text; }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void InputDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (DialogResult == DialogResult.None) DialogResult = DialogResult.Cancel;
        }
    }
}
