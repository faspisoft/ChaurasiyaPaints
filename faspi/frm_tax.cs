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
    public partial class frm_tax : Form
    {
        public bool calledIndirect = false;
        public String TaxCategoryName;
        DataTable dtTaxCategory;
        public String cmdmode;
        String strCombo;
        String gStr;


        List<UsersFeature> permission;

        public frm_tax()
        {
            InitializeComponent();
        }

        private void frm_tax_Load(object sender, EventArgs e)
        {
            tabPage1.Text = "Within " + funs.Select_state_nm(Database.CompanyState_id);

            if (Feature.Available("Taxation Applicable") == "VAT")
            {
                groupBox1.Text = "Commodity Code";
                groupBox10.Text = "Tax Category Name";
                label2.Text = "VAT";
                label3.Text = "SAT";
                label12.Text = "SAT";
                label13.Text = "VAT";
                label18.Text = "CST";
                label25.Text = "CST";
                label4.Text = "Service Tax";
                label17.Text = "Service Tax";
                label11.Text = "Service Tax";
                label20.Text = "Service Tax";
                comboBox2.Items.Add("VAT");
                comboBox2.Items.Add("NON-VAT");
            }
            else
            {
                groupBox1.Text = "HSN Code";
                groupBox10.Text = "HSN Name";
                label2.Text = "CGST";
                label3.Text = "SGST";
                label12.Text = "SGST";
                label13.Text = "CGST";
                label18.Text = "IGST";
                label25.Text = "IGST";
                label4.Text = "Cess (If Any)";
                label17.Text = "Cess (If Any)";
                label11.Text = "Cess (If Any)";
                label20.Text = "Cess (If Any)";
                comboBox2.Items.Add("Goods");
                comboBox2.Items.Add("Services");
            }
            SideFill();
        }

        private void frm_tax_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (textBox4.Text != "")
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

            else if (e.Control && e.KeyCode == Keys.S)
            {
                if (validate() == true)
                {
                    //create
                    UsersFeature ob = permission.Where(w => w.FeatureName == "Create").FirstOrDefault();
                    if (ob != null && gStr == "0" && ob.SelectedValue == "Allowed")
                    {
                        Save();
                    }

                    //alter
                    ob = permission.Where(w => w.FeatureName == "Alter").FirstOrDefault();
                    if (ob != null && gStr != "0" && ob.SelectedValue == "Allowed")
                    {
                        Save();
                    }
                    //if (Database.utype.ToUpper() == "SUPERADMIN" || Database.utype.ToUpper() == "ADMIN")
                    //{
                    //    Save();
                    //}
                    //else if (gStr == "0")
                    //{
                    //    Save();
                    //}
                }
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            string wheresrt = "(Path LIKE '9;%') OR   (Path LIKE '10;%') or (Path LIKE '6;%') or (Path LIKE '37;%')";
            //strCombo = funs.GetStrCombo(wheresrt);
            strCombo = funs.GetStrCombonew(wheresrt, "1=1");
            textBox2.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void textBox7_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox7);
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            string wheresrt = "(Path LIKE '9;%')  OR   (Path LIKE '8;12;%')";
           // strCombo = funs.GetStrCombo(wheresrt);
            strCombo = funs.GetStrCombonew(wheresrt, "1=1");
            textBox7.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            string wheresrt = "(Path LIKE '9;%')  OR   (Path LIKE '8;12;%')";
            //strCombo = funs.GetStrCombo(wheresrt);
            strCombo = funs.GetStrCombonew(wheresrt, "1=1");
            textBox8.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox8_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox8.Text != "")
                {
                    textBox8.Text = funs.EditAccount(textBox8.Text);
                }
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                textBox8.Text = funs.AddAccount();
            }
        }

        private void textBox8_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox8);
        }

        private void textBox8_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox8);
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox7);
        }

        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox7.Text != "")
                {
                    textBox7.Text = funs.EditAccount(textBox7.Text);
                }
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                textBox7.Text = funs.AddAccount();
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox2.Text != "")
                {
                    textBox2.Text = funs.EditAccount(textBox2.Text);
                }
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                textBox2.Text = funs.AddAccount();
            }
        }

        private void textBox16_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox16);
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox3);
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox5);
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox3);
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox5_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox5);
        }



        private void textBox4_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox4);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox15_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox14_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox13_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox12_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox12);
        }

        private void textBox11_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox11);
        }

        private void textBox10_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox10);
        }

        private void textBox13_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox13);
        }

        private void textBox14_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox14);
        }

        private void textBox15_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox12);
        }

        private void textBox16_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox16);
        }

        private void textBox15_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox15);
        }

        private void textBox14_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox14);
        }

        private void textBox13_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox13);
        }

        private void textBox10_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox10);
        }

        private void textBox11_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox11);
        }

        private void textBox12_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox12);
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
            permission = funs.GetPermissionKey("Tax");
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
                    if (ob != null && gStr== "0" && ob.SelectedValue == "Allowed")
                    {
                        Save();
                    }

                    //alter
                    ob = permission.Where(w => w.FeatureName == "Alter").FirstOrDefault();
                    if (ob != null && gStr != "0" && ob.SelectedValue == "Allowed")
                    {
                        Save();
                    }
                    //if (Database.utype.ToUpper() == "SUPERADMIN" || Database.utype.ToUpper() == "ADMIN")
                    //{
                    //    Save();
                    //}
                    //else if (gStr == "0")
                    //{
                    //    Save();
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
            this.Text = frmCaption;

            dtTaxCategory = new DataTable("TaxCategory");
            Database.GetSqlData("Select * from TaxCategory where Category_id='" + str + " ' ", dtTaxCategory);

            if (dtTaxCategory.Rows.Count == 0)
            {
                dtTaxCategory.Rows.Add();
                textBox1.Text = "";
                textBox2.Text = "";
                textBox4.Text = "";
                textBox3.Text = "0";
                textBox5.Text = "0";
                textBox22.Text = "0";
                textBox25.Text = "0";
                textBox6.Text = "0";
                textBox20.Text = "0";
                textBox7.Text = "";
                textBox8.Text = "";
                textBox10.Text = "";
                textBox11.Text = "";
                textBox12.Text = "";
                textBox13.Text = "0";
                textBox18.Text = "0";
                textBox14.Text = "0";
                textBox15.Text = "0";
                textBox16.Text = "";
                textBox17.Text = "";
                textBox23.Text = "";
                textBox30.Text = "";
                textBox9.Text = "";
                textBox19.Text = "";
                textBox21.Text = "";
                textBox24.Text = "";
                textBox29.Text = "";
                textBox28.Text = "";
                textBox26.Text = "";
            }
            else
            {
                textBox4.Text = dtTaxCategory.Rows[0]["Category_Name"].ToString();
                textBox1.Text = dtTaxCategory.Rows[0]["Commodity_Code"].ToString();
                comboBox2.Text = dtTaxCategory.Rows[0]["Item_Type"].ToString();

                if (dtTaxCategory.Rows[0]["PA"].ToString() == "")
                {
                    dtTaxCategory.Rows[0]["PA"] = "0";
                }
                textBox2.Text = funs.Select_ac_nm(dtTaxCategory.Rows[0]["PA"].ToString());
                if (dtTaxCategory.Rows[0]["SA"].ToString() == "")
                {
                    dtTaxCategory.Rows[0]["SA"] = "0";
                }
                textBox16.Text = funs.Select_ac_nm(dtTaxCategory.Rows[0]["SA"].ToString());

                if (dtTaxCategory.Rows[0]["PAEX"].ToString() == "")
                {
                    dtTaxCategory.Rows[0]["PAEX"] = "0";
                }
                textBox23.Text = funs.Select_ac_nm(dtTaxCategory.Rows[0]["PAEX"].ToString());
                if (dtTaxCategory.Rows[0]["SAEX"].ToString() == "")
                {
                    dtTaxCategory.Rows[0]["SAEX"] = "0";
                }
                textBox30.Text = funs.Select_ac_nm(dtTaxCategory.Rows[0]["SAEX"].ToString());

                if (dtTaxCategory.Rows[0]["PCA"].ToString() == "")
                {
                    dtTaxCategory.Rows[0]["PCA"] = "0";
                }
                textBox9.Text = funs.Select_ac_nm(dtTaxCategory.Rows[0]["PCA"].ToString());

                if (dtTaxCategory.Rows[0]["SCA"].ToString() == "")
                {
                    dtTaxCategory.Rows[0]["SCA"] = "0";
                }
                textBox19.Text = funs.Select_ac_nm(dtTaxCategory.Rows[0]["SCA"].ToString());

                if (dtTaxCategory.Rows[0]["PCAEX"].ToString() == "")
                {
                    dtTaxCategory.Rows[0]["PCAEX"] = "0";
                }
                textBox21.Text = funs.Select_ac_nm(dtTaxCategory.Rows[0]["PCAEX"].ToString());

                if (dtTaxCategory.Rows[0]["SCAEX"].ToString() == "")
                {
                    dtTaxCategory.Rows[0]["SCAEX"] = "0";
                }
                textBox24.Text = funs.Select_ac_nm(dtTaxCategory.Rows[0]["SCAEX"].ToString());

                if (dtTaxCategory.Rows[0]["PCR"].ToString() == "")
                {
                    dtTaxCategory.Rows[0]["PCR"] = 0;
                }
                textBox6.Text = funs.DecimalPoint(double.Parse(dtTaxCategory.Rows[0]["PCR"].ToString()), 2);

                if (dtTaxCategory.Rows[0]["SCR"].ToString() == "")
                {
                    dtTaxCategory.Rows[0]["SCR"] = 0;
                }
                textBox20.Text = funs.DecimalPoint(double.Parse(dtTaxCategory.Rows[0]["SCR"].ToString()), 2);

                if (dtTaxCategory.Rows[0]["PCREX"].ToString() == "")
                {
                    dtTaxCategory.Rows[0]["PCREX"] = 0;
                }
                textBox22.Text = funs.DecimalPoint(double.Parse(dtTaxCategory.Rows[0]["PCREX"].ToString()), 2);


                if (dtTaxCategory.Rows[0]["SCREX"].ToString() == "")
                {
                    dtTaxCategory.Rows[0]["SCREX"] = 0;
                }
                textBox25.Text = funs.DecimalPoint(double.Parse(dtTaxCategory.Rows[0]["SCREX"].ToString()), 2);

                if (dtTaxCategory.Rows[0]["PTR1"].ToString() == "")
                {
                    dtTaxCategory.Rows[0]["PTR1"] = 0;
                }
                textBox3.Text = funs.DecimalPoint(double.Parse(dtTaxCategory.Rows[0]["PTR1"].ToString()), 2);

                if (dtTaxCategory.Rows[0]["PTR2"].ToString() == "")
                {
                    dtTaxCategory.Rows[0]["PTR2"] = 0;
                }
                textBox5.Text = funs.DecimalPoint(double.Parse(dtTaxCategory.Rows[0]["PTR2"].ToString()), 2);
                if (dtTaxCategory.Rows[0]["PTR3"].ToString() == "")
                {
                    dtTaxCategory.Rows[0]["PTR3"] = 0;
                }
                textBox18.Text = funs.DecimalPoint(double.Parse(dtTaxCategory.Rows[0]["PTR3"].ToString()), 2);
                if (dtTaxCategory.Rows[0]["STR1"].ToString() == "")
                {
                    dtTaxCategory.Rows[0]["STR1"] = 0;
                }
                textBox15.Text = funs.DecimalPoint(double.Parse(dtTaxCategory.Rows[0]["STR1"].ToString()), 2);
                if (dtTaxCategory.Rows[0]["STR2"].ToString() == "")
                {
                    dtTaxCategory.Rows[0]["STR2"] = 0;
                }
                textBox14.Text = funs.DecimalPoint(double.Parse(dtTaxCategory.Rows[0]["STR2"].ToString()), 2);
                if (dtTaxCategory.Rows[0]["STR3"].ToString() == "")
                {
                    dtTaxCategory.Rows[0]["STR3"] = 0;
                }
                textBox13.Text = funs.DecimalPoint(double.Parse(dtTaxCategory.Rows[0]["STR3"].ToString()), 2);

                if (dtTaxCategory.Rows[0]["PTA1"].ToString() == "")
                {
                    dtTaxCategory.Rows[0]["PTA1"] = "0";
                }
                textBox7.Text = funs.Select_ac_nm(dtTaxCategory.Rows[0]["PTA1"].ToString());
                if (dtTaxCategory.Rows[0]["PTA2"].ToString() == "")
                {
                    dtTaxCategory.Rows[0]["PTA2"] = "0";
                }
                textBox8.Text = funs.Select_ac_nm(dtTaxCategory.Rows[0]["PTA2"].ToString());
                if (dtTaxCategory.Rows[0]["PTA3"].ToString() == "")
                {
                    dtTaxCategory.Rows[0]["PTA3"] = "0";
                }
                textBox17.Text = funs.Select_ac_nm(dtTaxCategory.Rows[0]["PTA3"].ToString());

                if (dtTaxCategory.Rows[0]["STA1"].ToString() == "")
                {
                    dtTaxCategory.Rows[0]["STA1"] = "0";
                }

                textBox12.Text = funs.Select_ac_nm(dtTaxCategory.Rows[0]["STA1"].ToString());

                if (dtTaxCategory.Rows[0]["STA2"].ToString() == "")
                {
                    dtTaxCategory.Rows[0]["STA2"] = "0";
                }

                if (dtTaxCategory.Rows[0]["STA3"].ToString() == "")
                {
                    dtTaxCategory.Rows[0]["STA3"] = "0";
                }

                textBox11.Text = funs.Select_ac_nm(dtTaxCategory.Rows[0]["STA2"].ToString());
                textBox10.Text = funs.Select_ac_nm(dtTaxCategory.Rows[0]["STA3"].ToString());


                if (dtTaxCategory.Rows[0]["RCMPay"].ToString() == "")
                {
                    dtTaxCategory.Rows[0]["RCMPay"] = "0";
                }

                if (dtTaxCategory.Rows[0]["RCMITC"].ToString() == "")
                {
                    dtTaxCategory.Rows[0]["RCMITC"] = "0";
                }

                if (dtTaxCategory.Rows[0]["RCMEli"].ToString() == "")
                {
                    dtTaxCategory.Rows[0]["RCMEli"] = "0";
                }
                textBox29.Text = funs.Select_ac_nm(dtTaxCategory.Rows[0]["RCMPay"].ToString());
                textBox28.Text = funs.Select_ac_nm(dtTaxCategory.Rows[0]["RCMITC"].ToString());
                textBox26.Text = funs.Select_ac_nm(dtTaxCategory.Rows[0]["RCMEli"].ToString());
            }
        }

        private void Save()
        {
            TaxCategoryName = textBox4.Text;

            if (gStr == "0")
            {
                DataTable dtCount = new DataTable();
                Database.GetSqlData("select count(*) from TAXCATEGORY where locationid='" + Database.LocationId + "'", dtCount);

                if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                {
                    dtTaxCategory.Rows[0]["Category_Id"] = Database.LocationId + "1";
                    dtTaxCategory.Rows[0]["Nid"] = 1;
                    dtTaxCategory.Rows[0]["LocationId"] = Database.LocationId;
                    dtTaxCategory.Rows[0]["user_id"] = Database.user_id;
                    dtTaxCategory.Rows[0]["Modifiedby"] = "";
                }
                else
                {
                    DataTable dtid = new DataTable();
                    Database.GetSqlData("select max(Nid) as Nid from TAXCATEGORY where locationid='" + Database.LocationId + "'", dtid);
                    int Nid = int.Parse(dtid.Rows[0][0].ToString());
                    dtTaxCategory.Rows[0]["Category_Id"] = Database.LocationId + (Nid + 1);
                    dtTaxCategory.Rows[0]["Nid"] = (Nid + 1);
                    dtTaxCategory.Rows[0]["LocationId"] = Database.LocationId;
                    dtTaxCategory.Rows[0]["user_id"] = Database.user_id;
                    dtTaxCategory.Rows[0]["Modifiedby"] = "";
                }
            }
            else
            {
                dtTaxCategory.Rows[0]["Modifiedby"] = Database.user_id;
            }

            dtTaxCategory.Rows[0]["Category_Name"] = textBox4.Text;
            dtTaxCategory.Rows[0]["Commodity_Code"] = textBox1.Text;
            dtTaxCategory.Rows[0]["Item_Type"] = comboBox2.Text;
            dtTaxCategory.Rows[0]["PA"] = funs.Select_ac_id(textBox2.Text);
            dtTaxCategory.Rows[0]["SA"] = funs.Select_ac_id(textBox16.Text);
            dtTaxCategory.Rows[0]["PAEX"] = funs.Select_ac_id(textBox23.Text);
            dtTaxCategory.Rows[0]["SAEX"] = funs.Select_ac_id(textBox30.Text);
            dtTaxCategory.Rows[0]["PCAEX"] = funs.Select_ac_id(textBox21.Text);
            dtTaxCategory.Rows[0]["SCAEX"] = funs.Select_ac_id(textBox24.Text);
            dtTaxCategory.Rows[0]["PCA"] = funs.Select_ac_id(textBox9.Text);
            dtTaxCategory.Rows[0]["SCA"] = funs.Select_ac_id(textBox19.Text);
            dtTaxCategory.Rows[0]["PCR"] = double.Parse(textBox6.Text);
            dtTaxCategory.Rows[0]["SCR"] = double.Parse(textBox20.Text);
            dtTaxCategory.Rows[0]["PCREX"] = double.Parse(textBox22.Text);
            dtTaxCategory.Rows[0]["SCREX"] = double.Parse(textBox25.Text);
            dtTaxCategory.Rows[0]["PTR1"] = double.Parse(textBox3.Text);
            dtTaxCategory.Rows[0]["PTR2"] = double.Parse(textBox5.Text);
            dtTaxCategory.Rows[0]["PTR3"] = double.Parse(textBox18.Text);
            dtTaxCategory.Rows[0]["STR1"] = double.Parse(textBox15.Text);
            dtTaxCategory.Rows[0]["STR2"] = double.Parse(textBox14.Text);
            dtTaxCategory.Rows[0]["STR3"] = double.Parse(textBox13.Text);
            dtTaxCategory.Rows[0]["PTA1"] = funs.Select_ac_id(textBox7.Text);
            dtTaxCategory.Rows[0]["PTA2"] = funs.Select_ac_id(textBox8.Text);
            dtTaxCategory.Rows[0]["PTA3"] = funs.Select_ac_id(textBox17.Text);
            dtTaxCategory.Rows[0]["STA1"] = funs.Select_ac_id(textBox12.Text);
            dtTaxCategory.Rows[0]["STA2"] = funs.Select_ac_id(textBox11.Text);
            dtTaxCategory.Rows[0]["STA3"] = funs.Select_ac_id(textBox10.Text);
            dtTaxCategory.Rows[0]["RCMPay"] = funs.Select_ac_id(textBox29.Text);
            dtTaxCategory.Rows[0]["RCMITC"] = funs.Select_ac_id(textBox28.Text);
            dtTaxCategory.Rows[0]["RCMEli"] = funs.Select_ac_id(textBox26.Text);

            dtTaxCategory.Rows[0]["Local_Purchase"] = false;
            dtTaxCategory.Rows[0]["Local_Sale"] = false;
            dtTaxCategory.Rows[0]["Central_Purchase"] = false;
            dtTaxCategory.Rows[0]["Central_Sale"] = false;
         
            Database.SaveData(dtTaxCategory);
            Master.UpdateTaxCategory();
            funs.ShowBalloonTip("Saved", "Saved Successfully");
            if (calledIndirect == true)
            {
                this.Close();
                this.Dispose();
            }
            else if (gStr == "0")
            {
                LoadData("0", "");
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

            if (textBox4.Text == "")
            {
                textBox4.BackColor = Color.Aqua;
                textBox4.Focus();
                return false;
            }

            if (textBox3.Text.Trim() == "")
            {
                textBox3.Text = "0";

                return true;
            }
            if (textBox5.Text.Trim() == "")
            {
                textBox5.Text = "0";

                return true;
            }
            if (textBox5.Text.Trim() == "")
            {
                textBox5.Text = "0";

                return true;
            }
            if (textBox3.Text.Trim() == "")
            {
                textBox3.Text = "0";

                return true;
            }
            if (textBox13.Text.Trim() == "")
            {
                textBox13.Text = "0";

                return true;
            }
            if (textBox14.Text.Trim() == "")
            {
                textBox14.Text = "0";

                return true;
            }
            if (textBox15.Text.Trim() == "")
            {
                textBox15.Text = "0";

                return true;
            }
            if (comboBox2.Text == "")
            {
                comboBox2.BackColor = Color.Aqua;
                comboBox2.Focus();
                return false;
            }
            if (funs.Select_tax_cat_id(textBox4.Text) != "" && funs.Select_tax_cat_id(textBox4.Text) != gStr)
            {
                MessageBox.Show("Name Already Exists");
                textBox4.Focus();
                return false;
            }
            return true;
        }

        private void comboBox2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox16_KeyPress(object sender, KeyPressEventArgs e)
        {
            string wheresrt = "(Path LIKE '10;%') OR   (Path LIKE '9;%') or (Path LIKE '6;%') or (Path LIKE '37;%')";
            //strCombo = funs.GetStrCombo(wheresrt);
            strCombo = funs.GetStrCombonew(wheresrt, "1=1");
            textBox16.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox12_KeyPress(object sender, KeyPressEventArgs e)
        {
            string wheresrt = "(Path LIKE '10;%')  OR   (Path LIKE '8;12;%')";
           // strCombo = funs.GetStrCombo(wheresrt);
            strCombo = funs.GetStrCombonew(wheresrt, "1=1");
            textBox12.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            string wheresrt = "(Path LIKE '10;%')  OR   (Path LIKE '8;12;%')";
           // strCombo = funs.GetStrCombo(wheresrt);
            strCombo = funs.GetStrCombonew(wheresrt, "1=1");
            textBox11.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            string wheresrt = "(Path LIKE '10;%')  OR   (Path LIKE '8;12;%')";
           // strCombo = funs.GetStrCombo(wheresrt);
            strCombo = funs.GetStrCombonew(wheresrt, "1=1");
            textBox10.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox16_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox16.Text != "")
                {
                    textBox16.Text = funs.EditAccount(textBox16.Text);
                }
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                textBox16.Text = funs.AddAccount();
            }
        }

        private void textBox12_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox12.Text != "")
                {
                    textBox12.Text = funs.EditAccount(textBox12.Text);
                }
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                textBox12.Text = funs.AddAccount();
            }
        }

        private void textBox11_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox11.Text != "")
                {
                    textBox11.Text = funs.EditAccount(textBox11.Text);
                }
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                textBox11.Text = funs.AddAccount();
            }
        }

        private void textBox10_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox10.Text != "")
                {
                    textBox10.Text = funs.EditAccount(textBox10.Text);
                }
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                textBox10.Text = funs.AddAccount();
            }
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox4);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox15_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox14_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox13_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox23_KeyPress(object sender, KeyPressEventArgs e)
        {
            string wheresrt = "(Path LIKE '9;%') OR   (Path LIKE '10;%') or (Path LIKE '6;%') or (Path LIKE '37;%')";
           // strCombo = funs.GetStrCombo(wheresrt);
            strCombo = funs.GetStrCombonew(wheresrt, "1=1");
            textBox23.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox30_KeyPress(object sender, KeyPressEventArgs e)
        {
            string wheresrt = "(Path LIKE '10;%') OR   (Path LIKE '9;%') or (Path LIKE '6;%') or (Path LIKE '37;%')";
          //  strCombo = funs.GetStrCombo(wheresrt);
            strCombo = funs.GetStrCombonew(wheresrt, "1=1");
            textBox30.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox23_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox23.Text != "")
                {
                    textBox23.Text = funs.EditAccount(textBox23.Text);
                }
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                textBox23.Text = funs.AddAccount();
            }
        }

        private void textBox30_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox30.Text != "")
                {
                    textBox30.Text = funs.EditAccount(textBox30.Text);
                }
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                textBox30.Text = funs.AddAccount();
            }
        }

        private void textBox23_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox23);
        }

        private void textBox30_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox30);
        }

        private void textBox23_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox23);
        }

        private void textBox30_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox30);
        }

        private void textBox17_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox17);
        }

        private void textBox17_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox17);
        }

        private void textBox18_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox18);
        }

        private void textBox18_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox18);
        }

        private void textBox18_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox17_KeyPress(object sender, KeyPressEventArgs e)
        {
            string wheresrt = "(Path LIKE '9;%')  OR   (Path LIKE '8;12;%')";
           // strCombo = funs.GetStrCombo(wheresrt);
            strCombo = funs.GetStrCombonew(wheresrt, "1=1");
            textBox17.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox17_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox17.Text != "")
                {
                    textBox17.Text = funs.EditAccount(textBox17.Text);
                }
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                textBox17.Text = funs.AddAccount();
            }
        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            string wheresrt = "(Path LIKE '9;%')  OR   (Path LIKE '8;12;%')";
            //strCombo = funs.GetStrCombo(wheresrt);
            strCombo = funs.GetStrCombonew(wheresrt, "1=1");
            textBox9.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox19_KeyPress(object sender, KeyPressEventArgs e)
        {
            string wheresrt = "(Path LIKE '10;%')  OR   (Path LIKE '8;12;%')";
            //strCombo = funs.GetStrCombo(wheresrt);
            strCombo = funs.GetStrCombonew(wheresrt, "1=1");
            textBox19.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox21_KeyPress(object sender, KeyPressEventArgs e)
        {
            string wheresrt = "(Path LIKE '9;%')  OR   (Path LIKE '8;12;%')";
           // strCombo = funs.GetStrCombo(wheresrt);
            strCombo = funs.GetStrCombonew(wheresrt, "1=1");
            textBox21.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox24_KeyPress(object sender, KeyPressEventArgs e)
        {
            string wheresrt = "(Path LIKE '10;%')  OR   (Path LIKE '8;12;%')";
            //strCombo = funs.GetStrCombo(wheresrt);
            strCombo = funs.GetStrCombonew(wheresrt, "1=1");
            textBox24.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox22_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox25_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox24_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox24.Text != "")
                {
                    textBox24.Text = funs.EditAccount(textBox24.Text);
                }
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                textBox24.Text = funs.AddAccount();
            }
        }

        private void textBox21_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox21.Text != "")
                {
                    textBox21.Text = funs.EditAccount(textBox21.Text);
                }
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                textBox21.Text = funs.AddAccount();
            }
        }

        private void textBox9_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox9.Text != "")
                {
                    textBox9.Text = funs.EditAccount(textBox9.Text);
                }
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                textBox9.Text = funs.AddAccount();
            }
        }

        private void textBox19_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox19.Text != "")
                {
                    textBox19.Text = funs.EditAccount(textBox19.Text);
                }
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                textBox19.Text = funs.AddAccount();
            }
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox6);
        }

        private void textBox6_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox6);
        }

        private void textBox20_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox20_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox20_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox20);
        }

        private void textBox20_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox20);
        }

        private void textBox9_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox9);
        }

        private void textBox9_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox9);
        }

        private void textBox19_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox19);
        }

        private void textBox19_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox19);
        }

        private void textBox22_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox22);
        }

        private void textBox22_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox22);
        }

        private void textBox22_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox25_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox25_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox25);
        }

        private void textBox25_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox25);
        }

        private void textBox21_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox21);
        }

        private void textBox21_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox21);
        }

        private void textBox24_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox24);
        }

        private void textBox24_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox24);
        }

        private void textBox29_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox29.Text != "")
                {
                    textBox29.Text = funs.EditAccount(textBox29.Text);
                }
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                textBox29.Text = funs.AddAccount();
            }
        }

        private void textBox28_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox28.Text != "")
                {
                    textBox28.Text = funs.EditAccount(textBox28.Text);
                }
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                textBox28.Text = funs.AddAccount();
            }
        }

        private void textBox26_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox26.Text != "")
                {
                    textBox26.Text = funs.EditAccount(textBox26.Text);
                }
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                textBox26.Text = funs.AddAccount();
            }
        }

        private void textBox29_KeyPress(object sender, KeyPressEventArgs e)
        {
            string wheresrt = "(Path LIKE '8;12;%')";
            // strCombo = funs.GetStrCombo(wheresrt);   
            strCombo = funs.GetStrCombonew(wheresrt,"1=1");
            textBox29.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox28_KeyPress(object sender, KeyPressEventArgs e)
        {
            string wheresrt = "(Path LIKE '8;12;%')";
          //  strCombo = funs.GetStrCombo(wheresrt);
            strCombo = funs.GetStrCombonew(wheresrt, "1=1");
            textBox28.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox26_KeyPress(object sender, KeyPressEventArgs e)
        {
            string wheresrt = "(Path LIKE '6;%')";
            //strCombo = funs.GetStrCombo(wheresrt);
            strCombo = funs.GetStrCombonew(wheresrt, "1=1");
            textBox26.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox29_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox29);
        }

        private void textBox28_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox28);
        }

        private void textBox26_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox26);
        }

        private void textBox29_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox29);
        }

        private void textBox28_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox28);
        }

        private void textBox26_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox26);
        }
    }
}
