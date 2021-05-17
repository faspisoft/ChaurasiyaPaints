using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace faspi
{
    public partial class frmBroker : Form
    {
        DataTable dtBroker;
        String dtName;
        public bool calledIndirect = false;
        public String BrokerName;
        String gStr;

        public frmBroker()
        {
            InitializeComponent();
        }

        private void frmBroker_Load(object sender, EventArgs e)
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
            dtsidefill.Rows[0]["Visible"] = true;

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
                    if (Database.utype.ToUpper() == "SUPERADMIN" || Database.utype.ToUpper() == "ADMIN")
                    {
                        save();
                    }

                    else if (gStr == "0")
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

        public void LoadData(String str, String frmCaption)
        {
            gStr = str;
            dtName = "contractor";
            this.Text = frmCaption;
            dtBroker = new DataTable(dtName);
            Database.GetSqlData("select * from " + dtName + " where con_id='" + str + "'", dtBroker);
            
            if (dtBroker.Rows.Count == 0)
            {
                dtBroker.Rows.Add(0);
                TextBox1.Text = "";
                TextBox4.Text = "";
                TextBox5.Text = "";
                TextBox2.Text = "";
                TextBox3.Text = "";
                textBox6.Text = "";
            }
            else
            {
                TextBox1.Text = dtBroker.Rows[0]["name"].ToString();
                TextBox2.Text = dtBroker.Rows[0]["address1"].ToString();
                TextBox3.Text = dtBroker.Rows[0]["address2"].ToString();
                TextBox4.Text = dtBroker.Rows[0]["phone"].ToString();
                TextBox5.Text = dtBroker.Rows[0]["email"].ToString();
                textBox6.Text = funs.Select_ac_nm(dtBroker.Rows[0]["Reff_id"].ToString());
            }
        }

        private void save()
        {
            BrokerName = TextBox1.Text;

            if (gStr == "0")
            {
                DataTable dtCount = new DataTable();
                Database.GetSqlData("select count(*) from CONTRACTOR where locationid='" + Database.LocationId + "'", dtCount);

                if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                {
                    dtBroker.Rows[0]["Con_id"] = Database.LocationId + "1";
                    dtBroker.Rows[0]["Nid"] = 1;
                    dtBroker.Rows[0]["LocationId"] = Database.LocationId;
                    dtBroker.Rows[0]["user_id"] = Database.user_id;
                    dtBroker.Rows[0]["Modifiedby"] = "";
                }
                else
                {
                    DataTable dtid = new DataTable();
                    Database.GetSqlData("select max(Nid) as Nid from CONTRACTOR where locationid='" + Database.LocationId + "'", dtid);
                    int Nid = int.Parse(dtid.Rows[0][0].ToString());
                    dtBroker.Rows[0]["Con_id"] = Database.LocationId + (Nid + 1);
                    dtBroker.Rows[0]["Nid"] = (Nid + 1);
                    dtBroker.Rows[0]["LocationId"] = Database.LocationId;
                    dtBroker.Rows[0]["user_id"] = Database.user_id;
                    dtBroker.Rows[0]["Modifiedby"] = "";
                }
            }
            else
            {
                dtBroker.Rows[0]["Modifiedby"] = Database.user_id;
            }

            dtBroker.Rows[0]["name"] = TextBox1.Text;
            dtBroker.Rows[0]["Address1"] = TextBox2.Text;
            dtBroker.Rows[0]["Address2"] = TextBox3.Text;
            dtBroker.Rows[0]["phone"] = TextBox4.Text;
            dtBroker.Rows[0]["email"] = TextBox5.Text;
            dtBroker.Rows[0]["Reff_id"] = funs.Select_ac_id(textBox6.Text);
            dtBroker.Rows[0]["Branch_id"] = Database.BranchId;
            Database.SaveData(dtBroker);
            //Master.UpdateAgent();
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
            if (TextBox1.Text == "")
            {
                TextBox1.BackColor = Color.Aqua;
                TextBox1.Focus();
                return false;
            }
           if (TextBox2.Text == "")
            {
                TextBox2.BackColor = Color.Aqua;
                TextBox2.Focus();
                return false;
            }

           if (funs.Select_con_id(TextBox1.Text) != "" && funs.Select_con_id(TextBox1.Text) != gStr)
           {
               MessageBox.Show("Broker Name Already Exists");
               return false;
           }
    
            return true;
        }

        private void frmBroker_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                if (validate() == true)
                {
                    if (Database.utype.ToUpper() == "SUPERADMIN" || Database.utype.ToUpper() == "ADMIN")
                    {
                        save();
                    }

                    else if (gStr == "0")
                    {
                        save();
                    }
                }
            }
            
            else if (e.KeyCode == Keys.Escape)
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

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void TextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void TextBox3_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void TextBox4_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void TextBox5_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\'')
            {
                e.Handled = true;
            }
        }

        private void textBox6_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox6);
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
           string strCombo = "SELECT Name FROM ACCOUNT ORDER BY Name";
            textBox6.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox6);
        }
    }
}
