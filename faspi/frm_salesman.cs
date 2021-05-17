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
    public partial class frm_salesman : Form
    {
        DataTable dtSalesman;
        String dtName;
        String strCombo;
        public bool calledIndirect = false;
        public string salesmanname;
        List<UsersFeature> permission;
        String gStr;
        public frm_salesman()
        {
            InitializeComponent();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }



        public void LoadData(String str, String frmCaption)
        {
            gStr = str;
            dtName = "Salesman";
            this.Text = frmCaption;
            dtSalesman = new DataTable(dtName);
            Database.GetSqlData("select * from " + dtName + " where s_id='" + str + "' ", dtSalesman);

            if (dtSalesman.Rows.Count == 0)
            {
                dtSalesman.Rows.Add(0);
                textBox1.Text = "";
            }
            else
            {
                textBox1.Text = dtSalesman.Rows[0]["name"].ToString();
                
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
          //  dtsidefill.Rows[0]["Visible"] = true;


            permission = funs.GetPermissionKey("Salesman");
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
                    //if (Database.utype.ToUpper() == "SuperAdmin" || Database.utype.ToUpper() == "Admin")
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



        private void save()
        {
            salesmanname = textBox1.Text;

            if (gStr == "0")
            {
                DataTable dtCount = new DataTable();
                Database.GetSqlData("select count(*) from Salesman where locationid='" + Database.LocationId + "'", dtCount);

                if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                {
                    dtSalesman.Rows[0]["s_id"] = Database.LocationId + "1";
                    dtSalesman.Rows[0]["Nid"] = 1;
                    dtSalesman.Rows[0]["LocationId"] = Database.LocationId;
                    dtSalesman.Rows[0]["user_id"] = Database.user_id;
                    dtSalesman.Rows[0]["Modifiedby"] = "";
                }
                else
                {
                    DataTable dtid = new DataTable();
                    Database.GetSqlData("select max(Nid) as Nid from Salesman where locationid='" + Database.LocationId + "'", dtid);
                    int Nid = int.Parse(dtid.Rows[0][0].ToString());
                    dtSalesman.Rows[0]["S_id"] = Database.LocationId + (Nid + 1);
                    dtSalesman.Rows[0]["Nid"] = (Nid + 1);
                    dtSalesman.Rows[0]["LocationId"] = Database.LocationId;
                    dtSalesman.Rows[0]["user_id"] = Database.user_id;
                    dtSalesman.Rows[0]["Modifiedby"] = "";
                }
            }
            else
            {
                dtSalesman.Rows[0]["Modifiedby"] = Database.user_id;
            }

            dtSalesman.Rows[0]["name"] = textBox1.Text;



            Database.SaveData(dtSalesman);
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
            if (funs.Select_salesman_id(textBox1.Text) != "" && funs.Select_salesman_id(textBox1.Text) != gStr)
            {
                MessageBox.Show("SalesMan Name Already Exists");
                return false;
            }
            
            return true;
        }

        private void frm_salesman_Load(object sender, EventArgs e)
        {
            SideFill();
        }

        private void frm_salesman_KeyDown(object sender, KeyEventArgs e)
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

    }
}
