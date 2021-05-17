using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web.Script.Serialization;

namespace faspi
{
    public partial class frm_DAT : Form
    {
        String strCombo;
        public string gStr = "";
        DataTable dtAftertax;
        String dtName;
        List<UsersFeature> permission;
        public frm_DAT()
        {
            InitializeComponent();
        }

        private void frm_DAT_Load(object sender, EventArgs e)
        {
            SideFill();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataTable dtcombo = new DataTable();
            dtcombo.Columns.Add("Type", typeof(string));

            dtcombo.Rows.Add();
            dtcombo.Rows[0][0] = "Quantity*PackValue";

            dtcombo.Rows.Add();
            dtcombo.Rows[1][0] = "Percentage";

            dtcombo.Rows.Add();
            dtcombo.Rows[2][0] = "Quantity";

            dtcombo.Rows.Add();
            dtcombo.Rows[3][0] = "Flat";

            textBox2.Text = SelectCombo.ComboDt(this, dtcombo, 0);
            SendKeys.Send("{tab}");
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from account where act_id='SER6' or act_id='SER7' or act_id='SER3' or act_id='SER37' or act_id='SER12' or act_id='SER30'";
            textBox3.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        public void LoadData(String str, String frmCaption)
        {
            gStr = str;
            dtName = "DisAfterTax";
            dtAftertax = new DataTable(dtName);
            Database.GetSqlData("select * from " + dtName + " where tax_id='" + str + "' ", dtAftertax);
            this.Text = frmCaption;
            if (dtAftertax.Rows.Count == 0)
            {
                dtAftertax.Rows.Add(0);
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
            }
            else
            {
                textBox1.Text = dtAftertax.Rows[0]["taxname"].ToString();
                textBox2.Text = dtAftertax.Rows[0]["type"].ToString();
                textBox3.Text = funs.Select_ac_nm(dtAftertax.Rows[0]["ac_id"].ToString());
            }
        }

        private void save()
        {
            dtAftertax.Rows[0]["taxname"] = textBox1.Text;
            dtAftertax.Rows[0]["type"] = textBox2.Text;
            dtAftertax.Rows[0]["ac_id"] = funs.Select_ac_id(textBox3.Text);
            dtAftertax.Rows[0]["LocationId"] = Database.LocationId;
            Database.SaveData(dtAftertax);
            funs.ShowBalloonTip("Saved", "Saved Successfully");
            if (gStr == "0")
            {
                LoadData("0", this.Text);
            }
            else
            {
                this.Close();
                this.Dispose();
            }
        }

        private bool validate()
        {
            if (textBox1.Text == "")
            {
                textBox1.BackColor = Color.Aqua;
                textBox1.Focus();
                return false;
            }
            if (textBox2.Text == "")
            {
                textBox2.BackColor = Color.Aqua;
                textBox2.Focus();
                return false;
            }

            if (textBox3.Text == "")
            {
                textBox3.BackColor = Color.Aqua;
                textBox3.Focus();
                return false;
            }
            return true;
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
            permission = funs.GetPermissionKey("DAT");
            //create
            UsersFeature ob = permission.Where(w => w.FeatureName == "Create").FirstOrDefault();
            if (ob != null && gStr == "0" && ob.SelectedValue == "Allowed")
            {
                dtsidefill.Rows[0]["Visible"] = true;
            }
            else if (gStr == "0")
            {
                dtsidefill.Rows[0]["Visible"] = false;
            }

            //alter
            ob = permission.Where(w => w.FeatureName == "Alter").FirstOrDefault();
            if (ob != null && gStr != "0" && ob.SelectedValue == "Allowed")
            {
                dtsidefill.Rows[0]["Visible"] = true;
            }
            else if (gStr != "0")
            {
                dtsidefill.Rows[0]["Visible"] = false;
            }


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
                    save();
                    this.Close();
                    this.Dispose();
                }
            }
            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
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
            Database.lostFocus(textBox2);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox3);
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox3);
        }

        private void frm_DAT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                if (validate() == true)
                {
                    save();
                    this.Close();
                    this.Dispose();
                }
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox3.Text != "")
                {
                    textBox3.Text = funs.EditAccount(textBox3.Text);
                }
            }
            if (e.Control && e.KeyCode == Keys.C)
            {
                textBox3.Text = funs.AddAccount();
            }
        }
    }
}
