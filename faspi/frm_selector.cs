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
    public partial class Form1 : Form
    {
        string strCombo = "";

        public string Fld1 = "";
        public string Fld2 = "";
        public string Fld3 = "";
        public string Fld4 = "";
        public string Fld5 = "";
        public string Fld6 = "";
        public string Fld7 = "";
        public string Fld8 = "";
        public string Fld9 = "";
        public string Fld10 = "";
        public string typ = "";
        public DateTime dt1;
        public DateTime dt2;
        public bool chk1,chk2;

        public bool calledindirect = false;
        public string ReportName = "";
        string grate = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select distinct '<MAIN>' as name from account union all SELECT ACCOUNT.Name as name FROM ACCOUNT LEFT JOIN ACCOUNTYPE ON ACCOUNT.Act_id = ACCOUNTYPE.Act_id WHERE ACCOUNTYPE.Name='Godown' and Account.Branch_id='" + Database.BranchId + "' GROUP BY ACCOUNT.Name";
            textBox4.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 1);
        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from other where Type='" + funs.Get_Company_id() + "' order by [name]";
            textBox10.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from other where Type='" + funs.Get_Item_id() + "' order by [name]";
            textBox11.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox13_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from other where Type='" + funs.Get_Colour_id() + "' order by [name]";
            textBox13.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "SELECT DISTINCT Pack as Packing FROM Description ORDER BY Packing";
            textBox2.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string sql = "";
            string sql1 = "";
            string sql2 = "";

            if (ReportName == "")
            {
                if (textBox3.Text == "")
                {
                    textBox3.Select();
                    return;
                }
            }

           
            if (textBox1.Text != "")
            {
                sql += " and ";
                sql += " Voucherinfo.Branch_id='" + funs.Select_branch_id(textBox1.Text) + "' ";
                sql1 += " and ";
                sql1 += " Voucherinfo_1.Branch_id='" + funs.Select_branch_id(textBox1.Text) + "' ";
                sql2 += " and ";
                sql2 += " Voucherinfo_2.Branch_id='" + funs.Select_branch_id(textBox1.Text) + "' ";
            }
            if (textBox4.Text != "")
            {                
                    sql += " and ";
                    sql1 += " and ";
                    sql2 += " and ";
                    if (funs.Select_ac_id(textBox4.Text) == "")
                    {
                        sql += " stock.Godown_id='' ";
                        sql1 += " stock_1.Godown_id='' ";
                        sql2 += " stock_2.Godown_id='' ";
                    }
                    else
                    {
                        sql += " stock.Godown_id='" + funs.Select_ac_id(textBox4.Text) + "' ";
                        sql1 += " stock_1.Godown_id='" + funs.Select_ac_id(textBox4.Text) + "' ";
                        sql2 += " stock_2.Godown_id='" + funs.Select_ac_id(textBox4.Text) + "' ";
                    }
            }
            if (textBox10.Text != "")
            {                
                sql += " and ";
                sql += " Description.Company_id='" + funs.Select_oth_id(textBox10.Text) + "' ";
                sql1 += " and ";
                sql1 += " Description_1.Company_id='" + funs.Select_oth_id(textBox10.Text) + "' ";
                sql2 += " and ";
                sql2 += " Description_2.Company_id='" + funs.Select_oth_id(textBox10.Text) + "' ";
            }
            if (textBox6.Text != "")
            {
                if (ReportName != "StockSummary")
                {
                    sql += " and ";
                    sql += " Description.Department_id='" + funs.Select_oth_id(textBox6.Text) + "' ";
                    sql1 += " and ";
                    sql1 += " Description_1.Department_id='" + funs.Select_oth_id(textBox6.Text) + "' ";
                    sql2 += " and ";
                    sql2 += " Description_2.Department_id='" + funs.Select_oth_id(textBox6.Text) + "' ";
                }
            }
            if (textBox11.Text != "")
            {                
                sql += " and ";
                sql += " Description.Item_id='" + funs.Select_oth_id(textBox11.Text) + "' ";
                sql1 += " and ";
                sql1 += " Description_1.Item_id='" + funs.Select_oth_id(textBox11.Text) + "' ";
                sql2 += " and ";
                sql2 += " Description_2.Item_id='" + funs.Select_oth_id(textBox11.Text) + "' ";
            }
            if (textBox13.Text != "")
            {
                sql += " and ";
                sql += " Description.Col_id='" + funs.Select_oth_id(textBox13.Text) + "' ";
                sql1 += " and ";
                sql1 += " Description_1.Col_id='" + funs.Select_oth_id(textBox13.Text) + "' ";
                sql2 += " and ";
                sql2 += " Description_2.Col_id='" + funs.Select_oth_id(textBox13.Text) + "' ";
            }
            if (textBox2.Text != "")
            {
                sql += " and ";
                sql += " Description.Pack='" + textBox2.Text + "' ";
                sql1 += " and ";
                sql1 += " Description_1.Pack='" + textBox2.Text + "' ";
                sql2 += " and ";
                sql2 += " Description_2.Pack='" + textBox2.Text + "' ";
            }
           
           
                    sql += " and ";
                    sql += " Vouchertype."+Database.BMode+"="+access_sql.Singlequote + "true"+access_sql.Singlequote ;
                    sql1 += " and ";
                    sql1 += " Vouchertype_1." + Database.BMode + "=" + access_sql.Singlequote + "true" + access_sql.Singlequote;
                    sql2 += " and ";
                    sql2 += " Vouchertype_2." + Database.BMode + "=" + access_sql.Singlequote + "true" + access_sql.Singlequote;
               
            
            bool amtrequired = false;


            this.Fld1 = textBox1.Text;
            this.Fld2 = textBox4.Text;
            this.Fld3 = textBox10.Text;
            this.Fld4 = textBox11.Text;
            this.Fld5 = textBox13.Text;
            this.Fld6 = textBox5.Text;
            this.Fld7 = textBox2.Text;
            this.Fld8 = textBox3.Text;
            this.Fld9 = textBox6.Text;
            this.ReportName = ReportName;
            this.dt1 = dateTimePicker1.Value;
            this.dt2 = dateTimePicker2.Value;
            this.chk1 = checkBox1.Checked;
            this.chk2 = checkBox2.Checked;
            if (calledindirect == false)
            {
                if (ReportName == "StockSummary")
                {


                    if (checkBox1.Checked == true)
                    {
                        amtrequired = true;
                    }

                    if (checkBox2.Checked == true)
                    {
                        Report gg = new Report();
                        gg.MdiParent = this.MdiParent;
                        gg.Fld1 = textBox1.Text;
                        gg.Fld2 = textBox4.Text;
                        gg.Fld3 = textBox10.Text;
                        gg.Fld4 = textBox11.Text;
                        gg.Fld5 = textBox13.Text;
                        gg.Fld6 = textBox5.Text;
                        gg.Fld7 = textBox2.Text;
                        gg.Fld8 = textBox3.Text;
                       // gg.Fld9 = textBox6.Text;
                        gg.gtype = ReportName;
                        gg.chk1 = checkBox1.Checked;
                        gg.chk2 = checkBox2.Checked;
                        gg.dt1 = dateTimePicker1.Value;
                        gg.dt2 = dateTimePicker2.Value;

                        gg.DayWiseReportCross(dateTimePicker1.Value, dateTimePicker2.Value, sql, sql2, sql1, amtrequired);
                       // gg.Show();
                    }
                    else
                    {
                        Report gg = new Report();
                        gg.MdiParent = this.MdiParent;
                        gg.Fld1 = textBox1.Text;
                        gg.Fld2 = textBox4.Text;
                        gg.Fld3 = textBox10.Text;
                        gg.Fld4 = textBox11.Text;
                        gg.Fld5 = textBox13.Text;
                        gg.Fld6 = textBox5.Text;
                        gg.Fld7 = textBox2.Text;
                        gg.Fld8 = textBox3.Text;
                       // gg.Fld9 = textBox6.Text;
                        gg.gtype = ReportName;
                        gg.chk1 = checkBox1.Checked;
                        gg.chk2 = checkBox2.Checked;
                        gg.dt1 = dateTimePicker1.Value;
                        gg.dt2 = dateTimePicker2.Value;

                        gg.DayWiseReport(dateTimePicker1.Value, dateTimePicker2.Value, sql, sql2, sql1, amtrequired);
                        gg.Show();
                    }
                }
                else
                {
                    Report gg = new Report();
                    gg.MdiParent = this.MdiParent;
                    gg.grate = grate;
                    gg.Fld1 = textBox1.Text;
                    gg.Fld2 = textBox4.Text;
                    gg.Fld3 = textBox10.Text;
                    gg.Fld4 = textBox11.Text;
                    gg.Fld5 = textBox13.Text;
                    gg.Fld6 = textBox5.Text;
                    gg.Fld7 = textBox2.Text;
                    gg.Fld8 = textBox3.Text;
                    gg.Fld9 = textBox6.Text;
                    gg.gtype = ReportName;
                    gg.chk1 = checkBox1.Checked;
                    gg.chk2 = checkBox2.Checked;
                    gg.dt1 = dateTimePicker1.Value;
                    gg.dt2 = dateTimePicker2.Value;




                    if (grate != "")
                    {
                        gg.StockValNew(dateTimePicker1.Value, dateTimePicker2.Value, sql, grate);
                    }
                    gg.Show();
                }
            }

            this.Close();
            this.Dispose();
        }

        public void DayWiseReportCross(DateTime DateFrom, DateTime DateTo, string str, string str2, string str1, bool amtrequired)
        {
            DataTable dt;
            string sql = "";


            sql = "SELECT CASE WHEN OTHER.Name IS NULL THEN '<UndefinedCompany>' ELSE OTHER.Name END AS Company, CASE WHEN OTHER_1.Name IS NULL   THEN '<UndefinedItem>' ELSE OTHER_1.Name END AS Item, Description_3.Description, Description_3.Pack, SUM(final.Opening) AS Opn, SUM(final.OpeningAmt)  AS OpnAmt, SUM(final.Purchase) AS Pur, SUM(final.PurchaseAmt) AS PurAmt, SUM(final.Sale) AS Sale, SUM(final.SaleAmt) AS SaleAmt,  SUM(final.Opening + final.Purchase - final.Sale) AS Closing, SUM(final.OpeningAmt + final.PurchaseAmt - final.SaleAmt) AS ClosingAmt, final.Did, MAX(Description_3.Pvalue) AS Pvalue FROM OTHER AS OTHER_1 RIGHT OUTER JOIN  Description AS Description_3 ON OTHER_1.Oth_id = Description_3.Item_id LEFT OUTER JOIN  OTHER ON Description_3.Company_id = OTHER.Oth_id RIGHT OUTER JOIN  (SELECT 'Opening Balance' AS Type, Did, SUM(Qty) AS Opening, SUM(Amount) AS OpeningAmt, 0 AS Purchase, 0 AS PurchaseAmt, 0 AS Sale, 0 AS SaleAmt  FROM (SELECT Stock.Did, SUM( Stock.Issue) * - 1 AS Qty, - (1 * SUM( Stock.IssueAmt)) AS Amount  FROM Description RIGHT OUTER JOIN  Stock ON Description.Des_id = Stock.Did LEFT OUTER JOIN  VOUCHERTYPE RIGHT OUTER JOIN  VOUCHERINFO ON VOUCHERTYPE.Vt_id = VOUCHERINFO.Vt_id ON Stock.Vid = VOUCHERINFO.Vi_id WHERE ( VOUCHERINFO.Vdate < '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND ( Description.StkMaintain = 'True') " + str + " GROUP BY Stock.Did  UNION ALL  SELECT Stock_2.Did, SUM(Stock_2.Receive) AS Qty, SUM(Stock_2.ReceiveAmt) AS Amount  FROM Description AS Description_2 RIGHT OUTER JOIN  Stock AS Stock_2 ON Description_2.Des_id = Stock_2.Did LEFT OUTER JOIN ";
            sql += " VOUCHERTYPE AS VOUCHERTYPE_2 RIGHT OUTER JOIN  VOUCHERINFO AS VOUCHERINFO_2 ON VOUCHERTYPE_2.Vt_id = VOUCHERINFO_2.Vt_id ON  Stock_2.Vid = VOUCHERINFO_2.Vi_id WHERE (VOUCHERINFO_2.Vdate < '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND (Description_2.StkMaintain = 'true') " + str2 + " GROUP BY Stock_2.Did) AS opn  GROUP BY Did UNION ALL  SELECT '' AS Type, Stock_1.Did, 0 AS Opening, 0 AS OpeningAmt, SUM(Stock_1.Receive) AS Purchase, SUM(Stock_1.ReceiveAmt) AS PurchaseAmt,  SUM(Stock_1.Issue) AS Sale, SUM(Stock_1.IssueAmt) AS SaleAmt  FROM Description AS Description_1 RIGHT OUTER JOIN  Stock AS Stock_1 ON Description_1.Des_id = Stock_1.Did RIGHT OUTER JOIN  VOUCHERINFO AS VOUCHERINFO_1 LEFT OUTER JOIN  VOUCHERTYPE AS VOUCHERTYPE_1 ON VOUCHERINFO_1.Vt_id = VOUCHERTYPE_1.Vt_id ON Stock_1.Vid = VOUCHERINFO_1.Vi_id WHERE (VOUCHERINFO_1.Vdate >= '" + dateTimePicker1.Value.Date.ToString(Database.dformat) + "') AND (VOUCHERINFO_1.Vdate <= '" + dateTimePicker2.Value.Date.ToString(Database.dformat) + "') AND   (Description_1.StkMaintain = 'true') " + str1 + " GROUP BY Stock_1.Did) AS final ON Description_3.Des_id = final.Did GROUP BY Description_3.Description, Description_3.Pack, final.Did, OTHER.Name, OTHER_1.Name ORDER BY OTHER.Name, OTHER_1.Name, Description_3.Description";

            
            dt = new DataTable();
            Database.GetSqlData(sql, dt);

            Object misValue = System.Reflection.Missing.Value;
            Excel.Application apl = new Microsoft.Office.Interop.Excel.Application();
            Excel.Workbook wb = (Excel.Workbook)apl.Workbooks.Add(misValue);
            Excel.Worksheet ws;
            ws = (Excel.Worksheet)wb.Worksheets[1];
            int lno = 1;
            DataTable dtExcel = new DataTable();
            DataTable dtRheader = new DataTable();
            Database.GetSqlData("select * from company", dtRheader);
            int SheetWeidht = 9;
            ws.Cells[lno, 1] = dtRheader.Rows[0]["name"].ToString();
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).Font.Bold = true;
            lno++;

            ws.Cells[lno, 1] = dtRheader.Rows[0]["Address1"].ToString();
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).Font.Bold = true;
            lno++;

            ws.Cells[lno, 1] = dtRheader.Rows[0]["Address2"].ToString();
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).Merge(Type.Missing);
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).Font.Bold = true;
            lno++;

            ws.get_Range("a:a").ColumnWidth = 30;
            ws.get_Range("b:i").ColumnWidth = 6;
            ws.get_Range("b:i").HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
            lno++;
            //ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).Merge(Type.Missing);

            DataTable distinctItem = dt.DefaultView.ToTable(true, "Company", "Item");

            for (int i = 0; i < distinctItem.Rows.Count; i++)
            {
                //Print Company and Item Heading
                ws.Cells[lno, 1] = distinctItem.Rows[i][0].ToString() + "-" + distinctItem.Rows[i][1].ToString();
                ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).Font.Bold = true;
                ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                ws.get_Range(ws.Cells[lno, 1], ws.Cells[lno, SheetWeidht]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.MediumSlateBlue);

                DataTable dt1 = dt.Select("Company='" + distinctItem.Rows[i][0].ToString() + "'  and Item='" + distinctItem.Rows[i][1].ToString() + "' ").CopyToDataTable();
                DataTable distinctDesc = dt1.DefaultView.ToTable(true, "Description");

                DataTable distinctPack = dt1.DefaultView.ToTable(true, "Pack");
                distinctPack.Columns.Add("Pvalue", typeof(double));

                for (int j = 0; j < distinctPack.Rows.Count; j++)
                {
                    distinctPack.Rows[j]["Pvalue"] = dt1.Compute("max(Pvalue)", "Pack='" + distinctPack.Rows[j]["Pack"].ToString() + "'");
                }

                DataView view = distinctPack.DefaultView;
                view.Sort = "Pvalue DESC";
                distinctPack = view.ToTable();


                for (int j = 0; j < distinctPack.Rows.Count; j++)
                {
                    //Print Avalable Pack Size
                    ws.Cells[lno, j + 2] = distinctPack.Rows[j]["Pack"].ToString();
                }



                lno++;
                for (int j = 0; j < distinctDesc.Rows.Count; j++)
                {
                    DataTable dt2 = dt1.Select("Description='" + distinctDesc.Rows[j][0].ToString() + "'").CopyToDataTable();

                    //Print Description
                    ws.Cells[lno, 1] = distinctDesc.Rows[j]["Description"].ToString();

                    for (int k = 0; k < distinctPack.Rows.Count; k++)
                    {
                        //print Stock
                        if (dt2.Select("Pack='" + distinctPack.Rows[k][0].ToString() + "'").Length > 0)
                        {
                            ws.Cells[lno, k + 2] = dt2.Select("Pack='" + distinctPack.Rows[k][0].ToString() + "'")[0]["Closing"].ToString();
                        }
                    }

                    lno++;
                }

                for (int j = 0; j < distinctPack.Rows.Count; j++)
                {
                    //Print Sum
                    ws.Cells[lno, j + 2] = dt1.Compute("sum(Closing)", "Pack='" + distinctPack.Rows[j][0].ToString() + "'");
                }
                ws.get_Range(ws.Cells[lno, 2], ws.Cells[lno, SheetWeidht]).Font.Bold = true;
                //ws.get_Range(ws.Cells[lno, 2], ws.Cells[lno, SheetWeidht]).Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                ws.get_Range(ws.Cells[lno, 2], ws.Cells[lno, SheetWeidht]).Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue);

                lno++;
            }

            Excel.Range last = ws.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            ws.get_Range("A1", last).WrapText = true;
            apl.Visible = true;
        }

       

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "SELECT Bname as BranchName from Branch order by Bname";
            textBox1.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from other where Type='" + funs.Get_Group_id()+ "' order by [name]";
            textBox5.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker2_KeyDown(object sender, KeyEventArgs e)
        {
            SelectCombo.IsEnter(this, e.KeyCode);
        }

        private void dateTimePicker1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(dateTimePicker1);
        }

        private void dateTimePicker1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(dateTimePicker1);
        }

        private void dateTimePicker2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(dateTimePicker2);
        }

        private void dateTimePicker2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(dateTimePicker2);
        }

       
        private void textBox1_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox1);
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox4);
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox4);
        }

        private void textBox10_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox10);
        }

        private void textBox10_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox10);
        }

        private void textBox11_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox11);
        }

        private void textBox11_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox11);
        }

        private void textBox13_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox13);
        }

        private void textBox13_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox13);
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox5);
        }

        private void textBox5_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox5);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox2);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox2);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = Database.cmonthFst;
            dateTimePicker1.MaxDate = Database.ldate;
            dateTimePicker1.MinDate = Database.stDate;
            dateTimePicker1.CustomFormat = Database.dformat;
            dateTimePicker2.Value = Database.ldate;
            dateTimePicker2.MaxDate = Database.ldate;
            dateTimePicker2.MinDate = Database.stDate;
            dateTimePicker2.CustomFormat = Database.dformat;

            if (ReportName == "StockSummary")
            {
                this.Text = "Stock Summary";
                label1.Visible = false;
                textBox3.Visible = false;
            }
            else
            {
                if (Feature.Available("Required Department").ToUpper() == "YES")
                {
                    label2.Enabled = true;
                    textBox6.Enabled = true;
                }
                checkBox1.Visible = false;
                checkBox2.Visible = false;
                this.Text = "Stock Valuation";
            }


            textBox1.Text = this.Fld1;
            textBox4.Text = this.Fld2;
            textBox10.Text = this.Fld3;
            textBox11.Text = this.Fld4;
            textBox13.Text = this.Fld5;
            textBox5.Text = this.Fld6;
            textBox2.Text = this.Fld7;
            textBox3.Text = this.Fld8;

            if (calledindirect == true)
            {
                dateTimePicker1.Value = DateTime.Parse(dt1.ToString(Database.dformat));

                dateTimePicker2.Value = DateTime.Parse(dt2.ToString(Database.dformat));

            }
           
            checkBox1.Checked = chk1;

            checkBox2.Checked = chk2;


        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            Database.lostFocus(textBox1);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataTable DtRates = new DataTable();
            DtRates.Columns.Add("RateValue", typeof(string));
            DtRates.Columns.Add("RateId", typeof(string));


            DtRates.Rows.Add();
            DtRates.Rows[0][0] = faspi.Feature.Available("Name of PriceList1");
            DtRates.Rows[0][1] = "Purchase_rate";


            DtRates.Rows.Add();
            DtRates.Rows[1][0] = faspi.Feature.Available("Name of PriceList2");
            DtRates.Rows[1][1] = "Retail";


            DtRates.Rows.Add();
            DtRates.Rows[2][0] = faspi.Feature.Available("Name of PriceList3");
            DtRates.Rows[2][1] = "Wholesale";

            DtRates.Rows.Add();
            DtRates.Rows[3][0] = faspi.Feature.Available("Name of PriceList4");
            DtRates.Rows[3][1] = "Rate_X";

            DtRates.Rows.Add();
            DtRates.Rows[4][0] = faspi.Feature.Available("Name of PriceList5");
            DtRates.Rows[4][1] = "Rate_Y";

            DtRates.Rows.Add();
            DtRates.Rows[5][0] = faspi.Feature.Available("Name of PriceList6");
            DtRates.Rows[5][1] = "Rate_Z";



            DtRates.Rows.Add();
            DtRates.Rows[6][0] = "MRP";
            DtRates.Rows[6][1] = "MRP";

            DtRates.Rows.Add();
            DtRates.Rows[7][0] = "Last Purchase Rate";
            DtRates.Rows[7][1] = "Last Purchase Rate";


            string rate = "";
            rate = SelectCombo.ComboDt(this, DtRates, 0);
            textBox3.Text = rate;

           


            if (rate == "MRP")
            {
                grate = "MRP";
            }
            else if (rate == "Last Purchase Rate")
            {
                grate = "Last Purchase Rate";
            }
            else
            {
                grate = funs.Select_Rates_Id(rate);
            }



        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            strCombo = "select [name] from other where Type='" + funs.Get_Department_id() + "' order by [name]";
            textBox6.Text = SelectCombo.ComboKeypress(this, e.KeyChar, strCombo, e.KeyChar.ToString(), 0);
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void textBox6_Layout(object sender, LayoutEventArgs e)
        {
            Database.lostFocus(textBox6);
        }

        private void textBox6_Enter(object sender, EventArgs e)
        {
            Database.setFocus(textBox6);
        }
    }
}
