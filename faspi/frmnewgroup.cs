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
    public partial class frmnewgroup : Form
    {
        DataTable dtGrp;
        String dtName;
        List<UsersFeature> permission;

        public bool calledIndirect = false;
        public String GrpName;

        String gStr;

        public frmnewgroup()
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
           // dtsidefill.Rows[0]["Visible"] = true;

            permission = funs.GetPermissionKey("Account Group");
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
             
                    if (calledIndirect == true)
                    {
                        this.Close();
                    }
                }
            }
            if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }

        private void frmnewgroup_Load(object sender, EventArgs e)
        {
            SideFill();
        }

        public void LoadData(String str, String frmCaption)
        {
            gStr = str;
            dtName = "Accountype";
            dtGrp = new DataTable(dtName);
            Database.GetSqlData("select * from " + dtName + " where Act_id='" + str+"'", dtGrp);




            textBox1.Focus();

            this.Text = frmCaption;
            if (dtGrp.Rows.Count == 0)
            {
                dtGrp.Rows.Add(0);
                textBox1.Text = "";
                textBox2.Text = "";

            }
            else
            {
                textBox1.Text = dtGrp.Rows[0]["name"].ToString();


                textBox2.Text = funs.Select_act_nm(dtGrp.Rows[0]["under"].ToString());
            }
        }

        private void save()
        {
            if (gStr == "0")
            {
                DataTable dtCount = new DataTable();
                Database.GetSqlData("select count(*) from Accountype where locationid='" + Database.LocationId + "'", dtCount);

                if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                {
                    dtGrp.Rows[0]["Act_id"] = Database.LocationId + "1";
                    dtGrp.Rows[0]["Nid"] = 1;
                    dtGrp.Rows[0]["LocationId"] = Database.LocationId;
                    //dtGrp.Rows[0]["user_id"] = Database.user_id;
                    //dtGrp.Rows[0]["Modifiedby"] = "";
                }
                else
                {
                    DataTable dtid = new DataTable();
                    Database.GetSqlData("select max(Nid) as Nid from Accountype where locationid='" + Database.LocationId + "'", dtid);
                    int Nid = int.Parse(dtid.Rows[0][0].ToString());
                    dtGrp.Rows[0]["Act_id"] = Database.LocationId + (Nid + 1);
                    dtGrp.Rows[0]["Nid"] = (Nid + 1);
                    dtGrp.Rows[0]["LocationId"] = Database.LocationId;
                    //dtGrp.Rows[0]["user_id"] = Database.user_id;
                    //dtGrp.Rows[0]["Modifiedby"] = "";
                }
            }
            else
            {
                //dtGrp.Rows[0]["Modifiedby"] = Database.user_id;
            }
            GrpName = textBox1.Text;
            dtGrp.Rows[0]["name"] = textBox1.Text;
            dtGrp.Rows[0]["RefineName"] = textBox1.Text;
            dtGrp.Rows[0]["Type"] = "Account";
            dtGrp.Rows[0]["under"] = funs.Select_act_id(textBox2.Text);
            dtGrp.Rows[0]["Nature"] = funs.Select_act_nature(textBox2.Text);
            dtGrp.Rows[0]["fixed"] = false;
            string path = funs.Select_act_path(textBox2.Text);
            dtGrp.Rows[0]["Path"] = "";
            int level = funs.Select_act_level(textBox2.Text) + 1;
            dtGrp.Rows[0]["level"] = level;
            dtGrp.Rows[0]["Sequence"] = 0;
            dtGrp.Rows[0]["Regsqn"] = funs.Select_act_regsqn(textBox2.Text);
            Database.SaveData(dtGrp);
           
            Master.UpdateAccountType();

            string actidactual = funs.Select_AccType_id(textBox1.Text);
            string act_id = actidactual;
            act_id = act_id.Substring(3);

            path = path + act_id + ";";
            Database.CommandExecutor("Update Accountype set Path='" + path + "' where Act_id='" + actidactual+"'");
            Master.UpdateAccountType();


            Master.UpdateAccountinfo();
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

            if (funs.Select_AccType_id(textBox1.Text) != "" && funs.Select_AccType_id(textBox1.Text) != gStr)
            {
                MessageBox.Show("Account Group Already Exists");
                return false;
            }

            return true;
        }
        private void frmnewgroup_KeyDown(object sender, KeyEventArgs e)
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

                        if (gStr == "0")
                        {
                            LoadData("0", this.Text);
                        }
                        else
                        {
                            this.Close();
                            this.Dispose();
                        }
                        if (calledIndirect == true)
                        {
                            this.Close();
                            this.Dispose();
                        }
                    

                   
                }
            }
            else
            {
                textBox1.BackColor = Color.White;

            }
            if (e.KeyCode == Keys.Escape)
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
                        this.Dispose();
                    }
                }
                else
                {
                    this.Dispose();
                }
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strCombo = "Select Name from Accountype order by Name";

            textBox2.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);


          
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
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

    }
}
