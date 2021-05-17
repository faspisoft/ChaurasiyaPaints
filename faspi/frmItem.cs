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
    public partial class frmItem : Form
    {
        DataTable dtItem;
        String dtName;         
        public bool calledIndirect = false;
        public String ItemName;
        public String Type;        
        String strCombo;        
        string gStr;
        List<UsersFeature> permission;


        public frmItem()
        {
            InitializeComponent();
        }

        private void frmItem_Load(object sender, EventArgs e)
        {
            SideFill();
        }

        public void LoadData(String str, String frmCaption)
        {
            gStr = str;
            this.Text = frmCaption;
            dtName = "other";
            dtItem = new DataTable(dtName);
            Database.GetSqlData("select * from " + dtName + " where oth_id='" + str + "' ", dtItem);
            
            if (Type==null && dtItem.Rows.Count == 0)
            {
                dtItem.Rows.Add(0);
                textBox1.Text = "";
                textBox2.Text = "";
            }

            else if (Type == "Company" && dtItem.Rows.Count == 0)
            {
                dtItem.Rows.Add();
                textBox1.Text = "";
                textBox2.Text = "Company";
                textBox2.Enabled = false;
            }
            else if (Type == "Colour" && dtItem.Rows.Count == 0)
            {
                dtItem.Rows.Add();
                textBox1.Text = "";
                textBox2.Text = "Colour";
                textBox2.Enabled = false;
                this.Text = frmCaption + " " + Type;
            }
            else if (Type == "Group" && dtItem.Rows.Count == 0)
            {
                dtItem.Rows.Add();
                textBox1.Text = "";
                textBox2.Text = "Group";
                textBox2.Enabled = false;
                this.Text = frmCaption + " " + Type;
            }
            else if (Type == "Item" && dtItem.Rows.Count == 0)
            {
                dtItem.Rows.Add();
                textBox1.Text = "";
                textBox2.Text = "Item";
                textBox2.Enabled = false;
                this.Text = frmCaption + " " + Type;
            }
            else if (Type == "Department" && dtItem.Rows.Count == 0)
            {
                dtItem.Rows.Add();
                textBox1.Text = "";
                textBox2.Text = "Department";
                textBox2.Enabled = false;
                this.Text = frmCaption + " " + Type;
            }
            else
            {
                textBox1.Text = dtItem.Rows[0]["name"].ToString();
                textBox2.Text = funs.Select_act_nm(dtItem.Rows[0]["type"].ToString());
                textBox2.Enabled = false;
                this.Text = frmCaption;
            }
        }

        private void save()
        {
            ItemName = textBox1.Text;

            if (gStr == "0")
            {
                DataTable dtCount = new DataTable();
                Database.GetSqlData("select count(*) from other where locationid='" + Database.LocationId + "'", dtCount);
                if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                {
                    dtItem.Rows[0]["Oth_id"] = Database.LocationId + "1";
                    dtItem.Rows[0]["Nid"] = 1;
                    dtItem.Rows[0]["LocationId"] = Database.LocationId;
                    dtItem.Rows[0]["user_id"] = Database.user_id;
                    dtItem.Rows[0]["Modifiedby"] = "";
                }
                else
                {
                    DataTable dtAcid = new DataTable();

                    Database.GetSqlData("select max(Nid) as Nid from other where locationid='" + Database.LocationId + "'", dtAcid);
                    int Nid = int.Parse(dtAcid.Rows[0][0].ToString());
                    dtItem.Rows[0]["Oth_id"] = Database.LocationId + (Nid + 1);
                    dtItem.Rows[0]["Nid"] = (Nid + 1);
                    dtItem.Rows[0]["LocationId"] = Database.LocationId;
                    dtItem.Rows[0]["user_id"] = Database.user_id;
                    dtItem.Rows[0]["Modifiedby"] = "";
                }
            }
            else
            {
                dtItem.Rows[0]["Modifiedby"] = Database.user_id;
            }

            dtItem.Rows[0]["name"] = textBox1.Text;
            dtItem.Rows[0]["type"] = funs.Select_act_id(textBox2.Text);
           
            Database.SaveData(dtItem);
            Master.UpdateOther();

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
            
            if (textBox1.Text == "")
            {
                textBox1.BackColor = Color.Aqua;
                textBox1.Focus();
                return false;
            }
            else if (textBox2.Text == "")
            {
                textBox2.BackColor = Color.Aqua;
                textBox2.Focus();
                return false;
            }
            if (funs.Select_oth_id(textBox1.Text) != "" && funs.Select_oth_id(textBox1.Text)!=gStr)
            {
                MessageBox.Show(" Name Already Exists of Same Type");
                return false;
            }
            return true;
        }

        private void frmItem_KeyDown(object sender, KeyEventArgs e)
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
            //strCombo = "select [name] from accountype where (Act_id='SER14' or Act_id='SER18' or Act_id='SER16' or Act_id='SER15')";
            //textBox2.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
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
           // dtsidefill.Rows[0]["Visible"] = true;

            permission = funs.GetPermissionKey("Company");
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
    }
}
