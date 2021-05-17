using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web.Script.Serialization;

namespace faspi
{
    public partial class frm_role : Form
    {
        DataTable dtUser;
        String dtName;
        public bool calledIndirect = false;
        public String User;
        String strCombo;
        public string gStr = "";
        int roleid = 0;
        DataTable dtpagesroleTobeSaved;
        public frm_role()
        {
            InitializeComponent();
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
        public void LoadData(String str, String frmCaption)
        {
            gStr = str;
            roleid = int.Parse(gStr);
            dtName = "SYS_Role";
            dtUser = new DataTable(dtName);
            Database.GetSqlData("select * from " + dtName + " where Role_id='" + str + "' ", dtUser);

            this.Text = frmCaption;


            DataTable dtpages = new DataTable();
            Database.GetSqlData("Select * from WinPage order by Pageid", dtpages);

            DataTable dtpagesroleSaved = new DataTable();
            Database.GetSqlData("Select * from WinPageRole where Role_id=" + gStr, dtpagesroleSaved);

            dtpagesroleTobeSaved = new DataTable("WinPageRole");
            Database.GetSqlData("Select * from WinPageRole where Role_id=0", dtpagesroleTobeSaved);




            for (int i = 0; i < dtpages.Rows.Count; i++)
            {



                dtpagesroleTobeSaved.Rows.Add();
                dtpagesroleTobeSaved.Rows[i]["Role_id"] = gStr;
                dtpagesroleTobeSaved.Rows[i]["Page_id"] = dtpages.Rows[i]["PageId"].ToString();

                if (dtpagesroleSaved.Select("Page_id=" + dtpages.Rows[i]["PageId"].ToString()).Length > 0)
                {
                    dtpagesroleTobeSaved.Rows[i]["Visible"] = dtpagesroleSaved.Select("Page_id=" + dtpages.Rows[i]["PageId"].ToString()).FirstOrDefault()["Visible"];
                    if (dtpagesroleSaved.Select("Page_id=" + dtpages.Rows[i]["PageId"].ToString()).FirstOrDefault()["Feature"].ToString() == null || dtpagesroleSaved.Select("Page_id=" + dtpages.Rows[i]["PageId"].ToString()).FirstOrDefault()["Feature"].ToString()=="")
                    {
                        dtpagesroleTobeSaved.Rows[i]["Feature"] = dtpages.Rows[i]["Feature"].ToString();
                    }
                    else
                    {
                        dtpagesroleTobeSaved.Rows[i]["Feature"] = dtpagesroleSaved.Select("Page_id=" + dtpages.Rows[i]["PageId"].ToString()).FirstOrDefault()["Feature"];
                    }
                }
                else
                {
                    dtpagesroleTobeSaved.Rows[i]["Visible"] = false;
                    dtpagesroleTobeSaved.Rows[i]["Feature"] = dtpages.Rows[i]["Feature"].ToString();
                }

                TreeNode[] pnt = treeView1.Nodes.Find(dtpages.Rows[i]["ParentPageid"].ToString(), true);
                if (pnt.Length > 0)
                {

                    TreeNode chld = pnt[0].Nodes.Add(dtpages.Rows[i]["PageId"].ToString(), dtpages.Rows[i]["PageTitle"].ToString());
                    chld.Checked = bool.Parse(dtpagesroleTobeSaved.Rows[i]["Visible"].ToString());

                }
                else
                {
                    TreeNode chld = treeView1.Nodes.Add(dtpages.Rows[i]["PageId"].ToString(), dtpages.Rows[i]["PageTitle"].ToString());
                    chld.Checked = bool.Parse(dtpagesroleTobeSaved.Rows[i]["Visible"].ToString());
                }

            }
                

              


            if (dtUser.Rows.Count == 0)
            {
                dtUser.Rows.Add(0);
                textBox1.Select();
                textBox1.Text = "";
               
            }
            else
            {
                textBox1.Select();
                textBox1.ReadOnly=true;
                textBox1.Text = dtUser.Rows[0]["RoleName"].ToString();

               
              

                   
               
             
            }
        }

        private void save()
        {
            User = textBox1.Text;
            dtUser.Rows[0]["RoleName"] = textBox1.Text;
           
            Database.SaveData(dtUser);

            if (gStr == "0")
            {
                roleid = Database.GetScalarInt("Select max(Role_id) from SYS_Role") + 1;
            }
            else
            {
                roleid = int.Parse(gStr);
            }

            DataTable dtpagesroleSaved = new DataTable("WinPageRole");
            Database.GetSqlData("Select * from WinPageRole where Role_id=" + roleid, dtpagesroleSaved);
            for (int i = 0; i < dtpagesroleSaved.Rows.Count; i++)
            {
                dtpagesroleSaved.Rows[i].Delete();
            }
            Database.SaveData(dtpagesroleSaved);

            for (int i = 0; i < dtpagesroleTobeSaved.Rows.Count; i++)
            {
               TreeNode[] tnode =  treeView1.Nodes.Find(dtpagesroleTobeSaved.Rows[i]["Page_id"].ToString(), true);
               dtpagesroleTobeSaved.Rows[i]["Role_id"] = roleid;
               if (tnode.Length > 0)
               {
                   dtpagesroleTobeSaved.Rows[i]["visible"] = tnode[0].Checked;
               }
            }
            Database.SaveData(dtpagesroleTobeSaved);



            funs.ShowBalloonTip("Saved", "Saved Successfully");

                this.Close();
                this.Dispose();
            

        }

        private bool validate()
        {
            if (textBox1.Text.Trim() == "")
            {
                textBox1.BackColor = Color.Aqua;
                textBox1.Focus();
                return false;
            }
            if (funs.Select_Role_id(textBox1.Text) != 0 && funs.Select_Role_id(textBox1.Text) != int.Parse(gStr))
            {
                MessageBox.Show("Role Name Already Exists.");
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
            if (gStr != "0")
            {
                if (Database.utype.ToUpper() == "USER")
                {
                    dtsidefill.Rows[0]["Visible"] = false;
                }
                else
                {
                    dtsidefill.Rows[0]["Visible"] = true;
                }
            }
            else
            {
                dtsidefill.Rows[0]["Visible"] = true;
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
                }
            }
            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
            
        }

        private void frm_role_Load(object sender, EventArgs e)
        {
            SideFill();


        }

        private void frm_role_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                if (validate() == true)
                {
                    save();
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

            if (dtpagesroleTobeSaved.Select("Page_id=" + e.Node.Name).Length > 0)
            {
                List<UsersFeature> objlist = new List<UsersFeature>();
                string str = dtpagesroleTobeSaved.Select("Page_id=" + e.Node.Name).FirstOrDefault()["Feature"].ToString();
                JavaScriptSerializer obj = new JavaScriptSerializer();
                objlist = obj.Deserialize<List<UsersFeature>>(str);
                flowLayoutPanel2.Controls.Clear();
                if (objlist == null)
                {
                    return;
                }
                foreach (UsersFeature uf in objlist)
                {
                    if (uf.FeatureType == "MultiValue")
                    {
                        ComboBox cb = new ComboBox();
                      //  cb.DataSource = uf.PossibleValues;

                        foreach (string str1 in uf.PossibleValues)
                        {
                            cb.Items.Add(str1);
                        }

                        cb.Text = uf.SelectedValue;
                        cb.Tag = e.Node.Name + ";" + uf.FeatureName;
                        Label lb = new Label();
                        lb.Text = uf.FeatureName;
                        lb.Width = 250;
                        cb.SelectedIndexChanged += new EventHandler(cb_SelctedIndexChanged);
                        flowLayoutPanel2.Controls.Add(lb);
                        flowLayoutPanel2.Controls.Add(cb);

                    }
                    else if (uf.FeatureType == "SingleValue")
                    {
                        TextBox tb = new TextBox();
                        //  cb.DataSource = uf.PossibleValues;



                        tb.Text = uf.SelectedValue;
                        tb.Tag = e.Node.Name + ";" + uf.FeatureName;
                        Label lb = new Label();
                        lb.Text = uf.FeatureName;
                        tb.TextChanged += new EventHandler(tb_TextChanged);
                        flowLayoutPanel2.Controls.Add(lb);
                        flowLayoutPanel2.Controls.Add(tb);

                    }
                }

            }
        }


        void cb_SelctedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
           string pageid =  cb.Tag.ToString().Split(';')[0];
           string feature = cb.Tag.ToString().Split(';')[1];

            DataRow dtr=dtpagesroleTobeSaved.Select("Page_Id=" + pageid).FirstOrDefault();
            if (dtr == null)
            { return; }

            string str = dtr["Feature"].ToString();
           List<UsersFeature> objlist = new List<UsersFeature>();
           JavaScriptSerializer obj = new JavaScriptSerializer();

           objlist = obj.Deserialize<List<UsersFeature>>(str);

         UsersFeature ob=  objlist.Where(w => w.FeatureName ==  feature).FirstOrDefault();
         if (ob != null)
         {
             ob.SelectedValue = cb.Text;
         }

         dtr["Feature"] = obj.Serialize(objlist);

        }
        void tb_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            string pageid = tb.Tag.ToString().Split(';')[0];
            string feature = tb.Tag.ToString().Split(';')[1];

            DataRow dtr = dtpagesroleTobeSaved.Select("Page_Id=" + pageid).FirstOrDefault();
            if (dtr == null)
            { return; }

            string str = dtr["Feature"].ToString();
            List<UsersFeature> objlist = new List<UsersFeature>();
            JavaScriptSerializer obj = new JavaScriptSerializer();

            objlist = obj.Deserialize<List<UsersFeature>>(str);
            UsersFeature ob = objlist.Where(w => w.FeatureName == feature).FirstOrDefault();
            if (ob != null)
            {
                ob.SelectedValue = tb.Text;
            }

            dtr["Feature"] = obj.Serialize(objlist);

        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
           // MessageBox.Show(e.Node.Name + " " + e.Node.Checked);
        }

    }
}
