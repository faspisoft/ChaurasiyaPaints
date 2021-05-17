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
    public partial class frmMobileno : Form
    {
        public frmMobileno()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() != "")
            {
                //DataTable dtmobileno = new DataTable();
                //Database.GetSqlData("Select * from Account where MobileNo='" + textBox1.Text + "'", dtmobileno);
                //if (dtmobileno.Rows.Count > 0)
                //{
                    Report gg = new Report();
                    gg.MobilenoSearch(textBox1.Text);
                    gg.MdiParent = this.MdiParent;
                    gg.Show();
            
                //}
            }
        }

        private void frmMobileno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }
    }
}
