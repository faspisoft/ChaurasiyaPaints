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
    public partial class frm_removesmslog : Form
    {
        DataTable dtsmslog = new DataTable();
        public frm_removesmslog()
        {
            InitializeComponent();
            dateTimePicker1.MinDate = Database.stDate;
            dateTimePicker1.MaxDate = Database.ldate;
            dateTimePicker1.Value = Database.stDate;
            dateTimePicker1.CustomFormat = Database.dformat;
            dateTimePicker2.CustomFormat = Database.dformat;
            dateTimePicker2.MinDate = Database.stDate;
            dateTimePicker2.MaxDate = Database.ldate;
            dateTimePicker2.Value = Database.ldate;
        }

        public void loaddata()
        {
            radioButton1.Checked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            dtsmslog.Clear();
            DialogResult res = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel);
            if (res == DialogResult.OK)
            {

                if (radioButton1.Checked == true)
                {
                    Database.CommandExecutor("Delete  from SMSLOG where Sdate>="+access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash+" and Sdate<="+access_sql.Hash + dateTimePicker2.Value.Date.ToString(Database.dformat) + access_sql.Hash+" ");

                    MessageBox.Show("Deleted Successfully");
                }
                else if (radioButton2.Checked == true)
                {
                    Database.CommandExecutor("Delete  from SMSLOG where  status='Send' and Sdate>="+access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash+" and Sdate<="+access_sql.Hash + dateTimePicker2.Value.Date.ToString(Database.dformat) + access_sql.Hash+" ");

                    MessageBox.Show("Deleted Successfully");
                }
                else if (radioButton3.Checked == true)
                {
                    Database.CommandExecutor("Delete  from SMSLOG where  status='Not Send' and Sdate>="+access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash+" and Sdate<="+access_sql.Hash + dateTimePicker2.Value.Date.ToString(Database.dformat) + access_sql.Hash+" ");

                    MessageBox.Show("Deleted Successfully");
                }
                else if (radioButton4.Checked == true)
                {
                    Database.CommandExecutor("Delete  from SMSLOG where  status='Fail' and Sdate>="+access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash+" and Sdate<="+access_sql.Hash + dateTimePicker2.Value.Date.ToString(Database.dformat) + access_sql.Hash +" ");

                    MessageBox.Show("Deleted Successfully");
                }
                loaddata();
            }
        }

        private void frm_removesmslog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {

                DialogResult chk = MessageBox.Show("Are u sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (chk == DialogResult.No)
                {
                    e.Handled = false;
                }
                else
                {
                    this.Close();
                    this.Dispose();
                }

            }

        }

        private void frm_removesmslog_Load(object sender, EventArgs e)
        {
            this.Size = this.MdiParent.Size;
        }
    }
}
