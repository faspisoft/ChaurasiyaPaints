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
    public partial class frm_rpt : Form
    {
        string strCombo = "";
        public string frmtext = "";
        public string Fld1 = "";
        public string Fld2 = "";
        public string Fld3 = "";
        public string Fld4 = "";
        public string Fld5 = "";
        public string Fld6 = "";
        public string Fld7 = "";
        public string Fld8 = "";
        public string Fld9 = "";
        public string Fld10 = "";
        public string typ = "";
        public DateTime dt1;
        public DateTime dt2;
        public bool calledindirect = false;
        public frm_rpt()
        {
            InitializeComponent();
            dateTimePicker1.Value = Database.cmonthFst;
            dateTimePicker1.MaxDate = Database.ldate;
            dateTimePicker1.MinDate = Database.stDate;
            dateTimePicker1.CustomFormat = Database.dformat;
            dateTimePicker2.Value = Database.ldate;
            dateTimePicker2.MaxDate = Database.ldate;
            dateTimePicker2.MinDate = Database.stDate;
            dateTimePicker2.CustomFormat = Database.dformat;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void Button1_Click(object sender, EventArgs e)
        {

            string sql = "";
            if (textBox1.Text != "")
            {
                sql += " and ";
                sql += " Branch_id='" + funs.Select_branch_id(textBox1.Text) + "' ";
            }
            if (textBox7.Text != "")
            {
                sql += " and ";
                sql += " Department='" + textBox7.Text + "' ";
            }
            if (textBox10.Text != "")
            {
                sql += " and ";
                sql += " Company='" + textBox10.Text + "' ";
            }
            if (textBox11.Text != "")
            {
                sql += " and ";
                sql += " Item='" +textBox11.Text + "' ";
            }
            if (textBox13.Text != "")
            {
                sql += " and ";
                sql += " Color='" + textBox13.Text + "' ";
            }
            if (textBox5.Text != "")
            {
                sql += " and ";
                sql += " OrgDescription='" + textBox5.Text + "' ";
            }
            if (textBox2.Text != "")
            {
                sql += " and ";
                sql += " Packing='" + textBox2.Text + "' ";
            }
            if (textBox4.Text != "")
            {
                sql += " and ";
                sql += " [ACCOUNT.Name]='" + textBox4.Text + "' ";
            }
            if (textBox3.Text != "")
            {
                sql += " and ";
                if (frmtext == "Agent")
                {
                    sql += " [Agent]='" + textBox3.Text + "' ";
                }
                else
                {
                    sql += " [Salesman]='" + textBox3.Text + "' ";
                }
            }
            if (textBox6.Text != "")
            {
                sql += " and ";
                sql += " RateApp='" + funs.Select_Rates_Id(textBox6.Text) + "' ";
            }
            
            sql += "and " + Database.BMode + "=" + access_sql.Singlequote + "true" + access_sql.Singlequote;


            this.Fld1 = textBox1.Text;
            this.Fld2 = textBox10.Text;
            this.Fld3 = textBox11.Text;
            this.Fld4 = textBox13.Text;
            this.Fld5 = textBox5.Text;
            this.Fld6 = textBox2.Text;
            this.Fld7 = textBox4.Text;
            this.Fld8 = textBox3.Text;
            this.Fld9 = textBox6.Text;
            this.Fld10 = textBox7.Text;
            this.dt1 = dateTimePicker1.Value;
            this.dt2 = dateTimePicker2.Value;
                    

            if (calledindirect == false)
            {
                Report gg = new Report();
                gg.AgentSalesmanReport(dateTimePicker1.Value, dateTimePicker2.Value, sql, frmtext);
                gg.Fld1 = textBox1.Text;
                gg.Fld2 = textBox10.Text;
                gg.Fld3 = textBox11.Text;
                gg.Fld4 = textBox13.Text;
                gg.Fld5 = textBox5.Text;
                gg.Fld6 = textBox2.Text;
                gg.Fld7 = textBox4.Text;
                gg.Fld8 = textBox3.Text;
                gg.Fld9 = textBox6.Text;
                gg.Fld10 = textBox7.Text;
                gg.gtype = frmtext;

                gg.dt1 = dateTimePicker1.Value;
                gg.dt2 = dateTimePicker2.Value;
                //  gg.SalePurchaseRegister(dateTimePicker1.Value, dateTimePicker2.Value, sql, typ);
                gg.MdiParent = this.MdiParent;
                gg.Show();

            }
            this.Close();
            this.Dispose();


        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "SELECT Bname as BranchName from Branch order by Bname";
            textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);            
        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from other where Type='" + funs.Get_Company_id() + "' order by [name]";
            textBox10.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);            
        }

        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from other where Type='" + funs.Get_Item_id() + "' order by [name]";
            textBox11.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);            
        }

        private void textBox13_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from other where Type='" + funs.Get_Colour_id() + "' order by [name]";
            textBox13.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);            
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

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "SELECT Name FROM ACCOUNT ORDER BY Name";
            textBox4.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            //strCombo = "SELECT Name FROM ACCOUNT ORDER BY Name";
            if (frmtext == "Agent")
            {
                strCombo = "SELECT ACCOUNT.Name FROM  ACCOUNT LEFT OUTER JOIN  ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id WHERE (ACCOUNTYPE.Name = 'Agent') ORDER BY ACCOUNT.Name";
            }
            else
            {
                strCombo = "SELECT Name FROM  Salesman order by name";
            }
            textBox3.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }
        
        private void frm_rpt_Load(object sender, EventArgs e)
        {
            textBox1.Text = funs.Select_branch_name(Database.BranchId);
            if (frmtext == "Agent")
            {
                this.Text = "Agent Report";
                label1.Text = "Agent Name";
            }
            else
            {
                this.Text = "Salesman Report";
                label1.Text = "Salesman Name";
            }


            textBox1.Text = this.Fld1;
            textBox10.Text = this.Fld2;
            textBox11.Text = this.Fld3;
            textBox13.Text = this.Fld4;
            textBox5.Text = this.Fld5;
            textBox2.Text = this.Fld6;
            textBox4.Text = this.Fld7;
            textBox3.Text = this.Fld8;
            textBox6.Text = this.Fld9;
            textBox7.Text = this.Fld10;
            dateTimePicker1.Value = Database.stDate;
            dateTimePicker2.Value = Database.ldate;





        }

        private void frm_rpt_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void frm_rpt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }

        private void dateTimePicker1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(dateTimePicker1);
        }

        private void dateTimePicker2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(dateTimePicker2);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox10_KeyDown(object sender, KeyEventArgs e)
        {
          //  SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox11_KeyDown(object sender, KeyEventArgs e)
        {
           // SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox13_KeyDown(object sender, KeyEventArgs e)
        {
           // SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
           // SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            //SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
           // SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
           // SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void textBox10_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox10);
        }

        private void textBox11_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox11);
        }

        private void textBox13_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox13);
        }

        private void textBox5_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox5);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox4);
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox3);
        }

        private void dateTimePicker1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(dateTimePicker1);
        }

        private void dateTimePicker2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(dateTimePicker2);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void textBox10_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox10);
        }

        private void textBox11_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox11);
        }

        private void textBox13_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox13);
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox5);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox4);
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox3);
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataTable dt1 = Master.DtRates.Select().CopyToDataTable();
            textBox6.Text = SelectCombo.ComboDt(this, dt1, 0);
            SendKeys.Send("{tab}");
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void textBox6_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox6);
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox6);
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from other where Type='" + funs.Get_Department_id() + "' order by [name]";
            textBox7.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);     
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox7);
        }

        private void textBox7_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox7);
        }
    }
}
