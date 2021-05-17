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
    public partial class frmModifyGroup : Form
    {
        String strCombo;
        public frmModifyGroup()
        {
            InitializeComponent();
        }

        private void frmModifyGroup_Load(object sender, EventArgs e)
        {
            if (Database.IsKacha == true)
            {
                groupBox13.Visible = true;
            }
            this.Size = this.MdiParent.Size;
            SideFill();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBox1.Enabled = true;
                textBox1.Focus();
            }
            else
            {
                textBox1.Enabled = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                textBox2.Enabled = true;
                textBox2.Focus();
            }
            else
            {
                textBox2.Enabled = false;
            }           
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                textBox3.Enabled = true;
                textBox3.Focus();
            }
            else
            {
                textBox3.Enabled = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                textBox10.Enabled = true;
                textBox10.Focus();
            }
            else
            {
                textBox10.Enabled = false;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked == true)
            {
                textBox11.Enabled = true;
                textBox11.Focus();
            }
            else
            {
                textBox11.Enabled = false;
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox9.Checked == true)
            {
                textBox12.Enabled = true;
                textBox12.Focus();
            }
            else
            {
                textBox12.Enabled = false;
            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked == true)
            {
                textBox6.Enabled = true;
                textBox6.Focus();
            }
            else
            {
                textBox6.Enabled = false;
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked == true)
            {
                textBox5.Enabled = true;
                textBox5.Focus();
            }
            else
            {
                textBox5.Enabled = false;
            } 
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked == true)
            {
                textBox14.Enabled = true;
                textBox14.Focus();
            }
            else
            {
                textBox14.Enabled = false;
            }
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox11.Checked == true)
            {
                textBox4.Enabled = true;
                textBox4.Focus();
            }
            else
            {
                textBox4.Enabled = false;
            }
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox12.Checked == true)
            {
                textBox7.Enabled = true;
                textBox7.Focus();
            }
            else
            {
                textBox7.Enabled = false;
            }
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox14.Checked == true)
            {
                textBox8.Enabled = true;
                textBox9.Enabled = true;
                textBox8.Focus();
            }
            else
            {
                textBox8.Enabled = false;
                textBox9.Enabled = false;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            t5.Enabled = true;
            t5.Focus();
        }

        private void s1_CheckedChanged(object sender, EventArgs e)
        {
            t1.Enabled = true;
            t1.Focus();
        }

        private void s2_CheckedChanged(object sender, EventArgs e)
        {
            t2.Enabled = true;
            t2.Focus();
        }

        private void s3_CheckedChanged(object sender, EventArgs e)
        {
            t3.Enabled = true;
            t3.Focus();
        }

        private void s4_CheckedChanged(object sender, EventArgs e)
        {
            t4.Enabled = true;
            t4.Focus();
        }

        private String whereSql()
        { 
            String st1=", state='Modified' ",st2;
            bool chk = false;
            if (s1.Checked == true)
            {
                if (chk == false)
                {
                    chk = true;
                    st2 = " Where ";
                }
                else
                {
                    st2 = " and ";
                }
                st1 = st1 + st2 + "Pack ='" + t5.Text +"' ";
            }
            if (s2.Checked == true)
            {
                if (chk == false)
                {
                    chk = true;
                    st2 = " Where ";
                }
                else
                {
                    st2 = " and ";
                }
                st1 = st1 + st2 + "Company_id ='" + funs.Select_oth_id(t1.Text)+"' ";
            }
            if (s3.Checked == true)
            {
                if (chk == false)
                {
                    chk = true;
                    st2 = " Where ";
                }
                else
                {
                    st2 = " and ";
                }
                st1 = st1 + st2 + "Item_id ='" + funs.Select_oth_id(t2.Text) + "' ";
            }
            if (s4.Checked == true)
            {
                if (chk == false)
                {
                    chk = true;
                    st2 = " Where ";
                }
                else
                {
                    st2 = " and ";
                }
                st1 = st1 + st2 + "Col_id ='" + funs.Select_oth_id(t3.Text) + "' ";
            }
            if (s5.Checked == true)
            {
                if (chk == false)
                {
                    chk = true;
                    st2 = " Where ";
                }
                else
                {
                    st2 = " and ";
                }
                st1 = st1 + st2 + "Group_id ='" + funs.Select_oth_id(t4.Text) + "' ";
            }
            return st1;
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void t5_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select distinct pack from Description order by pack";
            t5.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void t1_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "Select [Name] from OTHER  where type ='SER14' order by [Name]";
            t1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void t2_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "Select [Name] from OTHER  where type ='SER15' order by [Name]";
            t2.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void t3_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "Select [Name] from OTHER  where type = 'SER18' order by [Name]";
            t3.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);            
        }

        private void t4_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "Select [Name] from OTHER  where type = 'SER16' order by [Name]";
            t4.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }
       
        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox10_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox11_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox14_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select TAXCATEGORY.Category_Name From TAXCATEGORY ORDER BY TAXCATEGORY.Category_Name";
            textBox14.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void radioButton1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void s1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void a1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void s2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void a2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void s3_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void a3_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void s4_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void a4_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void s5_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void checkBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void checkBox2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void checkBox3_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void checkBox4_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void checkBox5_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void checkBox10_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void checkBox9_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void checkBox8_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void checkBox7_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void checkBox6_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void checkBox11_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void checkBox13_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void radioButton3_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void checkBox14_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox8_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox9_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            t5.Enabled = false;
        }

        private void a2_CheckedChanged(object sender, EventArgs e)
        {
            t2.Enabled = false;
        }

        private void a3_CheckedChanged(object sender, EventArgs e)
        {
            t3.Enabled = false;
        }

        private void a4_CheckedChanged(object sender, EventArgs e)
        {
            t4.Enabled = false;
        }

        private void tabControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                if (tabControl1.SelectedIndex == 1)
                {
                    tab1_save();
                }
                else if (tabControl1.SelectedIndex == 2)
                {
                    tab2_save();
                }
            }
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult chk = MessageBox.Show("Are u sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (chk == DialogResult.No)
                {
                    e.Handled = false;
                }
                else
                {
                    this.Dispose();
                }
            }
        }

        private void tab1_save()
        {
            String sql1 = "UPDATE DESCRIPTION SET ", sql2;
            bool chk = false;

            if (checkBox3.Checked == true)
            {
                if (chk == false)
                {
                    chk = true;
                    sql2 = " ";
                }
                else
                {
                    sql2 = ",";
                }
                sql1 = sql1 + sql2 + "Purchase_rate=" + funs.DecimalPoint(textBox3.Text, 2);
            }
            if (checkBox4.Checked == true)
            {
                if (chk == false)
                {
                    chk = true;
                    sql2 = " ";
                }
                else
                {
                    sql2 = ",";
                }
                sql1 = sql1 + sql2 + "Retail=" + funs.DecimalPoint(textBox10.Text, 2);
            }
            if (checkBox5.Checked == true)
            {
                if (chk == false)
                {
                    chk = true;
                    sql2 = " ";
                }
                else
                {
                    sql2 = ",";
                }
                sql1 = sql1 + sql2 + "Wholesale=" + funs.DecimalPoint(textBox11.Text, 3);
            }

            if (checkBox1.Checked == true)
            {
                if (chk == false)
                {
                    chk = true;
                    sql2 = " ";
                }
                else
                {
                    sql2 = ",";
                }
                sql1 = sql1 + sql2 + "Rate_X=" + funs.DecimalPoint(textBox1.Text, 3);
            }

            if (checkBox2.Checked == true)
            {
                if (chk == false)
                {
                    chk = true;
                    sql2 = " ";
                }
                else
                {
                    sql2 = ",";
                }
                sql1 = sql1 + sql2 + "Rate_Y=" + funs.DecimalPoint(textBox2.Text, 3);
            }
            if (checkBox7.Checked == true)
            {
                if (chk == false)
                {
                    chk = true;
                    sql2 = " ";
                }
                else
                {
                    sql2 = ",";
                }
                sql1 = sql1 + sql2 + "Rate_Z=" + funs.DecimalPoint(textBox5.Text, 3);
            }

            if (checkBox9.Checked == true)
            {
                if (chk == false)
                {
                    chk = true;
                    sql2 = " ";
                }
                else
                {
                    sql2 = ",";
                }
                sql1 = sql1 + sql2 + "MRP=" + funs.DecimalPoint(textBox12.Text, 3);
            }
            String wh = whereSql();
            String compSql = sql1 + wh;

            try
            {
                int cnt = Database.CommandExecutorInt(compSql);
                funs.ShowBalloonTip("Saved", cnt + " Record(s) Effected");
                Master.UpdateDecription();
                Master.UpdateDecriptionInfo();
                this.Close();
                this.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tab2_save() 
        {
            String sql1 = "UPDATE DESCRIPTION SET", sql2, mark;
            bool chk = false;
            if (checkBox6.Checked == true)
            {
                if (chk == false)
                {
                    chk = true;
                    sql2 = " ";
                }
                else
                {
                    sql2 = ",";
                }
                sql1 = sql1 + sql2 + "Tax_Cat_id='" + funs.Select_tax_cat_id(textBox14.Text) + "' ";
            }
            if (checkBox11.Checked == true)
            {
                if (chk == false)
                {
                    chk = true;
                    sql2 = " ";
                }
                else
                {
                    sql2 = ",";
                }
                sql1 = sql1 + sql2 + "Open_stock=" + funs.DecimalPoint(textBox4.Text);
            }
            if (checkBox8.Checked == true)
            {
                if (chk == false)
                {
                    chk = true;
                    sql2 = " ";
                }
                else
                {
                    sql2 = ",";
                }
                sql1 = sql1 + sql2 + "Open_stock2=" + funs.DecimalPoint(textBox6.Text);
            }


            if (checkBox12.Checked == true)
            {
                if (chk == false)
                {
                    chk = true;
                    sql2 = " ";
                }
                else
                {
                    sql2 = ",";
                }
                sql1 = sql1 + sql2 + "Wlavel=" + funs.DecimalPoint(textBox7.Text);
            }
            if (checkBox14.Checked == true)
            {
                if (chk == false)
                {
                    chk = true;
                    sql2 = " ";
                }
                else
                {
                    sql2 = ",";
                }
                sql1 = sql1 + sql2 + "[Commission%]=" + funs.DecimalPoint(textBox8.Text) + ",[Commission@]=" + funs.DecimalPoint(textBox9.Text);
            }
            if (checkBox13.Checked == true)
            {
                if (chk == false)
                {
                    chk = true;
                    sql2 = " ";
                }
                else
                {
                    sql2 = ",";
                }
                if (radioButton3.Checked == true)
                {
                    mark = "Mark";
                }
                else
                {
                    mark = "None";
                }
                sql1 = sql1 + sql2 + "Mark='" + mark + "'";
            }
            String wh = whereSql();

            try
            {
                int cnt = Database.CommandExecutorInt(sql1 + wh);


                funs.ShowBalloonTip("Saved", cnt + " Record(s) Effected");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SideFill()
        {
            flowLayoutPanel1.Controls.Clear();
            DataTable dtsidefill = new DataTable();

            dtsidefill.Columns.Add("Name", typeof(string));
            dtsidefill.Columns.Add("DisplayName", typeof(string));
            dtsidefill.Columns.Add("ShortcutKey", typeof(string));
            dtsidefill.Columns.Add("Visible", typeof(bool));


            if (tabControl1.SelectedIndex == 0)
            {
                //close
                dtsidefill.Rows.Add();
                dtsidefill.Rows[0]["Name"] = "quit";
                dtsidefill.Rows[0]["DisplayName"] = "Quit";
                dtsidefill.Rows[0]["ShortcutKey"] = "Esc";
                dtsidefill.Rows[0]["Visible"] = true;
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                //save1
                dtsidefill.Rows.Add();
                dtsidefill.Rows[0]["Name"] = "save1";
                dtsidefill.Rows[0]["DisplayName"] = "Save";
                dtsidefill.Rows[0]["ShortcutKey"] = "^S";
                dtsidefill.Rows[0]["Visible"] = true;

                //close
                dtsidefill.Rows.Add();
                dtsidefill.Rows[1]["Name"] = "quit";
                dtsidefill.Rows[1]["DisplayName"] = "Quit";
                dtsidefill.Rows[1]["ShortcutKey"] = "Esc";
                dtsidefill.Rows[1]["Visible"] = true;

            }

            else if (tabControl1.SelectedIndex == 2)
            {
                //save2
                dtsidefill.Rows.Add();
                dtsidefill.Rows[0]["Name"] = "save2";
                dtsidefill.Rows[0]["DisplayName"] = "Save";
                dtsidefill.Rows[0]["ShortcutKey"] = "^S";
                dtsidefill.Rows[0]["Visible"] = true;

                //close
                dtsidefill.Rows.Add();
                dtsidefill.Rows[1]["Name"] = "quit";
                dtsidefill.Rows[1]["DisplayName"] = "Quit";
                dtsidefill.Rows[1]["ShortcutKey"] = "Esc";
                dtsidefill.Rows[1]["Visible"] = true;

            }



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

            if (name == "save1")
            {
                tab1_save();
            }
            else if (name == "save2")
            {
                tab2_save();
            }
            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox13.Checked == true)
            {
                radioButton3.Enabled = true;
                radioButton4.Enabled = true;
               
            }
            else
            {
                radioButton3.Enabled = false;
                radioButton4.Enabled =  false;
            }
        }

        private void checkBox1_KeyDown_1(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".");
        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".");
        }

        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".");
        }

        private void textBox1_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".");
        }

        private void textBox2_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".");
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".");
        }

        private void a1_CheckedChanged(object sender, EventArgs e)
        {
            t1.Enabled = false;
        }

        private void tabPage1_Enter(object sender, EventArgs e)
        {
            SideFill();
        }

        private void tabPage2_Enter(object sender, EventArgs e)
        {
            SideFill();
        }

        private void tabPage4_Enter(object sender, EventArgs e)
        {
            SideFill();
        }

        private void frmModifyGroup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }

    }
}
