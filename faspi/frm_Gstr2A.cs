using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using System.Security.Cryptography;
using System.Runtime.Serialization.Json;

using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;


namespace faspi
{
    public partial class frm_Gstr2A : Form
    {
        OpenFileDialog opFile = new OpenFileDialog();
        DateTime dt1 = new DateTime();
        DateTime dt2 = new DateTime();

        public frm_Gstr2A()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //opFile.Title = "Select a Document";

            //if (opFile.ShowDialog() == DialogResult.OK)
            //{

            //    textBox1.Text = opFile.FileName;


            //}

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.AddExtension = true;

            openFileDialog1.Title = "Browse JSON Files";
            openFileDialog1.DefaultExt = "JSON";
            openFileDialog1.Filter = "Json files (*.json)|*.json"; ;
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.Multiselect = false;
            openFileDialog1.ShowDialog();
            textBox1.Text = openFileDialog1.FileName;
            if (textBox1.Text == "") return;
            try
            {
                string text = File.ReadAllText(textBox1.Text, Encoding.UTF8);
                DataContractJsonSerializer SerializerResponse = new DataContractJsonSerializer(typeof(JSONResponse));
                MemoryStream ms = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(text));
                JSONResponse Jsone = (JSONResponse)SerializerResponse.ReadObject(ms);


                DataTable dt = new DataTable();

                dt.Columns.Add("GSTIN", typeof(string));
                dt.Columns.Add("InvoiceNo", typeof(string));
                dt.Columns.Add("Rate", typeof(double));
                dt.Columns.Add("InvoiceDate", typeof(string));
                dt.Columns.Add("InvoiceValue", typeof(double));

              
                dt.Columns.Add("TaxableAmount", typeof(double));

                dt.Columns.Add("IGST", typeof(double));
                dt.Columns.Add("CGST", typeof(double));
                dt.Columns.Add("SGST", typeof(double));
                dt.Columns.Add("Cess", typeof(double));


                for (int i = 0; i < Jsone.b2b.Length; i++)
                {
                    for (int j = 0; j < Jsone.b2b[i].inv.Length; j++)
                    {
                        for (int k = 0; k < Jsone.b2b[i].inv[j].itms.Length; k++)
                        {

                            dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1]["GSTIN"] = Jsone.b2b[i].ctin;

                            dt.Rows[dt.Rows.Count - 1]["InvoiceNo"] = Jsone.b2b[i].inv[j].inum;
                            DateTime dtime = new DateTime(int.Parse(Jsone.b2b[i].inv[j].idt.Split('-')[2]), int.Parse(Jsone.b2b[i].inv[j].idt.Split('-')[1]), int.Parse(Jsone.b2b[i].inv[j].idt.Split('-')[0]));
                            dt.Rows[dt.Rows.Count - 1]["InvoiceDate"] = dtime.ToString("dd-MMM-yyyy");

                            dt.Rows[dt.Rows.Count - 1]["InvoiceValue"] = Jsone.b2b[i].inv[j].val;
                            dt.Rows[dt.Rows.Count - 1]["TaxableAmount"] = Jsone.b2b[i].inv[j].itms[k].itm_det.txval;
                            dt.Rows[dt.Rows.Count - 1]["Rate"] = Jsone.b2b[i].inv[j].itms[k].itm_det.rt;

                            dt.Rows[dt.Rows.Count - 1]["IGST"] = (Jsone.b2b[i].inv[j].itms[k].itm_det.iamt == null) ? 0 : double.Parse(Jsone.b2b[i].inv[j].itms[k].itm_det.iamt);
                            dt.Rows[dt.Rows.Count - 1]["CGST"] = (Jsone.b2b[i].inv[j].itms[k].itm_det.camt == null) ? 0 : double.Parse(Jsone.b2b[i].inv[j].itms[k].itm_det.camt);
                            dt.Rows[dt.Rows.Count - 1]["SGST"] = (Jsone.b2b[i].inv[j].itms[k].itm_det.samt == null) ? 0 : double.Parse(Jsone.b2b[i].inv[j].itms[k].itm_det.samt);
                            dt.Rows[dt.Rows.Count - 1]["Cess"] = (Jsone.b2b[i].inv[j].itms[k].itm_det.csamt == null) ? 0 : double.Parse(Jsone.b2b[i].inv[j].itms[k].itm_det.csamt);

                        }
                    }

                }
                string fp = Jsone.fp;
                string mn = funs.GetFixedLengthString(fp, 2);

                if (mn == "07")
                {
                    dt1 = new DateTime(Database.stDate.Year, 07, 01);
                    dt2 = new DateTime(Database.stDate.Year, 07, 31);
                }
                else if (mn == "08")
                {
                    dt1 = new DateTime(Database.stDate.Year, 08, 01);
                    dt2 = new DateTime(Database.stDate.Year, 08, 31);
                }
                else if (mn == "09")
                {
                    dt1 = new DateTime(Database.stDate.Year, 09, 01);
                    dt2 = new DateTime(Database.stDate.Year, 09, 30);
                }
                else if (mn == "10")
                {
                    dt1 = new DateTime(Database.stDate.Year, 10, 01);
                    dt2 = new DateTime(Database.stDate.Year, 10, 31);
                }
                else if (mn == "11")
                {
                    dt1 = new DateTime(Database.stDate.Year, 11, 01);
                    dt2 = new DateTime(Database.stDate.Year, 11, 30);
                }
                else if (mn == "12")
                {
                    dt1 = new DateTime(Database.stDate.Year, 12, 01);
                    dt2 = new DateTime(Database.stDate.Year, 12, 31);
                }
                DataTable tdt = new DataTable();
                Database.GetSqlData("SELECT VOUCHERINFO.ShiptoTIN AS GSTIN, VOUCHERINFO.Svnum AS InvoiceNO, Voucherdet.TotTaxPer AS Rate, VOUCHERINFO.Svdate as InvoiceDate,VOUCHERINFO.Totalamount as InvoiceValue, Sum(Voucherdet.Taxabelamount) AS Taxable, Sum(Voucherdet.taxamt3) AS IGST, Sum(Voucherdet.taxamt1) AS CGST, Sum(Voucherdet.taxamt2) AS SGST, Sum(Voucherdet.taxamt4) AS CESS FROM ((VOUCHERINFO LEFT JOIN Voucherdet ON VOUCHERINFO.Vi_id = Voucherdet.Vi_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id2 = ACCOUNT.Ac_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERINFO.vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ") AND ((VOUCHERTYPE.Type)='Purchase')) GROUP BY VOUCHERINFO.ShiptoTIN, VOUCHERINFO.Svnum, Voucherdet.TotTaxPer, VOUCHERINFO.Svdate, ACCOUNT.RegStatus ,VOUCHERINFO.Totalamount HAVING (((ACCOUNT.RegStatus)='Regular Registration')) ORDER BY VOUCHERINFO.ShiptoTIN", tdt);

                Microsoft.Office.Interop.Excel.Application ap2;
                Microsoft.Office.Interop.Excel.Workbook wb2;
                Microsoft.Office.Interop.Excel.Worksheet ws2;
                Microsoft.Office.Interop.Excel.Range Range2;

                object misValue2 = System.Reflection.Missing.Value;

                ap2 = new Excel.Application();
                wb2 = (Excel.Workbook)ap2.Workbooks.Add(misValue2);
                ws2 = (Excel.Worksheet)wb2.Worksheets[1];
                ws2.Name = "GSTR-2A JSON";


                ws2.Range[ws2.Cells[1, 1], ws2.Cells[1, 3]].Merge();
                ws2.Range[ws2.Cells[1, 4], ws2.Cells[1, 10]].Merge();
                ws2.Range[ws2.Cells[1, 11], ws2.Cells[1, 17]].Merge();
                ws2.Cells[1, 1] = "Description";

                ws2.Cells[1, 4] = "As Per JSON Format";
                ws2.Cells[1, 11] = "As Per Marwari Software";


                ws2.get_Range(ws2.Cells[1, 1], ws2.Cells[2, 3]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue);
                ws2.get_Range(ws2.Cells[1, 4], ws2.Cells[2, 10]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightSeaGreen);
                ws2.get_Range(ws2.Cells[1, 11], ws2.Cells[2, 17]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightSkyBlue);


                ws2.get_Range(ws2.Cells[1, 1], ws2.Cells[1, 3]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                ws2.get_Range(ws2.Cells[1, 4], ws2.Cells[1, 10]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                ws2.get_Range(ws2.Cells[1, 11], ws2.Cells[1, 17]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                int lno = 2;

                ws2.Cells[lno, 1] = "GSTIN";
                ws2.Cells[lno, 2] = "InvoiceNo";

                ws2.Cells[lno, 3] = "Rate";

                ws2.Cells[lno, 4] = "InvoiceDate";
                ws2.Cells[lno, 5] = "InvoiceValue";

                ws2.Cells[lno, 6] = "TaxableAmount";
                ws2.Cells[lno, 7] = "IGST";
                ws2.Cells[lno, 8] = "CGST";
                ws2.Cells[lno, 9] = "SGST";
                ws2.Cells[lno, 10] = "CESS";




                ws2.Cells[lno, 11] = "InvoiceDate";
                ws2.Cells[lno, 12] = "InvoiceValue";

                ws2.Cells[lno, 13] = "TaxableAmount";
                ws2.Cells[lno, 14] = "IGST";
                ws2.Cells[lno, 15] = "CGST";
                ws2.Cells[lno, 16] = "SGST";
                ws2.Cells[lno, 17] = "CESS";

                ws2.get_Range(ws2.Cells[lno, 1], ws2.Cells[lno, 20]).Font.Bold = true;
                string gstin = "";
                int lnono = 2;

                // Loop Statred

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    // gstin = ;



                    if (lno == 2)
                    {
                        lno = lno + 1;
                    }

                    else if (gstin == dt.Rows[i][0].ToString())
                    {
                        lno = lno + 1;
                    }
                    else
                    {
                        //if only in Excel
                            DataTable dttable = new DataTable();
                            DataRow[] drow;

                            drow = tdt.Select("GSTIN='" + gstin + "'");
                           
                            if (drow.GetLength(0) > 0)
                            {
                                dttable = drow.CopyToDataTable();
                            }
                            for (int k = 0; k < dttable.Rows.Count; k++)
                            {

                                    lno = lno + 1;
                                    ws2.Cells[lno, 1] = dttable.Rows[k][0].ToString();
                                    ws2.Cells[lno, 2] = dttable.Rows[k][1].ToString();
                                    ws2.Cells[lno, 3] = dttable.Rows[k][2].ToString();

                                    DateTime invdate = new DateTime();
                                    invdate = DateTime.Parse(dttable.Rows[k][3].ToString());
                                    double invvalue = 0;
                                    invvalue = double.Parse(dttable.Rows[k][4].ToString());
                                    double taxable = 0;
                                    taxable = double.Parse(dttable.Rows[k][5].ToString());
                                    double cgst = 0;
                                    cgst = double.Parse(dttable.Rows[k][7].ToString());


                                    double sgst = 0;
                                    sgst = double.Parse(dttable.Rows[k][8].ToString());
                                    double igst = 0;
                                    igst = double.Parse(dttable.Rows[k][6].ToString());


                                    double cess = 0;
                                    cess = double.Parse(dttable.Rows[k][9].ToString());
                                    ws2.Cells[lno, 11] = invdate.ToString("dd-MMM-yy");
                                    ws2.Cells[lno, 12] = invvalue;
                                    ws2.Cells[lno, 13] = taxable;
                                    ws2.Cells[lno, 14] = igst;
                                    ws2.Cells[lno, 15] = cgst;
                                    ws2.Cells[lno, 16] = sgst;
                                    ws2.Cells[lno, 17] = cess;
                                    ws2.Cells[lno, 4] = "Invoice Not Found";


                                    ws2.Range[ws2.Cells[lno, 4], ws2.Cells[lno, 10]].Merge();
                                    ws2.get_Range(ws2.Cells[lno, 4], ws2.Cells[lno, 10]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                                    ws2.get_Range(ws2.Cells[lno, 4], ws2.Cells[lno, 10]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);

                                    //tdt.Rows[k].Delete();
                                    //tdt.AcceptChanges();

                                DataView dv = tdt.DefaultView;
                                dv.RowFilter = "GSTIN='" + gstin + "' and InvoiceNo='" + dttable.Rows[k][1].ToString() + "'  and Rate=" + dttable.Rows[k][2].ToString();
                                dv.Delete(0);
                                tdt.AcceptChanges();

                            }
                           

                        //for (int k = 0; k < tdt.Rows.Count; k++)
                        //{
                           


                        //    if(tdt.Rows[k]["GSTIN"].ToString()==gstin)
                        //    {
                        //        lno = lno + 1;

                        //        ws2.Cells[lno, 1] = tdt.Rows[k][0].ToString();
                        //        ws2.Cells[lno, 2] = tdt.Rows[k][1].ToString();
                        //        ws2.Cells[lno, 3] = tdt.Rows[k][2].ToString();

                        //        DateTime invdate = new DateTime();
                        //        invdate = DateTime.Parse(tdt.Rows[k][3].ToString());
                        //        double invvalue = 0;
                        //        invvalue = double.Parse(tdt.Rows[k][4].ToString());
                        //        double taxable = 0;
                        //        taxable = double.Parse(tdt.Rows[k][5].ToString());
                        //        double cgst = 0;
                        //        cgst = double.Parse(tdt.Rows[k][7].ToString());


                        //        double sgst = 0;
                        //        sgst = double.Parse(tdt.Rows[k][8].ToString());
                        //        double igst = 0;
                        //        igst = double.Parse(tdt.Rows[k][6].ToString());


                        //        double cess = 0;
                        //        cess = double.Parse(tdt.Rows[k][9].ToString());
                        //        ws2.Cells[lno, 11] = invdate.ToString("dd-MMM-yy");
                        //        ws2.Cells[lno, 12] = invvalue;
                        //        ws2.Cells[lno, 13] = taxable;
                        //        ws2.Cells[lno, 14] = igst;
                        //        ws2.Cells[lno, 15] = cgst;
                        //        ws2.Cells[lno, 16] = sgst;
                        //        ws2.Cells[lno, 17] = cess;
                        //        ws2.Cells[lno, 4] = "Invoice Not Found";


                        //        ws2.Range[ws2.Cells[lno, 4], ws2.Cells[lno, 10]].Merge();
                        //        ws2.get_Range(ws2.Cells[lno, 4], ws2.Cells[lno, 10]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        //        ws2.get_Range(ws2.Cells[lno, 4], ws2.Cells[lno, 10]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);

                        //        tdt.Rows[k].Delete();
                        //        tdt.AcceptChanges();

                        //    }

                        //}



                        lno = lno + 1;
                        ws2.Cells[lno, 6] = "=SUM(F" + lnono + ":F" + (lno - 1).ToString() + ")";
                        ws2.Cells[lno, 7] = "=SUM(G" + lnono + ":G" + (lno - 1).ToString() + ")";
                        ws2.Cells[lno, 8] = "=SUM(H" + lnono + ":H" + (lno - 1).ToString() + ")";
                        ws2.Cells[lno, 9] = "=SUM(I" + lnono + ":I" + (lno - 1).ToString() + ")";
                        ws2.Cells[lno, 10] = "=SUM(J" + lnono + ":J" + (lno - 1).ToString() + ")";

                        ws2.Cells[lno, 13] = "=SUM(M" + lnono + ":M" + (lno - 1).ToString() + ")";
                        ws2.Cells[lno, 14] = "=SUM(N" + lnono + ":N" + (lno - 1).ToString() + ")";
                        ws2.Cells[lno, 15] = "=SUM(O" + lnono + ":O" + (lno - 1).ToString() + ")";
                        ws2.Cells[lno, 16] = "=SUM(P" + lnono + ":P" + (lno - 1).ToString() + ")";
                        ws2.Cells[lno, 17] = "=SUM(Q" + lnono + ":Q" + (lno - 1).ToString() + ")";

                        ws2.get_Range(ws2.Cells[lno, 1], ws2.Cells[lno, 20]).Font.Bold = true;
                        lno = lno + 1;
                        lnono = lno;

                    }

                    ws2.Cells[lno, 1] = dt.Rows[i][0].ToString();
                    ws2.Cells[lno, 2] = dt.Rows[i][1].ToString();
                    ws2.Cells[lno, 3] = dt.Rows[i][2].ToString();

                    ws2.Cells[lno, 4] = dt.Rows[i][3].ToString();
                    ws2.Cells[lno, 5] = dt.Rows[i][4].ToString();
                    ws2.Cells[lno, 6] = dt.Rows[i][5].ToString();
                    ws2.Cells[lno, 7] = dt.Rows[i][6].ToString();
                    ws2.Cells[lno, 8] = dt.Rows[i][7].ToString();
                    ws2.Cells[lno, 9] = dt.Rows[i][8].ToString();
                    ws2.Cells[lno, 10] = dt.Rows[i][9].ToString();

                    //found in both(Excel n json)
                    if (tdt.Select("GSTIN='" + dt.Rows[i]["GSTIN"].ToString() + "' and InvoiceNo='" + dt.Rows[i]["Invoiceno"].ToString() + "'  and Rate=" + dt.Rows[i]["Rate"].ToString()).Length != 0)
                    {
                        DateTime invdate = new DateTime();
                        invdate = DateTime.Parse(tdt.Select("GSTIN='" + dt.Rows[i]["GSTIN"].ToString() + "' and InvoiceNo='" + dt.Rows[i]["Invoiceno"].ToString() + "'   and Rate=" + dt.Rows[i]["Rate"].ToString()).FirstOrDefault()["InvoiceDate"].ToString());


                        double invvalue = 0;
                        invvalue = double.Parse(tdt.Select("GSTIN='" + dt.Rows[i]["GSTIN"].ToString() + "' and InvoiceNo='" + dt.Rows[i]["Invoiceno"].ToString() + "'   and Rate=" + dt.Rows[i]["Rate"].ToString()).FirstOrDefault()["InvoiceValue"].ToString());
                        double taxable = 0;
                        taxable = double.Parse(tdt.Select("GSTIN='" + dt.Rows[i]["GSTIN"].ToString() + "' and InvoiceNo='" + dt.Rows[i]["Invoiceno"].ToString() + "'   and Rate=" + dt.Rows[i]["Rate"].ToString()).FirstOrDefault()["Taxable"].ToString());
                        double cgst = 0;
                        cgst = double.Parse(tdt.Select("GSTIN='" + dt.Rows[i]["GSTIN"].ToString() + "' and InvoiceNo='" + dt.Rows[i]["Invoiceno"].ToString() + "'   and Rate=" + dt.Rows[i]["Rate"].ToString()).FirstOrDefault()["CGST"].ToString());
                        double sgst = 0;
                        sgst = double.Parse(tdt.Select("GSTIN='" + dt.Rows[i]["GSTIN"].ToString() + "' and InvoiceNo='" + dt.Rows[i]["Invoiceno"].ToString() + "'   and Rate=" + dt.Rows[i]["Rate"].ToString()).FirstOrDefault()["SGST"].ToString());
                        double igst = 0;
                        igst = double.Parse(tdt.Select("GSTIN='" + dt.Rows[i]["GSTIN"].ToString() + "' and InvoiceNo='" + dt.Rows[i]["Invoiceno"].ToString() + "'   and Rate=" + dt.Rows[i]["Rate"].ToString()).FirstOrDefault()["IGST"].ToString());

                        double cess = 0;
                        cess = double.Parse(tdt.Select("GSTIN='" + dt.Rows[i]["GSTIN"].ToString() + "' and InvoiceNo='" + dt.Rows[i]["Invoiceno"].ToString() + "'   and Rate=" + dt.Rows[i]["Rate"].ToString()).FirstOrDefault()["CESS"].ToString());
                        ws2.Cells[lno, 11] = invdate.ToString("dd-MMM-yy");
                        ws2.Cells[lno, 12] = invvalue;


                        ws2.Cells[lno, 13] = taxable;
                        ws2.Cells[lno, 14] = igst;
                        ws2.Cells[lno, 15] = cgst;
                        ws2.Cells[lno, 16] = sgst;
                        ws2.Cells[lno, 17] = cess;

                        //diff
                        if (double.Parse(dt.Rows[i]["TaxableAmount"].ToString()) != taxable)
                        {
                            ws2.get_Range(ws2.Cells[lno, 6], ws2.Cells[lno, 6]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                            ws2.get_Range(ws2.Cells[lno, 13], ws2.Cells[lno, 13]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);

                        }
                        if (double.Parse(dt.Rows[i]["IGST"].ToString()) != igst)
                        {
                            ws2.get_Range(ws2.Cells[lno, 7], ws2.Cells[lno, 7]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                            ws2.get_Range(ws2.Cells[lno, 14], ws2.Cells[lno, 14]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);

                        }
                        if (double.Parse(dt.Rows[i]["CGST"].ToString()) != cgst)
                        {
                            ws2.get_Range(ws2.Cells[lno, 8], ws2.Cells[lno, 8]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                            ws2.get_Range(ws2.Cells[lno, 15], ws2.Cells[lno, 15]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);

                        }
                        if (double.Parse(dt.Rows[i]["SGST"].ToString()) != sgst)
                        {
                            ws2.get_Range(ws2.Cells[lno, 9], ws2.Cells[lno, 9]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                            ws2.get_Range(ws2.Cells[lno, 16], ws2.Cells[lno, 16]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);

                        }
                        if (double.Parse(dt.Rows[i]["CESS"].ToString()) != cess)
                        {
                            ws2.get_Range(ws2.Cells[lno, 10], ws2.Cells[lno, 10]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                            ws2.get_Range(ws2.Cells[lno, 17], ws2.Cells[lno, 17]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);

                        }

                        if (DateTime.Parse(dt.Rows[i][3].ToString()) != invdate)
                        {
                            ws2.get_Range(ws2.Cells[lno, 4], ws2.Cells[lno, 4]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                            ws2.get_Range(ws2.Cells[lno, 11], ws2.Cells[lno, 11]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);

                        }
                        if (double.Parse(dt.Rows[i][4].ToString()) != invvalue)
                        {
                            ws2.get_Range(ws2.Cells[lno, 5], ws2.Cells[lno, 5]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                            ws2.get_Range(ws2.Cells[lno, 12], ws2.Cells[lno, 12]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);

                        }

                        DataView dv = tdt.DefaultView;
                        dv.RowFilter = "GSTIN='" + dt.Rows[i]["GSTIN"].ToString() + "' and InvoiceNo='" + dt.Rows[i]["Invoiceno"].ToString() + "'  and Rate=" + dt.Rows[i]["Rate"].ToString();
                        dv.Delete(0);
                        tdt.AcceptChanges();

                    }
                    //found only in json
                    else if (tdt.Select("GSTIN='" + dt.Rows[i]["GSTIN"].ToString() + "' ").Length == 0)
                    {
                        ws2.Cells[lno, 11] = "Invoice Not Found";
                        ws2.Range[ws2.Cells[lno, 11], ws2.Cells[lno, 17]].Merge();
                        ws2.get_Range(ws2.Cells[lno, 11], ws2.Cells[lno, 17]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        ws2.get_Range(ws2.Cells[lno, 11], ws2.Cells[lno, 17]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);

                
                            lno = lno + 1;
                            ws2.Cells[lno, 1] = dt.Rows[i][0].ToString();
                            ws2.Cells[lno, 2] = dt.Rows[i][1].ToString();
                            ws2.Cells[lno, 3] = dt.Rows[i][2].ToString();

                            DateTime invdate = new DateTime();
                            invdate = DateTime.Parse(dt.Rows[i][3].ToString());
                            double invvalue = 0;
                            invvalue = double.Parse(dt.Rows[i][4].ToString());
                            double taxable = 0;
                            taxable = double.Parse(dt.Rows[i][5].ToString());
                            double cgst = 0;
                            cgst = double.Parse(dt.Rows[i][7].ToString());


                            double sgst = 0;
                            sgst = double.Parse(dt.Rows[i][8].ToString());
                            double igst = 0;
                            igst = double.Parse(dt.Rows[i][6].ToString());


                            double cess = 0;
                            cess = double.Parse(dt.Rows[i][9].ToString());
                            ws2.Cells[lno, 11] = invdate.ToString("dd-MMM-yy");
                            ws2.Cells[lno, 12] = invvalue;
                            ws2.Cells[lno, 13] = taxable;
                            ws2.Cells[lno, 14] = igst;
                            ws2.Cells[lno, 15] = cgst;
                            ws2.Cells[lno, 16] = sgst;
                            ws2.Cells[lno, 17] = cess;
                            ws2.Cells[lno, 4] = "Invoice Not Found";
                            ws2.Range[ws2.Cells[lno, 4], ws2.Cells[lno, 10]].Merge();
                            ws2.get_Range(ws2.Cells[lno, 4], ws2.Cells[lno, 10]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                            ws2.get_Range(ws2.Cells[lno, 4], ws2.Cells[lno, 10]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        

                      
                    }
                  

                    

                    gstin = dt.Rows[i][0].ToString();
                }
                lno = lno + 1;
                ws2.Cells[lno, 6] = "=SUM(F" + lnono + ":F" + (lno - 1).ToString() + ")";
                ws2.Cells[lno, 7] = "=SUM(G" + lnono + ":G" + (lno - 1).ToString() + ")";
                ws2.Cells[lno, 8] = "=SUM(H" + lnono + ":H" + (lno - 1).ToString() + ")";
                ws2.Cells[lno, 9] = "=SUM(I" + lnono + ":I" + (lno - 1).ToString() + ")";
                ws2.Cells[lno, 10] = "=SUM(J" + lnono + ":J" + (lno - 1).ToString() + ")";


                ws2.Cells[lno, 13] = "=SUM(M" + lnono + ":M" + (lno - 1).ToString() + ")";
                ws2.Cells[lno, 14] = "=SUM(N" + lnono + ":N" + (lno - 1).ToString() + ")";
                ws2.Cells[lno, 15] = "=SUM(O" + lnono + ":O" + (lno - 1).ToString() + ")";
                ws2.Cells[lno, 16] = "=SUM(P" + lnono + ":P" + (lno - 1).ToString() + ")";
                ws2.Cells[lno, 17] = "=SUM(Q" + lnono + ":Q" + (lno - 1).ToString() + ")";
                ws2.get_Range(ws2.Cells[lno, 1], ws2.Cells[lno, 20]).Font.Bold = true;



                //even  GSTIN no is found in JSON

                for (int t = 0; t < tdt.Rows.Count; t++)
                {


                    lno = lno + 1;
                    ws2.Cells[lno, 1] = tdt.Rows[t][0].ToString();
                    ws2.Cells[lno, 2] = tdt.Rows[t][1].ToString();
                    ws2.Cells[lno, 3] = tdt.Rows[t][2].ToString();
                   
                    DateTime invdate = new DateTime();
                    invdate = DateTime.Parse(tdt.Rows[t][3].ToString());
                    double invvalue = 0;
                    invvalue = double.Parse(tdt.Rows[t][4].ToString());
                    double taxable = 0;
                    taxable = double.Parse(tdt.Rows[t][5].ToString());
                    double cgst = 0;
                    cgst = double.Parse(tdt.Rows[t][7].ToString());

                    double sgst = 0;
                    sgst = double.Parse(tdt.Rows[t][8].ToString());
                    double igst = 0;
                    igst = double.Parse(tdt.Rows[t][6].ToString());

                    double cess = 0;
                    cess = double.Parse(tdt.Rows[t][9].ToString());
                    ws2.Cells[lno, 4] = "Invoice Not Found";


                    ws2.Range[ws2.Cells[lno, 4], ws2.Cells[lno, 10]].Merge();
                    ws2.get_Range(ws2.Cells[lno, 4], ws2.Cells[lno, 10]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    ws2.get_Range(ws2.Cells[lno, 4], ws2.Cells[lno, 10]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);


                    ws2.Cells[lno, 11] = invdate.ToString("dd-MMM-yy");
                    ws2.Cells[lno, 12] = invvalue;

                    ws2.Cells[lno, 13] = taxable;
                    ws2.Cells[lno, 14] = igst;
                    ws2.Cells[lno, 15] = cgst;
                    ws2.Cells[lno, 16] = sgst;
                    ws2.Cells[lno, 17] = cess;
               

                }




                ap2.Visible = true;






            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Select JSON file..");
                textBox1.Focus();
                return;
            }
            string path = textBox1.Text;

            string text = File.ReadAllText(path, Encoding.UTF8);


          
            DataContractJsonSerializer SerializerResponse = new DataContractJsonSerializer(typeof(JSONResponse));
            MemoryStream ms = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(text));
            JSONResponse Jsone = (JSONResponse)SerializerResponse.ReadObject(ms);

            DataTable dt = new DataTable();
            DataTable dtdata = new DataTable();
            dt.Columns.Add("Sno", typeof(string));
            dt.Columns.Add("GSTIN", typeof(string));
            dt.Columns.Add("BillNo", typeof(string));
            dt.Columns.Add("BillDate", typeof(DateTime));
            dt.Columns.Add("TaxableAmount", typeof(string));
            dt.Columns.Add("CGST", typeof(string));
            dt.Columns.Add("SGST", typeof(string));
            dt.Columns.Add("IGST", typeof(string));

            string fp = Jsone.fp;
            string mn = funs.GetFixedLengthString(fp, 2);

            if (mn == "07")
            {
                dt1 = new DateTime(Database.stDate.Year, 07, 01);
                dt2 = new DateTime(Database.stDate.Year, 07, 31);
            }
            else if (mn == "08")
            {
                dt1 = new DateTime(Database.stDate.Year, 08, 01);
                dt2 = new DateTime(Database.stDate.Year, 08, 31);
            }
            else if (mn == "09")
            {
                dt1 = new DateTime(Database.stDate.Year, 09, 01);
                dt2 = new DateTime(Database.stDate.Year, 09, 30);
            }
            else if (mn == "10")
            {
                dt1 = new DateTime(Database.stDate.Year, 10, 01);
                dt2 = new DateTime(Database.stDate.Year, 10, 31);
            }
            else if (mn == "11")
            {
                dt1 = new DateTime(Database.stDate.Year, 11, 01);
                dt2 = new DateTime(Database.stDate.Year, 11, 30);
            }
            else if (mn == "12")
            {
                dt1 = new DateTime(Database.stDate.Year, 12, 01);
                dt2 = new DateTime(Database.stDate.Year, 12, 31);
            }


          //  string year = funs.GetFixedLengthString(fp.Length-2,6 );

         //GetFixedLengthStringdt1.Rows[i]["Description"].ToString(), 35
            for (int i = 0; i < Jsone.b2b.Length; i++)
            {
                for (int j = 0; j < Jsone.b2b[i].inv.Length; j++)
                {
                    for (int k = 0; k < Jsone.b2b[i].inv[j].itms.Length; k++)
                    {
                        dt.Rows.Add();
                        dt.Rows[dt.Rows.Count - 1]["SNo"] = dt.Rows.Count;
                        dt.Rows[dt.Rows.Count - 1]["GSTIN"] = Jsone.b2b[i].ctin;
                        dt.Rows[dt.Rows.Count - 1]["BillNo"] = Jsone.b2b[i].inv[j].inum;
                        //string vdate;
                        //vdate =Jsone.b2b[i].inv[j].idt.ToString();

                        DateTime newdate = new DateTime();
                        String[] vdate = Jsone.b2b[i].inv[j].idt.Split('-');

                       


                        newdate = new DateTime(int.Parse(vdate[2]), int.Parse(vdate[1]), int.Parse(vdate[0]));


                        
                        
                        dt.Rows[dt.Rows.Count - 1]["BillDate"] =  newdate;
                        dt.Rows[dt.Rows.Count - 1]["TaxableAmount"] = Jsone.b2b[i].inv[j].itms[k].itm_det.txval;
                        dt.Rows[dt.Rows.Count - 1]["CGST"] = Jsone.b2b[i].inv[j].itms[k].itm_det.camt;
                        dt.Rows[dt.Rows.Count - 1]["SGST"] = Jsone.b2b[i].inv[j].itms[k].itm_det.samt;
                        dt.Rows[dt.Rows.Count - 1]["IGST"] = Jsone.b2b[i].inv[j].itms[k].itm_det.iamt;

                    }
                }

            }

            if (dt.Rows.Count == 0)
            {
                return;
            }
           
            Object misValue = System.Reflection.Missing.Value;
            Excel.Application apl = new Microsoft.Office.Interop.Excel.Application();
            Excel.Workbook wb = (Excel.Workbook)apl.Workbooks.Add(misValue);
            Excel.Worksheet ws;
            ws = (Excel.Worksheet)wb.Worksheets[1];
            ws.Name = "GSTR-2A";
            int lno = 1;
            //Excel.Range usedRange = ws.UsedRange;

            //Excel.Range rows = usedRange.Rows;
          
           // Excel.Range last = ws.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            string StrCon = "";

            StrCon = "";
            //columnheader
            int coln = 1;
            ws.get_Range(ws.Cells[1, 1], ws.Cells[1, dt.Columns.Count]).Font.Bold = true;
            ws.get_Range(ws.Cells[2, 4], ws.Cells[dt.Rows.Count + 1, 4]).NumberFormat = "DD-MMM-YYYY";
            ws.get_Range(ws.Cells[2, 5], ws.Cells[dt.Rows.Count + 1, 8]).NumberFormat = "#00.00";
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                StrCon += dt.Columns[j].ColumnName.Replace('_', '.') + "\t";

                coln++;
            }
            
            //rowsdata
          //  lno=2;
           
          //  Database.GetSqlData("SELECT VOUCHERINFO.ShiptoTIN AS GSTIN, VOUCHERINFO.Svnum AS Invoiceno, VOUCHERINFO.Svdate as Svdate, SUM( Voucherdet.Taxabelamount)  AS Taxableamount, SUM( Voucherdet.taxamt1) AS CGST, SUM( Voucherdet.taxamt2) AS SGST, SUM( Voucherdet.taxamt3) AS IGST FROM VOUCHERINFO LEFT OUTER JOIN Voucherdet ON VOUCHERINFO.Vi_id = Voucherdet.Vi_id LEFT OUTER JOIN ACCOUNT ON VOUCHERINFO.Ac_id2 = ACCOUNT.Ac_id LEFT OUTER JOIN  VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE ( VOUCHERTYPE.A = " + access_sql.Singlequote + "True" + access_sql.Singlequote + ") AND ( VOUCHERTYPE.Type = 'Purchase') AND ( VOUCHERINFO.Vdate >= " + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " AND  VOUCHERINFO.Vdate <= " + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") GROUP BY VOUCHERINFO.ShiptoTIN, VOUCHERINFO.Svnum, VOUCHERINFO.Svdate, ACCOUNT.RegStatus HAVING ( ACCOUNT.RegStatus = 'Regular Registration') or ( ACCOUNT.RegStatus = 'Composition Dealer')", dtdata);
            Database.GetSqlData("SELECT VOUCHERINFO.ShiptoTIN AS GSTIN, VOUCHERINFO.Svnum AS Invoiceno, VOUCHERINFO.Svdate,Sum(Voucherdet.Taxabelamount) AS Taxableamount, Sum(Voucherdet.taxamt1) AS CGST, Sum(Voucherdet.taxamt2) AS SGST, Sum(Voucherdet.taxamt3) AS IGST FROM ((VOUCHERINFO LEFT JOIN Voucherdet ON VOUCHERINFO.Vi_id = Voucherdet.Vi_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id2 = ACCOUNT.Ac_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERINFO.vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ") AND ((VOUCHERTYPE.Type)='Purchase')) GROUP BY VOUCHERINFO.ShiptoTIN, VOUCHERINFO.Svnum, ACCOUNT.RegStatus, VOUCHERINFO.Svdate,Voucherdet.TotTaxPer HAVING (((ACCOUNT.RegStatus)='Regular Registration'))", dtdata);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Excel.Range usedRange = ws.UsedRange;

                Excel.Range rows = usedRange.Rows;

                lno++;
              

                ws.Cells[lno, 1] = dt.Rows[i][0].ToString();
                ws.Cells[lno, 2] = dt.Rows[i][1].ToString();
                ws.Cells[lno, 3] = dt.Rows[i][2].ToString();
                ws.Cells[lno, 4] = dt.Rows[i][3].ToString();
                ws.Cells[lno, 5] = dt.Rows[i][4].ToString();
                ws.Cells[lno, 6] = dt.Rows[i][5].ToString();
                ws.Cells[lno, 7] = dt.Rows[i][6].ToString();
                ws.Cells[lno, 8] = dt.Rows[i][7].ToString();


                if (dtdata.Select("GSTIN='" + dt.Rows[i][1].ToString() + "' and InvoiceNo='" + dt.Rows[i][2].ToString() + "'").Length == 0)
                {
                  
                   //  ws.Cells[lno,1] = System.Drawing.Color.Red;
                    ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, 8]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                   // rows.Interior.Color = System.Drawing.Color.Red;

                }
                else if (dtdata.Select("GSTIN='" + dt.Rows[i][1].ToString() + "' and InvoiceNo='" + dt.Rows[i][2].ToString() + "'").Length > 0)
                {
                    double taxable = 0;
                    taxable = double.Parse(dtdata.Select("GSTIN='" + dt.Rows[i][1].ToString() + "' and InvoiceNo='" + dt.Rows[i][2].ToString() + "'").FirstOrDefault()["Taxableamount"].ToString());
                  double cgst = 0;
                  cgst = double.Parse(dtdata.Select("GSTIN='" + dt.Rows[i][1].ToString() + "' and InvoiceNo='" + dt.Rows[i][2].ToString() + "'").FirstOrDefault()["CGST"].ToString());


                  double sgst = double.Parse(dtdata.Select("GSTIN='" + dt.Rows[i][1].ToString() + "' and InvoiceNo='" + dt.Rows[i][2].ToString() + "'").FirstOrDefault()["SGST"].ToString());
                  double igst = double.Parse(dtdata.Select("GSTIN='" + dt.Rows[i][1].ToString() + "' and InvoiceNo='" + dt.Rows[i][2].ToString() + "'").FirstOrDefault()["IGST"].ToString());
                    if (taxable != double.Parse(dt.Rows[i]["Taxableamount"].ToString()))
                    {
                        ws.get_Range(ws.Cells[lno, 5], ws.Cells[lno, 5]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                    }
                    if (dt.Rows[i]["CGST"].ToString() == "")
                    {
                        dt.Rows[i]["CGST"] = 0;
                    }

                    if (dt.Rows[i]["SGST"].ToString() == "")
                    {
                        dt.Rows[i]["SGST"] = 0;
                    }
                    if (dt.Rows[i]["IGST"].ToString() == "")
                    {
                        dt.Rows[i]["IGST"] = 0;
                    }

                    if (cgst != double.Parse(dt.Rows[i]["CGST"].ToString()))
                    {
                        ws.get_Range(ws.Cells[lno, 6], ws.Cells[lno, 6]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                    }
                    if (sgst != double.Parse(dt.Rows[i]["SGST"].ToString()))
                    {
                        ws.get_Range(ws.Cells[lno, 7], ws.Cells[lno, 7]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                    }
                    if (igst != double.Parse(dt.Rows[i]["IGST"].ToString()))
                    {
                        ws.get_Range(ws.Cells[lno, 8], ws.Cells[lno, 8]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                    }


                }

            }

            Clipboard.SetText(StrCon);

            ws.Paste(misValue, misValue);
            Clipboard.Clear();
            ws.Columns.AutoFit();
            ws.Cells.Locked = true;
            ws.UsedRange.Cells.Borders.Color = System.Drawing.Color.Black.ToArgb();




            //worksheet2
            ws = (Excel.Worksheet)wb.Worksheets[2];
            lno = 1;
            ws.Name = "Marwari-Data";

            StrCon = "Sno"+"\t";
            //columnheader
            coln = 1;
            ws.get_Range(ws.Cells[1, 1], ws.Cells[1, dtdata.Columns.Count]).Font.Bold = true;
            ws.get_Range(ws.Cells[2, 4], ws.Cells[dtdata.Rows.Count + 1, 4]).NumberFormat = "DD-MMM-YYYY";
            ws.get_Range(ws.Cells[2, 5], ws.Cells[dtdata.Rows.Count + 1, 8]).NumberFormat = "#00.00";
            for (int j = 0; j < dtdata.Columns.Count; j++)
            {
                StrCon += dtdata.Columns[j].ColumnName.Replace('_', '.') + "\t";

                coln++;
            }

            for (int k = 0; k < dtdata.Rows.Count; k++)
            {

                if (dt.Select("Billno='" + dtdata.Rows[k]["Invoiceno"].ToString() + "' and GSTIN='" + dtdata.Rows[k]["GSTIN"].ToString() + "'").Length == 0)
                    {
                        lno++;
                        ws.Cells[lno, 1] = lno-1;
                        ws.Cells[lno, 2] = dtdata.Rows[k][0].ToString();
                        ws.Cells[lno, 3] = dtdata.Rows[k][1].ToString();
                        ws.Cells[lno, 4] = dtdata.Rows[k][2].ToString();
                        ws.Cells[lno, 5] = dtdata.Rows[k][3].ToString();
                        ws.Cells[lno, 6] = dtdata.Rows[k][4].ToString();
                        ws.Cells[lno, 7] = dtdata.Rows[k][5].ToString();
                        ws.Cells[lno, 8] = dtdata.Rows[k][6].ToString();
                    }

                
            }

            Clipboard.SetText(StrCon);

            ws.Paste(misValue, misValue);
            Clipboard.Clear();
            ws.Columns.AutoFit();

            ws.Cells.Locked = true;
           
         
            apl.Visible = true;
            GC.Collect();

            this.Close();
            this.Dispose();
        }

        private void frm_Gstr2A_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Escape)
            {
                if (textBox1.Text != "")
                {
                    DialogResult chk = MessageBox.Show("Are u sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (chk == DialogResult.No)
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        this.Dispose();
                        this.Close();
                    }
                }
                else
                {
                    this.Dispose();
                    this.Close();
                }

            }
        }
    }
}
