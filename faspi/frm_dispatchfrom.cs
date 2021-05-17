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
    public partial class frm_dispatchfrom : Form
    {
        public string dispatchfrom = "", gstationid = "", gcityid = "";
        public string gtype = "", gPrintname = "", gAddress1 = "", gAddress2 = "", gEmail = "", gTin = "", gContact = "", gStateid = "", gPAN = "", gAadhar = "", gPincode = "";
        int gac_id = 0;
        string gvid = "";
        public bool gExstate = false;

        public frm_dispatchfrom(string type, string vid, string dispatchfromacc)
        {
            InitializeComponent();
            gtype = type;
            dispatchfrom = dispatchfromacc;
            gvid = vid;
        }

        private void frm_dispatchfrom_Load(object sender, EventArgs e)
        {
            textBox14.Text = funs.Select_ac_nm(dispatchfrom);
            textBox6.Text = funs.Select_TIN(textBox14.Text);
        }

        private void frm_dispatchfrom_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {


                dispatchfrom = funs.Select_ac_id(textBox14.Text);
               
                this.Close();
                this.Dispose();
            }
        }

        private void textBox14_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strCombo = "";

            if ((gtype == "Purchase" || gtype == "P Return" || gtype == "receive" || gtype == "RCM" || gtype == "Sale Order" || gtype == "JReceive"))
            {
                //multiloc
                //  strCombo = funs.GetStrCombonew(" (Path LIKE '1;39;%') OR  (Path LIKE '1;38;%') OR  (Path LIKE '1;3;%')   or   (Path LIKE '8;40;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ") or   (Path LIKE '8;39;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ") ", " HAVING  Status=" + access_sql.Singlequote + "true" + access_sql.Singlequote+" and Branch_id="+Database.BranchId);
                strCombo = funs.GetStrCombonew(" (Path LIKE '1;39;%') OR  (Path LIKE '1;38;%') OR  (Path LIKE '1;3;%')   or   (Path LIKE '8;40;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ") or   (Path LIKE '8;39;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ") ", " HAVING  Status=" + access_sql.Singlequote + "true" + access_sql.Singlequote);


            }

            else if ((gtype == "Sale" || gtype == "Return" || gtype == "issue" ||  gtype == "PWDebitNote" || gtype == "JIssue"))
            {
                //multiloc
                //strCombo = funs.GetStrCombonew(" (Path LIKE '8;40;%') OR  (Path LIKE '8;39;%') OR  (Path LIKE '1;3;%')   or   (Path LIKE '1;39;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ")  or   (Path LIKE '1;38;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ") ", " HAVING  Status=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " and Branch_id=" + Database.BranchId);
               // strCombo = funs.GetStrCombonew(" (Path LIKE '8;40;%') OR  (Path LIKE '8;39;%') OR  (Path LIKE '1;3;%')   or   (Path LIKE '1;39;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ")  or   (Path LIKE '1;38;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ") ", " HAVING  Status=" + access_sql.Singlequote + "true" + access_sql.Singlequote);
                strCombo = funs.GetStrCombonew(" (Path LIKE '1;39;%') or (Path LIKE '1;38;%')  OR  (Path LIKE '1;3;%')   or   (Path LIKE '8;40;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ")  or   (Path LIKE '8;39;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ") ", "   Status=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " and Branch_id='" + Database.BranchId + "' ");
            }

          
            textBox14.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);

            if (textBox14.Text != "")
            {
                textBox6.Text = funs.Select_TIN(textBox14.Text);


            }
            else
            {
                textBox6.Text = "";

            }
        }

        private void textBox14_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox14);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dispatchfrom = funs.Select_ac_id(textBox14.Text);
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
                textBox14.Text = funs.EditAccount(textBox14.Text);
            }
            if (textBox14.Text != "")
            {
                textBox6.Text = funs.Select_TIN(textBox14.Text);

            }
            else
            {
                textBox6.Text = "";

            }
        }
    }
}
