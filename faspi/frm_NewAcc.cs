using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;



namespace faspi
{
    public partial class frm_NewAcc : Form
    {
        DataTable dtAcc;
        String dtName;
        string act_name = "";
        DataTable dtBilladj;
        public bool calledIndirect = false;
        public String AccName;
        public String AccType;
        String strCombo;
        public string gStr = "";
        public bool gresave = false;
        List<UsersFeature> permission;

        public frm_NewAcc()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Load1();
            SideFill();
            if (Feature.Available("Taxation Applicable") == "VAT")
            {
                groupBox5.Text = "TIN";
            }
            else
            {
                groupBox5.Text = "GSTIN";
            }
            groupBox10.Text = Feature.Available("Show Text on AadhaarNo");

            if (gStr != "0" && Database.utype.ToUpper() != "SUPERADMIN")
            {
                groupBox2.Enabled = false;
                groupBox6.Enabled = false;
            }
        }

        private void Load1()
        {
            if (Feature.Available("Customer Credit Limits") == "No")
            {
                textBox3.Enabled = false;
                if (act_name != "STOCK-IN-HAND")
                {
                    textBox4.Enabled = false;
                }
            }




            if (Feature.Available("Customer Credit Limits") == "No" && act_name == "Stock")
            {
                textBox4.Enabled = true;
            }


            if (Feature.Available("Group Credit Limits") == "No")
            {
                textBox11.Enabled = false;
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
            permission = funs.GetPermissionKey("Account");
            //create
            UsersFeature ob = permission.Where(w => w.FeatureName == "Create").FirstOrDefault();
            if (ob != null && gStr == "0" && ob.SelectedValue == "Allowed")
            {
                dtsidefill.Rows[0]["Visible"] = true;
            }
            else if(gStr == "0")
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




            //close
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "gstindetails";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "GSTIN Details";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
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
            string name = "";
            if (gresave == false)
            {
                Button tbtn = (Button)sender;
                name = tbtn.Name.ToString();
            }
            else
            {
                name = "save";
            }




            if (name == "save"  && gresave==false)
            {
                if (validate() == true)
                {

                    permission = funs.GetPermissionKey("Account");
                    //create
                   
                    UsersFeature ob = permission.Where(w => w.FeatureName == "Create").FirstOrDefault();
                    if (ob != null && gStr == "0" && ob.SelectedValue == "Allowed")
                    {

                        try
                        {
                            Database.BeginTran();
                            save();
                            Database.CommitTran();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Not Saved Due to an Exception");
                            Database.RollbackTran();
                        }
                    }

                    //alter
                    ob = permission.Where(w => w.FeatureName == "Alter").FirstOrDefault();
                    if (ob != null && gStr != "0" && ob.SelectedValue == "Allowed")
                    {
                        try
                        {
                            Database.BeginTran();
                            save();
                            Database.CommitTran();
                        }
                        catch (Exception ex)
                        {
                            Database.RollbackTran();
                        }
                    }
                   
                }
            }
            else if (name == "save" && gresave==true)
            {
                  permission = funs.GetPermissionKey("Account");
                    //create

                  UsersFeature ob = permission.Where(w => w.FeatureName == "Alter").FirstOrDefault();
                   

                    //alter
                    
                    if (ob != null && gStr != "0" && ob.SelectedValue == "Allowed")
                    {
                        try
                        {
                            Database.BeginTran();
                            string ac_id = dtAcc.Rows[0]["ac_id"].ToString();

                            DataTable dttemp = new DataTable("Billadjest");
                            Database.GetSqlData("Select * from BillAdjest where Ac_id='" + ac_id + "' and Vi_id='0' and Reff_id='0'", dttemp);
                            for (int i = 0; i < dttemp.Rows.Count; i++)
                            {
                                dttemp.Rows[i].Delete();
                            }
                            Database.SaveData(dttemp);
                            dtBilladj = new DataTable("Billadjest");
                            Database.GetSqlData("Select * from BillAdjest where Ac_id='" + ac_id + "' and Vi_id='0' and Reff_id='0'", dtBilladj);
                            if (double.Parse(textBox2.Text) != 0)
                            {
                                dtBilladj.Rows.Add();
                                DataTable dtCount = new DataTable();
                                Database.GetSqlData("select count(*) from BillAdjest where locationid='" + Database.LocationId + "'", dtCount);
                                if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                                {
                                    dtBilladj.Rows[dtBilladj.Rows.Count - 1]["id"] = Database.LocationId + "1";
                                    dtBilladj.Rows[dtBilladj.Rows.Count - 1]["Nid"] = 1;
                                    dtBilladj.Rows[dtBilladj.Rows.Count - 1]["LocationId"] = Database.LocationId;

                                }
                                else
                                {
                                    DataTable dtAcid = new DataTable();
                                    Database.GetSqlData("select max(Nid) as Nid from BillAdjest where locationid='" + Database.LocationId + "'", dtAcid);
                                    int Nid = int.Parse(dtAcid.Rows[0][0].ToString());
                                    dtBilladj.Rows[dtBilladj.Rows.Count - 1]["id"] = Database.LocationId + (Nid + 1);
                                    dtBilladj.Rows[dtBilladj.Rows.Count - 1]["Nid"] = (Nid + 1);
                                    dtBilladj.Rows[dtBilladj.Rows.Count - 1]["LocationId"] = Database.LocationId;

                                }


                                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["Ac_id"] = ac_id;
                                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["vi_id"] = "0";
                                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["Reff_id"] = "0";
                                double blnc = 0;
                                if (radioButton1.Checked == true)
                                {
                                    blnc = double.Parse(textBox2.Text);
                                }
                                else
                                {
                                    blnc = -1 * double.Parse(textBox2.Text);
                                }

                              

                                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["Amount"] = blnc;
                                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["itemsr"] = 1;
                                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["AdjustSr"] = 1;
                                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["A"] = true;
                                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["B"] = false;
                                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["AB"] = true;

                            }

                            if (double.Parse(textBox14.Text) != 0)
                            {
                                if (dtBilladj.Rows.Count == 0)
                                {
                                    dtBilladj.Rows.Add();
                                    DataTable dtCount = new DataTable();
                                    Database.GetSqlData("select count(*) from BillAdjest where locationid='" + Database.LocationId + "'", dtCount);
                                    if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                                    {
                                        dtBilladj.Rows[dtBilladj.Rows.Count - 1]["id"] = Database.LocationId + "1";
                                        dtBilladj.Rows[dtBilladj.Rows.Count - 1]["Nid"] = 1;
                                        dtBilladj.Rows[dtBilladj.Rows.Count - 1]["LocationId"] = Database.LocationId;

                                    }
                                    else
                                    {
                                        DataTable dtAcid = new DataTable();
                                        Database.GetSqlData("select max(Nid) as Nid from BillAdjest where locationid='" + Database.LocationId + "'", dtAcid);
                                        int Nid = int.Parse(dtAcid.Rows[0][0].ToString());
                                        dtBilladj.Rows[dtBilladj.Rows.Count - 1]["id"] = Database.LocationId + (Nid + 1);
                                        dtBilladj.Rows[dtBilladj.Rows.Count - 1]["Nid"] = (Nid + 1);
                                        dtBilladj.Rows[dtBilladj.Rows.Count - 1]["LocationId"] = Database.LocationId;

                                    }
                                }
                                else
                                {
                                    dtBilladj.Rows.Add();

                                    int Nid = int.Parse(dtBilladj.Rows[0]["Nid"].ToString());
                                    dtBilladj.Rows[dtBilladj.Rows.Count - 1]["id"] = Database.LocationId + (Nid + 1);
                                    dtBilladj.Rows[dtBilladj.Rows.Count - 1]["Nid"] = (Nid + 1);
                                    dtBilladj.Rows[dtBilladj.Rows.Count - 1]["LocationId"] = Database.LocationId;


                                }



                                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["Ac_id"] = ac_id;
                                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["vi_id"] = "0";
                                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["Reff_id"] = "0";
                                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["itemsr"] = 1;
                                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["AdjustSr"] = 1;
                                double blnc2 = 0;
                                if (radioButton3.Checked == true)
                                {
                                    blnc2 = double.Parse(textBox14.Text);
                                }
                                else
                                {
                                    blnc2 = -1 * double.Parse(textBox14.Text);
                                }

                              
                                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["Amount"] = blnc2;
                                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["A"] = false;
                                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["B"] = true;
                                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["AB"] = true;

                            }
                            Database.SaveData(dtBilladj);
                            Database.CommitTran();
                        }
                        catch (Exception ex)
                        {
                            Database.RollbackTran();
                            MessageBox.Show("Account Name " + TextBox1.Text + " Not saved " + ex.ToString());
                        }
                    }
                   
                }


            else if (name == "gstindetails")
            {

                frm_gstindet frm = new frm_gstindet();
                frm.ShowDialog();

                if (frm.obj == null)
                {
                    return;
                }

                TextBox1.Text = frm.obj.tradenam;
                textBox18.Text = frm.obj.lgnm; ;
                textBox9.Text = frm.obj.gstin;
                textBox31.Text = frm.obj.pradr.addr.pncd;


                textBox19.Text = frm.obj.pradr.addr.stcd;
                textBox17.Text = frm.obj.gstin.Substring(2, 10);
                TextBox5.Text = frm.obj.pradr.addr.bno + " " + frm.obj.pradr.addr.flno + " " + frm.obj.pradr.addr.st;
                if (frm.obj.pradr.addr.dst == "")
                {
                    TextBox6.Text = frm.obj.pradr.addr.loc + " " + frm.obj.pradr.addr.city + " " + frm.obj.stj.Substring(0, frm.obj.stj.IndexOf(' '));
                }
                else
                {
                    TextBox6.Text = frm.obj.pradr.addr.loc + " " + frm.obj.pradr.addr.city + " " + frm.obj.pradr.addr.dst;
                }
                if (frm.obj.dty == "Regular")
                {

                    textBox21.Text = "Regular Registration";
                }
                else
                {
                    textBox21.Text = "Composition Dealer";
                }


                textBox10.Focus();

                TextBox5.Enabled = true;
                TextBox6.Enabled = true;
                textBox21.Enabled = true;
                textBox17.Enabled = true;



            }


            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }





     
        private void frm_NewAcc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                if (validate() == true)
                {
                //    if (Database.utype.ToUpper() == "SUPERADMIN" || Database.utype.ToUpper() == "ADMIN")
                //    {
                //        save();
                //    }

                //    else if (gStr == "0")
                //    {
                //        save();
                //    }
                    //create
                    UsersFeature ob = permission.Where(w => w.FeatureName == "Create").FirstOrDefault();
                    if (ob != null && gStr == "0" && ob.SelectedValue == "Allowed")
                    {
                        try
                        {
                            Database.BeginTran();
                            save();
                            Database.CommitTran();
                        }
                        catch (Exception ex)
                        {
                            Database.RollbackTran();
                        }
                    }
                  
                    //alter
                    ob = permission.Where(w => w.FeatureName == "Alter").FirstOrDefault();
                    if (ob != null && gStr != "0" && ob.SelectedValue == "Allowed")
                    {
                        try
                        {
                            Database.BeginTran();
                            save();
                            Database.CommitTran();
                        }
                        catch (Exception ex)
                        {
                            Database.RollbackTran();
                        }
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


        public void LoadData(String str, String frmCaption)
        {
            gStr = str;
            this.Text = frmCaption;

            if (AccType == "Bank")
            {
                textBox10.Text = AccType;
                textBox10.Enabled = false;
            }
            else if (AccType == "Cash")
            {
                textBox10.Text = AccType;
                textBox10.Enabled = false;
            }

            dtName = "account";
            dtAcc = new DataTable(dtName);
            Database.GetSqlData("select * from " + dtName + " where ac_id='" + str + "'", dtAcc);


            if (AccType == null && dtAcc.Rows.Count == 0)
            {
                dtAcc.Rows.Add(0);
                TextBox1.Select();
                TextBox1.Text = "";
                textBox2.Text = "0";
                textBox3.Text = "0";
                textBox4.Text = "0";
                TextBox5.Text = "";
                TextBox6.Text = "";
                TextBox7.Text = "";
                TextBox8.Text = "";
                textBox9.Text = "";
                textBox13.Text = "";
                textBox17.Text = "";
                textBox18.Text = "";
                textBox28.Text = "";
                textBox20.Text = "";
                textBox22.Text = "";
                textBox25.Text = "";
                textBox27.Text = "";
                textBox23.Text = "";
                textBox24.Text = "";
                textBox21.Text = "Unregistered";
                textBox26.Text = "";
                string state_id = Database.GetScalarText("Select CState_id from Company");
                textBox19.Text = funs.Select_state_nm(state_id);
                textBox14.Text = "0";
                textBox16.Text = "0";
                textBox11.Text = "";
                textBox31.Text = "0";
                textBox12.Text = "";
                textBox29.Text = "0";
                textBox30.Text = "";
            }
            else if (AccType != null && dtAcc.Rows.Count == 0)
            {
                dtAcc.Rows.Add(0);
                TextBox1.Select();
                TextBox1.Text = "";
                textBox2.Text = "0";
                textBox3.Text = "0";
                textBox4.Text = "0";
                TextBox5.Text = "";
                TextBox6.Text = "";
                TextBox7.Text = "";
                TextBox8.Text = "";
                textBox9.Text = "";
                textBox12.Text = "";
                textBox13.Text = "";
                textBox17.Text = "";
                textBox18.Text = "";
                textBox28.Text = "";
                textBox20.Text = "";
                textBox23.Text = "";
                textBox22.Text = "";
                textBox31.Text = "0";
                textBox21.Text = "Unregistered";
                textBox26.Text = "";
                string state_id = Database.GetScalarText("Select CState_id from Company");
                textBox19.Text = funs.Select_state_nm(state_id);
                textBox11.Text = "";
                textBox14.Text = "0";
                textBox29.Text = "0";
                textBox30.Text = "";
                textBox16.Text = "0";
                textBox25.Text = "";
                textBox27.Text = "";
            }

            else
            {
                TextBox1.Select();
                TextBox1.Text = dtAcc.Rows[0]["name"].ToString();
                textBox10.Text = funs.Select_Refineact_nm(dtAcc.Rows[0]["act_id"].ToString());
                if (double.Parse(dtAcc.Rows[0]["Balance"].ToString()) >= 0)
                {
                    textBox2.Text = funs.DecimalPoint(double.Parse(dtAcc.Rows[0]["Balance"].ToString()), 2);
                    radioButton1.Checked = true;
                }
                else
                {
                    textBox2.Text = funs.DecimalPoint(-1 * double.Parse(dtAcc.Rows[0]["Balance"].ToString()), 2);
                    radioButton2.Checked = true;
                }
                if (dtAcc.Rows[0]["Balance2"].ToString() == "")
                {
                    dtAcc.Rows[0]["Balance2"] = 0;
                }
                if (double.Parse(dtAcc.Rows[0]["Balance2"].ToString()) >= 0)
                {
                    textBox14.Text = funs.DecimalPoint(double.Parse(dtAcc.Rows[0]["Balance2"].ToString()), 2);
                    radioButton3.Checked = true;
                }
                else
                {
                    textBox14.Text = funs.DecimalPoint(-1 * double.Parse(dtAcc.Rows[0]["Balance2"].ToString()), 2);
                    radioButton4.Checked = true;
                }

                textBox3.Text = funs.DecimalPoint(dtAcc.Rows[0]["Blimit"]);
                TextBox5.Text = dtAcc.Rows[0]["address1"].ToString();
                TextBox6.Text = dtAcc.Rows[0]["address2"].ToString();
                TextBox7.Text = dtAcc.Rows[0]["phone"].ToString();
                textBox31.Text = dtAcc.Rows[0]["Pincode"].ToString();
                TextBox8.Text = dtAcc.Rows[0]["email"].ToString();
                textBox9.Text = dtAcc.Rows[0]["Tin_number"].ToString();
                textBox17.Text = dtAcc.Rows[0]["PAN"].ToString();
                textBox20.Text = dtAcc.Rows[0]["Aadhaarno"].ToString();
                textBox21.Text = dtAcc.Rows[0]["RegStatus"].ToString();
                textBox23.Text = dtAcc.Rows[0]["MobileNo"].ToString();
                textBox22.Text = funs.Select_Rates_Value(dtAcc.Rows[0]["RateApp"].ToString());
                textBox25.Text = funs.Select_Rates_Value(dtAcc.Rows[0]["RateApp2"].ToString());
                textBox11.Text = funs.Select_oth_nm(dtAcc.Rows[0]["loc_id"].ToString());
                textBox28.Text = funs.Select_city_name(dtAcc.Rows[0]["city_id"].ToString());
                textBox24.Text = funs.Select_salesman_nm(dtAcc.Rows[0]["salesman_id"].ToString());
                if (dtAcc.Rows[0]["State_id"].ToString() == "")
                {
                    dtAcc.Rows[0]["State_id"] = 0;
                }
                textBox19.Text = funs.Select_state_nm(dtAcc.Rows[0]["State_id"].ToString());

                if (dtAcc.Rows[0]["con_id"].ToString() == "")
                {
                    dtAcc.Rows[0]["con_id"] = 0;
                }
                textBox12.Text = funs.Select_ac_nm(dtAcc.Rows[0]["con_id"].ToString());
                textBox13.Text = dtAcc.Rows[0]["Note"].ToString();
                textBox26.Text = dtAcc.Rows[0]["Grade"].ToString();
                textBox16.Text = funs.DecimalPoint(dtAcc.Rows[0]["Closing_Bal"], 2);
                textBox18.Text = dtAcc.Rows[0]["Printname"].ToString();


                textBox29.Text = funs.DecimalPoint(double.Parse(dtAcc.Rows[0]["Distance"].ToString()), 2);
                textBox30.Text = funs.Select_ac_nm(dtAcc.Rows[0]["Transporter_id"].ToString());

                textBox27.Text = dtAcc.Rows[0]["Code"].ToString();
                if (bool.Parse(dtAcc.Rows[0]["Status"].ToString()) == true)
                {
                    radioButton6.Checked = true;
                }
                else
                {
                    radioButton5.Checked = true;
                }
                act_name = Database.GetScalarText("Select Name from accountype where Name='" + textBox10.Text + "'");
                if (act_name == "Stock")
                {
                    textBox4.Text = funs.DecimalPoint(dtAcc.Rows[0]["Closing_Bal2"], 2);
                }
                else
                {
                    textBox4.Text = dtAcc.Rows[0]["Dlimit"].ToString();
                }
                
                if (dtAcc.Rows[0]["AllowPS"].ToString() == "")
                {

                }
                else if (bool.Parse(dtAcc.Rows[0]["AllowPS"].ToString()) == true)
                {
                    checkBox1.Checked = true;
                }
                else
                {
                    checkBox1.Checked = false;
                }

                if (Database.utype.ToUpper() == "SUPERADMIN")
                {
                    textBox22.Enabled = true;
                }
                else
                {
                    textBox22.Enabled = false;
                }
            }

            act_name = Database.GetScalarText("Select Name from accountype where Name='" + textBox10.Text + "'");
            Displaysetting(act_name);

            if (Feature.Available("Group Credit Limits") == "No")
            {
                textBox11.Enabled = false;
            }

            if (Feature.Available("Broker Wise Report") == "No")
            {
                textBox12.Enabled = false;
            }

            if (Database.IsKacha == true)
            {
                groupBox6.Visible = true;
            }
            else
            {
                groupBox6.Visible = false;
            }
            if (gresave == true)
            {
                object sender = new object();
                EventArgs e = new EventArgs();
                btn_Click(sender, e);
            }
        }

        private void save()
        {
            AccName = TextBox1.Text;

            if (gStr == "0")
            {
                DataTable dtCount = new DataTable();
                Database.GetSqlData("select count(*) from account where locationid='" + Database.LocationId + "'", dtCount);
                if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                {
                    dtAcc.Rows[0]["ac_id"] = Database.LocationId + "1";
                    dtAcc.Rows[0]["Nid"] = 1;
                    dtAcc.Rows[0]["LocationId"] = Database.LocationId;
                    dtAcc.Rows[0]["user_id"] = Database.user_id;
                    dtAcc.Rows[0]["Modifiedby"] = "";
                }
                else
                {
                    DataTable dtAcid = new DataTable();

                    Database.GetSqlData("select max(Nid) as Nid from account where locationid='" + Database.LocationId + "'", dtAcid);
                    int Nid = int.Parse(dtAcid.Rows[0][0].ToString());
                    dtAcc.Rows[0]["ac_id"] = Database.LocationId + (Nid + 1);
                    dtAcc.Rows[0]["Nid"] = (Nid + 1);
                    dtAcc.Rows[0]["LocationId"] = Database.LocationId;
                    dtAcc.Rows[0]["user_id"] = Database.user_id;
                    dtAcc.Rows[0]["Modifiedby"] = "";
                }
            }
            else
            {
                dtAcc.Rows[0]["Modifiedby"] = Database.user_id;
            }

            dtAcc.Rows[0]["act_id"] = funs.Select_Refineact_id(textBox10.Text);
            dtAcc.Rows[0]["name"] = TextBox1.Text;
            dtAcc.Rows[0]["Address1"] = TextBox5.Text;
            dtAcc.Rows[0]["Address2"] = TextBox6.Text;
            dtAcc.Rows[0]["phone"] = TextBox7.Text;
            dtAcc.Rows[0]["email"] = TextBox8.Text;
            dtAcc.Rows[0]["tin_number"] = textBox9.Text;
            dtAcc.Rows[0]["MobileNo"] = textBox23.Text;
            dtAcc.Rows[0]["Pincode"] = textBox31.Text;
            dtAcc.Rows[0]["PAN"] = textBox17.Text;
            dtAcc.Rows[0]["con_id"] = funs.Select_ac_id(textBox12.Text);
            dtAcc.Rows[0]["State_id"] = funs.Select_state_id(textBox19.Text);
            dtAcc.Rows[0]["City_id"] = funs.Select_city_id(textBox28.Text);


            dtAcc.Rows[0]["Distance"] = double.Parse(textBox29.Text);
            dtAcc.Rows[0]["Transporter_id"] = funs.Select_ac_id(textBox30.Text);
            dtAcc.Rows[0]["Salesman_id"] = funs.Select_salesman_id(textBox24.Text);
            dtAcc.Rows[0]["Branch_id"] = Database.BranchId;
            dtAcc.Rows[0]["Aadhaarno"] = textBox20.Text;
            if (textBox11.Text != "")
            {
                dtAcc.Rows[0]["loc_id"] = funs.Select_oth_id(textBox11.Text);
            }
            else
            {
                dtAcc.Rows[0]["loc_id"] = "";
            }
            double blnc = 0;
            if (radioButton1.Checked == true)
            {
                blnc = double.Parse(textBox2.Text);
            }
            else
            {
                blnc = -1 * double.Parse(textBox2.Text);
            }

            dtAcc.Rows[0]["Balance"] = blnc;

            if (radioButton6.Checked == true)
            {
                dtAcc.Rows[0]["Status"] = true;
            }
            else
            {
                dtAcc.Rows[0]["Status"] = false;
            }

            double blnc2 = 0;
            if (radioButton3.Checked == true)
            {
                blnc2 = double.Parse(textBox14.Text);
            }
            else
            {
                blnc2 = -1 * double.Parse(textBox14.Text);
            }

            dtAcc.Rows[0]["Balance2"] = blnc2;

            if (textBox3.Text != "")
            {
                dtAcc.Rows[0]["Blimit"] = textBox3.Text;
            }
            else
            {
                dtAcc.Rows[0]["Blimit"] = "0.00";
            }

            dtAcc.Rows[0]["Closing_Bal2"] = textBox4.Text;
            dtAcc.Rows[0]["Closing_Bal"] = textBox16.Text;
            if (textBox4.Text == "")
            {
                textBox4.Text = "0";
            }
            dtAcc.Rows[0]["Dlimit"] = double.Parse(textBox4.Text);
            dtAcc.Rows[0]["RegStatus"] = textBox21.Text;
            dtAcc.Rows[0]["Grade"] = textBox26.Text;
            dtAcc.Rows[0]["note"] = textBox13.Text;
            dtAcc.Rows[0]["Printname"] = textBox18.Text;
            dtAcc.Rows[0]["Closing_Bal"] = textBox16.Text;
            if (textBox27.Text.Trim() == "")
            {
                dtAcc.Rows[0]["Code"] = dtAcc.Rows[0]["ac_id"].ToString(); 
            }
            else
            {
                dtAcc.Rows[0]["Code"] = textBox27.Text;
            }
                dtAcc.Rows[0]["RateApp"] = funs.Select_Rates_Id(textBox22.Text);
                dtAcc.Rows[0]["RateApp2"] = funs.Select_Rates_Id(textBox25.Text);
            if (checkBox1.Checked == true)
            {
                dtAcc.Rows[0]["AllowPS"] = true;
            }
            else
            {
                dtAcc.Rows[0]["AllowPS"] = false;
            }
            string ac_id = dtAcc.Rows[0]["ac_id"].ToString();
              
            DataTable dttemp = new DataTable("Billadjest");
            Database.GetSqlData("Select * from BillAdjest where Ac_id='" + ac_id + "' and Vi_id='0' and Reff_id='0'", dttemp);
            for (int i = 0; i < dttemp.Rows.Count; i++)
            {
                dttemp.Rows[i].Delete();
            }
            Database.SaveData(dttemp);
            dtBilladj = new DataTable("Billadjest");
            Database.GetSqlData("Select * from BillAdjest where Ac_id='" + ac_id + "' and Vi_id='0' and Reff_id='0'", dtBilladj);
            if (double.Parse(textBox2.Text) != 0)
            {
                dtBilladj.Rows.Add();  
                DataTable dtCount = new DataTable();
                Database.GetSqlData("select count(*) from BillAdjest where locationid='" + Database.LocationId + "'", dtCount);
                if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                {
                    dtBilladj.Rows[dtBilladj.Rows.Count - 1]["id"] = Database.LocationId + "1";
                    dtBilladj.Rows[dtBilladj.Rows.Count - 1]["Nid"] = 1;
                    dtBilladj.Rows[dtBilladj.Rows.Count - 1]["LocationId"] = Database.LocationId;
                   
                }
                else
                {
                    DataTable dtAcid = new DataTable();
                    Database.GetSqlData("select max(Nid) as Nid from BillAdjest where locationid='" + Database.LocationId + "'", dtAcid);
                    int Nid = int.Parse(dtAcid.Rows[0][0].ToString());
                    dtBilladj.Rows[dtBilladj.Rows.Count - 1]["id"] = Database.LocationId + (Nid + 1);
                    dtBilladj.Rows[dtBilladj.Rows.Count - 1]["Nid"] = (Nid + 1);
                    dtBilladj.Rows[dtBilladj.Rows.Count - 1]["LocationId"] = Database.LocationId;
                   
                }
              

                dtBilladj.Rows[dtBilladj.Rows.Count-1]["Ac_id"] = ac_id;
                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["vi_id"] = "0";
                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["Reff_id"] = "0";
                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["Amount"] = blnc;
                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["itemsr"] = 1;
                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["AdjustSr"] = 1;
                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["A"] = true;
                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["B"] = false;
                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["AB"] = true;
                
            }
           
            if (double.Parse(textBox14.Text) != 0)
            {
                if (dtBilladj.Rows.Count == 0)
                {
                    dtBilladj.Rows.Add();
                    DataTable dtCount = new DataTable();
                    Database.GetSqlData("select count(*) from BillAdjest where locationid='" + Database.LocationId + "'", dtCount);
                    if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                    {
                        dtBilladj.Rows[dtBilladj.Rows.Count - 1]["id"] = Database.LocationId + "1";
                        dtBilladj.Rows[dtBilladj.Rows.Count - 1]["Nid"] = 1;
                        dtBilladj.Rows[dtBilladj.Rows.Count - 1]["LocationId"] = Database.LocationId;

                    }
                    else
                    {
                        DataTable dtAcid = new DataTable();
                        Database.GetSqlData("select max(Nid) as Nid from BillAdjest where locationid='" + Database.LocationId + "'", dtAcid);
                        int Nid = int.Parse(dtAcid.Rows[0][0].ToString());
                        dtBilladj.Rows[dtBilladj.Rows.Count - 1]["id"] = Database.LocationId + (Nid + 1);
                        dtBilladj.Rows[dtBilladj.Rows.Count - 1]["Nid"] = (Nid + 1);
                        dtBilladj.Rows[dtBilladj.Rows.Count - 1]["LocationId"] = Database.LocationId;

                    }
                }
                else
                {
                       dtBilladj.Rows.Add();

                        int Nid = int.Parse(dtBilladj.Rows[0]["Nid"].ToString());
                        dtBilladj.Rows[dtBilladj.Rows.Count - 1]["id"] = Database.LocationId + (Nid + 1);
                        dtBilladj.Rows[dtBilladj.Rows.Count - 1]["Nid"] = (Nid + 1);
                        dtBilladj.Rows[dtBilladj.Rows.Count - 1]["LocationId"] = Database.LocationId;

                   
                }
              


                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["Ac_id"] = ac_id;
                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["vi_id"] = "0";
                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["Reff_id"] = "0";
                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["itemsr"] = 1;
                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["AdjustSr"] = 1;
                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["Amount"] = blnc2;
                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["A"] = false;
                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["B"] = true;
                dtBilladj.Rows[dtBilladj.Rows.Count - 1]["AB"] = true;

            }
            Database.SaveData(dtAcc);
            Database.SaveData(dtBilladj);
            Master.UpdateAccount();
            Master.UpdateAccountinfo();

         

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
        private void Displaysetting(string act_name)
        {
            string mainactname = textBox10.Text;
            if (funs.Select_act_fixed(mainactname) == false)
            {
                bool fix = false;
                while (fix == false)
                {
                    if (mainactname == "")
                    {
                        break;
                    }
                    string under = funs.Select_act_under(mainactname);
                    mainactname = funs.Select_act_nm(under);
                    fix = funs.Select_act_fixed(mainactname);

                }
            }



            string mainactnamen = textBox10.Text;

            if (mainactnamen == "SUNDRY DEBTORS" || mainactnamen == "SUNDRY CREDITORS")
            {
                if (Feature.Available("High Striction On Account").ToUpper() == "YES")
                {
                    label23.Visible = true;
                    label24.Visible = true;
                    label25.Visible = true;
                    label26.Visible = true;
                    label27.Visible = true;
                }
            }
            else
            {
                label23.Visible = false;
                label24.Visible = false;
                label25.Visible = false;
                label26.Visible = false;
                label27.Visible = false;
            }


            if (mainactname == "STOCK-IN-HAND")
            {
                textBox16.Visible = true;
                textBox16.Enabled = true;
                textBox3.Visible = false;
                label4.Text = "Balance";
                groupBox3.Text = "Stock Closing Balance";
                if (Database.IsKacha == true)
                {
                    label10.Text = "Balance2";
                    textBox4.Enabled = true;
                }
                else
                {
                    textBox4.Visible = false;
                    label10.Visible = false;
                }
            }

            else
            {
                textBox16.Visible = false;
                textBox3.Visible = true;
                if (Database.IsKacha == true)
                {
                    label10.Text = "Days";
                }
                else
                {
                    textBox4.Visible = true;
                    label10.Visible = true;
                }
                if (Feature.Available("Customer Credit Limits") == "No")
                {
                    textBox3.Enabled = false;
                    textBox4.Enabled = false;
                }

                label4.Text = "Rs.";
                groupBox3.Text = "Credit Limit";
            }

            if (mainactname == "CASH-IN-HAND" || mainactname == "Reserves & Surplus" || mainactname == "Tax" || mainactname == "Suspense" || mainactname == "Provisions")
            {
                textBox19.Enabled = true;
            }

            else if (mainactname == "SUNDRY DEBTORS" || mainactname == "SUNDRY CREDITORS" || mainactname == "Godown" || mainactname == "GODOWN")
            {
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                textBox20.Enabled = true;
                TextBox5.Enabled = true;
                TextBox6.Enabled = true;
                TextBox7.Enabled = true;
                TextBox8.Enabled = true;
                textBox9.Enabled = true;
                textBox17.Enabled = true;
                textBox19.Enabled = true;
                textBox11.Enabled = true;
                textBox12.Enabled = true;
                textBox13.Enabled = true;
                textBox28.Enabled = true;
                textBox29.Enabled = true;
                textBox30.Enabled = true;
                textBox31.Enabled = true;
                groupBox9.Visible = true;

                if (mainactname == "SUNDRY DEBTORS")
                {
                    checkBox1.Text = "Purchase Also";
                }
                else if (mainactname == "SUNDRY CREDITORS")
                {
                    checkBox1.Text = "Sale Also";
                }
            }

            else if (mainactname == "Libilities" || mainactname == "Bank" || mainactname == "Fixed Assets" || mainactname == "Unregistered Supplier" || mainactname == "Investments" || mainactname == "Security & Deposit (Asset)" || mainactname == "Loan & Advances" || mainactname == "Capital" || mainactname == "Bank Occ" || mainactname == "Unsecure Loans" || mainactname == "Secure loans")
            {
                TextBox5.Enabled = true;
                TextBox6.Enabled = true;
                TextBox7.Enabled = true;
                TextBox8.Enabled = true;
                textBox13.Enabled = true;
            }

            else
            {
                TextBox5.Enabled = false;
                TextBox6.Enabled = false;
                TextBox7.Enabled = false;
                TextBox8.Enabled = false;
                textBox9.Enabled = false;
                textBox11.Enabled = false;
                textBox12.Enabled = false;
                textBox13.Enabled = false;
                textBox17.Enabled = false;
                groupBox9.Visible = false;
            }

            if (textBox21.Text != "Unregistered")
            {
                textBox9.Enabled = true;
            }
            else
            {
                textBox9.Enabled = false;
            }
        }

        private bool validate()
        {
            if (act_name == "STOCK-IN-HAND")
            {
                if (textBox4.Text == "")
                {
                    textBox4.Text = "";
                }
                else if (textBox4.Text != "")
                {
                    textBox4.Text = textBox4.Text;
                }
                else
                {
                    textBox4.Text = "0";
                }
            }


           
            if (textBox9.Text == "")
            {
                textBox9.Text = "0";
            }
            if (textBox31.Text == "")
            {
                textBox31.Text = "0";
            }
            if (textBox29.Text == "")
            {
                textBox29.Text = "0";
            }
            if (textBox2.Text == "")
            {
                textBox2.Text = "0";
            }
            if (textBox14.Text == "")
            {
                textBox14.Text = "0";
            }
            if (textBox3.Text == "")
            {
                textBox3.Text = "0";
            }
            if (textBox21.Text == "")
            {
                textBox21.Text = "";
                textBox21.Focus();
                return false;
            }

            if (gStr == "0")
            {
                if (textBox23.Text.Trim() != "")
                {
                    int count = 0;
                    count = Database.GetScalarInt("Select count(*) from Account where MobileNo='" + textBox23.Text + "' or phone='" + textBox23.Text + "'");
                    if (count != 0)
                    {

                        DialogResult dr = MessageBox.Show("This Mobile No is already Entered With Another A/c.Are You Sure You want to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                        if (dr == System.Windows.Forms.DialogResult.Yes)
                        {

                        }
                        else
                        {
                            textBox23.Focus();
                            return false;
                        }

                    }
                }
                
                   
                

                if (TextBox7.Text.Trim() != "")
                {
                    int count = 0;
                    count = Database.GetScalarInt("Select count(*) from Account where phone='" + TextBox7.Text + "' or MobileNo='" + TextBox7.Text + "'");
                    if (count != 0)
                    {

                        DialogResult dr = MessageBox.Show("This Mobile No is already Entered With Another A/c.Are You Sure You want to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                        if (dr == System.Windows.Forms.DialogResult.Yes)
                        {

                        }
                        else
                        {
                            TextBox7.Focus();
                            return false;
                        }

                    }
                }


            }


            if (Feature.Available("Taxation Applicable") != "VAT")
            {
                if (textBox21.Text == "Composition Dealer" || textBox21.Text == "Regular Registration")
                {
                    Regex obj = new Regex("^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[0-9A-Z]{1}Z[0-9A-Z]{1}$");
                    if (textBox9.Text.Trim() == "" || textBox9.Text == "0")
                    {
                        textBox9.Focus();
                        return false;
                    }
                    else if (obj.IsMatch(textBox9.Text) == false)
                    {
                        MessageBox.Show("GSTIN is Not Correct");
                        return false;
                    }
                }
            }
            if (TextBox1.Text == "")
            {
                TextBox1.BackColor = Color.Aqua;
                TextBox1.Focus();
                return false;
            }

            else if (funs.isDouble(textBox2.Text) == false)
            {
                textBox2.BackColor = Color.Aqua;
                return false;
            }

            else if (funs.isDouble(textBox14.Text) == false)
            {
                textBox14.BackColor = Color.Aqua;
                return false;
            }

            else if (textBox10.Text == "")
            {
                textBox10.BackColor = Color.Aqua;
                textBox10.Focus();
                return false;
            }

            else if (funs.isDouble(textBox3.Text) == false)
            {
                textBox3.BackColor = Color.Aqua;
                return false;
            }

            if (funs.Select_ac_id(TextBox1.Text) != "" && funs.Select_ac_id(TextBox1.Text) != gStr)
            {
                MessageBox.Show("AccountName Already Exists.");
                return false;
            }

            if (textBox19.Text == "")
            {
                textBox19.Text = funs.Select_state_nm(Database.CompanyState_id);
            }

            if (Feature.Available("Taxation Applicable") == "VAT")
            {
                if (textBox19.Text == "")
                {
                    textBox19.Text = funs.Select_state_nm(Database.CompanyState_id);
                }
            }

            else
            {
                string mainactname = textBox10.Text;
                if (funs.Select_act_fixed(mainactname) == false)
                {
                    bool fix = false;
                    while (fix == false)
                    {
                        if (mainactname == "")
                        {
                            break;
                        }
                        string under = funs.Select_act_under(mainactname);
                        mainactname = funs.Select_act_nm(under);
                        fix = funs.Select_act_fixed(mainactname);

                    }
                }
                if (textBox19.Text == "")
                {
                    if (mainactname == "SUNDRY DEBTORS" || mainactname == "SUNDRY CREDITORS")
                    {
                        MessageBox.Show("Please Select State with this A/c");
                        textBox19.BackColor = Color.Aqua;
                        textBox19.Focus();
                        return false;
                    }

                }


            }

            string mainactnamen = textBox10.Text;

            if (gresave == false)
            {
                if (mainactnamen == "SUNDRY DEBTORS" || mainactnamen == "SUNDRY CREDITORS")
                {
                    if (Feature.Available("High Striction On Account").ToUpper() == "YES")
                    {

                        if (TextBox5.Text.Trim() == "")
                        {
                            MessageBox.Show("Enter Address");
                            TextBox5.Focus();
                            return false;
                        }
                        if (textBox28.Text.Trim() == "")
                        {
                            MessageBox.Show("Enter City");
                            textBox28.Focus();
                            return false;
                        }
                        if (textBox11.Text.Trim() == "")
                        {
                            MessageBox.Show("Enter Payment Collector");
                            textBox11.Focus();
                            return false;
                        }
                        if (textBox22.Text.Trim() == "" || textBox22.Text.Trim() == "0")
                        {
                            MessageBox.Show("Enter Rates Applicable");
                            textBox22.Focus();
                            return false;
                        }
                        if (TextBox7.Text.Trim() == "")
                        {
                            MessageBox.Show("Mobile No can't be Blank.. Please Enter Proper Mobile No.");
                            TextBox7.Focus();
                            return false;
                        }

                        if (TextBox7.Text.Trim() == "0")
                        {
                            MessageBox.Show("Mobile No can't be 0.. Please Enter Proper Mobile No.");
                            TextBox7.Focus();
                            return false;
                        }


                    }
                }
            }

            return true;
        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (AccType == null || AccType == "*")
            {
                if (Feature.Available("Multi-Godown") == "Yes")
                {
                    strCombo = "select Name from accountype where type='Account' order by Name";
                }
                else
                {
                    strCombo = "select Name from accountype where type='Account' and Name<>'Godown' order by Name";
                }

                textBox10.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
            }
            else
            {
                if (Feature.Available("Multi-Godown") == "Yes")
                {
                    strCombo = "select Name from accountype where type='Account' and Act_id " + AccType;
                }
                else
                {
                    strCombo = "select Name from accountype where type='Account' and Name<>'Godown' and Act_id " + AccType;
                }
                textBox10.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
            }
            act_name = Database.GetScalarText("Select Name from accountype where Name='" + textBox10.Text + "'");
            Displaysetting(act_name);
            Load1();
        }

        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from other where type='SER17'";
            textBox11.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox11_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                textBox11.Text = funs.AddGroup();
            }

            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox11.Text != "")
                {
                    textBox11.Text = funs.EditGroup(textBox11.Text);
                }
            }
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void radioButton1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void radioButton2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void TextBox5_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void TextBox6_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void TextBox7_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void TextBox8_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox9_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox10_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void TextBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(TextBox1);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox3);
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox4);
        }

        private void TextBox5_Enter(object sender, EventArgs e)
        {
            Database.setFocus(TextBox5);
        }

        private void TextBox6_Enter(object sender, EventArgs e)
        {
            Database.setFocus(TextBox6);
        }

        private void TextBox7_Enter(object sender, EventArgs e)
        {
            Database.setFocus(TextBox7);
        }

        private void TextBox8_Enter(object sender, EventArgs e)
        {
            Database.setFocus(TextBox8);
        }

        private void textBox11_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox11);
        }

        private void textBox9_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox9);
        }

        private void textBox9_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox9);
        }

        private void textBox11_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox11);
        }

        private void TextBox8_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(TextBox8);
        }

        private void TextBox7_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(TextBox7);
        }

        private void TextBox6_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(TextBox6);
        }

        private void TextBox5_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(TextBox5);
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox4);
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox3);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void TextBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(TextBox1);
            if (textBox18.Text == "")
            {
                textBox18.Text = TextBox1.Text;
            }
        }

        private void textBox12_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "SELECT ACCOUNT.Name FROM  ACCOUNT LEFT OUTER JOIN  ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id WHERE (ACCOUNTYPE.Name = 'Agent') AND (ACCOUNT.Branch_id = '"+Database.BranchId+"') ORDER BY ACCOUNT.Name";
            textBox12.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox12_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                textBox12.Text = funs.AddAccount();
            }
            else if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox12.Text != "")
                {
                    textBox12.Text = funs.EditAccount(textBox12.Text); ;
                }
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox14_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
        }

        private void textBox14_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox14_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox14);
        }

        private void textBox14_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox14);
        }

        private void textBox16_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox16);
        }

        private void textBox16_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox16);
        }

        private void textBox16_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar.ToString() == ".") && !(e.KeyChar.ToString() == "-");
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

        private void textBox17_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox17);
        }

        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\'')
            {
                e.Handled = true;
            }
        }

        private void textBox18_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox18_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox18);
        }

        private void textBox18_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox18);
        }

        private void textBox19_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select Sname As State from State order by Sname";
            textBox19.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox19_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox19);
        }

        private void textBox19_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox19);
        }

        private void checkBox1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox19_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                textBox19.Text = funs.AddState();
            }

            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox19.Text != "")
                {
                    textBox19.Text = funs.EditState(textBox19.Text);
                }
            }
        }

        private void radioButton6_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void radioButton5_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox20_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox20);
        }

        private void textBox20_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox20_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox20);
        }

        private void textBox21_TextChanged(object sender, EventArgs e)
        {
            if (textBox21.Text == "Unregistered")
            {
                textBox9.Text = "";
                textBox9.Enabled = false;
            }
            else if (textBox21.Text == "Regular Registration" || textBox21.Text == "Composition Dealer")
            {
                textBox9.Enabled = true;
            }
        }

        private void textBox21_KeyDown(object sender, KeyEventArgs e)
        {
            DataTable dtcombo = new DataTable();
            dtcombo.Columns.Add("Registration Status");
            dtcombo.Rows.Add();
            dtcombo.Rows[0][0] = "Unregistered";

            dtcombo.Rows.Add();
            dtcombo.Rows[1][0] = "Regular Registration";
            dtcombo.Rows.Add();
            dtcombo.Rows[2][0] = "Composition Dealer";

            textBox21.Text = SelectCombo.ComboDt(this, dtcombo, 0);
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox21_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox21);
        }

        private void textBox21_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox21);
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
            DataTable dt1 = Master.DtRates.Select().CopyToDataTable();
            textBox22.Text = SelectCombo.ComboDt(this, dt1, 0);
            SendKeys.Send("{tab}");
        }

        private void textBox23_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox23_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox23);
        }

        private void textBox23_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox23);
        }

        private void textBox10_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox10);
        }

        private void textBox12_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox12);
        }

        private void textBox13_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox13);
        }

        private void textBox13_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox13);
        }

        private void textBox23_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox24_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "";
            strCombo = "select [name] from Salesman";
            textBox24.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox24_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox24);
        }

        private void textBox24_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox24);
        }

        private void textBox24_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                textBox24.Text = funs.AddSalesman();
            }
            else if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox24.Text != "")
                {
                    textBox24.Text = funs.EditSalesman(textBox24.Text);
                }
            }
        }

        private void textBox25_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataTable dt1 = Master.DtRates.Select().CopyToDataTable();
            textBox25.Text = SelectCombo.ComboDt(this, dt1, 0);
            SendKeys.Send("{tab}");
        }

        private void textBox26_KeyDown(object sender, KeyEventArgs e)
        {
            DataTable dtcombo = new DataTable();
            dtcombo.Columns.Add("Grade");
            dtcombo.Rows.Add();
            dtcombo.Rows[0][0] = "A";

            dtcombo.Rows.Add();
            dtcombo.Rows[1][0] = "B";
            dtcombo.Rows.Add();
            dtcombo.Rows[2][0] = "C";
            dtcombo.Rows.Add();
            dtcombo.Rows[3][0] = "D";
            textBox26.Text = SelectCombo.ComboDt(this, dtcombo, 0);
           // SelectCombo.IsEnter(this, e.KeyCode);
           // SendKeys.Send("{tab}");
        }

        private void textBox26_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox26);
        }

        private void textBox26_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox26);
        }

        private void textBox27_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox27);
        }

        private void textBox27_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox27_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox27);
        }

        private void textBox28_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strcombo = "Select cname as City from city order by Cname";

            textBox28.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strcombo, textBox28.Text, 0);

        }

        private void textBox28_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox28);
        }

        private void textBox28_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox28);
        }

        private void textBox29_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this,e.KeyCode);
        }

        private void textBox29_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox29);

        }

        private void textBox29_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox29);
        }

        private void textBox30_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void textBox30_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox30);
        }

        private void textBox30_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox30);
        }

        private void textBox30_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strcombo = "SELECT ACCOUNT.Name FROM ACCOUNT LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id WHERE (((ACCOUNTYPE.RefineName)='Transport')) ";

            textBox30.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strcombo, textBox30.Text, 0);
        }

        private void textBox31_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox31);
        }

        private void textBox31_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox31);
        }

        private void textBox31_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox28_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                textBox28.Text = funs.AddCity();

            }
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox28.Text != "")
                {
                    textBox28.Text = funs.EditCity(textBox28.Text);
                }
            }
        }
    }
}
