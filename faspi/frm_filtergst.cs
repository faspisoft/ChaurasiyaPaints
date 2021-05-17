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
    public partial class frm_filtergst : Form
    {
        public bool calledindirect = false;
        public string Fld1 = "";
        public string Fld2 = "";

        public bool chk1;
        public bool chk2;
        public bool chk3;
        public bool chk4;
        public bool chk5;
        public bool chk6;
        public bool chk7;
        public bool chk8;

        public bool rd1;
        public bool rd2;
        public bool rd3;
        public DateTime dt1;
        public DateTime dt2;


        public frm_filtergst()
        {
            InitializeComponent();
            dateTimePicker1.MinDate = Database.stDate;
            dateTimePicker1.MaxDate = Database.ldate;
            dateTimePicker1.Value = Database.stDate;
            dateTimePicker1.CustomFormat = Database.dformat;
            dateTimePicker2.MinDate = Database.stDate;
            dateTimePicker2.MaxDate = Database.ldate;
            dateTimePicker2.Value = Database.ldate;

            dateTimePicker2.CustomFormat = Database.dformat;

        }

        private void frm_filtergst_Load(object sender, EventArgs e)
        {
            if (checkBox8.Checked == true)
            {
                textBox1.Enabled = true;
                checkBox6.Checked = false;
                checkBox7.Checked = false;

            }
            else
            {
                textBox1.Enabled = false;
            }
            checkBox4.Text = "Within " + funs.Select_state_nm(Database.CompanyState_id);
            checkBox5.Text = "Other Than " + funs.Select_state_nm(Database.CompanyState_id);
            if (calledindirect == true)
            {
                textBox14.Text = this.Fld1;
                textBox1.Text = this.Fld2;
                dateTimePicker1.Value = this.dt1;
                dateTimePicker2.Value = this.dt2;
                checkBox1.Checked = this.chk1;
                checkBox2.Checked = this.chk2;
                checkBox3.Checked = this.chk3;
                checkBox4.Checked = this.chk4;
                checkBox5.Checked = this.chk5;
                checkBox6.Checked = this.chk6;
                checkBox7.Checked = this.chk7;
                checkBox8.Checked = this.chk8;
                radioButton1.Checked = this.rd1;
                radioButton2.Checked = this.rd2;
                radioButton3.Checked = this.rd3;
            }
        }
     

        private void button3_Click(object sender, EventArgs e)
        {
            this.Fld1 = textBox14.Text;
            this.Fld2 = textBox1.Text;
            this.chk1 = checkBox1.Checked;
            this.chk2 = checkBox2.Checked;
            this.chk3 = checkBox3.Checked;
            this.chk4 = checkBox4.Checked;
            this.chk5 = checkBox5.Checked;
            this.chk6 = checkBox6.Checked;
            this.chk7 = checkBox7.Checked;
            this.chk8 = checkBox8.Checked;
            this.rd1 = radioButton1.Checked;
            this.rd2 = radioButton2.Checked;
            this.rd3 = radioButton3.Checked;
            this.dt1 = dateTimePicker1.Value;
            this.dt2 = dateTimePicker2.Value;

                string str = "";
                string type = "";
                if (radioButton1.Checked == true)
                {
                    type = "Purchase";
                }
                else if (radioButton2.Checked == true)
                {
                    type = "Sale";
                }
                else if (radioButton3.Checked == true)
                {
                    type = "JobWork";
                }
                string rstatus = "";
                string state = "";
                if (type == "Sale")
                {
                    str = " And (Vouchertype.Type='Sale' or Vouchertype.type='Return')";
                    str += " And  (VOUCHERINFO.vdate >=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash + " And VOUCHERINFO.vdate <=" + access_sql.Hash + dateTimePicker2.Value.Date.ToString(Database.dformat) + access_sql.Hash + "  )";
                }
                else if (type == "Purchase")
                {
                    str = " And (Vouchertype.Type='Purchase' or Vouchertype.type='P Return') ";
                    str += " And  (VOUCHERINFO.vdate >=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash + " And VOUCHERINFO.vdate <=" + access_sql.Hash + dateTimePicker2.Value.Date.ToString(Database.dformat) + access_sql.Hash + "  )";
                }

                else if (type == "JobWork")
                {
                    str = " And (Vouchertype.Type='JIssue' or Vouchertype.type='JReceive')";
                    str += " And  (VOUCHERINFO.vdate >=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash + " And VOUCHERINFO.vdate <=" + access_sql.Hash + dateTimePicker2.Value.Date.ToString(Database.dformat) + access_sql.Hash + "  )";
                }

                if (checkBox4.Checked == true && checkBox5.Checked == true)
                {
                    state = "All";
                }
                else if (checkBox4.Checked == true && checkBox5.Checked == false)
                {
                    state = "Intra State";
                    str += " And State.Sname='" + funs.Select_state_nm(Database.CompanyState_id) + "'";
                }
                else if (checkBox4.Checked == false && checkBox5.Checked == true)
                {
                    state = "Inter State";
                    str += " And State.Sname<>'" + funs.Select_state_nm(Database.CompanyState_id) + "'";
                }

               
               

                if (checkBox1.Checked == true && checkBox2.Checked == false && checkBox3.Checked == false)
                {
                   
                    str += " And ACCOUNT.RegStatus='Unregistered' ";
                }


                else if (checkBox1.Checked == false && checkBox2.Checked == true && checkBox3.Checked == false)
                {

                    str += " And ACCOUNT.RegStatus='Regular Registration' ";
                }

                else if (checkBox1.Checked == false && checkBox2.Checked == false && checkBox3.Checked == true)
                {

                    str += " And ACCOUNT.RegStatus='Composition Dealer' ";
                }
                else if (checkBox1.Checked == true && checkBox2.Checked == true && checkBox3.Checked == false)
                {


                    str += " And (ACCOUNT.RegStatus='Unregistered' or ACCOUNT.RegStatus='Regular Registration') ";
                }
                else if (checkBox1.Checked == false && checkBox2.Checked == true && checkBox3.Checked == true)
                {


                    str += " And (ACCOUNT.RegStatus='Composition Dealer' or ACCOUNT.RegStatus='Regular Registration') ";
                }
                else if (checkBox1.Checked == true && checkBox2.Checked == false && checkBox3.Checked == true)
                {


                    str += " And (ACCOUNT.RegStatus='Unregistered' or ACCOUNT.RegStatus='Composition Dealer') ";
                }
                else if (checkBox1.Checked == true && checkBox2.Checked == true && checkBox3.Checked == true)
                {


                    str += " And (ACCOUNT.RegStatus='Unregistered' or ACCOUNT.RegStatus='Composition Dealer' or ACCOUNT.RegStatus='Regular Registration') ";
                }


                if (checkBox6.Checked == true && checkBox7.Checked == true   )
                {

                 
                }

                else if (checkBox6.Checked == true && checkBox7.Checked == false)
                {

                    str += " And VOUCHERDET.TotTaxPer<>0 ";
                 
                }
                else if (checkBox6.Checked == false && checkBox7.Checked == true)
                {
                    str += " And VOUCHERDET.TotTaxPer=0 ";
                  
                }
                else if (checkBox8.Checked == true)
                {

                    str += " And VOUCHERDET.TotTaxPer="+textBox1.Text+" ";
                  
                }


                 if (textBox14.Text!="")
                {
                    if (Feature.Available("GST Reports on ShipTo") == "Yes")
                    {
                        str += " And (VOUCHERINFO.Shipto='"+textBox14.Text+"') ";
                       
                    }
                    else
                    {
                        str += " And (ACCOUNT.Name='" + textBox14.Text + "') ";
                       
                    }


                
                }



                 if (calledindirect == false)
                 {
                     Report gg = new Report();
                     gg.AllGstReport(dateTimePicker1.Value, dateTimePicker2.Value, str, type);
                     gg.Fld1 = textBox14.Text;
                     gg.Fld2 = textBox1.Text;
                     gg.chk1 = checkBox1.Checked;
                     gg.chk2 = checkBox2.Checked;
                     gg.chk3 = checkBox3.Checked;
                     gg.chk4 = checkBox4.Checked;
                     gg.chk5 = checkBox5.Checked;
                     gg.chk6 = checkBox6.Checked;
                     gg.chk7 = checkBox7.Checked;
                     gg.chk8 = checkBox8.Checked;
                     gg.rd1 = radioButton1.Checked;
                     gg.rd2 = radioButton2.Checked;
                     gg.rd3 = radioButton3.Checked;
                     gg.dt1 = dateTimePicker1.Value;
                     gg.dt2 = dateTimePicker2.Value;
                     gg.MdiParent = this.MdiParent;
                     gg.Show();

                 }
                 else
                 {
                     this.Close();
                     this.Dispose();
                 }
           
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //DataTable dtcombo = new DataTable();
            //dtcombo.Columns.Add("Registration Status");
            //dtcombo.Rows.Add();
            //dtcombo.Rows[0][0] = "Unregistered";
            //dtcombo.Rows.Add();
            //dtcombo.Rows[1][0] = "Regular Registration";
            //dtcombo.Rows.Add();
            //dtcombo.Rows[2][0] = "Composition Dealer";
            //textBox1.Text = SelectCombo.ComboDt(this, dtcombo, 0);
            //SelectCombo.IsEnter(this, e.KeyCode);
        }

       

      

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void frm_filtergst_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox14_KeyPress(object sender, KeyPressEventArgs e)
        {
          string  strCombo = funs.GetStrCombonew(" (Path LIKE '8;40;%') or (Path LIKE '1;39;%') or (Path LIKE '1;38;%')  OR  (Path LIKE '1;3;%')   or   (Path LIKE '8;40;%')  or   (Path LIKE '8;39;%') ", " HAVING  Status=" + access_sql.Singlequote + "true" + access_sql.Singlequote);

          textBox14.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);

        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(dateTimePicker1);
        }

        private void dateTimePicker1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(dateTimePicker1);
        }

        private void dateTimePicker2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(dateTimePicker2);
        }

        private void dateTimePicker2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(dateTimePicker2);
        }

        private void dateTimePicker2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox14_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox14);
        }

        private void textBox14_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox14);
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked == true)
            {
                textBox1.Enabled = true;
                checkBox6.Checked = false;
                checkBox7.Checked = false;

            }
            else
            {
                textBox1.Enabled = false;
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked == true)
            {
                checkBox8.Checked = false;
                textBox1.Text = "";
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked == true)
            {
                checkBox8.Checked = false;
                textBox1.Text = "";
            }
        }
    }
}
