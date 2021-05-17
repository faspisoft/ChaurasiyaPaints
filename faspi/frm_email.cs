using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace faspi
{
    public partial class frm_email : Form
    {
        public  string strto="";
        public string strcc = "";
        public string strsubject = "";
        public string strmessage = "";
        public string strsendcancel = "";

        public frm_email()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            strto = textBox1.Text;
            strcc = textBox2.Text;
            strsubject = textBox3.Text;
            strmessage = textBox4.Text;
            strsendcancel = "Send";
            this.Close();
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            strsendcancel = "Cancel";
            this.Close();
            this.Dispose();
        }

        private void frm_email_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }
    }
}
