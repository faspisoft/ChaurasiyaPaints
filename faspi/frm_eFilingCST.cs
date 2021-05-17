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
    public partial class frm_eFilingCST : Form
    {
        static Object misValue = System.Reflection.Missing.Value;
        static Excel.Application apl = new Microsoft.Office.Interop.Excel.Application();
        Excel.Workbook wb;
        Excel.Worksheet ws;        
        DateTime dt1 = new DateTime();
        DateTime dt2 = new DateTime();

        public frm_eFilingCST()
        {
            InitializeComponent();
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

        private void button29_Click(object sender, EventArgs e)
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

        private void button4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void button27_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
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
                    dt1 = new DateTime(Database.stDate.Year + 1, 01, 01);
                    dt2 = new DateTime(Database.stDate.Year + 1, 03, 31);
                }
            }
        }

        private void populateGrid1()
        {
            ansGridView1.Rows.Clear();
            wb = (Excel.Workbook)apl.Workbooks.Open(Application.StartupPath + "\\efile\\DraftCST.xls", true, true, misValue, null, null, false, misValue, null, false, false, misValue, misValue, misValue, false);
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

            ansGridView1.Rows[0].Cells["ent"].Value = dtComp.Rows[0]["Tin_no"];
            ansGridView1.Rows[1].Cells["ent"].Value = dtComp.Rows[0]["Firm_Period_name"];
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
                Str1 = "SELECT VOUCHERINFO.Vnumber," + access_sql.fnDatFormatting("VOUCHERINFO.Vdate", Database.dformat) + " AS Vdate, VOUCHERINFO.Svnum, VOUCHERINFO.Svdate, TAXCATEGORY.Commodity_Code, TAXCATEGORY.Category_Name, Sum(" + access_sql.fnstring("[TaxSr]=1", "[Voucherdet].[Pvalue]*[Quantity]", "0") + ") AS Quantity, Sum(" + access_sql.fnstring("[TaxSr]=1", "[Taxabelamount]", "0") + ") AS ItemTaxable, Sum(" + access_sql.fnstring("[TaxSr]=1", "[Tax_Amount]", "0") + ") AS Tax1, Sum(" + access_sql.fnstring("[TaxSr]=2", "[Tax_Amount]", "0") + ") AS Tax2, Sum(" + access_sql.fnstring("[TaxSr]=1", "[Taxabelamount]", "0") + ")+Sum(" + access_sql.fnstring("[TaxSr]=1", "[Tax_Amount]", "0") + ")+Sum(" + access_sql.fnstring("[TaxSr]=2", "[Tax_Amount]", "0") + ") AS VoucherNetAmt, ACCOUNT.Tin_number, ACCOUNT.Name, ACCOUNT.Address1, ACCOUNT.Address2, VOUCHERINFO.Formno";
                Str1 += " FROM ((((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN ACCOUNT ON VOUCHERINFO.Ac_id = ACCOUNT.Ac_id) LEFT JOIN (VOUCHERDET LEFT JOIN DESCRIPTION ON VOUCHERDET.Des_ac_id = DESCRIPTION.Des_id)  ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN ITEMTAX ON (VOUCHERDET.Vi_id = ITEMTAX.Vi_id) AND (VOUCHERDET.Itemsr = ITEMTAX.Itemsr)) LEFT JOIN TAXCATEGORY ON VOUCHERDET.Category_Id = TAXCATEGORY.Category_Id WHERE (((VOUCHERTYPE.Short)='SLS'))";
                Str1 += " GROUP BY VOUCHERINFO.Vnumber, VOUCHERINFO.Svnum, VOUCHERINFO.Svdate, TAXCATEGORY.Commodity_Code, TAXCATEGORY.Category_Name, ACCOUNT.Tin_number, ACCOUNT.Name, ACCOUNT.Address1, ACCOUNT.Address2, VOUCHERINFO.Formno, VOUCHERINFO.Vdate, VOUCHERINFO.Vnumber, VOUCHERTYPE.A HAVING (((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ") AND ((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) ORDER BY VOUCHERINFO.Vdate, VOUCHERINFO.Vnumber";
            }

            DataTable dtGrid2 = new DataTable();
            Database.GetSqlData(Str1, dtGrid2);
            for (int i = 0; i < dtGrid2.Rows.Count; i++)
            {
                ansGridView2.Rows.Add();
                ansGridView2.Rows[i].Cells["pur_tin"].Value = dtGrid2.Rows[i]["Tin_number"];
                ansGridView2.Rows[i].Cells["pur_nm"].Value = dtGrid2.Rows[i]["Name"];
                ansGridView2.Rows[i].Cells["pur_Addr"].Value = dtGrid2.Rows[i]["Address1"].ToString() + dtGrid2.Rows[i]["Address2"].ToString();
                ansGridView2.Rows[i].Cells["invno"].Value = dtGrid2.Rows[i]["Vnumber"];
                ansGridView2.Rows[i].Cells["invdt"].Value = dtGrid2.Rows[i]["Vdate"];
                ansGridView2.Rows[i].Cells["sale_val_goods"].Value = dtGrid2.Rows[i]["ItemTaxable"];
                ansGridView2.Rows[i].Cells["tax"].Value = double.Parse(dtGrid2.Rows[i]["Tax1"].ToString()) + double.Parse(dtGrid2.Rows[i]["Tax2"].ToString());
                ansGridView2.Rows[i].Cells["invamt"].Value = dtGrid2.Rows[i]["VoucherNetAmt"];
                ansGridView2.Rows[i].Cells["comm_code"].Value = dtGrid2.Rows[i]["Commodity_Code"];
                ansGridView2.Rows[i].Cells["formcno"].Value = 0;
            }
        }

        private void populateGrid3()
        {
            ansGridView3.Rows.Clear();
            String Str1 = "";

            if (Database.IsKacha == false)
            {
                Str1 = "SELECT TAXCATEGORY.Commodity_Code, ITEMTAX.Tax_Rate, Sum(" + access_sql.fnstring("[Short]='SLS'", "[Taxable]", "[Taxable]*-1") + ") AS Taxabelamount, Sum(" + access_sql.fnstring("[Short]='SLS'", "[Tax_Amount]", "[Tax_Amount]*-1") + ") AS Tax, 'VAT' AS VorAT FROM ((((VOUCHERINFO LEFT JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN ITEMTAX ON (VOUCHERDET.Itemsr = ITEMTAX.Itemsr) AND (VOUCHERDET.Vi_id = ITEMTAX.Vi_id)) LEFT JOIN DESCRIPTION ON VOUCHERDET.Des_ac_id = DESCRIPTION.Des_id) LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN TAXCATEGORY ON VOUCHERDET.Category_Id = TAXCATEGORY.Category_Id";
                Str1 += " WHERE (((ITEMTAX.Tax_Rate)<>0) AND ((VOUCHERTYPE.Short)='SLS' Or (VOUCHERTYPE.Short)='RES') AND ((ITEMTAX.Tax_Amount)<>0) AND ((VOUCHERINFO.Vdate)>=" + access_sql.Hash + dt1.ToString(Database.dformat) + access_sql.Hash + " And (VOUCHERINFO.Vdate)<=" + access_sql.Hash + dt2.ToString(Database.dformat) + access_sql.Hash + ")) GROUP BY TAXCATEGORY.Commodity_Code, ITEMTAX.Tax_Rate, VOUCHERTYPE.A";
                Str1 += " HAVING (((VOUCHERTYPE.A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) ORDER BY TAXCATEGORY.Commodity_Code DESC";
            }
            
            DataTable dtGrid3 = new DataTable();
            Database.GetSqlData(Str1, dtGrid3);
            for (int i = 0; i < dtGrid3.Rows.Count; i++)
            {
                ansGridView3.Rows.Add();
                ansGridView3.Rows[i].Cells["v_nv_at"].Value = dtGrid3.Rows[i]["VorAT"];
                ansGridView3.Rows[i].Cells["comm_code2"].Value = dtGrid3.Rows[i]["Commodity_Code"];
                ansGridView3.Rows[i].Cells["tax_rate"].Value = funs.DecimalPoint(dtGrid3.Rows[i]["Tax_Rate"]);
                ansGridView3.Rows[i].Cells["sale_amt"].Value = funs.DecimalPoint(dtGrid3.Rows[i]["Taxabelamount"]);
                ansGridView3.Rows[i].Cells["sale_tax_amt"].Value = funs.DecimalPoint(dtGrid3.Rows[i]["Tax"]);
            }
        }

        private void button28_Click(object sender, EventArgs e)
        {
            wb = (Excel.Workbook)apl.Workbooks.Open(Application.StartupPath + "\\efile\\DraftCST.xls", true, true, misValue, null, null, false, misValue, null, false, false, misValue, misValue, misValue, false);
            Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets["FormCST_ListofInterstateSales"];

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
            }

            ws = (Excel.Worksheet)wb.Worksheets["FormCSTTurnOver"];

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
            }

            
            ws = (Excel.Worksheet)wb.Worksheets["FormCSTMainForm"];
            ws.Cells[2, 2] = ansGridView1.Rows[0].Cells[1].Value;
            ws.Cells[3, 2] = ansGridView1.Rows[1].Cells[1].Value;
            ws.Cells[4, 2] = ansGridView1.Rows[2].Cells[1].Value;
            ws.Cells[5, 2] = ansGridView1.Rows[3].Cells[1].Value;
            ws.Cells[6, 2] = ansGridView1.Rows[4].Cells[1].Value;
            ws.Cells[7, 2] = "NA";
            ws.Cells[8, 2] = TurnOverZeroIncluding("PRX") - TurnOverZeroIncluding("RPX");
            ws.Cells[9, 2] = 0;
            ws.Cells[10, 2] = TurnOverZeroIncluding("PRO") - TurnOverZeroIncluding("RPO") + TurnOverZeroIncluding("PRU") - TurnOverZeroIncluding("RPU");
            ws.Cells[11, 2] = 0;
            ws.Cells[12, 2] = 0;
            ws.Cells[13, 2] = 0;
            ws.Cells[14, 2] = TurnOverZeroIncluding("SLT") - TurnOverZeroIncluding("RET") + TurnOverZeroIncluding("SLS") - TurnOverZeroIncluding("RES") + TurnOverZeroIncluding("SLB") - TurnOverZeroIncluding("REB") + TurnOverZeroIncluding("SLC") - TurnOverZeroIncluding("REC");
            ws.Cells[15, 2] = 0;
            ws.Cells[16, 2] = 0;
            ws.Cells[17, 2] = 0;
            ws.Cells[18, 2] = 0;
            ws.Cells[19, 2] = TurnOverZeroIncluding("SLT") - TurnOverZeroIncluding("RET") + TurnOverZeroIncluding("SLB") - TurnOverZeroIncluding("REB") + TurnOverZeroIncluding("SLC") - TurnOverZeroIncluding("REC");
            ws.Cells[20, 2] = TurnOverZeroIncluding("RES");
            ws.Cells[21, 2] = 0;
            ws.Cells[22, 2] = "=b19+b20+b21";
            ws.Cells[23, 2] = "=b14-b19";
            ws.Cells[24, 2] = 0;
            ws.Cells[25, 2] = 0;
            ws.Cells[26, 2] = 0;
            ws.Cells[27, 2] = 0;
            ws.Cells[28, 2] = "=b23";
            InputBox box = new InputBox("ITC Adjustment-", "", false);
            box.ShowInTaskbar = false;
            box.ShowDialog(this);
            double val = 0;
            if (box.outStr != null && box.outStr != "")
            {
                val = double.Parse(box.outStr);
            }
            ws.Cells[29, 2] = val;
            ws.Cells[30, 2] = "=" + CSTTax("SLS") + "- b29";

            apl.Visible = true;
        }

        private double CSTTax(String shrt)
        {
            double Tax = 0;
            String str;
           
            str = "SELECT Sum([ITEMTAX].[Tax_Amount]) AS Tax FROM ((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN ITEMTAX ON (VOUCHERDET.Vi_id = ITEMTAX.Vi_id) AND (VOUCHERDET.Itemsr = ITEMTAX.Itemsr)";
            str += " WHERE (((VOUCHERTYPE.Short)='" + shrt + "') AND ((VOUCHERINFO.Vdate)>="+access_sql.Hash + dt1.ToString("dd-MMM-yyyy") + access_sql.Hash+" And (VOUCHERINFO.Vdate)<="+access_sql.Hash + dt2.ToString("dd-MMM-yyyy") + access_sql.Hash+") AND ((ITEMTAX.Tax_Rate)<>0))";

            DataTable dtTax = new DataTable();
            Database.GetSqlData(str, dtTax);

            if (dtTax.Rows[0][0].ToString() != "")
            {
                Tax = double.Parse(dtTax.Rows[0][0].ToString());
            }
            return Tax;
        }

        private double TurnOverZeroExcluding(String shrt)
        {
            double TurnOver = 0;
            String str;
            
            str = "SELECT Sum(" + access_sql.fnstring("[TaxSr]=1", "[Taxable]", "0") + ") AS TurnOver FROM ((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN ITEMTAX ON (VOUCHERDET.Itemsr = ITEMTAX.Itemsr) AND (VOUCHERDET.Vi_id = ITEMTAX.Vi_id)";
            str += " WHERE (((VOUCHERTYPE.Short)='" + shrt + "') AND ((VOUCHERINFO.Vdate)>="+access_sql.Hash + dt1.ToString("dd-MMM-yyyy") + access_sql.Hash+" And (VOUCHERINFO.Vdate)<="+access_sql.Hash + dt2.ToString("dd-MMM-yyyy") + access_sql.Hash+") AND ((ITEMTAX.Tax_Rate)<>0))";

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
            
            str = "SELECT Sum(" + access_sql.fnstring("[TaxSr]=1", "[Taxable]", "0") + ") AS TurnOver FROM ((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN ITEMTAX ON (VOUCHERDET.Itemsr = ITEMTAX.Itemsr) AND (VOUCHERDET.Vi_id = ITEMTAX.Vi_id)";
            str += " WHERE (((VOUCHERTYPE.Short)='" + shrt + "') AND ((VOUCHERINFO.Vdate)>="+access_sql.Hash + dt1.ToString("dd-MMM-yyyy") + access_sql.Hash+" And (VOUCHERINFO.Vdate)<="+access_sql.Hash + dt2.ToString("dd-MMM-yyyy") + access_sql.Hash+") AND ((ITEMTAX.Tax_Rate)=0))";

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
            str = "SELECT Sum(" + access_sql.fnstring("[TaxSr]=1", "[Taxable]", "0") + ") AS TurnOver FROM ((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN ITEMTAX ON (VOUCHERDET.Itemsr = ITEMTAX.Itemsr) AND (VOUCHERDET.Vi_id = ITEMTAX.Vi_id)";
            str += " WHERE (((VOUCHERTYPE.Short)='" + shrt + "') AND ((VOUCHERINFO.Vdate)>="+access_sql.Hash + dt1.ToString("dd-MMM-yyyy") + access_sql.Hash+" And (VOUCHERINFO.Vdate)<="+access_sql.Hash + dt2.ToString("dd-MMM-yyyy") + access_sql.Hash+"))";
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
            str += " WHERE (VOUCHERTYPE.Short='" + shrt + "' AND (VOUCHERINFO.Vdate>="+access_sql.Hash + dt1.ToString("dd-MMM-yyyy") + access_sql.Hash+" And VOUCHERINFO.Vdate<="+access_sql.Hash + dt2.ToString("dd-MMM-yyyy") + access_sql.Hash+") AND ITEMTAX.Tax_Rate<>0 AND ITEMTAX.Tax_Name = 'CST (Against Form-C)')";
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
            str = "SELECT Sum(ITEMTAX.[Taxable]) AS TurnOver FROM ((VOUCHERINFO LEFT JOIN VOUCHERTYPE ON VOUCHERINFO.Vt_id = VOUCHERTYPE.Vt_id) LEFT JOIN VOUCHERDET ON VOUCHERINFO.Vi_id = VOUCHERDET.Vi_id) LEFT JOIN ITEMTAX ON (VOUCHERDET.Itemsr = ITEMTAX.Itemsr) AND (VOUCHERDET.Vi_id = ITEMTAX.Vi_id)";
            str += " WHERE (VOUCHERTYPE.Short='" + shrt + "' AND (VOUCHERINFO.Vdate>="+access_sql.Hash + dt1.ToString("dd-MMM-yyyy") + access_sql.Hash+" And VOUCHERINFO.Vdate<="+access_sql.Hash + dt2.ToString("dd-MMM-yyyy") + access_sql.Hash+") AND ITEMTAX.Tax_Rate<>0 AND (not ITEMTAX.Tax_Name = 'CST'))";
            DataTable dtTurnOver = new DataTable();
            Database.GetSqlData(str, dtTurnOver);

            if (dtTurnOver.Rows[0][0].ToString() != "")
            {
                TurnOver = double.Parse(dtTurnOver.Rows[0][0].ToString());
            }
            return TurnOver;
        }

        private void Button2_KeyDown(object sender, KeyEventArgs e)
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
