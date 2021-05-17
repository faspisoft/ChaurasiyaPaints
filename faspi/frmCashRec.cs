using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Web.Script.Serialization;

namespace faspi
{
    public partial class frmCashRec : Form
    {
        public bool gresave = false;
        DataTable dtItemCharges;
        DataTable dtVoucherInfo;
        DataTable dtVoucheractotal;
        DataTable dtJournal;
        DateTime dtm;
        DataTable dtBilladjest;
        public String recpay;
        Boolean generateVno = false;
        public String cmdnm;
        public String gStr="";
        Boolean itemCharges = false;
        String Prelocationid="";
        string vid = "", vtid = "", cashac_id = "";
        int vno = 0;
        string cmbVouTyp = "";         
        DateTime chkDt = new DateTime();      
        DataTable dtFid = new DataTable();
        DataTable dtUid = new DataTable();
        String strCombo;
        Boolean f12used = false;
        Boolean EditDelete = false;
        public String gfrmCaption;
        List<UsersFeature> permission;




        public frmCashRec()
        {
            InitializeComponent();
            dateTimePicker1.MinDate = Database.stDate;
            dateTimePicker1.MaxDate = Database.ldate;
            dateTimePicker1.CustomFormat = Database.dformat;
            dateTimePicker1.Value = Database.ldate; 
        }

        private void frmCashRec_Load(object sender, EventArgs e)
        {
            ansGridView1.Columns["Amount"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            SideFill();
        }

        private void frmCashRec_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Control && e.KeyCode == Keys.S)
            {
                if (vid == "")
                {
                    if (validate() == true)
                    {
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
                            permission = funs.GetPermissionKey(recpay);

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

                                        if (Database.user_id != user_id)
                                        {

                                            MessageBox.Show("Dear User You Don't Have Permission to Alter.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return;
                                        }

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
                            permission = funs.GetPermissionKey(recpay);

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
                                        if (Database.user_id != user_id)
                                        {

                                            MessageBox.Show("Dear User You Don't Have Permission to Alter.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return;
                                        }
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

                        permission = funs.GetPermissionKey(recpay);
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
                if (label4.Text != "0")
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
            else if (e.KeyCode == Keys.F12)
            {
                InputBox box = new InputBox("Enter Administrative password", "", true);
                box.ShowDialog(this);
                MessageBox.Show(box.outStr);
                String pass = box.outStr;
                if (pass.ToLower() == "admin")
                {
                    box = new InputBox("Enter Voucher Number", "", false);
                    box.ShowDialog();
                    vno = int.Parse(box.outStr);
                    generateVno = true;
                }
                else
                {
                    MessageBox.Show("Invalid Password");
                }
            }
        }

        private void SaveMethod(bool print)
        {
            try
            {
                Database.BeginTran();
                if (gresave == false)
                {
                    if (Feature.Available("Freeze Transaction") == "No")
                    {
                        if (save(print) == true)
                        {
                            if (gStr != "")
                            {
                                this.Close();
                                this.Dispose();
                            }
                            else
                            {
                                clear();
                            }
                        }
                    }
                    else
                    {
                        if (dateTimePicker1.Value > DateTime.Parse(Feature.Available("Freeze Transaction")))
                        {
                            if (save(print) == true)
                            {
                                if (gStr != "")
                                {
                                    this.Close();
                                    this.Dispose();
                                }
                                else
                                {
                                    clear();
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

                    bool A = Database.GetScalarBool("Select A from Vouchertype where vt_id='" + vtid + "'");
                    bool B = Database.GetScalarBool("Select B from Vouchertype where vt_id='" + vtid + "'");
                    bool AB = Database.GetScalarBool("Select AB from Vouchertype where vt_id='" + vtid + "'");



                    DataTable dtTemp = new DataTable("Billadjest");
                    Database.GetSqlData("Select * from Billadjest where Vi_id='" + vid + "'", dtTemp);
                    for (int j = 0; j < dtTemp.Rows.Count; j++)
                    {
                        dtTemp.Rows[j].Delete();
                    }
                    Database.SaveData(dtTemp);
                    dtBilladjest = new DataTable("Billadjest");
                    Database.GetSqlData("Select * from Billadjest where Vi_id='" + vid + "'", dtBilladjest);



                    for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
                    {
                        //if (dtBilladjest.Select("Itemsr=" + int.Parse(ansGridView1.Rows[i].Cells["sno"].Value.ToString())).Length == 0)
                        //{

                            if (funs.Select_MainAccTypeName(ansGridView1.Rows[i].Cells["acc"].Value.ToString()) == "SUNDRY DEBTORS" || funs.Select_MainAccTypeName(ansGridView1.Rows[i].Cells["acc"].Value.ToString()) == "SUNDRY CREDITORS")
                            {
                                if (dtBilladjest.Rows.Count == 0)
                                {
                                    dtBilladjest.Rows.Add();
                                    DataTable dtCount = new DataTable();
                                    Database.GetSqlData("select count(*) from BillAdjest where locationid='" + Database.LocationId + "'", dtCount);
                                    if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                                    {
                                        dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["id"] = Database.LocationId + "1";
                                        dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Nid"] = 1;
                                        dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["LocationId"] = Database.LocationId;

                                    }
                                    else
                                    {
                                        DataTable dtAcid = new DataTable();
                                        Database.GetSqlData("select max(Nid) as Nid from BillAdjest where locationid='" + Database.LocationId + "'", dtAcid);
                                        int Nid = int.Parse(dtAcid.Rows[0][0].ToString());
                                        dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["id"] = Database.LocationId + (Nid + 1);
                                        dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Nid"] = (Nid + 1);
                                        dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["LocationId"] = Database.LocationId;

                                    }
                                }
                                else
                                {
                                    dtBilladjest.Rows.Add();
                                    int rowindex = dtBilladjest.Rows.Count - 2;
                                    int Nid = int.Parse(dtBilladjest.Rows[rowindex]["Nid"].ToString());
                                    dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["id"] = Database.LocationId + (Nid + 1);
                                    dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Nid"] = (Nid + 1);
                                    dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["LocationId"] = Database.LocationId;


                                }

                                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Ac_id"] = funs.Select_ac_id(ansGridView1.Rows[i].Cells["acc"].Value.ToString());
                                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Vi_id"] = vid;
                                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["itemsr"] = int.Parse(ansGridView1.Rows[i].Cells["sno"].Value.ToString());
                                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["AdjustSr"] = 1;
                                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Reff_id"] = vid;


                                if (recpay == "Receipt")
                                {

                                    dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Amount"] = -1 * double.Parse(ansGridView1.Rows[i].Cells["Amount"].Value.ToString());
                                }
                                else
                                {
                                    dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Amount"] = double.Parse(ansGridView1.Rows[i].Cells["Amount"].Value.ToString());
                                }
                                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["A"] = A;
                                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["B"] = B;
                                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["AB"] = true;
                            }

                    //    }

                    }
                    //for (int i = 0; i < dtBilladjest.Rows.Count; i++)
                    //{

                    //    if (funs.Select_MainAccTypeName(ansGridView1.Rows[i].Cells["acc"].Value.ToString()) == "SUNDRY DEBTORS" || funs.Select_MainAccTypeName(ansGridView1.Rows[i].Cells["acc"].Value.ToString()) == "SUNDRY CREDITORS")
                    //    {
                    //        if (dtBilladjest.Rows.Count == 0)
                    //        {

                    //            DataTable dtCount = new DataTable();
                    //            Database.GetSqlData("select count(*) from BillAdjest where locationid='" + Database.LocationId + "'", dtCount);
                    //            if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                    //            {
                    //                dtBilladjest.Rows[i]["id"] = Database.LocationId + "1";
                    //                dtBilladjest.Rows[i]["Nid"] = 1;
                    //                dtBilladjest.Rows[i]["LocationId"] = Database.LocationId;

                    //            }
                    //            else
                    //            {
                    //                DataTable dtAcid = new DataTable();
                    //                Database.GetSqlData("select max(Nid) as Nid from BillAdjest where locationid='" + Database.LocationId + "'", dtAcid);
                    //                int Nid = int.Parse(dtAcid.Rows[0][0].ToString());
                    //                dtBilladjest.Rows[i]["id"] = Database.LocationId + (Nid + 1);
                    //                dtBilladjest.Rows[i]["Nid"] = (Nid + 1);
                    //                dtBilladjest.Rows[i]["LocationId"] = Database.LocationId;

                    //            }
                    //        }
                    //        else
                    //        {

                    //            //int rowindex = i;
                    //            //int Nid = int.Parse(dtBilladjest.Rows[rowindex]["Nid"].ToString());
                    //            ////   int Nid = int.Parse(dtBilladjest.Rows[0]["Nid"].ToString());
                    //            //dtBilladjest.Rows[i]["id"] = Database.LocationId + (Nid + 1);
                    //            //dtBilladjest.Rows[i]["Nid"] = (Nid + 1);
                    //            //dtBilladjest.Rows[i]["LocationId"] = Database.LocationId;

                    //            DataTable dtAcid = new DataTable();
                    //            Database.GetSqlData("select max(Nid) as Nid from BillAdjest where locationid='" + Database.LocationId + "'", dtAcid);
                    //            int Nid = 0;
                    //            if (i == 0)
                    //            {
                    //                Nid = int.Parse(dtAcid.Rows[0][0].ToString());
                    //            }
                    //            else
                    //            {
                    //                Nid = int.Parse(dtBilladjest.Rows[i - 1]["Nid"].ToString());
                    //            }
                    //            dtBilladjest.Rows[i]["id"] = Database.LocationId + (Nid + 1);
                    //            dtBilladjest.Rows[i]["Nid"] = (Nid + 1);
                    //            dtBilladjest.Rows[i]["LocationId"] = Database.LocationId;

                    //        }

                    //        dtBilladjest.Rows[i]["Vi_id"] = vid;
                    //        dtBilladjest.Rows[i]["A"] = A;
                    //        dtBilladjest.Rows[i]["B"] = B;
                    //        dtBilladjest.Rows[i]["AB"] = true;

                    //        dtBilladjest.Rows[i].AcceptChanges();
                    //        dtBilladjest.Rows[i].SetAdded();
                    //    }
                    //}
                    Database.SaveData(dtBilladjest);




                    funs.ShowBalloonTip("Saved", "Saved Successfully");

                }
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Voucher Not Saved, Due To An Exception  " + vno + dateTimePicker1.Value.Date.ToString(Database.dformat), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Database.RollbackTran();
            }
        }

        private void DisplayData()
        {
            dtVoucherInfo = new DataTable("Voucherinfo");
            Database.GetSqlData("select * from voucherinfo where vi_id='" + vid + "' ", dtVoucherInfo);

            if (dtVoucherInfo.Rows.Count > 0)
            {
                Prelocationid = dtVoucherInfo.Rows[0]["locationid"].ToString();
                textBox1.Text = funs.Select_ac_nm(dtVoucherInfo.Rows[0]["Ac_id"].ToString());
                textBox2.Text = dtVoucherInfo.Rows[0]["Narr"].ToString();
                textBox3.Enabled = false;
                textBox3.Text = funs.Select_vt_nm(dtVoucherInfo.Rows[0]["Vt_id"].ToString());
                vtid = dtVoucherInfo.Rows[0]["Vt_id"].ToString();
                dateTimePicker1.Value = DateTime.Parse(dtVoucherInfo.Rows[0]["Vdate"].ToString());
                vno = int.Parse(dtVoucherInfo.Rows[0]["Vnumber"].ToString());
                label10.Text = vno.ToString();
                chkDt = DateTime.Parse(dtVoucherInfo.Rows[0]["Vdate"].ToString());

                //string st = "TOP (" + Feature.Available("Voucher Editing Power") + ")";
                //if (st.ToUpper() == "TOP (UNLIMITED)")
                //{
                //    st = "";
                //}
                //DataTable dt = new DataTable();
                //string typ = Database.GetScalarText("select [Type] from Vouchertype where Vt_id='" + vtid + "'");

                //Database.GetSqlData("SELECT " + st + " VOUCHERINFO.Vi_id FROM VOUCHERINFO LEFT OUTER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (VOUCHERTYPE.Type = '" + typ + "') AND (VOUCHERINFO.Branch_id = '" + Database.BranchId + "') ORDER BY VOUCHERINFO.Nid DESC", dt);

                //DataTable dtfinal = new DataTable();
                //if (dt.Select("Vi_id='" + vid + "'").Length > 0)
                //{
                //    dtfinal = dt.Select("Vi_id='" + vid + "'").CopyToDataTable();
                //    if (dtfinal.Rows.Count == 1)
                //    {
                //        if (Feature.Available("Voucher Delete Permission") == "Yes")
                //        {
                //            EditDelete = true;
                //        }
                //        else
                //        {
                //            EditDelete = false;
                //        }
                //    }
                //}
            }

            dtVoucheractotal = new DataTable("Voucheractotal");
            Database.GetSqlData("select * from voucheractotal where vi_id='" + vid + "' order by Srno", dtVoucheractotal);

            for (int i = 0; i < dtVoucheractotal.Rows.Count; i++)
            {
                ansGridView1.Rows.Add();
                ansGridView1.Rows[i].Cells["sno"].Value = dtVoucheractotal.Rows[i]["Srno"];
                ansGridView1.Rows[i].Cells["acc"].Value = funs.Select_ac_nm(dtVoucheractotal.Rows[i]["Accid"].ToString());
                ansGridView1.Rows[i].Cells["instrumentno"].Value = dtVoucheractotal.Rows[i]["Chkno"];

                if (dtVoucheractotal.Rows[i]["Cdate"].ToString() == "")
                {
                    dtVoucheractotal.Rows[i]["Cdate"] = dtVoucherInfo.Rows[0]["Vdate"].ToString();
                }
                ansGridView1.Rows[i].Cells["instrumentdt"].Value = DateTime.Parse(dtVoucheractotal.Rows[i]["Cdate"].ToString()).ToString("dd / MM / yyyy");
                ansGridView1.Rows[i].Cells["Amount"].Value =double.Parse(dtVoucheractotal.Rows[i]["Amount"].ToString());
              
            }

            dtItemCharges = new DataTable("ITEMCHARGES");
            Database.GetSqlData("SELECT ITEMCHARGES.*, CHARGES.Name AS ChargeName FROM ITEMCHARGES INNER JOIN CHARGES ON ITEMCHARGES.Charg_id = CHARGES.Ch_id where vi_id='" + vid + "' order by Itemsr", dtItemCharges);

            dtBilladjest = new DataTable("BillAdjest");
            Database.GetSqlData("select * from BillAdjest where vi_id='" + vid + "' order by ItemSr,AdjustSr", dtBilladjest);
            dtJournal = new DataTable("Journal");
            Database.GetSqlData("select * from Journal where vi_id='" + vid + "' ", dtJournal);

            foreach (DataGridViewColumn column in ansGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            ansGridView1.Columns["Amount"].CellTemplate.ValueType = typeof(double);
        }

        public void LoadData(String str, String frmCaption)
        {
            vid = str;
            gStr = str;
            gfrmCaption = frmCaption;

            vtid = funs.Select_vt_id_vnm(textBox3.Text);
            vid = str;
            dtBilladjest = new DataTable("BillAdjest");
            dtBilladjest.Columns.Add("Vi_id", typeof(string));
            dtBilladjest.Columns.Add("Reff_id", typeof(string));
            dtBilladjest.Columns.Add("Ac_id", typeof(string));
            dtBilladjest.Columns.Add("ItemSr", typeof(int));
            dtBilladjest.Columns.Add("AdjustSr", typeof(int));
            dtBilladjest.Columns.Add("Amount", typeof(decimal));
            this.Text = frmCaption;
            Display();
            DisplayData();
            calcTot();
            SetVno();
            label10.Text = vno.ToString();
            if (vid == "")
            {
                textBox2.Text = funs.Select_vt_Narrtemplate(vtid);
            }
            SideFill();

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

                dttemp = new DataTable("voucheractotal");
                Database.GetSqlData("Select * from voucheractotal where vi_id='" + vid + "' ", dttemp);
                for (int i = 0; i < dttemp.Rows.Count; i++)
                {
                    dttemp.Rows[i].Delete();
                }
                Database.SaveData(dttemp);

                dttemp = new DataTable("journal");
                Database.GetSqlData("Select * from journal where vi_id='" + vid + "' ", dttemp);
                for (int i = 0; i < dttemp.Rows.Count; i++)
                {
                    dttemp.Rows[i].Delete();
                }
                Database.SaveData(dttemp);

                dttemp = new DataTable("BILLBYBILL");
                Database.GetSqlData("Select * from BILLBYBILL where Bill_id='" + vid + "' ", dttemp);
                for (int i = 0; i < dttemp.Rows.Count; i++)
                {
                    dttemp.Rows[i].Delete();
                }
                Database.SaveData(dttemp);

                dttemp = new DataTable("BILLBYBILL");
                Database.GetSqlData("Select * from BILLBYBILL where receive_id='" + vid + "' ", dttemp);
                for (int i = 0; i < dttemp.Rows.Count; i++)
                {
                    dttemp.Rows[i].Delete();
                }
                Database.SaveData(dttemp);
                dttemp = new DataTable("Billadjest");
                Database.GetSqlData("Select * from Billadjest where Vi_id='" + vid + "' ", dttemp);
                for (int i = 0; i < dttemp.Rows.Count; i++)
                {
                    dttemp.Rows[i].Delete();
                }
                Database.SaveData(dttemp);


                dttemp = new DataTable("Billadjest");
                Database.GetSqlData("Select * from Billadjest where Reff_id='" + vid + "' ", dttemp);
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
        private string IsDocumentNumber(String str)
        {

            string res= Database.GetScalarText("SELECT DISTINCT VOUCHERINFO.Vi_id FROM VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)) )='" + str + "'))");
            if (res == "")
            {
                res = "0";
            }
          
            return res;
        }
        private bool save(bool print)
        {
            ansGridView1.EndEdit();
            string actname = "";
            String cashac_id = funs.Select_ac_id(textBox1.Text);
            int conn_id, ac_id;
            String narr = "";

           
                narr = textBox2.Text;
            

            conn_id = 0;
           
            String[] dtmCheck = { "", "", "" };
            if (vno == 0)
            {
                vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
            }

            if (dtVoucherInfo.Rows.Count == 0)
            {
                dtVoucherInfo.Rows.Add();
            }
            bool A = Database.GetScalarBool("Select A from Vouchertype where vt_id='" + vtid + "'");
            bool B = Database.GetScalarBool("Select B from Vouchertype where vt_id='" + vtid + "'");
            bool AB = Database.GetScalarBool("Select AB from Vouchertype where vt_id='" + vtid + "'");



            string prefix = "";
            string postfix = "";
            int padding = 0;
            prefix = Database.GetScalarText("Select prefix from Vouchertype where vt_id='" + vtid + "' ");
            postfix = Database.GetScalarText("Select postfix from Vouchertype where vt_id='" + vtid + "' ");
            padding = Database.GetScalarInt("Select padding from Vouchertype where vt_id='" + vtid + "' ");
            string invoiceno = vno.ToString();

            if (vid == "")
            {
                DataTable dtCount = new DataTable();
                Database.GetSqlData("select count(*) from VOUCHERINFO where locationid='" + Database.LocationId + "'", dtCount);

                if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                {
                    dtVoucherInfo.Rows[0]["Vi_id"] = Database.LocationId + "1";
                    dtVoucherInfo.Rows[0]["Nid"] = 1;
                    dtVoucherInfo.Rows[0]["LocationId"] = Database.LocationId;
                    dtVoucherInfo.Rows[0]["user_id"] = Database.user_id;
                    dtVoucherInfo.Rows[0]["Modifiedby"] = "";
                    dtVoucherInfo.Rows[0]["Approvedby"] = "";
                    Prelocationid = Database.LocationId;

                }
                else
                {
                    DataTable dtid = new DataTable();
                    Database.GetSqlData("select max(Nid) as Nid from VOUCHERINFO where locationid='" + Database.LocationId + "'", dtid);
                    int Nid = int.Parse(dtid.Rows[0][0].ToString());
                    dtVoucherInfo.Rows[0]["Vi_id"] = Database.LocationId + (Nid + 1);
                    dtVoucherInfo.Rows[0]["Nid"] = (Nid + 1);
                    dtVoucherInfo.Rows[0]["LocationId"] = Database.LocationId;
                    dtVoucherInfo.Rows[0]["user_id"] = Database.user_id;
                    dtVoucherInfo.Rows[0]["Modifiedby"] = "";
                    dtVoucherInfo.Rows[0]["Approvedby"] = "";
                    Prelocationid = Database.LocationId;

                }
            }
            else
            {
               
                dtVoucherInfo.Rows[0]["Modifiedby"] = Database.user_id;
                dtVoucherInfo.Rows[0]["Approvedby"] = "";
            }

            dtVoucherInfo.Rows[0]["Invoiceno"] = prefix + invoiceno.PadLeft(padding, '0') + postfix;
            dtVoucherInfo.Rows[0]["Transdocdate"] = dateTimePicker1.Value.Date.ToString("dd-MMM-yyyy");
            dtVoucherInfo.Rows[0]["Vt_id"] = vtid;
            dtVoucherInfo.Rows[0]["Vnumber"] = vno;
            dtVoucherInfo.Rows[0]["Ac_id2"] = "";
            dtVoucherInfo.Rows[0]["ac_id"] = cashac_id;
            dtVoucherInfo.Rows[0]["branch_id"] = Database.BranchId;
            dtVoucherInfo.Rows[0]["Vdate"] = dateTimePicker1.Value.Date.ToString("dd-MMM-yyyy");
            dtVoucherInfo.Rows[0]["SVdate"] = dateTimePicker1.Value.Date.ToString("dd-MMM-yyyy");
            dtVoucherInfo.Rows[0]["Duedate"] = dateTimePicker1.Value.Date.ToString("dd-MMM-yyyy");
            dtVoucherInfo.Rows[0]["Narr"] = narr;
            dtVoucherInfo.Rows[0]["Totalamount"] = label4.Text;
            dtVoucherInfo.Rows[0]["Conn_id"] = conn_id;
            dtVoucherInfo.Rows[0]["FormC"] = false;
            dtVoucherInfo.Rows[0]["RCM"] = false;
            dtVoucherInfo.Rows[0]["5000Allowed"] = false;
            dtVoucherInfo.Rows[0]["DirectChanged"] = false;
            dtVoucherInfo.Rows[0]["ITC"] = false;
            dtVoucherInfo.Rows[0]["Reffno"] = "";
            dtVoucherInfo.Rows[0]["Iscancel"] = true;
            dtVoucherInfo.Rows[0]["Cash_Pending"] = false;

            dtVoucherInfo.Rows[0]["Tdtype"] = false;
            dtVoucherInfo.Rows[0]["RoffChanged"] = false;
            dtVoucherInfo.Rows[0]["TaxChanged"] = false;
            if (recpay == "Payment" || recpay == "Contra")
            {
                dtVoucherInfo.Rows[0]["dr_ac_id"] = funs.Select_ac_id(ansGridView1.Rows[0].Cells["acc"].Value.ToString());
                dtVoucherInfo.Rows[0]["cr_ac_id"] = cashac_id;

            }
            else
            {
                dtVoucherInfo.Rows[0]["dr_ac_id"] = cashac_id;
                dtVoucherInfo.Rows[0]["cr_ac_id"] = funs.Select_ac_id(ansGridView1.Rows[0].Cells["acc"].Value.ToString());

            }
            if (Database.utype.ToUpper() == "USER")
            {
                dtVoucherInfo.Rows[0]["NApproval"] = true;
            }
            else
            {
                dtVoucherInfo.Rows[0]["NApproval"] = false;
            }
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

            DataTable dtTemp = new DataTable("Voucheractotal");
            Database.GetSqlData("select * from Voucheractotal where vi_id='" + vid + "' ", dtTemp);
            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                dtTemp.Rows[i].Delete();
            }
            Database.SaveData(dtTemp);

            dtVoucheractotal = new DataTable("Voucheractotal");
            Database.GetSqlData("select * from Voucheractotal where vi_id='" + vid + "' ", dtVoucheractotal);

            //Voucheractotal
            for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
            {
                if (ansGridView1.Rows[i].Cells["instrumentdt"].Value == null)
                {
                    dtmCheck[0] = dateTimePicker1.Value.Day.ToString();
                    dtmCheck[1] = dateTimePicker1.Value.Month.ToString();
                    dtmCheck[2] = dateTimePicker1.Value.Year.ToString();
                }
                else if (ansGridView1.Rows[i].Cells["instrumentdt"].Value.ToString().Split('/').Length == 3)
                {
                    dtmCheck = ansGridView1.Rows[i].Cells["instrumentdt"].Value.ToString().Split('/');
                }
                else if (ansGridView1.Rows[i].Cells["instrumentdt"].Value.ToString().Split('-').Length == 3)
                {
                    dtmCheck = ansGridView1.Rows[i].Cells["instrumentdt"].Value.ToString().Split('-');
                }
                else if (ansGridView1.Rows[i].Cells["instrumentdt"].Value.ToString().Split('.').Length == 3)
                {
                    dtmCheck = ansGridView1.Rows[i].Cells["instrumentdt"].Value.ToString().Split('.');
                }
                dtm = new DateTime(int.Parse(dtmCheck[2]), int.Parse(dtmCheck[1]), int.Parse(dtmCheck[0]));
                string chkno;
                if (ansGridView1.Rows[i].Cells["instrumentno"].Value != null)
                {

                    chkno = ansGridView1.Rows[i].Cells["instrumentno"].Value.ToString();
                }
                else
                {
                    chkno = "";
                }


                //string reffno = "";
                //if (ansGridView1.Rows[i].Cells["reffno"].Value == null || ansGridView1.Rows[i].Cells["reffno"].Value.ToString() == "<New Refference>")
                //{
                //    reffno = vid;
                //}
                //else
                //{
                //    reffno = IsDocumentNumber(ansGridView1.Rows[i].Cells["reffno"].Value.ToString());
                //} 

                dtVoucheractotal.Rows.Add();
                dtVoucheractotal.Rows[i]["vi_id"] = vid;
                dtVoucheractotal.Rows[i]["Srno"] = ansGridView1.Rows[i].Cells["sno"].Value.ToString();
                dtVoucheractotal.Rows[i]["Chkno"] = chkno;
              
                dtVoucheractotal.Rows[i]["Cdate"] = dtm.ToString("dd-MMM-yyyy");
                dtVoucheractotal.Rows[i]["Accid"] = funs.Select_ac_id(ansGridView1.Rows[i].Cells["acc"].Value.ToString());
                dtVoucheractotal.Rows[i]["Amount"] = ansGridView1.Rows[i].Cells["Amount"].Value;
                dtVoucheractotal.Rows[i]["LocationId"] = Prelocationid;
            }

            Database.SaveData(dtVoucheractotal);

            //ItemCharges
            dtTemp = new DataTable("itemcharges");
            Database.GetSqlData("Select * from itemcharges where Vi_id='" + vid + "' ", dtTemp);
            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                dtTemp.Rows[i].Delete();
            }
            Database.SaveData(dtTemp);

            dtItemCharges = new DataTable("itemcharges");
            Database.GetSqlData("Select * from itemcharges where Vi_id='" + vid + "' ", dtItemCharges);

            for (int i = 0; i < dtItemCharges.Rows.Count; i++)
            {
                dtItemCharges.Rows[i]["Vi_id"] = vid;
                dtItemCharges.Rows[i]["LocationId"] = Prelocationid;
                dtItemCharges.Rows[i].AcceptChanges();
                dtItemCharges.Rows[i].SetAdded();
            }

            Database.SaveData(dtItemCharges);

            //Journal
            dtTemp = new DataTable("Journal");
            Database.GetSqlData("Select * from Journal where Vi_id='" + vid + "' ", dtTemp);
            for (int j = 0; j < dtTemp.Rows.Count; j++)
            {
                dtTemp.Rows[j].Delete();
            }
            Database.SaveData(dtTemp);

            DataTable dtJournal = new DataTable("Journal");
            Database.GetSqlData("Select * from Journal where Vi_id='" + vid + "' ", dtJournal);

            for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
            {
                double amount = double.Parse(ansGridView1.Rows[i].Cells["Amount"].Value.ToString());
                string jnarr = narr;

                

                //textbox
                dtJournal.Rows.Add();
                dtJournal.Rows[dtJournal.Rows.Count - 1]["vdate"] = dateTimePicker1.Value.ToString("dd-MMM-yyyy");
                dtJournal.Rows[dtJournal.Rows.Count - 1]["Vi_id"] = vid;
                dtJournal.Rows[dtJournal.Rows.Count - 1]["Ac_id"] = cashac_id;
                dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = funs.Select_ac_id(ansGridView1.Rows[i].Cells["acc"].Value.ToString()); ;
                dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr"] = jnarr;
                dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr2"] = jnarr;
                dtJournal.Rows[dtJournal.Rows.Count - 1]["Sno"] = i + 1;
                dtJournal.Rows[dtJournal.Rows.Count - 1]["LocationId"] = Database.LocationId;
                if (ansGridView1.Rows[i].Cells["instrumentno"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["instrumentno"].Value = "";
                }
                dtJournal.Rows[dtJournal.Rows.Count - 1]["Reffno"] = ansGridView1.Rows[i].Cells["instrumentno"].Value.ToString();
               

                if (recpay == "Receipt")
                {
                    dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = amount;
                }
                else if (recpay == "Payment")
                {
                    dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = -1 * amount;
                }
                else if (recpay == "Contra")
                {
                   
                    dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = -1 * amount;
                }
                dtJournal.Rows[dtJournal.Rows.Count - 1]["A"] = A;
                dtJournal.Rows[dtJournal.Rows.Count - 1]["B"] = B;
                dtJournal.Rows[dtJournal.Rows.Count - 1]["AB"] = AB;
                //grid
                dtJournal.Rows.Add();
                dtJournal.Rows[dtJournal.Rows.Count - 1]["vdate"] = dateTimePicker1.Value.ToString("dd-MMM-yyyy");
                dtJournal.Rows[dtJournal.Rows.Count - 1]["Vi_id"] = vid;
                dtJournal.Rows[dtJournal.Rows.Count - 1]["Ac_id"] = funs.Select_ac_id(ansGridView1.Rows[i].Cells["acc"].Value.ToString());
                dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = cashac_id;
                dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr"] = jnarr;
                dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr2"] = jnarr;
                dtJournal.Rows[dtJournal.Rows.Count - 1]["Sno"] = i + 1;
                dtJournal.Rows[dtJournal.Rows.Count - 1]["LocationId"] = Database.LocationId;
                dtJournal.Rows[dtJournal.Rows.Count - 1]["Reffno"] = ansGridView1.Rows[i].Cells["instrumentno"].Value.ToString();
               
                if (recpay == "Receipt")
                {
                    dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = -1 * amount;
                }
                else if (recpay == "Payment")
                {
                    dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = amount;
                }
                else if (recpay == "Contra")
                {
                    
                    dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = amount;
                }
                dtJournal.Rows[dtJournal.Rows.Count - 1]["A"] = A;
                dtJournal.Rows[dtJournal.Rows.Count - 1]["B"] = B;
                dtJournal.Rows[dtJournal.Rows.Count - 1]["AB"] = AB;
            }
            Database.SaveData(dtJournal);

           dtTemp= new DataTable("Billadjest");
            Database.GetSqlData("Select * from Billadjest where Vi_id='" + vid+"'", dtTemp);
            for (int j = 0; j < dtTemp.Rows.Count; j++)
            {
                dtTemp.Rows[j].Delete();
            }
            Database.SaveData(dtTemp);




            for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
            {
                if (dtBilladjest.Select("Itemsr=" + int.Parse(ansGridView1.Rows[i].Cells["sno"].Value.ToString())).Length == 0)
                {
                    if (funs.Select_MainAccTypeName(ansGridView1.Rows[i].Cells["acc"].Value.ToString()) == "SUNDRY DEBTORS" || funs.Select_MainAccTypeName(ansGridView1.Rows[i].Cells["acc"].Value.ToString()) == "SUNDRY CREDITORS")
                    {
                        if (dtBilladjest.Rows.Count == 0)
                        {
                            dtBilladjest.Rows.Add();
                            DataTable dtCount = new DataTable();
                            Database.GetSqlData("select count(*) from BillAdjest where locationid='" + Database.LocationId + "'", dtCount);
                            if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                            {
                                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["id"] = Database.LocationId + "1";
                                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Nid"] = 1;
                                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["LocationId"] = Database.LocationId;

                            }
                            else
                            {
                                DataTable dtAcid = new DataTable();
                                Database.GetSqlData("select max(Nid) as Nid from BillAdjest where locationid='" + Database.LocationId + "'", dtAcid);
                                int Nid = int.Parse(dtAcid.Rows[0][0].ToString());
                                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["id"] = Database.LocationId + (Nid + 1);
                                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Nid"] = (Nid + 1);
                                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["LocationId"] = Database.LocationId;

                            }
                        }
                        else
                        {
                            dtBilladjest.Rows.Add();
                            int rowindex = dtBilladjest.Rows.Count - 2;
                            int Nid = int.Parse(dtBilladjest.Rows[rowindex]["Nid"].ToString());
                            //   int Nid = int.Parse(dtBilladjest.Rows[0]["Nid"].ToString());
                            dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["id"] = Database.LocationId + (Nid + 1);
                            dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Nid"] = (Nid + 1);
                            dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["LocationId"] = Database.LocationId;


                        }

                        dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Ac_id"] = funs.Select_ac_id(ansGridView1.Rows[i].Cells["acc"].Value.ToString());
                        dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Vi_id"] = vid;
                        dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["itemsr"] = int.Parse(ansGridView1.Rows[i].Cells["sno"].Value.ToString());
                        dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["AdjustSr"] = 1;
                        dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Reff_id"] = vid;
                        dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["A"] = A;
                        dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["B"] = B;
                        dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["AB"] = true;
                        if (recpay == "Receipt")
                        {

                            dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Amount"] = -1 * double.Parse(ansGridView1.Rows[i].Cells["Amount"].Value.ToString());
                        }
                        else
                        {
                            dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Amount"] = double.Parse(ansGridView1.Rows[i].Cells["Amount"].Value.ToString());
                        }

                    }

                }

                else
                {


                    //for (int i = 0; i < dtBilladjest.Rows.Count; i++)
                    //{

                        if (funs.Select_MainAccTypeName(ansGridView1.Rows[i].Cells["acc"].Value.ToString()) == "SUNDRY DEBTORS" || funs.Select_MainAccTypeName(ansGridView1.Rows[i].Cells["acc"].Value.ToString()) == "SUNDRY CREDITORS")
                        {
                            if (dtBilladjest.Rows.Count == 0)
                            {

                                DataTable dtCount = new DataTable();
                                Database.GetSqlData("select count(*) from BillAdjest where locationid='" + Database.LocationId + "'", dtCount);
                                if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                                {
                                    dtBilladjest.Rows[i]["id"] = Database.LocationId + "1";
                                    dtBilladjest.Rows[i]["Nid"] = 1;
                                    dtBilladjest.Rows[i]["LocationId"] = Database.LocationId;

                                }
                                else
                                {
                                    DataTable dtAcid = new DataTable();
                                    Database.GetSqlData("select max(Nid) as Nid from BillAdjest where locationid='" + Database.LocationId + "'", dtAcid);
                                    int Nid = int.Parse(dtAcid.Rows[0][0].ToString());
                                    dtBilladjest.Rows[i]["id"] = Database.LocationId + (Nid + 1);
                                    dtBilladjest.Rows[i]["Nid"] = (Nid + 1);
                                    dtBilladjest.Rows[i]["LocationId"] = Database.LocationId;

                                }
                            }
                            else
                            {

                                //int rowindex = i;
                                //int Nid = int.Parse(dtBilladjest.Rows[rowindex]["Nid"].ToString());
                                ////   int Nid = int.Parse(dtBilladjest.Rows[0]["Nid"].ToString());
                                //dtBilladjest.Rows[i]["id"] = Database.LocationId + (Nid + 1);
                                //dtBilladjest.Rows[i]["Nid"] = (Nid + 1);
                                //dtBilladjest.Rows[i]["LocationId"] = Database.LocationId;

                                DataTable dtAcid = new DataTable();
                                Database.GetSqlData("select max(Nid) as Nid from BillAdjest where locationid='" + Database.LocationId + "'", dtAcid);
                                int Nid = 0;
                                if (i == 0)
                                {
                                    Nid = int.Parse(dtAcid.Rows[0][0].ToString());
                                }
                                else
                                {
                                    Nid = int.Parse(dtBilladjest.Rows[i - 1]["Nid"].ToString());
                                }
                                dtBilladjest.Rows[i]["id"] = Database.LocationId + (Nid + 1);
                                dtBilladjest.Rows[i]["Nid"] = (Nid + 1);
                                dtBilladjest.Rows[i]["LocationId"] = Database.LocationId;

                            }

                            dtBilladjest.Rows[i]["Vi_id"] = vid;
                            dtBilladjest.Rows[i]["A"] = A;
                            dtBilladjest.Rows[i]["B"] = B;
                            dtBilladjest.Rows[i]["AB"] = true;

                            //     dtBilladjest.Rows[i]["Amount"] = double.Parse(ansGridView1.Rows[i].Cells["Amount"].Value.ToString());

                            dtBilladjest.Rows[i].AcceptChanges();
                            dtBilladjest.Rows[i].SetAdded();
                        }
                   
                }


            }


          
    //        Database.SaveData(dtBilladjest);


















            //dtBilladjest = new DataTable("Billadjest");
            //Database.GetSqlData("Select * from Billadjest where Vi_id='" + vid + "'", dtBilladjest);

            //for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
            //{

            //    if (funs.Select_MainAccTypeName(ansGridView1.Rows[i].Cells["acc"].Value.ToString()) == "SUNDRY DEBTORS" || funs.Select_MainAccTypeName(ansGridView1.Rows[i].Cells["acc"].Value.ToString()) == "SUNDRY CREDITORS")
            //    {
            //        if (dtBilladjest.Rows.Count == 0)
            //        {
            //            dtBilladjest.Rows.Add();
            //            DataTable dtCount = new DataTable();
            //            Database.GetSqlData("select count(*) from BillAdjest where locationid='" + Database.LocationId + "'", dtCount);
            //            if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
            //            {
            //                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["id"] = Database.LocationId + "1";
            //                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Nid"] = 1;
            //                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["LocationId"] = Database.LocationId;

            //            }
            //            else
            //            {
            //                DataTable dtAcid = new DataTable();
            //                Database.GetSqlData("select max(Nid) as Nid from BillAdjest where locationid='" + Database.LocationId + "'", dtAcid);
            //                int Nid = int.Parse(dtAcid.Rows[0][0].ToString());
            //                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["id"] = Database.LocationId + (Nid + 1);
            //                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Nid"] = (Nid + 1);
            //                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["LocationId"] = Database.LocationId;

            //            }
            //        }
            //        else
            //        {
            //            dtBilladjest.Rows.Add();
            //            int rowindex = dtBilladjest.Rows.Count - 2;
            //            int Nid = int.Parse(dtBilladjest.Rows[rowindex]["Nid"].ToString());
            //         //   int Nid = int.Parse(dtBilladjest.Rows[0]["Nid"].ToString());
            //            dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["id"] = Database.LocationId + (Nid + 1);
            //            dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Nid"] = (Nid + 1);
            //            dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["LocationId"] = Database.LocationId;

                      
            //        }



            //        dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Ac_id"] = funs.Select_ac_id(ansGridView1.Rows[i].Cells["acc"].Value.ToString());
            //        dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Vi_id"] = vid;

             

            //        dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["A"] = A;
            //        dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["B"] = B;
            //        dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["AB"] = true;


            //        if (ansGridView1.Rows[i].Cells["reffno"].Value == null || ansGridView1.Rows[i].Cells["reffno"].Value.ToString() == "<New Refference>")
            //        {
            //            dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Reff_id"] = vid;
            //        }
            //        else
            //        {
            //            dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Reff_id"] = IsDocumentNumber(ansGridView1.Rows[i].Cells["reffno"].Value.ToString());
            //        }
            //        if (recpay == "Receipt")
            //        {
            //            dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Amount"] = -1 * double.Parse(ansGridView1.Rows[i].Cells["Amount"].Value.ToString());
            //        }
            //        else
            //        {
            //            dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Amount"] = double.Parse(ansGridView1.Rows[i].Cells["Amount"].Value.ToString());
            //        }
            //    }

            //}
           // Database.SaveData(dtBilladjest);







            funs.ShowBalloonTip("Saved", "Saved Successfully");

            if (print == true)
            {
                if (Feature.Available("Ask Copies") == "No")
                {
                    OtherReport rpt = new OtherReport();
                    DataTable dtprintcopy = new DataTable();
                    Database.GetSqlData("Select printcopy from Vouchertype where Vt_id='" + vtid +"'", dtprintcopy);
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

        private void SetVno()
        {
            if (vtid == "" || vtid == null || (vno != 0 && vid != "") || f12used == true)
            {
                return;
            }
            vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
            label10.Text = vno.ToString();

        }

        private void calcTot()
        {
            double total = 0.0;
            for (int i = 0; i < ansGridView1.RowCount - 1; i++)
            {
                total += double.Parse(ansGridView1.Rows[i].Cells["Amount"].Value.ToString());
            }
            double tot = 0;
            if (dtItemCharges.Rows.Count > 0)
            {
                tot = double.Parse(dtItemCharges.Compute("sum(Camount)", "").ToString());
            }
            label4.Text = (total - tot).ToString();
        }
        
        private void ansGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView1.CurrentCell.OwningColumn.Name == "Amount")
            {
                try
                {
                    double amt1 = double.Parse(ansGridView1.Rows[e.RowIndex].Cells["Amount"].Value.ToString());
                    if (double.Parse(ansGridView1.CurrentCell.Value.ToString()) > 0 && itemCharges == true)
                    {
                        frmItemCharges frm = new frmItemCharges(dtItemCharges, vid, ansGridView1.CurrentCell.OwningRow.Index + 1, "select [name] from charges where Ac_id<>0", double.Parse(ansGridView1.CurrentCell.OwningRow.Cells["Amount"].Value.ToString()), "", 0);
                        frm.ShowDialog(this);
                        dtItemCharges = frm.gdt;
                    }
                    calcTot();
                }
                catch (Exception ex)
                {
                    ansGridView1.Rows[e.RowIndex].Cells["Amount"].Value = "0.00";
                    return;
                }
            }
        }
        private void delbilladjest(int itemsr)
        {
            DataRow[] drow;
            drow = dtBilladjest.Select("Itemsr=" + itemsr + "  and Vi_id='" + vid+"'");


            DataTable tdt = new DataTable();
            if (drow.GetLength(0) > 0)
            {
                tdt = drow.CopyToDataTable();
            }
            for (int i = dtBilladjest.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = dtBilladjest.Rows[i];
                if (int.Parse(dr["Itemsr"].ToString()) == itemsr && dr["vi_id"].ToString() == vid)
                    dr.Delete();
            }
            dtBilladjest.AcceptChanges();
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

            
            if (vid != "")
            {
                //string st = "TOP (" + Feature.Available("Voucher Editing Power") + ")";
                //if (st.ToUpper() == "TOP (UNLIMITED)")
                //{
                //    st = "";
                //}
                //DataTable dt = new DataTable();

                //Database.GetSqlData("SELECT " + st + " VOUCHERINFO.Vi_id FROM VOUCHERINFO LEFT OUTER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (VOUCHERTYPE.Type = '" + recpay + "') AND (VOUCHERINFO.Branch_id = '" + Database.BranchId + "') ORDER BY VOUCHERINFO.Nid DESC", dt);
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
                permission = funs.GetPermissionKey(recpay);
               
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
                            if (Database.user_id != user_id)
                            {
                                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;

                            }
                            else
                            {


                                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
                            }
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
                // dtsidefill.Rows[0]["Visible"] = true;

                permission = funs.GetPermissionKey(recpay);
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

            }


            //print
            dtsidefill.Rows.Add();
            dtsidefill.Rows[1]["Name"] = "print";
            dtsidefill.Rows[1]["DisplayName"] = "Print";
            dtsidefill.Rows[1]["ShortcutKey"] = "^P";
            if (vid != "")
            {
                //if (EditDelete == true)
                //{
                //    dtsidefill.Rows[1]["Visible"] = true;
                //}
                //else
                //{
                //    dtsidefill.Rows[1]["Visible"] = false;
                //}
                permission = funs.GetPermissionKey(recpay);

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
                            if (Database.user_id != user_id)
                            {
                                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;

                            }
                            else
                            {
                                dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
                            }
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
                // dtsidefill.Rows[0]["Visible"] = true;

                permission = funs.GetPermissionKey(recpay);
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

            //chargeswindow
            dtsidefill.Rows.Add();
            dtsidefill.Rows[2]["Name"] = "charges";
            dtsidefill.Rows[2]["DisplayName"] = "Charges Window";
            dtsidefill.Rows[2]["ShortcutKey"] = "";
            if (recpay == "Contra")
            {
                dtsidefill.Rows[2]["Visible"] = false;
            }
            else
            {
                dtsidefill.Rows[2]["Visible"] = true;
            }


            //delete
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "delete";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Delete";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "^D";
            permission = funs.GetPermissionKey(recpay);
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

                    string user_id = Database.GetScalarText("Select User_id from Voucherinfo where vi_id='" + vid+ "'");
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
            else if (name == "delete")
            {
                if (vid != "")
                {
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
            else if (name == "charges")
            {
                if (itemCharges == false)
                {
                    itemCharges = true;
                }
                else
                {
                    itemCharges = false;
                }
                try
                {
                    double amt1 = double.Parse(ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Amount"].Value.ToString());
                    if (double.Parse(ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Amount"].Value.ToString()) > 0 && itemCharges == true)
                    {
                        frmItemCharges frm = new frmItemCharges(dtItemCharges, vid, ansGridView1.CurrentCell.OwningRow.Index + 1, "select [name] from charges where Ac_id<>0", double.Parse(ansGridView1.CurrentCell.OwningRow.Cells["Amount"].Value.ToString()), "", 0);
                        frm.ShowDialog(this);
                        dtItemCharges = frm.gdt;
                    }
                    calcTot();
                }
                catch (Exception ex)
                {
                    ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Amount"].Value = "0.00";
                    return;
                }
            }

            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
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

            string cash = "";
            cash = funs.Select_vt_Cashtran(vtid);

            if (ansGridView1.CurrentCell.OwningColumn.Name == "acc")
            {
                string wheresrt = "Not (Path  LIKE '1;3;%'  or Path  like '1;28;%')";
              
                strCombo = funs.GetStrCombonew(wheresrt,"1=1");
                if (cash == "Allowed")
                {
                    wheresrt = " not Path like '1;28;%'  ";
           
                       strCombo = funs.GetStrCombonew(wheresrt,"1=1");
                }
                else if (cash == "Not Allowed")
                {
                    wheresrt = " not (Path  LIKE '1;3;%'  or Path  like '1;28;%')  ";
                    strCombo = funs.GetStrCombonew(wheresrt, "1=1");
                  
                }
                else if (cash == "Only Allowed")
                {
                    wheresrt = "Not Path LIKE '1;3;%'";
                    strCombo = funs.GetStrCombonew(wheresrt, "1=1");
                   
                }
                if (recpay == "Contra")
                {
                    wheresrt = " Path  LIKE '1;3;%'  or Path  like '1;2;%'  ";
                    strCombo = funs.GetStrCombonew(wheresrt, "1=1");
                    
                }
                ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
                if (ansGridView1.CurrentCell.Value != null)
                {

                    delbilladjest(int.Parse(ansGridView1.CurrentRow.Cells["sno"].Value.ToString()));

                }
                SendKeys.Send("{tab}");
            }




            //if (ansGridView1.CurrentCell.OwningColumn.Name == "reffno")
            //{
            //    if (funs.Select_MainAccTypeName(ansGridView1.CurrentRow.Cells["acc"].Value.ToString()).ToUpper() == "SUNDRY DEBTORS" || funs.Select_MainAccTypeName(ansGridView1.CurrentRow.Cells["acc"].Value.ToString()).ToUpper() == "SUNDRY CREDITORS")
            //    {

            //        DataTable dtcombo = new DataTable();
            //       // strCombo = "select distinct '<New Refference>' as ReffNo, 0.00  & ' '  as Amt  from Account union all  select iif(DocNumber='','Opening',DocNumber) as ReffNo,amt  & ' '  as Amt from(SELECT VOUCHERTYPE.Short & ' ' & Format(Voucherinfo.Vdate,'yyyymmdd' & ' ' & VOUCHERINFO.Vnumber) AS DocNumber, Sum(BILLADJEST.Amount) AS amt FROM (BILLADJEST LEFT JOIN VOUCHERINFO ON BILLADJEST.Reff_id = VOUCHERINFO.Vi_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((BILLADJEST.Ac_id)=" + funs.Select_ac_id(ansGridView1.CurrentRow.Cells["acc"].Value.ToString()) + ")) GROUP BY VOUCHERTYPE.Short & ' ' & Format(Voucherinfo.Vdate,'yyyymmdd' & ' ' & VOUCHERINFO.Vnumber) HAVING (((Sum(BILLADJEST.Amount))<>0))) as res";
            //        strCombo = "select distinct '<New Refference>' as ReffNo, CAST(0.00 AS nvarchar(10))  as Amt  from Account  union all  select Case when  DocNumber is null then 'Opening' else DocNumber  End as ReffNo, CAST(amt AS nvarchar(10))  as Amt from(SELECT VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)) AS DocNumber, Sum(BILLADJEST.Amount) AS amt FROM (BILLADJEST LEFT JOIN VOUCHERINFO ON BILLADJEST.Reff_id = VOUCHERINFO.Vi_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((BILLADJEST.Ac_id)='" + funs.Select_ac_id(ansGridView1.CurrentRow.Cells["acc"].Value.ToString()) + "')   And BILLADJEST." + Database.BMode + "='true' ) GROUP BY VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)) HAVING (((Sum(BILLADJEST.Amount))<>0))) as res";
            //        Database.GetSqlData(strCombo, dtcombo);

            //        ansGridView1.CurrentCell.Value = SelectCombo.ComboDt(this, dtcombo, 1);
            //        dtcombo = new DataTable();
            //        //strCombo = "select distinct '<New Refference>' as ReffNo, 0 as Amt from Account union all  select iif(DocNumber='','Opening',DocNumber) as ReffNo,amt from(SELECT VOUCHERTYPE.Short & ' ' & Format(Voucherinfo.Vdate,'yyyymmdd' & ' ' & VOUCHERINFO.Vnumber) AS DocNumber, Sum(BILLADJEST.Amount) AS amt FROM (BILLADJEST LEFT JOIN VOUCHERINFO ON BILLADJEST.Reff_id = VOUCHERINFO.Vi_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((BILLADJEST.Ac_id)=" + funs.Select_ac_id(ansGridView1.CurrentRow.Cells["acc"].Value.ToString()) + ")) GROUP BY VOUCHERTYPE.Short & ' ' & Format(Voucherinfo.Vdate,'yyyymmdd' & ' ' & VOUCHERINFO.Vnumber) HAVING (((Sum(BILLADJEST.Amount))<>0))) as res";
            //        strCombo = "select distinct '<New Refference>' as ReffNo, 0 as Amt  from Account  union all  select Case when  DocNumber is null then 'Opening' else DocNumber  End as ReffNo, amt from(SELECT VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)) AS DocNumber, Sum(BILLADJEST.Amount) AS amt FROM (BILLADJEST LEFT JOIN VOUCHERINFO ON BILLADJEST.Reff_id = VOUCHERINFO.Vi_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((BILLADJEST.Ac_id)='" + funs.Select_ac_id(ansGridView1.CurrentRow.Cells["acc"].Value.ToString()) + "')   And BILLADJEST." + Database.BMode + "='true' ) GROUP BY VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)) HAVING (((Sum(BILLADJEST.Amount))<>0))) as res";
            //        Database.GetSqlData(strCombo, dtcombo);

            //        double amount = 0;
            //        if (dtcombo.Select("ReffNo='" + ansGridView1.CurrentCell.Value + "'").Length > 0)
            //        {
            //            amount = double.Parse(dtcombo.Compute("sum(amt)", " ReffNo='" + ansGridView1.CurrentCell.Value + "'").ToString());
            //        }
            //    }

            //}
        }

        private void ansGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (ansGridView1.CurrentCell == null)
            {
                return;
            }

            if (e.KeyCode == Keys.Delete)
            {
                if (ansGridView1.CurrentRow.Index == ansGridView1.Rows.Count - 1)
                {
                    for (int i = 1; i < ansGridView1.Columns.Count; i++)
                    {
                        ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells[i].Value = "";
                    }
                    return;
                }
                else
                {
                    int rindex = ansGridView1.CurrentRow.Index;
                    ansGridView1.Rows.RemoveAt(ansGridView1.CurrentRow.Index);
                    for (int i = 0; i < ansGridView1.Rows.Count; i++)
                    {
                        ansGridView1.Rows[i].Cells["sno"].Value = (i + 1);
                    }
                    for (int i = 0; i < dtBilladjest.Rows.Count; i++)
                    {
                        if (dtBilladjest.Rows[i].RowState.ToString() == "Deleted" || int.Parse(dtBilladjest.Rows[i]["Itemsr"].ToString()) < rindex + 1)
                        {

                        }
                        else if (int.Parse(dtBilladjest.Rows[i]["Itemsr"].ToString()) == rindex + 1)
                        {
                            dtBilladjest.Rows[i].Delete();

                        }
                        else if (int.Parse(dtBilladjest.Rows[i]["Itemsr"].ToString()) > rindex + 1)
                        {
                            dtBilladjest.Rows[i]["Itemsr"] = int.Parse(dtBilladjest.Rows[i]["Itemsr"].ToString()) - 1;
                        }
                    }
                    calcTot();
                    return;
                }
            }

            ansGridView1.CurrentCell.OwningRow.Cells["sno"].Value = ansGridView1.CurrentCell.OwningRow.Index + 1;
            if (ansGridView1.CurrentCell.OwningColumn.Name == "instrumentdt")
            {
                if (ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Amount"].Value == null)
                {
                    if (ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Amount"].Value == null || ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Amount"].Value.ToString() == "")
                    {
                        SendKeys.Send("{tab}");
                    }
                }
            }

            if (ansGridView1.CurrentCell.OwningColumn.Name == "acc")
            {
                if (e.Control && e.KeyCode == Keys.A)
                {
                    if (ansGridView1.CurrentCell.Value.ToString() != "")
                    {
                        ansGridView1.CurrentCell.Value = funs.EditAccount(ansGridView1.CurrentCell.Value.ToString());
                    }
                }
                else if (e.Control && e.KeyCode == Keys.C)
                {
                    ansGridView1.CurrentCell.Value = funs.AddAccount();
                }
            }



            if (e.Alt == true && e.Control == true && e.KeyCode == Keys.B)
            {
                string columnname = "";
                if (recpay=="Receipt")
                {
                    columnname = "Receipt"; 
                }
               
                else
                {
                    columnname = "Payment";
                }



                if (funs.Select_MainAccTypeName(ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["acc"].Value.ToString()) == "SUNDRY DEBTORS" || funs.Select_MainAccTypeName(ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["acc"].Value.ToString()) == "SUNDRY CREDITORS")
                {

                    int rnum = ansGridView1.CurrentRow.Index + 1;

                    if (columnname == "Payment")
                    {
                        if (dtBilladjest.Select("Itemsr=" + rnum).Length != 0)
                        {
                            double adtotalamt = double.Parse(dtBilladjest.Compute("sum(Amount)", "Itemsr=" + rnum).ToString());
                            if (-1*adtotalamt != -1 * double.Parse(ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Amount"].Value.ToString()))
                            {
                                delbilladjest(rnum);
                            }
                        }
                        if (double.Parse(ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Amount"].Value.ToString()) != 0)
                        {
                            frm_adjust frm = new frm_adjust(dtBilladjest, rnum, "Payment", vid, double.Parse(ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Amount"].Value.ToString()), funs.Select_ac_id(ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["acc"].Value.ToString()));
                            frm.ShowDialog();
                            dtBilladjest = frm.gdt;
                            SendKeys.Send("{Enter}");
                            SendKeys.Send("{Enter}");
                        }
                    }
                    else if (columnname == "Receipt")
                    {
                        if (dtBilladjest.Select("Itemsr=" + rnum).Length != 0)
                        {
                            double adtotalamt = double.Parse(dtBilladjest.Compute("sum(Amount)", "Itemsr=" + rnum).ToString());
                            if (adtotalamt != -1*double.Parse(ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Amount"].Value.ToString()))
                            {
                                delbilladjest(rnum);
                            }
                        }

                        if (double.Parse(ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Amount"].Value.ToString()) != 0)
                        {
                            frm_adjust frm = new frm_adjust(dtBilladjest, rnum, "Receipt", vid, double.Parse(ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Amount"].Value.ToString()), funs.Select_ac_id(ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["acc"].Value.ToString()));
                            frm.ShowDialog();
                            dtBilladjest = frm.gdt;
                            SendKeys.Send("{Enter}");
                        }
                    }

                }

            }


























        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            string wheresrt = "";
            //if (Database.IsKacha == false)
            //{
            //    wheresrt = "(Path LIKE '1;3;%')  OR   (Path LIKE '1;2;%')";
            //}
            //else
            //{
                bool a = false;
                a = Database.GetScalarBool("Select " + Database.BMode + " from Vouchertype where Name='" + textBox3.Text + "'");
                {
                    if (a == true)
                    {
                        wheresrt = "(Path LIKE '1;3;%')  OR   (Path LIKE '1;2;%')";
                    }
                    else if (a == false)
                    {
                        wheresrt = "(Path LIKE '1;3;%' ) OR   (Path LIKE '1;2;%')";
                    }
                }
           // }
            //strCombo = funs.GetStrCombo(wheresrt);
            strCombo = funs.GetStrCombonew(wheresrt, "1=1");
            textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
        }

        private void ansGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            ansGridView1.CurrentCell.Value = 0;
        }

        private void lastVoucher()
        {
            DataTable dtLastTran = new DataTable();
            if (this.Text != "")
            {
                Database.GetSqlData("SELECT temp.[name], temp.vnumber, temp.vdate, VOUCHERINFO.Totalamount FROM (SELECT VOUCHERTYPE.Name, Max(VOUCHERINFO.Vnumber) AS Vnumber, Max(VOUCHERINFO.Vdate) AS Vdate FROM VOUCHERINFO INNER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERTYPE.Name)='" + this.Text + "' )) GROUP BY VOUCHERTYPE.Name)  AS temp INNER JOIN (VOUCHERINFO INNER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) ON (temp.Vdate = VOUCHERINFO.Vdate) AND (temp.Vnumber = VOUCHERINFO.Vnumber) AND (temp.Name = VOUCHERTYPE.Name)", dtLastTran);
            }
            frm_main.clearDisplay2();
            frm_main.dtDisplay2.Columns.Add("Item");
            frm_main.dtDisplay2.Columns.Add("Description");

            if (dtLastTran.Rows.Count > 0)
            {
                frm_main.dtDisplay2.Rows.Add();
                frm_main.dtDisplay2.Rows[0]["Item"] = "Type";
                frm_main.dtDisplay2.Rows[0]["Description"] = dtLastTran.Rows[0]["name"];
                frm_main.dtDisplay2.Rows.Add();
                frm_main.dtDisplay2.Rows[1]["Item"] = "Voucher No.";
                frm_main.dtDisplay2.Rows[1]["Description"] = dtLastTran.Rows[0]["vnumber"];
                frm_main.dtDisplay2.Rows.Add();
                frm_main.dtDisplay2.Rows[2]["Item"] = "Voucher Date";
                frm_main.dtDisplay2.Rows[2]["Description"] = DateTime.Parse(dtLastTran.Rows[0]["vdate"].ToString()).ToString("dd-MMM-yyyy");
                frm_main.dtDisplay2.Rows.Add();
                frm_main.dtDisplay2.Rows[3]["Item"] = "Total Amount";
                frm_main.dtDisplay2.Rows[3]["Description"] = dtLastTran.Rows[0]["Totalamount"];
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            string wheresrt = "";
            if (Database.IsKacha == false)
            {
                wheresrt = "in(3,2,31)";
            }
            else
            {
                bool a = false;
                a = Database.GetScalarBool("Select A from Vouchertype where Name='" + textBox3.Text + "'");
                {
                    if (a == true)
                    {
                        wheresrt = "in(3)";
                    }
                    else if (a == false)
                    {
                        wheresrt = "in(3,2,31)";
                    }
                }
            }
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox1.Text != "")
                {
                    textBox1.Text = funs.EditAccount(textBox1.Text, wheresrt);
                }
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                textBox1.Text = funs.AddAccount(wheresrt);
            }
        }

        //private string VouchertypeId(String str)
        //{
        //    string vouTypId = funs.Select_vt_id_vnm(str);
        //    return vouTypId;
        //}

        private void ansGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView1.CurrentCell.OwningColumn.Name == "sno")
            {
                ansGridView1.Rows[e.RowIndex].Cells["sno"].Value = e.RowIndex + 1;
                SendKeys.Send("{tab}");
            }
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.Alt == true && e.KeyCode == Keys.N)
            {
                textBox2.ReadOnly = true;
                DataTable dtcombo = new DataTable();
                strCombo = "Select Distinct(Narr) from Voucherinfo where Narr<>' ' order by Narr";
                Database.GetSqlData(strCombo, dtcombo);
               textBox2.Text = SelectCombo.ComboDt(this, dtcombo, 0);
               textBox2.ReadOnly = false;
               SendKeys.Send("{End}");
            }
           
            else
            {
                SelectCombo.IsEnter(this, e.KeyCode);
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            label4.Text = "0";
            ansGridView1.Rows.Clear();
            dateTimePicker1.Focus();
            vid = "";
            vno = 0;
            label10.Text = vno.ToString();
            dtVoucherInfo.Rows.Clear();
            dtVoucheractotal.Rows.Clear();
            cmbVouTyp = "";
            cashac_id = "0";
            LoadData("", gfrmCaption);
        }

        private bool validate()
        {         
            ansGridView1.EndEdit();

            if (vid != "")
            {               
                int count = Database.GetScalarInt("SELECT Count([Vnumber]) AS Expr1 FROM VOUCHERINFO WHERE (((VOUCHERINFO.Vt_id)='" + vtid + "') AND ((VOUCHERINFO.Vi_id)<>'" + vid + "') AND ((VOUCHERINFO.Vnumber)=" + vno + ") AND ((VOUCHERINFO.Vdate)="+ access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat)+ access_sql.Hash+"))");
                if(count!=0)
                {
                    vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
                }
            }
            if (label4.Text == "0")
            {
                MessageBox.Show("Please enter some value");
                textBox1.Focus();
                return false;
            }
            if (textBox3.Text == "")
            {
                MessageBox.Show("Please enter Vouchertype");
                textBox3.BackColor = Color.Aqua;
                textBox3.Focus();
                return false;
            }
            if (textBox1.Text == "")
            {
                textBox1.BackColor = Color.Aqua;
                textBox1.Focus();
                return false;
            }
            if (funs.Select_ac_id(textBox1.Text) == "" || funs.Select_ac_id(textBox1.Text) == "")
            {
                textBox1.BackColor = Color.Aqua;
                textBox1.Focus();
                MessageBox.Show("Enter Valid Account Name");
                return false;
            }

            for (int i = 0; i < ansGridView1.RowCount - 1; i++)
            {
                if (ansGridView1.Rows[i].Cells["Acc"].Value.ToString() == "")
                {
                    ansGridView1.CurrentCell = ansGridView1["Acc", ansGridView1.CurrentCell.RowIndex];
                    MessageBox.Show("Enter Account Name");
                    return false;
                }
                if (funs.Select_ac_id(ansGridView1.Rows[i].Cells["Acc"].Value.ToString()) == "" || funs.Select_ac_id(ansGridView1.Rows[i].Cells["Acc"].Value.ToString()) == "")
                {
                    ansGridView1.CurrentCell = ansGridView1["Acc", ansGridView1.CurrentCell.RowIndex];
                    MessageBox.Show("Enter Valid Account Name");
                    return false;
                }
            }
            return true;
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (Database.IsKacha == false)
            //{
                cmbVouTyp = "select [name] from vouchertype where active=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " and type='" + recpay + "' and "+Database.BMode+"="+access_sql.Singlequote+"true"+access_sql.Singlequote+"";
            //}
            //else
            //{
            //    cmbVouTyp = "select [name] from vouchertype where active=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " and type='" + recpay + "' and B=" + access_sql.Singlequote + "true" + access_sql.Singlequote + "";
            //}
            textBox3.Text = SelectCombo.ComboKeypress(this, e.KeyChar, cmbVouTyp, e.KeyChar.ToString(), 0);
            vtid = funs.Select_vt_id_vnm(textBox3.Text);
            textBox2.Text = funs.Select_vt_Narrtemplate(vtid);
            if (textBox3.Text != "")
            {
                textBox1.Enabled = true;
            }
            SetVno();
            label10.Text = vno.ToString();
        }

        private void Display()
        {
            DataTable dtvt = new DataTable();
            string cmbVouTyp3 = "";
            //if (Database.IsKacha == false)
            //{
            cmbVouTyp3 = " and " + Database.BMode + "=" + access_sql.Singlequote + "True" + access_sql.Singlequote;
            //}
            //else
            //{
            //    cmbVouTyp3 = " and B="+access_sql.Singlequote +"True" +access_sql.Singlequote;
            //}

            cmbVouTyp = "select [name] from vouchertype where active=" + access_sql.Singlequote + "true" + access_sql.Singlequote +" and type='" + recpay + "'";
            cmbVouTyp = cmbVouTyp + cmbVouTyp3;
            Database.GetSqlData(cmbVouTyp, dtvt);
            if (dtvt.Rows.Count == 1)
            {
                textBox3.Text = dtvt.Rows[0]["name"].ToString();
                vtid = funs.Select_vt_id_vnm(textBox3.Text);
                textBox3.Enabled = false;
            }
            else
            {
                textBox3.Enabled = true;   
            }
            if (recpay == "Contra")
            {
                groupBox3.Text = "Credit Account";
                ansGridView1.Columns["acc"].HeaderText = "Debit Account Name";
                ansGridView1.Columns["instrumentno"].Visible = false;
                ansGridView1.Columns["instrumentdt"].Visible = false;
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

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            SetVno();
        }
    }
}
