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
    public partial class Merge : Form
    {
        string strCombo = "";
        public string mode = "";
        public Merge()
        {
            InitializeComponent();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (mode == "2Packings")
            {
                strCombo = "select distinct description from description ";
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
            }
            else
            {
                strCombo = "SELECT Distinct Pack FROM Description order by Pack";
                textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0); 
            }
        }

        private void Clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";        
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (mode == "2Packings")
            {
                string des_idfrom = funs.Select_des_id(textBox1.Text, textBox2.Text);
                string des_idTo = funs.Select_des_id(textBox1.Text, textBox3.Text);
                if (des_idfrom != "" && des_idTo != "")
                {
                    Database.CommandExecutor("Update VOUCHERDET set Des_ac_id='" + des_idTo + "', Description='" + textBox1.Text + "'  where Des_ac_id='" + des_idfrom + "' ");
                    Database.CommandExecutor("Update Stock set Did='" + des_idTo + "'  where Did='" + des_idfrom + "' ");
                    if (Database.GetScalarInt("Select  Des_ac_id from voucherdet where Des_ac_id='" + des_idfrom+"' ") == 0)
                    {
                        Database.CommandExecutor("Delete from Description where Des_id='" + des_idfrom + "' ");
                    }

                    MessageBox.Show("Merge Items Successfully");
                    Clear();
                }
                else
                {
                    MessageBox.Show("Select Item with Packings.");
                }
            }
            else
            {
                string des_idfrom = funs.Select_des_id(textBox2.Text, textBox1.Text);
                string des_idTo = funs.Select_des_id(textBox3.Text, textBox1.Text);
                if (des_idfrom != "" && des_idTo != "")
                {
                    Database.CommandExecutor("Update VOUCHERDET set Des_ac_id='" + des_idTo + "', Description='" + textBox3.Text + "'  where Des_ac_id='" + des_idfrom + "' ");
                    Database.CommandExecutor("Update Stock set Did='" + des_idTo + "'  where Did='" + des_idfrom + "' ");
                    if (Database.GetScalarInt("Select Des_ac_id from voucherdet where Des_ac_id='" + des_idfrom+"' ") == 0)
                    {
                        Database.CommandExecutor("Delete from Description where Des_id='" + des_idfrom + "' ");
                    }
                    MessageBox.Show("Merge Packings Successfully");
                    Clear();
                }
                else
                {
                    MessageBox.Show("Select Packing with Items.");
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (mode == "2Packings")
            {
                strCombo = "SELECT PACK from Description WHERE (((DESCRIPTION.Description)='" + textBox1.Text + "'))";
                textBox2.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
            }
            else
            {
                strCombo = "SELECT DESCRIPTION.Description FROM Description WHERE (((PACK)='" + textBox1.Text + "'))";
                textBox2.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
            }             
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (mode == "2Packings")
            {
                strCombo = "SELECT PACK from Description WHERE (((DESCRIPTION.Description)='" + textBox1.Text + "'))";
                textBox3.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
            }
            else
            {
                strCombo = "SELECT DESCRIPTION.Description FROM Description WHERE (((PACK)='" + textBox1.Text + "'))";
                textBox3.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
            }
        }
        private void Displaysetting()
        {
            if (mode == "2Packings")
            {
                groupBox1.Text = "Display Name";
                groupBox2.Text = "Packings";
            }
            else if (mode == "2DisplayName")
            {
                groupBox1.Text = "Packing";
                groupBox2.Text = "Display Name";
            }
        }

        private void Merge_Load(object sender, EventArgs e)
        {
            Displaysetting();
        }
    }
}
