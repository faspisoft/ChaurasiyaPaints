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
    public partial class frm_eFiling : Form
    {
        static Object misValue = System.Reflection.Missing.Value;
        static Excel.Application apl = new Microsoft.Office.Interop.Excel.Application();
        Excel.Workbook wb;
        Excel.Worksheet ws;        
        DateTime dt1 = new DateTime();
        DateTime dt2 = new DateTime();

        public frm_eFiling()
        {
            InitializeComponent();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Dispose(); 
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void button27_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void button24_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                listBox1.Visible = true;
                listBox2.Visible = false;
                listBox1.Focus();
                listBox1.SelectedIndex = 0;
                tabControl1.SelectedIndex = 1;
            }
            else if (radioButton2.Checked == true)
            {
                listBox1.Visible = false;
                listBox2.Visible = true;
                listBox2.Focus();
                listBox2.SelectedIndex = 0;
                tabControl1.SelectedIndex = 1;
            }
            else if (radioButton3.Checked == true)
            {
                dt1 = Database.stDate;
                dt2 = Database.enDate;
                populateGrid1();
                tabControl1.SelectedIndex = 2;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            getMonthStartEndDate();
            populateGrid1();
            tabControl1.SelectedIndex = 2;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            populateGrid2();
            tabControl1.SelectedIndex = 3;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            populateGrid3();
            tabControl1.SelectedIndex = 4;
        }

        private void getMonthStartEndDate()
        {
            if (listBox1.Visible == true)
            {
                if (listBox1.Text == "April")
                {
                    dt1 = new DateTime(Database.stDate.Year, 04, 01);
                    dt2 = new DateTime(Database.stDate.Year, 04, 30);
                }
                else if (listBox1.Text == "May")
                {
                    dt1 = new DateTime(Database.stDate.Year, 05, 01);
                    dt2 = new DateTime(Database.stDate.Year, 05, 31);
                }
                else if (listBox1.Text == "June")
                {
                    dt1 = new DateTime(Database.stDate.Year, 06, 01);
                    dt2 = new DateTime(Database.stDate.Year, 06, 30);
                }
                else if (listBox1.Text == "July")
                {
                    dt1 = new DateTime(Database.stDate.Year, 07, 01);
                    dt2 = new DateTime(Database.stDate.Year, 07, 31);
                }
                else if (listBox1.Text == "August")
                {
                    dt1 = new DateTime(Database.stDate.Year, 08, 01);
                    dt2 = new DateTime(Database.stDate.Year, 08, 31);
                }
                else if (listBox1.Text == "September")
                {
                    dt1 = new DateTime(Database.stDate.Year, 09, 01);
                    dt2 = new DateTime(Database.stDate.Year, 09, 30);
                }
                else if (listBox1.Text == "October")
                {
                    dt1 = new DateTime(Database.stDate.Year, 10, 01);
                    dt2 = new DateTime(Database.stDate.Year, 10, 31);
                }
                else if (listBox1.Text == "November")
                {
                    dt1 = new DateTime(Database.stDate.Year, 11, 01);
                    dt2 = new DateTime(Database.stDate.Year, 11, 30);
                }
                else if (listBox1.Text == "December")
                {
                    dt1 = new DateTime(Database.stDate.Year, 12, 01);
                    dt2 = new DateTime(Database.stDate.Year, 12, 31);
                }
                else if (listBox1.Text == "January")
                {
                    dt1 = new DateTime(Database.enDate.Year, 01, 01);
                    dt2 = new DateTime(Database.enDate.Year, 01, 31);
                }
                else if (listBox1.Text == "February")
                {
                    if (Database.enDate.Year % 4 == 0)
                    {
                        dt1 = new DateTime(Database.enDate.Year, 02, 01);
                        dt2 = new DateTime(Database.enDate.Year, 02, 29);
                    }
                    else
                    {
                        dt1 = new DateTime(Database.enDate.Year, 02, 01);
                        dt2 = new DateTime(Database.enDate.Year, 02, 28);
                    }
                }
                else if (listBox1.Text == "March")
                {
                    dt1 = new DateTime(Database.enDate.Year, 03, 01);
                    dt2 = new DateTime(Database.enDate.Year, 03, 31);
                }
            }
            else if (listBox2.Visible == true)
            {
                if (listBox2.SelectedIndex == 0)
                {
                    dt1 = new DateTime(Database.stDate.Year, 04, 01);
                    dt2 = new DateTime(Database.stDate.Year, 06, 30);
                }
                else if (listBox2.SelectedIndex == 1)
                {
                    dt1 = new DateTime(Database.stDate.Year, 07, 01);
                    dt2 = new DateTime(Database.stDate.Year, 09, 30);
                }
                else if (listBox2.SelectedIndex == 2)
                {
                    dt1 = new DateTime(Database.stDate.Year, 10, 01);
                    dt2 = new DateTime(Database.stDate.Year, 12, 31);
                }
                else if (listBox2.SelectedIndex == 3)
                {
                    dt1 = new DateTime(Database.stDate.Year+1 , 01, 01);
                    dt2 = new DateTime(Database.stDate.Year+1 , 03, 31);
                }
            }
        }

        private void populateGrid1()
        {
            ansGridView1.Rows.Clear();
            wb = (Excel.Workbook)apl.Workbooks.Open(Application.StartupPath + "\\efile\\Draft.xls", true, true, misValue, null, null, false, misValue, null, false, false, misValue, misValue, misValue, false);
            ws = (Excel.Worksheet)wb.Worksheets[1];
            Excel.Range range;
            range = ws.UsedRange;
            int i = 1;
            while (i <= 5)
            {
                ansGridView1.Rows.Add();
                ansGridView1.Rows[i - 1].Cells["desc"].Value = (range.Cells[(i + 1), 1] as Excel.Range).Value2;
                i++;
            }
            DataTable dtComp = new DataTable();
            Database.GetSqlData("SELECT COMPANY.Tin_no, COMPANY.Name, COMPANY.Firm_Period_name From Company WHERE (((COMPANY.Name)='" + Database.fname + "') AND ((COMPANY.Firm_Period_name)='" + Database.fyear + "'))", dtComp);
            if (dtComp.Rows.Count > 0)
            {
                ansGridView1.Rows[0].Cells["ent"].Value = dtComp.Rows[0]["Tin_no"];
                ansGridView1.Rows[1].Cells["ent"].Value = dtComp.Rows[0]["Firm_Period_name"];
            }
            if (radioButton1.Checked == true)
            {
                ansGridView1.Rows[2].Cells["ent"].Value = 2;
            }
            else if (radioButton2.Checked == true)
            {
                ansGridView1.Rows[2].Cells["ent"].Value = 3;
            }
            else if (radioButton3.Checked == true)
            {
                ansGridView1.Rows[2].Cells["ent"].Value = 1;
            }

            if (radioButton1.Checked == true)
            {
                if ((listBox1.SelectedIndex + 4) % 12 == 0)
                {
                    ansGridView1.Rows[3].Cells["ent"].Value = 12;
                }
                else
                {
                    ansGridView1.Rows[3].Cells["ent"].Value = (listBox1.SelectedIndex + 4) % 12;
                }
            }
            else
            {
                ansGridView1.Rows[3].Cells["ent"].Value = 0;
            }

            if (radioButton2.Checked == true)
            {
                ansGridView1.Rows[4].Cells["ent"].Value = listBox2.SelectedIndex + 1;
            }
            else
            {
                ansGridView1.Rows[4].Cells["ent"].Value = 0;
            }
        }

        private void populateGrid2()
        {
            ansGridView2.Rows.Clear();
            String Str1 = "";

            if (Database.IsKacha == false)
            {
                Str1 = "SELECT VOUCHERINFO.Vnumber, " + access_sql.fnDatFormatting("VOUCHERINFO.Vdate", Database.dformat) + " AS sdt, VOUCHERINFO.Svnum," + access_sql.fnDatFormatting("VOUCHERINFO.Svdate", Database.dformat) + " as Svdate, TAXCATEGORY.Commodity_Code, Sum(VOUCHERDET.Quantity * VOUCHERDET.Pvalue) AS Quantity, Sum(VOUCHERDET.Taxabelamount) AS ItemTaxable, Sum(VOUCHERDET.taxamt1) AS Tax1, Sum(VOUCHERDET.taxamt2) AS Tax2, Sum(VOUCHERDET.Taxabelamount+VOUCHERDET.taxamt1+VOUCHERDET.taxamt2) AS VoucherNetAmt, ACCOUNT.Tin_number, VOUCHERDET.Rate_Unit AS Utype FROM (((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id) LEFT JOIN (VOUCHERDET LEFT JOIN DESCRIPTION ON VOUCHERDET.Des_ac_id = DESCRIPTION.Des_id) ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN TAXCATEGORY ON VOUCHERDET.Category_Id = TAXCATEGORY.Category_Id WHERE VOUCHERTYPE.Short='PRO'";
                Str1 += " GROUP BY VOUCHERINFO.Vnumber, VOUCHERINFO.Svnum, TAXCATEGORY.Commodity_Code, ACCOUNT.Tin_number, VOUCHERDET.Rate_Unit, VOUCHERINFO.Svdate, VOUCHERINFO.Vdate, VOUCHERINFO.Vnumber, VOUCHERTYPE.A HAVING (((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) AND (SUM(Voucherdet.taxamt1) <> 0) AND (SUM(Voucherdet.taxamt2) <> 0) ORDER BY VOUCHERINFO.Vdate, VOUCHERINFO.Vnumber";
            }
            DataTable dtGrid2 = new DataTable();
            Database.GetSqlData(Str1, dtGrid2);
            for (int i = 0; i < dtGrid2.Rows.Count; i++)
            {
                ansGridView2.Rows.Add();
                ansGridView2.Rows[i].Cells["invno"].Value = dtGrid2.Rows[i]["Svnum"];
                ansGridView2.Rows[i].Cells["invdt"].Value = DateTime.Parse(dtGrid2.Rows[i]["Svdate"].ToString()).ToString("dd/MM/yyyy");
                ansGridView2.Rows[i].Cells["comm_code"].Value = dtGrid2.Rows[i]["Commodity_Code"];
                ansGridView2.Rows[i].Cells["comm_qty"].Value = dtGrid2.Rows[i]["Quantity"];
                ansGridView2.Rows[i].Cells["tax_goods"].Value = dtGrid2.Rows[i]["ItemTaxable"];
                ansGridView2.Rows[i].Cells["vat"].Value = dtGrid2.Rows[i]["Tax1"];
                ansGridView2.Rows[i].Cells["sat"].Value = dtGrid2.Rows[i]["Tax2"];
                ansGridView2.Rows[i].Cells["totinval"].Value = dtGrid2.Rows[i]["VoucherNetAmt"];
                ansGridView2.Rows[i].Cells["ven_cust_tin"].Value = dtGrid2.Rows[i]["Tin_number"];
                ansGridView2.Rows[i].Cells["unt"].Value = dtGrid2.Rows[i]["Utype"];
                ansGridView2.Rows[i].Cells["typ"].Value = 1;
            }
        }

        private void populateGrid3()
        {
            ansGridView3.Rows.Clear();
            String Str1 = "";

            if (Database.IsKacha == false)
            {
                Str1 = "SELECT VOUCHERINFO.Vnumber, " + access_sql.fnDatFormatting("VOUCHERINFO.Vdate", Database.dformat) + " AS Expr1, VOUCHERINFO.Svnum,  " + access_sql.fnDatFormatting("[VOUCHERINFO].[SVdate]", Database.dformat) + " AS Svdate, TAXCATEGORY.Commodity_Code, Sum([Voucherdet].[Pvalue]*[Quantity]) AS Quantity, Sum(VOUCHERDET.Taxabelamount) AS ItemTaxable, Sum(VOUCHERDET.taxamt1) AS Tax1, Sum(VOUCHERDET.taxamt2) AS Tax2, Sum(VOUCHERDET.Taxabelamount+VOUCHERDET.taxamt1+VOUCHERDET.taxamt2) AS VoucherNetAmt, ACCOUNT.Tin_number, VOUCHERDET.Rate_Unit AS Utype FROM (((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id) LEFT JOIN (VOUCHERDET LEFT JOIN DESCRIPTION ON VOUCHERDET.Des_ac_id = DESCRIPTION.Des_id) ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN TAXCATEGORY ON VOUCHERDET.Category_Id = TAXCATEGORY.Category_Id WHERE (((VOUCHERTYPE.Short)='RPO')) ";
                Str1 += " GROUP BY VOUCHERINFO.Vnumber, VOUCHERINFO.Svnum, TAXCATEGORY.Commodity_Code, ACCOUNT.Tin_number, VOUCHERDET.Rate_Unit, VOUCHERINFO.Svdate, VOUCHERINFO.Vdate, VOUCHERINFO.Vnumber, VOUCHERTYPE.A HAVING (((Sum(VOUCHERDET.taxamt1))<>0) AND ((Sum(VOUCHERDET.taxamt2))<>0) AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + ") AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) ORDER BY VOUCHERINFO.Vdate, VOUCHERINFO.Vnumber";
            }           

            DataTable dtGrid3 = new DataTable();
            Database.GetSqlData(Str1, dtGrid3);
            for (int i = 0; i < dtGrid3.Rows.Count; i++)
            {
                ansGridView3.Rows.Add();
                ansGridView3.Rows[i].Cells["invno2"].Value = "";
                ansGridView3.Rows[i].Cells["invdt2"].Value = "";
                ansGridView3.Rows[i].Cells["comm_code2"].Value = dtGrid3.Rows[i]["Commodity_Code"];
                ansGridView3.Rows[i].Cells["comm_qty2"].Value = dtGrid3.Rows[i]["Quantity"];
                ansGridView3.Rows[i].Cells["tax_goods2"].Value = dtGrid3.Rows[i]["ItemTaxable"];
                ansGridView3.Rows[i].Cells["vat2"].Value = dtGrid3.Rows[i]["Tax1"];
                ansGridView3.Rows[i].Cells["sat2"].Value = dtGrid3.Rows[i]["Tax2"];
                ansGridView3.Rows[i].Cells["totinval2"].Value = dtGrid3.Rows[i]["VoucherNetAmt"];
                ansGridView3.Rows[i].Cells["ven_cust_tin2"].Value = dtGrid3.Rows[i]["Tin_number"];
                ansGridView3.Rows[i].Cells["unt2"].Value = dtGrid3.Rows[i]["Utype"];
                ansGridView3.Rows[i].Cells["crnoteno"].Value = dtGrid3.Rows[i]["Vnumber"];
                ansGridView3.Rows[i].Cells["crnotedt"].Value = dtGrid3.Rows[i]["Expr1"];
                ansGridView3.Rows[i].Cells["drnoteno"].Value = dtGrid3.Rows[i]["Svnum"];
                ansGridView3.Rows[i].Cells["drnotedt"].Value =DateTime.Parse( dtGrid3.Rows[i]["Svdate"].ToString()).ToString("dd/MM/yyyy");
                ansGridView3.Rows[i].Cells["vat_non_vat"].Value = "V";
                ansGridView3.Rows[i].Cells["type"].Value = 1;               
            }
        }
        
        private void populateGrid4()
        {
            ansGridView4.Rows.Clear();
            String Str1 = "";

            if (Database.IsKacha == false)
            {
                Str1 = "SELECT VOUCHERINFO.Vnumber, " + access_sql.fnDatFormatting(" VOUCHERINFO.Vdate", Database.dformat) + " AS dt, VOUCHERINFO.Svnum, VOUCHERINFO.Svdate, TAXCATEGORY.Commodity_Code, Sum(VOUCHERDET.[Pvalue]*[Quantity]) AS Quantity, Sum(VOUCHERDET.Taxabelamount) AS ItemTaxable, Sum(VOUCHERDET.taxamt1) AS Tax1, Sum(VOUCHERDET.taxamt2) AS Tax2, Sum(Taxabelamount+taxamt1+taxamt2) AS VoucherNetAmt, ACCOUNT.Tin_number, VOUCHERDET.Rate_Unit AS Utype FROM (((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id) LEFT JOIN (VOUCHERDET LEFT JOIN DESCRIPTION ON VOUCHERDET.Des_ac_id = DESCRIPTION.Des_id) ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN TAXCATEGORY ON VOUCHERDET.Category_Id = TAXCATEGORY.Category_Id WHERE (((VOUCHERTYPE.Short)='SLT')) GROUP BY VOUCHERINFO.Vnumber, VOUCHERINFO.Svnum, VOUCHERINFO.Svdate, TAXCATEGORY.Commodity_Code, ACCOUNT.Tin_number, VOUCHERDET.Rate_Unit, VOUCHERINFO.Vdate, VOUCHERINFO.Vnumber, VOUCHERTYPE.A";
                Str1 += " HAVING (((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) ORDER BY VOUCHERINFO.Vdate, VOUCHERINFO.Vnumber";
            }
           
            DataTable dtGrid4 = new DataTable();
            Database.GetSqlData(Str1, dtGrid4);
            for (int i = 0; i < dtGrid4.Rows.Count; i++)
            {
                ansGridView4.Rows.Add();
                ansGridView4.Rows[i].Cells["invno3"].Value = dtGrid4.Rows[i]["Vnumber"];
                ansGridView4.Rows[i].Cells["invdt3"].Value =DateTime.Parse( dtGrid4.Rows[i]["dt"].ToString()).ToString("dd/MM/yyyy");
                ansGridView4.Rows[i].Cells["comm_code3"].Value = dtGrid4.Rows[i]["Commodity_Code"];
                ansGridView4.Rows[i].Cells["comm_qty3"].Value = dtGrid4.Rows[i]["Quantity"];
                ansGridView4.Rows[i].Cells["tax_goods3"].Value = dtGrid4.Rows[i]["ItemTaxable"];
                ansGridView4.Rows[i].Cells["vat3"].Value = dtGrid4.Rows[i]["Tax1"];
                ansGridView4.Rows[i].Cells["sat3"].Value = dtGrid4.Rows[i]["Tax2"];
                ansGridView4.Rows[i].Cells["totinval3"].Value = dtGrid4.Rows[i]["VoucherNetAmt"];
                ansGridView4.Rows[i].Cells["ven_cust_tin3"].Value = dtGrid4.Rows[i]["Tin_number"];
                ansGridView4.Rows[i].Cells["unt3"].Value = dtGrid4.Rows[i]["Utype"];
                ansGridView4.Rows[i].Cells["type2"].Value = 1;
            }
        }

        private void populateGrid5()
        {
            ansGridView5.Rows.Clear();
            String Str1 = "";
          
            if (Database.IsKacha == false)
            {
                Str1 = "SELECT VOUCHERINFO.Vnumber, " + access_sql.fnDatFormatting("VOUCHERINFO.Vdate", Database.dformat) + "AS dt, VOUCHERINFO.Svnum, VOUCHERINFO.Svdate, TAXCATEGORY.Commodity_Code, Sum(VOUCHERDET.Pvalue*VOUCHERDET.Quantity) AS Quantity, Sum(VOUCHERDET.Taxabelamount) AS ItemTaxable, Sum(VOUCHERDET.taxamt1) AS Tax1, Sum(VOUCHERDET.taxamt2) AS Tax2, Sum(VOUCHERDET.Taxabelamount+taxamt1+taxamt2) AS VoucherNetAmt, ACCOUNT.Tin_number, VOUCHERDET.Rate_Unit AS Utype FROM (((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id) LEFT JOIN (VOUCHERDET LEFT JOIN DESCRIPTION ON VOUCHERDET.Des_ac_id = DESCRIPTION.Des_id) ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN TAXCATEGORY ON VOUCHERDET.Category_Id = TAXCATEGORY.Category_Id WHERE (((VOUCHERTYPE.Short)='RET')) GROUP BY VOUCHERINFO.Vnumber, VOUCHERINFO.Svnum, VOUCHERINFO.Svdate, TAXCATEGORY.Commodity_Code, ACCOUNT.Tin_number, VOUCHERDET.Rate_Unit, VOUCHERINFO.Vdate, VOUCHERINFO.Vnumber, VOUCHERTYPE.A";
                Str1 += " HAVING (((Sum(VOUCHERDET.taxamt1))<>0) AND ((Sum(VOUCHERDET.taxamt2))<>0) AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) ORDER BY VOUCHERINFO.Vdate, VOUCHERINFO.Vnumber";
            }           

            DataTable dtGrid5 = new DataTable();
            Database.GetSqlData(Str1, dtGrid5);
            for (int i = 0; i < dtGrid5.Rows.Count; i++)
            {
                ansGridView5.Rows.Add();
                ansGridView5.Rows[i].Cells["invno4"].Value = dtGrid5.Rows[i]["Vnumber"];
                ansGridView5.Rows[i].Cells["invdt4"].Value = dtGrid5.Rows[i]["dt"];
                ansGridView5.Rows[i].Cells["comm_code4"].Value = dtGrid5.Rows[i]["Commodity_Code"];
                ansGridView5.Rows[i].Cells["comm_qty4"].Value = dtGrid5.Rows[i]["Quantity"];
                ansGridView5.Rows[i].Cells["tax_goods4"].Value = dtGrid5.Rows[i]["ItemTaxable"];
                ansGridView5.Rows[i].Cells["vat4"].Value = dtGrid5.Rows[i]["Tax1"];
                ansGridView5.Rows[i].Cells["sat4"].Value = dtGrid5.Rows[i]["Tax2"];
                ansGridView5.Rows[i].Cells["totinval4"].Value = dtGrid5.Rows[i]["VoucherNetAmt"];
                ansGridView5.Rows[i].Cells["ven_cust_tin4"].Value = dtGrid5.Rows[i]["Tin_number"];
                ansGridView5.Rows[i].Cells["unt4"].Value = dtGrid5.Rows[i]["Utype"];
                ansGridView5.Rows[i].Cells["type3"].Value = 1;
            }
        }

        private void populateGrid6()
        {
            ansGridView6.Rows.Clear();
            String Str1 = "";

            if (Database.IsKacha == false)
            {
                Str1 = "SELECT res.Commodity_Code as Commodity_Code, Sum(res.rate) AS Tax_Rate, Sum(res.Taxabelamount) AS Taxabelamount, Sum(res.Tax) AS Tax, res.VorAT as VorAT FROM (SELECT TAXCATEGORY.Commodity_Code, VOUCHERDET.rate1 as rate, Sum(" + access_sql.fnstring("[Short]='SLT' Or [Short]='SLB' Or [Short]='SLC'", "[Taxabelamount]", "[Taxabelamount]*-1") + ") AS Taxabelamount, Sum(" + access_sql.fnstring("[Short]='SLT' Or [Short]='SLB' Or [Short]='SLC'", "[taxamt1]", "[taxamt1]*-1") + ") AS Tax, 'V' AS VorAT FROM (((VOUCHERINFO LEFT JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN DESCRIPTION ON VOUCHERDET.Des_ac_id = DESCRIPTION.Des_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN TAXCATEGORY ON VOUCHERDET.Category_Id = TAXCATEGORY.Category_Id WHERE (((VOUCHERTYPE.Short)='SLB' Or (VOUCHERTYPE.Short)='SLC' Or (VOUCHERTYPE.Short)='SLT' Or (VOUCHERTYPE.Short)='REC' Or (VOUCHERTYPE.Short)='REB' Or (VOUCHERTYPE.Short)='RET') AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ")) GROUP BY TAXCATEGORY.Commodity_Code, VOUCHERDET.rate1, VOUCHERTYPE.A HAVING (((VOUCHERDET.rate1)<>0) AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) Union All";
                Str1 += " SELECT TAXCATEGORY.Commodity_Code, VOUCHERDET.rate2 AS rate, Sum(" + access_sql.fnstring("[Short]='SLT' Or [Short]='SLB' Or [Short]='SLC'", "[Taxabelamount]", "[Taxabelamount]*-1") + ") AS Taxabelamount, Sum(" + access_sql.fnstring("[Short]='SLT' Or [Short]='SLB' Or [Short]='SLC'", "[taxamt2]", "[taxamt2]*-1") + ") AS Tax, 'AT' AS VorAT FROM (((VOUCHERINFO LEFT JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN DESCRIPTION ON VOUCHERDET.Des_ac_id = DESCRIPTION.Des_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN TAXCATEGORY ON VOUCHERDET.Category_Id = TAXCATEGORY.Category_Id WHERE (((VOUCHERTYPE.Short)='SLB' Or (VOUCHERTYPE.Short)='SLC' Or (VOUCHERTYPE.Short)='SLT' Or (VOUCHERTYPE.Short)='REC' Or (VOUCHERTYPE.Short)='REB' Or (VOUCHERTYPE.Short)='RET') AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ")) GROUP BY TAXCATEGORY.Commodity_Code, VOUCHERDET.rate2, VOUCHERTYPE.A HAVING (((VOUCHERDET.rate2)<>0) AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")))  AS res GROUP BY res.Commodity_Code, res.VorAT ORDER BY res.Commodity_Code, res.VorAT DESC;";
            }
            
            DataTable dtGrid6 = new DataTable();
            Database.GetSqlData(Str1, dtGrid6);
            for (int i = 0; i < dtGrid6.Rows.Count; i++)
            {
                ansGridView6.Rows.Add();
                ansGridView6.Rows[i].Cells["v_nv_at"].Value = dtGrid6.Rows[i]["VorAT"];
                ansGridView6.Rows[i].Cells["comm_code5"].Value = dtGrid6.Rows[i]["Commodity_Code"];
                ansGridView6.Rows[i].Cells["tax_rate"].Value = funs.DecimalPoint(dtGrid6.Rows[i]["Tax_Rate"]);
                ansGridView6.Rows[i].Cells["sale_amt"].Value = funs.DecimalPoint(dtGrid6.Rows[i]["Taxabelamount"]);
                ansGridView6.Rows[i].Cells["sale_tax_amt"].Value = funs.DecimalPoint(dtGrid6.Rows[i]["Tax"]);
            }
        }

        private void populateGrid7()
        {
            ansGridView7.Rows.Clear();
            String Str1 = "";

            if (Database.IsKacha == false)
            {
                Str1 = "SELECT res.Commodity_Code as Commodity_Code, Sum(res.rate) AS Tax_Rate, Sum(res.Taxabelamount) AS Taxabelamount, Sum(res.Tax) AS Tax, res.VorAT as VorAT FROM (SELECT TAXCATEGORY.Commodity_Code, VOUCHERDET.rate1 as rate, Sum(" + access_sql.fnstring("[Short]='PRU'", "[Taxabelamount]", "[Taxabelamount]*-1") + ") AS Taxabelamount, Sum(" + access_sql.fnstring("[Short]='PRU'", "[taxamt1]", "[taxamt1]*-1") + ") AS Tax, 'V' AS VorAT FROM (((VOUCHERINFO LEFT JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN DESCRIPTION ON VOUCHERDET.Des_ac_id = DESCRIPTION.Des_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN TAXCATEGORY ON VOUCHERDET.Category_Id = TAXCATEGORY.Category_Id WHERE (((VOUCHERTYPE.Short)='PRU' Or (VOUCHERTYPE.Short)='RPU') AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ")) GROUP BY TAXCATEGORY.Commodity_Code, VOUCHERDET.rate1, VOUCHERTYPE.A HAVING (((VOUCHERDET.rate1)<>0) AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + "))";
                Str1 += " Union All SELECT TAXCATEGORY.Commodity_Code, VOUCHERDET.rate2 AS rate, Sum(" + access_sql.fnstring("[Short]='PRU'", "[Taxabelamount]", "[Taxabelamount]*-1") + ") AS Taxabelamount, Sum(" + access_sql.fnstring("[Short]='PRU'", "[taxamt2]", "[taxamt2]*-1") + ") AS Tax, 'AT' AS VorAT FROM (((VOUCHERINFO LEFT JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN DESCRIPTION ON VOUCHERDET.Des_ac_id = DESCRIPTION.Des_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN TAXCATEGORY ON VOUCHERDET.Category_Id = TAXCATEGORY.Category_Id WHERE (((VOUCHERTYPE.Short)='PRU' Or (VOUCHERTYPE.Short)='RPU') AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ")) GROUP BY TAXCATEGORY.Commodity_Code, VOUCHERDET.rate2, VOUCHERTYPE.A HAVING (((VOUCHERDET.rate2)<>0) AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")))  AS res GROUP BY res.Commodity_Code, res.VorAT ORDER BY res.Commodity_Code, res.VorAT DESC";
            }           

            DataTable dtGrid7 = new DataTable();
            Database.GetSqlData(Str1, dtGrid7);
            for (int i = 0; i < dtGrid7.Rows.Count; i++)
            {
                ansGridView7.Rows.Add();
                ansGridView7.Rows[i].Cells["v_nv_at2"].Value = dtGrid7.Rows[i]["VorAT"];
                ansGridView7.Rows[i].Cells["comm_code6"].Value = dtGrid7.Rows[i]["Commodity_Code"];
                ansGridView7.Rows[i].Cells["tax_rate2"].Value = funs.DecimalPoint(dtGrid7.Rows[i]["Tax_Rate"]);
                ansGridView7.Rows[i].Cells["sale_amt2"].Value = funs.DecimalPoint(dtGrid7.Rows[i]["Taxabelamount"]);
                ansGridView7.Rows[i].Cells["sale_tax_amt2"].Value = funs.DecimalPoint(dtGrid7.Rows[i]["Tax"]);
            }
        }

        private void populateGrid8()
        {
            ansGridView8.Rows.Clear();
            String Str1 = "";

            if (Database.IsKacha == false)
            {
                Str1 = "SELECT VOUCHERINFO.Vnumber, VOUCHERINFO.Vdate, VOUCHERINFO.Svnum,  " + access_sql.fnDatFormatting("VOUCHERINFO.Svdate", Database.dformat) + " AS Svdate, TAXCATEGORY.Commodity_Code, TAXCATEGORY.Category_Name, Sum([Voucherdet].[Pvalue]*[Quantity]) AS Quantity, Sum(VOUCHERDET.Taxabelamount) AS ItemTaxable, Sum(VOUCHERDET.taxamt3) AS Tax1, Sum(VOUCHERDET.taxamt4) AS Tax2, VOUCHERINFO.Totalamount AS VoucherNetAmt, ACCOUNT.Tin_number, VOUCHERDET.Rate_Unit AS Utype, ACCOUNT.Name, ACCOUNT.Address1, ACCOUNT.Address2, VOUCHERINFO.Formno";
                Str1 += " FROM (((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id) LEFT JOIN (VOUCHERDET LEFT JOIN DESCRIPTION ON VOUCHERDET.Des_ac_id = DESCRIPTION.Des_id) ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN TAXCATEGORY ON VOUCHERDET.Category_Id = TAXCATEGORY.Category_Id WHERE (((VOUCHERTYPE.Short)='PRX')) GROUP BY VOUCHERINFO.Vnumber, VOUCHERINFO.Vdate, VOUCHERINFO.Svnum, TAXCATEGORY.Commodity_Code, TAXCATEGORY.Category_Name, VOUCHERINFO.Totalamount, ACCOUNT.Tin_number, VOUCHERDET.Rate_Unit, ACCOUNT.Name, ACCOUNT.Address1, ACCOUNT.Address2, VOUCHERINFO.Formno, VOUCHERINFO.Svdate, VOUCHERINFO.Vnumber, VOUCHERTYPE.A HAVING (((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") AND ((Sum(VOUCHERDET.taxamt3))<>0) AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) ORDER BY VOUCHERINFO.Vdate, VOUCHERINFO.Vnumber";
            }

            DataTable dtGrid8 = new DataTable();
            Database.GetSqlData(Str1, dtGrid8);
            for (int i = 0; i < dtGrid8.Rows.Count; i++)
            {
                ansGridView8.Rows.Add();
                ansGridView8.Rows[i].Cells["sell_tin"].Value = dtGrid8.Rows[i]["Tin_number"];
                ansGridView8.Rows[i].Cells["sell_nm"].Value = dtGrid8.Rows[i]["Name"];
                ansGridView8.Rows[i].Cells["sell_addr"].Value = dtGrid8.Rows[i]["Address1"];
                ansGridView8.Rows[i].Cells["sell_state"].Value = dtGrid8.Rows[i]["Address2"];
                ansGridView8.Rows[i].Cells["frm38no"].Value = dtGrid8.Rows[i]["Formno"];
                ansGridView8.Rows[i].Cells["invno5"].Value = dtGrid8.Rows[i]["Svnum"];
                ansGridView8.Rows[i].Cells["invdt5"].Value =DateTime.Parse( dtGrid8.Rows[i]["Svdate"].ToString()).ToString("dd/MM/yyyy");
                ansGridView8.Rows[i].Cells["pur_ord_no"].Value = "0";
                ansGridView8.Rows[i].Cells["pur_ord_dt"].Value =DateTime.Parse( dtGrid8.Rows[i]["Svdate"].ToString()).ToString("dd/MM/yyyy");
                ansGridView8.Rows[i].Cells["comm_nm"].Value = dtGrid8.Rows[i]["Commodity_Code"];
                ansGridView8.Rows[i].Cells["comm_qty5"].Value = dtGrid8.Rows[i]["Quantity"];
                ansGridView8.Rows[i].Cells["unt5"].Value = dtGrid8.Rows[i]["Utype"];
                ansGridView8.Rows[i].Cells["tax_Amt2"].Value = dtGrid8.Rows[i]["ItemTaxable"];
                ansGridView8.Rows[i].Cells["tax"].Value = double.Parse(dtGrid8.Rows[i]["Tax1"].ToString()) + double.Parse(dtGrid8.Rows[i]["Tax2"].ToString());
                ansGridView8.Rows[i].Cells["tot_inv_val"].Value = dtGrid8.Rows[i]["VoucherNetAmt"];
                ansGridView8.Rows[i].Cells["resale_man"].Value = "1";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            populateGrid4();
            tabControl1.SelectedIndex = 5;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            populateGrid5();
            tabControl1.SelectedIndex = 6;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            populateGrid6();
            tabControl1.SelectedIndex = 7;
        }

        private void button22_Click(object sender, EventArgs e)
        {
            populateGrid7();
            tabControl1.SelectedIndex = 8;
        }

        private void button26_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 7;
        }

        private void button23_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 6;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 5;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 4;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void button25_Click(object sender, EventArgs e)
        {
            populateGrid8();
            tabControl1.SelectedIndex = 9;
        }

        private void button29_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 7;
        }

        private void button28_Click(object sender, EventArgs e)
        {
            wb = (Excel.Workbook)apl.Workbooks.Open(Application.StartupPath + "\\efile\\Draft.xls", true, true, misValue, null, null, false, misValue, null, false, false, misValue, misValue, misValue, false);
            Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets["Form24_Annexure_A"];

            for (int i = 1; i < ansGridView2.Rows.Count; i++)
            {
                ws.Cells[i + 1, 1] = ansGridView1.Rows[0].Cells[1].Value;
                ws.Cells[i + 1, 2] = ansGridView1.Rows[1].Cells[1].Value;
                ws.Cells[i + 1, 3] = ansGridView1.Rows[2].Cells[1].Value;
                ws.Cells[i + 1, 4] = ansGridView1.Rows[3].Cells[1].Value;
                ws.Cells[i + 1, 5] = ansGridView1.Rows[4].Cells[1].Value;
                ws.Cells[i + 1, 6] = ansGridView2.Rows[i - 1].Cells[0].Value;
                ws.Cells[i + 1, 7] = ansGridView2.Rows[i - 1].Cells[1].Value;
                ws.Cells[i + 1, 8] = ansGridView2.Rows[i - 1].Cells[2].Value;
                ws.Cells[i + 1, 9] = ansGridView2.Rows[i - 1].Cells[3].Value;
                ws.Cells[i + 1, 10] = ansGridView2.Rows[i - 1].Cells[4].Value;
                ws.Cells[i + 1, 11] = ansGridView2.Rows[i - 1].Cells[5].Value;
                ws.Cells[i + 1, 12] = ansGridView2.Rows[i - 1].Cells[6].Value;
                ws.Cells[i + 1, 13] = ansGridView2.Rows[i - 1].Cells[7].Value;
                ws.Cells[i + 1, 14] = ansGridView2.Rows[i - 1].Cells[8].Value;
                ws.Cells[i + 1, 15] = ansGridView2.Rows[i - 1].Cells[9].Value;
                ws.Cells[i + 1, 16] = ansGridView2.Rows[i - 1].Cells[10].Value;
            }

            ws = (Excel.Worksheet)wb.Worksheets["Form24_Annexure_A1"];

            for (int i = 1; i < ansGridView3.Rows.Count; i++)
            {
                ws.Cells[i + 1, 1] = ansGridView1.Rows[0].Cells[1].Value;
                ws.Cells[i + 1, 2] = ansGridView1.Rows[1].Cells[1].Value;
                ws.Cells[i + 1, 3] = ansGridView1.Rows[2].Cells[1].Value;
                ws.Cells[i + 1, 4] = ansGridView1.Rows[3].Cells[1].Value;
                ws.Cells[i + 1, 5] = ansGridView1.Rows[4].Cells[1].Value;
                ws.Cells[i + 1, 6] = ansGridView3.Rows[i - 1].Cells[0].Value;
                ws.Cells[i + 1, 7] = ansGridView3.Rows[i - 1].Cells[1].Value;
                ws.Cells[i + 1, 8] = ansGridView3.Rows[i - 1].Cells[2].Value;
                ws.Cells[i + 1, 9] = ansGridView3.Rows[i - 1].Cells[3].Value;
                ws.Cells[i + 1, 10] = ansGridView3.Rows[i - 1].Cells[4].Value;
                ws.Cells[i + 1, 11] = ansGridView3.Rows[i - 1].Cells[5].Value;
                ws.Cells[i + 1, 12] = ansGridView3.Rows[i - 1].Cells[6].Value;
                ws.Cells[i + 1, 13] = ansGridView3.Rows[i - 1].Cells[7].Value;
                ws.Cells[i + 1, 14] = ansGridView3.Rows[i - 1].Cells[8].Value;
                ws.Cells[i + 1, 15] = ansGridView3.Rows[i - 1].Cells[9].Value;
                ws.Cells[i + 1, 16] = ansGridView3.Rows[i - 1].Cells[10].Value;
            }

            ws = (Excel.Worksheet)wb.Worksheets["Form24_Annexure_B"];

            for (int i = 1; i < ansGridView4.Rows.Count; i++)
            {
                ws.Cells[i + 1, 1] = ansGridView1.Rows[0].Cells[1].Value;
                ws.Cells[i + 1, 2] = ansGridView1.Rows[1].Cells[1].Value;
                ws.Cells[i + 1, 3] = ansGridView1.Rows[2].Cells[1].Value;
                ws.Cells[i + 1, 4] = ansGridView1.Rows[3].Cells[1].Value;
                ws.Cells[i + 1, 5] = ansGridView1.Rows[4].Cells[1].Value;
                ws.Cells[i + 1, 6] = ansGridView4.Rows[i - 1].Cells[0].Value;
                ws.Cells[i + 1, 7] = ansGridView4.Rows[i - 1].Cells[1].Value;
                ws.Cells[i + 1, 8] = ansGridView4.Rows[i - 1].Cells[2].Value;
                ws.Cells[i + 1, 9] = ansGridView4.Rows[i - 1].Cells[3].Value;
                ws.Cells[i + 1, 10] = ansGridView4.Rows[i - 1].Cells[4].Value;
                ws.Cells[i + 1, 11] = ansGridView4.Rows[i - 1].Cells[5].Value;
                ws.Cells[i + 1, 12] = ansGridView4.Rows[i - 1].Cells[6].Value;
                ws.Cells[i + 1, 13] = ansGridView4.Rows[i - 1].Cells[7].Value;
                ws.Cells[i + 1, 14] = ansGridView4.Rows[i - 1].Cells[8].Value;
                ws.Cells[i + 1, 15] = ansGridView4.Rows[i - 1].Cells[9].Value;
                ws.Cells[i + 1, 16] = ansGridView4.Rows[i - 1].Cells[10].Value;
            }

            ws = (Excel.Worksheet)wb.Worksheets["Form24_Annexure_B1"];

            for (int i = 1; i < ansGridView5.Rows.Count; i++)
            {
                ws.Cells[i + 1, 1] = ansGridView1.Rows[0].Cells[1].Value;
                ws.Cells[i + 1, 2] = ansGridView1.Rows[1].Cells[1].Value;
                ws.Cells[i + 1, 3] = ansGridView1.Rows[2].Cells[1].Value;
                ws.Cells[i + 1, 4] = ansGridView1.Rows[3].Cells[1].Value;
                ws.Cells[i + 1, 5] = ansGridView1.Rows[4].Cells[1].Value;
                ws.Cells[i + 1, 6] = ansGridView5.Rows[i - 1].Cells[0].Value;
                ws.Cells[i + 1, 7] = ansGridView5.Rows[i - 1].Cells[1].Value;
                ws.Cells[i + 1, 8] = ansGridView5.Rows[i - 1].Cells[2].Value;
                ws.Cells[i + 1, 9] = ansGridView5.Rows[i - 1].Cells[3].Value;
                ws.Cells[i + 1, 10] = ansGridView5.Rows[i - 1].Cells[4].Value;
                ws.Cells[i + 1, 11] = ansGridView5.Rows[i - 1].Cells[5].Value;
                ws.Cells[i + 1, 12] = ansGridView5.Rows[i - 1].Cells[6].Value;
                ws.Cells[i + 1, 13] = ansGridView5.Rows[i - 1].Cells[7].Value;
                ws.Cells[i + 1, 14] = ansGridView5.Rows[i - 1].Cells[8].Value;
                ws.Cells[i + 1, 15] = ansGridView5.Rows[i - 1].Cells[9].Value;
                ws.Cells[i + 1, 16] = ansGridView5.Rows[i - 1].Cells[10].Value;
            }

            ws = (Excel.Worksheet)wb.Worksheets["Tax_Detail_Sale"];

            for (int i = 1; i < ansGridView6.Rows.Count; i++)
            {
                ws.Cells[i + 1, 1] = ansGridView1.Rows[0].Cells[1].Value;
                ws.Cells[i + 1, 2] = ansGridView1.Rows[1].Cells[1].Value;
                ws.Cells[i + 1, 3] = ansGridView1.Rows[2].Cells[1].Value;
                ws.Cells[i + 1, 4] = ansGridView1.Rows[3].Cells[1].Value;
                ws.Cells[i + 1, 5] = ansGridView1.Rows[4].Cells[1].Value;
                ws.Cells[i + 1, 6] = ansGridView6.Rows[i - 1].Cells[0].Value;
                ws.Cells[i + 1, 7] = ansGridView6.Rows[i - 1].Cells[1].Value;
                ws.Cells[i + 1, 8] = ansGridView6.Rows[i - 1].Cells[2].Value;
                ws.Cells[i + 1, 9] = ansGridView6.Rows[i - 1].Cells[3].Value;
                ws.Cells[i + 1, 10] = ansGridView6.Rows[i - 1].Cells[4].Value;
            }

            ws = (Excel.Worksheet)wb.Worksheets["Tax_Detail_Purchase"];

            for (int i = 1; i < ansGridView7.Rows.Count; i++)
            {
                ws.Cells[i + 1, 1] = ansGridView1.Rows[0].Cells[1].Value;
                ws.Cells[i + 1, 2] = ansGridView1.Rows[1].Cells[1].Value;
                ws.Cells[i + 1, 3] = ansGridView1.Rows[2].Cells[1].Value;
                ws.Cells[i + 1, 4] = ansGridView1.Rows[3].Cells[1].Value;
                ws.Cells[i + 1, 5] = ansGridView1.Rows[4].Cells[1].Value;
                ws.Cells[i + 1, 6] = ansGridView7.Rows[i - 1].Cells[0].Value;
                ws.Cells[i + 1, 7] = ansGridView7.Rows[i - 1].Cells[1].Value;
                ws.Cells[i + 1, 8] = ansGridView7.Rows[i - 1].Cells[2].Value;
                ws.Cells[i + 1, 9] = ansGridView7.Rows[i - 1].Cells[3].Value;
                ws.Cells[i + 1, 10] = ansGridView7.Rows[i - 1].Cells[4].Value;
            }

            ws = (Excel.Worksheet)wb.Worksheets["Form24_Annexure_C"];

            for (int i = 1; i < ansGridView8.Rows.Count; i++)
            {
                ws.Cells[i + 1, 1] = ansGridView1.Rows[0].Cells[1].Value;
                ws.Cells[i + 1, 2] = ansGridView1.Rows[1].Cells[1].Value;
                ws.Cells[i + 1, 3] = ansGridView1.Rows[2].Cells[1].Value;
                ws.Cells[i + 1, 4] = ansGridView1.Rows[3].Cells[1].Value;
                ws.Cells[i + 1, 5] = ansGridView1.Rows[4].Cells[1].Value;
                ws.Cells[i + 1, 6] = ansGridView8.Rows[i - 1].Cells[0].Value;
                ws.Cells[i + 1, 7] = ansGridView8.Rows[i - 1].Cells[1].Value;
                ws.Cells[i + 1, 8] = ansGridView8.Rows[i - 1].Cells[2].Value;
                ws.Cells[i + 1, 9] = ansGridView8.Rows[i - 1].Cells[3].Value;
                ws.Cells[i + 1, 10] = ansGridView8.Rows[i - 1].Cells[4].Value;
                ws.Cells[i + 1, 11] = ansGridView8.Rows[i - 1].Cells[5].Value;
                ws.Cells[i + 1, 12] = ansGridView8.Rows[i - 1].Cells[6].Value;
                ws.Cells[i + 1, 13] = ansGridView8.Rows[i - 1].Cells[7].Value;
                ws.Cells[i + 1, 14] = ansGridView8.Rows[i - 1].Cells[8].Value;
                ws.Cells[i + 1, 15] = ansGridView8.Rows[i - 1].Cells[9].Value;
                ws.Cells[i + 1, 16] = ansGridView8.Rows[i - 1].Cells[10].Value;
                ws.Cells[i + 1, 17] = ansGridView8.Rows[i - 1].Cells[11].Value;
                ws.Cells[i + 1, 18] = ansGridView8.Rows[i - 1].Cells[12].Value;
                ws.Cells[i + 1, 19] = ansGridView8.Rows[i - 1].Cells[13].Value;
                ws.Cells[i + 1, 20] = ansGridView8.Rows[i - 1].Cells[14].Value;
                ws.Cells[i + 1, 21] = ansGridView8.Rows[i - 1].Cells[15].Value;
            }

            ws = (Excel.Worksheet)wb.Worksheets["Vat_Non_Vat"];
            for (int i = 1; i <= 41; i++)
            { 
                ws.Cells[i + 1, 1] = ansGridView1.Rows[0].Cells[1].Value;
                ws.Cells[i + 1, 2] = ansGridView1.Rows[1].Cells[1].Value;
                ws.Cells[i + 1, 3] = ansGridView1.Rows[2].Cells[1].Value;
                ws.Cells[i + 1, 4] = ansGridView1.Rows[3].Cells[1].Value;
                ws.Cells[i + 1, 5] = ansGridView1.Rows[4].Cells[1].Value;
            }

            ws.Cells[2, 10] = TurnOverZeroExcluding("PRO");
            ws.Cells[3, 10] = TurnOverZeroExcluding("PRU") - TurnOverZeroExcluding("RPU");
            ws.Cells[4, 10] = TurnOverZeroOnly("PRO") + TurnOverZeroOnly("PRU") - TurnOverZeroOnly("RPO") - TurnOverZeroOnly("RPU");
            ws.Cells[5, 10] = TurnOverZeroExcluding("PRX") + TurnOverZeroOnly("PRX") - TurnOverZeroOnly("RPX") - TurnOverZeroOnly("RPX");

            for (int i = 6; i <= 20; i++)
            {
                ws.Cells[i, 10] = 0;
            }

            ws.Cells[21, 10] = TurnOverZeroExcluding("PRX") + TurnOverZeroOnly("PRX") - TurnOverZeroExcluding("RPX") - TurnOverZeroOnly("RPX");
            ws.Cells[22, 10] = 0;
            ws.Cells[23, 10] = TurnOverZeroExcluding("SLT");
            ws.Cells[24, 10] = TurnOverZeroExcluding("SLB") + TurnOverZeroExcluding("SLC") - TurnOverZeroExcluding("REB") - TurnOverZeroExcluding("REC");
            ws.Cells[25, 10] = TurnOverZeroOnly("SLB") + TurnOverZeroOnly("SLC") + TurnOverZeroOnly("SLT") - TurnOverZeroOnly("REB") - TurnOverZeroOnly("REC") - TurnOverZeroOnly("RET");
           // ws.Cells[26, 10] = AgainstFormC("SLS") - AgainstFormC("RES");
            ws.Cells[27, 10] = WithoutFormC("SLS") - WithoutFormC("RES");

            for (int i = 28; i <= 42; i++)
            {
                ws.Cells[i, 10] = 0;
            }

            
            ws = (Excel.Worksheet)wb.Worksheets["Main_Form"];
            ws.Cells[2, 2] = ansGridView1.Rows[0].Cells[1].Value;
            ws.Cells[3, 2] = ansGridView1.Rows[1].Cells[1].Value;
            ws.Cells[4, 2] = ansGridView1.Rows[2].Cells[1].Value;
            ws.Cells[5, 2] = ansGridView1.Rows[3].Cells[1].Value;
            ws.Cells[6, 2] = ansGridView1.Rows[4].Cells[1].Value;
            ws.Cells[7, 2] = 0;
            ws.Cells[8, 2] = 0;
            ws.Cells[10, 2] = "=SUM(Tax_Detail_Purchase!J:J)";
            ws.Cells[11, 2] = "=SUM(Tax_Detail_Sale!J:J)";
            ws.Cells[12, 2] = 0;
            ws.Cells[13, 2] = 0;
            InputBox box = new InputBox("ITC Brought Forward From Previous Period-", "", false);
            box.ShowInTaskbar = false;
            box.ShowDialog(this);
            double val = 0;
            if (box.outStr != null && box.outStr != "")
            {
                val = double.Parse(box.outStr);
            }
            ws.Cells[15, 2] = funs.DecimalPoint(val);
            ws.Cells[17, 2] = "=SUM(Form24_Annexure_A!K:L)";
            ws.Cells[18, 2] = 0;
            ws.Cells[19, 2] = 0;
            ws.Cells[20, 2] = 0;
            ws.Cells[21, 2] = "=SUM(Form24_Annexure_A1!K:L)";
            ws.Cells[23, 2] = "=MIN((B10+B12+B13+B15+B22),(B11+B12+B13+B15+B22),(B10+B11+B12+B13))";
            ws.Cells[25, 2] = "=(B10+B15+B22)-(B23+B24)";
            apl.Visible = true;
        }

        private double TurnOverZeroExcluding(String shrt)
        {
            double TurnOver = 0;
            String str;

            str = "SELECT Sum(VOUCHERDET.Taxabelamount) AS TurnOver FROM (VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id WHERE (((VOUCHERTYPE.Short)='" + shrt + "') AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString("dd-MMM-yyyy") + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString("dd-MMM-yyyy") + access_sql.Hash + ") AND ((VOUCHERDET.rate1)<>0))";

            DataTable dtTurnOver = new DataTable();
            Database.GetSqlData(str, dtTurnOver);

            if (dtTurnOver.Rows[0][0].ToString() != "")
            {
                TurnOver = double.Parse(dtTurnOver.Rows[0][0].ToString());
            }
            return TurnOver;
        }

        private double TurnOverZeroOnly(String shrt)
        {
            double TurnOver = 0;
            String str;
          
            str = "SELECT Sum(VOUCHERDET.Taxabelamount) AS TurnOver FROM (VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id WHERE (((VOUCHERTYPE.Short)='" + shrt + "') AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString("dd-MMM-yyyy") + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString("dd-MMM-yyyy") + access_sql.Hash + ") AND ((VOUCHERDET.rate1)=0))";

            DataTable dtTurnOver = new DataTable();
            Database.GetSqlData(str, dtTurnOver);

            if (dtTurnOver.Rows[0][0].ToString() != "")
            {
                TurnOver = double.Parse(dtTurnOver.Rows[0][0].ToString());
            }
            return TurnOver;
        }

        private double TurnOverZeroIncluding(String shrt)
        {
            double TurnOver = 0;
            String str;
         
            str = "SELECT Sum(VOUCHERDET.Taxabelamount) AS TurnOver FROM (VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id WHERE (((VOUCHERTYPE.Short)='" + shrt + "') AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString("dd-MMM-yyyy") + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString("dd-MMM-yyyy") + access_sql.Hash + "))";

            DataTable dtTurnOver = new DataTable();
            Database.GetSqlData(str, dtTurnOver);

            if (dtTurnOver.Rows[0][0].ToString() != "")
            {
                TurnOver = double.Parse(dtTurnOver.Rows[0][0].ToString());
            }
            return TurnOver;
        }

        private double AgainstFormC(String shrt)
        {
            double TurnOver = 0;
            String str;                                      
          
            str = "SELECT Sum(ITEMTAX.[Taxable]) AS TurnOver FROM ((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN ITEMTAX ON (VOUCHERDET.Itemsr = ITEMTAX.Itemsr) AND (VOUCHERDET.Vi_id = ITEMTAX.Vi_id)";
            str += " WHERE (VOUCHERTYPE.Short='" + shrt + "' AND (VOUCHERINFO.Vdate>="+access_sql.Hash + dt1.ToString("dd-MMM-yyyy") + access_sql.Hash+" And VOUCHERINFO.Vdate<="+access_sql.Hash + dt2.ToString("dd-MMM-yyyy") + access_sql.Hash+") AND ITEMTAX.Tax_Rate<>0 AND ITEMTAX.Tax_Name Like '%Against Form-C%')";

            DataTable dtTurnOver = new DataTable();
            Database.GetSqlData(str, dtTurnOver);

            if (dtTurnOver.Rows[0][0].ToString() != "")
            {
                TurnOver = double.Parse(dtTurnOver.Rows[0][0].ToString());
            }
            return TurnOver;
        }

        private double WithoutFormC(String shrt)
        {
            double TurnOver = 0;
            String str;

            str = "SELECT Sum(VOUCHERDET.Taxabelamount) AS TurnOver FROM (VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id WHERE (((VOUCHERTYPE.Short)='SLS') AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString("dd-MMM-yyyy") + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString("dd-MMM-yyyy") + access_sql.Hash + "))";
            DataTable dtTurnOver = new DataTable();
            Database.GetSqlData(str, dtTurnOver);

            if (dtTurnOver.Rows[0][0].ToString() != "")
            {
                TurnOver = double.Parse(dtTurnOver.Rows[0][0].ToString());
            }
            return TurnOver;
        }

        private void frm_eFiling_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
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
        }
    }
}
