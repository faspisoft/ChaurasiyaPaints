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
    public partial class frmJournal : Form
    {
        public bool gresave = false;
        DataTable dtVoucherInfo;
        DataTable dtVoucheractotal;
        DataTable dtJournal;
        DataTable dtBilladjest;
        public String cmdmode;
        Boolean generateVno = false;
        int vno = 0;
        string vtid = "", vid = "";
        string Prelocationid = "";
        OleDbCommand cmd;
        public String gFrmCaption;
        DateTime chkDt = new DateTime();       
        DataTable dtFid = new DataTable();
        DataTable dtUid = new DataTable();
        int fid, uid;
        List<UsersFeature> permission;
        String strCombo;
        Boolean EditDelete = false;
        public String gStr = "";
        string cmbVouTyp = "";
        string typ = "";
        
        public frmJournal()
        {
            InitializeComponent();
            dateTimePicker1.MinDate = Database.stDate;
            dateTimePicker1.MaxDate = Database.ldate;
            dateTimePicker1.CustomFormat = Database.dformat;
            dateTimePicker1.Value = Database.ldate;
        }

        private void frmJournal_Load(object sender, EventArgs e)
        {           
        }

        private void ansGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView1.CurrentCell.OwningColumn.Name == "sno")
            {
                ansGridView1.Rows[e.RowIndex].Cells["sno"].Value = e.RowIndex + 1;
                SendKeys.Send("{tab}");
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
                        if (save(false) == true)
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
                            if (save(false) == true)
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


                            if (double.Parse(ansGridView1.Rows[i].Cells["dr"].Value.ToString()) > 0)
                            {
                                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Amount"] = double.Parse(ansGridView1.Rows[i].Cells["dr"].Value.ToString());
                            }
                            else
                            {
                                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Amount"] = -1 * double.Parse(ansGridView1.Rows[i].Cells["cr"].Value.ToString());
                            }
                            dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["A"] = A;
                            dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["B"] = B;
                            dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["AB"] = true;
                        }

                        //    }

                    }

                    Database.SaveData(dtBilladjest);



                    funs.ShowBalloonTip("Saved", "Saved Successfully");


                }
                Database.CommitTran();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Voucher Not Saved, Due To An Exception", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Database.RollbackTran();
            }
        }

        private void frmJournal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                frmHelp frm = new frmHelp();
                frm.ShowDialog(this);
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                if (vid == "")
                {
                    if (Validate() == true)
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
                   
                        if (Validate() == true)
                        {
                            permission = funs.GetPermissionKey("Journal");

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
                    if (Validate() == true)
                    {
                       // SaveMethod(true);
                        UsersFeature ob = permission.Where(w => w.FeatureName == "Create").FirstOrDefault();
                        if (ob != null && gStr == "" && ob.SelectedValue == "Allowed")
                        {
                            SaveMethod(true);
                        }
                    }
                }
                else
                {
                    
                        if (Validate() == true)
                        {
                            permission = funs.GetPermissionKey("Journal");

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

                        permission = funs.GetPermissionKey("Journal");
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
                            Delete();

                            Database.CommitTran();
                            this.Close();
                            this.Dispose();
                        }

                    }
                
            }
            else if (e.KeyCode == Keys.Escape)
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
                    MessageBox.Show("Invalid password");
                }
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

            if (vid != "")
            {
                //string st = "TOP (" + Feature.Available("Voucher Editing Power") + ")";
                //if (st.ToUpper() == "TOP (UNLIMITED)")
                //{
                //    st = "";
                //}
                //DataTable dt = new DataTable();

                //Database.GetSqlData("SELECT " + st + " VOUCHERINFO.Vi_id FROM VOUCHERINFO LEFT OUTER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (VOUCHERTYPE.Type = 'Journal') AND (VOUCHERINFO.Branch_id = '" + Database.BranchId + "') ORDER BY VOUCHERINFO.Nid DESC", dt);

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
                permission = funs.GetPermissionKey("Journal");

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
               // dtsidefill.Rows[0]["Visible"] = true;
                permission = funs.GetPermissionKey("Journal");
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
                permission = funs.GetPermissionKey("Journal");

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
               //dtsidefill.Rows[1]["Visible"] = true;
                permission = funs.GetPermissionKey("Journal");
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
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "delete";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Delete";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "^D";
            permission = funs.GetPermissionKey(typ);
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
                if (Validate() == true)
                {                    
                    SaveMethod(false);
                }
            }
            else if (name == "print")
            {
                if (Validate() == true)
                {
                    SaveMethod(true);
                }
            }
            else if (name == "quit")
            {
                this.Close();
                this.Dispose();
            }
        }

        private void clear()
        {
            textBox2.Text = "";
            label3.Text = "0";
            label4.Text = "0";
            ansGridView1.Rows.Clear();
            dateTimePicker1.Focus();
            vno = 0;
            vid = "";
            vtid = "";
            dtVoucherInfo.Rows.Clear();
            dtVoucheractotal.Rows.Clear();
            cmbVouTyp = "";
            label10.Text = vno.ToString();
            LoadData("",gFrmCaption);
        }

        private void DisplayData()
        {
            dtVoucherInfo = new DataTable("Voucherinfo");
            Database.GetSqlData("select * from voucherinfo where vi_id='" + vid + "' ", dtVoucherInfo);

            if (dtVoucherInfo.Rows.Count > 0)
            {
                Prelocationid = dtVoucherInfo.Rows[0]["locationid"].ToString();
                textBox2.Text = dtVoucherInfo.Rows[0]["Narr"].ToString();
                dateTimePicker1.Value = DateTime.Parse(dtVoucherInfo.Rows[0]["Vdate"].ToString());
                vno = int.Parse(dtVoucherInfo.Rows[0]["Vnumber"].ToString());
                label10.Text = vno.ToString();
                chkDt = DateTime.Parse(dtVoucherInfo.Rows[0]["Vdate"].ToString());
                
                textBox3.Text = funs.Select_vt_nm(dtVoucherInfo.Rows[0]["Vt_id"].ToString());
                vtid = dtVoucherInfo.Rows[0]["Vt_id"].ToString();
                
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
                //for (int j = 0; j < dt.Rows.Count; j++)
                //{
                //    if (dt.Rows[j]["Vi_id"].ToString() == vid)
                //    {
                //        EditDelete = true;
                //        break;
                //    }
                //}

            }

            dtVoucheractotal = new DataTable("voucheractotal");
            Database.GetSqlData("select * from voucheractotal where vi_id='" + vid + "' ", dtVoucheractotal);
            for (int i = 0; i < dtVoucheractotal.Rows.Count; i++)
            {
                ansGridView1.Rows.Add();
                ansGridView1.Rows[i].Cells["sno"].Value = dtVoucheractotal.Rows[i]["Srno"];
                ansGridView1.Rows[i].Cells["acc"].Value = funs.Select_ac_nm(dtVoucheractotal.Rows[i]["accid"].ToString());
                ansGridView1.Rows[i].Cells["dr"].Value = funs.DecimalPoint(dtVoucheractotal.Rows[i]["Amount"]);
                ansGridView1.Rows[i].Cells["cr"].Value = funs.DecimalPoint(dtVoucheractotal.Rows[i]["cam"]);

               

            }

            dtJournal = new DataTable("Journal");
            Database.GetSqlData("select * from Journal where vi_id='" + vid + "' ", dtJournal);


            dtBilladjest = new DataTable("BillAdjest");
            Database.GetSqlData("select * from BillAdjest where vi_id='" + vid + "' order by ItemSr,AdjustSr", dtBilladjest);

            foreach (DataGridViewColumn column in ansGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            ansGridView1.Columns["dr"].CellTemplate.ValueType = typeof(double);
            ansGridView1.Columns["cr"].CellTemplate.ValueType = typeof(double);
            
            //Database.CloseConnection();
        }

        public void LoadData(String str, String frmCaption)
        {
            vid = str;
            gStr = str;
            //vtid = funs.Select_vt_id_vnm(frmCaption);
            gFrmCaption = frmCaption;
            this.Text = frmCaption;
            ansGridView1.Rows[0].Cells["dr"].Value = "0.00";
            ansGridView1.Rows[0].Cells["cr"].Value = "0.00";
            Display();
            dtBilladjest = new DataTable("BillAdjest");
            dtBilladjest.Columns.Add("Vi_id", typeof(string));
            dtBilladjest.Columns.Add("Reff_id", typeof(string));
            dtBilladjest.Columns.Add("Ac_id", typeof(string));
            dtBilladjest.Columns.Add("ItemSr", typeof(int));
            dtBilladjest.Columns.Add("AdjustSr", typeof(int));
            dtBilladjest.Columns.Add("Amount", typeof(decimal));



            DisplayData();
          
            calcTot();
            SetVno();
            SideFill();
            label10.Text = vno.ToString();
            SideFill();

            if (gresave == true)
            {
                object sender = new object();
                EventArgs e = new EventArgs();
                btn_Click(sender, e);
            }
        }

        private void Display()
        {
            typ = Database.GetScalarText("select [Type] from VOUCHERTYPE where Name='" + gFrmCaption + "'");
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

            cmbVouTyp = "select [name] from vouchertype where active=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " and type='" + typ + "'";
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
        }

        private void Delete()
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
                Database.GetSqlData("Select * from voucheractotal where vi_id='" + vid+"' ", dttemp);
                for (int i = 0; i < dttemp.Rows.Count; i++)
                {
                    dttemp.Rows[i].Delete();
                }
                Database.SaveData(dttemp);

                dttemp = new DataTable("journal");
                Database.GetSqlData("Select * from journal where vi_id='" + vid+"' ", dttemp);
                for (int i = 0; i < dttemp.Rows.Count; i++)
                {
                    dttemp.Rows[i].Delete();
                }
                Database.SaveData(dttemp);

                dttemp = new DataTable("BILLBYBILL");
                Database.GetSqlData("Select * from BILLBYBILL where Bill_id='" + vid+"' ", dttemp);
                for (int i = 0; i < dttemp.Rows.Count; i++)
                {
                    dttemp.Rows[i].Delete();
                }
                Database.SaveData(dttemp);
                dttemp = new DataTable("BILLBYBILL");
                Database.GetSqlData("Select * from BILLBYBILL where receive_id='" + vid+"' ", dttemp);
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

        private bool Validate()
        {
            if (vid != "")
            {
                int count = Database.GetScalarInt("SELECT Count([Vnumber]) AS Expr1 FROM VOUCHERINFO WHERE (((VOUCHERINFO.Vt_id)='" + vtid + "') AND ((VOUCHERINFO.Vi_id)<>'" + vid + "') AND ((VOUCHERINFO.Vnumber)=" + vno + ") AND ((VOUCHERINFO.Vdate)=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash + "))");
                if (count != 0)
                {
                    vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
                }
            }
            if (label3.Text == "0" || label4.Text == "0")
            {
                MessageBox.Show("Please enter some value");
                textBox2.Focus();
                return false;
            }
            else if(label3.Text != label4.Text)
            {
                MessageBox.Show("Debit and Credit are not equal");
                ansGridView1.Focus();
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

        private void SetVno()
        {
            if (generateVno == false)
            {
                int numType = 0;
                numType = funs.chkNumType(vtid);

                if (numType == 3)
                {
                    if (vid == "" || dateTimePicker1.Value.Date.ToString("dd-MMM-yyyy") != chkDt.ToString("dd-MMM-yyyy"))
                    {
                        vno = funs.GenerateVno(vtid, dateTimePicker1.Value.Date.ToString("dd-MMM-yyyy"), vid);
                    }
                }
                else if (numType == 1)
                {
                    if (vid == "")
                    {
                        vno = funs.GenerateVno(vtid, dateTimePicker1.Value.Date.ToString("dd-MMM-yyyy"), vid);
                    }
                }
                else if (numType == 2)
                {
                    if (vid == "" || dateTimePicker1.Value.Date.Month != chkDt.Month)
                    {
                        vno = funs.GenerateVno(vtid, dateTimePicker1.Value.Date.ToString("dd-MMM-yyyy"), vid);
                    }
                }
                label10.Text = vno.ToString();
            }
        }

        private bool save(bool print)
        {
            bool A = Database.GetScalarBool("Select A from Vouchertype where vt_id='" + vtid + "'");
            bool B = Database.GetScalarBool("Select B from Vouchertype where vt_id='" + vtid + "'");
            bool AB = Database.GetScalarBool("Select AB from Vouchertype where vt_id='" + vtid + "'");

            int conn_id = 0;
            ansGridView1.EndEdit();
            calcTot();
            String narr = "";

            if (textBox2.Text == "")
            {
                if (typ=="Journal")
                {
                    narr = "Being Amount Transfer";
                }
            }
            else
            {
                narr = textBox2.Text;
            }            

            if (label3.Text == label4.Text)
            {
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
                dtVoucherInfo.Rows[0]["Transdocdate"] = dateTimePicker1.Value.Date.ToString("dd-MMM-yyyy");
                dtVoucherInfo.Rows[0]["Invoiceno"] = prefix + invoiceno.PadLeft(padding, '0') + postfix;
                dtVoucherInfo.Rows[0]["tdtype"] = false;
                dtVoucherInfo.Rows[0]["RoffChanged"] = false;
                dtVoucherInfo.Rows[0]["TaxChanged"] = false;
                dtVoucherInfo.Rows[0]["Vt_id"] = vtid;
                dtVoucherInfo.Rows[0]["Vnumber"] = vno;
                dtVoucherInfo.Rows[0]["Conn_id"] = conn_id;
                dtVoucherInfo.Rows[0]["narr"] = narr;
                dtVoucherInfo.Rows[0]["Duedate"] = dateTimePicker1.Value.Date.ToString("dd-MMM-yyyy");
                dtVoucherInfo.Rows[0]["Vdate"] = dateTimePicker1.Value.Date.ToString("dd-MMM-yyyy");
                dtVoucherInfo.Rows[0]["SVdate"] = dateTimePicker1.Value.Date.ToString("dd-MMM-yyyy");
                dtVoucherInfo.Rows[0]["Totalamount"] = label4.Text;
                dtVoucherInfo.Rows[0]["FormC"] = false;
                dtVoucherInfo.Rows[0]["Ac_id2"] = 0;
                dtVoucherInfo.Rows[0]["Directchanged"] = false;
                dtVoucherInfo.Rows[0]["Iscancel"] = true;
                dtVoucherInfo.Rows[0]["RCM"] = false;
                dtVoucherInfo.Rows[0]["5000Allowed"] = false;
                dtVoucherInfo.Rows[0]["ITC"] = false;
                dtVoucherInfo.Rows[0]["Reffno"] = "";
              
                dtVoucherInfo.Rows[0]["branch_id"] = Database.BranchId;
                if (ansGridView1.Rows.Count == 3)
                {
                    if (double.Parse(ansGridView1.Rows[0].Cells["Dr"].Value.ToString()) > 0)
                    {
                        dtVoucherInfo.Rows[0]["dr_ac_id"] = funs.Select_ac_id(ansGridView1.Rows[0].Cells["acc"].Value.ToString());
                        dtVoucherInfo.Rows[0]["cr_ac_id"] = funs.Select_ac_id(ansGridView1.Rows[1].Cells["acc"].Value.ToString());
                    }
                    else
                    {
                        dtVoucherInfo.Rows[0]["cr_ac_id"] = funs.Select_ac_id(ansGridView1.Rows[0].Cells["acc"].Value.ToString());
                        dtVoucherInfo.Rows[0]["dr_ac_id"] = funs.Select_ac_id(ansGridView1.Rows[1].Cells["acc"].Value.ToString());
                    }
                }

                if (vid == "")
                {
                    dtVoucherInfo.Rows[0]["CreTime"] = System.DateTime.Now.ToString("HH:mm:ss");
                }

                dtVoucherInfo.Rows[0]["ModTime"] = System.DateTime.Now.ToString("HH:mm:ss");
                dtVoucherInfo.Rows[0]["Cash_Pending"] = false;

                if (Database.utype.ToUpper() == "USER")
                {
                    dtVoucherInfo.Rows[0]["NApproval"] = true;
                }
                else
                {
                    dtVoucherInfo.Rows[0]["NApproval"] = false;
                }

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
                for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
                {
                    if (ansGridView1.Rows[i].Cells["dr"].Value == null)
                    {
                        ansGridView1.Rows[i].Cells["dr"].Value = "0.00";
                    }
                    else if (ansGridView1.Rows[i].Cells["cr"].Value == null)
                    {
                        ansGridView1.Rows[i].Cells["cr"].Value = "0.00";
                    }
                    string reffno = "";
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
                   // dtVoucheractotal.Rows[i]["Reffno"] = reffno;
                    dtVoucheractotal.Rows[i]["Srno"] = ansGridView1.Rows[i].Cells["sno"].Value.ToString();
                    dtVoucheractotal.Rows[i]["Accid"] = funs.Select_ac_id(ansGridView1.Rows[i].Cells["acc"].Value.ToString());
                    dtVoucheractotal.Rows[i]["Cam"] = ansGridView1.Rows[i].Cells["cr"].Value;
                    dtVoucheractotal.Rows[i]["Amount"] = ansGridView1.Rows[i].Cells["dr"].Value;
                    dtVoucheractotal.Rows[i]["cdate"] = dateTimePicker1.Value.Date.ToString("dd-MMM-yyyy");
                    dtVoucheractotal.Rows[i]["LocationId"] = Prelocationid;
                }
                Database.SaveData(dtVoucheractotal);

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

                
                //DataTable dtJSave = new DataTable();
                //dtJSave.Columns.Add("Ac_id", typeof(int));
                //dtJSave.Columns.Add("Dr", typeof(double));
                //dtJSave.Columns.Add("Cr", typeof(double));
                //dtJSave.Columns.Add("Narr", typeof(string));

                for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
                {

                    dtJournal.Rows.Add();
                    dtJournal.Rows[dtJournal.Rows.Count - 1]["Vi_id"] = vid;

                    dtJournal.Rows[dtJournal.Rows.Count - 1]["Vdate"] = dateTimePicker1.Value;
                    dtJournal.Rows[dtJournal.Rows.Count - 1]["Ac_id"] = funs.Select_ac_id(ansGridView1.Rows[i].Cells["acc"].Value.ToString());
                    if (double.Parse(ansGridView1.Rows[i].Cells["Dr"].Value.ToString()) > 0)
                    {
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = ansGridView1.Rows[i].Cells["dr"].Value.ToString();
                    }
                    else
                    {
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = -1 * (double.Parse(ansGridView1.Rows[i].Cells["cr"].Value.ToString()));
                    }



                    dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr"] = narr;
                    dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr2"] = narr;
                    dtJournal.Rows[dtJournal.Rows.Count - 1]["Sno"] = ansGridView1.Rows[i].Cells["sno"].Value.ToString();
                    dtJournal.Rows[dtJournal.Rows.Count - 1]["LocationId"] = Database.LocationId;
                    dtJournal.Rows[dtJournal.Rows.Count - 1]["A"] = A;
                    dtJournal.Rows[dtJournal.Rows.Count - 1]["B"] = B;
                    dtJournal.Rows[dtJournal.Rows.Count - 1]["AB"] = AB;
                    if (ansGridView1.Rows.Count == 3)
                    {
                        if (i == 0)
                        {
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = funs.Select_ac_id(ansGridView1.Rows[1].Cells["acc"].Value.ToString());
                        }
                        else if (i == 1)
                        {
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = funs.Select_ac_id(ansGridView1.Rows[0].Cells["acc"].Value.ToString());
                        }
                    }
                }




                Database.SaveData(dtJournal);


                dtTemp = new DataTable("Billadjest");
                Database.GetSqlData("Select * from Billadjest where Vi_id='" + vid + "'", dtTemp);
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
                                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["id"] = Database.LocationId + (Nid + 1);
                                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Nid"] = (Nid + 1);
                                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["LocationId"] = Database.LocationId;


                            }

                            dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Ac_id"] = funs.Select_ac_id(ansGridView1.Rows[i].Cells["acc"].Value.ToString());
                            dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Vi_id"] = vid;
                            dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["itemsr"] = int.Parse(ansGridView1.Rows[i].Cells["sno"].Value.ToString());
                            dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["AdjustSr"] = 1;
                            dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Reff_id"] = vid;


                            if (double.Parse(ansGridView1.Rows[i].Cells["dr"].Value.ToString()) > 0)
                            {
                                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Amount"] = double.Parse(ansGridView1.Rows[i].Cells["dr"].Value.ToString());
                            }
                            else
                            {
                                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Amount"] = -1 * double.Parse(ansGridView1.Rows[i].Cells["cr"].Value.ToString());
                            }
                            dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["A"] = A;
                            dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["B"] = B;
                            dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["AB"] = true;
                        }

                    }


                    else
                    {
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

                                int rowindex = i;
                                int Nid = int.Parse(dtBilladjest.Rows[rowindex]["Nid"].ToString());
                                //   int Nid = int.Parse(dtBilladjest.Rows[0]["Nid"].ToString());
                                dtBilladjest.Rows[i]["id"] = Database.LocationId + (Nid + 1);
                                dtBilladjest.Rows[i]["Nid"] = (Nid + 1);
                                dtBilladjest.Rows[i]["LocationId"] = Database.LocationId;

                                //DataTable dtAcid = new DataTable();
                                //Database.GetSqlData("select max(Nid) as Nid from BillAdjest where locationid='" + Database.LocationId + "'", dtAcid);
                                //int Nid = 0;
                                //if (i == 0)
                                //{
                                //    Nid = int.Parse(dtAcid.Rows[0][0].ToString());
                                //}
                                //else
                                //{
                                //    Nid = int.Parse(dtBilladjest.Rows[i - 1]["Nid"].ToString());
                                //}

                                //dtBilladjest.Rows[i]["id"] = Database.LocationId + (Nid + 1);
                                //dtBilladjest.Rows[i]["Nid"] = (Nid + 1);
                                //dtBilladjest.Rows[i]["LocationId"] = Database.LocationId;

                            }

                            dtBilladjest.Rows[i]["Vi_id"] = vid;
                            dtBilladjest.Rows[i]["A"] = A;
                            dtBilladjest.Rows[i]["B"] = B;
                            dtBilladjest.Rows[i]["AB"] = true;

                            dtBilladjest.Rows[i].AcceptChanges();
                            dtBilladjest.Rows[i].SetAdded();
                        }
                    }

                }
                //for (int i = 0; i < dtBilladjest.Rows.Count; i++)
                //{

                    //if (funs.Select_MainAccTypeName(ansGridView1.Rows[i].Cells["acc"].Value.ToString()) == "SUNDRY DEBTORS" || funs.Select_MainAccTypeName(ansGridView1.Rows[i].Cells["acc"].Value.ToString()) == "SUNDRY CREDITORS")
                    //{
                    //    if (dtBilladjest.Rows.Count == 0)
                    //    {

                    //        DataTable dtCount = new DataTable();
                    //        Database.GetSqlData("select count(*) from BillAdjest where locationid='" + Database.LocationId + "'", dtCount);
                    //        if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                    //        {
                    //            dtBilladjest.Rows[i]["id"] = Database.LocationId + "1";
                    //            dtBilladjest.Rows[i]["Nid"] = 1;
                    //            dtBilladjest.Rows[i]["LocationId"] = Database.LocationId;

                    //        }
                    //        else
                    //        {
                    //            DataTable dtAcid = new DataTable();
                    //            Database.GetSqlData("select max(Nid) as Nid from BillAdjest where locationid='" + Database.LocationId + "'", dtAcid);
                    //            int Nid = int.Parse(dtAcid.Rows[0][0].ToString());
                    //            dtBilladjest.Rows[i]["id"] = Database.LocationId + (Nid + 1);
                    //            dtBilladjest.Rows[i]["Nid"] = (Nid + 1);
                    //            dtBilladjest.Rows[i]["LocationId"] = Database.LocationId;

                    //        }
                    //    }
                    //    else
                    //    {

                    //        int rowindex = i;
                    //        int Nid = int.Parse(dtBilladjest.Rows[rowindex]["Nid"].ToString());
                    //        //   int Nid = int.Parse(dtBilladjest.Rows[0]["Nid"].ToString());
                    //        dtBilladjest.Rows[i]["id"] = Database.LocationId + (Nid + 1);
                    //        dtBilladjest.Rows[i]["Nid"] = (Nid + 1);
                    //        dtBilladjest.Rows[i]["LocationId"] = Database.LocationId;

                    //        //DataTable dtAcid = new DataTable();
                    //        //Database.GetSqlData("select max(Nid) as Nid from BillAdjest where locationid='" + Database.LocationId + "'", dtAcid);
                    //        //int Nid = 0;
                    //        //if (i == 0)
                    //        //{
                    //        //    Nid = int.Parse(dtAcid.Rows[0][0].ToString());
                    //        //}
                    //        //else
                    //        //{
                    //        //    Nid = int.Parse(dtBilladjest.Rows[i - 1]["Nid"].ToString());
                    //        //}

                    //        //dtBilladjest.Rows[i]["id"] = Database.LocationId + (Nid + 1);
                    //        //dtBilladjest.Rows[i]["Nid"] = (Nid + 1);
                    //        //dtBilladjest.Rows[i]["LocationId"] = Database.LocationId;

                    //    }

                    //    dtBilladjest.Rows[i]["Vi_id"] = vid;
                    //    dtBilladjest.Rows[i]["A"] = A;
                    //    dtBilladjest.Rows[i]["B"] = B;
                    //    dtBilladjest.Rows[i]["AB"] = true;

                    //    dtBilladjest.Rows[i].AcceptChanges();
                    //    dtBilladjest.Rows[i].SetAdded();
                    //}
               // }
                Database.SaveData(dtBilladjest);








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
                //        if (double.Parse(ansGridView1.Rows[i].Cells["dr"].Value.ToString()) > 0)
                //        {
                //            dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Amount"] = double.Parse(ansGridView1.Rows[i].Cells["dr"].Value.ToString());
                //        }
                //        else
                //        {
                //            dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Amount"] = -1 * double.Parse(ansGridView1.Rows[i].Cells["cr"].Value.ToString());
                //        }
                //    }

                //}
               
                funs.ShowBalloonTip("Saved", "Saved Successfully");
                if (print == true)
                {
                    if (Feature.Available("Ask Copies") == "No")
                    {
                        OtherReport rpt = new OtherReport();
                        DataTable dtprintcopy = new DataTable();
                        Database.GetSqlData("Select printcopy from Vouchertype where Vt_id=" + vtid, dtprintcopy);
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
            }
            return true;
        }
        private string IsDocumentNumber(String str)
        {

            string res = Database.GetScalarText("SELECT DISTINCT VOUCHERINFO.Vi_id FROM VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERTYPE.Short + ' ' + CONVERT(nvarchar, VOUCHERINFO.Vdate, 112) + ' ' + CAST(VOUCHERINFO.Vnumber AS nvarchar(10)) )='" + str + "'))");
            if (res == "")
            {
                res = "0";
            }

            return res;
        }
        private void ansGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView1.CurrentCell.OwningColumn.Name == "dr")
            {
                try
                {
                    double amt1 = double.Parse(ansGridView1.Rows[e.RowIndex].Cells["dr"].Value.ToString());
                }
                catch (Exception ex)
                {
                    ansGridView1.Rows[e.RowIndex].Cells["dr"].Value = "0.00";
                    return;
                }
                calcTot();
            }
            else if (ansGridView1.CurrentCell.OwningColumn.Name == "cr")
            {
                try
                {
                    double amt1 = double.Parse(ansGridView1.Rows[e.RowIndex].Cells["cr"].Value.ToString());
                }
                catch (Exception ex)
                {
                    ansGridView1.Rows[e.RowIndex].Cells["cr"].Value = "0.00";
                    return;
                }
                calcTot();
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
             //   dtBilladjest.Rows[
            }
            dtBilladjest.AcceptChanges();
            
        }
        private void calcTot()
        {
            double dtot = 0.0, ctot = 0.0;
            for (int i = 0; i < ansGridView1.RowCount - 1; i++)
            {
                if (ansGridView1.Rows[i].Cells["dr"].Value != null)
                {
                    dtot += double.Parse(ansGridView1.Rows[i].Cells["dr"].Value.ToString());
                }
                if (ansGridView1.Rows[i].Cells["cr"].Value != null)
                {
                    ctot += double.Parse(ansGridView1.Rows[i].Cells["cr"].Value.ToString());
                }
            }
            label3.Text = dtot.ToString();
            label4.Text = ctot.ToString();
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
            if (ansGridView1.CurrentCell.OwningColumn.Name == "acc")
            {
             
                strCombo = funs.GetStrComboled("*");
                ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);


                if (ansGridView1.CurrentCell.Value != null)
                {
                    delbilladjest(int.Parse(ansGridView1.CurrentRow.Cells["sno"].Value.ToString()));
                       // SendKeys.Send("{enter}");
                    
                }
            }
            //if (ansGridView1.CurrentCell.OwningColumn.Name == "reffno")
            //{
            //    if (funs.Select_MainAccTypeName(ansGridView1.CurrentRow.Cells["acc"].Value.ToString()).ToUpper() == "SUNDRY DEBTORS" || funs.Select_MainAccTypeName(ansGridView1.CurrentRow.Cells["acc"].Value.ToString()).ToUpper() == "SUNDRY CREDITORS")
            //    {

            //        DataTable dtcombo = new DataTable();
            //        // strCombo = "select distinct '<New Refference>' as ReffNo, 0.00  & ' '  as Amt  from Account union all  select iif(DocNumber='','Opening',DocNumber) as ReffNo,amt  & ' '  as Amt from(SELECT VOUCHERTYPE.Short & ' ' & Format(Voucherinfo.Vdate,'yyyymmdd' & ' ' & VOUCHERINFO.Vnumber) AS DocNumber, Sum(BILLADJEST.Amount) AS amt FROM (BILLADJEST LEFT JOIN VOUCHERINFO ON BILLADJEST.Reff_id = VOUCHERINFO.Vi_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((BILLADJEST.Ac_id)=" + funs.Select_ac_id(ansGridView1.CurrentRow.Cells["acc"].Value.ToString()) + ")) GROUP BY VOUCHERTYPE.Short & ' ' & Format(Voucherinfo.Vdate,'yyyymmdd' & ' ' & VOUCHERINFO.Vnumber) HAVING (((Sum(BILLADJEST.Amount))<>0))) as res";
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

        private void ansGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            ansGridView1.CurrentCell.Value = 0;
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
                    ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells[1].Value = "";
                    ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["dr"].Value = "0.00";
                    ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["cr"].Value = "0.00";
                    return;
                }
                else
                {

                    int rindex = ansGridView1.CurrentRow.Index;
                    ansGridView1.Rows.RemoveAt(rindex);
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
            if (ansGridView1.CurrentCell.OwningColumn.Name == "cr")
            {
                if (ansGridView1.CurrentCell.Value == null && ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["dr"].Value == null)
                {
                    SendKeys.Send("{tab}");
                }
            }




            if (e.Alt == true && e.Control == true && e.KeyCode == Keys.B)
            {
                string columnname = "";
                if (double.Parse(ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["dr"].Value.ToString()) != 0)
                {
                    columnname = "Payment";
                }
                else if (double.Parse(ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["cr"].Value.ToString()) != 0)
                {
                    columnname = "Receipt";
                }
                else
                {
                    return;
                }



                if (funs.Select_MainAccTypeName(ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["acc"].Value.ToString()) == "SUNDRY DEBTORS" || funs.Select_MainAccTypeName(ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["acc"].Value.ToString()) == "SUNDRY CREDITORS")
                {

                    int rnum = ansGridView1.CurrentRow.Index + 1;

                    if (columnname == "Payment")
                    {
                        if (dtBilladjest.Select("Itemsr=" + rnum).Length != 0)
                        {
                            double adtotalamt = double.Parse(dtBilladjest.Compute("sum(Amount)", "Itemsr=" + rnum).ToString());
                            if (-1 * adtotalamt != -1 * double.Parse(ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["dr"].Value.ToString()))
                            {
                                delbilladjest(rnum);
                            }
                        }
                        if (double.Parse(ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["dr"].Value.ToString()) != 0)
                        {
                            frm_adjust frm = new frm_adjust(dtBilladjest, rnum, "Payment", vid, double.Parse(ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["dr"].Value.ToString()), funs.Select_ac_id(ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["acc"].Value.ToString()));
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
                            if (adtotalamt != -1 * double.Parse(ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["cr"].Value.ToString()))
                            {
                                delbilladjest(rnum);
                            }
                        }

                        if (double.Parse(ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["cr"].Value.ToString()) != 0)
                        {
                            frm_adjust frm = new frm_adjust(dtBilladjest, rnum, "Receipt", vid, double.Parse(ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["cr"].Value.ToString()), funs.Select_ac_id(ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["acc"].Value.ToString()));
                            frm.ShowDialog();
                            dtBilladjest = frm.gdt;
                            SendKeys.Send("{Enter}");
                        }
                    }

                }

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

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (Database.IsKacha == false)
            //{
            cmbVouTyp = "select [name] from vouchertype where active=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " and type='" + typ + "' and " + Database.BMode + "=" + access_sql.Singlequote + "true" + access_sql.Singlequote + "";
            //}
            //else
            //{
            //    cmbVouTyp = "select [name] from vouchertype where active=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " and type='" + typ + "' and B=" + access_sql.Singlequote + "true" + access_sql.Singlequote + "";
            //}
            textBox3.Text = SelectCombo.ComboKeypress(this, e.KeyChar, cmbVouTyp, e.KeyChar.ToString(), 0);
            vtid = funs.Select_vt_id_vnm(textBox3.Text);
            //if (textBox3.Text != "")
            //{
            //    textBox1.Enabled = true;
            //}
            SetVno();
            label10.Text = vno.ToString();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            SetVno();
        }      
    }
}
