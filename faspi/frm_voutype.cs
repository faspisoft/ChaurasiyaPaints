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
    public partial class frm_voutype : Form
    {
        DataTable dt;
       
        public frm_voutype()
        {
            InitializeComponent();
        }

        private void frm_voutype_Load(object sender, EventArgs e)
        {
            dt = new DataTable();
            //if (Database.IsKacha == false)
            //{
                Database.GetSqlData("SELECT AliasName, Name, Short FROM VOUCHERTYPE where "+Database.BMode+"=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " ORDER BY Name, Short, AliasName", dt);
            //}
            //else
            //{
            //    Database.GetSqlData("SELECT AliasName, Name, Short FROM VOUCHERTYPE where B=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " ORDER BY Name, Short, AliasName", dt);
            //}
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["Select"].Value = true;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["name"].Value = dt.Rows[i]["Name"].ToString();
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["shrt"].Value = dt.Rows[i]["Short"].ToString();
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["aliasname"].Value = dt.Rows[i]["AliasName"].ToString();
            }
            dateTimePicker1.MinDate = Database.stDate;
            dateTimePicker1.MaxDate = Database.ldate;
            dateTimePicker1.CustomFormat = Database.dformat;
            dateTimePicker1.Value = Database.ldate;
            dateTimePicker2.MinDate = Database.stDate;
            dateTimePicker2.MaxDate = Database.ldate;
            dateTimePicker2.CustomFormat = Database.dformat;
            dateTimePicker2.Value = Database.ldate;
        }

        private void frm_voutype_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = "";
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (bool.Parse(dataGridView1.Rows[i].Cells["Select"].Value.ToString()) == true)
                {
                    string id = funs.Select_vt_id_nm(dataGridView1.Rows[i].Cells["name"].Value.ToString());
                    if (id !="0")
                    {
                        str = str + " Or VOUCHERINFO.Vt_id='" + id + "' ";
                    }
                }
            }
            if (str.Length > 5)
            {
                str = str.Remove(0, 4);
            }
            Report gg = new Report();
            gg.MdiParent = this.MdiParent;
            gg.Journal(dateTimePicker1.Value, dateTimePicker2.Value, str);
            gg.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells["Select"].Value = false;
                }
            }
            else if (checkBox1.Checked == false)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells["Select"].Value = true;
                }
            }
        }
    }
}
