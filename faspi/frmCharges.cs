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
    public partial class frmCharges : Form
    {
        DataTable dtCharges;
        String dtName;
        String strCombo;
        public bool calledIndirect = false; 
        public string chrgname;
        List<UsersFeature> permission;

        String gStr;

        public frmCharges()
        {
            InitializeComponent();
        }

        private void frmCharges_Load(object sender, EventArgs e)
        {
            SideFill();
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
            //dtsidefill.Rows[0]["Visible"] = true;



            permission = funs.GetPermissionKey("Charges");
            //create
            UsersFeature ob = permission.Where(w => w.FeatureName == "Create").FirstOrDefault();
            if (ob != null && gStr== "0" && ob.SelectedValue == "Allowed")
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
                    //if (Database.utype.ToUpper() == "SUPERADMIN" || Database.utype.ToUpper() == "ADMIN")
                    //{
                    //    save();
                    //}

                    //else if (gStr == "0")
                    //{
                    //    save();
                    //}
                }
            }

            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }

        public void LoadData(String str, String frmCaption)
        {
            gStr = str;
            dtName = "charges";
            this.Text = frmCaption;
            dtCharges = new DataTable(dtName);
            Database.GetSqlData("select * from " + dtName + " where ch_id='" + str + "' ", dtCharges);
            
            if (dtCharges.Rows.Count == 0)
            {
                dtCharges.Rows.Add(0);
                textBox1.Text = "";
            }
            else
            {
                textBox1.Text = dtCharges.Rows[0]["name"].ToString();
                if (dtCharges.Rows[0]["ac_id"].ToString() == "" || dtCharges.Rows[0]["ac_id"].ToString() == "0")
                {
                    radioButton1.Checked = false;
                    radioButton2.Checked = true;
                }
                else
                {
                    radioButton1.Checked = true;
                    radioButton2.Checked = false;
                    textBox2.Text = funs.Select_ac_nm(dtCharges.Rows[0]["ac_id"].ToString()); 
                }
                if (int.Parse(dtCharges.Rows[0]["charge_type"].ToString()) == 1)
                {
                    radioButton3.Checked = true;
                }
                else if(int.Parse(dtCharges.Rows[0]["charge_type"].ToString()) == 2)
                {
                    radioButton4.Checked = true;
                }
                else if (int.Parse(dtCharges.Rows[0]["charge_type"].ToString()) == 3)
                {
                    radioButton5.Checked = true;
                }
                else if (int.Parse(dtCharges.Rows[0]["charge_type"].ToString()) == 4)
                {
                    radioButton8.Checked = true;
                }
                if (int.Parse(dtCharges.Rows[0]["add_sub"].ToString()) == 4)
                {
                    radioButton6.Checked = true;
                }
                else
                {
                    radioButton7.Checked = true;
                }
            }
        }

        private void save()
        {
            chrgname = textBox1.Text;

            if (gStr == "0")
            {
                DataTable dtCount = new DataTable();
                Database.GetSqlData("select count(*) from CHARGES where locationid='" + Database.LocationId + "'", dtCount);

                if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                {
                    dtCharges.Rows[0]["Ch_id"] = Database.LocationId + "1";
                    dtCharges.Rows[0]["Nid"] = 1;
                    dtCharges.Rows[0]["LocationId"] = Database.LocationId;
                    dtCharges.Rows[0]["user_id"] = Database.user_id;
                    dtCharges.Rows[0]["Modifiedby"] = "";
                }
                else
                {
                    DataTable dtid = new DataTable();
                    Database.GetSqlData("select max(Nid) as Nid from CHARGES where locationid='" + Database.LocationId + "'", dtid);
                    int Nid = int.Parse(dtid.Rows[0][0].ToString());
                    dtCharges.Rows[0]["Ch_id"] = Database.LocationId + (Nid + 1);
                    dtCharges.Rows[0]["Nid"] = (Nid + 1);
                    dtCharges.Rows[0]["LocationId"] = Database.LocationId;
                    dtCharges.Rows[0]["user_id"] = Database.user_id;
                    dtCharges.Rows[0]["Modifiedby"] = "";
                }
            }
            else
            {
                dtCharges.Rows[0]["Modifiedby"] = Database.user_id;
            }

            dtCharges.Rows[0]["name"] = textBox1.Text;
            if (textBox2.Text != "")
            {
                if (radioButton1.Checked == true)
                {
                    dtCharges.Rows[0]["ac_id"] = funs.Select_ac_id(textBox2.Text);
                }
                else
                {
                    dtCharges.Rows[0]["ac_id"] = "0";
                }
            }
            else
            {
                dtCharges.Rows[0]["ac_id"] = "0";
            }
            if (radioButton3.Checked == true)
            {
                dtCharges.Rows[0]["charge_type"] = 1;
            }
            else if (radioButton4.Checked == true)
            {
                dtCharges.Rows[0]["charge_type"] = 2;
            }
            else if (radioButton5.Checked == true)
            {
                dtCharges.Rows[0]["charge_type"] = 3;
            }
            else if (radioButton8.Checked == true)
            {
                dtCharges.Rows[0]["charge_type"] = 4;
            }
            if (radioButton6.Checked == true)
            {
                dtCharges.Rows[0]["add_sub"] = 4;
            }
            else if (radioButton7.Checked == true)
            {
                dtCharges.Rows[0]["add_sub"] = 5;
            }
           
            Database.SaveData(dtCharges);
            Master.UpdateCharge();
            funs.ShowBalloonTip("Saved", "Saved Successfully");

            if (calledIndirect == true)
            {
                this.Close();
                this.Dispose();
            }
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
            if (funs.Select_ch_id(textBox1.Text) != "" && funs.Select_ch_id(textBox1.Text)!=gStr)
            {
                MessageBox.Show("Charges Name Already Exists");
                return false;
            }
            if (radioButton1.Checked == true)
            {
                if (textBox2.Text == "")
                {
                    MessageBox.Show("Please Select A/c Name.");
                    textBox2.Focus();
                    return false;
                }
            }
            return true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Enabled = true;
            radioButton4.Enabled = false;
            radioButton8.Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Enabled = false;
            radioButton4.Enabled = true;
            radioButton8.Enabled = true;
        }

        private void frmCharges_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control &&  e.KeyCode == Keys.S)
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
                    //if (Database.utype.ToUpper() == "SUPERADMIN" || Database.utype.ToUpper() == "ADMIN")
                    //{
                    //    save();
                    //}

                    //else if (gStr == "0")
                    //{
                    //    save();
                    //}
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

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from account where act_id='SER6' or act_id='SER7' or act_id='SER3' or act_id='SER37' or act_id='SER12' or act_id='SER30'";
            textBox2.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void radioButton1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void radioButton2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void radioButton3_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void radioButton4_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void radioButton5_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void radioButton6_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void radioButton7_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Control && e.KeyCode == Keys.C)
            {
                textBox2.Text = funs.AddAccount();
            }
            else if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox2.Text != "")
                {
                    textBox2.Text = funs.EditAccount(textBox2.Text);
                }
            }
        }

        private void radioButton8_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\'')
            {
                e.Handled = true;
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }
    }
}
