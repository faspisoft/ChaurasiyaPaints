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
    public partial class frm_smslog : Form
    {
        DataTable dtsmslog = new DataTable();
        public frm_smslog()
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


        public void Loaddata() 
        {
            sms smsno = new sms();
            label1.Text=  smsno.GetBal();
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            dtsmslog.Clear();
            ansGridView5.Visible = true;
            ansGridView5.Rows.Clear();

            if (radioButton1.Checked == true)
            {

                //Database.GetSqlData("Select * from SMSLOG where Sdate>=#" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "# and Sdate<=#" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "# order by id", dtsmslog);

                Database.GetSqlData("Select * from SMSLOG where Sdate>=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash + " and Sdate<=" + access_sql.Hash + dateTimePicker2.Value.Date.ToString(Database.dformat) + access_sql.Hash + " order by id", dtsmslog);

                ansGridView5.Columns["Status"].Visible = true;
                for (int i = 0; i < dtsmslog.Rows.Count; i++)
                {
                    ansGridView5.Rows.Add();
                    ansGridView5.Rows[i].Cells["id"].Value = dtsmslog.Rows[i]["id"];
                    ansGridView5.Rows[i].Cells["Sno"].Value = (i + 1);
                    ansGridView5.Rows[i].Cells["AccName"].Value = dtsmslog.Rows[i]["AccName"];
                    ansGridView5.Rows[i].Cells["MNumber"].Value = dtsmslog.Rows[i]["MNumber"];
                    ansGridView5.Rows[i].Cells["Message"].Value = dtsmslog.Rows[i]["Message"];
                    ansGridView5.Rows[i].Cells["Sdate"].Value = DateTime.Parse(dtsmslog.Rows[i]["SDate"].ToString()).ToString(Database.dformat);
                    ansGridView5.Rows[i].Cells["Stime"].Value = dtsmslog.Rows[i]["STime"];
                    ansGridView5.Rows[i].Cells["RCode"].Value = dtsmslog.Rows[i]["RCode"];
                    ansGridView5.Rows[i].Cells["RDesc"].Value = dtsmslog.Rows[i]["RDesc"];
                    ansGridView5.Rows[i].Cells["Status"].Value = dtsmslog.Rows[i]["Status"];
                    ansGridView5.Rows[i].Cells["URL"].Value = dtsmslog.Rows[i]["URL"];
                }
            }
            else if (radioButton2.Checked == true)
            {
                //Database.GetSqlData("Select * from SMSLOG where status='Send' and Sdate>=#" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "# and Sdate<=#" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "# order by id", dtsmslog);

                Database.GetSqlData("Select * from SMSLOG where status='Send' and Sdate>=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash + " and Sdate<=" + access_sql.Hash + dateTimePicker2.Value.Date.ToString(Database.dformat) + access_sql.Hash + " order by id", dtsmslog);


                ansGridView5.Columns["Status"].Visible = false;
                for (int i = 0; i < dtsmslog.Rows.Count; i++)
                {
                    ansGridView5.Rows.Add();
                    ansGridView5.Rows[i].Cells["id"].Value = dtsmslog.Rows[i]["id"];
                    ansGridView5.Rows[i].Cells["Sno"].Value = (i + 1);
                    ansGridView5.Rows[i].Cells["AccName"].Value = dtsmslog.Rows[i]["AccName"];
                    ansGridView5.Rows[i].Cells["MNumber"].Value = dtsmslog.Rows[i]["MNumber"];
                    ansGridView5.Rows[i].Cells["Message"].Value = dtsmslog.Rows[i]["Message"];
                    ansGridView5.Rows[i].Cells["Sdate"].Value = DateTime.Parse(dtsmslog.Rows[i]["SDate"].ToString()).ToString(Database.dformat);
                    ansGridView5.Rows[i].Cells["Stime"].Value = dtsmslog.Rows[i]["STime"];
                    ansGridView5.Rows[i].Cells["RCode"].Value = dtsmslog.Rows[i]["RCode"];
                    ansGridView5.Rows[i].Cells["RDesc"].Value = dtsmslog.Rows[i]["RDesc"];
                    ansGridView5.Rows[i].Cells["Status"].Value = dtsmslog.Rows[i]["Status"];
                    ansGridView5.Rows[i].Cells["URL"].Value = dtsmslog.Rows[i]["URL"];
                }
            }
            else if (radioButton3.Checked == true)
            {
                //Database.GetSqlData("Select * from SMSLOG where status='Not Send' and Sdate>=#" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "# and Sdate<=#" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "# order by id", dtsmslog);

                Database.GetSqlData("Select * from SMSLOG where status='Not Send' and Sdate>=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash + " and Sdate<=" + access_sql.Hash + dateTimePicker2.Value.Date.ToString(Database.dformat) + access_sql.Hash + " order by id", dtsmslog);
                ansGridView5.Columns["Status"].Visible = false;
                for (int i = 0; i < dtsmslog.Rows.Count; i++)
                {
                    ansGridView5.Rows.Add();
                    ansGridView5.Rows[i].Cells["id"].Value = dtsmslog.Rows[i]["id"];
                    ansGridView5.Rows[i].Cells["Sno"].Value = (i + 1);
                    ansGridView5.Rows[i].Cells["AccName"].Value = dtsmslog.Rows[i]["AccName"];
                    ansGridView5.Rows[i].Cells["MNumber"].Value = dtsmslog.Rows[i]["MNumber"];
                    ansGridView5.Rows[i].Cells["Message"].Value = dtsmslog.Rows[i]["Message"];
                    ansGridView5.Rows[i].Cells["Sdate"].Value = DateTime.Parse(dtsmslog.Rows[i]["SDate"].ToString()).ToString(Database.dformat);
                    ansGridView5.Rows[i].Cells["Stime"].Value = dtsmslog.Rows[i]["STime"];
                    ansGridView5.Rows[i].Cells["RCode"].Value = dtsmslog.Rows[i]["RCode"];
                    ansGridView5.Rows[i].Cells["RDesc"].Value = dtsmslog.Rows[i]["RDesc"];
                    ansGridView5.Rows[i].Cells["Status"].Value = dtsmslog.Rows[i]["Status"];
                    ansGridView5.Rows[i].Cells["URL"].Value = dtsmslog.Rows[i]["URL"];
                }
            }
            else if (radioButton4.Checked == true)
            {
                //Database.GetSqlData("Select * from SMSLOG where status='Fail' and Sdate>=#" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "# and Sdate<=#" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "# order by id", dtsmslog);

                Database.GetSqlData("Select * from SMSLOG where status='Fail' and Sdate>=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash + " and Sdate<=" + access_sql.Hash + dateTimePicker2.Value.Date.ToString(Database.dformat) + access_sql.Hash + " order by id", dtsmslog);



                ansGridView5.Columns["Status"].Visible = false;
                for (int i = 0; i < dtsmslog.Rows.Count; i++)
                {
                    ansGridView5.Rows.Add();
                    ansGridView5.Rows[i].Cells["id"].Value = dtsmslog.Rows[i]["id"];
                    ansGridView5.Rows[i].Cells["Sno"].Value = (i + 1);
                    ansGridView5.Rows[i].Cells["AccName"].Value = dtsmslog.Rows[i]["AccName"];
                    ansGridView5.Rows[i].Cells["MNumber"].Value = dtsmslog.Rows[i]["MNumber"];
                    ansGridView5.Rows[i].Cells["Message"].Value = dtsmslog.Rows[i]["Message"];
                    ansGridView5.Rows[i].Cells["Sdate"].Value = DateTime.Parse(dtsmslog.Rows[i]["SDate"].ToString()).ToString(Database.dformat);
                    ansGridView5.Rows[i].Cells["Stime"].Value = dtsmslog.Rows[i]["STime"];
                    ansGridView5.Rows[i].Cells["RCode"].Value = dtsmslog.Rows[i]["RCode"];
                    ansGridView5.Rows[i].Cells["RDesc"].Value = dtsmslog.Rows[i]["RDesc"];
                    ansGridView5.Rows[i].Cells["Status"].Value = dtsmslog.Rows[i]["Status"];
                    ansGridView5.Rows[i].Cells["URL"].Value = dtsmslog.Rows[i]["URL"];
                }
            }
            Loaddata();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void frm_smslog_KeyDown(object sender, KeyEventArgs e)
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

        private void frm_smslog_Load(object sender, EventArgs e)
        {
            this.Size = this.MdiParent.Size;
        }
    }
}
