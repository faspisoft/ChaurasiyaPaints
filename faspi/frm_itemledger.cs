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
    public partial class frm_itemledger : Form
    {
        string strCombo = "";
        public bool calledindirect = false;
        public string Fld1 = "";
        public string Fld2 = "";
        public string Fld3 = "";
        public string Fld4 = "";
        public bool chk1;
        public DateTime dt1;
        public DateTime dt2;
      
        public frm_itemledger()
        {
            InitializeComponent();
            dateTimePicker1.Value = Database.stDate;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "SELECT Bname as BranchName from Branch order by Bname";
            textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select distinct '<MAIN>' as name from account union all SELECT ACCOUNT.Name as name FROM ACCOUNT LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id WHERE ACCOUNTYPE.Name='Godown' and Account.Branch_id='" + Database.BranchId + "' GROUP BY ACCOUNT.Name";
            textBox4.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textBox2.Text != "")
            {
                strCombo = "SELECT DISTINCT Description as name FROM Description WHERE Pack = '" + textBox2.Text + "' ORDER BY Description";
            }
            else
            {
                strCombo = "select distinct Description as name from Description order by Description";
            }
            textBox5.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textBox5.Text != "")
            {
                strCombo = "SELECT DISTINCT Pack as Packing FROM Description WHERE Description = '" + textBox5.Text + "' ORDER BY Pack";
            }
            else
            {
                strCombo = "SELECT DISTINCT Pack as Packing FROM Description ORDER BY Packing";
            }
            textBox2.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string sql = "  ";
            string sql1 = "";
            
          
            if (textBox1.Text == "")
            {
                textBox1.Focus();
                return;
            }
            if (textBox4.Text == "")
            {
                textBox4.Focus();
                return;
            }
            if (textBox5.Text == "")
            {
                textBox5.Focus();
                return;
            }
            if (textBox2.Text == "")
            {
                textBox2.Focus();
                return;
            }
            
            if (textBox1.Text != "")
            {
                sql += " and ";
                sql += " Voucherinfo.Branch_id='" + funs.Select_branch_id(textBox1.Text) + "' ";
                sql1 += " and ";
                sql1 += " Voucherinfo_1.Branch_id='" + funs.Select_branch_id(textBox1.Text) + "' ";                
            }
            if (textBox4.Text != "")
            {

                sql += " and ";
                sql1 += " and ";
               
                if (funs.Select_ac_id(textBox4.Text) == "")
                {
                    sql += " stock.Godown_id='' ";
                    sql1 += " stock_1.Godown_id='' ";
                  
                }
                else
                {
                    sql += " stock.Godown_id='" + funs.Select_ac_id(textBox4.Text) + "' ";
                    sql1 += " stock_1.Godown_id='" + funs.Select_ac_id(textBox4.Text) + "' ";
                    
                }
            }

          

            if (textBox5.Text != "")
            {
                sql += " and ";
                sql += " Description.Description='" + textBox5.Text + "' ";


                sql1 += " and ";
                sql1 += " Description_1.Description='" + textBox5.Text + "' ";   
            }

            if (textBox2.Text != "")
            {
                sql += " and ";
                sql += " Description.Pack='" + textBox2.Text + "' ";

                sql1 += " and ";
                sql1 += " Description_1.Pack='" + textBox2.Text + "' ";

               
            }

                    sql += " and ";
                    sql += " Vouchertype."+Database.BMode+"=" + access_sql.Singlequote + "true" + access_sql.Singlequote;


                    sql1 += " and ";
                    sql1 += " Vouchertype_1." + Database.BMode + "=" + access_sql.Singlequote + "true" + access_sql.Singlequote;
                    
              


            bool amtrequired = false;
            if (checkBox1.Checked == true)
            {
                amtrequired = true;
            }
            this.Fld1 = textBox1.Text;
            this.Fld2 = textBox4.Text;
            this.Fld3 = textBox5.Text;
            this.Fld4 = textBox2.Text;
            this.chk1 = amtrequired;
            this.dt1 = dateTimePicker1.Value;
            this.dt2 = dateTimePicker2.Value;


            if (calledindirect == false)
            {


                Report gg = new Report();
                gg.Fld1 = textBox1.Text;
                gg.Fld2 = textBox4.Text;
                gg.Fld3 = textBox5.Text;
                gg.Fld4 = textBox2.Text;
             
                gg.chk1 = checkBox1.Checked;
               
                gg.dt1 = dateTimePicker1.Value;
                gg.dt2 = dateTimePicker2.Value;
               
                gg.MdiParent = this.MdiParent;
                gg.ItemLedger1(dateTimePicker1.Value, dateTimePicker2.Value, sql, sql1, amtrequired);
                gg.Show();
            }
            this.Close();
            this.Dispose();
        }

        private void frm_itemledger_Load(object sender, EventArgs e)
        {
            dateTimePicker1.CustomFormat = Database.dformat;
            dateTimePicker1.Value = Database.stDate;
            dateTimePicker1.MaxDate = Database.ldate;
            dateTimePicker1.MinDate = Database.stDate;
            dateTimePicker2.CustomFormat = Database.dformat;
            dateTimePicker2.Value = Database.ldate;
            dateTimePicker2.MaxDate = Database.ldate;
            dateTimePicker2.MinDate = Database.stDate;



            textBox1.Text = this.Fld1;
            textBox4.Text = this.Fld2;
            textBox5.Text = this.Fld3;
          
            textBox2.Text = this.Fld4;
            if (calledindirect == true)
            {
                dateTimePicker1.Value = DateTime.Parse(dt1.ToString(Database.dformat));

                dateTimePicker2.Value = DateTime.Parse(dt2.ToString(Database.dformat));
            }

            if (chk1 == true)
            {
                checkBox1.Checked = true;
            }
            else
            {
                checkBox1.Checked = false;
            }
        }

      

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox4);
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox4);
        }

        private void textBox5_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox5);
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox5);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void dateTimePicker1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(dateTimePicker1);
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(dateTimePicker1);
        }

        private void dateTimePicker2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(dateTimePicker2);
        }

        private void dateTimePicker2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(dateTimePicker2);
        }
    }
}
