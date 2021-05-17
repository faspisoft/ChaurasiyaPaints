using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Threading;
using CrystalDecisions.Shared;
using System.IO;

namespace faspi
{
    public partial class frmLogin : Form
    {        
        String strCmd;
        String strCombo;

        public frmLogin()
        {
            InitializeComponent();
        }


        private void frmLogin_Load(object sender, EventArgs e)
        {
            dateTimePicker1.CustomFormat = Database.dformat;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            int randomno;
            Random ran = new Random();
            randomno = ran.Next(999999, 9999999);

            if (textBox1.Text == "")
            {
                MessageBox.Show("Enter username");
                textBox1.Focus();
                return;
            }
            else if (textBox2.Text == "")
            {
                MessageBox.Show("Enter password");
                textBox2.Focus();
                return;
            }
            else if (textBox3.Text == "")
            {
                MessageBox.Show("Select firm name");
                textBox3.Focus();
                return;
            }
            else if (textBox4.Text == "")
            {
                MessageBox.Show("Select financial year");
                textBox4.Focus();
                return;
            }

            strCmd = "SELECT Firm_name, Firm_Period_name, Firm_database, Firm_odate, Firm_edate FROM  FIRMINFO WHERE     (Firm_name = '" + textBox3.Text + "') AND (Firm_Period_name = '" + textBox4.Text + "')"; ;
            DataTable dtLogin = new DataTable();
            Database.GetOtherSqlData(strCmd, dtLogin);
            
            
            if (dtLogin.Rows.Count > 0)
            {
                DataTable dtInfo = new DataTable();
                Database.LoginbyDb = Database.GetOtherScalarBool("Select Gststatus from Firminfo where Firm_name = '" + textBox3.Text + "' and Firm_Period_name='" + textBox4.Text + "'");
                    

                if (Database.LoginbyDb == false)
                {
                    strCmd = "SELECT FIRMINFO.Firm_name, USERINFO.UName,USERINFO.Upass ,USERINFO.Utype, FIRMINFO.Firm_database, FIRMINFO.Firm_Period_name,Firm_odate,Firm_edate FROM (USERWCOMPANY INNER JOIN FIRMINFO ON USERWCOMPANY.F_id=FIRMINFO.F_id) INNER JOIN USERINFO ON USERWCOMPANY.U_id=USERINFO.U_id WHERE (((USERINFO.UName)='" + textBox1.Text + "') AND ((USERINFO.Upass)='" + textBox2.Text + "')) AND Firm_name='" + textBox3.Text + "' and Firm_Period_name='" + textBox4.Text + "'";
                    Database.GetOtherSqlData(strCmd, dtInfo);
                }
                else
                {
                    strCmd = "SELECT * from  Userinfo where uname='"+ textBox1.Text+"' and upass='"+ textBox2.Text+"'";
                    Database.databaseName = dtLogin.Rows[0]["Firm_database"].ToString();
                    Database.OpenConnection();
                    Database.GetSqlData(strCmd, dtInfo);
                    Database.CloseConnection();
                }

                if (dtInfo.Rows.Count > 0)
                {
                    Database.uname = dtInfo.Rows[0]["UName"].ToString();
                    Database.utype =  funs.Select_Role_Name(int.Parse(dtInfo.Rows[0]["RoleId"].ToString()));
                    Database.upass = dtInfo.Rows[0]["Upass"].ToString();
                    Database.fname = dtLogin.Rows[0]["Firm_name"].ToString();
                    Database.fyear = dtLogin.Rows[0]["Firm_Period_name"].ToString();
                    Database.databaseName = dtLogin.Rows[0]["Firm_database"].ToString();
                    Database.stDate = DateTime.Parse(dtLogin.Rows[0]["Firm_odate"].ToString());
                    Database.enDate = DateTime.Parse(dtLogin.Rows[0]["Firm_edate"].ToString());
                }
                else if (dtInfo.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid username or password");
                    textBox1.Focus();
                    return;
                }

                Database.ldate = DateTime.Parse(dateTimePicker1.Text);
                if (Database.ldate < Database.stDate)
                {
                    MessageBox.Show("login date cann't be less than starting date");
                    dateTimePicker1.Focus();
                    return;
                }
                else if (Database.ldate > Database.enDate)
                {
                    MessageBox.Show("login date cann't be greator than ending date");
                    dateTimePicker1.Focus();
                    return;
                }
                if (dtInfo.Rows.Count > 0)
                {
                    Database.setVariable(dtLogin.Rows[0]["Firm_name"].ToString(), dtLogin.Rows[0]["Firm_Period_name"].ToString(), Database.uname, Database.upass, Database.utype.ToUpper(), dtLogin.Rows[0]["Firm_database"].ToString(), DateTime.Parse(dtLogin.Rows[0]["Firm_odate"].ToString()), DateTime.Parse(dtLogin.Rows[0]["Firm_edate"].ToString()));
                    Database.SoftwareName = Database.GetOtherScalarText("SELECT SOFTWARENAME.Name as SoftwareName FROM SOFTWARENAME WHERE (((SOFTWARENAME.Value)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + "))");
                }
                DirectoryInfo dInfo = new System.IO.DirectoryInfo(Application.StartupPath + "\\efile");

                if (dInfo.Exists == false)
                {
                    Directory.CreateDirectory(Application.StartupPath + "\\efile");
                }

                frm_main frm = new frm_main();
                frm.random = randomno;
                frm.Show();
                this.Hide();             
            }
            else
            {
                MessageBox.Show("Invalid username or password");
                textBox1.Focus();
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
                Environment.Exit(0);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            Database.OpenConnection();
            strCombo = "SELECT distinct FIRMINFO.Firm_name FROM Firminfo order by FIRMINFO.Firm_name";
            textBox3.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
            Database.CloseConnection();
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            Database.OpenConnection();
            strCombo = "SELECT distinct FIRMINFO.Firm_Period_name FROM FIRMINFO where  FIRMINFO.Firm_name='" + textBox3.Text + "' order by FIRMINFO.Firm_Period_name desc";
            textBox4.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
            Database.CloseConnection();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox3);
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox4);
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox4);
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox3);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void dateTimePicker1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(dateTimePicker1);
        }

        private void dateTimePicker1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(dateTimePicker1);
        } 
    }
}
