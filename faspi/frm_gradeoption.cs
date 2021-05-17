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
    public partial class frm_gradeoption : Form
    {
        public string acctype = "";
        public frm_gradeoption()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string str = "";
            if (checkBox1.Checked == true)
            {
                //sql += " and ";
                //sql += " Voucherinfo.Branch_id='" + funs.Select_branch_id(textBox1.Text) + "' ";
                if (str == "")
                {

                }

                str = " grade='"+ checkBox1.Text+"'";

            }

            if (checkBox2.Checked == true)
            {
                if (str == "")
                {

                }
                else
                {
                    str += " or ";
                }
                str += " grade='"+ checkBox2.Text+"'";


            }

            if (checkBox3.Checked == true)
            {
                if (str == "")
                {

                }
                else
                {
                    str += " or ";
                }
                str += "  grade='" + checkBox3.Text + "'";

            }

            if (checkBox4.Checked == true)
            {
                if (str == "")
                {

                }
                else
                {
                    str += " or ";
                }
                str += "  grade='" + checkBox4.Text + "'";

            }

            if (checkBox5.Checked == true)
            {
                if (str == "")
                {

                }
                else
                {
                    str += " or ";
                }
                str += "  grade=''";

            }



            if (str != "")
            {

                //string strcombo = "SELECT Name FROM  ACCOUNTYPE WHERE  (Type = 'Account') ORDER BY Name";
                //char cg = 'a';
                //string selected = SelectCombo.ComboKeypress(this, cg, strcombo, "", 1);
                Report gg = new Report();
                gg.MdiParent = this.MdiParent;
                gg.Gradewise(Database.stDate, Database.ldate, str, acctype);
                gg.Show();

                this.Close();
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Please select atleast one");

            }

        
        }
    }
}
