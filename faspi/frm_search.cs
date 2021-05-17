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
    public partial class frm_search : Form
    {
        string strCombo = "";
        string selected = "";
        DataTable dt;

        public frm_search()
        {
            InitializeComponent();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textBox1.Text != "")
            {
                strCombo = "SELECT DISTINCT Description as name FROM Description WHERE Pack = '" + textBox1.Text + "' ORDER BY Description";
            }
            else
            {
                strCombo = "select distinct Description as name from Description order by Description";
            }
            textBox2.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textBox2.Text != "")
            {
                strCombo = "SELECT DISTINCT Pack as Packing FROM Description WHERE Description = '" + textBox2.Text + "' ORDER BY Pack";
            }
            else
            {
                strCombo = "SELECT DISTINCT Pack as Packing FROM Description ORDER BY Packing";
            }
            textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void frm_search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }

        private string IsDocumentNumber(String str)
        {
            return Database.GetScalarText("SELECT DISTINCT VOUCHERINFO.Vi_id, " + access_sql.Docnumber + " AS DocNumber FROM (VOUCHERINFO LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERINFO.Vt_id)=[VOUCHERTYPE].[Vt_id]) AND (" + access_sql.Docnumber + "='" + str + "')) AND (VOUCHERINFO.Branch_id = '" + Database.BranchId + "')");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                if (textBox3.Text.Trim() == "")
                {
                    textBox3.Focus();
                    return;
                }
                strCombo = "SELECT VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)) AS DocNumber, CONVERT(nvarchar, VOUCHERINFO.Vdate, 106) AS Vdate, CAST(VOUCHERINFO.Totalamount AS nvarchar(250)) AS Totalamount FROM VOUCHERINFO LEFT OUTER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (VOUCHERINFO.Vnumber = " + int.Parse(textBox3.Text) + ") GROUP BY VOUCHERINFO.Vdate, VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)), VOUCHERINFO.Totalamount, VOUCHERTYPE.Type, VOUCHERINFO.Branch_id HAVING (VOUCHERTYPE.Type = 'Sale') AND (VOUCHERINFO.Branch_id = '" + Database.BranchId + "') ORDER BY Vdate DESC, DocNumber";
                //strCombo = "SELECT VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)) AS DocNumber, CONVERT(nvarchar,Voucherinfo.Vdate, 106) as Vdate, CAST(VOUCHERINFO.Totalamount AS nvarchar(250)) AS Totalamount FROM VOUCHERINFO LEFT OUTER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (VOUCHERINFO.Vnumber = " + int.Parse(textBox3.Text) + ") GROUP BY VOUCHERINFO.Vdate, VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)), VOUCHERINFO.Totalamount  ORDER BY VOUCHERINFO.Vdate Desc, DocNumber";
                DataTable dttemp = new DataTable();
                Database.GetSqlData(strCombo, dttemp);

                if (dttemp.Rows.Count == 1)
                {
                    selected = dttemp.Rows[0][0].ToString();
                }
                else
                {
                    char cg = 'a';
                    selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 2);
                }
                label5.Text = selected;
            }
            else if (radioButton2.Checked == true)
            {
                if (textBox2.Text == "")
                {
                    textBox2.Focus();
                    return;
                }
                if (textBox1.Text == "")
                {
                    textBox1.Focus();
                    return;
                }
                string did = funs.Select_des_id(textBox2.Text, textBox1.Text);
                strCombo = "SELECT VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)) AS DocNumber, CONVERT(nvarchar, VOUCHERINFO.Vdate, 106) AS Vdate, CAST(VOUCHERINFO.Totalamount AS nvarchar(250)) AS Totalamount FROM VOUCHERINFO LEFT OUTER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id LEFT OUTER JOIN Voucherdet ON VOUCHERINFO.Vi_id = Voucherdet.Vi_id WHERE (Voucherdet.Des_ac_id = '" + did + "') GROUP BY VOUCHERINFO.Vdate, VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)), VOUCHERINFO.Totalamount, VOUCHERTYPE.Type, VOUCHERINFO.Branch_id HAVING (VOUCHERTYPE.Type = 'Sale') AND (VOUCHERINFO.Vdate >= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERINFO.Vdate <= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERINFO.Branch_id = '" + Database.BranchId + "') ORDER BY Vdate, DocNumber";
                //strCombo = "SELECT VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)) AS DocNumber, CONVERT(nvarchar, VOUCHERINFO.Vdate, 106) AS Vdate, CAST(VOUCHERINFO.Totalamount AS nvarchar(250)) AS Totalamount FROM VOUCHERINFO LEFT OUTER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id LEFT OUTER JOIN Voucherdet ON VOUCHERINFO.Vi_id = Voucherdet.Vi_id WHERE (Voucherdet.Des_ac_id = '" + did + "') GROUP BY VOUCHERINFO.Vdate, VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)), VOUCHERINFO.Totalamount, VOUCHERTYPE.Type, VOUCHERINFO.LocationId HAVING (VOUCHERINFO.Vdate >= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERINFO.Vdate <= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERTYPE.Type = 'Sale') AND (VOUCHERINFO.LocationId = '" + Database.LocationId + "') ORDER BY Vdate, DocNumber";
                //strCombo = "SELECT VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)) AS DocNumber, CONVERT(nvarchar,Voucherinfo.Vdate, 106) as Vdate, CAST(VOUCHERINFO.Totalamount AS nvarchar(250)) AS Totalamount FROM VOUCHERINFO LEFT OUTER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id LEFT OUTER JOIN Voucherdet ON VOUCHERINFO.Vi_id = Voucherdet.Vi_id WHERE (Voucherdet.Des_ac_id = '" + did + "') GROUP BY VOUCHERINFO.Vdate, VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)), VOUCHERINFO.Totalamount HAVING (VOUCHERINFO.Vdate >= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERINFO.Vdate <= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') ORDER BY VOUCHERINFO.Vdate, DocNumber";
                DataTable dttemp = new DataTable();
                Database.GetSqlData(strCombo, dttemp);

                if (dttemp.Rows.Count == 1)
                {
                    selected = dttemp.Rows[0][0].ToString();
                }
                else
                {
                    char cg = 'a';
                    selected = SelectCombo.ComboKeypress(this, cg, strCombo, "", 2);
                }
                 label5.Text = selected;
            }
            if (selected != "")
            {
                double com1 = 0;
                double com2 = 0;
                double sum = 0;
                string vid = IsDocumentNumber(selected);
                dt = new DataTable();
                Database.GetSqlData("select * from voucherdet where Vi_id='" + vid + "'", dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    com1 = double.Parse(dt.Rows[i]["Quantity"].ToString()) * double.Parse(dt.Rows[i]["Commission@"].ToString());

                    com2 = (double.Parse(dt.Rows[i]["Amount"].ToString()) * double.Parse(dt.Rows[i]["Commission%"].ToString())) / 100;

                    sum = sum + com1 + com2;
                }

                double amt = 0;

                textBox5.Text = funs.Select_ac_nm(Database.GetScalarText("select Conn_id from voucherinfo where Vi_id='" + vid + "'"));

                amt = double.Parse(Database.GetScalarDecimal("select cmsnAmt from voucherinfo where Vi_id='" + vid + "'").ToString());
                if (amt > 0)
                {
                    textBox4.Text = amt.ToString();
                }
                else
                {
                    textBox4.Text = sum.ToString();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void frm_search_Load(object sender, EventArgs e)
        {
            dateTimePicker1.CustomFormat = Database.dformat;
            dateTimePicker2.CustomFormat = Database.dformat;
            dateTimePicker1.MinDate = Database.stDate;
            dateTimePicker1.MaxDate = Database.ldate;
            dateTimePicker2.MinDate = Database.stDate;
            dateTimePicker2.MaxDate = Database.ldate;
            dateTimePicker1.Value = Database.ldate;
            dateTimePicker2.Value = Database.ldate;
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
           // strCombo = "SELECT ACCOUNT.Name FROM  ACCOUNT LEFT OUTER JOIN  ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id WHERE (ACCOUNTYPE.Name = 'Agent') AND (ACCOUNT.Branch_id = '" + Database.BranchId + "') ORDER BY ACCOUNT.Name";
            strCombo = "SELECT ACCOUNT.Name, dbo.ACCOUNTYPE.Name AS Acctype FROM ACCOUNT LEFT OUTER JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id WHERE     (ACCOUNT.Branch_id = '" + Database.BranchId + "') AND (dbo.ACCOUNTYPE.Path LIKE '1;39;43;%') AND (dbo.ACCOUNT.Status = 'true') ORDER BY dbo.ACCOUNT.Name";
            textBox5.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string vid = IsDocumentNumber(selected);

            DataTable dttemp = new DataTable("voucherinfo");
            Database.GetSqlData("select * from voucherinfo where vi_id='" + vid + "'", dttemp);
            string vtid = dttemp.Rows[0]["vt_id"].ToString();

            dttemp.Rows[0]["cmsnAmt"] = double.Parse(textBox4.Text);
            dttemp.Rows[0]["Conn_id"] = funs.Select_ac_id(textBox5.Text);
            string Transdocno = dttemp.Rows[0]["transdocno"].ToString();
            if (Transdocno == "")
            {
                dttemp.Rows[0]["Transdocdate"] = DateTime.Parse(dttemp.Rows[0]["vdate"].ToString());
            }
           
            Database.SaveData(dttemp);

            DataTable dt = new DataTable("Journal");
            Database.GetSqlData("select * from journal where vi_id='" + vid + "' and sno=10002", dt);
            //bool A = Database.GetScalarBool("Select A from Vouchertype where vt_id='" + vtid + "'");
            //bool B = Database.GetScalarBool("Select B from Vouchertype where vt_id='" + vtid + "'");
            //bool AB = Database.GetScalarBool("Select AB from Vouchertype where vt_id='" + vtid + "'");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i].Delete();
            }
            Database.SaveData(dt);

            dt = new DataTable("Journal");
            Database.GetSqlData("select * from journal where vi_id='" + vid + "' and sno=10002", dt);

            dt.Rows.Add();
            dt.Rows[dt.Rows.Count - 1]["Ac_id"] = "MAN1";
            dt.Rows[dt.Rows.Count - 1]["Amount"] = double.Parse(textBox4.Text);
            dt.Rows[dt.Rows.Count - 1]["Sno"] = 10002;
            dt.Rows[dt.Rows.Count - 1]["Vi_id"] = vid;
            dt.Rows[dt.Rows.Count - 1]["Vdate"] = DateTime.Parse(dttemp.Rows[0]["Vdate"].ToString());
            dt.Rows[dt.Rows.Count - 1]["Narr"] = "Commission Due";
            dt.Rows[dt.Rows.Count - 1]["locationId"] = Database.LocationId;
            dt.Rows[dt.Rows.Count - 1]["Opp_acid"] = dttemp.Rows[0]["Ac_id"].ToString();
            dt.Rows[dt.Rows.Count - 1]["A"] = false;
            dt.Rows[dt.Rows.Count - 1]["B"] = true;
            dt.Rows[dt.Rows.Count - 1]["AB"] = true;

            string id = Database.GetScalarText("select Ac_id from Account where name='" + textBox5.Text + "'");
            dt.Rows.Add();
            dt.Rows[dt.Rows.Count - 1]["Ac_id"] = id;
            dt.Rows[dt.Rows.Count - 1]["Amount"] = -1 * double.Parse(textBox4.Text);
            dt.Rows[dt.Rows.Count - 1]["Sno"] = 10002;
            dt.Rows[dt.Rows.Count - 1]["Vi_id"] = vid;
            dt.Rows[dt.Rows.Count - 1]["Vdate"] = DateTime.Parse(dttemp.Rows[0]["Vdate"].ToString());
            dt.Rows[dt.Rows.Count - 1]["Narr"] = "Commission Due";
            dt.Rows[dt.Rows.Count - 1]["locationId"] = Database.LocationId;
            dt.Rows[dt.Rows.Count - 1]["Opp_acid"] = dttemp.Rows[0]["Ac_id"].ToString();
            dt.Rows[dt.Rows.Count - 1]["A"] = false;
            dt.Rows[dt.Rows.Count - 1]["B"] = true;
            dt.Rows[dt.Rows.Count - 1]["AB"] = true;
            Database.SaveData(dt);

            funs.ShowBalloonTip("Saved", "Saved Successfully");
            this.Close();
            this.Dispose();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string vid = IsDocumentNumber(label5.Text);
            frm_printcopy frm = new frm_printcopy("View", vid, funs.Select_vt_id_vid(vid));
            frm.Show();
        }
    }
}
