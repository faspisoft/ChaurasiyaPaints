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
    public partial class ControlRoom : Form
    {
        DataTable firmsetup;
        string gstr = "";
        string type = "";

        public ControlRoom()
        {
            InitializeComponent();
            dateTimePicker1.CustomFormat = Database.dformat;
            dateTimePicker1.Value = Database.stDate;

           
        }

        private void ControlRoom_KeyDown(object sender, KeyEventArgs e)
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
                this.Close();
                this.Dispose();
            }
        }

        public void Loaddata(string str, string frmcaption)
        {
            gstr = str;
            this.Text = frmcaption;
            firmsetup = new DataTable("FirmSetup");
            Database.GetSqlData("select * from FirmSetup where ID=" + str, firmsetup);
            
            if (firmsetup.Rows.Count < 0)
            {
                firmsetup.Rows.Add();
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                //textBox6.Text = "";
                //textBox7.Text = "";
                //textBox8.Text = "";
                //textBox9.Text = "";
                textBox10.Text = "";
                //textBox11.Text = "";
                //textBox12.Text = "";
                //textBox13.Text = "";
                //textBox14.Text = "";
            }
            else
            {
                type = firmsetup.Rows[0]["Type"].ToString();
                textBox1.Text = firmsetup.Rows[0]["Group"].ToString();
                textBox2.Text = firmsetup.Rows[0]["Features"].ToString();
                textBox3.Text = firmsetup.Rows[0]["Description"].ToString();
                textBox4.Text = firmsetup.Rows[0]["OptionValues"].ToString();
                
                //superadmin
                textBox5.Text = firmsetup.Rows[0]["ToSuperAdmin"].ToString();
                textBox10.Text = firmsetup.Rows[0]["ToSuperAdmin"].ToString();

                ////admin
                //textBox6.Text = firmsetup.Rows[0]["ToAdmin"].ToString();
                //textBox7.Text = firmsetup.Rows[0]["ToAdmin"].ToString();

                ////superuser
                //textBox8.Text = firmsetup.Rows[0]["ToSuperUser"].ToString();
                //textBox9.Text = firmsetup.Rows[0]["ToSuperUser"].ToString();

                ////user
                //textBox11.Text = firmsetup.Rows[0]["ToUser"].ToString();
                //textBox12.Text = firmsetup.Rows[0]["ToUser"].ToString();

                ////cashier
                //textBox13.Text = firmsetup.Rows[0]["ToCashier"].ToString();
                //textBox14.Text = firmsetup.Rows[0]["ToCashier"].ToString();

                if (firmsetup.Rows[0]["Type"].ToString() == "DateTime")
                {
                    //superadmin
                    if (firmsetup.Rows[0]["ToSuperAdmin"].ToString() == "No")
                    {
                        groupBox2.Visible = false;
                    }
                    else
                    {
                        groupBox2.Visible = true;
                        dateTimePicker1.Value = DateTime.Parse(firmsetup.Rows[0]["ToSuperAdmin"].ToString());
                    }

                    ////admin
                    //if (firmsetup.Rows[0]["ToAdmin"].ToString() == "No")
                    //{
                    //    groupBox5.Visible = false;
                    //}
                    //else
                    //{
                    //    groupBox5.Visible = true;
                    //    dateTimePicker2.Value = DateTime.Parse(firmsetup.Rows[0]["ToAdmin"].ToString());
                    //}

                    ////superuser
                    //if (firmsetup.Rows[0]["ToSuperUser"].ToString() == "No")
                    //{
                    //    groupBox7.Visible = false;
                    //}
                    //else
                    //{
                    //    groupBox7.Visible = true;
                    //    dateTimePicker3.Value = DateTime.Parse(firmsetup.Rows[0]["ToSuperUser"].ToString());
                    //}

                    ////user
                    //if (firmsetup.Rows[0]["ToUser"].ToString() == "No")
                    //{
                    //    groupBox9.Visible = false;
                    //}
                    //else
                    //{
                    //    groupBox9.Visible = true;
                    //    dateTimePicker4.Value = DateTime.Parse(firmsetup.Rows[0]["ToUser"].ToString());
                    //}

                    ////cashier
                    //if (firmsetup.Rows[0]["ToCashier"].ToString() == "No")
                    //{
                    //    groupBox11.Visible = false;
                    //}
                    //else
                    //{
                    //    groupBox11.Visible = true;
                    //    dateTimePicker5.Value = DateTime.Parse(firmsetup.Rows[0]["ToCashier"].ToString());
                    //}
                }
                else if (firmsetup.Rows[0]["Type"].ToString() == "Textbox")
                {
                    //superadmin
                    textBox10.Visible = false;
                    label4.Visible = false;
                    textBox5.Visible = true;
                    label6.Visible = true;

                    ////admin
                    //textBox7.Visible = false;
                    //label7.Visible = false;
                    //textBox6.Visible = true;
                    //label9.Visible = true;

                    ////superuser
                    //textBox9.Visible = false;
                    //label10.Visible = false;
                    //textBox8.Visible = true;
                    //label12.Visible = true;

                    ////user
                    //textBox12.Visible = false;
                    //label13.Visible = false;
                    //textBox11.Visible = true;
                    //label15.Visible = true;

                    ////cashier
                    //textBox14.Visible = false;
                    //label16.Visible = false;
                    //textBox13.Visible = true;
                    //label18.Visible = true;
                }
            }
        }

        private bool validate()
        {
           
            return true;
        }

        private void save()
        {
            firmsetup.Rows[0]["ToSuperAdmin"] = textBox10.Text;
            //firmsetup.Rows[0]["ToAdmin"] = textBox7.Text;
            //firmsetup.Rows[0]["ToSuperUser"] = textBox9.Text;
            //firmsetup.Rows[0]["ToUser"] = textBox12.Text;
            //firmsetup.Rows[0]["ToCashier"] = textBox14.Text;

            Database.SaveData(firmsetup);
            Master.UpdateFeature();
            Master.UpdateRates();
            Master.UpdateAll();
            funs.ShowBalloonTip("Saved", "Saved Successfully");

            Database.TextCase = Feature.Available("Text Case");

            if (gstr == "0")
            {
                Loaddata("0", this.Text);
            }
            else
            {
                this.Close();
                this.Dispose();
            }
        }

        private void textBox10_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataTable dtcombo = new DataTable();
            dtcombo.Columns.Add("Options", typeof(string));
            String[] strtemp = textBox4.Text.Split(';');
            if (strtemp.Length != 1)
            {
                for (int j = 0; j < strtemp.Length; j++)
                {
                    if (strtemp[j] != "")
                    {
                        dtcombo.Rows.Add();
                        dtcombo.Rows[j][0] = strtemp[j].ToString();
                    }
                }
                textBox10.Text = SelectCombo.ComboDt(this, dtcombo, 0);
            }
            else
            {
                strtemp = textBox4.Text.Split('|');
                for (int j = 0; j < strtemp.Length; j++)
                {
                    if (strtemp[j] != "")
                    {
                        dtcombo.Rows.Add();
                        dtcombo.Rows[j][0] = strtemp[j].ToString();
                    }
                }
                textBox10.Text = SelectCombo.ComboDt(this, dtcombo, 0);
                if (textBox10.Text != "No")
                {
                    groupBox2.Visible = true;
                    textBox10.Text = dateTimePicker1.Value.Date.ToString(Database.dformat);
                }
                else
                {
                    groupBox2.Visible = false;
                }
            }
        }

        private void textBox10_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox10);
        }

        private void textBox10_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox10);
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
                    save();
                }
            }
            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }

        private void ControlRoom_Load(object sender, EventArgs e)
        {
            this.Size = this.MdiParent.Size;
            SideFill();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (type == "DateTime")
            {
                textBox10.Text = dateTimePicker1.Value.Date.ToString(Database.dformat);
            }
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            textBox10.Text = textBox5.Text;
            Database.lostFocus(textBox5);
        }

        

    

      

      


        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox9_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox12_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox14_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

  

        private void textBox5_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox5);
        }

    
     

      

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox8_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox11_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox13_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(dateTimePicker1);
        }

    

        private void dateTimePicker1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(dateTimePicker1);
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker3_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker4_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker5_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void textBox13_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }
    }
}
