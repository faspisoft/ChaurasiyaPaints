using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Net;
using System.IO;
namespace faspi
{
    class Tally
    {
        private string EscapeSequence(string str)
        {
            str = str.Replace("&", "&amp;");
            str = str.Replace("'", "&apos;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("<", "&lt;");
            return str;
        }

        public string CreateGroup(string GroupName, String UnderGroup)
        {
            string xmlstc = "";
            GroupName = EscapeSequence(GroupName);
            UnderGroup = EscapeSequence(UnderGroup);
            try
            {
                string xmlHeader = "<ENVELOPE><HEADER><TALLYREQUEST>Import Data</TALLYREQUEST></HEADER><BODY><IMPORTDATA><REQUESTDESC><REPORTNAME>All Masters</REPORTNAME></REQUESTDESC><REQUESTDATA>";

                string xmlCommand = "<TALLYMESSAGE xmlns:UDF='TallyUDF'><GROUP NAME='" + GroupName + "' ACTION='Create'><NAME.LIST><NAME>" + GroupName + "</NAME></NAME.LIST><PARENT>" + UnderGroup + "</PARENT><ISSUBLEDGER>No</ISSUBLEDGER><ISBILLWISEON>No</ISBILLWISEON><ISCOSTCENTRESON>No</ISCOSTCENTRESON></GROUP></TALLYMESSAGE>";

                string xmlFooter = "</REQUESTDATA></IMPORTDATA></BODY></ENVELOPE>";

                xmlstc = xmlHeader + xmlCommand + xmlFooter;
                return xmlstc;
            }
            catch (Exception ex)
            {
                return xmlstc;
            }
        }

        public string CreateLedger(int from, int to)
        {
            DataTable dtAcc = new DataTable();
            string branch = "";

            if (Feature.Available("Export Vouchers in Tally").ToUpper() != "ALL")
            {
                branch= " where Branch_id='"+ Database.BranchId+"'";
            }




            if (Database.BMode == "A")
            {
                Database.GetSqlData("SELECT ACCOUNT.Name, ACCOUNTYPE.Name, Balance AS Bal,State.Sname,ACCOUNT.TIN_number,RegStatus FROM (ACCOUNT INNER JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id) LEFT JOIN State ON ACCOUNT.State_id = State.State_id  " + branch + " ORDER BY ACCOUNT.Name", dtAcc);
            }
            else if (Database.BMode == "B")
            {
                Database.GetSqlData("SELECT ACCOUNT.Name, ACCOUNTYPE.Name, Balance2 AS Bal,State.Sname,ACCOUNT.TIN_number,RegStatus FROM (ACCOUNT INNER JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id ) LEFT JOIN State ON ACCOUNT.State_id = State.State_id " + branch + " ORDER BY ACCOUNT.Name", dtAcc);
            }
            else if (Database.BMode == "AB")
            {
                Database.GetSqlData("SELECT ACCOUNT.Name, ACCOUNTYPE.Name, Balance+Balance2 AS Bal,State.Sname,ACCOUNT.TIN_number,RegStatus FROM (ACCOUNT INNER JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id) LEFT JOIN State ON ACCOUNT.State_id = State.State_id " + branch + " ORDER BY ACCOUNT.Name", dtAcc);
            }

            string xmlHeader = "<ENVELOPE><HEADER><TALLYREQUEST>Import Data</TALLYREQUEST></HEADER><BODY><IMPORTDATA><REQUESTDESC><REPORTNAME>All Masters</REPORTNAME></REQUESTDESC><REQUESTDATA>";


            string xmlCommand = "";
            from = from - 1;
            if (to > dtAcc.Rows.Count)
            {
                to = dtAcc.Rows.Count;
            }

            for (int i = from; i < to; i++)
            {
                if (dtAcc.Rows[i]["RegStatus"].ToString().ToUpper() == "REGULAR REGISTRATION")
                {
                    dtAcc.Rows[i]["RegStatus"] = "Regular";
                }
                else if (dtAcc.Rows[i]["RegStatus"].ToString().ToUpper() == "COMPOSITION DEALER")
                {
                    dtAcc.Rows[i]["RegStatus"] = "Composition";
                }
                if (dtAcc.Rows[i][1].ToString() == "EXPENDITURE ACCOUNT (Direct)")
                {
                    dtAcc.Rows[i][1] = "Expenses (Direct)";
                }
                else if (dtAcc.Rows[i][1].ToString() == "EXPENDITURE ACCOUNT (Indirect )")
                {
                    dtAcc.Rows[i][1] = "Expenses (Indirect)";
                }
                else if (dtAcc.Rows[i][1].ToString() == "SECURE LOANS")
                {
                    dtAcc.Rows[i][1] = "Secured Loans";
                }
                else if (dtAcc.Rows[i][1].ToString() == "SECURITY & DEPOSITS (Assets)")
                {
                    dtAcc.Rows[i][1] = "Deposits (Asset)";
                }
                else if (dtAcc.Rows[i][1].ToString() == "SUSPENSE ACCOUNT (Temporary A/C)")
                {
                    dtAcc.Rows[i][1] = "Suspense A/c";
                }
                else if (dtAcc.Rows[i][1].ToString() == "PURCHASE ACCOUNTS")
                {
                    dtAcc.Rows[i][1] = "Purchase Accounts";
                }
                else if (dtAcc.Rows[i][1].ToString() == "SALES ACCOUNTS")
                {
                    dtAcc.Rows[i][1] = "Sales Accounts";
                }
                else if (dtAcc.Rows[i][1].ToString() == "CAPITAL ACCOUNT")
                {
                    dtAcc.Rows[i][1] = "Capital Account";
                }
                else if (dtAcc.Rows[i][1].ToString() == "UNSECURE LOANS")
                {
                    dtAcc.Rows[i][1] = "Unsecured Loans";
                }
                else if (dtAcc.Rows[i][1].ToString() == "LOAN & ADVANCES (Assests)")
                {
                    dtAcc.Rows[i][1] = "Loans & Advances (Asset)";
                }
                xmlCommand += "<TALLYMESSAGE xmlns:UDF='TallyUDF'><LEDGER NAME='" + dtAcc.Rows[i][0].ToString() + "' ACTION='Create'><NAME.LIST><NAME>" + dtAcc.Rows[i][0].ToString() + "</NAME></NAME.LIST> <GSTREGISTRATIONTYPE>" + dtAcc.Rows[i]["RegStatus"].ToString() + "</GSTREGISTRATIONTYPE>  <PARENT>" + dtAcc.Rows[i][1].ToString() + "</PARENT><PARTYGSTIN>" + dtAcc.Rows[i]["Tin_Number"].ToString() + "</PARTYGSTIN><LEDSTATENAME>" + dtAcc.Rows[i]["Sname"].ToString() + "</LEDSTATENAME><ISBILLWISEON>No</ISBILLWISEON><AFFECTSSTOCK>No</AFFECTSSTOCK><OPENINGBALANCE>" + -1 * double.Parse(dtAcc.Rows[i][2].ToString()) + "</OPENINGBALANCE><USEFORVAT>No </USEFORVAT><TAXCLASSIFICATIONNAME/><TAXTYPE/><RATEOFTAXCALCULATION/></LEDGER></TALLYMESSAGE>";
               // xmlCommand += "<TALLYMESSAGE xmlns:UDF='TallyUDF'><LEDGER NAME='" + dtAcc.Rows[i][0].ToString() + "' ACTION='Create'><NAME.LIST><NAME>" + dtAcc.Rows[i][0].ToString() + "</NAME></NAME.LIST><PARENT>" + dtAcc.Rows[i][1].ToString() + "</PARENT><ISBILLWISEON>No</ISBILLWISEON><AFFECTSSTOCK>No</AFFECTSSTOCK><OPENINGBALANCE>" + double.Parse(dtAcc.Rows[i][2].ToString()) + "</OPENINGBALANCE><USEFORVAT>No </USEFORVAT><TAXCLASSIFICATIONNAME/><TAXTYPE/><RATEOFTAXCALCULATION/></LEDGER></TALLYMESSAGE>";
            }

            string xmlFooter = "</REQUESTDATA></IMPORTDATA></BODY></ENVELOPE>";

            string xmlstc = xmlHeader + xmlCommand + xmlFooter;

            return xmlstc;



        }




        public bool CreateUnit(string UnitName)
        {

            System.IO.StreamWriter streamWriter;
            try
            {

                string xmlHeader = "<ENVELOPE><HEADER><TALLYREQUEST>Import Data</TALLYREQUEST></HEADER><BODY><IMPORTDATA><REQUESTDESC><REPORTNAME>All Masters</REPORTNAME></REQUESTDESC><REQUESTDATA>";

                string xmlCommand = "<TALLYMESSAGE xmlns:UDF='TallyUDF'><UNIT NAME='" + UnitName + "' ACTION='CREATE'><NAME>" + UnitName + "</NAME><ISSIMPLEUNIT>Yes</ISSIMPLEUNIT><FORPAYROLL>No</FORPAYROLL></UNIT></TALLYMESSAGE>";

                string xmlFooter = "</REQUESTDATA></IMPORTDATA></BODY></ENVELOPE>";

                string xmlstc = xmlHeader + xmlCommand + xmlFooter;

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:9000");
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentLength = xmlstc.Length;
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
                streamWriter.Write(xmlstc);
                streamWriter.Close();
                return true;
            }
            catch (Exception ex)
            {


                return false;
            }



        }


        public bool CreateStockGroup(string StockGroup)
        {

            System.IO.StreamWriter streamWriter;
            try
            {

                string xmlHeader = "<ENVELOPE><HEADER><TALLYREQUEST>Import Data</TALLYREQUEST></HEADER><BODY><IMPORTDATA><REQUESTDESC><REPORTNAME>All Masters</REPORTNAME></REQUESTDESC><REQUESTDATA>";

                string xmlCommand = "<TALLYMESSAGE xmlns:UDF='TallyUDF'><STOCKGROUP NAME='" + StockGroup + "' ACTION='Create'><NAME.LIST><NAME>" + StockGroup + "</NAME></NAME.LIST><PARENT/><ISADDABLE>Yes</ISADDABLE></STOCKGROUP></TALLYMESSAGE>";

                string xmlFooter = "</REQUESTDATA></IMPORTDATA></BODY></ENVELOPE>";

                string xmlstc = xmlHeader + xmlCommand + xmlFooter;

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:9000");
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentLength = xmlstc.Length;
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";


                streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
                streamWriter.Write(xmlstc);
                streamWriter.Close();
                return true;
            }
            catch (Exception ex)
            {


                return false;
            }



        }

        public bool CreateItem(string ItemName, string UnderGuorp, string BaseUnit)
        {

            System.IO.StreamWriter streamWriter;
            try
            {

                string xmlHeader = "<ENVELOPE><HEADER><TALLYREQUEST>Import Data</TALLYREQUEST></HEADER><BODY><IMPORTDATA><REQUESTDESC><REPORTNAME>All Masters</REPORTNAME></REQUESTDESC><REQUESTDATA>";

                string xmlCommand = "<TALLYMESSAGE xmlns:UDF='TallyUDF'><STOCKITEM NAME='" + ItemName + "' ACTION='Create'><NAME.LIST><NAME>" + ItemName + "</NAME></NAME.LIST><ADDITIONALNAME.LIST><ADDITIONALNAME>0010817824</ADDITIONALNAME></ADDITIONALNAME.LIST><PARENT>" + UnderGuorp + "</PARENT><BASEUNITS>" + BaseUnit + "</BASEUNITS><OPENINGBALANCE>0.000 NOS</OPENINGBALANCE><OPENINGVALUE>0.000</OPENINGVALUE><OPENINGRATE>0.000/NOS</OPENINGRATE><BATCHALLOCATIONS.LIST><NAME>Primary Batch</NAME><BATCHNAME>Primary Batch</BATCHNAME><GODOWNNAME>Main Location</GODOWNNAME><MFDON>20130331</MFDON><OPENINGBALANCE>0.000 NOS</OPENINGBALANCE><OPENINGVALUE>0.000</OPENINGVALUE><OPENINGRATE>0.000/NOS</OPENINGRATE></BATCHALLOCATIONS.LIST><STANDARDPRICELIST.LIST><RATE>100.000</RATE><DATE>20120401</DATE></STANDARDPRICELIST.LIST><STANDARDCOSTLIST.LIST><RATE>80.000</RATE><DATE>20120401</DATE></STANDARDCOSTLIST.LIST><REORDERBASE>0.000</REORDERBASE><MINIMUMORDERBASE>0.000</MINIMUMORDERBASE></STOCKITEM></TALLYMESSAGE>";

                string xmlFooter = "</REQUESTDATA></IMPORTDATA></BODY></ENVELOPE>";

                string xmlstc = xmlHeader + xmlCommand + xmlFooter;

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:9000");
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentLength = xmlstc.Length;
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";


                streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
                streamWriter.Write(xmlstc);
                streamWriter.Close();
                return true;
            }
            catch (Exception ex)
            {


                return false;
            }



        }

        public string CreateVoucher(DataTable dt, string type)
        {
            string xmlstc = "";
            string xmlHeader = "";
            string xmlVou = "";
            string xmlVouHeader = "";
            string xmlVouFooter = "";
            string xmlFooter = "";
            xmlHeader = "<ENVELOPE> <HEADER><TALLYREQUEST>Import Data</TALLYREQUEST> </HEADER> <BODY>  <IMPORTDATA>   <REQUESTDESC>    <REPORTNAME>Vouchers</REPORTNAME>   </REQUESTDESC>   <REQUESTDATA>";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataTable dtdet = new DataTable();
                if (type == "Receipt" || type == "Purchase")
                {
                    Database.GetSqlData("SELECT ACCOUNT.Name as AccountName, JOURNAL.Amount AS Amount, JOURNAL.Narr, JOURNAL.Vdate, VOUCHERTYPE.AliasName, VOUCHERINFO.Vnumber FROM ((VOUCHERINFO LEFT JOIN JOURNAL ON VOUCHERINFO.Vi_id = JOURNAL.Vi_id) LEFT JOIN ACCOUNT ON JOURNAL.Ac_id = ACCOUNT.Ac_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERINFO.Vi_id)='" + dt.Rows[i]["Vi_id"].ToString() + "')) ORDER BY JOURNAL.Amount", dtdet);
                }
                else if (type == "Payment" || type == "Sales")
                {
                    Database.GetSqlData("SELECT ACCOUNT.Name as AccountName, JOURNAL.Amount AS Amount, JOURNAL.Narr, JOURNAL.Vdate, VOUCHERTYPE.AliasName, VOUCHERINFO.Vnumber FROM ((VOUCHERINFO LEFT JOIN JOURNAL ON VOUCHERINFO.Vi_id = JOURNAL.Vi_id) LEFT JOIN ACCOUNT ON JOURNAL.Ac_id = ACCOUNT.Ac_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERINFO.Vi_id)='" + dt.Rows[i]["Vi_id"].ToString() + "')) ORDER BY JOURNAL.Amount DESC", dtdet);
                }
                else if (type == "Journal" || type == "Credit Note")
                {
                    Database.GetSqlData("SELECT ACCOUNT.Name as AccountName, JOURNAL.Amount AS Amount, JOURNAL.Narr, JOURNAL.Vdate, VOUCHERTYPE.AliasName, VOUCHERINFO.Vnumber FROM ((VOUCHERINFO LEFT JOIN JOURNAL ON VOUCHERINFO.Vi_id = JOURNAL.Vi_id) LEFT JOIN ACCOUNT ON JOURNAL.Ac_id = ACCOUNT.Ac_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERINFO.Vi_id)='" + dt.Rows[i]["Vi_id"].ToString() + "')) ORDER BY JOURNAL.Amount", dtdet);
                }
                else if (type == "Debit Note" || type == "Contra")
                {
                    Database.GetSqlData("SELECT ACCOUNT.Name as AccountName, JOURNAL.Amount AS Amount, JOURNAL.Narr, JOURNAL.Vdate, VOUCHERTYPE.AliasName, VOUCHERINFO.Vnumber FROM ((VOUCHERINFO LEFT JOIN JOURNAL ON VOUCHERINFO.Vi_id = JOURNAL.Vi_id) LEFT JOIN ACCOUNT ON JOURNAL.Ac_id = ACCOUNT.Ac_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERINFO.Vi_id)='" + dt.Rows[i]["Vi_id"].ToString() + "')) ORDER BY JOURNAL.Amount DESC", dtdet);
                }

                DateTime Vdate = DateTime.Parse(dtdet.Rows[0]["Vdate"].ToString());
                xmlVou = "";
                for (int j = 0; j < dtdet.Rows.Count; j++)
                {
                    if (j == 0)
                    {
                        if (type == "Sales" || type == "Purchase")
                        {
                            xmlVouHeader = "<TALLYMESSAGE xmlns:UDF='TallyUDF'>  <VOUCHER  VCHTYPE='" + type + "' ACTION='Create'><DATE>" + Vdate.ToString("yyyyMMdd") + "</DATE> <GUID></GUID> <VOUCHERTYPENAME>" + type + "</VOUCHERTYPENAME> <VOUCHERNUMBER>" + dtdet.Rows[j]["Vnumber"].ToString() + "</VOUCHERNUMBER>      <PARTYLEDGERNAME>" + dtdet.Rows[j]["AccountName"].ToString() + "</PARTYLEDGERNAME> <PARTYNAME>" + dtdet.Rows[j]["AccountName"].ToString() + "</PARTYNAME> <BASICBASEPARTYNAME>" + dtdet.Rows[j]["AccountName"].ToString() + "</BASICBASEPARTYNAME> <CSTFORMISSUETYPE/> <CSTFORMRECVTYPE/> <FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE> <BASICBUYERNAME>AABAAD KHAN RAMLEELA TILLA</BASICBUYERNAME> <BASICDATETIMEOFINVOICE>" + Vdate.ToString("dd-MMM-yyyy") + " at " + Vdate.ToString("hh:ss") + "</BASICDATETIMEOFINVOICE> <BASICDATETIMEOFREMOVAL>" + Vdate.ToString("dd-MMM-yyyy") + " at " + Vdate.ToString("hh:ss") + "</BASICDATETIMEOFREMOVAL> <VCHGSTCLASS/> <DIFFACTUALQTY>No</DIFFACTUALQTY> <AUDITED>No</AUDITED> <FORJOBCOSTING>No</FORJOBCOSTING> <ISOPTIONAL>No</ISOPTIONAL> <EFFECTIVEDATE>" + Vdate.ToString("yyyyMMdd") + "</EFFECTIVEDATE> <USEFORINTEREST>No</USEFORINTEREST> <USEFORGAINLOSS>No</USEFORGAINLOSS> <USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER> <USEFORCOMPOUND>No</USEFORCOMPOUND> <ALTERID> 27</ALTERID>";
                        }
                        else if (type == "Receipt" || type == "Payment")
                        {
                            xmlVouHeader = "<TALLYMESSAGE xmlns:UDF='TallyUDF'>  <VOUCHER  VCHTYPE='" + type + "' ACTION='Create'><DATE>" + Vdate.ToString("yyyyMMdd") + "</DATE> <GUID></GUID> <VOUCHERTYPENAME>" + type + "</VOUCHERTYPENAME> <VOUCHERNUMBER>" + dtdet.Rows[j]["Vnumber"].ToString() + "</VOUCHERNUMBER>      <PARTYLEDGERNAME>" + dtdet.Rows[j]["AccountName"].ToString() + "</PARTYLEDGERNAME> <CSTFORMISSUETYPE/> <CSTFORMRECVTYPE/> <FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE> <VCHGSTCLASS/> <DIFFACTUALQTY>No</DIFFACTUALQTY> <AUDITED>No</AUDITED> <FORJOBCOSTING>No</FORJOBCOSTING> <ISOPTIONAL>No</ISOPTIONAL> <EFFECTIVEDATE>" + Vdate.ToString("yyyyMMdd") + "</EFFECTIVEDATE> <USEFORINTEREST>No</USEFORINTEREST> <USEFORGAINLOSS>No</USEFORGAINLOSS> <USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER> <USEFORCOMPOUND>No</USEFORCOMPOUND> <ALTERID> 27</ALTERID>";
                        }
                        else if (type == "Journal" || type == "Contra" || type == "Debit Note" || type == "Credit Note")
                        {
                            xmlVouHeader = "<TALLYMESSAGE xmlns:UDF='TallyUDF'>  <VOUCHER  VCHTYPE='" + type + "' ACTION='Create'><DATE>" + Vdate.ToString("yyyyMMdd") + "</DATE> <GUID></GUID> <VOUCHERTYPENAME>" + type + "</VOUCHERTYPENAME> <VOUCHERNUMBER>" + dtdet.Rows[j]["Vnumber"].ToString() + "</VOUCHERNUMBER>      <PARTYLEDGERNAME>" + dtdet.Rows[j]["AccountName"].ToString() + "</PARTYLEDGERNAME> <CSTFORMISSUETYPE/> <CSTFORMRECVTYPE/> <FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE> <VCHGSTCLASS/> <DIFFACTUALQTY>No</DIFFACTUALQTY> <AUDITED>No</AUDITED> <FORJOBCOSTING>No</FORJOBCOSTING> <ISOPTIONAL>No</ISOPTIONAL> <EFFECTIVEDATE>" + Vdate.ToString("yyyyMMdd") + "</EFFECTIVEDATE> <USEFORINTEREST>No</USEFORINTEREST> <USEFORGAINLOSS>No</USEFORGAINLOSS> <USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER> <USEFORCOMPOUND>No</USEFORCOMPOUND> <ALTERID> 27</ALTERID><EXCISEOPENING>No</EXCISEOPENING><USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION><ISCANCELLED>No</ISCANCELLED><HASCASHFLOW>No</HASCASHFLOW><ISPOSTDATED>No</ISPOSTDATED><USETRACKINGNUMBER>No</USETRACKINGNUMBER><ISINVOICE>No</ISINVOICE><MFGJOURNAL>No</MFGJOURNAL><HASDISCOUNTS>No</HASDISCOUNTS><ASPAYSLIP>No</ASPAYSLIP><ISCOSTCENTRE>No</ISCOSTCENTRE><ISDELETED>No</ISDELETED><ASORIGINAL>No</ASORIGINAL>";
                        }
                        if (type == "Purchase" || type == "Receipt")
                        {
                            xmlVouHeader += "<EXCISEOPENING>No</EXCISEOPENING> <USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION> <ISCANCELLED>No</ISCANCELLED> <HASCASHFLOW>Yes</HASCASHFLOW> <ISPOSTDATED>No</ISPOSTDATED> <USETRACKINGNUMBER>No</USETRACKINGNUMBER> <ISINVOICE>No</ISINVOICE> <MFGJOURNAL>No</MFGJOURNAL> <HASDISCOUNTS>No</HASDISCOUNTS> <ASPAYSLIP>No</ASPAYSLIP> <ISCOSTCENTRE>No</ISCOSTCENTRE> <ISDELETED>No</ISDELETED> <ASORIGINAL>No</ASORIGINAL> <ALLLEDGERENTRIES.LIST><LEDGERNAME>" + dtdet.Rows[j]["AccountName"].ToString() + "</LEDGERNAME><GSTCLASS/><ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE><LEDGERFROMITEM>No</LEDGERFROMITEM> <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES><ISPARTYLEDGER>Yes</ISPARTYLEDGER> <AMOUNT>" + (-1 * double.Parse(dtdet.Rows[j]["Amount"].ToString())) + " </AMOUNT>       </ALLLEDGERENTRIES.LIST>";
                        }
                        else if (type == "Sales" || type == "Payment")
                        {
                            xmlVouHeader += "<EXCISEOPENING>No</EXCISEOPENING> <USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION> <ISCANCELLED>No</ISCANCELLED> <HASCASHFLOW>Yes</HASCASHFLOW> <ISPOSTDATED>No</ISPOSTDATED> <USETRACKINGNUMBER>No</USETRACKINGNUMBER> <ISINVOICE>No</ISINVOICE> <MFGJOURNAL>No</MFGJOURNAL> <HASDISCOUNTS>No</HASDISCOUNTS> <ASPAYSLIP>No</ASPAYSLIP> <ISCOSTCENTRE>No</ISCOSTCENTRE> <ISDELETED>No</ISDELETED> <ASORIGINAL>No</ASORIGINAL> <ALLLEDGERENTRIES.LIST><LEDGERNAME>" + dtdet.Rows[j]["AccountName"].ToString() + "</LEDGERNAME><GSTCLASS/><ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE><LEDGERFROMITEM>No</LEDGERFROMITEM> <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES><ISPARTYLEDGER>Yes</ISPARTYLEDGER> <AMOUNT>" + (-1 * double.Parse(dtdet.Rows[j]["Amount"].ToString())) + " </AMOUNT>       </ALLLEDGERENTRIES.LIST>";
                        }
                        else if (type == "Journal")
                        {
                            xmlVouHeader += "<ALLLEDGERENTRIES.LIST><LEDGERNAME>" + dtdet.Rows[j]["AccountName"].ToString() + "</LEDGERNAME><GSTCLASS/><ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE><LEDGERFROMITEM>No</LEDGERFROMITEM> <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES><ISPARTYLEDGER>Yes</ISPARTYLEDGER> <AMOUNT>" + (-1 * double.Parse(dtdet.Rows[j]["Amount"].ToString())) + "</AMOUNT>       </ALLLEDGERENTRIES.LIST>";
                        }
                        else if (type == "Debit Note")
                        {
                            xmlVouHeader += "<ALLLEDGERENTRIES.LIST><LEDGERNAME>" + dtdet.Rows[j]["AccountName"].ToString() + "</LEDGERNAME><GSTCLASS/><ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE><LEDGERFROMITEM>No</LEDGERFROMITEM> <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES><ISPARTYLEDGER>Yes</ISPARTYLEDGER> <AMOUNT>" + (-1 * double.Parse(dtdet.Rows[j]["Amount"].ToString())) + "</AMOUNT>       </ALLLEDGERENTRIES.LIST>";
                        }
                        else if (type == "Credit Note")
                        {
                            xmlVouHeader += "<ALLLEDGERENTRIES.LIST><LEDGERNAME>" + dtdet.Rows[j]["AccountName"].ToString() + "</LEDGERNAME><GSTCLASS/><ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE><LEDGERFROMITEM>No</LEDGERFROMITEM> <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES><ISPARTYLEDGER>Yes</ISPARTYLEDGER> <AMOUNT>" + (-1 * double.Parse(dtdet.Rows[j]["Amount"].ToString())) + "</AMOUNT>       </ALLLEDGERENTRIES.LIST>";
                        }
                        else if (type == "Contra")
                        {
                            xmlVouHeader += "<ALLLEDGERENTRIES.LIST><LEDGERNAME>" + dtdet.Rows[j]["AccountName"].ToString() + "</LEDGERNAME><GSTCLASS/><ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE><LEDGERFROMITEM>No</LEDGERFROMITEM> <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES><ISPARTYLEDGER>Yes</ISPARTYLEDGER> <AMOUNT>" + double.Parse(dtdet.Rows[j]["Amount"].ToString()) + "</AMOUNT>       </ALLLEDGERENTRIES.LIST>";
                        }
                    }
                    else
                    {
                        if (type == "Purchase" || type == "Receipt")
                        {
                            xmlVou = xmlVou + "<ALLLEDGERENTRIES.LIST><LEDGERNAME>" + dtdet.Rows[j]["AccountName"].ToString() + "</LEDGERNAME><GSTCLASS/><ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE><LEDGERFROMITEM>No</LEDGERFROMITEM> <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES><ISPARTYLEDGER>Yes</ISPARTYLEDGER> <AMOUNT>" + (-1 * double.Parse(dtdet.Rows[j]["Amount"].ToString())) + "</AMOUNT>       </ALLLEDGERENTRIES.LIST>";
                        }
                        else if (type == "Sales" || type == "Payment")
                        {
                            xmlVou = xmlVou + "<ALLLEDGERENTRIES.LIST><LEDGERNAME>" + dtdet.Rows[j]["AccountName"].ToString() + "</LEDGERNAME><GSTCLASS/><ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE><LEDGERFROMITEM>No</LEDGERFROMITEM> <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES><ISPARTYLEDGER>Yes</ISPARTYLEDGER> <AMOUNT>" + (-1 * double.Parse(dtdet.Rows[j]["Amount"].ToString())) + "</AMOUNT>       </ALLLEDGERENTRIES.LIST>";
                        }
                        else if (type == "Journal" || type == "Debit Note" || type == "Credit Note")
                        {
                            xmlVou = xmlVou + "<ALLLEDGERENTRIES.LIST><LEDGERNAME>" + dtdet.Rows[j]["AccountName"].ToString() + "</LEDGERNAME><GSTCLASS/><ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE><LEDGERFROMITEM>No</LEDGERFROMITEM> <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES><ISPARTYLEDGER>Yes</ISPARTYLEDGER> <AMOUNT>" + (-1 * double.Parse(dtdet.Rows[j]["Amount"].ToString())) + "</AMOUNT>       </ALLLEDGERENTRIES.LIST>";
                        }
                        else if (type == "Contra")
                        {
                            xmlVou = xmlVou + "<ALLLEDGERENTRIES.LIST><LEDGERNAME>" + dtdet.Rows[j]["AccountName"].ToString() + "</LEDGERNAME><GSTCLASS/><ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE><LEDGERFROMITEM>No</LEDGERFROMITEM> <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES><ISPARTYLEDGER>Yes</ISPARTYLEDGER> <AMOUNT>" + double.Parse(dtdet.Rows[j]["Amount"].ToString()) + "</AMOUNT>       </ALLLEDGERENTRIES.LIST>";
                        }
                    }

                    if (dtdet.Rows.Count - 1 == j)
                    {
                        xmlVouFooter = "</VOUCHER></TALLYMESSAGE>";
                        xmlstc = xmlstc + xmlVouHeader + xmlVou + xmlVouFooter;
                    }
                }
            }
            xmlFooter = "</REQUESTDATA>  </IMPORTDATA> </BODY></ENVELOPE>";
            xmlstc = xmlHeader + xmlstc + xmlFooter;
            return xmlstc;
        }

        public bool CreateVoucherB(string VoucherType, String DateYYYYMMDD, string Account, string Reff, string Narration, DataTable dt)
        {
            System.IO.StreamWriter streamWriter;
            try
            {
                string xmlHeader = "<ENVELOPE><HEADER><TALLYREQUEST>Import Data</TALLYREQUEST></HEADER><BODY><IMPORTDATA><REQUESTDESC><REPORTNAME>All Masters</REPORTNAME></REQUESTDESC><REQUESTDATA>";
                string xmlVouHeader = "<TALLYMESSAGE xmlns:UDF='TallyUDF'><VOUCHER  VCHTYPE='" + VoucherType + "'ACTION='Create'><ISOPTIONAL>No</ISOPTIONAL><USEFORGAINLOSS>No</USEFORGAINLOSS><USEFORCOMPOUND>No</USEFORCOMPOUND><VOUCHERTYPENAME>" + VoucherType + "</VOUCHERTYPENAME> <DATE>" + DateYYYYMMDD + "</DATE><EFFECTIVEDATE>" + DateYYYYMMDD + "</EFFECTIVEDATE><ISCANCELLED>No</ISCANCELLED><USETRACKINGNUMBER>No</USETRACKINGNUMBER><ISPOSTDATED>No</ISPOSTDATED><ISINVOICE>No</ISINVOICE><DIFFACTUALQTY>No</DIFFACTUALQTY><NARRATION>" + Narration + "</NARRATION><ASPAYSLIP>No</ASPAYSLIP><REFERENCE>" + Reff + "</REFERENCE><PARTYLEDGERNAME>" + Account + "</PARTYLEDGERNAME>";
                string xmlVou = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    xmlVou = xmlVou + "<LEDGERENTRIES.LIST><REMOVEZEROENTRIES>No</REMOVEZEROENTRIES> <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE><LEDGERFROMITEM>No</LEDGERFROMITEM><TAXCLASSIFICATIONNAME/><LEDGERNAME>" + dt.Rows[i][0].ToString() + "</LEDGERNAME><AMOUNT>" + dt.Rows[i][1].ToString() + "</AMOUNT> </LEDGERENTRIES.LIST>";
                }
                string xmlVouFooter = "</VOUCHER></TALLYMESSAGE>";
                string xmlFooter = "</REQUESTDATA></IMPORTDATA></BODY></ENVELOPE>";
                string xmlstc = xmlHeader + xmlVouHeader + xmlVou + xmlVouFooter + xmlFooter;
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:9000");
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentLength = xmlstc.Length;
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
                streamWriter.Write(xmlstc);
                streamWriter.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string CreateVoucher1(DataTable dt, string type)
        {
            string xmlstc = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataTable dtdet = new DataTable();
                Database.GetSqlData("SELECT ACCOUNT.Name as AccountName, JOURNAL.cr+(JOURNAL.Dr*-1) AS Amount, JOURNAL.Narr, JOURNAL.Vdate, VOUCHERTYPE.AliasName, VOUCHERINFO.Vnumber FROM ((VOUCHERINFO LEFT JOIN JOURNAL ON VOUCHERINFO.Vi_id = JOURNAL.Vi_id) LEFT JOIN ACCOUNT ON JOURNAL.Ac_id = ACCOUNT.Ac_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERINFO.Vi_id)=" + dt.Rows[i]["Vi_id"].ToString() + ")) ORDER BY JOURNAL.cr+(JOURNAL.Dr*-1)", dtdet);
                DateTime Vdate = DateTime.Parse(dtdet.Rows[0]["Vdate"].ToString());
                if (dtdet.Rows[0]["Vdate"].ToString() != "")
                {
                    string xmlHeader = "";
                    string xmlVou = "";
                    string xmlVouHeader = "";
                    string xmlVouFooter = "";
                    string xmlFooter = "";
                    xmlHeader = "<ENVELOPE> <HEADER><TALLYREQUEST>Import Data</TALLYREQUEST> </HEADER> <BODY>  <IMPORTDATA>   <REQUESTDESC>    <REPORTNAME>Vouchers</REPORTNAME>   </REQUESTDESC>   <REQUESTDATA>";
                    for (int j = 0; j < dtdet.Rows.Count; j++)
                    {
                        if (j == 0)
                        {
                            if (double.Parse(dtdet.Rows[0]["Amount"].ToString()) < 0)
                            {
                                xmlVouHeader = "<TALLYMESSAGE xmlns:UDF='TallyUDF'>  <VOUCHER  VCHTYPE='" + type + "' ACTION='Create'><DATE>" + Vdate.ToString("yyyyMMdd") + "</DATE> <GUID></GUID> <VOUCHERTYPENAME>" + type + "</VOUCHERTYPENAME> <VOUCHERNUMBER>" + dtdet.Rows[j]["Vnumber"].ToString() + "</VOUCHERNUMBER>      <PARTYLEDGERNAME>" + dtdet.Rows[j]["AccountName"].ToString() + "</PARTYLEDGERNAME> <ISOPTIONAL>No</ISOPTIONAL>      <EFFECTIVEDATE>" + Vdate.ToString("yyyyMMdd") + "</EFFECTIVEDATE><ALLLEDGERENTRIES.LIST><LEDGERNAME>" + dtdet.Rows[j]["AccountName"].ToString() + "</LEDGERNAME><ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE><ISPARTYLEDGER>Yes</ISPARTYLEDGER> <ISLASTDEEMEDPOSITIVE>Yes</ISLASTDEEMEDPOSITIVE> <AMOUNT>" + double.Parse(dtdet.Rows[j]["Amount"].ToString()) + "</AMOUNT>       </ALLLEDGERENTRIES.LIST>";
                            }
                        }
                        else
                        {
                            if (j != 0 && double.Parse(dtdet.Rows[j]["Amount"].ToString()) < 0)
                            {
                                xmlVou = xmlVou + "<ALLLEDGERENTRIES.LIST><LEDGERNAME>" + dtdet.Rows[j]["AccountName"].ToString() + "</LEDGERNAME><ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE><ISPARTYLEDGER>Yes</ISPARTYLEDGER> <ISLASTDEEMEDPOSITIVE>Yes</ISLASTDEEMEDPOSITIVE> <AMOUNT>" + double.Parse(dtdet.Rows[j]["Amount"].ToString()) + "</AMOUNT>       </ALLLEDGERENTRIES.LIST>";
                            }
                            else
                            {
                                xmlVou = xmlVou + "<ALLLEDGERENTRIES.LIST> <LEDGERNAME>" + dtdet.Rows[j]["AccountName"].ToString() + "</LEDGERNAME> <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE> <ISPARTYLEDGER>No</ISPARTYLEDGER> <ISLASTDEEMEDPOSITIVE>No</ISLASTDEEMEDPOSITIVE> <AMOUNT>" + double.Parse(dtdet.Rows[j]["Amount"].ToString()) + "</AMOUNT>    </ALLLEDGERENTRIES.LIST>";
                            }
                        }
                    }
                    xmlVouFooter = "</VOUCHER></TALLYMESSAGE>";
                    xmlFooter = "</REQUESTDATA>  </IMPORTDATA> </BODY></ENVELOPE>";
                    xmlstc = xmlstc + xmlHeader + xmlVouHeader + xmlVou + xmlVouFooter + xmlFooter;
                }
            }
            return xmlstc;
        }

        public bool CreateVoucher(string VoucherType, String DateYYYYMMDD, string Account, string Reff, string Narration, DataTable dt)
        {
            System.IO.StreamWriter streamWriter;
            try
            {
                string xmlHeader = "<ENVELOPE><HEADER><TALLYREQUEST>Import Data</TALLYREQUEST></HEADER><BODY><IMPORTDATA><REQUESTDESC><REPORTNAME>All Masters</REPORTNAME></REQUESTDESC><REQUESTDATA>";
                string xmlVouHeader = "<TALLYMESSAGE xmlns:UDF='TallyUDF'><VOUCHER  VCHTYPE='" + VoucherType + "'ACTION='Create'><ISOPTIONAL>No</ISOPTIONAL><USEFORGAINLOSS>No</USEFORGAINLOSS><USEFORCOMPOUND>No</USEFORCOMPOUND><VOUCHERTYPENAME>" + VoucherType + "</VOUCHERTYPENAME> <DATE>" + DateYYYYMMDD + "</DATE><EFFECTIVEDATE>" + DateYYYYMMDD + "</EFFECTIVEDATE><ISCANCELLED>No</ISCANCELLED><USETRACKINGNUMBER>No</USETRACKINGNUMBER><ISPOSTDATED>No</ISPOSTDATED><ISINVOICE>No</ISINVOICE><DIFFACTUALQTY>No</DIFFACTUALQTY><NARRATION>" + Narration + "</NARRATION><ASPAYSLIP>No</ASPAYSLIP><REFERENCE>" + Reff + "</REFERENCE><PARTYLEDGERNAME>" + Account + "</PARTYLEDGERNAME>";
                string xmlVou = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    xmlVou = xmlVou + "<LEDGERENTRIES.LIST><REMOVEZEROENTRIES>No</REMOVEZEROENTRIES> <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE><LEDGERFROMITEM>No</LEDGERFROMITEM><TAXCLASSIFICATIONNAME/><LEDGERNAME>" + dt.Rows[i][0].ToString() + "</LEDGERNAME><AMOUNT>" + dt.Rows[i][1].ToString() + "</AMOUNT> </LEDGERENTRIES.LIST>";
                }
                string xmlVouFooter = "</VOUCHER></TALLYMESSAGE>";
                string xmlFooter = "</REQUESTDATA></IMPORTDATA></BODY></ENVELOPE>";
                string xmlstc = xmlHeader + xmlVouHeader + xmlVou + xmlVouFooter + xmlFooter;
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:9000");
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentLength = xmlstc.Length;
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
                streamWriter.Write(xmlstc);
                streamWriter.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CreateSalePurchase(string VoucherType, String DateYYYYMMDD, string Account, string Reff, string Narration, DataTable dt)
        {
            System.IO.StreamWriter streamWriter;
            try
            {
                string xmlHeader = "<ENVELOPE><HEADER><TALLYREQUEST>Import Data</TALLYREQUEST></HEADER><BODY><IMPORTDATA><REQUESTDESC><REPORTNAME>All Masters</REPORTNAME></REQUESTDESC><REQUESTDATA>";
                string xmlVouHeader = "<TALLYMESSAGE xmlns:UDF='TallyUDF'><VOUCHER  VCHTYPE='" + VoucherType + "'ACTION='Create'><ISOPTIONAL>No</ISOPTIONAL><USEFORGAINLOSS>No</USEFORGAINLOSS><USEFORCOMPOUND>No</USEFORCOMPOUND><VOUCHERTYPENAME>" + VoucherType + "</VOUCHERTYPENAME> <DATE>" + DateYYYYMMDD + "</DATE><EFFECTIVEDATE>" + DateYYYYMMDD + "</EFFECTIVEDATE><ISCANCELLED>No</ISCANCELLED><USETRACKINGNUMBER>Yes</USETRACKINGNUMBER><ISPOSTDATED>No</ISPOSTDATED><ISINVOICE>Yes</ISINVOICE><DIFFACTUALQTY>No</DIFFACTUALQTY><NARRATION>" + Narration + "</NARRATION><ASPAYSLIP>No</ASPAYSLIP><REFERENCE>" + Reff + "</REFERENCE><PARTYLEDGERNAME>" + Account + "</PARTYLEDGERNAME>";
                string xmlVou = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    xmlVou = xmlVou + "<LEDGERENTRIES.LIST><REMOVEZEROENTRIES>No</REMOVEZEROENTRIES> <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE><LEDGERFROMITEM>No</LEDGERFROMITEM><TAXCLASSIFICATIONNAME/><LEDGERNAME>" + dt.Rows[i]["Account"].ToString() + "</LEDGERNAME><AMOUNT>" + dt.Rows[i]["Amount"].ToString() + "</AMOUNT> </LEDGERENTRIES.LIST>";
                }
                string xmlVouFooter = "</VOUCHER></TALLYMESSAGE>";
                string xmlFooter = "</REQUESTDATA></IMPORTDATA></BODY></ENVELOPE>";
                string xmlstc = xmlHeader + xmlVouHeader + xmlVou + xmlVouFooter + xmlFooter;
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:9000");
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentLength = xmlstc.Length;
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
                streamWriter.Write(xmlstc);
                streamWriter.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
