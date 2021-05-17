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
    public partial class frm_updateHSN : Form
    {
        string strCombo;
        public string mode = "";
        public frm_updateHSN()
        {
            InitializeComponent();
        }
        private void SideFill()
        {
            flowLayoutPanel1.Controls.Clear();
            DataTable dtsidefill = new DataTable();
            dtsidefill.Columns.Add("Name", typeof(string));
            dtsidefill.Columns.Add("DisplayName", typeof(string));
            dtsidefill.Columns.Add("ShortcutKey", typeof(string));
            dtsidefill.Columns.Add("Visible", typeof(bool));
            //save
            dtsidefill.Rows.Add();
            dtsidefill.Rows[0]["Name"] = "save";
            dtsidefill.Rows[0]["DisplayName"] = "Save";
            dtsidefill.Rows[0]["ShortcutKey"] = "^S";
            dtsidefill.Rows[0]["Visible"] = true;



            //close
            dtsidefill.Rows.Add();
            dtsidefill.Rows[1]["Name"] = "quit";
            dtsidefill.Rows[1]["DisplayName"] = "Quit";
            dtsidefill.Rows[1]["ShortcutKey"] = "Esc";
            dtsidefill.Rows[1]["Visible"] = true;






            for (int i = 0; i < dtsidefill.Rows.Count; i++)
            {


                if (bool.Parse(dtsidefill.Rows[i]["Visible"].ToString()) == true)
                {

                    Button btn = new Button();
                    btn.Size = new Size(150, 30);
                    btn.Name = dtsidefill.Rows[i]["Name"].ToString();
                    btn.Text = "";


                    Bitmap bmp = new Bitmap(btn.ClientRectangle.Width, btn.ClientRectangle.Height);
                    Graphics G = Graphics.FromImage(bmp);
                    G.Clear(btn.BackColor);
                    string line1 = dtsidefill.Rows[i]["ShortcutKey"].ToString();
                    string line2 = dtsidefill.Rows[i]["DisplayName"].ToString();

                    StringFormat SF = new StringFormat();
                    SF.Alignment = StringAlignment.Near;
                    SF.LineAlignment = StringAlignment.Center;
                    Rectangle RC = btn.ClientRectangle;
                    Font font = new Font("Arial", 12);
                    G.DrawString(line1, font, Brushes.Red, RC, SF);
                    G.DrawString("".PadLeft(line1.Length * 2 + 1) + line2, font, Brushes.Black, RC, SF);

                    btn.Image = bmp;

                    btn.Click += new EventHandler(btn_Click);
                    flowLayoutPanel1.Controls.Add(btn);
                }

            }


        }
        void btn_Click(object sender, EventArgs e)
        {
            Button tbtn = (Button)sender;
            string name = tbtn.Name.ToString();

            if (name == "save")
            {
                if (validate() == true)
                {
                    try
                    {
                        Database.BeginTran();
                        save();
                        Database.CommitTran();
                        MessageBox.Show("Update Successfully...");
                        textBox1.Text = "";
                        textBox2.Text = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Not Updated. Any Error is Found.");
                        Database.RollbackTran();
                    }
                    Master.UpdateDecription();
                    Master.UpdateDecriptionInfo();
                    Master.UpdateTaxCategory();
                }
              

            }




            if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }







        }

        private void frm_updateHSN_Load(object sender, EventArgs e)
        {
          //  this.Size = this.MdiParent.Size;
            if (mode == "Merge")
            {
                groupBox1.Text = "Merge";
                groupBox3.Text = "Merge From";
            }
            else
            {
                groupBox1.Text = "Shift";
                groupBox3.Text = "Shift From";
            }
            SideFill();
        }

        private void frm_updateHSN_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
            if (e.Control && e.KeyCode == Keys.S)
            {
                if (validate() == true)
                {
                    try
                    {
                        Database.BeginTran();
                        save();
                        Database.CommitTran();
                        MessageBox.Show("Update Successfully...");
                        textBox1.Text = "";
                        textBox2.Text = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Not Updated. Any Error is Found.");
                        Database.RollbackTran();
                    }
                    Master.UpdateDecription();
                    Master.UpdateDecriptionInfo();
                    Master.UpdateTaxCategory();
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "SELECT TAXCATEGORY.Category_Name as [HSN Name], TAXCATEGORY.Commodity_Code as [HSN Code] FROM TAXCATEGORY ORDER BY TAXCATEGORY.Category_Name";
            textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "SELECT TAXCATEGORY.Category_Name as [HSN Name], TAXCATEGORY.Commodity_Code as [HSN Code] FROM TAXCATEGORY ORDER BY TAXCATEGORY.Category_Name";
            textBox2.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void save()
        {

            string update = funs.Select_tax_cat_id(textBox1.Text);
            string updatefrom = funs.Select_tax_cat_id(textBox2.Text);

            Database.CommandExecutor("Update Description set Tax_Cat_id='" + update + "', Modifiedby='"+Database.user_id+"' where Tax_Cat_id='" + updatefrom + "' ");
            if (mode == "Merge")
            {
                Database.CommandExecutor("Update Voucherdet set Category_Id='" + update + "'  where Category_Id='" + updatefrom + "' ");
                Database.CommandExecutor("Delete from TaxCategory where Category_Id='" + updatefrom + "' ");
            }
               
           
        }

        private bool validate()
        {

            if (textBox1.Text.Trim() == "")
            {
                textBox1.Focus();
                return false;
            }



            if (textBox2.Text.Trim() == "")
            {
                textBox2.Focus();
                return false;
            }

            return true;
        }


    }
}
