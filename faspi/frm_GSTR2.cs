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

namespace faspi
{
    public partial class frm_GSTR2 : Form
    {
        static Object misValue = System.Reflection.Missing.Value;
        static Excel.Application apl = new Microsoft.Office.Interop.Excel.Application();
        Excel.Workbook wb;
        Excel.Worksheet ws;

        DateTime dt1 = new DateTime();
        DateTime dt2 = new DateTime();
        public frm_GSTR2()
        {
            InitializeComponent();
            dateTimePicker1.CustomFormat = Database.dformat;
            dateTimePicker2.CustomFormat = Database.dformat;
            dateTimePicker1.MinDate = Database.stDate;
            dateTimePicker1.MaxDate = Database.ldate;
            dateTimePicker2.MinDate = Database.stDate;
            dateTimePicker2.MaxDate = Database.ldate;
            dateTimePicker1.Value = Database.stDate;
            dateTimePicker2.Value = Database.ldate;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //if (listBox1.Text == "")
            //{
            //    MessageBox.Show("Select Month Name...");
            //    return;
            //}


            //if (listBox1.Text == "July")
            //{
            //    dt1 = new DateTime(Database.stDate.Year, 07, 01);
            //    dt2 = new DateTime(Database.stDate.Year, 07, 31);
            //}
            //else if (listBox1.Text == "August")
            //{
            //    dt1 = new DateTime(Database.stDate.Year, 08, 01);
            //    dt2 = new DateTime(Database.stDate.Year, 08, 31);
            //}
            //else if (listBox1.Text == "September")
            //{
            //    dt1 = new DateTime(Database.stDate.Year, 09, 01);
            //    dt2 = new DateTime(Database.stDate.Year, 09, 30);
            //}
            //else if (listBox1.Text == "October")
            //{
            //    dt1 = new DateTime(Database.stDate.Year, 10, 01);
            //    dt2 = new DateTime(Database.stDate.Year, 10, 31);
            //}
            //else if (listBox1.Text == "November")
            //{
            //    dt1 = new DateTime(Database.stDate.Year, 11, 01);
            //    dt2 = new DateTime(Database.stDate.Year, 11, 30);
            //}
            //else if (listBox1.Text == "December")
            //{
            //    dt1 = new DateTime(Database.stDate.Year, 12, 01);
            //    dt2 = new DateTime(Database.stDate.Year, 12, 31);
            //}

            dt1 = dateTimePicker1.Value;
            dt2 = dateTimePicker2.Value;
            wb = (Excel.Workbook)apl.Workbooks.Open(Application.StartupPath + "\\efile\\GSTR2.xlsx", true, true, misValue, null, null, false, misValue, null, false, false, misValue, misValue, misValue, false);
            Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets["b2b"];

            string sql = "";
            DataTable dtb2b = new DataTable();
            Database.GetSqlData("SELECT VOUCHERINFO.ShiptoTIN as Tin_number, VOUCHERINFO.Svnum AS Invoiceno, VOUCHERINFO.Svdate AS Vdate, VOUCHERINFO.Totalamount AS Totalamount, State.GSTCode " + access_sql.Concat + "'-'" + access_sql.Concat + " State.Sname AS State, Voucherdet.TotTaxPer, Sum(Voucherdet.Taxabelamount) AS Taxabelamount, Sum(Voucherdet.taxamt3) AS IGST, Sum(Voucherdet.taxamt1) AS CGST, Sum(Voucherdet.taxamt2) AS SGST, Sum(Voucherdet.taxamt4) AS CESS, " + access_sql.fnstring("TAXCATEGORY.Item_Type='Goods'", "'Inputs'", "'Input Services'") + " as Eligibility FROM ((((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id2 = ACCOUNT.Ac_id) LEFT JOIN State ON VOUCHERINFO.ShiptoStateid = State.State_id) LEFT JOIN Voucherdet ON VOUCHERINFO.Vi_id = Voucherdet.Vi_id) LEFT JOIN TAXCATEGORY ON Voucherdet.Category_Id = TAXCATEGORY.Category_Id WHERE (((ACCOUNT.RegStatus)='Regular Registration') And ((VOUCHERINFO.vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ")) GROUP BY VOUCHERINFO.ShiptoTIN, VOUCHERINFO.Svnum, VOUCHERINFO.Svdate, Voucherdet.TotTaxPer, VOUCHERINFO.Vnumber, VOUCHERINFO.Totalamount,State.GSTCode, State.Sname, VOUCHERTYPE.Type, VOUCHERTYPE.A, TAXCATEGORY.Item_Type HAVING  (VOUCHERTYPE.Type='Purchase' or VOUCHERTYPE.Type='RCM') AND VOUCHERTYPE.A=" + access_sql.Singlequote + "True" + access_sql.Singlequote + " ORDER BY VOUCHERINFO.Svdate, VOUCHERINFO.ShiptoTIN", dtb2b);


            var data = new object[dtb2b.Rows.Count, 18];
            for (int i = 0; i < dtb2b.Rows.Count; i++)
            {
                data[i, 0] = dtb2b.Rows[i]["Tin_number"].ToString();
                data[i, 1] = dtb2b.Rows[i]["Invoiceno"].ToString();

                data[i, 2] = DateTime.Parse(dtb2b.Rows[i]["Vdate"].ToString()).ToString("dd-MMM-yyyy");
                data[i, 3] = funs.DecimalPoint(double.Parse(dtb2b.Rows[i]["Totalamount"].ToString()), 2);
                data[i, 4] = dtb2b.Rows[i]["State"].ToString();

                //if (bool.Parse(dtb2b.Rows[i]["RCM"].ToString()) == false)
                //{
                    data[i, 5] = "N";
                //}
                //else
                //{
                //    data[i, 5] = "Y";
                //}


                data[i, 6] = "Regular";
               
                data[i, 7] = funs.DecimalPoint(double.Parse(dtb2b.Rows[i]["TotTaxPer"].ToString()), 2);
                data[i, 8] = funs.DecimalPoint(double.Parse(dtb2b.Rows[i]["Taxabelamount"].ToString()), 2);
                data[i, 9] = double.Parse(dtb2b.Rows[i]["IGST"].ToString());
                data[i, 10] = double.Parse(dtb2b.Rows[i]["CGST"].ToString());
                data[i,11] = double.Parse(dtb2b.Rows[i]["SGST"].ToString());
                data[i, 12] = double.Parse(dtb2b.Rows[i]["CESS"].ToString());


                data[i, 13] = dtb2b.Rows[i]["Eligibility"].ToString();
                data[i, 14] = double.Parse(dtb2b.Rows[i]["IGST"].ToString());
                data[i, 15] = double.Parse(dtb2b.Rows[i]["CGST"].ToString());
                data[i, 16] = double.Parse(dtb2b.Rows[i]["SGST"].ToString());
                data[i, 17] = double.Parse(dtb2b.Rows[i]["CESS"].ToString());


            }

            var startcell = (Excel.Range)ws.Cells[5, 1];
            var endcell = (Excel.Range)ws.Cells[dtb2b.Rows.Count + 4, 18];
            var writerange = ws.Range[startcell, endcell];
            writerange.Value = data;


            ws = (Excel.Worksheet)wb.Worksheets["b2bur"];

            sql = "SELECT VOUCHERINFO.Shipto, VOUCHERINFO.Svnum AS Invoiceno, VOUCHERINFO.Svdate AS Vdate, VOUCHERINFO.Totalamount AS Totalamount, State.GSTCode " + access_sql.Concat + "'-'" + access_sql.Concat + "  State.Sname AS State, " + access_sql.fnstring("State.Sname='"+funs.Select_state_nm(Database.CompanyState_id) +"'", "'Intra State'", "'Inter State'") + " AS SupplyType, Voucherdet.TotTaxPer, Sum(Voucherdet.Taxabelamount) AS Taxabelamount, Sum(Voucherdet.taxamt3) AS IGST, Sum(Voucherdet.taxamt1) AS CGST, Sum(Voucherdet.taxamt2) AS SGST, Sum(Voucherdet.taxamt4) AS CESS, " + access_sql.fnstring("TAXCATEGORY.Item_Type='Goods'", "'Inputs'", "'Input services'") + " as Eligibility FROM ((((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id2 = ACCOUNT.Ac_id) LEFT JOIN State ON VOUCHERINFO.ShiptoStateid = State.State_id) LEFT JOIN Voucherdet ON VOUCHERINFO.Vi_id = Voucherdet.Vi_id) LEFT JOIN TAXCATEGORY ON Voucherdet.Category_Id = TAXCATEGORY.Category_Id WHERE (((ACCOUNT.RegStatus)='Unregistered')) AND ((VOUCHERINFO.vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ")  GROUP BY VOUCHERTYPE.Type, VOUCHERINFO.Shipto, VOUCHERINFO.Svnum, VOUCHERINFO.Svdate,VOUCHERINFO.Totalamount, Voucherdet.TotTaxPer, VOUCHERINFO.Vnumber, State.GSTCode, State.Sname, VOUCHERTYPE.Type, VOUCHERTYPE.A, TAXCATEGORY.Item_Type HAVING (((VOUCHERTYPE.Type)='Purchase' or (VOUCHERTYPE.Type)='RCM')  AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) ORDER BY VOUCHERINFO.Svdate, VOUCHERINFO.Vnumber;";
            DataTable dtb2bur = new DataTable();
            Database.GetSqlData(sql, dtb2bur);

            data = new object[dtb2bur.Rows.Count, 17];

            for (int i = 0; i < dtb2bur.Rows.Count; i++)
            {

                data[i, 0] = dtb2bur.Rows[i]["Shipto"].ToString();
                data[i, 1] = dtb2bur.Rows[i]["Invoiceno"].ToString();
                data[i, 2] = DateTime.Parse(dtb2bur.Rows[i]["Vdate"].ToString()).ToString("dd-MMM-yyyy");

                data[i, 3] = funs.DecimalPoint(double.Parse(dtb2bur.Rows[i]["Totalamount"].ToString()), 2);
                data[i, 4] = dtb2bur.Rows[i]["State"].ToString();
                data[i, 5] = dtb2bur.Rows[i]["SupplyType"].ToString();
                data[i, 6] = funs.DecimalPoint(double.Parse(dtb2bur.Rows[i]["TotTaxPer"].ToString()), 2);
                data[i, 7] = funs.DecimalPoint(double.Parse(dtb2bur.Rows[i]["Taxabelamount"].ToString()), 2);
                data[i, 8] = double.Parse(dtb2bur.Rows[i]["IGST"].ToString());
                data[i, 9] = double.Parse(dtb2bur.Rows[i]["CGST"].ToString());
                data[i, 10] = double.Parse(dtb2bur.Rows[i]["SGST"].ToString());
                data[i, 11] = double.Parse(dtb2bur.Rows[i]["CESS"].ToString());
                data[i, 12] = dtb2bur.Rows[i]["Eligibility"].ToString();
                data[i, 13] = double.Parse(dtb2bur.Rows[i]["IGST"].ToString());
                data[i, 14] = double.Parse(dtb2bur.Rows[i]["CGST"].ToString());
                data[i, 15] = double.Parse(dtb2bur.Rows[i]["SGST"].ToString());
                data[i, 16] = double.Parse(dtb2bur.Rows[i]["CESS"].ToString());



            }
            startcell = (Excel.Range)ws.Cells[5, 1];
            endcell = (Excel.Range)ws.Cells[dtb2bur.Rows.Count + 4, 17];
            writerange = ws.Range[startcell, endcell];
            writerange.Value = data;


            //cdnr
            ws = (Excel.Worksheet)wb.Worksheets["cdnr"];
            sql = "SELECT VOUCHERINFO.ShiptoTIN as Tin_number, VOUCHERINFO.Svnum AS Invoiceno, VOUCHERINFO.Svdate AS Vdate, VOUCHERINFO.Totalamount AS Totalamount, " + access_sql.fnstring("State.Sname='" + funs.Select_state_nm(Database.CompanyState_id) + "'", "'Intra State'", "'Inter State'") + " AS SupplyType, Voucherdet.TotTaxPer, Sum(Voucherdet.Taxabelamount) AS Taxabelamount, Sum(Voucherdet.taxamt3) AS IGST, Sum(Voucherdet.taxamt1) AS CGST, Sum(Voucherdet.taxamt2) AS SGST, Sum(Voucherdet.taxamt4) AS CESS, " + access_sql.fnstring("TAXCATEGORY.Item_Type='Goods'", "'Inputs'", "'Input services'") + " AS Eligibility FROM ((((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id2 = ACCOUNT.Ac_id) LEFT JOIN State ON VOUCHERINFO.ShiptoStateid = State.State_id) LEFT JOIN Voucherdet ON VOUCHERINFO.Vi_id = Voucherdet.Vi_id) LEFT JOIN TAXCATEGORY ON Voucherdet.Category_Id = TAXCATEGORY.Category_Id WHERE (((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") AND ((ACCOUNT.RegStatus)='Regular Registration')) GROUP BY VOUCHERINFO.ShiptoTIN, VOUCHERINFO.Svnum, VOUCHERINFO.Svdate, VOUCHERINFO.Totalamount,Voucherdet.TotTaxPer, VOUCHERTYPE.Type, VOUCHERINFO.Vnumber, State.GSTCode, State.Sname, VOUCHERTYPE.A, TAXCATEGORY.Item_Type HAVING (((VOUCHERTYPE.Type)='P Return') AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) ORDER BY VOUCHERINFO.Svdate, VOUCHERINFO.Vnumber";
            DataTable dtcdnr = new DataTable();
            Database.GetSqlData(sql, dtcdnr);
            data = new object[dtcdnr.Rows.Count, 21];

            for (int i = 0; i < dtcdnr.Rows.Count; i++)
            {

                data[i, 0] = dtcdnr.Rows[i]["Tin_number"].ToString();
                data[i, 1] = "";

                data[i, 2] = "";
                data[i, 3] = dtcdnr.Rows[i]["Invoiceno"].ToString();
                data[i, 4] = DateTime.Parse(dtcdnr.Rows[i]["Vdate"].ToString()).ToString("dd-MMM-yyyy");
                data[i, 5] = "N";
                data[i, 6] = "D";
                data[i, 7] = "01-Sales Return";
                data[i, 8] = dtcdnr.Rows[i]["SupplyType"].ToString();
                data[i, 9] = funs.DecimalPoint(double.Parse(dtcdnr.Rows[i]["Totalamount"].ToString()), 2);
                data[i, 10] = funs.DecimalPoint(double.Parse(dtcdnr.Rows[i]["TotTaxPer"].ToString()), 2);
                data[i, 11] = funs.DecimalPoint(double.Parse(dtcdnr.Rows[i]["Taxabelamount"].ToString()), 2);

                data[i, 12] = funs.DecimalPoint(double.Parse(dtcdnr.Rows[i]["IGST"].ToString()), 2);
                data[i, 13] = funs.DecimalPoint(double.Parse(dtcdnr.Rows[i]["CGST"].ToString()), 2);
                data[i, 14] = funs.DecimalPoint(double.Parse(dtcdnr.Rows[i]["SGST"].ToString()), 2);
                data[i, 15] = funs.DecimalPoint(double.Parse(dtcdnr.Rows[i]["CESS"].ToString()), 2);
                data[i, 16] = dtcdnr.Rows[i]["Eligibility"].ToString();

                data[i, 17] = funs.DecimalPoint(double.Parse(dtcdnr.Rows[i]["IGST"].ToString()), 2);
                data[i, 18] = funs.DecimalPoint(double.Parse(dtcdnr.Rows[i]["CGST"].ToString()), 2);
                data[i, 19] = funs.DecimalPoint(double.Parse(dtcdnr.Rows[i]["SGST"].ToString()), 2);
                data[i, 20] = funs.DecimalPoint(double.Parse(dtcdnr.Rows[i]["CESS"].ToString()), 2);
            }


            startcell = (Excel.Range)ws.Cells[5, 1];
            endcell = (Excel.Range)ws.Cells[dtcdnr.Rows.Count + 4, 21];
            writerange = ws.Range[startcell, endcell];
            writerange.Value = data;

            //cdnur
            ws = (Excel.Worksheet)wb.Worksheets["cdnur"];


            sql = "SELECT VOUCHERINFO.Svnum AS Invoiceno, VOUCHERINFO.Svdate AS Vdate, VOUCHERINFO.Totalamount AS Totalamount, " + access_sql.fnstring("State.Sname='" + funs.Select_state_nm(Database.CompanyState_id) + "'", "'Intra State'", "'Inter State'") + " AS SupplyType, Voucherdet.TotTaxPer, Sum(Voucherdet.Taxabelamount) AS Taxabelamount, Sum(Voucherdet.taxamt3) AS IGST, Sum(Voucherdet.taxamt1) AS CGST, Sum(Voucherdet.taxamt2) AS SGST, Sum(Voucherdet.taxamt4) AS CESS, " + access_sql.fnstring("TAXCATEGORY.Item_Type='Goods'", "'Inputs'", "'Input services'") + " AS Eligibility FROM ((((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id2 = ACCOUNT.Ac_id) LEFT JOIN State ON VOUCHERINFO.ShiptoStateid = State.State_id) LEFT JOIN Voucherdet ON VOUCHERINFO.Vi_id = Voucherdet.Vi_id) LEFT JOIN TAXCATEGORY ON Voucherdet.Category_Id = TAXCATEGORY.Category_Id WHERE (((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") AND ((ACCOUNT.RegStatus)='Unregistered')) GROUP BY VOUCHERINFO.Svnum, VOUCHERINFO.Svdate,VOUCHERINFO.Totalamount, Voucherdet.TotTaxPer, VOUCHERTYPE.Type, VOUCHERINFO.Vnumber, State.GSTCode, State.Sname, VOUCHERTYPE.A, TAXCATEGORY.Item_Type HAVING (((VOUCHERTYPE.Type)='P Return') AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) ORDER BY VOUCHERINFO.Svdate, VOUCHERINFO.Vnumber";
            DataTable dtcdnur = new DataTable();
            Database.GetSqlData(sql, dtcdnur);
            data = new object[dtcdnur.Rows.Count, 20];

            for (int i = 0; i < dtcdnur.Rows.Count; i++)
            {

               
                data[i, 0] = "";

                data[i, 1] = "";
                data[i, 2] = dtcdnur.Rows[i]["Invoiceno"].ToString();
                data[i, 3] = DateTime.Parse(dtcdnur.Rows[i]["Vdate"].ToString()).ToString("dd-MMM-yyyy");
                data[i, 4] = "N";
                data[i, 5] = "D";
                data[i, 6] = "01-Sales Return";
                data[i, 7] = dtcdnur.Rows[i]["SupplyType"].ToString();
                data[i, 8] = funs.DecimalPoint(double.Parse(dtcdnur.Rows[i]["Totalamount"].ToString()), 2);
                data[i, 9] = funs.DecimalPoint(double.Parse(dtcdnur.Rows[i]["TotTaxPer"].ToString()), 2);
                data[i, 10] = funs.DecimalPoint(double.Parse(dtcdnur.Rows[i]["Taxabelamount"].ToString()), 2);

                data[i, 11] = funs.DecimalPoint(double.Parse(dtcdnur.Rows[i]["IGST"].ToString()), 2);
                data[i, 12] = funs.DecimalPoint(double.Parse(dtcdnur.Rows[i]["CGST"].ToString()), 2);
                data[i, 13] = funs.DecimalPoint(double.Parse(dtcdnur.Rows[i]["SGST"].ToString()), 2);
                data[i, 14] = funs.DecimalPoint(double.Parse(dtcdnur.Rows[i]["CESS"].ToString()), 2);
                data[i, 15] = dtcdnur.Rows[i]["Eligibility"].ToString();

                data[i, 16] = funs.DecimalPoint(double.Parse(dtcdnur.Rows[i]["IGST"].ToString()), 2);
                data[i, 17] = funs.DecimalPoint(double.Parse(dtcdnur.Rows[i]["CGST"].ToString()), 2);
                data[i, 18] = funs.DecimalPoint(double.Parse(dtcdnur.Rows[i]["SGST"].ToString()), 2);
                data[i, 19] = funs.DecimalPoint(double.Parse(dtcdnur.Rows[i]["CESS"].ToString()), 2);
            }


            startcell = (Excel.Range)ws.Cells[5, 1];
            endcell = (Excel.Range)ws.Cells[dtcdnr.Rows.Count + 4, 20];
            writerange = ws.Range[startcell, endcell];
            writerange.Value = data;

            ws = (Excel.Worksheet)wb.Worksheets["exemp"];

            //Interstate- RegularRegistration

            sql = "SELECT Sum(Voucherdet.Taxabelamount) AS ItemTaxable FROM ((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id2 = ACCOUNT.Ac_id) LEFT JOIN Voucherdet ON VOUCHERINFO.Vi_id = Voucherdet.Vi_id WHERE (((VOUCHERTYPE.Type)='Purchase' or (VOUCHERTYPE.Type)='RCM') AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") AND ((VOUCHERINFO.ShiptoStateid)<>'" + Database.CompanyState_id + "') AND ((Voucherdet.TotTaxPer)=0) AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) GROUP BY ACCOUNT.RegStatus HAVING (((ACCOUNT.RegStatus)='Regular Registration' Or (ACCOUNT.RegStatus)='Unregistered'));";
            DataTable dtexemp = new DataTable();
            Database.GetSqlData(sql, dtexemp);


            data = new object[1, 5];
            if (dtexemp.Rows.Count > 0)
            {
                data[0, 0] = 0;

                data[0, 1] = funs.DecimalPoint(double.Parse(dtexemp.Rows[0]["ItemTaxable"].ToString()), 2);
                data[0, 2] = 0;
               
            }
            else
            {
                data[0, 0] = 0;
                data[0, 1] = 0;
                data[0, 2] = 0;
               
               
            }
            startcell = (Excel.Range)ws.Cells[5, 3];
            endcell = (Excel.Range)ws.Cells[5, 5];
            writerange = ws.Range[startcell, endcell];
            writerange.Value = data;



           // IntrastateComposition
            sql = "SELECT Sum(Voucherdet.Taxabelamount) AS ItemTaxable FROM ((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id2 = ACCOUNT.Ac_id) LEFT JOIN Voucherdet ON VOUCHERINFO.Vi_id = Voucherdet.Vi_id WHERE (((VOUCHERTYPE.Type)='Purchase' or (VOUCHERTYPE.Type)='RCM') AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") AND ((VOUCHERINFO.ShiptoStateid)='" + Database.CompanyState_id + "')  AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) GROUP BY ACCOUNT.RegStatus HAVING ACCOUNT.RegStatus='Composition Dealer' ";
            dtexemp = new DataTable();
            Database.GetSqlData(sql, dtexemp);

            data = new object[1, 2];
            if (dtexemp.Rows.Count > 0)
            {
                data[0, 0] = funs.DecimalPoint(double.Parse(dtexemp.Rows[0]["ItemTaxable"].ToString()), 2);
                data[0, 1] = 0;
                //data[0, 2] = 0;
                //data[0, 3] = 0;
                //data[0, 4] = 0;
            }
            else
            {
                data[0, 0] = 0;
                data[0, 1] = 0;
                //data[0, 2] = 0;
                //data[0, 3] = 0;
                //data[0, 4] = 0;
            }


            startcell = (Excel.Range)ws.Cells[6, 2];
            endcell = (Excel.Range)ws.Cells[6, 3];
            writerange = ws.Range[startcell, endcell];
            writerange.Value = data;



            //Intrastate- RegularRegistration
            sql = "SELECT Sum(Voucherdet.Taxabelamount) AS ItemTaxable FROM ((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id2 = ACCOUNT.Ac_id) LEFT JOIN Voucherdet ON VOUCHERINFO.Vi_id = Voucherdet.Vi_id WHERE (((VOUCHERTYPE.Type)='Purchase' or (VOUCHERTYPE.Type)='RCM') AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") AND ((VOUCHERINFO.ShiptoStateid)='" + Database.CompanyState_id + "') AND ((Voucherdet.TotTaxPer)=0) AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) GROUP BY ACCOUNT.RegStatus HAVING (((ACCOUNT.RegStatus)='Regular Registration' Or (ACCOUNT.RegStatus)='Unregistered'));";
            dtexemp = new DataTable();
            Database.GetSqlData(sql, dtexemp);


            data = new object[1, 2];
            if (dtexemp.Rows.Count > 0)
            {
                data[0, 0] = funs.DecimalPoint(double.Parse(dtexemp.Rows[0]["ItemTaxable"].ToString()), 2);

                data[0, 1] = 0;
               

            }
            else
            {
                data[0, 0] = 0;
                data[0, 1] = 0;
             
            }
            startcell = (Excel.Range)ws.Cells[6, 4];
            endcell = (Excel.Range)ws.Cells[6, 5];
            writerange = ws.Range[startcell, endcell];
            writerange.Value = data;





            //hsn
            ws = (Excel.Worksheet)wb.Worksheets["hsnsum"];
            // sql = "SELECT TAXCATEGORY.Commodity_Code, TAXCATEGORY.Category_Name as Cat_Name, VOUCHERDET.Rate_Unit, Sum(VOUCHERDET.Quantity * VOUCHERDET.Pvalue) AS Quantity, Sum(VOUCHERDET.Taxabelamount+VOUCHERDET.taxamt3+VOUCHERDET.taxamt1+VOUCHERDET.taxamt2) AS Totalamount, Sum(VOUCHERDET.Taxabelamount) AS ItemTaxable, Sum(VOUCHERDET.taxamt3) AS taxamt3, Sum(VOUCHERDET.taxamt1) AS taxamt1, Sum(VOUCHERDET.taxamt2) AS taxamt2, Sum(VOUCHERDET.taxamt4) AS taxamt4 FROM ((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) RIGHT JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN TAXCATEGORY ON VOUCHERDET.Category_Id = TAXCATEGORY.Category_Id WHERE (((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") AND ((VOUCHERTYPE.Type)='Sale')) GROUP BY TAXCATEGORY.Commodity_Code, TAXCATEGORY.Category_Name, VOUCHERDET.Rate_Unit, VOUCHERTYPE.A HAVING (((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) ORDER BY TAXCATEGORY.Commodity_Code";
            sql = "SELECT TAXCATEGORY.Commodity_Code, TAXCATEGORY.Category_Name AS Cat_Name, VOUCHERDET.Rate_Unit, Sum(VOUCHERDET.Quantity*VOUCHERDET.Pvalue) AS Quantity, Sum(VOUCHERDET.Taxabelamount+VOUCHERDET.taxamt3+VOUCHERDET.taxamt1+VOUCHERDET.taxamt2) AS Totalamount, Sum(VOUCHERDET.Taxabelamount) AS ItemTaxable, Sum(VOUCHERDET.taxamt3) AS taxamt3, Sum(VOUCHERDET.taxamt1) AS taxamt1, Sum(VOUCHERDET.taxamt2) AS taxamt2, Sum(VOUCHERDET.taxamt4) AS taxamt4, VOUCHERTYPE.Type FROM ((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) RIGHT JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN TAXCATEGORY ON VOUCHERDET.Category_Id = TAXCATEGORY.Category_Id WHERE (((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ")) GROUP BY TAXCATEGORY.Commodity_Code, TAXCATEGORY.Category_Name, VOUCHERDET.Rate_Unit, VOUCHERTYPE.A, VOUCHERTYPE.Type HAVING (((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ") AND ((VOUCHERTYPE.Type)='Purchase' Or (VOUCHERTYPE.Type)='P Return'   Or (VOUCHERTYPE.Type)='RCM' )) ORDER BY TAXCATEGORY.Commodity_Code";
            DataTable dthsn = new DataTable();
            Database.GetSqlData(sql, dthsn);

            DataTable tdt = new DataTable();
            tdt.Columns.Add("Commodity_Code", typeof(string));
            tdt.Columns.Add("Cat_Name", typeof(string));
            tdt.Columns.Add("Rate_Unit", typeof(string));
            tdt.Columns.Add("Quantity", typeof(decimal));
            tdt.Columns.Add("Totalamount", typeof(decimal));

            tdt.Columns.Add("ItemTaxable", typeof(decimal));
            tdt.Columns.Add("taxamt3", typeof(decimal));
            tdt.Columns.Add("taxamt2", typeof(decimal));
            tdt.Columns.Add("taxamt1", typeof(decimal));
            tdt.Columns.Add("taxamt4", typeof(decimal));





            for (int i = 0; i < dthsn.Rows.Count; i++)
            {


                if (dthsn.Rows[i]["Type"].ToString() == "Return" || dthsn.Rows[i]["Type"].ToString() == "P Return")
                {
                    dthsn.Rows[i]["Quantity"] = double.Parse(dthsn.Rows[i]["Quantity"].ToString()) * -1;
                    dthsn.Rows[i]["Totalamount"] = double.Parse(dthsn.Rows[i]["Totalamount"].ToString()) * -1;
                    dthsn.Rows[i]["ItemTaxable"] = double.Parse(dthsn.Rows[i]["ItemTaxable"].ToString()) * -1;
                    dthsn.Rows[i]["taxamt3"] = double.Parse(dthsn.Rows[i]["taxamt3"].ToString()) * -1;
                    dthsn.Rows[i]["taxamt2"] = double.Parse(dthsn.Rows[i]["taxamt2"].ToString()) * -1;
                    dthsn.Rows[i]["taxamt1"] = double.Parse(dthsn.Rows[i]["taxamt1"].ToString()) * -1;
                    dthsn.Rows[i]["taxamt4"] = double.Parse(dthsn.Rows[i]["taxamt4"].ToString()) * -1;

                }
            }
            dthsn.Columns.Remove("Type");
            dthsn = dthsn.DefaultView.ToTable();


            DataTable tdt1 = new DataTable();
            tdt1 = dthsn.DefaultView.ToTable(true, "Commodity_Code", "Cat_Name", "Rate_Unit");






            for (int i = 0; i < tdt1.Rows.Count; i++)
            {


                tdt.Rows.Add();
                tdt.Rows[i]["Commodity_Code"] = tdt1.Rows[i]["Commodity_Code"].ToString();
                tdt.Rows[i]["Cat_Name"] = tdt1.Rows[i]["Cat_Name"].ToString();
                tdt.Rows[i]["Rate_Unit"] = tdt1.Rows[i]["Rate_Unit"].ToString();
                double qty = 0, taxableamt = 0, totalamount = 0, taxamt1, taxamt2, taxamt3 = 0, taxamt4 = 0;
                qty = double.Parse(dthsn.Compute("sum(Quantity)", "Commodity_Code='" + tdt.Rows[i]["Commodity_Code"].ToString() + "' And Cat_Name='" + tdt.Rows[i]["Cat_Name"].ToString() + "' And Rate_Unit='" + tdt.Rows[i]["Rate_Unit"].ToString() + "'").ToString());
                totalamount = double.Parse(dthsn.Compute("sum(Totalamount)", "Commodity_Code='" + tdt.Rows[i]["Commodity_Code"].ToString() + "' And Cat_Name='" + tdt.Rows[i]["Cat_Name"].ToString() + "' And Rate_Unit='" + tdt.Rows[i]["Rate_Unit"].ToString() + "'").ToString());
                taxableamt = double.Parse(dthsn.Compute("sum(ItemTaxable)", "Commodity_Code='" + tdt.Rows[i]["Commodity_Code"].ToString() + "' And Cat_Name='" + tdt.Rows[i]["Cat_Name"].ToString() + "' And Rate_Unit='" + tdt.Rows[i]["Rate_Unit"].ToString() + "'").ToString());
                taxamt3 = double.Parse(dthsn.Compute("sum(taxamt3)", "Commodity_Code='" + tdt.Rows[i]["Commodity_Code"].ToString() + "' And Cat_Name='" + tdt.Rows[i]["Cat_Name"].ToString() + "' And Rate_Unit='" + tdt.Rows[i]["Rate_Unit"].ToString() + "'").ToString());
                taxamt2 = double.Parse(dthsn.Compute("sum(taxamt2)", "Commodity_Code='" + tdt.Rows[i]["Commodity_Code"].ToString() + "' And Cat_Name='" + tdt.Rows[i]["Cat_Name"].ToString() + "' And Rate_Unit='" + tdt.Rows[i]["Rate_Unit"].ToString() + "'").ToString());
                taxamt1 = double.Parse(dthsn.Compute("sum(taxamt1)", "Commodity_Code='" + tdt.Rows[i]["Commodity_Code"].ToString() + "' And Cat_Name='" + tdt.Rows[i]["Cat_Name"].ToString() + "' And Rate_Unit='" + tdt.Rows[i]["Rate_Unit"].ToString() + "'").ToString());
                taxamt4 = double.Parse(dthsn.Compute("sum(taxamt4)", "Commodity_Code='" + tdt.Rows[i]["Commodity_Code"].ToString() + "' And Cat_Name='" + tdt.Rows[i]["Cat_Name"].ToString() + "' And Rate_Unit='" + tdt.Rows[i]["Rate_Unit"].ToString() + "'").ToString());


                tdt.Rows[i]["Quantity"] = qty;
                tdt.Rows[i]["Totalamount"] = totalamount;
                tdt.Rows[i]["ItemTaxable"] = taxableamt;

                tdt.Rows[i]["taxamt3"] = taxamt3;
                tdt.Rows[i]["taxamt2"] = taxamt2;
                tdt.Rows[i]["taxamt1"] = taxamt1;
                tdt.Rows[i]["taxamt4"] = taxamt4;
            }



















            data = new object[tdt.Rows.Count, 10];





            for (int i = 0; i < tdt.Rows.Count; i++)
            {


                data[i, 0] = tdt.Rows[i]["Commodity_Code"].ToString();
                data[i, 1] = tdt.Rows[i]["Cat_Name"].ToString();

                if (tdt.Rows[i]["Rate_Unit"].ToString() == "LTR-LITRES")
                {
                    tdt.Rows[i]["Rate_Unit"] = "MLT-MILILITRE";
                    data[i, 2] = tdt.Rows[i]["Rate_Unit"].ToString();

                    data[i, 3] = funs.DecimalPoint(double.Parse(tdt.Rows[i]["Quantity"].ToString()) * 1000, 2);
                }
                else
                {
                    data[i, 2] = tdt.Rows[i]["Rate_Unit"].ToString();
                    data[i, 3] = funs.DecimalPoint(double.Parse(tdt.Rows[i]["Quantity"].ToString()), 2);
                }
                data[i, 4] = funs.DecimalPoint(double.Parse(tdt.Rows[i]["Totalamount"].ToString()), 2);
                data[i, 5] = funs.DecimalPoint(double.Parse(tdt.Rows[i]["ItemTaxable"].ToString()), 2);
                data[i, 6] = funs.DecimalPoint(double.Parse(tdt.Rows[i]["taxamt3"].ToString()), 2);
                data[i, 7] = funs.DecimalPoint(double.Parse(tdt.Rows[i]["taxamt1"].ToString()), 2);
                data[i, 8] = funs.DecimalPoint(double.Parse(tdt.Rows[i]["taxamt2"].ToString()), 2);
                data[i, 9] = funs.DecimalPoint(double.Parse(tdt.Rows[i]["taxamt4"].ToString()), 2);

            }


            startcell = (Excel.Range)ws.Cells[5, 1];
            endcell = (Excel.Range)ws.Cells[tdt.Rows.Count + 4, 10];
            writerange = ws.Range[startcell, endcell];
            writerange.Value = data;



            apl.Visible = true;
            GC.Collect();
            MessageBox.Show("Done");
            this.Close();
            this.Dispose();
        }
    }
}
