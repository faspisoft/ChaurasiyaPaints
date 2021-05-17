﻿using System;
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
    public partial class frm_container : Form
    {
        DataTable dtcontainer;
        string gstr = "";
        public string cname;
        public bool calledIndirect = false;
        List<UsersFeature> permission;

        public frm_container()
        {
            InitializeComponent();
        }

        private void frm_container_Load(object sender, EventArgs e)
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
           // dtsidefill.Rows[0]["Visible"] = true;

            permission = funs.GetPermissionKey("Container");
            //create
            UsersFeature ob = permission.Where(w => w.FeatureName == "Create").FirstOrDefault();
            if (ob != null && gstr == "0" && ob.SelectedValue == "Allowed")
            {
                dtsidefill.Rows[0]["Visible"] = true;
            }
            else if (gstr == "0")
            {
                dtsidefill.Rows[0]["Visible"] = false;
            }

            //alter
            ob = permission.Where(w => w.FeatureName == "Alter").FirstOrDefault();
            if (ob != null && gstr != "0" && ob.SelectedValue == "Allowed")
            {
                dtsidefill.Rows[0]["Visible"] = true;
            }
            else if (gstr != "0")
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
                    if (ob != null && gstr == "0" && ob.SelectedValue == "Allowed")
                    {
                        save();
                    }

                    //alter
                    ob = permission.Where(w => w.FeatureName == "Alter").FirstOrDefault();
                    if (ob != null && gstr != "0" && ob.SelectedValue == "Allowed")
                    {
                        save();
                    }
                    //if (Database.utype.ToUpper() == "SUPERADMIN" || Database.utype.ToUpper() == "ADMIN")
                    //{
                    //    save();
                    //}

                    //else if (gstr == "0")
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
            cname = TextBox1.Text;

            if (gstr == "0")
            {
                DataTable dtCount = new DataTable();
                Database.GetSqlData("select count(*) from Container where locationid='" + Database.LocationId + "'", dtCount);

                if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                {
                    dtcontainer.Rows[0]["id"] = Database.LocationId + "1";
                    dtcontainer.Rows[0]["Nid"] = 1;
                    dtcontainer.Rows[0]["LocationId"] = Database.LocationId;
                    dtcontainer.Rows[0]["user_id"] = Database.user_id;
                    dtcontainer.Rows[0]["Modifiedby"] = "";
                }
                else
                {
                    DataTable dtid = new DataTable();
                    Database.GetSqlData("select max(Nid) as Nid from Container where locationid='" + Database.LocationId + "'", dtid);
                    int Nid = int.Parse(dtid.Rows[0][0].ToString());
                    dtcontainer.Rows[0]["id"] = Database.LocationId + (Nid + 1);
                    dtcontainer.Rows[0]["Nid"] = (Nid + 1);
                    dtcontainer.Rows[0]["LocationId"] = Database.LocationId;
                    dtcontainer.Rows[0]["user_id"] = Database.user_id;
                    dtcontainer.Rows[0]["Modifiedby"] = "";
                }
            }
            else
            {
                dtcontainer.Rows[0]["Modifiedby"] = Database.user_id;
            }
            dtcontainer.Rows[0]["Cname"] = TextBox1.Text;

            Database.SaveData(dtcontainer);
            funs.ShowBalloonTip("Saved", "Saved Successfully");

            if (calledIndirect == true)
            {
                this.Close();
                this.Dispose();
            }
            if (gstr == "0")
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
            if (TextBox1.Text == "")
            {
                MessageBox.Show("Please Enter Container Name");
                TextBox1.BackColor = Color.Aqua;
                TextBox1.Focus();
                return false;
            }
            if (funs.Select_state_id(TextBox1.Text) != "" && funs.Select_state_id(TextBox1.Text) != gstr)
            {
                MessageBox.Show("Container Name Already Exists.");
                return false;
            }

            return true;
        }

        public void LoadData(string str, string FrmCaption)
        {
            gstr = str;
            dtcontainer = new DataTable("Container");
            Database.GetSqlData("Select * From Container Where id='" + str + "'", dtcontainer);
            this.Text = FrmCaption;

            if (str == "0")
            {
                dtcontainer.Rows.Add();
                TextBox1.Text = "";
            }
            else
            {
                TextBox1.Text = dtcontainer.Rows[0]["Cname"].ToString();
            }
        }

        private void frm_container_KeyDown(object sender, KeyEventArgs e)
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


                    //create
                    UsersFeature ob = permission.Where(w => w.FeatureName == "Create").FirstOrDefault();
                    if (ob != null && gstr == "0" && ob.SelectedValue == "Allowed")
                    {
                        save();
                    }

                    //alter
                    ob = permission.Where(w => w.FeatureName == "Alter").FirstOrDefault();
                    if (ob != null && gstr != "0" && ob.SelectedValue == "Allowed")
                    {
                        save();
                    }
                    //if (Database.utype.ToUpper() == "SUPERADMIN" || Database.utype.ToUpper() == "ADMIN")
                    //{
                    //    save();
                    //}
                    //else if (gstr == "0")
                    //{
                    //    save();
                    //}
                }
            }
        }

        private void TextBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(TextBox1);
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void TextBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(TextBox1);
        }

    }
}
