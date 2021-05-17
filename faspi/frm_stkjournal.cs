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
    public partial class frm_stkjournal : Form
    {
        List<UsersFeature> permission;
        public bool gresave = false;
        public string cmdmode;
        string cmbVouTyp = "";
        DataTable dtVoucherInfo;
        DataTable dtVoucherDet;
        DataTable dtStock;
        string vid;
        string Prelocationid = "";
        int vno = 0;
        string vtid;
        bool locked = false;
        bool gExcludingTax = true;
        bool f12used = false;
        public bool formC = false;
        bool TaxChanged = false;
        bool RoffChanged = false;
        string[] packnm = new String[50];
        string desc_id = "";
        DateTime chkDt = new DateTime();
        public string SubCategory_Name = "", field1 = "", field2 = "", field3 = "", field4 = "", field5 = "", field6 = "", field7 = "", field8 = "";
        bool gtaxinvoice = false;
        string strCombo;
        DataTable dtDisp = new DataTable();
        DataTable dtDescItem = new DataTable();
        bool DirectChangeAmount = false;
        Boolean EditDelete = false;
        string gStr = "";

        public frm_stkjournal()
        {
            InitializeComponent();
            dateTimePicker1.MinDate = Database.stDate;
            dateTimePicker1.MaxDate = Database.ldate;
            dateTimePicker1.Value = Database.ldate;
            dateTimePicker1.CustomFormat = Database.dformat;
        }

        private void frm_stkjournal_Load(object sender, EventArgs e)
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
            //dtsidefill.Rows[0]["Visible"] = true;

            if (vid != "")
            {
                //string st = "TOP (" + Feature.Available("Voucher Editing Power") + ")";
                //if (st.ToUpper() == "TOP (UNLIMITED)")
                //{
                //    st = "";
                //}
                //DataTable dt = new DataTable();

                //Database.GetSqlData("SELECT " + st + " VOUCHERINFO.Vi_id FROM VOUCHERINFO LEFT OUTER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (VOUCHERTYPE.Type = 'Transfer') AND (VOUCHERINFO.Branch_id = '" + Database.BranchId + "') ORDER BY VOUCHERINFO.Nid DESC", dt);

                //DataTable dtfinal = new DataTable();
                //if (dt.Select("Vi_id='" + vid + "'").Length > 0)
                //{
                //    dtfinal = dt.Select("Vi_id='" + vid + "'").CopyToDataTable();
                //}

                //if (dtfinal.Rows.Count == 1)
                //{
                //    EditDelete = true;
                //    dtsidefill.Rows[0]["Visible"] = true;

                //}

                //else
                //{
                //    EditDelete = false;
                //    dtsidefill.Rows[0]["Visible"] = false;
                //}





                permission = funs.GetPermissionKey("Transfer");

                UsersFeature obalter = permission.Where(w => w.FeatureName == "Alter").FirstOrDefault();

                if (obalter != null && obalter.SelectedValue == "Not Allowed")
                {
                    dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
                }
                else
                {
                    if (obalter != null && obalter.SelectedValue == "Days Restricted")
                    {
                        string vdate = Database.GetScalarDate("Select Vdate from Voucherinfo where vi_id='" + vid + "' ");
                        obalter = permission.Where(w => w.FeatureName == "Alter Restrictions").FirstOrDefault();
                        double days = double.Parse(obalter.SelectedValue.ToString());
                        DateTime dt1 = Database.ldate.AddDays(-1 * days);
                        if (dt1 >= DateTime.Parse(vdate))
                        {
                            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
                            //MessageBox.Show("Dear User You Don't Have Permission to Delete.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //return;
                        }
                        else
                        {
                            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
                        }
                    }
                    else if (obalter != null && obalter.SelectedValue == "Count Restricted")
                    {

                        string user_id = Database.GetScalarText("Select User_id from Voucherinfo where vi_id='" + vid + "'");
                        string vt_id = Database.GetScalarText("Select Vt_id from Voucherinfo where vi_id='" + vid + "'");
                        if (Database.user_id != user_id)
                        {
                            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
                        }

                        int nid = Database.GetScalarInt("Select Nid from Voucherinfo where vi_id='" + vid + "'");

                        int countvou = Database.GetScalarInt("Select count(vi_id) from Voucherinfo where vt_id='" + vt_id + "' and  User_id='" + Database.user_id + "' and  nid>=" + nid);
                        obalter = permission.Where(w => w.FeatureName == "Alter Restrictions").FirstOrDefault();

                        double countres = double.Parse(obalter.SelectedValue.ToString());



                        if (countvou > countres)
                        {
                            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
                            //MessageBox.Show("Dear User You Don't Have Permission to Delete.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //return;
                        }

                        else
                        {
                            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
                        }
                    }
                    else if (Feature.Available("Freeze Transaction").ToUpper() != "NO")
                    {
                        string vdate = Database.GetScalarText("Select Vdate from Voucherinfo where vi_id='" + vid + "' ");

                        if (DateTime.Parse(vdate) < DateTime.Parse(Feature.Available("Freeze Transaction")))
                        {
                            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
                        }
                        else
                        {
                            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
                        }
                    }
                    else
                    {
                        dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
                    }

                }



            }
            else
            {
                permission = funs.GetPermissionKey("Transfer");
                //create
                UsersFeature ob = permission.Where(w => w.FeatureName == "Create").FirstOrDefault();
                if (ob != null && vid == "" && ob.SelectedValue == "Allowed")
                {
                    dtsidefill.Rows[0]["Visible"] = true;
                }
                else
                {
                    dtsidefill.Rows[0]["Visible"] = false;
                }

                //dtsidefill.Rows[0]["Visible"] = true;
            }

            //print
            dtsidefill.Rows.Add();
            dtsidefill.Rows[1]["Name"] = "print";
            dtsidefill.Rows[1]["DisplayName"] = "Print";
            dtsidefill.Rows[1]["ShortcutKey"] = "^P";
            if (vid != "")
            {




                permission = funs.GetPermissionKey("Transfer");

                UsersFeature obalter = permission.Where(w => w.FeatureName == "Alter").FirstOrDefault();

                if (obalter != null && obalter.SelectedValue == "Not Allowed")
                {
                    dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
                }
                else
                {
                    if (obalter != null && obalter.SelectedValue == "Days Restricted")
                    {
                        string vdate = Database.GetScalarDate("Select Vdate from Voucherinfo where vi_id='" + vid + "' ");
                        obalter = permission.Where(w => w.FeatureName == "Alter Restrictions").FirstOrDefault();
                        double days = double.Parse(obalter.SelectedValue.ToString());
                        DateTime dt1 = Database.ldate.AddDays(-1 * days);
                        if (dt1 >= DateTime.Parse(vdate))
                        {
                            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
                            //MessageBox.Show("Dear User You Don't Have Permission to Delete.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //return;
                        }
                        else
                        {
                            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
                        }
                    }
                    else if (obalter != null && obalter.SelectedValue == "Count Restricted")
                    {

                        string user_id = Database.GetScalarText("Select User_id from Voucherinfo where vi_id='" + vid + "'");
                        string vt_id = Database.GetScalarText("Select Vt_id from Voucherinfo where vi_id='" + vid + "'");
                        if (Database.user_id != user_id)
                        {
                            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
                        }

                        int nid = Database.GetScalarInt("Select Nid from Voucherinfo where vi_id='" + vid + "'");

                        int countvou = Database.GetScalarInt("Select count(vi_id) from Voucherinfo where vt_id='" + vt_id + "' and  User_id='" + Database.user_id + "' and  nid>=" + nid);
                        obalter = permission.Where(w => w.FeatureName == "Alter Restrictions").FirstOrDefault();

                        double countres = double.Parse(obalter.SelectedValue.ToString());



                        if (countvou > countres)
                        {
                            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
                            //MessageBox.Show("Dear User You Don't Have Permission to Delete.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //return;
                        }

                        else
                        {
                            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
                        }
                    }
                    else if (Feature.Available("Freeze Transaction").ToUpper() != "NO")
                    {
                        string vdate = Database.GetScalarText("Select Vdate from Voucherinfo where vi_id='" + vid + "' ");

                        if (DateTime.Parse(vdate) < DateTime.Parse(Feature.Available("Freeze Transaction")))
                        {
                            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
                        }
                        else
                        {
                            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
                        }
                    }
                    else
                    {
                        dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
                    }

                }
            }
            else
            {
               // dtsidefill.Rows[1]["Visible"] = true;
                permission = funs.GetPermissionKey("Transfer");
                //create
                UsersFeature ob = permission.Where(w => w.FeatureName == "Create").FirstOrDefault();
                if (ob != null && vid == "" && ob.SelectedValue == "Allowed")
                {
                    dtsidefill.Rows[1]["Visible"] = true;
                }
                else
                {
                    dtsidefill.Rows[1]["Visible"] = false;
                }

            }


            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count-1]["Name"] = "autofill";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Auto Cons Fill";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "^F";

            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;


            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "delete";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Delete";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "^D";
            permission = funs.GetPermissionKey("Transfer");
            //delete
            UsersFeature obdel = permission.Where(w => w.FeatureName == "Delete").FirstOrDefault();

            if (vid == "" || (obdel != null && obdel.SelectedValue == "Not Allowed"))
            {
                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
            }
            else
            {
                if (obdel != null && obdel.SelectedValue == "Days Restricted")
                {
                    string vdate = Database.GetScalarDate("Select Vdate from Voucherinfo where vi_id='" + vid + "' ");
                    obdel = permission.Where(w => w.FeatureName == "Delete  Restrictions").FirstOrDefault();
                    double days = double.Parse(obdel.SelectedValue.ToString());
                    DateTime dt1 = Database.ldate.AddDays(-1 * days);
                    if (dt1 >= DateTime.Parse(vdate))
                    {
                        dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
                        //MessageBox.Show("Dear User You Don't Have Permission to Delete.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //return;
                    }
                    else
                    {
                        dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
                    }
                }
                else if (obdel != null && obdel.SelectedValue == "Count Restricted")
                {

                    string user_id = Database.GetScalarText("Select User_id from Voucherinfo where vi_id='" + vid + "'");
                    string vt_id = Database.GetScalarText("Select Vt_id from Voucherinfo where vi_id='" + vid + "'");
                    if (Database.user_id != user_id)
                    {
                        dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
                    }

                    int nid = Database.GetScalarInt("Select Nid from Voucherinfo where vi_id='" + vid + "'");

                    int countvou = Database.GetScalarInt("Select count(vi_id) from Voucherinfo where vt_id='" + vt_id + "' and  User_id='" + Database.user_id + "' and  nid>=" + nid);
                    obdel = permission.Where(w => w.FeatureName == "Delete  Restrictions").FirstOrDefault();

                    double countres = double.Parse(obdel.SelectedValue.ToString());



                    if (countvou > countres)
                    {
                        dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
                        //MessageBox.Show("Dear User You Don't Have Permission to Delete.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //return;
                    }

                    else
                    {
                        dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
                    }
                }
                else if (Feature.Available("Freeze Transaction").ToUpper() != "NO")
                {
                    string vdate = Database.GetScalarText("Select Vdate from Voucherinfo where vi_id='" + vid + "' ");

                    if (DateTime.Parse(vdate) < DateTime.Parse(Feature.Available("Freeze Transaction")))
                    {
                        dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
                    }
                    else
                    {
                        dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
                    }

                }
                else
                {

                    dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
                }

            }






            //close
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "quit";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Quit";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "Esc";
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


        private void autofill()
        {
            DataTable dtdes = new DataTable();
            ansGridView1.Rows.Clear();
            for (int i = 0; i < ansGridView2.Rows.Count - 1; i++)
            {
                string desid = funs.Select_des_id(ansGridView2.Rows[i].Cells["orgdesc2"].Value.ToString(), ansGridView2.Rows[i].Cells["unt2"].Value.ToString());
                Database.GetSqlDataNotClear("SELECT Description.Description, dbo.Description.Pack, SUM(dbo.ProductFormula.qty)*" + ansGridView2.Rows[i].Cells["quantity2"].Value.ToString() + " AS Qty, ProductFormula.ConsumItem_id  FROM         dbo.ProductFormula INNER JOIN  Description ON dbo.ProductFormula.ConsumItem_id = dbo.Description.Des_id WHERE     (dbo.ProductFormula.productionItem_id = '" + desid + "') GROUP BY dbo.Description.Description, dbo.Description.Pack,ProductFormula.ConsumItem_id", dtdes);

            }
            DataTable dtdistinct= dtdes.DefaultView.ToTable(true, "ConsumItem_id");
            
               
            for (int k = 0; k < dtdistinct.Rows.Count; k++)
            {
                ansGridView1.Rows.Add();
                ansGridView1.Rows[k].Cells["sno"].Value = k + 1;
                ansGridView1.Rows[k].Cells["description"].Value = dtdes.Select("ConsumItem_id='"+dtdistinct.Rows[k]["ConsumItem_id"].ToString() +"'","").FirstOrDefault()["description"].ToString();
                ansGridView1.Rows[k].Cells["orgdesc"].Value = dtdes.Select("ConsumItem_id='" + dtdistinct.Rows[k]["ConsumItem_id"].ToString() + "'", "").FirstOrDefault()["description"].ToString();
                ansGridView1.Rows[k].Cells["quantity"].Value = double.Parse(dtdes.Compute("Sum(qty)","ConsumItem_id='" + dtdistinct.Rows[k]["ConsumItem_id"].ToString() + "'").ToString());
                ansGridView1.Rows[k].Cells["unt"].Value = dtdes.Select("ConsumItem_id='" + dtdistinct.Rows[k]["ConsumItem_id"].ToString() + "'", "").FirstOrDefault()["pack"].ToString();
                DataTable dtDesc = new DataTable();
                Database.GetSqlData("Select * from Description where description='" + ansGridView1.Rows[k].Cells["description"].Value.ToString() + "' and pack='" + ansGridView1.Rows[k].Cells["unt"].Value.ToString() + "'", dtDesc);

                    ansGridView1.Rows[k].Cells["rate_am"].Value = dtDesc.Rows[0]["retail"];
                    ansGridView1.Rows[k].Cells["pvalue"].Value = dtDesc.Rows[0]["pvalue"];
                    ansGridView1.Rows[k].Cells["rate_unit"].Value = dtDesc.Rows[0]["rate_unit"];
                    ansGridView1.Rows[k].Cells["Taxabelamount"].Value = 0;
                    ansGridView1.Rows[k].Cells["Des_ac_id"].Value = dtDesc.Rows[0]["Des_id"];
                    ansGridView1.Rows[k].Cells["Category_Id"].Value = dtDesc.Rows[0]["tax_cat_id"];
                    ansGridView1.Rows[k].Cells["Amount"].Value = double.Parse(dtDesc.Rows[0]["retail"].ToString()) * double.Parse(ansGridView1.Rows[k].Cells["quantity"].Value.ToString());
            }
            

        }

        private void SaveMethod(bool prnt)
        {

            try
            {
                Database.BeginTran();
                if (gresave == false)
                {
                    if (Feature.Available("Freeze Transaction") == "No")
                    {
                        if (save(prnt) == true)
                        {
                            if (gStr != "")
                            {
                                this.Close();
                                this.Dispose();
                            }
                            else
                            {
                                clear();
                                LoadData("", "Stock Journal");
                            }
                        }
                    }
                    else
                    {
                        if (dateTimePicker1.Value > DateTime.Parse(Feature.Available("Freeze Transaction")))
                        {
                            if (save(prnt) == true)
                            {
                                if (gStr != "")
                                {
                                    this.Close();
                                    this.Dispose();
                                }
                                else
                                {
                                    clear();
                                    LoadData("", "Stock Journal");
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Your Voucher is Freezed");
                        }
                    }

                }
                

                else
                {
                    DataTable dtTemp = new DataTable("Stock");
                    Database.GetSqlData("select * from Stock where Vid='" + vid + "' ", dtTemp);
                    for (int i = 0; i < dtTemp.Rows.Count; i++)
                    {
                        dtTemp.Rows[i].Delete();
                    }
                    Database.SaveData(dtTemp);

                    dtStock = new DataTable("Stock");
                    Database.GetSqlData("select * from Stock where Vid='" + vid + "' ", dtStock);

                    bool marked = Database.GetScalarBool("Select A from Vouchertype where Vt_id='" + vtid + "' ");
                    if (marked == false)
                    {
                        marked = true;
                    }
                    else
                    {
                        marked = false;
                    }
                    //stock
                    for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
                    {
                        dtStock.Rows.Add();
                        dtStock.Rows[i]["Vid"] = vid;
                        dtStock.Rows[i]["Did"] = ansGridView1.Rows[i].Cells["Des_ac_id"].Value;
                        dtStock.Rows[i]["Itemsr"] = ansGridView1.Rows[i].Cells["sno"].Value;
                        dtStock.Rows[i]["Receive"] = 0;
                        dtStock.Rows[i]["Issue"] = ansGridView1.Rows[i].Cells["Quantity"].Value;
                        dtStock.Rows[i]["ReceiveAmt"] = 0;
                        dtStock.Rows[i]["IssueAmt"] = ansGridView1.Rows[i].Cells["Amount"].Value;
                        dtStock.Rows[i]["godown_id"] = funs.Select_ac_id(textBox14.Text);
                        dtStock.Rows[i]["marked"] = marked;
                        if (ansGridView1.Rows[i].Cells["Batch_Code"].Value == null)
                        {
                            ansGridView1.Rows[i].Cells["Batch_Code"].Value = "";
                        }
                        if (Feature.Available("Batch Number") == "Yes")
                        {
                            dtStock.Rows[i]["Batch_no"] = ansGridView1.Rows[i].Cells["Batch_Code"].Value.ToString();
                        }
                        dtStock.Rows[i]["Batch_no"] = ansGridView1.Rows[i].Cells["Batch_Code"].Value.ToString();
                        dtStock.Rows[i]["LocationId"] = Prelocationid;
                        dtStock.Rows[i]["Branch_id"] = Database.BranchId;
                    }

                    Database.SaveData(dtStock);

                    dtStock.Rows.Clear();
                    for (int i = 0; i < ansGridView2.Rows.Count - 1; i++)
                    {
                        dtStock.Rows.Add();
                        dtStock.Rows[i]["Vid"] = vid;
                        dtStock.Rows[i]["Did"] = ansGridView2.Rows[i].Cells["Des_ac_id2"].Value;
                        dtStock.Rows[i]["Itemsr"] = ansGridView2.Rows[i].Cells["sno2"].Value;
                        dtStock.Rows[i]["Receive"] = ansGridView2.Rows[i].Cells["Quantity2"].Value;
                        dtStock.Rows[i]["Issue"] = 0;
                        dtStock.Rows[i]["ReceiveAmt"] = ansGridView2.Rows[i].Cells["Amount2"].Value;
                        dtStock.Rows[i]["IssueAmt"] = 0;
                        dtStock.Rows[i]["godown_id"] = funs.Select_ac_id(textBox13.Text);
                        dtStock.Rows[i]["marked"] = marked;
                        if (ansGridView2.Rows[i].Cells["Batch_Code2"].Value == null)
                        {
                            ansGridView2.Rows[i].Cells["Batch_Code2"].Value = "";
                        }
                        if (Feature.Available("Batch Number") == "Yes")
                        {
                            dtStock.Rows[i]["Batch_no"] = ansGridView2.Rows[i].Cells["Batch_Code2"].Value.ToString();
                        }
                        dtStock.Rows[i]["Batch_no"] = ansGridView2.Rows[i].Cells["Batch_Code2"].Value.ToString();
                        dtStock.Rows[i]["LocationId"] = Prelocationid;
                        dtStock.Rows[i]["Branch_id"] = Database.BranchId;
                    }
                    Database.SaveData(dtStock);
                }

                Database.CommitTran();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Voucher Not Saved, Due To An Exception", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Database.RollbackTran();
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


            if (name == "save")
            {
                if (validate() == true)
                {
                    SaveMethod(false);
                }
            }
            else if (name == "print")
            {
                if (validate() == true)
                {
                    SaveMethod(true);
                }
            }
            else if (name == "autofill")
            {
                autofill();
            }

            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }

        private bool validate()
        {
            if (textBox1.Text == "")
            {
                Database.setFocus(textBox1);
                MessageBox.Show("Enter Type");
                return false;
            }
            if (textBox13.Text == "")
            {
                Database.setFocus(textBox13);
                MessageBox.Show("Enter Godown");
                return false;
            }
            if (textBox14.Text == "")
            {
                Database.setFocus(textBox14);
                MessageBox.Show("Enter Godown");
                return false;
            }
            if (ansGridView1.Rows.Count == 1 && ansGridView2.Rows.Count == 1)
            {
                MessageBox.Show("Enter Some Values.");
                return false;
            }
            return true;
        }

        private void SetVno()
        {
            if (vtid == "" || (vno != 0 && vid != "") || f12used == true)
            {
                return;
            }
            vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
        }

        private void DisplaySetting()
        {
            foreach (DataGridViewColumn column in ansGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (DataGridViewColumn column in ansGridView2.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            ansGridView1.Columns["Amount"].ReadOnly = true;
            ansGridView2.Columns["Amount2"].ReadOnly = true;

            if (Feature.Available("Batch Number") == "Yes")
            {
                ansGridView1.Columns["Batch_Code"].Visible = true;
                ansGridView1.Columns["Batch_Code"].HeaderText = Feature.Available("Show Text on Batch Code");
                ansGridView2.Columns["Batch_Code2"].Visible = true;
                ansGridView2.Columns["Batch_Code2"].HeaderText = Feature.Available("Show Text on Batch Code");
            }

            DataTable dtvt = new DataTable();
            string cmbVouTyp3 = "";
            //if (Database.IsKacha == false)
            //{
                cmbVouTyp3 = " and "+Database.BMode+"=" + access_sql.Singlequote + "True" + access_sql.Singlequote;
            //}
            //else
            //{
            //    cmbVouTyp3 = " and B=" + access_sql.Singlequote + "True" + access_sql.Singlequote;
            //}
            cmbVouTyp = "select [name] from vouchertype where active=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " and type='Transfer'";
            cmbVouTyp = cmbVouTyp + cmbVouTyp3;
            Database.GetSqlData(cmbVouTyp, dtvt);
            if (dtvt.Rows.Count == 1)
            {
                textBox1.Text = dtvt.Rows[0]["name"].ToString();
                vtid = funs.Select_vt_id_vnm(textBox1.Text);
                textBox1.Enabled = false;
            }
            else
            {
                textBox1.Enabled = true;
            }
        }

        public void LoadData(String str, String frmCaption)
        {
            vid = str;
            gStr = str;
            this.Text = frmCaption;
            vtid = funs.Select_vt_id_vnm("Stock Journal");
            SubCategory_Name = "Local Sale";
            DisplayData(vid);
            DisplaySetting();
            SideFill();


            if (textBox1.Text != "")
            {
                SetVno();
                label10.Text = vno.ToString();
            }
            if (Feature.Available("Multi-Godown") == "No")
            {
                textBox14.Text = "<Main>";
                textBox13.Text = "<Main>";
                textBox13.Enabled = false;
                textBox14.Enabled = false;
            }
            if (gresave == true)
            {
                object sender = new object();
                EventArgs e = new EventArgs();
                btn_Click(sender, e);
            }

        }

        private void delete()
        {
            try
            {
                DataTable dttemp = new DataTable("Voucherinfo");
                Database.GetSqlData("Select * from Voucherinfo where vi_id='" + vid + "' ", dttemp);
                for (int i = 0; i < dttemp.Rows.Count; i++)
                {
                    dttemp.Rows[i].Delete();
                }
                Database.SaveData(dttemp);

                dttemp = new DataTable("voucherdet");
                Database.GetSqlData("Select * from voucherdet where vi_id='" + vid + "' ", dttemp);
                for (int i = 0; i < dttemp.Rows.Count; i++)
                {
                    dttemp.Rows[i].Delete();
                }
                Database.SaveData(dttemp);



                dttemp = new DataTable("stock");
                Database.GetSqlData("Select * from stock where vid='" + vid + "' ", dttemp);
                for (int i = 0; i < dttemp.Rows.Count; i++)
                {
                    dttemp.Rows[i].Delete();
                }
                Database.SaveData(dttemp);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox14_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Feature.Available("Allow Debtors/Creditors on Stock Journal") == "No")
            {
                strCombo = "select distinct '<MAIN>' as name from account union all SELECT ACCOUNT.Name as name FROM ACCOUNT LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id WHERE (((ACCOUNTYPE.Name)='Godown')) GROUP BY ACCOUNT.Name";
            }
            else
            {
                strCombo = "SELECT res.name as Name FROM (select distinct '<MAIN>' as name from account union all SELECT ACCOUNT.Name as name FROM ACCOUNT LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id WHERE (((ACCOUNTYPE.Name)='Godown')) GROUP BY ACCOUNT.Name Union all SELECT account.name FROM account LEFT JOIN ACCOUNTYPE ON account.Act_id = ACCOUNTYPE.Act_id where  (Path LIKE '8;40;%')  or   (Path LIKE '8;39;%')   or   (Path LIKE '1;39;%') or (Path LIKE '1;39;%') or (Path LIKE '1;38;%')    or   (Path LIKE '8;40;%')) AS res ORDER BY res.name;";
            }
            textBox14.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
        }

        private void textBox14_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox14);
        }

        private void textBox14_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox14);
        }

        private void textBox14_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox14.Text != "")
                {
                    textBox14.Text = funs.EditAccount(textBox14.Text);
                }
            }
            if (e.Control && e.KeyCode == Keys.C)
            {
                textBox14.Text = funs.AddAccount();
            }
        }

        private void textBox13_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox13);
        }

        private void textBox13_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox13);
        }

        private void textBox13_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Feature.Available("Allow Debtors/Creditors on Stock Journal") == "No")
            {
                strCombo = "select distinct '<MAIN>' as name from account union all SELECT ACCOUNT.Name as name FROM ACCOUNT LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id WHERE (((ACCOUNTYPE.Name)='Godown')) GROUP BY ACCOUNT.Name";
            }
            else
            {
                strCombo = "SELECT res.name as Name FROM (select distinct '<MAIN>' as name from account union all SELECT ACCOUNT.Name as name FROM ACCOUNT LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id WHERE (((ACCOUNTYPE.Name)='Godown')) GROUP BY ACCOUNT.Name Union all SELECT account.name FROM account LEFT JOIN ACCOUNTYPE ON account.Act_id = ACCOUNTYPE.Act_id where  (Path LIKE '8;40;%')  or   (Path LIKE '8;39;%')   or   (Path LIKE '1;39;%') or (Path LIKE '1;39;%') or (Path LIKE '1;38;%')    or   (Path LIKE '8;40;%')) AS res ORDER BY res.name;";
            }
            textBox13.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
        }

        private void textBox13_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox13.Text != "")
                {
                    textBox13.Text = funs.EditAccount(textBox14.Text);
                }
            }
            if (e.Control && e.KeyCode == Keys.C)
            {
                textBox13.Text = funs.AddAccount();
            }
        }

        private void ansGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            ansGridView1.CurrentCell.Value = 0;
        }

        private void ansGridView2_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            ansGridView2.CurrentCell.Value = 0;
        }

        private void ansGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            ansGridView1.Rows[e.RowIndex].Cells["sno"].Value = e.RowIndex + 1;
            if (ansGridView1.CurrentCell.OwningColumn.Name == "sno")
            {
                SendKeys.Send("{right}");
                this.Activate();
            }
        }

        private void ansGridView2_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            ansGridView2.Rows[e.RowIndex].Cells["sno2"].Value = e.RowIndex + 1;
            if (ansGridView2.CurrentCell.OwningColumn.Name == "sno2")
            {
                SendKeys.Send("{right}");
                this.Activate();
            }
        }

        private void ItemSelected(bool ChangeRate)
        {
            DataTable dtRateComm = new DataTable();
            DataTable dtRate = new DataTable();
            if (ansGridView1.CurrentCell.OwningRow.Cells["description"].Value.ToString() != "" && ansGridView1.CurrentCell.OwningRow.Cells["unt"].Value.ToString() != "")
            {
                if (Master.DescriptionInfo.Select("description='" + ansGridView1.CurrentCell.OwningRow.Cells["description"].Value + "' and Packing='" + ansGridView1.CurrentCell.OwningRow.Cells["unt"].Value.ToString() + "' ").Length == 0)
                {
                    return;
                }
                else
                {
                    dtRateComm = Master.DescriptionInfo.Select("description='" + ansGridView1.CurrentCell.OwningRow.Cells["description"].Value + "' and Packing='" + ansGridView1.CurrentCell.OwningRow.Cells["unt"].Value.ToString() + "' ", "").CopyToDataTable();
                }

                if (dtRateComm.Rows.Count > 0)
                {
                    ansGridView1.CurrentCell.OwningRow.Cells["rate_am"].Value = funs.DecimalPoint(dtRateComm.Rows[0]["retail"]);
                    ansGridView1.CurrentCell.OwningRow.Cells["Category_Id"].Value = dtRateComm.Rows[0]["Tax_Cat_id"];
                    ansGridView1.CurrentCell.OwningRow.Cells["Des_ac_id"].Value = dtRateComm.Rows[0]["Des_id"];
                    ansGridView1.CurrentCell.OwningRow.Cells["pvalue"].Value = dtRateComm.Rows[0]["pvalue"];
                    ansGridView1.CurrentCell.OwningRow.Cells["rate_unit"].Value = dtRateComm.Rows[0]["rate_unit"];
                    desc_id = dtRateComm.Rows[0]["Des_id"].ToString();
                }
            }
        }

        private void ItemSelected2(bool ChangeRate)
        {
            DataTable dtRateComm = new DataTable();
            DataTable dtRate = new DataTable();
            if (ansGridView2.CurrentCell.OwningRow.Cells["description2"].Value.ToString() != "" && ansGridView2.CurrentCell.OwningRow.Cells["unt2"].Value.ToString() != "")
            {
                if (Master.DescriptionInfo.Select("description='" + ansGridView2.CurrentCell.OwningRow.Cells["description2"].Value + "' and Packing='" + ansGridView2.CurrentCell.OwningRow.Cells["unt2"].Value.ToString() + "' ").Length == 0)
                {
                    return;
                }
                else
                {
                    dtRateComm = Master.DescriptionInfo.Select("description='" + ansGridView2.CurrentCell.OwningRow.Cells["description2"].Value + "' and Packing='" + ansGridView2.CurrentCell.OwningRow.Cells["unt2"].Value.ToString() + "' ", "").CopyToDataTable();
                }
                if (dtRateComm.Rows.Count > 0)
                {
                    ansGridView2.CurrentCell.OwningRow.Cells["rate_am2"].Value = funs.DecimalPoint(dtRateComm.Rows[0]["retail"]);
                    ansGridView2.CurrentCell.OwningRow.Cells["Category_Id2"].Value = dtRateComm.Rows[0]["Tax_Cat_id"];
                    ansGridView2.CurrentCell.OwningRow.Cells["Des_ac_id2"].Value = dtRateComm.Rows[0]["Des_id"];
                    ansGridView2.CurrentCell.OwningRow.Cells["pvalue2"].Value = dtRateComm.Rows[0]["pvalue"];
                    ansGridView2.CurrentCell.OwningRow.Cells["rate_unit2"].Value = dtRateComm.Rows[0]["rate_unit"];
                    desc_id = dtRateComm.Rows[0]["Des_id2"].ToString();
                }
            }
        }

        private void ansGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || e.KeyChar == ' ' || Convert.ToInt32(e.KeyChar) == 13)
            {
            }
            else
            {
                return;
            }
            if (ansGridView1.CurrentCell.OwningColumn.Name == "Batch_Code")
            {
                if (Feature.Available("Batch Number") == "Yes")
                {
                    ansGridView1.Columns["Batch_Code"].ReadOnly = true;
                    frm_Batchfrom frm = new frm_Batchfrom(ansGridView1.CurrentCell.OwningRow.Cells["Des_ac_id"].Value.ToString());
                    frm.ShowDialog();
                    DataTable dtfill = new DataTable();
                    dtfill = frm.gdt;
                    ansGridView1.AllowUserToAddRows = false;
                    if (dtfill.Rows.Count > 0)
                    {
                        ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Batch_Code"].Value = dtfill.Rows[0]["batchno"].ToString();
                    }
                    for (int i = 1; i < dtfill.Rows.Count; i++)
                    {
                        ansGridView1.Rows.Add();
                        ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["sno"].Value = ansGridView1.Rows.Count;
                        ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["description"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["description"].Value;
                        ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Quantity"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Quantity"].Value.ToString()), 3);
                        ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Rate_am"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Rate_am"].Value.ToString()), 2);
                        ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Amount"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Amount"].Value.ToString()), 2);
                        ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Des_ac_id"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Des_ac_id"].Value.ToString();
                        ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Category_Id"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Category_Id"].Value.ToString();
                        ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Taxabelamount"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Taxabelamount"].Value.ToString()), 2);
                        ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Batch_Code"].Value = dtfill.Rows[i]["batchno"].ToString();
                        ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["unt"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["unt"].Value.ToString();
                        ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["pvalue"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["pvalue"].Value.ToString();
                        ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["rate_unit"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["rate_unit"].Value.ToString();
                    }
                    ansGridView1.AllowUserToAddRows = true;
                }
            }

            if (ansGridView1.CurrentCell.OwningColumn.Name == "unt" || ansGridView1.CurrentCell.OwningColumn.Name == "description")
            {
                String ActiveCell = "";
                if (ansGridView1.CurrentCell.OwningColumn.Name == "unt")
                {
                    ActiveCell = "Packing";
                }
                else if (ansGridView1.CurrentCell.OwningColumn.Name == "description")
                {
                    ActiveCell = "Desc";
                }

                DataTable dtDesc = new DataTable();
                dtDesc = Master.DescriptionInfo.Select("Description<>''", "Description, PACKING").CopyToDataTable();
                DataTable dtPack = new DataTable(); ;
                if (ActiveCell == "Packing" && ansGridView1.CurrentCell.OwningRow.Cells["description"].Value != null && ansGridView1.CurrentCell.OwningRow.Cells["description"].Value.ToString() != "")
                {
                    dtDesc = dtDesc.Select("description='" + ansGridView1.CurrentCell.OwningRow.Cells["orgdesc"].Value.ToString() + "'").CopyToDataTable();
                }
                else if (ActiveCell == "Desc" && ansGridView1.CurrentCell.OwningRow.Cells["unt"].Value != null && ansGridView1.CurrentCell.OwningRow.Cells["unt"].Value.ToString() != "")
                {
                    dtDesc = dtDesc.Select("Packing='" + ansGridView1.CurrentCell.OwningRow.Cells["unt"].Value.ToString() + "'").CopyToDataTable();
                }
                if (ActiveCell == "Packing")
                {
                    dtPack = dtDesc.DefaultView.ToTable(true, "Packing");
                }
                else
                {
                    dtPack = dtDesc.DefaultView.ToTable(true, "description");
                }

                DataRow[] SKU = dtDesc.DefaultView.ToTable(true, "Skucode").Select("Skucode<>''");
                DataRow[] ShortCode = dtDesc.DefaultView.ToTable(true, "ShortCode").Select("ShortCode<>''");
                DataTable dtPS;
                DataTable dtPSS;
                if (SKU.Length > 0)
                {
                    DataTable dtSKU = SKU.CopyToDataTable();
                    dtPS = dtPack.AsEnumerable()
                        .Union(dtSKU.AsEnumerable()).CopyToDataTable();
                }
                else
                {
                    dtPS = dtPack.Copy();
                }
                if (ShortCode.Length > 0)
                {
                    DataTable dtShortCode = ShortCode.CopyToDataTable();
                    dtPSS = dtPS.AsEnumerable()
                         .Union(dtShortCode.AsEnumerable()).CopyToDataTable();
                }
                else
                {
                    dtPSS = dtPS.Copy();
                }
                if (ActiveCell == "Packing")
                {
                    String packing = SelectCombo.CallHelp(this, dtPSS, e.KeyChar.ToString(), 0);
                    if (packing == "") return;
                    dtDesc = dtDesc.Select("Packing='" + packing + "' or Skucode='" + packing + "' or ShortCode='" + packing + "'").CopyToDataTable();
                }
                else
                {
                    String Desc = SelectCombo.CallHelp(this, dtPSS, e.KeyChar.ToString(), 0);
                    if (Desc == "") return;
                    dtDesc = dtDesc.Select("description='" + Desc + "' or Skucode='" + Desc + "' or ShortCode='" + Desc + "'").CopyToDataTable();
                }
                if (dtDesc.Rows.Count == 1)
                {
                    ansGridView1.CurrentCell.OwningRow.Cells["description"].Value = dtDesc.Rows[0]["description"];
                    ansGridView1.CurrentCell.OwningRow.Cells["orgdesc"].Value = dtDesc.Rows[0]["description"];
                    ansGridView1.CurrentCell.OwningRow.Cells["Quantity"].Value = 0;
                    ansGridView1.CurrentCell.OwningRow.Cells["unt"].Value = dtDesc.Rows[0]["packing"];
                    ansGridView1.CurrentCell.OwningRow.Cells["rate_am"].Value = dtDesc.Rows[0]["retail"];
                    ansGridView1.CurrentCell.OwningRow.Cells["pvalue"].Value = dtDesc.Rows[0]["pvalue"];
                    ansGridView1.CurrentCell.OwningRow.Cells["rate_unit"].Value = dtDesc.Rows[0]["rate_unit"];
                    ansGridView1.CurrentCell.OwningRow.Cells["Taxabelamount"].Value = 0;
                    ansGridView1.CurrentCell.OwningRow.Cells["Des_ac_id"].Value = dtDesc.Rows[0]["Des_id"];
                    ansGridView1.CurrentCell.OwningRow.Cells["Category_Id"].Value = dtDesc.Rows[0]["tax_cat_id"];

                    if (ActiveCell == "Desc")
                    {
                        if (Feature.Available("Change Item Name") == "Yes")
                        {
                            InputBox box = new InputBox("Changed Description", ansGridView1.CurrentCell.OwningRow.Cells["orgdesc"].Value.ToString(), false);
                            box.ShowInTaskbar = false;
                            box.ShowDialog(this);
                            if (box.outStr != null)
                            {
                                ansGridView1.CurrentCell.OwningRow.Cells["description"].Value = box.outStr;
                            }
                        }
                    }
                    ansGridView1.CurrentCell = ansGridView1["Quantity", ansGridView1.CurrentCell.RowIndex];
                    this.Activate();
                }
                else if (dtDesc.Rows.Count > 1)
                {
                    if (ActiveCell == "Packing")
                    {
                        ansGridView1.CurrentCell.OwningRow.Cells["unt"].Value = dtDesc.Rows[0]["Packing"];
                    }
                    else
                    {
                        ansGridView1.CurrentCell.OwningRow.Cells["description"].Value = dtDesc.Rows[0]["description"];
                        ansGridView1.CurrentCell.OwningRow.Cells["orgdesc"].Value = dtDesc.Rows[0]["description"];
                        ansGridView1.CurrentCell.OwningRow.Cells["unt"].Value = "";
                    }
                    ansGridView1.CurrentCell.OwningRow.Cells["Taxabelamount"].Value = 0;
                    ansGridView1.CurrentCell.OwningRow.Cells["Des_ac_id"].Value = 0;
                    ansGridView1.CurrentCell.OwningRow.Cells["Quantity"].Value = 0;
                    ansGridView1.CurrentCell.OwningRow.Cells["pvalue"].Value = 0;
                    ansGridView1.CurrentCell.OwningRow.Cells["rate_unit"].Value = "";
                    ansGridView1.CurrentCell.OwningRow.Cells["Rate_am"].Value = 0;
                    ansGridView1.CurrentCell.OwningRow.Cells["Category_Id"].Value = 0;

                    if (ActiveCell == "Desc")
                    {
                        if (Feature.Available("Change Item Name") == "Yes")
                        {
                            InputBox box = new InputBox("Changed Description", ansGridView1.CurrentCell.OwningRow.Cells["orgdesc"].Value.ToString(), false);
                            box.ShowInTaskbar = false;
                            box.ShowDialog(this);
                            if (box.outStr != null)
                            {
                                ansGridView1.CurrentCell.OwningRow.Cells["description"].Value = box.outStr;
                            }
                        }
                    }

                    ItemSelected(true);
                    this.Activate();
                    if (ActiveCell == "Packing")
                    {
                        ansGridView1.CurrentCell = ansGridView1["description", ansGridView1.CurrentCell.RowIndex];
                    }
                    else
                    {
                        ansGridView1.CurrentCell = ansGridView1["unt", ansGridView1.CurrentCell.RowIndex];
                    }
                }
            }
        }

        private void ansGridView2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || e.KeyChar == ' ' || Convert.ToInt32(e.KeyChar) == 13)
            {
            }
            else
            {
                return;
            }

            if (ansGridView2.CurrentCell.OwningColumn.Name == "unt2" || ansGridView2.CurrentCell.OwningColumn.Name == "description2")
            {
                String ActiveCell = "";
                if (ansGridView2.CurrentCell.OwningColumn.Name == "unt2")
                {
                    ActiveCell = "Packing";
                }
                else if (ansGridView2.CurrentCell.OwningColumn.Name == "description2")
                {
                    ActiveCell = "Desc";
                }

                DataTable dtDesc = new DataTable();
                dtDesc = Master.DescriptionInfo.Select("Description<>''", "Description, PACKING").CopyToDataTable();
                DataTable dtPack = new DataTable();
                if (ActiveCell == "Packing" && ansGridView2.CurrentCell.OwningRow.Cells["description2"].Value != null && ansGridView2.CurrentCell.OwningRow.Cells["description2"].Value.ToString() != "")
                {
                    dtDesc = dtDesc.Select("description='" + ansGridView2.CurrentCell.OwningRow.Cells["orgdesc2"].Value.ToString() + "'").CopyToDataTable();
                }
                else if (ActiveCell == "Desc" && ansGridView2.CurrentCell.OwningRow.Cells["unt2"].Value != null && ansGridView2.CurrentCell.OwningRow.Cells["unt2"].Value.ToString() != "")
                {
                    dtDesc = dtDesc.Select("Packing='" + ansGridView2.CurrentCell.OwningRow.Cells["unt2"].Value.ToString() + "'").CopyToDataTable();
                }
                if (ActiveCell == "Packing")
                {
                    dtPack = dtDesc.DefaultView.ToTable(true, "Packing");
                }
                else
                {
                    dtPack = dtDesc.DefaultView.ToTable(true, "description");
                }

                DataRow[] SKU = dtDesc.DefaultView.ToTable(true, "Skucode").Select("Skucode<>''");
                DataRow[] ShortCode = dtDesc.DefaultView.ToTable(true, "ShortCode").Select("ShortCode<>''");
                DataTable dtPS;
                DataTable dtPSS;
                if (SKU.Length > 0)
                {
                    DataTable dtSKU = SKU.CopyToDataTable();
                    dtPS = dtPack.AsEnumerable()
                        .Union(dtSKU.AsEnumerable()).CopyToDataTable();
                }
                else
                {
                    dtPS = dtPack.Copy();
                }
                if (ShortCode.Length > 0)
                {
                    DataTable dtShortCode = ShortCode.CopyToDataTable();
                    dtPSS = dtPS.AsEnumerable()
                         .Union(dtShortCode.AsEnumerable()).CopyToDataTable();
                }
                else
                {
                    dtPSS = dtPS.Copy();
                }
                if (ActiveCell == "Packing")
                {
                    String packing = SelectCombo.CallHelp(this, dtPSS, e.KeyChar.ToString(), 0);
                    if (packing == "") return;
                    dtDesc = dtDesc.Select("Packing='" + packing + "' or Skucode='" + packing + "' or ShortCode='" + packing + "'").CopyToDataTable();
                }
                else
                {
                    String Desc = SelectCombo.CallHelp(this, dtPSS, e.KeyChar.ToString(), 0);
                    if (Desc == "") return;
                    dtDesc = dtDesc.Select("description='" + Desc + "' or Skucode='" + Desc + "' or ShortCode='" + Desc + "'").CopyToDataTable();
                }
                if (dtDesc.Rows.Count == 1)
                {
                    ansGridView2.CurrentCell.OwningRow.Cells["description2"].Value = dtDesc.Rows[0]["description"];
                    ansGridView2.CurrentCell.OwningRow.Cells["orgdesc2"].Value = dtDesc.Rows[0]["description"];
                    ansGridView2.CurrentCell.OwningRow.Cells["Quantity2"].Value = 0;
                    ansGridView2.CurrentCell.OwningRow.Cells["unt2"].Value = dtDesc.Rows[0]["packing"];
                    ansGridView2.CurrentCell.OwningRow.Cells["rate_am2"].Value = dtDesc.Rows[0]["retail"];
                    ansGridView2.CurrentCell.OwningRow.Cells["pvalue2"].Value = dtDesc.Rows[0]["pvalue"];
                    ansGridView2.CurrentCell.OwningRow.Cells["rate_unit2"].Value = dtDesc.Rows[0]["rate_unit"];
                    ansGridView2.CurrentCell.OwningRow.Cells["Taxableamount2"].Value = 0;
                    ansGridView2.CurrentCell.OwningRow.Cells["Des_ac_id2"].Value = dtDesc.Rows[0]["Des_id"];
                    ansGridView2.CurrentCell.OwningRow.Cells["Category_Id2"].Value = dtDesc.Rows[0]["tax_cat_id"];

                    if (ActiveCell == "Desc")
                    {
                        if (Feature.Available("Change Item Name") == "Yes")
                        {
                            InputBox box = new InputBox("Changed Description", ansGridView2.CurrentCell.OwningRow.Cells["orgdesc2"].Value.ToString(), false);
                            box.ShowInTaskbar = false;
                            box.ShowDialog(this);
                            if (box.outStr != null)
                            {
                                ansGridView2.CurrentCell.OwningRow.Cells["description2"].Value = box.outStr;
                            }
                        }
                    }

                    ansGridView2.CurrentCell = ansGridView2["Quantity2", ansGridView2.CurrentCell.RowIndex];
                    this.Activate();
                }
                else if (dtDesc.Rows.Count > 1)
                {
                    if (ActiveCell == "Packing")
                    {
                        ansGridView2.CurrentCell.OwningRow.Cells["unt2"].Value = dtDesc.Rows[0]["Packing"];
                    }
                    else
                    {
                        ansGridView2.CurrentCell.OwningRow.Cells["description2"].Value = dtDesc.Rows[0]["description"];
                        ansGridView2.CurrentCell.OwningRow.Cells["orgdesc2"].Value = dtDesc.Rows[0]["description"];
                        ansGridView2.CurrentCell.OwningRow.Cells["unt2"].Value = "";
                    }
                    ansGridView2.CurrentCell.OwningRow.Cells["Taxableamount2"].Value = 0;
                    ansGridView2.CurrentCell.OwningRow.Cells["Des_ac_id2"].Value = 0;
                    ansGridView2.CurrentCell.OwningRow.Cells["Quantity2"].Value = 0;
                    ansGridView2.CurrentCell.OwningRow.Cells["pvalue2"].Value = 0;
                    ansGridView2.CurrentCell.OwningRow.Cells["rate_unit2"].Value = "";
                    ansGridView2.CurrentCell.OwningRow.Cells["Rate_am2"].Value = 0;
                    ansGridView2.CurrentCell.OwningRow.Cells["Category_Id2"].Value = 0;

                    if (ActiveCell == "Desc")
                    {
                        if (Feature.Available("Change Item Name") == "Yes")
                        {
                            InputBox box = new InputBox("Changed Description", ansGridView2.CurrentCell.OwningRow.Cells["orgdesc2"].Value.ToString(), false);
                            box.ShowInTaskbar = false;
                            box.ShowDialog(this);
                            if (box.outStr != null)
                            {
                                ansGridView2.CurrentCell.OwningRow.Cells["description2"].Value = box.outStr;
                            }
                        }
                    }

                    ItemSelected2(true);
                    this.Activate();
                    if (ActiveCell == "Packing")
                    {
                        ansGridView2.CurrentCell = ansGridView2["description2", ansGridView2.CurrentCell.RowIndex];
                    }
                    else
                    {
                        ansGridView2.CurrentCell = ansGridView2["unt2", ansGridView2.CurrentCell.RowIndex];
                    }
                }
            }
        }

        private void ansGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView1.CurrentCell.OwningColumn.Name == "Rate_am" && ansGridView1.Rows[e.RowIndex].Cells["Rate_am"].Value.ToString() != "")
            {
                ansGridView1.Rows[e.RowIndex].Cells["rate_am"].Value = funs.DecimalPoint(ansGridView1.Rows[e.RowIndex].Cells["rate_am"].Value);
                ansGridView1.Rows[e.RowIndex].Cells["Amount"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[e.RowIndex].Cells["Quantity"].Value.ToString()) * double.Parse(ansGridView1.Rows[e.RowIndex].Cells["rate_am"].Value.ToString()));
            }
            if (ansGridView1.CurrentCell.OwningColumn.Name == "Quantity" && ansGridView1.Rows[e.RowIndex].Cells["Quantity"].Value.ToString() != "")
            {
                ansGridView1.Rows[e.RowIndex].Cells["Quantity"].Value = funs.DecimalPoint(ansGridView1.Rows[e.RowIndex].Cells["Quantity"].Value, 3);
                ansGridView1.Rows[e.RowIndex].Cells["Amount"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[e.RowIndex].Cells["Quantity"].Value.ToString()) * double.Parse(ansGridView1.Rows[e.RowIndex].Cells["rate_am"].Value.ToString()));
            }
        }

        private void ansGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView2.CurrentCell.OwningColumn.Name == "rate_am2" && ansGridView2.Rows[e.RowIndex].Cells["rate_am2"].Value.ToString() != "")
            {
                ansGridView2.Rows[e.RowIndex].Cells["rate_am2"].Value = funs.DecimalPoint(ansGridView2.Rows[e.RowIndex].Cells["rate_am2"].Value);
                ansGridView2.Rows[e.RowIndex].Cells["amount2"].Value = funs.DecimalPoint(double.Parse(ansGridView2.Rows[e.RowIndex].Cells["Quantity2"].Value.ToString()) * double.Parse(ansGridView2.Rows[e.RowIndex].Cells["rate_am2"].Value.ToString()));
            }
            if (ansGridView2.CurrentCell.OwningColumn.Name == "quantity2" && ansGridView2.Rows[e.RowIndex].Cells["quantity2"].Value.ToString() != "")
            {
                ansGridView2.Rows[e.RowIndex].Cells["Quantity2"].Value = funs.DecimalPoint(ansGridView2.Rows[e.RowIndex].Cells["Quantity2"].Value, 3);
                ansGridView2.Rows[e.RowIndex].Cells["amount2"].Value = funs.DecimalPoint(double.Parse(ansGridView2.Rows[e.RowIndex].Cells["Quantity2"].Value.ToString()) * double.Parse(ansGridView2.Rows[e.RowIndex].Cells["rate_am2"].Value.ToString()));
            }
            if (ansGridView2.CurrentCell.OwningColumn.Name == "Batch_Code2" && ansGridView2.Rows[e.RowIndex].Cells["Batch_Code2"].Value != null && ansGridView2.Rows[e.RowIndex].Cells["Batch_Code2"].Value.ToString() != "")
            {
                if (ansGridView2.Rows.Count == 2 || ansGridView2.Rows.Count >= 2)
                {
                    ansGridView2.AllowUserToAddRows = false;
                    string FromBatchcode = ansGridView2.Rows[ansGridView2.Rows.Count - 1].Cells["Batch_Code2"].Value.ToString();
                    InputBox box = new InputBox("No of Pcs.", "", false);
                    box.ShowDialog();
                    if (box.outStr == "")
                    {
                        FromBatchcode = "1";
                    }
                    else
                    {
                        FromBatchcode = box.outStr;
                    }

                    int no = 0;
                    no = int.Parse(FromBatchcode);
                    string batchcode = ansGridView2.Rows[ansGridView2.Rows.Count - 1].Cells["Batch_Code2"].Value.ToString();

                    for (int i = 0; i < no - 1; i++)
                    {
                        batchcode = BatctcodeGenearator(batchcode);
                        ansGridView2.Rows.Add();
                        ansGridView2.Rows[ansGridView2.Rows.Count - 1].Cells["sno2"].Value = ansGridView2.Rows.Count;
                        ansGridView2.Rows[ansGridView2.Rows.Count - 1].Cells["description2"].Value = ansGridView2.Rows[ansGridView2.Rows.Count - 2].Cells["description2"].Value;
                        ansGridView2.Rows[ansGridView2.Rows.Count - 1].Cells["Quantity2"].Value = funs.DecimalPoint(double.Parse(ansGridView2.Rows[ansGridView2.Rows.Count - 2].Cells["Quantity2"].Value.ToString()), 3);
                        ansGridView2.Rows[ansGridView2.Rows.Count - 1].Cells["Rate_am2"].Value = funs.DecimalPoint(double.Parse(ansGridView2.Rows[ansGridView2.Rows.Count - 2].Cells["Rate_am2"].Value.ToString()), 2);
                        ansGridView2.Rows[ansGridView2.Rows.Count - 1].Cells["Amount2"].Value = funs.DecimalPoint(double.Parse(ansGridView2.Rows[ansGridView2.Rows.Count - 2].Cells["Amount2"].Value.ToString()), 2);
                        ansGridView2.Rows[ansGridView2.Rows.Count - 1].Cells["Des_ac_id2"].Value = ansGridView2.Rows[ansGridView2.Rows.Count - 2].Cells["Des_ac_id2"].Value.ToString();
                        ansGridView2.Rows[ansGridView2.Rows.Count - 1].Cells["Category_Id2"].Value = ansGridView2.Rows[ansGridView2.Rows.Count - 2].Cells["Category_Id2"].Value.ToString();
                        ansGridView2.Rows[ansGridView2.Rows.Count - 1].Cells["Taxableamount2"].Value = funs.DecimalPoint(double.Parse(ansGridView2.Rows[ansGridView2.Rows.Count - 2].Cells["Taxableamount2"].Value.ToString()), 2);
                        ansGridView2.Rows[ansGridView2.Rows.Count - 1].Cells["Batch_Code2"].Value = batchcode;
                        ansGridView2.Rows[ansGridView2.Rows.Count - 1].Cells["unt2"].Value = ansGridView2.Rows[ansGridView2.Rows.Count - 2].Cells["unt2"].Value.ToString();
                        ansGridView2.Rows[ansGridView2.Rows.Count - 1].Cells["pvalue2"].Value = ansGridView2.Rows[ansGridView2.Rows.Count - 2].Cells["pvalue2"].Value.ToString();
                        ansGridView2.Rows[ansGridView2.Rows.Count - 1].Cells["rate_unit2"].Value = ansGridView2.Rows[ansGridView2.Rows.Count - 2].Cells["rate_unit2"].Value.ToString();
                    }
                }
                ansGridView2.AllowUserToAddRows = true;
            }
        }

        private string BatctcodeGenearator(string tag)
        {
            string result = null;
            Match m = Regex.Match(tag, @"^(.*?)(\d+)$");
            if (m.Success)
            {
                string head = m.Groups[1].Value;
                string tail = m.Groups[2].Value;
                string format = new string('0', tail.Length);
                int incremented = int.Parse(tail) + 1;
                result = head + incremented.ToString(format);
            }
            return result;
        }

        private bool save(bool print)
        {
            string narr = "";
            if (textBox7.Text == "")
            {
                narr = "Stock Journal";
            }
            else
            {
                narr = textBox7.Text;
            }
            //SetVno();
            //if (vno == 0)
            //{
            //    vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
            //}

            //Voucher Info
            if (dtVoucherInfo.Rows.Count == 0)
            {
                dtVoucherInfo.Rows.Add();
            }

            string prefix = "";
            string postfix = "";
            int padding = 0;
            prefix = Database.GetScalarText("Select prefix from Vouchertype where vt_id='" + vtid + "' ");
            postfix = Database.GetScalarText("Select postfix from Vouchertype where vt_id='" + vtid + "' ");
            padding = Database.GetScalarInt("Select padding from Vouchertype where vt_id='" + vtid + "' ");
            string invoiceno = vno.ToString();
            bool A = Database.GetScalarBool("Select A from Vouchertype where vt_id='" + vtid + "'");
            bool B = Database.GetScalarBool("Select B from Vouchertype where vt_id='" + vtid + "'");
            bool AB = Database.GetScalarBool("Select AB from Vouchertype where vt_id='" + vtid + "'");


            if (vid == "")
            {
                DataTable dtCount = new DataTable();
                Database.GetSqlData("select count(*) from VOUCHERINFO where locationid='" + Database.LocationId + "'", dtCount);

                if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                {
                    dtVoucherInfo.Rows[0]["Vi_id"] = Database.LocationId + "1";
                    dtVoucherInfo.Rows[0]["Nid"] = 1;
                    dtVoucherInfo.Rows[0]["user_id"] = Database.user_id;
                    dtVoucherInfo.Rows[0]["Modifiedby"] = "";
                    dtVoucherInfo.Rows[0]["Approvedby"] = "";
                    dtVoucherInfo.Rows[0]["LocationId"] = Database.LocationId;

                    Prelocationid = Database.LocationId;
                }
                else
                {
                    DataTable dtid = new DataTable();
                    Database.GetSqlData("select max(Nid) as Nid from VOUCHERINFO where locationid='" + Database.LocationId + "'", dtid);
                    int Nid = int.Parse(dtid.Rows[0][0].ToString());
                    dtVoucherInfo.Rows[0]["Vi_id"] = Database.LocationId + (Nid + 1);
                    dtVoucherInfo.Rows[0]["Nid"] = (Nid + 1);
                    dtVoucherInfo.Rows[0]["user_id"] = Database.user_id;
                    dtVoucherInfo.Rows[0]["Modifiedby"] = "";
                    dtVoucherInfo.Rows[0]["Approvedby"] = "";
                    dtVoucherInfo.Rows[0]["LocationId"] = Database.LocationId;
                    Prelocationid = Database.LocationId;
                }
            }
            else
            {

                dtVoucherInfo.Rows[0]["Modifiedby"] = Database.user_id;
                dtVoucherInfo.Rows[0]["Approvedby"] = "";
            }
            dtVoucherInfo.Rows[0]["Invoiceno"] = prefix + invoiceno.PadLeft(padding, '0') + postfix;
            dtVoucherInfo.Rows[0]["Vt_id"] = vtid;
            dtVoucherInfo.Rows[0]["Reffno"] = textBox2.Text;
            dtVoucherInfo.Rows[0]["Vnumber"] = vno;
            dtVoucherInfo.Rows[0]["ac_id"] = funs.Select_ac_id(textBox14.Text);
            dtVoucherInfo.Rows[0]["ac_id2"] = funs.Select_ac_id(textBox13.Text);
            dtVoucherInfo.Rows[0]["ShiptoAddress1"] = funs.Select_Address1(textBox13.Text);
            dtVoucherInfo.Rows[0]["ShiptoAddress2"] = funs.Select_Address2(textBox13.Text);
            dtVoucherInfo.Rows[0]["ShiptoEmail"] = funs.Select_Email(textBox13.Text);
            dtVoucherInfo.Rows[0]["ShiptoTIN"] = funs.Select_TIN(textBox13.Text);
            dtVoucherInfo.Rows[0]["ShiptoPhone"] = funs.Select_Mobile(textBox13.Text);
            dtVoucherInfo.Rows[0]["ShiptoStateid"] = funs.Select_ac_state_id(textBox13.Text);
            dtVoucherInfo.Rows[0]["Shipto"] = funs.Select_Print(textBox13.Text);
            dtVoucherInfo.Rows[0]["ShiptoPAN"] = funs.Select_PAN(textBox13.Text);
            dtVoucherInfo.Rows[0]["ShiptoAadhar"] = funs.Select_AAdhar(textBox13.Text);
            dtVoucherInfo.Rows[0]["Branch_id"] = Database.BranchId;
            dtVoucherInfo.Rows[0]["Vdate"] = dateTimePicker1.Value.Date;
            dtVoucherInfo.Rows[0]["Transdocdate"] = dateTimePicker1.Value.Date.ToString("dd-MMM-yyyy");
            dtVoucherInfo.Rows[0]["Svdate"] = dateTimePicker1.Value.Date.ToString("dd-MMM-yyyy");
            dtVoucherInfo.Rows[0]["Duedate"] = dateTimePicker1.Value.Date.ToString("dd-MMM-yyyy");
            dtVoucherInfo.Rows[0]["Narr"] = narr;
            dtVoucherInfo.Rows[0]["Totalamount"] = 0;
            dtVoucherInfo.Rows[0]["rate"] = 0;
            dtVoucherInfo.Rows[0]["Roff"] = 0;
            dtVoucherInfo.Rows[0]["Tdtype"] = gExcludingTax;
            dtVoucherInfo.Rows[0]["5000Allowed"] = false;
            dtVoucherInfo.Rows[0]["DirectChanged"] = false;
            dtVoucherInfo.Rows[0]["ITC"] = false;
            dtVoucherInfo.Rows[0]["RCM"] = false;
            dtVoucherInfo.Rows[0]["Formno"] = ";";
            dtVoucherInfo.Rows[0]["Cash_Pending"] = false;
            if (Database.utype.ToUpper() == "USER")
            {
                dtVoucherInfo.Rows[0]["NApproval"] = true;
            }
            else
            {
                dtVoucherInfo.Rows[0]["NApproval"] = false;
            }
            dtVoucherInfo.Rows[0]["RoffChanged"] = RoffChanged;
            dtVoucherInfo.Rows[0]["TaxChanged"] = TaxChanged;
            dtVoucherInfo.Rows[0]["Svnum"] = 0;
            dtVoucherInfo.Rows[0]["Iscancel"] = true;
            dtVoucherInfo.Rows[0]["FormC"] = formC;
            dtVoucherInfo.Rows[0]["Conn_id"] = 0;
            dtVoucherInfo.Rows[0]["DirectChanged"] = false;
           
            if (vid == "")
            {
                dtVoucherInfo.Rows[0]["CreTime"] = System.DateTime.Now.ToString("HH:mm:ss");
            }
            dtVoucherInfo.Rows[0]["ModTime"] = System.DateTime.Now.ToString("HH:mm:ss");

            dtVoucherInfo.Rows[0]["Cashier_approved"] = false;
            dtVoucherInfo.Rows[0]["Approved"] = false;

            Database.SaveData(dtVoucherInfo);

            if (vid == "")
            {
                DataTable dtid = new DataTable();
                Database.GetSqlData("select max(Nid) as Nid from VOUCHERINFO where locationid='" + Database.LocationId + "'", dtid);
                vid = Database.LocationId + dtid.Rows[0][0].ToString();
            }

            DataTable dtTemp = new DataTable("VOUCHERDET");
            Database.GetSqlData("select * from VOUCHERDET where vi_id='" + vid + "'", dtTemp);
            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                dtTemp.Rows[i].Delete();
            }
            Database.SaveData(dtTemp);

            dtVoucherDet = new DataTable("VOUCHERDET");
            Database.GetSqlData("select * from VOUCHERDET where vi_id='" + vid + "' ", dtVoucherDet);

            //voucherDetails
            for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
            {
                dtVoucherDet.Rows.Add();
                dtVoucherDet.Rows[i]["vi_id"] = vid;
                dtVoucherDet.Rows[i]["Itemsr"] = ansGridView1.Rows[i].Cells["sno"].Value.ToString();
                dtVoucherDet.Rows[i]["Description"] = ansGridView1.Rows[i].Cells["description"].Value.ToString();
                dtVoucherDet.Rows[i]["Quantity"] = ansGridView1.Rows[i].Cells["Quantity"].Value.ToString();
                dtVoucherDet.Rows[i]["comqty"] = ansGridView1.Rows[i].Cells["Quantity"].Value.ToString();
                dtVoucherDet.Rows[i]["Rate_am"] = ansGridView1.Rows[i].Cells["Rate_am"].Value.ToString();
                dtVoucherDet.Rows[i]["Amount"] = ansGridView1.Rows[i].Cells["Amount"].Value.ToString();
                dtVoucherDet.Rows[i]["Des_ac_id"] = ansGridView1.Rows[i].Cells["Des_ac_id"].Value.ToString();
                dtVoucherDet.Rows[i]["Category_Id"] = ansGridView1.Rows[i].Cells["Category_Id"].Value.ToString();
                dtVoucherDet.Rows[i]["Taxabelamount"] = ansGridView1.Rows[i].Cells["Taxabelamount"].Value.ToString();
                if (ansGridView1.Rows[i].Cells["Batch_Code"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["Batch_Code"].Value = "";
                }
                if (Feature.Available("Batch Number") == "Yes")
                {
                    dtVoucherDet.Rows[i]["Batch_Code"] = ansGridView1.Rows[i].Cells["Batch_Code"].Value.ToString();
                }
                dtVoucherDet.Rows[i]["Batch_Code"] = ansGridView1.Rows[i].Cells["Batch_Code"].Value.ToString();
                dtVoucherDet.Rows[i]["Commission%"] = 0;
                dtVoucherDet.Rows[i]["Rvi_id"] = 0;
                dtVoucherDet.Rows[i]["Ritemsr"] = 0;
                dtVoucherDet.Rows[i]["packing"] = ansGridView1.Rows[i].Cells["unt"].Value.ToString();
                dtVoucherDet.Rows[i]["orgpacking"] = ansGridView1.Rows[i].Cells["unt"].Value.ToString();
                dtVoucherDet.Rows[i]["pvalue"] = ansGridView1.Rows[i].Cells["pvalue"].Value.ToString();
                dtVoucherDet.Rows[i]["Rate_unit"] = ansGridView1.Rows[i].Cells["rate_unit"].Value.ToString();
                dtVoucherDet.Rows[i]["LocationId"] = Prelocationid;
                dtVoucherDet.Rows[i]["godown_id"] = funs.Select_ac_id(textBox14.Text);
                dtVoucherDet.Rows[i]["remarkreq"] = false;
                dtVoucherDet.Rows[i]["remark1"] = "";
                dtVoucherDet.Rows[i]["remark2"] = "";
                dtVoucherDet.Rows[i]["tax1"] = 0;
                dtVoucherDet.Rows[i]["tax2"] = 0;
                dtVoucherDet.Rows[i]["tax3"] = 0;
                dtVoucherDet.Rows[i]["tax4"] = 0;
                dtVoucherDet.Rows[i]["rate1"] = 0;
                dtVoucherDet.Rows[i]["rate2"] = 0;
                dtVoucherDet.Rows[i]["rate3"] = 0;
                dtVoucherDet.Rows[i]["rate4"] = 0;
                dtVoucherDet.Rows[i]["taxamt1"] = 0;
                dtVoucherDet.Rows[i]["taxamt2"] = 0;
                dtVoucherDet.Rows[i]["taxamt3"] = 0;
                dtVoucherDet.Rows[i]["taxamt4"] = 0;
                dtVoucherDet.Rows[i]["bottomdis"] = 0;
                dtVoucherDet.Rows[i]["Amount0"] = double.Parse(ansGridView1.Rows[i].Cells["Quantity"].Value.ToString()) * double.Parse(ansGridView1.Rows[i].Cells["Rate_am"].Value.ToString());
                dtVoucherDet.Rows[i]["QDType"] = "";
                dtVoucherDet.Rows[i]["QDAmount"] = 0;
                dtVoucherDet.Rows[i]["Amount1"] = 0;
                dtVoucherDet.Rows[i]["CDType"] = "";
                dtVoucherDet.Rows[i]["CDAmount"] = 0;
                dtVoucherDet.Rows[i]["Amount2"] = 0;
                dtVoucherDet.Rows[i]["FDType"] = "";
                dtVoucherDet.Rows[i]["FDAmount"] = 0;
                dtVoucherDet.Rows[i]["Amount3"] = 0;
                dtVoucherDet.Rows[i]["GridDis"] = 0;
                dtVoucherDet.Rows[i]["TotalDis"] = 0;
                dtVoucherDet.Rows[i]["Amount4"] = 0;
                dtVoucherDet.Rows[i]["TotTaxPer"] = 0;
                dtVoucherDet.Rows[i]["TotTaxAmount"] = 0;
                dtVoucherDet.Rows[i]["Amount5"] = 0;
                dtVoucherDet.Rows[i]["ExpAmount"] = 0;
                dtVoucherDet.Rows[i]["pur_sale_acc"] = 0;
                dtVoucherDet.Rows[i]["qd"] = 0;
                dtVoucherDet.Rows[i]["cd"] = 0;
                dtVoucherDet.Rows[i]["flatdis"] = 0;
                dtVoucherDet.Rows[i]["qd"] = 0;
                dtVoucherDet.Rows[i]["cd"] = 0;
                dtVoucherDet.Rows[i]["weight"] = 0;
                dtVoucherDet.Rows[i]["Cost"] = 0;
                dtVoucherDet.Rows[i]["MRP"] = 0;
                dtVoucherDet.Rows[i]["Commission@"] = 0;
                dtVoucherDet.Rows[i]["type"] = "S";
            }

            Database.SaveData(dtVoucherDet);

            dtVoucherDet.Rows.Clear();
            for (int i = 0; i < ansGridView2.Rows.Count - 1; i++)
            {
                dtVoucherDet.Rows.Add();
                dtVoucherDet.Rows[i]["vi_id"] = vid;
                dtVoucherDet.Rows[i]["Itemsr"] = ansGridView2.Rows[i].Cells["sno2"].Value.ToString();
                dtVoucherDet.Rows[i]["Description"] = ansGridView2.Rows[i].Cells["description2"].Value.ToString();
                dtVoucherDet.Rows[i]["Quantity"] = ansGridView2.Rows[i].Cells["Quantity2"].Value.ToString();
                dtVoucherDet.Rows[i]["comqty"] = ansGridView2.Rows[i].Cells["Quantity2"].Value.ToString();
                dtVoucherDet.Rows[i]["Rate_am"] = ansGridView2.Rows[i].Cells["Rate_am2"].Value.ToString();
                dtVoucherDet.Rows[i]["Amount"] = ansGridView2.Rows[i].Cells["Amount2"].Value.ToString();
                dtVoucherDet.Rows[i]["Des_ac_id"] = ansGridView2.Rows[i].Cells["Des_ac_id2"].Value.ToString();
                dtVoucherDet.Rows[i]["Category_Id"] = ansGridView2.Rows[i].Cells["Category_Id2"].Value.ToString();
                dtVoucherDet.Rows[i]["Taxabelamount"] = ansGridView2.Rows[i].Cells["Taxableamount2"].Value.ToString();
                if (ansGridView2.Rows[i].Cells["Batch_Code2"].Value == null)
                {
                    ansGridView2.Rows[i].Cells["Batch_Code2"].Value = "";
                }
                if (Feature.Available("Batch Number") == "Yes")
                {
                    dtVoucherDet.Rows[i]["Batch_Code"] = ansGridView2.Rows[i].Cells["Batch_Code2"].Value.ToString();
                }
                dtVoucherDet.Rows[i]["Batch_Code"] = ansGridView2.Rows[i].Cells["Batch_Code2"].Value.ToString();
                dtVoucherDet.Rows[i]["Rvi_id"] = 0;
                dtVoucherDet.Rows[i]["Ritemsr"] = 0;
                dtVoucherDet.Rows[i]["Commission%"] = 0;
                dtVoucherDet.Rows[i]["packing"] = ansGridView2.Rows[i].Cells["unt2"].Value.ToString();
                dtVoucherDet.Rows[i]["orgpacking"] = ansGridView2.Rows[i].Cells["unt2"].Value.ToString();
                dtVoucherDet.Rows[i]["pvalue"] = ansGridView2.Rows[i].Cells["pvalue2"].Value.ToString();
                dtVoucherDet.Rows[i]["Rate_unit"] = ansGridView2.Rows[i].Cells["rate_unit2"].Value.ToString();
                dtVoucherDet.Rows[i]["godown_id"] = funs.Select_ac_id(textBox13.Text);
                dtVoucherDet.Rows[i]["remarkreq"] = false;
                dtVoucherDet.Rows[i]["remark1"] = "";
                dtVoucherDet.Rows[i]["remark2"] = "";
                dtVoucherDet.Rows[i]["tax1"] = 0;
                dtVoucherDet.Rows[i]["tax2"] = 0;
                dtVoucherDet.Rows[i]["tax3"] = 0;
                dtVoucherDet.Rows[i]["tax4"] = 0;
                dtVoucherDet.Rows[i]["rate1"] = 0;
                dtVoucherDet.Rows[i]["rate2"] = 0;
                dtVoucherDet.Rows[i]["rate3"] = 0;
                dtVoucherDet.Rows[i]["rate4"] = 0;
                dtVoucherDet.Rows[i]["taxamt1"] = 0;
                dtVoucherDet.Rows[i]["taxamt2"] = 0;
                dtVoucherDet.Rows[i]["taxamt3"] = 0;
                dtVoucherDet.Rows[i]["taxamt4"] = 0;
                dtVoucherDet.Rows[i]["bottomdis"] = 0;
                dtVoucherDet.Rows[i]["Amount0"] = double.Parse(ansGridView2.Rows[i].Cells["Quantity2"].Value.ToString()) * double.Parse(ansGridView2.Rows[i].Cells["Rate_am2"].Value.ToString());
                dtVoucherDet.Rows[i]["QDType"] = "";
                dtVoucherDet.Rows[i]["QDAmount"] = 0;
                dtVoucherDet.Rows[i]["Amount1"] = 0;
                dtVoucherDet.Rows[i]["CDType"] = "";
                dtVoucherDet.Rows[i]["CDAmount"] = 0;
                dtVoucherDet.Rows[i]["Amount2"] = 0;
                dtVoucherDet.Rows[i]["FDType"] = "";
                dtVoucherDet.Rows[i]["FDAmount"] = 0;
                dtVoucherDet.Rows[i]["Amount3"] = 0;
                dtVoucherDet.Rows[i]["GridDis"] = 0;
                dtVoucherDet.Rows[i]["TotalDis"] = 0;
                dtVoucherDet.Rows[i]["Amount4"] = 0;
                dtVoucherDet.Rows[i]["TotTaxPer"] = 0;
                dtVoucherDet.Rows[i]["TotTaxAmount"] = 0;
                dtVoucherDet.Rows[i]["Amount5"] = 0;
                dtVoucherDet.Rows[i]["ExpAmount"] = 0;
                dtVoucherDet.Rows[i]["pur_sale_acc"] = 0;
                dtVoucherDet.Rows[i]["qd"] = 0;
                dtVoucherDet.Rows[i]["cd"] = 0;
                dtVoucherDet.Rows[i]["flatdis"] = 0;
                dtVoucherDet.Rows[i]["qd"] = 0;
                dtVoucherDet.Rows[i]["cd"] = 0;
                dtVoucherDet.Rows[i]["weight"] = 0;
                dtVoucherDet.Rows[i]["Cost"] = 0;
                dtVoucherDet.Rows[i]["MRP"] = 0;
                dtVoucherDet.Rows[i]["Commission@"] = 0;
                dtVoucherDet.Rows[i]["type"] = "D";
                dtVoucherDet.Rows[i]["LocationId"] = Prelocationid;
            }
            Database.SaveData(dtVoucherDet);

            dtTemp = new DataTable("Stock");
            Database.GetSqlData("select * from Stock where Vid='" + vid + "' ", dtTemp);
            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                dtTemp.Rows[i].Delete();
            }
            Database.SaveData(dtTemp);

            dtStock = new DataTable("Stock");
            Database.GetSqlData("select * from Stock where Vid='" + vid + "' ", dtStock);

            bool marked = Database.GetScalarBool("Select A from Vouchertype where Vt_id='" + vtid + "' ");
            if (marked == false)
            {
                marked = true;
            }
            else
            {
                marked = false;
            }
            //stock
            for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
            {
                dtStock.Rows.Add();
                dtStock.Rows[i]["Vid"] = vid;
                dtStock.Rows[i]["Did"] = ansGridView1.Rows[i].Cells["Des_ac_id"].Value;
                dtStock.Rows[i]["Itemsr"] = ansGridView1.Rows[i].Cells["sno"].Value;
                dtStock.Rows[i]["Receive"] = 0;
                dtStock.Rows[i]["Issue"] = ansGridView1.Rows[i].Cells["Quantity"].Value;
                dtStock.Rows[i]["ReceiveAmt"] = 0;
                dtStock.Rows[i]["IssueAmt"] = ansGridView1.Rows[i].Cells["Amount"].Value;
                dtStock.Rows[i]["godown_id"] = funs.Select_ac_id(textBox14.Text);
                dtStock.Rows[i]["marked"] = marked;
                if (ansGridView1.Rows[i].Cells["Batch_Code"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["Batch_Code"].Value = "";
                }
                if (Feature.Available("Batch Number") == "Yes")
                {
                    dtStock.Rows[i]["Batch_no"] = ansGridView1.Rows[i].Cells["Batch_Code"].Value.ToString();
                }
                dtStock.Rows[i]["Batch_no"] = ansGridView1.Rows[i].Cells["Batch_Code"].Value.ToString();
                dtStock.Rows[i]["LocationId"] = Prelocationid;
                dtStock.Rows[i]["Branch_id"] = Database.BranchId;
            }

            Database.SaveData(dtStock);

            dtStock.Rows.Clear();
            for (int i = 0; i < ansGridView2.Rows.Count - 1; i++)
            {
                dtStock.Rows.Add();
                dtStock.Rows[i]["Vid"] = vid;
                dtStock.Rows[i]["Did"] = ansGridView2.Rows[i].Cells["Des_ac_id2"].Value;
                dtStock.Rows[i]["Itemsr"] = ansGridView2.Rows[i].Cells["sno2"].Value;
                dtStock.Rows[i]["Receive"] = ansGridView2.Rows[i].Cells["Quantity2"].Value;
                dtStock.Rows[i]["Issue"] = 0;
                dtStock.Rows[i]["ReceiveAmt"] = ansGridView2.Rows[i].Cells["Amount2"].Value;
                dtStock.Rows[i]["IssueAmt"] = 0;
                dtStock.Rows[i]["godown_id"] = funs.Select_ac_id(textBox13.Text);
                dtStock.Rows[i]["marked"] = marked;
                if (ansGridView2.Rows[i].Cells["Batch_Code2"].Value == null)
                {
                    ansGridView2.Rows[i].Cells["Batch_Code2"].Value = "";
                }
                if (Feature.Available("Batch Number") == "Yes")
                {
                    dtStock.Rows[i]["Batch_no"] = ansGridView2.Rows[i].Cells["Batch_Code2"].Value.ToString();
                }
                dtStock.Rows[i]["Batch_no"] = ansGridView2.Rows[i].Cells["Batch_Code2"].Value.ToString();
                dtStock.Rows[i]["LocationId"] = Prelocationid;
                dtStock.Rows[i]["Branch_id"] = Database.BranchId;
            }
            Database.SaveData(dtStock);
            funs.ShowBalloonTip("Saved", "Saved Successfully");

            if (print == true)
            {
                if (Feature.Available("Ask Copies") == "No")
                {
                    OtherReport rpt = new OtherReport();
                    DataTable dtprintcopy = new DataTable();
                    Database.GetSqlData("Select printcopy from Vouchertype where Vt_id='" + vtid + "' ", dtprintcopy);
                    String[] print_option = dtprintcopy.Rows[0]["printcopy"].ToString().Split(';');
                    for (int j = 0; j < print_option.Length; j++)
                    {
                        if (print_option[j] != "")
                        {
                            String[] defaultcopy = print_option[j].Split(',');
                            if (bool.Parse(defaultcopy[1]) == true)
                            {
                                rpt.voucherprint(this, vtid, vid, defaultcopy[0], true, "Print");
                            }
                        }
                    }
                }
                else
                {
                    frm_printcopy frm = new frm_printcopy("Print", vid, vtid);
                    frm.ShowDialog();
                }
            }
            return true;
        }

        private void clear()
        {
            vno = 0;
            ansGridView1.Rows.Clear();
            ansGridView2.Rows.Clear();
            label10.Text = "";
            textBox7.Text = "";
            textBox2.Text = "";
            textBox13.Text = "";
            textBox14.Text = "";
            f12used = false;
            locked = false;
            RoffChanged = false;
            TaxChanged = false;
            DirectChangeAmount = false;
            dateTimePicker1.Focus();
            dtVoucherInfo.Rows.Add();
            ansGridView1.Rows[0].Cells["Quantity"].Value = 0;
            ansGridView1.Rows[0].Cells["Rate_am"].Value = 0;
            ansGridView1.Rows[0].Cells["Amount"].Value = 0;
            ansGridView1.Rows[0].Cells["Taxabelamount"].Value = 0;
            ansGridView1.Rows[0].Cells["Category_Id"].Value = 0;
            ansGridView2.Rows[0].Cells["Quantity2"].Value = 0;
            ansGridView2.Rows[0].Cells["Rate_am2"].Value = 0;
            ansGridView2.Rows[0].Cells["Amount2"].Value = 0;
            ansGridView2.Rows[0].Cells["Taxableamount2"].Value = 0;
            ansGridView2.Rows[0].Cells["Category_Id2"].Value = 0;
        }

        public void DisplayData(string vi_id)
        {
            dtVoucherInfo = new DataTable("Voucherinfo");
            Database.GetSqlData("select * from Voucherinfo where Vi_id='" + vi_id + "' ", dtVoucherInfo);
            if (dtVoucherInfo.Rows.Count > 0)
            {
                Prelocationid = dtVoucherInfo.Rows[0]["locationid"].ToString();
                vtid = dtVoucherInfo.Rows[0]["Vt_id"].ToString();
                textBox1.Text = funs.Select_vt_nm(vtid);
                gtaxinvoice = funs.Select_vt_taxinvoice(vtid);
                if (dtVoucherInfo.Rows[0]["ac_id"].ToString() == "")
                {
                    textBox14.Text = "<Main>";
                }
                else
                {
                    textBox14.Text = funs.Select_ac_nm(dtVoucherInfo.Rows[0]["ac_id"].ToString());
                }
                if (dtVoucherInfo.Rows[0]["ac_id2"].ToString() == "")
                {
                    textBox13.Text = "<Main>";
                }
                else
                {
                    textBox13.Text = funs.Select_ac_nm(dtVoucherInfo.Rows[0]["ac_id2"].ToString());
                }
                dateTimePicker1.Text = dtVoucherInfo.Rows[0]["Vdate"].ToString();
                vno = int.Parse(dtVoucherInfo.Rows[0]["Vnumber"].ToString());
                label10.Text = dtVoucherInfo.Rows[0]["Vnumber"].ToString();
                RoffChanged = bool.Parse(dtVoucherInfo.Rows[0]["RoffChanged"].ToString());
                TaxChanged = bool.Parse(dtVoucherInfo.Rows[0]["TaxChanged"].ToString());
                chkDt = DateTime.Parse(dtVoucherInfo.Rows[0]["Vdate"].ToString());
                textBox2.Text = dtVoucherInfo.Rows[0]["Reffno"].ToString();
                textBox7.Text = dtVoucherInfo.Rows[0]["Narr"].ToString();

                //string st = "TOP (" + Feature.Available("Voucher Editing Power") + ")";
                //if (st.ToUpper() == "TOP (UNLIMITED)")
                //{
                //    st = "";
                //}
                //DataTable dt = new DataTable();
                //string typ = Database.GetScalarText("select [Type] from Vouchertype where Vt_id='" + vtid + "'");

                ////Database.GetSqlData("SELECT " + st + " VOUCHERINFO.Vi_id FROM VOUCHERINFO LEFT OUTER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (VOUCHERTYPE.Type = '" + typ + "') AND (VOUCHERTYPE.A = " + access_sql.Singlequote + "true" + access_sql.Singlequote + ") AND (VOUCHERINFO.Branch_id = '" + Database.BranchId + "') ORDER BY VOUCHERINFO.Vdate DESC, VOUCHERTYPE.Name DESC, VOUCHERINFO.Vnumber DESC", dt);
                //Database.GetSqlData("SELECT " + st + " VOUCHERINFO.Vi_id FROM VOUCHERINFO LEFT OUTER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (VOUCHERTYPE.Type = '" + typ + "') AND (VOUCHERINFO.Branch_id = '" + Database.BranchId + "') ORDER BY VOUCHERINFO.Nid DESC", dt);

                //DataTable dtfinal = new DataTable();
                //if (dt.Select("Vi_id='" + vid + "'").Length > 0)
                //{
                //    dtfinal = dt.Select("Vi_id='" + vid + "'").CopyToDataTable();
                //}

                //if (dtfinal.Rows.Count == 1)
                //{
                //    if (Feature.Available("Voucher Delete Permission") == "Yes")
                //    {
                //        EditDelete = true;
                //    }
                //    else
                //    {
                //        EditDelete = false;
                //    }
                //}

            }
            dtVoucherDet = new DataTable("voucherdet");
            Database.GetSqlData("select * from voucherdet where vi_id='" + vi_id + "' and Type='S' order by Itemsr", dtVoucherDet);

            for (int i = 0; i < dtVoucherDet.Rows.Count; i++)
            {
                ansGridView1.Rows.Add();
                DataTable dtPackName = new DataTable();
                if (Master.DescriptionInfo.Select("Des_id='" + dtVoucherDet.Rows[i]["Des_ac_id"] + "' ", "").Length == 0)
                {
                    return;
                }
                else
                {
                    dtPackName = Master.DescriptionInfo.Select("Des_id='" + dtVoucherDet.Rows[i]["Des_ac_id"] + "' ", "").CopyToDataTable();
                }
                ansGridView1.Rows[i].Cells["sno"].Value = dtVoucherDet.Rows[i]["Itemsr"];
                ansGridView1.Rows[i].Cells["description"].Value = dtVoucherDet.Rows[i]["Description"];
                ansGridView1.Rows[i].Cells["orgdesc"].Value = funs.Select_des_nm(dtVoucherDet.Rows[i]["Des_ac_id"].ToString());
                ansGridView1.Rows[i].Cells["Quantity"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet.Rows[i]["Quantity"].ToString()), 3);
                ansGridView1.Rows[i].Cells["Rate_am"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet.Rows[i]["Rate_am"].ToString()), 2);
                ansGridView1.Rows[i].Cells["Amount"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet.Rows[i]["Amount"].ToString()), 2);
                ansGridView1.Rows[i].Cells["Des_ac_id"].Value = dtVoucherDet.Rows[i]["Des_ac_id"];
                ansGridView1.Rows[i].Cells["Category_Id"].Value = dtVoucherDet.Rows[i]["Category_Id"];
                ansGridView1.Rows[i].Cells["Batch_Code"].Value = dtVoucherDet.Rows[i]["Batch_Code"];
                ansGridView1.Rows[i].Cells["Taxabelamount"].Value = dtVoucherDet.Rows[i]["Taxabelamount"];
                ansGridView1.Rows[i].Cells["unt"].Value = dtVoucherDet.Rows[i]["packing"];
                ansGridView1.Rows[i].Cells["pvalue"].Value = dtVoucherDet.Rows[i]["pvalue"];
                ansGridView1.Rows[i].Cells["rate_unit"].Value = dtVoucherDet.Rows[i]["Rate_unit"];
            }

            dtVoucherDet = new DataTable("voucherdet");
            Database.GetSqlData("select * from voucherdet where vi_id='" + vi_id + "' and Type='D' order by Itemsr", dtVoucherDet);

            for (int i = 0; i < dtVoucherDet.Rows.Count; i++)
            {
                ansGridView2.Rows.Add();
                DataTable dtPackName = new DataTable();
                if (Master.DescriptionInfo.Select("Des_id='" + dtVoucherDet.Rows[i]["Des_ac_id"] + "' ", "").Length == 0)
                {
                    return;
                }
                else
                {
                    dtPackName = Master.DescriptionInfo.Select("Des_id='" + dtVoucherDet.Rows[i]["Des_ac_id"] + "' ", "").CopyToDataTable();
                }
                ansGridView2.Rows[i].Cells["sno2"].Value = dtVoucherDet.Rows[i]["Itemsr"];
                ansGridView2.Rows[i].Cells["orgdesc2"].Value = funs.Select_des_nm(dtVoucherDet.Rows[i]["Des_ac_id"].ToString());
                ansGridView2.Rows[i].Cells["description2"].Value = dtVoucherDet.Rows[i]["Description"];
                ansGridView2.Rows[i].Cells["Quantity2"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet.Rows[i]["Quantity"].ToString()), 3);
                ansGridView2.Rows[i].Cells["Batch_Code2"].Value = dtVoucherDet.Rows[i]["Batch_Code"].ToString();
                ansGridView2.Rows[i].Cells["Rate_am2"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet.Rows[i]["Rate_am"].ToString()), 2);
                ansGridView2.Rows[i].Cells["Amount2"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet.Rows[i]["Amount"].ToString()), 2);
                ansGridView2.Rows[i].Cells["Des_ac_id2"].Value = dtVoucherDet.Rows[i]["Des_ac_id"];
                ansGridView2.Rows[i].Cells["Category_Id2"].Value = dtVoucherDet.Rows[i]["Category_Id"];
                ansGridView2.Rows[i].Cells["Taxableamount2"].Value = dtVoucherDet.Rows[i]["Taxabelamount"];
                ansGridView2.Rows[i].Cells["unt2"].Value = dtVoucherDet.Rows[i]["packing"];
                ansGridView2.Rows[i].Cells["pvalue2"].Value = dtVoucherDet.Rows[i]["pvalue"];
                ansGridView2.Rows[i].Cells["rate_unit2"].Value = dtVoucherDet.Rows[i]["Rate_unit"];
            }

            dtStock = new DataTable("Stock");
            Database.GetSqlData("select * from Stock where Vid='" + vi_id + "' ", dtStock);
        }

        private void ansGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (ansGridView1.CurrentCell == null)
            {
                return;
            }
            if (ansGridView1.CurrentCell.OwningColumn.Name == "Amount")
            {
                if (ansGridView1.CurrentCell.Value == null || ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Amount"].Value.ToString() == "")
                {
                    return;
                }
                if (ansGridView1.CurrentRow.Index == ansGridView1.Rows.Count - 1 && ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Amount"].Value.ToString() == "")
                {
                    SendKeys.Send("{tab}");
                }
            }
            if (e.KeyCode == Keys.Delete)
            {
                if (ansGridView1.CurrentRow.Index == ansGridView1.Rows.Count - 1)
                {
                    for (int i = 1; i < ansGridView1.Columns.Count; i++)
                    {
                        ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells[i].Value = null;
                    }
                }
                else
                {
                    int rindex = ansGridView1.CurrentRow.Index;
                    ansGridView1.Rows.RemoveAt(rindex);
                    for (int i = 0; i < ansGridView1.Rows.Count; i++)
                    {
                        ansGridView1.Rows[i].Cells["sno"].Value = (i + 1);
                    }
                    return;
                }
            }
        }

        private void ansGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (ansGridView2.CurrentCell == null)
            {
                return;
            }
            if (ansGridView2.CurrentCell.OwningColumn.Name == "Amount2")
            {
                if (ansGridView2.CurrentCell.Value == null || ansGridView2.Rows[ansGridView2.CurrentRow.Index].Cells["Amount2"].Value.ToString() == "")
                {
                    return;
                }
                if (ansGridView2.CurrentRow.Index == ansGridView2.Rows.Count - 1 && double.Parse(ansGridView2.Rows[ansGridView2.CurrentRow.Index].Cells["Amount2"].Value.ToString()) == 0)
                {
                    SendKeys.Send("{tab}");
                }
            }
            if (e.KeyCode == Keys.Delete)
            {
                if (ansGridView2.CurrentRow.Index == ansGridView2.Rows.Count - 1)
                {
                    for (int i = 1; i < ansGridView2.Columns.Count; i++)
                    {
                        ansGridView2.Rows[ansGridView2.CurrentRow.Index].Cells[i].Value = null;
                    }
                }
                else
                {
                    int rindex = ansGridView2.CurrentRow.Index;
                    ansGridView2.Rows.RemoveAt(rindex);
                    for (int i = 0; i < ansGridView2.Rows.Count; i++)
                    {
                        ansGridView2.Rows[i].Cells["sno2"].Value = (i + 1);
                    }
                    return;
                }
            }
        }

        private void frm_stkjournal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                if (ansGridView2.Rows.Count > 1)
                {
                    ansGridView2.Rows.Clear();
                }
                for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
                {
                    ansGridView2.Rows.Add();
                    for (int j = 0; j < ansGridView1.Columns.Count; j++)
                    {
                        ansGridView2.Rows[i].Cells[j].Value = ansGridView1.Rows[i].Cells[j].Value;
                    }
                }
            }
            else if (e.Control && e.KeyCode == Keys.F)
            {
                autofill();
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                if (vid == "")
                {
                    if (validate() == true)
                    {
                       // SaveMethod(false);

                        UsersFeature ob = permission.Where(w => w.FeatureName == "Create").FirstOrDefault();
                        if (ob != null && gStr == "" && ob.SelectedValue == "Allowed")
                        {
                            SaveMethod(false);
                        }   
                    }
                }
                else
                {
                   
                        if (validate() == true)
                        {
                            permission = funs.GetPermissionKey("Transfer");

                            UsersFeature obalter = permission.Where(w => w.FeatureName == "Alter").FirstOrDefault();

                            if (obalter != null && obalter.SelectedValue == "Not Allowed")
                            {
                                MessageBox.Show("Dear User You Don't Have Permission to Alter.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            else
                            {
                                if (obalter != null && obalter.SelectedValue == "Days Restricted")
                                {
                                    string vdate = Database.GetScalarDate("Select Vdate from Voucherinfo where vi_id='" + vid + "' ");
                                    obalter = permission.Where(w => w.FeatureName == "Alter Restrictions").FirstOrDefault();
                                    double days = double.Parse(obalter.SelectedValue.ToString());
                                    DateTime dt1 = Database.ldate.AddDays(-1 * days);
                                    if (dt1 >= DateTime.Parse(vdate))
                                    {

                                        MessageBox.Show("Dear User You Don't Have Permission to Alter.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                    else
                                    {
                                        SaveMethod(false);
                                    }
                                }
                                else if (obalter != null && obalter.SelectedValue == "Count Restricted")
                                {

                                    string user_id = Database.GetScalarText("Select User_id from Voucherinfo where vi_id='" + vid + "'");
                                    string vt_id = Database.GetScalarText("Select Vt_id from Voucherinfo where vi_id='" + vid + "'");
                                    if (Database.user_id != user_id)
                                    {

                                        MessageBox.Show("Dear User You Don't Have Permission to Alter.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }

                                    int nid = Database.GetScalarInt("Select Nid from Voucherinfo where vi_id='" + vid + "'");

                                    int countvou = Database.GetScalarInt("Select count(vi_id) from Voucherinfo where vt_id='" + vt_id + "' and  User_id='" + Database.user_id + "' and  nid>=" + nid);
                                    obalter = permission.Where(w => w.FeatureName == "Alter Restrictions").FirstOrDefault();

                                    double countres = double.Parse(obalter.SelectedValue.ToString());



                                    if (countvou > countres)
                                    {

                                        MessageBox.Show("Dear User You Don't Have Permission to Alter.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }

                                    else
                                    {
                                        SaveMethod(false);
                                    }
                                }
                                else if (Feature.Available("Freeze Transaction").ToUpper() != "NO")
                                {
                                    string vdate = Database.GetScalarText("Select Vdate from Voucherinfo where vi_id='" + vid + "' ");

                                    if (DateTime.Parse(vdate) < DateTime.Parse(Feature.Available("Freeze Transaction")))
                                    {
                                        MessageBox.Show("Freezed voucher", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                    else
                                    {
                                        SaveMethod(false);
                                    }
                                }
                                else
                                {
                                    SaveMethod(false);
                                }

                            }  
                        }
                   
                }
            }
            else if (e.Control && e.KeyCode == Keys.P)
            {
                if (vid == "")
                {
                    if (validate() == true)
                    {
                        UsersFeature ob = permission.Where(w => w.FeatureName == "Create").FirstOrDefault();
                        if (ob != null && gStr == "" && ob.SelectedValue == "Allowed")
                        {
                            SaveMethod(true);
                        }   
                    }
                }
                else
                {
                    
                        if (validate() == true)
                        {
                            permission = funs.GetPermissionKey("Transfer");

                            UsersFeature obalter = permission.Where(w => w.FeatureName == "Alter").FirstOrDefault();

                            if (obalter != null && obalter.SelectedValue == "Not Allowed")
                            {
                                MessageBox.Show("Dear User You Don't Have Permission to Alter.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            else
                            {
                                if (obalter != null && obalter.SelectedValue == "Days Restricted")
                                {
                                    string vdate = Database.GetScalarDate("Select Vdate from Voucherinfo where vi_id='" + vid + "' ");
                                    obalter = permission.Where(w => w.FeatureName == "Alter Restrictions").FirstOrDefault();
                                    double days = double.Parse(obalter.SelectedValue.ToString());
                                    DateTime dt1 = Database.ldate.AddDays(-1 * days);
                                    if (dt1 >= DateTime.Parse(vdate))
                                    {

                                        MessageBox.Show("Dear User You Don't Have Permission to Alter.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                    else
                                    {
                                        SaveMethod(true);
                                    }
                                }
                                else if (obalter != null && obalter.SelectedValue == "Count Restricted")
                                {

                                    string user_id = Database.GetScalarText("Select User_id from Voucherinfo where vi_id='" + vid + "'");
                                    string vt_id = Database.GetScalarText("Select Vt_id from Voucherinfo where vi_id='" + vid + "'");
                                    if (Database.user_id != user_id)
                                    {

                                        MessageBox.Show("Dear User You Don't Have Permission to Alter.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }

                                    int nid = Database.GetScalarInt("Select Nid from Voucherinfo where vi_id='" + vid + "'");

                                    int countvou = Database.GetScalarInt("Select count(vi_id) from Voucherinfo where vt_id='" + vt_id + "' and  User_id='" + Database.user_id + "' and  nid>=" + nid);
                                    obalter = permission.Where(w => w.FeatureName == "Alter Restrictions").FirstOrDefault();

                                    double countres = double.Parse(obalter.SelectedValue.ToString());



                                    if (countvou > countres)
                                    {

                                        MessageBox.Show("Dear User You Don't Have Permission to Alter.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }

                                    else
                                    {
                                        SaveMethod(true);
                                    }
                                }
                                else if (Feature.Available("Freeze Transaction").ToUpper() != "NO")
                                {
                                    string vdate = Database.GetScalarText("Select Vdate from Voucherinfo where vi_id='" + vid + "' ");

                                    if (DateTime.Parse(vdate) < DateTime.Parse(Feature.Available("Freeze Transaction")))
                                    {
                                        MessageBox.Show("Freezed voucher", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                    else
                                    {
                                        SaveMethod(true);
                                    }
                                }
                                else
                                {
                                    SaveMethod(true);
                                }

                            }  
                        }
                    
                }
            }
            else if (e.Control && e.KeyCode == Keys.D)
            {
               
                    if (vid != "")
                    {
                        permission = funs.GetPermissionKey("Transfer");
                        //delete
                        UsersFeature obdel = permission.Where(w => w.FeatureName == "Delete").FirstOrDefault();

                        if (obdel != null && obdel.SelectedValue == "Not Allowed")
                        {
                            MessageBox.Show("Dear User You Don't Have Permission to Delete.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;

                        }
                        else
                        {
                            if (obdel != null && obdel.SelectedValue == "Days Restricted")
                            {
                                string vdate = Database.GetScalarDate("Select Vdate from Voucherinfo where vi_id='" + vid + "' ");
                                obdel = permission.Where(w => w.FeatureName == "Delete  Restrictions").FirstOrDefault();
                                double days = double.Parse(obdel.SelectedValue.ToString());
                                DateTime dt1 = Database.ldate.AddDays(-1 * days);
                                if (dt1 >= DateTime.Parse(vdate))
                                {
                                    MessageBox.Show("Dear User You Don't Have Permission to Delete.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                            else if (obdel != null && obdel.SelectedValue == "Count Restricted")
                            {

                                string user_id = Database.GetScalarText("Select User_id from Voucherinfo where vi_id='" + vid + "'");
                                string vt_id = Database.GetScalarText("Select Vt_id from Voucherinfo where vi_id='" + vid + "'");
                                if (Database.user_id != user_id)
                                {
                                    return;
                                }

                                int nid = Database.GetScalarInt("Select Nid from Voucherinfo where vi_id='" + vid + "'");

                                int countvou = Database.GetScalarInt("Select count(vi_id) from Voucherinfo where vt_id='" + vt_id + "' and  User_id='" + Database.user_id + "' and  nid>=" + nid);
                                obdel = permission.Where(w => w.FeatureName == "Delete  Restrictions").FirstOrDefault();

                                double countres = double.Parse(obdel.SelectedValue.ToString());

                                if (countvou > countres)
                                {
                                    MessageBox.Show("Dear User You Don't Have Permission to Delete.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }


                        }

                        if (MessageBox.Show("Are You Sure To Delete This Voucher", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {

                            Database.BeginTran();
                           delete();

                            Database.CommitTran();
                            this.Close();
                            this.Dispose();
                        }

                    }
               
            }
            else if (e.KeyCode == Keys.Escape)
            {
                if (ansGridView1.Rows.Count > 1)
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

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (Database.IsKacha == false)
            //{
            cmbVouTyp = "select [name] from vouchertype where active=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " and type='Transfer' and " + Database.BMode + "=" + access_sql.Singlequote + "true" + access_sql.Singlequote + "";
            //}
            //else
            //{
            //    cmbVouTyp = "select [name] from vouchertype where active=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " and type='Transfer' and B=" + access_sql.Singlequote + "true" + access_sql.Singlequote + "";
            //}
            textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, cmbVouTyp, e.KeyChar.ToString(), 0);
            vtid = funs.Select_vt_id_vnm(textBox1.Text);
            SetVno();
            label10.Text = vno.ToString();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.Alt == true && e.KeyCode == Keys.N)
            {
                textBox7.ReadOnly = true;
                DataTable dtcombo = new DataTable();
                strCombo = "Select Distinct(Narr) from Voucherinfo where Narr<>' ' order by Narr";
                Database.GetSqlData(strCombo, dtcombo);
                textBox7.Text = SelectCombo.ComboDt(this, dtcombo, 0);
                textBox7.ReadOnly = false;
                SendKeys.Send("{End}");
            }
            else
            {
                SelectCombo.IsEnter(this, e.KeyCode);
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            SetVno();
            if (vno == 0)
            {
                vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
            }
            label10.Text = vno.ToString();
        }
    }
}
