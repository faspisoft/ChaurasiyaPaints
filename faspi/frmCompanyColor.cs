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
    public partial class frmCompanyColor : Form
    {
        public String gCap;
         
        String strCombo;
 
        public frmCompanyColor()
        {
            InitializeComponent();
        }

        private void frmCompanyColor_Load(object sender, EventArgs e)
        {
            this.Size = this.MdiParent.Size;
            SideFill();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            t1.Enabled = true;
            t1.Focus();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            t2.Enabled = true;
            t2.Focus();
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            t3.Enabled = true;
            t3.Focus();
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            t4.Enabled = true;
            t4.Focus();
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            t4.Enabled = false;
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            t3.Enabled = false;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            t2.Enabled = false;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            t1.Enabled = false;
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
            dtsidefill.Rows[0]["Name"] = "ok";
            dtsidefill.Rows[0]["DisplayName"] = "Next";
            dtsidefill.Rows[0]["ShortcutKey"] = "";
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

            if (name == "ok")
            {
                String sql = "";
                if (s1.Checked == true)
                {
                    if (sql == "")
                    {
                        sql = " where ";
                    }
                    else
                    {
                        sql += " and ";
                    }
                    sql += " Company_id='" + funs.Select_oth_id(t1.Text) + "' ";
                }
                if (s2.Checked == true)
                {
                    if (sql == "")
                    {
                        sql = " where ";
                    }
                    else
                    {
                        sql += " and ";
                    }
                    sql += " Item_id='" + funs.Select_oth_id(t2.Text) + "' ";
                }
                if (s3.Checked == true)
                {
                    if (sql == "")
                    {
                        sql = " where ";
                    }
                    else
                    {
                        sql += " and ";
                    }
                    sql += " Col_id='" + funs.Select_oth_id(t3.Text) + "' ";
                }
                if (s4.Checked == true)
                {
                    if (sql == "")
                    {
                        sql = " where ";
                    }
                    else
                    {
                        sql += " and ";
                    }
                    sql += " Group_id='" + funs.Select_oth_id(t4.Text) + "' ";
                }

                if (radioButton2.Checked == true)
                {
                    if (sql == "")
                    {
                        sql = " where";
                    }
                    else
                    {
                        sql += " and ";
                    }

                    sql += " Description.Pack='" + t5.Text +"' ";
                }
                if (gCap == "Rate Modify")
                {
                    frmEditGroup frm = new frmEditGroup();
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                    frm.LoadData("0", gCap, sql);
                }
                else if (gCap == "List of Description")
                {
                    frmDescList frm = new frmDescList();
                    frm.MdiParent = this.MdiParent;
                    frm.LoadData("0", gCap, sql);
                    frm.Show();                    
                }
                this.Close();
            }
            if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }

        private void t1_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from other where Type='SER14' order by name";
            t1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
            
        }

        private void t2_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from other where Type='SER15' order by name";
            t2.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void t3_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from other where Type='SER18' order by name";
            t3.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void t4_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from other where Type='SER16' order by name";
            t4.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void frmCompanyColor_KeyDown(object sender, KeyEventArgs e)
        {           
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult chk = MessageBox.Show("Are u sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (chk == DialogResult.No)
                {
                    e.Handled = false;
                }
                else
                {
                    this.Close();
                    this.Dispose();
                }
            }
        }

        private void a1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void s1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void a2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void s2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void a3_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void s3_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void a4_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void s4_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            t5.Enabled = true;
            t5.Focus();
        }

        private void t5_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select Distinct pack from Description order by pack";
            t5.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void radioButton1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void radioButton2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            t5.Enabled = false;
        }        
    }
}
