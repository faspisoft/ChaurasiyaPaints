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
    public partial class frm_GSTR1 : Form
    {
        static Object misValue = System.Reflection.Missing.Value;
        static Excel.Application apl = new Microsoft.Office.Interop.Excel.Application();
        Excel.Workbook wb;
        Excel.Worksheet ws;
        
        DateTime dt1 = new DateTime();
        DateTime dt2 = new DateTime();
        public frm_GSTR1()
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
            wb = (Excel.Workbook)apl.Workbooks.Open(Application.StartupPath + "\\efile\\GSTR1.xlsx", true, true, misValue, null, null, false, misValue, null, false, false, misValue, misValue, misValue, false);
            Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets["b2b"];

            string sql = "";
            DataTable dtb2b = new DataTable();
           // Database.GetSqlData("SELECT ACCOUNT.Tin_number, VOUCHERINFO.Invoiceno, VOUCHERINFO.Vdate, VOUCHERINFO.Totalamount, State.GSTCode " + access_sql.Concat + "'-'" + access_sql.Concat + " State.Sname as sname, VOUCHERINFO.RCM, VOUCHERDET.TotTaxPer, Sum(VOUCHERDET.Taxabelamount) AS ItemTaxable,VOUCHERDET.taxamt4 FROM ((((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id= ACCOUNT.Ac_id) RIGHT JOIN (VOUCHERDET LEFT JOIN DESCRIPTION ON VOUCHERDET.Des_ac_id = DESCRIPTION.Des_id) ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN TAXCATEGORY ON VOUCHERDET.Category_Id = TAXCATEGORY.Category_Id) LEFT JOIN State ON ACCOUNT.State_id = State.State_id WHERE VOUCHERTYPE.Type='Sale' AND (ACCOUNT.RegStatus='Regular Registration' or ACCOUNT.RegStatus='Composition Dealer')  GROUP BY ACCOUNT.Tin_number, VOUCHERINFO.Invoiceno, VOUCHERINFO.Vdate, VOUCHERINFO.Totalamount, State.GSTCode, State.Sname, VOUCHERINFO.RCM, VOUCHERDET.TotTaxPer,VOUCHERDET.taxamt4, VOUCHERINFO.Vnumber, VOUCHERTYPE.A HAVING (((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And ((VOUCHERDET.TotTaxPer)<>0)    AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) ORDER BY VOUCHERINFO.Vdate, VOUCHERINFO.Vnumber", dtb2b);
            Database.GetSqlData("SELECT ACCOUNT.Tin_number, VOUCHERINFO.Invoiceno, VOUCHERINFO.Vdate, VOUCHERINFO.Totalamount, State.GSTCode " + access_sql.Concat + "'-'" + access_sql.Concat + " State.Sname as sname, VOUCHERINFO.RCM, VOUCHERDET.TotTaxPer, Sum(VOUCHERDET.Taxabelamount) AS ItemTaxable,VOUCHERDET.taxamt4 FROM ((((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id2 = ACCOUNT.Ac_id) RIGHT JOIN (VOUCHERDET LEFT JOIN DESCRIPTION ON VOUCHERDET.Des_ac_id = DESCRIPTION.Des_id) ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN TAXCATEGORY ON VOUCHERDET.Category_Id = TAXCATEGORY.Category_Id) LEFT JOIN State ON VOUCHERINFO.ShiptoStateid = State.State_id WHERE VOUCHERTYPE.Type='Sale' AND (ACCOUNT.RegStatus='Regular Registration' or ACCOUNT.RegStatus='Composition Dealer')  GROUP BY ACCOUNT.Tin_number, VOUCHERINFO.Invoiceno, VOUCHERINFO.Vdate, VOUCHERINFO.Totalamount, State.GSTCode, State.Sname, VOUCHERINFO.RCM, VOUCHERDET.TotTaxPer,VOUCHERDET.taxamt4, VOUCHERINFO.Vnumber, VOUCHERTYPE.A HAVING (((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") And ((VOUCHERDET.TotTaxPer)<>0)    AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) ORDER BY VOUCHERINFO.Vdate, VOUCHERINFO.Vnumber", dtb2b);

            var data = new object[dtb2b.Rows.Count, 11];

            for (int i = 0; i < dtb2b.Rows.Count; i++)
            {
                data[i, 0] = dtb2b.Rows[i]["Tin_number"].ToString();
                data[i, 1] = dtb2b.Rows[i]["Invoiceno"].ToString();

                data[i, 2] = DateTime.Parse(dtb2b.Rows[i]["Vdate"].ToString()).ToString("dd-MMM-yyyy");
                data[i, 3] = funs.DecimalPoint(double.Parse(dtb2b.Rows[i]["Totalamount"].ToString()), 2);
                data[i, 4] = dtb2b.Rows[i]["sname"].ToString();
                if (bool.Parse(dtb2b.Rows[i]["RCM"].ToString()) == false)
                {
                    data[i, 5] = "N";
                }
                else
                {
                    data[i, 5] = "Y";
                }


                data[i, 6] = "Regular";
                data[i, 7] = "";

                data[i, 8] = funs.DecimalPoint(double.Parse(dtb2b.Rows[i]["TotTaxPer"].ToString()), 2);
                data[i, 9] = funs.DecimalPoint(double.Parse(dtb2b.Rows[i]["ItemTaxable"].ToString()), 2);
                if (double.Parse(dtb2b.Rows[i]["taxamt4"].ToString()) == 0)
                {
                    data[i, 10] = "";
                }
                else
                {
                    data[i, 10] = double.Parse(dtb2b.Rows[i]["taxamt4"].ToString());
                }

            }

            var startcell = (Excel.Range)ws.Cells[5, 1];
            var endcell = (Excel.Range)ws.Cells[dtb2b.Rows.Count + 4, 11];
            var writerange = ws.Range[startcell, endcell];
            writerange.Value = data;


            ws = (Excel.Worksheet)wb.Worksheets["b2cl"];
           // sql = "SELECT VOUCHERINFO.Invoiceno, VOUCHERINFO.Vdate, VOUCHERINFO.Totalamount, State.GSTCode " + access_sql.Concat + "'-'" + access_sql.Concat + " State.Sname as sname, VOUCHERDET.TotTaxPer, Sum(VOUCHERDET.Taxabelamount) AS ItemTaxable, VOUCHERDET.taxamt4 FROM ((((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id) RIGHT JOIN (VOUCHERDET LEFT JOIN DESCRIPTION ON VOUCHERDET.Des_ac_id = DESCRIPTION.Des_id) ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN TAXCATEGORY ON VOUCHERDET.Category_Id = TAXCATEGORY.Category_Id) LEFT JOIN State ON ACCOUNT.State_id = State.State_id WHERE (((VOUCHERTYPE.Type)='Sale') AND ((ACCOUNT.RegStatus)='Unregistered')) GROUP BY VOUCHERINFO.Invoiceno, VOUCHERINFO.Vdate, VOUCHERINFO.Totalamount, State.GSTCode, State.Sname, VOUCHERDET.TotTaxPer, VOUCHERDET.taxamt4, VOUCHERINFO.Vnumber, VOUCHERTYPE.A HAVING (((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") AND ((VOUCHERINFO.Totalamount)>250000) AND ((State.Sname)<>'" + funs.Select_state_nm(Database.CompanyState_id) + "') AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) ORDER BY VOUCHERINFO.Vdate, VOUCHERINFO.Vnumber";
            sql = "SELECT VOUCHERINFO.Invoiceno, VOUCHERINFO.Vdate, VOUCHERINFO.Totalamount, State.GSTCode " + access_sql.Concat + "'-'" + access_sql.Concat + " State.Sname as sname, VOUCHERDET.TotTaxPer, Sum(VOUCHERDET.Taxabelamount) AS ItemTaxable, VOUCHERDET.taxamt4 FROM ((((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id2 = ACCOUNT.Ac_id) RIGHT JOIN (VOUCHERDET LEFT JOIN DESCRIPTION ON VOUCHERDET.Des_ac_id = DESCRIPTION.Des_id) ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN TAXCATEGORY ON VOUCHERDET.Category_Id = TAXCATEGORY.Category_Id) LEFT JOIN State ON VOUCHERINFO.ShiptoStateid = State.State_id WHERE (((VOUCHERTYPE.Type)='Sale') AND ((ACCOUNT.RegStatus)='Unregistered')) GROUP BY VOUCHERINFO.Invoiceno, VOUCHERINFO.Vdate, VOUCHERINFO.Totalamount, State.GSTCode, State.Sname, VOUCHERDET.TotTaxPer, VOUCHERDET.taxamt4, VOUCHERINFO.Vnumber, VOUCHERTYPE.A HAVING (((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") AND ((VOUCHERINFO.Totalamount)>250000) AND ((VOUCHERDET.TotTaxPer)>0) AND ((State.Sname)<>'" + funs.Select_state_nm(Database.CompanyState_id) + "') AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) ORDER BY VOUCHERINFO.Vdate, VOUCHERINFO.Vnumber";
            DataTable dtb2cl = new DataTable();
            Database.GetSqlData(sql, dtb2cl);

            data = new object[dtb2cl.Rows.Count, 8];

            for (int i = 0; i < dtb2cl.Rows.Count; i++)
            {

                data[i, 0] = dtb2cl.Rows[i]["Invoiceno"].ToString();
                data[i, 1] = DateTime.Parse(dtb2cl.Rows[i]["Vdate"].ToString()).ToString("dd-MMM-yyyy");

                data[i, 2] = funs.DecimalPoint(double.Parse(dtb2cl.Rows[i]["Totalamount"].ToString()), 2);
                data[i, 3] = dtb2cl.Rows[i]["Sname"].ToString();

                data[i, 4] = funs.DecimalPoint(double.Parse(dtb2cl.Rows[i]["TotTaxPer"].ToString()), 2);

                data[i, 5] = funs.DecimalPoint(double.Parse(dtb2cl.Rows[i]["ItemTaxable"].ToString()), 2);

                if (double.Parse(dtb2cl.Rows[i]["taxamt4"].ToString()) == 0)
                {
                    data[i, 6] = "";
                }
                else
                {
                    data[i, 6] = double.Parse(dtb2cl.Rows[i]["taxamt4"].ToString());
                }
                data[i, 7] = "";


            }
            startcell = (Excel.Range)ws.Cells[5, 1];
            endcell = (Excel.Range)ws.Cells[dtb2cl.Rows.Count + 4, 8];
            writerange = ws.Range[startcell, endcell];
            writerange.Value = data;


            ws = (Excel.Worksheet)wb.Worksheets["b2cs"];

          //  sql = "SELECT State.GSTCode " + access_sql.Concat + " '-' " + access_sql.Concat + " State.Sname AS sname, VOUCHERDET.TotTaxPer, Sum(VOUCHERDET.Taxabelamount) AS ItemTaxable, VOUCHERDET.taxamt4 FROM ((((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id) RIGHT JOIN (VOUCHERDET LEFT JOIN DESCRIPTION ON VOUCHERDET.Des_ac_id = DESCRIPTION.Des_id) ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN TAXCATEGORY ON VOUCHERDET.Category_Id = TAXCATEGORY.Category_Id) LEFT JOIN State ON ACCOUNT.State_id = State.State_id WHERE VOUCHERTYPE.A=" + access_sql.Singlequote + "True" + access_sql.Singlequote + " AND VOUCHERTYPE.Type='Sale' AND ACCOUNT.RegStatus='Unregistered' AND VOUCHERINFO.Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And VOUCHERINFO.Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + " AND ((VOUCHERINFO.Totalamount<250000 and State.Sname<>'" + funs.Select_state_nm(Database.CompanyState_id) + "') or  State.Sname='" + funs.Select_state_nm(Database.CompanyState_id) + "') GROUP BY State.GSTCode " + access_sql.Concat + " '-' " + access_sql.Concat + " State.Sname, VOUCHERDET.TotTaxPer, VOUCHERDET.taxamt4, State.GSTCode, State.Sname";
            sql = "SELECT State.GSTCode " + access_sql.Concat + " '-' " + access_sql.Concat + " State.Sname AS sname, VOUCHERDET.TotTaxPer, Sum(VOUCHERDET.Taxabelamount) AS ItemTaxable, VOUCHERDET.taxamt4 FROM ((((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id2 = ACCOUNT.Ac_id) RIGHT JOIN (VOUCHERDET LEFT JOIN DESCRIPTION ON VOUCHERDET.Des_ac_id = DESCRIPTION.Des_id) ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN TAXCATEGORY ON VOUCHERDET.Category_Id = TAXCATEGORY.Category_Id) LEFT JOIN State ON VOUCHERINFO.ShiptoStateid = State.State_id WHERE VOUCHERTYPE.A=" + access_sql.Singlequote + "True" + access_sql.Singlequote + " AND VOUCHERTYPE.Type='Sale' AND ACCOUNT.RegStatus='Unregistered' AND VOUCHERINFO.Vdate>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And VOUCHERINFO.Vdate<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + " AND ((VOUCHERINFO.Totalamount<250000 and State.Sname<>'" + funs.Select_state_nm(Database.CompanyState_id) + "') or  State.Sname='" + funs.Select_state_nm(Database.CompanyState_id) + "') GROUP BY State.GSTCode " + access_sql.Concat + " '-' " + access_sql.Concat + " State.Sname, VOUCHERDET.TotTaxPer, VOUCHERDET.taxamt4, State.GSTCode, State.Sname HAVING (((VOUCHERDET.TotTaxPer)>0))";
            DataTable dtb2cs = new DataTable();
            Database.GetSqlData(sql, dtb2cs);
            data = new object[dtb2cs.Rows.Count, 6];

            for (int i = 0; i < dtb2cs.Rows.Count; i++)
            {

                data[i, 0] = "OE";
                data[i, 1] = dtb2cs.Rows[i]["Sname"].ToString();

                data[i, 2] = funs.DecimalPoint(double.Parse(dtb2cs.Rows[i]["TotTaxPer"].ToString()), 2);
                data[i, 3] = funs.DecimalPoint(double.Parse(dtb2cs.Rows[i]["ItemTaxable"].ToString()), 2);

                if (double.Parse(dtb2cs.Rows[i]["taxamt4"].ToString()) == 0)
                {
                    data[i, 4] = "";
                }
                else
                {
                    data[i, 4] = double.Parse(dtb2cs.Rows[i]["taxamt4"].ToString());
                }

                data[i, 5] = "";


            }

            startcell = (Excel.Range)ws.Cells[5, 1];
            endcell = (Excel.Range)ws.Cells[dtb2cs.Rows.Count + 4, 6];
            writerange = ws.Range[startcell, endcell];
            writerange.Value = data;


            ws = (Excel.Worksheet)wb.Worksheets["cdnr"];

           // sql = "SELECT ACCOUNT.Tin_number, VOUCHERINFO.Invoiceno, VOUCHERINFO.Vdate, State.GSTCode " + access_sql.Concat + " '-' " + access_sql.Concat + " State.Sname AS sname, VOUCHERINFO.Totalamount, VOUCHERDET.TotTaxPer, Sum(VOUCHERDET.Taxabelamount) AS ItemTaxable, VOUCHERDET.taxamt4 FROM ((((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id) RIGHT JOIN (VOUCHERDET LEFT JOIN DESCRIPTION ON VOUCHERDET.Des_ac_id = DESCRIPTION.Des_id) ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN TAXCATEGORY ON VOUCHERDET.Category_Id = TAXCATEGORY.Category_Id) LEFT JOIN State ON ACCOUNT.State_id = State.State_id WHERE VOUCHERTYPE.Type='Return' AND (ACCOUNT.RegStatus='Regular Registration' or ACCOUNT.RegStatus='Composition Dealer') GROUP BY ACCOUNT.Tin_number, VOUCHERINFO.Invoiceno, VOUCHERINFO.Vdate, State.GSTCode " + access_sql.Concat + " '-' " + access_sql.Concat + " State.Sname, VOUCHERINFO.Totalamount, VOUCHERDET.TotTaxPer, VOUCHERDET.taxamt4, VOUCHERTYPE.A HAVING (((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) ORDER BY VOUCHERINFO.Vdate";
            sql = "SELECT ACCOUNT.Tin_number, VOUCHERINFO.Invoiceno, VOUCHERINFO.Vdate, State.GSTCode " + access_sql.Concat + " '-' " + access_sql.Concat + " State.Sname AS sname, VOUCHERINFO.Totalamount, VOUCHERDET.TotTaxPer, Sum(VOUCHERDET.Taxabelamount) AS ItemTaxable, VOUCHERDET.taxamt4 FROM ((((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id2 = ACCOUNT.Ac_id) RIGHT JOIN (VOUCHERDET LEFT JOIN DESCRIPTION ON VOUCHERDET.Des_ac_id = DESCRIPTION.Des_id) ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN TAXCATEGORY ON VOUCHERDET.Category_Id = TAXCATEGORY.Category_Id) LEFT JOIN State ON VOUCHERINFO.ShiptoStateid = State.State_id WHERE VOUCHERTYPE.Type='Return' AND (ACCOUNT.RegStatus='Regular Registration' or ACCOUNT.RegStatus='Composition Dealer') GROUP BY ACCOUNT.Tin_number, VOUCHERINFO.Invoiceno, VOUCHERINFO.Vdate, State.GSTCode " + access_sql.Concat + " '-' " + access_sql.Concat + " State.Sname, VOUCHERINFO.Totalamount, VOUCHERDET.TotTaxPer, VOUCHERDET.taxamt4, VOUCHERTYPE.A HAVING (((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) ORDER BY VOUCHERINFO.Vdate";
            DataTable dtcdnr = new DataTable();
            Database.GetSqlData(sql, dtcdnr);
            data = new object[dtcdnr.Rows.Count, 13];

            for (int i = 0; i < dtcdnr.Rows.Count; i++)
            {

                data[i, 0] = dtcdnr.Rows[i]["Tin_number"].ToString();
                data[i, 1] = "";

                data[i, 2] = "";
                data[i, 3] = dtcdnr.Rows[i]["Invoiceno"].ToString();
                data[i, 4] = DateTime.Parse(dtcdnr.Rows[i]["Vdate"].ToString()).ToString("dd-MMM-yyyy");
                data[i, 5] = "C";
                data[i, 6] = "01-Sales Return";
                data[i, 7] = dtcdnr.Rows[i]["sname"].ToString();
                data[i, 8] = funs.DecimalPoint(double.Parse(dtcdnr.Rows[i]["Totalamount"].ToString()), 2);
                data[i, 9] = funs.DecimalPoint(double.Parse(dtcdnr.Rows[i]["TotTaxPer"].ToString()), 2);
                data[i, 10] = funs.DecimalPoint(double.Parse(dtcdnr.Rows[i]["ItemTaxable"].ToString()), 2);

                if (double.Parse(dtcdnr.Rows[i]["taxamt4"].ToString()) == 0)
                {
                    data[i, 11] = "";
                }
                else
                {
                    data[i, 11] = double.Parse(dtcdnr.Rows[i]["taxamt4"].ToString());
                }

                data[i, 12] = "N";


            }


            startcell = (Excel.Range)ws.Cells[5, 1];
            endcell = (Excel.Range)ws.Cells[dtcdnr.Rows.Count + 4, 13];
            writerange = ws.Range[startcell, endcell];
            writerange.Value = data;


            ws = (Excel.Worksheet)wb.Worksheets["cdnur"];

           // sql = "SELECT VOUCHERINFO.Invoiceno, VOUCHERINFO.Vdate, State.GSTCode " + access_sql.Concat + " '-' " + access_sql.Concat + " State.Sname AS sname, VOUCHERINFO.Totalamount, VOUCHERDET.TotTaxPer, Sum(VOUCHERDET.Taxabelamount) AS ItemTaxable, VOUCHERDET.taxamt4 FROM ((((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id) RIGHT JOIN (VOUCHERDET LEFT JOIN DESCRIPTION ON VOUCHERDET.Des_ac_id = DESCRIPTION.Des_id) ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN TAXCATEGORY ON VOUCHERDET.Category_Id = TAXCATEGORY.Category_Id) LEFT JOIN State ON ACCOUNT.State_id = State.State_id WHERE (((State.Sname)<>'" + funs.Select_state_nm(Database.CompanyState_id) + "') AND ((VOUCHERTYPE.Type)='Return') AND ((ACCOUNT.RegStatus)='Unregistered')) GROUP BY VOUCHERINFO.Invoiceno, VOUCHERINFO.Vdate, VOUCHERINFO.Totalamount, VOUCHERDET.TotTaxPer, VOUCHERDET.taxamt4, State.GSTCode, State.Sname, VOUCHERTYPE.A HAVING (((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") AND ((VOUCHERINFO.Totalamount)>250000) AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) ORDER BY VOUCHERINFO.Vdate";
            sql = "SELECT VOUCHERINFO.Invoiceno, VOUCHERINFO.Vdate, State.GSTCode " + access_sql.Concat + " '-' " + access_sql.Concat + " State.Sname AS sname, VOUCHERINFO.Totalamount, VOUCHERDET.TotTaxPer, Sum(VOUCHERDET.Taxabelamount) AS ItemTaxable, VOUCHERDET.taxamt4 FROM ((((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id2 = ACCOUNT.Ac_id) RIGHT JOIN (VOUCHERDET LEFT JOIN DESCRIPTION ON VOUCHERDET.Des_ac_id = DESCRIPTION.Des_id) ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN TAXCATEGORY ON VOUCHERDET.Category_Id = TAXCATEGORY.Category_Id) LEFT JOIN State ON VOUCHERINFO.ShiptoStateid = State.State_id WHERE (((State.Sname)<>'" + funs.Select_state_nm(Database.CompanyState_id) + "') AND ((VOUCHERTYPE.Type)='Return') AND ((ACCOUNT.RegStatus)='Unregistered')) GROUP BY VOUCHERINFO.Invoiceno, VOUCHERINFO.Vdate, VOUCHERINFO.Totalamount, VOUCHERDET.TotTaxPer, VOUCHERDET.taxamt4, State.GSTCode, State.Sname, VOUCHERTYPE.A HAVING (((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") AND ((VOUCHERINFO.Totalamount)>250000) AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) ORDER BY VOUCHERINFO.Vdate";
            DataTable dtcdnur = new DataTable();
            Database.GetSqlData(sql, dtcdnur);
            data = new object[dtcdnur.Rows.Count, 13];

            for (int i = 0; i < dtcdnur.Rows.Count; i++)
            {

                data[i, 0] = "";
                data[i, 1] = dtcdnur.Rows[i]["Invoiceno"].ToString();
                data[i, 2] = DateTime.Parse(dtcdnur.Rows[i]["Vdate"].ToString()).ToString("dd-MMM-yyyy");
                data[i, 3] = "C";
                data[i, 4] = "";
                data[i, 5] = "";
                data[i, 6] = "01-Sales Return";
                data[i, 7] = dtcdnur.Rows[i]["sname"].ToString();
                data[i, 8] = funs.DecimalPoint(double.Parse(dtcdnur.Rows[i]["Totalamount"].ToString()), 2);
                data[i, 9] = funs.DecimalPoint(double.Parse(dtcdnur.Rows[i]["TotTaxPer"].ToString()), 2);
                data[i, 10] = funs.DecimalPoint(double.Parse(dtcdnur.Rows[i]["ItemTaxable"].ToString()), 2);
                if (double.Parse(dtcdnur.Rows[i]["taxamt4"].ToString()) == 0)
                {
                    data[i, 11] = "";
                }
                else
                {
                    data[i, 11] = double.Parse(dtcdnur.Rows[i]["taxamt4"].ToString());
                }
                data[i, 12] = "N";
            }
            startcell = (Excel.Range)ws.Cells[5, 1];
            endcell = (Excel.Range)ws.Cells[dtcdnur.Rows.Count + 4, 13];
            writerange = ws.Range[startcell, endcell];
            writerange.Value = data;




            ws = (Excel.Worksheet)wb.Worksheets["exemp"];

            //Interstate- Registered
          //  sql = "SELECT Sum(VOUCHERDET.Taxabelamount) AS ItemTaxable FROM (((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id) RIGHT JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN State ON ACCOUNT.State_id = State.State_id WHERE (((State.Sname)<>'" + funs.Select_state_nm(Database.CompanyState_id) + "') AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") AND ((VOUCHERDET.TotTaxPer)=0) AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ") AND ((ACCOUNT.RegStatus='Regular Registration' or ACCOUNT.RegStatus='Composition Dealer'))) GROUP BY VOUCHERTYPE.Type HAVING (((VOUCHERTYPE.Type)='Sale'))";
            sql = "SELECT Sum(VOUCHERDET.Taxabelamount) AS ItemTaxable FROM (((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id2 = ACCOUNT.Ac_id) RIGHT JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN State ON VOUCHERINFO.ShiptoStateid = State.State_id WHERE (((State.Sname)<>'" + funs.Select_state_nm(Database.CompanyState_id) + "') AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") AND ((VOUCHERDET.TotTaxPer)=0) AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ") AND ((ACCOUNT.RegStatus='Regular Registration' or ACCOUNT.RegStatus='Composition Dealer'))) GROUP BY VOUCHERTYPE.Type HAVING (((VOUCHERTYPE.Type)='Sale'))";
            DataTable dtexemp = new DataTable();
            Database.GetSqlData(sql, dtexemp);

            data = new object[1, 4];
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
            startcell = (Excel.Range)ws.Cells[5, 2];
            endcell = (Excel.Range)ws.Cells[5, 4];
            writerange = ws.Range[startcell, endcell];
            writerange.Value = data;



            //Intrastate Registered
           // sql = "SELECT Sum(VOUCHERDET.Taxabelamount) AS ItemTaxable FROM (((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id) RIGHT JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN State ON ACCOUNT.State_id = State.State_id WHERE (((State.Sname)='" + funs.Select_state_nm(Database.CompanyState_id) + "') AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") AND ((VOUCHERDET.TotTaxPer)=0) AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ") AND ((ACCOUNT.RegStatus='Regular Registration' or ACCOUNT.RegStatus='Composition Dealer'))) GROUP BY VOUCHERTYPE.Type HAVING (((VOUCHERTYPE.Type)='Sale'))";
            sql = "SELECT Sum(VOUCHERDET.Taxabelamount) AS ItemTaxable FROM (((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id2 = ACCOUNT.Ac_id) RIGHT JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN State ON VOUCHERINFO.ShiptoStateid = State.State_id WHERE (((State.Sname)='" + funs.Select_state_nm(Database.CompanyState_id) + "') AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") AND ((VOUCHERDET.TotTaxPer)=0) AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ") AND ((ACCOUNT.RegStatus='Regular Registration' or ACCOUNT.RegStatus='Composition Dealer'))) GROUP BY VOUCHERTYPE.Type HAVING (((VOUCHERTYPE.Type)='Sale'))";
            Database.GetSqlData(sql, dtexemp);
            data = new object[1, 4];
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
            startcell = (Excel.Range)ws.Cells[6, 2];
            endcell = (Excel.Range)ws.Cells[6, 4];
            writerange = ws.Range[startcell, endcell];
            writerange.Value = data;


            //Interstate Unregistered
           // sql = "SELECT Sum(VOUCHERDET.Taxabelamount) AS ItemTaxable FROM (((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id) RIGHT JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN State ON ACCOUNT.State_id = State.State_id WHERE (((State.Sname)<>'" + funs.Select_state_nm(Database.CompanyState_id) + "') AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") AND ((VOUCHERDET.TotTaxPer)=0) AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ") AND ((ACCOUNT.RegStatus)='Unregistered')) GROUP BY VOUCHERTYPE.Type HAVING (((VOUCHERTYPE.Type)='Sale'))";
            sql = "SELECT Sum(VOUCHERDET.Taxabelamount) AS ItemTaxable FROM (((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id2 = ACCOUNT.Ac_id) RIGHT JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN State ON VOUCHERINFO.ShiptoStateid = State.State_id WHERE (((State.Sname)<>'" + funs.Select_state_nm(Database.CompanyState_id) + "') AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") AND ((VOUCHERDET.TotTaxPer)=0) AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ") AND ((ACCOUNT.RegStatus)='Unregistered')) GROUP BY VOUCHERTYPE.Type HAVING (((VOUCHERTYPE.Type)='Sale'))";
            Database.GetSqlData(sql, dtexemp);
            data = new object[1, 4];
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
            startcell = (Excel.Range)ws.Cells[7, 2];
            endcell = (Excel.Range)ws.Cells[7, 4];
            writerange = ws.Range[startcell, endcell];
            writerange.Value = data;

            //Intrastate Unregistered
           // sql = "SELECT Sum(VOUCHERDET.Taxabelamount) AS ItemTaxable FROM (((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id) RIGHT JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN State ON ACCOUNT.State_id = State.State_id WHERE (((State.Sname)='" + funs.Select_state_nm(Database.CompanyState_id) + "') AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") AND ((VOUCHERDET.TotTaxPer)=0) AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ") AND ((ACCOUNT.RegStatus)='Unregistered')) GROUP BY VOUCHERTYPE.Type HAVING (((VOUCHERTYPE.Type)='Sale'))";
            sql = "SELECT Sum(VOUCHERDET.Taxabelamount) AS ItemTaxable FROM (((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id2 = ACCOUNT.Ac_id) RIGHT JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN State ON VOUCHERINFO.ShiptoStateid = State.State_id WHERE (((State.Sname)='" + funs.Select_state_nm(Database.CompanyState_id) + "') AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") AND ((VOUCHERDET.TotTaxPer)=0) AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ") AND ((ACCOUNT.RegStatus)='Unregistered')) GROUP BY VOUCHERTYPE.Type HAVING (((VOUCHERTYPE.Type)='Sale'))";
            Database.GetSqlData(sql, dtexemp);
            data = new object[1, 4];
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
            startcell = (Excel.Range)ws.Cells[8, 2];
            endcell = (Excel.Range)ws.Cells[8, 4];
            writerange = ws.Range[startcell, endcell];
            writerange.Value = data;



            //hsn
            ws = (Excel.Worksheet)wb.Worksheets["hsn"];
           // sql = "SELECT TAXCATEGORY.Commodity_Code, TAXCATEGORY.Category_Name as Cat_Name, VOUCHERDET.Rate_Unit, Sum(VOUCHERDET.Quantity * VOUCHERDET.Pvalue) AS Quantity, Sum(VOUCHERDET.Taxabelamount+VOUCHERDET.taxamt3+VOUCHERDET.taxamt1+VOUCHERDET.taxamt2) AS Totalamount, Sum(VOUCHERDET.Taxabelamount) AS ItemTaxable, Sum(VOUCHERDET.taxamt3) AS taxamt3, Sum(VOUCHERDET.taxamt1) AS taxamt1, Sum(VOUCHERDET.taxamt2) AS taxamt2, Sum(VOUCHERDET.taxamt4) AS taxamt4 FROM ((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) RIGHT JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN TAXCATEGORY ON VOUCHERDET.Category_Id = TAXCATEGORY.Category_Id WHERE (((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") AND ((VOUCHERTYPE.Type)='Sale')) GROUP BY TAXCATEGORY.Commodity_Code, TAXCATEGORY.Category_Name, VOUCHERDET.Rate_Unit, VOUCHERTYPE.A HAVING (((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) ORDER BY TAXCATEGORY.Commodity_Code";
            sql = "SELECT TAXCATEGORY.Commodity_Code, TAXCATEGORY.Category_Name AS Cat_Name, VOUCHERDET.Rate_Unit, Sum(VOUCHERDET.Quantity*VOUCHERDET.Pvalue) AS Quantity, Sum(VOUCHERDET.Taxabelamount+VOUCHERDET.taxamt3+VOUCHERDET.taxamt1+VOUCHERDET.taxamt2) AS Totalamount, Sum(VOUCHERDET.Taxabelamount) AS ItemTaxable, Sum(VOUCHERDET.taxamt3) AS taxamt3, Sum(VOUCHERDET.taxamt1) AS taxamt1, Sum(VOUCHERDET.taxamt2) AS taxamt2, Sum(VOUCHERDET.taxamt4) AS taxamt4, VOUCHERTYPE.Type FROM ((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) RIGHT JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN TAXCATEGORY ON VOUCHERDET.Category_Id = TAXCATEGORY.Category_Id WHERE (((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ")) GROUP BY TAXCATEGORY.Commodity_Code, TAXCATEGORY.Category_Name, VOUCHERDET.Rate_Unit, VOUCHERTYPE.A, VOUCHERTYPE.Type HAVING (((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ") AND ((VOUCHERTYPE.Type)='Sale' Or (VOUCHERTYPE.Type)='Return')) ORDER BY TAXCATEGORY.Commodity_Code";
            DataTable dthsn = new DataTable();
            Database.GetSqlData(sql, dthsn);

            DataTable   tdt = new DataTable();
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



            ws = (Excel.Worksheet)wb.Worksheets["docs"];
            //sale
            sql = "SELECT VOUCHERTYPE.Name as vname, Count(VOUCHERINFO.Vi_id)  as cnt FROM VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) GROUP BY VOUCHERTYPE.Type, VOUCHERTYPE.Name HAVING (((VOUCHERTYPE.Type)='Sale'))";
            DataTable dtdocs = new DataTable();
            Database.GetSqlData(sql, dtdocs);
            data = new object[dtdocs.Rows.Count, 5];

            int lno = dtdocs.Rows.Count+4;
            for (int i = 0; i < dtdocs.Rows.Count; i++)
            {
                data[i, 0] = "Invoice for outward supply";
                dthsn = new DataTable();
                Database.GetSqlData("SELECT VOUCHERTYPE.Name AS vname, VOUCHERINFO.Invoiceno FROM VOUCHERTYPE LEFT JOIN VOUCHERINFO ON VOUCHERTYPE.Vt_id = VOUCHERINFO.Vt_id WHERE (((VOUCHERTYPE.Name)='" + dtdocs.Rows[i]["vname"].ToString() + "') AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ")) ORDER BY VOUCHERINFO.Vdate, VOUCHERINFO.Vnumber", dthsn);
               // Database.GetSqlData("SELECT VOUCHERTYPE.Name as vname, VOUCHERINFO.Invoiceno FROM VOUCHERTYPE LEFT JOIN VOUCHERINFO ON VOUCHERTYPE.Vt_id = VOUCHERINFO.Vt_id WHERE (((VOUCHERTYPE.Name)='" + dtdocs.Rows[i]["vname"].ToString() + "') AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ")) ORDER BY VOUCHERINFO.Vdate,VOUCHERINFO.Vi_id ", dthsn);
                data[i, 1] = dthsn.Rows[0]["Invoiceno"].ToString();


                dthsn = new DataTable();
               // Database.GetSqlData("SELECT VOUCHERTYPE.Name as vname, VOUCHERINFO.Invoiceno FROM VOUCHERTYPE LEFT JOIN VOUCHERINFO ON VOUCHERTYPE.Vt_id = VOUCHERINFO.Vt_id WHERE (((VOUCHERTYPE.Name)='" + dtdocs.Rows[i]["vname"].ToString() + "') AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ")) ORDER BY VOUCHERINFO.Vdate DESC ,  VOUCHERINFO.Vnumber", dthsn);
                Database.GetSqlData("SELECT VOUCHERTYPE.Name AS vname, VOUCHERINFO.Invoiceno FROM VOUCHERTYPE LEFT JOIN VOUCHERINFO ON VOUCHERTYPE.Vt_id = VOUCHERINFO.Vt_id WHERE (((VOUCHERTYPE.Name)='" + dtdocs.Rows[i]["vname"].ToString() + "') AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ")) ORDER BY VOUCHERINFO.Vdate DESC , VOUCHERINFO.Vnumber DESC", dthsn);
                data[i, 2] = dthsn.Rows[0]["Invoiceno"].ToString();
                dthsn = new DataTable();
                Database.GetSqlData("SELECT Min(VOUCHERINFO.Vnumber) AS MinOfVnumber, Max(VOUCHERINFO.Vnumber) AS cnt FROM VOUCHERTYPE LEFT JOIN VOUCHERINFO ON VOUCHERTYPE.Vt_id = VOUCHERINFO.Vt_id WHERE (((VOUCHERTYPE.Name)='" + dtdocs.Rows[i]["vname"].ToString() + "') AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + "))", dthsn);
                int cnt = 0;
                cnt = int.Parse(dtdocs.Rows[i]["cnt"].ToString());
                int minvno = 0;
                int maxvno = 0;
                minvno = int.Parse(dthsn.Rows[0]["MinOfVnumber"].ToString());
                maxvno = int.Parse(dthsn.Rows[0]["cnt"].ToString());
                data[i, 3] = (maxvno - minvno)+1;
                data[i, 4] = maxvno - minvno+1-cnt;
            }


            startcell = (Excel.Range)ws.Cells[5, 1];
            endcell = (Excel.Range)ws.Cells[dtdocs.Rows.Count + 4, 5];
            writerange = ws.Range[startcell, endcell];
            writerange.Value = data;

            //RCM
            sql = "SELECT VOUCHERTYPE.Name as vname, Count(VOUCHERINFO.Vi_id)  as cnt FROM VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id WHERE (((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) GROUP BY VOUCHERTYPE.Type, VOUCHERTYPE.Name HAVING (((VOUCHERTYPE.Type)='RCM'))";
          
            Database.GetSqlData(sql, dtdocs);
            data = new object[dtdocs.Rows.Count, 5];


            for (int i = 0; i < dtdocs.Rows.Count; i++)
            {
                data[i, 0] = "Invoice for inward supply from unregistered person";
                dthsn = new DataTable();
                Database.GetSqlData("SELECT VOUCHERTYPE.Name as vname, VOUCHERINFO.Invoiceno FROM VOUCHERTYPE LEFT JOIN VOUCHERINFO ON VOUCHERTYPE.Vt_id = VOUCHERINFO.Vt_id WHERE (((VOUCHERTYPE.Name)='" + dtdocs.Rows[i]["vname"].ToString() + "') AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ")) ORDER BY VOUCHERINFO.Vdate,VOUCHERINFO.Vi_id ", dthsn);
                data[i, 1] = dthsn.Rows[0]["Invoiceno"].ToString();


                dthsn = new DataTable();
                Database.GetSqlData("SELECT VOUCHERTYPE.Name as vname, VOUCHERINFO.Invoiceno FROM VOUCHERTYPE LEFT JOIN VOUCHERINFO ON VOUCHERTYPE.Vt_id = VOUCHERINFO.Vt_id WHERE (((VOUCHERTYPE.Name)='" + dtdocs.Rows[i]["vname"].ToString() + "') AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ")) ORDER BY VOUCHERINFO.Vdate DESC , VOUCHERINFO.Vi_id DESC", dthsn);
                data[i, 2] = dthsn.Rows[0]["Invoiceno"].ToString();
                dthsn = new DataTable();
                Database.GetSqlData("SELECT Min(VOUCHERINFO.Vnumber) AS MinOfVnumber, Max(VOUCHERINFO.Vnumber) AS cnt FROM VOUCHERTYPE LEFT JOIN VOUCHERINFO ON VOUCHERTYPE.Vt_id = VOUCHERINFO.Vt_id WHERE (((VOUCHERTYPE.Name)='" + dtdocs.Rows[i]["vname"].ToString() + "') AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + "))", dthsn);
                int cnt = 0;
                cnt = int.Parse(dtdocs.Rows[i]["cnt"].ToString());
                int minvno = 0;
                int maxvno = 0;
                minvno = int.Parse(dthsn.Rows[0]["MinOfVnumber"].ToString());
                maxvno = int.Parse(dthsn.Rows[0]["cnt"].ToString());
                data[i, 3] = (maxvno - minvno) + 1;

                data[i, 4] = maxvno - minvno + 1 - cnt;
            }


            startcell = (Excel.Range)ws.Cells[lno+1, 1];
            endcell = (Excel.Range)ws.Cells[dtdocs.Rows.Count + lno, 5];
            writerange = ws.Range[startcell, endcell];
            writerange.Value = data;





        
            apl.Visible = true;
            GC.Collect();
            MessageBox.Show("Done");
            this.Close();
            this.Dispose();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frm_GSTR1_Load(object sender, EventArgs e)
        {

        }
    }
}
