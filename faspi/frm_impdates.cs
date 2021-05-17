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
    public partial class frm_impdates : Form
    {
        DataTable dtimpdates = new DataTable("importantdate");
        string sql = "";
        public string gStr = "";

        public frm_impdates()
        {
            InitializeComponent(); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Validate() == true)
            {
                Save();
                if (gStr == "0")
                {
                    Loaddata("0", "ReminderDates");
                    dateTimePicker1.Focus();
                }
                else
                {
                    this.Close();
                    this.Dispose();
                }
            }
        }

        private void Save()
        {
            dtimpdates.Rows[0]["idate"] = dateTimePicker1.Value.Date.ToString(Database.dformat);
            dtimpdates.Rows[0]["Ac_id"] = funs.Select_ac_id(textBox14.Text);
            dtimpdates.Rows[0]["Title"] = textBox1.Text;
            dtimpdates.Rows[0]["Amount"] = textBox2.Text;
            dtimpdates.Rows[0]["Note"] = textBox3.Text;
            dtimpdates.Rows[0]["LocationId"] = Database.LocationId;
            Database.SaveData(dtimpdates);
            funs.ShowBalloonTip("Saved", "Saved Successfully");
 
        }
        private bool Validate()
        {
            if (textBox14.Text == "")
            {
                textBox14.Focus();
                MessageBox.Show("Enter Associated A/c");
                return false;
            }
            return true;
        }     

        public void Loaddata(String str, String frmCaption)
        {
            gStr = str;
            Database.GetSqlData("select * from importantdate where id=" + int.Parse(str), dtimpdates);
            this.Text = frmCaption;
            if (dtimpdates.Rows.Count == 0)
            {
                dtimpdates.Rows.Add(0);
                textBox14.Text = "";
                textBox1.Text = "";
                textBox2.Text = "0";
                textBox3.Text = "";
                dateTimePicker1.Value = Database.ldate;
            }
            else
            {
                dateTimePicker1.Value =  DateTime.Parse(dtimpdates.Rows[0]["idate"].ToString());
                textBox1.Text = dtimpdates.Rows[0]["Title"].ToString();
                textBox14.Text = funs.Select_ac_nm(dtimpdates.Rows[0]["Ac_id"].ToString());
                textBox2.Text = dtimpdates.Rows[0]["Amount"].ToString();
                textBox3.Text = dtimpdates.Rows[0]["Note"].ToString();
            }
        }

        private void textBox14_KeyPress(object sender, KeyPressEventArgs e)
        {
            sql = funs.GetStrCombo("in(4,5,23,3,13,22,11,20,5)");
            textBox14.Text = SelectCombo.ComboKeypress(this, e.KeyChar, sql, e.KeyChar.ToString(), 1);
            if (textBox1.Text == "")
            {
                textBox1.Text = textBox14.Text;
            }
        }

        private void dateTimePicker1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(dateTimePicker1);
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

        private void dateTimePicker1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(dateTimePicker1);
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
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

        private void frm_impdates_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                button1_Click(sender, e);               
            }
            else if (e.KeyCode == Keys.Escape)
            {
                if (textBox14.Text != "")
                {
                    DialogResult chk = MessageBox.Show("Are u sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (chk == DialogResult.No)
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        this.Dispose();
                        this.Close();
                    }
                }
                else
                {
                    this.Dispose();
                    this.Close();
                }
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }
    }
}
