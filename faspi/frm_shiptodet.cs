using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace faspi
{
    public partial class frm_shiptodet : Form
    {
        public string shipto = "",gcityid="";
        public string gtype = "", gPrintname = "", gAddress1 = "", gAddress2 = "", gEmail = "", gTin = "", gContact = "", gState = "", gPAN = "", gAadhar = "", gPincode = "";
        int gac_id = 0;
        string gvid = "";
        public bool gExstate = false;



        public frm_shiptodet(string type, string vid, string Shiptoacc, string printname, string address1, string address2, string contact, string email, string tin, string state, string PAN, string Aadhar, bool Exstate, string Pincode, string cityid)
        {
            InitializeComponent();
            shipto = Shiptoacc;
            gvid = vid;
            gtype = type;
            gPrintname = printname;
            gAddress1 = address1;
            gAddress2 = address2;
            gEmail = email;
            gTin = tin;
            gContact = contact;
            gState = state;
            gPAN = PAN;
            gAadhar = Aadhar;
            gExstate = Exstate;
            gcityid = cityid;
            gPincode = Pincode;
        }

        private void frm_shiptodet_Load(object sender, EventArgs e)
        {

            if (Feature.Available("Taxation Applicable") == "VAT")
            {
                 label7.Text = "TIN";
            }
            else
            {
                label7.Text = "GSTN";
            }
            textBox10.Text = funs.Select_city_name(gcityid);
            textBox11.Text = gPincode;
            textBox14.Text = funs.Select_ac_nm(shipto);
            textBox1.Text = gPrintname;
            textBox2.Text = gAddress1;
            textBox3.Text = gAddress2;
            textBox4.Text = gContact;
            textBox5.Text = gEmail;
            textBox6.Text = gTin;
            textBox7.Text = gState;
            textBox9.Text = gAadhar;
            textBox8.Text = gPAN;

        }

        private void frm_shiptodet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {

                gcityid = funs.Select_city_id(textBox10.Text);
                gPincode = textBox11.Text;
                shipto = funs.Select_ac_id(textBox14.Text);
                gPrintname = textBox1.Text;
                gAddress1 = textBox2.Text;
                gAddress2 = textBox3.Text;
                gContact = textBox4.Text;
                gEmail = textBox5.Text;
                gTin = textBox6.Text;
                gState = textBox7.Text;
                gAadhar = textBox9.Text;
                gPAN = textBox8.Text;
                this.Close();
                this.Dispose();
            }
        }

        private void textBox14_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strCombo = "";

            if ((gtype == "Sale" || gtype == "Return" || gtype == "Pending" || gtype == "issue" || gtype == "Sale Order"))
            {
                strCombo = funs.GetStrCombonew(" (Path LIKE '1;39;%') or (Path LIKE '1;38;%')  OR  (Path LIKE '1;3;%')   or   (Path LIKE '8;40;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ")  or   (Path LIKE '8;39;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ") ", "   Status=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " and Branch_id='" + Database.BranchId + "' ");
                //strCombo = funs.GetStrCombonew(" (Path LIKE '1;39;%') OR  (Path LIKE '1;38;%') OR  (Path LIKE '1;3;%')   or   (Path LIKE '8;40;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ") or   (Path LIKE '8;39;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ") ", " HAVING  Status=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " and Branch_id=" + Database.BranchId);
            }

            else if ((gtype == "Purchase" || gtype == "P Return" || gtype == "receive" || gtype == "RCM" || gtype == "PWDebitNote"))
            {
                strCombo = funs.GetStrCombonew(" (Path LIKE '8;40;%') OR  (Path LIKE '1;3;%')  or   (Path LIKE '8;39;%')   or   (Path LIKE '1;39;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ") or   (Path LIKE '1;38;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ") ", "   Status=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " and Branch_id='" + Database.BranchId + "' ");
                //strCombo = funs.GetStrCombonew(" (Path LIKE '8;40;%') OR  (Path LIKE '8;39;%') OR  (Path LIKE '1;3;%')   or   (Path LIKE '1;39;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ")  or   (Path LIKE '1;38;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ") ", " HAVING  Status=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " and Branch_id=" + Database.BranchId);
            }

            else if (gtype == "Opening")
            {
                strCombo = "select distinct '<MAIN>' as name from account union all SELECT ACCOUNT.Name as name FROM ACCOUNT LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id WHERE (((ACCOUNTYPE.Name)='Godown')) GROUP BY ACCOUNT.Name";
            }
            textBox14.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
            textBox1.Text = funs.Select_Print(textBox14.Text);

            textBox2.Text = funs.Select_Address1(textBox14.Text);

            textBox10.Text = funs.Select_city_name(funs.Select_ac_City_id(textBox14.Text));
            textBox11.Text = funs.Select_Pincode(textBox14.Text);
            textBox3.Text = funs.Select_Address2(textBox14.Text);
            textBox4.Text = funs.Select_Mobile(textBox14.Text);
            textBox5.Text = funs.Select_Email(textBox14.Text);
            textBox6.Text = funs.Select_TIN(textBox14.Text);

            textBox8.Text = funs.Select_PAN(textBox14.Text);
            textBox9.Text = funs.Select_AAdhar(textBox14.Text); ;

            textBox7.Text = funs.Select_state_nm(funs.Select_ac_state_id(textBox14.Text).ToString());

        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strCombo = "";
            strCombo = "select Sname As State from State order by Sname";


            textBox7.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox14_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox14);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox3);
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox4);
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox5);
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox6);
        }

        private void textBox8_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox8);
        }

        private void textBox9_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox9);
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox7);
        }

        private void textBox14_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox14);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
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

        private void textBox5_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox5);
        }

        private void textBox6_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox6);
        }

        private void textBox8_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox8_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox8);
        }

        private void textBox9_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox9);
        }

        private void textBox7_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox7);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox9_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (Feature.Available("Taxation Applicable") != "VAT")
            {
                Regex obj = new Regex("^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[0-9A-Z]{1}Z[0-9A-Z]{1}$");
                if (textBox6.Text.Trim() == "" || textBox6.Text == "0")
                {
                    textBox6.Text = "0";

                }
                else if (obj.IsMatch(textBox6.Text) == false)
                {
                    MessageBox.Show("GSTIN is Not Correct");
                    return;
                }
            }
            gcityid = funs.Select_city_id(textBox10.Text);
            gPincode = textBox11.Text;
            shipto = funs.Select_ac_id(textBox14.Text);
            gPrintname = textBox1.Text;
            gAddress1 = textBox2.Text;
            gAddress2 = textBox3.Text;
            gContact = textBox4.Text;
            gEmail = textBox5.Text;
            gTin = textBox6.Text;
            gState = textBox7.Text;
            gAadhar = textBox9.Text;

            gPAN = textBox8.Text;

            string stateid = "";
            //stateid = funs.Select_ac_state_id(textBox14.Text);
            stateid = funs.Select_state_id(textBox7.Text);

            if (stateid == "")
            {
                stateid = Database.CompanyState_id;
            }



            if (Database.CompanyState_id == stateid)
            {
                gExstate = false;

            }
            else
            {
                gExstate = true;
            }
            this.Close();
            this.Dispose();
        }

        private void textBox14_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                textBox14.Text = funs.AddAccount();

            }
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox14.Text != "")
                {
                    textBox14.Text = funs.EditAccount(textBox14.Text);
                }
            }
        }

        private void textBox10_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox10);
        }

        private void textBox11_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox11);
        }

        private void textBox11_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strcombo = "Select cname as City from city order by Cname";

            textBox10.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strcombo, textBox10.Text, 0);
        }

        private void textBox10_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox10);
        }

        private void textBox11_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox11);
        }

        private void textBox10_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                textBox10.Text = funs.AddCity();

            }
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox10.Text != "")
                {
                    textBox10.Text = funs.EditCity(textBox10.Text);
                }
            }
        }
    }
}
