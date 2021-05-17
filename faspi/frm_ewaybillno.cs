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
    public partial class frm_ewaybillno : Form
    {
        public string gEwayBillno = "", gtransportname="",gtransdocno="",gvehicleno="";
        public double gdistance=0;
        public DateTime gtransdocdate;
        public frm_ewaybillno(string transportname,string transdocno,DateTime transdocdate,string vehicleno,double distance,string EwayBillno)
        {
            InitializeComponent();

            dateTimePicker1.MinDate = Database.stDate;
            dateTimePicker1.MaxDate = Database.ldate;
            dateTimePicker1.CustomFormat = Database.dformat;
            gEwayBillno = EwayBillno;
            gtransportname = transportname;
            gtransdocno = transdocno;
            gvehicleno = vehicleno;
            gtransdocdate = transdocdate;
            gdistance=distance;


            textBox31.Text = gtransportname;
            textBox32.Text = funs.DecimalPoint(gdistance,2);
            textBox2.Text = gtransdocno;
            textBox3.Text = gvehicleno;
            textBox1.Text = gEwayBillno;
            dateTimePicker1.Value = DateTime.Parse(gtransdocdate.ToString(Database.dformat));
            textBox31.Focus();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            gEwayBillno = textBox1.Text;
            if (textBox32.Text.Trim() == "")
            {
                textBox32.Text = "0";
            }
                 gdistance = double.Parse(textBox32.Text);
            gtransportname = textBox31.Text;
            gvehicleno = textBox3.Text;
            gtransdocno = textBox2.Text;
            gtransdocdate = dateTimePicker1.Value;
            this.Close();
            this.Dispose();
        }

        private void frm_ewaybillno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
               
                this.Close();
            }
        }

        private void frm_ewaybillno_Load(object sender, EventArgs e)
        {

        }

        private void textBox31_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strcombo = "SELECT ACCOUNT.Name FROM ACCOUNT LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id WHERE (((ACCOUNTYPE.RefineName)='Transport')) ";

            textBox31.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strcombo, textBox31.Text, 0);
        }

        private void textBox31_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox31.Text != "" || textBox31.Text != null)
                {
                    textBox31.Text = funs.EditAccount(textBox31.Text);

                }
            }

            else if (e.Control && e.KeyCode == Keys.C)
            {

                textBox31.Text = funs.AddAccount();

            }
            //SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox32_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }
    }
}
