using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Web.Script.Serialization;



namespace faspi
{
    public partial class frm_NewGroup : Form
    {
        DataTable dtGrp;
        String dtName;
        public bool calledIndirect = false;
        public String GrpName;
        public string statename;
        String gStr;
        List<UsersFeature> permission;

        public frm_NewGroup()
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
            //dtsidefill.Rows[0]["Visible"] = true;

            permission = funs.GetPermissionKey("Payment Collector");
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

        private void frm_NewGroup_Load(object sender, EventArgs e)
        {
            SideFill();
        }

        private void frm_NewGroup_KeyDown(object sender, KeyEventArgs e)
        {
            if  (e.Control &&  e.KeyCode == Keys.S)
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

        public void LoadData(String str,String frmCaption)
        {
            gStr = str;
            this.Text = frmCaption;
            dtName = "other";
            dtGrp = new DataTable(dtName);
            Database.GetSqlData("select * from " + dtName + " where oth_id='" + str + "' ", dtGrp);


            DataTable dtGroupName = new DataTable();
            Database.GetSqlData("select [name] from accountype where Act_id='SER17'", dtGroupName);
            

            textBox3.Text = dtGroupName.Rows[0][0].ToString();
            textBox3.Enabled = false;
            textBox1.Focus();
            
            if (dtGrp.Rows.Count == 0)
            {
                dtGrp.Rows.Add(0);
                textBox1.Text = "";
                textBox2.Text = "";
            }

            else
            {
                textBox1.Text = dtGrp.Rows[0]["name"].ToString();
                textBox2.Text = funs.DecimalPoint(dtGrp.Rows[0]["Blimit"]);
            }
        }

        private void save()
        {
            GrpName = textBox1.Text;

            if (gStr == "0")
            {
                DataTable dtCount = new DataTable();
                Database.GetSqlData("select count(*) from OTHER where locationid='" + Database.LocationId + "'", dtCount);

                if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                {
                    dtGrp.Rows[0]["Oth_id"] = Database.LocationId + "1";
                    dtGrp.Rows[0]["Nid"] = 1; dtGrp.Rows[0]["LocationId"] = Database.LocationId;
                    dtGrp.Rows[0]["user_id"] = Database.user_id;
                    dtGrp.Rows[0]["Modifiedby"] = "";
                }
                else
                {
                    DataTable dtid = new DataTable();
                    Database.GetSqlData("select max(Nid) as Nid from OTHER where locationid='" + Database.LocationId + "'", dtid);
                    int Nid = int.Parse(dtid.Rows[0][0].ToString());
                    dtGrp.Rows[0]["Oth_id"] = Database.LocationId + (Nid + 1);
                    dtGrp.Rows[0]["Nid"] = (Nid + 1);
                    dtGrp.Rows[0]["LocationId"] = Database.LocationId;
                    dtGrp.Rows[0]["user_id"] = Database.user_id;
                    dtGrp.Rows[0]["Modifiedby"] = "";
                }
            }
            else
            {
                dtGrp.Rows[0]["Modifiedby"] = Database.user_id;
            }
            dtGrp.Rows[0]["name"] = textBox1.Text;
            dtGrp.Rows[0]["Blimit"] = textBox2.Text;
            dtGrp.Rows[0]["Type"] = funs.Select_act_id(textBox3.Text);
            dtGrp.Rows[0]["Dlimit"] = 0;
           
            Database.SaveData(dtGrp);
            Master.UpdateOther();
            Master.UpdateAccountinfo();
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
            if (textBox2.Text == "")
            {
                textBox2.Text = "0";
            }

            if (textBox1.Text == "")
            {
                textBox1.BackColor = Color.Aqua;
                textBox1.Focus();
                return false;
            }
           
            if (funs.isDouble(textBox2.Text) == false)
            {
                textBox2.BackColor = Color.Aqua;
                textBox2.Focus();     
                return false;
            }

            if(funs.Select_oth_id(textBox1.Text)!="" && funs.Select_oth_id(textBox1.Text)!=gStr)
            {
                MessageBox.Show("Account Group Already Exists");
                return false;
            }
            return true;
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\'')
            {
                e.Handled = true;
            }
        }
    }
}
