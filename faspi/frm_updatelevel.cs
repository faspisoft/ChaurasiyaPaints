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
    public partial class frm_updatelevel : Form
    {
        string strCombo = "";
        DataTable dtlevel = new DataTable("Description");
        public frm_updatelevel()
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
                try
                {
                    Database.BeginTran();
                    Database.SaveData(dtlevel, "Select Des_id,Description,Pack,Wlavel,Max_level from Description");
                    Database.CommitTran();
                    funs.ShowBalloonTip("Saved Successfully","Saved");
                    this.Close();
                    this.Dispose();
                }
                catch (Exception ex)
                {
                    funs.ShowBalloonTip("Not Saved Successfully", "Error");
                    Database.RollbackTran();
                }
               
            }
            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }

        private void frm_updatelevel_Load(object sender, EventArgs e)
        {
            SideFill();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] as Company from other where Type='SER14' order by [name]";
            textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from other where Type='SER15' order by [name]";
            textBox2.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Enter Company/Manufacturer");
                textBox1.Focus();
                return;
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("Enter Brand/Item");
                textBox2.Focus();
                return;
            }
            Loaddata();
        }


        private void Loaddata()
        {
            if (textBox1.Text == "")
            {
                ansGridView1.Rows.Clear();
                ansGridView1.Columns.Clear();
            }
            else
            {
                Database.GetSqlData("SELECT Des_id, Description, Pack, Max_level, Wlavel FROM  Description WHERE  (Company_id = '" + funs.Select_oth_id(textBox1.Text) + "') AND (Item_id = '" + funs.Select_oth_id(textBox2.Text) + "') ORDER BY Col_id, Pvalue DESC, Description", dtlevel);
                ansGridView1.DataSource = dtlevel;
                ansGridView1.Columns["Des_id"].Visible = false;
                ansGridView1.Columns["Description"].ReadOnly = true;
                ansGridView1.Columns["Pack"].ReadOnly = true;

            }
        }

        private void frm_updatelevel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                try
                {
                    Database.BeginTran();

                    Database.SaveData(dtlevel, "Select Des_id,Description,Pack,Wlavel,Max_level from Description");
                    Database.CommitTran();
                    funs.ShowBalloonTip("Saved Successfully", "Saved");
                    this.Close();
                    this.Dispose();
                }
                catch (Exception ex)
                {
                    funs.ShowBalloonTip("Not Saved Successfully", "Error");
                    Database.RollbackTran();
                }
               
            }
        }


    }
}
