using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
using System.Net;
using System.Net.Mail;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace faspi
{
    public partial class frmTransaction : Form
    {
        public string EwayBillno, TransportName, Transdocno, Vehicleno;
        public double Distance = 0;
        public DateTime Transdocdate;
        List<UsersFeature> permission;
        DataTable dtBilladjest;
        public string cmdmode;
        string cmbAcc = "";
        string cmbVouTyp = "";
        string Prelocationid = "";
        string Ratesapp = "";
        int rateindex = 0;
        bool mailSent = false;
        string status = "";
        DataTable dtdispitems;
        DataTable dtVoucherInfo;
        DataTable dtVoucherDet;
        DataTable dtVoucherCharges1;
        DataTable dtVoucherCharges2;
        DataTable dtVoucherCharges4;
        DataTable dtItemTax;
        DataTable dtStock;
        DataTable dtVoucherCharges3;
        DataTable dtRoundOff;
        DataTable dtItemCharges;
        DataTable dtpaymentdet;
        DataTable dtJournal;
        public string shiptoacc_id = "";
        public string shiptoprint = "", shiptoaddress1 = "", shiptoaddress2 = "", shiptocontact = "", shiptoemail = "", shiptotin = "", shiptostate = "", shiptoPan = "", shiptoAadhar = "", shiptocityid = "", shiptoPincode = "", disfromacc_id = "";
        int vno = 0;
        string vid;
        string vtid;
        double ctaxamt1 = 0, ctaxamt2 = 0, ctaxamt3 = 0, ctaxamt4 = 0, totTaxabe = 0, totdisaftertax = 0;
        double dis1 = 0, dis2 = 0, dis3 = 0;
        bool locked = false;
        bool itemCharges = false;
        bool gExcludingTax = true;
        string gCalculationType = "";
        bool f12used = false;
        public bool formC = false;
        public bool gresave = false;
        bool TaxChanged = false;
        bool RoffChanged = false;
        string[] packnm = new string[50];
        string desc = "", unit = "";
        string gStr = "";
        string desc_id = "";
        OleDbCommand comm;
        DateTime chkDt = new DateTime();
        public string SubCategory_Name = "", field1 = "", field2 = "", field3 = "", field4 = "", field5 = "", field6 = "", field7 = "", field8 = "";
        string gFrmCaption = "";
        string gtype = "";
        bool gExState = false, gUnregistered = false, gtaxinvoice = false;
        int fid, uid;
        string wh1 = "", wh2 = "";
        string strCombo;
        double totDis = 0;
        int disCnt = 0;
        int dtRcnt = 0;
        double qtyTot = 0, weightTot = 0, chargesTot = 0, disTot = 0, cartageTot = 0, totbottomdis = 0, totexpamount = 0;
        DataTable dtDisp = new DataTable();
        DataTable dtDescItem = new DataTable();
        int qdAtLast = 0;
        bool DirectChangeAmount = false;
        Boolean EditDelete = false;

        public frmTransaction()
        {
            InitializeComponent();
            Transdocdate = dateTimePicker1.Value;
        }

        public void DisplayData(string vi_id)
        {
            int numtype = funs.Select_NumType(vtid);
            if (numtype == 1 && vi_id != "" )
            {
                dateTimePicker1.Enabled = false;
            }
            dtVoucherInfo = new DataTable("Voucherinfo");
            Database.GetSqlData("select * from Voucherinfo where Vi_id='" + vi_id + "' ", dtVoucherInfo);
            
            if (dtVoucherInfo.Rows.Count > 0)
            {
                if (Feature.Available("Common Sale Invoice Numbers") == "Yes")
                {
                    textBox15.Enabled = true;
                }
                else
                {
                    textBox15.Enabled = false;
                }
                Prelocationid = dtVoucherInfo.Rows[0]["Locationid"].ToString();
                textBox15.Text = funs.Select_vt_nm(dtVoucherInfo.Rows[0]["Vt_id"].ToString());
                vtid = dtVoucherInfo.Rows[0]["Vt_id"].ToString();
                gExcludingTax = bool.Parse(dtVoucherInfo.Rows[0]["tdtype"].ToString());


                //if (Database.IsKacha == false)
                //{
                string cmbVouTyp4 = " and " + Database.BMode + "=" + access_sql.Singlequote + "true" + access_sql.Singlequote;
                //}
                //else
                //{
                //    cmbVouTyp4 = " and B=" + access_sql.Singlequote + "true" + access_sql.Singlequote;
                //}

                //DataTable dtvt = new DataTable();
                //cmbVouTyp = "select [name] from vouchertype where vt_id='"+vtid+"' " + cmbVouTyp4 + "  ";

                //Database.GetSqlData(cmbVouTyp, dtvt);

                //if (dtvt.Rows.Count == 1)
                //{
                //    textBox15.Text = dtvt.Rows[0]["name"].ToString();
                //    vtid = funs.Select_vt_id_vnm(textBox15.Text);
                //    textBox27.Text = funs.Select_Rates_Value(funs.Select_vt_RateType(vtid));

                //    if (textBox27.Text != "")
                //    {
                //        Ratesapp = Master.DtRates.Select("RateValue='" + textBox27.Text + "'").FirstOrDefault()["RateId"].ToString();
                //        for (int i = 0; i < ansGridView1.RowCount - 1; i++)
                //        {
                //            ItemSelected(true, i);
                //            ItemCalc(i);
                //        }
                //        labelCalc();
                //    }

                //    //   Ratesapp=
                //    gtaxinvoice = funs.Select_vt_taxinvoice(vtid);
                //    textBox15.Enabled = false;

                //    if (textBox15.Text == "")
                //    {
                //        return;
                //    }
                //    vtid = funs.Select_vt_id_vnm(textBox15.Text);
                //    gtaxinvoice = funs.Select_vt_taxinvoice(vtid);
                //    gExState = funs.Select_vt_Exstate(vtid);
                //    gUnregistered = funs.Select_vt_Unregistered(vtid);
                //    gExcludingTax = funs.Select_vt_Excludungtax(vtid);
                //    gCalculationType = funs.Select_vt_CalculationType(vtid);

                //    if (gtype == "Sale" && gExState == true)
                //    {
                //        DialogResult chk = MessageBox.Show("Is Company Provide Form-C?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                //        if (chk == DialogResult.Yes)
                //        {
                //            formC = true;
                //        }
                //        else
                //        {
                //            formC = false;
                //        }
                //    }
                //    else
                //    {
                //        formC = false;
                //    }
                //    if ((gtype == "Purchase" || gtype == "P Return") && gExState == true)
                //    {
                //        SubCategory_Name = "Central Purchase";
                //    }
                //    else if ((gtype == "Purchase" || gtype == "P Return") && gExState == false)
                //    {
                //        SubCategory_Name = "Local Purchase";
                //    }
                //    else if ((gtype == "Sale" || gtype == "Return") && gExState == true)
                //    {
                //        SubCategory_Name = "Central Sale";
                //    }
                //    else if ((gtype == "Sale" || gtype == "Return" || gtype == "Pending") && gExState == false)
                //    {
                //        SubCategory_Name = "Local Sale";
                //    }
                //    else if (gtype == "receive")
                //    {
                //        SubCategory_Name = "Local Purchase";
                //    }
                //    else if (gtype == "issue")
                //    {
                //        SubCategory_Name = "Local Sale";
                //    }
                //    else if (gtype == "Transfer")
                //    {
                //        SubCategory_Name = "Local Purchase";
                //    }
                //    if (gCalculationType == "Including Tax Only")
                //    {
                //        checkBox1.Enabled = false;
                //        checkBox1.Checked = true;
                //        gExcludingTax = false;
                //    }
                //    else if (gCalculationType == "Excluding Tax Only")
                //    {
                //        checkBox1.Enabled = false;
                //        checkBox1.Checked = false;
                //        gExcludingTax = true;
                //    }
                //    else if (gCalculationType == "Default Excluding Tax")
                //    {
                //        checkBox1.Enabled = true;
                //        checkBox1.Checked = false;
                //        gExcludingTax = true;
                //    }
                //    else if (gCalculationType == "Default Including Tax")
                //    {
                //        checkBox1.Enabled = true;
                //        checkBox1.Checked = true;
                //        gExcludingTax = false;
                //    }
                //    if (vtid == "")
                //    {
                //        ansGridView1.Enabled = false;
                //    }
                //    else
                //    {
                //        ansGridView1.Enabled = true;
                //    }
                //}
                //else
                //{
                textBox15.Enabled = true;
                // }
                EwayBillno = dtVoucherInfo.Rows[0]["EwayBillno"].ToString();
                gtaxinvoice = funs.Select_vt_taxinvoice(vtid);
                DirectChangeAmount = bool.Parse(dtVoucherInfo.Rows[0]["DirectChanged"].ToString());
                if (dtVoucherInfo.Rows[0]["ac_id"].ToString() == "")
                {
                    textBox14.Text = "<MAIN>";
                }
                else
                {
                    textBox14.Text = funs.Select_ac_nm(dtVoucherInfo.Rows[0]["ac_id"].ToString());
                }
                textBox28.Text = funs.Select_salesman_nm(dtVoucherInfo.Rows[0]["s_id"].ToString());
                shiptoacc_id = dtVoucherInfo.Rows[0]["ac_id2"].ToString();
                if (dtVoucherInfo.Rows[0]["dispatch_id"].ToString() != "")
                {
                    disfromacc_id = dtVoucherInfo.Rows[0]["dispatch_id"].ToString();
                }

                textBox27.Text = funs.Select_Rates_Value(funs.Select_vt_RateType(vtid));

                if (textBox27.Text != "")
                {
                    Ratesapp = Master.DtRates.Select("RateValue='" + textBox27.Text + "'").FirstOrDefault()["RateId"].ToString();
                }
                string accrateapp = "";
                if (gtype == "Purchase" || gtype == "P Return")
                {
                    accrateapp = Database.GetScalarText("Select Rateapp2 from Account where Name='" + textBox14.Text + "'");
                }
                else
                {
                    accrateapp = Database.GetScalarText("Select Rateapp from Account where Name='" + textBox14.Text + "'");
                }
                if (accrateapp != "")
                {
                    textBox27.Text = funs.Select_Rates_Value(accrateapp);
                    Ratesapp = accrateapp;
                }

                textBox14.Tag = dtVoucherInfo.Rows[0]["ac_id"].ToString();

                if (gExState == true)
                {
                    textBox3.Text = dtVoucherInfo.Rows[0]["Formno"].ToString();
                }

                dateTimePicker1.Text = dtVoucherInfo.Rows[0]["Vdate"].ToString();
                vno = int.Parse(dtVoucherInfo.Rows[0]["Vnumber"].ToString());
                label10.Text = dtVoucherInfo.Rows[0]["Vnumber"].ToString();
                RoffChanged = bool.Parse(dtVoucherInfo.Rows[0]["RoffChanged"].ToString());
                TaxChanged = bool.Parse(dtVoucherInfo.Rows[0]["TaxChanged"].ToString());
                chkDt = DateTime.Parse(dtVoucherInfo.Rows[0]["Vdate"].ToString());
                textBox2.Text = dtVoucherInfo.Rows[0]["Svnum"].ToString();
                if (dtVoucherInfo.Rows[0]["rate"].ToString() == "")
                {
                    dtVoucherInfo.Rows[0]["rate"] = 0;
                }
                if (dtVoucherInfo.Rows[0]["ShiptoDistance"].ToString() == "")
                {
                    dtVoucherInfo.Rows[0]["ShiptoDistance"] = 0;
                }
                Distance = double.Parse(dtVoucherInfo.Rows[0]["ShiptoDistance"].ToString());

                if (dtVoucherInfo.Rows[0]["Transporter_id"].ToString() == "")
                {

                }
                else
                {
                    TransportName = funs.Select_ac_nm(dtVoucherInfo.Rows[0]["Transporter_id"].ToString());
                }
                Transdocno = dtVoucherInfo.Rows[0]["Transdocno"].ToString();
                Vehicleno = dtVoucherInfo.Rows[0]["TransVehNo"].ToString();
                if (dtVoucherInfo.Rows[0]["Transdocdate"].ToString() == "")
                {
                    Transdocdate = DateTime.Parse(dateTimePicker1.Value.Date.ToString());
                }
                else
                {
                    Transdocdate = DateTime.Parse(dtVoucherInfo.Rows[0]["Transdocdate"].ToString());
                }
                textBox12.Text = funs.DecimalPoint(double.Parse(dtVoucherInfo.Rows[0]["rate"].ToString()), 2);
                dateTimePicker2.Value = DateTime.Parse(dtVoucherInfo.Rows[0]["Svdate"].ToString());
                dateTimePicker3.Value = DateTime.Parse(dtVoucherInfo.Rows[0]["Duedate"].ToString());
                textBox7.Text = dtVoucherInfo.Rows[0]["Narr"].ToString();
                textBox9.Text = funs.DecimalPoint(double.Parse(dtVoucherInfo.Rows[0]["Roff"].ToString()), 2);
                gCalculationType = funs.Select_vt_CalculationType(vtid);
                if (dtVoucherInfo.Rows[0]["RCM"].ToString() == null || dtVoucherInfo.Rows[0]["RCM"].ToString() == "")
                {
                    dtVoucherInfo.Rows[0]["RCM"] = false;
                }
                if (bool.Parse(dtVoucherInfo.Rows[0]["RCM"].ToString()) == true)
                {
                    checkBox2.Checked = true;
                }
                else
                {
                    checkBox2.Checked = false;
                }


                if (dtVoucherInfo.Rows[0]["CashCredit"].ToString() == "Credit")
                {
                    radioButton7.Checked = true;
                }
                else
                {
                    radioButton8.Checked = true;
                }


                if (dtVoucherInfo.Rows[0]["5000Allowed"].ToString() == null || dtVoucherInfo.Rows[0]["5000Allowed"].ToString() == "")
                {
                    dtVoucherInfo.Rows[0]["5000Allowed"] = false;
                }
                if (dtVoucherInfo.Rows[0]["ITC"].ToString() == null || dtVoucherInfo.Rows[0]["ITC"].ToString() == "")
                {
                    dtVoucherInfo.Rows[0]["ITC"] = false;
                }
                if (bool.Parse(dtVoucherInfo.Rows[0]["5000Allowed"].ToString()) == true)
                {
                    checkBox3.Checked = true;
                }
                else
                {
                    checkBox3.Checked = false;
                }
                if (bool.Parse(dtVoucherInfo.Rows[0]["ITC"].ToString()) == true)
                {
                    checkBox4.Checked = true;
                }
                else
                {
                    checkBox4.Checked = false;
                }


                if (dtVoucherInfo.Rows[0]["CmsnAmt"].ToString() == "")
                {
                }
                else
                {
                    label12.Text = funs.IndianCurr(double.Parse(dtVoucherInfo.Rows[0]["CmsnAmt"].ToString()));
                }


                TextBox tbx1 = this.Controls.Find(Master.TransportDetails.Rows[0]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                field1 = dtVoucherInfo.Rows[0]["Transport1"].ToString();
                tbx1.Text = field1;

                TextBox tbx2 = this.Controls.Find(Master.TransportDetails.Rows[1]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                field2 = dtVoucherInfo.Rows[0]["Transport2"].ToString();
                tbx2.Text = field2;
                TextBox tbx3 = this.Controls.Find(Master.TransportDetails.Rows[3]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                field4 = dtVoucherInfo.Rows[0]["Grno"].ToString();
                tbx3.Text = field4;

                TextBox tbx4 = this.Controls.Find(Master.TransportDetails.Rows[2]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                field3 = dtVoucherInfo.Rows[0]["DeliveryAt"].ToString();
                tbx4.Text = field3;

                TextBox tbx5 = this.Controls.Find(Master.TransportDetails.Rows[4]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                field5 = dtVoucherInfo.Rows[0]["Transport3"].ToString();
                tbx5.Text = field5;

                TextBox tbx6 = this.Controls.Find(Master.TransportDetails.Rows[5]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                field6 = dtVoucherInfo.Rows[0]["Transport4"].ToString();
                tbx6.Text = field6;
                TextBox tbx7 = this.Controls.Find(Master.TransportDetails.Rows[6]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                field7 = dtVoucherInfo.Rows[0]["Transport5"].ToString();
                tbx7.Text = field7;

                TextBox tbx8 = this.Controls.Find(Master.TransportDetails.Rows[7]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                field8 = dtVoucherInfo.Rows[0]["Transport6"].ToString();
                tbx8.Text = field8;
                DisplayTransportdet();
                shiptoaddress1 = dtVoucherInfo.Rows[0]["ShiptoAddress1"].ToString();
                shiptoaddress2 = dtVoucherInfo.Rows[0]["ShiptoAddress2"].ToString();
                shiptoemail = dtVoucherInfo.Rows[0]["ShiptoEmail"].ToString();
                shiptotin = dtVoucherInfo.Rows[0]["ShiptoTIN"].ToString();
                shiptocontact = dtVoucherInfo.Rows[0]["ShiptoPhone"].ToString();
                shiptoPincode = dtVoucherInfo.Rows[0]["ShiptoPincode"].ToString();
                if (dtVoucherInfo.Rows[0]["ShiptoStateid"].ToString() == "")
                {
                    dtVoucherInfo.Rows[0]["ShiptoStateid"] = 0;
                }
                shiptostate = funs.Select_state_nm(dtVoucherInfo.Rows[0]["ShiptoStateid"].ToString());
                if (dtVoucherInfo.Rows[0]["ShiptoCity_id"].ToString() == "")
                {
                    dtVoucherInfo.Rows[0]["ShiptoCity_id"] = 0;
                }

                shiptocityid = dtVoucherInfo.Rows[0]["ShiptoCity_id"].ToString();
                shiptoprint = dtVoucherInfo.Rows[0]["Shipto"].ToString();
                shiptoPan = dtVoucherInfo.Rows[0]["ShiptoPAN"].ToString(); ;
                shiptoAadhar = dtVoucherInfo.Rows[0]["ShiptoAadhar"].ToString(); ;

                formC = bool.Parse(dtVoucherInfo.Rows[0]["FormC"].ToString());

                textBox17.Text = funs.Select_ac_nm(dtVoucherInfo.Rows[0]["Conn_id"].ToString());

                if (dtVoucherInfo.Rows[0]["Sq_FT_MT"].ToString() == radioButton10.Text)
                {
                    radioButton10.Checked = true;
                }
                if (dtVoucherInfo.Rows[0]["Sq_FT_MT"].ToString() == radioButton9.Text)
                {
                    radioButton9.Checked = true;
                }


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
            else
            {
                // flowLayoutPanel1.Controls.Clear();
                //  vno = 0;

                field1 = "";
                field2 = "";
                field3 = "";
                field4 = "";
                field5 = "";
                field6 = "";
                field7 = "";
                field8 = "";


                ansGridView1.Rows.Clear();
                ansGridView3.Rows.Clear();
                ansGridView4.Rows.Clear();
                // label10.Text = "";
                textBox1.Text = "0";
                textBox2.Text = "0";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                if (gtype != "RCM")
                {
                    checkBox2.Checked = false;
                    checkBox3.Checked = false;
                    checkBox4.Checked = false;
                }
                else
                {
                    checkBox2.Checked = true;
                    checkBox3.Checked = false;
                    checkBox4.Checked = true;
                }
                textBox7.Text = "";
                textBox8.Text = "";
                textBox9.Text = "0";
                textBox10.Text = "";
                textBox14.Text = "";
                textBox17.Text = "";
                //taxes
                textBox6.Text = "0";
                textBox19.Text = "0";
                textBox21.Text = "0";
                textBox22.Text = "0";
                textBox23.Text = "0";
                f12used = false;
                locked = false;
                RoffChanged = false;
                TaxChanged = false;
                DirectChangeAmount = false;
                dateTimePicker1.Focus();
                dtVoucherInfo.Rows.Add();
                ansGridView1.Rows[0].Cells["Quantity"].Value = 0;
                ansGridView1.Rows[0].Cells["Rate_am"].Value = 0;
                ansGridView1.Rows[0].Cells["cd"].Value = 0;
                ansGridView1.Rows[0].Cells["qd"].Value = 0;
                ansGridView1.Rows[0].Cells["Amount"].Value = 0;
                ansGridView1.Rows[0].Cells["Taxabelamount"].Value = 0;
                ansGridView1.Rows[0].Cells["Category_Id"].Value = 0;
            }

            dtItemCharges = new DataTable("ITEMCHARGES");
            Database.GetSqlData("SELECT ITEMCHARGES.*, CHARGES.Name AS ChargeName FROM ITEMCHARGES INNER JOIN CHARGES ON ITEMCHARGES.Charg_id = CHARGES.Ch_id where vi_id='" + vi_id + "' ", dtItemCharges);
            dtpaymentdet = new DataTable("voucherpaydet");
            Database.GetSqlData("select * from voucherpaydet where vi_id='" + vi_id + "' order by Itemsr ", dtpaymentdet);

            dtVoucherDet = new DataTable("voucherdet");
            Database.GetSqlData("select * from voucherdet where vi_id='" + vi_id + "' order by Itemsr", dtVoucherDet);


            for (int i = 0; i < dtVoucherDet.Rows.Count; i++)
            {
                ansGridView1.Rows.Add();
                DataTable dtPackName = new DataTable();

                if (Master.DescriptionInfo.Select("Des_id='" + dtVoucherDet.Rows[i]["Des_ac_id"] + "'", "").Length == 0)
                {
                    return;
                }
                else
                {
                    dtPackName = Master.DescriptionInfo.Select("Des_id='" + dtVoucherDet.Rows[i]["Des_ac_id"] + "' ", "").CopyToDataTable();
                }

                //ansGridView1.Rows[i].Cells["sno"].Value = i + 1;
                ansGridView1.Rows[i].Cells["sno"].Value = dtVoucherDet.Rows[i]["Itemsr"];
                ansGridView1.Rows[i].Cells["description"].Value = dtVoucherDet.Rows[i]["Description"];
                ansGridView1.Rows[i].Cells["Quantity"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet.Rows[i]["Quantity"].ToString()), 3);
                ansGridView1.Rows[i].Cells["comqty"].Value = dtVoucherDet.Rows[i]["comqty"];
                ansGridView1.Rows[i].Cells["Rate_am"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet.Rows[i]["Rate_am"].ToString()), 2);
                ansGridView1.Rows[i].Cells["Amount"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet.Rows[i]["Amount"].ToString()), 2);
                ansGridView1.Rows[i].Cells["Des_ac_id"].Value = dtVoucherDet.Rows[i]["Des_ac_id"];
                ansGridView1.Rows[i].Cells["Category_Id"].Value = dtVoucherDet.Rows[i]["Category_Id"];
                ansGridView1.Rows[i].Cells["Category"].Value = funs.Select_tax_cat_nm(dtVoucherDet.Rows[i]["Category_Id"].ToString());
                ansGridView1.Rows[i].Cells["Taxabelamount"].Value = dtVoucherDet.Rows[i]["Taxabelamount"];
                ansGridView1.Rows[i].Cells["Batch_Code"].Value = dtVoucherDet.Rows[i]["Batch_Code"];
                ansGridView1.Rows[i].Cells["Commission_per"].Value = dtVoucherDet.Rows[i]["Commission%"];
                ansGridView1.Rows[i].Cells["Rvi_id"].Value = dtVoucherDet.Rows[i]["Rvi_id"].ToString();
                ansGridView1.Rows[i].Cells["RItemsr"].Value = dtVoucherDet.Rows[i]["RItemsr"].ToString();
                ansGridView1.Rows[i].Cells["unt"].Value = dtVoucherDet.Rows[i]["packing"];
                ansGridView1.Rows[i].Cells["orgpack"].Value = dtVoucherDet.Rows[i]["orgpacking"];
                ansGridView1.Rows[i].Cells["pvalue"].Value = dtVoucherDet.Rows[i]["pvalue"];
                ansGridView1.Rows[i].Cells["rate_unit"].Value = dtVoucherDet.Rows[i]["Rate_unit"];
                ansGridView1.Rows[i].Cells["remark1"].Value = dtVoucherDet.Rows[i]["remark1"];
                ansGridView1.Rows[i].Cells["remark2"].Value = dtVoucherDet.Rows[i]["remark2"];
                ansGridView1.Rows[i].Cells["remark3"].Value = dtVoucherDet.Rows[i]["remark3"];
                ansGridView1.Rows[i].Cells["remark4"].Value = dtVoucherDet.Rows[i]["remark4"];

                if (dtVoucherDet.Rows[i]["remarkreq"].ToString() == "")
                {
                    dtVoucherDet.Rows[i]["remarkreq"] = false;
                }
                if (bool.Parse(dtVoucherDet.Rows[i]["remarkreq"].ToString()) == true)
                {
                    ansGridView1.Rows[i].Cells["remarkreq"].Value = "true";
                }
                else
                {
                    ansGridView1.Rows[i].Cells["remarkreq"].Value = "false";
                }
                if (dtVoucherDet.Rows[i]["godown_id"].ToString() == "" || dtVoucherDet.Rows[i]["godown_id"].ToString() == "0")
                {
                    if (gtype == "Transfer")
                    {
                        textBox13.Text = "<MAIN>";
                    }
                    else
                    {
                        ansGridView1.Rows[i].Cells["godown_id"].Value = "<MAIN>";
                    }
                }
                else
                {
                    if (gtype == "Transfer")
                    {
                        textBox13.Text = funs.Select_ac_nm(dtVoucherDet.Rows[i]["godown_id"].ToString());
                    }
                    else
                    {
                        ansGridView1.Rows[i].Cells["godown_id"].Value = funs.Select_ac_nm(dtVoucherDet.Rows[i]["godown_id"].ToString());
                    }
                }

                if (dtVoucherDet.Rows[i]["qd"].ToString() == "")
                {
                    dtVoucherDet.Rows[i]["qd"] = 0;
                }
                if (dtVoucherDet.Rows[i]["cd"].ToString() == "")
                {
                    dtVoucherDet.Rows[i]["cd"] = 0;
                }


                if (dtVoucherDet.Rows[i]["Square_FT"].ToString() == "")
                {
                    dtVoucherDet.Rows[i]["Square_FT"] = 1;
                }
                if (dtVoucherDet.Rows[i]["Square_MT"].ToString() == "")
                {
                    dtVoucherDet.Rows[i]["Square_MT"] = 1;
                }

                ansGridView1.Rows[i].Cells["sqft"].Value = double.Parse(dtVoucherDet.Rows[i]["Square_FT"].ToString());
                ansGridView1.Rows[i].Cells["sqmt"].Value = double.Parse(dtVoucherDet.Rows[i]["Square_MT"].ToString());

                ansGridView1.Rows[i].Cells["qd"].Value = dtVoucherDet.Rows[i]["qd"];
                ansGridView1.Rows[i].Cells["cd"].Value = dtVoucherDet.Rows[i]["cd"];
                ansGridView1.Rows[i].Cells["MRP"].Value = dtVoucherDet.Rows[i]["MRP"];
                ansGridView1.Rows[i].Cells["Cost"].Value = dtVoucherDet.Rows[i]["Cost"];
                ansGridView1.Rows[i].Cells["CommissionFix"].Value = dtVoucherDet.Rows[i]["Commission@"];
                ansGridView1.Rows[i].Cells["orgdesc"].Value = funs.Select_des_nm(dtVoucherDet.Rows[i]["Des_ac_id"].ToString());

                //new fields
                ansGridView1.Rows[i].Cells["pur_sale_acc"].Value = dtVoucherDet.Rows[i]["pur_sale_acc"];
                ansGridView1.Rows[i].Cells["tax1"].Value = dtVoucherDet.Rows[i]["tax1"];
                ansGridView1.Rows[i].Cells["tax2"].Value = dtVoucherDet.Rows[i]["tax2"];
                ansGridView1.Rows[i].Cells["tax3"].Value = dtVoucherDet.Rows[i]["tax3"];
                ansGridView1.Rows[i].Cells["tax4"].Value = dtVoucherDet.Rows[i]["tax4"];
                ansGridView1.Rows[i].Cells["rate1"].Value = dtVoucherDet.Rows[i]["rate1"];
                ansGridView1.Rows[i].Cells["rate2"].Value = dtVoucherDet.Rows[i]["rate2"];
                ansGridView1.Rows[i].Cells["rate3"].Value = dtVoucherDet.Rows[i]["rate3"];
                ansGridView1.Rows[i].Cells["rate4"].Value = dtVoucherDet.Rows[i]["rate4"];
                ansGridView1.Rows[i].Cells["taxamt1"].Value = dtVoucherDet.Rows[i]["taxamt1"];
                ansGridView1.Rows[i].Cells["taxamt2"].Value = dtVoucherDet.Rows[i]["taxamt2"];
                ansGridView1.Rows[i].Cells["taxamt3"].Value = dtVoucherDet.Rows[i]["taxamt3"];
                ansGridView1.Rows[i].Cells["taxamt4"].Value = dtVoucherDet.Rows[i]["taxamt4"];
                ansGridView1.Rows[i].Cells["bottomdis"].Value = dtVoucherDet.Rows[i]["bottomdis"];

                if (dtVoucherDet.Rows[i]["flatdis"].ToString() == "")
                {
                    dtVoucherDet.Rows[i]["flatdis"] = 0;
                }
                ansGridView1.Rows[i].Cells["flatdis"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet.Rows[i]["flatdis"].ToString()), 2);
                ansGridView1.Rows[i].Cells["Amount0"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet.Rows[i]["Amount0"].ToString()), 2);
                ansGridView1.Rows[i].Cells["QDType"].Value = dtVoucherDet.Rows[i]["QDType"].ToString();
                if (dtVoucherDet.Rows[i]["QDAmount"].ToString() == "")
                {
                    dtVoucherDet.Rows[i]["QDAmount"] = 0;
                }
                ansGridView1.Rows[i].Cells["QDAmount"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet.Rows[i]["QDAmount"].ToString()), 2);
                if (dtVoucherDet.Rows[i]["Amount1"].ToString() == "")
                {
                    dtVoucherDet.Rows[i]["Amount1"] = 0;
                }
                ansGridView1.Rows[i].Cells["Amount1"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet.Rows[i]["Amount1"].ToString()), 2);
                ansGridView1.Rows[i].Cells["CDType"].Value = dtVoucherDet.Rows[i]["CDType"].ToString();
                if (dtVoucherDet.Rows[i]["CDAmount"].ToString() == "")
                {
                    dtVoucherDet.Rows[i]["CDAmount"] = 0;
                }

                ansGridView1.Rows[i].Cells["CDAmount"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet.Rows[i]["CDAmount"].ToString()), 2);
                if (dtVoucherDet.Rows[i]["Amount2"].ToString() == "")
                {
                    dtVoucherDet.Rows[i]["Amount2"] = 0;
                }
                ansGridView1.Rows[i].Cells["Amount2"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet.Rows[i]["Amount2"].ToString()), 2);
                ansGridView1.Rows[i].Cells["FDType"].Value = dtVoucherDet.Rows[i]["FDType"].ToString();
                ansGridView1.Rows[i].Cells["FDAmount"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet.Rows[i]["FDAmount"].ToString()), 2);
                if (dtVoucherDet.Rows[i]["Amount3"].ToString() == "")
                {
                    dtVoucherDet.Rows[i]["Amount3"] = 0;
                }
                ansGridView1.Rows[i].Cells["Amount3"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet.Rows[i]["Amount3"].ToString()), 2);
                ansGridView1.Rows[i].Cells["GridDis"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet.Rows[i]["GridDis"].ToString()), 2);
                if (dtVoucherDet.Rows[i]["TotalDis"].ToString() == "")
                {
                    dtVoucherDet.Rows[i]["TotalDis"] = 0;
                }
                ansGridView1.Rows[i].Cells["TotalDis"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet.Rows[i]["TotalDis"].ToString()), 2);
                if (dtVoucherDet.Rows[i]["Amount4"].ToString() == "")
                {
                    dtVoucherDet.Rows[i]["Amount4"] = 0;
                }
                ansGridView1.Rows[i].Cells["Amount4"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet.Rows[i]["Amount4"].ToString()), 2);
                if (dtVoucherDet.Rows[i]["TotTaxPer"].ToString() == "")
                {
                    dtVoucherDet.Rows[i]["TotTaxPer"] = 0;
                }
                ansGridView1.Rows[i].Cells["TotTaxPer"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet.Rows[i]["TotTaxPer"].ToString()), 2);
                if (dtVoucherDet.Rows[i]["TotTaxAmount"].ToString() == "")
                {
                    dtVoucherDet.Rows[i]["TotTaxAmount"] = 0;
                }
                ansGridView1.Rows[i].Cells["TotTaxAmount"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet.Rows[i]["TotTaxAmount"].ToString()), 2);
                if (dtVoucherDet.Rows[i]["Amount5"].ToString() == "")
                {
                    dtVoucherDet.Rows[i]["Amount5"] = 0;
                }
                ansGridView1.Rows[i].Cells["Amount5"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet.Rows[i]["Amount5"].ToString()), 2);
                if (dtVoucherDet.Rows[i]["ExpAmount"].ToString() == "")
                {
                    dtVoucherDet.Rows[i]["ExpAmount"] = 0;
                }
                ansGridView1.Rows[i].Cells["ExpAmount"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet.Rows[i]["ExpAmount"].ToString()), 2);
                ansGridView1.Rows[i].Cells["dattype"].Value = dtVoucherDet.Rows[i]["dattype"].ToString();
                if (dtVoucherDet.Rows[i]["datamount"].ToString() == "")
                {
                    dtVoucherDet.Rows[i]["datamount"] = 0;
                }
                ansGridView1.Rows[i].Cells["datamount"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet.Rows[i]["datamount"].ToString()), 2);
                if (dtVoucherDet.Rows[i]["dat"].ToString() == "")
                {
                    dtVoucherDet.Rows[i]["dat"] = 0;
                }
                ansGridView1.Rows[i].Cells["dat"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet.Rows[i]["dat"].ToString()), 2);
                ansGridView1.Rows[i].Cells["datac_id"].Value = dtVoucherDet.Rows[i]["datac_id"].ToString();
                ansGridView1.Rows[i].Cells["RCMac_id"].Value = dtVoucherDet.Rows[i]["RCMac_id"].ToString();

                ItemCalc(i);
            }

            dtStock = new DataTable("Stock");
            Database.GetSqlData("select * from Stock where Vid='" + vi_id + "' ", dtStock);

            dtRoundOff = new DataTable("VouCharges");
            Database.GetSqlData("select * from VouCharges where vi_id='" + vi_id + "' and Entry_typ=4", dtRoundOff);


            dtJournal = new DataTable("Journal");
            Database.GetSqlData("select * from Journal where vi_id='" + vi_id + "' ", dtJournal);

            dtVoucherCharges1 = new DataTable("VouCharges");
            Database.GetSqlData("select * from VouCharges where vi_id='" + vi_id + "' and Entry_typ=1 order by Srno", dtVoucherCharges1);

            for (int i = 0; i < dtVoucherCharges1.Rows.Count; i++)
            {
                ansGridView3.Rows.Add();
                ansGridView3.Rows[i].Cells["sno2"].Value = dtVoucherCharges1.Rows[i]["Srno"];
                ansGridView3.Rows[i].Cells["Charg_Name"].Value = dtVoucherCharges1.Rows[i]["Charg_Name"];
                ansGridView3.Rows[i].Cells["Charg_id1"].Value = dtVoucherCharges1.Rows[i]["Charg_id"];
                ansGridView3.Rows[i].Cells["AmountA"].Value = funs.DecimalPoint(dtVoucherCharges1.Rows[i]["Amount"], 2);
                ansGridView3.Rows[i].Cells["CamountA"].Value = funs.DecimalPoint(dtVoucherCharges1.Rows[i]["Camount"], 2);
                ansGridView3.Rows[i].Cells["Accid1"].Value = dtVoucherCharges1.Rows[i]["Accid"];
                ansGridView3.Rows[i].Cells["Addsub1"].Value = dtVoucherCharges1.Rows[i]["Addsub"];
                ansGridView3.Rows[i].Cells["Ctype1"].Value = dtVoucherCharges1.Rows[i]["Ctype"];
                ansGridView3.Rows[i].Cells["Changed1"].Value = dtVoucherCharges1.Rows[i]["Changed"].ToString();
            }

            dtVoucherCharges2 = new DataTable("VouCharges");
            Database.GetSqlData("select * from VouCharges where vi_id='" + vi_id + "' and Entry_typ=3 and Srno<>0 order by Srno", dtVoucherCharges2);

            for (int i = 0; i < dtVoucherCharges2.Rows.Count; i++)
            {
                ansGridView4.Rows.Add();
                ansGridView4.Rows[i].Cells["sno3"].Value = dtVoucherCharges2.Rows[i]["Srno"];
                ansGridView4.Rows[i].Cells["Charg_Name2"].Value = dtVoucherCharges2.Rows[i]["Charg_Name"];
                ansGridView4.Rows[i].Cells["Charg_id2"].Value = dtVoucherCharges2.Rows[i]["Charg_id"];
                ansGridView4.Rows[i].Cells["AmountB"].Value = funs.DecimalPoint(dtVoucherCharges2.Rows[i]["Amount"], 2);
                ansGridView4.Rows[i].Cells["CamountB"].Value = funs.DecimalPoint(dtVoucherCharges2.Rows[i]["Camount"], 2);
                ansGridView4.Rows[i].Cells["Accid2"].Value = dtVoucherCharges2.Rows[i]["Accid"];
                ansGridView4.Rows[i].Cells["Addsub2"].Value = dtVoucherCharges2.Rows[i]["Addsub"];
                ansGridView4.Rows[i].Cells["Ctype2"].Value = dtVoucherCharges2.Rows[i]["Ctype"];
                ansGridView4.Rows[i].Cells["Changed2"].Value = dtVoucherCharges2.Rows[i]["Changed"].ToString();
            }

            dtVoucherCharges3 = new DataTable("VouCharges");
            Database.GetSqlData("select * from VouCharges where Vi_id='" + vi_id + "' and Entry_typ=2 and Srno <> 0", dtVoucherCharges3);

            dtVoucherCharges4 = new DataTable("VouCharges");
            Database.GetSqlData("select * from VouCharges where Vi_id='" + vi_id + "' and Entry_typ=5 and Srno <> 0", dtVoucherCharges4);

            if (gCalculationType == "Including Tax Only")
            {
                checkBox1.Enabled = false;
            }
            else if (gCalculationType == "Excluding Tax Only")
            {
                checkBox1.Enabled = false;
            }
            else if (gCalculationType == "Default Excluding Tax")
            {
                checkBox1.Enabled = true;
            }
            else if (gCalculationType == "Default Including Tax")
            {
                checkBox1.Enabled = true;
            }
            if (gExcludingTax == true)
            {
                checkBox1.Checked = false;
            }
            else
            {
                checkBox1.Checked = true;
            }
            string stateid = "0";
            stateid = funs.Select_state_id(shiptostate);
            if (stateid == "0")
            {
                stateid = Database.CompanyState_id;
            }
            if (Database.CompanyState_id == stateid)
            {
                gExState = false;
            }
            else
            {
                gExState = true;
            }

            dtDisp.Clear();
            dtDisp.Columns.Clear();
            dtDisp.Columns.Add("desc");
            dtDisp.Columns.Add("qty");
            dtDisp.Columns.Add("name");
            dtDisp.Columns.Add("dis");
            dtDisp.Columns.Add("amt");

            labelCalc();
            if (Feature.Available("Enable Order Management") == "Yes")
            {
                if (vid != "")
                {
                    textBox14.ReadOnly = true;
                    textBox14.Enabled = false;
                }
            }
        }

        private void SetAgent(string str)
        {
            if (gtype != "Transfer" && gtype != "Opening")
            {
                //try
                //{
                //    textBox17.Text = Master.Accountinfo.Select("Name='" + str + "' ", "").FirstOrDefault()["Agent"].ToString();
                //}
                //catch (Exception ex)
                //{
                //    Master.UpdateAccountinfo();
                //    textBox17.Text = Master.Accountinfo.Select("Name='" + str + "' ", "").FirstOrDefault()["Agent"].ToString();
                //}
                textBox17.Text = funs.Select_ac_nm(Database.GetScalarText("Select Con_id from Account where Name='" + textBox14.Text + "'"));
            }
        }

        private void SetSalesMAn(string str)
        {
            if (gtype != "Transfer" && gtype != "Opening")
            {
                try
                {
                    textBox28.Text = Master.Accountinfo.Select("Name='" + str + "' ", "").FirstOrDefault()["SalesMan"].ToString();
                }
                catch (Exception ex)
                {
                    Master.UpdateAccountinfo();
                    textBox28.Text = Master.Accountinfo.Select("Name='" + str + "' ", "").FirstOrDefault()["SalesMan"].ToString();
                }
            }
        }

        private string SelectGroupid(string str)
        {
            return funs.Select_Groupid(str);
        }

        private void SetDuedate(string str)
        {
            if (gtype == "Sale")
            {
                int da = funs.Select_ac_dlimit(str);
                dateTimePicker3.Value = dateTimePicker1.Value.AddDays(da);
            }
            else if (gtype == "Purchase")
            {
                int da = funs.Select_ac_dlimit(str);
                dateTimePicker3.Value = dateTimePicker2.Value.AddDays(da);
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

                dttemp = new DataTable("journal");
                Database.GetSqlData("Select * from journal where vi_id='" + vid + "' ", dttemp);
                for (int i = 0; i < dttemp.Rows.Count; i++)
                {
                    dttemp.Rows[i].Delete();
                }
                Database.SaveData(dttemp);

                dttemp = new DataTable("Stock");
                Database.GetSqlData("Select * from Stock where vid='" + vid + "' ", dttemp);
                for (int i = 0; i < dttemp.Rows.Count; i++)
                {
                    dttemp.Rows[i].Delete();
                }
                Database.SaveData(dttemp);

                dttemp = new DataTable("VouCharges");
                Database.GetSqlData("Select * from VouCharges where vi_id='" + vid + "' ", dttemp);
                for (int i = 0; i < dttemp.Rows.Count; i++)
                {
                    dttemp.Rows[i].Delete();
                }
                Database.SaveData(dttemp);

                dttemp = new DataTable("ITEMCHARGES");
                Database.GetSqlData("Select * from ITEMCHARGES where vi_id='" + vid + "' ", dttemp);
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
                dttemp = new DataTable("BILLADJEST");
                Database.GetSqlData("Select * from BILLADJEST where Vi_id='" + vid + "' ", dttemp);
                for (int i = 0; i < dttemp.Rows.Count; i++)
                {
                    dttemp.Rows[i].Delete();
                }
                Database.SaveData(dttemp);

                dttemp = new DataTable("BILLADJEST");
                Database.GetSqlData("Select * from BILLADJEST where reff_id='" + vid + "' ", dttemp);
                for (int i = 0; i < dttemp.Rows.Count; i++)
                {
                    dttemp.Rows[i].Delete();
                }
                Database.SaveData(dttemp);


                dttemp = new DataTable("Voucherpaydet");
                Database.GetSqlData("Select * from Voucherpaydet where vi_id='" + vid + "' ", dttemp);
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

        private double DisplayStock(string descid)
        {
            bool marked = Database.GetScalarBool("Select A from Vouchertype where Vt_id='" + vtid + "' ");
            if (marked == false)
            {
                marked = true;
            }
            else
            {
                marked = false;
            }

            //  double Stock = Database.GetScalarDecimal("SELECT res.Stok AS SumOfStok FROM (SELECT  Stock.Did, SUM(Stock.Receive - Stock.Issue) AS stok FROM  VOUCHERTYPE RIGHT OUTER JOIN  VOUCHERINFO ON VOUCHERTYPE.Vt_id = VOUCHERINFO.Vt_id RIGHT OUTER JOIN  Stock LEFT OUTER JOIN   Description ON Stock.Did = Description.Des_id ON VOUCHERINFO.Vi_id = Stock.Vid WHERE  (VOUCHERINFO.Vdate <= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERTYPE." + Database.BMode + " = 1) OR  (VOUCHERINFO.Vdate IS NULL) GROUP BY Stock.Did)  AS res WHERE (((res.Did)='" + descid + "')) GROUP BY res.Stok");
            double Stock = Database.GetScalarDecimal("SELECT stok AS SumOfStok FROM (SELECT  Stock.Did, SUM( Stock.Receive -  Stock.Issue) AS stok  FROM VOUCHERTYPE RIGHT OUTER JOIN  VOUCHERINFO ON  VOUCHERTYPE.Vt_id =  VOUCHERINFO.Vt_id RIGHT OUTER JOIN  Stock LEFT OUTER JOIN  Description ON  Stock.Did =  Description.Des_id ON  VOUCHERINFO.Vi_id =  Stock.Vid  WHERE  ( VOUCHERINFO.Vdate <= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND ( VOUCHERINFO.Branch_id = '" + Database.BranchId + "') AND (VOUCHERTYPE.AB= 1) OR   ( VOUCHERINFO.Vdate IS NULL)  GROUP BY  Stock.Did) AS res WHERE (Did = '" + descid + "') GROUP BY stok");
            double last_purchase_rate = Database.GetScalarDecimal("SELECT VOUCHERDET.Rate_am FROM (VOUCHERINFO LEFT JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERDET.Des_ac_id)='" + descid + "') AND ((VOUCHERTYPE.Type)='Purchase')) ORDER BY VOUCHERDET.Des_ac_id DESC");
            double war = Database.GetScalarDecimal("SELECT DESCRIPTION.Wlavel FROM DESCRIPTION WHERE (((DESCRIPTION.Des_id)='" + descid + "'))");

            label20.Text = Stock.ToString();
              textBox11.Text = last_purchase_rate.ToString();

            if (funs.Select_des_stkMaintain(descid) == true)
            {
                groupBox17.Visible = true;
            }
            else
            {
                groupBox17.Visible = false;
            }
            return Stock;
        }
        private void view()
        {
            frm_printcopy frm = new frm_printcopy("View", vid, vtid);
            frm.ShowDialog();
        }
        private void Print()
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

        private void save()
        {

            bool A = Database.GetScalarBool("Select A from Vouchertype where vt_id='" + vtid + "'");
            bool B = Database.GetScalarBool("Select B from Vouchertype where vt_id='" + vtid + "'");
            bool AB = Database.GetScalarBool("Select AB from Vouchertype where vt_id='" + vtid + "'");

            string prefix = "";
            string postfix = "";
            int padding = 0;

            prefix = Database.GetScalarText("Select prefix from Vouchertype where vt_id='" + vtid + "' ");
            postfix = Database.GetScalarText("Select postfix from Vouchertype where vt_id='" + vtid + "' ");
            padding = Database.GetScalarInt("Select padding from Vouchertype where vt_id='" + vtid + "' ");

            string narr = SetNarr();
            SetVno();
            if (vno == 0)
            {
                vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
            }

            //Voucher Info

            if (vid == "")
            {
                DataTable dtCount = new DataTable();
                Database.GetSqlData("select count(*) from VOUCHERINFO where locationid='" + Database.LocationId + "'", dtCount);

                if (int.Parse(dtCount.Rows[0][0].ToString()) == 0)
                {

                    vid = Database.LocationId + "1";
                    dtVoucherInfo.Rows[0]["Vi_id"] = vid;
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

                    vid = Database.LocationId + (Nid + 1);
                    dtVoucherInfo.Rows[0]["Vi_id"] = vid;
                    dtVoucherInfo.Rows[0]["Nid"] = (Nid + 1);
                    dtVoucherInfo.Rows[0]["LocationId"] = Database.LocationId;
                    dtVoucherInfo.Rows[0]["user_id"] = Database.user_id;
                    dtVoucherInfo.Rows[0]["Approvedby"] = "";
                    dtVoucherInfo.Rows[0]["Modifiedby"] = "";
                    Prelocationid = Database.LocationId;
                }
            }
            else
            {
                dtVoucherInfo.Rows[0]["Approvedby"] = "";
                dtVoucherInfo.Rows[0]["Modifiedby"] = Database.user_id;
            }
            string invoiceno = vno.ToString();
            dtVoucherInfo.Rows[0]["Invoiceno"] = prefix + invoiceno.PadLeft(padding, '0') + postfix;
            dtVoucherInfo.Rows[0]["Vt_id"] = vtid;
            dtVoucherInfo.Rows[0]["Vnumber"] = vno;
            dtVoucherInfo.Rows[0]["s_id"] = funs.Select_salesman_id(textBox28.Text);
            dtVoucherInfo.Rows[0]["Transporter_id"] = funs.Select_ac_id(TransportName);
            dtVoucherInfo.Rows[0]["ac_id"] = funs.Select_ac_id(textBox14.Text);
            dtVoucherInfo.Rows[0]["Ac_id2"] = shiptoacc_id;
            dtVoucherInfo.Rows[0]["Vdate"] = dateTimePicker1.Value.Date;
            dtVoucherInfo.Rows[0]["Svdate"] = dateTimePicker2.Value.Date.ToString("dd-MMM-yyyy");
            dtVoucherInfo.Rows[0]["Duedate"] = dateTimePicker3.Value.Date.ToString("dd-MMM-yyyy");
            dtVoucherInfo.Rows[0]["Narr"] = narr;
            dtVoucherInfo.Rows[0]["Reffno"] = "";
            dtVoucherInfo.Rows[0]["ShiptoDistance"] = Distance;
            dtVoucherInfo.Rows[0]["TaxableAmount"] = textBox24.Text;
            dtVoucherInfo.Rows[0]["Totalamount"] = textBox10.Text;
            dtVoucherInfo.Rows[0]["rate"] = textBox12.Text;
            dtVoucherInfo.Rows[0]["Roff"] = textBox9.Text;
            dtVoucherInfo.Rows[0]["Tdtype"] = gExcludingTax;
            dtVoucherInfo.Rows[0]["DirectChanged"] = DirectChangeAmount;
            dtVoucherInfo.Rows[0]["Branch_id"] = Database.BranchId;
            dtVoucherInfo.Rows[0]["dispatch_id"] = disfromacc_id;
            dtVoucherInfo.Rows[0]["EwayBillno"] = EwayBillno;
            dtVoucherInfo.Rows[0]["transdocno"] = Transdocno;
            if (Transdocno == "")
            {
                dtVoucherInfo.Rows[0]["Transdocdate"] = dateTimePicker1.Value.Date.ToString(Database.dformat);
            }
            else
            {

                dtVoucherInfo.Rows[0]["Transdocdate"] = Transdocdate;
            }
            dtVoucherInfo.Rows[0]["TransVehNo"] = Vehicleno;
            if (radioButton10.Checked == true)
            {
                dtVoucherInfo.Rows[0]["Sq_FT_MT"] = radioButton10.Text;
            }

            if (radioButton9.Checked == true)
            {
                dtVoucherInfo.Rows[0]["Sq_FT_MT"] = radioButton9.Text;
            }


            if (radioButton7.Checked == true)
            {
                dtVoucherInfo.Rows[0]["CashCredit"] = "Credit";
            }
            else
            {
                dtVoucherInfo.Rows[0]["CashCredit"] = "Cash";
            }

            if (gtype == "RCM")
            {
                if (checkBox2.Checked == true)
                {
                    dtVoucherInfo.Rows[0]["RCM"] = true;
                }
                else
                {
                    dtVoucherInfo.Rows[0]["RCM"] = false;
                }
                if (checkBox3.Checked == true)
                {
                    dtVoucherInfo.Rows[0]["5000Allowed"] = true;
                }
                else
                {
                    dtVoucherInfo.Rows[0]["5000Allowed"] = false;
                }

                if (checkBox4.Checked == true)
                {
                    dtVoucherInfo.Rows[0]["ITC"] = true;
                }
                else
                {
                    dtVoucherInfo.Rows[0]["ITC"] = false;
                }
            }
            else
            {
                dtVoucherInfo.Rows[0]["RCM"] = false;
                dtVoucherInfo.Rows[0]["5000Allowed"] = false;
                dtVoucherInfo.Rows[0]["ITC"] = false;
            }
            if (gExState == true)
            {
                dtVoucherInfo.Rows[0]["Formno"] = textBox3.Text;
            }

            dtVoucherInfo.Rows[0]["RoffChanged"] = RoffChanged;
            dtVoucherInfo.Rows[0]["TaxChanged"] = TaxChanged;
            dtVoucherInfo.Rows[0]["Svnum"] = textBox2.Text;

            TextBox tbx1 = this.Controls.Find(Master.TransportDetails.Rows[0]["FName"].ToString(), true).FirstOrDefault() as TextBox;
            field1 = tbx1.Text;
            dtVoucherInfo.Rows[0]["Transport1"] = field1;

            TextBox tbx2 = this.Controls.Find(Master.TransportDetails.Rows[1]["FName"].ToString(), true).FirstOrDefault() as TextBox;
            field2 = tbx2.Text;
            dtVoucherInfo.Rows[0]["Transport2"] = field2;

            TextBox tbx3 = this.Controls.Find(Master.TransportDetails.Rows[3]["FName"].ToString(), true).FirstOrDefault() as TextBox;
            field4 = tbx3.Text;
            dtVoucherInfo.Rows[0]["Grno"] = field4;

            TextBox tbx4 = this.Controls.Find(Master.TransportDetails.Rows[2]["FName"].ToString(), true).FirstOrDefault() as TextBox;
            field3 = tbx4.Text;
            dtVoucherInfo.Rows[0]["DeliveryAt"] = field3;

            TextBox tbx5 = this.Controls.Find(Master.TransportDetails.Rows[4]["FName"].ToString(), true).FirstOrDefault() as TextBox;
            field5 = tbx5.Text;
            dtVoucherInfo.Rows[0]["Transport3"] = field5;

            TextBox tbx6 = this.Controls.Find(Master.TransportDetails.Rows[5]["FName"].ToString(), true).FirstOrDefault() as TextBox;
            field6 = tbx6.Text;
            dtVoucherInfo.Rows[0]["Transport4"] = field6;

            TextBox tbx7 = this.Controls.Find(Master.TransportDetails.Rows[6]["FName"].ToString(), true).FirstOrDefault() as TextBox;
            field7 = tbx7.Text;
            dtVoucherInfo.Rows[0]["Transport5"] = field7;

            TextBox tbx8 = this.Controls.Find(Master.TransportDetails.Rows[7]["FName"].ToString(), true).FirstOrDefault() as TextBox;
            field8 = tbx8.Text;
            dtVoucherInfo.Rows[0]["Transport6"] = field8;


            dtVoucherInfo.Rows[0]["ShiptoPincode"] = shiptoPincode;
            dtVoucherInfo.Rows[0]["ShiptoAddress1"] = shiptoaddress1;
            dtVoucherInfo.Rows[0]["ShiptoAddress2"] = shiptoaddress2;
            dtVoucherInfo.Rows[0]["ShiptoEmail"] = shiptoemail;
            dtVoucherInfo.Rows[0]["ShiptoTIN"] = shiptotin;
            dtVoucherInfo.Rows[0]["ShiptoPhone"] = shiptocontact;
            dtVoucherInfo.Rows[0]["ShiptoStateid"] = funs.Select_state_id(shiptostate);
            dtVoucherInfo.Rows[0]["Shipto"] = shiptoprint;
            dtVoucherInfo.Rows[0]["ShiptoPAN"] = shiptoPan;
            dtVoucherInfo.Rows[0]["ShiptoAadhar"] = shiptoAadhar;
            dtVoucherInfo.Rows[0]["FormC"] = formC;
            dtVoucherInfo.Rows[0]["Conn_id"] = funs.Select_ac_id(textBox17.Text);
            dtVoucherInfo.Rows[0]["Iscancel"] = false;
            dtVoucherInfo.Rows[0]["ShiptoCity_id"] = shiptocityid;
            if (gStr == "")
            {
                dtVoucherInfo.Rows[0]["CreTime"] = System.DateTime.Now.ToString("HH:mm:ss");
            }
            dtVoucherInfo.Rows[0]["ModTime"] = System.DateTime.Now.ToString("HH:mm:ss");

            if (Database.utype.ToUpper() == "USER")
            {
                dtVoucherInfo.Rows[0]["NApproval"] = true;
            }
            else
            {
                dtVoucherInfo.Rows[0]["NApproval"] = false;
            }

            if (radioButton7.Checked == true)
            {
                dtVoucherInfo.Rows[0]["Cash_Pending"] = false;
            }
            else
            {
                dtVoucherInfo.Rows[0]["Cash_Pending"] = true;
            }


            if (gtype == "Sale" || gtype == "P Return" || gtype == "JIssue")
            {
                dtVoucherInfo.Rows[0]["dr_ac_id"] = funs.Select_ac_id(textBox14.Text);
                dtVoucherInfo.Rows[0]["cr_ac_id"] = ansGridView1.Rows[0].Cells["pur_sale_acc"].Value.ToString();
            }
            else if (gtype == "Purchase" || gtype == "Return" || gtype == "RCM")
            {
                dtVoucherInfo.Rows[0]["cr_ac_id"] = funs.Select_ac_id(textBox14.Text);
                dtVoucherInfo.Rows[0]["dr_ac_id"] = ansGridView1.Rows[0].Cells["pur_sale_acc"].Value.ToString();
            }

            dtVoucherInfo.Rows[0]["Cashier_approved"] = false;
            dtVoucherInfo.Rows[0]["Cashier_id"] = "";
            dtVoucherInfo.Rows[0]["Approved"] = false;

            Database.SaveData(dtVoucherInfo);

            DataTable dtTemp = new DataTable("VOUCHERDET");
            Database.GetSqlData("select * from VOUCHERDET where vi_id='" + vid + "' ", dtTemp);
            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                dtTemp.Rows[i].Delete();
            }
            Database.SaveData(dtTemp);

            dtVoucherDet = new DataTable("VOUCHERDET");
            Database.GetSqlData("select * from VOUCHERDET where vi_id='" + vid + "' ", dtVoucherDet);
            //voucherDetails

            double totalqdamount = 0;
            double totalcdamount = 0;
            double totalfixamount = 0;

            for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
            {
                totalqdamount += double.Parse(ansGridView1.Rows[i].Cells["QDAmount"].Value.ToString());
                totalcdamount += double.Parse(ansGridView1.Rows[i].Cells["CDAmount"].Value.ToString());
                totalfixamount += double.Parse(ansGridView1.Rows[i].Cells["FDAmount"].Value.ToString());

                dtVoucherDet.Rows.Add();
                dtVoucherDet.Rows[i]["vi_id"] = vid;
                dtVoucherDet.Rows[i]["LocationId"] = Prelocationid;
                dtVoucherDet.Rows[i]["Itemsr"] = ansGridView1.Rows[i].Cells["sno"].Value.ToString();
                dtVoucherDet.Rows[i]["Description"] = ansGridView1.Rows[i].Cells["description"].Value.ToString();
                dtVoucherDet.Rows[i]["Quantity"] = ansGridView1.Rows[i].Cells["Quantity"].Value.ToString();
                dtVoucherDet.Rows[i]["comqty"] = ansGridView1.Rows[i].Cells["comqty"].Value.ToString();
                dtVoucherDet.Rows[i]["Rate_am"] = ansGridView1.Rows[i].Cells["Rate_am"].Value.ToString();
                dtVoucherDet.Rows[i]["Amount"] = ansGridView1.Rows[i].Cells["Amount"].Value.ToString();
                dtVoucherDet.Rows[i]["Des_ac_id"] = ansGridView1.Rows[i].Cells["Des_ac_id"].Value.ToString();
                dtVoucherDet.Rows[i]["Category_Id"] = ansGridView1.Rows[i].Cells["Category_Id"].Value.ToString();
                ansGridView1.Rows[i].Cells["Category"].Value = funs.Select_tax_cat_nm(dtVoucherDet.Rows[i]["Category_Id"].ToString());
                dtVoucherDet.Rows[i]["Taxabelamount"] = ansGridView1.Rows[i].Cells["Taxabelamount"].Value.ToString();
                dtVoucherDet.Rows[i]["Rvi_id"] = ansGridView1.Rows[i].Cells["Rvi_id"].Value.ToString();
                dtVoucherDet.Rows[i]["RItemsr"] = ansGridView1.Rows[i].Cells["RItemsr"].Value.ToString();
                if (ansGridView1.Rows[i].Cells["Batch_Code"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["Batch_Code"].Value = "";
                }
                if (Feature.Available("Batch Number") == "Yes")
                {
                    dtVoucherDet.Rows[i]["Batch_Code"] = ansGridView1.Rows[i].Cells["Batch_Code"].Value.ToString();
                }
                dtVoucherDet.Rows[i]["Batch_Code"] = ansGridView1.Rows[i].Cells["Batch_Code"].Value.ToString();
                if (ansGridView1.Rows[i].Cells["Commission_per"].Value.ToString() == "")
                {
                    ansGridView1.Rows[i].Cells["Commission_per"].Value = 0;
                }
                dtVoucherDet.Rows[i]["Commission%"] = ansGridView1.Rows[i].Cells["Commission_per"].Value.ToString();
                dtVoucherDet.Rows[i]["packing"] = ansGridView1.Rows[i].Cells["unt"].Value.ToString();
                dtVoucherDet.Rows[i]["orgpacking"] = ansGridView1.Rows[i].Cells["orgpack"].Value.ToString();
                dtVoucherDet.Rows[i]["pvalue"] = ansGridView1.Rows[i].Cells["pvalue"].Value.ToString();
                dtVoucherDet.Rows[i]["Rate_unit"] = ansGridView1.Rows[i].Cells["rate_unit"].Value.ToString();
                if (ansGridView1.Rows[i].Cells["remark1"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["remark1"].Value = "";
                }
                dtVoucherDet.Rows[i]["remark1"] = ansGridView1.Rows[i].Cells["remark1"].Value.ToString();
                if (ansGridView1.Rows[i].Cells["remark2"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["remark2"].Value = "";
                }
                dtVoucherDet.Rows[i]["remark2"] = ansGridView1.Rows[i].Cells["remark2"].Value.ToString();
                if (ansGridView1.Rows[i].Cells["remark3"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["remark3"].Value = "";
                }
                dtVoucherDet.Rows[i]["remark3"] = ansGridView1.Rows[i].Cells["remark3"].Value.ToString();
                if (ansGridView1.Rows[i].Cells["remark4"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["remark4"].Value = "";
                }
                dtVoucherDet.Rows[i]["remark4"] = ansGridView1.Rows[i].Cells["remark4"].Value.ToString();
                if (ansGridView1.Rows[i].Cells["remarkreq"].Value.ToString() == "true")
                {
                    dtVoucherDet.Rows[i]["remarkreq"] = "true";
                }
                else
                {
                    dtVoucherDet.Rows[i]["remarkreq"] = "false";
                }
                dtVoucherDet.Rows[i]["Type"] = "0";
                if (ansGridView1.Rows[i].Cells["flatdis"].Value == null || ansGridView1.Rows[i].Cells["flatdis"].Value.ToString() == "")
                {
                    ansGridView1.Rows[i].Cells["flatdis"].Value = 0;
                }
                dtVoucherDet.Rows[i]["flatdis"] = double.Parse(ansGridView1.Rows[i].Cells["flatdis"].Value.ToString());
                if (gtype == "Transfer")
                {
                    dtVoucherDet.Rows[i]["godown_id"] = funs.Select_ac_id(textBox13.Text);
                }
                else
                {
                    string gridgod = "";
                    gridgod = funs.Select_ac_id(ansGridView1.Rows[i].Cells["godown_id"].Value.ToString());
                    //if(gridgod=="")
                    //{
                    //    gridgod = "0";
                    //}
                    dtVoucherDet.Rows[i]["godown_id"] = gridgod;
                }

                if (gtype == "Opening")
                {
                    dtVoucherDet.Rows[i]["godown_id"] = funs.Select_ac_id(textBox14.Text);
                }

                if (ansGridView1.Rows[i].Cells["qd"].ToString() == "")
                {
                    ansGridView1.Rows[i].Cells["qd"].Value = 0;
                }
                if (ansGridView1.Rows[i].Cells["cd"].ToString() == "")
                {
                    ansGridView1.Rows[i].Cells["qd"].Value = 0;
                }
                dtVoucherDet.Rows[i]["qd"] = ansGridView1.Rows[i].Cells["qd"].Value.ToString();
                dtVoucherDet.Rows[i]["cd"] = ansGridView1.Rows[i].Cells["cd"].Value.ToString();


                if (ansGridView1.Rows[i].Cells["sqft"].ToString() == "")
                {
                    ansGridView1.Rows[i].Cells["sqft"].Value = 1;
                }
                if (ansGridView1.Rows[i].Cells["sqmt"].ToString() == "")
                {
                    ansGridView1.Rows[i].Cells["sqmt"].Value = 0;
                }
                dtVoucherDet.Rows[i]["Square_FT"] = ansGridView1.Rows[i].Cells["sqft"].Value.ToString();
                dtVoucherDet.Rows[i]["Square_MT"] = ansGridView1.Rows[i].Cells["sqmt"].Value.ToString();


                if (ansGridView1.Rows[i].Cells["cost"].Value.ToString() == "")
                {
                    ansGridView1.Rows[i].Cells["cost"].Value = 0;
                }
                if (ansGridView1.Rows[i].Cells["MRP"].Value.ToString() == "" || ansGridView1.Rows[i].Cells["MRP"].Value.ToString() == null)
                {
                    ansGridView1.Rows[i].Cells["MRP"].Value = 0;
                }
                if (ansGridView1.Rows[i].Cells["weight"].Value == null || ansGridView1.Rows[i].Cells["weight"].Value.ToString() == "")
                {
                    ansGridView1.Rows[i].Cells["weight"].Value = 0;
                }
                if (ansGridView1.Rows[i].Cells["dattype"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["dattype"].Value = "";
                }
                dtVoucherDet.Rows[i]["dattype"] = ansGridView1.Rows[i].Cells["dattype"].Value.ToString();
                dtVoucherDet.Rows[i]["datamount"] = double.Parse(ansGridView1.Rows[i].Cells["datamount"].Value.ToString());
                dtVoucherDet.Rows[i]["dat"] = double.Parse(ansGridView1.Rows[i].Cells["dat"].Value.ToString());
                if (ansGridView1.Rows[i].Cells["datac_id"].Value == null || ansGridView1.Rows[i].Cells["datac_id"].Value.ToString() == "")
                {
                    ansGridView1.Rows[i].Cells["datac_id"].Value = 0;
                }
                dtVoucherDet.Rows[i]["datac_id"] = ansGridView1.Rows[i].Cells["datac_id"].Value.ToString();
                if (ansGridView1.Rows[i].Cells["RCMac_id"].Value == null || ansGridView1.Rows[i].Cells["RCMac_id"].Value.ToString() == "")
                {
                    ansGridView1.Rows[i].Cells["RCMac_id"].Value = 0;
                }
                dtVoucherDet.Rows[i]["RCMac_id"] = ansGridView1.Rows[i].Cells["RCMac_id"].Value.ToString();
                dtVoucherDet.Rows[i]["weight"] = ansGridView1.Rows[i].Cells["weight"].Value.ToString();
                dtVoucherDet.Rows[i]["Cost"] = ansGridView1.Rows[i].Cells["cost"].Value.ToString();
                dtVoucherDet.Rows[i]["MRP"] = ansGridView1.Rows[i].Cells["MRP"].Value.ToString();
                if (ansGridView1.Rows[i].Cells["CommissionFix"].Value.ToString() == "" || ansGridView1.Rows[i].Cells["CommissionFix"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["CommissionFix"].Value = 0;
                }
                dtVoucherDet.Rows[i]["Commission@"] = ansGridView1.Rows[i].Cells["CommissionFix"].Value.ToString();

                //new fields
                if (ansGridView1.Rows[i].Cells["pur_sale_acc"].Value.ToString() == "" || ansGridView1.Rows[i].Cells["pur_sale_acc"].Value.ToString() == null)
                {
                    ansGridView1.Rows[i].Cells["pur_sale_acc"].Value = "";
                }
                dtVoucherDet.Rows[i]["pur_sale_acc"] = ansGridView1.Rows[i].Cells["pur_sale_acc"].Value.ToString();
                dtVoucherDet.Rows[i]["tax1"] = ansGridView1.Rows[i].Cells["tax1"].Value.ToString();
                dtVoucherDet.Rows[i]["tax2"] = ansGridView1.Rows[i].Cells["tax2"].Value.ToString();
                dtVoucherDet.Rows[i]["tax3"] = ansGridView1.Rows[i].Cells["tax3"].Value.ToString();
                dtVoucherDet.Rows[i]["tax4"] = ansGridView1.Rows[i].Cells["tax4"].Value.ToString();
                dtVoucherDet.Rows[i]["rate1"] = double.Parse(ansGridView1.Rows[i].Cells["rate1"].Value.ToString());
                dtVoucherDet.Rows[i]["rate2"] = double.Parse(ansGridView1.Rows[i].Cells["rate2"].Value.ToString());
                dtVoucherDet.Rows[i]["rate3"] = double.Parse(ansGridView1.Rows[i].Cells["rate3"].Value.ToString());
                dtVoucherDet.Rows[i]["rate4"] = double.Parse(ansGridView1.Rows[i].Cells["rate4"].Value.ToString());
                dtVoucherDet.Rows[i]["taxamt1"] = double.Parse(ansGridView1.Rows[i].Cells["taxamt1"].Value.ToString());
                dtVoucherDet.Rows[i]["taxamt2"] = double.Parse(ansGridView1.Rows[i].Cells["taxamt2"].Value.ToString());
                dtVoucherDet.Rows[i]["taxamt3"] = double.Parse(ansGridView1.Rows[i].Cells["taxamt3"].Value.ToString());
                dtVoucherDet.Rows[i]["taxamt4"] = double.Parse(ansGridView1.Rows[i].Cells["taxamt4"].Value.ToString());
                dtVoucherDet.Rows[i]["bottomdis"] = double.Parse(ansGridView1.Rows[i].Cells["bottomdis"].Value.ToString());
                dtVoucherDet.Rows[i]["Amount0"] = double.Parse(ansGridView1.Rows[i].Cells["Amount0"].Value.ToString());
                dtVoucherDet.Rows[i]["QDType"] = ansGridView1.Rows[i].Cells["QDType"].Value.ToString();
                dtVoucherDet.Rows[i]["QDAmount"] = double.Parse(ansGridView1.Rows[i].Cells["QDAmount"].Value.ToString());
                dtVoucherDet.Rows[i]["Amount1"] = double.Parse(ansGridView1.Rows[i].Cells["Amount1"].Value.ToString());
                dtVoucherDet.Rows[i]["CDType"] = ansGridView1.Rows[i].Cells["CDType"].Value.ToString();
                dtVoucherDet.Rows[i]["CDAmount"] = double.Parse(ansGridView1.Rows[i].Cells["CDAmount"].Value.ToString());
                dtVoucherDet.Rows[i]["Amount2"] = double.Parse(ansGridView1.Rows[i].Cells["Amount2"].Value.ToString());
                dtVoucherDet.Rows[i]["FDType"] = ansGridView1.Rows[i].Cells["FDType"].Value.ToString();
                dtVoucherDet.Rows[i]["FDAmount"] = double.Parse(ansGridView1.Rows[i].Cells["FDAmount"].Value.ToString());
                dtVoucherDet.Rows[i]["Amount3"] = double.Parse(ansGridView1.Rows[i].Cells["Amount3"].Value.ToString());
                dtVoucherDet.Rows[i]["GridDis"] = double.Parse(ansGridView1.Rows[i].Cells["GridDis"].Value.ToString());
                dtVoucherDet.Rows[i]["TotalDis"] = double.Parse(ansGridView1.Rows[i].Cells["TotalDis"].Value.ToString());
                dtVoucherDet.Rows[i]["Amount4"] = double.Parse(ansGridView1.Rows[i].Cells["Amount4"].Value.ToString());
                dtVoucherDet.Rows[i]["TotTaxPer"] = double.Parse(ansGridView1.Rows[i].Cells["TotTaxPer"].Value.ToString());
                dtVoucherDet.Rows[i]["TotTaxAmount"] = double.Parse(ansGridView1.Rows[i].Cells["TotTaxAmount"].Value.ToString());
                dtVoucherDet.Rows[i]["Amount5"] = double.Parse(ansGridView1.Rows[i].Cells["Amount5"].Value.ToString());
                dtVoucherDet.Rows[i]["ExpAmount"] = double.Parse(ansGridView1.Rows[i].Cells["ExpAmount"].Value.ToString());
            }

            Database.SaveData(dtVoucherDet);

            DataTable dtTemp1 = new DataTable("Stock");
            Database.GetSqlData("select * from Stock where Vid='" + vid + "' ", dtTemp1);
            for (int i = 0; i < dtTemp1.Rows.Count; i++)
            {
                dtTemp1.Rows[i].Delete();
            }
            Database.SaveData(dtTemp1);

            dtStock = new DataTable("Stock");
            Database.GetSqlData("select * from Stock where Vid='" + vid + "'", dtStock);

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
                if (gtype == "Sale" || gtype == "Pending")
                {
                    dtStock.Rows.Add();
                    dtStock.Rows[i]["Vid"] = vid;
                    dtStock.Rows[i]["Itemsr"] = ansGridView1.Rows[i].Cells["sno"].Value.ToString();
                    dtStock.Rows[i]["Did"] = ansGridView1.Rows[i].Cells["Des_ac_id"].Value.ToString();
                    dtStock.Rows[i]["Receive"] = 0;
                    dtStock.Rows[i]["ReceiveAmt"] = 0;
                    dtStock.Rows[i]["Issue"] = ansGridView1.Rows[i].Cells["Quantity"].Value.ToString();
                    dtStock.Rows[i]["IssueAmt"] = ansGridView1.Rows[i].Cells["Taxabelamount"].Value.ToString();
                    string gridgod = "";
                    gridgod = funs.Select_ac_id(ansGridView1.Rows[i].Cells["godown_id"].Value.ToString()); ;
                    //if (gridgod == "")
                    //{
                    //    gridgod = "0";
                    //}
                    dtStock.Rows[i]["godown_id"] = gridgod;

                    dtStock.Rows[i]["marked"] = marked;
                    if (Feature.Available("Batch Number") == "Yes")
                    {
                        dtStock.Rows[i]["Batch_no"] = ansGridView1.Rows[i].Cells["Batch_Code"].Value.ToString();
                    }
                    if (ansGridView1.Rows[i].Cells["Batch_Code"].Value == null)
                    {
                        ansGridView1.Rows[i].Cells["Batch_Code"].Value = "";
                    }
                    dtStock.Rows[i]["Batch_no"] = ansGridView1.Rows[i].Cells["Batch_Code"].Value.ToString();
                    dtStock.Rows[i]["LocationId"] = Prelocationid;
                    dtStock.Rows[i]["Branch_id"] = Database.BranchId;
                }

                else if (gtype == "P Return")
                {
                    dtStock.Rows.Add();
                    dtStock.Rows[i]["Vid"] = vid;
                    dtStock.Rows[i]["Itemsr"] = ansGridView1.Rows[i].Cells["sno"].Value.ToString();
                    dtStock.Rows[i]["Did"] = ansGridView1.Rows[i].Cells["Des_ac_id"].Value.ToString();
                    dtStock.Rows[i]["Receive"] = -1 * (double.Parse(ansGridView1.Rows[i].Cells["Quantity"].Value.ToString()));
                    dtStock.Rows[i]["ReceiveAmt"] = -1 * (double.Parse(ansGridView1.Rows[i].Cells["Taxabelamount"].Value.ToString()));
                    dtStock.Rows[i]["Issue"] = 0;
                    dtStock.Rows[i]["IssueAmt"] = 0;
                    string gridgod = "";
                    gridgod = funs.Select_ac_id(ansGridView1.Rows[i].Cells["godown_id"].Value.ToString()); ;
                    //if (gridgod == "")
                    //{
                    //    gridgod = "0";
                    //}
                    dtStock.Rows[i]["godown_id"] = gridgod;
                    dtStock.Rows[i]["marked"] = marked;
                    if (Feature.Available("Batch Number") == "Yes")
                    {
                        dtStock.Rows[i]["Batch_no"] = ansGridView1.Rows[i].Cells["Batch_Code"].Value.ToString();
                    }
                    if (ansGridView1.Rows[i].Cells["Batch_Code"].Value == null)
                    {
                        ansGridView1.Rows[i].Cells["Batch_Code"].Value = "";
                    }
                    dtStock.Rows[i]["Batch_no"] = ansGridView1.Rows[i].Cells["Batch_Code"].Value.ToString();
                    dtStock.Rows[i]["LocationId"] = Prelocationid;
                    dtStock.Rows[i]["Branch_id"] = Database.BranchId;
                }

                else if (gtype == "Return")
                {
                    dtStock.Rows.Add();
                    dtStock.Rows[i]["Vid"] = vid;
                    dtStock.Rows[i]["Itemsr"] = ansGridView1.Rows[i].Cells["sno"].Value.ToString();
                    dtStock.Rows[i]["Did"] = ansGridView1.Rows[i].Cells["Des_ac_id"].Value.ToString();
                    dtStock.Rows[i]["Issue"] = -1 * (double.Parse(ansGridView1.Rows[i].Cells["Quantity"].Value.ToString()));
                    dtStock.Rows[i]["IssueAmt"] = -1 * (double.Parse(ansGridView1.Rows[i].Cells["Taxabelamount"].Value.ToString()));
                    dtStock.Rows[i]["Receive"] = 0;
                    dtStock.Rows[i]["ReceiveAmt"] = 0;
                    // dtStock.Rows[i]["godown_id"] = funs.Select_ac_id(ansGridView1.Rows[i].Cells["godown_id"].Value.ToString());
                    string gridgod = "";
                    gridgod = funs.Select_ac_id(ansGridView1.Rows[i].Cells["godown_id"].Value.ToString()); ;
                    //if (gridgod == "")
                    //{
                    //    gridgod = "0";
                    //}
                    dtStock.Rows[i]["godown_id"] = gridgod;
                    dtStock.Rows[i]["marked"] = marked;
                    if (Feature.Available("Batch Number") == "Yes")
                    {
                        dtStock.Rows[i]["Batch_no"] = ansGridView1.Rows[i].Cells["Batch_Code"].Value.ToString();
                    }
                    if (ansGridView1.Rows[i].Cells["Batch_Code"].Value == null)
                    {
                        ansGridView1.Rows[i].Cells["Batch_Code"].Value = "";
                    }
                    dtStock.Rows[i]["Batch_no"] = ansGridView1.Rows[i].Cells["Batch_Code"].Value.ToString();
                    dtStock.Rows[i]["LocationId"] = Prelocationid;
                    dtStock.Rows[i]["Branch_id"] = Database.BranchId;
                }

                else if (gtype == "Purchase" || gtype == "RCM" || gtype == "Opening")
                {
                    dtStock.Rows.Add();
                    dtStock.Rows[i]["Vid"] = vid;
                    dtStock.Rows[i]["Itemsr"] = ansGridView1.Rows[i].Cells["sno"].Value.ToString();
                    dtStock.Rows[i]["Did"] = ansGridView1.Rows[i].Cells["Des_ac_id"].Value.ToString();
                    dtStock.Rows[i]["Issue"] = 0;
                    dtStock.Rows[i]["IssueAmt"] = 0;
                    dtStock.Rows[i]["Receive"] = ansGridView1.Rows[i].Cells["Quantity"].Value.ToString();
                    dtStock.Rows[i]["ReceiveAmt"] = ansGridView1.Rows[i].Cells["Taxabelamount"].Value.ToString();
                    string gridgod = "";
                    gridgod = funs.Select_ac_id(ansGridView1.Rows[i].Cells["godown_id"].Value.ToString()); ;
                    //if (gridgod == "")
                    //{
                    //    gridgod = "0";
                    //}
                    dtStock.Rows[i]["godown_id"] = gridgod;
                    if (gtype == "Opening")
                    {
                        dtStock.Rows[i]["godown_id"] = funs.Select_ac_id(textBox14.Text);
                    }

                    dtStock.Rows[i]["marked"] = marked;
                    if (Feature.Available("Batch Number") == "Yes")
                    {
                        dtStock.Rows[i]["Batch_no"] = ansGridView1.Rows[i].Cells["Batch_Code"].Value.ToString();
                    }
                    if (ansGridView1.Rows[i].Cells["Batch_Code"].Value == null)
                    {
                        ansGridView1.Rows[i].Cells["Batch_Code"].Value = "";
                    }
                    dtStock.Rows[i]["Batch_no"] = ansGridView1.Rows[i].Cells["Batch_Code"].Value.ToString();
                    dtStock.Rows[i]["LocationId"] = Prelocationid;
                    dtStock.Rows[i]["Branch_id"] = Database.BranchId;
                }
            }

            Database.SaveData(dtStock);

            //VoucherCharges
            dtTemp = new DataTable("VouCharges");
            Database.GetSqlData("select * from VouCharges where vi_id='" + vid + "' ", dtTemp);
            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                dtTemp.Rows[i].Delete();
            }
            Database.SaveData(dtTemp);
            if (dtVoucherCharges1 != null)
            {
                dtVoucherCharges1.Rows.Clear();
            }
            if (Feature.Available("Discount Detailed on Bill").ToUpper() == "YES")
            {
                if (totalqdamount != 0)
                {
                    dtVoucherCharges1.Rows.Add();
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Vi_id"] = vid;
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Entry_typ"] = 0;
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Srno"] = 1;
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Charg_id"] = 0;
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Accid"] = 0;
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Charg_Name"] = Feature.Available("Show Text on Discount1");
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Amount"] = totalqdamount;
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Camount"] = totalqdamount;
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Addsub"] = 0;
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Ctype"] = 0;
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Changed"] = false;
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["LocationId"] = Prelocationid;
                }

                if (totalcdamount != 0)
                {
                    dtVoucherCharges1.Rows.Add();
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Vi_id"] = vid;
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Entry_typ"] = 0;
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Srno"] = 2;
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Charg_id"] = 0;
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Accid"] = 0;
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Charg_Name"] = Feature.Available("Show Text on Discount2"); ;
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Amount"] = totalcdamount;
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Camount"] = totalcdamount;
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Addsub"] = 0;
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Ctype"] = 0;
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Changed"] = false;
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["LocationId"] = Prelocationid;
                }

                if (totalfixamount != 0)
                {
                    dtVoucherCharges1.Rows.Add();
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Vi_id"] = vid;
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Entry_typ"] = 0;
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Srno"] = 3;
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Charg_id"] = 0;
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Accid"] = 0;
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Charg_Name"] = Feature.Available("Show Text on Discount3");
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Amount"] = totalfixamount;
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Camount"] = totalfixamount;
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Addsub"] = 0;
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Ctype"] = 0;
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Changed"] = false;
                    dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["LocationId"] = Prelocationid;
                }
            }

            for (int i = 0; i < ansGridView3.Rows.Count - 1; i++)
            {
                if (ansGridView3.Rows[i].Cells["Changed1"].Value == null)
                {
                    ansGridView3.Rows[i].Cells["Changed1"].Value = false;
                }

                dtVoucherCharges1.Rows.Add();
                dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Vi_id"] = vid;
                dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Entry_typ"] = 1;
                dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Srno"] = ansGridView3.Rows[i].Cells["sno2"].Value.ToString();
                dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Charg_id"] = funs.Select_ch_id(ansGridView3.Rows[i].Cells["Charg_Name"].Value.ToString());
                if (ansGridView3.Rows[i].Cells["Accid1"].Value == null)
                {
                    ansGridView3.Rows[i].Cells["Accid1"].Value = 0;
                }
                dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Accid"] = ansGridView3.Rows[i].Cells["Accid1"].Value.ToString();
                dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Charg_Name"] = ansGridView3.Rows[i].Cells["Charg_Name"].Value.ToString();
                dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Amount"] = ansGridView3.Rows[i].Cells["AmountA"].Value.ToString();
                dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Camount"] = ansGridView3.Rows[i].Cells["CamountA"].Value.ToString();
                dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Addsub"] = ansGridView3.Rows[i].Cells["Addsub1"].Value.ToString();
                dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Ctype"] = ansGridView3.Rows[i].Cells["Ctype1"].Value.ToString();
                dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["Changed"] = ansGridView3.Rows[i].Cells["Changed1"].Value.ToString();
                dtVoucherCharges1.Rows[dtVoucherCharges1.Rows.Count - 1]["LocationId"] = Prelocationid;
            }

            Database.SaveData(dtVoucherCharges1);
            if (dtVoucherCharges2 != null)
            {
                dtVoucherCharges2.Rows.Clear();
            }
            for (int i = 0; i < ansGridView4.Rows.Count - 1; i++)
            {
                if (ansGridView4.Rows[i].Cells["Changed2"].Value == null)
                {
                    ansGridView4.Rows[i].Cells["Changed2"].Value = false;
                }
                dtVoucherCharges2.Rows.Add();
                dtVoucherCharges2.Rows[i]["Vi_id"] = vid;
                dtVoucherCharges2.Rows[i]["Entry_typ"] = 3;
                dtVoucherCharges2.Rows[i]["Srno"] = ansGridView4.Rows[i].Cells["sno3"].Value.ToString();
                dtVoucherCharges2.Rows[i]["Charg_id"] = funs.Select_ch_id(ansGridView4.Rows[i].Cells["Charg_Name2"].Value.ToString());
                dtVoucherCharges2.Rows[i]["Accid"] = ansGridView4.Rows[i].Cells["Accid2"].Value.ToString();
                dtVoucherCharges2.Rows[i]["Charg_Name"] = ansGridView4.Rows[i].Cells["Charg_Name2"].Value.ToString();
                dtVoucherCharges2.Rows[i]["Amount"] = ansGridView4.Rows[i].Cells["AmountB"].Value.ToString();
                dtVoucherCharges2.Rows[i]["Camount"] = ansGridView4.Rows[i].Cells["CamountB"].Value.ToString();
                dtVoucherCharges2.Rows[i]["Addsub"] = ansGridView4.Rows[i].Cells["Addsub2"].Value.ToString();
                dtVoucherCharges2.Rows[i]["Ctype"] = ansGridView4.Rows[i].Cells["Ctype2"].Value.ToString();
                dtVoucherCharges2.Rows[i]["Changed"] = ansGridView4.Rows[i].Cells["Changed2"].Value.ToString();
                dtVoucherCharges2.Rows[i]["LocationId"] = Prelocationid;
            }

            if (totdisaftertax != 0)
            {
                string datac_id = Database.GetScalarText("Select Ac_id from DisAfterTax");
                string disname = Database.GetScalarText("Select taxname from DisAfterTax");
                dtVoucherCharges2.Rows.Add();
                dtVoucherCharges2.Rows[dtVoucherCharges2.Rows.Count - 1]["Vi_id"] = vid;
                dtVoucherCharges2.Rows[dtVoucherCharges2.Rows.Count - 1]["Entry_typ"] = 3;
                dtVoucherCharges2.Rows[dtVoucherCharges2.Rows.Count - 1]["Srno"] = 0;
                dtVoucherCharges2.Rows[dtVoucherCharges2.Rows.Count - 1]["Charg_id"] = 0;
                dtVoucherCharges2.Rows[dtVoucherCharges2.Rows.Count - 1]["Accid"] = datac_id;
                dtVoucherCharges2.Rows[dtVoucherCharges2.Rows.Count - 1]["Charg_Name"] = disname;
                dtVoucherCharges2.Rows[dtVoucherCharges2.Rows.Count - 1]["Amount"] = -1 * totdisaftertax;
                dtVoucherCharges2.Rows[dtVoucherCharges2.Rows.Count - 1]["Camount"] = -1 * totdisaftertax;
                dtVoucherCharges2.Rows[dtVoucherCharges2.Rows.Count - 1]["Addsub"] = 4;
                dtVoucherCharges2.Rows[dtVoucherCharges2.Rows.Count - 1]["Ctype"] = 3;
                dtVoucherCharges2.Rows[dtVoucherCharges2.Rows.Count - 1]["Changed"] = false;
                dtVoucherCharges2.Rows[dtVoucherCharges2.Rows.Count - 1]["LocationId"] = Prelocationid;
            }

            Database.SaveData(dtVoucherCharges2);


            if (dtVoucherCharges4 != null)
            {
                dtVoucherCharges4.Rows.Clear();
            }
            for (int i = 0; i < dtpaymentdet.Rows.Count; i++)
            {

                dtVoucherCharges4.Rows.Add();
                dtVoucherCharges4.Rows[i]["Vi_id"] = vid;
                dtVoucherCharges4.Rows[i]["Entry_typ"] = 5;
                dtVoucherCharges4.Rows[i]["Srno"] = dtpaymentdet.Rows[i]["itemsr"].ToString();
                dtVoucherCharges4.Rows[i]["Charg_id"] = 1;
                dtVoucherCharges4.Rows[i]["Accid"] = dtpaymentdet.Rows[i]["Acc_id"].ToString();
                dtVoucherCharges4.Rows[i]["Charg_Name"] = funs.Select_ac_nm(dtpaymentdet.Rows[i]["Acc_id"].ToString());
                dtVoucherCharges4.Rows[i]["Amount"] = -1 * double.Parse(dtpaymentdet.Rows[i]["Amount"].ToString());
                dtVoucherCharges4.Rows[i]["Camount"] = -1 * double.Parse(dtpaymentdet.Rows[i]["Amount"].ToString());
                dtVoucherCharges4.Rows[i]["Addsub"] = 4;
                dtVoucherCharges4.Rows[i]["Ctype"] = 3;
                dtVoucherCharges4.Rows[i]["Changed"] = false;
                dtVoucherCharges4.Rows[i]["LocationId"] = Prelocationid;
            }
            Database.SaveData(dtVoucherCharges4);

            //RoundOff
            if (dtRoundOff != null)
            {
                dtRoundOff.Rows.Clear();

                if (double.Parse(textBox9.Text) != 0)
                {
                    dtRoundOff.Rows.Add();
                    dtRoundOff.Rows[0]["Vi_id"] = vid;
                    dtRoundOff.Rows[0]["Entry_typ"] = 4;
                    dtRoundOff.Rows[0]["Srno"] = 1;
                    dtRoundOff.Rows[0]["Charg_id"] = 0;
                    dtRoundOff.Rows[0]["Accid"] = "MAN1";
                    dtRoundOff.Rows[0]["Charg_Name"] = "Round Off";
                    dtRoundOff.Rows[0]["Amount"] = textBox9.Text;
                    dtRoundOff.Rows[0]["Camount"] = textBox9.Text; ;
                    dtRoundOff.Rows[0]["Addsub"] = 4;
                    dtRoundOff.Rows[0]["Ctype"] = 3;
                    dtRoundOff.Rows[0]["Changed"] = RoffChanged;
                    dtRoundOff.Rows[0]["LocationId"] = Prelocationid;
                }

                Database.SaveData(dtRoundOff);
            }
            //taxable amount
            if (dtRoundOff != null)
            {
                dtRoundOff.Rows.Clear();

                string taxname1 = "", taxname2 = "", taxname3 = "", taxname4 = "";
                if (Feature.Available("Taxation Applicable") == "VAT")
                {
                    taxname1 = "VAT";
                    taxname2 = "SAT";
                    taxname3 = "CST";
                    taxname4 = "Service Tax";
                }
                else
                {
                    taxname1 = "CGST";
                    taxname2 = "SGST";
                    taxname3 = "IGST";
                    taxname4 = "Cess";
                }

                if (gExcludingTax == true || Feature.Available("Show Taxes in Including Tax").ToUpper() == "YES")
                {
                    dtRoundOff.Rows.Add();
                    dtRoundOff.Rows[0]["Vi_id"] = vid;
                    dtRoundOff.Rows[0]["Entry_typ"] = 2;
                    dtRoundOff.Rows[0]["Srno"] = 0;
                    dtRoundOff.Rows[0]["Charg_id"] = 0;
                    dtRoundOff.Rows[0]["Accid"] = 1;
                    dtRoundOff.Rows[0]["Charg_Name"] = "Taxable Amt";
                    dtRoundOff.Rows[0]["Amount"] = textBox24.Text;
                    dtRoundOff.Rows[0]["Camount"] = textBox24.Text; ;
                    dtRoundOff.Rows[0]["Addsub"] = 4;
                    dtRoundOff.Rows[0]["Ctype"] = 3;
                    dtRoundOff.Rows[0]["Changed"] = false;
                    dtRoundOff.Rows[0]["LocationId"] = Prelocationid;

                    if (ctaxamt1 != 0)
                    {
                        dtRoundOff.Rows.Add();
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Vi_id"] = vid;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Entry_typ"] = 2;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Srno"] = 1;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Charg_id"] = 0;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Accid"] = 1;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Charg_Name"] = taxname1;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Amount"] = ctaxamt1;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Camount"] = ctaxamt1;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Addsub"] = 4;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Ctype"] = 3;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Changed"] = false;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["LocationId"] = Prelocationid;
                    }

                    if (ctaxamt2 != 0)
                    {
                        dtRoundOff.Rows.Add();
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Vi_id"] = vid;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Entry_typ"] = 2;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Srno"] = 2;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Charg_id"] = 0;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Accid"] = 1;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Charg_Name"] = taxname2;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Amount"] = ctaxamt2;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Camount"] = ctaxamt2;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Addsub"] = 4;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Ctype"] = 3;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Changed"] = false;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["LocationId"] = Prelocationid;
                    }

                    if (ctaxamt3 != 0)
                    {
                        dtRoundOff.Rows.Add();
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Vi_id"] = vid;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Entry_typ"] = 2;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Srno"] = 3;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Charg_id"] = 0;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Accid"] = 1;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Charg_Name"] = taxname3;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Amount"] = ctaxamt3;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Camount"] = ctaxamt3;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Addsub"] = 4;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Ctype"] = 3;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Changed"] = false;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["LocationId"] = Prelocationid;
                    }

                    if (ctaxamt4 != 0)
                    {
                        dtRoundOff.Rows.Add();
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Vi_id"] = vid;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Entry_typ"] = 2;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Srno"] = 4;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Charg_id"] = 0;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Accid"] = 1;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Charg_Name"] = taxname4;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Amount"] = ctaxamt4;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Camount"] = ctaxamt4;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Addsub"] = 4;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Ctype"] = 3;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["Changed"] = false;
                        dtRoundOff.Rows[dtRoundOff.Rows.Count - 1]["LocationId"] = Prelocationid;
                    }
                }

                Database.SaveData(dtRoundOff);
            }
            //ItemCharges
            dtTemp = new DataTable("itemcharges");
            Database.GetSqlData("Select * from itemcharges where Vi_id='" + vid + "' ", dtTemp);
            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                dtTemp.Rows[i].Delete();
            }
            Database.SaveData(dtTemp);

            for (int i = 0; i < dtItemCharges.Rows.Count; i++)
            {
                dtItemCharges.Rows[i]["Vi_id"] = vid;
                dtItemCharges.Rows[i]["LocationId"] = Prelocationid;
                dtItemCharges.Rows[i].AcceptChanges();
                dtItemCharges.Rows[i].SetAdded();
            }
            Database.SaveData(dtItemCharges);




            //paymentdetde
            dtTemp = new DataTable("Voucherpaydet");
            Database.GetSqlData("Select * from Voucherpaydet where Vi_id='" + vid + "' ", dtTemp);
            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                dtTemp.Rows[i].Delete();
            }
            Database.SaveData(dtTemp);

            if (Feature.Available("Required PaymentMode Form").ToUpper() == "YES")
            {
                for (int i = 0; i < dtpaymentdet.Rows.Count; i++)
                {
                    dtpaymentdet.Rows[i]["Vi_id"] = vid;
                    dtpaymentdet.Rows[i].AcceptChanges();
                    dtpaymentdet.Rows[i].SetAdded();
                }
                Database.SaveData(dtpaymentdet);
            }
            //journal




            if (gtype != "Pending" && gtype != "Temp" && gtype != "Transfer" && gtype != "Sale Order" && gtype != "Opening")
            {
                dtTemp = new DataTable("Journal");
                Database.GetSqlData("Select * from Journal where Vi_id='" + vid + "' and sno<>10002 ", dtTemp);
                for (int j = 0; j < dtTemp.Rows.Count; j++)
                {
                    dtTemp.Rows[j].Delete();
                }
                Database.SaveData(dtTemp);

                dtJournal = new DataTable("Journal");
                Database.GetSqlData("Select * from Journal where Vi_id='" + vid + "' ", dtJournal);
                string effect_acc = "Y";
                int fsno = 0;

                if (effect_acc == Database.GetScalarText("Select Effect_On_Acc from Vouchertype where Vt_id='" + vtid + "' "))
                {
                    string acid = "";
                    //if (radioButton8.Checked == true)
                    //{
                    //    acid = Database.GetScalarText("Select ac_id from account where act_id='SER3' and branch_id='" + Database.BranchId + "' ");
                    //}
                    //else
                    //{
                    acid = funs.Select_ac_id(textBox14.Text);
                    //  }

                    string actualnarr = textBox7.Text;
                    actualnarr = actualnarr.Replace("{Vno}", vno.ToString());
                    actualnarr = actualnarr.Replace("{Vouchertype}", funs.Select_vt_nm(vtid));
                    actualnarr = actualnarr.Replace("{Amount}", funs.IndianCurr(double.Parse(textBox10.Text)));
                    actualnarr = actualnarr.Replace("{Svnum}", textBox2.Text);
                    actualnarr = actualnarr.Replace("{Svdate}", dateTimePicker2.Value.Date.ToString(Database.dformat));
                    actualnarr = actualnarr.Replace("{Partyname}", textBox14.Text);
                    actualnarr = actualnarr.Replace("{Amount}", funs.IndianCurr(double.Parse(textBox10.Text)));
                    actualnarr = actualnarr.Replace("\r", "");

                    for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
                    {
                        if (double.Parse(ansGridView1.Rows[i].Cells["Taxabelamount"].Value.ToString()) != 0)
                        {
                            fsno = i + 1;
                            dtJournal.Rows.Add();
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["vdate"] = dateTimePicker1.Value.ToString("dd-MMM-yyyy");
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Vi_id"] = vid;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr2"] = textBox7.Text;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr"] = actualnarr;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Sno"] = fsno;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["LocationId"] = Database.LocationId;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Ac_id"] = acid;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = ansGridView1.Rows[i].Cells["pur_sale_acc"].Value.ToString();
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = double.Parse(ansGridView1.Rows[i].Cells["Taxabelamount"].Value.ToString());
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["A"] = A;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["B"] = B;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["AB"] = AB;

                            dtJournal.Rows.Add();
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["vdate"] = dateTimePicker1.Value.ToString("dd-MMM-yyyy");
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Vi_id"] = vid;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr2"] = textBox7.Text;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr"] = actualnarr;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Sno"] = fsno;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["LocationId"] = Database.LocationId;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Ac_id"] = ansGridView1.Rows[i].Cells["pur_sale_acc"].Value.ToString();
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = acid;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = -1 * double.Parse(ansGridView1.Rows[i].Cells["Taxabelamount"].Value.ToString());
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["A"] = A;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["B"] = B;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["AB"] = AB;
                        }

                        //tax1

                        if (double.Parse(ansGridView1.Rows[i].Cells["taxamt1"].Value.ToString()) != 0)
                        {
                            fsno = i + 1001;
                            dtJournal.Rows.Add();
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["vdate"] = dateTimePicker1.Value.ToString("dd-MMM-yyyy");
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Vi_id"] = vid;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr2"] = textBox7.Text;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr"] = actualnarr;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Sno"] = fsno;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["LocationId"] = Database.LocationId;
                            if (gtype != "RCM")
                            {

                                dtJournal.Rows[dtJournal.Rows.Count - 1]["Ac_id"] = acid;
                            }
                            else
                            {
                                dtJournal.Rows[dtJournal.Rows.Count - 1]["Ac_id"] = ansGridView1.Rows[i].Cells["RCMac_id"].Value.ToString();
                            }
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = ansGridView1.Rows[i].Cells["tax1"].Value.ToString();
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = double.Parse(ansGridView1.Rows[i].Cells["taxamt1"].Value.ToString());
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["A"] = A;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["B"] = B;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["AB"] = AB;
                            dtJournal.Rows.Add();
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["vdate"] = dateTimePicker1.Value.ToString("dd-MMM-yyyy");
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Vi_id"] = vid;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr2"] = textBox7.Text;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr"] = actualnarr;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Sno"] = fsno;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["LocationId"] = Database.LocationId;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Ac_id"] = ansGridView1.Rows[i].Cells["tax1"].Value.ToString();

                            if (gtype != "RCM")
                            {
                                dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = acid;
                            }
                            else
                            {
                                dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = ansGridView1.Rows[i].Cells["RCMac_id"].Value.ToString();
                            }
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = -1 * (double.Parse(ansGridView1.Rows[i].Cells["taxamt1"].Value.ToString()));
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["A"] = A;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["B"] = B;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["AB"] = AB;
                        }

                        //tax2
                        if (double.Parse(ansGridView1.Rows[i].Cells["taxamt2"].Value.ToString()) != 0)
                        {
                            fsno = i + 2001;
                            dtJournal.Rows.Add();
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["vdate"] = dateTimePicker1.Value.ToString("dd-MMM-yyyy");
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Vi_id"] = vid;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr2"] = textBox7.Text;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr"] = actualnarr;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Sno"] = fsno;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["LocationId"] = Database.LocationId;
                            if (gtype != "RCM")
                            {

                                dtJournal.Rows[dtJournal.Rows.Count - 1]["Ac_id"] = acid;
                            }
                            else
                            {
                                dtJournal.Rows[dtJournal.Rows.Count - 1]["Ac_id"] = ansGridView1.Rows[i].Cells["RCMac_id"].Value.ToString();
                            }
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = ansGridView1.Rows[i].Cells["tax2"].Value.ToString();
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = double.Parse(ansGridView1.Rows[i].Cells["taxamt2"].Value.ToString());
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["A"] = A;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["B"] = B;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["AB"] = AB;

                            dtJournal.Rows.Add();
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["vdate"] = dateTimePicker1.Value.ToString("dd-MMM-yyyy");
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Vi_id"] = vid;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr2"] = textBox7.Text;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr"] = actualnarr;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Sno"] = fsno;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["LocationId"] = Database.LocationId;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Ac_id"] = ansGridView1.Rows[i].Cells["tax2"].Value.ToString();
                            if (gtype != "RCM")
                            {
                                dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = acid;
                            }
                            else
                            {
                                dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = ansGridView1.Rows[i].Cells["RCMac_id"].Value.ToString();
                            }
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = -1 * (double.Parse(ansGridView1.Rows[i].Cells["taxamt2"].Value.ToString()));
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["A"] = A;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["B"] = B;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["AB"] = AB;
                        }
                        //tax3
                        if (double.Parse(ansGridView1.Rows[i].Cells["taxamt3"].Value.ToString()) != 0)
                        {
                            fsno = i + 3001;
                            dtJournal.Rows.Add();
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["vdate"] = dateTimePicker1.Value.ToString("dd-MMM-yyyy");
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Vi_id"] = vid;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr2"] = textBox7.Text;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr"] = actualnarr;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Sno"] = fsno;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["LocationId"] = Database.LocationId;
                            if (gtype != "RCM")
                            {

                                dtJournal.Rows[dtJournal.Rows.Count - 1]["Ac_id"] = acid;
                            }
                            else
                            {
                                dtJournal.Rows[dtJournal.Rows.Count - 1]["Ac_id"] = ansGridView1.Rows[i].Cells["RCMac_id"].Value.ToString();
                            }
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = ansGridView1.Rows[i].Cells["tax3"].Value.ToString();
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = double.Parse(ansGridView1.Rows[i].Cells["taxamt3"].Value.ToString());
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["A"] = A;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["B"] = B;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["AB"] = AB;

                            dtJournal.Rows.Add();
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["vdate"] = dateTimePicker1.Value.ToString("dd-MMM-yyyy");
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Vi_id"] = vid;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr2"] = textBox7.Text;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr"] = actualnarr;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Sno"] = fsno;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["LocationId"] = Database.LocationId;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Ac_id"] = ansGridView1.Rows[i].Cells["tax3"].Value.ToString();
                            if (gtype != "RCM")
                            {
                                dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = acid;
                            }
                            else
                            {
                                dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = ansGridView1.Rows[i].Cells["RCMac_id"].Value.ToString();
                            }
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = -1 * (double.Parse(ansGridView1.Rows[i].Cells["taxamt3"].Value.ToString()));
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["A"] = A;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["B"] = B;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["AB"] = AB;

                        }
                        //tax4
                        if (double.Parse(ansGridView1.Rows[i].Cells["taxamt4"].Value.ToString()) != 0)
                        {
                            fsno = i + 4001;
                            dtJournal.Rows.Add();
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["vdate"] = dateTimePicker1.Value.ToString("dd-MMM-yyyy");
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Vi_id"] = vid;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr2"] = textBox7.Text;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr"] = actualnarr;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Sno"] = fsno;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["LocationId"] = Database.LocationId;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Ac_id"] = acid;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = ansGridView1.Rows[i].Cells["tax4"].Value.ToString();
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = double.Parse(ansGridView1.Rows[i].Cells["taxamt4"].Value.ToString());
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["A"] = A;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["B"] = B;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["AB"] = AB;

                            dtJournal.Rows.Add();
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["vdate"] = dateTimePicker1.Value.ToString("dd-MMM-yyyy");
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Vi_id"] = vid;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr2"] = textBox7.Text;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr"] = actualnarr;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Sno"] = fsno;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["LocationId"] = Database.LocationId;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Ac_id"] = ansGridView1.Rows[i].Cells["tax4"].Value.ToString();
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = acid;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = -1 * (double.Parse(ansGridView1.Rows[i].Cells["taxamt4"].Value.ToString()));
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["A"] = A;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["B"] = B;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["AB"] = AB;
                        }
                    }


                    for (int i = 0; i < ansGridView4.Rows.Count - 1; i++)
                    {
                        fsno = i + 7001;
                        dtJournal.Rows.Add();
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["vdate"] = dateTimePicker1.Value.ToString("dd-MMM-yyyy");
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["Vi_id"] = vid;
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr2"] = textBox7.Text;
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr"] = actualnarr;
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["Sno"] = fsno;
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["LocationId"] = Database.LocationId;
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["Ac_id"] = acid;
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = ansGridView4.Rows[i].Cells["Accid2"].Value.ToString();
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = double.Parse(ansGridView4.Rows[i].Cells["CamountB"].Value.ToString());
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["A"] = A;
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["B"] = B;
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["AB"] = AB;


                        dtJournal.Rows.Add();
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["vdate"] = dateTimePicker1.Value.ToString("dd-MMM-yyyy");
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["Vi_id"] = vid;
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr2"] = textBox7.Text;
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr"] = actualnarr;
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["Sno"] = fsno;
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["LocationId"] = Database.LocationId;
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["Ac_id"] = ansGridView4.Rows[i].Cells["Accid2"].Value.ToString();
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = acid;
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = -1 * double.Parse(ansGridView4.Rows[i].Cells["CamountB"].Value.ToString());
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["A"] = A;
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["B"] = B;
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["AB"] = AB;
                    }

                    if (double.Parse(textBox9.Text) != 0)
                    {
                        fsno = 8001;
                        dtJournal.Rows.Add();
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["vdate"] = dateTimePicker1.Value.ToString("dd-MMM-yyyy");
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["Vi_id"] = vid;
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr2"] = textBox7.Text;
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr"] = actualnarr;
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["Sno"] = fsno;
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["LocationId"] = Database.LocationId;
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["Ac_id"] = acid;
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = "MAN1";
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = double.Parse(textBox9.Text);
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["A"] = A;
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["B"] = B;
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["AB"] = AB;

                        dtJournal.Rows.Add();
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["vdate"] = dateTimePicker1.Value.ToString("dd-MMM-yyyy");
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["Vi_id"] = vid;
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr2"] = textBox7.Text;
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr"] = actualnarr;
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["Sno"] = fsno;
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["LocationId"] = Database.LocationId;
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["Ac_id"] = "MAN1";
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = acid;
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = -1 * double.Parse(textBox9.Text);
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["A"] = A;
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["B"] = B;
                        dtJournal.Rows[dtJournal.Rows.Count - 1]["AB"] = AB;

                    }
                    string act_id = Database.GetScalarText("Select Act_id from Account where Name='" + textBox14.Text + "'");
                    if (Feature.Available("Required PaymentMode Form").ToUpper() == "NO")
                    {

                        if (radioButton8.Checked == true && act_id != "SER3")
                        {
                            fsno = 9001;
                            dtJournal.Rows.Add();
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["vdate"] = dateTimePicker1.Value.ToString("dd-MMM-yyyy");
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Vi_id"] = vid;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr2"] = textBox7.Text;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr"] = actualnarr;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Sno"] = fsno;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["LocationId"] = Database.LocationId;
                            string ac_id = Database.GetScalarText("Select Ac_id from Account where Act_id='SER3' and Branch_id='" + Database.BranchId + "'");
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Ac_id"] = ac_id;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = acid;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = double.Parse(textBox10.Text);
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["A"] = A;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["B"] = B;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["AB"] = AB;

                            dtJournal.Rows.Add();
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["vdate"] = dateTimePicker1.Value.ToString("dd-MMM-yyyy");
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Vi_id"] = vid;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr2"] = textBox7.Text;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr"] = actualnarr;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Sno"] = fsno;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["LocationId"] = Database.LocationId;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Ac_id"] = acid;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = ac_id;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = -1 * double.Parse(textBox10.Text);
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["A"] = A;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["B"] = B;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["AB"] = AB;
                        }

                    }
                    else
                    {
                        for (int i = 0; i < dtpaymentdet.Rows.Count; i++)
                        {

                            fsno = i + 9001;
                            dtJournal.Rows.Add();
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["vdate"] = dateTimePicker1.Value.ToString("dd-MMM-yyyy");
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Vi_id"] = vid;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr2"] = textBox7.Text;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr"] = actualnarr;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Sno"] = fsno;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["LocationId"] = Database.LocationId;
                            //string ac_id = Database.GetScalarText("Select Ac_id from Account where Act_id='SER3' and Branch_id='" + Database.BranchId + "'");
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Ac_id"] = dtpaymentdet.Rows[i]["Acc_id"].ToString();
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = acid;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = double.Parse(dtpaymentdet.Rows[i]["Amount"].ToString());
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["A"] = A;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["B"] = B;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["AB"] = AB;

                            dtJournal.Rows.Add();
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["vdate"] = dateTimePicker1.Value.ToString("dd-MMM-yyyy");
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Vi_id"] = vid;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr2"] = textBox7.Text;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Narr"] = actualnarr;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Sno"] = fsno;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["LocationId"] = Database.LocationId;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Ac_id"] = acid;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Opp_acid"] = dtpaymentdet.Rows[i]["Acc_id"].ToString();
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["Amount"] = -1 * double.Parse(dtpaymentdet.Rows[i]["Amount"].ToString());
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["A"] = A;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["B"] = B;
                            dtJournal.Rows[dtJournal.Rows.Count - 1]["AB"] = AB;
                        }
                    }





                }


                for (int i = 0; i < dtJournal.Rows.Count; i++)
                {
                    if (gtype == "Purchase" || gtype == "P Return")
                    {
                        dtJournal.Rows[i]["Reffno"] = dtVoucherInfo.Rows[0]["Svnum"].ToString();
                    }
                    else
                    {
                        dtJournal.Rows[i]["Reffno"] = "";
                    }

                    if (gtype == "Purchase" || gtype == "Return" || gtype == "RCM" || gtype == "receive" || gtype == "PWDebitNote")
                    {
                        dtJournal.Rows[i]["Amount"] = -1 * double.Parse(dtJournal.Rows[i]["Amount"].ToString());
                    }


                }






                Database.SaveData(dtJournal);
            }




            dtTemp = new DataTable("Billadjest");
            Database.GetSqlData("Select * from Billadjest where Vi_id='" + vid + "'", dtTemp);
            for (int j = 0; j < dtTemp.Rows.Count; j++)
            {
                dtTemp.Rows[j].Delete();
            }
            Database.SaveData(dtTemp);

            dtBilladjest = new DataTable("Billadjest");
            Database.GetSqlData("Select * from Billadjest where Vi_id='" + vid + "'", dtBilladjest);


            if (funs.Select_MainAccTypeName(textBox14.Text) == "SUNDRY DEBTORS" || funs.Select_MainAccTypeName(textBox14.Text) == "SUNDRY CREDITORS")
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




                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Ac_id"] = funs.Select_ac_id(textBox14.Text);
                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Vi_id"] = vid;
                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Reff_id"] = vid;
                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Amount"] = double.Parse(dtJournal.Compute("Sum(Amount)", "Ac_id='" + funs.Select_ac_id(textBox14.Text) + "'").ToString());
                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["itemsr"] = 1;
                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["AdjustSr"] = 1;

                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["A"] = A;
                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["B"] = B;

                dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["AB"] = true;

            }


            Database.SaveData(dtBilladjest);



            Master.UpdateAccountinfo();
            funs.ShowBalloonTip("Saved", "Voucher Number: " + vno + " Saved Successfully");





        }

        private string SetNarr()
        {
            if (textBox7.Text != "")
            {
                return textBox7.Text;
            }
            else if (gtype == "Sale")
            {
                return "Being Goods Sold by " + funs.Select_vt_nm(vtid);
            }
            else if (gtype == "Opening")
            {
                return "Opening Stock";
            }
            else if (gtype == "Pending")
            {
                return "Being Goods Pendings";
            }
            else if (gtype == "Purchase")
            {
                return "Being Goods Purchase" + " Bill No." + textBox2.Text + " Dt. " + dateTimePicker2.Value.Date.ToString(Database.dformat);
            }
            else if (gtype == "PWDebitNote")
            {
                return "Being Goods Purchase With Debit Note";
            }
            else if (gtype == "P Return")
            {
                return "Being Goods Purchase Return" + " Bill No." + textBox2.Text + " Dt. " + dateTimePicker2.Value.Date.ToString(Database.dformat);
            }
            else if (gtype == "Temp")
            {
                return "Temporary Voucher";
            }
            else if (gtype == "Return")
            {
                return "Being Goods Return";
            }
            else if (gtype == "issue")
            {
                return "Stock issue";
            }
            else if (gtype == "receive")
            {
                return "Stock receive";
            }
            return textBox7.Text;
        }

        private void Sendsms()
        {
            if (funs.Select_AccTypeid(textBox14.Text) == "SER3")
            {
                return;
            }
            if (gtype != "Sale")
            {
                return;
            }
            if (funs.Select_Mobile(textBox14.Text) == "0")
            {
                return;
            }
            permission = funs.GetPermissionKey("SMS Setup");

            UsersFeature ob = permission.Where(w => w.FeatureName == "Send SMS").FirstOrDefault();

            if (ob != null && ob.SelectedValue == "No")
            {
                return;
            }
            else if (ob != null && ob.SelectedValue == "Ask")
            {
                if (MessageBox.Show("Are you want to send SMS?", "SMS", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }



            string msg = Database.GetScalarText("Select SmsTemplate from Vouchertype where Vt_id='" + vtid + "' ");
            msg = msg.Replace("{Vno}", vno.ToString());
            msg = msg.Replace("{Amount}", funs.IndianCurr(double.Parse(textBox10.Text)));
            msg = msg.Replace("\r", "");

            if (funs.isDouble(funs.Select_Mobile(textBox14.Text)) == true)
            {

                sms objsms = new sms();
                objsms.send(msg, funs.Select_Mobile(textBox14.Text), textBox14.Text);

            }





        }
        private void SendMail()
        {
            OtherReport rpt = new OtherReport();
            rpt.voucherprint(this, vtid, vid, "Email Copy", false, "Email");
        }

        public void SmtpClient_OnCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MailMessage mail = (MailMessage)e.UserState;
            string subject = mail.Subject;

            if (e.Cancelled)
            {
                status = "Send canceled for mail with subject [{0}]." + subject;
            }
            if (e.Error != null)
            {
                status = "Error {1} occurred when sending mail [{0}] " + subject + e.Error.ToString();
            }
            else
            {
                status = "Mail sent";
            }
            mailSent = true;
        }

        private void clear()
        {


            if (gStr == "")
            {
                flowLayoutPanel1.Controls.Clear();
                vno = 0;
                LoadData("", gtype, gExcludingTax, gExState, gUnregistered);
                dtVoucherInfo.Rows.Clear();
                dtVoucherDet.Rows.Clear();
                dtVoucherCharges1.Rows.Clear();
                dtVoucherCharges2.Rows.Clear();
                dtVoucherCharges3.Rows.Clear();
                dtVoucherCharges4.Rows.Clear();
                dtStock.Rows.Clear();
                dtJournal.Rows.Clear();



                field1 = "";
                field2 = "";
                field3 = "";
                field4 = "";
                field5 = "";
                field6 = "";
                field7 = "";
                field8 = "";

                ansGridView1.Rows.Clear();
                ansGridView3.Rows.Clear();
                ansGridView4.Rows.Clear();
                //  label10.Text = "";
                textBox1.Text = "0";
                textBox2.Text = "0";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                Vehicleno = "";
                TransportName = "";
                Transdocno = "";
                Transdocdate = dateTimePicker1.Value;
                Distance = 0;
                // 8874200906 balram

                if (gtype != "RCM")
                {
                    checkBox2.Checked = false;
                    checkBox3.Checked = false;
                    checkBox4.Checked = false;
                }
                else
                {
                    checkBox2.Checked = true;
                    checkBox3.Checked = false;
                    checkBox4.Checked = true;
                }
                textBox7.Text = "";
                textBox8.Text = "";
                textBox9.Text = "0";
                textBox10.Text = "";
                textBox14.Text = "";
                textBox17.Text = "";
                //taxes
                textBox6.Text = "0";
                textBox19.Text = "0";
                textBox21.Text = "0";
                textBox22.Text = "0";
                textBox23.Text = "0";
                f12used = false;
                locked = false;
                RoffChanged = false;
                TaxChanged = false;
                DirectChangeAmount = false;
                dateTimePicker1.Focus();
                dtVoucherInfo.Rows.Add();
                ansGridView1.Rows[0].Cells["Quantity"].Value = 0;
                ansGridView1.Rows[0].Cells["Rate_am"].Value = 0;
                ansGridView1.Rows[0].Cells["cd"].Value = 0;
                ansGridView1.Rows[0].Cells["qd"].Value = 0;
                ansGridView1.Rows[0].Cells["Amount"].Value = 0;
                ansGridView1.Rows[0].Cells["Taxabelamount"].Value = 0;
                ansGridView1.Rows[0].Cells["Category_Id"].Value = 0;
            }
            else
            {
                this.Close();
                this.Dispose();
            }
        }

        private void ansGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView1.CurrentCell.OwningColumn.Name == "Amount")
            {
                if (double.Parse(ansGridView1.Rows[e.RowIndex].Cells["Quantity"].Value.ToString()) == 0 && double.Parse(ansGridView1.Rows[e.RowIndex].Cells["rate_am"].Value.ToString()) == 0)
                {
                    ansGridView1.Rows[e.RowIndex].Cells["Quantity"].Value = 1;
                    ansGridView1.Rows[e.RowIndex].Cells["rate_am"].Value = funs.DecimalPoint(ansGridView1.Rows[e.RowIndex].Cells["Amount"].Value.ToString(), 2);
                }
                else if (double.Parse(ansGridView1.Rows[e.RowIndex].Cells["Quantity"].Value.ToString()) != 0 && double.Parse(ansGridView1.Rows[e.RowIndex].Cells["rate_am"].Value.ToString()) == 0)
                {
                    ansGridView1.Rows[e.RowIndex].Cells["rate_am"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[e.RowIndex].Cells["Amount"].Value.ToString()) / double.Parse(ansGridView1.Rows[e.RowIndex].Cells["Quantity"].Value.ToString()));
                }
                else if (double.Parse(ansGridView1.Rows[e.RowIndex].Cells["Quantity"].Value.ToString()) == 0 && double.Parse(ansGridView1.Rows[e.RowIndex].Cells["rate_am"].Value.ToString()) != 0)
                {
                    ansGridView1.Rows[e.RowIndex].Cells["Quantity"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[e.RowIndex].Cells["Amount"].Value.ToString()) / double.Parse(ansGridView1.Rows[e.RowIndex].Cells["rate_am"].Value.ToString()));
                }
                else if (double.Parse(ansGridView1.Rows[e.RowIndex].Cells["Quantity"].Value.ToString()) != 0 && double.Parse(ansGridView1.Rows[e.RowIndex].Cells["rate_am"].Value.ToString()) != 0)
                {
                    ansGridView1.Rows[e.RowIndex].Cells["rate_am"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[e.RowIndex].Cells["Amount"].Value.ToString()) / double.Parse(ansGridView1.Rows[e.RowIndex].Cells["Quantity"].Value.ToString()));
                }
                DirectChangeAmount = true;
            }
            if (ansGridView1.CurrentCell.OwningColumn.Name == "Category" && ansGridView1.Rows[e.RowIndex].Cells["Category"].Value.ToString() != "")
            {
                ansGridView1.Rows[e.RowIndex].Cells["Amount"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[e.RowIndex].Cells["Quantity"].Value.ToString()) * double.Parse(ansGridView1.Rows[e.RowIndex].Cells["rate_am"].Value.ToString()));
            }
            if (ansGridView1.CurrentCell.OwningColumn.Name == "comqty" && ansGridView1.Rows[e.RowIndex].Cells["comqty"].Value.ToString() != "")
            {
                string comqty = "0";
                DataTable dt = new DataTable();
                comqty = ansGridView1.CurrentCell.OwningRow.Cells["comqty"].Value.ToString();
                ansGridView1.CurrentCell.OwningRow.Cells["Quantity"].Value = double.Parse(dt.Compute(comqty, "").ToString());
            }
            if (ansGridView1.CurrentCell.OwningColumn.Name == "ptr1" && ansGridView1.Rows[e.RowIndex].Cells["ptr1"].Value.ToString() != "")
            {
                ItemSelected(true, ansGridView1.CurrentCell.RowIndex);
            }
            if (ansGridView1.CurrentCell.OwningColumn.Name == "ptr2" && ansGridView1.Rows[e.RowIndex].Cells["ptr2"].Value.ToString() != "")
            {
                ItemSelected(true, ansGridView1.CurrentCell.RowIndex);
            }
            if (ansGridView1.CurrentCell.OwningColumn.Name == "ptr3" && ansGridView1.Rows[e.RowIndex].Cells["ptr3"].Value.ToString() != "")
            {
                ItemSelected(true, ansGridView1.CurrentCell.RowIndex);
            }
            if (ansGridView1.CurrentCell.OwningColumn.Name == "qd" && ansGridView1.Rows[e.RowIndex].Cells["qd"].Value.ToString() != "")
            {
                DirectChangeAmount = false;
                TaxChanged = false;
            }
            if (ansGridView1.CurrentCell.OwningColumn.Name == "cd" && ansGridView1.Rows[e.RowIndex].Cells["cd"].Value.ToString() != "")
            {
                DirectChangeAmount = false;
                TaxChanged = false;
            }
            if (ansGridView1.CurrentCell.OwningColumn.Name == "flatdis" && ansGridView1.Rows[e.RowIndex].Cells["flatdis"].Value.ToString() != "")
            {
                DirectChangeAmount = false;
                TaxChanged = false;
            }
            if (ansGridView1.CurrentCell.OwningColumn.Name == "Batch_Code" && ansGridView1.Rows[e.RowIndex].Cells["Batch_Code"].Value != null && ansGridView1.Rows[e.RowIndex].Cells["Batch_Code"].Value.ToString() != "")
            {
                if (gtype == "Purchase")
                {
                    if (ansGridView1.Rows.Count == 2 || ansGridView1.Rows.Count >= 2)
                    {
                        ansGridView1.AllowUserToAddRows = false;
                        string FromBatchcode = ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Batch_Code"].Value.ToString();
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
                        string batchcode = ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Batch_Code"].Value.ToString();
                        for (int i = 0; i < no - 1; i++)
                        {
                            batchcode = BatctcodeGenearator(batchcode);
                            ansGridView1.Rows.Add();

                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["sno"].Value = ansGridView1.Rows.Count;
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["description"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["description"].Value;
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Quantity"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Quantity"].Value.ToString()), 3);
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["comqty"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["comqty"].Value.ToString();
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Rate_am"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Rate_am"].Value.ToString()), 2);
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Amount"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Amount"].Value.ToString()), 2);
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Des_ac_id"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Des_ac_id"].Value.ToString();
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Category_Id"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Category_Id"].Value.ToString();
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Category"].Value = funs.Select_tax_cat_nm(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Category_Id"].Value.ToString());
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Taxabelamount"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Taxabelamount"].Value.ToString()), 2);
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Batch_Code"].Value = batchcode;
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["unt"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["unt"].Value.ToString();
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["orgpack"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["orgpack"].Value.ToString();
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["pvalue"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["pvalue"].Value.ToString();
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["rate_unit"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["rate_unit"].Value.ToString();

                            if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["remark1"].Value == null)
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["remark1"].Value = "";
                            }
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["remark1"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["remark1"].Value;
                            if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["remark2"].Value == null)
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["remark2"].Value = "";
                            }
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["remark2"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["remark2"].Value;
                            if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["remark3"].Value == null)
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["remark3"].Value = "";
                            }
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["remark3"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["remark3"].Value;
                            if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["remark4"].Value == null)
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["remark4"].Value = "";
                            }
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["remark4"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["remark4"].Value;
                            if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["remarkreq"].Value.ToString() == "" || ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["remarkreq"].Value == null)
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["remarkreq"].Value = false;
                            }
                            if (bool.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["remarkreq"].Value.ToString()) == true)
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["remarkreq"].Value = "true";
                            }
                            else
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["remarkreq"].Value = "false";
                            }

                            if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["godown_id"].Value.ToString() == "<MAIN>")
                            {

                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["godown_id"].Value = "<MAIN>";
                            }
                            else
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["godown_id"].Value = funs.Select_ac_nm(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["godown_id"].Value.ToString());
                            }
                            if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["qd"].Value.ToString() == "" || ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["qd"].Value == null)
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["qd"].Value = 0;
                            }
                            else
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["qd"].Value = double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["qd"].Value.ToString());
                            }
                            if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["cd"].Value.ToString() == "" || ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["cd"].Value == null)
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["cd"].Value = 0;
                            }
                            else
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["cd"].Value = double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["cd"].Value.ToString());
                            }

                            if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Commission_per"].Value.ToString() == "" || ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Commission_per"].Value == null)
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Commission_per"].Value = 0;
                            }
                            else
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Commission_per"].Value = double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Commission_per"].Value.ToString());
                            }
                            if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["CommissionFix"].Value.ToString() == "" || ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["CommissionFix"].Value == null)
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["CommissionFix"].Value = 0;
                            }
                            else
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["CommissionFix"].Value = double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["CommissionFix"].Value.ToString());
                            }
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["MRP"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["MRP"].Value.ToString()), 2);
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Cost"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Cost"].Value.ToString()), 2);
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["CommissionFix"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["CommissionFix"].Value.ToString()), 2);
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["orgdesc"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["orgdesc"].Value.ToString();
                            //new fields
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["pur_sale_acc"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["pur_sale_acc"].Value.ToString();
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["tax1"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["tax1"].Value.ToString();
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["tax2"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["tax2"].Value.ToString();
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["tax3"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["tax3"].Value.ToString();
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["tax4"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["tax4"].Value.ToString();
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["rate1"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["rate1"].Value.ToString();
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["rate2"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["rate2"].Value.ToString();
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["rate3"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["rate3"].Value.ToString();
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["rate4"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["rate4"].Value.ToString();
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["taxamt1"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["taxamt1"].Value.ToString();
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["taxamt2"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["taxamt2"].Value.ToString();
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["taxamt3"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["taxamt3"].Value.ToString();
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["taxamt4"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["taxamt4"].Value.ToString();

                            if (ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["dattype"].Value == null)
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["dattype"].Value = "";
                            }

                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["dattype"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["dattype"].Value.ToString();
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["datamount"].Value = double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["datamount"].Value.ToString());
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["dat"].Value = double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["dat"].Value.ToString());
                            if (ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["datac_id"].Value == null || ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["datac_id"].Value.ToString() == "")
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["datac_id"].Value = 0;
                            }
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["datac_id"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["datac_id"].Value.ToString();
                            if (ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["RCMac_id"].Value == null || ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["RCMac_id"].Value.ToString() == "")
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["RCMac_id"].Value = 0;
                            }
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["RCMac_id"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["RCMac_id"].Value.ToString();

                            if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["bottomdis"].Value.ToString() == "" || ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["bottomdis"].Value == null)
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["bottomdis"].Value = 0;
                            }
                            else
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["bottomdis"].Value = double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["bottomdis"].Value.ToString());
                            }

                            if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["flatdis"].Value.ToString() == "" || ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["flatdis"].Value == null)
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["flatdis"].Value = 0;
                            }
                            else
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["flatdis"].Value = double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["flatdis"].Value.ToString());
                            }

                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Amount0"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Amount0"].Value.ToString()), 2);
                            if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["QDType"].Value == null)
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["QDType"].Value = "";
                            }

                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["QDType"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["QDType"].Value.ToString();
                            if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["QDAmount"].Value.ToString() == "" || ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["QDAmount"].Value == null)
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["QDAmount"].Value = 0;
                            }
                            else
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["QDAmount"].Value = double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["QDAmount"].Value.ToString());
                            }
                            if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Amount1"].Value.ToString() == "" || ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Amount1"].Value == null)
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Amount1"].Value = 0;
                            }
                            else
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Amount1"].Value = double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Amount1"].Value.ToString());
                            }
                            if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["CDType"].Value == null)
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["CDType"].Value = "";
                            }

                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["CDType"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["CDType"].Value.ToString();
                            if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["CDAmount"].Value.ToString() == "" || ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["CDAmount"].Value == null)
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["CDAmount"].Value = 0;
                            }
                            else
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["CDAmount"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["CDAmount"].Value.ToString()), 2);
                            }

                            if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Amount2"].Value.ToString() == "" || ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Amount2"].Value == null)
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Amount2"].Value = 0;
                            }
                            else
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Amount2"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Amount2"].Value.ToString()), 2);
                            }
                            if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["FDType"].Value == null)
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["FDType"].Value = "";
                            }

                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["FDType"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["FDType"].Value.ToString();
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["FDAmount"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["FDAmount"].Value.ToString()), 2);
                            if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Amount3"].Value.ToString() == "" || ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Amount3"].Value == null)
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Amount3"].Value = 0;
                            }
                            else
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Amount3"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Amount3"].Value.ToString()), 2);
                            }

                            if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["GridDis"].Value.ToString() == "" || ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["GridDis"].Value == null)
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["GridDis"].Value = 0;
                            }
                            else
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["GridDis"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["GridDis"].Value.ToString()), 2);
                            }

                            if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["TotalDis"].Value.ToString() == "" || ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["TotalDis"].Value == null)
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["TotalDis"].Value = 0;
                            }
                            else
                            {
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["TotalDis"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["TotalDis"].Value.ToString()), 2);
                            }

                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Amount4"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Amount4"].Value.ToString()), 2);
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["TotTaxPer"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["TotTaxPer"].Value.ToString()), 2);
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["TotTaxAmount"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["TotTaxAmount"].Value.ToString()), 2);
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Amount5"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Amount5"].Value.ToString()), 2);
                            ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["ExpAmount"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["ExpAmount"].Value.ToString()), 2);
                        }
                    }
                    ansGridView1.AllowUserToAddRows = true;
                }
                else
                {
                    ansGridView1.Columns["Batch_Code"].ReadOnly = true;
                }

                for (int j = 0; j < ansGridView1.Rows.Count - 1; j++)
                {
                    ItemCalc(j);
                }
            }

            if (ansGridView1.CurrentCell.OwningColumn.Name == "Quantity" && ansGridView1.Rows[e.RowIndex].Cells["Quantity"].Value.ToString() != "")
            {
                ansGridView1.Rows[e.RowIndex].Cells["Quantity"].Value = funs.DecimalPoint(ansGridView1.Rows[e.RowIndex].Cells["Quantity"].Value, 3);
                DirectChangeAmount = false;
                TaxChanged = false;

                permission = funs.GetPermissionKey("Transactions");

                UsersFeature ob1 = permission.Where(w => w.FeatureName == "Action on ChangeRate").FirstOrDefault();

                if (ob1 != null && ob1.SelectedValue == "Update With Account")
                {
                    if (textBox14.Text == "")
                    {
                        ansGridView1.Columns["Rate_am"].ReadOnly = true;
                        ansGridView1.Columns["Amount"].ReadOnly = true;
                    }
                    else
                    {
                        ansGridView1.Columns["Rate_am"].ReadOnly = false;
                        ansGridView1.Columns["Rate_am"].ReadOnly = false;
                    }
                }

            }
            //if (ansGridView1.CurrentCell.OwningColumn.Name == "MRP" && ansGridView1.Rows[e.RowIndex].Cells["MRP"].Value.ToString() != "")
            //{
            //    UpdateMRPRate();
            //}

            if (ansGridView1.CurrentCell.OwningColumn.Name == "Rate_am" && ansGridView1.Rows[e.RowIndex].Cells["Rate_am"].Value.ToString() != "")
            {
                ansGridView1.Rows[e.RowIndex].Cells["rate_am"].Value = funs.DecimalPoint(ansGridView1.Rows[e.RowIndex].Cells["rate_am"].Value);

                DirectChangeAmount = false;
                TaxChanged = false;



                if (gtype == "Sale" || gtype == "Return" || gtype == "Pending" || gtype == "issue" || gtype == "Sale Order")
                {

                    string desid = ansGridView1.Rows[e.RowIndex].Cells["des_ac_id"].Value.ToString();
                    double puramt = Database.GetScalarDecimal("Select purchase_rate from description where des_id='" + desid + "'");
                    double rate = double.Parse(ansGridView1.Rows[e.RowIndex].Cells["rate_am"].Value.ToString());
                    if (puramt > rate)
                    {
                        MessageBox.Show("Item Rate can't be less than Purchase Rate");
                        ansGridView1.Rows[e.RowIndex].Cells["rate_am"].Style.BackColor = Color.Red;
                        ansGridView1.Rows[e.RowIndex].Cells["rate_am"].Value = 0;
                    }
                    else
                    {
                        permission = funs.GetPermissionKey("Transactions");

                        UsersFeature ob1 = permission.Where(w => w.FeatureName == "Action on ChangeRate").FirstOrDefault();

                        if (ob1 != null && ob1.SelectedValue == "Ask")
                        {
                            if (textBox14.Text == "")
                            {
                                DialogResult res = MessageBox.Show("Want to save permanently?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                                if (res == DialogResult.Yes)
                                {
                                    UpdateMasterRate();
                                }
                            }
                            else
                            {
                                DialogResult res = MessageBox.Show("Want to Update Rate With Selected Account Only?" + Environment.NewLine + " Press Yes to Update Rate with Account" + Environment.NewLine + " Press No to Update Rate with Master" + Environment.NewLine + " Press Cancel to Update Rate for this Voucher Only ", "Confirm", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                                if (res == DialogResult.Yes)
                                {
                                    UpdatePartyRate();
                                }
                                else if (res == DialogResult.No)
                                {
                                    UpdateMasterRate();
                                }
                            }
                        }
                        permission = funs.GetPermissionKey("Transactions");

                        UsersFeature ob2 = permission.Where(w => w.FeatureName == "Action on ChangeRate").FirstOrDefault();

                        if (ob2 != null && ob2.SelectedValue == "Update With Account")
                        {
                            if (textBox14.Text != "")
                            {
                                ansGridView1.Columns["Rate_am"].ReadOnly = false;
                                UpdatePartyRate();
                            }
                        }

                        if (ob2 != null && ob2.SelectedValue == "Update With Master")
                        {
                            UpdateMasterRate();
                        }
                    }

                }
                if (gtype == "Purchase" || gtype == "P Return")
                {

                    permission = funs.GetPermissionKey("Transactions");

                    UsersFeature ob2 = permission.Where(w => w.FeatureName == "Action on ChangeRate").FirstOrDefault();
                    if (ob2 != null && ob2.SelectedValue == "Ask")
                    {
                        if (textBox14.Text == "")
                        {
                            DialogResult res = MessageBox.Show("Want to save permanently?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                            if (res == DialogResult.Yes)
                            {
                                UpdateMasterRate();
                            }
                        }
                        else
                        {
                            DialogResult res = MessageBox.Show("Want to Update Rate With Selected Account Only?" + Environment.NewLine + " Press Yes to Update Rate with Account" + Environment.NewLine + " Press No to Update Rate with Master" + Environment.NewLine + " Press Cancel to Update Rate for this Voucher Only ", "Confirm", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                            if (res == DialogResult.Yes)
                            {
                                UpdatePartyRate();
                            }
                            else if (res == DialogResult.No)
                            {
                                UpdateMasterRate();
                            }
                        }
                    }

                    permission = funs.GetPermissionKey("Transactions");

                    UsersFeature ob3 = permission.Where(w => w.FeatureName == "Action on ChangeRate").FirstOrDefault();
                    if (ob3 != null && ob3.SelectedValue == "Update With Account")
                    {
                        if (textBox14.Text != "")
                        {
                            ansGridView1.Columns["Rate_am"].ReadOnly = false;
                            UpdatePartyRate();
                        }
                    }
                    if (ob3 != null && ob3.SelectedValue == "Update With Master")
                    {
                        UpdateMasterRate();
                    }
                }

                if (double.Parse(ansGridView1.CurrentCell.OwningRow.Cells["rate_am"].Value.ToString()) <= 0)
                {
                    ansGridView1.CurrentRow.Cells["rate_am"].Style.BackColor = Color.Red;
                }
                else
                {
                    ansGridView1.CurrentRow.Cells["rate_am"].Style.BackColor = Color.White;
                }
            }
            ItemCalc(e.RowIndex);
            checkLock();
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


        private double MasterRate(int rowindex)
        {
            DataTable dtDescription = new DataTable("Description");
            Database.GetSqlData("select " + Ratesapp + " from description where Des_id='" + ansGridView1.Rows[rowindex].Cells["Des_ac_id"].Value.ToString() + "' ", dtDescription); ;
            return double.Parse(dtDescription.Rows[0][0].ToString());
        }

        private void UpdateMasterRate()
        {
            DataTable dtDescription = new DataTable("Description");
            Database.GetSqlData("select Des_id,Pack,Description," + Ratesapp + " from description where Des_id='" + ansGridView1.CurrentCell.OwningRow.Cells["Des_ac_id"].Value.ToString() + "' ", dtDescription);
            if (dtDescription.Rows.Count > 0)
            {

            }

            dtDescription.Rows[0][Ratesapp] = ansGridView1.CurrentCell.Value;
            Database.SaveData(dtDescription, "select Des_id,Pack,Description," + Ratesapp + " from description");
            Master.UpdateDecription();
            Master.UpdateDecriptionInfo();
        }

        private void UpdatePartyRate()
        {
            int cnt;
            cnt = Database.GetScalarInt("select count(*) from PARTYRATE where Des_id='" + ansGridView1.CurrentCell.OwningRow.Cells["Des_ac_id"].Value.ToString() + "' and Ac_id='" + funs.Select_ac_id(textBox14.Text) + "' ");
            if (cnt == 0)
            {
                Database.CommandExecutor("insert into PartyRate(Ac_id,Des_id,Rate) values('" + funs.Select_ac_id(textBox14.Text) + "','" + ansGridView1.CurrentCell.OwningRow.Cells["Des_ac_id"].Value + "','" + funs.DecimalPoint(ansGridView1.CurrentCell.OwningRow.Cells["rate_am"].Value) + "')");
            }
            else
            {
                Database.CommandExecutor("update PARTYRATE set Rate= " + funs.DecimalPoint(ansGridView1.CurrentCell.OwningRow.Cells["rate_am"].Value) + " where Des_id='" + ansGridView1.CurrentCell.OwningRow.Cells["Des_ac_id"].Value.ToString() + "' and Ac_id='" + funs.Select_ac_id(textBox14.Text) + "' ");
            }
        }

        private void UpdateMRPRate()
        {
            Database.CommandExecutor("update Description set MRP= " + funs.DecimalPoint(ansGridView1.CurrentCell.OwningRow.Cells["MRP"].Value) + " where Des_id='" + ansGridView1.CurrentCell.OwningRow.Cells["Des_ac_id"].Value.ToString() + "' ");
            Master.UpdateDecription();
            Master.UpdateDecriptionInfo();
        }

        private void ansGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            ansGridView1.CurrentCell.Value = 0;
        }

        private void ansGridView3_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView3.CurrentCell.OwningColumn.Name == "AmountA")
            {
                labelCalc();
            }

            if (ansGridView3.CurrentCell.OwningColumn.Name == "CamountA")
            {
                ansGridView3.Rows[e.RowIndex].Cells["Changed1"].Value = true;
                labelCalc();
            }
        }

        private void ansGridView3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || e.KeyChar == ' ' || Convert.ToInt32(e.KeyChar) == 13)
            {
                if (ansGridView3.CurrentCell.OwningColumn.Name == "Charg_Name")
                {
                    strCombo = "select [name] from charges where Ac_id='0'";
                    ansGridView3.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
                    if (ansGridView3.CurrentCell.Value.ToString() != "")
                    {
                        ansGridView3.CurrentCell.OwningRow.Cells["Charg_id1"].Value = funs.Select_ch_id(ansGridView3.CurrentCell.OwningRow.Cells["Charg_Name"].Value.ToString());
                    }
                    ansGridView3.CurrentCell.OwningRow.Cells["Entry_typ1"].Value = 1;

                    DataTable dtAddSub = new DataTable();
                    Database.GetSqlData("select Add_sub,Charge_type,Ac_id from charges where [name]='" + ansGridView3.CurrentCell.OwningRow.Cells["Charg_Name"].Value + "'", dtAddSub);
                    if (dtAddSub.Rows.Count == 1)
                    {
                        ansGridView3.CurrentCell.OwningRow.Cells["Addsub1"].Value = dtAddSub.Rows[0]["Add_sub"];
                        ansGridView3.CurrentCell.OwningRow.Cells["Ctype1"].Value = dtAddSub.Rows[0]["Charge_type"];
                        ansGridView3.CurrentCell.OwningRow.Cells["Accid1"].Value = dtAddSub.Rows[0]["Ac_id"];
                        ansGridView3.CurrentCell.OwningRow.Cells["Changed1"].Value = false;
                        ansGridView3.Columns["AmountA"].ReadOnly = false;
                        SendKeys.Send("{tab}");
                    }
                    else
                    {
                        ansGridView3.CurrentCell.Value = "";
                    }
                }
            }
        }

        private void labelCalc()
        {
            double subtot = 0, TotCdAmount = 0, totexpamt = 0, totdatamount = 0, totqty = 0, totweight = 0;
            for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
            {
                totweight += double.Parse(ansGridView1.Rows[i].Cells["Quantity"].Value.ToString()) * double.Parse(ansGridView1.Rows[i].Cells["pvalue"].Value.ToString());
                subtot += double.Parse(ansGridView1.Rows[i].Cells["Amount"].Value.ToString());
                totqty += double.Parse(ansGridView1.Rows[i].Cells["Quantity"].Value.ToString());
                if (ansGridView1.Rows[i].Cells["datamount"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["datamount"].Value = 0;
                }

                totdatamount += double.Parse(ansGridView1.Rows[i].Cells["datamount"].Value.ToString());
            }
            textBox4.Text = funs.DecimalPoint(subtot, 2);
            textBox16.Text = funs.DecimalPoint(totqty, 2);
            textBox20.Text = funs.DecimalPoint(totweight, 2);
            for (int i = 0; i < ansGridView3.RowCount - 1; i++)
            {
                DisCalc(i);
                TotCdAmount += double.Parse(ansGridView3.Rows[i].Cells["CamountA"].Value.ToString());
            }
            totbottomdis = -1 * TotCdAmount;

            textBox5.Text = funs.DecimalPoint(subtot - totbottomdis, 2);
            DisDistributor();
            TaxCalc();
            totdisaftertax = totdatamount;
            textBox26.Text = funs.DecimalPoint(-1 * totdisaftertax, 2);
            textBox25.Text = funs.DecimalPoint((double.Parse(textBox24.Text) + double.Parse(textBox23.Text)) - totdatamount, 2);

            for (int i = 0; i < ansGridView4.RowCount - 1; i++)
            {
                ExpCalc(i);
                totexpamt += double.Parse(ansGridView4.Rows[i].Cells["CamountB"].Value.ToString());
            }
            totexpamount = totexpamt;
            ExpDistributor();
            textBox8.Text = funs.DecimalPoint(totTaxabe + ctaxamt1 + ctaxamt2 + ctaxamt3 + ctaxamt4 + totexpamount - totdatamount, 2);
            textBox10.Text = funs.DecimalPoint(double.Parse(textBox8.Text), 0) + ".00";
            if (Feature.Available("Auto Roundoff") == "No" || gtype == "RCM")
            {
                textBox9.Text = funs.DecimalPoint(0, 0);
                textBox10.Text = funs.DecimalPoint(double.Parse(textBox8.Text), 0);
            }
            else
            {
                if (RoffChanged == false)
                {
                    textBox9.Text = funs.DecimalPoint((double.Parse(textBox10.Text) - double.Parse(textBox8.Text)));
                }
                else
                {
                    textBox10.Text = funs.DecimalPoint((double.Parse(textBox8.Text) - double.Parse(textBox9.Text)));
                }
            }
            textBox10.Text = funs.DecimalPoint((double.Parse(textBox8.Text) + double.Parse(textBox9.Text)));
        }

        private void ansGridView4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsNumber(e.KeyChar) || e.KeyChar == ' ' || Convert.ToInt32(e.KeyChar) == 13)
            {
                if (ansGridView4.CurrentCell.OwningColumn.Name == "Charg_Name2")
                {
                    strCombo = "select [name] from charges where Ac_id <> '0' UNION ALL select [name] from account where act_id = 'SER3'";
                    ansGridView4.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
                    if (ansGridView4.CurrentCell.Value.ToString() != "")
                    {
                        ansGridView4.CurrentCell.OwningRow.Cells["Charg_id2"].Value = funs.Select_ch_id(ansGridView4.CurrentCell.OwningRow.Cells["Charg_Name2"].Value.ToString());
                    }
                    ansGridView4.CurrentCell.OwningRow.Cells["Entry_typ2"].Value = 1;

                    DataTable dtAddSub2 = new DataTable();
                    Database.GetSqlData("select Add_sub,Charge_type,Ac_id from charges where [name]='" + ansGridView4.CurrentCell.OwningRow.Cells["Charg_Name2"].Value + "'", dtAddSub2);
                    if (dtAddSub2.Rows.Count == 1)
                    {
                        ansGridView4.CurrentCell.OwningRow.Cells["Addsub2"].Value = dtAddSub2.Rows[0]["Add_sub"];
                        ansGridView4.CurrentCell.OwningRow.Cells["Ctype2"].Value = dtAddSub2.Rows[0]["Charge_type"];
                        ansGridView4.CurrentCell.OwningRow.Cells["Accid2"].Value = dtAddSub2.Rows[0]["Ac_id"];
                        ansGridView4.CurrentCell.OwningRow.Cells["Changed2"].Value = false;
                        ansGridView4.Columns["AmountB"].ReadOnly = false;
                        SendKeys.Send("{tab}");
                    }
                    else
                    {
                        ansGridView4.CurrentCell.Value = "";
                    }
                }
            }
        }

        private void ansGridView4_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView4.CurrentCell.OwningColumn.Name == "AmountB")
            {
                labelCalc();
            }
            if (ansGridView4.CurrentCell.OwningColumn.Name == "CamountB")
            {
                ansGridView4.Rows[e.RowIndex].Cells["Changed2"].Value = true;
                labelCalc();
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

            if (funs.Select_vt_id_vnm(textBox15.Text) == "")
            {
                ansGridView1.Enabled = false;
                ansGridView3.Enabled = false;
                ansGridView4.Enabled = false;
            }
            else
            {
                ansGridView1.Enabled = true;
                ansGridView4.Enabled = true;
                if (ansGridView1.CurrentCell.OwningColumn.Name == "godown_id")
                {
                    strCombo = "select distinct  '<MAIN>' as name from account union all Select Name from Account where act_id='" + funs.Select_act_id("Godown") + "' order by Name";
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
                }

                if (ansGridView1.CurrentCell.OwningColumn.Name == "Category")
                {
                    strCombo = "Select Category_Name from TAXCATEGORY  order by Category_Name";
                    ansGridView1.CurrentCell.Value = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
                    ansGridView1.CurrentCell.OwningRow.Cells["Category_id"].Value = funs.Select_tax_cat_id(ansGridView1.CurrentCell.Value.ToString());
                    ItemCalc(ansGridView1.CurrentRow.Index);
                }

                if (ansGridView1.CurrentCell.OwningColumn.Name == "Batch_Code")
                {
                    if (gtype == "Sale")
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
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["comqty"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["comqty"].Value.ToString();
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Rate_am"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Rate_am"].Value.ToString()), 2);
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Amount"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Amount"].Value.ToString()), 2);
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Des_ac_id"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Des_ac_id"].Value.ToString();

                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Category_Id"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Category_Id"].Value.ToString();
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Category"].Value = funs.Select_tax_cat_nm(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Category_Id"].Value.ToString());
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Taxabelamount"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Taxabelamount"].Value.ToString()), 2);
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Batch_Code"].Value = dtfill.Rows[i]["batchno"].ToString();

                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["unt"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["unt"].Value.ToString();
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["orgpack"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["orgpack"].Value.ToString();
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["pvalue"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["pvalue"].Value.ToString();
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["rate_unit"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["rate_unit"].Value.ToString();

                                if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["remark1"].Value == null)
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["remark1"].Value = "";
                                }
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["remark1"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["remark1"].Value;
                                if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["remark2"].Value == null)
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["remark2"].Value = "";
                                }
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["remark2"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["remark2"].Value;
                                if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["remark3"].Value == null)
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["remark3"].Value = "";
                                }
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["remark3"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["remark3"].Value;
                                if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["remark4"].Value == null)
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["remark4"].Value = "";
                                }
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["remark4"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["remark4"].Value;
                                if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["remarkreq"].Value.ToString() == "" || ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["remarkreq"].Value == null)
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["remarkreq"].Value = false;
                                }
                                if (bool.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["remarkreq"].Value.ToString()) == true)
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["remarkreq"].Value = "true";
                                }
                                else
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["remarkreq"].Value = "false";
                                }
                                if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["godown_id"].Value.ToString() == "<MAIN>")
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["godown_id"].Value = "<MAIN>";
                                }
                                else
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["godown_id"].Value = funs.Select_ac_nm(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["godown_id"].Value.ToString());
                                }
                                if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["qd"].Value.ToString() == "" || ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["qd"].Value == null)
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["qd"].Value = 0;
                                }
                                else
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["qd"].Value = double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["qd"].Value.ToString());
                                }
                                if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["cd"].Value.ToString() == "" || ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["cd"].Value == null)
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["cd"].Value = 0;
                                }
                                else
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["cd"].Value = double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["cd"].Value.ToString());
                                }
                                if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Commission_per"].Value.ToString() == "" || ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Commission_per"].Value == null)
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Commission_per"].Value = 0;
                                }
                                else
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Commission_per"].Value = double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Commission_per"].Value.ToString());
                                }
                                if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["CommissionFix"].Value.ToString() == "" || ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["CommissionFix"].Value == null)
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["CommissionFix"].Value = 0;
                                }
                                else
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["CommissionFix"].Value = double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["CommissionFix"].Value.ToString());
                                }
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["MRP"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["MRP"].Value.ToString()), 2);
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Cost"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Cost"].Value.ToString()), 2);
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["CommissionFix"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["CommissionFix"].Value.ToString()), 2);
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["orgdesc"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["orgdesc"].Value.ToString();
                                //new fields
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["pur_sale_acc"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["pur_sale_acc"].Value.ToString();
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["tax1"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["tax1"].Value.ToString();
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["tax2"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["tax2"].Value.ToString();
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["tax3"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["tax3"].Value.ToString();
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["tax4"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["tax4"].Value.ToString();
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["rate1"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["rate1"].Value.ToString();
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["rate2"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["rate2"].Value.ToString();
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["rate3"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["rate3"].Value.ToString();
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["rate4"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["rate4"].Value.ToString();
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["taxamt1"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["taxamt1"].Value.ToString();
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["taxamt2"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["taxamt2"].Value.ToString();
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["taxamt3"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["taxamt3"].Value.ToString();
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["taxamt4"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["taxamt4"].Value.ToString();
                                if (ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["dattype"].Value == null)
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["dattype"].Value = "";
                                }
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["dattype"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["dattype"].Value.ToString();
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["datamount"].Value = double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["datamount"].Value.ToString());
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["dat"].Value = double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["dat"].Value.ToString());
                                if (ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["datac_id"].Value == null || ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["datac_id"].Value.ToString() == "")
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["datac_id"].Value = 0;
                                }
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["datac_id"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["datac_id"].Value.ToString();
                                if (ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["RCMac_id"].Value == null || ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["RCMac_id"].Value.ToString() == "")
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["RCMac_id"].Value = 0;
                                }
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["RCMac_id"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["RCMac_id"].Value.ToString();
                                if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["bottomdis"].Value.ToString() == "" || ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["bottomdis"].Value == null)
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["bottomdis"].Value = 0;
                                }
                                else
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["bottomdis"].Value = double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["bottomdis"].Value.ToString());
                                }
                                if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["flatdis"].Value.ToString() == "" || ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["flatdis"].Value == null)
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["flatdis"].Value = 0;
                                }
                                else
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["flatdis"].Value = double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["flatdis"].Value.ToString());
                                }
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Amount0"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Amount0"].Value.ToString()), 2);
                                if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["QDType"].Value == null)
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["QDType"].Value = "";
                                }
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["QDType"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["QDType"].Value.ToString();
                                if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["QDAmount"].Value.ToString() == "" || ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["QDAmount"].Value == null)
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["QDAmount"].Value = 0;
                                }
                                else
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["QDAmount"].Value = double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["QDAmount"].Value.ToString());
                                }
                                if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Amount1"].Value.ToString() == "" || ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Amount1"].Value == null)
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Amount1"].Value = 0;
                                }
                                else
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Amount1"].Value = double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Amount1"].Value.ToString());
                                }
                                if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["CDType"].Value == null)
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["CDType"].Value = "";
                                }
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["CDType"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["CDType"].Value.ToString();
                                if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["CDAmount"].Value.ToString() == "" || ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["CDAmount"].Value == null)
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["CDAmount"].Value = 0;
                                }
                                else
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["CDAmount"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["CDAmount"].Value.ToString()), 2);
                                }
                                if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Amount2"].Value.ToString() == "" || ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Amount2"].Value == null)
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Amount2"].Value = 0;
                                }
                                else
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Amount2"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Amount2"].Value.ToString()), 2);
                                }
                                if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["FDType"].Value == null)
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["FDType"].Value = "";
                                }
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["FDType"].Value = ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["FDType"].Value.ToString();
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["FDAmount"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["FDAmount"].Value.ToString()), 2);
                                if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Amount3"].Value.ToString() == "" || ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Amount3"].Value == null)
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Amount3"].Value = 0;
                                }
                                else
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Amount3"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Amount3"].Value.ToString()), 2);
                                }
                                if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["GridDis"].Value.ToString() == "" || ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["GridDis"].Value == null)
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["GridDis"].Value = 0;
                                }
                                else
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["GridDis"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["GridDis"].Value.ToString()), 2);
                                }
                                if (ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["TotalDis"].Value.ToString() == "" || ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["TotalDis"].Value == null)
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["TotalDis"].Value = 0;
                                }
                                else
                                {
                                    ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["TotalDis"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["TotalDis"].Value.ToString()), 2);
                                }
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Amount4"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Amount4"].Value.ToString()), 2);
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["TotTaxPer"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["TotTaxPer"].Value.ToString()), 2);
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["TotTaxAmount"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["TotTaxAmount"].Value.ToString()), 2);
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Amount5"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["Amount5"].Value.ToString()), 2);
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["ExpAmount"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[ansGridView1.Rows.Count - 2].Cells["ExpAmount"].Value.ToString()), 2);
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Rvi_id"].Value = 0;
                                ansGridView1.Rows[ansGridView1.Rows.Count - 1].Cells["Ritemsr"].Value = 0;
                            }
                            ansGridView1.AllowUserToAddRows = true;
                        }

                        for (int j = 0; j < ansGridView1.Rows.Count - 1; j++)
                        {
                            ItemCalc(j);
                        }
                    }
                }
                if (ansGridView1.CurrentCell == null)
                {
                    return;
                }

                if (ansGridView1.CurrentCell.OwningColumn.Name == "orgpack" || ansGridView1.CurrentCell.OwningColumn.Name == "description")
                {
                    String ActiveCell = "";
                    if (ansGridView1.CurrentCell.OwningColumn.Name == "orgpack")
                    {
                        ActiveCell = "Packing";
                    }
                    else if (ansGridView1.CurrentCell.OwningColumn.Name == "description")
                    {
                        ActiveCell = "Desc";
                    }
                    DataTable dtDesc = new DataTable();
                    if (Master.DescriptionInfo.Select("Description<>''", "Description, PACKING").Length == 0)
                    {
                        return;
                    }
                    else
                    {

                        dtDesc = Master.DescriptionInfo.Select("Description<>'' and Status='Enable'", "Description, PACKING").CopyToDataTable();

                    }
                    if (gtype == "Sale" || gtype == "Return")
                    {
                        if (funs.Select_vt_Exempted(vtid) == "Not Allowed")
                        {
                            dtDesc = dtDesc.Select("SaleTaxRate<>0").CopyToDataTable();
                        }
                        else if (funs.Select_vt_Exempted(vtid) == "Only Allowed")
                        {
                            dtDesc = dtDesc.Select("SaleTaxRate=0").CopyToDataTable();
                        }
                    }
                    else if (gtype == "Purchase" || gtype == "P Return" || gtype == "RCM" || gtype == "PWDebitNote")
                    {
                        if (funs.Select_vt_Exempted(vtid) == "Not Allowed")
                        {
                            dtDesc = dtDesc.Select("PurTaxRate<>0").CopyToDataTable();
                        }
                        else if (funs.Select_vt_Exempted(vtid) == "Only Allowed")
                        {
                            dtDesc = dtDesc.Select("PurTaxRate=0").CopyToDataTable();
                        }
                    }
                    else if (gtype == "Opening")
                    {
                        dtDesc = dtDesc.Select("").CopyToDataTable();
                    }

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
                    String packing = "", Desc = "";
                    if (ActiveCell == "Packing")
                    {
                        packing = SelectCombo.CallHelp(this, dtPSS, e.KeyChar.ToString(), 0);
                        if (packing == "") return;
                        dtDesc = dtDesc.Select("Packing='" + packing + "' or Skucode='" + packing + "' or ShortCode='" + packing + "'").CopyToDataTable();
                    }
                    else
                    {
                        Desc = SelectCombo.CallHelp(this, dtPSS, e.KeyChar.ToString(), 0);
                        if (Desc == "") return;
                        dtDesc = dtDesc.Select("description='" + Desc + "' or Skucode='" + Desc + "' or ShortCode='" + Desc + "'").CopyToDataTable();
                    }


                    // if (ActiveCell == "Packing")
                    //{

                    //    if (packing == "") return;
                    //    else
                    //    {
                    //        ansGridView1.CurrentCell.OwningRow.Cells["orgpack"].Value = packing;
                    //        ansGridView1.CurrentCell.OwningRow.Cells["unt"].Value = packing;
                    //    }
                    //}

                    //else
                    //{


                    //    if (Desc == "") return;
                    //    else
                    //    {
                    //        ansGridView1.CurrentCell.OwningRow.Cells["orgdesc"].Value = Desc;
                    //        ansGridView1.CurrentCell.Value = Desc;
                    //    }
                    //}
                    if (ansGridView1.CurrentCell.OwningRow.Cells["orgdesc"].Value != null && ansGridView1.CurrentCell.OwningRow.Cells["unt"].Value != null)
                    {

                        Desc = ansGridView1.CurrentCell.OwningRow.Cells["orgdesc"].Value.ToString();
                        packing = ansGridView1.CurrentCell.OwningRow.Cells["unt"].Value.ToString();
                    }
                    //if (ActiveCell == "Desc" && packing == "")
                    //{
                    //    Database.GetSqlData("select *,Pack as Packing from Description where description='" + Desc + "' ", dtDesc);
                    //}
                    //else if (ActiveCell == "Packing" && Desc == "")
                    //{
                    //    Database.GetSqlData("select *,Pack as Packing from Description where Pack='" + packing + "' ", dtDesc);
                    //}

                    //else if (Desc != "" && packing != "")
                    //{
                    //    Database.GetSqlData("select *,Pack as Packing from Description where description='" + Desc + "' and Pack='" + packing + "'", dtDesc);
                    //}
                    //else
                    //{
                    //    return;
                    //}
                    if (dtDesc.Rows.Count == 1)
                    {
                        if (Feature.Available("Change Packing Name") == "Yes" && ansGridView1.CurrentCell.OwningColumn.Name == "orgpack")
                        {
                            ansGridView1.CurrentCell = ansGridView1["orgpack", ansGridView1.CurrentCell.RowIndex];
                            ansGridView1.CurrentCell.OwningRow.Cells["unt"].Value = dtDesc.Rows[0]["Packing"];
                            ansGridView1.CurrentCell.OwningRow.Cells["orgpack"].Value = "";
                        }
                        else if (Feature.Available("Change Packing Name") == "No" && ansGridView1.CurrentCell.OwningColumn.Name == "orgpack")
                        {
                            if (Feature.Available("Compute Quantity") == "Yes")
                            {
                                ansGridView1.CurrentCell = ansGridView1["comqty", ansGridView1.CurrentCell.RowIndex];
                            }
                            else
                            {
                                ansGridView1.CurrentCell = ansGridView1["Quantity", ansGridView1.CurrentCell.RowIndex];
                            }
                            ansGridView1.CurrentCell.OwningRow.Cells["unt"].Value = dtDesc.Rows[0]["Packing"];
                            ansGridView1.CurrentCell.OwningRow.Cells["orgpack"].Value = dtDesc.Rows[0]["Packing"];
                        }
                        else if (Feature.Available("Change Packing Name") == "No" && (ansGridView1.CurrentCell.OwningColumn.Name == "description" && ansGridView1.CurrentCell.OwningRow.Cells["orgpack"].Value != ""))
                        {
                            if (Feature.Available("Compute Quantity") == "Yes")
                            {
                                ansGridView1.CurrentCell = ansGridView1["comqty", ansGridView1.CurrentCell.RowIndex];
                            }
                            else
                            {
                                ansGridView1.CurrentCell = ansGridView1["Quantity", ansGridView1.CurrentCell.RowIndex];
                            }
                            ansGridView1.CurrentCell.OwningRow.Cells["unt"].Value = dtDesc.Rows[0]["Packing"];
                            ansGridView1.CurrentCell.OwningRow.Cells["orgpack"].Value = dtDesc.Rows[0]["Packing"];
                        }

                        else if (Feature.Available("Change Packing Name") == "Yes" && (ansGridView1.CurrentCell.OwningColumn.Name == "description" && ansGridView1.CurrentCell.OwningRow.Cells["orgpack"].Value != ""))
                        {
                            if (Feature.Available("Compute Quantity") == "Yes")
                            {
                                ansGridView1.CurrentCell = ansGridView1["comqty", ansGridView1.CurrentCell.RowIndex];
                            }
                            else
                            {
                                ansGridView1.CurrentCell = ansGridView1["Quantity", ansGridView1.CurrentCell.RowIndex];
                            }
                            ansGridView1.CurrentCell.OwningRow.Cells["unt"].Value = dtDesc.Rows[0]["Packing"];
                            ansGridView1.CurrentCell.OwningRow.Cells["orgpack"].Value = dtDesc.Rows[0]["Packing"];
                        }

                        if (ansGridView1.CurrentCell.OwningRow.Cells["description"].Value == null || ansGridView1.CurrentCell.OwningRow.Cells["description"].Value.ToString() == "")
                        {
                            ansGridView1.CurrentCell.OwningRow.Cells["description"].Value = dtDesc.Rows[0]["description"];
                        }
                        ansGridView1.CurrentCell.OwningRow.Cells["orgdesc"].Value = dtDesc.Rows[0]["description"];
                        ansGridView1.CurrentCell.OwningRow.Cells["Quantity"].Value = 0;
                        ansGridView1.CurrentCell.OwningRow.Cells["comqty"].Value = 0;
                        if (dtDesc.Rows[0]["MRP"].ToString() == "")
                        {
                            dtDesc.Rows[0]["MRP"] = 0;
                        }
                        ansGridView1.CurrentCell.OwningRow.Cells["Rvi_id"].Value = 0;
                        ansGridView1.CurrentCell.OwningRow.Cells["RItemsr"].Value = 0;
                        ansGridView1.CurrentCell.OwningRow.Cells["Cost"].Value = dtDesc.Rows[0]["Purchase_rate"];
                        ansGridView1.CurrentCell.OwningRow.Cells["MRP"].Value = dtDesc.Rows[0]["MRP"];
                        ansGridView1.CurrentCell.OwningRow.Cells["pvalue"].Value = dtDesc.Rows[0]["pvalue"];
                        ansGridView1.CurrentCell.OwningRow.Cells["rate_unit"].Value = dtDesc.Rows[0]["rate_unit"];
                        ansGridView1.CurrentCell.OwningRow.Cells["Taxabelamount"].Value = 0;
                        ansGridView1.CurrentCell.OwningRow.Cells["Commission_per"].Value = 0;
                        ansGridView1.CurrentCell.OwningRow.Cells["CommissionFix"].Value = 0;
                        ansGridView1.CurrentCell.OwningRow.Cells["Des_ac_id"].Value = dtDesc.Rows[0]["Des_id"];
                        ansGridView1.CurrentCell.OwningRow.Cells["Category_Id"].Value = 0;

                        ansGridView1.CurrentCell.OwningRow.Cells["sqft"].Value = dtDesc.Rows[0]["Square_FT"];
                        ansGridView1.CurrentCell.OwningRow.Cells["sqmt"].Value = dtDesc.Rows[0]["Square_MT"];

                        if (gtype == "Sale" || gtype == "Return")
                        {
                            ansGridView1.CurrentCell.OwningRow.Cells["qd"].Value = dtDesc.Rows[0]["Srebate"];
                        }
                        else if (gtype == "Purchase" || gtype == "P Return")
                        {
                            ansGridView1.CurrentCell.OwningRow.Cells["qd"].Value = dtDesc.Rows[0]["Rebate"];
                        }

                        if (Database.BranchGodown_id == "")
                        {
                            string godownid = "";
                            godownid = Database.GetScalarText("Select Godown_id from Description where Des_id='" + dtDesc.Rows[0]["Des_id"].ToString() + "' ");
                            if (godownid == "" || godownid == "")
                            {
                                ansGridView1.CurrentCell.OwningRow.Cells["godown_id"].Value = "<MAIN>";
                            }
                            else
                            {
                                ansGridView1.CurrentCell.OwningRow.Cells["godown_id"].Value = funs.Select_ac_nm(godownid);
                            }
                        }
                        else
                        {
                            ansGridView1.CurrentCell.OwningRow.Cells["godown_id"].Value = funs.Select_ac_nm(Database.BranchGodown_id);
                        }

                        if (bool.Parse(dtDesc.Rows[0]["remarkreq"].ToString()) == true)
                        {
                            ansGridView1.CurrentCell.OwningRow.Cells["remarkreq"].Value = "true";
                        }
                        else
                        {
                            ansGridView1.CurrentCell.OwningRow.Cells["remarkreq"].Value = "false";
                        }
                        ItemSelected(true, ansGridView1.CurrentCell.RowIndex);
                        if (ActiveCell == "Desc")
                        {
                            if (bool.Parse(dtDesc.Rows[0]["Change_des"].ToString()) == true)
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
                        }

                        if (ActiveCell == "Packing")
                        {
                            if (Feature.Available("Change Packing Name") == "Yes")
                            {
                                InputBox box = new InputBox("Changed Packing", ansGridView1.CurrentCell.OwningRow.Cells["unt"].Value.ToString(), false);
                                box.ShowInTaskbar = false;
                                box.ShowDialog(this);
                                if (box.outStr != null)
                                {
                                    ansGridView1.CurrentCell.OwningRow.Cells["orgpack"].Value = box.outStr;
                                }
                            }
                        }
                        this.Activate();
                        ItemCalc(ansGridView1.CurrentCell.RowIndex);
                    }
                    else if (dtDesc.Rows.Count > 1)
                    {
                        if (ActiveCell == "Packing")
                        {
                            ansGridView1.CurrentCell.OwningRow.Cells["unt"].Value = dtDesc.Rows[0]["Packing"];
                            ansGridView1.CurrentCell.OwningRow.Cells["orgpack"].Value = dtDesc.Rows[0]["Packing"];
                            ansGridView1.CurrentCell.OwningRow.Cells["description"].Value = "";
                            ansGridView1.CurrentCell.OwningRow.Cells["orgdesc"].Value = "";
                        }
                        else
                        {
                            ansGridView1.CurrentCell.OwningRow.Cells["description"].Value = dtDesc.Rows[0]["description"];
                            ansGridView1.CurrentCell.OwningRow.Cells["orgdesc"].Value = dtDesc.Rows[0]["description"];
                            ansGridView1.CurrentCell.OwningRow.Cells["unt"].Value = "";
                            ansGridView1.CurrentCell.OwningRow.Cells["orgpack"].Value = "";
                        }

                        ansGridView1.CurrentCell.OwningRow.Cells["Taxabelamount"].Value = 0;
                        ansGridView1.CurrentCell.OwningRow.Cells["Commission_per"].Value = 0;
                        ansGridView1.CurrentCell.OwningRow.Cells["CommissionFix"].Value = 0;
                        ansGridView1.CurrentCell.OwningRow.Cells["Des_ac_id"].Value = 0;
                        ansGridView1.CurrentCell.OwningRow.Cells["Quantity"].Value = 0;
                        ansGridView1.CurrentCell.OwningRow.Cells["comqty"].Value = 0;
                        ansGridView1.CurrentCell.OwningRow.Cells["pvalue"].Value = 0;
                        ansGridView1.CurrentCell.OwningRow.Cells["rate_unit"].Value = "";
                        ansGridView1.CurrentCell.OwningRow.Cells["Rvi_id"].Value = 0;
                        ansGridView1.CurrentCell.OwningRow.Cells["RItemsr"].Value = 0;

                        ansGridView1.CurrentCell.OwningRow.Cells["sqft"].Value = 0;
                        ansGridView1.CurrentCell.OwningRow.Cells["sqmt"].Value = 0;
                        ansGridView1.CurrentCell.OwningRow.Cells["qd"].Value = 0;

                        if (bool.Parse(dtDesc.Rows[0]["remarkreq"].ToString()) == true)
                        {
                            ansGridView1.CurrentCell.OwningRow.Cells["remarkreq"].Value = "true";
                        }
                        else
                        {
                            ansGridView1.CurrentCell.OwningRow.Cells["remarkreq"].Value = "false";
                        }
                        ansGridView1.CurrentCell.OwningRow.Cells["Rate_am"].Value = 0;
                        ansGridView1.CurrentCell.OwningRow.Cells["Category_Id"].Value = 0;
                        ansGridView1.CurrentCell.OwningRow.Cells["Cost"].Value = 0;
                        ansGridView1.CurrentCell.OwningRow.Cells["MRP"].Value = 0;
                        ItemSelected(true, ansGridView1.CurrentCell.RowIndex);
                        if (ActiveCell == "Desc")
                        {
                            if (bool.Parse(dtDesc.Rows[0]["Change_des"].ToString()) == true)
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
                        }
                        if (ActiveCell == "Packing")
                        {
                            if (Feature.Available("Change Packing Name") == "Yes")
                            {
                                InputBox box = new InputBox("Changed Packing", ansGridView1.CurrentCell.OwningRow.Cells["unt"].Value.ToString(), false);
                                box.ShowInTaskbar = false;
                                box.ShowDialog(this);
                                if (box.outStr != null)
                                {
                                    ansGridView1.CurrentCell.OwningRow.Cells["orgpack"].Value = box.outStr;
                                }
                            }
                        }

                        this.Activate();
                        if (ActiveCell == "Packing")
                        {
                            ansGridView1.CurrentCell = ansGridView1["description", ansGridView1.CurrentCell.RowIndex];
                        }
                        else
                        {
                            ansGridView1.CurrentCell = ansGridView1["orgpack", ansGridView1.CurrentCell.RowIndex];
                        }
                    }
                }
            }
        }

        private void ExpDistributor()
        {
            double Exp = 0;
            for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
            {
                if (double.Parse(textBox4.Text) == 0)
                {
                    ansGridView1.Rows[i].Cells["ExpAmount"].Value = 0;
                    ansGridView1.Rows[i].Cells["Cost"].Value = 0;
                    Exp += 0;
                }
                else
                {
                    if (ansGridView1.Rows[i].Cells["Amount5"].Value == null) ansGridView1.Rows[i].Cells["Amount5"].Value = 0;
                    ansGridView1.Rows[i].Cells["ExpAmount"].Value = funs.DecimalPoint(totexpamount / (double.Parse(textBox23.Text) + double.Parse(textBox24.Text)) * double.Parse(ansGridView1.Rows[i].Cells["Amount5"].Value.ToString()), 2);
                    ansGridView1.Rows[i].Cells["Cost"].Value = funs.DecimalPoint(double.Parse(funs.DecimalPoint(ansGridView1.Rows[i].Cells["ExpAmount"].Value, 2)) + double.Parse(funs.DecimalPoint(ansGridView1.Rows[i].Cells["Amount5"].Value, 2)) + double.Parse(funs.DecimalPoint(ansGridView1.Rows[i].Cells["datamount"].Value, 2)), 2);
                    Exp += double.Parse(funs.DecimalPoint(ansGridView1.Rows[i].Cells["ExpAmount"].Value, 2));
                }
            }
            if (ansGridView1.Rows.Count > 1 && totexpamount != Exp)
            {
                double diff = 0;
                diff = double.Parse(funs.DecimalPoint(totexpamount - Exp, 2));
                ansGridView1.Rows[0].Cells["ExpAmount"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[0].Cells["ExpAmount"].Value.ToString()) + diff, 2);
                ansGridView1.Rows[0].Cells["Cost"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[0].Cells["Cost"].Value.ToString()) + diff, 2);
            }
        }

        private void DisDistributor()
        {
            double bottomdis = 0;
            for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
            {
                if (double.Parse(textBox4.Text) == 0)
                {
                    ansGridView1.Rows[i].Cells["bottomdis"].Value = 0;
                    bottomdis += 0;
                    ansGridView1.Rows[i].Cells["TotalDis"].Value = 0;
                    ansGridView1.Rows[i].Cells["Amount4"].Value = 0;
                }
                else
                {
                    ansGridView1.Rows[i].Cells["bottomdis"].Value = funs.DecimalPoint(totbottomdis / double.Parse(textBox4.Text) * double.Parse(ansGridView1.Rows[i].Cells["Amount"].Value.ToString()), 2);
                    bottomdis += double.Parse(funs.DecimalPoint(ansGridView1.Rows[i].Cells["bottomdis"].Value, 2));
                    if (ansGridView1.Rows[i].Cells["QDAmount"].Value == null)
                    {
                        ansGridView1.Rows[i].Cells["QDAmount"].Value = 0;
                    }
                    if (ansGridView1.Rows[i].Cells["CDAmount"].Value == null)
                    {
                        ansGridView1.Rows[i].Cells["CDAmount"].Value = 0;
                    }
                    if (ansGridView1.Rows[i].Cells["FDAmount"].Value == null)
                    {
                        ansGridView1.Rows[i].Cells["FDAmount"].Value = 0;
                    }
                    if (ansGridView1.Rows[i].Cells["GridDis"].Value == null)
                    {
                        ansGridView1.Rows[i].Cells["GridDis"].Value = 0;
                    }
                    ansGridView1.Rows[i].Cells["TotalDis"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[i].Cells["QDAmount"].Value.ToString()) + double.Parse(ansGridView1.Rows[i].Cells["CDAmount"].Value.ToString()) + double.Parse(ansGridView1.Rows[i].Cells["FDAmount"].Value.ToString()) + double.Parse(ansGridView1.Rows[i].Cells["GridDis"].Value.ToString()) + double.Parse(ansGridView1.Rows[i].Cells["bottomdis"].Value.ToString()), 2);
                    ansGridView1.Rows[i].Cells["Amount4"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[i].Cells["Amount"].Value.ToString()) - double.Parse(ansGridView1.Rows[i].Cells["bottomdis"].Value.ToString()), 2);
                }
            }

            if (ansGridView1.Rows.Count > 1 && bottomdis != totbottomdis)
            {
                double diff = 0;
                diff = double.Parse(funs.DecimalPoint(totbottomdis - bottomdis, 2));
                ansGridView1.Rows[0].Cells["bottomdis"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[0].Cells["bottomdis"].Value.ToString()) - diff, 2);
                ansGridView1.Rows[0].Cells["TotalDis"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[0].Cells["TotalDis"].Value.ToString()) - diff, 2);
                ansGridView1.Rows[0].Cells["Amount4"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[0].Cells["Amount4"].Value.ToString()) - diff, 2);
            }
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

                if (ansGridView1.CurrentRow.Index == ansGridView1.Rows.Count - 1 && double.Parse(ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Amount"].Value.ToString()) == 0)
                {
                    SendKeys.Send("{tab}");
                }
            }

            if (e.KeyValue == 38)     // up arrow
            {
                if (ansGridView1.CurrentCell.OwningColumn.Name == "description")
                {
                    if (ansGridView1.CurrentRow.Index >= 1)
                    {
                        desc = ansGridView1.Rows[ansGridView1.CurrentRow.Index - 1].Cells["description"].Value.ToString();
                        unit = ansGridView1.Rows[ansGridView1.CurrentRow.Index - 1].Cells["unt"].Value.ToString();
                        getDescid(desc, unit);
                    }
                }
            }

            if (e.KeyCode == Keys.Delete)
            {
                if (ansGridView1.CurrentRow.Index == ansGridView1.Rows.Count - 1)
                {
                    for (int i = 1; i < ansGridView1.Columns.Count; i++)
                    {
                        ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells[i].Value = null;
                        labelCalc();
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
                    for (int i = 0; i < dtItemCharges.Rows.Count; i++)
                    {
                        if (dtItemCharges.Rows[i].RowState.ToString() == "Deleted" || int.Parse(dtItemCharges.Rows[i]["Itemsr"].ToString()) < rindex + 1)
                        {
                        }
                        else if (int.Parse(dtItemCharges.Rows[i]["Itemsr"].ToString()) == rindex + 1)
                        {
                            dtItemCharges.Rows[i].Delete();
                        }
                        else if (int.Parse(dtItemCharges.Rows[i]["Itemsr"].ToString()) > rindex + 1)
                        {
                            dtItemCharges.Rows[i]["Itemsr"] = int.Parse(dtItemCharges.Rows[i]["Itemsr"].ToString()) - 1;
                        }
                    }

                    labelCalc();
                    return;
                }
            }
            if (ansGridView1.CurrentCell.OwningColumn.Name == "godown_id")
            {
                if (e.Control && e.KeyCode == Keys.A)
                {
                    if (ansGridView1.CurrentCell.Value.ToString() != "" || ansGridView1.CurrentCell.Value != null)
                    {
                        ansGridView1.CurrentCell.Value = funs.EditAccount(ansGridView1.CurrentCell.OwningRow.Cells["godown_id"].Value.ToString());
                    }
                }

                else if (e.Control && e.KeyCode == Keys.C)
                {
                    ansGridView1.CurrentCell.Value = funs.AddAccount();
                }
            }
            if (ansGridView1.CurrentCell.OwningColumn.Name == "description")
            {
                if (Database.utype.ToUpper() == "ADMIN" || Database.utype.ToUpper() == "SUPERADMIN")
                {
                    if (e.Control && e.KeyCode == Keys.A)
                    {
                        if ((ansGridView1.CurrentCell.Value.ToString() != "") || (ansGridView1.CurrentCell.Value != null && ansGridView1.CurrentCell.OwningRow.Cells["Des_ac_id"].Value != null && ansGridView1.CurrentCell.OwningRow.Cells["unt"].Value != null || ansGridView1.CurrentCell.OwningRow.Cells["unt"].Value != ""))
                        {
                            string tdid = funs.EditDescription(ansGridView1.CurrentCell.OwningRow.Cells["orgdesc"].Value.ToString());
                            if (tdid != "")
                            {
                                ansGridView1.CurrentCell.OwningRow.Cells["orgdesc"].Value = tdid;
                                ansGridView1.CurrentCell.OwningRow.Cells["description"].Value = tdid;
                                if (Master.Description.Select("description='" + ansGridView1.CurrentCell.OwningRow.Cells["orgdesc"].Value + "'", "").Length == 0)
                                {
                                    return;
                                }
                                else if (Master.Description.Select("description='" + ansGridView1.CurrentCell.OwningRow.Cells["orgdesc"].Value + "'", "").Length == 1)
                                {
                                    ansGridView1.CurrentCell.OwningRow.Cells["Rvi_id"].Value = 0;
                                    ansGridView1.CurrentCell.OwningRow.Cells["RItemsr"].Value = 0;
                                    ansGridView1.CurrentCell.OwningRow.Cells["unt"].Value = Master.Description.Select("description='" + ansGridView1.CurrentCell.OwningRow.Cells["orgdesc"].Value + "'").FirstOrDefault()["pack"].ToString();
                                    ansGridView1.CurrentCell.OwningRow.Cells["pvalue"].Value = Master.Description.Select("description='" + ansGridView1.CurrentCell.OwningRow.Cells["orgdesc"].Value + "'").FirstOrDefault()["pvalue"].ToString();
                                    ansGridView1.CurrentCell.OwningRow.Cells["rate_unit"].Value = Master.Description.Select("description='" + ansGridView1.CurrentCell.OwningRow.Cells["orgdesc"].Value + "'").FirstOrDefault()["rate_unit"].ToString();
                                }
                                else
                                {
                                    ansGridView1.CurrentCell.OwningRow.Cells["unt"].Value = "";
                                    ansGridView1.CurrentCell.OwningRow.Cells["pvalue"].Value = 0;
                                    ansGridView1.CurrentCell.OwningRow.Cells["rate_unit"].Value = "";
                                }
                                ItemSelected(true, ansGridView1.CurrentCell.RowIndex);
                                ItemCalc(ansGridView1.CurrentCell.RowIndex);
                            }
                        }
                    }
                    else if (e.Control && e.KeyCode == Keys.C)
                    {
                        string tdid = funs.AddDescription();
                        ansGridView1.CurrentCell.OwningRow.Cells["orgdesc"].Value = tdid;
                        ansGridView1.CurrentCell.OwningRow.Cells["description"].Value = tdid;
                        if (Master.Description.Select("description='" + ansGridView1.CurrentCell.OwningRow.Cells["description"].Value + "'", "").Length == 0)
                        {
                            return;
                        }
                        else
                        {
                            ansGridView1.CurrentCell.OwningRow.Cells["Rvi_id"].Value = 0;
                            ansGridView1.CurrentCell.OwningRow.Cells["RItemsr"].Value = 0;
                            ansGridView1.CurrentCell.OwningRow.Cells["unt"].Value = "";
                            ansGridView1.CurrentCell.OwningRow.Cells["orgpack"].Value = "";
                            ansGridView1.CurrentCell.OwningRow.Cells["pvalue"].Value = 0;
                            ansGridView1.CurrentCell.OwningRow.Cells["rate_unit"].Value = "";
                        }
                        ItemSelected(true, ansGridView1.CurrentCell.RowIndex);
                    }
                }
            }

            if (e.Control && e.KeyCode == Keys.F10)
            {
                String str = "";
                if (ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Des_ac_id"].Value != null)
                {
                    if (textBox14.Text != "")
                    {
                        if (gtype == "Sale" || gtype == "Return")
                        {
                            strCombo = "SELECT VOUCHERTYPE.Name AS VoucherType, VOUCHERINFO.Vnumber," + access_sql.DateFormat + " AS BillDate, VOUCHERDET.Description, VOUCHERDET.Quantity, Voucherdet.Packing AS Unit, VOUCHERDET.Rate_am AS Rate, VOUCHERDET.qd, VOUCHERDET.cd FROM VOUCHERTYPE INNER JOIN ((VOUCHERINFO INNER JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) INNER JOIN DESCRIPTION ON VOUCHERDET.Des_ac_id = DESCRIPTION.Des_id) ON VOUCHERTYPE.Vt_id = VOUCHERINFO.Vt_id WHERE (((VOUCHERINFO.Ac_id)='" + funs.Select_ac_id(textBox14.Text) + "') AND ((VOUCHERDET.Des_ac_id)='" + ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Des_ac_id"].Value.ToString() + "') AND ((VOUCHERTYPE.Type)='Sale' Or (VOUCHERTYPE.Type)='Return')) ORDER BY VOUCHERTYPE.Name, VOUCHERINFO.Vdate, VOUCHERINFO.Vnumber";
                        }
                        else
                        {
                            strCombo = "SELECT VOUCHERTYPE.Name AS VoucherType, VOUCHERINFO.Vnumber, " + access_sql.DateFormat + " AS BillDate, VOUCHERDET.Description, VOUCHERDET.Quantity, Voucherdet.Packing AS Unit, VOUCHERDET.Rate_am AS Rate, VOUCHERDET.qd, VOUCHERDET.cd FROM VOUCHERTYPE INNER JOIN ((VOUCHERINFO INNER JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) INNER JOIN DESCRIPTION ON VOUCHERDET.Des_ac_id = DESCRIPTION.Des_id) ON VOUCHERTYPE.Vt_id = VOUCHERINFO.Vt_id WHERE (((VOUCHERINFO.Ac_id)='" + funs.Select_ac_id(textBox14.Text) + "') AND ((VOUCHERDET.Des_ac_id)='" + ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Des_ac_id"].Value.ToString() + "') AND ((VOUCHERTYPE.Type)='Purchase' Or (VOUCHERTYPE.Type)='P Return')) ORDER BY VOUCHERTYPE.Name, VOUCHERINFO.Vdate, VOUCHERINFO.Vnumber";
                        }

                        str = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, "", 9);
                    }

                    else
                    {
                        if (gtype == "Sale" || gtype == "Return")
                        {
                            strCombo = "SELECT VOUCHERTYPE.Name AS VoucherType, VOUCHERINFO.Vnumber, " + access_sql.DateFormat + " AS BillDate, VOUCHERDET.Description, VOUCHERDET.Quantity, Voucherdet.Packing AS Unit, VOUCHERDET.Rate_am AS Rate, VOUCHERDET.qd, VOUCHERDET.cd FROM VOUCHERTYPE INNER JOIN ((VOUCHERINFO INNER JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) INNER JOIN DESCRIPTION ON VOUCHERDET.Des_ac_id = DESCRIPTION.Des_id)  ON VOUCHERTYPE.Vt_id = VOUCHERINFO.Vt_id WHERE (((VOUCHERDET.Des_ac_id)='" + ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Des_ac_id"].Value.ToString() + "') AND ((VOUCHERTYPE.Type)='Sale' Or (VOUCHERTYPE.Type)='Return')) ORDER BY VOUCHERTYPE.Name, VOUCHERINFO.Vdate, VOUCHERINFO.Vnumber";
                        }
                        else
                        {
                            strCombo = "SELECT VOUCHERTYPE.Name AS VoucherType, VOUCHERINFO.Vnumber, " + access_sql.DateFormat + " AS BillDate, VOUCHERDET.Description, VOUCHERDET.Quantity, Voucherdet.Packing AS Unit, VOUCHERDET.Rate_am AS Rate, VOUCHERDET.qd, VOUCHERDET.cd FROM VOUCHERTYPE INNER JOIN ((VOUCHERINFO INNER JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) INNER JOIN DESCRIPTION ON VOUCHERDET.Des_ac_id = DESCRIPTION.Des_id)  ON VOUCHERTYPE.Vt_id = VOUCHERINFO.Vt_id WHERE (((VOUCHERDET.Des_ac_id)='" + ansGridView1.Rows[ansGridView1.CurrentRow.Index].Cells["Des_ac_id"].Value.ToString() + "') AND ((VOUCHERTYPE.Type)='Purchase' Or (VOUCHERTYPE.Type)='P Return')) ORDER BY VOUCHERTYPE.Name, VOUCHERINFO.Vdate, VOUCHERINFO.Vnumber";
                        }
                        str = SelectCombo.ComboKeydown(this, e.KeyCode, strCombo, "", 9);
                    }
                }
            }
        }

        private void ansGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            ansGridView1.Rows[e.RowIndex].Cells["sno"].Value = e.RowIndex + 1;
            if (ansGridView1.CurrentCell.OwningColumn.Name == "sno")
            {
                SendKeys.Send("{right}");
                this.Activate();
            }
            if (ansGridView1.CurrentCell.OwningColumn.Name == "Batch_Code")
            {
                if (gtype == "Sale")
                {
                    ansGridView1.Columns["Batch_Code"].ReadOnly = true;
                }
            }
        }

        private void ansGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            string des_id = "";
            if (des_id == "")
            {
                try
                {
                    des_id = ansGridView1.Rows[e.RowIndex].Cells["Des_ac_id"].Value.ToString();
                }
                catch (Exception ex)
                {
                }
            }
            DisplayStock(des_id);
        }

        private void Updatebottomdis()
        {
            double totdisamt = 0;
            totdisamt = double.Parse(textBox4.Text) - double.Parse(textBox5.Text);
            double calTaxable = 0;
            calTaxable = double.Parse(textBox4.Text);

            for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
            {
                ansGridView1.Rows[i].Cells["bottomdis"].Value = 0;
            }
        }

        private void ItemSelected(bool ChangeRate, int rowindex)
        {
            DataTable dtRateComm = new DataTable();
            DataTable dtRate = new DataTable();
            if (ansGridView1.Rows[rowindex].Cells["unt"].Value == null || ansGridView1.Rows[rowindex].Cells["unt"].Value == "")
            {
                if (Master.DescriptionInfo.Select("description='" + ansGridView1.Rows[rowindex].Cells["description"].Value + "'", "").Length == 0)
                {
                    return;
                }
                else
                {
                    dtRateComm = Master.Description.Select("description='" + ansGridView1.Rows[rowindex].Cells["description"].Value + "'", "").CopyToDataTable();
                }

                if (dtRateComm.Rows.Count > 0)
                {
                    if (dtRateComm.Rows.Count == 1)
                    {
                        if (ChangeRate == true)
                        {
                            Database.GetSqlData("SELECT PARTYRATE.Rate FROM (PARTYRATE INNER JOIN DESCRIPTION ON PARTYRATE.Des_id = DESCRIPTION.Des_id) INNER JOIN ACCOUNT ON PARTYRATE.Ac_id = ACCOUNT.Ac_id WHERE (((DESCRIPTION.Description)='" + ansGridView1.Rows[rowindex].Cells["Description"].Value + "')  AND ((ACCOUNT.Name)='" + textBox14.Text + "'))", dtRate);
                            if (dtRate.Rows.Count != 0)
                            {
                                ansGridView1.Rows[rowindex].Cells["rate_am"].Value = funs.DecimalPoint(dtRate.Rows[0]["Rate"]);
                            }
                            else
                            {
                                ansGridView1.Rows[rowindex].Cells["rate_am"].Value = funs.DecimalPoint(dtRateComm.Rows[0][Ratesapp]);
                                if (double.Parse(ansGridView1.Rows[rowindex].Cells["rate_am"].Value.ToString()) <= 0)
                                {
                                    ansGridView1.Rows[rowindex].Cells["rate_am"].Style.BackColor = Color.Red;
                                }
                            }
                        }

                        if (bool.Parse(dtRateComm.Rows[0]["remarkreq"].ToString()) == true)
                        {
                            ansGridView1.Rows[rowindex].Cells["remarkreq"].Value = "true";
                        }
                        else
                        {
                            ansGridView1.Rows[rowindex].Cells["remarkreq"].Value = "false";
                        }
                        ansGridView1.Rows[rowindex].Cells["Commission_per"].Value = dtRateComm.Rows[0]["Commission%"];
                        ansGridView1.Rows[rowindex].Cells["CommissionFix"].Value = dtRateComm.Rows[0]["Commission@"];
                        ansGridView1.Rows[rowindex].Cells["Category_Id"].Value = dtRateComm.Rows[0]["Tax_Cat_id"];
                        ansGridView1.Rows[rowindex].Cells["Category"].Value = funs.Select_tax_cat_nm(dtRateComm.Rows[0]["Tax_Cat_id"].ToString());
                        ansGridView1.Rows[rowindex].Cells["QDType"].Value = Feature.Available("Type of Discount1");
                        ansGridView1.Rows[rowindex].Cells["CDType"].Value = Feature.Available("Type of Discount2");
                        ansGridView1.Rows[rowindex].Cells["FDType"].Value = Feature.Available("Type of Discount3");
                        ansGridView1.Rows[rowindex].Cells["Des_ac_id"].Value = dtRateComm.Rows[0]["Des_id"];
                        ansGridView1.Rows[rowindex].Cells["MRP"].Value = dtRateComm.Rows[0]["MRP"];
                        ansGridView1.Rows[rowindex].Cells["pvalue"].Value = dtRateComm.Rows[0]["pvalue"];
                        ansGridView1.Rows[rowindex].Cells["rate_unit"].Value = dtRateComm.Rows[0]["rate_unit"];
                        desc_id = dtRateComm.Rows[0]["Des_id"].ToString();
                        ansGridView1.Rows[rowindex].Cells["qd"].Value = 0;
                        ansGridView1.Rows[rowindex].Cells["cd"].Value = 0;
                        ansGridView1.Rows[rowindex].Cells["flatdis"].Value = 0;
                        ansGridView1.Rows[rowindex].Cells["GridDis"].Value = 0;
                    }
                    else
                    {
                        ansGridView1.Rows[rowindex].Cells["remarkreq"].Value = false;
                        ansGridView1.Rows[rowindex].Cells["rate_am"].Value = 0;
                        ansGridView1.Rows[rowindex].Cells["Commission_per"].Value = 0;
                        ansGridView1.Rows[rowindex].Cells["CommissionFix"].Value = 0;
                        ansGridView1.Rows[rowindex].Cells["Category_Id"].Value = 0;
                        ansGridView1.Rows[rowindex].Cells["flatdis"].Value = 0;
                        ansGridView1.Rows[rowindex].Cells["pur_sale_acc"].Value = 0;
                        ansGridView1.Rows[rowindex].Cells["tax1"].Value = 0;
                        ansGridView1.Rows[rowindex].Cells["tax2"].Value = 0;
                        ansGridView1.Rows[rowindex].Cells["tax3"].Value = 0;
                        ansGridView1.Rows[rowindex].Cells["tax4"].Value = 0;
                        ansGridView1.Rows[rowindex].Cells["rate1"].Value = 0;
                        ansGridView1.Rows[rowindex].Cells["rate2"].Value = 0;
                        ansGridView1.Rows[rowindex].Cells["rate3"].Value = 0;
                        ansGridView1.Rows[rowindex].Cells["rate4"].Value = 0;
                        ansGridView1.Rows[rowindex].Cells["GridDis"].Value = 0;
                        ansGridView1.Rows[rowindex].Cells["Category"].Value = "";
                        ansGridView1.Rows[rowindex].Cells["Des_ac_id"].Value = 0;
                        ansGridView1.Rows[rowindex].Cells["MRP"].Value = 0;
                        desc_id = "";
                        ansGridView1.Rows[rowindex].Cells["pvalue"].Value = 0;
                        ansGridView1.Rows[rowindex].Cells["rate_unit"].Value = "";
                        ansGridView1.Rows[rowindex].Cells["qd"].Value = 0;
                        ansGridView1.Rows[rowindex].Cells["cd"].Value = 0;
                        ansGridView1.Rows[rowindex].Cells["flatdis"].Value = 0;
                        ansGridView1.Rows[rowindex].Cells["GridDis"].Value = 0;

                        if (Master.TaxCategory.Select("Category_id='" + ansGridView1.Rows[rowindex].Cells["Category_id"].Value.ToString() + "' ").Length > 0)
                        {
                            DataRow dr = Master.TaxCategory.Select("Category_id='" + ansGridView1.Rows[rowindex].Cells["Category_id"].Value.ToString() + "' ").FirstOrDefault();

                            if ((gtype == "Purchase" || gtype == "P Return" || gtype == "PWDebitNote" || gtype == "Opening") && gExState == true)
                            {
                                ansGridView1.Rows[rowindex].Cells["tax1"].Value = 0;
                                ansGridView1.Rows[rowindex].Cells["tax2"].Value = 0;
                                ansGridView1.Rows[rowindex].Cells["tax3"].Value = dr["PTA3"];
                                ansGridView1.Rows[rowindex].Cells["tax4"].Value = dr["PCAEX"];
                                ansGridView1.Rows[rowindex].Cells["pur_sale_acc"].Value = dr["PAEX"];
                                ansGridView1.Rows[rowindex].Cells["rate1"].Value = 0;
                                ansGridView1.Rows[rowindex].Cells["rate2"].Value = 0;
                                ansGridView1.Rows[rowindex].Cells["rate3"].Value = dr["PTR3"];
                                ansGridView1.Rows[rowindex].Cells["rate4"].Value = dr["PCREX"];
                                ansGridView1.Rows[rowindex].Cells["RCMac_id"].Value = 0;
                            }
                            else if ((gtype == "Purchase" || gtype == "P Return" || gtype == "PWDebitNote" || gtype == "Opening") && gExState == false)
                            {
                                ansGridView1.Rows[rowindex].Cells["tax1"].Value = dr["PTA1"];
                                ansGridView1.Rows[rowindex].Cells["tax2"].Value = dr["PTA2"];
                                ansGridView1.Rows[rowindex].Cells["tax3"].Value = 0;
                                ansGridView1.Rows[rowindex].Cells["tax4"].Value = dr["PCA"];
                                ansGridView1.Rows[rowindex].Cells["pur_sale_acc"].Value = dr["PA"];
                                ansGridView1.Rows[rowindex].Cells["rate1"].Value = dr["PTR1"];
                                ansGridView1.Rows[rowindex].Cells["rate2"].Value = dr["PTR2"];
                                ansGridView1.Rows[rowindex].Cells["rate3"].Value = 0;
                                ansGridView1.Rows[rowindex].Cells["rate4"].Value = dr["PCR"];
                                ansGridView1.Rows[rowindex].Cells["RCMac_id"].Value = 0;
                            }
                            else if ((gtype == "Sale" || gtype == "Return" || gtype == "Sale Order") && gExState == true)
                            {
                                ansGridView1.Rows[rowindex].Cells["tax1"].Value = 0;
                                ansGridView1.Rows[rowindex].Cells["tax2"].Value = 0;
                                ansGridView1.Rows[rowindex].Cells["tax3"].Value = dr["STA3"];
                                ansGridView1.Rows[rowindex].Cells["tax4"].Value = dr["SCAEX"];
                                ansGridView1.Rows[rowindex].Cells["pur_sale_acc"].Value = dr["SAEX"];
                                ansGridView1.Rows[rowindex].Cells["rate1"].Value = 0;
                                ansGridView1.Rows[rowindex].Cells["rate2"].Value = 0;
                                ansGridView1.Rows[rowindex].Cells["rate3"].Value = dr["STR3"];
                                ansGridView1.Rows[rowindex].Cells["rate4"].Value = dr["SCREX"];
                                ansGridView1.Rows[rowindex].Cells["RCMac_id"].Value = 0;
                            }
                            else if ((gtype == "Sale" || gtype == "Return" || gtype == "Sale Order") && gExState == false)
                            {
                                ansGridView1.Rows[rowindex].Cells["tax1"].Value = dr["STA1"];
                                ansGridView1.Rows[rowindex].Cells["tax2"].Value = dr["STA2"];
                                ansGridView1.Rows[rowindex].Cells["tax3"].Value = 0;
                                ansGridView1.Rows[rowindex].Cells["tax4"].Value = dr["SCA"];
                                ansGridView1.Rows[rowindex].Cells["pur_sale_acc"].Value = dr["SA"];
                                ansGridView1.Rows[rowindex].Cells["rate1"].Value = dr["STR1"];
                                ansGridView1.Rows[rowindex].Cells["rate2"].Value = dr["STR2"];
                                ansGridView1.Rows[rowindex].Cells["rate3"].Value = 0;
                                ansGridView1.Rows[rowindex].Cells["rate4"].Value = dr["SCR"];
                                ansGridView1.Rows[rowindex].Cells["RCMac_id"].Value = 0;
                            }
                            else if (gtype == "RCM" && gExState == false && checkBox4.Checked == true)
                            {
                                ansGridView1.Rows[rowindex].Cells["tax1"].Value = dr["RCMITC"];
                                ansGridView1.Rows[rowindex].Cells["tax2"].Value = dr["RCMITC"];
                                ansGridView1.Rows[rowindex].Cells["tax3"].Value = 0;
                                ansGridView1.Rows[rowindex].Cells["tax4"].Value = dr["RCMITC"];
                                ansGridView1.Rows[rowindex].Cells["pur_sale_acc"].Value = dr["PA"];
                                ansGridView1.Rows[rowindex].Cells["rate1"].Value = dr["PTR1"];
                                ansGridView1.Rows[rowindex].Cells["rate2"].Value = dr["PTR2"];
                                ansGridView1.Rows[rowindex].Cells["rate3"].Value = 0;
                                ansGridView1.Rows[rowindex].Cells["rate4"].Value = dr["PCR"];
                                ansGridView1.Rows[rowindex].Cells["RCMac_id"].Value = dr["RCMPay"];
                            }
                            else if ((gtype == "RCM") && gExState == true && checkBox4.Checked == true)
                            {
                                ansGridView1.Rows[rowindex].Cells["tax1"].Value = 0;
                                ansGridView1.Rows[rowindex].Cells["tax2"].Value = 0;
                                ansGridView1.Rows[rowindex].Cells["tax3"].Value = dr["RCMITC"];
                                ansGridView1.Rows[rowindex].Cells["tax4"].Value = dr["RCMITC"];
                                ansGridView1.Rows[rowindex].Cells["pur_sale_acc"].Value = dr["PAEX"];
                                ansGridView1.Rows[rowindex].Cells["rate1"].Value = 0;
                                ansGridView1.Rows[rowindex].Cells["rate2"].Value = 0;
                                ansGridView1.Rows[rowindex].Cells["rate3"].Value = dr["PTR3"];
                                ansGridView1.Rows[rowindex].Cells["rate4"].Value = dr["PCREX"];
                                ansGridView1.Rows[rowindex].Cells["RCMac_id"].Value = dr["RCMPay"];
                            }

                            else if (gtype == "RCM" && gExState == false && checkBox4.Checked == false)
                            {
                                ansGridView1.Rows[rowindex].Cells["tax1"].Value = dr["RCMEli"];
                                ansGridView1.Rows[rowindex].Cells["tax2"].Value = dr["RCMEli"];
                                ansGridView1.Rows[rowindex].Cells["tax3"].Value = 0;
                                ansGridView1.Rows[rowindex].Cells["tax4"].Value = dr["RCMEli"];
                                ansGridView1.Rows[rowindex].Cells["pur_sale_acc"].Value = dr["PA"];
                                ansGridView1.Rows[rowindex].Cells["rate1"].Value = dr["PTR1"];
                                ansGridView1.Rows[rowindex].Cells["rate2"].Value = dr["PTR2"];
                                ansGridView1.Rows[rowindex].Cells["rate3"].Value = 0;
                                ansGridView1.Rows[rowindex].Cells["rate4"].Value = dr["PCR"];
                                ansGridView1.Rows[rowindex].Cells["RCMac_id"].Value = dr["RCMPay"];
                            }
                            else if ((gtype == "RCM") && gExState == true && checkBox4.Checked == false)
                            {
                                ansGridView1.Rows[rowindex].Cells["tax1"].Value = 0;
                                ansGridView1.Rows[rowindex].Cells["tax2"].Value = 0;
                                ansGridView1.Rows[rowindex].Cells["tax3"].Value = dr["RCMEli"];
                                ansGridView1.Rows[rowindex].Cells["tax4"].Value = dr["RCMEli"];
                                ansGridView1.Rows[rowindex].Cells["pur_sale_acc"].Value = dr["PAEX"];
                                ansGridView1.Rows[rowindex].Cells["rate1"].Value = 0;
                                ansGridView1.Rows[rowindex].Cells["rate2"].Value = 0;
                                ansGridView1.Rows[rowindex].Cells["rate3"].Value = dr["PTR3"];
                                ansGridView1.Rows[rowindex].Cells["rate4"].Value = dr["PCREX"];
                                ansGridView1.Rows[rowindex].Cells["RCMac_id"].Value = dr["RCMPay"];
                            }
                        }
                    }
                    return;
                }
            }

            if (ansGridView1.Rows[rowindex].Cells["description"].Value.ToString() != "" && ansGridView1.Rows[rowindex].Cells["unt"].Value.ToString() != "")
            {
                if (Master.DescriptionInfo.Select("description='" + ansGridView1.Rows[rowindex].Cells["orgdesc"].Value + "' and Packing='" + ansGridView1.Rows[rowindex].Cells["unt"].Value.ToString() + "' ").Length == 0)
                {
                    return;
                }
                else
                {
                    dtRateComm = Master.DescriptionInfo.Select("description='" + ansGridView1.Rows[rowindex].Cells["orgdesc"].Value + "' and Packing='" + ansGridView1.Rows[rowindex].Cells["unt"].Value.ToString() + "' ", "").CopyToDataTable();
                }

                if (dtRateComm.Rows.Count > 0)
                {
                    if (ansGridView1.Rows[rowindex].Cells["rate_am"].Value == null)
                    {
                        ansGridView1.Rows[rowindex].Cells["rate_am"].Value = 0;
                    }
                    if (ChangeRate == true)
                    {
                        Database.GetSqlData("SELECT PARTYRATE.Rate FROM (PARTYRATE INNER JOIN DESCRIPTION ON PARTYRATE.Des_id = DESCRIPTION.Des_id) INNER JOIN ACCOUNT ON PARTYRATE.Ac_id = ACCOUNT.Ac_id WHERE (((PARTYRATE.Des_id)='" + ansGridView1.Rows[rowindex].Cells["Des_ac_id"].Value.ToString() + "') AND ((ACCOUNT.Name)='" + textBox14.Text + "'))", dtRate);

                        if (dtRate.Rows.Count != 0)
                        {
                            ansGridView1.Rows[rowindex].Cells["rate_am"].Value = funs.DecimalPoint(dtRate.Rows[0]["Rate"]);
                        }
                        else
                        {
                            ansGridView1.Rows[rowindex].Cells["rate_am"].Value = funs.DecimalPoint(dtRateComm.Rows[0][Ratesapp]);

                            if (double.Parse(ansGridView1.Rows[rowindex].Cells["rate_am"].Value.ToString()) <= 0)
                            {
                                ansGridView1.CurrentRow.Cells["rate_am"].Style.BackColor = Color.Red;
                            }
                        }
                    }
                    permission = funs.GetPermissionKey("Transactions");

                    UsersFeature ob3 = permission.Where(w => w.FeatureName == "Action on ChangeRate").FirstOrDefault();

                    if (ob3 != null && ob3.SelectedValue == "Do Not Allow")
                    {
                        if (MasterRate(rowindex) == 0)
                        {
                            ansGridView1.Rows[rowindex].Cells["rate_am"].ReadOnly = false;
                            ansGridView1.Rows[rowindex].Cells["Amount"].ReadOnly = false;
                        }
                        else
                        {
                            ansGridView1.Rows[rowindex].Cells["rate_am"].ReadOnly = true;
                            ansGridView1.Rows[rowindex].Cells["Amount"].ReadOnly = true;
                        }
                    }

                    if (Database.BranchGodown_id == "")
                    {
                        string godownid = "";
                        godownid = Database.GetScalarText("Select Godown_id from Description where Des_id='" + dtRateComm.Rows[0]["Des_id"].ToString() + "' ");
                        if (godownid == "0" || godownid == "")
                        {

                            ansGridView1.Rows[rowindex].Cells["godown_id"].Value = "<MAIN>";
                        }
                        else
                        {
                            ansGridView1.Rows[rowindex].Cells["godown_id"].Value = funs.Select_ac_nm(godownid);
                        }
                    }
                    else
                    {
                        ansGridView1.Rows[rowindex].Cells["godown_id"].Value = funs.Select_ac_nm(Database.BranchGodown_id);
                    }
                    ansGridView1.Rows[rowindex].Cells["Commission_per"].Value = dtRateComm.Rows[0]["Commission%"];
                    ansGridView1.Rows[rowindex].Cells["CommissionFix"].Value = dtRateComm.Rows[0]["Commission@"];
                    ansGridView1.Rows[rowindex].Cells["Category_Id"].Value = dtRateComm.Rows[0]["Tax_Cat_id"];
                    ansGridView1.Rows[rowindex].Cells["Category"].Value = funs.Select_tax_cat_nm(dtRateComm.Rows[0]["Tax_Cat_id"].ToString());
                    ansGridView1.Rows[rowindex].Cells["QDType"].Value = Feature.Available("Type of Discount1");
                    ansGridView1.Rows[rowindex].Cells["CDType"].Value = Feature.Available("Type of Discount2");
                    ansGridView1.Rows[rowindex].Cells["FDType"].Value = Feature.Available("Type of Discount3");
                    ansGridView1.Rows[rowindex].Cells["Des_ac_id"].Value = dtRateComm.Rows[0]["Des_id"];
                    ansGridView1.Rows[rowindex].Cells["Cost"].Value = dtRateComm.Rows[0]["Purchase_rate"];
                    ansGridView1.Rows[rowindex].Cells["pvalue"].Value = dtRateComm.Rows[0]["pvalue"];
                    ansGridView1.Rows[rowindex].Cells["rate_unit"].Value = dtRateComm.Rows[0]["rate_unit"];
                    desc_id = dtRateComm.Rows[0]["Des_id"].ToString();
                    if (ansGridView1.Rows[rowindex].Cells["qd"].Value == null)
                    {
                        ansGridView1.Rows[rowindex].Cells["qd"].Value = 0;
                    }
                    if (ansGridView1.Rows[rowindex].Cells["cd"].Value == null)
                    {
                        ansGridView1.Rows[rowindex].Cells["cd"].Value = 0;
                    }
                    if (ansGridView1.Rows[rowindex].Cells["flatdis"].Value == null)
                    {
                        ansGridView1.Rows[rowindex].Cells["flatdis"].Value = 0;
                    }
                    if (ansGridView1.Rows[rowindex].Cells["GridDis"].Value == null)
                    {
                        ansGridView1.Rows[rowindex].Cells["GridDis"].Value = 0;
                    }
                    DataTable dtcompany = new DataTable();
                    Database.GetSqlData("SELECT     Company_id, Item_id FROM  Description WHERE DESCRIPTION.Des_id='" + ansGridView1.Rows[rowindex].Cells["des_ac_id"].Value.ToString() + "'", dtcompany);
                    if (gtype == "Sale" || gtype == "Return")
                    {
                        setrebate(funs.Select_ac_id(textBox14.Text), dtcompany.Rows[0]["Company_id"].ToString(), dtcompany.Rows[0]["Item_id"].ToString());

                    }

                    ansGridView1.Rows[rowindex].Cells["qd"].Value = dis1;
                    ansGridView1.Rows[rowindex].Cells["cd"].Value = dis2;
                    ansGridView1.Rows[rowindex].Cells["flatdis"].Value = dis3;
                    DataTable dtdat = new DataTable();
                    Database.GetSqlData("Select * from DisAfterTax", dtdat);
                    if (dtdat.Rows.Count != 0)
                    {
                        ansGridView1.Rows[rowindex].Cells["dattype"].Value = dtdat.Rows[0]["type"].ToString();
                        ansGridView1.Rows[rowindex].Cells["datac_id"].Value = dtdat.Rows[0]["ac_id"].ToString();
                    }

                    if (Master.TaxCategory.Select("Category_id='" + ansGridView1.Rows[rowindex].Cells["Category_id"].Value.ToString() + "' ").Length > 0)
                    {
                        DataRow dr = Master.TaxCategory.Select("Category_id='" + ansGridView1.Rows[rowindex].Cells["Category_id"].Value.ToString() + "' ").FirstOrDefault();

                        ansGridView1.Rows[rowindex].Cells["Category_id"].Value = funs.Select_destax_id(ansGridView1.Rows[rowindex].Cells["des_ac_id"].Value.ToString());
                        if (gtype == "Opening")
                        {
                            ansGridView1.Rows[rowindex].Cells["tax1"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["tax2"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["tax3"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["tax4"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["pur_sale_acc"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["rate1"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["rate2"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["rate3"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["rate4"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["RCMac_id"].Value = 0;
                        }

                        else if ((gtype == "Purchase" || gtype == "P Return" || gtype == "PWDebitNote") && gExState == true)
                        {
                            ansGridView1.Rows[rowindex].Cells["tax1"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["tax2"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["tax3"].Value = dr["PTA3"];
                            ansGridView1.Rows[rowindex].Cells["tax4"].Value = dr["PCAEX"];
                            ansGridView1.Rows[rowindex].Cells["pur_sale_acc"].Value = dr["PAEX"];
                            ansGridView1.Rows[rowindex].Cells["rate1"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["rate2"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["rate3"].Value = dr["PTR3"];
                            ansGridView1.Rows[rowindex].Cells["rate4"].Value = dr["PCREX"];
                            ansGridView1.Rows[rowindex].Cells["RCMac_id"].Value = 0;
                        }
                        else if ((gtype == "Purchase" || gtype == "P Return" || gtype == "PWDebitNote") && gExState == false)
                        {
                            ansGridView1.Rows[rowindex].Cells["tax1"].Value = dr["PTA1"];
                            ansGridView1.Rows[rowindex].Cells["tax2"].Value = dr["PTA2"];
                            ansGridView1.Rows[rowindex].Cells["tax3"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["tax4"].Value = dr["PCA"];
                            ansGridView1.Rows[rowindex].Cells["pur_sale_acc"].Value = dr["PA"];
                            ansGridView1.Rows[rowindex].Cells["rate1"].Value = dr["PTR1"];
                            ansGridView1.Rows[rowindex].Cells["rate2"].Value = dr["PTR2"];
                            ansGridView1.Rows[rowindex].Cells["rate3"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["rate4"].Value = dr["PCR"];
                            ansGridView1.Rows[rowindex].Cells["RCMac_id"].Value = 0;
                        }
                        else if ((gtype == "Sale" || gtype == "Return" || gtype == "Sale Order") && gExState == true)
                        {
                            ansGridView1.Rows[rowindex].Cells["tax1"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["tax2"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["tax3"].Value = dr["STA3"];
                            ansGridView1.Rows[rowindex].Cells["tax4"].Value = dr["SCAEX"];
                            ansGridView1.Rows[rowindex].Cells["pur_sale_acc"].Value = dr["SAEX"];
                            ansGridView1.Rows[rowindex].Cells["rate1"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["rate2"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["rate3"].Value = dr["STR3"];
                            ansGridView1.Rows[rowindex].Cells["rate4"].Value = dr["SCREX"];
                            ansGridView1.Rows[rowindex].Cells["RCMac_id"].Value = 0;
                        }
                        else if ((gtype == "Sale" || gtype == "Return" || gtype == "Sale Order") && gExState == false)
                        {
                            ansGridView1.Rows[rowindex].Cells["tax1"].Value = dr["STA1"];
                            ansGridView1.Rows[rowindex].Cells["tax2"].Value = dr["STA2"];
                            ansGridView1.Rows[rowindex].Cells["tax3"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["tax4"].Value = dr["SCA"];
                            ansGridView1.Rows[rowindex].Cells["pur_sale_acc"].Value = dr["SA"];
                            ansGridView1.Rows[rowindex].Cells["rate1"].Value = dr["STR1"];
                            ansGridView1.Rows[rowindex].Cells["rate2"].Value = dr["STR2"];
                            ansGridView1.Rows[rowindex].Cells["rate3"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["rate4"].Value = dr["SCR"];
                            ansGridView1.Rows[rowindex].Cells["RCMac_id"].Value = 0;
                        }
                        else if (gtype == "RCM" && gExState == false && checkBox4.Checked == true)
                        {
                            ansGridView1.Rows[rowindex].Cells["tax1"].Value = dr["RCMITC"];
                            ansGridView1.Rows[rowindex].Cells["tax2"].Value = dr["RCMITC"];
                            ansGridView1.Rows[rowindex].Cells["tax3"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["tax4"].Value = dr["RCMITC"];
                            ansGridView1.Rows[rowindex].Cells["pur_sale_acc"].Value = dr["PA"];
                            ansGridView1.Rows[rowindex].Cells["rate1"].Value = dr["PTR1"];
                            ansGridView1.Rows[rowindex].Cells["rate2"].Value = dr["PTR2"];
                            ansGridView1.Rows[rowindex].Cells["rate3"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["rate4"].Value = dr["PCR"];
                            ansGridView1.Rows[rowindex].Cells["RCMac_id"].Value = dr["RCMPay"];
                        }
                        else if ((gtype == "RCM") && gExState == true && checkBox4.Checked == true)
                        {
                            ansGridView1.Rows[rowindex].Cells["tax1"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["tax2"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["tax3"].Value = dr["RCMITC"];
                            ansGridView1.Rows[rowindex].Cells["tax4"].Value = dr["RCMITC"];
                            ansGridView1.Rows[rowindex].Cells["pur_sale_acc"].Value = dr["PAEX"];
                            ansGridView1.Rows[rowindex].Cells["rate1"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["rate2"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["rate3"].Value = dr["PTR3"];
                            ansGridView1.Rows[rowindex].Cells["rate4"].Value = dr["PCREX"];
                            ansGridView1.Rows[rowindex].Cells["RCMac_id"].Value = dr["RCMPay"];
                        }
                        else if (gtype == "RCM" && gExState == false && checkBox4.Checked == false)
                        {
                            ansGridView1.Rows[rowindex].Cells["tax1"].Value = dr["RCMEli"];
                            ansGridView1.Rows[rowindex].Cells["tax2"].Value = dr["RCMEli"];
                            ansGridView1.Rows[rowindex].Cells["tax3"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["tax4"].Value = dr["RCMEli"];
                            ansGridView1.Rows[rowindex].Cells["pur_sale_acc"].Value = dr["PA"];
                            ansGridView1.Rows[rowindex].Cells["rate1"].Value = dr["PTR1"];
                            ansGridView1.Rows[rowindex].Cells["rate2"].Value = dr["PTR2"];
                            ansGridView1.Rows[rowindex].Cells["rate3"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["rate4"].Value = dr["PCR"];
                            ansGridView1.Rows[rowindex].Cells["RCMac_id"].Value = dr["RCMPay"];
                        }
                        else if ((gtype == "RCM") && gExState == true && checkBox4.Checked == false)
                        {
                            ansGridView1.Rows[rowindex].Cells["tax1"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["tax2"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["tax3"].Value = dr["RCMEli"];
                            ansGridView1.Rows[rowindex].Cells["tax4"].Value = dr["RCMEli"];
                            ansGridView1.Rows[rowindex].Cells["pur_sale_acc"].Value = dr["PAEX"];
                            ansGridView1.Rows[rowindex].Cells["rate1"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["rate2"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["rate3"].Value = dr["PTR3"];
                            ansGridView1.Rows[rowindex].Cells["rate4"].Value = dr["PCREX"];
                            ansGridView1.Rows[rowindex].Cells["RCMac_id"].Value = dr["RCMPay"];
                        }

                        if (funs.Select_ac_regstatus(shiptoacc_id) == "Composition Dealer" && (gtype == "Purchase" || gtype == "P Return"))
                        {
                            ansGridView1.Rows[rowindex].Cells["tax1"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["tax2"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["tax3"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["tax4"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["rate1"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["rate2"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["rate3"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["rate4"].Value = 0;
                        }
                    }
                }

                ItemCalc(rowindex);
                if (desc_id != "")
                {
                    SetColor(DisplayStock(desc_id));
                }
            }
        }

        private void ItemSelectedPaste(int rowindex)
        {
            DataTable dtRateComm = new DataTable();
            DataTable dtRate = new DataTable();

            if (ansGridView1.Rows[rowindex].Cells["description"].Value.ToString() != "" && ansGridView1.Rows[rowindex].Cells["unt"].Value.ToString() != "")
            {
                if (Master.DescriptionInfo.Select("description='" + ansGridView1.Rows[rowindex].Cells["orgdesc"].Value + "' and Packing='" + ansGridView1.Rows[rowindex].Cells["unt"].Value.ToString() + "' ").Length == 0)
                {
                    return;
                }
                else
                {
                    dtRateComm = Master.DescriptionInfo.Select("description='" + ansGridView1.Rows[rowindex].Cells["orgdesc"].Value + "' and Packing='" + ansGridView1.Rows[rowindex].Cells["unt"].Value.ToString() + "' ", "").CopyToDataTable();
                }

                if (dtRateComm.Rows.Count > 0)
                {
                    ansGridView1.Rows[rowindex].Cells["Commission_per"].Value = dtRateComm.Rows[0]["Commission%"];
                    ansGridView1.Rows[rowindex].Cells["CommissionFix"].Value = dtRateComm.Rows[0]["Commission@"];
                    ansGridView1.Rows[rowindex].Cells["Category_Id"].Value = dtRateComm.Rows[0]["Tax_Cat_id"];
                    ansGridView1.Rows[rowindex].Cells["Category"].Value = funs.Select_tax_cat_nm(dtRateComm.Rows[0]["Tax_Cat_id"].ToString());
                    ansGridView1.Rows[rowindex].Cells["QDType"].Value = Feature.Available("Type of Discount1");
                    ansGridView1.Rows[rowindex].Cells["CDType"].Value = Feature.Available("Type of Discount2");
                    ansGridView1.Rows[rowindex].Cells["FDType"].Value = Feature.Available("Type of Discount3");
                    ansGridView1.Rows[rowindex].Cells["Des_ac_id"].Value = dtRateComm.Rows[0]["Des_id"];
                    ansGridView1.Rows[rowindex].Cells["Cost"].Value = dtRateComm.Rows[0]["Purchase_rate"];
                    ansGridView1.Rows[rowindex].Cells["pvalue"].Value = dtRateComm.Rows[0]["pvalue"];
                    ansGridView1.Rows[rowindex].Cells["rate_unit"].Value = dtRateComm.Rows[0]["rate_unit"];
                    desc_id = dtRateComm.Rows[0]["Des_id"].ToString();
                    ansGridView1.Rows[rowindex].Cells["qd"].Value = 0;
                    ansGridView1.Rows[rowindex].Cells["cd"].Value = 0;
                    ansGridView1.Rows[rowindex].Cells["flatdis"].Value = 0;
                    ansGridView1.Rows[rowindex].Cells["GridDis"].Value = 0;
                    if (Master.TaxCategory.Select("Category_id='" + ansGridView1.Rows[rowindex].Cells["Category_id"].Value.ToString() + "' ").Length > 0)
                    {
                        DataRow dr = Master.TaxCategory.Select("Category_id='" + ansGridView1.Rows[rowindex].Cells["Category_id"].Value.ToString() + "' ").FirstOrDefault();
                        if ((gtype == "Purchase" || gtype == "P Return" || gtype == "PWDebitNote") && gExState == true)
                        {
                            ansGridView1.Rows[rowindex].Cells["tax1"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["tax2"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["tax3"].Value = dr["PTA3"];
                            ansGridView1.Rows[rowindex].Cells["tax4"].Value = dr["PCAEX"];
                            ansGridView1.Rows[rowindex].Cells["pur_sale_acc"].Value = dr["PAEX"];
                            ansGridView1.Rows[rowindex].Cells["rate1"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["rate2"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["rate3"].Value = dr["PTR3"];
                            ansGridView1.Rows[rowindex].Cells["rate4"].Value = dr["PCREX"];
                        }
                        else if ((gtype == "Purchase" || gtype == "P Return" || gtype == "PWDebitNote") && gExState == false)
                        {
                            ansGridView1.Rows[rowindex].Cells["tax1"].Value = dr["PTA1"];
                            ansGridView1.Rows[rowindex].Cells["tax2"].Value = dr["PTA2"];
                            ansGridView1.Rows[rowindex].Cells["tax3"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["tax4"].Value = dr["PCA"];
                            ansGridView1.Rows[rowindex].Cells["pur_sale_acc"].Value = dr["PA"];
                            ansGridView1.Rows[rowindex].Cells["rate1"].Value = dr["PTR1"];
                            ansGridView1.Rows[rowindex].Cells["rate2"].Value = dr["PTR2"];
                            ansGridView1.Rows[rowindex].Cells["rate3"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["rate4"].Value = dr["PCR"];
                        }
                        else if ((gtype == "Sale" || gtype == "Return") && gExState == true)
                        {
                            ansGridView1.Rows[rowindex].Cells["tax1"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["tax2"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["tax3"].Value = dr["STA3"];
                            ansGridView1.Rows[rowindex].Cells["tax4"].Value = dr["SCAEX"];
                            ansGridView1.Rows[rowindex].Cells["pur_sale_acc"].Value = dr["SAEX"];
                            ansGridView1.Rows[rowindex].Cells["rate1"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["rate2"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["rate3"].Value = dr["STR3"];
                            ansGridView1.Rows[rowindex].Cells["rate4"].Value = dr["SCREX"];
                        }
                        else if ((gtype == "Sale" || gtype == "Return") && gExState == false)
                        {
                            ansGridView1.Rows[rowindex].Cells["tax1"].Value = dr["STA1"];
                            ansGridView1.Rows[rowindex].Cells["tax2"].Value = dr["STA2"];
                            ansGridView1.Rows[rowindex].Cells["tax3"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["tax4"].Value = dr["SCA"];
                            ansGridView1.Rows[rowindex].Cells["pur_sale_acc"].Value = dr["SA"];
                            ansGridView1.Rows[rowindex].Cells["rate1"].Value = dr["STR1"];
                            ansGridView1.Rows[rowindex].Cells["rate2"].Value = dr["STR2"];
                            ansGridView1.Rows[rowindex].Cells["rate3"].Value = 0;
                            ansGridView1.Rows[rowindex].Cells["rate4"].Value = dr["SCR"];
                        }
                    }
                }
                if (desc_id != "")
                {
                    SetColor(DisplayStock(desc_id));
                }
            }
        }

        private void TaxCalc()
        {
            ctaxamt1 = 0;
            ctaxamt2 = 0;
            ctaxamt3 = 0;
            ctaxamt4 = 0;
            totTaxabe = 0;
            for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
            {
                if (gresave == true)
                {
                    ansGridView1.Rows[i].Cells["Category_id"].Value = funs.Select_destax_id(ansGridView1.Rows[i].Cells["des_ac_id"].Value.ToString());
                    if (Master.TaxCategory.Select("Category_id='" + ansGridView1.Rows[i].Cells["Category_id"].Value.ToString() + "' ").Length > 0)
                    {
                        DataRow dr = Master.TaxCategory.Select("Category_id='" + ansGridView1.Rows[i].Cells["Category_id"].Value.ToString() + "' ").FirstOrDefault();
                        if ((gtype == "Purchase" || gtype == "P Return" || gtype == "PWDebitNote" || gtype == "Opening") && gExState == true)
                        {
                            ansGridView1.Rows[i].Cells["tax1"].Value = 0;
                            ansGridView1.Rows[i].Cells["tax2"].Value = 0;
                            ansGridView1.Rows[i].Cells["tax3"].Value = dr["PTA3"];
                            ansGridView1.Rows[i].Cells["tax4"].Value = dr["PCAEX"];
                            ansGridView1.Rows[i].Cells["pur_sale_acc"].Value = dr["PAEX"];
                            ansGridView1.Rows[i].Cells["rate1"].Value = 0;
                            ansGridView1.Rows[i].Cells["rate2"].Value = 0;
                            ansGridView1.Rows[i].Cells["rate3"].Value = dr["PTR3"];
                            ansGridView1.Rows[i].Cells["rate4"].Value = dr["PCREX"];
                            ansGridView1.Rows[i].Cells["RCMac_id"].Value = 0;
                        }
                        else if ((gtype == "Purchase" || gtype == "P Return" || gtype == "PWDebitNote" || gtype == "Opening") && gExState == false)
                        {
                            ansGridView1.Rows[i].Cells["tax1"].Value = dr["PTA1"];
                            ansGridView1.Rows[i].Cells["tax2"].Value = dr["PTA2"];
                            ansGridView1.Rows[i].Cells["tax3"].Value = 0;
                            ansGridView1.Rows[i].Cells["tax4"].Value = dr["PCA"];
                            ansGridView1.Rows[i].Cells["pur_sale_acc"].Value = dr["PA"];
                            ansGridView1.Rows[i].Cells["rate1"].Value = dr["PTR1"];
                            ansGridView1.Rows[i].Cells["rate2"].Value = dr["PTR2"];
                            ansGridView1.Rows[i].Cells["rate3"].Value = 0;
                            ansGridView1.Rows[i].Cells["rate4"].Value = dr["PCR"];
                            ansGridView1.Rows[i].Cells["RCMac_id"].Value = 0;
                        }
                        else if ((gtype == "Sale" || gtype == "Return" || gtype == "Sale Order") && gExState == true)
                        {
                            ansGridView1.Rows[i].Cells["tax1"].Value = 0;
                            ansGridView1.Rows[i].Cells["tax2"].Value = 0;
                            ansGridView1.Rows[i].Cells["tax3"].Value = dr["STA3"];
                            ansGridView1.Rows[i].Cells["tax4"].Value = dr["SCAEX"];
                            ansGridView1.Rows[i].Cells["pur_sale_acc"].Value = dr["SAEX"];
                            ansGridView1.Rows[i].Cells["rate1"].Value = 0;
                            ansGridView1.Rows[i].Cells["rate2"].Value = 0;
                            ansGridView1.Rows[i].Cells["rate3"].Value = dr["STR3"];
                            ansGridView1.Rows[i].Cells["rate4"].Value = dr["SCREX"];
                            ansGridView1.Rows[i].Cells["RCMac_id"].Value = 0;
                        }
                        else if ((gtype == "Sale" || gtype == "Return" || gtype == "Sale Order") && gExState == false)
                        {
                            ansGridView1.Rows[i].Cells["tax1"].Value = dr["STA1"];
                            ansGridView1.Rows[i].Cells["tax2"].Value = dr["STA2"];
                            ansGridView1.Rows[i].Cells["tax3"].Value = 0;
                            ansGridView1.Rows[i].Cells["tax4"].Value = dr["SCA"];
                            ansGridView1.Rows[i].Cells["pur_sale_acc"].Value = dr["SA"];
                            ansGridView1.Rows[i].Cells["rate1"].Value = dr["STR1"];
                            ansGridView1.Rows[i].Cells["rate2"].Value = dr["STR2"];
                            ansGridView1.Rows[i].Cells["rate3"].Value = 0;
                            ansGridView1.Rows[i].Cells["rate4"].Value = dr["SCR"];
                            ansGridView1.Rows[i].Cells["RCMac_id"].Value = 0;
                        }
                        else if (gtype == "RCM" && gExState == false && checkBox4.Checked == true)
                        {
                            ansGridView1.Rows[i].Cells["tax1"].Value = dr["RCMITC"];
                            ansGridView1.Rows[i].Cells["tax2"].Value = dr["RCMITC"];
                            ansGridView1.Rows[i].Cells["tax3"].Value = 0;
                            ansGridView1.Rows[i].Cells["tax4"].Value = dr["RCMITC"];
                            ansGridView1.Rows[i].Cells["pur_sale_acc"].Value = dr["PA"];
                            ansGridView1.Rows[i].Cells["rate1"].Value = dr["PTR1"];
                            ansGridView1.Rows[i].Cells["rate2"].Value = dr["PTR2"];
                            ansGridView1.Rows[i].Cells["rate3"].Value = 0;
                            ansGridView1.Rows[i].Cells["rate4"].Value = dr["PCR"];
                            ansGridView1.Rows[i].Cells["RCMac_id"].Value = dr["RCMPay"];
                        }
                        else if ((gtype == "RCM") && gExState == true && checkBox4.Checked == true)
                        {

                            ansGridView1.Rows[i].Cells["tax1"].Value = 0;
                            ansGridView1.Rows[i].Cells["tax2"].Value = 0;
                            ansGridView1.Rows[i].Cells["tax3"].Value = dr["RCMITC"];
                            ansGridView1.Rows[i].Cells["tax4"].Value = dr["RCMITC"];
                            ansGridView1.Rows[i].Cells["pur_sale_acc"].Value = dr["PAEX"];
                            ansGridView1.Rows[i].Cells["rate1"].Value = 0;
                            ansGridView1.Rows[i].Cells["rate2"].Value = 0;
                            ansGridView1.Rows[i].Cells["rate3"].Value = dr["PTR3"];
                            ansGridView1.Rows[i].Cells["rate4"].Value = dr["PCREX"];
                            ansGridView1.Rows[i].Cells["RCMac_id"].Value = dr["RCMPay"];

                        }
                        else if (gtype == "RCM" && gExState == false && checkBox4.Checked == false)
                        {
                            ansGridView1.Rows[i].Cells["tax1"].Value = dr["RCMEli"];
                            ansGridView1.Rows[i].Cells["tax2"].Value = dr["RCMEli"];
                            ansGridView1.Rows[i].Cells["tax3"].Value = 0;
                            ansGridView1.Rows[i].Cells["tax4"].Value = dr["RCMEli"];
                            ansGridView1.Rows[i].Cells["pur_sale_acc"].Value = dr["PA"];
                            ansGridView1.Rows[i].Cells["rate1"].Value = dr["PTR1"];
                            ansGridView1.Rows[i].Cells["rate2"].Value = dr["PTR2"];
                            ansGridView1.Rows[i].Cells["rate3"].Value = 0;
                            ansGridView1.Rows[i].Cells["rate4"].Value = dr["PCR"];
                            ansGridView1.Rows[i].Cells["RCMac_id"].Value = dr["RCMPay"];
                        }
                        else if ((gtype == "RCM") && gExState == true && checkBox4.Checked == false)
                        {
                            ansGridView1.Rows[i].Cells["tax1"].Value = 0;
                            ansGridView1.Rows[i].Cells["tax2"].Value = 0;
                            ansGridView1.Rows[i].Cells["tax3"].Value = dr["RCMEli"];
                            ansGridView1.Rows[i].Cells["tax4"].Value = dr["RCMEli"];
                            ansGridView1.Rows[i].Cells["pur_sale_acc"].Value = dr["PAEX"];
                            ansGridView1.Rows[i].Cells["rate1"].Value = 0;
                            ansGridView1.Rows[i].Cells["rate2"].Value = 0;
                            ansGridView1.Rows[i].Cells["rate3"].Value = dr["PTR3"];
                            ansGridView1.Rows[i].Cells["rate4"].Value = dr["PCREX"];
                            ansGridView1.Rows[i].Cells["RCMac_id"].Value = dr["RCMPay"];
                        }
                    }
                }
                if (ansGridView1.Rows[i].Cells["Category_id"].Value == null) ansGridView1.Rows[i].Cells["Category_id"].Value = 0;
                if (Master.TaxCategory.Select("Category_id='" + ansGridView1.Rows[i].Cells["Category_id"].Value.ToString() + "' ").Length > 0)
                {
                    DataRow dr = Master.TaxCategory.Select("Category_id='" + ansGridView1.Rows[i].Cells["Category_id"].Value.ToString() + "' ").FirstOrDefault();
                    ansGridView1.Rows[i].Cells["rate1"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[i].Cells["rate1"].Value.ToString()));
                    ansGridView1.Rows[i].Cells["rate2"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[i].Cells["rate2"].Value.ToString()));
                    ansGridView1.Rows[i].Cells["rate3"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[i].Cells["rate3"].Value.ToString()));
                    ansGridView1.Rows[i].Cells["rate4"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[i].Cells["rate4"].Value.ToString()));
                    ansGridView1.Rows[i].Cells["TotTaxPer"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[i].Cells["rate1"].Value.ToString()) + double.Parse(ansGridView1.Rows[i].Cells["rate2"].Value.ToString()) + double.Parse(ansGridView1.Rows[i].Cells["rate3"].Value.ToString()) + double.Parse(ansGridView1.Rows[i].Cells["rate4"].Value.ToString()), 2);
                    double taxableamt = 0;
                    if (gExcludingTax == true && TaxChanged == false && gtype != "RCM")
                    {
                        taxableamt = double.Parse(ansGridView1.Rows[i].Cells["Amount4"].Value.ToString());
                        ansGridView1.Rows[i].Cells["Taxabelamount"].Value = taxableamt;
                        ansGridView1.Rows[i].Cells["taxamt1"].Value = funs.DecimalPoint(taxableamt * double.Parse(ansGridView1.Rows[i].Cells["rate1"].Value.ToString()) / 100, 2);
                        ansGridView1.Rows[i].Cells["taxamt2"].Value = funs.DecimalPoint(taxableamt * double.Parse(ansGridView1.Rows[i].Cells["rate2"].Value.ToString()) / 100, 2);
                        ansGridView1.Rows[i].Cells["taxamt3"].Value = funs.DecimalPoint(taxableamt * double.Parse(ansGridView1.Rows[i].Cells["rate3"].Value.ToString()) / 100, 2);
                        ansGridView1.Rows[i].Cells["taxamt4"].Value = funs.DecimalPoint(taxableamt * double.Parse(ansGridView1.Rows[i].Cells["rate4"].Value.ToString()) / 100, 2);
                        ansGridView1.Rows[i].Cells["TotTaxAmount"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[i].Cells["taxamt1"].Value.ToString()) + double.Parse(ansGridView1.Rows[i].Cells["taxamt2"].Value.ToString()) + double.Parse(ansGridView1.Rows[i].Cells["taxamt3"].Value.ToString()) + double.Parse(ansGridView1.Rows[i].Cells["taxamt4"].Value.ToString()), 2);
                    }
                    else if (gExcludingTax == false && TaxChanged == false && gtype != "RCM")
                    {
                        taxableamt = double.Parse(funs.DecimalPoint(double.Parse(ansGridView1.Rows[i].Cells["Amount4"].Value.ToString()) / (100 + double.Parse(ansGridView1.Rows[i].Cells["TotTaxPer"].Value.ToString())) * 100));
                        ansGridView1.Rows[i].Cells["Taxabelamount"].Value = taxableamt;
                        ansGridView1.Rows[i].Cells["taxamt1"].Value = funs.DecimalPoint(taxableamt * double.Parse(ansGridView1.Rows[i].Cells["rate1"].Value.ToString()) / 100, 2);
                        ansGridView1.Rows[i].Cells["taxamt2"].Value = funs.DecimalPoint(taxableamt * double.Parse(ansGridView1.Rows[i].Cells["rate2"].Value.ToString()) / 100, 2);
                        ansGridView1.Rows[i].Cells["taxamt3"].Value = funs.DecimalPoint(taxableamt * double.Parse(ansGridView1.Rows[i].Cells["rate3"].Value.ToString()) / 100, 2);
                        ansGridView1.Rows[i].Cells["taxamt4"].Value = funs.DecimalPoint(taxableamt * double.Parse(ansGridView1.Rows[i].Cells["rate4"].Value.ToString()) / 100, 2);
                        ansGridView1.Rows[i].Cells["TotTaxAmount"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[i].Cells["taxamt1"].Value.ToString()) + double.Parse(ansGridView1.Rows[i].Cells["taxamt2"].Value.ToString()) + double.Parse(ansGridView1.Rows[i].Cells["taxamt3"].Value.ToString()) + double.Parse(ansGridView1.Rows[i].Cells["taxamt4"].Value.ToString()), 2);
                        if (taxableamt + double.Parse(funs.DecimalPoint(ansGridView1.Rows[i].Cells["TotTaxAmount"].Value)) != double.Parse(ansGridView1.Rows[i].Cells["Amount4"].Value.ToString()))
                        {
                            double diff = double.Parse(funs.DecimalPoint(taxableamt + double.Parse(funs.DecimalPoint(ansGridView1.Rows[i].Cells["TotTaxAmount"].Value)) - double.Parse(ansGridView1.Rows[i].Cells["Amount4"].Value.ToString())));
                            ansGridView1.Rows[i].Cells["Taxabelamount"].Value = taxableamt - diff;
                        }
                    }
                    else if (gtype == "RCM" && checkBox2.Checked == true)
                    {
                        taxableamt = double.Parse(ansGridView1.Rows[i].Cells["Amount4"].Value.ToString());
                        ansGridView1.Rows[i].Cells["Taxabelamount"].Value = taxableamt;
                        ansGridView1.Rows[i].Cells["taxamt1"].Value = funs.DecimalPoint(taxableamt * double.Parse(ansGridView1.Rows[i].Cells["rate1"].Value.ToString()) / 100, 2);
                        ansGridView1.Rows[i].Cells["taxamt2"].Value = funs.DecimalPoint(taxableamt * double.Parse(ansGridView1.Rows[i].Cells["rate2"].Value.ToString()) / 100, 2);
                        ansGridView1.Rows[i].Cells["taxamt3"].Value = funs.DecimalPoint(taxableamt * double.Parse(ansGridView1.Rows[i].Cells["rate3"].Value.ToString()) / 100, 2);
                        ansGridView1.Rows[i].Cells["taxamt4"].Value = funs.DecimalPoint(taxableamt * double.Parse(ansGridView1.Rows[i].Cells["rate4"].Value.ToString()) / 100, 2);
                        ansGridView1.Rows[i].Cells["TotTaxAmount"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[i].Cells["taxamt1"].Value.ToString()) + double.Parse(ansGridView1.Rows[i].Cells["taxamt2"].Value.ToString()) + double.Parse(ansGridView1.Rows[i].Cells["taxamt3"].Value.ToString()) + double.Parse(ansGridView1.Rows[i].Cells["taxamt4"].Value.ToString()), 2);
                    }
                    else if (gtype == "RCM" && checkBox2.Checked == false)
                    {
                        taxableamt = double.Parse(ansGridView1.Rows[i].Cells["Amount4"].Value.ToString());
                        ansGridView1.Rows[i].Cells["Taxabelamount"].Value = taxableamt;
                        ansGridView1.Rows[i].Cells["taxamt1"].Value = 0;
                        ansGridView1.Rows[i].Cells["taxamt2"].Value = 0;
                        ansGridView1.Rows[i].Cells["taxamt3"].Value = 0;
                        ansGridView1.Rows[i].Cells["taxamt4"].Value = 0;
                        ansGridView1.Rows[i].Cells["TotTaxAmount"].Value = 0;
                    }
                    if (Feature.Available("Roundoff On All Taxes") == "Yes")
                    {
                        ansGridView1.Rows[i].Cells["taxamt1"].Value = funs.DecimalPoint(funs.Roundoff(ansGridView1.Rows[i].Cells["taxamt1"].Value.ToString()), 2);
                        ansGridView1.Rows[i].Cells["taxamt2"].Value = funs.DecimalPoint(funs.Roundoff(ansGridView1.Rows[i].Cells["taxamt2"].Value.ToString()), 2);
                        ansGridView1.Rows[i].Cells["taxamt3"].Value = funs.DecimalPoint(funs.Roundoff(ansGridView1.Rows[i].Cells["taxamt3"].Value.ToString()), 2);
                        ansGridView1.Rows[i].Cells["taxamt4"].Value = funs.DecimalPoint(funs.Roundoff(ansGridView1.Rows[i].Cells["taxamt4"].Value.ToString()), 2);
                        ansGridView1.Rows[i].Cells["TotTaxAmount"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[i].Cells["taxamt1"].Value.ToString()) + double.Parse(ansGridView1.Rows[i].Cells["taxamt2"].Value.ToString()) + double.Parse(ansGridView1.Rows[i].Cells["taxamt3"].Value.ToString()) + double.Parse(ansGridView1.Rows[i].Cells["taxamt4"].Value.ToString()), 2);
                    }
                    ctaxamt1 += double.Parse(ansGridView1.Rows[i].Cells["taxamt1"].Value.ToString());
                    ctaxamt2 += double.Parse(ansGridView1.Rows[i].Cells["taxamt2"].Value.ToString());
                    ctaxamt3 += double.Parse(ansGridView1.Rows[i].Cells["taxamt3"].Value.ToString());
                    ctaxamt4 += double.Parse(ansGridView1.Rows[i].Cells["taxamt4"].Value.ToString());
                    totTaxabe += double.Parse(ansGridView1.Rows[i].Cells["Taxabelamount"].Value.ToString());
                    ansGridView1.Rows[i].Cells["Amount5"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[i].Cells["Taxabelamount"].Value.ToString()) + double.Parse(ansGridView1.Rows[i].Cells["TotTaxAmount"].Value.ToString()), 2);
                }
            }
            textBox24.Text = funs.DecimalPoint(totTaxabe, 2);
            textBox6.Text = funs.DecimalPoint(ctaxamt1, 2);
            textBox19.Text = funs.DecimalPoint(ctaxamt2, 2);
            textBox21.Text = funs.DecimalPoint(ctaxamt3, 2);
            textBox22.Text = funs.DecimalPoint(ctaxamt4, 2);
            textBox23.Text = funs.DecimalPoint(ctaxamt1 + ctaxamt2 + ctaxamt3 + ctaxamt4, 2);
        }

        private void ExpCalc(int rowindex)
        {
            double amt = 0;
            amt = double.Parse(textBox25.Text);
            for (int i = 0; i < rowindex; i++)
            {
                amt += double.Parse(ansGridView4.Rows[i].Cells["CamountB"].Value.ToString());
            }
            DataTable dtAddSub1;
            if (Master.Charges.Select("Name='" + ansGridView4.Rows[rowindex].Cells["Charg_Name2"].Value + "' ", "").Length == 0)
            {
                return;
            }
            else
            {
                dtAddSub1 = Master.Charges.Select("Name='" + ansGridView4.Rows[rowindex].Cells["Charg_Name2"].Value + "' ", "").CopyToDataTable();
            }
            if (bool.Parse(ansGridView4.Rows[rowindex].Cells["Changed2"].Value.ToString()) == false)
            {
                if (int.Parse(dtAddSub1.Rows[0]["Charge_type"].ToString()) == 1 && int.Parse(dtAddSub1.Rows[0]["add_sub"].ToString()) == 4)
                {
                    ansGridView4.Rows[rowindex].Cells["Accid2"].Value = dtAddSub1.Rows[0]["Ac_id"].ToString();
                    ansGridView4.Rows[rowindex].Cells["Ctype2"].Value = dtAddSub1.Rows[0]["Charge_type"].ToString();
                    ansGridView4.Rows[rowindex].Cells["Addsub2"].Value = dtAddSub1.Rows[0]["add_sub"].ToString();
                    ansGridView4.Rows[rowindex].Cells["CamountB"].Value = (amt * double.Parse(ansGridView4.Rows[rowindex].Cells["AmountB"].Value.ToString())) / 100;
                    ansGridView4.Rows[rowindex].Cells["CamountB"].ReadOnly = false;
                }
                else if (int.Parse(dtAddSub1.Rows[0]["Charge_type"].ToString()) == 1 && int.Parse(dtAddSub1.Rows[0]["add_sub"].ToString()) == 5)
                {
                    ansGridView4.Rows[rowindex].Cells["Accid2"].Value = dtAddSub1.Rows[0]["Ac_id"].ToString();
                    ansGridView4.Rows[rowindex].Cells["Ctype2"].Value = dtAddSub1.Rows[0]["Charge_type"].ToString();
                    ansGridView4.Rows[rowindex].Cells["Addsub2"].Value = dtAddSub1.Rows[0]["add_sub"].ToString();
                    ansGridView4.Rows[rowindex].Cells["CamountB"].Value = -(amt * double.Parse(ansGridView4.Rows[rowindex].Cells["AmountB"].Value.ToString()) / 100);
                    ansGridView4.Rows[rowindex].Cells["CamountB"].ReadOnly = false;
                }
                else if (int.Parse(dtAddSub1.Rows[0]["Charge_type"].ToString()) == 3 && int.Parse(dtAddSub1.Rows[0]["add_sub"].ToString()) == 4)
                {
                    ansGridView4.Rows[rowindex].Cells["Accid2"].Value = dtAddSub1.Rows[0]["Ac_id"].ToString();
                    ansGridView4.Rows[rowindex].Cells["Ctype2"].Value = dtAddSub1.Rows[0]["Charge_type"].ToString();
                    ansGridView4.Rows[rowindex].Cells["Addsub2"].Value = dtAddSub1.Rows[0]["add_sub"].ToString();
                    ansGridView4.Rows[rowindex].Cells["CamountB"].Value = double.Parse(ansGridView4.Rows[rowindex].Cells["AmountB"].Value.ToString());
                    ansGridView4.Rows[rowindex].Cells["CamountB"].ReadOnly = true;
                }
                else if (int.Parse(dtAddSub1.Rows[0]["Charge_type"].ToString()) == 3 && int.Parse(dtAddSub1.Rows[0]["add_sub"].ToString()) == 5)
                {
                    ansGridView4.Rows[rowindex].Cells["Accid2"].Value = dtAddSub1.Rows[0]["Ac_id"].ToString();
                    ansGridView4.Rows[rowindex].Cells["Ctype2"].Value = dtAddSub1.Rows[0]["Charge_type"].ToString();
                    ansGridView4.Rows[rowindex].Cells["Addsub2"].Value = dtAddSub1.Rows[0]["add_sub"].ToString();
                    ansGridView4.Rows[rowindex].Cells["CamountB"].Value = -(double.Parse(ansGridView4.Rows[rowindex].Cells["AmountB"].Value.ToString()));
                    ansGridView4.Rows[rowindex].Cells["CamountB"].ReadOnly = true;
                }
            }
        }

        private void DisCalc(int rowindex)
        {
            double amt = 0;
            amt = double.Parse(textBox4.Text);
            for (int i = 0; i < rowindex; i++)
            {
                amt += double.Parse(ansGridView3.Rows[i].Cells["CamountA"].Value.ToString());
            }
            DataTable dtAddSub;
            if (Master.Charges.Select("Name='" + ansGridView3.Rows[rowindex].Cells["Charg_Name"].Value.ToString() + "' ", "").Length == 0)
            {
                return;
            }
            else
            {
                dtAddSub = Master.Charges.Select("Name='" + ansGridView3.Rows[rowindex].Cells["Charg_Name"].Value + "'", "").CopyToDataTable();
            }

            if (bool.Parse(ansGridView3.Rows[rowindex].Cells["Changed1"].Value.ToString()) == false)
            {
                if (int.Parse(dtAddSub.Rows[0]["Charge_type"].ToString()) == 1 && int.Parse(dtAddSub.Rows[0]["add_sub"].ToString()) == 4)
                {
                    ansGridView3.Rows[rowindex].Cells["Accid1"].Value = dtAddSub.Rows[0]["Ac_id"].ToString();
                    ansGridView3.Rows[rowindex].Cells["Ctype1"].Value = dtAddSub.Rows[0]["Charge_type"].ToString();
                    ansGridView3.Rows[rowindex].Cells["Addsub1"].Value = dtAddSub.Rows[0]["add_sub"].ToString();
                    ansGridView3.Rows[rowindex].Cells["CamountA"].Value = funs.DecimalPoint((amt * double.Parse(ansGridView3.Rows[rowindex].Cells["AmountA"].Value.ToString())) / 100);
                    ansGridView3.Rows[rowindex].Cells["CamountA"].ReadOnly = false;
                }
                else if (int.Parse(dtAddSub.Rows[0]["Charge_type"].ToString()) == 1 && int.Parse(dtAddSub.Rows[0]["add_sub"].ToString()) == 5)
                {
                    ansGridView3.Rows[rowindex].Cells["Accid1"].Value = dtAddSub.Rows[0]["Ac_id"].ToString();
                    ansGridView3.Rows[rowindex].Cells["Ctype1"].Value = dtAddSub.Rows[0]["Charge_type"].ToString();
                    ansGridView3.Rows[rowindex].Cells["Addsub1"].Value = dtAddSub.Rows[0]["add_sub"].ToString();
                    ansGridView3.Rows[rowindex].Cells["CamountA"].Value = funs.DecimalPoint(-(amt * double.Parse(ansGridView3.Rows[rowindex].Cells["AmountA"].Value.ToString()) / 100));
                    ansGridView3.Rows[rowindex].Cells["CamountA"].ReadOnly = false;
                }
                else if (int.Parse(dtAddSub.Rows[0]["Charge_type"].ToString()) == 3 && int.Parse(dtAddSub.Rows[0]["add_sub"].ToString()) == 4)
                {
                    ansGridView3.Rows[rowindex].Cells["Accid1"].Value = dtAddSub.Rows[0]["Ac_id"].ToString();
                    ansGridView3.Rows[rowindex].Cells["Ctype1"].Value = dtAddSub.Rows[0]["Charge_type"].ToString();
                    ansGridView3.Rows[rowindex].Cells["Addsub1"].Value = dtAddSub.Rows[0]["add_sub"].ToString();
                    ansGridView3.Rows[rowindex].Cells["CamountA"].Value = funs.DecimalPoint((ansGridView3.Rows[rowindex].Cells["AmountA"].Value.ToString()));
                    ansGridView3.Rows[rowindex].Cells["CamountA"].ReadOnly = true;
                }
                else if (int.Parse(dtAddSub.Rows[0]["Charge_type"].ToString()) == 3 && int.Parse(dtAddSub.Rows[0]["add_sub"].ToString()) == 5)
                {
                    ansGridView3.Rows[rowindex].Cells["Accid1"].Value = dtAddSub.Rows[0]["Ac_id"].ToString();
                    ansGridView3.Rows[rowindex].Cells["Ctype1"].Value = dtAddSub.Rows[0]["Charge_type"].ToString();
                    ansGridView3.Rows[rowindex].Cells["Addsub1"].Value = dtAddSub.Rows[0]["add_sub"].ToString();
                    ansGridView3.Rows[rowindex].Cells["CamountA"].Value = funs.DecimalPoint(-double.Parse((ansGridView3.Rows[rowindex].Cells["AmountA"].Value.ToString())));
                    ansGridView3.Rows[rowindex].Cells["CamountA"].ReadOnly = true;
                }
                else if (int.Parse(dtAddSub.Rows[0]["Charge_type"].ToString()) == 2 && int.Parse(dtAddSub.Rows[0]["add_sub"].ToString()) == 4)
                {
                    ansGridView3.Rows[rowindex].Cells["Accid1"].Value = dtAddSub.Rows[0]["Ac_id"].ToString();
                    ansGridView3.Rows[rowindex].Cells["Ctype1"].Value = dtAddSub.Rows[0]["Charge_type"].ToString();
                    ansGridView3.Rows[rowindex].Cells["Addsub1"].Value = dtAddSub.Rows[0]["add_sub"].ToString();
                    ansGridView3.Rows[rowindex].Cells["CamountA"].Value = funs.DecimalPoint(double.Parse(ansGridView3.Rows[rowindex].Cells["AmountA"].Value.ToString()) * double.Parse(textBox20.Text));
                    ansGridView3.Rows[rowindex].Cells["CamountA"].ReadOnly = true;
                }
                else if (int.Parse(dtAddSub.Rows[0]["Charge_type"].ToString()) == 2 && int.Parse(dtAddSub.Rows[0]["add_sub"].ToString()) == 5)
                {
                    ansGridView3.Rows[rowindex].Cells["Accid1"].Value = dtAddSub.Rows[0]["Ac_id"].ToString();
                    ansGridView3.Rows[rowindex].Cells["Ctype1"].Value = dtAddSub.Rows[0]["Charge_type"].ToString();
                    ansGridView3.Rows[rowindex].Cells["Addsub1"].Value = dtAddSub.Rows[0]["add_sub"].ToString();
                    ansGridView3.Rows[rowindex].Cells["CamountA"].Value = funs.DecimalPoint(-1 * (double.Parse(ansGridView3.Rows[rowindex].Cells["AmountA"].Value.ToString()) * double.Parse(textBox20.Text)));
                    ansGridView3.Rows[rowindex].Cells["CamountA"].ReadOnly = true;
                }
                else if (int.Parse(dtAddSub.Rows[0]["Charge_type"].ToString()) == 4 && int.Parse(dtAddSub.Rows[0]["add_sub"].ToString()) == 4)
                {
                    ansGridView3.Rows[rowindex].Cells["Accid1"].Value = dtAddSub.Rows[0]["Ac_id"].ToString();
                    ansGridView3.Rows[rowindex].Cells["Ctype1"].Value = dtAddSub.Rows[0]["Charge_type"].ToString();
                    ansGridView3.Rows[rowindex].Cells["Addsub1"].Value = dtAddSub.Rows[0]["add_sub"].ToString();
                    ansGridView3.Rows[rowindex].Cells["CamountA"].Value = funs.DecimalPoint(double.Parse(ansGridView3.Rows[rowindex].Cells["AmountA"].Value.ToString()) * double.Parse(textBox16.Text));
                    ansGridView3.Rows[rowindex].Cells["CamountA"].ReadOnly = true;
                }
                else if (int.Parse(dtAddSub.Rows[0]["Charge_type"].ToString()) == 4 && int.Parse(dtAddSub.Rows[0]["add_sub"].ToString()) == 5)
                {
                    ansGridView3.Rows[rowindex].Cells["Accid1"].Value = dtAddSub.Rows[0]["Ac_id"].ToString();
                    ansGridView3.Rows[rowindex].Cells["Ctype1"].Value = dtAddSub.Rows[0]["Charge_type"].ToString();
                    ansGridView3.Rows[rowindex].Cells["Addsub1"].Value = dtAddSub.Rows[0]["add_sub"].ToString();
                    ansGridView3.Rows[rowindex].Cells["CamountA"].Value = funs.DecimalPoint(-1 * (double.Parse(ansGridView3.Rows[rowindex].Cells["AmountA"].Value.ToString()) * double.Parse(textBox16.Text)));
                    ansGridView3.Rows[rowindex].Cells["CamountA"].ReadOnly = true;
                }
            }
        }

        private void ItemCalc(int rowindex)
        {
            if (radioButton10.Checked == true)
            {
                ansGridView1.Rows[rowindex].Cells["Amount0"].Value = double.Parse(ansGridView1.Rows[rowindex].Cells["Quantity"].Value.ToString()) * double.Parse(ansGridView1.Rows[rowindex].Cells["rate_am"].Value.ToString()) * double.Parse(ansGridView1.Rows[rowindex].Cells["sqft"].Value.ToString());
            }
            else if (radioButton9.Checked == true)
            {
                ansGridView1.Rows[rowindex].Cells["Amount0"].Value = double.Parse(ansGridView1.Rows[rowindex].Cells["Quantity"].Value.ToString()) * double.Parse(ansGridView1.Rows[rowindex].Cells["rate_am"].Value.ToString()) * double.Parse(ansGridView1.Rows[rowindex].Cells["sqmt"].Value.ToString());
            }
            //ansGridView1.Rows[rowindex].Cells["Amount0"].Value = double.Parse(ansGridView1.Rows[rowindex].Cells["Quantity"].Value.ToString()) * double.Parse(ansGridView1.Rows[rowindex].Cells["rate_am"].Value.ToString());
            if (ansGridView1.Rows[rowindex].Cells["QDType"].Value == null)
            {
                ansGridView1.Rows[rowindex].Cells["QDType"].Value = "";
            }
            if (ansGridView1.Rows[rowindex].Cells["QDType"].Value.ToString() == "Flat")
            {
                ansGridView1.Rows[rowindex].Cells["QDAmount"].Value = double.Parse(ansGridView1.Rows[rowindex].Cells["qd"].Value.ToString());
            }
            if (ansGridView1.Rows[rowindex].Cells["QDType"].Value.ToString() == "Percentage")
            {
                ansGridView1.Rows[rowindex].Cells["QDAmount"].Value = double.Parse(ansGridView1.Rows[rowindex].Cells["Amount0"].Value.ToString()) * double.Parse(ansGridView1.Rows[rowindex].Cells["qd"].Value.ToString()) / 100;
            }
            if (ansGridView1.Rows[rowindex].Cells["QDType"].Value.ToString() == "Quantity")
            {
                ansGridView1.Rows[rowindex].Cells["QDAmount"].Value = double.Parse(ansGridView1.Rows[rowindex].Cells["qd"].Value.ToString()) * double.Parse(ansGridView1.Rows[rowindex].Cells["Quantity"].Value.ToString());
            }
            if (ansGridView1.Rows[rowindex].Cells["QDType"].Value.ToString() == "Quantity*PackValue")
            {
                ansGridView1.Rows[rowindex].Cells["QDAmount"].Value = double.Parse(ansGridView1.Rows[rowindex].Cells["Quantity"].Value.ToString()) * double.Parse(ansGridView1.Rows[rowindex].Cells["qd"].Value.ToString()) * double.Parse(ansGridView1.Rows[rowindex].Cells["pvalue"].Value.ToString());
            }
            ansGridView1.Rows[rowindex].Cells["Amount1"].Value = double.Parse(ansGridView1.Rows[rowindex].Cells["Amount0"].Value.ToString()) - double.Parse(ansGridView1.Rows[rowindex].Cells["QDAmount"].Value.ToString());
            if (ansGridView1.Rows[rowindex].Cells["CDType"].Value == null)
            {
                ansGridView1.Rows[rowindex].Cells["CDType"].Value = "";
            }
            if (ansGridView1.Rows[rowindex].Cells["CDType"].Value.ToString() == "Flat")
            {
                ansGridView1.Rows[rowindex].Cells["CDAmount"].Value = double.Parse(ansGridView1.Rows[rowindex].Cells["cd"].Value.ToString());
            }
            if (ansGridView1.Rows[rowindex].Cells["CDType"].Value.ToString() == "Percentage")
            {
                ansGridView1.Rows[rowindex].Cells["CDAmount"].Value = double.Parse(ansGridView1.Rows[rowindex].Cells["Amount1"].Value.ToString()) * double.Parse(ansGridView1.Rows[rowindex].Cells["cd"].Value.ToString()) / 100;
            }
            if (ansGridView1.Rows[rowindex].Cells["CDType"].Value.ToString() == "Quantity")
            {
                ansGridView1.Rows[rowindex].Cells["CDAmount"].Value = double.Parse(ansGridView1.Rows[rowindex].Cells["cd"].Value.ToString()) * double.Parse(ansGridView1.Rows[rowindex].Cells["Quantity"].Value.ToString());
            }
            if (ansGridView1.Rows[rowindex].Cells["CDType"].Value.ToString() == "Quantity*PackValue")
            {
                ansGridView1.Rows[rowindex].Cells["CDAmount"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[rowindex].Cells["Quantity"].Value.ToString()) * double.Parse(ansGridView1.Rows[rowindex].Cells["cd"].Value.ToString()) * double.Parse(ansGridView1.Rows[rowindex].Cells["pvalue"].Value.ToString()), 2);
            }
            ansGridView1.Rows[rowindex].Cells["Amount2"].Value = double.Parse(ansGridView1.Rows[rowindex].Cells["Amount1"].Value.ToString()) - double.Parse(ansGridView1.Rows[rowindex].Cells["CDAmount"].Value.ToString());

            if (ansGridView1.Rows[rowindex].Cells["FDType"].Value == null)
            {
                ansGridView1.Rows[rowindex].Cells["FDType"].Value = "";
            }
            if (ansGridView1.Rows[rowindex].Cells["FDType"].Value.ToString() == "Flat")
            {
                ansGridView1.Rows[rowindex].Cells["FDAmount"].Value = double.Parse(ansGridView1.Rows[rowindex].Cells["flatdis"].Value.ToString());
            }
            if (ansGridView1.Rows[rowindex].Cells["FDType"].Value.ToString() == "Percentage")
            {
                ansGridView1.Rows[rowindex].Cells["FDAmount"].Value = double.Parse(ansGridView1.Rows[rowindex].Cells["Amount2"].Value.ToString()) * double.Parse(ansGridView1.Rows[rowindex].Cells["flatdis"].Value.ToString()) / 100;
            }
            if (ansGridView1.Rows[rowindex].Cells["FDType"].Value.ToString() == "Quantity")
            {
                ansGridView1.Rows[rowindex].Cells["FDAmount"].Value = double.Parse(ansGridView1.Rows[rowindex].Cells["flatdis"].Value.ToString()) * double.Parse(ansGridView1.Rows[rowindex].Cells["Quantity"].Value.ToString());
            }
            if (ansGridView1.Rows[rowindex].Cells["FDType"].Value.ToString() == "Quantity*PackValue")
            {
                ansGridView1.Rows[rowindex].Cells["FDAmount"].Value = double.Parse(ansGridView1.Rows[rowindex].Cells["Quantity"].Value.ToString()) * double.Parse(ansGridView1.Rows[rowindex].Cells["flatdis"].Value.ToString()) * double.Parse(ansGridView1.Rows[rowindex].Cells["pvalue"].Value.ToString());
            }
            ansGridView1.Rows[rowindex].Cells["Amount3"].Value = double.Parse(ansGridView1.Rows[rowindex].Cells["Amount2"].Value.ToString()) - double.Parse(ansGridView1.Rows[rowindex].Cells["FDAmount"].Value.ToString());

            if (ansGridView1.Rows[rowindex].Cells["dattype"].Value == null)
            {
                ansGridView1.Rows[rowindex].Cells["dattype"].Value = "";
            }
            if (ansGridView1.Rows[rowindex].Cells["dat"].Value == null)
            {
                ansGridView1.Rows[rowindex].Cells["dat"].Value = 0;
            }
            if (ansGridView1.Rows[rowindex].Cells["dattype"].Value.ToString() == "Flat")
            {
                ansGridView1.Rows[rowindex].Cells["datamount"].Value = double.Parse(ansGridView1.Rows[rowindex].Cells["dat"].Value.ToString());
            }
            if (ansGridView1.Rows[rowindex].Cells["dattype"].Value.ToString() == "Percentage")
            {
                ansGridView1.Rows[rowindex].Cells["datamount"].Value = double.Parse(ansGridView1.Rows[rowindex].Cells["Amount5"].Value.ToString()) * double.Parse(ansGridView1.Rows[rowindex].Cells["dat"].Value.ToString()) / 100;
            }
            if (ansGridView1.Rows[rowindex].Cells["dattype"].Value.ToString() == "Quantity")
            {
                ansGridView1.Rows[rowindex].Cells["datamount"].Value = double.Parse(ansGridView1.Rows[rowindex].Cells["dat"].Value.ToString()) * double.Parse(ansGridView1.Rows[rowindex].Cells["Quantity"].Value.ToString());
            }
            if (ansGridView1.Rows[rowindex].Cells["dattype"].Value.ToString() == "Quantity*PackValue")
            {
                ansGridView1.Rows[rowindex].Cells["datamount"].Value = double.Parse(ansGridView1.Rows[rowindex].Cells["Quantity"].Value.ToString()) * double.Parse(ansGridView1.Rows[rowindex].Cells["dat"].Value.ToString()) * double.Parse(ansGridView1.Rows[rowindex].Cells["pvalue"].Value.ToString());
            }
            double Amount3 = double.Parse(ansGridView1.Rows[rowindex].Cells["Amount3"].Value.ToString());
            double GridDiscount = 0;
            DataRow[] drQd = dtItemCharges.Select("itemsr=" + (rowindex + 1), "Chargesr");
            if (drQd.Length != 0)
            {
                foreach (DataRow drow in drQd)
                {
                    if (int.Parse(drow["Ctype"].ToString()) == 1 && int.Parse(drow["Addsub"].ToString()) == 4)
                    {
                        drow["Camount"] = funs.DecimalPoint((Amount3 * double.Parse(drow["Amount"].ToString()) / 100));
                    }
                    else if (int.Parse(drow["Ctype"].ToString()) == 1 && int.Parse(drow["Addsub"].ToString()) == 5)
                    {
                        drow["Camount"] = funs.DecimalPoint(-(Amount3 * double.Parse(drow["Amount"].ToString()) / 100));
                    }
                    else if (int.Parse(drow["Ctype"].ToString()) == 3 && int.Parse(drow["Addsub"].ToString()) == 4)
                    {
                        drow["Camount"] = funs.DecimalPoint(drow["Amount"].ToString());
                    }
                    else if (int.Parse(drow["Ctype"].ToString()) == 3 && int.Parse(drow["Addsub"].ToString()) == 5)
                    {
                        drow["Camount"] = funs.DecimalPoint(-double.Parse((drow["Amount"].ToString())));
                    }
                    else if (int.Parse(drow["Ctype"].ToString()) == 2 && int.Parse(drow["Addsub"].ToString()) == 4)
                    {
                        drow["Camount"] = funs.DecimalPoint(double.Parse(drow["Amount"].ToString()) * funs.Select_pack_value(ansGridView1.Rows[rowindex].Cells["Des_ac_id"].Value.ToString()) * double.Parse(ansGridView1.Rows[rowindex].Cells["Quantity"].Value.ToString()));
                    }
                    else if (int.Parse(drow["Ctype"].ToString()) == 2 && int.Parse(drow["Addsub"].ToString()) == 5)
                    {
                        drow["Camount"] = funs.DecimalPoint(-(double.Parse(drow["Amount"].ToString()) * funs.Select_pack_value(ansGridView1.Rows[rowindex].Cells["Des_ac_id"].Value.ToString()) * double.Parse(ansGridView1.Rows[rowindex].Cells["Quantity"].Value.ToString())));
                    }
                    GridDiscount += double.Parse(drow["Camount"].ToString());
                }
            }

            ansGridView1.Rows[rowindex].Cells["GridDis"].Value = -1 * GridDiscount;
            if (DirectChangeAmount == true)
            {
                if (ansGridView1.Rows[rowindex].Cells["Amount"].Value == null)
                {
                    ansGridView1.Rows[rowindex].Cells["Amount"].Value = 0;
                }
                ansGridView1.Rows[rowindex].Cells["Amount"].Value = funs.DecimalPoint(ansGridView1.Rows[rowindex].Cells["Amount"].Value.ToString());
            }
            else
            {
                ansGridView1.Rows[rowindex].Cells["Amount"].Value = funs.DecimalPoint(double.Parse(ansGridView1.Rows[rowindex].Cells["Amount3"].Value.ToString()) - double.Parse(ansGridView1.Rows[rowindex].Cells["GridDis"].Value.ToString()), 2);
            }
            labelCalc();
        }

        private void SetColor(double Stock)
        {
            double war = funs.Select_des_Wlavel(desc_id);
            if (Stock <= 0 && (gtype == "Sale" || gtype == "issue" || gtype == "Sale Order") && ansGridView1.CurrentCell != null)
            {
                int row = ansGridView1.CurrentCell.RowIndex;
                ansGridView1.Rows[row].Cells["sno"].Style.BackColor = Color.Red;
            }

            else if (Stock < war && (gtype == "Sale" || gtype == "issue" || gtype == "Sale Order") && ansGridView1.CurrentCell != null)
            {
                int row = ansGridView1.CurrentCell.RowIndex;
                ansGridView1.Rows[row].Cells["sno"].Style.BackColor = Color.Yellow;
            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (ansGridView1.CurrentCell == null)
            {
                return;
            }
            ItemSelected(true, ansGridView1.CurrentCell.RowIndex);
            if (ansGridView1.Rows.Count != 1)
            {

                ItemCalc(ansGridView1.CurrentRow.Index);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (ansGridView1.CurrentCell == null)
            {
                return;
            }
            ItemSelected(true, ansGridView1.CurrentCell.RowIndex);
            if (ansGridView1.Rows.Count != 1)
            {
                ItemCalc(ansGridView1.CurrentRow.Index);
            }
        }

        private void frmTransaction_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Q)
            {
                Database.setFocus(textBox7);
                textBox7.Focus();
            }
            if (e.Control && e.KeyCode == Keys.T)
            {
                frm_ewaybillno frm = new frm_ewaybillno(TransportName, Transdocno, Transdocdate, Vehicleno, Distance, EwayBillno);
                frm.ShowDialog(this);
                EwayBillno = frm.gEwayBillno;
                TransportName = frm.gtransportname;
                Transdocno = frm.gtransdocno;
                Transdocdate = frm.gtransdocdate;
                Vehicleno = frm.gvehicleno;
                DisplayTransportdet();
            }
            if (e.Control && e.KeyCode == Keys.R)
            {
                textBox27.Enabled = true;
                DataTable dt1 = Master.DtRates.Select().CopyToDataTable();
                string ratevalue = SelectCombo.ComboDt(this, dt1, 0);
                if (ratevalue != "")
                {
                    textBox27.Text = ratevalue;
                    Ratesapp = funs.Select_Rates_Id(ratevalue);

                    ItemSelected(true, ansGridView1.CurrentRow.Index);
                    ItemCalc(ansGridView1.CurrentRow.Index);

                    labelCalc();
                }
                else
                {
                    Ratesapp = "";
                }
                SendKeys.Send("{tab}");
                textBox27.Enabled = false;
            }
            if (e.Control && e.KeyCode == Keys.F11)
            {
                locked = false;
                label27.Visible = false;
                SideFill();
            }
            if (e.Control && e.KeyCode == Keys.W)
            {
                if (vid == "")
                {
                    if (validate() == true)
                    {
                        if (gtype != "Opening")
                        {

                            UsersFeature ob = permission.Where(w => w.FeatureName == "Create").FirstOrDefault();
                            if (ob != null && gStr == "" && ob.SelectedValue == "Allowed")
                            {
                                SaveMethod(false, "View");
                            }

                        }
                        else if (gtype == "Opening")
                        {
                            UsersFeature ob = permission.Where(w => w.FeatureName == "Create").FirstOrDefault();
                            if (ob != null && gStr == "" && ob.SelectedValue == "Allowed")
                            {
                                SaveMethod(false, "View");
                            }
                            //if (Database.utype.ToUpper() == "SUPERADMIN")
                            //{
                            //    SaveMethod(false, "View");


                            //}
                            //else if (Database.utype.ToUpper() == "ADMIN" && vid == "")
                            //{
                            //    SaveMethod(false, "View");


                            //}
                        }
                    }
                }
                else
                {

                    if (validate() == true)
                    {
                        permission = funs.GetPermissionKey(gtype);

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
                                    SaveMethod(false, "View");
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
                                    SaveMethod(false, "");

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
                                    SaveMethod(false, "View");
                                }
                            }
                            else
                            {
                                SaveMethod(false, "View");
                            }

                        }
                    }

                }
            }
            if (e.Control && e.KeyCode == Keys.F12)
            {
                e.Handled = true;
                InputBox box = new InputBox("Enter Administrative password", "", true);
                box.ShowDialog(this);
                String pass = box.outStr;
                if (pass.ToLower() == "admin")
                {
                    box = new InputBox("Enter Voucher Number", "", false);
                    box.ShowDialog();
                    if (box.outStr == "")
                    {
                        vno = int.Parse(label10.Text);
                    }
                    else
                    {
                        vno = int.Parse(box.outStr);
                    }
                    label10.Text = vno.ToString();
                    int numtype = funs.chkNumType(vtid);
                    if (numtype != 1)
                    {
                        vid = Database.GetScalarText("Select Vi_id from voucherinfo where Vt_id='" + vtid + "' and Vnumber=" + vno + " and Vdate=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash + " and Branch_id='" + Database.BranchId + "' ");
                    }
                    else
                    {
                        int tempvid = 0;
                        tempvid = Database.GetScalarInt("Select Vi_id from voucherinfo where Vt_id='" + vtid + "' and Vnumber=" + vno + " and Branch_id='" + Database.BranchId + "' ");
                        if (tempvid != 0)
                        {
                            MessageBox.Show("Voucher can't be created on this No.");
                            return;
                        }
                    }
                    f12used = true;
                }
                else
                {
                    MessageBox.Show("Invalid password");
                }
            }

            else if (e.Control && e.KeyCode == Keys.O)
            {
                TextBox tbx1 = this.Controls.Find(Master.TransportDetails.Rows[0]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                field1 = tbx1.Text;

                TextBox tbx2 = this.Controls.Find(Master.TransportDetails.Rows[1]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                field2 = tbx2.Text;

                TextBox tbx3 = this.Controls.Find(Master.TransportDetails.Rows[3]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                field4 = tbx3.Text;

                TextBox tbx4 = this.Controls.Find(Master.TransportDetails.Rows[2]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                field3 = tbx4.Text;

                TextBox tbx5 = this.Controls.Find(Master.TransportDetails.Rows[4]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                field5 = tbx5.Text;

                TextBox tbx6 = this.Controls.Find(Master.TransportDetails.Rows[5]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                field6 = tbx6.Text;

                TextBox tbx7 = this.Controls.Find(Master.TransportDetails.Rows[6]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                field7 = tbx7.Text;

                TextBox tbx8 = this.Controls.Find(Master.TransportDetails.Rows[7]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                field8 = tbx8.Text;

                frm_odetails frm = new frm_odetails(field1, field2, field3, field4, field5, field6, field7, field8);
                frm.LoadData();
                frm.ShowDialog(this);
                field1 = frm.field1;
                field2 = frm.field2;
                field3 = frm.field3;
                field4 = frm.field4;
                field5 = frm.field5;
                field6 = frm.field6;
                field7 = frm.field7;
                field8 = frm.field8;

                tbx1 = this.Controls.Find(Master.TransportDetails.Rows[0]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                tbx1.Text = field1;

                tbx2 = this.Controls.Find(Master.TransportDetails.Rows[1]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                tbx2.Text = field2;

                tbx3 = this.Controls.Find(Master.TransportDetails.Rows[3]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                tbx3.Text = field4;

                tbx4 = this.Controls.Find(Master.TransportDetails.Rows[2]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                tbx4.Text = field3;

                tbx5 = this.Controls.Find(Master.TransportDetails.Rows[4]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                tbx5.Text = field5;

                tbx6 = this.Controls.Find(Master.TransportDetails.Rows[5]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                tbx6.Text = field6;

                tbx7 = this.Controls.Find(Master.TransportDetails.Rows[6]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                tbx7.Text = field7;

                tbx8 = this.Controls.Find(Master.TransportDetails.Rows[7]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                tbx8.Text = field8;
            }

            else if (e.Control && e.KeyCode == Keys.S)
            {
                if (vid == "")
                {
                    if (validate() == true)
                    {
                        if (gtype != "Opening")
                        {
                            UsersFeature ob = permission.Where(w => w.FeatureName == "Create").FirstOrDefault();
                            if (ob != null && gStr == "" && ob.SelectedValue == "Allowed")
                            {
                                SaveMethod(false, "");
                            }

                        }
                        else if (gtype == "Opening")
                        {
                            UsersFeature ob = permission.Where(w => w.FeatureName == "Create").FirstOrDefault();
                            if (ob != null && gStr == "" && ob.SelectedValue == "Allowed")
                            {
                                SaveMethod(false, "");
                            }
                            //if (Database.utype.ToUpper() == "SUPERADMIN")
                            //{
                            //    SaveMethod(false,"");

                            //}
                            //else if (Database.utype.ToUpper() == "ADMIN" && vid == "")
                            //{
                            //    SaveMethod(false,"");

                            //}
                        }
                    }
                }
                else
                {

                    if (validate() == true)
                    {
                        permission = funs.GetPermissionKey(gtype);

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
                                    SaveMethod(false, "");
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
                                    SaveMethod(false, "");

                                }
                                //else
                                //{
                                //    SaveMethod(false,"");
                                //}
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
                                    SaveMethod(false, "");
                                }
                            }
                            else
                            {
                                SaveMethod(false, "");
                            }

                        }
                    }

                }
            }
            if (e.Control && e.KeyCode == Keys.P)
            {
                if (vid == "")
                {
                    if (validate() == true)
                    {
                        if (gtype != "Opening")
                        {
                            UsersFeature ob = permission.Where(w => w.FeatureName == "Create").FirstOrDefault();
                            if (ob != null && gStr == "" && ob.SelectedValue == "Allowed")
                            {
                                SaveMethod(true, "");
                            }

                        }
                        else if (gtype == "Opening")
                        {
                            UsersFeature ob = permission.Where(w => w.FeatureName == "Create").FirstOrDefault();
                            if (ob != null && gStr == "" && ob.SelectedValue == "Allowed")
                            {
                                SaveMethod(true, "");
                            }
                            //if (Database.utype.ToUpper() == "SUPERADMIN")
                            //{
                            //    SaveMethod(true,"");
                            //   // Print();

                            //}
                            //else if (Database.utype.ToUpper() == "ADMIN" && vid == "")
                            //{
                            //    SaveMethod(true,"");
                            //   // Print();

                            //}
                        }
                    }
                }
                else
                {

                    if (validate() == true)
                    {
                        permission = funs.GetPermissionKey(gtype);

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
                                    SaveMethod(true, "");
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
                                    SaveMethod(true, "");

                                }

                                //else
                                //{
                                //    SaveMethod(true,"");
                                //}
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
                                    SaveMethod(true, "");
                                }
                            }
                            else
                            {
                                SaveMethod(true, "");
                            }

                        }
                    }

                }
            }
            else if (e.Control && e.KeyCode == Keys.F5)
            {
                if (Feature.Available("Discount on Grid") == "Yes" && Feature.Available("Company Colour") == "Yes")
                {
                    DataTable dtPackVal = new DataTable();
                    dtVoucherDet = new DataTable();
                    dtVoucherDet.Columns.Add("name");
                    dtVoucherDet.Columns.Add("qty");
                    dtVoucherDet.Columns.Add("pval");
                    dtVoucherDet.Columns.Add("dis");
                    dtVoucherDet.Columns.Add("amt");

                    for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
                    {
                        dtVoucherDet.Rows.Add();
                        dtVoucherDet.Rows[i]["qty"] = ansGridView1.Rows[i].Cells["Quantity"].Value;
                        dtPackVal.Clear();
                        dtVoucherDet.Rows[i]["pval"] = ansGridView1.Rows[i].Cells["Pvalue"].Value;
                        dtDescItem.Clear();
                        Database.GetSqlData("SELECT OTHER.Name FROM DESCRIPTION INNER JOIN OTHER ON DESCRIPTION.Item_id = OTHER.Oth_id WHERE DESCRIPTION.Description='" + ansGridView1.Rows[i].Cells["description"].Value + "'", dtDescItem);
                        if (dtDescItem.Rows.Count > 0)
                        {
                            dtVoucherDet.Rows[i]["name"] = dtDescItem.Rows[0][0];
                        }
                        else
                        {
                            dtVoucherDet.Rows[i]["name"] = "Unspecified";
                        }
                    }

                    dtVoucherDet.AcceptChanges();
                    dtDisp.Clear();
                    DisplayData dd = new DisplayData(dtVoucherDet, dtDisp);
                    dd.ShowDialog(this);
                    dtDisp = dd.gdt;

                    for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
                    {
                        DataRow[] dtSelectedCharges;

                        dtDescItem.Clear();
                        Database.GetSqlData("SELECT OTHER.Name FROM DESCRIPTION INNER JOIN OTHER ON DESCRIPTION.Item_id = OTHER.Oth_id WHERE DESCRIPTION.Description='" + ansGridView1.Rows[i].Cells["description"].Value + "'", dtDescItem);
                        string itemname = "";
                        if (dtDescItem.Rows.Count > 0)
                        {
                            itemname = dtDescItem.Rows[0][0].ToString();
                        }
                        else
                        {
                            itemname = "Unspecified";
                        }

                        dtSelectedCharges = dtDisp.Select("name='" + itemname + "'");
                        if (dtSelectedCharges.Length != 0)
                        {
                            ansGridView1.Rows[i].Cells["qd"].Value = dtSelectedCharges[0]["dis"];
                        }
                    }
                    ansGridView1.Focus();
                }
            }

            else if (e.Shift && e.KeyCode == Keys.F1)
            {
                if (validate() == true)
                {
                    try
                    {
                        Database.BeginTran();
                        PrintOnly("Challan", vno);
                        Database.CommitTran();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Bill Not Saved, Due To An Exception", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Database.RollbackTran();
                    }
                }
            }

            else if (e.Control && e.KeyCode == Keys.D)
            {

                if (vid != "")
                {
                    permission = funs.GetPermissionKey(gtype);
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

        private void textBox14_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (vtid != "")
            {
                string cash = "";
                cash = funs.Select_vt_Cashtran(vtid);

                if (radioButton7.Checked == true && (gtype == "Sale" || gtype == "Return"))
                {
                    strCombo = funs.GetStrCombonew(" (Path LIKE '1;39;%') or (Path LIKE '1;38;%')   or   (Path LIKE '8;40;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ")  or   (Path LIKE '8;39;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ") ", "   Status=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " and Branch_id='" + Database.BranchId + "' ");
                }

                else if ((gtype == "Sale" || gtype == "Return" || gtype == "Pending" || gtype == "issue" || gtype == "Sale Order") && cash == "Allowed")
                {
                    strCombo = funs.GetStrCombonew(" (Path LIKE '1;39;%') or (Path LIKE '1;38;%')  OR  (Path LIKE '1;3;%')   or   (Path LIKE '8;40;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ")  or   (Path LIKE '8;39;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ") ", "   Status=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " and Branch_id='" + Database.BranchId + "' ");
                }
                else if ((gtype == "Sale" || gtype == "Return" || gtype == "issue" || gtype == "Sale Order") && cash == "Not Allowed")
                {
                    strCombo = funs.GetStrCombonew(" (Path LIKE '1;39;%') or (Path LIKE '1;38;%')   or   (Path LIKE '8;40;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ")  or   (Path LIKE '8;39;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ") ", "   Status=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " and Branch_id='" + Database.BranchId + "' ");
                }
                else if ((gtype == "Sale" || gtype == "Return" || gtype == "issue" || gtype == "Sale Order") && cash == "Only Allowed")
                {
                    strCombo = funs.GetStrCombonew(" (Path LIKE '1;3;%')  or (Path LIKE '1;38;%')  or  (Path LIKE '8;40;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ")  or   (Path LIKE '8;39;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ") ", "   Status=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " and Branch_id='" + Database.BranchId + "' ");
                }
                else if ((gtype == "Purchase" || gtype == "P Return" || gtype == "receive" || gtype == "RCM" || gtype == "PWDebitNote") && cash == "Allowed")
                {
                    strCombo = funs.GetStrCombonew(" (Path LIKE '8;40;%') OR  (Path LIKE '1;3;%')  or   (Path LIKE '8;39;%')   or   (Path LIKE '1;39;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ") or   (Path LIKE '1;38;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ") ", "   Status=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " and Branch_id='" + Database.BranchId + "' ");
                }
                else if ((gtype == "Purchase" || gtype == "P Return" || gtype == "receive" || gtype == "RCM" || gtype == "PWDebitNote") && cash == "Not Allowed")
                {
                    strCombo = funs.GetStrCombonew(" (Path LIKE '8;40;%')  or   (Path LIKE '8;39;%')  or   (Path LIKE '1;39;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ") or   (Path LIKE '1;38;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ") ", "   Status=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " and Branch_id='" + Database.BranchId + "' ");
                }
                else if ((gtype == "Purchase" || gtype == "P Return" || gtype == "receive" || gtype == "RCM" || gtype == "PWDebitNote") && cash == "Only Allowed")
                {
                    strCombo = funs.GetStrCombonew(" (Path LIKE '1;3;%')    or   (Path LIKE '1;39;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ")  or   (Path LIKE '1;38;%' and  AllowPS=" + access_sql.Singlequote + "true" + access_sql.Singlequote + ")", "   Status=" + access_sql.Singlequote + "true" + access_sql.Singlequote + " and Branch_id='" + Database.BranchId + "' ");
                }
                else if (gtype == "Opening")
                {
                    strCombo = "select distinct '<MAIN>' as name from account union all SELECT ACCOUNT.Name as name FROM ACCOUNT LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id WHERE (((ACCOUNTYPE.Name)='Godown')) GROUP BY ACCOUNT.Name";
                }

                textBox14.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
                //DateTime dt2 = DateTime.Now;

                //System.Windows.Forms.MessageBox.Show((dt2 - dt1).TotalSeconds.ToString()); 

                string accrateapp = "";
                if (gtype == "Purchase" || gtype == "P Return")
                {
                    accrateapp = Database.GetScalarText("Select Rateapp2 from Account where Name='" + textBox14.Text + "'");
                }
                else
                {
                    accrateapp = Database.GetScalarText("Select Rateapp from Account where Name='" + textBox14.Text + "'");
                }
                if (accrateapp != "")
                {
                    textBox27.Text = funs.Select_Rates_Value(accrateapp);
                    Ratesapp = accrateapp;
                }
                else
                {
                    textBox27.Text = funs.Select_Rates_Value(funs.Select_vt_RateType(vtid));
                    Ratesapp = funs.Select_vt_RateType(vtid);
                }
                permission = funs.GetPermissionKey("Transactions");

                UsersFeature ob3 = permission.Where(w => w.FeatureName == "Action on ChangeRate").FirstOrDefault();

                if (ob3 != null && ob3.SelectedValue == "Update With Account")
                {
                    if (textBox14.Text == "")
                    {
                        ansGridView1.Columns["Rate_am"].ReadOnly = true;
                    }
                    else
                    {
                        ansGridView1.Columns["Rate_am"].ReadOnly = false;
                    }
                }
                double distance = 0;
                distance = Database.GetScalarDecimal("Select Distance from Account where Name='" + textBox14.Text + "'");
                Distance = distance;


                string transport_id = "";
                transport_id = Database.GetScalarText("Select transporter_id from Account where Name='" + textBox14.Text + "'");

                TransportName = funs.Select_ac_nm(transport_id);
                DisplayTransportdet();
                shiptoacc_id = funs.Select_ac_id(textBox14.Text);
                shiptoprint = funs.Select_Print(textBox14.Text);
                shiptoaddress1 = funs.Select_Address1(textBox14.Text);
                shiptoaddress2 = funs.Select_Address2(textBox14.Text);
                shiptoPincode = funs.Select_Pincode(textBox14.Text);
                shiptocontact = funs.Select_Mobile(textBox14.Text);
                shiptoemail = funs.Select_Email(textBox14.Text);
                shiptotin = funs.Select_TIN(textBox14.Text);
                shiptoPan = funs.Select_PAN(textBox14.Text);
                shiptoAadhar = funs.Select_AAdhar(textBox14.Text);
                shiptocityid = funs.Select_ac_City_id(textBox14.Text);
                shiptostate = funs.Select_state_nm(funs.Select_ac_state_id(textBox14.Text).ToString());
                string stateid = "";
                stateid = funs.Select_state_id(shiptostate);
                if (stateid == "")
                {
                    stateid = Database.CompanyState_id;
                }
                if (Database.CompanyState_id == stateid)
                {
                    gExState = false;
                }
                else
                {
                    gExState = true;
                }
                if (Feature.Available("Enable Order Management") == "Yes")
                {
                    if (textBox14.Text != "" && gtype == "Sale")
                    {
                        DataTable dt = new DataTable();
                        Database.GetSqlData("SELECT VOUCHERINFO.Invoiceno, VOUCHERINFO.Vdate, res.Itemsr, res.Description, res.Pack, Sum(res.Quantity) AS SumOfQuantity, res.Rate_am, res.Vi_id FROM (SELECT Voucherdet.Itemsr, Description.Description, Description.Pack, Voucherdet.Quantity, Voucherdet.Rate_am, Voucherdet.Vi_id FROM ((VOUCHERINFO LEFT JOIN Voucherdet ON VOUCHERINFO.Vi_id = Voucherdet.Vi_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN Description ON Voucherdet.Des_ac_id = Description.Des_id WHERE (((VOUCHERTYPE.Type)='Sale Order') AND ((VOUCHERINFO.Iscancel)=" + access_sql.Singlequote + "False" + access_sql.Singlequote + ") AND ((VOUCHERINFO.Ac_id)='" + funs.Select_ac_id(textBox14.Text) + "')) Union all SELECT Voucherdet.ritemsr, Description.Description, Description.Pack, -1*[Quantity] AS Expr1, Voucherdet.Rate_am, Voucherdet.rvi_id FROM ((VOUCHERINFO LEFT JOIN Voucherdet ON VOUCHERINFO.Vi_id = Voucherdet.Vi_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN Description ON Voucherdet.Des_ac_id = Description.Des_id WHERE (((VOUCHERTYPE.Type)='Sale') AND ((VOUCHERINFO.Iscancel)=" + access_sql.Singlequote + "False" + access_sql.Singlequote + ") AND ((VOUCHERINFO.Ac_id)='" + funs.Select_ac_id(textBox14.Text) + "')))  AS res LEFT JOIN VOUCHERINFO ON res.Vi_id = VOUCHERINFO.Vi_id GROUP BY VOUCHERINFO.Invoiceno, VOUCHERINFO.Vdate, res.Itemsr, res.Description, res.Pack, res.Rate_am, res.Vi_id HAVING (((Sum(res.Quantity))>0))", dt);
                        if (dt.Rows.Count == 0)
                        {
                            return;
                        }
                        ansGridView1.Rows.Clear();
                        frm_orderdetails frm = new frm_orderdetails(textBox14.Text, dtdispitems);
                        frm.ShowDialog();
                        dtdispitems = frm.gdt;
                        for (int k = 0; k < dtdispitems.Rows.Count; k++)
                        {
                            ansGridView1.AllowUserToAddRows = false;
                            DataTable dtVoucherDet1 = new DataTable("voucherdet");
                            Database.GetSqlData("select * from voucherdet where vi_id=" + dtdispitems.Rows[k]["Vi_id"].ToString() + " and Itemsr=" + dtdispitems.Rows[k]["Itemsr"].ToString(), dtVoucherDet1);

                            for (int i = 0; i < dtVoucherDet1.Rows.Count; i++)
                            {
                                ansGridView1.Rows.Add();
                                DataTable dtPackName = new DataTable();
                                if (Master.DescriptionInfo.Select("Des_id='" + dtVoucherDet1.Rows[i]["Des_ac_id"] + "'", "").Length == 0)
                                {
                                    return;
                                }
                                else
                                {
                                    dtPackName = Master.DescriptionInfo.Select("Des_id='" + dtVoucherDet1.Rows[i]["Des_ac_id"] + "'", "").CopyToDataTable();
                                }
                                ansGridView1.Rows[k].Cells["sno"].Value = k + 1; ;
                                ansGridView1.Rows[k].Cells["description"].Value = dtVoucherDet1.Rows[i]["Description"];
                                ansGridView1.Rows[k].Cells["Rvi_id"].Value = dtdispitems.Rows[k]["Vi_id"].ToString();
                                ansGridView1.Rows[k].Cells["RItemsr"].Value = dtdispitems.Rows[k]["Itemsr"].ToString();
                                ansGridView1.Rows[k].Cells["Quantity"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet1.Rows[i]["Quantity"].ToString()), 3);
                                ansGridView1.Rows[k].Cells["comqty"].Value = dtVoucherDet1.Rows[i]["comqty"];
                                ansGridView1.Rows[k].Cells["Rate_am"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet1.Rows[i]["Rate_am"].ToString()), 2);
                                ansGridView1.Rows[k].Cells["Amount"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet1.Rows[i]["Amount"].ToString()), 2);
                                ansGridView1.Rows[k].Cells["Des_ac_id"].Value = dtVoucherDet1.Rows[i]["Des_ac_id"];
                                ansGridView1.Rows[k].Cells["Category_Id"].Value = dtVoucherDet1.Rows[i]["Category_Id"];
                                ansGridView1.Rows[k].Cells["Category"].Value = funs.Select_tax_cat_nm(dtVoucherDet1.Rows[i]["Category_Id"].ToString());
                                ansGridView1.Rows[k].Cells["Taxabelamount"].Value = dtVoucherDet1.Rows[i]["Taxabelamount"];
                                ansGridView1.Rows[k].Cells["Batch_Code"].Value = dtVoucherDet1.Rows[i]["Batch_Code"];
                                ansGridView1.Rows[k].Cells["Commission_per"].Value = dtVoucherDet1.Rows[i]["Commission%"];
                                ansGridView1.Rows[k].Cells["unt"].Value = dtVoucherDet1.Rows[i]["packing"];
                                ansGridView1.Rows[k].Cells["orgpack"].Value = dtVoucherDet1.Rows[i]["orgpacking"];
                                ansGridView1.Rows[k].Cells["pvalue"].Value = dtVoucherDet1.Rows[i]["pvalue"];
                                ansGridView1.Rows[k].Cells["rate_unit"].Value = dtVoucherDet1.Rows[i]["Rate_unit"];
                                ansGridView1.Rows[k].Cells["remark1"].Value = dtVoucherDet1.Rows[i]["remark1"];
                                ansGridView1.Rows[k].Cells["remark2"].Value = dtVoucherDet1.Rows[i]["remark2"];
                                if (dtVoucherDet1.Rows[i]["remarkreq"].ToString() == "")
                                {
                                    dtVoucherDet1.Rows[i]["remarkreq"] = false;
                                }
                                if (bool.Parse(dtVoucherDet1.Rows[i]["remarkreq"].ToString()) == true)
                                {
                                    ansGridView1.Rows[k].Cells["remarkreq"].Value = "true";
                                }
                                else
                                {
                                    ansGridView1.Rows[k].Cells["remarkreq"].Value = "false";
                                }
                                if (int.Parse(dtVoucherDet1.Rows[i]["godown_id"].ToString()) == 0)
                                {
                                    ansGridView1.Rows[k].Cells["godown_id"].Value = "<MAIN>";
                                }
                                else
                                {
                                    ansGridView1.Rows[k].Cells["godown_id"].Value = funs.Select_ac_nm(dtVoucherDet1.Rows[i]["godown_id"].ToString());
                                }
                                if (dtVoucherDet1.Rows[i]["qd"].ToString() == "")
                                {
                                    dtVoucherDet1.Rows[i]["qd"] = 0;
                                }
                                if (dtVoucherDet1.Rows[i]["cd"].ToString() == "")
                                {
                                    dtVoucherDet1.Rows[i]["cd"] = 0;
                                }
                                ansGridView1.Rows[k].Cells["qd"].Value = dtVoucherDet1.Rows[i]["qd"];
                                ansGridView1.Rows[k].Cells["cd"].Value = dtVoucherDet1.Rows[i]["cd"];
                                ansGridView1.Rows[k].Cells["MRP"].Value = dtVoucherDet1.Rows[i]["MRP"];
                                ansGridView1.Rows[k].Cells["Cost"].Value = dtVoucherDet1.Rows[i]["Cost"];
                                ansGridView1.Rows[k].Cells["CommissionFix"].Value = dtVoucherDet1.Rows[i]["Commission@"];
                                ansGridView1.Rows[k].Cells["orgdesc"].Value = funs.Select_des_nm(dtVoucherDet1.Rows[i]["Des_ac_id"].ToString());

                                //new fields
                                ansGridView1.Rows[k].Cells["pur_sale_acc"].Value = dtVoucherDet1.Rows[i]["pur_sale_acc"];
                                ansGridView1.Rows[k].Cells["tax1"].Value = dtVoucherDet1.Rows[i]["tax1"];
                                ansGridView1.Rows[k].Cells["tax2"].Value = dtVoucherDet1.Rows[i]["tax2"];
                                ansGridView1.Rows[k].Cells["tax3"].Value = dtVoucherDet1.Rows[i]["tax3"];
                                ansGridView1.Rows[k].Cells["tax4"].Value = dtVoucherDet1.Rows[i]["tax4"];
                                ansGridView1.Rows[k].Cells["rate1"].Value = dtVoucherDet1.Rows[i]["rate1"];
                                ansGridView1.Rows[k].Cells["rate2"].Value = dtVoucherDet1.Rows[i]["rate2"];
                                ansGridView1.Rows[k].Cells["rate3"].Value = dtVoucherDet1.Rows[i]["rate3"];
                                ansGridView1.Rows[k].Cells["rate4"].Value = dtVoucherDet1.Rows[i]["rate4"];
                                ansGridView1.Rows[k].Cells["taxamt1"].Value = dtVoucherDet1.Rows[i]["taxamt1"];
                                ansGridView1.Rows[k].Cells["taxamt2"].Value = dtVoucherDet1.Rows[i]["taxamt2"];
                                ansGridView1.Rows[k].Cells["taxamt3"].Value = dtVoucherDet1.Rows[i]["taxamt3"];
                                ansGridView1.Rows[k].Cells["taxamt4"].Value = dtVoucherDet1.Rows[i]["taxamt4"];
                                ansGridView1.Rows[k].Cells["bottomdis"].Value = dtVoucherDet1.Rows[i]["bottomdis"];
                                if (dtVoucherDet1.Rows[i]["flatdis"].ToString() == "")
                                {
                                    dtVoucherDet1.Rows[i]["flatdis"] = 0;
                                }
                                ansGridView1.Rows[k].Cells["flatdis"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet1.Rows[i]["flatdis"].ToString()), 2);
                                ansGridView1.Rows[k].Cells["Amount0"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet1.Rows[i]["Amount0"].ToString()), 2);
                                ansGridView1.Rows[k].Cells["QDType"].Value = dtVoucherDet1.Rows[i]["QDType"].ToString();
                                ansGridView1.Rows[k].Cells["QDAmount"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet1.Rows[i]["QDAmount"].ToString()), 2);
                                ansGridView1.Rows[k].Cells["Amount1"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet1.Rows[i]["Amount1"].ToString()), 2);
                                ansGridView1.Rows[k].Cells["CDType"].Value = dtVoucherDet1.Rows[i]["CDType"].ToString();
                                ansGridView1.Rows[k].Cells["CDAmount"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet1.Rows[i]["CDAmount"].ToString()), 2);
                                ansGridView1.Rows[k].Cells["Amount2"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet1.Rows[i]["Amount2"].ToString()), 2);
                                ansGridView1.Rows[k].Cells["FDType"].Value = dtVoucherDet1.Rows[i]["FDType"].ToString();
                                ansGridView1.Rows[k].Cells["FDAmount"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet1.Rows[i]["FDAmount"].ToString()), 2);
                                ansGridView1.Rows[k].Cells["Amount3"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet1.Rows[i]["Amount3"].ToString()), 2);
                                ansGridView1.Rows[k].Cells["GridDis"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet1.Rows[i]["GridDis"].ToString()), 2);
                                ansGridView1.Rows[k].Cells["TotalDis"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet1.Rows[i]["TotalDis"].ToString()), 2);
                                ansGridView1.Rows[k].Cells["Amount4"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet1.Rows[i]["Amount4"].ToString()), 2);
                                ansGridView1.Rows[k].Cells["TotTaxPer"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet1.Rows[i]["TotTaxPer"].ToString()), 2);
                                ansGridView1.Rows[k].Cells["TotTaxAmount"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet1.Rows[i]["TotTaxAmount"].ToString()), 2);
                                ansGridView1.Rows[k].Cells["Amount5"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet1.Rows[i]["Amount5"].ToString()), 2);
                                ansGridView1.Rows[k].Cells["ExpAmount"].Value = funs.DecimalPoint(double.Parse(dtVoucherDet1.Rows[i]["ExpAmount"].ToString()), 2);

                                ItemCalc(k);
                            }
                        }
                        ansGridView1.AllowUserToAddRows = true;
                        labelCalc();
                    }
                }

                for (int i = 0; i < ansGridView1.RowCount - 1; i++)
                {
                    ItemSelected(true, i);
                }
                labelCalc();
            }

            if (gtype == "Pending")
            {
                string id = funs.Select_ac_id(textBox14.Text);
                int pen = Database.GetScalarInt("Select count(*) from voucherinfo,vouchertype  where voucherinfo.Vt_id=vouchertype.Vt_id and vouchertype.Type='Pending' and Ac_id='" + id + "'");

                if (pen > 0)
                {
                    MessageBox.Show("This Account has already Pendings. The Software will Upload is Automatically ??");
                    DataTable dtpen = new DataTable();
                    Database.GetSqlData("Select Vi_id,vouchertype.Vt_id,Ac_id,Tdtype from voucherinfo,vouchertype  where voucherinfo.Vt_id=vouchertype.Vt_id and vouchertype.Type='Pending' and Ac_id='" + id + "'", dtpen);
                    LoadData(dtpen.Rows[0]["Vi_id"].ToString(), "Pending", false, false, false);
                }
            }

            if (gtype == "Sale" || gtype == "issue")
            {
                string id = funs.Select_ac_id(textBox14.Text);
                int pen = Database.GetScalarInt("Select count(*) from voucherinfo,vouchertype where voucherinfo.Vt_id=vouchertype.Vt_id and vouchertype.Type='Pending' and Ac_id='" + id + "' ");

                if (pen > 0)
                {
                    MessageBox.Show("This Account has already Pendings. The Software will Upload it Automatically");
                    DataTable dtpen = new DataTable();
                    Database.GetSqlData("Select Vi_id,vouchertype.Vt_id,Ac_id,Tdtype from voucherinfo,vouchertype where voucherinfo.Vt_id=vouchertype.Vt_id and vouchertype.Type='Pending' and Ac_id='" + id + "' ", dtpen);
                    string tvtid = vtid;
                    string accname = textBox14.Text;
                    string strdt = dateTimePicker1.Value.ToString("dd-MMM-yyyy");
                    LoadData(dtpen.Rows[0]["Vi_id"].ToString(), gtype, gExcludingTax, gExState, false);
                    for (int i = 0; i < ansGridView1.RowCount - 1; i++)
                    {
                        ItemSelected(true, i);
                        ItemCalc(i);
                    }
                    labelCalc();
                    vtid = tvtid;
                    textBox15.Text = funs.Select_vt_nm(vtid);
                    textBox14.Text = accname;
                    dateTimePicker1.Value = DateTime.Parse(strdt);
                    vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
                    label10.Text = vno.ToString();
                }
            }
        }
        public void DisplayTransportdet()
        {


            label23.Text = "Transport : " + TransportName;
            label25.Text = "TransportDocNo : " + Transdocno;
            label26.Text = "TransportDocDate : " + Transdocdate.ToString(Database.dformat);
            label28.Text = "TransportVehicle No : " + Vehicleno;
            label29.Text = "Distance(Approx. in Km) : " + funs.DecimalPoint(Distance, 2);
        }
        private void SideFill()
        {
            flowLayoutPanel2.Controls.Clear();
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
                //if (Database.utype.ToUpper() == "USER")
                //{
                //    dtsidefill.Rows[0]["Visible"] = false;
                //}
                //else
                //{
                //    string st = "TOP (" + Feature.Available("Voucher Editing Power") + ")";
                //    if (st.ToUpper() == "TOP (UNLIMITED)")
                //    {
                //        st = "";
                //    }
                //    DataTable dt = new DataTable();

                //    Database.GetSqlData("SELECT " + st + " VOUCHERINFO.Vi_id FROM VOUCHERINFO LEFT OUTER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (VOUCHERTYPE.Type = '" + gtype + "') AND (VOUCHERINFO.Branch_id = '" + Database.BranchId + "') ORDER BY VOUCHERINFO.Nid DESC", dt);

                //    DataTable dtfinal = new DataTable();
                //    if (dt.Select("Vi_id='" + vid + "'").Length > 0)
                //    {
                //        dtfinal = dt.Select("Vi_id='" + vid + "'").CopyToDataTable();
                //    }

                //    if (dtfinal.Rows.Count == 1)
                //    {
                //        EditDelete = true;
                //            dtsidefill.Rows[0]["Visible"] = true;

                //    }

                //    else
                //    {
                //        EditDelete = false;
                //        dtsidefill.Rows[0]["Visible"] = false;
                //    }
                //}



                permission = funs.GetPermissionKey(gtype);

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
                        UsersFeature obalval = permission.Where(w => w.FeatureName == "Alter Restrictions").FirstOrDefault();
                        double days = double.Parse(obalval.SelectedValue.ToString());
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
                        UsersFeature obalval = permission.Where(w => w.FeatureName == "Alter Restrictions").FirstOrDefault();

                        double countres = double.Parse(obalval.SelectedValue.ToString());



                        if (countvou > countres)
                        {
                            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;

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
                //dtsidefill.Rows[0]["Visible"] = true;
                permission = funs.GetPermissionKey(gtype);
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

            //Print
            dtsidefill.Rows.Add();
            dtsidefill.Rows[1]["Name"] = "print";
            dtsidefill.Rows[1]["DisplayName"] = "Print";
            dtsidefill.Rows[1]["ShortcutKey"] = "^P";
            if (vid != "")
            {
                permission = funs.GetPermissionKey(gtype);

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
                        UsersFeature obalval = permission.Where(w => w.FeatureName == "Alter Restrictions").FirstOrDefault();
                        double days = double.Parse(obalval.SelectedValue.ToString());
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
                        UsersFeature obalval = permission.Where(w => w.FeatureName == "Alter Restrictions").FirstOrDefault();

                        double countres = double.Parse(obalval.SelectedValue.ToString());



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
                        if (Database.user_id != user_id)
                        {
                            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;

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
                //if (Database.utype.ToUpper() == "USER")
                //{
                //    dtsidefill.Rows[1]["Visible"] = false;
                //}
                //else
                //{
                //    if (EditDelete == true)
                //    {
                //        dtsidefill.Rows[1]["Visible"] = true;
                //    }
                //    else
                //    {
                //        dtsidefill.Rows[1]["Visible"] = false;
                //    }
                //}
            }
            else
            {
                permission = funs.GetPermissionKey(gtype);
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

            //Close
            dtsidefill.Rows.Add();
            dtsidefill.Rows[2]["Name"] = "quit";
            dtsidefill.Rows[2]["DisplayName"] = "Quit";
            dtsidefill.Rows[2]["ShortcutKey"] = "Esc";
            dtsidefill.Rows[2]["Visible"] = true;

            //RoughEstimate
            dtsidefill.Rows.Add();
            dtsidefill.Rows[3]["Name"] = "roughest";
            dtsidefill.Rows[3]["DisplayName"] = "Rough Estimate";
            dtsidefill.Rows[3]["ShortcutKey"] = "";
            if (Feature.Available("Rough Estimates") == "No")
            {
                dtsidefill.Rows[3]["Visible"] = false;

            }
            else
            {
                dtsidefill.Rows[3]["Visible"] = true;
            }

            //lock
            dtsidefill.Rows.Add();
            dtsidefill.Rows[4]["Name"] = "lock";
            dtsidefill.Rows[4]["DisplayName"] = "Unlocked";
            dtsidefill.Rows[4]["ShortcutKey"] = "^F11";
            if (locked == false)
            {
                dtsidefill.Rows[4]["Visible"] = false;
            }
            else
            {
                dtsidefill.Rows[4]["Visible"] = true;
            }

            //Ctrl+f5dis on another grid
            dtsidefill.Rows.Add();
            dtsidefill.Rows[5]["Name"] = "disonanother";
            dtsidefill.Rows[5]["DisplayName"] = "Ctrl+f5";
            dtsidefill.Rows[5]["ShortcutKey"] = "^F5";

            if (Feature.Available("Discount Count") == "1" && Feature.Available("Company Colour") == "Yes")
            {
                dtsidefill.Rows[5]["Visible"] = true;
            }
            else if (Feature.Available("Discount Count") == "2" && Feature.Available("Company Colour") == "Yes")
            {
                dtsidefill.Rows[5]["Visible"] = true;
            }
            else if (Feature.Available("Discount Count") == "3" && Feature.Available("Company Colour") == "Yes")
            {
                dtsidefill.Rows[5]["Visible"] = true;
            }
            else
            {
                dtsidefill.Rows[5]["Visible"] = false;
            }

            //Vnumberchange
            dtsidefill.Rows.Add();
            dtsidefill.Rows[6]["Name"] = "vnumber";
            dtsidefill.Rows[6]["DisplayName"] = "Change VNo.";
            dtsidefill.Rows[6]["ShortcutKey"] = "^F12";
            dtsidefill.Rows[6]["Visible"] = true;

            //itemcharges
            dtsidefill.Rows.Add();
            dtsidefill.Rows[7]["Name"] = "charges";
            dtsidefill.Rows[7]["DisplayName"] = "Charges Window";
            dtsidefill.Rows[7]["ShortcutKey"] = "";
            dtsidefill.Rows[7]["Visible"] = true;

            //delete
            dtsidefill.Rows.Add();
            dtsidefill.Rows[8]["Name"] = "delete";
            dtsidefill.Rows[8]["DisplayName"] = "Delete";
            dtsidefill.Rows[8]["ShortcutKey"] = "^D";
            permission = funs.GetPermissionKey(gtype);
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



            //delete
            dtsidefill.Rows.Add();
            dtsidefill.Rows[9]["Name"] = "challan";
            dtsidefill.Rows[9]["DisplayName"] = Feature.Available("Show Text on PickingSlip");
            dtsidefill.Rows[9]["ShortcutKey"] = "";
            dtsidefill.Rows[9]["Visible"] = true;

            //Otherdetails
            dtsidefill.Rows.Add();
            dtsidefill.Rows[10]["Name"] = "odetails";
            dtsidefill.Rows[10]["DisplayName"] = "Other Details";
            dtsidefill.Rows[10]["ShortcutKey"] = "^O";
            dtsidefill.Rows[10]["Visible"] = true;

            //Taxes Details
            dtsidefill.Rows.Add();
            dtsidefill.Rows[11]["Name"] = "Taxes";
            dtsidefill.Rows[11]["DisplayName"] = "Taxes Details";
            dtsidefill.Rows[11]["ShortcutKey"] = "";
            dtsidefill.Rows[11]["Visible"] = true;

            dtsidefill.Rows.Add();
            dtsidefill.Rows[12]["Name"] = "shipto";
            dtsidefill.Rows[12]["DisplayName"] = "Ship To";
            dtsidefill.Rows[12]["ShortcutKey"] = "";
            dtsidefill.Rows[12]["Visible"] = true;

            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "dispatchfrom";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Dispatch From";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
            //PrintPreview
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "PrintPre";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Print Preview";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "^W";
            if (vid != "")
            {
                permission = funs.GetPermissionKey(gtype);

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
                // dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
                permission = funs.GetPermissionKey(gtype);
                //create
                UsersFeature ob = permission.Where(w => w.FeatureName == "Create").FirstOrDefault();
                if (ob != null && vid == "" && ob.SelectedValue == "Allowed")
                {
                    dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = true;
                }
                else
                {
                    dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Visible"] = false;
                }
            }
            dtsidefill.Rows.Add();
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["Name"] = "transportdet";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["DisplayName"] = "Transport Det.";
            dtsidefill.Rows[dtsidefill.Rows.Count - 1]["ShortcutKey"] = "^T";


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
                    flowLayoutPanel2.Controls.Add(btn);
                }
            }
        }

        private void SaveMethod(bool print, string mode)
        {
            //try
            //{
            //    Database.BeginTran();
            if (gresave == false)
            {
                if (Feature.Available("Freeze Transaction") == "No")
                {
                    string act_id = Database.GetScalarText("Select Act_id from Account where Name='" + textBox14.Text + "' and Branch_id='" + Database.BranchId + "'");
                    if (gtype != "Opening")
                    {
                        if (Feature.Available("Required PaymentMode Form").ToUpper() == "YES" && act_id != "SER3")
                        {
                            frm_paymentmode frm = new frm_paymentmode(dtpaymentdet, vid, double.Parse(textBox10.Text));
                            frm.ShowDialog(this);
                            dtpaymentdet = frm.gdt;
                        }
                        else if (vid != "0" && act_id == "SER3")
                        {
                            dtpaymentdet = new DataTable("Voucherpaydet");
                            Database.GetSqlData("Select * from Voucherpaydet where Vi_id='" + vid + "' ", dtpaymentdet);
                            for (int i = 0; i < dtpaymentdet.Rows.Count; i++)
                            {
                                dtpaymentdet.Rows[i].Delete();
                            }
                            Database.SaveData(dtpaymentdet);
                        }
                    }
                    save();
                }
                else
                {
                    if (dateTimePicker1.Value > DateTime.Parse(Feature.Available("Freeze Transaction")))
                    {

                        string act_id = Database.GetScalarText("Select Act_id from Account where Name='" + textBox14.Text + "' and Branch_id='" + Database.BranchId + "'");
                        if (Feature.Available("Required PaymentMode Form").ToUpper() == "YES" && act_id != "SER3")
                        {
                            frm_paymentmode frm = new frm_paymentmode(dtpaymentdet, vid, double.Parse(textBox10.Text));
                            frm.ShowDialog(this);
                            dtpaymentdet = frm.gdt;
                        }
                        else if (vid != "0" && act_id == "SER3")
                        {
                            dtpaymentdet = new DataTable("Voucherpaydet");
                            Database.GetSqlData("Select * from Voucherpaydet where Vi_id='" + vid + "' ", dtpaymentdet);
                            for (int i = 0; i < dtpaymentdet.Rows.Count; i++)
                            {
                                dtpaymentdet.Rows[i].Delete();
                            }
                            Database.SaveData(dtpaymentdet);
                        }
                        save();
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


                if (funs.Select_MainAccTypeName(textBox14.Text) == "SUNDRY DEBTORS" || funs.Select_MainAccTypeName(textBox14.Text) == "SUNDRY CREDITORS")
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




                    dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Ac_id"] = funs.Select_ac_id(textBox14.Text);
                    dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Vi_id"] = vid;
                    dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Reff_id"] = vid;
                    dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["itemsr"] = 1;
                    dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["AdjustSr"] = 1;

                    dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["Amount"] = double.Parse(dtJournal.Compute("Sum(Amount)", "Ac_id='" + funs.Select_ac_id(textBox14.Text) + "'").ToString());
                    dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["A"] = A;
                    dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["B"] = B;
                    dtBilladjest.Rows[dtBilladjest.Rows.Count - 1]["AB"] = true;

                }


                Database.SaveData(dtBilladjest);



                Master.UpdateAccountinfo();
                funs.ShowBalloonTip("Saved", "Voucher Number: " + vno + " Saved Successfully");




            }





            //    Database.CommitTran();
            //}
            //catch (Exception ex)
            //{
            //    Database.RollbackTran();
            //    MessageBox.Show("Bill Not Saved, Due To An Exception." + ex.Message + Environment.NewLine + vno, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    this.Close();
            //    this.Dispose();
            //    // MessageBox.Show("Bill Not Saved, Due To An Exception" + vno, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}

            if (vid != "")
            {

                if (print == true)
                {
                    Print();
                }

                if (mode == "View")
                {
                    view();
                }

                // Sendsms();
            }
            clear();
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
                    if (gtype != "Opening")
                    {
                        SaveMethod(false, "");

                    }
                    else if (gtype == "Opening")
                    {
                        if (Database.utype.ToUpper() == "SUPERADMIN")
                        {
                            SaveMethod(false, "");

                        }
                        else if (Database.utype.ToUpper() == "ADMIN" && vid == "")
                        {
                            SaveMethod(false, "");

                        }
                    }
                }
            }
            else if (name == "PrintPre")
            {
                frm_printcopy frm = new frm_printcopy("View", vid, vtid);
                frm.Show();
            }
            else if (name == "print")
            {
                if (validate() == true)
                {
                    if (gtype != "Opening")
                    {
                        SaveMethod(true, "");


                    }
                    else if (gtype == "Opening")
                    {
                        if (Database.utype.ToUpper() == "SUPERADMIN")
                        {
                            SaveMethod(true, "");
                            // Print();

                        }
                        else if (Database.utype.ToUpper() == "ADMIN" && vid == "")
                        {
                            SaveMethod(true, "");
                            //Print();

                        }
                    }
                }
            }

            else if (name == "quit")
            {
                if (ansGridView1.Rows.Count > 1)
                {
                    DialogResult chk = MessageBox.Show("Are u sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (chk == DialogResult.Yes)
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

            else if (name == "shipto")
            {
                if (textBox15.Text != "" && textBox14.Text != "")
                {
                    frm_shiptodet frm = new frm_shiptodet(gtype, vid, shiptoacc_id, shiptoprint, shiptoaddress1, shiptoaddress2, shiptocontact, shiptoemail, shiptotin, shiptostate, shiptoPan, shiptoAadhar, gExState, shiptoPincode, shiptocityid);
                    frm.ShowDialog();
                    shiptoacc_id = frm.shipto;
                    shiptoprint = frm.gPrintname;
                    shiptoaddress1 = frm.gAddress1;
                    shiptoaddress2 = frm.gAddress2;
                    shiptocontact = frm.gContact;
                    shiptoemail = frm.gEmail;
                    shiptotin = frm.gTin;
                    shiptostate = frm.gState;
                    shiptoPan = frm.gPAN;
                    shiptoAadhar = frm.gAadhar;
                    gExState = frm.gExstate;
                    shiptocityid = frm.gcityid;
                    shiptoPincode = frm.gPincode;
                    for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
                    {
                        ItemSelected(false, i);
                    }
                    labelCalc();
                }
            }

            else if (name == "roughest")
            {
                if (MessageBox.Show("Are You Sure To Print Estimate, It will not Save in Database", "Estimate", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (Feature.Available("Rough Estimates") == "Yes")
                    {
                        PrintOnly("Rough Estimate", 1);
                    }
                }
            }
            else if (name == "dispatchfrom")
            {
                if (textBox15.Text != "" && textBox14.Text != "")
                {
                    frm_dispatchfrom frm = new frm_dispatchfrom(gtype, gStr, disfromacc_id);
                    frm.ShowDialog(this);
                    disfromacc_id = frm.dispatchfrom;
                }


            }
            else if (name == "lock")
            {
                locked = false;
                label27.Visible = false;
                SideFill();
            }
            else if (name == "disonanother")
            {
                if (Feature.Available("Company Colour") == "Yes")
                {
                    DataTable dtPackVal = new DataTable();

                    dtVoucherDet = new DataTable();
                    dtVoucherDet.Columns.Add("name");
                    dtVoucherDet.Columns.Add("qty");
                    dtVoucherDet.Columns.Add("pval");
                    dtVoucherDet.Columns.Add("dis");
                    dtVoucherDet.Columns.Add("amt");

                    for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
                    {
                        dtVoucherDet.Rows.Add();
                        dtVoucherDet.Rows[i]["qty"] = ansGridView1.Rows[i].Cells["Quantity"].Value;
                        dtPackVal.Clear();
                        dtVoucherDet.Rows[i]["pval"] = ansGridView1.Rows[i].Cells["pvalue"].Value;
                        dtDescItem.Clear();
                        Database.GetSqlData("SELECT OTHER.Name FROM DESCRIPTION INNER JOIN OTHER ON DESCRIPTION.Item_id = OTHER.Oth_id WHERE DESCRIPTION.Description='" + ansGridView1.Rows[i].Cells["description"].Value + "'", dtDescItem);
                        if (dtDescItem.Rows.Count > 0)
                        {
                            dtVoucherDet.Rows[i]["name"] = dtDescItem.Rows[0][0];
                        }
                        else
                        {
                            dtVoucherDet.Rows[i]["name"] = "Unspecified";
                        }
                    }
                    dtVoucherDet.AcceptChanges();
                    dtDisp.Clear();
                    DisplayData dd = new DisplayData(dtVoucherDet, dtDisp);
                    dd.ShowDialog(this);
                    dtDisp = dd.gdt;
                    for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
                    {
                        DataRow[] dtSelectedCharges;
                        dtDescItem.Clear();
                        Database.GetSqlData("SELECT OTHER.Name FROM DESCRIPTION INNER JOIN OTHER ON DESCRIPTION.Item_id = OTHER.Oth_id WHERE DESCRIPTION.Description='" + ansGridView1.Rows[i].Cells["description"].Value + "'", dtDescItem);
                        string itemname = "";
                        if (dtDescItem.Rows.Count > 0)
                        {
                            itemname = dtDescItem.Rows[0][0].ToString();
                        }
                        else
                        {
                            itemname = "Unspecified";
                        }
                        dtSelectedCharges = dtDisp.Select("name='" + itemname + "'");
                        if (dtSelectedCharges.Length != 0)
                        {
                            ansGridView1.Rows[i].Cells["qd"].Value = dtSelectedCharges[0]["dis"];
                        }
                        ItemCalc(i);
                    }
                    ansGridView1.Focus();
                }
            }

            else if (name == "vnumber")
            {
                InputBox box = new InputBox("Enter Administrative password", "", true);
                box.ShowDialog(this);
                String pass = box.outStr;
                if (pass.ToLower() == "admin")
                {
                    box = new InputBox("Enter Voucher Number", "", false);
                    box.ShowDialog();
                    if (box.outStr == "")
                    {
                        vno = int.Parse(label10.Text);
                    }
                    else
                    {
                        vno = int.Parse(box.outStr);
                    }
                    label10.Text = vno.ToString();
                    int numtype = funs.chkNumType(vtid);
                    if (numtype != 1)
                    {
                        vid = Database.GetScalarText("Select Vi_id from voucherinfo where Vt_id='" + vtid + "' and Vnumber=" + vno + " and Vdate=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash);
                    }
                    else
                    {
                        int tempvid = 0;
                        tempvid = Database.GetScalarInt("Select Vi_id from voucherinfo where Vt_id='" + vtid + "' and Vnumber=" + vno);
                        if (tempvid != 0)
                        {
                            MessageBox.Show("Voucher can't be created on this No.");
                            return;
                        }
                    }
                    f12used = true;
                }
                else
                {
                    MessageBox.Show("Invalid password");
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
                if (itemCharges == true)
                {
                    int rnum = ansGridView1.CurrentRow.Index;
                    frmItemCharges frm = new frmItemCharges(dtItemCharges, vid, ansGridView1.CurrentCell.OwningRow.Index + 1, "select [name] from charges where Ac_id=''", double.Parse(ansGridView1.CurrentCell.OwningRow.Cells["Amount3"].Value.ToString()), ansGridView1.CurrentCell.OwningRow.Cells["des_ac_id"].Value.ToString(), double.Parse(ansGridView1.CurrentCell.OwningRow.Cells["Quantity"].Value.ToString()));
                    frm.ShowDialog(this);
                    dtItemCharges = frm.gdt;
                    ItemCalc(ansGridView1.CurrentCell.OwningRow.Index);
                }
            }

            else if (name == "delete")
            {
                if (vid != "")
                {
                    if (MessageBox.Show("Are You Sure To Delete This Voucher", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Database.BeginTran();
                        if (Feature.Available("Freeze Transaction") == "No")
                        {
                            delete();
                        }
                        else
                        {
                            if (dateTimePicker1.Value > DateTime.Parse(Feature.Available("Freeze Transaction")))
                            {
                                delete();
                            }
                            else
                            {
                                MessageBox.Show("Your Voucher is Freezed");
                            }
                        }
                        Database.CommitTran();
                        this.Dispose();
                        this.Close();
                    }
                }
            }
            else if (name == "challan")
            {
                if (validate() == true)
                {
                    PrintOnly("Challan", vno);
                }
            }
            else if (name == "transportdet")
            {
                // 
                frm_ewaybillno frm = new frm_ewaybillno(TransportName, Transdocno, Transdocdate, Vehicleno, Distance, EwayBillno);
                frm.ShowDialog(this);
                EwayBillno = frm.gEwayBillno;
                TransportName = frm.gtransportname;
                Transdocno = frm.gtransdocno;
                Transdocdate = frm.gtransdocdate;
                Vehicleno = frm.gvehicleno;
                Distance = frm.gdistance;
                DisplayTransportdet();
            }

            else if (name == "odetails")
            {
                TextBox tbx1 = this.Controls.Find(Master.TransportDetails.Rows[0]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                field1 = tbx1.Text;

                TextBox tbx2 = this.Controls.Find(Master.TransportDetails.Rows[1]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                field2 = tbx2.Text;

                TextBox tbx3 = this.Controls.Find(Master.TransportDetails.Rows[3]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                field4 = tbx3.Text;

                TextBox tbx4 = this.Controls.Find(Master.TransportDetails.Rows[2]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                field3 = tbx4.Text;

                TextBox tbx5 = this.Controls.Find(Master.TransportDetails.Rows[4]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                field5 = tbx5.Text;

                TextBox tbx6 = this.Controls.Find(Master.TransportDetails.Rows[5]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                field6 = tbx6.Text;

                TextBox tbx7 = this.Controls.Find(Master.TransportDetails.Rows[6]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                field7 = tbx7.Text;

                TextBox tbx8 = this.Controls.Find(Master.TransportDetails.Rows[7]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                field8 = tbx8.Text;

                frm_odetails frm = new frm_odetails(field1, field2, field3, field4, field5, field6, field7, field8);
                frm.LoadData();
                frm.ShowDialog(this);
                field1 = frm.field1;
                field2 = frm.field2;
                field3 = frm.field3;
                field4 = frm.field4;
                field5 = frm.field5;
                field6 = frm.field6;
                field7 = frm.field7;
                field8 = frm.field8;

                tbx1 = this.Controls.Find(Master.TransportDetails.Rows[0]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                tbx1.Text = field1;

                tbx2 = this.Controls.Find(Master.TransportDetails.Rows[1]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                tbx2.Text = field2;

                tbx3 = this.Controls.Find(Master.TransportDetails.Rows[3]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                tbx3.Text = field4;

                tbx4 = this.Controls.Find(Master.TransportDetails.Rows[2]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                tbx4.Text = field3;

                tbx5 = this.Controls.Find(Master.TransportDetails.Rows[4]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                tbx5.Text = field5;

                tbx6 = this.Controls.Find(Master.TransportDetails.Rows[5]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                tbx6.Text = field6;

                tbx7 = this.Controls.Find(Master.TransportDetails.Rows[6]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                tbx7.Text = field7;

                tbx8 = this.Controls.Find(Master.TransportDetails.Rows[7]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                tbx8.Text = field8;
            }
            else if (name == "Taxes")
            {
                if (ansGridView1.CurrentCell != null)
                {
                    int Rindex = ansGridView1.CurrentCell.RowIndex;
                    if (double.Parse(ansGridView1.Rows[Rindex].Cells["Taxabelamount"].Value.ToString()) != 0)
                    {
                        frm_itementry frmTaxes = new frm_itementry(TaxChanged, ansGridView1.Rows[Rindex].Cells["Taxabelamount"].Value.ToString(), ansGridView1.Rows[Rindex].Cells["pur_sale_acc"].Value.ToString(), ansGridView1.Rows[Rindex].Cells["rate1"].Value.ToString(), ansGridView1.Rows[Rindex].Cells["taxamt1"].Value.ToString(), ansGridView1.Rows[Rindex].Cells["tax1"].Value.ToString(), ansGridView1.Rows[Rindex].Cells["rate2"].Value.ToString(), ansGridView1.Rows[Rindex].Cells["taxamt2"].Value.ToString(), ansGridView1.Rows[Rindex].Cells["tax2"].Value.ToString(), ansGridView1.Rows[Rindex].Cells["rate3"].Value.ToString(), ansGridView1.Rows[Rindex].Cells["taxamt3"].Value.ToString(), ansGridView1.Rows[Rindex].Cells["tax3"].Value.ToString(), ansGridView1.Rows[Rindex].Cells["rate4"].Value.ToString(), ansGridView1.Rows[Rindex].Cells["taxamt4"].Value.ToString(), ansGridView1.Rows[Rindex].Cells["tax4"].Value.ToString());
                        frmTaxes.ShowDialog(this);
                        ansGridView1.Rows[Rindex].Cells["taxamt1"].Value = frmTaxes.taxamt1;
                        ansGridView1.Rows[Rindex].Cells["taxamt2"].Value = frmTaxes.taxamt2;
                        ansGridView1.Rows[Rindex].Cells["taxamt3"].Value = frmTaxes.taxamt3;
                        ansGridView1.Rows[Rindex].Cells["taxamt4"].Value = frmTaxes.taxamt4;
                        TaxChanged = frmTaxes.TaxChanged;
                        labelCalc();
                    }
                }
            }
        }

        private void textBox14_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox14);
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = funs.accbal(funs.Select_ac_id(textBox14.Text), dateTimePicker1.Value);
            if (vtid != "" && textBox14.Text != "")
            {
                textBox1.Text = funs.accbal(funs.Select_ac_id(textBox14.Text), dateTimePicker1.Value);
                SetAgent(textBox14.Text);
                SetSalesMAn(textBox14.Text);
                checkLock();
                SetDuedate(textBox14.Text);
                AccGpLimit();
                SideFill();
            }
        }

        private void dispAccInfo()
        {
            DataTable dtAccInfo = new DataTable();
            Database.GetSqlData("select [name],Address1,address2,phone,email from account where Ac_id='" + funs.Select_ac_id(textBox14.Text) + "' ", dtAccInfo);
            frm_main.clearDisplay1();
            frm_main.dtDisplay1.Columns.Add("Item");
            frm_main.dtDisplay1.Columns.Add("Description");
            if (dtAccInfo.Rows.Count > 0)
            {
                frm_main.dtDisplay1.Rows.Add();
                frm_main.dtDisplay1.Rows[0]["Item"] = "Name";
                frm_main.dtDisplay1.Rows[0]["Description"] = dtAccInfo.Rows[0]["name"];
                frm_main.dtDisplay1.Rows.Add();
                frm_main.dtDisplay1.Rows[1]["Item"] = "Address";
                frm_main.dtDisplay1.Rows[1]["Description"] = dtAccInfo.Rows[0]["Address1"];
                frm_main.dtDisplay1.Rows.Add();
                frm_main.dtDisplay1.Rows[2]["Item"] = "";
                frm_main.dtDisplay1.Rows[2]["Description"] = dtAccInfo.Rows[0]["Address2"];
                frm_main.dtDisplay1.Rows.Add();
                frm_main.dtDisplay1.Rows[3]["Item"] = "Phone";
                frm_main.dtDisplay1.Rows[3]["Description"] = dtAccInfo.Rows[0]["phone"];
                frm_main.dtDisplay1.Rows.Add();
                frm_main.dtDisplay1.Rows[4]["Item"] = "Email";
                if (dtAccInfo.Rows[0]["email"].ToString() != "None")
                {
                    frm_main.dtDisplay1.Rows[4]["Description"] = dtAccInfo.Rows[0]["email"];
                }
            }
        }

        private void dispItemInfo()
        {
            frm_main.clearDisplay1();
            frm_main.dtDisplay1.Columns.Add("Item");
            frm_main.dtDisplay1.Columns.Add("Description");
            frm_main.dtDisplay1.Rows.Add();
            frm_main.dtDisplay1.Rows[0]["Item"] = "Description";
            if (ansGridView1.CurrentCell.OwningRow.Cells["unt"].Value == null)
            {
                return;
            }
            if (ansGridView1.CurrentCell.OwningRow.Cells["description"].Value.ToString() != null)
            {
                frm_main.dtDisplay1.Rows[0]["Description"] = desc;
            }
            frm_main.dtDisplay1.Rows.Add();
            frm_main.dtDisplay1.Rows[1]["Item"] = "Unit";
            if (ansGridView1.CurrentCell.OwningRow.Cells["unt"].Value.ToString() != null || ansGridView1.CurrentCell.OwningRow.Cells["unt"].Value.ToString() != "")
            {
                frm_main.dtDisplay1.Rows[1]["Description"] = unit;
            }
            frm_main.dtDisplay1.Rows.Add();
            frm_main.dtDisplay1.Rows[2]["Item"] = "Stock";
            if (ansGridView1.CurrentCell.OwningRow.Cells["Des_ac_id"].Value != null)
            {
                frm_main.dtDisplay1.Rows[2]["Description"] = funs.Stock(desc_id) + " Unit ";
            }
        }

        private void checkLock()
        {
            if (Feature.Available("Customer Credit Limits") == "No")
            {
                locked = false;
                label27.Visible = false;
                SideFill();
                return;
            }
            if (gtype == "Sale" || gtype == "issue")
            {
                DataTable dtCrLimit = new DataTable();
                if (Master.Account.Select("(Act_id='SER4' or Act_id='SER5' or Act_id='SER13' or Act_id<>'SER3') and Name='" + textBox14.Text + "'").Length > 0)
                {
                    dtCrLimit = Master.Account.Select("(Act_id='SER4' or Act_id='SER5' or Act_id='SER13' or Act_id<>'SER3') and Name='" + textBox14.Text + "'").CopyToDataTable();
                }
                String[] val = textBox1.Text.Split(' ');
                double totalamount = double.Parse(val[0]) + double.Parse(textBox10.Text);
                if (dtCrLimit.Rows.Count > 0)
                {
                    if (totalamount > double.Parse(dtCrLimit.Rows[0]["Blimit"].ToString()))
                    {
                        locked = true;
                        label27.Visible = true;
                        SideFill();
                        label27.Text = "Account Credit limit Exceed. Account is locked";
                    }
                    else
                    {
                        locked = false;
                        label27.Visible = false;
                        SideFill();
                    }
                }
            }
        }

        private void textBox15_KeyPress(object sender, KeyPressEventArgs e)
        {
            textBox15.Text = SelectCombo.ComboKeypress(this, e.KeyChar, cmbVouTyp, e.KeyChar.ToString(), 0);
            if (textBox15.Text == "")
            {
                return;
            }
            vtid = funs.Select_vt_id_vnm(textBox15.Text);
            textBox27.Text = funs.Select_Rates_Value(funs.Select_vt_RateType(vtid));
            if (textBox27.Text != "")
            {
                Ratesapp = Master.DtRates.Select("RateValue='" + textBox27.Text + "'").FirstOrDefault()["RateId"].ToString();
                for (int i = 0; i < ansGridView1.RowCount - 1; i++)
                {
                    ItemSelected(true, i);
                    ItemCalc(i);
                }
                labelCalc();
            }
            gtaxinvoice = funs.Select_vt_taxinvoice(vtid);
            gExState = funs.Select_vt_Exstate(vtid);
            gUnregistered = funs.Select_vt_Unregistered(vtid);
            gExcludingTax = funs.Select_vt_Excludungtax(vtid);
            gCalculationType = funs.Select_vt_CalculationType(vtid);

            if (gtype == "Sale" && gExState == true)
            {
                DialogResult chk = MessageBox.Show("Is Company Provide Form-C?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (chk == DialogResult.Yes)
                {
                    formC = true;
                }
                else
                {
                    formC = false;
                }
            }
            else
            {
                formC = false;
            }
            if ((gtype == "Purchase" || gtype == "P Return") && gExState == true)
            {
                SubCategory_Name = "Central Purchase";
            }
            else if ((gtype == "Purchase" || gtype == "P Return") && gExState == false)
            {
                SubCategory_Name = "Local Purchase";
            }
            else if ((gtype == "Sale" || gtype == "Return") && gExState == true)
            {
                SubCategory_Name = "Central Sale";
            }
            else if ((gtype == "Sale" || gtype == "Return" || gtype == "Pending") && gExState == false)
            {
                SubCategory_Name = "Local Sale";
            }
            else if (gtype == "receive")
            {
                SubCategory_Name = "Local Purchase";
            }
            else if (gtype == "issue")
            {
                SubCategory_Name = "Local Sale";
            }
            else if (gtype == "Transfer")
            {
                SubCategory_Name = "Local Purchase";
            }
            if (gCalculationType == "Including Tax Only")
            {
                checkBox1.Enabled = false;
                checkBox1.Checked = true;
                gExcludingTax = false;
            }
            else if (gCalculationType == "Excluding Tax Only")
            {
                checkBox1.Enabled = false;
                checkBox1.Checked = false;
                gExcludingTax = true;
            }
            else if (gCalculationType == "Default Excluding Tax")
            {
                checkBox1.Enabled = true;
                checkBox1.Checked = false;
                gExcludingTax = true;
            }
            else if (gCalculationType == "Default Including Tax")
            {
                checkBox1.Enabled = true;
                checkBox1.Checked = true;
                gExcludingTax = false;
            }
            if (vtid == "")
            {
                ansGridView1.Enabled = false;
            }
            else
            {
                ansGridView1.Enabled = true;
            }
            Displaysetting();
            if (vtid == "SER3")
            {
                groupBox15.Visible = false;
            }
            else
            {
                groupBox15.Visible = true;
            }
            SetVno();
            labelCalc();
        }

        private void ansGridView3_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            ansGridView3.CurrentCell.Value = 0;
        }

        private void ansGridView4_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            ansGridView4.CurrentCell.Value = 0;
        }

        private void textBox14_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox14);
        }

        private void lastVoucher()
        {
            DataTable dtLastTran = new DataTable();
            if (textBox15.Text != "")
            {
                Database.GetSqlData("SELECT temp.[name], temp.vnumber, temp.vdate, VOUCHERINFO.Totalamount FROM (SELECT VOUCHERTYPE.Name, Max(VOUCHERINFO.Vnumber) AS Vnumber, Max(VOUCHERINFO.Vdate) AS Vdate FROM VOUCHERINFO INNER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERTYPE.Name)='" + funs.Select_act_nm(vtid.ToString()) + "' )) GROUP BY VOUCHERTYPE.Name)  AS temp INNER JOIN (VOUCHERINFO INNER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) ON (temp.Vdate = VOUCHERINFO.Vdate) AND (temp.Vnumber = VOUCHERINFO.Vnumber) AND (temp.Name = VOUCHERTYPE.Name)", dtLastTran);
            }

            if (dtLastTran.Rows.Count > 0)
            {
                frm_main.dtDisplay2.Columns.Add("Item");
                frm_main.dtDisplay2.Columns.Add("Description");
                frm_main.dtDisplay2.Rows.Add();
                frm_main.dtDisplay2.Rows[0]["Item"] = "Last Voucher Detail";
                frm_main.dtDisplay2.Rows.Add();
                frm_main.dtDisplay2.Rows[1]["Item"] = "Type";
                frm_main.dtDisplay2.Rows[1]["Description"] = dtLastTran.Rows[0]["name"];
                frm_main.dtDisplay2.Rows.Add();
                frm_main.dtDisplay2.Rows[2]["Item"] = "Voucher No.";
                frm_main.dtDisplay2.Rows[2]["Description"] = dtLastTran.Rows[0]["vnumber"];
                frm_main.dtDisplay2.Rows.Add();
                frm_main.dtDisplay2.Rows[3]["Item"] = "Voucher Date";
                frm_main.dtDisplay2.Rows[3]["Description"] = DateTime.Parse(dtLastTran.Rows[0]["vdate"].ToString()).ToString("dd-MMM-yyyy");
                frm_main.dtDisplay2.Rows.Add();
                frm_main.dtDisplay2.Rows[4]["Item"] = "Total Amount";
                frm_main.dtDisplay2.Rows[4]["Description"] = dtLastTran.Rows[0]["Totalamount"];
            }
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            if (vtid == "SER3")
            {
                groupBox15.Visible = false;
            }
            textBox14.Text = "";
        }

        private void ansGridView3_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

            if (ansGridView3.CurrentCell.OwningColumn.Name == "sno2")
            {
                SendKeys.Send("{right}");
                this.Activate();
            }
            ansGridView3.Rows[e.RowIndex].Cells["sno2"].Value = e.RowIndex + 1;
        }

        private void ansGridView4_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (ansGridView4.CurrentCell.OwningColumn.Name == "sno3")
            {
                SendKeys.Send("{right}");
                this.Activate();
            }
            ansGridView4.Rows[e.RowIndex].Cells["sno3"].Value = e.RowIndex + 1;
        }

        private void ansGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            ansGridView1.Rows[e.RowIndex].Cells["QDAmount"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["CDAmount"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["FDAmount"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["RCMac_id"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["dat"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["datamount"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["taxamt1"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["bottomdis"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["taxamt2"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["taxamt3"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["taxamt4"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["rate1"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["rate2"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["rate3"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["rate4"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["tax1"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["tax2"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["tax3"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["tax4"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["Quantity"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["comqty"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["Rate_am"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["Amount"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["MRP"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["Cost"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["cd"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["qd"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["pvalue"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["flatdis"].Value = 0;
            ansGridView1.Rows[e.RowIndex].Cells["Taxabelamount"].Value = 0;
        }


        private void textBox9_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox9);
        }

        private void ansGridView3_KeyDown(object sender, KeyEventArgs e)
        {
            if (ansGridView3.CurrentCell == null)
            {
                return;
            }

            if (ansGridView3.CurrentCell.OwningColumn.Name == "Camount")
            {
                if (ansGridView3.CurrentRow.Index == ansGridView3.Rows.Count - 1 && ansGridView3.Rows[ansGridView3.CurrentRow.Index].Cells["Camount"].Value == null)
                {
                    SendKeys.Send("{tab}");
                }
            }
            if (e.KeyCode == Keys.Delete)
            {
                if (ansGridView3.CurrentRow.Index == ansGridView3.Rows.Count - 1)
                {
                    for (int i = 1; i < ansGridView3.Columns.Count; i++)
                    {
                        ansGridView3.Rows[ansGridView3.CurrentRow.Index].Cells[i].Value = "";
                    }
                    labelCalc();
                }
                else
                {
                    ansGridView3.Rows.RemoveAt(ansGridView3.CurrentRow.Index);
                    for (int i = 0; i < ansGridView3.Rows.Count; i++)
                    {
                        ansGridView3.Rows[i].Cells["sno2"].Value = (i + 1);
                    }
                    labelCalc();
                }
            }
            if (ansGridView3.CurrentCell.OwningColumn.Name == "Charg_Name")
            {
                if (e.Control && e.KeyCode == Keys.A)
                {
                    if (ansGridView3.CurrentCell.Value.ToString() != "")
                    {
                        ansGridView3.CurrentCell.Value = funs.EditCharge(ansGridView3.CurrentCell.Value.ToString());
                        ansGridView3.CurrentRow.Cells["Charg_id1"].Value = funs.Select_ch_id(ansGridView3.CurrentCell.Value.ToString());
                        ansGridView3.CurrentRow.Cells["Changed1"].Value = false;
                    }
                }
                if (e.Control && e.KeyCode == Keys.C)
                {
                    ansGridView3.CurrentCell.Value = funs.AddCharge();
                    ansGridView3.CurrentRow.Cells["Charg_id1"].Value = funs.Select_ch_id(ansGridView3.CurrentCell.Value.ToString());
                    ansGridView3.CurrentRow.Cells["Changed1"].Value = false;
                }
            }
        }

        private void textBox14_KeyDown(object sender, KeyEventArgs e)
        {
            string stateid = "";
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox14.Text != "")
                {
                    textBox14.Text = funs.EditAccount(textBox14.Text);
                    textBox1.Text = funs.AccountBalance(textBox14.Text);
                    string accrateapp = Database.GetScalarText("Select Rateapp from Account where Name='" + textBox14.Text + "'");
                    if (accrateapp != "")
                    {
                        textBox27.Text = funs.Select_Rates_Value(accrateapp);
                        Ratesapp = accrateapp;
                    }
                    else
                    {
                        textBox27.Text = funs.Select_Rates_Value(funs.Select_vt_RateType(vtid));
                        Ratesapp = funs.Select_vt_RateType(vtid);
                    }
                }
                shiptoacc_id = funs.Select_ac_id(textBox14.Text);
                shiptoprint = funs.Select_Print(textBox14.Text);
                shiptoaddress1 = funs.Select_Address1(textBox14.Text);
                shiptoaddress2 = funs.Select_Address2(textBox14.Text);
                shiptocontact = funs.Select_Mobile(textBox14.Text);
                shiptoemail = funs.Select_Email(textBox14.Text);
                shiptotin = funs.Select_TIN(textBox14.Text);
                shiptoPan = funs.Select_PAN(textBox14.Text);
                shiptoAadhar = funs.Select_AAdhar(textBox14.Text);
                shiptostate = funs.Select_state_nm(funs.Select_ac_state_id(textBox14.Text).ToString());
                stateid = funs.Select_state_id(shiptostate);
                if (stateid == "")
                {
                    stateid = Database.CompanyState_id;
                }
                if (Database.CompanyState_id == stateid)
                {
                    gExState = false;
                }
                else
                {
                    gExState = true;
                }
                for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
                {
                    ItemSelected(false, i);
                }
                labelCalc();
            }
            if (e.Control && e.KeyCode == Keys.C)
            {
                textBox14.Text = funs.AddAccount();
                string accrateapp = Database.GetScalarText("Select Rateapp from Account where Name='" + textBox14.Text + "'");
                if (accrateapp != "")
                {
                    textBox27.Text = funs.Select_Rates_Value(accrateapp);
                    Ratesapp = accrateapp;
                }
                else
                {
                    textBox27.Text = funs.Select_Rates_Value(funs.Select_vt_RateType(vtid));
                    Ratesapp = funs.Select_vt_RateType(vtid);
                }
                shiptoprint = funs.Select_Print(textBox14.Text);
                shiptoaddress1 = funs.Select_Address1(textBox14.Text);
                shiptoaddress2 = funs.Select_Address2(textBox14.Text);
                shiptocontact = funs.Select_Mobile(textBox14.Text);
                shiptoemail = funs.Select_Email(textBox14.Text);
                shiptotin = funs.Select_TIN(textBox14.Text);
                shiptoPan = funs.Select_PAN(textBox14.Text);
                shiptoAadhar = funs.Select_AAdhar(textBox14.Text);
                shiptostate = funs.Select_state_nm(funs.Select_ac_state_id(textBox14.Text).ToString());
                stateid = funs.Select_state_id(shiptostate);
                if (stateid == "")
                {
                    stateid = Database.CompanyState_id;
                }
                if (Database.CompanyState_id == stateid)
                {
                    gExState = false;
                }
                else
                {
                    gExState = true;
                }
                for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
                {
                    ItemSelected(false, i);
                }
                labelCalc();
            }
            shiptoacc_id = funs.Select_ac_id(textBox14.Text);
            shiptoprint = funs.Select_Print(textBox14.Text);
            shiptoaddress1 = funs.Select_Address1(textBox14.Text);
            shiptoaddress2 = funs.Select_Address2(textBox14.Text);
            shiptocontact = funs.Select_Mobile(textBox14.Text);
            shiptoemail = funs.Select_Email(textBox14.Text);
            shiptotin = funs.Select_TIN(textBox14.Text);
            shiptoPan = funs.Select_PAN(textBox14.Text);
            shiptoAadhar = funs.Select_AAdhar(textBox14.Text);
            shiptostate = funs.Select_state_nm(funs.Select_ac_state_id(textBox14.Text).ToString());
            stateid = funs.Select_ac_state_id(textBox14.Text).ToString();
            if (stateid == "")
            {
                stateid = Database.CompanyState_id;
            }
            if (Database.CompanyState_id == stateid)
            {
                gExState = false;
            }
            else
            {
                gExState = true;
            }
            for (int i = 0; i < ansGridView1.RowCount - 1; i++)
            {
                if (textBox14.Text != "")
                {
                    ItemSelected(false, i);
                }
            }
            labelCalc();
        }

        private void CopyClipboard()
        {
            string SrtClip = "";
            for (int i = 0; i < ansGridView1.Rows.Count; i++)
            {
                if (ansGridView1["orgdesc", i].Value != null)
                {
                    SrtClip = SrtClip + "Faspi" + '\t';
                    SrtClip = SrtClip + ansGridView1["sno", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["unt", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["orgdesc", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["description", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["Quantity", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["Rate_am", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["qd", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["cd", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["Des_ac_id", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["Category_Id", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["comqty", i].Value.ToString() + '\t';
                    ansGridView1.Rows[i].Cells["Category"].Value = funs.Select_tax_cat_nm(ansGridView1["Category_Id", i].Value.ToString());
                    SrtClip = SrtClip + ansGridView1["Category", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["Taxabelamount", i].Value.ToString() + '\t';
                    if (ansGridView1["Batch_Code", i].Value == null)
                    {
                        ansGridView1["Batch_Code", i].Value = "";
                    }
                    SrtClip = SrtClip + ansGridView1["Batch_Code", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["Commission_per", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["CommissionFix", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["orgpack", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["pvalue", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["rate_unit", i].Value.ToString() + '\t';
                    if (ansGridView1["remark1", i].Value == null)
                    {
                        ansGridView1["remark1", i].Value = "";
                    }
                    if (ansGridView1["remark2", i].Value == null)
                    {
                        ansGridView1["remark2", i].Value = "";
                    }
                    if (ansGridView1["remark3", i].Value == null)
                    {
                        ansGridView1["remark3", i].Value = "";
                    }
                    if (ansGridView1["remark4", i].Value == null)
                    {
                        ansGridView1["remark4", i].Value = "";
                    }
                    SrtClip = SrtClip + ansGridView1["remark1", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["remark2", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["remark3", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["remark4", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["remarkreq", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["flatdis", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["godown_id", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["cost", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["MRP", i].Value.ToString() + '\t';
                    if (ansGridView1["weight", i].Value == null)
                    {
                        ansGridView1["weight", i].Value = 0;
                    }
                    SrtClip = SrtClip + ansGridView1["weight", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["pur_sale_acc", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["tax1", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["tax2", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["tax3", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["tax4", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["rate1", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["rate2", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["rate3", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["rate4", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["taxamt1", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["taxamt2", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["taxamt3", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["taxamt4", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["bottomdis", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["Amount0", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["QDType", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["QDAmount", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["Amount1", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["CDType", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["CDAmount", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["Amount2", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["FDType", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["FDAmount", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["Amount3", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["GridDis", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["TotalDis", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["Amount4", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["TotTaxPer", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["TotTaxAmount", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["ExpAmount", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["Amount5", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + ansGridView1["Amount", i].Value.ToString() + '\t';
                    SrtClip = SrtClip + '\n';
                }
            }

            if (SrtClip != "")
            {
                Clipboard.SetText(SrtClip);
            }
        }

        private void PasteClipboard()
        {
            try
            {
                string s = Clipboard.GetText();
                if (s.Length < 5 || s.Substring(0, 5) != "Faspi")
                {
                    return;
                }
                s = Clipboard.GetText().Replace("\r", " ");
                string[] lines = s.Split('\n');
                int row = 0;
                int col = 0;
                int linesCount = lines.Count();

                foreach (string line in lines)
                {
                    if (line.Length > 0)
                    {
                        string[] cells = line.Split('\t');
                        ansGridView1.Rows.Add();
                        ansGridView1.Rows[row].Cells["sno"].Value = cells[1];
                        ansGridView1.Rows[row].Cells["unt"].Value = cells[2];
                        ansGridView1.Rows[row].Cells["orgdesc"].Value = cells[3];
                        ansGridView1.Rows[row].Cells["description"].Value = cells[4];
                        ansGridView1.Rows[row].Cells["Quantity"].Value = cells[5];
                        ansGridView1.Rows[row].Cells["Rate_am"].Value = cells[6];
                        ansGridView1.CurrentCell = ansGridView1[0, row];
                        ansGridView1.Rows[row].Cells["qd"].Value = cells[7];
                        ansGridView1.Rows[row].Cells["cd"].Value = cells[8];
                        ansGridView1.Rows[row].Cells["Des_ac_id"].Value = cells[9];
                        ansGridView1.Rows[row].Cells["Category_Id"].Value = cells[10];
                        ansGridView1.Rows[row].Cells["comqty"].Value = cells[11];
                        ansGridView1.Rows[row].Cells["Category"].Value = cells[12];
                        ansGridView1.Rows[row].Cells["Taxabelamount"].Value = cells[13];
                        ansGridView1.Rows[row].Cells["Batch_Code"].Value = cells[14];
                        ansGridView1.Rows[row].Cells["Commission_per"].Value = cells[15];
                        ansGridView1.Rows[row].Cells["CommissionFix"].Value = cells[16];
                        ansGridView1.Rows[row].Cells["orgpack"].Value = cells[17];
                        ansGridView1.Rows[row].Cells["pvalue"].Value = cells[18];
                        ansGridView1.Rows[row].Cells["rate_unit"].Value = cells[19];
                        ansGridView1.Rows[row].Cells["remark1"].Value = cells[20];
                        ansGridView1.Rows[row].Cells["remark2"].Value = cells[21];
                        ansGridView1.Rows[row].Cells["remark3"].Value = cells[22];
                        ansGridView1.Rows[row].Cells["remark4"].Value = cells[23];
                        ansGridView1.Rows[row].Cells["remarkreq"].Value = cells[24];
                        ansGridView1.Rows[row].Cells["flatdis"].Value = cells[25];
                        ansGridView1.Rows[row].Cells["godown_id"].Value = cells[26];
                        ansGridView1.Rows[row].Cells["cost"].Value = cells[27];
                        ansGridView1.Rows[row].Cells["MRP"].Value = cells[28];
                        ansGridView1.Rows[row].Cells["weight"].Value = cells[29];
                        ansGridView1.Rows[row].Cells["pur_sale_acc"].Value = cells[30];
                        ansGridView1.Rows[row].Cells["tax1"].Value = cells[31];
                        ansGridView1.Rows[row].Cells["tax2"].Value = cells[32];
                        ansGridView1.Rows[row].Cells["tax3"].Value = cells[33];
                        ansGridView1.Rows[row].Cells["tax4"].Value = cells[34];
                        ansGridView1.Rows[row].Cells["rate1"].Value = cells[35];
                        ansGridView1.Rows[row].Cells["rate2"].Value = cells[36];
                        ansGridView1.Rows[row].Cells["rate3"].Value = cells[37];
                        ansGridView1.Rows[row].Cells["rate4"].Value = cells[38];
                        ansGridView1.Rows[row].Cells["taxamt1"].Value = cells[39];
                        ansGridView1.Rows[row].Cells["taxamt2"].Value = cells[40];
                        ansGridView1.Rows[row].Cells["taxamt3"].Value = cells[41];
                        ansGridView1.Rows[row].Cells["taxamt4"].Value = cells[42];
                        ansGridView1.Rows[row].Cells["bottomdis"].Value = cells[43];
                        ansGridView1.Rows[row].Cells["Amount0"].Value = cells[44];
                        ansGridView1.Rows[row].Cells["QDType"].Value = cells[45];
                        ansGridView1.Rows[row].Cells["QDAmount"].Value = cells[46];
                        ansGridView1.Rows[row].Cells["Amount1"].Value = cells[47];
                        ansGridView1.Rows[row].Cells["CDType"].Value = cells[48];
                        ansGridView1.Rows[row].Cells["CDAmount"].Value = cells[49];
                        ansGridView1.Rows[row].Cells["Amount2"].Value = cells[50];
                        ansGridView1.Rows[row].Cells["FDType"].Value = cells[51];
                        ansGridView1.Rows[row].Cells["FDAmount"].Value = cells[52];
                        ansGridView1.Rows[row].Cells["Amount3"].Value = cells[53];
                        ansGridView1.Rows[row].Cells["GridDis"].Value = cells[54];
                        ansGridView1.Rows[row].Cells["TotalDis"].Value = cells[55];
                        ansGridView1.Rows[row].Cells["Amount4"].Value = cells[56];
                        ansGridView1.Rows[row].Cells["TotTaxPer"].Value = cells[57];
                        ansGridView1.Rows[row].Cells["TotTaxAmount"].Value = cells[58];
                        ansGridView1.Rows[row].Cells["ExpAmount"].Value = cells[59];
                        ansGridView1.Rows[row].Cells["Amount"].Value = cells[60];
                        ItemSelectedPaste(row);
                        ItemCalc(row);
                        row++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            catch (FormatException)
            {
                MessageBox.Show("The data you pasted is in the wrong format for the cell");
                return;
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyClipboard();
        }

        private void pasteCtrlVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasteClipboard();
        }

        private void ansGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int currentMouseOverRow = ansGridView1.HitTest(e.X, e.Y).RowIndex;
                contextMenuStrip1.Show(ansGridView1, new Point(e.X, e.Y));
            }
        }

        private void ansGridView4_KeyDown(object sender, KeyEventArgs e)
        {
            if (ansGridView4.CurrentCell == null)
            {
                return;
            }
            if (ansGridView4.CurrentCell.OwningColumn.Name == "CamountB")
            {
                if (ansGridView4.CurrentRow.Index == ansGridView4.Rows.Count - 1 && ansGridView4.Rows[ansGridView4.CurrentRow.Index].Cells["CamountB"].Value == null)
                {
                    SendKeys.Send("{tab}");
                }
            }
            if (e.KeyCode == Keys.Delete)
            {
                if (ansGridView4.CurrentRow.Index == ansGridView4.Rows.Count - 1)
                {
                    for (int i = 1; i < ansGridView4.Columns.Count; i++)
                    {
                        ansGridView4.Rows[ansGridView4.CurrentRow.Index].Cells[i].Value = "";
                        labelCalc();
                    }
                }
                else
                {
                    ansGridView4.Rows.RemoveAt(ansGridView4.CurrentRow.Index);
                    for (int i = 0; i < ansGridView4.Rows.Count; i++)
                    {
                        ansGridView4.Rows[i].Cells["sno3"].Value = (i + 1);
                    }
                    labelCalc();
                }
            }
            if (ansGridView4.CurrentCell.OwningColumn.Name == "Charg_Name2")
            {
            }
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (ansGridView4.CurrentCell.Value.ToString() != "")
                {
                    ansGridView4.CurrentCell.Value = funs.EditCharge(ansGridView4.CurrentCell.Value.ToString());
                    ansGridView4.CurrentRow.Cells["Charg_id2"].Value = funs.Select_ch_id(ansGridView4.CurrentCell.Value.ToString());
                    ansGridView4.CurrentRow.Cells["Changed2"].Value = false;
                }
            }
            if (e.Control && e.KeyCode == Keys.C)
            {
                ansGridView4.CurrentCell.Value = funs.AddCharge();
                ansGridView4.CurrentRow.Cells["Charg_id2"].Value = funs.Select_ch_id(ansGridView4.CurrentCell.Value.ToString());
                ansGridView4.CurrentRow.Cells["Changed2"].Value = false;
            }
        }

        private void ansGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (ansGridView1.CurrentRow.Cells.Count > 0)
            {
                bool found = false;
                foreach (DataGridViewCell cell in ansGridView1.CurrentRow.Cells)
                {
                    if (cell.Value == null)
                    {
                        found = true;
                        break;
                    }
                }
                if (found)
                {
                    MessageBox.Show("Null");
                }
                else
                {
                    Search frm = new Search();
                    frm.ShowDialog(this);
                    ansGridView1.CurrentCell.OwningRow.Cells["description"].Value += frm.outStr;
                }
            }
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void getDescid(String dsc, String un)
        {
            DataTable dtRateComm = new DataTable();
            dtRateComm.Clear();
            Database.GetSqlData("select retail,[Commission%],[Commission@],Des_id,Tax_Cat_id,Purchase_rate,Wholesale from description where description='" + dsc + "' and Pack='" + un + "'", dtRateComm);
            if (dtRateComm.Rows.Count > 0)
            {
                desc_id = dtRateComm.Rows[0]["Des_id"].ToString();
            }
        }

        private void textBox9_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox9.Text != "")
                {
                    RoffChanged = true;
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                    this.Activate();
                    labelCalc();
                }
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker3_KeyDown(object sender, KeyEventArgs e)
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

        private void textBox12_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox11_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
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

        private void textBox15_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox15);
        }

        private void SetVno()
        {
            int numtype = funs.Select_NumType(vtid);
            int tempvno;
            if (numtype == 3 && vno != 0 && vid != "")
            {
                DateTime dt1 = dateTimePicker1.Value;
                DateTime dt2 = DateTime.Parse(Database.GetScalarDate("select vdate from voucherinfo where vi_id='" + vid + "' "));
                if (dt1 != dt2)
                {
                    tempvno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
                    label10.Text = tempvno.ToString();
                    vno = tempvno;
                }
                return;
            }
            if (vtid == "" || vtid == null || (vno != 0 && vid != "") || f12used == true)
            {
                return;
            }

            tempvno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
            label10.Text = tempvno.ToString();
        }

        private void AccGpLimit()
        {
            if (Feature.Available("Group Credit Limits") == "No")
            {
                locked = false;
                label27.Visible = false;
                SideFill();
                return;
            }

            String str = "";
            string Loc_id = SelectGroupid(textBox14.Text);
            if (Loc_id == "")
            {
                return;
            }
            DataTable dtGpAmt = new DataTable();
            str = "SELECT qur.Loc_id, Sum(qur.Dr) AS SDr, Sum(qur.Cr) AS SCr FROM (SELECT ACCOUNT.Loc_id, JOURNAL.Vi_id, JOURNAL.Ac_id, " + access_sql.fnstring("JOURNAL.Amount>0", "JOURNAL.Amount", "0") + " AS Dr, " + access_sql.fnstring("JOURNAL.Amount<0", "-1*(JOURNAL.Amount)", "0") + " AS Cr  FROM JOURNAL INNER JOIN ACCOUNT ON JOURNAL.Ac_id = ACCOUNT.Ac_id Where (((ACCOUNT.Loc_id) = '" + Loc_id + "')) UNION ALL SELECT ACCOUNT.Loc_id,0 as Vi_id, ACCOUNT.Ac_id, " + access_sql.fnstring("ACCOUNT.Balance>0", "ACCOUNT.Balance", "0") + " AS Dr, " + access_sql.fnstring("ACCOUNT.Balance<0", "-1*(ACCOUNT.Balance)", "0") + " AS Cr FROM ACCOUNT where (loc_id='" + Loc_id + "'))  AS qur GROUP BY qur.Loc_id";
            Database.GetSqlData(str, dtGpAmt);
            double gpAmt = 0;
            if (dtGpAmt.Rows.Count > 0)
            {
                gpAmt = double.Parse(dtGpAmt.Rows[0]["SDr"].ToString()) - double.Parse(dtGpAmt.Rows[0]["SCr"].ToString());
            }

            DataTable dtGpLmt = new DataTable();
            str = "SELECT OTHER.Blimit, OTHER.Dlimit FROM ACCOUNT LEFT JOIN OTHER ON ACCOUNT.Loc_id = OTHER.Oth_id WHERE ACCOUNT.Ac_id='" + funs.Select_ac_id(textBox14.Text) + "' ";
            Database.GetSqlData(str, dtGpLmt);
            double gpLmt = 0;
            if (dtGpLmt.Rows.Count > 0)
            {
                if (dtGpLmt.Rows[0]["Blimit"].ToString() != "")
                {
                    gpLmt = double.Parse(dtGpLmt.Rows[0]["Blimit"].ToString());
                }
            }

            int count = Database.GetScalarInt("select count(*) from account where Act_id='SER3' and name='" + textBox14.Text + "'");

            if (gpAmt > gpLmt && gtype == "Sale" && count == 0)
            {
                locked = true;
                label27.Visible = true;
                SideFill();
                label27.Text = "Group Credit limit Exceed. Account is locked";
            }
            else
            {

            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            SetVno();
            textBox1.Text = funs.accbal(funs.Select_ac_id(textBox14.Text), dateTimePicker1.Value);
            SetDuedate(textBox14.Text);
        }

        private void textBox15_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox15);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox3);
        }

        private void textBox9_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox9);
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox3);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        public void LoadData(string vi_id, String type, Boolean tax, Boolean ExState, Boolean Unregistered)
        {            
            gtype = type;
            
            Transdocdate = dateTimePicker1.Value;
            if (gtype == "Opening")
            {
                //  DateTime olddate = dateTimePicker1.Value.AddDays(-1);
                dateTimePicker1.Value = Database.stDate.AddDays(-1);
                dateTimePicker1.MinDate = dateTimePicker1.Value.AddDays(-1);
                dateTimePicker1.MaxDate = dateTimePicker1.Value.AddDays(-1);
                dateTimePicker1.CustomFormat = Database.dformat;

                dateTimePicker2.Value = Database.stDate.AddDays(-1);
                dateTimePicker2.CustomFormat = Database.dformat;

                dateTimePicker3.Value = Database.stDate.AddDays(-1);
                dateTimePicker3.CustomFormat = Database.dformat;
            }
            else
            {

                //dateTimePicker1.Value = Database.ldate;
                dateTimePicker1.MinDate = Database.stDate;
                dateTimePicker1.MaxDate = Database.ldate;
                dateTimePicker1.CustomFormat = Database.dformat;
                //dateTimePicker2.Value = Database.ldate;
                dateTimePicker2.CustomFormat = Database.dformat;
                //dateTimePicker3.Value = Database.ldate;
                dateTimePicker3.CustomFormat = Database.dformat;

            }
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Height = 344;
            flowLayoutPanel1.Controls.Clear();
            for (int i = 0; i < Master.TransportDetails.Rows.Count; i++)
            {
                Label lbl = new Label();
                TextBox txtbox = new TextBox();
                txtbox.Name = Master.TransportDetails.Rows[i]["FName"].ToString();
                lbl.Text = Master.TransportDetails.Rows[i]["ShowingName"].ToString();
                if (Master.TransportDetails.Rows[i]["status"].ToString() == "Outside")
                {
                    lbl.Visible = true;
                    txtbox.Visible = true;
                }
                else
                {
                    lbl.Visible = false;
                    txtbox.Visible = false;
                }
                flowLayoutPanel1.Controls.Add(lbl);
                flowLayoutPanel1.Controls.Add(txtbox);
            }
            gStr = vi_id;
            vid = vi_id;
            gtype = type;
            gExcludingTax = tax;
            gExState = ExState;
            gUnregistered = Unregistered;
            

            if ((type == "Purchase" || type == "P Return") && ExState == true)
            {
                SubCategory_Name = "Central Purchase";
            }
            else if ((type == "Purchase" || type == "P Return") && ExState == false)
            {
                SubCategory_Name = "Local Purchase";
            }
            else if ((type == "Sale" || type == "Return") && ExState == true)
            {
                SubCategory_Name = "Central Sale";
            }
            else if ((type == "Sale" || type == "Return" || type == "Pending") && ExState == false)
            {
                SubCategory_Name = "Local Sale";
            }
            else if (type == "receive")
            {
                SubCategory_Name = "Local Purchase";
            }
            else if (type == "issue")
            {
                SubCategory_Name = "Local Sale";
            }
            else if (type == "Transfer")
            {
                SubCategory_Name = "Local Purchase";
            }
            if (type == "Sale" && ExState == true)
            {
                gFrmCaption = "Sale Ex-State";
            }
            else if (type == "Sale" && tax == true)
            {
                gFrmCaption = "Sale Excluding Tax";
            }
            else if (type == "Sale" && tax == false)
            {
                gFrmCaption = "Sale Including Tax";
            }
            else if (type == "Return" && ExState == true)
            {
                gFrmCaption = "Sale Return Ex-State";
            }
            else if (type == "Return" && tax == false)
            {
                gFrmCaption = "Sale Return Including Tax";
            }
            else if (type == "Opening")
            {
                gFrmCaption = "Opening";
            }
            else if (type == "Return" && tax == true)
            {
                gFrmCaption = "Sale Return Excluding Tax";
            }
            else if (type == "Purchase" && Unregistered == true)
            {
                gFrmCaption = "Purchase UnRegistered";
            }
            else if (type == "Purchase" && ExState == true)
            {
                gFrmCaption = "Purchase Ex-State";
            }
            else if (type == "PWDebitNote")
            {
                gFrmCaption = "Debit Note With GST";
            }
            else if (type == "Purchase" && ExState == false)
            {
                gFrmCaption = "Purchase On-State";
            }
            else if (type == "P Return" && Unregistered == true)
            {
                gFrmCaption = "Purchase Return UnRegistered";
            }
            else if (type == "P Return" && ExState == true)
            {
                gFrmCaption = "Purchase Return Ex-State";
            }
            else if (type == "P Return" && ExState == false)
            {
                gFrmCaption = "Purchase Return On-State";
            }
            else if (type == "Pending")
            {
                gFrmCaption = "Pendings";
            }
            else if (type == "receive")
            {
                gFrmCaption = "Stock receive";
            }
            else if (type == "issue")
            {
                gFrmCaption = "Stock issue";
            }
            else if (type == "Transfer")
            {
                gFrmCaption = "Godown Transfer";
            }
            this.Text = gFrmCaption;


            Displaysetting();
            DisplayData(vi_id);

            SideFill();
            SetVno();
            if (Feature.Available("Taxation Applicable") == "VAT")
            {
                label6.Text = "VAT";
                label13.Text = "SAT";
                label14.Text = "CST";
                label15.Text = "Service Tax";
            }
            else
            {
                label6.Text = "CGST";
                label13.Text = "SGST";
                label14.Text = "IGST";
                label15.Text = "Cess";
            }

            if (gresave == true)
            {
                object sender = new object();
                EventArgs e = new EventArgs();
                btn_Click(sender, e);
            }
        }

        private void setrebate()
        {

            string acid = funs.Select_ac_id(textBox14.Text);
            DataTable dtrebate = new DataTable();
            Database.GetSqlData("Select * from rebate where Acid='' or acid='" + acid + "'", dtrebate);


            if (Feature.Available("Company Colour").ToUpper() == "YES")
            {

                for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
                {

                    DataTable dtcompany = new DataTable();
                    Database.GetSqlData("SELECT     Company_id, Item_id FROM  Description WHERE DESCRIPTION.Des_id='" + ansGridView1.Rows[i].Cells["des_ac_id"].Value.ToString() + "'", dtcompany);
                    if (dtrebate.Select("Acid='" + acid + "' and Companyid='" + dtcompany.Rows[0]["Company_id"].ToString() + "' and  Itemid='" + dtcompany.Rows[0]["item_id"].ToString() + "'", "").Length > 0)
                    {
                        DataTable dtdis = dtrebate.Select("Acid='" + acid + "' and  Companyid='" + dtcompany.Rows[0]["Company_id"].ToString() + "' and  Itemid='" + dtcompany.Rows[0]["item_id"].ToString() + "'", "").CopyToDataTable();
                        if (dtdis.Rows.Count > 0)
                        {

                            ansGridView1.Rows[i].Cells["qd"].Value = double.Parse(dtdis.Rows[0]["dis1"].ToString());
                            ansGridView1.Rows[i].Cells["cd"].Value = double.Parse(dtdis.Rows[0]["dis2"].ToString());
                            ansGridView1.Rows[i].Cells["flatdis"].Value = double.Parse(dtdis.Rows[0]["dis3"].ToString());
                        }
                    }
                    else if (dtrebate.Select("Acid='" + acid + "' and Companyid='" + dtcompany.Rows[0]["Company_id"].ToString() + "' and  Itemid=''", "").Length > 0)
                    {
                        DataTable dtdis = dtrebate.Select("Acid='" + acid + "' and Companyid='" + dtcompany.Rows[0]["Company_id"].ToString() + "' and  Itemid=''", "").CopyToDataTable();
                        if (dtdis.Rows.Count > 0)
                        {

                            ansGridView1.Rows[i].Cells["qd"].Value = double.Parse(dtdis.Rows[0]["dis1"].ToString());
                            ansGridView1.Rows[i].Cells["cd"].Value = double.Parse(dtdis.Rows[0]["dis2"].ToString());
                            ansGridView1.Rows[i].Cells["flatdis"].Value = double.Parse(dtdis.Rows[0]["dis3"].ToString());
                        }
                    }





                    else if (dtrebate.Select("Acid='" + acid + "' and Companyid='' and  Itemid='" + dtcompany.Rows[0]["item_id"].ToString() + "'", "").Length > 0)
                    {
                        DataTable dtdis = dtrebate.Select("Acid='" + acid + "' and Companyid='' and  Itemid='" + dtcompany.Rows[0]["item_id"].ToString() + "'", "").CopyToDataTable();
                        if (dtdis.Rows.Count > 0)
                        {

                            ansGridView1.Rows[i].Cells["qd"].Value = double.Parse(dtdis.Rows[0]["dis1"].ToString());
                            ansGridView1.Rows[i].Cells["cd"].Value = double.Parse(dtdis.Rows[0]["dis2"].ToString());
                            ansGridView1.Rows[i].Cells["flatdis"].Value = double.Parse(dtdis.Rows[0]["dis3"].ToString());
                        }
                    }
                    else if (dtrebate.Select("Acid='" + acid + "' and Companyid='' and  Itemid=''", "").Length > 0)
                    {
                        DataTable dtdis = dtrebate.Select("Acid='" + acid + "' and Companyid='' and  Itemid=''", "").CopyToDataTable();
                        if (dtdis.Rows.Count > 0)
                        {

                            ansGridView1.Rows[i].Cells["qd"].Value = double.Parse(dtdis.Rows[0]["dis1"].ToString());
                            ansGridView1.Rows[i].Cells["cd"].Value = double.Parse(dtdis.Rows[0]["dis2"].ToString());
                            ansGridView1.Rows[i].Cells["flatdis"].Value = double.Parse(dtdis.Rows[0]["dis3"].ToString());
                        }
                    }

                    else if (dtrebate.Select("Acid='' and Companyid='" + dtcompany.Rows[0]["Company_id"].ToString() + "' and  Itemid='" + dtcompany.Rows[0]["item_id"].ToString() + "'", "").Length > 0)
                    {
                        DataTable dtdis = dtrebate.Select("Acid='' and Companyid='" + dtcompany.Rows[0]["Company_id"].ToString() + "' and  Itemid='" + dtcompany.Rows[0]["item_id"].ToString() + "'", "").CopyToDataTable();
                        if (dtdis.Rows.Count > 0)
                        {

                            ansGridView1.Rows[i].Cells["qd"].Value = double.Parse(dtdis.Rows[0]["dis1"].ToString());
                            ansGridView1.Rows[i].Cells["cd"].Value = double.Parse(dtdis.Rows[0]["dis2"].ToString());
                            ansGridView1.Rows[i].Cells["flatdis"].Value = double.Parse(dtdis.Rows[0]["dis3"].ToString());
                        }
                    }




                    else if (dtrebate.Select("Acid='' and Companyid='' and  Itemid='" + dtcompany.Rows[0]["item_id"].ToString() + "'", "").Length > 0)
                    {
                        DataTable dtdis = dtrebate.Select("Acid='' and Companyid='' and  Itemid='" + dtcompany.Rows[0]["item_id"].ToString() + "'", "").CopyToDataTable();
                        if (dtdis.Rows.Count > 0)
                        {

                            ansGridView1.Rows[i].Cells["qd"].Value = double.Parse(dtdis.Rows[0]["dis1"].ToString());
                            ansGridView1.Rows[i].Cells["cd"].Value = double.Parse(dtdis.Rows[0]["dis2"].ToString());
                            ansGridView1.Rows[i].Cells["flatdis"].Value = double.Parse(dtdis.Rows[0]["dis3"].ToString());
                        }
                    }


                    else if (dtrebate.Select("Acid='' and Companyid='' and  Itemid=''", "").Length > 0)
                    {
                        DataTable dtdis = dtrebate.Select("Acid='' and Companyid='' and  Itemid=''", "").CopyToDataTable();
                        if (dtdis.Rows.Count > 0)
                        {

                            ansGridView1.Rows[i].Cells["qd"].Value = double.Parse(dtdis.Rows[0]["dis1"].ToString());
                            ansGridView1.Rows[i].Cells["cd"].Value = double.Parse(dtdis.Rows[0]["dis2"].ToString());
                            ansGridView1.Rows[i].Cells["flatdis"].Value = double.Parse(dtdis.Rows[0]["dis3"].ToString());
                        }
                    }

                    ItemCalc(i);




                }



            }
        }


        private void setrebate(string acid, string companyid, string itemid)
        {

            DataTable dtrebate = new DataTable();
            Database.GetSqlData("select Srno, Acid, Companyid, Itemid, dis1, dis2, dis3 from(SELECT     1 AS srno, Acid, Companyid, Itemid, dis1, dis2, dis3 FROM         dbo.Rebate WHERE     (Acid = '" + acid + "') AND (Companyid = '" + companyid + "') AND (Itemid = '" + itemid + "') Union all SELECT     2 AS srno, Acid, Companyid, Itemid, dis1, dis2, dis3 FROM rebate WHERE     (Acid = '" + acid + "') AND (Companyid = '" + companyid + "') AND (Itemid = '') Union all SELECT     3 AS srno, Acid, Companyid, Itemid, dis1, dis2, dis3 FROM         dbo.Rebate WHERE     (Acid = '" + acid + "') AND (Companyid = '') AND (Itemid = '" + itemid + "') Union all SELECT     4 AS srno, Acid, Companyid, Itemid, dis1, dis2, dis3 FROM         dbo.Rebate WHERE     (Acid = '" + acid + "') AND (Companyid = '') AND (Itemid = '') Union all SELECT     5 AS srno, Acid, Companyid, Itemid, dis1, dis2, dis3 FROM         dbo.Rebate WHERE     (Acid = '') AND (Companyid = '" + companyid + "') AND (Itemid = '" + itemid + "') Union all SELECT     6 AS srno, Acid, Companyid, Itemid, dis1, dis2, dis3 FROM         dbo.Rebate WHERE     (Acid = '') AND (Companyid = '') AND (Itemid = '" + itemid + "') Union all SELECT     7 AS srno, Acid, Companyid, Itemid, dis1, dis2, dis3 FROM         dbo.Rebate WHERE     (Acid = '') AND (Companyid = '" + companyid + "') AND (Itemid = '')  Union all SELECT     8 AS srno, Acid, Companyid, Itemid, dis1, dis2, dis3 FROM         dbo.Rebate WHERE     (Acid = '') AND (Companyid = '') AND (Itemid = '') ) as res order by Srno", dtrebate);


            //if (dtrebate.Select("Acid='" + acid + "' or Companyid='" + companyid + "' or  Itemid='" + itemid + "'","Srno").Length > 0)
            //{
            //    DataTable dtdis = dtrebate.Select("Acid='" + acid + "' or Companyid='" + companyid + "' or  Itemid='" + itemid + "'", "Srno").CopyToDataTable();
            if (dtrebate.Rows.Count >= 1)
            {
                dis1 = double.Parse(dtrebate.Rows[0]["dis1"].ToString());
                dis2 = double.Parse(dtrebate.Rows[0]["dis2"].ToString());
                dis3 = double.Parse(dtrebate.Rows[0]["dis3"].ToString());
            }
            //  }
            else
            {
                dis1 = 0;
                dis2 = 0;
                dis3 = 0;
            }





        }




        private void Displaysetting()
        {
            foreach (DataGridViewColumn column in ansGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (DataGridViewColumn column in ansGridView3.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (DataGridViewColumn column in ansGridView4.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            SideFill();

            if (gtype == "RCM")
            {
                gFrmCaption = "RCM";
                checkBox1.Visible = false;
                checkBox2.Checked = true;
                checkBox4.Checked = true;
                groupBox8.Visible = true;
                groupBox15.Visible = false;

            }
            else
            {
                groupBox8.Visible = false;
                groupBox5.Visible = true;
            }
            if (gtype == "Sale")
            {
                gFrmCaption = "Sale";
            }
            if (gtype == "Sale Order")
            {
                gFrmCaption = "Sale Order";
            }
            else if (gtype == "Return")
            {
                gFrmCaption = "Sale Return";
            }
            else if (gtype == "Purchase" || gtype == "RCM" || gtype == "PWDebitNote")
            {
                gFrmCaption = "Purchase";
                groupBox3.Text = "Bill From";
            }
            else if (gtype == "P Return")
            {
                gFrmCaption = "Purchase Return";
                groupBox3.Text = "Bill From";
            }
            else if (gtype == "Pending")
            {
                gFrmCaption = "Pendings";
            }
            else if (gtype == "receive")
            {
                gFrmCaption = "Stock receive";
            }
            else if (gtype == "issue")
            {
                gFrmCaption = "Stock issue";
            }
            else if (gtype == "Transfer")
            {
                gFrmCaption = "Godown Transfer";
                checkBox1.Visible = false;
            }
            this.Text = gFrmCaption;

            if (Feature.Available("Required PaymentMode Form").ToUpper() == "YES")
            {
                groupBox20.Visible = false;
            }
            else
            {
                groupBox20.Visible = true;
            }
            if (Feature.Available("Square Feet/Square Meter").ToUpper() == "YES")
            {
                groupBox18.Visible = true;
            }
            else
            {
                groupBox18.Visible = false;
            }


            if (Feature.Available("Required Remark1") == "Yes")
            {
                ansGridView1.Columns["Remark1"].Visible = true;
            }
            if (Feature.Available("Required Remark2") == "Yes")
            {
                ansGridView1.Columns["Remark2"].Visible = true;
            }
            if (Feature.Available("Required Remark3") == "Yes")
            {
                ansGridView1.Columns["Remark3"].Visible = true;
            }
            if (Feature.Available("Required Remark4") == "Yes")
            {
                ansGridView1.Columns["Remark4"].Visible = true;
            }
            ansGridView1.Columns["sno"].DisplayIndex = 0;
            permission = funs.GetPermissionKey("Transactions");

            UsersFeature ob1 = permission.Where(w => w.FeatureName == "Action on ChangeRate").FirstOrDefault();

            if (ob1 != null && ob1.SelectedValue == "Do Not Allow")
            {
                ansGridView1.Columns["Rate_am"].ReadOnly = true;
                ansGridView1.Columns["Amount"].ReadOnly = true;
            }
            if (Feature.Available("Item Name before Packing") == "Yes")
            {
                ansGridView1.CurrentCell = ansGridView1.Rows[0].Cells[1];
                ansGridView1.Columns["description"].DisplayIndex = 1;
                ansGridView1.Columns["orgpack"].DisplayIndex = 2;
            }
            else
            {
                ansGridView1.Columns["orgpack"].DisplayIndex = 1;
                ansGridView1.Columns["description"].DisplayIndex = 2;
            }
            if (Feature.Available("Batch Number") == "Yes")
            {
                ansGridView1.Columns["Batch_Code"].Visible = true;
                ansGridView1.Columns["Batch_Code"].HeaderText = Feature.Available("Show Text on Batch Code");
                ansGridView1.Columns["Quantity"].DisplayIndex = 3;
                ansGridView1.Columns["Rate_am"].DisplayIndex = 4;
                ansGridView1.Columns["Amount"].DisplayIndex = 5;
            }
            else if (Feature.Available("Compute Quantity") == "Yes")
            {
                ansGridView1.Columns["comqty"].Visible = true;
                ansGridView1.Columns["comqty"].ReadOnly = false;
                ansGridView1.Columns["comqty"].DisplayIndex = 3;
                ansGridView1.Columns["Quantity"].DisplayIndex = 4;
                ansGridView1.Columns["Rate_am"].DisplayIndex = 5;
                ansGridView1.Columns["Amount"].DisplayIndex = 6;
            }
            else
            {
                ansGridView1.Columns["Quantity"].DisplayIndex = 3;
                ansGridView1.Columns["Rate_am"].DisplayIndex = 4;
                ansGridView1.Columns["Amount"].DisplayIndex = 5;
            }
            if (Feature.Available("MRP on Grid") == "Yes")
            {
                ansGridView1.Columns["description"].Width = 210;
                ansGridView1.Columns["MRP"].Visible = true;
                ansGridView1.Columns["MRP"].DisplayIndex = 5;

                if (Feature.Available("Discount Count") == "1")
                {
                    ansGridView1.Columns["qd"].DisplayIndex = 6;
                    ansGridView1.Columns["Amount"].DisplayIndex = 7;
                    ansGridView1.Columns["cd"].Visible = false;
                    ansGridView1.Columns["flatdis"].Visible = false;
                }
                else if (Feature.Available("Discount Count") == "2")
                {
                    ansGridView1.Columns["qd"].DisplayIndex = 6;
                    ansGridView1.Columns["cd"].DisplayIndex = 7;
                    ansGridView1.Columns["Amount"].DisplayIndex = 8;
                    ansGridView1.Columns["flatdis"].Visible = false;
                }
                else if (Feature.Available("Discount Count") == "3")
                {
                    ansGridView1.Columns["qd"].DisplayIndex = 6;
                    ansGridView1.Columns["cd"].DisplayIndex = 7;
                    ansGridView1.Columns["flatdis"].DisplayIndex = 8;
                    ansGridView1.Columns["Amount"].DisplayIndex = 9;
                }
                else
                {
                    ansGridView1.Columns["Amount"].DisplayIndex = 6;
                    ansGridView1.Columns["qd"].Visible = false;
                    ansGridView1.Columns["cd"].Visible = false;
                    ansGridView1.Columns["flatdis"].Visible = false;
                }
            }
            else
            {
                if (Feature.Available("Discount Count") == "1")
                {
                    ansGridView1.Columns["qd"].DisplayIndex = 5;
                    ansGridView1.Columns["Amount"].DisplayIndex = 6;
                    ansGridView1.Columns["cd"].Visible = false;
                    ansGridView1.Columns["flatdis"].Visible = false;
                }

                else if (Feature.Available("Discount Count") == "2")
                {
                    ansGridView1.Columns["qd"].DisplayIndex = 5;
                    ansGridView1.Columns["cd"].DisplayIndex = 6;
                    ansGridView1.Columns["Amount"].DisplayIndex = 7;
                    ansGridView1.Columns["flatdis"].Visible = false;
                }
                else if (Feature.Available("Discount Count") == "3")
                {
                    ansGridView1.Columns["qd"].DisplayIndex = 5;
                    ansGridView1.Columns["cd"].DisplayIndex = 6;
                    ansGridView1.Columns["flatdis"].DisplayIndex = 7;
                    ansGridView1.Columns["Amount"].DisplayIndex = 8;
                }
                else
                {
                    if (Feature.Available("Compute Quantity") == "Yes")
                    {
                        ansGridView1.Columns["Amount"].DisplayIndex = 7;
                    }
                    else
                    {
                        ansGridView1.Columns["Amount"].DisplayIndex = 7;
                    }

                    ansGridView1.Columns["qd"].Visible = false;
                    ansGridView1.Columns["cd"].Visible = false;
                    ansGridView1.Columns["flatdis"].Visible = false;
                }
            }

            if (gtype == "RCM")
            {
                ansGridView4.Visible = false;
            }
            else
            {
                ansGridView4.Visible = true;
            }

            if (Feature.Available("Multi-Godown") == "Yes")
            {
                ansGridView1.Columns["godown_id"].Visible = true;
            }
            else
            {
                ansGridView1.Columns["godown_id"].Visible = false;
            }




            if (gtype == "Opening")
            {
                ansGridView1.Columns["godown_id"].Visible = false;
                ansGridView3.Visible = false;
                ansGridView4.Visible = false;
                groupBox7.Visible = false;
                flowLayoutPanel1.Visible = false;
                groupBox9.Visible = false;
                label4.Visible = false;
                label17.Visible = false;
                label6.Visible = false;
                label13.Visible = false;
                label14.Visible = false;
                label15.Visible = false;
                label16.Visible = false;
                label19.Visible = false;
                textBox5.Visible = false;
                textBox6.Visible = false;
                textBox19.Visible = false;
                textBox21.Visible = false;
                textBox22.Visible = false;
                textBox23.Visible = false;
                textBox24.Visible = false;
                textBox25.Visible = false;
                textBox26.Visible = false;
                // groupBox20.Visible = false;
            }





            string taxname1 = "", taxname2 = "", taxname3 = "", taxname4 = "";
            if (Feature.Available("Taxation Applicable") == "VAT")
            {
                taxname1 = "VAT%";
                taxname2 = "SAT%";
                taxname3 = "CST%";
                taxname4 = "Service Tax%";
            }
            else
            {
                taxname1 = "CGST%";
                taxname2 = "SGST%";
                taxname3 = "IGST%";
                taxname4 = "Cess%";
            }

            if (Feature.Available("Tax Description on Grid") == "Yes")
            {
                ansGridView1.Columns["rate1"].Visible = true;
                ansGridView1.Columns["rate2"].Visible = true;
                ansGridView1.Columns["rate3"].Visible = true;
                ansGridView1.Columns["rate1"].HeaderText = taxname1;
                ansGridView1.Columns["rate2"].HeaderText = taxname2;
                ansGridView1.Columns["rate3"].HeaderText = taxname3;
            }
            ansGridView1.Columns["Quantity"].CellTemplate.ValueType = typeof(double);
            ansGridView1.Columns["Rate_am"].CellTemplate.ValueType = typeof(double);
            ansGridView1.Columns["qd"].CellTemplate.ValueType = typeof(double);
            ansGridView1.Columns["cd"].CellTemplate.ValueType = typeof(double);
            ansGridView1.Columns["Amount"].CellTemplate.ValueType = typeof(double);
            ansGridView3.Columns["AmountA"].CellTemplate.ValueType = typeof(double);
            ansGridView3.Columns["CamountA"].CellTemplate.ValueType = typeof(double);
            ansGridView4.Columns["AmountB"].CellTemplate.ValueType = typeof(double);
            ansGridView4.Columns["CamountB"].CellTemplate.ValueType = typeof(double);

            ansGridView1.Columns["Remark1"].HeaderText = Feature.Available("Show Text on Remark1");
            ansGridView1.Columns["Remark2"].HeaderText = Feature.Available("Show Text on Remark2");
            ansGridView1.Columns["Remark3"].HeaderText = Feature.Available("Show Text on Remark3");
            ansGridView1.Columns["Remark4"].HeaderText = Feature.Available("Show Text on Remark4");
            ansGridView1.Columns["qd"].HeaderText = Feature.Available("Show Text on Discount1");
            ansGridView1.Columns["cd"].HeaderText = Feature.Available("Show Text on Discount2");
            ansGridView1.Columns["flatdis"].HeaderText = Feature.Available("Show Text on Discount3");

            label27.Visible = false;
            SideFill();

            if (gtype == "Sale" || gtype == "Return" || gtype == "Pending" || gtype == "issue" || gtype == "Sale Order")
            {
                if (gExState == true)
                {
                    groupBox6.Visible = true;  //form no
                }
                else
                {
                    groupBox6.Visible = false;  //form no
                }
                groupBox16.Visible = true; // Agent
                groupBox19.Visible = true; // Salesman
                groupBox11.Visible = true; //Purchase Rate
                //  groupBox20.Visible = true;
                if (Feature.Available("Due Date") == "Yes")
                {
                    groupBox15.Visible = true; // Due Date
                }
                else
                {
                    groupBox15.Visible = false; // Due Date
                }
                if (Feature.Available("Show TaxCategory") == "Yes")
                {
                    ansGridView1.Columns["Category"].Visible = true;
                }
                else
                {
                    ansGridView1.Columns["Category"].Visible = false;
                }
                groupBox5.Visible = false;

                if (Feature.Available("Purchase Rate") == "No")
                {
                    groupBox11.Visible = false;
                }
            }
            else if (gtype == "Purchase" || gtype == "P Return" || gtype == "receive" || gtype == "PWDebitNote" || gtype == "Opening")
            {
                if (gExState == true)
                {
                    groupBox6.Visible = true;  //form no
                }
                else
                {
                    groupBox6.Visible = false;  //form no
                }
                //  groupBox20.Visible = false; // Rate option

                groupBox14.Visible = false; // Rate option
                groupBox15.Visible = true; // Due Date
                groupBox16.Visible = false; //agent name
                groupBox19.Visible = false; //salesman name
                textBox9.ReadOnly = false;  //round off               
                groupBox5.Visible = true;
                groupBox11.Visible = false; //Purchase Rate
                ansGridView1.Columns["Category"].Visible = false;
                if (Feature.Available("Show TaxCategory") == "Yes")
                {
                    ansGridView1.Columns["Category"].Visible = true;
                }
                else
                {
                    ansGridView1.Columns["Category"].Visible = false;
                }
            }

            if (gtype == "Sale Order")
            {
                groupBox5.Visible = true;
                groupBox5.Text = "PO Info";
                label2.Visible = false;
                dateTimePicker2.Visible = false;
                label1.Text = "PO No";

            }

            if (gtype == "Return")
            {
                groupBox5.Visible = true;
                groupBox5.Text = "Referrence Info";
                label2.Visible = true;
                label2.Text = "Date";
                dateTimePicker2.Visible = true;
                label1.Text = "No";

            }

            if (Feature.Available("Auto Roundoff") == "No")
            {
                textBox9.ReadOnly = true;
            }
            else
            {
                textBox9.ReadOnly = false;
            }

            if (Feature.Available("Discount After Tax") == "Yes")
            {
                string disname = Database.GetScalarText("Select taxname from DisAfterTax");
                ansGridView1.Columns["DAT"].Visible = true;
                label19.Visible = true;
                label19.Text = disname;
                textBox26.Visible = true;
            }
            if (gtype == "Opening")
            {
                dateTimePicker1.MaxDate = Database.stDate.AddDays(-1);
                dateTimePicker3.MaxDate = Database.stDate.AddDays(-1);
                dateTimePicker1.MinDate = Database.stDate.AddDays(-1);
                dateTimePicker3.MinDate = Database.stDate.AddDays(-1);
                groupBox5.Visible = false;
                dateTimePicker1.Enabled = false;
                dateTimePicker1.Value = Database.stDate.AddDays(-1);
            }
            textBox4.Text = "0.00";
            textBox5.Text = "0.00";
            textBox8.Text = "0.00";
            textBox9.Text = "0.00";
            textBox10.Text = "0.00";

            string cmbVouTyp2 = "", cmbVouTyp3 = "", cmbVouTyp4 = "";
            if (gExcludingTax == true)
            {
            }
            else
            {
            }
            if (Feature.Available("Common Bill Cash Memo") == "Yes")
            {
                cmbVouTyp3 = " and Name<>'Cash Memo' and Name<>'Cash Memo Return'";
            }
            //if (Database.IsKacha == false)
            //{
            cmbVouTyp4 = " and " + Database.BMode + "=" + access_sql.Singlequote + "true" + access_sql.Singlequote;
            //}
            //else
            //{
            //    cmbVouTyp4 = " and B=" + access_sql.Singlequote + "true" + access_sql.Singlequote;
            //}

            DataTable dtvt = new DataTable();
            cmbVouTyp = "select [name] from vouchertype where active=" + access_sql.Singlequote + "true" + access_sql.Singlequote + "  and type='" + gtype + "' " + cmbVouTyp4 + "  ";
            cmbVouTyp = cmbVouTyp + cmbVouTyp2 + cmbVouTyp3;
            Database.GetSqlData(cmbVouTyp, dtvt);

            if (dtvt.Rows.Count == 1)
            {
                textBox15.Text = dtvt.Rows[0]["name"].ToString();
                vtid = funs.Select_vt_id_vnm(textBox15.Text);
                textBox27.Text = funs.Select_Rates_Value(funs.Select_vt_RateType(vtid));

                if (textBox27.Text != "")
                {
                    Ratesapp = Master.DtRates.Select("RateValue='" + textBox27.Text + "'").FirstOrDefault()["RateId"].ToString();
                    for (int i = 0; i < ansGridView1.RowCount - 1; i++)
                    {
                        ItemSelected(true, i);
                        ItemCalc(i);
                    }
                    labelCalc();
                }

                //   Ratesapp=
                gtaxinvoice = funs.Select_vt_taxinvoice(vtid);
                textBox15.Enabled = false;

                if (textBox15.Text == "")
                {
                    return;
                }
                vtid = funs.Select_vt_id_vnm(textBox15.Text);
                gtaxinvoice = funs.Select_vt_taxinvoice(vtid);
                gExState = funs.Select_vt_Exstate(vtid);
                gUnregistered = funs.Select_vt_Unregistered(vtid);
                gExcludingTax = funs.Select_vt_Excludungtax(vtid);
                gCalculationType = funs.Select_vt_CalculationType(vtid);

                if (gtype == "Sale" && gExState == true)
                {
                    DialogResult chk = MessageBox.Show("Is Company Provide Form-C?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (chk == DialogResult.Yes)
                    {
                        formC = true;
                    }
                    else
                    {
                        formC = false;
                    }
                }
                else
                {
                    formC = false;
                }
                if ((gtype == "Purchase" || gtype == "P Return") && gExState == true)
                {
                    SubCategory_Name = "Central Purchase";
                }
                else if ((gtype == "Purchase" || gtype == "P Return") && gExState == false)
                {
                    SubCategory_Name = "Local Purchase";
                }
                else if ((gtype == "Sale" || gtype == "Return") && gExState == true)
                {
                    SubCategory_Name = "Central Sale";
                }
                else if ((gtype == "Sale" || gtype == "Return" || gtype == "Pending") && gExState == false)
                {
                    SubCategory_Name = "Local Sale";
                }
                else if (gtype == "receive")
                {
                    SubCategory_Name = "Local Purchase";
                }
                else if (gtype == "issue")
                {
                    SubCategory_Name = "Local Sale";
                }
                else if (gtype == "Transfer")
                {
                    SubCategory_Name = "Local Purchase";
                }
                if (gCalculationType == "Including Tax Only")
                {
                    checkBox1.Enabled = false;
                    checkBox1.Checked = true;
                    gExcludingTax = false;
                }
                else if (gCalculationType == "Excluding Tax Only")
                {
                    checkBox1.Enabled = false;
                    checkBox1.Checked = false;
                    gExcludingTax = true;
                }
                else if (gCalculationType == "Default Excluding Tax")
                {
                    checkBox1.Enabled = true;
                    checkBox1.Checked = false;
                    gExcludingTax = true;
                }
                else if (gCalculationType == "Default Including Tax")
                {
                    checkBox1.Enabled = true;
                    checkBox1.Checked = true;
                    gExcludingTax = false;
                }
                if (vtid == "")
                {
                    ansGridView1.Enabled = false;
                }
                else
                {
                    ansGridView1.Enabled = true;
                }
            }
            else
            {
                textBox15.Enabled = true;
            }
            if (Feature.Available("Broker Wise Report") == "No")
            {
                groupBox16.Visible = false;
            }
            if (Database.SoftwareName == "Faspi Iron Pro.")
            {
                radioButton1.Visible = false;
                radioButton2.Visible = false;
                radioButton3.Visible = false;
                radioButton4.Visible = false;
                radioButton5.Visible = false;
                radioButton6.Visible = false;
                groupBox14.Text = "Rates";
                textBox12.Visible = true;
            }
        }

        private bool validate()
        {
            ansGridView1.EndEdit();
            ansGridView2.EndEdit();
            ansGridView3.EndEdit();
            ansGridView4.EndEdit();

            if (vtid == "")
            {
                MessageBox.Show("select Voucher type");
                textBox15.Focus();
                return false;
            }
            if (funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid) == 0 && vno == 0)
            {
                MessageBox.Show("Voucher Number can't be created on this date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (textBox14.Text == "")
            {
                if (gtype == "Transfer")
                {
                    MessageBox.Show("Select Godown From");
                }
                else
                {
                    MessageBox.Show("Select Header Account");
                }
                textBox14.Focus();
                return false;
            }
            if (gtype != "Opening")
            {
                if (funs.Select_ac_id(textBox14.Text) == "" || funs.Select_ac_id(textBox14.Text) == "")
                {
                    textBox14.BackColor = Color.Aqua;
                    textBox14.Focus();
                    MessageBox.Show("Enter Valid Account Name");
                    return false;
                }

            }
            if (gtype != "Transfer" && gtype != "Opening")
            {
                if ((double.Parse(textBox10.Text) == 0 && Feature.Available("Allow Zero Value Billing") == "No") == true)
                {
                    MessageBox.Show("Please enter some items");
                    ansGridView1.Focus();
                    return false;
                }
            }
            if (gtype == "Transfer")
            {
                if (textBox13.Text == "")
                {
                    MessageBox.Show("Select Godown To");
                    textBox13.Focus();
                    return false;
                }
            }


            if (gtype != "Opening")
            {
                if (funs.Select_ac_id(textBox14.Text) == "" || funs.Select_ac_id(textBox14.Text) == "")
                {
                    textBox14.BackColor = Color.Aqua;
                    textBox14.Focus();
                    MessageBox.Show("Enter Valid Account Name");
                    return false;
                }
            }
            if (Feature.Available("Allow Zero Value Billing") == "Yes" && ansGridView1.Rows.Count == 1)
            {
                MessageBox.Show("Please enter some items");
                ansGridView1.Focus();
                return false;
            }
            if (gresave == false)
            {
                if (locked == true)
                {
                    MessageBox.Show("Account is locked");
                    return false;
                }
            }
            if (gtype == "Purchase" || gtype == "P Return" || gtype == "receive" || gtype == "PWDebitNote")
            {
                if (textBox2.Text == "0" || textBox2.Text == "")
                {
                    MessageBox.Show("Enter Bill No.");
                    textBox2.Select();
                    textBox2.Focus();
                    return false;
                }
            }
            if (gtype != "Opening")
            {

                if (Feature.Available("Taxation Applicable") == "GST")
                {
                    if (textBox14.Text != "")
                    {
                        string state_id = "";

                        state_id = funs.Select_ac_state_id(textBox14.Text);
                        if (state_id == "")
                        {
                            MessageBox.Show("Please Select State with this Party.");
                            textBox14.Focus();
                            return false;
                        }
                    }
                    else
                    {
                        textBox14.BackColor = Color.Aqua;
                        textBox14.Focus();
                        return false;
                    }
                }
            }
            //if (gtype == "Sale" || gtype == "Return" || gtype == "Pending" || gtype == "issue" || gtype == "Sale Order")
            //{
            //    for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
            //    {
            //        string desid = ansGridView1.Rows[i].Cells["des_ac_id"].Value.ToString();
            //        double puramt = Database.GetScalarDecimal("Select purchase_rate from description where des_id='" + desid + "'");
            //        double rate = double.Parse(ansGridView1.Rows[i].Cells["rate_am"].Value.ToString());
            //        if (puramt > rate)
            //        {
            //            MessageBox.Show("Item Rate can't be less than Purchase Rate");
            //            ansGridView1.Rows[i].Cells["rate_am"].Style.BackColor = Color.Red;
            //            return false;
            //        }
            //    }
            //}

            if ((gtype == "Sale" || gtype == "Return" || gtype == "Pending" || gtype == "issue" || gtype == "Sale Order") && Feature.Available("Stock Warning On Sales Vouchers") == "Yes")
            {
                int posStockCnt = 0, NegativeStock = 0;
                double[] stk = new double[ansGridView1.Rows.Count - 1];
                for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
                {


                    if (ansGridView1.Rows[i].Cells["unt"].Value == null || ansGridView1.Rows[i].Cells["unt"].Value.ToString() == "")
                    {
                        MessageBox.Show("Enter Pack Value");
                        return false;
                    }
                    stk[i] = double.Parse(funs.Stock(ansGridView1.Rows[i].Cells["Des_ac_id"].Value.ToString()));


                }
                for (int i = 0; i < stk.Length; i++)
                {
                    if ((stk[i] - double.Parse(ansGridView1.Rows[i].Cells["Quantity"].Value.ToString())) < 0)
                    {
                        NegativeStock++;
                    }
                    else
                    {
                        posStockCnt++;
                    }
                }
                if (NegativeStock > 0 && Feature.Available("Stock Warning On Sales Vouchers") == "Yes")
                {
                    DialogResult res = MessageBox.Show("Negative stock..Want to save?", "Confirm", MessageBoxButtons.OKCancel);
                    if (res != DialogResult.OK)
                    {
                        return false;
                    }
                }
            }

            int numtype = funs.chkNumType(vtid);
            if (vid != "")
            {
            }
            else if (numtype != 1)
            {
                vid = Database.GetScalarText("Select Vi_id from voucherinfo where Vt_id='" + vtid + "' and Vnumber=" + vno + " and Vdate=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash + " and Voucherinfo.branch_id='" + Database.BranchId + "' ");
            }
            else
            {
                if (vid == "")
                {
                    string tempvid = "";
                    tempvid = Database.GetScalarText("Select Vi_id from voucherinfo where Vt_id='" + vtid + "' and Vnumber=" + vno + " and Voucherinfo.branch_id='" + Database.BranchId + "' ");
                    if (tempvid != "")
                    {
                        MessageBox.Show("Voucher can't be created on this No.");
                        return false;
                    }
                    else
                    {
                        vid = tempvid;
                    }
                }
            }
            if (Feature.Available("Required Remark1") == "Yes" && Feature.Available("Required Remark2") == "Yes")
            {
                if (gtype == "Sale" || gtype == "Return")
                {
                    for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
                    {
                        if (ansGridView1.Rows[i].Cells["remarkreq"].Value.ToString() == "false")
                        {
                            if (ansGridView1.Rows[i].Cells["remark1"].Value == null || ansGridView1.Rows[i].Cells["remark2"].Value == null || ansGridView1.Rows[i].Cells["remark1"].Value.ToString().Trim() == "" || ansGridView1.Rows[i].Cells["remark2"].Value.ToString().Trim() == "")
                            {
                                MessageBox.Show(Feature.Available("Show Text on Remark1") + " and " + Feature.Available("Show Text on Remark2") + " are must.");
                                return false;
                            }
                            else if (Database.GetScalarInt("SELECT count(*) FROM Voucherdet WHERE remark1='" + ansGridView1.Rows[i].Cells["remark1"].Value.ToString() + "' and Vi_id<>'" + vid + "' ") != 0)
                            {
                                MessageBox.Show(Feature.Available("Show Text on Remark1") + " must be unique");
                                return false;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < ansGridView4.Rows.Count - 1; i++)
            {
                if (ansGridView4.Rows[i].Cells["Accid2"].Value.ToString() == "" || ansGridView4.Rows[i].Cells["Accid2"].Value == null || ansGridView4.Rows[i].Cells["Accid2"].Value.ToString() == "")
                {
                    MessageBox.Show("Discounts which you have entered in Discount Grid are not proper.Please Pay attention.");
                    return false;
                }
            }
            if (gtype == "Purchase" || gtype == "P Return")
            {
                if (textBox2.Text.Trim() != "")
                {
                    int count = 0;
                    count = Database.GetScalarInt("Select count(*) from voucherinfo where Svnum='" + textBox2.Text + "' and Ac_id='" + funs.Select_ac_id(textBox14.Text) + "'");
                    if (count != 0)
                    {
                        if (vid == "")
                        {
                            DialogResult dr = MessageBox.Show("This Voucher is already Entered in this Software.Are You Sure You want to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                            if (dr == System.Windows.Forms.DialogResult.Yes)
                            {
                            }
                            else
                            {

                                textBox2.Focus();
                                return false;
                            }

                        }
                    }
                }

            }

            if ((gtype == "Sale" || gtype == "Return") && radioButton7.Checked == true)
            {
                string actid = funs.Select_AccTypeid(textBox14.Text);
                if (actid == "SER3")
                {

                    textBox14.Focus();

                    return false;
                }
            }

            string val0allowed = Database.GetScalarText("Select Allowed0Val from Vouchertype where Vt_id='" + vtid + "'");

            if (val0allowed == "Not Allowed")
            {
                for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
                {
                    if (double.Parse(ansGridView1.Rows[i].Cells["Amount"].Value.ToString()) == 0)
                    {
                        MessageBox.Show("0 Value Billing Not Allowed");
                        return false;
                    }

                }

            }


            return true;
        }

<<<<<<< HEAD
        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            if (textBox14.Text != "")
            {
                if (radioButton8.Checked == false)
                {
                    if (gtype == "Sale" || gtype == "Return")
                    {
                        checkLock();

                        AccGpLimit();
                    }
                }
                SideFill();
            }
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton8.Checked==false)
            {
                checkLock();
                AccGpLimit();
            }
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton8.Checked == true)
            {
                locked = false;
                label27.Visible = false;
                SideFill();
            }
        }

=======
>>>>>>> 112c82ae3816a7f8e7eb91f18a82ff39cf0bf5b2
        private void frmTransactionWithCDQD_Load(object sender, EventArgs e)
        {
        }

        private void PrintOnly(string voucherType, int vno)
        {
            DataTable QryVoucher = new DataTable("QryVoucher");
            DataTable Qryvoucherdes = new DataTable("Qryvoucherdes");
            DataTable QryVoucherTax = new DataTable("QryVoucherTax");

            Database.GetSqlData("select * from QryVoucher where Vid=''", QryVoucher);
            Database.GetSqlData("select * from Qryvoucherdes where Vid=''", Qryvoucherdes);
            Database.GetSqlData("select * from QryVoucherTax where Vid=''", QryVoucherTax);

            for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
            {
                QryVoucher.Rows.Add();
                QryVoucher.Rows[i]["Vi_id"] = 0;
                QryVoucher.Rows[i]["Vid"] = 0;
                QryVoucher.Rows[i]["Vnumber"] = vno;

                string prefix = "";
                string postfix = "";
                int padding = 0;

                prefix = Database.GetScalarText("Select prefix from Vouchertype where vt_id='" + funs.Select_vt_id_vnm(voucherType) + "' ");
                postfix = Database.GetScalarText("Select postfix from Vouchertype where vt_id='" + funs.Select_vt_id_vnm(voucherType) + "' ");
                padding = Database.GetScalarInt("Select padding from Vouchertype where vt_id='" + funs.Select_vt_id_vnm(voucherType) + "' ");

                string narr = SetNarr();
                SetVno();
                if (vno == 0)
                {
                    vno = funs.GenerateVno(vtid, dateTimePicker1.Value.ToString("dd-MMM-yyyy"), vid);
                }

                string invoiceno = vno.ToString();
                QryVoucher.Rows[i]["Invoiceno"] = prefix + invoiceno.PadLeft(padding, '0') + postfix;
                QryVoucher.Rows[i]["Vdate"] = dateTimePicker1.Value.Date;
                QryVoucher.Rows[i]["Duedate"] = dateTimePicker3.Value.Date;
                QryVoucher.Rows[i]["Acc.Name"] = textBox14.Text;
                QryVoucher.Rows[i]["Short"] = funs.Select_vt_short(textBox15.Text);

                if (funs.Select_Address1(textBox14.Text) != "None" || funs.Select_Address1(textBox14.Text) != "")
                {
                    QryVoucher.Rows[i]["Address1"] = funs.Select_Address1(textBox14.Text);
                }
                if (funs.Select_Address2(textBox14.Text) != "None" || funs.Select_Address2(textBox14.Text) != "")
                {
                    QryVoucher.Rows[i]["Address2"] = funs.Select_Address2(textBox14.Text);
                }
                if (funs.Select_Mobile(textBox14.Text) != "")
                {
                    QryVoucher.Rows[i]["Phone"] = funs.Select_Mobile(textBox14.Text);
                }
                if (funs.Select_TIN(textBox14.Text) != "")
                {
                    QryVoucher.Rows[i]["Tin_number"] = funs.Select_TIN(textBox14.Text);
                }

                QryVoucher.Rows[i]["Itemsr"] = ansGridView1.Rows[i].Cells["sno"].Value.ToString();
                QryVoucher.Rows[i]["Rate_am"] = ansGridView1.Rows[i].Cells["Rate_am"].Value.ToString();
                QryVoucher.Rows[i]["Quantity"] = ansGridView1.Rows[i].Cells["Quantity"].Value.ToString();
                QryVoucher.Rows[i]["Description"] = ansGridView1.Rows[i].Cells["description"].Value.ToString();
                //  QryVoucher.Rows[i]["Orgescription"] = ansGridView1.Rows[i].Cells["orgdesc"].Value.ToString();
                QryVoucher.Rows[i]["MRP"] = ansGridView1.Rows[i].Cells["MRP"].Value.ToString();
                QryVoucher.Rows[i]["Sname"] = funs.Select_state_nm(funs.Select_ac_state_id(textBox14.Text).ToString());
                QryVoucher.Rows[i]["GSTCode"] = funs.Select_state_GST(funs.Select_state_nm(funs.Select_ac_state_id(textBox14.Text).ToString()));
                QryVoucher.Rows[i]["TaxSlab"] = ansGridView1.Rows[i].Cells["TotTaxPer"].Value.ToString();
                QryVoucher.Rows[i]["TaxRate1"] = ansGridView1.Rows[i].Cells["rate1"].Value.ToString();
                QryVoucher.Rows[i]["TaxRate2"] = ansGridView1.Rows[i].Cells["rate2"].Value.ToString();
                QryVoucher.Rows[i]["TaxRate3"] = ansGridView1.Rows[i].Cells["rate3"].Value.ToString();
                QryVoucher.Rows[i]["TaxRate4"] = ansGridView1.Rows[i].Cells["rate4"].Value.ToString();
                QryVoucher.Rows[i]["taxamt1"] = ansGridView1.Rows[i].Cells["taxamt1"].Value.ToString();
                QryVoucher.Rows[i]["taxamt2"] = ansGridView1.Rows[i].Cells["taxamt2"].Value.ToString();
                QryVoucher.Rows[i]["taxamt3"] = ansGridView1.Rows[i].Cells["taxamt3"].Value.ToString();
                QryVoucher.Rows[i]["taxamt4"] = ansGridView1.Rows[i].Cells["taxamt4"].Value.ToString();
                QryVoucher.Rows[i]["comqty"] = ansGridView1.Rows[i].Cells["comqty"].Value.ToString();

                if (ansGridView1.Rows[i].Cells["remark1"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["remark1"].Value = "";
                }



                QryVoucher.Rows[i]["remark1"] = ansGridView1.Rows[i].Cells["remark1"].Value.ToString();
                if (ansGridView1.Rows[i].Cells["remark2"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["remark2"].Value = "";
                }
                QryVoucher.Rows[i]["remark2"] = ansGridView1.Rows[i].Cells["remark2"].Value.ToString();
                if (ansGridView1.Rows[i].Cells["remark3"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["remark3"].Value = "";
                }
                QryVoucher.Rows[i]["remark3"] = ansGridView1.Rows[i].Cells["remark3"].Value.ToString();
                if (ansGridView1.Rows[i].Cells["remark4"].Value == null)
                {
                    ansGridView1.Rows[i].Cells["remark4"].Value = "";
                }
                QryVoucher.Rows[i]["remark4"] = ansGridView1.Rows[i].Cells["remark4"].Value.ToString();

                TextBox tbx1 = this.Controls.Find(Master.TransportDetails.Rows[0]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                field1 = tbx1.Text;
                QryVoucher.Rows[i]["Transport1"] = field1;

                TextBox tbx2 = this.Controls.Find(Master.TransportDetails.Rows[1]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                field2 = tbx2.Text;
                QryVoucher.Rows[i]["Transport2"] = field2;

                TextBox tbx3 = this.Controls.Find(Master.TransportDetails.Rows[3]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                field4 = tbx3.Text;
                QryVoucher.Rows[i]["Grno"] = field4;

                TextBox tbx4 = this.Controls.Find(Master.TransportDetails.Rows[2]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                field3 = tbx4.Text;
                QryVoucher.Rows[i]["DeliveryAt"] = field3;

                TextBox tbx5 = this.Controls.Find(Master.TransportDetails.Rows[4]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                field5 = tbx5.Text;
                QryVoucher.Rows[i]["Transport3"] = field5;

                TextBox tbx6 = this.Controls.Find(Master.TransportDetails.Rows[5]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                field6 = tbx6.Text;
                QryVoucher.Rows[i]["Transport4"] = field6;

                TextBox tbx7 = this.Controls.Find(Master.TransportDetails.Rows[6]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                field7 = tbx7.Text;
                QryVoucher.Rows[i]["Transport5"] = field7;

                TextBox tbx8 = this.Controls.Find(Master.TransportDetails.Rows[7]["FName"].ToString(), true).FirstOrDefault() as TextBox;
                field8 = tbx8.Text;
                QryVoucher.Rows[i]["Transport6"] = field8;

                QryVoucher.Rows[i]["flatdis"] = ansGridView1.Rows[i].Cells["flatdis"].Value.ToString();
                QryVoucher.Rows[i]["QDAmount"] = ansGridView1.Rows[i].Cells["QDAmount"].Value.ToString();
                QryVoucher.Rows[i]["CDAmount"] = ansGridView1.Rows[i].Cells["CDAmount"].Value.ToString();
                QryVoucher.Rows[i]["FDAmount"] = ansGridView1.Rows[i].Cells["FDAmount"].Value.ToString();
                QryVoucher.Rows[i]["TotalDis"] = ansGridView1.Rows[i].Cells["TotalDis"].Value.ToString();
                QryVoucher.Rows[i]["bottomdis"] = ansGridView1.Rows[i].Cells["bottomdis"].Value.ToString();
                QryVoucher.Rows[i]["Amount0"] = ansGridView1.Rows[i].Cells["Amount0"].Value.ToString();
                QryVoucher.Rows[i]["Amount5"] = ansGridView1.Rows[i].Cells["Amount5"].Value.ToString();
                string shiptoname = funs.Select_ac_nm(shiptoacc_id);
                QryVoucher.Rows[i]["ShiptoN"] = shiptoprint;
                QryVoucher.Rows[i]["ShiptoAddress1"] = shiptoaddress1;
                QryVoucher.Rows[i]["ShiptoAddress2"] = shiptoaddress2;
                QryVoucher.Rows[i]["ShiptoState"] = shiptostate;
                QryVoucher.Rows[i]["ShiptoStateCode"] = funs.Select_state_GST(shiptostate);
                QryVoucher.Rows[i]["ShiptoPhone"] = shiptocontact;
                QryVoucher.Rows[i]["ShiptoEmail"] = shiptoemail;
                QryVoucher.Rows[i]["ShiptoPAN"] = shiptoPan;
                QryVoucher.Rows[i]["ShiptoAadhar"] = shiptoAadhar;
                QryVoucher.Rows[i]["ShiptoTIN"] = shiptotin;
                QryVoucher.Rows[i]["Packing"] = ansGridView1.Rows[i].Cells["unt"].Value.ToString();
                QryVoucher.Rows[i]["TotalAmount"] = textBox10.Text;
            }
            for (int i = 0; i < ansGridView3.Rows.Count - 1; i++)
            {
                Qryvoucherdes.Rows.Add();
                Qryvoucherdes.Rows[Qryvoucherdes.Rows.Count - 1]["Vi_id"] = 1;
                Qryvoucherdes.Rows[Qryvoucherdes.Rows.Count - 1]["Sequence"] = 1;
                Qryvoucherdes.Rows[Qryvoucherdes.Rows.Count - 1]["SubSequence"] = ansGridView3.Rows[i].Cells["sno2"].Value.ToString();
                Qryvoucherdes.Rows[Qryvoucherdes.Rows.Count - 1]["Name"] = ansGridView3.Rows[i].Cells["Charg_Name"].Value.ToString();
                Qryvoucherdes.Rows[Qryvoucherdes.Rows.Count - 1]["Amount"] = ansGridView3.Rows[i].Cells["CamountA"].Value.ToString();
            }

            for (int i = 0; i < ansGridView4.Rows.Count - 1; i++)
            {
                Qryvoucherdes.Rows.Add();
                Qryvoucherdes.Rows[Qryvoucherdes.Rows.Count - 1]["Vi_id"] = 1;
                Qryvoucherdes.Rows[Qryvoucherdes.Rows.Count - 1]["Sequence"] = 3;
                Qryvoucherdes.Rows[Qryvoucherdes.Rows.Count - 1]["SubSequence"] = ansGridView4.Rows[i].Cells["sno3"].Value.ToString();
                Qryvoucherdes.Rows[Qryvoucherdes.Rows.Count - 1]["Name"] = ansGridView4.Rows[i].Cells["Charg_Name2"].Value.ToString();
                Qryvoucherdes.Rows[Qryvoucherdes.Rows.Count - 1]["Amount"] = ansGridView4.Rows[i].Cells["CamountB"].Value.ToString();
            }

            if (double.Parse(textBox9.Text) != 0)
            {
                Qryvoucherdes.Rows.Add();
                Qryvoucherdes.Rows[Qryvoucherdes.Rows.Count - 1]["Vi_id"] = 1;
                Qryvoucherdes.Rows[Qryvoucherdes.Rows.Count - 1]["Sequence"] = 4;
                Qryvoucherdes.Rows[Qryvoucherdes.Rows.Count - 1]["SubSequence"] = 1;
                Qryvoucherdes.Rows[Qryvoucherdes.Rows.Count - 1]["Name"] = "Round Off";
                Qryvoucherdes.Rows[Qryvoucherdes.Rows.Count - 1]["Amount"] = textBox9.Text;
            }

            OtherReport rpt = new OtherReport();
            rpt.voucherprint(QryVoucher, Qryvoucherdes, QryVoucherTax, funs.Select_vt_id_vnm(voucherType));
        }


        private void textBox17_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "SELECT ACCOUNT.Name FROM  ACCOUNT LEFT OUTER JOIN  ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id WHERE (ACCOUNTYPE.Name = 'Agent') AND (ACCOUNT.Branch_id = '" + Database.BranchId + "') ORDER BY ACCOUNT.Name";
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
            if (e.Control && e.KeyCode == Keys.C)
            {
                textBox17.Text = funs.AddAccount();
            }
        }

        private void radioButton3_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void radioButton4_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void radioButton5_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (ansGridView1.CurrentCell == null)
            {
                return;
            }
            ItemSelected(true, ansGridView1.CurrentCell.RowIndex);
            if (ansGridView1.Rows.Count != 1)
            {
                ItemCalc(ansGridView1.CurrentRow.Index);
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (ansGridView1.CurrentCell == null)
            {
                return;
            }
            ItemSelected(true, ansGridView1.CurrentCell.RowIndex);
            if (ansGridView1.Rows.Count != 1)
            {
                ItemCalc(ansGridView1.CurrentRow.Index);
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (ansGridView1.CurrentCell == null)
            {
                return;
            }
            ItemSelected(true, ansGridView1.CurrentCell.RowIndex);
            if (ansGridView1.Rows.Count != 1)
            {
                ItemCalc(ansGridView1.CurrentRow.Index);
            }
        }


        public void ImportData(DataTable dtImport)
        {
            if (dtImport.Rows.Count > 0)
            {
                for (int i = 0; i < dtImport.Rows.Count; i++)
                {
                    ansGridView1.Rows.Add();
                    if (dtImport.Rows.Count > i)
                    {
                        ansGridView1.Rows[i].Cells["sno"].Value = (i + 1);
                    }
                    ansGridView1.Rows[i].Cells["description"].Value = dtImport.Rows[i]["Description"];
                    ansGridView1.Rows[i].Cells["unt"].Value = dtImport.Rows[i]["Packing"];
                    ansGridView1.Rows[i].Cells["Quantity"].Value = dtImport.Rows[i]["Quantity"];
                    ansGridView1.Rows[i].Cells["Rate_am"].Value = dtImport.Rows[i]["Rate"];
                    ansGridView1.Rows[i].Cells["Category_Id"].Value = dtImport.Rows[i]["Category_id"];
                    ansGridView1.Rows[i].Cells["Des_ac_id"].Value = dtImport.Rows[i]["Description_id"];
                    ansGridView1.Rows[i].Cells["cost"].Value = dtImport.Rows[i]["cost"];
                    ansGridView1.Rows[i].Cells["Commission_per"].Value = 0;
                    ansGridView1.Rows[i].Cells["CommissionFix"].Value = 0;
                    ansGridView1.Rows[i].Cells["MRP"].Value = 0;
                    ansGridView1.Rows[i].Cells["qd"].Value = dtImport.Rows[i]["Inbillscheme"];
                    ansGridView1.Rows[i].Cells["cd"].Value = dtImport.Rows[i]["CD"];
                    textBox2.Text = dtImport.Rows[i]["Ino"].ToString();
                    dateTimePicker2.Value = DateTime.Parse(dtImport.Rows[i]["Idt"].ToString());
                }
            }
        }

        private void ansGridView1_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            label20.Text = "";
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (ansGridView1.CurrentCell == null)
            {
                return;
            }

            ItemSelected(true, ansGridView1.CurrentCell.RowIndex);
            if (ansGridView1.Rows.Count != 1)
            {
                ItemCalc(ansGridView1.CurrentRow.Index);
            }
        }

        private void radioButton6_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox20_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox12_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox12);
        }

        private void textBox12_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox12);
        }

        private void textBox13_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select distinct '<MAIN>' as name from account union all SELECT ACCOUNT.Name as name FROM ACCOUNT LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id WHERE (((ACCOUNTYPE.Name)='Godown')) GROUP BY ACCOUNT.Name";
            textBox13.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                gExcludingTax = false;
            }
            else
            {
                gExcludingTax = true;
            }
            if (ansGridView1.Rows.Count != 1)
            {
                labelCalc();
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            labelCalc();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
            {
                ItemSelected(false, i);
            }
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton10.Checked == true)
            {
                for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
                {
                    ItemCalc(i);
                }
            }
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton9.Checked == true)
            {
                for (int i = 0; i < ansGridView1.Rows.Count - 1; i++)
                {
                    ItemCalc(i);
                }
            }
        }

        private void ansGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            SetDuedate(textBox14.Text);
        }

        private void ansGridView1_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton7_Enter(object sender, EventArgs e)
        {
            Database.setFocus(radioButton7);
        }

        private void radioButton7_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(radioButton7);
        }

        private void radioButton8_Enter(object sender, EventArgs e)
        {
            Database.setFocus(radioButton8);
        }

        private void radioButton8_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(radioButton8);
        }

        private void radioButton8_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void radioButton7_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox28_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from SalesMan";
            textBox28.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox28_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (textBox28.Text != "")
                {
                    textBox28.Text = funs.EditSalesman(textBox28.Text);
                }
            }
            if (e.Control && e.KeyCode == Keys.C)
            {
                textBox28.Text = funs.AddBroker();
            }
        }

        private void textBox28_Layout(object sender, LayoutEventArgs e)
        {

        }

        private void textBox28_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox28);
        }

        private void textBox28_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox28);
        }

        private void textBox18_MouseMove(object sender, MouseEventArgs e)
        {


        }

        private void label20_MouseMove(object sender, MouseEventArgs e)
        {
            Label lb = (Label)sender;
            ToolTip tp = new ToolTip();
            string str = "";
            DataTable dtgodown = new DataTable();
            Database.GetSqlData("SELECT ISNULL(Name, '<Main>') AS Godown, stok AS Stock FROM (SELECT Stock.Did, SUM( Stock.Receive - Stock.Issue) AS stok, ACCOUNT.Name  FROM Description RIGHT OUTER JOIN  Stock LEFT OUTER JOIN  ACCOUNT ON Stock.godown_id = ACCOUNT.Ac_id ON Description.Des_id = Stock.Did LEFT OUTER JOIN  VOUCHERTYPE RIGHT OUTER JOIN  VOUCHERINFO ON VOUCHERTYPE.Vt_id = VOUCHERINFO.Vt_id ON Stock.Vid = VOUCHERINFO.Vi_id  WHERE ( VOUCHERINFO.Vdate <= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND ( VOUCHERINFO.Branch_id = '" + Database.BranchId + "') OR  ( VOUCHERINFO.Vdate IS NULL)  GROUP BY Stock.Did, ACCOUNT.Name) AS res WHERE (Did = '" + ansGridView1.CurrentRow.Cells["des_ac_id"].Value.ToString() + "') GROUP BY stok, Name", dtgodown);

            for (int i = 0; i < dtgodown.Rows.Count; i++)
            {
                str += dtgodown.Rows[i]["Godown"].ToString() + " : " + funs.DecimalPoint(double.Parse(dtgodown.Rows[i]["Stock"].ToString()), 2) + Environment.NewLine;
            }

            tp.Show(str, lb, 0, 0, 1000);
        }
    }
}


