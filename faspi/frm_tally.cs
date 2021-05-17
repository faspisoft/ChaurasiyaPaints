using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;

namespace faspi
{
    public partial class frm_tally : Form
    {
        ENVELOPE obj = new ENVELOPE();

        public string createledger = "";
        public frm_tally()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            if (DialogResult.OK == savefile.ShowDialog())
            {
                DataTable dt = new DataTable();

                obj.HEADER = new clsHEADER();

                obj.HEADER.TALLYREQUEST = "Import Data";
                obj.BODY = new clsBODY();

                obj.BODY.IMPORTDATA = new clsIMPORTDATA();


                obj.BODY.IMPORTDATA.REQUESTDESC = new clsREQUESTDESC();
                obj.BODY.IMPORTDATA.REQUESTDESC.REPORTNAME = "All Masters";


                List<clsTALLYMESSAGE> lstMsg = new List<clsTALLYMESSAGE>();
                Database.GetSqlData("SELECT ACCOUNTYPE.Name, ACCOUNTYPE_1.Name AS under FROM ACCOUNTYPE LEFT JOIN ACCOUNTYPE AS ACCOUNTYPE_1 ON ACCOUNTYPE.Under = ACCOUNTYPE_1.Act_id WHERE (((ACCOUNTYPE.Type)='Account') AND ((ACCOUNTYPE.Fixed)=" + access_sql.Singlequote + "False" + access_sql.Singlequote + ")) ORDER BY ACCOUNTYPE.Name;", dt);





                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No New Record Found..");
                    return;
                }


                foreach (DataRow dtr in dt.Rows)
                {
                    clsTALLYMESSAGE tm = new clsTALLYMESSAGE();
                    tm.GROUP = new clsGROUP();
                    tm.GROUP.RESERVEDNAME = dtr["Name"].ToString();

                    tm.GROUP.NAME = dtr["Name"].ToString();
                    if (dtr["under"].ToString().ToUpper() == "EXPENDITURE ACCOUNT (DIRECT)")
                    {
                        dtr["under"] = "Expenses (Direct)";
                    }
                    else if (dtr["under"].ToString().ToUpper() == "EXPENDITURE ACCOUNT (INDIRECT )")
                    {
                        dtr["under"] = "Expenses (Indirect)";
                    }
                    else if (dtr["under"].ToString().ToUpper() == "SECURE LOANS")
                    {
                        dtr["under"] = "Secured Loans";
                    }
                    else if (dtr["under"].ToString().ToUpper() == "SECURITY & DEPOSITS (ASSETS)")
                    {
                        dtr["under"] = "Deposits (Asset)";
                    }
                    else if (dtr["under"].ToString().ToUpper() == "SUSPENSE ACCOUNT (TEMPORARY A/C)")
                    {
                        dtr["under"] = "Suspense A/c";
                    }


                    else if (dtr["under"].ToString().ToUpper() == "PURCHASE ACCOUNTS")
                    {
                        dtr["under"] = "Purchase Accounts";
                    }
                    else if (dtr["under"].ToString().ToUpper() == "SALES ACCOUNTS")
                    {
                        dtr["under"] = "Sales Accounts";
                    }
                    else if (dtr["under"].ToString().ToUpper() == "CAPITAL ACCOUNT")
                    {
                        dtr["under"] = "Capital Account";
                    }
                    else if (dtr["under"].ToString().ToUpper() == "UNSECURE LOANS")
                    {
                        dtr["under"] = "Unsecured Loans";
                    }
                    else if (dtr["under"].ToString().ToUpper() == "LOAN & ADVANCES (ASSESTS)")
                    {
                        dtr["under"] = "Loans & Advances (Asset)";
                    }
                    else if (dtr["under"].ToString().ToUpper() == "LOAN (LIABILITIES)")
                    {
                        dtr["under"] = "Loans (Liability)";
                    }


                    tm.GROUP.PARENT = dtr["under"].ToString();
                    tm.GROUP.ISSUBLEDGER = "No";
                    tm.GROUP.ISBILLWISEON = "No";
                    tm.GROUP.ISCOSTCENTRESON = "No";


                    tm.GROUP.LANGUAGENAME_LIST = new clsLANGUAGENAMELIST();

                    tm.GROUP.LANGUAGENAME_LIST.NAME_LIST = new clsNAMELISTGrp();
                    tm.GROUP.LANGUAGENAME_LIST.NAME_LIST.TYPE = "String";
                    tm.GROUP.LANGUAGENAME_LIST.NAME_LIST.NAME = dtr["Name"].ToString();

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

      

        private void frm_tally_Load(object sender, EventArgs e)
        {
            int count = Database.GetScalarInt("Select Count(*) from Account");
            label4.Text = count.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            if (DialogResult.OK == savefile.ShowDialog())
            {
                DataTable dt = new DataTable();

                obj.HEADER = new clsHEADER();

                obj.HEADER.TALLYREQUEST = "Import Data";
                obj.BODY = new clsBODY();

                obj.BODY.IMPORTDATA = new clsIMPORTDATA();


                obj.BODY.IMPORTDATA.REQUESTDESC = new clsREQUESTDESC();
                obj.BODY.IMPORTDATA.REQUESTDESC.REPORTNAME = "All Masters";

                List<clsTALLYMESSAGE> lstMsg = new List<clsTALLYMESSAGE>();
                string branch = "";
                if (Feature.Available("Export Vouchers in Tally").ToUpper() != "ALL")
                {
                    branch = " where Branch_id='" + Database.BranchId + "'";
                }

                if (Database.BMode == "A")
                {

                    Database.GetSqlData("SELECT ACCOUNT.Name, ACCOUNTYPE.Name as ActType, ACCOUNT.Balance AS Bal, State.Sname,ACCOUNT.TIN_number,RegStatus FROM (ACCOUNT INNER JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id) LEFT JOIN State ON ACCOUNT.State_id = State.State_id "+ branch +" ORDER BY ACCOUNT.Name", dt);
                }
                else if (Database.BMode == "B")
                {

                    Database.GetSqlData("SELECT ACCOUNT.Name, ACCOUNTYPE.Name as ActType, ACCOUNT.Balance2 AS Bal, State.Sname,ACCOUNT.TIN_number,RegStatus FROM (ACCOUNT INNER JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id) LEFT JOIN State ON ACCOUNT.State_id = State.State_id " + branch + " ORDER BY ACCOUNT.Name", dt);
                }
                else if (Database.BMode == "AB")
                {

                    Database.GetSqlData("SELECT ACCOUNT.Name, ACCOUNTYPE.Name as ActType, ACCOUNT.Balance+ACCOUNT.Balance2  AS Bal, State.Sname,ACCOUNT.TIN_number,RegStatus FROM (ACCOUNT INNER JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id) LEFT JOIN State ON ACCOUNT.State_id = State.State_id " + branch + " ORDER BY ACCOUNT.Name", dt);
                }
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No Record Found..");
                    return;
                }
                foreach (DataRow dtr in dt.Rows)
                {

                    clsTALLYMESSAGE tm = new clsTALLYMESSAGE();
                    tm.LEDGER = new clsLEDGER();

                    tm.LEDGER.ACTION = "Create";
                    tm.LEDGER.NAME = dtr["Name"].ToString();
                    tm.LEDGER.NAME_LIST = new clsNAMELIST();

                    tm.LEDGER.NAME_LIST.NAME = dtr["Name"].ToString();
                    if (dtr["RegStatus"].ToString().ToUpper() == "REGULAR REGISTRATION")
                    {
                        dtr["RegStatus"] = "Regular";
                    }
                    else if (dtr["RegStatus"].ToString().ToUpper() == "COMPOSITION DEALER")
                    {
                        dtr["RegStatus"] = "Composition";
                    }
                    tm.LEDGER.GSTREGISTRATIONTYPE = dtr["RegStatus"].ToString();
                    if (dtr["ACTType"].ToString().ToUpper() == "EXPENDITURE ACCOUNT (DIRECT)")
                    {
                        dtr["ACTType"] = "Expenses (Direct)";
                    }
                    else if (dtr["ACTType"].ToString().ToUpper() == "EXPENDITURE ACCOUNT (INDIRECT )")
                    {
                        dtr["ACTType"] = "Expenses (Indirect)";
                    }
                    else if (dtr["ACTType"].ToString().ToUpper() == "SECURE LOANS")
                    {
                        dtr["ACTType"] = "Secured Loans";
                    }
                    else if (dtr["ACTType"].ToString().ToUpper() == "SECURITY & DEPOSITS (ASSETS)")
                    {
                        dtr["ACTType"] = "Deposits (Asset)";
                    }
                    else if (dtr["ACTType"].ToString().ToUpper() == "SUSPENSE ACCOUNT (TEMPORARY A/C)")
                    {
                        dtr["ACTType"] = "Suspense A/c";
                    }


                    else if (dtr["ACTType"].ToString().ToUpper() == "PURCHASE ACCOUNTS")
                    {
                        dtr["ACTType"] = "Purchase Accounts";
                    }
                    else if (dtr["ACTType"].ToString().ToUpper() == "SALES ACCOUNTS")
                    {
                        dtr["ACTType"] = "Sales Accounts";
                    }
                    else if (dtr["ACTType"].ToString().ToUpper() == "CAPITAL ACCOUNT")
                    {
                        dtr["ACTType"] = "Capital Account";
                    }
                    else if (dtr["ACTType"].ToString().ToUpper() == "UNSECURE LOANS")
                    {
                        dtr["ACTType"] = "Unsecured Loans";
                    }
                    else if (dtr["ACTType"].ToString().ToUpper() == "LOAN & ADVANCES (ASSESTS)")
                    {
                        dtr["ACTType"] = "Loans & Advances (Asset)";
                    }
                    else if (dtr["ACTType"].ToString().ToUpper() == "LOAN (LIABILITIES)")
                    {
                        dtr["ACTType"] = "Loans (Liability)";
                    }


                    tm.LEDGER.PARENT = dtr["ActType"].ToString();
                    tm.LEDGER.PARTYGSTIN = dtr["TIN_number"].ToString();
                    tm.LEDGER.LEDSTATENAME = dtr["Sname"].ToString();
                    tm.LEDGER.ISBILLWISEON = "No";
                    tm.LEDGER.AFFECTSSTOCK = "No";
                 
                    tm.LEDGER.OPENINGBALANCE = -1*double.Parse(dtr["bal"].ToString());

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
}
