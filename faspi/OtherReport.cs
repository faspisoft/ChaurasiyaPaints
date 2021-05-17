﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.ComponentModel;
using System.Windows.Forms;

namespace faspi
{
    class OtherReport
    {        
        DataTable DtFirm = new DataTable();
        ReportDocument rptOther = new ReportDocument();
        MailMessage mail = new MailMessage();
        showReport Viewer = new showReport();
        bool mailSent = false;
        string status = "";

        public void voucherprint(System.Windows.Forms.Form frm, string Rtype, string vid, String copytype, Boolean stationary, string Mode)
        {
            string Gtype = Database.GetScalarText("select Type from vouchertype where Vt_id='" + Rtype + "' ");
            String Repsql = "", Repsql2 = "", Repsql3 = "", Repsql4 = "";
            Database.GetSqlData("SELECT COMPANY.Name as Name , COMPANY.Cst_no as CompanyMobileno , COMPANY.Tin_no as Tin_no, COMPANY.Email as CompanyEmail, COMPANY.Address1 as Address1, COMPANY.Address2 as Address2, COMPANY.Contactno as CompanyLandline, COMPANY.BankName as BankName,COMPANY.IFSC as IFSC,COMPANY.AccountNo as AccountNo,State.Sname as CompanyState, State.GSTCode as Statecode FROM COMPANY LEFT JOIN State ON COMPANY.CState_id = State.State_id", DtFirm);
            FileInfo f = new FileInfo(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Report.net/" + getReportFileName(Rtype));

            if (f.Exists)
            {
                rptOther.Load(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Report.net/" + getReportFileName(Rtype));
            }
            else
            {
                MessageBox.Show("Report does not exist");
                return;
            }

            if (copytype == "None")
            {
                return;
            }

            showReport Viewer = new showReport();
            Viewer.MdiParent = frm.MdiParent;
            DataTable dt1 = new DataTable();
            if (Gtype == "Sale" || Gtype == "Return" || Gtype == "Purchase" || Gtype == "P Return" || Gtype == "Temp" || Gtype == "Purchase" || Gtype == "Pending" || Gtype == "RCM" || Gtype == "Transfer" || Gtype == "Sale Order")
            {
                Repsql = "select * from QryVoucher where vid='" + vid + "'";
                Repsql2 = "select * from Qryvoucherdes where vid='" + vid + "'";
                Repsql3 = "select * from QryVoucherTax where vid='" + vid + "'";
               // Repsql4 = "select * from QryVoucherPack where vi_id='" + vid + "'";
             
                Database.GetSqlData(Repsql, dt1);
                rptOther.SetDataSource(dt1);
                DataTable dt2 = new DataTable();
                Database.GetSqlData(Repsql2, dt2);
                DataTable dt3 = new DataTable();
                Database.GetSqlData(Repsql3, dt3);
                //DataTable dt4 = new DataTable();
                //Database.GetSqlData(Repsql4, dt4);

                ReportDocument subRepDoc = rptOther.Subreports[0];
                subRepDoc.SetDataSource(dt2);

                if (rptOther.Subreports.Count == 2)
                {

                    rptOther.Subreports[1].SetDataSource(dt3);
                }
                //if (rptOther.Subreports.Count == 2)
                //{

                //    rptOther.Subreports[1].SetDataSource(dt3);
                //}
                //if (rptOther.Subreports.Count == 3)
                //{

                //    rptOther.Subreports[2].SetDataSource(dt4);
                //}

                rptOther.SetParameterValue("Copy Name", copytype);
                rptOther.SetParameterValue("T.I.N.", DtFirm.Rows[0]["Tin_no"].ToString());
            }

            else if (Gtype == "Payment" || Gtype == "Receipt" || Gtype == "Journal" || Gtype == "Dnote" || Gtype == "Cnote" || Gtype == "Contra")
            {
                Repsql = "select * from QryReceptPaymentJournal where vid='" + vid + "'";
                
                Database.GetSqlData(Repsql, dt1);
                rptOther.SetDataSource(dt1);
            }

            rptOther.SetParameterValue("Company Name", DtFirm.Rows[0]["name"].ToString());
            rptOther.SetParameterValue("Company Address1", DtFirm.Rows[0]["Address1"].ToString());
            rptOther.SetParameterValue("Company Address2", DtFirm.Rows[0]["Address2"].ToString());

            if (rptOther.ParameterFields.Count > 12)
            {
                rptOther.SetParameterValue("CompanyMobileno", DtFirm.Rows[0]["CompanyMobileno"].ToString());
                rptOther.SetParameterValue("CompanyEmail", DtFirm.Rows[0]["CompanyEmail"].ToString());
                rptOther.SetParameterValue("CompanyLandline", DtFirm.Rows[0]["CompanyLandline"].ToString());
                rptOther.SetParameterValue("BankName", DtFirm.Rows[0]["BankName"].ToString());
                rptOther.SetParameterValue("IFSC", DtFirm.Rows[0]["IFSC"].ToString());
                rptOther.SetParameterValue("AccountNo", DtFirm.Rows[0]["AccountNo"].ToString());
                rptOther.SetParameterValue("CompanyState", DtFirm.Rows[0]["CompanyState"].ToString());
                rptOther.SetParameterValue("Statecode", DtFirm.Rows[0]["Statecode"].ToString());
            }
            if (rptOther.ParameterFields.Count >= 20)
            {

                double bal = 0;

                string act_id = Database.GetScalarText("Select act_id from account where ac_id='" + funs.Select_ac_id(dt1.Rows[0]["Acc.Name"].ToString()) + "'");
                if (act_id != "SER3")
                {
                    if (Database.BMode == "A")
                    {

                        bal = Database.GetScalarDecimal("SELECT SUM(Balance) AS Balance FROM         (SELECT     Balance FROM          dbo.ACCOUNT  WHERE      (Ac_id = '" + funs.Select_ac_id(dt1.Rows[0]["Acc.Name"].ToString()) + "') UNION ALL  SELECT     SUM(dbo.Journal.Amount) AS SumOfAmount  FROM         dbo.VOUCHERINFO LEFT OUTER JOIN  dbo.Journal ON dbo.VOUCHERINFO.Vi_id = dbo.Journal.Vi_id  WHERE     (dbo.Journal.Ac_id = '" + funs.Select_ac_id(dt1.Rows[0]["Acc.Name"].ToString()) + "') AND (dbo.Journal.Vdate <= " + access_sql.Hash + DateTime.Parse(dt1.Rows[0]["vdate"].ToString()).ToString(Database.dformat) + access_sql.Hash + ")  AND (dbo.Journal.A = 'true')) AS res");
                    }
                    else
                    {

                        bal = Database.GetScalarDecimal("SELECT SUM(Balance) AS Balance FROM         (SELECT     Balance+Balance2 as Balance FROM          dbo.ACCOUNT  WHERE      (Ac_id = '" + funs.Select_ac_id(dt1.Rows[0]["Acc.Name"].ToString()) + "') UNION ALL  SELECT     SUM(dbo.Journal.Amount) AS SumOfAmount  FROM         dbo.VOUCHERINFO LEFT OUTER JOIN  dbo.Journal ON dbo.VOUCHERINFO.Vi_id = dbo.Journal.Vi_id  WHERE     (dbo.Journal.Ac_id = '" + funs.Select_ac_id(dt1.Rows[0]["Acc.Name"].ToString()) + "') AND (dbo.Journal.Vdate <= " + access_sql.Hash + DateTime.Parse(dt1.Rows[0]["vdate"].ToString()).ToString(Database.dformat) + access_sql.Hash + ")  AND (dbo.Journal.AB = 'true')) AS res");
                    }
                }
                string balan = "";
                try
                {
                    if (bal == 0)
                    {
                        balan = "0";
                    }
                    else if (bal > 0)
                    {
                        balan = bal.ToString() + " Dr.";
                    }
                    else
                    {
                        balan = (-1 * bal).ToString() + " Cr.";
                    }
                    rptOther.SetParameterValue("CurBal", balan);
                }
                catch (Exception ex)
                {

                }

            }
            rptOther.SetParameterValue("Voucher Id", 0);
            rptOther.SetParameterValue("Name of Report", funs.Select_vt_Alias(Rtype));

            if (stationary == true)
            {
                rptOther.SetParameterValue("Display", false);
            }
            else
            {
                rptOther.SetParameterValue("Display", true);
            }
            if (Mode == "View")
            {
                Viewer.crystalReportViewer1.ReportSource = rptOther;
                Viewer.Show();
            }
            else if (Mode == "Print")
            {
                rptOther.PrintToPrinter(1, true, 0, 0);
                Database.CommandExecutor("Update Voucherinfo set printcount =printcount +1 where Vi_id='" + vid + "' ");
            }
            else if (Mode == "Email")
            {
                string smtpAddress = "";
                string emailFrom = "";
                string password = "";
                int port = 0;
                bool Credentials = false;
                bool EnableSsl = true;

                DataTable dtFirmMailInfo = new DataTable();
                Database.GetSqlData("select * from mailer", dtFirmMailInfo);
                if (dtFirmMailInfo.Rows.Count > 0)
                {
                    smtpAddress = dtFirmMailInfo.Rows[0]["smtp"].ToString();
                    emailFrom = dtFirmMailInfo.Rows[0]["emailid"].ToString();
                    password = dtFirmMailInfo.Rows[0]["password"].ToString();
                    port = int.Parse(dtFirmMailInfo.Rows[0]["port"].ToString());
                    Credentials = bool.Parse(dtFirmMailInfo.Rows[0]["Credentials"].ToString());
                    EnableSsl = bool.Parse(dtFirmMailInfo.Rows[0]["EnableSsl"].ToString());
                }

                SmtpClient client = new SmtpClient(smtpAddress, port);
                client.UseDefaultCredentials = Credentials;
                client.EnableSsl = EnableSsl;
                client.Credentials = new NetworkCredential(emailFrom, password);






                string ac_id = Database.GetScalarText("Select ac_id from Voucherinfo where vi_id='" + vid + "' ");
                string emailTo = Database.GetScalarText("Select Email from Account where Ac_id='" + ac_id + "' ");
                string acname = funs.Select_ac_nm(ac_id);
                if (emailTo != "None" || emailTo != "")
                {
                    string subject = "Bill";
                    string body = "Dear <b>" + acname + "</b>,<br/>";
                    body += "Please find enclosed Bill.<br/>";
                    body += "Your Balance is: <b>" + funs.accbal(funs.Select_ac_id(acname)) + "</b>";
                    body += "</b><br/><br/>Regards";
                    body += "<br/><b>" + Database.fname;
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(emailFrom);
                    mail.To.Add(emailTo);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;






                    try
                    {
                        rptOther.Load(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Report.net/" + getReportFileName(Rtype));
                        Viewer.crystalReportViewer1.ReportSource = rptOther;
                        rptOther.ExportToDisk(ExportFormatType.PortableDocFormat, "D:\a.pdf");
                        System.Net.Mime.ContentType contype = new System.Net.Mime.ContentType();

                        mail.Attachments.Add(new Attachment(rptOther.ExportToStream(ExportFormatType.PortableDocFormat), "Invoice.pdf"));
                        object userState = mail;

                        status = "Mail Sent Successfully";
                        client.SendCompleted += new SendCompletedEventHandler(SmtpClient_OnCompleted);
                        client.SendAsync(mail, userState);
                    }
                    catch (Exception ex)
                    {
                        status = "Mail Not Sent";
                    }

                    DataTable dtemail = new DataTable("EmailLOG");
                    Database.GetSqlData("select * from EmailLOG where id=0", dtemail);
                    dtemail.Rows.Add();
                    dtemail.Rows[0]["AccName"] = acname;
                    dtemail.Rows[0]["Email"] = emailTo;
                    dtemail.Rows[0]["Status"] = status;
                    dtemail.Rows[0]["SDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
                    dtemail.Rows[0]["STime"] = DateTime.Now.ToString("HH:mm");
                    Database.SaveData(dtemail);
                }
            }
        }




        public void voucherprint(DataTable QryVoucher, DataTable Qryvoucherdes, DataTable QryVoucherTax, string Rtype)
        {
            string vid = "0";
            bool stationary = Database.GetScalarBool("SELECT VOUCHERTYPE.Stationary FROM VOUCHERTYPE WHERE (((VOUCHERTYPE.Vt_id)='SER21'))");
            
           // Database.GetSqlData("select * from company", DtFirm);
            Database.GetSqlData("SELECT COMPANY.Name as Name , COMPANY.Cst_no as CompanyMobileno , COMPANY.Tin_no as Tin_no, COMPANY.Email as CompanyEmail, COMPANY.Address1 as Address1, COMPANY.Address2 as Address2, COMPANY.Contactno as CompanyLandline, COMPANY.BankName as BankName,COMPANY.IFSC as IFSC,COMPANY.AccountNo as AccountNo,State.Sname as CompanyState, State.GSTCode as Statecode FROM COMPANY LEFT JOIN State ON COMPANY.CState_id = State.State_id", DtFirm);
            FileInfo f = new FileInfo(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Report.net/" + getReportFileName(Rtype));


            if (f.Exists)
            {
                rptOther.Load(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Report.net/" + getReportFileName(Rtype));
            }
            else
            {
                MessageBox.Show("Report does not exist");
                return;
            }

            rptOther.SetDataSource(QryVoucher);

            ReportDocument subRepDoc = rptOther.Subreports[0];
            subRepDoc.SetDataSource(Qryvoucherdes);
            if (rptOther.Subreports.Count == 2)
            {
                rptOther.Subreports[1].SetDataSource(QryVoucherTax);
            }



            //rptOther.SetParameterValue("Copy Name", "Original Copy");
            //rptOther.SetParameterValue("T.I.N.", DtFirm.Rows[0]["Tin_no"].ToString());
            //rptOther.SetParameterValue("Company Name", DtFirm.Rows[0]["name"].ToString());
            //rptOther.SetParameterValue("Company Address1", DtFirm.Rows[0]["Address1"].ToString());
            //rptOther.SetParameterValue("Company Address2", DtFirm.Rows[0]["Address2"].ToString());
            //rptOther.SetParameterValue("Voucher Id", vid);
            //rptOther.SetParameterValue("Name of Report", funs.Select_vt_Alias(Rtype));

            //if (stationary == true)
            //{
            //    rptOther.SetParameterValue("Display", false);
            //}
            //else
            //{
            //    rptOther.SetParameterValue("Display", true);
            //}
           
            //rptOther.PrintToPrinter(1, true, 0, 0);



          //string  Repsql = "select * from QryVoucher where vid='" + vid + "'";
          //string Repsql2 = "select * from Qryvoucherdes where vid='" + vid + "'";
          //string Repsql3 = "select * from QryVoucherTax where vid='" + vid + "'";

            //DataTable dt1 = new DataTable();
            //Database.GetSqlData(Repsql, dt1);
            //rptOther.SetDataSource(dt1);
            //DataTable dt2 = new DataTable();
            //Database.GetSqlData(Repsql2, dt2);
            //DataTable dt3 = new DataTable();
            //Database.GetSqlData(Repsql3, dt3);

            //ReportDocument subRepDoc = rptOther.Subreports[0];
            //subRepDoc.SetDataSource(dt2);
            //if (rptOther.Subreports.Count == 2)
            //{

            //    rptOther.Subreports[1].SetDataSource(dt3);
            //}
          rptOther.SetParameterValue("Copy Name", "");
          rptOther.SetParameterValue("T.I.N.", DtFirm.Rows[0]["Tin_no"].ToString());
          rptOther.SetParameterValue("Company Name", DtFirm.Rows[0]["name"].ToString());
          rptOther.SetParameterValue("Company Address1", DtFirm.Rows[0]["Address1"].ToString());
          rptOther.SetParameterValue("Company Address2", DtFirm.Rows[0]["Address2"].ToString());

            if (rptOther.ParameterFields.Count > 12)
            {
                rptOther.SetParameterValue("CompanyMobileno", DtFirm.Rows[0]["CompanyMobileno"].ToString());
                rptOther.SetParameterValue("CompanyEmail", DtFirm.Rows[0]["CompanyEmail"].ToString());
                rptOther.SetParameterValue("CompanyLandline", DtFirm.Rows[0]["CompanyLandline"].ToString());
                rptOther.SetParameterValue("BankName", DtFirm.Rows[0]["BankName"].ToString());
                rptOther.SetParameterValue("IFSC", DtFirm.Rows[0]["IFSC"].ToString());
                rptOther.SetParameterValue("AccountNo", DtFirm.Rows[0]["AccountNo"].ToString());
                rptOther.SetParameterValue("CompanyState", DtFirm.Rows[0]["CompanyState"].ToString());
                rptOther.SetParameterValue("Statecode", DtFirm.Rows[0]["Statecode"].ToString());
            }

            rptOther.SetParameterValue("Voucher Id", 0);
            rptOther.SetParameterValue("Name of Report", funs.Select_vt_Alias(Rtype));
            rptOther.PrintToPrinter(1, true, 0, 0);
        }

        public static void SmtpClient_OnCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MailMessage mail = (MailMessage)e.UserState;
            
            string subject = mail.Subject;

            if (e.Cancelled)
            {
                MessageBox.Show("Send canceled for mail with subject [{0}]."+ subject);
            }
            if (e.Error != null)
            {
                MessageBox.Show("Error {1} occurred when sending mail [{0}] " + subject + e.Error.ToString());
            }            
        }

        public void PriceVariationPurchase(System.Windows.Forms.Form frm, DateTime DateFrom, DateTime DateTo, int descid, string Mode)
        {
            string RptId = "SER59";
            String Repsql = "";
            Database.GetSqlData("select * from company", DtFirm);
            Repsql = "select * from QryItemTranjection";
            rptOther.Load(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Report.net/" + getReportFileName(RptId));
            Viewer.MdiParent = frm.MdiParent;            
            DataTable dt = new DataTable();
            Database.GetSqlData(Repsql, dt);
           
            rptOther.SetDataSource(dt);
            rptOther.SetParameterValue("Company Name", DtFirm.Rows[0]["name"].ToString());
            rptOther.SetParameterValue("Company Address1", DtFirm.Rows[0]["Address1"].ToString());
            rptOther.SetParameterValue("Company Address2", DtFirm.Rows[0]["Address2"].ToString());
            rptOther.SetParameterValue("Description of report", "Price Variation Purchase for the period " + DateFrom.ToString("dd MMM, yyyy") + " To " + DateTo.ToString("dd MMM, yyyy"));
            rptOther.SetParameterValue("DescId", descid);
            rptOther.SetParameterValue("Date From", DateFrom);
            rptOther.SetParameterValue("Date To", DateTo);
            OpenMode(Mode);
        }

        public void PriceVariationSale(System.Windows.Forms.Form frm, DateTime DateFrom, DateTime DateTo, int descid, string Mode)
        {
            string RptId ="SER58";
            String Repsql = "";
            Database.GetSqlData("select * from company", DtFirm);
            Repsql = "select * from QryItemTranjection";
            rptOther.Load(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Report.net/" + getReportFileName(RptId));
            Viewer.MdiParent = frm.MdiParent;            
            DataTable dt = new DataTable();
            Database.GetSqlData(Repsql, dt);
            
            rptOther.SetDataSource(dt);
            rptOther.SetParameterValue("Company Name", DtFirm.Rows[0]["name"].ToString());
            rptOther.SetParameterValue("Company Address1", DtFirm.Rows[0]["Address1"].ToString());
            rptOther.SetParameterValue("Company Address2", DtFirm.Rows[0]["Address2"].ToString());
            rptOther.SetParameterValue("Description of report", "Price Variation Sale for the period " + DateFrom.ToString("dd MMM, yyyy") + " To " + DateTo.ToString("dd MMM, yyyy"));
            rptOther.SetParameterValue("DescId", descid);
            rptOther.SetParameterValue("Date From", DateFrom);
            rptOther.SetParameterValue("Date To", DateTo);
            OpenMode(Mode);
        }

        public void ItemLedger(System.Windows.Forms.Form frm, DateTime DateFrom, DateTime DateTo, int descid, string Mode)
        {
            string RptId = "SER37";
            String Repsql = "";
            Database.GetSqlData("select * from company", DtFirm);
            Repsql = "SELECT Final.Des_id, Final.Des, Final.Pak, Final.Vdate, Final.Short, Final.Vnumber, Final.Itemsr, Final.Inward, Final.Outward, [Final].[Short] & ' ' &  Format([Vdate],'yyyymmdd') & ' ' & [Final].[Vnumber] AS Doc, OTHER.Name AS Company, OTHER_1.Name AS Item, OTHER_2.Name AS Color, OTHER_3.Name AS PriceGroup FROM (((((SELECT Table2.Des_id, DESCRIPTION.Description AS Des, PACKING.Name AS Pak, #1/1/1901# AS Vdate, 'OPN' AS [Short], 0 AS Vnumber, 0 AS Itemsr, Table2.Inward, Table2.Outward FROM ((SELECT Table1.Des_id, Sum(Table1.Inward) AS Inward, Sum(Table1.Outward) AS Outward FROM (SELECT * FROM QryItemJournal WHERE Vdate< #" + DateFrom + "#";
            Repsql = Repsql + " UNION ALL SELECT DESCRIPTION.Des_id, DESCRIPTION.Description, PACKING.Name AS Packing, #01/01/2001# AS Vdate, 'OPN' AS [Short], 0 AS Vnumber, 0 AS Itemsr, DESCRIPTION.Open_stock AS Inward, 0 AS Outward FROM DESCRIPTION LEFT JOIN PACKING ON DESCRIPTION.Pack_id = PACKING.Pack_id)  AS Table1 GROUP BY Table1.Des_id)  AS Table2 LEFT JOIN DESCRIPTION ON Table2.Des_id=DESCRIPTION.Des_id) LEFT JOIN PACKING ON DESCRIPTION.Pack_id=PACKING.Pack_id";
            Repsql = Repsql + " UNION ALL select Des_id,Description as Des,Packing as Pak,Vdate,Short,Vnumber,Itemsr,Inward,Outward from QryItemJournal where Vdate>=#" + DateFrom + "# and Vdate<=#" + DateTo + "#)  ";
            Repsql = Repsql + " AS Final LEFT JOIN DESCRIPTION ON Final.Des_id = DESCRIPTION.Des_id) LEFT JOIN OTHER ON DESCRIPTION.Company_id = OTHER.Oth_id) LEFT JOIN OTHER AS OTHER_1 ON DESCRIPTION.Item_id = OTHER_1.Oth_id) LEFT JOIN OTHER AS OTHER_2 ON DESCRIPTION.Col_id = OTHER_2.Oth_id) LEFT JOIN OTHER AS OTHER_3 ON DESCRIPTION.Group_id = OTHER_3.Oth_id";
            rptOther.Load(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Report.net/" + getReportFileName(RptId));
            Viewer.MdiParent = frm.MdiParent;
            DataTable dt = new DataTable();
            Database.GetSqlData(Repsql, dt);
            rptOther.SetDataSource(dt);
            rptOther.SetParameterValue("Company Name", DtFirm.Rows[0]["name"].ToString());
            rptOther.SetParameterValue("Company Address1", DtFirm.Rows[0]["Address1"].ToString());
            rptOther.SetParameterValue("Company Address2", DtFirm.Rows[0]["Address2"].ToString());
            rptOther.SetParameterValue("Discruption Of Report", "Item Ledger");
            rptOther.SetParameterValue("Des Id", descid);
            rptOther.SetParameterValue("Date From", DateFrom);
            rptOther.SetParameterValue("Date To", DateTo);
            OpenMode(Mode);
        }

        public void PriceList(System.Windows.Forms.Form frm, String Packing, String Company, String Item, String Color, String PriceGroup, Boolean Purchase, Boolean Wholesale, Boolean Retail, string Mode)
        {
            string RptId = "SER43";
            String Repsql = "";
            Database.GetSqlData("select * from company", DtFirm);
            Repsql = "select * from QryPriceList";
            rptOther.Load(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Report.net/" + getReportFileName(RptId));
            Viewer.MdiParent = frm.MdiParent;
            DataTable dt = new DataTable();
            Database.GetSqlData(Repsql, dt);
            rptOther.SetDataSource(dt);
            rptOther.SetParameterValue("Company Name", DtFirm.Rows[0]["name"].ToString());
            rptOther.SetParameterValue("Company Address1", DtFirm.Rows[0]["Address1"].ToString());
            rptOther.SetParameterValue("Company Address2", DtFirm.Rows[0]["Address2"].ToString());
            rptOther.SetParameterValue("Discruption Of Report", "Price List");
            rptOther.SetParameterValue("Packing", Packing);
            rptOther.SetParameterValue("Company", Company);
            rptOther.SetParameterValue("Item", Item);
            rptOther.SetParameterValue("Color", Color);
            rptOther.SetParameterValue("Price Group", PriceGroup);
            rptOther.SetParameterValue("Purchase", Purchase);
            rptOther.SetParameterValue("WholeSale", Wholesale);
            rptOther.SetParameterValue("Retail", Retail);
            OpenMode(Mode);
        }

        public void WarningSummary(System.Windows.Forms.Form frm, DateTime DateFrom, DateTime DateTo, String Packing, String Company, String Item, String Color, String PriceGroup, string Mode)
        {
            string RptId = "SER56";
            String Repsql = "";
            Database.GetSqlData("select * from company", DtFirm);
            Repsql = "SELECT Final.Des_id, Final.Des, Final.Pak, Final.Vdate, Final.Short, Final.Vnumber, Final.Itemsr, Final.Inward, Final.Outward, [Final].[Short] & ' ' &  Format([Vdate],'yyyymmdd') & ' ' & [Final].[Vnumber] AS Doc, OTHER.Name AS Company, OTHER_1.Name AS Item, OTHER_2.Name AS Color, OTHER_3.Name AS PriceGroup, DESCRIPTION.Wlavel FROM (((((SELECT Table2.Des_id, DESCRIPTION.Description AS Des, PACKING.Name AS Pak, #1/1/1901# AS Vdate, 'OPN' AS [Short], 0 AS Vnumber, 0 AS Itemsr, Table2.Inward, Table2.Outward FROM ((SELECT Table1.Des_id, Sum(Table1.Inward) AS Inward, Sum(Table1.Outward) AS Outward FROM (SELECT * FROM QryItemJournal WHERE Vdate< #" + DateFrom + "#";
            Repsql = Repsql + " UNION ALL SELECT DESCRIPTION.Des_id, DESCRIPTION.Description, PACKING.Name AS Packing, #01/01/2001# AS Vdate, 'OPN' AS [Short], 0 AS Vnumber, 0 AS Itemsr, DESCRIPTION.Open_stock AS Inward, 0 AS Outward FROM DESCRIPTION LEFT JOIN PACKING ON DESCRIPTION.Pack_id = PACKING.Pack_id)  AS Table1 GROUP BY Table1.Des_id)  AS Table2 LEFT JOIN DESCRIPTION ON Table2.Des_id=DESCRIPTION.Des_id) LEFT JOIN PACKING ON DESCRIPTION.Pack_id=PACKING.Pack_id";
            Repsql = Repsql + " UNION ALL select Des_id,Description as Des,Packing as Pak,Vdate,Short,Vnumber,Itemsr,Inward,Outward from QryItemJournal where Vdate>=#" + DateFrom + "# and Vdate<=#" + DateTo + "#)  ";
            Repsql = Repsql + " AS Final LEFT JOIN DESCRIPTION ON Final.Des_id = DESCRIPTION.Des_id) LEFT JOIN OTHER ON DESCRIPTION.Company_id = OTHER.Oth_id) LEFT JOIN OTHER AS OTHER_1 ON DESCRIPTION.Item_id = OTHER_1.Oth_id) LEFT JOIN OTHER AS OTHER_2 ON DESCRIPTION.Col_id = OTHER_2.Oth_id) LEFT JOIN OTHER AS OTHER_3 ON DESCRIPTION.Group_id = OTHER_3.Oth_id";
            rptOther.Load(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Report.net/" + getReportFileName(RptId));
            Viewer.MdiParent = frm.MdiParent;
            DataTable dt = new DataTable();
            Database.GetSqlData(Repsql, dt);           
            rptOther.SetDataSource(dt);
            rptOther.SetParameterValue("Date From", DateFrom);
            rptOther.SetParameterValue("Date To", DateTo);
            rptOther.SetParameterValue("Company Name", DtFirm.Rows[0]["name"].ToString());
            rptOther.SetParameterValue("Company Address1", DtFirm.Rows[0]["Address1"].ToString());
            rptOther.SetParameterValue("Company Address2", DtFirm.Rows[0]["Address2"].ToString());
            rptOther.SetParameterValue("Discruption Of Report", "Below Stock Warning Report, for the period " + DateFrom.ToString("dd MMM, yyyy") + " To " + DateTo.ToString("dd MMM, yyyy"));
            rptOther.SetParameterValue("Packing", Packing);
            rptOther.SetParameterValue("Company", Company);
            rptOther.SetParameterValue("Item", Item);
            rptOther.SetParameterValue("Color", Color);
            rptOther.SetParameterValue("Price Group", PriceGroup);
            OpenMode(Mode);
        }

        public void StockSummary(System.Windows.Forms.Form frm, DateTime DateFrom, DateTime DateTo, String Packing, String Company, String Item, String Color, String PriceGroup, string Mode)
        {
            string RptId = "SER45";
            String Repsql="";
            Database.GetSqlData("select * from company", DtFirm);
            Repsql = "SELECT Final.Des_id, Final.Des, Final.Pak, Final.Vdate, Final.Short, Final.Vnumber, Final.Itemsr, Final.Inward, Final.Outward, [Final].[Short] & ' ' &  Format([Vdate],'yyyymmdd') & ' ' & [Final].[Vnumber] AS Doc, OTHER.Name AS Company, OTHER_1.Name AS Item, OTHER_2.Name AS Color, OTHER_3.Name AS PriceGroup FROM (((((SELECT Table2.Des_id, DESCRIPTION.Description AS Des, PACKING.Name AS Pak, #1/1/1901# AS Vdate, 'OPN' AS [Short], 0 AS Vnumber, 0 AS Itemsr, Table2.Inward, Table2.Outward FROM ((SELECT Table1.Des_id, Sum(Table1.Inward) AS Inward, Sum(Table1.Outward) AS Outward FROM (SELECT * FROM QryItemJournal WHERE Vdate< #" + DateFrom + "#";
            Repsql = Repsql + " UNION ALL SELECT DESCRIPTION.Des_id, DESCRIPTION.Description, PACKING.Name AS Packing, #01/01/2001# AS Vdate, 'OPN' AS [Short], 0 AS Vnumber, 0 AS Itemsr, DESCRIPTION.Open_stock AS Inward, 0 AS Outward FROM DESCRIPTION LEFT JOIN PACKING ON DESCRIPTION.Pack_id = PACKING.Pack_id)  AS Table1 GROUP BY Table1.Des_id)  AS Table2 LEFT JOIN DESCRIPTION ON Table2.Des_id=DESCRIPTION.Des_id) LEFT JOIN PACKING ON DESCRIPTION.Pack_id=PACKING.Pack_id";
            Repsql = Repsql + " UNION ALL select Des_id,Description as Des,Packing as Pak,Vdate,Short,Vnumber,Itemsr,Inward,Outward from QryItemJournal where Vdate>=#" + DateFrom + "# and Vdate<=#" + DateTo + "#)  ";
            Repsql = Repsql + " AS Final LEFT JOIN DESCRIPTION ON Final.Des_id = DESCRIPTION.Des_id) LEFT JOIN OTHER ON DESCRIPTION.Company_id = OTHER.Oth_id) LEFT JOIN OTHER AS OTHER_1 ON DESCRIPTION.Item_id = OTHER_1.Oth_id) LEFT JOIN OTHER AS OTHER_2 ON DESCRIPTION.Col_id = OTHER_2.Oth_id) LEFT JOIN OTHER AS OTHER_3 ON DESCRIPTION.Group_id = OTHER_3.Oth_id";

            rptOther.Load(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Report.net/" + getReportFileName(RptId));
            Viewer.MdiParent = frm.MdiParent;
            DataTable dt = new DataTable();
            Database.GetSqlData(Repsql, dt);
            rptOther.SetDataSource(dt);
            rptOther.SetParameterValue("Date From", DateFrom);
            rptOther.SetParameterValue("Date To", DateTo);
            rptOther.SetParameterValue("Company Name", DtFirm.Rows[0]["name"].ToString());
            rptOther.SetParameterValue("Company Address1", DtFirm.Rows[0]["Address1"].ToString());
            rptOther.SetParameterValue("Company Address2", DtFirm.Rows[0]["Address2"].ToString());
            rptOther.SetParameterValue("Discruption Of Report", "Stock Summary Report, for the period " + DateFrom.ToString("dd MMM, yyyy") + " To " + DateTo.ToString("dd MMM, yyyy"));
            rptOther.SetParameterValue("Packing", Packing);
            rptOther.SetParameterValue("Company", Company);
            rptOther.SetParameterValue("Item", Item);
            rptOther.SetParameterValue("Color", Color);
            rptOther.SetParameterValue("Price Group", PriceGroup);
            OpenMode(Mode);
        }

        public void InBillCharges(System.Windows.Forms.Form frm, DateTime DateFrom, DateTime DateTo, String Packing, String Company, String Item, String Color, String PriceGroup, string Mode)
        {
            string RptId = "SER54";
            Database.GetSqlData("select * from company", DtFirm);
            rptOther.Load(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Report.net/" + getReportFileName(RptId));
            Viewer.MdiParent = frm.MdiParent;   
            DataTable dt = new DataTable();
            Database.GetSqlData("select * from QryItemTranjectionDetaled", dt);
            rptOther.SetDataSource(dt);
            rptOther.SetParameterValue("Date From", DateFrom);
            rptOther.SetParameterValue("Date To", DateTo);
            rptOther.SetParameterValue("Company Name", DtFirm.Rows[0]["name"].ToString());
            rptOther.SetParameterValue("Company Address1", DtFirm.Rows[0]["Address1"].ToString());
            rptOther.SetParameterValue("Company Address2", DtFirm.Rows[0]["Address2"].ToString());
            rptOther.SetParameterValue("Discruption Of Report", "InBillCharges Report, for the period " + DateFrom.ToString("dd MMM, yyyy") + " To " + DateTo.ToString("dd MMM, yyyy"));
            rptOther.SetParameterValue("Packing", Packing);
            rptOther.SetParameterValue("Company", Company);
            rptOther.SetParameterValue("Item", Item);
            rptOther.SetParameterValue("Color", Color);
            rptOther.SetParameterValue("Price Group", PriceGroup);
            OpenMode(Mode);
        }

        public void ItemLifting(System.Windows.Forms.Form frm, DateTime DateFrom, DateTime DateTo, String Packing, String Company, String Item, String Color, String PriceGroup, string Mode)
        {
            string RptId = "SER44";
            Database.GetSqlData("select * from company", DtFirm);
            rptOther.Load(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Report.net/" + getReportFileName(RptId));
            Viewer.MdiParent = frm.MdiParent;            
            DataTable dt = new DataTable();
            Database.GetSqlData("select * from QryItemTranjection", dt);          
            rptOther.SetDataSource(dt);
            rptOther.SetParameterValue("Date From", DateFrom);
            rptOther.SetParameterValue("Date To", DateTo);
            rptOther.SetParameterValue("Company Name", DtFirm.Rows[0]["name"].ToString());
            rptOther.SetParameterValue("Company Address1", DtFirm.Rows[0]["Address1"].ToString());
            rptOther.SetParameterValue("Company Address2", DtFirm.Rows[0]["Address2"].ToString());
            rptOther.SetParameterValue("Discruption Of Report", "Item Lifting Report, for the period " + DateFrom.ToString("dd MMM, yyyy") + " To " + DateTo.ToString("dd MMM, yyyy"));
            rptOther.SetParameterValue("Packing", Packing);
            rptOther.SetParameterValue("Company", Company);
            rptOther.SetParameterValue("Item", Item);
            rptOther.SetParameterValue("Color", Color);
            rptOther.SetParameterValue("Price Group", PriceGroup);
            OpenMode(Mode);
        }

        public void Journal(System.Windows.Forms.Form frm, DateTime DateFrom, DateTime DateTo, string Mode )
        {
            string RptId = "SER39"; 
            Database.GetSqlData("select * from company", DtFirm);
            GC.Collect();
            if (rptOther != null)
            {
                rptOther.Close();
                rptOther.Dispose();
            }
            rptOther = new ReportDocument();

            rptOther.Load(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Report.net/" + getReportFileName(RptId));
            Viewer.MdiParent = frm.MdiParent;            
            DataTable dt = new DataTable();
            Database.GetSqlData("select * from QryJournal", dt);
            
            rptOther.SetDataSource(dt);
            rptOther.SetParameterValue("Date From", DateFrom);
            rptOther.SetParameterValue("Date To", DateTo);
            rptOther.SetParameterValue("Company Name", DtFirm.Rows[0]["name"].ToString());
            rptOther.SetParameterValue("Company Address1", DtFirm.Rows[0]["Address1"].ToString());
            rptOther.SetParameterValue("Company Address2", DtFirm.Rows[0]["Address2"].ToString());
            rptOther.SetParameterValue("Discruption Of Report", "Journal, for the period " + DateFrom.ToString("dd MMM, yyyy") + " To " + DateTo.ToString("dd MMM, yyyy"));
            OpenMode(Mode);          
        }

        private void OpenMode(string Mode)
        {
            if (Mode == "View")
            {
                Viewer.crystalReportViewer1.ReportSource = rptOther;
                Viewer.Show();
            }

            else if (Mode == "Print")
            {
                rptOther.PrintToPrinter(1, true, 0, 0);
            }

            else if (Mode == "PDF")
            {
                SaveFileDialog ofd = new SaveFileDialog();
                ofd.Filter = "Adobe Acrobat(*.pdf) | *.pdf";
                if (DialogResult.OK == ofd.ShowDialog())
                {
                    rptOther.ExportToDisk(ExportFormatType.PortableDocFormat, ofd.FileName);
                    MessageBox.Show("Export Successfully!!");
                }
            }
            else if (Mode == "Excel")
            {
                SaveFileDialog ofd = new SaveFileDialog();
                ofd.Filter = "Microsoft Excel(*.xls) | *.xls";
                if (DialogResult.OK == ofd.ShowDialog())
                {
                    rptOther.ExportToDisk(ExportFormatType.Excel, ofd.FileName);
                    MessageBox.Show("Export Successfully!!");
                }
            }
            else if (Mode == "Excel Data Only")
            {
                SaveFileDialog ofd = new SaveFileDialog();
                ofd.Filter = "Microsoft Excel Data Only(*.xls) | *.xls";
                if (DialogResult.OK == ofd.ShowDialog())
                {
                    rptOther.ExportToDisk(ExportFormatType.ExcelRecord, ofd.FileName);
                    MessageBox.Show("Export Successfully!!");
                }
            }
            else if (Mode == "Word")
            {
                SaveFileDialog ofd = new SaveFileDialog();
                ofd.Filter = "Microsoft Word(*.doc) | *.doc";
                if (DialogResult.OK == ofd.ShowDialog())
                {
                    rptOther.ExportToDisk(ExportFormatType.WordForWindows, ofd.FileName);
                    MessageBox.Show("Export Successfully!!");
                }
            }
            else if (Mode == "Rich")
            {
                SaveFileDialog ofd = new SaveFileDialog();
                ofd.Filter = "Rich Text Format(*.rtf) | *.rtf";
                if (DialogResult.OK == ofd.ShowDialog())
                {
                    rptOther.ExportToDisk(ExportFormatType.RichText, ofd.FileName);
                    MessageBox.Show("Export Successfully!!");
                }
            }
            else if (Mode == "E-Mail")
            {
                frm_email frm = new frm_email();
                frm.ShowDialog();
                if (frm.strsendcancel == "Send")
                {
                    string smtpAddress = "", emailFrom = "", password = "";
                    DataTable dtFirmMailInfo = new DataTable();
                    Database.GetSqlData("select emailid,password,smtp from mailer", dtFirmMailInfo);
                    if (dtFirmMailInfo.Rows.Count > 0)
                    {
                        smtpAddress = dtFirmMailInfo.Rows[0]["smtp"].ToString();
                        emailFrom = dtFirmMailInfo.Rows[0]["emailid"].ToString();
                        password = dtFirmMailInfo.Rows[0]["password"].ToString();
                    }
                    string emailTo = frm.strto;
                    string subject = frm.strsubject;
                    string body = frm.strmessage;
                    mail.From = new MailAddress(emailFrom);
                    mail.To.Add(emailTo);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    mail.Attachments.Add(new Attachment(rptOther.ExportToStream(ExportFormatType.PortableDocFormat), "Attachment.pdf"));
                    SmtpClient smtp = new SmtpClient(smtpAddress);
                    smtp.Credentials = new NetworkCredential(emailFrom, password);
                    object userState = mail;
                    smtp.SendCompleted += new SendCompletedEventHandler(SmtpClient_OnCompleted);
                    smtp.SendAsync(mail, userState);
                    MessageBox.Show("E-Mail has been sent successfully!!");
                }
            }
        }

        public void Ledger(System.Windows.Forms.Form frm, DateTime DateFrom, DateTime DateTo, string AccName,string Mode)
        {
            string RptId = "SER29";
            Database.GetSqlData("select * from company", DtFirm);
            rptOther.Load(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Report.net/" + getReportFileName(RptId));           
            Viewer.MdiParent = frm.MdiParent;            
            DataTable dt = new DataTable();
            Database.GetSqlData("SELECT * FROM (SELECT #2/1/1801# AS Vdate, 'OPN' AS [Short], 0 AS Vnumber, Y.Name, Y.Dr,Y.Cr, 'Opening Balance' AS Narr,' ' AS DocNumber FROM (SELECT X.Name, Sum(X.Dr) AS Dr, Sum(X.Cr) AS Cr FROM (SELECT 0 AS sr, ACCOUNT.Name, ACCOUNT.Dr, ACCOUNT.Cr From ACCOUNT UNION ALL SELECT 1 AS sr, QryJournal.Name, Sum(QryJournal.Dr) AS Dr, Sum(QryJournal.Cr) AS Cr From QryJournal Where (((QryJournal.Vdate) < #" + DateFrom + "#)) GROUP BY QryJournal.Name)  AS X GROUP BY X.Name) AS Y UNION ALL SELECT JOURNAL.Vdate, VOUCHERTYPE.Short, VOUCHERINFO.Vnumber, ACCOUNT.Name, JOURNAL.Dr, JOURNAL.Cr, VOUCHERINFO.Narr,VOUCHERTYPE.Short & ' ' & Format(JOURNAL.Vdate,'yyyymmdd' & ' ' & VOUCHERINFO.Vnumber ) AS DocNumber From JOURNAL, ACCOUNT, Voucherinfo, VOUCHERTYPE WHERE (((JOURNAL.Ac_id)=[ACCOUNT].[Ac_id]) AND ((JOURNAL.Vi_id)=[VOUCHERINFO].[Vi_id]) AND ((VOUCHERINFO.Vt_id)=[VOUCHERTYPE].[Vt_id]) AND ((JOURNAL.Vdate)>=#" + DateFrom + "#)))  AS aman", dt);
            rptOther.SetDataSource(dt);
            rptOther.SetParameterValue("Date From", DateFrom);
            rptOther.SetParameterValue("Date To", DateTo);
            rptOther.SetParameterValue("Company Name", DtFirm.Rows[0]["name"].ToString());
            rptOther.SetParameterValue("Company Address1", DtFirm.Rows[0]["Address1"].ToString());
            rptOther.SetParameterValue("Company Address2", DtFirm.Rows[0]["Address2"].ToString());
            rptOther.SetParameterValue("Discruption Of Report", "Ledger, for the period " + DateFrom.ToString("dd MMM, yyyy") + " To " + DateTo.ToString("dd MMM, yyyy"));
            rptOther.SetParameterValue("Account", AccName);
            rptOther.SetParameterValue("Display", "true");
            OpenMode(Mode);
        }

        public void CashBook(System.Windows.Forms.Form frm, DateTime DateFrom, DateTime DateTo, string Mode)
        {
            string RptId = "SER30";
            string Repsql;
            Database.GetSqlData("select * from company", DtFirm);
            Repsql = "SELECT aman.Vdate, aman.Short, aman.Vnumber, aman.Name, aman.Dr, aman.Cr, aman.Narr, aman.DocNumber, ACCOUNT.Act_id AS AccountType";
            Repsql = Repsql + " FROM (SELECT #2/1/1801# AS Vdate, 'OPN' AS [Short], 0 AS Vnumber, Y.Name, Y.Dr,Y.Cr, 'Opening Balance' AS Narr,' ' AS DocNumber FROM (SELECT X.Name, Sum(X.Dr) AS Dr, Sum(X.Cr) AS Cr FROM (SELECT 0 AS sr, ACCOUNT.Name, ACCOUNT.Dr, ACCOUNT.Cr From ACCOUNT UNION ALL SELECT 1 AS sr, QryJournal.Name, Sum(QryJournal.Dr) AS Dr, Sum(QryJournal.Cr) AS Cr From QryJournal Where (((QryJournal.Vdate) < #" + DateFrom + "#)) GROUP BY QryJournal.Name)  AS X GROUP BY X.Name) AS Y UNION ALL SELECT JOURNAL.Vdate, VOUCHERTYPE.Short, VOUCHERINFO.Vnumber, ACCOUNT.Name, JOURNAL.Dr, JOURNAL.Cr, VOUCHERINFO.Narr,VOUCHERTYPE.Short & ' ' & Format(JOURNAL.Vdate,'yyyymmdd' & ' ' & VOUCHERINFO.Vnumber ) AS DocNumber From JOURNAL, ACCOUNT, Voucherinfo, VOUCHERTYPE WHERE (((JOURNAL.Ac_id)=[ACCOUNT].[Ac_id]) AND ((JOURNAL.Vi_id)=[VOUCHERINFO].[Vi_id]) AND ((VOUCHERINFO.Vt_id)=[VOUCHERTYPE].[Vt_id]) AND ((JOURNAL.Vdate)>=#" + DateFrom + "#)))  AS aman INNER JOIN ACCOUNT ON aman.Name = ACCOUNT.Name";

            rptOther.Load(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Report.net/" + getReportFileName(RptId));            
            Viewer.MdiParent = frm.MdiParent;            
            DataTable dt = new DataTable();            
            Database.GetSqlData(Repsql, dt);
            
            rptOther.SetDataSource(dt);
            rptOther.SetParameterValue("Date From", DateFrom);
            rptOther.SetParameterValue("Date To", DateTo);
            rptOther.SetParameterValue("Company Name", DtFirm.Rows[0]["name"].ToString());
            rptOther.SetParameterValue("Company Address1", DtFirm.Rows[0]["Address1"].ToString());
            rptOther.SetParameterValue("Company Address2", DtFirm.Rows[0]["Address2"].ToString());
            rptOther.SetParameterValue("Discruption Of Report", "Cash Book, for the period " + DateFrom.ToString("dd MMM, yyyy") + " To " + DateTo.ToString("dd MMM, yyyy"));
            OpenMode(Mode);
        }

        public void AnnexureB(System.Windows.Forms.Form frm, DateTime DateFrom, DateTime DateTo, string Mode)
        {
            string RptId = "SER26";
            string Repsql;
            Database.GetSqlData("select * from company", DtFirm);
            Repsql = "SELECT VOUCHERTYPE.Short, VOUCHERINFO.Vnumber, VOUCHERINFO.Vdate, VOUCHERINFO.Duedate, VOUCHERINFO.TaxableAmount AS VoucherTaxable, VOUCHERINFO.Totalamount AS VoucherNetAmt, VOUCHERDET.Quantity, VOUCHERDET.Rate_am, VOUCHERDET.Description AS Description, VOUCHERDET.Amount AS ItemAmount, PACKING.Name AS Packing, DESCRIPTION.Description AS OrgDescription, PACKING.Pvalue, PACKING.Utype, TAXCATEGORY.Category_Name, TAXCATEGORY.Commodity_Code, ACCOUNT.Name, ACCOUNT.Address1, ACCOUNT.Address2, ACCOUNT.Phone, ACCOUNT.Email, ACCOUNT.Tin_number, DESCRIPTION.Mark, VOUCHERINFO.Svnum, VOUCHERINFO.Svdate, DESCRIPTION.Des_id, [VOUCHERTYPE].[Short] & ' ' &  Format([VOUCHERINFO].[Vdate],'yyyymmdd' & ' ' & [VOUCHERINFO].[Vnumber]) AS DocNumber, ITEMTAX.Taxable AS ItemTaxable, Sum(ITEMTAX.Tax_Amount) AS Tax";
            Repsql = Repsql + " FROM ((((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id) LEFT JOIN ((VOUCHERDET LEFT JOIN DESCRIPTION ON VOUCHERDET.Des_ac_id = DESCRIPTION.Des_id) LEFT JOIN PACKING ON DESCRIPTION.Pack_id = PACKING.Pack_id) ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN TAXCATEGORY ON VOUCHERDET.Category_Id = TAXCATEGORY.Category_Id) LEFT JOIN ITEMTAX ON (VOUCHERDET.Vi_id = ITEMTAX.Vi_id) AND (VOUCHERDET.Itemsr = ITEMTAX.Itemsr)";
            Repsql = Repsql + " GROUP BY VOUCHERTYPE.Short, VOUCHERINFO.Vnumber, VOUCHERINFO.Vdate, VOUCHERINFO.Duedate, VOUCHERINFO.TaxableAmount, VOUCHERINFO.Totalamount, VOUCHERDET.Quantity, VOUCHERDET.Rate_am, VOUCHERDET.Description, VOUCHERDET.Amount, PACKING.Name, DESCRIPTION.Description, PACKING.Pvalue, PACKING.Utype, TAXCATEGORY.Category_Name, TAXCATEGORY.Commodity_Code, ACCOUNT.Name, ACCOUNT.Address1, ACCOUNT.Address2, ACCOUNT.Phone, ACCOUNT.Email, ACCOUNT.Tin_number, DESCRIPTION.Mark, VOUCHERINFO.Svnum, VOUCHERINFO.Svdate, DESCRIPTION.Des_id, [VOUCHERTYPE].[Short] & ' ' &  Format([VOUCHERINFO].[Vdate],'yyyymmdd' & ' ' & [VOUCHERINFO].[Vnumber]), ITEMTAX.Taxable";
rptOther.Load(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Report.net/" + getReportFileName(RptId));            
            Viewer.MdiParent = frm.MdiParent;            
            DataTable dt = new DataTable();
            Database.GetSqlData(Repsql, dt);           
            rptOther.SetDataSource(dt);

            rptOther.SetParameterValue("Date From", DateFrom);
            rptOther.SetParameterValue("Date To", DateTo);
            rptOther.SetParameterValue("Company Name", DtFirm.Rows[0]["name"].ToString());
            rptOther.SetParameterValue("Company Address1", DtFirm.Rows[0]["Address1"].ToString());
            rptOther.SetParameterValue("Company Address2", DtFirm.Rows[0]["Address2"].ToString());
            rptOther.SetParameterValue("Company T I N", DtFirm.Rows[0]["Tin_no"].ToString());
            rptOther.SetParameterValue("Discruption Of Report", "Annexure B, for the period " + DateFrom.ToString("dd MMM, yyyy") + " To " + DateTo.ToString("dd MMM, yyyy"));
            OpenMode(Mode);
        }

        public void CustomerDetailBillWise(System.Windows.Forms.Form frm, DateTime DateFrom, DateTime DateTo, string AccName, string Mode)
        {
            string RptId = "SER27";
            string Repsql;
            Database.GetSqlData("select * from company", DtFirm);
            Repsql = "select * from QryItemTranjection";
            rptOther.Load(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Report.net/" + getReportFileName(RptId));
            Viewer.MdiParent = frm.MdiParent;
            DataTable dt = new DataTable();

            Database.GetSqlData(Repsql, dt);
            rptOther.SetDataSource(dt);
            rptOther.SetParameterValue("Date From", DateFrom);
            rptOther.SetParameterValue("Date To", DateTo);
            rptOther.SetParameterValue("Company Name", DtFirm.Rows[0]["name"].ToString());
            rptOther.SetParameterValue("Company Address1", DtFirm.Rows[0]["Address1"].ToString());
            rptOther.SetParameterValue("Company Address2", DtFirm.Rows[0]["Address2"].ToString());
            if (AccName == "/*All*/")
            {
                rptOther.SetParameterValue("Account", "/*All*/");
            }
            else
            {
                rptOther.SetParameterValue("Account", AccName);
            }
            rptOther.SetParameterValue("Discruption Of Report", "Customer Detail Bill Wise, for the period " + DateFrom.ToString("dd MMM, yyyy") + " To " + DateTo.ToString("dd MMM, yyyy"));
            OpenMode(Mode);
        }

        public void CustomerDetailItemWise(System.Windows.Forms.Form frm, DateTime DateFrom, DateTime DateTo,string AccName, string Mode)
        {
            string RptId = "SER28";
            string Repsql;
            Database.GetSqlData("select * from company", DtFirm);
            Repsql = "select * from QryItemTranjection";
            rptOther.Load(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Report.net/" + getReportFileName(RptId));           
            Viewer.MdiParent = frm.MdiParent;            
            DataTable dt = new DataTable();
            Database.GetSqlData(Repsql, dt);            
            rptOther.SetDataSource(dt);

            rptOther.SetParameterValue("Date From", DateFrom);
            rptOther.SetParameterValue("Date To", DateTo);
            rptOther.SetParameterValue("Company Name", DtFirm.Rows[0]["name"].ToString());
            rptOther.SetParameterValue("Company Address1", DtFirm.Rows[0]["Address1"].ToString());
            rptOther.SetParameterValue("Company Address2", DtFirm.Rows[0]["Address2"].ToString());
            if (AccName == "/*All*/")
            {
                rptOther.SetParameterValue("Account", "/*All*/");
            }
            else
            {
                rptOther.SetParameterValue("Account", AccName);
            }
            rptOther.SetParameterValue("Discruption Of Report", "Customer Detail Item Wise, for the period " + DateFrom.ToString("dd MMM, yyyy") + " To " + DateTo.ToString("dd MMM, yyyy"));
            OpenMode(Mode);
        }

        public void AnnexureA(System.Windows.Forms.Form frm, DateTime DateFrom, DateTime DateTo, string Mode)
        {
            string RptId = "SER32";
            string Repsql;
            Database.GetSqlData("select * from company", DtFirm);
            Repsql = "SELECT VOUCHERTYPE.Short, VOUCHERINFO.Vnumber, VOUCHERINFO.Vdate, VOUCHERINFO.Duedate, VOUCHERINFO.TaxableAmount AS VoucherTaxable, VOUCHERINFO.Totalamount AS VoucherNetAmt, VOUCHERDET.Quantity, VOUCHERDET.Rate_am, VOUCHERDET.Description AS Description, VOUCHERDET.Amount AS ItemAmount, PACKING.Name AS Packing, DESCRIPTION.Description AS OrgDescription, PACKING.Pvalue, PACKING.Utype, TAXCATEGORY.Category_Name, TAXCATEGORY.Commodity_Code, ACCOUNT.Name, ACCOUNT.Address1, ACCOUNT.Address2, ACCOUNT.Phone, ACCOUNT.Email, ACCOUNT.Tin_number, DESCRIPTION.Mark, VOUCHERINFO.Svnum, VOUCHERINFO.Svdate, DESCRIPTION.Des_id, [VOUCHERTYPE].[Short] & ' ' &  Format([VOUCHERINFO].[Vdate],'yyyymmdd' & ' ' & [VOUCHERINFO].[Vnumber]) AS DocNumber, ITEMTAX.Taxable AS ItemTaxable, Sum(ITEMTAX.Tax_Amount) AS Tax";
            Repsql = Repsql + " FROM ((((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id) LEFT JOIN ((VOUCHERDET LEFT JOIN DESCRIPTION ON VOUCHERDET.Des_ac_id = DESCRIPTION.Des_id) LEFT JOIN PACKING ON DESCRIPTION.Pack_id = PACKING.Pack_id) ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN TAXCATEGORY ON VOUCHERDET.Category_Id = TAXCATEGORY.Category_Id) LEFT JOIN ITEMTAX ON (VOUCHERDET.Vi_id = ITEMTAX.Vi_id) AND (VOUCHERDET.Itemsr = ITEMTAX.Itemsr)";
            Repsql = Repsql + " GROUP BY VOUCHERTYPE.Short, VOUCHERINFO.Vnumber, VOUCHERINFO.Vdate, VOUCHERINFO.Duedate, VOUCHERINFO.TaxableAmount, VOUCHERINFO.Totalamount, VOUCHERDET.Quantity, VOUCHERDET.Rate_am, VOUCHERDET.Description, VOUCHERDET.Amount, PACKING.Name, DESCRIPTION.Description, PACKING.Pvalue, PACKING.Utype, TAXCATEGORY.Category_Name, TAXCATEGORY.Commodity_Code, ACCOUNT.Name, ACCOUNT.Address1, ACCOUNT.Address2, ACCOUNT.Phone, ACCOUNT.Email, ACCOUNT.Tin_number, DESCRIPTION.Mark, VOUCHERINFO.Svnum, VOUCHERINFO.Svdate, DESCRIPTION.Des_id, [VOUCHERTYPE].[Short] & ' ' &  Format([VOUCHERINFO].[Vdate],'yyyymmdd' & ' ' & [VOUCHERINFO].[Vnumber]), ITEMTAX.Taxable";
            rptOther.Load(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Report.net/" + getReportFileName(RptId));           
            Viewer.MdiParent = frm.MdiParent;            
            DataTable dt = new DataTable();
            Database.GetSqlData(Repsql, dt);          
            rptOther.SetDataSource(dt);

            rptOther.SetParameterValue("Date From", DateFrom);
            rptOther.SetParameterValue("Date To", DateTo);
            rptOther.SetParameterValue("Company Name", DtFirm.Rows[0]["name"].ToString());
            rptOther.SetParameterValue("Company Address1", DtFirm.Rows[0]["Address1"].ToString());
            rptOther.SetParameterValue("Company Address2", DtFirm.Rows[0]["Address2"].ToString());
            rptOther.SetParameterValue("Company T I N", DtFirm.Rows[0]["Tin_no"].ToString());            
            rptOther.SetParameterValue("Discruption Of Report", "Annexure A, for the period " + DateFrom.ToString("dd MMM, yyyy") + " To " + DateTo.ToString("dd MMM, yyyy"));
            OpenMode(Mode);
        }

        public void OpeningTrial(System.Windows.Forms.Form frm, string Mode)
        {
            string RptId = "SER33";
            string Repsql;
            Database.GetSqlData("select * from company", DtFirm);
            Repsql = "select * from ACCOUNT" ;
            rptOther.Load(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Report.net/" + getReportFileName(RptId));            
            Viewer.MdiParent = frm.MdiParent;            
            DataTable dt = new DataTable();
            Database.GetSqlData(Repsql, dt);
            rptOther.SetDataSource(dt);

            rptOther.SetParameterValue("Company Name", DtFirm.Rows[0]["name"].ToString());
            rptOther.SetParameterValue("Company Address1", DtFirm.Rows[0]["Address1"].ToString());
            rptOther.SetParameterValue("Company Address2", DtFirm.Rows[0]["Address2"].ToString());
            rptOther.SetParameterValue("Discruption Of Report", "Opening Trial");
            OpenMode(Mode);
        }

        public void StandardTrial(System.Windows.Forms.Form frm, DateTime DateTo, string Mode)
        {
            string RptId = "SER34";
            string Repsql="";
            Database.GetSqlData("select * from company", DtFirm);           
            Repsql = "SELECT X.Name, sum(X.Dr) AS Dr, sum(X.Cr) AS Cr FROM (SELECT QryJournal.ACCOUNT.Name, sum(QryJournal.Dr) as Dr, sum(QryJournal.Cr) as Cr From QryJournal Where (((QryJournal.Vdate) <= #" + DateTo + "#))GROUP BY QryJournal.ACCOUNT.Name UNION All SELECT QryAccountinfo.Name, QryAccountinfo.Dr as Dr, QryAccountinfo.Cr as Cr FROM QryAccountinfo)  AS X GROUP BY x.Name";
            rptOther.Load(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Report.net/" + getReportFileName(RptId));           
            Viewer.MdiParent = frm.MdiParent;            
            DataTable dt = new DataTable();
            Database.GetSqlData(Repsql, dt);
           
            rptOther.SetDataSource(dt);
            rptOther.SetParameterValue("Company Name", DtFirm.Rows[0]["name"].ToString());
            rptOther.SetParameterValue("Company Address1", DtFirm.Rows[0]["Address1"].ToString());
            rptOther.SetParameterValue("Company Address2", DtFirm.Rows[0]["Address2"].ToString());
            rptOther.SetParameterValue("Discruption Of Report", "Standard Trial, Upto " + DateTo.ToString("dd MMM, yyyy"));
            OpenMode(Mode);
        }

        public void BrokerDetailCustomerWise(System.Windows.Forms.Form frm, DateTime DateFrom, DateTime DateTo, String accnm, string Mode)
        {
            string RptId = "SER35";
            string Repsql;
            Database.GetSqlData("select * from company", DtFirm);
            Repsql = "select * from QryItemTranjection";
            rptOther.Load(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Report.net/" + getReportFileName(RptId));
            Viewer.MdiParent = frm.MdiParent;            
            DataTable dt = new DataTable();
            Database.GetSqlData(Repsql, dt);
            rptOther.SetDataSource(dt);

            rptOther.SetParameterValue("Date From", DateFrom);
            rptOther.SetParameterValue("Date To", DateTo);
            rptOther.SetParameterValue("Company Name", DtFirm.Rows[0]["name"].ToString());
            rptOther.SetParameterValue("Company Address1", DtFirm.Rows[0]["Address1"].ToString());
            rptOther.SetParameterValue("Company Address2", DtFirm.Rows[0]["Address2"].ToString());
            rptOther.SetParameterValue("Discruption Of Report", "Broker Detail Customer Wise, for the period " + DateFrom.ToString("dd MMM, yyyy") + " To " + DateTo.ToString("dd MMM, yyyy"));
            if (accnm == "/*All*/")
            {
                rptOther.SetParameterValue("Broker", "/*All*/");
            }
            else
            {
                rptOther.SetParameterValue("Broker", accnm);
            }
            OpenMode(Mode);
        }

        public void BrokerDetailItemWise(System.Windows.Forms.Form frm, DateTime DateFrom, DateTime DateTo, String accnm, string Mode)
        {
            string RptId = "SER36";
            string Repsql;
            Database.GetSqlData("select * from company", DtFirm);
            Repsql = "select * from QryItemTranjection";
            rptOther.Load(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Report.net/" + getReportFileName(RptId));            
            Viewer.MdiParent = frm.MdiParent;            
            DataTable dt = new DataTable();
            Database.GetSqlData(Repsql, dt);
            
            rptOther.SetDataSource(dt);
            rptOther.SetParameterValue("Date From", DateFrom);
            rptOther.SetParameterValue("Date To", DateTo);
            rptOther.SetParameterValue("Company Name", DtFirm.Rows[0]["name"].ToString());
            rptOther.SetParameterValue("Company Address1", DtFirm.Rows[0]["Address1"].ToString());
            rptOther.SetParameterValue("Company Address2", DtFirm.Rows[0]["Address2"].ToString());
            rptOther.SetParameterValue("Discruption Of Report", "Broker Detail Item Wise, for the period " + DateFrom.ToString("dd MMM, yyyy") + " To " + DateTo.ToString("dd MMM, yyyy"));
            if (accnm == "/*All*/")
            {
                rptOther.SetParameterValue("Broker", "/*All*/");
            }
            else
            {
                rptOther.SetParameterValue("Broker", accnm);
            }
            OpenMode(Mode);
        }

        public void SupplierDetailBillWise(System.Windows.Forms.Form frm, DateTime DateFrom, DateTime DateTo, string AccName, string Mode)
        {
            string RptId = "SER41";
            string Repsql;
            Database.GetSqlData("select * from company", DtFirm);
            Repsql = "select * from QryItemTranjection";
            rptOther.Load(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Report.net/" + getReportFileName(RptId));
            Viewer.MdiParent = frm.MdiParent;
            DataTable dt = new DataTable();
            Database.GetSqlData(Repsql, dt);
            
            rptOther.SetDataSource(dt);
            rptOther.SetParameterValue("Date From", DateFrom);
            rptOther.SetParameterValue("Date To", DateTo);
            rptOther.SetParameterValue("Company Name", DtFirm.Rows[0]["name"].ToString());
            rptOther.SetParameterValue("Company Address1", DtFirm.Rows[0]["Address1"].ToString());
            rptOther.SetParameterValue("Company Address2", DtFirm.Rows[0]["Address2"].ToString());
            rptOther.SetParameterValue("Discruption Of Report", "Suplier Detail Bill Wise, for the period " + DateFrom.ToString("dd MMM, yyyy") + " To " + DateTo.ToString("dd MMM, yyyy"));
            if (AccName == "/*All*/")
            {
                rptOther.SetParameterValue("Suplier", "/*All*/");
            }
            else
            {
                rptOther.SetParameterValue("Suplier", AccName);
            }
            OpenMode(Mode);
        }

        public void SupplierDetailItemWise(System.Windows.Forms.Form frm, DateTime DateFrom, DateTime DateTo, string AccName, string Mode)
        {
            string RptId = "SER42";
            string Repsql;
            Database.GetSqlData("select * from company", DtFirm);
            Repsql = "select * from QryItemTranjection";
            rptOther.Load(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Report.net/" + getReportFileName(RptId));           
            Viewer.MdiParent = frm.MdiParent;            
            DataTable dt = new DataTable();
            Database.GetSqlData(Repsql, dt);
            
            rptOther.SetDataSource(dt);
            rptOther.SetParameterValue("Date From", DateFrom);
            rptOther.SetParameterValue("Date To", DateTo);
            rptOther.SetParameterValue("Company Name", DtFirm.Rows[0]["name"].ToString());
            rptOther.SetParameterValue("Company Address1", DtFirm.Rows[0]["Address1"].ToString());
            rptOther.SetParameterValue("Company Address2", DtFirm.Rows[0]["Address2"].ToString());
            if (AccName == "/*All*/")
            {
                rptOther.SetParameterValue("Suplier", "/*All*/");
            }
            else
            {
                rptOther.SetParameterValue("Suplier", AccName);
            }
            rptOther.SetParameterValue("Discruption Of Report", "Supplier Detail Item Wise, for the period " + DateFrom.ToString("dd MMM, yyyy") + " To " + DateTo.ToString("dd MMM, yyyy"));
            OpenMode(Mode);
        }

        public void MovedAccountSummary(System.Windows.Forms.Form frm, DateTime DateFrom, DateTime DateTo, string Mode)
        {
            string RptId = "SER47";
            string Repsql;
            Database.GetSqlData("select * from company", DtFirm);
            Repsql = "select * from QryJournal" ;
            rptOther.Load(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Report.net/" + getReportFileName(RptId));           
            Viewer.MdiParent = frm.MdiParent;            
            DataTable dt = new DataTable();
            Database.GetSqlData(Repsql, dt);
            
            rptOther.SetDataSource(dt);
            rptOther.SetParameterValue("Date From", DateFrom);
            rptOther.SetParameterValue("Date To", DateTo);
            rptOther.SetParameterValue("Company Name", DtFirm.Rows[0]["name"].ToString());
            rptOther.SetParameterValue("Company Address1", DtFirm.Rows[0]["Address1"].ToString());
            rptOther.SetParameterValue("Company Address2", DtFirm.Rows[0]["Address2"].ToString());
            rptOther.SetParameterValue("Discruption Of Report", "Moved Account Summary, for the period " + DateFrom.ToString("dd MMM, yyyy") + " To " + DateTo.ToString("dd MMM, yyyy"));
            OpenMode(Mode);
        }

        public void AccountGroupBalance(System.Windows.Forms.Form frm, DateTime DateFrom, DateTime DateTo, string Mode)
        {
            string RptId = "SER49";
            string Repsql;
            Database.GetSqlData("select * from company", DtFirm);
            Repsql = "SELECT X.Name, Sum(X.Dr) AS Dr, Sum(X.Cr) AS Cr, OTHER.Name FROM ((SELECT QryJournal.ACCOUNT.Name, sum(QryJournal.Dr) as Dr, sum(QryJournal.Cr) as Cr From QryJournal Where (((QryJournal.Vdate) <= #" + DateTo + "#))GROUP BY QryJournal.ACCOUNT.Name UNION ALL SELECT QryAccountinfo.Name, QryAccountinfo.Dr as Dr, QryAccountinfo.Cr as Cr FROM QryAccountinfo)  AS X LEFT JOIN ACCOUNT ON X.Name = ACCOUNT.Name) LEFT JOIN OTHER ON ACCOUNT.Loc_id = OTHER.Oth_id GROUP BY X.Name, OTHER.Name";
            rptOther.Load(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Report.net/" + getReportFileName(RptId));         
            Viewer.MdiParent = frm.MdiParent;            
            DataTable dt = new DataTable();
            Database.GetSqlData(Repsql, dt);
          
            rptOther.SetDataSource(dt);
            rptOther.SetParameterValue("Company Name", DtFirm.Rows[0]["name"].ToString());
            rptOther.SetParameterValue("Company Address1", DtFirm.Rows[0]["Address1"].ToString());
            rptOther.SetParameterValue("Company Address2", DtFirm.Rows[0]["Address2"].ToString());
            rptOther.SetParameterValue("Discruption Of Report", "Account Group Balance, for the period " + DateFrom.ToString("dd MMM, yyyy") + " To " + DateTo.ToString("dd MMM, yyyy"));
            OpenMode(Mode);
        }

        public void AddressPrinting(System.Windows.Forms.Form frm, String accname, string Mode)
        {
            string RptId ="SER50";
            string Repsql;
            Database.GetSqlData("select * from company", DtFirm);
            Repsql = "SELECT * FROM ACCOUNT";
            rptOther.Load(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Report.net/" + getReportFileName(RptId));           
            Viewer.MdiParent = frm;            
            DataTable dt = new DataTable();
            Database.GetSqlData(Repsql, dt);
           
            rptOther.SetDataSource(dt);
            rptOther.SetParameterValue("Account", accname);
            OpenMode(Mode);
        }

        public void CustomerBrokerage(System.Windows.Forms.Form frm, DateTime DateFrom, DateTime DateTo, string AccName,string Mode)
        {
            string RptId = "SER51";
            string Repsql;
            Database.GetSqlData("select * from company", DtFirm);
            Repsql = "select * from QryItemTranjection";
            rptOther.Load(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "/Report.net/" + getReportFileName(RptId));
            Viewer.MdiParent = frm.MdiParent;
            DataTable dt = new DataTable();
            Database.GetSqlData(Repsql, dt);
            rptOther.SetDataSource(dt);
            rptOther.SetParameterValue("Date From", DateFrom);
            rptOther.SetParameterValue("Date To", DateTo);
            rptOther.SetParameterValue("Company Name", DtFirm.Rows[0]["name"].ToString());
            rptOther.SetParameterValue("Company Address1", DtFirm.Rows[0]["Address1"].ToString());
            rptOther.SetParameterValue("Company Address2", DtFirm.Rows[0]["Address2"].ToString());
            rptOther.SetParameterValue("Discruption Of Report", "Customer Brokerage, for the period " + DateFrom.ToString("dd MMM, yyyy") + " To " + DateTo.ToString("dd MMM, yyyy"));
            if (AccName == "/*All*/")
            {
                rptOther.SetParameterValue("Customer", "/*All*/");
            }
            else
            {
                rptOther.SetParameterValue("Customer", AccName);
            }
            OpenMode(Mode);
        }
  
        public String getReportFileName(string rptid)
        {
            String reportFileName = "";
            DataTable dtReportFileName = new DataTable("vouchertype");
            Database.GetSqlData("select * from vouchertype where vt_id='" + rptid + "' ", dtReportFileName);
            if (dtReportFileName.Rows.Count > 0)
            {
                reportFileName = dtReportFileName.Rows[0]["ReportName"].ToString();
            }
            return reportFileName;
        }
    }
}
