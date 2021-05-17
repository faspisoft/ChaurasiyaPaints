using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace faspi
{
    public partial class frm_tallydate : Form
    {
        public string type = "";
        ENVELOPE obj = new ENVELOPE();
        public frm_tallydate()
        {
            InitializeComponent();


            dateTimePicker1.CustomFormat = Database.dformat;
            dateTimePicker2.CustomFormat = Database.dformat;
            dateTimePicker1.MinDate = Database.stDate;
            dateTimePicker1.MaxDate = Database.ldate;
            dateTimePicker2.MinDate = Database.stDate;
            dateTimePicker2.MaxDate = Database.ldate;
            dateTimePicker1.Value = Database.ldate;
            dateTimePicker2.Value = Database.ldate;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
           
            DataTable dtvou = new DataTable();
            string branch = "";
            if (Feature.Available("Export Vouchers in Tally").ToUpper() != "ALL")
            {
                branch = " And Voucherinfo.Branch_id='" + Database.BranchId + "'";
            }
            if (type == "Receipt")
            {
                dtvou = new DataTable();
                Database.GetSqlData("SELECT VOUCHERINFO.Vi_id FROM VOUCHERINFO INNER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERTYPE.Type)='Receipt') AND ((VOUCHERTYPE."+ Database.BMode+")=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ") AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dateTimePicker2.Value.Date.ToString(Database.dformat) + access_sql.Hash + "))" + branch, dtvou);


                if (dtvou.Rows.Count == 0)
                {

                    MessageBox.Show("No Record Found..");
                }
                else
                {


                    SaveFileDialog savefile = new SaveFileDialog();
                    if (DialogResult.OK == savefile.ShowDialog())
                    {

                        obj.HEADER = new clsHEADER();
                        obj.HEADER.TALLYREQUEST = "Import Data";
                        obj.BODY = new clsBODY();
                        obj.BODY.IMPORTDATA = new clsIMPORTDATA();
                        obj.BODY.IMPORTDATA.REQUESTDESC = new clsREQUESTDESC();
                        obj.BODY.IMPORTDATA.REQUESTDESC.REPORTNAME = "All Masters";
                        List<clsTALLYMESSAGE> lstMsg = new List<clsTALLYMESSAGE>();
                        foreach (DataRow dtr in dtvou.Rows)
                        {
                            DataTable dtdata = new DataTable();
                            Database.GetSqlData("SELECT ACCOUNT.Name AS AccountName, Sum(JOURNAL.Amount) AS Amount, JOURNAL.Narr, JOURNAL.Vdate, VOUCHERTYPE.AliasName, VOUCHERINFO.Vnumber FROM ((VOUCHERINFO LEFT JOIN JOURNAL ON VOUCHERINFO.Vi_id = JOURNAL.Vi_id) LEFT JOIN ACCOUNT ON JOURNAL.Ac_id = ACCOUNT.Ac_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id GROUP BY ACCOUNT.Name, JOURNAL.Narr, JOURNAL.Vdate, VOUCHERTYPE.AliasName, VOUCHERINFO.Vnumber, VOUCHERINFO.Vi_id HAVING (((VOUCHERINFO.Vi_id)='" + dtr["Vi_id"].ToString() + "')) ORDER BY Sum(JOURNAL.Amount)", dtdata);

                            clsTALLYMESSAGE tm = new clsTALLYMESSAGE();
                            tm.VOUCHER = new clsVOUCHER();
                            tm.VOUCHER.ACTION = "Create";
                            tm.VOUCHER.VCHTYPE = "Receipt";
                            tm.VOUCHER.DATE = DateTime.Parse(dtdata.Rows[0]["Vdate"].ToString()).ToString("yyyyMMdd");
                            tm.VOUCHER.NARRATION = dtdata.Rows[0]["Narr"].ToString();
                            tm.VOUCHER.VOUCHERTYPENAME = "Receipt";
                            tm.VOUCHER.VOUCHERNUMBER = dtdata.Rows[0]["Vnumber"].ToString();
                            tm.VOUCHER.PARTYLEDGERNAME = dtdata.Rows[0]["AccountName"].ToString();

                            List<clsALLLEDGERENTRIESLIST> lstItems = new List<clsALLLEDGERENTRIESLIST>();
                            for (int i = 0; i < dtdata.Rows.Count; i++)
                            {
                                clsALLLEDGERENTRIESLIST objItem = new clsALLLEDGERENTRIESLIST();
                                objItem.LEDGERNAME = dtdata.Rows[i]["AccountName"].ToString();
                                objItem.ISDEEMEDPOSITIVE = "No";
                                objItem.ISLASTDEEMEDPOSITIVE = "No";

                                if (double.Parse(dtdata.Rows[i]["Amount"].ToString()) > 0)
                                {
                                    objItem.ISDEEMEDPOSITIVE = "Yes";
                                    objItem.ISLASTDEEMEDPOSITIVE = "Yes";
                                }
                                objItem.AMOUNT = -1 * double.Parse(dtdata.Rows[i]["Amount"].ToString());
                                lstItems.Add(objItem);
                            }
                            tm.VOUCHER.ALLLEDGERENTRIES_LIST = lstItems;
                            lstMsg.Add(tm);
                        }
                        obj.BODY.IMPORTDATA.REQUESTDATA = lstMsg;
                        XmlSerializer SerializerObj = new XmlSerializer(obj.GetType());

                        string path = ".xml";
                        TextWriter WriteFileStream = new StreamWriter(savefile.FileName + path);
                        try
                        {
                            SerializerObj.Serialize(WriteFileStream, obj);
                            MessageBox.Show("XML File is created on Location : " + savefile.FileName);

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                            MessageBox.Show("XML File is  not created");
                        }
                        WriteFileStream.Close();

                    }

                }
            }

            if (type == "Payment")
            {
                dtvou = new DataTable();
                Database.GetSqlData("SELECT VOUCHERINFO.Vi_id FROM VOUCHERINFO INNER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERTYPE.Type)='Payment') AND ((VOUCHERTYPE."+ Database.BMode+")=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ") AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dateTimePicker2.Value.Date.ToString(Database.dformat) + access_sql.Hash + ")) "+ branch, dtvou);
                if (dtvou.Rows.Count == 0)
                {
                    MessageBox.Show("No Record Found..");
                }
                else
                {
                    SaveFileDialog savefile = new SaveFileDialog();
                    if (DialogResult.OK == savefile.ShowDialog())
                    {
                        obj.HEADER = new clsHEADER();

                        obj.HEADER.TALLYREQUEST = "Import Data";
                        obj.BODY = new clsBODY();

                        obj.BODY.IMPORTDATA = new clsIMPORTDATA();


                        obj.BODY.IMPORTDATA.REQUESTDESC = new clsREQUESTDESC();
                        obj.BODY.IMPORTDATA.REQUESTDESC.REPORTNAME = "All Masters";
                        //obj.BODY.IMPORTDATA.REQUESTDATA = new clsREQUESTDATA();
                        List<clsTALLYMESSAGE> lstMsg = new List<clsTALLYMESSAGE>();


                        foreach (DataRow dtr in dtvou.Rows)
                        {
                            DataTable dtdata = new DataTable();
                            Database.GetSqlData("SELECT ACCOUNT.Name AS AccountName, Sum(JOURNAL.Amount) AS Amount, JOURNAL.Narr, JOURNAL.Vdate, VOUCHERTYPE.AliasName, VOUCHERINFO.Vnumber FROM ((VOUCHERINFO LEFT JOIN JOURNAL ON VOUCHERINFO.Vi_id = JOURNAL.Vi_id) LEFT JOIN ACCOUNT ON JOURNAL.Ac_id = ACCOUNT.Ac_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id GROUP BY ACCOUNT.Name, JOURNAL.Narr, JOURNAL.Vdate, VOUCHERTYPE.AliasName, VOUCHERINFO.Vnumber, VOUCHERINFO.Vi_id HAVING (((VOUCHERINFO.Vi_id)='" + dtr["Vi_id"].ToString() + "')) ORDER BY Sum(JOURNAL.Amount) desc", dtdata);
                            // Database.GetSqlData("SELECT ACCOUNT.Name as AccountName, JOURNAL.Amount AS Amount, JOURNAL.Narr, JOURNAL.Vdate, VOUCHERTYPE.AliasName, VOUCHERINFO.Vnumber FROM ((VOUCHERINFO LEFT JOIN JOURNAL ON VOUCHERINFO.Vi_id = JOURNAL.Vi_id) LEFT JOIN ACCOUNT ON JOURNAL.Ac_id = ACCOUNT.Ac_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERINFO.Vi_id)=" + dtr["Vi_id"].ToString() + ")) ORDER BY JOURNAL.Amount", dtdata);

                            clsTALLYMESSAGE tm = new clsTALLYMESSAGE();
                            tm.VOUCHER = new clsVOUCHER();
                            tm.VOUCHER.ACTION = "Create";
                            tm.VOUCHER.VCHTYPE = "Payment";
                            tm.VOUCHER.DATE = DateTime.Parse(dtdata.Rows[0]["Vdate"].ToString()).ToString("yyyyMMdd");
                            tm.VOUCHER.NARRATION = dtdata.Rows[0]["Narr"].ToString();
                            tm.VOUCHER.VOUCHERTYPENAME = "Payment";
                            tm.VOUCHER.VOUCHERNUMBER = dtdata.Rows[0]["Vnumber"].ToString();
                            tm.VOUCHER.PARTYLEDGERNAME = dtdata.Rows[0]["AccountName"].ToString();

                            List<clsALLLEDGERENTRIESLIST> lstItems = new List<clsALLLEDGERENTRIESLIST>();

                            for (int i = 0; i < dtdata.Rows.Count; i++)
                            {
                                clsALLLEDGERENTRIESLIST objItem = new clsALLLEDGERENTRIESLIST();

                                objItem.LEDGERNAME = dtdata.Rows[i]["AccountName"].ToString();

                                objItem.ISDEEMEDPOSITIVE = "Yes";
                                objItem.ISLASTDEEMEDPOSITIVE = "Yes";

                                if (double.Parse(dtdata.Rows[i]["Amount"].ToString()) < 0)
                                {
                                    objItem.ISDEEMEDPOSITIVE = "No";
                                    objItem.ISLASTDEEMEDPOSITIVE = "No";
                                }
                                objItem.AMOUNT = -1 * double.Parse(dtdata.Rows[i]["Amount"].ToString());
                                lstItems.Add(objItem);
                            }

                            tm.VOUCHER.ALLLEDGERENTRIES_LIST = lstItems;
                            lstMsg.Add(tm);
                        }


                        obj.BODY.IMPORTDATA.REQUESTDATA = lstMsg;
                        XmlSerializer SerializerObj = new XmlSerializer(obj.GetType());

                        string path = ".xml";
                        TextWriter WriteFileStream = new StreamWriter(savefile.FileName + path);
                        try
                        {
                            SerializerObj.Serialize(WriteFileStream, obj);
                            MessageBox.Show("XML File is created on Location : " + savefile.FileName);

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                            MessageBox.Show("XML File is  not created");
                        }
                        WriteFileStream.Close();

                    }

                }
            }

            if (type == "Purchase")
            {
                dtvou = new DataTable();
                Database.GetSqlData("SELECT VOUCHERINFO.Vi_id FROM VOUCHERINFO INNER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERTYPE.Type)='Purchase') AND ((VOUCHERTYPE."+ Database.BMode+")=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ") AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dateTimePicker2.Value.Date.ToString(Database.dformat) + access_sql.Hash + ")) "+ branch, dtvou);
                if (dtvou.Rows.Count == 0)
                {
                    MessageBox.Show("No Record Found..");
                }
                else
                {
                    SaveFileDialog savefile = new SaveFileDialog();
                    if (DialogResult.OK == savefile.ShowDialog())
                    {
                        obj.HEADER = new clsHEADER();

                        obj.HEADER.TALLYREQUEST = "Import Data";
                        obj.BODY = new clsBODY();

                        obj.BODY.IMPORTDATA = new clsIMPORTDATA();


                        obj.BODY.IMPORTDATA.REQUESTDESC = new clsREQUESTDESC();
                        obj.BODY.IMPORTDATA.REQUESTDESC.REPORTNAME = "All Masters";
                        //obj.BODY.IMPORTDATA.REQUESTDATA = new clsREQUESTDATA();
                        List<clsTALLYMESSAGE> lstMsg = new List<clsTALLYMESSAGE>();

                        foreach (DataRow dtr in dtvou.Rows)
                        {
                            DataTable dtdata = new DataTable();
                            Database.GetSqlData("SELECT ACCOUNT.Name AS AccountName, Sum(JOURNAL.Amount) AS Amount, JOURNAL.Narr, JOURNAL.Vdate, VOUCHERTYPE.AliasName, VOUCHERINFO.Vnumber, VOUCHERINFO.Svnum, VOUCHERINFO.Svdate FROM ((VOUCHERINFO LEFT JOIN JOURNAL ON VOUCHERINFO.Vi_id = JOURNAL.Vi_id) LEFT JOIN ACCOUNT ON JOURNAL.Ac_id = ACCOUNT.Ac_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id GROUP BY ACCOUNT.Name, JOURNAL.Narr, JOURNAL.Vdate, VOUCHERTYPE.AliasName, VOUCHERINFO.Vnumber, VOUCHERINFO.Vi_id, VOUCHERINFO.Svnum, VOUCHERINFO.Svdate HAVING (((VOUCHERINFO.Vi_id)='" + dtr["Vi_id"].ToString() + "')) ORDER BY Sum(JOURNAL.Amount)", dtdata);

                            clsTALLYMESSAGE tm = new clsTALLYMESSAGE();
                            tm.VOUCHER = new clsVOUCHER();
                            tm.VOUCHER.ACTION = "Create";
                            tm.VOUCHER.VCHTYPE = "Purchase";
                            tm.VOUCHER.DATE = DateTime.Parse(dtdata.Rows[0]["Vdate"].ToString()).ToString("yyyyMMdd");
                            tm.VOUCHER.REFERENCEDATE = DateTime.Parse(dtdata.Rows[0]["SVdate"].ToString()).ToString("yyyyMMdd");
                            tm.VOUCHER.NARRATION = dtdata.Rows[0]["Narr"].ToString();
                            tm.VOUCHER.VOUCHERTYPENAME = "Purchase";
                            tm.VOUCHER.REFERENCE = dtdata.Rows[0]["Svnum"].ToString();
                            tm.VOUCHER.VOUCHERNUMBER = dtdata.Rows[0]["Vnumber"].ToString();
                            tm.VOUCHER.PARTYLEDGERNAME = dtdata.Rows[0]["AccountName"].ToString();

                            List<clsALLLEDGERENTRIESLIST> lstItems = new List<clsALLLEDGERENTRIESLIST>();

                            for (int i = 0; i < dtdata.Rows.Count; i++)
                            {
                                clsALLLEDGERENTRIESLIST objItem = new clsALLLEDGERENTRIESLIST();

                                objItem.LEDGERNAME = dtdata.Rows[i]["AccountName"].ToString();

                                objItem.ISDEEMEDPOSITIVE = "Yes";
                                objItem.ISLASTDEEMEDPOSITIVE = "Yes";

                                if (double.Parse(dtdata.Rows[i]["Amount"].ToString()) < 0)
                                {
                                    objItem.ISDEEMEDPOSITIVE = "No";
                                    objItem.ISLASTDEEMEDPOSITIVE = "No";
                                }
                                objItem.AMOUNT = -1 * double.Parse(dtdata.Rows[i]["Amount"].ToString());
                                lstItems.Add(objItem);
                            }

                            tm.VOUCHER.ALLLEDGERENTRIES_LIST = lstItems;
                            lstMsg.Add(tm);
                        }


                        obj.BODY.IMPORTDATA.REQUESTDATA = lstMsg;
                        XmlSerializer SerializerObj = new XmlSerializer(obj.GetType());

                        string path = ".xml";
                        TextWriter WriteFileStream = new StreamWriter(savefile.FileName + path);
                        try
                        {
                            SerializerObj.Serialize(WriteFileStream, obj);
                            MessageBox.Show("XML File is created on Location : " + savefile.FileName);

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                            MessageBox.Show("XML File is  not created");
                        }
                        WriteFileStream.Close();

                    }

                }
            }

            if (type == "Sale")
            {
                dtvou = new DataTable();
                Database.GetSqlData("SELECT VOUCHERINFO.Vi_id FROM VOUCHERINFO INNER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERTYPE.Type)='Sale') AND ((VOUCHERTYPE." + Database.BMode + ")=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ") AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dateTimePicker2.Value.Date.ToString(Database.dformat) + access_sql.Hash + ")) "+ branch, dtvou);
                if (dtvou.Rows.Count == 0)
                {
                    MessageBox.Show("No Record Found..");
                }
                else
                {
                    SaveFileDialog savefile = new SaveFileDialog();
                    if (DialogResult.OK == savefile.ShowDialog())
                    {


                        obj.HEADER = new clsHEADER();

                        obj.HEADER.TALLYREQUEST = "Import Data";
                        obj.BODY = new clsBODY();

                        obj.BODY.IMPORTDATA = new clsIMPORTDATA();


                        obj.BODY.IMPORTDATA.REQUESTDESC = new clsREQUESTDESC();
                        obj.BODY.IMPORTDATA.REQUESTDESC.REPORTNAME = "All Masters";
                        //obj.BODY.IMPORTDATA.REQUESTDATA = new clsREQUESTDATA();
                        List<clsTALLYMESSAGE> lstMsg = new List<clsTALLYMESSAGE>();

                        foreach (DataRow dtr in dtvou.Rows)
                        {
                            DataTable dtdata = new DataTable();
                            Database.GetSqlData("SELECT ACCOUNT.Name AS AccountName, Sum(JOURNAL.Amount) AS Amount, JOURNAL.Narr, JOURNAL.Vdate, VOUCHERTYPE.AliasName, VOUCHERINFO.Vnumber FROM ((VOUCHERINFO LEFT JOIN JOURNAL ON VOUCHERINFO.Vi_id = JOURNAL.Vi_id) LEFT JOIN ACCOUNT ON JOURNAL.Ac_id = ACCOUNT.Ac_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE     (Journal.A = 'true') GROUP BY ACCOUNT.Name, JOURNAL.Narr, JOURNAL.Vdate, VOUCHERTYPE.AliasName, VOUCHERINFO.Vnumber, VOUCHERINFO.Vi_id HAVING (((VOUCHERINFO.Vi_id)='" + dtr["Vi_id"].ToString() + "')) ORDER BY Sum(JOURNAL.Amount) desc", dtdata);

                            clsTALLYMESSAGE tm = new clsTALLYMESSAGE();
                            tm.VOUCHER = new clsVOUCHER();
                            tm.VOUCHER.ACTION = "Create";
                            tm.VOUCHER.VCHTYPE = "Sales";
                            tm.VOUCHER.DATE = DateTime.Parse(dtdata.Rows[0]["Vdate"].ToString()).ToString("yyyyMMdd");
                            tm.VOUCHER.NARRATION = dtdata.Rows[0]["Narr"].ToString();
                            tm.VOUCHER.VOUCHERTYPENAME = "Sales";
                            tm.VOUCHER.VOUCHERNUMBER = dtdata.Rows[0]["Vnumber"].ToString();
                            tm.VOUCHER.PARTYLEDGERNAME = dtdata.Rows[0]["AccountName"].ToString();

                            List<clsALLLEDGERENTRIESLIST> lstItems = new List<clsALLLEDGERENTRIESLIST>();

                            for (int i = 0; i < dtdata.Rows.Count; i++)
                            {
                                clsALLLEDGERENTRIESLIST objItem = new clsALLLEDGERENTRIESLIST();

                                objItem.LEDGERNAME = dtdata.Rows[i]["AccountName"].ToString();

                                objItem.ISDEEMEDPOSITIVE = "Yes";
                                objItem.ISLASTDEEMEDPOSITIVE = "Yes";

                                if (double.Parse(dtdata.Rows[i]["Amount"].ToString()) < 0)
                                {
                                    objItem.ISDEEMEDPOSITIVE = "No";
                                    objItem.ISLASTDEEMEDPOSITIVE = "No";
                                }
                                objItem.AMOUNT = -1 * double.Parse(dtdata.Rows[i]["Amount"].ToString());
                                lstItems.Add(objItem);
                            }

                            tm.VOUCHER.ALLLEDGERENTRIES_LIST = lstItems;
                            lstMsg.Add(tm);
                        }


                        obj.BODY.IMPORTDATA.REQUESTDATA = lstMsg;
                        XmlSerializer SerializerObj = new XmlSerializer(obj.GetType());

                        string path = ".xml";
                        TextWriter WriteFileStream = new StreamWriter(savefile.FileName + path);
                        try
                        {
                            SerializerObj.Serialize(WriteFileStream, obj);
                            MessageBox.Show("XML File is created on Location : " + savefile.FileName);

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                            MessageBox.Show("XML File is  not created");
                        }
                        WriteFileStream.Close();

                    }

                }
            }


            if (type == "Journal")
            {
                dtvou = new DataTable();
                Database.GetSqlData("SELECT VOUCHERINFO.Vi_id FROM VOUCHERINFO INNER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERTYPE.Type)='Journal') AND ((VOUCHERTYPE." + Database.BMode + ")=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ") AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dateTimePicker2.Value.Date.ToString(Database.dformat) + access_sql.Hash + ")) " + branch, dtvou);
                if (dtvou.Rows.Count == 0)
                {
                    MessageBox.Show("No Record Found..");
                }
                else
                {
                    SaveFileDialog savefile = new SaveFileDialog();
                    if (DialogResult.OK == savefile.ShowDialog())
                    {
                        obj.HEADER = new clsHEADER();

                        obj.HEADER.TALLYREQUEST = "Import Data";
                        obj.BODY = new clsBODY();

                        obj.BODY.IMPORTDATA = new clsIMPORTDATA();


                        obj.BODY.IMPORTDATA.REQUESTDESC = new clsREQUESTDESC();
                        obj.BODY.IMPORTDATA.REQUESTDESC.REPORTNAME = "All Masters";
                        //obj.BODY.IMPORTDATA.REQUESTDATA = new clsREQUESTDATA();
                        List<clsTALLYMESSAGE> lstMsg = new List<clsTALLYMESSAGE>();

                        foreach (DataRow dtr in dtvou.Rows)
                        {
                            DataTable dtdata = new DataTable();
                            Database.GetSqlData("SELECT ACCOUNT.Name AS AccountName, Sum(JOURNAL.Amount) AS Amount, JOURNAL.Narr, JOURNAL.Vdate, VOUCHERTYPE.AliasName, VOUCHERINFO.Vnumber FROM ((VOUCHERINFO LEFT JOIN JOURNAL ON VOUCHERINFO.Vi_id = JOURNAL.Vi_id) LEFT JOIN ACCOUNT ON JOURNAL.Ac_id = ACCOUNT.Ac_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id GROUP BY ACCOUNT.Name, JOURNAL.Narr, JOURNAL.Vdate, VOUCHERTYPE.AliasName, VOUCHERINFO.Vnumber, VOUCHERINFO.Vi_id HAVING (((VOUCHERINFO.Vi_id)='" + dtr["Vi_id"].ToString() + "')) ORDER BY Sum(JOURNAL.Amount) desc", dtdata);
                            // Database.GetSqlData("SELECT ACCOUNT.Name as AccountName, JOURNAL.Amount AS Amount, JOURNAL.Narr, JOURNAL.Vdate, VOUCHERTYPE.AliasName, VOUCHERINFO.Vnumber FROM ((VOUCHERINFO LEFT JOIN JOURNAL ON VOUCHERINFO.Vi_id = JOURNAL.Vi_id) LEFT JOIN ACCOUNT ON JOURNAL.Ac_id = ACCOUNT.Ac_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERINFO.Vi_id)=" + dtr["Vi_id"].ToString() + ")) ORDER BY JOURNAL.Amount", dtdata);

                            clsTALLYMESSAGE tm = new clsTALLYMESSAGE();
                            tm.VOUCHER = new clsVOUCHER();
                            tm.VOUCHER.ACTION = "Create";
                            tm.VOUCHER.VCHTYPE = "Journal";
                            tm.VOUCHER.DATE = DateTime.Parse(dtdata.Rows[0]["Vdate"].ToString()).ToString("yyyyMMdd");
                            tm.VOUCHER.NARRATION = dtdata.Rows[0]["Narr"].ToString();
                            tm.VOUCHER.VOUCHERTYPENAME = "Journal";
                            tm.VOUCHER.VOUCHERNUMBER = dtdata.Rows[0]["Vnumber"].ToString();
                            tm.VOUCHER.PARTYLEDGERNAME = dtdata.Rows[0]["AccountName"].ToString();

                            List<clsALLLEDGERENTRIESLIST> lstItems = new List<clsALLLEDGERENTRIESLIST>();

                            for (int i = 0; i < dtdata.Rows.Count; i++)
                            {
                                clsALLLEDGERENTRIESLIST objItem = new clsALLLEDGERENTRIESLIST();

                                objItem.LEDGERNAME = dtdata.Rows[i]["AccountName"].ToString();

                                objItem.ISDEEMEDPOSITIVE = "Yes";
                                objItem.ISLASTDEEMEDPOSITIVE = "Yes";

                                if (double.Parse(dtdata.Rows[i]["Amount"].ToString()) < 0)
                                {
                                    objItem.ISDEEMEDPOSITIVE = "No";
                                    objItem.ISLASTDEEMEDPOSITIVE = "No";
                                }
                                objItem.AMOUNT = -1 * double.Parse(dtdata.Rows[i]["Amount"].ToString());
                                lstItems.Add(objItem);
                            }

                            tm.VOUCHER.ALLLEDGERENTRIES_LIST = lstItems;
                            lstMsg.Add(tm);
                        }


                        obj.BODY.IMPORTDATA.REQUESTDATA = lstMsg;
                        XmlSerializer SerializerObj = new XmlSerializer(obj.GetType());

                        string path = ".xml";
                        TextWriter WriteFileStream = new StreamWriter(savefile.FileName + path);
                        try
                        {
                            SerializerObj.Serialize(WriteFileStream, obj);
                            MessageBox.Show("XML File is created on Location : " + savefile.FileName);

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                            MessageBox.Show("XML File is  not created");
                        }
                        WriteFileStream.Close();

                    }


                }
            }

            if (type == "Contra")
            {
                dtvou = new DataTable();

                Database.GetSqlData("SELECT VOUCHERINFO.Vi_id FROM VOUCHERINFO INNER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERTYPE.Type)='Contra') AND ((VOUCHERTYPE." + Database.BMode + ")=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ") AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dateTimePicker2.Value.Date.ToString(Database.dformat) + access_sql.Hash + "))" + branch, dtvou);
                if (dtvou.Rows.Count == 0)
                {
                    MessageBox.Show("No Record Found..");
                }
                else
                {
                    SaveFileDialog savefile = new SaveFileDialog();
                    if (DialogResult.OK == savefile.ShowDialog())
                    {
                        obj.HEADER = new clsHEADER();

                        obj.HEADER.TALLYREQUEST = "Import Data";
                        obj.BODY = new clsBODY();

                        obj.BODY.IMPORTDATA = new clsIMPORTDATA();


                        obj.BODY.IMPORTDATA.REQUESTDESC = new clsREQUESTDESC();
                        obj.BODY.IMPORTDATA.REQUESTDESC.REPORTNAME = "All Masters";
                        //obj.BODY.IMPORTDATA.REQUESTDATA = new clsREQUESTDATA();
                        List<clsTALLYMESSAGE> lstMsg = new List<clsTALLYMESSAGE>();

                        foreach (DataRow dtr in dtvou.Rows)
                        {
                            DataTable dtdata = new DataTable();

                            Database.GetSqlData("SELECT ACCOUNT.Name AS AccountName, Sum(JOURNAL.Amount) AS Amount, JOURNAL.Narr, JOURNAL.Vdate, VOUCHERTYPE.AliasName, VOUCHERINFO.Vnumber FROM ((VOUCHERINFO LEFT JOIN JOURNAL ON VOUCHERINFO.Vi_id = JOURNAL.Vi_id) LEFT JOIN ACCOUNT ON JOURNAL.Ac_id = ACCOUNT.Ac_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id GROUP BY ACCOUNT.Name, JOURNAL.Narr, JOURNAL.Vdate, VOUCHERTYPE.AliasName, VOUCHERINFO.Vnumber, VOUCHERINFO.Vi_id HAVING (((VOUCHERINFO.Vi_id)='" + dtr["Vi_id"].ToString() + "')) ORDER BY Sum(JOURNAL.Amount) desc", dtdata);
                            // Database.GetSqlData("SELECT ACCOUNT.Name as AccountName, JOURNAL.Amount AS Amount, JOURNAL.Narr, JOURNAL.Vdate, VOUCHERTYPE.AliasName, VOUCHERINFO.Vnumber FROM ((VOUCHERINFO LEFT JOIN JOURNAL ON VOUCHERINFO.Vi_id = JOURNAL.Vi_id) LEFT JOIN ACCOUNT ON JOURNAL.Ac_id = ACCOUNT.Ac_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERINFO.Vi_id)=" + dtr["Vi_id"].ToString() + ")) ORDER BY JOURNAL.Amount", dtdata);

                            clsTALLYMESSAGE tm = new clsTALLYMESSAGE();
                            tm.VOUCHER = new clsVOUCHER();
                            tm.VOUCHER.ACTION = "Create";
                            tm.VOUCHER.VCHTYPE = "Contra";
                            tm.VOUCHER.DATE = DateTime.Parse(dtdata.Rows[0]["Vdate"].ToString()).ToString("yyyyMMdd");
                            tm.VOUCHER.NARRATION = dtdata.Rows[0]["Narr"].ToString();
                            tm.VOUCHER.VOUCHERTYPENAME = "Contra";
                            tm.VOUCHER.VOUCHERNUMBER = dtdata.Rows[0]["Vnumber"].ToString();
                            tm.VOUCHER.PARTYLEDGERNAME = dtdata.Rows[0]["AccountName"].ToString();

                            List<clsALLLEDGERENTRIESLIST> lstItems = new List<clsALLLEDGERENTRIESLIST>();

                            for (int i = 0; i < dtdata.Rows.Count; i++)
                            {
                                clsALLLEDGERENTRIESLIST objItem = new clsALLLEDGERENTRIESLIST();

                                objItem.LEDGERNAME = dtdata.Rows[i]["AccountName"].ToString();

                                objItem.ISDEEMEDPOSITIVE = "Yes";
                                objItem.ISLASTDEEMEDPOSITIVE = "Yes";

                                if (double.Parse(dtdata.Rows[i]["Amount"].ToString()) < 0)
                                {
                                    objItem.ISDEEMEDPOSITIVE = "No";
                                    objItem.ISLASTDEEMEDPOSITIVE = "No";
                                }
                                objItem.AMOUNT = -1*double.Parse(dtdata.Rows[i]["Amount"].ToString());
                                lstItems.Add(objItem);
                            }
                      

                            tm.VOUCHER.ALLLEDGERENTRIES_LIST = lstItems;
                            lstMsg.Add(tm);
                        }


                        obj.BODY.IMPORTDATA.REQUESTDATA = lstMsg;
                        XmlSerializer SerializerObj = new XmlSerializer(obj.GetType());

                        string path = ".xml";
                        TextWriter WriteFileStream = new StreamWriter(savefile.FileName + path);
                        try
                        {
                            SerializerObj.Serialize(WriteFileStream, obj);
                            MessageBox.Show("XML File is created on Location : " + savefile.FileName);

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                            MessageBox.Show("XML File is  not created");
                        }
                        WriteFileStream.Close();

                    }


                }
            }
            if (type == "Dnote")
            {
                dtvou = new DataTable();
                Database.GetSqlData("SELECT VOUCHERINFO.Vi_id FROM VOUCHERINFO INNER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE ((VOUCHERTYPE.Type)='Dnote' AND ((VOUCHERTYPE." + Database.BMode + ")=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ") AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dateTimePicker2.Value.Date.ToString(Database.dformat) + access_sql.Hash + "))" + branch, dtvou);
                if (dtvou.Rows.Count == 0)
                {
                    MessageBox.Show("No Record found in Debit Notes..");
                }

                else
                {
                    MessageBox.Show("File will be created for Debit Notes Entered in Marwari S/w");
                    SaveFileDialog savefile = new SaveFileDialog();
                    if (DialogResult.OK == savefile.ShowDialog())
                    {
                        obj.HEADER = new clsHEADER();

                        obj.HEADER.TALLYREQUEST = "Import Data";
                        obj.BODY = new clsBODY();

                        obj.BODY.IMPORTDATA = new clsIMPORTDATA();


                        obj.BODY.IMPORTDATA.REQUESTDESC = new clsREQUESTDESC();
                        obj.BODY.IMPORTDATA.REQUESTDESC.REPORTNAME = "All Masters";
                        //obj.BODY.IMPORTDATA.REQUESTDATA = new clsREQUESTDATA();
                        List<clsTALLYMESSAGE> lstMsg = new List<clsTALLYMESSAGE>();

                        foreach (DataRow dtr in dtvou.Rows)
                        {
                            DataTable dtdata = new DataTable();
                            Database.GetSqlData("SELECT ACCOUNT.Name AS AccountName, Sum(JOURNAL.Amount) AS Amount, JOURNAL.Narr, JOURNAL.Vdate, VOUCHERTYPE.AliasName, VOUCHERINFO.Vnumber FROM ((VOUCHERINFO LEFT JOIN JOURNAL ON VOUCHERINFO.Vi_id = JOURNAL.Vi_id) LEFT JOIN ACCOUNT ON JOURNAL.Ac_id = ACCOUNT.Ac_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id GROUP BY ACCOUNT.Name, JOURNAL.Narr, JOURNAL.Vdate, VOUCHERTYPE.AliasName, VOUCHERINFO.Vnumber, VOUCHERINFO.Vi_id HAVING (((VOUCHERINFO.Vi_id)='" + dtr["Vi_id"].ToString() + "')) ORDER BY Sum(JOURNAL.Amount) desc", dtdata);

                            clsTALLYMESSAGE tm = new clsTALLYMESSAGE();
                            tm.VOUCHER = new clsVOUCHER();
                            tm.VOUCHER.ACTION = "Create";
                            tm.VOUCHER.VCHTYPE = "Debit Note";
                            tm.VOUCHER.DATE = DateTime.Parse(dtdata.Rows[0]["Vdate"].ToString()).ToString("yyyyMMdd");

                            tm.VOUCHER.NARRATION = dtdata.Rows[0]["Narr"].ToString();
                            tm.VOUCHER.VOUCHERTYPENAME = "Debit Note";

                            tm.VOUCHER.VOUCHERNUMBER = dtdata.Rows[0]["Vnumber"].ToString();
                            tm.VOUCHER.PARTYLEDGERNAME = dtdata.Rows[0]["AccountName"].ToString();

                            List<clsALLLEDGERENTRIESLIST> lstItems = new List<clsALLLEDGERENTRIESLIST>();

                            for (int i = 0; i < dtdata.Rows.Count; i++)
                            {
                                clsALLLEDGERENTRIESLIST objItem = new clsALLLEDGERENTRIESLIST();

                                objItem.LEDGERNAME = dtdata.Rows[i]["AccountName"].ToString();

                                objItem.ISDEEMEDPOSITIVE = "Yes";
                                objItem.ISLASTDEEMEDPOSITIVE = "Yes";

                                if (double.Parse(dtdata.Rows[i]["Amount"].ToString()) < 0)
                                {
                                    objItem.ISDEEMEDPOSITIVE = "No";
                                    objItem.ISLASTDEEMEDPOSITIVE = "No";
                                }
                                objItem.AMOUNT = -1 * double.Parse(dtdata.Rows[i]["Amount"].ToString());
                                lstItems.Add(objItem);
                            }

                            tm.VOUCHER.ALLLEDGERENTRIES_LIST = lstItems;
                            lstMsg.Add(tm);
                        }


                        obj.BODY.IMPORTDATA.REQUESTDATA = lstMsg;



                        XmlSerializer SerializerObj = new XmlSerializer(obj.GetType());

                        string path = ".xml";
                        TextWriter WriteFileStream = new StreamWriter(savefile.FileName + path);
                        try
                        {
                            SerializerObj.Serialize(WriteFileStream, obj);
                            MessageBox.Show("XML File for Debit Note is created on Location : " + savefile.FileName);

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                            MessageBox.Show("XML File is  not created");
                        }
                        WriteFileStream.Close();

                    }

                }

                dtvou = new DataTable();
                Database.GetSqlData("SELECT VOUCHERINFO.Vi_id FROM VOUCHERINFO INNER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE ((VOUCHERTYPE.Type)='P Return' AND ((VOUCHERTYPE." + Database.BMode + ")=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ") AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dateTimePicker2.Value.Date.ToString(Database.dformat) + access_sql.Hash + "))"+ branch, dtvou);
                if (dtvou.Rows.Count == 0)
                {
                    MessageBox.Show("No Record found in Purchase Return..");
                }
                else
                {
                    MessageBox.Show("File will be created for Purchase Returns Entered in Marwari S/w");
                    SaveFileDialog savefile = new SaveFileDialog();
                    if (DialogResult.OK == savefile.ShowDialog())
                    {
                        obj.HEADER = new clsHEADER();

                        obj.HEADER.TALLYREQUEST = "Import Data";
                        obj.BODY = new clsBODY();

                        obj.BODY.IMPORTDATA = new clsIMPORTDATA();


                        obj.BODY.IMPORTDATA.REQUESTDESC = new clsREQUESTDESC();
                        obj.BODY.IMPORTDATA.REQUESTDESC.REPORTNAME = "All Masters";
                        //obj.BODY.IMPORTDATA.REQUESTDATA = new clsREQUESTDATA();
                        List<clsTALLYMESSAGE> lstMsg = new List<clsTALLYMESSAGE>();

                        foreach (DataRow dtr in dtvou.Rows)
                        {
                            DataTable dtdata = new DataTable();
                            Database.GetSqlData("SELECT ACCOUNT.Name AS AccountName, Sum(JOURNAL.Amount) AS Amount, JOURNAL.Narr, JOURNAL.Vdate, VOUCHERTYPE.AliasName, VOUCHERINFO.Vnumber FROM ((VOUCHERINFO LEFT JOIN JOURNAL ON VOUCHERINFO.Vi_id = JOURNAL.Vi_id) LEFT JOIN ACCOUNT ON JOURNAL.Ac_id = ACCOUNT.Ac_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id GROUP BY ACCOUNT.Name, JOURNAL.Narr, JOURNAL.Vdate, VOUCHERTYPE.AliasName, VOUCHERINFO.Vnumber, VOUCHERINFO.Vi_id HAVING (((VOUCHERINFO.Vi_id)='" + dtr["Vi_id"].ToString() + "')) ORDER BY Sum(JOURNAL.Amount) desc", dtdata);

                            clsTALLYMESSAGE tm = new clsTALLYMESSAGE();
                            tm.VOUCHER = new clsVOUCHER();
                            tm.VOUCHER.ACTION = "Create";
                            tm.VOUCHER.VCHTYPE = "Debit Note";
                            tm.VOUCHER.DATE = DateTime.Parse(dtdata.Rows[0]["Vdate"].ToString()).ToString("yyyyMMdd");

                            tm.VOUCHER.NARRATION = dtdata.Rows[0]["Narr"].ToString();
                            tm.VOUCHER.VOUCHERTYPENAME = "Debit Note";

                            tm.VOUCHER.VOUCHERNUMBER = dtdata.Rows[0]["Vnumber"].ToString();
                            tm.VOUCHER.PARTYLEDGERNAME = dtdata.Rows[0]["AccountName"].ToString();

                            List<clsALLLEDGERENTRIESLIST> lstItems = new List<clsALLLEDGERENTRIESLIST>();

                            for (int i = 0; i < dtdata.Rows.Count; i++)
                            {
                                clsALLLEDGERENTRIESLIST objItem = new clsALLLEDGERENTRIESLIST();

                                objItem.LEDGERNAME = dtdata.Rows[i]["AccountName"].ToString();

                                objItem.ISDEEMEDPOSITIVE = "Yes";
                                objItem.ISLASTDEEMEDPOSITIVE = "Yes";

                                if (double.Parse(dtdata.Rows[i]["Amount"].ToString()) < 0)
                                {
                                    objItem.ISDEEMEDPOSITIVE = "No";
                                    objItem.ISLASTDEEMEDPOSITIVE = "No";
                                }
                                objItem.AMOUNT = -1 * double.Parse(dtdata.Rows[i]["Amount"].ToString());
                                lstItems.Add(objItem);
                            }

                            tm.VOUCHER.ALLLEDGERENTRIES_LIST = lstItems;
                            lstMsg.Add(tm);
                        }


                        obj.BODY.IMPORTDATA.REQUESTDATA = lstMsg;
                        XmlSerializer SerializerObj = new XmlSerializer(obj.GetType());

                        string path = ".xml";
                        TextWriter WriteFileStream = new StreamWriter(savefile.FileName + path);
                        try
                        {
                            SerializerObj.Serialize(WriteFileStream, obj);
                            MessageBox.Show("XML File for Purchase Return is created on Location : " + savefile.FileName);

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                            MessageBox.Show("XML File is  not created");
                        }
                        WriteFileStream.Close();

                    }
                }

            }

            if (type == "Cnote")
            {

                dtvou = new DataTable();
                Database.GetSqlData("SELECT VOUCHERINFO.Vi_id FROM VOUCHERINFO INNER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE ((VOUCHERTYPE.Type)='Cnote' AND ((VOUCHERTYPE." + Database.BMode + ")=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ") AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dateTimePicker2.Value.Date.ToString(Database.dformat) + access_sql.Hash + ")) "+ branch, dtvou);
                if (dtvou.Rows.Count == 0)
                {
                    MessageBox.Show("No Record found in Credit Notes..");
                }

                else
                {
                    MessageBox.Show("File will be created for Credit Notes Entered in Marwari S/w");
                    SaveFileDialog savefile = new SaveFileDialog();
                    if (DialogResult.OK == savefile.ShowDialog())
                    {

                        obj.HEADER = new clsHEADER();

                        obj.HEADER.TALLYREQUEST = "Import Data";
                        obj.BODY = new clsBODY();

                        obj.BODY.IMPORTDATA = new clsIMPORTDATA();


                        obj.BODY.IMPORTDATA.REQUESTDESC = new clsREQUESTDESC();
                        obj.BODY.IMPORTDATA.REQUESTDESC.REPORTNAME = "All Masters";
                        //obj.BODY.IMPORTDATA.REQUESTDATA = new clsREQUESTDATA();
                        List<clsTALLYMESSAGE> lstMsg = new List<clsTALLYMESSAGE>();

                        foreach (DataRow dtr in dtvou.Rows)
                        {
                            DataTable dtdata = new DataTable();
                            Database.GetSqlData("SELECT ACCOUNT.Name AS AccountName, Sum(JOURNAL.Amount) AS Amount, JOURNAL.Narr, JOURNAL.Vdate, VOUCHERTYPE.AliasName, VOUCHERINFO.Vnumber FROM ((VOUCHERINFO LEFT JOIN JOURNAL ON VOUCHERINFO.Vi_id = JOURNAL.Vi_id) LEFT JOIN ACCOUNT ON JOURNAL.Ac_id = ACCOUNT.Ac_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id GROUP BY ACCOUNT.Name, JOURNAL.Narr, JOURNAL.Vdate, VOUCHERTYPE.AliasName, VOUCHERINFO.Vnumber, VOUCHERINFO.Vi_id HAVING (((VOUCHERINFO.Vi_id)='" + dtr["Vi_id"].ToString() + "')) ORDER BY Sum(JOURNAL.Amount) desc", dtdata);

                            clsTALLYMESSAGE tm = new clsTALLYMESSAGE();
                            tm.VOUCHER = new clsVOUCHER();
                            tm.VOUCHER.ACTION = "Create";
                            tm.VOUCHER.VCHTYPE = "Credit Note";
                            tm.VOUCHER.DATE = DateTime.Parse(dtdata.Rows[0]["Vdate"].ToString()).ToString("yyyyMMdd");

                            tm.VOUCHER.NARRATION = dtdata.Rows[0]["Narr"].ToString();
                            tm.VOUCHER.VOUCHERTYPENAME = "Credit Note";

                            tm.VOUCHER.VOUCHERNUMBER = dtdata.Rows[0]["Vnumber"].ToString();
                            tm.VOUCHER.PARTYLEDGERNAME = dtdata.Rows[0]["AccountName"].ToString();

                            List<clsALLLEDGERENTRIESLIST> lstItems = new List<clsALLLEDGERENTRIESLIST>();

                            for (int i = 0; i < dtdata.Rows.Count; i++)
                            {
                                clsALLLEDGERENTRIESLIST objItem = new clsALLLEDGERENTRIESLIST();

                                objItem.LEDGERNAME = dtdata.Rows[i]["AccountName"].ToString();

                                objItem.ISDEEMEDPOSITIVE = "Yes";
                                objItem.ISLASTDEEMEDPOSITIVE = "Yes";

                                if (double.Parse(dtdata.Rows[i]["Amount"].ToString()) < 0)
                                {
                                    objItem.ISDEEMEDPOSITIVE = "No";
                                    objItem.ISLASTDEEMEDPOSITIVE = "No";
                                }
                                objItem.AMOUNT = -1 * double.Parse(dtdata.Rows[i]["Amount"].ToString());
                                lstItems.Add(objItem);
                            }

                            tm.VOUCHER.ALLLEDGERENTRIES_LIST = lstItems;
                            lstMsg.Add(tm);
                        }


                        obj.BODY.IMPORTDATA.REQUESTDATA = lstMsg;





                        XmlSerializer SerializerObj = new XmlSerializer(obj.GetType());
                        string path = ".xml";




                        TextWriter WriteFileStream = new StreamWriter(savefile.FileName + path);
                        try
                        {
                            SerializerObj.Serialize(WriteFileStream, obj);
                            MessageBox.Show("XML File for Credit Note is created on Location : " + savefile.FileName);

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                            MessageBox.Show("XML File is  not created");
                        }
                        WriteFileStream.Close();

                    }
                }

                dtvou = new DataTable();
                Database.GetSqlData("SELECT VOUCHERINFO.Vi_id FROM VOUCHERINFO INNER JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE ((VOUCHERTYPE.Type)='Return' AND ((VOUCHERTYPE." + Database.BMode + ")=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ") AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dateTimePicker1.Value.Date.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dateTimePicker2.Value.Date.ToString(Database.dformat) + access_sql.Hash + ")) " + branch, dtvou);
                if (dtvou.Rows.Count == 0)
                {
                    MessageBox.Show("No Record found in Sale Return..");
                }
                else
                {
                    MessageBox.Show("File will be created for Sale Returns Entered in Marwari S/w");
                    SaveFileDialog savefile = new SaveFileDialog();
                    if (DialogResult.OK == savefile.ShowDialog())
                    {
                        obj.HEADER = new clsHEADER();

                        obj.HEADER.TALLYREQUEST = "Import Data";
                        obj.BODY = new clsBODY();

                        obj.BODY.IMPORTDATA = new clsIMPORTDATA();


                        obj.BODY.IMPORTDATA.REQUESTDESC = new clsREQUESTDESC();
                        obj.BODY.IMPORTDATA.REQUESTDESC.REPORTNAME = "All Masters";
                        //obj.BODY.IMPORTDATA.REQUESTDATA = new clsREQUESTDATA();
                        List<clsTALLYMESSAGE> lstMsg = new List<clsTALLYMESSAGE>();


                        foreach (DataRow dtr in dtvou.Rows)
                        {
                            DataTable dtdata = new DataTable();
                            Database.GetSqlData("SELECT ACCOUNT.Name AS AccountName, Sum(JOURNAL.Amount) AS Amount, JOURNAL.Narr, JOURNAL.Vdate, VOUCHERTYPE.AliasName, VOUCHERINFO.Vnumber FROM ((VOUCHERINFO LEFT JOIN JOURNAL ON VOUCHERINFO.Vi_id = JOURNAL.Vi_id) LEFT JOIN ACCOUNT ON JOURNAL.Ac_id = ACCOUNT.Ac_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id GROUP BY ACCOUNT.Name, JOURNAL.Narr, JOURNAL.Vdate, VOUCHERTYPE.AliasName, VOUCHERINFO.Vnumber, VOUCHERINFO.Vi_id HAVING (((VOUCHERINFO.Vi_id)='" + dtr["Vi_id"].ToString() + "')) ORDER BY Sum(JOURNAL.Amount) desc", dtdata);

                            clsTALLYMESSAGE tm = new clsTALLYMESSAGE();
                            tm.VOUCHER = new clsVOUCHER();
                            tm.VOUCHER.ACTION = "Create";
                            tm.VOUCHER.VCHTYPE = "Credit Note";
                            tm.VOUCHER.DATE = DateTime.Parse(dtdata.Rows[0]["Vdate"].ToString()).ToString("yyyyMMdd");

                            tm.VOUCHER.NARRATION = dtdata.Rows[0]["Narr"].ToString();
                            tm.VOUCHER.VOUCHERTYPENAME = "Credit Note";

                            tm.VOUCHER.VOUCHERNUMBER = dtdata.Rows[0]["Vnumber"].ToString();
                            tm.VOUCHER.PARTYLEDGERNAME = dtdata.Rows[0]["AccountName"].ToString();

                            List<clsALLLEDGERENTRIESLIST> lstItems = new List<clsALLLEDGERENTRIESLIST>();

                            for (int i = 0; i < dtdata.Rows.Count; i++)
                            {
                                clsALLLEDGERENTRIESLIST objItem = new clsALLLEDGERENTRIESLIST();

                                objItem.LEDGERNAME = dtdata.Rows[i]["AccountName"].ToString();

                                objItem.ISDEEMEDPOSITIVE = "Yes";
                                objItem.ISLASTDEEMEDPOSITIVE = "Yes";

                                if (double.Parse(dtdata.Rows[i]["Amount"].ToString()) < 0)
                                {
                                    objItem.ISDEEMEDPOSITIVE = "No";
                                    objItem.ISLASTDEEMEDPOSITIVE = "No";
                                }
                                objItem.AMOUNT = -1 * double.Parse(dtdata.Rows[i]["Amount"].ToString());
                                lstItems.Add(objItem);
                            }

                            tm.VOUCHER.ALLLEDGERENTRIES_LIST = lstItems;
                            lstMsg.Add(tm);
                        }


                        obj.BODY.IMPORTDATA.REQUESTDATA = lstMsg;

                        XmlSerializer SerializerObj = new XmlSerializer(obj.GetType());

                        string path = ".xml";
                        TextWriter WriteFileStream = new StreamWriter(savefile.FileName + path);
                        try
                        {
                            SerializerObj.Serialize(WriteFileStream, obj);
                            MessageBox.Show("XML File for Sale Return is created on Location : " + savefile.FileName);

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                            MessageBox.Show("XML File is  not created");
                        }
                        WriteFileStream.Close();

                    }
                }

            }
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
    }
}
