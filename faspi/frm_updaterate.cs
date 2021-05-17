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
    public partial class frm_updaterate : Form
    {
        String strCombo;
        string gStr;
        DataTable dtCopyRate;

        public frm_updaterate()
        {
            InitializeComponent();
        }

        private void t1_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from other where Type='SER14' order by name";
            t1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [Category_name] as Name from TaxCategory  order by Category_name";
            textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            DataTable dtcombo = new DataTable();

            dtcombo.Columns.Add("PriceList", typeof(string));
            dtcombo.Rows.Add();
            dtcombo.Rows[0][0] = Feature.Available("Name of PriceList1");

            dtcombo.Rows.Add();
            dtcombo.Rows[1][0] = Feature.Available("Name of PriceList2");

            dtcombo.Rows.Add();
            dtcombo.Rows[2][0] = Feature.Available("Name of PriceList3");

            dtcombo.Rows.Add();
            dtcombo.Rows[3][0] = Feature.Available("Name of PriceList4");

            dtcombo.Rows.Add();
            dtcombo.Rows[4][0] = Feature.Available("Name of PriceList5");

            dtcombo.Rows.Add();
            dtcombo.Rows[5][0] = Feature.Available("Name of PriceList6");            

            dtcombo.Rows.Add();
            dtcombo.Rows[6][0] = "MRP";

            textBox2.Text = SelectCombo.ComboDt(this, dtcombo, 0);
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void clear()
        {
            t1.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "0";
        }

        private bool validate()
        {

            if (t1.Text == "" && textBox5.Text == "" && textBox6.Text == "" && textBox7.Text == "" && textBox1.Text == "" && textBox8.Text == "" && textBox9.Text == "" && textBox10.Text == "")
            {
                MessageBox.Show("Select Field");   
                return false;
            }
            if (textBox2.Text == "")
            {
                textBox2.BackColor = Color.Aqua;
                MessageBox.Show(" Select Rate To Update");
                textBox2.Focus();
                return false;
            }
            else if (textBox3.Text == "")
            {
                textBox3.BackColor = Color.Aqua;
                MessageBox.Show(" Select Rate To From");
                textBox3.Focus();
                return false;
            }
            else if (textBox4.Text == "")
            {
                textBox4.BackColor = Color.Aqua;
                MessageBox.Show("Enter Percentage");
                textBox4.Focus();
                return false;
            }

            return true;

        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            DataTable dtcombo = new DataTable();

            dtcombo.Columns.Add("PriceList", typeof(string));
            dtcombo.Rows.Add();
            dtcombo.Rows[0][0] = Feature.Available("Name of PriceList1");

            dtcombo.Rows.Add();
            dtcombo.Rows[1][0] = Feature.Available("Name of PriceList2");

            dtcombo.Rows.Add();
            dtcombo.Rows[2][0] = Feature.Available("Name of PriceList3");

            dtcombo.Rows.Add();
            dtcombo.Rows[3][0] = Feature.Available("Name of PriceList4");

            dtcombo.Rows.Add();
            dtcombo.Rows[4][0] = Feature.Available("Name of PriceList5");

            dtcombo.Rows.Add();
            dtcombo.Rows[5][0] = Feature.Available("Name of PriceList6");

            dtcombo.Rows.Add();
            dtcombo.Rows[6][0] = "MRP";

            textBox3.Text = SelectCombo.ComboDt(this, dtcombo, 0);
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void frm_updaterate_KeyDown(object sender, KeyEventArgs e)
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

        private void t1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(t1);
        }

        private void t1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(t1);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox3);
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            Database.lostFocus(textBox3);
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox4);
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox4);
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void a1_CheckedChanged(object sender, EventArgs e)
        {
            t1.Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
        }

        private void s1_CheckedChanged(object sender, EventArgs e)
        {
            t1.Enabled = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
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
                    //execute();   
                    save();
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
            if (gStr == "0")
            {
                DataTable dtCount = new DataTable();
                Database.GetSqlData("select count(*) from CopyRates where locationid='" + Database.LocationId + "'", dtCount);
                if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                {
                    dtCopyRate.Rows[0]["CR_id"] = Database.LocationId + "1";
                    dtCopyRate.Rows[0]["Nid"] = 1;
                    dtCopyRate.Rows[0]["LocationId"] = Database.LocationId;
                    //dtCopyRate.Rows[0]["user_id"] = Database.user_id;
                    //dtCopyRate.Rows[0]["Modifiedby"] = "";
                }
                else
                {
                    DataTable dtAcid = new DataTable();

                    Database.GetSqlData("select max(Nid) as Nid from CopyRates where locationid='" + Database.LocationId + "'", dtAcid);
                    int Nid = int.Parse(dtAcid.Rows[0][0].ToString());
                    dtCopyRate.Rows[0]["CR_id"] = Database.LocationId + (Nid + 1);
                    dtCopyRate.Rows[0]["Nid"] = (Nid + 1);
                    dtCopyRate.Rows[0]["LocationId"] = Database.LocationId;
                    //dtAcc.Rows[0]["user_id"] = Database.user_id;
                    //dtAcc.Rows[0]["Modifiedby"] = "";
                }
            }

            dtCopyRate.Rows[0]["Company_id"] = funs.Select_oth_id(t1.Text);
            dtCopyRate.Rows[0]["Item_id"] = funs.Select_oth_id(textBox5.Text);
            dtCopyRate.Rows[0]["Color_id"] = funs.Select_oth_id(textBox6.Text);
            dtCopyRate.Rows[0]["Group_id"] = funs.Select_oth_id(textBox7.Text);
            dtCopyRate.Rows[0]["HSN_id"] = funs.Select_tax_cat_id(textBox1.Text);
            dtCopyRate.Rows[0]["description"] = textBox8.Text;
            dtCopyRate.Rows[0]["pack"] = textBox9.Text;
            dtCopyRate.Rows[0]["Pack_category_id"] = funs.Select_packcat_id(textBox10.Text);

            dtCopyRate.Rows[0]["Ratefrom"] = textBox3.Text;
            dtCopyRate.Rows[0]["Rateto"] = textBox2.Text;
            dtCopyRate.Rows[0]["insurance"] = textBox15.Text;
            dtCopyRate.Rows[0]["Rebate"] = textBox16.Text;

            dtCopyRate.Rows[0]["Dis1"] = textBox17.Text;
            dtCopyRate.Rows[0]["Tax"] = textBox4.Text;
            dtCopyRate.Rows[0]["Dis2"] = textBox11.Text;
            dtCopyRate.Rows[0]["Rebate2"] = textBox12.Text;

            dtCopyRate.Rows[0]["Freight"] = textBox13.Text;
            dtCopyRate.Rows[0]["On"] = textBox18.Text;
            dtCopyRate.Rows[0]["Rateunit"] = textBox19.Text;
            dtCopyRate.Rows[0]["Profit"] = textBox14.Text;


            if (radioButton6.Checked == true)
            {
                dtCopyRate.Rows[0]["Rounding"] = "As Actual";
            }
            else if (radioButton5.Checked == true)
            {
                dtCopyRate.Rows[0]["Rounding"] = "Round Down";
            }
            else if (radioButton7.Checked == true)
            {
                dtCopyRate.Rows[0]["Rounding"]= "Round Up";
            }
            else if (radioButton8.Checked == true)
            {
                dtCopyRate.Rows[0]["Rounding"] = "Roundoff";
            }
            else if (radioButton1.Checked == true)
            {
                dtCopyRate.Rows[0]["Rounding"] = "Round Up /10 p";
            }
            else if (radioButton2.Checked == true)
            {
                dtCopyRate.Rows[0]["Rounding"] = "Round Up /5p";
            }
            else if (radioButton3.Checked == true)
            {
                dtCopyRate.Rows[0]["Rounding"] = "Round Up /10 Rs.";
            }
            else if (radioButton4.Checked == true)
            {
                dtCopyRate.Rows[0]["Rounding"] = "Round Up /5 Rs.";
            }





            //textBox13.Text = dtCopyRate.Rows[0]["Freight"].ToString();
            //comboBox2.Text = dtCopyRate.Rows[0]["On"].ToString();
            //comboBox1.Text = dtCopyRate.Rows[0]["Rateunit"].ToString();
            //textBox14.Text = dtCopyRate.Rows[0]["Profit"].ToString();
            Database.SaveData(dtCopyRate);
            if (gStr == "0")
            {
                LoadData("0", "Copy Rate");
            }
            else
            {
                this.Close();
                this.Dispose();
            }
        }


        private void execute()
        {
            string str = "";
            if (t1.Text != "")
            {
                 str = str + "  DESCRIPTION.Company_id='" + funs.Select_oth_id(t1.Text) + "' ";
            }
            if (textBox1.Text != "")
            {
                if (str != "") str += " and ";
                str = str + "DESCRIPTION.Tax_Cat_id= '" + funs.Select_tax_cat_id(textBox1.Text) + "' ";
            }

            if (textBox5.Text != "")
            {
                if (str != "") str += " and ";
                str = str + "DESCRIPTION.Item_id= '" + funs.Select_oth_id(textBox5.Text) + "' ";
            }

            if (textBox6.Text != "")
            {
                if (str != "") str += " and ";
                str = str + "DESCRIPTION.Col_id= '" + funs.Select_oth_id(textBox6.Text) + "' ";
            }
            if (textBox7.Text != "")
            {
                if (str != "") str += " and ";
                str = str + "DESCRIPTION.Group_id= '" + funs.Select_oth_id(textBox7.Text) + "' ";
            }
            if (textBox8.Text != "")
            {
                if (str != "") str += " and ";
                str = str + "DESCRIPTION.Description= '"+textBox8.Text + "' ";
            }
            if (textBox9.Text != "")
            {
                if (str != "") str += " and ";
                str = str + "DESCRIPTION.Pack= '" + textBox9.Text + "' ";
            }
            if (textBox10.Text != "")
            {
                if (str != "") str += " and ";
                str = str + "DESCRIPTION.PackCat_id= '" + funs.Select_packcat_id(textBox10.Text) + "' ";
            }
            String sql = "Select Des_id,Description,Company_id,Item_id,Col_id,Group_id,Tax_Cat_id,Skucode,Shortcode,Open_stock2,Rate_Unit,PAck,Retail,Wholesale,Purchase_rate,MRP,Rate_X,Rate_Y,Rate_Z,Srebate,Weight,Pvalue from description";
            str = " where (" + str + ")";
            DataTable dtdes = new DataTable("Description");


            Database.GetSqlData(sql + str,dtdes);
            string ratetoupdate= funs.Select_Rates_Id(textBox2.Text);
            string rateupdatefrom=funs.Select_Rates_Id(textBox3.Text);

            

            for (int i = 0; i < dtdes.Rows.Count; i++)
            {
                double baseRate = double.Parse(dtdes.Rows[i][rateupdatefrom].ToString());
                double pv = double.Parse(dtdes.Rows[i]["Pvalue"].ToString());
                double wt = double.Parse(dtdes.Rows[i]["Weight"].ToString());


                baseRate = baseRate + double.Parse(textBox15.Text) ;
                baseRate -= double.Parse(textBox16.Text) * pv;
                baseRate -= baseRate * double.Parse(textBox17.Text) / 100;
                baseRate += baseRate * double.Parse(textBox4.Text)/100;
                baseRate -= baseRate * double.Parse(textBox11.Text) / 100;
                baseRate -= double.Parse(textBox12.Text) * pv;


                if (textBox18.Text == "Weight")
                {
                    baseRate += double.Parse(textBox13.Text) * wt;
                }
                else
                {
                    baseRate += double.Parse(textBox13.Text) * pv;
                }
                if (textBox19.Text == "%")
                {
                    baseRate += baseRate * double.Parse(textBox14.Text) / 100;
                }
                else if (textBox19.Text == "/Lt")
                {
                    baseRate +=  double.Parse(textBox14.Text) * pv ;
                }
                else
                {
                    baseRate += double.Parse(textBox14.Text);
                }

                if (radioButton6.Checked == true)
                {
                    dtdes.Rows[i][ratetoupdate] = baseRate;
                }
                else if(radioButton5.Checked==true)
                {
                    dtdes.Rows[i][ratetoupdate] = Math.Floor(baseRate);
                }
                else if (radioButton7.Checked == true)
                {
                    dtdes.Rows[i][ratetoupdate] = Math.Ceiling(baseRate);
                }
                else if(radioButton8.Checked==true)
                {
                    dtdes.Rows[i][ratetoupdate] = funs.Roundoff(baseRate.ToString());
                }
                else if (radioButton1.Checked == true)
                {
                    baseRate = baseRate * 10;
                    baseRate = Math.Ceiling(baseRate);
                    baseRate = baseRate / 10;
                    dtdes.Rows[i][ratetoupdate] = baseRate;
                }
                else if (radioButton2.Checked == true)
                {
                    dtdes.Rows[i][ratetoupdate] = (Math.Ceiling(baseRate / 0.05d) * 0.05);
                   
                }
                else if (radioButton3.Checked == true)
                {
                   
                    dtdes.Rows[i][ratetoupdate] = (int)(Math.Ceiling(baseRate / 10.0d) * 10);
                }
                else if (radioButton4.Checked == true)
                {
                   
                    dtdes.Rows[i][ratetoupdate] = (int)(Math.Ceiling(baseRate / 5.0d) * 5);

                }
            }
            Database.SaveData(dtdes,sql);

            //string textbox2 = funs.Select_Rates_Id(textBox2.Text);
            //string textbox3 = funs.Select_Rates_Id(textBox3.Text);
            //string sql3 = "";
            //double per = 0;
            //per = double.Parse(textBox4.Text);



            //if (radioButton6.Checked == true)
            //{
            //    sql3 = "UPDATE DESCRIPTION SET " + textbox2 + " = " + textbox3 + "+ (" + textbox3 + " *" + textBox4.Text + "/100)" + str;
            //}

            //else if (radioButton5.Checked == true)
            //{
            //    if (Database.DatabaseType == "access")
            //    {
            //        sql3 = "UPDATE DESCRIPTION SET " + textbox2 + " = Int(" + textbox3 + " " + PlusMinus + "(" + textbox3 + " *" + textBox4.Text + "/100))" + str;
            //    }
            //    else
            //    {
            //        sql3 = "UPDATE DESCRIPTION SET " + textbox2 + " = Floor(" + textbox3 + " " + PlusMinus + "(" + textbox3 + " *" + textBox4.Text + "/100))" + str;
            //    }
            //}

            //else if (radioButton7.Checked == true)
            //{
            //    if (Database.DatabaseType == "access")
            //    {
            //        sql3 = "UPDATE DESCRIPTION SET " + textbox2 + " = Int(" + textbox3 + " " + PlusMinus + "(" + textbox3 + "*" + textBox4.Text + "/100)) + iif(" + textbox3 + "+(" + textbox3 + "*" + textBox4.Text + "/100) - Int(" + textbox3 + "+(" + textbox3 + "*" + textBox4.Text + "/100))>0,1,0)" + str;
            //    }
            //    else
            //    {
            //        sql3 = "UPDATE DESCRIPTION SET " + textbox2 + " = CEILING(" + textbox3 + " " + PlusMinus + "(" + textBox3.Text + " *" + textBox4.Text + "/100))" + str;
            //    }
            //}

            //else if (radioButton8.Checked == true)
            //{
            //    if (Database.DatabaseType == "access")
            //    {
            //        sql3 = "UPDATE DESCRIPTION SET " + textbox2 + " = Round(" + textbox3 + " " + PlusMinus + "(" + textbox3 + " *" + textBox4.Text + "/100))" + str;
            //    }
            //    else
            //    {
            //        sql3 = "UPDATE DESCRIPTION SET " + textbox2 + " = Round(" + textbox3 + " " + PlusMinus + "(" + textbox3 + " *" + textBox4.Text + "/100),0)" + str;
            //    }
            //}
            //Database.CommandExecutor(sql3);


            Master.UpdateDecription();
            Master.UpdateDecriptionInfo();
            MessageBox.Show("Modify Rate Successfully");
            


            textBox4.Text  = "0";
            textBox11.Text = "0";
            textBox12.Text = "0";
            textBox13.Text = "0";
            textBox14.Text = "0";
            textBox15.Text = "0";
            textBox16.Text = "0";
            textBox17.Text = "0"; 
            t1.Select();   
        }


        public void LoadData(string str,String frmcaption)
        {
            gStr = str;
            dtCopyRate = new DataTable("CopyRates");
            Database.GetSqlData("Select * from CopyRates where Cr_id='"+str+"'",dtCopyRate);
            if (dtCopyRate.Rows.Count > 0)
            {
                t1.Text = funs.Select_oth_nm(dtCopyRate.Rows[0]["Company_id"].ToString());
                textBox5.Text = funs.Select_oth_nm(dtCopyRate.Rows[0]["Item_id"].ToString());
                textBox6.Text = funs.Select_oth_nm(dtCopyRate.Rows[0]["Color_id"].ToString());
                textBox7.Text = funs.Select_oth_nm(dtCopyRate.Rows[0]["Group_id"].ToString());
                textBox1.Text = funs.Select_tax_cat_nm(dtCopyRate.Rows[0]["HSN_id"].ToString());
                textBox8.Text = dtCopyRate.Rows[0]["description"].ToString();
                textBox9.Text = dtCopyRate.Rows[0]["pack"].ToString();
                textBox10.Text = funs.Select_packcat_name(dtCopyRate.Rows[0]["Pack_category_id"].ToString());
                textBox3.Text = dtCopyRate.Rows[0]["Ratefrom"].ToString();
                textBox2.Text = dtCopyRate.Rows[0]["Rateto"].ToString();
                textBox15.Text = dtCopyRate.Rows[0]["insurance"].ToString();
                textBox16.Text = dtCopyRate.Rows[0]["Rebate"].ToString();
                textBox17.Text = dtCopyRate.Rows[0]["Dis1"].ToString();
                textBox4.Text = dtCopyRate.Rows[0]["Tax"].ToString();
                textBox11.Text = dtCopyRate.Rows[0]["Dis2"].ToString();
                textBox12.Text = dtCopyRate.Rows[0]["Rebate2"].ToString();
                textBox13.Text = dtCopyRate.Rows[0]["Freight"].ToString();
                textBox18.Text = dtCopyRate.Rows[0]["On"].ToString();
                textBox19.Text = dtCopyRate.Rows[0]["Rateunit"].ToString();
                textBox14.Text = dtCopyRate.Rows[0]["Profit"].ToString();
                if (dtCopyRate.Rows[0]["Rounding"].ToString() == "As Actual")
                {
                    radioButton6.Checked = true;
                }
                else if (dtCopyRate.Rows[0]["Rounding"].ToString() == "Round Down")
                {
                    radioButton5.Checked = true;
                }
                else if (dtCopyRate.Rows[0]["Rounding"].ToString() == "Round Up")
                {
                    radioButton7.Checked = true;
                }
                else if (dtCopyRate.Rows[0]["Rounding"].ToString() == "Roundoff")
                {
                    radioButton8.Checked = true;
                }
                else if (dtCopyRate.Rows[0]["Rounding"].ToString() == "Round Up /10 p")
                {
                    radioButton1.Checked = true;
                }
                else if (dtCopyRate.Rows[0]["Rounding"].ToString() == "Round Up /5p")
                {
                    radioButton2.Checked = true;
                }
                else if (dtCopyRate.Rows[0]["Rounding"].ToString() == "Round Up /10 Rs.")
                {
                    radioButton3.Checked = true;
                }
                else if (dtCopyRate.Rows[0]["Rounding"].ToString() == "Round Up /5 Rs.")
                {
                    radioButton4.Checked = true;
                }
            }
            else
            {
                dtCopyRate.Rows.Add();
                textBox4.Text = "0";
                textBox11.Text = "0";
                textBox12.Text = "0";
                textBox13.Text = "0";
                textBox14.Text = "0";
                textBox15.Text = "0";
                textBox16.Text = "0";
                textBox17.Text = "0";
                textBox19.Text = "%";
                textBox18.Text = "Weight";
                t1.Select();
                radioButton1.Checked = true;
            }
            this.Text = frmcaption;
        }



        private void frm_updaterate_Load(object sender, EventArgs e)
        {
            
           // this.Size = this.MdiParent.Size;
            SideFill();
            t1.Select();
            



            if (Feature.Available("Taxation Applicable") == "VAT")
            {
                label6.Text = "TaxCategory";
            }
            else
            {
                label6.Text = "HSN";
            }
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from other where Type='" + funs.Get_Item_id() + "' order by [name]";
            textBox5.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from other where Type='" + funs.Get_Colour_id() + "' order by [name]";
            textBox6.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from other where Type='" + funs.Get_Group_id() + "' order by [name]";
            textBox7.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select distinct [Description] from Description order by description";
            textBox8.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
           // strCombo = "SELECT DISTINCT  Pack, CAST(Pvalue AS nvarchar) AS Pvalue  FROM  Description ORDER BY Pvalue";
            strCombo = "SELECT DISTINCT  Pack  FROM  Description ORDER BY Pack";
            textBox9.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "Select name from PackCategory order by Name";
            textBox10.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox5_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox5);
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox5);
        }

        private void textBox6_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox6);
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox6);
        }

        private void textBox7_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox7);
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox7);
        }

        private void textBox8_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox8);
        }

        private void textBox8_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox8);
        }

        private void textBox9_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox9);
        }

        private void textBox9_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox9);
        }

        private void textBox10_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox10);
        }

        private void textBox10_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox10);
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox11_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox12_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox13_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox14_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void comboBox1_Enter(object sender, EventArgs e)
        {
           // SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox11_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox11);
        }

        private void textBox12_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox12);
        }

        private void textBox13_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox13);
        }

        private void textBox14_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox14);
        }

        private void textBox11_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox11);
        }

        private void textBox12_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox12);
        }

        private void textBox13_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox13);
        }

        private void textBox14_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox14);
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
           // Database.setFocus(comboBox1);
        }

        private void textBox15_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox15);
        }

        private void textBox15_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox15);
        }

        private void textBox15_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox16_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox17_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox17_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox17);
        }

        private void textBox16_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox16);
        }

        private void textBox16_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox16);
        }

        private void textBox17_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox17);
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox18_KeyDown(object sender, KeyEventArgs e)
        {
            DataTable dtcombo = new DataTable();
            dtcombo.Columns.Add("Value", typeof(string));
            dtcombo.Rows.Add();
            dtcombo.Rows[0][0] = "Weight";
            dtcombo.Rows.Add();
            dtcombo.Rows[1][0] = "Pvalue";
            textBox18.Text = SelectCombo.ComboDt(this, dtcombo, 0);
            SendKeys.Send("{tab}");
        }

        private void textBox19_KeyDown(object sender, KeyEventArgs e)
        {
            DataTable dtcombo = new DataTable();
            dtcombo.Columns.Add("Value", typeof(string));
            dtcombo.Rows.Add();
            dtcombo.Rows[0][0] = "%";
            dtcombo.Rows.Add();
            dtcombo.Rows[1][0] = "/Lt";
            dtcombo.Rows.Add();
            dtcombo.Rows[2][0] = "Pcs";
            textBox19.Text = SelectCombo.ComboDt(this, dtcombo, 0);
            SendKeys.Send("{tab}");
        }

        private void textBox18_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox18);
        }

        private void textBox18_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox18);
        }

        private void textBox19_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox19);
        }

        private void textBox19_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox19);
        }
    }
}
