using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;

namespace faspi
{
    public partial class frm_GSTR_3B : Form
    {
        static Object misValue = System.Reflection.Missing.Value;
        static Excel.Application apl = new Microsoft.Office.Interop.Excel.Application();
        Excel.Workbook wb;
        Excel.Worksheet ws;
        public string formattype = "";
        DateTime dt1 = new DateTime();
        DateTime dt2 = new DateTime();
        int month = 0;
        int year = 0;
        int stdate = 1;
        int lastdate = 0;
        string mnthname = "";

        public frm_GSTR_3B()
        {
            InitializeComponent();
        }

        private void frm_GSTR_3B_Load(object sender, EventArgs e)
        {
            dateTimePicker1.CustomFormat = "MMMM yyyy";
            dateTimePicker1.Value = Database.ldate;
            dateTimePicker1.MinDate = Database.stDate;
            dateTimePicker1.MaxDate = Database.ldate;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            month = dateTimePicker1.Value.Month;
            year = dateTimePicker1.Value.Year;
            stdate = 1;
            lastdate = DateTime.DaysInMonth(year, month);
            if (month == 1)
            {
                mnthname = "January";
            }
            else if (month == 2)
            {
                mnthname = "February";
            }
            else if (month == 3)
            {
                mnthname = "March";
            }
            else if (month == 4)
            {
                mnthname = "April";
            }
            else if (month == 5)
            {
                mnthname = "May";
            }
            else if (month == 6)
            {
                mnthname = "June";
            }
            else if (month == 7)
            {
                mnthname = "July";
            }
            else if (month == 8)
            {
                mnthname = "August";
            }
            else if (month == 9)
            {
                mnthname = "September";
            }
            else if (month == 10)
            {
                mnthname = "October";
            }
            else if (month == 11)
            {
                mnthname = "November";
            }
            else if (month == 12)
            {
                mnthname = "December";
            }

            string tPath = "";
            if (formattype == "pdf")
            {
                DialogResult val = folderBrowserDialog1.ShowDialog(this);
                string pathtobackup = "";

                if (val == DialogResult.OK)
                {
                    pathtobackup = folderBrowserDialog1.SelectedPath.ToString() + "\\GSTR-3B" + mnthname + ".pdf";
                }
                tPath = pathtobackup;
                if (tPath == "")
                {
                    return;
                }
            }

            dt1 = new DateTime(year, month, stdate);
            dt2 = new DateTime(year, month, lastdate);

            if (formattype == "pdf")
            {
                GSTR3B(tPath);
                MessageBox.Show("Report is Ready.");
            }
            else
            {
                GSTR3Baccess(tPath);
            }

            GC.Collect();
            this.Close();
            this.Dispose();
        }

        public void GSTR3Baccess(string tPath)
        {
            DataTable DtFirm = new DataTable();
            Database.GetSqlData("SELECT COMPANY.Name as Name , COMPANY.Cst_no as CompanyMobileno , COMPANY.Tin_no as Tin_no, COMPANY.Email as CompanyEmail, COMPANY.Address1 as Address1, COMPANY.Address2 as Address2, COMPANY.Contactno as CompanyLandline, COMPANY.BankName as BankName,COMPANY.IFSC as IFSC,COMPANY.AccountNo as AccountNo,State.Sname as CompanyState, State.GSTCode as Statecode FROM COMPANY LEFT JOIN State ON COMPANY.CState_id = State.State_id", DtFirm);
            wb = (Excel.Workbook)apl.Workbooks.Open(Application.StartupPath + "\\efile\\GSTR3B.xls", true, true, misValue, null, null, false, misValue, null, false, false, misValue, misValue, misValue, false);
            Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets["GSTR-3B"];

            ws.Cells[5, 3] = DtFirm.Rows[0]["Tin_no"].ToString();
            ws.Cells[5, 7] = year.ToString();

            ws.Cells[6, 3] = DtFirm.Rows[0]["Name"].ToString();
            ws.Cells[6, 7] = mnthname;

            DataTable dtgstr = new DataTable();
            Database.GetSqlData("SELECT VOUCHERINFO.RCM, VOUCHERINFO.ITC, TAXCATEGORY.Item_Type, " + access_sql.fnstring("VOUCHERTYPE.Type='Return' or  VOUCHERTYPE.Type='P Return'", "-1* Voucherdet.Taxabelamount", "Voucherdet.Taxabelamount") + " AS Taxableamount, " + access_sql.fnstring("VOUCHERTYPE.Type='Return' or  VOUCHERTYPE.Type='P Return'", "-1* Voucherdet.taxamt3", "Voucherdet.taxamt3") + " AS IGST,  " + access_sql.fnstring("VOUCHERTYPE.Type='Return' or  VOUCHERTYPE.Type='P Return'", "-1* Voucherdet.taxamt1", "Voucherdet.taxamt1") + " AS CGST, " + access_sql.fnstring("VOUCHERTYPE.Type='Return' or  VOUCHERTYPE.Type='P Return'", "-1* Voucherdet.taxamt2", "Voucherdet.taxamt2") + " AS SGST, " + access_sql.fnstring("VOUCHERTYPE.Type='Return' or  VOUCHERTYPE.Type='P Return'", "-1* Voucherdet.taxamt4", "Voucherdet.taxamt4") + " AS Cess, State.Sname, ACCOUNT.RegStatus, VOUCHERTYPE.Type, VOUCHERINFO.Vdate, Voucherdet.TotTaxPer FROM ((((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN Voucherdet ON VOUCHERINFO.Vi_id = Voucherdet.Vi_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id) LEFT JOIN State ON ACCOUNT.State_id = State.State_id) LEFT JOIN TAXCATEGORY ON Voucherdet.Category_Id = TAXCATEGORY.Category_Id WHERE (((TAXCATEGORY.Item_Type)<>'') AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + "))", dtgstr);

            ws.Cells[11, 3] = dtgstr.Compute("sum(Taxableamount)", "(type='Sale' or type='Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And TotTaxPer >0").ToString();
            
            ws.Cells[11, 4] = dtgstr.Compute("sum(IGST)", "(type='Sale' or type='Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And TotTaxPer >0").ToString();
            
            ws.Cells[11, 5] = dtgstr.Compute("sum(CGST)", "(type='Sale' or type='Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And TotTaxPer >0").ToString();
            
            ws.Cells[11, 7] = dtgstr.Compute("sum(Cess)", "(type='Sale' or type='Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And TotTaxPer >0").ToString();
            
            ws.Cells[13, 3] = dtgstr.Compute("sum(Taxableamount)", "(type='Sale' or type='Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And TotTaxPer =0").ToString();
            
            ws.Cells[14, 3] = dtgstr.Compute("sum(Taxableamount)", "(type='RCM') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And RCM =" + access_sql.Singlequote + "true" + access_sql.Singlequote).ToString();
           
            ws.Cells[14, 4] = dtgstr.Compute("sum(IGST)", "(type='RCM') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And RCM =" + access_sql.Singlequote + "true" + access_sql.Singlequote).ToString();
           
            ws.Cells[14, 5] = dtgstr.Compute("sum(CGST)", "(type='RCM') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And RCM =" + access_sql.Singlequote + "true" + access_sql.Singlequote).ToString();
           
            ws.Cells[14, 7] = dtgstr.Compute("sum(Cess)", "(type='RCM') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And RCM =" + access_sql.Singlequote + "true" + access_sql.Singlequote).ToString();
           
            ws.Cells[24, 3] = dtgstr.Compute("sum(IGST)", "(type='RCM') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And ITC =" + access_sql.Singlequote + "true" + access_sql.Singlequote).ToString();
            
             ws.Cells[24, 4] = dtgstr.Compute("sum(CGST)", "(type='RCM') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And ITC =" + access_sql.Singlequote + "true" + access_sql.Singlequote).ToString();
           
            ws.Cells[24, 6] = dtgstr.Compute("sum(Cess)", "(type='RCM') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And ITC =" + access_sql.Singlequote + "true" + access_sql.Singlequote).ToString();
            
            ws.Cells[26, 3] = dtgstr.Compute("sum(IGST)", "(type='Purchase' or type='P Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ")").ToString();
            
            ws.Cells[26, 4] = dtgstr.Compute("sum(CGST)", "(type='Purchase' or type='P Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ")").ToString();
            
            ws.Cells[26, 6] = dtgstr.Compute("sum(Cess)", "(type='Purchase' or type='P Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ")").ToString();
            
            ws.Cells[32, 3] = dtgstr.Compute("sum(IGST)", "(type='RCM') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And ITC =" + access_sql.Singlequote + "false" + access_sql.Singlequote).ToString();
           
            ws.Cells[32, 4] = dtgstr.Compute("sum(CGST)", "(type='RCM') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And ITC =" + access_sql.Singlequote + "false" + access_sql.Singlequote).ToString();
           
            ws.Cells[32, 6] = dtgstr.Compute("sum(Cess)", "(type='RCM') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And ITC =" + access_sql.Singlequote + "false" + access_sql.Singlequote).ToString();
           
            ws.Cells[39, 4] = dtgstr.Compute("sum(Taxableamount)", "(type='Purchase' or type='P Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And (TotTaxPer = 0 or RegStatus='Composition Dealer')   And (Sname<>'" + funs.Select_state_nm(Database.CompanyState_id) + "')  ").ToString();
            
            ws.Cells[39, 5] = dtgstr.Compute("sum(Taxableamount)", "(type='Purchase' or type='P Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And (TotTaxPer = 0 or RegStatus='Composition Dealer')   And Sname='" + funs.Select_state_nm(Database.CompanyState_id) + "'  ").ToString();
           
            ws.Cells[79, 3] = dtgstr.Compute("sum(Taxableamount)", "(type='Sale' or type='Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And Sname<>'" + funs.Select_state_nm(Database.CompanyState_id) + "' And RegStatus='Unregistered'").ToString();
            
            ws.Cells[79, 4] = dtgstr.Compute("sum(IGST)", "(type='Sale' or type='Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And Sname<>'" + funs.Select_state_nm(Database.CompanyState_id) + "' And RegStatus='Unregistered'").ToString();
            
            ws.Cells[79, 5] = dtgstr.Compute("sum(Taxableamount)", "(type='Sale' or type='Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And Sname<>'" + funs.Select_state_nm(Database.CompanyState_id) + "' And RegStatus='Composition Dealer'").ToString();
           
            ws.Cells[79, 6] = dtgstr.Compute("sum(IGST)", "(type='Sale' or type='Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And Sname<>'" + funs.Select_state_nm(Database.CompanyState_id) + "' And RegStatus='Composition Dealer'").ToString();
           
            apl.Visible = true;
            GC.Collect();
        }

        public void GSTR3B(string tPath)
        {
            DataTable DtFirm = new DataTable();
            Database.GetSqlData("SELECT COMPANY.Name as Name , COMPANY.Cst_no as CompanyMobileno , COMPANY.Tin_no as Tin_no, COMPANY.Email as CompanyEmail, COMPANY.Address1 as Address1, COMPANY.Address2 as Address2, COMPANY.Contactno as CompanyLandline, COMPANY.BankName as BankName,COMPANY.IFSC as IFSC,COMPANY.AccountNo as AccountNo,State.Sname as CompanyState, State.GSTCode as Statecode FROM COMPANY LEFT JOIN State ON COMPANY.CState_id = State.State_id", DtFirm);
            
            FileStream fs = new FileStream(tPath, FileMode.Create, FileAccess.Write, FileShare.None);
            Document document;
            document = new Document(PageSize.A4, 15f, 15f, 15f, 15f);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            string watermarkText = "Prepared By: Marwari Software";
            float fontSize = 25;
            float xPosition = 300;
            float yPosition = 400;
            float angle = 0;
            PdfContentByte under = writer.DirectContentUnder;
            BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED);
            under.BeginText();
            under.SetColorFill(iTextSharp.text.pdf.CMYKColor.LIGHT_GRAY);
            under.SetFontAndSize(baseFont, fontSize);
            under.ShowTextAligned(PdfContentByte.ALIGN_CENTER, watermarkText, xPosition, yPosition, angle);
            under.EndText();

            iTextSharp.text.Font font_body = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            iTextSharp.text.Font font_body_bold = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD);

            var Head = new Paragraph("FORM GSTR-3B", font_body_bold);
            Head.Alignment = 1;
            document.Add(Head);

            Head = new Paragraph("[See rule 61(5)]", font_body);
            Head.Alignment = 1;
            document.Add(Head);

            PdfPTable table = new PdfPTable(2);
            table.WidthPercentage = 25;
            table.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(new PdfPCell(new Phrase("Year", font_body)));
            table.AddCell(year.ToString());
            table.AddCell(new PdfPCell(new Phrase("Month", font_body)));
            table.AddCell(mnthname);
            document.Add(table);

            Head = new Paragraph("" + Environment.NewLine, font_body);
            document.Add(Head);

            table = new PdfPTable(3);
            table.WidthPercentage = 100;
            float[] widths = new float[] { 5f, 35f, 60f };
            table.SetWidths(widths);
            table.AddCell(new PdfPCell(new Phrase("1", font_body)));
            table.AddCell(new PdfPCell(new Phrase("GSTIN", font_body)));
            table.AddCell(DtFirm.Rows[0]["Tin_no"].ToString());
            table.AddCell(new PdfPCell(new Phrase("2", font_body)));
            table.AddCell(new PdfPCell(new Phrase("Legal name of the registered person", font_body)));
            table.AddCell(DtFirm.Rows[0]["Name"].ToString());
            document.Add(table);

            var mater = new Paragraph("3.1 Details of Outward Supplies and inward supplies liable to reverse charge" + Environment.NewLine + Environment.NewLine, font_body);
            document.Add(mater);

            table = new PdfPTable(6);
            table.WidthPercentage = 100;
            widths = new float[] { 50f, 10f, 10f, 10f, 10f, 10f };
            table.SetWidths(widths);

            table.AddCell(new PdfPCell(new Phrase("Nature of Supplies", font_body)));
            table.AddCell(new PdfPCell(new Phrase("Total Taxable value", font_body)));
            table.AddCell(new PdfPCell(new Phrase("Integrated Tax", font_body)));
            table.AddCell(new PdfPCell(new Phrase("Central Tax", font_body)));
            table.AddCell(new PdfPCell(new Phrase("State / UT Tax", font_body)));
            table.AddCell(new PdfPCell(new Phrase("Cess", font_body)));

            DataTable dtgstr = new DataTable();
            Database.GetSqlData("SELECT VOUCHERINFO.RCM, VOUCHERINFO.ITC, TAXCATEGORY.Item_Type, " + access_sql.fnstring("VOUCHERTYPE.Type='Return' or  VOUCHERTYPE.Type='P Return'", "-1* Voucherdet.Taxabelamount", "Voucherdet.Taxabelamount") + " AS Taxableamount, " + access_sql.fnstring("VOUCHERTYPE.Type='Return' or  VOUCHERTYPE.Type='P Return'", "-1* Voucherdet.taxamt3", "Voucherdet.taxamt3") + " AS IGST,  " + access_sql.fnstring("VOUCHERTYPE.Type='Return' or  VOUCHERTYPE.Type='P Return'", "-1* Voucherdet.taxamt1", "Voucherdet.taxamt1") + " AS CGST, " + access_sql.fnstring("VOUCHERTYPE.Type='Return' or  VOUCHERTYPE.Type='P Return'", "-1* Voucherdet.taxamt2", "Voucherdet.taxamt2") + " AS SGST, " + access_sql.fnstring("VOUCHERTYPE.Type='Return' or  VOUCHERTYPE.Type='P Return'", "-1* Voucherdet.taxamt4", "Voucherdet.taxamt4") + " AS Cess, State.Sname, ACCOUNT.RegStatus, VOUCHERTYPE.Type, VOUCHERINFO.Vdate, VOUCHERINFO.SVdate,Voucherdet.TotTaxPer FROM ((((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN Voucherdet ON VOUCHERINFO.Vi_id = Voucherdet.Vi_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id) LEFT JOIN State ON ACCOUNT.State_id = State.State_id) LEFT JOIN TAXCATEGORY ON Voucherdet.Category_Id = TAXCATEGORY.Category_Id WHERE (((TAXCATEGORY.Item_Type)<>'') AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + "))", dtgstr);
            table.AddCell(new PdfPCell(new Phrase("(a) Outward taxable  supplies  (other than zero rated, nil rated and exempted)", font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(Taxableamount)", "(type='Sale' or type='Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And TotTaxPer >0").ToString(), font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(IGST)", "(type='Sale' or type='Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And TotTaxPer >0").ToString(), font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(CGST)", "(type='Sale' or type='Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And TotTaxPer >0").ToString(), font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(SGST)", "(type='Sale' or type='Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And TotTaxPer >0").ToString(), font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(Cess)", "(type='Sale' or type='Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And TotTaxPer >0").ToString(), font_body)));

            table.AddCell(new PdfPCell(new Phrase("(b) Outward taxable  supplies  (zero rated )", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));

            table.AddCell(new PdfPCell(new Phrase("(c) Other outward supplies (Nil rated, exempted)", font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(Taxableamount)", "(type='Sale' or type='Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And TotTaxPer =0").ToString(), font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(IGST)", "(type='Sale' or type='Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And TotTaxPer =0").ToString(), font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(CGST)", "(type='Sale' or type='Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And TotTaxPer =0").ToString(), font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(SGST)", "(type='Sale' or type='Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And TotTaxPer =0").ToString(), font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(Cess)", "(type='Sale' or type='Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And TotTaxPer =0").ToString(), font_body)));

            table.AddCell(new PdfPCell(new Phrase("(d) Inward supplies (liable to reverse charge)", font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(Taxableamount)", "(type='RCM') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And RCM =" + access_sql.Singlequote + "true" + access_sql.Singlequote).ToString(), font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(IGST)", "(type='RCM') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And RCM =" + access_sql.Singlequote + "true" + access_sql.Singlequote).ToString(), font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(CGST)", "(type='RCM') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And RCM =" + access_sql.Singlequote + "true" + access_sql.Singlequote).ToString(), font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(SGST)", "(type='RCM') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And RCM =" + access_sql.Singlequote + "true" + access_sql.Singlequote).ToString(), font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(Cess)", "(type='RCM') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And RCM =" + access_sql.Singlequote + "true" + access_sql.Singlequote).ToString(), font_body)));

            table.AddCell(new PdfPCell(new Phrase("(e) Non-GST outward supplies", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));

            document.Add(table);

            mater = new Paragraph("3.2   Of the supplies shown in 3.1 (a)  above, details of inter-State supplies made to unregistered persons, composition taxable persons and UIN holders" + Environment.NewLine + Environment.NewLine, font_body);
            document.Add(mater);

            table = new PdfPTable(4);
            table.WidthPercentage = 100;
            widths = new float[] { 55f, 15f, 15f, 15f };
            table.SetWidths(widths);

            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("Place of Supply (State/UT)", font_body)));
            table.AddCell(new PdfPCell(new Phrase("Total Taxable value", font_body)));
            table.AddCell(new PdfPCell(new Phrase("Amount of Integrated Tax", font_body)));


            table.AddCell(new PdfPCell(new Phrase("Supplies made to Unregistered Persons", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(Taxableamount)", "(type='Sale' or type='Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And Sname<>'" + funs.Select_state_nm(Database.CompanyState_id) + "' And RegStatus='Unregistered'").ToString(), font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(IGST)", "(type='Sale' or type='Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And Sname<>'" + funs.Select_state_nm(Database.CompanyState_id) + "' And RegStatus='Unregistered'").ToString(), font_body)));


            table.AddCell(new PdfPCell(new Phrase("Supplies made to Composition Taxable Persons", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(Taxableamount)", "(type='Sale' or type='Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And Sname<>'" + funs.Select_state_nm(Database.CompanyState_id) + "' And RegStatus='Composition Dealer'").ToString(), font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(IGST)", "(type='Sale' or type='Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And Sname<>'" + funs.Select_state_nm(Database.CompanyState_id) + "' And RegStatus='Composition Dealer'").ToString(), font_body)));


            table.AddCell(new PdfPCell(new Phrase("Supplies made to UIN holders", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));


            document.Add(table);

            mater = new Paragraph("4.Eligible ITC" + Environment.NewLine + Environment.NewLine, font_body);
            document.Add(mater);

            table = new PdfPTable(5);
            table.WidthPercentage = 100;
            widths = new float[] { 50, 10f, 10f, 10f, 10f };
            table.SetWidths(widths);

            table.AddCell(new PdfPCell(new Phrase("Details", font_body)));
            table.AddCell(new PdfPCell(new Phrase("Integrated Tax", font_body)));
            table.AddCell(new PdfPCell(new Phrase("Central Tax", font_body)));
            table.AddCell(new PdfPCell(new Phrase("State / UT Tax", font_body)));
            table.AddCell(new PdfPCell(new Phrase("Cess", font_body)));

            table.AddCell(new PdfPCell(new Phrase("(A) ITC Available (whether in full or part)", font_body_bold)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));

            table.AddCell(new PdfPCell(new Phrase("(1)  Import of goods", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));

            table.AddCell(new PdfPCell(new Phrase("(2)  Import of services", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));

            table.AddCell(new PdfPCell(new Phrase("(3)  Inward supplies liable to reverse charge (other than 1 & 2 above)", font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(IGST)", "(type='RCM') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And ITC =" + access_sql.Singlequote + "true" + access_sql.Singlequote).ToString(), font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(CGST)", "(type='RCM') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And ITC =" + access_sql.Singlequote + "true" + access_sql.Singlequote).ToString(), font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(SGST)", "(type='RCM') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And ITC =" + access_sql.Singlequote + "true" + access_sql.Singlequote).ToString(), font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(Cess)", "(type='RCM') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And ITC =" + access_sql.Singlequote + "true" + access_sql.Singlequote).ToString(), font_body)));

            table.AddCell(new PdfPCell(new Phrase("(4)  Inward supplies from ISD", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));

            table.AddCell(new PdfPCell(new Phrase("(5)  All other ITC", font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(IGST)", "(type='Purchase' or type='P Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ")").ToString(), font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(CGST)", "(type='Purchase' or type='P Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ")").ToString(), font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(SGST)", "(type='Purchase' or type='P Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ")").ToString(), font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(Cess)", "(type='Purchase' or type='P Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ")").ToString(), font_body)));

            table.AddCell(new PdfPCell(new Phrase("(B) ITC Reversed", font_body_bold)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));

            table.AddCell(new PdfPCell(new Phrase("(1) As per rules 42 & 43 of CGST Rules", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));

            table.AddCell(new PdfPCell(new Phrase("(2) Others", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));

            table.AddCell(new PdfPCell(new Phrase("(C) Net ITC Available (A) – (B)", font_body_bold)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));

            table.AddCell(new PdfPCell(new Phrase("(D) Ineligible ITC", font_body_bold)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));

            table.AddCell(new PdfPCell(new Phrase("(1) As per section 17(5)", font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(IGST)", "(type='RCM') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And ITC =" + access_sql.Singlequote + "false" + access_sql.Singlequote).ToString(), font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(CGST)", "(type='RCM') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And ITC =" + access_sql.Singlequote + "false" + access_sql.Singlequote).ToString(), font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(SGST)", "(type='RCM') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And ITC =" + access_sql.Singlequote + "false" + access_sql.Singlequote).ToString(), font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(Cess)", "(type='RCM') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And ITC =" + access_sql.Singlequote + "false" + access_sql.Singlequote).ToString(), font_body)));



            //table.AddCell(new PdfPCell(new Phrase("", font_body)));
            //table.AddCell(new PdfPCell(new Phrase("", font_body)));
            //table.AddCell(new PdfPCell(new Phrase("", font_body)));

            table.AddCell(new PdfPCell(new Phrase("(2) Others", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            document.Add(table);

            mater = new Paragraph("5.Values of exempt, nil-rated and non-GST inward " + Environment.NewLine + Environment.NewLine, font_body);
            document.Add(mater);

            table = new PdfPTable(3);
            table.WidthPercentage = 100;
            widths = new float[] { 60, 20f, 20f };
            table.SetWidths(widths);

            table.AddCell(new PdfPCell(new Phrase("Nature of supplies", font_body)));
            table.AddCell(new PdfPCell(new Phrase("Inter-State supplies", font_body)));
            table.AddCell(new PdfPCell(new Phrase("Intra-State supplies", font_body)));

            table.AddCell(new PdfPCell(new Phrase("From a supplier under composition scheme, Exempt and Nil rated supply", font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(Taxableamount)", "(type='Purchase' or type='P Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And (TotTaxPer = 0 or RegStatus='Composition Dealer')   And (Sname<>'" + funs.Select_state_nm(Database.CompanyState_id) + "')  ").ToString(), font_body)));
            table.AddCell(new PdfPCell(new Phrase(dtgstr.Compute("sum(Taxableamount)", "(type='Purchase' or type='P Return') and (Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " and Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And (TotTaxPer = 0 or RegStatus='Composition Dealer')   And Sname='" + funs.Select_state_nm(Database.CompanyState_id) + "'  ").ToString(), font_body)));

            table.AddCell(new PdfPCell(new Phrase("Non GST supply", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));

            document.Add(table);

            mater = new Paragraph("6.1   Payment of tax" + Environment.NewLine + Environment.NewLine, font_body);
            document.Add(mater);

            table = new PdfPTable(10);
            table.WidthPercentage = 100;
            widths = new float[] { 19f, 9f, 9f, 9f, 9f, 9f, 9f, 9f, 9f, 9f };
            table.SetWidths(widths);
            PdfPCell cell;
            cell = new PdfPCell(new Phrase("Description"));
            cell.Rowspan = 2;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Tax payable"));
            cell.Rowspan = 2;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Paid through ITC"));
            cell.Colspan = 4;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Tax paid"));
            cell.Rowspan = 2;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Tax /Cess paid in cash"));
            cell.Rowspan = 2;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Interest"));
            cell.Rowspan = 2;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Late Fee"));
            cell.Rowspan = 2;
            table.AddCell(cell);
            table.AddCell("Integrated Tax");
            table.AddCell("Central Tax");
            table.AddCell("State /UT Tax");
            table.AddCell("Cess");

            table.AddCell("Integrated Tax");
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");

            table.AddCell("Central Tax");
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");

            table.AddCell("State / UT Tax");
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");

            table.AddCell("Cess");
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");

            document.Add(table);

            mater = new Paragraph("6.2 TDS/TCS Credit" + Environment.NewLine + Environment.NewLine, font_body);
            document.Add(mater);

            table = new PdfPTable(4);
            table.WidthPercentage = 100;
            widths = new float[] { 25f, 25f, 25f, 25f };
            table.SetWidths(widths);

            table.AddCell(new PdfPCell(new Phrase("Details", font_body)));
            table.AddCell(new PdfPCell(new Phrase("Integrated Tax", font_body)));
            table.AddCell(new PdfPCell(new Phrase("Central Tax", font_body)));
            table.AddCell(new PdfPCell(new Phrase("State/UT Tax", font_body)));

            table.AddCell(new PdfPCell(new Phrase("TDS", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));

            table.AddCell(new PdfPCell(new Phrase("TCS", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));
            table.AddCell(new PdfPCell(new Phrase("", font_body)));

            document.Add(table);

            mater = new Paragraph("Verification (by Authorised signatory)" + Environment.NewLine, font_body_bold);
            document.Add(mater);
            mater = new Paragraph("I  hereby solemnly affirm  and  declare  that  the  information  given  herein  above  is  true   and correct to the best of my knowledge and belief and nothing has been concealed there from." + Environment.NewLine + Environment.NewLine, font_body);
            document.Add(mater);

            mater = new Paragraph("Instructions:" + Environment.NewLine, font_body_bold);
            document.Add(mater);
            mater = new Paragraph("1) Value of Taxable Supplies = Value of invoices + value of Debit Notes – value of credit notes  + same month value of advances received for which invoices have not been issued in the – value of advances adjusted against invoices" + Environment.NewLine, font_body);
            document.Add(mater);
            mater = new Paragraph("2) Details of advances as well as adjustment of same against invoices to be adjusted and not shown separately" + Environment.NewLine, font_body);
            document.Add(mater);
            mater = new Paragraph("3) Amendment in any details to be adjusted and not shown separately." + Environment.NewLine, font_body);
            document.Add(mater);

            under = writer.DirectContentUnder;
            baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED);
            under.BeginText();
            under.SetColorFill(iTextSharp.text.pdf.CMYKColor.LIGHT_GRAY);
            under.SetFontAndSize(baseFont, fontSize);
            under.ShowTextAligned(PdfContentByte.ALIGN_CENTER, watermarkText, xPosition, yPosition, angle);
            under.EndText();

            document.Close();
            fs.Dispose();
        }

        private void frm_GSTR_3B_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }
    }
}
