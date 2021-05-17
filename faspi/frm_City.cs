using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace faspi
{
    public partial class frm_City : Form
    {
        DataTable dtCity;
        String dtName;
        public bool calledIndirect = false;
        public String cityName;
        public string gStr = "";
        
        public frm_City()
        {
            InitializeComponent();
        }

        public void LoadData(String str, String frmCaption)
        {
            gStr = str;
            dtName = "city";
            dtCity = new DataTable(dtName);
            Database.GetSqlData("select * from " + dtName + " where City_id='" + str+"'", dtCity);

            this.Text = frmCaption;
            if (dtCity.Rows.Count == 0)
            {
                dtCity.Rows.Add(0);
                TextBox1.Select();
                TextBox1.Text = "";
              
            }
            else
            {
                TextBox1.Select();
                TextBox1.Text = dtCity.Rows[0]["Cname"].ToString();
              
            }
        }

        private void save()
        {
            cityName = TextBox1.Text;
           
            if (gStr == "0")
            {
                DataTable dtCount = new DataTable();
                Database.GetSqlData("select count(*) from City where locationid='" + Database.LocationId + "'", dtCount);

                if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                {
                    dtCity.Rows[0]["City_id"] = Database.LocationId + "1";
                    dtCity.Rows[0]["Nid"] = 1;
                    dtCity.Rows[0]["LocationId"] = Database.LocationId;
                  
                }
                else
                {
                    DataTable dtid = new DataTable();
                    Database.GetSqlData("select max(Nid) as Nid from City where locationid='" + Database.LocationId + "'", dtid);
                    int Nid = int.Parse(dtid.Rows[0][0].ToString());
                    dtCity.Rows[0]["City_id"] = Database.LocationId + (Nid + 1);
                    dtCity.Rows[0]["Nid"] = (Nid + 1);
                    dtCity.Rows[0]["LocationId"] = Database.LocationId;
                   
                }
            }

            dtCity.Rows[0]["Cname"] = TextBox1.Text;
          
            Database.SaveData(dtCity);
           
            funs.ShowBalloonTip("Saved", "Saved Successfully");

        }

        private bool validate()
        {
            if (TextBox1.Text.Trim() == "")
            {
                TextBox1.BackColor = Color.Aqua;
                TextBox1.Focus();
                return false;
            }


            if (funs.Select_city_id(TextBox1.Text) != "" && funs.Select_city_id(TextBox1.Text) != gStr)
            {
                MessageBox.Show("City Name Already Exists.");
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
                if (Database.utype == "User")
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




            if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }

        private void frm_City_Load(object sender, EventArgs e)
        {
            SideFill();
        }

        private void TextBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(TextBox1);
        }

        private void TextBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(TextBox1);

        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void frm_City_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                if (validate() == true)
                {
                    if (Database.utype == "Admin" || Database.utype == "SuperAdmin")
                    {
                        save();


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

                    else if (gStr == "0")
                    {
                        save();

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
            }

            if (e.KeyCode == Keys.Escape)
            {
                if (TextBox1.Text != "")
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

    }
}
