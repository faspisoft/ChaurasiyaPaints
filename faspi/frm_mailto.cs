using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace faspi
{
    public partial class frm_mailto : Form
    {
        public string mailto = "";
        public string gsub = "";
        public string gbody = "";
        public frm_mailto(string mailto,string sub,string body,string atta)
        {
            InitializeComponent();
            textBox1.Text = mailto;
            textBox2.Text = sub;
            textBox3.Text = body;
            label5.Text = atta;

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void frm_mailto_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() != "")
            {
                this.mailto = textBox1.Text;
                this.gsub = textBox2.Text;
                this.gbody = textBox3.Text;
                this.Close();
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Enter Mail Id");
                textBox1.Focus();
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox3);
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox3);
        }
    }
}
