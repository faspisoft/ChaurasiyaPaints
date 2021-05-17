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
    public partial class frm_usermgmt : Form
    {
        DataTable dtUser;
        String dtName;
        public bool calledIndirect = false;
        public String User;
        String strCombo;
        public string gStr = "";
        List<UsersFeature> permission;

        public frm_usermgmt()
        {
            InitializeComponent();
        }

        private void frm_usermgmt_Load(object sender, EventArgs e)
        {
            SideFill();

            if (Feature.Available("Required Department").ToUpper() == "NO")
            {
                label4.Visible = false;
                textBox4.Visible = false;
            }
            else
            {
                label4.Visible = true;
                textBox4.Visible = true;
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

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {


           // DataTable dtcombo = new DataTable();
           // dtcombo.Columns.Add("Type", typeof(string));

           // dtcombo.Columns["Type"].ColumnName = "Type";
           // dtcombo.Rows.Add();
           // dtcombo.Rows[0][0] = "SuperAdmin";
           // dtcombo.Rows.Add();
           // dtcombo.Rows[1][0] = "Admin";
           // dtcombo.Rows.Add();
           // dtcombo.Rows[2][0] = "SuperUser";
           // dtcombo.Rows.Add();
           // dtcombo.Rows[3][0] = "User";
           // dtcombo.Rows.Add();
           // dtcombo.Rows[4][0] = "Cashier";

           //textBox3.Text = SelectCombo.ComboDt(this, dtcombo, 0);
           //SendKeys.Send("{tab}");
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox3);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox3);
        }
        public void LoadData(String str, String frmCaption)
        {
            gStr = str;
            dtName = "Userinfo";
            dtUser = new DataTable(dtName);
            Database.GetSqlData("select * from " + dtName + " where U_id='" + str+"' ", dtUser);

            this.Text = frmCaption;
            if (dtUser.Rows.Count == 0)
            {
                dtUser.Rows.Add(0);
                textBox1.Select();
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
            }
            else
            {
                textBox1.Select();
                textBox1.Text = dtUser.Rows[0]["UName"].ToString();
                textBox2.Text = dtUser.Rows[0]["UPass"].ToString();
                textBox3.Text =  funs.Select_Role_Name(int.Parse(dtUser.Rows[0]["Roleid"].ToString()));
                textBox4.Text = funs.Select_oth_nm(dtUser.Rows[0]["Department_id"].ToString());
                textBox5.Text = funs.Select_branch_name(dtUser.Rows[0]["Branch_id"].ToString());
            }
            SideFill();
        }

        private void save()
        {
            User = textBox1.Text;

            if (gStr == "0")
            {
                DataTable dtCount = new DataTable();
                Database.GetSqlData("select count(*) from Userinfo where locationid='" + Database.LocationId + "'", dtCount);

                if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                {
                    dtUser.Rows[0]["U_Id"] = Database.LocationId + "1";
                    dtUser.Rows[0]["Nid"] = 1;
                    dtUser.Rows[0]["LocationId"] = Database.LocationId;
                    dtUser.Rows[0]["user_id"] = Database.user_id;
                    dtUser.Rows[0]["Modifiedby"] = "";
                }
                else
                {
                    DataTable dtid = new DataTable();
                    Database.GetSqlData("select max(Nid) as Nid from Userinfo where locationid='" + Database.LocationId + "'", dtid);
                    int Nid = int.Parse(dtid.Rows[0][0].ToString());
                    dtUser.Rows[0]["U_Id"] = Database.LocationId + (Nid + 1);
                    dtUser.Rows[0]["Nid"] = (Nid + 1);
                    dtUser.Rows[0]["LocationId"] = Database.LocationId;
                    dtUser.Rows[0]["user_id"] = Database.user_id;
                    dtUser.Rows[0]["Modifiedby"] = "";
                }
            }
            else
            {
                dtUser.Rows[0]["Modifiedby"] = Database.user_id;
            }

            dtUser.Rows[0]["UName"] = textBox1.Text;
            dtUser.Rows[0]["UPass"] = textBox2.Text;
            dtUser.Rows[0]["Roleid"] = funs.Select_Role_id(textBox3.Text);
            dtUser.Rows[0]["Branch_id"] =  funs.Select_branch_id(textBox5.Text);
           
            if (textBox3.Text == "Admin" || textBox3.Text == "SuperAdmin")
            {
                dtUser.Rows[0]["Department_id"] = 0;
            }
            else
            {
                dtUser.Rows[0]["Department_id"] = funs.Select_oth_id(textBox4.Text);
            }
           
            Database.SaveData(dtUser);       
            funs.ShowBalloonTip("Saved", "Saved Successfully");

            if (calledIndirect == true)
            {
                this.Close();
                this.Dispose();
            }
            else if (gStr == "0")
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
            if (textBox1.Text.Trim() == "")
            {
                textBox1.BackColor = Color.Aqua;
                textBox1.Focus();
                return false;
            }
            if (textBox2.Text.Trim() == "")
            {
                textBox2.BackColor = Color.Aqua;
                textBox2.Focus();
                return false;
            }
            if (textBox3.Text.Trim() == "")
            {
                textBox3.BackColor = Color.Aqua;
                textBox3.Focus();
                return false;
            }
            if (textBox5.Text.Trim() == "")
            {
                textBox5.BackColor = Color.Aqua;
                textBox5.Focus();
                return false;
            }
            if (funs.Select_user_id(textBox1.Text) != "" && funs.Select_user_id(textBox1.Text) != gStr)
            {
                MessageBox.Show("User Name Already Exists.");
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
            permission = funs.GetPermissionKey("User");
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
                    //save();
                    //create
                    UsersFeature ob = permission.Where(w => w.FeatureName == "Create").FirstOrDefault();
                    if (ob != null && gStr == "0" && ob.SelectedValue == "Allowed")
                    {
                        save();
                    }

                    //alter
                    ob = permission.Where(w => w.FeatureName == "Alter").FirstOrDefault();
                    if (ob != null && gStr != "0" && ob.SelectedValue == "Allowed")
                    {
                        save();
                    }
                }
            }
            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
            
        }

        private void frm_usermgmt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                if (validate() == true)
                {
                    //create
                    UsersFeature ob = permission.Where(w => w.FeatureName == "Create").FirstOrDefault();
                    if (ob != null && gStr == "0" && ob.SelectedValue == "Allowed")
                    {
                        save();
                    }

                    //alter
                    ob = permission.Where(w => w.FeatureName == "Alter").FirstOrDefault();
                    if (ob != null && gStr != "0" && ob.SelectedValue == "Allowed")
                    {
                        save();
                    }
                }
            }

            else if (e.KeyCode == Keys.Escape)
            {
                if (textBox1.Text != "")
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
                else
                {
                    this.Close();
                    this.Dispose();
                }
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from other where Type='" + funs.Get_Department_id() + "' order by [name]";
            textBox4.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            //if (textBox3.Text == "User")
            //{
            //    label4.Visible = true;
            //    textBox4.Visible = true;
            //}
            //else
            //{
            //    label4.Visible = false;
            //    textBox4.Visible = false;
            //}
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [Bname] from Branch  order by Bname";
            textBox5.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);            
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox5);
        }

        private void textBox5_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox5);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "Select RoleName from SYS_Role order by RoleName";
            textBox3.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);

        }
    }
}
